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
        private string DbSave { get { return DBName("AstroPlot"); } }

        private StarDistanceComputer computer;
        private HistoryEntry last_he = null;
        private List<object[]> systemsInRange = new List<object[]>();
        private HotSpotMap hotSpotMap = new HotSpotMap();

        private string howerName;
        private Point howerLocation;

        private int maxSystems = 150;
        private const double minRadius = 0;
        private const double maxRadius = 100;

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

            maxSystems = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingInt(DbSave + "MaxSystems", maxSystems);
            numberBoxMaxRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MaxRadius", maxRadius);
            numberBoxMinRadius.ValueNoChange = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingDouble(DbSave + "MinRadius", minRadius);
            toolStripMenuItem25.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "25sys", false);
            toolStripMenuItem50.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "50sys", false);
            toolStripMenuItem100.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "100sys", false);
            toolStripMenuItem150.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "150sys", true);
            toolStripMenuItem200.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "200sys", false);
            toolStripMenuItem250.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "250sys", false);
            numberBoxMinRadius.SetComparitor(numberBoxMaxRadius, -2);
            numberBoxMaxRadius.SetComparitor(numberBoxMinRadius, 2);

            astroPlot.MouseWheel_Multiply = 1;
            astroPlot.MouseWheel_Resistance = 70;

            astroPlot.Distance = 50;
            astroPlot.Elevation = -0.4;
            astroPlot.Azimuth = -0.3;
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
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MinRadius", numberBoxMinRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingDouble(DbSave + "MaxRadius", numberBoxMaxRadius.Value);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingInt(DbSave + "MaxSystems", maxSystems);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "25sys", toolStripMenuItem25.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "50sys", toolStripMenuItem50.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "100sys", toolStripMenuItem100.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "150sys", toolStripMenuItem150.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "200sys", toolStripMenuItem200.Checked);
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "250sys", toolStripMenuItem250.Checked);
            uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
            computer.ShutDown();
            base.Closing();
        }

        public override void InitialDisplay()
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        /// <summary>
        /// Triggered when the highlighted system in the history list changes
        /// </summary>
        /// <param name="he"></param>
        /// <param name="hl"></param>
        /// <param name="selectedEntry"></param>
        private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            KickComputation(he);
        }

        /// <summary>
        /// Calculate a list of nearest systems, according to given parameters
        /// </summary>
        /// <param name="he"></param>
        private void KickComputation(HistoryEntry he)
        {
            systemsInRange.Clear();
            astroPlot.Clear();

            if (he?.System != null && he.System.HasCoordinate)
            {
                computer.CalculateClosestSystems(he.System,
                    (s, d) => this.ParentForm.BeginInvoke((MethodInvoker)delegate { NewStarListComputed(s, d); }),
                    maxSystems, numberBoxMinRadius.Value, numberBoxMaxRadius.Value, true);
            }
        }

        /// <summary>
        /// Tirgger the population of the plot with the generated systems list
        /// </summary>
        /// <param name="sys"></param>
        /// <param name="list"></param>
        private void NewStarListComputed(ISystem sys, BaseUtils.SortedListDoubleDuplicate<ISystem> list)      // In UI
        {
            //Debug.Assert(Application.MessageLoop);       // check!
            discoveryform.history.CalculateSqDistances(list, sys.X, sys.Y, sys.Z, maxSystems, numberBoxMinRadius.Value, numberBoxMaxRadius.Value, true);
            FillMap(list, sys);
        }

        /// <summary>
        /// Populate the plot with the given systems list
        /// </summary>
        /// <param name="list"></param>
        /// <param name="sys"></param>
        private void FillMap(SortedListDoubleDuplicate<ISystem> list, ISystem sys)
        {
            SetControlText(string.Format("3D Map of closest systems from {0}".T(EDTx.UserControlAstroPlot), sys.Name));

            systemsInRange.Add(new object[]
            {
                sys.Name,
                sys.X,
                sys.Y,
                sys.Z,
                true, // is visited
                false, // is waypoint
                true // is current
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

        /// <summary>
        /// Toggle axes widget
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            astroPlot.ShowAxesWidget = showToolStripShowAxes.Checked;
        }

        /// <summary>
        /// Toggle frames widget
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showToolStripMenuItem1_CheckedChanged(object sender, EventArgs e)
        {
            astroPlot.ShowFrameWidget = showToolStripShowFrames.Checked;
        }

        /// <summary>
        /// Change to cube frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cubeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (cubeToolStripMenuItem.Checked)
            {
                planesToolStripMenuItem.Checked = false;
                astroPlot.FrameShape = ExtendedControls.Controls.AstroPlot.Shape.Cube;
            }
        }

        /// <summary>
        /// Change to planes frame
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void planesToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (planesToolStripMenuItem.Checked)
            {
                cubeToolStripMenuItem.Checked = false;
                astroPlot.FrameShape = ExtendedControls.Controls.AstroPlot.Shape.Planes;
            }
        }

        private void numberBoxMaxRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void numberBoxMinRadius_ValueChanged(object sender, EventArgs e)
        {
            KickComputation(uctg.GetCurrentHistoryEntry);
        }
        
        private void toolStripMenuItem250_Click(object sender, EventArgs e)
        {
            toolStripMenuItem250.Checked = true;
            toolStripMenuItem200.Checked = false;
            toolStripMenuItem150.Checked = false;
            toolStripMenuItem100.Checked = false;
            toolStripMenuItem50.Checked = false;
            toolStripMenuItem25.Checked = false;

            maxSystems = 250;

            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void toolStripMenuItem200_Click(object sender, EventArgs e)
        {
            toolStripMenuItem250.Checked = false;
            toolStripMenuItem200.Checked = true;
            toolStripMenuItem150.Checked = false;
            toolStripMenuItem100.Checked = false;
            toolStripMenuItem50.Checked = false;
            toolStripMenuItem25.Checked = false;

            maxSystems = 200;

            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void toolStripMenuItem150_Click(object sender, EventArgs e)
        {
            toolStripMenuItem250.Checked = false;
            toolStripMenuItem200.Checked = false;
            toolStripMenuItem150.Checked = true;
            toolStripMenuItem100.Checked = false;
            toolStripMenuItem50.Checked = false;
            toolStripMenuItem25.Checked = false;

            maxSystems = 150;

            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void toolStripMenuItem100_Click(object sender, EventArgs e)
        {
            toolStripMenuItem250.Checked = false;
            toolStripMenuItem200.Checked = false;
            toolStripMenuItem150.Checked = false;
            toolStripMenuItem100.Checked = true;
            toolStripMenuItem50.Checked = false;
            toolStripMenuItem25.Checked = false;

            maxSystems = 100;

            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void toolStripMenuItem50_Click(object sender, EventArgs e)
        {
            toolStripMenuItem250.Checked = false;
            toolStripMenuItem200.Checked = false;
            toolStripMenuItem150.Checked = false;
            toolStripMenuItem100.Checked = false;
            toolStripMenuItem50.Checked = true;
            toolStripMenuItem25.Checked = false;

            maxSystems = 50;

            KickComputation(uctg.GetCurrentHistoryEntry);
        }

        private void toolStripMenuItem25_Click(object sender, EventArgs e)
        {
            toolStripMenuItem250.Checked = false;
            toolStripMenuItem200.Checked = false;
            toolStripMenuItem150.Checked = false;
            toolStripMenuItem100.Checked = false;
            toolStripMenuItem50.Checked = false;
            toolStripMenuItem25.Checked = true;

            maxSystems = 25;

            KickComputation(uctg.GetCurrentHistoryEntry);
        }
    }
}

