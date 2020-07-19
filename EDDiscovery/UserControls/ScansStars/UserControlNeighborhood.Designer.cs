namespace EDDiscovery.UserControls
{
    partial class UserControlNeighborhood
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
            this.astroPlot1 = new ExtendedControls.Controls.AstroPlot();
            this.SuspendLayout();
            // 
            // astroPlot1
            // 
            this.astroPlot1.AxesLength = 10;
            this.astroPlot1.AxesThickness = 1;
            this.astroPlot1.Azimuth = 0D;
            this.astroPlot1.CurrentColor = System.Drawing.Color.Red;
            this.astroPlot1.Distance = 300D;
            this.astroPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.astroPlot1.Elevation = 0D;
            this.astroPlot1.Focus = 1000D;
            this.astroPlot1.ForeColor = System.Drawing.Color.White;
            this.astroPlot1.FramesLength = 20D;
            this.astroPlot1.FramesThickness = 1;
            this.astroPlot1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(30)))), ((int)(((byte)(190)))), ((int)(((byte)(240)))));
            this.astroPlot1.GridCount = 5;
            this.astroPlot1.GridUnit = 10;
            this.astroPlot1.HotSpotSize = 0D;
            this.astroPlot1.IsObjectSelected = false;
            this.astroPlot1.LargeDotSize = 15;
            this.astroPlot1.Location = new System.Drawing.Point(0, 0);
            this.astroPlot1.MediumDotSize = 10;
            this.astroPlot1.MouseDrag_Multiply = 20D;
            this.astroPlot1.MouseDrag_Resistance = 12D;
            this.astroPlot1.MouseRotation_Multiply = 1D;
            this.astroPlot1.MouseRotation_Resistance = 75D;
            this.astroPlot1.MouseWheel_Multiply = 7D;
            this.astroPlot1.MouseWheel_Resistance = 2D;
            this.astroPlot1.Name = "astroPlot1";
            this.astroPlot1.Projection = ExtendedControls.Controls.AstroPlot.PlotProjection.Free;
            this.astroPlot1.ShowAxesWidget = true;
            this.astroPlot1.ShowFrameWidget = false;
            this.astroPlot1.ShowGridWidget = true;
            this.astroPlot1.Size = new System.Drawing.Size(300, 300);
            this.astroPlot1.SmallDotSize = 5;
            this.astroPlot1.TabIndex = 0;
            this.astroPlot1.Text = "astroPlot1";
            this.astroPlot1.UnVisitedColor = System.Drawing.Color.Yellow;
            this.astroPlot1.View = ExtendedControls.Controls.AstroPlot.PlotPlainView.Perspective;
            this.astroPlot1.VisitedColor = System.Drawing.Color.Aqua;
            // 
            // UserControlAstroPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.astroPlot1);
            this.Name = "UserControlAstroPlot";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.Controls.AstroPlot astroPlot1;
    }
}
