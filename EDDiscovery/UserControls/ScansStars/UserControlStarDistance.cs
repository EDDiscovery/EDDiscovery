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
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStarDistance : UserControlCommonBase
    {
        public UserControlStarDistance()
        {
            InitializeComponent();
            var corner = dataGridViewNearest.TopLeftHeaderCell; // work around #1487
        }

        private string DbSave { get { return DBName("StarDistancePanel" ); } }

        private StarDistanceComputer computer = null;
        private HistoryEntry last_he = null;
        private ISystem rightclicksystem = null;
        private int rightclickrow = -1;

        private const double defaultMaxRadius = 1000;
        private const double defaultMinRadius = 0;
        private const int maxitems = 500;

        public override void Init()
        {
            computer = new StarDistanceComputer();

            textMinRadius.ValueNoChange = SQLiteConnectionUser.GetSettingDouble(DbSave + "Min", defaultMinRadius);
            textMaxRadius.ValueNoChange = SQLiteConnectionUser.GetSettingDouble(DbSave + "Max", defaultMaxRadius);
            textMinRadius.SetComparitor(textMaxRadius, -2);     // need to do this after values are set
            textMaxRadius.SetComparitor(textMinRadius, 2);

            checkBoxCube.Checked = SQLiteConnectionUser.GetSettingBool(DbSave + "Behaviour", false);

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void LoadLayout()
        {
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void Closing()
        {
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            SQLiteConnectionUser.PutSettingDouble(DbSave + "Min", textMinRadius.Value);
            SQLiteConnectionUser.PutSettingDouble(DbSave + "Max", textMaxRadius.Value);
            SQLiteConnectionUser.PutSettingBool(DbSave + "Behaviour", checkBoxCube.Checked);
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry, true);
        }

        private void Discoveryform_OnHistoryChange(HistoryList obj)
        {
            KickComputation(obj.GetLast);   // copes with getlast = null
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            KickComputation(he);
        }

        private void KickComputation(HistoryEntry he, bool force = false)
        {
            if (he != null && he.System != null && (force || !he.System.Equals(last_he?.System)))
            {
                last_he = he;

                //System.Diagnostics.Debug.WriteLine("Star grid started, uctg selected, ask");

                // Get nearby systems from the systems DB.
                computer.CalculateClosestSystems(he.System, 
                    (s, d) => BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }) , 
                    maxitems, textMinRadius.Value, textMaxRadius.Value, !checkBoxCube.Checked
                    );     // hook here, force closes system update
            }
        }

        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!

            // Get nearby systems from our travel history. This will filter out duplicates from the systems DB.
            discoveryform.history.CalculateSqDistances(list, sys.X, sys.Y, sys.Z,
                                maxitems, textMinRadius.Value, textMaxRadius.Value, !checkBoxCube.Checked
                                );

            FillGrid(sys.Name, list);
        }

        private void FillGrid(string name, BaseUtils.SortedListDoubleDuplicate<ISystem> csl)
        {
            dataGridViewNearest.Rows.Clear();

            if (csl != null && csl.Any())
            {
                SetControlText(string.Format("From {0}".Tx(this,"From"), name));
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    double dist = Math.Sqrt(tvp.Key);   // distances are stored squared for speed, back to normal.

                    if (tvp.Value.Name != name && (checkBoxCube.Checked || (dist >= textMinRadius.Value && dist <= textMaxRadius.Value)))
                    {
                        int visits = discoveryform.history.GetVisitsCount(tvp.Value.Name, tvp.Value.EDSMID);
                        object[] rowobj = { tvp.Value.Name, $"{dist:0.00}", $"{visits:n0}" };

                        int rowindex = dataGridViewNearest.Rows.Add(rowobj);
                        dataGridViewNearest.Rows[rowindex].Tag = tvp.Value;
                    }
                }
            }
            else
            {
                SetControlText(string.Empty);
            }
        }

        private void dataGridViewNearest_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
            {
                rightclicksystem = null;
                rightclickrow = -1;
            }

            if (dataGridViewNearest.SelectedCells.Count < 2 || dataGridViewNearest.SelectedRows.Count == 1)      // if single row completely selected, or 1 cell or less..
            {
                DataGridView.HitTestInfo hti = dataGridViewNearest.HitTest(e.X, e.Y);
                if (hti.Type == DataGridViewHitTestType.Cell)
                {
                    dataGridViewNearest.ClearSelection();                // select row under cursor.
                    dataGridViewNearest.Rows[hti.RowIndex].Selected = true;

                    if (e.Button == MouseButtons.Right)         // right click on travel map, get in before the context menu
                    {
                        rightclickrow = hti.RowIndex;
                        rightclicksystem = (ISystem)dataGridViewNearest.Rows[hti.RowIndex].Tag;
                    }
                }
            }

            viewOnEDSMToolStripMenuItem1.Enabled = rightclicksystem != null;
        }

        private void addToTrilaterationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewNearest.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            this.Cursor = Cursors.WaitCursor;
            string sysName = "";
            foreach (DataGridViewRow r in selectedRows)
            {
                sysName = r.Cells[0].Value.ToString();
                discoveryform.NewTriLatStars(new List<string>() { sysName }, false);
            }

            this.Cursor = Cursors.Default;
        }

        private void viewOnEDSMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EDSMClass edsm = new EDSMClass();
            long? id_edsm = rightclicksystem.EDSMID;

            if (id_edsm == 0)
            {
                id_edsm = null;
            }

            if (!edsm.ShowSystemInEDSM(rightclicksystem.Name, id_edsm))
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".Tx(this,"NoEDSMSys"));
            }

            this.Cursor = Cursors.Default;
        }

        private void dataGridViewNearest_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 1)
                e.SortDataGridViewColumnNumeric();
        }

        private void checkBoxCube_CheckedChanged(object sender, EventArgs e)
        {
            if (this.IsHandleCreated)
                KickComputation(last_he, true);
        }

        private void textMinRadius_ValueChanged(object sender, EventArgs e)
        {
            if (this.IsHandleCreated)
                KickComputation(last_he, true);
        }

        private void textMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            if (this.IsHandleCreated)
                KickComputation(last_he, true);
        }
    }
}
