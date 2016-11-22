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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.dataGridViewStats = new System.Windows.Forms.DataGridView();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Information = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mostVisited = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelData = new ExtendedControls.PanelVScroll();
            this.vScrollBarCustom = new ExtendedControls.VScrollBarCustom();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mostVisited)).BeginInit();
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
            this.ItemName,
            this.Information});
            this.dataGridViewStats.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewStats.Name = "dataGridViewStats";
            this.dataGridViewStats.RowHeadersVisible = false;
            this.dataGridViewStats.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewStats.Size = new System.Drawing.Size(156, 336);
            this.dataGridViewStats.TabIndex = 2;
            // 
            // ItemName
            // 
            this.ItemName.HeaderText = "Item";
            this.ItemName.MinimumWidth = 50;
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // Information
            // 
            this.Information.FillWeight = 400F;
            this.Information.HeaderText = "Information";
            this.Information.MinimumWidth = 50;
            this.Information.Name = "Information";
            this.Information.ReadOnly = true;
            // 
            // mostVisited
            // 
            this.mostVisited.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.mostVisited.ChartAreas.Add(chartArea1);
            this.mostVisited.Location = new System.Drawing.Point(-3, 356);
            this.mostVisited.Name = "mostVisited";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.mostVisited.Series.Add(series1);
            this.mostVisited.Size = new System.Drawing.Size(159, 148);
            this.mostVisited.TabIndex = 5;
            this.mostVisited.Text = "Most Visited";
            // 
            // panelData
            // 
            this.panelData.Controls.Add(this.vScrollBarCustom);
            this.panelData.Controls.Add(this.mostVisited);
            this.panelData.Controls.Add(this.dataGridViewStats);
            this.panelData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelData.InternalMargin = new System.Windows.Forms.Padding(0);
            this.panelData.Location = new System.Drawing.Point(0, 0);
            this.panelData.Name = "panelData";
            this.panelData.ScrollBarWidth = 20;
            this.panelData.Size = new System.Drawing.Size(798, 752);
            this.panelData.TabIndex = 4;
            this.panelData.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = false;
            this.vScrollBarCustom.LargeChange = 10;
            this.vScrollBarCustom.Location = new System.Drawing.Point(754, 0);
            this.vScrollBarCustom.Maximum = -244;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(24, 752);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 8;
            this.vScrollBarCustom.Text = "vScrollBarCustom1";
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -244;
            this.vScrollBarCustom.ValueLimited = -244;
            // 
            // UserControlStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelData);
            this.Name = "UserControlStats";
            this.Size = new System.Drawing.Size(798, 752);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mostVisited)).EndInit();
            this.panelData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewStats;
        private System.Windows.Forms.DataVisualization.Charting.Chart mostVisited;
        private ExtendedControls.PanelVScroll panelData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Information;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom;
    }
}
