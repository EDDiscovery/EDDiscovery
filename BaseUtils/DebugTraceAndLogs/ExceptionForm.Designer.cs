namespace BaseUtils
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
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnDetails = new System.Windows.Forms.Button();
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnReport = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.textboxDetails = new System.Windows.Forms.TextBox();
            this.pnlHeader.SuspendLayout();
            this.pnlDetails.SuspendLayout();
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
            // pnlHeader
            // 
            this.pnlHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHeader.Controls.Add(this.btnDetails);
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Controls.Add(this.btnReport);
            this.pnlHeader.Controls.Add(this.pnlIcon);
            this.pnlHeader.Controls.Add(this.btnExit);
            this.pnlHeader.Controls.Add(this.btnContinue);
            this.pnlHeader.Location = new System.Drawing.Point(12, 12);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(446, 149);
            this.pnlHeader.TabIndex = 1;
            // 
            // btnDetails
            // 
            this.btnDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDetails.Location = new System.Drawing.Point(3, 123);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(86, 23);
            this.btnDetails.TabIndex = 2;
            this.btnDetails.Text = "▼  &Details";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblHeader.Location = new System.Drawing.Point(58, 3);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(385, 117);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Summary";
            // 
            // btnReport
            // 
            this.btnReport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReport.Location = new System.Drawing.Point(95, 123);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(86, 23);
            this.btnReport.TabIndex = 3;
            this.btnReport.Text = "&Report Issue";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Abort;
            this.btnExit.Location = new System.Drawing.Point(357, 123);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(86, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnContinueOrExit_MouseClick);
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnContinue.Location = new System.Drawing.Point(265, 123);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(86, 23);
            this.btnContinue.TabIndex = 4;
            this.btnContinue.Text = "&Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.MouseClick += new System.Windows.Forms.MouseEventHandler(this.btnContinueOrExit_MouseClick);
            // 
            // pnlDetails
            // 
            this.pnlDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlDetails.Controls.Add(this.textboxDetails);
            this.pnlDetails.Location = new System.Drawing.Point(12, 167);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Size = new System.Drawing.Size(446, 220);
            this.pnlDetails.TabIndex = 2;
            // 
            // textboxDetails
            // 
            this.textboxDetails.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textboxDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textboxDetails.Location = new System.Drawing.Point(0, 0);
            this.textboxDetails.Multiline = true;
            this.textboxDetails.Name = "textboxDetails";
            this.textboxDetails.ReadOnly = true;
            this.textboxDetails.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textboxDetails.Size = new System.Drawing.Size(442, 216);
            this.textboxDetails.TabIndex = 0;
            this.textboxDetails.Text = "Detail & stacktrace";
            this.textboxDetails.WordWrap = false;
            // 
            // ExceptionForm
            // 
            this.AcceptButton = this.btnDetails;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(470, 399);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.pnlHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(409, 39);
            this.Name = "ExceptionForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "($AppName) (?Fatal?) Error";
            this.pnlHeader.ResumeLayout(false);
            this.pnlDetails.ResumeLayout(false);
            this.pnlDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlIcon;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnReport;
        private System.Windows.Forms.Button btnDetails;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.TextBox textboxDetails;
        private System.Windows.Forms.Button btnContinue;
    }
}