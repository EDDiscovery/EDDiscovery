namespace EDDiscovery
{
    partial class SysWarning
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
            this.labelExt1 = new ExtendedControls.LabelExt();
            this.buttonSubAnyway = new ExtendedControls.ButtonExt();
            this.buttonUpdateAndSub = new ExtendedControls.ButtonExt();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.SuspendLayout();
            // 
            // labelExt1
            // 
            this.labelExt1.AutoSize = true;
            this.labelExt1.Location = new System.Drawing.Point(13, 13);
            this.labelExt1.Name = "labelExt1";
            this.labelExt1.Padding = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.labelExt1.Size = new System.Drawing.Size(62, 13);
            this.labelExt1.TabIndex = 0;
            this.labelExt1.Text = "labelExt1";
            // 
            // buttonSubAnyway
            // 
            this.buttonSubAnyway.BorderColorScaling = 1.25F;
            this.buttonSubAnyway.ButtonColorScaling = 0.5F;
            this.buttonSubAnyway.ButtonDisabledScaling = 0.5F;
            this.buttonSubAnyway.Location = new System.Drawing.Point(12, 76);
            this.buttonSubAnyway.Name = "buttonSubAnyway";
            this.buttonSubAnyway.Size = new System.Drawing.Size(120, 23);
            this.buttonSubAnyway.TabIndex = 1;
            this.buttonSubAnyway.Text = "Submit Anyway";
            this.buttonSubAnyway.UseVisualStyleBackColor = true;
            this.buttonSubAnyway.Click += new System.EventHandler(this.buttonSubAnyway_Click);
            // 
            // buttonUpdateAndSub
            // 
            this.buttonUpdateAndSub.BorderColorScaling = 1.25F;
            this.buttonUpdateAndSub.ButtonColorScaling = 0.5F;
            this.buttonUpdateAndSub.ButtonDisabledScaling = 0.5F;
            this.buttonUpdateAndSub.Location = new System.Drawing.Point(138, 76);
            this.buttonUpdateAndSub.Name = "buttonUpdateAndSub";
            this.buttonUpdateAndSub.Size = new System.Drawing.Size(171, 23);
            this.buttonUpdateAndSub.TabIndex = 2;
            this.buttonUpdateAndSub.Text = "Update System and Submit";
            this.buttonUpdateAndSub.UseVisualStyleBackColor = true;
            this.buttonUpdateAndSub.Click += new System.EventHandler(this.buttonUpdateAndSub_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(315, 76);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(71, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // SysWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(398, 111);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonUpdateAndSub);
            this.Controls.Add(this.buttonSubAnyway);
            this.Controls.Add(this.labelExt1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SysWarning";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Warning: System Mismatch";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.LabelExt labelExt1;
        private ExtendedControls.ButtonExt buttonSubAnyway;
        private ExtendedControls.ButtonExt buttonUpdateAndSub;
        private ExtendedControls.ButtonExt buttonCancel;
    }
}