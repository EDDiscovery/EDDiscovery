namespace EDDiscovery.ScreenShots
{
    partial class ScreenShotConfigureForm
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
            this.panelConfigure = new System.Windows.Forms.Panel();
            this.buttonExtCancel = new ExtendedControls.ExtButton();
            this.buttonExtOK = new ExtendedControls.ExtButton();
            this.groupBoxCropSettings = new ExtendedControls.ExtGroupBox();
            this.numericUpDownHeight = new ExtendedControls.ExtNumericUpDown();
            this.numericUpDownWidth = new ExtendedControls.ExtNumericUpDown();
            this.numericUpDownLeft = new ExtendedControls.ExtNumericUpDown();
            this.numericUpDownTop = new ExtendedControls.ExtNumericUpDown();
            this.labelHeight = new System.Windows.Forms.Label();
            this.labelWidth = new System.Windows.Forms.Label();
            this.labelLeft = new System.Windows.Forms.Label();
            this.labelTop = new System.Windows.Forms.Label();
            this.checkBoxCropImage = new ExtendedControls.ExtCheckBox();
            this.textBoxScreenshotsDir = new ExtendedControls.ExtTextBox();
            this.labelFolder = new System.Windows.Forms.Label();
            this.textBoxFileNameExample = new ExtendedControls.ExtTextBox();
            this.buttonChangeScreenshotsFolder = new ExtendedControls.ExtButton();
            this.labelSubfolder = new System.Windows.Forms.Label();
            this.labelFileNameFormat = new System.Windows.Forms.Label();
            this.textBoxOutputDir = new ExtendedControls.ExtTextBox();
            this.comboBoxSubFolder = new ExtendedControls.ExtComboBox();
            this.comboBoxFileNameFormat = new ExtendedControls.ExtComboBox();
            this.labelStoreFolder = new System.Windows.Forms.Label();
            this.buttonEDChangeOutputFolder = new ExtendedControls.ExtButton();
            this.labelScanFor = new System.Windows.Forms.Label();
            this.labelSaveAs = new System.Windows.Forms.Label();
            this.comboBoxScanFor = new ExtendedControls.ExtComboBox();
            this.comboBoxOutputAs = new ExtendedControls.ExtComboBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.ExtPanelDrawn();
            this.panel_minimize = new ExtendedControls.ExtPanelDrawn();
            this.label_index = new System.Windows.Forms.Label();
            this.panelConfigure.SuspendLayout();
            this.groupBoxCropSettings.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelConfigure
            // 
            this.panelConfigure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelConfigure.Controls.Add(this.buttonExtCancel);
            this.panelConfigure.Controls.Add(this.buttonExtOK);
            this.panelConfigure.Controls.Add(this.groupBoxCropSettings);
            this.panelConfigure.Controls.Add(this.checkBoxCropImage);
            this.panelConfigure.Controls.Add(this.textBoxScreenshotsDir);
            this.panelConfigure.Controls.Add(this.labelFolder);
            this.panelConfigure.Controls.Add(this.textBoxFileNameExample);
            this.panelConfigure.Controls.Add(this.buttonChangeScreenshotsFolder);
            this.panelConfigure.Controls.Add(this.labelSubfolder);
            this.panelConfigure.Controls.Add(this.labelFileNameFormat);
            this.panelConfigure.Controls.Add(this.textBoxOutputDir);
            this.panelConfigure.Controls.Add(this.comboBoxSubFolder);
            this.panelConfigure.Controls.Add(this.comboBoxFileNameFormat);
            this.panelConfigure.Controls.Add(this.labelStoreFolder);
            this.panelConfigure.Controls.Add(this.buttonEDChangeOutputFolder);
            this.panelConfigure.Controls.Add(this.labelScanFor);
            this.panelConfigure.Controls.Add(this.labelSaveAs);
            this.panelConfigure.Controls.Add(this.comboBoxScanFor);
            this.panelConfigure.Controls.Add(this.comboBoxOutputAs);
            this.panelConfigure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConfigure.Location = new System.Drawing.Point(0, 26);
            this.panelConfigure.Name = "panelConfigure";
            this.panelConfigure.Size = new System.Drawing.Size(705, 473);
            this.panelConfigure.TabIndex = 0;
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtCancel.Location = new System.Drawing.Point(473, 426);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(100, 23);
            this.buttonExtCancel.TabIndex = 32;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            this.buttonExtCancel.Click += new System.EventHandler(this.buttonExtCancel_Click);
            // 
            // buttonExtOK
            // 
            this.buttonExtOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtOK.Location = new System.Drawing.Point(590, 426);
            this.buttonExtOK.Name = "buttonExtOK";
            this.buttonExtOK.Size = new System.Drawing.Size(100, 23);
            this.buttonExtOK.TabIndex = 31;
            this.buttonExtOK.Text = "OK";
            this.buttonExtOK.UseVisualStyleBackColor = true;
            this.buttonExtOK.Click += new System.EventHandler(this.buttonExtOK_Click);
            // 
            // groupBoxCropSettings
            // 
            this.groupBoxCropSettings.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCropSettings.BackColorScaling = 0.5F;
            this.groupBoxCropSettings.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCropSettings.BorderColorScaling = 0.5F;
            this.groupBoxCropSettings.Controls.Add(this.numericUpDownHeight);
            this.groupBoxCropSettings.Controls.Add(this.numericUpDownWidth);
            this.groupBoxCropSettings.Controls.Add(this.numericUpDownLeft);
            this.groupBoxCropSettings.Controls.Add(this.numericUpDownTop);
            this.groupBoxCropSettings.Controls.Add(this.labelHeight);
            this.groupBoxCropSettings.Controls.Add(this.labelWidth);
            this.groupBoxCropSettings.Controls.Add(this.labelLeft);
            this.groupBoxCropSettings.Controls.Add(this.labelTop);
            this.groupBoxCropSettings.FillClientAreaWithAlternateColor = false;
            this.groupBoxCropSettings.Location = new System.Drawing.Point(250, 251);
            this.groupBoxCropSettings.Name = "groupBoxCropSettings";
            this.groupBoxCropSettings.Size = new System.Drawing.Size(197, 143);
            this.groupBoxCropSettings.TabIndex = 30;
            this.groupBoxCropSettings.TabStop = false;
            this.groupBoxCropSettings.Text = "Crop Settings";
            this.groupBoxCropSettings.TextPadding = 0;
            this.groupBoxCropSettings.TextStartPosition = -1;
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.AutoSizeTextBox = false;
            this.numericUpDownHeight.BorderColor = System.Drawing.Color.Transparent;
            this.numericUpDownHeight.BorderColorScaling = 0.5F;
            this.numericUpDownHeight.Location = new System.Drawing.Point(122, 110);
            this.numericUpDownHeight.Maximum = 10000;
            this.numericUpDownHeight.Minimum = 0;
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownHeight.SpinnerSize = 16;
            this.numericUpDownHeight.TabIndex = 4;
            this.numericUpDownHeight.Text = "0";
            this.numericUpDownHeight.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownHeight.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.numericUpDownHeight.Value = 0;
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.AutoSizeTextBox = false;
            this.numericUpDownWidth.BorderColor = System.Drawing.Color.Transparent;
            this.numericUpDownWidth.BorderColorScaling = 0.5F;
            this.numericUpDownWidth.Location = new System.Drawing.Point(122, 80);
            this.numericUpDownWidth.Maximum = 10000;
            this.numericUpDownWidth.Minimum = 0;
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownWidth.SpinnerSize = 16;
            this.numericUpDownWidth.TabIndex = 3;
            this.numericUpDownWidth.Text = "0";
            this.numericUpDownWidth.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownWidth.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.numericUpDownWidth.Value = 0;
            // 
            // numericUpDownLeft
            // 
            this.numericUpDownLeft.AutoSizeTextBox = false;
            this.numericUpDownLeft.BorderColor = System.Drawing.Color.Transparent;
            this.numericUpDownLeft.BorderColorScaling = 0.5F;
            this.numericUpDownLeft.Location = new System.Drawing.Point(122, 50);
            this.numericUpDownLeft.Maximum = 10000;
            this.numericUpDownLeft.Minimum = 0;
            this.numericUpDownLeft.Name = "numericUpDownLeft";
            this.numericUpDownLeft.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownLeft.SpinnerSize = 16;
            this.numericUpDownLeft.TabIndex = 2;
            this.numericUpDownLeft.Text = "0";
            this.numericUpDownLeft.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownLeft.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.numericUpDownLeft.Value = 0;
            // 
            // numericUpDownTop
            // 
            this.numericUpDownTop.AutoSizeTextBox = false;
            this.numericUpDownTop.BorderColor = System.Drawing.Color.Transparent;
            this.numericUpDownTop.BorderColorScaling = 0.5F;
            this.numericUpDownTop.Location = new System.Drawing.Point(122, 20);
            this.numericUpDownTop.Maximum = 10000;
            this.numericUpDownTop.Minimum = 0;
            this.numericUpDownTop.Name = "numericUpDownTop";
            this.numericUpDownTop.Size = new System.Drawing.Size(57, 20);
            this.numericUpDownTop.SpinnerSize = 16;
            this.numericUpDownTop.TabIndex = 1;
            this.numericUpDownTop.Text = "0";
            this.numericUpDownTop.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownTop.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.numericUpDownTop.Value = 0;
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Location = new System.Drawing.Point(6, 112);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(38, 13);
            this.labelHeight.TabIndex = 7;
            this.labelHeight.Text = "Height";
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Location = new System.Drawing.Point(6, 82);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(35, 13);
            this.labelWidth.TabIndex = 6;
            this.labelWidth.Text = "Width";
            // 
            // labelLeft
            // 
            this.labelLeft.AutoSize = true;
            this.labelLeft.Location = new System.Drawing.Point(6, 52);
            this.labelLeft.Name = "labelLeft";
            this.labelLeft.Size = new System.Drawing.Size(25, 13);
            this.labelLeft.TabIndex = 5;
            this.labelLeft.Text = "Left";
            // 
            // labelTop
            // 
            this.labelTop.AutoSize = true;
            this.labelTop.Location = new System.Drawing.Point(6, 22);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(26, 13);
            this.labelTop.TabIndex = 0;
            this.labelTop.Text = "Top";
            // 
            // checkBoxCropImage
            // 
            this.checkBoxCropImage.AutoSize = true;
            this.checkBoxCropImage.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCropImage.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCropImage.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCropImage.FontNerfReduction = 0.5F;
            this.checkBoxCropImage.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCropImage.Location = new System.Drawing.Point(8, 251);
            this.checkBoxCropImage.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCropImage.Name = "checkBoxCropImage";
            this.checkBoxCropImage.Size = new System.Drawing.Size(80, 17);
            this.checkBoxCropImage.TabIndex = 29;
            this.checkBoxCropImage.Text = "Crop Image";
            this.checkBoxCropImage.TickBoxReductionSize = 10;
            this.checkBoxCropImage.UseVisualStyleBackColor = true;
            this.checkBoxCropImage.CheckedChanged += new System.EventHandler(this.checkBoxCropImage_CheckedChanged);
            // 
            // textBoxScreenshotsDir
            // 
            this.textBoxScreenshotsDir.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxScreenshotsDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxScreenshotsDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxScreenshotsDir.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxScreenshotsDir.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxScreenshotsDir.BorderColorScaling = 0.5F;
            this.textBoxScreenshotsDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxScreenshotsDir.ClearOnFirstChar = false;
            this.textBoxScreenshotsDir.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxScreenshotsDir.InErrorCondition = false;
            this.textBoxScreenshotsDir.Location = new System.Drawing.Point(250, 14);
            this.textBoxScreenshotsDir.Multiline = false;
            this.textBoxScreenshotsDir.Name = "textBoxScreenshotsDir";
            this.textBoxScreenshotsDir.ReadOnly = false;
            this.textBoxScreenshotsDir.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxScreenshotsDir.SelectionLength = 0;
            this.textBoxScreenshotsDir.SelectionStart = 0;
            this.textBoxScreenshotsDir.Size = new System.Drawing.Size(281, 20);
            this.textBoxScreenshotsDir.TabIndex = 14;
            this.textBoxScreenshotsDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxScreenshotsDir.WordWrap = true;
            this.textBoxScreenshotsDir.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxScreenshotsDir_KeyUp);
            this.textBoxScreenshotsDir.Leave += new System.EventHandler(this.textBoxScreenshotsDir_Leave);
            // 
            // labelFolder
            // 
            this.labelFolder.AutoSize = true;
            this.labelFolder.Location = new System.Drawing.Point(5, 17);
            this.labelFolder.Name = "labelFolder";
            this.labelFolder.Size = new System.Drawing.Size(143, 13);
            this.labelFolder.TabIndex = 15;
            this.labelFolder.Text = "ED/Steam Screenshot folder";
            // 
            // textBoxFileNameExample
            // 
            this.textBoxFileNameExample.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxFileNameExample.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFileNameExample.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFileNameExample.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxFileNameExample.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFileNameExample.BorderColorScaling = 0.5F;
            this.textBoxFileNameExample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFileNameExample.ClearOnFirstChar = false;
            this.textBoxFileNameExample.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFileNameExample.InErrorCondition = false;
            this.textBoxFileNameExample.Location = new System.Drawing.Point(250, 214);
            this.textBoxFileNameExample.Multiline = false;
            this.textBoxFileNameExample.Name = "textBoxFileNameExample";
            this.textBoxFileNameExample.ReadOnly = true;
            this.textBoxFileNameExample.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFileNameExample.SelectionLength = 0;
            this.textBoxFileNameExample.SelectionStart = 0;
            this.textBoxFileNameExample.Size = new System.Drawing.Size(299, 20);
            this.textBoxFileNameExample.TabIndex = 28;
            this.textBoxFileNameExample.TabStop = false;
            this.textBoxFileNameExample.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxFileNameExample.WordWrap = true;
            // 
            // buttonChangeScreenshotsFolder
            // 
            this.buttonChangeScreenshotsFolder.Location = new System.Drawing.Point(548, 12);
            this.buttonChangeScreenshotsFolder.Name = "buttonChangeScreenshotsFolder";
            this.buttonChangeScreenshotsFolder.Size = new System.Drawing.Size(100, 23);
            this.buttonChangeScreenshotsFolder.TabIndex = 16;
            this.buttonChangeScreenshotsFolder.Text = "Browse";
            this.buttonChangeScreenshotsFolder.UseVisualStyleBackColor = true;
            this.buttonChangeScreenshotsFolder.Click += new System.EventHandler(this.buttonChangeEDScreenshot_Click);
            // 
            // labelSubfolder
            // 
            this.labelSubfolder.AutoSize = true;
            this.labelSubfolder.Location = new System.Drawing.Point(5, 156);
            this.labelSubfolder.Name = "labelSubfolder";
            this.labelSubfolder.Size = new System.Drawing.Size(67, 13);
            this.labelSubfolder.TabIndex = 26;
            this.labelSubfolder.Text = "In Sub folder";
            // 
            // labelFileNameFormat
            // 
            this.labelFileNameFormat.AutoSize = true;
            this.labelFileNameFormat.Location = new System.Drawing.Point(5, 190);
            this.labelFileNameFormat.Name = "labelFileNameFormat";
            this.labelFileNameFormat.Size = new System.Drawing.Size(93, 13);
            this.labelFileNameFormat.TabIndex = 27;
            this.labelFileNameFormat.Text = "In Filename format";
            // 
            // textBoxOutputDir
            // 
            this.textBoxOutputDir.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxOutputDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxOutputDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxOutputDir.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxOutputDir.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxOutputDir.BorderColorScaling = 0.5F;
            this.textBoxOutputDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxOutputDir.ClearOnFirstChar = false;
            this.textBoxOutputDir.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxOutputDir.InErrorCondition = false;
            this.textBoxOutputDir.Location = new System.Drawing.Point(250, 88);
            this.textBoxOutputDir.Multiline = false;
            this.textBoxOutputDir.Name = "textBoxOutputDir";
            this.textBoxOutputDir.ReadOnly = false;
            this.textBoxOutputDir.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxOutputDir.SelectionLength = 0;
            this.textBoxOutputDir.SelectionStart = 0;
            this.textBoxOutputDir.Size = new System.Drawing.Size(281, 20);
            this.textBoxOutputDir.TabIndex = 17;
            this.textBoxOutputDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxOutputDir.WordWrap = true;
            // 
            // comboBoxSubFolder
            // 
            this.comboBoxSubFolder.ArrowWidth = 1;
            this.comboBoxSubFolder.BorderColor = System.Drawing.Color.Red;
            this.comboBoxSubFolder.ButtonColorScaling = 0.5F;
            this.comboBoxSubFolder.DataSource = null;
            this.comboBoxSubFolder.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxSubFolder.DisplayMember = "";
            this.comboBoxSubFolder.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxSubFolder.DropDownHeight = 400;
            this.comboBoxSubFolder.DropDownWidth = 280;
            this.comboBoxSubFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSubFolder.ItemHeight = 13;
            this.comboBoxSubFolder.Location = new System.Drawing.Point(251, 152);
            this.comboBoxSubFolder.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxSubFolder.Name = "comboBoxSubFolder";
            this.comboBoxSubFolder.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxSubFolder.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxSubFolder.ScrollBarWidth = 16;
            this.comboBoxSubFolder.SelectedIndex = -1;
            this.comboBoxSubFolder.SelectedItem = null;
            this.comboBoxSubFolder.SelectedValue = null;
            this.comboBoxSubFolder.Size = new System.Drawing.Size(280, 21);
            this.comboBoxSubFolder.TabIndex = 22;
            this.comboBoxSubFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxSubFolder.ValueMember = "";
            // 
            // comboBoxFileNameFormat
            // 
            this.comboBoxFileNameFormat.ArrowWidth = 1;
            this.comboBoxFileNameFormat.BorderColor = System.Drawing.Color.Red;
            this.comboBoxFileNameFormat.ButtonColorScaling = 0.5F;
            this.comboBoxFileNameFormat.DataSource = null;
            this.comboBoxFileNameFormat.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxFileNameFormat.DisplayMember = "";
            this.comboBoxFileNameFormat.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxFileNameFormat.DropDownHeight = 400;
            this.comboBoxFileNameFormat.DropDownWidth = 350;
            this.comboBoxFileNameFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxFileNameFormat.ItemHeight = 13;
            this.comboBoxFileNameFormat.Location = new System.Drawing.Point(251, 187);
            this.comboBoxFileNameFormat.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxFileNameFormat.Name = "comboBoxFileNameFormat";
            this.comboBoxFileNameFormat.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxFileNameFormat.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxFileNameFormat.ScrollBarWidth = 16;
            this.comboBoxFileNameFormat.SelectedIndex = -1;
            this.comboBoxFileNameFormat.SelectedItem = null;
            this.comboBoxFileNameFormat.SelectedValue = null;
            this.comboBoxFileNameFormat.Size = new System.Drawing.Size(280, 21);
            this.comboBoxFileNameFormat.TabIndex = 23;
            this.comboBoxFileNameFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxFileNameFormat.ValueMember = "";
            this.comboBoxFileNameFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxFileNameFormat_SelectedIndexChanged);
            // 
            // labelStoreFolder
            // 
            this.labelStoreFolder.AutoSize = true;
            this.labelStoreFolder.Location = new System.Drawing.Point(5, 91);
            this.labelStoreFolder.Name = "labelStoreFolder";
            this.labelStoreFolder.Size = new System.Drawing.Size(124, 13);
            this.labelStoreFolder.TabIndex = 19;
            this.labelStoreFolder.Text = "Store Converted pictures";
            // 
            // buttonEDChangeOutputFolder
            // 
            this.buttonEDChangeOutputFolder.Location = new System.Drawing.Point(548, 86);
            this.buttonEDChangeOutputFolder.Name = "buttonEDChangeOutputFolder";
            this.buttonEDChangeOutputFolder.Size = new System.Drawing.Size(100, 23);
            this.buttonEDChangeOutputFolder.TabIndex = 18;
            this.buttonEDChangeOutputFolder.Text = "Browse";
            this.buttonEDChangeOutputFolder.UseVisualStyleBackColor = true;
            this.buttonEDChangeOutputFolder.Click += new System.EventHandler(this.buttonChangeOutputFolder_Click);
            // 
            // labelScanFor
            // 
            this.labelScanFor.AutoSize = true;
            this.labelScanFor.Location = new System.Drawing.Point(5, 47);
            this.labelScanFor.Name = "labelScanFor";
            this.labelScanFor.Size = new System.Drawing.Size(47, 13);
            this.labelScanFor.TabIndex = 24;
            this.labelScanFor.Text = "Scan for";
            // 
            // labelSaveAs
            // 
            this.labelSaveAs.AutoSize = true;
            this.labelSaveAs.Location = new System.Drawing.Point(5, 118);
            this.labelSaveAs.Name = "labelSaveAs";
            this.labelSaveAs.Size = new System.Drawing.Size(46, 13);
            this.labelSaveAs.TabIndex = 25;
            this.labelSaveAs.Text = "Save as";
            // 
            // comboBoxScanFor
            // 
            this.comboBoxScanFor.ArrowWidth = 1;
            this.comboBoxScanFor.BorderColor = System.Drawing.Color.Red;
            this.comboBoxScanFor.ButtonColorScaling = 0.5F;
            this.comboBoxScanFor.DataSource = null;
            this.comboBoxScanFor.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxScanFor.DisplayMember = "";
            this.comboBoxScanFor.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxScanFor.DropDownHeight = 200;
            this.comboBoxScanFor.DropDownWidth = 161;
            this.comboBoxScanFor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxScanFor.ItemHeight = 13;
            this.comboBoxScanFor.Location = new System.Drawing.Point(250, 39);
            this.comboBoxScanFor.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxScanFor.Name = "comboBoxScanFor";
            this.comboBoxScanFor.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxScanFor.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxScanFor.ScrollBarWidth = 16;
            this.comboBoxScanFor.SelectedIndex = -1;
            this.comboBoxScanFor.SelectedItem = null;
            this.comboBoxScanFor.SelectedValue = null;
            this.comboBoxScanFor.Size = new System.Drawing.Size(135, 21);
            this.comboBoxScanFor.TabIndex = 20;
            this.comboBoxScanFor.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxScanFor.ValueMember = "";
            // 
            // comboBoxOutputAs
            // 
            this.comboBoxOutputAs.ArrowWidth = 1;
            this.comboBoxOutputAs.BorderColor = System.Drawing.Color.Red;
            this.comboBoxOutputAs.ButtonColorScaling = 0.5F;
            this.comboBoxOutputAs.DataSource = null;
            this.comboBoxOutputAs.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxOutputAs.DisplayMember = "";
            this.comboBoxOutputAs.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxOutputAs.DropDownHeight = 200;
            this.comboBoxOutputAs.DropDownWidth = 161;
            this.comboBoxOutputAs.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxOutputAs.ItemHeight = 13;
            this.comboBoxOutputAs.Location = new System.Drawing.Point(250, 115);
            this.comboBoxOutputAs.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxOutputAs.Name = "comboBoxOutputAs";
            this.comboBoxOutputAs.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxOutputAs.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxOutputAs.ScrollBarWidth = 16;
            this.comboBoxOutputAs.SelectedIndex = -1;
            this.comboBoxOutputAs.SelectedItem = null;
            this.comboBoxOutputAs.SelectedValue = null;
            this.comboBoxOutputAs.Size = new System.Drawing.Size(135, 21);
            this.comboBoxOutputAs.TabIndex = 21;
            this.comboBoxOutputAs.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxOutputAs.ValueMember = "";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(705, 26);
            this.panelTop.TabIndex = 32;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseUp);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(682, 0);
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.TabStop = false;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(652, 0);
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 8);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(43, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "<code>";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseDown);
            this.label_index.MouseUp += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseUp);
            // 
            // ScreenShotConfigureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 499);
            this.Controls.Add(this.panelConfigure);
            this.Controls.Add(this.panelTop);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "ScreenShotConfigureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Screen Shot Configure";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ScreenShotConfigureForm_FormClosed);
            this.panelConfigure.ResumeLayout(false);
            this.panelConfigure.PerformLayout();
            this.groupBoxCropSettings.ResumeLayout(false);
            this.groupBoxCropSettings.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelConfigure;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtPanelDrawn panel_close;
        private ExtendedControls.ExtPanelDrawn panel_minimize;
        private System.Windows.Forms.Label label_index;
        private ExtendedControls.ExtTextBox textBoxScreenshotsDir;
        private System.Windows.Forms.Label labelFolder;
        private ExtendedControls.ExtTextBox textBoxFileNameExample;
        private ExtendedControls.ExtButton buttonChangeScreenshotsFolder;
        private System.Windows.Forms.Label labelSubfolder;
        private System.Windows.Forms.Label labelFileNameFormat;
        private ExtendedControls.ExtTextBox textBoxOutputDir;
        private ExtendedControls.ExtComboBox comboBoxSubFolder;
        private ExtendedControls.ExtComboBox comboBoxFileNameFormat;
        private System.Windows.Forms.Label labelStoreFolder;
        private ExtendedControls.ExtButton buttonEDChangeOutputFolder;
        private System.Windows.Forms.Label labelScanFor;
        private System.Windows.Forms.Label labelSaveAs;
        private ExtendedControls.ExtComboBox comboBoxScanFor;
        private ExtendedControls.ExtComboBox comboBoxOutputAs;
        private ExtendedControls.ExtGroupBox groupBoxCropSettings;
        private ExtendedControls.ExtNumericUpDown numericUpDownHeight;
        private ExtendedControls.ExtNumericUpDown numericUpDownWidth;
        private ExtendedControls.ExtNumericUpDown numericUpDownLeft;
        private ExtendedControls.ExtNumericUpDown numericUpDownTop;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.Label labelLeft;
        private System.Windows.Forms.Label labelTop;
        protected ExtendedControls.ExtCheckBox checkBoxCropImage;
        private ExtendedControls.ExtButton buttonExtCancel;
        private ExtendedControls.ExtButton buttonExtOK;
    }
}