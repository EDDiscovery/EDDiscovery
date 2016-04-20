namespace EDDiscovery2.ImageHandler
{
    partial class ImageHandler
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageHandler));
            this.groupBox_Preview = new ExtendedControls.GroupBoxCustom();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox_Prevcontrols = new ExtendedControls.GroupBoxCustom();
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
            this.checkBoxPreview = new ExtendedControls.CheckBoxCustom();
            this.groupBox_Controls = new ExtendedControls.GroupBoxCustom();
            this.checkBoxAutoConvert = new ExtendedControls.CheckBoxCustom();
            this.textBoxScreenshotsDir = new ExtendedControls.TextBoxBorder();
            this.checkBoxHires = new ExtendedControls.CheckBoxCustom();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFileNameExample = new ExtendedControls.TextBoxBorder();
            this.buttonChnageEDScreenshot = new ExtendedControls.ButtonExt();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxOutputDir = new ExtendedControls.TextBoxBorder();
            this.comboBoxFileNameFormat = new ExtendedControls.ComboBoxCustom();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxRemove = new ExtendedControls.CheckBoxCustom();
            this.buttonImageStore = new ExtendedControls.ButtonExt();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxFormat = new ExtendedControls.ComboBoxCustom();
            this.groupBox_Preview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox_Prevcontrols.SuspendLayout();
            this.groupBoxCropSettings.SuspendLayout();
            this.groupBox_Controls.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_Preview
            // 
            this.groupBox_Preview.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox_Preview.BackColorScaling = 0.5F;
            this.groupBox_Preview.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox_Preview.BorderColorScaling = 0.5F;
            this.groupBox_Preview.Controls.Add(this.pictureBox1);
            this.groupBox_Preview.Controls.Add(this.groupBox_Prevcontrols);
            this.groupBox_Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Preview.FillClientAreaWithAlternateColor = false;
            this.groupBox_Preview.Location = new System.Drawing.Point(0, 150);
            this.groupBox_Preview.Name = "groupBox_Preview";
            this.groupBox_Preview.Size = new System.Drawing.Size(898, 512);
            this.groupBox_Preview.TabIndex = 15;
            this.groupBox_Preview.TabStop = false;
            this.groupBox_Preview.TextPadding = 0;
            this.groupBox_Preview.TextStartPosition = -1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(149, 16);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(746, 493);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox_Prevcontrols
            // 
            this.groupBox_Prevcontrols.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox_Prevcontrols.BackColorScaling = 0.5F;
            this.groupBox_Prevcontrols.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox_Prevcontrols.BorderColorScaling = 0.5F;
            this.groupBox_Prevcontrols.Controls.Add(this.groupBoxCropSettings);
            this.groupBox_Prevcontrols.Controls.Add(this.checkBoxCropImage);
            this.groupBox_Prevcontrols.Controls.Add(this.checkBoxPreview);
            this.groupBox_Prevcontrols.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_Prevcontrols.FillClientAreaWithAlternateColor = false;
            this.groupBox_Prevcontrols.Location = new System.Drawing.Point(3, 16);
            this.groupBox_Prevcontrols.Name = "groupBox_Prevcontrols";
            this.groupBox_Prevcontrols.Size = new System.Drawing.Size(146, 493);
            this.groupBox_Prevcontrols.TabIndex = 11;
            this.groupBox_Prevcontrols.TabStop = false;
            this.groupBox_Prevcontrols.TextPadding = 0;
            this.groupBox_Prevcontrols.TextStartPosition = -1;
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
            this.groupBoxCropSettings.Location = new System.Drawing.Point(6, 65);
            this.groupBoxCropSettings.Name = "groupBoxCropSettings";
            this.groupBoxCropSettings.Size = new System.Drawing.Size(126, 143);
            this.groupBoxCropSettings.TabIndex = 9;
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
            this.numericUpDownHeight.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownHeight.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.numericUpDownHeight.Value = 0;
            this.numericUpDownHeight.Leave += new System.EventHandler(this.numericUpDownHeight_Leave);
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
            this.numericUpDownWidth.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownWidth.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.numericUpDownWidth.Value = 0;
            this.numericUpDownWidth.Leave += new System.EventHandler(this.numericUpDownWidth_Leave);
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
            this.numericUpDownLeft.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownLeft.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.numericUpDownLeft.Value = 0;
            this.numericUpDownLeft.Leave += new System.EventHandler(this.numericUpDownLeft_Leave);
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
            this.numericUpDownTop.TextBoxBackColor = System.Drawing.SystemColors.Window;
            this.numericUpDownTop.TextBoxForeColor = System.Drawing.SystemColors.WindowText;
            this.numericUpDownTop.Value = 0;
            this.numericUpDownTop.Leave += new System.EventHandler(this.numericUpDownTop_Leave);
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
            this.checkBoxCropImage.Location = new System.Drawing.Point(8, 19);
            this.checkBoxCropImage.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCropImage.Name = "checkBoxCropImage";
            this.checkBoxCropImage.Size = new System.Drawing.Size(80, 17);
            this.checkBoxCropImage.TabIndex = 8;
            this.checkBoxCropImage.Text = "Crop Image";
            this.checkBoxCropImage.TickBoxReductionSize = 10;
            this.checkBoxCropImage.UseVisualStyleBackColor = true;
            this.checkBoxCropImage.CheckedChanged += new System.EventHandler(this.checkBoxCropImage_CheckedChanged);
            // 
            // checkBoxPreview
            // 
            this.checkBoxPreview.AutoSize = true;
            this.checkBoxPreview.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxPreview.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxPreview.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxPreview.FontNerfReduction = 0.5F;
            this.checkBoxPreview.Location = new System.Drawing.Point(8, 42);
            this.checkBoxPreview.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxPreview.Name = "checkBoxPreview";
            this.checkBoxPreview.Size = new System.Drawing.Size(64, 17);
            this.checkBoxPreview.TabIndex = 10;
            this.checkBoxPreview.Text = "Preview";
            this.checkBoxPreview.TickBoxReductionSize = 10;
            this.checkBoxPreview.UseVisualStyleBackColor = true;
            this.checkBoxPreview.CheckedChanged += new System.EventHandler(this.checkBoxPreview_CheckedChanged);
            // 
            // groupBox_Controls
            // 
            this.groupBox_Controls.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox_Controls.BackColorScaling = 0.5F;
            this.groupBox_Controls.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox_Controls.BorderColorScaling = 0.5F;
            this.groupBox_Controls.Controls.Add(this.checkBoxAutoConvert);
            this.groupBox_Controls.Controls.Add(this.textBoxScreenshotsDir);
            this.groupBox_Controls.Controls.Add(this.checkBoxHires);
            this.groupBox_Controls.Controls.Add(this.label1);
            this.groupBox_Controls.Controls.Add(this.textBoxFileNameExample);
            this.groupBox_Controls.Controls.Add(this.buttonChnageEDScreenshot);
            this.groupBox_Controls.Controls.Add(this.label4);
            this.groupBox_Controls.Controls.Add(this.textBoxOutputDir);
            this.groupBox_Controls.Controls.Add(this.comboBoxFileNameFormat);
            this.groupBox_Controls.Controls.Add(this.label2);
            this.groupBox_Controls.Controls.Add(this.checkBoxRemove);
            this.groupBox_Controls.Controls.Add(this.buttonImageStore);
            this.groupBox_Controls.Controls.Add(this.label3);
            this.groupBox_Controls.Controls.Add(this.comboBoxFormat);
            this.groupBox_Controls.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Controls.FillClientAreaWithAlternateColor = false;
            this.groupBox_Controls.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Controls.Name = "groupBox_Controls";
            this.groupBox_Controls.Size = new System.Drawing.Size(898, 150);
            this.groupBox_Controls.TabIndex = 16;
            this.groupBox_Controls.TabStop = false;
            this.groupBox_Controls.TextPadding = 0;
            this.groupBox_Controls.TextStartPosition = -1;
            // 
            // checkBoxAutoConvert
            // 
            this.checkBoxAutoConvert.AutoSize = true;
            this.checkBoxAutoConvert.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxAutoConvert.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxAutoConvert.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxAutoConvert.FontNerfReduction = 0.5F;
            this.checkBoxAutoConvert.Location = new System.Drawing.Point(18, 19);
            this.checkBoxAutoConvert.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxAutoConvert.Name = "checkBoxAutoConvert";
            this.checkBoxAutoConvert.Size = new System.Drawing.Size(165, 17);
            this.checkBoxAutoConvert.TabIndex = 0;
            this.checkBoxAutoConvert.Text = "Auto convert ED screenshots";
            this.checkBoxAutoConvert.TickBoxReductionSize = 10;
            this.checkBoxAutoConvert.UseVisualStyleBackColor = true;
            this.checkBoxAutoConvert.CheckedChanged += new System.EventHandler(this.checkBoxAutoConvert_CheckedChanged);
            // 
            // textBoxScreenshotsDir
            // 
            this.textBoxScreenshotsDir.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxScreenshotsDir.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxScreenshotsDir.BorderColorScaling = 0.5F;
            this.textBoxScreenshotsDir.Location = new System.Drawing.Point(184, 42);
            this.textBoxScreenshotsDir.Name = "textBoxScreenshotsDir";
            this.textBoxScreenshotsDir.Size = new System.Drawing.Size(311, 20);
            this.textBoxScreenshotsDir.TabIndex = 1;
            this.textBoxScreenshotsDir.Leave += new System.EventHandler(this.textBoxScreenshotsDir_Leave);
            // 
            // checkBoxHires
            // 
            this.checkBoxHires.AutoSize = true;
            this.checkBoxHires.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxHires.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxHires.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxHires.FontNerfReduction = 0.5F;
            this.checkBoxHires.Location = new System.Drawing.Point(257, 94);
            this.checkBoxHires.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxHires.Name = "checkBoxHires";
            this.checkBoxHires.Size = new System.Drawing.Size(152, 17);
            this.checkBoxHires.TabIndex = 14;
            this.checkBoxHires.Text = "Mark High Resolution Files";
            this.checkBoxHires.TickBoxReductionSize = 10;
            this.checkBoxHires.UseVisualStyleBackColor = true;
            this.checkBoxHires.CheckedChanged += new System.EventHandler(this.checkBox_hires_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ED Screenshot directory";
            // 
            // textBoxFileNameExample
            // 
            this.textBoxFileNameExample.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxFileNameExample.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFileNameExample.BorderColorScaling = 0.5F;
            this.textBoxFileNameExample.Location = new System.Drawing.Point(448, 117);
            this.textBoxFileNameExample.Name = "textBoxFileNameExample";
            this.textBoxFileNameExample.ReadOnly = true;
            this.textBoxFileNameExample.Size = new System.Drawing.Size(184, 20);
            this.textBoxFileNameExample.TabIndex = 13;
            this.textBoxFileNameExample.TabStop = false;
            // 
            // buttonChnageEDScreenshot
            // 
            this.buttonChnageEDScreenshot.BorderColorScaling = 1.25F;
            this.buttonChnageEDScreenshot.ButtonColorScaling = 0.5F;
            this.buttonChnageEDScreenshot.ButtonDisabledScaling = 0.5F;
            this.buttonChnageEDScreenshot.Location = new System.Drawing.Point(512, 40);
            this.buttonChnageEDScreenshot.Name = "buttonChnageEDScreenshot";
            this.buttonChnageEDScreenshot.Size = new System.Drawing.Size(57, 23);
            this.buttonChnageEDScreenshot.TabIndex = 2;
            this.buttonChnageEDScreenshot.Text = "Browse";
            this.buttonChnageEDScreenshot.UseVisualStyleBackColor = true;
            this.buttonChnageEDScreenshot.Click += new System.EventHandler(this.buttonChnageEDScreenshot_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 120);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Filename format";
            // 
            // textBoxOutputDir
            // 
            this.textBoxOutputDir.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxOutputDir.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxOutputDir.BorderColorScaling = 0.5F;
            this.textBoxOutputDir.Location = new System.Drawing.Point(184, 68);
            this.textBoxOutputDir.Name = "textBoxOutputDir";
            this.textBoxOutputDir.Size = new System.Drawing.Size(311, 20);
            this.textBoxOutputDir.TabIndex = 3;
            this.textBoxOutputDir.Leave += new System.EventHandler(this.textBoxOutputDir_Leave);
            // 
            // comboBoxFileNameFormat
            // 
            this.comboBoxFileNameFormat.ArrowWidth = 1;
            this.comboBoxFileNameFormat.BorderColor = System.Drawing.Color.Red;
            this.comboBoxFileNameFormat.ButtonColorScaling = 0.5F;
            this.comboBoxFileNameFormat.DataSource = null;
            this.comboBoxFileNameFormat.DisplayMember = null;
            this.comboBoxFileNameFormat.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxFileNameFormat.DropDownHeight = 200;
            this.comboBoxFileNameFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxFileNameFormat.ItemHeight = 20;
            this.comboBoxFileNameFormat.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("comboBoxFileNameFormat.Items")));
            this.comboBoxFileNameFormat.Location = new System.Drawing.Point(183, 117);
            this.comboBoxFileNameFormat.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxFileNameFormat.Name = "comboBoxFileNameFormat";
            this.comboBoxFileNameFormat.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxFileNameFormat.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxFileNameFormat.ScrollBarWidth = 16;
            this.comboBoxFileNameFormat.SelectedIndex = -1;
            this.comboBoxFileNameFormat.SelectedItem = null;
            this.comboBoxFileNameFormat.Size = new System.Drawing.Size(218, 21);
            this.comboBoxFileNameFormat.TabIndex = 7;
            this.comboBoxFileNameFormat.ValueMember = null;
            this.comboBoxFileNameFormat.SelectedIndexChanged += new ExtendedControls.ComboBoxCustom.OnSelectedIndexChanged(this.comboBoxFileNameFormat_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Converted pictures";
            // 
            // checkBoxRemove
            // 
            this.checkBoxRemove.AutoSize = true;
            this.checkBoxRemove.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxRemove.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxRemove.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxRemove.FontNerfReduction = 0.5F;
            this.checkBoxRemove.Location = new System.Drawing.Point(18, 94);
            this.checkBoxRemove.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxRemove.Name = "checkBoxRemove";
            this.checkBoxRemove.Size = new System.Drawing.Size(212, 17);
            this.checkBoxRemove.TabIndex = 6;
            this.checkBoxRemove.Text = "Remove original image after conversion";
            this.checkBoxRemove.TickBoxReductionSize = 10;
            this.checkBoxRemove.UseVisualStyleBackColor = true;
            this.checkBoxRemove.CheckedChanged += new System.EventHandler(this.checkBoxRemove_CheckedChanged);
            // 
            // buttonImageStore
            // 
            this.buttonImageStore.BorderColorScaling = 1.25F;
            this.buttonImageStore.ButtonColorScaling = 0.5F;
            this.buttonImageStore.ButtonDisabledScaling = 0.5F;
            this.buttonImageStore.Location = new System.Drawing.Point(512, 66);
            this.buttonImageStore.Name = "buttonImageStore";
            this.buttonImageStore.Size = new System.Drawing.Size(57, 23);
            this.buttonImageStore.TabIndex = 4;
            this.buttonImageStore.Text = "Browse";
            this.buttonImageStore.UseVisualStyleBackColor = true;
            this.buttonImageStore.Click += new System.EventHandler(this.buttonImageStore_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(591, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Save as";
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.ArrowWidth = 1;
            this.comboBoxFormat.BorderColor = System.Drawing.Color.Red;
            this.comboBoxFormat.ButtonColorScaling = 0.5F;
            this.comboBoxFormat.DataSource = null;
            this.comboBoxFormat.DisplayMember = null;
            this.comboBoxFormat.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxFormat.DropDownHeight = 200;
            this.comboBoxFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxFormat.ItemHeight = 20;
            this.comboBoxFormat.Items = ((System.Collections.Generic.List<string>)(resources.GetObject("comboBoxFormat.Items")));
            this.comboBoxFormat.Location = new System.Drawing.Point(652, 42);
            this.comboBoxFormat.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxFormat.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxFormat.ScrollBarWidth = 16;
            this.comboBoxFormat.SelectedIndex = -1;
            this.comboBoxFormat.SelectedItem = null;
            this.comboBoxFormat.Size = new System.Drawing.Size(77, 21);
            this.comboBoxFormat.TabIndex = 5;
            this.comboBoxFormat.ValueMember = null;
            this.comboBoxFormat.SelectedIndexChanged += new ExtendedControls.ComboBoxCustom.OnSelectedIndexChanged(this.comboBoxFormat_SelectedIndexChanged);
            // 
            // ImageHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.groupBox_Preview);
            this.Controls.Add(this.groupBox_Controls);
            this.Name = "ImageHandler";
            this.Size = new System.Drawing.Size(898, 662);
            this.Load += new System.EventHandler(this.ImageHandler_Load);
            this.groupBox_Preview.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox_Prevcontrols.ResumeLayout(false);
            this.groupBox_Prevcontrols.PerformLayout();
            this.groupBoxCropSettings.ResumeLayout(false);
            this.groupBoxCropSettings.PerformLayout();
            this.groupBox_Controls.ResumeLayout(false);
            this.groupBox_Controls.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.CheckBoxCustom checkBoxAutoConvert;
        private ExtendedControls.TextBoxBorder textBoxScreenshotsDir;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.ButtonExt buttonChnageEDScreenshot;
        private ExtendedControls.ButtonExt buttonImageStore;
        private System.Windows.Forms.Label label2;
        private ExtendedControls.TextBoxBorder textBoxOutputDir;
        private ExtendedControls.ComboBoxCustom comboBoxFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ExtendedControls.CheckBoxCustom checkBoxRemove;
        private ExtendedControls.ComboBoxCustom comboBoxFileNameFormat;
        private System.Windows.Forms.Label label4;
        private ExtendedControls.TextBoxBorder textBoxFileNameExample;
        private ExtendedControls.CheckBoxCustom checkBoxPreview;
        protected ExtendedControls.CheckBoxCustom checkBoxCropImage;
        private ExtendedControls.GroupBoxCustom groupBoxCropSettings;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private ExtendedControls.NumericUpDownCustom numericUpDownHeight;
        private ExtendedControls.NumericUpDownCustom numericUpDownWidth;
        private ExtendedControls.NumericUpDownCustom numericUpDownLeft;
        private ExtendedControls.NumericUpDownCustom numericUpDownTop;
        private ExtendedControls.CheckBoxCustom checkBoxHires;
        private ExtendedControls.GroupBoxCustom groupBox_Preview;
        private ExtendedControls.GroupBoxCustom groupBox_Controls;
        private ExtendedControls.GroupBoxCustom groupBox_Prevcontrols;
    }
}
