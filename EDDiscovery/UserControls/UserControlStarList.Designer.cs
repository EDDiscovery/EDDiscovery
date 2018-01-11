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
    partial class UserControlStarList
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
            this.historyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setNoteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel1 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewStarList = new System.Windows.Forms.DataGridView();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Icon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rollUpPanel1 = new ExtendedControls.RollUpPanel();
            this.checkBoxEDSM = new ExtendedControls.CheckBoxCustom();
            this.buttonExtExcel = new ExtendedControls.ButtonExt();
            this.checkBoxMoveToTop = new ExtendedControls.CheckBoxCustom();
            this.panelHistoryIcon = new System.Windows.Forms.Panel();
            this.labelTime = new System.Windows.Forms.Label();
            this.textBoxFilter = new ExtendedControls.TextBoxBorder();
            this.comboBoxHistoryWindow = new ExtendedControls.ComboBoxCustom();
            this.labelSearch = new System.Windows.Forms.Label();
            this.historyContextMenu.SuspendLayout();
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStarList)).BeginInit();
            this.rollUpPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // historyContextMenu
            // 
            this.historyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapGotoStartoolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.setNoteToolStripMenuItem});
            this.historyContextMenu.Name = "historyContextMenu";
            this.historyContextMenu.Size = new System.Drawing.Size(187, 70);
            this.historyContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.historyContextMenu_Opening);
            // 
            // mapGotoStartoolStripMenuItem
            // 
            this.mapGotoStartoolStripMenuItem.Name = "mapGotoStartoolStripMenuItem";
            this.mapGotoStartoolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.mapGotoStartoolStripMenuItem.Text = "Go to star on 3D Map";
            this.mapGotoStartoolStripMenuItem.Click += new System.EventHandler(this.mapGotoStartoolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // setNoteToolStripMenuItem
            // 
            this.setNoteToolStripMenuItem.Name = "setNoteToolStripMenuItem";
            this.setNoteToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
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
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewStarList);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 36);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(870, 574);
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
            this.vScrollBarCustom1.Location = new System.Drawing.Point(847, 23);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 551);
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
            // dataGridViewStarList
            // 
            this.dataGridViewStarList.AllowUserToAddRows = false;
            this.dataGridViewStarList.AllowUserToDeleteRows = false;
            this.dataGridViewStarList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewStarList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewStarList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStarList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.Icon,
            this.ColumnSystem,
            this.ColumnDistance});
            this.dataGridViewStarList.ContextMenuStrip = this.historyContextMenu;
            this.dataGridViewStarList.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewStarList.Name = "dataGridViewStarList";
            this.dataGridViewStarList.RowHeadersWidth = 50;
            this.dataGridViewStarList.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewStarList.Size = new System.Drawing.Size(847, 574);
            this.dataGridViewStarList.TabIndex = 3;
            this.dataGridViewStarList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellClick);
            this.dataGridViewStarList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellDoubleClick);
            this.dataGridViewStarList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTravel_ColumnHeaderMouseClick);
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
            // Icon
            // 
            this.Icon.HeaderText = "System";
            this.Icon.MinimumWidth = 50;
            this.Icon.Name = "Icon";
            this.Icon.ReadOnly = true;
            // 
            // ColumnSystem
            // 
            this.ColumnSystem.FillWeight = 40F;
            this.ColumnSystem.HeaderText = "Visits";
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
            // rollUpPanel1
            // 
            this.rollUpPanel1.Controls.Add(this.checkBoxEDSM);
            this.rollUpPanel1.Controls.Add(this.buttonExtExcel);
            this.rollUpPanel1.Controls.Add(this.checkBoxMoveToTop);
            this.rollUpPanel1.Controls.Add(this.panelHistoryIcon);
            this.rollUpPanel1.Controls.Add(this.labelTime);
            this.rollUpPanel1.Controls.Add(this.textBoxFilter);
            this.rollUpPanel1.Controls.Add(this.comboBoxHistoryWindow);
            this.rollUpPanel1.Controls.Add(this.labelSearch);
            this.rollUpPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.rollUpPanel1.HiddenMarkerWidth = 0;
            this.rollUpPanel1.Location = new System.Drawing.Point(0, 0);
            this.rollUpPanel1.Name = "rollUpPanel1";
            this.rollUpPanel1.PinState = true;
            this.rollUpPanel1.RolledUpHeight = 5;
            this.rollUpPanel1.RollUpAnimationTime = 250;
            this.rollUpPanel1.RollUpDelay = 500;
            this.rollUpPanel1.ShowHiddenMarker = true;
            this.rollUpPanel1.Size = new System.Drawing.Size(870, 36);
            this.rollUpPanel1.TabIndex = 31;
            this.rollUpPanel1.UnrolledHeight = 32;
            this.rollUpPanel1.UnrollHoverDelay = 500;
            // 
            // checkBoxEDSM
            // 
            this.checkBoxEDSM.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEDSM.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxEDSM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxEDSM.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEDSM.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxEDSM.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxEDSM.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxEDSM.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxEDSM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxEDSM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEDSM.FontNerfReduction = 0.5F;
            this.checkBoxEDSM.Image = global::EDDiscovery.Properties.Resources.edsm24;
            this.checkBoxEDSM.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxEDSM.Location = new System.Drawing.Point(432, 2);
            this.checkBoxEDSM.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEDSM.Name = "checkBoxEDSM";
            this.checkBoxEDSM.Size = new System.Drawing.Size(32, 32);
            this.checkBoxEDSM.TabIndex = 30;
            this.checkBoxEDSM.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxEDSM, "Show/Hide Body data from EDSM. Due to server constraints, you must click on a sys" +
        "tem to retreive data on it from EDSM.");
            this.checkBoxEDSM.UseVisualStyleBackColor = false;
            this.checkBoxEDSM.CheckedChanged += new System.EventHandler(this.checkBoxEDSM_CheckedChanged);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Properties.Resources.excel;
            this.buttonExtExcel.Location = new System.Drawing.Point(470, 5);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(24, 24);
            this.buttonExtExcel.TabIndex = 28;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // checkBoxMoveToTop
            // 
            this.checkBoxMoveToTop.AutoSize = true;
            this.checkBoxMoveToTop.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxMoveToTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMoveToTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMoveToTop.FontNerfReduction = 0.5F;
            this.checkBoxMoveToTop.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxMoveToTop.Location = new System.Drawing.Point(500, 10);
            this.checkBoxMoveToTop.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMoveToTop.Name = "checkBoxMoveToTop";
            this.checkBoxMoveToTop.Size = new System.Drawing.Size(90, 17);
            this.checkBoxMoveToTop.TabIndex = 29;
            this.checkBoxMoveToTop.Text = "Cursor to Top";
            this.checkBoxMoveToTop.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMoveToTop, "Select if cursor moves to top entry when a new entry is received");
            this.checkBoxMoveToTop.UseVisualStyleBackColor = true;
            // 
            // panelHistoryIcon
            // 
            this.panelHistoryIcon.BackgroundImage = global::EDDiscovery.Properties.Resources.starlist;
            this.panelHistoryIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelHistoryIcon.Location = new System.Drawing.Point(3, 3);
            this.panelHistoryIcon.Name = "panelHistoryIcon";
            this.panelHistoryIcon.Size = new System.Drawing.Size(24, 24);
            this.panelHistoryIcon.TabIndex = 26;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(64, 10);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(30, 13);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "Time";
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFilter.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFilter.BorderColorScaling = 0.5F;
            this.textBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFilter.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFilter.Location = new System.Drawing.Point(278, 7);
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
            // comboBoxHistoryWindow
            // 
            this.comboBoxHistoryWindow.ArrowWidth = 1;
            this.comboBoxHistoryWindow.BorderColor = System.Drawing.Color.Red;
            this.comboBoxHistoryWindow.ButtonColorScaling = 0.5F;
            this.comboBoxHistoryWindow.DataSource = null;
            this.comboBoxHistoryWindow.DisplayMember = "";
            this.comboBoxHistoryWindow.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxHistoryWindow.DropDownHeight = 200;
            this.comboBoxHistoryWindow.DropDownWidth = 1;
            this.comboBoxHistoryWindow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxHistoryWindow.ItemHeight = 13;
            this.comboBoxHistoryWindow.Location = new System.Drawing.Point(110, 7);
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
            this.toolTip.SetToolTip(this.comboBoxHistoryWindow, "Select the entries by age");
            this.comboBoxHistoryWindow.ValueMember = "";
            this.comboBoxHistoryWindow.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(220, 8);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 24;
            this.labelSearch.Text = "Search";
            // 
            // UserControlStarList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.rollUpPanel1);
            this.Name = "UserControlStarList";
            this.Size = new System.Drawing.Size(870, 610);
            this.historyContextMenu.ResumeLayout(false);
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStarList)).EndInit();
            this.rollUpPanel1.ResumeLayout(false);
            this.rollUpPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.TextBoxBorder textBoxFilter;
        private System.Windows.Forms.Label labelSearch;
        internal ExtendedControls.ComboBoxCustom comboBoxHistoryWindow;
        private System.Windows.Forms.Label labelTime;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel1;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        public System.Windows.Forms.DataGridView dataGridViewStarList;
        private System.Windows.Forms.ContextMenuStrip historyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.Panel panelHistoryIcon;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ButtonExt buttonExtExcel;
        private ExtendedControls.CheckBoxCustom checkBoxMoveToTop;
        private System.Windows.Forms.ToolStripMenuItem setNoteToolStripMenuItem;
        private ExtendedControls.CheckBoxCustom checkBoxEDSM;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private ExtendedControls.RollUpPanel rollUpPanel1;
    }
}
