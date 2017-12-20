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
using EliteDangerousCore.JournalEvents;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScanGrid : UserControlCommonBase
    {
        private HistoryEntry last_he = null;
        private string DbColumnSave { get { return ("ScanGridPanel") + ((displaynumber > 0) ? displaynumber.ToString() : "") + "DGVCol"; } }

        public UserControlScanGrid()
        {
            InitializeComponent();
            var corner = dataGridViewScangrid.TopLeftHeaderCell; // work around #1487

            // dataGridView setup - the rule is, use the designer for most properties.. only do these here since they are so buried or not available.

            // this allows the row to grow to accomodate the text.. with a min height of 32.
            dataGridViewScangrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewScangrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            dataGridViewScangrid.RowTemplate.MinimumHeight = 32;
            this.dataGridViewScangrid.Columns["ImageColumn"].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;
        }

        public override void Init()
        {
            uctg.OnTravelSelectionChanged += Display;
            discoveryform.OnNewEntry += NewEntry;
            labelTotalValue.Text = $"No scan data yet.";
        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridViewScangrid, DbColumnSave);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;            
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewScangrid, DbColumnSave);
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= NewEntry;
        }

        public override void InitialDisplay()
        {
            Display(uctg.GetCurrentHistoryEntry, discoveryform.history);
        }             

        public void NewEntry(HistoryEntry he, HistoryList hl) // called when a new entry is made.. check to see if its a scan update
        {
            // if he valid, and last is null, or not he, or we have a new scan
            if (he != null && (last_he == null || he != last_he || he.EntryType == JournalTypeEnum.Scan))
            {
                last_he = he;
                DrawSystem();                
            }
        }
               

        private void Display(HistoryEntry he, HistoryList hl) // Called at first start or hooked to change cursor
        {
            if (he != null && (last_he == null || he.System != last_he.System))
            {
                last_he = he;
                labelTotalValue.Text = $"No scan data available.";
                dataGridViewScangrid.Refresh();
                DrawSystem();
                dataGridViewScangrid.ClearSelection();                
            }
        }

        void DrawSystem() // draw last_sn, last_he
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
                        if (sn.ScanData != null && sn.ScanData.BodyName != null)
                    {                        
                        StringBuilder bdClass = new StringBuilder();
                        StringBuilder bdDist = new StringBuilder();
                        StringBuilder bdDetails = new StringBuilder();

                        if (sn.ScanData.PlanetClass != null)
                            bdClass.Append(sn.ScanData.PlanetClass);
                        if (sn.ScanData.StarTypeText != null)
                            bdClass.Append(sn.ScanData.StarTypeText);

                        if (sn.level >= 2 && sn.type == StarScan.ScanNodeType.body)
                        {
                            bdClass.Append(" Moon");
                        }

                        if (sn.ScanData.IsStar && sn.ScanData.BodyName.EndsWith(" A"))
                        {
                            bdDist.AppendFormat("Main Star");
                        }
                        else if (sn.ScanData.nSemiMajorAxis.HasValue)
                        {
                            if (sn.ScanData.IsStar || sn.ScanData.nSemiMajorAxis.Value > JournalScan.oneAU_m / 10)
                                bdDist.AppendFormat("{0:0.00}AU ({1:0.00}ls)", (sn.ScanData.nSemiMajorAxis.Value / JournalScan.oneAU_m), sn.ScanData.nSemiMajorAxis.Value / JournalScan.oneLS_m);
                            else
                                bdDist.AppendFormat("{0}km", (sn.ScanData.nSemiMajorAxis.Value / 1000).ToString("N1"));
                        }

                        // display stars and stellar bodies mass
                        if (sn.ScanData.IsStar && sn.ScanData.nStellarMass.HasValue)
                            bdDetails.Append("Mass:" + sn.ScanData.nStellarMass.Value.ToString("N2") + ", ");

                        // habitable zone for stars - do not display for black holes.
                        if (sn.ScanData.HabitableZoneInner != null && sn.ScanData.HabitableZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                            bdDetails.AppendFormat("Habitable Zone: {0}-{1}AU ({2}). ", (sn.ScanData.HabitableZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.HabitableZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetHabZoneStringLs());

                        // tell us that a bodie is landable, and shows its gravity
                        if (sn.ScanData.IsLandable == true)
                        {
                            string Gg = "";

                            if (sn.ScanData.nSurfaceGravity.HasValue)
                            {
                                double? g = sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2;
                                Gg = " (G: " + g.Value.ToString("N1") + ")";
                            }

                            bdDetails.Append("Landable" + Gg + ". ");
                        }

                        // append the terraformable state to the planet class
                        if (sn.ScanData.Terraformable == true)
                            bdDetails.Append("Terraformable. ");                                               

                        // tell us that there is some volcanic activity
                        if (sn.ScanData.Volcanism != null)
                            bdDetails.Append("Volcanism. ");

                        // have some ring?
                        if (sn.ScanData.HasRings && sn.ScanData.IsStar == false)
                        {
                            if (sn.ScanData.Rings.Count() > 1)
                            {
                                bdDetails.Append("Has " + sn.ScanData.Rings.Count() + " rings: ");
                            }
                            else
                            {
                                bdDetails.Append("Has 1 ring: ");
                            }

                            for (int i = 0; i < sn.ScanData.Rings.Length; i++)
                            {
                                string RingName = sn.ScanData.Rings[i].Name;
                                bdDetails.Append(JournalScan.StarPlanetRing.DisplayStringFromRingClass(sn.ScanData.Rings[i].RingClass) + " ");
                                bdDetails.Append((sn.ScanData.Rings[i].InnerRad / JournalScan.oneLS_m).ToString("N2") + "ls to " + (sn.ScanData.Rings[i].OuterRad / JournalScan.oneLS_m).ToString("N2") + "ls. ");
                            }
                        }                                                        

                        // print the main atmospheric composition
                        if (sn.ScanData.Atmosphere != null && sn.ScanData.Atmosphere != "None")
                            bdDetails.Append(sn.ScanData.Atmosphere + ". ");

                        int value = sn.ScanData.EstimatedValue;
                        bdDetails.Append("Value " + value.ToString("N0"));

                        Image img = null;

                        if (sn.ScanData.IsStar == true)
                        {
                            img = sn.ScanData.GetStarTypeImage(); // if is a star, use the Star image
                        }
                        else 
                        {
                            img = sn.ScanData.GetPlanetClassImage(); // use the correct image in case of planets and moons
                        }

                        dataGridViewScangrid.Rows.Add(new object[] { null, sn.ScanData.BodyName, bdClass, bdDist, bdDetails });

                        DataGridViewRow cur = dataGridViewScangrid.Rows[dataGridViewScangrid.Rows.Count - 1];

                        string scan = sn.ScanData.DisplayString(); // display tooltip with full information when hower bodies image and name
                        cur.Cells[0].ToolTipText = scan;
                        cur.Cells[1].ToolTipText = scan;
                        cur.Cells[2].ToolTipText = scan;
                        cur.Cells[3].ToolTipText = scan;
                        cur.Cells[4].ToolTipText = scan;

                        cur.Tag = img;

                        BuildSystemInfo(last_sn);
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

        private void dataGridViewScangrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow cur = dataGridViewScangrid.Rows[e.RowIndex];
            if ( cur.Tag != null )
            {
                // we programatically draw the image because we have control over its pos/ size this way, which you can't do
                // with a image column - there you can only draw a fixed image or stretch it to cell contents.. which we don't want to do
                int sz = dataGridViewScangrid.RowTemplate.MinimumHeight - 2;
                int vpos = e.RowBounds.Top + e.RowBounds.Height / 2 - sz / 2;
                e.Graphics.DrawImage((Image)cur.Tag, new Rectangle(e.RowBounds.Left+1,vpos,sz,sz));
            }
        }              

        private void BuildSystemInfo(StarScan.SystemNode system)
        {
            labelTotalValue.Text = BuildScanValue(system);
        }

        private string BuildScanValue(StarScan.SystemNode system)
        {
            var value = 0;

                foreach (var body in system.Bodies)
                {
                    if (body?.ScanData?.EstimatedValue != null)
                    {
                        value += body.ScanData.EstimatedValue;
                    }
                }

            return $"Approx total scan value: {value:N0}";
        }
    }
}
