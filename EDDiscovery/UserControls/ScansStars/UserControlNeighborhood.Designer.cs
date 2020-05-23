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
            this.scatterPlot1 = new EDDiscovery.UserControls.ScatterPlot();
            this.SuspendLayout();
            // 
            // scatterPlot1
            // 
            this.scatterPlot1.Azimuth = 0D;
            this.scatterPlot1.CameraPos = new double[] {
        0D,
        0D,
        0D};
            this.scatterPlot1.Distance = 5D;
            this.scatterPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scatterPlot1.Elevation = 0D;
            this.scatterPlot1.F = 1000D;
            this.scatterPlot1.Location = new System.Drawing.Point(0, 0);
            this.scatterPlot1.Name = "scatterPlot1";
            this.scatterPlot1.Size = new System.Drawing.Size(349, 334);
            this.scatterPlot1.TabIndex = 0;
            // 
            // UserControlNeighborhood
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scatterPlot1);
            this.Name = "UserControlNeighborhood";
            this.Size = new System.Drawing.Size(349, 334);
            this.ResumeLayout(false);

        }

        #endregion

        private ScatterPlot scatterPlot1;
    }
}
