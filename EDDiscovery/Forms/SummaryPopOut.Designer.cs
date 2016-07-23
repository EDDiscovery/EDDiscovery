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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SummaryPopOut));
            this.panel_grip = new ExtendedControls.DrawnPanel();
            this.labelExt_NoSystems = new ExtendedControls.LabelExt();
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
            // labelExt_NoSystems
            // 
            this.labelExt_NoSystems.AutoSize = true;
            this.labelExt_NoSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExt_NoSystems.ForeColor = System.Drawing.Color.White;
            this.labelExt_NoSystems.Location = new System.Drawing.Point(48, 30);
            this.labelExt_NoSystems.Name = "labelExt_NoSystems";
            this.labelExt_NoSystems.Size = new System.Drawing.Size(153, 20);
            this.labelExt_NoSystems.TabIndex = 18;
            this.labelExt_NoSystems.Text = "No Systems to show";
            // 
            // SummaryPopOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(600, 268);
            this.Controls.Add(this.labelExt_NoSystems);
            this.Controls.Add(this.panel_grip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SummaryPopOut";
            this.Text = "SummaryPopOut";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SummaryPopOut_FormClosing);
            this.Load += new System.EventHandler(this.SummaryPopOut_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.SummaryPopOut_Layout);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.DrawnPanel panel_grip;
        private ExtendedControls.LabelExt labelExt_NoSystems;
    }
}