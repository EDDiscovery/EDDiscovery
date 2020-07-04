/*
 * Copyright © 2016 - 2020 EDDiscovery development team
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
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;
using System.Diagnostics;
using BaseUtils;
using OpenTK.Graphics.ES20;

namespace EDDiscovery.UserControls
{
    public partial class UserControlAstroPlot : UserControlCommonBase
    {
        private StarDistanceComputer computer;
        private HistoryEntry last_he = null;
        private List<object[]> systemsInRange = new List<object[]>();
        private HotSpotMap hotSpotMap = new HotSpotMap();

        private string howerName;
        private Point howerLocation;

        public UserControlAstroPlot()
        {
            InitializeComponent();
            hotSpotMap.OnHotSpot += HotSpotMap_OnHotSpot;
        }

        private void HotSpotMap_OnHotSpot()
        {
            Debug.WriteLine("ok");
            howerName = hotSpotMap.GetHotSpotName();
            howerLocation = hotSpotMap.GetHotSpotLocation();
        }

        public override void Init()
        {
            computer = new StarDistanceComputer();

            astroPlot.Distance = 50;
            astroPlot.Elevation = -0.4;
            astroPlot.Azimuth = -0.3;

            astroPlot.MouseWheel_Multiply = 1;
            astroPlot.MouseWheel_Resistance = 70;

            astroPlot.SmallDotSize = 10;

            astroPlot.HotSpotSize = 10;

            astroPlot.OnSystemSelected += AstroPlot_OnSystemSelected;

            Translator.Instance.Translate(this);
            Translator.Instance.Translate(toolTip, this);
        }

        private void AstroPlot_OnSystemSelected()
        {
            howerName = astroPlot.SelectedObjectName;
            howerLocation = astroPlot.SelectedObjectLocation;            
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
            base.Closing();
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            systemsInRange.Clear();
            astroPlot.Clear();
            KickComputation(he);
        }

        private void KickComputation(HistoryEntry he)
        {
            if (he?.System != null && he.System.HasCoordinate)
            {
                computer.CalculateClosestSystems(he.System,
                    (s, d) => this.ParentForm.BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
                    250, 0, 100, true);
            }
        }

        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
        {
            //Debug.Assert(Application.MessageLoop);       // check!
            discoveryform.history.CalculateSqDistances(list, sys.X, sys.Y, sys.Z, 250, 0, 100, true);
            FillMap(list, sys);
        }

        private void FillMap(SortedListDoubleDuplicate<ISystem> list, ISystem sys)
        {
            SetControlText(string.Format("3D Map of closest systems from {0}".T(EDTx.UserControlAstroPlot), sys.Name));

            systemsInRange.Add(new object[]
            {
                sys.Name,
                sys.X,
                sys.Y,
                sys.Z,
                true,
                false,
                true
            });

            if (list.Any())
            {
                foreach (KeyValuePair<double, ISystem> sysInRange in list)
                {
                    if (sysInRange.Value.Name != sys.Name)
                    {
                        var visited = discoveryform.history.GetVisitsCount(sysInRange.Value.Name) > 0;

                        systemsInRange.Add(new object[]
                        {
                        sysInRange.Value.Name,
                        sysInRange.Value.X,
                        sysInRange.Value.Y,
                        sysInRange.Value.Z,
                        visited,
                        false,
                        false
                        });
                    }
                }
            }

            astroPlot.SetCenterCoordinates(sys.X, sys.Y, sys.Z);
            astroPlot.AddSystemsToMap(systemsInRange);
        }
    }
}

