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
using System.Reflection;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // Extends the filter form to know how to save back to DB.  and to add some standard option lists in

    public class FilterSelector : ExtendedControls.CheckedIconListBoxSelectionForm
    {
        private string dbstring;

        public new Action<string, bool, Object> Closing;                       // Action on close, string is the settings, bool is true if same as before, object is sender

        public FilterSelector(string db) : base()
        {
            dbstring = db;
            base.Closing += (x, t) =>                                          // use the base class closing, and work out if we changed anything
            {
                string org = SQLiteDBClass.GetSettingString(dbstring,"All");
                SQLiteDBClass.PutSettingString(dbstring, x);
                this.Closing?.Invoke(x, x.Equals(org), t);
            };
        }

        public void AddJournalExtraOptions()
        {
            AddGroupOption("ApproachBody;Docked;FSDJump;Location;Undocked;", "Travel".Tx(this), JournalEntry.JournalTypeIcons[JournalTypeEnum.FSDJump]);

            AddGroupOption("Scan;Scan Auto;Scan Basic;Scan Nav;NavBeaconScan;SAAScanComplete;FSSAllBodiesFound;FSSSignalDiscovered;FSSDiscoveryScan;DiscoveryScan", "Scan".Tx(this), JournalEntry.JournalTypeIcons[JournalTypeEnum.Scan]);

            var mile = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateMissions" });
            string miltype = string.Join(";", mile.Select(x => x.Item1)) + ";";
            AddGroupOption(miltype, "Missions".Tx(this), JournalEntry.JournalTypeIcons[JournalTypeEnum.Missions]);

            var mle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateMaterials" });
            mle.Add(EliteDangerousCore.JournalEntry.GetNameImageOfEvent(JournalTypeEnum.MaterialDiscovered));
            string mattype = string.Join(";", mle.Select(x => x.Item1)) + ";";
            AddGroupOption(mattype, "Materials".Tx(this), JournalEntry.JournalTypeIcons[JournalTypeEnum.Materials]);

            var cle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateCommodities" });
            string comtype = string.Join(";", cle.Select(x => x.Item1)) + ";";
            AddGroupOption(comtype, "Commodities".Tx(this), JournalEntry.JournalTypeIcons[JournalTypeEnum.Market]);

            var lle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "Ledger", "LedgerNC" });
            string legtype = string.Join(";", lle.Select(x => x.Item1)) + ";";
            AddGroupOption(legtype, "Ledger".Tx(this), EDDiscovery.Icons.IconSet.GetIcon("Panels.Ledger"));

            var sle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "ShipInformation" });
            string shiptype = string.Join(";", sle.Select(x => x.Item1)) + ";";
            AddGroupOption(shiptype, "Ship".Tx(this), JournalEntry.JournalTypeIcons[JournalTypeEnum.Shipyard]);
        }

        public void AddJournalEntries(string[] methods = null)
        {
            var items = JournalEntry.GetNameImageOfEvents(methods);

            AddStandardOption(items);

            var list = JournalEntry.GetEnumOfEvents(new string[] { "FilterItems" });
            foreach( var e in list)
            {
                Type t = JournalEntry.TypeOfJournalEntry(e);
                MethodInfo info = t.GetMethod("FilterItems", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                List<Tuple<string, string, Image>> retlist = info.Invoke(null, new object[] { }) as List<Tuple<string, string, Image>>;
                AddStandardOption(retlist);
            }

            SortStandardOptions();  // sorted by text
        }

        // do not use base Filter options - use these.

        public void Filter(Control ctr, Form parent, int width = 300)
        {
            Show(SQLiteDBClass.GetSettingString(dbstring, "All"), ctr, parent, width);
        }

        public void Filter(Point p, Size s, Form parent)
        {
            Show(SQLiteDBClass.GetSettingString(dbstring, "All"), p, s, parent);
        }

    }
}