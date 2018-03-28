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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.UserControls
{
    static public class FilterHelpers
    {
        static public List<HistoryEntry> CheckFilterTrue(List<HistoryEntry> he, Conditions.ConditionLists cond, Conditions.ConditionVariables othervars)    // conditions match for item to stay
        {
            if (cond.Count == 0)       // no filters, all in
                return he;
            else
            {
                string er;
                List<HistoryEntry> ret = (from s in he where cond.CheckFilterTrue(s.journalEntry, new Conditions.ConditionVariables[] { othervars, new Conditions.ConditionVariables("Note", s.snc?.Note ?? "") }, out er, null) select s).ToList();
                return ret;
            }
        }

        static public bool FilterHistory(HistoryEntry he, Conditions.ConditionLists cond, Conditions.ConditionVariables othervars)                // true if it should be included
        {
            string er;
            return cond.CheckFilterFalse(he.journalEntry, he.journalEntry.EventTypeStr,
                new Conditions.ConditionVariables[] { othervars , new Conditions.ConditionVariables("Note", he.snc?.Note ?? "") }, out er, null);     // true it should be included
        }

        static public List<HistoryEntry> FilterHistory(List<HistoryEntry> he, Conditions.ConditionLists cond, Conditions.ConditionVariables othervars, out int count)    // filter in all entries
        {
            count = 0;
            if (cond.Count == 0)       // no filters, all in
                return he;
            else
            {
                string er;
                List<HistoryEntry> ret = (from s in he where cond.CheckFilterFalse(s.journalEntry, s.journalEntry.EventTypeStr,
                            new Conditions.ConditionVariables[] { othervars, new Conditions.ConditionVariables("Note", s.snc?.Note ?? "") }, 
                            out er, null) select s).ToList();

                count = he.Count - ret.Count;
                return ret;
            }
        }

        static public List<HistoryEntry> MarkHistory(List<HistoryEntry> he, Conditions.ConditionLists cond, Conditions.ConditionVariables othervars, out int count)       // Used for debugging it..
        {
            count = 0;

            if (cond.Count == 0)       // no filters, all in
                return he;
            else
            {
                List<HistoryEntry> ret = new List<HistoryEntry>();

                foreach (HistoryEntry s in he)
                {
                    List<Conditions.Condition> list = new List<Conditions.Condition>();    // don't want it

                    int mrk = s.EventDescription.IndexOf(":::");
                    if (mrk >= 0)
                        s.EventDescription = s.EventDescription.Substring(mrk + 3);

                    string er;

                    if (!cond.CheckFilterFalse(s.journalEntry, s.journalEntry.EventTypeStr, new Conditions.ConditionVariables[] { othervars, new Conditions.ConditionVariables("Note", s.snc?.Note ?? "") }, out er, list))
                    {
                        //System.Diagnostics.Debug.WriteLine("Filter out " + s.Journalid + " " + s.EntryType + " " + s.EventDescription);
                        s.EventDescription = "!" + list[0].eventname + ":::" + s.EventDescription;
                        count++;
                    }

                    ret.Add(s);
                }

                return ret;
            }
        }
    }
}