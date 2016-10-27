namespace EDDiscovery2
{
    partial class Settings
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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxTheme = new ExtendedControls.GroupBoxCustom();
            this.checkBoxKeepOnTop = new ExtendedControls.CheckBoxCustom();
            this.comboBoxTheme = new ExtendedControls.ComboBoxCustom();
            this.button_edittheme = new ExtendedControls.ButtonExt();
            this.buttonSaveTheme = new ExtendedControls.ButtonExt();
            this.groupBox2 = new ExtendedControls.GroupBoxCustom();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxDefaultZoom = new ExtendedControls.TextBoxBorder();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButtonHistorySelection = new ExtendedControls.RadioButtonCustom();
            this.radioButtonCentreHome = new ExtendedControls.RadioButtonCustom();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxHomeSystem = new ExtendedControls.AutoCompleteTextBox();
            this.panel_defaultmapcolor = new System.Windows.Forms.Panel();
            this.groupBox3 = new ExtendedControls.GroupBoxCustom();
            this.checkBoxFocusNewSystem = new ExtendedControls.CheckBoxCustom();
            this.checkBoxUTC = new ExtendedControls.CheckBoxCustom();
            this.checkBoxOrderRowsInverted = new ExtendedControls.CheckBoxCustom();
            this.checkBoxEDSMLog = new ExtendedControls.CheckBoxCustom();
            this.checkboxSkipSlowUpdates = new ExtendedControls.CheckBoxCustom();
            this.groupBox4 = new ExtendedControls.GroupBoxCustom();
            this.btnDeleteCommander = new ExtendedControls.ButtonExt();
            this.buttonAddCommander = new ExtendedControls.ButtonExt();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewCommanders = new System.Windows.Forms.DataGridView();
            this.ColumnNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommander = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EdsmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEDSMAPIKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnJournalDir = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSyncToEDSM = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnSyncFromEDSM = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnSyncToEddn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnDeleted = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnNetLogDirOld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBoxTheme.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBoxTheme
            // 
            this.groupBoxTheme.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxTheme.BackColorScaling = 0.5F;
            this.groupBoxTheme.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxTheme.BorderColorScaling = 0.5F;
            this.groupBoxTheme.Controls.Add(this.checkBoxKeepOnTop);
            this.groupBoxTheme.Controls.Add(this.comboBoxTheme);
            this.groupBoxTheme.Controls.Add(this.button_edittheme);
            this.groupBoxTheme.Controls.Add(this.buttonSaveTheme);
            this.groupBoxTheme.FillClientAreaWithAlternateColor = false;
            this.groupBoxTheme.Location = new System.Drawing.Point(3, 345);
            this.groupBoxTheme.Name = "groupBoxTheme";
            this.groupBoxTheme.Size = new System.Drawing.Size(426, 166);
            this.groupBoxTheme.TabIndex = 18;
            this.groupBoxTheme.TabStop = false;
            this.groupBoxTheme.Text = "Theme";
            this.groupBoxTheme.TextPadding = 0;
            this.groupBoxTheme.TextStartPosition = -1;
            // 
            // checkBoxKeepOnTop
            // 
            this.checkBoxKeepOnTop.AutoSize = true;
            this.checkBoxKeepOnTop.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxKeepOnTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxKeepOnTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxKeepOnTop.FontNerfReduction = 0.5F;
            this.checkBoxKeepOnTop.Location = new System.Drawing.Point(7, 63);
            this.checkBoxKeepOnTop.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxKeepOnTop.Name = "checkBoxKeepOnTop";
            this.checkBoxKeepOnTop.Size = new System.Drawing.Size(88, 17);
            this.checkBoxKeepOnTop.TabIndex = 5;
            this.checkBoxKeepOnTop.Text = "Keep on Top";
            this.checkBoxKeepOnTop.TickBoxReductionSize = 10;
            this.checkBoxKeepOnTop.UseVisualStyleBackColor = true;
            this.checkBoxKeepOnTop.CheckedChanged += new System.EventHandler(this.checkBoxKeepOnTop_CheckedChanged);
            // 
            // comboBoxTheme
            // 
            this.comboBoxTheme.ArrowWidth = 1;
            this.comboBoxTheme.BackColor = System.Drawing.Color.Gray;
            this.comboBoxTheme.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTheme.ButtonColorScaling = 0.5F;
            this.comboBoxTheme.DataSource = null;
            this.comboBoxTheme.DisplayMember = "";
            this.comboBoxTheme.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxTheme.DropDownHeight = 100;
            this.comboBoxTheme.DropDownWidth = 267;
            this.comboBoxTheme.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTheme.ItemHeight = 13;
            this.comboBoxTheme.Location = new System.Drawing.Point(7, 19);
            this.comboBoxTheme.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxTheme.Name = "comboBoxTheme";
            this.comboBoxTheme.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxTheme.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxTheme.ScrollBarWidth = 16;
            this.comboBoxTheme.SelectedIndex = -1;
            this.comboBoxTheme.SelectedItem = null;
            this.comboBoxTheme.SelectedValue = null;
            this.comboBoxTheme.Size = new System.Drawing.Size(267, 21);
            this.comboBoxTheme.TabIndex = 0;
            this.comboBoxTheme.ValueMember = "";
            // 
            // button_edittheme
            // 
            this.button_edittheme.BorderColorScaling = 1.25F;
            this.button_edittheme.ButtonColorScaling = 0.5F;
            this.button_edittheme.ButtonDisabledScaling = 0.5F;
            this.button_edittheme.Location = new System.Drawing.Point(291, 63);
            this.button_edittheme.Name = "button_edittheme";
            this.button_edittheme.Size = new System.Drawing.Size(95, 23);
            this.button_edittheme.TabIndex = 10;
            this.button_edittheme.Text = "Edit Theme";
            this.button_edittheme.UseVisualStyleBackColor = true;
            this.button_edittheme.Click += new System.EventHandler(this.button_edittheme_Click);
            // 
            // buttonSaveTheme
            // 
            this.buttonSaveTheme.BorderColorScaling = 1.25F;
            this.buttonSaveTheme.ButtonColorScaling = 0.5F;
            this.buttonSaveTheme.ButtonDisabledScaling = 0.5F;
            this.buttonSaveTheme.Location = new System.Drawing.Point(291, 19);
            this.buttonSaveTheme.Name = "buttonSaveTheme";
            this.buttonSaveTheme.Size = new System.Drawing.Size(95, 23);
            this.buttonSaveTheme.TabIndex = 7;
            this.buttonSaveTheme.Text = "Save Theme";
            this.buttonSaveTheme.UseVisualStyleBackColor = true;
            this.buttonSaveTheme.Click += new System.EventHandler(this.buttonSaveTheme_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox2.BackColorScaling = 0.5F;
            this.groupBox2.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox2.BorderColorScaling = 0.5F;
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.textBoxDefaultZoom);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.radioButtonHistorySelection);
            this.groupBox2.Controls.Add(this.radioButtonCentreHome);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.textBoxHomeSystem);
            this.groupBox2.Controls.Add(this.panel_defaultmapcolor);
            this.groupBox2.FillClientAreaWithAlternateColor = false;
            this.groupBox2.Location = new System.Drawing.Point(440, 217);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(379, 100);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "3D Map Settings";
            this.groupBox2.TextPadding = 0;
            this.groupBox2.TextStartPosition = -1;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(224, 73);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(92, 13);
            this.label17.TabIndex = 7;
            this.label17.Text = "Default Map Color";
            // 
            // textBoxDefaultZoom
            // 
            this.textBoxDefaultZoom.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxDefaultZoom.BorderColorScaling = 0.5F;
            this.textBoxDefaultZoom.Location = new System.Drawing.Point(120, 70);
            this.textBoxDefaultZoom.Name = "textBoxDefaultZoom";
            this.textBoxDefaultZoom.Size = new System.Drawing.Size(51, 20);
            this.textBoxDefaultZoom.TabIndex = 6;
            this.textBoxDefaultZoom.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDefaultZoom_Validating);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Default Zoom";
            // 
            // radioButtonHistorySelection
            // 
            this.radioButtonHistorySelection.AutoSize = true;
            this.radioButtonHistorySelection.FontNerfReduction = 0.5F;
            this.radioButtonHistorySelection.Location = new System.Drawing.Point(224, 46);
            this.radioButtonHistorySelection.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonHistorySelection.Name = "radioButtonHistorySelection";
            this.radioButtonHistorySelection.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonHistorySelection.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonHistorySelection.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonHistorySelection.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButtonHistorySelection.Size = new System.Drawing.Size(137, 17);
            this.radioButtonHistorySelection.TabIndex = 4;
            this.radioButtonHistorySelection.TabStop = true;
            this.radioButtonHistorySelection.Text = "Travel History Selection";
            this.radioButtonHistorySelection.UseVisualStyleBackColor = true;
            // 
            // radioButtonCentreHome
            // 
            this.radioButtonCentreHome.AutoSize = true;
            this.radioButtonCentreHome.FontNerfReduction = 0.5F;
            this.radioButtonCentreHome.Location = new System.Drawing.Point(120, 46);
            this.radioButtonCentreHome.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonCentreHome.Name = "radioButtonCentreHome";
            this.radioButtonCentreHome.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonCentreHome.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonCentreHome.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonCentreHome.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButtonCentreHome.Size = new System.Drawing.Size(90, 17);
            this.radioButtonCentreHome.TabIndex = 3;
            this.radioButtonCentreHome.TabStop = true;
            this.radioButtonCentreHome.Text = "Home System";
            this.radioButtonCentreHome.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Open Centred On";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Home System";
            // 
            // textBoxHomeSystem
            // 
            this.textBoxHomeSystem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxHomeSystem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxHomeSystem.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxHomeSystem.BorderColorScaling = 0.5F;
            this.textBoxHomeSystem.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.textBoxHomeSystem.DropDownBorderColor = System.Drawing.Color.Green;
            this.textBoxHomeSystem.DropDownHeight = 200;
            this.textBoxHomeSystem.DropDownItemHeight = 20;
            this.textBoxHomeSystem.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.textBoxHomeSystem.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.textBoxHomeSystem.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.textBoxHomeSystem.DropDownWidth = 0;
            this.textBoxHomeSystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxHomeSystem.Location = new System.Drawing.Point(120, 19);
            this.textBoxHomeSystem.Name = "textBoxHomeSystem";
            this.textBoxHomeSystem.Size = new System.Drawing.Size(221, 20);
            this.textBoxHomeSystem.TabIndex = 0;
            // 
            // panel_defaultmapcolor
            // 
            this.panel_defaultmapcolor.AccessibleDescription = "";
            this.panel_defaultmapcolor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel_defaultmapcolor.Location = new System.Drawing.Point(335, 70);
            this.panel_defaultmapcolor.Name = "panel_defaultmapcolor";
            this.panel_defaultmapcolor.Size = new System.Drawing.Size(28, 20);
            this.panel_defaultmapcolor.TabIndex = 5;
            this.panel_defaultmapcolor.Tag = "";
            this.panel_defaultmapcolor.Click += new System.EventHandler(this.panel_defaultmapcolor_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox3.BackColorScaling = 0.5F;
            this.groupBox3.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox3.BorderColorScaling = 0.5F;
            this.groupBox3.Controls.Add(this.checkBoxFocusNewSystem);
            this.groupBox3.Controls.Add(this.checkBoxUTC);
            this.groupBox3.Controls.Add(this.checkBoxOrderRowsInverted);
            this.groupBox3.Controls.Add(this.checkBoxEDSMLog);
            this.groupBox3.Controls.Add(this.checkboxSkipSlowUpdates);
            this.groupBox3.FillClientAreaWithAlternateColor = false;
            this.groupBox3.Location = new System.Drawing.Point(3, 217);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(426, 122);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options";
            this.groupBox3.TextPadding = 0;
            this.groupBox3.TextStartPosition = -1;
            // 
            // checkBoxFocusNewSystem
            // 
            this.checkBoxFocusNewSystem.AutoSize = true;
            this.checkBoxFocusNewSystem.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxFocusNewSystem.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxFocusNewSystem.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxFocusNewSystem.FontNerfReduction = 0.5F;
            this.checkBoxFocusNewSystem.Location = new System.Drawing.Point(17, 69);
            this.checkBoxFocusNewSystem.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxFocusNewSystem.Name = "checkBoxFocusNewSystem";
            this.checkBoxFocusNewSystem.Size = new System.Drawing.Size(132, 17);
            this.checkBoxFocusNewSystem.TabIndex = 3;
            this.checkBoxFocusNewSystem.Text = "Focus on New System";
            this.checkBoxFocusNewSystem.TickBoxReductionSize = 10;
            this.checkBoxFocusNewSystem.UseVisualStyleBackColor = true;
            this.checkBoxFocusNewSystem.CheckedChanged += new System.EventHandler(this.checkBoxFocusNewSystem_CheckedChanged);
            // 
            // checkBoxUTC
            // 
            this.checkBoxUTC.AutoSize = true;
            this.checkBoxUTC.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxUTC.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxUTC.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxUTC.FontNerfReduction = 0.5F;
            this.checkBoxUTC.Location = new System.Drawing.Point(17, 92);
            this.checkBoxUTC.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxUTC.Name = "checkBoxUTC";
            this.checkBoxUTC.Size = new System.Drawing.Size(209, 17);
            this.checkBoxUTC.TabIndex = 0;
            this.checkBoxUTC.Text = "Display Game time instead of local time";
            this.checkBoxUTC.TickBoxReductionSize = 10;
            this.checkBoxUTC.UseVisualStyleBackColor = true;
            this.checkBoxUTC.CheckedChanged += new System.EventHandler(this.checkBoxUTC_CheckedChanged);
            // 
            // checkBoxOrderRowsInverted
            // 
            this.checkBoxOrderRowsInverted.AutoSize = true;
            this.checkBoxOrderRowsInverted.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxOrderRowsInverted.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxOrderRowsInverted.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxOrderRowsInverted.FontNerfReduction = 0.5F;
            this.checkBoxOrderRowsInverted.Location = new System.Drawing.Point(17, 46);
            this.checkBoxOrderRowsInverted.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxOrderRowsInverted.Name = "checkBoxOrderRowsInverted";
            this.checkBoxOrderRowsInverted.Size = new System.Drawing.Size(196, 17);
            this.checkBoxOrderRowsInverted.TabIndex = 2;
            this.checkBoxOrderRowsInverted.Text = "Number Rows Lastest Entry Highest";
            this.checkBoxOrderRowsInverted.TickBoxReductionSize = 10;
            this.checkBoxOrderRowsInverted.UseVisualStyleBackColor = true;
            // 
            // checkBoxEDSMLog
            // 
            this.checkBoxEDSMLog.AutoSize = true;
            this.checkBoxEDSMLog.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxEDSMLog.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEDSMLog.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEDSMLog.FontNerfReduction = 0.5F;
            this.checkBoxEDSMLog.Location = new System.Drawing.Point(17, 23);
            this.checkBoxEDSMLog.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEDSMLog.Name = "checkBoxEDSMLog";
            this.checkBoxEDSMLog.Size = new System.Drawing.Size(121, 17);
            this.checkBoxEDSMLog.TabIndex = 1;
            this.checkBoxEDSMLog.Text = "Log EDSM requests";
            this.checkBoxEDSMLog.TickBoxReductionSize = 10;
            this.checkBoxEDSMLog.UseVisualStyleBackColor = true;
            // 
            // checkboxSkipSlowUpdates
            // 
            this.checkboxSkipSlowUpdates.AutoSize = true;
            this.checkboxSkipSlowUpdates.BackColor = System.Drawing.Color.Transparent;
            this.checkboxSkipSlowUpdates.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkboxSkipSlowUpdates.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkboxSkipSlowUpdates.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkboxSkipSlowUpdates.FontNerfReduction = 0.5F;
            this.checkboxSkipSlowUpdates.Location = new System.Drawing.Point(182, 21);
            this.checkboxSkipSlowUpdates.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkboxSkipSlowUpdates.Name = "checkboxSkipSlowUpdates";
            this.checkboxSkipSlowUpdates.Size = new System.Drawing.Size(238, 17);
            this.checkboxSkipSlowUpdates.TabIndex = 4;
            this.checkboxSkipSlowUpdates.Text = "DEBUG ONLY: Skip slow updates on startup";
            this.checkboxSkipSlowUpdates.TickBoxReductionSize = 10;
            this.checkboxSkipSlowUpdates.UseVisualStyleBackColor = false;
            this.checkboxSkipSlowUpdates.Visible = false;
            // 
            // groupBox4
            // 
            this.groupBox4.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox4.BackColorScaling = 0.5F;
            this.groupBox4.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox4.BorderColorScaling = 0.5F;
            this.groupBox4.Controls.Add(this.btnDeleteCommander);
            this.groupBox4.Controls.Add(this.buttonAddCommander);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.dataGridViewCommanders);
            this.groupBox4.FillClientAreaWithAlternateColor = false;
            this.groupBox4.Location = new System.Drawing.Point(0, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(819, 210);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Commanders";
            this.groupBox4.TextPadding = 0;
            this.groupBox4.TextStartPosition = -1;
            // 
            // btnDeleteCommander
            // 
            this.btnDeleteCommander.BorderColorScaling = 1.25F;
            this.btnDeleteCommander.ButtonColorScaling = 0.5F;
            this.btnDeleteCommander.ButtonDisabledScaling = 0.5F;
            this.btnDeleteCommander.Location = new System.Drawing.Point(680, 16);
            this.btnDeleteCommander.Name = "btnDeleteCommander";
            this.btnDeleteCommander.Size = new System.Drawing.Size(121, 23);
            this.btnDeleteCommander.TabIndex = 3;
            this.btnDeleteCommander.Text = "Delete Commander";
            this.btnDeleteCommander.UseVisualStyleBackColor = true;
            this.btnDeleteCommander.Click += new System.EventHandler(this.btnDeleteCommander_Click);
            // 
            // buttonAddCommander
            // 
            this.buttonAddCommander.BorderColorScaling = 1.25F;
            this.buttonAddCommander.ButtonColorScaling = 0.5F;
            this.buttonAddCommander.ButtonDisabledScaling = 0.5F;
            this.buttonAddCommander.Location = new System.Drawing.Point(11, 16);
            this.buttonAddCommander.Name = "buttonAddCommander";
            this.buttonAddCommander.Size = new System.Drawing.Size(104, 23);
            this.buttonAddCommander.TabIndex = 0;
            this.buttonAddCommander.Text = "Add commander";
            this.buttonAddCommander.UseVisualStyleBackColor = true;
            this.buttonAddCommander.Click += new System.EventHandler(this.buttonAddCommander_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(133, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(416, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Leave Override Journal Location blank to use the standard Frontier location for j" +
    "ournals";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(134, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(354, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Get an EDSM API key from https://www.edsm.net in \"My account\" menu";
            // 
            // dataGridViewCommanders
            // 
            this.dataGridViewCommanders.AllowUserToAddRows = false;
            this.dataGridViewCommanders.AllowUserToDeleteRows = false;
            this.dataGridViewCommanders.AllowUserToOrderColumns = true;
            this.dataGridViewCommanders.AllowUserToResizeRows = false;
            this.dataGridViewCommanders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCommanders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCommanders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnNr,
            this.ColumnCommander,
            this.EdsmName,
            this.ColumnEDSMAPIKey,
            this.ColumnJournalDir,
            this.ColumnSyncToEDSM,
            this.ColumnSyncFromEDSM,
            this.ColumnSyncToEddn,
            this.ColumnDeleted,
            this.ColumnNetLogDirOld});
            this.dataGridViewCommanders.Location = new System.Drawing.Point(11, 71);
            this.dataGridViewCommanders.MultiSelect = false;
            this.dataGridViewCommanders.Name = "dataGridViewCommanders";
            this.dataGridViewCommanders.RowHeadersWidth = 20;
            this.dataGridViewCommanders.Size = new System.Drawing.Size(792, 128);
            this.dataGridViewCommanders.TabIndex = 2;
            this.dataGridViewCommanders.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCommanders_CellEndEdit);
            // 
            // ColumnNr
            // 
            this.ColumnNr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.ColumnNr.DataPropertyName = "Nr";
            this.ColumnNr.FillWeight = 15F;
            this.ColumnNr.HeaderText = "Nr";
            this.ColumnNr.MinimumWidth = 45;
            this.ColumnNr.Name = "ColumnNr";
            this.ColumnNr.ReadOnly = true;
            this.ColumnNr.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnNr.Width = 45;
            // 
            // ColumnCommander
            // 
            this.ColumnCommander.DataPropertyName = "Name";
            this.ColumnCommander.HeaderText = "Commander";
            this.ColumnCommander.MinimumWidth = 150;
            this.ColumnCommander.Name = "ColumnCommander";
            // 
            // EdsmName
            // 
            this.EdsmName.DataPropertyName = "EdsmName";
            this.EdsmName.HeaderText = "EDSM Name";
            this.EdsmName.Name = "EdsmName";
            // 
            // ColumnEDSMAPIKey
            // 
            this.ColumnEDSMAPIKey.DataPropertyName = "APIKey";
            this.ColumnEDSMAPIKey.HeaderText = "EDSM API key";
            this.ColumnEDSMAPIKey.MinimumWidth = 150;
            this.ColumnEDSMAPIKey.Name = "ColumnEDSMAPIKey";
            // 
            // ColumnJournalDir
            // 
            this.ColumnJournalDir.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnJournalDir.DataPropertyName = "JournalDir";
            this.ColumnJournalDir.HeaderText = "Override Journal Location";
            this.ColumnJournalDir.MinimumWidth = 50;
            this.ColumnJournalDir.Name = "ColumnJournalDir";
            // 
            // ColumnSyncToEDSM
            // 
            this.ColumnSyncToEDSM.DataPropertyName = "SyncToEDSM";
            this.ColumnSyncToEDSM.FillWeight = 15F;
            this.ColumnSyncToEDSM.HeaderText = "Sync to EDSM";
            this.ColumnSyncToEDSM.MinimumWidth = 45;
            this.ColumnSyncToEDSM.Name = "ColumnSyncToEDSM";
            // 
            // ColumnSyncFromEDSM
            // 
            this.ColumnSyncFromEDSM.DataPropertyName = "SyncFromEdsm";
            this.ColumnSyncFromEDSM.FillWeight = 15F;
            this.ColumnSyncFromEDSM.HeaderText = "Sync From Edsm";
            this.ColumnSyncFromEDSM.MinimumWidth = 45;
            this.ColumnSyncFromEDSM.Name = "ColumnSyncFromEDSM";
            // 
            // ColumnSyncToEddn
            // 
            this.ColumnSyncToEddn.DataPropertyName = "SyncToEddn";
            this.ColumnSyncToEddn.FillWeight = 15F;
            this.ColumnSyncToEddn.HeaderText = "Sync to Eddn";
            this.ColumnSyncToEddn.MinimumWidth = 45;
            this.ColumnSyncToEddn.Name = "ColumnSyncToEddn";
            // 
            // ColumnDeleted
            // 
            this.ColumnDeleted.DataPropertyName = "Deleted";
            this.ColumnDeleted.FillWeight = 10F;
            this.ColumnDeleted.HeaderText = "Deleted";
            this.ColumnDeleted.Name = "ColumnDeleted";
            this.ColumnDeleted.ReadOnly = true;
            this.ColumnDeleted.Visible = false;
            // 
            // ColumnNetLogDirOld
            // 
            this.ColumnNetLogDirOld.DataPropertyName = "NetLogDir";
            this.ColumnNetLogDirOld.HeaderText = "NetLogDir";
            this.ColumnNetLogDirOld.Name = "ColumnNetLogDirOld";
            this.ColumnNetLogDirOld.ReadOnly = true;
            this.ColumnNetLogDirOld.Visible = false;
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTheme);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(937, 725);
            this.groupBoxTheme.ResumeLayout(false);
            this.groupBoxTheme.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.GroupBoxCustom groupBox4;
        private ExtendedControls.ButtonExt buttonAddCommander;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridViewCommanders;
        private ExtendedControls.GroupBoxCustom groupBox3;
        private ExtendedControls.CheckBoxCustom checkBoxEDSMLog;
        public ExtendedControls.CheckBoxCustom checkboxSkipSlowUpdates;
        private ExtendedControls.GroupBoxCustom groupBox2;
        private ExtendedControls.TextBoxBorder textBoxDefaultZoom;
        private System.Windows.Forms.Label label5;
        private ExtendedControls.RadioButtonCustom radioButtonHistorySelection;
        private ExtendedControls.RadioButtonCustom radioButtonCentreHome;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private ExtendedControls.AutoCompleteTextBox textBoxHomeSystem;
        private ExtendedControls.ComboBoxCustom comboBoxTheme;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panel_defaultmapcolor;
        private ExtendedControls.ButtonExt buttonSaveTheme;
        private System.Windows.Forms.Label label17;
        private ExtendedControls.ButtonExt button_edittheme;
        private ExtendedControls.GroupBoxCustom groupBoxTheme;
        private ExtendedControls.CheckBoxCustom checkBoxOrderRowsInverted;
        private ExtendedControls.CheckBoxCustom checkBoxFocusNewSystem;
        private ExtendedControls.CheckBoxCustom checkBoxKeepOnTop;
        private ExtendedControls.ButtonExt btnDeleteCommander;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.CheckBoxCustom checkBoxUTC;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommander;
        private System.Windows.Forms.DataGridViewTextBoxColumn EdsmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEDSMAPIKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnJournalDir;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnSyncToEDSM;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnSyncFromEDSM;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnSyncToEddn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ColumnDeleted;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNetLogDirOld;
    }
}
