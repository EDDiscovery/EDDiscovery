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
    partial class UserControlExpedition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlExpedition));
            this.labelCml = new System.Windows.Forms.Label();
            this.labelP2P = new System.Windows.Forms.Label();
            this.txtP2PDIstance = new ExtendedControls.ExtTextBox();
            this.txtCmlDistance = new ExtendedControls.ExtTextBox();
            this.buttonReverseRoute = new ExtendedControls.ExtButton();
            this.dateTimePickerEndTime = new ExtendedControls.ExtDateTimePicker();
            this.dateTimePickerEndDate = new ExtendedControls.ExtDateTimePicker();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerStartTime = new ExtendedControls.ExtDateTimePicker();
            this.dateTimePickerStartDate = new ExtendedControls.ExtDateTimePicker();
            this.labelDateStart = new System.Windows.Forms.Label();
            this.textBoxRouteName = new ExtendedControls.ExtTextBox();
            this.labelRouteName = new System.Windows.Forms.Label();
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.contextMenuCopyPaste = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertCopiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.extPanelDataGridViewScroll = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarDGV = new ExtendedControls.ExtScrollBar();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonLoadRoute = new ExtendedControls.ExtButton();
            this.extButtonNew = new ExtendedControls.ExtButton();
            this.extButtonSave = new ExtendedControls.ExtButton();
            this.extButtonDelete = new ExtendedControls.ExtButton();
            this.extButtonImportFile = new ExtendedControls.ExtButton();
            this.extButtonImportRoute = new ExtendedControls.ExtButton();
            this.extButtonImportNavRoute = new ExtendedControls.ExtButton();
            this.extButtonNavRouteLatest = new ExtendedControls.ExtButton();
            this.extButtonAddSystems = new ExtendedControls.ExtButton();
            this.buttonExtExport = new ExtendedControls.ExtButton();
            this.extButtonShow3DMap = new ExtendedControls.ExtButton();
            this.extButtonDisplayFilters = new ExtendedControls.ExtButton();
            this.checkBoxEDSM = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SystemName = new ExtendedControls.ExtDataGridViewColumnAutoComplete();
            this.Distance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistanceRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CurDist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Visits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Scans = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FSSBodies = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KnownBodies = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stars = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Info = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.contextMenuCopyPaste.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.extPanelDataGridViewScroll.SuspendLayout();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelCml
            // 
            this.labelCml.AutoSize = true;
            this.labelCml.Location = new System.Drawing.Point(444, 53);
            this.labelCml.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.labelCml.Name = "labelCml";
            this.labelCml.Size = new System.Drawing.Size(72, 13);
            this.labelCml.TabIndex = 12;
            this.labelCml.Text = "Cml Distance:";
            // 
            // labelP2P
            // 
            this.labelP2P.AutoSize = true;
            this.labelP2P.Location = new System.Drawing.Point(444, 29);
            this.labelP2P.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.labelP2P.Name = "labelP2P";
            this.labelP2P.Size = new System.Drawing.Size(75, 13);
            this.labelP2P.TabIndex = 11;
            this.labelP2P.Text = "P2P Distance:";
            // 
            // txtP2PDIstance
            // 
            this.txtP2PDIstance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtP2PDIstance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtP2PDIstance.BackErrorColor = System.Drawing.Color.Red;
            this.txtP2PDIstance.BorderColor = System.Drawing.Color.Transparent;
            this.txtP2PDIstance.BorderColorScaling = 0.5F;
            this.txtP2PDIstance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtP2PDIstance.ClearOnFirstChar = false;
            this.txtP2PDIstance.ControlBackground = System.Drawing.SystemColors.Control;
            this.txtP2PDIstance.EndButtonEnable = true;
            this.txtP2PDIstance.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("txtP2PDIstance.EndButtonImage")));
            this.txtP2PDIstance.EndButtonVisible = false;
            this.txtP2PDIstance.InErrorCondition = false;
            this.txtP2PDIstance.Location = new System.Drawing.Point(529, 29);
            this.txtP2PDIstance.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.txtP2PDIstance.Multiline = false;
            this.txtP2PDIstance.Name = "txtP2PDIstance";
            this.txtP2PDIstance.ReadOnly = true;
            this.txtP2PDIstance.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtP2PDIstance.SelectionLength = 0;
            this.txtP2PDIstance.SelectionStart = 0;
            this.txtP2PDIstance.Size = new System.Drawing.Size(100, 20);
            this.txtP2PDIstance.TabIndex = 10;
            this.txtP2PDIstance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtP2PDIstance.WordWrap = true;
            // 
            // txtCmlDistance
            // 
            this.txtCmlDistance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.txtCmlDistance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.txtCmlDistance.BackErrorColor = System.Drawing.Color.Red;
            this.txtCmlDistance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCmlDistance.BorderColorScaling = 0.5F;
            this.txtCmlDistance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCmlDistance.ClearOnFirstChar = false;
            this.txtCmlDistance.ControlBackground = System.Drawing.SystemColors.Control;
            this.txtCmlDistance.EndButtonEnable = true;
            this.txtCmlDistance.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("txtCmlDistance.EndButtonImage")));
            this.txtCmlDistance.EndButtonVisible = false;
            this.txtCmlDistance.InErrorCondition = false;
            this.txtCmlDistance.Location = new System.Drawing.Point(529, 53);
            this.txtCmlDistance.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.txtCmlDistance.Multiline = false;
            this.txtCmlDistance.Name = "txtCmlDistance";
            this.txtCmlDistance.ReadOnly = true;
            this.txtCmlDistance.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtCmlDistance.SelectionLength = 0;
            this.txtCmlDistance.SelectionStart = 0;
            this.txtCmlDistance.Size = new System.Drawing.Size(100, 20);
            this.txtCmlDistance.TabIndex = 9;
            this.txtCmlDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtCmlDistance.WordWrap = true;
            // 
            // buttonReverseRoute
            // 
            this.buttonReverseRoute.Image = global::EDDiscovery.Icons.Controls.Reverse;
            this.buttonReverseRoute.Location = new System.Drawing.Point(208, 1);
            this.buttonReverseRoute.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonReverseRoute.Name = "buttonReverseRoute";
            this.buttonReverseRoute.Size = new System.Drawing.Size(28, 28);
            this.buttonReverseRoute.TabIndex = 8;
            this.toolTip.SetToolTip(this.buttonReverseRoute, "Reverse");
            this.buttonReverseRoute.UseVisualStyleBackColor = true;
            this.buttonReverseRoute.Click += new System.EventHandler(this.buttonReverseRoute_Click);
            // 
            // dateTimePickerEndTime
            // 
            this.dateTimePickerEndTime.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerEndTime.BorderColorScaling = 0.5F;
            this.dateTimePickerEndTime.Checked = false;
            this.dateTimePickerEndTime.CustomFormat = "HH:mm:ss";
            this.dateTimePickerEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerEndTime.Location = new System.Drawing.Point(299, 53);
            this.dateTimePickerEndTime.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.dateTimePickerEndTime.Name = "dateTimePickerEndTime";
            this.dateTimePickerEndTime.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerEndTime.ShowCheckBox = true;
            this.dateTimePickerEndTime.ShowUpDown = true;
            this.dateTimePickerEndTime.Size = new System.Drawing.Size(135, 20);
            this.dateTimePickerEndTime.TabIndex = 7;
            this.dateTimePickerEndTime.TextBackColor = System.Drawing.Color.AliceBlue;
            this.dateTimePickerEndTime.Value = new System.DateTime(2017, 8, 30, 10, 50, 42, 845);
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerEndDate.BorderColorScaling = 0.5F;
            this.dateTimePickerEndDate.Checked = false;
            this.dateTimePickerEndDate.CustomFormat = "dd MMMM yyyy";
            this.dateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(78, 53);
            this.dateTimePickerEndDate.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerEndDate.ShowCheckBox = true;
            this.dateTimePickerEndDate.ShowUpDown = false;
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(211, 20);
            this.dateTimePickerEndDate.TabIndex = 6;
            this.dateTimePickerEndDate.TextBackColor = System.Drawing.Color.AliceBlue;
            this.dateTimePickerEndDate.Value = new System.DateTime(2017, 8, 30, 10, 50, 42, 853);
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.Location = new System.Drawing.Point(3, 51);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(55, 13);
            this.labelEndDate.TabIndex = 5;
            this.labelEndDate.Text = "End Date:";
            // 
            // dateTimePickerStartTime
            // 
            this.dateTimePickerStartTime.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerStartTime.BorderColorScaling = 0.5F;
            this.dateTimePickerStartTime.Checked = false;
            this.dateTimePickerStartTime.CustomFormat = "HH:mm:ss";
            this.dateTimePickerStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerStartTime.Location = new System.Drawing.Point(299, 29);
            this.dateTimePickerStartTime.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.dateTimePickerStartTime.Name = "dateTimePickerStartTime";
            this.dateTimePickerStartTime.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerStartTime.ShowCheckBox = true;
            this.dateTimePickerStartTime.ShowUpDown = true;
            this.dateTimePickerStartTime.Size = new System.Drawing.Size(135, 20);
            this.dateTimePickerStartTime.TabIndex = 4;
            this.dateTimePickerStartTime.TextBackColor = System.Drawing.Color.AliceBlue;
            this.dateTimePickerStartTime.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerStartDate.BorderColorScaling = 0.5F;
            this.dateTimePickerStartDate.Checked = false;
            this.dateTimePickerStartDate.CustomFormat = "dd MMMM yyyy";
            this.dateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(78, 29);
            this.dateTimePickerStartDate.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerStartDate.ShowCheckBox = true;
            this.dateTimePickerStartDate.ShowUpDown = false;
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(211, 20);
            this.dateTimePickerStartDate.TabIndex = 3;
            this.dateTimePickerStartDate.TextBackColor = System.Drawing.Color.AliceBlue;
            this.dateTimePickerStartDate.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // labelDateStart
            // 
            this.labelDateStart.AutoSize = true;
            this.labelDateStart.Location = new System.Drawing.Point(3, 27);
            this.labelDateStart.Name = "labelDateStart";
            this.labelDateStart.Size = new System.Drawing.Size(58, 13);
            this.labelDateStart.TabIndex = 2;
            this.labelDateStart.Text = "Start Date:";
            // 
            // textBoxRouteName
            // 
            this.textBoxRouteName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxRouteName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxRouteName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxRouteName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxRouteName.BorderColorScaling = 0.5F;
            this.textBoxRouteName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxRouteName.ClearOnFirstChar = false;
            this.textBoxRouteName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxRouteName.EndButtonEnable = true;
            this.textBoxRouteName.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxRouteName.EndButtonImage")));
            this.textBoxRouteName.EndButtonVisible = false;
            this.textBoxRouteName.InErrorCondition = false;
            this.textBoxRouteName.Location = new System.Drawing.Point(78, 5);
            this.textBoxRouteName.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            this.textBoxRouteName.Multiline = false;
            this.textBoxRouteName.Name = "textBoxRouteName";
            this.textBoxRouteName.ReadOnly = false;
            this.textBoxRouteName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxRouteName.SelectionLength = 0;
            this.textBoxRouteName.SelectionStart = 0;
            this.textBoxRouteName.Size = new System.Drawing.Size(211, 20);
            this.textBoxRouteName.TabIndex = 1;
            this.textBoxRouteName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxRouteName.WordWrap = true;
            // 
            // labelRouteName
            // 
            this.labelRouteName.AutoSize = true;
            this.labelRouteName.Location = new System.Drawing.Point(3, 3);
            this.labelRouteName.Name = "labelRouteName";
            this.labelRouteName.Size = new System.Drawing.Size(70, 13);
            this.labelRouteName.TabIndex = 0;
            this.labelRouteName.Text = "Route Name:";
            // 
            // dataGridView
            // 
            this.dataGridView.AllowDrop = true;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView.AutoSortByColumnName = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnReorder = true;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SystemName,
            this.Distance,
            this.Note,
            this.ColumnX,
            this.ColumnY,
            this.ColumnZ,
            this.ColumnDistStart,
            this.ColumnDistanceRemaining,
            this.CurDist,
            this.Visits,
            this.Scans,
            this.FSSBodies,
            this.KnownBodies,
            this.Stars,
            this.Info});
            this.dataGridView.ContextMenuStrip = this.contextMenuCopyPaste;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.PerColumnWordWrapControl = true;
            this.dataGridView.RowHeaderMenuStrip = null;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(1118, 565);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellDoubleClick);
            this.dataGridView.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRouteSystems_CellValidated);
            this.dataGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewRouteSystems_RowPostPaint);
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewRouteSystems_SortCompare);
            this.dataGridView.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewRouteSystems_DragDrop);
            this.dataGridView.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewRouteSystems_DragOver);
            this.dataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewRouteSystems_MouseDown);
            this.dataGridView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewRouteSystems_MouseMove);
            // 
            // contextMenuCopyPaste
            // 
            this.contextMenuCopyPaste.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.insertCopiedToolStripMenuItem,
            this.deleteRowsToolStripMenuItem,
            this.setTargetToolStripMenuItem,
            this.editBookmarkToolStripMenuItem});
            this.contextMenuCopyPaste.Name = "contextMenuCopyPaste";
            this.contextMenuCopyPaste.Size = new System.Drawing.Size(176, 136);
            this.contextMenuCopyPaste.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuCopyPaste_Opening);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // insertCopiedToolStripMenuItem
            // 
            this.insertCopiedToolStripMenuItem.Name = "insertCopiedToolStripMenuItem";
            this.insertCopiedToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.insertCopiedToolStripMenuItem.Text = "Insert Copied Rows";
            this.insertCopiedToolStripMenuItem.Click += new System.EventHandler(this.insertCopiedToolStripMenuItem_Click);
            // 
            // deleteRowsToolStripMenuItem
            // 
            this.deleteRowsToolStripMenuItem.Name = "deleteRowsToolStripMenuItem";
            this.deleteRowsToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.deleteRowsToolStripMenuItem.Text = "Delete Rows";
            this.deleteRowsToolStripMenuItem.Click += new System.EventHandler(this.deleteRowsToolStripMenuItem_Click);
            // 
            // setTargetToolStripMenuItem
            // 
            this.setTargetToolStripMenuItem.Name = "setTargetToolStripMenuItem";
            this.setTargetToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.setTargetToolStripMenuItem.Text = "Set Target";
            this.setTargetToolStripMenuItem.Click += new System.EventHandler(this.setTargetToolStripMenuItem_Click);
            // 
            // editBookmarkToolStripMenuItem
            // 
            this.editBookmarkToolStripMenuItem.Name = "editBookmarkToolStripMenuItem";
            this.editBookmarkToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.editBookmarkToolStripMenuItem.Text = "Edit bookmark";
            this.editBookmarkToolStripMenuItem.Click += new System.EventHandler(this.editBookmarkToolStripMenuItem_Click);
            // 
            // topPanel2
            // 
            this.topPanel2.AutoSize = true;
            this.topPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel2.Location = new System.Drawing.Point(445, 6);
            this.topPanel2.Name = "topPanel2";
            this.topPanel2.Size = new System.Drawing.Size(79, 0);
            this.topPanel2.TabIndex = 14;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelRouteName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelDateStart, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePickerStartTime, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePickerStartDate, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelEndDate, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtCmlDistance, 4, 2);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePickerEndDate, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dateTimePickerEndTime, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelCml, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelP2P, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtP2PDIstance, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.textBoxRouteName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.topPanel2, 3, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1134, 78);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // extPanelDataGridViewScroll
            // 
            this.extPanelDataGridViewScroll.Controls.Add(this.extScrollBarDGV);
            this.extPanelDataGridViewScroll.Controls.Add(this.dataGridView);
            this.extPanelDataGridViewScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScroll.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScroll.Location = new System.Drawing.Point(0, 108);
            this.extPanelDataGridViewScroll.Name = "extPanelDataGridViewScroll";
            this.extPanelDataGridViewScroll.Size = new System.Drawing.Size(1134, 565);
            this.extPanelDataGridViewScroll.TabIndex = 13;
            this.extPanelDataGridViewScroll.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarDGV
            // 
            this.extScrollBarDGV.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarDGV.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarDGV.ArrowColorScaling = 0.5F;
            this.extScrollBarDGV.ArrowDownDrawAngle = 270F;
            this.extScrollBarDGV.ArrowUpDrawAngle = 90F;
            this.extScrollBarDGV.BorderColor = System.Drawing.Color.White;
            this.extScrollBarDGV.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarDGV.HideScrollBar = false;
            this.extScrollBarDGV.LargeChange = 1;
            this.extScrollBarDGV.Location = new System.Drawing.Point(1118, 0);
            this.extScrollBarDGV.Maximum = 0;
            this.extScrollBarDGV.Minimum = 0;
            this.extScrollBarDGV.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarDGV.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarDGV.Name = "extScrollBarDGV";
            this.extScrollBarDGV.Size = new System.Drawing.Size(16, 565);
            this.extScrollBarDGV.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarDGV.SmallChange = 1;
            this.extScrollBarDGV.TabIndex = 14;
            this.extScrollBarDGV.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarDGV.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarDGV.ThumbColorScaling = 0.5F;
            this.extScrollBarDGV.ThumbDrawAngle = 0F;
            this.extScrollBarDGV.Value = 0;
            this.extScrollBarDGV.ValueLimited = 0;
            // 
            // rollUpPanelTop
            // 
            this.rollUpPanelTop.AutoHeight = false;
            this.rollUpPanelTop.AutoHeightWidthDisable = false;
            this.rollUpPanelTop.AutoSize = true;
            this.rollUpPanelTop.AutoWidth = false;
            this.rollUpPanelTop.Controls.Add(this.panelControls);
            this.rollUpPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.rollUpPanelTop.HiddenMarkerWidth = 400;
            this.rollUpPanelTop.Location = new System.Drawing.Point(0, 0);
            this.rollUpPanelTop.Name = "rollUpPanelTop";
            this.rollUpPanelTop.PinState = true;
            this.rollUpPanelTop.RolledUpHeight = 5;
            this.rollUpPanelTop.RollUpAnimationTime = 500;
            this.rollUpPanelTop.RollUpDelay = 1000;
            this.rollUpPanelTop.SecondHiddenMarkerWidth = 0;
            this.rollUpPanelTop.ShowHiddenMarker = true;
            this.rollUpPanelTop.Size = new System.Drawing.Size(1134, 30);
            this.rollUpPanelTop.TabIndex = 14;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.extButtonLoadRoute);
            this.panelControls.Controls.Add(this.extButtonNew);
            this.panelControls.Controls.Add(this.extButtonSave);
            this.panelControls.Controls.Add(this.extButtonDelete);
            this.panelControls.Controls.Add(this.extButtonImportFile);
            this.panelControls.Controls.Add(this.buttonReverseRoute);
            this.panelControls.Controls.Add(this.extButtonImportRoute);
            this.panelControls.Controls.Add(this.extButtonImportNavRoute);
            this.panelControls.Controls.Add(this.extButtonNavRouteLatest);
            this.panelControls.Controls.Add(this.extButtonAddSystems);
            this.panelControls.Controls.Add(this.buttonExtExport);
            this.panelControls.Controls.Add(this.extButtonShow3DMap);
            this.panelControls.Controls.Add(this.extButtonDisplayFilters);
            this.panelControls.Controls.Add(this.checkBoxEDSM);
            this.panelControls.Controls.Add(this.extCheckBoxWordWrap);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(1134, 30);
            this.panelControls.TabIndex = 32;
            // 
            // extButtonLoadRoute
            // 
            this.extButtonLoadRoute.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonLoadRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonLoadRoute.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonLoadRoute.Location = new System.Drawing.Point(8, 1);
            this.extButtonLoadRoute.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonLoadRoute.Name = "extButtonLoadRoute";
            this.extButtonLoadRoute.Size = new System.Drawing.Size(28, 28);
            this.extButtonLoadRoute.TabIndex = 30;
            this.toolTip.SetToolTip(this.extButtonLoadRoute, "Load Route");
            this.extButtonLoadRoute.UseVisualStyleBackColor = false;
            this.extButtonLoadRoute.Click += new System.EventHandler(this.extButtonLoadRoute_Click);
            // 
            // extButtonNew
            // 
            this.extButtonNew.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonNew.Image = global::EDDiscovery.Icons.Controls.ClearRoute;
            this.extButtonNew.Location = new System.Drawing.Point(48, 1);
            this.extButtonNew.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonNew.Name = "extButtonNew";
            this.extButtonNew.Size = new System.Drawing.Size(28, 28);
            this.extButtonNew.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonNew, "New Expedition");
            this.extButtonNew.UseVisualStyleBackColor = false;
            this.extButtonNew.Click += new System.EventHandler(this.extButtonNew_Click);
            // 
            // extButtonSave
            // 
            this.extButtonSave.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonSave.Image = global::EDDiscovery.Icons.Controls.Save;
            this.extButtonSave.Location = new System.Drawing.Point(88, 1);
            this.extButtonSave.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonSave.Name = "extButtonSave";
            this.extButtonSave.Size = new System.Drawing.Size(28, 28);
            this.extButtonSave.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonSave, "Save Expedition");
            this.extButtonSave.UseVisualStyleBackColor = false;
            this.extButtonSave.Click += new System.EventHandler(this.extButtonSave_Click);
            // 
            // extButtonDelete
            // 
            this.extButtonDelete.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonDelete.Image = global::EDDiscovery.Icons.Controls.Delete;
            this.extButtonDelete.Location = new System.Drawing.Point(128, 1);
            this.extButtonDelete.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonDelete.Name = "extButtonDelete";
            this.extButtonDelete.Size = new System.Drawing.Size(28, 28);
            this.extButtonDelete.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonDelete, "Delete Expedition");
            this.extButtonDelete.UseVisualStyleBackColor = false;
            this.extButtonDelete.Click += new System.EventHandler(this.extButtonDelete_Click);
            // 
            // extButtonImportFile
            // 
            this.extButtonImportFile.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonImportFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonImportFile.Image = global::EDDiscovery.Icons.Controls.ImportFile;
            this.extButtonImportFile.Location = new System.Drawing.Point(168, 1);
            this.extButtonImportFile.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonImportFile.Name = "extButtonImportFile";
            this.extButtonImportFile.Size = new System.Drawing.Size(28, 28);
            this.extButtonImportFile.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonImportFile, "Import File");
            this.extButtonImportFile.UseVisualStyleBackColor = false;
            this.extButtonImportFile.Click += new System.EventHandler(this.extButtonImport_Click);
            // 
            // extButtonImportRoute
            // 
            this.extButtonImportRoute.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonImportRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonImportRoute.Image = global::EDDiscovery.Icons.Controls.ImportRoute;
            this.extButtonImportRoute.Location = new System.Drawing.Point(248, 1);
            this.extButtonImportRoute.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonImportRoute.Name = "extButtonImportRoute";
            this.extButtonImportRoute.Size = new System.Drawing.Size(28, 28);
            this.extButtonImportRoute.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonImportRoute, "Import from Route Panel");
            this.extButtonImportRoute.UseVisualStyleBackColor = false;
            this.extButtonImportRoute.Click += new System.EventHandler(this.extButtonImportRoute_Click);
            // 
            // extButtonImportNavRoute
            // 
            this.extButtonImportNavRoute.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonImportNavRoute.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonImportNavRoute.Image = ((System.Drawing.Image)(resources.GetObject("extButtonImportNavRoute.Image")));
            this.extButtonImportNavRoute.Location = new System.Drawing.Point(288, 1);
            this.extButtonImportNavRoute.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonImportNavRoute.Name = "extButtonImportNavRoute";
            this.extButtonImportNavRoute.Size = new System.Drawing.Size(28, 28);
            this.extButtonImportNavRoute.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonImportNavRoute, "Import a Nav Route");
            this.extButtonImportNavRoute.UseVisualStyleBackColor = false;
            this.extButtonImportNavRoute.Click += new System.EventHandler(this.extButtonImportNavRoute_Click);
            // 
            // extButtonNavRouteLatest
            // 
            this.extButtonNavRouteLatest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonNavRouteLatest.Image = global::EDDiscovery.Icons.Controls.ImportNavRouteLatest;
            this.extButtonNavRouteLatest.Location = new System.Drawing.Point(328, 1);
            this.extButtonNavRouteLatest.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonNavRouteLatest.Name = "extButtonNavRouteLatest";
            this.extButtonNavRouteLatest.Size = new System.Drawing.Size(28, 28);
            this.extButtonNavRouteLatest.TabIndex = 34;
            this.toolTip.SetToolTip(this.extButtonNavRouteLatest, "Import Latest Nav Route");
            this.extButtonNavRouteLatest.UseVisualStyleBackColor = true;
            this.extButtonNavRouteLatest.Click += new System.EventHandler(this.extButtonNavLatest_Click);
            // 
            // extButtonAddSystems
            // 
            this.extButtonAddSystems.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonAddSystems.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonAddSystems.Image = global::EDDiscovery.Icons.Controls.ImportSphere;
            this.extButtonAddSystems.Location = new System.Drawing.Point(368, 1);
            this.extButtonAddSystems.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonAddSystems.Name = "extButtonAddSystems";
            this.extButtonAddSystems.Size = new System.Drawing.Size(28, 28);
            this.extButtonAddSystems.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonAddSystems, "Import Systems from EDSM/DB");
            this.extButtonAddSystems.UseVisualStyleBackColor = false;
            this.extButtonAddSystems.Click += new System.EventHandler(this.extButtonAddSystems_Click);
            // 
            // buttonExtExport
            // 
            this.buttonExtExport.BackColor = System.Drawing.SystemColors.Control;
            this.buttonExtExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExport.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExport.Location = new System.Drawing.Point(408, 1);
            this.buttonExtExport.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonExtExport.Name = "buttonExtExport";
            this.buttonExtExport.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExport.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtExport, "Export to File");
            this.buttonExtExport.UseVisualStyleBackColor = false;
            this.buttonExtExport.Click += new System.EventHandler(this.buttonExtExport_Click);
            // 
            // extButtonShow3DMap
            // 
            this.extButtonShow3DMap.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonShow3DMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonShow3DMap.Image = global::EDDiscovery.Icons.Controls.ShowOnMap;
            this.extButtonShow3DMap.Location = new System.Drawing.Point(448, 1);
            this.extButtonShow3DMap.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonShow3DMap.Name = "extButtonShow3DMap";
            this.extButtonShow3DMap.Size = new System.Drawing.Size(28, 28);
            this.extButtonShow3DMap.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonShow3DMap, "Show expedition on 3D Map");
            this.extButtonShow3DMap.UseVisualStyleBackColor = false;
            this.extButtonShow3DMap.Click += new System.EventHandler(this.extButtonShow3DMap_Click);
            // 
            // extButtonDisplayFilters
            // 
            this.extButtonDisplayFilters.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonDisplayFilters.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonDisplayFilters.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonDisplayFilters.Location = new System.Drawing.Point(488, 1);
            this.extButtonDisplayFilters.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonDisplayFilters.Name = "extButtonDisplayFilters";
            this.extButtonDisplayFilters.Size = new System.Drawing.Size(28, 28);
            this.extButtonDisplayFilters.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonDisplayFilters, "Select filters on Info");
            this.extButtonDisplayFilters.UseVisualStyleBackColor = false;
            this.extButtonDisplayFilters.Click += new System.EventHandler(this.extButtonDisplayFilters_Click);
            // 
            // checkBoxEDSM
            // 
            this.checkBoxEDSM.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEDSM.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxEDSM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxEDSM.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxEDSM.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxEDSM.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxEDSM.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxEDSM.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxEDSM.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxEDSM.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxEDSM.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxEDSM.Image = global::EDDiscovery.Icons.Controls.EDSM;
            this.checkBoxEDSM.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxEDSM.ImageIndeterminate = null;
            this.checkBoxEDSM.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxEDSM.ImageUnchecked = null;
            this.checkBoxEDSM.Location = new System.Drawing.Point(528, 1);
            this.checkBoxEDSM.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.checkBoxEDSM.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxEDSM.Name = "checkBoxEDSM";
            this.checkBoxEDSM.Size = new System.Drawing.Size(28, 28);
            this.checkBoxEDSM.TabIndex = 33;
            this.checkBoxEDSM.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxEDSM, "Show/Hide Body data from EDSM.");
            this.checkBoxEDSM.UseVisualStyleBackColor = false;
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
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(568, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extCheckBoxWordWrap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 42;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // SystemName
            // 
            this.SystemName.FillWeight = 50F;
            this.SystemName.HeaderText = "System Name";
            this.SystemName.Name = "SystemName";
            this.SystemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // Distance
            // 
            this.Distance.FillWeight = 25F;
            this.Distance.HeaderText = "Dist.";
            this.Distance.Name = "Distance";
            this.Distance.ReadOnly = true;
            // 
            // Note
            // 
            this.Note.HeaderText = "Note";
            this.Note.Name = "Note";
            this.Note.ReadOnly = true;
            // 
            // ColumnX
            // 
            this.ColumnX.FillWeight = 25F;
            this.ColumnX.HeaderText = "X";
            this.ColumnX.Name = "ColumnX";
            this.ColumnX.ReadOnly = true;
            // 
            // ColumnY
            // 
            this.ColumnY.FillWeight = 25F;
            this.ColumnY.HeaderText = "Y";
            this.ColumnY.Name = "ColumnY";
            this.ColumnY.ReadOnly = true;
            // 
            // ColumnZ
            // 
            this.ColumnZ.FillWeight = 25F;
            this.ColumnZ.HeaderText = "Z";
            this.ColumnZ.Name = "ColumnZ";
            this.ColumnZ.ReadOnly = true;
            // 
            // ColumnDistStart
            // 
            this.ColumnDistStart.FillWeight = 25F;
            this.ColumnDistStart.HeaderText = "Dist Start";
            this.ColumnDistStart.Name = "ColumnDistStart";
            this.ColumnDistStart.ReadOnly = true;
            // 
            // ColumnDistanceRemaining
            // 
            this.ColumnDistanceRemaining.FillWeight = 25F;
            this.ColumnDistanceRemaining.HeaderText = "Dist Remaining";
            this.ColumnDistanceRemaining.Name = "ColumnDistanceRemaining";
            this.ColumnDistanceRemaining.ReadOnly = true;
            // 
            // CurDist
            // 
            this.CurDist.FillWeight = 25F;
            this.CurDist.HeaderText = "Cur. Dist";
            this.CurDist.Name = "CurDist";
            this.CurDist.ReadOnly = true;
            // 
            // Visits
            // 
            this.Visits.FillWeight = 25F;
            this.Visits.HeaderText = "Visits";
            this.Visits.Name = "Visits";
            this.Visits.ReadOnly = true;
            // 
            // Scans
            // 
            this.Scans.FillWeight = 25F;
            this.Scans.HeaderText = "Scans";
            this.Scans.Name = "Scans";
            this.Scans.ReadOnly = true;
            // 
            // FSSBodies
            // 
            this.FSSBodies.FillWeight = 25F;
            this.FSSBodies.HeaderText = "FSS Bodies";
            this.FSSBodies.Name = "FSSBodies";
            this.FSSBodies.ReadOnly = true;
            // 
            // KnownBodies
            // 
            this.KnownBodies.FillWeight = 25F;
            this.KnownBodies.HeaderText = "Known Bodies";
            this.KnownBodies.Name = "KnownBodies";
            this.KnownBodies.ReadOnly = true;
            // 
            // Stars
            // 
            this.Stars.FillWeight = 25F;
            this.Stars.HeaderText = "Stars";
            this.Stars.Name = "Stars";
            this.Stars.ReadOnly = true;
            // 
            // Info
            // 
            this.Info.FillWeight = 150F;
            this.Info.HeaderText = "Info";
            this.Info.Name = "Info";
            this.Info.ReadOnly = true;
            // 
            // UserControlExpedition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extPanelDataGridViewScroll);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.rollUpPanelTop);
            this.Name = "UserControlExpedition";
            this.Size = new System.Drawing.Size(1134, 673);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.contextMenuCopyPaste.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.extPanelDataGridViewScroll.ResumeLayout(false);
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtDateTimePicker dateTimePickerEndTime;
        private ExtendedControls.ExtDateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label labelEndDate;
        private ExtendedControls.ExtDateTimePicker dateTimePickerStartTime;
        private ExtendedControls.ExtDateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelDateStart;
        private System.Windows.Forms.Label labelRouteName;
        private BaseUtils.DataGridViewColumnControl dataGridView;
        private ExtendedControls.ExtTextBox textBoxRouteName;
        private System.Windows.Forms.ContextMenuStrip contextMenuCopyPaste;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertCopiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRowsToolStripMenuItem;
        private ExtendedControls.ExtButton buttonReverseRoute;
        private System.Windows.Forms.Label labelCml;
        private System.Windows.Forms.Label labelP2P;
        private ExtendedControls.ExtTextBox txtP2PDIstance;
        private ExtendedControls.ExtTextBox txtCmlDistance;
        private System.Windows.Forms.ToolStripMenuItem setTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editBookmarkToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel topPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScroll;
        private ExtendedControls.ExtScrollBar extScrollBarDGV;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private ExtendedControls.ExtButton extButtonNew;
        private ExtendedControls.ExtButton extButtonImportFile;
        private ExtendedControls.ExtButton extButtonImportRoute;
        private ExtendedControls.ExtButton extButtonImportNavRoute;
        private ExtendedControls.ExtButton buttonExtExport;
        private ExtendedControls.ExtButton extButtonDelete;
        private ExtendedControls.ExtButton extButtonShow3DMap;
        private ExtendedControls.ExtButton extButtonLoadRoute;
        private ExtendedControls.ExtButton extButtonSave;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton extButtonAddSystems;
        private ExtendedControls.ExtButton extButtonDisplayFilters;
        private ExtendedControls.ExtCheckBox checkBoxEDSM;
        private ExtendedControls.ExtButton extButtonNavRouteLatest;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private ExtendedControls.ExtDataGridViewColumnAutoComplete SystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Distance;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistanceRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn CurDist;
        private System.Windows.Forms.DataGridViewTextBoxColumn Visits;
        private System.Windows.Forms.DataGridViewTextBoxColumn Scans;
        private System.Windows.Forms.DataGridViewTextBoxColumn FSSBodies;
        private System.Windows.Forms.DataGridViewTextBoxColumn KnownBodies;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stars;
        private System.Windows.Forms.DataGridViewTextBoxColumn Info;
    }
}
