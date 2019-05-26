/*
 * Copyright © 2016-2019 EDDiscovery development team
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

using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;

namespace EliteDangerousCore
{
    // DB accessor part of this class

    public abstract partial class JournalEntry
    {
        static protected JournalEntry CreateJournalEntry(DataRow dr)
        {
            string EDataString = (string)dr["EventData"];

            JournalEntry jr = JournalEntry.CreateJournalEntry(EDataString);     // this sets EventTypeId, EventTypeStr and UTC via constructor above.. 

            jr.Id = (int)(long)dr["Id"];
            jr.TLUId = (int)(long)dr["TravelLogId"];
            jr.CommanderId = (int)(long)dr["CommanderId"];
            if (jr.EventTimeUTC == default(DateTime))
                jr.EventTimeUTC = (DateTime)dr["EventTime"];
            if (jr.EventTypeID == JournalTypeEnum.Unknown)
                jr.EventTypeID = (JournalTypeEnum)(long)dr["eventTypeID"];
            jr.EdsmID = (long)dr["EdsmID"];
            jr.Synced = (int)(long)dr["Synced"];
            return jr;
        }

        static protected JournalEntry CreateJournalEntry(DbDataReader dr)
        {
            string EDataString = (string)dr["EventData"];

            JournalEntry jr = JournalEntry.CreateJournalEntry(EDataString);

            jr.Id = (int)(long)dr["Id"];
            jr.TLUId = (int)(long)dr["TravelLogId"];
            jr.CommanderId = (int)(long)dr["CommanderId"];
            if (jr.EventTimeUTC == default(DateTime))
                jr.EventTimeUTC = ((DateTime)dr["EventTime"]).ToUniversalTime();
            if (jr.EventTypeID == JournalTypeEnum.Unknown)
                jr.EventTypeID = (JournalTypeEnum)(long)dr["eventTypeID"];
            jr.EdsmID = (long)dr["EdsmID"];
            jr.Synced = (int)(long)dr["Synced"];
            return jr;
        }


        public bool Add(JObject jo)
        {
            Id = UserDatabase.Instance.Add<long>("JournalEntries", "id", new Dictionary<string, object>
            {
                ["EventTime"] = EventTimeUTC,
                ["TravelLogID"] = TLUId,
                ["CommanderId"] = CommanderId,
                ["EventTypeID"] = (int)EventTypeID,
                ["EventType"] = EventTypeStr,
                ["EventData"] = jo.ToString(),
                ["EdsmId"] = EdsmID,
                ["Synced"] = Synced
            });

            return true;
        }

        public class RowInserter : IDisposable
        {
            private IRowInserter<long> Inserter;

            public RowInserter(IUserDatabase db)
            {
                Inserter = db.CreateInserter<long>("JournalEntries", "id", new Dictionary<string, DbType>
                {
                    ["EventTime"] = DbType.DateTime,
                    ["TravelLogID"] = DbType.Int64,
                    ["CommanderId"] = DbType.Int32,
                    ["EventTypeID"] = DbType.Int32,
                    ["EventType"] = DbType.String,
                    ["EventData"] = DbType.String,
                    ["EdsmId"] = DbType.Int64,
                    ["Synced"] = DbType.Int32
                });
            }

            public bool Add(JournalEntry entry, JObject jo)
            {
                entry.Id = Inserter.Add(new Dictionary<string, object>
                {
                    ["EventTime"] = entry.EventTimeUTC,
                    ["TravelLogID"] = entry.TLUId,
                    ["CommanderId"] = entry.CommanderId,
                    ["EventTypeID"] = (int)entry.EventTypeID,
                    ["EventType"] = entry.EventTypeStr,
                    ["EventData"] = jo.ToString(),
                    ["EdsmId"] = entry.EdsmID,
                    ["Synced"] = entry.Synced
                });

                return true;
            }

            public void Commit()
            {
                Inserter.Commit();
            }

            public void Dispose()
            {
                if (Inserter != null)
                {
                    Inserter.Dispose();
                    Inserter = null;
                }
            }
        }

        public class RowUpdater : IDisposable
        {
            private IRowUpdater<long> Updater;

            public RowUpdater(IUserDatabase db)
            {
                Updater = db.CreateUpdater<long>("JournalEntries", "id");
            }

            public void Update(Dictionary<string, object> fields, string where, Dictionary<string, object> whereparams)
            {
                Updater.Update(fields, where, whereparams);
            }

            public void Update(long id, Dictionary<string, object> fields)
            {
                Updater.Update(id, fields);
            }

            public TOut Retrieve<TOut>(long id, Func<DbDataReader, TOut> func)
            {
                return Updater.Retrieve<TOut>(id, func);
            }

            public JournalEntry Retrieve(long id)
            {
                return Updater.Retrieve(id, rdr => JournalEntry.CreateJournalEntry(rdr));
            }

            public JObject GetJson(long id)
            {
                string json = Updater.Retrieve(id, rdr => (string)rdr["EventData"]);

                try
                {
                    return JObject.Parse(json);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error parsing journal entry\n{json}\n{ex.ToString()}");
                    return null;
                }
            }

            public void Commit()
            {
                Updater.Commit();
            }

            public void Dispose()
            {
                if (Updater != null)
                {
                    Updater.Dispose();
                    Updater = null;
                }
            }
        }

        public static void ExecuteWithInserter(Action<RowInserter> action, bool usetxn = false, bool utc = true)
        {
            UserDatabase.Instance.ExecuteWithDatabase(db =>
            {
                using (var inserter = new RowInserter(db))
                {
                    action(inserter);
                }
            }, usetxn, utc, SQLLiteExtensions.SQLExtConnection.AccessMode.Writer);
        }

        public static T ExecuteWithInserter<T>(Func<RowInserter, T> func, bool usetxn = false, bool utc = true)
        {
            return UserDatabase.Instance.ExecuteWithDatabase(db =>
            {
                using (var inserter = new RowInserter(db))
                {
                    return func(inserter);
                }
            }, usetxn, utc, SQLLiteExtensions.SQLExtConnection.AccessMode.Writer);
        }

        public static void ExecuteWithUpdater(Action<RowUpdater> action, bool usetxn = false, bool utc = true)
        {
            UserDatabase.Instance.ExecuteWithDatabase(db =>
            {
                using (var updater = new RowUpdater(db))
                {
                    action(updater);
                }
            }, usetxn, utc, SQLLiteExtensions.SQLExtConnection.AccessMode.Writer);
        }

        public static T ExecuteWithUpdater<T>(Func<RowUpdater, T> action, bool usetxn = false, bool utc = true)
        {
            return UserDatabase.Instance.ExecuteWithDatabase(db =>
            {
                using (var updater = new RowUpdater(db))
                {
                    return action(updater);
                }
            }, usetxn, utc, SQLLiteExtensions.SQLExtConnection.AccessMode.Writer);
        }

        public bool Update()
        {
            UserDatabase.Instance.Update("JournalEntries", "id", Id, new Dictionary<string, object>
            {
                ["EventTime"] = EventTimeUTC,
                ["TravelLogID"] = TLUId,
                ["CommanderId"] = CommanderId,
                ["EventTypeID"] = (int)EventTypeID,
                ["EventType"] = EventTypeStr,
                ["EdsmId"] = EdsmID,
                ["Synced"] = Synced
            });

            return true;
        }

        protected void UpdateJsonEntry(JObject jo)
        {
            ExecuteWithUpdater(updater => UpdateJsonEntry(jo, updater));
        }

        protected void UpdateJsonEntry(JObject jo, RowUpdater updater)
        {
            updater.Update(Id, new Dictionary<string, object> { ["EventData"] = jo.ToString() });
        }

        static public void Delete(long idvalue)
        {
            UserDatabase.Instance.Delete("JournalEntries", "id", idvalue);
        }

        static private void Delete(long idvalue, IUserDatabase db)
        {
            db.Delete("JournalEntries", "id", idvalue);
        }

        public void UpdateMapColour(int v)
        {
            JournalFSDJump fsd = this as JournalFSDJump;        // only update if fsd
            if (fsd != null)
                fsd.SetMapColour(v);
        }

        public static void UpdateEDSMIDPosJump(long journalid, ISystem system, bool jsonpos, double dist)
        {
            ExecuteWithUpdater(updater => UpdateEDSMIDPosJump(journalid, system, jsonpos, dist, updater));
        }

        //dist >0 to update
        public static void UpdateEDSMIDPosJump(long journalid, ISystem system, bool jsonpos, double dist, RowUpdater updater)
        {
            bool updatejson = jsonpos || dist > 0;
            JObject jo = updatejson ? GetJson(journalid) : null;       // if JSON pos update, get it, else null
                                                                        // no need to JSON read if just doing an EDSM update
            if (jo != null || !updatejson)        // if got it, or no pos
            {
                if (jsonpos)
                {
                    jo["StarPos"] = new JArray() { system.X, system.Y, system.Z };
                    jo["StarPosFromEDSM"] = true;
                }

                if (dist > 0)
                    jo["JumpDist"] = dist;

                Dictionary<string, object> updatefields = new Dictionary<string, object>
                {
                    ["EdsmId"] = system.EDSMID
                };

                if (updatejson)
                {
                    updatefields["EventData"] = jo.ToString();
                    Trace.WriteLine(string.Format("Update journal ID {0} with pos/edsmid {1} dist {2}", journalid, system.EDSMID, dist));
                }
                else
                {
                    Trace.WriteLine(string.Format("Update journal ID {0} with edsmid {1}", journalid, system.EDSMID));
                }

                updater.Update(journalid, updatefields);
            }
        }

        private void UpdateSyncFlagBit(SyncFlags bit1, bool value1, SyncFlags bit2, bool value2)
        {
            ExecuteWithUpdater(updater => UpdateSyncFlagBit(bit1, value1, bit2, value2));
        }

        private void UpdateSyncFlagBit(SyncFlags bit1, bool value1, SyncFlags bit2, bool value2, RowUpdater updater)
        {
            if (value1)
                Synced |= (int)bit1;
            else
                Synced &= ~(int)bit1;

            if (value2)
                Synced |= (int)bit2;
            else
                Synced &= ~(int)bit2;

            updater.Update(Id, new Dictionary<string, object>
            {
                ["Synced"] = Synced
            });
            Trace.WriteLine(string.Format("Update sync flag ID {0} with {1}", Id, Synced));
        }

        public void UpdateCommanderID(int cmdrid)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Update cmdr id ID {0}", Id));

            UserDatabase.Instance.Update("JournalEntries", "Id", Id, new Dictionary<string, object>
            {
                ["CommanderID"] = cmdrid
            });

            CommanderId = cmdrid;
        }

        static public bool ResetCommanderID(int from, int to)
        {
            System.Diagnostics.Trace.WriteLine(string.Format("Update cmdr id ID {0} with {1}", from, to));

            string where = null;
            Dictionary<string, object> whereparams = null;

            if (from != -1)
            {
                where = "CommanderID = @CommanderID";
                whereparams = new Dictionary<string, object> { ["CommanderID"] = from };
            }

            UserDatabase.Instance.Update("JournalEntries", new Dictionary<string, object> { ["CommanderID"] = to }, where, whereparams);
            return true;
        }

        public JObject GetJson()
        {
            return GetJson(Id);
        }

        static public JObject GetJson(long journalid)
        {
            var events = UserDatabase.Instance.Retrieve("JournalEntries", rdr => (string)rdr["EventData"], new[] { "EventData" }, "Id = @Id", new Dictionary<string, object> { ["Id"] = journalid });

            if (events.Count == 1)
            {
                try
                {
                    return JObject.Parse(events[0]);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine($"Error parsing journal entry\n{events[0]}\n{ex.ToString()}");
                    return null;
                }
            }

            return null;
        }

        static public JournalEntry Get(long journalid)
        {
            var entry = UserDatabase.Instance.Retrieve("JournalEntries", "Id", journalid, rdr => CreateJournalEntry(rdr));

            if (entry != null)
            {
                var tlu = TravelLogUnit.Get(entry.TLUId);
                entry.beta = tlu?.Beta ?? false;
            }

            return entry;
        }

        static public List<JournalEntry> Get(string eventtype)            // any commander, find me an event of this type..
        {
            Dictionary<long, TravelLogUnit> tlus = TravelLogUnit.GetAll().ToDictionary(t => t.id);
            var entries = UserDatabase.Instance.RetrievePaged("JournalEntries", "Id", rdr => CreateJournalEntry(rdr), e => e.Id, where: "EventType = @EventType", whereparams: new Dictionary<string, object> { ["EventType"] = eventtype }).ToList();

            foreach (var je in entries)
            {
                je.beta = tlus.ContainsKey(je.TLUId) ? tlus[je.TLUId].Beta : false;
            }

            return entries;
        }

        static public List<JournalEntry> GetAll(int commander = -999, DateTime? after = null, DateTime? before = null,
                            JournalTypeEnum[] ids = null, DateTime? allidsafter = null)
        {
            Dictionary<long, TravelLogUnit> tlus = TravelLogUnit.GetAll().ToDictionary(t => t.id);

            string where = "";
            Dictionary<string, object> whereparams = new Dictionary<string, object>();

            if (commander != -999)
            {
                where.AppendPrePad("Commander = @Commander", " AND ");
                whereparams["Commander"] = commander;
            }

            if (after != null)
            {
                where.AppendPrePad("EventTime >= @After", " AND ");
                whereparams["After"] = after.Value;
            }

            if (before != null)
            {
                where.AppendPrePad("EventTime <= @Before", " AND ");
                whereparams["Before"] = before.Value;
            }

            if (ids != null)
            {
                int[] array = Array.ConvertAll(ids, x => (int)x);
                if (allidsafter != null)
                {
                    where.AppendPrePad("(EventTypeId in (" + string.Join(",", array) + ") Or EventTime>=@idafter)", " and ");
                    whereparams.Add("idafter", allidsafter.Value);
                }
                else
                {
                    where.AppendPrePad("EventTypeId in (" + string.Join(",", array) + ")", " and ");
                }
            }

            var comparer = new Comparison<JournalEntry>((l, r) => l.EventTimeUTC.CompareTo(r.EventTimeUTC));
            var list = UserDatabase.Instance.RetrievePaged("JournalEntries", "Id", rdr => CreateJournalEntry(rdr), e => e.Id, where: where, whereparams: whereparams, orderby: comparer).ToArray();
            Array.Sort(list, comparer);

            foreach (var je in list)
            {
                je.beta = tlus.ContainsKey(je.TLUId) ? tlus[je.TLUId].Beta : false;
            }

            return new List<JournalEntry>(list);
        }


        public static List<JournalEntry> GetByEventType(JournalTypeEnum eventtype, int commanderid, DateTime start, DateTime stop)
        {
            Dictionary<long, TravelLogUnit> tlus = TravelLogUnit.GetAll().ToDictionary(t => t.id);

            string where = "EventTypeID = @EventTypeID AND CommanderID = @CommanderID AND EventType >= @After AND EventType <= @Before";
            Dictionary<string, object> whereparams = new Dictionary<string, object>
            {
                ["EventTypeID"] = (int)eventtype,
                ["CommanderID"] = (int)commanderid,
                ["After"] = start,
                ["Before"] = stop
            };

            var comparer = new Comparison<JournalEntry>((l, r) => l.EventTimeUTC.CompareTo(r.EventTimeUTC));
            var list = UserDatabase.Instance.RetrievePaged("JournalEntries", "Id", rdr => CreateJournalEntry(rdr), e => e.Id, where: where, whereparams: whereparams, orderby: comparer).ToArray();
            Array.Sort(list, comparer);

            foreach (var je in list)
            {
                je.beta = tlus.ContainsKey(je.TLUId) ? tlus[je.TLUId].Beta : false;
            }

            return new List<JournalEntry>(list);
        }
               
        public static List<JournalEntry> GetAllByTLU(long tluid)
        {
            TravelLogUnit tlu = TravelLogUnit.Get(tluid);

            string where = "TravelLogId = @TravelLogId";
            Dictionary<string, object> whereparams = new Dictionary<string, object>
            {
                ["TravelLogId"] = tluid
            };

            var list = UserDatabase.Instance.Retrieve("JournalEntries", rdr => CreateJournalEntry(rdr), where: where, whereparams: whereparams, orderby: "EventTime ASC");

            foreach (var je in list)
            {
                je.beta = tlu?.Beta ?? false;
            }

            return list;
        }

        public static JournalEntry GetLast(int cmdrid, DateTime before, Func<JournalEntry, bool> filter)
        {
            string where = "CommanderId = @CommanderId AND EventTime < @Before";
            Dictionary<string, object> whereparams = new Dictionary<string, object>
            {
                ["CommanderId"] = cmdrid,
                ["Before"] = before
            };

            var list = UserDatabase.Instance.Retrieve("JournalEntries", rdr => CreateJournalEntry(rdr), where: where, whereparams: whereparams, orderby: "EventTime ASC", limit: 1, filter: filter);

            if (list.Count >= 1)
            {
                var entry = list[0];
                var tlu = TravelLogUnit.Get(entry.TLUId);
                entry.beta = tlu?.Beta ?? false;
                return entry;
            }

            return null;
        }

        public static JournalEntry GetLast(DateTime before, Func<JournalEntry, bool> filter)
        {
            string where = "EventTime < @Before";
            Dictionary<string, object> whereparams = new Dictionary<string, object>
            {
                ["Before"] = before
            };

            var list = UserDatabase.Instance.Retrieve("JournalEntries", rdr => CreateJournalEntry(rdr), where: where, whereparams: whereparams, orderby: "EventTime ASC", limit: 1, filter: filter);

            if (list.Count >= 1)
            {
                var entry = list[0];
                var tlu = TravelLogUnit.Get(entry.TLUId);
                entry.beta = tlu?.Beta ?? false;
                return entry;
            }

            return null;
        }

        public static T GetLast<T>(int cmdrid, DateTime before, Func<T, bool> filter = null)
            where T : JournalEntry
        {
            return (T)GetLast(cmdrid, before, e => e is T && (filter == null || filter((T)e)));
        }

        public static T GetLast<T>(DateTime before, Func<T, bool> filter = null)
            where T : JournalEntry
        {
            return (T)GetLast(before, e => e is T && (filter == null || filter((T)e)));
        }

        public static List<JournalEntry> FindEntry(JournalEntry ent, JObject entjo = null)      // entjo is not changed.
        {
            List<JournalEntry> entries = new List<JournalEntry>();
            if (entjo == null)
            {
                entjo = GetJson(ent.Id);
            }

            entjo = RemoveEDDGeneratedKeys(entjo);

            string where = "CommanderId = @CommanderId AND EventTime = @EventTime AND TravelLogId = @TravelLogId AND EventTypeId = @EventTypeId";
            Dictionary<string, object> whereparams = new Dictionary<string, object>
            {
                ["CommanderId"] = ent.CommanderId,
                ["EventTime"] = ent.EventTimeUTC,
                ["TravelLogId"] = ent.TLUId,
                ["EventTypeId"] = (int)ent.EventTypeID
            };

            return UserDatabase.Instance.Retrieve("JournalEntries", rdr => CreateJournalEntry(rdr), where: where, whereparams: whereparams, orderby: "Id ASC", filter: jent => AreSameEntry(ent, jent, entjo));
        }

        public static int RemoveDuplicateFSDEntries(int currentcmdrid)
        {
            // list of systems in journal, sorted by time
            List<JournalLocOrJump> vsSystemsEnts = JournalEntry.GetAll(currentcmdrid).OfType<JournalLocOrJump>().OrderBy(j => j.EventTimeUTC).ToList();

            int count = 0;
            for (int ji = 1; ji < vsSystemsEnts.Count; ji++)
            {
                JournalEvents.JournalFSDJump prev = vsSystemsEnts[ji - 1] as JournalEvents.JournalFSDJump;
                JournalEvents.JournalFSDJump current = vsSystemsEnts[ji] as JournalEvents.JournalFSDJump;

                if (prev != null && current != null)
                {
                    bool previssame = (prev.StarSystem.Equals(current.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!prev.HasCoordinate || !current.HasCoordinate || (prev.StarPos - current.StarPos).LengthSquared < 0.01));

                    if (previssame)
                    {
                        Delete(prev.Id);
                        count++;
                        System.Diagnostics.Debug.WriteLine("Dup {0} {1} {2} {3}", prev.Id, current.Id, prev.StarSystem, current.StarSystem);
                    }
                }
            }

            return count;
        }

        public static void ClearEDSMID(int currentcmdrid = -2)      // -2 is all
        {
            string where = currentcmdrid == -2 ? null : "CommanderId = @CommanderId";
            Dictionary<string, object> whereparams = currentcmdrid == -2 ? null : new Dictionary<string, object>
            {
                ["CommanderId"] = currentcmdrid
            };

            UserDatabase.Instance.Update("JournalEntries", new Dictionary<string, object> { ["EdsmId"] = 0 }, where: where, whereparams: whereparams);
        }
    }
}

