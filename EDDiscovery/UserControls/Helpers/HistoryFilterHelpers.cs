/*
 * Copyright © 2016-2022 EDDiscovery development team
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

using BaseUtils;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EDDiscovery.UserControls
{
    static public class HistoryFilterHelpers
    {
        static public List<HistoryEntry> CheckFilterTrue(List<HistoryEntry> he, BaseUtils.ConditionLists cond, BaseUtils.Variables othervars)    // conditions match for item to stay
        {
            if (cond.Count == 0)       // no filters, all in
                return he;
            else
            {
                List<HistoryEntry> ret = he.Where(s => BaseUtils.ConditionLists.CheckConditionWithObjectData(cond.List,
                                                    s.journalEntry,
                                                    new BaseUtils.Variables[] { othervars, new BaseUtils.Variables("Note", s.GetNoteText) },
                                                    out string eliststring, out BaseUtils.ConditionLists.ErrorClass errclass) == true).
                                                    Select(x => x).ToList();
                return ret;
            }
        }

        static public ConditionLists ShowDialog(System.Windows.Forms.Form parent, BaseUtils.ConditionLists fieldfilter, EDDiscoveryForm discoveryform, string title )
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

    public class HistoryEventFilter
    {
        private Dictionary<string, List<Condition>> filterbyeventname;
        private Variables filtervars;
        private HashSet<string> eventfilter;

        public HistoryEventFilter(string eventfilterset,ConditionLists filters, Variables def)      // operates the event and field filter
        {
            eventfilter = eventfilterset.Split(';').ToHashSet();
            filterbyeventname = filters.GetConditionListDictionaryByEventName();
            filtervars = new Variables(def);
        }

        public bool IsIncluded(HistoryEntry he)
        {
            if (!eventfilter.Contains(he.journalEntry.EventFilterName) && !eventfilter.Contains("All"))        // All or event name must be in list
                return false;

            List<Condition> filterrules;

            if (filterbyeventname.TryGetValue(he.journalEntry.EventFilterName, out filterrules) || filterbyeventname.TryGetValue("All", out filterrules))     // if we have rules for this event..
            {
                filtervars["Note"] = he.GetNoteText;  // add in SNC Note

                var fres = ConditionLists.CheckConditionWithObjectData(filterrules, he.journalEntry, new Variables[] { filtervars }, out string errlist, out ConditionLists.ErrorClass errclass);

                if (fres == true)
                    return false;
            }

            return true;
        }
    }
}