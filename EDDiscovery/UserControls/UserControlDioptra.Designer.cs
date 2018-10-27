namespace EDDiscovery.UserControls
{
	partial class UserControlDioptra
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.contextMenuStripDioptre = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.currentLocationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showSystemNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showCoordinatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.targetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.solToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.coloniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.sagAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.customMarkersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.addCustomPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripTextBoxCustomPanel = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.removeASystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripComboBoxPanelsList = new System.Windows.Forms.ToolStripComboBox();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dockedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.systemMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.galacticMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.panelsOrientationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.topDownToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.leftToRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.rightToLeftToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.downTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.labelCurrentSystem = new ExtendedControls.LabelExt();
			this.panelCurrentSystem = new ExtendedControls.PanelNoTheme();
			this.curSysCoordinates = new ExtendedControls.LabelExt();
			this.panelHome = new ExtendedControls.PanelNoTheme();
			this.labelHomeDist = new ExtendedControls.LabelExt();
			this.labelHomeName = new ExtendedControls.LabelExt();
			this.panelSol = new ExtendedControls.PanelNoTheme();
			this.labelSolDist = new ExtendedControls.LabelExt();
			this.labelSolName = new ExtendedControls.LabelExt();
			this.panelColonia = new ExtendedControls.PanelNoTheme();
			this.labelColoniaDist = new ExtendedControls.LabelExt();
			this.labelColoniaName = new ExtendedControls.LabelExt();
			this.panelTarget = new ExtendedControls.PanelNoTheme();
			this.labelTargetDist = new ExtendedControls.LabelExt();
			this.labelTargetName = new ExtendedControls.LabelExt();
			this.panelSagA = new ExtendedControls.PanelNoTheme();
			this.labelSagADist = new ExtendedControls.LabelExt();
			this.labelSagAName = new ExtendedControls.LabelExt();
			this.pictureBox = new ExtendedControls.PictureBoxHotspot();
			this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.contextMenuStripDioptre.SuspendLayout();
			this.panelCurrentSystem.SuspendLayout();
			this.panelHome.SuspendLayout();
			this.panelSol.SuspendLayout();
			this.panelColonia.SuspendLayout();
			this.panelTarget.SuspendLayout();
			this.panelSagA.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.flowLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStripDioptre
			// 
			this.contextMenuStripDioptre.ImageScalingSize = new System.Drawing.Size(17, 17);
			this.contextMenuStripDioptre.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentLocationToolStripMenuItem,
            this.targetToolStripMenuItem,
            this.homeToolStripMenuItem,
            this.toolStripSeparator1,
            this.solToolStripMenuItem,
            this.coloniaToolStripMenuItem,
            this.sagAToolStripMenuItem,
            this.toolStripSeparator2,
            this.customMarkersToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.contextMenuStripDioptre.Name = "contextMenuStrip1";
			this.contextMenuStripDioptre.Size = new System.Drawing.Size(170, 192);
			// 
			// currentLocationToolStripMenuItem
			// 
			this.currentLocationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showSystemNameToolStripMenuItem,
            this.showCoordinatesToolStripMenuItem});
			this.currentLocationToolStripMenuItem.Name = "currentLocationToolStripMenuItem";
			this.currentLocationToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.currentLocationToolStripMenuItem.Text = "Current Location";
			// 
			// showSystemNameToolStripMenuItem
			// 
			this.showSystemNameToolStripMenuItem.Checked = true;
			this.showSystemNameToolStripMenuItem.CheckOnClick = true;
			this.showSystemNameToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showSystemNameToolStripMenuItem.Name = "showSystemNameToolStripMenuItem";
			this.showSystemNameToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.showSystemNameToolStripMenuItem.Text = "Show System Name";
			this.showSystemNameToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.showSystemNameToolStripMenuItem_CheckStateChanged);
			// 
			// showCoordinatesToolStripMenuItem
			// 
			this.showCoordinatesToolStripMenuItem.Checked = true;
			this.showCoordinatesToolStripMenuItem.CheckOnClick = true;
			this.showCoordinatesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.showCoordinatesToolStripMenuItem.Name = "showCoordinatesToolStripMenuItem";
			this.showCoordinatesToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
			this.showCoordinatesToolStripMenuItem.Text = "Show Coordinates";
			this.showCoordinatesToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.showCoordinatesToolStripMenuItem_CheckStateChanged);
			// 
			// targetToolStripMenuItem
			// 
			this.targetToolStripMenuItem.Checked = true;
			this.targetToolStripMenuItem.CheckOnClick = true;
			this.targetToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.targetToolStripMenuItem.Name = "targetToolStripMenuItem";
			this.targetToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.targetToolStripMenuItem.Text = "Target";
			this.targetToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.targetToolStripMenuItem_CheckStateChanged);
			// 
			// homeToolStripMenuItem
			// 
			this.homeToolStripMenuItem.Checked = true;
			this.homeToolStripMenuItem.CheckOnClick = true;
			this.homeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
			this.homeToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.homeToolStripMenuItem.Text = "Home";
			this.homeToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.homeToolStripMenuItem_CheckStateChanged);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(166, 6);
			// 
			// solToolStripMenuItem
			// 
			this.solToolStripMenuItem.Checked = true;
			this.solToolStripMenuItem.CheckOnClick = true;
			this.solToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.solToolStripMenuItem.Name = "solToolStripMenuItem";
			this.solToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.solToolStripMenuItem.Text = "Sol";
			this.solToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.solToolStripMenuItem_CheckStateChanged);
			// 
			// coloniaToolStripMenuItem
			// 
			this.coloniaToolStripMenuItem.Checked = true;
			this.coloniaToolStripMenuItem.CheckOnClick = true;
			this.coloniaToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.coloniaToolStripMenuItem.Name = "coloniaToolStripMenuItem";
			this.coloniaToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.coloniaToolStripMenuItem.Text = "Colonia";
			this.coloniaToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.coloniaToolStripMenuItem_CheckStateChanged);
			// 
			// sagAToolStripMenuItem
			// 
			this.sagAToolStripMenuItem.Checked = true;
			this.sagAToolStripMenuItem.CheckOnClick = true;
			this.sagAToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.sagAToolStripMenuItem.Name = "sagAToolStripMenuItem";
			this.sagAToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.sagAToolStripMenuItem.Text = "SagA*";
			this.sagAToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.sagAToolStripMenuItem_CheckStateChanged);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(166, 6);
			// 
			// customMarkersToolStripMenuItem
			// 
			this.customMarkersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addCustomPanelToolStripMenuItem,
            this.toolStripTextBoxCustomPanel,
            this.toolStripSeparator3,
            this.removeASystemToolStripMenuItem,
            this.toolStripComboBoxPanelsList});
			this.customMarkersToolStripMenuItem.Name = "customMarkersToolStripMenuItem";
			this.customMarkersToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.customMarkersToolStripMenuItem.Text = "Add/Remove";
			// 
			// addCustomPanelToolStripMenuItem
			// 
			this.addCustomPanelToolStripMenuItem.Name = "addCustomPanelToolStripMenuItem";
			this.addCustomPanelToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.addCustomPanelToolStripMenuItem.Text = "Add a system";
			// 
			// toolStripTextBoxCustomPanel
			// 
			this.toolStripTextBoxCustomPanel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
			this.toolStripTextBoxCustomPanel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.toolStripTextBoxCustomPanel.Name = "toolStripTextBoxCustomPanel";
			this.toolStripTextBoxCustomPanel.Size = new System.Drawing.Size(100, 24);
			this.toolStripTextBoxCustomPanel.Validated += new System.EventHandler(this.toolStripTextBox1_Validated);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(178, 6);
			// 
			// removeASystemToolStripMenuItem
			// 
			this.removeASystemToolStripMenuItem.Name = "removeASystemToolStripMenuItem";
			this.removeASystemToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
			this.removeASystemToolStripMenuItem.Text = "Remove a system";
			// 
			// toolStripComboBoxPanelsList
			// 
			this.toolStripComboBoxPanelsList.Name = "toolStripComboBoxPanelsList";
			this.toolStripComboBoxPanelsList.Size = new System.Drawing.Size(121, 25);
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureToolStripMenuItem,
            this.showToolStripMenuItem,
            this.panelsOrientationToolStripMenuItem});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
			this.optionsToolStripMenuItem.Text = "Options";
			// 
			// configureToolStripMenuItem
			// 
			this.configureToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dockedToolStripMenuItem,
            this.systemMapToolStripMenuItem,
            this.galacticMapToolStripMenuItem});
			this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
			this.configureToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
			this.configureToolStripMenuItem.Text = "Don\'t show while...";
			// 
			// dockedToolStripMenuItem
			// 
			this.dockedToolStripMenuItem.CheckOnClick = true;
			this.dockedToolStripMenuItem.Name = "dockedToolStripMenuItem";
			this.dockedToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.dockedToolStripMenuItem.Text = "...Docked.";
			// 
			// systemMapToolStripMenuItem
			// 
			this.systemMapToolStripMenuItem.CheckOnClick = true;
			this.systemMapToolStripMenuItem.Name = "systemMapToolStripMenuItem";
			this.systemMapToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.systemMapToolStripMenuItem.Text = "...in System Map.";
			// 
			// galacticMapToolStripMenuItem
			// 
			this.galacticMapToolStripMenuItem.CheckOnClick = true;
			this.galacticMapToolStripMenuItem.Name = "galacticMapToolStripMenuItem";
			this.galacticMapToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
			this.galacticMapToolStripMenuItem.Text = "...in Galactic Map";
			// 
			// showToolStripMenuItem
			// 
			this.showToolStripMenuItem.CheckOnClick = true;
			this.showToolStripMenuItem.Name = "showToolStripMenuItem";
			this.showToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
			this.showToolStripMenuItem.Text = "Show black box around text";
			// 
			// panelsOrientationToolStripMenuItem
			// 
			this.panelsOrientationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.topDownToolStripMenuItem,
            this.leftToRightToolStripMenuItem,
            this.rightToLeftToolStripMenuItem,
            this.downTopToolStripMenuItem});
			this.panelsOrientationToolStripMenuItem.Name = "panelsOrientationToolStripMenuItem";
			this.panelsOrientationToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
			this.panelsOrientationToolStripMenuItem.Text = "Panels Orientation";
			// 
			// topDownToolStripMenuItem
			// 
			this.topDownToolStripMenuItem.Checked = true;
			this.topDownToolStripMenuItem.CheckOnClick = true;
			this.topDownToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.topDownToolStripMenuItem.Name = "topDownToolStripMenuItem";
			this.topDownToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.topDownToolStripMenuItem.Text = "TopDown";
			this.topDownToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.topDownToolStripMenuItem_CheckStateChanged);
			// 
			// leftToRightToolStripMenuItem
			// 
			this.leftToRightToolStripMenuItem.CheckOnClick = true;
			this.leftToRightToolStripMenuItem.Name = "leftToRightToolStripMenuItem";
			this.leftToRightToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.leftToRightToolStripMenuItem.Text = "LeftToRight";
			this.leftToRightToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.leftToRightToolStripMenuItem_CheckStateChanged);
			// 
			// rightToLeftToolStripMenuItem
			// 
			this.rightToLeftToolStripMenuItem.CheckOnClick = true;
			this.rightToLeftToolStripMenuItem.Name = "rightToLeftToolStripMenuItem";
			this.rightToLeftToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.rightToLeftToolStripMenuItem.Text = "RightToLeft";
			this.rightToLeftToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.rightToLeftToolStripMenuItem_CheckStateChanged);
			// 
			// downTopToolStripMenuItem
			// 
			this.downTopToolStripMenuItem.CheckOnClick = true;
			this.downTopToolStripMenuItem.Name = "downTopToolStripMenuItem";
			this.downTopToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
			this.downTopToolStripMenuItem.Text = "DownTop";
			this.downTopToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.downTopToolStripMenuItem_CheckStateChanged);
			// 
			// labelCurrentSystem
			// 
			this.labelCurrentSystem.AutoEllipsis = true;
			this.labelCurrentSystem.AutoSize = true;
			this.labelCurrentSystem.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelCurrentSystem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelCurrentSystem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelCurrentSystem.Location = new System.Drawing.Point(0, 0);
			this.labelCurrentSystem.Name = "labelCurrentSystem";
			this.labelCurrentSystem.Size = new System.Drawing.Size(0, 13);
			this.labelCurrentSystem.TabIndex = 1;
			this.labelCurrentSystem.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// panelCurrentSystem
			// 
			this.panelCurrentSystem.BackColor = System.Drawing.Color.Transparent;
			this.panelCurrentSystem.Controls.Add(this.curSysCoordinates);
			this.panelCurrentSystem.Controls.Add(this.labelCurrentSystem);
			this.panelCurrentSystem.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelCurrentSystem.Location = new System.Drawing.Point(0, 0);
			this.panelCurrentSystem.Name = "panelCurrentSystem";
			this.panelCurrentSystem.Size = new System.Drawing.Size(576, 30);
			this.panelCurrentSystem.TabIndex = 2;
			// 
			// curSysCoordinates
			// 
			this.curSysCoordinates.AutoEllipsis = true;
			this.curSysCoordinates.AutoSize = true;
			this.curSysCoordinates.Dock = System.Windows.Forms.DockStyle.Left;
			this.curSysCoordinates.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.curSysCoordinates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.curSysCoordinates.Location = new System.Drawing.Point(0, 0);
			this.curSysCoordinates.Name = "curSysCoordinates";
			this.curSysCoordinates.Size = new System.Drawing.Size(0, 13);
			this.curSysCoordinates.TabIndex = 2;
			this.curSysCoordinates.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// panelHome
			// 
			this.panelHome.BackColor = System.Drawing.Color.Transparent;
			this.panelHome.Controls.Add(this.labelHomeDist);
			this.panelHome.Controls.Add(this.labelHomeName);
			this.panelHome.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelHome.Location = new System.Drawing.Point(3, 39);
			this.panelHome.Name = "panelHome";
			this.panelHome.Size = new System.Drawing.Size(0, 30);
			this.panelHome.TabIndex = 3;
			// 
			// labelHomeDist
			// 
			this.labelHomeDist.AutoEllipsis = true;
			this.labelHomeDist.AutoSize = true;
			this.labelHomeDist.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelHomeDist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelHomeDist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHomeDist.Location = new System.Drawing.Point(0, 0);
			this.labelHomeDist.Name = "labelHomeDist";
			this.labelHomeDist.Size = new System.Drawing.Size(0, 13);
			this.labelHomeDist.TabIndex = 1;
			this.labelHomeDist.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// labelHomeName
			// 
			this.labelHomeName.AutoEllipsis = true;
			this.labelHomeName.AutoSize = true;
			this.labelHomeName.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelHomeName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelHomeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHomeName.Location = new System.Drawing.Point(0, 0);
			this.labelHomeName.Name = "labelHomeName";
			this.labelHomeName.Size = new System.Drawing.Size(0, 13);
			this.labelHomeName.TabIndex = 2;
			this.labelHomeName.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// panelSol
			// 
			this.panelSol.BackColor = System.Drawing.Color.Transparent;
			this.panelSol.Controls.Add(this.labelSolDist);
			this.panelSol.Controls.Add(this.labelSolName);
			this.panelSol.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelSol.Location = new System.Drawing.Point(3, 75);
			this.panelSol.Name = "panelSol";
			this.panelSol.Size = new System.Drawing.Size(0, 30);
			this.panelSol.TabIndex = 4;
			// 
			// labelSolDist
			// 
			this.labelSolDist.AutoEllipsis = true;
			this.labelSolDist.AutoSize = true;
			this.labelSolDist.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelSolDist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelSolDist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSolDist.Location = new System.Drawing.Point(0, 0);
			this.labelSolDist.Name = "labelSolDist";
			this.labelSolDist.Size = new System.Drawing.Size(0, 13);
			this.labelSolDist.TabIndex = 1;
			this.labelSolDist.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// labelSolName
			// 
			this.labelSolName.AutoEllipsis = true;
			this.labelSolName.AutoSize = true;
			this.labelSolName.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelSolName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelSolName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSolName.Location = new System.Drawing.Point(0, 0);
			this.labelSolName.Name = "labelSolName";
			this.labelSolName.Size = new System.Drawing.Size(0, 13);
			this.labelSolName.TabIndex = 2;
			this.labelSolName.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// panelColonia
			// 
			this.panelColonia.BackColor = System.Drawing.Color.Transparent;
			this.panelColonia.Controls.Add(this.labelColoniaDist);
			this.panelColonia.Controls.Add(this.labelColoniaName);
			this.panelColonia.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelColonia.Location = new System.Drawing.Point(3, 111);
			this.panelColonia.Name = "panelColonia";
			this.panelColonia.Size = new System.Drawing.Size(0, 30);
			this.panelColonia.TabIndex = 5;
			// 
			// labelColoniaDist
			// 
			this.labelColoniaDist.AutoEllipsis = true;
			this.labelColoniaDist.AutoSize = true;
			this.labelColoniaDist.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelColoniaDist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelColoniaDist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelColoniaDist.Location = new System.Drawing.Point(0, 0);
			this.labelColoniaDist.Name = "labelColoniaDist";
			this.labelColoniaDist.Size = new System.Drawing.Size(0, 13);
			this.labelColoniaDist.TabIndex = 1;
			this.labelColoniaDist.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// labelColoniaName
			// 
			this.labelColoniaName.AutoEllipsis = true;
			this.labelColoniaName.AutoSize = true;
			this.labelColoniaName.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelColoniaName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelColoniaName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelColoniaName.Location = new System.Drawing.Point(0, 0);
			this.labelColoniaName.Name = "labelColoniaName";
			this.labelColoniaName.Size = new System.Drawing.Size(0, 13);
			this.labelColoniaName.TabIndex = 2;
			this.labelColoniaName.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// panelTarget
			// 
			this.panelTarget.BackColor = System.Drawing.Color.Transparent;
			this.panelTarget.Controls.Add(this.labelTargetDist);
			this.panelTarget.Controls.Add(this.labelTargetName);
			this.panelTarget.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelTarget.Location = new System.Drawing.Point(3, 3);
			this.panelTarget.Name = "panelTarget";
			this.panelTarget.Size = new System.Drawing.Size(0, 30);
			this.panelTarget.TabIndex = 6;
			// 
			// labelTargetDist
			// 
			this.labelTargetDist.AutoEllipsis = true;
			this.labelTargetDist.AutoSize = true;
			this.labelTargetDist.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelTargetDist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelTargetDist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTargetDist.Location = new System.Drawing.Point(0, 0);
			this.labelTargetDist.Name = "labelTargetDist";
			this.labelTargetDist.Size = new System.Drawing.Size(0, 13);
			this.labelTargetDist.TabIndex = 1;
			this.labelTargetDist.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// labelTargetName
			// 
			this.labelTargetName.AutoEllipsis = true;
			this.labelTargetName.AutoSize = true;
			this.labelTargetName.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelTargetName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelTargetName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelTargetName.Location = new System.Drawing.Point(0, 0);
			this.labelTargetName.Name = "labelTargetName";
			this.labelTargetName.Size = new System.Drawing.Size(0, 13);
			this.labelTargetName.TabIndex = 2;
			this.labelTargetName.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// panelSagA
			// 
			this.panelSagA.BackColor = System.Drawing.Color.Transparent;
			this.panelSagA.Controls.Add(this.labelSagADist);
			this.panelSagA.Controls.Add(this.labelSagAName);
			this.panelSagA.Dock = System.Windows.Forms.DockStyle.Top;
			this.panelSagA.Location = new System.Drawing.Point(3, 147);
			this.panelSagA.Name = "panelSagA";
			this.panelSagA.Size = new System.Drawing.Size(0, 30);
			this.panelSagA.TabIndex = 7;
			// 
			// labelSagADist
			// 
			this.labelSagADist.AutoEllipsis = true;
			this.labelSagADist.AutoSize = true;
			this.labelSagADist.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelSagADist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelSagADist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSagADist.Location = new System.Drawing.Point(0, 0);
			this.labelSagADist.Name = "labelSagADist";
			this.labelSagADist.Size = new System.Drawing.Size(0, 13);
			this.labelSagADist.TabIndex = 1;
			this.labelSagADist.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// labelSagAName
			// 
			this.labelSagAName.AutoEllipsis = true;
			this.labelSagAName.AutoSize = true;
			this.labelSagAName.Dock = System.Windows.Forms.DockStyle.Left;
			this.labelSagAName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.labelSagAName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelSagAName.Location = new System.Drawing.Point(0, 0);
			this.labelSagAName.Name = "labelSagAName";
			this.labelSagAName.Size = new System.Drawing.Size(0, 13);
			this.labelSagAName.TabIndex = 2;
			this.labelSagAName.TextBackColor = System.Drawing.Color.Transparent;
			// 
			// pictureBox
			// 
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Location = new System.Drawing.Point(0, 0);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(576, 973);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox.TabIndex = 8;
			this.pictureBox.ClickElement += new ExtendedControls.PictureBoxHotspot.OnElement(this.pictureBox_ClickElement);
			this.pictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseClick);
			// 
			// flowLayoutPanel1
			// 
			this.flowLayoutPanel1.AutoSize = true;
			this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowLayoutPanel1.Controls.Add(this.panelTarget);
			this.flowLayoutPanel1.Controls.Add(this.panelHome);
			this.flowLayoutPanel1.Controls.Add(this.panelSol);
			this.flowLayoutPanel1.Controls.Add(this.panelColonia);
			this.flowLayoutPanel1.Controls.Add(this.panelSagA);
			this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 30);
			this.flowLayoutPanel1.Name = "flowLayoutPanel1";
			this.flowLayoutPanel1.Size = new System.Drawing.Size(576, 943);
			this.flowLayoutPanel1.TabIndex = 9;
			this.flowLayoutPanel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.flowLayoutPanel1_MouseClick);
			// 
			// UserControlDioptra
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.flowLayoutPanel1);
			this.Controls.Add(this.panelCurrentSystem);
			this.Controls.Add(this.pictureBox);
			this.Name = "UserControlDioptra";
			this.Size = new System.Drawing.Size(576, 973);
			this.contextMenuStripDioptre.ResumeLayout(false);
			this.panelCurrentSystem.ResumeLayout(false);
			this.panelCurrentSystem.PerformLayout();
			this.panelHome.ResumeLayout(false);
			this.panelHome.PerformLayout();
			this.panelSol.ResumeLayout(false);
			this.panelSol.PerformLayout();
			this.panelColonia.ResumeLayout(false);
			this.panelColonia.PerformLayout();
			this.panelTarget.ResumeLayout(false);
			this.panelTarget.PerformLayout();
			this.panelSagA.ResumeLayout(false);
			this.panelSagA.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.flowLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStripDioptre;
		private System.Windows.Forms.ToolStripMenuItem currentLocationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showSystemNameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showCoordinatesToolStripMenuItem;
		private ExtendedControls.LabelExt labelCurrentSystem;
		private ExtendedControls.PanelNoTheme panelCurrentSystem;
		private ExtendedControls.LabelExt curSysCoordinates;
		private ExtendedControls.PanelNoTheme panelHome;
		private ExtendedControls.LabelExt labelHomeName;
		private ExtendedControls.LabelExt labelHomeDist;
		private ExtendedControls.PanelNoTheme panelSol;
		private ExtendedControls.LabelExt labelSolName;
		private ExtendedControls.LabelExt labelSolDist;
		private ExtendedControls.PanelNoTheme panelColonia;
		private ExtendedControls.LabelExt labelColoniaName;
		private ExtendedControls.LabelExt labelColoniaDist;
		private ExtendedControls.PanelNoTheme panelTarget;
		private ExtendedControls.LabelExt labelTargetName;
		private ExtendedControls.LabelExt labelTargetDist;
		private System.Windows.Forms.ToolStripMenuItem solToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem sagAToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem coloniaToolStripMenuItem;
		private ExtendedControls.PanelNoTheme panelSagA;
		private ExtendedControls.LabelExt labelSagAName;
		private ExtendedControls.LabelExt labelSagADist;
		private System.Windows.Forms.ToolStripMenuItem targetToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem homeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem customMarkersToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem dockedToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem systemMapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem galacticMapToolStripMenuItem;
		private ExtendedControls.PictureBoxHotspot pictureBox;
		private System.Windows.Forms.ToolStripMenuItem addCustomPanelToolStripMenuItem;
		private System.Windows.Forms.ToolStripTextBox toolStripTextBoxCustomPanel;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem removeASystemToolStripMenuItem;
		private System.Windows.Forms.ToolStripComboBox toolStripComboBoxPanelsList;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.ToolStripMenuItem panelsOrientationToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem topDownToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem leftToRightToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem rightToLeftToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem downTopToolStripMenuItem;
	}
}
