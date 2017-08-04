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
        EDDiscoveryForm _discoveryForm;
        UserControlTravelGrid uctg;
        StarDistanceComputer computer;

        public UserControlStarDistance()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, UserControlTravelGrid thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            _discoveryForm = ed;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            computer = new StarDistanceComputer();

            HistoryEntry he = uctg.GetCurrentHistoryEntry;      // does our UCTG have a system selected?

            if (he != null)
            {
                //System.Diagnostics.Debug.WriteLine("Star grid started, uctg selected, ask");
                computer.CalculateClosestSystems(he.System, (s, d) => BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }));     // hook here, force closes system update
            }
        }

        public override void ChangeTravelGrid(UserControlTravelGrid thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
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

            _discoveryForm.history.CalculateSqDistances(list, sys.x, sys.y, sys.z, 50, true);   // add on any history list systems

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
