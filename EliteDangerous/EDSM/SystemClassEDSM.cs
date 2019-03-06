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
using SQLLiteExtensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;

namespace EliteDangerousCore.EDSM
{
    public static class SystemClassEDSM
    {
        private static DateTime ED21date = new DateTime(2016, 5, 26);
        private static DateTime ED23date = new DateTime(2017, 4, 11);

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

        public static void RemoveHiddenSystems(string json)         // protected against bad json
        {
            try
            {
                JsonTextReader jr = new JsonTextReader(new StringReader(json));
                bool jr_eof = false;

                while (!jr_eof)
                {
                    using (SQLiteConnectionSystem cn2 = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Writer))  // open the db
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("SysClassEDSM Exception " + e.ToString());
            }
        }

        private static Dictionary<long, SystemClassBase> GetEdsmSystemsLite()
        {
            using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Reader))
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
                                ID = (long)reader["id"],
                                Name = (string)reader["name"]
                            };

                            string searchname = sys.Name.ToLowerInvariant();

                            if (System.DBNull.Value == reader["x"])
                            {
                                sys.X = double.NaN;
                                sys.Y = double.NaN;
                                sys.Z = double.NaN;
                            }
                            else
                            {
                                sys.X = ((double)(long)reader["x"]) / SystemClassDB.XYZScalar;
                                sys.Y = ((double)(long)reader["y"]) / SystemClassDB.XYZScalar;
                                sys.Z = ((double)(long)reader["z"]) / SystemClassDB.XYZScalar;
                            }

                            sys.EDSMID = (long)reader["EdsmId"];
                            systemsByEdsmId[sys.EDSMID] = sys;
                            sys.GridID = reader["gridid"] == DBNull.Value ? 0 : (int)((long)reader["gridid"]);
                            sys.RandomID = reader["randomid"] == DBNull.Value ? 0 : (int)((long)reader["randomid"]);
                        }
                    }
                }

                return systemsByEdsmId;
            }
        }

        private class EDSMDumpSystemCoords
        {
            public static EDSMDumpSystemCoords Deserialize(JsonReader rdr)
            {
                EDSMDumpSystemCoords c = new EDSMDumpSystemCoords();

                if (rdr.TokenType != JsonToken.StartObject)
                    rdr.Read();

                Debug.Assert(rdr.TokenType == JsonToken.StartObject);

                while (rdr.Read() && rdr.TokenType == JsonToken.PropertyName)
                {
                    string name = rdr.Value as string;
                    switch (name)
                    {
                        case "x": c.x = rdr.ReadAsDouble() ?? Double.NaN; break;
                        case "y": c.y = rdr.ReadAsDouble() ?? Double.NaN; break;
                        case "z": c.z = rdr.ReadAsDouble() ?? Double.NaN; break;
                    }
                }

                Debug.Assert(rdr.TokenType == JsonToken.EndObject);

                return c;
            }

            public double x;
            public double y;
            public double z;
        }

        private class EDSMDumpSystem
        {
            public static EDSMDumpSystem Deserialize(JsonReader rdr)
            {
                EDSMDumpSystem s = new EDSMDumpSystem();

                while (rdr.Read() && rdr.TokenType == JsonToken.PropertyName)
                {
                    string name = rdr.Value as string;
                    switch (name)
                    {
                        case "name": s.name = rdr.ReadAsString(); break;
                        case "id": s.id = rdr.ReadAsInt32() ?? 0; break;
                        case "date": s.date = rdr.ReadAsDateTime() ?? DateTime.MinValue; break;
                        case "coords": s.coords = EDSMDumpSystemCoords.Deserialize(rdr); break;
                        default: rdr.Read(); JToken.Load(rdr); break;
                    }
                }

                Debug.Assert(rdr.TokenType == JsonToken.EndObject);

                return s;
            }

            public string name;
            public long id;
            public DateTime date;
            public EDSMDumpSystemCoords coords;
        }


        #region Parse and store in DB

        private static long ParseEDSMUpdateSystemsString(string json, bool[] grididallow, ref DateTime date, bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true)
        {
            using (StringReader sr = new StringReader(json))
                return ParseEDSMUpdateSystemsStream(sr, grididallow, ref date,  removenonedsmids, cancelRequested, reportProgress, useCache);
        }

        private static long ParseEDSMUpdateSystemsFile(string filename, bool[] grididallow, ref DateTime date, bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true)
        {
            using (StreamReader sr = new StreamReader(filename))         // read directly from file..
                return ParseEDSMUpdateSystemsStream(sr, grididallow, ref date,  removenonedsmids, cancelRequested, reportProgress, useCache);
        }

        private static long ParseEDSMUpdateSystemsStream(TextReader sr, bool[] grididallow, ref DateTime date, bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            using (JsonTextReader jr = new JsonTextReader(sr))
                return ParseEDSMUpdateSystemsReader(jr, grididallow, ref date,  removenonedsmids, cancelRequested, reportProgress, useCache, useTempSystems);
        }

        private static long ParseEDSMUpdateSystemsReader(JsonTextReader jr, bool[] grididallow, ref DateTime date,  bool removenonedsmids, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            return DoParseEDSMUpdateSystemsReader(jr, grididallow, ref date,  cancelRequested, reportProgress, useCache, useTempSystems);
        }

        // returns no of updates + inserts, not no of items processed.   Protect yourself against bad json 

        private static long DoParseEDSMUpdateSystemsReader(JsonTextReader jr, bool[] grididallowed, ref DateTime maxdate, Func<bool> cancelRequested, Action<int, string> reportProgress, bool useCache = true, bool useTempSystems = false)
        {
            Dictionary<long, SystemClassBase> systemsByEdsmId = useCache ? GetEdsmSystemsLite() : new Dictionary<long, SystemClassBase>();
            int count = 0;
            int updatecount = 0;
            int insertcount = 0;
            Random rnd = new Random();
            string sysnamesTableName = useTempSystems ? "SystemNames_temp" : "SystemNames";
            string edsmsysTableName = useTempSystems ? "EdsmSystems_temp" : "EdsmSystems";
            Stopwatch sw = Stopwatch.StartNew();
            const int BlockSize = 10000;

            while (!cancelRequested())
            {
                bool jr_eof = false;
                List<EDSMDumpSystem> objs = new List<EDSMDumpSystem>(BlockSize);

                while (!cancelRequested())
                {
                    if (jr.Read())
                    {
                        if (jr.TokenType == JsonToken.StartObject)
                        {
                            objs.Add(EDSMDumpSystem.Deserialize(jr));

                            if (objs.Count >= BlockSize)
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

                IEnumerator<EDSMDumpSystem> jo_enum = objs.GetEnumerator();
                bool jo_enum_finished = false;

                while (!jo_enum_finished && !cancelRequested())
                {
                    int blkcount = 0;
                    int oldinsertcnt = insertcount;
                    int oldupdatecnt = updatecount;
                    int oldcount = count;

                    using (SQLExtTransactionLock<SQLiteConnectionSystem> tl = new SQLExtTransactionLock<SQLiteConnectionSystem>())
                    {
                        tl.OpenWriter();
                        using (SQLiteConnectionSystem cn = new SQLiteConnectionSystem(mode: SQLLiteExtensions.SQLExtConnection.AccessMode.Writer))
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

                                    while (!cancelRequested())
                                    {
                                        if (!jo_enum.MoveNext())
                                        {
                                            reportProgress(-1, $"Syncing EDSM systems: {count:N0} processed, {insertcount:N0} new systems, {updatecount:N0} updated systems");
                                            txn.Commit();

                                            if (jr_eof)
                                            {
                                                System.Diagnostics.Debug.WriteLine("Maximum date was " + maxdate.ToString());
                                                System.Diagnostics.Debug.WriteLine($"Import took {sw.ElapsedMilliseconds}ms");

                                                return updatecount + insertcount;
                                            }

                                            jo_enum_finished = true;
                                            break;
                                        }
                                        else if (SQLiteConnectionSystem.IsReadWaiting)
                                        {
                                            if (blkcount < objs.Count * 3 / 4) // Let the reader barge in if we've processed less than 3/4 of the items
                                            {
                                                // Reset the counts, roll back the transaction, and let the reader through...
                                                insertcount = oldinsertcnt;
                                                updatecount = oldupdatecnt;
                                                count = oldcount;
                                                jo_enum.Reset();
                                                txn.Rollback();
                                                break;
                                            }
                                        }

                                        EDSMDumpSystem jo = jo_enum.Current;
                                        EDSMDumpSystemCoords coords = jo.coords;

                                        if (coords != null)
                                        {
                                            DateTime updatedate = DateTime.SpecifyKind(jo.date, DateTimeKind.Utc);

                                            if (updatedate > maxdate)                                   // even if we reject it due to grid id, keep last date up to date
                                                maxdate = updatedate;

                                            double x = coords.x;
                                            double z = coords.z;
                                            int gridid = GridId.Id(x, z);

                                            if (grididallowed[gridid])  // if grid allows it to be added..
                                            {
                                                //System.Diagnostics.Debug.WriteLine("Accept due to gridid " + gridid);
                                                double y = coords.y;
                                                long edsmid = jo.id;
                                                string name = jo.name;

                                                int randomid = rnd.Next(0, 99);

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
                                                                    ID = (long)reader["id"],
                                                                    EDSMID = edsmid
                                                                };

                                                                if (System.DBNull.Value == reader["x"])
                                                                {
                                                                    dbsys.X = double.NaN;
                                                                    dbsys.Y = double.NaN;
                                                                    dbsys.Z = double.NaN;
                                                                }
                                                                else
                                                                {
                                                                    dbsys.X = ((double)(long)reader["X"]) / SystemClassDB.XYZScalar;
                                                                    dbsys.Y = ((double)(long)reader["Y"]) / SystemClassDB.XYZScalar;
                                                                    dbsys.Z = ((double)(long)reader["Z"]) / SystemClassDB.XYZScalar;
                                                                }

                                                                dbsys.EDSMID = edsmid;
                                                                dbsys.GridID = reader["GridId"] == DBNull.Value ? 0 : (int)((long)reader["GridId"]);
                                                                dbsys.RandomID = reader["RandomId"] == DBNull.Value ? 0 : (int)((long)reader["RandomId"]);
                                                            }
                                                        }

                                                        if (dbsys != null)
                                                        {
                                                            selectNameCmd.Parameters["@EdsmId"].Value = edsmid;
                                                            using (DbDataReader reader = selectNameCmd.ExecuteReader())
                                                            {
                                                                if (reader.Read())
                                                                {
                                                                    dbsys.Name = (string)reader["Name"];
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                                if (dbsys != null)
                                                {
                                                    // see if EDSM data changed..
                                                    if (!dbsys.Name.Equals(name))
                                                    {
                                                        updateNameCmd.Parameters["@Name"].Value = name;
                                                        updateNameCmd.Parameters["@EdsmId"].Value = edsmid;
                                                        updateNameCmd.ExecuteNonQuery();
                                                    }

                                                    if (Math.Abs(dbsys.X - x) > 0.01 ||
                                                        Math.Abs(dbsys.Y - y) > 0.01 ||
                                                        Math.Abs(dbsys.Z - z) > 0.01 ||
                                                        dbsys.GridID != gridid)  // position changed
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
                                            else
                                            {
                                                //System.Diagnostics.Debug.WriteLine("Reject due to gridid " + gridid);
                                            }
                                        }
                                        else
                                        {
                                            System.Diagnostics.Debug.WriteLine("Reject due to coords ");

                                        }

                                        count++;
                                        blkcount++;
                                    }   // WHILE END
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
            }

            if (cancelRequested())
            {
                throw new OperationCanceledException();
            }

            return updatecount + insertcount;
        }

        #endregion

        #region Sync it up

        public class SystemsSyncState
        {
            public bool perform_edsm_fullsync = false;
            public bool perform_eddb_sync = false;

            public long edsm_fullsync_count = 0;
            public long edsm_updatesync_count = 0;
            public long eddb_sync_count = 0;

            public void ClearCounters()
            {
                eddb_sync_count = 0;
                edsm_fullsync_count = 0;
                edsm_updatesync_count = 0;
            }
        }

        // Called from EDDiscoveryController, in back Init thread, to determine what sync to do on power on..

        public static void DetermineIfFullEDSMSyncRequired(SystemsSyncState state)
        {
            DateTime lastrecordtime = GetLastEDSMRecordTimeUTC();

            // If we do not have a record for at least X days, do a full one since performing X*2 updates is too much

            if (DateTime.UtcNow.Subtract(lastrecordtime).TotalDays >= 28)   // 600k ish per 12hours.  So 33MB.  Much less than a full download which is (23/1/2018) 2400MB, or 600MB compressed
            {
                System.Diagnostics.Debug.WriteLine("EDSM full sync ordered, time since {0}",DateTime.UtcNow.Subtract(lastrecordtime).TotalDays);
                state.perform_edsm_fullsync = true;       // do a full sync.
            }
        }

        // Called from EDDiscoveryController, in DoPerform, on back thread, to determine what sync to do..

        public static long PerformEDSMFullSync(bool[] grididallow, Func<bool> PendingClose, Action<int, string> ReportProgress, Action<string> LogLine, Action<string> LogLineHighlight)
        {
            long updates = 0;

            EDSMClass edsm = new EDSMClass();

            LogLine("Get hidden systems from EDSM and remove from database");

            RemoveHiddenSystems();

            if (PendingClose())
                return updates;

            LogLine("Download systems file from EDSM.");

            string edsmsystems = Path.Combine(EliteConfigInstance.InstanceOptions.AppDataDirectory, "edsmsystems.json");

            LogLine("Resyncing all downloaded EDSM systems with System Database." + Environment.NewLine + "This will take a while.");

            //  string s = edsmsystems; bool success = true; // debug, comment out next two lines

            // with the new downloader, the file is stored, then processed.  If the user cancels, we will use the old file again.  Once complete, the file is removed

            ReportProgress(-1, "Downloading star database from EDSM");

            bool success = BaseUtils.DownloadFile.HTTPDownloadFile(EliteConfigInstance.InstanceConfig.EDSMFullSystemsURL, edsmsystems, false, out bool newfile, (n, s) =>
            {
                SQLiteConnectionSystem.CreateTempSystemsTable();

                DateTime maxdate = new DateTime(2000, 1, 1);

                try
                {
                    using (var reader = new StreamReader(s))
                        updates = ParseEDSMUpdateSystemsStream(reader, grididallow, ref maxdate, true, PendingClose, ReportProgress, useCache: false, useTempSystems: true);
                }
                catch( Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("SysClassEDSM.3 Exception " + e.ToString());
                }

                if (!PendingClose())       // abort, without saving time, to make it do it again
                {
                    LogLine("Replacing old systems table with new systems table and re-indexing - please wait");
                    ReportProgress(-1, "Replacing old systems table with new systems table and re-indexing - please wait");

                    SQLiteConnectionSystem.ReplaceSystemsTable();

                    SetLastEDSMRecordTimeUTC(maxdate);      // record the last record seen in time

                    ReportProgress(-1, "");

                    BaseUtils.FileHelpers.DeleteFileNoError(edsmsystems);       // remove file - don't hold in storage

                    LogLine("System Database updated with EDSM data, " + updates + " systems updated.");

                    GC.Collect();
                }
                else
                {
                    success = false;
                    ReportProgress(-1, "Operation Cancelled");
                    throw new OperationCanceledException();
                }
            }, cancelRequested:PendingClose);

            if (!success)
            {
                ReportProgress(-1, "EDSM Failed to download correctly");
                LogLine("Failed to download EDSM system file from server, will check next time");
            }

            return updates;
        }

        // Partial update sync, do the 12 hour get and store until we come up to UTC time now approx.

        public static long PerformEDSMUpdateSync(bool[] grididallow, Func<bool> PendingClose, Action<int, string> ReportProgress, Action<string> LogLine, Action<string> LogLineHighlight)
        {
            long updates = 0;
            DateTime lastrecordtime = GetLastEDSMRecordTimeUTC();      // this is in UTC, as it comes out of the EDSM records

            if (DateTime.UtcNow.Subtract(lastrecordtime).TotalHours <= 1)  // If we have partial synced for 1 hour, don't waste our time
            {
                System.Diagnostics.Debug.WriteLine("EDSM No partial sync, last record less than 1 hour old");
                return updates;
            }

            // Go For SYNC

            LogLine("Checking for updated EDSM systems (may take a few moments).");

            EDSMClass edsm = new EDSMClass();

            while (lastrecordtime < DateTime.UtcNow.Subtract(new TimeSpan(0,30,0)))     // stop at X mins before now, so we don't get in a condition
            {                                                                           // where we do a set, the time moves to just before now, 
                                                                                        // and we then do another set with minimum amount of hours
                if (PendingClose())     
                    return updates;

                int hourstofetch = 6;

                if (lastrecordtime < ED21date.AddHours(-48))
                    hourstofetch = 48;
                else if (lastrecordtime < ED23date.AddHours(-12))
                    hourstofetch = 12;

                DateTime enddate = lastrecordtime + TimeSpan.FromHours(hourstofetch);
                if (enddate > DateTime.UtcNow)
                    enddate = DateTime.UtcNow;

                LogLine($"Downloading systems from UTC {lastrecordtime.ToUniversalTime().ToString()} to {enddate.ToUniversalTime().ToString()}");
                ReportProgress(-1, "Requesting systems from EDSM");
                System.Diagnostics.Debug.WriteLine($"Downloading systems from UTC {lastrecordtime.ToUniversalTime().ToString()} to {enddate.ToUniversalTime().ToString()}");

                string json = null;
                BaseUtils.ResponseData response;
                try
                {
                    response = edsm.RequestSystemsData(lastrecordtime, enddate, timeout: 20000);
                }
                catch (WebException ex)
                {
                    ReportProgress(-1, $"EDSM request failed");
                    if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null && ex.Response is HttpWebResponse)
                    {
                        string status = ((HttpWebResponse)ex.Response).StatusDescription;
                        LogLine($"Download of EDSM systems from the server failed ({status}), will try next time program is run");
                    }
                    else
                    {
                        LogLine($"Download of EDSM systems from the server failed ({ex.Status.ToString()}), will try next time program is run");
                    }

                    return updates;
                }
                catch (Exception ex)
                {
                    ReportProgress(-1, $"EDSM request failed");
                    LogLine($"Download of EDSM systems from the server failed ({ex.Message}), will try next time program is run");
                    return updates;
                }

                if (response.Error)
                {
                    if ((int)response.StatusCode == 429)
                    {
                        LogLine($"EDSM rate limit hit - waiting 2 minutes");
                        for (int sec = 0; sec < 120; sec++)
                        {
                            if (!PendingClose())
                            {
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                    }
                    else
                    {
                        LogLine($"Download of EDSM systems from the server failed ({response.StatusCode.ToString()}), will try next time program is run");
                        return updates;
                    }
                }

                json = response.Body;

                if (json == null)
                {
                    ReportProgress(-1, "EDSM request failed");
                    LogLine("Download of EDSM systems from the server failed (no data returned), will try next time program is run");
                    return updates;
                }

                // debug File.WriteAllText(@"c:\code\json.txt", json);

                DateTime prevrectime = lastrecordtime;
                System.Diagnostics.Debug.WriteLine("Last record time {0} JSON size {1}", lastrecordtime.ToUniversalTime() , json.Length);

                long updated = 0;

                try
                {
                    updated = ParseEDSMUpdateSystemsString(json, grididallow, ref lastrecordtime, false, PendingClose, ReportProgress, false);
                    System.Diagnostics.Debug.WriteLine($".. Updated {updated} to {lastrecordtime.ToUniversalTime().ToString()}");
                    System.Diagnostics.Debug.WriteLine("Updated to time {0}", lastrecordtime.ToUniversalTime());

                    // if lastrecordtime did not change (=) or worse still, EDSM somehow moved the time back (unlikely)
                    if (lastrecordtime <= prevrectime)
                    {
                        lastrecordtime += TimeSpan.FromHours(12);       // Lets move on manually so we don't get stuck
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("SysClassEDSM.2 Exception " + e.ToString());
                    ReportProgress(-1, "EDSM request failed");
                    LogLine("Processing EDSM systems download failed, will try next time program is run");
                    return updates;
                }

                updates += updated;

                SetLastEDSMRecordTimeUTC(lastrecordtime);       // keep on storing this in case next time we get an exception

                int delay = 10;     // Anthor's normal delay 
                int ratelimitlimit;
                int ratelimitremain;
                int ratelimitreset;

                if (response.Headers != null &&
                    response.Headers["X-Rate-Limit-Limit"] != null &&
                    response.Headers["X-Rate-Limit-Remaining"] != null &&
                    response.Headers["X-Rate-Limit-Reset"] != null &&
                    Int32.TryParse(response.Headers["X-Rate-Limit-Limit"], out ratelimitlimit) &&
                    Int32.TryParse(response.Headers["X-Rate-Limit-Remaining"], out ratelimitremain) &&
                    Int32.TryParse(response.Headers["X-Rate-Limit-Reset"], out ratelimitreset) )
                {
                    if (ratelimitremain < ratelimitlimit * 3 / 4)       // lets keep at least X remaining for other purposes later..
                        delay = ratelimitreset / (ratelimitlimit - ratelimitremain);    // slow down to its pace now.. example 878/(360-272) = 10 seconds per quota
                    else
                        delay = 0;

                    System.Diagnostics.Debug.WriteLine("EDSM Delay Parameters {0} {1} {2} => {3}s", ratelimitlimit, ratelimitremain, ratelimitreset, delay);
                }

                for (int sec = 0; sec < delay; sec++)
                {
                    if (!PendingClose())
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                }
            }

            return updates;
        }

        // Time storers

        static public void ForceEDSMFullUpdate()
        {
            SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", "2010-01-01 00:00:00");
        }

        static public DateTime GetLastEDSMRecordTimeUTC()
        {
            string rwsystime = SQLiteConnectionSystem.GetSettingString("EDSMLastSystems", "2000-01-01 00:00:00"); // Latest time from RW file.
            DateTime edsmdate;

            if (!DateTime.TryParse(rwsystime, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out edsmdate))
                edsmdate = new DateTime(2000, 1, 1);

            return edsmdate;
        }

        static public void SetLastEDSMRecordTimeUTC(DateTime time)
        {
            SQLiteConnectionSystem.PutSettingString("EDSMLastSystems", time.ToString(CultureInfo.InvariantCulture));
            System.Diagnostics.Debug.WriteLine("Last EDSM record " + time.ToString());
        }

        #endregion

    }
}
