namespace EDDiscovery.Forms
{
    partial class ImportSphere
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
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.txtsphereRadius = new ExtendedControls.TextBoxBorder();
            this.labelExt8 = new ExtendedControls.LabelExt();
            this.txtExportVisited = new ExtendedControls.AutoCompleteTextBox();
            this.btnExportTravel = new ExtendedControls.ButtonExt();
            this.labelExt9 = new ExtendedControls.LabelExt();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(218, 61);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 22;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(299, 61);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 21;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // txtsphereRadius
            // 
            this.txtsphereRadius.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtsphereRadius.BorderColor = System.Drawing.Color.Transparent;
            this.txtsphereRadius.BorderColorScaling = 0.5F;
            this.txtsphereRadius.Location = new System.Drawing.Point(47, 35);
            this.txtsphereRadius.Name = "txtsphereRadius";
            this.txtsphereRadius.Size = new System.Drawing.Size(54, 20);
            this.txtsphereRadius.TabIndex = 19;
            // 
            // labelExt8
            // 
            this.labelExt8.AutoSize = true;
            this.labelExt8.Location = new System.Drawing.Point(1, 38);
            this.labelExt8.Name = "labelExt8";
            this.labelExt8.Size = new System.Drawing.Size(40, 13);
            this.labelExt8.TabIndex = 20;
            this.labelExt8.Text = "Radius";
            // 
            // txtExportVisited
            // 
            this.txtExportVisited.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExportVisited.BorderColor = System.Drawing.Color.Transparent;
            this.txtExportVisited.BorderColorScaling = 0.5F;
            this.txtExportVisited.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.txtExportVisited.DropDownBorderColor = System.Drawing.Color.Green;
            this.txtExportVisited.DropDownHeight = 200;
            this.txtExportVisited.DropDownItemHeight = 20;
            this.txtExportVisited.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.txtExportVisited.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.txtExportVisited.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.txtExportVisited.DropDownWidth = 0;
            this.txtExportVisited.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.txtExportVisited.Location = new System.Drawing.Point(47, 9);
            this.txtExportVisited.Name = "txtExportVisited";
            this.txtExportVisited.Size = new System.Drawing.Size(210, 20);
            this.txtExportVisited.TabIndex = 16;
            // 
            // btnExportTravel
            // 
            this.btnExportTravel.BorderColorScaling = 1.25F;
            this.btnExportTravel.ButtonColorScaling = 0.5F;
            this.btnExportTravel.ButtonDisabledScaling = 0.5F;
            this.btnExportTravel.Location = new System.Drawing.Point(263, 7);
            this.btnExportTravel.Name = "btnExportTravel";
            this.btnExportTravel.Size = new System.Drawing.Size(111, 23);
            this.btnExportTravel.TabIndex = 17;
            this.btnExportTravel.Text = "Current system";
            this.btnExportTravel.UseVisualStyleBackColor = true;
            this.btnExportTravel.Click += new System.EventHandler(this.btnExportTravel_Click);
            // 
            // labelExt9
            // 
            this.labelExt9.AutoSize = true;
            this.labelExt9.Location = new System.Drawing.Point(1, 12);
            this.labelExt9.Name = "labelExt9";
            this.labelExt9.Size = new System.Drawing.Size(41, 13);
            this.labelExt9.TabIndex = 18;
            this.labelExt9.Text = "System";
            // 
            // ImportSphere
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 92);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.txtsphereRadius);
            this.Controls.Add(this.labelExt8);
            this.Controls.Add(this.txtExportVisited);
            this.Controls.Add(this.btnExportTravel);
            this.Controls.Add(this.labelExt9);
            this.Name = "ImportSphere";
            this.Text = "Import sphere systems";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.AutoCompleteTextBox txtExportVisited;
        private ExtendedControls.ButtonExt btnExportTravel;
        private ExtendedControls.LabelExt labelExt9;
        private ExtendedControls.TextBoxBorder txtsphereRadius;
        private ExtendedControls.LabelExt labelExt8;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
    }
}