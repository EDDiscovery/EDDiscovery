﻿/*
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
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
using EDDiscovery2.DB;
using EDDiscovery.DB;
using EDDiscovery2.EDSM;

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
                MessageBox.Show("System could not be found - has not been synched or EDSM is unavailable");
            }

            this.Cursor = Cursors.Default;
        }
    }

    public class ComputeStarDistance       // only from Control for the invoke
    {
        public ComputeStarDistance()
        {
        }

        public void Init(IDiscoveryController form)
        {
            _discoveryForm = form;
        }


        public void StartComputeThread()
        {
            closestthread = new Thread(CalculateClosestSystems) { Name = "Closest Calc", IsBackground = true };
            closestthread.Start();
        }

        public void StopComputeThread()
        {
            if (closestthread != null && closestthread.IsAlive)
            {
                ShutdownEDD = true;
                closestsystem_queue.Add(null);
                closestthread.Join();
            }
        }

        public void Add(ISystem s)
        {
            closestsystem_queue.Add(s);
        }

        public delegate void OtherStarDistances(SortedList<double, ISystem> closestsystemlist, ISystem vsc);
        public event OtherStarDistances OnOtherStarDistances;
        
        public delegate void NewStarList(string name, SortedList<double, ISystem> csl);
        public event NewStarList OnNewStarList;

        IDiscoveryController _discoveryForm;
        bool ShutdownEDD = false;
        Thread closestthread = null;
        BlockingCollection<ISystem> closestsystem_queue = new BlockingCollection<ISystem>();
        SortedList<double, ISystem> closestsystemlist = new SortedList<double, ISystem>(new DuplicateKeyComparer<double>()); //lovely list allowing duplicate keys - can only iterate in it.

        private void CalculateClosestSystems()
        {
            ISystem vsc;

            while (true)
            {
                ISystem nextsys = null;
                ISystem cursys = null;
                cursys = closestsystem_queue.Take();           // block until got one..

                closestsystemlist.Clear();

                do
                {
                    vsc = cursys;

                    if (ShutdownEDD)
                        return;

                    while (closestsystem_queue.TryTake(out nextsys))    // try and empty the queue in case multiple ones are there
                    {
                        //Console.WriteLine("Chuck " + closestname);
                        vsc = nextsys;

                        if (ShutdownEDD)
                            return;
                    }

                    SystemClass.GetSystemSqDistancesFrom(closestsystemlist, vsc.x, vsc.y, vsc.z, 50, true, 1000);

                    if ( OnOtherStarDistances!=null)
                        OnOtherStarDistances(closestsystemlist, vsc);

                    cursys = vsc;
                } while (closestsystem_queue.TryTake(out vsc));     // if there is another one there, just re-run (slow down doggy!)

                if (OnNewStarList != null)
                    OnNewStarList(cursys.name, closestsystemlist);
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
