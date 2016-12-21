namespace EDDiscovery.UserControls
{
    partial class UserControlTrippanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pictureBox = new ExtendedControls.PictureBoxHotspot();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setFuelTankToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setFuelWarningToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setJumpRangeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.ContextMenuStrip = this.contextMenuStrip;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(728, 570);
            this.pictureBox.TabIndex = 0;
            this.pictureBox.ClickElement += new ExtendedControls.PictureBoxHotspot.OnElement(this.pictureBox_ClickElement);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setFuelTankToolStripMenuItem,
            this.setFuelWarningToolStripMenuItem,
            this.setJumpRangeToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(160, 114);
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
            // setJumpRangeToolStripMenuItem
            // 
            this.setJumpRangeToolStripMenuItem.Name = "setJumpRangeToolStripMenuItem";
            this.setJumpRangeToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.setJumpRangeToolStripMenuItem.Text = "Set Jump Range";
            this.setJumpRangeToolStripMenuItem.Click += new System.EventHandler(this.setShipDetailsToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // UserControlTrippanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Name = "UserControlTrippanel";
            this.Size = new System.Drawing.Size(728, 570);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.PictureBoxHotspot pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem setFuelTankToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setFuelWarningToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setJumpRangeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
    }
}
