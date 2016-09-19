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
            this.textboxFrom = new ExtendedControls.AutoCompleteTextBox();
            this.labelSystemCoords = new System.Windows.Forms.Label();
            this.toolStripShowAllStars = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonGoBackward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonGoForward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLastKnownPosition = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAutoForward = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHome = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonHistory = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonTarget = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButtonVisitedStars = new System.Windows.Forms.ToolStripDropDownButton();
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.drawADiscOnStarsWithPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonFilterStars = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.showStarstoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showStationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enableColoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonNameStars = new System.Windows.Forms.ToolStripDropDownButton();
            this.showDiscsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNamesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonBookmarks = new System.Windows.Forms.ToolStripDropDownButton();
            this.showBookmarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNoteMarksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newRegionBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButtonGalObjects = new System.Windows.Forms.ToolStripDropDownButton();
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
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonHelp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownRecord = new System.Windows.Forms.ToolStripDropDownButton();
            this.recordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordStepToStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newRecordStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pauseRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemClearRecording = new System.Windows.Forms.ToolStripMenuItem();
            this.playbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dotSelectedSystemCoords = new System.Windows.Forms.PictureBox();
            this.dotSystemCoords = new System.Windows.Forms.PictureBox();
            this.buttonCenter = new System.Windows.Forms.Button();
            this.buttonLookAt = new System.Windows.Forms.Button();
            this.labelClickedSystemCoords = new System.Windows.Forms.Label();
            this.systemselectionMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelRight = new System.Windows.Forms.Panel();
            this.toolStripShowAllStars.SuspendLayout();
            this.statusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dotSelectedSystemCoords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotSystemCoords)).BeginInit();
            this.systemselectionMenuStrip.SuspendLayout();
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
            this.glControl.Size = new System.Drawing.Size(1114, 753);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = true;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.DoubleClick += new System.EventHandler(this.glControl_DoubleClick);
            this.glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseDown);
            this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseMove);
            this.glControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseUp);
            this.glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl_OnMouseWheel);
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
            this.toolTip1.SetToolTip(this.textboxFrom, "Enter system to centre on");
            // 
            // labelSystemCoords
            // 
            this.labelSystemCoords.AutoSize = true;
            this.labelSystemCoords.Location = new System.Drawing.Point(211, 3);
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
            this.toolStripButtonGoBackward,
            this.toolStripButtonGoForward,
            this.toolStripButtonLastKnownPosition,
            this.toolStripButtonAutoForward,
            this.toolStripButtonHome,
            this.toolStripButtonHistory,
            this.toolStripButtonTarget,
            this.toolStripSeparator5,
            this.toolStripDropDownButtonVisitedStars,
            this.toolStripDropDownButtonFilterStars,
            this.toolStripDropDownButtonNameStars,
            this.toolStripDropDownButtonBookmarks,
            this.toolStripDropDownButtonGalObjects,
            this.toolStripButtonGrid,
            this.toolStripButtonFineGrid,
            this.toolStripButtonCoords,
            this.toolStripSeparator3,
            this.toolStripButtonPerspective,
            this.toolStripSeparator1,
            this.toolStripButtonEliteMovement,
            this.toolStripSeparator2,
            this.dropdownMapNames,
            this.dropdownFilterDate,
            this.toolStripSeparator6,
            this.toolStripButtonHelp,
            this.toolStripSeparator7,
            this.toolStripDropDownRecord});
            this.toolStripShowAllStars.Location = new System.Drawing.Point(0, 0);
            this.toolStripShowAllStars.Name = "toolStripShowAllStars";
            this.toolStripShowAllStars.Size = new System.Drawing.Size(589, 40);
            this.toolStripShowAllStars.TabIndex = 19;
            this.toolStripShowAllStars.Text = "toolStrip1";
            // 
            // toolStripButtonGoBackward
            // 
            this.toolStripButtonGoBackward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGoBackward.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGoBackward.Image")));
            this.toolStripButtonGoBackward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGoBackward.Name = "toolStripButtonGoBackward";
            this.toolStripButtonGoBackward.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonGoBackward.Text = "Go Backward in Travel History";
            this.toolStripButtonGoBackward.Click += new System.EventHandler(this.toolStripButtonGoBackward_Click);
            // 
            // toolStripButtonGoForward
            // 
            this.toolStripButtonGoForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonGoForward.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGoForward.Image")));
            this.toolStripButtonGoForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGoForward.Name = "toolStripButtonGoForward";
            this.toolStripButtonGoForward.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonGoForward.Text = "Go Forward in Travel History";
            this.toolStripButtonGoForward.Click += new System.EventHandler(this.toolStripButtonGoForward_Click);
            // 
            // toolStripButtonLastKnownPosition
            // 
            this.toolStripButtonLastKnownPosition.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonLastKnownPosition.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLastKnownPosition.Image")));
            this.toolStripButtonLastKnownPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLastKnownPosition.Name = "toolStripButtonLastKnownPosition";
            this.toolStripButtonLastKnownPosition.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonLastKnownPosition.Text = "Go to latest position from travel history";
            this.toolStripButtonLastKnownPosition.Click += new System.EventHandler(this.toolStripLastKnownPosition_Click);
            // 
            // toolStripButtonAutoForward
            // 
            this.toolStripButtonAutoForward.Checked = true;
            this.toolStripButtonAutoForward.CheckOnClick = true;
            this.toolStripButtonAutoForward.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonAutoForward.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonAutoForward.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutoForward.Image")));
            this.toolStripButtonAutoForward.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoForward.Name = "toolStripButtonAutoForward";
            this.toolStripButtonAutoForward.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonAutoForward.Text = "Go Forward Automatically on Jump";
            this.toolStripButtonAutoForward.Click += new System.EventHandler(this.toolStripButtonAutoForward_Click);
            // 
            // toolStripButtonHome
            // 
            this.toolStripButtonHome.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHome.Image = global::EDDiscovery.Properties.Resources.Homeicon;
            this.toolStripButtonHome.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHome.Name = "toolStripButtonHome";
            this.toolStripButtonHome.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonHome.Text = "toolStripButton1";
            this.toolStripButtonHome.ToolTipText = "Go to Home System";
            this.toolStripButtonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // toolStripButtonHistory
            // 
            this.toolStripButtonHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHistory.Image = global::EDDiscovery.Properties.Resources.Travelicon;
            this.toolStripButtonHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHistory.Name = "toolStripButtonHistory";
            this.toolStripButtonHistory.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonHistory.Text = "toolStripButton2";
            this.toolStripButtonHistory.ToolTipText = "Go to system selected in travelled history window";
            this.toolStripButtonHistory.Click += new System.EventHandler(this.buttonHistory_Click);
            // 
            // toolStripButtonTarget
            // 
            this.toolStripButtonTarget.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonTarget.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonTarget.Image")));
            this.toolStripButtonTarget.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonTarget.Name = "toolStripButtonTarget";
            this.toolStripButtonTarget.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonTarget.Text = "toolStripButton1";
            this.toolStripButtonTarget.ToolTipText = "Go to target designator";
            this.toolStripButtonTarget.Click += new System.EventHandler(this.toolStripButtonTarget_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 40);
            // 
            // toolStripDropDownButtonVisitedStars
            // 
            this.toolStripDropDownButtonVisitedStars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonVisitedStars.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem,
            this.drawADiscOnStarsWithPositionToolStripMenuItem,
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem});
            this.toolStripDropDownButtonVisitedStars.Image = global::EDDiscovery.Properties.Resources.ImageTravel;
            this.toolStripDropDownButtonVisitedStars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonVisitedStars.Name = "toolStripDropDownButtonVisitedStars";
            this.toolStripDropDownButtonVisitedStars.Size = new System.Drawing.Size(29, 37);
            this.toolStripDropDownButtonVisitedStars.Text = "toolStripDropDownButtonVisitedStars";
            this.toolStripDropDownButtonVisitedStars.ToolTipText = "Select how your travel history is displayed";
            // 
            // drawLinesBetweenStarsWithPositionToolStripMenuItem
            // 
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem.Checked = true;
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem.CheckOnClick = true;
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.ImageTravel;
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem.Name = "drawLinesBetweenStarsWithPositionToolStripMenuItem";
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem.Size = new System.Drawing.Size(342, 22);
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem.Text = "Draw lines between stars with position";
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem.ToolTipText = "Lines connect, in the Map Colour, the stars with known position";
            this.drawLinesBetweenStarsWithPositionToolStripMenuItem.Click += new System.EventHandler(this.drawLinesBetweenStarsWithPositionToolStripMenuItem_Click);
            // 
            // drawADiscOnStarsWithPositionToolStripMenuItem
            // 
            this.drawADiscOnStarsWithPositionToolStripMenuItem.Checked = true;
            this.drawADiscOnStarsWithPositionToolStripMenuItem.CheckOnClick = true;
            this.drawADiscOnStarsWithPositionToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.drawADiscOnStarsWithPositionToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.ImageStarDisc;
            this.drawADiscOnStarsWithPositionToolStripMenuItem.Name = "drawADiscOnStarsWithPositionToolStripMenuItem";
            this.drawADiscOnStarsWithPositionToolStripMenuItem.Size = new System.Drawing.Size(342, 22);
            this.drawADiscOnStarsWithPositionToolStripMenuItem.Text = "Draw a disc on stars with position";
            this.drawADiscOnStarsWithPositionToolStripMenuItem.ToolTipText = "Stars with position are shown as discs";
            this.drawADiscOnStarsWithPositionToolStripMenuItem.Click += new System.EventHandler(this.drawADiscOnStarsWithPositionToolStripMenuItem_Click);
            // 
            // useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem
            // 
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.Checked = true;
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.CheckOnClick = true;
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.ImageStarDiscWhite;
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.Name = "useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem";
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.Size = new System.Drawing.Size(342, 22);
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.Text = "Use White for discs instead of assigned map colour";
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.ToolTipText = "Instead of using the map colour, use white. Useful when you have the lines on at " +
    "the same time";
            this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem.Click += new System.EventHandler(this.useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem_Click);
            // 
            // toolStripDropDownButtonFilterStars
            // 
            this.toolStripDropDownButtonFilterStars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonFilterStars.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator4,
            this.showStarstoolStripMenuItem,
            this.showStationsToolStripMenuItem,
            this.enableColoursToolStripMenuItem});
            this.toolStripDropDownButtonFilterStars.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonFilterStars.Image")));
            this.toolStripDropDownButtonFilterStars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonFilterStars.Name = "toolStripDropDownButtonFilterStars";
            this.toolStripDropDownButtonFilterStars.Size = new System.Drawing.Size(29, 37);
            this.toolStripDropDownButtonFilterStars.Text = "Filter Stars";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(150, 6);
            // 
            // showStarstoolStripMenuItem
            // 
            this.showStarstoolStripMenuItem.Checked = true;
            this.showStarstoolStripMenuItem.CheckOnClick = true;
            this.showStarstoolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showStarstoolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showStarstoolStripMenuItem.Image")));
            this.showStarstoolStripMenuItem.Name = "showStarstoolStripMenuItem";
            this.showStarstoolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.showStarstoolStripMenuItem.Text = "Show All Stars";
            this.showStarstoolStripMenuItem.ToolTipText = "Show all stars";
            this.showStarstoolStripMenuItem.Click += new System.EventHandler(this.showStarstoolStripMenuItem_Click);
            // 
            // showStationsToolStripMenuItem
            // 
            this.showStationsToolStripMenuItem.Checked = true;
            this.showStationsToolStripMenuItem.CheckOnClick = true;
            this.showStationsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showStationsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showStationsToolStripMenuItem.Image")));
            this.showStationsToolStripMenuItem.Name = "showStationsToolStripMenuItem";
            this.showStationsToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.showStationsToolStripMenuItem.Text = "Show Stations";
            this.showStationsToolStripMenuItem.ToolTipText = "Show Stations";
            this.showStationsToolStripMenuItem.Click += new System.EventHandler(this.showStationsToolStripMenuItem_Click);
            // 
            // enableColoursToolStripMenuItem
            // 
            this.enableColoursToolStripMenuItem.CheckOnClick = true;
            this.enableColoursToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("enableColoursToolStripMenuItem.Image")));
            this.enableColoursToolStripMenuItem.Name = "enableColoursToolStripMenuItem";
            this.enableColoursToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.enableColoursToolStripMenuItem.Text = "Enable Colours";
            this.enableColoursToolStripMenuItem.Click += new System.EventHandler(this.enableColoursToolStripMenuItem_Click);
            // 
            // toolStripDropDownButtonNameStars
            // 
            this.toolStripDropDownButtonNameStars.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonNameStars.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDiscsToolStripMenuItem,
            this.showNamesToolStripMenuItem});
            this.toolStripDropDownButtonNameStars.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonNameStars.Image")));
            this.toolStripDropDownButtonNameStars.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonNameStars.Name = "toolStripDropDownButtonNameStars";
            this.toolStripDropDownButtonNameStars.Size = new System.Drawing.Size(29, 37);
            this.toolStripDropDownButtonNameStars.Text = "toolStripDropDownButtonNameStars";
            this.toolStripDropDownButtonNameStars.ToolTipText = "Configure discs and naming of stars";
            // 
            // showDiscsToolStripMenuItem
            // 
            this.showDiscsToolStripMenuItem.CheckOnClick = true;
            this.showDiscsToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.ImageStarDisc;
            this.showDiscsToolStripMenuItem.Name = "showDiscsToolStripMenuItem";
            this.showDiscsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.showDiscsToolStripMenuItem.Text = "Show Discs";
            this.showDiscsToolStripMenuItem.Click += new System.EventHandler(this.showDiscsToolStripMenuItem_Click);
            // 
            // showNamesToolStripMenuItem
            // 
            this.showNamesToolStripMenuItem.CheckOnClick = true;
            this.showNamesToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showNamesToolStripMenuItem.Image")));
            this.showNamesToolStripMenuItem.Name = "showNamesToolStripMenuItem";
            this.showNamesToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.showNamesToolStripMenuItem.Text = "Show Names";
            this.showNamesToolStripMenuItem.Click += new System.EventHandler(this.showNamesToolStripMenuItem_Click);
            // 
            // toolStripDropDownButtonBookmarks
            // 
            this.toolStripDropDownButtonBookmarks.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonBookmarks.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showBookmarksToolStripMenuItem,
            this.showNoteMarksToolStripMenuItem,
            this.newRegionBookmarkToolStripMenuItem});
            this.toolStripDropDownButtonBookmarks.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonBookmarks.Image")));
            this.toolStripDropDownButtonBookmarks.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonBookmarks.Name = "toolStripDropDownButtonBookmarks";
            this.toolStripDropDownButtonBookmarks.Size = new System.Drawing.Size(29, 37);
            this.toolStripDropDownButtonBookmarks.Text = "toolStripDropDownButton1";
            this.toolStripDropDownButtonBookmarks.ToolTipText = "Enable or disable display of bookmarks and add a new region bookmark";
            // 
            // showBookmarksToolStripMenuItem
            // 
            this.showBookmarksToolStripMenuItem.Checked = true;
            this.showBookmarksToolStripMenuItem.CheckOnClick = true;
            this.showBookmarksToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showBookmarksToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showBookmarksToolStripMenuItem.Image")));
            this.showBookmarksToolStripMenuItem.Name = "showBookmarksToolStripMenuItem";
            this.showBookmarksToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.showBookmarksToolStripMenuItem.Text = "Show Bookmarks";
            this.showBookmarksToolStripMenuItem.Click += new System.EventHandler(this.showBookmarksToolStripMenuItem_Click);
            // 
            // showNoteMarksToolStripMenuItem
            // 
            this.showNoteMarksToolStripMenuItem.Checked = true;
            this.showNoteMarksToolStripMenuItem.CheckOnClick = true;
            this.showNoteMarksToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showNoteMarksToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("showNoteMarksToolStripMenuItem.Image")));
            this.showNoteMarksToolStripMenuItem.Name = "showNoteMarksToolStripMenuItem";
            this.showNoteMarksToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.showNoteMarksToolStripMenuItem.Text = "Show Note marks";
            this.showNoteMarksToolStripMenuItem.Click += new System.EventHandler(this.showNoteMarksToolStripMenuItem_Click);
            // 
            // newRegionBookmarkToolStripMenuItem
            // 
            this.newRegionBookmarkToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newRegionBookmarkToolStripMenuItem.Image")));
            this.newRegionBookmarkToolStripMenuItem.Name = "newRegionBookmarkToolStripMenuItem";
            this.newRegionBookmarkToolStripMenuItem.Size = new System.Drawing.Size(195, 22);
            this.newRegionBookmarkToolStripMenuItem.Text = "New Region Bookmark";
            this.newRegionBookmarkToolStripMenuItem.Click += new System.EventHandler(this.newRegionBookmarkToolStripMenuItem_Click);
            // 
            // toolStripDropDownButtonGalObjects
            // 
            this.toolStripDropDownButtonGalObjects.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonGalObjects.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonGalObjects.Image")));
            this.toolStripDropDownButtonGalObjects.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonGalObjects.Name = "toolStripDropDownButtonGalObjects";
            this.toolStripDropDownButtonGalObjects.Size = new System.Drawing.Size(29, 37);
            this.toolStripDropDownButtonGalObjects.Text = "toolStripDropDownButton1";
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
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 40);
            // 
            // toolStripButtonHelp
            // 
            this.toolStripButtonHelp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonHelp.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonHelp.Image")));
            this.toolStripButtonHelp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonHelp.Name = "toolStripButtonHelp";
            this.toolStripButtonHelp.Size = new System.Drawing.Size(23, 37);
            this.toolStripButtonHelp.Text = "Help";
            this.toolStripButtonHelp.Click += new System.EventHandler(this.toolStripButtonHelp_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 40);
            // 
            // toolStripDropDownRecord
            // 
            this.toolStripDropDownRecord.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownRecord.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordToolStripMenuItem,
            this.recordStepToStepToolStripMenuItem,
            this.newRecordStepToolStripMenuItem,
            this.pauseRecordToolStripMenuItem,
            this.toolStripMenuItemClearRecording,
            this.playbackToolStripMenuItem,
            this.saveToFileToolStripMenuItem,
            this.LoadFileToolStripMenuItem});
            this.toolStripDropDownRecord.Image = global::EDDiscovery.Properties.Resources.VideoRecorder;
            this.toolStripDropDownRecord.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownRecord.Name = "toolStripDropDownRecord";
            this.toolStripDropDownRecord.Size = new System.Drawing.Size(29, 20);
            this.toolStripDropDownRecord.Text = "toolStripDropDownButton1";
            this.toolStripDropDownRecord.ToolTipText = "Record or Playback videos";
            this.toolStripDropDownRecord.DropDownOpening += new System.EventHandler(this.toolStripDropDownRecord_DropDownOpening);
            // 
            // recordToolStripMenuItem
            // 
            this.recordToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.RecordPressed;
            this.recordToolStripMenuItem.Name = "recordToolStripMenuItem";
            this.recordToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.recordToolStripMenuItem.Text = "Record (F5)";
            this.recordToolStripMenuItem.Click += new System.EventHandler(this.recordToolStripMenuItem_Click);
            // 
            // recordStepToStepToolStripMenuItem
            // 
            this.recordStepToStepToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.RecordPressed;
            this.recordStepToStepToolStripMenuItem.Name = "recordStepToStepToolStripMenuItem";
            this.recordStepToStepToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.recordStepToStepToolStripMenuItem.Text = "Record Step (F6)";
            this.recordStepToStepToolStripMenuItem.Click += new System.EventHandler(this.recordStepToStepToolStripMenuItem_Click);
            // 
            // newRecordStepToolStripMenuItem
            // 
            this.newRecordStepToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.RecordPressed;
            this.newRecordStepToolStripMenuItem.Name = "newRecordStepToolStripMenuItem";
            this.newRecordStepToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.newRecordStepToolStripMenuItem.Text = "New Record Step (F7)";
            this.newRecordStepToolStripMenuItem.Click += new System.EventHandler(this.newRecordStepToolStripMenuItem_Click);
            // 
            // pauseRecordToolStripMenuItem
            // 
            this.pauseRecordToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.PauseNormalRed;
            this.pauseRecordToolStripMenuItem.Name = "pauseRecordToolStripMenuItem";
            this.pauseRecordToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.pauseRecordToolStripMenuItem.Text = "Pause Record (F8)";
            this.pauseRecordToolStripMenuItem.Click += new System.EventHandler(this.pauseRecordToolStripMenuItem_Click);
            // 
            // toolStripMenuItemClearRecording
            // 
            this.toolStripMenuItemClearRecording.Image = global::EDDiscovery.Properties.Resources.StopNormalRed;
            this.toolStripMenuItemClearRecording.Name = "toolStripMenuItemClearRecording";
            this.toolStripMenuItemClearRecording.Size = new System.Drawing.Size(187, 22);
            this.toolStripMenuItemClearRecording.Text = "Clear Recording";
            this.toolStripMenuItemClearRecording.Click += new System.EventHandler(this.toolStripMenuItemClearRecording_Click);
            // 
            // playbackToolStripMenuItem
            // 
            this.playbackToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.PlayNormal;
            this.playbackToolStripMenuItem.Name = "playbackToolStripMenuItem";
            this.playbackToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.playbackToolStripMenuItem.Text = "Playback (F9)";
            this.playbackToolStripMenuItem.Click += new System.EventHandler(this.playbackToolStripMenuItem_Click);
            // 
            // saveToFileToolStripMenuItem
            // 
            this.saveToFileToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.floppy;
            this.saveToFileToolStripMenuItem.Name = "saveToFileToolStripMenuItem";
            this.saveToFileToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveToFileToolStripMenuItem.Text = "Save flight to file";
            this.saveToFileToolStripMenuItem.Click += new System.EventHandler(this.saveToFileToolStripMenuItem_Click);
            // 
            // LoadFileToolStripMenuItem
            // 
            this.LoadFileToolStripMenuItem.Image = global::EDDiscovery.Properties.Resources.floppy;
            this.LoadFileToolStripMenuItem.Name = "LoadFileToolStripMenuItem";
            this.LoadFileToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.LoadFileToolStripMenuItem.Text = "Load flight from file";
            this.LoadFileToolStripMenuItem.Click += new System.EventHandler(this.LoadFileToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 800);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1114, 22);
            this.statusStrip.TabIndex = 21;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(35, 17);
            this.statusLabel.Text = "x=0.0";
            // 
            // dotSelectedSystemCoords
            // 
            this.dotSelectedSystemCoords.Image = global::EDDiscovery.Properties.Resources.OrangeDot;
            this.dotSelectedSystemCoords.InitialImage = global::EDDiscovery.Properties.Resources.OrangeDot;
            this.dotSelectedSystemCoords.Location = new System.Drawing.Point(193, 22);
            this.dotSelectedSystemCoords.Name = "dotSelectedSystemCoords";
            this.dotSelectedSystemCoords.Size = new System.Drawing.Size(12, 12);
            this.dotSelectedSystemCoords.TabIndex = 26;
            this.dotSelectedSystemCoords.TabStop = false;
            this.toolTip1.SetToolTip(this.dotSelectedSystemCoords, "Centre map on this system");
            this.dotSelectedSystemCoords.Click += new System.EventHandler(this.dotSelectedSystemCoords_Click);
            // 
            // dotSystemCoords
            // 
            this.dotSystemCoords.Image = global::EDDiscovery.Properties.Resources.YellowDot;
            this.dotSystemCoords.InitialImage = global::EDDiscovery.Properties.Resources.YellowDot;
            this.dotSystemCoords.Location = new System.Drawing.Point(193, 4);
            this.dotSystemCoords.Name = "dotSystemCoords";
            this.dotSystemCoords.Size = new System.Drawing.Size(12, 12);
            this.dotSystemCoords.TabIndex = 25;
            this.dotSystemCoords.TabStop = false;
            this.toolTip1.SetToolTip(this.dotSystemCoords, "Centre map on this system");
            this.dotSystemCoords.Click += new System.EventHandler(this.dotSystemCoords_Click);
            // 
            // buttonCenter
            // 
            this.buttonCenter.Image = ((System.Drawing.Image)(resources.GetObject("buttonCenter.Image")));
            this.buttonCenter.Location = new System.Drawing.Point(134, 7);
            this.buttonCenter.Name = "buttonCenter";
            this.buttonCenter.Size = new System.Drawing.Size(23, 23);
            this.buttonCenter.TabIndex = 17;
            this.buttonCenter.TabStop = false;
            this.toolTip1.SetToolTip(this.buttonCenter, "Center map on system");
            this.buttonCenter.UseVisualStyleBackColor = true;
            this.buttonCenter.Click += new System.EventHandler(this.buttonCenter_Click);
            // 
            // buttonLookAt
            // 
            this.buttonLookAt.Image = global::EDDiscovery.Properties.Resources.eye;
            this.buttonLookAt.Location = new System.Drawing.Point(164, 7);
            this.buttonLookAt.Name = "buttonLookAt";
            this.buttonLookAt.Size = new System.Drawing.Size(23, 23);
            this.buttonLookAt.TabIndex = 27;
            this.toolTip1.SetToolTip(this.buttonLookAt, "Look at System");
            this.buttonLookAt.UseVisualStyleBackColor = true;
            this.buttonLookAt.Click += new System.EventHandler(this.buttonLookAt_Click);
            // 
            // labelClickedSystemCoords
            // 
            this.labelClickedSystemCoords.AutoSize = true;
            this.labelClickedSystemCoords.Location = new System.Drawing.Point(211, 22);
            this.labelClickedSystemCoords.Name = "labelClickedSystemCoords";
            this.labelClickedSystemCoords.Size = new System.Drawing.Size(57, 13);
            this.labelClickedSystemCoords.TabIndex = 24;
            this.labelClickedSystemCoords.Text = "Sol x=0.00";
            this.labelClickedSystemCoords.Click += new System.EventHandler(this.labelClickedSystemCoords_Click);
            // 
            // systemselectionMenuStrip
            // 
            this.systemselectionMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewOnEDSMToolStripMenuItem});
            this.systemselectionMenuStrip.Name = "systemselectionMenuStrip";
            this.systemselectionMenuStrip.Size = new System.Drawing.Size(151, 26);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // panelRight
            // 
            this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight.Controls.Add(this.buttonLookAt);
            this.panelRight.Controls.Add(this.labelClickedSystemCoords);
            this.panelRight.Controls.Add(this.dotSelectedSystemCoords);
            this.panelRight.Controls.Add(this.textboxFrom);
            this.panelRight.Controls.Add(this.buttonCenter);
            this.panelRight.Controls.Add(this.labelSystemCoords);
            this.panelRight.Controls.Add(this.dotSystemCoords);
            this.panelRight.Location = new System.Drawing.Point(608, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(506, 40);
            this.panelRight.TabIndex = 27;
            // 
            // FormMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1114, 822);
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
            this.Shown += new System.EventHandler(this.FormMap_Shown);
            this.Resize += new System.EventHandler(this.FormMap_Resize);
            this.toolStripShowAllStars.ResumeLayout(false);
            this.toolStripShowAllStars.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dotSelectedSystemCoords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dotSystemCoords)).EndInit();
            this.systemselectionMenuStrip.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

            }


            #endregion

            private OpenTK.GLControl glControl;
            internal ExtendedControls.AutoCompleteTextBox textboxFrom;
            private Label labelSystemCoords;
        private ToolStrip toolStripShowAllStars;
        private ToolStripButton toolStripButtonLastKnownPosition;
        private ToolStripButton toolStripButtonGrid;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel statusLabel;
        private ToolTip toolTip1;
        private ToolStripButton toolStripButtonPerspective;
        private Label labelClickedSystemCoords;
        private PictureBox dotSystemCoords;
        private PictureBox dotSelectedSystemCoords;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripDropDownButton dropdownMapNames;
        private ToolStripDropDownButton dropdownFilterDate;
        private ContextMenuStrip systemselectionMenuStrip;
        private ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private ToolStripButton toolStripButtonFineGrid;
        private ToolStripButton toolStripButtonCoords;
        private ToolStripButton toolStripButtonEliteMovement;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripSeparator toolStripSeparator3;
        private Panel panelRight;
        private ToolStripDropDownButton toolStripDropDownButtonFilterStars;
        private ToolStripSeparator toolStripSeparator4;
        private ToolStripSeparator toolStripSeparator5;
        private ToolStripDropDownButton toolStripDropDownButtonBookmarks;
        private ToolStripMenuItem showBookmarksToolStripMenuItem;
        private ToolStripMenuItem showNoteMarksToolStripMenuItem;
        private ToolStripMenuItem newRegionBookmarkToolStripMenuItem;
        private ToolStripMenuItem showStarstoolStripMenuItem;
        private ToolStripMenuItem showStationsToolStripMenuItem;
        private ToolStripButton toolStripButtonGoBackward;
        private ToolStripButton toolStripButtonGoForward;
        private ToolStripButton toolStripButtonAutoForward;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripButton toolStripButtonHelp;
        private ToolStripButton toolStripButtonHome;
        private ToolStripButton toolStripButtonHistory;
        private ToolStripButton toolStripButtonTarget;
        private ToolStripDropDownButton toolStripDropDownButtonGalObjects;
        private ToolStripDropDownButton toolStripDropDownButtonNameStars;
        private ToolStripMenuItem showDiscsToolStripMenuItem;
        private ToolStripMenuItem showNamesToolStripMenuItem;
        private ToolStripMenuItem enableColoursToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownRecord;
        private ToolStripMenuItem recordToolStripMenuItem;
        private ToolStripMenuItem playbackToolStripMenuItem;
        private ToolStripMenuItem saveToFileToolStripMenuItem;
        private ToolStripMenuItem LoadFileToolStripMenuItem;
        private ToolStripMenuItem pauseRecordToolStripMenuItem;
        private ToolStripMenuItem recordStepToStepToolStripMenuItem;
        private Button buttonCenter;
        private Button buttonLookAt;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripMenuItem toolStripMenuItemClearRecording;
        private ToolStripMenuItem newRecordStepToolStripMenuItem;
        private ToolStripDropDownButton toolStripDropDownButtonVisitedStars;
        private ToolStripMenuItem drawLinesBetweenStarsWithPositionToolStripMenuItem;
        private ToolStripMenuItem drawADiscOnStarsWithPositionToolStripMenuItem;
        private ToolStripMenuItem useWhiteForDiscsInsteadOfAssignedMapColourToolStripMenuItem;
    }
    }
