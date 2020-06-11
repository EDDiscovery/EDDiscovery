/*
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
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

        int iconsize;   // computed icon and body sizes
        int bodysize;

        private string DbColumnSave { get { return DBName("ScanGridPanel", "DGVCol"); } }
        private string DbSave { get { return DBName("ScanGridPanel"); } }

        public UserControlScanGrid()
        {
            InitializeComponent();
            var corner = dataGridViewScangrid.TopLeftHeaderCell; // work around #1487
            // this allows the row to grow to accomodate the text.. with a min height of 48px.
            dataGridViewScangrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewScangrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            this.dataGridViewScangrid.Columns[nameof(colImage)].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
        }

        #region Init

        public override void Init()
        {
            discoveryform.OnNewEntry += NewEntry;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);

            // retrieve context menu entries check state from DB
            circumstellarZoneToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showStellarZones", true);
            habitableZoneToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showHabitable", true);
            metalRichToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showMetalRich", true);
            waterWorldsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showWaterWorlds", true);
            earthLikePlanetsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showEarthLike", true);
            ammoniaWorldsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showAmmonia", true);
            icyBodiesToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showIcyBodies", true);
            structuresToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showStructures", true);
            beltsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showBelts", true);
            ringsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showRings", true);
            materialsToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showMaterials", true);
            valuesToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "showValues", true);
            nameToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "colName", true);
            classToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "colClass", true);
            distanceToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "colDistance", true);
            informationToolStripMenuItem.Checked = EliteDangerousCore.DB.UserDatabase.Instance.GetSettingBool(DbSave + "coldBriefing", true);
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

        private void NewEntry(HistoryEntry he, HistoryList hl)
        {
            DrawSystem(he, he.EntryType == JournalTypeEnum.Scan);
        }

        private void Display(HistoryEntry he, HistoryList hl, bool selectedEntry)
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

        private class Overlays
        {
            public bool landable, materials, volcanism, mapped;     // all false on creation
        }

        private async void DrawSystem(HistoryEntry he, bool force)
        {
            StarScan.SystemNode scannode = null;

            var samesys = last_he?.System != null && he?.System != null && he.System.Name == last_he.System.Name;

            //System.Diagnostics.Debug.WriteLine("Scan grid " + samesys + " F:" + force);

            if (he == null)     //  no he, no display
            {
                last_he = he;
                dataGridViewScangrid.Rows.Clear();
                SetControlText("No Scan".T(EDTx.NoScan));
                return;
            }
            else
            {
                scannode = await discoveryform.history.starscan.FindSystemAsync(he.System, true);        // get data with EDSM

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
            var firstdisplayedrow = (dataGridViewScangrid.RowCount > 0 && samesys) ? dataGridViewScangrid.FirstDisplayedScrollingRowIndex : -1;

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
                            cur.Cells[0].Tag = null;
                            cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
                                        sn.ScanData.DisplayString(historicmatlist: last_he.MaterialCommodity, currentmatlist: discoveryform.history.GetLast?.MaterialCommodity);
                        }
                    }
                }
                else
                {
                    var overlays = new Overlays();
                    // check for null data

                    if (sn.ScanData?.BodyName != null)
                    {
                        // check for body class
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
                            if (showStellarZones)
                            {
                                if (showHabitable && sn.ScanData.HabitableZoneInner != null && sn.ScanData.HabitableZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Habitable Zone".T(EDTx.UserControlScanGrid_HabitableZone) + ": {0}-{1}AU ({2}). ", (sn.ScanData.HabitableZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.HabitableZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetHabZoneStringLs());
                                }
                                if (showMetalRich && sn.ScanData.MetalRichZoneInner != null && sn.ScanData.MetalRichZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Metal Rich bodies".T(EDTx.UserControlScanGrid_MetalRichbodies) + ": {0}-{1}AU ({2}). ", (sn.ScanData.MetalRichZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.MetalRichZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetMetalRichZoneStringLs());
                                }
                                if (showWaterWorlds && sn.ScanData.WaterWrldZoneInner != null && sn.ScanData.WaterWrldZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Water worlds".T(EDTx.UserControlScanGrid_Waterworlds) + ": {0}-{1}AU ({2}). ", (sn.ScanData.WaterWrldZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.WaterWrldZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetWaterWorldZoneStringLs());
                                }
                                if (showEarthLike && sn.ScanData.EarthLikeZoneInner != null && sn.ScanData.EarthLikeZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Earth likes planets".T(EDTx.UserControlScanGrid_Earthlikesplanets) + ": {0}-{1}AU ({2}). ", (sn.ScanData.EarthLikeZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.EarthLikeZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetEarthLikeZoneStringLs());
                                }
                                if (showAmmonia && sn.ScanData.AmmonWrldZoneInner != null && sn.ScanData.AmmonWrldZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Ammonia worlds".T(EDTx.UserControlScanGrid_Ammoniaworlds) + ": {0}-{1}AU ({2}). ", (sn.ScanData.AmmonWrldZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.AmmonWrldZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetAmmoniaWorldZoneStringLs());
                                }
                                if (showIcyBodies && sn.ScanData.IcyPlanetZoneInner != null && sn.ScanData.IcyPlanetZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                                {
                                    bdDetails.AppendFormat(Environment.NewLine + "Habitable Zone".T(EDTx.UserControlScanGrid_HabitableZone) + ": {0}AU-{1} ({2}). ", (sn.ScanData.IcyPlanetZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.IcyPlanetZoneOuter), sn.ScanData.GetIcyPlanetsZoneStringLs());
                                }
                            }
                        }
                        else
                        {
                            // is a non-stellar body                        

                            // is terraformable? If so, prepend it to the body class
                            if (sn.ScanData.Terraformable)
                                bdClass.Append("Terraformable".T(EDTx.UserControlScanGrid_Terraformable)).Append(", ");

                            // is a planet?...
                            if (sn.ScanData.PlanetClass != null)
                                bdClass.Append(sn.ScanData.PlanetClass);

                            planets++;

                            // tell us the distance from the arrivals in both AU and LS
                            if (sn.level <= 1 && sn.type == StarScan.ScanNodeType.body)
                                bdDist.AppendFormat("{0:0.00}AU ({1:0.0}ls)", sn.ScanData.DistanceFromArrivalLS / JournalScan.oneAU_LS, sn.ScanData.DistanceFromArrivalLS);

                            // ...or a moon?
                            if (sn.level >= 2 && sn.type == StarScan.ScanNodeType.body)
                            {
                                if (sn.ScanData.PlanetClass != null)
                                    moons++;

                                bdClass.Append(" ").Append("Moon".T(EDTx.UserControlScanGrid_Moon));

                                // moon distances from center body are measured from in SemiMajorAxis
                                if (sn.ScanData.nSemiMajorAxis.HasValue)
                                    bdDist.AppendFormat("{0:0.0}ls ({1:0}km)", sn.ScanData.nSemiMajorAxis.Value / JournalScan.oneLS_m, sn.ScanData.nSemiMajorAxis.Value / 1000);
                            }

                            if (sn.ScanData.PlanetClass != null && sn.ScanData.PlanetClass.Contains("Giant"))
                                gasgiants++;
                            else
                                terrestrial++;

                            // Details

                            // display non-stellar bodies radius in earth radiuses
                            if (sn.ScanData.nRadius.HasValue)
                                bdDetails.Append("Radius".T(EDTx.UserControlScanGrid_Radius)).Append(": ").Append((sn.ScanData.nRadius.Value / JournalScan.oneEarthRadius_m).ToString("N2")).Append(", ");

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
                            if (sn.ScanData.Volcanism != null)
                            {
                                bdDetails.Append(Environment.NewLine).Append("Geological activity".T(EDTx.UserControlScanGrid_Geologicalactivity)).Append(": ").Append(sn.ScanData.Volcanism).Append(". ");
                                overlays.volcanism = true;
                            }

                            if (sn.ScanData.Mapped)
                            {
                                bdDetails.Append(Environment.NewLine).Append("Surface mapped".T(EDTx.UserControlScanGrid_Surfacemapped)).Append(". ");
                                overlays.mapped = true;
                            }
                            
                            // materials                        
                            if (sn.ScanData.HasMaterials)
                            {
                                toolStripProgressBar.Visible = true;

                                var ret = "";
                                foreach (KeyValuePair<string, double> mat in sn.ScanData.Materials)
                                {
                                    var mc = MaterialCommodityData.GetByFDName(mat.Key);
                                    if (mc?.IsJumponium == true)
                                    {
                                        ret = ret.AppendPrePad(mc.Name, ", ");
                                        overlays.materials = true;
                                    }
                                }

                                if (ret.Length > 0 && showMaterials)
                                {
                                    bdDetails.Append(Environment.NewLine).Append("This body contains: ".T(EDTx.UserControlScanGrid_BC)).Append(ret);
                                }
                                                                
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
                                            bdDetails.Append(Environment.NewLine).Append("Belt: ".T(EDTx.UserControlScanGrid_Belt));
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
                                            bdDetails.Append(Environment.NewLine).Append("Ring: ".T(EDTx.UserControlScanGrid_Ring));
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
                        if (showValues)
                        {
                            var value = sn.ScanData.EstimatedValue;
                            bdDetails.Append(Environment.NewLine).Append("Value".T(EDTx.UserControlScanGrid_Value)).Append(" ").Append(value.ToString("N0"));
                        }

                        // pick an image
                        var img = sn.ScanData.IsStar ? sn.ScanData.GetStarTypeImage() : sn.ScanData.GetPlanetClassImage();

                        dataGridViewScangrid.Rows.Add(new object[] { null, sn.ScanData.BodyName, bdClass, bdDist, bdDetails });

                        var cur = dataGridViewScangrid.Rows[dataGridViewScangrid.Rows.Count - 1];

                        cur.Tag = img;

                        cur.Cells[0].Tag = overlays;
                        cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
                                    sn.ScanData.DisplayString(historicmatlist: last_he.MaterialCommodity, currentmatlist: discoveryform.history.GetLast?.MaterialCommodity);
                    }
                }

                // check if it's a green system
                isGreenSystem |= (hasArsenic && hasCadmium && hasCarbon && hasGermanium && hasNiobium && hasPopolonium && hasVanadium && hasYttrium);

                if (isGreenSystem)
                {
                    toolStripProgressBar.ToolTipText = "This is a green system, as it has all existing jumponium materials available!".T(EDTx.UserControlScanGrid_GS);
                    toolStripStatusGreen.Visible = true;
                }
                else if (!isGreenSystem)
                {
                    toolStripProgressBar.ToolTipText = toolStripProgressBar.Value + " jumponium materials found in system.".T(EDTx.UserControlScanGrid_JS);
                }

                // set a meaningful title for the controller            
                SetControlText(string.Format("Scan Summary for {0}: {1} stars; {2} planets ({3} terrestrial, {4} gas giants), {5} moons".T(EDTx.UserControlScanGrid_ScanSummaryfor), scannode.system.Name, stars, planets, terrestrial, gasgiants, moons));
                if (firstdisplayedrow >= 0 && firstdisplayedrow < dataGridViewScangrid.RowCount)
                    dataGridViewScangrid.SafeFirstDisplayedScrollingRowIndex(firstdisplayedrow);

                toolStripStatusTotalValue.Text = "~"+ scannode.ScanValue(true).ToString() + " cr";
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


        #endregion

        private void dataGridViewScangrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var cur = dataGridViewScangrid.Rows[e.RowIndex];
            Overlays overlays = cur.Cells[0].Tag as Overlays;       // may be null

            if (cur.Tag != null)
            {
                // we programatically draw the image because we have control over its pos/ size this way, which you can't do
                // with a image column - there you can only draw a fixed image or stretch it to cell contents.. which we don't want to do

                int vmid = (e.RowBounds.Top + e.RowBounds.Bottom) / 2;
                int hmid = e.RowBounds.Left + cur.Cells[0].Size.Width / 2;

                int icons = 0;

                if (overlays != null)
                    icons = (overlays.mapped ? 1 : 0) + (overlays.volcanism ? 1 : 0) + (overlays.materials ? 1 : 0) + (overlays.landable ? 1 : 0);

                int iconspacing = Font.ScalePixels(2);

                Image img = cur.Tag as Image;
                Rectangle body = new Rectangle(hmid - (iconsize + bodysize + iconspacing) / 2, vmid - bodysize / 2, bodysize, bodysize);
                e.Graphics.DrawImage(img, body);        // main icon

                int right = body.Left + bodysize + iconspacing;
                int vposoverlay = vmid - iconsize * icons / 2;                       // position it centrally vertically

                if (overlays?.landable ?? false)
                {
                    e.Graphics.DrawImage((Image)EDDiscovery.Icons.Controls.Scan_Bodies_Landable, new Rectangle(right, vposoverlay, iconsize, iconsize));
                    vposoverlay += iconsize + iconspacing;
                }

                if (overlays?.materials ?? false)
                {
                    e.Graphics.DrawImage((Image)EDDiscovery.Icons.Controls.Scan_ShowAllMaterials, new Rectangle(right, vposoverlay, iconsize, iconsize));
                    vposoverlay += iconsize + iconspacing;
                }

                if (overlays?.volcanism ?? false)
                {
                    e.Graphics.DrawImage((Image)EDDiscovery.Icons.Controls.Scan_Bodies_Volcanism, new Rectangle(right, vposoverlay, iconsize, iconsize));
                    vposoverlay += iconsize + iconspacing;
                }

                if (overlays?.mapped ?? false)
                {
                    e.Graphics.DrawImage((Image)EDDiscovery.Icons.Controls.Scan_Bodies_Mapped, new Rectangle(right, vposoverlay, iconsize, iconsize));
                }
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
            contextMenuStrip.Visible |= e.Button == MouseButtons.Right;
            contextMenuStrip.Top = MousePosition.Y;
            contextMenuStrip.Left = MousePosition.X;
        }

        void circumstellarZoneToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showStellarZones = circumstellarZoneToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showStellarZones", showStellarZones);
            DrawSystem(last_he, true);
        }

        private void habitableZoneToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showHabitable = habitableZoneToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showHabitable", showHabitable);
            DrawSystem(last_he, true);
        }

        private void metallicRichToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showMetalRich = metalRichToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showMetalRich", showMetalRich);
            DrawSystem(last_he, true);
        }

        private void waterWorldsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showWaterWorlds = waterWorldsToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showWaterWorlds", showWaterWorlds);
            DrawSystem(last_he, true);
        }

        private void earthLikePlanetsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showEarthLike = earthLikePlanetsToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showEarthLike", showEarthLike);
            DrawSystem(last_he, true);
        }

        private void ammoniaWorldsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showAmmonia = ammoniaWorldsToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showAmmonia", showAmmonia);
            DrawSystem(last_he, true);
        }

        private void icyBodiesToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showIcyBodies = icyBodiesToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showIcyBodies", showIcyBodies);
            DrawSystem(last_he, true);
        }

        private void structuresToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showStructures = structuresToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showStructures", showStructures);
            DrawSystem(last_he, true);
        }

        private void beltsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showBelts = beltsToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showBelts", showBelts);
            DrawSystem(last_he, true);
        }

        private void ringsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showRings = ringsToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showRings", showRings);
            DrawSystem(last_he, true);
        }

        private void materialsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showMaterials = materialsToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showMaterials", showMaterials);
            DrawSystem(last_he, true);
        }

        private void valuesToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            showValues = valuesToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "showValues", showValues);
            DrawSystem(last_he, true);
        }
        
        private void nameToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            colName.Visible = nameToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "colName", colName.Visible);
            DrawSystem(last_he, true);
        }

        private void classToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            colClass.Visible = classToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "colClass", colClass.Visible);
            DrawSystem(last_he, true);
        }

        private void distanceToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            colDistance.Visible = distanceToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "colDistance", colDistance.Visible);
            DrawSystem(last_he, true);
        }

        private void informationToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            colBriefing.Visible = informationToolStripMenuItem.Checked;
            EliteDangerousCore.DB.UserDatabase.Instance.PutSettingBool(DbSave + "colBriefing", colBriefing.Visible);
            DrawSystem(last_he, true);
        }

        #endregion
    }
}
