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
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonExtImport = new ExtendedControls.ButtonExt();
            this.buttonExtExcel = new ExtendedControls.ButtonExt();
            this.buttonDelete = new ExtendedControls.ButtonExt();
            this.buttonNew = new ExtendedControls.ButtonExt();
            this.buttonEdit = new ExtendedControls.ButtonExt();
            this.textBoxFilter = new ExtendedControls.TextBoxBorder();
            this.labelSearch = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataViewScrollerPanel = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewBookMarks = new System.Windows.Forms.DataGridView();
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
            this.panelButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBookMarks)).BeginInit();
            this.contextMenuStripBookmarks.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonExtImport);
            this.panelButtons.Controls.Add(this.buttonExtExcel);
            this.panelButtons.Controls.Add(this.buttonDelete);
            this.panelButtons.Controls.Add(this.buttonNew);
            this.panelButtons.Controls.Add(this.buttonEdit);
            this.panelButtons.Controls.Add(this.textBoxFilter);
            this.panelButtons.Controls.Add(this.labelSearch);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(676, 32);
            this.panelButtons.TabIndex = 0;
            // 
            // buttonExtImport
            // 
            this.buttonExtImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtImport.Image = global::EDDiscovery.Icons.Controls.ImportExcel;
            this.buttonExtImport.Location = new System.Drawing.Point(338, 3);
            this.buttonExtImport.Name = "buttonExtImport";
            this.buttonExtImport.Size = new System.Drawing.Size(24, 24);
            this.buttonExtImport.TabIndex = 38;
            this.toolTip.SetToolTip(this.buttonExtImport, "Import bookmarks to EDD from CSV file");
            this.buttonExtImport.UseVisualStyleBackColor = true;
            this.buttonExtImport.Click += new System.EventHandler(this.buttonExtImport_Click);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.TravelGrid_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(308, 3);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(24, 24);
            this.buttonExtExcel.TabIndex = 38;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Export bookmarks to CSV file");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Image = global::EDDiscovery.Icons.Controls.Bookmarks_Delete;
            this.buttonDelete.Location = new System.Drawing.Point(270, 0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(32, 32);
            this.buttonDelete.TabIndex = 27;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete selected bookmark");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Image = global::EDDiscovery.Icons.Controls.Bookmarks_New;
            this.buttonNew.Location = new System.Drawing.Point(204, 0);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(32, 32);
            this.buttonNew.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonNew, "New Bookmark");
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Image = global::EDDiscovery.Icons.Controls.Bookmarks_Edit;
            this.buttonEdit.Location = new System.Drawing.Point(236, 0);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(32, 32);
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
            this.textBoxFilter.InErrorCondition = false;
            this.textBoxFilter.Location = new System.Drawing.Point(50, 6);
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
            this.labelSearch.Location = new System.Drawing.Point(3, 6);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 26;
            this.labelSearch.Text = "Search";
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 32);
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
            this.splitContainer.Size = new System.Drawing.Size(676, 411);
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
            this.dataViewScrollerPanel.ScrollBarWidth = 20;
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
            this.vScrollBarCustom1.Location = new System.Drawing.Point(656, 34);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 170);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 6;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
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
            this.dataGridViewBookMarks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
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
            this.dataGridViewBookMarks.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewBookMarks.Size = new System.Drawing.Size(656, 204);
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
            this.userControlSurfaceBookmarks.Size = new System.Drawing.Size(676, 203);
            this.userControlSurfaceBookmarks.TabIndex = 5;
            // 
            // UserControlBookmarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panelButtons);
            this.Name = "UserControlBookmarks";
            this.Size = new System.Drawing.Size(676, 443);
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBookMarks)).EndInit();
            this.contextMenuStripBookmarks.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private ExtendedControls.TextBoxBorder textBoxFilter;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dataGridViewBookMarks;
        private EDDiscovery.Forms.SurfaceBookmarkUserControl userControlSurfaceBookmarks;
        private ExtendedControls.ButtonExt buttonNew;
        private ExtendedControls.ButtonExt buttonEdit;
        private ExtendedControls.ButtonExt buttonDelete;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
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
        private ExtendedControls.ButtonExt buttonExtExcel;
        private ExtendedControls.ButtonExt buttonExtImport;
    }
}
