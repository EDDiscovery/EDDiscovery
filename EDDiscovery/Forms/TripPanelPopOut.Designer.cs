namespace EDDiscovery.Forms
{
    partial class TripPanelPopOut
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TripPanelPopOut));
            this.lblOutput = new ExtendedControls.LabelExt();
            this.panel_grip = new ExtendedControls.DrawnPanel();
            this.lblSystemName = new ExtendedControls.LabelExt();
            this.dpEDSM = new ExtendedControls.DrawnPanel();
            this.dpReset = new ExtendedControls.DrawnPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setFuelTankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFuelWarningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setShipDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOutput
            // 
            this.lblOutput.AutoSize = true;
            this.lblOutput.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblOutput.ForeColor = System.Drawing.Color.White;
            this.lblOutput.Location = new System.Drawing.Point(62, 32);
            this.lblOutput.Name = "lblOutput";
            this.lblOutput.Size = new System.Drawing.Size(158, 20);
            this.lblOutput.TabIndex = 0;
            this.lblOutput.Text = "Distance output here";
            // 
            // panel_grip
            // 
            this.panel_grip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_grip.BackColor = System.Drawing.Color.Transparent;
            this.panel_grip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_grip.DrawnImage = null;
            this.panel_grip.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Gripper;
            this.panel_grip.ImageText = null;
            this.panel_grip.Location = new System.Drawing.Point(470, 48);
            this.panel_grip.MarginSize = 6;
            this.panel_grip.MouseOverColor = System.Drawing.Color.Black;
            this.panel_grip.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_grip.Name = "panel_grip";
            this.panel_grip.Size = new System.Drawing.Size(20, 20);
            this.panel_grip.TabIndex = 18;
            this.panel_grip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_grip_MouseDown);
            // 
            // lblSystemName
            // 
            this.lblSystemName.AutoSize = true;
            this.lblSystemName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblSystemName.ForeColor = System.Drawing.Color.White;
            this.lblSystemName.Location = new System.Drawing.Point(62, 12);
            this.lblSystemName.Name = "lblSystemName";
            this.lblSystemName.Size = new System.Drawing.Size(172, 20);
            this.lblSystemName.TabIndex = 19;
            this.lblSystemName.Text = "EDSM Infomation here";
            // 
            // dpEDSM
            // 
            this.dpEDSM.DrawnImage = null;
            this.dpEDSM.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Close;
            this.dpEDSM.ImageText = null;
            this.dpEDSM.Location = new System.Drawing.Point(7, 13);
            this.dpEDSM.MarginSize = 4;
            this.dpEDSM.MouseOverColor = System.Drawing.Color.White;
            this.dpEDSM.MouseSelectedColor = System.Drawing.Color.Green;
            this.dpEDSM.Name = "dpEDSM";
            this.dpEDSM.Size = new System.Drawing.Size(50, 19);
            this.dpEDSM.TabIndex = 20;
            // 
            // dpReset
            // 
            this.dpReset.DrawnImage = null;
            this.dpReset.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Close;
            this.dpReset.ImageText = null;
            this.dpReset.Location = new System.Drawing.Point(7, 38);
            this.dpReset.MarginSize = 4;
            this.dpReset.MouseOverColor = System.Drawing.Color.White;
            this.dpReset.MouseSelectedColor = System.Drawing.Color.Green;
            this.dpReset.Name = "dpReset";
            this.dpReset.Size = new System.Drawing.Size(50, 19);
            this.dpReset.TabIndex = 21;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setFuelTankToolStripMenuItem,
            this.setFuelWarningToolStripMenuItem,
            this.setShipDetailsToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(160, 114);
            // 
            // setFuelTankToolStripMenuItem
            // 
            this.setFuelTankToolStripMenuItem.Name = "setFuelTankToolStripMenuItem";
            this.setFuelTankToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.setFuelTankToolStripMenuItem.Text = "Set fuel tank";
            this.setFuelTankToolStripMenuItem.Click += new System.EventHandler(this.setFuelTankToolStripMenuItem_Click);
            // 
            // setFuelWarningToolStripMenuItem
            // 
            this.setFuelWarningToolStripMenuItem.Name = "setFuelWarningToolStripMenuItem";
            this.setFuelWarningToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.setFuelWarningToolStripMenuItem.Text = "Set fuel warning";
            this.setFuelWarningToolStripMenuItem.Click += new System.EventHandler(this.setFuelWarningToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // setShipDetailsToolStripMenuItem
            // 
            this.setShipDetailsToolStripMenuItem.Name = "setShipDetailsToolStripMenuItem";
            this.setShipDetailsToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.setShipDetailsToolStripMenuItem.Text = "Set ship details";
            this.setShipDetailsToolStripMenuItem.Click += new System.EventHandler(this.setShipDetailsToolStripMenuItem_Click);
            // 
            // TripPanelPopOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 71);
            this.Controls.Add(this.dpReset);
            this.Controls.Add(this.dpEDSM);
            this.Controls.Add(this.lblSystemName);
            this.Controls.Add(this.panel_grip);
            this.Controls.Add(this.lblOutput);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TripPanelPopOut";
            this.Text = "Trip Panel";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TripPanelPopOut_FormClosing);
            this.Load += new System.EventHandler(this.TripPanelPopOut_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.TripPanelPopOut_Layout);
            this.Resize += new System.EventHandler(this.TripPanelPopOut_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.LabelExt lblOutput;
        private ExtendedControls.DrawnPanel panel_grip;
        private ExtendedControls.LabelExt lblSystemName;
        private ExtendedControls.DrawnPanel dpEDSM;
        private ExtendedControls.DrawnPanel dpReset;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setFuelTankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setFuelWarningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setShipDetailsToolStripMenuItem;
    }
}