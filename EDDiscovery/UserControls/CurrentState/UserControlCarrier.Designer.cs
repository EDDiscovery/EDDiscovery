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
            this.T = new ExtendedControls.Controls.ImageControl();
            this.tabPageItinerary = new System.Windows.Forms.TabPage();
            this.dataViewScrollerPanelFactions = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarFactions = new ExtendedControls.ExtScrollBar();
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
            this.labelFTaxPioneerSupplies = new System.Windows.Forms.Label();
            this.labelFTaxRefuel = new System.Windows.Forms.Label();
            this.labelFReservePercent = new System.Windows.Forms.Label();
            this.labelFTaxOutfitting = new System.Windows.Forms.Label();
            this.labelFTaxRearm = new System.Windows.Forms.Label();
            this.tabPageServices = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.tabPageCargo = new System.Windows.Forms.TabPage();
            this.label11 = new System.Windows.Forms.Label();
            this.tabPagePacks = new System.Windows.Forms.TabPage();
            this.label16 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.tabPageMarket = new System.Windows.Forms.TabPage();
            this.label15 = new System.Windows.Forms.Label();
            this.tabPageOrders = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.extTabControlOverall.SuspendLayout();
            this.tabPageOverall.SuspendLayout();
            this.tabPageItinerary.SuspendLayout();
            this.dataViewScrollerPanelFactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItinerary)).BeginInit();
            this.tabPageFinances.SuspendLayout();
            this.extPanelDataGridViewScrollLedger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).BeginInit();
            this.panelFinancesTop.SuspendLayout();
            this.tabPageServices.SuspendLayout();
            this.tabPageCargo.SuspendLayout();
            this.tabPagePacks.SuspendLayout();
            this.tabPageMarket.SuspendLayout();
            this.tabPageOrders.SuspendLayout();
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
            this.extTabControlOverall.Controls.Add(this.tabPageCargo);
            this.extTabControlOverall.Controls.Add(this.tabPagePacks);
            this.extTabControlOverall.Controls.Add(this.tabPageMarket);
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
            this.tabPageOverall.Controls.Add(this.T);
            this.tabPageOverall.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverall.Name = "tabPageOverall";
            this.tabPageOverall.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverall.Size = new System.Drawing.Size(845, 546);
            this.tabPageOverall.TabIndex = 0;
            this.tabPageOverall.Text = "Overall";
            this.tabPageOverall.UseVisualStyleBackColor = true;
            // 
            // T
            // 
            this.T.BackColor = System.Drawing.Color.Black;
            this.T.BackgroundImage = global::EDDiscovery.Icons.Controls.FleetCarrier;
            this.T.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.T.Dock = System.Windows.Forms.DockStyle.Fill;
            this.T.ImageBackgroundColor = System.Drawing.Color.Transparent;
            this.T.ImageDepth = 1;
            this.T.ImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.T.ImageSize = new System.Drawing.Size(128, 128);
            this.T.ImageVisible = new bool[] {
        false};
            this.T.Location = new System.Drawing.Point(3, 3);
            this.T.Name = "T";
            this.T.Size = new System.Drawing.Size(839, 540);
            this.T.TabIndex = 0;
            this.T.Text = "imageControl1";
            // 
            // tabPageItinerary
            // 
            this.tabPageItinerary.Controls.Add(this.dataViewScrollerPanelFactions);
            this.tabPageItinerary.Location = new System.Drawing.Point(4, 22);
            this.tabPageItinerary.Name = "tabPageItinerary";
            this.tabPageItinerary.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageItinerary.Size = new System.Drawing.Size(845, 546);
            this.tabPageItinerary.TabIndex = 1;
            this.tabPageItinerary.Text = "Itinerary";
            this.tabPageItinerary.UseVisualStyleBackColor = true;
            // 
            // dataViewScrollerPanelFactions
            // 
            this.dataViewScrollerPanelFactions.Controls.Add(this.extScrollBarFactions);
            this.dataViewScrollerPanelFactions.Controls.Add(this.dataGridViewItinerary);
            this.dataViewScrollerPanelFactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelFactions.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelFactions.Location = new System.Drawing.Point(3, 3);
            this.dataViewScrollerPanelFactions.Margin = new System.Windows.Forms.Padding(2);
            this.dataViewScrollerPanelFactions.Name = "dataViewScrollerPanelFactions";
            this.dataViewScrollerPanelFactions.Size = new System.Drawing.Size(839, 540);
            this.dataViewScrollerPanelFactions.TabIndex = 2;
            this.dataViewScrollerPanelFactions.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarFactions
            // 
            this.extScrollBarFactions.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarFactions.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarFactions.ArrowColorScaling = 0.5F;
            this.extScrollBarFactions.ArrowDownDrawAngle = 270F;
            this.extScrollBarFactions.ArrowUpDrawAngle = 90F;
            this.extScrollBarFactions.BorderColor = System.Drawing.Color.White;
            this.extScrollBarFactions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarFactions.HideScrollBar = false;
            this.extScrollBarFactions.LargeChange = 0;
            this.extScrollBarFactions.Location = new System.Drawing.Point(823, 0);
            this.extScrollBarFactions.Margin = new System.Windows.Forms.Padding(2);
            this.extScrollBarFactions.Maximum = -1;
            this.extScrollBarFactions.Minimum = 0;
            this.extScrollBarFactions.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarFactions.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarFactions.Name = "extScrollBarFactions";
            this.extScrollBarFactions.Size = new System.Drawing.Size(16, 540);
            this.extScrollBarFactions.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarFactions.SmallChange = 1;
            this.extScrollBarFactions.TabIndex = 1;
            this.extScrollBarFactions.Text = "extScrollBar1";
            this.extScrollBarFactions.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarFactions.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarFactions.ThumbColorScaling = 0.5F;
            this.extScrollBarFactions.ThumbDrawAngle = 0F;
            this.extScrollBarFactions.Value = -1;
            this.extScrollBarFactions.ValueLimited = -1;
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
            this.tabPageServices.Controls.Add(this.label10);
            this.tabPageServices.Location = new System.Drawing.Point(4, 22);
            this.tabPageServices.Name = "tabPageServices";
            this.tabPageServices.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageServices.Size = new System.Drawing.Size(845, 546);
            this.tabPageServices.TabIndex = 3;
            this.tabPageServices.Text = "Services";
            this.tabPageServices.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(103, 109);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(377, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Set of boxes, each with a image of the service, and its state from servicesCrew";
            // 
            // tabPageCargo
            // 
            this.tabPageCargo.Controls.Add(this.label11);
            this.tabPageCargo.Location = new System.Drawing.Point(4, 22);
            this.tabPageCargo.Name = "tabPageCargo";
            this.tabPageCargo.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageCargo.Size = new System.Drawing.Size(845, 546);
            this.tabPageCargo.TabIndex = 4;
            this.tabPageCargo.Text = "Cargo";
            this.tabPageCargo.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(234, 267);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(191, 13);
            this.label11.TabIndex = 1;
            this.label11.Text = "Grid, cargo array, carrier Locker as well";
            // 
            // tabPagePacks
            // 
            this.tabPagePacks.Controls.Add(this.label16);
            this.tabPagePacks.Controls.Add(this.label13);
            this.tabPagePacks.Location = new System.Drawing.Point(4, 22);
            this.tabPagePacks.Name = "tabPagePacks";
            this.tabPagePacks.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePacks.Size = new System.Drawing.Size(845, 546);
            this.tabPagePacks.TabIndex = 6;
            this.tabPagePacks.Text = "Packs";
            this.tabPagePacks.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(224, 214);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(433, 13);
            this.label16.TabIndex = 0;
            this.label16.Text = "From CAPI ships+modules it lists the actual ships - we can work out real cost usi" +
    "ng taxation";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(242, 165);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(234, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "from carrierstats and packs, it gives pack names";
            // 
            // tabPageMarket
            // 
            this.tabPageMarket.Controls.Add(this.label15);
            this.tabPageMarket.Location = new System.Drawing.Point(4, 22);
            this.tabPageMarket.Name = "tabPageMarket";
            this.tabPageMarket.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMarket.Size = new System.Drawing.Size(845, 546);
            this.tabPageMarket.TabIndex = 7;
            this.tabPageMarket.Text = "Market";
            this.tabPageMarket.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(287, 214);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(177, 13);
            this.label15.TabIndex = 0;
            this.label15.Text = "Market.Commodities array? Not sure";
            // 
            // tabPageOrders
            // 
            this.tabPageOrders.Controls.Add(this.label12);
            this.tabPageOrders.Location = new System.Drawing.Point(4, 22);
            this.tabPageOrders.Name = "tabPageOrders";
            this.tabPageOrders.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOrders.Size = new System.Drawing.Size(845, 546);
            this.tabPageOrders.TabIndex = 8;
            this.tabPageOrders.Text = "Orders";
            this.tabPageOrders.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(219, 135);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(267, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Grid, all the commodities and MR orders and purchases";
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
            this.dataViewScrollerPanelFactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItinerary)).EndInit();
            this.tabPageFinances.ResumeLayout(false);
            this.extPanelDataGridViewScrollLedger.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).EndInit();
            this.panelFinancesTop.ResumeLayout(false);
            this.panelFinancesTop.PerformLayout();
            this.tabPageServices.ResumeLayout(false);
            this.tabPageServices.PerformLayout();
            this.tabPageCargo.ResumeLayout(false);
            this.tabPageCargo.PerformLayout();
            this.tabPagePacks.ResumeLayout(false);
            this.tabPagePacks.PerformLayout();
            this.tabPageMarket.ResumeLayout(false);
            this.tabPageMarket.PerformLayout();
            this.tabPageOrders.ResumeLayout(false);
            this.tabPageOrders.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtTabControl extTabControlOverall;
        private System.Windows.Forms.TabPage tabPageOverall;
        private System.Windows.Forms.TabPage tabPageItinerary;
        private System.Windows.Forms.TabPage tabPageFinances;
        private System.Windows.Forms.TabPage tabPageServices;
        private System.Windows.Forms.TabPage tabPageCargo;
        private System.Windows.Forms.TabPage tabPagePacks;
        private System.Windows.Forms.TabPage tabPageMarket;
        private System.Windows.Forms.Label labelFCarrierBalance;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TabPage tabPageOrders;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label15;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelFactions;
        private ExtendedControls.ExtScrollBar extScrollBarFactions;
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
        private ExtendedControls.Controls.ImageControl T;
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
    }
}
