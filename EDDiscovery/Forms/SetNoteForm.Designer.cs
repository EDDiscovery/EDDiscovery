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
            this.labelTTimestamp = new System.Windows.Forms.Label();
            this.labelTSystem = new System.Windows.Forms.Label();
            this.labelTSummary = new System.Windows.Forms.Label();
            this.labelTimestamp = new System.Windows.Forms.Label();
            this.labelSystem = new System.Windows.Forms.Label();
            this.labelSummary = new System.Windows.Forms.Label();
            this.labelTDetails = new System.Windows.Forms.Label();
            this.labelDetails = new System.Windows.Forms.Label();
            this.panelBack = new System.Windows.Forms.Panel();
            this.buttonCancel = new ExtendedControls.ButtonExt();
            this.buttonSave = new ExtendedControls.ButtonExt();
            this.textBoxNote = new ExtendedControls.RichTextBoxScroll();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.label_index = new System.Windows.Forms.Label();
            this.panelBack.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTTimestamp
            // 
            this.labelTTimestamp.AutoSize = true;
            this.labelTTimestamp.Location = new System.Drawing.Point(3, 10);
            this.labelTTimestamp.Name = "labelTTimestamp";
            this.labelTTimestamp.Size = new System.Drawing.Size(61, 13);
            this.labelTTimestamp.TabIndex = 1;
            this.labelTTimestamp.Text = "Timestamp:";
            // 
            // labelTSystem
            // 
            this.labelTSystem.AutoSize = true;
            this.labelTSystem.Location = new System.Drawing.Point(3, 34);
            this.labelTSystem.Name = "labelTSystem";
            this.labelTSystem.Size = new System.Drawing.Size(44, 13);
            this.labelTSystem.TabIndex = 2;
            this.labelTSystem.Text = "System:";
            // 
            // labelTSummary
            // 
            this.labelTSummary.AutoSize = true;
            this.labelTSummary.Location = new System.Drawing.Point(3, 58);
            this.labelTSummary.Name = "labelTSummary";
            this.labelTSummary.Size = new System.Drawing.Size(53, 13);
            this.labelTSummary.TabIndex = 6;
            this.labelTSummary.Text = "Summary:";
            // 
            // labelTimestamp
            // 
            this.labelTimestamp.AutoSize = true;
            this.labelTimestamp.Location = new System.Drawing.Point(150, 10);
            this.labelTimestamp.Name = "labelTimestamp";
            this.labelTimestamp.Size = new System.Drawing.Size(43, 13);
            this.labelTimestamp.TabIndex = 7;
            this.labelTimestamp.Text = "<code>";
            // 
            // labelSystem
            // 
            this.labelSystem.AutoSize = true;
            this.labelSystem.Location = new System.Drawing.Point(150, 34);
            this.labelSystem.Name = "labelSystem";
            this.labelSystem.Size = new System.Drawing.Size(43, 13);
            this.labelSystem.TabIndex = 8;
            this.labelSystem.Text = "<code>";
            // 
            // labelSummary
            // 
            this.labelSummary.AutoSize = true;
            this.labelSummary.Location = new System.Drawing.Point(150, 58);
            this.labelSummary.Name = "labelSummary";
            this.labelSummary.Size = new System.Drawing.Size(43, 13);
            this.labelSummary.TabIndex = 9;
            this.labelSummary.Text = "<code>";
            // 
            // labelTDetails
            // 
            this.labelTDetails.AutoSize = true;
            this.labelTDetails.Location = new System.Drawing.Point(3, 82);
            this.labelTDetails.Name = "labelTDetails";
            this.labelTDetails.Size = new System.Drawing.Size(42, 13);
            this.labelTDetails.TabIndex = 10;
            this.labelTDetails.Text = "Details:";
            // 
            // labelDetails
            // 
            this.labelDetails.Location = new System.Drawing.Point(150, 82);
            this.labelDetails.Name = "labelDetails";
            this.labelDetails.Size = new System.Drawing.Size(366, 91);
            this.labelDetails.TabIndex = 11;
            this.labelDetails.Text = "<code>";
            // 
            // panelBack
            // 
            this.panelBack.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelBack.Controls.Add(this.labelTTimestamp);
            this.panelBack.Controls.Add(this.buttonCancel);
            this.panelBack.Controls.Add(this.buttonSave);
            this.panelBack.Controls.Add(this.textBoxNote);
            this.panelBack.Controls.Add(this.labelTSystem);
            this.panelBack.Controls.Add(this.labelDetails);
            this.panelBack.Controls.Add(this.labelTSummary);
            this.panelBack.Controls.Add(this.labelTDetails);
            this.panelBack.Controls.Add(this.labelTimestamp);
            this.panelBack.Controls.Add(this.labelSummary);
            this.panelBack.Controls.Add(this.labelSystem);
            this.panelBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBack.Location = new System.Drawing.Point(0, 32);
            this.panelBack.Name = "panelBack";
            this.panelBack.Size = new System.Drawing.Size(529, 354);
            this.panelBack.TabIndex = 13;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(298, 312);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(416, 312);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 23);
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
            this.textBoxNote.Location = new System.Drawing.Point(6, 184);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.ReadOnly = false;
            this.textBoxNote.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.textBoxNote.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.textBoxNote.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.textBoxNote.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.textBoxNote.ScrollBarBorderColor = System.Drawing.Color.White;
            this.textBoxNote.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxNote.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNote.ScrollBarLineTweak = 0;
            this.textBoxNote.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.textBoxNote.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.textBoxNote.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.textBoxNote.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.textBoxNote.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.textBoxNote.ScrollBarWidth = 20;
            this.textBoxNote.ShowLineCount = false;
            this.textBoxNote.Size = new System.Drawing.Size(510, 110);
            this.textBoxNote.TabIndex = 12;
            this.textBoxNote.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.textBoxNote.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(529, 32);
            this.panelTop.TabIndex = 33;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(506, 0);
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.TabStop = false;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(476, 0);
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 8);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(49, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "Set Note";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            // 
            // SetNoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(529, 386);
            this.Controls.Add(this.panelBack);
            this.Controls.Add(this.panelTop);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "SetNoteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Note";
            this.panelBack.ResumeLayout(false);
            this.panelBack.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label labelTTimestamp;
        private System.Windows.Forms.Label labelTSystem;
        private ExtendedControls.ButtonExt buttonCancel;
        private ExtendedControls.ButtonExt buttonSave;
        private System.Windows.Forms.Label labelTSummary;
        private System.Windows.Forms.Label labelTimestamp;
        private System.Windows.Forms.Label labelSystem;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.Label labelTDetails;
        private System.Windows.Forms.Label labelDetails;
        private ExtendedControls.RichTextBoxScroll textBoxNote;
        private System.Windows.Forms.Panel panelBack;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_index;
    }
}