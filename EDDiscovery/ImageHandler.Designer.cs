/*
 * Copyright © 2016 EDDiscovery development team
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
namespace EDDiscovery.ImageHandler
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
            this.components = new System.ComponentModel.Container();
            this.groupBox_Preview = new ExtendedControls.GroupBoxCustom();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.groupBox_Prevcontrols = new ExtendedControls.GroupBoxCustom();
            this.label10 = new System.Windows.Forms.Label();
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
            this.checkBoxCopyClipboard = new ExtendedControls.CheckBoxCustom();
            this.checkBoxPreview = new ExtendedControls.CheckBoxCustom();
            this.groupBox_Controls = new ExtendedControls.GroupBoxCustom();
            this.checkBoxAutoConvert = new ExtendedControls.CheckBoxCustom();
            this.textBoxScreenshotsDir = new ExtendedControls.TextBoxBorder();
            this.checkBoxHires = new ExtendedControls.CheckBoxCustom();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxFileNameExample = new ExtendedControls.TextBoxBorder();
            this.buttonChnageEDScreenshot = new ExtendedControls.ButtonExt();
            this.label11 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxOutputDir = new ExtendedControls.TextBoxBorder();
            this.comboBoxSubFolder = new ExtendedControls.ComboBoxCustom();
            this.comboBoxFileNameFormat = new ExtendedControls.ComboBoxCustom();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBoxRemove = new ExtendedControls.CheckBoxCustom();
            this.buttonImageStore = new ExtendedControls.ButtonExt();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxScanFor = new ExtendedControls.ComboBoxCustom();
            this.comboBoxFormat = new ExtendedControls.ComboBoxCustom();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox_Preview.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
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
            this.groupBox_Preview.Controls.Add(this.pictureBox);
            this.groupBox_Preview.Controls.Add(this.groupBox_Prevcontrols);
            this.groupBox_Preview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Preview.FillClientAreaWithAlternateColor = false;
            this.groupBox_Preview.Location = new System.Drawing.Point(0, 196);
            this.groupBox_Preview.Name = "groupBox_Preview";
            this.groupBox_Preview.Size = new System.Drawing.Size(898, 466);
            this.groupBox_Preview.TabIndex = 15;
            this.groupBox_Preview.TabStop = false;
            this.groupBox_Preview.TextPadding = 0;
            this.groupBox_Preview.TextStartPosition = -1;
            // 
            // pictureBox
            // 
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(149, 16);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(746, 447);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 9;
            this.pictureBox.TabStop = false;
            // 
            // groupBox_Prevcontrols
            // 
            this.groupBox_Prevcontrols.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBox_Prevcontrols.BackColorScaling = 0.5F;
            this.groupBox_Prevcontrols.BorderColor = System.Drawing.Color.LightGray;
            this.groupBox_Prevcontrols.BorderColorScaling = 0.5F;
            this.groupBox_Prevcontrols.Controls.Add(this.label10);
            this.groupBox_Prevcontrols.Controls.Add(this.groupBoxCropSettings);
            this.groupBox_Prevcontrols.Controls.Add(this.checkBoxCropImage);
            this.groupBox_Prevcontrols.Controls.Add(this.checkBoxCopyClipboard);
            this.groupBox_Prevcontrols.Controls.Add(this.checkBoxPreview);
            this.groupBox_Prevcontrols.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox_Prevcontrols.FillClientAreaWithAlternateColor = false;
            this.groupBox_Prevcontrols.Location = new System.Drawing.Point(3, 16);
            this.groupBox_Prevcontrols.Name = "groupBox_Prevcontrols";
            this.groupBox_Prevcontrols.Size = new System.Drawing.Size(146, 447);
            this.groupBox_Prevcontrols.TabIndex = 11;
            this.groupBox_Prevcontrols.TabStop = false;
            this.groupBox_Prevcontrols.TextPadding = 0;
            this.groupBox_Prevcontrols.TextStartPosition = -1;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(8, 252);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 65);
            this.label10.TabIndex = 11;
            this.label10.Text = "For Steam, select Scan For JPG and turn off remove originals";
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
            this.groupBoxCropSettings.Location = new System.Drawing.Point(6, 89);
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
            this.numericUpDownHeight.Text = "0";
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
            this.numericUpDownWidth.Text = "0";
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
            this.numericUpDownLeft.Text = "0";
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
            this.numericUpDownTop.Text = "0";
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
            this.checkBoxCropImage.Location = new System.Drawing.Point(8, 65);
            this.checkBoxCropImage.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCropImage.Name = "checkBoxCropImage";
            this.checkBoxCropImage.Size = new System.Drawing.Size(80, 17);
            this.checkBoxCropImage.TabIndex = 8;
            this.checkBoxCropImage.Text = "Crop Image";
            this.checkBoxCropImage.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxCropImage, "Crop image to values below");
            this.checkBoxCropImage.UseVisualStyleBackColor = true;
            this.checkBoxCropImage.CheckedChanged += new System.EventHandler(this.checkBoxCropImage_CheckedChanged);
            // 
            // checkBoxCopyClipboard
            // 
            this.checkBoxCopyClipboard.AutoSize = true;
            this.checkBoxCopyClipboard.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCopyClipboard.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCopyClipboard.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCopyClipboard.FontNerfReduction = 0.5F;
            this.checkBoxCopyClipboard.Location = new System.Drawing.Point(8, 42);
            this.checkBoxCopyClipboard.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCopyClipboard.Name = "checkBoxCopyClipboard";
            this.checkBoxCopyClipboard.Size = new System.Drawing.Size(109, 17);
            this.checkBoxCopyClipboard.TabIndex = 10;
            this.checkBoxCopyClipboard.Text = "Copy to Clipboard";
            this.checkBoxCopyClipboard.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxCopyClipboard, "Auto Copy to the clipboard");
            this.checkBoxCopyClipboard.UseVisualStyleBackColor = true;
            this.checkBoxCopyClipboard.CheckedChanged += new System.EventHandler(this.checkBoxCopyClipboard_CheckedChanged);
            // 
            // checkBoxPreview
            // 
            this.checkBoxPreview.AutoSize = true;
            this.checkBoxPreview.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxPreview.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxPreview.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxPreview.FontNerfReduction = 0.5F;
            this.checkBoxPreview.Location = new System.Drawing.Point(8, 19);
            this.checkBoxPreview.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxPreview.Name = "checkBoxPreview";
            this.checkBoxPreview.Size = new System.Drawing.Size(64, 17);
            this.checkBoxPreview.TabIndex = 10;
            this.checkBoxPreview.Text = "Preview";
            this.checkBoxPreview.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxPreview, "Preview the file in this window");
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
            this.groupBox_Controls.Controls.Add(this.label11);
            this.groupBox_Controls.Controls.Add(this.label4);
            this.groupBox_Controls.Controls.Add(this.textBoxOutputDir);
            this.groupBox_Controls.Controls.Add(this.comboBoxSubFolder);
            this.groupBox_Controls.Controls.Add(this.comboBoxFileNameFormat);
            this.groupBox_Controls.Controls.Add(this.label2);
            this.groupBox_Controls.Controls.Add(this.checkBoxRemove);
            this.groupBox_Controls.Controls.Add(this.buttonImageStore);
            this.groupBox_Controls.Controls.Add(this.label9);
            this.groupBox_Controls.Controls.Add(this.label3);
            this.groupBox_Controls.Controls.Add(this.comboBoxScanFor);
            this.groupBox_Controls.Controls.Add(this.comboBoxFormat);
            this.groupBox_Controls.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox_Controls.FillClientAreaWithAlternateColor = false;
            this.groupBox_Controls.Location = new System.Drawing.Point(0, 0);
            this.groupBox_Controls.Name = "groupBox_Controls";
            this.groupBox_Controls.Size = new System.Drawing.Size(898, 196);
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
            this.checkBoxAutoConvert.Size = new System.Drawing.Size(200, 17);
            this.checkBoxAutoConvert.TabIndex = 0;
            this.checkBoxAutoConvert.Text = "Auto convert ED/Steam screenshots";
            this.checkBoxAutoConvert.TickBoxReductionSize = 10;
            this.checkBoxAutoConvert.UseVisualStyleBackColor = true;
            this.checkBoxAutoConvert.CheckedChanged += new System.EventHandler(this.checkBoxAutoConvert_CheckedChanged);
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
            this.textBoxScreenshotsDir.Location = new System.Drawing.Point(214, 42);
            this.textBoxScreenshotsDir.Multiline = false;
            this.textBoxScreenshotsDir.Name = "textBoxScreenshotsDir";
            this.textBoxScreenshotsDir.ReadOnly = false;
            this.textBoxScreenshotsDir.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxScreenshotsDir.SelectionLength = 0;
            this.textBoxScreenshotsDir.SelectionStart = 0;
            this.textBoxScreenshotsDir.Size = new System.Drawing.Size(281, 20);
            this.textBoxScreenshotsDir.TabIndex = 1;
            this.textBoxScreenshotsDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxScreenshotsDir, "Enter the folder where Elite or Steam stores its screenshot");
            this.textBoxScreenshotsDir.WordWrap = true;
            this.textBoxScreenshotsDir.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxScreenshotsDir_KeyUp);
            this.textBoxScreenshotsDir.Leave += new System.EventHandler(this.textBoxScreenshotsDir_Leave);
            // 
            // checkBoxHires
            // 
            this.checkBoxHires.AutoSize = true;
            this.checkBoxHires.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxHires.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxHires.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxHires.FontNerfReduction = 0.5F;
            this.checkBoxHires.Location = new System.Drawing.Point(257, 167);
            this.checkBoxHires.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxHires.Name = "checkBoxHires";
            this.checkBoxHires.Size = new System.Drawing.Size(152, 17);
            this.checkBoxHires.TabIndex = 14;
            this.checkBoxHires.Text = "Mark High Resolution Files";
            this.checkBoxHires.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxHires, "Mark high res files with a marker in the name");
            this.checkBoxHires.UseVisualStyleBackColor = true;
            this.checkBoxHires.CheckedChanged += new System.EventHandler(this.checkBox_hires_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 13);
            this.label1.TabIndex = 2;
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
            this.textBoxFileNameExample.Location = new System.Drawing.Point(448, 128);
            this.textBoxFileNameExample.Multiline = false;
            this.textBoxFileNameExample.Name = "textBoxFileNameExample";
            this.textBoxFileNameExample.ReadOnly = true;
            this.textBoxFileNameExample.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFileNameExample.SelectionLength = 0;
            this.textBoxFileNameExample.SelectionStart = 0;
            this.textBoxFileNameExample.Size = new System.Drawing.Size(220, 20);
            this.textBoxFileNameExample.TabIndex = 13;
            this.textBoxFileNameExample.TabStop = false;
            this.textBoxFileNameExample.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxFileNameExample, "Example file name");
            this.textBoxFileNameExample.WordWrap = true;
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
            this.toolTip.SetToolTip(this.buttonChnageEDScreenshot, "Browse to the folder");
            this.buttonChnageEDScreenshot.UseVisualStyleBackColor = true;
            this.buttonChnageEDScreenshot.Click += new System.EventHandler(this.buttonChnageEDScreenshot_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(15, 100);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(67, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "In Sub folder";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 131);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 12;
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
            this.textBoxOutputDir.Location = new System.Drawing.Point(214, 68);
            this.textBoxOutputDir.Multiline = false;
            this.textBoxOutputDir.Name = "textBoxOutputDir";
            this.textBoxOutputDir.ReadOnly = false;
            this.textBoxOutputDir.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxOutputDir.SelectionLength = 0;
            this.textBoxOutputDir.SelectionStart = 0;
            this.textBoxOutputDir.Size = new System.Drawing.Size(281, 20);
            this.textBoxOutputDir.TabIndex = 3;
            this.textBoxOutputDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxOutputDir, "Enter the folder to store converted pictures in");
            this.textBoxOutputDir.WordWrap = true;
            this.textBoxOutputDir.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxOutputDir_KeyUp);
            this.textBoxOutputDir.Leave += new System.EventHandler(this.textBoxOutputDir_Leave);
            // 
            // comboBoxSubFolder
            // 
            this.comboBoxSubFolder.ArrowWidth = 1;
            this.comboBoxSubFolder.BorderColor = System.Drawing.Color.Red;
            this.comboBoxSubFolder.ButtonColorScaling = 0.5F;
            this.comboBoxSubFolder.DataSource = null;
            this.comboBoxSubFolder.DisplayMember = "";
            this.comboBoxSubFolder.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxSubFolder.DropDownHeight = 200;
            this.comboBoxSubFolder.DropDownWidth = 218;
            this.comboBoxSubFolder.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSubFolder.ItemHeight = 13;
            this.comboBoxSubFolder.Location = new System.Drawing.Point(214, 96);
            this.comboBoxSubFolder.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxSubFolder.Name = "comboBoxSubFolder";
            this.comboBoxSubFolder.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxSubFolder.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxSubFolder.ScrollBarWidth = 16;
            this.comboBoxSubFolder.SelectedIndex = -1;
            this.comboBoxSubFolder.SelectedItem = null;
            this.comboBoxSubFolder.SelectedValue = null;
            this.comboBoxSubFolder.Size = new System.Drawing.Size(220, 21);
            this.comboBoxSubFolder.TabIndex = 7;
            this.toolTip.SetToolTip(this.comboBoxSubFolder, "Select if a subfolder is to be used");
            this.comboBoxSubFolder.ValueMember = "";
            this.comboBoxSubFolder.SelectedIndexChanged += new System.EventHandler(this.comboBoxSubFolder_SelectedIndexChanged);
            // 
            // comboBoxFileNameFormat
            // 
            this.comboBoxFileNameFormat.ArrowWidth = 1;
            this.comboBoxFileNameFormat.BorderColor = System.Drawing.Color.Red;
            this.comboBoxFileNameFormat.ButtonColorScaling = 0.5F;
            this.comboBoxFileNameFormat.DataSource = null;
            this.comboBoxFileNameFormat.DisplayMember = "";
            this.comboBoxFileNameFormat.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxFileNameFormat.DropDownHeight = 200;
            this.comboBoxFileNameFormat.DropDownWidth = 218;
            this.comboBoxFileNameFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxFileNameFormat.ItemHeight = 13;
            this.comboBoxFileNameFormat.Location = new System.Drawing.Point(214, 128);
            this.comboBoxFileNameFormat.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxFileNameFormat.Name = "comboBoxFileNameFormat";
            this.comboBoxFileNameFormat.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxFileNameFormat.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxFileNameFormat.ScrollBarWidth = 16;
            this.comboBoxFileNameFormat.SelectedIndex = -1;
            this.comboBoxFileNameFormat.SelectedItem = null;
            this.comboBoxFileNameFormat.SelectedValue = null;
            this.comboBoxFileNameFormat.Size = new System.Drawing.Size(220, 21);
            this.comboBoxFileNameFormat.TabIndex = 7;
            this.toolTip.SetToolTip(this.comboBoxFileNameFormat, "Select the filename format of the created file");
            this.comboBoxFileNameFormat.ValueMember = "";
            this.comboBoxFileNameFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxFileNameFormat_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Store Converted pictures";
            // 
            // checkBoxRemove
            // 
            this.checkBoxRemove.AutoSize = true;
            this.checkBoxRemove.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxRemove.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxRemove.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxRemove.FontNerfReduction = 0.5F;
            this.checkBoxRemove.Location = new System.Drawing.Point(18, 167);
            this.checkBoxRemove.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxRemove.Name = "checkBoxRemove";
            this.checkBoxRemove.Size = new System.Drawing.Size(212, 17);
            this.checkBoxRemove.TabIndex = 6;
            this.checkBoxRemove.Text = "Remove original image after conversion";
            this.checkBoxRemove.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxRemove, "After conversion, delete the original");
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
            this.toolTip.SetToolTip(this.buttonImageStore, "Browse to the folder");
            this.buttonImageStore.UseVisualStyleBackColor = true;
            this.buttonImageStore.Click += new System.EventHandler(this.buttonImageStore_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(592, 45);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Scan for";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(592, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 8;
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
            this.comboBoxScanFor.Location = new System.Drawing.Point(656, 42);
            this.comboBoxScanFor.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxScanFor.Name = "comboBoxScanFor";
            this.comboBoxScanFor.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxScanFor.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxScanFor.ScrollBarWidth = 16;
            this.comboBoxScanFor.SelectedIndex = -1;
            this.comboBoxScanFor.SelectedItem = null;
            this.comboBoxScanFor.SelectedValue = null;
            this.comboBoxScanFor.Size = new System.Drawing.Size(135, 21);
            this.comboBoxScanFor.TabIndex = 5;
            this.toolTip.SetToolTip(this.comboBoxScanFor, "Scan folders for this format of file");
            this.comboBoxScanFor.ValueMember = "";
            this.comboBoxScanFor.SelectedIndexChanged += new System.EventHandler(this.comboBoxScanFor_SelectedIndexChanged);
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.ArrowWidth = 1;
            this.comboBoxFormat.BorderColor = System.Drawing.Color.Red;
            this.comboBoxFormat.ButtonColorScaling = 0.5F;
            this.comboBoxFormat.DataSource = null;
            this.comboBoxFormat.DisplayMember = "";
            this.comboBoxFormat.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxFormat.DropDownHeight = 200;
            this.comboBoxFormat.DropDownWidth = 161;
            this.comboBoxFormat.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxFormat.ItemHeight = 13;
            this.comboBoxFormat.Location = new System.Drawing.Point(656, 66);
            this.comboBoxFormat.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxFormat.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxFormat.ScrollBarWidth = 16;
            this.comboBoxFormat.SelectedIndex = -1;
            this.comboBoxFormat.SelectedItem = null;
            this.comboBoxFormat.SelectedValue = null;
            this.comboBoxFormat.Size = new System.Drawing.Size(135, 21);
            this.comboBoxFormat.TabIndex = 5;
            this.toolTip.SetToolTip(this.comboBoxFormat, "Save the image as this type");
            this.comboBoxFormat.ValueMember = "";
            this.comboBoxFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxFormat_SelectedIndexChanged);
            // 
            // toolTip1
            // 
            this.toolTip.ShowAlways = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
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
        private System.Windows.Forms.PictureBox pictureBox;
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
        private System.Windows.Forms.Label label9;
        private ExtendedControls.ComboBoxCustom comboBoxScanFor;
        private System.Windows.Forms.Label label10;
        private ExtendedControls.CheckBoxCustom checkBoxCopyClipboard;
        private System.Windows.Forms.Label label11;
        private ExtendedControls.ComboBoxCustom comboBoxSubFolder;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
