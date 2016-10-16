namespace EDDiscovery
{
    partial class ExportControl
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
            this.groupBoxCustomFormat = new ExtendedControls.GroupBoxCustom();
            this.checkBoxCustomIncludeHeader = new ExtendedControls.CheckBoxCustom();
            this.labelExtSeparator = new ExtendedControls.LabelExt();
            this.radioButtonCustomEU = new ExtendedControls.RadioButtonCustom();
            this.radioButtonCustomUSAUK = new ExtendedControls.RadioButtonCustom();
            this.buttonExport = new ExtendedControls.ButtonExt();
            this.checkBoxCustomAutoOpen = new ExtendedControls.CheckBoxCustom();
            this.labelExtDataExport = new ExtendedControls.LabelExt();
            this.comboBoxCustomExportType = new ExtendedControls.ComboBoxCustom();
            this.groupBoxCustomFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxCustomFormat
            // 
            this.groupBoxCustomFormat.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCustomFormat.BackColorScaling = 0.5F;
            this.groupBoxCustomFormat.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomFormat.BorderColorScaling = 0.5F;
            this.groupBoxCustomFormat.Controls.Add(this.checkBoxCustomIncludeHeader);
            this.groupBoxCustomFormat.Controls.Add(this.labelExtSeparator);
            this.groupBoxCustomFormat.Controls.Add(this.radioButtonCustomEU);
            this.groupBoxCustomFormat.Controls.Add(this.radioButtonCustomUSAUK);
            this.groupBoxCustomFormat.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomFormat.Location = new System.Drawing.Point(3, 3);
            this.groupBoxCustomFormat.Name = "groupBoxCustomFormat";
            this.groupBoxCustomFormat.Size = new System.Drawing.Size(457, 112);
            this.groupBoxCustomFormat.TabIndex = 0;
            this.groupBoxCustomFormat.TabStop = false;
            this.groupBoxCustomFormat.Text = "CSV format";
            this.groupBoxCustomFormat.TextPadding = 0;
            this.groupBoxCustomFormat.TextStartPosition = -1;
            // 
            // checkBoxCustomIncludeHeader
            // 
            this.checkBoxCustomIncludeHeader.AutoSize = true;
            this.checkBoxCustomIncludeHeader.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomIncludeHeader.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomIncludeHeader.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomIncludeHeader.Checked = true;
            this.checkBoxCustomIncludeHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCustomIncludeHeader.FontNerfReduction = 0.5F;
            this.checkBoxCustomIncludeHeader.Location = new System.Drawing.Point(139, 15);
            this.checkBoxCustomIncludeHeader.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomIncludeHeader.Name = "checkBoxCustomIncludeHeader";
            this.checkBoxCustomIncludeHeader.Size = new System.Drawing.Size(97, 17);
            this.checkBoxCustomIncludeHeader.TabIndex = 4;
            this.checkBoxCustomIncludeHeader.Text = "Include header";
            this.checkBoxCustomIncludeHeader.TickBoxReductionSize = 10;
            this.checkBoxCustomIncludeHeader.UseVisualStyleBackColor = true;
            // 
            // labelExtSeparator
            // 
            this.labelExtSeparator.AutoSize = true;
            this.labelExtSeparator.Location = new System.Drawing.Point(11, 16);
            this.labelExtSeparator.Name = "labelExtSeparator";
            this.labelExtSeparator.Size = new System.Drawing.Size(77, 13);
            this.labelExtSeparator.TabIndex = 3;
            this.labelExtSeparator.Text = "CSV Separator";
            // 
            // radioButtonCustomEU
            // 
            this.radioButtonCustomEU.AutoSize = true;
            this.radioButtonCustomEU.FontNerfReduction = 0.5F;
            this.radioButtonCustomEU.Location = new System.Drawing.Point(6, 64);
            this.radioButtonCustomEU.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonCustomEU.Name = "radioButtonCustomEU";
            this.radioButtonCustomEU.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonCustomEU.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonCustomEU.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonCustomEU.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButtonCustomEU.Size = new System.Drawing.Size(55, 17);
            this.radioButtonCustomEU.TabIndex = 2;
            this.radioButtonCustomEU.Text = ";  (EU)";
            this.radioButtonCustomEU.UseVisualStyleBackColor = true;
            // 
            // radioButtonCustomUSAUK
            // 
            this.radioButtonCustomUSAUK.AutoSize = true;
            this.radioButtonCustomUSAUK.Checked = true;
            this.radioButtonCustomUSAUK.FontNerfReduction = 0.5F;
            this.radioButtonCustomUSAUK.Location = new System.Drawing.Point(6, 41);
            this.radioButtonCustomUSAUK.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.radioButtonCustomUSAUK.Name = "radioButtonCustomUSAUK";
            this.radioButtonCustomUSAUK.RadioButtonColor = System.Drawing.Color.Gray;
            this.radioButtonCustomUSAUK.RadioButtonInnerColor = System.Drawing.Color.White;
            this.radioButtonCustomUSAUK.SelectedColor = System.Drawing.Color.DarkBlue;
            this.radioButtonCustomUSAUK.SelectedColorRing = System.Drawing.Color.Black;
            this.radioButtonCustomUSAUK.Size = new System.Drawing.Size(85, 17);
            this.radioButtonCustomUSAUK.TabIndex = 1;
            this.radioButtonCustomUSAUK.TabStop = true;
            this.radioButtonCustomUSAUK.Text = ",   (USA/UK)";
            this.radioButtonCustomUSAUK.UseVisualStyleBackColor = true;
            // 
            // buttonExport
            // 
            this.buttonExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExport.BorderColorScaling = 1.25F;
            this.buttonExport.ButtonColorScaling = 0.5F;
            this.buttonExport.ButtonDisabledScaling = 0.5F;
            this.buttonExport.Location = new System.Drawing.Point(466, 11);
            this.buttonExport.Name = "buttonExport";
            this.buttonExport.Size = new System.Drawing.Size(90, 24);
            this.buttonExport.TabIndex = 1;
            this.buttonExport.Text = "Export";
            this.buttonExport.UseVisualStyleBackColor = true;
            this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
            // 
            // checkBoxCustomAutoOpen
            // 
            this.checkBoxCustomAutoOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxCustomAutoOpen.AutoSize = true;
            this.checkBoxCustomAutoOpen.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomAutoOpen.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomAutoOpen.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomAutoOpen.Checked = true;
            this.checkBoxCustomAutoOpen.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxCustomAutoOpen.FontNerfReduction = 0.5F;
            this.checkBoxCustomAutoOpen.Location = new System.Drawing.Point(466, 44);
            this.checkBoxCustomAutoOpen.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomAutoOpen.Name = "checkBoxCustomAutoOpen";
            this.checkBoxCustomAutoOpen.Size = new System.Drawing.Size(75, 17);
            this.checkBoxCustomAutoOpen.TabIndex = 5;
            this.checkBoxCustomAutoOpen.Text = "Auto open";
            this.checkBoxCustomAutoOpen.TickBoxReductionSize = 10;
            this.checkBoxCustomAutoOpen.UseVisualStyleBackColor = true;
            // 
            // labelExtDataExport
            // 
            this.labelExtDataExport.AutoSize = true;
            this.labelExtDataExport.Location = new System.Drawing.Point(6, 127);
            this.labelExtDataExport.Name = "labelExtDataExport";
            this.labelExtDataExport.Size = new System.Drawing.Size(75, 13);
            this.labelExtDataExport.TabIndex = 5;
            this.labelExtDataExport.Text = "Data to Export";
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
            this.comboBoxCustomExportType.DropDownWidth = 151;
            this.comboBoxCustomExportType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomExportType.ItemHeight = 13;
            this.comboBoxCustomExportType.Location = new System.Drawing.Point(9, 152);
            this.comboBoxCustomExportType.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomExportType.Name = "comboBoxCustomExportType";
            this.comboBoxCustomExportType.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomExportType.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomExportType.ScrollBarWidth = 16;
            this.comboBoxCustomExportType.SelectedIndex = -1;
            this.comboBoxCustomExportType.SelectedItem = null;
            this.comboBoxCustomExportType.SelectedValue = null;
            this.comboBoxCustomExportType.Size = new System.Drawing.Size(151, 19);
            this.comboBoxCustomExportType.TabIndex = 6;
            this.comboBoxCustomExportType.Text = "comboBoxCustom1";
            this.comboBoxCustomExportType.ValueMember = "";
            // 
            // ExportControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.comboBoxCustomExportType);
            this.Controls.Add(this.labelExtDataExport);
            this.Controls.Add(this.checkBoxCustomAutoOpen);
            this.Controls.Add(this.buttonExport);
            this.Controls.Add(this.groupBoxCustomFormat);
            this.Name = "ExportControl";
            this.Size = new System.Drawing.Size(571, 356);
            this.Load += new System.EventHandler(this.ExportControl_Load);
            this.groupBoxCustomFormat.ResumeLayout(false);
            this.groupBoxCustomFormat.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.GroupBoxCustom groupBoxCustomFormat;
        private ExtendedControls.RadioButtonCustom radioButtonCustomEU;
        private ExtendedControls.RadioButtonCustom radioButtonCustomUSAUK;
        private ExtendedControls.CheckBoxCustom checkBoxCustomIncludeHeader;
        private ExtendedControls.LabelExt labelExtSeparator;
        private ExtendedControls.ButtonExt buttonExport;
        private ExtendedControls.CheckBoxCustom checkBoxCustomAutoOpen;
        private ExtendedControls.LabelExt labelExtDataExport;
        private ExtendedControls.ComboBoxCustom comboBoxCustomExportType;
    }
}
