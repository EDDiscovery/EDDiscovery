﻿/*
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
namespace EDDiscovery
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImportFile = new System.Windows.Forms.ToolStripButton();
            this.tsbImportSphere = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.panelRouteInfo = new System.Windows.Forms.Panel();
            this.textBoxFileName = new ExtendedControls.TextBoxBorder();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxRouteName = new ExtendedControls.TextBoxBorder();
            this.labelRouteName = new System.Windows.Forms.Label();
            this.contextMenuCopyPaste = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.insertCopiedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteRowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editBookmarkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewExplore = new System.Windows.Forms.DataGridView();
            this.dataViewScrollerPanel1 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.ColumnSystemName = new ExtendedControls.AutoCompleteDGVColumn();
            this.ColumnDist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnVisits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnScans = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPrimaryStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.panelRouteInfo.SuspendLayout();
            this.contextMenuCopyPaste.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExplore)).BeginInit();
            this.dataViewScrollerPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNew,
            this.toolStripButtonImportFile,
            this.tsbImportSphere,
            this.toolStripButtonLoad,
            this.toolStripButtonSave,
            this.toolStripButtonExport,
            this.toolStripButtonDelete,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(817, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNew.Image")));
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonNew.Text = "New";
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
            // tsbImportSphere
            // 
            this.tsbImportSphere.Image = ((System.Drawing.Image)(resources.GetObject("tsbImportSphere.Image")));
            this.tsbImportSphere.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbImportSphere.Name = "tsbImportSphere";
            this.tsbImportSphere.Size = new System.Drawing.Size(102, 22);
            this.tsbImportSphere.Text = "Import Sphere";
            this.tsbImportSphere.Click += new System.EventHandler(this.tsbImportSphere_Click);
            // 
            // toolStripButtonLoad
            // 
            this.toolStripButtonLoad.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLoad.Image")));
            this.toolStripButtonLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLoad.Name = "toolStripButtonLoad";
            this.toolStripButtonLoad.Size = new System.Drawing.Size(53, 22);
            this.toolStripButtonLoad.Text = "Load";
            this.toolStripButtonLoad.Click += new System.EventHandler(this.toolStripButtonLoad_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonSave.Text = "Save";
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
            this.toolStripButtonDelete.Size = new System.Drawing.Size(60, 22);
            this.toolStripButtonDelete.Text = "Delete";
            this.toolStripButtonDelete.Visible = false;
            this.toolStripButtonDelete.Click += new System.EventHandler(this.toolStripButtonDelete_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // panelRouteInfo
            // 
            this.panelRouteInfo.Controls.Add(this.textBoxFileName);
            this.panelRouteInfo.Controls.Add(this.label1);
            this.panelRouteInfo.Controls.Add(this.textBoxRouteName);
            this.panelRouteInfo.Controls.Add(this.labelRouteName);
            this.panelRouteInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelRouteInfo.Location = new System.Drawing.Point(0, 25);
            this.panelRouteInfo.Name = "panelRouteInfo";
            this.panelRouteInfo.Size = new System.Drawing.Size(817, 54);
            this.panelRouteInfo.TabIndex = 1;
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFileName.BorderColorScaling = 0.5F;
            this.textBoxFileName.Location = new System.Drawing.Point(90, 3);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.ReadOnly = true;
            this.textBoxFileName.Size = new System.Drawing.Size(364, 20);
            this.textBoxFileName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Filename";
            // 
            // textBoxRouteName
            // 
            this.textBoxRouteName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxRouteName.BorderColorScaling = 0.5F;
            this.textBoxRouteName.Location = new System.Drawing.Point(90, 28);
            this.textBoxRouteName.Name = "textBoxRouteName";
            this.textBoxRouteName.Size = new System.Drawing.Size(364, 20);
            this.textBoxRouteName.TabIndex = 1;
            // 
            // labelRouteName
            // 
            this.labelRouteName.AutoSize = true;
            this.labelRouteName.Location = new System.Drawing.Point(5, 31);
            this.labelRouteName.Name = "labelRouteName";
            this.labelRouteName.Size = new System.Drawing.Size(79, 13);
            this.labelRouteName.TabIndex = 0;
            this.labelRouteName.Text = "Exploration set:";
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
            this.dataGridViewExplore.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewExplore.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSystemName,
            this.ColumnDist,
            this.ColumnX,
            this.ColumnY,
            this.ColumnZ,
            this.ColumnVisits,
            this.ColumnScans,
            this.ColumnPrimaryStar,
            this.ColumnInfo,
            this.ColumnNote});
            this.dataGridViewExplore.ContextMenuStrip = this.contextMenuCopyPaste;
            this.dataGridViewExplore.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewExplore.Name = "dataGridViewExplore";
            this.dataGridViewExplore.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewExplore.Size = new System.Drawing.Size(797, 494);
            this.dataGridViewExplore.TabIndex = 2;
            this.dataGridViewExplore.CellValidated += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewExplore_CellValidated);
            this.dataGridViewExplore.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewExplore_ColumnHeaderMouseClick);
            // 
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewExplore);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 79);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(817, 494);
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
            this.vScrollBarCustom1.Location = new System.Drawing.Point(797, 21);
            this.vScrollBarCustom1.Maximum = 0;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 473);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 3;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = 0;
            this.vScrollBarCustom1.ValueLimited = 0;
            // 
            // ColumnSystemName
            // 
            this.ColumnSystemName.AutoCompleteGenerator = null;
            this.ColumnSystemName.FillWeight = 150F;
            this.ColumnSystemName.HeaderText = "System Name";
            this.ColumnSystemName.MinimumWidth = 50;
            this.ColumnSystemName.Name = "ColumnSystemName";
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
            this.ColumnVisits.HeaderText = "Visists";
            this.ColumnVisits.MinimumWidth = 25;
            this.ColumnVisits.Name = "ColumnVisits";
            this.ColumnVisits.ReadOnly = true;
            this.ColumnVisits.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Programmatic;
            // 
            // ColumnScans
            // 
            this.ColumnScans.FillWeight = 50F;
            this.ColumnScans.HeaderText = "Scans";
            this.ColumnScans.MinimumWidth = 25;
            this.ColumnScans.Name = "ColumnScans";
            this.ColumnScans.ReadOnly = true;
            // 
            // ColumnPrimaryStar
            // 
            this.ColumnPrimaryStar.FillWeight = 75F;
            this.ColumnPrimaryStar.HeaderText = "Pri star";
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
            // UserControlExploration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.panelRouteInfo);
            this.Controls.Add(this.toolStrip1);
            this.Name = "UserControlExploration";
            this.Size = new System.Drawing.Size(817, 573);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelRouteInfo.ResumeLayout(false);
            this.panelRouteInfo.PerformLayout();
            this.contextMenuCopyPaste.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewExplore)).EndInit();
            this.dataViewScrollerPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.Panel panelRouteInfo;
        private System.Windows.Forms.Label labelRouteName;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private ExtendedControls.TextBoxBorder textBoxRouteName;
        private System.Windows.Forms.ContextMenuStrip contextMenuCopyPaste;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem insertCopiedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteRowsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonDelete;
        private System.Windows.Forms.ToolStripButton toolStripButtonImportFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonExport;
        private System.Windows.Forms.ToolStripMenuItem setTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editBookmarkToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewExplore;
        private System.Windows.Forms.ToolStripButton toolStripButtonLoad;
        private ExtendedControls.TextBoxBorder textBoxFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton tsbImportSphere;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel1;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private ExtendedControls.AutoCompleteDGVColumn ColumnSystemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDist;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnVisits;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnScans;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPrimaryStar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNote;
    }
}
