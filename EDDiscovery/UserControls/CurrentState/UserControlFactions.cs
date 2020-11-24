﻿/*
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
using System.Threading.Tasks;
using System.Windows.Forms;
using BaseUtils.JSON;
using EDDiscovery.Controls;
using EDDiscovery.UserControls.Helpers;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;

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
                this.Name = name;
                this.Count = 0;
            }

            public void Add(int amount)
            {
                this.Count += amount;
            }
        }

        private class SystemInfluence
        {
            public long SystemAddress { get; }
            public int Influence { get; private set; }

            public SystemInfluence(long systemAddress, int influence)
            {
                this.SystemAddress = systemAddress;
                this.Influence = influence;
            }

            public void AddInfluence(int influence)
            {
                this.Influence += influence;
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

            public FactionStatistics(string name)
            {
                this.Name = name;
                this.Missions = 0;
                this.Influence = 0;
                this.Reputation = 0;
                this.Credits = 0;
                this.Rewards = new Dictionary<string, MissionReward>();
                this.Systems = new Dictionary<long, SystemInfluence>();
            }

            public void AddMissions(int amount)
            {
                this.Missions += amount;
            }

            public void AddInfluence(int amount)
            {
                this.Influence += amount;
            }

            public void AddReputation(int amount)
            {
                this.Reputation += amount;
            }

            public void AddCredits(long amount)
            {
                this.Credits += amount;
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

            public void AddSystemInfluence(long systemAddress, int amount)
            {
                SystemInfluence si;
                if (!Systems.TryGetValue(systemAddress, out si))
                {
                    si = new SystemInfluence(systemAddress, amount);
                    Systems.Add(systemAddress, si);
                }
                else
                {
                    si.AddInfluence(amount);
                }
            }
        }

        private class SystemInfo
        {
            public string Name { get; set; }
            public long Address { get; set; }
            public int Influence { get; set; }
            public string Allegiance { get; set; }
            public string Government { get; set; }
            public string Faction { get; set; }
            public string State { get; set; }
            public int Population { get; set; }
            public string Security { get; set; }
            public string Economy { get; set; }
            public string SecondEconomy { get; set; }
            public string Reserve { get; set; }
        }

        private string DbColumnSaveFactions { get { return DBName("MissionAccountingFactions", "DGVCol"); } }
        private string DbColumnSaveSystems { get { return DBName("MissionAccountingSystems", "DGVCol"); } }
        private string DbStartDate { get { return DBName("MissionAccountingStartDate"); } }
        private string DbEndDate { get { return DBName("MissionAccountingEndDate"); } }
        private string DbStartDateChecked { get { return DBName("MissionAccountingStartDateCheck"); } }
        private string DbEndDateChecked { get { return DBName("MissionAccountingEndDateCheck"); } }
        private DateTime NextExpiry;
        private HistoryEntry last_he = null;
        private Dictionary<string, FactionStatistics> Factions;
        private Task<List<SystemInfo>> SystemUpdateTask = null;

        #region Init

        public UserControlFactions()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            dataGridViewFactions.MakeDoubleBuffered();
            colMissions.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            colSystems.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            colInfluence.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            colReputation.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCredits.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            colMissions.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colSystems.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colInfluence.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colReputation.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colCredits.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            startDateTime.Value = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDate(DbStartDate, DateTime.UtcNow);
            startDateTime.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbStartDateChecked, false);
            endDateTime.Value = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDate(DbEndDate, DateTime.UtcNow);
            endDateTime.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbEndDateChecked, false);
            VerifyDates();

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;

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
            DGVLoadColumnLayout(dataGridViewSystems, DbColumnSaveSystems);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewFactions, DbColumnSaveFactions);
            DGVSaveColumnLayout(dataGridViewSystems, DbColumnSaveSystems);

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
            this.Factions = new Dictionary<string, FactionStatistics>();
            dataGridViewFactions.Rows.Clear();

            MissionList ml = last_he?.MissionList;
            var total = 0;
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
                            total++;
                            var faction = ms.Mission.Faction;
                            FactionStatistics factionStats;
                            if (!this.Factions.TryGetValue(faction, out factionStats))
                            {
                                factionStats = new FactionStatistics(faction);
                                this.Factions.Add(faction, factionStats);
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
                                                factionStats.AddSystemInfluence(si.SystemAddress, si.Influence.Length);
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
                if (total > 0)
                {
                    var rows = new List<DataGridViewRow>(total);
                    foreach (FactionStatistics fs in this.Factions.Values)
                    {
                        var rewards = "";
                        foreach (var reward in fs.Rewards.Values)
                        {
                            if (rewards.Length > 0)
                            {
                                rewards += ", ";
                            }
                            rewards += reward.Count + " " + reward.Name;
                        }
                        object[] rowobj = {
                            fs.Name,
                            fs.Missions,
                            fs.Systems.Values.Count,
                            fs.Influence,
                            fs.Reputation,
                            String.Format("{0:n0}", fs.Credits),
                            rewards
                        };
                        var row = dataGridViewFactions.RowTemplate.Clone() as DataGridViewRow;
                        row.CreateCells(dataGridViewFactions, rowobj);
                        row.Tag = fs;
                        rows.Add(row);
                    }
                    dataGridViewFactions.Rows.AddRange(rows.ToArray());
                }
            }
            labelValue.Text = total + " missions";
            UpdateSystems();
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
            if (e.Column.Index >= 1 && e.Column.Index <= 4)
            {
                e.SortDataGridViewColumnNumeric();
            }
        }

        private void dataGridViewFactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            UpdateSystems();
        }

        private async void UpdateSystems()
        {
            if (SystemUpdateTask != null)
            {
                SystemUpdateTask.Wait();
            }
            dataGridViewSystems.Rows.Clear();
            if (dataGridViewFactions.SelectedRows.Count > 0)
            {
                FactionStatistics fs = dataGridViewFactions.SelectedRows[0].Tag as FactionStatistics;
                SystemUpdateTask = Task.Factory.StartNew(() =>
                {
                    var sr = new List<SystemInfo>();
                    var edsm = new EDSMClass();
                    foreach (SystemInfluence si in fs.Systems.Values)
                    {
                        JObject sys = edsm.GetSystemByAddress(si.SystemAddress);
                        JObject info = sys["information"].Object();
                        string ex(string v) => info != null ? info[v].Str("Unknown") : "Unknown";
                        sr.Add(new SystemInfo {
                            Name = sys["name"].Str("Unknown"),
                            Address = si.SystemAddress,
                            Influence = si.Influence,
                            Allegiance = ex("allegiance"),
                            Government = ex("government"),
                            Faction = ex("faction"),
                            State = ex("factionState"),
                            Population = info != null ? info["population"].Int(-1) : -1,
                            Security = ex("security"),
                            Economy = ex("economy"),
                            SecondEconomy = ex("secondEconomy"),
                            Reserve = ex("reserve"),
                        }); ;
                    }
                    return sr;
                });
                await SystemUpdateTask;

                var rows = new List<DataGridViewRow>();
                foreach (var system in SystemUpdateTask.Result)
                {
                    object[] rowobj =
                    {
                        system.Name,
                        system.Influence,
                        system.Faction,
                        system.Allegiance,
                        system.Government,
                        system.State
                    };
                    var row = dataGridViewSystems.RowTemplate.Clone() as DataGridViewRow;
                    row.CreateCells(dataGridViewSystems, rowobj);
                    row.Tag = system;
                    rows.Add(row);
                }
                dataGridViewSystems.Rows.AddRange(rows.ToArray());
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

                f.AddOK(new Point(800 - 100, 460), "OK", anchor: AnchorStyles.Right | AnchorStyles.Bottom);

                f.Trigger += (dialogname, controlname, xtag) =>
                {
                    if (controlname == "OK" || controlname == "Close")
                    {
                        f.ReturnResult(DialogResult.OK);
                    }
                };

                f.AllowResize = true;

                f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon, "Missions for ".Tx(EDTx.UserControlMissionAccounting_MissionsFor) + fs.Name, closeicon: true);
            }
        }

        private void showMissionsForFactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowMissions(dataGridViewFactions.RightClickRow);
        }
    }
}
