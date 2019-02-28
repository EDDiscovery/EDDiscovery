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
            this.dataGridViewStats = new System.Windows.Forms.DataGridView();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Information = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mostVisited = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelData = new ExtendedControls.ExtPanelScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.tabControlCustomStats = new ExtendedControls.ExtTabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabPageTravel = new System.Windows.Forms.TabPage();
            this.dataGridViewTravel = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlStatsTimeTravel = new EDDiscovery.UserControls.StatsTimeUserControl();
            this.tabPageScan = new System.Windows.Forms.TabPage();
            this.dataGridViewScan = new System.Windows.Forms.DataGridView();
            this.ScanNotUsed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlStatsTimeScan = new EDDiscovery.UserControls.StatsTimeUserControl();
            this.tabPageGameStats = new System.Windows.Forms.TabPage();
            this.treeViewStats = new ExtendedControls.Controls.ExtTreeView();
            this.tabPageByShip = new System.Windows.Forms.TabPage();
            this.dataGridViewByShip = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mostVisited)).BeginInit();
            this.panelData.SuspendLayout();
            this.tabControlCustomStats.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageTravel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).BeginInit();
            this.tabPageScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).BeginInit();
            this.tabPageGameStats.SuspendLayout();
            this.tabPageByShip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewByShip)).BeginInit();
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
            this.panelData.Location = new System.Drawing.Point(3, 3);
            this.panelData.Name = "panelData";
            this.panelData.ScrollBarWidth = 20;
            this.panelData.Size = new System.Drawing.Size(784, 720);
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
            this.vScrollBarCustom.Location = new System.Drawing.Point(764, 0);
            this.vScrollBarCustom.Maximum = -202;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(20, 720);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 8;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -202;
            this.vScrollBarCustom.ValueLimited = -202;
            // 
            // tabControlCustomStats
            // 
            this.tabControlCustomStats.AllowDragReorder = false;
            this.tabControlCustomStats.Controls.Add(this.tabPageGeneral);
            this.tabControlCustomStats.Controls.Add(this.tabPageTravel);
            this.tabControlCustomStats.Controls.Add(this.tabPageScan);
            this.tabControlCustomStats.Controls.Add(this.tabPageGameStats);
            this.tabControlCustomStats.Controls.Add(this.tabPageByShip);
            this.tabControlCustomStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCustomStats.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tabControlCustomStats.Location = new System.Drawing.Point(0, 0);
            this.tabControlCustomStats.Name = "tabControlCustomStats";
            this.tabControlCustomStats.SelectedIndex = 0;
            this.tabControlCustomStats.Size = new System.Drawing.Size(798, 752);
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
            this.tabControlCustomStats.SelectedIndexChanged += new System.EventHandler(this.tabControlCustomStats_SelectedIndexChanged);
            // 
            // tabPageGeneral
            // 
            this.tabPageGeneral.Controls.Add(this.panelData);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(790, 726);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // tabPageTravel
            // 
            this.tabPageTravel.Controls.Add(this.dataGridViewTravel);
            this.tabPageTravel.Controls.Add(this.userControlStatsTimeTravel);
            this.tabPageTravel.Location = new System.Drawing.Point(4, 22);
            this.tabPageTravel.Name = "tabPageTravel";
            this.tabPageTravel.Size = new System.Drawing.Size(790, 726);
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
            this.dataGridViewTravel.Location = new System.Drawing.Point(0, 28);
            this.dataGridViewTravel.Name = "dataGridViewTravel";
            this.dataGridViewTravel.RowHeadersVisible = false;
            this.dataGridViewTravel.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewTravel.Size = new System.Drawing.Size(790, 698);
            this.dataGridViewTravel.TabIndex = 3;
            this.dataGridViewTravel.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewTravel_SortCompare);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Item";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 400F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Information";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // userControlStatsTimeTravel
            // 
            this.userControlStatsTimeTravel.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlStatsTimeTravel.Location = new System.Drawing.Point(0, 0);
            this.userControlStatsTimeTravel.Name = "userControlStatsTimeTravel";
            this.userControlStatsTimeTravel.Size = new System.Drawing.Size(790, 28);
            this.userControlStatsTimeTravel.TabIndex = 0;
            this.userControlStatsTimeTravel.TimeMode = EDDiscovery.UserControls.StatsTimeUserControl.TimeModeType.Summary;
            this.userControlStatsTimeTravel.TimeModeChanged += new System.EventHandler(this.userControlStatsTimeTravel_TimeModeChanged);
            // 
            // tabPageScan
            // 
            this.tabPageScan.Controls.Add(this.dataGridViewScan);
            this.tabPageScan.Controls.Add(this.userControlStatsTimeScan);
            this.tabPageScan.Location = new System.Drawing.Point(4, 22);
            this.tabPageScan.Name = "tabPageScan";
            this.tabPageScan.Size = new System.Drawing.Size(790, 726);
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
            this.ScanNotUsed});
            this.dataGridViewScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewScan.Location = new System.Drawing.Point(0, 28);
            this.dataGridViewScan.Name = "dataGridViewScan";
            this.dataGridViewScan.RowHeadersVisible = false;
            this.dataGridViewScan.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataGridViewScan.Size = new System.Drawing.Size(790, 698);
            this.dataGridViewScan.TabIndex = 4;
            this.dataGridViewScan.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewScan_SortCompare);
            // 
            // ScanNotUsed
            // 
            this.ScanNotUsed.HeaderText = "Column1";
            this.ScanNotUsed.Name = "ScanNotUsed";
            // 
            // userControlStatsTimeScan
            // 
            this.userControlStatsTimeScan.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlStatsTimeScan.Location = new System.Drawing.Point(0, 0);
            this.userControlStatsTimeScan.Name = "userControlStatsTimeScan";
            this.userControlStatsTimeScan.Size = new System.Drawing.Size(790, 28);
            this.userControlStatsTimeScan.TabIndex = 1;
            this.userControlStatsTimeScan.TimeMode = EDDiscovery.UserControls.StatsTimeUserControl.TimeModeType.Summary;
            this.userControlStatsTimeScan.TimeModeChanged += new System.EventHandler(this.userControlStatsTimeScan_TimeModeChanged);
            // 
            // tabPageGameStats
            // 
            this.tabPageGameStats.Controls.Add(this.treeViewStats);
            this.tabPageGameStats.Location = new System.Drawing.Point(4, 22);
            this.tabPageGameStats.Name = "tabPageGameStats";
            this.tabPageGameStats.Size = new System.Drawing.Size(790, 726);
            this.tabPageGameStats.TabIndex = 5;
            this.tabPageGameStats.Text = "In Game";
            this.tabPageGameStats.UseVisualStyleBackColor = true;
            // 
            // treeViewStats
            // 
            this.treeViewStats.BorderColor = System.Drawing.Color.Transparent;
            this.treeViewStats.BorderColorScaling = 0.5F;
            this.treeViewStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewStats.HideScrollBar = true;
            this.treeViewStats.Location = new System.Drawing.Point(0, 0);
            this.treeViewStats.Name = "treeViewStats";
            this.treeViewStats.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.treeViewStats.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.treeViewStats.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.treeViewStats.ScrollBarBorderColor = System.Drawing.Color.White;
            this.treeViewStats.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.treeViewStats.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.treeViewStats.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.treeViewStats.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.treeViewStats.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.treeViewStats.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.treeViewStats.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.treeViewStats.ScrollBarWidth = 20;
            this.treeViewStats.ShowLineCount = false;
            this.treeViewStats.ShowLines = true;
            this.treeViewStats.ShowPlusMinus = true;
            this.treeViewStats.ShowRootLines = true;
            this.treeViewStats.Size = new System.Drawing.Size(790, 726);
            this.treeViewStats.TabIndex = 0;
            this.treeViewStats.TreeViewBackColor = System.Drawing.SystemColors.Control;
            this.treeViewStats.TreeViewForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // tabPageByShip
            // 
            this.tabPageByShip.Controls.Add(this.dataGridViewByShip);
            this.tabPageByShip.Location = new System.Drawing.Point(4, 22);
            this.tabPageByShip.Name = "tabPageByShip";
            this.tabPageByShip.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByShip.Size = new System.Drawing.Size(790, 726);
            this.tabPageByShip.TabIndex = 6;
            this.tabPageByShip.Text = "By Ship";
            this.tabPageByShip.UseVisualStyleBackColor = true;
            // 
            // dataGridViewByShip
            // 
            this.dataGridViewByShip.AllowUserToAddRows = false;
            this.dataGridViewByShip.AllowUserToDeleteRows = false;
            this.dataGridViewByShip.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewByShip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewByShip.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dataGridViewByShip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewByShip.Location = new System.Drawing.Point(3, 3);
            this.dataGridViewByShip.Name = "dataGridViewByShip";
            this.dataGridViewByShip.RowHeadersVisible = false;
            this.dataGridViewByShip.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewByShip.Size = new System.Drawing.Size(784, 720);
            this.dataGridViewByShip.TabIndex = 3;
            this.dataGridViewByShip.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewByShip_SortCompare);
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Item";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 400F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Information";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // UserControlStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlCustomStats);
            this.Name = "UserControlStats";
            this.Size = new System.Drawing.Size(798, 752);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mostVisited)).EndInit();
            this.panelData.ResumeLayout(false);
            this.tabControlCustomStats.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageTravel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).EndInit();
            this.tabPageScan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).EndInit();
            this.tabPageGameStats.ResumeLayout(false);
            this.tabPageByShip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewByShip)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewStats;
        private System.Windows.Forms.DataVisualization.Charting.Chart mostVisited;
        private ExtendedControls.ExtPanelScroll panelData;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Information;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private ExtendedControls.ExtTabControl tabControlCustomStats;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageTravel;
        private System.Windows.Forms.TabPage tabPageScan;
        private StatsTimeUserControl userControlStatsTimeTravel;
        private System.Windows.Forms.DataGridView dataGridViewTravel;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private StatsTimeUserControl userControlStatsTimeScan;
        private System.Windows.Forms.DataGridView dataGridViewScan;
        private System.Windows.Forms.TabPage tabPageGameStats;
        private ExtendedControls.Controls.ExtTreeView treeViewStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn ScanNotUsed;
        private System.Windows.Forms.TabPage tabPageByShip;
        private System.Windows.Forms.DataGridView dataGridViewByShip;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
    }
}
