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
namespace EDDiscovery
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
            this.checkBoxKeepOnTop = new ExtendedControls.CheckBoxCustom();
            this.comboBoxTheme = new ExtendedControls.ComboBoxCustom();
            this.button_edittheme = new ExtendedControls.ButtonExt();
            this.buttonSaveTheme = new ExtendedControls.ButtonExt();
            this.panel_defaultmapcolor = new ExtendedControls.PanelNoTheme();
            this.checkBoxMinimizeToNotifyIcon = new ExtendedControls.CheckBoxCustom();
            this.checkBoxUseNotifyIcon = new ExtendedControls.CheckBoxCustom();
            this.checkBoxUTC = new ExtendedControls.CheckBoxCustom();
            this.checkBoxOrderRowsInverted = new ExtendedControls.CheckBoxCustom();
            this.checkBoxEDSMLog = new ExtendedControls.CheckBoxCustom();
            this.dataGridViewCommanders = new System.Windows.Forms.DataGridView();
            this.ColumnCommander = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EdsmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JournalDirCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotesCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteCommander = new ExtendedControls.ButtonExt();
            this.buttonAddCommander = new ExtendedControls.ButtonExt();
            this.textBoxDefaultZoom = new ExtendedControls.TextBoxBorder();
            this.textBoxHomeSystem = new ExtendedControls.AutoCompleteTextBox();
            this.buttonReloadSaved = new ExtendedControls.ButtonExt();
            this.buttonSaveSetup = new ExtendedControls.ButtonExt();
            this.checkBoxAutoSave = new ExtendedControls.CheckBoxCustom();
            this.checkBoxAutoLoad = new ExtendedControls.CheckBoxCustom();
            this.radioButtonHistorySelection = new ExtendedControls.RadioButtonCustom();
            this.radioButtonCentreHome = new ExtendedControls.RadioButtonCustom();
            this.buttonEditCommander = new ExtendedControls.ButtonExt();
            this.groupBoxPopOuts = new ExtendedControls.GroupBoxCustom();
            this.groupBoxTheme = new ExtendedControls.GroupBoxCustom();
            this.groupBox2 = new ExtendedControls.GroupBoxCustom();
            this.label17 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new ExtendedControls.GroupBoxCustom();
            this.groupBox4 = new ExtendedControls.GroupBoxCustom();
            this.dataViewScrollerPanel1 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.checkBoxShowUIEvents = new ExtendedControls.CheckBoxCustom();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).BeginInit();
            this.groupBoxPopOuts.SuspendLayout();
            this.groupBoxTheme.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.dataViewScrollerPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // checkBoxKeepOnTop
            // 
            this.checkBoxKeepOnTop.AutoSize = true;
            this.checkBoxKeepOnTop.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxKeepOnTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxKeepOnTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxKeepOnTop.FontNerfReduction = 0.5F;
            this.checkBoxKeepOnTop.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxKeepOnTop.Location = new System.Drawing.Point(7, 63);
            this.checkBoxKeepOnTop.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxKeepOnTop.Name = "checkBoxKeepOnTop";
            this.checkBoxKeepOnTop.Size = new System.Drawing.Size(88, 17);
            this.checkBoxKeepOnTop.TabIndex = 5;
            this.checkBoxKeepOnTop.Text = "Keep on Top";
            this.checkBoxKeepOnTop.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxKeepOnTop, "This window, and its children, top");
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
            this.comboBoxTheme.DropDownHeight = 150;
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
            this.toolTip.SetToolTip(this.comboBoxTheme, "Select the theme to use");
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
            this.toolTip.SetToolTip(this.button_edittheme, "Edit theme and change colours fonts");
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
            this.toolTip.SetToolTip(this.buttonSaveTheme, "Save theme to disk");
            this.buttonSaveTheme.UseVisualStyleBackColor = true;
            this.buttonSaveTheme.Click += new System.EventHandler(this.buttonSaveTheme_Click);
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
            this.toolTip.SetToolTip(this.panel_defaultmapcolor, "New travel entries get this colour on the map");
            this.panel_defaultmapcolor.Click += new System.EventHandler(this.panel_defaultmapcolor_Click);
            // 
            // checkBoxMinimizeToNotifyIcon
            // 
            this.checkBoxMinimizeToNotifyIcon.AutoSize = true;
            this.checkBoxMinimizeToNotifyIcon.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxMinimizeToNotifyIcon.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMinimizeToNotifyIcon.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMinimizeToNotifyIcon.FontNerfReduction = 0.5F;
            this.checkBoxMinimizeToNotifyIcon.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxMinimizeToNotifyIcon.Location = new System.Drawing.Point(17, 115);
            this.checkBoxMinimizeToNotifyIcon.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMinimizeToNotifyIcon.Name = "checkBoxMinimizeToNotifyIcon";
            this.checkBoxMinimizeToNotifyIcon.Size = new System.Drawing.Size(179, 17);
            this.checkBoxMinimizeToNotifyIcon.TabIndex = 6;
            this.checkBoxMinimizeToNotifyIcon.Text = "Minimize to notification area icon";
            this.checkBoxMinimizeToNotifyIcon.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMinimizeToNotifyIcon, "Minimize the main window to the system notification area (system tray) icon.");
            this.checkBoxMinimizeToNotifyIcon.UseVisualStyleBackColor = true;
            this.checkBoxMinimizeToNotifyIcon.CheckedChanged += new System.EventHandler(this.checkBoxMinimizeToNotifyIcon_CheckedChanged);
            // 
            // checkBoxUseNotifyIcon
            // 
            this.checkBoxUseNotifyIcon.AutoSize = true;
            this.checkBoxUseNotifyIcon.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxUseNotifyIcon.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxUseNotifyIcon.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxUseNotifyIcon.FontNerfReduction = 0.5F;
            this.checkBoxUseNotifyIcon.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxUseNotifyIcon.Location = new System.Drawing.Point(17, 92);
            this.checkBoxUseNotifyIcon.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxUseNotifyIcon.Name = "checkBoxUseNotifyIcon";
            this.checkBoxUseNotifyIcon.Size = new System.Drawing.Size(154, 17);
            this.checkBoxUseNotifyIcon.TabIndex = 5;
            this.checkBoxUseNotifyIcon.Text = "Show notification area icon";
            this.checkBoxUseNotifyIcon.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxUseNotifyIcon, "Show a system notification area (system tray) icon for EDDiscovery.");
            this.checkBoxUseNotifyIcon.UseVisualStyleBackColor = true;
            this.checkBoxUseNotifyIcon.CheckedChanged += new System.EventHandler(this.checkBoxUseNotifyIcon_CheckedChanged);
            // 
            // checkBoxUTC
            // 
            this.checkBoxUTC.AutoSize = true;
            this.checkBoxUTC.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxUTC.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxUTC.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxUTC.FontNerfReduction = 0.5F;
            this.checkBoxUTC.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxUTC.Location = new System.Drawing.Point(17, 69);
            this.checkBoxUTC.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxUTC.Name = "checkBoxUTC";
            this.checkBoxUTC.Size = new System.Drawing.Size(209, 17);
            this.checkBoxUTC.TabIndex = 0;
            this.checkBoxUTC.Text = "Display Game time instead of local time";
            this.checkBoxUTC.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxUTC, "Display game time (UTC) instead of your local time");
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
            this.checkBoxOrderRowsInverted.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxOrderRowsInverted.Location = new System.Drawing.Point(17, 46);
            this.checkBoxOrderRowsInverted.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxOrderRowsInverted.Name = "checkBoxOrderRowsInverted";
            this.checkBoxOrderRowsInverted.Size = new System.Drawing.Size(196, 17);
            this.checkBoxOrderRowsInverted.TabIndex = 2;
            this.checkBoxOrderRowsInverted.Text = "Number Rows Lastest Entry Highest";
            this.checkBoxOrderRowsInverted.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxOrderRowsInverted, "Number oldest entry 1, latest entry highest");
            this.checkBoxOrderRowsInverted.UseVisualStyleBackColor = true;
            this.checkBoxOrderRowsInverted.CheckedChanged += new System.EventHandler(this.checkBoxOrderRowsInverted_CheckedChanged);
            // 
            // checkBoxEDSMLog
            // 
            this.checkBoxEDSMLog.AutoSize = true;
            this.checkBoxEDSMLog.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxEDSMLog.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEDSMLog.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEDSMLog.FontNerfReduction = 0.5F;
            this.checkBoxEDSMLog.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxEDSMLog.Location = new System.Drawing.Point(17, 23);
            this.checkBoxEDSMLog.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEDSMLog.Name = "checkBoxEDSMLog";
            this.checkBoxEDSMLog.Size = new System.Drawing.Size(119, 17);
            this.checkBoxEDSMLog.TabIndex = 1;
            this.checkBoxEDSMLog.Text = "Log HTTP requests";
            this.checkBoxEDSMLog.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxEDSMLog, "Store EDSM queries in a log file");
            this.checkBoxEDSMLog.UseVisualStyleBackColor = true;
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
            this.ColumnCommander,
            this.EdsmName,
            this.JournalDirCol,
            this.NotesCol});
            this.dataGridViewCommanders.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCommanders.MultiSelect = false;
            this.dataGridViewCommanders.Name = "dataGridViewCommanders";
            this.dataGridViewCommanders.ReadOnly = true;
            this.dataGridViewCommanders.RowHeadersVisible = false;
            this.dataGridViewCommanders.RowHeadersWidth = 20;
            this.dataGridViewCommanders.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCommanders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCommanders.Size = new System.Drawing.Size(671, 219);
            this.dataGridViewCommanders.TabIndex = 2;
            this.toolTip.SetToolTip(this.dataGridViewCommanders, "Configure commanders");
            // 
            // ColumnCommander
            // 
            this.ColumnCommander.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnCommander.DataPropertyName = "Name";
            this.ColumnCommander.FillWeight = 120F;
            this.ColumnCommander.HeaderText = "Commander";
            this.ColumnCommander.MinimumWidth = 50;
            this.ColumnCommander.Name = "ColumnCommander";
            this.ColumnCommander.ReadOnly = true;
            // 
            // EdsmName
            // 
            this.EdsmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EdsmName.DataPropertyName = "EdsmName";
            this.EdsmName.HeaderText = "EDSM Name";
            this.EdsmName.MinimumWidth = 50;
            this.EdsmName.Name = "EdsmName";
            this.EdsmName.ReadOnly = true;
            // 
            // JournalDirCol
            // 
            this.JournalDirCol.DataPropertyName = "JournalDir";
            this.JournalDirCol.FillWeight = 120F;
            this.JournalDirCol.HeaderText = "Journal Dir";
            this.JournalDirCol.MinimumWidth = 50;
            this.JournalDirCol.Name = "JournalDirCol";
            this.JournalDirCol.ReadOnly = true;
            // 
            // NotesCol
            // 
            this.NotesCol.DataPropertyName = "Info";
            this.NotesCol.FillWeight = 180F;
            this.NotesCol.HeaderText = "Notes";
            this.NotesCol.MinimumWidth = 50;
            this.NotesCol.Name = "NotesCol";
            this.NotesCol.ReadOnly = true;
            // 
            // btnDeleteCommander
            // 
            this.btnDeleteCommander.BorderColorScaling = 1.25F;
            this.btnDeleteCommander.ButtonColorScaling = 0.5F;
            this.btnDeleteCommander.ButtonDisabledScaling = 0.5F;
            this.btnDeleteCommander.Location = new System.Drawing.Point(713, 104);
            this.btnDeleteCommander.Name = "btnDeleteCommander";
            this.btnDeleteCommander.Size = new System.Drawing.Size(71, 23);
            this.btnDeleteCommander.TabIndex = 3;
            this.btnDeleteCommander.Text = "Delete";
            this.toolTip.SetToolTip(this.btnDeleteCommander, "Delete selected commander");
            this.btnDeleteCommander.UseVisualStyleBackColor = true;
            this.btnDeleteCommander.Click += new System.EventHandler(this.btnDeleteCommander_Click);
            // 
            // buttonAddCommander
            // 
            this.buttonAddCommander.BorderColorScaling = 1.25F;
            this.buttonAddCommander.ButtonColorScaling = 0.5F;
            this.buttonAddCommander.ButtonDisabledScaling = 0.5F;
            this.buttonAddCommander.Location = new System.Drawing.Point(713, 19);
            this.buttonAddCommander.Name = "buttonAddCommander";
            this.buttonAddCommander.Size = new System.Drawing.Size(71, 23);
            this.buttonAddCommander.TabIndex = 0;
            this.buttonAddCommander.Text = "Add";
            this.toolTip.SetToolTip(this.buttonAddCommander, "Add a new commander");
            this.buttonAddCommander.UseVisualStyleBackColor = true;
            this.buttonAddCommander.Click += new System.EventHandler(this.buttonAddCommander_Click);
            // 
            // textBoxDefaultZoom
            // 
            this.textBoxDefaultZoom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxDefaultZoom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxDefaultZoom.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxDefaultZoom.BorderColorScaling = 0.5F;
            this.textBoxDefaultZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDefaultZoom.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxDefaultZoom.Location = new System.Drawing.Point(120, 70);
            this.textBoxDefaultZoom.Multiline = false;
            this.textBoxDefaultZoom.Name = "textBoxDefaultZoom";
            this.textBoxDefaultZoom.ReadOnly = false;
            this.textBoxDefaultZoom.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxDefaultZoom.SelectionLength = 0;
            this.textBoxDefaultZoom.SelectionStart = 0;
            this.textBoxDefaultZoom.Size = new System.Drawing.Size(51, 20);
            this.textBoxDefaultZoom.TabIndex = 6;
            this.textBoxDefaultZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxDefaultZoom, "Set the zoom level of the map. 1 is normal");
            this.textBoxDefaultZoom.WordWrap = true;
            this.textBoxDefaultZoom.Validating += new System.ComponentModel.CancelEventHandler(this.textBoxDefaultZoom_Validating);
            // 
            // textBoxHomeSystem
            // 
            this.textBoxHomeSystem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxHomeSystem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxHomeSystem.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxHomeSystem.BorderColorScaling = 0.5F;
            this.textBoxHomeSystem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxHomeSystem.ControlBackground = System.Drawing.SystemColors.Control;
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
            this.textBoxHomeSystem.Multiline = false;
            this.textBoxHomeSystem.Name = "textBoxHomeSystem";
            this.textBoxHomeSystem.ReadOnly = false;
            this.textBoxHomeSystem.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxHomeSystem.SelectionLength = 0;
            this.textBoxHomeSystem.SelectionStart = 0;
            this.textBoxHomeSystem.Size = new System.Drawing.Size(221, 20);
            this.textBoxHomeSystem.TabIndex = 0;
            this.textBoxHomeSystem.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxHomeSystem, "Pick a home system");
            this.textBoxHomeSystem.WordWrap = true;
            this.textBoxHomeSystem.Validated += new System.EventHandler(this.textBoxHomeSystem_Validated);
            // 
            // buttonReloadSaved
            // 
            this.buttonReloadSaved.BorderColorScaling = 1.25F;
            this.buttonReloadSaved.ButtonColorScaling = 0.5F;
            this.buttonReloadSaved.ButtonDisabledScaling = 0.5F;
            this.buttonReloadSaved.Location = new System.Drawing.Point(134, 16);
            this.buttonReloadSaved.Name = "buttonReloadSaved";
            this.buttonReloadSaved.Size = new System.Drawing.Size(127, 23);
            this.buttonReloadSaved.TabIndex = 3;
            this.buttonReloadSaved.Text = "Open Saved Setup";
            this.toolTip.SetToolTip(this.buttonReloadSaved, "Open now the saved setup of pop outs");
            this.buttonReloadSaved.UseVisualStyleBackColor = true;
            this.buttonReloadSaved.Click += new System.EventHandler(this.buttonReloadSaved_Click);
            // 
            // buttonSaveSetup
            // 
            this.buttonSaveSetup.BorderColorScaling = 1.25F;
            this.buttonSaveSetup.ButtonColorScaling = 0.5F;
            this.buttonSaveSetup.ButtonDisabledScaling = 0.5F;
            this.buttonSaveSetup.Location = new System.Drawing.Point(134, 45);
            this.buttonSaveSetup.Name = "buttonSaveSetup";
            this.buttonSaveSetup.Size = new System.Drawing.Size(127, 23);
            this.buttonSaveSetup.TabIndex = 2;
            this.buttonSaveSetup.Text = "Save Current Setup";
            this.toolTip.SetToolTip(this.buttonSaveSetup, "Save now the current pop out state");
            this.buttonSaveSetup.UseVisualStyleBackColor = true;
            this.buttonSaveSetup.Click += new System.EventHandler(this.buttonSaveSetup_Click);
            // 
            // checkBoxAutoSave
            // 
            this.checkBoxAutoSave.AutoSize = true;
            this.checkBoxAutoSave.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxAutoSave.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxAutoSave.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxAutoSave.FontNerfReduction = 0.5F;
            this.checkBoxAutoSave.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxAutoSave.Location = new System.Drawing.Point(7, 46);
            this.checkBoxAutoSave.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxAutoSave.Name = "checkBoxAutoSave";
            this.checkBoxAutoSave.Size = new System.Drawing.Size(86, 17);
            this.checkBoxAutoSave.TabIndex = 1;
            this.checkBoxAutoSave.Text = "Save on Exit";
            this.checkBoxAutoSave.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxAutoSave, "Save pop out state on exit");
            this.checkBoxAutoSave.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoLoad
            // 
            this.checkBoxAutoLoad.AutoSize = true;
            this.checkBoxAutoLoad.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxAutoLoad.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxAutoLoad.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxAutoLoad.FontNerfReduction = 0.5F;
            this.checkBoxAutoLoad.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxAutoLoad.Location = new System.Drawing.Point(7, 20);
            this.checkBoxAutoLoad.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxAutoLoad.Name = "checkBoxAutoLoad";
            this.checkBoxAutoLoad.Size = new System.Drawing.Size(87, 17);
            this.checkBoxAutoLoad.TabIndex = 0;
            this.checkBoxAutoLoad.Text = "Load at Start";
            this.checkBoxAutoLoad.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxAutoLoad, "Auto show pop outs");
            this.checkBoxAutoLoad.UseVisualStyleBackColor = true;
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
            this.radioButtonHistorySelection.Size = new System.Drawing.Size(126, 17);
            this.radioButtonHistorySelection.TabIndex = 4;
            this.radioButtonHistorySelection.TabStop = true;
            this.radioButtonHistorySelection.Text = "History Grid Selection";
            this.toolTip.SetToolTip(this.radioButtonHistorySelection, "Select history entry as opening location");
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
            this.toolTip.SetToolTip(this.radioButtonCentreHome, "Select home system as opening location");
            this.radioButtonCentreHome.UseVisualStyleBackColor = true;
            // 
            // buttonEditCommander
            // 
            this.buttonEditCommander.BorderColorScaling = 1.25F;
            this.buttonEditCommander.ButtonColorScaling = 0.5F;
            this.buttonEditCommander.ButtonDisabledScaling = 0.5F;
            this.buttonEditCommander.Location = new System.Drawing.Point(713, 60);
            this.buttonEditCommander.Name = "buttonEditCommander";
            this.buttonEditCommander.Size = new System.Drawing.Size(71, 23);
            this.buttonEditCommander.TabIndex = 5;
            this.buttonEditCommander.Text = "Edit";
            this.toolTip.SetToolTip(this.buttonEditCommander, "Edit selected commander");
            this.buttonEditCommander.UseVisualStyleBackColor = true;
            this.buttonEditCommander.Click += new System.EventHandler(this.buttonEditCommander_Click);
            // 
            // groupBoxPopOuts
            // 
            this.groupBoxPopOuts.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxPopOuts.BackColorScaling = 0.5F;
            this.groupBoxPopOuts.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxPopOuts.BorderColorScaling = 0.5F;
            this.groupBoxPopOuts.Controls.Add(this.buttonReloadSaved);
            this.groupBoxPopOuts.Controls.Add(this.buttonSaveSetup);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxAutoSave);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxAutoLoad);
            this.groupBoxPopOuts.FillClientAreaWithAlternateColor = false;
            this.groupBoxPopOuts.Location = new System.Drawing.Point(440, 361);
            this.groupBoxPopOuts.Name = "groupBoxPopOuts";
            this.groupBoxPopOuts.Size = new System.Drawing.Size(277, 79);
            this.groupBoxPopOuts.TabIndex = 19;
            this.groupBoxPopOuts.TabStop = false;
            this.groupBoxPopOuts.Text = "Pop Out Window Options";
            this.groupBoxPopOuts.TextPadding = 0;
            this.groupBoxPopOuts.TextStartPosition = -1;
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
            this.groupBoxTheme.Location = new System.Drawing.Point(3, 450);
            this.groupBoxTheme.Name = "groupBoxTheme";
            this.groupBoxTheme.Size = new System.Drawing.Size(426, 108);
            this.groupBoxTheme.TabIndex = 18;
            this.groupBoxTheme.TabStop = false;
            this.groupBoxTheme.Text = "Theme";
            this.groupBoxTheme.TextPadding = 0;
            this.groupBoxTheme.TextStartPosition = -1;
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
            this.groupBox2.Location = new System.Drawing.Point(440, 254);
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Default Zoom";
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
            // groupBox3
            // 
            this.groupBox3.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox3.BackColorScaling = 0.5F;
            this.groupBox3.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox3.BorderColorScaling = 0.5F;
            this.groupBox3.Controls.Add(this.checkBoxShowUIEvents);
            this.groupBox3.Controls.Add(this.checkBoxMinimizeToNotifyIcon);
            this.groupBox3.Controls.Add(this.checkBoxUseNotifyIcon);
            this.groupBox3.Controls.Add(this.checkBoxUTC);
            this.groupBox3.Controls.Add(this.checkBoxOrderRowsInverted);
            this.groupBox3.Controls.Add(this.checkBoxEDSMLog);
            this.groupBox3.FillClientAreaWithAlternateColor = false;
            this.groupBox3.Location = new System.Drawing.Point(3, 254);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(426, 186);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Options";
            this.groupBox3.TextPadding = 0;
            this.groupBox3.TextStartPosition = -1;
            // 
            // groupBox4
            // 
            this.groupBox4.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox4.BackColorScaling = 0.5F;
            this.groupBox4.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox4.BorderColorScaling = 0.5F;
            this.groupBox4.Controls.Add(this.buttonEditCommander);
            this.groupBox4.Controls.Add(this.dataViewScrollerPanel1);
            this.groupBox4.Controls.Add(this.btnDeleteCommander);
            this.groupBox4.Controls.Add(this.buttonAddCommander);
            this.groupBox4.FillClientAreaWithAlternateColor = false;
            this.groupBox4.Location = new System.Drawing.Point(0, 4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(819, 244);
            this.groupBox4.TabIndex = 15;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Commanders";
            this.groupBox4.TextPadding = 0;
            this.groupBox4.TextStartPosition = -1;
            // 
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewCommanders);
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(10, 19);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(691, 219);
            this.dataViewScrollerPanel1.TabIndex = 4;
            this.dataViewScrollerPanel1.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = false;
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(671, 21);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 198);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 3;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // checkBoxShowUIEvents
            // 
            this.checkBoxShowUIEvents.AutoSize = true;
            this.checkBoxShowUIEvents.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxShowUIEvents.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxShowUIEvents.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxShowUIEvents.FontNerfReduction = 0.5F;
            this.checkBoxShowUIEvents.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxShowUIEvents.Location = new System.Drawing.Point(17, 138);
            this.checkBoxShowUIEvents.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxShowUIEvents.Name = "checkBoxShowUIEvents";
            this.checkBoxShowUIEvents.Size = new System.Drawing.Size(170, 17);
            this.checkBoxShowUIEvents.TabIndex = 6;
            this.checkBoxShowUIEvents.Text = "Show Elite UI Events in history";
            this.checkBoxShowUIEvents.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxShowUIEvents, "Show the UI events (such as Music) in the history. The quantity of them can be ve" +
        "ry large");
            this.checkBoxShowUIEvents.UseVisualStyleBackColor = true;
            this.checkBoxShowUIEvents.CheckedChanged += new System.EventHandler(this.checkBoxShowUIEvents_CheckedChanged);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPopOuts);
            this.Controls.Add(this.groupBoxTheme);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox4);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(937, 725);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).EndInit();
            this.groupBoxPopOuts.ResumeLayout(false);
            this.groupBoxPopOuts.PerformLayout();
            this.groupBoxTheme.ResumeLayout(false);
            this.groupBoxTheme.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.dataViewScrollerPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.GroupBoxCustom groupBox4;
        private ExtendedControls.ButtonExt buttonAddCommander;
        private System.Windows.Forms.DataGridView dataGridViewCommanders;
        private ExtendedControls.GroupBoxCustom groupBox3;
        private ExtendedControls.CheckBoxCustom checkBoxEDSMLog;
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
        private ExtendedControls.PanelNoTheme panel_defaultmapcolor;
        private ExtendedControls.ButtonExt buttonSaveTheme;
        private System.Windows.Forms.Label label17;
        private ExtendedControls.ButtonExt button_edittheme;
        private ExtendedControls.GroupBoxCustom groupBoxTheme;
        private ExtendedControls.CheckBoxCustom checkBoxOrderRowsInverted;
        private ExtendedControls.CheckBoxCustom checkBoxKeepOnTop;
        private ExtendedControls.ButtonExt btnDeleteCommander;
        private ExtendedControls.CheckBoxCustom checkBoxUTC;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel1;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private ExtendedControls.GroupBoxCustom groupBoxPopOuts;
        private ExtendedControls.ButtonExt buttonReloadSaved;
        private ExtendedControls.ButtonExt buttonSaveSetup;
        private ExtendedControls.CheckBoxCustom checkBoxAutoSave;
        private ExtendedControls.CheckBoxCustom checkBoxAutoLoad;
        private ExtendedControls.CheckBoxCustom checkBoxMinimizeToNotifyIcon;
        internal ExtendedControls.CheckBoxCustom checkBoxUseNotifyIcon;
        private ExtendedControls.ButtonExt buttonEditCommander;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommander;
        private System.Windows.Forms.DataGridViewTextBoxColumn EdsmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn JournalDirCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotesCol;
        private ExtendedControls.CheckBoxCustom checkBoxShowUIEvents;
    }
}
