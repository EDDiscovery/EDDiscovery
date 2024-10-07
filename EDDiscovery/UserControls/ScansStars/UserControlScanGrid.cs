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
 * 
 */

using EliteDangerousCore;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private string dbRolledUp = "RolledUp";
        const string dbValueLimit = "ValueLimit";

        public UserControlScanGrid()
        {
            InitializeComponent();

            // this allows the row to grow to accomodate the text.. with a min height of 48px.
            dataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            this.dataGridView.Columns[nameof(colImage)].DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Black;
        }

        #region Init

        public override void Init()
        {
            DBBaseName = "ScanGridPanel";

            var enumlist = new Enum[] { EDTx.UserControlScanGrid_colName, EDTx.UserControlScanGrid_colClass, EDTx.UserControlScanGrid_colDistance, EDTx.UserControlScanGrid_colBriefing,
            EDTx.UserControlScanGrid_ColCurValue, EDTx.UserControlScanGrid_ColMaxValue, EDTx.UserControlScanGrid_ColOrganics };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            var enumlisttt = new Enum[] { EDTx.UserControlScanGrid_extButtonShowControl_ToolTip, EDTx.UserControlScanGrid_extButtonHighValue_ToolTip };
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            rollUpPanelTop.SetToolTip(toolTip);

            edsmSpanshButton.Init(this, "EDSMSpansh", "");
            edsmSpanshButton.ValueChanged += (s, ch) =>
            {
                DrawSystem(last_he, true);
            };

            rollUpPanelTop.PinState = GetSetting(dbRolledUp, true);
            PopulateCtrlList();

            toolStripJumponiumProgressBar.Visible = false;

        }

        public override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            PutSetting(dbRolledUp, rollUpPanelTop.PinState);
        }

        public override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)        //we track the history cursor
        {
            DrawSystem(he);
        }

        #endregion

        #region PopulateGrid

        private async void DrawSystem(HistoryEntry he, bool redrawall = false)
        {
            StarScan.SystemNode systemnode = null;

            var samesys = last_he?.System != null && he?.System != null && he.System.Name == last_he.System.Name;       // is it the same system

            bool scanupdate = he.journalEntry is IStarScan;      // possible update to data..

            System.Diagnostics.Debug.WriteLine($"Scan grid @{he?.EventTimeUTC} {he?.EventSummary} Samesync {samesys} force {redrawall} istarscan {scanupdate}");

            if (he == null || he.System.Name == "Unknown" || (samesys && !redrawall && !scanupdate))      // no he, or unknown system, or same system with no force, no redisplay
                return;

            last_he = he;   // record the he to note the last system visited

            systemnode = await DiscoveryForm.History.StarScan.FindSystemAsync(he.System, edsmSpanshButton.WebLookup);        // get data with EDSM maybe

            if (IsClosed)
                return;

            if (systemnode == null)     // no data, clear display, clear any last_he so samesys is false next time
            {
                System.Diagnostics.Debug.WriteLine($"Scan grid scan node not found");
                last_he = null;
                dataGridView.Rows.Clear();
                toolStripJumponiumProgressBar.Visible = false;
                SetControlText("No Scan".T(EDTx.NoScan));
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Scan grid update display at {he.System.Name} found with {systemnode.Bodies.Count()} bodies");

            if (redrawall || !samesys)
            {
                System.Diagnostics.Debug.WriteLine($"Scan grid clear grid - samesys {samesys} redisplay all {redrawall}");
                dataGridView.Rows.Clear();
            }

            // only record first row if same system 
            //var firstdisplayedrow = (dataGridView.RowCount > 0 && samesys) ? dataGridView.SafeFirstDisplayedScrollingRowIndex() : -1;
                //            if (firstdisplayedrow >= 0 && firstdisplayedrow < dataGridView.RowCount) // not sure if needed
                //              dataGridView.SafeFirstDisplayedScrollingRowIndex(firstdisplayedrow);


            var stars = 0;      // count entities
            var planets = 0;
            var terrestrial = 0;
            var gasgiants = 0;
            var moons = 0;

            List<MaterialCommodityMicroResource> historicmcl = DiscoveryForm.History.MaterialCommoditiesMicroResources.Get(he.MaterialCommodity);
            List<MaterialCommodityMicroResource> curmcl = DiscoveryForm.History.MaterialCommoditiesMicroResources.GetLast();

            HashSet<string> jumponiums = new HashSet<string>();     // accumulate jumponium in a hash set to prevent repeats

            SystemDisplay sd = new SystemDisplay();                 // drawing planet image class
            sd.ValueLimit = GetSetting(dbValueLimit, 50000);
            sd.Font = Theme.Current.GetFont;
            Size imagesize = new Size(48, 48);

            long organicvaluetotal = 0;     // total of organic values

            var bodynodes = systemnode.Bodies.ToList(); // flatten tree of scan nodes to prepare for listing
            foreach (StarScan.ScanNode sn in bodynodes)
            {
                // define strings to be populated
                var bdClass = new StringBuilder();
                var bdDist = new StringBuilder();
                var bdDetails = new StringBuilder();
                string[] texttoadd = null;     // items to add to row
                List<ExtPictureBox.ImageElement> pc = new List<ExtPictureBox.ImageElement>();

                long organicssystemvalue = 0;

                if (sn.NodeType == StarScan.ScanNodeType.ring || sn.NodeType == StarScan.ScanNodeType.belt || sn.NodeType == StarScan.ScanNodeType.barycentre)
                {
                    // do nothing
                }
                else if (sn.NodeType == StarScan.ScanNodeType.beltcluster)
                {
                    //if have a scan, we show belts, and its not edsm body, or getting edsm
                    if (sn.ScanData?.BodyName != null && IsSet(CtrlList.showBelts) && (!sn.ScanData.IsWebSourced || edsmSpanshButton.IsAnySet))
                    {
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
                            bdDist.AppendFormat("{0:N2} AU ({1:N1} ls)", sn.ScanData.DistanceFromArrivalLS / BodyPhysicalConstants.oneAU_LS, sn.ScanData.DistanceFromArrivalLS);
                        }

                        texttoadd = new string[] { sn.ScanData.BodyDesignationOrName, bdClass.ToString(), bdDist.ToString(), bdDetails.ToString() };

                        pc.Add(new ExtPictureBox.ImageElement(new Rectangle(0, 0, imagesize.Width, imagesize.Height), BodyToImages.GetBeltImage(),
                                    imgowned:false)); // NOTE the picture box does not own the image
                    }
                }
                // must have scan data and either not edsm body or edsm check
                else if (sn.ScanData != null && (!sn.ScanData.IsWebSourced || edsmSpanshButton.IsAnySet))
                {
                    if (sn.ScanData.IsStar)
                    {
                        // is a star, so populate its information field with relevant data
                        stars++;

                        // star class
                        if (sn.ScanData.StarTypeText != null)
                            bdClass.Append(sn.ScanData.StarTypeText);

                        // is the main star?
                        if (sn.ScanData.BodyName.EndsWith(" A", StringComparison.Ordinal) || sn.ScanData.BodyName == sn.SystemNode.System.Name)
                        {
                            bdDist.Append("Main Star".T(EDTx.UserControlScanGrid_MainStar));
                        }
                        // if not, then tell us its hierarchy leveland distance from main star
                        else if (sn.ScanData.nSemiMajorAxis.HasValue)
                        {
                            if (sn.ScanData.IsStar || sn.ScanData.nSemiMajorAxis.Value > BodyPhysicalConstants.oneAU_m / 10)
                                bdDist.AppendFormat("{0:N2} AU ({1:N1} ls)", (sn.ScanData.nSemiMajorAxis.Value / BodyPhysicalConstants.oneAU_m), sn.ScanData.nSemiMajorAxis.Value / BodyPhysicalConstants.oneLS_m);
                            else
                                bdDist.AppendFormat("{0} km", (sn.ScanData.nSemiMajorAxis.Value / 1000).ToString("N1"));
                        }

                        // display stellar bodies mass, in sols
                        if (sn.ScanData.nStellarMass.HasValue)
                            bdDetails.Append("Mass".T(EDTx.UserControlScanGrid_Mass)).AppendColonS().Append(sn.ScanData.nStellarMass.Value.ToString("N2")).Append(" SM, ");

                        // display stellar bodies radius in sols
                        if (sn.ScanData.nRadius.HasValue)
                            bdDetails.Append("Radius".T(EDTx.UserControlScanGrid_Radius)).AppendColonS().Append((sn.ScanData.nRadius.Value / BodyPhysicalConstants.oneSolRadius_m).ToString("N2")).Append(" SR, ");

                        // show the temperature
                        if (sn.ScanData.nSurfaceTemperature.HasValue)
                            bdDetails.Append("Temperature".T(EDTx.UserControlScanGrid_Temperature)).AppendColonS().Append((sn.ScanData.nSurfaceTemperature.Value.ToString("N2"))).Append(" K.");

                        // habitable zone for stars - do not display for black holes.
                        if (sn.ScanData.StarTypeID != EDStar.H)
                        {
                            JournalScan.HabZones hz = sn.ScanData.GetHabZones();

                            if (IsSet(CtrlList.showHabitable))
                            {
                                bdDetails.AppendCR();
                                sn.ScanData.HabZoneText_Hab(hz, bdDetails);
                            }
                            if (IsSet(CtrlList.showMetalRich))
                            {
                                bdDetails.AppendCR();
                                sn.ScanData.HabZoneText_MRP(hz, bdDetails);
                            }
                            if (IsSet(CtrlList.showWaterWorlds))
                            {
                                bdDetails.AppendCR();
                                sn.ScanData.HabZoneText_WW(hz, bdDetails);
                            }
                            if (IsSet(CtrlList.showEarthLike))
                            {
                                bdDetails.AppendCR();
                                sn.ScanData.HabZoneText_EL(hz, bdDetails);
                            }
                            if (IsSet(CtrlList.showAmmonia))
                            {
                                bdDetails.AppendCR();
                                sn.ScanData.HabZoneText_AW(hz, bdDetails);
                            }
                            if (IsSet(CtrlList.showIcyBodies))
                            {
                                bdDetails.AppendCR();
                                sn.ScanData.HabZoneText_ZIP(hz, bdDetails);
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

                                bdDist.AppendFormat("{0:N2} AU ({1:N1} ls)", sn.ScanData.DistanceFromArrivalLS / BodyPhysicalConstants.oneAU_LS, sn.ScanData.DistanceFromArrivalLS);
                            }
                            else
                            {
                                moons++;

                                bdClass.AppendSPC().Append("Moon".T(EDTx.UserControlScanGrid_Moon));

                                // moon distances from center body are measured from in SemiMajorAxis
                                if (sn.ScanData.nSemiMajorAxis.HasValue)
                                    bdDist.AppendFormat("{0:N1} ls ({1:N0} km)", sn.ScanData.nSemiMajorAxis.Value / BodyPhysicalConstants.oneLS_m, sn.ScanData.nSemiMajorAxis.Value / 1000);
                            }
                        }

                        // Details

                        // display non-stellar bodies radius in earth radiuses
                        if (sn.ScanData.nRadius.HasValue)
                        {
                            bdDetails.Append("Radius".T(EDTx.UserControlScanGrid_Radius)).AppendColonS()
                                .Append((sn.ScanData.nRadius.Value / 1000.0).ToString("N0")).Append(" km (").
                                Append((sn.ScanData.nRadius.Value / BodyPhysicalConstants.oneEarthRadius_m).ToString("N2")).Append(" ER), ");
                        }

                        // show the temperature, both in K and C degrees
                        if (sn.ScanData.nSurfaceTemperature.HasValue)
                        {
                            bdDetails.Append("Temperature".T(EDTx.UserControlScanGrid_Temperature)).AppendColonS()
                            .Append((sn.ScanData.nSurfaceTemperature.Value).ToString("N2")).Append(" K, (")
                            .Append((sn.ScanData.nSurfaceTemperature.Value - 273.15).ToString("N2")).Append(" C).");
                        }

                        // print the main atmospheric composition and pressure, if presents
                        if (sn.ScanData.Atmosphere != "none")
                        {
                            bdDetails.AppendCR().Append(sn.ScanData.Atmosphere);
                            if (sn.ScanData.nSurfacePressure.HasValue)
                            {
                                bdDetails.Append(", ").Append((sn.ScanData.nSurfacePressure.Value / BodyPhysicalConstants.oneAtmosphere_Pa).ToString("N3")).Append(" Pa.");
                            }
                        }

                        // tell us that a bodie is landable, and shows its gravity
                        if (sn.ScanData.IsLandable)
                        {
                            var Gg = "";

                            if (sn.ScanData.nSurfaceGravity.HasValue)
                            {
                                var g = sn.ScanData.nSurfaceGravity / BodyPhysicalConstants.oneGee_m_s2;
                                Gg = " (G: " + g.Value.ToString("N1") + " g)";
                            }

                            bdDetails.AppendCR().Append("Landable".T(EDTx.UserControlScanGrid_Landable)).Append(Gg).Append(". ");
                        }

                        // tell us that there is some volcanic activity
                        if (sn.ScanData.Volcanism.HasChars())
                        {
                            bdDetails.AppendCR().Append("Geological activity".T(EDTx.UserControlScanGrid_Geologicalactivity)).AppendColonS().Append(sn.ScanData.Volcanism).Append(". ");
                        }

                        if (sn.ScanData.Mapped)
                        {
                            bdDetails.AppendCR().Append("Surface mapped".T(EDTx.UserControlScanGrid_Surfacemapped)).Append(". ");
                        }

                        if (sn.SurfaceFeatures != null)
                        {
                            bdDetails.AppendCR();
                            StarScan.SurfaceFeatureList(bdDetails, sn.SurfaceFeatures, 0, false);
                        }
                        if (sn.Organics != null)        // organic scans done
                        {
                            bdDetails.AppendCR();
                            JournalScanOrganic.OrganicList(bdDetails, sn.Organics, 0, false);
                            foreach (var os in sn.Organics)
                                organicssystemvalue += os.Value;
                        }
                        if (sn.Signals != null)
                        {
                            bdDetails.AppendCR();
                            JournalSAASignalsFound.SignalList(bdDetails, sn.Signals, 0, false, false);
                        }
                        if (sn.Genuses != null)
                        {
                            bdDetails.AppendCR();
                            JournalSAASignalsFound.GenusList(bdDetails, sn.Genuses, 0, false, false);
                        }

                        // materials                        
                        if (sn.ScanData.HasMaterials)
                        {
                            var ret = "";
                            foreach (KeyValuePair<string, double> mat in sn.ScanData.Materials)
                            {
                                var mc = MaterialCommodityMicroResourceType.GetByFDName(mat.Key);
                                if (mc?.IsJumponium == true)
                                {
                                    ret = ret.AppendPrePad(mc.TranslatedName, ", ");
                                }
                            }

                            if (ret.Length > 0 && IsSet(CtrlList.showMaterials))
                            {
                                bdDetails.AppendCR().Append("This body contains: ".T(EDTx.UserControlScanGrid_BC)).Append(ret);
                            }
                        }
                    }

                    // have some belt, ring or other special structure?
                    if (sn.ScanData.HasRingsOrBelts)
                    {
                        for (int r = 0; r < sn.ScanData.Rings.Length; r++)
                        {
                            if (sn.ScanData.Rings[r].Name.EndsWith("Belt", StringComparison.Ordinal))
                            {
                                if (IsSet(CtrlList.showBelts))
                                {
                                    // is a belt
                                    bdDetails.AppendCR().Append("Belt: ".T(EDTx.UserControlScanGrid_Belt));
                                    var RingName = sn.ScanData.Rings[r].Name;
                                    bdDetails.Append(sn.ScanData.Rings[r].TranslatedRingClass()).AppendSPC();
                                    bdDetails.Append((sn.ScanData.Rings[r].InnerRad / BodyPhysicalConstants.oneLS_m).ToString("N2")).Append(" ls to ").Append((sn.ScanData.Rings[r].OuterRad / BodyPhysicalConstants.oneLS_m).ToString("N2")).Append(" ls. ");
                                }
                            }
                            else
                            {
                                if (IsSet(CtrlList.showRings))
                                {
                                    // is a ring
                                    bdDetails.AppendCR().Append("Ring: ".T(EDTx.UserControlScanGrid_Ring));
                                    var RingName = sn.ScanData.Rings[r].Name;
                                    bdDetails.Append(sn.ScanData.Rings[r].TranslatedRingClass()).AppendSPC();
                                    bdDetails.Append((sn.ScanData.Rings[r].InnerRad / BodyPhysicalConstants.oneLS_m).ToString("N2")).Append(" ls to ").Append((sn.ScanData.Rings[r].OuterRad / BodyPhysicalConstants.oneLS_m).ToString("N2")).Append(" ls. ");
                                }
                            }
                        }
                    }

                    if (sn.ScanData.IsWebSourced)
                        bdDetails.AppendCR().Append(sn.ScanData.DataSourceName);

                    sn.ScanData.Jumponium(jumponiums);      // add to jumponiums hash any seen

                    texttoadd = new string[] { sn.ScanData.BodyDesignationOrName, bdClass.ToString(), bdDist.ToString(), bdDetails.ToString() };

                    sd.DrawNode(pc, sn, null, null, BodyToImages.GetPlanetImageNotScanned(), new Point(imagesize.Width, imagesize.Height), true, false, out Rectangle _, out int _, imagesize,
                        SystemDisplay.DrawLevel.NoText, new Random());
                }
                else if (!sn.WebCreatedNode)             // rejected above, due no scan data or its EDSM and not EDSM selected.. present what we have if its ours
                {
                    if (sn.SurfaceFeatures != null)
                    {
                        bdDetails.AppendCR();
                        StarScan.SurfaceFeatureList(bdDetails, sn.SurfaceFeatures, 0, false);
                    }
                    if (sn.Organics != null)
                    {
                        bdDetails.AppendCR();
                        JournalScanOrganic.OrganicList(bdDetails, sn.Organics, 0, false);
                    }
                    if (sn.Signals != null)
                    {
                        bdDetails.AppendCR();
                        JournalSAASignalsFound.SignalList(bdDetails, sn.Signals, 0, false, false);
                    }
                    if (sn.Genuses != null)
                    {
                        bdDetails.AppendCR();
                        JournalSAASignalsFound.GenusList(bdDetails, sn.Genuses, 0, false, false);
                    }

                    texttoadd = new string[] { sn.FullName, "", "", bdDetails.ToString() };
                    pc.Add(new ExtPictureBox.ImageElement(new Rectangle(0, 0, imagesize.Width, imagesize.Height),
                            sn.NodeType == StarScan.ScanNodeType.star ? BodyToImages.GetStarImageNotScanned() :
                            sn.NodeType == StarScan.ScanNodeType.belt ? BodyToImages.GetBeltImage() :
                            BodyToImages.GetPlanetImageNotScanned(),
                            imgowned:false));       // NOTE the picture box does not own the image
                }

                if (texttoadd != null)      // if row to add
                {
                    DataGridViewRow rw = null;

                    foreach (DataGridViewRow crw in dataGridView.Rows)
                    {
                        if (crw.Tag == sn)
                        {
                            rw = crw;
                            //System.Diagnostics.Debug.WriteLine($"Found row row for {sn.FullName} at {rw.Index}");
                            break;
                        }
                    }

                    if (rw == null)
                    {
                        //System.Diagnostics.Debug.WriteLine($"Need a new row for {sn.FullName}");

                        rw = dataGridView.RowTemplate.Clone() as DataGridViewRow;           // need to add like this due to different types of cells
                        DataGridViewPictureBoxCell c0 = new DataGridViewPictureBoxCell();
                        rw.Cells.Add(c0);

                        for (int c = 1; c <= 7; c++)        // N more text rows
                        {
                            DataGridViewTextBoxCell cx = new DataGridViewTextBoxCell();
                            cx.Value = "";
                            rw.Cells.Add(cx);
                        }

                        int rwn = dataGridView.Rows.Add(rw);
                        rw.Tag = sn;                    // record sn in row so we can find it next time
                    }

                    // update row cell contents - at the moment its a blind replacement - maybe we can be more clevered

                    var pbc = rw.Cells[0] as DataGridViewPictureBoxCell;
                    pbc.PictureBox.ClearImageList();
                    pbc.PictureBox.AddRange(pc);
                    pbc.PictureBox.Render();
                    dataGridView.InvalidateCell(pbc);   // ensure revalidation

                    rw.Cells[1].Value = texttoadd[0];
                    rw.Cells[2].Value = texttoadd[1];
                    rw.Cells[3].Value = texttoadd[2];
                    rw.Cells[4].Value = texttoadd[3];
                    var tooltiptext = sn.ScanData?.DisplayString(historicmcl, curmcl);
                    if (tooltiptext != null)
                        rw.Cells[4].Tag = rw.Cells[4].ToolTipText = tooltiptext;

                    if (sn.ScanData != null)
                    {
                        sn.ScanData.GetPossibleEstimatedValues(false,
                                            out long basevalue,
                                            out long mappedvalue, out long mappedefficiently,
                                            out long firstmappedvalue, out long firstmappedefficiently,
                                            out long firstdiscoveredmappedvalue, out long firstdiscoveredmappedefficiently,
                                            out long best
                            );

                        rw.Cells[5].Value = sn.ScanData.EstimatedValue.ToString("N0");
                        rw.Cells[6].Value = best.ToString("N0");
                    }
                    else
                    {
                        rw.Cells[5].Value = 
                        rw.Cells[6].Value = "";
                    }

                    bool biosignals = sn.Signals != null ? JournalSAASignalsFound.ContainsBio(sn.Signals) : false;      // has bio signals
                    rw.Cells[7].Value = organicssystemvalue > 0 ? organicssystemvalue.ToString("N0") : biosignals ? "???" : "";     // tick if has biosignals, but we don't have totals

                    organicvaluetotal += organicssystemvalue;
                }
            }
            
            toolStripJumponiumProgressBar.Value = jumponiums.Count;
            toolStripJumponiumProgressBar.Visible = toolStripJumponiumProgressBar.Value > 0;

            //System.Diagnostics.Debug.WriteLine("Jumponiums " + toolStripJumponiumProgressBar.Value + " " + toolStripJumponiumProgressBar.Visible);

            if (toolStripJumponiumProgressBar.Value == 8)
                toolStripJumponiumProgressBar.ToolTipText = "This is a green system, as it has all existing jumponium materials available!".T(EDTx.UserControlScanGrid_GS);
            else
                toolStripJumponiumProgressBar.ToolTipText = toolStripJumponiumProgressBar.Value + " jumponium materials found in system.".T(EDTx.UserControlScanGrid_JS);

            string ct = systemnode.System.Name;
            long totalv = systemnode.ScanValue(edsmSpanshButton.IsAnySet) + organicvaluetotal;
            if (totalv > 0)
                ct += " ~" + totalv.ToString("N0") + " cr";
            SetControlText( ct ); 

            toolStripStatusTotalValue.Text = string.Format("Scan Summary for {0}: {1} stars; {2} planets ({3} terrestrial, {4} gas giants), {5} moons".T(EDTx.UserControlScanGrid_ScanSummaryfor), systemnode.System.Name, stars, planets, terrestrial, gasgiants, moons);

            if (dataGridView.SortedColumn != null)      // resort if sorted
            {
                dataGridView.Sort(dataGridView.SortedColumn, dataGridView.SortOrder == SortOrder.Descending ? ListSortDirection.Descending : ListSortDirection.Ascending);
            }
        }

        #endregion

        #region UI

        void dataGridViewScangrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == colBriefing.Index && e.RowIndex >= 0)  // if clicked on this row, we swap text between tag and value to display detailed view
            {
                var curdata = dataGridView.Rows[e.RowIndex].Cells[colBriefing.Index].Value;
                dataGridView.Rows[e.RowIndex].Cells[colBriefing.Index].Value = dataGridView.Rows[e.RowIndex].Cells[colBriefing.Index].Tag;      
                dataGridView.Rows[e.RowIndex].Cells[colBriefing.Index].Tag = curdata;
            }
        }

        protected enum CtrlList
        {
            showHabitable,showMetalRich,showWaterWorlds,showEarthLike,showAmmonia,showIcyBodies,
            showBelts,showRings,showMaterials
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
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();
            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(CtrlList.showBelts.ToString(), "Show belts".TxID(EDTx.UserControlScanGrid_structuresToolStripMenuItem_beltsToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showRings.ToString(), "Show rings".TxID(EDTx.UserControlScanGrid_structuresToolStripMenuItem_ringsToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showMaterials.ToString(), "Show materials".TxID(EDTx.UserControlScanGrid_materialsToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showHabitable.ToString(), "Show Habitation Zone".TxID(EDTx.UserControlSpanel_showCircumstellarZonesToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showMetalRich.ToString(), "Show Metal Rich Planet Zone".TxID(EDTx.UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showMetalRichPlanetsToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showWaterWorlds.ToString(), "Show Water World Zone".TxID(EDTx.UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showWaterWorldsToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showEarthLike.ToString(), "Show Earth Like Zone".TxID(EDTx.UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showEarthLikeToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showAmmonia.ToString(), "Show Ammonia Worlds Zone".TxID(EDTx.UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showAmmoniaWorldsToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.showIcyBodies.ToString(), "Show Icy Planets Zone".TxID(EDTx.UserControlSpanel_showCircumstellarZonesToolStripMenuItem_showIcyPlanetsToolStripMenuItem));
            CommonCtrl(displayfilter, extButtonShowControl);
        }
        
        private void CommonCtrl(CheckedIconNewListBoxForm displayfilter, Control button)
        {
            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ScreenMargin = new Size(0, 0);
            displayfilter.CloseBoundaryRegion = new Size(32, button.Height);

            displayfilter.SaveSettings = (s, o) =>
            {
                var ctrllist = displayfilter.UC.TagList();
                PutBoolSettingsFromString(s, ctrllist);
                PopulateCtrlList();
                DrawSystem(last_he, true);
            };

            displayfilter.Show(CtrlStateAsString(), button, this.FindForm());
        }

        private void extButtonHighValue_Click(object sender, EventArgs e)
        {
            int v = ScanDisplayUserControl.HighValueForm(this.FindForm(), GetSetting(dbValueLimit,50000));
            if (v >= 0)
            {
                PutSetting(dbValueLimit, v);
                DrawSystem(last_he,true);
            }
        }


        #endregion

        private void dataGridView_SortCompare(object sender, DataGridViewSortCompareEventArgs e)
        {
            if (e.Column.Index == colName.Index)
                e.SortDataGridViewColumnAlphaInt();
            else if (e.Column.Index == colDistance.Index || e.Column.Index == ColCurValue.Index || e.Column.Index == ColMaxValue.Index || e.Column.Index == ColOrganics.Index)
                e.SortDataGridViewColumnNumeric();
        }
    }
}
