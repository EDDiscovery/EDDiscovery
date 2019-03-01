namespace EDDiscovery.UserControls
{
    partial class CaptainsLogEntries
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
            this.dateTimePickerEndDate = new ExtendedControls.ExtDateTimePicker();
            this.labelEndDate = new System.Windows.Forms.Label();
            this.dateTimePickerStartDate = new ExtendedControls.ExtDateTimePicker();
            this.labelDateStart = new System.Windows.Forms.Label();
            this.buttonTags = new ExtendedControls.ExtButton();
            this.buttonDelete = new ExtendedControls.ExtButton();
            this.buttonNew = new ExtendedControls.ExtButton();
            this.textBoxFilter = new ExtendedControls.ExtTextBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.ColTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBodyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemGotoStar3dmap = new System.Windows.Forms.ToolStripMenuItem();
            this.openInEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openAScanPanelViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelButtons.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.dateTimePickerEndDate);
            this.panelButtons.Controls.Add(this.labelEndDate);
            this.panelButtons.Controls.Add(this.dateTimePickerStartDate);
            this.panelButtons.Controls.Add(this.labelDateStart);
            this.panelButtons.Controls.Add(this.buttonTags);
            this.panelButtons.Controls.Add(this.buttonDelete);
            this.panelButtons.Controls.Add(this.buttonNew);
            this.panelButtons.Controls.Add(this.textBoxFilter);
            this.panelButtons.Controls.Add(this.labelSearch);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(676, 68);
            this.panelButtons.TabIndex = 0;
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerEndDate.BorderColorScaling = 0.5F;
            this.dateTimePickerEndDate.Checked = false;
            this.dateTimePickerEndDate.CustomFormat = "dd MMMM yyyy";
            this.dateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(419, 38);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerEndDate.ShowCheckBox = true;
            this.dateTimePickerEndDate.ShowUpDown = false;
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(211, 20);
            this.dateTimePickerEndDate.TabIndex = 32;
            this.dateTimePickerEndDate.TextBackColor = System.Drawing.Color.AliceBlue;
            this.dateTimePickerEndDate.Value = new System.DateTime(2017, 8, 30, 10, 50, 42, 853);
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.Location = new System.Drawing.Point(343, 44);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(55, 13);
            this.labelEndDate.TabIndex = 31;
            this.labelEndDate.Text = "End Date:";
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerStartDate.BorderColorScaling = 0.5F;
            this.dateTimePickerStartDate.Checked = false;
            this.dateTimePickerStartDate.CustomFormat = "dd MMMM yyyy";
            this.dateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(80, 38);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerStartDate.ShowCheckBox = true;
            this.dateTimePickerStartDate.ShowUpDown = false;
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(211, 20);
            this.dateTimePickerStartDate.TabIndex = 29;
            this.dateTimePickerStartDate.TextBackColor = System.Drawing.Color.AliceBlue;
            this.dateTimePickerStartDate.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // labelDateStart
            // 
            this.labelDateStart.AutoSize = true;
            this.labelDateStart.Location = new System.Drawing.Point(4, 44);
            this.labelDateStart.Name = "labelDateStart";
            this.labelDateStart.Size = new System.Drawing.Size(58, 13);
            this.labelDateStart.TabIndex = 28;
            this.labelDateStart.Text = "Start Date:";
            // 
            // buttonTags
            // 
            this.buttonTags.Image = global::EDDiscovery.Icons.Controls.CaptainsLog_Tags;
            this.buttonTags.Location = new System.Drawing.Point(280, 0);
            this.buttonTags.Name = "buttonTags";
            this.buttonTags.Size = new System.Drawing.Size(32, 32);
            this.buttonTags.TabIndex = 27;
            this.toolTip.SetToolTip(this.buttonTags, "Edit Tags");
            this.buttonTags.UseVisualStyleBackColor = true;
            this.buttonTags.Click += new System.EventHandler(this.buttonTags_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Image = global::EDDiscovery.Icons.Controls.CaptainsLog_Delete;
            this.buttonDelete.Location = new System.Drawing.Point(242, 0);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(32, 32);
            this.buttonDelete.TabIndex = 27;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete selected entries");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.Image = global::EDDiscovery.Icons.Controls.CaptainsLog_New;
            this.buttonNew.Location = new System.Drawing.Point(204, 0);
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(32, 32);
            this.buttonNew.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonNew, "New Entry");
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
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
            this.toolTip.SetToolTip(this.textBoxFilter, "Search for Log Entries");
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
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 68);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 20;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(676, 375);
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
            this.vScrollBarCustom1.Location = new System.Drawing.Point(656, 0);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 375);
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
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColTime,
            this.ColSystem,
            this.ColBodyName,
            this.ColNote,
            this.ColTags});
            this.dataGridView.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.Size = new System.Drawing.Size(656, 375);
            this.dataGridView.TabIndex = 4;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            this.dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellEndEdit);
            this.dataGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            this.dataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dataGridView_KeyDown);
            this.dataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseDown);
            // 
            // ColTime
            // 
            this.ColTime.HeaderText = "Time";
            this.ColTime.Name = "ColTime";
            // 
            // ColSystem
            // 
            this.ColSystem.HeaderText = "System";
            this.ColSystem.Name = "ColSystem";
            // 
            // ColBodyName
            // 
            this.ColBodyName.HeaderText = "Body";
            this.ColBodyName.Name = "ColBodyName";
            // 
            // ColNote
            // 
            this.ColNote.FillWeight = 300F;
            this.ColNote.HeaderText = "Note";
            this.ColNote.Name = "ColNote";
            this.ColNote.ReadOnly = true;
            // 
            // ColTags
            // 
            this.ColTags.HeaderText = "Tags";
            this.ColTags.Name = "ColTags";
            this.ColTags.ReadOnly = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemGotoStar3dmap,
            this.openInEDSMToolStripMenuItem,
            this.openAScanPanelViewToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStripBookmarks";
            this.contextMenuStrip.Size = new System.Drawing.Size(201, 70);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // toolStripMenuItemGotoStar3dmap
            // 
            this.toolStripMenuItemGotoStar3dmap.Name = "toolStripMenuItemGotoStar3dmap";
            this.toolStripMenuItemGotoStar3dmap.Size = new System.Drawing.Size(200, 22);
            this.toolStripMenuItemGotoStar3dmap.Text = "Goto in 3D Map";
            this.toolStripMenuItemGotoStar3dmap.Click += new System.EventHandler(this.toolStripMenuItemGotoStar3dmap_Click);
            // 
            // openInEDSMToolStripMenuItem
            // 
            this.openInEDSMToolStripMenuItem.Name = "openInEDSMToolStripMenuItem";
            this.openInEDSMToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.openInEDSMToolStripMenuItem.Text = "Open in EDSM";
            this.openInEDSMToolStripMenuItem.Click += new System.EventHandler(this.openInEDSMToolStripMenuItem_Click);
            // 
            // openAScanPanelViewToolStripMenuItem
            // 
            this.openAScanPanelViewToolStripMenuItem.Name = "openAScanPanelViewToolStripMenuItem";
            this.openAScanPanelViewToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.openAScanPanelViewToolStripMenuItem.Text = "Open a Scan Panel View";
            this.openAScanPanelViewToolStripMenuItem.Click += new System.EventHandler(this.openAScanPanelViewToolStripMenuItem_Click);
            // 
            // CaptainsLogEntries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelButtons);
            this.Name = "CaptainsLogEntries";
            this.Size = new System.Drawing.Size(676, 443);
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelButtons;
        private ExtendedControls.ExtTextBox textBoxFilter;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.DataGridView dataGridView;
        private ExtendedControls.ExtButton buttonNew;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGotoStar3dmap;
        private System.Windows.Forms.ToolStripMenuItem openInEDSMToolStripMenuItem;
        private ExtendedControls.ExtButton buttonDelete;
        private ExtendedControls.ExtButton buttonTags;
        private ExtendedControls.ExtDateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label labelEndDate;
        private ExtendedControls.ExtDateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelDateStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTags;
        private System.Windows.Forms.ToolStripMenuItem openAScanPanelViewToolStripMenuItem;
    }
}
