/*
 * Copyright © 2021 - 2021 EDDiscovery development team
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
    partial class UserControlSuitsWeapons
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelPrev = new System.Windows.Forms.Panel();
            this.extPanelDataGridViewScrollWeapons = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewWeapons = new BaseUtils.DataGridViewColumnControl();
            this.CWTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWMods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWPrimary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWWType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWDamageType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWFireMode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWDamage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWRPS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWDPS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWClipSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWHopper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWRange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWHSD = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extScrollBar1 = new ExtendedControls.ExtScrollBar();
            this.panelCurrent = new System.Windows.Forms.Panel();
            this.extPanelDataGridViewScrollSuits = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewSuits = new BaseUtils.DataGridViewColumnControl();
            this.CSTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSMods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSLoadout = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSPrimary1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSPrimary2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSSecondary = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripSuits = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.forceSellShipToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extScrollBar2 = new ExtendedControls.ExtScrollBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.splitContainerSuitsWeapons = new System.Windows.Forms.SplitContainer();
            this.panelPrev.SuspendLayout();
            this.extPanelDataGridViewScrollWeapons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWeapons)).BeginInit();
            this.panelCurrent.SuspendLayout();
            this.extPanelDataGridViewScrollSuits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSuits)).BeginInit();
            this.contextMenuStripSuits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSuitsWeapons)).BeginInit();
            this.splitContainerSuitsWeapons.Panel1.SuspendLayout();
            this.splitContainerSuitsWeapons.Panel2.SuspendLayout();
            this.splitContainerSuitsWeapons.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPrev
            // 
            this.panelPrev.Controls.Add(this.extPanelDataGridViewScrollWeapons);
            this.panelPrev.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPrev.Location = new System.Drawing.Point(0, 0);
            this.panelPrev.Name = "panelPrev";
            this.panelPrev.Size = new System.Drawing.Size(800, 302);
            this.panelPrev.TabIndex = 4;
            // 
            // extPanelDataGridViewScrollWeapons
            // 
            this.extPanelDataGridViewScrollWeapons.Controls.Add(this.dataGridViewWeapons);
            this.extPanelDataGridViewScrollWeapons.Controls.Add(this.extScrollBar1);
            this.extPanelDataGridViewScrollWeapons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollWeapons.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollWeapons.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollWeapons.Name = "extPanelDataGridViewScrollWeapons";
            this.extPanelDataGridViewScrollWeapons.Size = new System.Drawing.Size(800, 302);
            this.extPanelDataGridViewScrollWeapons.TabIndex = 0;
            this.extPanelDataGridViewScrollWeapons.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewWeapons
            // 
            this.dataGridViewWeapons.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewWeapons.AllowUserToAddRows = false;
            this.dataGridViewWeapons.AllowUserToDeleteRows = false;
            this.dataGridViewWeapons.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewWeapons.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewWeapons.AutoSortByColumnName = false;
            this.dataGridViewWeapons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewWeapons.ColumnReorder = true;
            this.dataGridViewWeapons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CWTime,
            this.CWName,
            this.CWClass,
            this.CWMods,
            this.CWPrice,
            this.CWPrimary,
            this.CWWType,
            this.CWDamageType,
            this.CWFireMode,
            this.CWDamage,
            this.CWRPS,
            this.CWDPS,
            this.CWClipSize,
            this.CWHopper,
            this.CWRange,
            this.CWHSD});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewWeapons.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewWeapons.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewWeapons.Name = "dataGridViewWeapons";
            this.dataGridViewWeapons.PerColumnWordWrapControl = true;
            this.dataGridViewWeapons.ReadOnly = true;
            this.dataGridViewWeapons.RowHeaderMenuStrip = null;
            this.dataGridViewWeapons.RowHeadersVisible = false;
            this.dataGridViewWeapons.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewWeapons.SingleRowSelect = true;
            this.dataGridViewWeapons.Size = new System.Drawing.Size(784, 302);
            this.dataGridViewWeapons.TabIndex = 1;
            this.dataGridViewWeapons.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewWeapons_SortCompare);
            // 
            // CWTime
            // 
            this.CWTime.HeaderText = "Time";
            this.CWTime.Name = "CWTime";
            this.CWTime.ReadOnly = true;
            // 
            // CWName
            // 
            this.CWName.HeaderText = "Name";
            this.CWName.Name = "CWName";
            this.CWName.ReadOnly = true;
            // 
            // CWClass
            // 
            this.CWClass.FillWeight = 50F;
            this.CWClass.HeaderText = "Class";
            this.CWClass.Name = "CWClass";
            this.CWClass.ReadOnly = true;
            // 
            // CWMods
            // 
            this.CWMods.HeaderText = "Mods";
            this.CWMods.Name = "CWMods";
            this.CWMods.ReadOnly = true;
            // 
            // CWPrice
            // 
            this.CWPrice.FillWeight = 50F;
            this.CWPrice.HeaderText = "Price";
            this.CWPrice.Name = "CWPrice";
            this.CWPrice.ReadOnly = true;
            // 
            // CWPrimary
            // 
            this.CWPrimary.FillWeight = 50F;
            this.CWPrimary.HeaderText = "Primary";
            this.CWPrimary.Name = "CWPrimary";
            this.CWPrimary.ReadOnly = true;
            // 
            // CWWType
            // 
            this.CWWType.FillWeight = 50F;
            this.CWWType.HeaderText = "Weapon Type";
            this.CWWType.Name = "CWWType";
            this.CWWType.ReadOnly = true;
            // 
            // CWDamageType
            // 
            this.CWDamageType.FillWeight = 50F;
            this.CWDamageType.HeaderText = "Damage Type";
            this.CWDamageType.Name = "CWDamageType";
            this.CWDamageType.ReadOnly = true;
            // 
            // CWFireMode
            // 
            this.CWFireMode.FillWeight = 50F;
            this.CWFireMode.HeaderText = "Fire Mode";
            this.CWFireMode.Name = "CWFireMode";
            this.CWFireMode.ReadOnly = true;
            // 
            // CWDamage
            // 
            this.CWDamage.FillWeight = 40F;
            this.CWDamage.HeaderText = "Damage";
            this.CWDamage.Name = "CWDamage";
            this.CWDamage.ReadOnly = true;
            // 
            // CWRPS
            // 
            this.CWRPS.FillWeight = 40F;
            this.CWRPS.HeaderText = "Rate/Sec";
            this.CWRPS.Name = "CWRPS";
            this.CWRPS.ReadOnly = true;
            // 
            // CWDPS
            // 
            this.CWDPS.FillWeight = 40F;
            this.CWDPS.HeaderText = "DPS";
            this.CWDPS.Name = "CWDPS";
            this.CWDPS.ReadOnly = true;
            // 
            // CWClipSize
            // 
            this.CWClipSize.FillWeight = 40F;
            this.CWClipSize.HeaderText = "Clip Size";
            this.CWClipSize.Name = "CWClipSize";
            this.CWClipSize.ReadOnly = true;
            // 
            // CWHopper
            // 
            this.CWHopper.FillWeight = 50F;
            this.CWHopper.HeaderText = "Hopper Size";
            this.CWHopper.Name = "CWHopper";
            this.CWHopper.ReadOnly = true;
            // 
            // CWRange
            // 
            this.CWRange.FillWeight = 40F;
            this.CWRange.HeaderText = "Range m";
            this.CWRange.Name = "CWRange";
            this.CWRange.ReadOnly = true;
            // 
            // CWHSD
            // 
            this.CWHSD.FillWeight = 40F;
            this.CWHSD.HeaderText = "Head Shot Mult";
            this.CWHSD.Name = "CWHSD";
            this.CWHSD.ReadOnly = true;
            // 
            // extScrollBar1
            // 
            this.extScrollBar1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBar1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBar1.ArrowDownDrawAngle = 270F;
            this.extScrollBar1.ArrowUpDrawAngle = 90F;
            this.extScrollBar1.BorderColor = System.Drawing.Color.White;
            this.extScrollBar1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBar1.HideScrollBar = false;
            this.extScrollBar1.LargeChange = 0;
            this.extScrollBar1.Location = new System.Drawing.Point(784, 0);
            this.extScrollBar1.Maximum = -1;
            this.extScrollBar1.Minimum = 0;
            this.extScrollBar1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBar1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBar1.Name = "extScrollBar1";
            this.extScrollBar1.Size = new System.Drawing.Size(16, 302);
            this.extScrollBar1.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBar1.SmallChange = 1;
            this.extScrollBar1.TabIndex = 0;
            this.extScrollBar1.Text = "";
            this.extScrollBar1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBar1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBar1.ThumbDrawAngle = 0F;
            this.extScrollBar1.Value = -1;
            this.extScrollBar1.ValueLimited = -1;
            // 
            // panelCurrent
            // 
            this.panelCurrent.Controls.Add(this.extPanelDataGridViewScrollSuits);
            this.panelCurrent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCurrent.Location = new System.Drawing.Point(0, 0);
            this.panelCurrent.Name = "panelCurrent";
            this.panelCurrent.Size = new System.Drawing.Size(800, 266);
            this.panelCurrent.TabIndex = 3;
            // 
            // extPanelDataGridViewScrollSuits
            // 
            this.extPanelDataGridViewScrollSuits.Controls.Add(this.dataGridViewSuits);
            this.extPanelDataGridViewScrollSuits.Controls.Add(this.extScrollBar2);
            this.extPanelDataGridViewScrollSuits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollSuits.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollSuits.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollSuits.Name = "extPanelDataGridViewScrollSuits";
            this.extPanelDataGridViewScrollSuits.Size = new System.Drawing.Size(800, 266);
            this.extPanelDataGridViewScrollSuits.TabIndex = 1;
            this.extPanelDataGridViewScrollSuits.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewSuits
            // 
            this.dataGridViewSuits.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewSuits.AllowUserToAddRows = false;
            this.dataGridViewSuits.AllowUserToDeleteRows = false;
            this.dataGridViewSuits.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSuits.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewSuits.AutoSortByColumnName = false;
            this.dataGridViewSuits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSuits.ColumnReorder = true;
            this.dataGridViewSuits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CSTime,
            this.CSName,
            this.CSMods,
            this.CSPrice,
            this.CSLoadout,
            this.CSPrimary1,
            this.CSPrimary2,
            this.CSSecondary});
            this.dataGridViewSuits.ContextMenuStrip = this.contextMenuStripSuits;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewSuits.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewSuits.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSuits.Name = "dataGridViewSuits";
            this.dataGridViewSuits.PerColumnWordWrapControl = true;
            this.dataGridViewSuits.ReadOnly = true;
            this.dataGridViewSuits.RowHeaderMenuStrip = null;
            this.dataGridViewSuits.RowHeadersVisible = false;
            this.dataGridViewSuits.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewSuits.SingleRowSelect = true;
            this.dataGridViewSuits.Size = new System.Drawing.Size(784, 266);
            this.dataGridViewSuits.TabIndex = 1;
            this.dataGridViewSuits.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewSuits_SortCompare);
            // 
            // CSTime
            // 
            this.CSTime.HeaderText = "Time";
            this.CSTime.Name = "CSTime";
            this.CSTime.ReadOnly = true;
            // 
            // CSName
            // 
            this.CSName.HeaderText = "Name";
            this.CSName.Name = "CSName";
            this.CSName.ReadOnly = true;
            // 
            // CSMods
            // 
            this.CSMods.HeaderText = "Mods";
            this.CSMods.Name = "CSMods";
            this.CSMods.ReadOnly = true;
            // 
            // CSPrice
            // 
            this.CSPrice.HeaderText = "Price";
            this.CSPrice.Name = "CSPrice";
            this.CSPrice.ReadOnly = true;
            // 
            // CSLoadout
            // 
            this.CSLoadout.HeaderText = "Loadout";
            this.CSLoadout.Name = "CSLoadout";
            this.CSLoadout.ReadOnly = true;
            // 
            // CSPrimary1
            // 
            this.CSPrimary1.HeaderText = "Primary 1";
            this.CSPrimary1.Name = "CSPrimary1";
            this.CSPrimary1.ReadOnly = true;
            // 
            // CSPrimary2
            // 
            this.CSPrimary2.HeaderText = "Primary 2";
            this.CSPrimary2.Name = "CSPrimary2";
            this.CSPrimary2.ReadOnly = true;
            // 
            // CSSecondary
            // 
            this.CSSecondary.HeaderText = "Secondary";
            this.CSSecondary.Name = "CSSecondary";
            this.CSSecondary.ReadOnly = true;
            // 
            // contextMenuStripSuits
            // 
            this.contextMenuStripSuits.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forceSellShipToolStripMenuItem});
            this.contextMenuStripSuits.Name = "contextMenuStripSuits";
            this.contextMenuStripSuits.Size = new System.Drawing.Size(151, 26);
            // 
            // forceSellShipToolStripMenuItem
            // 
            this.forceSellShipToolStripMenuItem.Name = "forceSellShipToolStripMenuItem";
            this.forceSellShipToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.forceSellShipToolStripMenuItem.Text = "Force Suit Sale";
            this.forceSellShipToolStripMenuItem.Click += new System.EventHandler(this.forceSellSuitToolStripMenuItem_Click);
            // 
            // extScrollBar2
            // 
            this.extScrollBar2.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBar2.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBar2.ArrowDownDrawAngle = 270F;
            this.extScrollBar2.ArrowUpDrawAngle = 90F;
            this.extScrollBar2.BorderColor = System.Drawing.Color.White;
            this.extScrollBar2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBar2.HideScrollBar = false;
            this.extScrollBar2.LargeChange = 0;
            this.extScrollBar2.Location = new System.Drawing.Point(784, 0);
            this.extScrollBar2.Maximum = -1;
            this.extScrollBar2.Minimum = 0;
            this.extScrollBar2.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBar2.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBar2.Name = "extScrollBar2";
            this.extScrollBar2.Size = new System.Drawing.Size(16, 266);
            this.extScrollBar2.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBar2.SmallChange = 1;
            this.extScrollBar2.TabIndex = 0;
            this.extScrollBar2.Text = "";
            this.extScrollBar2.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBar2.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBar2.ThumbDrawAngle = 0F;
            this.extScrollBar2.Value = -1;
            this.extScrollBar2.ValueLimited = -1;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // splitContainerSuitsWeapons
            // 
            this.splitContainerSuitsWeapons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerSuitsWeapons.Location = new System.Drawing.Point(0, 0);
            this.splitContainerSuitsWeapons.Name = "splitContainerSuitsWeapons";
            this.splitContainerSuitsWeapons.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerSuitsWeapons.Panel1
            // 
            this.splitContainerSuitsWeapons.Panel1.Controls.Add(this.panelCurrent);
            // 
            // splitContainerSuitsWeapons.Panel2
            // 
            this.splitContainerSuitsWeapons.Panel2.Controls.Add(this.panelPrev);
            this.splitContainerSuitsWeapons.Size = new System.Drawing.Size(800, 572);
            this.splitContainerSuitsWeapons.SplitterDistance = 266;
            this.splitContainerSuitsWeapons.TabIndex = 3;
            // 
            // UserControlSuitsWeapons
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerSuitsWeapons);
            this.Name = "UserControlSuitsWeapons";
            this.Size = new System.Drawing.Size(800, 572);
            this.panelPrev.ResumeLayout(false);
            this.extPanelDataGridViewScrollWeapons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewWeapons)).EndInit();
            this.panelCurrent.ResumeLayout(false);
            this.extPanelDataGridViewScrollSuits.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSuits)).EndInit();
            this.contextMenuStripSuits.ResumeLayout(false);
            this.splitContainerSuitsWeapons.Panel1.ResumeLayout(false);
            this.splitContainerSuitsWeapons.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSuitsWeapons)).EndInit();
            this.splitContainerSuitsWeapons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelPrev;
        private System.Windows.Forms.Panel panelCurrent;
        private System.Windows.Forms.SplitContainer splitContainerSuitsWeapons;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollWeapons;
        private BaseUtils.DataGridViewColumnControl dataGridViewWeapons;
        private ExtendedControls.ExtScrollBar extScrollBar1;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollSuits;
        private BaseUtils.DataGridViewColumnControl dataGridViewSuits;
        private ExtendedControls.ExtScrollBar extScrollBar2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSuits;
        private System.Windows.Forms.ToolStripMenuItem forceSellShipToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSMods;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSLoadout;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSPrimary1;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSPrimary2;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSSecondary;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWName;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWMods;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWPrimary;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWWType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWDamageType;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWFireMode;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWDamage;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWRPS;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWDPS;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWClipSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWHopper;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWRange;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWHSD;
    }
}
