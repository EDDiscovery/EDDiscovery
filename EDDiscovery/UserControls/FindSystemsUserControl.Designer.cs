namespace EDDiscovery.UserControls
{
    partial class FindSystemsUserControl
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelFilter = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonExtDBLookup = new ExtendedControls.ButtonExt();
            this.buttonExtEDSMSphere = new ExtendedControls.ButtonExt();
            this.buttonExtExcel = new ExtendedControls.ButtonExt();
            this.numberBoxMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.numberBoxMinRadius = new ExtendedControls.NumberBoxDouble();
            this.textBoxSystemName = new ExtendedControls.AutoCompleteTextBox();
            this.buttonExtDBSphere = new ExtendedControls.ButtonExt();
            this.buttonExtDBCube = new ExtendedControls.ButtonExt();
            this.buttonExtEDSMCube = new ExtendedControls.ButtonExt();
            this.numberBoxDoubleX = new ExtendedControls.NumberBoxDouble();
            this.numberBoxDoubleY = new ExtendedControls.NumberBoxDouble();
            this.numberBoxDoubleZ = new ExtendedControls.NumberBoxDouble();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 34;
            this.label1.Text = "Radius ly Min";
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(19, 11);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(57, 13);
            this.labelFilter.TabIndex = 35;
            this.labelFilter.Text = "Star Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(49, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(27, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Max";
            // 
            // buttonExtDBLookup
            // 
            this.buttonExtDBLookup.Location = new System.Drawing.Point(288, 3);
            this.buttonExtDBLookup.Name = "buttonExtDBLookup";
            this.buttonExtDBLookup.Size = new System.Drawing.Size(176, 23);
            this.buttonExtDBLookup.TabIndex = 37;
            this.buttonExtDBLookup.Text = "From DB Find Names";
            this.buttonExtDBLookup.UseVisualStyleBackColor = true;
            this.buttonExtDBLookup.Click += new System.EventHandler(this.buttonExtDBLookup_Click);
            // 
            // buttonExtEDSMSphere
            // 
            this.buttonExtEDSMSphere.Location = new System.Drawing.Point(168, 62);
            this.buttonExtEDSMSphere.Name = "buttonExtEDSMSphere";
            this.buttonExtEDSMSphere.Size = new System.Drawing.Size(176, 23);
            this.buttonExtEDSMSphere.TabIndex = 38;
            this.buttonExtEDSMSphere.Text = "From EDSM Sphere Systems";
            this.buttonExtEDSMSphere.UseVisualStyleBackColor = true;
            this.buttonExtEDSMSphere.Click += new System.EventHandler(this.buttonExtEDSMSphere_Click);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.TravelGrid_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(502, 5);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(24, 24);
            this.buttonExtExcel.TabIndex = 36;
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // numberBoxMaxRadius
            // 
            this.numberBoxMaxRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxMaxRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxMaxRadius.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxMaxRadius.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxMaxRadius.BorderColorScaling = 0.5F;
            this.numberBoxMaxRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxMaxRadius.ClearOnFirstChar = false;
            this.numberBoxMaxRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxMaxRadius.DelayBeforeNotification = 0;
            this.numberBoxMaxRadius.Format = "0.##";
            this.numberBoxMaxRadius.InErrorCondition = false;
            this.numberBoxMaxRadius.Location = new System.Drawing.Point(95, 88);
            this.numberBoxMaxRadius.Maximum = 100000D;
            this.numberBoxMaxRadius.Minimum = 0D;
            this.numberBoxMaxRadius.Multiline = false;
            this.numberBoxMaxRadius.Name = "numberBoxMaxRadius";
            this.numberBoxMaxRadius.ReadOnly = false;
            this.numberBoxMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxMaxRadius.SelectionLength = 0;
            this.numberBoxMaxRadius.SelectionStart = 0;
            this.numberBoxMaxRadius.Size = new System.Drawing.Size(48, 20);
            this.numberBoxMaxRadius.TabIndex = 32;
            this.numberBoxMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxMaxRadius.Value = 0D;
            this.numberBoxMaxRadius.WordWrap = true;
            // 
            // numberBoxMinRadius
            // 
            this.numberBoxMinRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxMinRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxMinRadius.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxMinRadius.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxMinRadius.BorderColorScaling = 0.5F;
            this.numberBoxMinRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxMinRadius.ClearOnFirstChar = false;
            this.numberBoxMinRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxMinRadius.DelayBeforeNotification = 0;
            this.numberBoxMinRadius.Format = "0.##";
            this.numberBoxMinRadius.InErrorCondition = false;
            this.numberBoxMinRadius.Location = new System.Drawing.Point(95, 62);
            this.numberBoxMinRadius.Maximum = 100000D;
            this.numberBoxMinRadius.Minimum = 0D;
            this.numberBoxMinRadius.Multiline = false;
            this.numberBoxMinRadius.Name = "numberBoxMinRadius";
            this.numberBoxMinRadius.ReadOnly = false;
            this.numberBoxMinRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxMinRadius.SelectionLength = 0;
            this.numberBoxMinRadius.SelectionStart = 0;
            this.numberBoxMinRadius.Size = new System.Drawing.Size(48, 20);
            this.numberBoxMinRadius.TabIndex = 32;
            this.numberBoxMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxMinRadius.Value = 0D;
            this.numberBoxMinRadius.WordWrap = true;
            // 
            // textBoxSystemName
            // 
            this.textBoxSystemName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxSystemName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxSystemName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxSystemName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSystemName.BorderColorScaling = 0.5F;
            this.textBoxSystemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSystemName.ClearOnFirstChar = false;
            this.textBoxSystemName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSystemName.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.textBoxSystemName.DropDownBorderColor = System.Drawing.Color.Green;
            this.textBoxSystemName.DropDownHeight = 200;
            this.textBoxSystemName.DropDownItemHeight = 20;
            this.textBoxSystemName.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.textBoxSystemName.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.textBoxSystemName.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.textBoxSystemName.DropDownWidth = 0;
            this.textBoxSystemName.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxSystemName.InErrorCondition = false;
            this.textBoxSystemName.Location = new System.Drawing.Point(95, 6);
            this.textBoxSystemName.Multiline = false;
            this.textBoxSystemName.Name = "textBoxSystemName";
            this.textBoxSystemName.ReadOnly = false;
            this.textBoxSystemName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSystemName.SelectionLength = 0;
            this.textBoxSystemName.SelectionStart = 0;
            this.textBoxSystemName.Size = new System.Drawing.Size(166, 20);
            this.textBoxSystemName.TabIndex = 33;
            this.textBoxSystemName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxSystemName.WordWrap = true;
            // 
            // buttonExtDBSphere
            // 
            this.buttonExtDBSphere.Location = new System.Drawing.Point(168, 88);
            this.buttonExtDBSphere.Name = "buttonExtDBSphere";
            this.buttonExtDBSphere.Size = new System.Drawing.Size(176, 23);
            this.buttonExtDBSphere.TabIndex = 38;
            this.buttonExtDBSphere.Text = "From DB Sphere Systems";
            this.buttonExtDBSphere.UseVisualStyleBackColor = true;
            this.buttonExtDBSphere.Click += new System.EventHandler(this.buttonExtDBSphere_Click);
            // 
            // buttonExtDBCube
            // 
            this.buttonExtDBCube.Location = new System.Drawing.Point(350, 88);
            this.buttonExtDBCube.Name = "buttonExtDBCube";
            this.buttonExtDBCube.Size = new System.Drawing.Size(176, 23);
            this.buttonExtDBCube.TabIndex = 38;
            this.buttonExtDBCube.Text = "From DB Cube Systems";
            this.buttonExtDBCube.UseVisualStyleBackColor = true;
            this.buttonExtDBCube.Click += new System.EventHandler(this.buttonExtDBCube_Click);
            // 
            // buttonExtEDSMCube
            // 
            this.buttonExtEDSMCube.Location = new System.Drawing.Point(350, 62);
            this.buttonExtEDSMCube.Name = "buttonExtEDSMCube";
            this.buttonExtEDSMCube.Size = new System.Drawing.Size(176, 23);
            this.buttonExtEDSMCube.TabIndex = 38;
            this.buttonExtEDSMCube.Text = "From EDSM Cube Systems";
            this.buttonExtEDSMCube.UseVisualStyleBackColor = true;
            this.buttonExtEDSMCube.Click += new System.EventHandler(this.buttonExtEDSMCube_Click);
            // 
            // numberBoxDoubleX
            // 
            this.numberBoxDoubleX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxDoubleX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxDoubleX.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxDoubleX.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxDoubleX.BorderColorScaling = 0.5F;
            this.numberBoxDoubleX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxDoubleX.ClearOnFirstChar = false;
            this.numberBoxDoubleX.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxDoubleX.DelayBeforeNotification = 0;
            this.numberBoxDoubleX.Format = "0.##";
            this.numberBoxDoubleX.InErrorCondition = false;
            this.numberBoxDoubleX.Location = new System.Drawing.Point(95, 32);
            this.numberBoxDoubleX.Maximum = 40000D;
            this.numberBoxDoubleX.Minimum = -40000D;
            this.numberBoxDoubleX.Multiline = false;
            this.numberBoxDoubleX.Name = "numberBoxDoubleX";
            this.numberBoxDoubleX.ReadOnly = false;
            this.numberBoxDoubleX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxDoubleX.SelectionLength = 0;
            this.numberBoxDoubleX.SelectionStart = 0;
            this.numberBoxDoubleX.Size = new System.Drawing.Size(80, 20);
            this.numberBoxDoubleX.TabIndex = 32;
            this.numberBoxDoubleX.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxDoubleX.Value = 0D;
            this.numberBoxDoubleX.WordWrap = true;
            this.numberBoxDoubleX.Enter += new System.EventHandler(this.numberBoxDoubleXYZ_Enter);
            // 
            // numberBoxDoubleY
            // 
            this.numberBoxDoubleY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxDoubleY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxDoubleY.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxDoubleY.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxDoubleY.BorderColorScaling = 0.5F;
            this.numberBoxDoubleY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxDoubleY.ClearOnFirstChar = false;
            this.numberBoxDoubleY.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxDoubleY.DelayBeforeNotification = 0;
            this.numberBoxDoubleY.Format = "0.##";
            this.numberBoxDoubleY.InErrorCondition = false;
            this.numberBoxDoubleY.Location = new System.Drawing.Point(218, 32);
            this.numberBoxDoubleY.Maximum = 5000D;
            this.numberBoxDoubleY.Minimum = -5000D;
            this.numberBoxDoubleY.Multiline = false;
            this.numberBoxDoubleY.Name = "numberBoxDoubleY";
            this.numberBoxDoubleY.ReadOnly = false;
            this.numberBoxDoubleY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxDoubleY.SelectionLength = 0;
            this.numberBoxDoubleY.SelectionStart = 0;
            this.numberBoxDoubleY.Size = new System.Drawing.Size(80, 20);
            this.numberBoxDoubleY.TabIndex = 32;
            this.numberBoxDoubleY.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxDoubleY.Value = 0D;
            this.numberBoxDoubleY.WordWrap = true;
            this.numberBoxDoubleY.Enter += new System.EventHandler(this.numberBoxDoubleXYZ_Enter);
            // 
            // numberBoxDoubleZ
            // 
            this.numberBoxDoubleZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxDoubleZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxDoubleZ.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxDoubleZ.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxDoubleZ.BorderColorScaling = 0.5F;
            this.numberBoxDoubleZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxDoubleZ.ClearOnFirstChar = false;
            this.numberBoxDoubleZ.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxDoubleZ.DelayBeforeNotification = 0;
            this.numberBoxDoubleZ.Format = "0.##";
            this.numberBoxDoubleZ.InErrorCondition = false;
            this.numberBoxDoubleZ.Location = new System.Drawing.Point(350, 32);
            this.numberBoxDoubleZ.Maximum = 80000D;
            this.numberBoxDoubleZ.Minimum = -20000D;
            this.numberBoxDoubleZ.Multiline = false;
            this.numberBoxDoubleZ.Name = "numberBoxDoubleZ";
            this.numberBoxDoubleZ.ReadOnly = false;
            this.numberBoxDoubleZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxDoubleZ.SelectionLength = 0;
            this.numberBoxDoubleZ.SelectionStart = 0;
            this.numberBoxDoubleZ.Size = new System.Drawing.Size(80, 20);
            this.numberBoxDoubleZ.TabIndex = 32;
            this.numberBoxDoubleZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxDoubleZ.Value = 0D;
            this.numberBoxDoubleZ.WordWrap = true;
            this.numberBoxDoubleZ.Enter += new System.EventHandler(this.numberBoxDoubleXYZ_Enter);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Or X";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(198, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 35;
            this.label4.Text = "Y";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(330, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "Z";
            // 
            // FindSystemsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.buttonExtDBLookup);
            this.Controls.Add(this.buttonExtDBCube);
            this.Controls.Add(this.buttonExtDBSphere);
            this.Controls.Add(this.buttonExtEDSMCube);
            this.Controls.Add(this.buttonExtEDSMSphere);
            this.Controls.Add(this.buttonExtExcel);
            this.Controls.Add(this.numberBoxMaxRadius);
            this.Controls.Add(this.numberBoxDoubleZ);
            this.Controls.Add(this.numberBoxDoubleY);
            this.Controls.Add(this.numberBoxDoubleX);
            this.Controls.Add(this.numberBoxMinRadius);
            this.Controls.Add(this.textBoxSystemName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelFilter);
            this.Name = "FindSystemsUserControl";
            this.Size = new System.Drawing.Size(664, 241);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ButtonExt buttonExtDBLookup;
        private ExtendedControls.ButtonExt buttonExtEDSMSphere;
        private ExtendedControls.ButtonExt buttonExtExcel;
        private ExtendedControls.AutoCompleteTextBox textBoxSystemName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.Label label2;
        private ExtendedControls.NumberBoxDouble numberBoxMinRadius;
        private ExtendedControls.NumberBoxDouble numberBoxMaxRadius;
        private ExtendedControls.ButtonExt buttonExtDBSphere;
        private ExtendedControls.ButtonExt buttonExtDBCube;
        private ExtendedControls.ButtonExt buttonExtEDSMCube;
        private ExtendedControls.NumberBoxDouble numberBoxDoubleX;
        private ExtendedControls.NumberBoxDouble numberBoxDoubleY;
        private ExtendedControls.NumberBoxDouble numberBoxDoubleZ;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}
