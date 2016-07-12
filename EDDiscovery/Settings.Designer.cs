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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxTheme = new ExtendedControls.GroupBoxCustom();
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
            this.textBoxHomeSystem = new ExtendedControls.TextBoxBorder();
            this.panel_defaultmapcolor = new System.Windows.Forms.Panel();
            this.groupBox3 = new ExtendedControls.GroupBoxCustom();
            this.checkBoxFocusNewSystem = new ExtendedControls.CheckBoxCustom();
            this.checkBox_Distances = new ExtendedControls.CheckBoxCustom();
            this.checkBoxOrderRowsInverted = new ExtendedControls.CheckBoxCustom();
            this.checkBoxEDSMLog = new ExtendedControls.CheckBoxCustom();
            this.checkboxSkipSlowUpdates = new ExtendedControls.CheckBoxCustom();
            this.groupBox4 = new ExtendedControls.GroupBoxCustom();
            this.buttonAddCommander = new ExtendedControls.ButtonExt();
            this.label2 = new System.Windows.Forms.Label();
            this.dataGridViewCommanders = new System.Windows.Forms.DataGridView();
            this.ColumnNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCommander = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEDSMAPIKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNetLogPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new ExtendedControls.GroupBoxCustom();
            this.button_Browse = new ExtendedControls.ButtonExt();
            this.textBoxNetLogDir = new ExtendedControls.TextBoxBorder();
            this.radioButton_Manual = new ExtendedControls.RadioButtonCustom();
            this.radioButton_Auto = new ExtendedControls.RadioButtonCustom();
            this.groupBoxTheme.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxTheme
            // 
            this.groupBoxTheme.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxTheme.BackColorScaling = 0.5F;
            this.groupBoxTheme.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxTheme.BorderColorScaling = 0.5F;
            this.groupBoxTheme.Controls.Add(this.comboBoxTheme);
            this.groupBoxTheme.Controls.Add(this.button_edittheme);
            this.groupBoxTheme.Controls.Add(this.buttonSaveTheme);
            this.groupBoxTheme.FillClientAreaWithAlternateColor = false;
            this.groupBoxTheme.Location = new System.Drawing.Point(3, 435);
            this.groupBoxTheme.Name = "groupBoxTheme";
            this.groupBoxTheme.Size = new System.Drawing.Size(426, 120);
            this.groupBoxTheme.TabIndex = 18;
            this.groupBoxTheme.TabStop = false;
            this.groupBoxTheme.Text = "Theme";
            this.groupBoxTheme.TextPadding = 0;
            this.groupBoxTheme.TextStartPosition = -1;
            // 
            // comboBoxTheme
            // 
            this.comboBoxTheme.ArrowWidth = 1;
            this.comboBoxTheme.BackColor = System.Drawing.Color.Gray;
            this.comboBoxTheme.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTheme.ButtonColorScaling = 0.5F;
            this.comboBoxTheme.DataSource = null;
            this.comboBoxTheme.DisplayMember = null;
            this.comboBoxTheme.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxTheme.DropDownHeight = 150;
            this.comboBoxTheme.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTheme.ItemHeight = 20;
            this.comboBoxTheme.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("comboBoxTheme.Items")));
            this.comboBoxTheme.Location = new System.Drawing.Point(7, 19);
            this.comboBoxTheme.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxTheme.Name = "comboBoxTheme";
            this.comboBoxTheme.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxTheme.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxTheme.ScrollBarWidth = 16;
            this.comboBoxTheme.SelectedIndex = -1;
            this.comboBoxTheme.SelectedItem = null;
            this.comboBoxTheme.Size = new System.Drawing.Size(267, 21);
            this.comboBoxTheme.TabIndex = 0;
            this.comboBoxTheme.ValueMember = null;
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
            this.groupBox2.Location = new System.Drawing.Point(440, 280);
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
            this.groupBox3.Controls.Add(this.checkBox_Distances);
            this.groupBox3.Controls.Add(this.checkBoxOrderRowsInverted);
            this.groupBox3.Controls.Add(this.checkBoxEDSMLog);
            this.groupBox3.Controls.Add(this.checkboxSkipSlowUpdates);
            this.groupBox3.FillClientAreaWithAlternateColor = false;
            this.groupBox3.Location = new System.Drawing.Point(3, 280);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(426, 149);
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
            this.checkBoxFocusNewSystem.Location = new System.Drawing.Point(17, 87);
            this.checkBoxFocusNewSystem.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxFocusNewSystem.Name = "checkBoxFocusNewSystem";
            this.checkBoxFocusNewSystem.Size = new System.Drawing.Size(132, 17);
            this.checkBoxFocusNewSystem.TabIndex = 3;
            this.checkBoxFocusNewSystem.Text = "Focus on New System";
            this.checkBoxFocusNewSystem.TickBoxReductionSize = 10;
            this.checkBoxFocusNewSystem.UseVisualStyleBackColor = true;
            this.checkBoxFocusNewSystem.CheckedChanged += new System.EventHandler(this.checkBoxFocusNewSystem_CheckedChanged);
            // 
            // checkBox_Distances
            // 
            this.checkBox_Distances.AutoSize = true;
            this.checkBox_Distances.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBox_Distances.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBox_Distances.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBox_Distances.FontNerfReduction = 0.5F;
            this.checkBox_Distances.Location = new System.Drawing.Point(17, 18);
            this.checkBox_Distances.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBox_Distances.Name = "checkBox_Distances";
            this.checkBox_Distances.Size = new System.Drawing.Size(219, 17);
            this.checkBox_Distances.TabIndex = 0;
            this.checkBox_Distances.Text = "Get Distance Pairs from EDSM (Optional)";
            this.checkBox_Distances.TickBoxReductionSize = 10;
            this.checkBox_Distances.UseVisualStyleBackColor = true;
            this.checkBox_Distances.CheckedChanged += new System.EventHandler(this.checkBox_Distances_CheckedChanged);
            // 
            // checkBoxOrderRowsInverted
            // 
            this.checkBoxOrderRowsInverted.AutoSize = true;
            this.checkBoxOrderRowsInverted.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxOrderRowsInverted.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxOrderRowsInverted.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxOrderRowsInverted.FontNerfReduction = 0.5F;
            this.checkBoxOrderRowsInverted.Location = new System.Drawing.Point(17, 64);
            this.checkBoxOrderRowsInverted.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxOrderRowsInverted.Name = "checkBoxOrderRowsInverted";
            this.checkBoxOrderRowsInverted.Size = new System.Drawing.Size(171, 17);
            this.checkBoxOrderRowsInverted.TabIndex = 2;
            this.checkBoxOrderRowsInverted.Text = "Order Rows By Visited Number";
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
            this.checkBoxEDSMLog.Location = new System.Drawing.Point(17, 41);
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
            this.checkboxSkipSlowUpdates.Location = new System.Drawing.Point(17, 110);
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
            this.groupBox4.Controls.Add(this.buttonAddCommander);
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.dataGridViewCommanders);
            this.groupBox4.FillClientAreaWithAlternateColor = false;
            this.groupBox4.Location = new System.Drawing.Point(0, 93);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(819, 184);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Commanders";
            this.groupBox4.TextPadding = 0;
            this.groupBox4.TextStartPosition = -1;
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(309, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Get Api key from https://www.edsm.net in  \"My account\" menu.";
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
            this.ColumnEDSMAPIKey,
            this.ColumnNetLogPath});
            this.dataGridViewCommanders.Location = new System.Drawing.Point(11, 45);
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
            this.ColumnNr.HeaderText = "Nr";
            this.ColumnNr.MinimumWidth = 50;
            this.ColumnNr.Name = "ColumnNr";
            this.ColumnNr.ReadOnly = true;
            this.ColumnNr.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnNr.Width = 50;
            // 
            // ColumnCommander
            // 
            this.ColumnCommander.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnCommander.DataPropertyName = "Name";
            this.ColumnCommander.HeaderText = "Commander";
            this.ColumnCommander.MinimumWidth = 150;
            this.ColumnCommander.Name = "ColumnCommander";
            // 
            // ColumnEDSMAPIKey
            // 
            this.ColumnEDSMAPIKey.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnEDSMAPIKey.DataPropertyName = "APIKey";
            this.ColumnEDSMAPIKey.HeaderText = "EDSM api key";
            this.ColumnEDSMAPIKey.MinimumWidth = 150;
            this.ColumnEDSMAPIKey.Name = "ColumnEDSMAPIKey";
            // 
            // ColumnNetLogPath
            // 
            this.ColumnNetLogPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnNetLogPath.DataPropertyName = "NetLogPath";
            this.ColumnNetLogPath.HeaderText = "NetLog path";
            this.ColumnNetLogPath.MinimumWidth = 150;
            this.ColumnNetLogPath.Name = "ColumnNetLogPath";
            // 
            // groupBox1
            // 
            this.groupBox1.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox1.BackColorScaling = 0.5F;
            this.groupBox1.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox1.BorderColorScaling = 0.5F;
            this.groupBox1.Controls.Add(this.button_Browse);
            this.groupBox1.Controls.Add(this.textBoxNetLogDir);
            this.groupBox1.Controls.Add(this.radioButton_Manual);
            this.groupBox1.Controls.Add(this.radioButton_Auto);
            this.groupBox1.FillClientAreaWithAlternateColor = false;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(819, 87);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Elite Dangerous netlog location (Set to manual to use folder below or commander s" +
    "pecific netlog path)";
            this.groupBox1.TextPadding = 0;
            this.groupBox1.TextStartPosition = -1;
            // 
            // button_Browse
            // 
            this.button_Browse.BorderColorScaling = 1.25F;
            this.button_Browse.ButtonColorScaling = 0.5F;
            this.button_Browse.ButtonDisabledScaling = 0.5F;
            this.button_Browse.Location = new System.Drawing.Point(728, 46);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(75, 23);
            this.button_Browse.TabIndex = 3;
            this.button_Browse.Text = "Browse";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // textBoxNetLogDir
            // 
            this.textBoxNetLogDir.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxNetLogDir.BorderColorScaling = 0.5F;
            this.textBoxNetLogDir.Location = new System.Drawing.Point(97, 48);
            this.textBoxNetLogDir.Name = "textBoxNetLogDir";
            this.textBoxNetLogDir.Size = new System.Drawing.Size(623, 20);
            this.textBoxNetLogDir.TabIndex = 2;
            this.textBoxNetLogDir.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxNetLogDir_Validating);
            this.textBoxNetLogDir.Validated += new System.EventHandler(this.textBoxNetLogDir_Validated);
            // 
            // radioButton_Manual
            // 
            this.radioButton_Manual.AutoSize = true;
            this.radioButton_Manual.FontNerfReduction = 0.5F;
            this.radioButton_Manual.Location = new System.Drawing.Point(17, 49);
            this.radioButton_Manual.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButton_Manual.Name = "radioButton_Manual";
            this.radioButton_Manual.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButton_Manual.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButton_Manual.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButton_Manual.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButton_Manual.Size = new System.Drawing.Size(60, 17);
            this.radioButton_Manual.TabIndex = 1;
            this.radioButton_Manual.TabStop = true;
            this.radioButton_Manual.Text = "Manual";
            this.radioButton_Manual.UseVisualStyleBackColor = true;
            // 
            // radioButton_Auto
            // 
            this.radioButton_Auto.AutoSize = true;
            this.radioButton_Auto.FontNerfReduction = 0.5F;
            this.radioButton_Auto.Location = new System.Drawing.Point(17, 26);
            this.radioButton_Auto.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButton_Auto.Name = "radioButton_Auto";
            this.radioButton_Auto.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButton_Auto.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButton_Auto.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButton_Auto.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButton_Auto.Size = new System.Drawing.Size(47, 17);
            this.radioButton_Auto.TabIndex = 0;
            this.radioButton_Auto.TabStop = true;
            this.radioButton_Auto.Text = "Auto";
            this.radioButton_Auto.UseVisualStyleBackColor = true;
            this.radioButton_Auto.CheckedChanged += new System.EventHandler(this.radioButton_Auto_CheckedChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxTheme);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(937, 725);
            this.groupBoxTheme.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.GroupBoxCustom groupBox1;
        private ExtendedControls.ButtonExt button_Browse;
        private ExtendedControls.TextBoxBorder textBoxNetLogDir;
        private ExtendedControls.RadioButtonCustom radioButton_Manual;
        private ExtendedControls.RadioButtonCustom radioButton_Auto;
        private ExtendedControls.GroupBoxCustom groupBox4;
        private ExtendedControls.ButtonExt buttonAddCommander;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dataGridViewCommanders;
        private ExtendedControls.GroupBoxCustom groupBox3;
        private ExtendedControls.CheckBoxCustom checkBox_Distances;
        private ExtendedControls.CheckBoxCustom checkBoxEDSMLog;
        public ExtendedControls.CheckBoxCustom checkboxSkipSlowUpdates;
        private ExtendedControls.GroupBoxCustom groupBox2;
        private ExtendedControls.TextBoxBorder textBoxDefaultZoom;
        private System.Windows.Forms.Label label5;
        private ExtendedControls.RadioButtonCustom radioButtonHistorySelection;
        private ExtendedControls.RadioButtonCustom radioButtonCentreHome;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public ExtendedControls.TextBoxBorder textBoxHomeSystem;
        private ExtendedControls.ComboBoxCustom comboBoxTheme;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panel_defaultmapcolor;
        private ExtendedControls.ButtonExt buttonSaveTheme;
        private System.Windows.Forms.Label label17;
        private ExtendedControls.ButtonExt button_edittheme;
        private ExtendedControls.GroupBoxCustom groupBoxTheme;
        private ExtendedControls.CheckBoxCustom checkBoxOrderRowsInverted;
        private ExtendedControls.CheckBoxCustom checkBoxFocusNewSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNr;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommander;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEDSMAPIKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNetLogPath;
    }
}
