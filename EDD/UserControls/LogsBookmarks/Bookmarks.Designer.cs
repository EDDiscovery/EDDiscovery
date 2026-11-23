namespace EDDiscovery.UserControls
{
    partial class Bookmarks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Bookmarks));
            this.contextMenuStripBookmarks = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewScanOfSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnSpanshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToExpeditionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxFilter = new ExtendedControls.ExtTextBox();
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.buttonNew = new ExtendedControls.ExtButton();
            this.extButtonEditSystem = new ExtendedControls.ExtButton();
            this.extButtonNewRegion = new ExtendedControls.ExtButton();
            this.buttonEdit = new ExtendedControls.ExtButton();
            this.buttonDelete = new ExtendedControls.ExtButton();
            this.buttonTags = new ExtendedControls.ExtButton();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.buttonExtImport = new ExtendedControls.ExtButton();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.ColType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBookmarkName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPlanetMarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlSurfaceBookmarks = new EDDiscovery.UserControls.SurfaceBookmarkUserControl();
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.labelSearch = new System.Windows.Forms.Label();
            this.contextMenuStripBookmarks.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripBookmarks
            // 
            this.contextMenuStripBookmarks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewScanOfSystemToolStripMenuItem,
            this.mapGotoStartoolStripMenuItem,
            this.viewOnSpanshToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.addToExpeditionToolStripMenuItem});
            this.contextMenuStripBookmarks.Name = "contextMenuStripBookmarks";
            this.contextMenuStripBookmarks.Size = new System.Drawing.Size(187, 114);
            this.contextMenuStripBookmarks.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripBookmarks_Opening);
            // 
            // viewScanOfSystemToolStripMenuItem
            // 
            this.viewScanOfSystemToolStripMenuItem.Name = "viewScanOfSystemToolStripMenuItem";
            this.viewScanOfSystemToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewScanOfSystemToolStripMenuItem.Text = "View Scan Display";
            this.viewScanOfSystemToolStripMenuItem.Click += new System.EventHandler(this.viewScanOfSystemToolStripMenuItem_Click);
            // 
            // mapGotoStartoolStripMenuItem
            // 
            this.mapGotoStartoolStripMenuItem.Name = "mapGotoStartoolStripMenuItem";
            this.mapGotoStartoolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.mapGotoStartoolStripMenuItem.Text = "Go to star on 3D Map";
            this.mapGotoStartoolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemGotoStar3dmap_Click);
            // 
            // viewOnSpanshToolStripMenuItem
            // 
            this.viewOnSpanshToolStripMenuItem.Name = "viewOnSpanshToolStripMenuItem";
            this.viewOnSpanshToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewOnSpanshToolStripMenuItem.Text = "View on Spansh";
            this.viewOnSpanshToolStripMenuItem.Click += new System.EventHandler(this.viewOnSpanshToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.openInEDSMToolStripMenuItem_Click);
            // 
            // addToExpeditionToolStripMenuItem
            // 
            this.addToExpeditionToolStripMenuItem.Name = "addToExpeditionToolStripMenuItem";
            this.addToExpeditionToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.addToExpeditionToolStripMenuItem.Text = "Add to Expedition";
            this.addToExpeditionToolStripMenuItem.Click += new System.EventHandler(this.addToExpeditionToolStripMenuItem_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxFilter.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFilter.BorderColor2 = System.Drawing.Color.Transparent;
            this.textBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFilter.ClearOnFirstChar = false;
            this.textBoxFilter.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFilter.EndButtonEnable = true;
            this.textBoxFilter.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxFilter.EndButtonImage")));
            this.textBoxFilter.EndButtonSize16ths = 10;
            this.textBoxFilter.EndButtonVisible = false;
            this.textBoxFilter.InErrorCondition = false;
            this.textBoxFilter.Location = new System.Drawing.Point(50, 4);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.textBoxFilter.Multiline = false;
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.ReadOnly = false;
            this.textBoxFilter.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFilter.SelectionLength = 0;
            this.textBoxFilter.SelectionStart = 0;
            this.textBoxFilter.Size = new System.Drawing.Size(148, 20);
            this.textBoxFilter.TabIndex = 1;
            this.textBoxFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxFilter.TextNoChange = "";
            this.toolTip.SetToolTip(this.textBoxFilter, "Search for Bookmark");
            this.textBoxFilter.WordWrap = true;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // buttonFilter
            // 
            this.buttonFilter.BackColor2 = System.Drawing.Color.Red;
            this.buttonFilter.ButtonDisabledScaling = 0.5F;
            this.buttonFilter.GradientDirection = 90F;
            this.buttonFilter.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.buttonFilter.Location = new System.Drawing.Point(204, 1);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonFilter.MouseOverScaling = 1.3F;
            this.buttonFilter.MouseSelectedScaling = 1.3F;
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(28, 28);
            this.buttonFilter.TabIndex = 40;
            this.toolTip.SetToolTip(this.buttonFilter, "Filter out entries based on tag");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonEditTagsList_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.BackColor2 = System.Drawing.Color.Red;
            this.buttonNew.ButtonDisabledScaling = 0.5F;
            this.buttonNew.GradientDirection = 90F;
            this.buttonNew.Image = global::EDDiscovery.Icons.Controls.New;
            this.buttonNew.Location = new System.Drawing.Point(238, 1);
            this.buttonNew.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonNew.MouseOverScaling = 1.3F;
            this.buttonNew.MouseSelectedScaling = 1.3F;
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(28, 28);
            this.buttonNew.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonNew, "New Bookmark");
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // extButtonEditSystem
            // 
            this.extButtonEditSystem.BackColor2 = System.Drawing.Color.Red;
            this.extButtonEditSystem.ButtonDisabledScaling = 0.5F;
            this.extButtonEditSystem.GradientDirection = 90F;
            this.extButtonEditSystem.Image = global::EDDiscovery.Icons.Controls.EditSystem;
            this.extButtonEditSystem.Location = new System.Drawing.Point(272, 1);
            this.extButtonEditSystem.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonEditSystem.MouseOverScaling = 1.3F;
            this.extButtonEditSystem.MouseSelectedScaling = 1.3F;
            this.extButtonEditSystem.Name = "extButtonEditSystem";
            this.extButtonEditSystem.Size = new System.Drawing.Size(28, 28);
            this.extButtonEditSystem.TabIndex = 3;
            this.toolTip.SetToolTip(this.extButtonEditSystem, "New bookmark on current system");
            this.extButtonEditSystem.UseVisualStyleBackColor = true;
            this.extButtonEditSystem.Click += new System.EventHandler(this.extButtonEditSystem_Click);
            // 
            // extButtonNewRegion
            // 
            this.extButtonNewRegion.BackColor2 = System.Drawing.Color.Red;
            this.extButtonNewRegion.ButtonDisabledScaling = 0.5F;
            this.extButtonNewRegion.GradientDirection = 90F;
            this.extButtonNewRegion.Image = global::EDDiscovery.Icons.Controls.NewRegion;
            this.extButtonNewRegion.Location = new System.Drawing.Point(306, 1);
            this.extButtonNewRegion.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonNewRegion.MouseOverScaling = 1.3F;
            this.extButtonNewRegion.MouseSelectedScaling = 1.3F;
            this.extButtonNewRegion.Name = "extButtonNewRegion";
            this.extButtonNewRegion.Size = new System.Drawing.Size(28, 28);
            this.extButtonNewRegion.TabIndex = 2;
            this.toolTip.SetToolTip(this.extButtonNewRegion, "New Region Bookmark");
            this.extButtonNewRegion.UseVisualStyleBackColor = true;
            this.extButtonNewRegion.Click += new System.EventHandler(this.extButtonNewRegion_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.BackColor2 = System.Drawing.Color.Red;
            this.buttonEdit.ButtonDisabledScaling = 0.5F;
            this.buttonEdit.GradientDirection = 90F;
            this.buttonEdit.Image = global::EDDiscovery.Icons.Controls.Edit;
            this.buttonEdit.Location = new System.Drawing.Point(340, 1);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonEdit.MouseOverScaling = 1.3F;
            this.buttonEdit.MouseSelectedScaling = 1.3F;
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(28, 28);
            this.buttonEdit.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonEdit, "Edit Selected Bookmark");
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEditBookmark_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.BackColor2 = System.Drawing.Color.Red;
            this.buttonDelete.ButtonDisabledScaling = 0.5F;
            this.buttonDelete.GradientDirection = 90F;
            this.buttonDelete.Image = global::EDDiscovery.Icons.Controls.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(374, 1);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonDelete.MouseOverScaling = 1.3F;
            this.buttonDelete.MouseSelectedScaling = 1.3F;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(28, 28);
            this.buttonDelete.TabIndex = 27;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete selected bookmark");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonTags
            // 
            this.buttonTags.BackColor2 = System.Drawing.Color.Red;
            this.buttonTags.ButtonDisabledScaling = 0.5F;
            this.buttonTags.GradientDirection = 90F;
            this.buttonTags.Image = global::EDDiscovery.Icons.Controls.Tags;
            this.buttonTags.Location = new System.Drawing.Point(408, 1);
            this.buttonTags.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonTags.MouseOverScaling = 1.3F;
            this.buttonTags.MouseSelectedScaling = 1.3F;
            this.buttonTags.Name = "buttonTags";
            this.buttonTags.Size = new System.Drawing.Size(28, 28);
            this.buttonTags.TabIndex = 39;
            this.toolTip.SetToolTip(this.buttonTags, "Edit Tags");
            this.buttonTags.UseVisualStyleBackColor = true;
            this.buttonTags.Click += new System.EventHandler(this.buttonTags_Click);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtExcel.ButtonDisabledScaling = 0.5F;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.GradientDirection = 90F;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(442, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtExcel.MouseOverScaling = 1.3F;
            this.buttonExtExcel.MouseSelectedScaling = 1.3F;
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 38;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Export bookmarks to CSV file");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // buttonExtImport
            // 
            this.buttonExtImport.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtImport.ButtonDisabledScaling = 0.5F;
            this.buttonExtImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtImport.GradientDirection = 90F;
            this.buttonExtImport.Image = global::EDDiscovery.Icons.Controls.ImportExcel;
            this.buttonExtImport.Location = new System.Drawing.Point(476, 1);
            this.buttonExtImport.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtImport.MouseOverScaling = 1.3F;
            this.buttonExtImport.MouseSelectedScaling = 1.3F;
            this.buttonExtImport.Name = "buttonExtImport";
            this.buttonExtImport.Size = new System.Drawing.Size(28, 28);
            this.buttonExtImport.TabIndex = 38;
            this.toolTip.SetToolTip(this.buttonExtImport, "Import bookmarks to EDD from CSV file");
            this.buttonExtImport.UseVisualStyleBackColor = true;
            this.buttonExtImport.Click += new System.EventHandler(this.buttonExtImport_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 30);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dataViewScrollerPanel);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.userControlSurfaceBookmarks);
            this.splitContainer.Size = new System.Drawing.Size(676, 413);
            this.splitContainer.SplitterDistance = 204;
            this.splitContainer.TabIndex = 1;
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 24;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(676, 204);
            this.dataViewScrollerPanel.TabIndex = 5;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
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
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(652, 0);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(24, 204);
            this.vScrollBarCustom1.SkinnyStyle = ExtendedControls.ExtScrollBar.ScrollStyle.Normal;
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SliderDrawAngle = 90F;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 6;
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowRowHeaderVisibleSelection = false;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView.AutoSortByColumnName = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnReorder = true;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColType,
            this.ColBookmarkName,
            this.ColDescription,
            this.ColX,
            this.ColY,
            this.ColZ,
            this.ColTags,
            this.ColPlanetMarks});
            this.dataGridView.ContextMenuStrip = this.contextMenuStripBookmarks;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.PerColumnWordWrapControl = true;
            this.dataGridView.RowHeaderMenuStrip = null;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(652, 204);
            this.dataGridView.TabIndex = 4;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBookMarks_CellClick);
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBookMarks_CellDoubleClick);
            this.dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBookMarks_CellEndEdit);
            this.dataGridView.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridViewBookMarks_ColumnWidthChanged);
            this.dataGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewBookMarks_RowPostPaint);
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewBookMarks_SortCompare);
            this.dataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewBookMarks_MouseDown);
            // 
            // ColType
            // 
            this.ColType.HeaderText = "Bookmark Type";
            this.ColType.Name = "ColType";
            this.ColType.ReadOnly = true;
            // 
            // ColBookmarkName
            // 
            this.ColBookmarkName.FillWeight = 200F;
            this.ColBookmarkName.HeaderText = "Bookmark Name";
            this.ColBookmarkName.Name = "ColBookmarkName";
            this.ColBookmarkName.ReadOnly = true;
            // 
            // ColDescription
            // 
            this.ColDescription.FillWeight = 200F;
            this.ColDescription.HeaderText = "Description";
            this.ColDescription.Name = "ColDescription";
            // 
            // ColX
            // 
            this.ColX.HeaderText = "X";
            this.ColX.Name = "ColX";
            this.ColX.ReadOnly = true;
            // 
            // ColY
            // 
            this.ColY.HeaderText = "Y";
            this.ColY.Name = "ColY";
            this.ColY.ReadOnly = true;
            // 
            // ColZ
            // 
            this.ColZ.HeaderText = "Z";
            this.ColZ.Name = "ColZ";
            this.ColZ.ReadOnly = true;
            // 
            // ColTags
            // 
            this.ColTags.FillWeight = 200F;
            this.ColTags.HeaderText = "Tags";
            this.ColTags.MinimumWidth = 32;
            this.ColTags.Name = "ColTags";
            this.ColTags.ReadOnly = true;
            // 
            // ColPlanetMarks
            // 
            this.ColPlanetMarks.HeaderText = "Planet Marks";
            this.ColPlanetMarks.Name = "ColPlanetMarks";
            this.ColPlanetMarks.ReadOnly = true;
            // 
            // userControlSurfaceBookmarks
            // 
            this.userControlSurfaceBookmarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSurfaceBookmarks.Location = new System.Drawing.Point(0, 0);
            this.userControlSurfaceBookmarks.Name = "userControlSurfaceBookmarks";
            this.userControlSurfaceBookmarks.Size = new System.Drawing.Size(676, 205);
            this.userControlSurfaceBookmarks.TabIndex = 5;
            this.userControlSurfaceBookmarks.TagFilter = null;
            // 
            // topPanel
            // 
            this.topPanel.AutoSize = true;
            this.topPanel.Controls.Add(this.labelSearch);
            this.topPanel.Controls.Add(this.textBoxFilter);
            this.topPanel.Controls.Add(this.buttonFilter);
            this.topPanel.Controls.Add(this.buttonNew);
            this.topPanel.Controls.Add(this.extButtonEditSystem);
            this.topPanel.Controls.Add(this.extButtonNewRegion);
            this.topPanel.Controls.Add(this.buttonEdit);
            this.topPanel.Controls.Add(this.buttonDelete);
            this.topPanel.Controls.Add(this.buttonTags);
            this.topPanel.Controls.Add(this.buttonExtExcel);
            this.topPanel.Controls.Add(this.buttonExtImport);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(676, 30);
            this.topPanel.TabIndex = 39;
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(3, 4);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 26;
            this.labelSearch.Text = "Search";
            // 
            // UserControlBookmarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.topPanel);
            this.Name = "UserControlBookmarks";
            this.Size = new System.Drawing.Size(676, 443);
            this.contextMenuStripBookmarks.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtTextBox textBoxFilter;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.SplitContainer splitContainer;
        private BaseUtils.DataGridViewColumnControl dataGridView;
        private SurfaceBookmarkUserControl userControlSurfaceBookmarks;
        private ExtendedControls.ExtButton buttonNew;
        private ExtendedControls.ExtButton buttonEdit;
        private ExtendedControls.ExtButton buttonDelete;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBookmarks;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtButton buttonExtImport;
        private System.Windows.Forms.FlowLayoutPanel topPanel;
        private ExtendedControls.ExtButton extButtonEditSystem;
        private ExtendedControls.ExtButton extButtonNewRegion;
        private ExtendedControls.ExtButton buttonTags;
        private ExtendedControls.ExtButton buttonFilter;
        private System.Windows.Forms.ToolStripMenuItem viewScanOfSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnSpanshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToExpeditionToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBookmarkName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTags;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPlanetMarks;
    }
}
