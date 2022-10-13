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

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCarrier : UserControlCommonBase
    {
        private Timer period = new Timer();
        private int periodtickcounter = 0;

        private Control[] lefttopalignedfinancecontrols;
        private Control[] midtopalignedfinancecontrols;
        private Control[] righttopalignedfinancecontrols;
        private bool loadlayouthappened = false;
        private Font bigfont, normfont;
        private bool paintedlayer1 = false;
        private const int linemargin = 8;
        private const int hspacing = 8;
        private const int servicewidth = 2000;
        private const int serviceheight = 140;

        #region Init

        public UserControlCarrier()
        {
            InitializeComponent();

            period.Interval = 1000;
            period.Tick += Period_Tick;

            lefttopalignedfinancecontrols = new Control[] { labelFCarrierBalance, labelFReserveBalance, labelFAvailableBalance, labelFReservePercent };
            midtopalignedfinancecontrols = new Control[] { labelFCoreCost, labelFServicesCost };
            righttopalignedfinancecontrols = new Control[] { labelFTaxPioneerSupplies, labelFTaxShipyard, labelFTaxRearm, labelFTaxOutfitting, labelFTaxRefuel, labelFTaxRepair };

            var org = global::EDDiscovery.Icons.Controls.FleetCarrier;
            imageControlOverall.ImageSize = org.Size;     // same size as FC PNG
            imageControlOverall.ImageDepth = 3;         // 0 is most text, 1 is destination, 2 is carrier flashes

            using (Graphics gr = imageControlOverall.GetGraphics(2))        // paint this on plane 2, which is visibility toggled
            {
                using (Brush br = new SolidBrush(Color.FromArgb(200, 92, 79, 75)))
                {
                    gr.FillRectangle(br, new Rectangle(392, 306, 4, 4));
                }
                using (Brush br1 = new SolidBrush(Color.FromArgb(200, 109, 96, 91)))
                {
                    gr.FillRectangle(br1, new Rectangle(436, 335, 8, 4));
                }

                using (Brush br2 = new SolidBrush(Color.FromArgb(200, 40, 40, 40)))
                {
                    gr.FillRectangle(br2, new Rectangle(627, 410, 4, 4));
                    gr.FillRectangle(br2, new Rectangle(627, 448, 4, 4));
                }

                using (Brush br3 = new SolidBrush(Color.FromArgb(200, Color.FromArgb(255, 89, 72, 67))))
                {
                    gr.FillRectangle(br3, new Rectangle(759, 472, 5, 5));
                }

                using (Brush br4 = new SolidBrush(Color.FromArgb(200, Color.FromArgb(120, 10, 10, 10))))
                {
                    gr.FillRectangle(br4, new Rectangle(766, 513, 5, 5));
                }
                using (Brush br5 = new SolidBrush(Color.FromArgb(250, Color.FromArgb(200,88,73,69))))
                {
                    gr.FillRectangle(br5, new Rectangle(1096, 625, 5, 4));
                }
            }

            imageControlServices.ImageSize = new Size(2000, (serviceheight + linemargin) * EliteDangerousCore.JournalEvents.JournalCarrierCrewServices.GetServiceCount() + linemargin);
            imageControlServices.Height = imageControlServices.ImageSize.Height;

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
            Discoveryform_OnThemeChanged();
            period.Start();
        }

        public override void Closing()
        {
            period.Stop();
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            discoveryform.OnThemeChanged -= Discoveryform_OnThemeChanged;
            bigfont.Dispose();
            normfont.Dispose();
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)     
        {
            // discoveryform.history.Carrier.SetCarrierJump("Fred", 20202, "fredplanet", 22, DateTime.UtcNow);//debug check
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
            bigfont = new Font(ExtendedControls.Theme.Current.FontName, 16f);
            normfont = new Font(ExtendedControls.Theme.Current.FontName, 12f);

            using (Font medium = ExtendedControls.Theme.Current.GetScaledFont(1.2f))
            {
                foreach (var x in lefttopalignedfinancecontrols)
                    x.Font = medium;
                foreach (var x in midtopalignedfinancecontrols)
                    x.Font = medium;
                foreach (var x in righttopalignedfinancecontrols)
                    x.Font = medium;
            }

            tabPageOverall.BackColor = Color.Black;
            ClearAndDisplay();
            PositionControls();
        }


        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (loadlayouthappened)
            {
                PositionControls();
            }
        }


        private void Period_Tick(object sender, EventArgs e)
        {
            var cs = discoveryform.history.Carrier;

            if (cs.CheckCarrierJump(DateTime.UtcNow)) // if autojump happened
                Display();      // redisplay all - including destinationsystem
            else
                DestinationSystem();     // redisplay just the time

            if ( ++periodtickcounter % 4 == 0)     // every N go, change
            {
                imageControlOverall.ImageVisible[2] = periodtickcounter % 8 == 0;
                imageControlOverall.Invalidate();       
            }
        }


        #endregion

        #region Display

        private void ClearAndDisplay()
        {
            dataGridViewItinerary.Rows.Clear();
            dataGridViewLedger.Rows.Clear();
            Display();
        }

        private async void Display()
        {
            var cs = discoveryform.history.Carrier;

            cs.CheckCarrierJump(DateTime.UtcNow);       // see if auto jump happened

            Color color = ExtendedControls.Theme.Current.SPanelColor;

            int rightpos = imageControlOverall.ImageWidth - 350;

            imageControlOverall.Clear(0);

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
                            it.SetSystem(p);  // this should be fine - the list gets cleared on new commander, and only ever filled, not overwritten
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

                string[] text = new string[] { cs.LastJumpText(1), cs.LastJumpText(2), cs.LastJumpText(3), cs.LastJumpText(4), cs.LastJumpText(5), cs.LastJumpText(6) };
                int lines = text.Where(x => x != null).Count();
                imageControlOverall.DrawText(new Point(hspacing, imageControlOverall.ImageHeight - linemargin - lines * (normfont.Height + linemargin)),
                                    new Size(30000, 30000), text, normfont, linemargin, color);
            }

            if ( dataGridViewLedger.RowCount != cs.Ledger.Count)
            {
                DataGridViewColumn sortcol = dataGridViewLedger.SortedColumn != null ? dataGridViewLedger.SortedColumn : dataGridViewLedger.Columns[0];
                SortOrder sortorder = dataGridViewLedger.SortOrder != SortOrder.None ? dataGridViewLedger.SortOrder : SortOrder.Descending;

                dataGridViewLedger.Rows.Clear();

                for (int i = cs.Ledger.Count - 1; i >= 0; i--)
                {
                    var le = cs.Ledger[i];
                    long diff = i > 0 ? (le.Balance - cs.Ledger[i - 1].Balance) : 0;        // difference between us and previous, + if credit, - if not


                    object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(le.JournalEntry.EventTimeUTC),
                                        le.StarSystem.Name,
                                        le.Body,
                                        le.JournalEntry.EventTypeStr.SplitCapsWordFull(),
                                        diff>0 ? diff.ToString("N0") : "",
                                        diff<0 ? (-diff).ToString("N0") : "",
                                        le.Balance.ToString("N0"),
                                        le.Notes
                    };

                    dataGridViewLedger.Rows.Add(rowobj);
                }

                dataGridViewLedger.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                dataGridViewLedger.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
            }

            {                                           // main tab
                int vposl = linemargin, vposr = linemargin;

                if (cs.State.HaveCarrier)
                {
                    string name = cs.State.Name + " " + cs.State.Callsign;

                    if (cs.IsDecommisioned)
                        name += " (" + "Decommisioned".TxID(EDTx.Unknown) + ")";
                    else if ( cs.IsDecommisioning )
                    {
                        name += " (" + "Decommissioning on".TxID(EDTx.Unknown) + " " + EDDConfig.Instance.ConvertTimeToSelectedFromUTC(cs.DecommisionTimeUTC.Value) + ")";
                    }

                    imageControlOverall.DrawText(new Point(hspacing, vposl), new Size(30000, 30000), name, bigfont, color);
                    vposl += bigfont.Height + linemargin;

                    imageControlOverall.DrawText(new Point(hspacing, vposl), new Size(30000, 30000), cs.CurrentPositionText, bigfont, color);
                    vposl += bigfont.Height + linemargin;

                    imageControlOverall.DrawText(new Point(rightpos, vposr), new Size(30000, 30000), (cs.State.Finance.AvailableBalance * 1).ToString("N0") + "cr",
                                                bigfont, color);
                    vposr += bigfont.Height + linemargin;

                    string[] text = new string[] {
                        "Cargo".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.Cargo.ToString("N0") + "t",
                        "Services".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.Crew.ToString("N0") + "t",
                        "Ship packs".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.ShipPacks.ToString("N0") + "t",
                        "Module Packs".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.ModulePacks.ToString("N0") + "t",
                        "Free Space".TxID(EDTx.Unknown) + ": " + cs.State.SpaceUsage.FreeSpace.ToString("N0") + "t",
                        "Jump Range".TxID(EDTx.Unknown) + ": " + cs.State.JumpRangeCurr.ToString("N0") + "ly",
                        "Max Jump".TxID(EDTx.Unknown) + ": " + cs.State.JumpRangeMax.ToString("N0") + "ly",
                        "Fuel".TxID(EDTx.Unknown) + ": " + cs.State.FuelLevel.ToString("N0") + "t",
                        "Docking Access".TxID(EDTx.Unknown) + ": " + cs.State.DockingAccessSplittable.SplitCapsWordFull(),
                        "Notorious".TxID(EDTx.Unknown) + ": " + (cs.State.AllowNotorious ? "Yes".TxID(EDTx.MessageBoxTheme_Yes) : "No".TxID(EDTx.MessageBoxTheme_No)),
                    };

                    imageControlOverall.DrawText(new Point(rightpos, vposr + linemargin), new Size(30000, 30000), text, normfont, linemargin, color);

                    labelFCarrierBalance.Text = "Balance".TxID(EDTx.Unknown) + ": " + cs.State.Finance.CarrierBalance.ToString("N0") + "cr";
                    labelFReserveBalance.Text = "Reserve".TxID(EDTx.Unknown) + ": " + cs.State.Finance.ReserveBalance.ToString("N0") + "cr";
                    labelFAvailableBalance.Text = "Available".TxID(EDTx.Unknown) + ": " + cs.State.Finance.AvailableBalance.ToString("N0") + "cr";
                    labelFReservePercent.Text = "Reserve".TxID(EDTx.Unknown) + ": " + cs.State.Finance.ReservePercent.ToString("N0") + "%";

                    TaxRate(labelFTaxPioneerSupplies, "Pioneer Tax".TxID(EDTx.Unknown), cs.State.Finance.TaxRatePioneersupplies);
                    TaxRate(labelFTaxShipyard, "Shipyard Tax".TxID(EDTx.Unknown), cs.State.Finance.TaxRateShipyard);
                    TaxRate(labelFTaxRearm, "Rearm Tax".TxID(EDTx.Unknown), cs.State.Finance.TaxRateRearm);
                    TaxRate(labelFTaxOutfitting, "Outfitting Tax".TxID(EDTx.Unknown), cs.State.Finance.TaxRateOutfitting);
                    TaxRate(labelFTaxRefuel, "Refuel Tax".TxID(EDTx.Unknown),  cs.State.Finance.TaxRateRefuel);
                    TaxRate(labelFTaxRepair, "Repair Tax".TxID(EDTx.Unknown), cs.State.Finance.TaxRateRepair);

                    labelFCoreCost.Text = "Core Cost: ".TxID(EDTx.Unknown) + cs.State.GetCoreCost().ToString("N0") + "cr";
                    labelFServicesCost.Text = "Services Cost: ".TxID(EDTx.Unknown) + cs.State.GetServicesCost().ToString("N0") + "cr";
                }
                else
                {
                    imageControlOverall.DrawText(new Point(hspacing, vposl), new Size(30000, 30000), "No Carrier".TxID(EDTx.Unknown), bigfont,color);
                }

                DestinationSystem();

                foreach (var x in righttopalignedfinancecontrols)
                    x.Visible = cs.State.HaveCarrier;
                foreach (var x in midtopalignedfinancecontrols)
                    x.Visible = cs.State.HaveCarrier;
                foreach (var x in lefttopalignedfinancecontrols)
                    x.Visible = cs.State.HaveCarrier;
            }

            {
                imageControlServices.Clear();

                Graphics gr = imageControlServices.GetGraphics();
                int vpos = linemargin;

                foreach (JournalCarrierCrewServices.ServiceType en in Enum.GetValues(typeof(JournalCarrierCrewServices.ServiceType)))
                {
                    if (JournalCarrierCrewServices.IsValidService(en))
                    {
                        var area = new Rectangle(hspacing, vpos, servicewidth, serviceheight);

                        using (Brush br = new SolidBrush(Color.FromArgb(255, 53, 36, 2)))
                        {
                            gr.FillRectangle(br, area);
                        }

                        var pointtextleft = new Point(hspacing * 3, vpos + linemargin * 2);

                        const int titlewidth = 200;

                        var size = imageControlServices.DrawMeasureText(pointtextleft, new Size(titlewidth, 1000), JournalCarrierCrewServices.GetTranslatedServiceName(en), bigfont, color);
                        pointtextleft.Y += (int)(size.Height + 1);
                        string coreoroptional = en <= EliteDangerousCore.JournalEvents.JournalCarrierCrewServices.ServiceType.TritiumDepot ? "Core Service" : "Optional Service";
                        imageControlServices.DrawText(pointtextleft, new Size(titlewidth, 1000), coreoroptional, normfont, color);

                        Image img = BaseUtils.Icons.IconSet.Instance.Get("Controls." + en.ToString());
                        imageControlServices.DrawImage(img, new Rectangle(hspacing * 4, vpos + serviceheight - linemargin - img.Height,img.Width,img.Height));

                        var servicestate = cs.State.GetService(en);     // may be null for a core or non listed service
                        var optional = JournalCarrierCrewServices.IsOptionalService(en);
                        bool active = servicestate != null && servicestate.Enabled == true && servicestate.Activated == true;
                        bool disabled= servicestate != null && servicestate.Enabled == false && servicestate.Activated == true;

                        var servicecol1top = new Point(titlewidth + 50, vpos + linemargin * 2);

                        JournalCarrierCrewServices.ServicesData si = JournalCarrierCrewServices.GetDataOnServiceType(en);
                        if (si != null)
                        {
                            int lineh = normfont.Height + linemargin * 2;
                            imageControlServices.DrawText(new Point(servicecol1top.X, servicecol1top.Y + lineh), new Size(2000, 2000), "Install cost: " + si.InstallCost.ToString("N0"), normfont, color);
                            imageControlServices.DrawText(new Point(servicecol1top.X, servicecol1top.Y + lineh * 2), new Size(2000, 2000), "Capacity Allocated: " + si.CargoSize.ToString("N0") + " Units", normfont, color);

                            imageControlServices.DrawText(new Point(servicecol1top.X + 400, servicecol1top.Y + lineh), new Size(2000, 2000), "Upkeep cost: " + si.UpkeepCost.ToString("N0"), normfont, color);
                            imageControlServices.DrawText(new Point(servicecol1top.X + 400, servicecol1top.Y + lineh * 2), new Size(2000, 2000), "Suspended upkeep cost: " + si.SuspendedUpkeepCost.ToString("N0"), normfont, color);
                        }

                        // TBD add tick

                        if ( !optional || active )
                        {
                            imageControlServices.DrawText(servicecol1top, new Size(2000, 2000), "This Service is Active", normfont, color);
                        }
                        else if ( disabled )
                        {
                            imageControlServices.DrawText(servicecol1top, new Size(2000, 2000), "This Service is Suspended", normfont, color);
                            using (Brush br2 = new SolidBrush(Color.FromArgb(80, 255, 255, 0)))
                            {
                                gr.FillRectangle(br2, area);
                            }
                        }
                        else
                        {
                            imageControlServices.DrawText(servicecol1top, new Size(2000, 2000), "This Service is Inactive", normfont, color);
                            using (Brush br2 = new SolidBrush(Color.FromArgb(80, 255, 0, 0)))
                            {
                                gr.FillRectangle(br2, area);
                            }
                        }


                        vpos += serviceheight + linemargin;
                    }
                }

                imageControlServices.Invalidate();
            }

            // add display of Ship and module packs

            imageControlOverall.Invalidate();       // force overall image stack 
            PositionControls();
        }

        private void DestinationSystem()
        {
            var cs = discoveryform.history.Carrier;

            if (cs.State.HaveCarrier && cs.IsJumping)
            {
                imageControlOverall.Clear(1);       // clear destination bitmap plane - plane 1

                int vpos = bigfont.Height + linemargin * 2;
                int hpos = hspacing + (int)BaseUtils.BitMapHelpers.MeasureStringInBitmap(cs.CurrentPositionText, bigfont).Width + hspacing;

                var timetogo = cs.TimeTillJump;
                string jumptext = cs.NextDestinationText + " " + (timetogo.TotalSeconds > 0 ? timetogo.ToString(@"mm\.ss") : "Jumping".TxID(EDTx.Unknown));

                imageControlOverall.DrawImage(global::EDDiscovery.Icons.Controls.ArrowsRight, new Rectangle(hpos, vpos, 48, 24), bitmap:1);
                hpos += 48;

                imageControlOverall.DrawText(new Point(hpos, vpos), new Size(30000, 30000), jumptext, bigfont, ExtendedControls.Theme.Current.SPanelColor, bitmap: 1);

                if (!period.Enabled)
                    period.Start();     // jumping, make sure its started

                imageControlOverall.Invalidate();

                paintedlayer1 = true;
            }
            else if ( paintedlayer1)
            {
                imageControlOverall.Clear(1);       // clear destination bitmap plane
                imageControlOverall.Invalidate();
                paintedlayer1 = false;              // and don't repaint again
            }
        }

        private void TaxRate(Label l, string t, double? value)
        {
            if (value.HasValue)
                l.Text = t + ": " + value.Value.ToString("N0") + "%";
            else
                l.Text = "";
        }

        private void PositionControls()
        {
            int lypos = linemargin;
            foreach (var x in lefttopalignedfinancecontrols)
            {
                x.Location = new Point(hspacing, lypos);
                lypos += x.Height + linemargin;
            }

            int midpos = Width / 2 - 200;
            int lmpos = linemargin;
            foreach (var x in midtopalignedfinancecontrols)
            {
                x.Location = new Point(midpos, lmpos);
                lmpos += x.Height + linemargin;
            }

            int rightpos = Width- 270;
            int rypos = linemargin;
            foreach (var x in righttopalignedfinancecontrols)
            {
                x.Location = new Point(rightpos, rypos);
                if (x.Text.HasChars())
                    rypos += x.Height + linemargin;
            }

            panelFinancesTop.Height = Math.Max(rypos, lypos);
        }

        #endregion

        #region Helpers


        private void dataGridViewItinerary_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index <= 2 )
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column.Index <= 7)
                e.SortDataGridViewColumnNumeric();
        }

        private void dataGridViewLedger_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 0)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 2)
                e.SortDataGridViewColumnNumeric();
        }


        #endregion


    }
}
