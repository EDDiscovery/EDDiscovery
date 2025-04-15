﻿/*
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
    partial class UserControlTravelGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTravelGrid));
            this.extCheckBoxOutlines = new ExtendedControls.ExtCheckBox();
            this.checkBoxCursorToTop = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.buttonField = new ExtendedControls.ExtButton();
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.textBoxSearch = new ExtendedControls.ExtTextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.comboBoxTime = new ExtendedControls.ExtComboBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.historyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quickMarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeSortingOfColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoEntryNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createEditBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoNextStartStopMarkerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewScanDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnSpanshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.starMapColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToTrilaterationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trilaterationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wantedSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bothToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.expeditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToAnotherCommanderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeJournalEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runActionsOnThisEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyJournalEntryToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeEventInfoToLogDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runActionsAcrossSelectionToolSpeechStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSelectionThroughInaraSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runEntryThroughProfileSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSelectionThroughEDDNThruTestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSelectionThroughEDAstroDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendJournalEntriesToDLLsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showSystemVisitedForeColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.travelGridInDebugModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.extButtonTimeRanges = new ExtendedControls.ExtButton();
            this.extComboBoxQuickMarks = new ExtendedControls.ExtComboBox();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.panelOutlining = new ExtendedControls.ExtPanelDataGridViewScrollOutlining();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridViewTravel = new BaseUtils.DataGridViewColumnControl();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripOutlines = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.outliningOnOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanEventsOutliningOnOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRollUpOlderOutlines = new System.Windows.Forms.ToolStripMenuItem();
            this.rollUpOffToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rollUpAfterFirstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rollUpAfter5ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonDrawnHelp = new ExtendedControls.ExtButtonDrawn();
            this.historyContextMenu.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).BeginInit();
            this.contextMenuStripOutlines.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // extCheckBoxOutlines
            // 
            this.extCheckBoxOutlines.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxOutlines.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxOutlines.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxOutlines.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxOutlines.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxOutlines.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxOutlines.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxOutlines.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxOutlines.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxOutlines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxOutlines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxOutlines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxOutlines.Image = global::EDDiscovery.Icons.Controls.Outlines;
            this.extCheckBoxOutlines.ImageIndeterminate = null;
            this.extCheckBoxOutlines.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxOutlines.ImageUnchecked = null;
            this.extCheckBoxOutlines.Location = new System.Drawing.Point(714, 1);
            this.extCheckBoxOutlines.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extCheckBoxOutlines.Name = "extCheckBoxOutlines";
            this.extCheckBoxOutlines.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxOutlines.TabIndex = 30;
            this.extCheckBoxOutlines.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxOutlines, "Control Outlining");
            this.extCheckBoxOutlines.UseVisualStyleBackColor = false;
            this.extCheckBoxOutlines.Click += new System.EventHandler(this.extButtonOutlines_Click);
            // 
            // checkBoxCursorToTop
            // 
            this.checkBoxCursorToTop.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCursorToTop.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxCursorToTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxCursorToTop.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxCursorToTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCursorToTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCursorToTop.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxCursorToTop.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxCursorToTop.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxCursorToTop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxCursorToTop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxCursorToTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCursorToTop.Image = global::EDDiscovery.Icons.Controls.CursorToTop;
            this.checkBoxCursorToTop.ImageIndeterminate = null;
            this.checkBoxCursorToTop.ImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxCursorToTop.ImageUnchecked = global::EDDiscovery.Icons.Controls.CursorStill;
            this.checkBoxCursorToTop.Location = new System.Drawing.Point(642, 1);
            this.checkBoxCursorToTop.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.checkBoxCursorToTop.Name = "checkBoxCursorToTop";
            this.checkBoxCursorToTop.Size = new System.Drawing.Size(28, 28);
            this.checkBoxCursorToTop.TabIndex = 30;
            this.checkBoxCursorToTop.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCursorToTop, "Automatically move the cursor to the latest entry when it arrives");
            this.checkBoxCursorToTop.UseVisualStyleBackColor = false;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(606, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 28;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // buttonField
            // 
            this.buttonField.Image = global::EDDiscovery.Icons.Controls.FieldFilter;
            this.buttonField.Location = new System.Drawing.Point(570, 1);
            this.buttonField.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonField.Name = "buttonField";
            this.buttonField.Size = new System.Drawing.Size(28, 28);
            this.buttonField.TabIndex = 25;
            this.toolTip.SetToolTip(this.buttonField, "Filter out entries matching the field selection");
            this.buttonField.UseVisualStyleBackColor = true;
            this.buttonField.Click += new System.EventHandler(this.buttonField_Click);
            // 
            // buttonFilter
            // 
            this.buttonFilter.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.buttonFilter.Location = new System.Drawing.Point(534, 1);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(28, 28);
            this.buttonFilter.TabIndex = 25;
            this.toolTip.SetToolTip(this.buttonFilter, "Filter out entries based on event type");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxSearch.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxSearch.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearch.ClearOnFirstChar = false;
            this.textBoxSearch.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSearch.EndButtonEnable = true;
            this.textBoxSearch.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxSearch.EndButtonImage")));
            this.textBoxSearch.EndButtonSize16ths = 10;
            this.textBoxSearch.EndButtonVisible = false;
            this.textBoxSearch.InErrorCondition = false;
            this.textBoxSearch.Location = new System.Drawing.Point(231, 3);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
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
            this.labelSearch.Location = new System.Drawing.Point(182, 3);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 24;
            this.labelSearch.Text = "Search";
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTime.DataSource = null;
            this.comboBoxTime.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTime.DisplayMember = "";
            this.comboBoxTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTime.Location = new System.Drawing.Point(38, 3);
            this.comboBoxTime.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
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
            this.labelTime.Location = new System.Drawing.Point(0, 3);
            this.labelTime.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(30, 13);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "Time";
            // 
            // historyContextMenu
            // 
            this.historyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quickMarkToolStripMenuItem,
            this.removeSortingOfColumnsToolStripMenuItem,
            this.gotoEntryNumberToolStripMenuItem,
            this.setNoteToolStripMenuItem,
            this.createEditBookmarkToolStripMenuItem,
            this.toolStripMenuItemStartStop,
            this.gotoNextStartStopMarkerToolStripMenuItem,
            this.mapGotoStartoolStripMenuItem,
            this.viewScanDisplayToolStripMenuItem,
            this.viewOnSpanshToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.starMapColourToolStripMenuItem,
            this.addToTrilaterationToolStripMenuItem,
            this.moveToAnotherCommanderToolStripMenuItem,
            this.hideSystemToolStripMenuItem,
            this.removeJournalEntryToolStripMenuItem,
            this.runActionsOnThisEntryToolStripMenuItem,
            this.copyJournalEntryToClipboardToolStripMenuItem,
            this.writeEventInfoToLogDebugToolStripMenuItem,
            this.runActionsAcrossSelectionToolSpeechStripMenuItem,
            this.runSelectionThroughInaraSystemToolStripMenuItem,
            this.runEntryThroughProfileSystemToolStripMenuItem,
            this.runSelectionThroughEDDNThruTestToolStripMenuItem,
            this.runSelectionThroughEDAstroDebugToolStripMenuItem,
            this.sendJournalEntriesToDLLsToolStripMenuItem,
            this.showSystemVisitedForeColourToolStripMenuItem,
            this.travelGridInDebugModeToolStripMenuItem});
            this.historyContextMenu.Name = "historyContextMenu";
            this.historyContextMenu.Size = new System.Drawing.Size(388, 620);
            this.historyContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.historyContextMenu_Opening);
            // 
            // quickMarkToolStripMenuItem
            // 
            this.quickMarkToolStripMenuItem.Checked = true;
            this.quickMarkToolStripMenuItem.CheckOnClick = true;
            this.quickMarkToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.quickMarkToolStripMenuItem.Name = "quickMarkToolStripMenuItem";
            this.quickMarkToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.quickMarkToolStripMenuItem.Text = "Marked";
            this.quickMarkToolStripMenuItem.Click += new System.EventHandler(this.quickMarkToolStripMenuItem_Click);
            // 
            // removeSortingOfColumnsToolStripMenuItem
            // 
            this.removeSortingOfColumnsToolStripMenuItem.Name = "removeSortingOfColumnsToolStripMenuItem";
            this.removeSortingOfColumnsToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.removeSortingOfColumnsToolStripMenuItem.Text = "Remove Sorting of Columns";
            this.removeSortingOfColumnsToolStripMenuItem.Click += new System.EventHandler(this.removeSortingOfColumnsToolStripMenuItem_Click);
            // 
            // gotoEntryNumberToolStripMenuItem
            // 
            this.gotoEntryNumberToolStripMenuItem.Name = "gotoEntryNumberToolStripMenuItem";
            this.gotoEntryNumberToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.gotoEntryNumberToolStripMenuItem.Text = "Jump to Entry";
            this.gotoEntryNumberToolStripMenuItem.Click += new System.EventHandler(this.gotoEntryNumberToolStripMenuItem_Click);
            // 
            // setNoteToolStripMenuItem
            // 
            this.setNoteToolStripMenuItem.Name = "setNoteToolStripMenuItem";
            this.setNoteToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.setNoteToolStripMenuItem.Text = "Set Note";
            this.setNoteToolStripMenuItem.Click += new System.EventHandler(this.setNoteToolStripMenuItem_Click);
            // 
            // createEditBookmarkToolStripMenuItem
            // 
            this.createEditBookmarkToolStripMenuItem.Name = "createEditBookmarkToolStripMenuItem";
            this.createEditBookmarkToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.createEditBookmarkToolStripMenuItem.Text = "Create/Edit Bookmark";
            this.createEditBookmarkToolStripMenuItem.Click += new System.EventHandler(this.createEditBookmarkToolStripMenuItem_Click);
            // 
            // toolStripMenuItemStartStop
            // 
            this.toolStripMenuItemStartStop.Name = "toolStripMenuItemStartStop";
            this.toolStripMenuItemStartStop.Size = new System.Drawing.Size(387, 22);
            this.toolStripMenuItemStartStop.Text = "Set Start/Stop point for travel calculations";
            this.toolStripMenuItemStartStop.Click += new System.EventHandler(this.toolStripMenuItemStartStop_Click);
            // 
            // gotoNextStartStopMarkerToolStripMenuItem
            // 
            this.gotoNextStartStopMarkerToolStripMenuItem.Name = "gotoNextStartStopMarkerToolStripMenuItem";
            this.gotoNextStartStopMarkerToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.gotoNextStartStopMarkerToolStripMenuItem.Text = "Jump to next Start/Stop marker";
            this.gotoNextStartStopMarkerToolStripMenuItem.Click += new System.EventHandler(this.gotoNextStartStopMarkerToolStripMenuItem_Click);
            // 
            // mapGotoStartoolStripMenuItem
            // 
            this.mapGotoStartoolStripMenuItem.Name = "mapGotoStartoolStripMenuItem";
            this.mapGotoStartoolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.mapGotoStartoolStripMenuItem.Text = "Go to star on 3D Map";
            this.mapGotoStartoolStripMenuItem.Click += new System.EventHandler(this.mapGotoStartoolStripMenuItem_Click);
            // 
            // viewScanDisplayToolStripMenuItem
            // 
            this.viewScanDisplayToolStripMenuItem.Name = "viewScanDisplayToolStripMenuItem";
            this.viewScanDisplayToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.viewScanDisplayToolStripMenuItem.Text = "View Scan Display";
            this.viewScanDisplayToolStripMenuItem.Click += new System.EventHandler(this.viewScanDisplayToolStripMenuItem_Click);
            // 
            // viewOnSpanshToolStripMenuItem
            // 
            this.viewOnSpanshToolStripMenuItem.Name = "viewOnSpanshToolStripMenuItem";
            this.viewOnSpanshToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.viewOnSpanshToolStripMenuItem.Text = "View on Spansh";
            this.viewOnSpanshToolStripMenuItem.Click += new System.EventHandler(this.viewOnSpanshToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // starMapColourToolStripMenuItem
            // 
            this.starMapColourToolStripMenuItem.Name = "starMapColourToolStripMenuItem";
            this.starMapColourToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.starMapColourToolStripMenuItem.Text = "Star Map Colour...";
            this.starMapColourToolStripMenuItem.Click += new System.EventHandler(this.starMapColourToolStripMenuItem_Click);
            // 
            // addToTrilaterationToolStripMenuItem
            // 
            this.addToTrilaterationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trilaterationToolStripMenuItem,
            this.wantedSystemsToolStripMenuItem,
            this.bothToolStripMenuItem,
            this.expeditionToolStripMenuItem});
            this.addToTrilaterationToolStripMenuItem.Name = "addToTrilaterationToolStripMenuItem";
            this.addToTrilaterationToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.addToTrilaterationToolStripMenuItem.Text = "Add to ...";
            // 
            // trilaterationToolStripMenuItem
            // 
            this.trilaterationToolStripMenuItem.Name = "trilaterationToolStripMenuItem";
            this.trilaterationToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.trilaterationToolStripMenuItem.Text = "System on Trilateration Panel";
            this.trilaterationToolStripMenuItem.Click += new System.EventHandler(this.trilaterationToolStripMenuItem_Click);
            // 
            // wantedSystemsToolStripMenuItem
            // 
            this.wantedSystemsToolStripMenuItem.Name = "wantedSystemsToolStripMenuItem";
            this.wantedSystemsToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.wantedSystemsToolStripMenuItem.Text = "Wanted Systems on Trilateration Panel";
            this.wantedSystemsToolStripMenuItem.Click += new System.EventHandler(this.wantedSystemsToolStripMenuItem_Click);
            // 
            // bothToolStripMenuItem
            // 
            this.bothToolStripMenuItem.Name = "bothToolStripMenuItem";
            this.bothToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.bothToolStripMenuItem.Text = "Both of the above";
            this.bothToolStripMenuItem.Click += new System.EventHandler(this.bothToolStripMenuItem_Click);
            // 
            // expeditionToolStripMenuItem
            // 
            this.expeditionToolStripMenuItem.Name = "expeditionToolStripMenuItem";
            this.expeditionToolStripMenuItem.Size = new System.Drawing.Size(275, 22);
            this.expeditionToolStripMenuItem.Text = "Expedition Panel";
            this.expeditionToolStripMenuItem.Click += new System.EventHandler(this.expeditionToolStripMenuItem_Click);
            // 
            // moveToAnotherCommanderToolStripMenuItem
            // 
            this.moveToAnotherCommanderToolStripMenuItem.Name = "moveToAnotherCommanderToolStripMenuItem";
            this.moveToAnotherCommanderToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.moveToAnotherCommanderToolStripMenuItem.Text = "Move Entries to another Commander";
            this.moveToAnotherCommanderToolStripMenuItem.Click += new System.EventHandler(this.moveToAnotherCommanderToolStripMenuItem_Click);
            // 
            // hideSystemToolStripMenuItem
            // 
            this.hideSystemToolStripMenuItem.Name = "hideSystemToolStripMenuItem";
            this.hideSystemToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.hideSystemToolStripMenuItem.Text = "Hide Entries";
            this.hideSystemToolStripMenuItem.Click += new System.EventHandler(this.hideSystemToolStripMenuItem_Click);
            // 
            // removeJournalEntryToolStripMenuItem
            // 
            this.removeJournalEntryToolStripMenuItem.Name = "removeJournalEntryToolStripMenuItem";
            this.removeJournalEntryToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.removeJournalEntryToolStripMenuItem.Text = "Remove Journal Entry";
            this.removeJournalEntryToolStripMenuItem.Click += new System.EventHandler(this.removeJournalEntryToolStripMenuItem_Click);
            // 
            // runActionsOnThisEntryToolStripMenuItem
            // 
            this.runActionsOnThisEntryToolStripMenuItem.Name = "runActionsOnThisEntryToolStripMenuItem";
            this.runActionsOnThisEntryToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.runActionsOnThisEntryToolStripMenuItem.Text = "Run Actions on this entry";
            this.runActionsOnThisEntryToolStripMenuItem.Click += new System.EventHandler(this.runActionsOnThisEntryToolStripMenuItem_Click);
            // 
            // copyJournalEntryToClipboardToolStripMenuItem
            // 
            this.copyJournalEntryToClipboardToolStripMenuItem.Name = "copyJournalEntryToClipboardToolStripMenuItem";
            this.copyJournalEntryToClipboardToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.copyJournalEntryToClipboardToolStripMenuItem.Text = "Copy journal entry to clipboard/Log";
            this.copyJournalEntryToClipboardToolStripMenuItem.Click += new System.EventHandler(this.copyJournalEntryToClipboardToolStripMenuItem_Click);
            // 
            // writeEventInfoToLogDebugToolStripMenuItem
            // 
            this.writeEventInfoToLogDebugToolStripMenuItem.Name = "writeEventInfoToLogDebugToolStripMenuItem";
            this.writeEventInfoToLogDebugToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.writeEventInfoToLogDebugToolStripMenuItem.Text = "Write event class info to Log (Debug)";
            this.writeEventInfoToLogDebugToolStripMenuItem.Click += new System.EventHandler(this.writeEventInfoToLogDebugToolStripMenuItem_Click);
            // 
            // runActionsAcrossSelectionToolSpeechStripMenuItem
            // 
            this.runActionsAcrossSelectionToolSpeechStripMenuItem.Name = "runActionsAcrossSelectionToolSpeechStripMenuItem";
            this.runActionsAcrossSelectionToolSpeechStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.runActionsAcrossSelectionToolSpeechStripMenuItem.Text = "Run actions across selection for speech debugging (Debug)";
            this.runActionsAcrossSelectionToolSpeechStripMenuItem.Click += new System.EventHandler(this.runActionsAcrossSelectionToolStripMenuItem_Click);
            // 
            // runSelectionThroughInaraSystemToolStripMenuItem
            // 
            this.runSelectionThroughInaraSystemToolStripMenuItem.Name = "runSelectionThroughInaraSystemToolStripMenuItem";
            this.runSelectionThroughInaraSystemToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.runSelectionThroughInaraSystemToolStripMenuItem.Text = "Run selection through Inara System (Debug)";
            this.runSelectionThroughInaraSystemToolStripMenuItem.Click += new System.EventHandler(this.runSelectionThroughInaraSystemToolStripMenuItem_Click);
            // 
            // runEntryThroughProfileSystemToolStripMenuItem
            // 
            this.runEntryThroughProfileSystemToolStripMenuItem.Name = "runEntryThroughProfileSystemToolStripMenuItem";
            this.runEntryThroughProfileSystemToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.runEntryThroughProfileSystemToolStripMenuItem.Text = "Run entry through Profile System (Debug)";
            this.runEntryThroughProfileSystemToolStripMenuItem.Click += new System.EventHandler(this.runEntryThroughProfileSystemToolStripMenuItem_Click);
            // 
            // runSelectionThroughEDDNThruTestToolStripMenuItem
            // 
            this.runSelectionThroughEDDNThruTestToolStripMenuItem.Name = "runSelectionThroughEDDNThruTestToolStripMenuItem";
            this.runSelectionThroughEDDNThruTestToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.runSelectionThroughEDDNThruTestToolStripMenuItem.Text = "Run selection through EDDN Test Server (Debug)";
            this.runSelectionThroughEDDNThruTestToolStripMenuItem.Click += new System.EventHandler(this.runSelectionThroughEDDNDebugNoSendToolStripMenuItem_Click);
            // 
            // runSelectionThroughEDAstroDebugToolStripMenuItem
            // 
            this.runSelectionThroughEDAstroDebugToolStripMenuItem.Name = "runSelectionThroughEDAstroDebugToolStripMenuItem";
            this.runSelectionThroughEDAstroDebugToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.runSelectionThroughEDAstroDebugToolStripMenuItem.Text = "Run selection through EDAstro (Debug)";
            this.runSelectionThroughEDAstroDebugToolStripMenuItem.Click += new System.EventHandler(this.runSelectionThroughEDAstroDebugToolStripMenuItem_Click);
            // 
            // sendJournalEntriesToDLLsToolStripMenuItem
            // 
            this.sendJournalEntriesToDLLsToolStripMenuItem.Name = "sendJournalEntriesToDLLsToolStripMenuItem";
            this.sendJournalEntriesToDLLsToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.sendJournalEntriesToDLLsToolStripMenuItem.Text = "Send selection to DLLs (Debug)";
            this.sendJournalEntriesToDLLsToolStripMenuItem.Click += new System.EventHandler(this.sendJournalEntriesToDLLsToolStripMenuItem_Click);
            // 
            // showSystemVisitedForeColourToolStripMenuItem
            // 
            this.showSystemVisitedForeColourToolStripMenuItem.Checked = true;
            this.showSystemVisitedForeColourToolStripMenuItem.CheckOnClick = true;
            this.showSystemVisitedForeColourToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showSystemVisitedForeColourToolStripMenuItem.Name = "showSystemVisitedForeColourToolStripMenuItem";
            this.showSystemVisitedForeColourToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.showSystemVisitedForeColourToolStripMenuItem.Text = "Show lines in System Visited Fore Colour";
            // 
            // travelGridInDebugModeToolStripMenuItem
            // 
            this.travelGridInDebugModeToolStripMenuItem.Checked = true;
            this.travelGridInDebugModeToolStripMenuItem.CheckOnClick = true;
            this.travelGridInDebugModeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.travelGridInDebugModeToolStripMenuItem.Name = "travelGridInDebugModeToolStripMenuItem";
            this.travelGridInDebugModeToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.travelGridInDebugModeToolStripMenuItem.Text = "Travel grid in Debug Mode";
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 1000;
            this.toolTip.ShowAlways = true;
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(678, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 30;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // extButtonTimeRanges
            // 
            this.extButtonTimeRanges.Image = global::EDDiscovery.Icons.Controls.Clock;
            this.extButtonTimeRanges.Location = new System.Drawing.Point(146, 1);
            this.extButtonTimeRanges.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extButtonTimeRanges.Name = "extButtonTimeRanges";
            this.extButtonTimeRanges.Size = new System.Drawing.Size(28, 28);
            this.extButtonTimeRanges.TabIndex = 25;
            this.toolTip.SetToolTip(this.extButtonTimeRanges, "Define new time ranges for time selector");
            this.extButtonTimeRanges.UseVisualStyleBackColor = true;
            this.extButtonTimeRanges.Click += new System.EventHandler(this.extButtonTimeRanges_Click);
            // 
            // extComboBoxQuickMarks
            // 
            this.extComboBoxQuickMarks.BorderColor = System.Drawing.Color.Red;
            this.extComboBoxQuickMarks.DataSource = null;
            this.extComboBoxQuickMarks.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxQuickMarks.DisplayMember = "";
            this.extComboBoxQuickMarks.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxQuickMarks.Location = new System.Drawing.Point(387, 3);
            this.extComboBoxQuickMarks.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
            this.extComboBoxQuickMarks.Name = "extComboBoxQuickMarks";
            this.extComboBoxQuickMarks.SelectedIndex = -1;
            this.extComboBoxQuickMarks.SelectedItem = null;
            this.extComboBoxQuickMarks.SelectedValue = null;
            this.extComboBoxQuickMarks.Size = new System.Drawing.Size(139, 21);
            this.extComboBoxQuickMarks.TabIndex = 0;
            this.extComboBoxQuickMarks.Text = "Go to a marked entry";
            this.extComboBoxQuickMarks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.extComboBoxQuickMarks, "Go to a marked journal entry. Use right click to mark an entry");
            this.extComboBoxQuickMarks.ValueMember = "";
            this.extComboBoxQuickMarks.SelectedIndexChanged += new System.EventHandler(this.extComboBoxBookmark_SelectedIndexChanged);
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.panelOutlining);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewTravel);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(859, 616);
            this.dataViewScrollerPanel.TabIndex = 28;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // panelOutlining
            // 
            this.panelOutlining.KeepLastEntriesVisibleOnRollUp = 1;
            this.panelOutlining.Location = new System.Drawing.Point(0, 0);
            this.panelOutlining.Name = "panelOutlining";
            this.panelOutlining.Size = new System.Drawing.Size(2, 616);
            this.panelOutlining.TabIndex = 5;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.AlwaysHideScrollBar = false;
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = true;
            this.vScrollBarCustom.LargeChange = 0;
            this.vScrollBarCustom.Location = new System.Drawing.Point(837, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(19, 616);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 4;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -1;
            this.vScrollBarCustom.ValueLimited = -1;
            // 
            // dataGridViewTravel
            // 
            this.dataGridViewTravel.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewTravel.AllowUserToAddRows = false;
            this.dataGridViewTravel.AllowUserToDeleteRows = false;
            this.dataGridViewTravel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTravel.AutoSortByColumnName = false;
            this.dataGridViewTravel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTravel.ColumnReorder = true;
            this.dataGridViewTravel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.ColumnEvent,
            this.ColumnDescription,
            this.ColumnInformation,
            this.ColumnNote});
            this.dataGridViewTravel.ContextMenuStrip = this.historyContextMenu;
            this.dataGridViewTravel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTravel.Location = new System.Drawing.Point(2, 0);
            this.dataGridViewTravel.Name = "dataGridViewTravel";
            this.dataGridViewTravel.PerColumnWordWrapControl = true;
            this.dataGridViewTravel.RowHeaderMenuStrip = null;
            this.dataGridViewTravel.RowHeadersWidth = 100;
            this.dataGridViewTravel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewTravel.SingleRowSelect = true;
            this.dataGridViewTravel.Size = new System.Drawing.Size(835, 616);
            this.dataGridViewTravel.TabIndex = 3;
            this.dataGridViewTravel.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellClick);
            this.dataGridViewTravel.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellDoubleClick);
            this.dataGridViewTravel.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellEndEdit);
            this.dataGridViewTravel.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTravel_ColumnHeaderMouseClick);
            this.dataGridViewTravel.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewTravel_RowPostPaint);
            this.dataGridViewTravel.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewTravel_SortCompare);
            this.dataGridViewTravel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewTravel_KeyDown);
            this.dataGridViewTravel.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dataGridViewTravel_KeyUp);
            this.dataGridViewTravel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewTravel_MouseDown);
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
            this.ColumnDescription.HeaderText = "Description";
            this.ColumnDescription.MinimumWidth = 50;
            this.ColumnDescription.Name = "ColumnDescription";
            this.ColumnDescription.ReadOnly = true;
            // 
            // ColumnInformation
            // 
            this.ColumnInformation.FillWeight = 200F;
            this.ColumnInformation.HeaderText = "Information";
            this.ColumnInformation.MinimumWidth = 50;
            this.ColumnInformation.Name = "ColumnInformation";
            this.ColumnInformation.ReadOnly = true;
            // 
            // ColumnNote
            // 
            this.ColumnNote.HeaderText = "Note";
            this.ColumnNote.MinimumWidth = 20;
            this.ColumnNote.Name = "ColumnNote";
            // 
            // contextMenuStripOutlines
            // 
            this.contextMenuStripOutlines.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.outliningOnOffToolStripMenuItem,
            this.scanEventsOutliningOnOffToolStripMenuItem,
            this.toolStripRollUpOlderOutlines});
            this.contextMenuStripOutlines.Name = "contextMenuStripOutlines";
            this.contextMenuStripOutlines.Size = new System.Drawing.Size(190, 70);
            // 
            // outliningOnOffToolStripMenuItem
            // 
            this.outliningOnOffToolStripMenuItem.Checked = true;
            this.outliningOnOffToolStripMenuItem.CheckOnClick = true;
            this.outliningOnOffToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.outliningOnOffToolStripMenuItem.Name = "outliningOnOffToolStripMenuItem";
            this.outliningOnOffToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.outliningOnOffToolStripMenuItem.Text = "Outlining On";
            // 
            // scanEventsOutliningOnOffToolStripMenuItem
            // 
            this.scanEventsOutliningOnOffToolStripMenuItem.Checked = true;
            this.scanEventsOutliningOnOffToolStripMenuItem.CheckOnClick = true;
            this.scanEventsOutliningOnOffToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scanEventsOutliningOnOffToolStripMenuItem.Name = "scanEventsOutliningOnOffToolStripMenuItem";
            this.scanEventsOutliningOnOffToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.scanEventsOutliningOnOffToolStripMenuItem.Text = "Scan Events Outlining";
            // 
            // toolStripRollUpOlderOutlines
            // 
            this.toolStripRollUpOlderOutlines.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rollUpOffToolStripMenuItem,
            this.rollUpAfterFirstToolStripMenuItem,
            this.rollUpAfter5ToolStripMenuItem});
            this.toolStripRollUpOlderOutlines.Name = "toolStripRollUpOlderOutlines";
            this.toolStripRollUpOlderOutlines.Size = new System.Drawing.Size(189, 22);
            this.toolStripRollUpOlderOutlines.Text = "Roll up older entries";
            // 
            // rollUpOffToolStripMenuItem
            // 
            this.rollUpOffToolStripMenuItem.Checked = true;
            this.rollUpOffToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rollUpOffToolStripMenuItem.Name = "rollUpOffToolStripMenuItem";
            this.rollUpOffToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.rollUpOffToolStripMenuItem.Text = "Off";
            // 
            // rollUpAfterFirstToolStripMenuItem
            // 
            this.rollUpAfterFirstToolStripMenuItem.Checked = true;
            this.rollUpAfterFirstToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rollUpAfterFirstToolStripMenuItem.Name = "rollUpAfterFirstToolStripMenuItem";
            this.rollUpAfterFirstToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.rollUpAfterFirstToolStripMenuItem.Text = "After First";
            // 
            // rollUpAfter5ToolStripMenuItem
            // 
            this.rollUpAfter5ToolStripMenuItem.Checked = true;
            this.rollUpAfter5ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.rollUpAfter5ToolStripMenuItem.Name = "rollUpAfter5ToolStripMenuItem";
            this.rollUpAfter5ToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.rollUpAfter5ToolStripMenuItem.Text = "After 5";
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
            this.panelTop.Controls.Add(this.buttonExtExcel);
            this.panelTop.Controls.Add(this.checkBoxCursorToTop);
            this.panelTop.Controls.Add(this.extCheckBoxWordWrap);
            this.panelTop.Controls.Add(this.extCheckBoxOutlines);
            this.panelTop.Controls.Add(this.extButtonDrawnHelp);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(859, 30);
            this.panelTop.TabIndex = 31;
            // 
            // extButtonDrawnHelp
            // 
            this.extButtonDrawnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelp.AutoEllipsis = false;
            this.extButtonDrawnHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelp.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelp.BorderWidth = 1;
            this.extButtonDrawnHelp.Image = null;
            this.extButtonDrawnHelp.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelp.Location = new System.Drawing.Point(753, 3);
            this.extButtonDrawnHelp.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelp.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelp.Name = "extButtonDrawnHelp";
            this.extButtonDrawnHelp.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelp.Selectable = true;
            this.extButtonDrawnHelp.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelp.TabIndex = 31;
            this.extButtonDrawnHelp.Text = "<code>";
            this.extButtonDrawnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelp.UseMnemonic = true;
            this.extButtonDrawnHelp.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // UserControlTravelGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlTravelGrid";
            this.Size = new System.Drawing.Size(859, 646);
            this.historyContextMenu.ResumeLayout(false);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).EndInit();
            this.contextMenuStripOutlines.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtButton buttonFilter;
        private ExtendedControls.ExtTextBox textBoxSearch;
        private System.Windows.Forms.Label labelSearch;
        internal ExtendedControls.ExtComboBox comboBoxTime;
        private System.Windows.Forms.Label labelTime;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        public BaseUtils.DataGridViewColumnControl dataGridViewTravel;
        private System.Windows.Forms.ContextMenuStrip historyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem starMapColourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToAnotherCommanderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToTrilaterationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trilaterationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wantedSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bothToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem expeditionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStartStop;
        private System.Windows.Forms.ToolStripMenuItem removeJournalEntryToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonField;
        private System.Windows.Forms.ToolStripMenuItem runActionsOnThisEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setNoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem writeEventInfoToLogDebugToolStripMenuItem;
        private ExtendedControls.ExtButton buttonExtExcel;
        private System.Windows.Forms.ToolStripMenuItem copyJournalEntryToClipboardToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createEditBookmarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runActionsAcrossSelectionToolSpeechStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeSortingOfColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSelectionThroughInaraSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoEntryNumberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoNextStartStopMarkerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runEntryThroughProfileSystemToolStripMenuItem;
        private ExtendedControls.ExtPanelDataGridViewScrollOutlining panelOutlining;
        private ExtendedControls.ExtCheckBox checkBoxCursorToTop;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOutlines;
        private System.Windows.Forms.ToolStripMenuItem outliningOnOffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanEventsOutliningOnOffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripRollUpOlderOutlines;
        private System.Windows.Forms.ToolStripMenuItem rollUpOffToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rollUpAfterFirstToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rollUpAfter5ToolStripMenuItem;
        private ExtendedControls.ExtCheckBox extCheckBoxOutlines;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private System.Windows.Forms.ToolStripMenuItem runSelectionThroughEDDNThruTestToolStripMenuItem;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelp;
        private System.Windows.Forms.ToolStripMenuItem showSystemVisitedForeColourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSelectionThroughEDAstroDebugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem travelGridInDebugModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendJournalEntriesToDLLsToolStripMenuItem;
        internal ExtendedControls.ExtComboBox extComboBoxQuickMarks;
        private System.Windows.Forms.ToolStripMenuItem quickMarkToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNote;
        private System.Windows.Forms.ToolStripMenuItem viewOnSpanshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewScanDisplayToolStripMenuItem;
        private ExtendedControls.ExtButton extButtonTimeRanges;
    }
}
