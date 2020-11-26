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

        public MissionListUserControl()
        {
            InitializeComponent();

            var corner = dataGridView.TopLeftHeaderCell; // work around #1487
            dataGridView.MakeDoubleBuffered();
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            panelButtons.Visible = false;
            labelValue.Visible = false;
        }

        public void SetMinimumHeight(int m)
        {
            dataGridView.RowTemplate.MinimumHeight = m;
        }

        public void SetDateTime(DateTime start, bool startchecked, DateTime end, bool endchecked)
        {
            customDateTimePickerStart.Value = start;
            customDateTimePickerStart.Checked = startchecked;
            customDateTimePickerEnd.Value = end;
            customDateTimePickerEnd.Checked = endchecked;
            VerifyDates();
            panelButtons.Visible = true;
        }

        DataGridViewColumn sortcolcur;
        SortOrder sortordercur;
        long totalreward;
        int completed, abandonded, failed;

        public void Clear()
        {
            sortcolcur = dataGridView.SortedColumn != null ? dataGridView.SortedColumn : dataGridView.Columns[1];
            sortordercur = dataGridView.SortedColumn != null ? dataGridView.SortOrder : SortOrder.Descending;
            dataGridView.Rows.Clear();
            totalreward = 0;
            completed = abandonded = failed = 0;
        }

        public void Add(MissionState ms, bool previousmissions)
        {
            bool show = true;
            if (panelButtons.Visible)
            {
                DateTime startdateutc = customDateTimePickerStart.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(customDateTimePickerStart.Value) : new DateTime(1980, 1, 1);
                DateTime enddateutc = customDateTimePickerEnd.Checked ? EDDConfig.Instance.ConvertTimeToUTCFromSelected(customDateTimePickerEnd.Value) : new DateTime(8999, 1, 1);
                show = DateTime.Compare(ms.Mission.EventTimeUTC, startdateutc) >= 0 && DateTime.Compare(ms.Mission.EventTimeUTC, enddateutc) <= 0;
            }

            if (show)
            {
                object[] rowobj = { JournalFieldNaming.ShortenMissionName(ms.Mission.LocalisedName) ,
                                    EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(ms.Mission.EventTimeUTC),
                                    EDDiscoveryForm.EDDConfig.ConvertTimeToSelectedFromUTC(ms.Mission.Expiry),
                                    ms.OriginatingSystem + ": " + ms.OriginatingStation,
                                    ms.Mission.Faction,
                                    ms.DestinationSystemStation(),
                                    ms.Mission.TargetFaction,
                                    previousmissions ? ms.StateText : ms.Mission.Reward.GetValueOrDefault().ToString("N0"),
                                    ms.MissionInfoColumn()
                };

                if (ms.State == MissionState.StateTypes.Abandoned)
                    abandonded++;
                else if (ms.State == MissionState.StateTypes.Completed)
                    completed++;
                else if (ms.State == MissionState.StateTypes.Failed)
                    failed++;

                if (previousmissions)
                {
                    totalreward += ms.Value;
                }
                else if (ms.Mission.Reward.HasValue)
                {
                    totalreward += ms.Mission.Reward.Value;
                }

                var row = dataGridView.RowTemplate.Clone() as DataGridViewRow;
                row.CreateCells(dataGridView, rowobj);
                row.Tag = ms;
                dataGridView.Rows.Add(row);
            }
        }

        public void Finish()
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
                customDateTimePickerEnd.Value = customDateTimePickerStart.Value = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(DateTime.UtcNow);
            }
        }

    }
}
