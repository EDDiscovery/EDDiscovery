namespace EDDiscovery.UserControls
{
    partial class UserControlCarrier
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
            ExtendedControls.TabStyleSquare tabStyleSquare1 = new ExtendedControls.TabStyleSquare();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extTabControlOverall = new ExtendedControls.ExtTabControl();
            this.tabPageOverall = new System.Windows.Forms.TabPage();
            this.imageControlOverall = new ExtendedControls.Controls.ImageControl();
            this.tabPageItinerary = new System.Windows.Forms.TabPage();
            this.dataViewScrollerPanelItinerary = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarItinerary = new ExtendedControls.ExtScrollBar();
            this.dataGridViewItinerary = new BaseUtils.DataGridViewColumnControl();
            this.colItinDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinJumpDist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinDistFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageFinances = new System.Windows.Forms.TabPage();
            this.extPanelDataGridViewScrollLedger = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarLedger = new ExtendedControls.ExtScrollBar();
            this.dataGridViewLedger = new BaseUtils.DataGridViewColumnControl();
            this.colLedgerDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerStarsystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerCredit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerDebit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelFinancesTop = new System.Windows.Forms.Panel();
            this.labelFCarrierBalance = new System.Windows.Forms.Label();
            this.labelFReserveBalance = new System.Windows.Forms.Label();
            this.labelFTaxShipyard = new System.Windows.Forms.Label();
            this.labelFAvailableBalance = new System.Windows.Forms.Label();
            this.labelFTaxRepair = new System.Windows.Forms.Label();
            this.labelFServicesCost = new System.Windows.Forms.Label();
            this.labelFCoreCost = new System.Windows.Forms.Label();
            this.labelFTaxPioneerSupplies = new System.Windows.Forms.Label();
            this.labelFTaxRefuel = new System.Windows.Forms.Label();
            this.labelFReservePercent = new System.Windows.Forms.Label();
            this.labelFTaxOutfitting = new System.Windows.Forms.Label();
            this.labelFTaxRearm = new System.Windows.Forms.Label();
            this.tabPageServices = new System.Windows.Forms.TabPage();
            this.imageControlScrollServices = new ExtendedControls.Controls.ImageControlScroll();
            this.imageControlServices = new ExtendedControls.Controls.ImageControl();
            this.extScrollBarServices = new ExtendedControls.ExtScrollBar();
            this.tabPagePacks = new System.Windows.Forms.TabPage();
            this.imageControlScrollPacks = new ExtendedControls.Controls.ImageControlScroll();
            this.imageControlPacks = new ExtendedControls.Controls.ImageControl();
            this.extScrollBarPacks = new ExtendedControls.ExtScrollBar();
            this.tabPageOrders = new System.Windows.Forms.TabPage();
            this.extPanelDataGridViewScrollOrders = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarOrders = new ExtendedControls.ExtScrollBar();
            this.dataGridViewOrders = new BaseUtils.DataGridViewColumnControl();
            this.colOrdersDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersCommodity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersPurchase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersSale = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersBlackmarket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extTabControlOverall.SuspendLayout();
            this.tabPageOverall.SuspendLayout();
            this.tabPageItinerary.SuspendLayout();
            this.dataViewScrollerPanelItinerary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItinerary)).BeginInit();
            this.tabPageFinances.SuspendLayout();
            this.extPanelDataGridViewScrollLedger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).BeginInit();
            this.panelFinancesTop.SuspendLayout();
            this.tabPageServices.SuspendLayout();
            this.imageControlScrollServices.SuspendLayout();
            this.tabPagePacks.SuspendLayout();
            this.imageControlScrollPacks.SuspendLayout();
            this.tabPageOrders.SuspendLayout();
            this.extPanelDataGridViewScrollOrders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).BeginInit();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // extTabControlOverall
            // 
            this.extTabControlOverall.AllowDragReorder = false;
            this.extTabControlOverall.Controls.Add(this.tabPageOverall);
            this.extTabControlOverall.Controls.Add(this.tabPageItinerary);
            this.extTabControlOverall.Controls.Add(this.tabPageFinances);
            this.extTabControlOverall.Controls.Add(this.tabPageServices);
            this.extTabControlOverall.Controls.Add(this.tabPagePacks);
            this.extTabControlOverall.Controls.Add(this.tabPageOrders);
            this.extTabControlOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extTabControlOverall.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extTabControlOverall.Location = new System.Drawing.Point(0, 0);
            this.extTabControlOverall.Name = "extTabControlOverall";
            this.extTabControlOverall.SelectedIndex = 0;
            this.extTabControlOverall.Size = new System.Drawing.Size(853, 572);
            this.extTabControlOverall.TabColorScaling = 0.5F;
            this.extTabControlOverall.TabControlBorderBrightColor = System.Drawing.Color.LightGray;
            this.extTabControlOverall.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.extTabControlOverall.TabDisabledScaling = 0.5F;
            this.extTabControlOverall.TabIndex = 0;
            this.extTabControlOverall.TabMouseOverColor = System.Drawing.Color.White;
            this.extTabControlOverall.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.extTabControlOverall.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.extTabControlOverall.TabOpaque = 100F;
            this.extTabControlOverall.TabSelectedColor = System.Drawing.Color.LightGray;
            this.extTabControlOverall.TabStyle = tabStyleSquare1;
            this.extTabControlOverall.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.extTabControlOverall.TextSelectedColor = System.Drawing.SystemColors.ControlText;
            // 
            // tabPageOverall
            // 
            this.tabPageOverall.Controls.Add(this.imageControlOverall);
            this.tabPageOverall.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverall.Name = "tabPageOverall";
            this.tabPageOverall.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverall.Size = new System.Drawing.Size(845, 546);
            this.tabPageOverall.TabIndex = 0;
            this.tabPageOverall.Text = "Overall";
            this.tabPageOverall.UseVisualStyleBackColor = true;
            // 
            // imageControlOverall
            // 
            this.imageControlOverall.BackColor = System.Drawing.Color.Black;
            this.imageControlOverall.BackgroundImage = global::EDDiscovery.Icons.Controls.FleetCarrier;
            this.imageControlOverall.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imageControlOverall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageControlOverall.ImageBackgroundColor = System.Drawing.Color.Transparent;
            this.imageControlOverall.ImageDepth = 1;
            this.imageControlOverall.ImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.imageControlOverall.ImageSize = new System.Drawing.Size(128, 128);
            this.imageControlOverall.ImageVisible = new bool[] {
        false};
            this.imageControlOverall.Location = new System.Drawing.Point(3, 3);
            this.imageControlOverall.Name = "imageControlOverall";
            this.imageControlOverall.Size = new System.Drawing.Size(839, 540);
            this.imageControlOverall.TabIndex = 0;
            this.imageControlOverall.Text = "imageControl1";
            // 
            // tabPageItinerary
            // 
            this.tabPageItinerary.Controls.Add(this.dataViewScrollerPanelItinerary);
            this.tabPageItinerary.Location = new System.Drawing.Point(4, 22);
            this.tabPageItinerary.Name = "tabPageItinerary";
            this.tabPageItinerary.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageItinerary.Size = new System.Drawing.Size(845, 546);
            this.tabPageItinerary.TabIndex = 1;
            this.tabPageItinerary.Text = "Itinerary";
            this.tabPageItinerary.UseVisualStyleBackColor = true;
            // 
            // dataViewScrollerPanelItinerary
            // 
            this.dataViewScrollerPanelItinerary.Controls.Add(this.extScrollBarItinerary);
            this.dataViewScrollerPanelItinerary.Controls.Add(this.dataGridViewItinerary);
            this.dataViewScrollerPanelItinerary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelItinerary.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelItinerary.Location = new System.Drawing.Point(3, 3);
            this.dataViewScrollerPanelItinerary.Margin = new System.Windows.Forms.Padding(2);
            this.dataViewScrollerPanelItinerary.Name = "dataViewScrollerPanelItinerary";
            this.dataViewScrollerPanelItinerary.Size = new System.Drawing.Size(839, 540);
            this.dataViewScrollerPanelItinerary.TabIndex = 2;
            this.dataViewScrollerPanelItinerary.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarItinerary
            // 
            this.extScrollBarItinerary.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarItinerary.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarItinerary.ArrowColorScaling = 0.5F;
            this.extScrollBarItinerary.ArrowDownDrawAngle = 270F;
            this.extScrollBarItinerary.ArrowUpDrawAngle = 90F;
            this.extScrollBarItinerary.BorderColor = System.Drawing.Color.White;
            this.extScrollBarItinerary.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarItinerary.HideScrollBar = false;
            this.extScrollBarItinerary.LargeChange = 0;
            this.extScrollBarItinerary.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarItinerary.Margin = new System.Windows.Forms.Padding(2);
            this.extScrollBarItinerary.Maximum = -1;
            this.extScrollBarItinerary.Minimum = 0;
            this.extScrollBarItinerary.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarItinerary.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarItinerary.Name = "extScrollBarItinerary";
            this.extScrollBarItinerary.Size = new System.Drawing.Size(16, 540);
            this.extScrollBarItinerary.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarItinerary.SmallChange = 1;
            this.extScrollBarItinerary.TabIndex = 1;
            this.extScrollBarItinerary.Text = "extScrollBar1";
            this.extScrollBarItinerary.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarItinerary.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarItinerary.ThumbColorScaling = 0.5F;
            this.extScrollBarItinerary.ThumbDrawAngle = 0F;
            this.extScrollBarItinerary.Value = -1;
            this.extScrollBarItinerary.ValueLimited = -1;
            // 
            // dataGridViewItinerary
            // 
            this.dataGridViewItinerary.AllowUserToAddRows = false;
            this.dataGridViewItinerary.AllowUserToDeleteRows = false;
            this.dataGridViewItinerary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewItinerary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewItinerary.ColumnReorder = true;
            this.dataGridViewItinerary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItinDate,
            this.colItinSystem,
            this.colItinBody,
            this.colItinX,
            this.colItinY,
            this.colItinZ,
            this.colItinJumpDist,
            this.colItinDistFrom,
            this.colItinInformation});
            this.dataGridViewItinerary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewItinerary.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewItinerary.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewItinerary.Name = "dataGridViewItinerary";
            this.dataGridViewItinerary.PerColumnWordWrapControl = true;
            this.dataGridViewItinerary.RowHeaderMenuStrip = null;
            this.dataGridViewItinerary.RowHeadersVisible = false;
            this.dataGridViewItinerary.RowHeadersWidth = 62;
            this.dataGridViewItinerary.RowTemplate.Height = 28;
            this.dataGridViewItinerary.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewItinerary.SingleRowSelect = true;
            this.dataGridViewItinerary.Size = new System.Drawing.Size(823, 540);
            this.dataGridViewItinerary.TabIndex = 0;
            this.dataGridViewItinerary.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewItinerary_SortCompare);
            // 
            // colItinDate
            // 
            this.colItinDate.FillWeight = 200.039F;
            this.colItinDate.HeaderText = "Date";
            this.colItinDate.MinimumWidth = 8;
            this.colItinDate.Name = "colItinDate";
            this.colItinDate.ReadOnly = true;
            // 
            // colItinSystem
            // 
            this.colItinSystem.FillWeight = 100.0195F;
            this.colItinSystem.HeaderText = "System";
            this.colItinSystem.MinimumWidth = 8;
            this.colItinSystem.Name = "colItinSystem";
            this.colItinSystem.ReadOnly = true;
            // 
            // colItinBody
            // 
            this.colItinBody.FillWeight = 100.0195F;
            this.colItinBody.HeaderText = "Body";
            this.colItinBody.MinimumWidth = 8;
            this.colItinBody.Name = "colItinBody";
            this.colItinBody.ReadOnly = true;
            // 
            // colItinX
            // 
            this.colItinX.FillWeight = 100.0195F;
            this.colItinX.HeaderText = "X";
            this.colItinX.MinimumWidth = 8;
            this.colItinX.Name = "colItinX";
            this.colItinX.ReadOnly = true;
            // 
            // colItinY
            // 
            this.colItinY.FillWeight = 130.0254F;
            this.colItinY.HeaderText = "Y";
            this.colItinY.MinimumWidth = 8;
            this.colItinY.Name = "colItinY";
            this.colItinY.ReadOnly = true;
            // 
            // colItinZ
            // 
            this.colItinZ.FillWeight = 100.0195F;
            this.colItinZ.HeaderText = "Z";
            this.colItinZ.MinimumWidth = 8;
            this.colItinZ.Name = "colItinZ";
            this.colItinZ.ReadOnly = true;
            // 
            // colItinJumpDist
            // 
            this.colItinJumpDist.FillWeight = 100.0195F;
            this.colItinJumpDist.HeaderText = "Jump Dist";
            this.colItinJumpDist.MinimumWidth = 8;
            this.colItinJumpDist.Name = "colItinJumpDist";
            // 
            // colItinDistFrom
            // 
            this.colItinDistFrom.FillWeight = 100.0195F;
            this.colItinDistFrom.HeaderText = "Dist From";
            this.colItinDistFrom.MinimumWidth = 8;
            this.colItinDistFrom.Name = "colItinDistFrom";
            this.colItinDistFrom.ReadOnly = true;
            // 
            // colItinInformation
            // 
            this.colItinInformation.FillWeight = 99.59389F;
            this.colItinInformation.HeaderText = "Information";
            this.colItinInformation.MinimumWidth = 8;
            this.colItinInformation.Name = "colItinInformation";
            // 
            // tabPageFinances
            // 
            this.tabPageFinances.Controls.Add(this.extPanelDataGridViewScrollLedger);
            this.tabPageFinances.Controls.Add(this.panelFinancesTop);
            this.tabPageFinances.Location = new System.Drawing.Point(4, 22);
            this.tabPageFinances.Name = "tabPageFinances";
            this.tabPageFinances.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFinances.Size = new System.Drawing.Size(845, 546);
            this.tabPageFinances.TabIndex = 2;
            this.tabPageFinances.Text = "Finances";
            this.tabPageFinances.UseVisualStyleBackColor = true;
            // 
            // extPanelDataGridViewScrollLedger
            // 
            this.extPanelDataGridViewScrollLedger.Controls.Add(this.extScrollBarLedger);
            this.extPanelDataGridViewScrollLedger.Controls.Add(this.dataGridViewLedger);
            this.extPanelDataGridViewScrollLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollLedger.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollLedger.Location = new System.Drawing.Point(3, 103);
            this.extPanelDataGridViewScrollLedger.Margin = new System.Windows.Forms.Padding(2);
            this.extPanelDataGridViewScrollLedger.Name = "extPanelDataGridViewScrollLedger";
            this.extPanelDataGridViewScrollLedger.Size = new System.Drawing.Size(839, 440);
            this.extPanelDataGridViewScrollLedger.TabIndex = 3;
            this.extPanelDataGridViewScrollLedger.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarLedger
            // 
            this.extScrollBarLedger.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarLedger.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarLedger.ArrowColorScaling = 0.5F;
            this.extScrollBarLedger.ArrowDownDrawAngle = 270F;
            this.extScrollBarLedger.ArrowUpDrawAngle = 90F;
            this.extScrollBarLedger.BorderColor = System.Drawing.Color.White;
            this.extScrollBarLedger.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarLedger.HideScrollBar = false;
            this.extScrollBarLedger.LargeChange = 0;
            this.extScrollBarLedger.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarLedger.Margin = new System.Windows.Forms.Padding(2);
            this.extScrollBarLedger.Maximum = -1;
            this.extScrollBarLedger.Minimum = 0;
            this.extScrollBarLedger.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarLedger.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarLedger.Name = "extScrollBarLedger";
            this.extScrollBarLedger.Size = new System.Drawing.Size(16, 440);
            this.extScrollBarLedger.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarLedger.SmallChange = 1;
            this.extScrollBarLedger.TabIndex = 1;
            this.extScrollBarLedger.Text = "extScrollBar1";
            this.extScrollBarLedger.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarLedger.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarLedger.ThumbColorScaling = 0.5F;
            this.extScrollBarLedger.ThumbDrawAngle = 0F;
            this.extScrollBarLedger.Value = -1;
            this.extScrollBarLedger.ValueLimited = -1;
            // 
            // dataGridViewLedger
            // 
            this.dataGridViewLedger.AllowUserToAddRows = false;
            this.dataGridViewLedger.AllowUserToDeleteRows = false;
            this.dataGridViewLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLedger.ColumnReorder = true;
            this.dataGridViewLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLedgerDate,
            this.colLedgerStarsystem,
            this.colLedgerBody,
            this.colLedgerEvent,
            this.colLedgerCredit,
            this.colLedgerDebit,
            this.colLedgerBalance,
            this.colLedgerNotes});
            this.dataGridViewLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewLedger.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewLedger.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewLedger.Name = "dataGridViewLedger";
            this.dataGridViewLedger.PerColumnWordWrapControl = true;
            this.dataGridViewLedger.RowHeaderMenuStrip = null;
            this.dataGridViewLedger.RowHeadersVisible = false;
            this.dataGridViewLedger.RowHeadersWidth = 62;
            this.dataGridViewLedger.RowTemplate.Height = 28;
            this.dataGridViewLedger.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewLedger.SingleRowSelect = true;
            this.dataGridViewLedger.Size = new System.Drawing.Size(823, 440);
            this.dataGridViewLedger.TabIndex = 0;
            this.dataGridViewLedger.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewLedger_SortCompare);
            // 
            // colLedgerDate
            // 
            this.colLedgerDate.HeaderText = "Date";
            this.colLedgerDate.MinimumWidth = 8;
            this.colLedgerDate.Name = "colLedgerDate";
            this.colLedgerDate.ReadOnly = true;
            // 
            // colLedgerStarsystem
            // 
            this.colLedgerStarsystem.HeaderText = "System";
            this.colLedgerStarsystem.Name = "colLedgerStarsystem";
            this.colLedgerStarsystem.ReadOnly = true;
            // 
            // colLedgerBody
            // 
            this.colLedgerBody.HeaderText = "Body";
            this.colLedgerBody.Name = "colLedgerBody";
            this.colLedgerBody.ReadOnly = true;
            // 
            // colLedgerEvent
            // 
            this.colLedgerEvent.HeaderText = "Event";
            this.colLedgerEvent.MinimumWidth = 8;
            this.colLedgerEvent.Name = "colLedgerEvent";
            this.colLedgerEvent.ReadOnly = true;
            // 
            // colLedgerCredit
            // 
            this.colLedgerCredit.HeaderText = "Credits";
            this.colLedgerCredit.Name = "colLedgerCredit";
            this.colLedgerCredit.ReadOnly = true;
            // 
            // colLedgerDebit
            // 
            this.colLedgerDebit.HeaderText = "Debits";
            this.colLedgerDebit.Name = "colLedgerDebit";
            this.colLedgerDebit.ReadOnly = true;
            // 
            // colLedgerBalance
            // 
            this.colLedgerBalance.HeaderText = "Balance cr";
            this.colLedgerBalance.MinimumWidth = 8;
            this.colLedgerBalance.Name = "colLedgerBalance";
            this.colLedgerBalance.ReadOnly = true;
            // 
            // colLedgerNotes
            // 
            this.colLedgerNotes.FillWeight = 150F;
            this.colLedgerNotes.HeaderText = "Notes";
            this.colLedgerNotes.Name = "colLedgerNotes";
            this.colLedgerNotes.ReadOnly = true;
            // 
            // panelFinancesTop
            // 
            this.panelFinancesTop.Controls.Add(this.labelFCarrierBalance);
            this.panelFinancesTop.Controls.Add(this.labelFReserveBalance);
            this.panelFinancesTop.Controls.Add(this.labelFTaxShipyard);
            this.panelFinancesTop.Controls.Add(this.labelFAvailableBalance);
            this.panelFinancesTop.Controls.Add(this.labelFTaxRepair);
            this.panelFinancesTop.Controls.Add(this.labelFServicesCost);
            this.panelFinancesTop.Controls.Add(this.labelFCoreCost);
            this.panelFinancesTop.Controls.Add(this.labelFTaxPioneerSupplies);
            this.panelFinancesTop.Controls.Add(this.labelFTaxRefuel);
            this.panelFinancesTop.Controls.Add(this.labelFReservePercent);
            this.panelFinancesTop.Controls.Add(this.labelFTaxOutfitting);
            this.panelFinancesTop.Controls.Add(this.labelFTaxRearm);
            this.panelFinancesTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFinancesTop.Location = new System.Drawing.Point(3, 3);
            this.panelFinancesTop.Name = "panelFinancesTop";
            this.panelFinancesTop.Size = new System.Drawing.Size(839, 100);
            this.panelFinancesTop.TabIndex = 4;
            // 
            // labelFCarrierBalance
            // 
            this.labelFCarrierBalance.AutoSize = true;
            this.labelFCarrierBalance.Location = new System.Drawing.Point(42, 7);
            this.labelFCarrierBalance.Name = "labelFCarrierBalance";
            this.labelFCarrierBalance.Size = new System.Drawing.Size(85, 13);
            this.labelFCarrierBalance.TabIndex = 0;
            this.labelFCarrierBalance.Text = "<code Balance>";
            // 
            // labelFReserveBalance
            // 
            this.labelFReserveBalance.AutoSize = true;
            this.labelFReserveBalance.Location = new System.Drawing.Point(42, 31);
            this.labelFReserveBalance.Name = "labelFReserveBalance";
            this.labelFReserveBalance.Size = new System.Drawing.Size(65, 13);
            this.labelFReserveBalance.TabIndex = 0;
            this.labelFReserveBalance.Text = "<code Res>";
            // 
            // labelFTaxShipyard
            // 
            this.labelFTaxShipyard.AutoSize = true;
            this.labelFTaxShipyard.Location = new System.Drawing.Point(294, 31);
            this.labelFTaxShipyard.Name = "labelFTaxShipyard";
            this.labelFTaxShipyard.Size = new System.Drawing.Size(88, 13);
            this.labelFTaxShipyard.TabIndex = 0;
            this.labelFTaxShipyard.Text = "<code Tax Ship>";
            // 
            // labelFAvailableBalance
            // 
            this.labelFAvailableBalance.AutoSize = true;
            this.labelFAvailableBalance.Location = new System.Drawing.Point(42, 55);
            this.labelFAvailableBalance.Name = "labelFAvailableBalance";
            this.labelFAvailableBalance.Size = new System.Drawing.Size(69, 13);
            this.labelFAvailableBalance.TabIndex = 0;
            this.labelFAvailableBalance.Text = "<code Avail>";
            // 
            // labelFTaxRepair
            // 
            this.labelFTaxRepair.AutoSize = true;
            this.labelFTaxRepair.Location = new System.Drawing.Point(294, 121);
            this.labelFTaxRepair.Name = "labelFTaxRepair";
            this.labelFTaxRepair.Size = new System.Drawing.Size(98, 13);
            this.labelFTaxRepair.TabIndex = 0;
            this.labelFTaxRepair.Text = "<code Tax Repair>";
            // 
            // labelFServicesCost
            // 
            this.labelFServicesCost.AutoSize = true;
            this.labelFServicesCost.Location = new System.Drawing.Point(175, 31);
            this.labelFServicesCost.Name = "labelFServicesCost";
            this.labelFServicesCost.Size = new System.Drawing.Size(111, 13);
            this.labelFServicesCost.TabIndex = 0;
            this.labelFServicesCost.Text = "<code Services Cost>";
            // 
            // labelFCoreCost
            // 
            this.labelFCoreCost.AutoSize = true;
            this.labelFCoreCost.Location = new System.Drawing.Point(175, 7);
            this.labelFCoreCost.Name = "labelFCoreCost";
            this.labelFCoreCost.Size = new System.Drawing.Size(97, 13);
            this.labelFCoreCost.TabIndex = 0;
            this.labelFCoreCost.Text = "<code Core Costs>";
            // 
            // labelFTaxPioneerSupplies
            // 
            this.labelFTaxPioneerSupplies.AutoSize = true;
            this.labelFTaxPioneerSupplies.Location = new System.Drawing.Point(294, 7);
            this.labelFTaxPioneerSupplies.Name = "labelFTaxPioneerSupplies";
            this.labelFTaxPioneerSupplies.Size = new System.Drawing.Size(74, 13);
            this.labelFTaxPioneerSupplies.TabIndex = 0;
            this.labelFTaxPioneerSupplies.Text = "<code Tax P>";
            // 
            // labelFTaxRefuel
            // 
            this.labelFTaxRefuel.AutoSize = true;
            this.labelFTaxRefuel.Location = new System.Drawing.Point(294, 98);
            this.labelFTaxRefuel.Name = "labelFTaxRefuel";
            this.labelFTaxRefuel.Size = new System.Drawing.Size(98, 13);
            this.labelFTaxRefuel.TabIndex = 0;
            this.labelFTaxRefuel.Text = "<code Tax Refuel>";
            // 
            // labelFReservePercent
            // 
            this.labelFReservePercent.AutoSize = true;
            this.labelFReservePercent.Location = new System.Drawing.Point(42, 74);
            this.labelFReservePercent.Name = "labelFReservePercent";
            this.labelFReservePercent.Size = new System.Drawing.Size(94, 13);
            this.labelFReservePercent.TabIndex = 0;
            this.labelFReservePercent.Text = "<code Reserve%>";
            // 
            // labelFTaxOutfitting
            // 
            this.labelFTaxOutfitting.AutoSize = true;
            this.labelFTaxOutfitting.Location = new System.Drawing.Point(294, 74);
            this.labelFTaxOutfitting.Name = "labelFTaxOutfitting";
            this.labelFTaxOutfitting.Size = new System.Drawing.Size(90, 13);
            this.labelFTaxOutfitting.TabIndex = 0;
            this.labelFTaxOutfitting.Text = "<code Tax outfit>";
            // 
            // labelFTaxRearm
            // 
            this.labelFTaxRearm.AutoSize = true;
            this.labelFTaxRearm.Location = new System.Drawing.Point(294, 55);
            this.labelFTaxRearm.Name = "labelFTaxRearm";
            this.labelFTaxRearm.Size = new System.Drawing.Size(98, 13);
            this.labelFTaxRearm.TabIndex = 0;
            this.labelFTaxRearm.Text = "<code Tax Rearm>";
            // 
            // tabPageServices
            // 
            this.tabPageServices.Controls.Add(this.imageControlScrollServices);
            this.tabPageServices.Location = new System.Drawing.Point(4, 22);
            this.tabPageServices.Name = "tabPageServices";
            this.tabPageServices.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServices.Size = new System.Drawing.Size(845, 546);
            this.tabPageServices.TabIndex = 3;
            this.tabPageServices.Text = "Services";
            this.tabPageServices.UseVisualStyleBackColor = true;
            // 
            // imageControlScrollServices
            // 
            this.imageControlScrollServices.Controls.Add(this.imageControlServices);
            this.imageControlScrollServices.Controls.Add(this.extScrollBarServices);
            this.imageControlScrollServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageControlScrollServices.ImageControlMinimumHeight = 0;
            this.imageControlScrollServices.Location = new System.Drawing.Point(3, 3);
            this.imageControlScrollServices.Name = "imageControlScrollServices";
            this.imageControlScrollServices.ScrollBarEnabled = true;
            this.imageControlScrollServices.Size = new System.Drawing.Size(839, 540);
            this.imageControlScrollServices.TabIndex = 0;
            this.imageControlScrollServices.VerticalScrollBarDockRight = true;
            // 
            // imageControlServices
            // 
            this.imageControlServices.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.imageControlServices.ImageBackgroundColor = System.Drawing.Color.Transparent;
            this.imageControlServices.ImageDepth = 1;
            this.imageControlServices.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.imageControlServices.ImageSize = new System.Drawing.Size(128, 128);
            this.imageControlServices.ImageVisible = new bool[] {
        false};
            this.imageControlServices.Location = new System.Drawing.Point(0, 0);
            this.imageControlServices.Name = "imageControlServices";
            this.imageControlServices.Size = new System.Drawing.Size(823, 217);
            this.imageControlServices.TabIndex = 1;
            this.imageControlServices.Text = "imageControl1";
            // 
            // extScrollBarServices
            // 
            this.extScrollBarServices.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarServices.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarServices.ArrowColorScaling = 0.5F;
            this.extScrollBarServices.ArrowDownDrawAngle = 270F;
            this.extScrollBarServices.ArrowUpDrawAngle = 90F;
            this.extScrollBarServices.BorderColor = System.Drawing.Color.White;
            this.extScrollBarServices.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarServices.HideScrollBar = false;
            this.extScrollBarServices.LargeChange = 540;
            this.extScrollBarServices.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarServices.Maximum = 216;
            this.extScrollBarServices.Minimum = 0;
            this.extScrollBarServices.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarServices.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarServices.Name = "extScrollBarServices";
            this.extScrollBarServices.Size = new System.Drawing.Size(16, 540);
            this.extScrollBarServices.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarServices.SmallChange = 1;
            this.extScrollBarServices.TabIndex = 0;
            this.extScrollBarServices.Text = "extScrollBar1";
            this.extScrollBarServices.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarServices.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarServices.ThumbColorScaling = 0.5F;
            this.extScrollBarServices.ThumbDrawAngle = 0F;
            this.extScrollBarServices.Value = 0;
            this.extScrollBarServices.ValueLimited = 0;
            // 
            // tabPagePacks
            // 
            this.tabPagePacks.Controls.Add(this.imageControlScrollPacks);
            this.tabPagePacks.Location = new System.Drawing.Point(4, 22);
            this.tabPagePacks.Name = "tabPagePacks";
            this.tabPagePacks.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePacks.Size = new System.Drawing.Size(845, 546);
            this.tabPagePacks.TabIndex = 6;
            this.tabPagePacks.Text = "Packs";
            this.tabPagePacks.UseVisualStyleBackColor = true;
            // 
            // imageControlScrollPacks
            // 
            this.imageControlScrollPacks.Controls.Add(this.imageControlPacks);
            this.imageControlScrollPacks.Controls.Add(this.extScrollBarPacks);
            this.imageControlScrollPacks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageControlScrollPacks.ImageControlMinimumHeight = 0;
            this.imageControlScrollPacks.Location = new System.Drawing.Point(3, 3);
            this.imageControlScrollPacks.Name = "imageControlScrollPacks";
            this.imageControlScrollPacks.ScrollBarEnabled = true;
            this.imageControlScrollPacks.Size = new System.Drawing.Size(839, 540);
            this.imageControlScrollPacks.TabIndex = 1;
            this.imageControlScrollPacks.VerticalScrollBarDockRight = true;
            // 
            // imageControlPacks
            // 
            this.imageControlPacks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.imageControlPacks.ImageBackgroundColor = System.Drawing.Color.Transparent;
            this.imageControlPacks.ImageDepth = 1;
            this.imageControlPacks.ImageLayout = System.Windows.Forms.ImageLayout.None;
            this.imageControlPacks.ImageSize = new System.Drawing.Size(128, 128);
            this.imageControlPacks.ImageVisible = new bool[] {
        false};
            this.imageControlPacks.Location = new System.Drawing.Point(0, 0);
            this.imageControlPacks.Name = "imageControlPacks";
            this.imageControlPacks.Size = new System.Drawing.Size(823, 217);
            this.imageControlPacks.TabIndex = 1;
            this.imageControlPacks.Text = "imageControl1";
            // 
            // extScrollBarPacks
            // 
            this.extScrollBarPacks.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarPacks.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarPacks.ArrowColorScaling = 0.5F;
            this.extScrollBarPacks.ArrowDownDrawAngle = 270F;
            this.extScrollBarPacks.ArrowUpDrawAngle = 90F;
            this.extScrollBarPacks.BorderColor = System.Drawing.Color.White;
            this.extScrollBarPacks.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarPacks.HideScrollBar = false;
            this.extScrollBarPacks.LargeChange = 540;
            this.extScrollBarPacks.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarPacks.Maximum = 216;
            this.extScrollBarPacks.Minimum = 0;
            this.extScrollBarPacks.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarPacks.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarPacks.Name = "extScrollBarPacks";
            this.extScrollBarPacks.Size = new System.Drawing.Size(16, 540);
            this.extScrollBarPacks.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarPacks.SmallChange = 1;
            this.extScrollBarPacks.TabIndex = 0;
            this.extScrollBarPacks.Text = "extScrollBar2";
            this.extScrollBarPacks.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarPacks.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarPacks.ThumbColorScaling = 0.5F;
            this.extScrollBarPacks.ThumbDrawAngle = 0F;
            this.extScrollBarPacks.Value = 0;
            this.extScrollBarPacks.ValueLimited = 0;
            // 
            // tabPageOrders
            // 
            this.tabPageOrders.Controls.Add(this.extPanelDataGridViewScrollOrders);
            this.tabPageOrders.Location = new System.Drawing.Point(4, 22);
            this.tabPageOrders.Name = "tabPageOrders";
            this.tabPageOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOrders.Size = new System.Drawing.Size(845, 546);
            this.tabPageOrders.TabIndex = 8;
            this.tabPageOrders.Text = "Orders";
            this.tabPageOrders.UseVisualStyleBackColor = true;
            // 
            // extPanelDataGridViewScrollOrders
            // 
            this.extPanelDataGridViewScrollOrders.Controls.Add(this.extScrollBarOrders);
            this.extPanelDataGridViewScrollOrders.Controls.Add(this.dataGridViewOrders);
            this.extPanelDataGridViewScrollOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollOrders.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollOrders.Location = new System.Drawing.Point(3, 3);
            this.extPanelDataGridViewScrollOrders.Margin = new System.Windows.Forms.Padding(2);
            this.extPanelDataGridViewScrollOrders.Name = "extPanelDataGridViewScrollOrders";
            this.extPanelDataGridViewScrollOrders.Size = new System.Drawing.Size(839, 540);
            this.extPanelDataGridViewScrollOrders.TabIndex = 3;
            this.extPanelDataGridViewScrollOrders.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarOrders
            // 
            this.extScrollBarOrders.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarOrders.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarOrders.ArrowColorScaling = 0.5F;
            this.extScrollBarOrders.ArrowDownDrawAngle = 270F;
            this.extScrollBarOrders.ArrowUpDrawAngle = 90F;
            this.extScrollBarOrders.BorderColor = System.Drawing.Color.White;
            this.extScrollBarOrders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarOrders.HideScrollBar = false;
            this.extScrollBarOrders.LargeChange = 0;
            this.extScrollBarOrders.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarOrders.Margin = new System.Windows.Forms.Padding(2);
            this.extScrollBarOrders.Maximum = -1;
            this.extScrollBarOrders.Minimum = 0;
            this.extScrollBarOrders.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarOrders.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarOrders.Name = "extScrollBarOrders";
            this.extScrollBarOrders.Size = new System.Drawing.Size(16, 540);
            this.extScrollBarOrders.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarOrders.SmallChange = 1;
            this.extScrollBarOrders.TabIndex = 1;
            this.extScrollBarOrders.Text = "extScrollBar1";
            this.extScrollBarOrders.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarOrders.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarOrders.ThumbColorScaling = 0.5F;
            this.extScrollBarOrders.ThumbDrawAngle = 0F;
            this.extScrollBarOrders.Value = -1;
            this.extScrollBarOrders.ValueLimited = -1;
            // 
            // dataGridViewOrders
            // 
            this.dataGridViewOrders.AllowUserToAddRows = false;
            this.dataGridViewOrders.AllowUserToDeleteRows = false;
            this.dataGridViewOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrders.ColumnReorder = true;
            this.dataGridViewOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrdersDate,
            this.colOrdersCommodity,
            this.colOrdersType,
            this.colOrdersGroup,
            this.colOrdersPurchase,
            this.colOrdersSale,
            this.colOrdersPrice,
            this.colOrdersBlackmarket});
            this.dataGridViewOrders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewOrders.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewOrders.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewOrders.Name = "dataGridViewOrders";
            this.dataGridViewOrders.PerColumnWordWrapControl = true;
            this.dataGridViewOrders.RowHeaderMenuStrip = null;
            this.dataGridViewOrders.RowHeadersVisible = false;
            this.dataGridViewOrders.RowHeadersWidth = 62;
            this.dataGridViewOrders.RowTemplate.Height = 28;
            this.dataGridViewOrders.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewOrders.SingleRowSelect = true;
            this.dataGridViewOrders.Size = new System.Drawing.Size(823, 540);
            this.dataGridViewOrders.TabIndex = 0;
            this.dataGridViewOrders.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewColumnControlOrders_SortCompare);
            // 
            // colOrdersDate
            // 
            this.colOrdersDate.HeaderText = "Date";
            this.colOrdersDate.MinimumWidth = 8;
            this.colOrdersDate.Name = "colOrdersDate";
            this.colOrdersDate.ReadOnly = true;
            // 
            // colOrdersCommodity
            // 
            this.colOrdersCommodity.FillWeight = 150F;
            this.colOrdersCommodity.HeaderText = "Commodity";
            this.colOrdersCommodity.MinimumWidth = 8;
            this.colOrdersCommodity.Name = "colOrdersCommodity";
            this.colOrdersCommodity.ReadOnly = true;
            // 
            // colOrdersType
            // 
            this.colOrdersType.HeaderText = "Type";
            this.colOrdersType.Name = "colOrdersType";
            this.colOrdersType.ReadOnly = true;
            // 
            // colOrdersGroup
            // 
            this.colOrdersGroup.HeaderText = "Group";
            this.colOrdersGroup.Name = "colOrdersGroup";
            this.colOrdersGroup.ReadOnly = true;
            // 
            // colOrdersPurchase
            // 
            this.colOrdersPurchase.FillWeight = 100.0195F;
            this.colOrdersPurchase.HeaderText = "Purchase";
            this.colOrdersPurchase.MinimumWidth = 8;
            this.colOrdersPurchase.Name = "colOrdersPurchase";
            this.colOrdersPurchase.ReadOnly = true;
            // 
            // colOrdersSale
            // 
            this.colOrdersSale.FillWeight = 100.0195F;
            this.colOrdersSale.HeaderText = "Sale";
            this.colOrdersSale.MinimumWidth = 8;
            this.colOrdersSale.Name = "colOrdersSale";
            this.colOrdersSale.ReadOnly = true;
            // 
            // colOrdersPrice
            // 
            this.colOrdersPrice.FillWeight = 130.0254F;
            this.colOrdersPrice.HeaderText = "Price";
            this.colOrdersPrice.MinimumWidth = 8;
            this.colOrdersPrice.Name = "colOrdersPrice";
            this.colOrdersPrice.ReadOnly = true;
            // 
            // colOrdersBlackmarket
            // 
            this.colOrdersBlackmarket.FillWeight = 75F;
            this.colOrdersBlackmarket.HeaderText = "Blackmarket";
            this.colOrdersBlackmarket.MinimumWidth = 8;
            this.colOrdersBlackmarket.Name = "colOrdersBlackmarket";
            this.colOrdersBlackmarket.ReadOnly = true;
            // 
            // UserControlCarrier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.extTabControlOverall);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserControlCarrier";
            this.Size = new System.Drawing.Size(853, 572);
            this.extTabControlOverall.ResumeLayout(false);
            this.tabPageOverall.ResumeLayout(false);
            this.tabPageItinerary.ResumeLayout(false);
            this.dataViewScrollerPanelItinerary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItinerary)).EndInit();
            this.tabPageFinances.ResumeLayout(false);
            this.extPanelDataGridViewScrollLedger.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).EndInit();
            this.panelFinancesTop.ResumeLayout(false);
            this.panelFinancesTop.PerformLayout();
            this.tabPageServices.ResumeLayout(false);
            this.imageControlScrollServices.ResumeLayout(false);
            this.tabPagePacks.ResumeLayout(false);
            this.imageControlScrollPacks.ResumeLayout(false);
            this.tabPageOrders.ResumeLayout(false);
            this.extPanelDataGridViewScrollOrders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOrders)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtTabControl extTabControlOverall;
        private System.Windows.Forms.TabPage tabPageOverall;
        private System.Windows.Forms.TabPage tabPageItinerary;
        private System.Windows.Forms.TabPage tabPageFinances;
        private System.Windows.Forms.TabPage tabPageServices;
        private System.Windows.Forms.TabPage tabPagePacks;
        private System.Windows.Forms.Label labelFCarrierBalance;
        private System.Windows.Forms.TabPage tabPageOrders;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelItinerary;
        private ExtendedControls.ExtScrollBar extScrollBarItinerary;
        private BaseUtils.DataGridViewColumnControl dataGridViewItinerary;
        private System.Windows.Forms.Label labelFTaxShipyard;
        private System.Windows.Forms.Label labelFTaxRepair;
        private System.Windows.Forms.Label labelFTaxRefuel;
        private System.Windows.Forms.Label labelFTaxOutfitting;
        private System.Windows.Forms.Label labelFTaxRearm;
        private System.Windows.Forms.Label labelFReservePercent;
        private System.Windows.Forms.Label labelFTaxPioneerSupplies;
        private System.Windows.Forms.Label labelFAvailableBalance;
        private System.Windows.Forms.Label labelFReserveBalance;
        private ExtendedControls.Controls.ImageControl imageControlOverall;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollLedger;
        private ExtendedControls.ExtScrollBar extScrollBarLedger;
        private BaseUtils.DataGridViewColumnControl dataGridViewLedger;
        private System.Windows.Forms.Panel panelFinancesTop;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinBody;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinJumpDist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinDistFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerStarsystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerBody;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerCredit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerDebit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerBalance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerNotes;
        private ExtendedControls.Controls.ImageControlScroll imageControlScrollServices;
        private ExtendedControls.ExtScrollBar extScrollBarServices;
        private ExtendedControls.Controls.ImageControl imageControlServices;
        private System.Windows.Forms.Label labelFServicesCost;
        private System.Windows.Forms.Label labelFCoreCost;
        private ExtendedControls.Controls.ImageControlScroll imageControlScrollPacks;
        private ExtendedControls.Controls.ImageControl imageControlPacks;
        private ExtendedControls.ExtScrollBar extScrollBarPacks;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollOrders;
        private ExtendedControls.ExtScrollBar extScrollBarOrders;
        private BaseUtils.DataGridViewColumnControl dataGridViewOrders;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersCommodity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersPurchase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersSale;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersBlackmarket;
    }
}
