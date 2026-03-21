/*
 * Copyright 2016-2024 EDDiscovery development team
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

    public class JournalFilterSelector : ExtendedControls.CheckedIconNewListBoxForm
    {
        public JournalFilterSelector() : base()
        {
            CloseOnDeactivate = false;          // this one, we hide it on deactivate, to make it pop up quicker next time
            HideOnDeactivate = true;
            UC.MultiColumnSlide = true;
            UC.ShowClose = true;
        }

        public void AddJournalExtraOptions()
        {
            UC.AddGroupItem("ApproachBody;Docked;FSDJump;CarrierJump;Location;Undocked;NavRoute;StartJump", "Travel".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.FSDJump]);

            UC.AddGroupItem("Scan;Scan Auto;Scan Basic;Scan Nav;NavBeaconScan;SAAScanComplete;FSSAllBodiesFound;FSSSignalDiscovered;FSSDiscoveryScan;DiscoveryScan;SAASignalsFound;FSSBodySignals", "Scan".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.Scan]);

            var mile = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateMissions" });
            string miltype = string.Join(";", mile.Select(x => x.Item1)) + ";";
            UC.AddGroupItem(miltype, "Missions".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.Missions]);

            var mle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateMaterials" });
            mle.Add(EliteDangerousCore.JournalEntry.GetNameImageOfEvent(JournalTypeEnum.MaterialDiscovered));
            string mattype = string.Join(";", mle.Select(x => x.Item1)) + ";";
            UC.AddGroupItem(mattype, "Materials".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.Materials]);

            var cle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateCommodities" });
            string comtype = string.Join(";", cle.Select(x => x.Item1)) + ";";
            UC.AddGroupItem(comtype, "Commodities".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.Market]);

            var mrle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateMicroResource" });
            string mrtype = string.Join(";", mrle.Select(x => x.Item1)) + ";";
            UC.AddGroupItem(mrtype, "Micro Resources".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.BuyMicroResources]);

            var lle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "Ledger", "LedgerNC" });
            string legtype = string.Join(";", lle.Select(x => x.Item1)) + ";";
            UC.AddGroupItem(legtype, "Ledger".Tx(), BaseUtils.Icons.IconSet.GetImage("Controls.Ledger"));

            var sle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "ShipInformation" });
            string shiptype = string.Join(";", sle.Select(x => x.Item1)) + ";";
            UC.AddGroupItem(shiptype, "Ship".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.Shipyard]);

            var suitle = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "SuitInformation", "LoadoutInformation", "WeaponInformation" });
            string suittype = string.Join(";", suitle.Select(x => x.Item1)) + ";";
            UC.AddGroupItem(suittype, "Suits".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.BuySuit]);

            var carriere = EliteDangerousCore.JournalEntry.GetNameImageOfEvents(new string[] { "UpdateCarrierStats" });
            string carriertype = string.Join(";", carriere.Select(x => x.Item1)) + ";";

            UC.AddGroupItem(carriertype, "Carrier".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.CarrierStats]);

            UC.AddGroupItem("MiningRefined;AsteroidCracked;ProspectedAsteroid;LaunchDrone","Mining".Tx(), JournalEntry.JournalTypeIcons[JournalTypeEnum.MiningRefined]);
        }


        public void AddJournalEntries(string[] methods = null)
        {
            var items = JournalEntry.GetNameImageOfEvents(methods);

            UC.Add(items);

            // get any events which define FilterItems as additional filters

            var itemsfilter = JournalEntry.GetNameImageOfEventsFilterItems();

            UC.Add(itemsfilter);

            UC.Sort();  // sorted by text
        }

        public void AddUserGroups(string groupswithids, Control refctrl)
        {
            UC.AddStringListDefinitions(groupswithids, 1, true, global::EDDiscovery.Icons.Controls.RescanJournals);      // create with a usertag of int

            //System.Diagnostics.Debug.WriteLine($"Group setting {GetUserGroupDefinition(1)}");

            UC.AddButton("creategroup", "Create new group".Tx(), global::EDDiscovery.Icons.Controls.AddJournals, attop:true);

            UC.ButtonPressed += (index,stag, text, usertag, e) => 
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (usertag is int)
                    {
                        Hide();

                        if (ExtendedControls.MessageBoxTheme.Show(refctrl, $"Confirm removal of".Tx()+ " " + text, "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            UC.Remove(index);
                            ForceRedrawOnNextShow();
                        }
                    }
                }
                else if ( e.Button == MouseButtons.Left)
                {
                    if ( stag == "creategroup" )
                    {
                        Hide();

                        string promptValue = ExtendedControls.PromptSingleLine.ShowDialog(refctrl, "", "", "Enter name of new group".Tx(), Properties.Resources.edlogo_3mo_icon, requireinput: true);
                        if (promptValue != null)
                        {
                            string cursettings = GetChecked();
                            UC.AddGroupItem( cursettings, promptValue, global::EDDiscovery.Icons.Controls.RescanJournals, usertag:1);   // new entry with usertag:1
                            ForceRedrawOnNextShow();
                        }
                    }
                }
            };
        }

        public string GetUserGroups()
        {
            return UC.GetUserTagDefinitions(1);     // all with int
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

