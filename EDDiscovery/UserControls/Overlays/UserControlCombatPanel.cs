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

            public DateTime StartTime { get { return starttime; } }
            public DateTime EndTime { get { return endtime; } }     // for missions, its the expiry time, not the MissionEndTime

            public string NameDate
            {
                get
                {
                    string date = "";
                    if (Type == FilterEntry.EntryType.Time || Type == FilterEntry.EntryType.Mission)
                    {
                        date = EDDiscoveryForm.EDDConfig.DisplayUTC ? starttime.ToString("d") : starttime.ToLocalTime().ToString("d");
                        string dte = EDDiscoveryForm.EDDConfig.DisplayUTC ? endtime.ToString("d") : endtime.ToLocalTime().ToString("d");
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
            string filter = SQLiteConnectionUser.GetSettingString(DbSave + "Campaign", "");
            List<string> filtarray = BaseUtils.StringParser.ParseWordList(filter);

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

            checkBoxCustomGridOn.Checked = SQLiteConnectionUser.GetSettingBool(DbSave + "Gridshow", false);
            checkBoxCustomGridOn.Visible = IsFloatingWindow;

            transparentfont = EDDTheme.Instance.GetFontAtSize(12);

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            labelTarget.Size = new Size(1280, 24);
            labelTarget.Text = "No Target".Tx(this, "NT");
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
                                    f.StartTime.ToStringZulu() + "," + f.EndTime.ToStringZulu() + ",";
            }

            SQLiteConnectionUser.PutSettingString(DbSave + "Campaign", s);

            SQLiteConnectionUser.PutSettingBool(DbSave + "Gridshow", checkBoxCustomGridOn.Checked);
            SQLiteConnectionUser.PutSettingString(DbSave + "Selected", current?.UniqueID ?? "");
        }

        public override void SetTransparency(bool on, Color curbackcol)
        {
            dataViewScrollerPanelCombat.Visible = !on || checkBoxCustomGridOn.Checked;
            panelTop.Visible = !on;
            panelStatus.BackColor = on ? Color.Transparent : curbackcol;

            Color fore = on ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;

            labelTarget.Font = labelCredits.Font = labelTotalKills.Font = labelTotalReward.Font = labelFactionKills.Font = labelFactionReward.Font = labelFaction.Font = labelTotalCrimes.Font = labelBalance.Font = on ? transparentfont : EDDTheme.Instance.GetFont;
            
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
            SortOrder sortorder = dataGridViewCombat.SortOrder;

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
                    hel = ml != null ? discoveryform.history.FilterByDateRangeLatestFirst(current.StartTime, ml.MissionEndTime) : new List<HistoryEntry>();
                }
                else
                    hel = discoveryform.history.FilterByDateRangeLatestFirst(current.StartTime, current.EndTime);

                foreach ( HistoryEntry he in hel )
                {
                    //System.Diagnostics.Debug.WriteLine("Combat Add {0} {1} {2}", he.EventTimeUTC.ToStringZulu(), he.EventSummary, he.EventDescription);
                    AddToGrid(he);
                }
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
                    tryadd = he.EventTimeUTC <= current.EndTime;
                else if (current.Type == FilterEntry.EntryType.Mission) // mission is limited, lookup mission and check end time
                {
                    MissionState ml = current.MissionKey != null && he.MissionList.Missions.ContainsKey(current.MissionKey) ? he.MissionList.Missions[current.MissionKey] : null;
                    tryadd = ml != null ? (he.EventTimeUTC <= ml.MissionEndTime) : false;
                }

                if (tryadd)
                {
                    if (AddToGrid(he, true))        // if did add..
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

        bool AddToGrid(HistoryEntry he , bool ins = false )
        {
            if (CreateEntry(he, out string summarycol, out string descriptioncol, out string rewardcol))
            {
                var rw = dataGridViewCombat.RowTemplate.Clone() as DataGridViewRow;
                he.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);

                rw.CreateCells(dataGridViewCombat, EDDiscoveryForm.EDDConfig.DisplayUTC ? he.EventTimeUTC : he.EventTimeLocal,
                    he.EventSummary, EventDescription, rewardcol);

                rw.Tag = he;

                if (ins)
                    dataGridViewCombat.Rows.Insert(0, rw);
                else
                    dataGridViewCombat.Rows.Add(rw);

                return true;
            }
            else
                return false;
        }

        static JournalTypeEnum[] jtelist = new JournalTypeEnum[]            // ones to display without any extra detail
        {
            JournalTypeEnum.Died,
            JournalTypeEnum.FighterDestroyed,JournalTypeEnum.FighterRebuilt,JournalTypeEnum.PVPKill,
            JournalTypeEnum.Interdicted,JournalTypeEnum.EscapeInterdiction,
            JournalTypeEnum.HeatDamage,JournalTypeEnum.HullDamage,
            JournalTypeEnum.SRVDestroyed
        };

        bool CreateEntry( HistoryEntry he, out string summarycol, out string descriptioncol, out string rewardcol )
        {
            rewardcol = "";

            he.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);

            summarycol = he.EventSummary;
            descriptioncol = EventDescription;

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
            labelTotalKills.Text = (total_kills>0) ? ("Kills:".Tx(this) + total_kills.ToStringInvariant()) : "";
            labelFactionKills.Text = faction ? ("Faction:".Tx(this) + faction_kills.ToStringInvariant()) : "";
            labelFaction.Text = faction ? (current.TargetFaction) : "";
            labelTotalCrimes.Text = (total_crimes>0) ? ("Crimes:".Tx(this) + total_crimes.ToStringInvariant()) : "";

            labelCredits.Text = (discoveryform.history.GetLast != null) ? (discoveryform.history.GetLast.Credits.ToString("N0") + "cr") : "";
            labelBalance.Text = (balance > 0 ) ? ("Bal:".Tx(this) + balance.ToString("N0") + "cr") : "";
            labelFactionReward.Text = (faction && faction_reward != balance) ? (faction_reward.ToString("N0") + "cr") : "";
            labelTotalReward.Text = (total_reward != balance) ? (total_reward.ToString("N0") + "cr") : "";

        }

        static JournalTypeEnum[] targetofflist = new JournalTypeEnum[]            // ones to display without any extra detail
        {
            JournalTypeEnum.Died,
            JournalTypeEnum.FighterDestroyed,
            JournalTypeEnum.SupercruiseEntry,
            JournalTypeEnum.FSDJump,
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
                labelTarget.Text = "No Target".Tx(this, "NT");
            }
        }

        #region UI

        public void FillCampaignCombo()
        {
            displayedfilterentries = new List<FilterEntry>();

            displayedfilterentries.Add(new FilterEntry("New Campaign".Tx(this), FilterEntry.EntryType.NewEntry));
            displayedfilterentries.Add(new FilterEntry("Since Last Dock".Tx(this), FilterEntry.EntryType.Lastdock));
            displayedfilterentries.Add(new FilterEntry("Today".Tx(), FilterEntry.EntryType.Today));
            displayedfilterentries.Add(new FilterEntry("24h".Tx(), FilterEntry.EntryType.Oneday));
            displayedfilterentries.Add(new FilterEntry("7 days".Tx(), FilterEntry.EntryType.Sevendays));
            displayedfilterentries.Add(new FilterEntry("All".Tx(), FilterEntry.EntryType.All));

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
            string sel = SQLiteConnectionUser.GetSettingString(DbSave + "Selected", "Since Last Dock");

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

            DateTime start = EDDConfig.Instance.DisplayUTC ? entry.StartTime : entry.StartTime.ToLocalTime();
            DateTime end = EDDConfig.Instance.DisplayUTC ? entry.EndTime : entry.EndTime.ToLocalTime();

            int width = 430;

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "Name:".Tx(this), new Point(10, 40), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Name", typeof(ExtendedControls.TextBoxBorder), entry.Name, new Point(100, 40), new Size(width - 100 - 20, 24), "Give name to campaign".Tx(this, "C1")) { clearonfirstchar = newentry });

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "Faction:".Tx(this), new Point(10, 70), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Faction", typeof(ExtendedControls.TextBoxBorder), entry.TargetFaction, new Point(100, 70), new Size(width - 100 - 20, 24), "Optional faction to target".Tx(this, "C2")) );

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "Start:".Tx(this), new Point(10, 100), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("DTS", typeof(ExtendedControls.CustomDateTimePicker), entry.StartTime.ToStringZulu(), new Point(100, 100), new Size(width - 100 - 20, 24), "Select Start time".Tx(this, "C3")) { customdateformat = "yyyy-MM-dd HH:mm:ss" });

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L", typeof(Label), "End:".Tx(this), new Point(10, 130), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("DTE", typeof(ExtendedControls.CustomDateTimePicker), entry.EndTime.ToStringZulu(), new Point(100, 130), new Size(width - 100 - 20, 24), "Select Start time".Tx(this, "C4")) { customdateformat = "yyyy-MM-dd HH:mm:ss" });

            f.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ButtonExt), "OK".Tx(), new Point(width - 100, 180), new Size(80, 24), "Press to Accept".Tx(this, "C5")));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Cancel", typeof(ExtendedControls.ButtonExt), "Cancel".Tx(), new Point(width - 200, 180), new Size(80, 24), "Press to Cancel".Tx(this, "C6")));
            if ( allowdel )
                f.Add(new ExtendedControls.ConfigurableForm.Entry("Delete", typeof(ExtendedControls.ButtonExt), "Delete".Tx(), new Point(10, 180), new Size(80, 24), "Press to Delete".Tx(this, "C7")));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK")
                {
                    FilterEntry fe = displayedfilterentries.Find(x => x.UniqueID.Equals(f.Get("Name")));

                    if (fe != null && fe != entry)
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Name of campaign already in use, cannot overwrite".Tx(this, "NoOverwrite"), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        f.DialogResult = DialogResult.OK;
                        f.Close();
                    }
                }
                else if (controlname == "Delete")
                {
                    if (f.Get("Name").Equals(entry.Name))
                    {
                        if (ExtendedControls.MessageBoxTheme.Show(this.FindForm(), string.Format("Confirm deletion of {0}".Tx(this,"Condel"), entry.Name), "Warning".Tx(), MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK )
                        {
                            f.DialogResult = DialogResult.Abort;
                            f.Close();
                        }
                    }
                    else
                    {
                        ExtendedControls.MessageBoxTheme.Show(this.FindForm(), "Name changed - can't delete".Tx(this,"NC"), "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if ( controlname == "Cancel" )
                {
                    f.DialogResult = DialogResult.Cancel;
                    f.Close();
                }
            };

            DialogResult res = f.ShowDialog(this.FindForm(), this.FindForm().Icon, new Size(width, 220), new Point(-999, -999), "Campaign".Tx());
            if (res == DialogResult.OK)
            {
                entry.Reset(f.Get("Name"), f.Get("Faction"),
                                EDDConfig.Instance.DisplayUTC ? f.GetDateTime("DTS").Value : f.GetDateTime("DTS").Value.ToUniversalTime(),
                                EDDConfig.Instance.DisplayUTC ? f.GetDateTime("DTE").Value : f.GetDateTime("DTE").Value.ToUniversalTime());
            }

            return res;
        }

        #endregion

        #region Clicks

        HistoryEntry rightclicksystem = null;
        int rightclickrow = -1;
        HistoryEntry leftclicksystem = null;
        int leftclickrow = -1;

        private void dataGridViewCombat_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclicksystem = null;
                rightclickrow = -1;
            }
            if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
            {
                leftclicksystem = null;
                leftclickrow = -1;
            }

            if (dataGridViewCombat.SelectedCells.Count < 2 || dataGridViewCombat.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewCombat.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewCombat.ClearSelection();                // select row under cursor.
                    dataGridViewCombat.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                        rightclicksystem = (HistoryEntry)dataGridViewCombat.Rows[hti.RowIndex].Tag;
                    }
                    if (e.Button == MouseButtons.Left)         // right click on travel map, get in before the context menu
                    {
                        leftclickrow = hti.RowIndex;
                        leftclicksystem = (HistoryEntry)dataGridViewCombat.Rows[hti.RowIndex].Tag;
                    }
                }
            }
        }

        private const int DefaultRowHeight = 26;

        private void dataGridViewCombat_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (leftclickrow >= 0)                                                   // Click expands it..
            {
                int ch = dataGridViewCombat.Rows[leftclickrow].Height;
                bool toexpand = (ch <= DefaultRowHeight);

                leftclicksystem.journalEntry.FillInformation(out string EventDescription, out string EventDetailedInfo);
                string infotext = EventDescription + ((toexpand && EventDetailedInfo.Length > 0) ? (Environment.NewLine + EventDetailedInfo) : "");

                int h = DefaultRowHeight;

                if (toexpand)
                {
                    using (Graphics g = Parent.CreateGraphics())
                    {
                        int desch = (int)(g.MeasureString((string)dataGridViewCombat.Rows[leftclickrow].Cells[1].Value, dataGridViewCombat.Font, dataGridViewCombat.Columns[1].Width - 4).Height + 2);
                        int infoh = (int)(g.MeasureString(infotext, dataGridViewCombat.Font, dataGridViewCombat.Columns[2].Width - 4).Height + 2);

                        h = Math.Max(desch, h);
                        h = Math.Max(infoh, h);
                        h += 20;
                    }
                }

                toexpand = (h > DefaultRowHeight);      // now we have our h, is it bigger? If so, we need to go into wrap mode

                dataGridViewCombat.Rows[leftclickrow].Height = h;
                dataGridViewCombat.Rows[leftclickrow].Cells[2].Value = infotext;

                DataGridViewTriState ti = (toexpand) ? DataGridViewTriState.True : DataGridViewTriState.False;

                dataGridViewCombat.Rows[leftclickrow].Cells[1].Style.WrapMode = ti;
                dataGridViewCombat.Rows[leftclickrow].Cells[2].Style.WrapMode = ti;
            }
        }

        #endregion
    }
}
