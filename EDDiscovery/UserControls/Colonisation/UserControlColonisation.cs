/*
 * Copyright 2025 - 2025 EDDiscovery development team
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

using EDDiscovery.UserControls.Colonisation;
using EliteDangerousCore;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlColonisation : UserControlCommonBase
    {
        public UserControlColonisation()
        {
            InitializeComponent();
            DBBaseName = "Colonisation";
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
        }

        public override void Init()
        {
            DiscoveryForm.OnHistoryChange += HistoryChange;
            DiscoveryForm.OnNewEntry += NewEntry;
        }
        public override void InitialDisplay()
        {
            HistoryChange();
        }

        private void HistoryChange()
        {
            var colonisationevents = HistoryList.FilterByEventEntryOrder(DiscoveryForm.History.EntryOrder(), 
                            new HashSet<JournalTypeEnum> { JournalTypeEnum.ColonisationBeaconDeployed, JournalTypeEnum.ColonisationConstructionDepot, JournalTypeEnum.ColonisationContribution, 
                                JournalTypeEnum.ColonisationSystemClaim, JournalTypeEnum.ColonisationSystemClaimRelease,
                                JournalTypeEnum.Docked , JournalTypeEnum.FSDJump, JournalTypeEnum.Location, JournalTypeEnum.CarrierJump},
                            mindatetime:EliteReleaseDates.Trailblazers);

            colonisation.Clear();

            foreach (var he in colonisationevents.EmptyIfNull())
                colonisation.Add( he, colonisationevents);

            current = null;     // no system

            // no point doing the selection if the list is empty -
            // we stay on no system but we don't clear the lastsystem selector as we may be just in the first History refresh triggered by the system

            if (colonisation.Systems.Count > 0)     
            {
                string lastsystem = GetSetting(dbSelection, "");
                var csystems = colonisation.Systems.Values.ToList();

                // given the db entry last system, try and find it, if found, set current.
                int index = lastsystem.HasChars() ? csystems.FindIndex(x => x.System.Name.Equals(lastsystem)) : -1;
                if (index >= 0)
                    current = csystems[index];
                else
                    SelectLastCreatedSystem();      // current can still be null if there are no systems
            }

            UpdateComboBox();

            extPanelGradientFillUCCP.Controls.Clear();      // start afresh
            Display();
        }

        public void SelectLastCreatedSystem()
        {
            current = colonisation.LastCreatedSystem;       // this could be null - no systems
            PutSetting(dbSelection, "");      // back to last system (empty string) as not found
        }

        private void NewEntry(HistoryEntry he)
        {
            bool redisplay = false;     // if we need a display 

            if (he.journalEntry is IStarScan || he.journalEntry is IMaterialJournalEntry || he.journalEntry is IBodyNameAndID)
            {
                if (he.System.Name == current?.System.Name)     // current may be null.  If we have updated star scan data
                    redisplay = true;
            }

            var ret = colonisation.Add(he, DiscoveryForm.History.EntryOrder()); // process entry

            if (current == null && ret.newsystem)      // if no current system, but we have a new system, select it
            {
                extPanelGradientFillUCCP.Controls.Clear();      // start afresh (not needed, but safe)
                SelectLastCreatedSystem();
                System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} Colonisation switched to `{current.System.Name}` due to ret.newsystem");
            }
            else if (extComboBoxSystemSel.SelectedIndex == 0 && ret.newsystem)     // if new system, and we are on last created system
            {
                extPanelGradientFillUCCP.Controls.Clear();  // start afresh
                current = ret.csd;
                redisplay = true;
                System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} Colonisation switched to `{current.System.Name}` due to ret.newsystem");
            }
            else if (ret.csd != null && ret.csd == current)                    // if we changed something about the current system
            {
                System.Diagnostics.Debug.WriteLine($"{he.EventTimeUTC} Colonisation update display due to change in `{current.System.Name}`");
                redisplay = true;
            }

            if (ret.newsystem)                              // if we have a change in system list, update the combo
                UpdateComboBox();

            if ( redisplay )
                Display();
        }

        public void UpdateComboBox()
        {
            extComboBoxSystemSel.Items.Clear();
            var taglist = new List<ColonisationSystemData>();

            HashSet<string> sysdone = new HashSet<string>();

            extComboBoxSystemSel.Items.Add("Last system added");
            taglist.Add(null);      // this is associated with null for current

            // first ones where a beacon is deployed 
            foreach (var kvp in colonisation.Systems.Where(x => x.Value.BeaconDeployed))
            {
                extComboBoxSystemSel.Items.Add(kvp.Value.System.Name + " \u2729");
                taglist.Add(kvp.Value);
                sysdone.Add(kvp.Value.System.Name);
            }

            // where we have a port depot
            foreach (var kvp in colonisation.Systems.Where(x => x.Value.Ports.Values.ToList().Find(y => y.State != null) != null && !sysdone.Contains(x.Value.System.Name)))
            {
                extComboBoxSystemSel.Items.Add(kvp.Value.System.Name + " \u2b50");
                taglist.Add(kvp.Value);
                sysdone.Add(kvp.Value.System.Name);
            }

            // all others
            foreach (var kvp in colonisation.Systems.Where(x => !sysdone.Contains(x.Value.System.Name)))
            {
                extComboBoxSystemSel.Items.Add(kvp.Value.System.Name);
                taglist.Add(kvp.Value);
            }

            extComboBoxSystemSel.Tag = taglist;

            ignorechange = true;

            string lastsystem = GetSetting(dbSelection, "");
            int index = taglist.IndexOf(current);       // will match 0 if current == null, setting entry to last created system

            if (lastsystem.HasChars() && index > 0)        // if not on last system and index > 0
            {
                extComboBoxSystemSel.SelectedIndex = index;     // change to it with no trigger
            }
            else
                extComboBoxSystemSel.SelectedIndex = 0;         // set to last system no trigger

            ignorechange = false;
        }
           

        private void extComboBoxSystemSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ignorechange)
            {
                if (extComboBoxSystemSel.SelectedIndex == 0)        // if last created, select
                    SelectLastCreatedSystem();
                else
                {
                    current = (extComboBoxSystemSel.Tag as List<ColonisationSystemData>)[extComboBoxSystemSel.SelectedIndex];
                    PutSetting(dbSelection, current.System.Name);
                }

                extPanelGradientFillUCCP.Controls.Clear();
                Display();
            }
        }

        public override void Closing()
        {
            DiscoveryForm.OnHistoryChange -= HistoryChange;
            DiscoveryForm.OnNewEntry -= NewEntry;
        }

        public override bool SupportTransparency => true;
        public override void SetTransparency(bool on, Color curcol)
        {
            this.BackColor = curcol;
        }

        private void Display()
        {
            if ( current != null)
            {
                if (extPanelGradientFillUCCP.Controls.Count == 0)        // no controls in user control content panel
                {
                    // FillContents has a ExtPanelVertScrollWithBar which has
                    // a ExtPanelVertScroll which has a single control ExtPanelGradientFill pcontent
                    // pcontent is sized to the items placed in it, making the vert scroll work

                    ExtPanelVertScrollWithBar pouter = new ExtPanelVertScrollWithBar();
                    pouter.Dock = DockStyle.Fill;
                    ExtPanelVertScroll pinnervscroll = new ExtPanelVertScroll();
                    pinnervscroll.Dock = DockStyle.Fill;
                    pinnervscroll.Name = "PanelVertScroll";
                    pouter.Controls.Add(pinnervscroll);

                    ExtPanelGradientFill pscrolledcontent = new ExtPanelGradientFill();     // use this due to having LayoutComplete callback
                    pscrolledcontent.Name = "Content Holder";
                    pscrolledcontent.Location = new Point(0, 0);

                    ColonisationSystemDisplay csi = new ColonisationSystemDisplay();
                    csi.Tag = current;          // we tag mark it to find it
                    csi.Dock = DockStyle.Top;
                    csi.Initialise(current, DiscoveryForm.History);


                    foreach (var kvp in current.Ports)
                    {
                        ColonisationPortDisplay cp = new ColonisationPortDisplay();
                        cp.Tag = kvp.Value;
                        cp.Dock = DockStyle.Top;
                        pscrolledcontent.Controls.Add(cp);
                        cp.Initialise(kvp.Value);
                    }

                    pscrolledcontent.Controls.Add(csi);

                    // we need it all hooked up now so the tab page gets a size
                    pinnervscroll.Controls.Add(pscrolledcontent);
                    extPanelGradientFillUCCP.Controls.Add(pouter);

                    Theme.Current.Apply(extPanelGradientFillUCCP);

                    // content panel has dock=top children. set size of content panel to them.
                    // Can't use autosize on the content panel as the dock of children means the content panel itself sets the width
                    // so we must manually control the height of the content panel to the height of the children, and the width to the available space

                    Size contentsize = pscrolledcontent.FindMaxSubControlArea(0, 0);
                    pscrolledcontent.Size = new Size(pouter.ContentWidth , contentsize.Height);

                    pinnervscroll.Recalcuate();        // we need to calculate the ranging of the scroll bar..

                    pscrolledcontent.LayoutComplete += Pcontent_LayoutComplete; // if content is relayed out, we need to tell the upper levels for scroll bar/content sizing
                    pinnervscroll.Resize += Pinner_Resize;     // if the inner, docked fill to the outer, and outer, docked fill to the tab resizes, need recalc
                    csi.Resize += (s, e) => { csi.UpdateSystemDiagramAsync(DiscoveryForm.History); };
                }

                {
                    // go thru pouter, pinner to pscrolledcontent
                    ExtPanelGradientFill pscrolledcontent = extPanelGradientFillUCCP.Controls[0].Controls[0].Controls[0] as ExtPanelGradientFill;

                    // find the system display and update
                    ColonisationSystemDisplay cs = pscrolledcontent.Controls.FindTag(current) as ColonisationSystemDisplay;

                    cs.UpdateSystem();
                    cs.UpdateSystemDiagramAsync(DiscoveryForm.History);       // async update always irrespective of visibility in case user makes it visible again
                    if (IsClosed)
                        return;

                    // find each port in current list
                    foreach (var kvp in current.Ports)
                    {
                        ColonisationPortDisplay cp = pscrolledcontent.Controls.FindTag(kvp.Value) as ColonisationPortDisplay;
                        if (cp == null)     // if new, add
                        {
                            cp = new ColonisationPortDisplay();
                            cp.Tag = kvp.Value;
                            cp.Dock = DockStyle.Top;
                            pscrolledcontent.Controls.Add(cp);
                            pscrolledcontent.Controls.SetChildIndex(cp, 0);
                            cp.Initialise(kvp.Value);
                            Theme.Current.Apply(cp);
                        }
                        cp.UpdatePort();
                    }

                    //// unlikely but remove any ports not in current list
                    //foreach( Control ctrl in pscrolledcontent.Controls)
                    //{
                    //    ColonisationPortDisplay cp = ctrl.Tag as ColonisationPortDisplay;     // is it a port display
                    //    if ( cp != null && !current.Ports.Values.ToList().Contains(cp.Port))        // it is, but its not in the current list
                    //    {
                    //        pscrolledcontent.Controls.Remove(cp);
                    //    }
                    //}
                }
            }
        }


        // Inner/Outer has resized, we need to rewidth the content and recalculate the scroll bar
        private void Pinner_Resize(object sender, EventArgs e)
        {
            ExtPanelVertScroll pinner = sender as ExtPanelVertScroll;
            ExtPanelVertScrollWithBar pouter = pinner.Parent as ExtPanelVertScrollWithBar;
            ExtPanelGradientFill pcontent = pinner.Controls[0] as ExtPanelGradientFill;
            pcontent.Width = pouter.ContentWidth;
            pinner.Recalcuate();        // update scroll bar.
        }

        // Layout complete on content - we need to find its content size, and size the pcontent appropriately.
        private void Pcontent_LayoutComplete(ExtPanelGradientFill pcontent)
        {
            ExtPanelVertScroll pinner = pcontent.Parent as ExtPanelVertScroll;
            ExtPanelVertScrollWithBar pouter = pinner.Parent as ExtPanelVertScrollWithBar;

            Size contentsize = pcontent.FindMaxSubControlArea(0, 0);                    // remeasure..
            //System.Diagnostics.Debug.WriteLine($"Pcontent layout {contentsize}");      
            pcontent.Size = new Size(pouter.ContentWidth, contentsize.Height);  // and set size.
            pinner.Recalcuate();        // update scroll bar.
        }

        private ColonisationData colonisation = new ColonisationData();
        private ColonisationSystemData current = null;
        private bool ignorechange = false;

        private const string dbSelection = "Selection";

    }

}
