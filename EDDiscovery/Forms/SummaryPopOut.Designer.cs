namespace EDDiscovery2
{
    partial class SummaryPopOut
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
            this.panel_grip = new ExtendedControls.DrawnPanel();
            this.SuspendLayout();
            // 
            // panel_grip
            // 
            this.panel_grip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_grip.BackColor = System.Drawing.Color.Transparent;
            this.panel_grip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_grip.Image = ExtendedControls.DrawnPanel.ImageType.Gripper;
            this.panel_grip.ImageText = null;
            this.panel_grip.Location = new System.Drawing.Point(580, 236);
            this.panel_grip.MarginSize = 6;
            this.panel_grip.MouseOverColor = System.Drawing.Color.Black;
            this.panel_grip.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_grip.Name = "panel_grip";
            this.panel_grip.Size = new System.Drawing.Size(20, 20);
            this.panel_grip.TabIndex = 17;
            this.panel_grip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_grip_MouseDown);
            // 
            // SummaryPopOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(600, 268);
            this.Controls.Add(this.panel_grip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SummaryPopOut";
            this.Text = "SummaryPopOut";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SummaryPopOut_FormClosing);
            this.Load += new System.EventHandler(this.SummaryPopOut_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.SummaryPopOut_Layout);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.DrawnPanel panel_grip;
    }
}