/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using SQLLiteExtensions;
using System.Data.Common;
using System.Data;
using Newtonsoft.Json.Linq;

namespace EliteDangerousCore.DB
{
    public partial class SystemsDB
    {
        #region Table Update from JSON FILE

        public static long ParseEDSMJSONFile(string filename, bool[] grididallow, ref DateTime date, Func<bool> cancelRequested, Action<string> reportProgress, string tableposfix, bool presumeempty = false, string debugoutputfile = null)
        {
            using (StreamReader sr = new StreamReader(filename))         // read directly from file..
                return ParseEDSMJSON(sr, grididallow, ref date, cancelRequested, reportProgress, tableposfix, presumeempty, debugoutputfile);
        }

        public static long ParseEDSMJSONString(string data, bool[] grididallow, ref DateTime date, Func<bool> cancelRequested, Action<string> reportProgress, string tableposfix, bool presumeempty = false, string debugoutputfile = null)
        {
            using (StringReader sr = new StringReader(data))         // read directly from file..
                return ParseEDSMJSON(sr, grididallow, ref date, cancelRequested, reportProgress, tableposfix, presumeempty, debugoutputfile);
        }

        public static long ParseEDSMJSON(TextReader sr, bool[] grididallow, ref DateTime date, Func<bool> cancelRequested, Action<string> reportProgress, string tablepostfix, bool presumeempty = false, string debugoutputfile = null)
        {
            using (JsonTextReader jr = new JsonTextReader(sr))
                return ParseEDSMJSON(jr, grididallow, ref date, cancelRequested, reportProgress, tablepostfix, presumeempty, debugoutputfile);
        }

        // set tempostfix to use another set of tables

        public static long ParseEDSMJSON(JsonTextReader jr, 
                                        bool[] grididallowed,       // null = all, else grid bool value
                                        ref DateTime maxdate,       // updated with latest date
                                        Func<bool> cancelRequested,
                                        Action<string> reportProgress,
                                        string tablepostfix,        // set to add on text to table names to redirect to another table
                                        bool tablesareempty = false,     // set to presume table is empty, so we don't have to do look up queries
                                        string debugoutputfile = null
                                        )
        {
            var cache = new SectorCache();

            long updates = 0;

            int nextsectorid = GetNextSectorID();
            bool jr_eof = false;
            StreamWriter sw = null;

            try
            {
#if DEBUG
                try
                {
                    if (debugoutputfile != null) sw = new StreamWriter(debugoutputfile);
                }
                catch
                {
                }
#endif

                while (jr_eof == false)
                {
                    if (cancelRequested())
                    {
                        updates = -1;
                        break;
                    }

                    int recordstostore = ProcessBlock(cache, jr, grididallowed, tablesareempty, tablepostfix, ref maxdate, ref nextsectorid, ref jr_eof);

                    System.Diagnostics.Debug.WriteLine("Process " + BaseUtils.AppTicks.TickCountLap("L1") + "   " + updates);

                    if (recordstostore > 0)
                    {
                        updates += StoreNewEntries(cache, tablepostfix, sw);

                        reportProgress?.Invoke("EDSM Star database updated " + updates);
                    }

                    if (jr_eof)
                        break;

                    if (SQLiteConnectionSystem.IsReadWaiting)
                    {
                        System.Threading.Thread.Sleep(20);      // just sleepy for a bit to let others use the db
                    }
                }
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }

            System.Diagnostics.Debug.WriteLine("Process " + BaseUtils.AppTicks.TickCountLap("L1") + "   " + updates);
            reportProgress?.Invoke("EDSM Star database updated " + updates);


            PutNextSectorID(nextsectorid);    // and store back

            return updates;
        }

        #endregion

        #region UPGRADE FROM 102

        // take old system table and turn to new.  tablesarempty=false is normal, only set to true if using this code for checking replace algorithm

        public static long UpgradeDB102to200(Func<bool> cancelRequested, Action<string> reportProgress, string tablepostfix, bool tablesareempty = false, int maxgridid = int.MaxValue)
        {
            var cache = new SectorCache();

            int nextsectorid = GetNextSectorID();
            long updates = 0;

            long Limit = long.MaxValue;

            DateTime maxdate = DateTime.MinValue;       // we don't pass this back due to using the same date
            reportProgress?.Invoke("Begin System DB upgrade");
            List<int> gridids = DB.GridId.AllId();
            BaseUtils.AppTicks.TickCountLap("UTotal");

            //int debug_z = 0;

            foreach (int gridid in gridids)  // using grid id to keep chunks a good size.. can't read and write so can't just read the whole.
            {
                if (cancelRequested())
                {
                    updates = -1;
                    break;
                }

                if (gridid == maxgridid)        // for debugging
                    break;

                int recordstostore = 0;
                DbCommand selectSectorCmd = null;
                DbCommand selectPrev = null;

                SystemsDatabase.Instance.ExecuteWithDatabase(db =>
                {
                    try
                    {
                        var cn = db.Connection;

                        selectSectorCmd = cn.CreateSelect("Sectors" + tablepostfix, "id", "name = @sname AND gridid = @gid", null,
                                                                new string[] { "sname", "gid" }, new DbType[] { DbType.String, DbType.Int32 });
                        selectPrev = cn.CreateSelect("EdsmSystems s", "s.EdsmId,s.x,s.y,s.z,n.Name,s.UpdateTimeStamp", "s.GridId = @gid", null,
                                                                new string[] { "gid" }, new DbType[] { DbType.Int32 },
                                                                joinlist: new string[] { "LEFT OUTER JOIN SystemNames n ON n.EdsmId=s.EdsmId" });

                        selectPrev.Parameters["gid"].Value = gridid;

                        using (DbDataReader reader = selectPrev.ExecuteReader())       // find name:gid
                        {
                            BaseUtils.AppTicks.TickCountLap("U1");

                            while (reader.Read())
                            {
                                try
                                {
                                    EDSMFileEntry d = new EDSMFileEntry();
                                    d.id = (long)reader[0];
                                    d.x = (int)(long)reader[1];
                                    d.y = (int)(long)reader[2];
                                    d.z = (int)(long)reader[3];
                                    d.name = (string)reader[4];
                                    d.date = new DateTime(2015, 1, 1, 0, 0, 0, DateTimeKind.Utc) + TimeSpan.FromSeconds((long)reader["UpdateTimestamp"]);
                                    int grididentry = GridId.Id(d.x, d.z);  // because i don't trust the previous gridid - it should be the same as the outer loop, but lets recalc

                                    //if (!tablesareempty)  d.z = debug_z++;  // for debug checking

                                    CreateNewUpdate(cache, selectSectorCmd, d, grididentry, tablesareempty, ref maxdate, ref nextsectorid);      // not using gridid on purpose to double check it.
                                    recordstostore++;
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine("Reading prev table" + ex);
                                }
                            }
                        }
                    }
                    finally
                    {
                        selectSectorCmd?.Dispose();
                    }
                });

                //System.Diagnostics.Debug.WriteLine("Reader took " + BaseUtils.AppTicks.TickCountLap("U1") + " in " + gridid + "  " + recordpos + " total " + recordstostore);

                if (recordstostore >= 0)
                {
                    updates += StoreNewEntries(cache, tablepostfix, null);
                    reportProgress?.Invoke("System DB upgrade processed " + updates);

                    Limit -= recordstostore;

                    if (Limit <= 0)
                        break;

                    if (SQLiteConnectionSystem.IsReadWaiting)
                    {
                        System.Threading.Thread.Sleep(20);      // just sleepy for a bit to let others use the db
                    }
                }

                var tres1 = BaseUtils.AppTicks.TickCountLapDelta("U1");
                var tres2 = BaseUtils.AppTicks.TickCountFrom("UTotal");
                System.Diagnostics.Debug.WriteLine("Sector " + gridid + " took " + tres1.Item1 + " store " + recordstostore + " total " + updates + " " + ((float)tres1.Item2 / (float)recordstostore) + " cumulative " + tres2);
            }

            reportProgress?.Invoke("System DB complete, processed " + updates);

            PutNextSectorID(nextsectorid);    // and store back

            return updates;
        }

        #endregion


        #region Table Update Helpers

        private static int ProcessBlock(SectorCache cache,
                                         JsonTextReader jr,
                                         bool[] grididallowed,       // null = all, else grid bool value
                                         bool tablesareempty,
                                         string tablepostfix,
                                         ref DateTime maxdate,       // updated with latest date
                                         ref int nextsectorid,
                                         ref bool jr_eof)
        {
            int recordstostore = 0;
            DbCommand selectSectorCmd = null;
            DateTime cpmaxdate = maxdate;
            int cpnextsectorid = nextsectorid;
            const int BlockSize = 10000;
            int Limit = int.MaxValue;
            var entries = new List<TableWriteData>();

            while (jr_eof == false)
            {
                try
                {
                    if (jr.Read())
                    {
                        if (jr.TokenType == JsonToken.StartObject)
                        {
                            EDSMFileEntry d = new EDSMFileEntry();

                            if (d.Deserialize(jr) && d.id >= 0 && d.name.HasChars() && d.z != int.MinValue)     // if we have a valid record
                            {
                                int gridid = GridId.Id(d.x, d.z);
                                if (grididallowed == null || (grididallowed.Length > gridid && grididallowed[gridid]))    // allows a null or small grid
                                {
                                    TableWriteData data = new TableWriteData() { edsm = d, classifier = new EliteNameClassifier(d.name), gridid = gridid };

                                    if (!TryCreateNewUpdate(cache, data, tablesareempty, ref cpmaxdate, ref cpnextsectorid, out Sector sector))
                                    {
                                        entries.Add(data);
                                    }

                                    recordstostore++;
                                }
                            }

                            if (--Limit == 0)
                            {
                                jr_eof = true;
                                break;
                            }

                            if (recordstostore >= BlockSize)
                                break;
                        }
                    }
                    else
                    {
                        jr_eof = true;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("EDSM JSON file exception " + ex.ToString());
                    jr_eof = true;                                                                              // stop read, but let it continue to finish this section
                }
            }

            SystemsDatabase.Instance.ExecuteWithDatabase(mode: SQLExtConnection.AccessMode.Reader, action: db =>
            {
                try
                {
                    var cn = db.Connection;

                    selectSectorCmd = cn.CreateSelect("Sectors" + tablepostfix, "id", "name = @sname AND gridid = @gid", null,
                                                            new string[] { "sname", "gid" }, new DbType[] { DbType.String, DbType.Int32 });

                    foreach (var entry in entries)
                    {
                        CreateNewUpdate(cache, selectSectorCmd, entry, tablesareempty, ref cpmaxdate, ref cpnextsectorid);
                    }
                }
                finally
                {
                    if (selectSectorCmd != null)
                    {
                        selectSectorCmd.Dispose();
                    }
                }
            });

            maxdate = cpmaxdate;
            nextsectorid = cpnextsectorid;

            return recordstostore;
        }


        // create a new entry for insert in the sector tables 
        private static void CreateNewUpdate(SectorCache cache, DbCommand selectSectorCmd, EDSMFileEntry d, int gid, bool tablesareempty, ref DateTime maxdate, ref int nextsectorid)
        {
            TableWriteData data = new TableWriteData() { edsm = d, classifier = new EliteNameClassifier(d.name), gridid = gid };
            CreateNewUpdate(cache, selectSectorCmd, data, tablesareempty, ref maxdate, ref nextsectorid);
        }

        private static bool TryCreateNewUpdate(SectorCache cache, TableWriteData data, bool tablesareempty, ref DateTime maxdate, ref int nextsectorid, out Sector t, bool makenew = false)
        {
            if (data.edsm.date > maxdate)                                   // for all, record last recorded date processed
                maxdate = data.edsm.date;

            Sector prev = null;

            t = null;

            if (!cache.SectorNameCache.ContainsKey(data.classifier.SectorName))   // if unknown to cache
            {
                if (!tablesareempty && !makenew)
                {
                    return false;
                }

                cache.SectorNameCache[data.classifier.SectorName] = t = new Sector(data.classifier.SectorName, gridid: data.gridid);   // make a sector of sectorname and with gridID n , id == -1
            }
            else
            {
                t = cache.SectorNameCache[data.classifier.SectorName];        // find the first sector of name
                while (t != null && t.GId != data.gridid)        // if GID of sector disagrees
                {
                    prev = t;                          // go thru list
                    t = t.NextSector;
                }

                if (t == null)      // still not got it, its a new one.
                {
                    if (!tablesareempty && !makenew)
                    {
                        return false;
                    }

                    prev.NextSector = t = new Sector(data.classifier.SectorName, gridid: data.gridid);   // make a sector of sectorname and with gridID n , id == -1
                }
            }

            if (t.Id == -1)   // if unknown sector ID..
            {
                if (tablesareempty)     // if tables are empty, we can just presume its id
                {
                    t.Id = nextsectorid++;      // insert the sector with the guessed ID
                    t.insertsec = true;
                    cache.SectorIDCache[t.Id] = t;    // and cache
                    //System.Diagnostics.Debug.WriteLine("Made sector " + t.Name + ":" + t.GId);
                }
            }

            if (t.edsmdatalist == null)
                t.edsmdatalist = new List<TableWriteData>(5000);

            t.edsmdatalist.Add(data);                       // add to list of systems to process for this sector

            return true;
        }

        private static void CreateNewUpdate(SectorCache cache, DbCommand selectSectorCmd, TableWriteData data, bool tablesareempty, ref DateTime maxdate, ref int nextsectorid)
        {
            TryCreateNewUpdate(cache, data, tablesareempty, ref maxdate, ref nextsectorid, out Sector t, true);

            if (t.Id == -1)   // if unknown sector ID..
            {
                selectSectorCmd.Parameters[0].Value = t.Name;   
                selectSectorCmd.Parameters[1].Value = t.GId;

                using (DbDataReader reader = selectSectorCmd.ExecuteReader())       // find name:gid
                {
                    if (reader.Read())      // if found name:gid
                    {
                        t.Id = (long)reader[0];
                    }
                    else
                    {
                        t.Id = nextsectorid++;      // insert the sector with the guessed ID
                        t.insertsec = true;
                    }

                    cache.SectorIDCache[t.Id] = t;                // and cache
                    //  System.Diagnostics.Debug.WriteLine("Made sector " + t.Name + ":" + t.GId);
                }
            }
        }

        private static long StoreNewEntries(SectorCache cache, string tablepostfix = "",        // set to add on text to table names to redirect to another table
                                           StreamWriter sw = null
                                        )
        {
            ////////////////////////////////////////////////////////////// push all new data to the db without any selects

            return SystemsDatabase.Instance.ExecuteWithDatabase(mode: SQLExtConnection.AccessMode.Writer, usetxnlock: true, func: db =>
            {
                long updates = 0;

                DbTransaction txn = null;
                DbCommand replaceSectorCmd = null;
                DbCommand replaceSysCmd = null;
                DbCommand replaceNameCmd = null;
                try
                {
                    var cn = db.Connection;
                    txn = cn.BeginTransaction();

                    replaceSectorCmd = cn.CreateReplace("Sectors" + tablepostfix, new string[] { "name", "gridid", "id" }, new DbType[] { DbType.String, DbType.Int32, DbType.Int64 }, txn);

                    replaceSysCmd = cn.CreateReplace("Systems" + tablepostfix, new string[] { "sectorid", "nameid", "x", "y", "z", "edsmid" },
                                        new DbType[] { DbType.Int64, DbType.Int64, DbType.Int32, DbType.Int32, DbType.Int32, DbType.Int64 }, txn);

                    replaceNameCmd = cn.CreateReplace("Names" + tablepostfix, new string[] { "name", "id" }, new DbType[] { DbType.String, DbType.Int64 }, txn);

                    foreach (var kvp in cache.SectorIDCache)                  // all sectors cached, id is unique so its got all sectors                           
                    {
                        Sector t = kvp.Value;

                        if (t.insertsec)         // if we have been told to insert the sector, do it
                        {
                            replaceSectorCmd.Parameters[0].Value = t.Name;     // make a new one so we can get the ID
                            replaceSectorCmd.Parameters[1].Value = t.GId;
                            replaceSectorCmd.Parameters[2].Value = t.Id;        // and we insert with ID, managed by us, and replace in case there are any repeat problems (which there should not be)
                            replaceSectorCmd.ExecuteNonQuery();
                            //System.Diagnostics.Debug.WriteLine("Written sector " + t.GId + " " +t.Name);
                            t.insertsec = false;
                        }

                        if (t.edsmdatalist != null)       // if updated..
                        {
#if DEBUG
                            t.edsmdatalist.Sort(delegate (TableWriteData left, TableWriteData right) { return left.edsm.id.CompareTo(right.edsm.id); });
#endif

                            foreach (var data in t.edsmdatalist)            // now write the star list in this sector
                            {
                                try
                                {
                                    if (data.classifier.IsNamed)    // if its a named entry, we need a name
                                    {
                                        data.classifier.NameIdNumeric = data.edsm.id;           // name is the edsm id
                                        replaceNameCmd.Parameters[0].Value = data.classifier.StarName;       // insert a new name
                                        replaceNameCmd.Parameters[1].Value = data.edsm.id;      // we use edsmid as the nameid, and use replace to ensure that if a prev one is there, its replaced
                                        replaceNameCmd.ExecuteNonQuery();
                                        // System.Diagnostics.Debug.WriteLine("Make name " + data.classifier.NameIdNumeric);
                                    }

                                    replaceSysCmd.Parameters[0].Value = t.Id;
                                    replaceSysCmd.Parameters[1].Value = data.classifier.ID;
                                    replaceSysCmd.Parameters[2].Value = data.edsm.x;
                                    replaceSysCmd.Parameters[3].Value = data.edsm.y;
                                    replaceSysCmd.Parameters[4].Value = data.edsm.z;
                                    replaceSysCmd.Parameters[5].Value = data.edsm.id;       // in the event a new entry has the same edsmid, the system table edsmid is replace with new data
                                    replaceSysCmd.ExecuteNonQuery();

                                    if (sw != null)
                                        sw.WriteLine(data.edsm.name + " " + data.edsm.x + "," + data.edsm.y + "," + data.edsm.z + ", EDSM:" + data.edsm.id + " Grid:" + data.gridid);

                                    updates++;
                                }
                                catch (Exception ex)
                                {
                                    System.Diagnostics.Debug.WriteLine("general exception during insert - ignoring " + ex.ToString());
                                }

                            }
                        }

                        t.edsmdatalist = null;     // and delete back
                    }

                    txn.Commit();

                    return updates;
                }
                finally
                {
                    replaceSectorCmd?.Dispose();
                    replaceSysCmd?.Dispose();
                    replaceNameCmd?.Dispose();
                    txn?.Dispose();
                }
            });
        }

        #endregion

        #region Upgrade from 102

        #endregion

        #region Internal Vars and Classes

        private static int GetNextSectorID() { return SystemsDatabase.Instance.GetEDSMSectorIDNext(); }
        private static void PutNextSectorID(int v) { SystemsDatabase.Instance.SetEDSMSectorIDNext(v); }  

        private class SectorCache
        {
            public Dictionary<long, Sector> SectorIDCache { get; set; } = new Dictionary<long, Sector>();          // only used during store operation
            public Dictionary<string, Sector> SectorNameCache { get; set; } = new Dictionary<string, Sector>();
        }

        private class Sector
        {
            public long Id;
            public int GId;
            public string Name;

            public Sector NextSector;       // memory only field, link to next in list

            public Sector(string name, long id = -1, int gridid = -1 )
            {
                this.Name = name;
                this.GId = gridid;
                this.Id = id;
                this.NextSector = null;
            }

            // for write table purposes only

            public List<TableWriteData> edsmdatalist;
            public bool insertsec = false;
        };

        private class TableWriteData
        {
            public EDSMFileEntry edsm;
            public EliteNameClassifier classifier;
            public int gridid;
        }

        public class EDSMFileEntry
        {
            public bool Deserialize(JsonReader rdr)
            {
                while (rdr.Read() && rdr.TokenType == JsonToken.PropertyName)
                {
                    string field = rdr.Value as string;
                    switch (field)
                    {
                        case "name":
                            name = rdr.ReadAsString();
                            break;
                        case "id":
                            id = rdr.ReadAsInt32() ?? 0;
                            break;
                        case "date":
                            date = rdr.ReadAsDateTime() ?? DateTime.MinValue;
                            break;
                        case "coords":
                            {
                                if (rdr.TokenType != JsonToken.StartObject)
                                    rdr.Read();

                                while (rdr.Read() && rdr.TokenType == JsonToken.PropertyName)
                                {
                                    field = rdr.Value as string;
                                    double? v = rdr.ReadAsDouble();
                                    if (v == null)
                                        return false;
                                    int vi = (int)(v * SystemClass.XYZScalar);

                                    switch (field)
                                    {
                                        case "x":
                                            x = vi;
                                            break;
                                        case "y":
                                            y = vi;
                                            break;
                                        case "z":
                                            z = vi;
                                            break;
                                    }
                                }

                                break;
                            }
                        default:
                            rdr.Read();
                            JToken.Load(rdr);
                            break;
                    }
                }

                return true;
            }

            public string name;
            public long id = -1;
            public DateTime date;
            public int x = int.MinValue;
            public int y = int.MinValue;
            public int z = int.MinValue;
        }

        #endregion
    }
}


