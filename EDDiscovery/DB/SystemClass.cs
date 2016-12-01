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

    public enum SystemInfoSource
    {
        _RW = 1,
        _EDSC = 2,
        EDDB = 4,
        EDSM = 5
    }


    [DebuggerDisplay("System {name} ({x,nq},{y,nq},{z,nq})")]
    public class SystemClass : EDDiscovery2.DB.InMemory.SystemClass
    {
        const float XYZScalar = 128.0F;     // scaling between DB stored values and floats

        public SystemClass()
        {
        }

        public SystemClass(string Name)
        {
            name = Name;
            status = SystemStatusEnum.Unknown;
            x = double.NaN;
            y = double.NaN;
            z = double.NaN;
        }

        public SystemClass(string Name, double vx, double vy, double vz)
        {
            name = Name;
            status = SystemStatusEnum.Unknown;
            x = vx; y = vy; z = vz;
        }

        public SystemClass(JObject jo, SystemInfoSource source)
        {
            try
            {
                if (source == SystemInfoSource.EDSM)
                {
                    JObject coords = (JObject)jo["coords"];

                    name = jo["name"].Value<string>();

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
                else if (source == SystemInfoSource.EDDB)
                {
                    name = jo["name"].Value<string>();

                    cr = 1;

                    x = jo["x"].Value<double>();
                    y = jo["y"].Value<double>();
                    z = jo["z"].Value<double>();

                    id_eddb = jo["id"].Value<int>();

                    faction = jo["controlling_minor_faction"].Value<string>();

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

                    id_edsm = JSONHelper.GetLong(jo["edsm_id"]);                         // pick up its edsm ID

                    status = SystemStatusEnum.EDDB;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("SystemClass exception: " + ex.Message);
            }           // since we don't have control of outside formats, we fail quietly.
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

                        //Stopwatch ws = new Stopwatch();  ws.Start();

                        Object[] array = new Object[5];     // to the number of items above queried

                        vertices = new Vector3[250000];
                        colours = new uint[250000];

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reader.GetValues(array);

                                long id = (long)array[0];
                                long x = (long)array[1];
                                long y = (long)array[2];
                                long z = (long)array[3];
                                int rand = (int)(long)array[4];

                                if (numvertices == vertices.Length)
                                {
                                    Array.Resize(ref vertices, vertices.Length + 32768);
                                    Array.Resize(ref colours, colours.Length + 32768);
                                }

                                Vector3 pos = new Vector3((float)(x / XYZScalar), (float)(y / XYZScalar), (float)(z / XYZScalar));

                                Color basec = fixedc[rand&3]; 
                                int fade = 100 - ((rand>>2)&7) * 8;
                                byte red = (byte)(basec.R * fade / 100);
                                byte green = (byte)(basec.G * fade / 100);
                                byte blue = (byte)(basec.B * fade / 100);
                                colours[numvertices] = BitConverter.ToUInt32(new byte[] { red, green, blue, 255 }, 0);
                                vertices[numvertices++] = pos;
                            }
                        }

                        Array.Resize(ref vertices, numvertices);
                        Array.Resize(ref colours, numvertices);

                        //Console.WriteLine("Query {0} grid {1} ret {2} took {3}", cmd.CommandText, gridid, numvertices, ws.ElapsedMilliseconds);

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
                                    list.Add(new Point3D(((double)(long)reader["x"]) / XYZScalar, ((double)(long)reader["y"]) / XYZScalar, ((double)(long)reader["z"]) / XYZScalar));
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


        public static List<long> GetEdsmIdsFromName(string name, SQLiteConnectionSystem cn = null)
        {
            List<long> ret = new List<long>();

            if (name.Length > 0)
            {
                bool ownconn = false;
                try
                {
                    if (cn == null)
                    {
                        ownconn = true;
                        cn = new SQLiteConnectionSystem();
                    }

                    using (DbCommand cmd = cn.CreateCommand("SELECT Name,EdsmId FROM SystemNames WHERE Name==@first"))
                    {
                        cmd.AddParameterWithValue("first", name);
                        //Console.WriteLine("Look up {0}", name);

                        using (DbDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                ret.Add((long)rdr[1]);
                            }
                        }
                    }
                }
                finally
                {
                    if (ownconn)
                    {
                        cn.Dispose();
                    }
                }
            }

            return ret;
        }


        public static SystemClass GetSystem(string name, SQLiteConnectionSystem cn = null)      // with an open database, case insensitive
        {
            return GetSystemsByName(name, cn).FirstOrDefault();
        }

        public static List<SystemClass> GetSystemsByName(string name, SQLiteConnectionSystem cn = null)
        {
            List<SystemClass> systems = new List<SystemClass>();

            List<long> edsmidlist = GetEdsmIdsFromName(name, cn);

            foreach (long edsmid in edsmidlist )
            {
                SystemClass sys = GetSystem(edsmid, cn, SystemIDType.EdsmId);
                if (sys != null)
                {
                    systems.Add(sys);
                }
            }

            return systems;
        }

        public enum SystemIDType { id, EdsmId, EddbId };       // which ID to match?

        public static SystemClass GetSystem(long id,  SQLiteConnectionSystem cn = null, SystemIDType idtype = SystemIDType.id)      // using an id
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
                                sys.x = ((double)(long)reader["x"]) / XYZScalar;
                                sys.y = ((double)(long)reader["y"]) / XYZScalar;
                                sys.z = ((double)(long)reader["z"]) / XYZScalar;
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

        // Only hidden systems are deleted, and the table is re-synced every
        // 14 days, so the maximum Id should be very close to the total
        // system count.
        public static long GetTotalSystemsFast()
        {
            long value = 0;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("select MAX(Id) from EdsmSystems"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                value = (long)reader["MAX(Id)"];
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

        public static bool IsSystemsTableEmpty()
        {
            bool isempty = true;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("select Id from EdsmSystems LIMIT 1"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                                isempty = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return isempty;
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

        // Systems in data dumps are now sorted by modify time ascending, so
        // the last inserted system should be the most recently modified system.
        //
        // The beta.edsm.net dumps are currently still in coordinate order, so
        // anything using this should check whether the last dump was ordered by date
        public static DateTime GetLastSystemModifiedTimeFast()
        {
            DateTime lasttime = new DateTime(2010, 1, 1, 0, 0, 0);

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT UpdateTimestamp FROM EdsmSystems ORDER BY Id DESC LIMIT 1"))
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

        public static List<ISystem>  GetSystemDistancesFrom(double x, double y, double z, int maxitems, double maxdist = 200, SQLiteConnectionSystem cn = null)
        {
            bool closeit = false;
            List<ISystem> distlist = new List<ISystem>();

            try
            {
                if (cn == null)
                {
                    closeit = true;
                    cn = new SQLiteConnectionSystem();
                }

                using (DbCommand cmd = cn.CreateCommand(
                    "SELECT EdsmId " +
                    "FROM EdsmSystems " +
                    "WHERE (x-@xv)*(x-@xv)+(y-@yv)*(y-@yv)+(z-@zv)*(z-@zv) < @maxsqdist " +
                    "ORDER BY (x-@xv)*(x-@xv)+(y-@yv)*(y-@yv)+(z-@zv)*(z-@zv) " +
                    "LIMIT @max"))
                {
                    cmd.AddParameterWithValue("@maxsqdist", (long)(maxdist* maxdist* XYZScalar* XYZScalar));
                    cmd.AddParameterWithValue("@max", maxitems);
                    cmd.AddParameterWithValue("xv", (long)(x * XYZScalar));
                    cmd.AddParameterWithValue("yv", (long)(y * XYZScalar));
                    cmd.AddParameterWithValue("zv", (long)(z * XYZScalar));

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() && distlist.Count < maxitems)    
                        {
                            long edsmid = (long)reader[0];
                            {
                                    distlist.Add(GetSystem(edsmid, cn, SystemIDType.EdsmId));
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
            return distlist;
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
                    cmd.AddParameterWithValue("xv", (long)(x * XYZScalar));
                    cmd.AddParameterWithValue("yv", (long)(y * XYZScalar));
                    cmd.AddParameterWithValue("zv", (long)(z * XYZScalar));
                    cmd.AddParameterWithValue("max", maxitems + 1);     // 1 more, because if we are on a star, that will be returned
                    cmd.AddParameterWithValue("maxdist", (long)(maxdist * XYZScalar));

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() && distlist.Count < maxitems)           // already sorted, and already limited to max items
                        {
                            long edsmid = (long)reader[0];

                            if (System.DBNull.Value != reader[1])                 // paranoid check for null
                            {
                                double dx = ((double)(long)reader[1]) / XYZScalar - x;
                                double dy = ((double)(long)reader[2]) / XYZScalar - y;
                                double dz = ((double)(long)reader[3]) / XYZScalar - z;

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

        public static SystemClass GetSystemNearestTo(double x, double y, double z, SQLiteConnectionSystem conn)
        {
            using (DbCommand selectByPosCmd = conn.CreateCommand(
                "SELECT s.EdsmId FROM EdsmSystems s " +         // 16 is 0.125 of 1/128, so pick system near this one
                "WHERE s.X >= @X - 16 " +
                "AND s.X <= @X + 16 " +
                "AND s.Y >= @Y - 16 " +
                "AND s.Y <= @Y + 16 " +
                "AND s.Z >= @Z - 16 " +
                "AND s.Z <= @Z + 16 LIMIT 1"))
            {
                selectByPosCmd.AddParameterWithValue("@X", (long)(x * XYZScalar));
                selectByPosCmd.AddParameterWithValue("@Y", (long)(y * XYZScalar));
                selectByPosCmd.AddParameterWithValue("@Z", (long)(z * XYZScalar));

                using (DbDataReader reader = selectByPosCmd.ExecuteReader())        // MEASURED very fast, <1ms
                {
                    while (reader.Read())
                    {
                        long pos_edsmid = (long)reader["EdsmId"];
                        SystemClass sys = GetSystem(pos_edsmid, conn, SystemIDType.EdsmId);
                        return sys;
                    }
                }
            }

            return null;
        }

        public static SystemClass GetSystemNearestTo(Point3D curpos, Point3D wantedpos, double maxfromcurpos, double maxfromwanted,
                                    int routemethod)
        {
            SystemClass nearestsystem = null;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    string sqlquery = "SELECT EdsmId, x, y, z " +                   // DO a square test for speed, then double check its within the circle later..
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
                        cmd.AddParameterWithValue("xw", (long)(wantedpos.X * XYZScalar));
                        cmd.AddParameterWithValue("yw", (long)(wantedpos.Y * XYZScalar));
                        cmd.AddParameterWithValue("zw", (long)(wantedpos.Z * XYZScalar));
                        cmd.AddParameterWithValue("maxfromwanted", (long)(maxfromwanted * XYZScalar));     //squared

                        cmd.AddParameterWithValue("xc", (long)(curpos.X * XYZScalar));
                        cmd.AddParameterWithValue("yc", (long)(curpos.Y * XYZScalar));
                        cmd.AddParameterWithValue("zc", (long)(curpos.Z * XYZScalar));
                        cmd.AddParameterWithValue("maxfromcurpos", (long)(maxfromcurpos * XYZScalar));     //squared

                        double bestmindistance = double.MaxValue;
                        long nearestedsmid = -1;

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                long edsmid = (long)reader[0];

                                //SystemClass sys = GetSystem(edsmid, null, SystemIDType.EdsmId);  Console.WriteLine("FOund {0} at {1} {2} {3}", sys.name, sys.x, sys.y, sys.z);

                                if (System.DBNull.Value != reader["x"]) // paranoid check, it could be null in db
                                {
                                    Point3D syspos = new Point3D(((double)(long)reader[1])/XYZScalar, ((double)(long)reader[2])/XYZScalar, ((double)(long)reader[3])/XYZScalar);

                                    double distancefromwantedx2 = Point3D.DistanceBetweenX2(wantedpos, syspos); // range between the wanted point and this, ^2
                                    double distancefromcurposx2 = Point3D.DistanceBetweenX2(curpos, syspos);    // range between the wanted point and this, ^2

                                                                                                                // ENSURE its withing the circles now
                                    if (distancefromcurposx2 <= (maxfromcurpos * maxfromcurpos) && distancefromwantedx2 <= (maxfromwanted * maxfromwanted))
                                    {
                                        if (routemethod == metric_nearestwaypoint)
                                        {
                                            if (distancefromwantedx2 < bestmindistance)
                                            {
                                                nearestedsmid = edsmid;
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
                                                nearestedsmid = edsmid;
                                                bestmindistance = metric;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (nearestedsmid != -1)
                            nearestsystem = GetSystem(nearestedsmid, cn, SystemIDType.EdsmId);
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

        public static void GetSystemAndAlternatives(EliteDangerous.JournalEvents.JournalLocOrJump vsc, out ISystem system, out List<ISystem> alternatives, out string namestatus)
        {
            system = new EDDiscovery2.DB.InMemory.SystemClass
            {
                name = vsc.StarSystem,
                x = vsc.HasCoordinate ? vsc.StarPos[0] : Double.NaN,
                y = vsc.HasCoordinate ? vsc.StarPos[1] : Double.NaN,
                z = vsc.HasCoordinate ? vsc.StarPos[2] : Double.NaN,
                id_edsm = vsc.EdsmID
            };

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                // Check that the SystemAliases table is not empty
                using (DbCommand cmd = cn.CreateCommand("SELECT COUNT(id) FROM SystemAliases"))
                {
                    long nrows = (long)cmd.ExecuteScalar();

                    if (nrows == 0)
                    {
                        //Console.WriteLine("Populating system aliases table");
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


                Dictionary<long, SystemClass> altmatches = new Dictionary<long, SystemClass>();
                Dictionary<long, SystemClass> matches = new Dictionary<long, SystemClass>();
                SystemClass edsmidmatch = null;
                long sel_edsmid = vsc.EdsmID;
                bool hastravcoords = vsc.HasCoordinate && (vsc.StarSystem.ToLowerInvariant() == "sol" || vsc.StarPos[0] != 0 || vsc.StarPos[1] != 0 || vsc.StarPos[2] != 0);
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

                //Stopwatch sw2 = new Stopwatch(); sw2.Start(); //long t2 = sw2.ElapsedMilliseconds; Tools.LogToFile(string.Format("Query names in {0}", t2));

                Dictionary<long, SystemClass> namematches = GetSystemsByName(vsc.StarSystem).Where(s => s != null).ToDictionary(s => s.id, s => s);
                Dictionary<long, SystemClass> posmatches = new Dictionary<long, SystemClass>();
                Dictionary<long, SystemClass> nameposmatches = new Dictionary<long, SystemClass>();

                if (hastravcoords)
                {
                    using (DbCommand selectByPosCmd = cn.CreateCommand(
                        "SELECT s.EdsmId FROM EdsmSystems s " +         // 16 is 0.125 of 1/128, so pick system near this one
                        "WHERE s.X >= @X - 16 " +
                        "AND s.X <= @X + 16 " +
                        "AND s.Y >= @Y - 16 " +
                        "AND s.Y <= @Y + 16 " +
                        "AND s.Z >= @Z - 16 " +
                        "AND s.Z <= @Z + 16"))
                    {
                        selectByPosCmd.AddParameterWithValue("@X", (long)(vsc.StarPos[0] * XYZScalar));
                        selectByPosCmd.AddParameterWithValue("@Y", (long)(vsc.StarPos[1] * XYZScalar));
                        selectByPosCmd.AddParameterWithValue("@Z", (long)(vsc.StarPos[2] * XYZScalar));

                        //Stopwatch sw = new Stopwatch(); sw.Start(); long t1 = sw.ElapsedMilliseconds; Tools.LogToFile(string.Format("Query pos in {0}", t1));

                        using (DbDataReader reader = selectByPosCmd.ExecuteReader())        // MEASURED very fast, <1ms
                        {


                            while (reader.Read())
                            {
                                long pos_edsmid = (long)reader["EdsmId"];
                                SystemClass sys = GetSystem(pos_edsmid, cn, SystemIDType.EdsmId);
                                if (sys != null)
                                {
                                    matches[sys.id] = sys;
                                    posmatches[sys.id] = sys;

                                    if (sys.name.Equals(vsc.StarSystem, StringComparison.InvariantCultureIgnoreCase))
                                    {
                                        nameposmatches[sys.id] = sys;
                                    }
                                }
                            }
                        }
                    }
                }

                if (aliasesByName.ContainsKey(vsc.StarSystem))
                {
                    foreach (long alt_edsmid in aliasesByName[vsc.StarSystem])
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

                alternatives = matches.Values.Select(s => (ISystem)s).ToList();

                if (edsmidmatch != null)
                {
                    system = edsmidmatch;

                    if (nameposmatches.ContainsKey(system.id)) // name and position matches
                    {
                        namestatus = "Exact match";
                        return; // Continue to next system
                    }
                    else if (posmatches.ContainsKey(system.id)) // position matches
                    {
                        namestatus = "Name differs";
                        return; // Continue to next system
                    }
                    else if (!hastravcoords || !system.HasCoordinate) // no coordinates available
                    {
                        if (namematches.ContainsKey(system.id)) // name matches
                        {
                            if (!system.HasCoordinate)
                            {
                                namestatus = "System has no known coordinates";
                            }
                            else
                            {
                                namestatus = "Travel log entry has no coordinates";
                            }

                            return; // Continue to next system
                        }
                        else if (!vsc.HasCoordinate)
                        {
                            namestatus = "Name differs";
                        }
                    }
                }

                if (nameposmatches != null && nameposmatches.Count != 0)
                {
                    if (nameposmatches.Count == 1)
                    {
                        // Both name and position matches
                        system = nameposmatches.Values.Single();
                        namestatus = "Exact match";
                        return; // Continue to next system
                    }
                    else if (posmatches.Count == 1)
                    {
                        // Position matches
                        system = posmatches.Values.Single();
                        namestatus = $"System {system.name} found at location";
                        return; // Continue to next system
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
                        system = namematches.Values.Single();
                        namestatus = "Name matched";
                        return;
                    }
                    else if (namematches.Count > 1)
                    {
                        multimatch = true;
                    }
                }

                if (multimatch)
                {
                    namestatus = "Multiple system matches found";
                }
                else
                {
                    namestatus = "System not found";
                }
            }
        }

        public static long ParseEDSMUpdateSystemsString(string json, ref string date, ref bool outoforder, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true)
        {
            using (StringReader sr = new StringReader(json))
                return ParseEDSMUpdateSystemsStream(sr, ref date, ref outoforder, removenonedsmids, discoveryform, cancelRequested, reportProgress, useCache);
        }

        public static long ParseEDSMUpdateSystemsFile(string filename, ref string date, ref bool outoforder, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true)
        {
            using (StreamReader sr = new StreamReader(filename))         // read directly from file..
                return ParseEDSMUpdateSystemsStream(sr, ref date, ref outoforder, removenonedsmids, discoveryform, cancelRequested, reportProgress, useCache);
        }

        public static long ParseEDSMUpdateSystemsStream(TextReader sr, ref string date, ref bool outoforder, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            using (JsonTextReader jr = new JsonTextReader(sr))
                return ParseEDSMUpdateSystemsReader(jr, ref date, ref outoforder, removenonedsmids, discoveryform, cancelRequested, reportProgress, useCache, useTempSystems);
        }

        private static Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase> GetEdsmSystemsLite()
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: EDDbAccessMode.Reader))
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
                                sys.x = ((double)(long)reader["x"]) / XYZScalar;
                                sys.y = ((double)(long)reader["y"]) / XYZScalar;
                                sys.z = ((double)(long)reader["z"]) / XYZScalar;
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
        }

        private static long DoParseEDSMUpdateSystemsReader(JsonTextReader jr, ref string date, ref bool outoforder, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            DateTime maxdate;

            if (!DateTime.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out maxdate))
            {
                maxdate = new DateTime(2010, 1, 1);
            }

            Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase> systemsByEdsmId = useCache ? GetEdsmSystemsLite() : new Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase>();
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

                IEnumerator<JObject> jo_enum = objs.GetEnumerator();
                bool jo_enum_finished = false;

                while (!jo_enum_finished && !cancelRequested())
                {
                    using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: EDDbAccessMode.Writer))
                    {
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

                                while (!cancelRequested() && !SQLiteConnectionSystem.IsReadWaiting)
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

                                        jo_enum_finished = true;
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
                                        else if (updatedate < maxdate - TimeSpan.FromHours(1))
                                            outoforder = true;

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
                                                            dbsys.x = ((double)(long)reader["X"]) / XYZScalar;
                                                            dbsys.y = ((double)(long)reader["Y"]) / XYZScalar;
                                                            dbsys.z = ((double)(long)reader["Z"]) / XYZScalar;
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
                                                updateNameCmd.Parameters["@Name"].Value = name;
                                                updateNameCmd.Parameters["@EdsmId"].Value = edsmid;
                                                updateNameCmd.ExecuteNonQuery();
                                            }

                                            if (Math.Abs(dbsys.x - x) > 0.01 ||
                                                Math.Abs(dbsys.y - y) > 0.01 ||
                                                Math.Abs(dbsys.z - z) > 0.01 ||
                                                dbsys.gridid != gridid)  // position changed
                                            {
                                                updateSysCmd.Parameters["@X"].Value = (long)(x * XYZScalar);
                                                updateSysCmd.Parameters["@Y"].Value = (long)(y * XYZScalar);
                                                updateSysCmd.Parameters["@Z"].Value = (long)(z * XYZScalar);
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
                                            insertSysCmd.Parameters["@X"].Value = (long)(x * XYZScalar);
                                            insertSysCmd.Parameters["@Y"].Value = (long)(y * XYZScalar);
                                            insertSysCmd.Parameters["@Z"].Value = (long)(z * XYZScalar);
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
                }
            }

            if (cancelRequested())
            {
                throw new OperationCanceledException();
            }

            return updatecount + insertcount;
        }

        private static long ParseEDSMUpdateSystemsReader(JsonTextReader jr, ref string date, ref bool outoforder, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            return DoParseEDSMUpdateSystemsReader(jr, ref date, ref outoforder, discoveryform, cancelRequested, reportProgress, useCache, useTempSystems);
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
            bool jr_eof = false;

            while (!jr_eof)
            {
                using (SQLiteConnectionSystem cn2 = new SQLiteConnectionSystem(mode: EDDbAccessMode.Writer))  // open the db
                {
                    using (DbTransaction txn = cn2.BeginTransaction())
                    {
                        DbCommand infoinscmd = null;
                        DbCommand infodelcmd = null;
                        DbCommand namedelcmd = null;

                        try
                        {
                            infoinscmd = cn2.CreateCommand("INSERT OR IGNORE INTO SystemAliases (name, id_edsm, id_edsm_mergedto) VALUES (@name, @id_edsm, @id_edsm_mergedto)", txn);
                            infoinscmd.AddParameter("@name", DbType.String);
                            infoinscmd.AddParameter("@id_edsm", DbType.Int64);
                            infoinscmd.AddParameter("@id_edsm_mergedto", DbType.Int64);
                            infodelcmd = cn2.CreateCommand("DELETE FROM EdsmSystems WHERE EdsmId=@EdsmId", txn);
                            infodelcmd.AddParameter("@EdsmId", DbType.Int64);
                            namedelcmd = cn2.CreateCommand("DELETE FROM SystemNames WHERE EdsmId=@EdsmId", txn);
                            namedelcmd.AddParameter("@EdsmId", DbType.Int64);

                            while (!SQLiteConnectionSystem.IsReadWaiting)
                            {
                                if (!jr.Read())
                                {
                                    jr_eof = true;
                                    break;
                                }

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
                                    infodelcmd.Parameters["@EdsmId"].Value = edsmid;
                                    infodelcmd.ExecuteNonQuery();
                                    namedelcmd.Parameters["@EdsmId"].Value = edsmid;
                                    namedelcmd.ExecuteNonQuery();

                                    if (mergedto > 0)
                                    {
                                        infoinscmd.Parameters["@name"].Value = name;
                                        infoinscmd.Parameters["@id_edsm"].Value = edsmid;
                                        infoinscmd.Parameters["@id_edsm_mergedto"].Value = mergedto;
                                        infoinscmd.ExecuteNonQuery();
                                    }
                                }
                            }

                            txn.Commit();
                        }
                        finally
                        {
                            if (infoinscmd != null) infoinscmd.Dispose();
                            if (infodelcmd != null) infodelcmd.Dispose();
                            if (namedelcmd != null) namedelcmd.Dispose();
                        }
                    }
                }
            }
        }

        static public long ParseEDDBUpdateSystems(string filename, Action<string> logline)
        {
            StreamReader sr = new StreamReader(filename);         // read directly from file..

            if (sr == null)
                return 0;

            string line;

            int updated = 0;
            int inserted = 0;

            while (!sr.EndOfStream)
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: EDDbAccessMode.Writer))  // open the db
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

                            while (!SQLiteConnectionSystem.IsReadWaiting)
                            {
                                line = sr.ReadLine();
                                if (line == null)  // End of stream
                                {
                                    break;
                                }

                                {
                                    JObject jo = JObject.Parse(line);

                                    SystemClass system = new SystemClass(jo, SystemInfoSource.EDDB);

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
                            break;
                        }
                        finally
                        {
                            if (selectCmd != null) selectCmd.Dispose();
                            if (updateCmd != null) updateCmd.Dispose();
                            if (insertCmd != null) insertCmd.Dispose();
                        }
                    }
                }
            }

            return updated + inserted;
        }



        public static List<string> AutoCompleteAdditionalList = new List<string>();

        public static void AddToAutoComplete( List<string> t )
        {
            lock (AutoCompleteAdditionalList)
            {
                AutoCompleteAdditionalList.AddRange(t);
            }
        }

        public static List<string> ReturnSystemListForAutoComplete(string input)
        {
            List<string> ret = new List<string>();

            if (input.Length > 0)
            {
                lock (AutoCompleteAdditionalList)
                {
                    foreach (string other in AutoCompleteAdditionalList)
                    {
                        if (other.StartsWith(input, StringComparison.InvariantCultureIgnoreCase))
                            ret.Add(other);
                    }
                }

                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT Name,EdsmId FROM SystemNames WHERE Name>=@first AND Name<=@second LIMIT 1000"))
                    {
                        cmd.AddParameterWithValue("first", input);
                        cmd.AddParameterWithValue("second", input + "~");

                        using (DbDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                ret.Add((string)rdr[0]);
                            }
                        }
                    }
                }
            }

            return ret;
        }

        public static SystemClass FindEDSM(ISystem s, SQLiteConnectionSystem conn = null) // called find an EDSM system corresponding to s
        {
            SystemClass system = null;

            if (s.status != SystemStatusEnum.EDSC)      // if not EDSM already..
            {
                bool closeit = false;

                if (conn == null)
                {
                    closeit = true;
                    conn = new SQLiteConnectionSystem();
                }

                if (s.id_edsm > 0)                      // if it has an ID, look it up
                    system = SystemClass.GetSystem(s.id_edsm, conn, SystemClass.SystemIDType.EdsmId);

                if (system == null)                   // not found, so  try
                {
                    List<SystemClass> systemsByName = GetSystemsByName(s.name, conn);

                    if (systemsByName.Count == 0 && s.HasCoordinate)
                    {
                        system = GetSystemNearestTo(s.x, s.y, s.z, conn);
                    }
                    else
                    {
                        double mindist = 0.5;

                        foreach (SystemClass sys in systemsByName)
                        {
                            double dist = Distance(sys, s);

                            if (dist < mindist)
                            {
                                mindist = dist;
                                system = sys;
                            }
                        }
                    }
                }

                if (closeit && conn != null)
                {
                    conn.Dispose();
                }
            }

            return system;
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

