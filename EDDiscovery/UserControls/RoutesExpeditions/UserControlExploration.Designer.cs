/*
 * Copyright © 2017 EDDiscovery development team
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
    partial class UserControlExploration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlExploration));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.tsbAddSystems = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImportFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.textBoxFileName = new ExtendedControls.ExtTextBox();
            this.labelFilename = new System.Windows.Forms.Label();
            this.textBoxRouteName = new ExtendedControls.ExtTextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.contextMenuCopyPaste = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertCopiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewExplore = new System.Windows.Forms.DataGridView();
            this.ColumnSystemName = new ExtendedControls.ExtDataGridViewColumnAutoComplete();
            this.ColumnDist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVisits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnScans = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bodies = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPrimaryStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataViewScrollerPanel1 = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.toolStrip.SuspendLayout();
            this.contextMenuCopyPaste.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExplore)).BeginInit();
            this.dataViewScrollerPanel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonLoad,
            this.toolStripButtonSave,
            this.tsbAddSystems,
            this.toolStripButtonImportFile,
            this.toolStripButtonExport,
            this.toolStripButtonClear,
            this.toolStripSeparator1});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(817, 32);
            this.toolStrip.TabIndex = 0;
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.Image = global::EDDiscovery.Icons.Controls.Exploration_New;
            this.toolStripButtonNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(59, 29);
            this.toolStripButtonNew.Text = "New";
            this.toolStripButtonNew.Click += new System.EventHandler(this.toolStripButtonNew_Click);
            // 
            // toolStripButtonLoad
            // 
            this.toolStripButtonLoad.Image = global::EDDiscovery.Icons.Controls.Exploration_Load;
            this.toolStripButtonLoad.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoad.Name = "toolStripButtonLoad";
            this.toolStripButtonLoad.Size = new System.Drawing.Size(61, 29);
            this.toolStripButtonLoad.Text = "Load";
            this.toolStripButtonLoad.Click += new System.EventHandler(this.toolStripButtonLoad_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = global::EDDiscovery.Icons.Controls.Exploration_Save;
            this.toolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(59, 29);
            this.toolStripButtonSave.Text = "Save";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // tsbAddSystems
            // 
            this.tsbAddSystems.Image = global::EDDiscovery.Icons.Controls.Exploration_ImportSphere;
            this.tsbAddSystems.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsbAddSystems.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAddSystems.Name = "tsbAddSystems";
            this.tsbAddSystems.Size = new System.Drawing.Size(103, 29);
            this.tsbAddSystems.Text = "Add Systems";
            this.tsbAddSystems.Click += new System.EventHandler(this.tsbAddSystems_Click);
            // 
            // toolStripButtonImportFile
            // 
            this.toolStripButtonImportFile.Image = global::EDDiscovery.Icons.Controls.Exploration_ImportFile;
            this.toolStripButtonImportFile.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonImportFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImportFile.Name = "toolStripButtonImportFile";
            this.toolStripButtonImportFile.Size = new System.Drawing.Size(116, 29);
            this.toolStripButtonImportFile.Text = "Import Text File";
            this.toolStripButtonImportFile.Click += new System.EventHandler(this.toolStripButtonImportFile_Click);
            // 
            // toolStripButtonExport
            // 
            this.toolStripButtonExport.Image = global::EDDiscovery.Icons.Controls.Exploration_Export;
            this.toolStripButtonExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonExport.Name = "toolStripButtonExport";
            this.toolStripButtonExport.Size = new System.Drawing.Size(69, 29);
            this.toolStripButtonExport.Text = "Export";
            this.toolStripButtonExport.ToolTipText = "Export to file";
            this.toolStripButtonExport.Click += new System.EventHandler(this.toolStripButtonExport_Click);
            // 
            // toolStripButtonClear
            // 
            this.toolStripButtonClear.Image = global::EDDiscovery.Icons.Controls.Exploration_Delete;
            this.toolStripButtonClear.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClear.Name = "toolStripButtonClear";
            this.toolStripButtonClear.Size = new System.Drawing.Size(83, 29);
            this.toolStripButtonClear.Text = "Clear List";
            this.toolStripButtonClear.Click += new System.EventHandler(this.toolStripButtonClear_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFileName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFileName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxFileName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFileName.BorderColorScaling = 0.5F;
            this.textBoxFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFileName.ClearOnFirstChar = false;
            this.textBoxFileName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFileName.EndButtonEnable = true;
            this.textBoxFileName.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxFileName.EndButtonImage")));
            this.textBoxFileName.EndButtonVisible = false;
            this.textBoxFileName.InErrorCondition = false;
            this.textBoxFileName.Location = new System.Drawing.Point(72, 6);
            this.textBoxFileName.Multiline = false;
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.ReadOnly = true;
            this.textBoxFileName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFileName.SelectionLength = 0;
            this.textBoxFileName.SelectionStart = 0;
            this.textBoxFileName.Size = new System.Drawing.Size(364, 20);
            this.textBoxFileName.TabIndex = 3;
            this.textBoxFileName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxFileName.WordWrap = true;
            // 
            // labelFilename
            // 
            this.labelFilename.AutoSize = true;
            this.labelFilename.Location = new System.Drawing.Point(3, 3);
            this.labelFilename.Name = "labelFilename";
            this.labelFilename.Size = new System.Drawing.Size(52, 13);
            this.labelFilename.TabIndex = 2;
            this.labelFilename.Text = "Filename:";
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
            this.textBoxRouteName.Location = new System.Drawing.Point(72, 32);
            this.textBoxRouteName.Multiline = false;
            this.textBoxRouteName.Name = "textBoxRouteName";
            this.textBoxRouteName.ReadOnly = false;
            this.textBoxRouteName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxRouteName.SelectionLength = 0;
            this.textBoxRouteName.SelectionStart = 0;
            this.textBoxRouteName.Size = new System.Drawing.Size(364, 20);
            this.textBoxRouteName.TabIndex = 1;
            this.textBoxRouteName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxRouteName.WordWrap = true;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 29);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(63, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Description:";
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
            // dataGridViewExplore
            // 
            this.dataGridViewExplore.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewExplore.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewExplore.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExplore.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSystemName,
            this.ColumnDist,
            this.ColumnX,
            this.ColumnY,
            this.ColumnZ,
            this.ColumnVisits,
            this.ColumnScans,
            this.Bodies,
            this.ColumnPrimaryStar,
            this.ColumnInfo,
            this.ColumnNote});
            this.dataGridViewExplore.ContextMenuStrip = this.contextMenuCopyPaste;
            this.dataGridViewExplore.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewExplore.Name = "dataGridViewExplore";
            this.dataGridViewExplore.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewExplore.Size = new System.Drawing.Size(801, 483);
            this.dataGridViewExplore.TabIndex = 2;
            this.dataGridViewExplore.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewExplore_CellValidated);
            this.dataGridViewExplore.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewExplore_SortCompare);
            // 
            // ColumnSystemName
            // 
            this.ColumnSystemName.FillWeight = 150F;
            this.ColumnSystemName.HeaderText = "System Name";
            this.ColumnSystemName.MinimumWidth = 50;
            this.ColumnSystemName.Name = "ColumnSystemName";
            this.ColumnSystemName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ColumnDist
            // 
            this.ColumnDist.FillWeight = 75F;
            this.ColumnDist.HeaderText = "Dist";
            this.ColumnDist.MinimumWidth = 25;
            this.ColumnDist.Name = "ColumnDist";
            // 
            // ColumnX
            // 
            this.ColumnX.FillWeight = 50F;
            this.ColumnX.HeaderText = "X";
            this.ColumnX.MinimumWidth = 25;
            this.ColumnX.Name = "ColumnX";
            this.ColumnX.ReadOnly = true;
            // 
            // ColumnY
            // 
            this.ColumnY.FillWeight = 50F;
            this.ColumnY.HeaderText = "Y";
            this.ColumnY.MinimumWidth = 25;
            this.ColumnY.Name = "ColumnY";
            this.ColumnY.ReadOnly = true;
            // 
            // ColumnZ
            // 
            this.ColumnZ.FillWeight = 50F;
            this.ColumnZ.HeaderText = "Z";
            this.ColumnZ.MinimumWidth = 25;
            this.ColumnZ.Name = "ColumnZ";
            this.ColumnZ.ReadOnly = true;
            // 
            // ColumnVisits
            // 
            this.ColumnVisits.FillWeight = 50F;
            this.ColumnVisits.HeaderText = "Visits";
            this.ColumnVisits.MinimumWidth = 25;
            this.ColumnVisits.Name = "ColumnVisits";
            this.ColumnVisits.ReadOnly = true;
            // 
            // ColumnScans
            // 
            this.ColumnScans.FillWeight = 50F;
            this.ColumnScans.HeaderText = "Scans";
            this.ColumnScans.MinimumWidth = 25;
            this.ColumnScans.Name = "ColumnScans";
            this.ColumnScans.ReadOnly = true;
            // 
            // Bodies
            // 
            this.Bodies.FillWeight = 50F;
            this.Bodies.HeaderText = "Bodies";
            this.Bodies.MinimumWidth = 25;
            this.Bodies.Name = "Bodies";
            this.Bodies.ReadOnly = true;
            // 
            // ColumnPrimaryStar
            // 
            this.ColumnPrimaryStar.FillWeight = 75F;
            this.ColumnPrimaryStar.HeaderText = "Stars";
            this.ColumnPrimaryStar.MinimumWidth = 50;
            this.ColumnPrimaryStar.Name = "ColumnPrimaryStar";
            this.ColumnPrimaryStar.ReadOnly = true;
            // 
            // ColumnInfo
            // 
            this.ColumnInfo.HeaderText = "Info";
            this.ColumnInfo.MinimumWidth = 50;
            this.ColumnInfo.Name = "ColumnInfo";
            this.ColumnInfo.ReadOnly = true;
            // 
            // ColumnNote
            // 
            this.ColumnNote.HeaderText = "Note";
            this.ColumnNote.MinimumWidth = 50;
            this.ColumnNote.Name = "ColumnNote";
            this.ColumnNote.ReadOnly = true;
            // 
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewExplore);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 90);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(817, 483);
            this.dataViewScrollerPanel1.TabIndex = 3;
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
            this.vScrollBarCustom1.HideScrollBar = false;
            this.vScrollBarCustom1.LargeChange = 1;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(801, 0);
            this.vScrollBarCustom1.Maximum = 0;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(16, 483);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 3;
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = 0;
            this.vScrollBarCustom1.ValueLimited = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labelFilename, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxFileName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxRouteName, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 32);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(817, 58);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // UserControlExploration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.toolStrip);
            this.Name = "UserControlExploration";
            this.Size = new System.Drawing.Size(817, 573);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuCopyPaste.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExplore)).EndInit();
            this.dataViewScrollerPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private ExtendedControls.ExtTextBox textBoxRouteName;
        private System.Windows.Forms.ContextMenuStrip contextMenuCopyPaste;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertCopiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonClear;
        private System.Windows.Forms.ToolStripButton toolStripButtonImportFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonExport;
        private System.Windows.Forms.ToolStripMenuItem setTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editBookmarkToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewExplore;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoad;
        private ExtendedControls.ExtTextBox textBoxFileName;
        private System.Windows.Forms.Label labelFilename;
        private System.Windows.Forms.ToolStripButton tsbAddSystems;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel1;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ExtendedControls.ExtDataGridViewColumnAutoComplete ColumnSystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDist;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVisits;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnScans;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bodies;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrimaryStar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNote;
    }
}
