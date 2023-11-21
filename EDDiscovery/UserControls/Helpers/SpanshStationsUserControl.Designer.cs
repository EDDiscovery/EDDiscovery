
namespace EDDiscovery.UserControls.Helpers
{
    partial class SpanshStationsUserControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpanshStationsUserControl));
            this.flowLayoutStarDistances = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.labelMaxLs = new System.Windows.Forms.Label();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewOnSpanshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMarketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOutfittingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewShipyardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
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
            this.extTextBoxAutoCompleteSystem = new ExtendedControls.ExtTextBoxAutoComplete();
            this.valueBoxMaxLs = new ExtendedControls.NumberBoxDouble();
            this.extButtonType = new ExtendedControls.ExtButtonWithCheckedIconListBoxGroup();
            this.extButtonCommodities = new ExtendedControls.ExtButtonWithCheckedIconListBoxGroup();
            this.extButtonOutfitting = new ExtendedControls.ExtButtonWithCheckedIconListBoxGroup();
            this.extButtonShipyard = new ExtendedControls.ExtButtonWithCheckedIconListBoxGroup();
            this.extButtonEconomy = new ExtendedControls.ExtButtonWithCheckedIconListBoxGroup();
            this.extButtonServices = new ExtendedControls.ExtButtonWithCheckedIconListBoxGroup();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.flowLayoutStarDistances.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayoutStarDistances
            // 
            this.flowLayoutStarDistances.AutoSize = true;
            this.flowLayoutStarDistances.Controls.Add(this.label1);
            this.flowLayoutStarDistances.Controls.Add(this.extTextBoxAutoCompleteSystem);
            this.flowLayoutStarDistances.Controls.Add(this.labelMaxLs);
            this.flowLayoutStarDistances.Controls.Add(this.valueBoxMaxLs);
            this.flowLayoutStarDistances.Controls.Add(this.extButtonType);
            this.flowLayoutStarDistances.Controls.Add(this.extButtonCommodities);
            this.flowLayoutStarDistances.Controls.Add(this.extButtonOutfitting);
            this.flowLayoutStarDistances.Controls.Add(this.extButtonShipyard);
            this.flowLayoutStarDistances.Controls.Add(this.extButtonEconomy);
            this.flowLayoutStarDistances.Controls.Add(this.extButtonServices);
            this.flowLayoutStarDistances.Controls.Add(this.extCheckBoxWordWrap);
            this.flowLayoutStarDistances.Controls.Add(this.buttonExtExcel);
            this.flowLayoutStarDistances.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutStarDistances.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutStarDistances.Name = "flowLayoutStarDistances";
            this.flowLayoutStarDistances.Size = new System.Drawing.Size(978, 30);
            this.flowLayoutStarDistances.TabIndex = 25;
            this.flowLayoutStarDistances.WrapContents = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 2);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "System";
            // 
            // labelMaxLs
            // 
            this.labelMaxLs.AutoSize = true;
            this.labelMaxLs.Location = new System.Drawing.Point(223, 2);
            this.labelMaxLs.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.labelMaxLs.Name = "labelMaxLs";
            this.labelMaxLs.Size = new System.Drawing.Size(41, 13);
            this.labelMaxLs.TabIndex = 3;
            this.labelMaxLs.Text = "Max Ls";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewOnSpanshToolStripMenuItem,
            this.viewMarketToolStripMenuItem,
            this.viewOutfittingToolStripMenuItem,
            this.viewShipyardToolStripMenuItem});
            this.contextMenuStrip.Name = "closestContextMenu";
            this.contextMenuStrip.Size = new System.Drawing.Size(158, 92);
            // 
            // viewOnSpanshToolStripMenuItem
            // 
            this.viewOnSpanshToolStripMenuItem.Name = "viewOnSpanshToolStripMenuItem";
            this.viewOnSpanshToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.viewOnSpanshToolStripMenuItem.Text = "View on Spansh";
            this.viewOnSpanshToolStripMenuItem.Click += new System.EventHandler(this.viewOnSpanshToolStripMenuItem_Click);
            // 
            // viewMarketToolStripMenuItem
            // 
            this.viewMarketToolStripMenuItem.Name = "viewMarketToolStripMenuItem";
            this.viewMarketToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.viewMarketToolStripMenuItem.Text = "View Market";
            this.viewMarketToolStripMenuItem.Click += new System.EventHandler(this.viewMarketToolStripMenuItem_Click);
            // 
            // viewOutfittingToolStripMenuItem
            // 
            this.viewOutfittingToolStripMenuItem.Name = "viewOutfittingToolStripMenuItem";
            this.viewOutfittingToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.viewOutfittingToolStripMenuItem.Text = "View Outfitting";
            this.viewOutfittingToolStripMenuItem.Click += new System.EventHandler(this.viewOutfittingToolStripMenuItem_Click);
            // 
            // viewShipyardToolStripMenuItem
            // 
            this.viewShipyardToolStripMenuItem.Name = "viewShipyardToolStripMenuItem";
            this.viewShipyardToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.viewShipyardToolStripMenuItem.Text = "View Shipyard";
            this.viewShipyardToolStripMenuItem.Click += new System.EventHandler(this.viewShipyardToolStripMenuItem_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 480;
            this.toolTip.ReshowDelay = 96;
            this.toolTip.ShowAlways = true;
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(978, 747);
            this.dataViewScrollerPanel.TabIndex = 26;
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
            this.vScrollBarCustom.Location = new System.Drawing.Point(962, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(16, 747);
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
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
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
            this.dataGridView.Size = new System.Drawing.Size(962, 747);
            this.dataGridView.TabIndex = 23;
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            this.dataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseDown);
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
            this.colServices.FillWeight = 200F;
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
            // extTextBoxAutoCompleteSystem
            // 
            this.extTextBoxAutoCompleteSystem.AutoCompleteCommentMarker = null;
            this.extTextBoxAutoCompleteSystem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.extTextBoxAutoCompleteSystem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.extTextBoxAutoCompleteSystem.AutoCompleteTimeout = 500;
            this.extTextBoxAutoCompleteSystem.BackErrorColor = System.Drawing.Color.Red;
            this.extTextBoxAutoCompleteSystem.BorderColor = System.Drawing.Color.Transparent;
            this.extTextBoxAutoCompleteSystem.BorderColorScaling = 0.5F;
            this.extTextBoxAutoCompleteSystem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.extTextBoxAutoCompleteSystem.ClearOnFirstChar = false;
            this.extTextBoxAutoCompleteSystem.ControlBackground = System.Drawing.SystemColors.Control;
            this.extTextBoxAutoCompleteSystem.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.extTextBoxAutoCompleteSystem.DropDownBorderColor = System.Drawing.Color.Green;
            this.extTextBoxAutoCompleteSystem.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.extTextBoxAutoCompleteSystem.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.extTextBoxAutoCompleteSystem.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.extTextBoxAutoCompleteSystem.EndButtonEnable = false;
            this.extTextBoxAutoCompleteSystem.EndButtonSize16ths = 10;
            this.extTextBoxAutoCompleteSystem.EndButtonVisible = false;
            this.extTextBoxAutoCompleteSystem.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extTextBoxAutoCompleteSystem.InErrorCondition = false;
            this.extTextBoxAutoCompleteSystem.Location = new System.Drawing.Point(53, 2);
            this.extTextBoxAutoCompleteSystem.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.extTextBoxAutoCompleteSystem.Multiline = false;
            this.extTextBoxAutoCompleteSystem.Name = "extTextBoxAutoCompleteSystem";
            this.extTextBoxAutoCompleteSystem.ReadOnly = false;
            this.extTextBoxAutoCompleteSystem.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.extTextBoxAutoCompleteSystem.SelectionLength = 0;
            this.extTextBoxAutoCompleteSystem.SelectionStart = 0;
            this.extTextBoxAutoCompleteSystem.Size = new System.Drawing.Size(162, 23);
            this.extTextBoxAutoCompleteSystem.TabIndex = 30;
            this.extTextBoxAutoCompleteSystem.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.extTextBoxAutoCompleteSystem.TextChangedEvent = "";
            this.extTextBoxAutoCompleteSystem.TextNoChange = "";
            this.toolTip.SetToolTip(this.extTextBoxAutoCompleteSystem, "Type in system name and hit return");
            this.extTextBoxAutoCompleteSystem.WordWrap = true;
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
            this.valueBoxMaxLs.Location = new System.Drawing.Point(272, 2);
            this.valueBoxMaxLs.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
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
            this.valueBoxMaxLs.TextNoChange = "1000000";
            this.toolTip.SetToolTip(this.valueBoxMaxLs, "Maximum station distance");
            this.valueBoxMaxLs.Value = 1000000D;
            this.valueBoxMaxLs.WordWrap = true;
            this.valueBoxMaxLs.ValueChanged += new System.EventHandler(this.valueBoxMaxLs_ValueChanged);
            // 
            // extButtonType
            // 
            this.extButtonType.Image = global::EDDiscovery.Icons.Controls.CoriolisYellow;
            this.extButtonType.Location = new System.Drawing.Point(364, 2);
            this.extButtonType.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.extButtonType.Name = "extButtonType";
            this.extButtonType.SettingsSplittingChar = ';';
            this.extButtonType.Size = new System.Drawing.Size(28, 28);
            this.extButtonType.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonType, "Filter on station type");
            this.extButtonType.UseVisualStyleBackColor = true;
            // 
            // extButtonCommodities
            // 
            this.extButtonCommodities.Image = global::EDDiscovery.Icons.Controls.Commodity;
            this.extButtonCommodities.Location = new System.Drawing.Point(400, 2);
            this.extButtonCommodities.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.extButtonCommodities.Name = "extButtonCommodities";
            this.extButtonCommodities.SettingsSplittingChar = ';';
            this.extButtonCommodities.Size = new System.Drawing.Size(28, 28);
            this.extButtonCommodities.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonCommodities, "Filter on commodities to buy");
            this.extButtonCommodities.UseVisualStyleBackColor = true;
            // 
            // extButtonOutfitting
            // 
            this.extButtonOutfitting.Image = global::EDDiscovery.Icons.Controls.Outfitting;
            this.extButtonOutfitting.Location = new System.Drawing.Point(436, 2);
            this.extButtonOutfitting.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.extButtonOutfitting.Name = "extButtonOutfitting";
            this.extButtonOutfitting.SettingsSplittingChar = ';';
            this.extButtonOutfitting.Size = new System.Drawing.Size(28, 28);
            this.extButtonOutfitting.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonOutfitting, "Filter on outfitting");
            this.extButtonOutfitting.UseVisualStyleBackColor = true;
            // 
            // extButtonShipyard
            // 
            this.extButtonShipyard.Image = global::EDDiscovery.Icons.Controls.Shipyard;
            this.extButtonShipyard.Location = new System.Drawing.Point(472, 2);
            this.extButtonShipyard.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.extButtonShipyard.Name = "extButtonShipyard";
            this.extButtonShipyard.SettingsSplittingChar = ';';
            this.extButtonShipyard.Size = new System.Drawing.Size(28, 28);
            this.extButtonShipyard.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonShipyard, "Filter on ships for sale");
            this.extButtonShipyard.UseVisualStyleBackColor = true;
            // 
            // extButtonEconomy
            // 
            this.extButtonEconomy.Image = global::EDDiscovery.Icons.Controls.Economy;
            this.extButtonEconomy.Location = new System.Drawing.Point(508, 2);
            this.extButtonEconomy.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.extButtonEconomy.Name = "extButtonEconomy";
            this.extButtonEconomy.SettingsSplittingChar = ';';
            this.extButtonEconomy.Size = new System.Drawing.Size(28, 28);
            this.extButtonEconomy.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonEconomy, "Filter on economy");
            this.extButtonEconomy.UseVisualStyleBackColor = true;
            // 
            // extButtonServices
            // 
            this.extButtonServices.Image = global::EDDiscovery.Icons.Controls.People;
            this.extButtonServices.Location = new System.Drawing.Point(544, 2);
            this.extButtonServices.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.extButtonServices.Name = "extButtonServices";
            this.extButtonServices.SettingsSplittingChar = ';';
            this.extButtonServices.Size = new System.Drawing.Size(28, 28);
            this.extButtonServices.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonServices, "Filter on services offered");
            this.extButtonServices.UseVisualStyleBackColor = true;
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(580, 2);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.extCheckBoxWordWrap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 33;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = ((System.Drawing.Image)(resources.GetObject("buttonExtExcel.Image")));
            this.buttonExtExcel.Location = new System.Drawing.Point(616, 2);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 0);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Output grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            // 
            // SpanshStationsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.flowLayoutStarDistances);
            this.Name = "SpanshStationsUserControl";
            this.Size = new System.Drawing.Size(978, 777);
            this.flowLayoutStarDistances.ResumeLayout(false);
            this.flowLayoutStarDistances.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutStarDistances;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.ExtTextBoxAutoComplete extTextBoxAutoCompleteSystem;
        private System.Windows.Forms.Label labelMaxLs;
        private ExtendedControls.NumberBoxDouble valueBoxMaxLs;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private BaseUtils.DataGridViewColumnControl dataGridView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem viewOnSpanshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewMarketToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOutfittingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewShipyardToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButtonWithCheckedIconListBoxGroup extButtonType;
        private ExtendedControls.ExtButtonWithCheckedIconListBoxGroup extButtonCommodities;
        private ExtendedControls.ExtButtonWithCheckedIconListBoxGroup extButtonOutfitting;
        private ExtendedControls.ExtButtonWithCheckedIconListBoxGroup extButtonShipyard;
        private ExtendedControls.ExtButtonWithCheckedIconListBoxGroup extButtonEconomy;
        private ExtendedControls.ExtButtonWithCheckedIconListBoxGroup extButtonServices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLattitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLongitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHasMarket;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutfitting;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShipyard;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAllegiance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEconomy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGovernment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSmallPad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMediumPads;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLargePads;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
    }
}
