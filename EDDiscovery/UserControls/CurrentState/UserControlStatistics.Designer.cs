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
            ExtendedControls.TabStyleSquare tabStyleSquare1 = new ExtendedControls.TabStyleSquare();
            this.dataGridViewGeneral = new BaseUtils.DataGridViewBaseEnhancements();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Information = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelGeneral = new ExtendedControls.ExtPanelScroll();
            this.vScrollBarGeneral = new ExtendedControls.ExtScrollBar();
            this.tabControlCustomStats = new ExtendedControls.ExtTabControl();
            this.tabPageGeneral = new System.Windows.Forms.TabPage();
            this.tabPageTravel = new System.Windows.Forms.TabPage();
            this.extPanelDataGridViewScrollTravel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarTravel = new ExtendedControls.ExtScrollBar();
            this.dataGridViewTravel = new BaseUtils.DataGridViewColumnHider();
            this.userControlStatsTimeTravel = new EDDiscovery.UserControls.StatsTimeUserControl();
            this.tabPageScan = new System.Windows.Forms.TabPage();
            this.extPanelDataGridViewScrollScan = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewScan = new BaseUtils.DataGridViewColumnHider();
            this.extScrollBarScan = new ExtendedControls.ExtScrollBar();
            this.userControlStatsTimeScan = new EDDiscovery.UserControls.StatsTimeUserControl();
            this.tabPageGameStats = new System.Windows.Forms.TabPage();
            this.treeViewStats = new ExtendedControls.Controls.ExtTreeView();
            this.tabPageByShip = new System.Windows.Forms.TabPage();
            this.extPanelDataGridViewScrollByShip = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewByShip = new BaseUtils.DataGridViewColumnHider();
            this.extScrollBarByShip = new ExtendedControls.ExtScrollBar();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerStartDate = new ExtendedControls.ExtDateTimePicker();
            this.labelStatus = new System.Windows.Forms.Label();
            this.extPanelRollUp = new ExtendedControls.ExtPanelRollUp();
            this.dateTimePickerEndDate = new ExtendedControls.ExtDateTimePicker();
            this.labelStart = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).BeginInit();
            this.panelGeneral.SuspendLayout();
            this.tabControlCustomStats.SuspendLayout();
            this.tabPageGeneral.SuspendLayout();
            this.tabPageTravel.SuspendLayout();
            this.extPanelDataGridViewScrollTravel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).BeginInit();
            this.tabPageScan.SuspendLayout();
            this.extPanelDataGridViewScrollScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).BeginInit();
            this.tabPageGameStats.SuspendLayout();
            this.tabPageByShip.SuspendLayout();
            this.extPanelDataGridViewScrollByShip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewByShip)).BeginInit();
            this.extPanelRollUp.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridViewGeneral
            // 
            this.dataGridViewGeneral.AllowUserToAddRows = false;
            this.dataGridViewGeneral.AllowUserToDeleteRows = false;
            this.dataGridViewGeneral.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewGeneral.ColumnHeaderMenuStrip = null;
            this.dataGridViewGeneral.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGeneral.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemName,
            this.Information});
            this.dataGridViewGeneral.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewGeneral.Name = "dataGridViewGeneral";
            this.dataGridViewGeneral.RowHeaderMenuStrip = null;
            this.dataGridViewGeneral.RowHeadersVisible = false;
            this.dataGridViewGeneral.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewGeneral.SingleRowSelect = true;
            this.dataGridViewGeneral.Size = new System.Drawing.Size(634, 190);
            this.dataGridViewGeneral.TabIndex = 2;
            this.dataGridViewGeneral.TopLeftHeaderMenuStrip = null;
            // 
            // ItemName
            // 
            this.ItemName.HeaderText = "Item";
            this.ItemName.Name = "ItemName";
            // 
            // Information
            // 
            this.Information.FillWeight = 400F;
            this.Information.HeaderText = "Information";
            this.Information.MinimumWidth = 50;
            this.Information.Name = "Information";
            // 
            // panelGeneral
            // 
            this.panelGeneral.Controls.Add(this.dataGridViewGeneral);
            this.panelGeneral.Controls.Add(this.vScrollBarGeneral);
            this.panelGeneral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGeneral.FlowControlsLeftToRight = false;
            this.panelGeneral.Location = new System.Drawing.Point(3, 3);
            this.panelGeneral.Name = "panelGeneral";
            this.panelGeneral.Size = new System.Drawing.Size(1059, 505);
            this.panelGeneral.TabIndex = 4;
            this.panelGeneral.VerticalScrollBarDockRight = true;
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
            this.vScrollBarGeneral.LargeChange = 10;
            this.vScrollBarGeneral.Location = new System.Drawing.Point(1043, 0);
            this.vScrollBarGeneral.Maximum = -306;
            this.vScrollBarGeneral.Minimum = 0;
            this.vScrollBarGeneral.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarGeneral.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarGeneral.Name = "vScrollBarGeneral";
            this.vScrollBarGeneral.Size = new System.Drawing.Size(16, 505);
            this.vScrollBarGeneral.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarGeneral.SmallChange = 1;
            this.vScrollBarGeneral.TabIndex = 8;
            this.vScrollBarGeneral.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarGeneral.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarGeneral.ThumbColorScaling = 0.5F;
            this.vScrollBarGeneral.ThumbDrawAngle = 0F;
            this.vScrollBarGeneral.Value = -306;
            this.vScrollBarGeneral.ValueLimited = -306;
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
            this.tabPageGeneral.Controls.Add(this.panelGeneral);
            this.tabPageGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabPageGeneral.Name = "tabPageGeneral";
            this.tabPageGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGeneral.Size = new System.Drawing.Size(1065, 511);
            this.tabPageGeneral.TabIndex = 0;
            this.tabPageGeneral.Text = "General";
            this.tabPageGeneral.UseVisualStyleBackColor = true;
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
            this.extPanelDataGridViewScrollTravel.Location = new System.Drawing.Point(0, 23);
            this.extPanelDataGridViewScrollTravel.Name = "extPanelDataGridViewScrollTravel";
            this.extPanelDataGridViewScrollTravel.Size = new System.Drawing.Size(1065, 488);
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
            this.extScrollBarTravel.Size = new System.Drawing.Size(16, 488);
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
            this.dataGridViewTravel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTravel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTravel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTravel.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTravel.Name = "dataGridViewTravel";
            this.dataGridViewTravel.RowHeaderMenuStrip = null;
            this.dataGridViewTravel.RowHeadersVisible = false;
            this.dataGridViewTravel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewTravel.SingleRowSelect = true;
            this.dataGridViewTravel.Size = new System.Drawing.Size(1049, 488);
            this.dataGridViewTravel.TabIndex = 3;
            this.dataGridViewTravel.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewTravel_SortCompare);
            // 
            // userControlStatsTimeTravel
            // 
            this.userControlStatsTimeTravel.AllowCustomTime = false;
            this.userControlStatsTimeTravel.AutoSize = true;
            this.userControlStatsTimeTravel.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlStatsTimeTravel.Location = new System.Drawing.Point(0, 0);
            this.userControlStatsTimeTravel.Name = "userControlStatsTimeTravel";
            this.userControlStatsTimeTravel.Size = new System.Drawing.Size(1065, 23);
            this.userControlStatsTimeTravel.TabIndex = 0;
            this.userControlStatsTimeTravel.TimeMode = EDDiscovery.UserControls.StatsTimeUserControl.TimeModeType.Summary;
            this.userControlStatsTimeTravel.TimeModeChanged += new System.EventHandler(this.userControlStatsTimeTravel_TimeModeChanged);
            // 
            // tabPageScan
            // 
            this.tabPageScan.Controls.Add(this.extPanelDataGridViewScrollScan);
            this.tabPageScan.Controls.Add(this.userControlStatsTimeScan);
            this.tabPageScan.Location = new System.Drawing.Point(4, 22);
            this.tabPageScan.Name = "tabPageScan";
            this.tabPageScan.Size = new System.Drawing.Size(1065, 511);
            this.tabPageScan.TabIndex = 1;
            this.tabPageScan.Text = "Scan";
            this.tabPageScan.UseVisualStyleBackColor = true;
            // 
            // extPanelDataGridViewScrollScan
            // 
            this.extPanelDataGridViewScrollScan.Controls.Add(this.dataGridViewScan);
            this.extPanelDataGridViewScrollScan.Controls.Add(this.extScrollBarScan);
            this.extPanelDataGridViewScrollScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollScan.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollScan.Location = new System.Drawing.Point(0, 23);
            this.extPanelDataGridViewScrollScan.Name = "extPanelDataGridViewScrollScan";
            this.extPanelDataGridViewScrollScan.Size = new System.Drawing.Size(1065, 488);
            this.extPanelDataGridViewScrollScan.TabIndex = 5;
            this.extPanelDataGridViewScrollScan.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewScan
            // 
            this.dataGridViewScan.AllowUserToAddRows = false;
            this.dataGridViewScan.AllowUserToDeleteRows = false;
            this.dataGridViewScan.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewScan.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewScan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewScan.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewScan.Name = "dataGridViewScan";
            this.dataGridViewScan.RowHeaderMenuStrip = null;
            this.dataGridViewScan.RowHeadersVisible = false;
            this.dataGridViewScan.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewScan.SingleRowSelect = true;
            this.dataGridViewScan.Size = new System.Drawing.Size(1049, 488);
            this.dataGridViewScan.TabIndex = 4;
            this.dataGridViewScan.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewScan_SortCompare);
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
            this.extScrollBarScan.Size = new System.Drawing.Size(16, 488);
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
            // userControlStatsTimeScan
            // 
            this.userControlStatsTimeScan.AllowCustomTime = false;
            this.userControlStatsTimeScan.AutoSize = true;
            this.userControlStatsTimeScan.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlStatsTimeScan.Location = new System.Drawing.Point(0, 0);
            this.userControlStatsTimeScan.Name = "userControlStatsTimeScan";
            this.userControlStatsTimeScan.Size = new System.Drawing.Size(1065, 23);
            this.userControlStatsTimeScan.TabIndex = 1;
            this.userControlStatsTimeScan.TimeMode = EDDiscovery.UserControls.StatsTimeUserControl.TimeModeType.Summary;
            this.userControlStatsTimeScan.TimeModeChanged += new System.EventHandler(this.userControlStatsTimeScan_TimeModeChanged);
            this.userControlStatsTimeScan.DrawModeChanged += new System.EventHandler(this.userControlStatsTimeScan_DrawModeChanged);
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
            this.tabPageByShip.Controls.Add(this.extPanelDataGridViewScrollByShip);
            this.tabPageByShip.Location = new System.Drawing.Point(4, 22);
            this.tabPageByShip.Name = "tabPageByShip";
            this.tabPageByShip.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageByShip.Size = new System.Drawing.Size(1065, 511);
            this.tabPageByShip.TabIndex = 6;
            this.tabPageByShip.Text = "By Ship";
            this.tabPageByShip.UseVisualStyleBackColor = true;
            // 
            // extPanelDataGridViewScrollByShip
            // 
            this.extPanelDataGridViewScrollByShip.Controls.Add(this.dataGridViewByShip);
            this.extPanelDataGridViewScrollByShip.Controls.Add(this.extScrollBarByShip);
            this.extPanelDataGridViewScrollByShip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollByShip.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollByShip.Location = new System.Drawing.Point(3, 3);
            this.extPanelDataGridViewScrollByShip.Name = "extPanelDataGridViewScrollByShip";
            this.extPanelDataGridViewScrollByShip.Size = new System.Drawing.Size(1059, 505);
            this.extPanelDataGridViewScrollByShip.TabIndex = 4;
            this.extPanelDataGridViewScrollByShip.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewByShip
            // 
            this.dataGridViewByShip.AllowUserToAddRows = false;
            this.dataGridViewByShip.AllowUserToDeleteRows = false;
            this.dataGridViewByShip.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewByShip.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewByShip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewByShip.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewByShip.Name = "dataGridViewByShip";
            this.dataGridViewByShip.RowHeaderMenuStrip = null;
            this.dataGridViewByShip.RowHeadersVisible = false;
            this.dataGridViewByShip.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewByShip.SingleRowSelect = true;
            this.dataGridViewByShip.Size = new System.Drawing.Size(1043, 505);
            this.dataGridViewByShip.TabIndex = 3;
            this.dataGridViewByShip.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewByShip_SortCompare);
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
            this.extScrollBarByShip.Size = new System.Drawing.Size(16, 505);
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
            // UserControlStats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlCustomStats);
            this.Controls.Add(this.extPanelRollUp);
            this.Name = "UserControlStats";
            this.Size = new System.Drawing.Size(1073, 569);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGeneral)).EndInit();
            this.panelGeneral.ResumeLayout(false);
            this.tabControlCustomStats.ResumeLayout(false);
            this.tabPageGeneral.ResumeLayout(false);
            this.tabPageTravel.ResumeLayout(false);
            this.tabPageTravel.PerformLayout();
            this.extPanelDataGridViewScrollTravel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).EndInit();
            this.tabPageScan.ResumeLayout(false);
            this.tabPageScan.PerformLayout();
            this.extPanelDataGridViewScrollScan.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScan)).EndInit();
            this.tabPageGameStats.ResumeLayout(false);
            this.tabPageByShip.ResumeLayout(false);
            this.extPanelDataGridViewScrollByShip.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewByShip)).EndInit();
            this.extPanelRollUp.ResumeLayout(false);
            this.extPanelRollUp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BaseUtils.DataGridViewBaseEnhancements dataGridViewGeneral;
        private ExtendedControls.ExtPanelScroll panelGeneral;
        private ExtendedControls.ExtScrollBar vScrollBarGeneral;
        private ExtendedControls.ExtTabControl tabControlCustomStats;
        private System.Windows.Forms.TabPage tabPageGeneral;
        private System.Windows.Forms.TabPage tabPageTravel;
        private System.Windows.Forms.TabPage tabPageScan;
        private StatsTimeUserControl userControlStatsTimeTravel;
        private BaseUtils.DataGridViewColumnHider dataGridViewTravel;
        private StatsTimeUserControl userControlStatsTimeScan;
        private BaseUtils.DataGridViewColumnHider dataGridViewScan;
        private System.Windows.Forms.TabPage tabPageGameStats;
        private ExtendedControls.Controls.ExtTreeView treeViewStats;
        private System.Windows.Forms.TabPage tabPageByShip;
        private BaseUtils.DataGridViewColumnHider dataGridViewByShip;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollTravel;
        private ExtendedControls.ExtScrollBar extScrollBarTravel;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollScan;
        private ExtendedControls.ExtScrollBar extScrollBarScan;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollByShip;
        private ExtendedControls.ExtScrollBar extScrollBarByShip;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Information;
        private System.Windows.Forms.Label labelEndDate;
        private ExtendedControls.ExtDateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelStatus;
        private ExtendedControls.ExtPanelRollUp extPanelRollUp;
        private ExtendedControls.ExtDateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label labelStart;
    }
}
