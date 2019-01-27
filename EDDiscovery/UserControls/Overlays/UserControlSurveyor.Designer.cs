namespace EDDiscovery.UserControls
{
    partial class UserControlSurveyor
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
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showAmmoniaWorldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ammoniaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.earthsLikeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waterWorldToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.terraformableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hasVolcanismToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hasRingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBoxHotspot = new ExtendedControls.PictureBoxHotspot();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHotspot)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAmmoniaWorldsToolStripMenuItem,
            this.showToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(163, 48);
            // 
            // showAmmoniaWorldsToolStripMenuItem
            // 
            this.showAmmoniaWorldsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ammoniaToolStripMenuItem,
            this.earthsLikeToolStripMenuItem,
            this.waterWorldToolStripMenuItem});
            this.showAmmoniaWorldsToolStripMenuItem.Name = "showAmmoniaWorldsToolStripMenuItem";
            this.showAmmoniaWorldsToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.showAmmoniaWorldsToolStripMenuItem.Text = "Toggle Bodies";
            // 
            // ammoniaToolStripMenuItem
            // 
            this.ammoniaToolStripMenuItem.Checked = true;
            this.ammoniaToolStripMenuItem.CheckOnClick = true;
            this.ammoniaToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ammoniaToolStripMenuItem.Name = "ammoniaToolStripMenuItem";
            this.ammoniaToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.ammoniaToolStripMenuItem.Text = "Ammonia";
            this.ammoniaToolStripMenuItem.Click += new System.EventHandler(this.ammoniaToolStripMenuItem_Click);
            // 
            // earthsLikeToolStripMenuItem
            // 
            this.earthsLikeToolStripMenuItem.Checked = true;
            this.earthsLikeToolStripMenuItem.CheckOnClick = true;
            this.earthsLikeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.earthsLikeToolStripMenuItem.Name = "earthsLikeToolStripMenuItem";
            this.earthsLikeToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.earthsLikeToolStripMenuItem.Text = "Earth Like";
            this.earthsLikeToolStripMenuItem.Click += new System.EventHandler(this.earthsLikeToolStripMenuItem_Click);
            // 
            // waterWorldToolStripMenuItem
            // 
            this.waterWorldToolStripMenuItem.Checked = true;
            this.waterWorldToolStripMenuItem.CheckOnClick = true;
            this.waterWorldToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.waterWorldToolStripMenuItem.Name = "waterWorldToolStripMenuItem";
            this.waterWorldToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.waterWorldToolStripMenuItem.Text = "Water World";
            this.waterWorldToolStripMenuItem.Click += new System.EventHandler(this.waterWorldToolStripMenuItem_Click);
            // 
            // showToolStripMenuItem
            // 
            this.showToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.terraformableToolStripMenuItem,
            this.hasVolcanismToolStripMenuItem,
            this.hasRingsToolStripMenuItem});
            this.showToolStripMenuItem.Name = "showToolStripMenuItem";
            this.showToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.showToolStripMenuItem.Text = "Toggles Features";
            // 
            // terraformableToolStripMenuItem
            // 
            this.terraformableToolStripMenuItem.Checked = true;
            this.terraformableToolStripMenuItem.CheckOnClick = true;
            this.terraformableToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.terraformableToolStripMenuItem.Name = "terraformableToolStripMenuItem";
            this.terraformableToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.terraformableToolStripMenuItem.Text = "Terraformable";
            this.terraformableToolStripMenuItem.Click += new System.EventHandler(this.terraformableToolStripMenuItem_Click);
            // 
            // hasVolcanismToolStripMenuItem
            // 
            this.hasVolcanismToolStripMenuItem.Checked = true;
            this.hasVolcanismToolStripMenuItem.CheckOnClick = true;
            this.hasVolcanismToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hasVolcanismToolStripMenuItem.Name = "hasVolcanismToolStripMenuItem";
            this.hasVolcanismToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.hasVolcanismToolStripMenuItem.Text = "Has Volcanism";
            this.hasVolcanismToolStripMenuItem.Click += new System.EventHandler(this.hasVolcanismToolStripMenuItem_Click);
            // 
            // hasRingsToolStripMenuItem
            // 
            this.hasRingsToolStripMenuItem.Checked = true;
            this.hasRingsToolStripMenuItem.CheckOnClick = true;
            this.hasRingsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hasRingsToolStripMenuItem.Name = "hasRingsToolStripMenuItem";
            this.hasRingsToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.hasRingsToolStripMenuItem.Text = "Has Rings";
            this.hasRingsToolStripMenuItem.Click += new System.EventHandler(this.hasRingsToolStripMenuItem_Click);
            // 
            // pictureBoxHotspot
            // 
            this.pictureBoxHotspot.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxHotspot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxHotspot.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxHotspot.Name = "pictureBoxHotspot";
            this.pictureBoxHotspot.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxHotspot.TabIndex = 1;
            // 
            // UserControlSurveyor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pictureBoxHotspot);
            this.MinimumSize = new System.Drawing.Size(64, 64);
            this.Name = "UserControlSurveyor";
            this.Size = new System.Drawing.Size(64, 64);
            this.contextMenuStrip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxHotspot)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showAmmoniaWorldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ammoniaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem earthsLikeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waterWorldToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem terraformableToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hasVolcanismToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hasRingsToolStripMenuItem;
        private ExtendedControls.PictureBoxHotspot pictureBoxHotspot;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
