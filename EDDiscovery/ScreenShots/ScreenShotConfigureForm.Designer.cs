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
            this.buttonExtCancel = new ExtendedControls.ButtonExt();
            this.buttonExtOK = new ExtendedControls.ButtonExt();
            this.groupBoxCropSettings = new ExtendedControls.GroupBoxCustom();
            this.numericUpDownHeight = new ExtendedControls.NumericUpDownCustom();
            this.numericUpDownWidth = new ExtendedControls.NumericUpDownCustom();
            this.numericUpDownLeft = new ExtendedControls.NumericUpDownCustom();
            this.numericUpDownTop = new ExtendedControls.NumericUpDownCustom();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxCropImage = new ExtendedControls.CheckBoxCustom();
            this.textBoxScreenshotsDir = new ExtendedControls.TextBoxBorder();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFileNameExample = new ExtendedControls.TextBoxBorder();
            this.buttonChangeScreenshotsFolder = new ExtendedControls.ButtonExt();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxOutputDir = new ExtendedControls.TextBoxBorder();
            this.comboBoxSubFolder = new ExtendedControls.ComboBoxCustom();
            this.comboBoxFileNameFormat = new ExtendedControls.ComboBoxCustom();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonEDChangeOutputFolder = new ExtendedControls.ButtonExt();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxScanFor = new ExtendedControls.ComboBoxCustom();
            this.comboBoxOutputAs = new ExtendedControls.ComboBoxCustom();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
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
            this.panelConfigure.Controls.Add(this.label1);
            this.panelConfigure.Controls.Add(this.textBoxFileNameExample);
            this.panelConfigure.Controls.Add(this.buttonChangeScreenshotsFolder);
            this.panelConfigure.Controls.Add(this.label11);
            this.panelConfigure.Controls.Add(this.label4);
            this.panelConfigure.Controls.Add(this.textBoxOutputDir);
            this.panelConfigure.Controls.Add(this.comboBoxSubFolder);
            this.panelConfigure.Controls.Add(this.comboBoxFileNameFormat);
            this.panelConfigure.Controls.Add(this.label2);
            this.panelConfigure.Controls.Add(this.buttonEDChangeOutputFolder);
            this.panelConfigure.Controls.Add(this.label9);
            this.panelConfigure.Controls.Add(this.label3);
            this.panelConfigure.Controls.Add(this.comboBoxScanFor);
            this.panelConfigure.Controls.Add(this.comboBoxOutputAs);
            this.panelConfigure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConfigure.Location = new System.Drawing.Point(0, 26);
            this.panelConfigure.Name = "panelConfigure";
            this.panelConfigure.Size = new System.Drawing.Size(822, 338);
            this.panelConfigure.TabIndex = 0;
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.BorderColorScaling = 1.25F;
            this.buttonExtCancel.ButtonColorScaling = 0.5F;
            this.buttonExtCancel.ButtonDisabledScaling = 0.5F;
            this.buttonExtCancel.Location = new System.Drawing.Point(641, 291);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonExtCancel.TabIndex = 32;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            this.buttonExtCancel.Click += new System.EventHandler(this.buttonExtCancel_Click);
            // 
            // buttonExtOK
            // 
            this.buttonExtOK.BorderColorScaling = 1.25F;
            this.buttonExtOK.ButtonColorScaling = 0.5F;
            this.buttonExtOK.ButtonDisabledScaling = 0.5F;
            this.buttonExtOK.Location = new System.Drawing.Point(732, 291);
            this.buttonExtOK.Name = "buttonExtOK";
            this.buttonExtOK.Size = new System.Drawing.Size(75, 23);
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
            this.groupBoxCropSettings.Controls.Add(this.label8);
            this.groupBoxCropSettings.Controls.Add(this.label7);
            this.groupBoxCropSettings.Controls.Add(this.label6);
            this.groupBoxCropSettings.Controls.Add(this.label5);
            this.groupBoxCropSettings.FillClientAreaWithAlternateColor = false;
            this.groupBoxCropSettings.Location = new System.Drawing.Point(215, 145);
            this.groupBoxCropSettings.Name = "groupBoxCropSettings";
            this.groupBoxCropSettings.Size = new System.Drawing.Size(126, 143);
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
            this.numericUpDownHeight.Location = new System.Drawing.Point(60, 110);
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
            this.numericUpDownWidth.Location = new System.Drawing.Point(60, 80);
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
            this.numericUpDownLeft.Location = new System.Drawing.Point(60, 50);
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
            this.numericUpDownTop.Location = new System.Drawing.Point(60, 20);
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 112);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Height";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Width";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 52);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Left";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Top";
            // 
            // checkBoxCropImage
            // 
            this.checkBoxCropImage.AutoSize = true;
            this.checkBoxCropImage.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCropImage.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCropImage.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCropImage.FontNerfReduction = 0.5F;
            this.checkBoxCropImage.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCropImage.Location = new System.Drawing.Point(8, 145);
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
            this.textBoxScreenshotsDir.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxScreenshotsDir.BorderColorScaling = 0.5F;
            this.textBoxScreenshotsDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxScreenshotsDir.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxScreenshotsDir.Location = new System.Drawing.Point(212, 14);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "ED/Steam Screenshot folder";
            // 
            // textBoxFileNameExample
            // 
            this.textBoxFileNameExample.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxFileNameExample.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFileNameExample.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFileNameExample.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFileNameExample.BorderColorScaling = 0.5F;
            this.textBoxFileNameExample.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFileNameExample.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFileNameExample.Location = new System.Drawing.Point(510, 100);
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
            this.buttonChangeScreenshotsFolder.BorderColorScaling = 1.25F;
            this.buttonChangeScreenshotsFolder.ButtonColorScaling = 0.5F;
            this.buttonChangeScreenshotsFolder.ButtonDisabledScaling = 0.5F;
            this.buttonChangeScreenshotsFolder.Location = new System.Drawing.Point(510, 12);
            this.buttonChangeScreenshotsFolder.Name = "buttonChangeScreenshotsFolder";
            this.buttonChangeScreenshotsFolder.Size = new System.Drawing.Size(57, 23);
            this.buttonChangeScreenshotsFolder.TabIndex = 16;
            this.buttonChangeScreenshotsFolder.Text = "Browse";
            this.buttonChangeScreenshotsFolder.UseVisualStyleBackColor = true;
            this.buttonChangeScreenshotsFolder.Click += new System.EventHandler(this.buttonChangeEDScreenshot_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(5, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 26;
            this.label11.Text = "In Sub folder";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 27;
            this.label4.Text = "In Filename format";
            // 
            // textBoxOutputDir
            // 
            this.textBoxOutputDir.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxOutputDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxOutputDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxOutputDir.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxOutputDir.BorderColorScaling = 0.5F;
            this.textBoxOutputDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxOutputDir.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxOutputDir.Location = new System.Drawing.Point(212, 40);
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
            this.comboBoxSubFolder.DisplayMember = "";
            this.comboBoxSubFolder.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxSubFolder.DropDownHeight = 400;
            this.comboBoxSubFolder.DropDownWidth = 280;
            this.comboBoxSubFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSubFolder.ItemHeight = 13;
            this.comboBoxSubFolder.Location = new System.Drawing.Point(213, 68);
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
            this.comboBoxSubFolder.ValueMember = "";
            // 
            // comboBoxFileNameFormat
            // 
            this.comboBoxFileNameFormat.ArrowWidth = 1;
            this.comboBoxFileNameFormat.BorderColor = System.Drawing.Color.Red;
            this.comboBoxFileNameFormat.ButtonColorScaling = 0.5F;
            this.comboBoxFileNameFormat.DataSource = null;
            this.comboBoxFileNameFormat.DisplayMember = "";
            this.comboBoxFileNameFormat.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxFileNameFormat.DropDownHeight = 400;
            this.comboBoxFileNameFormat.DropDownWidth = 350;
            this.comboBoxFileNameFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxFileNameFormat.ItemHeight = 13;
            this.comboBoxFileNameFormat.Location = new System.Drawing.Point(213, 100);
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
            this.comboBoxFileNameFormat.ValueMember = "";
            this.comboBoxFileNameFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxFileNameFormat_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Store Converted pictures";
            // 
            // buttonEDChangeOutputFolder
            // 
            this.buttonEDChangeOutputFolder.BorderColorScaling = 1.25F;
            this.buttonEDChangeOutputFolder.ButtonColorScaling = 0.5F;
            this.buttonEDChangeOutputFolder.ButtonDisabledScaling = 0.5F;
            this.buttonEDChangeOutputFolder.Location = new System.Drawing.Point(510, 38);
            this.buttonEDChangeOutputFolder.Name = "buttonEDChangeOutputFolder";
            this.buttonEDChangeOutputFolder.Size = new System.Drawing.Size(57, 23);
            this.buttonEDChangeOutputFolder.TabIndex = 18;
            this.buttonEDChangeOutputFolder.Text = "Browse";
            this.buttonEDChangeOutputFolder.UseVisualStyleBackColor = true;
            this.buttonEDChangeOutputFolder.Click += new System.EventHandler(this.buttonChangeOutputFolder_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(590, 17);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 24;
            this.label9.Text = "Scan for";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(590, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "Save as";
            // 
            // comboBoxScanFor
            // 
            this.comboBoxScanFor.ArrowWidth = 1;
            this.comboBoxScanFor.BorderColor = System.Drawing.Color.Red;
            this.comboBoxScanFor.ButtonColorScaling = 0.5F;
            this.comboBoxScanFor.DataSource = null;
            this.comboBoxScanFor.DisplayMember = "";
            this.comboBoxScanFor.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxScanFor.DropDownHeight = 200;
            this.comboBoxScanFor.DropDownWidth = 161;
            this.comboBoxScanFor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxScanFor.ItemHeight = 13;
            this.comboBoxScanFor.Location = new System.Drawing.Point(672, 14);
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
            this.comboBoxScanFor.ValueMember = "";
            // 
            // comboBoxOutputAs
            // 
            this.comboBoxOutputAs.ArrowWidth = 1;
            this.comboBoxOutputAs.BorderColor = System.Drawing.Color.Red;
            this.comboBoxOutputAs.ButtonColorScaling = 0.5F;
            this.comboBoxOutputAs.DataSource = null;
            this.comboBoxOutputAs.DisplayMember = "";
            this.comboBoxOutputAs.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxOutputAs.DropDownHeight = 200;
            this.comboBoxOutputAs.DropDownWidth = 161;
            this.comboBoxOutputAs.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxOutputAs.ItemHeight = 13;
            this.comboBoxOutputAs.Location = new System.Drawing.Point(672, 38);
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
            this.panelTop.Size = new System.Drawing.Size(822, 26);
            this.panelTop.TabIndex = 32;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseUp);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Location = new System.Drawing.Point(799, 0);
            this.panel_close.MarginSize = 6;
            this.panel_close.Name = "panel_close";
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_close_MouseClick);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.BackColor = System.Drawing.SystemColors.Control;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(769, 0);
            this.panel_minimize.MarginSize = 6;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_minimize_MouseClick);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 8);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(109, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "Screenshot Configure";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseDown);
            this.label_index.MouseUp += new System.Windows.Forms.MouseEventHandler(this.captionControl_MouseUp);
            // 
            // ScreenShotConfigureForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 364);
            this.Controls.Add(this.panelConfigure);
            this.Controls.Add(this.panelTop);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "ScreenShotConfigureForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ScreenShotConfigureForm";
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
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_index;
        private ExtendedControls.TextBoxBorder textBoxScreenshotsDir;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.TextBoxBorder textBoxFileNameExample;
        private ExtendedControls.ButtonExt buttonChangeScreenshotsFolder;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label4;
        private ExtendedControls.TextBoxBorder textBoxOutputDir;
        private ExtendedControls.ComboBoxCustom comboBoxSubFolder;
        private ExtendedControls.ComboBoxCustom comboBoxFileNameFormat;
        private System.Windows.Forms.Label label2;
        private ExtendedControls.ButtonExt buttonEDChangeOutputFolder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private ExtendedControls.ComboBoxCustom comboBoxScanFor;
        private ExtendedControls.ComboBoxCustom comboBoxOutputAs;
        private ExtendedControls.GroupBoxCustom groupBoxCropSettings;
        private ExtendedControls.NumericUpDownCustom numericUpDownHeight;
        private ExtendedControls.NumericUpDownCustom numericUpDownWidth;
        private ExtendedControls.NumericUpDownCustom numericUpDownLeft;
        private ExtendedControls.NumericUpDownCustom numericUpDownTop;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        protected ExtendedControls.CheckBoxCustom checkBoxCropImage;
        private ExtendedControls.ButtonExt buttonExtCancel;
        private ExtendedControls.ButtonExt buttonExtOK;
    }
}