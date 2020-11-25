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
 
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EDDiscovery.Controls;
using EDDiscovery.UserControls.Helpers;
using EliteDangerousCore;

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

        private class FactionStatistics
        {
            public string Name { get; }
            public int Missions { get; private set; }
            public int Influence { get; private set; }
            public int Reputation { get; private set; }
            public long Credits { get; private set; }
            public Dictionary<string, MissionReward> Rewards { get; }
            public Stats.FactionInfo FactionStats { get; private set; }

            public FactionStatistics(string name)
            {
                Name = name;
                Missions = 0;
                Influence = 0;
                Reputation = 0;
                Credits = 0;
                Rewards = new Dictionary<string, MissionReward>();
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
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewFactions, DbColumnSaveFactions);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewFactions, DbColumnSaveFactions);

            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDate(DbStartDate, EDDiscoveryForm.EDDConfig.ConvertTimeToUTCFromSelected(startDateTime.Value));
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDate(DbEndDate, EDDiscoveryForm.EDDConfig.ConvertTimeToUTCFromSelected(endDateTime.Value));

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbStartDateChecked, startDateTime.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbEndDateChecked, endDateTime.Checked);
        }

        #endregion

        #region Display

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        private void Display(HistoryEntry he, HistoryList hl) =>
            Display(he, hl, true);

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            last_he = he;
            Display();
            NextExpiry = he?.MissionList?.GetAllCurrentMissions(he.EventTimeUTC).OrderBy(e => e.MissionEndTime).FirstOrDefault()?.MissionEndTime ?? DateTime.MaxValue;
        }

        private void Display()
        {
            var factionslist = new Dictionary<string, FactionStatistics>();

            DataGridViewColumn sortcol = dataGridViewFactions.SortedColumn != null ? dataGridViewFactions.SortedColumn : dataGridViewFactions.Columns[0];
            SortOrder sortorder = dataGridViewFactions.SortOrder != SortOrder.None ? dataGridViewFactions.SortOrder : SortOrder.Ascending;
            string toprowfaction = dataGridViewFactions.FirstDisplayedScrollingRowIndex >= 0 ? (dataGridViewFactions.Rows[dataGridViewFactions.FirstDisplayedScrollingRowIndex].Tag as FactionStatistics).Name : "";
            System.Diagnostics.Debug.WriteLine("Current first row " + toprowfaction);

            dataGridViewFactions.Rows.Clear();

            MissionList ml = last_he?.MissionList;

            if (ml != null)
            {
                foreach (MissionState ms in ml.Missions.Values)
                {
                    if (ms.State == MissionState.StateTypes.Completed && ms.Completed != null)
                    {
                        DateTime startdateutc = startDateTime.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(startDateTime.Value) : new DateTime(1980, 1, 1);
                        DateTime enddateutc = endDateTime.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(endDateTime.Value) : new DateTime(8999, 1, 1);

                        if (DateTime.Compare(ms.Completed.EventTimeUTC, startdateutc) >= 0 &&
                            DateTime.Compare(ms.Completed.EventTimeUTC, enddateutc) <= 0)
                        {
                            //      System.Diagnostics.Debug.WriteLine(ms.Mission.Faction + " " + ms.Mission.Name + " " + ms.Completed.EventTimeUTC);
                            var faction = ms.Mission.Faction;
                            int inf = 0;
                            int rep = 0;
                            if (ms.Completed.FactionEffects != null)
                            {
                                foreach (var fe in ms.Completed.FactionEffects)
                                {
                                    if (fe.Faction == faction)
                                    {
                                        if (fe.ReputationTrend == "UpGood" && fe.Reputation?.Length > 0)
                                        {
                                            rep = fe.Reputation.Length;
                                        }

                                        foreach (var si in fe.Influence)
                                        {
                                            if (si.Trend == "UpGood" && si.Influence?.Length > 0)
                                            {
                                                inf += si.Influence.Length;
                                            }
                                        }
                                    }
                                }
                            }
                            long credits = ms.Completed.Reward != null ? (long)ms.Completed.Reward : 0;

                            if (!factionslist.TryGetValue(faction, out FactionStatistics factionStats))
                            {
                                factionStats = new FactionStatistics(faction);
                                factionslist.Add(faction, factionStats);
                            }

                            factionStats.AddMissions(1);

                            if (inf > 0)
                            {
                                factionStats.AddInfluence(inf);
                            }
                            if (rep > 0)
                            {
                                factionStats.AddReputation(rep);
                            }
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

            Stats mcs = last_he?.Stats;

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
                //System.Diagnostics.Debug.WriteLine(total + " missions");

                var rows = new List<DataGridViewRow>();
                foreach (FactionStatistics fs in factionslist.Values)
                {
                    var info = "";
                    foreach (var reward in fs.Rewards.Values)
                    {
                        if (info.Length > 0)
                        {
                            info += ", ";
                        }
                        info += reward.Count + " " + reward.Name;
                    }

                    object[] rowobj = { fs.Name,
                                        fs.Missions, fs.Influence, fs.Reputation, String.Format("{0:n0}", fs.Credits),
                                        fs.FactionStats.BoughtCommodity, fs.FactionStats.SoldCommodity, fs.FactionStats.BoughtMaterial, fs.FactionStats.SoldMaterial,
                                        fs.FactionStats.CrimesAgainst ,
                                        fs.FactionStats.BountyKill, fs.FactionStats.BountyRewards, fs.FactionStats.BountyRewardsValue,
                                        fs.FactionStats.Interdicted,fs.FactionStats.Interdiction,
                                        fs.FactionStats.KillBondAwardAsVictimFaction, fs.FactionStats.KillBondAwardAsAwaringFaction, fs.FactionStats.KillBondAwardAsAwaringFactionValue,
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
                            //System.Diagnostics.Debug.WriteLine("Set row to " + i);
                            dataGridViewFactions.SafeFirstDisplayedScrollingRowIndex(i);
                            break;
                        }
                    }
                }
            }

            labelValue.Text = factionslist.Count + " Missions".T(EDTx.UserControlMissions_MPlural);
        }

        #endregion

        private void Discoveryform_OnHistoryChange(HistoryList obj)
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

        private void dataGridViewFactions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ShowMissions(dataGridViewFactions.LeftClickRow);
        }

        void ShowMissions(int row)
        { 
            if (row >= 0)
            {
                FactionStatistics fs = dataGridViewFactions.Rows[row].Tag as FactionStatistics;

                ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();
                MissionListUserControl mluc = new MissionListUserControl();

                mluc.Clear();
                MissionList ml = last_he?.MissionList;
                if (ml != null)
                {
                    foreach (MissionState ms in ml.Missions.Values)
                    {
                        if (ms.State == MissionState.StateTypes.Completed && ms.Completed != null)
                        {
                            var faction = ms.Mission.Faction;
                            if (faction == fs.Name)
                                mluc.Add(ms, true);
                        }
                    }

                    mluc.Finish();
                }

                f.Add(new ExtendedControls.ConfigurableForm.Entry(mluc, "Grid", "", new System.Drawing.Point(3, 30), new System.Drawing.Size(800, 400), null)
                { anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom });

                f.AddOK(new Point(800 - 100, 460), "OK", anchor:AnchorStyles.Right | AnchorStyles.Bottom);

                f.Trigger += (dialogname, controlname, xtag) =>
                {
                    if (controlname == "OK" || controlname == "Close")
                    {
                        f.ReturnResult(DialogResult.OK);
                    }
                };

                f.AllowResize = true;

                f.ShowDialogCentred(FindForm(), FindForm().Icon, "Missions for ".Tx(EDTx.UserControlFactions_MissionsFor) + fs.Name, closeicon:true);
            }
        }

        private void showMissionsForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMissions(dataGridViewFactions.RightClickRow);
        }
    }
}
