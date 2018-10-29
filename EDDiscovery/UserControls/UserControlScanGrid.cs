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
        private HistoryEntry _lastHe = null;
        private string DbColumnSave => DBName("ScanGridPanel", "DGVCol");
		private string DbSave => DBName("ScanGrid" );

		private bool _showGoldilocksZone;
		private bool _showMetalRichZone;
		private bool _showWaterWorldZone;
		private bool _showEarthLikeZone;
		private bool _showAmmoniaZone;
		private bool _showMaterials;
		private bool _showAtmosphere;
		private bool _showRings;

		public UserControlScanGrid()
        {
            InitializeComponent();
            var corner = dataGridViewScanGrid.TopLeftHeaderCell; // work around #1487

            // dataGridView setup - the rule is, use the designer for most properties.. only do these here since they are so buried or not available.

            // this allows the row to grow to accomodate the text.. with a min height of 32.
            dataGridViewScanGrid.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dataGridViewScanGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCells;     // NEW! appears to work https://msdn.microsoft.com/en-us/library/74b2wakt(v=vs.110).aspx
            dataGridViewScanGrid.RowTemplate.MinimumHeight = 32;
            this.dataGridViewScanGrid.Columns["ImageColumn"].DefaultCellStyle.SelectionBackColor =
				System.Drawing.Color.Transparent;
        }

        public override void Init()
        {
			// get settings from db
			// habitable zones
			goldilocksZoneToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "ShowGoldilocks", true);
			metalRichToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "ShowMetalRich", true);
			waterWorldsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "ShowWaterWorld ", true);
			earthLikeToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "ShowEarthLike", true);
			ammoniaWorldsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "ShowAmmonia", true);
			// toggles
			showAvailableMaterialsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "ShowMaterials", true);
			showAtmosphericDetailsToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "ShowAtmosphere", true);
			showRingsInformationToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "ShowRings", true);
			
            discoveryform.OnNewEntry += NewEntry;

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

		private void DrawSystem(HistoryEntry he, bool force)
        {
            StarScan.SystemNode scanNode = null;

            var sameSys = _lastHe != null && he != null && he.System.Name == _lastHe.System.Name;

            //System.Diagnostics.Debug.WriteLine("Scan grid " + sameSys + " F:" + force);

            if (he == null)     //  no he, no display
            {
                _lastHe = null;
                dataGridViewScanGrid.Rows.Clear();
                SetControlText("No Scan".Tx());
                return;
            }
            else
            {
                scanNode = discoveryform.history.starscan.FindSystem(he.System, true);        // get data with EDSM

                if (scanNode == null)     // no data, clear display, clear any last_he so sameSys is false next time
                {
                    _lastHe = null;
                    dataGridViewScanGrid.Rows.Clear();
                    SetControlText("No Scan".Tx());
                    return;
                }

                if (sameSys && !force)      // same system, no force, no redisplay
                    return;
            }

            _lastHe = he;

            // only record first row if same system 
            var firstDisplayedScrollingRowIndex = (dataGridViewScanGrid.RowCount > 0 && sameSys) ? dataGridViewScanGrid.FirstDisplayedScrollingRowIndex : -1;

            dataGridViewScanGrid.Rows.Clear();

            var allNodes = scanNode.Bodies.ToList();// flatten tree of scan nodes to prepare for listing

            foreach (var sn in allNodes)
            {
				if (sn.ScanData?.BodyName == null) continue;
				var bdClass = new StringBuilder();
				var bdDist = new StringBuilder();
				var bdDetails = new StringBuilder();

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
						bdDist.AppendFormat("{0:N1}km", (sn.ScanData.nSemiMajorAxis.Value / 1000));
				}

				// display stars and stellar bodies mass
				if (sn.ScanData.IsStar && sn.ScanData.nStellarMass.HasValue)
					bdDetails.Append("Solar Masses".Tx(this) + ": " + sn.ScanData.nStellarMass.Value.ToString("N2") + ", ");

				if (sn.ScanData.IsStar && sn.ScanData.nStellarMass.HasValue)
					bdDetails.Append("Solar Radii".Tx(this) + ": " + (sn.ScanData.nRadius.Value / JournalScan.solarRadius_m).ToString("N2") + ", ");
				
				if (sn.ScanData.IsStar && sn.ScanData.nStellarMass.HasValue)
					bdDetails.Append("Age".Tx(this) + ": " + sn.ScanData.nAge.Value.ToString("N0") + " my. \n");

				// habitable zone for stars - do not display for black holes.
				if (_showGoldilocksZone != false && sn.ScanData.HabitableZoneInner != null && sn.ScanData.HabitableZoneOuter != null && sn.ScanData.StarTypeID != EDStar.H)
					bdDetails.AppendFormat("Habitable Zone".Tx(this) + ": {0}-{1}AU ({2}). \n", (sn.ScanData.HabitableZoneInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.HabitableZoneOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetHabZoneStringLs());

				// possible metal rich zone
				if (_showMetalRichZone != false && sn.ScanData.MetalRichOuter != null && sn.ScanData.StarTypeID != EDStar.H)
					bdDetails.AppendFormat("Metal Rich".Tx(this) + ": {0}-{1}AU ({2}). \n", (sn.ScanData.MetalRichInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.MetalRichOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetMetalRichZoneStringLs());

				// possible earth like zone
				if (_showEarthLikeZone != false && sn.ScanData.EarthLikeInner != null && sn.ScanData.EarthLikeOuter != null && sn.ScanData.StarTypeID != EDStar.H)
					bdDetails.AppendFormat("Earth Like".Tx(this) + ": {0}-{1}AU ({2}). \n", (sn.ScanData.EarthLikeInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.EarthLikeOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetEarthLikeZoneStringLs());

				// possible water worlds zone
				if (_showWaterWorldZone != false && sn.ScanData.WaterWorldsInner != null && sn.ScanData.WaterWorldsOuter != null && sn.ScanData.StarTypeID != EDStar.H)
					bdDetails.AppendFormat("Water Worlds".Tx(this) + ": {0}-{1}AU ({2}). \n", (sn.ScanData.WaterWorldsInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.WaterWorldsOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetWaterWorldZoneStringLs());		
				
				// possible ammonia worlds zone
				if (_showAmmoniaZone != false && sn.ScanData.AmmoniaWorldsInner != null && sn.ScanData.AmmoniaWorldsOuter != null && sn.ScanData.StarTypeID != EDStar.H)
					bdDetails.AppendFormat("Ammonia Worlds".Tx(this) + ": {0}-{1}AU ({2}). \n", (sn.ScanData.AmmoniaWorldsInner.Value / JournalScan.oneAU_LS).ToString("N2"), (sn.ScanData.AmmoniaWorldsOuter.Value / JournalScan.oneAU_LS).ToString("N2"), sn.ScanData.GetAmmoniaWorldsStringLs());

				// tell us that a body is landable, and shows its gravity
				var Gg = "";
				if (sn.ScanData.IsLandable == true)
				{
					if (sn.ScanData.nSurfaceGravity.HasValue)
					{
						var g = sn.ScanData.nSurfaceGravity / JournalScan.oneGee_m_s2;
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
				if (_showRings != false && sn.ScanData.HasRings && sn.ScanData.IsStar == false)
				{
					if (sn.ScanData.Rings.Count() <= 1)
					{
						bdDetails.Append("Has 1 ring: ".Tx(this, "Ring"));
					}
					else
					{
						bdDetails.Append(string.Format("Has {0} rings: ".Tx(this, "Rings"), sn.ScanData.Rings.Count()));
					}

					for (var i = sn.ScanData.Rings.Length - 1; i >= 0; i--)
					{
						var ringName = sn.ScanData.Rings[i].Name;
						bdDetails.Append(JournalScan.StarPlanetRing.DisplayStringFromRingClass(sn.ScanData.Rings[i].RingClass) + " ");
						bdDetails.Append((sn.ScanData.Rings[i].InnerRad / JournalScan.oneLS_m).ToString("N2") + "ls to " + (sn.ScanData.Rings[i].OuterRad / JournalScan.oneLS_m).ToString("N2") + "ls. ");
					}
				}

				// print the main atmospheric composition
				if (_showAtmosphere != false && sn.ScanData.Atmosphere != null && sn.ScanData.Atmosphere != "None")
					bdDetails.Append(sn.ScanData.Atmosphere + ". ");

				// materials                        
				if (_showMaterials != false && sn.ScanData.HasMaterials)
				{
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

				var value = sn.ScanData.EstimatedValue;
				bdDetails.Append(Environment.NewLine + "Value".Tx(this) + " " + value.ToString("N0"));

				//if ( sn.ScanData.EDSMDiscoveryCommander != null)      // not doing this, could be an option..
				//    bdDetails.Append("\n" + "Discovered by: " + sn.ScanData.EDSMDiscoveryCommander + " on " + sn.ScanData.EDSMDiscoveryUTC.ToStringYearFirst());

				Image img = null;

				switch (sn.ScanData.IsStar)
				{
					case true:
						img = sn.ScanData.GetStarTypeImage(); // if is a star, use the Star image
						break;
					default:
						img = sn.ScanData.GetPlanetClassImage(); // use the correct image in case of planets and moons
						break;
				}

				dataGridViewScanGrid.Rows.Add(new object[] { null, sn.ScanData.BodyName, bdClass, bdDist, bdDetails });

				var cur = dataGridViewScanGrid.Rows[dataGridViewScanGrid.Rows.Count - 1];

				cur.Tag = img;
				cur.Cells[4].Tag = cur.Cells[0].ToolTipText = cur.Cells[1].ToolTipText = cur.Cells[2].ToolTipText = cur.Cells[3].ToolTipText = cur.Cells[4].ToolTipText =
					sn.ScanData.DisplayString(historicmatlist: _lastHe.MaterialCommodity, currentmatlist: discoveryform.history.GetLast?.MaterialCommodity);
			}

            // display total scan values
            SetControlText(string.Format("Scan Summary for {0}. {1}".Tx(this, "SS"), scanNode.system.Name, BuildScanValue(scanNode)));

            if (firstDisplayedScrollingRowIndex >= 0 && firstDisplayedScrollingRowIndex < dataGridViewScanGrid.RowCount)
                dataGridViewScanGrid.FirstDisplayedScrollingRowIndex = firstDisplayedScrollingRowIndex;
        }

        private void dataGridViewScanGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var cur = dataGridViewScanGrid.Rows[e.RowIndex];
			if (cur.Tag == null) return;
			// we programatically draw the image because we have control over its pos/ size this way, which you can't do
			// with a image column - there you can only draw a fixed image or stretch it to cell contents.. which we don't want to do
			var sz = dataGridViewScanGrid.RowTemplate.MinimumHeight - 2;
			var vPos = e.RowBounds.Top + e.RowBounds.Height / 2 - sz / 2;
			e.Graphics.DrawImage((Image)cur.Tag, new Rectangle(e.RowBounds.Left + 1, vPos, sz, sz));
		}

        private string BuildScanValue(StarScan.SystemNode system)
        {
            var value = system.Bodies.Where(body => body?.ScanData?.EstimatedValue != null).Sum(body => body.ScanData.EstimatedValue);

			return string.Format("Approx total scan value: {0:N0}".Tx(this, "AV"), value);
        }

        private void dataGridViewScanGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
			if (e.ColumnIndex != 4) return;
			var curData = dataGridViewScanGrid.Rows[e.RowIndex].Cells[4].Value;
			dataGridViewScanGrid.Rows[e.RowIndex].Cells[4].Value = dataGridViewScanGrid.Rows[e.RowIndex].Cells[4].Tag;
			dataGridViewScanGrid.Rows[e.RowIndex].Cells[4].Tag = curData;
		}

		private void dataGridViewScanGrid_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button != MouseButtons.Right) return;
			contextMenuStripSG.Visible = true;
			contextMenuStripSG.Top = MousePosition.Y;
			contextMenuStripSG.Left = MousePosition.X;
		}

		private void goldilocksZoneToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			_showGoldilocksZone = goldilocksZoneToolStripMenuItem.CheckState == CheckState.Checked;
			SQLiteDBClass.PutSettingBool(DbSave + "ShowGoldilocks", goldilocksZoneToolStripMenuItem.Checked);
			DrawSystem(_lastHe, true);
		}

		private void metalRichToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			_showMetalRichZone = metalRichToolStripMenuItem.CheckState == CheckState.Checked;
			SQLiteDBClass.PutSettingBool(DbSave + "ShowMetalRich", metalRichToolStripMenuItem.Checked);
			DrawSystem(_lastHe, true);
		}
		
		private void waterWorldsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			_showWaterWorldZone = waterWorldsToolStripMenuItem.CheckState == CheckState.Checked;
			SQLiteDBClass.PutSettingBool(DbSave + "ShowWaterWorld", metalRichToolStripMenuItem.Checked);
			DrawSystem(_lastHe, true);
		}

		private void earthLikeToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			_showEarthLikeZone = earthLikeToolStripMenuItem.CheckState == CheckState.Checked;
			SQLiteDBClass.PutSettingBool(DbSave + "ShowEarthLike", earthLikeToolStripMenuItem.Checked);
			DrawSystem(_lastHe, true);
		}

		private void ammoniaWorldsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			_showAmmoniaZone = ammoniaWorldsToolStripMenuItem.CheckState == CheckState.Checked;
			SQLiteDBClass.PutSettingBool(DbSave + "ShowAmmonia", ammoniaWorldsToolStripMenuItem.Checked);
			DrawSystem(_lastHe, true);
		}

		private void showAvailableMaterialsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			_showMaterials = showAvailableMaterialsToolStripMenuItem.CheckState == CheckState.Checked;
			SQLiteDBClass.PutSettingBool(DbSave + "ShowMaterials", showAvailableMaterialsToolStripMenuItem.Checked);
			DrawSystem(_lastHe, true);
		}

		private void showAtmosphericDetailsToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			_showAtmosphere = showAtmosphericDetailsToolStripMenuItem.CheckState == CheckState.Checked;
			SQLiteDBClass.PutSettingBool(DbSave + "ShowAtmosphere", showAtmosphericDetailsToolStripMenuItem.Checked);
			DrawSystem(_lastHe, true);
		}

		private void showRingsInformationToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			_showRings = showRingsInformationToolStripMenuItem.CheckState == CheckState.Checked;
			SQLiteDBClass.PutSettingBool(DbSave + "ShowRings", showRingsInformationToolStripMenuItem.Checked);
			DrawSystem(_lastHe, true);
		}
	}
}
