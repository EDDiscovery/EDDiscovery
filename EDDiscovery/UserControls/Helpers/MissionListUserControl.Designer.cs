namespace EDDiscovery.UserControls.Helpers
{
    partial class MissionListUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MissionListUserControl));
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.PcolName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColEnd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColOrigin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColFromFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColDestSys = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColTargetFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pColInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.customDateTimePickerStart = new ExtendedControls.ExtDateTimePicker();
            this.labelTo = new System.Windows.Forms.Label();
            this.customDateTimePickerEnd = new ExtendedControls.ExtDateTimePicker();
            this.labelSearch = new System.Windows.Forms.Label();
            this.extTextBoxSearch = new ExtendedControls.ExtTextBox();
            this.labelValue = new System.Windows.Forms.Label();
            this.extPanelDataGridViewScroll = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBar1 = new ExtendedControls.ExtScrollBar();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.extPanelDataGridViewScroll.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView
            // 
            this.dataGridView.AllowDrop = true;
            this.dataGridView.AllowRowHeaderVisibleSelection = false;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AutoSortByColumnName = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnReorder = true;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.PcolName,
            this.pColStart,
            this.pColEnd,
            this.pColOrigin,
            this.pColFromFaction,
            this.pColDestSys,
            this.pColTargetFaction,
            this.pColResult,
            this.pColInfo});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.PerColumnWordWrapControl = true;
            this.dataGridView.RowHeaderMenuStrip = null;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowHeadersWidth = 25;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(1074, 592);
            this.dataGridView.TabIndex = 3;
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            // 
            // PcolName
            // 
            this.PcolName.HeaderText = "Name";
            this.PcolName.MinimumWidth = 50;
            this.PcolName.Name = "PcolName";
            this.PcolName.ReadOnly = true;
            // 
            // pColStart
            // 
            this.pColStart.FillWeight = 75F;
            this.pColStart.HeaderText = "Start Date";
            this.pColStart.MinimumWidth = 50;
            this.pColStart.Name = "pColStart";
            this.pColStart.ReadOnly = true;
            // 
            // pColEnd
            // 
            this.pColEnd.FillWeight = 75F;
            this.pColEnd.HeaderText = "End Date";
            this.pColEnd.MinimumWidth = 50;
            this.pColEnd.Name = "pColEnd";
            this.pColEnd.ReadOnly = true;
            // 
            // pColOrigin
            // 
            this.pColOrigin.FillWeight = 80F;
            this.pColOrigin.HeaderText = "Origin";
            this.pColOrigin.MinimumWidth = 50;
            this.pColOrigin.Name = "pColOrigin";
            this.pColOrigin.ReadOnly = true;
            // 
            // pColFromFaction
            // 
            this.pColFromFaction.FillWeight = 50F;
            this.pColFromFaction.HeaderText = "Faction";
            this.pColFromFaction.MinimumWidth = 50;
            this.pColFromFaction.Name = "pColFromFaction";
            this.pColFromFaction.ReadOnly = true;
            // 
            // pColDestSys
            // 
            this.pColDestSys.FillWeight = 80F;
            this.pColDestSys.HeaderText = "Destination";
            this.pColDestSys.MinimumWidth = 50;
            this.pColDestSys.Name = "pColDestSys";
            this.pColDestSys.ReadOnly = true;
            // 
            // pColTargetFaction
            // 
            this.pColTargetFaction.FillWeight = 50F;
            this.pColTargetFaction.HeaderText = "Target Faction";
            this.pColTargetFaction.MinimumWidth = 50;
            this.pColTargetFaction.Name = "pColTargetFaction";
            this.pColTargetFaction.ReadOnly = true;
            // 
            // pColResult
            // 
            this.pColResult.FillWeight = 80F;
            this.pColResult.HeaderText = "Result (cr)";
            this.pColResult.MinimumWidth = 25;
            this.pColResult.Name = "pColResult";
            this.pColResult.ReadOnly = true;
            // 
            // pColInfo
            // 
            this.pColInfo.FillWeight = 150F;
            this.pColInfo.HeaderText = "Info";
            this.pColInfo.MinimumWidth = 50;
            this.pColInfo.Name = "pColInfo";
            this.pColInfo.ReadOnly = true;
            // 
            // panelButtons
            // 
            this.panelButtons.AutoSize = true;
            this.panelButtons.Controls.Add(this.customDateTimePickerStart);
            this.panelButtons.Controls.Add(this.labelTo);
            this.panelButtons.Controls.Add(this.customDateTimePickerEnd);
            this.panelButtons.Controls.Add(this.labelSearch);
            this.panelButtons.Controls.Add(this.extTextBoxSearch);
            this.panelButtons.Controls.Add(this.labelValue);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(1090, 27);
            this.panelButtons.TabIndex = 4;
            // 
            // customDateTimePickerStart
            // 
            this.customDateTimePickerStart.BorderColor = System.Drawing.Color.Transparent;
            this.customDateTimePickerStart.Checked = false;
            this.customDateTimePickerStart.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.customDateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.customDateTimePickerStart.Location = new System.Drawing.Point(0, 1);
            this.customDateTimePickerStart.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.customDateTimePickerStart.Name = "customDateTimePickerStart";
            this.customDateTimePickerStart.SelectedColor = System.Drawing.Color.Yellow;
            this.customDateTimePickerStart.ShowCheckBox = true;
            this.customDateTimePickerStart.ShowUpDown = false;
            this.customDateTimePickerStart.Size = new System.Drawing.Size(250, 24);
            this.customDateTimePickerStart.TabIndex = 0;
            this.customDateTimePickerStart.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.customDateTimePickerStart.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            this.customDateTimePickerStart.ValueChanged += new System.EventHandler(this.customDateTimePickerStart_ValueChanged);
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(258, 4);
            this.labelTo.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(16, 13);
            this.labelTo.TabIndex = 1;
            this.labelTo.Text = "to";
            // 
            // customDateTimePickerEnd
            // 
            this.customDateTimePickerEnd.BorderColor = System.Drawing.Color.Transparent;
            this.customDateTimePickerEnd.Checked = false;
            this.customDateTimePickerEnd.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.customDateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.customDateTimePickerEnd.Location = new System.Drawing.Point(282, 1);
            this.customDateTimePickerEnd.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.customDateTimePickerEnd.Name = "customDateTimePickerEnd";
            this.customDateTimePickerEnd.SelectedColor = System.Drawing.Color.Yellow;
            this.customDateTimePickerEnd.ShowCheckBox = true;
            this.customDateTimePickerEnd.ShowUpDown = false;
            this.customDateTimePickerEnd.Size = new System.Drawing.Size(250, 24);
            this.customDateTimePickerEnd.TabIndex = 0;
            this.customDateTimePickerEnd.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.customDateTimePickerEnd.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            this.customDateTimePickerEnd.ValueChanged += new System.EventHandler(this.customDateTimePickerEnd_ValueChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(540, 4);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 1;
            this.labelSearch.Text = "Search";
            // 
            // extTextBoxSearch
            // 
            this.extTextBoxSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.extTextBoxSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.extTextBoxSearch.BackErrorColor = System.Drawing.Color.Red;
            this.extTextBoxSearch.BorderColor = System.Drawing.Color.Transparent;
            this.extTextBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.extTextBoxSearch.ClearOnFirstChar = false;
            this.extTextBoxSearch.ControlBackground = System.Drawing.SystemColors.Control;
            this.extTextBoxSearch.EndButtonEnable = true;
            this.extTextBoxSearch.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("extTextBoxSearch.EndButtonImage")));
            this.extTextBoxSearch.EndButtonVisible = false;
            this.extTextBoxSearch.InErrorCondition = false;
            this.extTextBoxSearch.Location = new System.Drawing.Point(592, 1);
            this.extTextBoxSearch.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.extTextBoxSearch.Multiline = false;
            this.extTextBoxSearch.Name = "extTextBoxSearch";
            this.extTextBoxSearch.ReadOnly = false;
            this.extTextBoxSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.extTextBoxSearch.SelectionLength = 0;
            this.extTextBoxSearch.SelectionStart = 0;
            this.extTextBoxSearch.Size = new System.Drawing.Size(150, 23);
            this.extTextBoxSearch.TabIndex = 2;
            this.extTextBoxSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.extTextBoxSearch.WordWrap = true;
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(745, 4);
            this.labelValue.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(43, 13);
            this.labelValue.TabIndex = 1;
            this.labelValue.Text = "<code>";
            // 
            // extPanelDataGridViewScroll
            // 
            this.extPanelDataGridViewScroll.Controls.Add(this.extScrollBar1);
            this.extPanelDataGridViewScroll.Controls.Add(this.dataGridView);
            this.extPanelDataGridViewScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScroll.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScroll.Location = new System.Drawing.Point(0, 27);
            this.extPanelDataGridViewScroll.Name = "extPanelDataGridViewScroll";
            this.extPanelDataGridViewScroll.Size = new System.Drawing.Size(1090, 592);
            this.extPanelDataGridViewScroll.TabIndex = 5;
            this.extPanelDataGridViewScroll.VerticalScrollBarDockRight = true;
            // 
            // extScrollBar1
            // 
            this.extScrollBar1.AlwaysHideScrollBar = false;
            this.extScrollBar1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBar1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBar1.ArrowDownDrawAngle = 270F;
            this.extScrollBar1.ArrowUpDrawAngle = 90F;
            this.extScrollBar1.BorderColor = System.Drawing.Color.White;
            this.extScrollBar1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBar1.HideScrollBar = false;
            this.extScrollBar1.LargeChange = 0;
            this.extScrollBar1.Location = new System.Drawing.Point(1074, 0);
            this.extScrollBar1.Maximum = -1;
            this.extScrollBar1.Minimum = 0;
            this.extScrollBar1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBar1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBar1.Name = "extScrollBar1";
            this.extScrollBar1.Size = new System.Drawing.Size(16, 592);
            this.extScrollBar1.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBar1.SmallChange = 1;
            this.extScrollBar1.TabIndex = 6;
            this.extScrollBar1.Text = "";
            this.extScrollBar1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBar1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBar1.ThumbDrawAngle = 0F;
            this.extScrollBar1.Value = -1;
            this.extScrollBar1.ValueLimited = -1;
            // 
            // MissionListUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extPanelDataGridViewScroll);
            this.Controls.Add(this.panelButtons);
            this.Name = "MissionListUserControl";
            this.Size = new System.Drawing.Size(1090, 619);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.extPanelDataGridViewScroll.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public BaseUtils.DataGridViewColumnControl dataGridView;
        private System.Windows.Forms.FlowLayoutPanel panelButtons;
        public ExtendedControls.ExtDateTimePicker customDateTimePickerStart;
        private System.Windows.Forms.Label labelTo;
        public ExtendedControls.ExtDateTimePicker customDateTimePickerEnd;
        private System.Windows.Forms.Label labelValue;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScroll;
        private ExtendedControls.ExtScrollBar extScrollBar1;
        private System.Windows.Forms.DataGridViewTextBoxColumn PcolName;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColEnd;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColOrigin;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColFromFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColDestSys;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColTargetFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColResult;
        private System.Windows.Forms.DataGridViewTextBoxColumn pColInfo;
        private System.Windows.Forms.Label labelSearch;
        private ExtendedControls.ExtTextBox extTextBoxSearch;
    }
}
