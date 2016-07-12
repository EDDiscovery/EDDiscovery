using EDDiscovery2.DB;
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
using System.Text;
using System.Windows.Forms;

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

        public DistanceClass(DbDataReader dr)
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
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                using (DbCommand cmd = cn.CreateCommand("Delete from Distances where Status=@Status"))
                {
                    cmd.AddParameterWithValue("@Status", (int)distsource);
                    SQLiteDBClass.SQLNonQueryText(cn, cmd);
                }
            }

            return true;
        }

        public bool Store()
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                bool ret;
                ret = Store(cn);

                if (ret == true)
                {
                    using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from Distances"))
                    {
                        id = (long)SQLiteDBClass.SQLScalar(cn, cmd2);
                    }

                    return true;
                }

                return ret;
            }
        }

        private bool Store(SQLiteConnectionED cn, DbTransaction tn = null)
        {
            if (CommanderCreate == null)
                CommanderCreate = "";

            using (DbCommand cmd = cn.CreateCommand("Insert into Distances (NameA, NameB, Dist, CommanderCreate, CreateTime, Status, id_edsm) values (@NameA, @NameB, @Dist, @CommanderCreate, @CreateTime, @Status, @id_edsm)",tn))
            {
                cmd.AddParameterWithValue("@NameA", NameA);
                cmd.AddParameterWithValue("@NameB", NameB);
                cmd.AddParameterWithValue("@Dist", Dist);
                cmd.AddParameterWithValue("@CommanderCreate", CommanderCreate);
                cmd.AddParameterWithValue("@CreateTime", CreateTime);
                cmd.AddParameterWithValue("@Status", Status);
                cmd.AddParameterWithValue("@id_edsm", id_edsm);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
            }

            return true;

        }

        public bool Update()
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionED cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Update Distances  set NameA=@NameA, NameB=@NameB, Dist=@Dist, commandercreate=@commandercreate, CreateTime=@CreateTime, status=@status, id_edsm=@id_edsm  where ID=@id",tn))
            {
                cmd.AddParameterWithValue("@ID", id);
                cmd.AddParameterWithValue("@NameA", NameA);
                cmd.AddParameterWithValue("@NameB", NameB);
                cmd.AddParameterWithValue("@Dist", Dist);
                cmd.AddParameterWithValue("@CommanderCreate", CommanderCreate);
                cmd.AddParameterWithValue("@CreateTime", CreateTime);
                cmd.AddParameterWithValue("@Status", Status);
                cmd.AddParameterWithValue("@id_edsm", id_edsm);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public static long GetTotalDistances()
        {
            long value = 0;

            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select Count(*) from Distances"))
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


        public bool Delete()
        {
            using (SQLiteConnectionED cn = new SQLiteConnectionED())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnectionED cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Delete From  Distances where ID=@id"))
            {
                cmd.AddParameterWithValue("@ID", id);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public static double FindDistance(EDDiscovery2.DB.ISystem s1, EDDiscovery2.DB.ISystem s2)
        {
            if (s1 == null || s2 == null)
                return -1;

            try
            {
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT * FROM Distances WHERE (NameA = @NameA and NameB = @NameB) OR (NameA = @NameB and NameB = @NameA) limit 1"))
                    {
                        cmd.AddParameterWithValue("@NameA", s1.name);
                        cmd.AddParameterWithValue("@NameB", s2.name);
                        DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);

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
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    using (DbCommand cmd = cn.CreateCommand("select * from Distances WHERE status='" + status.ToString() + "'"))
                    {
                        DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);

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
                using (SQLiteConnectionED cn = new SQLiteConnectionED())
                {
                    DbCommand cmd = cn.CreateCommand("SELECT * FROM Distances WHERE(NameA = @NameA and NameB = @NameB) OR(NameA = @NameB and NameB = @NameA) limit 1");

                    for (int i = 1; i < visitedSystems.Count; i++)                 // now we filled in current system, fill in previous system (except for last)
                    {
                        VisitedSystemsClass cur = visitedSystems[i];
                        VisitedSystemsClass prev = visitedSystems[i - 1];
                        cur.prevSystem = prev.curSystem;

                        double dist = SystemClass.Distance(cur.curSystem, prev.curSystem);  // Try the easy way

                        if ( dist < 0 && usedb )     // failed, and use the db is allowed..
                        {
                            cmd.Parameters.Clear();
                            cmd.AddParameterWithValue("@NameA", cur.Name);
                            cmd.AddParameterWithValue("@NameB", prev.Name);

                            using (DbDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    DistanceClass dst = new DistanceClass(reader);
                                    dist = dst.Dist;
                                }
                            }
                        }

                        if (dist > 0)
                            cur.strDistance = dist.ToString("0.00");
                    }
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

            bool emptydatabase = GetTotalDistances() == 0;            // if empty database, we can skip the lookup

            using (SQLiteConnectionED cn = new SQLiteConnectionED())  // open the db 
            {
                int c = 0;

                DbCommand cmd = null;

                int lasttc = Environment.TickCount;

                try
                {
                    cmd = cn.CreateCommand("select * from Distances where id_edsm=@id limit 1");   // 1 return matching

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

                            if (emptydatabase)                                                  // empty DB, just store..
                            {
                                newpairs.Add(dc);
                            }
                            else
                            {
                                cmd.Parameters.Clear();
                                cmd.AddParameterWithValue("id", dc.id_edsm);

                                using (DbDataReader reader1 = cmd.ExecuteReader())              // see if ESDM ID is there..
                                {
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
                                }
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("There is a problem using the EDSM distance file." + Environment.NewLine +
                                    "Please perform a manual EDSM distance sync (see Admin menu) next time you run the program ", "ESDM Sync Error");
                }
                finally
                {
                    if (cmd != null) cmd.Dispose();
                }
            }

            using (SQLiteConnectionED cn2 = new SQLiteConnectionED())  // open the db
            {
                if (toupdate.Count > 0)
                {
                    using (DbTransaction transaction = cn2.BeginTransaction())
                    {
                        foreach (DistanceClass dc in toupdate)
                            dc.Update(cn2, transaction);

                        transaction.Commit();
                    }
                }

                if (newpairs.Count > 0)
                {
                    int count = 0;

                    while (count < newpairs.Count())
                    {
                        using (DbTransaction transaction = cn2.BeginTransaction())
                        {
                            while (count < newpairs.Count())
                            {
                                newpairs[count].Store(cn2, transaction);

                                if (++count % 100000 == 0)
                                    break;
                            }

                            Console.WriteLine("EDSM Dist Store Count " + count);
                            transaction.Commit();
                        }
                    }
                }

                if (removenonedsmids)                            // done on a full sync..
                {
                    Console.WriteLine("Delete old ones");
                    using (DbCommand cmddel = cn2.CreateCommand("Delete from Distances where id_edsm is null"))
                    {
                        SQLiteDBClass.SQLNonQueryText(cn2, cmddel);
                    }
                }
            }

            date = maxdate.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            return toupdate.Count + newpairs.Count;
        }
    }
}
