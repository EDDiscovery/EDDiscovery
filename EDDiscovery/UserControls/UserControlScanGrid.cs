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
        private string DbColumnSave => DBName("ScanGridPanel", "DGVCol");
		private string DbSave => DBName("ScanGrid");

		public UserControlScanGrid()
        {
            InitializeComponent();
            var corner = dataGridViewScanGrid.TopLeftHeaderCell; // work around #1487

            // this allows the row to grow to accomodate the text.. with a min height of 32.
            dataGridViewScanGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewScanGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            dataGridViewScanGrid.RowTemplate.MinimumHeight = 32;
            dataGridViewScanGrid.Columns["colImg"].DefaultCellStyle.SelectionBackColor = Color.Transparent;
        }

        public override void Init()
        {
            discoveryform.OnNewEntry += NewEntry;

			// retrieve values from db
			// columns
			imageToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showImageColumn", true);
			nameToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showNameColumn", true);
			classToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showClassColumn", true);
			distanceToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showDistanceColumn", true);
			informationToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showInformationColumn", true);
			
			// details
			massToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showMass", true);
			radiusToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showRadius", true);
			atmosphereToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showAtmosphere", true);
			circumstellarZonesToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showZones", true);
			massToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showMaterials", true);
			valueToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showValue", true);
			beltsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showBelts", true);
			ringsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showRings", true);
			
            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(toolTip, this);
        }

        public override void LoadLayout()
        {
            uctg.OnTravelSelectionChanged += Display;
            DGVLoadColumnLayout(dataGridViewScanGrid, DbColumnSave);
        }

        public override void ChangeCursorType(IHistoryCursor thc)
        {
            uctg.OnTravelSelectionChanged -= Display;
            uctg = thc;
            uctg.OnTravelSelectionChanged += Display;
        }

        public override void Closing()
        {
            DGVSaveColumnLayout(dataGridViewScanGrid, DbColumnSave);
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
                dataGridViewScanGrid.Rows.Clear();
                SetControlText("No Scan".Tx());
                return;
            }
            else
            {
                scannode = discoveryform.history.starscan.FindSystem(he.System, true);        // get data with EDSM

                if (scannode == null)     // no data, clear display, clear any last_he so samesys is false next time
                {
                    last_he = null;
                    dataGridViewScanGrid.Rows.Clear();
                    SetControlText("No Scan".Tx());
                    return;
                }

                if (samesys && !force)      // same system, no force, no redisplay
                    return;
            }

            last_he = he;

            // only record first row if same system 
            int firstdisplayedrow = (dataGridViewScanGrid.RowCount > 0 && samesys) ? dataGridViewScanGrid.FirstDisplayedScrollingRowIndex : -1;

            dataGridViewScanGrid.Rows.Clear();

            List<StarScan.ScanNode> all_nodes = scannode.Bodies.ToList();// flatten tree of scan nodes to prepare for listing

            foreach (StarScan.ScanNode sn in all_nodes)
            {
				if (sn.ScanData == null || sn.ScanData.BodyName == null) continue;
				var bdClass = new StringBuilder();
				var bdDist = new StringBuilder();
				var bdDetails = new StringBuilder();

				// Is it a star?
				if (sn.ScanData.StarTypeText != null) bdClass.Append(sn.ScanData.StarTypeText);

				// If so, check if this is the main star, and inform us accordingly.
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
					
				// If it's not a star, it can be a planet...
				if (sn.ScanData.PlanetClass != null)
				{
					// Is it terraformable?
					if (sn.ScanData.Terraformable == true)
						bdClass.Append("Terraformable".Tx(this) + " ");

					bdClass.Append(sn.ScanData.PlanetClass);

					// ...or a moon!
					if (sn.level >= 2 && sn.type == StarScan.ScanNodeType.body)
					{
						bdClass.Append(" " + "Moon".Tx(this));
					}
				}

				// 
				// Proceed retrieving detailed information
				//

				// Get Mass...
				if (massToolStripMenuItem.Checked) 
					GetMass(sn, bdDetails);

				// ...and Radius.
				if (radiusToolStripMenuItem.Checked) 
					GetRadius(sn, bdDetails);

				// For stars only, retrieve the inferred circumstellar zones.
				if (sn.ScanData.IsStar && circumstellarZonesToolStripMenuItem.Checked) 
					GetZones(sn, bdDetails);
				
				// Tell us that a body is landable, and shows its gravity, please.
				if (sn.ScanData.IsLandable)
					GetLandable(sn, bdDetails);

				// show main atmospheric composition
				if (atmosphereToolStripMenuItem.Checked)
					GetAtmosphere(sn, bdDetails);
				
				// Tell us if there is some volcanic activity, ok?
				if (volcanismToolStripMenuItem.Checked)
					GetVolcanism(sn, bdDetails);
			
				// have some ring? Is so, let us know!
				if (ringsToolStripMenuItem.Checked)
					GetRings(sn, bdDetails);
				
				// retrieve materials list
				if (materialsToolStripMenuItem.Checked)
					GetMaterials(sn, bdDetails);
					
				// retrieve value
				if (valueToolStripMenuItem.Checked)
					GetValue(sn, bdDetails);
					
				// Populate grid

				Image img = null;
				img = sn.ScanData.IsStar == true ? sn.ScanData.GetStarTypeImage() : sn.ScanData.GetPlanetClassImage();

				dataGridViewScanGrid.Rows.Add(new object[] { null, sn.ScanData.BodyName, bdClass, bdDist, bdDetails });

				var cur = dataGridViewScanGrid.Rows[dataGridViewScanGrid.Rows.Count - 1];

				cur.Tag = img;
				cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
					sn.ScanData.DisplayString(historicmatlist: last_he.MaterialCommodity, currentmatlist: discoveryform.history.GetLast?.MaterialCommodity);
			}

            // display total scan values
            SetControlText(string.Format("Scan Summary for {0}. {1}".Tx(this, "SS"), scannode.system.Name, GetTotalScanValues(scannode)));

            if (firstdisplayedrow >= 0 && firstdisplayedrow < dataGridViewScanGrid.RowCount)
                dataGridViewScanGrid.FirstDisplayedScrollingRowIndex = firstdisplayedrow;
        }

		/// <summary>
		/// Retrieve bodies information
		/// </summary>
		/// <param name="sn"></param>
		/// <param name="bdDetails"></param>
		
		private void GetMass(StarScan.ScanNode sn, StringBuilder bdDetails)
		{
			// display stars and stellar bodies mass
			if (sn.ScanData.nStellarMass.HasValue)
			{
				bdDetails.Append("Mass".Tx(this) + ": " + sn.ScanData.nStellarMass.Value.ToString("N2") + ", ");
			}
			// do this also for planets
			else if (sn.ScanData.nMassEM.HasValue)
				bdDetails.Append("Mass".Tx(this) + ": " + sn.ScanData.nMassEM.Value.ToString("N2") + ", ");
		}

		private void GetRadius(StarScan.ScanNode sn, StringBuilder bdDetails)
		{
			// display body radius
			if (sn.ScanData.IsStar)
			{
				if (sn.ScanData.nRadius.HasValue)
					bdDetails.Append("Radius".Tx(this) + ": " + (sn.ScanData.nRadius.Value / JournalScan.solarRadius_m).ToString("N2") + " Sols. ");
			}
			else
			{
				if (sn.ScanData.nRadius.HasValue)
					bdDetails.Append("Radius".Tx(this) + ": " + (sn.ScanData.nRadius.Value / 1000).ToString("N2") + " km. ");
			}
		}

		private void GetZones(StarScan.ScanNode sn, StringBuilder bdDetails)
		{
			// habitable zone for stars (do not display for black holes)
			if (sn.ScanData.HabitableZoneInner != null && sn.ScanData.HabitableZoneOuter != null &&
				sn.ScanData.StarTypeID != EDStar.H)
				bdDetails.AppendFormat("\n" + "Habitable Zone".Tx(this) + ": {0}-{1}AU ({2}).",
									   (sn.ScanData.HabitableZoneInner.Value / JournalScan.oneAU_LS)
									   .ToString("N2"),
									   (sn.ScanData.HabitableZoneOuter.Value / JournalScan.oneAU_LS)
									   .ToString("N2"), sn.ScanData.GetHabZoneStringLs());
		}

		private void GetLandable(StarScan.ScanNode sn, StringBuilder bdDetails)
		{
			if (sn.ScanData.IsLandable != true) return;
			var Gg = "";

			if (sn.ScanData.nSurfaceGravity.HasValue)
			{
				var g = sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2;
				Gg = " (G: " + g.Value.ToString("N1") + ")";
			}

			bdDetails.Append("Landable".Tx(this) + Gg + ". ");
		}

		private static void GetAtmosphere(StarScan.ScanNode sn, StringBuilder bdDetails)
		{
			if (sn.ScanData.Atmosphere != null && sn.ScanData.Atmosphere != "None")
				bdDetails.Append(sn.ScanData.Atmosphere + ". ");
		}

		private void GetVolcanism(StarScan.ScanNode sn, StringBuilder bdDetails)
		{
			if (sn.ScanData.Volcanism != null)
				bdDetails.Append("Volcanism".Tx(this) + ". ");
		}
		
		private void GetRings(StarScan.ScanNode sn, StringBuilder bdDetails)
		{
			if (!sn.ScanData.HasRings) return;

			bdDetails.Append("\n");

			// check for rings
			if (sn.ScanData.Rings[0].Name.EndsWith("Belt"))
			{
				bdDetails.AppendFormat("Belt{0}".Tx(this), sn.ScanData.Rings.Count() == 1 ? ":" : "s:");

				foreach (var belt in sn.ScanData.Rings)
				{
					var beltName = belt.Name;
					bdDetails.Append("\n > " + JournalScan.StarPlanetRing.DisplayStringFromRingClass(belt.RingClass) + " ");
					bdDetails.Append((belt.InnerRad / JournalScan.oneLS_m).ToString("N2") + "ls to " + (belt.OuterRad / JournalScan.oneLS_m).ToString("N2") + "ls. ");
				}
			}
			else if (sn.ScanData.Rings[0].Name.EndsWith("Ring"))
			{
				bdDetails.AppendFormat("Ring{0}".Tx(this), sn.ScanData.Rings.Count() == 1 ? ":" : "s:");

				foreach (var ring in sn.ScanData.Rings)
				{
					var ringName = ring.Name;
					bdDetails.Append("\n > " + JournalScan.StarPlanetRing.DisplayStringFromRingClass(ring.RingClass) + " ");
					bdDetails.Append((ring.InnerRad / JournalScan.oneLS_m).ToString("N2") + "ls to " + (ring.OuterRad / JournalScan.oneLS_m).ToString("N2") + "ls. ");
				}
			}
		}

		private void GetMaterials(StarScan.ScanNode sn, StringBuilder bdDetails)
		{
			// materials                        
			if (!sn.ScanData.HasMaterials) return;
			
			var ret = "";
			foreach (KeyValuePair<string, double> mat in sn.ScanData.Materials)
			{
				var mc = MaterialCommodityData.GetByFDName(mat.Key);
				if (mc != null && mc.IsJumponium)
					ret = ret.AppendPrePad(mc.Name, ", ");
			}

			if (ret.Length > 0)
				bdDetails.Append("\n" + "This body contains: ".Tx(this, "BC") + ret);
		}

		private void GetValue(StarScan.ScanNode sn, StringBuilder bdDetails)
		{
			// body approx. value
			var value = sn.ScanData.EstimatedValue;
			bdDetails.Append(Environment.NewLine + "Value".Tx(this) + " " + value.ToString("N0"));
		}

		// Calculate approximate total values of scanned bodies
		private string GetTotalScanValues(StarScan.SystemNode system)
		{
			var value = system.Bodies.Where(body => body?.ScanData?.EstimatedValue != null).Sum(body => body.ScanData.EstimatedValue);
			return string.Format("Approx total scan value: {0:N0}".Tx(this, "AV"), value);
		}

		/// <summary>
		/// Events
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>

        private void dataGridViewScanGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var cur = dataGridViewScanGrid.Rows[e.RowIndex];
			
			if (cur.Tag == null) return;
			
			// we programatically draw the image because we have control over its pos/ size this way, which you can't do
			// with a image column - there you can only draw a fixed image or stretch it to cell contents.. which we don't want to do
			var sz = dataGridViewScanGrid.RowTemplate.MinimumHeight - 2;
			var vpos = e.RowBounds.Top + e.RowBounds.Height / 2 - sz / 2;
			e.Graphics.DrawImage((Image)cur.Tag, new Rectangle(e.RowBounds.Left + 1, vpos, sz, sz));
		}
		
        private void dataGridViewScanGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 4)
            {
                object curdata = dataGridViewScanGrid.Rows[e.RowIndex].Cells[4].Value;
                dataGridViewScanGrid.Rows[e.RowIndex].Cells[4].Value = dataGridViewScanGrid.Rows[e.RowIndex].Cells[4].Tag;
                dataGridViewScanGrid.Rows[e.RowIndex].Cells[4].Tag = curdata;
            }
        }

		private void dataGridViewScanGrid_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right) return;
			contextMenuStripSG.Visible = true;

			contextMenuStripSG.Top = MousePosition.Y;
			contextMenuStripSG.Left = MousePosition.X;
		}
		
		private void imageToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showImageColumn", imageToolStripMenuItem.Checked);
			colImg.Visible = SQLiteDBClass.GetSettingBool(DbSave + "showImageColumn", true);
		}

		private void nameToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showNameColumn", nameToolStripMenuItem.Checked);
			colName.Visible = SQLiteDBClass.GetSettingBool(DbSave + "showNameColumn", true);
		}

		private void classToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showClassColumn", classToolStripMenuItem.Checked);
			colClass.Visible = SQLiteDBClass.GetSettingBool(DbSave + "showClassColumn", true);
		}

		private void distanceToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showDistanceColumn", distanceToolStripMenuItem.Checked);
			colDistance.Visible = SQLiteDBClass.GetSettingBool(DbSave + "showDistanceColumn", true);
		}

		private void informationToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showInformationColumn", informationToolStripMenuItem.Checked);
			colInformation.Visible = SQLiteDBClass.GetSettingBool(DbSave + "showInformationColumn", true);
		}

		private void massToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showMass", massToolStripMenuItem.Checked);
			massToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showMass", true);
			if (last_he != null)
			{
				DrawSystem(last_he, true);
			}
		}

		private void radiusToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showRadius", radiusToolStripMenuItem.Checked);
			radiusToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showRadius", true);
			if (last_he != null)
			{
				DrawSystem(last_he, true);
			}
		}

		private void circumstellarZonesToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showZones", circumstellarZonesToolStripMenuItem.Checked);
			circumstellarZonesToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showZones", true);
			if (last_he != null)
			{
				DrawSystem(last_he, true);
			}
		}

		private void atmosphereToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showAtmosphere", atmosphereToolStripMenuItem.Checked);
			atmosphereToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showAtmosphere", true);
			if (last_he != null)
			{
				DrawSystem(last_he, true);
			}
		}

		private void materialsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showMaterials", materialsToolStripMenuItem.Checked);
			materialsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showMaterials", true);
			if (last_he != null)
			{
				DrawSystem(last_he, true);
			}
		}

		private void valueToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showValue", valueToolStripMenuItem.Checked);
			valueToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showValue", true);
			if (last_he != null)
			{
				DrawSystem(last_he, true);
			}
		}

		private void ringsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showRings", ringsToolStripMenuItem.Checked);
			ringsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showRings", true);
			if (last_he != null)
			{
				DrawSystem(last_he, true);
			}
		}

		private void beltsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			SQLiteDBClass.PutSettingBool(DbSave + "showBelts", beltsToolStripMenuItem.Checked);
			beltsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "showBelts", true);
			if (last_he != null)
			{
				DrawSystem(last_he, true);
			}
		}
	}
}
