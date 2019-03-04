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
        static public List<HistoryEntry> CheckFilterTrue(List<HistoryEntry> he, BaseUtils.ConditionLists cond, BaseUtils.Variables othervars)    // conditions match for item to stay
        {
            if (cond.Count == 0)       // no filters, all in
                return he;
            else
            {
                string er;
                List<HistoryEntry> ret = (from s in he where cond.CheckFilterTrue(s.journalEntry, new BaseUtils.Variables[] { othervars, new BaseUtils.Variables("Note", s.snc?.Note ?? "") }, out er, null) select s).ToList();
                return ret;
            }
        }

        static public bool FilterHistory(HistoryEntry he, BaseUtils.ConditionLists cond, BaseUtils.Variables othervars)                // true if it should be included
        {
            string er;
            return cond.CheckFilterFalse(he.journalEntry, he.journalEntry.EventTypeStr,
                new BaseUtils.Variables[] { othervars , new BaseUtils.Variables("Note", he.snc?.Note ?? "") }, out er, null);     // true it should be included
        }

        static public List<HistoryEntry> FilterHistory(List<HistoryEntry> he, BaseUtils.ConditionLists cond, BaseUtils.Variables othervars, out int count)    // filter in all entries
        {
            count = 0;
            if (cond.Count == 0)       // no filters, all in
                return he;
            else
            {
                string er;
                List<HistoryEntry> ret = (from s in he where cond.CheckFilterFalse(s.journalEntry, s.journalEntry.EventTypeStr,
                            new BaseUtils.Variables[] { othervars, new BaseUtils.Variables("Note", s.snc?.Note ?? "") }, 
                            out er, null) select s).ToList();

                count = he.Count - ret.Count;
                return ret;
            }
        }

        static public BaseUtils.ConditionLists ShowDialog(System.Windows.Forms.Form parent, BaseUtils.ConditionLists fieldfilter, EDDiscoveryForm discoveryform, string title )
        {
            ExtendedConditionsForms.ConditionFilterForm frm = new ExtendedConditionsForms.ConditionFilterForm();

            frm.VariableNamesEvents += (s) => { return BaseUtils.TypeHelpers.GetPropertyFieldNames(JournalEntry.TypeOfJournalEntry(s)); };
            frm.VariableNames = (from x in discoveryform.Globals.NameList select new BaseUtils.TypeHelpers.PropertyNameInfo(x, "Global Variable String or Number" + Environment.NewLine + "Not part of the event, set up by either EDD or one of the action packs")).ToList();
            frm.VariableNames.Add(new BaseUtils.TypeHelpers.PropertyNameInfo("Note", "String"));

            frm.InitFilter(title,
                            parent.Icon,
                            JournalEntry.GetNameOfEvents(),
                            fieldfilter);

            if (frm.ShowDialog(parent) == System.Windows.Forms.DialogResult.OK)
            {
                return frm.Result;
            }
            else
                return null;
        }

    }
}