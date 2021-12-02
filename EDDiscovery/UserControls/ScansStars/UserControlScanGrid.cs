﻿/*
 * Copyright © 2016 - 2019 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static EDDiscovery.UserControls.PaintHelpers;

namespace EDDiscovery.UserControls
{
    public partial class UserControlScanGrid : UserControlCommonBase
    {
        private HistoryEntry last_he = null;

        private int iconsize;   // computed icon and body sizes
        private int bodysize;

        public UserControlScanGrid()
        {
            InitializeComponent();

            // this allows the row to grow to accomodate the text.. with a min height of 48px.
            dataGridViewScangrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewScangrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            this.dataGridViewScangrid.Columns[nameof(colImage)].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
        }

        #region Init

        public override void Init()
        {
            DBBaseName = "ScanGridPanel";

            discoveryform.OnNewEntry += NewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            // retrieve context menu entries check state from DB

            checkBoxEDSM.Checked = GetSetting("checkEDSM", true);
            checkBoxEDSM.Click += CheckBoxEDSM_Click;

            PopulateCtrlList();
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewScangrid);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewScangrid);
            uctg.OnTravelSelectionChanged -= Display;
            discoveryform.OnNewEntry -= NewEntry;
        }

        public override void InitialDisplay()
        {
            DrawSystem(uctg.GetCurrentHistoryEntry, false);
        }

        private void NewEntry(HistoryEntry he, HistoryList hl)
        {
            DrawSystem(he, he.journalEntry is IStarScan ); // not IBodyNameAndID because all that can do is add an empty scan node, and we do not present info if no scan data
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
        {
            DrawSystem(he, false);
        }

        #endregion

        #region PopulateGrid

        private async void DrawSystem(HistoryEntry he, bool force)
        {
            var samesys = last_he?.System != null && he?.System != null && he.System.Name == last_he.System.Name;

            //System.Diagnostics.Debug.WriteLine("Scan grid " + samesys + " F:" + force);

            StarScan.SystemNode scannode = null;

            if (he == null)     //  no he, no display
            {
                last_he = he;
                dataGridViewScangrid.Rows.Clear();
                SetControlText("No Scan".T(EDTx.NoScan));
                return;
            }
            else
            {
                scannode = await discoveryform.history.StarScan.FindSystemAsync(he.System, checkBoxEDSM.Checked);        // get data with EDSM maybe

                if (scannode == null)     // no data, clear display, clear any last_he so samesys is false next time
                {
                    last_he = null;
                    dataGridViewScangrid.Rows.Clear();
                    SetControlText("No Scan".T(EDTx.NoScan));
                    return;
                }

                if (samesys && !force)      // same system, no force, no redisplay
                    return;
            }

            last_he = he;

            // only record first row if same system 
            var firstdisplayedrow = (dataGridViewScangrid.RowCount > 0 && samesys) ? dataGridViewScangrid.SafeFirstDisplayedScrollingRowIndex() : -1;

            toolStripJumponiumProgressBar.Visible = false;
            toolStripJumponiumProgressBar.Value = 0;     // reset the jumponium progress

            dataGridViewScangrid.RowTemplate.MinimumHeight = Font.ScalePixels(64);        // based on icon size
            bodysize = dataGridViewScangrid.RowTemplate.MinimumHeight;
            iconsize = bodysize / 4;
         
            dataGridViewScangrid.Rows.Clear();

            var all_nodes = scannode.Bodies.ToList(); // flatten tree of scan nodes to prepare for listing

            var stars = 0;
            var planets = 0;
            var terrestrial = 0;
            var gasgiants = 0;
            var moons = 0;

            List<MaterialCommodityMicroResource> historicmcl = discoveryform.history.MaterialCommoditiesMicroResources.Get(last_he.MaterialCommodity);
            List<MaterialCommodityMicroResource> curmcl = discoveryform.history.MaterialCommoditiesMicroResources.GetLast();

            HashSet<string> jumponiums = new HashSet<string>();

            foreach (StarScan.ScanNode sn in all_nodes)
            {
                // define strings to be populated
                var bdClass = new StringBuilder();
                var bdDist = new StringBuilder();
                var bdDetails = new StringBuilder();

                if (sn.NodeType == StarScan.ScanNodeType.ring)
                {
                    // do nothing, by now
                }
                else if (sn.NodeType == StarScan.ScanNodeType.beltcluster )
                {
                    // if have a scan, we show belts, and its not edsm body, or getting edsm
                    if (sn.ScanData?.BodyName != null && IsSet(CtrlList.showBelts) && (!sn.ScanData.IsEDSMBody || checkBoxEDSM.Checked))
                    {
                        bdClass.Clear();
                        bdClass.Append("Belt Cluster");

                        if (sn.ScanData.ScanType == "Detailed")
                        {
                            bdDetails.Append("Scanned");
                        }
                        else
                        {
                            bdDetails.Append("No scan data available");
                        }

                        if (Math.Abs(sn.ScanData.DistanceFromArrivalLS) > 0)
                        {
                            bdDist.AppendFormat("{0:0.00}AU ({1:0.0}ls)", sn.ScanData.DistanceFromArrivalLS / JournalScan.oneAU_LS, sn.ScanData.DistanceFromArrivalLS);
                        }

                        var img = global::EDDiscovery.Icons.Controls.Belt;

                        dataGridViewScangrid.Rows.Add(new object[] { null, sn.ScanData.BodyDesignationOrName, bdClass, bdDist, bdDetails });

                        var cur = dataGridViewScangrid.Rows[dataGridViewScangrid.Rows.Count - 1];

                        cur.Tag = img;
                        cur.Cells[0].Tag = null;
                        cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
                                    sn.ScanData.DisplayString(0, historicmcl, curmcl);
                    }
                }
                // must have scan data and a name to be good, and either not edsm body or edsm check
                else if (sn.ScanData?.BodyName != null && (!sn.ScanData.IsEDSMBody || checkBoxEDSM.Checked))     
                { 
                    var overlays = new StarColumnOverlays();

                    if (sn.ScanData.IsStar)
                    {
                        // is a star, so populate its information field with relevant data
                        stars++;

                        // star class
                        if (sn.ScanData.StarTypeText != null)
                            bdClass.Append(sn.ScanData.StarTypeText);

                        // is the main star?
                        if (sn.ScanData.BodyName.EndsWith(" A", StringComparison.Ordinal))
                        {
                            bdDist.AppendFormat("Main Star".T(EDTx.UserControlScanGrid_MainStar));
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
                            bdDetails.Append("Mass".T(EDTx.UserControlScanGrid_Mass)).Append(": ").Append(sn.ScanData.nStellarMass.Value.ToString("N2")).Append(", ");

                        // display stellar bodies radius in sols
                        if (sn.ScanData.nRadius.HasValue)
                            bdDetails.Append("Radius".T(EDTx.UserControlScanGrid_Radius)).Append(": ").Append((sn.ScanData.nRadius.Value / JournalScan.oneSolRadius_m).ToString("N2")).Append(", ");

                        // show the temperature
                        if (sn.ScanData.nSurfaceTemperature.HasValue)
                            bdDetails.Append("Temperature".T(EDTx.UserControlScanGrid_Temperature)).Append(": ").Append((sn.ScanData.nSurfaceTemperature.Value)).Append("K.");

                        // habitable zone for stars - do not display for black holes.
                        if (sn.ScanData.StarTypeID != EDStar.H)
                        {
                            if (IsSet(CtrlList.showHabitable))
                            {
                                string hz = sn.ScanData.CircumstellarZonesString(false, JournalScan.CZPrint.CZHab);
                                bdDetails.AppendFormat(Environment.NewLine + hz);
                            }
                            if (IsSet(CtrlList.showMetalRich))
                            {
                                string hz = sn.ScanData.CircumstellarZonesString(false, JournalScan.CZPrint.CZMR);
                                bdDetails.AppendFormat(Environment.NewLine + hz);
                            }
                            if (IsSet(CtrlList.showWaterWorlds))
                            {
                                string hz = sn.ScanData.CircumstellarZonesString(false, JournalScan.CZPrint.CZWW);
                                bdDetails.AppendFormat(Environment.NewLine + hz);
                            }
                            if (IsSet(CtrlList.showEarthLike))
                            {
                                string hz = sn.ScanData.CircumstellarZonesString(false, JournalScan.CZPrint.CZEL);
                                bdDetails.AppendFormat(Environment.NewLine + hz);
                            }
                            if (IsSet(CtrlList.showAmmonia))
                            {
                                string hz = sn.ScanData.CircumstellarZonesString(false, JournalScan.CZPrint.CZAW);
                                bdDetails.AppendFormat(Environment.NewLine + hz);
                            }
                            if (IsSet(CtrlList.showIcyBodies))
                            {
                                string hz = sn.ScanData.CircumstellarZonesString(false, JournalScan.CZPrint.CZIP);
                                bdDetails.AppendFormat(Environment.NewLine + hz);
                            }
                        }
                    }
                    else
                    {
                        // is a non-stellar body                        

                        // is terraformable? If so, prepend it to the body class
                        if (sn.ScanData.Terraformable)
                            bdClass.Append("Terraformable".T(EDTx.UserControlScanGrid_Terraformable)).Append(", ");

                        if (sn.NodeType == StarScan.ScanNodeType.body)      // Planet, not barycenter/belt
                        {
                            bdClass.Append(sn.ScanData.PlanetTypeText);

                            if (sn.Level <= 1)      // top level planet
                            {
                                planets++;

                                if (sn.ScanData.GasWorld)
                                    gasgiants++;
                                else
                                    terrestrial++;

                                bdDist.AppendFormat("{0:0.00}AU ({1:0.0}ls)", sn.ScanData.DistanceFromArrivalLS / JournalScan.oneAU_LS, sn.ScanData.DistanceFromArrivalLS);
                            }
                            else
                            {
                                moons++;

                                bdClass.Append(" ").Append("Moon".T(EDTx.UserControlScanGrid_Moon));

                                // moon distances from center body are measured from in SemiMajorAxis
                                if (sn.ScanData.nSemiMajorAxis.HasValue)
                                    bdDist.AppendFormat("{0:0.0}ls ({1:0}km)", sn.ScanData.nSemiMajorAxis.Value / JournalScan.oneLS_m, sn.ScanData.nSemiMajorAxis.Value / 1000);
                            }
                        }

                        // Details

                        // display non-stellar bodies radius in earth radiuses
                        if (sn.ScanData.nRadius.HasValue)
                        {
                            bdDetails.Append("Radius".T(EDTx.UserControlScanGrid_Radius)).Append(": ").Append((sn.ScanData.nRadius.Value/1000.0).ToString("N0") + "km (").
                                Append((sn.ScanData.nRadius.Value / JournalScan.oneEarthRadius_m).ToString("N2")).Append("ER), ");
                        }

                        // show the temperature, both in K and C degrees
                        if (sn.ScanData.nSurfaceTemperature.HasValue)
                            bdDetails.Append("Temperature".T(EDTx.UserControlScanGrid_Temperature)).Append(": ").Append((sn.ScanData.nSurfaceTemperature.Value).ToString("N2")).Append("K, (").Append((sn.ScanData.nSurfaceTemperature.Value - 273).ToString("N2")).Append("C).");

                        // print the main atmospheric composition and pressure, if presents
                        if (!String.IsNullOrEmpty(sn.ScanData.Atmosphere) && sn.ScanData.Atmosphere != "None")
                        {
                            bdDetails.Append(Environment.NewLine).Append(sn.ScanData.Atmosphere);
                            if (sn.ScanData.nSurfacePressure.HasValue)
                                bdDetails.Append(", ").Append((sn.ScanData.nSurfacePressure.Value / JournalScan.oneAtmosphere_Pa).ToString("N3")).Append("Pa.");
                        }

                        // tell us that a bodie is landable, and shows its gravity
                        if (sn.ScanData.IsLandable)
                        {
                            var Gg = "";

                            if (sn.ScanData.nSurfaceGravity.HasValue)
                            {
                                var g = sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2;
                                Gg = " (G: " + g.Value.ToString("N1") + ")";
                            }

                            bdDetails.Append(Environment.NewLine).Append("Landable".T(EDTx.UserControlScanGrid_Landable)).Append(Gg).Append(". ");
                            overlays.landable = true;
                        }

                        // tell us that there is some volcanic activity
                        if (sn.ScanData.Volcanism.HasChars())
                        {
                            bdDetails.Append(Environment.NewLine).Append("Geological activity".T(EDTx.UserControlScanGrid_Geologicalactivity)).Append(": ").Append(sn.ScanData.Volcanism).Append(". ");
                            overlays.volcanism = true;
                        }

                        if (sn.ScanData.Mapped)
                        {
                            bdDetails.Append(Environment.NewLine).Append("Surface mapped".T(EDTx.UserControlScanGrid_Surfacemapped)).Append(". ");
                            overlays.mapped = true;
                        }

                        if ( sn.Organics != null )
                        {
                            string ol = JournalScanOrganic.OrganicList(sn.Organics);
                            bdDetails.Append(Environment.NewLine).Append(ol);
                        }
                            
                        // materials                        
                        if (sn.ScanData.HasMaterials )
                        {
                            var ret = "";
                            foreach (KeyValuePair<string, double> mat in sn.ScanData.Materials)
                            {
                                var mc = MaterialCommodityMicroResourceType.GetByFDName(mat.Key);
                                if (mc?.IsJumponium == true)
                                {
                                    ret = ret.AppendPrePad(mc.Name, ", ");
                                    overlays.materials = true;
                                }
                            }

                            if (ret.Length > 0 && IsSet(CtrlList.showMaterials))
                            {
                                bdDetails.Append(Environment.NewLine).Append("This body contains: ".T(EDTx.UserControlScanGrid_BC)).Append(ret);
                            }
                        }
                    }

                    // have some belt, ring or other special structure?
                    if (sn.ScanData.HasRings)
                    {
                        for (int r = 0; r < sn.ScanData.Rings.Length; r++)
                        {
                            if (sn.ScanData.Rings[r].Name.EndsWith("Belt", StringComparison.Ordinal))
                            {
                                if (IsSet(CtrlList.showBelts))
                                {
                                    // is a belt
                                    bdDetails.Append(Environment.NewLine).Append("Belt: ".T(EDTx.UserControlScanGrid_Belt));
                                    var RingName = sn.ScanData.Rings[r].Name;
                                    bdDetails.Append(JournalScan.StarPlanetRing.DisplayStringFromRingClass(sn.ScanData.Rings[r].RingClass)).Append(" ");
                                    bdDetails.Append((sn.ScanData.Rings[r].InnerRad / JournalScan.oneLS_m).ToString("N2")).Append("ls to ").Append((sn.ScanData.Rings[r].OuterRad / JournalScan.oneLS_m).ToString("N2")).Append("ls. ");
                                }
                            }
                            else
                            {
                                if (IsSet(CtrlList.showRings))
                                {
                                    // is a ring
                                    bdDetails.Append(Environment.NewLine).Append("Ring: ".T(EDTx.UserControlScanGrid_Ring));
                                    var RingName = sn.ScanData.Rings[r].Name;
                                    bdDetails.Append(JournalScan.StarPlanetRing.DisplayStringFromRingClass(sn.ScanData.Rings[r].RingClass)).Append(" ");
                                    bdDetails.Append((sn.ScanData.Rings[r].InnerRad / JournalScan.oneLS_m).ToString("N2")).Append("ls to ").Append((sn.ScanData.Rings[r].OuterRad / JournalScan.oneLS_m).ToString("N2")).Append("ls. ");
                                }
                            }
                        }
                    }

                    // give estimated value
                    if (IsSet(CtrlList.showValues))
                    {
                        var value = sn.ScanData.EstimatedValue;
                        bdDetails.Append(Environment.NewLine).Append("Value".T(EDTx.UserControlScanGrid_Value)).Append(" ").Append(value.ToString("N0"));
                    }

                    if ( sn.ScanData.IsEDSMBody)
                        bdDetails.Append(Environment.NewLine).Append("EDSM");

                    // pick an image

                    Bitmap img = (Bitmap)BaseUtils.Icons.IconSet.GetIcon(sn.ScanData.GetStarPlanetTypeImageName());

                    dataGridViewScangrid.Rows.Add(new object[] { null, sn.ScanData.BodyDesignationOrName, bdClass, bdDist, bdDetails });

                    var cur = dataGridViewScangrid.Rows[dataGridViewScangrid.Rows.Count - 1];

                    cur.Tag = img;

                    cur.Cells[0].Tag = overlays;
                    cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
                                sn.ScanData.DisplayString(0, historicmcl, curmcl);

                    sn.ScanData.Jumponium(jumponiums);      // add to jumponiums hash any seen
                }
            }

            toolStripJumponiumProgressBar.Value = jumponiums.Count;
            toolStripJumponiumProgressBar.Visible = toolStripJumponiumProgressBar.Value > 0;

            //System.Diagnostics.Debug.WriteLine("Jumponiums " + toolStripJumponiumProgressBar.Value + " " + toolStripJumponiumProgressBar.Visible);

            if (toolStripJumponiumProgressBar.Value == 8)
                toolStripJumponiumProgressBar.ToolTipText = "This is a green system, as it has all existing jumponium materials available!".T(EDTx.UserControlScanGrid_GS);
            else
                toolStripJumponiumProgressBar.ToolTipText = toolStripJumponiumProgressBar.Value + " jumponium materials found in system.".T(EDTx.UserControlScanGrid_JS);

            string report = string.Format("Scan Summary for {0}: {1} stars; {2} planets ({3} terrestrial, {4} gas giants), {5} moons".T(EDTx.UserControlScanGrid_ScanSummaryfor), scannode.System.Name, stars, planets, terrestrial, gasgiants, moons);
            report = "~" + scannode.ScanValue( checkBoxEDSM.Checked).ToString() + " cr " + report;
            SetControlText(scannode.System.Name);
            toolStripStatusTotalValue.Text = report;

            if (firstdisplayedrow >= 0 && firstdisplayedrow < dataGridViewScangrid.RowCount)
                dataGridViewScangrid.SafeFirstDisplayedScrollingRowIndex(firstdisplayedrow);
        }

        #endregion

        private void dataGridViewScangrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var cur = dataGridViewScangrid.Rows[e.RowIndex];
            PaintHelpers.PaintStarColumn(dataGridViewScangrid, e, cur.Cells[0].Tag as StarColumnOverlays, colImage.Index, iconsize, bodysize);
        }

        #region UI
        void dataGridViewScangrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex >= 0)
            {
                var curdata = dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Value;
                dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Value = dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Tag;
                dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Tag = curdata;
            }
        }


        protected enum CtrlList
        {
            showHabitable,showMetalRich,showWaterWorlds,showEarthLike,showAmmonia,showIcyBodies,
            showBelts,showRings,showMaterials,showValues,
        };


        private bool[] ctrlset; // holds current state of each control above

        private bool IsSet(CtrlList v)
        {
            return ctrlset[(int)v];
        }

        private void PopulateCtrlList()
        {
            ctrlset = new bool[Enum.GetNames(typeof(CtrlList)).Length];
            foreach (CtrlList e in Enum.GetValues(typeof(CtrlList)))
            {
                bool def = true;
                var v = GetSetting(e.ToString(), def);
                //System.Diagnostics.Debug.WriteLine($"Surveyor {e.ToString()} = {v}");
                ctrlset[(int)e] = v;
            }
        }

        private string CtrlStateAsString()      // returns all controls in one string, note Show below does not care about the extras
        {
            string s = "";
            foreach (CtrlList v in Enum.GetValues(typeof(CtrlList)))
            {
                if (ctrlset[(int)v])
                    s += v.ToString() + ";";
            }

            return s;
        }
        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.showBelts.ToString(), "Show belts".TxID("UserControlScanGrid.structuresToolStripMenuItem.beltsToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showRings.ToString(), "Show rings".TxID("UserControlScanGrid.structuresToolStripMenuItem.ringsToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showMaterials.ToString(), "Show materials".TxID("UserControlScanGrid.materialsToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showValues.ToString(), "Show values".TxID("UserControlScanGrid.valuesToolStripMenuItem"));
            CommonCtrl(displayfilter, extButtonHabZones);
        }

        private void extButtonHabZones_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.showHabitable.ToString(), "Show Habitation Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showMetalRich.ToString(), "Show Metal Rich Planet Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showMetalRichPlanetsToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showWaterWorlds.ToString(), "Show Water World Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showWaterWorldsToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showEarthLike.ToString(), "Show Earth Like Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showEarthLikeToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showAmmonia.ToString(), "Show Ammonia Worlds Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showAmmoniaWorldsToolStripMenuItem"));
            displayfilter.AddStandardOption(CtrlList.showIcyBodies.ToString(), "Show Icy Planets Zone".TxID("UserControlSpanel.showCircumstellarZonesToolStripMenuItem.showIcyPlanetsToolStripMenuItem"));
            CommonCtrl(displayfilter, extButtonHabZones);
        }

        private void CommonCtrl(CheckedIconListBoxFormGroup displayfilter, Control button)
        {
            displayfilter.AllOrNoneBack = false;
            displayfilter.ScreenMargin = new Size(0, 0);

            displayfilter.SaveSettings = (s, o) =>
            {
                var ctrllist = displayfilter.SettingsTagList();
                PutBoolSettingsFromString(s, ctrllist);
                PopulateCtrlList();
                DrawSystem(last_he, true);
            };

            displayfilter.Show(CtrlStateAsString(), button, this.FindForm());
        }

        private void CheckBoxEDSM_Click(object sender, EventArgs e)
        {
            PutSetting("checkEDSM", checkBoxEDSM.Checked);
            DrawSystem(last_he, true);
        }

        #endregion

    }
}
