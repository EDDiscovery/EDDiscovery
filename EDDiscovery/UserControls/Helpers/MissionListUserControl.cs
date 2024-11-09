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
using EliteDangerousCore;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace EDDiscovery.UserControls.Helpers
{
    public partial class MissionListUserControl : UserControl
    {
        public Action DateTimeChanged;
        public Action SearchTextChanged;
        public string SearchText { get { return extTextBoxSearch.Text; } }
        private Timer searchtimer;

        public MissionListUserControl()
        {
            InitializeComponent();

            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            panelButtons.Visible = false;
            labelValue.Visible = false;
            extTextBoxSearch.TextChanged += (s, e) => { searchtimer.Stop(); searchtimer.Start(); };
            searchtimer = new Timer();
            searchtimer.Interval = 500;
            searchtimer.Tick += (x,y) => { searchtimer.Stop(); SearchTextChanged.Invoke(); };
        }

        public void Closing()
        {
            searchtimer.Stop();
        }

        public void SetMinimumHeight(int m)
        {
            dataGridView.RowTemplate.MinimumHeight = m;
        }

        // time and dates are in picker time, ie whatever they were set to last.
        public void SetDateTime(DateTime startpickertime, bool startchecked, DateTime endpickertime, bool endchecked)
        {
            customDateTimePickerStart.Value = startpickertime.StartOfDay();     // force start/end days
            customDateTimePickerStart.Checked = startchecked;
            customDateTimePickerEnd.Value = endpickertime.EndOfDay();
            customDateTimePickerEnd.Checked = endchecked;
            VerifyDates();
            panelButtons.Visible = true;
        }

        DataGridViewColumn sortcolcur;
        SortOrder sortordercur;
        long totalreward;
        int completed, abandonded, failed;

        public void Start()
        {
            sortcolcur = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[1];
            sortordercur = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;
            dataGridView.Rows.Clear();
            totalreward = 0;
            completed = abandonded = failed = 0;
            extPanelDataGridViewScroll.Suspend();
        }

        public void Add(MissionState ms, bool previousmissions, string search)
        {
            bool show = true;
            if (panelButtons.Visible)
            {
                // Start of day/end is being paranoid
                DateTime startdateutc = customDateTimePickerStart.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(customDateTimePickerStart.Value.StartOfDay()) : EliteDangerousCore.EliteReleaseDates.GameRelease;
                DateTime enddateutc = customDateTimePickerEnd.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromPicker(customDateTimePickerEnd.Value.EndOfDay()) : EliteDangerousCore.EliteReleaseDates.GameEndTime;
                show = DateTime.Compare(ms.Mission.EventTimeUTC, startdateutc) >= 0 && DateTime.Compare(ms.Mission.EventTimeUTC, enddateutc) <= 0;
            }

            if (show)
            {
                string[] rowobj = { JournalFieldNaming.ShortenMissionName(ms.Mission.LocalisedName) ,
                                    EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ms.Mission.EventTimeUTC).ToString(),
                                    EDDConfig.Instance.ConvertTimeToSelectedFromUTC(ms.Mission.Expiry).ToString(),
                                    ms.OriginatingSystem.Name + ": " + ms.OriginatingStation,
                                    ms.Mission.Faction,
                                    ms.DestinationSystemStationSettlement(),
                                    ms.Mission.TargetFaction,
                                    previousmissions ? ms.StateText() : ms.Mission.Reward.GetValueOrDefault().ToString("N0"),
                                    ms.MissionInfoColumn()
                };

                if ( search.HasChars() )
                {
                    if (Array.Find(rowobj, x => x.Contains(search, StringComparison.InvariantCultureIgnoreCase)) == null)
                        return;
                }

                if (ms.State == MissionState.StateTypes.Abandoned)
                    abandonded++;
                else if (ms.State == MissionState.StateTypes.Completed)
                    completed++;
                else if (ms.State == MissionState.StateTypes.Failed)
                    failed++;

                if (previousmissions)
                {
                    totalreward += ms.Value();
                }
                else if (ms.Mission.Reward.HasValue)
                {
                    totalreward += ms.Mission.Reward.Value;
                }

                var row = dataGridView.RowTemplate.Clone() as DataGridViewRow;
                row.CreateCells(dataGridView, rowobj);
                row.Tag = ms;
                dataGridView.Rows.Add(row);
                //System.Diagnostics.Debug.WriteLine($"Add mission JournalFieldNaming.ShortenMissionName(ms.Mission.LocalisedName) {ms.State}");
            }
        }

        public void CompletedFill()
        {
            if (panelButtons.Visible)
            {
                labelValue.Visible = (totalreward != 0);
                labelValue.Text = "Value: ".T(EDTx.UserControlMissions_ValueC) + totalreward.ToString("N0") + " C:" + completed.ToString("N0") + " A:" + abandonded.ToString("N0") + " F:" + failed.ToString("N0");
            }

            int count = dataGridView.RowCount;
            
            PcolName.HeaderText = (count > 0) ? (count.ToString() + (count > 1 ? " Missions".T(EDTx.UserControlMissions_MPlural) : " Mission".T(EDTx.UserControlMissions_MSingular))) : "Name".T(EDTx.UserControlMissions_Name);
            pColResult.HeaderText = (totalreward != 0) ? string.Format("Value (cr):\n{0:N0}".T(EDTx.UserControlMissions_Value), totalreward) : "Value (cr)".T(EDTx.UserControlMissions_ValueN);

            dataGridView.Sort(sortcolcur, (sortordercur == SortOrder.Descending) ? ListSortDirection.Descending : ListSortDirection.Ascending);
            dataGridView.Columns[sortcolcur.Index].HeaderCell.SortGlyphDirection = sortordercur;
        }

        public void Finish()
        {
            extPanelDataGridViewScroll.Resume();
        }

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == 1 || e.Column.Index == 2)
                e.SortDataGridViewColumnDate();
            else if (e.Column.Index == 7)
                e.SortDataGridViewColumnNumeric();
        }

        private void customDateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
            VerifyDates();
            DateTimeChanged?.Invoke();
        }

        private void customDateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
            VerifyDates();
            DateTimeChanged?.Invoke();
        }

        public void VerifyDates()
        {
            if (!EDDConfig.Instance.DateTimeInRangeForGame(customDateTimePickerStart.Value) || !EDDConfig.Instance.DateTimeInRangeForGame(customDateTimePickerEnd.Value))
            {
                customDateTimePickerStart.Checked = customDateTimePickerEnd.Checked = false;
                customDateTimePickerStart.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EliteDangerousCore.EliteReleaseDates.GameRelease);
                customDateTimePickerEnd.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow.EndOfDay());
            }
        }

    }
}
