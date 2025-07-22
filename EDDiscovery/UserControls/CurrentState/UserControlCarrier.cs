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
 */

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

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
        private const int servicewidth = 1400;
        private const int serviceheight = 140;

        private string dbSplitterSaveCAPI1 = "CAPISplitter1";
        private string dbSplitterSaveCAPI2 = "CAPISplitter2";
        private string dbTabSave = "TabPage";
        private string dbCAPISave = "CAPI_Fleetcarrier_Data";           // global across all panels
        private string dbCAPIDateUTC = "CAPI_Fleetcarrier_Date";
        private string dbCAPICommander = "CAPI_Fleetcarrier_CmdrID";
        private string dbSCLedger = "SCFinance";

        #region Init

        public UserControlCarrier()
        {
            InitializeComponent();

            DBBaseName = "CarrierPanel";

            period.Interval = 1000;
            period.Tick += Period_Tick;

            lefttopalignedfinancecontrols = new Control[] { labelFCarrierBalance, labelFReserveBalance, labelFAvailableBalance, labelFReservePercent };
            midtopalignedfinancecontrols = new Control[] { labelFCoreCost, labelFServicesCost };
            righttopalignedfinancecontrols = new Control[] { labelFTaxPioneerSupplies, labelFTaxShipyard, labelFTaxRearm, labelFTaxOutfitting, labelFTaxRefuel, labelFTaxRepair };

            var org = global::EDDiscovery.Icons.Controls.FleetCarrier;
            imageControlOverall.ImageSize = org.Size;     // same size as FC PNG
            imageControlOverall.ImageDepth = 3;         // 0 is most text, 1 is destination, 2 is carrier flashes

            extScrollBarServices.SmallChange = serviceheight / 2;
            imageControlServices.ImageSize = new Size(servicewidth, (serviceheight + linemargin) * EliteDangerousCore.JournalEvents.JournalCarrierCrewServices.GetServiceCount() + linemargin);
            imageControlServices.ImageLayout = ImageLayout.Stretch;     // stretch horizonally/vertically to fix, excepting that a min height is set
            imageControlScrollServices.ImageControlMinimumHeight = imageControlServices.ImageSize.Height;           // setting minimum height mak

            extScrollBarPacks.SmallChange = serviceheight / 2;
            imageControlPacks.ImageLayout = ImageLayout.Stretch;     // stretch horizonally/vertically to fix, excepting that a min height is set

            splitContainerCAPI1.SplitterDistance(GetSetting(dbSplitterSaveCAPI1, 0.5));
            splitContainerCAPI2.SplitterDistance(GetSetting(dbSplitterSaveCAPI2, 0.5));
            splitContainerLedger.SplitterDistance(GetSetting(dbSCLedger, 0.5));

            labelCAPICarrierBalance.Text = labelCAPIDateTime1.Text = labelCAPIDateTime2.Text = labelCAPIDateTime3.Text = "";
            extButtonDoCAPI1.Enabled = extButtonDoCAPI2.Enabled = extButtonDoCAPI3.Enabled = false;     // off until period poll

            var enumlist = new Enum[] {EDTx.UserControlCarrier_extTabControl_tabPageOverall, 
                EDTx.UserControlCarrier_extTabControl_tabPageItinerary, EDTx.UserControlCarrier_extTabControl_tabPageItinerary_colItinDate,
                EDTx.UserControlCarrier_extTabControl_tabPageItinerary_colItinSystemAlphaInt, EDTx.UserControlCarrier_extTabControl_tabPageItinerary_colItinBodyAlphaInt,
                EDTx.UserControlCarrier_extTabControl_tabPageItinerary_colItinJumpDistNumeric, EDTx.UserControlCarrier_extTabControl_tabPageItinerary_colItinDistFromNumeric,
                EDTx.UserControlCarrier_extTabControl_tabPageItinerary_colItinInformation, EDTx.UserControlCarrier_extTabControl_tabPageFinances,
                EDTx.UserControlCarrier_extTabControl_tabPageFinances_colLedgerDate, EDTx.UserControlCarrier_extTabControl_tabPageFinances_colLedgerStarsystemAlphaInt,
                EDTx.UserControlCarrier_extTabControl_tabPageFinances_colLedgerBodyAlphaInt, EDTx.UserControlCarrier_extTabControl_tabPageFinances_colLedgerEvent,
                EDTx.UserControlCarrier_extTabControl_tabPageFinances_colLedgerCreditNumeric, EDTx.UserControlCarrier_extTabControl_tabPageFinances_colLedgerDebitNumeric,
                EDTx.UserControlCarrier_extTabControl_tabPageFinances_colLedgerBalanceNumeric, EDTx.UserControlCarrier_extTabControl_tabPageFinances_colLedgerNotes,
                EDTx.UserControlCarrier_extTabControl_tabPageServices, 
                EDTx.UserControlCarrier_extTabControl_tabPagePacks, 
                EDTx.UserControlCarrier_extTabControl_tabPageOrders, EDTx.UserControlCarrier_extTabControl_tabPageOrders_colOrdersDate,
                EDTx.UserControlCarrier_extTabControl_tabPageOrders_colOrdersCommodity, EDTx.UserControlCarrier_extTabControl_tabPageOrders_colOrdersType,
                EDTx.UserControlCarrier_extTabControl_tabPageOrders_colOrdersGroup, EDTx.UserControlCarrier_extTabControl_tabPageOrders_colOrdersPurchaseNumeric,
                EDTx.UserControlCarrier_extTabControl_tabPageOrders_colOrdersSaleNumeric, EDTx.UserControlCarrier_extTabControl_tabPageOrders_colOrdersPriceNumeric,
                EDTx.UserControlCarrier_extTabControl_tabPageOrders_colOrdersBlackmarket, EDTx.UserControlCarrier_extTabControl_tabPageCAPI3,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI3_dataGridViewTextBoxColumn1,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI3_dataGridViewTextBoxColumnValue, EDTx.UserControlCarrier_extTabControl_tabPageCAPI1,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsName, EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsManu,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsPriceNumeric, EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsSpeedNumeric,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsBoostNumeric, EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsMassNumeric,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIShipsLandingPadNumeric, EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesName,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesCat, EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesMassNumeric,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesPowerNumeric, EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesCostNumeric,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesStockNumeric, EDTx.UserControlCarrier_extTabControl_tabPageCAPI1_colCAPIModulesInfo,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI2, EDTx.UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoCommodity,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoType, EDTx.UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoGroup,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoQuantityNumeric, EDTx.UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoPriceNumeric,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI2_colCAPICargoStolen, EDTx.UserControlCarrier_extTabControl_tabPageCAPI2_colCAPILockerName,
                EDTx.UserControlCarrier_extTabControl_tabPageCAPI2_colCAPILockerType, EDTx.UserControlCarrier_extTabControl_tabPageCAPI2_colCAPILockerQuantityNumeric};

            BaseUtils.Translator.Instance.TranslateControls(this,enumlist);


            extChartLedger.AddChartArea("LedgerCA1");
            extChartLedger.AddSeries("LedgerS1", "LedgerCA1", SeriesChartType.Line);

            extChartLedger.EnableZoomMouseWheelX();
            extChartLedger.ZoomMouseWheelXMinimumInterval = 5.0 / 60.0 / 24.0;

            extChartLedger.SetXAxisInterval(DateTimeIntervalType.Days, 0, IntervalAutoMode.VariableCount);
            extChartLedger.SetXAxisFormat("g");

            extChartLedger.XCursorShown();
            extChartLedger.XCursorSelection();
            extChartLedger.SetXCursorInterval(1, DateTimeIntervalType.Seconds);

            //extChartLedger.SetSeriesXAxisLabelType(ChartValueType.Date);

            extChartLedger.YAutoScale();
            extChartLedger.SetYAxisFormat("N0");
            extChartLedger.IsStartedFromZeroY = false;

            extChartLedger.ShowSeriesMarkers(MarkerStyle.Diamond);

            extChartLedger.AddContextMenu(new string[] { "Zoom out by 1", "Reset Zoom" },
                                new Action<ToolStripMenuItem>[]
                                    { new Action<ToolStripMenuItem>((s)=> {extChartLedger.ZoomOutX(); } ),
                                          new Action<ToolStripMenuItem>((s)=> {extChartLedger.ZoomResetX(); } ),
                                    },
                                new Action<ToolStripMenuItem[]>((list) => {
                                    list[0].Enabled = list[1].Enabled = extChartLedger.IsZoomedX;
                                })
                                );

            extChartLedger.CursorPositionChanged = LedgerCursorPositionChanged;


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
        }

        public override void Init()
        {
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnThemeChanged += ClearDisplayFontJournalCAPI;

            dataGridViewItinerary.Init(DiscoveryForm);      // set up 
            dataGridViewItinerary.WebLookup = EliteDangerousCore.WebExternalDataLookup.All;
        }

        public override void LoadLayout()
        {
            extTabControl.TabStyle = new ExtendedControls.TabStyleSquare();        // after themeing, set to differentiate

            loadlayouthappened = true;
            DGVLoadColumnLayout(dataGridViewItinerary, "Itinerary");
            DGVLoadColumnLayout(dataGridViewLedger, "Ledger");
            DGVLoadColumnLayout(dataGridViewOrders, "Orders");
            DGVLoadColumnLayout(dataGridViewCAPIStats, "CAPIStats");
            DGVLoadColumnLayout(dataGridViewCAPIShips, "CAPIShips");
            DGVLoadColumnLayout(dataGridViewCAPIModules, "CAPIModules");
            DGVLoadColumnLayout(dataGridViewCAPICargo, "CAPICargo");
            DGVLoadColumnLayout(dataGridViewCAPILocker, "CAPILocker");

            int index = GetSetting(dbTabSave, 0);
            if (index >= 0 && index < extTabControl.TabCount)
                extTabControl.SelectedIndex = index;
        }

        public override void InitialDisplay()
        {
            ClearDisplayFontJournalCAPI();     // do the lot
            period.Start();
        }

        public override void Closing()
        {
            period.Stop();

            DGVSaveColumnLayout(dataGridViewItinerary, "Itinerary");
            DGVSaveColumnLayout(dataGridViewLedger, "Ledger");
            DGVSaveColumnLayout(dataGridViewOrders, "Orders");
            DGVSaveColumnLayout(dataGridViewCAPIStats, "CAPIStats");
            DGVSaveColumnLayout(dataGridViewCAPIShips, "CAPIShips");
            DGVSaveColumnLayout(dataGridViewCAPIModules, "CAPIModules");
            DGVSaveColumnLayout(dataGridViewCAPICargo, "CAPICargo");
            DGVSaveColumnLayout(dataGridViewCAPILocker, "CAPILocker");

            PutSetting(dbSplitterSaveCAPI1, splitContainerCAPI1.GetSplitterDistance());
            PutSetting(dbSplitterSaveCAPI2, splitContainerCAPI2.GetSplitterDistance());
            PutSetting(dbSCLedger, splitContainerLedger.GetSplitterDistance());
            PutSetting(dbTabSave, extTabControl.SelectedIndex);


            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnThemeChanged -= ClearDisplayFontJournalCAPI;
            bigfont.Dispose();
            normfont.Dispose();
        }

        private void Discoveryform_OnHistoryChange()     
        {
            ClearDisplayFontJournalCAPI();   // do the lot, including the capi, etc.
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he)     
        {
            if ( he.journalEntry is ICarrierStats)
            {
                DisplayJournal();
            }
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
            var cs = DiscoveryForm.History.Carrier;

            if (cs.CheckCarrierJump(DateTime.UtcNow)) // if autojump happened
                DisplayJournal();      // redisplay all - including destinationsystem
            else
                DisplayDestinationSystem();     // redisplay just the time

            if ( ++periodtickcounter % 4 == 0)     // every N go, change the flashy lights on the image overall
            {
                imageControlOverall.ImageVisible[2] = periodtickcounter % 8 == 0;
                imageControlOverall.Invalidate();       
            }

            // capi enable/disable  - get stats
            DateTime capitime = GetSettingGlobal(dbCAPIDateUTC, DateTime.UtcNow);
            int capicmdrid = GetSettingGlobal(dbCAPICommander, -1);
            bool capisamecmdr = DiscoveryForm.History.CommanderId == capicmdrid;

            // enabled if greater than this time ago or not same commander
            extButtonDoCAPI1.Enabled = extButtonDoCAPI2.Enabled = extButtonDoCAPI3.Enabled = DiscoveryForm.History.IsRealCommanderId && (!capisamecmdr || (DateTime.UtcNow - capitime) >= new TimeSpan(0, 0, 2, 0));

            // if its the same commander, and our display is in the past, another panel fetched it, redisplay
            if (capisamecmdr && capitime > capidisplayedtime)
                DisplayCAPIFromDB();

        }

        private void ClearDisplayFontJournalCAPI()         // the lot
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

            ClearDisplayJournal();

            DisplayCAPIFromDB();
        }

        #endregion

        #region Journal

        private void ClearDisplayJournal()          // the non CAPI bits
        {
            dataGridViewItinerary.Rows.Clear();
            dataGridViewLedger.Rows.Clear();
            extChartLedger.ClearSeriesPoints();
            dataGridViewOrders.Rows.Clear();
            DisplayJournal();
        }

        private async void DisplayJournal()
        {
            var cs = DiscoveryForm.History.Carrier;

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

                ISystem cursys = DiscoveryForm.History.GetLast?.System;     // last system, if present

                for (int i = cs.JumpHistory.Count - 1; i >= 0; i--)
                {
                    var it = cs.JumpHistory[i];

                    if (!it.StarSystem.HasCoordinate)
                    {
                        ISystem p = await EliteDangerousCore.SystemCache.FindSystemAsync(it.StarSystem, EliteDangerousCore.WebExternalDataLookup.All);           // find, even in EDSM, synchronously for now
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
                                        "",
                    };

                    int rwn = dataGridViewItinerary.Rows.Add(rowobj);
                    dataGridViewItinerary.Rows[rwn].Tag = it.StarSystem;
                }

                dataGridViewItinerary.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                dataGridViewItinerary.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            }

            if (dataGridViewLedger.RowCount != cs.Ledger.Count)
            {
                DataGridViewColumn sortcol = dataGridViewLedger.SortedColumn != null ? dataGridViewLedger.SortedColumn : dataGridViewLedger.Columns[0];
                SortOrder sortorder = dataGridViewLedger.SortOrder != SortOrder.None ? dataGridViewLedger.SortOrder : SortOrder.Descending;

                dataGridViewLedger.Rows.Clear();

                extChartLedger.ClearSeriesPoints();

                for (int i = cs.Ledger.Count - 1; i >= 0; i--)
                {
                    var le = cs.Ledger[i];
                    long diff = i > 0 ? (le.Balance - cs.Ledger[i - 1].Balance) : 0;        // difference between us and previous, + if credit, - if not

                    DateTime seltime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(le.JournalEntry.EventTimeUTC);

                    object[] rowobj = { seltime.ToString(),
                                        le.StarSystem.Name,
                                        le.Body,
                                        le.JournalEntry.EventTypeStr.SplitCapsWordFull(),
                                        diff>0 ? diff.ToString("N0") : "",
                                        diff<0 ? (-diff).ToString("N0") : "",
                                        le.Balance.ToString("N0"),
                                        le.Notes
                    };

                    var row = dataGridViewLedger.Rows.Add(rowobj);
                    dataGridViewLedger.Rows[row].Tag = seltime;
                    extChartLedger.AddXY(seltime, le.Balance,graphtooltip:$"{seltime.ToString()} {le.Balance:N0}cr");
                    //System.Diagnostics.Debug.WriteLine($"Add {seltime} {le.Balance}");
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
                    {
                        name += " (" + "Decommissioned".TxID(EDTx.UserControlCarrier_Decommissioned) + ")";
                    }
                    else if (cs.IsDecommisioning)
                    {
                        name += " (" + "Decommissioning on".TxID(EDTx.UserControlCarrier_DecommissionedOn) + " " + EDDConfig.Instance.ConvertTimeToSelectedFromUTC(cs.DecommisionTimeUTC.Value) + ")";
                    }

                    imageControlOverall.DrawText(new Point(hspacing, vposl), new Size(30000, 30000), name, bigfont, color);
                    vposl += bigfont.Height + linemargin;

                    imageControlOverall.DrawText(new Point(hspacing, vposl), new Size(30000, 30000), cs.CurrentPositionText, bigfont, color);
                    vposl += bigfont.Height + linemargin;

                    imageControlOverall.DrawText(new Point(rightpos, vposr), new Size(30000, 30000), (cs.State.Finance.AvailableBalance * 1).ToString("N0") + "cr",
                                                bigfont, color);
                    vposr += bigfont.Height + linemargin;

                    string[] text = new string[] {
                        BaseUtils.FieldBuilder.Build("Cargo: ; t;N0".TxID(EDTx.UserControlCarrier_Cargo), cs.State.SpaceUsage.Cargo),
                        BaseUtils.FieldBuilder.Build("Services: ; t;N0".TxID(EDTx.UserControlCarrier_Services), cs.State.SpaceUsage.Crew),
                        BaseUtils.FieldBuilder.Build("Ship Packs: ; t;N0".TxID(EDTx.UserControlCarrier_Shippacks), cs.State.SpaceUsage.ShipPacks),
                        BaseUtils.FieldBuilder.Build("Module Packs: ; t;N0".TxID(EDTx.UserControlCarrier_Modulepacks), cs.State.SpaceUsage.ModulePacks),
                        BaseUtils.FieldBuilder.Build("Reserved Space: ; t;N0".TxID(EDTx.UserControlCarrier_Reservedspace), cs.State.SpaceUsage.CargoSpaceReserved),
                        BaseUtils.FieldBuilder.Build("Free Space: ; t;N0".TxID(EDTx.UserControlCarrier_Freespace), cs.State.SpaceUsage.FreeSpace),
                        BaseUtils.FieldBuilder.Build("Jump Range: ; ly;N0".TxID(EDTx.UserControlCarrier_Jumprange), cs.State.JumpRangeCurr),
                        BaseUtils.FieldBuilder.Build("Max Jump: ; ly;N0".TxID(EDTx.UserControlCarrier_Maxjump), cs.State.JumpRangeMax),
                        BaseUtils.FieldBuilder.Build("Fuel: ; t;N0".TxID(EDTx.UserControlCarrier_Fuel), cs.State.FuelLevel),
                        BaseUtils.FieldBuilder.Build("Docking Access: ".TxID(EDTx.UserControlCarrier_DockingAccess), cs.State.DockingAccessSplittable.SplitCapsWordFull()),
                        BaseUtils.FieldBuilder.Build("Notorious: ".TxID(EDTx.UserControlCarrier_Notorious), (cs.State.AllowNotorious ? "Yes".TxID(EDTx.MessageBoxTheme_Yes) : "No".TxID(EDTx.MessageBoxTheme_No))),
                    };

                    imageControlOverall.DrawText(new Point(rightpos, vposr + linemargin), new Size(30000, 30000), text, normfont, linemargin, color);

                    labelFCarrierBalance.Text = BaseUtils.FieldBuilder.Build("Balance: ; cr;N0".TxID(EDTx.UserControlCarrier_Balance), cs.State.Finance.CarrierBalance);
                    labelFReserveBalance.Text = BaseUtils.FieldBuilder.Build("Reserve: ; cr;N0".TxID(EDTx.UserControlCarrier_Reserve), cs.State.Finance.ReserveBalance);
                    labelFAvailableBalance.Text = BaseUtils.FieldBuilder.Build("Available: ; cr;N0".TxID(EDTx.UserControlCarrier_Available), cs.State.Finance.AvailableBalance);
                    labelFReservePercent.Text = BaseUtils.FieldBuilder.Build("Reserve: ; %;N0".TxID(EDTx.UserControlCarrier_Reservepercent), cs.State.Finance.ReservePercent);

                    TaxRate(labelFTaxPioneerSupplies, "Pioneer Tax".TxID(EDTx.UserControlCarrier_PioneerTax), cs.State.Finance.TaxRatePioneersupplies);
                    TaxRate(labelFTaxShipyard, "Shipyard Tax".TxID(EDTx.UserControlCarrier_ShipyardTax), cs.State.Finance.TaxRateShipyard);
                    TaxRate(labelFTaxRearm, "Rearm Tax".TxID(EDTx.UserControlCarrier_RearmTax), cs.State.Finance.TaxRateRearm);
                    TaxRate(labelFTaxOutfitting, "Outfitting Tax".TxID(EDTx.UserControlCarrier_OutfittingTax), cs.State.Finance.TaxRateOutfitting);
                    TaxRate(labelFTaxRefuel, "Refuel Tax".TxID(EDTx.UserControlCarrier_RefuelTax), cs.State.Finance.TaxRateRefuel);
                    TaxRate(labelFTaxRepair, "Repair Tax".TxID(EDTx.UserControlCarrier_RepairTax), cs.State.Finance.TaxRateRepair);

                    labelFCoreCost.Text = BaseUtils.FieldBuilder.Build("Core Cost: ; cr;N0".TxID(EDTx.UserControlCarrier_CoreCost), cs.State.GetCoreCost());
                    labelFServicesCost.Text = BaseUtils.FieldBuilder.Build("Services Cost: ; cr;N0".TxID(EDTx.UserControlCarrier_ServicesCost), cs.State.GetServicesCost());

                    string[] itinerylines = new string[] { cs.LastJumpText(1), cs.LastJumpText(2), cs.LastJumpText(3), cs.LastJumpText(4), cs.LastJumpText(5), cs.LastJumpText(6) };
                    int lines = itinerylines.Where(x => x != null).Count();
                    imageControlOverall.DrawText(new Point(hspacing, imageControlOverall.ImageHeight - linemargin - lines * (normfont.Height + linemargin)),
                                        new Size(30000, 30000), itinerylines, normfont, linemargin, color);

                }
                else
                {
                    imageControlOverall.DrawText(new Point(hspacing, vposl), new Size(30000, 30000), "No Carrier".TxID(EDTx.UserControlCarrier_NoCarrier), bigfont, color);
                }

                DisplayDestinationSystem();

                foreach (var x in righttopalignedfinancecontrols)
                    x.Visible = cs.State.HaveCarrier;
                foreach (var x in midtopalignedfinancecontrols)
                    x.Visible = cs.State.HaveCarrier;
                foreach (var x in lefttopalignedfinancecontrols)
                    x.Visible = cs.State.HaveCarrier;
            }

            imageControlOverall.Invalidate();       // force overall image stack 

            {
                imageControlServices.Clear();

                if (cs.State.HaveCarrier)
                {

                    Graphics gr = imageControlServices.GetGraphics();
                    int vpos = linemargin;

                    foreach (JournalCarrierCrewServices.ServiceType srvtype in Enum.GetValues(typeof(JournalCarrierCrewServices.ServiceType)))
                    {
                        if (JournalCarrierCrewServices.IsValidService(srvtype))
                        {
                            var servicestate = cs.State.GetService(srvtype);     // may be null for a core or non listed service
                            var optional = JournalCarrierCrewServices.IsOptionalService(srvtype);
                            bool active = !optional || (servicestate != null && servicestate.Enabled == true && servicestate.Activated == true);
                            bool disabled = servicestate != null && servicestate.Enabled == false && servicestate.Activated == true;

                            var area = new Rectangle(hspacing, vpos, servicewidth, serviceheight);

                            using (Brush br = new SolidBrush(Color.FromArgb(255, 53, 36, 2).Multiply(active ? 1f : 0.75f)))
                            {
                                gr.FillRectangle(br, area);
                            }

                            var pointtextleft = new Point(hspacing * 3, vpos + linemargin * 2);

                            const int titlewidth = 200;

                            var size = imageControlServices.DrawMeasureText(pointtextleft, new Size(titlewidth, 1000), JournalCarrierCrewServices.GetTranslatedServiceName(srvtype), bigfont, color);
                            pointtextleft.Y += (int)(size.Height + 1);
                            string coreoroptional = srvtype <= EliteDangerousCore.JournalEvents.JournalCarrierCrewServices.ServiceType.TritiumDepot ? "Core Service".TxID(EDTx.UserControlCarrier_CoreService) : "Optional Service".TxID(EDTx.UserControlCarrier_OptionalService);
                            imageControlServices.DrawText(pointtextleft, new Size(titlewidth, 1000), coreoroptional, normfont, color);

                            Image img = BaseUtils.Icons.IconSet.Instance.Get("Controls." + srvtype.ToString());
                            imageControlServices.DrawImage(img, new Rectangle(hspacing * 4, vpos + serviceheight - linemargin - img.Height, img.Width, img.Height));

                            var servicecol1top = new Point(titlewidth + 50, vpos + linemargin * 2);

                            // lookup fixed information on service type
                            JournalCarrierCrewServices.ServicesData si = JournalCarrierCrewServices.GetDataOnServiceType(srvtype);
                            if (si != null)
                            {
                                int lineh = normfont.Height + linemargin * 2;
                                imageControlServices.DrawText(new Point(servicecol1top.X, servicecol1top.Y + lineh), new Size(2000, 2000), BaseUtils.FieldBuilder.Build("Install cost: ; cr;N0".TxID(EDTx.UserControlCarrier_InstallCost), si.InstallCost), normfont, color);
                                imageControlServices.DrawText(new Point(servicecol1top.X, servicecol1top.Y + lineh * 2), new Size(2000, 2000), BaseUtils.FieldBuilder.Build("Capacity Allocated: ; Units;N0".TxID(EDTx.UserControlCarrier_CapacityAllocated), si.CargoSize), normfont, color);

                                imageControlServices.DrawText(new Point(servicecol1top.X + 400, servicecol1top.Y + lineh), new Size(2000, 2000), BaseUtils.FieldBuilder.Build("Upkeep cost: ; cr;N0".TxID(EDTx.UserControlCarrier_Upkeepcost), si.UpkeepCost), normfont, color);
                                imageControlServices.DrawText(new Point(servicecol1top.X + 400, servicecol1top.Y + lineh * 2), new Size(2000, 2000), BaseUtils.FieldBuilder.Build("Suspended upkeep cost: ; cr;N0".TxID(EDTx.UserControlCarrier_Suspendedupkeepcost), si.SuspendedUpkeepCost), normfont, color);

                                // crewname, if either no service state or name is null, ??
                                string crewname = servicestate?.CrewName ?? "??";

                                imageControlServices.DrawText(new Point(servicecol1top.X + 800, servicecol1top.Y + lineh), new Size(2000, 2000), "Crew Name: ".TxID(EDTx.UserControlCarrier_CrewName) + crewname, normfont, color);
                            }

                            if (active)
                            {
                                imageControlServices.DrawText(servicecol1top, new Size(2000, 2000), "This Service is Active".TxID(EDTx.UserControlCarrier_ServiceActive), normfont, color);
                                Image img2 = BaseUtils.Icons.IconSet.Instance.Get("Controls.ServicesTick");
                                imageControlServices.DrawImage(img2, new Rectangle(titlewidth, vpos, img.Width, img.Height));
                            }
                            else if (disabled)
                            {
                                imageControlServices.DrawText(servicecol1top, new Size(2000, 2000), "This Service is Suspended".TxID(EDTx.UserControlCarrier_ServiceSuspended), normfont, color);
                                using (Brush br2 = new SolidBrush(Color.FromArgb(40, 255, 255, 0)))
                                {
                                    gr.FillRectangle(br2, area);
                                }
                            }
                            else
                            {
                                imageControlServices.DrawText(servicecol1top, new Size(2000, 2000), "This Service is Not Installed".TxID(EDTx.UserControlCarrier_ServiceNotInstalled), normfont, color);
                                using (Brush br2 = new SolidBrush(Color.FromArgb(40, 255, 0, 0)))
                                {
                                    gr.FillRectangle(br2, area);
                                }
                            }


                            vpos += serviceheight + linemargin;
                        }
                    }

                    gr.Dispose();
                }

                imageControlServices.Invalidate();
            }

            {
                int no = cs.State.ModulePacksCount() + cs.State.ShipPacksCount();
                int packhpixels = (serviceheight + linemargin) * no + linemargin;
                // we need to give it a fair number of pixels, otherwise the stretch is too big. So use the image control to min size it
                imageControlPacks.ImageSize = new Size(servicewidth, Math.Max(packhpixels, imageControlOverall.ImageSize.Height));
                imageControlScrollPacks.ImageControlMinimumHeight = imageControlPacks.ImageSize.Height;           // setting minimum height mak
                imageControlPacks.Clear();

                if (cs.State.HaveCarrier)
                {
                    Graphics gr = imageControlPacks.GetGraphics();
                    int vpos = linemargin;

                    if (no == 0)
                    {
                        imageControlPacks.DrawText(new Point(hspacing, linemargin), new Size(2000, 2000), "No Module or Ship packs installed".TxID(EDTx.UserControlCarrier_NoPacks), bigfont, color);
                    }
                    else
                    {
                        foreach (CarrierState.PackClass sp in cs.State.ShipPacks.EmptyIfNull())
                        {
                            vpos += DisplayPackItem(gr, sp, false, vpos, color);
                        }
                        foreach (CarrierState.PackClass mp in cs.State.ModulePacks.EmptyIfNull())
                        {
                            vpos += DisplayPackItem(gr, mp, true, vpos, color);
                        }
                    }

                    gr.Dispose();
                }
                imageControlPacks.Invalidate();
            }

            if (dataGridViewOrders.RowCount != cs.TradeOrders.Count)
            {
                DataGridViewColumn sortcol = dataGridViewOrders.SortedColumn != null ? dataGridViewOrders.SortedColumn : dataGridViewOrders.Columns[0];
                SortOrder sortorder = dataGridViewOrders.SortOrder != SortOrder.None ? dataGridViewOrders.SortOrder : SortOrder.Descending;

                dataGridViewOrders.Rows.Clear();

                for (int i = 0; i < cs.TradeOrders.Count; i++)
                {
                    var ord = cs.TradeOrders[i];

                    string mname = ord.Commodity_Localised ?? ord.Commodity;
                    string cat = "";
                    string type = "";

                    MaterialCommodityMicroResourceType ty = MaterialCommodityMicroResourceType.GetByFDName(ord.Commodity);

                    if (ty != null)        // if we have found it, use the translated names and cat etc from
                    {
                        mname = ty.TranslatedName;
                        cat = ty.TranslatedCategory;
                        type = ty.TranslatedType;
                        if (ty.IsMicroResources)
                            type = "Micro Resources".TxID(EDTx.FilterSelector_MicroResources);
                    }

                    object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ord.Placed),
                                        mname,
                                        cat,
                                        type,
                                        ord.PurchaseOrder.HasValue ? ord.PurchaseOrder.Value.ToString("N0") : "",
                                        ord.SaleOrder.HasValue ? ord.SaleOrder.Value.ToString("N0") : "",
                                        ord.Price.ToString("N0"),
                                        ord.BlackMarket ? "\u2713" : "",
                    };

                    dataGridViewOrders.Rows.Add(rowobj);
                }

                dataGridViewOrders.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                dataGridViewOrders.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
            }

            PositionControls();
        }

        private int DisplayPackItem(Graphics gr, CarrierState.PackClass sp, bool module, int vpos, Color color)
        {
            const int titlewidth = 600;

            var area = new Rectangle(hspacing, vpos, servicewidth, serviceheight);

            using (Brush br = new SolidBrush(Color.FromArgb(255, 53, 36, 2)))
            {
                gr.FillRectangle(br, area);
            }

            var pointtextleft = new Point(hspacing * 3, vpos + linemargin * 2);

            var size = imageControlPacks.DrawMeasureText(pointtextleft, new Size(titlewidth, 1000), sp.PackTheme.SplitCapsWordFull(), bigfont, color);
            pointtextleft.Y += (int)(size.Height + 1) + linemargin;
            imageControlPacks.DrawText(pointtextleft, new Size(titlewidth, 2000), "Tier ".TxID(EDTx.UserControlCarrier_Tier) + sp.PackTier.ToString("N0"), bigfont, color);


            var pointtextmid = new Point(titlewidth + 50, pointtextleft.Y);

            if (DiscoveryForm.History.Carrier.PackCost.TryGetValue(CarrierStats.PackCostKey(sp), out long value))
            {
                imageControlPacks.DrawText(pointtextmid, new Size(titlewidth, 2000), BaseUtils.FieldBuilder.Build("Cost: ; cr;N0".TxID(EDTx.UserControlCarrier_Cost), value), normfont, color);
            }

            Image img = BaseUtils.Icons.IconSet.Instance.Get(module ? "Controls.ModulePack" : "Controls.Shipyard");
            imageControlPacks.DrawImage(img, new Rectangle(hspacing * 4, vpos + serviceheight - linemargin - img.Height, img.Width, img.Height));

            return serviceheight + linemargin;
        }

        private void DisplayDestinationSystem()
        {
            var cs = DiscoveryForm.History.Carrier;

            if (cs.State.HaveCarrier && cs.IsJumping)
            {
                imageControlOverall.Clear(1);       // clear destination bitmap plane - plane 1

                int vpos = bigfont.Height + linemargin * 2;
                int hpos = hspacing + (int)BaseUtils.BitMapHelpers.MeasureStringInBitmap(cs.CurrentPositionText, bigfont).Width + hspacing;

                var timetogo = cs.TimeTillJump;
                string jumptext = cs.NextDestinationText + " ";
                if (timetogo.TotalSeconds > 0)
                {
                    var mins = timetogo.TotalMinutes;       // a double
                    var second = ((int)(mins * 60)) % 60;
                    jumptext += ((int)mins).ToString() + ":" + second.ToString("00");
                }
                else
                    jumptext += "Jumping".TxID(EDTx.UserControlCarrier_Jumping);

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
                l.Text = t + BaseUtils.FieldBuilder.Build(": ; %;N0".TxID(EDTx.UserControlCarrier_Taxpercentage), value.Value);
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
            int maxmidwidth = midpos;
            foreach (var x in midtopalignedfinancecontrols)
            {
                x.Location = new Point(midpos, lmpos);
                maxmidwidth = Math.Max(x.Right, maxmidwidth);
                lmpos += x.Height + linemargin;
            }

            int maxh = Math.Max(lypos, lmpos);

            int rightpos = Width- 270;
            int xpos = rightpos;
            int rypos = linemargin;
            int shown = 0;
            foreach (var x in righttopalignedfinancecontrols)
            {
                if (x.Text.HasChars())
                {
                    x.Location = new Point(xpos, rypos);
                    rypos += x.Height + linemargin;
                    maxh = Math.Max(maxh, rypos);

                    if (++shown == 4 && xpos - 200 > maxmidwidth)
                    {
                        rypos = linemargin;
                        xpos -= 200;
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine($"Max h {maxh}");
            labelCAPICarrierBalance.Location = new Point(rightpos, labelCAPICarrierBalance.Top);
            panelFinancesTop.Height = maxh;
        }

        private void dataGridViewLedger_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewLedger.RowCount)
            {
                var row = dataGridViewLedger.Rows[e.RowIndex];
                var datetime = (DateTime)row.Tag;
                //System.Diagnostics.Debug.WriteLine($"Stats Selected Graph cursor position {datetime}");
                extChartLedger.SetXCursorPosition(datetime);
            }
        }
        private void LedgerCursorPositionChanged(ExtendedControls.ExtSafeChart chart, string chartarea, AxisName axis, double pos)
        {
            if (!double.IsNaN(pos))     // this means its off graph, ignore
            {
                DateTime dtgraph = DateTime.FromOADate(pos);                    // back to date/time
                int row = dataGridViewLedger.FindRowWithDateTagWithin((r) => (DateTime)r.Tag, dtgraph, long.MaxValue);  // Find nearest row whatever
                if (row >= 0)
                {
                    dataGridViewLedger.SetCurrentAndSelectAllCellsOnRow(row);
                    dataGridViewLedger.Rows[row].Selected = true;
                }
            }
        }

        #endregion

        #region CAPI

        private void extButtonDoCAPI1_Click(object sender, EventArgs e)
        {
            if (DiscoveryForm.FrontierCAPI.Active)
            {
                extButtonDoCAPI1.Enabled = extButtonDoCAPI2.Enabled = extButtonDoCAPI3.Enabled = false;

                // record when and who did capi, and clear data.  

                PutSettingGlobal(dbCAPIDateUTC, DateTime.UtcNow);
                PutSettingGlobal(dbCAPICommander, DiscoveryForm.History.CommanderId);
                PutSettingGlobal(dbCAPISave, "");

                // don't hold up the main thread, do it in a task, as its a HTTP operation

                System.Threading.Tasks.Task.Run(() =>
                {
                    CAPI.FleetCarrier fc = null;
                    bool nocarrier = false;

                    int tries = 3;
                    while (tries-- > 0)        // goes at getting the valid data from frontier
                    {
                        string fleetcarrier = DiscoveryForm.FrontierCAPI.FleetCarrier(out DateTime _, nocontentreturnemptystring:true);

                        if (fleetcarrier != null)
                        {
                            if (fleetcarrier.Length == 0)       // an empty string means no carrier
                            {
                                nocarrier = true;
                                break;
                            }
                            else
                            {
                                fc = new CAPI.FleetCarrier(fleetcarrier);

                                if (fc.IsValid)
                                {
                                    //BaseUtils.FileHelpers.TryWriteToFile(@"c:\code\fc.json", fc.Json.ToString(true));
                                    System.Diagnostics.Debug.WriteLine($"Got CAPI fleetcarrier try {3 - tries} {fleetcarrier}");
                                    PutSettingGlobal(dbCAPISave, fleetcarrier);       // store data into global capi slot
                                    break;
                                }
                                else
                                    fc = null;
                            }
                        }
                        else
                            System.Threading.Thread.Sleep(2000);        // pause before another try
                    }

                    this.Invoke(new Action(() =>
                    {
                        System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);

                        DisplayCAPI(fc);        // may be null, this will clear it if it is

                        if ( nocarrier )
                        {
                            ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No Carrier".TxID(EDTx.UserControlCarrier_NoCarrier),
                                "Warning".TxID(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else if ( fc == null  )
                        {
                            ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "No CAPI data from frontier, due to either or server/network failure".TxID(EDTx.UserControlCarrier_NetworkFailure),
                                "Warning".TxID(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }

                        System.Diagnostics.Debug.WriteLine("Capi function complete");
                    }));
                });
            }
            else
            {
                ExtendedControls.MessageBoxTheme.Show(this.FindForm(),
                    "You are not logged into CAPI.\r\nGo to the settings page and select your commanders settings, and log into CAPI.".TxID(EDTx.UserControlCarrier_NotLoggedIn),
                    "Warning".TxID(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DisplayCAPIFromDB()
        {
            string capi = GetSettingGlobal(dbCAPISave, "");
            int capicmd = GetSettingGlobal(dbCAPICommander, -1);

            // if its a valid capi for commander, turn it into a FC entity
            var fc = (capi.Length > 0 && capicmd == DiscoveryForm.History.CommanderId) ? new CAPI.FleetCarrier(capi) : null;        

            DisplayCAPI(fc);
        }

        private DateTime capidisplayedtime;

       
        // fc = null means invalid capi data, clear display
        private void DisplayCAPI(CAPI.FleetCarrier fc)
        {
            System.Diagnostics.Debug.Assert(System.Windows.Forms.Application.MessageLoop);

            if (fc == null || fc.IsValid == false)      // if not valid
            {
                labelCAPIDateTime1.Text = labelCAPIDateTime2.Text = labelCAPIDateTime3.Text = "";
                dataGridViewCAPICargo.Rows.Clear();
                dataGridViewCAPIShips.Rows.Clear();
                dataGridViewCAPIModules.Rows.Clear();
                dataGridViewCAPIStats.Rows.Clear();
            }
            else
            {
                DateTime capitime = GetSettingGlobal(dbCAPIDateUTC, DateTime.UtcNow);
                labelCAPIDateTime1.Text = labelCAPIDateTime2.Text = labelCAPIDateTime3.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(capitime).ToString();
                capidisplayedtime = capitime;

                labelCAPICarrierBalance.Text = fc.Balance.ToString("N0") + "cr";

                {
                    DataGridViewColumn sortcol = dataGridViewCAPICargo.SortedColumn != null ? dataGridViewCAPICargo.SortedColumn : dataGridViewCAPICargo.Columns[0];
                    SortOrder sortorder = dataGridViewCAPICargo.SortOrder != SortOrder.None ? dataGridViewCAPICargo.SortOrder : SortOrder.Ascending;
                    dataGridViewCAPICargo.Rows.Clear();

                    List<CAPI.FleetCarrier.Cargo> cargounmerged = fc.GetCargo();
                    //cargounmerged.Add(new CAPI.FleetCarrier.Cargo() { Commodity = "Polymers", Value = 100, Quantity = 100 });     // for debugging the merge
                    //cargounmerged.Add(new CAPI.FleetCarrier.Cargo() { Commodity = "Polymers", Value = 100, Quantity = 200 });
                    //cargounmerged.Add(new CAPI.FleetCarrier.Cargo() { Commodity = "Polymers", Value = 100, Quantity = 300 });
                    //cargounmerged.Add(new CAPI.FleetCarrier.Cargo() { Commodity = "Pesticides", Value = 656, Quantity = 300 });

                    if (cargounmerged != null)
                    {
                        var cargo = CAPI.FleetCarrier.MergeCargo(cargounmerged);        // merge similar entries

                        for (int i = 0; i < cargo.Count; i++)
                        {
                            var ord = cargo[i];

                            string mname = ord.LocName ?? ord.Commodity;
                            string cat = "";
                            string type = "";

                            MaterialCommodityMicroResourceType ty = MaterialCommodityMicroResourceType.GetByFDName(ord.Commodity);

                            if (ty != null)        // if we have found it, use the translated names and cat etc from
                            {
                                mname = ty.TranslatedName;
                                cat = ty.TranslatedCategory;
                                type = ty.TranslatedType;
                            }

                            object[] rowobj = {
                                                mname,
                                                cat,
                                                type,
                                                ord.Quantity.ToString("N0"),
                                                ord.Value.ToString("N0"),
                                                ord.Stolen? "\u2713" : "",
                                        };

                            dataGridViewCAPICargo.Rows.Add(rowobj);
                        }
                    }
                }

                {
                    List<CAPI.FleetCarrier.LockerItem> locker = fc.GetCarrierLockerAll();       // always get an list, even if all empty

                    DataGridViewColumn sortcol = dataGridViewCAPILocker.SortedColumn != null ? dataGridViewCAPILocker.SortedColumn : dataGridViewCAPILocker.Columns[0];
                    SortOrder sortorder = dataGridViewCAPILocker.SortOrder != SortOrder.None ? dataGridViewCAPILocker.SortOrder : SortOrder.Ascending;

                    dataGridViewCAPILocker.Rows.Clear();

                    for (int i = 0; i < locker.Count; i++)
                    {
                        var ord = locker[i];

                        string mname = ord.LocName ?? ord.Name;
                        string cat = ord.Category.ToString();

                        MaterialCommodityMicroResourceType ty = MaterialCommodityMicroResourceType.GetByFDName(ord.Name);

                        if (ty != null)        // if we have found it, use the translated names and cat etc from
                        {
                            mname = ty.TranslatedName;
                            cat = ty.TranslatedCategory;
                        }

                        object[] rowobj = {
                                        mname,
                                        cat,
                                        ord.Quantity.ToString("N0"),
                                    };

                        dataGridViewCAPILocker.Rows.Add(rowobj);
                    }


                    dataGridViewCAPILocker.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                    dataGridViewCAPILocker.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
                }

                {
                    List<CAPI.FleetCarrier.Ship> ships = fc.GetShips();

                    DataGridViewColumn sortcol = dataGridViewCAPIShips.SortedColumn != null ? dataGridViewCAPIShips.SortedColumn : dataGridViewCAPIShips.Columns[0];
                    SortOrder sortorder = dataGridViewCAPIShips.SortOrder != SortOrder.None ? dataGridViewCAPIShips.SortOrder : SortOrder.Ascending;

                    dataGridViewCAPIShips.Rows.Clear();

                    if (ships != null)
                    {
                        for (int i = 0; i < ships.Count; i++)
                        {
                            var ord = ships[i];
                            ItemData.ShipProperties ship = ItemData.GetShipProperties(ord.Name);

                            string name = ship?.Name ?? ord.Name.SplitCapsWordFull();
                            string manu = ship?.Manufacturer ?? "";
                            string speed = ship?.Speed.ToString("N0") ?? "";
                            string boost = ship?.Boost.ToString("N0") ?? "";
                            string mass = ship?.HullMass.ToString("N0") ?? "";
                            string classv = ship?.ClassString ?? ""; 

                            object[] rowobj = {
                                            name,
                                            manu,
                                            ord.BaseValue.ToString("N0"),
                                            speed,
                                            boost,
                                            mass,
                                            classv,
                                        };

                            dataGridViewCAPIShips.Rows.Add(rowobj);
                        }
                    }

                    dataGridViewCAPIShips.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                    dataGridViewCAPIShips.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
                }

                {
                    List<CAPI.FleetCarrier.Module> modules = fc.GetModules();

                    DataGridViewColumn sortcol = dataGridViewCAPIModules.SortedColumn != null ? dataGridViewCAPIModules.SortedColumn : dataGridViewCAPIModules.Columns[0];
                    SortOrder sortorder = dataGridViewCAPIModules.SortOrder != SortOrder.None ? dataGridViewCAPIModules.SortOrder : SortOrder.Ascending;

                    dataGridViewCAPIModules.Rows.Clear();

                    if (modules != null)
                    {
                        for (int i = 0; i < modules.Count; i++)
                        {
                            var ord = modules[i];
                            if (ItemData.TryGetShipModule(ord.Name, out ItemData.ShipModule modp, false))       // find, no create
                            {
                                string name = modp?.TranslatedModName ?? ord.Name.SplitCapsWordFull();
                                string mtype = modp?.TranslatedModTypeString ?? ord.Category ?? "";
                                string mass = modp?.Mass?.ToString("N1") ?? "";
                                string power = modp?.PowerDraw?.ToString("N1") ?? "";
                                string info = modp.ToString();

                                object[] rowobj = {
                                                name,
                                                mtype,
                                                mass,
                                                power,
                                                ord.Cost.ToString("N0"),
                                                ord.Stock.ToString("N0"),
                                                info,
                                            };

                                dataGridViewCAPIModules.Rows.Add(rowobj);
                            }
                            else
                            {
                                object[] rowobj = {
                                                ord.Name,
                                                ord.Category,
                                                0,
                                                0,
                                                ord.Cost.ToString("N0"),
                                                ord.Stock.ToString("N0"),
                                                "Not found",
                                            };

                                dataGridViewCAPIModules.Rows.Add(rowobj);
                            }
                        }
                    }

                    dataGridViewCAPIModules.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                    dataGridViewCAPIModules.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
                }

                {
                    DataGridViewColumn sortcol = dataGridViewCAPIStats.SortedColumn != null ? dataGridViewCAPIStats.SortedColumn : dataGridViewCAPIStats.Columns[0];
                    SortOrder sortorder = dataGridViewCAPIStats.SortOrder != SortOrder.None ? dataGridViewCAPIStats.SortOrder : SortOrder.Ascending;

                    dataGridViewCAPIStats.Rows.Clear();

                    Type jtype = fc.GetType();

                    foreach (System.Reflection.PropertyInfo pi in jtype.GetProperties())
                    {
                        System.Reflection.MethodInfo getter = pi.GetGetMethod();
                        dynamic value = getter.Invoke(fc, null);

                        string res = value is string ? (string)value :
                                        value is int ? ((int)value).ToString("N0") :
                                        value is long ? ((long)value).ToString("N0") :
                                        value is double ? ((double)value).ToString("") :
                                        value is bool ? (((bool)value) ? "True" : "False") : null;
                        if (res != null)
                        {
                            object[] rowobj = {
                                            pi.Name.SplitCapsWordFull(),
                                            res,
                                        };

                            dataGridViewCAPIStats.Rows.Add(rowobj);
                        }
                    }

                    dataGridViewCAPIStats.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                    dataGridViewCAPIStats.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
                }
            }
        }

        #endregion

        

    }
}
