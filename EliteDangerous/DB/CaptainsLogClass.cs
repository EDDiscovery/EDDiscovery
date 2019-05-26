/*
 * Copyright © 2019 EDDiscovery development team
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

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace EliteDangerousCore.DB
{
    public class CaptainsLogClass
    {
        public long ID { get; private set; }
        public int Commander { get; private set; }
        public DateTime TimeUTC { get; private set; }
        public string SystemName { get; private set; }
        public string BodyName { get; private set; }
        public string Note { get; private set; }
        public string Tags { get; private set; }     // may be null
        public string Parameters { get; private set; }     // may be null

        public DateTime TimeLocal { get { return TimeUTC.ToLocalTime(); } }
        public DateTime Time(bool utc) { return utc ? TimeUTC : TimeLocal; }

        public CaptainsLogClass()
        {
        }

        public CaptainsLogClass(DbDataReader dr)
        {
            ID = (long)dr["Id"];
            Commander = (int)(long)dr["Commander"];
            TimeUTC = (DateTime)dr["Time"];
            SystemName = (string)dr["SystemName"];
            BodyName = (string)dr["BodyName"];
            Note = (string)dr["Note"];

            if (System.DBNull.Value != dr["Tags"])
                Tags = (string)dr["Tags"];
            if (System.DBNull.Value != dr["Parameters"])
                Parameters = (string)dr["Parameters"];
        }

        public void Set( int cmdr, string system , string body , DateTime timeutc, string note, string tags = null, string paras = null)
        {
            Commander = cmdr;
            SystemName = system;
            BodyName = body;
            TimeUTC = timeutc;
            Note = note;
            Tags = tags;
            Parameters = paras;
        }

        internal bool Add()
        {
            ID = UserDatabase.Instance.Add<long>("CaptainsLog", "id", new Dictionary<string, object>
            {
                ["Commander"] = Commander,
                ["TimeUTC"] = TimeUTC,
                ["SystemName"] = SystemName,
                ["BodyName"] = BodyName,
                ["Note"] = Note,
                ["Tags"] = Tags,
                ["Parameters"] = Parameters
            });

            return true;
        }

        internal bool Update()
        {
            UserDatabase.Instance.Update("CaptainsLog", "id", ID, new Dictionary<string, object>
            {
                ["Commander"] = Commander,
                ["TimeUTC"] = TimeUTC,
                ["SystemName"] = SystemName,
                ["BodyName"] = BodyName,
                ["Note"] = Note,
                ["Tags"] = Tags,
                ["Parameters"] = Parameters
            });

            return true;
        }

        public bool Delete()
        {
            UserDatabase.Instance.Delete("CaptainsLog", "id", ID);
            return true;
        }

        // Update notes
        public void UpdateNotes(string notes)
        {
            Note = notes;
            Update();
        }
       
    }

    // EVERYTHING goes thru list class for adding/deleting log entries

    public class GlobalCaptainsLogList
    {
        public static bool Instanced { get { return gbl != null; } }
        public static GlobalCaptainsLogList Instance { get { return gbl; } }

        public List<CaptainsLogClass> LogEntries { get { return globallog; } }

        public List<CaptainsLogClass> LogEntriesCmdr(int cmdrid) { return globallog.Where(x => x.Commander == cmdrid).ToList(); }
        public List<CaptainsLogClass> LogEntriesCmdrTimeOrder(int cmdrid) { return globallog.Where(x => x.Commander == cmdrid).OrderBy(x=>x.TimeUTC).ToList(); }

        public Action<CaptainsLogClass, bool> OnLogEntryChanged;        // bool = true if deleted

        private static GlobalCaptainsLogList gbl = null;

        private List<CaptainsLogClass> globallog = new List<CaptainsLogClass>();

        public static bool LoadLog()
        {
            System.Diagnostics.Debug.Assert(gbl == null);       // no double instancing!
            gbl = new GlobalCaptainsLogList();

            try
            {
                List<CaptainsLogClass> logs = UserDatabase.Instance.Retrieve("CaptainsLog", rdr => new CaptainsLogClass(rdr));

                if (logs.Count == 0)
                {
                    return false;
                }
                else
                {
                    foreach (var bc in logs)
                    {
                        gbl.globallog.Add(bc);
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.ToString());
                return false;
            }
        }

        public CaptainsLogClass[] Find(DateTime start, DateTime end, bool utc, int cmdr)
        {
            if ( utc )
                return (from x in LogEntries where x.TimeUTC >= start && x.TimeUTC <= end && x.Commander == cmdr select x).ToArray();
            else
                return (from x in LogEntries where x.TimeLocal >= start && x.TimeLocal <= end && x.Commander == cmdr select x).ToArray();
        }

        // bk = null, new note, else update. Note systemname/bodyname are not unique.  Id it the only unique property

        public CaptainsLogClass AddOrUpdate(CaptainsLogClass bk, int commander, string systemname, string bodyname, DateTime timeutc, string notes, string tags = null, string parameters = null)
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            bool addit = bk == null;

            if (addit)
            {
                bk = new CaptainsLogClass();
                globallog.Add(bk);
                System.Diagnostics.Debug.WriteLine("New log created");
            }

            bk.Set(commander, systemname, bodyname, timeutc, notes, tags, parameters);

            if (addit)
                bk.Add();
            else
            {
                System.Diagnostics.Debug.WriteLine(GlobalCaptainsLogList.Instance.LogEntries.Find((xx) => Object.ReferenceEquals(bk, xx)) != null);
                bk.Update();
            }

            //System.Diagnostics.Debug.WriteLine("Write captains log " + bk.SystemName + ":" + bk.BodyName + " Notes " + notes);

            OnLogEntryChanged?.Invoke(bk, false);

            return bk;
        }

        public void Delete(CaptainsLogClass bk)
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);
            long id = bk.ID;
            bk.Delete();
            globallog.RemoveAll(x => x.ID == id);
            OnLogEntryChanged?.Invoke(bk, true);
        }

        public void TriggerChange(CaptainsLogClass bk)
        {
            OnLogEntryChanged?.Invoke(bk, true);
        }
    }
}
