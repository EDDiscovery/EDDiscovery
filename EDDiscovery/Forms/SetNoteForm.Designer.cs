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
            this.panelMain = new System.Windows.Forms.Panel();
            this.textBoxNote = new ExtendedControls.ExtRichTextBox();
            this.buttonCancel = new ExtendedControls.ExtButton();
            this.buttonSave = new ExtendedControls.ExtButton();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.ExtButtonDrawn();
            this.panel_minimize = new ExtendedControls.ExtButtonDrawn();
            this.label_index = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.statusStripCustom = new ExtendedControls.ExtStatusStrip();
            this.panelMain.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
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
            this.labelDetails.Size = new System.Drawing.Size(195, 100);
            this.labelDetails.TabIndex = 11;
            this.labelDetails.Text = "<code>";
            // 
            // panelMain
            // 
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Controls.Add(this.labelTTimestamp);
            this.panelMain.Controls.Add(this.labelTimestamp);
            this.panelMain.Controls.Add(this.labelTSystem);
            this.panelMain.Controls.Add(this.labelSystem);
            this.panelMain.Controls.Add(this.labelTSummary);
            this.panelMain.Controls.Add(this.labelSummary);
            this.panelMain.Controls.Add(this.labelTDetails);
            this.panelMain.Controls.Add(this.labelDetails);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMain.Location = new System.Drawing.Point(0, 32);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(676, 200);
            this.panelMain.TabIndex = 13;
            // 
            // textBoxNote
            // 
            this.textBoxNote.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxNote.BorderColorScaling = 0.5F;
            this.textBoxNote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNote.HideScrollBar = true;
            this.textBoxNote.Location = new System.Drawing.Point(0, 232);
            this.textBoxNote.Name = "textBoxNote";
            this.textBoxNote.ReadOnly = false;
            this.textBoxNote.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\par" +
    "d\\f0\\fs17\\par\r\n}\r\n";
            this.textBoxNote.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.textBoxNote.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.textBoxNote.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.textBoxNote.ScrollBarBorderColor = System.Drawing.Color.White;
            this.textBoxNote.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxNote.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxNote.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.textBoxNote.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.textBoxNote.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.textBoxNote.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.textBoxNote.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.textBoxNote.ShowLineCount = false;
            this.textBoxNote.Size = new System.Drawing.Size(676, 218);
            this.textBoxNote.TabIndex = 1;
            this.textBoxNote.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.textBoxNote.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(441, 6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 24);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "%Cancel%";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(561, 6);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 24);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(676, 32);
            this.panelTop.TabIndex = 33;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(653, 0);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
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
            this.panel_minimize.Location = new System.Drawing.Point(623, 0);
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_minimize.UseMnemonic = true;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 8);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(43, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "<code>";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelTop_MouseDown);
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.buttonSave);
            this.panelBottom.Controls.Add(this.buttonCancel);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 450);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(676, 40);
            this.panelBottom.TabIndex = 12;
            // 
            // statusStripCustom
            // 
            this.statusStripCustom.Location = new System.Drawing.Point(0, 490);
            this.statusStripCustom.Name = "statusStripCustom";
            this.statusStripCustom.Size = new System.Drawing.Size(676, 22);
            this.statusStripCustom.TabIndex = 33;
            // 
            // SetNoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 512);
            this.Controls.Add(this.textBoxNote);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.statusStripCustom);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "SetNoteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Note";

            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelTTimestamp;
        private System.Windows.Forms.Label labelTSystem;
        private ExtendedControls.ExtButton buttonCancel;
        private ExtendedControls.ExtButton buttonSave;
        private System.Windows.Forms.Label labelTSummary;
        private System.Windows.Forms.Label labelTimestamp;
        private System.Windows.Forms.Label labelSystem;
        private System.Windows.Forms.Label labelSummary;
        private System.Windows.Forms.Label labelTDetails;
        private System.Windows.Forms.Label labelDetails;
        private ExtendedControls.ExtRichTextBox textBoxNote;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtButtonDrawn panel_close;
        private ExtendedControls.ExtButtonDrawn panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panelBottom;
        private ExtendedControls.ExtStatusStrip statusStripCustom;
    }
}