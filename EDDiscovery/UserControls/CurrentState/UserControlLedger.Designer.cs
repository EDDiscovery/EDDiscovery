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
    partial class UserControlLedger
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlLedger));
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition1 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition2 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewLedger = new BaseUtils.DataGridViewColumnControl();
            this.TimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Credits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Debits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NormProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemGotoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.labelTime = new System.Windows.Forms.Label();
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.textBoxFilter = new ExtendedControls.ExtTextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.comboBoxTime = new ExtendedControls.ExtComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.label24h = new System.Windows.Forms.Label();
            this.splitContainerLedger = new System.Windows.Forms.SplitContainer();
            this.extChartLedger = new ExtendedControls.ExtSafeChart();
            this.label7d = new System.Windows.Forms.Label();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.topPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLedger)).BeginInit();
            this.splitContainerLedger.Panel1.SuspendLayout();
            this.splitContainerLedger.Panel2.SuspendLayout();
            this.splitContainerLedger.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewLedger);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(800, 266);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewLedger
            // 
            this.dataGridViewLedger.AllowUserToAddRows = false;
            this.dataGridViewLedger.AllowUserToDeleteRows = false;
            this.dataGridViewLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLedger.AutoSortByColumnName = false;
            this.dataGridViewLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLedger.ColumnReorder = true;
            this.dataGridViewLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TimeCol,
            this.Type,
            this.Notes,
            this.Credits,
            this.Debits,
            this.Balance,
            this.NormProfit,
            this.TotalProfit});
            this.dataGridViewLedger.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridViewLedger.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewLedger.Name = "dataGridViewLedger";
            this.dataGridViewLedger.PerColumnWordWrapControl = true;
            this.dataGridViewLedger.RowHeaderMenuStrip = null;
            this.dataGridViewLedger.RowHeadersVisible = false;
            this.dataGridViewLedger.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLedger.SingleRowSelect = true;
            this.dataGridViewLedger.Size = new System.Drawing.Size(784, 266);
            this.dataGridViewLedger.TabIndex = 1;
            this.dataGridViewLedger.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLedger_CellClick);
            this.dataGridViewLedger.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewLedger_SortCompare);
            // 
            // TimeCol
            // 
            this.TimeCol.HeaderText = "Time";
            this.TimeCol.MinimumWidth = 50;
            this.TimeCol.Name = "TimeCol";
            this.TimeCol.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.MinimumWidth = 80;
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Notes
            // 
            this.Notes.FillWeight = 200F;
            this.Notes.HeaderText = "Notes";
            this.Notes.MinimumWidth = 80;
            this.Notes.Name = "Notes";
            this.Notes.ReadOnly = true;
            // 
            // Credits
            // 
            this.Credits.HeaderText = "Credits";
            this.Credits.MinimumWidth = 80;
            this.Credits.Name = "Credits";
            this.Credits.ReadOnly = true;
            // 
            // Debits
            // 
            this.Debits.HeaderText = "Debits";
            this.Debits.MinimumWidth = 80;
            this.Debits.Name = "Debits";
            this.Debits.ReadOnly = true;
            // 
            // Balance
            // 
            this.Balance.HeaderText = "Balance";
            this.Balance.MinimumWidth = 80;
            this.Balance.Name = "Balance";
            this.Balance.ReadOnly = true;
            // 
            // NormProfit
            // 
            this.NormProfit.HeaderText = "Profit Per Unit";
            this.NormProfit.MinimumWidth = 20;
            this.NormProfit.Name = "NormProfit";
            // 
            // TotalProfit
            // 
            this.TotalProfit.HeaderText = "Total Profit";
            this.TotalProfit.MinimumWidth = 80;
            this.TotalProfit.Name = "TotalProfit";
            this.TotalProfit.ReadOnly = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemGotoItem});
            this.contextMenuStrip.Name = "contextMenuStripLedger";
            this.contextMenuStrip.Size = new System.Drawing.Size(175, 26);
            // 
            // toolStripMenuItemGotoItem
            // 
            this.toolStripMenuItemGotoItem.Name = "toolStripMenuItemGotoItem";
            this.toolStripMenuItemGotoItem.Size = new System.Drawing.Size(174, 22);
            this.toolStripMenuItemGotoItem.Text = "Go to entry on grid";
            this.toolStripMenuItemGotoItem.Click += new System.EventHandler(this.toolStripMenuItemGotoItem_Click);
            // 
            // vScrollBarCustomMC
            // 
            this.vScrollBarCustomMC.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustomMC.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustomMC.ArrowColorScaling = 0.5F;
            this.vScrollBarCustomMC.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustomMC.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustomMC.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustomMC.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustomMC.HideScrollBar = false;
            this.vScrollBarCustomMC.LargeChange = 0;
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(784, 0);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(16, 266);
            this.vScrollBarCustomMC.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomMC.SmallChange = 1;
            this.vScrollBarCustomMC.TabIndex = 0;
            this.vScrollBarCustomMC.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustomMC.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomMC.ThumbColorScaling = 0.5F;
            this.vScrollBarCustomMC.ThumbDrawAngle = 0F;
            this.vScrollBarCustomMC.Value = -1;
            this.vScrollBarCustomMC.ValueLimited = -1;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(0, 3);
            this.labelTime.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(30, 13);
            this.labelTime.TabIndex = 26;
            this.labelTime.Text = "Time";
            // 
            // buttonFilter
            // 
            this.buttonFilter.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.buttonFilter.Location = new System.Drawing.Point(351, 1);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(28, 28);
            this.buttonFilter.TabIndex = 25;
            this.toolTip.SetToolTip(this.buttonFilter, "Display entries matching this event type filter");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFilter.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxFilter.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFilter.BorderColorScaling = 0.5F;
            this.textBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFilter.ClearOnFirstChar = false;
            this.textBoxFilter.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFilter.EndButtonEnable = true;
            this.textBoxFilter.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxFilter.EndButtonImage")));
            this.textBoxFilter.EndButtonVisible = false;
            this.textBoxFilter.InErrorCondition = false;
            this.textBoxFilter.Location = new System.Drawing.Point(195, 1);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.textBoxFilter.Multiline = false;
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.ReadOnly = false;
            this.textBoxFilter.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFilter.SelectionLength = 0;
            this.textBoxFilter.SelectionStart = 0;
            this.textBoxFilter.Size = new System.Drawing.Size(148, 20);
            this.textBoxFilter.TabIndex = 1;
            this.textBoxFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxFilter, "Search to particular items");
            this.textBoxFilter.WordWrap = true;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(146, 3);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 24;
            this.labelSearch.Text = "Search";
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTime.ButtonColorScaling = 0.5F;
            this.comboBoxTime.DataSource = null;
            this.comboBoxTime.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTime.DisplayMember = "";
            this.comboBoxTime.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTime.Location = new System.Drawing.Point(38, 1);
            this.comboBoxTime.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.comboBoxTime.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxTime.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxTime.SelectedIndex = -1;
            this.comboBoxTime.SelectedItem = null;
            this.comboBoxTime.SelectedValue = null;
            this.comboBoxTime.Size = new System.Drawing.Size(100, 21);
            this.comboBoxTime.TabIndex = 0;
            this.comboBoxTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxTime, "Select the entries by age");
            this.comboBoxTime.ValueMember = "";
            this.comboBoxTime.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(423, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 32;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(387, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extCheckBoxWordWrap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 33;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // topPanel
            // 
            this.topPanel.AutoSize = true;
            this.topPanel.Controls.Add(this.labelTime);
            this.topPanel.Controls.Add(this.comboBoxTime);
            this.topPanel.Controls.Add(this.labelSearch);
            this.topPanel.Controls.Add(this.textBoxFilter);
            this.topPanel.Controls.Add(this.buttonFilter);
            this.topPanel.Controls.Add(this.extCheckBoxWordWrap);
            this.topPanel.Controls.Add(this.buttonExtExcel);
            this.topPanel.Controls.Add(this.label24h);
            this.topPanel.Controls.Add(this.label7d);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(800, 30);
            this.topPanel.TabIndex = 2;
            // 
            // label24h
            // 
            this.label24h.AutoSize = true;
            this.label24h.Location = new System.Drawing.Point(459, 3);
            this.label24h.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
            this.label24h.Name = "label24h";
            this.label24h.Size = new System.Drawing.Size(64, 13);
            this.label24h.TabIndex = 34;
            this.label24h.Text = "<code 24h>";
            // 
            // splitContainerLedger
            // 
            this.splitContainerLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLedger.Location = new System.Drawing.Point(0, 30);
            this.splitContainerLedger.Name = "splitContainerLedger";
            this.splitContainerLedger.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLedger.Panel1
            // 
            this.splitContainerLedger.Panel1.Controls.Add(this.dataViewScrollerPanel);
            // 
            // splitContainerLedger.Panel2
            // 
            this.splitContainerLedger.Panel2.Controls.Add(this.extChartLedger);
            this.splitContainerLedger.Size = new System.Drawing.Size(800, 542);
            this.splitContainerLedger.SplitterDistance = 266;
            this.splitContainerLedger.TabIndex = 2;
            // 
            // extChartLedger
            // 
            this.extChartLedger.AutoScaleYAddedPercent = 5D;
            this.extChartLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extChartLedger.LeftArrowEnable = true;
            this.extChartLedger.LeftArrowPosition = elementPosition1;
            this.extChartLedger.Location = new System.Drawing.Point(0, 0);
            this.extChartLedger.Name = "extChartLedger";
            this.extChartLedger.RightArrowEnable = true;
            this.extChartLedger.RightArrowPosition = elementPosition2;
            this.extChartLedger.Size = new System.Drawing.Size(800, 272);
            this.extChartLedger.TabIndex = 11;
            this.extChartLedger.ZoomMouseWheelXMinimumInterval = 5D;
            this.extChartLedger.ZoomMouseWheelXZoomFactor = 1.5D;
            // 
            // label7d
            // 
            this.label7d.AutoSize = true;
            this.label7d.Location = new System.Drawing.Point(531, 3);
            this.label7d.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
            this.label7d.Name = "label7d";
            this.label7d.Size = new System.Drawing.Size(58, 13);
            this.label7d.TabIndex = 34;
            this.label7d.Text = "<code 7d>";
            // 
            // UserControlLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerLedger);
            this.Controls.Add(this.topPanel);
            this.Name = "UserControlLedger";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.splitContainerLedger.Panel1.ResumeLayout(false);
            this.splitContainerLedger.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLedger)).EndInit();
            this.splitContainerLedger.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private BaseUtils.DataGridViewColumnControl dataGridViewLedger;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private ExtendedControls.ExtButton buttonFilter;
        internal ExtendedControls.ExtComboBox comboBoxTime;
        private System.Windows.Forms.Label labelSearch;
        private ExtendedControls.ExtTextBox textBoxFilter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGotoItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.FlowLayoutPanel topPanel;
        private ExtendedControls.ExtButton buttonExtExcel;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Credits;
        private System.Windows.Forms.DataGridViewTextBoxColumn Debits;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn NormProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalProfit;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private System.Windows.Forms.SplitContainer splitContainerLedger;
        private ExtendedControls.ExtSafeChart extChartLedger;
        private System.Windows.Forms.Label label24h;
        private System.Windows.Forms.Label label7d;
    }
}
