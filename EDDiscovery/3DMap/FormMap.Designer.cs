using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;

namespace EDDiscovery2
{
        partial class FormMap
        {
            /// <summary>
            /// Required designer variable.
            /// </summary>
            private System.ComponentModel.IContainer components = null;

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMap));
            this.glControl = new OpenTK.GLControl();
            this.textboxFrom = new System.Windows.Forms.TextBox();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.labelSystemCoords = new System.Windows.Forms.Label();
            this.toolStripShowAllStars = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonLastKnownPosition = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDrawLines = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButtonFilterStars = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButtonShowAllStars = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStations = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonStarNames = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFineGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCoords = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonPerspective = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonEliteMovement = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.dropdownMapNames = new System.Windows.Forms.ToolStripDropDownButton();
            this.dropdownFilterDate = new System.Windows.Forms.ToolStripDropDownButton();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.buttonHistory = new System.Windows.Forms.Button();
            this.buttonHome = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.labelClickedSystemCoords = new System.Windows.Forms.Label();
            this.systemselectionMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectionAllegiance = new System.Windows.Forms.ToolStripMenuItem();
            this.selectionEconomy = new System.Windows.Forms.ToolStripMenuItem();
            this.selectionState = new System.Windows.Forms.ToolStripMenuItem();
            this.selectionGov = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dotSelectedSystemCoords = new System.Windows.Forms.PictureBox();
            this.dotSystemCoords = new System.Windows.Forms.PictureBox();
            this.UpdateTimer = new System.Windows.Forms.Timer(this.components);
            this.panelRight = new System.Windows.Forms.Panel();
            this.toolStripShowAllStars.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.systemselectionMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dotSelectedSystemCoords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotSystemCoords)).BeginInit();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Location = new System.Drawing.Point(0, 44);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(984, 693);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = true;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.DoubleClick += new System.EventHandler(this.glControl_DoubleClick);
            this.glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyDown);
            this.glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(this.glControl_KeyUp);
            this.glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseDown);
            this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseMove);
            this.glControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseUp);
            this.glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl_OnMouseWheel);
            this.glControl.Resize += new System.EventHandler(this.glControl_Resize);
            // 
            // textboxFrom
            // 
            this.textboxFrom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textboxFrom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textboxFrom.Location = new System.Drawing.Point(3, 9);
            this.textboxFrom.Name = "textboxFrom";
            this.textboxFrom.Size = new System.Drawing.Size(125, 20);
            this.textboxFrom.TabIndex = 16;
            this.textboxFrom.TabStop = false;
            this.textboxFrom.Text = "Sol";
            this.toolTip1.SetToolTip(this.textboxFrom, "Home System");
            // 
            // buttonCenter
            // 
            this.buttonCenter.Location = new System.Drawing.Point(134, 7);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(47, 23);
            this.buttonCenter.TabIndex = 17;
            this.buttonCenter.TabStop = false;
            this.buttonCenter.Text = "Center";
            this.toolTip1.SetToolTip(this.buttonCenter, "Center map on system");
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // labelSystemCoords
            // 
            this.labelSystemCoords.AutoSize = true;
            this.labelSystemCoords.Location = new System.Drawing.Point(260, 3);
            this.labelSystemCoords.Name = "labelSystemCoords";
            this.labelSystemCoords.Size = new System.Drawing.Size(57, 13);
            this.labelSystemCoords.TabIndex = 18;
            this.labelSystemCoords.Text = "Sol x=0.00";
            // 
            // toolStripShowAllStars
            // 
            this.toolStripShowAllStars.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.toolStripShowAllStars.AutoSize = false;
            this.toolStripShowAllStars.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStripShowAllStars.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonLastKnownPosition,
            this.toolStripButtonDrawLines,
            this.toolStripDropDownButtonFilterStars,
            this.toolStripButtonStarNames,
            this.toolStripButtonGrid,
            this.toolStripButtonFineGrid,
            this.toolStripButtonCoords,
            this.toolStripSeparator3,
            this.toolStripButtonPerspective,
            this.toolStripSeparator1,
            this.toolStripButtonEliteMovement,
            this.toolStripSeparator2,
            this.dropdownMapNames,
            this.dropdownFilterDate});
            this.toolStripShowAllStars.Location = new System.Drawing.Point(0, 0);
            this.toolStripShowAllStars.Name = "toolStripShowAllStars";
            this.toolStripShowAllStars.Size = new System.Drawing.Size(397, 40);
            this.toolStripShowAllStars.TabIndex = 19;
            this.toolStripShowAllStars.Text = "toolStrip1";
            // 
            // toolStripButtonLastKnownPosition
            // 
            this.toolStripButtonLastKnownPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLastKnownPosition.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLastKnownPosition.Image")));
            this.toolStripButtonLastKnownPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLastKnownPosition.Name = "toolStripButtonLastKnownPosition";
            this.toolStripButtonLastKnownPosition.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonLastKnownPosition.Text = "Last known position";
            this.toolStripButtonLastKnownPosition.Click += new System.EventHandler(this.toolStripLastKnownPosition_Click);
            // 
            // toolStripButtonDrawLines
            // 
            this.toolStripButtonDrawLines.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawLines.BackgroundImage")));
            this.toolStripButtonDrawLines.CheckOnClick = true;
            this.toolStripButtonDrawLines.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonDrawLines.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDrawLines.Image")));
            this.toolStripButtonDrawLines.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDrawLines.Name = "toolStripButtonDrawLines";
            this.toolStripButtonDrawLines.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonDrawLines.Text = "Draw lines";
            this.toolStripButtonDrawLines.Click += new System.EventHandler(this.toolStripButtonDrawLines_Click);
            // 
            // toolStripDropDownButtonFilterStars
            // 
            this.toolStripDropDownButtonFilterStars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonFilterStars.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonShowAllStars,
            this.toolStripButtonStations,
            this.toolStripSeparator4});
            this.toolStripDropDownButtonFilterStars.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonFilterStars.Image")));
            this.toolStripDropDownButtonFilterStars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonFilterStars.Name = "toolStripDropDownButtonFilterStars";
            this.toolStripDropDownButtonFilterStars.Size = new System.Drawing.Size(29, 37);
            this.toolStripDropDownButtonFilterStars.Text = "Filter Stars";
            // 
            // toolStripButtonShowAllStars
            // 
            this.toolStripButtonShowAllStars.Checked = true;
            this.toolStripButtonShowAllStars.CheckOnClick = true;
            this.toolStripButtonShowAllStars.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonShowAllStars.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowAllStars.Image")));
            this.toolStripButtonShowAllStars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowAllStars.Name = "toolStripButtonShowAllStars";
            this.toolStripButtonShowAllStars.Size = new System.Drawing.Size(98, 20);
            this.toolStripButtonShowAllStars.Text = "Show all stars";
            this.toolStripButtonShowAllStars.Click += new System.EventHandler(this.toolStripButtonShowAllStars_Click);
            // 
            // toolStripButtonStations
            // 
            this.toolStripButtonStations.Checked = true;
            this.toolStripButtonStations.CheckOnClick = true;
            this.toolStripButtonStations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonStations.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStations.Image")));
            this.toolStripButtonStations.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonStations.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStations.Name = "toolStripButtonStations";
            this.toolStripButtonStations.Size = new System.Drawing.Size(77, 28);
            this.toolStripButtonStations.Text = "Stations";
            this.toolStripButtonStations.Click += new System.EventHandler(this.toolStripButtonStations_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(155, 6);
            // 
            // toolStripButtonStarNames
            // 
            this.toolStripButtonStarNames.CheckOnClick = true;
            this.toolStripButtonStarNames.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStarNames.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStarNames.Image")));
            this.toolStripButtonStarNames.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStarNames.Name = "toolStripButtonStarNames";
            this.toolStripButtonStarNames.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonStarNames.Text = "Star Names";
            this.toolStripButtonStarNames.ToolTipText = "Enable Star Naming at higher zoom levels";
            this.toolStripButtonStarNames.Click += new System.EventHandler(this.toolStripButtonStarNames_Click);
            // 
            // toolStripButtonGrid
            // 
            this.toolStripButtonGrid.CheckOnClick = true;
            this.toolStripButtonGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGrid.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGrid.Image")));
            this.toolStripButtonGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGrid.Name = "toolStripButtonGrid";
            this.toolStripButtonGrid.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonGrid.Text = "Grid";
            this.toolStripButtonGrid.ToolTipText = "Show Coarse Grid";
            this.toolStripButtonGrid.Click += new System.EventHandler(this.toolStripButtonGrid_Click);
            // 
            // toolStripButtonFineGrid
            // 
            this.toolStripButtonFineGrid.CheckOnClick = true;
            this.toolStripButtonFineGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFineGrid.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFineGrid.Image")));
            this.toolStripButtonFineGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFineGrid.Name = "toolStripButtonFineGrid";
            this.toolStripButtonFineGrid.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonFineGrid.Text = "Grid";
            this.toolStripButtonFineGrid.ToolTipText = "Show Fine Grid";
            this.toolStripButtonFineGrid.Click += new System.EventHandler(this.toolStripButtonFineGrid_Click);
            // 
            // toolStripButtonCoords
            // 
            this.toolStripButtonCoords.CheckOnClick = true;
            this.toolStripButtonCoords.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCoords.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonCoords.Image")));
            this.toolStripButtonCoords.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCoords.Name = "toolStripButtonCoords";
            this.toolStripButtonCoords.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonCoords.Text = "Grid";
            this.toolStripButtonCoords.ToolTipText = "Show Grid Coordinates";
            this.toolStripButtonCoords.Click += new System.EventHandler(this.toolStripButtonCoords_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 40);
            // 
            // toolStripButtonPerspective
            // 
            this.toolStripButtonPerspective.CheckOnClick = true;
            this.toolStripButtonPerspective.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPerspective.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPerspective.Image")));
            this.toolStripButtonPerspective.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPerspective.Name = "toolStripButtonPerspective";
            this.toolStripButtonPerspective.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonPerspective.Text = "Perspective";
            this.toolStripButtonPerspective.ToolTipText = "Show Perspective View";
            this.toolStripButtonPerspective.Click += new System.EventHandler(this.toolStripButtonPerspective_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 40);
            // 
            // toolStripButtonEliteMovement
            // 
            this.toolStripButtonEliteMovement.CheckOnClick = true;
            this.toolStripButtonEliteMovement.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonEliteMovement.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonEliteMovement.Image")));
            this.toolStripButtonEliteMovement.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonEliteMovement.Name = "toolStripButtonEliteMovement";
            this.toolStripButtonEliteMovement.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonEliteMovement.Text = "Grid";
            this.toolStripButtonEliteMovement.ToolTipText = "Elite Movement Mode (Perspective only, ASWD does not affect Y)";
            this.toolStripButtonEliteMovement.Click += new System.EventHandler(this.toolStripButtonEliteMovement_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 40);
            // 
            // dropdownMapNames
            // 
            this.dropdownMapNames.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dropdownMapNames.Image = ((System.Drawing.Image)(resources.GetObject("dropdownMapNames.Image")));
            this.dropdownMapNames.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dropdownMapNames.Name = "dropdownMapNames";
            this.dropdownMapNames.Size = new System.Drawing.Size(29, 37);
            this.dropdownMapNames.Text = "Select Maps";
            // 
            // dropdownFilterDate
            // 
            this.dropdownFilterDate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.dropdownFilterDate.Image = ((System.Drawing.Image)(resources.GetObject("dropdownFilterDate.Image")));
            this.dropdownFilterDate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dropdownFilterDate.Name = "dropdownFilterDate";
            this.dropdownFilterDate.Size = new System.Drawing.Size(29, 37);
            this.dropdownFilterDate.Text = "Filter by Expedition or date";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 740);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(984, 22);
            this.statusStrip.TabIndex = 21;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 17);
            this.statusLabel.Text = "x=0.0";
            // 
            // buttonHistory
            // 
            this.buttonHistory.Image = global::EDDiscovery.Properties.Resources.Travelicon;
            this.buttonHistory.Location = new System.Drawing.Point(215, 7);
            this.buttonHistory.Name = "buttonHistory";
            this.buttonHistory.Size = new System.Drawing.Size(22, 23);
            this.buttonHistory.TabIndex = 23;
            this.toolTip1.SetToolTip(this.buttonHistory, "Centre map on selected system from Travel History");
            this.buttonHistory.UseVisualStyleBackColor = true;
            this.buttonHistory.Click += new System.EventHandler(this.buttonHistory_Click);
            // 
            // buttonHome
            // 
            this.buttonHome.Image = global::EDDiscovery.Properties.Resources.Homeicon;
            this.buttonHome.Location = new System.Drawing.Point(187, 7);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(22, 23);
            this.buttonHome.TabIndex = 22;
            this.toolTip1.SetToolTip(this.buttonHome, "Centre map on Home System");
            this.buttonHome.UseVisualStyleBackColor = true;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // labelClickedSystemCoords
            // 
            this.labelClickedSystemCoords.AutoSize = true;
            this.labelClickedSystemCoords.Location = new System.Drawing.Point(260, 22);
            this.labelClickedSystemCoords.Name = "labelClickedSystemCoords";
            this.labelClickedSystemCoords.Size = new System.Drawing.Size(57, 13);
            this.labelClickedSystemCoords.TabIndex = 24;
            this.labelClickedSystemCoords.Text = "Sol x=0.00";
            this.labelClickedSystemCoords.Click += new System.EventHandler(this.labelClickedSystemCoords_Click);
            // 
            // systemselectionMenuStrip
            // 
            this.systemselectionMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectionAllegiance,
            this.selectionEconomy,
            this.selectionState,
            this.selectionGov,
            this.viewOnEDSMToolStripMenuItem});
            this.systemselectionMenuStrip.Name = "systemselectionMenuStrip";
            this.systemselectionMenuStrip.Size = new System.Drawing.Size(151, 114);
            // 
            // selectionAllegiance
            // 
            this.selectionAllegiance.Name = "selectionAllegiance";
            this.selectionAllegiance.Size = new System.Drawing.Size(150, 22);
            this.selectionAllegiance.Text = "Allegiance";
            // 
            // selectionEconomy
            // 
            this.selectionEconomy.Name = "selectionEconomy";
            this.selectionEconomy.Size = new System.Drawing.Size(150, 22);
            this.selectionEconomy.Text = "Economy";
            // 
            // selectionState
            // 
            this.selectionState.Name = "selectionState";
            this.selectionState.Size = new System.Drawing.Size(150, 22);
            this.selectionState.Text = "State";
            // 
            // selectionGov
            // 
            this.selectionGov.Name = "selectionGov";
            this.selectionGov.Size = new System.Drawing.Size(150, 22);
            this.selectionGov.Text = "Gov";
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // dotSelectedSystemCoords
            // 
            this.dotSelectedSystemCoords.Image = global::EDDiscovery.Properties.Resources.OrangeDot;
            this.dotSelectedSystemCoords.InitialImage = global::EDDiscovery.Properties.Resources.OrangeDot;
            this.dotSelectedSystemCoords.Location = new System.Drawing.Point(242, 22);
            this.dotSelectedSystemCoords.Name = "dotSelectedSystemCoords";
            this.dotSelectedSystemCoords.Size = new System.Drawing.Size(12, 12);
            this.dotSelectedSystemCoords.TabIndex = 26;
            this.dotSelectedSystemCoords.TabStop = false;
            // 
            // dotSystemCoords
            // 
            this.dotSystemCoords.Image = global::EDDiscovery.Properties.Resources.YellowDot;
            this.dotSystemCoords.InitialImage = global::EDDiscovery.Properties.Resources.YellowDot;
            this.dotSystemCoords.Location = new System.Drawing.Point(242, 4);
            this.dotSystemCoords.Name = "dotSystemCoords";
            this.dotSystemCoords.Size = new System.Drawing.Size(12, 12);
            this.dotSystemCoords.TabIndex = 25;
            this.dotSystemCoords.TabStop = false;
            // 
            // UpdateTimer
            // 
            this.UpdateTimer.Tick += new System.EventHandler(this.UpdateTimer_Tick);
            // 
            // panelRight
            // 
            this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight.Controls.Add(this.labelClickedSystemCoords);
            this.panelRight.Controls.Add(this.dotSelectedSystemCoords);
            this.panelRight.Controls.Add(this.textboxFrom);
            this.panelRight.Controls.Add(this.buttonCenter);
            this.panelRight.Controls.Add(this.labelSystemCoords);
            this.panelRight.Controls.Add(this.dotSystemCoords);
            this.panelRight.Controls.Add(this.buttonHome);
            this.panelRight.Controls.Add(this.buttonHistory);
            this.panelRight.Location = new System.Drawing.Point(398, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(586, 40);
            this.panelRight.TabIndex = 27;
            // 
            // FormMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 762);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.glControl);
            this.Controls.Add(this.toolStripShowAllStars);
            this.Controls.Add(this.panelRight);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "3D Star Map";
            this.Activated += new System.EventHandler(this.FormMap_Activated);
            this.Deactivate += new System.EventHandler(this.FormMap_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMap_FormClosing);
            this.Load += new System.EventHandler(this.FormMap_Load);
            this.toolStripShowAllStars.ResumeLayout(false);
            this.toolStripShowAllStars.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.systemselectionMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dotSelectedSystemCoords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotSystemCoords)).EndInit();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }


            #endregion

            private OpenTK.GLControl glControl;
            internal TextBox textboxFrom;
            private Button buttonCenter;
            private Label labelSystemCoords;
        private ToolStrip toolStripShowAllStars;
        private ToolStripButton toolStripButtonLastKnownPosition;
        private ToolStripButton toolStripButtonDrawLines;
        private ToolStripButton toolStripButtonShowAllStars;
        private ToolStripButton toolStripButtonStations;
        private ToolStripButton toolStripButtonGrid;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private Button buttonHome;
        private Button buttonHistory;
        private ToolTip toolTip1;
        private ToolStripButton toolStripButtonPerspective;
        private Label labelClickedSystemCoords;
        private PictureBox dotSystemCoords;
        private PictureBox dotSelectedSystemCoords;
        private Timer UpdateTimer;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripDropDownButton dropdownMapNames;
        private ToolStripDropDownButton dropdownFilterDate;
        private ContextMenuStrip systemselectionMenuStrip;
        private ToolStripMenuItem selectionAllegiance;
        private ToolStripMenuItem selectionEconomy;
        private ToolStripMenuItem selectionState;
        private ToolStripMenuItem selectionGov;
        private ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private ToolStripButton toolStripButtonFineGrid;
        private ToolStripButton toolStripButtonCoords;
        private ToolStripButton toolStripButtonEliteMovement;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private Panel panelRight;
        private ToolStripDropDownButton toolStripDropDownButtonFilterStars;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripButton toolStripButtonStarNames;
    }
    }
