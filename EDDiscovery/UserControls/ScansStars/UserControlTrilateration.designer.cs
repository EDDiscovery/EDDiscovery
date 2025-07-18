/*
 * Copyright © 2015 - 2017 EDDiscovery development team
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
using ExtendedControls;

namespace EDDiscovery.UserControls
{
    partial class UserControlTrilateration
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlTrilateration));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.trilatContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToWantedSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wantedContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.removeFromWantedSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAllWithKnownPositionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAllLocalSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAllEDSMSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addAllSectorSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonSubmitDistances = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonRemoveUnused = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonRemoveAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonMap = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabelSystem = new System.Windows.Forms.ToolStripLabel();
            this.toolStripTextBoxSystem = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabelNoCoords = new System.Windows.Forms.ToolStripLabel();
            this.toolStripAddFromHistory = new System.Windows.Forms.ToolStripButton();
            this.toolStripAddRecentHistory = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonSector = new System.Windows.Forms.ToolStripButton();
            this.splitContainerCustom1 = new System.Windows.Forms.SplitContainer();
            this.splitContainerCustom2 = new System.Windows.Forms.SplitContainer();
            this.dataViewScroller_Distances = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.dataGridViewDistances = new BaseUtils.DataGridViewBaseEnhancements();
            this.ColumnSystem = new ExtendedControls.ExtDataGridViewColumnAutoComplete();
            this.ColumnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCalculated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.richTextBox_History = new ExtendedControls.ExtRichTextBox();
            this.dataViewScroller_Wanted = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom2 = new ExtendedControls.ExtScrollBar();
            this.dataGridViewClosestSystems = new BaseUtils.DataGridViewBaseEnhancements();
            this.Source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnClosestSystemsSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.trilatContextMenu.SuspendLayout();
            this.wantedContextMenu.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCustom1)).BeginInit();
            this.splitContainerCustom1.Panel1.SuspendLayout();
            this.splitContainerCustom1.Panel2.SuspendLayout();
            this.splitContainerCustom1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCustom2)).BeginInit();
            this.splitContainerCustom2.Panel1.SuspendLayout();
            this.splitContainerCustom2.Panel2.SuspendLayout();
            this.splitContainerCustom2.SuspendLayout();
            this.dataViewScroller_Distances.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDistances)).BeginInit();
            this.dataViewScroller_Wanted.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClosestSystems)).BeginInit();
            this.SuspendLayout();
            // 
            // trilatContextMenu
            // 
            this.trilatContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToWantedSystemsToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.pasteToolStripMenuItem});
            this.trilatContextMenu.Name = "trilatContextMenu";
            this.trilatContextMenu.Size = new System.Drawing.Size(198, 70);
            // 
            // addToWantedSystemsToolStripMenuItem
            // 
            this.addToWantedSystemsToolStripMenuItem.Name = "addToWantedSystemsToolStripMenuItem";
            this.addToWantedSystemsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.addToWantedSystemsToolStripMenuItem.Text = "Add to wanted systems";
            this.addToWantedSystemsToolStripMenuItem.Click += new System.EventHandler(this.addToWantedSystemsToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // wantedContextMenu
            // 
            this.wantedContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.removeFromWantedSystemsToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem1,
            this.deleteAllWithKnownPositionToolStripMenuItem,
            this.addAllLocalSystemsToolStripMenuItem,
            this.addAllEDSMSystemsToolStripMenuItem,
            this.addAllSectorSystemsToolStripMenuItem});
            this.wantedContextMenu.Name = "wantedContextMenu";
            this.wantedContextMenu.Size = new System.Drawing.Size(234, 136);
            // 
            // removeFromWantedSystemsToolStripMenuItem
            // 
            this.removeFromWantedSystemsToolStripMenuItem.Name = "removeFromWantedSystemsToolStripMenuItem";
            this.removeFromWantedSystemsToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.removeFromWantedSystemsToolStripMenuItem.Text = "Remove from wanted systems";
            this.removeFromWantedSystemsToolStripMenuItem.Click += new System.EventHandler(this.removeFromWantedSystemsToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem1
            // 
            this.viewOnEDSMToolStripMenuItem1.Name = "viewOnEDSMToolStripMenuItem1";
            this.viewOnEDSMToolStripMenuItem1.Size = new System.Drawing.Size(233, 22);
            this.viewOnEDSMToolStripMenuItem1.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem1.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem1_Click);
            // 
            // deleteAllWithKnownPositionToolStripMenuItem
            // 
            this.deleteAllWithKnownPositionToolStripMenuItem.Name = "deleteAllWithKnownPositionToolStripMenuItem";
            this.deleteAllWithKnownPositionToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.deleteAllWithKnownPositionToolStripMenuItem.Text = "Delete all with known position";
            this.deleteAllWithKnownPositionToolStripMenuItem.Click += new System.EventHandler(this.deleteAllWithKnownPositionToolStripMenuItem_Click);
            // 
            // addAllLocalSystemsToolStripMenuItem
            // 
            this.addAllLocalSystemsToolStripMenuItem.Name = "addAllLocalSystemsToolStripMenuItem";
            this.addAllLocalSystemsToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.addAllLocalSystemsToolStripMenuItem.Text = "Add all local systems";
            this.addAllLocalSystemsToolStripMenuItem.Click += new System.EventHandler(this.addAllLocalSystemsToolStripMenuItem_Click);
            // 
            // addAllEDSMSystemsToolStripMenuItem
            // 
            this.addAllEDSMSystemsToolStripMenuItem.Name = "addAllEDSMSystemsToolStripMenuItem";
            this.addAllEDSMSystemsToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.addAllEDSMSystemsToolStripMenuItem.Text = "Add all EDSM systems";
            this.addAllEDSMSystemsToolStripMenuItem.Click += new System.EventHandler(this.addAllEDSMSystemsToolStripMenuItem_Click);
            // 
            // addAllSectorSystemsToolStripMenuItem
            // 
            this.addAllSectorSystemsToolStripMenuItem.Name = "addAllSectorSystemsToolStripMenuItem";
            this.addAllSectorSystemsToolStripMenuItem.Size = new System.Drawing.Size(233, 22);
            this.addAllSectorSystemsToolStripMenuItem.Text = "Add all Sector systems";
            this.addAllSectorSystemsToolStripMenuItem.Click += new System.EventHandler(this.addAllSectorSystemsToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.AutoSize = false;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonSubmitDistances,
            this.toolStripButtonNew,
            this.toolStripSeparator1,
            this.toolStripButtonRemoveUnused,
            this.toolStripButtonRemoveAll,
            this.toolStripButtonMap,
            this.toolStripSeparator2,
            this.toolStripLabelSystem,
            this.toolStripTextBoxSystem,
            this.toolStripLabel1,
            this.toolStripLabelNoCoords,
            this.toolStripAddFromHistory,
            this.toolStripAddRecentHistory,
            this.toolStripSeparator3,
            this.toolStripButtonSector});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1088, 32);
            this.toolStrip.TabIndex = 24;
            // 
            // toolStripButtonSubmitDistances
            // 
            this.toolStripButtonSubmitDistances.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonSubmitDistances.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripButtonSubmitDistances.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSubmitDistances.Image")));
            this.toolStripButtonSubmitDistances.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonSubmitDistances.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSubmitDistances.Name = "toolStripButtonSubmitDistances";
            this.toolStripButtonSubmitDistances.Size = new System.Drawing.Size(103, 29);
            this.toolStripButtonSubmitDistances.Text = "&Submit Distances";
            this.toolStripButtonSubmitDistances.Click += new System.EventHandler(this.toolStripButtonSubmitDistances_Click);
            // 
            // toolStripButtonNew
            // 
            this.toolStripButtonNew.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripButtonNew.Image = global::EDDiscovery.Icons.Controls.ManyStars;
            this.toolStripButtonNew.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNew.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.toolStripButtonNew.Name = "toolStripButtonNew";
            this.toolStripButtonNew.Size = new System.Drawing.Size(86, 30);
            this.toolStripButtonNew.Text = "Start New";
            this.toolStripButtonNew.ToolTipText = "Enter distances to systems with unknown coordinates";
            this.toolStripButtonNew.Click += new System.EventHandler(this.buttonStartNew_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 30);
            // 
            // toolStripButtonRemoveUnused
            // 
            this.toolStripButtonRemoveUnused.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripButtonRemoveUnused.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveUnused.Image = global::EDDiscovery.Icons.Controls.Cross;
            this.toolStripButtonRemoveUnused.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonRemoveUnused.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveUnused.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.toolStripButtonRemoveUnused.Name = "toolStripButtonRemoveUnused";
            this.toolStripButtonRemoveUnused.Size = new System.Drawing.Size(28, 30);
            this.toolStripButtonRemoveUnused.Text = "<code>";
            this.toolStripButtonRemoveUnused.ToolTipText = "Remove unused";
            this.toolStripButtonRemoveUnused.Click += new System.EventHandler(this.toolStripButtonRemoveUnused_Click);
            // 
            // toolStripButtonRemoveAll
            // 
            this.toolStripButtonRemoveAll.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripButtonRemoveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonRemoveAll.Image = global::EDDiscovery.Icons.Controls.Delete;
            this.toolStripButtonRemoveAll.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonRemoveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRemoveAll.Name = "toolStripButtonRemoveAll";
            this.toolStripButtonRemoveAll.Size = new System.Drawing.Size(28, 29);
            this.toolStripButtonRemoveAll.Text = "<code>";
            this.toolStripButtonRemoveAll.ToolTipText = "Remove all";
            this.toolStripButtonRemoveAll.Click += new System.EventHandler(this.toolStripButtonRemoveAll_Click);
            // 
            // toolStripButtonMap
            // 
            this.toolStripButtonMap.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripButtonMap.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonMap.Image = global::EDDiscovery.Icons.Controls.Globe;
            this.toolStripButtonMap.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButtonMap.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonMap.Name = "toolStripButtonMap";
            this.toolStripButtonMap.Size = new System.Drawing.Size(28, 29);
            this.toolStripButtonMap.Text = "<code>";
            this.toolStripButtonMap.ToolTipText = "Show 3d map";
            this.toolStripButtonMap.Click += new System.EventHandler(this.toolStripButtonMap_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripLabelSystem
            // 
            this.toolStripLabelSystem.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.toolStripLabelSystem.Name = "toolStripLabelSystem";
            this.toolStripLabelSystem.Size = new System.Drawing.Size(79, 27);
            this.toolStripLabelSystem.Text = "From System:";
            // 
            // toolStripTextBoxSystem
            // 
            this.toolStripTextBoxSystem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStripTextBoxSystem.Name = "toolStripTextBoxSystem";
            this.toolStripTextBoxSystem.ReadOnly = true;
            this.toolStripTextBoxSystem.Size = new System.Drawing.Size(200, 32);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(154, 30);
            this.toolStripLabel1.Text = "Visited without coordinates:";
            this.toolStripLabel1.ToolTipText = "Start New to update count";
            // 
            // toolStripLabelNoCoords
            // 
            this.toolStripLabelNoCoords.Name = "toolStripLabelNoCoords";
            this.toolStripLabelNoCoords.Size = new System.Drawing.Size(0, 29);
            // 
            // toolStripAddFromHistory
            // 
            this.toolStripAddFromHistory.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripAddFromHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripAddFromHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAddFromHistory.Name = "toolStripAddFromHistory";
            this.toolStripAddFromHistory.Size = new System.Drawing.Size(83, 29);
            this.toolStripAddFromHistory.Text = "Add 20 oldest";
            this.toolStripAddFromHistory.ToolTipText = "Add the oldest 20 unknown systems from history to wanted list";
            this.toolStripAddFromHistory.Click += new System.EventHandler(this.toolStripAddFromHistory_Click);
            // 
            // toolStripAddRecentHistory
            // 
            this.toolStripAddRecentHistory.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripAddRecentHistory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripAddRecentHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripAddRecentHistory.Name = "toolStripAddRecentHistory";
            this.toolStripAddRecentHistory.Size = new System.Drawing.Size(88, 29);
            this.toolStripAddRecentHistory.Text = "Add 20 newest";
            this.toolStripAddRecentHistory.ToolTipText = "Add 20 most recent systems with no coordinates";
            this.toolStripAddRecentHistory.Click += new System.EventHandler(this.toolStripAddRecentHistory_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripButtonSector
            // 
            this.toolStripButtonSector.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toolStripButtonSector.CheckOnClick = true;
            this.toolStripButtonSector.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSector.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSector.Name = "toolStripButtonSector";
            this.toolStripButtonSector.Size = new System.Drawing.Size(90, 29);
            this.toolStripButtonSector.Text = "Sector Systems";
            this.toolStripButtonSector.ToolTipText = "Get systems in current sector from EDSM for trilateration";
            this.toolStripButtonSector.Click += new System.EventHandler(this.toolStripButtonSector_Click);
            // 
            // splitContainerCustom1
            // 
            this.splitContainerCustom1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCustom1.Location = new System.Drawing.Point(0, 32);
            this.splitContainerCustom1.Name = "splitContainerCustom1";
            // 
            // splitContainerCustom1.Panel1
            // 
            this.splitContainerCustom1.Panel1.Controls.Add(this.splitContainerCustom2);
            // 
            // splitContainerCustom1.Panel2
            // 
            this.splitContainerCustom1.Panel2.Controls.Add(this.dataViewScroller_Wanted);
            this.splitContainerCustom1.Size = new System.Drawing.Size(1088, 820);
            this.splitContainerCustom1.SplitterDistance = 588;
            this.splitContainerCustom1.TabIndex = 25;
            // 
            // splitContainerCustom2
            // 
            this.splitContainerCustom2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCustom2.Location = new System.Drawing.Point(0, 0);
            this.splitContainerCustom2.Name = "splitContainerCustom2";
            this.splitContainerCustom2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerCustom2.Panel1
            // 
            this.splitContainerCustom2.Panel1.Controls.Add(this.dataViewScroller_Distances);
            // 
            // splitContainerCustom2.Panel2
            // 
            this.splitContainerCustom2.Panel2.Controls.Add(this.richTextBox_History);
            this.splitContainerCustom2.Size = new System.Drawing.Size(588, 820);
            this.splitContainerCustom2.SplitterDistance = 675;
            this.splitContainerCustom2.TabIndex = 0;
            // 
            // dataViewScroller_Distances
            // 
            this.dataViewScroller_Distances.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScroller_Distances.Controls.Add(this.dataGridViewDistances);
            this.dataViewScroller_Distances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScroller_Distances.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScroller_Distances.Location = new System.Drawing.Point(0, 0);
            this.dataViewScroller_Distances.Name = "dataViewScroller_Distances";
            this.dataViewScroller_Distances.ScrollBarWidth = 24;
            this.dataViewScroller_Distances.Size = new System.Drawing.Size(588, 675);
            this.dataViewScroller_Distances.TabIndex = 0;
            this.dataViewScroller_Distances.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.AlwaysHideScrollBar = false;
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = false;
            this.vScrollBarCustom1.LargeChange = 1;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(564, 0);
            this.vScrollBarCustom1.Maximum = 0;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(24, 675);
            this.vScrollBarCustom1.SkinnyStyle = false;
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SliderDrawAngle = 90F;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 1;
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = 0;
            this.vScrollBarCustom1.ValueLimited = 0;
            // 
            // dataGridViewDistances
            // 
            this.dataGridViewDistances.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewDistances.AutoSortByColumnName = false;
            this.dataGridViewDistances.ColumnHeaderMenuStrip = null;
            this.dataGridViewDistances.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewDistances.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnSystem,
            this.ColumnDistance,
            this.ColumnCalculated,
            this.ColumnStatus});
            this.dataGridViewDistances.ContextMenuStrip = this.trilatContextMenu;
            this.dataGridViewDistances.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewDistances.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewDistances.Name = "dataGridViewDistances";
            this.dataGridViewDistances.RowHeaderMenuStrip = null;
            this.dataGridViewDistances.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewDistances.SingleRowSelect = true;
            this.dataGridViewDistances.Size = new System.Drawing.Size(564, 675);
            this.dataGridViewDistances.TabIndex = 0;
            this.dataGridViewDistances.TopLeftHeaderMenuStrip = null;
            this.dataGridViewDistances.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDistances_CellClick);
            this.dataGridViewDistances.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDistances_CellEndEdit);
            this.dataGridViewDistances.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewDistances_CellLeave);
            this.dataGridViewDistances.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridViewDistances_CellValidating);
            this.dataGridViewDistances.CurrentCellChanged += new System.EventHandler(this.dataGridViewDistances_CurrentCellChanged);
            this.dataGridViewDistances.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewDistances_SortCompare);
            this.dataGridViewDistances.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridViewDistances_KeyDown);
            // 
            // ColumnSystem
            // 
            this.ColumnSystem.HeaderText = "System";
            this.ColumnSystem.Name = "ColumnSystem";
            // 
            // ColumnDistance
            // 
            this.ColumnDistance.FillWeight = 50F;
            this.ColumnDistance.HeaderText = "Distance";
            this.ColumnDistance.Name = "ColumnDistance";
            // 
            // ColumnCalculated
            // 
            this.ColumnCalculated.FillWeight = 50F;
            this.ColumnCalculated.HeaderText = "Calculated";
            this.ColumnCalculated.Name = "ColumnCalculated";
            this.ColumnCalculated.ReadOnly = true;
            this.ColumnCalculated.Visible = false;
            // 
            // ColumnStatus
            // 
            this.ColumnStatus.HeaderText = "Status";
            this.ColumnStatus.Name = "ColumnStatus";
            this.ColumnStatus.ReadOnly = true;
            // 
            // richTextBox_History
            // 
            this.richTextBox_History.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBox_History.BorderColor2 = System.Drawing.Color.Transparent;
            this.richTextBox_History.DetectUrls = true;
            this.richTextBox_History.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox_History.HideScrollBar = true;
            this.richTextBox_History.Location = new System.Drawing.Point(0, 0);
            this.richTextBox_History.Name = "richTextBox_History";
            this.richTextBox_History.ReadOnly = false;
            this.richTextBox_History.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\par" +
    "d\\f0\\fs17\\par\r\n}\r\n";
            this.richTextBox_History.ShowLineCount = false;
            this.richTextBox_History.Size = new System.Drawing.Size(588, 141);
            this.richTextBox_History.TabIndex = 0;
            this.richTextBox_History.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.richTextBox_History.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // dataViewScroller_Wanted
            // 
            this.dataViewScroller_Wanted.Controls.Add(this.vScrollBarCustom2);
            this.dataViewScroller_Wanted.Controls.Add(this.dataGridViewClosestSystems);
            this.dataViewScroller_Wanted.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScroller_Wanted.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScroller_Wanted.Location = new System.Drawing.Point(0, 0);
            this.dataViewScroller_Wanted.Name = "dataViewScroller_Wanted";
            this.dataViewScroller_Wanted.ScrollBarWidth = 24;
            this.dataViewScroller_Wanted.Size = new System.Drawing.Size(496, 820);
            this.dataViewScroller_Wanted.TabIndex = 0;
            this.dataViewScroller_Wanted.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom2
            // 
            this.vScrollBarCustom2.AlwaysHideScrollBar = false;
            this.vScrollBarCustom2.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom2.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom2.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.vScrollBarCustom2.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom2.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom2.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom2.HideScrollBar = false;
            this.vScrollBarCustom2.LargeChange = 0;
            this.vScrollBarCustom2.Location = new System.Drawing.Point(472, 0);
            this.vScrollBarCustom2.Maximum = -1;
            this.vScrollBarCustom2.Minimum = 0;
            this.vScrollBarCustom2.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom2.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom2.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom2.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom2.Name = "vScrollBarCustom2";
            this.vScrollBarCustom2.Size = new System.Drawing.Size(24, 820);
            this.vScrollBarCustom2.SkinnyStyle = false;
            this.vScrollBarCustom2.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom2.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom2.SliderDrawAngle = 90F;
            this.vScrollBarCustom2.SmallChange = 1;
            this.vScrollBarCustom2.TabIndex = 14;
            this.vScrollBarCustom2.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom2.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom2.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom2.ThumbDrawAngle = 0F;
            this.vScrollBarCustom2.Value = -1;
            this.vScrollBarCustom2.ValueLimited = -1;
            // 
            // dataGridViewClosestSystems
            // 
            this.dataGridViewClosestSystems.AllowUserToAddRows = false;
            this.dataGridViewClosestSystems.AllowUserToDeleteRows = false;
            this.dataGridViewClosestSystems.AllowUserToResizeRows = false;
            this.dataGridViewClosestSystems.AutoSortByColumnName = false;
            this.dataGridViewClosestSystems.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dataGridViewClosestSystems.ColumnHeaderMenuStrip = null;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewClosestSystems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewClosestSystems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewClosestSystems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Source,
            this.dataGridViewTextBoxColumnClosestSystemsSystem});
            this.dataGridViewClosestSystems.ContextMenuStrip = this.wantedContextMenu;
            this.dataGridViewClosestSystems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewClosestSystems.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewClosestSystems.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewClosestSystems.Name = "dataGridViewClosestSystems";
            this.dataGridViewClosestSystems.ReadOnly = true;
            this.dataGridViewClosestSystems.RowHeaderMenuStrip = null;
            this.dataGridViewClosestSystems.RowHeadersVisible = false;
            this.dataGridViewClosestSystems.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewClosestSystems.SingleRowSelect = true;
            this.dataGridViewClosestSystems.Size = new System.Drawing.Size(472, 820);
            this.dataGridViewClosestSystems.TabIndex = 13;
            this.dataGridViewClosestSystems.TopLeftHeaderMenuStrip = null;
            this.dataGridViewClosestSystems.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewClosestSystems_CellMouseClick);
            // 
            // Source
            // 
            this.Source.HeaderText = "Source";
            this.Source.Name = "Source";
            this.Source.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnClosestSystemsSystem
            // 
            this.dataGridViewTextBoxColumnClosestSystemsSystem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnClosestSystemsSystem.HeaderText = "Wanted System";
            this.dataGridViewTextBoxColumnClosestSystemsSystem.MinimumWidth = 100;
            this.dataGridViewTextBoxColumnClosestSystemsSystem.Name = "dataGridViewTextBoxColumnClosestSystemsSystem";
            this.dataGridViewTextBoxColumnClosestSystemsSystem.ReadOnly = true;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlTrilateration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerCustom1);
            this.Controls.Add(this.toolStrip);
            this.Name = "UserControlTrilateration";
            this.Size = new System.Drawing.Size(1088, 852);
            this.trilatContextMenu.ResumeLayout(false);
            this.wantedContextMenu.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainerCustom1.Panel1.ResumeLayout(false);
            this.splitContainerCustom1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCustom1)).EndInit();
            this.splitContainerCustom1.ResumeLayout(false);
            this.splitContainerCustom2.Panel1.ResumeLayout(false);
            this.splitContainerCustom2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCustom2)).EndInit();
            this.splitContainerCustom2.ResumeLayout(false);
            this.dataViewScroller_Distances.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDistances)).EndInit();
            this.dataViewScroller_Wanted.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClosestSystems)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        protected BaseUtils.DataGridViewBaseEnhancements dataGridViewDistances;
        private System.Windows.Forms.ContextMenuStrip trilatContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addToWantedSystemsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip wantedContextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeFromWantedSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem1;
        private BaseUtils.DataGridViewBaseEnhancements dataGridViewClosestSystems;
        private System.Windows.Forms.DataGridViewTextBoxColumn Source;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnClosestSystemsSystem;
        private System.Windows.Forms.SplitContainer splitContainerCustom1;
        private System.Windows.Forms.SplitContainer splitContainerCustom2;
        private ExtendedControls.ExtRichTextBox richTextBox_History;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScroller_Distances;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScroller_Wanted;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private ExtendedControls.ExtScrollBar vScrollBarCustom2;
        private System.Windows.Forms.ToolStripMenuItem deleteAllWithKnownPositionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addAllLocalSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addAllEDSMSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonSubmitDistances;
        private System.Windows.Forms.ToolStripButton toolStripButtonNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveUnused;
        private System.Windows.Forms.ToolStripButton toolStripButtonRemoveAll;
        private System.Windows.Forms.ToolStripButton toolStripButtonMap;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel toolStripLabelSystem;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBoxSystem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelNoCoords;
        private System.Windows.Forms.ToolStripButton toolStripAddFromHistory;
        private System.Windows.Forms.ToolStripButton toolStripAddRecentHistory;
        private System.Windows.Forms.ToolStripMenuItem addAllSectorSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonSector;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private ExtDataGridViewColumnAutoComplete ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCalculated;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStatus;
    }
}
