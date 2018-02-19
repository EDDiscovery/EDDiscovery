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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlStarList));
            this.TopPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBoxMoveToTop = new ExtendedControls.CheckBoxCustom();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExtExcel = new ExtendedControls.ButtonExt();
            this.buttonJumponium = new ExtendedControls.ButtonExt();
            this.buttonBodyClasses = new ExtendedControls.ButtonExt();
            this.checkBoxEDSM = new ExtendedControls.CheckBoxCustom();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxFilter = new ExtendedControls.TextBoxBorder();
            this.labelSearch = new System.Windows.Forms.Label();
            this.comboBoxHistoryWindow = new ExtendedControls.ComboBoxCustom();
            this.labelTime = new System.Windows.Forms.Label();
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
            this.TopPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.historyContextMenu.SuspendLayout();
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStarList)).BeginInit();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.panel3);
            this.TopPanel.Controls.Add(this.panel1);
            this.TopPanel.Controls.Add(this.panel2);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(870, 32);
            this.TopPanel.TabIndex = 27;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBoxMoveToTop);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(770, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(100, 32);
            this.panel3.TabIndex = 7;
            // 
            // checkBoxMoveToTop
            // 
            this.checkBoxMoveToTop.AutoSize = true;
            this.checkBoxMoveToTop.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxMoveToTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMoveToTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMoveToTop.Dock = System.Windows.Forms.DockStyle.Right;
            this.checkBoxMoveToTop.FontNerfReduction = 0.5F;
            this.checkBoxMoveToTop.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxMoveToTop.Location = new System.Drawing.Point(4, 0);
            this.checkBoxMoveToTop.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMoveToTop.Name = "checkBoxMoveToTop";
            this.checkBoxMoveToTop.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.checkBoxMoveToTop.Size = new System.Drawing.Size(96, 32);
            this.checkBoxMoveToTop.TabIndex = 29;
            this.checkBoxMoveToTop.Text = "Cursor to Top";
            this.checkBoxMoveToTop.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxMoveToTop, "Select if cursor moves to top entry when a new entry is received");
            this.checkBoxMoveToTop.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonExtExcel);
            this.panel1.Controls.Add(this.buttonJumponium);
            this.panel1.Controls.Add(this.buttonBodyClasses);
            this.panel1.Controls.Add(this.checkBoxEDSM);
            this.panel1.Location = new System.Drawing.Point(344, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(153, 32);
            this.panel1.TabIndex = 5;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonExtExcel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.StarList_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(96, 0);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(32, 32);
            this.buttonExtExcel.TabIndex = 28;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // buttonJumponium
            // 
            this.buttonJumponium.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonJumponium.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonJumponium.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonJumponium.Image = ((System.Drawing.Image)(resources.GetObject("buttonJumponium.Image")));
            this.buttonJumponium.Location = new System.Drawing.Point(64, 0);
            this.buttonJumponium.Name = "buttonJumponium";
            this.buttonJumponium.Size = new System.Drawing.Size(32, 32);
            this.buttonJumponium.TabIndex = 36;
            this.buttonJumponium.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip.SetToolTip(this.buttonJumponium, "Show/Hide presence of Jumponium Materials");
            this.buttonJumponium.UseVisualStyleBackColor = true;
            this.buttonJumponium.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonExt1_MouseClick);
            // 
            // buttonBodyClasses
            // 
            this.buttonBodyClasses.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonBodyClasses.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.buttonBodyClasses.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonBodyClasses.Image = ((System.Drawing.Image)(resources.GetObject("buttonBodyClasses.Image")));
            this.buttonBodyClasses.Location = new System.Drawing.Point(32, 0);
            this.buttonBodyClasses.Name = "buttonBodyClasses";
            this.buttonBodyClasses.Size = new System.Drawing.Size(32, 32);
            this.buttonBodyClasses.TabIndex = 37;
            this.buttonBodyClasses.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.toolTip.SetToolTip(this.buttonBodyClasses, "Show/Hide Special bodies classes");
            this.buttonBodyClasses.UseVisualStyleBackColor = true;
            this.buttonBodyClasses.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonBodyClasses_MouseClick);
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
            this.checkBoxEDSM.Dock = System.Windows.Forms.DockStyle.Left;
            this.checkBoxEDSM.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxEDSM.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxEDSM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxEDSM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEDSM.FontNerfReduction = 0.5F;
            this.checkBoxEDSM.Image = global::EDDiscovery.Icons.Controls.StarList_EDSM;
            this.checkBoxEDSM.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxEDSM.Location = new System.Drawing.Point(0, 0);
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
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxFilter);
            this.panel2.Controls.Add(this.labelSearch);
            this.panel2.Controls.Add(this.comboBoxHistoryWindow);
            this.panel2.Controls.Add(this.labelTime);
            this.panel2.Location = new System.Drawing.Point(3, 6);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(338, 20);
            this.panel2.TabIndex = 6;
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFilter.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxFilter.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFilter.BorderColorScaling = 0.5F;
            this.textBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFilter.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFilter.Dock = System.Windows.Forms.DockStyle.Left;
            this.textBoxFilter.InErrorCondition = false;
            this.textBoxFilter.Location = new System.Drawing.Point(177, 0);
            this.textBoxFilter.Multiline = false;
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Padding = new System.Windows.Forms.Padding(0, 3, 6, 0);
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
            this.labelSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelSearch.Location = new System.Drawing.Point(130, 0);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Padding = new System.Windows.Forms.Padding(6, 3, 0, 0);
            this.labelSearch.Size = new System.Drawing.Size(47, 16);
            this.labelSearch.TabIndex = 24;
            this.labelSearch.Text = "Search";
            // 
            // comboBoxHistoryWindow
            // 
            this.comboBoxHistoryWindow.ArrowWidth = 1;
            this.comboBoxHistoryWindow.BorderColor = System.Drawing.Color.Red;
            this.comboBoxHistoryWindow.ButtonColorScaling = 0.5F;
            this.comboBoxHistoryWindow.DataSource = null;
            this.comboBoxHistoryWindow.DisplayMember = "";
            this.comboBoxHistoryWindow.Dock = System.Windows.Forms.DockStyle.Left;
            this.comboBoxHistoryWindow.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxHistoryWindow.DropDownHeight = 200;
            this.comboBoxHistoryWindow.DropDownWidth = 1;
            this.comboBoxHistoryWindow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxHistoryWindow.ItemHeight = 13;
            this.comboBoxHistoryWindow.Location = new System.Drawing.Point(30, 0);
            this.comboBoxHistoryWindow.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxHistoryWindow.Name = "comboBoxHistoryWindow";
            this.comboBoxHistoryWindow.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
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
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelTime.Location = new System.Drawing.Point(0, 0);
            this.labelTime.Name = "labelTime";
            this.labelTime.Padding = new System.Windows.Forms.Padding(0, 3, 0, 0);
            this.labelTime.Size = new System.Drawing.Size(30, 16);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "Time";
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
            this.dataGridViewStarList.Size = new System.Drawing.Size(847, 578);
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
            // UserControlStarList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.TopPanel);
            this.Name = "UserControlStarList";
            this.Size = new System.Drawing.Size(870, 610);
            this.TopPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.historyContextMenu.ResumeLayout(false);
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStarList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TopPanel;
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
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ButtonExt buttonExtExcel;
        private ExtendedControls.CheckBoxCustom checkBoxMoveToTop;
        private System.Windows.Forms.ToolStripMenuItem setNoteToolStripMenuItem;
        private ExtendedControls.CheckBoxCustom checkBoxEDSM;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private ExtendedControls.ButtonExt buttonJumponium;
        private ExtendedControls.ButtonExt buttonBodyClasses;
    }
}
