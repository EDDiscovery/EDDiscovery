namespace EDDiscovery.Forms
{
    partial class SetNoteForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.labelForSystem = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTimestamp = new System.Windows.Forms.Label();
            this.labelSystem = new System.Windows.Forms.Label();
            this.labelSummary = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelDetails = new System.Windows.Forms.Label();
            this.buttonSave = new ExtendedControls.ButtonExt();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.textBoxNote = new ExtendedControls.TextBoxBorder();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Timestamp:";
            // 
            // labelForSystem
            // 
            this.labelForSystem.AutoSize = true;
            this.labelForSystem.Location = new System.Drawing.Point(13, 26);
            this.labelForSystem.Name = "labelForSystem";
            this.labelForSystem.Size = new System.Drawing.Size(44, 13);
            this.labelForSystem.TabIndex = 2;
            this.labelForSystem.Text = "System:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Summary:";
            // 
            // labelTimestamp
            // 
            this.labelTimestamp.AutoSize = true;
            this.labelTimestamp.Location = new System.Drawing.Point(80, 13);
            this.labelTimestamp.Name = "labelTimestamp";
            this.labelTimestamp.Size = new System.Drawing.Size(58, 13);
            this.labelTimestamp.TabIndex = 7;
            this.labelTimestamp.Text = "Timestamp";
            // 
            // labelSystem
            // 
            this.labelSystem.AutoSize = true;
            this.labelSystem.Location = new System.Drawing.Point(80, 26);
            this.labelSystem.Name = "labelSystem";
            this.labelSystem.Size = new System.Drawing.Size(52, 13);
            this.labelSystem.TabIndex = 8;
            this.labelSystem.Text = "SysName";
            // 
            // labelSummary
            // 
            this.labelSummary.Location = new System.Drawing.Point(80, 39);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(292, 13);
            this.labelSummary.TabIndex = 9;
            this.labelSummary.Text = "Summary";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Details:";
            // 
            // labelDetails
            // 
            this.labelDetails.Location = new System.Drawing.Point(80, 52);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(292, 52);
            this.labelDetails.TabIndex = 11;
            this.labelDetails.Text = "Details\r\n1\r\n2\r\n3";
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BorderColorScaling = 1.25F;
            this.buttonSave.ButtonColorScaling = 0.5F;
            this.buttonSave.ButtonDisabledScaling = 0.5F;
            this.buttonSave.Location = new System.Drawing.Point(297, 224);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCancel.BorderColorScaling = 1.25F;
            this.buttonCancel.ButtonColorScaling = 0.5F;
            this.buttonCancel.ButtonDisabledScaling = 0.5F;
            this.buttonCancel.Location = new System.Drawing.Point(12, 224);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxNote
            // 
            this.textBoxNote.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxNote.BorderColorScaling = 0.5F;
            this.textBoxNote.Location = new System.Drawing.Point(16, 108);
            this.textBoxNote.Multiline = true;
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxNote.Size = new System.Drawing.Size(356, 110);
            this.textBoxNote.TabIndex = 12;
            // 
            // SetNoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 259);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.labelDetails);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelSummary);
            this.Controls.Add(this.labelSystem);
            this.Controls.Add(this.labelTimestamp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.labelForSystem);
            this.Controls.Add(this.label1);
            this.Name = "SetNoteForm";
            this.Text = "SetNoteForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelForSystem;
        private ExtendedControls.ButtonExt buttonCancel;
        private ExtendedControls.ButtonExt buttonSave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelTimestamp;
        private System.Windows.Forms.Label labelSystem;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelDetails;
        private ExtendedControls.TextBoxBorder textBoxNote;
    }
}