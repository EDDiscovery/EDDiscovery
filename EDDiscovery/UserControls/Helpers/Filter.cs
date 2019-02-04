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
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // Extends the filter form to know how to save back to DB.  and to add some standard option lists in

    public class FilterSelector : ExtendedControls.CheckedIconListBoxFilterForm
    {
        private string dbstring;

        public FilterSelector(string db) : base()
        {
            dbstring = db;
            SaveBack += (x,t)=> SQLiteDBClass.PutSettingString(dbstring, x);
        }

        public void AddJournalExtraOptions()
        {
            // must be in alpha order..
            AddGroupOption("Travel".Tx(), "Docked;FSD Jump;Undocked;", JournalEntry.JournalTypeIcons[JournalTypeEnum.FSDJump]);
            AddGroupOption("Missions".Tx(), "Mission Abandoned;Mission Accepted;Mission Completed;Mission Failed;Mission Redirected;", JournalEntry.JournalTypeIcons[JournalTypeEnum.Missions]);
        }

        public void AddJournalEntries(string[] methods = null)
        {
            var items = JournalEntry.GetListOfEventsWithOptMethodSortedImage(true, methods);
            foreach (var x in items)
                AddStandardOption(x.Item1, x.Item2);
        }

        // do not use base Filter options - use these.

        public void Filter(Control ctr, Form parent)
        {
            Filter(SQLiteDBClass.GetSettingString(dbstring, "All"), ctr, parent);
        }

        public void Filter(Point p, Size s, Form parent)
        {
            Filter(SQLiteDBClass.GetSettingString(dbstring, "All"), p, s, parent);
        }

    }
}