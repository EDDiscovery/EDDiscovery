namespace EDDiscovery
{
    partial class TravelHistoryControl
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
            this.toolTipEddb = new System.Windows.Forms.ToolTip(this.components);
            this.labelNote = new System.Windows.Forms.Label();
            this.textBoxTargetDist = new ExtendedControls.TextBoxBorder();
            this.textBoxTarget = new ExtendedControls.AutoCompleteTextBox();
            this.buttonEDSM = new ExtendedControls.DrawnPanel();
            this.textBoxHomeDist = new ExtendedControls.TextBoxBorder();
            this.buttonRoss = new ExtendedControls.DrawnPanel();
            this.buttonEDDB = new ExtendedControls.DrawnPanel();
            this.comboBoxCustomPopOut = new ExtendedControls.ComboBoxCustom();
            this.button_RefreshHistory = new ExtendedControls.ButtonExt();
            this.buttonMap2D = new ExtendedControls.ButtonExt();
            this.comboBoxCommander = new ExtendedControls.ComboBoxCustom();
            this.buttonMap = new ExtendedControls.ButtonExt();
            this.buttonSync = new ExtendedControls.ButtonExt();
            this.splitContainerLeftRight = new System.Windows.Forms.SplitContainer();
            this.splitContainerLeft = new System.Windows.Forms.SplitContainer();
            this.userControlTravelGrid = new EDDiscovery.UserControls.UserControlTravelGrid();
            this.tabStripBottom = new EDDiscovery.Controls.TabStrip();
            this.splitContainerRightOuter = new System.Windows.Forms.SplitContainer();
            this.panelTarget = new System.Windows.Forms.Panel();
            this.labelTarget = new System.Windows.Forms.Label();
            this.panelNoteArea = new System.Windows.Forms.Panel();
            this.richTextBoxNote = new ExtendedControls.RichTextBoxScroll();
            this.panel_system = new System.Windows.Forms.Panel();
            this.labelHomeSystem = new System.Windows.Forms.Label();
            this.textBoxState = new ExtendedControls.TextBoxBorder();
            this.textBoxEconomy = new ExtendedControls.TextBoxBorder();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.textBoxGovernment = new ExtendedControls.TextBoxBorder();
            this.textBoxAllegiance = new ExtendedControls.TextBoxBorder();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxVisits = new ExtendedControls.TextBoxBorder();
            this.label_Z = new System.Windows.Forms.Label();
            this.textBoxZ = new ExtendedControls.TextBoxBorder();
            this.labelDistEnter = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxY = new ExtendedControls.TextBoxBorder();
            this.textBoxX = new ExtendedControls.TextBoxBorder();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSystem = new ExtendedControls.TextBoxBorder();
            this.panel_topright = new System.Windows.Forms.Panel();
            this.labelCMDR = new System.Windows.Forms.Label();
            this.splitContainerRightInner = new System.Windows.Forms.SplitContainer();
            this.tabStripMiddleRight = new EDDiscovery.Controls.TabStrip();
            this.tabStripBottomRight = new EDDiscovery.Controls.TabStrip();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeftRight)).BeginInit();
            this.splitContainerLeftRight.Panel1.SuspendLayout();
            this.splitContainerLeftRight.Panel2.SuspendLayout();
            this.splitContainerLeftRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).BeginInit();
            this.splitContainerLeft.Panel1.SuspendLayout();
            this.splitContainerLeft.Panel2.SuspendLayout();
            this.splitContainerLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightOuter)).BeginInit();
            this.splitContainerRightOuter.Panel1.SuspendLayout();
            this.splitContainerRightOuter.Panel2.SuspendLayout();
            this.splitContainerRightOuter.SuspendLayout();
            this.panelTarget.SuspendLayout();
            this.panelNoteArea.SuspendLayout();
            this.panel_system.SuspendLayout();
            this.panel_topright.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightInner)).BeginInit();
            this.splitContainerRightInner.Panel1.SuspendLayout();
            this.splitContainerRightInner.Panel2.SuspendLayout();
            this.splitContainerRightInner.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTipEddb
            // 
            this.toolTipEddb.ShowAlways = true;
            // 
            // labelNote
            // 
            this.labelNote.AutoSize = true;
            this.labelNote.Location = new System.Drawing.Point(0, 0);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(30, 13);
            this.labelNote.TabIndex = 28;
            this.labelNote.Text = "Note";
            this.toolTipEddb.SetToolTip(this.labelNote, "Enter a note against the currently selected entry");
            // 
            // textBoxTargetDist
            // 
            this.textBoxTargetDist.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTargetDist.BorderColorScaling = 0.5F;
            this.textBoxTargetDist.Location = new System.Drawing.Point(210, 6);
            this.textBoxTargetDist.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.textBoxTargetDist.Name = "textBoxTargetDist";
            this.textBoxTargetDist.ReadOnly = true;
            this.textBoxTargetDist.Size = new System.Drawing.Size(62, 20);
            this.textBoxTargetDist.TabIndex = 15;
            this.textBoxTargetDist.TabStop = false;
            this.toolTipEddb.SetToolTip(this.textBoxTargetDist, "Distance to target");
            // 
            // textBoxTarget
            // 
            this.textBoxTarget.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxTarget.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxTarget.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTarget.BorderColorScaling = 0.5F;
            this.textBoxTarget.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.textBoxTarget.DropDownBorderColor = System.Drawing.Color.Green;
            this.textBoxTarget.DropDownHeight = 200;
            this.textBoxTarget.DropDownItemHeight = 20;
            this.textBoxTarget.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.textBoxTarget.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.textBoxTarget.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.textBoxTarget.DropDownWidth = 0;
            this.textBoxTarget.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxTarget.Location = new System.Drawing.Point(68, 6);
            this.textBoxTarget.Name = "textBoxTarget";
            this.textBoxTarget.Size = new System.Drawing.Size(129, 20);
            this.textBoxTarget.TabIndex = 15;
            this.textBoxTarget.TabStop = false;
            this.toolTipEddb.SetToolTip(this.textBoxTarget, "Sets the target");
            this.textBoxTarget.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxTarget_KeyUp);
            // 
            // buttonEDSM
            // 
            this.buttonEDSM.DrawnImage = null;
            this.buttonEDSM.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonEDSM.ImageSelected = ExtendedControls.DrawnPanel.ImageType.InverseText;
            this.buttonEDSM.ImageText = "EDSM";
            this.buttonEDSM.Location = new System.Drawing.Point(208, 6);
            this.buttonEDSM.MarginSize = 0;
            this.buttonEDSM.MouseOverColor = System.Drawing.Color.White;
            this.buttonEDSM.MouseSelectedColor = System.Drawing.Color.Green;
            this.buttonEDSM.Name = "buttonEDSM";
            this.buttonEDSM.Size = new System.Drawing.Size(44, 20);
            this.buttonEDSM.TabIndex = 23;
            this.toolTipEddb.SetToolTip(this.buttonEDSM, "Click to show system on EDSM");
            this.buttonEDSM.Click += new System.EventHandler(this.buttonEDSM_Click);
            // 
            // textBoxHomeDist
            // 
            this.textBoxHomeDist.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxHomeDist.BorderColorScaling = 0.5F;
            this.textBoxHomeDist.Location = new System.Drawing.Point(50, 84);
            this.textBoxHomeDist.Name = "textBoxHomeDist";
            this.textBoxHomeDist.ReadOnly = true;
            this.textBoxHomeDist.Size = new System.Drawing.Size(67, 20);
            this.textBoxHomeDist.TabIndex = 42;
            this.textBoxHomeDist.TabStop = false;
            this.toolTipEddb.SetToolTip(this.textBoxHomeDist, "Distance to home planet");
            // 
            // buttonRoss
            // 
            this.buttonRoss.BackColor = System.Drawing.SystemColors.ControlText;
            this.buttonRoss.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonRoss.DrawnImage = null;
            this.buttonRoss.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonRoss.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Ross;
            this.buttonRoss.ImageText = null;
            this.buttonRoss.Location = new System.Drawing.Point(275, 6);
            this.buttonRoss.MarginSize = 0;
            this.buttonRoss.MouseOverColor = System.Drawing.Color.White;
            this.buttonRoss.MouseSelectedColor = System.Drawing.Color.Green;
            this.buttonRoss.Name = "buttonRoss";
            this.buttonRoss.Size = new System.Drawing.Size(20, 20);
            this.buttonRoss.TabIndex = 40;
            this.toolTipEddb.SetToolTip(this.buttonRoss, "Click to edit system in Ross");
            this.buttonRoss.Click += new System.EventHandler(this.buttonRoss_Click);
            // 
            // buttonEDDB
            // 
            this.buttonEDDB.BackColor = System.Drawing.SystemColors.Control;
            this.buttonEDDB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonEDDB.DrawnImage = null;
            this.buttonEDDB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonEDDB.ImageSelected = ExtendedControls.DrawnPanel.ImageType.EDDB;
            this.buttonEDDB.ImageText = null;
            this.buttonEDDB.Location = new System.Drawing.Point(252, 6);
            this.buttonEDDB.MarginSize = 0;
            this.buttonEDDB.MouseOverColor = System.Drawing.Color.White;
            this.buttonEDDB.MouseSelectedColor = System.Drawing.Color.Green;
            this.buttonEDDB.Name = "buttonEDDB";
            this.buttonEDDB.Size = new System.Drawing.Size(20, 20);
            this.buttonEDDB.TabIndex = 39;
            this.toolTipEddb.SetToolTip(this.buttonEDDB, "Click to show system in EDDB");
            this.buttonEDDB.Click += new System.EventHandler(this.buttonEDDB_Click);
            // 
            // comboBoxCustomPopOut
            // 
            this.comboBoxCustomPopOut.ArrowWidth = 1;
            this.comboBoxCustomPopOut.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomPopOut.ButtonColorScaling = 0.5F;
            this.comboBoxCustomPopOut.DataSource = null;
            this.comboBoxCustomPopOut.DisplayMember = "";
            this.comboBoxCustomPopOut.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomPopOut.DropDownHeight = 200;
            this.comboBoxCustomPopOut.DropDownWidth = 150;
            this.comboBoxCustomPopOut.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomPopOut.ItemHeight = 13;
            this.comboBoxCustomPopOut.Location = new System.Drawing.Point(151, 37);
            this.comboBoxCustomPopOut.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomPopOut.Name = "comboBoxCustomPopOut";
            this.comboBoxCustomPopOut.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomPopOut.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomPopOut.ScrollBarWidth = 16;
            this.comboBoxCustomPopOut.SelectedIndex = -1;
            this.comboBoxCustomPopOut.SelectedItem = null;
            this.comboBoxCustomPopOut.SelectedValue = null;
            this.comboBoxCustomPopOut.Size = new System.Drawing.Size(65, 23);
            this.comboBoxCustomPopOut.TabIndex = 18;
            this.comboBoxCustomPopOut.Text = "comboBoxCustom1";
            this.toolTipEddb.SetToolTip(this.comboBoxCustomPopOut, "Open another window");
            this.comboBoxCustomPopOut.ValueMember = "";
            this.comboBoxCustomPopOut.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomPopOut_SelectedIndexChanged);
            // 
            // button_RefreshHistory
            // 
            this.button_RefreshHistory.BorderColorScaling = 1.25F;
            this.button_RefreshHistory.ButtonColorScaling = 0.5F;
            this.button_RefreshHistory.ButtonDisabledScaling = 0.5F;
            this.button_RefreshHistory.Location = new System.Drawing.Point(6, 6);
            this.button_RefreshHistory.Name = "button_RefreshHistory";
            this.button_RefreshHistory.Size = new System.Drawing.Size(65, 23);
            this.button_RefreshHistory.TabIndex = 2;
            this.button_RefreshHistory.Text = "Refresh";
            this.toolTipEddb.SetToolTip(this.button_RefreshHistory, "Perform an EDSM synchronisation then refresh the list");
            this.button_RefreshHistory.UseVisualStyleBackColor = true;
            this.button_RefreshHistory.Click += new System.EventHandler(this.button_RefreshHistory_Click);
            // 
            // buttonMap2D
            // 
            this.buttonMap2D.BorderColorScaling = 1.25F;
            this.buttonMap2D.ButtonColorScaling = 0.5F;
            this.buttonMap2D.ButtonDisabledScaling = 0.5F;
            this.buttonMap2D.Location = new System.Drawing.Point(6, 36);
            this.buttonMap2D.Name = "buttonMap2D";
            this.buttonMap2D.Size = new System.Drawing.Size(65, 23);
            this.buttonMap2D.TabIndex = 3;
            this.buttonMap2D.Text = "2D map";
            this.toolTipEddb.SetToolTip(this.buttonMap2D, "Open the 2D Map");
            this.buttonMap2D.UseVisualStyleBackColor = true;
            this.buttonMap2D.Click += new System.EventHandler(this.button2DMap_Click);
            // 
            // comboBoxCommander
            // 
            this.comboBoxCommander.ArrowWidth = 1;
            this.comboBoxCommander.BorderColor = System.Drawing.Color.Red;
            this.comboBoxCommander.ButtonColorScaling = 0.5F;
            this.comboBoxCommander.DataSource = null;
            this.comboBoxCommander.DisplayMember = "";
            this.comboBoxCommander.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCommander.DropDownHeight = 200;
            this.comboBoxCommander.DropDownWidth = 173;
            this.comboBoxCommander.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCommander.ItemHeight = 13;
            this.comboBoxCommander.Location = new System.Drawing.Point(114, 6);
            this.comboBoxCommander.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCommander.Name = "comboBoxCommander";
            this.comboBoxCommander.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCommander.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCommander.ScrollBarWidth = 16;
            this.comboBoxCommander.SelectedIndex = -1;
            this.comboBoxCommander.SelectedItem = null;
            this.comboBoxCommander.SelectedValue = null;
            this.comboBoxCommander.Size = new System.Drawing.Size(173, 23);
            this.comboBoxCommander.TabIndex = 0;
            this.toolTipEddb.SetToolTip(this.comboBoxCommander, "Select your commander. Use Settings page to configure and add additional commande" +
        "rs");
            this.comboBoxCommander.ValueMember = "";
            this.comboBoxCommander.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommander_SelectedIndexChanged);
            // 
            // buttonMap
            // 
            this.buttonMap.BorderColorScaling = 1.25F;
            this.buttonMap.ButtonColorScaling = 0.5F;
            this.buttonMap.ButtonDisabledScaling = 0.5F;
            this.buttonMap.Location = new System.Drawing.Point(80, 36);
            this.buttonMap.Name = "buttonMap";
            this.buttonMap.Size = new System.Drawing.Size(65, 23);
            this.buttonMap.TabIndex = 4;
            this.buttonMap.Text = "3D map";
            this.toolTipEddb.SetToolTip(this.buttonMap, "Open the 3D Map");
            this.buttonMap.UseVisualStyleBackColor = true;
            this.buttonMap.Click += new System.EventHandler(this.buttonMap_Click);
            // 
            // buttonSync
            // 
            this.buttonSync.BorderColorScaling = 1.25F;
            this.buttonSync.ButtonColorScaling = 0.5F;
            this.buttonSync.ButtonDisabledScaling = 0.5F;
            this.buttonSync.Location = new System.Drawing.Point(7, 66);
            this.buttonSync.Name = "buttonSync";
            this.buttonSync.Size = new System.Drawing.Size(92, 23);
            this.buttonSync.TabIndex = 4;
            this.buttonSync.Text = "EDSM Sync";
            this.toolTipEddb.SetToolTip(this.buttonSync, "Send history to your EDSM account (make sure the EDSM ID is set in settings)");
            this.buttonSync.UseVisualStyleBackColor = true;
            this.buttonSync.Click += new System.EventHandler(this.buttonSync_Click);
            // 
            // splitContainerLeftRight
            // 
            this.splitContainerLeftRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeftRight.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeftRight.Name = "splitContainerLeftRight";
            // 
            // splitContainerLeftRight.Panel1
            // 
            this.splitContainerLeftRight.Panel1.Controls.Add(this.splitContainerLeft);
            // 
            // splitContainerLeftRight.Panel2
            // 
            this.splitContainerLeftRight.Panel2.Controls.Add(this.splitContainerRightOuter);
            this.splitContainerLeftRight.Size = new System.Drawing.Size(891, 650);
            this.splitContainerLeftRight.SplitterDistance = 550;
            this.splitContainerLeftRight.TabIndex = 6;
            // 
            // splitContainerLeft
            // 
            this.splitContainerLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLeft.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLeft.Name = "splitContainerLeft";
            this.splitContainerLeft.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLeft.Panel1
            // 
            this.splitContainerLeft.Panel1.Controls.Add(this.userControlTravelGrid);
            // 
            // splitContainerLeft.Panel2
            // 
            this.splitContainerLeft.Panel2.Controls.Add(this.tabStripBottom);
            this.splitContainerLeft.Size = new System.Drawing.Size(550, 650);
            this.splitContainerLeft.SplitterDistance = 350;
            this.splitContainerLeft.TabIndex = 5;
            // 
            // userControlTravelGrid
            // 
            this.userControlTravelGrid.currentGridRow = -1;
            this.userControlTravelGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlTravelGrid.Location = new System.Drawing.Point(0, 0);
            this.userControlTravelGrid.Name = "userControlTravelGrid";
            this.userControlTravelGrid.Size = new System.Drawing.Size(550, 350);
            this.userControlTravelGrid.TabIndex = 0;
            // 
            // tabStripBottom
            // 
            this.tabStripBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStripBottom.Location = new System.Drawing.Point(0, 0);
            this.tabStripBottom.Name = "tabStripBottom";
            this.tabStripBottom.SelectedIndex = -1;
            this.tabStripBottom.ShowPopOut = true;
            this.tabStripBottom.Size = new System.Drawing.Size(550, 296);
            this.tabStripBottom.StripAtTop = true;
            this.tabStripBottom.TabIndex = 0;
            // 
            // splitContainerRightOuter
            // 
            this.splitContainerRightOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRightOuter.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRightOuter.Name = "splitContainerRightOuter";
            this.splitContainerRightOuter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRightOuter.Panel1
            // 
            this.splitContainerRightOuter.Panel1.Controls.Add(this.panelTarget);
            this.splitContainerRightOuter.Panel1.Controls.Add(this.panelNoteArea);
            this.splitContainerRightOuter.Panel1.Controls.Add(this.panel_system);
            this.splitContainerRightOuter.Panel1.Controls.Add(this.panel_topright);
            // 
            // splitContainerRightOuter.Panel2
            // 
            this.splitContainerRightOuter.Panel2.Controls.Add(this.splitContainerRightInner);
            this.splitContainerRightOuter.Size = new System.Drawing.Size(337, 650);
            this.splitContainerRightOuter.SplitterDistance = 325;
            this.splitContainerRightOuter.TabIndex = 17;
            // 
            // panelTarget
            // 
            this.panelTarget.Controls.Add(this.textBoxTargetDist);
            this.panelTarget.Controls.Add(this.textBoxTarget);
            this.panelTarget.Controls.Add(this.labelTarget);
            this.panelTarget.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTarget.Location = new System.Drawing.Point(0, 284);
            this.panelTarget.Name = "panelTarget";
            this.panelTarget.Size = new System.Drawing.Size(337, 32);
            this.panelTarget.TabIndex = 44;
            this.panelTarget.Resize += new System.EventHandler(this.panelTarget_Resize);
            // 
            // labelTarget
            // 
            this.labelTarget.AutoSize = true;
            this.labelTarget.Location = new System.Drawing.Point(2, 9);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(38, 13);
            this.labelTarget.TabIndex = 16;
            this.labelTarget.Text = "Target";
            // 
            // panelNoteArea
            // 
            this.panelNoteArea.Controls.Add(this.richTextBoxNote);
            this.panelNoteArea.Controls.Add(this.labelNote);
            this.panelNoteArea.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNoteArea.Location = new System.Drawing.Point(0, 234);
            this.panelNoteArea.Name = "panelNoteArea";
            this.panelNoteArea.Size = new System.Drawing.Size(337, 50);
            this.panelNoteArea.TabIndex = 44;
            this.panelNoteArea.Resize += new System.EventHandler(this.panelNoteArea_Resize);
            // 
            // richTextBoxNote
            // 
            this.richTextBoxNote.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBoxNote.BorderColorScaling = 0.5F;
            this.richTextBoxNote.HideScrollBar = true;
            this.richTextBoxNote.Location = new System.Drawing.Point(30, 0);
            this.richTextBoxNote.Name = "richTextBoxNote";
            this.richTextBoxNote.ScrollBarWidth = 20;
            this.richTextBoxNote.ShowLineCount = false;
            this.richTextBoxNote.Size = new System.Drawing.Size(277, 50);
            this.richTextBoxNote.TabIndex = 0;
            this.richTextBoxNote.TextChanged += new System.EventHandler(this.richTextBoxNote_TextChanged);
            this.richTextBoxNote.Leave += new System.EventHandler(this.richTextBoxNote_Leave);
            // 
            // panel_system
            // 
            this.panel_system.Controls.Add(this.buttonEDSM);
            this.panel_system.Controls.Add(this.labelHomeSystem);
            this.panel_system.Controls.Add(this.textBoxHomeDist);
            this.panel_system.Controls.Add(this.buttonRoss);
            this.panel_system.Controls.Add(this.buttonEDDB);
            this.panel_system.Controls.Add(this.textBoxState);
            this.panel_system.Controls.Add(this.textBoxEconomy);
            this.panel_system.Controls.Add(this.label12);
            this.panel_system.Controls.Add(this.label13);
            this.panel_system.Controls.Add(this.textBoxGovernment);
            this.panel_system.Controls.Add(this.textBoxAllegiance);
            this.panel_system.Controls.Add(this.label11);
            this.panel_system.Controls.Add(this.label10);
            this.panel_system.Controls.Add(this.label9);
            this.panel_system.Controls.Add(this.textBoxVisits);
            this.panel_system.Controls.Add(this.label_Z);
            this.panel_system.Controls.Add(this.textBoxZ);
            this.panel_system.Controls.Add(this.labelDistEnter);
            this.panel_system.Controls.Add(this.label5);
            this.panel_system.Controls.Add(this.textBoxY);
            this.panel_system.Controls.Add(this.textBoxX);
            this.panel_system.Controls.Add(this.label4);
            this.panel_system.Controls.Add(this.textBoxSystem);
            this.panel_system.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_system.Location = new System.Drawing.Point(0, 100);
            this.panel_system.Name = "panel_system";
            this.panel_system.Size = new System.Drawing.Size(337, 134);
            this.panel_system.TabIndex = 6;
            // 
            // labelHomeSystem
            // 
            this.labelHomeSystem.AutoSize = true;
            this.labelHomeSystem.Location = new System.Drawing.Point(1, 84);
            this.labelHomeSystem.Name = "labelHomeSystem";
            this.labelHomeSystem.Size = new System.Drawing.Size(35, 13);
            this.labelHomeSystem.TabIndex = 43;
            this.labelHomeSystem.Text = "Home";
            this.labelHomeSystem.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxState
            // 
            this.textBoxState.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxState.BorderColorScaling = 0.5F;
            this.textBoxState.Location = new System.Drawing.Point(186, 84);
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.ReadOnly = true;
            this.textBoxState.Size = new System.Drawing.Size(66, 20);
            this.textBoxState.TabIndex = 37;
            this.textBoxState.TabStop = false;
            // 
            // textBoxEconomy
            // 
            this.textBoxEconomy.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxEconomy.BorderColorScaling = 0.5F;
            this.textBoxEconomy.Location = new System.Drawing.Point(186, 66);
            this.textBoxEconomy.Name = "textBoxEconomy";
            this.textBoxEconomy.ReadOnly = true;
            this.textBoxEconomy.Size = new System.Drawing.Size(66, 20);
            this.textBoxEconomy.TabIndex = 33;
            this.textBoxEconomy.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(144, 84);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 13);
            this.label12.TabIndex = 38;
            this.label12.Text = "State";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(149, 102);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(27, 13);
            this.label13.TabIndex = 36;
            this.label13.Text = "Gov";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxGovernment
            // 
            this.textBoxGovernment.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxGovernment.BorderColorScaling = 0.5F;
            this.textBoxGovernment.Location = new System.Drawing.Point(186, 102);
            this.textBoxGovernment.Name = "textBoxGovernment";
            this.textBoxGovernment.ReadOnly = true;
            this.textBoxGovernment.Size = new System.Drawing.Size(66, 20);
            this.textBoxGovernment.TabIndex = 35;
            this.textBoxGovernment.TabStop = false;
            // 
            // textBoxAllegiance
            // 
            this.textBoxAllegiance.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxAllegiance.BorderColorScaling = 0.5F;
            this.textBoxAllegiance.Location = new System.Drawing.Point(186, 48);
            this.textBoxAllegiance.Name = "textBoxAllegiance";
            this.textBoxAllegiance.ReadOnly = true;
            this.textBoxAllegiance.Size = new System.Drawing.Size(66, 20);
            this.textBoxAllegiance.TabIndex = 31;
            this.textBoxAllegiance.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(125, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(51, 13);
            this.label11.TabIndex = 34;
            this.label11.Text = "Economy";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(120, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(56, 13);
            this.label10.TabIndex = 32;
            this.label10.Text = "Allegiance";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(145, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 30;
            this.label9.Text = "Visits";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxVisits
            // 
            this.textBoxVisits.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxVisits.BorderColorScaling = 0.5F;
            this.textBoxVisits.Location = new System.Drawing.Point(186, 30);
            this.textBoxVisits.Name = "textBoxVisits";
            this.textBoxVisits.ReadOnly = true;
            this.textBoxVisits.Size = new System.Drawing.Size(66, 20);
            this.textBoxVisits.TabIndex = 29;
            this.textBoxVisits.TabStop = false;
            // 
            // label_Z
            // 
            this.label_Z.AutoSize = true;
            this.label_Z.Location = new System.Drawing.Point(30, 66);
            this.label_Z.Name = "label_Z";
            this.label_Z.Size = new System.Drawing.Size(14, 13);
            this.label_Z.TabIndex = 20;
            this.label_Z.Text = "Z";
            this.label_Z.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxZ
            // 
            this.textBoxZ.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxZ.BorderColorScaling = 0.5F;
            this.textBoxZ.Location = new System.Drawing.Point(50, 66);
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.ReadOnly = true;
            this.textBoxZ.Size = new System.Drawing.Size(67, 20);
            this.textBoxZ.TabIndex = 19;
            this.textBoxZ.TabStop = false;
            // 
            // labelDistEnter
            // 
            this.labelDistEnter.AutoSize = true;
            this.labelDistEnter.ForeColor = System.Drawing.Color.Black;
            this.labelDistEnter.Location = new System.Drawing.Point(30, 48);
            this.labelDistEnter.Name = "labelDistEnter";
            this.labelDistEnter.Size = new System.Drawing.Size(14, 13);
            this.labelDistEnter.TabIndex = 18;
            this.labelDistEnter.Text = "Y";
            this.labelDistEnter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 18;
            this.label5.Text = "X";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxY
            // 
            this.textBoxY.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxY.BorderColorScaling = 0.5F;
            this.textBoxY.Location = new System.Drawing.Point(50, 48);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.ReadOnly = true;
            this.textBoxY.Size = new System.Drawing.Size(67, 20);
            this.textBoxY.TabIndex = 17;
            this.textBoxY.TabStop = false;
            // 
            // textBoxX
            // 
            this.textBoxX.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxX.BorderColorScaling = 0.5F;
            this.textBoxX.Location = new System.Drawing.Point(50, 30);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.ReadOnly = true;
            this.textBoxX.Size = new System.Drawing.Size(67, 20);
            this.textBoxX.TabIndex = 17;
            this.textBoxX.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "System";
            // 
            // textBoxSystem
            // 
            this.textBoxSystem.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSystem.BorderColorScaling = 0.5F;
            this.textBoxSystem.Location = new System.Drawing.Point(50, 10);
            this.textBoxSystem.Name = "textBoxSystem";
            this.textBoxSystem.ReadOnly = true;
            this.textBoxSystem.Size = new System.Drawing.Size(152, 20);
            this.textBoxSystem.TabIndex = 15;
            this.textBoxSystem.TabStop = false;
            // 
            // panel_topright
            // 
            this.panel_topright.Controls.Add(this.comboBoxCustomPopOut);
            this.panel_topright.Controls.Add(this.button_RefreshHistory);
            this.panel_topright.Controls.Add(this.buttonMap2D);
            this.panel_topright.Controls.Add(this.comboBoxCommander);
            this.panel_topright.Controls.Add(this.labelCMDR);
            this.panel_topright.Controls.Add(this.buttonMap);
            this.panel_topright.Controls.Add(this.buttonSync);
            this.panel_topright.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_topright.Location = new System.Drawing.Point(0, 0);
            this.panel_topright.Name = "panel_topright";
            this.panel_topright.Size = new System.Drawing.Size(337, 100);
            this.panel_topright.TabIndex = 26;
            this.panel_topright.Resize += new System.EventHandler(this.panel_topright_Resize);
            // 
            // labelCMDR
            // 
            this.labelCMDR.AutoSize = true;
            this.labelCMDR.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCMDR.Location = new System.Drawing.Point(77, 9);
            this.labelCMDR.Name = "labelCMDR";
            this.labelCMDR.Size = new System.Drawing.Size(31, 13);
            this.labelCMDR.TabIndex = 17;
            this.labelCMDR.Text = "Cmdr";
            // 
            // splitContainerRightInner
            // 
            this.splitContainerRightInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerRightInner.Location = new System.Drawing.Point(0, 0);
            this.splitContainerRightInner.Name = "splitContainerRightInner";
            this.splitContainerRightInner.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerRightInner.Panel1
            // 
            this.splitContainerRightInner.Panel1.Controls.Add(this.tabStripMiddleRight);
            // 
            // splitContainerRightInner.Panel2
            // 
            this.splitContainerRightInner.Panel2.Controls.Add(this.tabStripBottomRight);
            this.splitContainerRightInner.Size = new System.Drawing.Size(337, 321);
            this.splitContainerRightInner.SplitterDistance = 160;
            this.splitContainerRightInner.TabIndex = 7;
            // 
            // tabStripMiddleRight
            // 
            this.tabStripMiddleRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStripMiddleRight.Location = new System.Drawing.Point(0, 0);
            this.tabStripMiddleRight.Name = "tabStripMiddleRight";
            this.tabStripMiddleRight.SelectedIndex = -1;
            this.tabStripMiddleRight.ShowPopOut = true;
            this.tabStripMiddleRight.Size = new System.Drawing.Size(337, 160);
            this.tabStripMiddleRight.StripAtTop = true;
            this.tabStripMiddleRight.TabIndex = 1;
            // 
            // tabStripBottomRight
            // 
            this.tabStripBottomRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStripBottomRight.Location = new System.Drawing.Point(0, 0);
            this.tabStripBottomRight.Name = "tabStripBottomRight";
            this.tabStripBottomRight.SelectedIndex = -1;
            this.tabStripBottomRight.ShowPopOut = true;
            this.tabStripBottomRight.Size = new System.Drawing.Size(337, 157);
            this.tabStripBottomRight.StripAtTop = true;
            this.tabStripBottomRight.TabIndex = 0;
            // 
            // TravelHistoryControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerLeftRight);
            this.Name = "TravelHistoryControl";
            this.Size = new System.Drawing.Size(891, 650);
            this.splitContainerLeftRight.Panel1.ResumeLayout(false);
            this.splitContainerLeftRight.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeftRight)).EndInit();
            this.splitContainerLeftRight.ResumeLayout(false);
            this.splitContainerLeft.Panel1.ResumeLayout(false);
            this.splitContainerLeft.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLeft)).EndInit();
            this.splitContainerLeft.ResumeLayout(false);
            this.splitContainerRightOuter.Panel1.ResumeLayout(false);
            this.splitContainerRightOuter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightOuter)).EndInit();
            this.splitContainerRightOuter.ResumeLayout(false);
            this.panelTarget.ResumeLayout(false);
            this.panelTarget.PerformLayout();
            this.panelNoteArea.ResumeLayout(false);
            this.panelNoteArea.PerformLayout();
            this.panel_system.ResumeLayout(false);
            this.panel_system.PerformLayout();
            this.panel_topright.ResumeLayout(false);
            this.panel_topright.PerformLayout();
            this.splitContainerRightInner.Panel1.ResumeLayout(false);
            this.splitContainerRightInner.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerRightInner)).EndInit();
            this.splitContainerRightInner.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ButtonExt button_RefreshHistory;
        private ExtendedControls.ButtonExt buttonMap;
        private ExtendedControls.TextBoxBorder textBoxSystem;
        private System.Windows.Forms.Panel panel_system;
        private System.Windows.Forms.Label label_Z;
        private ExtendedControls.TextBoxBorder textBoxZ;
        private System.Windows.Forms.Label labelDistEnter;
        private System.Windows.Forms.Label label5;
        private ExtendedControls.TextBoxBorder textBoxY;
        private ExtendedControls.TextBoxBorder textBoxX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelCMDR;
        private ExtendedControls.RichTextBoxScroll richTextBoxNote;
        private ExtendedControls.ButtonExt buttonSync;
        private System.Windows.Forms.Label label9;
        private ExtendedControls.TextBoxBorder textBoxVisits;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Label label11;
        private ExtendedControls.TextBoxBorder textBoxEconomy;
        private System.Windows.Forms.Label label10;
        private ExtendedControls.TextBoxBorder textBoxAllegiance;
        private System.Windows.Forms.Label label12;
        private ExtendedControls.TextBoxBorder textBoxState;
        private System.Windows.Forms.Label label13;
        private ExtendedControls.TextBoxBorder textBoxGovernment;
        private ExtendedControls.DrawnPanel buttonEDDB;
        private System.Windows.Forms.ToolTip toolTipEddb;
        private ExtendedControls.DrawnPanel buttonRoss;
        private System.Windows.Forms.Label labelHomeSystem;
        private ExtendedControls.TextBoxBorder textBoxHomeDist;
        private ExtendedControls.ComboBoxCustom comboBoxCommander;
        private System.Windows.Forms.Panel panel_topright;
        private ExtendedControls.ButtonExt buttonMap2D;
        private ExtendedControls.DrawnPanel buttonEDSM;
        private System.Windows.Forms.Label labelTarget;
        private ExtendedControls.TextBoxBorder textBoxTargetDist;
        private ExtendedControls.AutoCompleteTextBox textBoxTarget;
        private System.Windows.Forms.SplitContainer splitContainerLeft;
        private System.Windows.Forms.SplitContainer splitContainerLeftRight;
        private System.Windows.Forms.SplitContainer splitContainerRightInner;
        private System.Windows.Forms.Panel panelTarget;
        private System.Windows.Forms.Panel panelNoteArea;
        private UserControls.UserControlTravelGrid userControlTravelGrid;
        private ExtendedControls.ComboBoxCustom comboBoxCustomPopOut;
        private System.Windows.Forms.SplitContainer splitContainerRightOuter;
        private Controls.TabStrip tabStripBottom;
        private Controls.TabStrip tabStripMiddleRight;
        private Controls.TabStrip tabStripBottomRight;
    }
}
