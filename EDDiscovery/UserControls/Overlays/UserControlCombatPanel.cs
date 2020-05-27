/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCombatPanel : UserControlCommonBase
    {
        private string DbSave { get { return DBName("CombatPanel" ); } }
        private string DbColumnSave { get { return DBName("CombatPanel" ,  "DGVCol"); } }

        private long total_kills = 0;
        private long faction_kills = 0;
        private long total_reward = 0;
        private long faction_reward = 0;
        private long balance = 0;
        private long total_crimes = 0;
        private List<FilterEntry> savedfilterentries;
        private List<FilterEntry> displayedfilterentries;

        private FilterEntry current;        // current selected

        private Font transparentfont;

        class FilterEntry
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
                    if (Type == FilterEntry.EntryType.Time || Type == FilterEntry.EntryType.Mission)
                    {
                        date = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(starttime).ToString("d") ;
                        string dte = EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(endtime).ToString("d");
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

            public FilterEntry(string n, EntryType t)
            {
                Name = n;
                TargetFaction = "";
                Type = t;
                starttime = endtime = DateTime.UtcNow;
            }

            public FilterEntry(MissionState m)
            {
                Type = EntryType.Mission;
                Name = m.Mission.Name + ":" + m.Mission.Faction;
                if (m.Mission.Target.Length > 0)
                    Name += ":" + m.Mission.Target;
                starttime = m.Mission.EventTimeUTC;
                endtime = m.Mission.Expiry;                 // NOTE expiry time of mission, not used however to gate below, we use Mission
                MissionKey = MissionList.Key(m.Mission);
                TargetFaction = m.Mission.TargetFaction;
            }

            public FilterEntry(List<string> filtarray, int i)
            {
                Type = EntryType.Invalid;

                EntryType t;

                if (Enum.TryParse<FilterEntry.EntryType>(filtarray[i], out t))
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

        #region Init

        public override void Init()
        {
            string filter = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave + "Campaign", "");
            List<string> filtarray = BaseUtils.StringParser.ParseWordList(filter);

            dataGridViewCombat.DefaultCellStyle.WrapMode = DataGridViewTriState.False;
            dataGridViewCombat.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;

            savedfilterentries = new List<FilterEntry>();

            for (int i = 0; i < filtarray.Count / 5; i++)
            {
                FilterEntry f = new FilterEntry(filtarray, i * 5);
                if (f.Type != FilterEntry.EntryType.Invalid)
                {
                    savedfilterentries.Add(f);
                }
            }

            buttonExtEditCampaign.Enabled = false;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;

            checkBoxCustomGridOn.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "Gridshow", false);
            checkBoxCustomGridOn.Visible = IsFloatingWindow;

            transparentfont = EDDTheme.Instance.GetFont;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            labelTarget.Size = new Size(1280, 24);
            labelTarget.Text = "No Target".T(EDTx.UserControlCombatPanel_NT);
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewCombat, DbColumnSave);
            //DEBUG target with           uctg.OnTravelSelectionChanged += (he,hl,s) => SetTarget(he);
        }

        public UserControlCombatPanel()
        {
            InitializeComponent();
        }

        public override void InitialDisplay()
        {
            if (discoveryform.history.Count > 0)      // on program start, this can be called with an empty history
            {
                FillCampaignCombo();                  // if so, ignore it.  otherwise the history could be full and we need to process
                SelectInitial();
            }

            Display();
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewCombat, DbColumnSave);

            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            transparentfont?.Dispose();

            string s = "";
            foreach (FilterEntry f in savedfilterentries)
            {
                s += f.Type.ToString() + "," + f.Name.QuoteString(comma: true) + "," + f.TargetFaction.QuoteString(comma: true) + "," +
                                    f.StartTimeUTC.ToStringZulu() + "," + f.EndTimeUTC.ToStringZulu() + ",";
            }

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave + "Campaign", s);

            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "Gridshow", checkBoxCustomGridOn.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingString(DbSave + "Selected", current?.UniqueID ?? "");
        }

        public override void SetTransparency(bool on, Color curbackcol)
        {
            dataViewScrollerPanelCombat.Visible = !on || checkBoxCustomGridOn.Checked;
            dataViewScrollerPanelCombat.BackColor = curbackcol;
            BackColor = curbackcol;
            panelTop.Visible = !on;
            panelStatus.BackColor =  curbackcol;

            Color fore = on ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;

            labelTarget.Font = labelCredits.Font = labelTotalKills.Font = labelTotalReward.Font = labelFactionKills.Font = labelFactionReward.Font = 
                        labelFaction.Font = labelTotalCrimes.Font = labelBalance.Font = on ? transparentfont : EDDTheme.Instance.GetFont;

            labelTarget.ForeColor = labelCredits.ForeColor = labelTotalKills.ForeColor = labelTotalReward.ForeColor = labelFactionKills.ForeColor = labelFactionReward.ForeColor = labelFaction.ForeColor = labelTotalCrimes.ForeColor = labelBalance.ForeColor = fore;
            labelTarget.TextBackColor = labelCredits.TextBackColor = labelTotalKills.TextBackColor = labelTotalReward.TextBackColor = labelFactionKills.TextBackColor = labelFactionReward.TextBackColor = labelFaction.TextBackColor = labelTotalCrimes.TextBackColor = labelBalance.TextBackColor = curbackcol;

        }
        public override Color ColorTransparency { get { return Color.Green; } }

        private void dataGridViewCombat_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 3)        
                e.SortDataGridViewColumnNumeric();
        }


        #endregion

        private void Discoveryform_OnHistoryChange(EliteDangerousCore.HistoryList obj)      // called if history re-read, so need to recalc combo and display
        {
            bool firsttime = comboBoxCustomCampaign.Items.Count == 0;

            FillCampaignCombo();

            if (firsttime)
                SelectInitial();        // pick saved entry..
            else
                CheckCurrent();         // someone changed history, see if we still have that entry..

            Display();
        }

        public void Display()
        {
            DataGridViewColumn sortcol = dataGridViewCombat.SortedColumn != null ? dataGridViewCombat.SortedColumn : dataGridViewCombat.Columns[0];
            SortOrder sortorder = dataGridViewCombat.SortOrder != SortOrder.None ? dataGridViewCombat.SortOrder : SortOrder.Descending;

            dataGridViewCombat.Rows.Clear();
            total_kills = faction_kills = total_reward = faction_reward = balance = total_crimes = 0;

            if ( current != null )
            {
                //System.Diagnostics.Debug.WriteLine("Filter {0} {1}", current.StartTime.ToStringZulu(), current.EndTime.ToStringZulu());
                List<HistoryEntry> hel;

                if (current.Type == FilterEntry.EntryType.Lastdock)
                    hel = discoveryform.history.FilterToLastDock();
                else if (current.Type == FilterEntry.EntryType.All)
                    hel = discoveryform.history.LastFirst;
                else if (current.Type == FilterEntry.EntryType.Oneday)
                    hel = discoveryform.history.FilterByDateRangeLatestFirst(DateTime.UtcNow.AddDays(-1), DateTime.UtcNow);
                else if (current.Type == FilterEntry.EntryType.Sevendays)
                    hel = discoveryform.history.FilterByDateRangeLatestFirst(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
                else if (current.Type == FilterEntry.EntryType.Today)
                    hel = discoveryform.history.FilterByDateRangeLatestFirst(DateTime.UtcNow.Date, DateTime.UtcNow);
                else if (current.Type == FilterEntry.EntryType.Mission)
                {
                    // look up the mission in the current data
                    MissionState ml = current.MissionKey != null && discoveryform.history.GetLast.MissionList.Missions.ContainsKey(current.MissionKey) ? discoveryform.history.GetLast.MissionList.Missions[current.MissionKey] : null;
                    hel = ml != null ? discoveryform.history.FilterByDateRangeLatestFirst(current.StartTimeUTC, ml.MissionEndTime) : new List<HistoryEntry>();
                }
                else
                    hel = discoveryform.history.FilterByDateRangeLatestFirst(current.StartTimeUTC, current.EndTimeUTC);

                var rows = new List<DataGridViewRow>(hel.Count);
                foreach (HistoryEntry he in hel)
                {
                    //System.Diagnostics.Debug.WriteLine("Combat Add {0} {1} {2}", he.EventTimeUTC.ToStringZulu(), he.EventSummary, he.EventDescription);
                    var row = createRow(he);
                    if ( row != null)
                        rows.Add(row);
                }
                dataGridViewCombat.Rows.AddRange(rows.ToArray());
            }

            dataGridViewCombat.Sort(sortcol, (sortorder == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridViewCombat.Columns[sortcol.Index].HeaderCell.SortGlyphDirection = sortorder;

            SetLabels();
        }

        private void Discoveryform_OnNewEntry(EliteDangerousCore.HistoryEntry he, EliteDangerousCore.HistoryList hel)
        {
            if ( current != null )
            {
                bool tryadd = true; // normally go for it..

                if (current.Type == FilterEntry.EntryType.Time)     // time is limited 
                    tryadd = he.EventTimeUTC <= current.EndTimeUTC;
                else if (current.Type == FilterEntry.EntryType.Mission) // mission is limited, lookup mission and check end time
                {
                    MissionState ml = current.MissionKey != null && he.MissionList.Missions.ContainsKey(current.MissionKey) ? he.MissionList.Missions[current.MissionKey] : null;
                    tryadd = ml != null ? (he.EventTimeUTC <= ml.MissionEndTime) : false;
                }

                if (tryadd)
                {
                    if (insertToGrid(he))        // if did add..
                    {
                        SetLabels();
                    }
                }

                if (he.EntryType == JournalTypeEnum.Undocked && current.Type == FilterEntry.EntryType.Lastdock )        // on undock, and we are in last docked mode, refresh
                {
                    Display();
                }
            }

            if (he.EntryType == JournalTypeEnum.MissionAccepted)        // mission accepted means another entry..
                FillCampaignCombo();                                    // could only add entries, so no need to check it its disappeared


            SetTarget(he);
        }

        DataGridViewRow createRow(HistoryEntry he)
        {
            if (CreateEntry(he, out var rewardcol))
            {
                var rw = dataGridViewCombat.RowTemplate.Clone() as DataGridViewRow;
                he.journalEntry.FillInformation(out var eventDescription, out _);

                rw.CreateCells(dataGridViewCombat, EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(he.EventTimeUTC),
                    he.EventSummary, eventDescription, rewardcol);

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
            JournalTypeEnum.Died,
            JournalTypeEnum.FighterDestroyed,JournalTypeEnum.FighterRebuilt,JournalTypeEnum.PVPKill,
            JournalTypeEnum.Interdicted,JournalTypeEnum.EscapeInterdiction,
            JournalTypeEnum.HeatDamage,JournalTypeEnum.HullDamage,
            JournalTypeEnum.SRVDestroyed
        };

        bool CreateEntry(HistoryEntry he, out string rewardcol)
        {
            rewardcol = "";

            MissionState ml = current.MissionKey != null && he.MissionList.Missions.ContainsKey(current.MissionKey) ? he.MissionList.Missions[current.MissionKey] : null;

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
                total_kills++;
                balance += c.TotalReward;

                if (current.TargetFaction != null && current.TargetFaction.Equals(c.VictimFaction))
                {
                    faction_reward += c.TotalReward;
                    faction_kills++;
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
                total_kills++;
                balance += c.Reward;
                if (current.TargetFaction != null && current.TargetFaction.Equals(c.VictimFaction))
                {
                    faction_reward += c.Reward;
                    faction_kills++;
                }
            }
            else if (he.EntryType == JournalTypeEnum.CapShipBond)
            {
                var c = he.journalEntry as EliteDangerousCore.JournalEvents.JournalCapShipBond;
                rewardcol = c.Reward.ToString("N0");
                total_reward += c.Reward;
                total_kills++;
                balance += c.Reward;
                if (current.TargetFaction != null && current.TargetFaction.Equals(c.VictimFaction))
                {
                    faction_reward += c.Reward;
                    faction_kills++;
                }
            }
            else if (he.EntryType == JournalTypeEnum.Resurrect)
            {
                var c = he.journalEntry as EliteDangerousCore.JournalEvents.JournalResurrect;
                rewardcol = (-c.Cost).ToString("N0");
                balance -= c.Cost;
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
            bool faction = current != null ? current.TargetFaction.Length > 0 : false;
            labelTotalKills.Text = (total_kills>0) ? ("Kills:".T(EDTx.UserControlCombatPanel_Kills) + total_kills.ToString()) : "";
            labelFactionKills.Text = faction ? ("Faction:".T(EDTx.UserControlCombatPanel_Faction) + faction_kills.ToString()) : "";
            labelFaction.Text = faction ? (current.TargetFaction) : "";
            labelTotalCrimes.Text = (total_crimes>0) ? ("Crimes:".T(EDTx.UserControlCombatPanel_Crimes) + total_crimes.ToString()) : "";

            labelCredits.Text = (discoveryform.history.GetLast != null) ? (discoveryform.history.GetLast.Credits.ToString("N0") + "cr") : "";
            labelBalance.Text = (balance > 0 ) ? ("Bal:".T(EDTx.UserControlCombatPanel_Bal) + balance.ToString("N0") + "cr") : "";
            labelFactionReward.Text = (faction && faction_reward != balance) ? (faction_reward.ToString("N0") + "cr") : "";
            labelTotalReward.Text = (total_reward != balance) ? (total_reward.ToString("N0") + "cr") : "";

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
                labelTarget.Text = "No Target".T(EDTx.UserControlCombatPanel_NT);
            }
        }

        #region UI

        public void FillCampaignCombo()
        {
            displayedfilterentries = new List<FilterEntry>();

            displayedfilterentries.Add(new FilterEntry("New Campaign".T(EDTx.UserControlCombatPanel_NewCampaign), FilterEntry.EntryType.NewEntry));
            displayedfilterentries.Add(new FilterEntry("Since Last Dock".T(EDTx.UserControlCombatPanel_SinceLastDock), FilterEntry.EntryType.Lastdock));
            displayedfilterentries.Add(new FilterEntry("Today".T(EDTx.Today), FilterEntry.EntryType.Today));
            displayedfilterentries.Add(new FilterEntry("24h".T(EDTx.t24h), FilterEntry.EntryType.Oneday));
            displayedfilterentries.Add(new FilterEntry("7 days".T(EDTx.t7days), FilterEntry.EntryType.Sevendays));
            displayedfilterentries.Add(new FilterEntry("All".T(EDTx.All), FilterEntry.EntryType.All));

            displayedfilterentries.AddRange(savedfilterentries);

            var missionlist = discoveryform.history.GetLast?.MissionList.GetAllCombatMissionsLatestFirst();

            if (missionlist != null )
            {
                foreach (var s in missionlist)
                {
                    FilterEntry f = new FilterEntry(s);
                    displayedfilterentries.Add(f);
                }
            }

            disablecombobox = true;

            comboBoxCustomCampaign.Items.Clear();
            foreach (FilterEntry f in displayedfilterentries)
                comboBoxCustomCampaign.Items.Add(f.NameDate);

            int cursel = current != null ? displayedfilterentries.FindIndex(x => x.UniqueID.Equals(current.UniqueID)) : -1;
            if (cursel >= 0)
                comboBoxCustomCampaign.SelectedIndex = cursel;

            disablecombobox = false;
        }

        bool disablecombobox = false;
        private void comboBoxCustomCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!disablecombobox && comboBoxCustomCampaign.SelectedIndex >= 0)
            {
                int entry = comboBoxCustomCampaign.SelectedIndex;

                FilterEntry f = displayedfilterentries[entry];
                if (f.Type == FilterEntry.EntryType.NewEntry)
                {
                    FilterEntry newe = new FilterEntry("Enter name", FilterEntry.EntryType.Time);
                    if ( EditEntry(newe, true, false) == DialogResult.OK )
                    {
                        current = newe;
                        savedfilterentries.Add(current);
                        FillCampaignCombo();
                    }
                }
                else
                {
                    current = f;
                }

                buttonExtEditCampaign.Enabled = current != null && current.Type == FilterEntry.EntryType.Time;
                Display();
            }
        }

        private void SelectInitial()
        {
            string sel = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingString(DbSave + "Selected", "Since Last Dock");

            if (!sel.IsEmpty())
            {
                int i = displayedfilterentries.FindIndex(x => x.UniqueID.Equals(sel));
                if (i >= 0)
                {
                    current = displayedfilterentries[i];
                    disablecombobox = true;
                    comboBoxCustomCampaign.SelectedIndex = i;
                    disablecombobox = false;

                    buttonExtEditCampaign.Enabled = current != null && current.Type == FilterEntry.EntryType.Time;
                }
            }
        }

        private void CheckCurrent()
        {
            if (current == null || displayedfilterentries.Find(x => x.UniqueID.Equals(current.UniqueID)) == null)
            {
                current = null;
                disablecombobox = true;
                comboBoxCustomCampaign.SelectedIndex = -1;
                disablecombobox = false;
            }
        }

        private void buttonExtEditCampaign_Click(object sender, EventArgs e)
        {
            if (current != null)        // should always be non null, but double check.  Note must be a saved entry, and we never remake these, only the mission ones
            {
                DialogResult res = EditEntry(current,false,true);
                if (res == DialogResult.Abort)
                {
                    savedfilterentries.Remove(current);
                    current = null;
                }

                CheckCurrent();
                FillCampaignCombo();
                Display();
            }
        }

        DialogResult EditEntry(FilterEntry entry, bool newentry, bool allowdel)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            DateTime starttime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(entry.StartTimeUTC);
            DateTime endtime = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(entry.EndTimeUTC);

            int width = 430;

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "Name:".T(EDTx.UserControlCombatPanel_Name), new Point(10, 40), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Name", typeof(ExtendedControls.ExtTextBox), entry.Name, new Point(100, 40), new Size(width - 100 - 20, 24), "Give name to campaign".T(EDTx.UserControlCombatPanel_C1)) { clearonfirstchar = newentry });

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "Faction:".T(EDTx.UserControlCombatPanel_Faction), new Point(10, 70), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Faction", typeof(ExtendedControls.ExtTextBox), entry.TargetFaction, new Point(100, 70), new Size(width - 100 - 20, 24), "Optional faction to target".T(EDTx.UserControlCombatPanel_C2)) );

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "Start:".T(EDTx.UserControlCombatPanel_Start), new Point(10, 100), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("DTS", typeof(ExtendedControls.ExtDateTimePicker), starttime.ToStringZulu(), new Point(100, 100), new Size(width - 100 - 20, 24), "Select Start time".T(EDTx.UserControlCombatPanel_C3)) { customdateformat = "yyyy-MM-dd HH:mm:ss" });

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "End:".T(EDTx.UserControlCombatPanel_End), new Point(10, 130), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("DTE", typeof(ExtendedControls.ExtDateTimePicker), endtime.ToStringZulu(), new Point(100, 130), new Size(width - 100 - 20, 24), "Select Start time".T(EDTx.UserControlCombatPanel_C4)) { customdateformat = "yyyy-MM-dd HH:mm:ss" });

            f.AddOK(new Point(width - 100, 180), "Press to Accept".T(EDTx.UserControlCombatPanel_C5));
            f.AddCancel(new Point(width - 200, 180), "Press to Cancel".T(EDTx.UserControlCombatPanel_C6));

            if ( allowdel )
                f.Add(new ExtendedControls.ConfigurableForm.Entry("Delete", typeof(ExtendedControls.ExtButton), "Delete".T(EDTx.Delete), new Point(10, 180), new Size(80, 24), "Press to Delete".T(EDTx.UserControlCombatPanel_C7)));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK")
                {
                    FilterEntry fe = displayedfilterentries.Find(x => x.UniqueID.Equals(f.Get("Name")));

                    if (fe != null && fe != entry)
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Name of campaign already in use, cannot overwrite".T(EDTx.UserControlCombatPanel_NoOverwrite), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        f.ReturnResult(DialogResult.OK);
                    }
                }
                else if (controlname == "Delete")
                {
                    if (f.Get("Name").Equals(entry.Name))
                    {
                        if (ExtendedControls.MessageBoxTheme.Show(this.FindForm(), string.Format("Confirm deletion of {0}".T(EDTx.UserControlCombatPanel_Condel), entry.Name), "Warning".T(EDTx.Warning), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK )
                        {
                            f.ReturnResult(DialogResult.Abort);
                        }
                    }
                    else
                    {
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Name changed - can't delete".T(EDTx.UserControlCombatPanel_NC), "Warning".T(EDTx.Warning), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if ( controlname == "Cancel" || controlname == "Close" )
                {
                    f.ReturnResult(DialogResult.Cancel);
                }
            };

            DialogResult res = f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon,  "Campaign".T(EDTx.Campaign) , closeicon:true);
            if (res == DialogResult.OK)
            {
                entry.Reset(f.Get("Name"), f.Get("Faction"),
                                EDDConfig.Instance.ConvertTimeToUTCFromSelected(f.GetDateTime("DTS").Value),
                                EDDConfig.Instance.ConvertTimeToUTCFromSelected(f.GetDateTime("DTE").Value));
            }

            return res;
        }

        #endregion

        #region Clicks

        int leftclickrow = -1;

        private void dataGridViewCombat_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridViewCombat.HandleClickOnDataGrid(e, out leftclickrow, out int unusedrightclickrow);
        }

        private void dataGridViewCombat_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (leftclickrow >= 0)                                                   // Click expands it..
            {
                DataGridViewRow row = dataGridViewCombat.Rows[leftclickrow];
                bool expanded = row.Cells[0].Tag != null;

                var leftclicksystem = (HistoryEntry)dataGridViewCombat.Rows[leftclickrow].Tag;

                leftclicksystem.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);

                if (expanded) // put it back to original text
                {
                    dataGridViewCombat.Rows[leftclickrow].Cells[2].Value = EventDescription;
                    row.Cells[0].Tag = null;

                    row.Cells[2].Style.WrapMode = row.Cells[1].Style.WrapMode = DataGridViewTriState.NotSet;
                }
                else
                {
                    dataGridViewCombat.Rows[leftclickrow].Cells[2].Value = EventDescription + ((EventDetailedInfo.Length > 0) ? (Environment.NewLine + EventDetailedInfo) : "");
                    row.Cells[0].Tag = true;
                    row.Cells[2].Style.WrapMode = row.Cells[1].Style.WrapMode = DataGridViewTriState.True;
                }
            }
        }

        #endregion
    }
}
