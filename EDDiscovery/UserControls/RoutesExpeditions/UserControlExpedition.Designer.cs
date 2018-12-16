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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImportFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImportRoute = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowOn3DMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBoxRouteSelection = new ExtendedControls.ToolStripComboBoxCustom();
            this.panelRouteInfo = new System.Windows.Forms.Panel();
            this.labelCml = new System.Windows.Forms.Label();
            this.labelP2P = new System.Windows.Forms.Label();
            this.txtP2PDIstance = new ExtendedControls.TextBoxBorder();
            this.txtCmlDistance = new ExtendedControls.TextBoxBorder();
            this.buttonReverseRoute = new ExtendedControls.ButtonExt();
            this.dateTimePickerEndTime = new ExtendedControls.CustomDateTimePicker();
            this.dateTimePickerEndDate = new ExtendedControls.CustomDateTimePicker();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerStartTime = new ExtendedControls.CustomDateTimePicker();
            this.dateTimePickerStartDate = new ExtendedControls.CustomDateTimePicker();
            this.labelDateStart = new System.Windows.Forms.Label();
            this.textBoxRouteName = new ExtendedControls.TextBoxBorder();
            this.labelRouteName = new System.Windows.Forms.Label();
            this.dataGridViewRouteSystems = new System.Windows.Forms.DataGridView();
            this.SystemName = new ExtendedControls.AutoCompleteDGVColumn();
            this.Distance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Note = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuCopyPaste = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertCopiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ctxMenuCombo = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ctxMenuItemUndelete = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.panelRouteInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRouteSystems)).BeginInit();
            this.contextMenuCopyPaste.SuspendLayout();
            this.ctxMenuCombo.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonImportFile,
            this.toolStripButtonImportRoute,
            this.toolStripButtonSave,
            this.toolStripButtonExport,
            this.toolStripButtonDelete,
            this.toolStripButtonShowOn3DMap,
            this.toolStripComboBoxRouteSelection});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(787, 25);
            this.toolStrip.TabIndex = 0;
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.Image = global::EDDiscovery.Icons.Controls.Expedition_New;
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(85, 22);
            this.toolStripButtonNew.Text = "New Route";
            this.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_Click);
            // 
            // toolStripButtonImportFile
            // 
            this.toolStripButtonImportFile.Image = global::EDDiscovery.Icons.Controls.Expedition_ImportFile;
            this.toolStripButtonImportFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImportFile.Name = "toolStripButtonImportFile";
            this.toolStripButtonImportFile.Size = new System.Drawing.Size(82, 22);
            this.toolStripButtonImportFile.Text = "Import file";
            this.toolStripButtonImportFile.Click += new System.EventHandler(this.toolStripButtonImportFile_Click);
            // 
            // toolStripButtonImportRoute
            // 
            this.toolStripButtonImportRoute.Image = global::EDDiscovery.Icons.Controls.Expedition_ImportRoute;
            this.toolStripButtonImportRoute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImportRoute.Name = "toolStripButtonImportRoute";
            this.toolStripButtonImportRoute.Size = new System.Drawing.Size(94, 22);
            this.toolStripButtonImportRoute.Text = "Import route";
            this.toolStripButtonImportRoute.ToolTipText = "Import from route tab";
            this.toolStripButtonImportRoute.Click += new System.EventHandler(this.toolStripButtonImportRoute_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::EDDiscovery.Icons.Controls.Expedition_Save;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(85, 22);
            this.toolStripButtonSave.Text = "Save Route";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonExport
            // 
            this.toolStripButtonExport.Image = global::EDDiscovery.Icons.Controls.Expedition_Export;
            this.toolStripButtonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExport.Name = "toolStripButtonExport";
            this.toolStripButtonExport.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonExport.Text = "Export";
            this.toolStripButtonExport.ToolTipText = "Export to file";
            this.toolStripButtonExport.Click += new System.EventHandler(this.toolStripButtonExport_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = global::EDDiscovery.Icons.Controls.Expedition_Delete;
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(94, 22);
            this.toolStripButtonDelete.Text = "Delete Route";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripButtonShowOn3DMap
            // 
            this.toolStripButtonShowOn3DMap.Image = global::EDDiscovery.Icons.Controls.Expedition_ShowOnMap;
            this.toolStripButtonShowOn3DMap.Name = "toolStripButtonShowOn3DMap";
            this.toolStripButtonShowOn3DMap.Size = new System.Drawing.Size(117, 20);
            this.toolStripButtonShowOn3DMap.Text = "Show on 3D Map";
            this.toolStripButtonShowOn3DMap.Click += new System.EventHandler(this.toolStripButtonShowOn3DMap_Click);
            // 
            // toolStripComboBoxRouteSelection
            // 
            this.toolStripComboBoxRouteSelection.ArrowWidth = 1;
            this.toolStripComboBoxRouteSelection.BorderColor = System.Drawing.Color.White;
            this.toolStripComboBoxRouteSelection.ButtonColorScaling = 0.5F;
            this.toolStripComboBoxRouteSelection.DataSource = null;
            this.toolStripComboBoxRouteSelection.DisplayMember = "";
            this.toolStripComboBoxRouteSelection.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.toolStripComboBoxRouteSelection.DropDownHeight = 200;
            this.toolStripComboBoxRouteSelection.DropDownWidth = 200;
            this.toolStripComboBoxRouteSelection.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBoxRouteSelection.ItemHeight = 13;
            this.toolStripComboBoxRouteSelection.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.toolStripComboBoxRouteSelection.Name = "toolStripComboBoxRouteSelection";
            this.toolStripComboBoxRouteSelection.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripComboBoxRouteSelection.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.toolStripComboBoxRouteSelection.ScrollBarColor = System.Drawing.Color.LightGray;
            this.toolStripComboBoxRouteSelection.ScrollBarWidth = 16;
            this.toolStripComboBoxRouteSelection.SelectedIndex = -1;
            this.toolStripComboBoxRouteSelection.SelectedItem = null;
            this.toolStripComboBoxRouteSelection.Size = new System.Drawing.Size(200, 22);
            this.toolStripComboBoxRouteSelection.ValueMember = "";
            this.toolStripComboBoxRouteSelection.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxRouteSelection_SelectedIndexChanged);
            this.toolStripComboBoxRouteSelection.MouseUp += new System.Windows.Forms.MouseEventHandler(this.toolStripComboBoxRouteSelection_MouseUp);
            // 
            // panelRouteInfo
            // 
            this.panelRouteInfo.Controls.Add(this.labelCml);
            this.panelRouteInfo.Controls.Add(this.labelP2P);
            this.panelRouteInfo.Controls.Add(this.txtP2PDIstance);
            this.panelRouteInfo.Controls.Add(this.txtCmlDistance);
            this.panelRouteInfo.Controls.Add(this.buttonReverseRoute);
            this.panelRouteInfo.Controls.Add(this.dateTimePickerEndTime);
            this.panelRouteInfo.Controls.Add(this.dateTimePickerEndDate);
            this.panelRouteInfo.Controls.Add(this.labelEndDate);
            this.panelRouteInfo.Controls.Add(this.dateTimePickerStartTime);
            this.panelRouteInfo.Controls.Add(this.dateTimePickerStartDate);
            this.panelRouteInfo.Controls.Add(this.labelDateStart);
            this.panelRouteInfo.Controls.Add(this.textBoxRouteName);
            this.panelRouteInfo.Controls.Add(this.labelRouteName);
            this.panelRouteInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRouteInfo.Location = new System.Drawing.Point(0, 25);
            this.panelRouteInfo.Name = "panelRouteInfo";
            this.panelRouteInfo.Size = new System.Drawing.Size(787, 89);
            this.panelRouteInfo.TabIndex = 1;
            // 
            // labelCml
            // 
            this.labelCml.AutoSize = true;
            this.labelCml.Location = new System.Drawing.Point(439, 64);
            this.labelCml.Name = "labelCml";
            this.labelCml.Size = new System.Drawing.Size(72, 13);
            this.labelCml.TabIndex = 12;
            this.labelCml.Text = "Cml Distance:";
            // 
            // labelP2P
            // 
            this.labelP2P.AutoSize = true;
            this.labelP2P.Location = new System.Drawing.Point(439, 37);
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
            this.txtP2PDIstance.InErrorCondition = false;
            this.txtP2PDIstance.Location = new System.Drawing.Point(517, 32);
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
            this.txtCmlDistance.InErrorCondition = false;
            this.txtCmlDistance.Location = new System.Drawing.Point(517, 58);
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
            this.buttonReverseRoute.Location = new System.Drawing.Point(517, 4);
            this.buttonReverseRoute.Name = "buttonReverseRoute";
            this.buttonReverseRoute.Size = new System.Drawing.Size(59, 23);
            this.buttonReverseRoute.TabIndex = 8;
            this.buttonReverseRoute.Text = "Reverse";
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
            this.dateTimePickerEndTime.Location = new System.Drawing.Point(298, 58);
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
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(81, 58);
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
            this.labelEndDate.Location = new System.Drawing.Point(5, 64);
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
            this.dateTimePickerStartTime.Location = new System.Drawing.Point(298, 31);
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
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(81, 31);
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
            this.labelDateStart.Location = new System.Drawing.Point(5, 37);
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
            this.textBoxRouteName.InErrorCondition = false;
            this.textBoxRouteName.Location = new System.Drawing.Point(81, 4);
            this.textBoxRouteName.Multiline = false;
            this.textBoxRouteName.Name = "textBoxRouteName";
            this.textBoxRouteName.ReadOnly = false;
            this.textBoxRouteName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxRouteName.SelectionLength = 0;
            this.textBoxRouteName.SelectionStart = 0;
            this.textBoxRouteName.Size = new System.Drawing.Size(352, 20);
            this.textBoxRouteName.TabIndex = 1;
            this.textBoxRouteName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxRouteName.WordWrap = true;
            // 
            // labelRouteName
            // 
            this.labelRouteName.AutoSize = true;
            this.labelRouteName.Location = new System.Drawing.Point(5, 7);
            this.labelRouteName.Name = "labelRouteName";
            this.labelRouteName.Size = new System.Drawing.Size(70, 13);
            this.labelRouteName.TabIndex = 0;
            this.labelRouteName.Text = "Route Name:";
            // 
            // dataGridViewRouteSystems
            // 
            this.dataGridViewRouteSystems.AllowDrop = true;
            this.dataGridViewRouteSystems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRouteSystems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SystemName,
            this.Distance,
            this.Note});
            this.dataGridViewRouteSystems.ContextMenuStrip = this.contextMenuCopyPaste;
            this.dataGridViewRouteSystems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewRouteSystems.Location = new System.Drawing.Point(0, 114);
            this.dataGridViewRouteSystems.Name = "dataGridViewRouteSystems";
            this.dataGridViewRouteSystems.Size = new System.Drawing.Size(787, 436);
            this.dataGridViewRouteSystems.TabIndex = 2;
            this.dataGridViewRouteSystems.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRouteSystems_CellValidated);
            this.dataGridViewRouteSystems.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewRouteSystems_CellValidating);
            this.dataGridViewRouteSystems.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewRouteSystems_RowPostPaint);
            this.dataGridViewRouteSystems.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewRouteSystems_DragDrop);
            this.dataGridViewRouteSystems.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewRouteSystems_DragOver);
            this.dataGridViewRouteSystems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewRouteSystems_MouseDown);
            this.dataGridViewRouteSystems.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewRouteSystems_MouseMove);
            // 
            // SystemName
            // 
            this.SystemName.HeaderText = "System Name";
            this.SystemName.Name = "SystemName";
            this.SystemName.Width = 200;
            // 
            // Distance
            // 
            this.Distance.HeaderText = "Dist.";
            this.Distance.Name = "Distance";
            this.Distance.ReadOnly = true;
            this.Distance.Width = 75;
            // 
            // Note
            // 
            this.Note.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Note.HeaderText = "Note";
            this.Note.Name = "Note";
            this.Note.ReadOnly = true;
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
            // ctxMenuCombo
            // 
            this.ctxMenuCombo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ctxMenuItemUndelete});
            this.ctxMenuCombo.Name = "ctxMenuCombo";
            this.ctxMenuCombo.Size = new System.Drawing.Size(122, 26);
            // 
            // ctxMenuItemUndelete
            // 
            this.ctxMenuItemUndelete.Name = "ctxMenuItemUndelete";
            this.ctxMenuItemUndelete.Size = new System.Drawing.Size(121, 22);
            this.ctxMenuItemUndelete.Text = "&Undelete";
            // 
            // UserControlExpedition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewRouteSystems);
            this.Controls.Add(this.panelRouteInfo);
            this.Controls.Add(this.toolStrip);
            this.Name = "UserControlExpedition";
            this.Size = new System.Drawing.Size(787, 550);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelRouteInfo.ResumeLayout(false);
            this.panelRouteInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRouteSystems)).EndInit();
            this.contextMenuCopyPaste.ResumeLayout(false);
            this.ctxMenuCombo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Panel panelRouteInfo;
        private ExtendedControls.CustomDateTimePicker dateTimePickerEndTime;
        private ExtendedControls.CustomDateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label labelEndDate;
        private ExtendedControls.CustomDateTimePicker dateTimePickerStartTime;
        private ExtendedControls.CustomDateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelDateStart;
        private System.Windows.Forms.Label labelRouteName;
        private System.Windows.Forms.DataGridView dataGridViewRouteSystems;
        private ExtendedControls.AutoCompleteDGVColumn SystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Distance;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowOn3DMap;
        private ExtendedControls.TextBoxBorder textBoxRouteName;
        private ExtendedControls.ToolStripComboBoxCustom toolStripComboBoxRouteSelection;
        private System.Windows.Forms.ContextMenuStrip contextMenuCopyPaste;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertCopiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRowsToolStripMenuItem;
        private ExtendedControls.ButtonExt buttonReverseRoute;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonImportFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonImportRoute;
        private System.Windows.Forms.ToolStripButton toolStripButtonExport;
        private System.Windows.Forms.Label labelCml;
        private System.Windows.Forms.Label labelP2P;
        private ExtendedControls.TextBoxBorder txtP2PDIstance;
        private ExtendedControls.TextBoxBorder txtCmlDistance;
        private System.Windows.Forms.ToolStripMenuItem setTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editBookmarkToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip ctxMenuCombo;
        private System.Windows.Forms.ToolStripMenuItem ctxMenuItemUndelete;
    }
}
