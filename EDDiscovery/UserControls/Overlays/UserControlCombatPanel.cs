﻿/*
 * Copyright © 2016 - 2022 EDDiscovery development team
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCombatPanel : UserControlCommonBase
    {
        private long npc_total_kills = 0;
        private long npc_faction_kills = 0;
        private long total_reward = 0;
        private long faction_reward = 0;
        private long balance = 0;
        private long total_crimes = 0;
        private long pvp_kills = 0;
        private long died = 0;
        private List<CombatFilterEntry> savedfilterentries;
        private List<CombatFilterEntry> displayedfilterentries;

        public const string dbFilter = "Filter";
        public const string dbCampaign = "Campaign";
        public const string dbGridshow = "Gridshow";
        public const string dbSelected = "Selected";

        private JournalFilterSelector cfs;

        private CombatFilterEntry currentcombatfilter;        // current selected

        private Font transparentfont;

        #region Init

        public UserControlCombatPanel()
        {
            InitializeComponent();
            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);

            DBBaseName = "CombatPanel";
        }

        protected override void Init()
        {

            string filter = GetSetting(dbCampaign, "");
            List<string> filtarray = BaseUtils.StringParser.ParseWordList(filter);

            dataGridViewCombat.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewCombat.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            savedfilterentries = new List<CombatFilterEntry>();

            for (int i = 0; i < filtarray.Count / 5; i++)
            {
                CombatFilterEntry f = new CombatFilterEntry(filtarray, i * 5);
                if (f.Type != CombatFilterEntry.EntryType.Invalid)
                {
                    savedfilterentries.Add(f);
                }
            }

            cfs = new JournalFilterSelector();
            cfs.UC.AddAllNone();
            foreach (var x in jtelist)
                cfs.UC.Add(JournalEntry.GetNameImageOfEvent(x));
            foreach (var x in extradetaillist)
                cfs.UC.Add(JournalEntry.GetNameImageOfEvent(x));
            
            cfs.UC.Sort();
            cfs.SaveSettings += (newset,e) => {
                string filters = GetSetting(dbFilter, "All");
                if (filters != newset)
                {
                    PutSetting(dbFilter, newset);
                    Display();
                }
            };

            buttonExtEditCampaign.Enabled = false;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            DiscoveryForm.OnNewEntry += Discoveryform_OnNewEntry;

            checkBoxCustomGridOn.Checked = GetSetting(dbGridshow, false);
            checkBoxCustomGridOn.Visible = IsFloatingWindow;

            transparentfont = ExtendedControls.Theme.Current.GetFont;


            labelTarget.Size = new Size(1280, 24);
            labelTarget.Text = "No Target".Tx();
        }

        protected override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewCombat);
        }

        protected override void InitialDisplay()
        {
            if (DiscoveryForm.History.Count > 0)      // on program start, this can be called with an empty history
            {
                FillCampaignCombo();                  // if so, ignore it.  otherwise the history could be full and we need to process
                SetCampaignCombo();
            }

            Display();
        }

        protected override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewCombat);

            DiscoveryForm.OnNewEntry -= Discoveryform_OnNewEntry;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            transparentfont?.Dispose();

            string s = "";
            foreach (CombatFilterEntry f in savedfilterentries)
            {
                s += f.Type.ToString() + "," + f.Name.QuoteString(comma: true) + "," + f.TargetFaction.QuoteString(comma: true) + "," +
                                    f.StartTimeUTC.ToStringZulu() + "," + f.EndTimeUTC.ToStringZulu() + ",";
            }

            PutSetting(dbCampaign, s);

            PutSetting(dbGridshow, checkBoxCustomGridOn.Checked);
            PutSetting(dbSelected, currentcombatfilter?.UniqueID ?? "");
        }

        protected override void SetTransparency(bool on, Color curbackcol)
        {
            dataViewScrollerPanelCombat.Visible = !on || checkBoxCustomGridOn.Checked;
            dataViewScrollerPanelCombat.BackColor = curbackcol;
            BackColor = curbackcol;
            panelTop.Visible = !on;
            panelStatus.BackColor =  curbackcol;

            Color fore = on ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;

            labelTarget.Font = labelCredits.Font = labelTotalKills.Font = labelTotalReward.Font = labelFactionKills.Font = labelFactionReward.Font = 
                        labelFaction.Font = labelTotalCrimes.Font = labelBalance.Font = on ? transparentfont : ExtendedControls.Theme.Current.GetFont;

            labelTarget.ForeColor = labelCredits.ForeColor = labelTotalKills.ForeColor = labelTotalReward.ForeColor = labelFactionKills.ForeColor = labelFactionReward.ForeColor = labelFaction.ForeColor = labelTotalCrimes.ForeColor = labelBalance.ForeColor = fore;
            labelTarget.TextBackColor = labelCredits.TextBackColor = labelTotalKills.TextBackColor = labelTotalReward.TextBackColor = labelFactionKills.TextBackColor = labelFactionReward.TextBackColor = labelFaction.TextBackColor = labelTotalCrimes.TextBackColor = labelBalance.TextBackColor = curbackcol;

        }
        public override bool SupportTransparency { get { return true; } }

        #endregion

        #region Event Handlers

        private void Discoveryform_OnHistoryChange()      // called if history re-read, so need to recalc combo and display
        {
            bool firsttime = comboBoxCustomCampaign.Items.Count == 0;

            FillCampaignCombo();

            if (firsttime)
                SetCampaignCombo();        // pick saved entry..
            else
                CheckCurrent();         // someone changed history, see if we still have that entry..

            Display();
        }

        //public override void ReceiveHistoryEntry(HistoryEntry he)
        //{
          //  Discoveryform_OnNewEntry(he);       // enable for pushing thru a historical event as new - only for debugging
        //}

        private void Discoveryform_OnNewEntry(HistoryEntry he)
        {
            if (currentcombatfilter != null)
            {
                bool tryadd = true; // normally go for it..

                if (currentcombatfilter.Type == CombatFilterEntry.EntryType.Time)     // time is limited 
                    tryadd = he.EventTimeUTC <= currentcombatfilter.EndTimeUTC;
                else if (currentcombatfilter.Type == CombatFilterEntry.EntryType.Mission) // mission is limited, lookup mission and check end time
                {
                    MissionState ms = DiscoveryForm.History.MissionListAccumulator.GetMission(currentcombatfilter.MissionKey ?? "-");
                    tryadd = ms != null ? (he.EventTimeUTC <= ms.MissionEndTime) : false;
                }

                if (tryadd)
                {
                    HashSet<string> eventfilter = GetSetting(dbFilter, "All").SplitNoEmptyStartFinish(';').ToHashSet();
                    bool allevents = eventfilter.Contains("All");

                    if (!(allevents || eventfilter.Contains(he.journalEntry.EventFilterName)))      // filter out if not true
                        tryadd = false;
                }

                if ( tryadd )
                { 
                    if (insertToGrid(he))        // if did add..
                    {
                        SetLabels();
                    }
                }

                if (he.EntryType == JournalTypeEnum.Undocked && currentcombatfilter.Type == CombatFilterEntry.EntryType.Lastdock)        // on undock, and we are in last docked mode, refresh
                {
                    Display();
                }
            }

            if (he.EntryType == JournalTypeEnum.MissionAccepted)        // mission accepted means another entry..
                FillCampaignCombo();                                    // could only add entries, so no need to check it its disappeared

            SetTarget(he);
        }

        #endregion

        #region Display

        public void Display()
        {
            DataGridViewColumn sortcol = dataGridViewCombat.SortedColumn != null ? dataGridViewCombat.SortedColumn : dataGridViewCombat.Columns[0];
            SortOrder sortorder = dataGridViewCombat.SortOrder != SortOrder.None ? dataGridViewCombat.SortOrder : SortOrder.Descending;

            dataGridViewCombat.Rows.Clear();
            npc_total_kills = npc_faction_kills = total_reward = faction_reward = balance = total_crimes = pvp_kills = died = 0;

            if ( currentcombatfilter != null )
            {
            
                //System.Diagnostics.Debug.WriteLine("Filter {0} {1}", current.StartTime.ToStringZulu(), current.EndTime.ToStringZulu());
                List<HistoryEntry> hel;

                if (currentcombatfilter.Type == CombatFilterEntry.EntryType.Lastdock)
                    hel = HistoryList.ToLastDock(DiscoveryForm.History.EntryOrder());
                else if (currentcombatfilter.Type == CombatFilterEntry.EntryType.All)
                    hel = DiscoveryForm.History.LatestFirst();
                else if (currentcombatfilter.Type == CombatFilterEntry.EntryType.Oneday)
                    hel = HistoryList.FilterByDateRangeLatestFirst(DiscoveryForm.History.EntryOrder(), DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
                else if (currentcombatfilter.Type == CombatFilterEntry.EntryType.Sevendays)
                    hel = HistoryList.FilterByDateRangeLatestFirst(DiscoveryForm.History.EntryOrder(), DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
                else if (currentcombatfilter.Type == CombatFilterEntry.EntryType.Today)
                    hel = HistoryList.FilterByDateRangeLatestFirst(DiscoveryForm.History.EntryOrder(), DateTime.UtcNow.Date, DateTime.UtcNow);
                else if (currentcombatfilter.Type == CombatFilterEntry.EntryType.Mission)
                {
                    hel = new List<HistoryEntry>();     // default empty
                    if (currentcombatfilter.MissionKey != null)
                    {
                        // look up the mission in the current data
                        MissionState ms = DiscoveryForm.History.MissionListAccumulator.GetMission(currentcombatfilter.MissionKey);
                        if ( ms != null )
                            hel = HistoryList.FilterByDateRangeLatestFirst(DiscoveryForm.History.EntryOrder(), currentcombatfilter.StartTimeUTC, ms.MissionEndTime);
                    }
                }
                else
                    hel = HistoryList.FilterByDateRangeLatestFirst(DiscoveryForm.History.EntryOrder(), currentcombatfilter.StartTimeUTC, currentcombatfilter.EndTimeUTC);

                HashSet<string> eventfilter = GetSetting(dbFilter, "All").SplitNoEmptyStartFinish(';').ToHashSet();
                bool allevents = eventfilter.Contains("All");

                var rows = new List<DataGridViewRow>(hel.Count);
                foreach (HistoryEntry he in hel)
                {
                    if (allevents || eventfilter.Contains(he.journalEntry.EventFilterName))
                    {
                        //System.Diagnostics.Debug.WriteLine("Combat Add {0} {1} {2}", he.EventTimeUTC.ToStringZulu(), he.EventSummary, he.EventDescription);
                        var row = createRow(he);
                        if (row != null)
                            rows.Add(row);
                    }
                }
                dataGridViewCombat.Rows.AddRange(rows.ToArray());
            }

            dataGridViewCombat.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewCombat.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            SetLabels();
        }

        DataGridViewRow createRow(HistoryEntry he)
        {
            if (CreateEntry(he, out var rewardcol))
            {
                var rw = dataGridViewCombat.RowTemplate.Clone() as DataGridViewRow;

                rw.CreateCells(dataGridViewCombat, EDDConfig.Instance.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
                    he.EventSummary, he.GetInfo(), rewardcol);

                rw.Tag = he;
                return rw;
            }

            return null;
        }

        bool insertToGrid(HistoryEntry he)
        {
            var row = createRow(he);
            if (row is null)
                return false;

            dataGridViewCombat.Rows.Insert(0, row);
            return true;
        }

        static JournalTypeEnum[] jtelist = new JournalTypeEnum[]            // ones to display without any extra detail
        {
            JournalTypeEnum.FighterDestroyed,JournalTypeEnum.FighterRebuilt,
            JournalTypeEnum.Interdicted,JournalTypeEnum.EscapeInterdiction,
            JournalTypeEnum.HeatDamage,JournalTypeEnum.HullDamage,
            JournalTypeEnum.SRVDestroyed
        };

        static JournalTypeEnum[] extradetaillist = new JournalTypeEnum[]   // ones picked out by case statements
        {
            JournalTypeEnum.MissionAccepted,            JournalTypeEnum.MissionCompleted,            JournalTypeEnum.MissionRedirected,
            JournalTypeEnum.Bounty,            JournalTypeEnum.CommitCrime,            JournalTypeEnum.FactionKillBond,            JournalTypeEnum.CapShipBond,
            JournalTypeEnum.Resurrect,            JournalTypeEnum.Died,            JournalTypeEnum.PVPKill,        }
        ;

        bool CreateEntry(HistoryEntry he, out string rewardcol)
        {
            rewardcol = "";

            MissionState ml = currentcombatfilter.MissionKey != null ? DiscoveryForm.History.MissionListAccumulator.GetMission(currentcombatfilter.MissionKey) : null;

            if ( ml != null &&
                 ((he.EntryType == JournalTypeEnum.MissionAccepted && (he.journalEntry as EliteDangerousCore.JournalEvents.JournalMissionAccepted).MissionId == ml.Mission.MissionId)
                 || (he.EntryType == JournalTypeEnum.MissionCompleted && (he.journalEntry as EliteDangerousCore.JournalEvents.JournalMissionCompleted).MissionId == ml.Mission.MissionId)
                 || (he.EntryType == JournalTypeEnum.MissionRedirected && (he.journalEntry as EliteDangerousCore.JournalEvents.JournalMissionRedirected).MissionId == ml.Mission.MissionId)
               ))
            { 
            }
            else if (he.EntryType == JournalTypeEnum.Bounty)
            {
                var c = he.journalEntry as EliteDangerousCore.JournalEvents.JournalBounty;
                rewardcol = c.TotalReward.ToString("N0");

                total_reward += c.TotalReward;
                npc_total_kills++;
                balance += c.TotalReward;

                if (currentcombatfilter.TargetFaction != null && currentcombatfilter.TargetFaction.Equals(c.VictimFaction))
                {
                    faction_reward += c.TotalReward;
                    npc_faction_kills++;
                }
            }
            else if (he.EntryType == JournalTypeEnum.CommitCrime)
            {
                var c = he.journalEntry as EliteDangerousCore.JournalEvents.JournalCommitCrime;
                rewardcol = (-c.Cost).ToString("N0");
                balance -= c.Cost;
                total_crimes++;
            }
            else if (he.EntryType == JournalTypeEnum.FactionKillBond)
            {
                var c = he.journalEntry as EliteDangerousCore.JournalEvents.JournalFactionKillBond;
                rewardcol = c.Reward.ToString("N0");
                total_reward += c.Reward;
                npc_total_kills++;
                balance += c.Reward;
                if (currentcombatfilter.TargetFaction != null && currentcombatfilter.TargetFaction.Equals(c.VictimFaction))
                {
                    faction_reward += c.Reward;
                    npc_faction_kills++;
                }
            }
            else if (he.EntryType == JournalTypeEnum.CapShipBond)
            {
                var c = he.journalEntry as EliteDangerousCore.JournalEvents.JournalCapShipBond;
                rewardcol = c.Reward.ToString("N0");
                total_reward += c.Reward;
                npc_total_kills++;
                balance += c.Reward;
                if (currentcombatfilter.TargetFaction != null && currentcombatfilter.TargetFaction.Equals(c.VictimFaction))
                {
                    faction_reward += c.Reward;
                    npc_faction_kills++;
                }
            }
            else if (he.EntryType == JournalTypeEnum.Resurrect)
            {
                var c = he.journalEntry as EliteDangerousCore.JournalEvents.JournalResurrect;
                rewardcol = (-c.Cost).ToString("N0");
                balance -= c.Cost;
            }
            else if (he.EntryType == JournalTypeEnum.Died)
            {
                died++;
            }
            else if (he.EntryType == JournalTypeEnum.PVPKill)
            {
                pvp_kills++;
            }
            else if (jtelist.Contains(he.EntryType))
            {
            }
            else
                return false;

            return true;
        }

        void SetLabels()
        {
            bool faction = currentcombatfilter != null ? currentcombatfilter.TargetFaction.Length > 0 : false;
            labelTotalKills.Text = (npc_total_kills>0 || pvp_kills>0) ? ("Kills".Tx()+ ": " + npc_total_kills.ToString() + "/" + pvp_kills.ToString()) : "";
            labelFactionKills.Text = faction ? ("Faction".Tx()+ ": " + npc_faction_kills.ToString()) : "";
            labelFaction.Text = faction ? (currentcombatfilter.TargetFaction) : "";
            labelTotalCrimes.Text = (total_crimes>0) ? ("Crimes".Tx()+ ": " + total_crimes.ToString()) : "";

            labelCredits.Text = (DiscoveryForm.History.GetLast != null) ? (DiscoveryForm.History.GetLast.Credits.ToString("N0") + "cr") : "";
            labelBalance.Text = (balance != 0 ) ? ("Bal".Tx()+ ": " + balance.ToString("N0") + "cr") : "";
            labelFactionReward.Text = (faction && faction_reward != balance) ? ("+" + faction_reward.ToString("N0") + "cr") : "";
            labelTotalReward.Text = (total_reward != balance) ? ("+" + total_reward.ToString("N0") + "cr") : "";
            labelDied.Text = (died != 0) ? ("Died".Tx()+ ": " + died.ToString()) : "";
        }

        static JournalTypeEnum[] targetofflist = new JournalTypeEnum[]            // ones to display without any extra detail
        {
            JournalTypeEnum.Died,
            JournalTypeEnum.FighterDestroyed,
            JournalTypeEnum.SupercruiseEntry,
            JournalTypeEnum.FSDJump,             JournalTypeEnum.CarrierJump,
            JournalTypeEnum.Docked,
            JournalTypeEnum.LaunchSRV,
            JournalTypeEnum.LaunchFighter,
        };

        private void SetTarget(HistoryEntry he)
        {
            if (he.journalEntry.EventTypeID == JournalTypeEnum.ShipTargeted)
            {
                EliteDangerousCore.JournalEvents.JournalShipTargeted je = he.journalEntry as EliteDangerousCore.JournalEvents.JournalShipTargeted;
                labelTarget.Text = je.ToString();
            }
            else if (Array.IndexOf(targetofflist, he.journalEntry.EventTypeID) >= 0)
            {
                labelTarget.Text = "No Target".Tx();
            }
        }

        #endregion

        #region UI

        public void FillCampaignCombo()
        {
            displayedfilterentries = new List<CombatFilterEntry>();

            displayedfilterentries.Add(new CombatFilterEntry("New Campaign".Tx(), CombatFilterEntry.EntryType.NewEntry));
            displayedfilterentries.Add(new CombatFilterEntry("Since Last Dock".Tx(), CombatFilterEntry.EntryType.Lastdock));
            displayedfilterentries.Add(new CombatFilterEntry("Today".Tx(), CombatFilterEntry.EntryType.Today));
            displayedfilterentries.Add(new CombatFilterEntry("24h".Tx(), CombatFilterEntry.EntryType.Oneday));
            displayedfilterentries.Add(new CombatFilterEntry("7 days".Tx(), CombatFilterEntry.EntryType.Sevendays));
            displayedfilterentries.Add(new CombatFilterEntry("All".Tx(), CombatFilterEntry.EntryType.All));

            displayedfilterentries.AddRange(savedfilterentries);

            var missions = DiscoveryForm.History.MissionListAccumulator.GetAllMissions();
            var combatmissions = MissionListAccumulator.GetAllCombatMissionsLatestFirst(missions);

            if (combatmissions != null )
            {
                foreach (var s in combatmissions)
                {
                    CombatFilterEntry f = new CombatFilterEntry(s);
                    displayedfilterentries.Add(f);
                }
            }

            disablecombobox = true;

            comboBoxCustomCampaign.Items.Clear();
            foreach (CombatFilterEntry f in displayedfilterentries)
                comboBoxCustomCampaign.Items.Add(f.NameDate);

            int cursel = currentcombatfilter != null ? displayedfilterentries.FindIndex(x => x.UniqueID.Equals(currentcombatfilter.UniqueID)) : -1;
            if (cursel >= 0)
                comboBoxCustomCampaign.SelectedIndex = cursel;

            disablecombobox = false;
        }

        private void SetCampaignCombo()
        {
            string sel = GetSetting(dbSelected, "INITIAL DEFAULT !!!");

            int i = displayedfilterentries.FindIndex(x => x.UniqueID.Equals(sel));      // note we select on name, which is translated above, so changing languages will reset it

            if (i < 0)        // no selection
                i = 1;          // select last doc - see FillComboBox above for reason its one

            currentcombatfilter = displayedfilterentries[i];
            disablecombobox = true;
            comboBoxCustomCampaign.SelectedIndex = i;
            disablecombobox = false;

            buttonExtEditCampaign.Enabled = currentcombatfilter != null && currentcombatfilter.Type == CombatFilterEntry.EntryType.Time;
        }


        bool disablecombobox = false;
        private void comboBoxCustomCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!disablecombobox && comboBoxCustomCampaign.SelectedIndex >= 0)
            {
                int entry = comboBoxCustomCampaign.SelectedIndex;

                CombatFilterEntry f = displayedfilterentries[entry];
                if (f.Type == CombatFilterEntry.EntryType.NewEntry)
                {
                    CombatFilterEntry newe = new CombatFilterEntry("Enter name", CombatFilterEntry.EntryType.Time);
                    if ( EditEntry(newe, true, false) == DialogResult.OK )
                    {
                        currentcombatfilter = newe;
                        savedfilterentries.Add(currentcombatfilter);
                        FillCampaignCombo();
                    }
                }
                else
                {
                    currentcombatfilter = f;
                }

                buttonExtEditCampaign.Enabled = currentcombatfilter != null && currentcombatfilter.Type == CombatFilterEntry.EntryType.Time;
                Display();
            }
        }


        private void CheckCurrent()
        {
            if (currentcombatfilter == null || displayedfilterentries.Find(x => x.UniqueID.Equals(currentcombatfilter.UniqueID)) == null)
            {
                currentcombatfilter = null;
                disablecombobox = true;
                comboBoxCustomCampaign.SelectedIndex = -1;
                disablecombobox = false;
            }
        }

        private void buttonExtEditCampaign_Click(object sender, EventArgs e)
        {
            if (currentcombatfilter != null)        // should always be non null, but double check.  Note must be a saved entry, and we never remake these, only the mission ones
            {
                DialogResult res = EditEntry(currentcombatfilter,false,true);
                if (res == DialogResult.Abort)
                {
                    savedfilterentries.Remove(currentcombatfilter);
                    currentcombatfilter = null;
                }

                CheckCurrent();
                FillCampaignCombo();
                Display();
            }
        }

        DialogResult EditEntry(CombatFilterEntry entry, bool newentry, bool allowdel)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            DateTime starttime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(entry.StartTimeUTC);
            DateTime endtime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(entry.EndTimeUTC);

            int width = 430;

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "Name".Tx()+ ":", new Point(10, 40), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Name", typeof(ExtendedControls.ExtTextBox), entry.Name, new Point(100, 40), new Size(width - 100 - 20, 24), "Give name to campaign".Tx()) { TextBoxClearOnFirstChar = newentry });

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "Faction".Tx()+ ":", new Point(10, 70), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Faction", typeof(ExtendedControls.ExtTextBox), entry.TargetFaction, new Point(100, 70), new Size(width - 100 - 20, 24), "Optional faction to target".Tx()) );

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "Start".Tx()+ ":", new Point(10, 100), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("DTS", typeof(ExtendedControls.ExtDateTimePicker), starttime.ToStringZulu(), new Point(100, 100), new Size(width - 100 - 20, 24), "Select Start time".Tx()) { CustomDateFormat = "yyyy-MM-dd HH:mm:ss" });

            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("L", typeof(Label), "End".Tx()+ ":", new Point(10, 130), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableEntryList.Entry("DTE", typeof(ExtendedControls.ExtDateTimePicker), endtime.ToStringZulu(), new Point(100, 130), new Size(width - 100 - 20, 24), "Select Start time".Tx()) { CustomDateFormat = "yyyy-MM-dd HH:mm:ss" });

            f.AddOK(new Point(width - 100, 180), "Press to Accept".Tx());
            f.AddCancel(new Point(width - 200, 180), "Press to Cancel".Tx());

            if ( allowdel )
                f.Add(new ExtendedControls.ConfigurableEntryList.Entry("Delete", typeof(ExtendedControls.ExtButton), "Delete".Tx(), new Point(10, 180), new Size(80, 24), "Press to Delete".Tx()));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK")
                {
                    CombatFilterEntry fe = displayedfilterentries.Find(x => x.UniqueID.Equals(f.Get("Name")));

                    if (fe != null && fe != entry)
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Name of campaign already in use, cannot overwrite".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        f.ReturnResult(DialogResult.OK);
                    }
                }
                else if (controlname == "Delete")
                {
                    if (f.Get("Name").Equals(entry.Name))
                    {
                        if (ExtendedControls.MessageBoxTheme.Show(this.FindForm(), string.Format("Confirm deletion of {0}".Tx(), entry.Name), "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK )
                        {
                            f.ReturnResult(DialogResult.Abort);
                        }
                    }
                    else
                    {
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Name changed - can't delete".Tx(), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if ( controlname == "Cancel" || controlname == "Close" )
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon,  "Campaign".Tx(), closeicon:true);
            if (res == DialogResult.OK)
            {
                entry.Reset(f.Get("Name"), f.Get("Faction"),
                                EDDConfig.Instance.ConvertTimeToUTCFromPicker(f.GetDateTime("DTS").Value),
                                EDDConfig.Instance.ConvertTimeToUTCFromPicker(f.GetDateTime("DTE").Value));
            }

            return res;
        }
        private void buttonFilter_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            cfs.Open(GetSetting(dbFilter, "All"), b, this.FindForm());
        }

        #endregion

        #region Clicks

        private void dataGridViewCombat_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewCombat.LeftClickRowValid)                                                   // Click expands it..
            {
                DataGridViewRow row = dataGridViewCombat.Rows[dataGridViewCombat.LeftClickRow];
                bool expanded = row.Cells[0].Tag != null;

                var leftclicksystem = (HistoryEntry)dataGridViewCombat.Rows[dataGridViewCombat.LeftClickRow].Tag;


                if (expanded) // put it back to original text
                {
                    dataGridViewCombat.Rows[dataGridViewCombat.LeftClickRow].Cells[2].Value = leftclicksystem.GetInfo();
                    row.Cells[0].Tag = null;
                    row.Cells[2].Style.WrapMode = row.Cells[1].Style.WrapMode = DataGridViewTriState.NotSet;
                }
                else
                {
                    dataGridViewCombat.Rows[dataGridViewCombat.LeftClickRow].Cells[2].Value = leftclicksystem.GetInfoDetailed(); 
                    row.Cells[0].Tag = true;
                    row.Cells[2].Style.WrapMode = row.Cells[1].Style.WrapMode = DataGridViewTriState.True;
                }
            }
        }

        #endregion

        #region Sort

        private void dataGridViewCombat_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 3)
                e.SortDataGridViewColumnNumeric();
        }

        #endregion


        #region FilterEntry

        [System.Diagnostics.DebuggerDisplay("{Name} {Type} {MissionKey} {TargetFaction} {StartTimeUTC} {EndTimeUTC}")]
        class CombatFilterEntry
        {
            public enum EntryType { Invalid, NewEntry, Time, Mission, Lastdock, Today, Sevendays, Oneday, All }

            public EntryType Type { get; private set; }
            public string Name { get; private set; }

            public string MissionKey { get; private set; }  // null if not set.  Mission only
            public string TargetFaction { get; private set; }  // null if not set

            public DateTime StartTimeUTC { get { return starttime; } }
            public DateTime EndTimeUTC { get { return endtime; } }     // for missions, its the expiry time, not the MissionEndTime

            public string NameDate
            {
                get
                {
                    string date = "";
                    if (Type == CombatFilterEntry.EntryType.Time || Type == CombatFilterEntry.EntryType.Mission)
                    {
                        date = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(starttime).ToString("d");
                        string dte = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(endtime).ToString("d");
                        if (date.Equals(dte))
                            date = "(" + date + ")";
                        else
                            date = "(" + date + "-" + dte + ")";
                    }

                    return Name + date;
                }
            }

            public string UniqueID      // used to identify it for retrieval.
            {
                get
                {
                    return Name + (MissionKey ?? "");
                }
            }

            private DateTime starttime;      // UTC
            private DateTime endtime;        // UTC only for non missions

            public CombatFilterEntry(string n, EntryType t)
            {
                Name = n;
                TargetFaction = "";
                Type = t;
                starttime = endtime = DateTime.UtcNow;
            }

            public CombatFilterEntry(MissionState m)
            {
                Type = EntryType.Mission;
                Name = m.Mission.Name + ":" + m.Mission.Faction;
                if (m.Mission.Target.Length > 0)
                    Name += ":" + m.Mission.Target;
                starttime = m.Mission.EventTimeUTC;
                endtime = m.Mission.Expiry;                 // NOTE expiry time of mission, not used however to gate below, we use Mission
                MissionKey = MissionListAccumulator.Key(m.Mission);
                TargetFaction = m.Mission.TargetFaction;
            }

            public CombatFilterEntry(List<string> filtarray, int i)
            {
                Type = EntryType.Invalid;

                EntryType t;

                if (Enum.TryParse<CombatFilterEntry.EntryType>(filtarray[i], out t))
                {
                    Name = filtarray[i + 1];
                    TargetFaction = filtarray[i + 2];

                    // keep the UTC marker in the time/date
                    if (DateTime.TryParse(filtarray[i + 3], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind, out starttime) &&
                        DateTime.TryParse(filtarray[i + 4], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind, out endtime))
                    {
                        Type = t;
                    }
                }
            }

            public void Reset(string n, string f, DateTime s, DateTime e)
            {
                Name = n; TargetFaction = f; starttime = s; endtime = e;
            }
        }


        #endregion
    }
}
