/*
 * Copyright © 2022-2022 EDDiscovery development team
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

using EDDiscovery.Controls;
using EDDiscovery.UserControls.Helpers;
using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCarrier : UserControlCommonBase
    {
        Timer period = new Timer();
        Timer lightflash = new Timer();
        Control[] leftbottomalignedoverallcontrols;
        Control[] righttopalignedoverallcontrols;
        Control[] lefttopalignedfinancecontrols;
        Control[] righttopalignedfinancecontrols;
        bool loadlayouthappened = false;

        Image fcmapflash;

        #region Init

        public UserControlCarrier()
        {
            InitializeComponent();
            tabPageOverall.Controls.Remove(pictureBoxFleetCarrier);     // ensure its behind everything - irrespective of designer
            pictureBoxFleetCarrier.Dock = DockStyle.Fill;
            tabPageOverall.Controls.Add(pictureBoxFleetCarrier);
            period.Interval = 1000;
            period.Tick += Period_Tick;
            lightflash.Tick += Lightflash_Tick;
            lightflash.Interval = 1000;

            leftbottomalignedoverallcontrols = new Control[] { labelItin1, labelItin2, labelItin3, labelItin4, labelItin5 };
            righttopalignedoverallcontrols = new Control[] { labelBalance, labelCargo, labelCrewServicesSpace, labelShipPacks, labelModulePacks, labelFreeSpace,
                                                     labelJumpRange, labelJumpMax, labelFuelLevel, labelDockingAccess, labelNotorious};
            lefttopalignedfinancecontrols = new Control[] { labelFCarrierBalance, labelFReserveBalance, labelFAvailableBalance, labelFReservePercent };
            righttopalignedfinancecontrols = new Control[] { labelFTaxPioneerSupplies, labelFTaxShipyard, labelFTaxRearm, labelFTaxOutfitting, labelFTaxRefuel, labelFTaxRepair };

            var org = global::EDDiscovery.Icons.Controls.FleetCarrier;
            fcmapflash = (Image)org.Clone();

            using (Graphics gr = Graphics.FromImage(fcmapflash))
            {
                using (Brush br = new SolidBrush(Color.FromArgb(100, Color.Gray)))
                {
                    gr.FillRectangle(br, new Rectangle(436, 336, 8, 4));
                    gr.FillRectangle(br, new Rectangle(436, 336, 8, 4));
                    gr.FillRectangle(br, new Rectangle(392, 306, 4, 4));
                    gr.FillRectangle(br, new Rectangle(392, 306, 4, 4));
                }

                using (Brush br2 = new SolidBrush(Color.FromArgb(100, 40, 40, 40)))
                {
                    gr.FillRectangle(br2, new Rectangle(627, 410, 4, 4));
                    gr.FillRectangle(br2, new Rectangle(627, 410, 4, 4));
                    gr.FillRectangle(br2, new Rectangle(627, 448, 4, 4));
                    gr.FillRectangle(br2, new Rectangle(627, 448, 4, 4));
                }
            }
        }

        public override void Init()
        {
            DBBaseName = "CarrierPanel";
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnThemeChanged += Discoveryform_OnThemeChanged;
        }

        public override void LoadLayout()
        {
            loadlayouthappened = true;
        }

        public override void InitialDisplay()
        {
            Display();
            Discoveryform_OnThemeChanged();
            lightflash.Start();
        }

        public override void Closing()
        {
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnThemeChanged -= Discoveryform_OnThemeChanged;
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)     
        {
            ClearAndDisplay();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)     
        {
            if ( he.journalEntry is ICarrierStats)
            {
                Display();
            }
        }

        private void Discoveryform_OnThemeChanged()
        {
            Font medium = ExtendedControls.Theme.Current.GetScaledFont(1.2f);
            foreach (var x in righttopalignedoverallcontrols)
                x.Font = medium;
            foreach (var x in leftbottomalignedoverallcontrols)
                x.Font = medium;
            foreach (var x in lefttopalignedfinancecontrols)
                x.Font = medium;
            foreach (var x in righttopalignedfinancecontrols)
                x.Font = medium;

            Font big = ExtendedControls.Theme.Current.GetScaledFont(2.0f);
            labelStarSystem.Font =
            labelBalance.Font =
            labelDestSystemTime.Font =
            labelName.Font = big;

            PositionControls();
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if ( loadlayouthappened)
                PositionControls();
        }


        int colc = 0;
        private void Lightflash_Tick(object sender, EventArgs e)
        {
            if (colc++ % 2 == 0)
                this.pictureBoxFleetCarrier.Image = global::EDDiscovery.Icons.Controls.FleetCarrier;
            else
                this.pictureBoxFleetCarrier.Image = fcmapflash;
        }


        #endregion

        #region Display

        private void ClearAndDisplay()
        {
            dataGridViewItinerary.Rows.Clear();
            Display();
        }

        private async void Display()
        {
            var cs = discoveryform.history.Carrier;

            cs.CheckCarrierJump(DateTime.UtcNow);       // see if auto jump happened

            if (dataGridViewItinerary.RowCount != cs.JumpHistory.Count)     // only update if we have not displayed all rows
            {
                DataGridViewColumn sortcol = dataGridViewItinerary.SortedColumn != null ? dataGridViewItinerary.SortedColumn : dataGridViewItinerary.Columns[0];
                SortOrder sortorder = dataGridViewItinerary.SortOrder != SortOrder.None ? dataGridViewItinerary.SortOrder : SortOrder.Descending;

                dataGridViewItinerary.Rows.Clear();
                ISystem lastsys = null;

                ISystem cursys = discoveryform.history.GetLast?.System;     // last system, if present

                for (int i = cs.JumpHistory.Count - 1; i >= 0; i--)
                {
                    var it = cs.JumpHistory[i];

                    if (!it.StarSystem.HasCoordinate)
                    {
                        ISystem p = await EliteDangerousCore.DB.SystemCache.FindSystemAsync(it.StarSystem, true);           // find, even in EDSM, synchronously for now
                        if (IsClosed)       // may have closed in the meanwhile
                            return;
                        if (p != null)      // if found, replace star system with it, for future
                            it.StarSystem = p;  // this should be fine - the list gets cleared on new commander, and only ever filled, not overwritten
                    }

                    double dist = lastsys != null && it.StarSystem.HasCoordinate ? lastsys.Distance(it.StarSystem) : -1;

                    lastsys = it.StarSystem;

                    double distfrom = cursys != null && cursys.HasCoordinate && it.StarSystem.HasCoordinate ? cursys.Distance(it.StarSystem) : -1;

                    object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(it.JumpTime),
                                        it.StarSystem.Name,
                                        it.Body.HasChars() ? it.Body : "System",
                                        it.StarSystem.HasCoordinate ? it.StarSystem.X.ToString("N2") : "",
                                        it.StarSystem.HasCoordinate ? it.StarSystem.Y.ToString("N2") : "",
                                        it.StarSystem.HasCoordinate ? it.StarSystem.Z.ToString("N2") : "",
                                        dist>0 ? dist.ToString("N1") : "",
                                        distfrom>0 ? distfrom.ToString("N1") : "",
                                        "?",
                    };

                    dataGridViewItinerary.Rows.Add(rowobj);
                }

                dataGridViewItinerary.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                dataGridViewItinerary.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

                labelItin1.Text = cs.LastJumpText(1);       // itinerary has changed, fill
                labelItin2.Text = cs.LastJumpText(2);
                labelItin3.Text = cs.LastJumpText(3);
                labelItin4.Text = cs.LastJumpText(4);
                labelItin5.Text = cs.LastJumpText(5);
            }

            {                                           // main tab
                if (cs.State.HaveCarrier)
                {
                    labelName.Text = cs.State.Name + " " + cs.State.Callsign;
                    if (cs.IsDecommisioned)
                        labelName.Text += " (" + "Decommisioned".TxID(EDTx.Unknown) + ")";
                    else if ( cs.IsDecommisioning )
                    {
                        labelName.Text += " (" + "Decommissioning on".TxID(EDTx.Unknown) + " " + EDDConfig.Instance.ConvertTimeToSelectedFromUTC(cs.DecommisionTimeUTC.Value) + ")";
                    }

                    labelBalance.Text = (cs.State.Finance.AvailableBalance * 1).ToString("N0") + "cr";
                    labelStarSystem.Text = (cs.StarSystem.Name ?? "Unknown") + (cs.BodyID != 0 ? (": " + cs.Body) : "");
                    labelCargo.Text = "Cargo".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.Cargo.ToString("N0") + "t";
                    labelCrewServicesSpace.Text = "Services".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.Crew.ToString("N0") + "t";
                    labelShipPacks.Text = "Ship packs".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.ShipPacks.ToString("N0") + "t";
                    labelModulePacks.Text = "Module Packs".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.ModulePacks.ToString("N0") + "t";
                    labelFreeSpace.Text = "Free Space".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.FreeSpace.ToString("N0") + "t";
                    labelJumpRange.Text = "Jump Range".TxID(EDTx.Unknown) + ": " + cs.State.JumpRangeCurr.ToString("N0") + "ly";
                    labelJumpMax.Text = "Max Jump".TxID(EDTx.Unknown) + ": " + cs.State.JumpRangeMax.ToString("N0") + "ly";
                    labelFuelLevel.Text = "Fuel".TxID(EDTx.Unknown) + ": " + cs.State.FuelLevel.ToString("N0") + "t";
                    labelDockingAccess.Text = "Docking Access".TxID(EDTx.Unknown) + ": " + cs.State.DockingAccessSplittable.SplitCapsWordFull();
                    labelNotorious.Text = "Notorious".TxID(EDTx.Unknown) + ": " + (cs.State.AllowNotorious ? "Yes".TxID(EDTx.MessageBoxTheme_Yes) : "No".TxID(EDTx.MessageBoxTheme_No));


                    labelFCarrierBalance.Text = "Balance".TxID(EDTx.Unknown) + ": " + cs.State.Finance.CarrierBalance.ToString("N0") + "cr";
                    labelFReserveBalance.Text = "Reserve".TxID(EDTx.Unknown) + ": " + cs.State.Finance.ReserveBalance.ToString("N0") + "cr";
                    labelFAvailableBalance.Text = "Available".TxID(EDTx.Unknown) + ": " + cs.State.Finance.AvailableBalance.ToString("N0") + "cr";
                    labelFReservePercent.Text = "Reserve".TxID(EDTx.Unknown) + ": " + cs.State.Finance.ReservePercent.ToString("N0") + "%";
                    labelFTaxPioneerSupplies.Text = "Pioneer Tax".TxID(EDTx.Unknown) + ": " + cs.State.Finance.TaxRatePioneersupplies.ToString("N1") + "%";
                    labelFTaxShipyard.Text = "Shipyard Tax".TxID(EDTx.Unknown) + ": " + cs.State.Finance.TaxRateShipyard.ToString("N1") + "%";
                    labelFTaxRearm.Text = "Rearm Tax".TxID(EDTx.Unknown) + ": " + cs.State.Finance.TaxRateRearm.ToString("N1") + "%";
                    labelFTaxOutfitting.Text = "Outfitting Tax".TxID(EDTx.Unknown) + ": " + cs.State.Finance.TaxRateOutfitting.ToString("N1") + "%";
                    labelFTaxRefuel.Text = "Refuel Tax".TxID(EDTx.Unknown) + ": " + cs.State.Finance.TaxRateRefuel.ToString("N1") + "%";
                    labelFTaxRepair.Text = "Repair Tax".TxID(EDTx.Unknown) + ": " + cs.State.Finance.TaxRateRepair.ToString("N1") + "%";
                }
                else
                {
                    labelName.Text = "No Carrier".TxID(EDTx.Unknown);
                }

                DestinationSystem();

                foreach (var x in righttopalignedoverallcontrols)
                    x.Visible = cs.State.HaveCarrier;
                foreach (var x in leftbottomalignedoverallcontrols)
                    x.Visible = cs.State.HaveCarrier;
                foreach (var x in lefttopalignedfinancecontrols)
                    x.Visible = cs.State.HaveCarrier;
                foreach (var x in righttopalignedfinancecontrols)
                    x.Visible = cs.State.HaveCarrier;

                labelStarSystem.Visible = cs.State.HaveCarrier;
            }

            // add display of Crew Services activation
            // add display of Ship and module packs

            PositionControls();
        }

        private const int linemargin = 8;
        private const int hspacing = 8;
        private void PositionControls()
        {
            labelStarSystem.Top = labelDestSystemTime.Top = labelBalance.Bottom + linemargin;
            pictureBoxGoto.Location = new Point(labelStarSystem.Right + hspacing, labelStarSystem.YCenter() - pictureBoxGoto.Height / 2);
            labelDestSystemTime.Left = pictureBoxGoto.Right + hspacing;

            for ( int x = 0; x < leftbottomalignedoverallcontrols.Count(); x++)
                leftbottomalignedoverallcontrols[x].Location = new Point(labelStarSystem.Left, tabPageOverall.Height - (labelItin1.Height + linemargin) * (5-x));

            int maxlabelw = righttopalignedoverallcontrols.Select(x => x.Width).Max();
            int rcolpos = this.Width - ((maxlabelw+20) / 50) * 50 + 70;       // some hysteris (+N), need to use client width, tab pages are not created until visible size wise

            int ypos = labelBalance.Top;
            foreach (var x in righttopalignedoverallcontrols)
            {
                x.Location = new Point(rcolpos, ypos);      
                ypos += x.Height + linemargin;
                if (x == labelBalance)
                    ypos += linemargin;
            }

            ypos = labelBalance.Top;        // same pos on this page
            foreach (var x in lefttopalignedfinancecontrols)
            {
                x.Location = new Point(labelStarSystem.Left, ypos);
                ypos += x.Height + linemargin;
            }

            ypos = labelBalance.Top;        // same pos on this page
            foreach (var x in righttopalignedfinancecontrols)
            {
                x.Location = new Point(rcolpos, ypos);
                ypos += x.Height + linemargin;
            }
        }

        private void DestinationSystem()
        {
            var cs = discoveryform.history.Carrier;

            if ( cs.IsJumping )
            {
                var timetogo = cs.TimeTillJump;
                labelDestSystemTime.Text = cs.NextStarSystem + (cs.NextBodyID != 0 ? (": " + cs.NextBody) : "") + " " +
                                (timetogo.TotalSeconds > 0 ? timetogo.ToString(@"mm\.ss") : "Jumping".TxID(EDTx.Unknown)); 
                if ( !period.Enabled )
                    period.Start();     // jumping, make sure its started
            }

            pictureBoxGoto.Visible = labelDestSystemTime.Visible = cs.IsJumping;
        }

        #endregion

        #region Interactivity

        private void Period_Tick(object sender, EventArgs e)
        {
            var cs = discoveryform.history.Carrier;

            if (cs.CheckCarrierJump(DateTime.UtcNow)) // if autojump happened
                Display();      // redisplay all
            else
                DestinationSystem();     // redisplay just the time

            if (!cs.IsJumping)           // when jumping, we update the display
                period.Stop();          // not needed when not jumping
        }

        private void dataGridViewItinerary_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index <= 2 )
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column.Index <= 7)
                e.SortDataGridViewColumnNumeric();
        }

        #endregion


    }
}
