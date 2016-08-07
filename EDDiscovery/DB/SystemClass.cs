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

                    government = EliteDangerous.Government2ID(jo["government"]);
                    allegiance = EliteDangerous.Allegiance2ID(jo["allegiance"]);

                    state = EliteDangerous.EDState2ID(jo["state"]);
                    security = EliteDangerous.EDSecurity2ID(jo["security"]);

                    primary_economy = EliteDangerous.EDEconomy2ID(jo["primary_economy"]);

                    if (jo["needs_permit"].Type == JTokenType.Integer)
                        needs_permit = jo["needs_permit"].Value<int>();

                    eddb_updated_at = jo["updated_at"].Value<int>();

                    id_edsm = (long)jo["edsm_id"];                         // pick up its edsm ID

                    status = SystemStatusEnum.EDDB;
                }
            }
            catch { }           // since we don't have control of outside formats, we fail quietly.
        }

        public SystemClass(DataRow dr)
        {
            Object o;

            id = (long)dr["id"];
            name = (string)dr["name"];
            SearchName = name.ToLower();

            if (System.DBNull.Value != dr["id_edsm"])
                id_edsm = (long)dr["id_edsm"];

            cr = (int)(long)dr["cr"];

            if (System.DBNull.Value == dr["x"])
            {
                x = double.NaN;
                y = double.NaN;
                z = double.NaN;
            }
            else
            {
                x = (double)dr["x"];
                y = (double)dr["y"];
                z = (double)dr["z"];
                CommanderCreate = dr["commandercreate"].ToString();
                if (CommanderCreate.Length > 0)
                    CreateDate = (DateTime)dr["createdate"];
                else
                    CreateDate = new DateTime(1980, 1, 1);

                CommanderUpdate = (string)dr["commanderupdate"].ToString();
                if (CommanderUpdate.Length > 0)
                    UpdateDate = (DateTime)dr["updatedate"];
                else
                    UpdateDate = new DateTime(1980, 1, 1);
            }

            status = (SystemStatusEnum)((long)dr["status"]);
            SystemNote = dr["Note"].ToString();

            o = dr["id_eddb"];
            id_eddb = o == DBNull.Value ? 0 : ((long)o);

            o = dr["population"];
            population = o == DBNull.Value ? 0 : (long)o;

            o = dr["faction"];
            faction = o == DBNull.Value ? null : (string)o;

            o = dr["government_id"];
            government = o == DBNull.Value ? EDGovernment.Unknown : (EDGovernment)((long)o);

            o = dr["allegiance_id"];
            allegiance = o == DBNull.Value ? EDAllegiance.Unknown : (EDAllegiance)((long)o);

            o = dr["primary_economy_id"];
            primary_economy = o == DBNull.Value ? EDEconomy.Unknown : (EDEconomy)((long)o);

            o = dr["security"];
            security = o == DBNull.Value ? EDSecurity.Unknown : (EDSecurity)((long)o);

            o = dr["eddb_updated_at"];
            eddb_updated_at = o == DBNull.Value ? 0 : (int)((long)o);

            o = dr["state"];
            state = o == DBNull.Value ? EDState.Unknown : (EDState)((long)o);

            o = dr["needs_permit"];
            needs_permit = o == DBNull.Value ? 0 : (int)((long)o);

            o = dr["gridid"];
            gridid = o == DBNull.Value ? 0 : (int)((long)o);

            o = dr["randomid"];
            randomid = o == DBNull.Value ? 0 : (int)((long)o);
        }

        public SystemClass(DbDataReader dr)         // read from a SQLite reader after a query
        {
            Object o;

            id = (long)dr["id"];
            name = (string)dr["name"];
            SearchName = name.ToLower();

            if (System.DBNull.Value != dr["id_edsm"])
                id_edsm = (long)dr["id_edsm"];

            cr = (int)(long)dr["cr"];

            if (System.DBNull.Value == dr["x"])
            {
                x = double.NaN;
                y = double.NaN;
                z = double.NaN;
            }
            else
            {
                x = (double)dr["x"];
                y = (double)dr["y"];
                z = (double)dr["z"];
                CommanderCreate = dr["commandercreate"].ToString();
                if (CommanderCreate.Length > 0)
                    CreateDate = (DateTime)dr["createdate"];
                else
                    CreateDate = new DateTime(1980, 1, 1);

                CommanderUpdate = (string)dr["commanderupdate"].ToString();
                if (CommanderUpdate.Length > 0)
                    UpdateDate = (DateTime)dr["updatedate"];
                else
                    UpdateDate = new DateTime(1980, 1, 1);
            }

            status = (SystemStatusEnum)((long)dr["status"]);
            SystemNote = dr["Note"].ToString();

            o = dr["id_eddb"];
            id_eddb = o == DBNull.Value ? 0 : ((long)o);

            o = dr["population"];
            population = o == DBNull.Value ? 0 : (long)o;

            o = dr["faction"];
            faction = o == DBNull.Value ? null : (string)o;

            o = dr["government_id"];
            government = o == DBNull.Value ? EDGovernment.Unknown : (EDGovernment)((long)o);

            o = dr["allegiance_id"];
            allegiance = o == DBNull.Value ? EDAllegiance.Unknown : (EDAllegiance)((long)o);

            o = dr["primary_economy_id"];
            primary_economy = o == DBNull.Value ? EDEconomy.Unknown : (EDEconomy)((long)o);

            o = dr["security"];
            security = o == DBNull.Value ? EDSecurity.Unknown : (EDSecurity)((long)o);

            o = dr["eddb_updated_at"];
            eddb_updated_at = o == DBNull.Value ? 0 : (int)((long)o);

            o = dr["state"];
            state = o == DBNull.Value ? EDState.Unknown : (EDState)((long)o);

            o = dr["needs_permit"];
            needs_permit = o == DBNull.Value ? 0 : (int)((long)o);

            o = dr["gridid"];
            gridid = o == DBNull.Value ? 0 : (int)((long)o);

            o = dr["randomid"];
            randomid = o == DBNull.Value ? 0 : (int)((long)o);
        }


        public static bool Delete(SystemStatusEnum source)
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                using (DbCommand cmd = cn.CreateCommand("Delete from Systems where Status=@Status"))
                {
                    cmd.AddParameterWithValue("@Status", (int)source);
                    SQLiteDBClass.SQLNonQueryText(cn, cmd);
                }
            }
            return true;
        }

        public static List<SystemClass> ParseEDSC(string json, ref string date)
        {
            JObject edsc = null;
            if (json != null)
                edsc = (JObject)JObject.Parse(json);

            List<SystemClass> listSystems = new List<SystemClass>();

            if (edsc == null)
                return listSystems;


            JObject edscdata = (JObject)edsc["d"];

            if (edscdata == null)
                edscdata = edsc;

            JArray systems = (JArray)edscdata["systems"];

            date = edscdata["date"].Value<string>();

            foreach (JObject jo in systems)
            {
                string name = jo["name"].Value<string>();

                SystemClass system = new SystemClass(jo, EDDiscovery2.DB.SystemInfoSource.EDSC);

                if (system.HasCoordinate)
                    listSystems.Add(system);
            }
            return listSystems;
        }


        public bool Store(SQLiteConnectionED cn, DbTransaction transaction)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note, id_eddb, population, faction, government_id, allegiance_id, primary_economy_id, security, eddb_updated_at, state, needs_permit, versiondate, id_edsm, gridid, randomid) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note, @id_eddb, @population, @faction, @government_id, @allegiance_id, @primary_economy_id,  @security, @eddb_updated_at, @state, @needs_permit, datetime('now'),@id_edsm,@gridid,@randomid)", transaction))
            {
                if (SystemNote == null)
                    SystemNote = "";

                if (id_eddb != 0)
                {
                    cmd.AddParameterWithValue("@name", name);
                    cmd.AddParameterWithValue("@x", x);
                    cmd.AddParameterWithValue("@y", y);
                    cmd.AddParameterWithValue("@z", z);
                    cmd.AddParameterWithValue("@cr", cr);
                    cmd.AddParameterWithValue("@CommanderCreate", CommanderCreate);
                    cmd.AddParameterWithValue("@Createdate", CreateDate);
                    cmd.AddParameterWithValue("@CommanderUpdate", CommanderCreate);
                    cmd.AddParameterWithValue("@updatedate", CreateDate);
                    cmd.AddParameterWithValue("@Status", (int)status);

                    cmd.AddParameterWithValue("@id_eddb", id_eddb);
                    cmd.AddParameterWithValue("@population", population);
                    cmd.AddParameterWithValue("@faction", faction);
                    cmd.AddParameterWithValue("@government_id", government);
                    cmd.AddParameterWithValue("@allegiance_id", allegiance);
                    cmd.AddParameterWithValue("@primary_economy_id", primary_economy);
                    cmd.AddParameterWithValue("@security", security);
                    cmd.AddParameterWithValue("@eddb_updated_at", eddb_updated_at);
                    cmd.AddParameterWithValue("@state", state);
                    cmd.AddParameterWithValue("@needs_permit", needs_permit);
                    cmd.AddParameterWithValue("@Note", SystemNote);
                    cmd.AddParameterWithValue("@id_edsm", id_edsm);
                    cmd.AddParameterWithValue("@gridid", gridid);
                    cmd.AddParameterWithValue("@randomid", randomid);
                }
                else
                {       // override the cmd.
                    cmd.CommandText = "Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note, versiondate,id_edsm,gridid,randomid) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note, datetime('now'),@id_edsm,@gridid,@randomid)";
                    cmd.AddParameterWithValue("@name", name);
                    cmd.AddParameterWithValue("@x", x);
                    cmd.AddParameterWithValue("@y", y);
                    cmd.AddParameterWithValue("@z", z);
                    cmd.AddParameterWithValue("@cr", cr);
                    cmd.AddParameterWithValue("@CommanderCreate", CommanderCreate);
                    cmd.AddParameterWithValue("@Createdate", CreateDate);
                    cmd.AddParameterWithValue("@CommanderUpdate", CommanderCreate);
                    cmd.AddParameterWithValue("@updatedate", CreateDate);
                    cmd.AddParameterWithValue("@Status", (int)status);
                    cmd.AddParameterWithValue("@Note", SystemNote);
                    cmd.AddParameterWithValue("@id_edsm", id_edsm);
                    cmd.AddParameterWithValue("@gridid", gridid);
                    cmd.AddParameterWithValue("@randomid", randomid);
                }

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public bool Update(SQLiteConnectionED cn, long id, DbTransaction transaction)
        {
            using (DbCommand cmd = cn.CreateCommand("Update Systems set name=@name, x=@x, y=@y, z=@z, cr=@cr, commandercreate=@commandercreate, createdate=@createdate, commanderupdate=@commanderupdate, updatedate=@updatedate, status=@status, note=@Note, id_eddb=@id_eddb, population=@population, faction=@faction, government_id=@government_id, allegiance_id=@allegiance_id, primary_economy_id=@primary_economy_id,  security=@security, eddb_updated_at=@eddb_updated_at, state=@state, needs_permit=@needs_permit, versiondate=datetime('now'), id_edsm=@id_edsm, gridid=@gridid, randomid=@randomid where ID=@id", transaction))
            {
                cmd.AddParameterWithValue("@id", id);
                cmd.AddParameterWithValue("@name", name);
                cmd.AddParameterWithValue("@x", x);
                cmd.AddParameterWithValue("@y", y);
                cmd.AddParameterWithValue("@z", z);
                cmd.AddParameterWithValue("@cr", cr);
                cmd.AddParameterWithValue("@CommanderCreate", CommanderCreate);
                cmd.AddParameterWithValue("@Createdate", CreateDate);
                cmd.AddParameterWithValue("@CommanderUpdate", CommanderCreate);
                cmd.AddParameterWithValue("@updatedate", CreateDate);
                cmd.AddParameterWithValue("@Status", (int)status);
                if (SystemNote == null)
                    SystemNote = "";
                cmd.AddParameterWithValue("@Note", SystemNote);

                cmd.AddParameterWithValue("@id_eddb", id_eddb);
                cmd.AddParameterWithValue("@population", population);
                cmd.AddParameterWithValue("@faction", faction);
                cmd.AddParameterWithValue("@government_id", government);
                cmd.AddParameterWithValue("@allegiance_id", allegiance);
                cmd.AddParameterWithValue("@primary_economy_id", primary_economy);
                cmd.AddParameterWithValue("@security", security);
                cmd.AddParameterWithValue("@eddb_updated_at", eddb_updated_at);
                cmd.AddParameterWithValue("@state", state);
                cmd.AddParameterWithValue("@needs_permit", needs_permit);

                cmd.AddParameterWithValue("@id_edsm", id_edsm);
                cmd.AddParameterWithValue("@gridid", gridid);
                cmd.AddParameterWithValue("@randomid", randomid);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public bool UpdateEDSM(SQLiteConnectionED cn, long id, DbTransaction transaction)     // only altering fields EDSM sets..
        {
            using (DbCommand cmd = cn.CreateCommand("Update Systems set name=@name, x=@x, y=@y, z=@z, versiondate=datetime('now'), gridid=@gridid, randomid=@randomid where ID=@id", transaction))
            {
                cmd.AddParameterWithValue("@id", id);
                cmd.AddParameterWithValue("@name", name);
                cmd.AddParameterWithValue("@x", x);
                cmd.AddParameterWithValue("@y", y);
                cmd.AddParameterWithValue("@z", z);
                cmd.AddParameterWithValue("@gridid", gridid);
                cmd.AddParameterWithValue("@randomid", randomid);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public enum SystemIDType { id, id_edsm, id_eddb };       // which ID to match?

        public static bool Delete(long id, SQLiteConnectionED cn = null, DbTransaction transaction = null, SystemIDType idtype = SystemIDType.id)
        {
            using (DbCommand cmd = cn.CreateCommand("Delete from Systems where " + idtype.ToString() + "=@id", transaction))
            {
                cmd.AddParameterWithValue("@id", id);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);
            }

            return true;
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
        public static int GetSystemVector(int gridid, ref Vector3d[] vertices, SystemAskType ask, int percentage)
        {
            int numvertices = 0;
            vertices = null;

            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select id,x,y,z from Systems where gridid=@gridid"))
                    {
                        cmd.AddParameterWithValue("gridid", gridid);

                        if (ask == SystemAskType.PopulatedStars)
                            cmd.CommandText += " and population<>0";
                        else if (ask == SystemAskType.UnPopulatedStars)
                            cmd.CommandText += " and (population=0 or population is null)";

                        if (percentage < 100)
                            cmd.CommandText += " and randomid<" + percentage;

                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (System.DBNull.Value != reader["x"])
                                {
                                    if (vertices == null)
                                        vertices = new Vector3d[1024];
                                    else if (numvertices == vertices.Length)
                                        Array.Resize(ref vertices, vertices.Length + 8192);

                                    vertices[numvertices++] = new Vector3d((double)reader["x"], (double)reader["y"], (double)reader["z"]);
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

            return numvertices;
        }

        public static void GetSystemNames(ref AutoCompleteStringCollection asc)
        {
            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select name from Systems"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                asc.Add((string)reader["name"]);
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
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select x,y,z from Systems"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (System.DBNull.Value != reader["x"])
                                    list.Add(new Point3D((double)reader["x"], (double)reader["y"], (double)reader["z"]));
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

        public delegate void StarCallBack(SystemClass sys);
        public static void ProcessStars(StarCallBack cb)  // return star positions..
        {
            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select * from Systems"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SystemClass sys = new SystemClass(reader);
                                cb(sys);
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

        public static Dictionary<string, int> GetSystemNamesUpperCase()  // return a dictionary, in upper case, id is the row ID in the table, duplicates ignored.
        {
            Dictionary<string, int> dict = new Dictionary<string, int>(StringComparer.CurrentCultureIgnoreCase);

            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select name from Systems"))
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

        public static SystemClass GetSystem(string name, SQLiteConnectionED cn = null)      // with an open database, case insensitive
        {
            SystemClass sys = null;
            bool closeit = false;

            try
            {
                if (cn == null)
                {
                    closeit = true;
                    cn = new SQLiteConnectionED();
                }

                using (DbCommand cmd = cn.CreateCommand("select * from Systems where name = @name limit 1"))   // 1 return matching name
                {
                    cmd.AddParameterWithValue("name", name);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            sys = new SystemClass(reader);
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

        public static List<SystemClass> GetSystemsByName(string name, SQLiteConnectionED cn = null)
        {
            List<SystemClass> systems = new List<SystemClass>();
            bool closeit = false;

            try
            {
                if (cn == null)
                {
                    closeit = true;
                    cn = new SQLiteConnectionED();
                }

                using (DbCommand cmd = cn.CreateCommand("select * from Systems where name = @name"))
                {
                    cmd.AddParameterWithValue("name", name);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            systems.Add(new SystemClass(reader));
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

            return systems;
        }

        public static SystemClass GetSystem(long id, SQLiteConnectionED cn = null, SystemIDType idtype = SystemIDType.id)      // using an id
        {
            SystemClass sys = null;
            bool closeit = false;

            try
            {
                if (cn == null)
                {
                    closeit = true;
                    cn = new SQLiteConnectionED();
                }

                using (DbCommand cmd = cn.CreateCommand("select * from Systems where " + idtype.ToString() + "=@id limit 1"))   // 1 return matching name
                {
                    cmd.AddParameterWithValue("id", id);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            sys = new SystemClass(reader);
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
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select Count(*) from Systems"))
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
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select versiondate from Systems Order By versiondate DESC limit 1"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read() && System.DBNull.Value != reader["versiondate"])
                                lasttime = (DateTime)reader["versiondate"];
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

        public static void TouchSystem(SQLiteConnectionED cn, string systemName)
        {
            using (DbCommand cmd = cn.CreateCommand("update systems set versiondate=datetime('now') where name=@systemName"))
            {
                cmd.AddParameterWithValue("systemName", systemName);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);
            }
        }

        public static void GetSystemSqDistancesFrom(SortedList<double, ISystem> distlist, double x, double y, double z, int maxitems, bool removezerodiststar, 
                                                    double maxdist = 200 , SQLiteConnectionED cn = null)
        {
            bool closeit = false;

            try
            {
                if (cn == null)
                {
                    closeit = true;
                    cn = new SQLiteConnectionED();
                }

                using (DbCommand cmd = cn.CreateCommand(
                    "SELECT * " +
                    "FROM Systems " +
                    "WHERE x >= @xv - @maxdist " +
                    "AND x <= @xv + @maxdist " +
                    "AND y >= @yv - @maxdist " +
                    "AND y <= @yv + @maxdist " +
                    "AND z >= @zv - @maxdist " +
                    "AND z <= @zv + @maxdist " +
                    "ORDER BY (x-@xv)*(x-@xv)+(y-@yv)*(y-@yv)+(z-@zv)*(z-@zv) " +
                    "LIMIT @max"))
                {
                    cmd.AddParameterWithValue("xv", x);
                    cmd.AddParameterWithValue("yv", y);
                    cmd.AddParameterWithValue("zv", z);
                    cmd.AddParameterWithValue("max", maxitems + 1);     // 1 more, because if we are on a star, that will be returned
                    cmd.AddParameterWithValue("maxdist", maxdist);

                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read() && distlist.Count < maxitems)           // already sorted, and already limited to max items
                        {
                            string name = ((string)reader["name"]);

                            if (System.DBNull.Value != reader["x"])                 // paranoid check for null
                            {
                                double dx = (double)reader["x"] - x;
                                double dy = (double)reader["y"] - y;
                                double dz = (double)reader["z"] - z;

                                double dist = dx * dx + dy * dy + dz * dz;
                                if (dist > 0.001 || !removezerodiststar)
                                    distlist.Add(dist, new SystemClass(reader));
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

        public static ISystem FindNearestSystem(double x, double y, double z, bool removezerodiststar = false, double maxdist = 1000, SQLiteConnectionED cn = null)
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
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    string sqlquery = "select id,name, x, y, z from Systems " +
                                      "where (x-@xw)*(x-@xw)+(y-@yw)*(y-@yw)+(z-@zw)*(z-@zw)<=@maxfromwanted AND " +
                                      "(x-@xc)*(x-@xc)+(y-@yc)*(y-@yc)+(z-@zc)*(z-@zc)<=@maxfromcurrent";

                    using (DbCommand cmd = cn.CreateCommand(sqlquery))
                    {
                        cmd.AddParameterWithValue("xw", wantedpos.X);
                        cmd.AddParameterWithValue("yw", wantedpos.Y);
                        cmd.AddParameterWithValue("zw", wantedpos.Z);
                        cmd.AddParameterWithValue("maxfromwanted", maxfromwanted * maxfromwanted);     //squared

                        cmd.AddParameterWithValue("xc", curpos.X);
                        cmd.AddParameterWithValue("yc", curpos.Y);
                        cmd.AddParameterWithValue("zc", curpos.Z);
                        cmd.AddParameterWithValue("maxfromcurrent", maxfromcurpos * maxfromcurpos);     //squared

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
                // Indexed sets of systems
                Dictionary<long, List<SystemClass>> systemsByVscId = new Dictionary<long, List<SystemClass>>();
                Dictionary<long, List<SystemClass>> systemsAliasesByVscId = new Dictionary<long, List<SystemClass>>();
                Dictionary<string, List<SystemClass>> systemsByName = new Dictionary<string, List<SystemClass>>();
                Dictionary<long, SystemClass> systemsByEdsmId = new Dictionary<long, SystemClass>();

                using (SQLiteConnectionED cn = new SQLiteConnectionED())
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

                    // Retrieve systems matching on name, position or EDSM ID.
                    // Having the database filter the systems is much quicker
                    // than having to sift through all of the systems
                    // ourselves.
                    using (DbCommand cmd = cn.CreateCommand(
                        "SELECT DISTINCT vsc.id AS vscid, a.id AS aliasid, s.* " +
                        "FROM VisitedSystems vsc " +
                        "LEFT JOIN SystemAliases a " +
                        "ON a.Name = vsc.Name " +
                        "OR a.id_edsm = vsc.id_edsm_assigned " +
                        "LEFT JOIN Systems s " +
                        "ON s.Name = vsc.Name " +
                        "OR s.id_edsm = vsc.id_edsm_assigned " +
                        "OR (s.X >= vsc.X - 0.125 AND s.X <= vsc.X + 0.125 AND s.Y >= vsc.Y - 0.125 AND s.Y <= vsc.Y + 0.125 AND s.Z >= vsc.Z - 0.125 AND s.Z <= vsc.Z + 0.125) " +
                        "OR s.id_edsm = a.id_edsm_mergedto " +
                        "WHERE s.id IS NOT NULL " +
                        "ORDER BY s.id ASC"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            long lastid = -1;
                            SystemClass sys = null;

                            while (reader.Read())
                            {
                                string name = ((string)reader["name"]).ToLower();
                                long id_edsm = 0;
                                long id = (long)reader["id"];
                                long vscid = (long)reader["vscid"];
                                bool isalias = false;

                                if (reader["aliasid"] != DBNull.Value)
                                {
                                    isalias = true;
                                }

                                if (id != lastid)
                                {
                                    sys = new SystemClass(reader);

                                    // Get name match
                                    if (!systemsByName.ContainsKey(name))
                                    {
                                        systemsByName[name] = new List<SystemClass>();
                                    }

                                    systemsByName[name].Add(sys);

                                    // Get EDSM ID match
                                    if (reader["id_edsm"] != System.DBNull.Value)
                                    {
                                        id_edsm = (long)reader["id_edsm"];

                                        if (sys == null)
                                        {
                                            sys = new SystemClass(reader);
                                        }

                                        systemsByEdsmId[id_edsm] = sys;
                                    }

                                    lastid = id;
                                }

                                // Get VSC ID match
                                if (isalias)
                                {
                                    if (!systemsAliasesByVscId.ContainsKey(vscid))
                                    {
                                        systemsAliasesByVscId[vscid] = new List<SystemClass>();
                                    }

                                    systemsAliasesByVscId[vscid].Add(sys);
                                }
                                else
                                {
                                    if (!systemsByVscId.ContainsKey(vscid))
                                    {
                                        systemsByVscId[vscid] = new List<SystemClass>();
                                    }

                                    systemsByVscId[vscid].Add(sys);
                                }
                            }
                        }
                    }
                }

                // Now that we have gathered the matching systems
                // we determine what and how well they match
                foreach (VisitedSystemsClass vsc in visitedSystems)
                {
                    string vsc_searchname = vsc.Name.ToLower();

                    if (vsc.curSystem == null)                                              // if not set before, look it up
                    {
                        List<SystemClass> posmatches = null;
                        List<SystemClass> nameposmatches = null;
                        List<SystemClass> namematches = null;
                        SystemClass edsmidmatch = null;
                        bool multimatch = false;
                        Dictionary<long, SystemClass> matches;
                        bool hastravcoords = vsc.HasTravelCoordinates &&
                            (vsc.Name.ToLower() == "sol" || vsc.X != 0 || vsc.Y != 0 || vsc.Z != 0);

                        if (systemsByVscId.ContainsKey(vsc.id))
                        {
                            matches = systemsByVscId[vsc.id].ToDictionary(s => s.id);
                        }
                        else
                        {
                            matches = new Dictionary<long, SystemClass>();
                        }

                        if (vsc.id_edsm_assigned != null && vsc.id_edsm_assigned != 0)
                        {
                            long id_edsm = (long)vsc.id_edsm_assigned;

                            if (systemsByEdsmId.ContainsKey(id_edsm) && systemsByEdsmId[id_edsm] != null)
                            {
                                edsmidmatch = systemsByEdsmId[id_edsm];
                                matches[edsmidmatch.id] = edsmidmatch;
                            }
                        }

                        if (hastravcoords)
                        {
                            posmatches = matches.Values.Where(s => s.x >= vsc.X - 0.125 && s.x <= vsc.X + 0.125 && s.y >= vsc.X - 0.125 && s.y <= vsc.Y + 0.125 && s.z >= vsc.Z - 0.125 && s.z <= vsc.Z + 0.125).ToList();
                            if (posmatches.Count >= 1)
                            {
                                nameposmatches = posmatches.Where(s => s.SearchName == vsc_searchname).ToList();

                                foreach (var sys in posmatches)
                                {
                                    matches[sys.id] = sys;
                                }
                            }
                        }

                        if (systemsByName.ContainsKey(vsc_searchname) && systemsByName[vsc_searchname].Count >= 1)
                        {
                            namematches = systemsByName[vsc_searchname];

                            foreach (var sys in namematches)
                            {
                                matches[sys.id] = sys;
                            }
                        }

                        if (systemsAliasesByVscId.ContainsKey(vsc.id))
                        {
                            foreach (var alt in systemsAliasesByVscId[vsc.id])
                            {
                                matches[alt.id] = alt;
                            }
                        }

                        vsc.alternatives = matches.Values.Select(s => (ISystem)s).ToList();

                        if (edsmidmatch != null)
                        {
                            SystemClass sys = edsmidmatch;
                            vsc.curSystem = sys;

                            if (sys.SearchName == vsc_searchname && hastravcoords && sys.x >= vsc.X - 0.125 && sys.x <= vsc.X + 0.125 && sys.y >= vsc.Y - 0.125 && sys.y <= vsc.Y + 0.125 && sys.z >= vsc.Z - 0.125 && sys.z <= vsc.Z + 0.125) // name and position matches
                            {
                                vsc.NameStatus = "Exact match";
                                continue; // Continue to next system
                            }
                            else if (hastravcoords && sys.x >= vsc.X - 0.125 && sys.x <= vsc.X + 0.125 && sys.y >= vsc.Y - 0.125 && sys.y <= vsc.Y + 0.125 && sys.z >= vsc.Z - 0.125 && sys.z <= vsc.Z + 0.125) // position matches
                            {
                                vsc.NameStatus = "Name differs";
                                continue; // Continue to next system
                            }
                            else if (!hastravcoords || !sys.HasCoordinate) // no coordinates available
                            {
                                if (sys.SearchName == vsc_searchname) // name matches
                                {
                                    if (!sys.HasCoordinate)
                                    {
                                        vsc.NameStatus = "System has no known coordinates";
                                    }
                                    else
                                    {
                                        vsc.NameStatus = "Travel log entry has no coordinates";
                                    }

                                    vsc.curSystem = sys;
                                    continue; // Continue to next system
                                }
                                else if (!vsc.HasTravelCoordinates)
                                {
                                    vsc.NameStatus = "Name differs";
                                }
                            }
                        }

                        if (nameposmatches != null)
                        {
                            if (nameposmatches.Count == 1)
                            {
                                // Both name and position matches
                                vsc.curSystem = nameposmatches[0];
                                vsc.NameStatus = "Exact match";
                                continue; // Continue to next system
                            }
                            else if (posmatches.Count == 1)
                            {
                                var sys = posmatches[0];

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

                        if (namematches != null)
                        {
                            if (namematches.Count == 1)
                            {
                                // One system name matched
                                vsc.curSystem = namematches[0];
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
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
        }

        public static long ParseEDSMUpdateSystemsString(string json, ref string date, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            using (StringReader sr = new StringReader(json))
                return ParseEDSMUpdateSystemsStream(sr, ref date, removenonedsmids, discoveryform, cancelRequested, reportProgress);
        }

        public static long ParseEDSMUpdateSystemsFile(string filename, ref string date, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            using (StreamReader sr = new StreamReader(filename))         // read directly from file..
                return ParseEDSMUpdateSystemsStream(sr, ref date, removenonedsmids, discoveryform, cancelRequested, reportProgress);
        }

        public static long ParseEDSMUpdateSystemsStream(TextReader sr, ref string date, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            using (JsonTextReader jr = new JsonTextReader(sr))
                return ParseEDSMUpdateSystemsReader(jr, ref date, removenonedsmids, discoveryform, cancelRequested, reportProgress);
        }

        private static Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase> GetEdsmSystemsLite(SQLiteConnectionED cn)
        {
            Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase> systemsByEdsmId = new Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase>();

            using (DbCommand cmd = cn.CreateCommand("SELECT id, id_edsm, name, x, y, z, UpdateDate FROM Systems WHERE id_edsm IS NOT NULL"))
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
                            sys.x = (double)reader["x"];
                            sys.y = (double)reader["y"];
                            sys.z = (double)reader["z"];
                        }

                        sys.id_edsm = (long)reader["id_edsm"];
                        systemsByEdsmId[sys.id_edsm] = sys;
                    }
                }
            }

            return systemsByEdsmId;
        }

        private static long DoParseEDSMUpdateSystemsReader(JsonTextReader jr, ref string date, SQLiteConnectionED cn, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            DateTime maxdate = DateTime.Parse(date, CultureInfo.InvariantCulture);
            Dictionary<long, EDDiscovery2.DB.InMemory.SystemClassBase> systemsByEdsmId = GetEdsmSystemsLite(cn);
            int count = 0;
            int updatecount = 0;
            int insertcount = 0;
            Random rnd = new Random();
            int[] histogramsystems = new int[50000];
            Stopwatch sw = Stopwatch.StartNew();

            while (!cancelRequested())
            {
                using (DbTransaction txn = cn.BeginTransaction())
                {
                    DbCommand updatecmd = null;
                    DbCommand insertcmd = null;

                    try
                    {
                        updatecmd = cn.CreateCommand("UPDATE Systems SET name=@name, x=@x, y=@y, z=@z, UpdateDate=@UpdateDate, gridid=@gridid, randomid=@randomid WHERE id_edsm=@id_edsm", txn);
                        updatecmd.AddParameter("@name", DbType.String);
                        updatecmd.AddParameter("@x", DbType.Double);
                        updatecmd.AddParameter("@y", DbType.Double);
                        updatecmd.AddParameter("@z", DbType.Double);
                        updatecmd.AddParameter("@UpdateDate", DbType.DateTime);
                        updatecmd.AddParameter("@gridid", DbType.Int64);
                        updatecmd.AddParameter("@randomid", DbType.Int64);
                        updatecmd.AddParameter("@id_edsm", DbType.Int64);

                        insertcmd = cn.CreateCommand(
                            "INSERT INTO Systems " +
                            "(name, x, y, z, CreateDate, UpdateDate, Status, versiondate, id_edsm, gridid, randomid, cr) VALUES " +
                            "(@name, @x, @y, @z, @CreateDate, @UpdateDate, @Status, CURRENT_TIMESTAMP, @id_edsm, @gridid, @randomid, 0)",
                            txn);
                        insertcmd.AddParameter("@name", DbType.String);
                        insertcmd.AddParameter("@x", DbType.Double);
                        insertcmd.AddParameter("@y", DbType.Double);
                        insertcmd.AddParameter("@z", DbType.Double);
                        insertcmd.AddParameter("@CreateDate", DbType.DateTime);
                        insertcmd.AddParameter("@UpdateDate", DbType.DateTime);
                        insertcmd.AddParameter("@Status", DbType.Int64);
                        insertcmd.AddParameter("@id_edsm", DbType.Int64);
                        insertcmd.AddParameter("@gridid", DbType.Int64);
                        insertcmd.AddParameter("@randomid", DbType.Int64);

                        while (!cancelRequested())
                        {
                            if (!jr.Read())
                            {
                                reportProgress(-1, $"Syncing EDSM systems: {count} processed, {insertcount} new systems, {updatecount} updated systems");
                                date = maxdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
                                txn.Commit();

                                Console.WriteLine($"Import took {sw.ElapsedMilliseconds}ms");

                                for (int id = 0; id < histogramsystems.Length; id++)
                                {
                                    if (histogramsystems[id] != 0)
                                        Console.WriteLine("Id " + id + " count " + histogramsystems[id]);
                                }

                                return updatecount + insertcount;
                            }

                            if (jr.TokenType == JsonToken.StartObject)
                            {
                                JObject jo = JObject.Load(jr);

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

                                    if (systemsByEdsmId.ContainsKey(edsmid))
                                    {
                                        var dbsys = systemsByEdsmId[edsmid];
                                        // see if EDSM data changed..
                                        if (!dbsys.name.Equals(name) || Math.Abs(dbsys.x - x) > 0.01 || Math.Abs(dbsys.y - y) > 0.01 || Math.Abs(dbsys.z - z) > 0.01)  // name or position changed
                                        {
                                            updatecmd.Parameters["@name"].Value = name;
                                            updatecmd.Parameters["@x"].Value = x;
                                            updatecmd.Parameters["@y"].Value = y;
                                            updatecmd.Parameters["@z"].Value = z;
                                            updatecmd.Parameters["@UpdateDate"].Value = updatedate;
                                            updatecmd.Parameters["@gridid"].Value = gridid;
                                            updatecmd.Parameters["@randomid"].Value = randomid;
                                            updatecmd.Parameters["@id_edsm"].Value = edsmid;
                                            updatecmd.ExecuteNonQuery();
                                            updatecount++;
                                        }
                                    }
                                    else                                                                  // not in database..
                                    {
                                        insertcmd.Parameters["@name"].Value = name;
                                        insertcmd.Parameters["@x"].Value = x;
                                        insertcmd.Parameters["@y"].Value = y;
                                        insertcmd.Parameters["@z"].Value = z;
                                        insertcmd.Parameters["@CreateDate"].Value = updatedate;
                                        insertcmd.Parameters["@UpdateDate"].Value = updatedate;
                                        insertcmd.Parameters["@Status"].Value = (int)SystemStatusEnum.EDSC;
                                        insertcmd.Parameters["@id_edsm"].Value = edsmid;
                                        insertcmd.Parameters["@gridid"].Value = gridid;
                                        insertcmd.Parameters["@randomid"].Value = randomid;
                                        insertcmd.ExecuteNonQuery();
                                        insertcount++;
                                    }
                                }

                                if ((++count) % 10000 == 0)
                                {
                                    reportProgress(-1, $"Syncing EDSM systems: {count} processed, {insertcount} new systems, {updatecount} updated systems");
                                    txn.Commit();
                                    break;
                                }
                            }
                        }
                    }
                    finally
                    {
                        if (updatecmd != null) updatecmd.Dispose();
                        if (insertcmd != null) insertcmd.Dispose();
                    }
                }
            }

            return updatecount + insertcount;
        }

        private static long ParseEDSMUpdateSystemsReader(JsonTextReader jr, ref string date, bool removenonedsmids, EDDiscoveryForm discoveryform, Func<bool> cancelRequested, Action<int, string> reportProgress)
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())  // open the db
            {
                long count = DoParseEDSMUpdateSystemsReader(jr, ref date, cn, discoveryform, cancelRequested, reportProgress);

                if (removenonedsmids)                            // done on a full sync..
                {
                    Console.WriteLine("Delete old ones");
                    using (DbCommand cmddel = cn.CreateCommand("Delete from Systems where id_edsm is null"))
                    {
                        cmddel.ExecuteNonQuery();
                    }
                }

                return count;
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

            using (SQLiteConnectionED cn2 = new SQLiteConnectionED())  // open the db
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

                        SystemClass cs = GetSystem(edsmid, null, SystemIDType.id_edsm);   // test before delete..
                        if (cs != null)
                        {
                            Console.Write("Remove " + edsmid);
                            Delete(edsmid, cn2, null, SystemIDType.id_edsm);             // and wack!
                        }

                        if (mergedto > 0)
                        {
                            using (DbCommand cmd = cn2.CreateCommand("INSERT OR IGNORE INTO SystemAliases (name, id_edsm, id_edsm_mergedto) VALUES (@name, @id_edsm, @id_edsm_mergedto)"))
                            {
                                cmd.AddParameterWithValue("@name", name);
                                cmd.AddParameterWithValue("@id_edsm", edsmid);
                                cmd.AddParameterWithValue("@id_edsm_mergedto", mergedto);
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }


        static public long ParseEDDBUpdateSystems(string filename)
        {
            StreamReader sr = new StreamReader(filename);         // read directly from file..

            if (sr == null)
                return 0;

            JsonTextReader jr = new JsonTextReader(sr);

            if (jr == null)
                return 0;

            List<SystemClass> toupdate = new List<SystemClass>();

            using (SQLiteConnectionED cn = new SQLiteConnectionED())  // open the db
            {
                DbCommand cmd1 = null;

                try
                {
                    cmd1 = cn.CreateCommand("select * from Systems where id_edsm = @id limit 1");   // 1 return matching ID

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

                                cmd1.Parameters.Clear();
                                cmd1.AddParameterWithValue("id", system.id_edsm);           // EDDB carries EDSM ID, so find entry in dB

                                //DEBUGif ( c > 30000 )  Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " Late info system");

                                using (DbDataReader reader1 = cmd1.ExecuteReader())         // if found (if not, we ignore EDDB system)
                                {
                                    if (reader1.Read())                                     // its there.. check its got the right stuff in it.
                                    {
                                        SystemClass dbsys = new SystemClass(reader1);

                                        if (dbsys.eddb_updated_at != system.eddb_updated_at || dbsys.population != system.population)
                                        {
                                            dbsys.id_eddb = system.id_eddb;
                                            dbsys.faction = system.faction;
                                            dbsys.population = system.population;
                                            dbsys.government = system.government;
                                            dbsys.allegiance = system.allegiance;
                                            dbsys.state = system.state;
                                            dbsys.security = system.security;
                                            dbsys.primary_economy = system.primary_economy;
                                            dbsys.needs_permit = system.needs_permit;
                                            dbsys.eddb_updated_at = system.eddb_updated_at;

                                            toupdate.Add(dbsys);                                // add to update list
                                        }
                                    }
                                    else
                                    {
                                        //                                        Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " Not found");
                                    }
                                }
                            }
                            else
                            {
                                //Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " No info reject");
                            }

                            if (++c % 10000 == 0)
                            {
                                Console.WriteLine("EDDB Count " + c + " Delta " + (Environment.TickCount - lasttc) + " info " + hasinfo + " update " + toupdate.Count());
                                lasttc = Environment.TickCount;
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("There is a problem using the EDDB systems file." + Environment.NewLine +
                                    "Please perform a manual EDDB sync (see Admin menu) next time you run the program ", "EDDB Sync Error");
                }
                finally
                {
                    if (cmd1 != null) cmd1.Dispose();
                }
            }

            using (SQLiteConnectionED cn2 = new SQLiteConnectionED())  // open the db
            {
                if (toupdate.Count > 0)
                {
                    int count = 0;

                    while (count < toupdate.Count())
                    {
                        using (DbTransaction transaction = cn2.BeginTransaction())
                        {
                            while (count < toupdate.Count())
                            {
                                toupdate[count].Update(cn2, toupdate[count].id, transaction);       // update existing entry..

                                if (++count % 10000 == 0)
                                    break;
                            }

                            Console.WriteLine("EDDB Store Count " + count);
                            transaction.Commit();
                        }
                    }
                }
            }

            return toupdate.Count();
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
                                                19                                      // 40   +20
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
                                                25,                                     // 70 +60
                                            };
        public const int xleft = -20500;
        public const int xright = 20000;
        public const int zbot = -10500;
        public const int ztop = 60000;

        public static int Id(double x, double z)
        {
            x = Math.Min(Math.Max(x - xleft, 0), xright - xleft);
            z = Math.Min(Math.Max(z - zbot, 0), ztop - zbot);
            x /= 1000;
            z /= 1000;
            return compresstablex[(int)x] + 100 * compresstablez[(int)z];
        }

        public static int IdFromComponents(int x, int z)
        {
            return x + 100 * z;
        }

        public static bool XZ(int id, out double x, out double z)
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

                            x = (((i * 1000) + xleft) + startx) / 2.0;
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

                            z = (((i * 1000) + zbot) + startz) / 2.0;
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
