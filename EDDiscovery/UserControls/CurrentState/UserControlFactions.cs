/*
 * Copyright © 2020 EDDiscovery development team
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
            private List<int> MissionsList;

            public SystemInfluence(long systemAddress, int influence, int missionId)
            {
                this.SystemAddress = systemAddress;
                this.Influence = influence;
                this.MissionsList = new List<int>();
                this.MissionsList.Add(missionId);
            }

            public void AddInfluence(int influence, int missionId)
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
            public int Influence { get; private set; }
            public int Reputation { get; private set; }
            public long Credits { get; private set; }
            public Dictionary<string, MissionReward> Rewards { get; }
            public Dictionary<long, SystemInfluence> Systems { get; }
            public Stats.FactionInfo FactionStats { get; private set; }

            public FactionStatistics(string name)
            {
                Name = name;
                Missions = 0;
                Influence = 0;
                Reputation = 0;
                Credits = 0;
                Rewards = new Dictionary<string, MissionReward>();
                this.Systems = new Dictionary<long, SystemInfluence>();
                FactionStats = defstats;
            }

            private static Stats.FactionInfo defstats = new Stats.FactionInfo("-");

            public void AddMissions(int amount)
            {
                Missions += amount;
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

            public void AddSystemInfluence(long systemAddress, int amount, int missionId)
            {
                SystemInfluence si;
                if (!Systems.TryGetValue(systemAddress, out si))
                {
                    si = new SystemInfluence(systemAddress, amount, missionId);
                    Systems.Add(systemAddress, si);
                }
                else
                {
                    si.AddInfluence(amount, missionId);
                }
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

            public void AddCommoditiesSold(int a) { CommoditiesSold = CommoditiesSold ?? 0 - a; }
            public void AddCommoditiesBought(int a) { CommoditiesSold = CommoditiesSold ?? 0 + a; }
            public void AddMaterialsSold(int a) { MaterialsSold = MaterialsSold??0 - a; }
            public void AddMaterialsBought(int a) { MaterialsBought = MaterialsBought??0 - a; }
            public void AddBounties(int a) { Bounties = Bounties??0 +a; }
            public void AddBountyRewardsValue(long a) { BountyRewardsValue = BountyRewardsValue??0 + a; }
            public void AddKillBonds(int a) { KillBonds = KillBonds??0 + a; }
            public void AddBondsRewardsValue(long a) { BondsRewardsValue = BondsRewardsValue??0 + a; }
        }

        private string DbColumnSaveFactions { get { return DBName("FactionsPanel", "DGVCol"); } }
        private string DbStartDate { get { return DBName("FactionsPanelStartDate"); } }
        private string DbEndDate { get { return DBName("FactionsPanelEndDate"); } }
        private string DbStartDateChecked { get { return DBName("FactionsPanelStartDateCheck"); } }
        private string DbEndDateChecked { get { return DBName("FactionsPanelEndDateCheck"); } }
        private DateTime NextExpiry;
        private HistoryEntry last_he = null;

        #region Init

        public UserControlFactions()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            dataGridViewFactions.MakeDoubleBuffered();

            for (int col = 1; col < dataGridViewFactions.ColumnCount - 1; col++)
                dataGridViewFactions.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            startDateTime.Value = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDate(DbStartDate, DateTime.UtcNow);
            startDateTime.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbStartDateChecked, false);
            endDateTime.Value = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDate(DbEndDate, DateTime.UtcNow);
            endDateTime.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbEndDateChecked, false);
            VerifyDates();

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

            dataGridViewFactions.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewFactions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;      

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Discoveryform_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Discoveryform_OnTravelSelectionChanged;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Discoveryform_OnTravelSelectionChanged;
            DGVLoadColumnLayout(dataGridViewFactions, DbColumnSaveFactions);
        }

        public override void InitialDisplay()
        {
            Discoveryform_OnTravelSelectionChanged(uctg.GetCurrentHistoryEntry, discoveryform.history, true);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewFactions, DbColumnSaveFactions);

            uctg.OnTravelSelectionChanged -= Discoveryform_OnTravelSelectionChanged;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDate(DbStartDate, EDDiscoveryForm.EDDConfig.ConvertTimeToUTCFromSelected(startDateTime.Value));
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDate(DbEndDate, EDDiscoveryForm.EDDConfig.ConvertTimeToUTCFromSelected(endDateTime.Value));

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbStartDateChecked, startDateTime.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbEndDateChecked, endDateTime.Checked);
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)     // may have changed date system, this causes this
        {
            VerifyDates();
        }

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            if (!object.ReferenceEquals(he.MissionList, last_he?.MissionList) || he.EventTimeUTC > NextExpiry)
            {
                last_he = he;
                Display();
                NextExpiry = he?.MissionList?.GetAllCurrentMissions(he.EventTimeUTC).OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? DateTime.MaxValue;
            }
        }

        private void Discoveryform_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            last_he = he;
            Display();
            NextExpiry = he?.MissionList?.GetAllCurrentMissions(he.EventTimeUTC).OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? DateTime.MaxValue;
        }

        #endregion

        #region Display

        private void Display()
        {
            DataGridViewColumn sortcol = dataGridViewFactions.SortedColumn != null ? dataGridViewFactions.SortedColumn : dataGridViewFactions.Columns[0];
            SortOrder sortorder = dataGridViewFactions.SortOrder != SortOrder.None ? dataGridViewFactions.SortOrder : SortOrder.Ascending;
            string toprowfaction = dataGridViewFactions.FirstDisplayedScrollingRowIndex >= 0 ? (dataGridViewFactions.Rows[dataGridViewFactions.FirstDisplayedScrollingRowIndex].Tag as FactionStatistics).Name : "";

            dataGridViewFactions.Rows.Clear();

            MissionList ml = last_he?.MissionList;

            DateTime startdateutc = startDateTime.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(startDateTime.Value) : new DateTime(1980, 1, 1);
            DateTime enddateutc = endDateTime.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(endDateTime.Value) : new DateTime(8999, 1, 1);

            var factionslist = new Dictionary<string, FactionStatistics>();

            if (ml != null)
            {
                foreach (MissionState ms in ml.Missions.Values)
                {
                    if (ms.State == MissionState.StateTypes.Completed && ms.Completed != null)
                    {
                        if (DateTime.Compare(ms.Completed.EventTimeUTC, startdateutc) >= 0 &&
                            DateTime.Compare(ms.Completed.EventTimeUTC, enddateutc) <= 0)
                        {
                            var faction = ms.Mission.Faction;
                            FactionStatistics factionStats;
                            if (!factionslist.TryGetValue(faction, out factionStats))
                            {
                                factionStats = new FactionStatistics(faction);
                                factionslist.Add(faction, factionStats);
                            }
                            factionStats.AddMissions(1);
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
                    }
                }
            }

            Stats mcs = null;

            if (startDateTime.Checked || endDateTime.Checked)                           // if we have a date range, can't rely on stats accumulated automatically
            {
                foreach (var he in HistoryList.FilterByDateRange(discoveryform.history.EntryOrder(), startdateutc, enddateutc))
                {
                    mcs = Stats.Process(he.journalEntry, mcs, he.StationFaction);
                }
            }
            else
            {
                mcs = last_he?.Stats;
            }

            if (mcs != null)
            {
                foreach (var fkvp in mcs.FactionInformation)
                {
                    if (!factionslist.TryGetValue(fkvp.Value.Faction, out FactionStatistics factionStats))
                    {
                        factionStats = new FactionStatistics(fkvp.Value.Faction);
                        factionslist.Add(fkvp.Value.Faction, factionStats);
                    }

                    factionslist[fkvp.Value.Faction].AddFactionStats(fkvp.Value);
                }
            }

            if (factionslist.Count > 0)
            {
                var rows = new List<DataGridViewRow>();
                foreach (FactionStatistics fs in factionslist.Values)
                {
                    var info = "";
                    foreach (var reward in fs.Rewards.Values)
                    {
                        info = info.AppendPrePad(reward.Count + " " + reward.Name, ", ");
                    }

                    if (fs.FactionStats.CapShipAwardAsVictimFaction > 0)
                        info = info.AppendPrePad("Capital ship Victims: ".T(EDTx.UserControlFactions_CapShipVictims) + fs.FactionStats.CapShipAwardAsVictimFaction, ", ");
                    if (fs.FactionStats.CapShipAwardAsAwaringFaction > 0)
                        info = info.AppendPrePad("Capital ship Award: ".T(EDTx.UserControlFactions_CapShipAward) + fs.FactionStats.CapShipAwardAsAwaringFaction + ":" + fs.FactionStats.CapShipAwardAsAwaringFactionValue.ToString("N0") + "cr", ", ");

                    object[] rowobj = { fs.Name,
                                        fs.Missions.ToString("N0"), fs.Influence.ToString("N0"), fs.Reputation.ToString("N0"), fs.Credits.ToString("N0"),
                                        fs.FactionStats.BoughtCommodity.ToString("N0"), fs.FactionStats.SoldCommodity.ToString("N0"), fs.FactionStats.BoughtMaterial.ToString("N0"), fs.FactionStats.SoldMaterial.ToString("N0"),
                                        fs.FactionStats.CrimesAgainst.ToString("N0") ,
                                        fs.FactionStats.BountyKill.ToString("N0"), fs.FactionStats.BountyRewards.ToString("N0"), fs.FactionStats.BountyRewardsValue.ToString("N0"),
                                        fs.FactionStats.Interdicted.ToString("N0"), fs.FactionStats.Interdiction.ToString("N0"),
                                        fs.FactionStats.KillBondAwardAsVictimFaction.ToString("N0"), fs.FactionStats.KillBondAwardAsAwaringFaction.ToString("N0"), fs.FactionStats.KillBondAwardAsAwaringFactionValue.ToString("N0"),
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
            if (!EDDConfig.Instance.DateTimeInRangeForGame(startDateTime.Value) || !EDDConfig.Instance.DateTimeInRangeForGame(endDateTime.Value))
            {
                startDateTime.Checked = endDateTime.Checked = false;
                endDateTime.Value = startDateTime.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
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
                MissionList ml = last_he?.MissionList;

                DateTime startdateutc = startDateTime.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(startDateTime.Value) : new DateTime(1980, 1, 1);
                DateTime enddateutc = endDateTime.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(endDateTime.Value) : new DateTime(8999, 1, 1);

                if (ml != null)
                {
                    foreach (MissionState ms in ml.Missions.Values)
                    {
                        if (ms.State == MissionState.StateTypes.Completed && ms.Completed != null)
                        {
                            if (DateTime.Compare(ms.Completed.EventTimeUTC, startdateutc) >= 0 &&
                                DateTime.Compare(ms.Completed.EventTimeUTC, enddateutc) <= 0)
                            {
                                var faction = ms.Mission.Faction;
                                if (faction == fs.Name)
                                    mluc.Add(ms, true);
                            }
                        }
                    }

                    mluc.Finish();
                }

                string keyname = "UserControlFactionsShowMission";
                mluc.dataGridView.LoadColumnSettings(keyname, (a) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(a, int.MinValue),
                                                                          (b) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(b, double.MinValue));

                f.Add(new ExtendedControls.ConfigurableForm.Entry(mluc, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
                { anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });

                f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();

                f.AllowResize = true;

                f.ShowDialogCentred(FindForm(), FindForm().Icon, "Missions for ".T(EDTx.UserControlFactions_MissionsFor) + fs.Name, closeicon: true);

                mluc.dataGridView.SaveColumnSettings(keyname, (a, b) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(a, b),
                                                 (c, d) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(c, d));

            }
        }

        private List<HistoryEntry> FilterHistory(Predicate<HistoryEntry> where)
        {
            if (last_he != null)
            {
                DateTime startdateutc = startDateTime.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(startDateTime.Value) : new DateTime(1980, 1, 1);
                DateTime enddateutc = endDateTime.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(endDateTime.Value) : new DateTime(8999, 1, 1);
                return HistoryList.FilterBefore(discoveryform.history.EntryOrder(), last_he, 
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

                var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnHider>();
                dgvpanel.DataGrid.CreateTextColumns("Date".T(EDTx.UserControlOutfitting_Date), 100, 5,
                                                    "Item".T(EDTx.UserControlFactions_Item), 150, 5,
                                                    "Bought".T(EDTx.UserControlStats_GoodsBought), 50, 5,
                                                    "Sold".T(EDTx.UserControlStats_GoodsSold), 50, 5);
                dgvpanel.DataGrid.SortCompare += (s, ev) => { if (ev.Column.Index >= 2) ev.SortDataGridViewColumnNumeric(); };
                dgvpanel.DataGrid.RowHeadersVisible = false;
                dgvpanel.DataGrid.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvpanel.DataGrid.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                string keyname = "UserControlFactionsShowCommdsMats";
                dgvpanel.DataGrid.LoadColumnSettings(keyname, (a) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(a, int.MinValue),
                                                                          (b) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(b, double.MinValue));

                foreach (var he in FilterHistory((x) => x.journalEntry is IStatsJournalEntryMatCommod && x.StationFaction == fs.Name))
                {
                    var items = (he.journalEntry as IStatsJournalEntryMatCommod).ItemsList;
                    foreach (var i in items)
                    {
                        var m = EliteDangerousCore.MaterialCommodityData.GetByFDName(i.Item1);     // and we see if we actually have some at this time
                        string name = m?.Name ?? i.Item1;

                        int bought = i.Item2 > 0 ? i.Item2 : 0;
                        int sold = i.Item2 < 0 ? -i.Item2 : 0;

                        object[] rowobj = { EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
                                            name,
                                            bought.ToString("N0"),
                                            sold.ToString("N0") };
                        var row = dgvpanel.DataGrid.RowTemplate.Clone() as DataGridViewRow;
                        row.CreateCells(dgvpanel.DataGrid, rowobj);
                        dgvpanel.DataGrid.Rows.Add(row);
                    }
                }

                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                f.Add(new ExtendedControls.ConfigurableForm.Entry(dgvpanel, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
                { anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });
                f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);
                f.InstallStandardTriggers();
                f.AllowResize = true;

                f.ShowDialogCentred(FindForm(), FindForm().Icon, "Materials/Commodities for ".T(EDTx.UserControlFactions_MaterialCommodsFor) + fs.Name, closeicon: true);

                dgvpanel.DataGrid.SaveColumnSettings(keyname, (a, b) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(a, b),
                                                 (c, d) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(c, d));

            }
        }

        private void showBountiesAndBondsForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewFactions.RightClickRow >= 0)
            {
                FactionStatistics fs = dataGridViewFactions.Rows[dataGridViewFactions.RightClickRow].Tag as FactionStatistics;

                var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnHider>();
                dgvpanel.DataGrid.CreateTextColumns("Date".T(EDTx.UserControlOutfitting_Date), 100, 5,
                                                    "Bounty/Bond".T(EDTx.UserControlFactions_BountyBond), 80, 5,
                                                    "Target".T(EDTx.UserControlFactions_Target), 150, 5,
                                                    "Target Faction".T(EDTx.UserControlFactions_TargetFaction), 150, 5,
                                                    "Reward".T(EDTx.UserControlFactions_Reward), 60, 5);
                dgvpanel.DataGrid.SortCompare += (s, ev) => { if (ev.Column.Index >= 4) ev.SortDataGridViewColumnNumeric(); };
                dgvpanel.DataGrid.RowHeadersVisible = false;
                dgvpanel.DataGrid.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                string keyname = "UserControlFactionsShowBonds";
                dgvpanel.DataGrid.LoadColumnSettings(keyname, (a) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(a, int.MinValue),
                                                                          (b) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(b, double.MinValue));


                foreach (var he in FilterHistory((x) => x.journalEntry is IStatsJournalEntryBountyOrBond &&
                                                    (x.journalEntry as IStatsJournalEntryBountyOrBond).HasFaction(fs.Name)))
                {
                    var bb = he.journalEntry as IStatsJournalEntryBountyOrBond;
                    object[] rowobj = { EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
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

                dgvpanel.DataGrid.SaveColumnSettings(keyname, (a, b) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(a, b),
                                                 (c, d) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(c, d));
            }
        }

        private void showFactionSystemDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridViewFactions.RightClickRow >= 0)
            {
                FactionStatistics fs = dataGridViewFactions.Rows[dataGridViewFactions.RightClickRow].Tag as FactionStatistics;

                var dgvpanel = new ExtendedControls.ExtPanelDataGridViewScrollWithDGV<BaseUtils.DataGridViewColumnHider>();
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
                                                    "Rewards".T(EDTx.UserControlFactions_RewardsPlural), 60, 5);
                dgvpanel.DataGrid.SortCompare += (s, ev) => { if (ev.Column.Index >= 1) ev.SortDataGridViewColumnNumeric(); };
                dgvpanel.DataGrid.RowHeadersVisible = false;
                dgvpanel.DataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                for (int col = 1; col < dgvpanel.DataGrid.ColumnCount - 1; col++)
                    dgvpanel.DataGrid.Columns[col].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                string keyname = "UserControlFactionsShowSystemDetail";
                dgvpanel.DataGrid.LoadColumnSettings(keyname, (a) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(a, int.MinValue),
                                                                          (b) => EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(b, double.MinValue));

                var systems = new List<SystemInfo>();
                foreach (var si in fs.Systems.Values)
                {
                    string systemName = null;
                    if (last_he != null)
                    {
                        foreach (var he in HistoryList.FilterBefore(discoveryform.history.EntryOrder(), last_he, 
                                    (x) => x.System.SystemAddress == si.SystemAddress))
                        {
                            systemName = he.System.Name;
                            break;
                        }
                    }
                    systems.Add(new SystemInfo { Name = systemName, Address = si.SystemAddress, Missions = si.Missions, Influence = si.Influence });
                }

                var list = FilterHistory((x) => (x.journalEntry is IStatsJournalEntryMatCommod && x.StationFaction == fs.Name) ||
                                                (x.journalEntry is IStatsJournalEntryBountyOrBond &&
                                                 (x.journalEntry as IStatsJournalEntryBountyOrBond).HasFaction(fs.Name)));
                foreach (var he in list)
                {
                    SystemInfo si = systems.Find(x =>
                        (he.System.SystemAddress != null && x.Address == he.System.SystemAddress) ||
                        (he.System.Name != null && x.Name == he.System.Name));
                    if (si == null)
                    {
                        si = new SystemInfo { Name = he.System.Name, Address = he.System.SystemAddress };
                        systems.Add(si);
                    }
                    if (he.journalEntry is IStatsJournalEntryMatCommod)
                    {
                        var items = (he.journalEntry as IStatsJournalEntryMatCommod).ItemsList;
                        foreach (var i in items)
                        {
                            if (he.journalEntry.EventTypeID == JournalTypeEnum.MaterialTrade)
                            {
                                if (i.Item2 > 0)
                                {
                                    si.AddMaterialsBought(i.Item2);
                                }
                                else if (i.Item2 < 0)
                                {
                                    si.AddMaterialsSold(i.Item2);
                                }
                            }
                            else
                            {
                                if (i.Item2 > 0)
                                {
                                    si.AddCommoditiesBought(i.Item2);
                                }
                                else if (i.Item2 < 0)
                                {
                                    si.AddCommoditiesSold(i.Item2);
                                }
                            }
                        }
                    }
                    else
                    {
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
                                        system.BondsRewardsValue?.ToString("N0") };
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

                dgvpanel.DataGrid.SaveColumnSettings(keyname, (a, b) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(a, b),
                                                 (c, d) => EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(c, d));
            }
        }

        #endregion
    }
}
