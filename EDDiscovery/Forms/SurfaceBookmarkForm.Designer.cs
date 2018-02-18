namespace EDDiscovery.Forms
{
    partial class SurfaceBookmarkForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SurfaceBookmarkForm));
            this.comboBoxBodies = new ExtendedControls.ComboBoxCustom();
            this.labelExt1 = new ExtendedControls.LabelExt();
            this.labelSystemName = new ExtendedControls.LabelExt();
            this.labelExt2 = new ExtendedControls.LabelExt();
            this.numberBoxLatitude = new ExtendedControls.NumberBoxDouble();
            this.labelExt3 = new ExtendedControls.LabelExt();
            this.numberBoxLongitude = new ExtendedControls.NumberBoxDouble();
            this.textBoxName = new ExtendedControls.TextBoxBorder();
            this.labelExt4 = new ExtendedControls.LabelExt();
            this.labelExt5 = new ExtendedControls.LabelExt();
            this.richTextBoxDescription = new ExtendedControls.RichTextBoxScroll();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.SuspendLayout();
            // 
            // comboBoxBodies
            // 
            this.comboBoxBodies.ArrowWidth = 1;
            this.comboBoxBodies.BorderColor = System.Drawing.Color.White;
            this.comboBoxBodies.ButtonColorScaling = 0.5F;
            this.comboBoxBodies.DataSource = null;
            this.comboBoxBodies.DisplayMember = "";
            this.comboBoxBodies.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxBodies.DropDownHeight = 106;
            this.comboBoxBodies.DropDownWidth = 200;
            this.comboBoxBodies.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxBodies.ItemHeight = 13;
            this.comboBoxBodies.Location = new System.Drawing.Point(154, 39);
            this.comboBoxBodies.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxBodies.Name = "comboBoxBodies";
            this.comboBoxBodies.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxBodies.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxBodies.ScrollBarWidth = 16;
            this.comboBoxBodies.SelectedIndex = -1;
            this.comboBoxBodies.SelectedItem = null;
            this.comboBoxBodies.SelectedValue = null;
            this.comboBoxBodies.Size = new System.Drawing.Size(200, 21);
            this.comboBoxBodies.TabIndex = 1;
            this.comboBoxBodies.Text = "comboBoxCustom1";
            this.comboBoxBodies.ValueMember = "";
            this.comboBoxBodies.SelectedIndexChanged += new System.EventHandler(this.comboBoxBodies_SelectedIndexChanged);
            // 
            // labelExt1
            // 
            this.labelExt1.AutoSize = true;
            this.labelExt1.Location = new System.Drawing.Point(70, 39);
            this.labelExt1.Name = "labelExt1";
            this.labelExt1.Size = new System.Drawing.Size(78, 13);
            this.labelExt1.TabIndex = 2;
            this.labelExt1.Text = "Planetary Body";
            this.labelExt1.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // labelSystemName
            // 
            this.labelSystemName.AutoSize = true;
            this.labelSystemName.Location = new System.Drawing.Point(29, 9);
            this.labelSystemName.Name = "labelSystemName";
            this.labelSystemName.Size = new System.Drawing.Size(119, 13);
            this.labelSystemName.TabIndex = 3;
            this.labelSystemName.Text = "Hypiae Brue NI-B c13-0";
            this.labelSystemName.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // labelExt2
            // 
            this.labelExt2.AutoSize = true;
            this.labelExt2.Location = new System.Drawing.Point(85, 98);
            this.labelExt2.Name = "labelExt2";
            this.labelExt2.Size = new System.Drawing.Size(63, 13);
            this.labelExt2.TabIndex = 4;
            this.labelExt2.Text = "Coordinates";
            this.labelExt2.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // numberBoxLatitude
            // 
            this.numberBoxLatitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxLatitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxLatitude.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxLatitude.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxLatitude.BorderColorScaling = 0.5F;
            this.numberBoxLatitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxLatitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxLatitude.DelayBeforeNotification = 0;
            this.numberBoxLatitude.Format = "N4";
            this.numberBoxLatitude.InErrorCondition = false;
            this.numberBoxLatitude.Location = new System.Drawing.Point(182, 98);
            this.numberBoxLatitude.Maximum = 180D;
            this.numberBoxLatitude.Minimum = -180D;
            this.numberBoxLatitude.Multiline = false;
            this.numberBoxLatitude.Name = "numberBoxLatitude";
            this.numberBoxLatitude.ReadOnly = false;
            this.numberBoxLatitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxLatitude.SelectionLength = 0;
            this.numberBoxLatitude.SelectionStart = 0;
            this.numberBoxLatitude.Size = new System.Drawing.Size(75, 20);
            this.numberBoxLatitude.TabIndex = 3;
            this.numberBoxLatitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxLatitude.Value = 0D;
            this.numberBoxLatitude.WordWrap = true;
            // 
            // labelExt3
            // 
            this.labelExt3.AutoSize = true;
            this.labelExt3.Location = new System.Drawing.Point(263, 105);
            this.labelExt3.Name = "labelExt3";
            this.labelExt3.Size = new System.Drawing.Size(10, 13);
            this.labelExt3.TabIndex = 6;
            this.labelExt3.Text = ",";
            this.labelExt3.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // numberBoxLongitude
            // 
            this.numberBoxLongitude.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxLongitude.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxLongitude.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxLongitude.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxLongitude.BorderColorScaling = 0.5F;
            this.numberBoxLongitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxLongitude.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxLongitude.DelayBeforeNotification = 0;
            this.numberBoxLongitude.Format = "N4";
            this.numberBoxLongitude.InErrorCondition = false;
            this.numberBoxLongitude.Location = new System.Drawing.Point(279, 98);
            this.numberBoxLongitude.Maximum = 180D;
            this.numberBoxLongitude.Minimum = -180D;
            this.numberBoxLongitude.Multiline = false;
            this.numberBoxLongitude.Name = "numberBoxLongitude";
            this.numberBoxLongitude.ReadOnly = false;
            this.numberBoxLongitude.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxLongitude.SelectionLength = 0;
            this.numberBoxLongitude.SelectionStart = 0;
            this.numberBoxLongitude.Size = new System.Drawing.Size(75, 20);
            this.numberBoxLongitude.TabIndex = 4;
            this.numberBoxLongitude.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxLongitude.Value = 0D;
            this.numberBoxLongitude.WordWrap = true;
            // 
            // textBoxName
            // 
            this.textBoxName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxName.BorderColorScaling = 0.5F;
            this.textBoxName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxName.InErrorCondition = false;
            this.textBoxName.Location = new System.Drawing.Point(154, 68);
            this.textBoxName.Multiline = false;
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.ReadOnly = false;
            this.textBoxName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxName.SelectionLength = 0;
            this.textBoxName.SelectionStart = 0;
            this.textBoxName.Size = new System.Drawing.Size(200, 20);
            this.textBoxName.TabIndex = 2;
            this.textBoxName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxName.WordWrap = true;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // labelExt4
            // 
            this.labelExt4.AutoSize = true;
            this.labelExt4.Location = new System.Drawing.Point(113, 68);
            this.labelExt4.Name = "labelExt4";
            this.labelExt4.Size = new System.Drawing.Size(35, 13);
            this.labelExt4.TabIndex = 9;
            this.labelExt4.Text = "Name";
            this.labelExt4.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // labelExt5
            // 
            this.labelExt5.AutoSize = true;
            this.labelExt5.Location = new System.Drawing.Point(88, 124);
            this.labelExt5.Name = "labelExt5";
            this.labelExt5.Size = new System.Drawing.Size(60, 13);
            this.labelExt5.TabIndex = 10;
            this.labelExt5.Text = "Description";
            this.labelExt5.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // richTextBoxDescription
            // 
            this.richTextBoxDescription.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBoxDescription.BorderColorScaling = 0.5F;
            this.richTextBoxDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.richTextBoxDescription.HideScrollBar = true;
            this.richTextBoxDescription.Location = new System.Drawing.Point(154, 124);
            this.richTextBoxDescription.Name = "richTextBoxDescription";
            this.richTextBoxDescription.ReadOnly = false;
            this.richTextBoxDescription.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.richTextBoxDescription.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.richTextBoxDescription.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxDescription.ScrollBarBorderColor = System.Drawing.Color.White;
            this.richTextBoxDescription.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.richTextBoxDescription.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBoxDescription.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.richTextBoxDescription.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.richTextBoxDescription.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.richTextBoxDescription.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.richTextBoxDescription.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.richTextBoxDescription.ScrollBarWidth = 20;
            this.richTextBoxDescription.ShowLineCount = false;
            this.richTextBoxDescription.Size = new System.Drawing.Size(200, 100);
            this.richTextBoxDescription.TabIndex = 5;
            this.richTextBoxDescription.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxDescription.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(279, 231);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(198, 230);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // SurfaceBookmarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(366, 261);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.richTextBoxDescription);
            this.Controls.Add(this.labelExt5);
            this.Controls.Add(this.labelExt4);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.numberBoxLongitude);
            this.Controls.Add(this.labelExt3);
            this.Controls.Add(this.numberBoxLatitude);
            this.Controls.Add(this.labelExt2);
            this.Controls.Add(this.labelSystemName);
            this.Controls.Add(this.labelExt1);
            this.Controls.Add(this.comboBoxBodies);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SurfaceBookmarkForm";
            this.Text = "Surface Bookmark";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ComboBoxCustom comboBoxBodies;
        private ExtendedControls.LabelExt labelExt1;
        private ExtendedControls.LabelExt labelSystemName;
        private ExtendedControls.LabelExt labelExt2;
        private ExtendedControls.NumberBoxDouble numberBoxLatitude;
        private ExtendedControls.LabelExt labelExt3;
        private ExtendedControls.NumberBoxDouble numberBoxLongitude;
        private ExtendedControls.TextBoxBorder textBoxName;
        private ExtendedControls.LabelExt labelExt4;
        private ExtendedControls.LabelExt labelExt5;
        private ExtendedControls.RichTextBoxScroll richTextBoxDescription;
        private ExtendedControls.ButtonExt buttonOK;
        private ExtendedControls.ButtonExt buttonCancel;
    }
}