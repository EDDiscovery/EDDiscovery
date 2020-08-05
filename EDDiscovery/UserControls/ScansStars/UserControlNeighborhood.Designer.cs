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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlNeighborhood));
            this.astroPlot = new ExtendedControls.Controls.AstroPlot();
            this.numberBoxMin = new ExtendedControls.NumberBoxDouble();
            this.numberBoxMax = new ExtendedControls.NumberBoxDouble();
            this.extLabelMin = new ExtendedControls.ExtLabel();
            this.extLabelMax = new ExtendedControls.ExtLabel();
            this.SuspendLayout();
            // 
            // astroPlot1
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
            this.astroPlot.Name = "astroPlot1";
            this.astroPlot.Projection = ExtendedControls.Controls.AstroPlot.PlotProjection.Free;
            this.astroPlot.ShowAxesWidget = true;
            this.astroPlot.ShowFrameWidget = false;
            this.astroPlot.ShowGridWidget = true;
            this.astroPlot.Size = new System.Drawing.Size(300, 300);
            this.astroPlot.SmallDotSize = 5;
            this.astroPlot.TabIndex = 0;
            this.astroPlot.Text = "astroPlot1";
            this.astroPlot.UnVisitedColor = System.Drawing.Color.Yellow;
            this.astroPlot.View = ExtendedControls.Controls.AstroPlot.PlotPlainView.Perspective;
            this.astroPlot.VisitedColor = System.Drawing.Color.Aqua;
            // 
            // numberBoxMin
            // 
            this.numberBoxMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numberBoxMin.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxMin.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxMin.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxMin.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxMin.BorderColorScaling = 0.5F;
            this.numberBoxMin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxMin.ClearOnFirstChar = false;
            this.numberBoxMin.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxMin.DelayBeforeNotification = 0;
            this.numberBoxMin.EndButtonEnable = true;
            this.numberBoxMin.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxMin.EndButtonImage")));
            this.numberBoxMin.EndButtonVisible = false;
            this.numberBoxMin.Format = "N";
            this.numberBoxMin.InErrorCondition = false;
            this.numberBoxMin.Location = new System.Drawing.Point(191, 274);
            this.numberBoxMin.Maximum = 1.7976931348623157E+308D;
            this.numberBoxMin.Minimum = -1.7976931348623157E+308D;
            this.numberBoxMin.Multiline = false;
            this.numberBoxMin.Name = "numberBoxMin";
            this.numberBoxMin.ReadOnly = false;
            this.numberBoxMin.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxMin.SelectionLength = 0;
            this.numberBoxMin.SelectionStart = 0;
            this.numberBoxMin.Size = new System.Drawing.Size(50, 23);
            this.numberBoxMin.TabIndex = 1;
            this.numberBoxMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxMin.Value = 0D;
            this.numberBoxMin.WordWrap = true;
            this.numberBoxMin.ValueChanged += new System.EventHandler(this.NumberBoxMin_ValueChanged);
            // 
            // numberBoxMax
            // 
            this.numberBoxMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numberBoxMax.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxMax.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxMax.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxMax.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxMax.BorderColorScaling = 0.5F;
            this.numberBoxMax.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxMax.ClearOnFirstChar = false;
            this.numberBoxMax.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxMax.DelayBeforeNotification = 0;
            this.numberBoxMax.EndButtonEnable = true;
            this.numberBoxMax.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxMax.EndButtonImage")));
            this.numberBoxMax.EndButtonVisible = false;
            this.numberBoxMax.Format = "N";
            this.numberBoxMax.InErrorCondition = false;
            this.numberBoxMax.Location = new System.Drawing.Point(247, 274);
            this.numberBoxMax.Maximum = 1.7976931348623157E+308D;
            this.numberBoxMax.Minimum = -1.7976931348623157E+308D;
            this.numberBoxMax.Multiline = false;
            this.numberBoxMax.Name = "numberBoxMax";
            this.numberBoxMax.ReadOnly = false;
            this.numberBoxMax.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxMax.SelectionLength = 0;
            this.numberBoxMax.SelectionStart = 0;
            this.numberBoxMax.Size = new System.Drawing.Size(50, 23);
            this.numberBoxMax.TabIndex = 2;
            this.numberBoxMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxMax.Value = 0D;
            this.numberBoxMax.WordWrap = true;
            this.numberBoxMax.ValueChanged += new System.EventHandler(this.NumberBoxMax_ValueChanged);
            // 
            // extLabelMin
            // 
            this.extLabelMin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.extLabelMin.AutoSize = true;
            this.extLabelMin.Location = new System.Drawing.Point(199, 258);
            this.extLabelMin.Name = "extLabelMin";
            this.extLabelMin.Size = new System.Drawing.Size(23, 13);
            this.extLabelMin.TabIndex = 3;
            this.extLabelMin.Text = "min";
            this.extLabelMin.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // extLabelMax
            // 
            this.extLabelMax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.extLabelMax.AutoSize = true;
            this.extLabelMax.Location = new System.Drawing.Point(254, 258);
            this.extLabelMax.Name = "extLabelMax";
            this.extLabelMax.Size = new System.Drawing.Size(26, 13);
            this.extLabelMax.TabIndex = 4;
            this.extLabelMax.Text = "max";
            this.extLabelMax.TextBackColor = System.Drawing.Color.Transparent;
            // 
            // UserControlNeighborhood
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extLabelMax);
            this.Controls.Add(this.extLabelMin);
            this.Controls.Add(this.numberBoxMax);
            this.Controls.Add(this.numberBoxMin);
            this.Controls.Add(this.astroPlot);
            this.Name = "UserControlNeighborhood";
            this.Size = new System.Drawing.Size(300, 300);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.Controls.AstroPlot astroPlot;
        private ExtendedControls.NumberBoxDouble numberBoxMin;
        private ExtendedControls.NumberBoxDouble numberBoxMax;
        private ExtendedControls.ExtLabel extLabelMin;
        private ExtendedControls.ExtLabel extLabelMax;
    }
}
