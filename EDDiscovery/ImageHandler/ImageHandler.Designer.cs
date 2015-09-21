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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
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
            this.textBoxScreenshotsDir.Size = new System.Drawing.Size(281, 20);
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
            this.buttonChnageEDScreenshot.Location = new System.Drawing.Point(418, 26);
            this.buttonChnageEDScreenshot.Name = "buttonChnageEDScreenshot";
            this.buttonChnageEDScreenshot.Size = new System.Drawing.Size(57, 23);
            this.buttonChnageEDScreenshot.TabIndex = 3;
            this.buttonChnageEDScreenshot.Text = "Browse";
            this.buttonChnageEDScreenshot.UseVisualStyleBackColor = true;
            this.buttonChnageEDScreenshot.Click += new System.EventHandler(this.buttonChnageEDScreenshot_Click);
            // 
            // buttonImageStore
            // 
            this.buttonImageStore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImageStore.Location = new System.Drawing.Point(418, 52);
            this.buttonImageStore.Name = "buttonImageStore";
            this.buttonImageStore.Size = new System.Drawing.Size(57, 23);
            this.buttonImageStore.TabIndex = 6;
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
            this.textBoxOutputDir.Size = new System.Drawing.Size(281, 20);
            this.textBoxOutputDir.TabIndex = 4;
            this.textBoxOutputDir.Leave += new System.EventHandler(this.textBoxOutputDir_Leave);
            // 
            // comboBoxFormat
            // 
            this.comboBoxFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFormat.FormattingEnabled = true;
            this.comboBoxFormat.Items.AddRange(new object[] {
            "Png",
            "Jpg",
            "Bmp",
            "Tiff"});
            this.comboBoxFormat.Location = new System.Drawing.Point(511, 25);
            this.comboBoxFormat.Name = "comboBoxFormat";
            this.comboBoxFormat.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFormat.TabIndex = 7;
            this.comboBoxFormat.SelectedIndexChanged += new System.EventHandler(this.comboBoxFormat_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(508, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Save as";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(15, 154);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(406, 280);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // checkBoxRemove
            // 
            this.checkBoxRemove.AutoSize = true;
            this.checkBoxRemove.Location = new System.Drawing.Point(6, 78);
            this.checkBoxRemove.Name = "checkBoxRemove";
            this.checkBoxRemove.Size = new System.Drawing.Size(212, 17);
            this.checkBoxRemove.TabIndex = 10;
            this.checkBoxRemove.Text = "Remove original image after conversion";
            this.checkBoxRemove.UseVisualStyleBackColor = true;
            this.checkBoxRemove.CheckedChanged += new System.EventHandler(this.checkBoxRemove_CheckedChanged);
            // 
            // comboBoxFileNameFormat
            // 
            this.comboBoxFileNameFormat.FormattingEnabled = true;
            this.comboBoxFileNameFormat.Items.AddRange(new object[] {
            "Sysname (short time)",
            "Sysname (Windows dateformat)",
            "Keep original"});
            this.comboBoxFileNameFormat.Location = new System.Drawing.Point(131, 101);
            this.comboBoxFileNameFormat.Name = "comboBoxFileNameFormat";
            this.comboBoxFileNameFormat.Size = new System.Drawing.Size(121, 21);
            this.comboBoxFileNameFormat.TabIndex = 11;
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
            this.textBoxFileNameExample.Location = new System.Drawing.Point(258, 101);
            this.textBoxFileNameExample.Name = "textBoxFileNameExample";
            this.textBoxFileNameExample.ReadOnly = true;
            this.textBoxFileNameExample.Size = new System.Drawing.Size(154, 20);
            this.textBoxFileNameExample.TabIndex = 13;
            // 
            // ImageHandler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
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
            this.Size = new System.Drawing.Size(682, 451);
            this.Load += new System.EventHandler(this.ImageHandler_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
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
    }
}
