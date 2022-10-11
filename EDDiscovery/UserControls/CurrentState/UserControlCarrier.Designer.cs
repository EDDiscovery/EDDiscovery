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
            this.tabPageItinerary = new System.Windows.Forms.TabPage();
            this.dataViewScrollerPanelFactions = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.scrollBarFactions = new ExtendedControls.ExtScrollBar();
            this.dataGridViewItinerary = new BaseUtils.DataGridViewColumnControl();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colX = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colZ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colJumpDist = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDistFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPageFinances = new System.Windows.Forms.TabPage();
            this.labelFTaxShipyard = new System.Windows.Forms.Label();
            this.labelFTaxRepair = new System.Windows.Forms.Label();
            this.labelFTaxRefuel = new System.Windows.Forms.Label();
            this.labelFTaxOutfitting = new System.Windows.Forms.Label();
            this.labelFTaxRearm = new System.Windows.Forms.Label();
            this.labelFReservePercent = new System.Windows.Forms.Label();
            this.labelFTaxPioneerSupplies = new System.Windows.Forms.Label();
            this.labelFAvailableBalance = new System.Windows.Forms.Label();
            this.labelFReserveBalance = new System.Windows.Forms.Label();
            this.labelFCarrierBalance = new System.Windows.Forms.Label();
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
            this.overallImageControl = new ExtendedControls.Controls.ImageControl();
            this.extTabControlOverall.SuspendLayout();
            this.tabPageOverall.SuspendLayout();
            this.tabPageItinerary.SuspendLayout();
            this.dataViewScrollerPanelFactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewItinerary)).BeginInit();
            this.tabPageFinances.SuspendLayout();
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
            this.tabPageOverall.Controls.Add(this.overallImageControl);
            this.tabPageOverall.Location = new System.Drawing.Point(4, 22);
            this.tabPageOverall.Name = "tabPageOverall";
            this.tabPageOverall.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageOverall.Size = new System.Drawing.Size(845, 546);
            this.tabPageOverall.TabIndex = 0;
            this.tabPageOverall.Text = "Overall";
            this.tabPageOverall.UseVisualStyleBackColor = true;
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
            this.dataViewScrollerPanelFactions.Controls.Add(this.scrollBarFactions);
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
            // scrollBarFactions
            // 
            this.scrollBarFactions.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.scrollBarFactions.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.scrollBarFactions.ArrowColorScaling = 0.5F;
            this.scrollBarFactions.ArrowDownDrawAngle = 270F;
            this.scrollBarFactions.ArrowUpDrawAngle = 90F;
            this.scrollBarFactions.BorderColor = System.Drawing.Color.White;
            this.scrollBarFactions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.scrollBarFactions.HideScrollBar = false;
            this.scrollBarFactions.LargeChange = 0;
            this.scrollBarFactions.Location = new System.Drawing.Point(823, 0);
            this.scrollBarFactions.Margin = new System.Windows.Forms.Padding(2);
            this.scrollBarFactions.Maximum = -1;
            this.scrollBarFactions.Minimum = 0;
            this.scrollBarFactions.MouseOverButtonColor = System.Drawing.Color.Green;
            this.scrollBarFactions.MousePressedButtonColor = System.Drawing.Color.Red;
            this.scrollBarFactions.Name = "scrollBarFactions";
            this.scrollBarFactions.Size = new System.Drawing.Size(16, 540);
            this.scrollBarFactions.SliderColor = System.Drawing.Color.DarkGray;
            this.scrollBarFactions.SmallChange = 1;
            this.scrollBarFactions.TabIndex = 1;
            this.scrollBarFactions.Text = "extScrollBar1";
            this.scrollBarFactions.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.scrollBarFactions.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.scrollBarFactions.ThumbColorScaling = 0.5F;
            this.scrollBarFactions.ThumbDrawAngle = 0F;
            this.scrollBarFactions.Value = -1;
            this.scrollBarFactions.ValueLimited = -1;
            // 
            // dataGridViewItinerary
            // 
            this.dataGridViewItinerary.AllowUserToAddRows = false;
            this.dataGridViewItinerary.AllowUserToDeleteRows = false;
            this.dataGridViewItinerary.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewItinerary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewItinerary.ColumnReorder = true;
            this.dataGridViewItinerary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDate,
            this.colSystem,
            this.colBody,
            this.colX,
            this.colY,
            this.colZ,
            this.colJumpDist,
            this.colDistFrom,
            this.colInformation});
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
            // colDate
            // 
            this.colDate.FillWeight = 200.039F;
            this.colDate.HeaderText = "Date";
            this.colDate.MinimumWidth = 8;
            this.colDate.Name = "colDate";
            this.colDate.ReadOnly = true;
            // 
            // colSystem
            // 
            this.colSystem.FillWeight = 100.0195F;
            this.colSystem.HeaderText = "System";
            this.colSystem.MinimumWidth = 8;
            this.colSystem.Name = "colSystem";
            this.colSystem.ReadOnly = true;
            // 
            // colBody
            // 
            this.colBody.FillWeight = 100.0195F;
            this.colBody.HeaderText = "Body";
            this.colBody.MinimumWidth = 8;
            this.colBody.Name = "colBody";
            this.colBody.ReadOnly = true;
            // 
            // colX
            // 
            this.colX.FillWeight = 100.0195F;
            this.colX.HeaderText = "X";
            this.colX.MinimumWidth = 8;
            this.colX.Name = "colX";
            this.colX.ReadOnly = true;
            // 
            // colY
            // 
            this.colY.FillWeight = 130.0254F;
            this.colY.HeaderText = "Y";
            this.colY.MinimumWidth = 8;
            this.colY.Name = "colY";
            this.colY.ReadOnly = true;
            // 
            // colZ
            // 
            this.colZ.FillWeight = 100.0195F;
            this.colZ.HeaderText = "Z";
            this.colZ.MinimumWidth = 8;
            this.colZ.Name = "colZ";
            this.colZ.ReadOnly = true;
            // 
            // colJumpDist
            // 
            this.colJumpDist.FillWeight = 100.0195F;
            this.colJumpDist.HeaderText = "Jump Dist";
            this.colJumpDist.MinimumWidth = 8;
            this.colJumpDist.Name = "colJumpDist";
            // 
            // colDistFrom
            // 
            this.colDistFrom.FillWeight = 100.0195F;
            this.colDistFrom.HeaderText = "Dist From";
            this.colDistFrom.MinimumWidth = 8;
            this.colDistFrom.Name = "colDistFrom";
            this.colDistFrom.ReadOnly = true;
            // 
            // colInformation
            // 
            this.colInformation.FillWeight = 99.59389F;
            this.colInformation.HeaderText = "Information";
            this.colInformation.MinimumWidth = 8;
            this.colInformation.Name = "colInformation";
            // 
            // tabPageFinances
            // 
            this.tabPageFinances.Controls.Add(this.labelFTaxShipyard);
            this.tabPageFinances.Controls.Add(this.labelFTaxRepair);
            this.tabPageFinances.Controls.Add(this.labelFTaxRefuel);
            this.tabPageFinances.Controls.Add(this.labelFTaxOutfitting);
            this.tabPageFinances.Controls.Add(this.labelFTaxRearm);
            this.tabPageFinances.Controls.Add(this.labelFReservePercent);
            this.tabPageFinances.Controls.Add(this.labelFTaxPioneerSupplies);
            this.tabPageFinances.Controls.Add(this.labelFAvailableBalance);
            this.tabPageFinances.Controls.Add(this.labelFReserveBalance);
            this.tabPageFinances.Controls.Add(this.labelFCarrierBalance);
            this.tabPageFinances.Location = new System.Drawing.Point(4, 22);
            this.tabPageFinances.Name = "tabPageFinances";
            this.tabPageFinances.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFinances.Size = new System.Drawing.Size(845, 546);
            this.tabPageFinances.TabIndex = 2;
            this.tabPageFinances.Text = "Finances";
            this.tabPageFinances.UseVisualStyleBackColor = true;
            // 
            // labelFTaxShipyard
            // 
            this.labelFTaxShipyard.AutoSize = true;
            this.labelFTaxShipyard.Location = new System.Drawing.Point(258, 41);
            this.labelFTaxShipyard.Name = "labelFTaxShipyard";
            this.labelFTaxShipyard.Size = new System.Drawing.Size(88, 13);
            this.labelFTaxShipyard.TabIndex = 0;
            this.labelFTaxShipyard.Text = "<code Tax Ship>";
            // 
            // labelFTaxRepair
            // 
            this.labelFTaxRepair.AutoSize = true;
            this.labelFTaxRepair.Location = new System.Drawing.Point(258, 131);
            this.labelFTaxRepair.Name = "labelFTaxRepair";
            this.labelFTaxRepair.Size = new System.Drawing.Size(98, 13);
            this.labelFTaxRepair.TabIndex = 0;
            this.labelFTaxRepair.Text = "<code Tax Repair>";
            // 
            // labelFTaxRefuel
            // 
            this.labelFTaxRefuel.AutoSize = true;
            this.labelFTaxRefuel.Location = new System.Drawing.Point(258, 108);
            this.labelFTaxRefuel.Name = "labelFTaxRefuel";
            this.labelFTaxRefuel.Size = new System.Drawing.Size(98, 13);
            this.labelFTaxRefuel.TabIndex = 0;
            this.labelFTaxRefuel.Text = "<code Tax Refuel>";
            // 
            // labelFTaxOutfitting
            // 
            this.labelFTaxOutfitting.AutoSize = true;
            this.labelFTaxOutfitting.Location = new System.Drawing.Point(258, 84);
            this.labelFTaxOutfitting.Name = "labelFTaxOutfitting";
            this.labelFTaxOutfitting.Size = new System.Drawing.Size(90, 13);
            this.labelFTaxOutfitting.TabIndex = 0;
            this.labelFTaxOutfitting.Text = "<code Tax outfit>";
            // 
            // labelFTaxRearm
            // 
            this.labelFTaxRearm.AutoSize = true;
            this.labelFTaxRearm.Location = new System.Drawing.Point(258, 65);
            this.labelFTaxRearm.Name = "labelFTaxRearm";
            this.labelFTaxRearm.Size = new System.Drawing.Size(98, 13);
            this.labelFTaxRearm.TabIndex = 0;
            this.labelFTaxRearm.Text = "<code Tax Rearm>";
            // 
            // labelFReservePercent
            // 
            this.labelFReservePercent.AutoSize = true;
            this.labelFReservePercent.Location = new System.Drawing.Point(6, 84);
            this.labelFReservePercent.Name = "labelFReservePercent";
            this.labelFReservePercent.Size = new System.Drawing.Size(94, 13);
            this.labelFReservePercent.TabIndex = 0;
            this.labelFReservePercent.Text = "<code Reserve%>";
            // 
            // labelFTaxPioneerSupplies
            // 
            this.labelFTaxPioneerSupplies.AutoSize = true;
            this.labelFTaxPioneerSupplies.Location = new System.Drawing.Point(258, 17);
            this.labelFTaxPioneerSupplies.Name = "labelFTaxPioneerSupplies";
            this.labelFTaxPioneerSupplies.Size = new System.Drawing.Size(74, 13);
            this.labelFTaxPioneerSupplies.TabIndex = 0;
            this.labelFTaxPioneerSupplies.Text = "<code Tax P>";
            // 
            // labelFAvailableBalance
            // 
            this.labelFAvailableBalance.AutoSize = true;
            this.labelFAvailableBalance.Location = new System.Drawing.Point(6, 65);
            this.labelFAvailableBalance.Name = "labelFAvailableBalance";
            this.labelFAvailableBalance.Size = new System.Drawing.Size(69, 13);
            this.labelFAvailableBalance.TabIndex = 0;
            this.labelFAvailableBalance.Text = "<code Avail>";
            // 
            // labelFReserveBalance
            // 
            this.labelFReserveBalance.AutoSize = true;
            this.labelFReserveBalance.Location = new System.Drawing.Point(6, 41);
            this.labelFReserveBalance.Name = "labelFReserveBalance";
            this.labelFReserveBalance.Size = new System.Drawing.Size(65, 13);
            this.labelFReserveBalance.TabIndex = 0;
            this.labelFReserveBalance.Text = "<code Res>";
            // 
            // labelFCarrierBalance
            // 
            this.labelFCarrierBalance.AutoSize = true;
            this.labelFCarrierBalance.Location = new System.Drawing.Point(6, 17);
            this.labelFCarrierBalance.Name = "labelFCarrierBalance";
            this.labelFCarrierBalance.Size = new System.Drawing.Size(85, 13);
            this.labelFCarrierBalance.TabIndex = 0;
            this.labelFCarrierBalance.Text = "<code Balance>";
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
            // imageControl1
            // 
            this.overallImageControl.BackColor = System.Drawing.Color.Black;
            this.overallImageControl.BackgroundImage = global::EDDiscovery.Icons.Controls.FleetCarrier;
            this.overallImageControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.overallImageControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.overallImageControl.ImageBackgroundColor = System.Drawing.Color.Transparent;
            this.overallImageControl.ImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.overallImageControl.ImageSize = new System.Drawing.Size(128, 128);
            this.overallImageControl.Location = new System.Drawing.Point(3, 3);
            this.overallImageControl.Name = "imageControl1";
            this.overallImageControl.Size = new System.Drawing.Size(839, 540);
            this.overallImageControl.TabIndex = 0;
            this.overallImageControl.Text = "imageControl1";
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
            this.tabPageFinances.PerformLayout();
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
        private ExtendedControls.ExtScrollBar scrollBarFactions;
        private BaseUtils.DataGridViewColumnControl dataGridViewItinerary;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBody;
        private System.Windows.Forms.DataGridViewTextBoxColumn colX;
        private System.Windows.Forms.DataGridViewTextBoxColumn colY;
        private System.Windows.Forms.DataGridViewTextBoxColumn colZ;
        private System.Windows.Forms.DataGridViewTextBoxColumn colJumpDist;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDistFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInformation;
        private System.Windows.Forms.Label labelFTaxShipyard;
        private System.Windows.Forms.Label labelFTaxRepair;
        private System.Windows.Forms.Label labelFTaxRefuel;
        private System.Windows.Forms.Label labelFTaxOutfitting;
        private System.Windows.Forms.Label labelFTaxRearm;
        private System.Windows.Forms.Label labelFReservePercent;
        private System.Windows.Forms.Label labelFTaxPioneerSupplies;
        private System.Windows.Forms.Label labelFAvailableBalance;
        private System.Windows.Forms.Label labelFReserveBalance;
        private ExtendedControls.Controls.ImageControl overallImageControl;
    }
}
