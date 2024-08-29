/*
 * Copyright © 2020-2024 EDDiscovery development team
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
    public partial class UserControlFactions : UserControlCommonBase
    {
        private DateTime NextExpiryUTC;
        private HistoryEntry last_he = null;

        #region Init

        public UserControlFactions()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "FactionsPanel";

            dataGridView.MakeDoubleBuffered();

            for (int col = 1; col < dataGridView.ColumnCount - 1; col++)
                dataGridView.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            startDateTimePicker.Value = GetSetting("StartDate", DateTime.UtcNow).StartOfDay();
            startDateTimePicker.Checked = GetSetting("StartDateChecked", false);
            endDateTimePicker.Value = GetSetting("EndDate", DateTime.UtcNow.EndOfDay()).EndOfDay();
            endDateTimePicker.Checked = GetSetting("EndDateChecked", false);
            VerifyDates();
            startDateTimePicker.ValueChanged += new System.EventHandler(this.startDateTime_ValueChanged);        // now install the change handlers
            endDateTimePicker.ValueChanged += new System.EventHandler(this.endDateTime_ValueChanged);

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;

            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            var enumlist = new Enum[] { 
                        EDTx.UserControlFactions_colFaction, EDTx.UserControlFactions_colMissions, EDTx.UserControlFactions_colInfluence, 
                        EDTx.UserControlFactions_colReputation, EDTx.UserControlFactions_colMissionCredits, EDTx.UserControlFactions_CBought, 
                        EDTx.UserControlFactions_CSold, EDTx.UserControlFactions_CProfit, EDTx.UserControlFactions_MBought, EDTx.UserControlFactions_MSold, 
                        EDTx.UserControlFactions_CrimeCommitted, EDTx.UserControlFactions_BountyKills, EDTx.UserControlFactions_BountyValue, 
                        EDTx.UserControlFactions_BountyRewardsValue, EDTx.UserControlFactions_Interdicted, EDTx.UserControlFactions_Interdiction, 
                        EDTx.UserControlFactions_KillBondVictim, EDTx.UserControlFactions_KillBondsAward, EDTx.UserControlFactions_KillBondsValue, 
                        EDTx.UserControlFactions_colInfo, EDTx.UserControlFactions_labelTo, EDTx.UserControlFactions_CartoValue,
                        EDTx.UserControlFactions_colRedeemVoucher, EDTx.UserControlFactions_colFines, EDTx.UserControlFactions_colBountyValue,
                        EDTx.UserControlFactions_colDataLinkVictimFaction, EDTx.UserControlFactions_colDataLinkPayeeFaction, EDTx.UserControlFactions_colDataLinkPayeeValue
                        };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            var enumlistcms = new Enum[] { EDTx.UserControlFactions_showMissionsForFactionToolStripMenuItem, EDTx.UserControlFactions_showCommoditymaterialTradesForFactionToolStripMenuItem,
                        EDTx.UserControlFactions_showBountiesAndBondsForFactionToolStripMenuItem, EDTx.UserControlFactions_showFactionSystemDetailToolStripMenuItem
                };

            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);

            var enumlisttt = new Enum[] { EDTx.UserControlFactions_startDateTimePicker_ToolTip, EDTx.UserControlFactions_endDateTimePicker_ToolTip, EDTx.UserControlFactions_buttonExtExcel_ToolTip
                    };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            labelInfo.Text = "";
        }


        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
        }

        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;

            PutSetting("StartDate", startDateTimePicker.Value);                 // picker value is stored..
            PutSetting("StartDateChecked", startDateTimePicker.Checked);
            PutSetting("EndDate", endDateTimePicker.Value);
            PutSetting("EndDateChecked", endDateTimePicker.Checked);
        }

        private void Discoveryform_OnHistoryChange()     // may have changed date system, this causes this
        {
            VerifyDates();
            Display();
        }

        // follow travel history cursor, he is never null
        public override void ReceiveHistoryEntry(HistoryEntry he)
        {
            if (he.MissionList != last_he?.MissionList || he.EventTimeUTC > NextExpiryUTC)      // note this works if last_he is null....
            {
                last_he = he;
                Display();
                var ml = DiscoveryForm.History.MissionListAccumulator.GetAllCurrentMissions(he.MissionList, he.EventTimeUTC);    // will always return an array
                NextExpiryUTC = ml.OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? EliteReleaseDates.GameEndTime;
                //System.Diagnostics.Debug.WriteLine($"Faction List recalc {he.EventTimeUTC} {he.MissionList} vs {last_he?.MissionList}, next expiry time {NextExpiryUTC}");
            }
        }

        #endregion

        #region Display

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[0];
            SortOrder sortorder = dataGridView.SortOrder != SortOrder.None ? dataGridView.SortOrder : SortOrder.Ascending;
            string toprowfaction = dataGridView.SafeFirstDisplayedScrollingRowIndex() >= 0 ? (dataGridView.Rows[dataGridView.SafeFirstDisplayedScrollingRowIndex()].Tag as FactionStats.FactionStatistics).Name : "";

            dataGridView.Rows.Clear();

            DateTime startdateutc = startDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(startDateTimePicker.Value) : EliteReleaseDates.GameRelease;
            DateTime enddateutc = endDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(endDateTimePicker.Value) : EliteReleaseDates.GameEndTime;
            bool timepicked = startDateTimePicker.Checked || endDateTimePicker.Checked;

            var factionslist = EliteDangerousCore.FactionStats.Compute(DiscoveryForm.History.MissionListAccumulator.GetMissionList(last_he?.MissionList ?? 0),
                                                                       timepicked ? null : DiscoveryForm.History.GetStatsAtGeneration(last_he?.Statistics ?? 0),
                                                                       DiscoveryForm.History, startdateutc, enddateutc);

            // now update the grid with faction info

            if (factionslist.Count > 0)
            {
                var rows = new List<DataGridViewRow>();
                foreach (FactionStats.FactionStatistics fs in factionslist.Values)
                {
                    var info = "";

                    if (fs.MissionsInProgress > 0)
                        info = info.AppendPrePad("Missions In Progress:".T(EDTx.UserControlFactions_MissionsInProgress) + " " + fs.MissionsInProgress , ", ");

                    foreach (var reward in fs.MissionRewards.Values)
                    {
                        info = info.AppendPrePad(reward.Count + " " + reward.Name, ", ");
                    }

                    if (fs.FactionStats.CapShipAwardAsVictimFaction > 0 )
                        info = info.AppendPrePad("Capital ship Victims: ".T(EDTx.UserControlFactions_CapShipVictims) + fs.FactionStats.CapShipAwardAsVictimFaction, ", ");
                    if (fs.FactionStats.CapShipAwardAsAwaringFaction > 0 )
                        info = info.AppendPrePad("Capital ship Award: ".T(EDTx.UserControlFactions_CapShipAward) + fs.FactionStats.CapShipAwardAsAwaringFaction + ":" + fs.FactionStats.CapShipAwardAsAwaringFactionValue.ToString("N0") + "cr", ", ");

                    object[] rowobj = { fs.Name,
                                        fs.TotalMissions.ToString("N0"), fs.Influence.ToString("N0"), fs.Reputation.ToString("N0"), fs.MissionCredits.ToString("N0"),
                                        fs.FactionStats.BoughtCommodity.ToString("N0"), fs.FactionStats.SoldCommodity.ToString("N0"),fs.FactionStats.ProfitCommodity.ToString("N0"),
                                        fs.FactionStats.BoughtMaterial.ToString("N0"), fs.FactionStats.SoldMaterial.ToString("N0"),
                                        fs.FactionStats.CrimesAgainst.ToString("N0") ,
                                        fs.FactionStats.BountyKill.ToString("N0"), fs.FactionStats.BountyRewards.ToString("N0"), fs.FactionStats.BountyRewardsValue.ToString("N0"),
                                        fs.FactionStats.RedeemVoucherValue.ToString("N0"), fs.FactionStats.FineValue.ToString("N0"),fs.FactionStats.PayBountyValue.ToString("N0"),                 
                                        fs.FactionStats.Interdicted.ToString("N0"), fs.FactionStats.Interdiction.ToString("N0"),
                                        fs.FactionStats.KillBondAwardAsVictimFaction.ToString("N0"), fs.FactionStats.KillBondAwardAsAwaringFaction.ToString("N0"), fs.FactionStats.KillBondAwardAsAwaringFactionValue.ToString("N0"),
                                        fs.FactionStats.CartographicDataSold.ToString("N0"),
                                        fs.FactionStats.DataLinkAwardAsVictimFaction.ToString("N0"),fs.FactionStats.DataLinkAwardAsPayeeFaction.ToString("N0"),fs.FactionStats.DataLinkAwardAsPayeeFactionValue.ToString("N0"),
                                        info };

                    var row = dataGridView.RowTemplate.Clone() as DataGridViewRow;
                    row.CreateCells(dataGridView, rowobj);
                    row.Tag = fs;
                    rows.Add(row);
                }

                dataGridView.Rows.AddRange(rows.ToArray());

                dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

                if (toprowfaction.HasChars())
                {
                    for( int i = 0; i < dataGridView.RowCount; i++ )
                    {
                        var fs = dataGridView.Rows[i].Tag as FactionStats.FactionStatistics;
                        if ( fs.Name == toprowfaction )
                        {
                            dataGridView.SafeFirstDisplayedScrollingRowIndex(i);
                            break;
                        }
                    }
                }
            }

            labelInfo.Text = factionslist.Count + " " + "Factions".T(EDTx.UserControlFactions_FactionsPlural);
        }

        #endregion

        #region Misc

        private void VerifyDates()
        {
            if (!EDDConfig.Instance.DateTimeInRangeForGame(startDateTimePicker.Value) || !EDDConfig.Instance.DateTimeInRangeForGame(endDateTimePicker.Value))
            {
                startDateTimePicker.Checked = endDateTimePicker.Checked = false;
                startDateTimePicker.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EliteReleaseDates.GameRelease);
                endDateTimePicker.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow.EndOfDay());
            }
        }

        private void startDateTime_ValueChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void endDateTime_ValueChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void dataGridViewFactions_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 1 && e.Column.Index <= dataGridView.ColumnCount-2)
            {
                e.SortDataGridViewColumnNumeric();
            }
        }

        #endregion

        #region Right clicks

        private void showMissionsForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = dataGridView.RightClickRow;

            if (row >= 0)
            {
                var fs = dataGridView.Rows[row].Tag as FactionStats.FactionStatistics;

                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                MissionListUserControl mluc = new MissionListUserControl();

                mluc.Start();

                List<MissionState> ml = DiscoveryForm.History.MissionListAccumulator.GetMissionList(last_he?.MissionList ?? 0);

                DateTime startdateutc = startDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(startDateTimePicker.Value) : EliteReleaseDates.GameRelease;
                DateTime enddateutc = endDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(endDateTimePicker.Value) : EliteReleaseDates.GameEndTime;

                if (ml != null)
                {
                    foreach (MissionState ms in ml)
                    {
                        if (ms.State == MissionState.StateTypes.Completed && ms.Completed != null)
                        {
                            if (DateTime.Compare(ms.Completed.EventTimeUTC, startdateutc) >= 0 &&
                                DateTime.Compare(ms.Completed.EventTimeUTC, enddateutc) <= 0)
                            {
                                var faction = ms.Mission.Faction;
                                if (faction == fs.Name)
                                    mluc.Add(ms, true,"");
                            }
                        }
                    }

                    mluc.CompletedFill();
                }

                mluc.Finish();

                DGVLoadColumnLayout(mluc.dataGridView, "ShowMission");

                f.Add(new ExtendedControls.ConfigurableForm.Entry(mluc, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
                { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });

                f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();

                f.AllowResize = true;

                f.ShowDialogCentred(FindForm(), FindForm().Icon, "Missions for ".T(EDTx.UserControlFactions_MissionsFor) + fs.Name, closeicon: true);

                DGVSaveColumnLayout(mluc.dataGridView, "ShowMission");
            }
        }

        // From last_he, and using the start/end time if required, filter history before this date, with the predicate

        private List<HistoryEntry> FilterHistory(Predicate<HistoryEntry> where)
        {
            if (last_he != null)
            {
                DateTime startdateutc = startDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(startDateTimePicker.Value) : EliteReleaseDates.GameRelease;
                DateTime enddateutc = endDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(endDateTimePicker.Value) : EliteReleaseDates.GameEndTime;
                return HistoryList.FilterBefore(DiscoveryForm.History.EntryOrder(), last_he, 
                                    (x) => ((DateTime.Compare(x.EventTimeUTC, startdateutc) >= 0 &&
                                             DateTime.Compare(x.EventTimeUTC, enddateutc) <= 0) &&
                                             where(x)));
            }
            else
            {
                return new List<HistoryEntry>();
            }
        }

        private void showCommoditymaterialTradesForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.RightClickRow >= 0)
            {
                var fs = dataGridView.Rows[dataGridView.RightClickRow].Tag as FactionStats.FactionStatistics;

                var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
                dgvpanel.DataGrid.CreateTextColumns("Date".T(EDTx.UserControlOutfitting_Date), 100, 5,
                                                    "Item".T(EDTx.UserControlFactions_Item), 150, 5,
                                                    "Bought".T(EDTx.UserControlStats_GoodsBought), 50, 5,
                                                    "Sold".T(EDTx.UserControlStats_GoodsSold), 50, 5,
                                                    "Profit".T(EDTx.UserControlStats_GoodsProfit), 50, 5);

                dgvpanel.DataGrid.SortCompare += (s, ev) => { if (ev.Column.Index >= 2) ev.SortDataGridViewColumnNumeric(); };
                dgvpanel.DataGrid.RowHeadersVisible = false;
                dgvpanel.DataGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvpanel.DataGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvpanel.DataGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowCommdsMats");

                long profit = 0;

                foreach (var he in FilterHistory((x) => x.journalEntry is IStatsJournalEntryMatCommod && x.Status.StationFaction == fs.Name))
                {
                    var items = (he.journalEntry as IStatsJournalEntryMatCommod).ItemsList;
                    foreach (var i in items)
                    {
                        var m = EliteDangerousCore.MaterialCommodityMicroResourceType.GetByFDName(i.FDName);     // and we see if we actually have some at this time
                        string name = m?.TranslatedName ?? i.FDName;

                        int bought = i.Count > 0 ? i.Count : 0;
                        int sold = i.Count < 0 ? -i.Count : 0;

                        object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
                                            name,
                                            bought.ToString("N0"),
                                            sold.ToString("N0"),
                                            i.Profit.ToString("N0")};
                        var row = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                        row.CreateCells(dgvpanel.DataGrid, rowobj);
                        dgvpanel.DataGrid.Rows.Add(row);

                        profit += i.Profit;
                    }
                }

                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                f.Add(new ExtendedControls.ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
                { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
                f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();
                f.AllowResize = true;

                string title = "Materials/Commodities for ".T(EDTx.UserControlFactions_MaterialCommodsFor) + fs.Name;
                if (profit != 0)
                    title += " (" + profit.ToString("N0") + "cr)";
                f.ShowDialogCentred(FindForm(), FindForm().Icon, title  , closeicon: true);

                DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowCommdsMats");

            }
        }

        private void showBountiesAndBondsForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.RightClickRow >= 0)
            {
                var fs = dataGridView.Rows[dataGridView.RightClickRow].Tag as FactionStats.FactionStatistics;

                var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
                dgvpanel.DataGrid.CreateTextColumns("Date".T(EDTx.UserControlOutfitting_Date), 100, 5,
                                                    "Bounty/Bond".T(EDTx.UserControlFactions_BountyBond), 80, 5,
                                                    "Target".T(EDTx.UserControlFactions_Target), 150, 5,
                                                    "Target Faction".T(EDTx.UserControlFactions_TargetFaction), 150, 5,
                                                    "Reward".T(EDTx.UserControlFactions_Reward), 60, 5);
                dgvpanel.DataGrid.SortCompare += (s, ev) => { if (ev.Column.Index >= 4) ev.SortDataGridViewColumnNumeric(); };
                dgvpanel.DataGrid.RowHeadersVisible = false;
                dgvpanel.DataGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowBonds");

                foreach (var he in FilterHistory((x) => x.journalEntry is IStatsJournalEntryBountyOrBond &&
                                                    (x.journalEntry as IStatsJournalEntryBountyOrBond).HasFaction(fs.Name)))
                {
                    var bb = he.journalEntry as IStatsJournalEntryBountyOrBond;
                    object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
                                        bb.Type, bb.Target, bb.TargetFaction, bb.FactionReward(fs.Name).ToString("N0") };
                    var row = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                    row.CreateCells(dgvpanel.DataGrid, rowobj);
                    dgvpanel.DataGrid.Rows.Add(row);
                }

                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                f.Add(new ExtendedControls.ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
                { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
                f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();
                f.AllowResize = true;

                f.ShowDialogCentred(FindForm(), FindForm().Icon, "Bounties/Bonds for ".T(EDTx.UserControlFactions_BountiesBondsFor) + fs.Name, closeicon: true);

                DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowBonds");
            }
        }

        private class SystemInfo
        {
            public string Name { get; set; }
            public long? Address { get; set; }
            public int? Influence { get; set; }
            public int? Missions { get; set; }
            public int? CommoditiesSold { get; private set; }
            public int? CommoditiesBought { get; private set; }
            public int? MaterialsSold { get; private set; }
            public int? MaterialsBought { get; private set; }
            public int? Bounties { get; private set; }
            public long? BountyRewardsValue { get; private set; }
            public int? KillBonds { get; private set; }
            public long? BondsRewardsValue { get; private set; }
            public long? CartographicValue { get; private set; }

            public void AddCommoditiesSold(int a) { CommoditiesSold = (CommoditiesSold ?? 0) + a; }
            public void AddCommoditiesBought(int a) { CommoditiesBought = (CommoditiesBought ?? 0) + a; }
            public void AddMaterialsSold(int a) { MaterialsSold = (MaterialsSold ?? 0) + a; }
            public void AddMaterialsBought(int a) { MaterialsBought = (MaterialsBought ?? 0) + a; }
            public void AddBounties(int a) { Bounties = (Bounties ?? 0) + a; }
            public void AddBountyRewardsValue(long a) { BountyRewardsValue = (BountyRewardsValue ?? 0) + a; }
            public void AddKillBonds(int a) { KillBonds = (KillBonds ?? 0) + a; }
            public void AddBondsRewardsValue(long a) { BondsRewardsValue = (BondsRewardsValue ?? 0) + a; }
            public void AddCartographicValue(long a) { CartographicValue = (CartographicValue ?? 0) + a; }
        }



        private void showFactionSystemDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.RightClickRow >= 0)
            {
               var fs = dataGridView.Rows[dataGridView.RightClickRow].Tag as FactionStats.FactionStatistics;

                var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
                dgvpanel.DataGrid.CreateTextColumns("System".T(EDTx.UserControlModules_System), 100, 5,
                                                    "System Address".T(EDTx.UserControlFactions_SystemAddress), 60, 5,
                                                    "Missions".T(EDTx.UserControlMissions_MPlural), 50, 5,
                                                    "+Influence".T(EDTx.UserControlFactions_colInfluence), 50, 5,       // these align with columns of main view, with same names
                                                    "Commds +".T(EDTx.UserControlFactions_CBought), 50, 5,      
                                                    "Commds -".T(EDTx.UserControlFactions_CSold), 50, 5,
                                                    "Mats +".T(EDTx.UserControlFactions_MBought), 50, 5,
                                                    "Mats -".T(EDTx.UserControlFactions_MSold), 50, 5,
                                                    "Bounties".T(EDTx.UserControlFactions_BountiesPlural), 50, 5,
                                                    "Rewards".T(EDTx.UserControlFactions_RewardsPlural), 60, 5,
                                                    "Bonds".T(EDTx.UserControlFactions_BondsPlural), 50, 5,
                                                    "Rewards".T(EDTx.UserControlFactions_RewardsPlural), 60, 5,
                                                    "Cartographic Value".T(EDTx.UserControlFactions_CartoValue), 60, 5);
                dgvpanel.DataGrid.SortCompare += (s, ev) => { if (ev.Column.Index >= 1) ev.SortDataGridViewColumnNumeric(); };
                dgvpanel.DataGrid.RowHeadersVisible = false;
                dgvpanel.DataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                for (int col = 1; col < dgvpanel.DataGrid.ColumnCount - 1; col++)
                    dgvpanel.DataGrid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowSystemDetail");

                // systems to present
                var systems = new List<SystemInfo>();

                // look thru the influence systems and add it to the list of systems

                foreach (var si in fs.MissionSystemsWithInfluence.Values)
                {
                    string systemName = null;
                    if (last_he != null)
                    {
                        foreach (var he in HistoryList.FilterBefore(DiscoveryForm.History.EntryOrder(), last_he, 
                                    (x) => x.System.SystemAddress == si.SystemAddress))
                        {
                            systemName = he.System.Name;
                            break;
                        }
                    }
                    systems.Add(new SystemInfo { Name = systemName, Address = si.SystemAddress, Missions = si.Missions, Influence = si.Influence });
                }

                // find all the history entries with faction, taking into account start/end date, and last_he position

                var list = FilterHistory((x) =>
                        (x.journalEntry is IStatsJournalEntryMatCommod && x.Status.StationFaction == fs.Name) ||  // he's with changes in stats due to MatCommod trading
                        (x.journalEntry is IStatsJournalEntryBountyOrBond && (x.journalEntry as IStatsJournalEntryBountyOrBond).HasFaction(fs.Name) ) ||  // he's with Bountry/bond
                        ((x.journalEntry.EventTypeID == JournalTypeEnum.SellExplorationData || x.journalEntry.EventTypeID == JournalTypeEnum.MultiSellExplorationData) && x.Status.StationFaction == fs.Name)// he's for exploration
                        );

                foreach (var he in list)
                {
                    SystemInfo si = systems.Find(x =>           // do we have this previous entry?
                        (he.System.SystemAddress != null && x.Address == he.System.SystemAddress) ||
                        (he.System.Name != null && x.Name == he.System.Name));

                    if (si == null)     // no, add it to the system list
                    {
                        si = new SystemInfo { Name = he.System.Name, Address = he.System.SystemAddress };
                        systems.Add(si);
                    }

                    if (he.journalEntry is IStatsJournalEntryMatCommod)         // is this a material or commodity trade?
                    {
                        var items = (he.journalEntry as IStatsJournalEntryMatCommod).ItemsList;
                        foreach (var i in items)
                        {
                            if (he.journalEntry.EventTypeID == JournalTypeEnum.MaterialTrade)       // material trade is only counter for mats
                            {
                                if (i.Count > 0)
                                    si.AddMaterialsBought(i.Count);
                                else if (i.Count < 0)
                                    si.AddMaterialsSold(-i.Count);
                            }
                            else
                            {                                               // all others are commds
                                if (i.Count > 0)
                                    si.AddCommoditiesBought(i.Count);
                                else
                                    si.AddCommoditiesSold(-i.Count);        // value is negative, invert
                            }
                        }
                    }
                    else
                    {
                      //  System.Diagnostics.Debug.WriteLine($"Faction {fs.Name} Journal entry {he.journalEntry.EventTypeStr} {he.System.Name}");

                        if (he.journalEntry.EventTypeID == JournalTypeEnum.Bounty)
                        {
                            si.AddBounties(1);
                            si.AddBountyRewardsValue((he.journalEntry as IStatsJournalEntryBountyOrBond).FactionReward(fs.Name));
                        }
                        else if (he.journalEntry.EventTypeID == JournalTypeEnum.FactionKillBond)
                        {
                            si.AddKillBonds(1);
                            si.AddBondsRewardsValue((he.journalEntry as IStatsJournalEntryBountyOrBond).FactionReward(fs.Name));
                        }
                        else if (he.journalEntry.EventTypeID == JournalTypeEnum.SellExplorationData)
                        {
                            si.AddCartographicValue((he.journalEntry as EliteDangerousCore.JournalEvents.JournalSellExplorationData).TotalEarnings);
                        }
                        else if (he.journalEntry.EventTypeID == JournalTypeEnum.MultiSellExplorationData)
                        {
                            si.AddCartographicValue((he.journalEntry as EliteDangerousCore.JournalEvents.JournalMultiSellExplorationData).TotalEarnings);
                        }
                    }
                }

                foreach (var system in systems)
                {
                    object[] rowobj = { system.Name,
                                        system.Address,
                                        system.Missions?.ToString("N0"),
                                        system.Influence?.ToString("N0"),
                                        system.CommoditiesBought?.ToString("N0"),
                                        system.CommoditiesSold?.ToString("N0"),
                                        system.MaterialsBought?.ToString("N0"),
                                        system.MaterialsSold?.ToString("N0"),
                                        system.Bounties?.ToString("N0"),
                                        system.BountyRewardsValue?.ToString("N0"),
                                        system.KillBonds?.ToString("N0"),
                                        system.BondsRewardsValue?.ToString("N0"),
                                        system.CartographicValue?.ToString("N0"),
                                    };
                    var row = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                    row.CreateCells(dgvpanel.DataGrid, rowobj);
                    dgvpanel.DataGrid.Rows.Add(row);
                }

                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                f.Add(new ExtendedControls.ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(960, 400), null)
                { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
                f.AddOK(new Point(960 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();
                f.AllowResize = true;

                f.ShowDialogCentred(FindForm(), FindForm().Icon, "Systems Detail for ".T(EDTx.UserControlFactions_SystemsDetailFor) + fs.Name, closeicon: true);

                DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowSystemDetail");
            }
        }

        #endregion

        #region Excel
        private void buttonExtExcel_Click(object sender, EventArgs e)
        {
            Forms.ImportExportForm frm = new Forms.ImportExportForm();

            frm.Export(new string[] { "Export Current View" }, new Forms.ImportExportForm.ShowFlags[] { Forms.ImportExportForm.ShowFlags.ShowCSVOpenInclude });

            if (frm.ShowDialog(this.FindForm()) == DialogResult.OK)
            {
                BaseUtils.CSVWriteGrid grd = new BaseUtils.CSVWriteGrid(frm.Delimiter);

                grd.GetHeader += delegate (int c)
                {
                    return (frm.IncludeHeader && c < dataGridView.ColumnCount) ? dataGridView.Columns[c].HeaderText : null;
                };

                grd.GetLine += delegate (int r)
                {
                    if (r < dataGridView.RowCount)
                    {
                        DataGridViewRow rw = dataGridView.Rows[r];
                        return rw.CellsObjects();
                    }
                    else
                        return null;
                };

                grd.WriteGrid(frm.Path, frm.AutoOpen, FindForm());
            }
        }

        #endregion
    }
}
