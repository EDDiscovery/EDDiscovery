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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenShotConfigureForm));
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
            this.panel_close = new ExtendedControls.ExtButtonDrawn();
            this.panel_minimize = new ExtendedControls.ExtButtonDrawn();
            this.label_index = new System.Windows.Forms.Label();
            this.extComboBoxConvert1 = new ExtendedControls.ExtComboBox();
            this.extGroupBox1 = new ExtendedControls.ExtGroupBox();
            this.extNumericUpDown1 = new ExtendedControls.ExtNumericUpDown();
            this.extNumericUpDown2 = new ExtendedControls.ExtNumericUpDown();
            this.extNumericUpDown3 = new ExtendedControls.ExtNumericUpDown();
            this.extNumericUpDown4 = new ExtendedControls.ExtNumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.extComboBoxConvert2 = new ExtendedControls.ExtComboBox();
            this.extCheckBox1 = new ExtendedControls.ExtCheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.panelConfigure.SuspendLayout();
            this.groupBoxCropSettings.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.extGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelConfigure
            // 
            this.panelConfigure.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelConfigure.Controls.Add(this.extCheckBox1);
            this.panelConfigure.Controls.Add(this.extComboBoxConvert2);
            this.panelConfigure.Controls.Add(this.extComboBoxConvert1);
            this.panelConfigure.Controls.Add(this.buttonExtCancel);
            this.panelConfigure.Controls.Add(this.extGroupBox1);
            this.panelConfigure.Controls.Add(this.buttonExtOK);
            this.panelConfigure.Controls.Add(this.groupBoxCropSettings);
            this.panelConfigure.Controls.Add(this.textBoxScreenshotsDir);
            this.panelConfigure.Controls.Add(this.labelFolder);
            this.panelConfigure.Controls.Add(this.textBoxFileNameExample);
            this.panelConfigure.Controls.Add(this.buttonChangeScreenshotsFolder);
            this.panelConfigure.Controls.Add(this.labelSubfolder);
            this.panelConfigure.Controls.Add(this.label5);
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
            this.panelConfigure.Size = new System.Drawing.Size(636, 511);
            this.panelConfigure.TabIndex = 0;
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExtCancel.Location = new System.Drawing.Point(404, 475);
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
            this.buttonExtOK.Location = new System.Drawing.Point(521, 475);
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
            this.groupBoxCropSettings.Location = new System.Drawing.Point(211, 309);
            this.groupBoxCropSettings.Name = "groupBoxCropSettings";
            this.groupBoxCropSettings.Size = new System.Drawing.Size(197, 143);
            this.groupBoxCropSettings.TabIndex = 30;
            this.groupBoxCropSettings.TabStop = false;
            this.groupBoxCropSettings.Text = "Crop/Resize Settings";
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
            this.textBoxScreenshotsDir.EndButtonEnable = true;
            this.textBoxScreenshotsDir.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxScreenshotsDir.EndButtonImage")));
            this.textBoxScreenshotsDir.EndButtonVisible = false;
            this.textBoxScreenshotsDir.InErrorCondition = false;
            this.textBoxScreenshotsDir.Location = new System.Drawing.Point(210, 14);
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
            this.textBoxFileNameExample.EndButtonEnable = true;
            this.textBoxFileNameExample.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxFileNameExample.EndButtonImage")));
            this.textBoxFileNameExample.EndButtonVisible = false;
            this.textBoxFileNameExample.InErrorCondition = false;
            this.textBoxFileNameExample.Location = new System.Drawing.Point(210, 214);
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
            this.buttonChangeScreenshotsFolder.Location = new System.Drawing.Point(508, 12);
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
            this.textBoxOutputDir.EndButtonEnable = true;
            this.textBoxOutputDir.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxOutputDir.EndButtonImage")));
            this.textBoxOutputDir.EndButtonVisible = false;
            this.textBoxOutputDir.InErrorCondition = false;
            this.textBoxOutputDir.Location = new System.Drawing.Point(210, 88);
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
            this.comboBoxSubFolder.BorderColor = System.Drawing.Color.Red;
            this.comboBoxSubFolder.ButtonColorScaling = 0.5F;
            this.comboBoxSubFolder.DataSource = null;
            this.comboBoxSubFolder.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxSubFolder.DisplayMember = "";
            this.comboBoxSubFolder.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxSubFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSubFolder.Location = new System.Drawing.Point(211, 152);
            this.comboBoxSubFolder.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxSubFolder.Name = "comboBoxSubFolder";
            this.comboBoxSubFolder.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxSubFolder.ScrollBarColor = System.Drawing.Color.LightGray;
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
            this.comboBoxFileNameFormat.BorderColor = System.Drawing.Color.Red;
            this.comboBoxFileNameFormat.ButtonColorScaling = 0.5F;
            this.comboBoxFileNameFormat.DataSource = null;
            this.comboBoxFileNameFormat.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxFileNameFormat.DisplayMember = "";
            this.comboBoxFileNameFormat.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxFileNameFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxFileNameFormat.Location = new System.Drawing.Point(211, 187);
            this.comboBoxFileNameFormat.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxFileNameFormat.Name = "comboBoxFileNameFormat";
            this.comboBoxFileNameFormat.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxFileNameFormat.ScrollBarColor = System.Drawing.Color.LightGray;
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
            this.buttonEDChangeOutputFolder.Location = new System.Drawing.Point(508, 86);
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
            this.comboBoxScanFor.BorderColor = System.Drawing.Color.Red;
            this.comboBoxScanFor.ButtonColorScaling = 0.5F;
            this.comboBoxScanFor.DataSource = null;
            this.comboBoxScanFor.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxScanFor.DisplayMember = "";
            this.comboBoxScanFor.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxScanFor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxScanFor.Location = new System.Drawing.Point(210, 39);
            this.comboBoxScanFor.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxScanFor.Name = "comboBoxScanFor";
            this.comboBoxScanFor.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxScanFor.ScrollBarColor = System.Drawing.Color.LightGray;
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
            this.comboBoxOutputAs.BorderColor = System.Drawing.Color.Red;
            this.comboBoxOutputAs.ButtonColorScaling = 0.5F;
            this.comboBoxOutputAs.DataSource = null;
            this.comboBoxOutputAs.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxOutputAs.DisplayMember = "";
            this.comboBoxOutputAs.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxOutputAs.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxOutputAs.Location = new System.Drawing.Point(210, 115);
            this.comboBoxOutputAs.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxOutputAs.Name = "comboBoxOutputAs";
            this.comboBoxOutputAs.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxOutputAs.ScrollBarColor = System.Drawing.Color.LightGray;
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
            this.panelTop.Size = new System.Drawing.Size(636, 26);
            this.panelTop.TabIndex = 32;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseUp);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(613, 0);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.PanelDisabledScaling = 0.25F;
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.TabStop = false;
            this.panel_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_close.UseMnemonic = true;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.AutoEllipsis = false;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.Image = null;
            this.panel_minimize.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(583, 0);
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.PanelDisabledScaling = 0.25F;
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_minimize.UseMnemonic = true;
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
            // extComboBoxConvert1
            // 
            this.extComboBoxConvert1.BorderColor = System.Drawing.Color.White;
            this.extComboBoxConvert1.ButtonColorScaling = 0.5F;
            this.extComboBoxConvert1.DataSource = null;
            this.extComboBoxConvert1.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxConvert1.DisplayMember = "";
            this.extComboBoxConvert1.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.extComboBoxConvert1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxConvert1.Location = new System.Drawing.Point(210, 251);
            this.extComboBoxConvert1.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.extComboBoxConvert1.Name = "extComboBoxConvert1";
            this.extComboBoxConvert1.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.extComboBoxConvert1.ScrollBarColor = System.Drawing.Color.LightGray;
            this.extComboBoxConvert1.SelectedIndex = -1;
            this.extComboBoxConvert1.SelectedItem = null;
            this.extComboBoxConvert1.SelectedValue = null;
            this.extComboBoxConvert1.Size = new System.Drawing.Size(198, 21);
            this.extComboBoxConvert1.TabIndex = 33;
            this.extComboBoxConvert1.Text = "extComboBox1";
            this.extComboBoxConvert1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxConvert1.ValueMember = "";
            // 
            // extGroupBox1
            // 
            this.extGroupBox1.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.extGroupBox1.BackColorScaling = 0.5F;
            this.extGroupBox1.BorderColor = System.Drawing.Color.LightGray;
            this.extGroupBox1.BorderColorScaling = 0.5F;
            this.extGroupBox1.Controls.Add(this.extNumericUpDown1);
            this.extGroupBox1.Controls.Add(this.extNumericUpDown2);
            this.extGroupBox1.Controls.Add(this.extNumericUpDown3);
            this.extGroupBox1.Controls.Add(this.extNumericUpDown4);
            this.extGroupBox1.Controls.Add(this.label1);
            this.extGroupBox1.Controls.Add(this.label2);
            this.extGroupBox1.Controls.Add(this.label3);
            this.extGroupBox1.Controls.Add(this.label4);
            this.extGroupBox1.FillClientAreaWithAlternateColor = false;
            this.extGroupBox1.Location = new System.Drawing.Point(424, 309);
            this.extGroupBox1.Name = "extGroupBox1";
            this.extGroupBox1.Size = new System.Drawing.Size(197, 143);
            this.extGroupBox1.TabIndex = 30;
            this.extGroupBox1.TabStop = false;
            this.extGroupBox1.Text = "Crop/Resize Settings";
            this.extGroupBox1.TextPadding = 0;
            this.extGroupBox1.TextStartPosition = -1;
            // 
            // extNumericUpDown1
            // 
            this.extNumericUpDown1.AutoSizeTextBox = false;
            this.extNumericUpDown1.BorderColor = System.Drawing.Color.Transparent;
            this.extNumericUpDown1.BorderColorScaling = 0.5F;
            this.extNumericUpDown1.Location = new System.Drawing.Point(122, 110);
            this.extNumericUpDown1.Maximum = 10000;
            this.extNumericUpDown1.Minimum = 0;
            this.extNumericUpDown1.Name = "extNumericUpDown1";
            this.extNumericUpDown1.Size = new System.Drawing.Size(57, 20);
            this.extNumericUpDown1.TabIndex = 4;
            this.extNumericUpDown1.Text = "0";
            this.extNumericUpDown1.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.extNumericUpDown1.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.extNumericUpDown1.Value = 0;
            // 
            // extNumericUpDown2
            // 
            this.extNumericUpDown2.AutoSizeTextBox = false;
            this.extNumericUpDown2.BorderColor = System.Drawing.Color.Transparent;
            this.extNumericUpDown2.BorderColorScaling = 0.5F;
            this.extNumericUpDown2.Location = new System.Drawing.Point(122, 80);
            this.extNumericUpDown2.Maximum = 10000;
            this.extNumericUpDown2.Minimum = 0;
            this.extNumericUpDown2.Name = "extNumericUpDown2";
            this.extNumericUpDown2.Size = new System.Drawing.Size(57, 20);
            this.extNumericUpDown2.TabIndex = 3;
            this.extNumericUpDown2.Text = "0";
            this.extNumericUpDown2.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.extNumericUpDown2.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.extNumericUpDown2.Value = 0;
            // 
            // extNumericUpDown3
            // 
            this.extNumericUpDown3.AutoSizeTextBox = false;
            this.extNumericUpDown3.BorderColor = System.Drawing.Color.Transparent;
            this.extNumericUpDown3.BorderColorScaling = 0.5F;
            this.extNumericUpDown3.Location = new System.Drawing.Point(122, 50);
            this.extNumericUpDown3.Maximum = 10000;
            this.extNumericUpDown3.Minimum = 0;
            this.extNumericUpDown3.Name = "extNumericUpDown3";
            this.extNumericUpDown3.Size = new System.Drawing.Size(57, 20);
            this.extNumericUpDown3.TabIndex = 2;
            this.extNumericUpDown3.Text = "0";
            this.extNumericUpDown3.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.extNumericUpDown3.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.extNumericUpDown3.Value = 0;
            // 
            // extNumericUpDown4
            // 
            this.extNumericUpDown4.AutoSizeTextBox = false;
            this.extNumericUpDown4.BorderColor = System.Drawing.Color.Transparent;
            this.extNumericUpDown4.BorderColorScaling = 0.5F;
            this.extNumericUpDown4.Location = new System.Drawing.Point(122, 20);
            this.extNumericUpDown4.Maximum = 10000;
            this.extNumericUpDown4.Minimum = 0;
            this.extNumericUpDown4.Name = "extNumericUpDown4";
            this.extNumericUpDown4.Size = new System.Drawing.Size(57, 20);
            this.extNumericUpDown4.TabIndex = 1;
            this.extNumericUpDown4.Text = "0";
            this.extNumericUpDown4.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.extNumericUpDown4.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.extNumericUpDown4.Value = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Height";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Width";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Left";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(26, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Top";
            // 
            // extComboBoxConvert2
            // 
            this.extComboBoxConvert2.BorderColor = System.Drawing.Color.White;
            this.extComboBoxConvert2.ButtonColorScaling = 0.5F;
            this.extComboBoxConvert2.DataSource = null;
            this.extComboBoxConvert2.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxConvert2.DisplayMember = "";
            this.extComboBoxConvert2.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.extComboBoxConvert2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxConvert2.Location = new System.Drawing.Point(423, 251);
            this.extComboBoxConvert2.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.extComboBoxConvert2.Name = "extComboBoxConvert2";
            this.extComboBoxConvert2.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.extComboBoxConvert2.ScrollBarColor = System.Drawing.Color.LightGray;
            this.extComboBoxConvert2.SelectedIndex = -1;
            this.extComboBoxConvert2.SelectedItem = null;
            this.extComboBoxConvert2.SelectedValue = null;
            this.extComboBoxConvert2.Size = new System.Drawing.Size(198, 21);
            this.extComboBoxConvert2.TabIndex = 33;
            this.extComboBoxConvert2.Text = "extComboBox1";
            this.extComboBoxConvert2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxConvert2.ValueMember = "";
            // 
            // extCheckBox1
            // 
            this.extCheckBox1.AutoSize = true;
            this.extCheckBox1.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBox1.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBox1.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBox1.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBox1.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBox1.ImageIndeterminate = null;
            this.extCheckBox1.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBox1.ImageUnchecked = null;
            this.extCheckBox1.Location = new System.Drawing.Point(210, 281);
            this.extCheckBox1.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBox1.Name = "extCheckBox1";
            this.extCheckBox1.Size = new System.Drawing.Size(89, 17);
            this.extCheckBox1.TabIndex = 34;
            this.extCheckBox1.Text = "Keep Original";
            this.extCheckBox1.TickBoxReductionRatio = 0.75F;
            this.extCheckBox1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 251);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 13);
            this.label5.TabIndex = 27;
            this.label5.Text = "Crop/Resize Options";
            // 
            // ScreenShotConfigureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(636, 537);
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
            this.extGroupBox1.ResumeLayout(false);
            this.extGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelConfigure;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtButtonDrawn panel_close;
        private ExtendedControls.ExtButtonDrawn panel_minimize;
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
        private ExtendedControls.ExtButton buttonExtCancel;
        private ExtendedControls.ExtButton buttonExtOK;
        private ExtendedControls.ExtCheckBox extCheckBox1;
        private ExtendedControls.ExtComboBox extComboBoxConvert2;
        private ExtendedControls.ExtComboBox extComboBoxConvert1;
        private ExtendedControls.ExtGroupBox extGroupBox1;
        private ExtendedControls.ExtNumericUpDown extNumericUpDown1;
        private ExtendedControls.ExtNumericUpDown extNumericUpDown2;
        private ExtendedControls.ExtNumericUpDown extNumericUpDown3;
        private ExtendedControls.ExtNumericUpDown extNumericUpDown4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}