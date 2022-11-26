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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition1 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition2 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            ExtendedControls.TabStyleSquare tabStyleSquare1 = new ExtendedControls.TabStyleSquare();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition3 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition4 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition5 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition6 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition7 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition8 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition9 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition10 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            this.dataGridViewGeneral = new BaseUtils.DataGridViewBaseEnhancements();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Information = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extChartTravelDest = new ExtendedControls.ExtSafeChart();
            this.vScrollBarGeneral = new ExtendedControls.ExtScrollBar();
            this.tabControlCustomStats = new ExtendedControls.ExtTabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.splitContainerGeneral = new System.Windows.Forms.SplitContainer();
            this.extPanelDataGridViewScrollGeneral = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.tabPageRanks = new System.Windows.Forms.TabPage();
            this.extPanelDataGridViewScrollRanks = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewRanks = new BaseUtils.DataGridViewBaseEnhancements();
            this.extScrollBarRanks = new ExtendedControls.ExtScrollBar();
            this.tabPageLedger = new System.Windows.Forms.TabPage();
            this.splitContainerLedger = new System.Windows.Forms.SplitContainer();
            this.extPanelDataGridViewScrollLedger = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewLedger = new BaseUtils.DataGridViewBaseEnhancements();
            this.dataGridViewTextBoxColumnLedgerDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnNumericCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extScrollBarLedger = new ExtendedControls.ExtScrollBar();
            this.extChartLedger = new ExtendedControls.ExtSafeChart();
            this.tabPageTravel = new System.Windows.Forms.TabPage();
            this.extPanelDataGridViewScrollTravel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarTravel = new ExtendedControls.ExtScrollBar();
            this.dataGridViewTravel = new BaseUtils.DataGridViewColumnControl();
            this.userControlStatsTimeTravel = new EDDiscovery.UserControls.StatsTimeUserControl();
            this.tabPageScan = new System.Windows.Forms.TabPage();
            this.splitContainerScan = new System.Windows.Forms.SplitContainer();
            this.extPanelDataGridViewScrollScan = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewScan = new BaseUtils.DataGridViewColumnControl();
            this.extScrollBarScan = new ExtendedControls.ExtScrollBar();
            this.extChartScan = new ExtendedControls.ExtSafeChart();
            this.userControlStatsTimeScan = new EDDiscovery.UserControls.StatsTimeUserControl();
            this.tabPageCombat = new System.Windows.Forms.TabPage();
            this.splitContainerCombat = new System.Windows.Forms.SplitContainer();
            this.extPanelDataGridViewScrollCombat = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarCombatDGV = new ExtendedControls.ExtScrollBar();
            this.dataGridViewCombat = new BaseUtils.DataGridViewColumnControl();
            this.extChartCombat = new ExtendedControls.ExtSafeChart();
            this.statsTimeUserControlCombat = new EDDiscovery.UserControls.StatsTimeUserControl();
            this.tabPageGameStats = new System.Windows.Forms.TabPage();
            this.treeViewStats = new ExtendedControls.Controls.ExtTreeView();
            this.tabPageByShip = new System.Windows.Forms.TabPage();
            this.splitContainerShips = new System.Windows.Forms.SplitContainer();
            this.extPanelDataGridViewScrollByShip = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewByShip = new BaseUtils.DataGridViewColumnControl();
            this.extScrollBarByShip = new ExtendedControls.ExtScrollBar();
            this.extChartShips = new ExtendedControls.ExtSafeChart();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerStartDate = new ExtendedControls.ExtDateTimePicker();
            this.labelStatus = new System.Windows.Forms.Label();
            this.extPanelRollUp = new ExtendedControls.ExtPanelRollUp();
            this.dateTimePickerEndDate = new ExtendedControls.ExtDateTimePicker();
            this.labelStart = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumnRank = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnAtStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxAtEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextColumnRankProgressNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnLastPromotionDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).BeginInit();
            this.tabControlCustomStats.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGeneral)).BeginInit();
            this.splitContainerGeneral.Panel1.SuspendLayout();
            this.splitContainerGeneral.Panel2.SuspendLayout();
            this.splitContainerGeneral.SuspendLayout();
            this.extPanelDataGridViewScrollGeneral.SuspendLayout();
            this.tabPageRanks.SuspendLayout();
            this.extPanelDataGridViewScrollRanks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRanks)).BeginInit();
            this.tabPageLedger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLedger)).BeginInit();
            this.splitContainerLedger.Panel1.SuspendLayout();
            this.splitContainerLedger.Panel2.SuspendLayout();
            this.splitContainerLedger.SuspendLayout();
            this.extPanelDataGridViewScrollLedger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).BeginInit();
            this.tabPageTravel.SuspendLayout();
            this.extPanelDataGridViewScrollTravel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).BeginInit();
            this.tabPageScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerScan)).BeginInit();
            this.splitContainerScan.Panel1.SuspendLayout();
            this.splitContainerScan.Panel2.SuspendLayout();
            this.splitContainerScan.SuspendLayout();
            this.extPanelDataGridViewScrollScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).BeginInit();
            this.tabPageCombat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCombat)).BeginInit();
            this.splitContainerCombat.Panel1.SuspendLayout();
            this.splitContainerCombat.Panel2.SuspendLayout();
            this.splitContainerCombat.SuspendLayout();
            this.extPanelDataGridViewScrollCombat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCombat)).BeginInit();
            this.tabPageGameStats.SuspendLayout();
            this.tabPageByShip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerShips)).BeginInit();
            this.splitContainerShips.Panel1.SuspendLayout();
            this.splitContainerShips.Panel2.SuspendLayout();
            this.splitContainerShips.SuspendLayout();
            this.extPanelDataGridViewScrollByShip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewByShip)).BeginInit();
            this.extPanelRollUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewGeneral
            // 
            this.dataGridViewGeneral.AllowUserToAddRows = false;
            this.dataGridViewGeneral.AllowUserToDeleteRows = false;
            this.dataGridViewGeneral.AllowUserToResizeRows = false;
            this.dataGridViewGeneral.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewGeneral.AutoSortByColumnName = false;
            this.dataGridViewGeneral.ColumnHeaderMenuStrip = null;
            this.dataGridViewGeneral.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGeneral.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemName,
            this.Information});
            this.dataGridViewGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewGeneral.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewGeneral.Name = "dataGridViewGeneral";
            this.dataGridViewGeneral.RowHeaderMenuStrip = null;
            this.dataGridViewGeneral.RowHeadersVisible = false;
            this.dataGridViewGeneral.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewGeneral.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewGeneral.SingleRowSelect = true;
            this.dataGridViewGeneral.Size = new System.Drawing.Size(1043, 300);
            this.dataGridViewGeneral.TabIndex = 2;
            this.dataGridViewGeneral.TopLeftHeaderMenuStrip = null;
            // 
            // ItemName
            // 
            this.ItemName.HeaderText = "Item";
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
            // extChartTravelDest
            // 
            this.extChartTravelDest.AutoScaleYAddedPercent = 5D;
            this.extChartTravelDest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extChartTravelDest.LeftArrowEnable = true;
            this.extChartTravelDest.LeftArrowPosition = elementPosition1;
            this.extChartTravelDest.Location = new System.Drawing.Point(0, 0);
            this.extChartTravelDest.Name = "extChartTravelDest";
            this.extChartTravelDest.RightArrowEnable = true;
            this.extChartTravelDest.RightArrowPosition = elementPosition2;
            this.extChartTravelDest.Size = new System.Drawing.Size(1059, 201);
            this.extChartTravelDest.TabIndex = 9;
            this.extChartTravelDest.ZoomMouseWheelXMinimumInterval = 5D;
            this.extChartTravelDest.ZoomMouseWheelXZoomFactor = 1.5D;
            // 
            // vScrollBarGeneral
            // 
            this.vScrollBarGeneral.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarGeneral.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarGeneral.ArrowColorScaling = 0.5F;
            this.vScrollBarGeneral.ArrowDownDrawAngle = 270F;
            this.vScrollBarGeneral.ArrowUpDrawAngle = 90F;
            this.vScrollBarGeneral.BorderColor = System.Drawing.Color.White;
            this.vScrollBarGeneral.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarGeneral.HideScrollBar = false;
            this.vScrollBarGeneral.LargeChange = 0;
            this.vScrollBarGeneral.Location = new System.Drawing.Point(1043, 0);
            this.vScrollBarGeneral.Maximum = -1;
            this.vScrollBarGeneral.Minimum = 0;
            this.vScrollBarGeneral.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarGeneral.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarGeneral.Name = "vScrollBarGeneral";
            this.vScrollBarGeneral.Size = new System.Drawing.Size(16, 300);
            this.vScrollBarGeneral.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarGeneral.SmallChange = 1;
            this.vScrollBarGeneral.TabIndex = 8;
            this.vScrollBarGeneral.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarGeneral.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarGeneral.ThumbColorScaling = 0.5F;
            this.vScrollBarGeneral.ThumbDrawAngle = 0F;
            this.vScrollBarGeneral.Value = -1;
            this.vScrollBarGeneral.ValueLimited = -1;
            // 
            // tabControlCustomStats
            // 
            this.tabControlCustomStats.AllowDragReorder = false;
            this.tabControlCustomStats.Controls.Add(this.tabPageGeneral);
            this.tabControlCustomStats.Controls.Add(this.tabPageRanks);
            this.tabControlCustomStats.Controls.Add(this.tabPageLedger);
            this.tabControlCustomStats.Controls.Add(this.tabPageTravel);
            this.tabControlCustomStats.Controls.Add(this.tabPageScan);
            this.tabControlCustomStats.Controls.Add(this.tabPageCombat);
            this.tabControlCustomStats.Controls.Add(this.tabPageGameStats);
            this.tabControlCustomStats.Controls.Add(this.tabPageByShip);
            this.tabControlCustomStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlCustomStats.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tabControlCustomStats.Location = new System.Drawing.Point(0, 32);
            this.tabControlCustomStats.Name = "tabControlCustomStats";
            this.tabControlCustomStats.SelectedIndex = 0;
            this.tabControlCustomStats.Size = new System.Drawing.Size(1073, 537);
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
            this.tabPageGeneral.Controls.Add(this.splitContainerGeneral);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(1065, 511);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
            // 
            // splitContainerGeneral
            // 
            this.splitContainerGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerGeneral.Location = new System.Drawing.Point(3, 3);
            this.splitContainerGeneral.Name = "splitContainerGeneral";
            this.splitContainerGeneral.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerGeneral.Panel1
            // 
            this.splitContainerGeneral.Panel1.Controls.Add(this.extPanelDataGridViewScrollGeneral);
            // 
            // splitContainerGeneral.Panel2
            // 
            this.splitContainerGeneral.Panel2.Controls.Add(this.extChartTravelDest);
            this.splitContainerGeneral.Size = new System.Drawing.Size(1059, 505);
            this.splitContainerGeneral.SplitterDistance = 300;
            this.splitContainerGeneral.TabIndex = 9;
            // 
            // extPanelDataGridViewScrollGeneral
            // 
            this.extPanelDataGridViewScrollGeneral.Controls.Add(this.dataGridViewGeneral);
            this.extPanelDataGridViewScrollGeneral.Controls.Add(this.vScrollBarGeneral);
            this.extPanelDataGridViewScrollGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollGeneral.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollGeneral.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollGeneral.Name = "extPanelDataGridViewScrollGeneral";
            this.extPanelDataGridViewScrollGeneral.Size = new System.Drawing.Size(1059, 300);
            this.extPanelDataGridViewScrollGeneral.TabIndex = 10;
            this.extPanelDataGridViewScrollGeneral.VerticalScrollBarDockRight = true;
            // 
            // tabPageRanks
            // 
            this.tabPageRanks.Controls.Add(this.extPanelDataGridViewScrollRanks);
            this.tabPageRanks.Location = new System.Drawing.Point(4, 22);
            this.tabPageRanks.Name = "tabPageRanks";
            this.tabPageRanks.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRanks.Size = new System.Drawing.Size(1065, 511);
            this.tabPageRanks.TabIndex = 9;
            this.tabPageRanks.Text = "Ranks";
            this.tabPageRanks.UseVisualStyleBackColor = true;
            // 
            // extPanelDataGridViewScrollRanks
            // 
            this.extPanelDataGridViewScrollRanks.Controls.Add(this.dataGridViewRanks);
            this.extPanelDataGridViewScrollRanks.Controls.Add(this.extScrollBarRanks);
            this.extPanelDataGridViewScrollRanks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollRanks.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollRanks.Location = new System.Drawing.Point(3, 3);
            this.extPanelDataGridViewScrollRanks.Name = "extPanelDataGridViewScrollRanks";
            this.extPanelDataGridViewScrollRanks.Size = new System.Drawing.Size(1059, 505);
            this.extPanelDataGridViewScrollRanks.TabIndex = 12;
            this.extPanelDataGridViewScrollRanks.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewRanks
            // 
            this.dataGridViewRanks.AllowUserToAddRows = false;
            this.dataGridViewRanks.AllowUserToDeleteRows = false;
            this.dataGridViewRanks.AllowUserToResizeRows = false;
            this.dataGridViewRanks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRanks.AutoSortByColumnName = true;
            this.dataGridViewRanks.ColumnHeaderMenuStrip = null;
            this.dataGridViewRanks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRanks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumnRank,
            this.dataGridViewTextBoxColumnAtStart,
            this.dataGridViewTextBoxAtEnd,
            this.dataGridViewTextColumnRankProgressNumeric,
            this.dataGridViewTextBoxColumnLastPromotionDate});
            this.dataGridViewRanks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRanks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRanks.Name = "dataGridViewRanks";
            this.dataGridViewRanks.RowHeaderMenuStrip = null;
            this.dataGridViewRanks.RowHeadersVisible = false;
            this.dataGridViewRanks.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewRanks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewRanks.SingleRowSelect = true;
            this.dataGridViewRanks.Size = new System.Drawing.Size(1043, 505);
            this.dataGridViewRanks.TabIndex = 2;
            this.dataGridViewRanks.TopLeftHeaderMenuStrip = null;
            // 
            // extScrollBarRanks
            // 
            this.extScrollBarRanks.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarRanks.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarRanks.ArrowColorScaling = 0.5F;
            this.extScrollBarRanks.ArrowDownDrawAngle = 270F;
            this.extScrollBarRanks.ArrowUpDrawAngle = 90F;
            this.extScrollBarRanks.BorderColor = System.Drawing.Color.White;
            this.extScrollBarRanks.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarRanks.HideScrollBar = false;
            this.extScrollBarRanks.LargeChange = 0;
            this.extScrollBarRanks.Location = new System.Drawing.Point(1043, 0);
            this.extScrollBarRanks.Maximum = -1;
            this.extScrollBarRanks.Minimum = 0;
            this.extScrollBarRanks.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarRanks.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarRanks.Name = "extScrollBarRanks";
            this.extScrollBarRanks.Size = new System.Drawing.Size(16, 505);
            this.extScrollBarRanks.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarRanks.SmallChange = 1;
            this.extScrollBarRanks.TabIndex = 8;
            this.extScrollBarRanks.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarRanks.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarRanks.ThumbColorScaling = 0.5F;
            this.extScrollBarRanks.ThumbDrawAngle = 0F;
            this.extScrollBarRanks.Value = -1;
            this.extScrollBarRanks.ValueLimited = -1;
            // 
            // tabPageLedger
            // 
            this.tabPageLedger.Controls.Add(this.splitContainerLedger);
            this.tabPageLedger.Location = new System.Drawing.Point(4, 22);
            this.tabPageLedger.Name = "tabPageLedger";
            this.tabPageLedger.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLedger.Size = new System.Drawing.Size(1065, 511);
            this.tabPageLedger.TabIndex = 8;
            this.tabPageLedger.Text = "Ledger";
            this.tabPageLedger.UseVisualStyleBackColor = true;
            // 
            // splitContainerLedger
            // 
            this.splitContainerLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLedger.Location = new System.Drawing.Point(3, 3);
            this.splitContainerLedger.Name = "splitContainerLedger";
            this.splitContainerLedger.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLedger.Panel1
            // 
            this.splitContainerLedger.Panel1.Controls.Add(this.extPanelDataGridViewScrollLedger);
            // 
            // splitContainerLedger.Panel2
            // 
            this.splitContainerLedger.Panel2.Controls.Add(this.extChartLedger);
            this.splitContainerLedger.Size = new System.Drawing.Size(1059, 505);
            this.splitContainerLedger.SplitterDistance = 300;
            this.splitContainerLedger.TabIndex = 9;
            // 
            // extPanelDataGridViewScrollLedger
            // 
            this.extPanelDataGridViewScrollLedger.Controls.Add(this.dataGridViewLedger);
            this.extPanelDataGridViewScrollLedger.Controls.Add(this.extScrollBarLedger);
            this.extPanelDataGridViewScrollLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollLedger.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollLedger.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollLedger.Name = "extPanelDataGridViewScrollLedger";
            this.extPanelDataGridViewScrollLedger.Size = new System.Drawing.Size(1059, 300);
            this.extPanelDataGridViewScrollLedger.TabIndex = 11;
            this.extPanelDataGridViewScrollLedger.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewLedger
            // 
            this.dataGridViewLedger.AllowUserToAddRows = false;
            this.dataGridViewLedger.AllowUserToDeleteRows = false;
            this.dataGridViewLedger.AllowUserToResizeRows = false;
            this.dataGridViewLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLedger.AutoSortByColumnName = true;
            this.dataGridViewLedger.ColumnHeaderMenuStrip = null;
            this.dataGridViewLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumnLedgerDate,
            this.dataGridViewTextBoxColumnNumericCredits});
            this.dataGridViewLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLedger.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewLedger.Name = "dataGridViewLedger";
            this.dataGridViewLedger.RowHeaderMenuStrip = null;
            this.dataGridViewLedger.RowHeadersVisible = false;
            this.dataGridViewLedger.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLedger.SingleRowSelect = true;
            this.dataGridViewLedger.Size = new System.Drawing.Size(1043, 300);
            this.dataGridViewLedger.TabIndex = 2;
            this.dataGridViewLedger.TopLeftHeaderMenuStrip = null;
            this.dataGridViewLedger.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLedger_CellClick);
            // 
            // dataGridViewTextBoxColumnLedgerDate
            // 
            this.dataGridViewTextBoxColumnLedgerDate.HeaderText = "Date";
            this.dataGridViewTextBoxColumnLedgerDate.Name = "dataGridViewTextBoxColumnLedgerDate";
            this.dataGridViewTextBoxColumnLedgerDate.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnNumericCredits
            // 
            this.dataGridViewTextBoxColumnNumericCredits.FillWeight = 400F;
            this.dataGridViewTextBoxColumnNumericCredits.HeaderText = "Credits";
            this.dataGridViewTextBoxColumnNumericCredits.MinimumWidth = 50;
            this.dataGridViewTextBoxColumnNumericCredits.Name = "dataGridViewTextBoxColumnNumericCredits";
            this.dataGridViewTextBoxColumnNumericCredits.ReadOnly = true;
            // 
            // extScrollBarLedger
            // 
            this.extScrollBarLedger.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarLedger.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarLedger.ArrowColorScaling = 0.5F;
            this.extScrollBarLedger.ArrowDownDrawAngle = 270F;
            this.extScrollBarLedger.ArrowUpDrawAngle = 90F;
            this.extScrollBarLedger.BorderColor = System.Drawing.Color.White;
            this.extScrollBarLedger.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarLedger.HideScrollBar = false;
            this.extScrollBarLedger.LargeChange = 0;
            this.extScrollBarLedger.Location = new System.Drawing.Point(1043, 0);
            this.extScrollBarLedger.Maximum = -1;
            this.extScrollBarLedger.Minimum = 0;
            this.extScrollBarLedger.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarLedger.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarLedger.Name = "extScrollBarLedger";
            this.extScrollBarLedger.Size = new System.Drawing.Size(16, 300);
            this.extScrollBarLedger.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarLedger.SmallChange = 1;
            this.extScrollBarLedger.TabIndex = 8;
            this.extScrollBarLedger.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarLedger.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarLedger.ThumbColorScaling = 0.5F;
            this.extScrollBarLedger.ThumbDrawAngle = 0F;
            this.extScrollBarLedger.Value = -1;
            this.extScrollBarLedger.ValueLimited = -1;
            // 
            // extChartLedger
            // 
            this.extChartLedger.AutoScaleYAddedPercent = 5D;
            this.extChartLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extChartLedger.LeftArrowEnable = true;
            this.extChartLedger.LeftArrowPosition = elementPosition3;
            this.extChartLedger.Location = new System.Drawing.Point(0, 0);
            this.extChartLedger.Name = "extChartLedger";
            this.extChartLedger.RightArrowEnable = true;
            this.extChartLedger.RightArrowPosition = elementPosition4;
            this.extChartLedger.Size = new System.Drawing.Size(1059, 201);
            this.extChartLedger.TabIndex = 10;
            this.extChartLedger.ZoomMouseWheelXMinimumInterval = 5D;
            this.extChartLedger.ZoomMouseWheelXZoomFactor = 1.5D;
            // 
            // tabPageTravel
            // 
            this.tabPageTravel.Controls.Add(this.extPanelDataGridViewScrollTravel);
            this.tabPageTravel.Controls.Add(this.userControlStatsTimeTravel);
            this.tabPageTravel.Location = new System.Drawing.Point(4, 22);
            this.tabPageTravel.Name = "tabPageTravel";
            this.tabPageTravel.Size = new System.Drawing.Size(1065, 511);
            this.tabPageTravel.TabIndex = 4;
            this.tabPageTravel.Text = "Travel";
            this.tabPageTravel.UseVisualStyleBackColor = true;
            // 
            // extPanelDataGridViewScrollTravel
            // 
            this.extPanelDataGridViewScrollTravel.Controls.Add(this.extScrollBarTravel);
            this.extPanelDataGridViewScrollTravel.Controls.Add(this.dataGridViewTravel);
            this.extPanelDataGridViewScrollTravel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollTravel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollTravel.Location = new System.Drawing.Point(0, 26);
            this.extPanelDataGridViewScrollTravel.Name = "extPanelDataGridViewScrollTravel";
            this.extPanelDataGridViewScrollTravel.Size = new System.Drawing.Size(1065, 485);
            this.extPanelDataGridViewScrollTravel.TabIndex = 4;
            this.extPanelDataGridViewScrollTravel.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarTravel
            // 
            this.extScrollBarTravel.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarTravel.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarTravel.ArrowColorScaling = 0.5F;
            this.extScrollBarTravel.ArrowDownDrawAngle = 270F;
            this.extScrollBarTravel.ArrowUpDrawAngle = 90F;
            this.extScrollBarTravel.BorderColor = System.Drawing.Color.White;
            this.extScrollBarTravel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarTravel.HideScrollBar = false;
            this.extScrollBarTravel.LargeChange = 0;
            this.extScrollBarTravel.Location = new System.Drawing.Point(1049, 0);
            this.extScrollBarTravel.Maximum = -1;
            this.extScrollBarTravel.Minimum = 0;
            this.extScrollBarTravel.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarTravel.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarTravel.Name = "extScrollBarTravel";
            this.extScrollBarTravel.Size = new System.Drawing.Size(16, 485);
            this.extScrollBarTravel.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarTravel.SmallChange = 1;
            this.extScrollBarTravel.TabIndex = 4;
            this.extScrollBarTravel.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarTravel.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarTravel.ThumbColorScaling = 0.5F;
            this.extScrollBarTravel.ThumbDrawAngle = 0F;
            this.extScrollBarTravel.Value = -1;
            this.extScrollBarTravel.ValueLimited = -1;
            // 
            // dataGridViewTravel
            // 
            this.dataGridViewTravel.AllowUserToAddRows = false;
            this.dataGridViewTravel.AllowUserToDeleteRows = false;
            this.dataGridViewTravel.AllowUserToResizeRows = false;
            this.dataGridViewTravel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTravel.AutoSortByColumnName = true;
            this.dataGridViewTravel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTravel.ColumnReorder = true;
            this.dataGridViewTravel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTravel.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTravel.Name = "dataGridViewTravel";
            this.dataGridViewTravel.PerColumnWordWrapControl = true;
            this.dataGridViewTravel.RowHeaderMenuStrip = null;
            this.dataGridViewTravel.RowHeadersVisible = false;
            this.dataGridViewTravel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewTravel.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTravel.SingleRowSelect = true;
            this.dataGridViewTravel.Size = new System.Drawing.Size(1049, 485);
            this.dataGridViewTravel.TabIndex = 3;
            // 
            // userControlStatsTimeTravel
            // 
            this.userControlStatsTimeTravel.AutoSize = true;
            this.userControlStatsTimeTravel.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlStatsTimeTravel.Location = new System.Drawing.Point(0, 0);
            this.userControlStatsTimeTravel.Name = "userControlStatsTimeTravel";
            this.userControlStatsTimeTravel.Size = new System.Drawing.Size(1065, 26);
            this.userControlStatsTimeTravel.TabIndex = 0;
            this.userControlStatsTimeTravel.TimeMode = EDDiscovery.UserControls.StatsTimeUserControl.TimeModeType.Summary;
            // 
            // tabPageScan
            // 
            this.tabPageScan.Controls.Add(this.splitContainerScan);
            this.tabPageScan.Controls.Add(this.userControlStatsTimeScan);
            this.tabPageScan.Location = new System.Drawing.Point(4, 22);
            this.tabPageScan.Name = "tabPageScan";
            this.tabPageScan.Size = new System.Drawing.Size(1065, 511);
            this.tabPageScan.TabIndex = 1;
            this.tabPageScan.Text = "Scan";
            this.tabPageScan.UseVisualStyleBackColor = true;
            // 
            // splitContainerScan
            // 
            this.splitContainerScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerScan.Location = new System.Drawing.Point(0, 26);
            this.splitContainerScan.Name = "splitContainerScan";
            this.splitContainerScan.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerScan.Panel1
            // 
            this.splitContainerScan.Panel1.Controls.Add(this.extPanelDataGridViewScrollScan);
            // 
            // splitContainerScan.Panel2
            // 
            this.splitContainerScan.Panel2.Controls.Add(this.extChartScan);
            this.splitContainerScan.Size = new System.Drawing.Size(1065, 485);
            this.splitContainerScan.SplitterDistance = 238;
            this.splitContainerScan.TabIndex = 10;
            // 
            // extPanelDataGridViewScrollScan
            // 
            this.extPanelDataGridViewScrollScan.Controls.Add(this.dataGridViewScan);
            this.extPanelDataGridViewScrollScan.Controls.Add(this.extScrollBarScan);
            this.extPanelDataGridViewScrollScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollScan.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollScan.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollScan.Name = "extPanelDataGridViewScrollScan";
            this.extPanelDataGridViewScrollScan.Size = new System.Drawing.Size(1065, 238);
            this.extPanelDataGridViewScrollScan.TabIndex = 5;
            this.extPanelDataGridViewScrollScan.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewScan
            // 
            this.dataGridViewScan.AllowUserToAddRows = false;
            this.dataGridViewScan.AllowUserToDeleteRows = false;
            this.dataGridViewScan.AllowUserToResizeRows = false;
            this.dataGridViewScan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewScan.AutoSortByColumnName = true;
            this.dataGridViewScan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScan.ColumnReorder = true;
            this.dataGridViewScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewScan.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewScan.Name = "dataGridViewScan";
            this.dataGridViewScan.PerColumnWordWrapControl = true;
            this.dataGridViewScan.RowHeaderMenuStrip = null;
            this.dataGridViewScan.RowHeadersVisible = false;
            this.dataGridViewScan.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewScan.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewScan.SingleRowSelect = true;
            this.dataGridViewScan.Size = new System.Drawing.Size(1049, 238);
            this.dataGridViewScan.TabIndex = 4;
            // 
            // extScrollBarScan
            // 
            this.extScrollBarScan.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarScan.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarScan.ArrowColorScaling = 0.5F;
            this.extScrollBarScan.ArrowDownDrawAngle = 270F;
            this.extScrollBarScan.ArrowUpDrawAngle = 90F;
            this.extScrollBarScan.BorderColor = System.Drawing.Color.White;
            this.extScrollBarScan.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarScan.HideScrollBar = false;
            this.extScrollBarScan.LargeChange = 0;
            this.extScrollBarScan.Location = new System.Drawing.Point(1049, 0);
            this.extScrollBarScan.Maximum = -1;
            this.extScrollBarScan.Minimum = 0;
            this.extScrollBarScan.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarScan.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarScan.Name = "extScrollBarScan";
            this.extScrollBarScan.Size = new System.Drawing.Size(16, 238);
            this.extScrollBarScan.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarScan.SmallChange = 1;
            this.extScrollBarScan.TabIndex = 0;
            this.extScrollBarScan.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarScan.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarScan.ThumbColorScaling = 0.5F;
            this.extScrollBarScan.ThumbDrawAngle = 0F;
            this.extScrollBarScan.Value = -1;
            this.extScrollBarScan.ValueLimited = -1;
            // 
            // extChartScan
            // 
            this.extChartScan.AutoScaleYAddedPercent = 5D;
            this.extChartScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extChartScan.LeftArrowEnable = true;
            this.extChartScan.LeftArrowPosition = elementPosition5;
            this.extChartScan.Location = new System.Drawing.Point(0, 0);
            this.extChartScan.Name = "extChartScan";
            this.extChartScan.RightArrowEnable = true;
            this.extChartScan.RightArrowPosition = elementPosition6;
            this.extChartScan.Size = new System.Drawing.Size(1065, 243);
            this.extChartScan.TabIndex = 0;
            this.extChartScan.ZoomMouseWheelXMinimumInterval = 5D;
            this.extChartScan.ZoomMouseWheelXZoomFactor = 1.5D;
            // 
            // userControlStatsTimeScan
            // 
            this.userControlStatsTimeScan.AutoSize = true;
            this.userControlStatsTimeScan.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlStatsTimeScan.Location = new System.Drawing.Point(0, 0);
            this.userControlStatsTimeScan.Name = "userControlStatsTimeScan";
            this.userControlStatsTimeScan.Size = new System.Drawing.Size(1065, 26);
            this.userControlStatsTimeScan.TabIndex = 1;
            this.userControlStatsTimeScan.TimeMode = EDDiscovery.UserControls.StatsTimeUserControl.TimeModeType.Summary;
            // 
            // tabPageCombat
            // 
            this.tabPageCombat.Controls.Add(this.splitContainerCombat);
            this.tabPageCombat.Controls.Add(this.statsTimeUserControlCombat);
            this.tabPageCombat.Location = new System.Drawing.Point(4, 22);
            this.tabPageCombat.Name = "tabPageCombat";
            this.tabPageCombat.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCombat.Size = new System.Drawing.Size(1065, 511);
            this.tabPageCombat.TabIndex = 7;
            this.tabPageCombat.Text = "Combat";
            this.tabPageCombat.UseVisualStyleBackColor = true;
            // 
            // splitContainerCombat
            // 
            this.splitContainerCombat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCombat.Location = new System.Drawing.Point(3, 29);
            this.splitContainerCombat.Name = "splitContainerCombat";
            this.splitContainerCombat.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerCombat.Panel1
            // 
            this.splitContainerCombat.Panel1.Controls.Add(this.extPanelDataGridViewScrollCombat);
            // 
            // splitContainerCombat.Panel2
            // 
            this.splitContainerCombat.Panel2.Controls.Add(this.extChartCombat);
            this.splitContainerCombat.Size = new System.Drawing.Size(1059, 479);
            this.splitContainerCombat.SplitterDistance = 239;
            this.splitContainerCombat.TabIndex = 7;
            // 
            // extPanelDataGridViewScrollCombat
            // 
            this.extPanelDataGridViewScrollCombat.Controls.Add(this.extScrollBarCombatDGV);
            this.extPanelDataGridViewScrollCombat.Controls.Add(this.dataGridViewCombat);
            this.extPanelDataGridViewScrollCombat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollCombat.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollCombat.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollCombat.Name = "extPanelDataGridViewScrollCombat";
            this.extPanelDataGridViewScrollCombat.Size = new System.Drawing.Size(1059, 239);
            this.extPanelDataGridViewScrollCombat.TabIndex = 6;
            this.extPanelDataGridViewScrollCombat.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarCombatDGV
            // 
            this.extScrollBarCombatDGV.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarCombatDGV.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarCombatDGV.ArrowColorScaling = 0.5F;
            this.extScrollBarCombatDGV.ArrowDownDrawAngle = 270F;
            this.extScrollBarCombatDGV.ArrowUpDrawAngle = 90F;
            this.extScrollBarCombatDGV.BorderColor = System.Drawing.Color.White;
            this.extScrollBarCombatDGV.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarCombatDGV.HideScrollBar = false;
            this.extScrollBarCombatDGV.LargeChange = 0;
            this.extScrollBarCombatDGV.Location = new System.Drawing.Point(1043, 0);
            this.extScrollBarCombatDGV.Maximum = -1;
            this.extScrollBarCombatDGV.Minimum = 0;
            this.extScrollBarCombatDGV.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarCombatDGV.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarCombatDGV.Name = "extScrollBarCombatDGV";
            this.extScrollBarCombatDGV.Size = new System.Drawing.Size(16, 239);
            this.extScrollBarCombatDGV.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarCombatDGV.SmallChange = 1;
            this.extScrollBarCombatDGV.TabIndex = 6;
            this.extScrollBarCombatDGV.Text = "extScrollBar1";
            this.extScrollBarCombatDGV.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarCombatDGV.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarCombatDGV.ThumbColorScaling = 0.5F;
            this.extScrollBarCombatDGV.ThumbDrawAngle = 0F;
            this.extScrollBarCombatDGV.Value = -1;
            this.extScrollBarCombatDGV.ValueLimited = -1;
            // 
            // dataGridViewCombat
            // 
            this.dataGridViewCombat.AllowUserToAddRows = false;
            this.dataGridViewCombat.AllowUserToDeleteRows = false;
            this.dataGridViewCombat.AllowUserToResizeRows = false;
            this.dataGridViewCombat.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCombat.AutoSortByColumnName = true;
            this.dataGridViewCombat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCombat.ColumnReorder = true;
            this.dataGridViewCombat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCombat.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCombat.Name = "dataGridViewCombat";
            this.dataGridViewCombat.PerColumnWordWrapControl = true;
            this.dataGridViewCombat.RowHeaderMenuStrip = null;
            this.dataGridViewCombat.RowHeadersVisible = false;
            this.dataGridViewCombat.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCombat.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCombat.SingleRowSelect = true;
            this.dataGridViewCombat.Size = new System.Drawing.Size(1043, 239);
            this.dataGridViewCombat.TabIndex = 5;
            // 
            // extChartCombat
            // 
            this.extChartCombat.AutoScaleYAddedPercent = 5D;
            this.extChartCombat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extChartCombat.LeftArrowEnable = true;
            this.extChartCombat.LeftArrowPosition = elementPosition7;
            this.extChartCombat.Location = new System.Drawing.Point(0, 0);
            this.extChartCombat.Name = "extChartCombat";
            this.extChartCombat.RightArrowEnable = true;
            this.extChartCombat.RightArrowPosition = elementPosition8;
            this.extChartCombat.Size = new System.Drawing.Size(1059, 236);
            this.extChartCombat.TabIndex = 1;
            this.extChartCombat.ZoomMouseWheelXMinimumInterval = 5D;
            this.extChartCombat.ZoomMouseWheelXZoomFactor = 1.5D;
            // 
            // statsTimeUserControlCombat
            // 
            this.statsTimeUserControlCombat.AutoSize = true;
            this.statsTimeUserControlCombat.Dock = System.Windows.Forms.DockStyle.Top;
            this.statsTimeUserControlCombat.Location = new System.Drawing.Point(3, 3);
            this.statsTimeUserControlCombat.Name = "statsTimeUserControlCombat";
            this.statsTimeUserControlCombat.Size = new System.Drawing.Size(1059, 26);
            this.statsTimeUserControlCombat.TabIndex = 4;
            this.statsTimeUserControlCombat.TimeMode = EDDiscovery.UserControls.StatsTimeUserControl.TimeModeType.Summary;
            // 
            // tabPageGameStats
            // 
            this.tabPageGameStats.Controls.Add(this.treeViewStats);
            this.tabPageGameStats.Location = new System.Drawing.Point(4, 22);
            this.tabPageGameStats.Name = "tabPageGameStats";
            this.tabPageGameStats.Size = new System.Drawing.Size(1065, 511);
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
            this.treeViewStats.ShowLineCount = false;
            this.treeViewStats.ShowLines = true;
            this.treeViewStats.ShowPlusMinus = true;
            this.treeViewStats.ShowRootLines = true;
            this.treeViewStats.Size = new System.Drawing.Size(1065, 511);
            this.treeViewStats.TabIndex = 0;
            this.treeViewStats.TreeViewBackColor = System.Drawing.SystemColors.Control;
            this.treeViewStats.TreeViewForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // tabPageByShip
            // 
            this.tabPageByShip.Controls.Add(this.splitContainerShips);
            this.tabPageByShip.Location = new System.Drawing.Point(4, 22);
            this.tabPageByShip.Name = "tabPageByShip";
            this.tabPageByShip.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByShip.Size = new System.Drawing.Size(1065, 511);
            this.tabPageByShip.TabIndex = 6;
            this.tabPageByShip.Text = "By Ship";
            this.tabPageByShip.UseVisualStyleBackColor = true;
            // 
            // splitContainerShips
            // 
            this.splitContainerShips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerShips.Location = new System.Drawing.Point(3, 3);
            this.splitContainerShips.Name = "splitContainerShips";
            this.splitContainerShips.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerShips.Panel1
            // 
            this.splitContainerShips.Panel1.Controls.Add(this.extPanelDataGridViewScrollByShip);
            // 
            // splitContainerShips.Panel2
            // 
            this.splitContainerShips.Panel2.Controls.Add(this.extChartShips);
            this.splitContainerShips.Size = new System.Drawing.Size(1059, 505);
            this.splitContainerShips.SplitterDistance = 252;
            this.splitContainerShips.TabIndex = 4;
            // 
            // extPanelDataGridViewScrollByShip
            // 
            this.extPanelDataGridViewScrollByShip.Controls.Add(this.dataGridViewByShip);
            this.extPanelDataGridViewScrollByShip.Controls.Add(this.extScrollBarByShip);
            this.extPanelDataGridViewScrollByShip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollByShip.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollByShip.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollByShip.Name = "extPanelDataGridViewScrollByShip";
            this.extPanelDataGridViewScrollByShip.Size = new System.Drawing.Size(1059, 252);
            this.extPanelDataGridViewScrollByShip.TabIndex = 4;
            this.extPanelDataGridViewScrollByShip.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewByShip
            // 
            this.dataGridViewByShip.AllowUserToAddRows = false;
            this.dataGridViewByShip.AllowUserToDeleteRows = false;
            this.dataGridViewByShip.AllowUserToResizeRows = false;
            this.dataGridViewByShip.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewByShip.AutoSortByColumnName = true;
            this.dataGridViewByShip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewByShip.ColumnReorder = true;
            this.dataGridViewByShip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewByShip.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewByShip.Name = "dataGridViewByShip";
            this.dataGridViewByShip.PerColumnWordWrapControl = true;
            this.dataGridViewByShip.RowHeaderMenuStrip = null;
            this.dataGridViewByShip.RowHeadersVisible = false;
            this.dataGridViewByShip.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewByShip.SingleRowSelect = true;
            this.dataGridViewByShip.Size = new System.Drawing.Size(1043, 252);
            this.dataGridViewByShip.TabIndex = 3;
            // 
            // extScrollBarByShip
            // 
            this.extScrollBarByShip.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarByShip.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarByShip.ArrowColorScaling = 0.5F;
            this.extScrollBarByShip.ArrowDownDrawAngle = 270F;
            this.extScrollBarByShip.ArrowUpDrawAngle = 90F;
            this.extScrollBarByShip.BorderColor = System.Drawing.Color.White;
            this.extScrollBarByShip.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarByShip.HideScrollBar = false;
            this.extScrollBarByShip.LargeChange = 0;
            this.extScrollBarByShip.Location = new System.Drawing.Point(1043, 0);
            this.extScrollBarByShip.Maximum = -1;
            this.extScrollBarByShip.Minimum = 0;
            this.extScrollBarByShip.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarByShip.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarByShip.Name = "extScrollBarByShip";
            this.extScrollBarByShip.Size = new System.Drawing.Size(16, 252);
            this.extScrollBarByShip.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarByShip.SmallChange = 1;
            this.extScrollBarByShip.TabIndex = 0;
            this.extScrollBarByShip.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarByShip.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarByShip.ThumbColorScaling = 0.5F;
            this.extScrollBarByShip.ThumbDrawAngle = 0F;
            this.extScrollBarByShip.Value = -1;
            this.extScrollBarByShip.ValueLimited = -1;
            // 
            // extChartShips
            // 
            this.extChartShips.AutoScaleYAddedPercent = 5D;
            this.extChartShips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extChartShips.LeftArrowEnable = true;
            this.extChartShips.LeftArrowPosition = elementPosition9;
            this.extChartShips.Location = new System.Drawing.Point(0, 0);
            this.extChartShips.Name = "extChartShips";
            this.extChartShips.RightArrowEnable = true;
            this.extChartShips.RightArrowPosition = elementPosition10;
            this.extChartShips.Size = new System.Drawing.Size(1059, 249);
            this.extChartShips.TabIndex = 2;
            this.extChartShips.ZoomMouseWheelXMinimumInterval = 5D;
            this.extChartShips.ZoomMouseWheelXZoomFactor = 1.5D;
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.Location = new System.Drawing.Point(321, 7);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(20, 13);
            this.labelEndDate.TabIndex = 0;
            this.labelEndDate.Text = "To";
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerStartDate.BorderColorScaling = 0.5F;
            this.dateTimePickerStartDate.Checked = false;
            this.dateTimePickerStartDate.CustomFormat = "dd MMMM yyyy";
            this.dateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(56, 3);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerStartDate.ShowCheckBox = true;
            this.dateTimePickerStartDate.ShowUpDown = false;
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(250, 24);
            this.dateTimePickerStartDate.TabIndex = 1;
            this.dateTimePickerStartDate.TextBackColor = System.Drawing.Color.DarkBlue;
            this.dateTimePickerStartDate.Value = new System.DateTime(2021, 2, 5, 7, 56, 15, 927);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(618, 7);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(43, 13);
            this.labelStatus.TabIndex = 0;
            this.labelStatus.Text = "<code>";
            // 
            // extPanelRollUp
            // 
            this.extPanelRollUp.AutoHeight = false;
            this.extPanelRollUp.AutoHeightWidthDisable = false;
            this.extPanelRollUp.AutoWidth = false;
            this.extPanelRollUp.Controls.Add(this.labelStatus);
            this.extPanelRollUp.Controls.Add(this.dateTimePickerEndDate);
            this.extPanelRollUp.Controls.Add(this.dateTimePickerStartDate);
            this.extPanelRollUp.Controls.Add(this.labelStart);
            this.extPanelRollUp.Controls.Add(this.labelEndDate);
            this.extPanelRollUp.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelRollUp.HiddenMarkerWidth = 0;
            this.extPanelRollUp.Location = new System.Drawing.Point(0, 0);
            this.extPanelRollUp.Name = "extPanelRollUp";
            this.extPanelRollUp.PinState = true;
            this.extPanelRollUp.RolledUpHeight = 5;
            this.extPanelRollUp.RollUpAnimationTime = 500;
            this.extPanelRollUp.RollUpDelay = 1000;
            this.extPanelRollUp.SecondHiddenMarkerWidth = 0;
            this.extPanelRollUp.ShowHiddenMarker = true;
            this.extPanelRollUp.Size = new System.Drawing.Size(1073, 32);
            this.extPanelRollUp.TabIndex = 7;
            this.extPanelRollUp.UnrollHoverDelay = 1000;
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerEndDate.BorderColorScaling = 0.5F;
            this.dateTimePickerEndDate.Checked = false;
            this.dateTimePickerEndDate.CustomFormat = "dd MMMM yyyy";
            this.dateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(354, 3);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerEndDate.ShowCheckBox = true;
            this.dateTimePickerEndDate.ShowUpDown = false;
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(250, 24);
            this.dateTimePickerEndDate.TabIndex = 1;
            this.dateTimePickerEndDate.TextBackColor = System.Drawing.Color.DarkBlue;
            this.dateTimePickerEndDate.Value = new System.DateTime(2021, 2, 5, 7, 56, 15, 927);
            // 
            // labelStart
            // 
            this.labelStart.AutoSize = true;
            this.labelStart.Location = new System.Drawing.Point(3, 7);
            this.labelStart.Name = "labelStart";
            this.labelStart.Size = new System.Drawing.Size(29, 13);
            this.labelStart.TabIndex = 0;
            this.labelStart.Text = "Start";
            // 
            // dataGridViewTextBoxColumnRank
            // 
            this.dataGridViewTextBoxColumnRank.HeaderText = "Rank";
            this.dataGridViewTextBoxColumnRank.Name = "dataGridViewTextBoxColumnRank";
            this.dataGridViewTextBoxColumnRank.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnAtStart
            // 
            this.dataGridViewTextBoxColumnAtStart.HeaderText = "At Start";
            this.dataGridViewTextBoxColumnAtStart.MinimumWidth = 50;
            this.dataGridViewTextBoxColumnAtStart.Name = "dataGridViewTextBoxColumnAtStart";
            this.dataGridViewTextBoxColumnAtStart.ReadOnly = true;
            // 
            // dataGridViewTextBoxAtEnd
            // 
            this.dataGridViewTextBoxAtEnd.HeaderText = "At End";
            this.dataGridViewTextBoxAtEnd.Name = "dataGridViewTextBoxAtEnd";
            this.dataGridViewTextBoxAtEnd.ReadOnly = true;
            // 
            // dataGridViewTextColumnRankProgressNumeric
            // 
            this.dataGridViewTextColumnRankProgressNumeric.HeaderText = "Progress";
            this.dataGridViewTextColumnRankProgressNumeric.Name = "dataGridViewTextColumnRankProgressNumeric";
            this.dataGridViewTextColumnRankProgressNumeric.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnLastPromotionDate
            // 
            this.dataGridViewTextBoxColumnLastPromotionDate.HeaderText = "Last Promotion/Date";
            this.dataGridViewTextBoxColumnLastPromotionDate.Name = "dataGridViewTextBoxColumnLastPromotionDate";
            this.dataGridViewTextBoxColumnLastPromotionDate.ReadOnly = true;
            // 
            // UserControlStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlCustomStats);
            this.Controls.Add(this.extPanelRollUp);
            this.Name = "UserControlStats";
            this.Size = new System.Drawing.Size(1073, 569);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).EndInit();
            this.tabControlCustomStats.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.splitContainerGeneral.Panel1.ResumeLayout(false);
            this.splitContainerGeneral.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerGeneral)).EndInit();
            this.splitContainerGeneral.ResumeLayout(false);
            this.extPanelDataGridViewScrollGeneral.ResumeLayout(false);
            this.tabPageRanks.ResumeLayout(false);
            this.extPanelDataGridViewScrollRanks.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRanks)).EndInit();
            this.tabPageLedger.ResumeLayout(false);
            this.splitContainerLedger.Panel1.ResumeLayout(false);
            this.splitContainerLedger.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLedger)).EndInit();
            this.splitContainerLedger.ResumeLayout(false);
            this.extPanelDataGridViewScrollLedger.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).EndInit();
            this.tabPageTravel.ResumeLayout(false);
            this.tabPageTravel.PerformLayout();
            this.extPanelDataGridViewScrollTravel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).EndInit();
            this.tabPageScan.ResumeLayout(false);
            this.tabPageScan.PerformLayout();
            this.splitContainerScan.Panel1.ResumeLayout(false);
            this.splitContainerScan.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerScan)).EndInit();
            this.splitContainerScan.ResumeLayout(false);
            this.extPanelDataGridViewScrollScan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).EndInit();
            this.tabPageCombat.ResumeLayout(false);
            this.tabPageCombat.PerformLayout();
            this.splitContainerCombat.Panel1.ResumeLayout(false);
            this.splitContainerCombat.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCombat)).EndInit();
            this.splitContainerCombat.ResumeLayout(false);
            this.extPanelDataGridViewScrollCombat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCombat)).EndInit();
            this.tabPageGameStats.ResumeLayout(false);
            this.tabPageByShip.ResumeLayout(false);
            this.splitContainerShips.Panel1.ResumeLayout(false);
            this.splitContainerShips.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerShips)).EndInit();
            this.splitContainerShips.ResumeLayout(false);
            this.extPanelDataGridViewScrollByShip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewByShip)).EndInit();
            this.extPanelRollUp.ResumeLayout(false);
            this.extPanelRollUp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseUtils.DataGridViewBaseEnhancements dataGridViewGeneral;
        private ExtendedControls.ExtScrollBar vScrollBarGeneral;
        private ExtendedControls.ExtTabControl tabControlCustomStats;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageTravel;
        private System.Windows.Forms.TabPage tabPageScan;
        private StatsTimeUserControl userControlStatsTimeTravel;
        private BaseUtils.DataGridViewColumnControl dataGridViewTravel;
        private StatsTimeUserControl userControlStatsTimeScan;
        private BaseUtils.DataGridViewColumnControl dataGridViewScan;
        private System.Windows.Forms.TabPage tabPageGameStats;
        private ExtendedControls.Controls.ExtTreeView treeViewStats;
        private System.Windows.Forms.TabPage tabPageByShip;
        private BaseUtils.DataGridViewColumnControl dataGridViewByShip;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollTravel;
        private ExtendedControls.ExtScrollBar extScrollBarTravel;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollScan;
        private ExtendedControls.ExtScrollBar extScrollBarScan;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollByShip;
        private ExtendedControls.ExtScrollBar extScrollBarByShip;
        private System.Windows.Forms.Label labelEndDate;
        private ExtendedControls.ExtDateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelStatus;
        private ExtendedControls.ExtPanelRollUp extPanelRollUp;
        private ExtendedControls.ExtDateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label labelStart;
        private System.Windows.Forms.TabPage tabPageCombat;
        private BaseUtils.DataGridViewColumnControl dataGridViewCombat;
        private StatsTimeUserControl statsTimeUserControlCombat;
        private ExtendedControls.ExtSafeChart extChartTravelDest;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollGeneral;
        private System.Windows.Forms.TabPage tabPageLedger;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollLedger;
        private BaseUtils.DataGridViewBaseEnhancements dataGridViewLedger;
        private ExtendedControls.ExtScrollBar extScrollBarLedger;
        private ExtendedControls.ExtSafeChart extChartLedger;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnLedgerDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnNumericCredits;
        private System.Windows.Forms.SplitContainer splitContainerGeneral;
        private System.Windows.Forms.SplitContainer splitContainerLedger;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollCombat;
        private ExtendedControls.ExtScrollBar extScrollBarCombatDGV;
        private System.Windows.Forms.SplitContainer splitContainerScan;
        private System.Windows.Forms.SplitContainer splitContainerCombat;
        private ExtendedControls.ExtSafeChart extChartScan;
        private ExtendedControls.ExtSafeChart extChartCombat;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Information;
        private System.Windows.Forms.SplitContainer splitContainerShips;
        private ExtendedControls.ExtSafeChart extChartShips;
        private System.Windows.Forms.TabPage tabPageRanks;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollRanks;
        private BaseUtils.DataGridViewBaseEnhancements dataGridViewRanks;
        private ExtendedControls.ExtScrollBar extScrollBarRanks;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnRank;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnAtStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxAtEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextColumnRankProgressNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnLastPromotionDate;
    }
}
