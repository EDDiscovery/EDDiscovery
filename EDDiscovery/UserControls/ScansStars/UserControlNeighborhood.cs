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
using System.Collections.Concurrent;
using System.Threading;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;
using System.Diagnostics;
using BaseUtils;
using System.Diagnostics.Contracts;

namespace EDDiscovery.UserControls
{
    public partial class UserControlNeighborhood : UserControlCommonBase
    {
        private string DbSave { get { return DBName("Neighborhood"); } }

        private StarDistanceComputer computer;
        private HistoryEntry last_he = null;
        private const double defaultMapMaxRadius = 100;
        private const double defaultMapMinRadius = 0;
        private readonly int maxitems = 250;

        public UserControlNeighborhood()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            computer = new StarDistanceComputer();
            
            numberBoxMin.ValueNoChange = UserDatabase.Instance.GetSettingDouble(DbSave + "MinRange", defaultMapMinRadius);
            numberBoxMax.ValueNoChange = UserDatabase.Instance.GetSettingDouble(DbSave + "MaxRange", defaultMapMaxRadius);
            numberBoxMin.SetComparitor(numberBoxMax, -2);
            numberBoxMax.SetComparitor(numberBoxMin, 2);

            astroPlot.HotSpotSize = 10;
            astroPlot.SmallDotSize = 7;
            astroPlot.Distance = (int)(numberBoxMax.Value * 0.75);
            astroPlot.AxesLength = (int)(numberBoxMax.Value / 2);
            astroPlot.MouseWheel_Multiply = 0.8;
            astroPlot.MouseWheel_Resistance = 1.2;
            astroPlot.Elevation = -0.3;
            astroPlot.Azimuth = -0.3;
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
        }

        public override void Closing()
        {
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MinRange", numberBoxMin.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MaxRange", numberBoxMax.Value);
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            KickComputation(he);
        }

        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
        {
            System.Diagnostics.Debug.Assert(Application.MessageLoop);       // check!
            discoveryform.history.CalculateSqDistances(list, sys.X, sys.Y, sys.Z, maxitems, numberBoxMin.Value, numberBoxMax.Value, true);
            FillMap(list, sys);
        }
        
        private void KickComputation(HistoryEntry he)
        {
            if (he?.System != null && he.System.HasCoordinate)
            {
                computer.CalculateClosestSystems(he.System,
                    (s, d) => this.ParentForm.BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
                    maxitems, defaultMapMinRadius, defaultMapMaxRadius, true);
            }
        }

        private List<object[]> systemsInRange = new List<object[]>();

        private void FillMap(SortedList<double, ISystem> sysInRange, ISystem centerSystem)
        {
            astroPlot.Clear();
            systemsInRange.Clear();

            if (sysInRange.Count > 0)
            {
                foreach (KeyValuePair<double, ISystem> tvp in sysInRange)
                {
                    if (tvp.Value == centerSystem)
                    {
                        astroPlot.SetCenterCoordinates(new double[] {tvp.Value.X, tvp.Value.Y, tvp.Value.Z });
                    }

                    systemsInRange.Add(new object[]
                    {
                        tvp.Value.Name,
                        tvp.Value.X,
                        tvp.Value.Y,
                        tvp.Value.Z,
                        discoveryform.history.GetVisitsCount(tvp.Value.Name) > 0,
                        false,
                        tvp.Value == centerSystem,
                    });
                }
                
                astroPlot.AddSystemsToMap(systemsInRange);
            }
        }

        private void NumberBoxMax_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(last_he ?? uctg.GetCurrentHistoryEntry);
        }

        private void NumberBoxMin_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(last_he ?? uctg.GetCurrentHistoryEntry);
        }
    }
}
