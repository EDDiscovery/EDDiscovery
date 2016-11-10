namespace EDDiscovery.UserControls
{
    partial class UserControlStats
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridViewStats = new System.Windows.Forms.DataGridView();
            this.TimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelData = new System.Windows.Forms.Panel();
            this.buttonExt1 = new ExtendedControls.ButtonExt();
            this.vScrollBar = new System.Windows.Forms.VScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.panelData.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewStats
            // 
            this.dataGridViewStats.AllowUserToAddRows = false;
            this.dataGridViewStats.AllowUserToDeleteRows = false;
            this.dataGridViewStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TimeCol,
            this.Type});
            this.dataGridViewStats.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewStats.Name = "dataGridViewStats";
            this.dataGridViewStats.RowHeadersVisible = false;
            this.dataGridViewStats.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewStats.Size = new System.Drawing.Size(726, 336);
            this.dataGridViewStats.TabIndex = 2;
            // 
            // TimeCol
            // 
            this.TimeCol.HeaderText = "Name";
            this.TimeCol.MinimumWidth = 50;
            this.TimeCol.Name = "TimeCol";
            this.TimeCol.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.FillWeight = 400F;
            this.Type.HeaderText = "Information";
            this.Type.MinimumWidth = 50;
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // chart1
            // 
            chartArea2.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.chart1.Legends.Add(legend2);
            this.chart1.Location = new System.Drawing.Point(-3, 356);
            this.chart1.Name = "chart1";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(758, 148);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            // 
            // panelData
            // 
            this.panelData.Controls.Add(this.buttonExt1);
            this.panelData.Controls.Add(this.chart1);
            this.panelData.Controls.Add(this.dataGridViewStats);
            this.panelData.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelData.Location = new System.Drawing.Point(0, 0);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(781, 716);
            this.panelData.TabIndex = 4;
            // 
            // buttonExt1
            // 
            this.buttonExt1.BorderColorScaling = 1.25F;
            this.buttonExt1.ButtonColorScaling = 0.5F;
            this.buttonExt1.ButtonDisabledScaling = 0.5F;
            this.buttonExt1.Location = new System.Drawing.Point(56, 528);
            this.buttonExt1.Name = "buttonExt1";
            this.buttonExt1.Size = new System.Drawing.Size(75, 23);
            this.buttonExt1.TabIndex = 6;
            this.buttonExt1.Text = "buttonExt1";
            this.buttonExt1.UseVisualStyleBackColor = true;
            // 
            // vScrollBar
            // 
            this.vScrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBar.Location = new System.Drawing.Point(781, 0);
            this.vScrollBar.Name = "vScrollBar";
            this.vScrollBar.Size = new System.Drawing.Size(17, 752);
            this.vScrollBar.TabIndex = 7;
            // 
            // UserControlStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelData);
            this.Controls.Add(this.vScrollBar);
            this.Name = "UserControlStats";
            this.Size = new System.Drawing.Size(798, 752);
            this.Resize += new System.EventHandler(this.UserControlStats_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.panelData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Panel panelData;
        private ExtendedControls.ButtonExt buttonExt1;
        private System.Windows.Forms.VScrollBar vScrollBar;
    }
}
