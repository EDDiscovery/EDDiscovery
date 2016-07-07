using EDDiscovery2.DB;
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
using System.Text;

namespace EDDiscovery.DB
{
    public enum DistancsEnum
    {
        Unknown = 0,
        EDSC = 1,
        RedWizzard =2,
        EDDiscovery = 3,
        EDDiscoverySubmitted = 4,

    }

    public class DistanceClass
    {
        public long id;
        public long id_edsm;
        public string NameA;
        public string NameB;
        public double Dist;
        public string CommanderCreate;
        public DateTime CreateTime;
        public DistancsEnum Status;

        public DistanceClass()
        {
        }

        public DistanceClass(DataRow dr)
        {
            id = (long)dr["id"];

            if (System.DBNull.Value != dr["id_edsm"])
                id_edsm = (long)dr["id_edsm"];

            NameA = (string)dr["NameA"];
            NameB = (string)dr["NameB"];
            Dist = Convert.ToDouble(dr["Dist"]);

            CommanderCreate = (string)dr["commanderCreate"];
            CreateTime = (DateTime)dr["CreateTime"];
            CreateTime = CreateTime.ToUniversalTime();
            Status = (DistancsEnum)(long)dr["status"];
        }

        public DistanceClass(SQLiteDataReader dr)
        {
            id = (long)dr["id"];
            if (System.DBNull.Value != dr["id_edsm"])
                id_edsm = (long)dr["id_edsm"];

            NameA = (string)dr["NameA"];
            NameB = (string)dr["NameB"];
            Dist = Convert.ToDouble(dr["Dist"]);

            CommanderCreate = (string)dr["commanderCreate"];
            CreateTime = (DateTime)dr["CreateTime"];
            CreateTime = CreateTime.ToUniversalTime();
            Status = (DistancsEnum)(long)dr["status"];
        }

        public DistanceClass(JObject jo)
        {
            JObject sys1, sys2;
            sys1 = (JObject)jo["sys1"];
            sys2 = (JObject)jo["sys2"];
            JArray submitted_by = (JArray)jo["submitted_by"];

            id_edsm = jo["id"].Value<long>();

            NameA = sys1["name"].Value<string>();
            NameB = sys2["name"].Value<string>();
            Dist = jo["distance"].Value<float>();
            if (submitted_by != null && submitted_by.Count > 0)
            {
                CommanderCreate = submitted_by[0]["cmdrname"].Value<string>();
            }
            else
                CommanderCreate = "";

            CreateTime = jo["date"].Value<DateTime>();
            Status = DistancsEnum.EDSC;
        }

        public static bool Delete(DistancsEnum distsource)
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                cn.Open();

                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = cn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 30;
                    cmd.CommandText = "Delete from Distances where Status=@Status";
                    cmd.Parameters.AddWithValue("@Status", (int)distsource);

                    SQLiteDBClass.SqlNonQueryText(cn, cmd);
                }

                cn.Close();
            }

            return true;
        }

        public bool Store()
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                bool ret;
                ret = Store(cn);

                if (ret == true)
                {
                    using (SQLiteCommand cmd2 = new SQLiteCommand())
                    {
                        cmd2.Connection = cn;
                        cmd2.CommandType = CommandType.Text;
                        cmd2.CommandTimeout = 30;
                        cmd2.CommandText = "Select Max(id) as id from Distances";

                        id = (int)(long)SQLiteDBClass.SqlScalar(cn, cmd2);
                    }

                    return true;
                }

                return ret;
            }
        }

        private bool Store(SQLiteConnection cn, SQLiteTransaction tn = null)
        {
            if (CommanderCreate == null)
                CommanderCreate = "";

            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.Transaction = tn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Insert into Distances (NameA, NameB, Dist, CommanderCreate, CreateTime, Status, id_edsm) values (@NameA, @NameB, @Dist, @CommanderCreate, @CreateTime, @Status, @id_edsm)";
                cmd.Parameters.AddWithValue("@NameA", NameA);
                cmd.Parameters.AddWithValue("@NameB", NameB);
                cmd.Parameters.AddWithValue("@Dist", Dist);
                cmd.Parameters.AddWithValue("@CommanderCreate", CommanderCreate);
                cmd.Parameters.AddWithValue("@CreateTime", CreateTime);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@id_edsm", id_edsm);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
            }

            return true;

        }

        public bool Update()
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnection cn, SQLiteTransaction tn = null)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.Transaction = tn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Update Distances  set NameA=@NameA, NameB=@NameB, Dist=@Dist, commandercreate=@commandercreate, CreateTime=@CreateTime, status=@status, id_edsm=@id_edsm  where ID=@id";
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@NameA", NameA);
                cmd.Parameters.AddWithValue("@NameB", NameB);
                cmd.Parameters.AddWithValue("@Dist", Dist);
                cmd.Parameters.AddWithValue("@CommanderCreate", CommanderCreate);
                cmd.Parameters.AddWithValue("@CreateTime", CreateTime);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@id_edsm", id_edsm);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                return true;
            }
        }

        public bool Delete()
        {
            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnection cn)
        {
            using (SQLiteCommand cmd = new SQLiteCommand())
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 30;
                cmd.CommandText = "Delete From  Distances where ID=@id";
                cmd.Parameters.AddWithValue("@ID", id);

                SQLiteDBClass.SqlNonQueryText(cn, cmd);
                return true;
            }
        }

        public static double FindDistance(EDDiscovery2.DB.ISystem s1, EDDiscovery2.DB.ISystem s2)
        {
            if (s1 == null || s2 == null)
                return -1;

            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "SELECT * FROM Distances WHERE (NameA = @NameA and NameB = @NameB) OR (NameA = @NameB and NameB = @NameA) limit 1";

                        cmd.Parameters.AddWithValue("@NameA", s1.name);
                        cmd.Parameters.AddWithValue("@NameB", s2.name);
                        DataSet ds = SQLiteDBClass.SqlQueryText(cn, cmd);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)     // if found.
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DistanceClass dist = new DistanceClass(dr);
                                return dist.Dist;                                   // return first entry
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

            return -1;
        }

        public static List<DistanceClass> GetDistancesByStatus(int status)
        {
            List<DistanceClass> ldist = new List<DistanceClass>();

            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = cn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 30;
                        cmd.CommandText = "select * from Distances WHERE status='" + status.ToString() + "'";

                        DataSet ds = SQLiteDBClass.SqlQueryText(cn, cmd);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                DistanceClass dist = new DistanceClass(dr);
                                ldist.Add(dist);
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

            return ldist;
        }

        public static void FillVisitedSystems(List<VisitedSystemsClass> visitedSystems, bool usedb)
        {
            try
            {
                using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))
                {
                    cn.Open();
                    SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM Distances WHERE(NameA = @NameA and NameB = @NameB) OR(NameA = @NameB and NameB = @NameA) limit 1", cn);
                    cmd.CommandTimeout = 30;

                    for (int i = 1; i < visitedSystems.Count; i++)                 // now we filled in current system, fill in previous system (except for last)
                    {
                        VisitedSystemsClass cur = visitedSystems[i];
                        VisitedSystemsClass prev = visitedSystems[i - 1];
                        cur.prevSystem = prev.curSystem;

                        double dist = SystemClass.Distance(cur.curSystem, prev.curSystem);  // Try the easy way

                        if ( dist < 0 && usedb )     // failed, and use the db is allowed..
                        {
                            cmd.Parameters.Clear();
                            cmd.Parameters.AddWithValue("@NameA", cur.Name);
                            cmd.Parameters.AddWithValue("@NameB", prev.Name);

                            SQLiteDataReader reader = cmd.ExecuteReader();
                            if (reader.Read())
                            {
                                DistanceClass dst = new DistanceClass(reader);
                                dist = dst.Dist;
                            }

                            cmd.Reset();
                        }

                        if (dist > 0)
                            cur.strDistance = dist.ToString("0.00");
                    }

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }
        }


        public static long ParseEDSMUpdateDistancesString(string json, ref string date, bool removenonedsmids)
        {
            JsonTextReader jr = new JsonTextReader(new StringReader(json));
            return ParseEDSMUpdateDistancesReader(jr, ref date, removenonedsmids);
        }

        public static long ParseEDSMUpdateDistancesFile(string filename, ref string date, bool removenonedsmids)
        {
            StreamReader sr = new StreamReader(filename);         // read directly from file..
            JsonTextReader jr = new JsonTextReader(sr);
            return ParseEDSMUpdateDistancesReader(jr, ref date, removenonedsmids);
        }

        private static long ParseEDSMUpdateDistancesReader(JsonTextReader jr, ref string date , bool removenonedsmids)
        {
            List<DistanceClass> toupdate = new List<DistanceClass>();
            List<DistanceClass> newpairs = new List<DistanceClass>();
            DateTime maxdate = DateTime.Parse(date, new CultureInfo("sv-SE"));

            using (SQLiteConnection cn = new SQLiteConnection(SQLiteDBClass.ConnectionString))  // open the db
            {
                cn.Open();

                int c = 0;

                SQLiteCommand cmd = new SQLiteCommand("select * from Distances where id_edsm=@id limit 1", cn);   // 1 return matching

                int lasttc = Environment.TickCount;

                while (jr.Read())
                {
                    if (jr.TokenType == JsonToken.StartObject)
                    {
                        JObject jo = JObject.Load(jr);

                        DistanceClass dc = new DistanceClass(jo);

                        if (dc.CreateTime.Subtract(maxdate).TotalSeconds > 0)
                            maxdate = dc.CreateTime;

                        if (++c % 10000 == 0)
                        {
                            Console.WriteLine("Dist Count " + c + " Delta " + (Environment.TickCount - lasttc) + " newpairs " + newpairs.Count + " update " + toupdate.Count());
                            lasttc = Environment.TickCount;
                        }

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("id", dc.id_edsm);

                        SQLiteDataReader reader1 = cmd.ExecuteReader();              // see if ESDM ID is there..
                        if (reader1.Read())                                          // its there..
                        {
                            DistanceClass dbdc = new DistanceClass(reader1);

                            // see if EDSM data changed..
                            if (!dbdc.NameA.Equals(dc.NameA) || !dbdc.NameB.Equals(dc.NameB) || Math.Abs(dbdc.Dist - dc.Dist) > 0.05)
                            {
                                dbdc.NameA = dc.NameA;
                                dbdc.NameB = dc.NameB;
                                dbdc.Dist = dc.Dist;
                                toupdate.Add(dbdc);
                            }
                        }
                        else                                                                  // not in database..
                        {
                            //Console.WriteLine("Add new system " + system.name);
                            newpairs.Add(dc);
                        }

                        cmd.Reset();
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

                    foreach (DistanceClass dc in toupdate)
                        dc.Update(cn2, transaction);        

                    transaction.Commit();
                }

                if (newpairs.Count > 0)
                {
                    int count = 0;

                    SQLiteTransaction transaction = cn2.BeginTransaction();

                    foreach (DistanceClass dc in newpairs)             // do a full store
                    {
                        dc.Store(cn2, transaction);

                        if (++count % 100000 == 0)                      // had problems with DB is locked, so do it in small trances..
                        {
                            transaction.Commit();
                            transaction.Dispose();
                            transaction = cn2.BeginTransaction();
                            Console.WriteLine("Dist Store Count " + count);
                        }
                    }

                    Console.WriteLine("Complete store");

                    transaction.Commit();
                }

                if (removenonedsmids)                            // done on a full sync..
                {
                    Console.WriteLine("Delete old ones");
                    using (SQLiteCommand cmddel = new SQLiteCommand("Delete from Distances where id_edsm is null", cn2))
                    {
                        cmddel.CommandType = CommandType.Text;
                        cmddel.CommandTimeout = 30;
                        SQLiteDBClass.SqlNonQueryText(cn2, cmddel);
                    }
                }

                cn2.Close();
            }

            date = maxdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return toupdate.Count + newpairs.Count;
        }
    }
}
