namespace EDDiscovery.UserControls
{
    partial class UserControlBookmarks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlBookmarks));
            this.buttonExtImport = new ExtendedControls.ExtButton();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.buttonDelete = new ExtendedControls.ExtButton();
            this.buttonNew = new ExtendedControls.ExtButton();
            this.buttonEdit = new ExtendedControls.ExtButton();
            this.textBoxFilter = new ExtendedControls.ExtTextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.dataGridViewBookMarks = new BaseUtils.DataGridViewColumnControl();
            this.ColType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBookmarkName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStripBookmarks = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemGotoStar3dmap = new System.Windows.Forms.ToolStripMenuItem();
            this.openInEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userControlSurfaceBookmarks = new EDDiscovery.Forms.SurfaceBookmarkUserControl();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extButtonEditSystem = new ExtendedControls.ExtButton();
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonNewRegion = new ExtendedControls.ExtButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBookMarks)).BeginInit();
            this.contextMenuStripBookmarks.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonExtImport
            // 
            this.buttonExtImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtImport.Image = global::EDDiscovery.Icons.Controls.ImportExcel;
            this.buttonExtImport.Location = new System.Drawing.Point(421, 1);
            this.buttonExtImport.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonExtImport.Name = "buttonExtImport";
            this.buttonExtImport.Size = new System.Drawing.Size(28, 28);
            this.buttonExtImport.TabIndex = 38;
            this.toolTip.SetToolTip(this.buttonExtImport, "Import bookmarks to EDD from CSV file");
            this.buttonExtImport.UseVisualStyleBackColor = true;
            this.buttonExtImport.Click += new System.EventHandler(this.buttonExtImport_Click);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(385, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 38;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Export bookmarks to CSV file");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Image = global::EDDiscovery.Icons.Controls.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(349, 1);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(28, 28);
            this.buttonDelete.TabIndex = 27;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete selected bookmark");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Image = global::EDDiscovery.Icons.Controls.New;
            this.buttonNew.Location = new System.Drawing.Point(205, 1);
            this.buttonNew.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(28, 28);
            this.buttonNew.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonNew, "New Bookmark");
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Image = global::EDDiscovery.Icons.Controls.Edit;
            this.buttonEdit.Location = new System.Drawing.Point(277, 1);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(28, 28);
            this.buttonEdit.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonEdit, "Edit Selected Bookmark");
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFilter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFilter.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxFilter.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFilter.BorderColorScaling = 0.5F;
            this.textBoxFilter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFilter.ClearOnFirstChar = false;
            this.textBoxFilter.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFilter.EndButtonEnable = true;
            this.textBoxFilter.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxFilter.EndButtonImage")));
            this.textBoxFilter.EndButtonVisible = false;
            this.textBoxFilter.InErrorCondition = false;
            this.textBoxFilter.Location = new System.Drawing.Point(49, 1);
            this.textBoxFilter.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.textBoxFilter.Multiline = false;
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.ReadOnly = false;
            this.textBoxFilter.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFilter.SelectionLength = 0;
            this.textBoxFilter.SelectionStart = 0;
            this.textBoxFilter.Size = new System.Drawing.Size(148, 20);
            this.textBoxFilter.TabIndex = 1;
            this.textBoxFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxFilter, "Search for Bookmark");
            this.textBoxFilter.WordWrap = true;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(0, 1);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 26;
            this.labelSearch.Text = "Search";
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
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewBookMarks);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(676, 204);
            this.dataViewScrollerPanel.TabIndex = 5;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
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
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(660, 0);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(16, 204);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 6;
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // dataGridViewBookMarks
            // 
            this.dataGridViewBookMarks.AllowUserToAddRows = false;
            this.dataGridViewBookMarks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBookMarks.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewBookMarks.AutoSortByColumnName = false;
            this.dataGridViewBookMarks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBookMarks.ColumnReorder = true;
            this.dataGridViewBookMarks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColType,
            this.ColBookmarkName,
            this.ColDescription,
            this.ColX,
            this.ColY,
            this.ColZ});
            this.dataGridViewBookMarks.ContextMenuStrip = this.contextMenuStripBookmarks;
            this.dataGridViewBookMarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBookMarks.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewBookMarks.Name = "dataGridViewBookMarks";
            this.dataGridViewBookMarks.PerColumnWordWrapControl = true;
            this.dataGridViewBookMarks.RowHeaderMenuStrip = null;
            this.dataGridViewBookMarks.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewBookMarks.SingleRowSelect = true;
            this.dataGridViewBookMarks.Size = new System.Drawing.Size(660, 204);
            this.dataGridViewBookMarks.TabIndex = 4;
            this.dataGridViewBookMarks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBookMarks_CellDoubleClick);
            this.dataGridViewBookMarks.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBookMarks_CellEndEdit);
            this.dataGridViewBookMarks.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewBookMarks_SortCompare);
            this.dataGridViewBookMarks.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewBookMarks_MouseDown);
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
            // contextMenuStripBookmarks
            // 
            this.contextMenuStripBookmarks.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemGotoStar3dmap,
            this.openInEDSMToolStripMenuItem});
            this.contextMenuStripBookmarks.Name = "contextMenuStripBookmarks";
            this.contextMenuStripBookmarks.Size = new System.Drawing.Size(158, 48);
            this.contextMenuStripBookmarks.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripBookmarks_Opening);
            // 
            // toolStripMenuItemGotoStar3dmap
            // 
            this.toolStripMenuItemGotoStar3dmap.Name = "toolStripMenuItemGotoStar3dmap";
            this.toolStripMenuItemGotoStar3dmap.Size = new System.Drawing.Size(157, 22);
            this.toolStripMenuItemGotoStar3dmap.Text = "Goto in 3D Map";
            this.toolStripMenuItemGotoStar3dmap.Click += new System.EventHandler(this.toolStripMenuItemGotoStar3dmap_Click);
            // 
            // openInEDSMToolStripMenuItem
            // 
            this.openInEDSMToolStripMenuItem.Name = "openInEDSMToolStripMenuItem";
            this.openInEDSMToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.openInEDSMToolStripMenuItem.Text = "Open in EDSM";
            this.openInEDSMToolStripMenuItem.Click += new System.EventHandler(this.openInEDSMToolStripMenuItem_Click);
            // 
            // userControlSurfaceBookmarks
            // 
            this.userControlSurfaceBookmarks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlSurfaceBookmarks.Location = new System.Drawing.Point(0, 0);
            this.userControlSurfaceBookmarks.Name = "userControlSurfaceBookmarks";
            this.userControlSurfaceBookmarks.Size = new System.Drawing.Size(676, 205);
            this.userControlSurfaceBookmarks.TabIndex = 5;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // extButtonEditSystem
            // 
            this.extButtonEditSystem.Image = global::EDDiscovery.Icons.Controls.EditSystem;
            this.extButtonEditSystem.Location = new System.Drawing.Point(313, 1);
            this.extButtonEditSystem.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extButtonEditSystem.Name = "extButtonEditSystem";
            this.extButtonEditSystem.Size = new System.Drawing.Size(28, 28);
            this.extButtonEditSystem.TabIndex = 3;
            this.toolTip.SetToolTip(this.extButtonEditSystem, "Edit Bookmark on current system");
            this.extButtonEditSystem.UseVisualStyleBackColor = true;
            this.extButtonEditSystem.Click += new System.EventHandler(this.extButtonEditSystem_Click);
            // 
            // topPanel
            // 
            this.topPanel.AutoSize = true;
            this.topPanel.Controls.Add(this.labelSearch);
            this.topPanel.Controls.Add(this.textBoxFilter);
            this.topPanel.Controls.Add(this.buttonNew);
            this.topPanel.Controls.Add(this.extButtonNewRegion);
            this.topPanel.Controls.Add(this.buttonEdit);
            this.topPanel.Controls.Add(this.extButtonEditSystem);
            this.topPanel.Controls.Add(this.buttonDelete);
            this.topPanel.Controls.Add(this.buttonExtExcel);
            this.topPanel.Controls.Add(this.buttonExtImport);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(676, 30);
            this.topPanel.TabIndex = 39;
            // 
            // extButtonNewRegion
            // 
            this.extButtonNewRegion.Image = global::EDDiscovery.Icons.Controls.NewRegion;
            this.extButtonNewRegion.Location = new System.Drawing.Point(241, 1);
            this.extButtonNewRegion.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extButtonNewRegion.Name = "extButtonNewRegion";
            this.extButtonNewRegion.Size = new System.Drawing.Size(28, 28);
            this.extButtonNewRegion.TabIndex = 2;
            this.toolTip.SetToolTip(this.extButtonNewRegion, "New Region Bookmark");
            this.extButtonNewRegion.UseVisualStyleBackColor = true;
            this.extButtonNewRegion.Click += new System.EventHandler(this.extButtonNewRegion_Click);
            // 
            // UserControlBookmarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.topPanel);
            this.Name = "UserControlBookmarks";
            this.Size = new System.Drawing.Size(676, 443);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBookMarks)).EndInit();
            this.contextMenuStripBookmarks.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtTextBox textBoxFilter;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.SplitContainer splitContainer;
        private BaseUtils.DataGridViewColumnControl dataGridViewBookMarks;
        private EDDiscovery.Forms.SurfaceBookmarkUserControl userControlSurfaceBookmarks;
        private ExtendedControls.ExtButton buttonNew;
        private ExtendedControls.ExtButton buttonEdit;
        private ExtendedControls.ExtButton buttonDelete;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBookmarkName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDescription;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColX;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColY;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColZ;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripBookmarks;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGotoStar3dmap;
        private System.Windows.Forms.ToolStripMenuItem openInEDSMToolStripMenuItem;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtButton buttonExtImport;
        private System.Windows.Forms.FlowLayoutPanel topPanel;
        private ExtendedControls.ExtButton extButtonEditSystem;
        private ExtendedControls.ExtButton extButtonNewRegion;
    }
}
