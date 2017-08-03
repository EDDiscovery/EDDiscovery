/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */

using EliteDangerousCore;
using EliteDangerousCore.DB;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;


namespace EliteDangerousCore.EDSM
{
    public static class SystemClassEDSM
    {
        public static void CheckSystemAliases()
        {
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
            }
        }

        public static void RemoveHiddenSystems()
        {
            EDSMClass edsm = new EDSMClass();

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

        public static bool PerformEDSMFullSync(Func<bool> cancelRequested, Action<int, string> reportProgress, Action<string> logLine, Action<string> logError)
        {
            string rwsystime = SQLiteConnectionSystem.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
            DateTime edsmdate;

            if (!DateTime.TryParse(rwsystime, CultureInfo.InvariantCulture, DateTimeStyles.None, out edsmdate))
            {
                edsmdate = new DateTime(2000, 1, 1);
            }

            long updates = 0;

            // Delete all old systems
            SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", "2010-01-01 00:00:00");
            SQLiteConnectionSystem.PutSettingString("EDDBSystemsTime", "0");

            EDSMClass edsm = new EDSMClass();

            logLine("Get hidden systems from EDSM and remove from database");

            RemoveHiddenSystems();

            if (cancelRequested())
                return false;

            logLine("Download systems file from EDSM.");

            string edsmsystems = Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "edsmsystems.json");

            logLine("Resyncing all downloaded EDSM systems with local database." + Environment.NewLine + "This will take a while.");

            bool newfile;
            bool success = BaseUtils.DownloadFileHandler.DownloadFile(EDSMClass.ServerAddress + "dump/systemsWithCoordinates.json", edsmsystems, out newfile, (n, s) =>
            {
                SQLiteConnectionSystem.CreateTempSystemsTable();

                string rwsysfiletime = "2014-01-01 00:00:00";
                bool outoforder = false;
                using (var reader = new StreamReader(s))
                    updates = ParseEDSMUpdateSystemsStream(reader, ref rwsysfiletime, ref outoforder, true, cancelRequested, reportProgress, useCache: false, useTempSystems: true);
                if (!cancelRequested())       // abort, without saving time, to make it do it again
                {
                    SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", rwsysfiletime);
                    logLine("Replacing old systems table with new systems table and re-indexing - please wait");
                    reportProgress(-1, "Replacing old systems table with new systems table and re-indexing - please wait");
                    SQLiteConnectionSystem.ReplaceSystemsTable();
                    SQLiteConnectionSystem.PutSettingBool("EDSMSystemsOutOfOrder", outoforder);
                    reportProgress(-1, "");
                }
                else
                {
                    throw new OperationCanceledException();
                }
            });

            GC.Collect();
            if (!success)
            {
                logLine("Failed to download EDSM system file from server, will check next time");
                return false;
            }
            else if (cancelRequested())
            {   // Stop if requested
                return false;
            }
            else
            {
                logLine("Local database updated with EDSM data, " + updates + " systems updated.");
                File.Delete(edsmsystems);
                return (updates > 0);
            }
        }


        public static long ParseEDSMUpdateSystemsString(string json, ref string date, ref bool outoforder, bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true)
        {
            using (StringReader sr = new StringReader(json))
                return ParseEDSMUpdateSystemsStream(sr, ref date, ref outoforder, removenonedsmids, cancelRequested, reportProgress, useCache);
        }

        public static long ParseEDSMUpdateSystemsFile(string filename, ref string date, ref bool outoforder, bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true)
        {
            using (StreamReader sr = new StreamReader(filename))         // read directly from file..
                return ParseEDSMUpdateSystemsStream(sr, ref date, ref outoforder, removenonedsmids, cancelRequested, reportProgress, useCache);
        }

        public static long ParseEDSMUpdateSystemsStream(TextReader sr, ref string date, ref bool outoforder, bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            using (JsonTextReader jr = new JsonTextReader(sr))
                return ParseEDSMUpdateSystemsReader(jr, ref date, ref outoforder, removenonedsmids, cancelRequested, reportProgress, useCache, useTempSystems);
        }

        private static Dictionary<long, SystemClassBase> GetEdsmSystemsLite()
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: EDDbAccessMode.Reader))
            {
                Dictionary<long, SystemClassBase> systemsByEdsmId = new Dictionary<long, SystemClassBase>();

                using (DbCommand cmd = cn.CreateCommand("SELECT s.id, s.EdsmId, n.Name, s.x, s.y, s.z, s.UpdateTimestamp, s.gridid, s.randomid FROM EdsmSystems s JOIN SystemNames n ON n.EdsmId = s.EdsmId"))
                {
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SystemClassBase sys = new SystemClassBase
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
                                sys.x = ((double)(long)reader["x"]) / SystemClassDB.XYZScalar;
                                sys.y = ((double)(long)reader["y"]) / SystemClassDB.XYZScalar;
                                sys.z = ((double)(long)reader["z"]) / SystemClassDB.XYZScalar;
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

        private static long DoParseEDSMUpdateSystemsReader(JsonTextReader jr, ref string date, ref bool outoforder, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            DateTime maxdate;

            if (!DateTime.TryParse(date, CultureInfo.InvariantCulture, DateTimeStyles.None, out maxdate))
            {
                maxdate = new DateTime(2010, 1, 1);
            }

            Dictionary<long, SystemClassBase> systemsByEdsmId = useCache ? GetEdsmSystemsLite() : new Dictionary<long, SystemClassBase>();
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

                                        SystemClassBase dbsys = null;

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
                                                        dbsys = new SystemClassBase
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
                                                            dbsys.x = ((double)(long)reader["X"]) / SystemClassDB.XYZScalar;
                                                            dbsys.y = ((double)(long)reader["Y"]) / SystemClassDB.XYZScalar;
                                                            dbsys.z = ((double)(long)reader["Z"]) / SystemClassDB.XYZScalar;
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
                                                updateSysCmd.Parameters["@X"].Value = (long)(x * SystemClassDB.XYZScalar);
                                                updateSysCmd.Parameters["@Y"].Value = (long)(y * SystemClassDB.XYZScalar);
                                                updateSysCmd.Parameters["@Z"].Value = (long)(z * SystemClassDB.XYZScalar);
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
                                            insertSysCmd.Parameters["@X"].Value = (long)(x * SystemClassDB.XYZScalar);
                                            insertSysCmd.Parameters["@Y"].Value = (long)(y * SystemClassDB.XYZScalar);
                                            insertSysCmd.Parameters["@Z"].Value = (long)(z * SystemClassDB.XYZScalar);
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

        private static long ParseEDSMUpdateSystemsReader(JsonTextReader jr, ref string date, ref bool outoforder, bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            return DoParseEDSMUpdateSystemsReader(jr, ref date, ref outoforder, cancelRequested, reportProgress, useCache, useTempSystems);
        }


        public class SystemsSyncState
        {
            public bool performhistoryrefresh = false;
            public bool syncwasfirstrun = false;
            public bool syncwaseddboredsm = false;
            public bool performedsmsync = false;
            public bool performeddbsync = false;
        }

        public static void PerformSync(Func<bool> cancelRequested, Action<int, string> reportProgress, Action<string> logLine, Action<string> logError, SystemsSyncState state)           // big check.. done in a thread.
        {
            reportProgress(-1, "");

            state.performhistoryrefresh = false;
            state.syncwasfirstrun = SystemClassDB.IsSystemsTableEmpty();                 // remember if DB is empty

            // Force a full sync if newest data is more than 14 days old

            bool outoforder = SQLiteConnectionSystem.GetSettingBool("EDSMSystemsOutOfOrder", true);
            DateTime lastmod = outoforder ? SystemClassDB.GetLastSystemModifiedTime() : SystemClassDB.GetLastSystemModifiedTimeFast();
            if (DateTime.UtcNow.Subtract(lastmod).TotalDays >= 14)
            {
                state.performedsmsync = true;
            }

            bool edsmoreddbsync = state.performedsmsync || state.performeddbsync;           // remember if we are syncing
            state.syncwaseddboredsm = edsmoreddbsync;

            if (state.performedsmsync || state.performeddbsync)
            {
                if (state.performedsmsync && !cancelRequested())
                {
                    // Download new systems
                    try
                    {
                        state.performhistoryrefresh |= PerformEDSMFullSync(cancelRequested, reportProgress, logLine, logError);
                        state.performedsmsync = false;
                    }
                    catch (Exception ex)
                    {
                        logError("GetAllEDSMSystems exception:" + ex.Message);
                    }
                }

                if (!cancelRequested())
                {
                    logLine("Indexing systems table");
                    SQLiteConnectionSystem.CreateSystemsTableIndexes();

                    try
                    {
                        EliteDangerousCore.EDDB.SystemClassEDDB.PerformEDDBFullSync(cancelRequested, reportProgress, logLine, logError);
                        state.performeddbsync = false;
                    }
                    catch (Exception ex)
                    {
                        logError("GetEDDBUpdate exception: " + ex.Message);
                    }
                    state.performhistoryrefresh = true;
                }
            }

            if (!cancelRequested())
            {
                logLine("Indexing systems table");
                SQLiteConnectionSystem.CreateSystemsTableIndexes();

                lastmod = outoforder ? SystemClassDB.GetLastSystemModifiedTime() : SystemClassDB.GetLastSystemModifiedTimeFast();
                if (DateTime.UtcNow.Subtract(lastmod).TotalHours >= 1)
                {
                    logLine("Checking for new EDSM systems (may take a few moments).");
                    EDSMClass edsm = new EDSMClass();
                    long updates = edsm.GetNewSystems(cancelRequested, reportProgress, logLine);
                    logLine("EDSM updated " + updates + " systems.");
                    state.performhistoryrefresh |= (updates > 0);
                }
            }

            reportProgress(-1, "");
        }


    }
}
