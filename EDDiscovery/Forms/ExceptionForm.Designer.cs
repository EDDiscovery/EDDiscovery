namespace EDDiscovery.Forms
{ 
    partial class ExceptionForm
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
            if (disposing)
            {
                components?.Dispose();
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
            this.pnlIcon = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.textboxDetails = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelRecoveryOptions = new System.Windows.Forms.Panel();
            this.textBoxRecoveryDetails = new System.Windows.Forms.TextBox();
            this.panelTop.SuspendLayout();
            this.panelDetails.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelRecoveryOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlIcon
            // 
            this.pnlIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pnlIcon.Location = new System.Drawing.Point(3, 3);
            this.pnlIcon.Name = "pnlIcon";
            this.pnlIcon.Size = new System.Drawing.Size(48, 48);
            this.pnlIcon.TabIndex = 0;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblHeader);
            this.panelTop.Controls.Add(this.pnlIcon);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1024, 132);
            this.panelTop.TabIndex = 1;
            // 
            // lblHeader
            // 
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.Location = new System.Drawing.Point(58, 3);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(881, 109);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Summary";
            // 
            // btnReport
            // 
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReport.Location = new System.Drawing.Point(926, 7);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(86, 23);
            this.btnReport.TabIndex = 3;
            this.btnReport.Text = "&Report Issue";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnExit.Location = new System.Drawing.Point(834, 7);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(86, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnContinueOrExit_MouseClick);
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnContinue.Location = new System.Drawing.Point(742, 7);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(86, 23);
            this.btnContinue.TabIndex = 4;
            this.btnContinue.Text = "&Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnContinueOrExit_MouseClick);
            // 
            // panelDetails
            // 
            this.panelDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelDetails.Controls.Add(this.textboxDetails);
            this.panelDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDetails.Location = new System.Drawing.Point(0, 283);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.panelDetails.Size = new System.Drawing.Size(1024, 369);
            this.panelDetails.TabIndex = 2;
            // 
            // textboxDetails
            // 
            this.textboxDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textboxDetails.Location = new System.Drawing.Point(8, 4);
            this.textboxDetails.Multiline = true;
            this.textboxDetails.Name = "textboxDetails";
            this.textboxDetails.ReadOnly = true;
            this.textboxDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textboxDetails.Size = new System.Drawing.Size(1004, 357);
            this.textboxDetails.TabIndex = 0;
            this.textboxDetails.Text = "Detail & stacktrace";
            this.textboxDetails.WordWrap = false;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.btnExit);
            this.panelBottom.Controls.Add(this.btnReport);
            this.panelBottom.Controls.Add(this.btnContinue);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 652);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1024, 38);
            this.panelBottom.TabIndex = 1;
            // 
            // panelRecoveryOptions
            // 
            this.panelRecoveryOptions.Controls.Add(this.textBoxRecoveryDetails);
            this.panelRecoveryOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRecoveryOptions.Location = new System.Drawing.Point(0, 132);
            this.panelRecoveryOptions.Name = "panelRecoveryOptions";
            this.panelRecoveryOptions.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            this.panelRecoveryOptions.Size = new System.Drawing.Size(1024, 151);
            this.panelRecoveryOptions.TabIndex = 1;
            // 
            // textBoxRecoveryDetails
            // 
            this.textBoxRecoveryDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxRecoveryDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxRecoveryDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxRecoveryDetails.Location = new System.Drawing.Point(8, 4);
            this.textBoxRecoveryDetails.Multiline = true;
            this.textBoxRecoveryDetails.Name = "textBoxRecoveryDetails";
            this.textBoxRecoveryDetails.ReadOnly = true;
            this.textBoxRecoveryDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxRecoveryDetails.Size = new System.Drawing.Size(1008, 143);
            this.textBoxRecoveryDetails.TabIndex = 1;
            this.textBoxRecoveryDetails.Text = "Recovery Options";
            this.textBoxRecoveryDetails.WordWrap = false;
            // 
            // ExceptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1024, 690);
            this.Controls.Add(this.panelDetails);
            this.Controls.Add(this.panelRecoveryOptions);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(409, 39);
            this.Name = "ExceptionForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "($AppName) (?Fatal?) Error";
            this.panelTop.ResumeLayout(false);
            this.panelDetails.ResumeLayout(false);
            this.panelDetails.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelRecoveryOptions.ResumeLayout(false);
            this.panelRecoveryOptions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlIcon;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.TextBox textboxDetails;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelRecoveryOptions;
        private System.Windows.Forms.TextBox textBoxRecoveryDetails;
    }
}