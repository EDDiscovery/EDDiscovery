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
            this.checkBoxAutoConvert = new System.Windows.Forms.CheckBox();
            this.textBoxScreenshotsDir = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonChnageEDScreenshot = new System.Windows.Forms.Button();
            this.buttonImageStore = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxOutputDir = new System.Windows.Forms.TextBox();
            this.comboBoxFormat = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.checkBoxRemove = new System.Windows.Forms.CheckBox();
            this.comboBoxFileNameFormat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxFileNameExample = new System.Windows.Forms.TextBox();
            this.checkBoxPreview = new System.Windows.Forms.CheckBox();
            this.checkBoxCropImage = new System.Windows.Forms.CheckBox();
            this.groupBoxCropSettings = new System.Windows.Forms.GroupBox();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownLeft = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownTop = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxHires = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBoxCropSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTop)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxAutoConvert
            // 
            this.checkBoxAutoConvert.AutoSize = true;
            this.checkBoxAutoConvert.Location = new System.Drawing.Point(6, 3);
            this.checkBoxAutoConvert.Name = "checkBoxAutoConvert";
            this.checkBoxAutoConvert.Size = new System.Drawing.Size(165, 17);
            this.checkBoxAutoConvert.TabIndex = 0;
            this.checkBoxAutoConvert.Text = "Auto convert ED screenshots";
            this.checkBoxAutoConvert.UseVisualStyleBackColor = true;
            this.checkBoxAutoConvert.CheckedChanged += new System.EventHandler(this.checkBoxAutoConvert_CheckedChanged);
            // 
            // textBoxScreenshotsDir
            // 
            this.textBoxScreenshotsDir.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxScreenshotsDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxScreenshotsDir.Location = new System.Drawing.Point(131, 26);
            this.textBoxScreenshotsDir.Name = "textBoxScreenshotsDir";
            this.textBoxScreenshotsDir.Size = new System.Drawing.Size(311, 20);
            this.textBoxScreenshotsDir.TabIndex = 1;
            this.textBoxScreenshotsDir.Leave += new System.EventHandler(this.textBoxScreenshotsDir_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "ED Screenshot directory";
            // 
            // buttonChnageEDScreenshot
            // 
            this.buttonChnageEDScreenshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonChnageEDScreenshot.Location = new System.Drawing.Point(448, 26);
            this.buttonChnageEDScreenshot.Name = "buttonChnageEDScreenshot";
            this.buttonChnageEDScreenshot.Size = new System.Drawing.Size(57, 23);
            this.buttonChnageEDScreenshot.TabIndex = 2;
            this.buttonChnageEDScreenshot.Text = "Browse";
            this.buttonChnageEDScreenshot.UseVisualStyleBackColor = true;
            this.buttonChnageEDScreenshot.Click += new System.EventHandler(this.buttonChnageEDScreenshot_Click);
            // 
            // buttonImageStore
            // 
            this.buttonImageStore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImageStore.Location = new System.Drawing.Point(448, 52);
            this.buttonImageStore.Name = "buttonImageStore";
            this.buttonImageStore.Size = new System.Drawing.Size(57, 23);
            this.buttonImageStore.TabIndex = 4;
            this.buttonImageStore.Text = "Browse";
            this.buttonImageStore.UseVisualStyleBackColor = true;
            this.buttonImageStore.Click += new System.EventHandler(this.buttonImageStore_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Converted pictures";
            // 
            // textBoxOutputDir
            // 
            this.textBoxOutputDir.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxOutputDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputDir.Location = new System.Drawing.Point(131, 52);
            this.textBoxOutputDir.Name = "textBoxOutputDir";
            this.textBoxOutputDir.Size = new System.Drawing.Size(311, 20);
            this.textBoxOutputDir.TabIndex = 3;
            this.textBoxOutputDir.Leave += new System.EventHandler(this.textBoxOutputDir_Leave);
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.ItemHeight = 13;
            this.comboBoxFormat.Items.AddRange(new object[] {
            "png",
            "jpg",
            "bmp",
            "tiff"});
            this.comboBoxFormat.Location = new System.Drawing.Point(588, 26);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFormat.TabIndex = 5;
            this.comboBoxFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxFormat_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(585, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Save as";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(188, 168);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(521, 329);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // checkBoxRemove
            // 
            this.checkBoxRemove.AutoSize = true;
            this.checkBoxRemove.Location = new System.Drawing.Point(6, 78);
            this.checkBoxRemove.Name = "checkBoxRemove";
            this.checkBoxRemove.Size = new System.Drawing.Size(212, 17);
            this.checkBoxRemove.TabIndex = 6;
            this.checkBoxRemove.Text = "Remove original image after conversion";
            this.checkBoxRemove.UseVisualStyleBackColor = true;
            this.checkBoxRemove.CheckedChanged += new System.EventHandler(this.checkBoxRemove_CheckedChanged);
            // 
            // comboBoxFileNameFormat
            // 
            this.comboBoxFileNameFormat.FormattingEnabled = true;
            this.comboBoxFileNameFormat.Items.AddRange(new object[] {
            "Sysname (YYYYMMDD-HHMMSS)",
            "Sysname (Windows dateformat)",
            "YYYY-MM-DD HH-MM-SS Sysname",
            "DD-MM-YYYY HH-MM-SS Sysname",
            "MM-DD-YYYY HH-MM-SS Sysname",
            "Keep original"});
            this.comboBoxFileNameFormat.Location = new System.Drawing.Point(131, 101);
            this.comboBoxFileNameFormat.Name = "comboBoxFileNameFormat";
            this.comboBoxFileNameFormat.Size = new System.Drawing.Size(218, 21);
            this.comboBoxFileNameFormat.TabIndex = 7;
            this.comboBoxFileNameFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxFileNameFormat_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Filename format";
            // 
            // textBoxFileNameExample
            // 
            this.textBoxFileNameExample.AccessibleRole = System.Windows.Forms.AccessibleRole.ScrollBar;
            this.textBoxFileNameExample.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileNameExample.Location = new System.Drawing.Point(370, 101);
            this.textBoxFileNameExample.Name = "textBoxFileNameExample";
            this.textBoxFileNameExample.ReadOnly = true;
            this.textBoxFileNameExample.Size = new System.Drawing.Size(184, 20);
            this.textBoxFileNameExample.TabIndex = 13;
            this.textBoxFileNameExample.TabStop = false;
            // 
            // checkBoxPreview
            // 
            this.checkBoxPreview.AutoSize = true;
            this.checkBoxPreview.Location = new System.Drawing.Point(188, 145);
            this.checkBoxPreview.Name = "checkBoxPreview";
            this.checkBoxPreview.Size = new System.Drawing.Size(64, 17);
            this.checkBoxPreview.TabIndex = 10;
            this.checkBoxPreview.Text = "Preview";
            this.checkBoxPreview.UseVisualStyleBackColor = true;
            this.checkBoxPreview.CheckedChanged += new System.EventHandler(this.checkBoxPreview_CheckedChanged);
            // 
            // checkBoxCropImage
            // 
            this.checkBoxCropImage.AutoSize = true;
            this.checkBoxCropImage.Location = new System.Drawing.Point(6, 145);
            this.checkBoxCropImage.Name = "checkBoxCropImage";
            this.checkBoxCropImage.Size = new System.Drawing.Size(80, 17);
            this.checkBoxCropImage.TabIndex = 8;
            this.checkBoxCropImage.Text = "Crop Image";
            this.checkBoxCropImage.UseVisualStyleBackColor = true;
            this.checkBoxCropImage.CheckedChanged += new System.EventHandler(this.checkBoxCropImage_CheckedChanged);
            // 
            // groupBoxCropSettings
            // 
            this.groupBoxCropSettings.Controls.Add(this.numericUpDownHeight);
            this.groupBoxCropSettings.Controls.Add(this.numericUpDownWidth);
            this.groupBoxCropSettings.Controls.Add(this.numericUpDownLeft);
            this.groupBoxCropSettings.Controls.Add(this.numericUpDownTop);
            this.groupBoxCropSettings.Controls.Add(this.label8);
            this.groupBoxCropSettings.Controls.Add(this.label7);
            this.groupBoxCropSettings.Controls.Add(this.label6);
            this.groupBoxCropSettings.Controls.Add(this.label5);
            this.groupBoxCropSettings.Location = new System.Drawing.Point(5, 168);
            this.groupBoxCropSettings.Name = "groupBoxCropSettings";
            this.groupBoxCropSettings.Size = new System.Drawing.Size(166, 125);
            this.groupBoxCropSettings.TabIndex = 9;
            this.groupBoxCropSettings.TabStop = false;
            this.groupBoxCropSettings.Text = "Crop Settings";
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.Location = new System.Drawing.Point(77, 98);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownHeight.TabIndex = 3;
            this.numericUpDownHeight.Leave += new System.EventHandler(this.numericUpDownHeight_Leave);
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.Location = new System.Drawing.Point(77, 72);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownWidth.TabIndex = 2;
            this.numericUpDownWidth.Leave += new System.EventHandler(this.numericUpDownWidth_Leave);
            // 
            // numericUpDownLeft
            // 
            this.numericUpDownLeft.Location = new System.Drawing.Point(77, 46);
            this.numericUpDownLeft.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownLeft.Name = "numericUpDownLeft";
            this.numericUpDownLeft.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownLeft.TabIndex = 1;
            this.numericUpDownLeft.Leave += new System.EventHandler(this.numericUpDownLeft_Leave);
            // 
            // numericUpDownTop
            // 
            this.numericUpDownTop.Location = new System.Drawing.Point(77, 20);
            this.numericUpDownTop.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDownTop.Name = "numericUpDownTop";
            this.numericUpDownTop.Size = new System.Drawing.Size(83, 20);
            this.numericUpDownTop.TabIndex = 0;
            this.numericUpDownTop.Leave += new System.EventHandler(this.numericUpDownTop_Leave);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 100);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Height";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Width";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 48);
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
            this.label5.TabIndex = 4;
            this.label5.Text = "Top";
            // 
            // checkBoxHires
            // 
            this.checkBoxHires.AutoSize = true;
            this.checkBoxHires.Location = new System.Drawing.Point(245, 78);
            this.checkBoxHires.Name = "checkBoxHires";
            this.checkBoxHires.Size = new System.Drawing.Size(152, 17);
            this.checkBoxHires.TabIndex = 14;
            this.checkBoxHires.Text = "Mark High Resolution Files";
            this.checkBoxHires.UseVisualStyleBackColor = true;
            this.checkBoxHires.CheckedChanged += new System.EventHandler(this.checkBox_hires_CheckedChanged);
            // 
            // ImageHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.checkBoxHires);
            this.Controls.Add(this.checkBoxCropImage);
            this.Controls.Add(this.groupBoxCropSettings);
            this.Controls.Add(this.checkBoxPreview);
            this.Controls.Add(this.textBoxFileNameExample);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxFileNameFormat);
            this.Controls.Add(this.checkBoxRemove);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxFormat);
            this.Controls.Add(this.buttonImageStore);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxOutputDir);
            this.Controls.Add(this.buttonChnageEDScreenshot);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxScreenshotsDir);
            this.Controls.Add(this.checkBoxAutoConvert);
            this.Name = "ImageHandler";
            this.Size = new System.Drawing.Size(712, 500);
            this.Load += new System.EventHandler(this.ImageHandler_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBoxCropSettings.ResumeLayout(false);
            this.groupBoxCropSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownTop)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBoxAutoConvert;
        private System.Windows.Forms.TextBox textBoxScreenshotsDir;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonChnageEDScreenshot;
        private System.Windows.Forms.Button buttonImageStore;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxOutputDir;
        private System.Windows.Forms.ComboBox comboBoxFormat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.CheckBox checkBoxRemove;
        private System.Windows.Forms.ComboBox comboBoxFileNameFormat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxFileNameExample;
        private System.Windows.Forms.CheckBox checkBoxPreview;
        protected System.Windows.Forms.CheckBox checkBoxCropImage;
        private System.Windows.Forms.GroupBox groupBoxCropSettings;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.NumericUpDown numericUpDownLeft;
        private System.Windows.Forms.NumericUpDown numericUpDownTop;
        private System.Windows.Forms.CheckBox checkBoxHires;
    }
}
