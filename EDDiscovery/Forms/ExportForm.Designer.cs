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
            this.panelBottom = new System.Windows.Forms.Panel();
            this.buttonCancel = new ExtendedControls.ExtButton();
            this.buttonExport = new ExtendedControls.ExtButton();
            this.labelCVSSep = new System.Windows.Forms.Label();
            this.radioButtonSemiColon = new ExtendedControls.ExtRadioButton();
            this.radioButtonComma = new ExtendedControls.ExtRadioButton();
            this.checkBoxIncludeHeader = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomAutoOpen = new ExtendedControls.ExtCheckBox();
            this.customDateTimePickerFrom = new ExtendedControls.ExtDateTimePicker();
            this.customDateTimePickerTo = new ExtendedControls.ExtDateTimePicker();
            this.comboBoxSelectedType = new ExtendedControls.ExtComboBox();
            this.panel_close = new ExtendedControls.ExtButtonDrawn();
            this.panel_minimize = new ExtendedControls.ExtButtonDrawn();
            this.label_index = new System.Windows.Forms.Label();
            this.labelUTCEnd = new System.Windows.Forms.Label();
            this.labelUTCStart = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panelCombo = new System.Windows.Forms.Panel();
            this.panelDate = new System.Windows.Forms.Panel();
            this.panelIncludeOpen = new System.Windows.Forms.Panel();
            this.panelCSV = new System.Windows.Forms.Panel();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelBottom.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelCombo.SuspendLayout();
            this.panelDate.SuspendLayout();
            this.panelIncludeOpen.SuspendLayout();
            this.panelCSV.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.buttonCancel);
            this.panelBottom.Controls.Add(this.buttonExport);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 204);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(329, 39);
            this.panelBottom.TabIndex = 0;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(111, 11);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "%Cancel%";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.Location = new System.Drawing.Point(217, 11);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(100, 23);
            this.buttonExport.TabIndex = 5;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // labelCVSSep
            // 
            this.labelCVSSep.AutoSize = true;
            this.labelCVSSep.Location = new System.Drawing.Point(5, 10);
            this.labelCVSSep.Name = "labelCVSSep";
            this.labelCVSSep.Size = new System.Drawing.Size(77, 13);
            this.labelCVSSep.TabIndex = 2;
            this.labelCVSSep.Text = "CSV Separator";
            // 
            // radioButtonSemiColon
            // 
            this.radioButtonSemiColon.AutoSize = true;
            this.radioButtonSemiColon.Location = new System.Drawing.Point(180, 31);
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
            // radioButtonComma
            // 
            this.radioButtonComma.AutoSize = true;
            this.radioButtonComma.Location = new System.Drawing.Point(180, 8);
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
            this.checkBoxIncludeHeader.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxIncludeHeader.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxIncludeHeader.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxIncludeHeader.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxIncludeHeader.ImageIndeterminate = null;
            this.checkBoxIncludeHeader.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxIncludeHeader.ImageUnchecked = null;
            this.checkBoxIncludeHeader.Location = new System.Drawing.Point(8, 7);
            this.checkBoxIncludeHeader.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxIncludeHeader.Name = "checkBoxIncludeHeader";
            this.checkBoxIncludeHeader.Size = new System.Drawing.Size(99, 17);
            this.checkBoxIncludeHeader.TabIndex = 1;
            this.checkBoxIncludeHeader.Text = "Include Header";
            this.checkBoxIncludeHeader.TickBoxReductionRatio = 0.75F;
            this.checkBoxIncludeHeader.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomAutoOpen
            // 
            this.checkBoxCustomAutoOpen.AutoSize = true;
            this.checkBoxCustomAutoOpen.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomAutoOpen.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomAutoOpen.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomAutoOpen.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomAutoOpen.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomAutoOpen.ImageIndeterminate = null;
            this.checkBoxCustomAutoOpen.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomAutoOpen.ImageUnchecked = null;
            this.checkBoxCustomAutoOpen.Location = new System.Drawing.Point(180, 7);
            this.checkBoxCustomAutoOpen.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomAutoOpen.Name = "checkBoxCustomAutoOpen";
            this.checkBoxCustomAutoOpen.Size = new System.Drawing.Size(52, 17);
            this.checkBoxCustomAutoOpen.TabIndex = 1;
            this.checkBoxCustomAutoOpen.Text = "Open";
            this.checkBoxCustomAutoOpen.TickBoxReductionRatio = 0.75F;
            this.checkBoxCustomAutoOpen.UseVisualStyleBackColor = true;
            // 
            // customDateTimePickerFrom
            // 
            this.customDateTimePickerFrom.BorderColor = System.Drawing.Color.Transparent;
            this.customDateTimePickerFrom.BorderColorScaling = 0.5F;
            this.customDateTimePickerFrom.Checked = false;
            this.customDateTimePickerFrom.CustomFormat = "dd MMMM yyyy    HH:mm:ss";
            this.customDateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.customDateTimePickerFrom.Location = new System.Drawing.Point(3, 12);
            this.customDateTimePickerFrom.Name = "customDateTimePickerFrom";
            this.customDateTimePickerFrom.SelectedColor = System.Drawing.Color.Yellow;
            this.customDateTimePickerFrom.ShowCheckBox = false;
            this.customDateTimePickerFrom.ShowUpDown = false;
            this.customDateTimePickerFrom.Size = new System.Drawing.Size(270, 23);
            this.customDateTimePickerFrom.TabIndex = 2;
            this.customDateTimePickerFrom.TextBackColor = System.Drawing.Color.DarkBlue;
            this.customDateTimePickerFrom.Value = new System.DateTime(2017, 9, 15, 14, 17, 6, 509);
            // 
            // customDateTimePickerTo
            // 
            this.customDateTimePickerTo.BorderColor = System.Drawing.Color.Transparent;
            this.customDateTimePickerTo.BorderColorScaling = 0.5F;
            this.customDateTimePickerTo.Checked = false;
            this.customDateTimePickerTo.CustomFormat = "dd MMMM yyyy    HH:mm:ss";
            this.customDateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.customDateTimePickerTo.Location = new System.Drawing.Point(3, 45);
            this.customDateTimePickerTo.Name = "customDateTimePickerTo";
            this.customDateTimePickerTo.SelectedColor = System.Drawing.Color.Yellow;
            this.customDateTimePickerTo.ShowCheckBox = false;
            this.customDateTimePickerTo.ShowUpDown = false;
            this.customDateTimePickerTo.Size = new System.Drawing.Size(270, 23);
            this.customDateTimePickerTo.TabIndex = 3;
            this.customDateTimePickerTo.TextBackColor = System.Drawing.Color.DarkBlue;
            this.customDateTimePickerTo.Value = new System.DateTime(2017, 9, 15, 14, 17, 10, 468);
            // 
            // comboBoxSelectedType
            // 
            this.comboBoxSelectedType.BorderColor = System.Drawing.Color.White;
            this.comboBoxSelectedType.ButtonColorScaling = 0.5F;
            this.comboBoxSelectedType.DataSource = null;
            this.comboBoxSelectedType.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxSelectedType.DisplayMember = "";
            this.comboBoxSelectedType.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxSelectedType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSelectedType.Location = new System.Drawing.Point(3, 3);
            this.comboBoxSelectedType.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxSelectedType.Name = "comboBoxSelectedType";
            this.comboBoxSelectedType.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxSelectedType.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxSelectedType.SelectedIndex = -1;
            this.comboBoxSelectedType.SelectedItem = null;
            this.comboBoxSelectedType.SelectedValue = null;
            this.comboBoxSelectedType.Size = new System.Drawing.Size(270, 21);
            this.comboBoxSelectedType.TabIndex = 4;
            this.comboBoxSelectedType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxSelectedType.ValueMember = "";
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(304, 2);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.PanelDisabledScaling = 0.25F;
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 30;
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
            this.panel_minimize.Location = new System.Drawing.Point(280, 2);
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.PanelDisabledScaling = 0.25F;
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 29;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_minimize.UseMnemonic = true;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 3);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(43, 13);
            this.label_index.TabIndex = 28;
            this.label_index.Text = "<code>";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            this.label_index.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseUp);
            // 
            // labelUTCEnd
            // 
            this.labelUTCEnd.AutoSize = true;
            this.labelUTCEnd.Location = new System.Drawing.Point(282, 55);
            this.labelUTCEnd.Name = "labelUTCEnd";
            this.labelUTCEnd.Size = new System.Drawing.Size(29, 13);
            this.labelUTCEnd.TabIndex = 5;
            this.labelUTCEnd.Text = "UTC";
            // 
            // labelUTCStart
            // 
            this.labelUTCStart.AutoSize = true;
            this.labelUTCStart.Location = new System.Drawing.Point(282, 22);
            this.labelUTCStart.Name = "labelUTCStart";
            this.labelUTCStart.Size = new System.Drawing.Size(29, 13);
            this.labelUTCStart.TabIndex = 5;
            this.labelUTCStart.Text = "UTC";
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(331, 32);
            this.panelTop.TabIndex = 32;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseUp);
            // 
            // panelCombo
            // 
            this.panelCombo.Controls.Add(this.comboBoxSelectedType);
            this.panelCombo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCombo.Location = new System.Drawing.Point(0, 0);
            this.panelCombo.Name = "panelCombo";
            this.panelCombo.Size = new System.Drawing.Size(329, 34);
            this.panelCombo.TabIndex = 7;
            // 
            // panelDate
            // 
            this.panelDate.Controls.Add(this.customDateTimePickerFrom);
            this.panelDate.Controls.Add(this.labelUTCEnd);
            this.panelDate.Controls.Add(this.customDateTimePickerTo);
            this.panelDate.Controls.Add(this.labelUTCStart);
            this.panelDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDate.Location = new System.Drawing.Point(0, 34);
            this.panelDate.Name = "panelDate";
            this.panelDate.Size = new System.Drawing.Size(329, 83);
            this.panelDate.TabIndex = 5;
            // 
            // panelIncludeOpen
            // 
            this.panelIncludeOpen.Controls.Add(this.checkBoxIncludeHeader);
            this.panelIncludeOpen.Controls.Add(this.checkBoxCustomAutoOpen);
            this.panelIncludeOpen.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelIncludeOpen.Location = new System.Drawing.Point(0, 117);
            this.panelIncludeOpen.Name = "panelIncludeOpen";
            this.panelIncludeOpen.Size = new System.Drawing.Size(329, 31);
            this.panelIncludeOpen.TabIndex = 8;
            // 
            // panelCSV
            // 
            this.panelCSV.Controls.Add(this.labelCVSSep);
            this.panelCSV.Controls.Add(this.radioButtonComma);
            this.panelCSV.Controls.Add(this.radioButtonSemiColon);
            this.panelCSV.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCSV.Location = new System.Drawing.Point(0, 148);
            this.panelCSV.Name = "panelCSV";
            this.panelCSV.Size = new System.Drawing.Size(329, 52);
            this.panelCSV.TabIndex = 9;
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.panelCSV);
            this.panelOuter.Controls.Add(this.panelIncludeOpen);
            this.panelOuter.Controls.Add(this.panelDate);
            this.panelOuter.Controls.Add(this.panelCombo);
            this.panelOuter.Controls.Add(this.panelBottom);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 32);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(331, 245);
            this.panelOuter.TabIndex = 5;
            // 
            // ExportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 277);
            this.Controls.Add(this.panelOuter);
            this.Controls.Add(this.panelTop);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "ExportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export data";
            this.panelBottom.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelCombo.ResumeLayout(false);
            this.panelDate.ResumeLayout(false);
            this.panelDate.PerformLayout();
            this.panelIncludeOpen.ResumeLayout(false);
            this.panelIncludeOpen.PerformLayout();
            this.panelCSV.ResumeLayout(false);
            this.panelCSV.PerformLayout();
            this.panelOuter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBottom;
        private ExtendedControls.ExtRadioButton radioButtonSemiColon;
        private ExtendedControls.ExtRadioButton radioButtonComma;
        private System.Windows.Forms.Label labelCVSSep;
        private ExtendedControls.ExtCheckBox checkBoxIncludeHeader;
        private ExtendedControls.ExtDateTimePicker customDateTimePickerFrom;
        private ExtendedControls.ExtDateTimePicker customDateTimePickerTo;
        private ExtendedControls.ExtComboBox comboBoxSelectedType;
        private ExtendedControls.ExtButton buttonCancel;
        private ExtendedControls.ExtButtonDrawn panel_close;
        private ExtendedControls.ExtButtonDrawn panel_minimize;
        private System.Windows.Forms.Label label_index;
        private ExtendedControls.ExtCheckBox checkBoxCustomAutoOpen;
        private ExtendedControls.ExtButton buttonExport;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelUTCEnd;
        private System.Windows.Forms.Label labelUTCStart;
        private System.Windows.Forms.Panel panelCombo;
        private System.Windows.Forms.Panel panelDate;
        private System.Windows.Forms.Panel panelIncludeOpen;
        private System.Windows.Forms.Panel panelCSV;
        private System.Windows.Forms.Panel panelOuter;
    }
}