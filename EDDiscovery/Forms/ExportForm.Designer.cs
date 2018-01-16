namespace EDDiscovery.Forms
{
    partial class ExportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportForm));
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonSemiColon = new ExtendedControls.RadioButtonCustom();
            this.buttonExport = new ExtendedControls.ButtonExt();
            this.radioButtonComma = new ExtendedControls.RadioButtonCustom();
            this.checkBoxIncludeHeader = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomAutoOpen = new ExtendedControls.CheckBoxCustom();
            this.customDateTimePickerFrom = new ExtendedControls.CustomDateTimePicker();
            this.customDateTimePickerTo = new ExtendedControls.CustomDateTimePicker();
            this.comboBoxCustomExportType = new ExtendedControls.ComboBoxCustom();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.label_index = new System.Windows.Forms.Label();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelBottom.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.buttonCancel);
            this.panelBottom.Controls.Add(this.label1);
            this.panelBottom.Controls.Add(this.radioButtonSemiColon);
            this.panelBottom.Controls.Add(this.buttonExport);
            this.panelBottom.Controls.Add(this.radioButtonComma);
            this.panelBottom.Controls.Add(this.checkBoxIncludeHeader);
            this.panelBottom.Controls.Add(this.checkBoxCustomAutoOpen);
            this.panelBottom.Location = new System.Drawing.Point(6, 107);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(257, 133);
            this.panelBottom.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.Location = new System.Drawing.Point(98, 106);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "CVS Seperator";
            // 
            // radioButtonSemiColon
            // 
            this.radioButtonSemiColon.AutoSize = true;
            this.radioButtonSemiColon.FontNerfReduction = 0.5F;
            this.radioButtonSemiColon.Location = new System.Drawing.Point(146, 70);
            this.radioButtonSemiColon.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonSemiColon.Name = "radioButtonSemiColon";
            this.radioButtonSemiColon.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonSemiColon.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonSemiColon.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonSemiColon.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButtonSemiColon.Size = new System.Drawing.Size(74, 17);
            this.radioButtonSemiColon.TabIndex = 1;
            this.radioButtonSemiColon.TabStop = true;
            this.radioButtonSemiColon.Text = "Semicolon";
            this.radioButtonSemiColon.UseVisualStyleBackColor = true;
            // 
            // buttonExport
            // 
            this.buttonExport.BorderColorScaling = 1.25F;
            this.buttonExport.ButtonColorScaling = 0.5F;
            this.buttonExport.ButtonDisabledScaling = 0.5F;
            this.buttonExport.Location = new System.Drawing.Point(179, 106);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(75, 23);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // radioButtonComma
            // 
            this.radioButtonComma.AutoSize = true;
            this.radioButtonComma.FontNerfReduction = 0.5F;
            this.radioButtonComma.Location = new System.Drawing.Point(146, 45);
            this.radioButtonComma.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonComma.Name = "radioButtonComma";
            this.radioButtonComma.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonComma.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonComma.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonComma.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButtonComma.Size = new System.Drawing.Size(60, 17);
            this.radioButtonComma.TabIndex = 0;
            this.radioButtonComma.TabStop = true;
            this.radioButtonComma.Text = "Comma";
            this.radioButtonComma.UseVisualStyleBackColor = true;
            // 
            // checkBoxIncludeHeader
            // 
            this.checkBoxIncludeHeader.AutoSize = true;
            this.checkBoxIncludeHeader.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxIncludeHeader.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxIncludeHeader.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxIncludeHeader.FontNerfReduction = 0.5F;
            this.checkBoxIncludeHeader.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxIncludeHeader.Location = new System.Drawing.Point(12, 14);
            this.checkBoxIncludeHeader.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxIncludeHeader.Name = "checkBoxIncludeHeader";
            this.checkBoxIncludeHeader.Size = new System.Drawing.Size(99, 17);
            this.checkBoxIncludeHeader.TabIndex = 1;
            this.checkBoxIncludeHeader.Text = "Include Header";
            this.checkBoxIncludeHeader.TickBoxReductionSize = 10;
            this.checkBoxIncludeHeader.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomAutoOpen
            // 
            this.checkBoxCustomAutoOpen.AutoSize = true;
            this.checkBoxCustomAutoOpen.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomAutoOpen.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomAutoOpen.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomAutoOpen.FontNerfReduction = 0.5F;
            this.checkBoxCustomAutoOpen.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomAutoOpen.Location = new System.Drawing.Point(146, 14);
            this.checkBoxCustomAutoOpen.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomAutoOpen.Name = "checkBoxCustomAutoOpen";
            this.checkBoxCustomAutoOpen.Size = new System.Drawing.Size(91, 17);
            this.checkBoxCustomAutoOpen.TabIndex = 1;
            this.checkBoxCustomAutoOpen.Text = "Open in excel";
            this.checkBoxCustomAutoOpen.TickBoxReductionSize = 10;
            this.checkBoxCustomAutoOpen.UseVisualStyleBackColor = true;
            // 
            // customDateTimePickerFrom
            // 
            this.customDateTimePickerFrom.BorderColor = System.Drawing.Color.Transparent;
            this.customDateTimePickerFrom.BorderColorScaling = 0.5F;
            this.customDateTimePickerFrom.Checked = false;
            this.customDateTimePickerFrom.CustomFormat = "dd MMMM yyyy";
            this.customDateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.customDateTimePickerFrom.Location = new System.Drawing.Point(18, 40);
            this.customDateTimePickerFrom.Name = "customDateTimePickerFrom";
            this.customDateTimePickerFrom.SelectedColor = System.Drawing.Color.Yellow;
            this.customDateTimePickerFrom.ShowCheckBox = false;
            this.customDateTimePickerFrom.ShowUpDown = false;
            this.customDateTimePickerFrom.Size = new System.Drawing.Size(245, 23);
            this.customDateTimePickerFrom.TabIndex = 2;
            this.customDateTimePickerFrom.TextBackColor = System.Drawing.Color.DarkBlue;
            this.customDateTimePickerFrom.Value = new System.DateTime(2017, 9, 15, 14, 17, 6, 509);
            // 
            // customDateTimePickerTo
            // 
            this.customDateTimePickerTo.BorderColor = System.Drawing.Color.Transparent;
            this.customDateTimePickerTo.BorderColorScaling = 0.5F;
            this.customDateTimePickerTo.Checked = false;
            this.customDateTimePickerTo.CustomFormat = "dd MMMM yyyy";
            this.customDateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.customDateTimePickerTo.Location = new System.Drawing.Point(18, 73);
            this.customDateTimePickerTo.Name = "customDateTimePickerTo";
            this.customDateTimePickerTo.SelectedColor = System.Drawing.Color.Yellow;
            this.customDateTimePickerTo.ShowCheckBox = false;
            this.customDateTimePickerTo.ShowUpDown = false;
            this.customDateTimePickerTo.Size = new System.Drawing.Size(245, 23);
            this.customDateTimePickerTo.TabIndex = 3;
            this.customDateTimePickerTo.TextBackColor = System.Drawing.Color.DarkBlue;
            this.customDateTimePickerTo.Value = new System.DateTime(2017, 9, 15, 14, 17, 10, 468);
            // 
            // comboBoxCustomExportType
            // 
            this.comboBoxCustomExportType.ArrowWidth = 1;
            this.comboBoxCustomExportType.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomExportType.ButtonColorScaling = 0.5F;
            this.comboBoxCustomExportType.DataSource = null;
            this.comboBoxCustomExportType.DisplayMember = "";
            this.comboBoxCustomExportType.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomExportType.DropDownHeight = 106;
            this.comboBoxCustomExportType.DropDownWidth = 200;
            this.comboBoxCustomExportType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomExportType.ItemHeight = 13;
            this.comboBoxCustomExportType.Location = new System.Drawing.Point(18, 8);
            this.comboBoxCustomExportType.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomExportType.Name = "comboBoxCustomExportType";
            this.comboBoxCustomExportType.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomExportType.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomExportType.ScrollBarWidth = 16;
            this.comboBoxCustomExportType.SelectedIndex = -1;
            this.comboBoxCustomExportType.SelectedItem = null;
            this.comboBoxCustomExportType.SelectedValue = null;
            this.comboBoxCustomExportType.Size = new System.Drawing.Size(245, 21);
            this.comboBoxCustomExportType.TabIndex = 4;
            this.comboBoxCustomExportType.ValueMember = "";
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Location = new System.Drawing.Point(269, 0);
            this.panel_close.MarginSize = 6;
            this.panel_close.Name = "panel_close";
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 30;
            this.panel_close.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_close_MouseClick);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.BackColor = System.Drawing.SystemColors.Control;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(239, 0);
            this.panel_minimize.MarginSize = 6;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 29;
            this.panel_minimize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_minimize_MouseClick);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 3);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(37, 13);
            this.label_index.TabIndex = 28;
            this.label_index.Text = "Export";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            this.label_index.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseUp);
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.comboBoxCustomExportType);
            this.panelOuter.Controls.Add(this.panelBottom);
            this.panelOuter.Controls.Add(this.customDateTimePickerFrom);
            this.panelOuter.Controls.Add(this.customDateTimePickerTo);
            this.panelOuter.Location = new System.Drawing.Point(5, 25);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(285, 246);
            this.panelOuter.TabIndex = 31;
            // 
            // ExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 278);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panel_close);
            this.Controls.Add(this.panel_minimize);
            this.Controls.Add(this.label_index);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ExportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ExportForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ExportForm_FormClosed);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.panelOuter.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelBottom;
        private ExtendedControls.RadioButtonCustom radioButtonSemiColon;
        private ExtendedControls.RadioButtonCustom radioButtonComma;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.CheckBoxCustom checkBoxIncludeHeader;
        private ExtendedControls.CustomDateTimePicker customDateTimePickerFrom;
        private ExtendedControls.CustomDateTimePicker customDateTimePickerTo;
        private ExtendedControls.ComboBoxCustom comboBoxCustomExportType;
        private ExtendedControls.ButtonExt buttonCancel;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_index;
        private ExtendedControls.CheckBoxCustom checkBoxCustomAutoOpen;
        private ExtendedControls.ButtonExt buttonExport;
        private System.Windows.Forms.Panel panelOuter;
    }
}