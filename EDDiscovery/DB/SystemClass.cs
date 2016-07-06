using EDDiscovery2;
using EMK.LightGeometry;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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
            Note = dr["Note"].ToString();

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

        public SystemClass(SQLiteDataReader dr)         // read from a SQLite reader after a query
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
            Note = dr["Note"].ToString();

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
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                cn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "Delete from Systems where Status=@Status";
                    cmd.Parameters.AddWithValue("@Status", (int)source);

                    SQLiteDBClass.SqlNonQueryText(cn, cmd);
                }

                cn.Close();
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


        public bool Store(SQLiteConnection cn, SQLiteTransaction transaction)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.Transaction = transaction;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;

                if (Note == null)
                    Note = "";

                if (id_eddb != 0)
                {
                    cmd.CommandText = "Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note, id_eddb, population, faction, government_id, allegiance_id, primary_economy_id, security, eddb_updated_at, state, needs_permit, versiondate, id_edsm) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note, @id_eddb, @population, @faction, @government_id, @allegiance_id, @primary_economy_id,  @security, @eddb_updated_at, @state, @needs_permit, datetime('now'),@id_edsm)";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@x", x);
                    cmd.Parameters.AddWithValue("@y", y);
                    cmd.Parameters.AddWithValue("@z", z);
                    cmd.Parameters.AddWithValue("@cr", cr);
                    cmd.Parameters.AddWithValue("@CommanderCreate", CommanderCreate);
                    cmd.Parameters.AddWithValue("@Createdate", CreateDate);
                    cmd.Parameters.AddWithValue("@CommanderUpdate", CommanderCreate);
                    cmd.Parameters.AddWithValue("@updatedate", CreateDate);
                    cmd.Parameters.AddWithValue("@Status", (int)status);


                    cmd.Parameters.AddWithValue("@id_eddb", id_eddb);
                    cmd.Parameters.AddWithValue("@population", population);
                    cmd.Parameters.AddWithValue("@faction", faction);
                    cmd.Parameters.AddWithValue("@government_id", government);
                    cmd.Parameters.AddWithValue("@allegiance_id", allegiance);
                    cmd.Parameters.AddWithValue("@primary_economy_id", primary_economy);
                    cmd.Parameters.AddWithValue("@security", security);
                    cmd.Parameters.AddWithValue("@eddb_updated_at", eddb_updated_at);
                    cmd.Parameters.AddWithValue("@state", state);
                    cmd.Parameters.AddWithValue("@needs_permit", needs_permit);
                    cmd.Parameters.AddWithValue("@Note", Note);
                    cmd.Parameters.AddWithValue("@id_edsm", id_edsm);
                }
                else
                {
                    cmd.CommandText = "Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note, versiondate,id_edsm) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note, datetime('now'),@id_edsm)";
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@x", x);
                    cmd.Parameters.AddWithValue("@y", y);
                    cmd.Parameters.AddWithValue("@z", z);
                    cmd.Parameters.AddWithValue("@cr", cr);
                    cmd.Parameters.AddWithValue("@CommanderCreate", CommanderCreate);
                    cmd.Parameters.AddWithValue("@Createdate", CreateDate);
                    cmd.Parameters.AddWithValue("@CommanderUpdate", CommanderCreate);
                    cmd.Parameters.AddWithValue("@updatedate", CreateDate);
                    cmd.Parameters.AddWithValue("@Status", (int)status);
                    cmd.Parameters.AddWithValue("@Note", Note);
                    cmd.Parameters.AddWithValue("@id_edsm", id_edsm);
                }

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                return true;
            }
        }

        public bool Update(SQLiteConnection cn, long id, SQLiteTransaction transaction)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("Update", cn, transaction))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update Systems set name=@name, x=@x, y=@y, z=@z, cr=@cr, commandercreate=@commandercreate, createdate=@createdate, commanderupdate=@commanderupdate, updatedate=@updatedate, status=@status, note=@Note, id_eddb=@id_eddb, population=@population, faction=@faction, government_id=@government_id, allegiance_id=@allegiance_id, primary_economy_id=@primary_economy_id,  security=@security, eddb_updated_at=@eddb_updated_at, state=@state, needs_permit=@needs_permit, versiondate=datetime('now'), id_edsm=@id_edsm where ID=@id";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@x", x);
                cmd.Parameters.AddWithValue("@y", y);
                cmd.Parameters.AddWithValue("@z", z);
                cmd.Parameters.AddWithValue("@cr", cr);
                cmd.Parameters.AddWithValue("@CommanderCreate", CommanderCreate);
                cmd.Parameters.AddWithValue("@Createdate", CreateDate);
                cmd.Parameters.AddWithValue("@CommanderUpdate", CommanderCreate);
                cmd.Parameters.AddWithValue("@updatedate", CreateDate);
                cmd.Parameters.AddWithValue("@Status", (int)status);
                if (Note == null)
                    Note = "";
                cmd.Parameters.AddWithValue("@Note", Note);


                cmd.Parameters.AddWithValue("@id_eddb", id_eddb);
                cmd.Parameters.AddWithValue("@population", population);
                cmd.Parameters.AddWithValue("@faction", faction);
                cmd.Parameters.AddWithValue("@government_id", government);
                cmd.Parameters.AddWithValue("@allegiance_id", allegiance);
                cmd.Parameters.AddWithValue("@primary_economy_id", primary_economy);
                cmd.Parameters.AddWithValue("@security", security);
                cmd.Parameters.AddWithValue("@eddb_updated_at", eddb_updated_at);
                cmd.Parameters.AddWithValue("@state", state);
                cmd.Parameters.AddWithValue("@needs_permit", needs_permit);

                cmd.Parameters.AddWithValue("@id_edsm", id_edsm);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                return true;
            }
        }

        public bool UpdateEDSM(SQLiteConnection cn, long id, SQLiteTransaction transaction)     // only altering fields EDSM sets..
        {
            using (SQLiteCommand cmd = new SQLiteCommand("Update", cn, transaction))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update Systems set name=@name, x=@x, y=@y, z=@z, versiondate=datetime('now') where ID=@id";

                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@x", x);
                cmd.Parameters.AddWithValue("@y", y);
                cmd.Parameters.AddWithValue("@z", z);
                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                return true;
            }
        }

        public enum SystemIDType { id, id_edsm, id_eddb };       // which ID to match?

        public static bool Delete(long id, SQLiteConnection cn = null, SQLiteTransaction transaction = null, SystemIDType idtype = SystemIDType.id)
        {
            bool closeit = false;
            if (cn == null)
            {
                cn = new SQLiteConnection(SQLiteDBClass.ConnectionString);
                cn.Open();
                closeit = true;
            }

            using (SQLiteCommand cmd = new SQLiteCommand("Delete from Systems where " + idtype.ToString() + "=@id", cn, transaction))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.Parameters.AddWithValue("@id", id);
                SQLiteDBClass.SqlNonQueryText(cn, cmd);
            }

            if (closeit)
            {
                cn.Close();
                cn.Dispose();
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
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("select id,name,x,y,z,population from Systems", cn))
                    {
                        cmd.CommandTimeout = 30;
                        cn.Open();

                        SQLiteDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string name = (string)reader["name"];
                            long population = (System.DBNull.Value == reader["population"]) ? 0 : ((long)reader["population"]);

                            if (System.DBNull.Value != reader["x"] )
                            {
                                SystemClassStarNames sys = new SystemClassStarNames(name, (double)reader["x"], (double)reader["y"], (double)reader["z"], population, (long)reader["id"]);

                                if (!dict.ContainsKey(sys.name))    // protect against crap ups in the star list having duplicate names
                                    dict.Add(sys.name, sys);

                                snlist.Add(sys);
                            }
                        }

                        cn.Close();
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
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("select name from Systems", cn))
                    {
                        cmd.CommandTimeout = 30;
                        cn.Open();

                        SQLiteDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            asc.Add((string)reader["name"]);
                        }

                        cn.Close();
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
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("select x,y,z from Systems", cn))
                    {
                        cmd.CommandTimeout = 30;
                        cn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            if (System.DBNull.Value != reader["x"])
                                list.Add(new Point3D((double)reader["x"], (double)reader["y"], (double)reader["z"]));
                        }
                        cn.Close();
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
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("select * from Systems", cn))
                    {
                        cmd.CommandTimeout = 30;
                        cn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            SystemClass sys = new SystemClass(reader);
                            cb(sys);
                        }
                        cn.Close();
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
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("select name from Systems", cn))
                    {
                        cmd.CommandTimeout = 30;
                        cn.Open();

                        SQLiteDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            string name = ((string)reader["name"]).ToUpper();
                            if (!dict.ContainsKey(name))
                                dict.Add(name, (int)reader["id"]);
                        }

                        cn.Close();
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

        public static SystemClass GetSystem(string name, SQLiteConnection cn = null )      // with an open database, case insensitive
        {
            SystemClass sys = null;

            try
            {
                bool closeit = false;
                if (cn == null)
                {
                    cn = new SQLiteConnection(SQLiteDBClass.ConnectionString);
                    cn.Open();
                    closeit = true;
                }

                using (SQLiteCommand cmd = new SQLiteCommand("select * from Systems where name = @name limit 1", cn))   // 1 return matching name
                {
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.CommandTimeout = 30;
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        sys = new SystemClass(reader);
                        if (SQLiteDBClass.globalSystemNotes.ContainsKey(sys.SearchName))
                            sys.Note = SQLiteDBClass.globalSystemNotes[sys.SearchName].Note;
                    }
                }

                if (closeit)
                {
                    cn.Close();
                    cn.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return sys;
        }

        public static SystemClass GetSystem(long id, SQLiteConnection cn = null, SystemIDType idtype = SystemIDType.id )      // using an id
        {
            SystemClass sys = null;

            try
            {
                bool closeit = false;
                if (cn == null)
                {
                    cn = new SQLiteConnection(SQLiteDBClass.ConnectionString);
                    cn.Open();
                    closeit = true;
                }

                using (SQLiteCommand cmd = new SQLiteCommand("select * from Systems where " + idtype.ToString() + "=@id limit 1", cn))   // 1 return matching name
                {
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.CommandTimeout = 30;
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        sys = new SystemClass(reader);
                        if (SQLiteDBClass.globalSystemNotes.ContainsKey(sys.SearchName))
                            sys.Note = SQLiteDBClass.globalSystemNotes[sys.SearchName].Note;
                    }
                }

                if (closeit)
                {
                    cn.Close();
                    cn.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return sys;
        }

        public static long GetTotalSystems()
        {
            long value = 0;

            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("select Count(*) from Systems", cn))
                    {
                        cmd.CommandTimeout = 30;
                        cn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                            value = (long)reader["Count(*)"];

                        cn.Close();
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
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("select versiondate from Systems Order By versiondate DESC limit 1", cn))
                    {
                        cmd.CommandTimeout = 30;
                        cn.Open();
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        if (reader.Read() && System.DBNull.Value != reader["versiondate"])
                            lasttime = (DateTime)reader["versiondate"];

                        cn.Close();
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

        public static void TouchSystem(SQLiteConnection connection, string systemName)
        {
            using (SQLiteCommand cmd = connection.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "update systems set versiondate=datetime('now') where name=@systemName";
                cmd.Parameters.AddWithValue("systemName", systemName);
                SQLiteDBClass.SqlNonQueryText(connection, cmd);
            }
        }

        public static void GetSystemSqDistancesFrom(SortedList<double, string> distlist, double x, double y, double z, int maxitems, bool removezerodiststar)
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand("select name,x,y,z from Systems Order By (x-@xv)*(x-@xv)+(y-@yv)*(y-@yv)+(z-@zv)*(z-@zv) Limit @max", cn))
                    {
                        cmd.Parameters.AddWithValue("xv", x);
                        cmd.Parameters.AddWithValue("yv", y);
                        cmd.Parameters.AddWithValue("zv", z);
                        cmd.Parameters.AddWithValue("max", maxitems+1);     // 1 more, because if we are on a star, that will be returned

                        cmd.CommandTimeout = 30;
                        cn.Open();

                        SQLiteDataReader reader = cmd.ExecuteReader();
                        while (reader.Read() && distlist.Count < maxitems )           // already sorted, and already limited to max items
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

                        cn.Close();
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
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    string sqlquery = "select id,name, x, y, z from Systems " +
                                      "where (x-@xw)*(x-@xw)+(y-@yw)*(y-@yw)+(z-@zw)*(z-@zw)<=@maxfromwanted AND " +
                                      "(x-@xc)*(x-@xc)+(y-@yc)*(y-@yc)+(z-@zc)*(z-@zc)<=@maxfromcurrent";
                    
                    using (SQLiteCommand cmd = new SQLiteCommand(sqlquery, cn))
                    {
                        cmd.Parameters.AddWithValue("xw", wantedpos.X);
                        cmd.Parameters.AddWithValue("yw", wantedpos.Y);
                        cmd.Parameters.AddWithValue("zw", wantedpos.Z);
                        cmd.Parameters.AddWithValue("maxfromwanted", maxfromwanted * maxfromwanted);     //squared

                        cmd.Parameters.AddWithValue("xc", curpos.X);
                        cmd.Parameters.AddWithValue("yc", curpos.Y);
                        cmd.Parameters.AddWithValue("zc", curpos.Z);
                        cmd.Parameters.AddWithValue("maxfromcurrent", maxfromcurpos * maxfromcurpos);     //squared

                        cmd.CommandTimeout = 30;
                        cn.Open();

                        double bestmindistance = double.MaxValue;

                        SQLiteDataReader reader = cmd.ExecuteReader();

                        while (reader.Read())
                        {
                            string name = (string)reader["name"];
                            int id = (int)(long)reader["id"];

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

                        cn.Close();
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

            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))  // open the db
            {
                cn.Open();

                int c = 0;

                SQLiteCommand cmd = new SQLiteCommand("select * from Systems where id_edsm = @id_edsm limit 1", cn);   // 1 return matching EDSM ID
                
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
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("id_edsm", system.id_edsm);

                            SQLiteDataReader reader1 = cmd.ExecuteReader();              // see if ESDM ID is there..
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

                            cmd.Reset();
                        }

                    }
                }

                cn.Close();
            }

            using (SQLiteConnection cn2 = new SQLiteConnection(SQLiteDBClass.ConnectionString))  // open the db
            {
                cn2.Open();

                if (toupdate.Count > 0)
                {
                    SQLiteTransaction transaction = cn2.BeginTransaction();

                    foreach (SystemClass sys in toupdate)
                        sys.UpdateEDSM(cn2, sys.id, transaction);        // do an EDSM update of only name/x/y/z, not expected to be many at a time

                    transaction.Commit();
                }

                if (newsystems.Count > 0)
                {
                    int count = 0;

                    SQLiteTransaction transaction = cn2.BeginTransaction();

                    foreach (SystemClass sys in newsystems)             // do a full store, EDDB stuff will be null
                    {
                        sys.Store(cn2, transaction);

                        if (++count % 100000 == 0)                      // had problems with DB is locked, so do it in small trances..
                        {
                            transaction.Commit();
                            transaction.Dispose();
                            transaction = cn2.BeginTransaction();
                            Console.WriteLine("EDSM Store Count " + count);
                        }
                    }

                    Console.WriteLine("Complete store");

                    transaction.Commit();
                }

                if (removenonedsmids)                            // done on a full sync..
                {
                    Console.WriteLine("Delete old ones");
                    using (SQLiteCommand cmddel = new SQLiteCommand("Delete from Systems where id_edsm is null", cn2))
                    {
                        cmddel.CommandType = CommandType.Text;
                        cmddel.CommandTimeout = 30;
                        SQLiteDBClass.SqlNonQueryText(cn2, cmddel);
                    }
                }


                cn2.Close();
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

            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))  // open the db
            {
                cn.Open();

                SQLiteCommand cmd1 = new SQLiteCommand("select * from Systems where id_eddb = @id limit 1", cn);   // 1 return matching ID
                SQLiteCommand cmd2 = new SQLiteCommand("select * from Systems where name = @name and ABS(x-@x)<1 and ABS(y-@y)<1 and ABS(z-@z)<1 limit 1", cn);   // 1 return matching name


                int c = 0;
                int lasttc = Environment.TickCount;

                while (jr.Read())
                {
                    if (jr.TokenType == JsonToken.StartObject)
                    {
                        JObject jo = JObject.Load(jr);

                        SystemClass system = new SystemClass(jo, EDDiscovery2.DB.SystemInfoSource.EDDB);
                        SystemClass dbsys = null;

                        cmd1.Parameters.Clear();
                        cmd1.Parameters.AddWithValue("id", system.id_eddb);

                        SQLiteDataReader reader1 = cmd1.ExecuteReader();             // try its ID first
                        if (reader1.Read())                                          // its there..
                        {
                            dbsys = new SystemClass(reader1);
                            cmd1.Reset();
                        }
                        else
                        {
                            cmd1.Reset();
                            cmd2.Parameters.Clear();                                // else just pick the best name
                            cmd2.Parameters.AddWithValue("name", system.name);
                            cmd2.Parameters.AddWithValue("x", system.x);
                            cmd2.Parameters.AddWithValue("y", system.y);
                            cmd2.Parameters.AddWithValue("z", system.z);

                            SQLiteDataReader reader2 = cmd2.ExecuteReader();        // case insensitive
                            if (reader2.Read())                                     // its there..
                            {
                                dbsys = new SystemClass(reader2);
                            }

                            cmd2.Reset();
                        }

                        if (dbsys!=null && (dbsys.id_eddb != system.id_eddb || dbsys.eddb_updated_at != system.eddb_updated_at || dbsys.population != system.population))
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

                        if (++c % 10000 == 0)
                        {
                            Console.WriteLine("EDDB Count " + c + " Delta " + (Environment.TickCount - lasttc) + " update " + toupdate.Count());
                            lasttc = Environment.TickCount;
                        }
                    }
                }
                cn.Close();
            }

            using (SQLiteConnection cn2 = new SQLiteConnection(SQLiteDBClass.ConnectionString))  // open the db
            {
                cn2.Open();

                if (toupdate.Count > 0)
                {
                    SQLiteTransaction transaction = cn2.BeginTransaction();

                    int count = 0;

                    foreach (SystemClass sys in toupdate)             // do a full store, EDDB stuff will be null
                    {
                        sys.Update(cn2, sys.id, transaction);

                        if (++count % 100000 == 0)                      // had problems with DB is locked, so do it in small trances..
                        {
                            transaction.Commit();
                            transaction.Dispose();
                            transaction = cn2.BeginTransaction();
                            Console.WriteLine("EDDB Store Count " + count);
                        }
                    }

                    Console.WriteLine("Complete store");
                    transaction.Commit();
                }

                cn2.Close();
            }

            return toupdate.Count();
        }

    }
}
