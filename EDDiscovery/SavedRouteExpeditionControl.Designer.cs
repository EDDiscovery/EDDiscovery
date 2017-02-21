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
namespace EDDiscovery
{
    partial class SavedRouteExpeditionControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SavedRouteExpeditionControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImportFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImportRoute = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonShowOn3DMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripComboBoxRouteSelection = new ExtendedControls.ToolStripComboBoxCustom();
            this.panelRouteInfo = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtP2PDIstance = new ExtendedControls.TextBoxBorder();
            this.txtCmlDistance = new ExtendedControls.TextBoxBorder();
            this.buttonReverseRoute = new ExtendedControls.ButtonExt();
            this.dateTimePickerEndTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerEndDate = new System.Windows.Forms.DateTimePicker();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerStartTime = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerStartDate = new System.Windows.Forms.DateTimePicker();
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
            this.toolStrip1.SuspendLayout();
            this.panelRouteInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRouteSystems)).BeginInit();
            this.contextMenuCopyPaste.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonImportFile,
            this.toolStripButtonImportRoute,
            this.toolStripButtonSave,
            this.toolStripButtonExport,
            this.toolStripButtonDelete,
            this.toolStripSeparator1,
            this.toolStripButtonShowOn3DMap,
            this.toolStripComboBoxRouteSelection});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(787, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNew.Image")));
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(85, 22);
            this.toolStripButtonNew.Text = "New Route";
            this.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_Click);
            // 
            // toolStripButtonImportFile
            // 
            this.toolStripButtonImportFile.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImportFile.Image")));
            this.toolStripButtonImportFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImportFile.Name = "toolStripButtonImportFile";
            this.toolStripButtonImportFile.Size = new System.Drawing.Size(82, 22);
            this.toolStripButtonImportFile.Text = "Import file";
            this.toolStripButtonImportFile.Click += new System.EventHandler(this.toolStripButtonImportFile_Click);
            // 
            // toolStripButtonImportRoute
            // 
            this.toolStripButtonImportRoute.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImportRoute.Image")));
            this.toolStripButtonImportRoute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImportRoute.Name = "toolStripButtonImportRoute";
            this.toolStripButtonImportRoute.Size = new System.Drawing.Size(94, 22);
            this.toolStripButtonImportRoute.Text = "Import route";
            this.toolStripButtonImportRoute.ToolTipText = "Import from route tab";
            this.toolStripButtonImportRoute.Click += new System.EventHandler(this.toolStripButtonImportRoute_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(85, 22);
            this.toolStripButtonSave.Text = "Save Route";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // toolStripButtonExport
            // 
            this.toolStripButtonExport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonExport.Image")));
            this.toolStripButtonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExport.Name = "toolStripButtonExport";
            this.toolStripButtonExport.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonExport.Text = "Export";
            this.toolStripButtonExport.ToolTipText = "Export to file";
            this.toolStripButtonExport.Click += new System.EventHandler(this.toolStripButtonExport_Click);
            // 
            // toolStripButtonDelete
            // 
            this.toolStripButtonDelete.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDelete.Image")));
            this.toolStripButtonDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonDelete.Name = "toolStripButtonDelete";
            this.toolStripButtonDelete.Size = new System.Drawing.Size(94, 22);
            this.toolStripButtonDelete.Text = "Delete Route";
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonShowOn3DMap
            // 
            this.toolStripButtonShowOn3DMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonShowOn3DMap.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowOn3DMap.Image")));
            this.toolStripButtonShowOn3DMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowOn3DMap.Name = "toolStripButtonShowOn3DMap";
            this.toolStripButtonShowOn3DMap.Size = new System.Drawing.Size(101, 22);
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
            this.toolStripComboBoxRouteSelection.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.toolStripComboBoxRouteSelection.ItemHeight = 13;
            this.toolStripComboBoxRouteSelection.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.toolStripComboBoxRouteSelection.Name = "toolStripComboBoxRouteSelection";
            this.toolStripComboBoxRouteSelection.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.toolStripComboBoxRouteSelection.ScrollBarColor = System.Drawing.Color.LightGray;
            this.toolStripComboBoxRouteSelection.ScrollBarWidth = 16;
            this.toolStripComboBoxRouteSelection.SelectedIndex = -1;
            this.toolStripComboBoxRouteSelection.SelectedItem = null;
            this.toolStripComboBoxRouteSelection.Size = new System.Drawing.Size(150, 22);
            this.toolStripComboBoxRouteSelection.ValueMember = "";
            this.toolStripComboBoxRouteSelection.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxRouteSelection_SelectedIndexChanged);
            // 
            // panelRouteInfo
            // 
            this.panelRouteInfo.Controls.Add(this.label2);
            this.panelRouteInfo.Controls.Add(this.label1);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(439, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Cml Distance:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(439, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "P2P Distance:";
            // 
            // txtP2PDIstance
            // 
            this.txtP2PDIstance.BorderColor = System.Drawing.Color.Transparent;
            this.txtP2PDIstance.BorderColorScaling = 0.5F;
            this.txtP2PDIstance.Location = new System.Drawing.Point(517, 32);
            this.txtP2PDIstance.Name = "txtP2PDIstance";
            this.txtP2PDIstance.ReadOnly = true;
            this.txtP2PDIstance.Size = new System.Drawing.Size(100, 20);
            this.txtP2PDIstance.TabIndex = 10;
            // 
            // txtCmlDistance
            // 
            this.txtCmlDistance.BorderColor = System.Drawing.Color.Transparent;
            this.txtCmlDistance.BorderColorScaling = 0.5F;
            this.txtCmlDistance.Location = new System.Drawing.Point(517, 58);
            this.txtCmlDistance.Name = "txtCmlDistance";
            this.txtCmlDistance.ReadOnly = true;
            this.txtCmlDistance.Size = new System.Drawing.Size(100, 20);
            this.txtCmlDistance.TabIndex = 9;
            // 
            // buttonReverseRoute
            // 
            this.buttonReverseRoute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReverseRoute.BorderColorScaling = 1.25F;
            this.buttonReverseRoute.ButtonColorScaling = 0.5F;
            this.buttonReverseRoute.ButtonDisabledScaling = 0.5F;
            this.buttonReverseRoute.Location = new System.Drawing.Point(677, 55);
            this.buttonReverseRoute.Name = "buttonReverseRoute";
            this.buttonReverseRoute.Size = new System.Drawing.Size(107, 23);
            this.buttonReverseRoute.TabIndex = 8;
            this.buttonReverseRoute.Text = "Reverse Route";
            this.buttonReverseRoute.UseVisualStyleBackColor = true;
            this.buttonReverseRoute.Click += new System.EventHandler(this.buttonReverseRoute_Click);
            // 
            // dateTimePickerEndTime
            // 
            this.dateTimePickerEndTime.Checked = false;
            this.dateTimePickerEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerEndTime.Location = new System.Drawing.Point(298, 58);
            this.dateTimePickerEndTime.Name = "dateTimePickerEndTime";
            this.dateTimePickerEndTime.ShowCheckBox = true;
            this.dateTimePickerEndTime.ShowUpDown = true;
            this.dateTimePickerEndTime.Size = new System.Drawing.Size(135, 20);
            this.dateTimePickerEndTime.TabIndex = 7;
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.Checked = false;
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(81, 58);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.ShowCheckBox = true;
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(211, 20);
            this.dateTimePickerEndDate.TabIndex = 6;
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
            this.dateTimePickerStartTime.Checked = false;
            this.dateTimePickerStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dateTimePickerStartTime.Location = new System.Drawing.Point(298, 31);
            this.dateTimePickerStartTime.Name = "dateTimePickerStartTime";
            this.dateTimePickerStartTime.ShowCheckBox = true;
            this.dateTimePickerStartTime.ShowUpDown = true;
            this.dateTimePickerStartTime.Size = new System.Drawing.Size(135, 20);
            this.dateTimePickerStartTime.TabIndex = 4;
            this.dateTimePickerStartTime.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.Checked = false;
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(81, 31);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.ShowCheckBox = true;
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(211, 20);
            this.dateTimePickerStartDate.TabIndex = 3;
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
            this.textBoxRouteName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRouteName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxRouteName.BorderColorScaling = 0.5F;
            this.textBoxRouteName.Location = new System.Drawing.Point(81, 4);
            this.textBoxRouteName.Name = "textBoxRouteName";
            this.textBoxRouteName.Size = new System.Drawing.Size(703, 20);
            this.textBoxRouteName.TabIndex = 1;
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
            // SavedRouteExpeditionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewRouteSystems);
            this.Controls.Add(this.panelRouteInfo);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SavedRouteExpeditionControl";
            this.Size = new System.Drawing.Size(787, 550);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelRouteInfo.ResumeLayout(false);
            this.panelRouteInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRouteSystems)).EndInit();
            this.contextMenuCopyPaste.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Panel panelRouteInfo;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label labelEndDate;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartTime;
        private System.Windows.Forms.DateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelDateStart;
        private System.Windows.Forms.Label labelRouteName;
        private System.Windows.Forms.DataGridView dataGridViewRouteSystems;
        private ExtendedControls.AutoCompleteDGVColumn SystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Distance;
        private System.Windows.Forms.DataGridViewTextBoxColumn Note;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.TextBoxBorder txtP2PDIstance;
        private ExtendedControls.TextBoxBorder txtCmlDistance;
        private System.Windows.Forms.ToolStripMenuItem setTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editBookmarkToolStripMenuItem;
    }
}
