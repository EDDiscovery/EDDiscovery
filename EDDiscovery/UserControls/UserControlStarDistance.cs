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
using System.Collections.Concurrent;
using System.Threading;
using EDDiscovery.DB;
using EDDiscovery.EDSM;
using EDDiscovery.EliteDangerous;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStarDistance : UserControlCommonBase
    {
        EDDiscoveryForm _discoveryForm;
        TravelHistoryControl _travelcontrol;

        public UserControlStarDistance()
        {
            InitializeComponent();
            Name = "Stars";
        }

        public override void Init( EDDiscoveryForm ed, int vn) //0=primary, 1 = first windowed version, etc
        {
            _discoveryForm = ed;
            _travelcontrol = _discoveryForm.TravelControl;
            _travelcontrol.OnNearestStarListChanged += FillGrid;
        }

        public override void Closing()
        {
            _travelcontrol.OnNearestStarListChanged -= FillGrid;
        }

        public void FillGrid(string name, SortedList<double, ISystem> csl)
        {
            SetControlText("");
            dataGridViewNearest.Rows.Clear();

            if (csl.Count() > 0)
            {
                SetControlText("Closest systems from " + name);
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    object[] rowobj = { tvp.Value.name, Math.Sqrt(tvp.Key).ToString("0.00") };       // distances are stored squared for speed, back to normal.
                    int rowindex = dataGridViewNearest.Rows.Add(rowobj);
                    dataGridViewNearest.Rows[rowindex].Tag = tvp.Value;
                }
            }

        }

        private void addToTrilaterationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            TrilaterationControl tctrl = _discoveryForm.trilaterationControl;

            IEnumerable<DataGridViewRow> selectedRows = dataGridViewNearest.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            string sysName = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                sysName = r.Cells[0].Value.ToString();

                tctrl.AddSystemToDataGridViewDistances(sysName);
            }

            this.Cursor = Cursors.Default;
        }

        private void viewOnEDSMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewNearest.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            ISystem system = (ISystem)selectedRows.First<DataGridViewRow>().Tag;
            EDSMClass edsm = new EDSMClass();

            if (!edsm.ShowSystemInEDSM(system.name, system.id_edsm))
            {
                ExtendedControls.MessageBoxTheme.Show("System could not be found - has not been synched or EDSM is unavailable");
            }

            this.Cursor = Cursors.Default;
        }
    }
}
