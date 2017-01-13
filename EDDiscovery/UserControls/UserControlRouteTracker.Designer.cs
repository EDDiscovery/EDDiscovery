namespace EDDiscovery.UserControls
{
    partial class UserControlRouteTracker
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.setRouteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoCopyWPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoSetTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.ContextMenuStrip = this.contextMenuStrip1;
            this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox.Location = new System.Drawing.Point(0, 0);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(150, 150);
            this.pictureBox.TabIndex = 1;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setRouteToolStripMenuItem,
            this.autoCopyWPToolStripMenuItem,
            this.autoSetTargetToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 92);
            // 
            // setRouteToolStripMenuItem
            // 
            this.setRouteToolStripMenuItem.Name = "setRouteToolStripMenuItem";
            this.setRouteToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.setRouteToolStripMenuItem.Text = "Set route";
            this.setRouteToolStripMenuItem.Click += new System.EventHandler(this.setRouteToolStripMenuItem_Click);
            // 
            // autoCopyWPToolStripMenuItem
            // 
            this.autoCopyWPToolStripMenuItem.CheckOnClick = true;
            this.autoCopyWPToolStripMenuItem.Name = "autoCopyWPToolStripMenuItem";
            this.autoCopyWPToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.autoCopyWPToolStripMenuItem.Text = "Auto copy waypoint";
            this.autoCopyWPToolStripMenuItem.CheckedChanged += new System.EventHandler(this.autoCopyWPToolStripMenuItem_CheckedChanged);
            // 
            // autoSetTargetToolStripMenuItem
            // 
            this.autoSetTargetToolStripMenuItem.CheckOnClick = true;
            this.autoSetTargetToolStripMenuItem.Name = "autoSetTargetToolStripMenuItem";
            this.autoSetTargetToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.autoSetTargetToolStripMenuItem.Text = "Auto set target";
            // 
            // UserControlRouteTracker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox);
            this.Name = "UserControlRouteTracker";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.PictureBoxHotspot pictureBox;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem setRouteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoCopyWPToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoSetTargetToolStripMenuItem;
    }
}
