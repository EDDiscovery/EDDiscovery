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
            return UserDatabase.Instance.ExecuteWithDatabase<bool>(cn => { return Add(jo, cn.Connection); });
        }

        internal bool Add(JObject jo, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Insert into JournalEntries (EventTime, TravelLogID, CommanderId, EventTypeId , EventType, EventData, EdsmId, Synced) values (@EventTime, @TravelLogID, @CommanderID, @EventTypeId , @EventStrName, @EventData, @EdsmId, @Synced)", tn))
            {
                cmd.AddParameterWithValue("@EventTime", EventTimeUTC);           // MUST use UTC connection
                cmd.AddParameterWithValue("@TravelLogID", TLUId);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@EventTypeId", EventTypeID);
                cmd.AddParameterWithValue("@EventStrName", EventTypeStr);
                cmd.AddParameterWithValue("@EventData", jo.ToString());
                cmd.AddParameterWithValue("@EdsmId", EdsmID);
                cmd.AddParameterWithValue("@Synced", Synced);

                cmd.ExecuteNonQuery();

                using (DbCommand cmd2 = cn.CreateCommand("Select Max(id) as id from JournalEntries"))
                {
                    Id = (long)cmd2.ExecuteScalar();
                }
                return true;
            }
        }

        public bool Update()
        {
            return UserDatabase.Instance.ExecuteWithDatabase<bool>(cn => { return Update(cn.Connection); });
        }

        private bool Update(SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set EventTime=@EventTime, TravelLogID=@TravelLogID, CommanderID=@CommanderID, EventTypeId=@EventTypeId, EventType=@EventStrName, EdsmId=@EdsmId, Synced=@Synced where ID=@id", tn))
            {
                cmd.AddParameterWithValue("@ID", Id);
                cmd.AddParameterWithValue("@EventTime", EventTimeUTC);  // MUST use UTC connection
                cmd.AddParameterWithValue("@TravelLogID", TLUId);
                cmd.AddParameterWithValue("@CommanderID", CommanderId);
                cmd.AddParameterWithValue("@EventTypeId", EventTypeID);
                cmd.AddParameterWithValue("@EventStrName", EventTypeStr);
                cmd.AddParameterWithValue("@EdsmId", EdsmID);
                cmd.AddParameterWithValue("@Synced", Synced);
                cmd.ExecuteNonQuery();

                return true;
            }
        }

        internal void UpdateJsonEntry(JObject jo, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set EventData=@EventData where ID=@id", tn))
            {
                cmd.AddParameterWithValue("@ID", Id);
                cmd.AddParameterWithValue("@EventData", jo.ToString());
                cmd.ExecuteNonQuery();
            }
        }

        static public void Delete(long idvalue)
        {
            UserDatabase.Instance.ExecuteWithDatabase(cn => { Delete(idvalue,cn.Connection); });
        }

        static private void Delete(long idvalue, SQLiteConnectionUser cn)
        {
            using (DbCommand cmd = cn.CreateCommand("DELETE FROM JournalEntries WHERE id = @id"))
            {
                cmd.AddParameterWithValue("@id", idvalue);
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateMapColour(int v)
        {
            JournalFSDJump fsd = this as JournalFSDJump;        // only update if fsd
            if (fsd != null)
                fsd.SetMapColour(v);
        }

        //dist >0 to update
        internal static void UpdateEDSMIDPosJump(long journalid, ISystem system, bool jsonpos, double dist, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            bool updatejson = jsonpos || dist > 0;

            JObject jo = updatejson ? GetJson(journalid, cn, tn) : null;       // if JSON pos update, get it, else null
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

                using (DbCommand cmd2 = cn.CreateCommand("Update JournalEntries set EdsmId = @EdsmId where ID = @ID", tn))
                {
                    if (updatejson)
                    {
                        cmd2.CommandText = "Update JournalEntries set EventData = @EventData, EdsmId = @EdsmId where ID = @ID";
                        cmd2.AddParameterWithValue("@EventData", jo.ToString());
                        System.Diagnostics.Trace.WriteLine(string.Format("Update journal ID {0} with pos/edsmid {1} dist {2}", journalid, system.EDSMID, dist));
                    }
                    else
                    {
                        System.Diagnostics.Trace.WriteLine(string.Format("Update journal ID {0} with edsmid {1}", journalid, system.EDSMID));
                    }

                    cmd2.AddParameterWithValue("@ID", journalid);
                    cmd2.AddParameterWithValue("@EdsmId", system.EDSMID);

                    cmd2.ExecuteNonQuery();
                }
            }
        }

        private void UpdateSyncFlagBit(SyncFlags bit1, bool value1, SyncFlags bit2, bool value2, SQLiteConnectionUser cn , DbTransaction txn = null)
        {
            if (value1)
                Synced |= (int)bit1;
            else
                Synced &= ~(int)bit1;

            if (value2)
                Synced |= (int)bit2;
            else
                Synced &= ~(int)bit2;

            using (DbCommand cmd = cn.CreateCommand("Update JournalEntries set Synced = @sync where ID=@journalid", txn))
            {
                cmd.AddParameterWithValue("@journalid", Id);
                cmd.AddParameterWithValue("@sync", Synced);
                System.Diagnostics.Trace.WriteLine(string.Format("Update sync flag ID {0} with {1}", Id, Synced));
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateCommanderID(int cmdrid)
        {
            UserDatabase.Instance.ExecuteWithDatabase(cn =>
            {
                using (DbCommand cmd = cn.Connection.CreateCommand("Update JournalEntries set CommanderID = @cmdrid where ID=@journalid"))
                {
                    cmd.AddParameterWithValue("@journalid", Id);
                    cmd.AddParameterWithValue("@cmdrid", cmdrid);
                    System.Diagnostics.Trace.WriteLine(string.Format("Update cmdr id ID {0} with map colour", Id));
                    cmd.ExecuteNonQuery();
                    CommanderId = cmdrid;
                }
            });
        }

        static public bool ResetCommanderID(int from, int to)
        {
            UserDatabase.Instance.ExecuteWithDatabase(cn =>
            {
                using (DbCommand cmd = cn.Connection.CreateCommand("Update JournalEntries set CommanderID = @cmdridto where CommanderID=@cmdridfrom"))
                {
                    if (from == -1)
                        cmd.CommandText = "Update JournalEntries set CommanderID = @cmdridto";

                    cmd.AddParameterWithValue("@cmdridto", to);
                    cmd.AddParameterWithValue("@cmdridfrom", from);
                    System.Diagnostics.Trace.WriteLine(string.Format("Update cmdr id ID {0} with {1}", from, to));
                    cmd.ExecuteNonQuery();
                }
            });
            return true;
        }

        public JObject GetJson()
        {
            return JsonCached != null ? JsonCached : UserDatabase.Instance.ExecuteWithDatabase<JObject>(cn => { return GetJson(Id, cn.Connection); });
        }

        static internal JObject GetJson(long journalid, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("select EventData from JournalEntries where ID=@journalid", tn))
            {
                cmd.AddParameterWithValue("@journalid", journalid);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string EDataString = (string)reader["EventData"];

                        try
                        {
                            return JObject.Parse(EDataString);
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine($"Error parsing journal entry\n{EDataString}\n{ex.ToString()}");
                            return null;
                        }
                    }
                }
            }

            return null;
        }

        static public JournalEntry Get(long journalid)
        {
            return UserDatabase.Instance.ExecuteWithDatabase<JournalEntry>(cn => { return Get(journalid, cn.Connection); });
        }

        static internal JournalEntry Get(long journalid, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            using (DbCommand cmd = cn.CreateCommand("select * from JournalEntries where ID=@journalid", tn))
            {
                cmd.AddParameterWithValue("@journalid", journalid);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return CreateJournalEntry(reader);
                    }
                }
            }

            return null;
        }

        static public List<JournalEntry> Get(string eventtype)            // any commander, find me an event of this type..
        {
            return UserDatabase.Instance.ExecuteWithDatabase<List<JournalEntry>>(cn => { return Get(eventtype, cn.Connection); });
        }

        static internal List<JournalEntry> Get(string eventtype, SQLiteConnectionUser cn, DbTransaction tn = null)
        {
            Dictionary<long, TravelLogUnit> tlus = TravelLogUnit.GetAll().ToDictionary(t => t.id);

            using (DbCommand cmd = cn.CreateCommand("select * from JournalEntries where EventType=@ev", tn))
            {
                cmd.AddParameterWithValue("@ev", eventtype);

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    List<JournalEntry> entries = new List<JournalEntry>();

                    while (reader.Read())
                    {
                        JournalEntry je = CreateJournalEntry(reader);
                        je.beta = tlus.ContainsKey(je.TLUId) ? tlus[je.TLUId].Beta : false;
                        entries.Add(je);
                    }

                    return entries;
                }
            }
        }

        static public List<JournalEntry> GetAll(int commander = -999, DateTime? afterutc = null, DateTime? beforeutc = null,
                            JournalTypeEnum[] ids = null, DateTime? allidsafterutc = null)
        {
            Dictionary<long, TravelLogUnit> tlus = TravelLogUnit.GetAll().ToDictionary(t => t.id);
            DbCommand cmd = null;
            DbDataReader reader = null;
            List<JournalEntry> entries = new List<JournalEntry>();

            try
            {
                cmd = UserDatabase.Instance.ExecuteWithDatabase(cn => cn.Connection.CreateCommand("select * from JournalEntries"));
                reader = UserDatabase.Instance.ExecuteWithDatabase(cn =>
                {
                    string cnd = "";
                    if (commander != -999)
                    {
                        cnd = cnd.AppendPrePad("CommanderID = @commander", " and ");
                        cmd.AddParameterWithValue("@commander", commander);
                    }
                    if (afterutc != null)
                    {
                        cnd = cnd.AppendPrePad("EventTime >= @after", " and ");
                        cmd.AddParameterWithValue("@after", afterutc.Value);
                    }
                    if (beforeutc != null)
                    {
                        cnd = cnd.AppendPrePad("EventTime <= @before", " and ");
                        cmd.AddParameterWithValue("@before", beforeutc.Value);
                    }
                    if (ids != null)
                    {
                        int[] array = Array.ConvertAll(ids, x => (int)x);
                        if (allidsafterutc != null)
                        {
                            cmd.AddParameterWithValue("@idafter", allidsafterutc.Value);
                            cnd = cnd.AppendPrePad("(EventTypeId in (" + string.Join(",", array) + ") Or EventTime>=@idafter)", " and ");
                        }
                        else
                        {
                            cnd = cnd.AppendPrePad("EventTypeId in (" + string.Join(",", array) + ")", " and ");
                        }
                    }

                    if (cnd.HasChars())
                        cmd.CommandText += " where " + cnd;

                    cmd.CommandText += " Order By EventTime ASC";

                    return cmd.ExecuteReader();
                });

                List<JournalEntry> retlist = null;

                do
                {
                    retlist = UserDatabase.Instance.ExecuteWithDatabase(cn =>
                    {
                        List<JournalEntry> list = new List<JournalEntry>();

                        while (list.Count < 1000 && reader.Read())
                        {
                            JournalEntry sys = JournalEntry.CreateJournalEntry(reader);
                            sys.beta = tlus.ContainsKey(sys.TLUId) ? tlus[sys.TLUId].Beta : false;
                            list.Add(sys);
                        }

                        return list;
                    });

                    entries.AddRange(retlist);
                }
                while (retlist != null && retlist.Count != 0);

                return entries;
            }
            finally
            {
                if (reader != null || cmd != null)
                {
                    UserDatabase.Instance.ExecuteWithDatabase(cn =>
                    {
                        reader?.Close();
                        cmd?.Dispose();
                    });
                }
            }
        }


        public static List<JournalEntry> GetByEventType(JournalTypeEnum eventtype, int commanderid, DateTime startutc, DateTime stoputc)
        {
            Dictionary<long, TravelLogUnit> tlus = TravelLogUnit.GetAll().ToDictionary(t => t.id);
            DbCommand cmd = null;
            DbDataReader reader = null;
            List<JournalEntry> entries = new List<JournalEntry>();

            try
            {
                cmd = UserDatabase.Instance.ExecuteWithDatabase(cn => cn.Connection.CreateCommand("SELECT * FROM JournalEntries WHERE EventTypeID = @eventtype and  CommanderID=@commander and  EventTime >=@start and EventTime<=@Stop ORDER BY EventTime ASC"));
                reader = UserDatabase.Instance.ExecuteWithDatabase(cn =>
                {
                    cmd.AddParameterWithValue("@eventtype", (int)eventtype);
                    cmd.AddParameterWithValue("@commander", (int)commanderid);
                    cmd.AddParameterWithValue("@start", startutc);
                    cmd.AddParameterWithValue("@stop", stoputc);
                    return cmd.ExecuteReader();
                });

                List<JournalEntry> retlist = null;

                do
                {
                    retlist = UserDatabase.Instance.ExecuteWithDatabase(cn =>
                    {
                        List<JournalEntry> vsc = new List<JournalEntry>();

                        while (vsc.Count < 1000 && reader.Read())
                        {
                            JournalEntry je = CreateJournalEntry(reader);
                            je.beta = tlus.ContainsKey(je.TLUId) ? tlus[je.TLUId].Beta : false;
                            vsc.Add(je);
                        }

                        return vsc;
                    });

                    entries.AddRange(retlist);
                }
                while (retlist != null && retlist.Count != 0);

                return entries;
            }
            finally
            {
                if (reader != null || cmd != null)
                {
                    UserDatabase.Instance.ExecuteWithDatabase(cn =>
                    {
                        reader?.Close();
                        cmd?.Dispose();
                    });
                }
            }
        }
               
        internal static List<JournalEntry> GetAllByTLU(long tluid, SQLiteConnectionUser cn)
        {
            TravelLogUnit tlu = TravelLogUnit.Get(tluid, cn);
            List<JournalEntry> vsc = new List<JournalEntry>();

            using (DbCommand cmd = cn.CreateCommand("SELECT * FROM JournalEntries WHERE TravelLogId = @source ORDER BY EventTime ASC"))
            {
                cmd.AddParameterWithValue("@source", tluid);
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        JournalEntry je = CreateJournalEntry(reader);
                        je.beta = tlu?.Beta ?? false;
                        vsc.Add(je);
                    }
                }
            }
            return vsc;
        }

        public static JournalEntry GetLast(int cmdrid, DateTime beforeutc, Func<JournalEntry, bool> filter)
        {
            return UserDatabase.Instance.ExecuteWithDatabase<JournalEntry>(cn =>
            {
                using (DbCommand cmd = cn.Connection.CreateCommand("SELECT * FROM JournalEntries WHERE CommanderId = @cmdrid AND EventTime < @time ORDER BY EventTime DESC"))
                {
                    cmd.AddParameterWithValue("@cmdrid", cmdrid);
                    cmd.AddParameterWithValue("@time", beforeutc);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JournalEntry ent = CreateJournalEntry(reader);
                            if (filter(ent))
                            {
                                return ent;
                            }
                        }
                    }
                }
                return null;
            });
        }

        public static JournalEntry GetLast(DateTime beforeutc, Func<JournalEntry, bool> filter)
        {
            return UserDatabase.Instance.ExecuteWithDatabase<JournalEntry>(cn =>
            {
                using (DbCommand cmd = cn.Connection.CreateCommand("SELECT * FROM JournalEntries WHERE EventTime < @time ORDER BY EventTime DESC"))
                {
                    cmd.AddParameterWithValue("@time", beforeutc);
                    using (DbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            JournalEntry ent = CreateJournalEntry(reader);
                            if (filter(ent))
                            {
                                return ent;
                            }
                        }
                    }
                }
                return null;
            });
        }

        public static T GetLast<T>(int cmdrid, DateTime beforeutc, Func<T, bool> filter = null)
            where T : JournalEntry
        {
            return (T)GetLast(cmdrid, beforeutc, e => e is T && (filter == null || filter((T)e)));
        }

        public static T GetLast<T>(DateTime beforeutc, Func<T, bool> filter = null)
            where T : JournalEntry
        {
            return (T)GetLast(beforeutc, e => e is T && (filter == null || filter((T)e)));
        }

        public static List<JournalEntry> FindEntry(JournalEntry ent, UserDatabaseConnection cn , JObject entjo = null)      // entjo is not changed.
        {
            if (entjo == null)
            {
                entjo = GetJson(ent.Id,cn.Connection);
            }

            entjo = RemoveEDDGeneratedKeys(entjo);

            List<JournalEntry> entries = new List<JournalEntry>();

            using (DbCommand cmd = cn.Connection.CreateCommand("SELECT * FROM JournalEntries WHERE CommanderId = @cmdrid AND EventTime = @time AND TravelLogId = @tluid AND EventTypeId = @evttype ORDER BY Id ASC"))
            {
                cmd.AddParameterWithValue("@cmdrid", ent.CommanderId);
                cmd.AddParameterWithValue("@time", ent.EventTimeUTC);
                cmd.AddParameterWithValue("@tluid", ent.TLUId);
                cmd.AddParameterWithValue("@evttype", ent.EventTypeID);
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        JournalEntry jent = CreateJournalEntry(reader);
                        if (AreSameEntry(ent, jent, cn.Connection, entjo))
                        {
                            entries.Add(jent);
                        }
                    }
                }
            }

            return entries;
        }

        public static int RemoveDuplicateFSDEntries(int currentcmdrid)
        {
            // list of systems in journal, sorted by time
            List<JournalFSDJump> vsSystemsEnts = GetAll(currentcmdrid, ids: new[] { JournalTypeEnum.FSDJump }).OfType<JournalFSDJump>().OrderBy(j => j.EventTimeUTC).ToList();

            int count = 0;
            UserDatabase.Instance.ExecuteWithDatabase(cn =>
            {
                for (int ji = 1; ji < vsSystemsEnts.Count; ji++)
                {
                    JournalEvents.JournalFSDJump prev = vsSystemsEnts[ji - 1] as JournalEvents.JournalFSDJump;
                    JournalEvents.JournalFSDJump current = vsSystemsEnts[ji] as JournalEvents.JournalFSDJump;

                    if (prev != null && current != null)
                    {
                        bool previssame = (prev.StarSystem.Equals(current.StarSystem, StringComparison.CurrentCultureIgnoreCase) && (!prev.HasCoordinate || !current.HasCoordinate || (prev.StarPos - current.StarPos).LengthSquared < 0.01));

                        if (previssame)
                        {
                            Delete(prev.Id, cn.Connection);
                            count++;
                            System.Diagnostics.Debug.WriteLine("Dup {0} {1} {2} {3}", prev.Id, current.Id, prev.StarSystem, current.StarSystem);
                        }
                    }
                }
            });

            return count;
        }

        public static void ClearEDSMID(int currentcmdrid = -2)      // -2 is all
        {
            UserDatabase.Instance.ExecuteWithDatabase(cn =>
            {
                using (DbCommand cmd = cn.Connection.CreateCommand("UPDATE JournalEntries SET EdsmId=0"))
                {
                    if (currentcmdrid != -2)
                    {
                        cmd.CommandText = "UPDATE JournalEntries SET EdsmId=0 WHERE CommanderId==@cmd";
                        cmd.AddParameterWithValue("@cmd", currentcmdrid);
                    }

                    cmd.ExecuteNonQuery();
                }
            });
        }
    }
}

