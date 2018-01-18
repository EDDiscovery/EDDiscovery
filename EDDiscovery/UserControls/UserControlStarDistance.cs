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

        private string DbSave { get { return "StarDistancePanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        [DefaultValue(defaultMaxRadius)]
        private double MaxRadius
        {
            get { return _MaxRadius; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value) || value <= 0)
                    value = defaultMaxRadius;
                if (_MaxRadius != value)
                {
                    _MaxRadius = value;
                    if (uctg != null)
                        KickComputation(uctg.GetCurrentHistoryEntry);
                }
                // Don't adjust text in a focused textbox while a user is typing.
                if (textMaxRadius.ContainsFocus)
                    pendingText = $"{value:0.00}";
                else
                    textMaxRadius.Text = $"{value:0.00}";
            }
        }
        [DefaultValue(defaultMinRadius)]
        private double MinRadius
        {
            get { return _MinRadius; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value) || value < 0)
                    value = defaultMinRadius;
                if (_MinRadius != value)
                {
                    _MinRadius = value;
                    if (uctg != null)
                        KickComputation(uctg.GetCurrentHistoryEntry);
                }
                // Don't adjust text in a focused textbox while a user is typing.
                if (textMinRadius.ContainsFocus)
                    pendingText = $"{value:0.00}";
                else
                    textMinRadius.Text = $"{value:0.00}";
            }
        }

        private StarDistanceComputer computer = null;
        private HistoryEntry last_he = null;
        private ISystem rightclicksystem = null;
        private int rightclickrow = -1;
        private string pendingText = null;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _MaxRadius = defaultMaxRadius;
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private double _MinRadius = defaultMinRadius;

        private const double defaultMaxRadius = 1000;
        private const double defaultMinRadius = 0;
        private const int maxitems = 500;

        public override void Init()
        {
            computer = new StarDistanceComputer();

            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            MaxRadius = SQLiteConnectionUser.GetSettingDouble(DbSave + "Max", MaxRadius);
            MinRadius = SQLiteConnectionUser.GetSettingDouble(DbSave + "Min", MinRadius);
            checkBoxCube.Checked = SQLiteConnectionUser.GetSettingBool(DbSave + "Behaviour", false);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            SQLiteConnectionUser.PutSettingDouble(DbSave + "Min", MinRadius);
            SQLiteConnectionUser.PutSettingDouble(DbSave + "Max", MaxRadius);
            SQLiteConnectionUser.PutSettingBool(DbSave + "Behaviour", checkBoxCube.Checked);
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl)
        {
            KickComputation(he);
        }

        private void KickComputation(HistoryEntry he)
        {
            if (he != null)
            {
                last_he = he;

                //System.Diagnostics.Debug.WriteLine("Star grid started, uctg selected, ask");
                computer.CalculateClosestSystems(he.System, 
                    (s, d) => BeginInvoke((MethodInvoker) delegate { FillGrid(s, d); }),
                    maxitems, MinRadius, MaxRadius, !checkBoxCube.Checked
                    );     // hook here, force closes system update
            }
        }

        private void FillGrid(ISystem sys, SortedList<double, ISystem> csl)
        {
            Debug.Assert(Application.MessageLoop);       // check!

            SetControlText("");
            dataGridViewNearest.Rows.Clear();

            if (csl.Count() > 0)
            {
                SetControlText("From " + sys.name);
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    double dist = Math.Sqrt(tvp.Key);   // distances are stored squared for speed, back to normal.

                    if (tvp.Value.name != sys.name && (checkBoxCube.Checked || (dist >= MinRadius && dist <= MaxRadius)))
                    {
                        int visits = discoveryform.history.GetVisitsCount(tvp.Value.name, tvp.Value.id_edsm);
                        object[] rowobj = { tvp.Value.name, $"{dist:0.00}", $"{visits:n0}" };

                        int rowindex = dataGridViewNearest.Rows.Add(rowobj);
                        dataGridViewNearest.Rows[rowindex].Tag = tvp.Value;
                    }
                }
                
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
            long? id_edsm = rightclicksystem.id_edsm;

            if (id_edsm == 0)
            {
                id_edsm = null;
            }

            if (!edsm.ShowSystemInEDSM(rightclicksystem.name, id_edsm))
            {
                ExtendedControls.MessageBoxTheme.Show(FindForm(), "System could not be found - has not been synched or EDSM is unavailable");
            }

            this.Cursor = Cursors.Default;
        }

        private void dataGridViewNearest_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (colDistance.Equals(e.Column))
            {
                double v1, v2;
                string vs1 = e.CellValue1?.ToString();
                string vs2 = e.CellValue2?.ToString();
                if (vs1 != null && vs2 != null && Double.TryParse(vs1, out v1) && Double.TryParse(vs2, out v2))
                {
                    e.SortResult = v1.CompareTo(v2);
                    e.Handled = true;
                }
            }
            else if (colVisited.Equals(e.Column))
            {
                int v1, v2;
                string s1 = e.CellValue1?.ToString();
                string s2 = e.CellValue2?.ToString();
                if (!string.IsNullOrEmpty(s1) && !string.IsNullOrEmpty(s2) && int.TryParse(s1, out v1) && int.TryParse(s2, out v2))
                {
                    e.SortResult = v1.CompareTo(v2);
                    e.Handled = true;
                }
            }
        }


        private void textMinRadius_TextChanged(object sender, EventArgs e)
        {
            // Don't let others directly assigning to textMinRadius.Text result in parsing.
            if (textMinRadius.ContainsFocus)
            {
                double? min = textMinRadius.Text.InvariantParseDoubleNull();
                MinRadius = min ?? defaultMinRadius;
            }
        }

        private void textMinRadius_Leave(object sender, EventArgs e)
        {
            if (pendingText != null)
                textMinRadius.Text = pendingText;
            pendingText = null;
        }

        private void textMaxRadius_TextChanged(object sender, EventArgs e)
        {
            // Don't let others directly assigning to textMaxRadius.Text result in parsing.
            if (textMaxRadius.ContainsFocus)
            {
                double? max = textMaxRadius.Text.InvariantParseDoubleNull();
                MaxRadius = max ?? defaultMaxRadius;
            }
        }

        private void textMaxRadius_Leave(object sender, EventArgs e)
        {
            if (pendingText != null)
                textMaxRadius.Text = pendingText;
            pendingText = null;
        }

        private void checkBoxCube_CheckedChanged(object sender, EventArgs e)
        {
            KickComputation(last_he ?? uctg.GetCurrentHistoryEntry);
        }


        /// <summary>
        /// Computer
        /// </summary>
        public class StarDistanceComputer
        {
            private Thread backgroundStardistWorker;
            private bool PendingClose { get; set; }           // we want to close boys!

            private class StardistRequest
            {
                public ISystem System;
                public bool IgnoreOnDuplicate;      // don't compute until last one is present
                public double MinDistance;
                public double MaxDistance;
                public bool Spherical;
                public int MaxItems;
                public Action<ISystem, SortedList<double, ISystem>> Callback;
            }

            private ConcurrentQueue<StardistRequest> closestsystem_queue = new ConcurrentQueue<StardistRequest>();

            private AutoResetEvent stardistRequested = new AutoResetEvent(false);
            private AutoResetEvent closeRequested = new AutoResetEvent(false);

            public StarDistanceComputer()
            {
                PendingClose = false;
                backgroundStardistWorker = new Thread(BackgroundStardistWorkerThread) { Name = "Star Distance Worker", IsBackground = true };
                backgroundStardistWorker.Start();
            }

            public void CalculateClosestSystems(ISystem sys, Action<ISystem, SortedList<double, ISystem>> callback, 
                            int maxitems, double mindistance, double maxdistance, bool spherical, bool ignoreDuplicates = true)
            {
                closestsystem_queue.Enqueue(new StardistRequest { System = sys, Callback = callback,
                                MaxItems = maxitems, MinDistance = mindistance, MaxDistance = maxdistance,  Spherical = spherical , IgnoreOnDuplicate = ignoreDuplicates });
                stardistRequested.Set();
            }

            public void ShutDown()
            {
                PendingClose = true;
                closeRequested.Set();
                backgroundStardistWorker.Join();
            }

            private void BackgroundStardistWorkerThread()
            {
                while (!PendingClose)
                {
                    int wh = WaitHandle.WaitAny(new WaitHandle[] { closeRequested, stardistRequested });

                    if (PendingClose)
                        break;

                    StardistRequest stardistreq = null;

                    switch (wh)
                    {
                        case 0:  // Close Requested
                            break;
                        case 1:  // Star Distances Requested
                            while (!PendingClose && closestsystem_queue.TryDequeue(out stardistreq))
                            {
                                if (!stardistreq.IgnoreOnDuplicate || closestsystem_queue.Count == 0)
                                {
                                    StardistRequest req = stardistreq;
                                    ISystem sys = req.System;
                                    SortedList<double, ISystem> closestsystemlist = new SortedList<double, ISystem>(new DuplicateKeyComparer<double>()); //lovely list allowing duplicate keys - can only iterate in it.

                                    //System.Diagnostics.Debug.WriteLine("DB Computer Max distance " + req.MaxDistance);

                                    SystemClassDB.GetSystemSqDistancesFrom(closestsystemlist, sys.x, sys.y, sys.z, req.MaxItems , 
                                                    req.MinDistance, req.MaxDistance , req.Spherical);

                                    if (!PendingClose)
                                    {
                                        req.Callback(sys, closestsystemlist);
                                    }
                                }
                            }

                            break;
                    }
                }
            }

            private class DuplicateKeyComparer<TKey> : IComparer<TKey> where TKey : IComparable      // special compare for sortedlist
            {
                public int Compare(TKey x, TKey y)
                {
                    int result = x.CompareTo(y);
                    return (result == 0) ? 1 : result;      // for this, equals just means greater than, to allow duplicate distance values to be added.
                }
            }
        }
    }
}
