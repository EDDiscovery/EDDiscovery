using EDDiscovery2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Globalization;

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


        public SystemClass(JObject jo, SystemInfoSource source)
        {
            if (source == SystemInfoSource.RW)
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
            else if (source == SystemInfoSource.EDSC)
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
            else if (source == SystemInfoSource.EDSM)
            {
                JObject coords = (JObject)jo["coords"];

                name = jo["name"].Value<string>();
                SearchName = name.ToLower();

                //cr = jo["cr"].Value<int>();
                x = double.NaN;
                y = double.NaN;
                z = double.NaN;


                if (coords !=null &&  (coords["x"].Type == JTokenType.Float || coords["x"].Type == JTokenType.Integer))
                {
                    x = coords["x"].Value<double>();
                    y = coords["y"].Value<double>();
                    z = coords["z"].Value<double>();
                }
                JArray submitted = (JArray)jo["submitted"];

                if (submitted != null && submitted.Count>0)
                {
                    if (submitted[0]["cmdrname"]!=null)
                        CommanderCreate = submitted[0]["cmdrname"].Value<string>();
                    CreateDate = submitted[0]["date"].Value<DateTime>();

                    if (submitted[submitted.Count - 1]["cmdrname"] != null)
                        CommanderUpdate = submitted[submitted.Count-1]["cmdrname"].Value<string>();
                    UpdateDate = submitted[submitted.Count-1]["date"].Value<DateTime>();

                }

                UpdateDate = jo["date"].Value<DateTime>();
                if (CreateDate.Year <= 1)
                    CreateDate = UpdateDate;


                status = SystemStatusEnum.EDSC;
            }
            else if (source == SystemInfoSource.EDDB)
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





        public SystemClass(DataRow dr)
        {
            try
            {
                Object o;

                id = (int)(long)dr["id"];
                name = (string)dr["name"];



                SearchName = name.ToLower();


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
                id_eddb = o == DBNull.Value ? 0 : (int)((long)o);

                o = dr["population"];
                population = o == DBNull.Value ? 0 : (int)((long)o);


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
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception SystemClass: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);

            }
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

        public static bool Delete(string name)
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                cn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "Delete from Systems where Name=@Name";
                    cmd.Parameters.AddWithValue("@Name", name);

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


                SystemClass system = new SystemClass(jo, SystemInfoSource.EDSC);


                

                if (system.HasCoordinate)
                    listSystems.Add(system);
            }
            return listSystems;
        }


        public static List<SystemClass> ParseEDSM(string json, ref string date)
        {
            JArray edsc = null;
            if (json != null && json.Length > 5)
                edsc = (JArray)JArray.Parse(json);

            List<SystemClass> listSystems = new List<SystemClass>();

            if (edsc == null)
                return listSystems;


            DateTime maxdate = DateTime.Parse(date,  new CultureInfo("sv-SE"));

//            date = edscdata["date"].Value<string>();




            foreach (JObject jo in edsc)
            {
                string name = jo["name"].Value<string>();

                SystemClass system = new SystemClass(jo, SystemInfoSource.EDSM);

                if (system.UpdateDate.Subtract(maxdate).TotalSeconds>0)
                    maxdate = system.UpdateDate;



                if (system.HasCoordinate)
                    listSystems.Add(system);
            }

            date = maxdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return listSystems;
        }


        public static bool Store(List<SystemClass> systems)
        {
            if (systems == null)
                return true;

            try
            {
                Stopwatch sw = new Stopwatch();

                sw.Start();

                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    cn.Open();
                    SQLiteTransaction transaction = cn.BeginTransaction();
                    foreach (SystemClass system in systems)
                    {

                        SystemClass sys = SystemData.GetSystem(system.name);
                        if (sys != null)
                        {
                            system.UpdateEDSM(cn, sys.id, transaction);
                        }
                        else
                            system.Store(cn, transaction);
                    }

                    transaction.Commit();
                    cn.Close();
                }
                sw.Stop();
                System.Diagnostics.Trace.WriteLine("SQLite Add  " + systems.Count.ToString() + " systems: " + sw.Elapsed.TotalSeconds.ToString("0.000s"));
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception SystemClass: " + ex.Message);
                System.Diagnostics.Trace.WriteLine("Trace: " + ex.StackTrace);
                return false;
            }

        }

        public bool Store()
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                return Store(cn, null);
            }
        }

        public bool Store(SQLiteConnection cn, SQLiteTransaction transaction)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                if (id_eddb != 0)
                {
                    cmd.Connection = cn;
                    cmd.Transaction = transaction;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note, id_eddb, population, faction, government_id, allegiance_id, primary_economy_id, security, eddb_updated_at, state, needs_permit, versiondate) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note, @id_eddb, @population, @faction, @government_id, @allegiance_id, @primary_economy_id,  @security, @eddb_updated_at, @state, @needs_permit, datetime('now'))";
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


                    if (Note == null)
                        Note = "";
                    cmd.Parameters.AddWithValue("@Note", Note);

                    SQLiteDBClass.SqlNonQueryText(cn, cmd);

                    return true;
                }
                else
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note, versiondate) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note, datetime('now'))";
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




                    SQLiteDBClass.SqlNonQueryText(cn, cmd);
                    return true;

                }
            }
        }

        public bool Update(SQLiteConnection cn, int id, SQLiteTransaction transaction)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("Update", cn, transaction))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update Systems set name=@name, x=@x, y=@y, z=@z, cr=@cr, commandercreate=@commandercreate, createdate=@createdate, commanderupdate=@commanderupdate, updatedate=@updatedate, status=@status, note=@Note, id_eddb=@id_eddb, population=@population, faction=@faction, government_id=@government_id, allegiance_id=@allegiance_id, primary_economy_id=@primary_economy_id,  security=@security, eddb_updated_at=@eddb_updated_at, state=@state, needs_permit=@needs_permit, versiondate=datetime('now') where ID=@id";

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

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                return true;
            }
        }

        public bool UpdateEDSM(SQLiteConnection cn, int id, SQLiteTransaction transaction)
        {
            using (SQLiteCommand cmd = new SQLiteCommand("Update", cn, transaction))
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update Systems set name=@name, x=@x, y=@y, z=@z, commandercreate=@commandercreate, createdate=@createdate, commanderupdate=@commanderupdate, updatedate=@updatedate, status=@status, versiondate=datetime('now') where ID=@id";

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

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                return true;
            }
        }

    }
}
