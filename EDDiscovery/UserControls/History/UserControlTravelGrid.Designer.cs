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
            this.TopPanel = new System.Windows.Forms.Panel();
            this.checkBoxMoveToTop = new ExtendedControls.CheckBoxCustom();
            this.buttonExtExcel = new ExtendedControls.ButtonExt();
            this.buttonField = new ExtendedControls.ButtonExt();
            this.buttonFilter = new ExtendedControls.ButtonExt();
            this.textBoxFilter = new ExtendedControls.TextBoxBorder();
            this.labelSearch = new System.Windows.Forms.Label();
            this.comboBoxHistoryWindow = new ExtendedControls.ComboBoxCustom();
            this.labelTime = new System.Windows.Forms.Label();
            this.historyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeSortingOfColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoEntryNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createEditBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoNextStartStopMarkerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.starMapColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToTrilaterationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trilaterationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wantedSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bothToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.routeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToAnotherCommanderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectCorrectSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeJournalEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendUnsyncedScanToEDDNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runActionsOnThisEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyJournalEntryToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeEventInfoToLogDebugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runActionsAcrossSelectionToolSpeechStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runSelectionThroughInaraSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runEntryThroughProfileSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel1 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewTravel = new System.Windows.Forms.DataGridView();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Icon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TopPanel.SuspendLayout();
            this.historyContextMenu.SuspendLayout();
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).BeginInit();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.checkBoxMoveToTop);
            this.TopPanel.Controls.Add(this.buttonExtExcel);
            this.TopPanel.Controls.Add(this.buttonField);
            this.TopPanel.Controls.Add(this.buttonFilter);
            this.TopPanel.Controls.Add(this.textBoxFilter);
            this.TopPanel.Controls.Add(this.labelSearch);
            this.TopPanel.Controls.Add(this.comboBoxHistoryWindow);
            this.TopPanel.Controls.Add(this.labelTime);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(870, 32);
            this.TopPanel.TabIndex = 27;
            // 
            // checkBoxMoveToTop
            // 
            this.checkBoxMoveToTop.AutoSize = true;
            this.checkBoxMoveToTop.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxMoveToTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMoveToTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMoveToTop.FontNerfReduction = 0.5F;
            this.checkBoxMoveToTop.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxMoveToTop.Location = new System.Drawing.Point(575, 7);
            this.checkBoxMoveToTop.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMoveToTop.Name = "checkBoxMoveToTop";
            this.checkBoxMoveToTop.Size = new System.Drawing.Size(90, 17);
            this.checkBoxMoveToTop.TabIndex = 29;
            this.checkBoxMoveToTop.Text = "Cursor to Top";
            this.checkBoxMoveToTop.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMoveToTop, "Select if cursor moves to top entry when a new entry is received");
            this.checkBoxMoveToTop.UseVisualStyleBackColor = true;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.TravelGrid_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(545, 4);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(24, 24);
            this.buttonExtExcel.TabIndex = 28;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // buttonField
            // 
            this.buttonField.Location = new System.Drawing.Point(464, 3);
            this.buttonField.Name = "buttonField";
            this.buttonField.Size = new System.Drawing.Size(75, 23);
            this.buttonField.TabIndex = 25;
            this.buttonField.Text = "Field Filter";
            this.toolTip.SetToolTip(this.buttonField, "Filter out entries matching the field selection");
            this.buttonField.UseVisualStyleBackColor = true;
            this.buttonField.Click += new System.EventHandler(this.buttonField_Click);
            // 
            // buttonFilter
            // 
            this.buttonFilter.Location = new System.Drawing.Point(383, 3);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonFilter.TabIndex = 25;
            this.buttonFilter.Text = "Event Filter";
            this.toolTip.SetToolTip(this.buttonFilter, "Filter out entries based on event type");
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
            this.textBoxFilter.InErrorCondition = false;
            this.textBoxFilter.Location = new System.Drawing.Point(217, 6);
            this.textBoxFilter.Multiline = false;
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.ReadOnly = false;
            this.textBoxFilter.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFilter.SelectionLength = 0;
            this.textBoxFilter.SelectionStart = 0;
            this.textBoxFilter.Size = new System.Drawing.Size(148, 20);
            this.textBoxFilter.TabIndex = 1;
            this.textBoxFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxFilter, "Enter text to search in any fields for an item");
            this.textBoxFilter.WordWrap = true;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(159, 7);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 24;
            this.labelSearch.Text = "Search";
            // 
            // comboBoxHistoryWindow
            // 
            this.comboBoxHistoryWindow.ArrowWidth = 1;
            this.comboBoxHistoryWindow.BorderColor = System.Drawing.Color.Red;
            this.comboBoxHistoryWindow.ButtonColorScaling = 0.5F;
            this.comboBoxHistoryWindow.DataSource = null;
            this.comboBoxHistoryWindow.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxHistoryWindow.DisplayMember = "";
            this.comboBoxHistoryWindow.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxHistoryWindow.DropDownHeight = 200;
            this.comboBoxHistoryWindow.DropDownWidth = 1;
            this.comboBoxHistoryWindow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxHistoryWindow.ItemHeight = 13;
            this.comboBoxHistoryWindow.Location = new System.Drawing.Point(49, 4);
            this.comboBoxHistoryWindow.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxHistoryWindow.Name = "comboBoxHistoryWindow";
            this.comboBoxHistoryWindow.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxHistoryWindow.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxHistoryWindow.ScrollBarWidth = 16;
            this.comboBoxHistoryWindow.SelectedIndex = -1;
            this.comboBoxHistoryWindow.SelectedItem = null;
            this.comboBoxHistoryWindow.SelectedValue = null;
            this.comboBoxHistoryWindow.Size = new System.Drawing.Size(100, 21);
            this.comboBoxHistoryWindow.TabIndex = 0;
            this.comboBoxHistoryWindow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxHistoryWindow, "Select the entries by age");
            this.comboBoxHistoryWindow.ValueMember = "";
            this.comboBoxHistoryWindow.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(3, 7);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(30, 13);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "Time";
            // 
            // historyContextMenu
            // 
            this.historyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSortingOfColumnsToolStripMenuItem,
            this.gotoEntryNumberToolStripMenuItem,
            this.setNoteToolStripMenuItem,
            this.createEditBookmarkToolStripMenuItem,
            this.toolStripMenuItemStartStop,
            this.gotoNextStartStopMarkerToolStripMenuItem,
            this.mapGotoStartoolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.starMapColourToolStripMenuItem,
            this.addToTrilaterationToolStripMenuItem,
            this.moveToAnotherCommanderToolStripMenuItem,
            this.hideSystemToolStripMenuItem,
            this.selectCorrectSystemToolStripMenuItem,
            this.removeJournalEntryToolStripMenuItem,
            this.sendUnsyncedScanToEDDNToolStripMenuItem,
            this.runActionsOnThisEntryToolStripMenuItem,
            this.copyJournalEntryToClipboardToolStripMenuItem,
            this.writeEventInfoToLogDebugToolStripMenuItem,
            this.runActionsAcrossSelectionToolSpeechStripMenuItem,
            this.runSelectionThroughInaraSystemToolStripMenuItem,
            this.runEntryThroughProfileSystemToolStripMenuItem});
            this.historyContextMenu.Name = "historyContextMenu";
            this.historyContextMenu.Size = new System.Drawing.Size(388, 466);
            this.historyContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.historyContextMenu_Opening);
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
            this.routeToolStripMenuItem});
            this.addToTrilaterationToolStripMenuItem.Name = "addToTrilaterationToolStripMenuItem";
            this.addToTrilaterationToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.addToTrilaterationToolStripMenuItem.Text = "Add to ...";
            // 
            // trilaterationToolStripMenuItem
            // 
            this.trilaterationToolStripMenuItem.Name = "trilaterationToolStripMenuItem";
            this.trilaterationToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.trilaterationToolStripMenuItem.Text = "System on Trilateration Panel";
            this.trilaterationToolStripMenuItem.Click += new System.EventHandler(this.trilaterationToolStripMenuItem_Click);
            // 
            // wantedSystemsToolStripMenuItem
            // 
            this.wantedSystemsToolStripMenuItem.Name = "wantedSystemsToolStripMenuItem";
            this.wantedSystemsToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.wantedSystemsToolStripMenuItem.Text = "Wanted Systems on Trilateration Panel";
            this.wantedSystemsToolStripMenuItem.Click += new System.EventHandler(this.wantedSystemsToolStripMenuItem_Click);
            // 
            // bothToolStripMenuItem
            // 
            this.bothToolStripMenuItem.Name = "bothToolStripMenuItem";
            this.bothToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.bothToolStripMenuItem.Text = "Both of the above";
            this.bothToolStripMenuItem.Click += new System.EventHandler(this.bothToolStripMenuItem_Click);
            // 
            // routeToolStripMenuItem
            // 
            this.routeToolStripMenuItem.Name = "routeToolStripMenuItem";
            this.routeToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.routeToolStripMenuItem.Text = "Expedition Panel";
            this.routeToolStripMenuItem.Click += new System.EventHandler(this.routeToolStripMenuItem_Click);
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
            // selectCorrectSystemToolStripMenuItem
            // 
            this.selectCorrectSystemToolStripMenuItem.Name = "selectCorrectSystemToolStripMenuItem";
            this.selectCorrectSystemToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.selectCorrectSystemToolStripMenuItem.Text = "Assign new system";
            this.selectCorrectSystemToolStripMenuItem.Click += new System.EventHandler(this.selectCorrectSystemToolStripMenuItem_Click);
            // 
            // removeJournalEntryToolStripMenuItem
            // 
            this.removeJournalEntryToolStripMenuItem.Name = "removeJournalEntryToolStripMenuItem";
            this.removeJournalEntryToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.removeJournalEntryToolStripMenuItem.Text = "Remove Journal Entry";
            this.removeJournalEntryToolStripMenuItem.Click += new System.EventHandler(this.removeJournalEntryToolStripMenuItem_Click);
            // 
            // sendUnsyncedScanToEDDNToolStripMenuItem
            // 
            this.sendUnsyncedScanToEDDNToolStripMenuItem.Name = "sendUnsyncedScanToEDDNToolStripMenuItem";
            this.sendUnsyncedScanToEDDNToolStripMenuItem.Size = new System.Drawing.Size(387, 22);
            this.sendUnsyncedScanToEDDNToolStripMenuItem.Text = "Send unsynced scan to EDDN";
            this.sendUnsyncedScanToEDDNToolStripMenuItem.Click += new System.EventHandler(this.sendUnsyncedScanToEDDNToolStripMenuItem_Click);
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
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewTravel);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 32);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(870, 578);
            this.dataViewScrollerPanel1.TabIndex = 28;
            this.dataViewScrollerPanel1.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = true;
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(847, 21);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 557);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 4;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // dataGridViewTravel
            // 
            this.dataGridViewTravel.AllowUserToAddRows = false;
            this.dataGridViewTravel.AllowUserToDeleteRows = false;
            this.dataGridViewTravel.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTravel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTravel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.Icon,
            this.ColumnSystem,
            this.ColumnDistance,
            this.ColumnNote});
            this.dataGridViewTravel.ContextMenuStrip = this.historyContextMenu;
            this.dataGridViewTravel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTravel.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTravel.Name = "dataGridViewTravel";
            this.dataGridViewTravel.RowHeadersWidth = 100;
            this.dataGridViewTravel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewTravel.Size = new System.Drawing.Size(847, 578);
            this.dataGridViewTravel.TabIndex = 3;
            this.dataGridViewTravel.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellClick);
            this.dataGridViewTravel.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellDoubleClick);
            this.dataGridViewTravel.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewTravel_RowPostPaint);
            this.dataGridViewTravel.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewTravel_SortCompare);
            this.dataGridViewTravel.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewTravel_KeyDown);
            this.dataGridViewTravel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.dataGridViewTravel_KeyPress);
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
            // Icon
            // 
            this.Icon.FillWeight = 50F;
            this.Icon.HeaderText = "Event";
            this.Icon.MinimumWidth = 50;
            this.Icon.Name = "Icon";
            this.Icon.ReadOnly = true;
            this.Icon.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnSystem
            // 
            this.ColumnSystem.HeaderText = "Description";
            this.ColumnSystem.MinimumWidth = 50;
            this.ColumnSystem.Name = "ColumnSystem";
            this.ColumnSystem.ReadOnly = true;
            // 
            // ColumnDistance
            // 
            this.ColumnDistance.FillWeight = 200F;
            this.ColumnDistance.HeaderText = "Information";
            this.ColumnDistance.MinimumWidth = 50;
            this.ColumnDistance.Name = "ColumnDistance";
            this.ColumnDistance.ReadOnly = true;
            // 
            // ColumnNote
            // 
            this.ColumnNote.HeaderText = "Note";
            this.ColumnNote.MinimumWidth = 20;
            this.ColumnNote.Name = "ColumnNote";
            this.ColumnNote.ReadOnly = true;
            // 
            // UserControlTravelGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.TopPanel);
            this.Name = "UserControlTravelGrid";
            this.Size = new System.Drawing.Size(870, 610);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.historyContextMenu.ResumeLayout(false);
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TopPanel;
        private ExtendedControls.ButtonExt buttonFilter;
        private ExtendedControls.TextBoxBorder textBoxFilter;
        private System.Windows.Forms.Label labelSearch;
        internal ExtendedControls.ComboBoxCustom comboBoxHistoryWindow;
        private System.Windows.Forms.Label labelTime;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel1;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        public System.Windows.Forms.DataGridView dataGridViewTravel;
        private System.Windows.Forms.ContextMenuStrip historyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem starMapColourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToAnotherCommanderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToTrilaterationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trilaterationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wantedSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bothToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem routeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCorrectSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStartStop;
        private System.Windows.Forms.ToolStripMenuItem removeJournalEntryToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ButtonExt buttonField;
        private System.Windows.Forms.ToolStripMenuItem sendUnsyncedScanToEDDNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runActionsOnThisEntryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setNoteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem writeEventInfoToLogDebugToolStripMenuItem;
        private ExtendedControls.ButtonExt buttonExtExcel;
        private System.Windows.Forms.ToolStripMenuItem copyJournalEntryToClipboardToolStripMenuItem;
        private ExtendedControls.CheckBoxCustom checkBoxMoveToTop;
        private System.Windows.Forms.ToolStripMenuItem createEditBookmarkToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runActionsAcrossSelectionToolSpeechStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNote;
        private System.Windows.Forms.ToolStripMenuItem removeSortingOfColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runSelectionThroughInaraSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoEntryNumberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoNextStartStopMarkerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runEntryThroughProfileSystemToolStripMenuItem;
    }
}
