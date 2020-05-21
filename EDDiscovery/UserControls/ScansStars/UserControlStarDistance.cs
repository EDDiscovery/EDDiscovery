/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        private string DbSave { get { return DBName("StarDistancePanel"); } }

        private StarDistanceComputer computer = null;
        private HistoryEntry last_he = null;

        private const double defaultMaxRadius = 100;
        private const double defaultMinRadius = 0;
        private const int maxitems = 500;

        private double lookup_limit = 100;      // we start with a reasonable number, because if your in the bubble, you don't want to be looking up 1000

        public override void Init()
        {
            computer = new StarDistanceComputer();

            textMinRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "Min", defaultMinRadius);
            textMaxRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "Max", defaultMaxRadius);
            textMinRadius.SetComparitor(textMaxRadius, -2);     // need to do this after values are set
            textMaxRadius.SetComparitor(textMinRadius, 2);

            checkBoxCube.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "Behaviour", false);

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
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "Min", textMinRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "Max", textMaxRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "Behaviour", checkBoxCube.Checked);
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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

                double lookup_max = Math.Min(textMaxRadius.Value, lookup_limit);
                //System.Diagnostics.Debug.WriteLine("Lookup limit " + lookup_limit + " lookup " + lookup_max);

                // Get nearby systems from the systems DB.
                computer.CalculateClosestSystems(he.System,
                    NewStarListComputedAsync,
                    maxitems, textMinRadius.Value, lookup_max, !checkBoxCube.Checked
                    );     // hook here, force closes system update


            }
        }

        private void NewStarListComputedAsync(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)
        {
            //System.Diagnostics.Debug.WriteLine("Computer returned " + list.Count);
            this.ParentForm.BeginInvoke(new Action(() => NewStarListComputed(sys, list)));
        }

        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!

            double lookup_max = Math.Min(textMaxRadius.Value, lookup_limit);

            // Get nearby systems from our travel history. This will filter out duplicates from the systems DB.
            discoveryform.history.CalculateSqDistances(list, sys.X, sys.Y, sys.Z,
                                maxitems, textMinRadius.Value, lookup_max, !checkBoxCube.Checked
                                );

            FillGrid(sys.Name, list);
        }

        private void FillGrid(string name, BaseUtils.SortedListDoubleDuplicate<ISystem> csl)
        {
            dataGridViewNearest.Rows.Clear();

            if (csl != null && csl.Any())
            {
                SetControlText(string.Format("From {0}".T(EDTx.UserControlStarDistance_From), name));

                int maxdist = 0;
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    double dist = Math.Sqrt(tvp.Key);   // distances are stored squared for speed, back to normal.

                    maxdist = Math.Max(maxdist, (int)dist);

                    if (tvp.Value.Name != name && (checkBoxCube.Checked || (dist >= textMinRadius.Value && dist <= textMaxRadius.Value)))
                    {
                        int visits = discoveryform.history.GetVisitsCount(tvp.Value.Name);
                        object[] rowobj = { tvp.Value.Name, $"{dist:0.00}", $"{visits:n0}" };

                        int rowindex = dataGridViewNearest.Rows.Add(rowobj);
                        dataGridViewNearest.Rows[rowindex].Tag = tvp.Value;
                    }
                }

                if (csl.Count > maxitems / 2)             // if we filled up at least half the list, we limit to max distance plus
                {
                    lookup_limit = maxdist * 11 / 10;   // lookup limit is % more than max dist, to allow for growth
                }
                else
                    lookup_limit = maxdist * 2;         // else we did not get close to filling the list, so double the limit and try again

                System.Diagnostics.Debug.WriteLine("Star distance Lookup " + name + " found " + csl.Count + " max was " + maxdist + " New limit " + lookup_limit);
            }
            else
            {
                SetControlText(string.Empty);
            }
        }

        private int rightclickrow = -1;

        private void dataGridViewNearest_MouseDown(object sender, MouseEventArgs e)
        {
            dataGridViewNearest.HandleClickOnDataGrid(e, out int unusedleftclickrow, out rightclickrow);
            viewOnEDSMToolStripMenuItem1.Enabled = rightclickrow != -1;
        }

        private void addToTrilaterationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AddTo(OnNewStarsPushType.TriSystems);
        }

        private void addToExplorationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTo(OnNewStarsPushType.Exploration);
        }

        private void addToExpeditionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTo(OnNewStarsPushType.Expedition);

        }

        private void AddTo(OnNewStarsPushType pushtype)
        {
            IEnumerable<DataGridViewRow> selectedRows = dataGridViewNearest.SelectedCells.Cast<DataGridViewCell>()
                                                                        .Select(cell => cell.OwningRow)
                                                                        .Distinct()
                                                                        .OrderBy(cell => cell.Index);

            List<string> syslist = new List<string>();
            foreach (DataGridViewRow r in selectedRows)
                syslist.Add(r.Cells[0].Value.ToString());

            if (uctg is IHistoryCursorNewStarList)
                (uctg as IHistoryCursorNewStarList).FireNewStarList(syslist, pushtype);
        }

        private void viewOnEDSMToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (rightclickrow >= 0 && rightclickrow < dataGridViewNearest.Rows.Count)
            {
                var rightclicksystem = (ISystem)dataGridViewNearest.Rows[rightclickrow].Tag;

                if (rightclicksystem != null)
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
                        ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable".T(EDTx.UserControlStarDistance_NoEDSMSys));
                    }

                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void dataGridViewNearest_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 1)
                e.SortDataGridViewColumnNumeric();
        }

        private void checkBoxCube_CheckedChanged(object sender, EventArgs e)
        {
            KickComputation(last_he, true);
        }

        private void textMinRadius_ValueChanged(object sender, EventArgs e)
        {
            lookup_limit = textMaxRadius.Value;
            KickComputation(last_he, true);
        }

        private void textMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            lookup_limit = textMaxRadius.Value;
            KickComputation(last_he, true);
        }

        private void dataGridViewNearest_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.RowIndex >= 0 && e.RowIndex < dataGridViewNearest.Rows.Count)
            {
                Clipboard.SetText(dataGridViewNearest.Rows[e.RowIndex].Cells[0].Value as string);
            }
        }

        private void viewSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
                if (rightclickrow >= 0 && rightclickrow < dataGridViewNearest.Rows.Count)
                {
                    var rightclicksystem = (ISystem)dataGridViewNearest.Rows[rightclickrow].Tag;
                if ( rightclicksystem != null )
                    ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), rightclicksystem, true, discoveryform.history);
            }
        }

        private void dataGridViewNearest_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dataGridViewNearest.Rows.Count)
            { 
                var clicksystem = (ISystem)dataGridViewNearest.Rows[e.RowIndex].Tag;
                if ( clicksystem != null )
                    ScanDisplayForm.ShowScanOrMarketForm(this.FindForm(), clicksystem, true, discoveryform.history);
            }

        }
    }
}
