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
			this.dataGridViewScanGrid = new System.Windows.Forms.DataGridView();
			this.colImg = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.vScrollBarCustom2 = new ExtendedControls.VScrollBarCustom();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.contextMenuStripSG = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toggleColumnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.classToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.distanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.informationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleDetailsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.circumstellarZonesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.materialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.atmosphereToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.massToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.radiusToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.valueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.dataViewScrollerPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewScanGrid)).BeginInit();
			this.contextMenuStripSG.SuspendLayout();
			this.SuspendLayout();
			// 
			// dataViewScrollerPanel2
			// 
			this.dataViewScrollerPanel2.Controls.Add(this.dataGridViewScanGrid);
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
			// dataGridViewScanGrid
			// 
			this.dataGridViewScanGrid.AllowUserToAddRows = false;
			this.dataGridViewScanGrid.AllowUserToDeleteRows = false;
			this.dataGridViewScanGrid.AllowUserToResizeRows = false;
			this.dataGridViewScanGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
			this.dataGridViewScanGrid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
			this.dataGridViewScanGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridViewScanGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colImg,
            this.colName,
            this.colClass,
            this.colDistance,
            this.colInformation});
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.dataGridViewScanGrid.DefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridViewScanGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dataGridViewScanGrid.Location = new System.Drawing.Point(0, 0);
			this.dataGridViewScanGrid.Name = "dataGridViewScanGrid";
			this.dataGridViewScanGrid.ReadOnly = true;
			this.dataGridViewScanGrid.RowHeadersVisible = false;
			this.dataGridViewScanGrid.RowTemplate.Height = 36;
			this.dataGridViewScanGrid.RowTemplate.ReadOnly = true;
			this.dataGridViewScanGrid.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.dataGridViewScanGrid.Size = new System.Drawing.Size(552, 572);
			this.dataGridViewScanGrid.TabIndex = 23;
			this.dataGridViewScanGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewScangrid_CellDoubleClick);
			this.dataGridViewScanGrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewScangrid_RowPostPaint);
			this.dataGridViewScanGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewScanGrid_MouseClick);
			// 
			// colImg
			// 
			this.colImg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
			this.colImg.HeaderText = "";
			this.colImg.MinimumWidth = 36;
			this.colImg.Name = "colImg";
			this.colImg.ReadOnly = true;
			this.colImg.Resizable = System.Windows.Forms.DataGridViewTriState.False;
			this.colImg.Width = 36;
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
			// colDistance
			// 
			this.colDistance.FillWeight = 23.39628F;
			this.colDistance.HeaderText = "Distance";
			this.colDistance.Name = "colDistance";
			this.colDistance.ReadOnly = true;
			// 
			// colInformation
			// 
			this.colInformation.FillWeight = 93.92184F;
			this.colInformation.HeaderText = "Information";
			this.colInformation.MinimumWidth = 20;
			this.colInformation.Name = "colInformation";
			this.colInformation.ReadOnly = true;
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
            this.toggleColumnsToolStripMenuItem,
            this.toggleDetailsToolStripMenuItem});
			this.contextMenuStripSG.Name = "contextMenuStripSG";
			this.contextMenuStripSG.Size = new System.Drawing.Size(167, 48);
			// 
			// toggleColumnsToolStripMenuItem
			// 
			this.toggleColumnsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageToolStripMenuItem,
            this.nameToolStripMenuItem,
            this.classToolStripMenuItem,
            this.distanceToolStripMenuItem,
            this.informationToolStripMenuItem});
			this.toggleColumnsToolStripMenuItem.Name = "toggleColumnsToolStripMenuItem";
			this.toggleColumnsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.toggleColumnsToolStripMenuItem.Text = "Toggle Columns";
			// 
			// imageToolStripMenuItem
			// 
			this.imageToolStripMenuItem.Checked = true;
			this.imageToolStripMenuItem.CheckOnClick = true;
			this.imageToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
			this.imageToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.imageToolStripMenuItem.Text = "Image";
			this.imageToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.imageToolStripMenuItem_CheckStateChanged);
			// 
			// nameToolStripMenuItem
			// 
			this.nameToolStripMenuItem.Checked = true;
			this.nameToolStripMenuItem.CheckOnClick = true;
			this.nameToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
			this.nameToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.nameToolStripMenuItem.Text = "Name";
			this.nameToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.nameToolStripMenuItem_CheckStateChanged);
			// 
			// classToolStripMenuItem
			// 
			this.classToolStripMenuItem.Checked = true;
			this.classToolStripMenuItem.CheckOnClick = true;
			this.classToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.classToolStripMenuItem.Name = "classToolStripMenuItem";
			this.classToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.classToolStripMenuItem.Text = "Class";
			this.classToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.classToolStripMenuItem_CheckStateChanged);
			// 
			// distanceToolStripMenuItem
			// 
			this.distanceToolStripMenuItem.Checked = true;
			this.distanceToolStripMenuItem.CheckOnClick = true;
			this.distanceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.distanceToolStripMenuItem.Name = "distanceToolStripMenuItem";
			this.distanceToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.distanceToolStripMenuItem.Text = "Distance";
			this.distanceToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.distanceToolStripMenuItem_CheckStateChanged);
			// 
			// informationToolStripMenuItem
			// 
			this.informationToolStripMenuItem.Checked = true;
			this.informationToolStripMenuItem.CheckOnClick = true;
			this.informationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.informationToolStripMenuItem.Name = "informationToolStripMenuItem";
			this.informationToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
			this.informationToolStripMenuItem.Text = "Information";
			this.informationToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.informationToolStripMenuItem_CheckStateChanged);
			// 
			// toggleDetailsToolStripMenuItem
			// 
			this.toggleDetailsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.massToolStripMenuItem,
            this.radiusToolStripMenuItem,
            this.circumstellarZonesToolStripMenuItem,
            this.toolStripSeparator1,
            this.atmosphereToolStripMenuItem,
            this.materialsToolStripMenuItem,
            this.toolStripSeparator2,
            this.valueToolStripMenuItem});
			this.toggleDetailsToolStripMenuItem.Name = "toggleDetailsToolStripMenuItem";
			this.toggleDetailsToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
			this.toggleDetailsToolStripMenuItem.Text = "Toggle Details";
			// 
			// circumstellarZonesToolStripMenuItem
			// 
			this.circumstellarZonesToolStripMenuItem.Name = "circumstellarZonesToolStripMenuItem";
			this.circumstellarZonesToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.circumstellarZonesToolStripMenuItem.Text = "Circumstellar Zones";
			// 
			// materialsToolStripMenuItem
			// 
			this.materialsToolStripMenuItem.Name = "materialsToolStripMenuItem";
			this.materialsToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.materialsToolStripMenuItem.Text = "Materials";
			// 
			// atmosphereToolStripMenuItem
			// 
			this.atmosphereToolStripMenuItem.Name = "atmosphereToolStripMenuItem";
			this.atmosphereToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.atmosphereToolStripMenuItem.Text = "Atmosphere";
			// 
			// massToolStripMenuItem
			// 
			this.massToolStripMenuItem.Name = "massToolStripMenuItem";
			this.massToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.massToolStripMenuItem.Text = "Mass";
			// 
			// radiusToolStripMenuItem
			// 
			this.radiusToolStripMenuItem.Name = "radiusToolStripMenuItem";
			this.radiusToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.radiusToolStripMenuItem.Text = "Radius";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(185, 6);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(185, 6);
			// 
			// valueToolStripMenuItem
			// 
			this.valueToolStripMenuItem.Name = "valueToolStripMenuItem";
			this.valueToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
			this.valueToolStripMenuItem.Text = "Value";
			// 
			// UserControlScanGrid
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.dataViewScrollerPanel2);
			this.Name = "UserControlScanGrid";
			this.Size = new System.Drawing.Size(572, 572);
			this.dataViewScrollerPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dataGridViewScanGrid)).EndInit();
			this.contextMenuStripSG.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel2;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom2;
        private System.Windows.Forms.DataGridView dataGridViewScanGrid;
        private System.Windows.Forms.ToolTip toolTip;
		private System.Windows.Forms.ContextMenuStrip contextMenuStripSG;
		private System.Windows.Forms.ToolStripMenuItem toggleColumnsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem classToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem distanceToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem informationToolStripMenuItem;
		private System.Windows.Forms.DataGridViewTextBoxColumn colImg;
		private System.Windows.Forms.DataGridViewTextBoxColumn colName;
		private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
		private System.Windows.Forms.DataGridViewTextBoxColumn colDistance;
		private System.Windows.Forms.DataGridViewTextBoxColumn colInformation;
		private System.Windows.Forms.ToolStripMenuItem toggleDetailsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem massToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem radiusToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem circumstellarZonesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem atmosphereToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem materialsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem valueToolStripMenuItem;
	}
}
