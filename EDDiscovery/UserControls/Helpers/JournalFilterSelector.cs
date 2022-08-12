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
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    // Extends the filter form to know how to save back to DB.  and to add some standard option lists in

    public class JournalFilterSelector : ExtendedControls.CheckedIconListBoxFormGroup
    {
        public JournalFilterSelector() : base()
        {
            CloseOnDeactivate = false;          // this one, we hide it on deactivate, to make it pop up quicker next time
            HideOnDeactivate = true;
        }

        public void AddJournalExtraOptions()
        {
            AddGroupOption("ApproachBody;Docked;FSDJump;CarrierJump;Location;Undocked;NavRoute", "Travel".T(EDTx.FilterSelector_Travel), JournalEntry.JournalTypeIcons[JournalTypeEnum.FSDJump]);

            AddGroupOption("Scan;Scan Auto;Scan Basic;Scan Nav;NavBeaconScan;SAAScanComplete;FSSAllBodiesFound;FSSSignalDiscovered;FSSDiscoveryScan;DiscoveryScan;SAASignalsFound;FSSBodySignals", "Scan".T(EDTx.FilterSelector_Scan), JournalEntry.JournalTypeIcons[JournalTypeEnum.Scan]);

            var mile = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateMissions" });
            string miltype = string.Join(";", mile.Select(x => x.Item1)) + ";";
            AddGroupOption(miltype, "Missions".T(EDTx.FilterSelector_Missions), JournalEntry.JournalTypeIcons[JournalTypeEnum.Missions]);

            var mle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateMaterials" });
            mle.Add(EliteDangerousCore.JournalEntry.GetNameImageOfEvent(JournalTypeEnum.MaterialDiscovered));
            string mattype = string.Join(";", mle.Select(x => x.Item1)) + ";";
            AddGroupOption(mattype, "Materials".T(EDTx.FilterSelector_Materials), JournalEntry.JournalTypeIcons[JournalTypeEnum.Materials]);

            var cle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateCommodities" });
            string comtype = string.Join(";", cle.Select(x => x.Item1)) + ";";
            AddGroupOption(comtype, "Commodities".T(EDTx.FilterSelector_Commodities), JournalEntry.JournalTypeIcons[JournalTypeEnum.Market]);

            var mrle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateMicroResource" });
            string mrtype = string.Join(";", mrle.Select(x => x.Item1)) + ";";
            AddGroupOption(mrtype, "Micro Resources".T(EDTx.FilterSelector_MicroResources), JournalEntry.JournalTypeIcons[JournalTypeEnum.BuyMicroResources]);

            var lle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "Ledger", "LedgerNC" });
            string legtype = string.Join(";", lle.Select(x => x.Item1)) + ";";
            AddGroupOption(legtype, "Ledger".T(EDTx.FilterSelector_Ledger), BaseUtils.Icons.IconSet.GetIcon("Controls.Ledger"));

            var sle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "ShipInformation" });
            string shiptype = string.Join(";", sle.Select(x => x.Item1)) + ";";
            AddGroupOption(shiptype, "Ship".T(EDTx.FilterSelector_Ship), JournalEntry.JournalTypeIcons[JournalTypeEnum.Shipyard]);

            var suitle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "SuitInformation", "LoadoutInformation", "WeaponInformation" });
            string suittype = string.Join(";", suitle.Select(x => x.Item1)) + ";";
            AddGroupOption(suittype, "Suits".T(EDTx.FilterSelector_Suits), JournalEntry.JournalTypeIcons[JournalTypeEnum.BuySuit]);

            AddGroupOption("MiningRefined;AsteroidCracked;ProspectedAsteroid;LaunchDrone","Mining".T(EDTx.FilterSelector_Mining), JournalEntry.JournalTypeIcons[JournalTypeEnum.MiningRefined]);
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

        // use this to open the filter.

        public void Open(string settings, Control ctr, Form parent)
        {
            if (this.Visible == true)
            {
                Hide();
            }
            else if (!this.DeactivatedWithin(250))     // when we hide due to clicking on the button, we still get the click back thru. So debounce it
            {
                CloseBoundaryRegion = new Size(32, ctr.Height);
                Show(settings, ctr, parent);     // use the quick helper. 
            }
        }

    }
}

