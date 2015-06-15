using EDDiscovery2;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Linq;
using System.Text;

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



    public class SystemClass
    {
        public int id;
        public string name;
        public string SearchName;
        public double x, y, z;
        public int cr;
        public string CommanderCreate;
        public DateTime CreateDate;
        public string CommanderUpdate;
        public DateTime UpdateDate;
        public SystemStatusEnum status;
        public string Note;


        public int id_eddb;
        public string faction;
        public long population;
        public EDGovernment government;
        public EDAllegiance allegiance;
        public EDState state;
        public EDSecurity security;
        public EDEconomy primary_economy;
        public int needs_permit;
        public int eddb_updated_at;

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
                    CommanderCreate = (string)dr["commandercreate"];
                    CreateDate = (DateTime)dr["createdate"];
                    CommanderUpdate = (string)dr["commanderupdate"];
                    UpdateDate = (DateTime)dr["updatedate"];

                }

                status = (SystemStatusEnum)((long)dr["status"]);
                Note = dr["Note"].ToString();
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



        public bool HasCoordinate
        {
            get
            {
                return (!double.IsNaN(x));
            }
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

                        var sys = SQLiteDBClass.globalSystems.FirstOrDefault(sy => sy.SearchName == system.SearchName);

                        if (sys != null)
                        {
                            system.Update(cn, sys.id);
                        }
                        else
                            system.Store(cn);
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
                return Store(cn);
            }
        }

        private bool Store(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                if (id_eddb != null)
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note, id_eddb, faction, government_id, allegiance_id, primary_economy_id,  security, eddb_updated_at, state, needs_permit) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note, @id_eddb, @faction, @government_id, @allegiance_id, @primary_economy_id,  @security, @eddb_updated_at, @state, @needs_permit)";
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
                    cmd.CommandText = "Insert into Systems (name, x, y, z, cr, commandercreate, createdate, commanderupdate, updatedate, status, note) values (@name, @x, @y, @z, @cr, @commandercreate, @createdate, @commanderupdate, @updatedate, @status, @Note)";
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

        private bool Update(SQLiteConnection cn, int id)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update Systems set name=@name, x=@x, y=@y, z=@z, cr=@cr, commandercreate=@commandercreate, createdate=@createdate, commanderupdate=@commanderupdate, updatedate=@updatedate, status=@status, note=@Note  where ID=@id";

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


                if (name.Equals("Sol"))
                {
                    System.Diagnostics.Trace.WriteLine("Sol");
                }

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                return true;
            }
        }

    }
}
