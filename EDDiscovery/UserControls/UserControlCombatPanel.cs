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

namespace EDDiscovery.UserControls
{
    public partial class UserControlCombatPanel : UserControlCommonBase
    {
        private string DbSave { get { return "CombatPanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        public UserControlCombatPanel()
        {
            InitializeComponent();
        }

        class FilterEntry
        {
            public enum EntryType { invalid, time, mission, lastdock , newentry }
            public EntryType type;
            public string name;
            public DateTime start;      // UTC
            public DateTime end;        // UTC
            public EliteDangerousCore.MissionState mission;     // missions only, else null

            public FilterEntry(string n, EntryType t)
            {
                name = n;
                type = t;
                start = end = DateTime.UtcNow;
            }

            public FilterEntry(EliteDangerousCore.MissionState m)
            {
                type = EntryType.mission;
                name = m.Mission.Name;
                start = m.Mission.EventTimeUTC;
                end = m.Mission.Expiry;
                mission = m;
            }

            public FilterEntry(string[] filtarray, int i)
            {
                type = (FilterEntry.EntryType)Enum.Parse(typeof(FilterEntry.EntryType), filtarray[i]);
                name = filtarray[i + 1];

                // keep the UTC marker in the time/date
                if (!DateTime.TryParse(filtarray[i + 2], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind, out start) ||
                    !DateTime.TryParse(filtarray[i + 3], System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind, out end))
                {
                    type = EntryType.invalid;
                }
            }
        }

        List<FilterEntry> savedfilterentries;
        List<FilterEntry> displayedfilterentries;

        public override void Init()
        {
            string filter = SQLiteConnectionUser.GetSettingString(DbSave + "Campaign", "");
            string[] filtarray = filter.Split(';');

            savedfilterentries = new List<FilterEntry>();

            for (int i = 0; i < filtarray.Length / 4; i++)
            {
                FilterEntry f = new FilterEntry(filtarray, i * 4);
                if (f.type != FilterEntry.EntryType.invalid)
                {
                    savedfilterentries.Add(f);
                }
            }

            buttonExtEditCampaign.Enabled = false;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
        }

        public override void InitialDisplay()
        {
        }

        public override void Closing()
        {
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;

            string s = "";
            foreach (FilterEntry f in savedfilterentries)
                s += f.type.ToString() + ";" + f.name + ";" + f.start.ToStringZulu() + ";" + f.end.ToStringZulu();
            SQLiteConnectionUser.PutSettingString(DbSave + "Campaign", s);
        }


        private void Discoveryform_OnHistoryChange(EliteDangerousCore.HistoryList obj)
        {
            FillCampaignCombo();
        }

        private void Discoveryform_OnNewEntry(EliteDangerousCore.HistoryEntry arg1, EliteDangerousCore.HistoryList arg2)
        {
            Display();
        }


        public  void Display()
        {

        }

        #region UI

        bool disablecombobox = false;
        public void FillCampaignCombo()
        {
            displayedfilterentries = new List<FilterEntry>();

            displayedfilterentries.Add(new FilterEntry("New Entry", FilterEntry.EntryType.newentry));
            displayedfilterentries.Add(new FilterEntry("Since Last Dock", FilterEntry.EntryType.lastdock));

            displayedfilterentries.AddRange(savedfilterentries);

            var missionlist = discoveryform.history.GetLast?.MissionList.GetAllCombatMissions();

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
                comboBoxCustomCampaign.Items.Add(f.name);

            disablecombobox = false;
        }

        private void comboBoxCustomCampaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!disablecombobox && comboBoxCustomCampaign.SelectedIndex >= 0)
            {
                FilterEntry f = displayedfilterentries[comboBoxCustomCampaign.SelectedIndex];
                if (f.type == FilterEntry.EntryType.newentry)
                {
                    EditEntry(-1);
                }
                else 
                {
                    buttonExtEditCampaign.Enabled = f.type == FilterEntry.EntryType.time;
                    Display();
                }
            }
        }

        private void buttonExtEditCampaign_Click(object sender, EventArgs e)
        {
            if (comboBoxCustomCampaign.SelectedIndex >= 0)
                EditEntry(comboBoxCustomCampaign.SelectedIndex);
        }

        void EditEntry(int edit)
        {
            FilterEntry entry = (edit >= 0) ? displayedfilterentries[edit] : new FilterEntry("Enter name", FilterEntry.EntryType.time);

            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            DateTime start = EDDConfig.Instance.DisplayUTC ? entry.start : entry.start.ToLocalTime();
            DateTime end = EDDConfig.Instance.DisplayUTC ? entry.end : entry.end.ToLocalTime();

            int width = 430;

            f.Add(new ExtendedControls.ConfigurableForm.Entry("L1", typeof(Label), "Name:", new Point(10, 40), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Name", typeof(ExtendedControls.TextBoxBorder), entry.name, new Point(100, 40), new Size(width - 100 - 20, 24), "Give name to campaign") { clearonfirstchar = edit == -1 });
            f.Add(new ExtendedControls.ConfigurableForm.Entry("L2", typeof(Label), "Start:", new Point(10, 70), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("DTS", typeof(ExtendedControls.CustomDateTimePicker), entry.start.ToStringZulu(), new Point(100, 70), new Size(width - 100 - 20, 24), "Select Start time") { customdateformat = "yyyy-MM-dd HH:mm:ss" });
            f.Add(new ExtendedControls.ConfigurableForm.Entry("L2", typeof(Label), "End:", new Point(10, 100), new Size(80, 24), ""));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("DTE", typeof(ExtendedControls.CustomDateTimePicker), entry.end.ToStringZulu(), new Point(100, 100), new Size(width - 100 - 20, 24), "Select Start time") { customdateformat = "yyyy-MM-dd HH:mm:ss" });

            f.Add(new ExtendedControls.ConfigurableForm.Entry("OK", typeof(ExtendedControls.ButtonExt), "OK", new Point(width - 100, 140), new Size(80, 24), "Press to Accept"));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Cancel", typeof(ExtendedControls.ButtonExt), "Cancel", new Point(width - 200, 140), new Size(80, 24), "Press to Cancel"));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "OK" || controlname == "Cancel")
                {
                    f.DialogResult = (controlname == "OK") ? DialogResult.OK : DialogResult.Cancel;
                    f.Close();
                }
            };

            if (f.ShowDialog(this.FindForm(), this.FindForm().Icon, new Size(width, 200), new Point(-999, -999), "Campaign") == DialogResult.OK)
            {
                entry.start = f.GetDateTime("DTS").Value;        // okay, will get the with the same Kind on them
                entry.end = f.GetDateTime("DTE").Value;

                if (!EDDConfig.Instance.DisplayUTC)
                {
                    entry.start = entry.start.ToUniversalTime();
                    entry.end = entry.end.ToUniversalTime();
                }

                entry.name = f.Get("Name").Replace(";","_");

                if ( edit == -1 )
                {
                    savedfilterentries.Add(entry);
                    FillCampaignCombo();
                }
            }
        }

        #endregion

    }
}
