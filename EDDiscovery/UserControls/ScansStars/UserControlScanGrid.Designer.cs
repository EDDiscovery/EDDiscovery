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
            this.vScrollBarCustom2 = new ExtendedControls.VScrollBarCustom();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.circumstellarZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.habitableZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.metalRichToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waterWorldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.earthLikePlanetsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ammoniaWorldsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.icyBodiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.structuresToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.beltsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ringsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.valuesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.columnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.classToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.distanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStripSummary = new ExtendedControls.StatusStripCustom();
            this.toolStripStatusTotalValue = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusGreen = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.colImage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBriefing = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataViewScrollerPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScangrid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.statusStripSummary.SuspendLayout();
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
            this.dataViewScrollerPanel2.Size = new System.Drawing.Size(572, 550);
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
            this.colImage,
            this.colName,
            this.colClass,
            this.colDistance,
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
            this.dataGridViewScangrid.Size = new System.Drawing.Size(552, 550);
            this.dataGridViewScangrid.TabIndex = 23;
            this.dataGridViewScangrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewScangrid_CellDoubleClick);
            this.dataGridViewScangrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewScangrid_RowPostPaint);
            this.dataGridViewScangrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewScangrid_MouseClick);
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
            this.vScrollBarCustom2.Size = new System.Drawing.Size(20, 532);
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
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.circumstellarZoneToolStripMenuItem,
            this.structuresToolStripMenuItem,
            this.materialsToolStripMenuItem,
            this.valuesToolStripMenuItem,
            this.toolStripSeparator1,
            this.columnsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(180, 120);
            // 
            // circumstellarZoneToolStripMenuItem
            // 
            this.circumstellarZoneToolStripMenuItem.Checked = true;
            this.circumstellarZoneToolStripMenuItem.CheckOnClick = true;
            this.circumstellarZoneToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.circumstellarZoneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.habitableZoneToolStripMenuItem,
            this.metalRichToolStripMenuItem,
            this.waterWorldsToolStripMenuItem,
            this.earthLikePlanetsToolStripMenuItem,
            this.ammoniaWorldsToolStripMenuItem,
            this.icyBodiesToolStripMenuItem});
            this.circumstellarZoneToolStripMenuItem.Name = "circumstellarZoneToolStripMenuItem";
            this.circumstellarZoneToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.circumstellarZoneToolStripMenuItem.Text = "Circumstellar Zones";
            this.circumstellarZoneToolStripMenuItem.ToolTipText = "Toggle visibility for all circumstellar zones";
            this.circumstellarZoneToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.circumstellarZoneToolStripMenuItem_CheckStateChanged);
            // 
            // habitableZoneToolStripMenuItem
            // 
            this.habitableZoneToolStripMenuItem.Checked = true;
            this.habitableZoneToolStripMenuItem.CheckOnClick = true;
            this.habitableZoneToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.habitableZoneToolStripMenuItem.Name = "habitableZoneToolStripMenuItem";
            this.habitableZoneToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.habitableZoneToolStripMenuItem.Text = "Habitable Zone";
            this.habitableZoneToolStripMenuItem.ToolTipText = "Toggle habitable zone visibility";
            this.habitableZoneToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.habitableZoneToolStripMenuItem_CheckStateChanged);
            // 
            // metalRichToolStripMenuItem
            // 
            this.metalRichToolStripMenuItem.Checked = true;
            this.metalRichToolStripMenuItem.CheckOnClick = true;
            this.metalRichToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.metalRichToolStripMenuItem.Name = "metalRichToolStripMenuItem";
            this.metalRichToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.metalRichToolStripMenuItem.Text = "Metal Rich";
            this.metalRichToolStripMenuItem.ToolTipText = "Toggle metal rich planets visibility";
            this.metalRichToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.metallicRichToolStripMenuItem_CheckStateChanged);
            // 
            // waterWorldsToolStripMenuItem
            // 
            this.waterWorldsToolStripMenuItem.Checked = true;
            this.waterWorldsToolStripMenuItem.CheckOnClick = true;
            this.waterWorldsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.waterWorldsToolStripMenuItem.Name = "waterWorldsToolStripMenuItem";
            this.waterWorldsToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.waterWorldsToolStripMenuItem.Text = "Water Worlds";
            this.waterWorldsToolStripMenuItem.ToolTipText = "Toggle water worlds visibility visibility";
            this.waterWorldsToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.waterWorldsToolStripMenuItem_CheckStateChanged);
            // 
            // earthLikePlanetsToolStripMenuItem
            // 
            this.earthLikePlanetsToolStripMenuItem.Checked = true;
            this.earthLikePlanetsToolStripMenuItem.CheckOnClick = true;
            this.earthLikePlanetsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.earthLikePlanetsToolStripMenuItem.Name = "earthLikePlanetsToolStripMenuItem";
            this.earthLikePlanetsToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.earthLikePlanetsToolStripMenuItem.Text = "Earth Like Planets";
            this.earthLikePlanetsToolStripMenuItem.ToolTipText = "Toggle earth like planets visibility";
            this.earthLikePlanetsToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.earthLikePlanetsToolStripMenuItem_CheckStateChanged);
            // 
            // ammoniaWorldsToolStripMenuItem
            // 
            this.ammoniaWorldsToolStripMenuItem.Checked = true;
            this.ammoniaWorldsToolStripMenuItem.CheckOnClick = true;
            this.ammoniaWorldsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ammoniaWorldsToolStripMenuItem.Name = "ammoniaWorldsToolStripMenuItem";
            this.ammoniaWorldsToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.ammoniaWorldsToolStripMenuItem.Text = "Ammonia Worlds";
            this.ammoniaWorldsToolStripMenuItem.ToolTipText = "Toggle ammonia worlds visibility";
            this.ammoniaWorldsToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.ammoniaWorldsToolStripMenuItem_CheckStateChanged);
            // 
            // icyBodiesToolStripMenuItem
            // 
            this.icyBodiesToolStripMenuItem.Checked = true;
            this.icyBodiesToolStripMenuItem.CheckOnClick = true;
            this.icyBodiesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.icyBodiesToolStripMenuItem.Name = "icyBodiesToolStripMenuItem";
            this.icyBodiesToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.icyBodiesToolStripMenuItem.Text = "Icy Bodies";
            this.icyBodiesToolStripMenuItem.ToolTipText = "Toggle ice bodies zone visibility";
            this.icyBodiesToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.icyBodiesToolStripMenuItem_CheckStateChanged);
            // 
            // structuresToolStripMenuItem
            // 
            this.structuresToolStripMenuItem.Checked = true;
            this.structuresToolStripMenuItem.CheckOnClick = true;
            this.structuresToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.structuresToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.beltsToolStripMenuItem,
            this.ringsToolStripMenuItem});
            this.structuresToolStripMenuItem.Name = "structuresToolStripMenuItem";
            this.structuresToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.structuresToolStripMenuItem.Text = "Structures";
            this.structuresToolStripMenuItem.ToolTipText = "Toggle belts and rings visibility";
            this.structuresToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.structuresToolStripMenuItem_CheckStateChanged);
            // 
            // beltsToolStripMenuItem
            // 
            this.beltsToolStripMenuItem.Checked = true;
            this.beltsToolStripMenuItem.CheckOnClick = true;
            this.beltsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.beltsToolStripMenuItem.Name = "beltsToolStripMenuItem";
            this.beltsToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.beltsToolStripMenuItem.Text = "Belts";
            this.beltsToolStripMenuItem.ToolTipText = "Toggle belts visibility";
            this.beltsToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.beltsToolStripMenuItem_CheckStateChanged);
            // 
            // ringsToolStripMenuItem
            // 
            this.ringsToolStripMenuItem.Checked = true;
            this.ringsToolStripMenuItem.CheckOnClick = true;
            this.ringsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ringsToolStripMenuItem.Name = "ringsToolStripMenuItem";
            this.ringsToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.ringsToolStripMenuItem.Text = "Rings";
            this.ringsToolStripMenuItem.ToolTipText = "Toggle rings visibility";
            this.ringsToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.ringsToolStripMenuItem_CheckStateChanged);
            // 
            // materialsToolStripMenuItem
            // 
            this.materialsToolStripMenuItem.Checked = true;
            this.materialsToolStripMenuItem.CheckOnClick = true;
            this.materialsToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.materialsToolStripMenuItem.Name = "materialsToolStripMenuItem";
            this.materialsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.materialsToolStripMenuItem.Text = "Materials";
            this.materialsToolStripMenuItem.ToolTipText = "Toggle visibility of materials available in the surface ofa landable body";
            this.materialsToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.materialsToolStripMenuItem_CheckStateChanged);
            // 
            // valuesToolStripMenuItem
            // 
            this.valuesToolStripMenuItem.Checked = true;
            this.valuesToolStripMenuItem.CheckOnClick = true;
            this.valuesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.valuesToolStripMenuItem.Name = "valuesToolStripMenuItem";
            this.valuesToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.valuesToolStripMenuItem.Text = "Values";
            this.valuesToolStripMenuItem.ToolTipText = "Toggle display of estimated value";
            this.valuesToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.valuesToolStripMenuItem_CheckStateChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(176, 6);
            // 
            // columnsToolStripMenuItem
            // 
            this.columnsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nameToolStripMenuItem,
            this.classToolStripMenuItem,
            this.distanceToolStripMenuItem,
            this.informationToolStripMenuItem});
            this.columnsToolStripMenuItem.Name = "columnsToolStripMenuItem";
            this.columnsToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.columnsToolStripMenuItem.Text = "Columns";
            this.columnsToolStripMenuItem.ToolTipText = "Toggle columns visibility";
            // 
            // nameToolStripMenuItem
            // 
            this.nameToolStripMenuItem.Checked = true;
            this.nameToolStripMenuItem.CheckOnClick = true;
            this.nameToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.nameToolStripMenuItem.Name = "nameToolStripMenuItem";
            this.nameToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.nameToolStripMenuItem.Text = "Name";
            this.nameToolStripMenuItem.ToolTipText = "Toggle body name column visibility";
            this.nameToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.nameToolStripMenuItem_CheckStateChanged);
            // 
            // classToolStripMenuItem
            // 
            this.classToolStripMenuItem.Checked = true;
            this.classToolStripMenuItem.CheckOnClick = true;
            this.classToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.classToolStripMenuItem.Name = "classToolStripMenuItem";
            this.classToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.classToolStripMenuItem.Text = "Class";
            this.classToolStripMenuItem.ToolTipText = "Toggle body class column visibility";
            this.classToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.classToolStripMenuItem_CheckStateChanged);
            // 
            // distanceToolStripMenuItem
            // 
            this.distanceToolStripMenuItem.Checked = true;
            this.distanceToolStripMenuItem.CheckOnClick = true;
            this.distanceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.distanceToolStripMenuItem.Name = "distanceToolStripMenuItem";
            this.distanceToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.distanceToolStripMenuItem.Text = "Distance";
            this.distanceToolStripMenuItem.ToolTipText = "Toggle relative body distance column visibility";
            this.distanceToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.distanceToolStripMenuItem_CheckStateChanged);
            // 
            // informationToolStripMenuItem
            // 
            this.informationToolStripMenuItem.Checked = true;
            this.informationToolStripMenuItem.CheckOnClick = true;
            this.informationToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.informationToolStripMenuItem.Name = "informationToolStripMenuItem";
            this.informationToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.informationToolStripMenuItem.Text = "Information";
            this.informationToolStripMenuItem.ToolTipText = "Toggle body information column visibility";
            this.informationToolStripMenuItem.CheckStateChanged += new System.EventHandler(this.informationToolStripMenuItem_CheckStateChanged);
            // 
            // statusStripSummary
            // 
            this.statusStripSummary.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusTotalValue,
            this.toolStripStatusGreen,
            this.toolStripProgressBar});
            this.statusStripSummary.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStripSummary.Location = new System.Drawing.Point(0, 550);
            this.statusStripSummary.Name = "statusStripSummary";
            this.statusStripSummary.Size = new System.Drawing.Size(572, 22);
            this.statusStripSummary.SizingGrip = false;
            this.statusStripSummary.TabIndex = 27;
            // 
            // toolStripStatusTotalValue
            // 
            this.toolStripStatusTotalValue.Name = "toolStripStatusTotalValue";
            this.toolStripStatusTotalValue.Size = new System.Drawing.Size(122, 17);
            this.toolStripStatusTotalValue.Text = "Estimated scans value";
            // 
            // toolStripStatusGreen
            // 
            this.toolStripStatusGreen.BackColor = System.Drawing.Color.Green;
            this.toolStripStatusGreen.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripStatusGreen.ForeColor = System.Drawing.Color.White;
            this.toolStripStatusGreen.Margin = new System.Windows.Forms.Padding(20, 3, 0, 3);
            this.toolStripStatusGreen.Name = "toolStripStatusGreen";
            this.toolStripStatusGreen.Padding = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.toolStripStatusGreen.Size = new System.Drawing.Size(116, 17);
            this.toolStripStatusGreen.Text = "GREEN SYSTEM";
            this.toolStripStatusGreen.Visible = false;
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Maximum = 8;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 17);
            this.toolStripProgressBar.Visible = false;
            // 
            // colImage
            // 
            this.colImage.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.colImage.FillWeight = 1F;
            this.colImage.HeaderText = "";
            this.colImage.MinimumWidth = 55;
            this.colImage.Name = "colImage";
            this.colImage.ReadOnly = true;
            this.colImage.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colImage.Width = 55;
            // 
            // colName
            // 
            this.colName.FillWeight = 42.49234F;
            this.colName.HeaderText = "Name";
            this.colName.MinimumWidth = 20;
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            this.colName.ToolTipText = "Body name";
            // 
            // colClass
            // 
            this.colClass.FillWeight = 53.11543F;
            this.colClass.HeaderText = "Class";
            this.colClass.MinimumWidth = 20;
            this.colClass.Name = "colClass";
            this.colClass.ReadOnly = true;
            this.colClass.ToolTipText = "Body class";
            // 
            // colDistance
            // 
            this.colDistance.FillWeight = 26.4625F;
            this.colDistance.HeaderText = "Distance";
            this.colDistance.Name = "colDistance";
            this.colDistance.ReadOnly = true;
            this.colDistance.ToolTipText = "Body relative distance";
            // 
            // colBriefing
            // 
            this.colBriefing.FillWeight = 106.2309F;
            this.colBriefing.HeaderText = "Information";
            this.colBriefing.MinimumWidth = 20;
            this.colBriefing.Name = "colBriefing";
            this.colBriefing.ReadOnly = true;
            this.colBriefing.ToolTipText = "Body detailed information";
            // 
            // UserControlScanGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel2);
            this.Controls.Add(this.statusStripSummary);
            this.Name = "UserControlScanGrid";
            this.Size = new System.Drawing.Size(572, 572);
            this.dataViewScrollerPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewScangrid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.statusStripSummary.ResumeLayout(false);
            this.statusStripSummary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel2;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom2;
        private System.Windows.Forms.DataGridView dataGridViewScangrid;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem circumstellarZoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem metalRichToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waterWorldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem earthLikePlanetsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ammoniaWorldsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem icyBodiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem materialsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem valuesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem structuresToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem beltsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ringsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem habitableZoneToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem columnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem classToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem distanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informationToolStripMenuItem;
        private ExtendedControls.StatusStripCustom statusStripSummary;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusTotalValue;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusGreen;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClass;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBriefing;
    }
}
