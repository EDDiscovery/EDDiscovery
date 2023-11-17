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
using System;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    partial class UserControlSpanshStations
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlSpanshStations));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewOnSpanshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMarketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.viewOutfittingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.flowLayoutStarDistances = new System.Windows.Forms.FlowLayoutPanel();
            this.labelMaxLs = new System.Windows.Forms.Label();
            this.valueBoxMaxLs = new ExtendedControls.NumberBoxDouble();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.colBodyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLattitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLongitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHasMarket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutfitting = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShipyard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAllegiance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEconomy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGovernment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServices = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSmallPad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMediumPads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLargePads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.viewShipyardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.flowLayoutStarDistances.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewOnSpanshToolStripMenuItem,
            this.viewMarketToolStripMenuItem,
            this.viewOutfittingToolStripMenuItem,
            this.viewShipyardToolStripMenuItem});
            this.contextMenuStrip.Name = "closestContextMenu";
            this.contextMenuStrip.Size = new System.Drawing.Size(181, 114);
            // 
            // viewOnSpanshToolStripMenuItem
            // 
            this.viewOnSpanshToolStripMenuItem.Name = "viewOnSpanshToolStripMenuItem";
            this.viewOnSpanshToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewOnSpanshToolStripMenuItem.Text = "View on Spansh";
            this.viewOnSpanshToolStripMenuItem.Click += new System.EventHandler(this.viewOnSpanshToolStripMenuItem_Click);
            // 
            // viewMarketToolStripMenuItem
            // 
            this.viewMarketToolStripMenuItem.Name = "viewMarketToolStripMenuItem";
            this.viewMarketToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewMarketToolStripMenuItem.Text = "View Market";
            this.viewMarketToolStripMenuItem.Click += new System.EventHandler(this.viewMarketToolStripMenuItem_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // viewOutfittingToolStripMenuItem
            // 
            this.viewOutfittingToolStripMenuItem.Name = "viewOutfittingToolStripMenuItem";
            this.viewOutfittingToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewOutfittingToolStripMenuItem.Text = "View Outfitting";
            this.viewOutfittingToolStripMenuItem.Click += new System.EventHandler(this.viewOutfittingToolStripMenuItem_Click);
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 28);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(868, 544);
            this.dataViewScrollerPanel.TabIndex = 25;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.AlwaysHideScrollBar = false;
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.Dock = System.Windows.Forms.DockStyle.Top;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = true;
            this.vScrollBarCustom.LargeChange = 0;
            this.vScrollBarCustom.Location = new System.Drawing.Point(852, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(16, 544);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 24;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -1;
            this.vScrollBarCustom.ValueLimited = -1;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowRowHeaderVisibleSelection = false;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToResizeRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView.AutoSortByColumnName = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnReorder = true;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colBodyName,
            this.colStationName,
            this.colDistance,
            this.colType,
            this.colLattitude,
            this.colLongitude,
            this.colHasMarket,
            this.colOutfitting,
            this.colShipyard,
            this.colAllegiance,
            this.colEconomy,
            this.colGovernment,
            this.colServices,
            this.colSmallPad,
            this.colMediumPads,
            this.colLargePads});
            this.dataGridView.ContextMenuStrip = this.contextMenuStrip;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.PerColumnWordWrapControl = true;
            this.dataGridView.RowHeaderMenuStrip = null;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(852, 544);
            this.dataGridView.TabIndex = 23;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewNearest_CellClick);
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewNearest_CellDoubleClick);
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewNearest_SortCompare);
            this.dataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewNearest_MouseDown);
            // 
            // flowLayoutStarDistances
            // 
            this.flowLayoutStarDistances.AutoSize = true;
            this.flowLayoutStarDistances.Controls.Add(this.labelMaxLs);
            this.flowLayoutStarDistances.Controls.Add(this.valueBoxMaxLs);
            this.flowLayoutStarDistances.Controls.Add(this.buttonExtExcel);
            this.flowLayoutStarDistances.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutStarDistances.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutStarDistances.Name = "flowLayoutStarDistances";
            this.flowLayoutStarDistances.Size = new System.Drawing.Size(868, 28);
            this.flowLayoutStarDistances.TabIndex = 5;
            this.flowLayoutStarDistances.WrapContents = false;
            // 
            // labelMaxLs
            // 
            this.labelMaxLs.AutoSize = true;
            this.labelMaxLs.Location = new System.Drawing.Point(4, 4);
            this.labelMaxLs.Margin = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.labelMaxLs.Name = "labelMaxLs";
            this.labelMaxLs.Size = new System.Drawing.Size(41, 13);
            this.labelMaxLs.TabIndex = 3;
            this.labelMaxLs.Text = "Max Ls";
            // 
            // valueBoxMaxLs
            // 
            this.valueBoxMaxLs.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.valueBoxMaxLs.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.valueBoxMaxLs.BackErrorColor = System.Drawing.Color.Red;
            this.valueBoxMaxLs.BorderColor = System.Drawing.Color.Transparent;
            this.valueBoxMaxLs.BorderColorScaling = 0.5F;
            this.valueBoxMaxLs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.valueBoxMaxLs.ClearOnFirstChar = false;
            this.valueBoxMaxLs.ControlBackground = System.Drawing.SystemColors.Control;
            this.valueBoxMaxLs.DelayBeforeNotification = 500;
            this.valueBoxMaxLs.EndButtonEnable = true;
            this.valueBoxMaxLs.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("valueBoxMaxLs.EndButtonImage")));
            this.valueBoxMaxLs.EndButtonSize16ths = 10;
            this.valueBoxMaxLs.EndButtonVisible = false;
            this.valueBoxMaxLs.Format = "0.#######";
            this.valueBoxMaxLs.InErrorCondition = false;
            this.valueBoxMaxLs.Location = new System.Drawing.Point(49, 4);
            this.valueBoxMaxLs.Margin = new System.Windows.Forms.Padding(4, 4, 0, 0);
            this.valueBoxMaxLs.Maximum = 10000000D;
            this.valueBoxMaxLs.Minimum = 0D;
            this.valueBoxMaxLs.Multiline = false;
            this.valueBoxMaxLs.Name = "valueBoxMaxLs";
            this.valueBoxMaxLs.NumberStyles = ((System.Globalization.NumberStyles)(((System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowDecimalPoint) 
            | System.Globalization.NumberStyles.AllowThousands)));
            this.valueBoxMaxLs.ReadOnly = false;
            this.valueBoxMaxLs.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.valueBoxMaxLs.SelectionLength = 0;
            this.valueBoxMaxLs.SelectionStart = 0;
            this.valueBoxMaxLs.Size = new System.Drawing.Size(84, 20);
            this.valueBoxMaxLs.TabIndex = 1;
            this.valueBoxMaxLs.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.valueBoxMaxLs, "Maximum ls to station from arrival");
            this.valueBoxMaxLs.Value = 1000000D;
            this.valueBoxMaxLs.WordWrap = true;
            this.valueBoxMaxLs.ValueChanged += new System.EventHandler(this.textMinRadius_ValueChanged);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(137, 0);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(4, 0, 0, 0);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 29;
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // colBodyName
            // 
            this.colBodyName.FillWeight = 75F;
            this.colBodyName.HeaderText = "Body";
            this.colBodyName.Name = "colBodyName";
            this.colBodyName.ReadOnly = true;
            // 
            // colStationName
            // 
            this.colStationName.FillWeight = 125F;
            this.colStationName.HeaderText = "Name";
            this.colStationName.MinimumWidth = 50;
            this.colStationName.Name = "colStationName";
            this.colStationName.ReadOnly = true;
            // 
            // colDistance
            // 
            this.colDistance.HeaderText = "Distance";
            this.colDistance.MinimumWidth = 50;
            this.colDistance.Name = "colDistance";
            this.colDistance.ReadOnly = true;
            // 
            // colType
            // 
            this.colType.HeaderText = "Type";
            this.colType.Name = "colType";
            this.colType.ReadOnly = true;
            // 
            // colLattitude
            // 
            this.colLattitude.HeaderText = "Latitude";
            this.colLattitude.Name = "colLattitude";
            this.colLattitude.ReadOnly = true;
            // 
            // colLongitude
            // 
            this.colLongitude.HeaderText = "Longitude";
            this.colLongitude.Name = "colLongitude";
            this.colLongitude.ReadOnly = true;
            // 
            // colHasMarket
            // 
            this.colHasMarket.FillWeight = 50F;
            this.colHasMarket.HeaderText = "Market";
            this.colHasMarket.Name = "colHasMarket";
            this.colHasMarket.ReadOnly = true;
            // 
            // colOutfitting
            // 
            this.colOutfitting.FillWeight = 50F;
            this.colOutfitting.HeaderText = "Outfitting";
            this.colOutfitting.Name = "colOutfitting";
            this.colOutfitting.ReadOnly = true;
            // 
            // colShipyard
            // 
            this.colShipyard.FillWeight = 50F;
            this.colShipyard.HeaderText = "Shipyard";
            this.colShipyard.Name = "colShipyard";
            this.colShipyard.ReadOnly = true;
            // 
            // colAllegiance
            // 
            this.colAllegiance.HeaderText = "Allegiance";
            this.colAllegiance.Name = "colAllegiance";
            this.colAllegiance.ReadOnly = true;
            // 
            // colEconomy
            // 
            this.colEconomy.HeaderText = "Economy";
            this.colEconomy.Name = "colEconomy";
            this.colEconomy.ReadOnly = true;
            // 
            // colGovernment
            // 
            this.colGovernment.HeaderText = "Government";
            this.colGovernment.Name = "colGovernment";
            this.colGovernment.ReadOnly = true;
            // 
            // colServices
            // 
            this.colServices.HeaderText = "Services";
            this.colServices.Name = "colServices";
            this.colServices.ReadOnly = true;
            // 
            // colSmallPad
            // 
            this.colSmallPad.FillWeight = 50F;
            this.colSmallPad.HeaderText = "SPad";
            this.colSmallPad.Name = "colSmallPad";
            this.colSmallPad.ReadOnly = true;
            // 
            // colMediumPads
            // 
            this.colMediumPads.FillWeight = 50F;
            this.colMediumPads.HeaderText = "MPad";
            this.colMediumPads.Name = "colMediumPads";
            this.colMediumPads.ReadOnly = true;
            // 
            // colLargePads
            // 
            this.colLargePads.FillWeight = 50F;
            this.colLargePads.HeaderText = "LPad";
            this.colLargePads.Name = "colLargePads";
            this.colLargePads.ReadOnly = true;
            // 
            // viewShipyardToolStripMenuItem
            // 
            this.viewShipyardToolStripMenuItem.Name = "viewShipyardToolStripMenuItem";
            this.viewShipyardToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.viewShipyardToolStripMenuItem.Text = "View Shipyard";
            this.viewShipyardToolStripMenuItem.Click += new System.EventHandler(this.viewShipyardToolStripMenuItem_Click);
            // 
            // UserControlSpanshStations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.flowLayoutStarDistances);
            this.Name = "UserControlSpanshStations";
            this.Size = new System.Drawing.Size(868, 572);
            this.contextMenuStrip.ResumeLayout(false);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.flowLayoutStarDistances.ResumeLayout(false);
            this.flowLayoutStarDistances.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }               

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private BaseUtils.DataGridViewColumnControl dataGridView;
        private ToolTip toolTip;
        private FlowLayoutPanel flowLayoutStarDistances;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ToolStripMenuItem viewOnSpanshToolStripMenuItem;
        private Label labelMaxLs;
        private ExtendedControls.NumberBoxDouble valueBoxMaxLs;
        private ToolStripMenuItem viewMarketToolStripMenuItem;
        private ToolStripMenuItem viewOutfittingToolStripMenuItem;
        private DataGridViewTextBoxColumn colBodyName;
        private DataGridViewTextBoxColumn colStationName;
        private DataGridViewTextBoxColumn colDistance;
        private DataGridViewTextBoxColumn colType;
        private DataGridViewTextBoxColumn colLattitude;
        private DataGridViewTextBoxColumn colLongitude;
        private DataGridViewTextBoxColumn colHasMarket;
        private DataGridViewTextBoxColumn colOutfitting;
        private DataGridViewTextBoxColumn colShipyard;
        private DataGridViewTextBoxColumn colAllegiance;
        private DataGridViewTextBoxColumn colEconomy;
        private DataGridViewTextBoxColumn colGovernment;
        private DataGridViewTextBoxColumn colServices;
        private DataGridViewTextBoxColumn colSmallPad;
        private DataGridViewTextBoxColumn colMediumPads;
        private DataGridViewTextBoxColumn colLargePads;
        private ToolStripMenuItem viewShipyardToolStripMenuItem;
    }
}
