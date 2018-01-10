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
    public partial class UserControlPlot : UserControlCommonBase
    {
        private string DbSave { get { return "StarDistancePanel" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }

        private StarDistanceComputer computer;

        public UserControlPlot()
        {
            InitializeComponent();
        }

        const double defaultmaximumradarradius = 50;
        const int maxitems = 500;

        double MaxRadius = 0;
        double MinRadius = 0;

        public override void Init()
        {
            computer = new StarDistanceComputer();

            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;

            textMinRadius.Text = SQLiteConnectionUser.GetSettingDouble(DbSave + "PlotMin", 0).ToStringInvariant();
            textMaxRadius.Text = SQLiteConnectionUser.GetSettingDouble(DbSave + "PlotMax", defaultmaximumradarradius).ToStringInvariant();
            MaxRadius = float.Parse(textMaxRadius.Text);
            MinRadius = float.Parse(textMinRadius.Text);
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
            SQLiteConnectionUser.PutSettingDouble(DbSave + "PlotMin", textMinRadius.Text.InvariantParseDouble(0));
            SQLiteConnectionUser.PutSettingDouble(DbSave + "PlotMax", textMaxRadius.Text.InvariantParseDouble(defaultmaximumradarradius));
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
        }

        private void KickComputation(HistoryEntry he)
        {
            if (he?.System != null && he.System.HasCoordinate)
            {
                computer.CalculateClosestSystems(he.System,
                    (s, d) => BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
                    maxitems, MinRadius, MaxRadius);
            }
        }
        private void NewStarListComputed(ISystem sys, SortedList<double, ISystem> list)      // In UI
        {
            double? max = textMaxRadius.Text.InvariantParseDoubleNull();
            if (max != null)
                MaxRadius = float.Parse(textMaxRadius.Text);

            double? min = textMinRadius.Text.InvariantParseDoubleNull();
            if (min != null)
                MinRadius = float.Parse(textMinRadius.Text);

            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!
            discoveryform.history.CalculateSqDistances(list, sys.x, sys.y, sys.z, maxitems, MinRadius, MaxRadius);
            FillRadar(list, sys);
        }
        
        private void FillRadar(SortedList<double, ISystem> csl, ISystem centerSystem)
        {
            SetControlText("");

            if (csl.Count() > 0)
            {
                SetControlText("Plot of closest systems from " + centerSystem.name);

                chartXY.Series[0].Points.AddXY(0, 0, 4);
                chartXY.Series[0].ToolTip = centerSystem.name;
                chartXZ.Series[0].Points.AddXY(0, 0, 4);
                chartXZ.Series[0].ToolTip = centerSystem.name;
                chartYZ.Series[0].Points.AddXY(0, 0, 4);
                chartYZ.Series[0].ToolTip = centerSystem.name;

                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    if (tvp.Value.name != centerSystem.name)
                    { 

                    var theISystemInQuestion = tvp.Value;
                    var sysX = theISystemInQuestion.x;
                    var sysY = theISystemInQuestion.y;
                    var sysZ = theISystemInQuestion.z;
                    var distFromCurrentSys = Math.Round(Math.Sqrt(tvp.Key), 2, MidpointRounding.AwayFromZero);
                    var curX = centerSystem.x;
                    var curY = centerSystem.y;
                    var curZ = centerSystem.z;

                    // reset charts axis
                        chartXY.ChartAreas[0].AxisY.IsStartedFromZero = false;
                        chartXZ.ChartAreas[0].AxisY.IsStartedFromZero = false;
                        chartYZ.ChartAreas[0].AxisY.IsStartedFromZero = false;

                        chartXY.ChartAreas[0].AxisX.IsStartedFromZero = false;
                        chartXZ.ChartAreas[0].AxisX.IsStartedFromZero = false;
                        chartYZ.ChartAreas[0].AxisX.IsStartedFromZero = false;


                        if (distFromCurrentSys > MinRadius)
                        {
                            int visits = discoveryform.history.GetVisitsCount(tvp.Value.name, tvp.Value.id_edsm);

                            StringBuilder label = new StringBuilder();
                            label.Append(theISystemInQuestion.name + " / " + visits + " visits" + "\n" + distFromCurrentSys);

                            double dx = curX - sysX;
                            double dy = curY - sysY;
                            double dz = curZ - sysZ;

                            int px = Convert.ToInt32(dx);
                            int py = Convert.ToInt32(dy);
                            int pz = Convert.ToInt32(dz);

                            // visited systems go to the serie #1, unvisited to the serie #2. Serie #0 is for the current system...
                            int pv = 2;
                            if (visits > 0)
                            {
                                pv = 1;
                            }
                            else
                            {
                                pv = 2;
                            }

                            chartXY.Series[pv].Points.AddXY(px, py, pz);
                            chartXY.Series[pv].ToolTip = label.ToString();

                            chartXZ.Series[pv].Points.AddXY(px, pz, py);
                            chartXZ.Series[pv].ToolTip = label.ToString();

                            chartYZ.Series[pv].Points.AddXY(py, pz, px);
                            chartYZ.Series[pv].ToolTip = label.ToString();
                        }
                    }
                }
            }
        }

        private void textMinRadius_TextChanged(object sender, EventArgs e)
        {
            double? min = textMinRadius.Text.InvariantParseDoubleNull();
            if (min != null)
            MinRadius = float.Parse(textMinRadius.Text);

            KickComputation(uctg.GetCurrentHistoryEntry);

            refreshRadar();
        }

        private void textMaxRadius_TextChanged(object sender, EventArgs e)
        {
            double? max = textMaxRadius.Text.InvariantParseDoubleNull();
            if (max != null)
            MaxRadius = float.Parse(textMaxRadius.Text);

            KickComputation(uctg.GetCurrentHistoryEntry);

            refreshRadar();            
        }
                
        private void refreshRadar()
        {
            chartXY.Series[0].Points.Clear();
            chartXY.Series[1].Points.Clear();
            chartXY.Series[2].Points.Clear();

            chartXZ.Series[0].Points.Clear();
            chartXZ.Series[1].Points.Clear();
            chartXZ.Series[2].Points.Clear();

            chartYZ.Series[0].Points.Clear();
            chartYZ.Series[1].Points.Clear();
            chartYZ.Series[2].Points.Clear();

            chartXY.Update();
            chartXZ.Update();
            chartYZ.Update();
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
        
        private void buttonExt2dtop_MouseDown(object sender, MouseEventArgs e)
        {
            chartXY.Visible = true;
            chartXZ.Visible = false;
            chartYZ.Visible = false;
        }

        private void buttonExt2dfront_MouseDown(object sender, MouseEventArgs e)
        {
            chartXY.Visible = false;
            chartXZ.Visible = true;
            chartYZ.Visible = false;
        }

        private void buttonExt2dside_MouseDown(object sender, MouseEventArgs e)
        {
            chartXY.Visible = false;
            chartXZ.Visible = false;
            chartYZ.Visible = true;
        }
    }    
}
