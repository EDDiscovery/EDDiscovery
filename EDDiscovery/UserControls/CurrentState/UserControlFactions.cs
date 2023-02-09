/*
 * Copyright © 2020-2023 EDDiscovery development team
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
        private class MissionReward
        {
            public string Name { get; }
            public int Count { get; private set; }
            
            public MissionReward(string name)
            {
                Name = name;
                Count = 0;
            }

            public void Add(int amount)
            {
                Count += amount;
            }
        }

        private class SystemInfluence
        {
            public long SystemAddress { get; }
            public int Influence { get; private set; }
            public int Missions { get { return MissionsList.Count; } }
            private List<ulong> MissionsList;

            public SystemInfluence(long systemAddress, int influence, ulong missionId)
            {
                this.SystemAddress = systemAddress;
                this.Influence = influence;
                this.MissionsList = new List<ulong>();
                this.MissionsList.Add(missionId);
            }

            public void AddInfluence(int influence, ulong missionId)
            {
                this.Influence += influence;
                if (!this.MissionsList.Contains(missionId))
                {
                    this.MissionsList.Add(missionId);
                }
            }
        }

        private class FactionStatistics
        {
            public string Name { get; }
            public int Missions { get; private set; }
            public int MissionsInProgress {get; private set;}
            public int Influence { get; private set; }
            public int Reputation { get; private set; }
            public long Credits { get; private set; }
            public Dictionary<string, MissionReward> Rewards { get; }
            public Dictionary<long, SystemInfluence> SystemsWithInfluence { get; }
            public Stats.FactionInfo FactionStats { get; private set; }

            public FactionStatistics(string name)
            {
                Name = name;
                Missions = 0;
                Influence = 0;
                Reputation = 0;
                Credits = 0;
                Rewards = new Dictionary<string, MissionReward>();
                this.SystemsWithInfluence = new Dictionary<long, SystemInfluence>();
                FactionStats = defstats;
            }

            private static Stats.FactionInfo defstats = new Stats.FactionInfo("-");

            public void AddMissions(int amount)
            {
                Missions += amount;
            }

            public void AddMissionsInProgress(int amount)
            {
                MissionsInProgress += amount;
            }

            public void AddInfluence(int amount)
            {
                Influence += amount;
            }

            public void AddReputation(int amount)
            {
                Reputation += amount;
            }

            public void AddCredits(long amount)
            {
                Credits += amount;
            }

            public void AddFactionStats(Stats.FactionInfo mc)
            {
                FactionStats = mc;
            }

            public void AddReward(string name, int amount)
            {
                MissionReward reward;
                if (!Rewards.TryGetValue(name, out reward))
                {
                    reward = new MissionReward(name);
                    Rewards.Add(name, reward);
                }
                reward.Add(amount);
            }

            public void AddSystemInfluence(long systemAddress, int amount, ulong missionId)
            {
                SystemInfluence si;
                if (!SystemsWithInfluence.TryGetValue(systemAddress, out si))
                {
                    si = new SystemInfluence(systemAddress, amount, missionId);
                    SystemsWithInfluence.Add(systemAddress, si);
                    //System.Diagnostics.Debug.WriteLine($"Faction sys influence made for {systemAddress}");
                }
                else
                {
                    si.AddInfluence(amount, missionId);
                }
            }
        }
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

            dataGridViewFactions.MakeDoubleBuffered();

            for (int col = 1; col < dataGridViewFactions.ColumnCount - 1; col++)
                dataGridViewFactions.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            startDateTimePicker.Value = GetSetting("StartDate", DateTime.UtcNow).StartOfDay();
            startDateTimePicker.Checked = GetSetting("StartDateChecked", false);
            endDateTimePicker.Value = GetSetting("EndDate", DateTime.UtcNow.EndOfDay()).EndOfDay();
            endDateTimePicker.Checked = GetSetting("EndDateChecked", false);
            VerifyDates();
            startDateTimePicker.ValueChanged += new System.EventHandler(this.startDateTime_ValueChanged);        // now install the change handlers
            endDateTimePicker.ValueChanged += new System.EventHandler(this.endDateTime_ValueChanged);

            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;

            dataGridViewFactions.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewFactions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            var enumlist = new Enum[] { EDTx.UserControlFactions_colFaction, EDTx.UserControlFactions_colMissions, EDTx.UserControlFactions_colInfluence, EDTx.UserControlFactions_colReputation, EDTx.UserControlFactions_colMissionCredits, EDTx.UserControlFactions_CBought, EDTx.UserControlFactions_CSold, EDTx.UserControlFactions_CProfit, EDTx.UserControlFactions_MBought, EDTx.UserControlFactions_MSold, EDTx.UserControlFactions_CrimeCommitted, EDTx.UserControlFactions_BountyKills, EDTx.UserControlFactions_BountyValue, EDTx.UserControlFactions_BountyRewardsValue, EDTx.UserControlFactions_Interdicted, EDTx.UserControlFactions_Interdiction, EDTx.UserControlFactions_KillBondVictim, EDTx.UserControlFactions_KillBondsAward, EDTx.UserControlFactions_KillBondsValue, EDTx.UserControlFactions_colInfo, EDTx.UserControlFactions_labelTo, EDTx.UserControlFactions_CartoValue };
            var enumlistcms = new Enum[] { EDTx.UserControlFactions_showMissionsForFactionToolStripMenuItem, EDTx.UserControlFactions_showCommoditymaterialTradesForFactionToolStripMenuItem, EDTx.UserControlFactions_showBountiesAndBondsForFactionToolStripMenuItem, EDTx.UserControlFactions_showFactionSystemDetailToolStripMenuItem };
            var enumlisttt = new Enum[] { EDTx.UserControlFactions_startDateTime_ToolTip, EDTx.UserControlFactions_endDateTime_ToolTip };

            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            BaseUtils.Translator.Instance.TranslateToolstrip(contextMenuStrip, enumlistcms, this);
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            labelValue.Text = "";
        }


        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewFactions);
        }

        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewFactions);

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
                NextExpiryUTC = ml.OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? EDDConfig.GameEndTimeUTC();
                //System.Diagnostics.Debug.WriteLine($"Faction List recalc {he.EventTimeUTC} {he.MissionList} vs {last_he?.MissionList}, next expiry time {NextExpiryUTC}");
            }
        }

        #endregion

        #region Display

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridViewFactions.SortedColumn != null ? dataGridViewFactions.SortedColumn : dataGridViewFactions.Columns[0];
            SortOrder sortorder = dataGridViewFactions.SortOrder != SortOrder.None ? dataGridViewFactions.SortOrder : SortOrder.Ascending;
            string toprowfaction = dataGridViewFactions.SafeFirstDisplayedScrollingRowIndex() >= 0 ? (dataGridViewFactions.Rows[dataGridViewFactions.SafeFirstDisplayedScrollingRowIndex()].Tag as FactionStatistics).Name : "";

            dataGridViewFactions.Rows.Clear();

            DateTime startdateutc = startDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(startDateTimePicker.Value) : EDDConfig.GameLaunchTimeUTC();
            DateTime enddateutc = endDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(endDateTimePicker.Value) : EDDConfig.GameEndTimeUTC();

            // this accumulates factions info
            var factionslist = new Dictionary<string, FactionStatistics>();

            // first do the mission lists and accumulate into factionslist

            List<MissionState> ml = DiscoveryForm.History.MissionListAccumulator.GetMissionList(last_he?.MissionList ?? 0);
            if (ml != null)
            {
                foreach (MissionState ms in ml)
                {
                    bool withinstarttime = DateTime.Compare(ms.Mission.EventTimeUTC, startdateutc) >= 0 && DateTime.Compare(ms.Mission.EventTimeUTC, enddateutc) <= 0;
                    bool withinexpirytime = ms.Mission.ExpiryValid && DateTime.Compare(ms.Mission.Expiry, startdateutc) >= 0 && DateTime.Compare(ms.Mission.Expiry, enddateutc) <= 0;
                    bool withincompletetime = ms.Completed != null && DateTime.Compare(ms.Completed.EventTimeUTC, startdateutc) >= 0 && DateTime.Compare(ms.Completed.EventTimeUTC, enddateutc) <= 0;

                    if ( withinstarttime || withincompletetime)
                    {
                        var faction = ms.Mission.Faction;
                        FactionStatistics factionStats;
                        if (!factionslist.TryGetValue(faction, out factionStats))   // is faction present? if not create
                        {
                            factionStats = new FactionStatistics(faction);
                            factionslist.Add(faction, factionStats);
                        }

                        if (ms.Completed != null)           // effects/rewards are dependent on completion
                        {
                            factionStats.AddMissions(1);        // 1 more mission, 

                            if (ms.Completed.FactionEffects != null)
                            {
                                foreach (var fe in ms.Completed.FactionEffects)
                                {
                                    if (fe.Faction == faction)
                                    {
                                        if (fe.ReputationTrend == "UpGood" && fe.Reputation?.Length > 0)
                                        {
                                            factionStats.AddReputation(fe.Reputation.Length);
                                        }

                                        foreach (var si in fe.Influence)
                                        {
                                            if (si.Trend == "UpGood" && si.Influence?.Length > 0)
                                            {
                                                factionStats.AddInfluence(si.Influence.Length);
                                                factionStats.AddSystemInfluence(si.SystemAddress, si.Influence.Length, ms.Completed.MissionId);
                                            }
                                        }
                                    }
                                }
                            }
                            long credits = ms.Completed.Reward != null ? (long)ms.Completed.Reward : 0;
                            if (credits > 0)
                            {
                                factionStats.AddCredits(credits);
                            }
                            if (ms.Completed.MaterialsReward != null)
                            {
                                foreach (var mr in ms.Completed.MaterialsReward)
                                {
                                    factionStats.AddReward(mr.Name_Localised, mr.Count);
                                }
                            }
                            if (ms.Completed.CommodityReward != null)
                            {
                                foreach (var cr in ms.Completed.CommodityReward)
                                {
                                    factionStats.AddReward(cr.Name_Localised, cr.Count);
                                }
                            }
                        }
                        else if ( withinexpirytime && ms.State == MissionState.StateTypes.InProgress)
                        {
                            factionStats.AddMissionsInProgress(1);
                        }
                    }

                }
            }

            // now either pick up the factioninfo stats accumulated by history (if no date range) or recalculate if date range

            Dictionary<string,Stats.FactionInfo> factioninfo = null;

            if (startDateTimePicker.Checked || endDateTimePicker.Checked)                           
            {
                Stats stats = new Stats();      // reprocess this list completely

                foreach (var he in HistoryList.FilterByDateRange(DiscoveryForm.History.EntryOrder(), startdateutc, enddateutc))
                {
                    stats.Process(he.journalEntry, he.Status.StationFaction);
                }

                factioninfo = stats.GetLastEntries(); // pick the last generation in there.
            }
            else
            {
                factioninfo = DiscoveryForm.History.GetStatsAtGeneration(last_he?.Statistics ?? 0);
            }

            // if we have some stats on factions accumulated via the history, add to the faction list

            if (factioninfo != null)
            {
                foreach (var fkvp in factioninfo)
                {
                    if (!factionslist.TryGetValue(fkvp.Value.Faction, out FactionStatistics factionStats))  // is faction present? if not create
                    {
                        factionStats = new FactionStatistics(fkvp.Value.Faction);
                        factionslist.Add(fkvp.Value.Faction, factionStats);
                    }

                    factionslist[fkvp.Value.Faction].AddFactionStats(fkvp.Value);
                }
            }

            // now update the grid with faction info

            if (factionslist.Count > 0)
            {
                var rows = new List<DataGridViewRow>();
                foreach (FactionStatistics fs in factionslist.Values)
                {
                    var info = "";

                    if (fs.MissionsInProgress > 0)
                        info = info.AppendPrePad("Missions In Progress:".T(EDTx.UserControlFactions_MissionsInProgress) + " " + fs.MissionsInProgress , ", ");

                    foreach (var reward in fs.Rewards.Values)
                    {
                        info = info.AppendPrePad(reward.Count + " " + reward.Name, ", ");
                    }

                    if (fs.FactionStats.CapShipAwardAsVictimFaction > 0 )
                        info = info.AppendPrePad("Capital ship Victims: ".T(EDTx.UserControlFactions_CapShipVictims) + fs.FactionStats.CapShipAwardAsVictimFaction, ", ");
                    if (fs.FactionStats.CapShipAwardAsAwaringFaction > 0 )
                        info = info.AppendPrePad("Capital ship Award: ".T(EDTx.UserControlFactions_CapShipAward) + fs.FactionStats.CapShipAwardAsAwaringFaction + ":" + fs.FactionStats.CapShipAwardAsAwaringFactionValue.ToString("N0") + "cr", ", ");

                    object[] rowobj = { fs.Name,
                                        fs.Missions.ToString("N0"), fs.Influence.ToString("N0"), fs.Reputation.ToString("N0"), fs.Credits.ToString("N0"),
                                        fs.FactionStats.BoughtCommodity.ToString("N0"), fs.FactionStats.SoldCommodity.ToString("N0"),fs.FactionStats.ProfitCommodity.ToString("N0"),
                                        fs.FactionStats.BoughtMaterial.ToString("N0"), fs.FactionStats.SoldMaterial.ToString("N0"),
                                        fs.FactionStats.CrimesAgainst.ToString("N0") ,
                                        fs.FactionStats.BountyKill.ToString("N0"), fs.FactionStats.BountyRewards.ToString("N0"), fs.FactionStats.BountyRewardsValue.ToString("N0"),
                                        fs.FactionStats.Interdicted.ToString("N0"), fs.FactionStats.Interdiction.ToString("N0"),
                                        fs.FactionStats.KillBondAwardAsVictimFaction.ToString("N0"), fs.FactionStats.KillBondAwardAsAwaringFaction.ToString("N0"), fs.FactionStats.KillBondAwardAsAwaringFactionValue.ToString("N0"),
                                        fs.FactionStats.CartographicDataSold.ToString("N0"),
                                        info };

                    var row = dataGridViewFactions.RowTemplate.Clone() as DataGridViewRow;
                    row.CreateCells(dataGridViewFactions, rowobj);
                    row.Tag = fs;
                    rows.Add(row);
                }

                dataGridViewFactions.Rows.AddRange(rows.ToArray());

                dataGridViewFactions.Sort(sortcol, (sortorder == SortOrder.Descending) ? System.ComponentModel.ListSortDirection.Descending : System.ComponentModel.ListSortDirection.Ascending);
                dataGridViewFactions.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

                if (toprowfaction.HasChars())
                {
                    for( int i = 0; i < dataGridViewFactions.RowCount; i++ )
                    {
                        var fs = dataGridViewFactions.Rows[i].Tag as FactionStatistics;
                        if ( fs.Name == toprowfaction )
                        {
                            dataGridViewFactions.SafeFirstDisplayedScrollingRowIndex(i);
                            break;
                        }
                    }
                }
            }

            labelValue.Text = factionslist.Count + " " + "Factions".T(EDTx.UserControlFactions_FactionsPlural);
        }

        #endregion

        #region Misc

        private void VerifyDates()
        {
            if (!EDDConfig.Instance.DateTimeInRangeForGame(startDateTimePicker.Value) || !EDDConfig.Instance.DateTimeInRangeForGame(endDateTimePicker.Value))
            {
                startDateTimePicker.Checked = endDateTimePicker.Checked = false;
                startDateTimePicker.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EDDConfig.GameLaunchTimeUTC());
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
            if (e.Column.Index >= 1 && e.Column.Index <= dataGridViewFactions.ColumnCount-2)
            {
                e.SortDataGridViewColumnNumeric();
            }
        }

        #endregion

        #region Right clicks

        private void showMissionsForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int row = dataGridViewFactions.RightClickRow;

            if (row >= 0)
            {
                FactionStatistics fs = dataGridViewFactions.Rows[row].Tag as FactionStatistics;

                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                MissionListUserControl mluc = new MissionListUserControl();

                mluc.Clear();
                List<MissionState> ml = DiscoveryForm.History.MissionListAccumulator.GetMissionList(last_he?.MissionList ?? 0);

                DateTime startdateutc = startDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(startDateTimePicker.Value) : EDDConfig.GameLaunchTimeUTC();
                DateTime enddateutc = endDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(endDateTimePicker.Value) : EDDConfig.GameEndTimeUTC();

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

                    mluc.Finish();
                }

                DGVLoadColumnLayout(mluc.dataGridView, "ShowMission");

                f.Add(new ExtendedControls.ConfigurableForm.Entry(mluc, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
                { anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });

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
                DateTime startdateutc = startDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(startDateTimePicker.Value) : EDDConfig.GameLaunchTimeUTC();
                DateTime enddateutc = endDateTimePicker.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(endDateTimePicker.Value) : EDDConfig.GameEndTimeUTC();
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
            if (dataGridViewFactions.RightClickRow >= 0)
            {
                FactionStatistics fs = dataGridViewFactions.Rows[dataGridViewFactions.RightClickRow].Tag as FactionStatistics;

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
                        string name = m?.Name ?? i.FDName;

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
                { anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
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
            if (dataGridViewFactions.RightClickRow >= 0)
            {
                FactionStatistics fs = dataGridViewFactions.Rows[dataGridViewFactions.RightClickRow].Tag as FactionStatistics;

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
                { anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
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
            if (dataGridViewFactions.RightClickRow >= 0)
            {
                FactionStatistics fs = dataGridViewFactions.Rows[dataGridViewFactions.RightClickRow].Tag as FactionStatistics;

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

                foreach (var si in fs.SystemsWithInfluence.Values)
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
                { anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
                f.AddOK(new Point(960 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();
                f.AllowResize = true;

                f.ShowDialogCentred(FindForm(), FindForm().Icon, "Systems Detail for ".T(EDTx.UserControlFactions_SystemsDetailFor) + fs.Name, closeicon: true);

                DGVSaveColumnLayout(dgvpanel.DataGrid, "ShowSystemDetail");
            }
        }

        #endregion
    }
}
