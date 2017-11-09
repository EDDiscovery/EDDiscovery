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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetNoteForm));
            this.label1 = new System.Windows.Forms.Label();
            this.labelForSystem = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelTimestamp = new System.Windows.Forms.Label();
            this.labelSystem = new System.Windows.Forms.Label();
            this.labelSummary = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelDetails = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonSave = new ExtendedControls.ButtonExt();
            this.textBoxNote = new ExtendedControls.RichTextBoxScroll();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Timestamp:";
            // 
            // labelForSystem
            // 
            this.labelForSystem.AutoSize = true;
            this.labelForSystem.Location = new System.Drawing.Point(17, 34);
            this.labelForSystem.Name = "labelForSystem";
            this.labelForSystem.Size = new System.Drawing.Size(44, 13);
            this.labelForSystem.TabIndex = 2;
            this.labelForSystem.Text = "System:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Summary:";
            // 
            // labelTimestamp
            // 
            this.labelTimestamp.AutoSize = true;
            this.labelTimestamp.Location = new System.Drawing.Point(105, 14);
            this.labelTimestamp.Name = "labelTimestamp";
            this.labelTimestamp.Size = new System.Drawing.Size(58, 13);
            this.labelTimestamp.TabIndex = 7;
            this.labelTimestamp.Text = "Timestamp";
            // 
            // labelSystem
            // 
            this.labelSystem.AutoSize = true;
            this.labelSystem.Location = new System.Drawing.Point(105, 34);
            this.labelSystem.Name = "labelSystem";
            this.labelSystem.Size = new System.Drawing.Size(52, 13);
            this.labelSystem.TabIndex = 8;
            this.labelSystem.Text = "SysName";
            // 
            // labelSummary
            // 
            this.labelSummary.Location = new System.Drawing.Point(105, 54);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(292, 13);
            this.labelSummary.TabIndex = 9;
            this.labelSummary.Text = "Summary";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Details:";
            // 
            // labelDetails
            // 
            this.labelDetails.Location = new System.Drawing.Point(105, 74);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(271, 76);
            this.labelDetails.TabIndex = 11;
            this.labelDetails.Text = "Details\r\n1\r\n2\r\n3";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonCancel);
            this.panel1.Controls.Add(this.buttonSave);
            this.panel1.Controls.Add(this.textBoxNote);
            this.panel1.Controls.Add(this.labelForSystem);
            this.panel1.Controls.Add(this.labelDetails);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.labelTimestamp);
            this.panel1.Controls.Add(this.labelSummary);
            this.panel1.Controls.Add(this.labelSystem);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(396, 327);
            this.panel1.TabIndex = 13;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(203, 289);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(301, 289);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // textBoxNote
            // 
            this.textBoxNote.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxNote.BorderColorScaling = 0.5F;
            this.textBoxNote.HideScrollBar = true;
            this.textBoxNote.Location = new System.Drawing.Point(20, 157);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.ScrollBarWidth = 20;
            this.textBoxNote.ShowLineCount = false;
            this.textBoxNote.Size = new System.Drawing.Size(356, 110);
            this.textBoxNote.TabIndex = 12;
            // 
            // SetNoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 327);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SetNoteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SetNoteForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

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
        private ExtendedControls.RichTextBoxScroll textBoxNote;
        private System.Windows.Forms.Panel panel1;
    }
}