using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;
using EliteDangerousCore.DB;
using EliteDangerousCore;
using EliteDangerousCore.EDSM;
using static EliteDangerousCore.DB.SystemClassDB;

namespace EDDiscovery.UserControls
{
	public partial class UserControlDioptra : UserControlCommonBase
	{
		/// <summary>
		/// This UserControl calculated distance from the current system to user defined reference systems.
		///
		/// Pseudo-Code description:
		/// 1) Systems are added to and removed from a list from a context menu.
		/// 2) This list is used to populated various instances of RefSystem class.
		/// 3) For each RefSystem class, the distance is calculated and printed.
		/// 4) RefSystem must be present in the systems.db to be added (autocomplete text box).
		/// </summary>
	
		// reference in userDB
		private string DbSave => DBName("DioptraPanel" );
		
		private enum UIState { Normal, SystemMap, GalMap };
		private UIState _uistate = UIState.Normal;

		private Font displayfont;

		// define configuration fields
		public class Configuration
		{
			public const long ShowCurrentSystemName = 1;
			public const long ShowCurrentSystemCoord = 2;
			public const long ShowTarget = 4;
			public const long ShowHome = 8;
			public const long ShowSol = 16;
			public const long ShowColonia = 32;
			public const long ShowSagA = 64;
			public const long DoNotShowIfDocked = 128;
			public const long DoNotShowIfSysMap = 256;
			public const long DoNotShowIfGalMap = 512;
			public const long ShowBlackBoxText = 1024;
		}

		// configuration fields initialization
		private long _config = Configuration.ShowCurrentSystemName | Configuration.ShowCurrentSystemCoord |
							  Configuration.ShowTarget | Configuration.ShowHome | Configuration.ShowSol |
							  Configuration.ShowColonia | Configuration.ShowSagA;

		private bool Config(long c) { return (_config & c) != 0; }

		public UserControlDioptra()
		{
			InitializeComponent();
		}
		
		public override void Init()
		{
			_config = (long)(SQLiteDBClass.GetSettingInt(DbSave + "Config", (int)_config)) | ((long)(SQLiteDBClass.GetSettingInt(DbSave + "ConfigH", (int)(_config >> 32))) << 32);

			// use the font configured
			displayfont = discoveryform.theme.GetFont;

			pictureBox.ContextMenuStrip = contextMenuStripDioptre;
		}

		// retrieve the name and coordinates of the current system
		private void Uctg_OnTravelSelectionChanged(HistoryEntry he, HistoryList hl)
		{
			SetControlText(he.System.Name);
			
			labelCurrentSystem.Text = he.System.Name;

			// Check if the system has coordinates
			if (he.System.HasCoordinate)         
			{
				const string singleCoordinateFormat = "0.##";
				
				const string labelX = "X: ";
				var labelY = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " Y: ";
				var labelZ = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + " Z: ";

				curSysCoordinates.Text = labelX + he.System.X.ToString(singleCoordinateFormat) + labelY + he.System.Y.ToString(singleCoordinateFormat) + labelZ + he.System.Z.ToString(singleCoordinateFormat);

				var homeSystem = EDDConfig.Instance.HomeSystem;

				if (homeSystem.Name != "Sol")
				{
					panelHome.Visible = true;
					homeToolStripMenuItem.Visible = true;

					// Home panel
					labelHomeName.Text = homeSystem.Name;
					labelHomeDist.Text = he.System.Distance(homeSystem).ToString(singleCoordinateFormat);
				}
				else
				{
					panelHome.Visible = false;
					homeToolStripMenuItem.Enabled = false;
				}
				
				if (homeSystem.Name != "Colonia")
				{
					panelColonia.Visible = true;

					// Home panel
					labelColoniaName.Text = @"Colonia";
					labelColoniaDist.Text = he.System.Distance(-9530.5,-910.28125,19808.125).ToString(singleCoordinateFormat);
				}
				else
					panelHome.Visible = false;
				
				// Sol panel
				labelSolName.Text = @"Sol";
				labelSolDist.Text = he.System.Distance(0, 0, 0).ToString(singleCoordinateFormat);

				// SagA* panel
				labelSagAName.Text = @"Sagittarius A*";
				labelSagADist.Text = he.System.Distance(25.21875, -20.90625, 25899.96875).ToString(singleCoordinateFormat);

				// Target panel
				if (TargetClass.GetTargetPosition(out var name, out var x, out var y, out var z))
				{
					panelTarget.Visible = true;

					labelTargetName.Text = name;
					labelTargetDist.Text = he.System.Distance(x, y, z).ToString(singleCoordinateFormat);
				}
				else
				{
					panelTarget.Visible = false;
				}
			}
			else
			{
				curSysCoordinates.Text = @"Current system has not known coordinates!";
				panelHome.Visible = false;
				panelSol.Visible = false;
			}
		}

		public static ISystem CurrentSystem { get { HistoryEntry he = GetLast; return (he != null) ? he.System : null; } }  // current system
		public static HistoryEntry GetLast { get; private set; }

		private static void AddCustomSystemPanel(string name)
		{
			ISystem sys = SystemCache.FindSystem(name, null);
			ISystem cursys = CurrentSystem;

			var panelName = "panel" + sys.Name;
			var labelName = "label" + sys.Name + "Name";
			var labelDist = "label" + sys.Name + "Dist";

			var panelSysName = new Panel();
			var labelSysName = new Label();
			var labelSysDist = new Label();

			const string singleCoordinateFormat = "0.##";
			panelSysName.Name = panelName;
			labelSysName.Text = labelName;
			labelSysDist.Text = cursys.Distance(sys.X, sys.Y, sys.Z).ToString(singleCoordinateFormat);

			panelSysName.Controls.Add(labelSysName);
			panelSysName.Controls.Add(labelSysDist);
		}

		public override void Closing()
		{
			SQLiteDBClass.PutSettingInt(DbSave + "Config", (int)_config);
			SQLiteDBClass.PutSettingInt(DbSave + "ConfigH", (int)(_config>>32));
		}

		public override Color ColorTransparency => Color.Green;

		public override void SetTransparency(bool on, Color currentColor)
		{
			pictureBox.BackColor = this.BackColor = currentColor;
		}

		/// <summary>
		/// Deal with the display.
		/// Also deal with transparencies and hiding of the control when in special UI state.
		/// </summary>
		/// <param name="hl"></param>
		private void Display(HistoryList hl)
		{
			var textColor = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
			var backColor = IsTransparent ? (Config(Configuration.ShowBlackBoxText) ? Color.Black : Color.Transparent) : this.BackColor;
			
			if (Config(Configuration.DoNotShowIfDocked) && (hl.IsCurrentlyDocked || hl.IsCurrentlyLanded))
			{
				//AddColText(0, 0, rowpos, rowheight, (hl.IsCurrentlyDocked) ? "Docked" : "Landed", textcolour, backcolour, null);
			}
			else if ( ( _uistate == UIState.GalMap && Config(Configuration.DoNotShowIfGalMap)) || ( _uistate == UIState.SystemMap && Config(Configuration.DoNotShowIfSysMap)))
			{
				//AddColText(0, 0, rowpos, rowheight, (uistate == UIState.GalMap) ? "Galaxy Map" : "System Map", textcolour, backcolour, null);
			}
		}

		// update the current system reference whenever the cursor moves to another history entry
		public override void ChangeCursorType(IHistoryCursor thc)
		{
			uctg.OnTravelSelectionChanged -= Uctg_OnTravelSelectionChanged;
			uctg = thc;
			uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
		}

		public override void LoadLayout()
		{
			uctg.OnTravelSelectionChanged += Uctg_OnTravelSelectionChanged;
		}
		
		/// <summary>
		/// Context menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		
		private void pictureBox_ClickElement(object sender, MouseEventArgs eventargs, ExtendedControls.PictureBoxHotspot.ImageElement i, object tag)
		{
			
		}

		private void flowLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
		{
			// Open the context menu
			if (e.Button == MouseButtons.Right)
			{
				contextMenuStripDioptre.Show(MousePosition.X, MousePosition.Y);
			}
		}

		private void pictureBox_MouseClick(object sender, MouseEventArgs e)
		{
			// Open the context menu
			if (e.Button == MouseButtons.Right)
			{
				contextMenuStripDioptre.Show(MousePosition.X, MousePosition.Y);
			}
		}

		private void showSystemNameToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			labelCurrentSystem.Visible = showSystemNameToolStripMenuItem.Checked != false;
		}

		private void showCoordinatesToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			curSysCoordinates.Visible = showCoordinatesToolStripMenuItem.Checked != false;
		}

		private void targetToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			panelTarget.Visible = targetToolStripMenuItem.Checked != false;
		}

		private void solToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			panelSol.Visible = solToolStripMenuItem.Checked != false;
		}

		private void sagAToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			panelSagA.Visible = sagAToolStripMenuItem.Checked != false;
		}

		private void coloniaToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			panelColonia.Visible = coloniaToolStripMenuItem.Checked != false;
		}
		
		private void homeToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			panelHome.Visible = homeToolStripMenuItem.Checked != false;
		}

		private void toolStripTextBox1_Validated(object sender, EventArgs e)
		{
			//AddCustomSystemPanel("Arine");
		}

		private void topDownToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			if (!topDownToolStripMenuItem.Checked) return;
			leftToRightToolStripMenuItem.Checked = false;
			rightToLeftToolStripMenuItem.Checked = false;
			downTopToolStripMenuItem.Checked = false;
			flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
		}

		private void leftToRightToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			if (!leftToRightToolStripMenuItem.Checked) return;
			topDownToolStripMenuItem.Checked = false;
			rightToLeftToolStripMenuItem.Checked = false;
			downTopToolStripMenuItem.Checked = false;
			flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
		}

		private void rightToLeftToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			if (!rightToLeftToolStripMenuItem.Checked) return;
			leftToRightToolStripMenuItem.Checked = false;
			topDownToolStripMenuItem.Checked = false;
			downTopToolStripMenuItem.Checked = false;
			flowLayoutPanel1.FlowDirection = FlowDirection.RightToLeft;
		}

		private void downTopToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
		{
			if (!downTopToolStripMenuItem.Checked) return;
			leftToRightToolStripMenuItem.Checked = false;
			rightToLeftToolStripMenuItem.Checked = false;
			topDownToolStripMenuItem.Checked = false;
			flowLayoutPanel1.FlowDirection = FlowDirection.BottomUp;
		}
	}
}
