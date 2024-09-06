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
        private HistoryEntry last_he_received = null;           // tracks the last entry in history
        private Timer searchtimer;

        #region Init

        public UserControlFactions()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "FactionsPanel";

            dataGridView.MakeDoubleBuffered();

            for (int col = colMissions.Index; col < colInfo.Index; col++)
                dataGridView.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            startDateTimePicker.Value = GetSetting("StartDate", DateTime.UtcNow).StartOfDay();
            startDateTimePicker.Checked = GetSetting("StartDateChecked", false);
            endDateTimePicker.Value = GetSetting("EndDate", DateTime.UtcNow.EndOfDay()).EndOfDay();
            endDateTimePicker.Checked = GetSetting("EndDateChecked", false);
            VerifyDates();
            startDateTimePicker.ValueChanged += new System.EventHandler(this.startDateTime_ValueChanged);        // now install the change handlers
            endDateTimePicker.ValueChanged += new System.EventHandler(this.endDateTime_ValueChanged);

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry += DiscoveryForm_OnNewEntry;

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
                        EDTx.UserControlFactions_colDataLinkVictimFaction, EDTx.UserControlFactions_colDataLinkPayeeFaction, EDTx.UserControlFactions_colDataLinkPayeeValue,
                        EDTx.UserControlFactions_colLastRep,EDTx.UserControlFactions_colSystem,
                        EDTx.UserControlFactions_labelSearch,
                        EDTx.UserControlFactions_colFactionState, EDTx.UserControlFactions_colFactionGov,
                        EDTx.UserControlFactions_colFactionAllegiance, EDTx.UserControlFactions_colFactionSystemInfluence, EDTx.UserControlFactions_colFactionOtherSystemInfo,
                        EDTx.UserControlFactions_colOrganicDataSold,

                        };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            var enumlistcms = new Enum[] { EDTx.UserControlFactions_showMissionsForFactionToolStripMenuItem, 
                                           EDTx.UserControlFactions_showCommoditymaterialTradesForFactionToolStripMenuItem,
                                           EDTx.UserControlFactions_showBountiesAndBondsForFactionToolStripMenuItem,
                };

            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);

            var enumlisttt = new Enum[] { EDTx.UserControlFactions_startDateTimePicker_ToolTip, EDTx.UserControlFactions_endDateTimePicker_ToolTip, 
                                        EDTx.UserControlFactions_buttonExtExcel_ToolTip,
                                        EDTx.UserControlFactions_extCheckBoxShowHideSystemInfo_ToolTip, EDTx.UserControlFactions_extCheckBoxShowHideMission_ToolTip,
                                        EDTx.UserControlFactions_extCheckBoxShowHideCommodities_ToolTip, EDTx.UserControlFactions_extCheckBoxShowHideMaterials_ToolTip,
                                        EDTx.UserControlFactions_extCheckBoxShowHideBounties_ToolTip, EDTx.UserControlFactions_extCheckBoxShowHideInterdictions_ToolTip,
                                        EDTx.UserControlFactions_extCheckBoxShowHideKillBonds_ToolTip, EDTx.UserControlFactions_extCheckBoxShowHideDataLink_ToolTip,
                                        EDTx.UserControlFactions_extCheckBoxShowHideCartographic_ToolTip
                    };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            labelInfo.Text = "";

            searchtimer = new Timer() { Interval = 500 };
            searchtimer.Tick += (e, s) => { searchtimer.Stop(); Display(); };
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
            SetHideButtonState();
        }

        public override void InitialDisplay()
        {
            last_he_received = DiscoveryForm.History.GetLast;
            Display();
        }

        public override void Closing()
        {
            searchtimer.Stop();

            DGVSaveColumnLayout(dataGridView);

            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry -= DiscoveryForm_OnNewEntry;

            PutSetting("StartDate", startDateTimePicker.Value);                 // picker value is stored..
            PutSetting("StartDateChecked", startDateTimePicker.Checked);
            PutSetting("EndDate", endDateTimePicker.Value);
            PutSetting("EndDateChecked", endDateTimePicker.Checked);
        }

        private void Discoveryform_OnHistoryChange()     // may have changed date system, this causes this
        {
            last_he_received = DiscoveryForm.History.GetLast;       // set last he received to the top
            VerifyDates();
            Display();
        }

        private void DiscoveryForm_OnNewEntry(HistoryEntry he)
        {
            if (he.MissionList != last_he_received?.MissionList || he.journalEntry is IStatsJournalEntry || he.EventTimeUTC > NextExpiryUTC)      // note this works if last_he is null....
            {
                last_he_received = he;
                Display();
                var ml = DiscoveryForm.History.MissionListAccumulator.GetAllCurrentMissions(he.MissionList, he.EventTimeUTC);    // will always return an array
                NextExpiryUTC = ml.OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? EliteReleaseDates.GameEndTime;
                System.Diagnostics.Debug.WriteLine($"Faction List recalc {he.EventTimeUTC} {he.MissionList} vs {last_he_received?.MissionList}, next expiry time {NextExpiryUTC}");
            }
        }

        #endregion

        #region Display

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : null;      // default is no sort, as per the order coming from stats computer
            SortOrder sortorder = dataGridView.SortOrder != SortOrder.None ? dataGridView.SortOrder : SortOrder.Ascending;
            string toprowfaction = dataGridView.SafeFirstDisplayedScrollingRowIndex() >= 0 ? (dataGridView.Rows[dataGridView.SafeFirstDisplayedScrollingRowIndex()].Tag as FactionStatsComputer.FactionResults).Name : "";

            dataGridView.Rows.Clear();

            bool timepicked = UTCRange(out DateTime startdateutc, out DateTime enddateutc);

            var laststatsfromcomputer = FactionStatsComputer.Compute(DiscoveryForm.History.MissionListAccumulator.GetAllMissions(), // pass in all missions, the computer will filter them out 
                                                                       timepicked ? null : DiscoveryForm.History.Stats,     // if we are timepicking, we let the computer make stats up. Else we pass in the latest
                                                                       DiscoveryForm.History,
                                                                       startdateutc, enddateutc);
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            foreach (FactionStatsComputer.FactionResults fr in laststatsfromcomputer.Values)
            {
                var systems = fr.FactionStats?.PerSystemData.Keys.ToHashSet();      // faction stats may be null
                if (systems != null)
                    systems.UnionWith(fr.PerSystemData.Keys.ToList());       // MissionPerSystemData always there.
                else
                    systems = fr.PerSystemData.Keys.ToHashSet();

                int sysindex = 0;
                foreach ( var systemname in systems)
                {
                    //System.Diagnostics.Debug.WriteLine($"{fr.Name} in {systemname}");

                    Stats.FactionStatistics.PerSystem fs = null;
                    if ( fr.FactionStats != null)
                        fr.FactionStats.PerSystemData.TryGetValue(systemname, out fs);        // may be null

                    fr.PerSystemData.TryGetValue(systemname, out FactionStatsComputer.FactionResults.PerSystem ms);        // may be null

                    var info = "";

                    if (ms != null)
                    {
                        if (ms.MissionsInProgress > 0)
                            info = info.AppendPrePad("Missions In Progress:".T(EDTx.UserControlFactions_MissionsInProgress) + " " + ms.MissionsInProgress, ", ");

                        foreach (var reward in ms.MissionRewards.Values)
                        {
                            info = info.AppendPrePad(reward.Count + " " + reward.Name, ", ");
                        }
                    }

                    if (fs != null)
                    {
                        if (fs.CapShipAwardAsVictimFaction > 0)
                            info = info.AppendPrePad("Capital ship Victims: ".T(EDTx.UserControlFactions_CapShipVictims) + fs.CapShipAwardAsVictimFaction, ", ");
                        if (fs.CapShipAwardAsAwaringFaction > 0)
                            info = info.AppendPrePad("Capital ship Award: ".T(EDTx.UserControlFactions_CapShipAward) + fs.CapShipAwardAsAwaringFaction + ":" + fs.CapShipAwardAsAwaringFactionValue.ToString("N0") + "cr", ", ");
                    }

                    System.Text.StringBuilder factionstring = new System.Text.StringBuilder();
                    if (fs?.FactionInfo != null)
                        fs.FactionInfo.ToString(factionstring, true, false, true, true,true,false);

                    object[] rowobj = { fr.Name, ms!=null ? ms.System.Name : fs.System.Name,
                                                fr.FactionStats?.LastReputation != null ? $"{FactionDefinitions.FactionInformation.Reputation(fr.FactionStats.LastReputation)} ({fr.FactionStats.LastReputation:0.#})" : "",
                                                fs?.FactionInfo != null ? FactionDefinitions.ToLocalisedLanguage(fs.FactionInfo.FactionState) : "",
                                                fs?.FactionInfo != null ? GovernmentDefinitions.ToLocalisedLanguage(fs.FactionInfo.Government) : "",
                                                fs?.FactionInfo != null ? AllegianceDefinitions.ToLocalisedLanguage(fs.FactionInfo.Allegiance) : "",
                                                fs?.FactionInfo != null ? (fs.FactionInfo.Influence*100).ToString("N1") + "%" : "",
                                                factionstring.ToString(),
                                                ms != null ? ms.Missions.ToString("N0") : "0",
                                                ms != null ? ms.Influence.ToString("N0") : "0",
                                                ms != null ? ms.Reputation.ToString("N0") : "0",
                                                ms != null ? ms.MissionCredits.ToString("N0") : "0",
                                                fs != null ? fs.BoughtCommodity.ToString("N0") : "0",
                                                fs != null ? fs.SoldCommodity.ToString("N0") : "0",
                                                fs != null ? fs.ProfitCommodity.ToString("N0") : "0",
                                                fs != null ? fs.BoughtMaterial.ToString("N0") : "0",
                                                fs != null ? fs.SoldMaterial.ToString("N0") : "0",
                                                fs != null ? fs.CrimesAgainst.ToString("N0") : "0",
                                                fs != null ? fs.BountyKill.ToString("N0") : "0",
                                                fs != null ? fs.BountyRewards.ToString("N0") : "0",
                                                fs != null ? fs.BountyRewardsValue.ToString("N0") : "0",
                                                fs != null ? fs.RedeemVoucherValue.ToString("N0") : "0",
                                                fs != null ? fs.FineValue.ToString("N0") : "0",
                                                fs != null ? fs.PayBountyValue.ToString("N0") : "0",
                                                fs != null ? fs.Interdicted.ToString("N0") : "0",
                                                fs != null ? fs.Interdiction.ToString("N0") : "0",
                                                fs != null ? fs.KillBondAwardAsVictimFaction.ToString("N0") : "0",
                                                fs != null ? fs.KillBondAwardAsAwardingFaction.ToString("N0") : "0",
                                                fs != null ? fs.KillBondAwardAsAwardingFactionValue.ToString("N0") : "0",
                                                fs != null ? fs.CartographicDataSold.ToString("N0") : "0",
                                                fs != null ? fs.OrganicDataSold.ToString("N0") : "0",
                                                fs != null ? fs.DataLinkAwardAsVictimFaction.ToString("N0") : "0",
                                                fs != null ? fs.DataLinkAwardAsPayeeFaction.ToString("N0") : "0",
                                                fs != null ? fs.DataLinkAwardAsPayeeFactionValue.ToString("N0") : "0",
                                                info };

                    bool matched = false;
                    if (textBoxSearch.Text.HasChars())
                    {
                        foreach (var c in rowobj)
                        {
                            if (((string)c).ContainsIIC(textBoxSearch.Text))
                            {
                                matched = true;
                                break;
                            }
                        }
                    }
                    else
                        matched = true;

                    if (matched)
                    {
                        var row = dataGridView.RowTemplate.Clone() as DataGridViewRow;
                        row.CreateCells(dataGridView, rowobj);
                        row.Tag = fr;

                        if (sysindex++ >= 1)
                            row.Cells[0].Style.Alignment = DataGridViewContentAlignment.MiddleRight;    // to indicate second or futher systems
                        rows.Add(row);

                    }
                }

            }

            dataGridView.Rows.AddRange(rows.ToArray());

            if (sortcol != null)
            {
                dataGridView.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                dataGridView.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;
            }

            if (toprowfaction.HasChars())
            {
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    var fs = dataGridView.Rows[i].Tag as FactionStatsComputer.FactionResults;
                    if (fs.Name == toprowfaction)
                    {
                        dataGridView.SafeFirstDisplayedScrollingRowIndex(i);
                        break;
                    }
                }
            }

            labelInfo.Text = laststatsfromcomputer.Count + " " + "Factions".T(EDTx.UserControlFactions_FactionsPlural);
        }

        #endregion

        #region UI

        private void startDateTime_ValueChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void endDateTime_ValueChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            searchtimer.Stop();
            searchtimer.Start();
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex >= colMissions.Index && e.ColumnIndex <= colMissionCredits.Index)
                {
                    ShowMissionInfo(e.RowIndex);
                }
                else if (e.ColumnIndex >= CBought.Index && e.ColumnIndex <= MSold.Index)
                {
                    ShowCommodityMaterialTrades(e.RowIndex);
                }
                else if ((e.ColumnIndex >= BountyKills.Index && e.ColumnIndex <= BountyRewardsValue.Index) ||
                        (e.ColumnIndex >= KillBondVictim.Index && e.ColumnIndex <= KillBondsValue.Index))
                {
                    ShowBountiesAndBonds(e.RowIndex);
                }
            }
        }

        private void SetHideButtonState()
        {
            changinghide = true;
            extCheckBoxShowHideSystemInfo.Checked = colFactionState.Visible || colFactionGov.Visible || colFactionAllegiance.Visible ||
                colFactionSystemInfluence.Visible || colFactionOtherSystemInfo.Visible;
            extCheckBoxShowHideMission.Checked = colMissions.Visible || colInfluence.Visible || colReputation.Visible || colMissionCredits.Visible;
            extCheckBoxShowHideCommodities.Checked = CBought.Visible || CSold.Visible || CProfit.Visible;
            extCheckBoxShowHideMaterials.Checked = MBought.Visible || MSold.Visible;
            extCheckBoxShowHideBounties.Checked = CrimeCommitted.Visible || BountyKills.Visible || BountyRewardsValue.Visible || BountyValue.Visible ||
                                colRedeemVoucher.Visible || colFines.Visible || colBountyValue.Visible;
            extCheckBoxShowHideInterdictions.Checked = Interdiction.Visible || Interdicted.Visible;
            extCheckBoxShowHideKillBonds.Checked = KillBondVictim.Visible || KillBondsAward.Visible || KillBondsValue.Visible;
            extCheckBoxShowHideCartographic.Checked = colOrganicDataSold.Visible || CartoValue.Visible;
            extCheckBoxShowHideDataLink.Checked = colDataLinkPayeeFaction.Visible || colDataLinkVictimFaction.Visible || colDataLinkPayeeValue.Visible;
            changinghide = false;
        }

        bool changinghide = true;
        private void extCheckBoxShowHideSystemInfo_CheckedChanged(object sender, EventArgs e)
        {
            if (!changinghide)
            {
                colFactionState.Visible = colFactionGov.Visible = colFactionAllegiance.Visible =
                    colFactionSystemInfluence.Visible = colFactionOtherSystemInfo.Visible = extCheckBoxShowHideSystemInfo.Checked;
            }
        }

        private void extCheckBoxShowHideMission_CheckedChanged(object sender, EventArgs e)
        {
            if (!changinghide)
                colMissions.Visible = colInfluence.Visible = colReputation.Visible = colMissionCredits.Visible = extCheckBoxShowHideMission.Checked;
        }
        private void extCheckBoxShowHideMaterials_CheckedChanged(object sender, EventArgs e)
        {
            if (!changinghide)
                MBought.Visible = MSold.Visible = extCheckBoxShowHideMaterials.Checked;
        }

        private void extCheckBoxShowHideCommodities_CheckedChanged(object sender, EventArgs e)
        {
            if (!changinghide)
                CBought.Visible = CSold.Visible = CProfit.Visible = extCheckBoxShowHideCommodities.Checked;
        }

        private void extCheckBoxShowHideBounties_CheckedChanged(object sender, EventArgs e)
        {
            if (!changinghide)
                CrimeCommitted.Visible = BountyKills.Visible = BountyRewardsValue.Visible = BountyValue.Visible =
                            colRedeemVoucher.Visible = colFines.Visible = colBountyValue.Visible = extCheckBoxShowHideBounties.Checked;
        }

        private void extCheckBoxShowHideInterdictions_CheckedChanged(object sender, EventArgs e)
        {
            if (!changinghide)
                Interdiction.Visible = Interdicted.Visible = extCheckBoxShowHideInterdictions.Checked;
        }

        private void extCheckBoxShowHideKillBonds_CheckedChanged(object sender, EventArgs e)
        {
            if (!changinghide)
                KillBondVictim.Visible = KillBondsAward.Visible = KillBondsValue.Visible = extCheckBoxShowHideKillBonds.Checked;
        }
        private void extCheckBoxShowHideCartographic_CheckedChanged(object sender, EventArgs e)
        {
            if (!changinghide)
                colOrganicDataSold.Visible = CartoValue.Visible = extCheckBoxShowHideCartographic.Checked;

        }

        private void extCheckBoxShowHideDataLink_CheckedChanged(object sender, EventArgs e)
        {
            if (!changinghide)
                colDataLinkPayeeFaction.Visible = colDataLinkVictimFaction.Visible = colDataLinkPayeeValue.Visible = extCheckBoxShowHideDataLink.Checked;
        }
        #endregion

        #region Right clicks

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void showMissionsForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.RightClickRow >= 0)
                ShowMissionInfo(dataGridView.RightClickRow);
        }

        private void showCommoditymaterialTradesForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.RightClickRow >= 0)
                ShowCommodityMaterialTrades(dataGridView.RightClickRow);
        }

        private void showBountiesAndBondsForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView.RightClickRow >= 0)
                ShowBountiesAndBonds(dataGridView.RightClickRow);
        }

        #endregion

        #region Pop out dialogs
        private void ShowMissionInfo(int row)
        { 
            var fr = dataGridView.Rows[row].Tag as FactionStatsComputer.FactionResults;      // grab relevant FS
            string factionname = fr.Name;

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
            MissionListUserControl mluc = new MissionListUserControl();

            mluc.Start();

            UTCRange(out DateTime startdateutc, out DateTime enddateutc);

            List<MissionState> ml = DiscoveryForm.History.MissionListAccumulator.GetMissionsBetween(startdateutc, enddateutc);  // get missions in date range

            foreach (MissionState ms in ml)
            {
                if (ms.State == MissionState.StateTypes.Completed && ms.Completed != null)
                {
                    if (ms.Mission.Faction == factionname)
                    {
                        mluc.Add(ms, true, "");
                        System.Diagnostics.Debug.WriteLine($"Mission {ms.Mission.MissionBasicInfo(false)}");
                    }
                }
            }

            mluc.CompletedFill();

            mluc.Finish();

            DGVLoadColumnLayout(mluc.dataGridView, "ShowMission");

            f.Add(new ExtendedControls.ConfigurableForm.Entry(mluc, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
            { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });

            f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();

            f.AllowResize = true;

            f.ShowDialogCentred(FindForm(), FindForm().Icon, "Missions for ".T(EDTx.UserControlFactions_MissionsFor) + factionname, closeicon: true);

            DGVSaveColumnLayout(mluc.dataGridView, "ShowMission");
        }

        private void ShowCommodityMaterialTrades(int row)
        { 
            var fr = dataGridView.Rows[row].Tag as FactionStatsComputer.FactionResults;      // grab relevant FS
            string factionname = fr.Name;

            var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnControl>();
            dgvpanel.DataGrid.CreateTextColumns("Date".T(EDTx.UserControlOutfitting_Date), 100, 5,
                                                "System".T(EDTx.CaptainsLogEntries_ColSystem), 150, 5,
                                                "Station".T(EDTx.ScanDisplayForm_Station), 150, 5,
                                                "Item".T(EDTx.UserControlFactions_Item), 150, 5,
                                                "Bought".T(EDTx.UserControlStats_GoodsBought), 50, 5,
                                                "Sold".T(EDTx.UserControlStats_GoodsSold), 50, 5,
                                                "Profit".T(EDTx.UserControlStats_GoodsProfit), 50, 5);

            dgvpanel.DataGrid.SortCompare += (s, ev) => { if (ev.Column.Index >= 4) ev.SortDataGridViewColumnNumeric(); };
            dgvpanel.DataGrid.RowHeadersVisible = false;
            dgvpanel.DataGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvpanel.DataGrid.Columns[5].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvpanel.DataGrid.Columns[6].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            DGVLoadColumnLayout(dgvpanel.DataGrid, "ShowCommdsMats");

            long profit = 0;

            var helist = FilterHistoryDatePredicate((x) => x.journalEntry is IStatsJournalEntryMatCommod && x.Status.StationFaction == factionname);

            foreach (var he in helist)
            {
                System.Diagnostics.Debug.WriteLine($"HE {he.EventTimeUTC} {he.EventSummary}");

                var items = (he.journalEntry as IStatsJournalEntryMatCommod).ItemsList;
                foreach (var i in items)
                {
                    var m = EliteDangerousCore.MaterialCommodityMicroResourceType.GetByFDName(i.FDName);     // and we see if we actually have some at this time
                    string name = m?.TranslatedName ?? i.FDName;

                    int bought = i.Count > 0 ? i.Count : 0;
                    int sold = i.Count < 0 ? -i.Count : 0;

                    object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
                                        he.System.Name,
                                        he.WhereAmI,
                                        name,
                                        bought.ToString("N0"),
                                        sold.ToString("N0"),
                                        i.Profit.ToString("N0")};
                    var dgvrow = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                    dgvrow.CreateCells(dgvpanel.DataGrid, rowobj);
                    dgvpanel.DataGrid.Rows.Add(dgvrow);

                    profit += i.Profit;
                }
            }

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
            f.Add(new ExtendedControls.ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
            { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
            f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.AllowResize = true;

            string title = "Materials/Commodities for ".T(EDTx.UserControlFactions_MaterialCommodsFor) + factionname;
            if (profit != 0)
                title += " (" + profit.ToString("N0") + "cr)";
            f.ShowDialogCentred(FindForm(), FindForm().Icon, title, closeicon: true);

            DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowCommdsMats");
        }

        private void ShowBountiesAndBonds(int row)
        { 
            var fr = dataGridView.Rows[row].Tag as FactionStatsComputer.FactionResults;      // grab relevant FS
            string factionname = fr.Name;

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

            foreach (var he in FilterHistoryDatePredicate((x) => x.journalEntry is IStatsJournalEntryBountyOrBond &&
                                                (x.journalEntry as IStatsJournalEntryBountyOrBond).HasFaction(factionname)))
            {
                System.Diagnostics.Debug.WriteLine($"HE {he.EventTimeUTC} {he.EventSummary}");
                var bb = he.journalEntry as IStatsJournalEntryBountyOrBond;
                object[] rowobj = { EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
                                    bb.Type, bb.Target, bb.TargetFaction, bb.FactionReward(factionname).ToString("N0") };
                var dgvrow = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                dgvrow.CreateCells(dgvpanel.DataGrid, rowobj);
                dgvpanel.DataGrid.Rows.Add(dgvrow);
            }

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
            f.Add(new ExtendedControls.ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
            { Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
            f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
            f.InstallStandardTriggers();
            f.AllowResize = true;

            f.ShowDialogCentred(FindForm(), FindForm().Icon, "Bounties/Bonds for ".T(EDTx.UserControlFactions_BountiesBondsFor) + factionname, closeicon: true);

            DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowBonds");
        }

        #endregion

        #region Helpers
        private bool UTCRange(out DateTime start, out DateTime end)
        {
            start = startDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(startDateTimePicker.Value) : EliteReleaseDates.GameRelease;
            end = endDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(endDateTimePicker.Value) : EliteReleaseDates.GameEndTime;
            return startDateTimePicker.Checked || endDateTimePicker.Checked;
        }

        // From top, and using the start/end time if required, filter history before this date, with the predicate
        private List<HistoryEntry> FilterHistoryDatePredicate(Predicate<HistoryEntry> where)
        {
            UTCRange(out DateTime start, out DateTime end);
            return HistoryList.FilterByDateRange(DiscoveryForm.History.EntryOrder(), start, end, where);
        }
        private void VerifyDates()
        {
            if (!EDDConfig.Instance.DateTimeInRangeForGame(startDateTimePicker.Value) || !EDDConfig.Instance.DateTimeInRangeForGame(endDateTimePicker.Value))
            {
                startDateTimePicker.Checked = endDateTimePicker.Checked = false;
                startDateTimePicker.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EliteReleaseDates.GameRelease);
                endDateTimePicker.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow.EndOfDay());
            }
        }

        private void dataGridViewFactions_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            // nasty but required
            if ( e.Column.Index >= colMissions.Index && e.Column.Index <= colDataLinkPayeeValue.Index || e.Column == colFactionSystemInfluence)
                e.SortDataGridViewColumnNumeric();
            else
                e.SortDataGridViewColumnAlpha();    // use this for others, will push "" down bottom
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
