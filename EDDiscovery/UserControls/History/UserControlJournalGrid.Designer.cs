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
    partial class UserControlJournalGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlJournalGrid));
            this.historyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quickMarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSortingOfColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpToEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewScanDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnSpanshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.runActionsOnThisEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyJournalEntryToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.comboBoxTime = new ExtendedControls.ExtComboBox();
            this.extButtonTimeRanges = new ExtendedControls.ExtButton();
            this.textBoxSearch = new ExtendedControls.ExtTextBox();
            this.extComboBoxQuickMarks = new ExtendedControls.ExtComboBox();
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.buttonField = new ExtendedControls.ExtButton();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.checkBoxCursorToTop = new ExtendedControls.ExtCheckBox();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridViewJournal = new BaseUtils.DataGridViewColumnControl();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.labelTime = new System.Windows.Forms.Label();
            this.labelSearch = new System.Windows.Forms.Label();
            this.extButtonEventColours = new ExtendedControls.ExtButton();
            this.historyContextMenu.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJournal)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // historyContextMenu
            // 
            this.historyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quickMarkToolStripMenuItem,
            this.removeSortingOfColumnsToolStripMenuItem,
            this.jumpToEntryToolStripMenuItem,
            this.mapGotoStartoolStripMenuItem,
            this.viewScanDisplayToolStripMenuItem,
            this.viewOnSpanshToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.toolStripMenuItemStartStop,
            this.runActionsOnThisEntryToolStripMenuItem,
            this.copyJournalEntryToClipboardToolStripMenuItem});
            this.historyContextMenu.Name = "historyContextMenu";
            this.historyContextMenu.Size = new System.Drawing.Size(294, 224);
            this.historyContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.historyContextMenu_Opening);
            // 
            // quickMarkToolStripMenuItem
            // 
            this.quickMarkToolStripMenuItem.Checked = true;
            this.quickMarkToolStripMenuItem.CheckOnClick = true;
            this.quickMarkToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.quickMarkToolStripMenuItem.Name = "quickMarkToolStripMenuItem";
            this.quickMarkToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.quickMarkToolStripMenuItem.Text = "Marked";
            this.quickMarkToolStripMenuItem.Click += new System.EventHandler(this.quickMarkToolStripMenuItem_Click);
            // 
            // removeSortingOfColumnsToolStripMenuItem
            // 
            this.removeSortingOfColumnsToolStripMenuItem.Name = "removeSortingOfColumnsToolStripMenuItem";
            this.removeSortingOfColumnsToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.removeSortingOfColumnsToolStripMenuItem.Text = "Remove sorting of columns";
            this.removeSortingOfColumnsToolStripMenuItem.Click += new System.EventHandler(this.removeSortingOfColumnsToolStripMenuItem_Click);
            // 
            // jumpToEntryToolStripMenuItem
            // 
            this.jumpToEntryToolStripMenuItem.Name = "jumpToEntryToolStripMenuItem";
            this.jumpToEntryToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.jumpToEntryToolStripMenuItem.Text = "Jump to Entry";
            this.jumpToEntryToolStripMenuItem.Click += new System.EventHandler(this.jumpToEntryToolStripMenuItem_Click);
            // 
            // mapGotoStartoolStripMenuItem
            // 
            this.mapGotoStartoolStripMenuItem.Name = "mapGotoStartoolStripMenuItem";
            this.mapGotoStartoolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.mapGotoStartoolStripMenuItem.Text = "Go to star on 3D Map";
            this.mapGotoStartoolStripMenuItem.Click += new System.EventHandler(this.mapGotoStartoolStripMenuItem_Click);
            // 
            // viewScanDisplayToolStripMenuItem
            // 
            this.viewScanDisplayToolStripMenuItem.Name = "viewScanDisplayToolStripMenuItem";
            this.viewScanDisplayToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.viewScanDisplayToolStripMenuItem.Text = "View Scan Display";
            this.viewScanDisplayToolStripMenuItem.Click += new System.EventHandler(this.viewScanDisplayToolStripMenuItem_Click);
            // 
            // viewOnSpanshToolStripMenuItem
            // 
            this.viewOnSpanshToolStripMenuItem.Name = "viewOnSpanshToolStripMenuItem";
            this.viewOnSpanshToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.viewOnSpanshToolStripMenuItem.Text = "View on Spansh";
            this.viewOnSpanshToolStripMenuItem.Click += new System.EventHandler(this.viewOnSpanshToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // toolStripMenuItemStartStop
            // 
            this.toolStripMenuItemStartStop.Name = "toolStripMenuItemStartStop";
            this.toolStripMenuItemStartStop.Size = new System.Drawing.Size(293, 22);
            this.toolStripMenuItemStartStop.Text = "Set Start/Stop point for travel calculations";
            this.toolStripMenuItemStartStop.Click += new System.EventHandler(this.toolStripMenuItemStartStop_Click);
            // 
            // runActionsOnThisEntryToolStripMenuItem
            // 
            this.runActionsOnThisEntryToolStripMenuItem.Name = "runActionsOnThisEntryToolStripMenuItem";
            this.runActionsOnThisEntryToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.runActionsOnThisEntryToolStripMenuItem.Text = "Run Actions on this entry";
            this.runActionsOnThisEntryToolStripMenuItem.Click += new System.EventHandler(this.runActionsOnThisEntryToolStripMenuItem_Click);
            // 
            // copyJournalEntryToClipboardToolStripMenuItem
            // 
            this.copyJournalEntryToClipboardToolStripMenuItem.Name = "copyJournalEntryToClipboardToolStripMenuItem";
            this.copyJournalEntryToClipboardToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.copyJournalEntryToClipboardToolStripMenuItem.Text = "Copy journal entry to clipboard";
            this.copyJournalEntryToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyJournalEntryToClipboardToolStripMenuItem_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 120000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
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
            this.toolTip.SetToolTip(this.comboBoxTime, "Select the entries selected by age");
            this.comboBoxTime.ValueMember = "";
            this.comboBoxTime.SelectedIndexChanged += new System.EventHandler(this.comboBoxJournalWindow_SelectedIndexChanged);
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
            this.toolTip.SetToolTip(this.extButtonTimeRanges, "Define new time ranges for time selector");
            this.extButtonTimeRanges.UseVisualStyleBackColor = true;
            this.extButtonTimeRanges.Click += new System.EventHandler(this.extButtonTimeRanges_Click);
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
            this.extComboBoxQuickMarks.TabIndex = 31;
            this.extComboBoxQuickMarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.extComboBoxQuickMarks, "Go to a marked journal entry. Use right click to mark an entry");
            this.extComboBoxQuickMarks.ValueMember = "";
            this.extComboBoxQuickMarks.SelectedIndexChanged += new System.EventHandler(this.extComboBoxQuickMarks_SelectedIndexChanged);
            // 
            // buttonFilter
            // 
            this.buttonFilter.BackColor2 = System.Drawing.Color.Red;
            this.buttonFilter.ButtonDisabledScaling = 0.5F;
            this.buttonFilter.GradientDirection = 90F;
            this.buttonFilter.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.buttonFilter.Location = new System.Drawing.Point(525, 1);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonFilter.MouseOverScaling = 1.3F;
            this.buttonFilter.MouseSelectedScaling = 1.3F;
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(28, 28);
            this.buttonFilter.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonFilter, "Filter out entries based on event type");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // buttonField
            // 
            this.buttonField.BackColor2 = System.Drawing.Color.Red;
            this.buttonField.ButtonDisabledScaling = 0.5F;
            this.buttonField.GradientDirection = 90F;
            this.buttonField.Image = global::EDDiscovery.Icons.Controls.FieldFilter;
            this.buttonField.Location = new System.Drawing.Point(559, 1);
            this.buttonField.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonField.MouseOverScaling = 1.3F;
            this.buttonField.MouseSelectedScaling = 1.3F;
            this.buttonField.Name = "buttonField";
            this.buttonField.Size = new System.Drawing.Size(28, 28);
            this.buttonField.TabIndex = 25;
            this.toolTip.SetToolTip(this.buttonField, "Filter out entries matching the field selection");
            this.buttonField.UseVisualStyleBackColor = true;
            this.buttonField.Click += new System.EventHandler(this.buttonField_Click);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtExcel.ButtonDisabledScaling = 0.5F;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.GradientDirection = 90F;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(627, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtExcel.MouseOverScaling = 1.3F;
            this.buttonExtExcel.MouseSelectedScaling = 1.3F;
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 30;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
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
            this.checkBoxCursorToTop.Location = new System.Drawing.Point(661, 1);
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
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewJournal);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 24;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(804, 686);
            this.dataViewScrollerPanel.TabIndex = 7;
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
            this.vScrollBarCustom.Location = new System.Drawing.Point(780, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(24, 686);
            this.vScrollBarCustom.SkinnyStyle = ExtendedControls.ExtScrollBar.ScrollStyle.Normal;
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SliderDrawAngle = 90F;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 7;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -1;
            this.vScrollBarCustom.ValueLimited = -1;
            // 
            // dataGridViewJournal
            // 
            this.dataGridViewJournal.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewJournal.AllowUserToAddRows = false;
            this.dataGridViewJournal.AllowUserToDeleteRows = false;
            this.dataGridViewJournal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewJournal.AutoSortByColumnName = false;
            this.dataGridViewJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewJournal.ColumnReorder = true;
            this.dataGridViewJournal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.ColumnEvent,
            this.ColumnDescription,
            this.ColumnInformation});
            this.dataGridViewJournal.ContextMenuStrip = this.historyContextMenu;
            this.dataGridViewJournal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewJournal.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewJournal.Name = "dataGridViewJournal";
            this.dataGridViewJournal.PerColumnWordWrapControl = true;
            this.dataGridViewJournal.RowHeaderMenuStrip = null;
            this.dataGridViewJournal.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewJournal.SingleRowSelect = true;
            this.dataGridViewJournal.Size = new System.Drawing.Size(780, 686);
            this.dataGridViewJournal.TabIndex = 0;
            this.dataGridViewJournal.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewJournal_CellClick);
            this.dataGridViewJournal.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewJournal_CellDoubleClick);
            this.dataGridViewJournal.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewJournal_RowPostPaint);
            this.dataGridViewJournal.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewJournal_SortCompare);
            this.dataGridViewJournal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewJournal_MouseDown);
            // 
            // ColumnTime
            // 
            this.ColumnTime.HeaderText = "Time";
            this.ColumnTime.MinimumWidth = 50;
            this.ColumnTime.Name = "ColumnTime";
            this.ColumnTime.ReadOnly = true;
            // 
            // ColumnEvent
            // 
            this.ColumnEvent.FillWeight = 50F;
            this.ColumnEvent.HeaderText = "Event";
            this.ColumnEvent.MinimumWidth = 50;
            this.ColumnEvent.Name = "ColumnEvent";
            this.ColumnEvent.ReadOnly = true;
            this.ColumnEvent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnDescription
            // 
            this.ColumnDescription.FillWeight = 125F;
            this.ColumnDescription.HeaderText = "Description";
            this.ColumnDescription.MinimumWidth = 50;
            this.ColumnDescription.Name = "ColumnDescription";
            this.ColumnDescription.ReadOnly = true;
            // 
            // ColumnInformation
            // 
            this.ColumnInformation.FillWeight = 300F;
            this.ColumnInformation.HeaderText = "Information";
            this.ColumnInformation.MinimumWidth = 50;
            this.ColumnInformation.Name = "ColumnInformation";
            this.ColumnInformation.ReadOnly = true;
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.labelTime);
            this.panelTop.Controls.Add(this.comboBoxTime);
            this.panelTop.Controls.Add(this.extButtonTimeRanges);
            this.panelTop.Controls.Add(this.labelSearch);
            this.panelTop.Controls.Add(this.textBoxSearch);
            this.panelTop.Controls.Add(this.extComboBoxQuickMarks);
            this.panelTop.Controls.Add(this.buttonFilter);
            this.panelTop.Controls.Add(this.buttonField);
            this.panelTop.Controls.Add(this.extButtonEventColours);
            this.panelTop.Controls.Add(this.buttonExtExcel);
            this.panelTop.Controls.Add(this.checkBoxCursorToTop);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(804, 30);
            this.panelTop.TabIndex = 31;
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
            // extButtonEventColours
            // 
            this.extButtonEventColours.BackColor2 = System.Drawing.Color.Red;
            this.extButtonEventColours.ButtonDisabledScaling = 0.5F;
            this.extButtonEventColours.GradientDirection = 90F;
            this.extButtonEventColours.Image = global::EDDiscovery.Icons.Controls.ColourSelector;
            this.extButtonEventColours.Location = new System.Drawing.Point(593, 1);
            this.extButtonEventColours.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonEventColours.MouseOverScaling = 1.3F;
            this.extButtonEventColours.MouseSelectedScaling = 1.3F;
            this.extButtonEventColours.Name = "extButtonEventColours";
            this.extButtonEventColours.Size = new System.Drawing.Size(28, 28);
            this.extButtonEventColours.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonEventColours, "Colour events by type");
            this.extButtonEventColours.UseVisualStyleBackColor = true;
            this.extButtonEventColours.Click += new System.EventHandler(this.extButtonEventColours_Click);
            // 
            // UserControlJournalGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlJournalGrid";
            this.Size = new System.Drawing.Size(804, 716);
            this.historyContextMenu.ResumeLayout(false);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJournal)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private BaseUtils.DataGridViewColumnControl dataGridViewJournal;
        private ExtendedControls.ExtTextBox textBoxSearch;
        private System.Windows.Forms.Label labelSearch;
        private ExtendedControls.ExtButton buttonFilter;
        private ExtendedControls.ExtComboBox comboBoxTime;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.ContextMenuStrip historyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStartStop;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonField;
        private System.Windows.Forms.ToolStripMenuItem runActionsOnThisEntryToolStripMenuItem;
        private ExtendedControls.ExtButton buttonExtExcel;
        private System.Windows.Forms.ToolStripMenuItem copyJournalEntryToClipboardToolStripMenuItem;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInformation;
        private System.Windows.Forms.ToolStripMenuItem removeSortingOfColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpToEntryToolStripMenuItem;
        private ExtendedControls.ExtCheckBox checkBoxCursorToTop;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private System.Windows.Forms.ToolStripMenuItem viewOnSpanshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewScanDisplayToolStripMenuItem;
        private ExtendedControls.ExtButton extButtonTimeRanges;
        internal ExtendedControls.ExtComboBox extComboBoxQuickMarks;
        private System.Windows.Forms.ToolStripMenuItem quickMarkToolStripMenuItem;
        private ExtendedControls.ExtButton extButtonEventColours;
    }
}
