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
using System.Diagnostics;
using System.Windows.Automation.Peers;

namespace EDDiscovery.UserControls
{
    public partial class UserControlLocalMap : UserControlCommonBase
    {
        #region init

        private string DbSave { get { return DBName("MapPanel" ); } }

        private HistoryEntry last_he = null;
        private const double defaultMapMaxRadius = 1000;
        private const double defaultMapMinRadius = 0;
        private int maxitems = 500;
        private System.Windows.Forms.Timer slidetimer;

        private StarDistanceComputer computer;

        public UserControlLocalMap()
        {
            InitializeComponent();

            extAstroPlot.AxesWidget = true;
            extAstroPlot.FramesWidget = false;
        }

        public override void Init()
        {
            computer = new StarDistanceComputer();

            slideMaxItems.Value = maxitems = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSave + "MapMaxItems", maxitems);

            textMaxRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MapMax", defaultMapMaxRadius);
            textMinRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MapMin", defaultMapMinRadius);
            textMinRadius.SetComparitor(textMaxRadius, -2);     // need to do this after values are set
            textMaxRadius.SetComparitor(textMinRadius, 2);

            extAstroPlot.AxesLength = (int)(textMaxRadius.Value * 0.25);
            extAstroPlot.SetAxesCoordinates(extAstroPlot.AxesLength);

            extAstroPlot.FramesRadius = (int)textMaxRadius.Value;
            extAstroPlot.SetFrameCoordinates(extAstroPlot.FramesRadius);

            slidetimer = new System.Windows.Forms.Timer
            {
                Interval = 500
            };
            slidetimer.Tick += Slidetimer_Tick;
                        
            extAstroPlot.MouseSensitivity_Wheel = 20;
            extAstroPlot.MouseSensitivity_Movement = 200;

            extAstroPlot.VisitedColor = Color.Aqua;
            extAstroPlot.UnVisitedColor = Color.Yellow;
            extAstroPlot.CurrentColor = Color.Red;

            extAstroPlot.SmallDotSize = 6;

            BaseUtils.Translator.Instance.Translate(this);
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
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MapMin", textMinRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MapMax", textMaxRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSave + "MapMaxItems", maxitems);
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
            extAstroPlot.AxesWidget = true;
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            KickComputation(he);
            extAstroPlot.Clear();
            RefreshMap();
        }

        #endregion

        #region computer

        private void KickComputation(HistoryEntry he)
        {
            if (he?.System != null && he.System.HasCoordinate)
            {
                computer.CalculateClosestSystems(he.System,
                    (s, d) => this.ParentForm.BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
                    maxitems, textMinRadius.Value, textMaxRadius.Value, true);
            }
        }

        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
        {
#if DEBUG
System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!
#endif
            discoveryform.history.CalculateSqDistances(list, sys.X, sys.Y, sys.Z, maxitems, textMinRadius.Value, textMaxRadius.Value, true);
            FillMap(list, sys);                        
        }

        private class NearbySystem
        {
            public string Name { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public bool IsVisited { get; set; } = false;
            public bool IsWaypoint { get; set; } = false;
            public bool IsCurrent { get; set; } = false;
            public bool IsKnown { get; set; } = false;
        }

        private readonly List<NearbySystem> nearbySystems = new List<NearbySystem>();

        private void SetCenterSystem(double[] coords)
        {
            extAstroPlot.CoordsCenter = coords;
            extAstroPlot.Clear();
        }

        private void PlotObjects(List<NearbySystem> nearbySystemsList)
        {
            extAstroPlot.Clear();

            var List = new List<object[]>();

            for (int i = 0; i < nearbySystemsList.Count; i++)
            {
                List.Add(new object[]
                {
                    nearbySystemsList[i].Name,
                    nearbySystemsList[i].X,
                    nearbySystemsList[i].Y,
                    nearbySystemsList[i].Z,
                    nearbySystemsList[i].IsVisited,
                    nearbySystemsList[i].IsWaypoint,
                    nearbySystemsList[i].IsCurrent
                });
            }
            extAstroPlot.AddSystemsToMap(List);
        }

        private void FillMap(SortedList<double, ISystem> csl, ISystem centerSystem)
        {
            if (csl.Count > 0)
            {
                foreach (KeyValuePair<double, ISystem> tvp in csl)
                {
                    var isCurrentCheck = false;
                    var isVisitedCheck = false;

                    if (tvp.Value.Name == centerSystem.Name)
                    {
                        isCurrentCheck = true;
                    }

                    var visits = discoveryform.history.GetVisitsCount(tvp.Value.Name);

                    if (visits > 0)
                    {
                        isVisitedCheck = true;
                    }

                    nearbySystems.Add(new NearbySystem
                    {
                        Name = tvp.Value.Name,
                        X = tvp.Value.X,
                        Y = tvp.Value.Y,
                        Z = tvp.Value.Z,
                        IsVisited = isVisitedCheck,
                        IsCurrent = isCurrentCheck
                    });
                }

                extAstroPlot.Distance = (int)textMaxRadius.Value;

                SetCenterSystem(new double[] { centerSystem.X, centerSystem.Y, centerSystem.Z });

                PlotObjects(nearbySystems);
            }
        }

        #endregion

        #region refresh

        private void TextMinRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(last_he ?? uctg.GetCurrentHistoryEntry);
        }

        private void TextMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            extAstroPlot.AxesLength = (int)(textMaxRadius.Value * 0.25);
            extAstroPlot.SetAxesCoordinates(extAstroPlot.AxesLength);

            extAstroPlot.FramesRadius = (int)textMaxRadius.Value;
            extAstroPlot.SetFrameCoordinates(extAstroPlot.FramesRadius);

            KickComputation(last_he ?? uctg.GetCurrentHistoryEntry);
        }

        private void RefreshMap()
        {            
            nearbySystems.Clear();
            extAstroPlot.Clear();
        }

        #endregion

        private void SlideMaxItems_Scroll(object sender, EventArgs e)
        {
            slidetimer.Start();
        }
        private void Slidetimer_Tick(object sender, EventArgs e)            // DONT use a Wait loop - this kills the rest of the system..
        {
            slidetimer.Stop();
            toolTip.SetToolTip(slideMaxItems, slideMaxItems.Value.ToString());
            maxitems = slideMaxItems.Value;
            KickComputation(uctg.GetCurrentHistoryEntry);
            RefreshMap();
        }

        private void SlideMaxItems_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(slideMaxItems, slideMaxItems.Value.ToString() + " max number of systems.");
        }

        // About Map
        private void AboutToolStripAbout_Click(object sender, EventArgs e)
        {
            using (var frm = new ExtendedControls.InfoForm())
            {
                frm.Info("Map Help", FindForm().Icon, EDDiscovery.Properties.Resources.mapuc, new int[1] { 300 });
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.Show(FindForm());
            }
        }
    }
}

