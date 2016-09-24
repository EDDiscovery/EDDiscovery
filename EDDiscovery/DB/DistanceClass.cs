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
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
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
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
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

        private bool Store(SQLiteConnectionSystem cn, DbTransaction tn = null)
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
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                return Update(cn);
            }
        }

        private bool Update(SQLiteConnectionSystem cn, DbTransaction tn = null)
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
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
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
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
            {
                return Delete(cn);
            }
        }

        private bool Delete(SQLiteConnectionSystem cn)
        {
            using (DbCommand cmd = cn.CreateCommand("Delete From  Distances where ID=@id"))
            {
                cmd.AddParameterWithValue("@ID", id);

                SQLiteDBClass.SQLNonQueryText(cn, cmd);
                return true;
            }
        }

        public static DistanceClass GetDistanceClass(EDDiscovery2.DB.ISystem s1, EDDiscovery2.DB.ISystem s2)
        {
            if (s1 == null || s2 == null)
                return null;

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
                {
                    using (DbCommand cmd = cn.CreateCommand("SELECT * FROM Distances WHERE (NameA = @NameA and NameB = @NameB) OR (NameA = @NameB and NameB = @NameA) limit 1"))
                    {
                        cmd.AddParameterWithValue("@NameA", s1.name);
                        cmd.AddParameterWithValue("@NameB", s2.name);

                        DataSet ds = SQLiteDBClass.SQLQueryText(cn, cmd);

                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)     // if found.
                        {
                            DistanceClass dist = new DistanceClass(ds.Tables[0].Rows[0]);
                            return dist;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Exception : " + ex.Message);
                System.Diagnostics.Trace.WriteLine(ex.StackTrace);
            }

            return null;
        }


        public static double FindDistance(EDDiscovery2.DB.ISystem s1, EDDiscovery2.DB.ISystem s2)
        {
            DistanceClass dst = GetDistanceClass(s1, s2);
            return (dst == null) ? -1 : dst.Dist;
        }

        public static List<DistanceClass> GetDistancesByStatus(int status)
        {
            List<DistanceClass> ldist = new List<DistanceClass>();

            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
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

        /*
        public static void FillVisitedSystems(List<VisitedSystemsClass> visitedSystems, bool usedb)
        {
            try
            {
                using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())
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
        */

        public static long ParseEDSMUpdateDistancesString(string json, ref string date, bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, Action<string> logline)
        {
            JsonTextReader jr = new JsonTextReader(new StringReader(json));
            return ParseEDSMUpdateDistancesReader(jr, ref date, removenonedsmids, cancelRequested, reportProgress, logline);
        }

        public static long ParseEDSMUpdateDistancesFile(string filename, ref string date, bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, Action<string> logline)
        {
            StreamReader sr = new StreamReader(filename);         // read directly from file..
            JsonTextReader jr = new JsonTextReader(sr);
            return ParseEDSMUpdateDistancesReader(jr, ref date, removenonedsmids, cancelRequested, reportProgress, logline);
        }

        private static long ParseEDSMUpdateDistancesReader(JsonTextReader jr, ref string date , bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, Action<string> logline)
        {
            List<DistanceClass> toupdate = new List<DistanceClass>();
            List<DistanceClass> newpairs = new List<DistanceClass>();
            DateTime maxdate = DateTime.Parse(date, new CultureInfo("sv-SE"));

            bool emptydatabase = GetTotalDistances() == 0;            // if empty database, we can skip the lookup

            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem())  // open the db 
            {
                int c = 0;

                DbCommand cmd = null;

                int lasttc = Environment.TickCount;

                try
                {
                    cmd = cn.CreateCommand("select * from Distances where id_edsm=@id limit 1");   // 1 return matching

                    while (jr.Read() && !cancelRequested())
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
                                reportProgress(-1, $"Reading EDSM distances: {c} processed, {newpairs.Count} new, {toupdate.Count} to update");
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
                    logline("There is a problem using the EDSM distance file." + Environment.NewLine +
                                    "Please perform a manual EDSM distance sync (see Admin menu) next time you run the program ");
                }
                finally
                {
                    if (cmd != null) cmd.Dispose();
                }
            }

            if (cancelRequested())
            {
                return 0;
            }

            using (SQLiteConnectionSystem cn2 = new SQLiteConnectionSystem())  // open the db
            {
                if (toupdate.Count > 0)
                {
                    reportProgress(-1, $"Updating EDSM distances: {toupdate.Count} distances to update");
                    using (DbTransaction transaction = cn2.BeginTransaction())
                    {
                        foreach (DistanceClass dc in toupdate)
                            dc.Update(cn2, transaction);

                        transaction.Commit();
                    }
                }

                if (cancelRequested())
                {
                    return toupdate.Count();
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

                                if (++count % 10000 == 0)
                                    break;
                            }

                            reportProgress(count * 100 / newpairs.Count, $"Adding EDSM distances: {count} added");
                            Console.WriteLine("EDSM Dist Store Count " + count);
                            transaction.Commit();
                        }

                        if (cancelRequested())
                        {
                            return toupdate.Count() + count;
                        }
                    }
                }

                if (removenonedsmids)                            // done on a full sync..
                {
                    reportProgress(-1, "Removing distances without an ID");
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
