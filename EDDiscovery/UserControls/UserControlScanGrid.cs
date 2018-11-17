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
                        bdClass.Append(" " + "Moon".Tx(this));
                    }

                    if (sn.ScanData.IsStar && sn.ScanData.BodyName.EndsWith(" A"))
                    {
                        bdDist.AppendFormat("Main Star".Tx(this));
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
                        bdDetails.Append("Mass".Tx(this) + ": " + sn.ScanData.nStellarMass.Value.ToString("N2") + ", ");

                    // habitable zone for stars - do not display for black holes.
                    if (sn.ScanData.HabitableZoneInner != null && sn.ScanData.HabitableZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
                        bdDetails.AppendFormat("Habitable Zone".Tx(this) + ": {0}-{1}AU ({2}). ", (sn.ScanData.HabitableZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.HabitableZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetHabZoneStringLs());

                    // tell us that a bodie is landable, and shows its gravity
                    if (sn.ScanData.IsLandable == true)
                    {
                        string Gg = "";

                        if (sn.ScanData.nSurfaceGravity.HasValue)
                        {
                            double? g = sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2;
                            Gg = " (G: " + g.Value.ToString("N1") + ")";
                        }

                        bdDetails.Append("Landable".Tx(this) + Gg + ". ");
                    }

                    // append the terraformable state to the planet class
                    if (sn.ScanData.Terraformable == true)
                        bdDetails.Append("Terraformable".Tx(this) + ". ");

                    // tell us that there is some volcanic activity
                    if (sn.ScanData.Volcanism != null)
                        bdDetails.Append("Volcanism".Tx(this) + ". ");

                    // have some ring?
                    if (sn.ScanData.HasBelts || sn.ScanData.HasRings)
                    {
                        if (sn.ScanData.Rings[0].Name.EndsWith("Belt"))
                        {
                            if (sn.ScanData.Rings.Count() > 1)
                            {
                                bdDetails.Append(string.Format("Has {0} belts: ".Tx(this, "Has {0} belts"),
                                                               sn.ScanData.Rings.Count()));
                            }
                            else
                            {
                                bdDetails.Append("Has 1 belt: ".Tx(this, "Has 1 belt"));
                            }

                            foreach (var r in sn.ScanData.Rings)
                            {
                                var BeltName = r.Name;
                                bdDetails.Append(JournalScan.StarPlanetRing
                                                            .DisplayStringFromRingClass(r
                                                                                            .RingClass) + " ");
                                bdDetails.Append((r.InnerRad / JournalScan.oneLS_m).ToString("N2") +
                                                 "ls to " + (r.OuterRad / JournalScan.oneLS_m)
                                                 .ToString("N2") + "ls. ");
                            }
                        }
                        else if (sn.ScanData.Rings[0].Name.EndsWith("Ring"))
                        {
                            if (sn.ScanData.Rings.Count() > 1)
                            {
                                bdDetails.Append(string.Format("Has {0} rings: ".Tx(this, "Rings"),
                                                               sn.ScanData.Rings.Count()));
                            }
                            else
                            {
                                bdDetails.Append("Has 1 ring: ".Tx(this, "Ring"));
                            }

                            foreach (var r in sn.ScanData.Rings)
                            {
                                var RingName = r.Name;
                                bdDetails.Append(JournalScan.StarPlanetRing
                                                            .DisplayStringFromRingClass(r
                                                                                            .RingClass) + " ");
                                bdDetails.Append((r.InnerRad / JournalScan.oneLS_m).ToString("N2") +
                                                 "ls to " + (r.OuterRad / JournalScan.oneLS_m)
                                                 .ToString("N2") + "ls. ");
                            }
                        }
                    }

                    // print the main atmospheric composition
                    if (sn.ScanData.Atmosphere != null && sn.ScanData.Atmosphere != "None")
                        bdDetails.Append(sn.ScanData.Atmosphere + ". ");

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
                            bdDetails.Append("\n" + "This body contains: ".Tx(this, "BC") + ret);
                    }

                    int value = sn.ScanData.EstimatedValue;
                    bdDetails.Append(Environment.NewLine + "Value".Tx(this) + " " + value.ToString("N0"));

                    //if ( sn.ScanData.EDSMDiscoveryCommander != null)      // not doing this, could be an option..
                    //    bdDetails.Append("\n" + "Discovered by: " + sn.ScanData.EDSMDiscoveryCommander + " on " + sn.ScanData.EDSMDiscoveryUTC.ToStringYearFirst());

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

                    cur.Tag = img;
                    cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
                            sn.ScanData.DisplayString(historicmatlist: last_he.MaterialCommodity, currentmatlist: discoveryform.history.GetLast?.MaterialCommodity);
                }
            }

            // display total scan values
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

            return string.Format("Approx total scan value: {0:N0}".Tx(this, "AV"), value);
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
