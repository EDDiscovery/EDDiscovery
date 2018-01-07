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
    public partial class UserControlRadar : UserControlCommonBase
    {
        private string DbSave { get { return "StarDistancePanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private StarDistanceComputer computer;

        public UserControlRadar()
        {
            InitializeComponent();         
        }

        const double defaultmaximumradarradius = 75;
        const int maxitems = 500;

        public override void Init()
        {
            computer = new StarDistanceComputer();

            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            textMinRadius.Text = SQLiteConnectionUser.GetSettingDouble(DbSave + "RadarMin", 0).ToStringInvariant();
            textMaxRadius.Text = SQLiteConnectionUser.GetSettingDouble(DbSave + "RadarMax", defaultmaximumradarradius).ToStringInvariant();            
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {            
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            refreshRadar();
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            SQLiteConnectionUser.PutSettingDouble(DbSave + "RadarMin", textMinRadius.Text.InvariantParseDouble(0));
            SQLiteConnectionUser.PutSettingDouble(DbSave + "RadarMax", textMaxRadius.Text.InvariantParseDouble(defaultmaximumradarradius));
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);            

        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl)
        {
            KickComputation(he);

            // this is our current system, centered on the map
            refreshRadar();

            chartXY.Series[0].Points.AddXY(0, 0, 0);
            chartXY.Series[0].ToolTip = he.System.name;
            chartXZ.Series[0].Points.AddXY(0, 0, 0);
            chartXZ.Series[0].ToolTip = he.System.name;
        }
        
        private void KickComputation(HistoryEntry he)
        {
            if (he != null)
            {
                double curX = he.System.x;
                double curY = he.System.y;
                double curZ = he.System.z;

                computer.CalculateClosestSystems(he.System, 
                    (s, d) => BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d, curX, curY, curZ); }) , 
                    maxitems,
                    Math.Max(textMinRadius.Text.InvariantParseDouble(0), 8.0/128.0),     // min to exclude our star
                    textMaxRadius.Text.InvariantParseDouble(defaultmaximumradarradius));     // hook here, force closes system update
            }
        }

        private void NewStarListComputed(ISystem sys, SortedList<double, ISystem> list, double curX, double curY, double curZ)      // In UI
        {            
            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!

            discoveryform.history.CalculateSqDistances(list, sys.x, sys.y, sys.z,
                                maxitems,
                                Math.Max(textMinRadius.Text.InvariantParseDouble(0), 8.0/128.0),     // min to exclude our star
                                textMaxRadius.Text.InvariantParseDouble(defaultmaximumradarradius)
                                );

            FillRadar(sys.name, list, sys.x, sys.y, sys.z, curX, curY, curZ);
        }

        private void FillRadar(string name, SortedList<double, ISystem> csl, double x, double y, double z, double curX, double curY, double curZ)
        {
            SetControlText("");

            if (csl.Count() > 0)
            {                
                SetControlText("Plot of closest systems from " + name);
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    int visits = discoveryform.history.GetVisitsCount(tvp.Value.name, tvp.Value.id_edsm);
                    object[] rowobj = { tvp.Value.name, Math.Sqrt(tvp.Key).ToString("0.00"), visits.ToStringInvariant() };       // distances are stored squared for speed, back to normal.

                    //int rowindex = rowobj;
                    //dataGridViewNearest.Rows[rowindex].Tag = tvp.Value;

                    // mockup with random coordinates

                    int range = int.Parse(SQLiteConnectionUser.GetSettingDouble(DbSave + "RadarMax", defaultmaximumradarradius).ToStringInvariant());

                    // distances from center
                    Random rnd = new Random();
                    int sysX2 = rnd.Next((range * -1), range);
                    int sysY2 = rnd.Next((range * -1), range);
                    int sysZ2 = rnd.Next((range * -1), range);
                    string label = tvp.Value.name;

                    /*
                    double sysx = curX - sysX;
                    double sysy = curY - sysY;
                    double sysz = curZ - sysZ;

                    string label = sysX.ToString() + sysX.ToString() + sysX.ToString();
                    string test = curX.ToString() + curX.ToString() + curX.ToString();
                    */
                                        
                    int px = (int)sysX2;
                    int py = (int)sysY2;
                    int pz = (int)sysZ2;

                    if (visits > 0) // visited system are blue
                    {
                        chartXY.Series[1].Points.AddXY(px, py, pz);
                        chartXY.Series[1].ToolTip = label;
                        chartXZ.Series[1].Points.AddXY(px, pz, py);
                        chartXZ.Series[1].ToolTip = label;
                    }
                    else if (visits == 0) // non visited sytems are yellow
                    {
                        chartXY.Series[2].Points.AddXY(px, py, pz);
                        chartXY.Series[2].ToolTip = label;
                        chartXZ.Series[2].Points.AddXY(px, pz, py);
                        chartXZ.Series[2].ToolTip = label;                        
                    }
                    //                    
                }                
            }            
        }        
                
        private void textMinRadius_TextChanged(object sender, EventArgs e)
        {
            double? min = textMinRadius.Text.InvariantParseDoubleNull();
            if (min != null)
                KickComputation(uctg.GetCurrentHistoryEntry);

            refreshRadar();
        }

        private void textMaxRadius_TextChanged(object sender, EventArgs e)
        {
            double? max = textMaxRadius.Text.InvariantParseDoubleNull();
            if (max != null)
                KickComputation(uctg.GetCurrentHistoryEntry);

            refreshRadar();
        }
                
        private void refreshRadar()
        {
            chartXY.Series[1].Points.Clear();
            chartXY.Series[2].Points.Clear();

            chartXY.Update();
            chartXY.Legends.Clear();

            chartXZ.Series[1].Points.Clear();
            chartXZ.Series[2].Points.Clear();

            chartXZ.Update();
            chartXZ.Legends.Clear();

            int range = int.Parse(SQLiteConnectionUser.GetSettingDouble(DbSave + "RadarMax", defaultmaximumradarradius).ToStringInvariant());

            chartXY.ChartAreas[0].AxisX.Maximum = range;
            chartXY.ChartAreas[0].AxisX.Minimum = range * -1;
            chartXY.ChartAreas[0].AxisY.Maximum = range;
            chartXY.ChartAreas[0].AxisY.Minimum = range * -1;

            chartXZ.ChartAreas[0].AxisX.Maximum = range;
            chartXZ.ChartAreas[0].AxisX.Minimum = range * -1;
            chartXZ.ChartAreas[0].AxisY.Maximum = range;
            chartXZ.ChartAreas[0].AxisY.Minimum = range * -1;
        }

        /// <summary>
        /// Computer
        /// </summary>

        class StarDistanceComputer
        {
            private Thread backgroundStardistWorker;
            private bool PendingClose { get; set; }           // we want to close boys!

            private class StardistRequest
            {
                public ISystem System;
                public bool IgnoreOnDuplicate;      // don't compute until last one is present
                public double MinDistance;
                public double MaxDistance;
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
                            int maxitems, double mindistance, double maxdistance, bool ignoreDuplicates = true)
            {
                closestsystem_queue.Enqueue(new StardistRequest { System = sys, Callback = callback,
                                MaxItems = maxitems, MinDistance = mindistance, MaxDistance = maxdistance,  IgnoreOnDuplicate = ignoreDuplicates });
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
                                                    req.MinDistance, req.MaxDistance);

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

        private void checkBoxSwitchCharts_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSwitchCharts.Checked == true)
            {
                chartXY.Visible = true;
                chartXZ.Visible = false;
            }
            else if (checkBoxSwitchCharts.Checked == false)
            {
                chartXY.Visible = false;
                chartXZ.Visible = true;
            }
        }

        private void checkBoxSwitchCharts_CheckStateChanged(object sender, EventArgs e)
        {
            if (checkBoxSwitchCharts.Checked == true)
            {
                chartXY.Visible = true;
                chartXZ.Visible = false;
            }
            else if (checkBoxSwitchCharts.Checked == false)
            {
                chartXY.Visible = false;
                chartXZ.Visible = true;
            }
        }
    }
}
