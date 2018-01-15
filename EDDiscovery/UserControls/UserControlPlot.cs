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
            var x = this.Handle; // workaround to avoid throw an exception if open the panel as popup

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
                SetControlText("2D Plot of systems in range from " + centerSystem.name);                

                chartBubble.Series[0].Points.AddXY(0, 0, 4);
                chartBubble.Series[0].ToolTip = centerSystem.name;
                chartBubble.Series[3].Points.AddXY(0, 0, 4);
                chartBubble.Series[3].ToolTip = centerSystem.name;
                chartBubble.Series[6].Points.AddXY(0, 0, 4);
                chartBubble.Series[6].ToolTip = centerSystem.name;



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
                        chartBubble.ChartAreas[0].AxisY.IsStartedFromZero = false;
                        chartBubble.ChartAreas[1].AxisY.IsStartedFromZero = false;
                        chartBubble.ChartAreas[2].AxisY.IsStartedFromZero = false;

                        chartBubble.ChartAreas[0].AxisX.IsStartedFromZero = false;
                        chartBubble.ChartAreas[1].AxisX.IsStartedFromZero = false;
                        chartBubble.ChartAreas[2].AxisX.IsStartedFromZero = false;


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

                            // visited systems go to series #1, #4 and #7; unvisited to series #2, #5 and #8. 
                            // Serie #0, #3 and #6 is for the current system...
                                                        
                            if (visits > 0)
                            {
                                chartBubble.Series[1].Points.AddXY(px, py, pz);
                                chartBubble.Series[1].ToolTip = label.ToString();

                                chartBubble.Series[4].Points.AddXY(px, pz, py);
                                chartBubble.Series[4].ToolTip = label.ToString();
                                
                                chartBubble.Series[7].Points.AddXY(py, pz, px);
                                chartBubble.Series[7].ToolTip = label.ToString();
                            }
                            else
                            {
                                chartBubble.Series[2].Points.AddXY(px, py, pz);
                                chartBubble.Series[2].ToolTip = label.ToString();

                                chartBubble.Series[5].Points.AddXY(px, pz, py);
                                chartBubble.Series[5].ToolTip = label.ToString();

                                chartBubble.Series[8].Points.AddXY(py, pz, px);
                                chartBubble.Series[8].ToolTip = label.ToString();
                            }
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
            for (int i = 0; i < 9; i++)
            {
                chartBubble.Series[i].Points.Clear();
            }            
            chartBubble.Update();            
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
            chartBubble.ChartAreas[0].Visible = false;
            chartBubble.ChartAreas[1].Visible = true;
            chartBubble.ChartAreas[2].Visible = false;
        }

        private void buttonExt2dfront_MouseDown(object sender, MouseEventArgs e)
        {
            chartBubble.ChartAreas[0].Visible = true;
            chartBubble.ChartAreas[1].Visible = false;
            chartBubble.ChartAreas[2].Visible = false;
        }

        private void buttonExt2dside_MouseDown(object sender, MouseEventArgs e)
        {
            chartBubble.ChartAreas[0].Visible = false;
            chartBubble.ChartAreas[1].Visible = false;
            chartBubble.ChartAreas[2].Visible = true;
        }
    }    
}
