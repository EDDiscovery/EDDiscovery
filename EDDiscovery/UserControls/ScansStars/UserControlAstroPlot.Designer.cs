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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.astroPlot = new ExtendedControls.Controls.AstroPlot();
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
            this.astroPlot.Location = new System.Drawing.Point(0, 0);
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
            this.astroPlot.Size = new System.Drawing.Size(400, 400);
            this.astroPlot.SmallDotSize = 5;
            this.astroPlot.TabIndex = 0;
            this.astroPlot.Text = "astroPlot1";
            this.astroPlot.UnVisitedColor = System.Drawing.Color.Yellow;
            this.astroPlot.View = ExtendedControls.Controls.AstroPlot.PlotPlainView.Perspective;
            this.astroPlot.VisitedColor = System.Drawing.Color.Aqua;
            // 
            // UserControlAstroPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.astroPlot);
            this.DoubleBuffered = true;
            this.Name = "UserControlAstroPlot";
            this.Size = new System.Drawing.Size(400, 400);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.Controls.AstroPlot astroPlot;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
