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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptainsLogEntries));
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
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.ColTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBodyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColTags = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewScanDisplayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapGoto3StartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnSpanshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonExcel = new ExtendedControls.ExtButton();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dateTimePickerEndDate
            // 
            this.dateTimePickerEndDate.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerEndDate.BorderColor2 = System.Drawing.Color.Transparent;
            this.dateTimePickerEndDate.Checked = false;
            this.dateTimePickerEndDate.CustomFormat = "dd MMMM yyyy";
            this.dateTimePickerEndDate.DisabledScaling = 0.5F;
            this.dateTimePickerEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dateTimePickerEndDate.Location = new System.Drawing.Point(384, 1);
            this.dateTimePickerEndDate.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.dateTimePickerEndDate.Name = "dateTimePickerEndDate";
            this.dateTimePickerEndDate.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerEndDate.ShowCheckBox = true;
            this.dateTimePickerEndDate.ShowUpDown = false;
            this.dateTimePickerEndDate.Size = new System.Drawing.Size(250, 20);
            this.dateTimePickerEndDate.TabIndex = 32;
            this.dateTimePickerEndDate.TextBackColor = System.Drawing.Color.AliceBlue;
            this.dateTimePickerEndDate.Value = new System.DateTime(2017, 8, 30, 10, 50, 42, 853);
            // 
            // labelEndDate
            // 
            this.labelEndDate.AutoSize = true;
            this.labelEndDate.Location = new System.Drawing.Point(323, 1);
            this.labelEndDate.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.labelEndDate.Name = "labelEndDate";
            this.labelEndDate.Size = new System.Drawing.Size(55, 13);
            this.labelEndDate.TabIndex = 31;
            this.labelEndDate.Text = "End Date";
            // 
            // dateTimePickerStartDate
            // 
            this.dateTimePickerStartDate.BorderColor = System.Drawing.Color.Transparent;
            this.dateTimePickerStartDate.BorderColor2 = System.Drawing.Color.Transparent;
            this.dateTimePickerStartDate.Checked = false;
            this.dateTimePickerStartDate.CustomFormat = "dd MMMM yyyy";
            this.dateTimePickerStartDate.DisabledScaling = 0.5F;
            this.dateTimePickerStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Long;
            this.dateTimePickerStartDate.Location = new System.Drawing.Point(67, 1);
            this.dateTimePickerStartDate.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.dateTimePickerStartDate.Name = "dateTimePickerStartDate";
            this.dateTimePickerStartDate.SelectedColor = System.Drawing.Color.Yellow;
            this.dateTimePickerStartDate.ShowCheckBox = true;
            this.dateTimePickerStartDate.ShowUpDown = false;
            this.dateTimePickerStartDate.Size = new System.Drawing.Size(250, 20);
            this.dateTimePickerStartDate.TabIndex = 29;
            this.dateTimePickerStartDate.TextBackColor = System.Drawing.Color.AliceBlue;
            this.dateTimePickerStartDate.Value = new System.DateTime(2010, 1, 1, 0, 0, 0, 0);
            // 
            // labelDateStart
            // 
            this.labelDateStart.AutoSize = true;
            this.labelDateStart.Location = new System.Drawing.Point(3, 1);
            this.labelDateStart.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.labelDateStart.Name = "labelDateStart";
            this.labelDateStart.Size = new System.Drawing.Size(58, 13);
            this.labelDateStart.TabIndex = 28;
            this.labelDateStart.Text = "Start Date";
            // 
            // buttonTags
            // 
            this.buttonTags.BackColor2 = System.Drawing.Color.Red;
            this.buttonTags.ButtonDisabledScaling = 0.5F;
            this.buttonTags.GradientDirection = 90F;
            this.buttonTags.Image = global::EDDiscovery.Icons.Controls.Tags;
            this.buttonTags.Location = new System.Drawing.Point(306, 1);
            this.buttonTags.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonTags.MouseOverScaling = 1.3F;
            this.buttonTags.MouseSelectedScaling = 1.3F;
            this.buttonTags.Name = "buttonTags";
            this.buttonTags.Size = new System.Drawing.Size(28, 28);
            this.buttonTags.TabIndex = 27;
            this.toolTip.SetToolTip(this.buttonTags, "Edit Tags");
            this.buttonTags.UseVisualStyleBackColor = true;
            this.buttonTags.Click += new System.EventHandler(this.buttonTags_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.BackColor2 = System.Drawing.Color.Red;
            this.buttonDelete.ButtonDisabledScaling = 0.5F;
            this.buttonDelete.GradientDirection = 90F;
            this.buttonDelete.Image = global::EDDiscovery.Icons.Controls.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(272, 1);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonDelete.MouseOverScaling = 1.3F;
            this.buttonDelete.MouseSelectedScaling = 1.3F;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(28, 28);
            this.buttonDelete.TabIndex = 27;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete selected entries");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonNew
            // 
            this.buttonNew.BackColor2 = System.Drawing.Color.Red;
            this.buttonNew.ButtonDisabledScaling = 0.5F;
            this.buttonNew.GradientDirection = 90F;
            this.buttonNew.Image = global::EDDiscovery.Icons.Controls.NewPage;
            this.buttonNew.Location = new System.Drawing.Point(238, 1);
            this.buttonNew.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonNew.MouseOverScaling = 1.3F;
            this.buttonNew.MouseSelectedScaling = 1.3F;
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(28, 28);
            this.buttonNew.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonNew, "New Entry");
            this.buttonNew.UseVisualStyleBackColor = true;
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
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
            this.toolTip.SetToolTip(this.textBoxFilter, "Search for Log Entries");
            this.textBoxFilter.WordWrap = true;
            this.textBoxFilter.TextChanged += new System.EventHandler(this.textBoxFilter_TextChanged);
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
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 52);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 24;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(896, 598);
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
            this.vScrollBarCustom1.Location = new System.Drawing.Point(872, 0);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(24, 598);
            this.vScrollBarCustom1.SkinnyStyle = false;
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
            this.dataGridView.AutoSortByColumnName = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnReorder = true;
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
            this.dataGridView.PerColumnWordWrapControl = true;
            this.dataGridView.RowHeaderMenuStrip = null;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(872, 598);
            this.dataGridView.TabIndex = 4;
            this.dataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellClick);
            this.dataGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellEndEdit);
            this.dataGridView.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView_ColumnWidthChanged);
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
            this.ColTags.MinimumWidth = 32;
            this.ColTags.Name = "ColTags";
            this.ColTags.ReadOnly = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewScanDisplayToolStripMenuItem,
            this.mapGoto3StartoolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.viewOnSpanshToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStripBookmarks";
            this.contextMenuStrip.Size = new System.Drawing.Size(187, 92);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // viewScanDisplayToolStripMenuItem
            // 
            this.viewScanDisplayToolStripMenuItem.Name = "viewScanDisplayToolStripMenuItem";
            this.viewScanDisplayToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewScanDisplayToolStripMenuItem.Text = "View Scan Display";
            this.viewScanDisplayToolStripMenuItem.Click += new System.EventHandler(this.viewScanDisplayToolStripMenuItem_Click);
            // 
            // mapGoto3StartoolStripMenuItem
            // 
            this.mapGoto3StartoolStripMenuItem.Name = "mapGoto3StartoolStripMenuItem";
            this.mapGoto3StartoolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.mapGoto3StartoolStripMenuItem.Text = "Go to star on 3D Map";
            this.mapGoto3StartoolStripMenuItem.Click += new System.EventHandler(this.toolStripMenuItemGotoStar3dmap_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.openInEDSMToolStripMenuItem_Click);
            // 
            // viewOnSpanshToolStripMenuItem
            // 
            this.viewOnSpanshToolStripMenuItem.Name = "viewOnSpanshToolStripMenuItem";
            this.viewOnSpanshToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.viewOnSpanshToolStripMenuItem.Text = "View on Spansh";
            this.viewOnSpanshToolStripMenuItem.Click += new System.EventHandler(this.viewOnSpanshToolStripMenuItem_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
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
            this.buttonFilter.TabIndex = 28;
            this.toolTip.SetToolTip(this.buttonFilter, "Filter out entries based on tag");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.MouseClick += new System.Windows.Forms.MouseEventHandler(this.buttonFilter_MouseClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.labelSearch);
            this.flowLayoutPanel1.Controls.Add(this.textBoxFilter);
            this.flowLayoutPanel1.Controls.Add(this.buttonFilter);
            this.flowLayoutPanel1.Controls.Add(this.buttonNew);
            this.flowLayoutPanel1.Controls.Add(this.buttonDelete);
            this.flowLayoutPanel1.Controls.Add(this.buttonTags);
            this.flowLayoutPanel1.Controls.Add(this.extButtonExcel);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(896, 30);
            this.flowLayoutPanel1.TabIndex = 33;
            // 
            // extButtonExcel
            // 
            this.extButtonExcel.BackColor2 = System.Drawing.Color.Red;
            this.extButtonExcel.ButtonDisabledScaling = 0.5F;
            this.extButtonExcel.GradientDirection = 90F;
            this.extButtonExcel.Image = global::EDDiscovery.Icons.Controls.Scan_ExportToExcel;
            this.extButtonExcel.Location = new System.Drawing.Point(340, 1);
            this.extButtonExcel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonExcel.MouseOverScaling = 1.3F;
            this.extButtonExcel.MouseSelectedScaling = 1.3F;
            this.extButtonExcel.Name = "extButtonExcel";
            this.extButtonExcel.Size = new System.Drawing.Size(28, 28);
            this.extButtonExcel.TabIndex = 27;
            this.extButtonExcel.UseVisualStyleBackColor = true;
            this.extButtonExcel.Click += new System.EventHandler(this.extButtonExcel_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.AutoSize = true;
            this.flowLayoutPanel2.Controls.Add(this.labelDateStart);
            this.flowLayoutPanel2.Controls.Add(this.dateTimePickerStartDate);
            this.flowLayoutPanel2.Controls.Add(this.labelEndDate);
            this.flowLayoutPanel2.Controls.Add(this.dateTimePickerEndDate);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 30);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(896, 22);
            this.flowLayoutPanel2.TabIndex = 28;
            // 
            // CaptainsLogEntries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.flowLayoutPanel2);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "CaptainsLogEntries";
            this.Size = new System.Drawing.Size(896, 650);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtTextBox textBoxFilter;
        private System.Windows.Forms.Label labelSearch;
        private BaseUtils.DataGridViewColumnControl dataGridView;
        private ExtendedControls.ExtButton buttonNew;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem mapGoto3StartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private ExtendedControls.ExtButton buttonDelete;
        private ExtendedControls.ExtButton buttonTags;
        private ExtendedControls.ExtDateTimePicker dateTimePickerEndDate;
        private System.Windows.Forms.Label labelEndDate;
        private ExtendedControls.ExtDateTimePicker dateTimePickerStartDate;
        private System.Windows.Forms.Label labelDateStart;
        private System.Windows.Forms.ToolStripMenuItem viewScanDisplayToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private ExtendedControls.ExtButton extButtonExcel;
        private ExtendedControls.ExtButton buttonFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNote;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColTags;
        private System.Windows.Forms.ToolStripMenuItem viewOnSpanshToolStripMenuItem;
    }
}
