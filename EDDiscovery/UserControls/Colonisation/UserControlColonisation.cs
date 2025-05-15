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
            colonisedsystems = new Dictionary<long,ColonisationSystemData>();

            var colonisationevents = HistoryList.FilterByEventEntryOrder(DiscoveryForm.History.EntryOrder(), 
                            new HashSet<JournalTypeEnum> { JournalTypeEnum.ColonisationBeaconDeployed, JournalTypeEnum.ColonisationConstructionDepot, JournalTypeEnum.ColonisationContribution, 
                                JournalTypeEnum.ColonisationSystemClaim, JournalTypeEnum.ColonisationSystemClaimRelease,
                                JournalTypeEnum.Docked , JournalTypeEnum.FSDJump, JournalTypeEnum.Location, JournalTypeEnum.CarrierJump});

            foreach (var he in colonisationevents.EmptyIfNull())
                ColonisationSystemData.AddColonisationEntry(colonisedsystems,he, colonisationevents);

            extTabControl.TabPages.Clear();

            Display();
        }
        private void NewEntry(HistoryEntry he)
        {
            bool redisplay = ColonisationSystemData.AddColonisationEntry(colonisedsystems, he, DiscoveryForm.History.EntryOrder());

            // check for star scan updates
            if (he.journalEntry is IStarScan || he.journalEntry is IMaterialJournalEntry || he.journalEntry is IBodyNameAndID)
            {
                if (colonisedsystems.Values.ToList().Find(x => x.System == he.System) != null)      // if the he system relates to ours
                {
                    redisplay = true; // due to a change in star scan data, see UserControlScan:NewEntry for a copy of this clause
                }
            }

            if ( redisplay )
            {
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
            extTabControl.PaintTransparentColor = on ? curcol : Color.Transparent;
        }

        private void Display()
        {
            foreach(var kvp in colonisedsystems)
            {
                TabPage tp = extTabControl.FindTabPageByName(kvp.Value.System.Name);
                if ( tp == null )
                {
                    tp = new TabPage();
                    tp.Name = tp.Text = kvp.Value.System.Name;

                    ExtPanelVertScrollWithBar pouter = new ExtPanelVertScrollWithBar();
                    pouter.Dock = DockStyle.Fill;
                    ExtPanelVertScroll pinnervscroll = new ExtPanelVertScroll();
                    pinnervscroll.Dock = DockStyle.Fill;
                    pinnervscroll.Name = "PanelVertScroll";
                    pouter.Controls.Add(pinnervscroll);

                    ExtPanelGradientFill pcontent = new ExtPanelGradientFill();     // use this due to having LayoutComplete callback
                    pcontent.Name = "Content Holder";
                    pcontent.Location = new Point(0, 0);

                    foreach (var k in kvp.Value.Ports)
                    {
                        ColonisationPort cp = new ColonisationPort();
                        cp.Tag = k.Value;
                        cp.Update(k.Value);
                        cp.Dock = DockStyle.Top;
                        pcontent.Controls.Add(cp);
                        cp.Initialise();
                    }

                    ColonisationSystem csi = new ColonisationSystem();
                    csi.Tag = kvp.Value;
                    csi.Dock = DockStyle.Top;
                    pcontent.Controls.Add(csi);

                    csi.Initialise(kvp.Value.System, DiscoveryForm.History);

                    // we need it all hooked up now so the tab page gets a size
                    pinnervscroll.Controls.Add(pcontent);
                    tp.Controls.Add(pouter);
                    extTabControl.TabPages.Add(tp);

                    Theme.Current.Apply(tp);

                    // content panel has dock=top children. set size of content panel to them.
                    // Can't use autosize on the content panel as the dock of children means the content panel itself sets the width
                    // so we must manually control the height of the content panel to the height of the children, and the width to the available space

                    Size contentsize = pcontent.FindMaxSubControlArea(0, 0);
                    pcontent.Size = new Size(pouter.ContentWidth , contentsize.Height);

                    pinnervscroll.Recalcuate();        // we need to calculate the ranging of the scroll bar..

                    pcontent.LayoutComplete += Pcontent_LayoutComplete; // if content is relayed out, we need to tell the upper levels for scroll bar/content sizing
                    pinnervscroll.Resize += Pinner_Resize;     // if the inner, docked fill to the outer, and outer, docked fill to the tab resizes, need recalc
                    csi.Resize += (s, e) => { csi.UpdateSystemDiagramAsync(kvp.Value, DiscoveryForm.History); };
                }

                ColonisationSystem cs = tp.Controls[0].Controls[0].Controls[0].Controls.FindTag(kvp.Value) as ColonisationSystem;
                cs.Update(kvp.Value);
                cs.UpdateSystemDiagramAsync(kvp.Value, DiscoveryForm.History);       // async update always irrespective of visibility in case user makes it visible again
                if (IsClosed)
                    return;
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
            System.Diagnostics.Debug.WriteLine($"Pcontent layout {contentsize}");      
            pcontent.Size = new Size(pouter.ContentWidth, contentsize.Height);  // and set size.
            pinner.Recalcuate();        // update scroll bar.
        }

        private Dictionary<long, ColonisationSystemData> colonisedsystems;
    }

}
