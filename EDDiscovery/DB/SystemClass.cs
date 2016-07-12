using EDDiscovery2;
using EDDiscovery2.DB;
using EMK.LightGeometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public enum SystemStatusEnum
    {
        Unknown = 0,
        EDSC = 1,
        RedWizzard =2,
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

                    /*CommanderCreate = jo["commandercreate"].Value<string>();
                    CreateDate = jo["createdate"].Value<DateTime>();
                    CommanderUpdate = jo["commanderupdate"].Value<string>();
                    UpdateDate = jo["updatedate"].Value<DateTime>();*/

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
            using (DbCommand cmd = cn.CreateCommand("Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note, id_eddb, population, faction, government_id, allegiance_id, primary_economy_id, security, eddb_updated_at, state, needs_permit, versiondate, id_edsm) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note, @id_eddb, @population, @faction, @government_id, @allegiance_id, @primary_economy_id,  @security, @eddb_updated_at, @state, @needs_permit, datetime('now'),@id_edsm)"))
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
                }
                else
                {       // override the cmd.
                    cmd.CommandText = "Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note, versiondate,id_edsm) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note, datetime('now'),@id_edsm)";
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
                }

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public bool Update(SQLiteConnectionED cn, long id, DbTransaction transaction)
        {
            using (DbCommand cmd = cn.CreateCommand("Update Systems set name=@name, x=@x, y=@y, z=@z, cr=@cr, commandercreate=@commandercreate, createdate=@createdate, commanderupdate=@commanderupdate, updatedate=@updatedate, status=@status, note=@Note, id_eddb=@id_eddb, population=@population, faction=@faction, government_id=@government_id, allegiance_id=@allegiance_id, primary_economy_id=@primary_economy_id,  security=@security, eddb_updated_at=@eddb_updated_at, state=@state, needs_permit=@needs_permit, versiondate=datetime('now'), id_edsm=@id_edsm where ID=@id",  transaction))
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

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public bool UpdateEDSM(SQLiteConnectionED cn, long id, DbTransaction transaction)     // only altering fields EDSM sets..
        {
            using (DbCommand cmd = cn.CreateCommand("Update Systems set name=@name, x=@x, y=@y, z=@z, versiondate=datetime('now') where ID=@id",  transaction))
            {
                cmd.AddParameterWithValue("@id", id);
                cmd.AddParameterWithValue("@name", name);
                cmd.AddParameterWithValue("@x", x);
                cmd.AddParameterWithValue("@y", y);
                cmd.AddParameterWithValue("@z", z);
                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public enum SystemIDType { id, id_edsm, id_eddb };       // which ID to match?

        public static bool Delete(long id, SQLiteConnectionED cn = null, DbTransaction transaction = null, SystemIDType idtype = SystemIDType.id)
        {
            using (DbCommand cmd = cn.CreateCommand("Delete from Systems where " + idtype.ToString() + "=@id",  transaction))
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

        public static double DistanceIncludeDB(EDDiscovery2.DB.ISystem s1, EDDiscovery2.DB.ISystem s2)
        {
            double dist = Distance(s1, s2);
            if (dist < 0)
                dist = DistanceClass.FindDistance(s1, s2);
            return dist;
        }

        public static void GetSystemNamesList(List<SystemClassStarNames> snlist , Dictionary<string, SystemClassStarNames> dict )
        {
            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select id,name,x,y,z,population from Systems"))
                    {
                        using (DbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string name = (string)reader["name"];
                                long population = (System.DBNull.Value == reader["population"]) ? 0 : ((long)reader["population"]);

                                if (System.DBNull.Value != reader["x"])
                                {
                                    SystemClassStarNames sys = new SystemClassStarNames(name, (double)reader["x"], (double)reader["y"], (double)reader["z"], population, (long)reader["id"]);

                                    if (!dict.ContainsKey(sys.name))    // protect against crap ups in the star list having duplicate names
                                        dict.Add(sys.name, sys);

                                    snlist.Add(sys);
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
        public static void ProcessStars(StarCallBack cb )  // return star positions..
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

        public static SystemClass GetSystem(string name, SQLiteConnectionED cn = null )      // with an open database, case insensitive
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

        public static SystemClass GetSystem(long id, SQLiteConnectionED cn = null, SystemIDType idtype = SystemIDType.id )      // using an id
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

        public static void GetSystemSqDistancesFrom(SortedList<double, string> distlist, double x, double y, double z, int maxitems, bool removezerodiststar)
        {
            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select name,x,y,z from Systems Order By (x-@xv)*(x-@xv)+(y-@yv)*(y-@yv)+(z-@zv)*(z-@zv) Limit @max"))
                    {
                        cmd.AddParameterWithValue("xv", x);
                        cmd.AddParameterWithValue("yv", y);
                        cmd.AddParameterWithValue("zv", z);
                        cmd.AddParameterWithValue("max", maxitems+1);     // 1 more, because if we are on a star, that will be returned

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
                                        distlist.Add(dist, name);
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

        public static SystemClass FindNearestSystem(double x, double y, double z, bool removezerodiststar = false)
        {
            SortedList<double, string> distlist = new SortedList<double, string>();
            GetSystemSqDistancesFrom(distlist, x, y, z, 1, removezerodiststar);
            return (distlist.Count > 0) ? GetSystem(distlist.First().Value) : null;
        }

        public const int metric_nearestwaypoint = 0;     // easiest way to synchronise metric selection..
        public const int metric_mindevfrompath = 1;
        public const int metric_maximum100ly = 2;
        public const int metric_maximum250ly = 3;
        public const int metric_maximum500ly = 4;
        public const int metric_waypointdev2 = 5;

        public static SystemClass GetSystemNearestTo(Point3D curpos, Point3D wantedpos, double maxfromcurpos, double maxfromwanted,
                                    int routemethod )
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
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    DbCommand cmd = cn.CreateCommand("select * from Systems where name = @name limit 1");

                    foreach (VisitedSystemsClass vsc in visitedSystems)
                    {
                        if (vsc.curSystem == null)                                              // if not set before, look it up
                        {
                            cmd.Parameters.Clear();
                            cmd.AddParameterWithValue("name", vsc.Name);

                            using (DbDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                    vsc.curSystem = new SystemClass(reader);
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

        public static long ParseEDSMUpdateSystemsString(string json, ref string date, bool removenonedsmids)
        {
            JsonTextReader jr = new JsonTextReader(new StringReader(json));
            return ParseEDSMUpdateSystemsReader(jr, ref date, removenonedsmids);
        }

        public static long ParseEDSMUpdateSystemsFile(string filename, ref string date , bool removenonedsmids)
        {
            StreamReader sr = new StreamReader(filename);         // read directly from file..
            JsonTextReader jr = new JsonTextReader(sr);
            return ParseEDSMUpdateSystemsReader(jr, ref date,removenonedsmids);
        }

 
        private static long ParseEDSMUpdateSystemsReader(JsonTextReader jr, ref string date, bool removenonedsmids)
        {
            List<SystemClass> toupdate = new List<SystemClass>();
            List<SystemClass> newsystems = new List<SystemClass>();
            DateTime maxdate = DateTime.Parse(date, new CultureInfo("sv-SE"));

            bool emptydatabase = SystemClass.GetTotalSystems() == 0;            // if empty database, we can skip the lookup

            using (SQLiteConnectionED cn = new SQLiteConnectionED())  // open the db
            {
                int c = 0;

                try
                {
                    using (DbCommand cmd = cn.CreateCommand("select * from Systems where id_edsm = @id_edsm limit 1"))   // 1 return matching EDSM ID
                    {
                        int lasttc = Environment.TickCount;

                        while (jr.Read())
                        {
                            if (jr.TokenType == JsonToken.StartObject)
                            {
                                JObject jo = JObject.Load(jr);

                                SystemClass system = new SystemClass(jo, EDDiscovery2.DB.SystemInfoSource.EDSM);

                                if (system.UpdateDate.Subtract(maxdate).TotalSeconds > 0)
                                    maxdate = system.UpdateDate;

                                if (++c % 10000 == 0)
                                {
                                    Console.WriteLine("EDSM Count " + c + " Delta " + (Environment.TickCount - lasttc) + " newsys " + newsystems.Count + " update " + toupdate.Count());
                                    lasttc = Environment.TickCount;
                                }

                                if (system.HasCoordinate)
                                {
                                    if (emptydatabase)      // if no database, just add immediately
                                    {
                                        //Console.WriteLine("Empty database Add new system " + system.name);
                                        newsystems.Add(system);
                                    }
                                    else
                                    {
                                        cmd.Parameters.Clear();
                                        cmd.AddParameterWithValue("id_edsm", system.id_edsm);

                                        using (DbDataReader reader1 = cmd.ExecuteReader())              // see if ESDM ID is there..
                                        {
                                            if (reader1.Read())                                          // its there..
                                            {
                                                SystemClass dbsys = new SystemClass(reader1);
                                                // see if EDSM data changed..
                                                if (!dbsys.name.Equals(system.name) || Math.Abs(dbsys.x - system.x) > 0.01 || Math.Abs(dbsys.y - system.y) > 0.01 || Math.Abs(dbsys.z - system.z) > 0.01)  // name or position changed
                                                {
                                                    dbsys.x = system.x;
                                                    dbsys.y = system.y;
                                                    dbsys.z = system.z;
                                                    dbsys.name = system.name;

                                                    //Console.WriteLine("Update " + dbsys.id + " due to pos or case " + dbsys.name);
                                                    toupdate.Add(dbsys);
                                                }
                                            }
                                            else                                                                  // not in database..
                                            {
                                                //Console.WriteLine("Add new system " + system.name);
                                                newsystems.Add(system);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch           // any errors abort
                { }
            }

            using (SQLiteConnectionED cn2 = new SQLiteConnectionED())  // open the db
            {
                if (toupdate.Count > 0)
                {
                    using (DbTransaction transaction = cn2.BeginTransaction())
                    {
                        foreach (SystemClass sys in toupdate)
                            sys.UpdateEDSM(cn2, sys.id, transaction);        // do an EDSM update of only name/x/y/z, not expected to be many at a time

                        transaction.Commit();
                    }
                }

                if (newsystems.Count > 0)
                {
                    int count = 0;

                    while (count < newsystems.Count())
                    {
                        using (DbTransaction transaction = cn2.BeginTransaction())
                        {
                            while (count < newsystems.Count())
                            {
                                newsystems[count].Store(cn2, transaction);

                                if (++count % 100000 == 0)
                                    break;
                            }

                            Console.WriteLine("EDSM Store Count " + count);
                            transaction.Commit();
                        }
                    }
                }

                if (removenonedsmids)                            // done on a full sync..
                {
                    Console.WriteLine("Delete old ones");
                    using (DbCommand cmddel = cn2.CreateCommand("Delete from Systems where id_edsm is null"))
                    {
                        SQLiteDBClass.SQLNonQueryText(cn2, cmddel);
                    }
                }
            }

            date = maxdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return toupdate.Count + newsystems.Count;
        }

        public static void RemoveHiddenSystems(string json)
        {
            JsonTextReader jr = new JsonTextReader(new StringReader(json));

            while (jr.Read())
            {
                if (jr.TokenType == JsonToken.StartObject)
                {
                    JObject jo = JObject.Load(jr);

                    long edsmid = (long)jo["id"];

                    SystemClass cs = GetSystem(edsmid, null, SystemIDType.id_edsm);   // test before delete..
                    if (cs != null)
                    {
                        Console.Write("Remove " + edsmid);
                        Delete(edsmid, null, null, SystemIDType.id_edsm);             // and wack!
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
                    int lasttc = Environment.TickCount;

                    while (jr.Read())
                    {
                        if (jr.TokenType == JsonToken.StartObject)
                        {
                            JObject jo = JObject.Load(jr);

                            SystemClass system = new SystemClass(jo, EDDiscovery2.DB.SystemInfoSource.EDDB);

                            cmd1.Parameters.Clear();
                            cmd1.AddParameterWithValue("id", system.id_edsm);           // EDDB carries EDSM ID, so find entry in dB

                            using (DbDataReader reader1 = cmd1.ExecuteReader())         // if found (if not, we ignore EDDB system)
                            {
                                if (reader1.Read())                                     // its there.. check its got the right stuff in it.
                                {
                                    SystemClass dbsys = new SystemClass(reader1);       // we always do an update.  if EDDB entry is not in DB, we ignore it

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
                                    Console.WriteLine("EDDB ID " + system.id_eddb + " EDSM ID " + system.id_edsm + " " + system.name + " Not found");
                                }
                            }

                            if (++c % 10000 == 0)
                            {
                                Console.WriteLine("EDDB Count " + c + " Delta " + (Environment.TickCount - lasttc) + " update " + toupdate.Count());
                                lasttc = Environment.TickCount;
                            }
                        }
                    }
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

                                if (++count % 100000 == 0)
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
}
