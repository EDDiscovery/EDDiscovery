/*
 * Copyright © 2016 - 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
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
            ExtendedControls.TabStyleSquare tabStyleSquare1 = new ExtendedControls.TabStyleSquare();
            this.tabPageGraph = new System.Windows.Forms.TabPage();
            this.panelGraphs = new ExtendedControls.PanelVScroll();
            this.mostVisited = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tabPageScan = new System.Windows.Forms.TabPage();
            this.dataGridViewScan = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageTravel = new System.Windows.Forms.TabPage();
            this.dataGridViewTravel = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.panelGrids = new ExtendedControls.PanelVScroll();
            this.panelCurrentSystemGrid = new System.Windows.Forms.Panel();
            this.labelExtCurrentSysGrid = new ExtendedControls.LabelExt();
            this.dataGridViewCurrentStats = new System.Windows.Forms.DataGridView();
            this.Information = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelGlobalStatsGrid = new System.Windows.Forms.Panel();
            this.labelExtGlobalStatsGrid = new ExtendedControls.LabelExt();
            this.dataGridViewGlobalStats = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControlCustomStats = new ExtendedControls.TabControlCustom();
            this.userControlStatsTimeTravel = new EDDiscovery.UserControls.UserControlStatsTime();
            this.userControlStatsTimeScan = new EDDiscovery.UserControls.UserControlStatsTime();
            this.tabPageGraph.SuspendLayout();
            this.panelGraphs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mostVisited)).BeginInit();
            this.tabPageScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).BeginInit();
            this.tabPageTravel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).BeginInit();
            this.tabPageGeneral.SuspendLayout();
            this.panelGrids.SuspendLayout();
            this.panelCurrentSystemGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurrentStats)).BeginInit();
            this.panelGlobalStatsGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGlobalStats)).BeginInit();
            this.tabControlCustomStats.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageGraph
            // 
            this.tabPageGraph.Controls.Add(this.panelGraphs);
            this.tabPageGraph.Location = new System.Drawing.Point(4, 22);
            this.tabPageGraph.Name = "tabPageGraph";
            this.tabPageGraph.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGraph.Size = new System.Drawing.Size(481, 364);
            this.tabPageGraph.TabIndex = 6;
            this.tabPageGraph.Text = "Graphs";
            this.tabPageGraph.UseVisualStyleBackColor = true;
            // 
            // panelGraphs
            // 
            this.panelGraphs.AutoScroll = true;
            this.panelGraphs.Controls.Add(this.mostVisited);
            this.panelGraphs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraphs.InternalMargin = new System.Windows.Forms.Padding(0);
            this.panelGraphs.Location = new System.Drawing.Point(3, 3);
            this.panelGraphs.Name = "panelGraphs";
            this.panelGraphs.ScrollBarWidth = 20;
            this.panelGraphs.Size = new System.Drawing.Size(475, 358);
            this.panelGraphs.TabIndex = 4;
            this.panelGraphs.VerticalScrollBarDockRight = true;
            this.panelGraphs.Paint += new System.Windows.Forms.PaintEventHandler(this.panelData_Paint);
            // 
            // mostVisited
            // 
            this.mostVisited.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            chartArea1.Name = "ChartArea1";
            this.mostVisited.ChartAreas.Add(chartArea1);
            this.mostVisited.Dock = System.Windows.Forms.DockStyle.Top;
            this.mostVisited.Location = new System.Drawing.Point(0, 0);
            this.mostVisited.Name = "mostVisited";
            series1.ChartArea = "ChartArea1";
            series1.Name = "Series1";
            this.mostVisited.Series.Add(series1);
            this.mostVisited.Size = new System.Drawing.Size(475, 261);
            this.mostVisited.TabIndex = 6;
            this.mostVisited.Text = "Most Visited";
            // 
            // tabPageScan
            // 
            this.tabPageScan.Controls.Add(this.dataGridViewScan);
            this.tabPageScan.Controls.Add(this.userControlStatsTimeScan);
            this.tabPageScan.Location = new System.Drawing.Point(4, 22);
            this.tabPageScan.Name = "tabPageScan";
            this.tabPageScan.Size = new System.Drawing.Size(481, 364);
            this.tabPageScan.TabIndex = 1;
            this.tabPageScan.Text = "Scan";
            this.tabPageScan.UseVisualStyleBackColor = true;
            // 
            // dataGridViewScan
            // 
            this.dataGridViewScan.AllowUserToAddRows = false;
            this.dataGridViewScan.AllowUserToDeleteRows = false;
            this.dataGridViewScan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewScan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScan.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dataGridViewScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewScan.Location = new System.Drawing.Point(0, 27);
            this.dataGridViewScan.Name = "dataGridViewScan";
            this.dataGridViewScan.RowHeadersVisible = false;
            this.dataGridViewScan.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewScan.Size = new System.Drawing.Size(481, 337);
            this.dataGridViewScan.TabIndex = 4;
            this.dataGridViewScan.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewScan_ColumnHeaderMouseClick);
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 77.59495F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Information";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 79.99479F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Item";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // tabPageTravel
            // 
            this.tabPageTravel.Controls.Add(this.dataGridViewTravel);
            this.tabPageTravel.Controls.Add(this.userControlStatsTimeTravel);
            this.tabPageTravel.Location = new System.Drawing.Point(4, 22);
            this.tabPageTravel.Name = "tabPageTravel";
            this.tabPageTravel.Size = new System.Drawing.Size(481, 364);
            this.tabPageTravel.TabIndex = 4;
            this.tabPageTravel.Text = "Travel";
            this.tabPageTravel.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTravel
            // 
            this.dataGridViewTravel.AllowUserToAddRows = false;
            this.dataGridViewTravel.AllowUserToDeleteRows = false;
            this.dataGridViewTravel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTravel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTravel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dataGridViewTravel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTravel.Location = new System.Drawing.Point(0, 27);
            this.dataGridViewTravel.Name = "dataGridViewTravel";
            this.dataGridViewTravel.RowHeadersVisible = false;
            this.dataGridViewTravel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewTravel.Size = new System.Drawing.Size(481, 337);
            this.dataGridViewTravel.TabIndex = 3;
            this.dataGridViewTravel.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTravel_ColumnHeaderMouseClick);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 400F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Information";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Item";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.panelGrids);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(481, 364);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // panelGrids
            // 
            this.panelGrids.AutoScroll = true;
            this.panelGrids.Controls.Add(this.dataGridViewGlobalStats);
            this.panelGrids.Controls.Add(this.panelGlobalStatsGrid);
            this.panelGrids.Controls.Add(this.dataGridViewCurrentStats);
            this.panelGrids.Controls.Add(this.panelCurrentSystemGrid);
            this.panelGrids.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGrids.InternalMargin = new System.Windows.Forms.Padding(0);
            this.panelGrids.Location = new System.Drawing.Point(3, 3);
            this.panelGrids.Name = "panelGrids";
            this.panelGrids.Padding = new System.Windows.Forms.Padding(0, 0, 5, 5);
            this.panelGrids.ScrollBarWidth = 20;
            this.panelGrids.Size = new System.Drawing.Size(475, 358);
            this.panelGrids.TabIndex = 4;
            this.panelGrids.VerticalScrollBarDockRight = true;
            // 
            // panelCurrentSystemGrid
            // 
            this.panelCurrentSystemGrid.Controls.Add(this.labelExtCurrentSysGrid);
            this.panelCurrentSystemGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCurrentSystemGrid.Location = new System.Drawing.Point(0, 0);
            this.panelCurrentSystemGrid.Name = "panelCurrentSystemGrid";
            this.panelCurrentSystemGrid.Size = new System.Drawing.Size(470, 27);
            this.panelCurrentSystemGrid.TabIndex = 5;
            // 
            // labelExtCurrentSysGrid
            // 
            this.labelExtCurrentSysGrid.AutoSize = true;
            this.labelExtCurrentSysGrid.Location = new System.Drawing.Point(5, 7);
            this.labelExtCurrentSysGrid.Name = "labelExtCurrentSysGrid";
            this.labelExtCurrentSysGrid.Size = new System.Drawing.Size(133, 13);
            this.labelExtCurrentSysGrid.TabIndex = 0;
            this.labelExtCurrentSysGrid.Text = "Current system information:";
            // 
            // dataGridViewCurrentStats
            // 
            this.dataGridViewCurrentStats.AllowUserToAddRows = false;
            this.dataGridViewCurrentStats.AllowUserToDeleteRows = false;
            this.dataGridViewCurrentStats.AllowUserToResizeRows = false;
            this.dataGridViewCurrentStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCurrentStats.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewCurrentStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCurrentStats.ColumnHeadersVisible = false;
            this.dataGridViewCurrentStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemName,
            this.Information});
            this.dataGridViewCurrentStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewCurrentStats.Location = new System.Drawing.Point(0, 27);
            this.dataGridViewCurrentStats.Name = "dataGridViewCurrentStats";
            this.dataGridViewCurrentStats.ReadOnly = true;
            this.dataGridViewCurrentStats.RowHeadersVisible = false;
            this.dataGridViewCurrentStats.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridViewCurrentStats.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCurrentStats.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridViewCurrentStats.Size = new System.Drawing.Size(470, 93);
            this.dataGridViewCurrentStats.TabIndex = 2;
            this.dataGridViewCurrentStats.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewStats_ColumnHeaderMouseClick);
            // 
            // Information
            // 
            this.Information.FillWeight = 237.5635F;
            this.Information.HeaderText = "Information";
            this.Information.MinimumWidth = 50;
            this.Information.Name = "Information";
            this.Information.ReadOnly = true;
            // 
            // ItemName
            // 
            this.ItemName.FillWeight = 162.4366F;
            this.ItemName.HeaderText = "Item";
            this.ItemName.MinimumWidth = 50;
            this.ItemName.Name = "ItemName";
            this.ItemName.ReadOnly = true;
            // 
            // panelGlobalStatsGrid
            // 
            this.panelGlobalStatsGrid.Controls.Add(this.labelExtGlobalStatsGrid);
            this.panelGlobalStatsGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelGlobalStatsGrid.Location = new System.Drawing.Point(0, 120);
            this.panelGlobalStatsGrid.Name = "panelGlobalStatsGrid";
            this.panelGlobalStatsGrid.Size = new System.Drawing.Size(470, 27);
            this.panelGlobalStatsGrid.TabIndex = 6;
            // 
            // labelExtGlobalStatsGrid
            // 
            this.labelExtGlobalStatsGrid.AutoSize = true;
            this.labelExtGlobalStatsGrid.Location = new System.Drawing.Point(5, 7);
            this.labelExtGlobalStatsGrid.Name = "labelExtGlobalStatsGrid";
            this.labelExtGlobalStatsGrid.Size = new System.Drawing.Size(83, 13);
            this.labelExtGlobalStatsGrid.TabIndex = 1;
            this.labelExtGlobalStatsGrid.Text = "Global statistics:";
            // 
            // dataGridViewGlobalStats
            // 
            this.dataGridViewGlobalStats.AllowUserToAddRows = false;
            this.dataGridViewGlobalStats.AllowUserToDeleteRows = false;
            this.dataGridViewGlobalStats.AllowUserToResizeRows = false;
            this.dataGridViewGlobalStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewGlobalStats.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dataGridViewGlobalStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGlobalStats.ColumnHeadersVisible = false;
            this.dataGridViewGlobalStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6});
            this.dataGridViewGlobalStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewGlobalStats.Location = new System.Drawing.Point(0, 147);
            this.dataGridViewGlobalStats.Name = "dataGridViewGlobalStats";
            this.dataGridViewGlobalStats.ReadOnly = true;
            this.dataGridViewGlobalStats.RowHeadersVisible = false;
            this.dataGridViewGlobalStats.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewGlobalStats.Size = new System.Drawing.Size(470, 206);
            this.dataGridViewGlobalStats.TabIndex = 3;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 109.1128F;
            this.dataGridViewTextBoxColumn6.HeaderText = "Information";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 60;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 35.15044F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Item";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 40;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // tabControlCustomStats
            // 
            this.tabControlCustomStats.AllowDragReorder = false;
            this.tabControlCustomStats.Controls.Add(this.tabPageGeneral);
            this.tabControlCustomStats.Controls.Add(this.tabPageTravel);
            this.tabControlCustomStats.Controls.Add(this.tabPageScan);
            this.tabControlCustomStats.Controls.Add(this.tabPageGraph);
            this.tabControlCustomStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCustomStats.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tabControlCustomStats.Location = new System.Drawing.Point(0, 0);
            this.tabControlCustomStats.Name = "tabControlCustomStats";
            this.tabControlCustomStats.SelectedIndex = 0;
            this.tabControlCustomStats.Size = new System.Drawing.Size(489, 390);
            this.tabControlCustomStats.TabColorScaling = 0.5F;
            this.tabControlCustomStats.TabControlBorderBrightColor = System.Drawing.Color.LightGray;
            this.tabControlCustomStats.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.tabControlCustomStats.TabDisabledScaling = 0.5F;
            this.tabControlCustomStats.TabIndex = 5;
            this.tabControlCustomStats.TabMouseOverColor = System.Drawing.Color.White;
            this.tabControlCustomStats.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.tabControlCustomStats.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.tabControlCustomStats.TabOpaque = 100F;
            this.tabControlCustomStats.TabSelectedColor = System.Drawing.Color.LightGray;
            this.tabControlCustomStats.TabStyle = tabStyleSquare1;
            this.tabControlCustomStats.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.tabControlCustomStats.TextSelectedColor = System.Drawing.SystemColors.ControlText;
            // 
            // userControlStatsTimeTravel
            // 
            this.userControlStatsTimeTravel.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlStatsTimeTravel.DrawMode = EDDiscovery.UserControls.UserControlStatsDrawModeEnum.Graph;
            this.userControlStatsTimeTravel.Location = new System.Drawing.Point(0, 0);
            this.userControlStatsTimeTravel.Name = "userControlStatsTimeTravel";
            this.userControlStatsTimeTravel.Size = new System.Drawing.Size(481, 27);
            this.userControlStatsTimeTravel.Stars = false;
            this.userControlStatsTimeTravel.TabIndex = 0;
            this.userControlStatsTimeTravel.TimeMode = EDDiscovery.UserControls.UserControlStatsTimeModeEnum.Summary;
            this.userControlStatsTimeTravel.TimeModeChanged += new System.EventHandler(this.userControlStatsTimeTravel_TimeModeChanged);
            this.userControlStatsTimeTravel.DrawModeChanged += new System.EventHandler(this.userControlStatsTimeTravel_DrawModeChanged);
            // 
            // userControlStatsTimeScan
            // 
            this.userControlStatsTimeScan.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlStatsTimeScan.DrawMode = EDDiscovery.UserControls.UserControlStatsDrawModeEnum.Graph;
            this.userControlStatsTimeScan.Location = new System.Drawing.Point(0, 0);
            this.userControlStatsTimeScan.Name = "userControlStatsTimeScan";
            this.userControlStatsTimeScan.Size = new System.Drawing.Size(481, 27);
            this.userControlStatsTimeScan.Stars = false;
            this.userControlStatsTimeScan.TabIndex = 1;
            this.userControlStatsTimeScan.TimeMode = EDDiscovery.UserControls.UserControlStatsTimeModeEnum.Summary;
            this.userControlStatsTimeScan.TimeModeChanged += new System.EventHandler(this.userControlStatsTimeScan_TimeModeChanged);
            // 
            // UserControlStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlCustomStats);
            this.Name = "UserControlStats";
            this.Size = new System.Drawing.Size(489, 390);
            this.tabPageGraph.ResumeLayout(false);
            this.panelGraphs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mostVisited)).EndInit();
            this.tabPageScan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).EndInit();
            this.tabPageTravel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).EndInit();
            this.tabPageGeneral.ResumeLayout(false);
            this.panelGrids.ResumeLayout(false);
            this.panelCurrentSystemGrid.ResumeLayout(false);
            this.panelCurrentSystemGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCurrentStats)).EndInit();
            this.panelGlobalStatsGrid.ResumeLayout(false);
            this.panelGlobalStatsGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGlobalStats)).EndInit();
            this.tabControlCustomStats.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPageGraph;
        private ExtendedControls.PanelVScroll panelGraphs;
        private System.Windows.Forms.DataVisualization.Charting.Chart mostVisited;
        private System.Windows.Forms.TabPage tabPageScan;
        private System.Windows.Forms.DataGridView dataGridViewScan;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private UserControlStatsTime userControlStatsTimeScan;
        private System.Windows.Forms.TabPage tabPageTravel;
        private System.Windows.Forms.DataGridView dataGridViewTravel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private UserControlStatsTime userControlStatsTimeTravel;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private ExtendedControls.PanelVScroll panelGrids;
        private System.Windows.Forms.DataGridView dataGridViewGlobalStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.Panel panelGlobalStatsGrid;
        private ExtendedControls.LabelExt labelExtGlobalStatsGrid;
        private System.Windows.Forms.DataGridView dataGridViewCurrentStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Information;
        private System.Windows.Forms.Panel panelCurrentSystemGrid;
        private ExtendedControls.LabelExt labelExtCurrentSysGrid;
        private ExtendedControls.TabControlCustom tabControlCustomStats;
    }
}
