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
    partial class UserControlScanGrid
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
			this.dataViewScrollerPanel2 = new ExtendedControls.DataViewScrollerPanel();
			this.dataGridViewScangrid = new System.Windows.Forms.DataGridView();
			this.ImageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Distance = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colBriefing = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.vScrollBarCustom2 = new ExtendedControls.VScrollBarCustom();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStripSG = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.showCircumstellarZonesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showAvailableMaterialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.showAtmoshphericDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.goldilocksZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.metalRichToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.earthLikeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.waterWorldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ammoniaWorldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.narrowHeader = new System.Windows.Forms.ToolStripMenuItem();
			this.wideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataViewScrollerPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewScangrid)).BeginInit();
			this.contextMenuStripSG.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataViewScrollerPanel2
			// 
			this.dataViewScrollerPanel2.Controls.Add(this.dataGridViewScangrid);
			this.dataViewScrollerPanel2.Controls.Add(this.vScrollBarCustom2);
			this.dataViewScrollerPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataViewScrollerPanel2.InternalMargin = new System.Windows.Forms.Padding(0);
			this.dataViewScrollerPanel2.Location = new System.Drawing.Point(0, 0);
			this.dataViewScrollerPanel2.Name = "dataViewScrollerPanel2";
			this.dataViewScrollerPanel2.ScrollBarWidth = 20;
			this.dataViewScrollerPanel2.Size = new System.Drawing.Size(572, 572);
			this.dataViewScrollerPanel2.TabIndex = 25;
			this.dataViewScrollerPanel2.VerticalScrollBarDockRight = true;
			// 
			// dataGridViewScangrid
			// 
			this.dataGridViewScangrid.AllowUserToAddRows = false;
			this.dataGridViewScangrid.AllowUserToDeleteRows = false;
			this.dataGridViewScangrid.AllowUserToResizeRows = false;
			this.dataGridViewScangrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridViewScangrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dataGridViewScangrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewScangrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ImageColumn,
            this.colName,
            this.colClass,
            this.Distance,
            this.colBriefing});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewScangrid.DefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridViewScangrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridViewScangrid.Location = new System.Drawing.Point(0, 0);
			this.dataGridViewScangrid.Name = "dataGridViewScangrid";
			this.dataGridViewScangrid.ReadOnly = true;
			this.dataGridViewScangrid.RowHeadersVisible = false;
			this.dataGridViewScangrid.RowTemplate.Height = 36;
			this.dataGridViewScangrid.RowTemplate.ReadOnly = true;
			this.dataGridViewScangrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dataGridViewScangrid.Size = new System.Drawing.Size(552, 572);
			this.dataGridViewScangrid.TabIndex = 23;
			this.dataGridViewScangrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewScanGrid_CellDoubleClick);
			this.dataGridViewScangrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewScanGrid_RowPostPaint);
			// 
			// ImageColumn
			// 
			this.ImageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.ImageColumn.HeaderText = "";
			this.ImageColumn.MinimumWidth = 36;
			this.ImageColumn.Name = "ImageColumn";
			this.ImageColumn.ReadOnly = true;
			this.ImageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.ImageColumn.Width = 36;
			// 
			// colName
			// 
			this.colName.FillWeight = 37.56873F;
			this.colName.HeaderText = "Name";
			this.colName.MinimumWidth = 20;
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// colClass
			// 
			this.colClass.FillWeight = 46.96092F;
			this.colClass.HeaderText = "Class";
			this.colClass.MinimumWidth = 20;
			this.colClass.Name = "colClass";
			this.colClass.ReadOnly = true;
			// 
			// Distance
			// 
			this.Distance.FillWeight = 23.39628F;
			this.Distance.HeaderText = "Distance";
			this.Distance.Name = "Distance";
			this.Distance.ReadOnly = true;
			// 
			// colBriefing
			// 
			this.colBriefing.FillWeight = 93.92184F;
			this.colBriefing.HeaderText = "Briefing";
			this.colBriefing.MinimumWidth = 20;
			this.colBriefing.Name = "colBriefing";
			this.colBriefing.ReadOnly = true;
			// 
			// vScrollBarCustom2
			// 
			this.vScrollBarCustom2.ArrowBorderColor = System.Drawing.Color.LightBlue;
			this.vScrollBarCustom2.ArrowButtonColor = System.Drawing.Color.LightGray;
			this.vScrollBarCustom2.ArrowColorScaling = 0.5F;
			this.vScrollBarCustom2.ArrowDownDrawAngle = 270F;
			this.vScrollBarCustom2.ArrowUpDrawAngle = 90F;
			this.vScrollBarCustom2.BorderColor = System.Drawing.Color.White;
			this.vScrollBarCustom2.Dock = System.Windows.Forms.DockStyle.Top;
			this.vScrollBarCustom2.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.vScrollBarCustom2.HideScrollBar = true;
			this.vScrollBarCustom2.LargeChange = 0;
			this.vScrollBarCustom2.Location = new System.Drawing.Point(552, 18);
			this.vScrollBarCustom2.Maximum = -1;
			this.vScrollBarCustom2.Minimum = 0;
			this.vScrollBarCustom2.MouseOverButtonColor = System.Drawing.Color.Green;
			this.vScrollBarCustom2.MousePressedButtonColor = System.Drawing.Color.Red;
			this.vScrollBarCustom2.Name = "vScrollBarCustom2";
			this.vScrollBarCustom2.Size = new System.Drawing.Size(20, 554);
			this.vScrollBarCustom2.SliderColor = System.Drawing.Color.DarkGray;
			this.vScrollBarCustom2.SmallChange = 1;
			this.vScrollBarCustom2.TabIndex = 24;
			this.vScrollBarCustom2.Text = "vScrollBarCustom2";
			this.vScrollBarCustom2.ThumbBorderColor = System.Drawing.Color.Yellow;
			this.vScrollBarCustom2.ThumbButtonColor = System.Drawing.Color.DarkBlue;
			this.vScrollBarCustom2.ThumbColorScaling = 0.5F;
			this.vScrollBarCustom2.ThumbDrawAngle = 0F;
			this.vScrollBarCustom2.Value = -1;
			this.vScrollBarCustom2.ValueLimited = -1;
			// 
			// contextMenuStripSG
			// 
			this.contextMenuStripSG.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCircumstellarZonesToolStripMenuItem,
            this.showAvailableMaterialsToolStripMenuItem,
            this.showAtmoshphericDetailsToolStripMenuItem});
			this.contextMenuStripSG.Name = "contextMenuStripSG";
			this.contextMenuStripSG.Size = new System.Drawing.Size(230, 70);
			// 
			// showCircumstellarZonesToolStripMenuItem
			// 
			this.showCircumstellarZonesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.goldilocksZoneToolStripMenuItem,
            this.toolStripSeparator1,
            this.narrowHeader,
            this.metalRichToolStripMenuItem,
            this.earthLikeToolStripMenuItem,
            this.toolStripSeparator2,
            this.wideToolStripMenuItem,
            this.waterWorldsToolStripMenuItem,
            this.ammoniaWorldsToolStripMenuItem});
			this.showCircumstellarZonesToolStripMenuItem.Name = "showCircumstellarZonesToolStripMenuItem";
			this.showCircumstellarZonesToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
			this.showCircumstellarZonesToolStripMenuItem.Text = "Show &Circumstellar Zones";
			// 
			// showAvailableMaterialsToolStripMenuItem
			// 
			this.showAvailableMaterialsToolStripMenuItem.Name = "showAvailableMaterialsToolStripMenuItem";
			this.showAvailableMaterialsToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
			this.showAvailableMaterialsToolStripMenuItem.Text = "Show Available &Materials";
			// 
			// showAtmoshphericDetailsToolStripMenuItem
			// 
			this.showAtmoshphericDetailsToolStripMenuItem.Name = "showAtmoshphericDetailsToolStripMenuItem";
			this.showAtmoshphericDetailsToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
			this.showAtmoshphericDetailsToolStripMenuItem.Text = "Show &Atmoshpheric Details";
			// 
			// goldilocksZoneToolStripMenuItem
			// 
			this.goldilocksZoneToolStripMenuItem.Checked = true;
			this.goldilocksZoneToolStripMenuItem.CheckOnClick = true;
			this.goldilocksZoneToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.goldilocksZoneToolStripMenuItem.Name = "goldilocksZoneToolStripMenuItem";
			this.goldilocksZoneToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.goldilocksZoneToolStripMenuItem.Text = "&Goldilocks";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
			// 
			// metalRichToolStripMenuItem
			// 
			this.metalRichToolStripMenuItem.CheckOnClick = true;
			this.metalRichToolStripMenuItem.Name = "metalRichToolStripMenuItem";
			this.metalRichToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.metalRichToolStripMenuItem.Text = "&Metal Rich";
			// 
			// earthLikeToolStripMenuItem
			// 
			this.earthLikeToolStripMenuItem.CheckOnClick = true;
			this.earthLikeToolStripMenuItem.Name = "earthLikeToolStripMenuItem";
			this.earthLikeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.earthLikeToolStripMenuItem.Text = "&Earth Like";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
			// 
			// waterWorldsToolStripMenuItem
			// 
			this.waterWorldsToolStripMenuItem.CheckOnClick = true;
			this.waterWorldsToolStripMenuItem.Name = "waterWorldsToolStripMenuItem";
			this.waterWorldsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.waterWorldsToolStripMenuItem.Text = "&Water Worlds";
			// 
			// ammoniaWorldsToolStripMenuItem
			// 
			this.ammoniaWorldsToolStripMenuItem.CheckOnClick = true;
			this.ammoniaWorldsToolStripMenuItem.Name = "ammoniaWorldsToolStripMenuItem";
			this.ammoniaWorldsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.ammoniaWorldsToolStripMenuItem.Text = "&Ammonia Worlds";
			// 
			// narrowHeader
			// 
			this.narrowHeader.Enabled = false;
			this.narrowHeader.Name = "narrowHeader";
			this.narrowHeader.Size = new System.Drawing.Size(180, 22);
			this.narrowHeader.Text = "Narrow";
			// 
			// wideToolStripMenuItem
			// 
			this.wideToolStripMenuItem.Enabled = false;
			this.wideToolStripMenuItem.Name = "wideToolStripMenuItem";
			this.wideToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.wideToolStripMenuItem.Text = "Wide";
			// 
			// UserControlScanGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataViewScrollerPanel2);
			this.Name = "UserControlScanGrid";
			this.Size = new System.Drawing.Size(572, 572);
			this.dataViewScrollerPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewScangrid)).EndInit();
			this.contextMenuStripSG.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel2;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom2;
        private System.Windows.Forms.DataGridView dataGridViewScangrid;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DataGridViewTextBoxColumn ImageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn Distance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBriefing;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripSG;
		private System.Windows.Forms.ToolStripMenuItem showCircumstellarZonesToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem goldilocksZoneToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem narrowHeader;
		private System.Windows.Forms.ToolStripMenuItem metalRichToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem earthLikeToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem wideToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem waterWorldsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem ammoniaWorldsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showAvailableMaterialsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showAtmoshphericDetailsToolStripMenuItem;
	}
}
