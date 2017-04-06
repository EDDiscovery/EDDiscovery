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
using EDDiscovery.Controls;
using EDDiscovery2.DB;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery.UserControls
{
    public partial class UserControlMissions : UserControlCommonBase
    {
        private int displaynumber = 0;
        private EDDiscoveryForm discoveryform;
        
        private string DbColumnSave { get { return ("MissionsGrid") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }

        #region Init

        public UserControlMissions()
        {
            InitializeComponent();
            Name = "Missions";
        }

        public override void Init(EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            dataGridViewCurrent.MakeDoubleBuffered();
            dataGridViewCurrent.RowTemplate.Height = 26;
            dataGridViewCurrent.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewCurrent.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            dataGridViewPrevious.MakeDoubleBuffered();
            dataGridViewPrevious.RowTemplate.Height = 26;
            dataGridViewPrevious.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewPrevious.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx

            discoveryform.OnNewEntry += Discoveryform_OnNewEntry;
            ed.TravelControl.OnTravelSelectionChanged += Display;

        }

        #endregion

        #region Display

        private void Discoveryform_OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }

        HistoryEntry last_he = null;
        public override void Display(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            Display();
        }

        public void Display()
        {
            MissionList ml = last_he?.MissionList;

            dataGridViewCurrent.Rows.Clear();
            dataGridViewPrevious.Rows.Clear();

            if (ml != null)
            {
                DateTime hetime = last_he.EventTimeUTC;

                List<MissionState> mcurrent = (from MissionState ms in ml.Missions.Values where ms.InProgressDateTime(hetime) orderby ms.Mission.EventTimeUTC descending select ms).ToList();

                foreach (MissionState ms in mcurrent)
                {
                    object[] rowobj = { JournalFieldNaming.ShortenMissionName(ms.Mission.Name) ,
                                        EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.EventTimeUTC : ms.Mission.EventTimeLocal,
                                        EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.Expiry : ms.Mission.Expiry.ToLocalTime(),
                                        ms.OriginatingSystem + ":" + ms.OriginatingStation,
                                        ms.Mission.Faction,
                                        ms.Mission.DestinationSystem + ((ms.Mission.DestinationStation.Length>0) ? ":" +ms.Mission.DestinationStation :""),
                                        ms.Mission.TargetFaction,
                                        ms.Info()
                    };

                    int rowno = dataGridViewCurrent.Rows.Add(rowobj);
                    dataGridViewCurrent.Rows[rowno].Tag = ms;
                }

                List<MissionState> mprev = (from MissionState ms in ml.Missions.Values where !ms.InProgressDateTime(hetime) orderby ms.Mission.EventTimeUTC descending select ms).ToList();

                foreach (MissionState ms in mprev)
                {
                    object[] rowobj = { JournalFieldNaming.ShortenMissionName(ms.Mission.Name) ,
                                        EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.EventTimeUTC : ms.Mission.EventTimeLocal,
                                        EDDiscoveryForm.EDDConfig.DisplayUTC ? ms.Mission.Expiry : ms.Mission.Expiry.ToLocalTime(),
                                        ms.OriginatingSystem + ":" + ms.OriginatingStation,
                                        ms.Mission.Faction,
                                        ms.Mission.DestinationSystem + ((ms.Mission.DestinationStation.Length>0) ? ":" +ms.Mission.DestinationStation :""),
                                        ms.Mission.TargetFaction,
                                        ms.StateText,
                                        ms.Info()
                    };

                    int rowno = dataGridViewPrevious.Rows.Add(rowobj);
                    dataGridViewPrevious.Rows[rowno].Tag = ms;
                }
            }
        }

        #endregion

        #region Layout

        public override void LoadLayout()
        {
            //DGVLoadColumnLayout(dataGridViewCurrent, DbColumnSave);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewCurrent, DbColumnSave);

            discoveryform.TravelControl.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= Discoveryform_OnNewEntry;

            //DB.SQLiteDBClass.PutSettingString(DbOSave, Order.ToString(","));
            //DB.SQLiteDBClass.PutSettingString(DbWSave, Wanted.ToString(","));
            //DB.SQLiteDBClass.PutSettingString(DbSelSave, (string)comboBoxSynthesis.SelectedItem);
        }

        #endregion
    }
}
