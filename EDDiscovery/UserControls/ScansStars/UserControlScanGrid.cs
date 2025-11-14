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

            BaseUtils.TranslatorMkII.Instance.TranslateControls(this);
            BaseUtils.TranslatorMkII.Instance.TranslateTooltip(toolTip, this);

            DBBaseName = "ScanGridPanel";
        }

        #region Init

        protected override void Init()
        {
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

        protected override void LoadLayout()
        {
            DGVLoadColumnLayout(dataGridView);
        }

        protected override void Closing()
        {
            DGVSaveColumnLayout(dataGridView);
            PutSetting(dbRolledUp, rollUpPanelTop.PinState);
        }

        protected override void InitialDisplay()
        {
            RequestPanelOperation(this, new UserControlCommonBase.RequestTravelHistoryPos());     //request an update 
        }

        public override void ReceiveHistoryEntry(HistoryEntry he)        //we track the history cursor
        {
            DrawSystem(he);
        }

        #endregion

        #region PopulateGrid

        // HE will be null if there is no systems data available (empty db) and we change an option, so you need to defend against it
        private async void DrawSystem(HistoryEntry he, bool redrawall = false)
        {
            var samesys = last_he?.System != null && he?.System != null && he.System.Name == last_he.System.Name;       // is it the same system

            bool scanupdate = he?.journalEntry is IStarScan;      // possible update to data..

            System.Diagnostics.Debug.WriteLine($"Scan grid @{he?.EventTimeUTC} {he?.EventSummary} Samesync {samesys} force {redrawall} istarscan {scanupdate}");

            if (he == null || he.System.Name == "Unknown" || (samesys && !redrawall && !scanupdate))      // no he, or unknown system, or same system with no force, no redisplay
                return;

            last_he = he;   // record the he to note the last system visited

            EliteDangerousCore.StarScan2.SystemNode systemnode = await DiscoveryForm.History.StarScan2.FindSystemAsync(he.System, edsmSpanshButton.WebLookup);        // get data with EDSM maybe
            if (IsClosed)
                return;

            if (systemnode == null)     // no data, clear display, clear any last_he so samesys is false next time
            {
                System.Diagnostics.Debug.WriteLine($"Scan grid scan node not found");
                last_he = null;
                dataGridView.Rows.Clear();
                toolStripJumponiumProgressBar.Visible = false;
                SetControlText("No Scan".Tx());
                return;
            }

            System.Diagnostics.Debug.WriteLine($"Scan grid update display at {he.System.Name} found with {systemnode.Bodies().Count()} bodies");

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

            var systemdisplay = new EliteDangerousCore.StarScan2.SystemDisplay();
            systemdisplay.ValueLimit = GetSetting(dbValueLimit, 50000);
            systemdisplay.Font = Theme.Current.GetFont;

            long organicvaluetotal = 0;     // total of organic values

            foreach (var bn in systemnode.Bodies())
            {
                // define strings to be populated
                var bdClass = new StringBuilder();
                var bdDist = new StringBuilder();
                var bdDetails = new StringBuilder();
                string[] texttoadd = null;     // items to add to row
                List<ExtPictureBox.ImageElement> pc = new List<ExtPictureBox.ImageElement>();

                long organicssystemvalue = 0;

                if (bn.BodyType == BodyDefinitions.BodyType.PlanetaryRing || bn.BodyType == BodyDefinitions.BodyType.AsteroidCluster
                                    || bn.BodyType == BodyDefinitions.BodyType.Barycentre)
                {
                    // do nothing
                }
                else if (bn.BodyType == BodyDefinitions.BodyType.StellarRing)
                {
                    if (IsSet(CtrlList.showBelts) )
                    {
                        var subbody = bn.ChildBodies.FirstOrDefault();          // we need the belt cluster body, which has the scan, under it
                        bdClass.Append("Belt Cluster");

                        if (subbody?.Scan != null && (!subbody.Scan.IsWebSourced || edsmSpanshButton.IsAnySet))
                        {
                            if (Math.Abs(subbody.Scan.DistanceFromArrivalLS) > 0)
                            {
                                bdDist.AppendFormat("{0:N2} AU ({1:N1} ls)", subbody.Scan.DistanceFromArrivalLS / BodyPhysicalConstants.oneAU_LS, subbody.Scan.DistanceFromArrivalLS);
                            }
                        }

                        texttoadd = new string[] { bn.Name(), bdClass.ToString(), bdDist.ToString(), bdDetails.ToString() };

                        var bi = BodyDefinitions.GetBeltImage();
                        pc.Add(new ExtPictureBox.ImageElement(new Rectangle(0, 0, bi.Width/2, bi.Height/2),bi,
                                    imgowned:false)); // NOTE the picture box does not own the image
                    }
                }
                // must have scan data and either not edsm body or edsm check
                else if ( bn.Scan != null && (!bn.Scan.IsWebSourced || edsmSpanshButton.IsAnySet))
                {
                    if (bn.Scan.IsStar)
                    {
                        // is a star, so populate its information field with relevant data
                        stars++;

                        // star class
                        if (bn.Scan.StarTypeText != null)
                            bdClass.Append(bn.Scan.StarTypeText);

                        // is the main star?
                        if (bn.Scan.BodyName.EndsWith(" A", StringComparison.Ordinal) || bn.Scan.BodyName == bn.SystemNode.System.Name)
                        {
                            bdDist.Append("Main Star".Tx());
                        }
                        // if not, then tell us its hierarchy leveland distance from main star
                        else if (bn.Scan.nSemiMajorAxis.HasValue)
                        {
                            if (bn.Scan.IsStar || bn.Scan.nSemiMajorAxis.Value > BodyPhysicalConstants.oneAU_m / 10)
                                bdDist.AppendFormat("{0:N2} AU ({1:N1} ls)", (bn.Scan.nSemiMajorAxis.Value / BodyPhysicalConstants.oneAU_m), bn.Scan.nSemiMajorAxis.Value / BodyPhysicalConstants.oneLS_m);
                            else
                                bdDist.AppendFormat("{0} km", (bn.Scan.nSemiMajorAxis.Value / 1000).ToString("N1"));
                        }

                        // display stellar bodies mass, in sols
                        if (bn.Scan.nStellarMass.HasValue)
                            bdDetails.Append("Mass".Tx()).AppendColonS().Append(bn.Scan.nStellarMass.Value.ToString("N2")).Append(" SM, ");

                        // display stellar bodies radius in sols
                        if (bn.Scan.nRadius.HasValue)
                            bdDetails.Append("Radius".Tx()).AppendColonS().Append((bn.Scan.nRadius.Value / BodyPhysicalConstants.oneSolRadius_m).ToString("N2")).Append(" SR, ");

                        // show the temperature
                        if (bn.Scan.nSurfaceTemperature.HasValue)
                            bdDetails.Append("Temperature".Tx()).AppendColonS().Append((bn.Scan.nSurfaceTemperature.Value.ToString("N2"))).Append(" K.");

                        HabZones hz;
                        // habitable zone for stars - do not display for black holes.  And defend against hab zones returning null due to missing data
                        if (bn.Scan.StarTypeID != EDStar.H && (hz = bn.Scan.GetHabZones()) != null)
                        {
                            if (IsSet(CtrlList.showHabitable))
                            {
                                bdDetails.AppendCR();
                                hz.HabZoneText_Hab(bdDetails);
                            }
                            if (IsSet(CtrlList.showMetalRich))
                            {
                                bdDetails.AppendCR();
                                hz.HabZoneText_MRP(bdDetails);
                            }
                            if (IsSet(CtrlList.showWaterWorlds))
                            {
                                bdDetails.AppendCR();
                                hz.HabZoneText_WW(bdDetails);
                            }
                            if (IsSet(CtrlList.showEarthLike))
                            {
                                bdDetails.AppendCR();
                                hz.HabZoneText_EL(bdDetails);
                            }
                            if (IsSet(CtrlList.showAmmonia))
                            {
                                bdDetails.AppendCR();
                                hz.HabZoneText_AW(bdDetails);
                            }
                            if (IsSet(CtrlList.showIcyBodies))
                            {
                                bdDetails.AppendCR();
                                hz.HabZoneText_ZIP(bdDetails);
                            }
                        }
                    }
                    else
                    {
                        // is a non-stellar body                        

                        // is terraformable? If so, prepend it to the body class
                        if (bn.Scan.Terraformable)
                            bdClass.Append("Terraformable".Tx()).Append(", ");

                        if (bn.Scan.IsPlanet)      // Planet, not barycenter/belt
                        {
                            bdClass.Append(bn.Scan.PlanetTypeText);

                            int level = bn.GetNameDepth();

                            if (level <= 1)      // top level 
                            {
                                planets++;

                                if (bn.Scan.GasWorld)
                                    gasgiants++;
                                else
                                    terrestrial++;

                                bdDist.AppendFormat("{0:N2} AU ({1:N1} ls)", bn.Scan.DistanceFromArrivalLS / BodyPhysicalConstants.oneAU_LS, bn.Scan.DistanceFromArrivalLS);
                            }
                            else
                            {
                                moons++;

                                bdClass.AppendSPC().Append("Moon".Tx());

                                // moon distances from center body are measured from in SemiMajorAxis
                                if (bn.Scan.nSemiMajorAxis.HasValue)
                                    bdDist.AppendFormat("{0:N1} ls ({1:N0} km)", bn.Scan.nSemiMajorAxis.Value / BodyPhysicalConstants.oneLS_m, bn.Scan.nSemiMajorAxis.Value / 1000);
                            }
                        }

                        // Details

                        // display non-stellar bodies radius in earth radiuses
                        if (bn.Scan.nRadius.HasValue)
                        {
                            bdDetails.Append("Radius".Tx()).AppendColonS()
                                .Append((bn.Scan.nRadius.Value / 1000.0).ToString("N0")).Append(" km (").
                                Append((bn.Scan.nRadius.Value / BodyPhysicalConstants.oneEarthRadius_m).ToString("N2")).Append(" ER), ");
                        }

                        // show the temperature, both in K and C degrees
                        if (bn.Scan.nSurfaceTemperature.HasValue)
                        {
                            bdDetails.Append("Temperature".Tx()).AppendColonS()
                            .Append((bn.Scan.nSurfaceTemperature.Value).ToString("N2")).Append(" K, (")
                            .Append((bn.Scan.nSurfaceTemperature.Value - 273.15).ToString("N2")).Append(" C).");
                        }

                        // print the main atmospheric composition and pressure, if presents
                        if (bn.Scan.HasAtmosphere)
                        {
                            bdDetails.AppendCR().Append(bn.Scan.AtmosphereTranslated);
                            if (bn.Scan.nSurfacePressure.HasValue)
                            {
                                bdDetails.Append(", ").Append((bn.Scan.nSurfacePressure.Value / BodyPhysicalConstants.oneAtmosphere_Pa).ToString("N3")).Append(" Pa.");
                            }
                        }

                        // tell us that a bodie is landable, and shows its gravity
                        if (bn.Scan.IsLandable)
                        {
                            var Gg = "";

                            if (bn.Scan.nSurfaceGravity.HasValue)
                            {
                                var g = bn.Scan.nSurfaceGravity / BodyPhysicalConstants.oneGee_m_s2;
                                Gg = " (G: " + g.Value.ToString("N1") + " g)";
                            }

                            bdDetails.AppendCR().Append("Landable".Tx()).Append(Gg).Append(". ");
                        }

                        // tell us that there is some volcanic activity
                        if (bn.Scan.HasMeaningfulVolcanism)
                        {
                            bdDetails.AppendCR().Append("Geological activity".Tx()).AppendColonS().Append(bn.Scan.VolcanismTranslated).Append(". ");
                        }

                        if (bn.Scan.Mapped)
                        {
                            bdDetails.AppendCR().Append("Surface mapped".Tx()).Append(". ");
                        }

                        if (bn.Features != null)
                        {
                            bdDetails.AppendCR();
                            JournalScan.DisplaySurfaceFeatures(bdDetails, bn.Features, 0, false);
                        }
                        if (bn.Organics != null)        // organic scans done
                        {
                            bdDetails.AppendCR();
                            JournalScanOrganic.OrganicList(bdDetails, bn.Organics, 0, false);
                            foreach (var os in bn.Organics.Where(x => x.EstimatedValue.HasValue))
                                organicssystemvalue += os.EstimatedValue.Value;
                        }
                        if (bn.Signals != null)
                        {
                            bdDetails.AppendCR();
                            JournalSAASignalsFound.SignalList(bdDetails, bn.Signals, 0, false, false);
                        }
                        if (bn.Genuses != null)
                        {
                            bdDetails.AppendCR();
                            JournalSAASignalsFound.GenusList(bdDetails, bn.Genuses, 0, false, false);
                        }

                        // materials                        
                        if (bn.Scan.HasMaterials)
                        {
                            var ret = "";
                            foreach (KeyValuePair<string, double> mat in bn.Scan.Materials)
                            {
                                var mc = MaterialCommodityMicroResourceType.GetByFDName(mat.Key);
                                if (mc?.IsJumponium == true)
                                {
                                    ret = ret.AppendPrePad(mc.TranslatedName, ", ");
                                }
                            }

                            if (ret.Length > 0 && IsSet(CtrlList.showMaterials))
                            {
                                bdDetails.AppendCR().Append("This body contains".Tx()+": ").Append(ret);
                            }
                        }
                    }

                    // have some belt, ring or other special structure?
                    if (bn.Scan.HasRingsOrBelts)
                    {
                        for (int r = 0; r < bn.Scan.Rings.Length; r++)
                        {
                            if (bn.Scan.Rings[r].Name.EndsWith("Belt", StringComparison.Ordinal))
                            {
                                if (IsSet(CtrlList.showBelts))
                                {
                                    // is a belt
                                    bdDetails.AppendCR().Append("Belt".Tx()+": ");
                                    var RingName = bn.Scan.Rings[r].Name;
                                    bdDetails.Append(bn.Scan.Rings[r].TranslatedRingClass()).AppendSPC();
                                    bdDetails.Append((bn.Scan.Rings[r].InnerRad / BodyPhysicalConstants.oneLS_m).ToString("N2")).Append(" ls to ").Append((bn.Scan.Rings[r].OuterRad / BodyPhysicalConstants.oneLS_m).ToString("N2")).Append(" ls. ");
                                }
                            }
                            else
                            {
                                if (IsSet(CtrlList.showRings))
                                {
                                    // is a ring
                                    bdDetails.AppendCR().Append("Ring".Tx()+": ");
                                    var RingName = bn.Scan.Rings[r].Name;
                                    bdDetails.Append(bn.Scan.Rings[r].TranslatedRingClass()).AppendSPC();
                                    bdDetails.Append((bn.Scan.Rings[r].InnerRad / BodyPhysicalConstants.oneLS_m).ToString("N2")).Append(" ls to ").Append((bn.Scan.Rings[r].OuterRad / BodyPhysicalConstants.oneLS_m).ToString("N2")).Append(" ls. ");
                                }
                            }
                        }
                    }

                    if (bn.Scan.IsWebSourced)
                        bdDetails.AppendCR().Append(bn.Scan.DataSourceName);

                    bn.Scan.Jumponium(jumponiums);      // add to jumponiums hash any seen

                    texttoadd = new string[] { bn.Scan.BodyName, bdClass.ToString(), bdDist.ToString(), bdDetails.ToString() };

                    systemdisplay.DrawNode(pc, bn, null, null, new Point(0,0), true, false, out Rectangle _, out int _, new Size(48,48),
                                                new Random(), null,null,notext:true);
                    pc[0].Location = new Rectangle(0, 0, pc[0].Image.Width, pc[0].Image.Height);
                }
                else if (!bn.WebCreatedNode)             // rejected above, due no scan data or its EDSM and not EDSM selected.. present what we have if its ours
                {
                    if (bn.Features != null)
                    {
                        bdDetails.AppendCR();
                        JournalScan.DisplaySurfaceFeatures(bdDetails, bn.Features, 0, false);
                    }
                    if (bn.Organics != null)
                    {
                        bdDetails.AppendCR();
                        JournalScanOrganic.OrganicList(bdDetails, bn.Organics, 0, false);
                    }
                    if (bn.Signals != null)
                    {
                        bdDetails.AppendCR();
                        JournalSAASignalsFound.SignalList(bdDetails, bn.Signals, 0, false, false);
                    }
                    if (bn.Genuses != null)
                    {
                        bdDetails.AppendCR();
                        JournalSAASignalsFound.GenusList(bdDetails, bn.Genuses, 0, false, false);
                    }

                    texttoadd = new string[] { bn.Name(), "", "", bdDetails.ToString() };
                    pc.Add(new ExtPictureBox.ImageElement(new Rectangle(0, 0, 48,48),
                            bn.BodyType == BodyDefinitions.BodyType.Star ? BodyDefinitions.GetImageNotScanned() :
                            bn.BodyType == BodyDefinitions.BodyType.StellarRing ? BodyDefinitions.GetBeltImage() :
                            BodyDefinitions.GetImageNotScanned(),
                            imgowned:false));       // NOTE the picture box does not own the image
                }

                if (texttoadd != null)      // if row to add
                {
                    DataGridViewRow rw = null;

                    foreach (DataGridViewRow crw in dataGridView.Rows)
                    {
                        if (crw.Tag == bn)
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
                        rw.AddTextCells(7);
                        int rwn = dataGridView.Rows.Add(rw);
                        rw.Tag = bn;                    // record sn in row so we can find it next time
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
                    var tooltiptext = bn.Scan?.DisplayText(historicmcl, curmcl);
                    if (tooltiptext != null)
                        rw.Cells[4].Tag = rw.Cells[4].ToolTipText = tooltiptext;

                    if (bn.Scan != null)
                    {
                        bn.Scan.GetPossibleEstimatedValues(false,
                                            out long basevalue,
                                            out long mappedvalue, out long mappedefficiently,
                                            out long firstmappedvalue, out long firstmappedefficiently,
                                            out long firstdiscoveredmappedvalue, out long firstdiscoveredmappedefficiently,
                                            out long best
                            );

                        rw.Cells[5].Value = bn.Scan.EstimatedValue.ToString("N0");
                        rw.Cells[6].Value = best.ToString("N0");
                    }
                    else
                    {
                        rw.Cells[5].Value = 
                        rw.Cells[6].Value = "";
                    }

                    bool biosignals = bn.Signals != null ? JournalSAASignalsFound.ContainsBio(bn.Signals) : false;      // has bio signals
                    rw.Cells[7].Value = organicssystemvalue > 0 ? organicssystemvalue.ToString("N0") : biosignals ? "???" : "";     // tick if has biosignals, but we don't have totals

                    organicvaluetotal += organicssystemvalue;
                }
            }
            
            toolStripJumponiumProgressBar.Value = jumponiums.Count;
            toolStripJumponiumProgressBar.Visible = toolStripJumponiumProgressBar.Value > 0;

            //System.Diagnostics.Debug.WriteLine("Jumponiums " + toolStripJumponiumProgressBar.Value + " " + toolStripJumponiumProgressBar.Visible);

            if (toolStripJumponiumProgressBar.Value == 8)
                toolStripJumponiumProgressBar.ToolTipText = "This is a green system, as it has all existing jumponium materials available!".Tx();
            else
                toolStripJumponiumProgressBar.ToolTipText = toolStripJumponiumProgressBar.Value + " jumponium materials found in system.".Tx();

            string ct = systemnode.System.Name;
            long totalv = systemnode.ScanValue(edsmSpanshButton.IsAnySet) + organicvaluetotal;
            if (totalv > 0)
                ct += " ~" + totalv.ToString("N0") + " cr";
            SetControlText( ct ); 

            toolStripStatusTotalValue.Text = string.Format("Scan Summary for {0}: {1} stars; {2} planets ({3} terrestrial, {4} gas giants), {5} moons".Tx(), systemnode.System.Name, stars, planets, terrestrial, gasgiants, moons);

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
            displayfilter.UC.Add(CtrlList.showBelts.ToString(), "Show belts".Tx());
            displayfilter.UC.Add(CtrlList.showRings.ToString(), "Show rings".Tx());
            displayfilter.UC.Add(CtrlList.showMaterials.ToString(), "Show materials".Tx());
            displayfilter.UC.Add(CtrlList.showHabitable.ToString(), "Show Habitation Zone".Tx());
            displayfilter.UC.Add(CtrlList.showMetalRich.ToString(), "Show Metal Rich Planet Zone".Tx());
            displayfilter.UC.Add(CtrlList.showWaterWorlds.ToString(), "Show Water World Zone".Tx());
            displayfilter.UC.Add(CtrlList.showEarthLike.ToString(), "Show Earth Like Zone".Tx());
            displayfilter.UC.Add(CtrlList.showAmmonia.ToString(), "Show Ammonia Worlds Zone".Tx());
            displayfilter.UC.Add(CtrlList.showIcyBodies.ToString(), "Show Icy Planets Zone".Tx());
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

        private void extButtonNewBookmark_Click(object sender, EventArgs e)
        {
            BookmarkHelpers.SendToBookmarkForm(this.FindForm(), DiscoveryForm, last_he?.System);
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
