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
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlStarDistance : UserControlCommonBase
    {
        private StarDistanceComputer computer;

        public UserControlStarDistance()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            computer = new StarDistanceComputer();

            HistoryEntry he = uctg.GetCurrentHistoryEntry;      // does our UCTG have a system selected?

            if (he != null)
            {
                //System.Diagnostics.Debug.WriteLine("Star grid started, uctg selected, ask");
                computer.CalculateClosestSystems(he.System, (s, d) => BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }));     // hook here, force closes system update
            }
        }

        public override void ChangeCursorType(UserControlCursorType thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
        }

        public override void InitialDisplay()
        {
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl)
        {
            if (he != null)
            {
                //System.Diagnostics.Debug.WriteLine("Star grid sel changed ask");
                computer.CalculateClosestSystems(he.System, (s, d) => BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }));     // hook here, force closes system update
            }
        }

        private void NewStarListComputed(ISystem sys, SortedList<double, ISystem> list)      // In UI
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!

            discoveryform.history.CalculateSqDistances(list, sys.x, sys.y, sys.z, 50, true);   // add on any history list systems

            FillGrid(sys.name, list);
        }

        private void FillGrid(string name, SortedList<double, ISystem> csl)
        {
            SetControlText("");
            dataGridViewNearest.Rows.Clear();

            if (csl.Count() > 0)
            {
                SetControlText("Closest systems from " + name);
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    int visits = discoveryform.history.GetVisitsCount(tvp.Value.name, tvp.Value.id_edsm);
                    object[] rowobj = { tvp.Value.name, Math.Sqrt(tvp.Key).ToString("0.00"), visits.ToStringInvariant()};       // distances are stored squared for speed, back to normal.
                    int rowindex = dataGridViewNearest.Rows.Add(rowobj);
                    dataGridViewNearest.Rows[rowindex].Tag = tvp.Value;
                }
            }
        }

        ISystem rightclicksystem = null;
        int rightclickrow = -1;

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
                ExtendedControls.MessageBoxTheme.Show("System could not be found - has not been synched or EDSM is unavailable");
            }

            this.Cursor = Cursors.Default;
        }

        private void dataGridViewNearest_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index >= 1)
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
        }

    }


    class StarDistanceComputer
    {
        private Thread backgroundStardistWorker;
        private bool PendingClose { get;  set; }           // we want to close boys!

        private class StardistRequest
        {
            public ISystem System;
            public bool IgnoreOnDuplicate;
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

        public void CalculateClosestSystems(ISystem sys, Action<ISystem, SortedList<double, ISystem>> callback, bool ignoreDuplicates = true)
        {
            closestsystem_queue.Enqueue(new StardistRequest { System = sys, Callback = callback, IgnoreOnDuplicate = ignoreDuplicates });
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
                                SystemClassDB.GetSystemSqDistancesFrom(closestsystemlist, sys.x, sys.y, sys.z, 50, true, 1000);
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
