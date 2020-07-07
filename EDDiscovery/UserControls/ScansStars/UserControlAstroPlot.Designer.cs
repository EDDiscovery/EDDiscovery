namespace EDDiscovery.UserControls
{
    partial class UserControlAstroPlot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlAstroPlot));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.astroPlot = new ExtendedControls.Controls.AstroPlot();
            this.numberBoxMinRadius = new ExtendedControls.NumberBoxDouble();
            this.numberBoxMaxRadius = new ExtendedControls.NumberBoxDouble();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.maxSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem250 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem200 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem150 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem100 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem50 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem25 = new System.Windows.Forms.ToolStripMenuItem();
            this.axesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripShowAxes = new System.Windows.Forms.ToolStripMenuItem();
            this.framesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showToolStripShowFrames = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cubeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.planesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelNoTheme1 = new ExtendedControls.PanelNoTheme();
            this.menuStrip1.SuspendLayout();
            this.panelNoTheme1.SuspendLayout();
            this.SuspendLayout();
            // 
            // astroPlot
            // 
            this.astroPlot.AxesLength = 10;
            this.astroPlot.AxesThickness = 1;
            this.astroPlot.Azimuth = 0D;
            this.astroPlot.CurrentColor = System.Drawing.Color.Red;
            this.astroPlot.Distance = 300D;
            this.astroPlot.Dock = System.Windows.Forms.DockStyle.Fill;
            this.astroPlot.Elevation = 0D;
            this.astroPlot.Focus = 1000D;
            this.astroPlot.ForeColor = System.Drawing.Color.White;
            this.astroPlot.FramesLength = 20D;
            this.astroPlot.FramesThickness = 1;
            this.astroPlot.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(30)))), ((int)(((byte)(190)))), ((int)(((byte)(240)))));
            this.astroPlot.GridCount = 5;
            this.astroPlot.GridUnit = 10;
            this.astroPlot.HotSpotSize = 0D;
            this.astroPlot.IsObjectSelected = false;
            this.astroPlot.LargeDotSize = 15;
            this.astroPlot.Location = new System.Drawing.Point(0, 30);
            this.astroPlot.MediumDotSize = 10;
            this.astroPlot.MouseDrag_Multiply = 20D;
            this.astroPlot.MouseDrag_Resistance = 12D;
            this.astroPlot.MouseRotation_Multiply = 1D;
            this.astroPlot.MouseRotation_Resistance = 75D;
            this.astroPlot.MouseWheel_Multiply = 7D;
            this.astroPlot.MouseWheel_Resistance = 2D;
            this.astroPlot.Name = "astroPlot";
            this.astroPlot.Projection = ExtendedControls.Controls.AstroPlot.PlotProjection.Free;
            this.astroPlot.ShowAxesWidget = true;
            this.astroPlot.ShowFrameWidget = false;
            this.astroPlot.ShowGridWidget = true;
            this.astroPlot.Size = new System.Drawing.Size(400, 370);
            this.astroPlot.SmallDotSize = 5;
            this.astroPlot.TabIndex = 0;
            this.astroPlot.Text = "astroPlot1";
            this.astroPlot.UnVisitedColor = System.Drawing.Color.Yellow;
            this.astroPlot.View = ExtendedControls.Controls.AstroPlot.PlotPlainView.Perspective;
            this.astroPlot.VisitedColor = System.Drawing.Color.Aqua;
            // 
            // numberBoxMinRadius
            // 
            this.numberBoxMinRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxMinRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxMinRadius.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxMinRadius.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxMinRadius.BorderColorScaling = 0.5F;
            this.numberBoxMinRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxMinRadius.ClearOnFirstChar = false;
            this.numberBoxMinRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxMinRadius.DelayBeforeNotification = 0;
            this.numberBoxMinRadius.EndButtonEnable = true;
            this.numberBoxMinRadius.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxMinRadius.EndButtonImage")));
            this.numberBoxMinRadius.EndButtonVisible = false;
            this.numberBoxMinRadius.Format = "N";
            this.numberBoxMinRadius.InErrorCondition = false;
            this.numberBoxMinRadius.Location = new System.Drawing.Point(3, 2);
            this.numberBoxMinRadius.Maximum = 1.7976931348623157E+308D;
            this.numberBoxMinRadius.Minimum = -1.7976931348623157E+308D;
            this.numberBoxMinRadius.Multiline = false;
            this.numberBoxMinRadius.Name = "numberBoxMinRadius";
            this.numberBoxMinRadius.ReadOnly = false;
            this.numberBoxMinRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxMinRadius.SelectionLength = 0;
            this.numberBoxMinRadius.SelectionStart = 0;
            this.numberBoxMinRadius.Size = new System.Drawing.Size(50, 25);
            this.numberBoxMinRadius.TabIndex = 2;
            this.numberBoxMinRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxMinRadius.Value = 0D;
            this.numberBoxMinRadius.WordWrap = true;
            this.numberBoxMinRadius.ValueChanged += new System.EventHandler(this.numberBoxMinRadius_ValueChanged);
            // 
            // numberBoxMaxRadius
            // 
            this.numberBoxMaxRadius.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxMaxRadius.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxMaxRadius.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxMaxRadius.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxMaxRadius.BorderColorScaling = 0.5F;
            this.numberBoxMaxRadius.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxMaxRadius.ClearOnFirstChar = false;
            this.numberBoxMaxRadius.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxMaxRadius.DelayBeforeNotification = 0;
            this.numberBoxMaxRadius.EndButtonEnable = true;
            this.numberBoxMaxRadius.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxMaxRadius.EndButtonImage")));
            this.numberBoxMaxRadius.EndButtonVisible = false;
            this.numberBoxMaxRadius.Format = "N";
            this.numberBoxMaxRadius.InErrorCondition = false;
            this.numberBoxMaxRadius.Location = new System.Drawing.Point(61, 2);
            this.numberBoxMaxRadius.Maximum = 1.7976931348623157E+308D;
            this.numberBoxMaxRadius.Minimum = -1.7976931348623157E+308D;
            this.numberBoxMaxRadius.Multiline = false;
            this.numberBoxMaxRadius.Name = "numberBoxMaxRadius";
            this.numberBoxMaxRadius.ReadOnly = false;
            this.numberBoxMaxRadius.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxMaxRadius.SelectionLength = 0;
            this.numberBoxMaxRadius.SelectionStart = 0;
            this.numberBoxMaxRadius.Size = new System.Drawing.Size(50, 25);
            this.numberBoxMaxRadius.TabIndex = 3;
            this.numberBoxMaxRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxMaxRadius.Value = 0D;
            this.numberBoxMaxRadius.WordWrap = true;
            this.numberBoxMaxRadius.ValueChanged += new System.EventHandler(this.numberBoxMaxRadius_ValueChanged);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(112, 4);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(189, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.maxSystemsToolStripMenuItem,
            this.axesToolStripMenuItem,
            this.framesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // maxSystemsToolStripMenuItem
            // 
            this.maxSystemsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem250,
            this.toolStripMenuItem200,
            this.toolStripMenuItem150,
            this.toolStripMenuItem100,
            this.toolStripMenuItem50,
            this.toolStripMenuItem25});
            this.maxSystemsToolStripMenuItem.Name = "maxSystemsToolStripMenuItem";
            this.maxSystemsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.maxSystemsToolStripMenuItem.Text = "Max Systems";
            // 
            // toolStripMenuItem250
            // 
            this.toolStripMenuItem250.CheckOnClick = true;
            this.toolStripMenuItem250.Name = "toolStripMenuItem250";
            this.toolStripMenuItem250.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem250.Text = "250";
            this.toolStripMenuItem250.Click += new System.EventHandler(this.toolStripMenuItem250_Click);
            // 
            // toolStripMenuItem200
            // 
            this.toolStripMenuItem200.CheckOnClick = true;
            this.toolStripMenuItem200.Name = "toolStripMenuItem200";
            this.toolStripMenuItem200.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem200.Text = "200";
            this.toolStripMenuItem200.Click += new System.EventHandler(this.toolStripMenuItem200_Click);
            // 
            // toolStripMenuItem150
            // 
            this.toolStripMenuItem150.Checked = true;
            this.toolStripMenuItem150.CheckOnClick = true;
            this.toolStripMenuItem150.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem150.Name = "toolStripMenuItem150";
            this.toolStripMenuItem150.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem150.Text = "150";
            this.toolStripMenuItem150.Click += new System.EventHandler(this.toolStripMenuItem150_Click);
            // 
            // toolStripMenuItem100
            // 
            this.toolStripMenuItem100.CheckOnClick = true;
            this.toolStripMenuItem100.Name = "toolStripMenuItem100";
            this.toolStripMenuItem100.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem100.Text = "100";
            this.toolStripMenuItem100.Click += new System.EventHandler(this.toolStripMenuItem100_Click);
            // 
            // toolStripMenuItem50
            // 
            this.toolStripMenuItem50.CheckOnClick = true;
            this.toolStripMenuItem50.Name = "toolStripMenuItem50";
            this.toolStripMenuItem50.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem50.Text = "50";
            this.toolStripMenuItem50.Click += new System.EventHandler(this.toolStripMenuItem50_Click);
            // 
            // toolStripMenuItem25
            // 
            this.toolStripMenuItem25.CheckOnClick = true;
            this.toolStripMenuItem25.Name = "toolStripMenuItem25";
            this.toolStripMenuItem25.Size = new System.Drawing.Size(180, 22);
            this.toolStripMenuItem25.Text = "25";
            this.toolStripMenuItem25.Click += new System.EventHandler(this.toolStripMenuItem25_Click);
            // 
            // axesToolStripMenuItem
            // 
            this.axesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripShowAxes});
            this.axesToolStripMenuItem.Name = "axesToolStripMenuItem";
            this.axesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.axesToolStripMenuItem.Text = "Axes";
            // 
            // showToolStripShowAxes
            // 
            this.showToolStripShowAxes.Checked = true;
            this.showToolStripShowAxes.CheckOnClick = true;
            this.showToolStripShowAxes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showToolStripShowAxes.Name = "showToolStripShowAxes";
            this.showToolStripShowAxes.Size = new System.Drawing.Size(103, 22);
            this.showToolStripShowAxes.Text = "Show";
            this.showToolStripShowAxes.CheckedChanged += new System.EventHandler(this.showToolStripMenuItem_CheckedChanged);
            // 
            // framesToolStripMenuItem
            // 
            this.framesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showToolStripShowFrames,
            this.toolStripSeparator1,
            this.cubeToolStripMenuItem,
            this.planesToolStripMenuItem});
            this.framesToolStripMenuItem.Name = "framesToolStripMenuItem";
            this.framesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.framesToolStripMenuItem.Text = "Frames";
            // 
            // showToolStripShowFrames
            // 
            this.showToolStripShowFrames.CheckOnClick = true;
            this.showToolStripShowFrames.Name = "showToolStripShowFrames";
            this.showToolStripShowFrames.Size = new System.Drawing.Size(108, 22);
            this.showToolStripShowFrames.Text = "Show";
            this.showToolStripShowFrames.CheckedChanged += new System.EventHandler(this.showToolStripMenuItem1_CheckedChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(105, 6);
            // 
            // cubeToolStripMenuItem
            // 
            this.cubeToolStripMenuItem.Checked = true;
            this.cubeToolStripMenuItem.CheckOnClick = true;
            this.cubeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cubeToolStripMenuItem.Name = "cubeToolStripMenuItem";
            this.cubeToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.cubeToolStripMenuItem.Text = "Cube";
            this.cubeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.cubeToolStripMenuItem_CheckedChanged);
            // 
            // planesToolStripMenuItem
            // 
            this.planesToolStripMenuItem.CheckOnClick = true;
            this.planesToolStripMenuItem.Name = "planesToolStripMenuItem";
            this.planesToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.planesToolStripMenuItem.Text = "Planes";
            this.planesToolStripMenuItem.CheckedChanged += new System.EventHandler(this.planesToolStripMenuItem_CheckedChanged);
            // 
            // panelNoTheme1
            // 
            this.panelNoTheme1.Controls.Add(this.numberBoxMaxRadius);
            this.panelNoTheme1.Controls.Add(this.numberBoxMinRadius);
            this.panelNoTheme1.Controls.Add(this.menuStrip1);
            this.panelNoTheme1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelNoTheme1.Location = new System.Drawing.Point(0, 0);
            this.panelNoTheme1.Name = "panelNoTheme1";
            this.panelNoTheme1.Size = new System.Drawing.Size(400, 30);
            this.panelNoTheme1.TabIndex = 4;
            // 
            // UserControlAstroPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.astroPlot);
            this.Controls.Add(this.panelNoTheme1);
            this.DoubleBuffered = true;
            this.Name = "UserControlAstroPlot";
            this.Size = new System.Drawing.Size(400, 400);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelNoTheme1.ResumeLayout(false);
            this.panelNoTheme1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.Controls.AstroPlot astroPlot;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.NumberBoxDouble numberBoxMinRadius;
        private ExtendedControls.NumberBoxDouble numberBoxMaxRadius;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem axesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripShowAxes;
        private System.Windows.Forms.ToolStripMenuItem framesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showToolStripShowFrames;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cubeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem planesToolStripMenuItem;
        private ExtendedControls.PanelNoTheme panelNoTheme1;
        private System.Windows.Forms.ToolStripMenuItem maxSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem250;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem200;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem150;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem100;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem50;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem25;
    }
}
