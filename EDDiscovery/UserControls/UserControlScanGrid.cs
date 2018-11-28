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
        private string DbColumnSave { get { return DBName("ScanGridPanel", "DGVCol"); } }

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
            discoveryform.OnNewEntry += NewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
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
            DrawSystem(uctg.GetCurrentHistoryEntry, false);
        }

        public void NewEntry(HistoryEntry he, HistoryList hl) // called when a new entry is made.. check to see if its a scan update
        {
            DrawSystem(he, he.EntryType == JournalTypeEnum.Scan);
        }

        private void Display(HistoryEntry he, HistoryList hl) // Called at first start or hooked to change cursor
        {
            DrawSystem(he, false);
        }

        void DrawSystem(HistoryEntry he, bool force)
        {
            StarScan.SystemNode scannode = null;

            bool samesys = last_he != null && he != null && he.System.Name == last_he.System.Name;

            //System.Diagnostics.Debug.WriteLine("Scan grid " + samesys + " F:" + force);

            if (he == null)     //  no he, no display
            {
                last_he = null;
                dataGridViewScangrid.Rows.Clear();
                SetControlText("No Scan".Tx());
                return;
            }
            else
            {
                scannode = discoveryform.history.starscan.FindSystem(he.System, true);        // get data with EDSM

                if (scannode == null)     // no data, clear display, clear any last_he so samesys is false next time
                {
                    last_he = null;
                    dataGridViewScangrid.Rows.Clear();
                    SetControlText("No Scan".Tx());
                    return;
                }

                if (samesys && !force)      // same system, no force, no redisplay
                    return;
            }

            last_he = he;

            // only record first row if same system 
            int firstdisplayedrow = (dataGridViewScangrid.RowCount > 0 && samesys) ? dataGridViewScangrid.FirstDisplayedScrollingRowIndex : -1;

            dataGridViewScangrid.Rows.Clear();

            List<StarScan.ScanNode> all_nodes = scannode.Bodies.ToList();// flatten tree of scan nodes to prepare for listing

            foreach (StarScan.ScanNode sn in all_nodes)
            {
                // check for null data
                if (sn.ScanData != null && sn.ScanData.BodyName != null)
                {
                    // define strings to be populated
                    StringBuilder bdClass = new StringBuilder();
                    StringBuilder bdDist = new StringBuilder();
                    StringBuilder bdDetails = new StringBuilder();

                    // check for body class
                    if (sn.ScanData.IsStar)
                    {
                        // is a star, so populate its information field with relevant data

                        // star class
                        if (sn.ScanData.StarTypeText != null)
                            bdClass.Append(sn.ScanData.StarTypeText);

                        // is the main star?
                        if (sn.ScanData.BodyName.EndsWith(" A"))
                        {
                            bdDist.AppendFormat("Main Star".Tx(this));
                        }
                        // if not, then tell us its hierarchy leveland distance from main star
                        else if (sn.ScanData.nSemiMajorAxis.HasValue)
                        {
                            if (sn.ScanData.IsStar || sn.ScanData.nSemiMajorAxis.Value > JournalScan.oneAU_m / 10)
                                bdDist.AppendFormat("{0:0.00}AU ({1:0.00}ls)", (sn.ScanData.nSemiMajorAxis.Value / JournalScan.oneAU_m), sn.ScanData.nSemiMajorAxis.Value / JournalScan.oneLS_m);
                            else
                                bdDist.AppendFormat("{0}km", (sn.ScanData.nSemiMajorAxis.Value / 1000).ToString("N1"));
                        }

                        // display stellar bodies mass, in sols
                        if (sn.ScanData.nStellarMass.HasValue)
                            bdDetails.Append("Mass".Tx(this) + ": " + sn.ScanData.nStellarMass.Value.ToString("N2") + ", ");

                        // display stellar bodies radius in sols
                        if (sn.ScanData.nRadius.HasValue)
                            bdDetails.Append("Radius".Tx(this) + ": " + (sn.ScanData.nRadius.Value / JournalScan.oneSolRadius_m).ToString("N2") + ", ");

                        // show the temperature
                        if (sn.ScanData.nSurfaceTemperature.HasValue)
                            bdDetails.Append("Temperature".Tx(this) + ": " + (sn.ScanData.nSurfaceTemperature.Value) + "K.");

                        // habitable zone for stars - do not display for black holes.
                        if (sn.ScanData.HabitableZoneInner != null && sn.ScanData.HabitableZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                            bdDetails.AppendFormat(Environment.NewLine + "Habitable Zone".Tx(this) + ": {0}-{1}AU ({2}). ", (sn.ScanData.HabitableZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.HabitableZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetHabZoneStringLs());                        
                    }
                    else
                    {
                        // is a non-stellar body                        
                            
                        // is terraformable? If so, prepend it to the body class
                        if (sn.ScanData.Terraformable == true)
                            bdClass.Append("Terraformable".Tx(this) + ", ");

                        // is a planet?...
                        if (sn.ScanData.PlanetClass != null)
                            bdClass.Append(sn.ScanData.PlanetClass);

                        // ...or a moon?
                        if (sn.level >= 2 && sn.type == StarScan.ScanNodeType.body)
                        {
                            bdClass.Append(" " + "Moon".Tx(this));
                        }

                        // Details
                        
                        // display non-stellar bodies radius in earth radiuses
                        if (sn.ScanData.nRadius.HasValue)
                            bdDetails.Append("Radius".Tx(this) + ": " + (sn.ScanData.nRadius.Value / JournalScan.oneEarthRadius_m).ToString("N2") + ", ");

                        // show the temperature, both in K and C degrees
                        if (sn.ScanData.nSurfaceTemperature.HasValue)
                            bdDetails.Append("Temperature".Tx(this) + ": " + (sn.ScanData.nSurfaceTemperature.Value).ToString("N2") + "K, (" + (sn.ScanData.nSurfaceTemperature.Value - 273).ToString("N2") + "C).");

                        // print the main atmospheric composition and pressure, if presents
                        if (sn.ScanData.Atmosphere != null && sn.ScanData.Atmosphere != "None")
                            bdDetails.Append(Environment.NewLine + sn.ScanData.Atmosphere + ", " + (sn.ScanData.nSurfacePressure.Value / JournalScan.oneAtmosphere_Pa).ToString("N3") + "Pa.");

                        // tell us that a bodie is landable, and shows its gravity
                        if (sn.ScanData.IsLandable)
                        {
                            string Gg = "";

                            if (sn.ScanData.nSurfaceGravity.HasValue)
                            {
                                double? g = sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2;
                                Gg = " (G: " + g.Value.ToString("N1") + ")";
                            }

                            bdDetails.Append(Environment.NewLine + "Landable".Tx(this) + Gg + ". ");                         
                        }
                                                
                        // tell us that there is some volcanic activity
                        if (sn.ScanData.Volcanism != null)
                            bdDetails.Append(Environment.NewLine + "Volcanic activity".Tx(this) + ". ");

                        // materials                        
                        if (sn.ScanData.HasMaterials)
                        {
                            string ret = "";
                            foreach (KeyValuePair<string, double> mat in sn.ScanData.Materials)
                            {
                                MaterialCommodityData mc = MaterialCommodityData.GetByFDName(mat.Key);
                                if (mc != null && mc.IsJumponium)
                                    ret = ret.AppendPrePad(mc.Name, ", ");
                            }

                            if (ret.Length > 0)
                                bdDetails.Append(Environment.NewLine + "This body contains: ".Tx(this, "BC") + ret);
                        }
                    }
                                                           
                    // have some belt or ring?
                    if (sn.ScanData.HasRings)
                    {
                        for (int r = 0; r < sn.ScanData.Rings.Length; r++)
                        {
                            if (sn.ScanData.Rings[r].Name.EndsWith("Belt"))
                            {
                                // is a belt
                                bdDetails.Append(Environment.NewLine + "Belt: ".Tx(this, "Belt"));
                                string RingName = sn.ScanData.Rings[r].Name;
                                bdDetails.Append(JournalScan.StarPlanetRing.DisplayStringFromRingClass(sn.ScanData.Rings[r].RingClass) + " ");
                                bdDetails.Append((sn.ScanData.Rings[r].InnerRad / JournalScan.oneLS_m).ToString("N2") + "ls to " + (sn.ScanData.Rings[r].OuterRad / JournalScan.oneLS_m).ToString("N2") + "ls. ");                         
                            }
                            else
                            {
                                // is a ring
                                bdDetails.Append(Environment.NewLine + "Ring: ".Tx(this, "Ring"));
                                string RingName = sn.ScanData.Rings[r].Name;
                                bdDetails.Append(JournalScan.StarPlanetRing.DisplayStringFromRingClass(sn.ScanData.Rings[r].RingClass) + " ");
                                bdDetails.Append((sn.ScanData.Rings[r].InnerRad / JournalScan.oneLS_m).ToString("N2") + "ls to " + (sn.ScanData.Rings[r].OuterRad / JournalScan.oneLS_m).ToString("N2") + "ls. ");
                            }
                        }                        
                    }

                    // for all bodies:
                   
                    // give estimated value
                    int value = sn.ScanData.EstimatedValue;
                    bdDetails.Append(Environment.NewLine + "Value".Tx(this) + " " + value.ToString("N0"));
                    
                    // pick an image
                    Image img = sn.ScanData.IsStar == true ? sn.ScanData.GetStarTypeImage() : sn.ScanData.GetPlanetClassImage();

                    dataGridViewScangrid.Rows.Add(new object[] { null, sn.ScanData.BodyName, bdClass, bdDist, bdDetails });

                    DataGridViewRow cur = dataGridViewScangrid.Rows[dataGridViewScangrid.Rows.Count - 1];

                    cur.Tag = img;
                    cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
                            sn.ScanData.DisplayString(historicmatlist: last_he.MaterialCommodity, currentmatlist: discoveryform.history.GetLast?.MaterialCommodity);
                }
            }

            // calculate the estimated total scan values
            SetControlText(string.Format("Scan Summary for {0}. {1}".Tx(this, "SS"), scannode.system.Name, BuildScanValue(scannode)));

            if (firstdisplayedrow >= 0 && firstdisplayedrow < dataGridViewScangrid.RowCount)
                dataGridViewScangrid.FirstDisplayedScrollingRowIndex = firstdisplayedrow;
        }

        private void dataGridViewScangrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridViewRow cur = dataGridViewScangrid.Rows[e.RowIndex];
            if (cur.Tag != null)
            {
                // we programatically draw the image because we have control over its pos/ size this way, which you can't do
                // with a image column - there you can only draw a fixed image or stretch it to cell contents.. which we don't want to do
                int sz = dataGridViewScangrid.RowTemplate.MinimumHeight - 2;
                int vpos = e.RowBounds.Top + e.RowBounds.Height / 2 - sz / 2;
                e.Graphics.DrawImage((Image)cur.Tag, new Rectangle(e.RowBounds.Left + 1, vpos, sz, sz));
            }
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

            return string.Format("Estimated total scan value: {0:N0}".Tx(this, "AV"), value);
        }

        private void dataGridViewScangrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                object curdata = dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Value;
                dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Value = dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Tag;
                dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Tag = curdata;
            }
        }
    }
}
