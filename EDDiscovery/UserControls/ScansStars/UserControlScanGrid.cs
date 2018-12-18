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

        private bool showStellarZones = true;
        private bool showHabitable = true;
        private bool showMetalRich = true;
        private bool showWaterWorlds = true;
        private bool showEarthLike = true;
        private bool showAmmonia = true;
        private bool showIcyBodies = true;
        private bool showStructures = true;
        private bool showBelts = true;
        private bool showRings = true;
        private bool showMaterials = true;
        private bool showValues = true;
        private bool isGreenSystem;
        private bool hasArsenic;
        private bool hasCadmium;
        private bool hasCarbon;
        private bool hasGermanium;
        private bool hasNiobium;
        private bool hasPopolonium;
        private bool hasVanadium;
        private bool hasYttrium;

        private string DbColumnSave { get { return DBName("ScanGridPanel", "DGVCol"); } }
        private string DbSave { get { return DBName("ScanGridPanel"); } }

        public UserControlScanGrid()
        {
            InitializeComponent();
            var corner = dataGridViewScangrid.TopLeftHeaderCell; // work around #1487

            // dataGridView setup - the rule is, use the designer for most properties.. only do these here since they are so buried or not available.

            // this allows the row to grow to accomodate the text.. with a min height of 32.
            dataGridViewScangrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewScangrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            dataGridViewScangrid.RowTemplate.MinimumHeight = 32;
            this.dataGridViewScangrid.Columns[nameof(colImage)].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
        }

        #region Init

        public override void Init()
        {
            discoveryform.OnNewEntry += NewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);

            // retrieve context menu entries check state from DB
            circumstellarZoneToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showStellarZones", true);
            habitableZoneToolStripMenuItem.Checked = SQLiteDBClass.PutSettingBool(DbSave + "showHabitable", true);
            metalRichToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showMetalRich", true);
            waterWorldsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showWaterWorlds", true);
            earthLikePlanetsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showEarthLike", true);
            ammoniaWorldsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showAmmonia", true);
            icyBodiesToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showIcyBodies", true);
            structuresToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showStructures", true);
            beltsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showBelts", true);
            ringsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showRings", true);
            materialsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showMaterials", true);
            valuesToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showValues", true);
            nameToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "colName", true);
            classToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "colClass", true);
            distanceToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "colDistance", true);
            informationToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "coldBriefing", true);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewScangrid, DbColumnSave);
        }

        /// <summary>
        /// Called when the cursor move to another system
        /// </summary>
        /// <param name="thc"></param>
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

        /// <summary>
        /// called when a new entry is made.. check to see if its a scan update
        /// </summary>
        /// <param name="he">HistoryEntry</param>
        /// <param name="hl">HistoryList</param>
        private void NewEntry(HistoryEntry he, HistoryList hl)
        {
            DrawSystem(he, he.EntryType == JournalTypeEnum.Scan);
        }

        /// <summary>
        /// Called at first start or hooked to change cursor
        /// </summary>
        /// <param name="he">HistoryEntry</param>
        /// <param name="hl">HistoryList</param>
        private void Display(HistoryEntry he, HistoryList hl)
        {
            ResetDefaults();
            DrawSystem(he, false);
        }

        private void ResetDefaults()
        {
            // reset isGreenSystem tag
            isGreenSystem = false;


            // reset indicator for jumponium materials
            hasArsenic = false;
            hasCadmium = false;
            hasCarbon = false;
            hasGermanium = false;
            hasNiobium = false;
            hasPopolonium = false;
            hasVanadium = false;
            hasYttrium = false;

            // reset the progress bar value
            toolStripProgressBar.Value = 0;
            toolStripProgressBar.Visible = false;
            toolStripStatusGreen.Visible = false;
        }

        #endregion

        #region PopulateGrid

        /// <summary>
        /// Draw the system bodies
        /// </summary>
        /// <param name="he">HistoryEntry</param>
        /// <param name="force">Boolean</param>
        private void DrawSystem(HistoryEntry he, bool force)
        {
            StarScan.SystemNode scannode = null;

            var samesys = last_he?.System != null && he?.System != null && he.System.Name == last_he.System.Name;

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
            var firstdisplayedrow = (dataGridViewScangrid.RowCount > 0 && samesys) ? dataGridViewScangrid.FirstDisplayedScrollingRowIndex : -1;

            dataGridViewScangrid.Rows.Clear();

            var all_nodes = scannode.Bodies.ToList(); // flatten tree of scan nodes to prepare for listing

            var _stars = 0;
            var _planets = 0;
            var _terrestrial = 0;
            var _gasgiants = 0;
            var _moons = 0;

            foreach (StarScan.ScanNode sn in all_nodes)
            {
                // define strings to be populated
                var bdClass = new StringBuilder();
                var bdDist = new StringBuilder();
                var bdDetails = new StringBuilder();
                
                if (sn.type == StarScan.ScanNodeType.ring)
                {
                    // do nothing, by now
                }
                else if (sn.type == StarScan.ScanNodeType.beltcluster)
                {
                    if (showStructures && sn.ScanData?.BodyName != null)
                    {
                        if (showBelts)
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

                            var img = global::EDDiscovery.Icons.Controls.ScanGrid_Belt;

                            dataGridViewScangrid.Rows.Add(new object[] { null, sn.ScanData.BodyName, bdClass, bdDist, bdDetails });

                            var cur = dataGridViewScangrid.Rows[dataGridViewScangrid.Rows.Count - 1];

                            cur.Tag = img;

                            cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
                                        sn.ScanData.DisplayString(historicmatlist: last_he.MaterialCommodity, currentmatlist: discoveryform.history.GetLast?.MaterialCommodity);
                        }
                    }
                }
                else
                {
                    // check for null data
                    if (sn.ScanData?.BodyName != null)
                    {
                        // check for body class
                        if (sn.ScanData.IsStar)
                        {
                            // is a star, so populate its information field with relevant data
                            _stars++;

                            // star class
                            if (sn.ScanData.StarTypeText != null)
                                bdClass.Append(sn.ScanData.StarTypeText);

                            // is the main star?
                            if (sn.ScanData.BodyName.EndsWith(" A", StringComparison.Ordinal))
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
                                bdDetails.Append("Mass".Tx(this)).Append(": ").Append(sn.ScanData.nStellarMass.Value.ToString("N2")).Append(", ");

                            // display stellar bodies radius in sols
                            if (sn.ScanData.nRadius.HasValue)
                                bdDetails.Append("Radius".Tx(this)).Append(": ").Append((sn.ScanData.nRadius.Value / JournalScan.oneSolRadius_m).ToString("N2")).Append(", ");

                            // show the temperature
                            if (sn.ScanData.nSurfaceTemperature.HasValue)
                                bdDetails.Append("Temperature".Tx(this)).Append(": ").Append((sn.ScanData.nSurfaceTemperature.Value)).Append("K.");

                            // habitable zone for stars - do not display for black holes.
                            if (showStellarZones)
                            {
                                if (showHabitable && sn.ScanData.HabitableZoneInner != null && sn.ScanData.HabitableZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Habitable Zone".Tx(this) + ": {0}-{1}AU ({2}). ", (sn.ScanData.HabitableZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.HabitableZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetHabZoneStringLs());
                                }
                                if (showMetalRich && sn.ScanData.MetalRichZoneInner != null && sn.ScanData.MetalRichZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Metal Rich bodies".Tx(this) + ": {0}-{1}AU ({2}). ", (sn.ScanData.MetalRichZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.MetalRichZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetMetalRichZoneStringLs());
                                }
                                if (showWaterWorlds && sn.ScanData.WaterWrldZoneInner != null && sn.ScanData.WaterWrldZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Water worlds".Tx(this) + ": {0}-{1}AU ({2}). ", (sn.ScanData.WaterWrldZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.WaterWrldZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetWaterWorldZoneStringLs());
                                }
                                if (showEarthLike && sn.ScanData.EarthLikeZoneInner != null && sn.ScanData.EarthLikeZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Earth likes planets".Tx(this) + ": {0}-{1}AU ({2}). ", (sn.ScanData.EarthLikeZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.EarthLikeZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetEarthLikeZoneStringLs());
                                }
                                if (showAmmonia && sn.ScanData.AmmonWrldZoneInner != null && sn.ScanData.AmmonWrldZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Ammonia worlds".Tx(this) + ": {0}-{1}AU ({2}). ", (sn.ScanData.AmmonWrldZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.AmmonWrldZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetAmmoniaWorldZoneStringLs());
                                }
                                if (showIcyBodies && sn.ScanData.IcyPlanetZoneInner != null && sn.ScanData.IcyPlanetZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Habitable Zone".Tx(this) + ": {0}AU-{1} ({2}). ", (sn.ScanData.IcyPlanetZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.IcyPlanetZoneOuter), sn.ScanData.GetIcyPlanetsZoneStringLs());
                                }
                            }
                        }
                        else
                        {
                            // is a non-stellar body                        

                            // is terraformable? If so, prepend it to the body class
                            if (sn.ScanData.Terraformable)
                                bdClass.Append("Terraformable".Tx(this)).Append(", ");

                            // is a planet?...
                            if (sn.ScanData.PlanetClass != null)
                                bdClass.Append(sn.ScanData.PlanetClass);

                            _planets++;

                            // tell us the distance from the arrivals in both AU and LS
                            if (sn.level <= 1 && sn.type == StarScan.ScanNodeType.body)
                                bdDist.AppendFormat("{0:0.00}AU ({1:0.0}ls)", sn.ScanData.DistanceFromArrivalLS / JournalScan.oneAU_LS, sn.ScanData.DistanceFromArrivalLS);

                            // ...or a moon?
                            if (sn.level >= 2 && sn.type == StarScan.ScanNodeType.body)
                            {
                                if (sn.ScanData.PlanetClass != null)
                                    _moons++;

                                bdClass.Append(" ").Append("Moon".Tx(this));

                                // moon distances from center body are measured from in SemiMajorAxis
                                bdDist.AppendFormat("{0:0.0}ls ({1:0}km)", sn.ScanData.nSemiMajorAxis.Value / JournalScan.oneLS_m, sn.ScanData.nSemiMajorAxis.Value / 1000);
                            }

                            if (sn.ScanData.PlanetClass != null && sn.ScanData.PlanetClass.Contains("Giant"))
                                _gasgiants++;
                            else
                                _terrestrial++;

                            // Details

                            // display non-stellar bodies radius in earth radiuses
                            if (sn.ScanData.nRadius.HasValue)
                                bdDetails.Append("Radius".Tx(this)).Append(": ").Append((sn.ScanData.nRadius.Value / JournalScan.oneEarthRadius_m).ToString("N2")).Append(", ");

                            // show the temperature, both in K and C degrees
                            if (sn.ScanData.nSurfaceTemperature.HasValue)
                                bdDetails.Append("Temperature".Tx(this)).Append(": ").Append((sn.ScanData.nSurfaceTemperature.Value).ToString("N2")).Append("K, (").Append((sn.ScanData.nSurfaceTemperature.Value - 273).ToString("N2")).Append("C).");

                            // print the main atmospheric composition and pressure, if presents
                            if (sn.ScanData.Atmosphere != null && sn.ScanData.Atmosphere != "None")
                                bdDetails.Append(Environment.NewLine).Append(sn.ScanData.Atmosphere).Append(", ").Append((sn.ScanData.nSurfacePressure.Value / JournalScan.oneAtmosphere_Pa).ToString("N3")).Append("Pa.");

                            // tell us that a bodie is landable, and shows its gravity
                            if (sn.ScanData.IsLandable)
                            {
                                var Gg = "";

                                if (sn.ScanData.nSurfaceGravity.HasValue)
                                {
                                    var g = sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2;
                                    Gg = " (G: " + g.Value.ToString("N1") + ")";
                                }

                                bdDetails.Append(Environment.NewLine).Append("Landable".Tx(this)).Append(Gg).Append(". ");
                            }

                            // tell us that there is some volcanic activity
                            if (sn.ScanData.Volcanism != null)
                                bdDetails.Append(Environment.NewLine).Append("Volcanic activity".Tx(this)).Append(". ");

                            // materials                        
                            if (sn.ScanData.HasMaterials)
                            {
                                toolStripProgressBar.Visible = true;

                                var ret = "";
                                foreach (KeyValuePair<string, double> mat in sn.ScanData.Materials)
                                {
                                    var mc = MaterialCommodityData.GetByFDName(mat.Key);
                                    if (mc?.IsJumponium == true)
                                        ret = ret.AppendPrePad(mc.Name, ", ");
                                }

                                if (ret.Length > 0 && showMaterials)
                                    bdDetails.Append(Environment.NewLine).Append("This body contains: ".Tx(this, "BC")).Append(ret);

                                ReportJumponium(ret);
                            }
                        }

                        // have some belt, ring or other special structure?
                        if (showStructures)
                        {
                            if (sn.ScanData.HasRings)
                            {
                                for (int r = 0; r < sn.ScanData.Rings.Length; r++)
                                {
                                    if (sn.ScanData.Rings[r].Name.EndsWith("Belt", StringComparison.Ordinal))
                                    {
                                        if (showBelts)
                                        {
                                            // is a belt
                                            bdDetails.Append(Environment.NewLine).Append("Belt: ".Tx(this, "Belt"));
                                            var RingName = sn.ScanData.Rings[r].Name;
                                            bdDetails.Append(JournalScan.StarPlanetRing.DisplayStringFromRingClass(sn.ScanData.Rings[r].RingClass)).Append(" ");
                                            bdDetails.Append((sn.ScanData.Rings[r].InnerRad / JournalScan.oneLS_m).ToString("N2")).Append("ls to ").Append((sn.ScanData.Rings[r].OuterRad / JournalScan.oneLS_m).ToString("N2")).Append("ls. ");
                                        }
                                    }
                                    else
                                    {
                                        if (showRings)
                                        {
                                            // is a ring
                                            bdDetails.Append(Environment.NewLine).Append("Ring: ".Tx(this, "Ring"));
                                            var RingName = sn.ScanData.Rings[r].Name;
                                            bdDetails.Append(JournalScan.StarPlanetRing.DisplayStringFromRingClass(sn.ScanData.Rings[r].RingClass)).Append(" ");
                                            bdDetails.Append((sn.ScanData.Rings[r].InnerRad / JournalScan.oneLS_m).ToString("N2")).Append("ls to ").Append((sn.ScanData.Rings[r].OuterRad / JournalScan.oneLS_m).ToString("N2")).Append("ls. ");
                                        }
                                    }
                                }
                            }
                        }

                        //! for all relevant bodies:

                        // give estimated value
                        var value = sn.ScanData.EstimatedValue;
                        if (showValues)
                        {
                            bdDetails.Append(Environment.NewLine).Append("Value".Tx(this)).Append(" ").Append(value.ToString("N0"));
                        }

                        // pick an image
                        var img = sn.ScanData.IsStar ? sn.ScanData.GetStarTypeImage() : sn.ScanData.GetPlanetClassImage();

                        dataGridViewScangrid.Rows.Add(new object[] { null, sn.ScanData.BodyName, bdClass, bdDist, bdDetails });

                        var cur = dataGridViewScangrid.Rows[dataGridViewScangrid.Rows.Count - 1];

                        cur.Tag = img;

                        cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
                                    sn.ScanData.DisplayString(historicmatlist: last_he.MaterialCommodity, currentmatlist: discoveryform.history.GetLast?.MaterialCommodity);
                    }
                }

                // check if it's a green system
                isGreenSystem |= (hasArsenic && hasCadmium && hasCarbon && hasGermanium && hasNiobium && hasPopolonium && hasVanadium && hasYttrium);

                if (isGreenSystem)
                {
                    toolStripProgressBar.ToolTipText = "This is a green system, as it has all existing jumponium materials available!";
                    toolStripStatusGreen.Visible = true;
                }
                else if (!isGreenSystem)
                {
                    toolStripProgressBar.ToolTipText = toolStripProgressBar.Value + " jumponium materials found in system.";
                }

                // set a meaningful title for the controller            
                SetControlText(string.Format("Scan Summary for {0}: {1} stars; {2} planets ({3} terrestrial, {4} gas giants), {5} moons".Tx(this), scannode.system.Name, _stars, _planets, _terrestrial, _gasgiants, _moons));
                if (firstdisplayedrow >= 0 && firstdisplayedrow < dataGridViewScangrid.RowCount)
                    dataGridViewScangrid.FirstDisplayedScrollingRowIndex = firstdisplayedrow;

                toolStripStatusTotalValue.Text = BuildScanValue(scannode);
            }
        }

        #endregion

        #region global_functions
                
        private void ReportJumponium(string ret)
        {
            if (!hasArsenic && ret.Contains("Arsenic"))
            {
                toolStripProgressBar.Value += 1;
                hasArsenic = ret.Contains("Arsenic");
            }

            if (!hasCadmium && ret.Contains("Cadmium"))
            {
                toolStripProgressBar.Value += 1;
                hasCadmium = ret.Contains("Cadmium");
            }

            if (!hasCarbon && ret.Contains("Carbon"))
            {
                toolStripProgressBar.Value += 1;
                hasCarbon = ret.Contains("Carbon");
            }

            if (!hasGermanium && ret.Contains("Germanium"))
            {
                toolStripProgressBar.Value += 1;
                hasGermanium = ret.Contains("Germanium");
            }

            if (hasNiobium && ret.Contains("Niobium"))
            {
                toolStripProgressBar.Value += 1;
                hasNiobium = ret.Contains("Niobium");
            }

            if (!hasPopolonium && ret.Contains("Polonium"))
            {
                toolStripProgressBar.Value += 1;
                hasPopolonium = ret.Contains("Polonium");
            }

            if (!hasVanadium && ret.Contains("Vanadium"))
            {
                toolStripProgressBar.Value += 1;
                hasVanadium = ret.Contains("Vanadium");
            }

            if (!hasYttrium && ret.Contains("Yttrium"))
            {
                toolStripProgressBar.Value += 1;
                hasYttrium = ret.Contains("Yttrium");
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

        #endregion

        private void dataGridViewScangrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var cur = dataGridViewScangrid.Rows[e.RowIndex];
            if (cur.Tag != null)
            {
                // we programatically draw the image because we have control over its pos/ size this way, which you can't do
                // with a image column - there you can only draw a fixed image or stretch it to cell contents.. which we don't want to do
                var sz = dataGridViewScangrid.RowTemplate.MinimumHeight - 2;
                var vpos = e.RowBounds.Top + e.RowBounds.Height / 2 - sz / 2;
                e.Graphics.DrawImage((Image)cur.Tag, new Rectangle(e.RowBounds.Left + 1, vpos, sz, sz));
            }
        }

        #region ContextMenuInteraction

        void dataGridViewScangrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                var curdata = dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Value;
                dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Value = dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Tag;
                dataGridViewScangrid.Rows[e.RowIndex].Cells[4].Tag = curdata;
            }
        }

        void dataGridViewScangrid_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenuStrip1.Visible |= e.Button == MouseButtons.Right;
            contextMenuStrip1.Top = MousePosition.Y;
            contextMenuStrip1.Left = MousePosition.X;
        }

        void circumstellarZoneToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showStellarZones = circumstellarZoneToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showStellarZones", showStellarZones);
            DrawSystem(last_he, true);
        }

        private void habitableZoneToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showHabitable = habitableZoneToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showHabitable", showHabitable);
            DrawSystem(last_he, true);
        }

        private void metallicRichToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showMetalRich = metalRichToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showMetalRich", showMetalRich);
            DrawSystem(last_he, true);
        }

        private void waterWorldsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showWaterWorlds = waterWorldsToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showWaterWorlds", showWaterWorlds);
            DrawSystem(last_he, true);
        }

        private void earthLikePlanetsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showEarthLike = earthLikePlanetsToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showEarthLike", showEarthLike);
            DrawSystem(last_he, true);
        }

        private void ammoniaWorldsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showAmmonia = ammoniaWorldsToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showAmmonia", showAmmonia);
            DrawSystem(last_he, true);
        }

        private void icyBodiesToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showIcyBodies = icyBodiesToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showIcyBodies", showIcyBodies);
            DrawSystem(last_he, true);
        }

        private void structuresToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showStructures = structuresToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showStructures", showStructures);
            DrawSystem(last_he, true);
        }

        private void beltsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showBelts = beltsToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showBelts", showBelts);
            DrawSystem(last_he, true);
        }

        private void ringsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showRings = ringsToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showRings", showRings);
            DrawSystem(last_he, true);
        }

        private void materialsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showMaterials = materialsToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showMaterials", showMaterials);
            DrawSystem(last_he, true);
        }

        private void valuesToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showValues = valuesToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "showValues", showValues);
            DrawSystem(last_he, true);
        }
        
        private void nameToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            colName.Visible = nameToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "colName", colName.Visible);
            DrawSystem(last_he, true);
        }

        private void classToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            colClass.Visible = classToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "colClass", colClass.Visible);
            DrawSystem(last_he, true);
        }

        private void distanceToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            colDistance.Visible = distanceToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "colDistance", colDistance.Visible);
            DrawSystem(last_he, true);
        }

        private void informationToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            colBriefing.Visible = informationToolStripMenuItem.Checked;
            SQLiteDBClass.PutSettingBool(DbSave + "colBriefing", colBriefing.Visible);
            DrawSystem(last_he, true);
        }

        #endregion
    }
}
