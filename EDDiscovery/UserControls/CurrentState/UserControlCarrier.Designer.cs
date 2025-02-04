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
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition1 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            System.Windows.Forms.DataVisualization.Charting.ElementPosition elementPosition2 = new System.Windows.Forms.DataVisualization.Charting.ElementPosition();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extTabControl = new ExtendedControls.ExtTabControl();
            this.tabPageOverall = new System.Windows.Forms.TabPage();
            this.imageControlOverall = new ExtendedControls.Controls.ImageControl();
            this.tabPageItinerary = new System.Windows.Forms.TabPage();
            this.dataViewScrollerPanelItinerary = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarItinerary = new ExtendedControls.ExtScrollBar();
            this.dataGridViewItinerary = new EDDiscovery.UserControls.Search.DataGridViewStarResults();
            this.colItinDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinSystemAlphaInt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinBodyAlphaInt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinXNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinYNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinZNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinJumpDistNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinDistFromNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItinInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageFinances = new System.Windows.Forms.TabPage();
            this.splitContainerLedger = new System.Windows.Forms.SplitContainer();
            this.extPanelDataGridViewScrollLedger = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarLedger = new ExtendedControls.ExtScrollBar();
            this.dataGridViewLedger = new BaseUtils.DataGridViewColumnControl();
            this.colLedgerDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerStarsystemAlphaInt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerBodyAlphaInt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerEvent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerCreditNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerDebitNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerBalanceNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLedgerNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extChartLedger = new ExtendedControls.ExtSafeChart();
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
            this.colOrdersPurchaseNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersSaleNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersPriceNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrdersBlackmarket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageCAPI3 = new System.Windows.Forms.TabPage();
            this.extPanelDataGridViewScrollCAPIStats = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarCAPIStats = new ExtendedControls.ExtScrollBar();
            this.dataGridViewCAPIStats = new BaseUtils.DataGridViewColumnControl();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCAPI3 = new System.Windows.Forms.Panel();
            this.labelCAPICarrierBalance = new System.Windows.Forms.Label();
            this.labelCAPIDateTime1 = new System.Windows.Forms.Label();
            this.extButtonDoCAPI3 = new ExtendedControls.ExtButton();
            this.tabPageCAPI1 = new System.Windows.Forms.TabPage();
            this.splitContainerCAPI1 = new System.Windows.Forms.SplitContainer();
            this.extPanelDataGridViewScrollCAPIShips = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarCAPIShips = new ExtendedControls.ExtScrollBar();
            this.dataGridViewCAPIShips = new BaseUtils.DataGridViewColumnControl();
            this.colCAPIShipsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIShipsManu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIShipsPriceNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIShipsSpeedNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIShipsBoostNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIShipsMassNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIShipsLandingPadNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extPanelDataGridViewCAPIModules = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarCAPIModules = new ExtendedControls.ExtScrollBar();
            this.dataGridViewCAPIModules = new BaseUtils.DataGridViewColumnControl();
            this.colCAPIModulesName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIModulesCat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIModulesMassNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIModulesPowerNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIModulesCostNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIModulesStockNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPIModulesInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCAPI1 = new System.Windows.Forms.Panel();
            this.labelCAPIDateTime2 = new System.Windows.Forms.Label();
            this.extButtonDoCAPI1 = new ExtendedControls.ExtButton();
            this.tabPageCAPI2 = new System.Windows.Forms.TabPage();
            this.splitContainerCAPI2 = new System.Windows.Forms.SplitContainer();
            this.extPanelDataGridViewScrollCAPICargo = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarCAPICargo = new ExtendedControls.ExtScrollBar();
            this.dataGridViewCAPICargo = new BaseUtils.DataGridViewColumnControl();
            this.colCAPICargoCommodity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPICargoType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPICargoGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPICargoQuantityNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPICargoPriceNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPICargoStolen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extPanelDataGridViewScrollCAPILocker = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarCAPILocker = new ExtendedControls.ExtScrollBar();
            this.dataGridViewCAPILocker = new BaseUtils.DataGridViewColumnControl();
            this.colCAPILockerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPILockerType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCAPILockerQuantityNumeric = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelCAPI2 = new System.Windows.Forms.Panel();
            this.labelCAPIDateTime3 = new System.Windows.Forms.Label();
            this.extButtonDoCAPI2 = new ExtendedControls.ExtButton();
            this.extTabControl.SuspendLayout();
            this.tabPageOverall.SuspendLayout();
            this.tabPageItinerary.SuspendLayout();
            this.dataViewScrollerPanelItinerary.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItinerary)).BeginInit();
            this.tabPageFinances.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLedger)).BeginInit();
            this.splitContainerLedger.Panel1.SuspendLayout();
            this.splitContainerLedger.Panel2.SuspendLayout();
            this.splitContainerLedger.SuspendLayout();
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
            this.tabPageCAPI3.SuspendLayout();
            this.extPanelDataGridViewScrollCAPIStats.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPIStats)).BeginInit();
            this.panelCAPI3.SuspendLayout();
            this.tabPageCAPI1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCAPI1)).BeginInit();
            this.splitContainerCAPI1.Panel1.SuspendLayout();
            this.splitContainerCAPI1.Panel2.SuspendLayout();
            this.splitContainerCAPI1.SuspendLayout();
            this.extPanelDataGridViewScrollCAPIShips.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPIShips)).BeginInit();
            this.extPanelDataGridViewCAPIModules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPIModules)).BeginInit();
            this.panelCAPI1.SuspendLayout();
            this.tabPageCAPI2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCAPI2)).BeginInit();
            this.splitContainerCAPI2.Panel1.SuspendLayout();
            this.splitContainerCAPI2.Panel2.SuspendLayout();
            this.splitContainerCAPI2.SuspendLayout();
            this.extPanelDataGridViewScrollCAPICargo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPICargo)).BeginInit();
            this.extPanelDataGridViewScrollCAPILocker.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPILocker)).BeginInit();
            this.panelCAPI2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // extTabControl
            // 
            this.extTabControl.AllowDragReorder = false;
            this.extTabControl.Controls.Add(this.tabPageOverall);
            this.extTabControl.Controls.Add(this.tabPageItinerary);
            this.extTabControl.Controls.Add(this.tabPageFinances);
            this.extTabControl.Controls.Add(this.tabPageServices);
            this.extTabControl.Controls.Add(this.tabPagePacks);
            this.extTabControl.Controls.Add(this.tabPageOrders);
            this.extTabControl.Controls.Add(this.tabPageCAPI3);
            this.extTabControl.Controls.Add(this.tabPageCAPI1);
            this.extTabControl.Controls.Add(this.tabPageCAPI2);
            this.extTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extTabControl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extTabControl.Location = new System.Drawing.Point(0, 0);
            this.extTabControl.Name = "extTabControl";
            this.extTabControl.SelectedIndex = 0;
            this.extTabControl.Size = new System.Drawing.Size(853, 572);
            this.extTabControl.TabColorScaling = 0.5F;
            this.extTabControl.TabControlBorderBrightColor = System.Drawing.Color.LightGray;
            this.extTabControl.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.extTabControl.TabDisabledScaling = 0.5F;
            this.extTabControl.TabIndex = 0;
            this.extTabControl.TabMouseOverColor = System.Drawing.Color.White;
            this.extTabControl.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.extTabControl.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.extTabControl.TabSelectedColor = System.Drawing.Color.LightGray;
            this.extTabControl.TabStyle = tabStyleSquare1;
            this.extTabControl.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.extTabControl.TextSelectedColor = System.Drawing.SystemColors.ControlText;
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
            this.extScrollBarItinerary.AlwaysHideScrollBar = false;
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
            this.dataGridViewItinerary.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewItinerary.AllowUserToAddRows = false;
            this.dataGridViewItinerary.AllowUserToDeleteRows = false;
            this.dataGridViewItinerary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewItinerary.AutoSortByColumnName = true;
            this.dataGridViewItinerary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewItinerary.ColumnReorder = true;
            this.dataGridViewItinerary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colItinDate,
            this.colItinSystemAlphaInt,
            this.colItinBodyAlphaInt,
            this.colItinXNumeric,
            this.colItinYNumeric,
            this.colItinZNumeric,
            this.colItinJumpDistNumeric,
            this.colItinDistFromNumeric,
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
            this.dataGridViewItinerary.WebLookup = EliteDangerousCore.WebExternalDataLookup.None;
            // 
            // colItinDate
            // 
            this.colItinDate.FillWeight = 200.039F;
            this.colItinDate.HeaderText = "Date";
            this.colItinDate.MinimumWidth = 8;
            this.colItinDate.Name = "colItinDate";
            this.colItinDate.ReadOnly = true;
            // 
            // colItinSystemAlphaInt
            // 
            this.colItinSystemAlphaInt.FillWeight = 100.0195F;
            this.colItinSystemAlphaInt.HeaderText = "System";
            this.colItinSystemAlphaInt.MinimumWidth = 8;
            this.colItinSystemAlphaInt.Name = "colItinSystemAlphaInt";
            this.colItinSystemAlphaInt.ReadOnly = true;
            // 
            // colItinBodyAlphaInt
            // 
            this.colItinBodyAlphaInt.FillWeight = 100.0195F;
            this.colItinBodyAlphaInt.HeaderText = "Body";
            this.colItinBodyAlphaInt.MinimumWidth = 8;
            this.colItinBodyAlphaInt.Name = "colItinBodyAlphaInt";
            this.colItinBodyAlphaInt.ReadOnly = true;
            // 
            // colItinXNumeric
            // 
            this.colItinXNumeric.FillWeight = 100.0195F;
            this.colItinXNumeric.HeaderText = "X";
            this.colItinXNumeric.MinimumWidth = 8;
            this.colItinXNumeric.Name = "colItinXNumeric";
            this.colItinXNumeric.ReadOnly = true;
            // 
            // colItinYNumeric
            // 
            this.colItinYNumeric.FillWeight = 130.0254F;
            this.colItinYNumeric.HeaderText = "Y";
            this.colItinYNumeric.MinimumWidth = 8;
            this.colItinYNumeric.Name = "colItinYNumeric";
            this.colItinYNumeric.ReadOnly = true;
            // 
            // colItinZNumeric
            // 
            this.colItinZNumeric.FillWeight = 100.0195F;
            this.colItinZNumeric.HeaderText = "Z";
            this.colItinZNumeric.MinimumWidth = 8;
            this.colItinZNumeric.Name = "colItinZNumeric";
            this.colItinZNumeric.ReadOnly = true;
            // 
            // colItinJumpDistNumeric
            // 
            this.colItinJumpDistNumeric.FillWeight = 100.0195F;
            this.colItinJumpDistNumeric.HeaderText = "Jump Dist";
            this.colItinJumpDistNumeric.MinimumWidth = 8;
            this.colItinJumpDistNumeric.Name = "colItinJumpDistNumeric";
            // 
            // colItinDistFromNumeric
            // 
            this.colItinDistFromNumeric.FillWeight = 100.0195F;
            this.colItinDistFromNumeric.HeaderText = "Dist From";
            this.colItinDistFromNumeric.MinimumWidth = 8;
            this.colItinDistFromNumeric.Name = "colItinDistFromNumeric";
            this.colItinDistFromNumeric.ReadOnly = true;
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
            this.tabPageFinances.Controls.Add(this.splitContainerLedger);
            this.tabPageFinances.Controls.Add(this.panelFinancesTop);
            this.tabPageFinances.Location = new System.Drawing.Point(4, 22);
            this.tabPageFinances.Name = "tabPageFinances";
            this.tabPageFinances.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFinances.Size = new System.Drawing.Size(845, 546);
            this.tabPageFinances.TabIndex = 2;
            this.tabPageFinances.Text = "Finances";
            this.tabPageFinances.UseVisualStyleBackColor = true;
            // 
            // splitContainerLedger
            // 
            this.splitContainerLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLedger.Location = new System.Drawing.Point(3, 103);
            this.splitContainerLedger.Name = "splitContainerLedger";
            this.splitContainerLedger.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLedger.Panel1
            // 
            this.splitContainerLedger.Panel1.Controls.Add(this.extPanelDataGridViewScrollLedger);
            // 
            // splitContainerLedger.Panel2
            // 
            this.splitContainerLedger.Panel2.Controls.Add(this.extChartLedger);
            this.splitContainerLedger.Size = new System.Drawing.Size(839, 440);
            this.splitContainerLedger.SplitterDistance = 220;
            this.splitContainerLedger.TabIndex = 2;
            // 
            // extPanelDataGridViewScrollLedger
            // 
            this.extPanelDataGridViewScrollLedger.Controls.Add(this.extScrollBarLedger);
            this.extPanelDataGridViewScrollLedger.Controls.Add(this.dataGridViewLedger);
            this.extPanelDataGridViewScrollLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollLedger.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollLedger.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollLedger.Margin = new System.Windows.Forms.Padding(2);
            this.extPanelDataGridViewScrollLedger.Name = "extPanelDataGridViewScrollLedger";
            this.extPanelDataGridViewScrollLedger.Size = new System.Drawing.Size(839, 220);
            this.extPanelDataGridViewScrollLedger.TabIndex = 3;
            this.extPanelDataGridViewScrollLedger.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarLedger
            // 
            this.extScrollBarLedger.AlwaysHideScrollBar = false;
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
            this.extScrollBarLedger.Size = new System.Drawing.Size(16, 220);
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
            this.dataGridViewLedger.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewLedger.AllowUserToAddRows = false;
            this.dataGridViewLedger.AllowUserToDeleteRows = false;
            this.dataGridViewLedger.AllowUserToResizeRows = false;
            this.dataGridViewLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLedger.AutoSortByColumnName = true;
            this.dataGridViewLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLedger.ColumnReorder = true;
            this.dataGridViewLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLedgerDate,
            this.colLedgerStarsystemAlphaInt,
            this.colLedgerBodyAlphaInt,
            this.colLedgerEvent,
            this.colLedgerCreditNumeric,
            this.colLedgerDebitNumeric,
            this.colLedgerBalanceNumeric,
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
            this.dataGridViewLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewLedger.SingleRowSelect = true;
            this.dataGridViewLedger.Size = new System.Drawing.Size(823, 220);
            this.dataGridViewLedger.TabIndex = 0;
            this.dataGridViewLedger.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewLedger_CellClick);
            // 
            // colLedgerDate
            // 
            this.colLedgerDate.HeaderText = "Date";
            this.colLedgerDate.MinimumWidth = 8;
            this.colLedgerDate.Name = "colLedgerDate";
            this.colLedgerDate.ReadOnly = true;
            // 
            // colLedgerStarsystemAlphaInt
            // 
            this.colLedgerStarsystemAlphaInt.HeaderText = "System";
            this.colLedgerStarsystemAlphaInt.Name = "colLedgerStarsystemAlphaInt";
            this.colLedgerStarsystemAlphaInt.ReadOnly = true;
            // 
            // colLedgerBodyAlphaInt
            // 
            this.colLedgerBodyAlphaInt.HeaderText = "Body";
            this.colLedgerBodyAlphaInt.Name = "colLedgerBodyAlphaInt";
            this.colLedgerBodyAlphaInt.ReadOnly = true;
            // 
            // colLedgerEvent
            // 
            this.colLedgerEvent.HeaderText = "Event";
            this.colLedgerEvent.MinimumWidth = 8;
            this.colLedgerEvent.Name = "colLedgerEvent";
            this.colLedgerEvent.ReadOnly = true;
            // 
            // colLedgerCreditNumeric
            // 
            this.colLedgerCreditNumeric.HeaderText = "Credits";
            this.colLedgerCreditNumeric.Name = "colLedgerCreditNumeric";
            this.colLedgerCreditNumeric.ReadOnly = true;
            // 
            // colLedgerDebitNumeric
            // 
            this.colLedgerDebitNumeric.HeaderText = "Debits";
            this.colLedgerDebitNumeric.Name = "colLedgerDebitNumeric";
            this.colLedgerDebitNumeric.ReadOnly = true;
            // 
            // colLedgerBalanceNumeric
            // 
            this.colLedgerBalanceNumeric.HeaderText = "Balance cr";
            this.colLedgerBalanceNumeric.MinimumWidth = 8;
            this.colLedgerBalanceNumeric.Name = "colLedgerBalanceNumeric";
            this.colLedgerBalanceNumeric.ReadOnly = true;
            // 
            // colLedgerNotes
            // 
            this.colLedgerNotes.FillWeight = 150F;
            this.colLedgerNotes.HeaderText = "Notes";
            this.colLedgerNotes.Name = "colLedgerNotes";
            this.colLedgerNotes.ReadOnly = true;
            // 
            // extChartLedger
            // 
            this.extChartLedger.AutoScaleYAddedPercent = 5D;
            this.extChartLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extChartLedger.LeftArrowEnable = true;
            this.extChartLedger.LeftArrowPosition = elementPosition1;
            this.extChartLedger.Location = new System.Drawing.Point(0, 0);
            this.extChartLedger.Name = "extChartLedger";
            this.extChartLedger.RightArrowEnable = true;
            this.extChartLedger.RightArrowPosition = elementPosition2;
            this.extChartLedger.Size = new System.Drawing.Size(839, 216);
            this.extChartLedger.TabIndex = 0;
            this.extChartLedger.ZoomMouseWheelXMinimumInterval = 5D;
            this.extChartLedger.ZoomMouseWheelXZoomFactor = 1.5D;
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
            // 
            // extScrollBarServices
            // 
            this.extScrollBarServices.AlwaysHideScrollBar = false;
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
            // 
            // extScrollBarPacks
            // 
            this.extScrollBarPacks.AlwaysHideScrollBar = false;
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
            this.extScrollBarOrders.AlwaysHideScrollBar = false;
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
            this.dataGridViewOrders.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewOrders.AllowUserToAddRows = false;
            this.dataGridViewOrders.AllowUserToDeleteRows = false;
            this.dataGridViewOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOrders.AutoSortByColumnName = true;
            this.dataGridViewOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOrders.ColumnReorder = true;
            this.dataGridViewOrders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colOrdersDate,
            this.colOrdersCommodity,
            this.colOrdersType,
            this.colOrdersGroup,
            this.colOrdersPurchaseNumeric,
            this.colOrdersSaleNumeric,
            this.colOrdersPriceNumeric,
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
            // colOrdersPurchaseNumeric
            // 
            this.colOrdersPurchaseNumeric.FillWeight = 100.0195F;
            this.colOrdersPurchaseNumeric.HeaderText = "Purchase";
            this.colOrdersPurchaseNumeric.MinimumWidth = 8;
            this.colOrdersPurchaseNumeric.Name = "colOrdersPurchaseNumeric";
            this.colOrdersPurchaseNumeric.ReadOnly = true;
            // 
            // colOrdersSaleNumeric
            // 
            this.colOrdersSaleNumeric.FillWeight = 100.0195F;
            this.colOrdersSaleNumeric.HeaderText = "Sale";
            this.colOrdersSaleNumeric.MinimumWidth = 8;
            this.colOrdersSaleNumeric.Name = "colOrdersSaleNumeric";
            this.colOrdersSaleNumeric.ReadOnly = true;
            // 
            // colOrdersPriceNumeric
            // 
            this.colOrdersPriceNumeric.FillWeight = 130.0254F;
            this.colOrdersPriceNumeric.HeaderText = "Price";
            this.colOrdersPriceNumeric.MinimumWidth = 8;
            this.colOrdersPriceNumeric.Name = "colOrdersPriceNumeric";
            this.colOrdersPriceNumeric.ReadOnly = true;
            // 
            // colOrdersBlackmarket
            // 
            this.colOrdersBlackmarket.FillWeight = 75F;
            this.colOrdersBlackmarket.HeaderText = "Blackmarket";
            this.colOrdersBlackmarket.MinimumWidth = 8;
            this.colOrdersBlackmarket.Name = "colOrdersBlackmarket";
            this.colOrdersBlackmarket.ReadOnly = true;
            // 
            // tabPageCAPI3
            // 
            this.tabPageCAPI3.Controls.Add(this.extPanelDataGridViewScrollCAPIStats);
            this.tabPageCAPI3.Controls.Add(this.panelCAPI3);
            this.tabPageCAPI3.Location = new System.Drawing.Point(4, 22);
            this.tabPageCAPI3.Name = "tabPageCAPI3";
            this.tabPageCAPI3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCAPI3.Size = new System.Drawing.Size(845, 546);
            this.tabPageCAPI3.TabIndex = 11;
            this.tabPageCAPI3.Text = "CAPI Stats";
            this.tabPageCAPI3.UseVisualStyleBackColor = true;
            // 
            // extPanelDataGridViewScrollCAPIStats
            // 
            this.extPanelDataGridViewScrollCAPIStats.Controls.Add(this.extScrollBarCAPIStats);
            this.extPanelDataGridViewScrollCAPIStats.Controls.Add(this.dataGridViewCAPIStats);
            this.extPanelDataGridViewScrollCAPIStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollCAPIStats.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollCAPIStats.Location = new System.Drawing.Point(3, 35);
            this.extPanelDataGridViewScrollCAPIStats.Margin = new System.Windows.Forms.Padding(2);
            this.extPanelDataGridViewScrollCAPIStats.Name = "extPanelDataGridViewScrollCAPIStats";
            this.extPanelDataGridViewScrollCAPIStats.Size = new System.Drawing.Size(839, 508);
            this.extPanelDataGridViewScrollCAPIStats.TabIndex = 5;
            this.extPanelDataGridViewScrollCAPIStats.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarCAPIStats
            // 
            this.extScrollBarCAPIStats.AlwaysHideScrollBar = false;
            this.extScrollBarCAPIStats.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarCAPIStats.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarCAPIStats.ArrowColorScaling = 0.5F;
            this.extScrollBarCAPIStats.ArrowDownDrawAngle = 270F;
            this.extScrollBarCAPIStats.ArrowUpDrawAngle = 90F;
            this.extScrollBarCAPIStats.BorderColor = System.Drawing.Color.White;
            this.extScrollBarCAPIStats.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarCAPIStats.HideScrollBar = false;
            this.extScrollBarCAPIStats.LargeChange = 0;
            this.extScrollBarCAPIStats.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarCAPIStats.Margin = new System.Windows.Forms.Padding(2);
            this.extScrollBarCAPIStats.Maximum = -1;
            this.extScrollBarCAPIStats.Minimum = 0;
            this.extScrollBarCAPIStats.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarCAPIStats.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarCAPIStats.Name = "extScrollBarCAPIStats";
            this.extScrollBarCAPIStats.Size = new System.Drawing.Size(16, 508);
            this.extScrollBarCAPIStats.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarCAPIStats.SmallChange = 1;
            this.extScrollBarCAPIStats.TabIndex = 1;
            this.extScrollBarCAPIStats.Text = "extScrollBar1";
            this.extScrollBarCAPIStats.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarCAPIStats.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarCAPIStats.ThumbColorScaling = 0.5F;
            this.extScrollBarCAPIStats.ThumbDrawAngle = 0F;
            this.extScrollBarCAPIStats.Value = -1;
            this.extScrollBarCAPIStats.ValueLimited = -1;
            // 
            // dataGridViewCAPIStats
            // 
            this.dataGridViewCAPIStats.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewCAPIStats.AllowUserToAddRows = false;
            this.dataGridViewCAPIStats.AllowUserToDeleteRows = false;
            this.dataGridViewCAPIStats.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCAPIStats.AutoSortByColumnName = true;
            this.dataGridViewCAPIStats.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCAPIStats.ColumnReorder = true;
            this.dataGridViewCAPIStats.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumnValue});
            this.dataGridViewCAPIStats.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCAPIStats.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCAPIStats.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCAPIStats.Name = "dataGridViewCAPIStats";
            this.dataGridViewCAPIStats.PerColumnWordWrapControl = true;
            this.dataGridViewCAPIStats.RowHeaderMenuStrip = null;
            this.dataGridViewCAPIStats.RowHeadersVisible = false;
            this.dataGridViewCAPIStats.RowHeadersWidth = 62;
            this.dataGridViewCAPIStats.RowTemplate.Height = 28;
            this.dataGridViewCAPIStats.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCAPIStats.SingleRowSelect = true;
            this.dataGridViewCAPIStats.Size = new System.Drawing.Size(823, 508);
            this.dataGridViewCAPIStats.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 50F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Name";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnValue
            // 
            this.dataGridViewTextBoxColumnValue.HeaderText = "Value";
            this.dataGridViewTextBoxColumnValue.MinimumWidth = 8;
            this.dataGridViewTextBoxColumnValue.Name = "dataGridViewTextBoxColumnValue";
            this.dataGridViewTextBoxColumnValue.ReadOnly = true;
            // 
            // panelCAPI3
            // 
            this.panelCAPI3.Controls.Add(this.labelCAPICarrierBalance);
            this.panelCAPI3.Controls.Add(this.labelCAPIDateTime1);
            this.panelCAPI3.Controls.Add(this.extButtonDoCAPI3);
            this.panelCAPI3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCAPI3.Location = new System.Drawing.Point(3, 3);
            this.panelCAPI3.Name = "panelCAPI3";
            this.panelCAPI3.Size = new System.Drawing.Size(839, 32);
            this.panelCAPI3.TabIndex = 6;
            // 
            // labelCAPICarrierBalance
            // 
            this.labelCAPICarrierBalance.AutoSize = true;
            this.labelCAPICarrierBalance.Location = new System.Drawing.Point(681, 9);
            this.labelCAPICarrierBalance.Name = "labelCAPICarrierBalance";
            this.labelCAPICarrierBalance.Size = new System.Drawing.Size(85, 13);
            this.labelCAPICarrierBalance.TabIndex = 1;
            this.labelCAPICarrierBalance.Text = "<code Balance>";
            // 
            // labelCAPIDateTime1
            // 
            this.labelCAPIDateTime1.AutoSize = true;
            this.labelCAPIDateTime1.Location = new System.Drawing.Point(33, 9);
            this.labelCAPIDateTime1.Name = "labelCAPIDateTime1";
            this.labelCAPIDateTime1.Size = new System.Drawing.Size(92, 13);
            this.labelCAPIDateTime1.TabIndex = 1;
            this.labelCAPIDateTime1.Text = "<code DateTime>";
            // 
            // extButtonDoCAPI3
            // 
            this.extButtonDoCAPI3.Image = global::EDDiscovery.Icons.Controls.Refresh;
            this.extButtonDoCAPI3.Location = new System.Drawing.Point(3, 3);
            this.extButtonDoCAPI3.Name = "extButtonDoCAPI3";
            this.extButtonDoCAPI3.Size = new System.Drawing.Size(24, 24);
            this.extButtonDoCAPI3.TabIndex = 0;
            this.extButtonDoCAPI3.UseVisualStyleBackColor = true;
            this.extButtonDoCAPI3.Click += new System.EventHandler(this.extButtonDoCAPI1_Click);
            // 
            // tabPageCAPI1
            // 
            this.tabPageCAPI1.Controls.Add(this.splitContainerCAPI1);
            this.tabPageCAPI1.Controls.Add(this.panelCAPI1);
            this.tabPageCAPI1.Location = new System.Drawing.Point(4, 22);
            this.tabPageCAPI1.Name = "tabPageCAPI1";
            this.tabPageCAPI1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCAPI1.Size = new System.Drawing.Size(845, 546);
            this.tabPageCAPI1.TabIndex = 9;
            this.tabPageCAPI1.Text = "CAPI Ships/Modules";
            this.tabPageCAPI1.UseVisualStyleBackColor = true;
            // 
            // splitContainerCAPI1
            // 
            this.splitContainerCAPI1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCAPI1.Location = new System.Drawing.Point(3, 35);
            this.splitContainerCAPI1.Name = "splitContainerCAPI1";
            this.splitContainerCAPI1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerCAPI1.Panel1
            // 
            this.splitContainerCAPI1.Panel1.Controls.Add(this.extPanelDataGridViewScrollCAPIShips);
            // 
            // splitContainerCAPI1.Panel2
            // 
            this.splitContainerCAPI1.Panel2.Controls.Add(this.extPanelDataGridViewCAPIModules);
            this.splitContainerCAPI1.Size = new System.Drawing.Size(839, 508);
            this.splitContainerCAPI1.SplitterDistance = 284;
            this.splitContainerCAPI1.TabIndex = 2;
            // 
            // extPanelDataGridViewScrollCAPIShips
            // 
            this.extPanelDataGridViewScrollCAPIShips.Controls.Add(this.extScrollBarCAPIShips);
            this.extPanelDataGridViewScrollCAPIShips.Controls.Add(this.dataGridViewCAPIShips);
            this.extPanelDataGridViewScrollCAPIShips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollCAPIShips.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollCAPIShips.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollCAPIShips.Margin = new System.Windows.Forms.Padding(2);
            this.extPanelDataGridViewScrollCAPIShips.Name = "extPanelDataGridViewScrollCAPIShips";
            this.extPanelDataGridViewScrollCAPIShips.Size = new System.Drawing.Size(839, 284);
            this.extPanelDataGridViewScrollCAPIShips.TabIndex = 4;
            this.extPanelDataGridViewScrollCAPIShips.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarCAPIShips
            // 
            this.extScrollBarCAPIShips.AlwaysHideScrollBar = false;
            this.extScrollBarCAPIShips.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarCAPIShips.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarCAPIShips.ArrowColorScaling = 0.5F;
            this.extScrollBarCAPIShips.ArrowDownDrawAngle = 270F;
            this.extScrollBarCAPIShips.ArrowUpDrawAngle = 90F;
            this.extScrollBarCAPIShips.BorderColor = System.Drawing.Color.White;
            this.extScrollBarCAPIShips.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarCAPIShips.HideScrollBar = false;
            this.extScrollBarCAPIShips.LargeChange = 0;
            this.extScrollBarCAPIShips.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarCAPIShips.Margin = new System.Windows.Forms.Padding(2);
            this.extScrollBarCAPIShips.Maximum = -1;
            this.extScrollBarCAPIShips.Minimum = 0;
            this.extScrollBarCAPIShips.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarCAPIShips.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarCAPIShips.Name = "extScrollBarCAPIShips";
            this.extScrollBarCAPIShips.Size = new System.Drawing.Size(16, 284);
            this.extScrollBarCAPIShips.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarCAPIShips.SmallChange = 1;
            this.extScrollBarCAPIShips.TabIndex = 1;
            this.extScrollBarCAPIShips.Text = "extScrollBar1";
            this.extScrollBarCAPIShips.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarCAPIShips.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarCAPIShips.ThumbColorScaling = 0.5F;
            this.extScrollBarCAPIShips.ThumbDrawAngle = 0F;
            this.extScrollBarCAPIShips.Value = -1;
            this.extScrollBarCAPIShips.ValueLimited = -1;
            // 
            // dataGridViewCAPIShips
            // 
            this.dataGridViewCAPIShips.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewCAPIShips.AllowUserToAddRows = false;
            this.dataGridViewCAPIShips.AllowUserToDeleteRows = false;
            this.dataGridViewCAPIShips.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCAPIShips.AutoSortByColumnName = true;
            this.dataGridViewCAPIShips.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCAPIShips.ColumnReorder = true;
            this.dataGridViewCAPIShips.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCAPIShipsName,
            this.colCAPIShipsManu,
            this.colCAPIShipsPriceNumeric,
            this.colCAPIShipsSpeedNumeric,
            this.colCAPIShipsBoostNumeric,
            this.colCAPIShipsMassNumeric,
            this.colCAPIShipsLandingPadNumeric});
            this.dataGridViewCAPIShips.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCAPIShips.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCAPIShips.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCAPIShips.Name = "dataGridViewCAPIShips";
            this.dataGridViewCAPIShips.PerColumnWordWrapControl = true;
            this.dataGridViewCAPIShips.RowHeaderMenuStrip = null;
            this.dataGridViewCAPIShips.RowHeadersVisible = false;
            this.dataGridViewCAPIShips.RowHeadersWidth = 62;
            this.dataGridViewCAPIShips.RowTemplate.Height = 28;
            this.dataGridViewCAPIShips.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCAPIShips.SingleRowSelect = true;
            this.dataGridViewCAPIShips.Size = new System.Drawing.Size(823, 284);
            this.dataGridViewCAPIShips.TabIndex = 0;
            // 
            // colCAPIShipsName
            // 
            this.colCAPIShipsName.FillWeight = 150F;
            this.colCAPIShipsName.HeaderText = "Name";
            this.colCAPIShipsName.MinimumWidth = 8;
            this.colCAPIShipsName.Name = "colCAPIShipsName";
            this.colCAPIShipsName.ReadOnly = true;
            // 
            // colCAPIShipsManu
            // 
            this.colCAPIShipsManu.HeaderText = "Manufacturer";
            this.colCAPIShipsManu.MinimumWidth = 8;
            this.colCAPIShipsManu.Name = "colCAPIShipsManu";
            this.colCAPIShipsManu.ReadOnly = true;
            // 
            // colCAPIShipsPriceNumeric
            // 
            this.colCAPIShipsPriceNumeric.FillWeight = 50F;
            this.colCAPIShipsPriceNumeric.HeaderText = "Price";
            this.colCAPIShipsPriceNumeric.Name = "colCAPIShipsPriceNumeric";
            this.colCAPIShipsPriceNumeric.ReadOnly = true;
            // 
            // colCAPIShipsSpeedNumeric
            // 
            this.colCAPIShipsSpeedNumeric.FillWeight = 50F;
            this.colCAPIShipsSpeedNumeric.HeaderText = "Speed";
            this.colCAPIShipsSpeedNumeric.Name = "colCAPIShipsSpeedNumeric";
            this.colCAPIShipsSpeedNumeric.ReadOnly = true;
            // 
            // colCAPIShipsBoostNumeric
            // 
            this.colCAPIShipsBoostNumeric.FillWeight = 50F;
            this.colCAPIShipsBoostNumeric.HeaderText = "Boost";
            this.colCAPIShipsBoostNumeric.MinimumWidth = 8;
            this.colCAPIShipsBoostNumeric.Name = "colCAPIShipsBoostNumeric";
            this.colCAPIShipsBoostNumeric.ReadOnly = true;
            // 
            // colCAPIShipsMassNumeric
            // 
            this.colCAPIShipsMassNumeric.FillWeight = 50F;
            this.colCAPIShipsMassNumeric.HeaderText = "Mass";
            this.colCAPIShipsMassNumeric.MinimumWidth = 8;
            this.colCAPIShipsMassNumeric.Name = "colCAPIShipsMassNumeric";
            this.colCAPIShipsMassNumeric.ReadOnly = true;
            // 
            // colCAPIShipsLandingPadNumeric
            // 
            this.colCAPIShipsLandingPadNumeric.FillWeight = 50F;
            this.colCAPIShipsLandingPadNumeric.HeaderText = "Landing Pad";
            this.colCAPIShipsLandingPadNumeric.MinimumWidth = 8;
            this.colCAPIShipsLandingPadNumeric.Name = "colCAPIShipsLandingPadNumeric";
            this.colCAPIShipsLandingPadNumeric.ReadOnly = true;
            // 
            // extPanelDataGridViewCAPIModules
            // 
            this.extPanelDataGridViewCAPIModules.Controls.Add(this.extScrollBarCAPIModules);
            this.extPanelDataGridViewCAPIModules.Controls.Add(this.dataGridViewCAPIModules);
            this.extPanelDataGridViewCAPIModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewCAPIModules.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewCAPIModules.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewCAPIModules.Margin = new System.Windows.Forms.Padding(2);
            this.extPanelDataGridViewCAPIModules.Name = "extPanelDataGridViewCAPIModules";
            this.extPanelDataGridViewCAPIModules.Size = new System.Drawing.Size(839, 220);
            this.extPanelDataGridViewCAPIModules.TabIndex = 4;
            this.extPanelDataGridViewCAPIModules.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarCAPIModules
            // 
            this.extScrollBarCAPIModules.AlwaysHideScrollBar = false;
            this.extScrollBarCAPIModules.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarCAPIModules.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarCAPIModules.ArrowColorScaling = 0.5F;
            this.extScrollBarCAPIModules.ArrowDownDrawAngle = 270F;
            this.extScrollBarCAPIModules.ArrowUpDrawAngle = 90F;
            this.extScrollBarCAPIModules.BorderColor = System.Drawing.Color.White;
            this.extScrollBarCAPIModules.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarCAPIModules.HideScrollBar = false;
            this.extScrollBarCAPIModules.LargeChange = 0;
            this.extScrollBarCAPIModules.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarCAPIModules.Margin = new System.Windows.Forms.Padding(2);
            this.extScrollBarCAPIModules.Maximum = -1;
            this.extScrollBarCAPIModules.Minimum = 0;
            this.extScrollBarCAPIModules.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarCAPIModules.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarCAPIModules.Name = "extScrollBarCAPIModules";
            this.extScrollBarCAPIModules.Size = new System.Drawing.Size(16, 220);
            this.extScrollBarCAPIModules.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarCAPIModules.SmallChange = 1;
            this.extScrollBarCAPIModules.TabIndex = 1;
            this.extScrollBarCAPIModules.Text = "extScrollBar1";
            this.extScrollBarCAPIModules.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarCAPIModules.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarCAPIModules.ThumbColorScaling = 0.5F;
            this.extScrollBarCAPIModules.ThumbDrawAngle = 0F;
            this.extScrollBarCAPIModules.Value = -1;
            this.extScrollBarCAPIModules.ValueLimited = -1;
            // 
            // dataGridViewCAPIModules
            // 
            this.dataGridViewCAPIModules.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewCAPIModules.AllowUserToAddRows = false;
            this.dataGridViewCAPIModules.AllowUserToDeleteRows = false;
            this.dataGridViewCAPIModules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCAPIModules.AutoSortByColumnName = true;
            this.dataGridViewCAPIModules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCAPIModules.ColumnReorder = true;
            this.dataGridViewCAPIModules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCAPIModulesName,
            this.colCAPIModulesCat,
            this.colCAPIModulesMassNumeric,
            this.colCAPIModulesPowerNumeric,
            this.colCAPIModulesCostNumeric,
            this.colCAPIModulesStockNumeric,
            this.colCAPIModulesInfo});
            this.dataGridViewCAPIModules.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCAPIModules.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCAPIModules.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCAPIModules.Name = "dataGridViewCAPIModules";
            this.dataGridViewCAPIModules.PerColumnWordWrapControl = true;
            this.dataGridViewCAPIModules.RowHeaderMenuStrip = null;
            this.dataGridViewCAPIModules.RowHeadersVisible = false;
            this.dataGridViewCAPIModules.RowHeadersWidth = 62;
            this.dataGridViewCAPIModules.RowTemplate.Height = 28;
            this.dataGridViewCAPIModules.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCAPIModules.SingleRowSelect = true;
            this.dataGridViewCAPIModules.Size = new System.Drawing.Size(823, 220);
            this.dataGridViewCAPIModules.TabIndex = 0;
            // 
            // colCAPIModulesName
            // 
            this.colCAPIModulesName.FillWeight = 150F;
            this.colCAPIModulesName.HeaderText = "Name";
            this.colCAPIModulesName.MinimumWidth = 8;
            this.colCAPIModulesName.Name = "colCAPIModulesName";
            this.colCAPIModulesName.ReadOnly = true;
            // 
            // colCAPIModulesCat
            // 
            this.colCAPIModulesCat.FillWeight = 75F;
            this.colCAPIModulesCat.HeaderText = "Category";
            this.colCAPIModulesCat.MinimumWidth = 8;
            this.colCAPIModulesCat.Name = "colCAPIModulesCat";
            this.colCAPIModulesCat.ReadOnly = true;
            // 
            // colCAPIModulesMassNumeric
            // 
            this.colCAPIModulesMassNumeric.FillWeight = 75F;
            this.colCAPIModulesMassNumeric.HeaderText = "Mass";
            this.colCAPIModulesMassNumeric.Name = "colCAPIModulesMassNumeric";
            this.colCAPIModulesMassNumeric.ReadOnly = true;
            // 
            // colCAPIModulesPowerNumeric
            // 
            this.colCAPIModulesPowerNumeric.FillWeight = 50F;
            this.colCAPIModulesPowerNumeric.HeaderText = "Power";
            this.colCAPIModulesPowerNumeric.Name = "colCAPIModulesPowerNumeric";
            this.colCAPIModulesPowerNumeric.ReadOnly = true;
            // 
            // colCAPIModulesCostNumeric
            // 
            this.colCAPIModulesCostNumeric.FillWeight = 50F;
            this.colCAPIModulesCostNumeric.HeaderText = "Cost";
            this.colCAPIModulesCostNumeric.MinimumWidth = 8;
            this.colCAPIModulesCostNumeric.Name = "colCAPIModulesCostNumeric";
            this.colCAPIModulesCostNumeric.ReadOnly = true;
            // 
            // colCAPIModulesStockNumeric
            // 
            this.colCAPIModulesStockNumeric.FillWeight = 50F;
            this.colCAPIModulesStockNumeric.HeaderText = "Stock";
            this.colCAPIModulesStockNumeric.MinimumWidth = 8;
            this.colCAPIModulesStockNumeric.Name = "colCAPIModulesStockNumeric";
            this.colCAPIModulesStockNumeric.ReadOnly = true;
            // 
            // colCAPIModulesInfo
            // 
            this.colCAPIModulesInfo.FillWeight = 150F;
            this.colCAPIModulesInfo.HeaderText = "Info";
            this.colCAPIModulesInfo.MinimumWidth = 8;
            this.colCAPIModulesInfo.Name = "colCAPIModulesInfo";
            this.colCAPIModulesInfo.ReadOnly = true;
            // 
            // panelCAPI1
            // 
            this.panelCAPI1.Controls.Add(this.labelCAPIDateTime2);
            this.panelCAPI1.Controls.Add(this.extButtonDoCAPI1);
            this.panelCAPI1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCAPI1.Location = new System.Drawing.Point(3, 3);
            this.panelCAPI1.Name = "panelCAPI1";
            this.panelCAPI1.Size = new System.Drawing.Size(839, 32);
            this.panelCAPI1.TabIndex = 5;
            // 
            // labelCAPIDateTime2
            // 
            this.labelCAPIDateTime2.AutoSize = true;
            this.labelCAPIDateTime2.Location = new System.Drawing.Point(33, 9);
            this.labelCAPIDateTime2.Name = "labelCAPIDateTime2";
            this.labelCAPIDateTime2.Size = new System.Drawing.Size(92, 13);
            this.labelCAPIDateTime2.TabIndex = 2;
            this.labelCAPIDateTime2.Text = "<code DateTime>";
            // 
            // extButtonDoCAPI1
            // 
            this.extButtonDoCAPI1.Image = global::EDDiscovery.Icons.Controls.Refresh;
            this.extButtonDoCAPI1.Location = new System.Drawing.Point(3, 3);
            this.extButtonDoCAPI1.Name = "extButtonDoCAPI1";
            this.extButtonDoCAPI1.Size = new System.Drawing.Size(24, 24);
            this.extButtonDoCAPI1.TabIndex = 0;
            this.extButtonDoCAPI1.UseVisualStyleBackColor = true;
            this.extButtonDoCAPI1.Click += new System.EventHandler(this.extButtonDoCAPI1_Click);
            // 
            // tabPageCAPI2
            // 
            this.tabPageCAPI2.Controls.Add(this.splitContainerCAPI2);
            this.tabPageCAPI2.Controls.Add(this.panelCAPI2);
            this.tabPageCAPI2.Location = new System.Drawing.Point(4, 22);
            this.tabPageCAPI2.Name = "tabPageCAPI2";
            this.tabPageCAPI2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCAPI2.Size = new System.Drawing.Size(845, 546);
            this.tabPageCAPI2.TabIndex = 10;
            this.tabPageCAPI2.Text = "CAPI Items";
            this.tabPageCAPI2.UseVisualStyleBackColor = true;
            // 
            // splitContainerCAPI2
            // 
            this.splitContainerCAPI2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCAPI2.Location = new System.Drawing.Point(3, 35);
            this.splitContainerCAPI2.Name = "splitContainerCAPI2";
            this.splitContainerCAPI2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerCAPI2.Panel1
            // 
            this.splitContainerCAPI2.Panel1.Controls.Add(this.extPanelDataGridViewScrollCAPICargo);
            // 
            // splitContainerCAPI2.Panel2
            // 
            this.splitContainerCAPI2.Panel2.Controls.Add(this.extPanelDataGridViewScrollCAPILocker);
            this.splitContainerCAPI2.Size = new System.Drawing.Size(839, 508);
            this.splitContainerCAPI2.SplitterDistance = 284;
            this.splitContainerCAPI2.TabIndex = 1;
            // 
            // extPanelDataGridViewScrollCAPICargo
            // 
            this.extPanelDataGridViewScrollCAPICargo.Controls.Add(this.extScrollBarCAPICargo);
            this.extPanelDataGridViewScrollCAPICargo.Controls.Add(this.dataGridViewCAPICargo);
            this.extPanelDataGridViewScrollCAPICargo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollCAPICargo.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollCAPICargo.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollCAPICargo.Margin = new System.Windows.Forms.Padding(2);
            this.extPanelDataGridViewScrollCAPICargo.Name = "extPanelDataGridViewScrollCAPICargo";
            this.extPanelDataGridViewScrollCAPICargo.Size = new System.Drawing.Size(839, 284);
            this.extPanelDataGridViewScrollCAPICargo.TabIndex = 4;
            this.extPanelDataGridViewScrollCAPICargo.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarCAPICargo
            // 
            this.extScrollBarCAPICargo.AlwaysHideScrollBar = false;
            this.extScrollBarCAPICargo.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarCAPICargo.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarCAPICargo.ArrowColorScaling = 0.5F;
            this.extScrollBarCAPICargo.ArrowDownDrawAngle = 270F;
            this.extScrollBarCAPICargo.ArrowUpDrawAngle = 90F;
            this.extScrollBarCAPICargo.BorderColor = System.Drawing.Color.White;
            this.extScrollBarCAPICargo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarCAPICargo.HideScrollBar = false;
            this.extScrollBarCAPICargo.LargeChange = 0;
            this.extScrollBarCAPICargo.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarCAPICargo.Margin = new System.Windows.Forms.Padding(2);
            this.extScrollBarCAPICargo.Maximum = -1;
            this.extScrollBarCAPICargo.Minimum = 0;
            this.extScrollBarCAPICargo.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarCAPICargo.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarCAPICargo.Name = "extScrollBarCAPICargo";
            this.extScrollBarCAPICargo.Size = new System.Drawing.Size(16, 284);
            this.extScrollBarCAPICargo.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarCAPICargo.SmallChange = 1;
            this.extScrollBarCAPICargo.TabIndex = 1;
            this.extScrollBarCAPICargo.Text = "extScrollBar1";
            this.extScrollBarCAPICargo.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarCAPICargo.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarCAPICargo.ThumbColorScaling = 0.5F;
            this.extScrollBarCAPICargo.ThumbDrawAngle = 0F;
            this.extScrollBarCAPICargo.Value = -1;
            this.extScrollBarCAPICargo.ValueLimited = -1;
            // 
            // dataGridViewCAPICargo
            // 
            this.dataGridViewCAPICargo.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewCAPICargo.AllowUserToAddRows = false;
            this.dataGridViewCAPICargo.AllowUserToDeleteRows = false;
            this.dataGridViewCAPICargo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCAPICargo.AutoSortByColumnName = true;
            this.dataGridViewCAPICargo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCAPICargo.ColumnReorder = true;
            this.dataGridViewCAPICargo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCAPICargoCommodity,
            this.colCAPICargoType,
            this.colCAPICargoGroup,
            this.colCAPICargoQuantityNumeric,
            this.colCAPICargoPriceNumeric,
            this.colCAPICargoStolen});
            this.dataGridViewCAPICargo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCAPICargo.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCAPICargo.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCAPICargo.Name = "dataGridViewCAPICargo";
            this.dataGridViewCAPICargo.PerColumnWordWrapControl = true;
            this.dataGridViewCAPICargo.RowHeaderMenuStrip = null;
            this.dataGridViewCAPICargo.RowHeadersVisible = false;
            this.dataGridViewCAPICargo.RowHeadersWidth = 62;
            this.dataGridViewCAPICargo.RowTemplate.Height = 28;
            this.dataGridViewCAPICargo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCAPICargo.SingleRowSelect = true;
            this.dataGridViewCAPICargo.Size = new System.Drawing.Size(823, 284);
            this.dataGridViewCAPICargo.TabIndex = 0;
            // 
            // colCAPICargoCommodity
            // 
            this.colCAPICargoCommodity.FillWeight = 150F;
            this.colCAPICargoCommodity.HeaderText = "Commodity";
            this.colCAPICargoCommodity.MinimumWidth = 8;
            this.colCAPICargoCommodity.Name = "colCAPICargoCommodity";
            this.colCAPICargoCommodity.ReadOnly = true;
            // 
            // colCAPICargoType
            // 
            this.colCAPICargoType.HeaderText = "Type";
            this.colCAPICargoType.Name = "colCAPICargoType";
            this.colCAPICargoType.ReadOnly = true;
            // 
            // colCAPICargoGroup
            // 
            this.colCAPICargoGroup.HeaderText = "Group";
            this.colCAPICargoGroup.Name = "colCAPICargoGroup";
            this.colCAPICargoGroup.ReadOnly = true;
            // 
            // colCAPICargoQuantityNumeric
            // 
            this.colCAPICargoQuantityNumeric.FillWeight = 80F;
            this.colCAPICargoQuantityNumeric.HeaderText = "Quantity";
            this.colCAPICargoQuantityNumeric.MinimumWidth = 8;
            this.colCAPICargoQuantityNumeric.Name = "colCAPICargoQuantityNumeric";
            this.colCAPICargoQuantityNumeric.ReadOnly = true;
            // 
            // colCAPICargoPriceNumeric
            // 
            this.colCAPICargoPriceNumeric.FillWeight = 80F;
            this.colCAPICargoPriceNumeric.HeaderText = "Price";
            this.colCAPICargoPriceNumeric.Name = "colCAPICargoPriceNumeric";
            this.colCAPICargoPriceNumeric.ReadOnly = true;
            // 
            // colCAPICargoStolen
            // 
            this.colCAPICargoStolen.FillWeight = 75F;
            this.colCAPICargoStolen.HeaderText = "Stolen";
            this.colCAPICargoStolen.MinimumWidth = 8;
            this.colCAPICargoStolen.Name = "colCAPICargoStolen";
            this.colCAPICargoStolen.ReadOnly = true;
            // 
            // extPanelDataGridViewScrollCAPILocker
            // 
            this.extPanelDataGridViewScrollCAPILocker.Controls.Add(this.extScrollBarCAPILocker);
            this.extPanelDataGridViewScrollCAPILocker.Controls.Add(this.dataGridViewCAPILocker);
            this.extPanelDataGridViewScrollCAPILocker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollCAPILocker.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollCAPILocker.Location = new System.Drawing.Point(0, 0);
            this.extPanelDataGridViewScrollCAPILocker.Margin = new System.Windows.Forms.Padding(2);
            this.extPanelDataGridViewScrollCAPILocker.Name = "extPanelDataGridViewScrollCAPILocker";
            this.extPanelDataGridViewScrollCAPILocker.Size = new System.Drawing.Size(839, 220);
            this.extPanelDataGridViewScrollCAPILocker.TabIndex = 4;
            this.extPanelDataGridViewScrollCAPILocker.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarCAPILocker
            // 
            this.extScrollBarCAPILocker.AlwaysHideScrollBar = false;
            this.extScrollBarCAPILocker.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarCAPILocker.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarCAPILocker.ArrowColorScaling = 0.5F;
            this.extScrollBarCAPILocker.ArrowDownDrawAngle = 270F;
            this.extScrollBarCAPILocker.ArrowUpDrawAngle = 90F;
            this.extScrollBarCAPILocker.BorderColor = System.Drawing.Color.White;
            this.extScrollBarCAPILocker.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarCAPILocker.HideScrollBar = false;
            this.extScrollBarCAPILocker.LargeChange = 0;
            this.extScrollBarCAPILocker.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarCAPILocker.Margin = new System.Windows.Forms.Padding(2);
            this.extScrollBarCAPILocker.Maximum = -1;
            this.extScrollBarCAPILocker.Minimum = 0;
            this.extScrollBarCAPILocker.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarCAPILocker.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarCAPILocker.Name = "extScrollBarCAPILocker";
            this.extScrollBarCAPILocker.Size = new System.Drawing.Size(16, 220);
            this.extScrollBarCAPILocker.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarCAPILocker.SmallChange = 1;
            this.extScrollBarCAPILocker.TabIndex = 1;
            this.extScrollBarCAPILocker.Text = "extScrollBar1";
            this.extScrollBarCAPILocker.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarCAPILocker.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarCAPILocker.ThumbColorScaling = 0.5F;
            this.extScrollBarCAPILocker.ThumbDrawAngle = 0F;
            this.extScrollBarCAPILocker.Value = -1;
            this.extScrollBarCAPILocker.ValueLimited = -1;
            // 
            // dataGridViewCAPILocker
            // 
            this.dataGridViewCAPILocker.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewCAPILocker.AllowUserToAddRows = false;
            this.dataGridViewCAPILocker.AllowUserToDeleteRows = false;
            this.dataGridViewCAPILocker.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCAPILocker.AutoSortByColumnName = true;
            this.dataGridViewCAPILocker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCAPILocker.ColumnReorder = true;
            this.dataGridViewCAPILocker.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCAPILockerName,
            this.colCAPILockerType,
            this.colCAPILockerQuantityNumeric});
            this.dataGridViewCAPILocker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCAPILocker.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCAPILocker.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewCAPILocker.Name = "dataGridViewCAPILocker";
            this.dataGridViewCAPILocker.PerColumnWordWrapControl = true;
            this.dataGridViewCAPILocker.RowHeaderMenuStrip = null;
            this.dataGridViewCAPILocker.RowHeadersVisible = false;
            this.dataGridViewCAPILocker.RowHeadersWidth = 62;
            this.dataGridViewCAPILocker.RowTemplate.Height = 28;
            this.dataGridViewCAPILocker.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCAPILocker.SingleRowSelect = true;
            this.dataGridViewCAPILocker.Size = new System.Drawing.Size(823, 220);
            this.dataGridViewCAPILocker.TabIndex = 0;
            // 
            // colCAPILockerName
            // 
            this.colCAPILockerName.HeaderText = "Microresource";
            this.colCAPILockerName.MinimumWidth = 8;
            this.colCAPILockerName.Name = "colCAPILockerName";
            this.colCAPILockerName.ReadOnly = true;
            // 
            // colCAPILockerType
            // 
            this.colCAPILockerType.HeaderText = "Type";
            this.colCAPILockerType.Name = "colCAPILockerType";
            this.colCAPILockerType.ReadOnly = true;
            // 
            // colCAPILockerQuantityNumeric
            // 
            this.colCAPILockerQuantityNumeric.HeaderText = "Quantity";
            this.colCAPILockerQuantityNumeric.Name = "colCAPILockerQuantityNumeric";
            this.colCAPILockerQuantityNumeric.ReadOnly = true;
            // 
            // panelCAPI2
            // 
            this.panelCAPI2.Controls.Add(this.labelCAPIDateTime3);
            this.panelCAPI2.Controls.Add(this.extButtonDoCAPI2);
            this.panelCAPI2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCAPI2.Location = new System.Drawing.Point(3, 3);
            this.panelCAPI2.Name = "panelCAPI2";
            this.panelCAPI2.Size = new System.Drawing.Size(839, 32);
            this.panelCAPI2.TabIndex = 5;
            // 
            // labelCAPIDateTime3
            // 
            this.labelCAPIDateTime3.AutoSize = true;
            this.labelCAPIDateTime3.Location = new System.Drawing.Point(33, 9);
            this.labelCAPIDateTime3.Name = "labelCAPIDateTime3";
            this.labelCAPIDateTime3.Size = new System.Drawing.Size(92, 13);
            this.labelCAPIDateTime3.TabIndex = 2;
            this.labelCAPIDateTime3.Text = "<code DateTime>";
            // 
            // extButtonDoCAPI2
            // 
            this.extButtonDoCAPI2.Image = global::EDDiscovery.Icons.Controls.Refresh;
            this.extButtonDoCAPI2.Location = new System.Drawing.Point(3, 3);
            this.extButtonDoCAPI2.Name = "extButtonDoCAPI2";
            this.extButtonDoCAPI2.Size = new System.Drawing.Size(24, 24);
            this.extButtonDoCAPI2.TabIndex = 0;
            this.extButtonDoCAPI2.UseVisualStyleBackColor = true;
            this.extButtonDoCAPI2.Click += new System.EventHandler(this.extButtonDoCAPI1_Click);
            // 
            // UserControlCarrier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.extTabControl);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserControlCarrier";
            this.Size = new System.Drawing.Size(853, 572);
            this.extTabControl.ResumeLayout(false);
            this.tabPageOverall.ResumeLayout(false);
            this.tabPageItinerary.ResumeLayout(false);
            this.dataViewScrollerPanelItinerary.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItinerary)).EndInit();
            this.tabPageFinances.ResumeLayout(false);
            this.splitContainerLedger.Panel1.ResumeLayout(false);
            this.splitContainerLedger.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLedger)).EndInit();
            this.splitContainerLedger.ResumeLayout(false);
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
            this.tabPageCAPI3.ResumeLayout(false);
            this.extPanelDataGridViewScrollCAPIStats.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPIStats)).EndInit();
            this.panelCAPI3.ResumeLayout(false);
            this.panelCAPI3.PerformLayout();
            this.tabPageCAPI1.ResumeLayout(false);
            this.splitContainerCAPI1.Panel1.ResumeLayout(false);
            this.splitContainerCAPI1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCAPI1)).EndInit();
            this.splitContainerCAPI1.ResumeLayout(false);
            this.extPanelDataGridViewScrollCAPIShips.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPIShips)).EndInit();
            this.extPanelDataGridViewCAPIModules.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPIModules)).EndInit();
            this.panelCAPI1.ResumeLayout(false);
            this.panelCAPI1.PerformLayout();
            this.tabPageCAPI2.ResumeLayout(false);
            this.splitContainerCAPI2.Panel1.ResumeLayout(false);
            this.splitContainerCAPI2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCAPI2)).EndInit();
            this.splitContainerCAPI2.ResumeLayout(false);
            this.extPanelDataGridViewScrollCAPICargo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPICargo)).EndInit();
            this.extPanelDataGridViewScrollCAPILocker.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCAPILocker)).EndInit();
            this.panelCAPI2.ResumeLayout(false);
            this.panelCAPI2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtTabControl extTabControl;
        private System.Windows.Forms.TabPage tabPageOverall;
        private System.Windows.Forms.TabPage tabPageItinerary;
        private System.Windows.Forms.TabPage tabPageFinances;
        private System.Windows.Forms.TabPage tabPageServices;
        private System.Windows.Forms.TabPage tabPagePacks;
        private System.Windows.Forms.Label labelFCarrierBalance;
        private System.Windows.Forms.TabPage tabPageOrders;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelItinerary;
        private ExtendedControls.ExtScrollBar extScrollBarItinerary;
        private Search.DataGridViewStarResults dataGridViewItinerary;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinSystemAlphaInt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinBodyAlphaInt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinXNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinYNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinZNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinJumpDistNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinDistFromNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItinInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerStarsystemAlphaInt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerBodyAlphaInt;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerEvent;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerCreditNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerDebitNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLedgerBalanceNumeric;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersPurchaseNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersSaleNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersPriceNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrdersBlackmarket;
        private System.Windows.Forms.TabPage tabPageCAPI1;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewCAPIModules;
        private ExtendedControls.ExtScrollBar extScrollBarCAPIModules;
        private BaseUtils.DataGridViewColumnControl dataGridViewCAPIModules;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollCAPIShips;
        private ExtendedControls.ExtScrollBar extScrollBarCAPIShips;
        private BaseUtils.DataGridViewColumnControl dataGridViewCAPIShips;
        private System.Windows.Forms.Panel panelCAPI1;
        private ExtendedControls.ExtButton extButtonDoCAPI1;
        private System.Windows.Forms.TabPage tabPageCAPI2;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollCAPICargo;
        private ExtendedControls.ExtScrollBar extScrollBarCAPICargo;
        private BaseUtils.DataGridViewColumnControl dataGridViewCAPICargo;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollCAPILocker;
        private ExtendedControls.ExtScrollBar extScrollBarCAPILocker;
        private BaseUtils.DataGridViewColumnControl dataGridViewCAPILocker;
        private System.Windows.Forms.Panel panelCAPI2;
        private ExtendedControls.ExtButton extButtonDoCAPI2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPICargoCommodity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPICargoType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPICargoGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPICargoQuantityNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPICargoPriceNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPICargoStolen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPILockerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPILockerType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPILockerQuantityNumeric;
        private System.Windows.Forms.SplitContainer splitContainerCAPI2;
        private System.Windows.Forms.SplitContainer splitContainerCAPI1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIShipsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIShipsManu;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIShipsPriceNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIShipsSpeedNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIShipsBoostNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIShipsMassNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIShipsLandingPadNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIModulesName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIModulesCat;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIModulesMassNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIModulesPowerNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIModulesCostNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIModulesStockNumeric;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCAPIModulesInfo;
        private System.Windows.Forms.TabPage tabPageCAPI3;
        private System.Windows.Forms.Panel panelCAPI3;
        private ExtendedControls.ExtButton extButtonDoCAPI3;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollCAPIStats;
        private ExtendedControls.ExtScrollBar extScrollBarCAPIStats;
        private BaseUtils.DataGridViewColumnControl dataGridViewCAPIStats;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnValue;
        private System.Windows.Forms.Label labelCAPIDateTime1;
        private System.Windows.Forms.Label labelCAPICarrierBalance;
        private System.Windows.Forms.Label labelCAPIDateTime2;
        private System.Windows.Forms.Label labelCAPIDateTime3;
        private System.Windows.Forms.SplitContainer splitContainerLedger;
        private ExtendedControls.ExtSafeChart extChartLedger;
    }
}
