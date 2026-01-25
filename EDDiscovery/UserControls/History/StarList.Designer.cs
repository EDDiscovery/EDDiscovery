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
    partial class StarList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StarList));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.checkBoxCursorToTop = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.textBoxSearch = new ExtendedControls.ExtTextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.comboBoxTime = new ExtendedControls.ExtComboBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quickMarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSortingOfColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewScanDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnSpanshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extComboBoxQuickMarks = new ExtendedControls.ExtComboBox();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridViewStarList = new BaseUtils.DataGridViewColumnControl();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVisits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBodycount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonTimeRanges = new ExtendedControls.ExtButton();
            this.extButtonDisplayFilters = new ExtendedControls.ExtButton();
            this.edsmSpanshButton = new EDDiscovery.UserControls.EDSMSpanshButton();
            this.contextMenuStrip.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStarList)).BeginInit();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBoxCursorToTop
            // 
            this.checkBoxCursorToTop.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCursorToTop.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxCursorToTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxCursorToTop.ButtonGradientDirection = 90F;
            this.checkBoxCursorToTop.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxCursorToTop.CheckBoxGradientDirection = 225F;
            this.checkBoxCursorToTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCursorToTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCursorToTop.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxCursorToTop.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxCursorToTop.DisabledScaling = 0.5F;
            this.checkBoxCursorToTop.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxCursorToTop.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxCursorToTop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxCursorToTop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxCursorToTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCursorToTop.Image = global::EDDiscovery.Icons.Controls.CursorToTop;
            this.checkBoxCursorToTop.ImageIndeterminate = null;
            this.checkBoxCursorToTop.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCursorToTop.ImageUnchecked = global::EDDiscovery.Icons.Controls.CursorStill;
            this.checkBoxCursorToTop.Location = new System.Drawing.Point(627, 1);
            this.checkBoxCursorToTop.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.checkBoxCursorToTop.MouseOverScaling = 1.3F;
            this.checkBoxCursorToTop.MouseSelectedScaling = 1.3F;
            this.checkBoxCursorToTop.Name = "checkBoxCursorToTop";
            this.checkBoxCursorToTop.Size = new System.Drawing.Size(28, 28);
            this.checkBoxCursorToTop.TabIndex = 30;
            this.checkBoxCursorToTop.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCursorToTop, "Automatically move the cursor to the latest entry when it arrives");
            this.checkBoxCursorToTop.UseVisualStyleBackColor = false;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtExcel.ButtonDisabledScaling = 0.5F;
            this.buttonExtExcel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.GradientDirection = 90F;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(593, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtExcel.MouseOverScaling = 1.3F;
            this.buttonExtExcel.MouseSelectedScaling = 1.3F;
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 28;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxSearch.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSearch.BorderColor2 = System.Drawing.Color.Transparent;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearch.ClearOnFirstChar = false;
            this.textBoxSearch.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSearch.EndButtonEnable = true;
            this.textBoxSearch.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxSearch.EndButtonImage")));
            this.textBoxSearch.EndButtonSize16ths = 10;
            this.textBoxSearch.EndButtonVisible = false;
            this.textBoxSearch.InErrorCondition = false;
            this.textBoxSearch.Location = new System.Drawing.Point(226, 4);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.textBoxSearch.Multiline = false;
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.ReadOnly = false;
            this.textBoxSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSearch.SelectionLength = 0;
            this.textBoxSearch.SelectionStart = 0;
            this.textBoxSearch.Size = new System.Drawing.Size(148, 20);
            this.textBoxSearch.TabIndex = 1;
            this.textBoxSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxSearch.TextNoChange = "";
            this.toolTip.SetToolTip(this.textBoxSearch, resources.GetString("textBoxSearch.ToolTip"));
            this.textBoxSearch.WordWrap = true;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(179, 4);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 24;
            this.labelSearch.Text = "Search";
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxTime.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxTime.DataSource = null;
            this.comboBoxTime.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTime.DisabledScaling = 0.5F;
            this.comboBoxTime.DisplayMember = "";
            this.comboBoxTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTime.GradientDirection = 90F;
            this.comboBoxTime.Location = new System.Drawing.Point(39, 4);
            this.comboBoxTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.comboBoxTime.MouseOverScalingColor = 1.3F;
            this.comboBoxTime.Name = "comboBoxTime";
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
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(3, 4);
            this.labelTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(30, 13);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "Time";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quickMarkToolStripMenuItem,
            this.removeSortingOfColumnsToolStripMenuItem,
            this.mapGotoStartoolStripMenuItem,
            this.viewScanDisplayToolStripMenuItem,
            this.viewOnSpanshToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.setNoteToolStripMenuItem});
            this.contextMenuStrip.Name = "historyContextMenu";
            this.contextMenuStrip.Size = new System.Drawing.Size(221, 158);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.historyContextMenu_Opening);
            // 
            // quickMarkToolStripMenuItem
            // 
            this.quickMarkToolStripMenuItem.Checked = true;
            this.quickMarkToolStripMenuItem.CheckOnClick = true;
            this.quickMarkToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.quickMarkToolStripMenuItem.Name = "quickMarkToolStripMenuItem";
            this.quickMarkToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.quickMarkToolStripMenuItem.Text = "Marked";
            this.quickMarkToolStripMenuItem.Click += new System.EventHandler(this.quickMarkToolStripMenuItem_Click);
            // 
            // removeSortingOfColumnsToolStripMenuItem
            // 
            this.removeSortingOfColumnsToolStripMenuItem.Name = "removeSortingOfColumnsToolStripMenuItem";
            this.removeSortingOfColumnsToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.removeSortingOfColumnsToolStripMenuItem.Text = "Remove sorting of columns";
            this.removeSortingOfColumnsToolStripMenuItem.Click += new System.EventHandler(this.removeSortingOfColumnsToolStripMenuItem_Click);
            // 
            // mapGotoStartoolStripMenuItem
            // 
            this.mapGotoStartoolStripMenuItem.Name = "mapGotoStartoolStripMenuItem";
            this.mapGotoStartoolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.mapGotoStartoolStripMenuItem.Text = "Go to star on 3D Map";
            this.mapGotoStartoolStripMenuItem.Click += new System.EventHandler(this.mapGotoStartoolStripMenuItem_Click);
            // 
            // viewScanDisplayToolStripMenuItem
            // 
            this.viewScanDisplayToolStripMenuItem.Name = "viewScanDisplayToolStripMenuItem";
            this.viewScanDisplayToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.viewScanDisplayToolStripMenuItem.Text = "View Scan Display";
            this.viewScanDisplayToolStripMenuItem.Click += new System.EventHandler(this.viewScanDisplayToolStripMenuItem_Click);
            // 
            // viewOnSpanshToolStripMenuItem
            // 
            this.viewOnSpanshToolStripMenuItem.Name = "viewOnSpanshToolStripMenuItem";
            this.viewOnSpanshToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.viewOnSpanshToolStripMenuItem.Text = "View on Spansh";
            this.viewOnSpanshToolStripMenuItem.Click += new System.EventHandler(this.viewOnSpanshToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // setNoteToolStripMenuItem
            // 
            this.setNoteToolStripMenuItem.Name = "setNoteToolStripMenuItem";
            this.setNoteToolStripMenuItem.Size = new System.Drawing.Size(220, 22);
            this.setNoteToolStripMenuItem.Text = "Set Note";
            this.setNoteToolStripMenuItem.Click += new System.EventHandler(this.setNoteToolStripMenuItem_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // extComboBoxQuickMarks
            // 
            this.extComboBoxQuickMarks.BackColor2 = System.Drawing.Color.Red;
            this.extComboBoxQuickMarks.BorderColor = System.Drawing.Color.Red;
            this.extComboBoxQuickMarks.ControlBackground = System.Drawing.SystemColors.Control;
            this.extComboBoxQuickMarks.DataSource = null;
            this.extComboBoxQuickMarks.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxQuickMarks.DisabledScaling = 0.5F;
            this.extComboBoxQuickMarks.DisplayMember = "";
            this.extComboBoxQuickMarks.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxQuickMarks.GradientDirection = 90F;
            this.extComboBoxQuickMarks.Location = new System.Drawing.Point(380, 4);
            this.extComboBoxQuickMarks.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.extComboBoxQuickMarks.MouseOverScalingColor = 1.3F;
            this.extComboBoxQuickMarks.Name = "extComboBoxQuickMarks";
            this.extComboBoxQuickMarks.SelectedIndex = -1;
            this.extComboBoxQuickMarks.SelectedItem = null;
            this.extComboBoxQuickMarks.SelectedValue = null;
            this.extComboBoxQuickMarks.Size = new System.Drawing.Size(139, 21);
            this.extComboBoxQuickMarks.TabIndex = 33;
            this.extComboBoxQuickMarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.extComboBoxQuickMarks, "Go to a marked journal entry. Use right click to mark an entry");
            this.extComboBoxQuickMarks.ValueMember = "";
            this.extComboBoxQuickMarks.SelectedIndexChanged += new System.EventHandler(this.extComboBoxQuickMarks_SelectedIndexChanged);
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewStarList);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 24;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(870, 580);
            this.dataViewScrollerPanel.TabIndex = 28;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.AlwaysHideScrollBar = false;
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = true;
            this.vScrollBarCustom.LargeChange = 0;
            this.vScrollBarCustom.Location = new System.Drawing.Point(843, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(24, 580);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SliderDrawAngle = 90F;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 4;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -1;
            this.vScrollBarCustom.ValueLimited = -1;
            // 
            // dataGridViewStarList
            // 
            this.dataGridViewStarList.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewStarList.AllowUserToAddRows = false;
            this.dataGridViewStarList.AllowUserToDeleteRows = false;
            this.dataGridViewStarList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewStarList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewStarList.AutoSortByColumnName = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewStarList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewStarList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStarList.ColumnReorder = true;
            this.dataGridViewStarList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.ColumnSystem,
            this.ColumnVisits,
            this.ColumnInformation,
            this.Value,
            this.ColumnBodycount});
            this.dataGridViewStarList.ContextMenuStrip = this.contextMenuStrip;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.TopLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewStarList.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewStarList.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewStarList.Name = "dataGridViewStarList";
            this.dataGridViewStarList.PerColumnWordWrapControl = true;
            this.dataGridViewStarList.RowHeaderMenuStrip = null;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewStarList.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewStarList.RowHeadersWidth = 50;
            this.dataGridViewStarList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewStarList.SingleRowSelect = true;
            this.dataGridViewStarList.Size = new System.Drawing.Size(843, 580);
            this.dataGridViewStarList.TabIndex = 3;
            this.dataGridViewStarList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellClick);
            this.dataGridViewStarList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewStarList_CellDoubleClick);
            this.dataGridViewStarList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewStarList_RowPostPaint);
            this.dataGridViewStarList.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewStarList_SortCompare);
            this.dataGridViewStarList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewTravel_KeyDown);
            this.dataGridViewStarList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridViewTravel_KeyPress);
            this.dataGridViewStarList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridViewTravel_KeyUp);
            this.dataGridViewStarList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewTravel_MouseDown);
            // 
            // ColumnTime
            // 
            this.ColumnTime.FillWeight = 80F;
            this.ColumnTime.HeaderText = "Last Visit";
            this.ColumnTime.MinimumWidth = 50;
            this.ColumnTime.Name = "ColumnTime";
            this.ColumnTime.ReadOnly = true;
            // 
            // ColumnSystem
            // 
            this.ColumnSystem.HeaderText = "System";
            this.ColumnSystem.MinimumWidth = 50;
            this.ColumnSystem.Name = "ColumnSystem";
            this.ColumnSystem.ReadOnly = true;
            // 
            // ColumnVisits
            // 
            dataGridViewCellStyle6.NullValue = null;
            this.ColumnVisits.DefaultCellStyle = dataGridViewCellStyle6;
            this.ColumnVisits.FillWeight = 40F;
            this.ColumnVisits.HeaderText = "Visits";
            this.ColumnVisits.MinimumWidth = 50;
            this.ColumnVisits.Name = "ColumnVisits";
            this.ColumnVisits.ReadOnly = true;
            this.ColumnVisits.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // ColumnInformation
            // 
            this.ColumnInformation.FillWeight = 200F;
            this.ColumnInformation.HeaderText = "Information";
            this.ColumnInformation.MinimumWidth = 50;
            this.ColumnInformation.Name = "ColumnInformation";
            this.ColumnInformation.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.HeaderText = "Scan Value";
            this.Value.Name = "Value";
            // 
            // ColumnBodycount
            // 
            this.ColumnBodycount.HeaderText = "Body Count";
            this.ColumnBodycount.Name = "ColumnBodycount";
            // 
            // topPanel
            // 
            this.topPanel.AutoSize = true;
            this.topPanel.Controls.Add(this.labelTime);
            this.topPanel.Controls.Add(this.comboBoxTime);
            this.topPanel.Controls.Add(this.extButtonTimeRanges);
            this.topPanel.Controls.Add(this.labelSearch);
            this.topPanel.Controls.Add(this.textBoxSearch);
            this.topPanel.Controls.Add(this.extComboBoxQuickMarks);
            this.topPanel.Controls.Add(this.extButtonDisplayFilters);
            this.topPanel.Controls.Add(this.edsmSpanshButton);
            this.topPanel.Controls.Add(this.buttonExtExcel);
            this.topPanel.Controls.Add(this.checkBoxCursorToTop);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(870, 30);
            this.topPanel.TabIndex = 38;
            // 
            // extButtonTimeRanges
            // 
            this.extButtonTimeRanges.BackColor2 = System.Drawing.Color.Red;
            this.extButtonTimeRanges.ButtonDisabledScaling = 0.5F;
            this.extButtonTimeRanges.GradientDirection = 90F;
            this.extButtonTimeRanges.Image = global::EDDiscovery.Icons.Controls.Clock;
            this.extButtonTimeRanges.Location = new System.Drawing.Point(145, 1);
            this.extButtonTimeRanges.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonTimeRanges.MouseOverScaling = 1.3F;
            this.extButtonTimeRanges.MouseSelectedScaling = 1.3F;
            this.extButtonTimeRanges.Name = "extButtonTimeRanges";
            this.extButtonTimeRanges.Size = new System.Drawing.Size(28, 28);
            this.extButtonTimeRanges.TabIndex = 25;
            this.extButtonTimeRanges.UseVisualStyleBackColor = true;
            this.extButtonTimeRanges.Click += new System.EventHandler(this.extButtonTimeRanges_Click);
            // 
            // extButtonDisplayFilters
            // 
            this.extButtonDisplayFilters.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonDisplayFilters.BackColor2 = System.Drawing.Color.Red;
            this.extButtonDisplayFilters.ButtonDisabledScaling = 0.5F;
            this.extButtonDisplayFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonDisplayFilters.GradientDirection = 90F;
            this.extButtonDisplayFilters.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonDisplayFilters.Location = new System.Drawing.Point(525, 1);
            this.extButtonDisplayFilters.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonDisplayFilters.MouseOverScaling = 1.3F;
            this.extButtonDisplayFilters.MouseSelectedScaling = 1.3F;
            this.extButtonDisplayFilters.Name = "extButtonDisplayFilters";
            this.extButtonDisplayFilters.Size = new System.Drawing.Size(28, 28);
            this.extButtonDisplayFilters.TabIndex = 31;
            this.extButtonDisplayFilters.UseVisualStyleBackColor = false;
            this.extButtonDisplayFilters.Click += new System.EventHandler(this.extButtonDisplayFilters_Click);
            // 
            // edsmSpanshButton
            // 
            this.edsmSpanshButton.BackColor2 = System.Drawing.Color.Red;
            this.edsmSpanshButton.ButtonDisabledScaling = 0.5F;
            this.edsmSpanshButton.GradientDirection = 90F;
            this.edsmSpanshButton.Image = global::EDDiscovery.Icons.Controls.EDSMSpansh;
            this.edsmSpanshButton.Location = new System.Drawing.Point(559, 1);
            this.edsmSpanshButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.edsmSpanshButton.MouseOverScaling = 1.3F;
            this.edsmSpanshButton.MouseSelectedScaling = 1.3F;
            this.edsmSpanshButton.Name = "edsmSpanshButton";
            this.edsmSpanshButton.SettingsSplittingChar = ';';
            this.edsmSpanshButton.Size = new System.Drawing.Size(28, 28);
            this.edsmSpanshButton.TabIndex = 32;
            this.edsmSpanshButton.UseVisualStyleBackColor = true;
            // 
            // UserControlStarList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "UserControlStarList";
            this.Size = new System.Drawing.Size(870, 610);
            this.contextMenuStrip.ResumeLayout(false);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStarList)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtTextBox textBoxSearch;
        private System.Windows.Forms.Label labelSearch;
        internal ExtendedControls.ExtComboBox comboBoxTime;
        private System.Windows.Forms.Label labelTime;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        public  BaseUtils.DataGridViewColumnControl dataGridViewStarList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonExtExcel;
        private System.Windows.Forms.ToolStripMenuItem setNoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSortingOfColumnsToolStripMenuItem;
        private ExtendedControls.ExtCheckBox checkBoxCursorToTop;
        private System.Windows.Forms.FlowLayoutPanel topPanel;
        private System.Windows.Forms.ToolStripMenuItem viewScanDisplayToolStripMenuItem;
        private ExtendedControls.ExtButton extButtonDisplayFilters;
        private System.Windows.Forms.ToolStripMenuItem viewOnSpanshToolStripMenuItem;
        private EDSMSpanshButton edsmSpanshButton;
        private ExtendedControls.ExtButton extButtonTimeRanges;
        internal ExtendedControls.ExtComboBox extComboBoxQuickMarks;
        private System.Windows.Forms.ToolStripMenuItem quickMarkToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVisits;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBodycount;
    }
}
