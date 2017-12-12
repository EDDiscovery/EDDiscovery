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
using System.Drawing.Drawing2D;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using EliteDangerousCore.DB;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScanGrid : UserControlCommonBase
    {
        private HistoryEntry last_he = null;
        
        public UserControlScanGrid()
        {
            InitializeComponent();
            var corner = dataGridViewScangrid.TopLeftHeaderCell; // work around #1487
        }

        public override void Init()
        {
            uctg.OnTravelSelectionChanged += Display;
            discoveryform.OnNewEntry += NewEntry;
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= NewEntry;
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl)               // called when a new entry is made.. check to see if its a scan update
        {
            // if he valid, and last is null, or not he, or we have a new scan
            if (he != null && (last_he == null || he != last_he || he.EntryType == JournalTypeEnum.Scan))
            {
                last_he = he;
                DrawSystem();
            }
        }

        private void Display(HistoryEntry he, HistoryList hl)            // Called at first start or hooked to change cursor
        {
            if (he != null && (last_he == null || he.System != last_he.System))
            {
                last_he = he;
                DrawSystem();
            }
        }

        void DrawSystem()   // draw last_sn, last_he
        {
            dataGridViewScangrid.Rows.Clear();

            if (last_he == null)
            {
                SetControlText("No Scan");
                return;
            }


            StarScan.SystemNode last_sn = discoveryform.history.starscan.FindSystem(last_he.System, true);

            SetControlText((last_sn == null) ? "No Scan" : ("Brief Scan Summary for " + last_sn.system.name));

            if (last_sn != null)
            {
                List<StarScan.ScanNode> all_nodes = new List<StarScan.ScanNode>();
                foreach (StarScan.ScanNode starnode in last_sn.starnodes.Values)
                {
                    all_nodes = Flatten(starnode, all_nodes);
                }


                // flatten tree of scan nodes to prepare for listing
                foreach (StarScan.ScanNode sn in all_nodes)
                {

                    
                    // create the grid data

                    // populate the body class
                    StringBuilder bdClass = new StringBuilder();
                    
                    if (sn.ScanData != null && sn.ScanData.BodyName != null && sn.ScanData.PlanetClass != null)
                    {
                        bdClass.Append(sn.ScanData.PlanetClass);
                    }
                    if (sn.ScanData != null && sn.ScanData.BodyName != null && sn.ScanData.StarTypeText != null)
                    {
                        bdClass.Append(sn.ScanData.StarTypeText);
                    }
                    if (sn.level >= 2 && sn.type == StarScan.ScanNodeType.body)
                    {
                        bdClass.Append(" Moon");
                    }

                        // populate the detailed information
                        StringBuilder bdDetails = new StringBuilder();

                    if (sn.ScanData != null && sn.ScanData.BodyName != null && sn.ScanData.Terraformable == true)
                    {
                        bdDetails.Append("Terraformable. ");
                    }

                    if (sn.ScanData != null && sn.ScanData.BodyName != null && sn.ScanData.IsLandable == true)
                    {
                        double? g = sn.ScanData.nSurfaceGravity;
                        double? oneGee_m_s2 = 9.80665;
                        if (g.HasValue)
                            g = g / oneGee_m_s2;
                        string Gg = g.Value.ToString("N1");
                        bdDetails.Append("Landable, " + "(G: " + Gg + "). ");
                    }

                    if (sn.ScanData != null && sn.ScanData.BodyName != null && sn.ScanData.HasRings == true && sn.ScanData.IsStar == false)
                    {
                        bdDetails.Append("Ringed. ");
                    }

                    if (sn.ScanData != null && sn.ScanData.BodyName != null && sn.ScanData.Volcanism != null)
                    {
                        bdDetails.Append("Volcanism. ");
                    }                                      

                    if (sn.ScanData != null && sn.ScanData.BodyName != null && sn.ScanData.IsStar == true)
                    {
                        bdDetails.Append("M:" + sn.ScanData.nStellarMass.Value.ToString("N2") + ", ");
                    }
                                        
                    // habitable zone - do not display for black holes.
                    if (sn.ScanData != null && sn.ScanData.BodyName != null && sn.ScanData.HabitableZoneInner != null && sn.ScanData.HabitableZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                    {
                        bdDetails.AppendFormat("Habitable Zone Approx. {0}", sn.ScanData.GetHabZoneStringLs(), (sn.ScanData.HabitableZoneInner.Value).ToString("N2"), (sn.ScanData.HabitableZoneOuter.Value).ToString("N2"));
                    }

                    // populate the grid                                                          
                    if (sn.ScanData != null && sn.ScanData.BodyName != null && bdClass != null && bdDetails != null && sn.ScanData.IsStar == true)
                    {
                        Image bdImage = sn.ScanData.GetStarTypeImage(); // if is a star, use the Star image
                        dataGridViewScangrid.Rows.Add(new object[] { bdImage, sn.ScanData.BodyName, bdClass, bdDetails });
                    }

                    if (sn.ScanData != null && sn.ScanData.BodyName != null && bdClass != null && bdDetails != null && sn.ScanData.IsStar == false)
                    {
                        Image bdImage = sn.ScanData.GetPlanetClassImage(); // if is a planet or a moon, use the Planet image
                        dataGridViewScangrid.Rows.Add(new object[] { bdImage, sn.ScanData.BodyName, bdClass, bdDetails });
                    }
                    
                }

            }
        }

        private List<StarScan.ScanNode> Flatten(StarScan.ScanNode sn, List<StarScan.ScanNode> flattened)
        {
            flattened.Add(sn);


            if (sn.children != null)
            {
                foreach (StarScan.ScanNode node in sn.children.Values)
                {
                    Flatten(node, flattened);
                }
            }
            return flattened;
        }

    }
}
