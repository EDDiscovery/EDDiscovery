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
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridViewJournal = new System.Windows.Forms.DataGridView();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.historyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeSortingOfColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jumpToEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.sendUnsyncedScanToEDDNToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runActionsOnThisEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyJournalEntryToClipboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop = new System.Windows.Forms.Panel();
            this.checkBoxCursorToTop = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.buttonField = new ExtendedControls.ExtButton();
            this.textBoxFilter = new ExtendedControls.ExtTextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.comboBoxJournalWindow = new ExtendedControls.ExtComboBox();
            this.labelTime = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJournal)).BeginInit();
            this.historyContextMenu.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewJournal);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 32);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 20;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(804, 684);
            this.dataViewScrollerPanel.TabIndex = 7;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = true;
            this.vScrollBarCustom.LargeChange = 0;
            this.vScrollBarCustom.Location = new System.Drawing.Point(784, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(20, 684);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 7;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -1;
            this.vScrollBarCustom.ValueLimited = -1;
            // 
            // dataGridViewJournal
            // 
            this.dataGridViewJournal.AllowUserToAddRows = false;
            this.dataGridViewJournal.AllowUserToDeleteRows = false;
            this.dataGridViewJournal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewJournal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.Event,
            this.ColumnType,
            this.ColumnText});
            this.dataGridViewJournal.ContextMenuStrip = this.historyContextMenu;
            this.dataGridViewJournal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewJournal.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewJournal.Name = "dataGridViewJournal";
            this.dataGridViewJournal.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewJournal.Size = new System.Drawing.Size(784, 684);
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
            // Event
            // 
            this.Event.FillWeight = 50F;
            this.Event.HeaderText = "Event";
            this.Event.MinimumWidth = 50;
            this.Event.Name = "Event";
            this.Event.ReadOnly = true;
            this.Event.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ColumnType
            // 
            this.ColumnType.FillWeight = 125F;
            this.ColumnType.HeaderText = "Description";
            this.ColumnType.MinimumWidth = 50;
            this.ColumnType.Name = "ColumnType";
            this.ColumnType.ReadOnly = true;
            // 
            // ColumnText
            // 
            this.ColumnText.FillWeight = 300F;
            this.ColumnText.HeaderText = "Information";
            this.ColumnText.MinimumWidth = 50;
            this.ColumnText.Name = "ColumnText";
            this.ColumnText.ReadOnly = true;
            // 
            // historyContextMenu
            // 
            this.historyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeSortingOfColumnsToolStripMenuItem,
            this.jumpToEntryToolStripMenuItem,
            this.mapGotoStartoolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.toolStripMenuItemStartStop,
            this.sendUnsyncedScanToEDDNToolStripMenuItem,
            this.runActionsOnThisEntryToolStripMenuItem,
            this.copyJournalEntryToClipboardToolStripMenuItem});
            this.historyContextMenu.Name = "historyContextMenu";
            this.historyContextMenu.Size = new System.Drawing.Size(294, 180);
            this.historyContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.historyContextMenu_Opening);
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
            // sendUnsyncedScanToEDDNToolStripMenuItem
            // 
            this.sendUnsyncedScanToEDDNToolStripMenuItem.Name = "sendUnsyncedScanToEDDNToolStripMenuItem";
            this.sendUnsyncedScanToEDDNToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.sendUnsyncedScanToEDDNToolStripMenuItem.Text = "Send unsynced scan to EDDN";
            this.sendUnsyncedScanToEDDNToolStripMenuItem.Click += new System.EventHandler(this.sendUnsyncedScanToEDDNToolStripMenuItem_Click);
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
            // panelTop
            // 
            this.panelTop.Controls.Add(this.checkBoxCursorToTop);
            this.panelTop.Controls.Add(this.buttonExtExcel);
            this.panelTop.Controls.Add(this.buttonField);
            this.panelTop.Controls.Add(this.textBoxFilter);
            this.panelTop.Controls.Add(this.labelSearch);
            this.panelTop.Controls.Add(this.buttonFilter);
            this.panelTop.Controls.Add(this.comboBoxJournalWindow);
            this.panelTop.Controls.Add(this.labelTime);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(804, 32);
            this.panelTop.TabIndex = 8;
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
            this.checkBoxCursorToTop.FontNerfReduction = 0.5F;
            this.checkBoxCursorToTop.Image = global::EDDiscovery.Icons.Controls.TravelGrid_CursorToTop;
            this.checkBoxCursorToTop.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCursorToTop.Location = new System.Drawing.Point(450, 1);
            this.checkBoxCursorToTop.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCursorToTop.Name = "checkBoxCursorToTop";
            this.checkBoxCursorToTop.Size = new System.Drawing.Size(28, 28);
            this.checkBoxCursorToTop.TabIndex = 30;
            this.checkBoxCursorToTop.TickBoxReductionSize = 10;
            this.toolTip1.SetToolTip(this.checkBoxCursorToTop, "Automatically move the cursor to the latest entry when it arrives");
            this.checkBoxCursorToTop.UseVisualStyleBackColor = false;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.JournalGrid_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(485, 1);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 30;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // buttonField
            // 
            this.buttonField.Image = global::EDDiscovery.Icons.Controls.TravelGrid_FieldFilter;
            this.buttonField.Location = new System.Drawing.Point(415, 1);
            this.buttonField.Name = "buttonField";
            this.buttonField.Size = new System.Drawing.Size(28, 28);
            this.buttonField.TabIndex = 25;
            this.buttonField.UseVisualStyleBackColor = true;
            this.buttonField.Click += new System.EventHandler(this.buttonField_Click);
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
            // buttonFilter
            // 
            this.buttonFilter.Image = global::EDDiscovery.Icons.Controls.TravelGrid_EventFilter;
            this.buttonFilter.Location = new System.Drawing.Point(380, 1);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(28, 28);
            this.buttonFilter.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonFilter, "Filter out entries based on event type");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // comboBoxJournalWindow
            // 
            this.comboBoxJournalWindow.ArrowWidth = 1;
            this.comboBoxJournalWindow.BorderColor = System.Drawing.Color.Red;
            this.comboBoxJournalWindow.ButtonColorScaling = 0.5F;
            this.comboBoxJournalWindow.DataSource = null;
            this.comboBoxJournalWindow.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxJournalWindow.DisplayMember = "";
            this.comboBoxJournalWindow.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxJournalWindow.DropDownHeight = 200;
            this.comboBoxJournalWindow.DropDownWidth = 1;
            this.comboBoxJournalWindow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxJournalWindow.ItemHeight = 13;
            this.comboBoxJournalWindow.Location = new System.Drawing.Point(49, 4);
            this.comboBoxJournalWindow.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxJournalWindow.Name = "comboBoxJournalWindow";
            this.comboBoxJournalWindow.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxJournalWindow.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxJournalWindow.ScrollBarWidth = 16;
            this.comboBoxJournalWindow.SelectedIndex = -1;
            this.comboBoxJournalWindow.SelectedItem = null;
            this.comboBoxJournalWindow.SelectedValue = null;
            this.comboBoxJournalWindow.Size = new System.Drawing.Size(100, 21);
            this.comboBoxJournalWindow.TabIndex = 0;
            this.comboBoxJournalWindow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxJournalWindow, "Select the entries selected by age");
            this.comboBoxJournalWindow.ValueMember = "";
            this.comboBoxJournalWindow.SelectedIndexChanged += new System.EventHandler(this.comboBoxJournalWindow_SelectedIndexChanged);
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
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 120000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 30000;
            this.toolTip1.InitialDelay = 250;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ShowAlways = true;
            // 
            // UserControlJournalGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlJournalGrid";
            this.Size = new System.Drawing.Size(804, 716);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJournal)).EndInit();
            this.historyContextMenu.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewJournal;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtTextBox textBoxFilter;
        private System.Windows.Forms.Label labelSearch;
        private ExtendedControls.ExtButton buttonFilter;
        private ExtendedControls.ExtComboBox comboBoxJournalWindow;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.ContextMenuStrip historyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStartStop;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonField;
        private System.Windows.Forms.ToolStripMenuItem sendUnsyncedScanToEDDNToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runActionsOnThisEntryToolStripMenuItem;
        private ExtendedControls.ExtButton buttonExtExcel;
        private System.Windows.Forms.ToolStripMenuItem copyJournalEntryToClipboardToolStripMenuItem;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Event;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnText;
        private System.Windows.Forms.ToolStripMenuItem removeSortingOfColumnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jumpToEntryToolStripMenuItem;
        private ExtendedControls.ExtCheckBox checkBoxCursorToTop;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
