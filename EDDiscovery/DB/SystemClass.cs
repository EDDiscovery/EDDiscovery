using EDDiscovery2;
using EDDiscovery2.DB;
using EMK.LightGeometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.DB
{
    public enum SystemStatusEnum                // Who made the information?
    {
        Unknown = 0,
        EDSC = 1,
        RedWizzard = 2,
        EDDiscovery = 3,
        EDDB = 4,
        Inhumierer = 5,

    }

    [DebuggerDisplay("System {name} ({x,nq},{y,nq},{z,nq})")]
    public class SystemClass : EDDiscovery2.DB.InMemory.SystemClass
    {
        private static List<List<long>> EdsmIdArray = new List<List<long>>();
        private static Dictionary<string, long> SystemNameToEdsmIdTable = new Dictionary<string, long>(StringComparer.InvariantCultureIgnoreCase);

        public SystemClass()
        {
        }


        public SystemClass(string Name)
        {
            name = Name;
            SearchName = Name.ToLower();
            status = SystemStatusEnum.Unknown;
            x = double.NaN;
            y = double.NaN;
            z = double.NaN;
        }

        public SystemClass(string Name, double vx, double vy, double vz)
        {
            name = Name;
            SearchName = Name.ToLower();
            status = SystemStatusEnum.Unknown;
            x = vx; y = vy; z = vz;
        }

        public SystemClass(JObject jo, EDDiscovery2.DB.SystemInfoSource source)
        {
            try
            {
                if (source == EDDiscovery2.DB.SystemInfoSource.RW)
                {
                    try
                    {
                        x = jo["x"].Value<double>();
                        y = jo["y"].Value<double>();
                        z = jo["z"].Value<double>();

                        name = jo["name"].Value<string>();
                        SearchName = name.ToLower();

                        cr = 1;
                        status = SystemStatusEnum.RedWizzard;
                    }
                    catch
                    {
                    }
                }
                else if (source == EDDiscovery2.DB.SystemInfoSource.EDSC)
                {
                    JArray ja = (JArray)jo["coord"];

                    name = jo["name"].Value<string>();
                    SearchName = name.ToLower();

                    cr = jo["cr"].Value<int>();

                    if (ja[0].Type == JTokenType.Float || ja[0].Type == JTokenType.Integer)
                    {
                        x = ja[0].Value<double>();
                        y = ja[1].Value<double>();
                        z = ja[2].Value<double>();
                    }
                    else
                    {
                        x = double.NaN;
                        y = double.NaN;
                        z = double.NaN;
                    }


                    CommanderCreate = jo["commandercreate"].Value<string>();
                    CreateDate = jo["createdate"].Value<DateTime>();
                    CommanderUpdate = jo["commanderupdate"].Value<string>();
                    UpdateDate = jo["updatedate"].Value<DateTime>();
                    status = SystemStatusEnum.EDSC;
                }
                else if (source == EDDiscovery2.DB.SystemInfoSource.EDSM)
                {
                    JObject coords = (JObject)jo["coords"];

                    name = jo["name"].Value<string>();
                    SearchName = name.ToLower();

                    //cr = jo["cr"].Value<int>();
                    x = double.NaN;
                    y = double.NaN;
                    z = double.NaN;

                    if (coords != null && (coords["x"].Type == JTokenType.Float || coords["x"].Type == JTokenType.Integer))
                    {
                        x = coords["x"].Value<double>();
                        y = coords["y"].Value<double>();
                        z = coords["z"].Value<double>();
                    }
                    JArray submitted = (JArray)jo["submitted"];

                    if (submitted != null && submitted.Count > 0)
                    {
                        if (submitted[0]["cmdrname"] != null)
                            CommanderCreate = submitted[0]["cmdrname"].Value<string>();
                        CreateDate = submitted[0]["date"].Value<DateTime>();

                        if (submitted[submitted.Count - 1]["cmdrname"] != null)
                            CommanderUpdate = submitted[submitted.Count - 1]["cmdrname"].Value<string>();
                        UpdateDate = submitted[submitted.Count - 1]["date"].Value<DateTime>();

                    }

                    UpdateDate = jo["date"].Value<DateTime>();
                    if (CreateDate.Year <= 1)
                        CreateDate = UpdateDate;

                    id_edsm = (long)jo["id"];                         // pick up its edsm ID

                    status = SystemStatusEnum.EDSC;
                }
                else if (source == EDDiscovery2.DB.SystemInfoSource.EDDB)
                {
                    name = jo["name"].Value<string>();
                    SearchName = name.ToLower();

                    cr = 1;

                    x = jo["x"].Value<double>();
                    y = jo["y"].Value<double>();
                    z = jo["z"].Value<double>();

                    id_eddb = jo["id"].Value<int>();

                    faction = jo["faction"].Value<string>();

                    if (jo["population"].Type == JTokenType.Integer)
                        population = jo["population"].Value<long>();

                    government = EliteDangerousClass.Government2ID(jo["government"]);
                    allegiance = EliteDangerousClass.Allegiance2ID(jo["allegiance"]);

                    state = EliteDangerousClass.EDState2ID(jo["state"]);
                    security = EliteDangerousClass.EDSecurity2ID(jo["security"]);

                    primary_economy = EliteDangerousClass.EDEconomy2ID(jo["primary_economy"]);

                    if (jo["needs_permit"].Type == JTokenType.Integer)
                        needs_permit = jo["needs_permit"].Value<int>();

                    eddb_updated_at = jo["updated_at"].Value<int>();

                    id_edsm = (long)jo["edsm_id"];                         // pick up its edsm ID

                    status = SystemStatusEnum.EDDB;
                }
            }
            catch { }           // since we don't have control of outside formats, we fail quietly.
        }

        public enum SystemIDType { id, EdsmId, EddbId };       // which ID to match?

        public static void CacheSystemNames()
        {

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                using (DbCommand cmd = cn.CreateCommand("SELECT Name, EdsmId FROM SystemNames"))
                {
                    using (DbDataReader rdr = cmd.ExecuteReader())
                    {
                        SystemNameToEdsmIdTable.Clear();
                        while (rdr.Read())
                        {
                            string name = String.Intern((string)rdr["Name"]);
                            long edsmid = (long)rdr["EdsmId"];

                            if (SystemNameToEdsmIdTable.ContainsKey(name))
                            {
                                long index = SystemNameToEdsmIdTable[name];

                                if (index < 0)
                                {
                                    index = -index;
                                }
                                else
                                {
                                    index = EdsmIdArray.Count;
                                    EdsmIdArray.Add(new List<long>());
                                    SystemNameToEdsmIdTable[name] = -index;
                                }

                                EdsmIdArray[(int)index].Add(SystemNameToEdsmIdTable[name]);
                            }
                            else
                            {
                                SystemNameToEdsmIdTable[name] = edsmid;
                            }
                        }
                    }
                }
            }
        }

        public static long[] GetEdsmIdsFromName(string name)
        {
            lock (SystemNameToEdsmIdTable)
            {
                if (SystemNameToEdsmIdTable.Count == 0)
                {
                    CacheSystemNames();
                }
            }

            if (SystemNameToEdsmIdTable.ContainsKey(name.ToLowerInvariant()))
            {
                long index = SystemNameToEdsmIdTable[name.ToLowerInvariant()];

                if (index >= 0)
                {
                    return new long[] { index };
                }
                else
                {
                    return EdsmIdArray[(int)(-index)].ToArray();
                }
            }
            else
            {
                return new long[0];
            }
        }

        public static double Distance(EDDiscovery2.DB.ISystem s1, EDDiscovery2.DB.ISystem s2)
        {
            if (s1 != null && s2 != null && s1.HasCoordinate && s2.HasCoordinate)
                return Math.Sqrt((s1.x - s2.x) * (s1.x - s2.x) + (s1.y - s2.y) * (s1.y - s2.y) + (s1.z - s2.z) * (s1.z - s2.z));
            else
                return -1;
        }

        public static double Distance(EDDiscovery2.DB.ISystem s1, double x, double y, double z)
        {
            if (s1 != null && s1.HasCoordinate)
                return Math.Sqrt((s1.x - x) * (s1.x - x) + (s1.y - y) * (s1.y - y) + (s1.z - z) * (s1.z - z));
            else
                return -1;
        }

        public static double DistanceIncludeDB(EDDiscovery2.DB.ISystem s1, EDDiscovery2.DB.ISystem s2)
        {
            double dist = Distance(s1, s2);
            if (dist < 0)
                dist = DistanceClass.FindDistance(s1, s2);
            return dist;
        }

        public enum SystemAskType { AnyStars, PopulatedStars, UnPopulatedStars };
        public static int GetSystemVector(int gridid, ref Vector3[] vertices, ref uint[] colours, 
                                            SystemAskType ask, int percentage )
        {
            int numvertices = 0;

            vertices = null;
            colours = null;

            Color[] fixedc = new Color[4];
            fixedc[0] = Color.Red;
            fixedc[1] = Color.Orange;
            fixedc[2] = Color.Yellow;
            fixedc[3] = Color.White;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT id,x,y,z,randomid from EdsmSystems where gridid=@gridid"))
                    {
                        cmd.AddParameterWithValue("gridid", gridid);

                        if (ask == SystemAskType.PopulatedStars)
                            cmd.CommandText += " AND (EddbId IS NOT NULL AND EddbId <> 0)";
                        else if (ask == SystemAskType.UnPopulatedStars)
                            cmd.CommandText += " AND (EddbId IS NULL OR EddbId = 0)";

                        if (percentage < 100)
                            cmd.CommandText += " and randomid<" + percentage;

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (System.DBNull.Value != reader["x"])
                                {
                                    if (vertices == null)
                                    {
                                        vertices = new Vector3[1024];
                                        colours = new uint[1024];
                                    }
                                    else if (numvertices == vertices.Length)
                                    {
                                        Array.Resize(ref vertices, vertices.Length + 8192);
                                        Array.Resize(ref colours, colours.Length + 8192);
                                    }

                                    Vector3 pos = new Vector3((float)((long)reader["x"] / 128.0), (float)((long)reader["y"] / 128.0), (float)((long)reader["z"] / 128.0));

                                    int rand = (int)(long)reader["randomid"];
                                    Color basec = fixedc[rand&3]; 
                                    int fade = 100 - ((rand>>2)&7) * 8;
                                    byte red = (byte)(basec.R * fade / 100);
                                    byte green = (byte)(basec.G * fade / 100);
                                    byte blue = (byte)(basec.B * fade / 100);
                                    colours[numvertices] = BitConverter.ToUInt32(new byte[] { red, green, blue, 255 }, 0);
                                    vertices[numvertices++] = pos;
                                }
                            }
                        }

                        if (gridid == 810 && vertices!=null)    // BODGE do here, better once on here than every star for every grid..
                        {                       // replace when we have a better naming system
                            int solindex = Array.IndexOf(vertices, new Vector3(0, 0, 0));

                            if (solindex >= 0)
                                colours[solindex] = 0x00ffff;   //yellow
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return numvertices;
        }

        public static void GetSystemNames(ref AutoCompleteStringCollection asc)
        {
            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("select name from SystemNames"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                asc.Add(String.Intern((string)reader["name"]));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
        }


        public static List<Point3D> GetStarPositions()  // return star positions..
        {
            List<Point3D> list = new List<Point3D>();

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("select x,y,z from EdsmSystems"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (System.DBNull.Value != reader["x"])
                                    list.Add(new Point3D((double)((long)reader["x"] / 128.0), (double)((long)reader["y"] / 128.0), (double)((long)reader["z"] / 128.0)));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
            return list;
        }

        public static Dictionary<string, int> GetSystemNamesUpperCase()  // return a dictionary, in upper case, id is the row ID in the table, duplicates ignored.
        {
            Dictionary<string, int> dict = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("select name from SystemNames"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = ((string)reader["name"]).ToUpper();
                                if (!dict.ContainsKey(name))
                                    dict.Add(name, (int)reader["id"]);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return dict;
        }

        public static SystemClass GetSystem(string name, SQLiteConnectionSystem cn = null)      // with an open database, case insensitive
        {
            return GetSystemsByName(name, cn).FirstOrDefault();
        }

        public static List<SystemClass> GetSystemsByName(string name, SQLiteConnectionSystem cn = null)
        {
            List<SystemClass> systems = new List<SystemClass>();

            foreach (long edsmid in GetEdsmIdsFromName(name))
            {
                systems.Add(GetSystem(edsmid, cn, SystemIDType.EdsmId));
            }

            return systems;
        }

        public static SystemClass GetSystem(long id, SQLiteConnectionSystem cn = null, SystemIDType idtype = SystemIDType.id)      // using an id
        {
            SystemClass sys = null;
            bool closeit = false;

            try
            {
                if (cn == null)
                {
                    closeit = true;
                    cn = new SQLiteConnectionSystem();
                }

                using (DbCommand cmd = cn.CreateCommand("SELECT * FROM EdsmSystems WHERE " + idtype.ToString() + "=@id LIMIT 1"))   // 1 return matching name
                {
                    cmd.AddParameterWithValue("id", id);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            long edsmid = (long)reader["EdsmId"];

                            sys = new SystemClass
                            {
                                id = (long)reader["id"],
                                id_edsm = (long)reader["EdsmId"],
                                id_eddb = reader["EddbId"] == System.DBNull.Value ? 0 : (long)reader["EddbId"],
                                CreateDate = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds((long)reader["CreateTimestamp"]),
                                UpdateDate = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds((long)reader["UpdateTimestamp"]),
                                cr = 0,
                                status = SystemStatusEnum.EDSC,
                                gridid = (int)(long)reader["GridId"],
                                randomid = (int)(long)reader["RandomId"]
                            };

                            if (System.DBNull.Value == reader["x"])
                            {
                                sys.x = double.NaN;
                                sys.y = double.NaN;
                                sys.z = double.NaN;
                            }
                            else
                            {
                                sys.x = (double)((long)reader["x"] / 128.0);
                                sys.y = (double)((long)reader["y"] / 128.0);
                                sys.z = (double)((long)reader["z"] / 128.0);
                            }
                        }
                    }
                }

                if (sys != null && sys.id_edsm != 0)
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT Name FROM SystemNames WHERE EdsmId = @EdsmId LIMIT 1"))
                    {
                        cmd.AddParameterWithValue("@EdsmId", sys.id_edsm);
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                sys.name = (string)reader["Name"];
                            }
                        }
                    }
                }

                if (sys != null && sys.id_eddb != 0)
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT * FROM EddbSystems WHERE EddbId = @EddbId LIMIT 1"))
                    {
                        cmd.AddParameterWithValue("EddbId", sys.id_eddb);
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                object o;

                                o = reader["Population"];
                                sys.population = o == DBNull.Value ? 0 : (long)o;

                                o = reader["Faction"];
                                sys.faction = o == DBNull.Value ? null : (string)o;

                                o = reader["GovernmentId"];
                                sys.government = o == DBNull.Value ? EDGovernment.Unknown : (EDGovernment)((long)o);

                                o = reader["AllegianceId"];
                                sys.allegiance = o == DBNull.Value ? EDAllegiance.Unknown : (EDAllegiance)((long)o);

                                o = reader["PrimaryEconomyId"];
                                sys.primary_economy = o == DBNull.Value ? EDEconomy.Unknown : (EDEconomy)((long)o);

                                o = reader["Security"];
                                sys.security = o == DBNull.Value ? EDSecurity.Unknown : (EDSecurity)((long)o);

                                o = reader["EddbUpdatedAt"];
                                sys.eddb_updated_at = o == DBNull.Value ? 0 : (int)((long)o);

                                o = reader["State"];
                                sys.state = o == DBNull.Value ? EDState.Unknown : (EDState)((long)o);

                                o = reader["NeedsPermit"];
                                sys.needs_permit = o == DBNull.Value ? 0 : (int)((long)o);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (closeit && cn != null)
                {
                    cn.Dispose();
                }
            }

            return sys;
        }

        public static long GetTotalSystems()
        {
            long value = 0;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("select Count(*) from EdsmSystems"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                value = (long)reader["Count(*)"];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return value;
        }

        public static DateTime GetLastSystemEntryTime()
        {
            DateTime lasttime = new DateTime(2010, 1, 1, 0, 0, 0);

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT VersionTimestamp FROM EdsmSystems ORDER BY VersionTimestamp DESC LIMIT 1"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && System.DBNull.Value != reader["VersionTimestamp"])
                                lasttime = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds((long)reader["VersionTimestamp"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return lasttime;
        }

        public static DateTime GetLastSystemModifiedTime()
        {
            DateTime lasttime = new DateTime(2010, 1, 1, 0, 0, 0);

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT UpdateTimestamp FROM EdsmSystems ORDER BY UpdateTimestamp DESC LIMIT 1"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && System.DBNull.Value != reader["UpdateTimestamp"])
                                lasttime = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds((long)reader["UpdateTimestamp"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return lasttime;
        }

        public static void TouchSystem(SQLiteConnectionSystem cn, string systemName)
        {
            foreach (long edsmid in GetEdsmIdsFromName(systemName))
            {
                using (DbCommand cmd = cn.CreateCommand("UPDATE EdsmSystems SET VersionTimestamp = @VersionTimestamp where EdsmId=@EdsmId"))
                {
                    cmd.AddParameterWithValue("@EdsmId", edsmid);
                    cmd.AddParameterWithValue("@VersionTimestamp", DateTime.UtcNow.Subtract(new DateTime(2015, 1, 1)).TotalSeconds);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void GetSystemSqDistancesFrom(SortedList<double, ISystem> distlist, double x, double y, double z, int maxitems, bool removezerodiststar, 
                                                    double maxdist = 200 , SQLiteConnectionSystem cn = null)
        {
            bool closeit = false;

            try
            {
                if (cn == null)
                {
                    closeit = true;
                    cn = new SQLiteConnectionSystem();
                }

                using (DbCommand cmd = cn.CreateCommand(
                    "SELECT EdsmId, x, y, z " +
                    "FROM EdsmSystems " +
                    "WHERE x >= @xv - @maxdist " +
                    "AND x <= @xv + @maxdist " +
                    "AND y >= @yv - @maxdist " +
                    "AND y <= @yv + @maxdist " +
                    "AND z >= @zv - @maxdist " +
                    "AND z <= @zv + @maxdist " +
                    "ORDER BY (x-@xv)*(x-@xv)+(y-@yv)*(y-@yv)+(z-@zv)*(z-@zv) " +
                    "LIMIT @max"))
                {
                    cmd.AddParameterWithValue("xv", x * 128);
                    cmd.AddParameterWithValue("yv", y * 128);
                    cmd.AddParameterWithValue("zv", z * 128);
                    cmd.AddParameterWithValue("max", maxitems + 1);     // 1 more, because if we are on a star, that will be returned
                    cmd.AddParameterWithValue("maxdist", maxdist * 128);

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() && distlist.Count < maxitems)           // already sorted, and already limited to max items
                        {
                            long edsmid = (long)reader["EdsmId"];

                            if (System.DBNull.Value != reader["x"])                 // paranoid check for null
                            {
                                double dx = (double)((long)reader["x"] / 128.0) - x;
                                double dy = (double)((long)reader["y"] / 128.0) - y;
                                double dz = (double)((long)reader["z"] / 128.0) - z;

                                double dist = dx * dx + dy * dy + dz * dz;
                                if (dist > 0.001 || !removezerodiststar)
                                    distlist.Add(dist, GetSystem(edsmid, cn, SystemIDType.EdsmId));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
            finally
            {
                if (closeit && cn != null)
                {
                    cn.Dispose();
                }
            }
        }

        public static ISystem FindNearestSystem(double x, double y, double z, bool removezerodiststar = false, double maxdist = 1000, SQLiteConnectionSystem cn = null)
        {
            SortedList<double, ISystem> distlist = new SortedList<double, ISystem>();
            GetSystemSqDistancesFrom(distlist, x, y, z, 1, removezerodiststar, maxdist,cn);
            return distlist.Select(v => v.Value).FirstOrDefault();
        }

        public const int metric_nearestwaypoint = 0;     // easiest way to synchronise metric selection..
        public const int metric_mindevfrompath = 1;
        public const int metric_maximum100ly = 2;
        public const int metric_maximum250ly = 3;
        public const int metric_maximum500ly = 4;
        public const int metric_waypointdev2 = 5;

        public static SystemClass GetSystemNearestTo(Point3D curpos, Point3D wantedpos, double maxfromcurpos, double maxfromwanted,
                                    int routemethod)
        {
            SystemClass nearestsystem = null;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    string sqlquery = "SELECT EdsmId, x, y, z " +
                                      "FROM EdsmSystems " +
                                      "WHERE x >= @xc - @maxfromcurpos " +
                                      "AND x <= @xc + @maxfromcurpos " +
                                      "AND y >= @yc - @maxfromcurpos " +
                                      "AND y <= @yc + @maxfromcurpos " +
                                      "AND z >= @zc - @maxfromcurpos " +
                                      "AND z <= @zc + @maxfromcurpos " +
                                      "AND x >= @xw - @maxfromwanted " +
                                      "AND x <= @xw + @maxfromwanted " +
                                      "AND y >= @yw - @maxfromwanted " +
                                      "AND y <= @yw + @maxfromwanted " +
                                      "AND z >= @zw - @maxfromwanted " +
                                      "AND z <= @zw + @maxfromwanted ";

                    using (DbCommand cmd = cn.CreateCommand(sqlquery))
                    {
                        cmd.AddParameterWithValue("@xw", wantedpos.X);
                        cmd.AddParameterWithValue("@yw", wantedpos.Y);
                        cmd.AddParameterWithValue("@zw", wantedpos.Z);
                        cmd.AddParameterWithValue("@maxfromwanted", maxfromwanted * maxfromwanted);     //squared

                        cmd.AddParameterWithValue("@xc", curpos.X);
                        cmd.AddParameterWithValue("@yc", curpos.Y);
                        cmd.AddParameterWithValue("@zc", curpos.Z);
                        cmd.AddParameterWithValue("@maxfromcurrent", maxfromcurpos * maxfromcurpos);     //squared

                        double bestmindistance = double.MaxValue;

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = (string)reader["name"];
                                long id = (long)reader["id"];

                                if (System.DBNull.Value != reader["x"]) // paranoid check, it could be null in db
                                {

                                    Point3D syspos = new Point3D((double)reader["x"], (double)reader["y"], (double)reader["z"]);

                                    double distancefromwantedx2 = Point3D.DistanceBetweenX2(wantedpos, syspos); // range between the wanted point and this, ^2
                                    double distancefromcurposx2 = Point3D.DistanceBetweenX2(curpos, syspos);    // range between the wanted point and this, ^2

                                    if (routemethod == metric_nearestwaypoint)
                                    {
                                        if (distancefromwantedx2 < bestmindistance)
                                        {
                                            nearestsystem = GetSystem(id);
                                            bestmindistance = distancefromwantedx2;
                                        }
                                    }
                                    else
                                    {
                                        Point3D interceptpoint = curpos.InterceptPoint(wantedpos, syspos);      // work out where the perp. intercept point is..
                                        double deviation = Point3D.DistanceBetween(interceptpoint, syspos);
                                        double metric = 1E39;

                                        if (routemethod == metric_mindevfrompath)
                                            metric = deviation;
                                        else if (routemethod == metric_maximum100ly)
                                            metric = (deviation <= 100) ? distancefromwantedx2 : metric;        // no need to sqrt it..
                                        else if (routemethod == metric_maximum250ly)
                                            metric = (deviation <= 250) ? distancefromwantedx2 : metric;
                                        else if (routemethod == metric_maximum500ly)
                                            metric = (deviation <= 500) ? distancefromwantedx2 : metric;
                                        else if (routemethod == metric_waypointdev2)
                                            metric = Math.Sqrt(distancefromwantedx2) + deviation / 2;

                                        if (metric < bestmindistance)
                                        {
                                            nearestsystem = GetSystem(id);
                                            bestmindistance = metric;
                                            //Console.WriteLine("System " + syscheck.name + " way " + deviation.ToString("0.0") + " metric " + metric.ToString("0.0") + " *");
                                        }
                                        else
                                        {
                                            //Console.WriteLine("System " + syscheck.name + " way " + deviation.ToString("0.0") + " metric " + metric.ToString("0.0"));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return nearestsystem;
        }

        public static void FillVisitedSystems(List<VisitedSystemsClass> visitedSystems)
        {
            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    // Check that the SystemAliases table is not empty
                    using (DbCommand cmd = cn.CreateCommand("SELECT COUNT(id) FROM SystemAliases"))
                    {
                        long nrows = (long)cmd.ExecuteScalar();

                        if (nrows == 0)
                        {
                            Console.WriteLine("Populating system aliases table");
                            RemoveHiddenSystems();
                        }
                    }

                    Dictionary<string, List<long>> aliasesByName = new Dictionary<string, List<long>>(StringComparer.InvariantCultureIgnoreCase);
                    Dictionary<long, long> aliasesById = new Dictionary<long, long>();

                    using (DbCommand cmd = cn.CreateCommand("SELECT name, id_edsm, id_edsm_mergedto FROM SystemAliases"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = (string)reader["name"];
                                long edsmid = (long)reader["id_edsm"];
                                long mergedto = (long)reader["id_edsm_mergedto"];
                                if (!aliasesByName.ContainsKey(name))
                                {
                                    aliasesByName[name] = new List<long>();
                                }
                                aliasesByName[name].Add(mergedto);
                                aliasesById[edsmid] = mergedto;
                            }
                        }
                    }

                    using (DbCommand selectByPosCmd = cn.CreateCommand(
                            "SELECT s.EdsmId FROM EdsmSystems s " +
                            "WHERE s.X >= @X - 0.125 " +
                            "AND s.X <= @X + 0.125 " +
                            "AND s.Y >= @Y - 0.125 " +
                            "AND s.Y <= @Y + 0.125 " +
                            "AND s.Z >= @Z - 0.125 " +
                            "AND s.Z <= @Z + 0.125"))
                    {
                        selectByPosCmd.AddParameter("@X", DbType.Int64);
                        selectByPosCmd.AddParameter("@Y", DbType.Int64);
                        selectByPosCmd.AddParameter("@Z", DbType.Int64);

                        foreach (VisitedSystemsClass vsc in visitedSystems)
                        {
                            SystemClass[] systemsByName = GetSystemsByName(vsc.Name, cn).ToArray();

                            if (vsc.curSystem == null || vsc.curSystem.id_edsm == 0)                                              // if not set before, look it up
                            {
                                Dictionary<long, SystemClass> altmatches = new Dictionary<long, SystemClass>();
                                Dictionary<long, SystemClass> matches = new Dictionary<long, SystemClass>();
                                SystemClass edsmidmatch = null;
                                long sel_edsmid = vsc.id_edsm_assigned ?? 0;
                                bool hastravcoords = vsc.HasTravelCoordinates && (vsc.Name.ToLowerInvariant() == "sol" || vsc.X != 0 || vsc.Y != 0 || vsc.Z != 0);
                                bool multimatch = false;

                                if (sel_edsmid != 0)
                                {
                                    edsmidmatch = GetSystem(sel_edsmid, cn, SystemIDType.EdsmId);
                                    matches.Add(edsmidmatch.id, edsmidmatch);

                                    while (aliasesById.ContainsKey(sel_edsmid))
                                    {
                                        sel_edsmid = aliasesById[sel_edsmid];
                                        SystemClass sys = GetSystem(sel_edsmid, cn, SystemIDType.EdsmId);
                                        altmatches.Add(sys.id, sys);
                                        edsmidmatch = null;
                                    }
                                }

                                Dictionary<long, SystemClass> namematches = GetSystemsByName(vsc.Name).Where(s => s != null).ToDictionary(s => s.id, s => s);
                                Dictionary<long, SystemClass> posmatches = new Dictionary<long, SystemClass>();
                                Dictionary<long, SystemClass> nameposmatches = new Dictionary<long, SystemClass>();

                                if (hastravcoords)
                                {
                                    selectByPosCmd.Parameters["@X"].Value = vsc.X;
                                    selectByPosCmd.Parameters["@Y"].Value = vsc.Y;
                                    selectByPosCmd.Parameters["@Z"].Value = vsc.Z;

                                    using (DbDataReader reader = selectByPosCmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            long pos_edsmid = (long)reader["EdsmId"];
                                            SystemClass sys = GetSystem(pos_edsmid, cn, SystemIDType.EdsmId);
                                            if (sys != null)
                                            {
                                                matches[sys.id] = sys;
                                                posmatches[sys.id] = sys;

                                                if (sys.name.Equals(vsc.Name, StringComparison.InvariantCultureIgnoreCase))
                                                {
                                                    nameposmatches[sys.id] = sys;
                                                }
                                            }
                                        }
                                    }
                                }

                                if (aliasesByName.ContainsKey(vsc.Name))
                                {
                                    foreach (long alt_edsmid in aliasesByName[vsc.Name])
                                    {
                                        SystemClass sys = GetSystem(alt_edsmid, cn, SystemIDType.EdsmId);
                                        if (sys != null)
                                        {
                                            altmatches[sys.id] = sys;
                                        }
                                    }
                                }

                                foreach (var sys in namematches.Values)
                                {
                                    matches[sys.id] = sys;
                                }

                                if (altmatches.Count != 0)
                                {
                                    foreach (var alt in altmatches.Values)
                                    {
                                        matches[alt.id] = alt;
                                    }
                                }

                                vsc.alternatives = matches.Values.Select(s => (ISystem)s).ToList();

                                if (edsmidmatch != null)
                                {
                                    SystemClass sys = edsmidmatch;
                                    vsc.curSystem = sys;

                                    if (nameposmatches.ContainsKey(sys.id)) // name and position matches
                                    {
                                        vsc.NameStatus = "Exact match";
                                        continue; // Continue to next system
                                    }
                                    else if (posmatches.ContainsKey(sys.id)) // position matches
                                    {
                                        vsc.NameStatus = "Name differs";
                                        continue; // Continue to next system
                                    }
                                    else if (!hastravcoords || !sys.HasCoordinate) // no coordinates available
                                    {
                                        if (namematches.ContainsKey(sys.id)) // name matches
                                        {
                                            if (!sys.HasCoordinate)
                                            {
                                                vsc.NameStatus = "System has no known coordinates";
                                            }
                                            else
                                            {
                                                vsc.NameStatus = "Travel log entry has no coordinates";
                                            }

                                            continue; // Continue to next system
                                        }
                                        else if (!vsc.HasTravelCoordinates)
                                        {
                                            vsc.NameStatus = "Name differs";
                                        }
                                    }
                                }

                                if (nameposmatches != null && nameposmatches.Count != 0)
                                {
                                    if (nameposmatches.Count == 1)
                                    {
                                        // Both name and position matches
                                        vsc.curSystem = nameposmatches.Values.Single();
                                        vsc.NameStatus = "Exact match";
                                        continue; // Continue to next system
                                    }
                                    else if (posmatches.Count == 1)
                                    {
                                        var sys = posmatches.Values.Single();

                                        // Position matches
                                        vsc.curSystem = sys;
                                        vsc.NameStatus = $"System {sys.name} found at location";
                                        continue; // Continue to next system
                                    }
                                    else
                                    {
                                        multimatch = true;
                                    }
                                }

                                if (namematches != null && namematches.Count != 0)
                                {
                                    if (namematches.Count == 1)
                                    {
                                        // One system name matched
                                        vsc.curSystem = namematches.Values.Single();
                                        vsc.NameStatus = "Name matched";
                                        continue;
                                    }
                                    else if (namematches.Count > 1)
                                    {
                                        multimatch = true;
                                    }
                                }

                                if (multimatch)
                                {
                                    vsc.NameStatus = "Multiple system matches found";
                                }
                                else
                                {
                                    vsc.NameStatus = "System not found";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
        }

        public static long ParseEDSMUpdateSystemsString(string json, ref string date, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true)
        {
            using (StringReader sr = new StringReader(json))
                return ParseEDSMUpdateSystemsStream(sr, ref date, removenonedsmids, discoveryform, cancelRequested, reportProgress, useCache);
        }

        public static long ParseEDSMUpdateSystemsFile(string filename, ref string date, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true)
        {
            using (StreamReader sr = new StreamReader(filename))         // read directly from file..
                return ParseEDSMUpdateSystemsStream(sr, ref date, removenonedsmids, discoveryform, cancelRequested, reportProgress, useCache);
        }

        public static long ParseEDSMUpdateSystemsStream(TextReader sr, ref string date, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            using (JsonTextReader jr = new JsonTextReader(sr))
                return ParseEDSMUpdateSystemsReader(jr, ref date, removenonedsmids, discoveryform, cancelRequested, reportProgress, useCache, useTempSystems);
        }

        private static Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase> GetEdsmSystemsLite(SQLiteConnectionSystem cn)
        {
            Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase> systemsByEdsmId = new Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase>();

            using (DbCommand cmd = cn.CreateCommand("SELECT s.id, s.EdsmId, n.Name, s.x, s.y, s.z, s.UpdateTimestamp, s.gridid, s.randomid FROM EdsmSystems s JOIN SystemNames n ON n.EdsmId = s.EdsmId"))
            {
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        EDDiscovery2.DB.InMemory.SystemClassBase sys = new EDDiscovery2.DB.InMemory.SystemClassBase
                        {
                            id = (long)reader["id"],
                            name = (string)reader["name"]
                        };

                        string searchname = sys.name.ToLower();

                        if (System.DBNull.Value == reader["x"])
                        {
                            sys.x = double.NaN;
                            sys.y = double.NaN;
                            sys.z = double.NaN;
                        }
                        else
                        {
                            sys.x = (double)((long)reader["x"] / 128.0);
                            sys.y = (double)((long)reader["y"] / 128.0);
                            sys.z = (double)((long)reader["z"] / 128.0);
                        }

                        sys.id_edsm = (long)reader["EdsmId"];
                        systemsByEdsmId[sys.id_edsm] = sys;
                        sys.gridid = reader["gridid"] == DBNull.Value ? 0 : (int)((long)reader["gridid"]);
                        sys.randomid = reader["randomid"] == DBNull.Value ? 0 : (int)((long)reader["randomid"]);
                    }
                }
            }

            return systemsByEdsmId;
        }

        private static long DoParseEDSMUpdateSystemsReader(JsonTextReader jr, ref string date, SQLiteConnectionSystem cn, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            DateTime maxdate;

            if (!DateTime.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out maxdate))
            {
                maxdate = new DateTime(2010, 1, 1);
            }

            Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase> systemsByEdsmId = useCache ? GetEdsmSystemsLite(cn) : new Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase>();
            int count = 0;
            int updatecount = 0;
            int insertcount = 0;
            Random rnd = new Random();
            int[] histogramsystems = new int[50000];
            string sysnamesTableName = useTempSystems ? "SystemNames_temp" : "SystemNames";
            string edsmsysTableName = useTempSystems ? "EdsmSystems_temp" : "EdsmSystems";
            Stopwatch sw = Stopwatch.StartNew();

            while (!cancelRequested())
            {
                bool jr_eof = false;
                List<JObject> objs = new List<JObject>();

                while (!cancelRequested())
                {
                    if (jr.Read())
                    {
                        if (jr.TokenType == JsonToken.StartObject)
                        {
                            objs.Add(JObject.Load(jr));

                            if (objs.Count >= 10000)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        jr_eof = true;
                        break;
                    }
                }
                 

                using (DbTransaction txn = cn.BeginTransaction())
                {
                    DbCommand updateNameCmd = null;
                    DbCommand updateSysCmd = null;
                    DbCommand insertNameCmd = null;
                    DbCommand insertSysCmd = null;
                    DbCommand selectSysCmd = null;
                    DbCommand selectNameCmd = null;

                    try
                    {
                        updateNameCmd = cn.CreateCommand("UPDATE SystemNames SET Name=@Name WHERE EdsmId=@EdsmId", txn);
                        updateNameCmd.AddParameter("@Name", DbType.String);
                        updateNameCmd.AddParameter("@EdsmId", DbType.Int64);

                        updateSysCmd = cn.CreateCommand("UPDATE EdsmSystems SET X=@X, Y=@Y, Z=@Z, UpdateTimestamp=@UpdateTimestamp, VersionTimestamp=@VersionTimestamp, GridId=@GridId, RandomId=@RandomId WHERE EdsmId=@EdsmId", txn);
                        updateSysCmd.AddParameter("@X", DbType.Int64);
                        updateSysCmd.AddParameter("@Y", DbType.Int64);
                        updateSysCmd.AddParameter("@Z", DbType.Int64);
                        updateSysCmd.AddParameter("@UpdateTimestamp", DbType.Int64);
                        updateSysCmd.AddParameter("@VersionTimestamp", DbType.Int64);
                        updateSysCmd.AddParameter("@GridId", DbType.Int64);
                        updateSysCmd.AddParameter("@RandomId", DbType.Int64);
                        updateSysCmd.AddParameter("@EdsmId", DbType.Int64);

                        insertNameCmd = cn.CreateCommand("INSERT INTO " + sysnamesTableName + " (Name, EdsmId) VALUES (@Name, @EdsmId)", txn);
                        insertNameCmd.AddParameter("@Name", DbType.String);
                        insertNameCmd.AddParameter("@EdsmId", DbType.Int64);

                        insertSysCmd = cn.CreateCommand("INSERT INTO " + edsmsysTableName + " (EdsmId, X, Y, Z, CreateTimestamp, UpdateTimestamp, VersionTimestamp, GridId, RandomId) VALUES (@EdsmId, @X, @Y, @Z, @CreateTimestamp, @UpdateTimestamp, @VersionTimestamp, @GridId, @RandomId)", txn);
                        insertSysCmd.AddParameter("@EdsmId", DbType.Int64);
                        insertSysCmd.AddParameter("@X", DbType.Int64);
                        insertSysCmd.AddParameter("@Y", DbType.Int64);
                        insertSysCmd.AddParameter("@Z", DbType.Int64);
                        insertSysCmd.AddParameter("@CreateTimestamp", DbType.Int64);
                        insertSysCmd.AddParameter("@UpdateTimestamp", DbType.Int64);
                        insertSysCmd.AddParameter("@VersionTimestamp", DbType.Int64);
                        insertSysCmd.AddParameter("@GridId", DbType.Int64);
                        insertSysCmd.AddParameter("@RandomId", DbType.Int64);

                        selectSysCmd = cn.CreateCommand("SELECT Id, X, Y, Z, GridId, RandomId FROM EdsmSystems WHERE EdsmId=@EdsmId");
                        selectSysCmd.AddParameter("@EdsmId", DbType.Int64);

                        selectNameCmd = cn.CreateCommand("SELECT Name FROM SystemNames WHERE EdsmId = @EdsmId");
                        selectNameCmd.AddParameter("@EdsmId", DbType.Int64);

                        IEnumerator<JObject> jo_enum = objs.GetEnumerator();

                        while (!cancelRequested())
                        {
                            if (!jo_enum.MoveNext())
                            {
                                reportProgress(-1, $"Syncing EDSM systems: {count} processed, {insertcount} new systems, {updatecount} updated systems");
                                txn.Commit();

                                if (jr_eof)
                                {
                                    date = maxdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                                    Console.WriteLine($"Import took {sw.ElapsedMilliseconds}ms");

                                    for (int id = 0; id < histogramsystems.Length; id++)
                                    {
                                        if (histogramsystems[id] != 0)
                                            Console.WriteLine("Id " + id + " count " + histogramsystems[id]);
                                    }

                                    return updatecount + insertcount;
                                }

                                break;
                            }

                            JObject jo = jo_enum.Current;

                            JObject coords = (JObject)jo["coords"];

                            if (coords != null && (coords["x"].Type == JTokenType.Float || coords["x"].Type == JTokenType.Integer))
                            {
                                double x = coords["x"].Value<double>();
                                double y = coords["y"].Value<double>();
                                double z = coords["z"].Value<double>();
                                long edsmid = jo["id"].Value<long>();
                                string name = jo["name"].Value<string>();
                                int gridid = GridId.Id(x, z);
                                int randomid = rnd.Next(0, 99);
                                DateTime updatedate = jo["date"].Value<DateTime>();
                                histogramsystems[gridid]++;

                                if (updatedate > maxdate)
                                    maxdate = updatedate;

                                EDDiscovery2.DB.InMemory.SystemClassBase dbsys = null;

                                if (!useTempSystems)
                                {
                                    if (useCache && systemsByEdsmId.ContainsKey(edsmid))
                                        dbsys = systemsByEdsmId[edsmid];

                                    if (!useCache)
                                    {
                                        selectSysCmd.Parameters["@EdsmId"].Value = edsmid;
                                        using (DbDataReader reader = selectSysCmd.ExecuteReader())
                                        {
                                            if (reader.Read())
                                            {
                                                dbsys = new EDDiscovery2.DB.InMemory.SystemClassBase
                                                {
                                                    id = (long)reader["id"],
                                                    id_edsm = edsmid
                                                };

                                                if (System.DBNull.Value == reader["x"])
                                                {
                                                    dbsys.x = double.NaN;
                                                    dbsys.y = double.NaN;
                                                    dbsys.z = double.NaN;
                                                }
                                                else
                                                {
                                                    dbsys.x = (double)((long)reader["X"] / 128.0);
                                                    dbsys.y = (double)((long)reader["Y"] / 128.0);
                                                    dbsys.z = (double)((long)reader["Z"] / 128.0);
                                                }

                                                dbsys.id_edsm = edsmid;
                                                dbsys.gridid = reader["GridId"] == DBNull.Value ? 0 : (int)((long)reader["GridId"]);
                                                dbsys.randomid = reader["RandomId"] == DBNull.Value ? 0 : (int)((long)reader["RandomId"]);
                                            }
                                        }

                                        if (dbsys != null)
                                        {
                                            selectNameCmd.Parameters["@EdsmId"].Value = edsmid;
                                            using (DbDataReader reader = selectNameCmd.ExecuteReader())
                                            {
                                                if (reader.Read())
                                                {
                                                    dbsys.name = (string)reader["Name"];
                                                }
                                            }
                                        }
                                    }
                                }

                                if (dbsys != null)
                                {
                                    // see if EDSM data changed..
                                    if (!dbsys.name.Equals(name))
                                    {
                                        updateNameCmd.Parameters["Name"].Value = name;
                                        updateNameCmd.Parameters["EdsmId"].Value = edsmid;
                                        updateNameCmd.ExecuteNonQuery();
                                    }

                                    if (Math.Abs(dbsys.x - x) > 0.01 ||
                                        Math.Abs(dbsys.y - y) > 0.01 ||
                                        Math.Abs(dbsys.z - z) > 0.01 ||
                                        dbsys.gridid != gridid)  // position changed
                                    {
                                        updateSysCmd.Parameters["@X"].Value = (long)(x * 128);
                                        updateSysCmd.Parameters["@Y"].Value = (long)(y * 128);
                                        updateSysCmd.Parameters["@Z"].Value = (long)(z * 128);
                                        updateSysCmd.Parameters["@UpdateTimestamp"].Value = updatedate.Subtract(new DateTime(2015, 1, 1)).TotalSeconds;
                                        updateSysCmd.Parameters["@VersionTimestamp"].Value = DateTime.UtcNow.Subtract(new DateTime(2015, 1, 1)).TotalSeconds;
                                        updateSysCmd.Parameters["@GridId"].Value = gridid;
                                        updateSysCmd.Parameters["@RandomId"].Value = randomid;
                                        updateSysCmd.Parameters["@EdsmId"].Value = edsmid;
                                        updateSysCmd.ExecuteNonQuery();
                                        updatecount++;
                                    }
                                }
                                else                                                                  // not in database..
                                {
                                    insertNameCmd.Parameters["@Name"].Value = name;
                                    insertNameCmd.Parameters["@EdsmId"].Value = edsmid;
                                    insertNameCmd.ExecuteNonQuery();
                                    insertSysCmd.Parameters["@EdsmId"].Value = edsmid;
                                    insertSysCmd.Parameters["@X"].Value = (long)(x * 128);
                                    insertSysCmd.Parameters["@Y"].Value = (long)(y * 128);
                                    insertSysCmd.Parameters["@Z"].Value = (long)(z * 128);
                                    insertSysCmd.Parameters["@CreateTimestamp"].Value = updatedate.Subtract(new DateTime(2015, 1, 1)).TotalSeconds;
                                    insertSysCmd.Parameters["@UpdateTimestamp"].Value = updatedate.Subtract(new DateTime(2015, 1, 1)).TotalSeconds;
                                    insertSysCmd.Parameters["@VersionTimestamp"].Value = DateTime.UtcNow.Subtract(new DateTime(2015, 1, 1)).TotalSeconds;
                                    insertSysCmd.Parameters["@GridId"].Value = gridid;
                                    insertSysCmd.Parameters["@RandomId"].Value = randomid;
                                    insertSysCmd.ExecuteNonQuery();
                                    insertcount++;
                                }
                            }

                            count++;
                        }
                    }
                    finally
                    {
                        if (updateNameCmd != null) updateNameCmd.Dispose();
                        if (updateSysCmd != null) updateSysCmd.Dispose();
                        if (insertNameCmd != null) insertNameCmd.Dispose();
                        if (insertSysCmd != null) insertSysCmd.Dispose();
                        if (selectSysCmd != null) selectSysCmd.Dispose();
                    }
                }
            }

            if (cancelRequested())
            {
                throw new OperationCanceledException();
            }

            return updatecount + insertcount;
        }

        private static long ParseEDSMUpdateSystemsReader(JsonTextReader jr, ref string date, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())  // open the db
            {
                return DoParseEDSMUpdateSystemsReader(jr, ref date, cn, discoveryform, cancelRequested, reportProgress, useCache, useTempSystems);
            }
        }

        public static void RemoveHiddenSystems()
        {
            EDDiscovery2.EDSM.EDSMClass edsm = new EDDiscovery2.EDSM.EDSMClass();

            string strhiddensystems = edsm.GetHiddenSystems();

            if (strhiddensystems != null && strhiddensystems.Length >= 6)
                RemoveHiddenSystems(strhiddensystems);
        }

        public static void RemoveHiddenSystems(string json)
        {
            JsonTextReader jr = new JsonTextReader(new StringReader(json));

            using (SQLiteConnectionSystem cn2 = new SQLiteConnectionSystem())  // open the db
            {
                using (DbTransaction txn = cn2.BeginTransaction())
                {
                    while (jr.Read())
                    {
                        if (jr.TokenType == JsonToken.StartObject)
                        {
                            JObject jo = JObject.Load(jr);

                            long edsmid = (long)jo["id"];
                            string name = (string)jo["system"];
                            string action = (string)jo["action"];
                            long mergedto = 0;

                            if (jo["mergedTo"] != null)
                            {
                                mergedto = (long)jo["mergedTo"];
                            }

                            Console.Write("Remove " + edsmid);
                            using (DbCommand cmd = cn2.CreateCommand("DELETE FROM EdsmSystems WHERE EdsmId=@EdsmId", txn))
                            {
                                cmd.AddParameterWithValue("@EdsmId", edsmid);
                                cmd.ExecuteNonQuery();
                            }

                            if (mergedto > 0)
                            {
                                using (DbCommand cmd = cn2.CreateCommand("INSERT OR IGNORE INTO SystemNames (Name, EdsmId) VALUES (@Name, @EdsmId)", txn))
                                {
                                    cmd.AddParameterWithValue("@Name", name);
                                    cmd.AddParameterWithValue("@EdsmId", edsmid);
                                    cmd.ExecuteNonQuery();
                                }
                                using (DbCommand cmd = cn2.CreateCommand("INSERT OR IGNORE INTO SystemAliases (name, id_edsm, id_edsm_mergedto) VALUES (@name, @id_edsm, @id_edsm_mergedto)", txn))
                                {
                                    cmd.AddParameterWithValue("@name", name);
                                    cmd.AddParameterWithValue("@id_edsm", edsmid);
                                    cmd.AddParameterWithValue("@id_edsm_mergedto", mergedto);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                    }

                    txn.Commit();
                }
            }
        }


        static public long ParseEDDBUpdateSystems(string filename, Action<string> logline)
        {
            StreamReader sr = new StreamReader(filename);         // read directly from file..

            if (sr == null)
                return 0;

            JsonTextReader jr = new JsonTextReader(sr);

            if (jr == null)
                return 0;

            int updated = 0;
            int inserted = 0;

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())  // open the db
            {
                DbCommand selectCmd = null;
                DbCommand insertCmd = null;
                DbCommand updateCmd = null;
                DbCommand updateSysCmd = null;

                using (DbTransaction txn = cn.BeginTransaction())
                {
                    try
                    {
                        selectCmd = cn.CreateCommand("SELECT EddbId, Population, EddbUpdatedAt FROM EddbSystems WHERE EdsmId = @EdsmId LIMIT 1", txn);   // 1 return matching ID
                        selectCmd.AddParameter("@Edsmid", DbType.Int64);

                        insertCmd = cn.CreateCommand("INSERT INTO EddbSystems (EdsmId, EddbId, Name, Faction, Population, GovernmentId, AllegianceId, State, Security, PrimaryEconomyId, NeedsPermit, EddbUpdatedAt) " +
                                                                      "VALUES (@EdsmId, @EddbId, @Name, @Faction, @Population, @GovernmentId, @AllegianceId, @State, @Security, @PrimaryEconomyid, @NeedsPermit, @EddbUpdatedAt)", txn);
                        insertCmd.AddParameter("@EdsmId", DbType.Int64);
                        insertCmd.AddParameter("@EddbId", DbType.Int64);
                        insertCmd.AddParameter("@Name", DbType.String);
                        insertCmd.AddParameter("@Faction", DbType.String);
                        insertCmd.AddParameter("@Population", DbType.Int64);
                        insertCmd.AddParameter("@GovernmentId", DbType.Int64);
                        insertCmd.AddParameter("@AllegianceId", DbType.Int64);
                        insertCmd.AddParameter("@State", DbType.Int64);
                        insertCmd.AddParameter("@Security", DbType.Int64);
                        insertCmd.AddParameter("@PrimaryEconomyId", DbType.Int64);
                        insertCmd.AddParameter("@NeedsPermit", DbType.Int64);
                        insertCmd.AddParameter("@EddbUpdatedAt", DbType.Int64);

                        updateCmd = cn.CreateCommand("UPDATE EddbSystems SET EddbId=@EddbId, Name=@Name, Faction=@Faction, Population=@Population, GovernmentId=@GovernmentId, AllegianceId=@AllegianceId, State=@State, Security=@Security, PrimaryEconomyId=@PrimaryEconomyId, NeedsPermit=@NeedsPermit, EddbUpdatedAt=@EddbUpdatedAt WHERE EdsmId=@EdsmId", txn);
                        updateCmd.AddParameter("@EdsmId", DbType.Int64);
                        updateCmd.AddParameter("@EddbId", DbType.Int64);
                        updateCmd.AddParameter("@Name", DbType.String);
                        updateCmd.AddParameter("@Faction", DbType.String);
                        updateCmd.AddParameter("@Population", DbType.Int64);
                        updateCmd.AddParameter("@GovernmentId", DbType.Int64);
                        updateCmd.AddParameter("@AllegianceId", DbType.Int64);
                        updateCmd.AddParameter("@State", DbType.Int64);
                        updateCmd.AddParameter("@Security", DbType.Int64);
                        updateCmd.AddParameter("@PrimaryEconomyId", DbType.Int64);
                        updateCmd.AddParameter("@NeedsPermit", DbType.Int64);
                        updateCmd.AddParameter("@EddbUpdatedAt", DbType.Int64);

                        updateSysCmd = cn.CreateCommand("UPDATE EdsmSystems SET EddbId=@EddbId WHERE EdsmId=@EdsmId");
                        updateSysCmd.AddParameter("@EdsmId", DbType.Int64);
                        updateSysCmd.AddParameter("@EddbId", DbType.Int64);

                        int c = 0;
                        int hasinfo = 0;
                        int lasttc = Environment.TickCount;

                        while (jr.Read())
                        {
                            if (jr.TokenType == JsonToken.StartObject)
                            {
                                JObject jo = JObject.Load(jr);

                                SystemClass system = new SystemClass(jo, EDDiscovery2.DB.SystemInfoSource.EDDB);

                                if (system.HasEDDBInformation)                                  // screen out for speed any EDDB data with empty interesting fields
                                {
                                    hasinfo++;

                                    selectCmd.Parameters["@EdsmId"].Value = system.id_edsm;     // EDDB carries EDSM ID, so find entry in dB

                                    //DEBUGif ( c > 30000 )  Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " Late info system");

                                    long updated_at = 0;
                                    long population = 0;
                                    long eddbid = 0;

                                    using (DbDataReader reader1 = selectCmd.ExecuteReader())         // if found (if not, we ignore EDDB system)
                                    {
                                        if (reader1.Read())                                     // its there.. check its got the right stuff in it.
                                        {
                                            eddbid = (long)reader1["EddbId"];
                                            updated_at = (long)reader1["EddbUpdatedAt"];
                                            population = (long)reader1["Population"];
                                        }
                                    }

                                    updateSysCmd.Parameters["@EdsmId"].Value = system.id_edsm;
                                    updateSysCmd.Parameters["@EddbId"].Value = system.id_eddb;
                                    updateSysCmd.ExecuteNonQuery();

                                    if (eddbid != 0)
                                    {
                                        if (updated_at != system.eddb_updated_at || population != system.population)
                                        {
                                            updateCmd.Parameters["@EddbId"].Value = system.id_eddb;
                                            updateCmd.Parameters["@Name"].Value = system.name;
                                            updateCmd.Parameters["@Faction"].Value = system.faction;
                                            updateCmd.Parameters["@Population"].Value = system.population;
                                            updateCmd.Parameters["@GovernmentId"].Value = system.government;
                                            updateCmd.Parameters["@AllegianceId"].Value = system.allegiance;
                                            updateCmd.Parameters["@State"].Value = system.state;
                                            updateCmd.Parameters["@Security"].Value = system.security;
                                            updateCmd.Parameters["@PrimaryEconomyId"].Value = system.primary_economy;
                                            updateCmd.Parameters["@NeedsPermit"].Value = system.needs_permit;
                                            updateCmd.Parameters["@EddbUpdatedAt"].Value = system.eddb_updated_at;
                                            updateCmd.Parameters["@EdsmId"].Value = system.id_edsm;
                                            updateCmd.ExecuteNonQuery();
                                            updated++;
                                        }
                                    }
                                    else
                                    {
                                        insertCmd.Parameters["@EdsmId"].Value = system.id_edsm;
                                        insertCmd.Parameters["@EddbId"].Value = system.id_eddb;
                                        insertCmd.Parameters["@Name"].Value = system.name;
                                        insertCmd.Parameters["@Faction"].Value = system.faction;
                                        insertCmd.Parameters["@Population"].Value = system.population;
                                        insertCmd.Parameters["@GovernmentId"].Value = system.government;
                                        insertCmd.Parameters["@AllegianceId"].Value = system.allegiance;
                                        insertCmd.Parameters["@State"].Value = system.state;
                                        insertCmd.Parameters["@Security"].Value = system.security;
                                        insertCmd.Parameters["@PrimaryEconomyId"].Value = system.primary_economy;
                                        insertCmd.Parameters["@NeedsPermit"].Value = system.needs_permit;
                                        insertCmd.Parameters["@EddbUpdatedAt"].Value = system.eddb_updated_at;
                                        insertCmd.ExecuteNonQuery();
                                        inserted++;
                                    }
                                }
                                else
                                {
                                    //Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " No info reject");
                                }

                                if (++c % 10000 == 0)
                                {
                                    Console.WriteLine("EDDB Count " + c + " Delta " + (Environment.TickCount - lasttc) + " info " + hasinfo + " update " + updated + " new " + inserted);
                                    lasttc = Environment.TickCount;
                                }
                            }
                        }

                        txn.Commit();
                    }
                    catch
                    {
                        MessageBox.Show("There is a problem using the EDDB systems file." + Environment.NewLine +
                                        "Please perform a manual EDDB sync (see Admin menu) next time you run the program ", "EDDB Sync Error");
                    }
                    finally
                    {
                        if (selectCmd != null) selectCmd.Dispose();
                        if (updateCmd != null) updateCmd.Dispose();
                        if (insertCmd != null) insertCmd.Dispose();
                    }
                }
            }

            return updated + inserted;
        }
    }

    public class GridId
    {
        public const int gridxrange = 20;
        static private int[] compresstablex = {
                                                0,1,1,1,1, 2,2,2,2,2,                   // 0   -20
                                                3,3,4,4,5, 5,6,7,8,9,                   // 10   -10,-8,-6,..
                                                10,11,12,13,14, 14,15,15,16,16,         // 20 centre
                                                17,17,17,17,17, 18,18,18,18,18,         // 30   +10
                                                19,19                                   // 40   +20
                                            };
        public const int gridzrange = 26;
        static private int[] compresstablez = {
                                                0,1,1,2,2,      3,4,5,6,7,              // 0  -10
                                                8,9,10,11,12,   12,13,13,14,14,         // 10 Sol 0
                                                15,15,15,15,15, 16,16,16,16,16,         // 20   +10
                                                17,17,17,17,17, 18,18,18,18,18,         // 30 centre +20
                                                19,19,19,19,19, 20,20,20,20,20,         // 40 +30
                                                21,21,21,21,21, 22,22,22,22,22,         // 50 +40    
                                                23,23,23,23,23, 24,24,24,24,24,         // 60 +50
                                                25,25                                   // 70 +60
                                            };
        public const int xleft = -20500;
        public const int xright = 20000;
        public const int zbot = -10500;
        public const int ztop = 60000;

        public static int Id(double x, double z)
        {
            x = Math.Min(Math.Max(x - xleft, 0), xright - xleft);       // 40500
            z = Math.Min(Math.Max(z - zbot, 0), ztop - zbot);           // 70500
            x /= 1000;                                                  // 0-40.5 inc
            z /= 1000;                                                  // 0-70.5 inc
            return compresstablex[(int)x] + 100 * compresstablez[(int)z];
        }

        public static int IdFromComponents(int x, int z)
        {
            return x + 100 * z;
        }

        public static bool XZ(int id, out float x, out float z)
        {
            x = 0; z = 0;
            if (id >= 0)
            {
                int xid = (id % 100);
                int zid = (id / 100);

                if (xid < gridxrange && zid < gridzrange)
                {
                    for (int i = 0; i < compresstablex.Length; i++)
                    {
                        if (compresstablex[i] == xid)
                        {
                            double startx = i * 1000 + xleft;

                            while (i < compresstablex.Length && compresstablex[i] == xid)
                                i++;

                            x = (float)((((i * 1000) + xleft) + startx) / 2.0);
                            break;
                        }
                    }

                    for (int i = 0; i < compresstablez.Length; i++)
                    {
                        if (compresstablez[i] == zid)
                        {
                            double startz = i * 1000 + zbot;

                            while (i < compresstablez.Length && compresstablez[i] == zid)
                                i++;

                            z = (float)((((i * 1000) + zbot) + startz) / 2.0);
                            break;
                        }
                    }

                    return true;
                }
            }

            return false;
        }
    }
}
