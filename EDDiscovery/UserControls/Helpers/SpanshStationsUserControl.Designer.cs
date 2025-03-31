
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpanshStationsUserControl));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.flowLayouSearch = new System.Windows.Forms.FlowLayoutPanel();
            this.labelSystem = new System.Windows.Forms.Label();
            this.extTextBoxAutoCompleteSystem = new ExtendedControls.ExtTextBoxAutoComplete();
            this.extButtonTravelSystem = new ExtendedControls.ExtButton();
            this.labelSearch = new System.Windows.Forms.Label();
            this.extButtonSearchCommodities = new ExtendedControls.ExtButton();
            this.extButtonSearchServiceTypes = new ExtendedControls.ExtButton();
            this.extButtonSearchEconomy = new ExtendedControls.ExtButton();
            this.extButtonSearchShips = new ExtendedControls.ExtButton();
            this.extButtonSearchOutfitting = new ExtendedControls.ExtButton();
            this.labelSettings = new System.Windows.Forms.Label();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.labelMaxLs = new System.Windows.Forms.Label();
            this.valueBoxMaxLs = new ExtendedControls.NumberBoxDouble();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewOnSpanshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewMarketToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOutfittingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewShipyardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extButtonType = new ExtendedControls.ExtButtonWithNewCheckedListBox();
            this.extButtonCommoditiesBuy = new ExtendedControls.ExtButtonWithNewCheckedListBox();
            this.extButtonServices = new ExtendedControls.ExtButtonWithNewCheckedListBox();
            this.extButtonEconomy = new ExtendedControls.ExtButtonWithNewCheckedListBox();
            this.extButtonShipyard = new ExtendedControls.ExtButtonWithNewCheckedListBox();
            this.extButtonOutfitting = new ExtendedControls.ExtButtonWithNewCheckedListBox();
            this.extButtonEditCommodities = new ExtendedControls.ExtButton();
            this.extButtonCommoditiesSell = new ExtendedControls.ExtButtonWithNewCheckedListBox();
            this.panelControls = new System.Windows.Forms.Panel();
            this.flowLayoutPanelFilters = new System.Windows.Forms.FlowLayoutPanel();
            this.labelFilter = new System.Windows.Forms.Label();
            this.labelShow = new System.Windows.Forms.Label();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.colSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDistanceRef = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBodyName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStationName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLattitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLongitude = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHasMarket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPrice1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStockDemand1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPrice2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStockDemand2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPrice3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColStockDemand3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOutfitting = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colShipyard = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAllegiance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEconomy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colGovernment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colServices = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSmallPad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMediumPads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLargePads = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.flowLayouSearch.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.flowLayoutPanelFilters.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // flowLayouSearch
            // 
            this.flowLayouSearch.AutoSize = true;
            this.flowLayouSearch.Controls.Add(this.labelSystem);
            this.flowLayouSearch.Controls.Add(this.extTextBoxAutoCompleteSystem);
            this.flowLayouSearch.Controls.Add(this.extButtonTravelSystem);
            this.flowLayouSearch.Controls.Add(this.labelSearch);
            this.flowLayouSearch.Controls.Add(this.extButtonSearchCommodities);
            this.flowLayouSearch.Controls.Add(this.extButtonSearchServiceTypes);
            this.flowLayouSearch.Controls.Add(this.extButtonSearchEconomy);
            this.flowLayouSearch.Controls.Add(this.extButtonSearchShips);
            this.flowLayouSearch.Controls.Add(this.extButtonSearchOutfitting);
            this.flowLayouSearch.Controls.Add(this.labelSettings);
            this.flowLayouSearch.Controls.Add(this.extCheckBoxWordWrap);
            this.flowLayouSearch.Controls.Add(this.buttonExtExcel);
            this.flowLayouSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayouSearch.Location = new System.Drawing.Point(0, 0);
            this.flowLayouSearch.Name = "flowLayouSearch";
            this.flowLayouSearch.Size = new System.Drawing.Size(978, 34);
            this.flowLayouSearch.TabIndex = 25;
            this.flowLayouSearch.WrapContents = false;
            // 
            // labelSystem
            // 
            this.labelSystem.AutoSize = true;
            this.labelSystem.Location = new System.Drawing.Point(4, 6);
            this.labelSystem.Margin = new System.Windows.Forms.Padding(4, 6, 4, 0);
            this.labelSystem.Name = "labelSystem";
            this.labelSystem.Size = new System.Drawing.Size(41, 13);
            this.labelSystem.TabIndex = 31;
            this.labelSystem.Text = "System";
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
            this.extTextBoxAutoCompleteSystem.DropDownBorderColor = System.Drawing.Color.Green;
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
            // extButtonTravelSystem
            // 
            this.extButtonTravelSystem.Image = global::EDDiscovery.Icons.Controls.Home;
            this.extButtonTravelSystem.Location = new System.Drawing.Point(222, 1);
            this.extButtonTravelSystem.Margin = new System.Windows.Forms.Padding(3, 1, 3, 3);
            this.extButtonTravelSystem.Name = "extButtonTravelSystem";
            this.extButtonTravelSystem.Size = new System.Drawing.Size(22, 22);
            this.extButtonTravelSystem.TabIndex = 34;
            this.toolTip.SetToolTip(this.extButtonTravelSystem, "Press to go back to current travel history system");
            this.extButtonTravelSystem.UseVisualStyleBackColor = true;
            this.extButtonTravelSystem.Click += new System.EventHandler(this.extButtonTravelSystem_Click);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(251, 6);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(4, 6, 4, 0);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 3;
            this.labelSearch.Text = "Search";
            // 
            // extButtonSearchCommodities
            // 
            this.extButtonSearchCommodities.Image = global::EDDiscovery.Icons.Controls.Commodity;
            this.extButtonSearchCommodities.Location = new System.Drawing.Point(300, 3);
            this.extButtonSearchCommodities.Margin = new System.Windows.Forms.Padding(4, 3, 3, 3);
            this.extButtonSearchCommodities.Name = "extButtonSearchCommodities";
            this.extButtonSearchCommodities.Size = new System.Drawing.Size(28, 28);
            this.extButtonSearchCommodities.TabIndex = 34;
            this.toolTip.SetToolTip(this.extButtonSearchCommodities, "Search for stations with commodities across systems");
            this.extButtonSearchCommodities.UseVisualStyleBackColor = true;
            this.extButtonSearchCommodities.Click += new System.EventHandler(this.extButtonSearchCommodities_Click);
            // 
            // extButtonSearchServiceTypes
            // 
            this.extButtonSearchServiceTypes.Image = global::EDDiscovery.Icons.Controls.People;
            this.extButtonSearchServiceTypes.Location = new System.Drawing.Point(334, 3);
            this.extButtonSearchServiceTypes.Name = "extButtonSearchServiceTypes";
            this.extButtonSearchServiceTypes.Size = new System.Drawing.Size(28, 28);
            this.extButtonSearchServiceTypes.TabIndex = 34;
            this.toolTip.SetToolTip(this.extButtonSearchServiceTypes, "Search for stations with services across systems");
            this.extButtonSearchServiceTypes.UseVisualStyleBackColor = true;
            this.extButtonSearchServiceTypes.Click += new System.EventHandler(this.extButtonSearchServiceTypes_Click);
            // 
            // extButtonSearchEconomy
            // 
            this.extButtonSearchEconomy.Image = global::EDDiscovery.Icons.Controls.Economy;
            this.extButtonSearchEconomy.Location = new System.Drawing.Point(368, 3);
            this.extButtonSearchEconomy.Name = "extButtonSearchEconomy";
            this.extButtonSearchEconomy.Size = new System.Drawing.Size(28, 28);
            this.extButtonSearchEconomy.TabIndex = 34;
            this.toolTip.SetToolTip(this.extButtonSearchEconomy, "Search for stations with economies across systems");
            this.extButtonSearchEconomy.UseVisualStyleBackColor = true;
            this.extButtonSearchEconomy.Click += new System.EventHandler(this.extButtonSearchEconomy_Click);
            // 
            // extButtonSearchShips
            // 
            this.extButtonSearchShips.Image = global::EDDiscovery.Icons.Controls.Shipyard;
            this.extButtonSearchShips.Location = new System.Drawing.Point(402, 3);
            this.extButtonSearchShips.Name = "extButtonSearchShips";
            this.extButtonSearchShips.Size = new System.Drawing.Size(28, 28);
            this.extButtonSearchShips.TabIndex = 34;
            this.toolTip.SetToolTip(this.extButtonSearchShips, "Search for stations with ships across systems");
            this.extButtonSearchShips.UseVisualStyleBackColor = true;
            this.extButtonSearchShips.Click += new System.EventHandler(this.extButtonSearchShips_Click);
            // 
            // extButtonSearchOutfitting
            // 
            this.extButtonSearchOutfitting.Image = global::EDDiscovery.Icons.Controls.Outfitting;
            this.extButtonSearchOutfitting.Location = new System.Drawing.Point(436, 3);
            this.extButtonSearchOutfitting.Name = "extButtonSearchOutfitting";
            this.extButtonSearchOutfitting.Size = new System.Drawing.Size(28, 28);
            this.extButtonSearchOutfitting.TabIndex = 34;
            this.toolTip.SetToolTip(this.extButtonSearchOutfitting, "Search for stations with outfitting modules across systems");
            this.extButtonSearchOutfitting.UseVisualStyleBackColor = true;
            this.extButtonSearchOutfitting.Click += new System.EventHandler(this.extButtonSearchOutfitting_Click);
            // 
            // labelSettings
            // 
            this.labelSettings.AutoSize = true;
            this.labelSettings.Location = new System.Drawing.Point(471, 6);
            this.labelSettings.Margin = new System.Windows.Forms.Padding(4, 6, 4, 0);
            this.labelSettings.Name = "labelSettings";
            this.labelSettings.Size = new System.Drawing.Size(29, 13);
            this.labelSettings.TabIndex = 3;
            this.labelSettings.Text = "Misc";
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
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(508, 2);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
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
            this.buttonExtExcel.Location = new System.Drawing.Point(544, 2);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Output grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // labelMaxLs
            // 
            this.labelMaxLs.AutoSize = true;
            this.labelMaxLs.Location = new System.Drawing.Point(293, 6);
            this.labelMaxLs.Margin = new System.Windows.Forms.Padding(4, 6, 4, 0);
            this.labelMaxLs.Name = "labelMaxLs";
            this.labelMaxLs.Size = new System.Drawing.Size(41, 13);
            this.labelMaxLs.TabIndex = 3;
            this.labelMaxLs.Text = "Max Ls";
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
            this.valueBoxMaxLs.Location = new System.Drawing.Point(342, 2);
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
            // extButtonType
            // 
            this.extButtonType.Image = global::EDDiscovery.Icons.Controls.CoriolisYellow;
            this.extButtonType.Location = new System.Drawing.Point(41, 2);
            this.extButtonType.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.extButtonType.Name = "extButtonType";
            this.extButtonType.SettingsSplittingChar = ';';
            this.extButtonType.Size = new System.Drawing.Size(28, 28);
            this.extButtonType.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonType, "Filter on station type");
            this.extButtonType.UseVisualStyleBackColor = true;
            // 
            // extButtonCommoditiesBuy
            // 
            this.extButtonCommoditiesBuy.Image = global::EDDiscovery.Icons.Controls.CommodityBuy;
            this.extButtonCommoditiesBuy.Location = new System.Drawing.Point(77, 2);
            this.extButtonCommoditiesBuy.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.extButtonCommoditiesBuy.Name = "extButtonCommoditiesBuy";
            this.extButtonCommoditiesBuy.SettingsSplittingChar = ';';
            this.extButtonCommoditiesBuy.Size = new System.Drawing.Size(28, 28);
            this.extButtonCommoditiesBuy.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonCommoditiesBuy, "Filter on commodities that you can buy and has stock");
            this.extButtonCommoditiesBuy.UseVisualStyleBackColor = true;
            // 
            // extButtonServices
            // 
            this.extButtonServices.Image = global::EDDiscovery.Icons.Controls.People;
            this.extButtonServices.Location = new System.Drawing.Point(149, 2);
            this.extButtonServices.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.extButtonServices.Name = "extButtonServices";
            this.extButtonServices.SettingsSplittingChar = ';';
            this.extButtonServices.Size = new System.Drawing.Size(28, 28);
            this.extButtonServices.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonServices, "Filter on services offered");
            this.extButtonServices.UseVisualStyleBackColor = true;
            // 
            // extButtonEconomy
            // 
            this.extButtonEconomy.Image = global::EDDiscovery.Icons.Controls.Economy;
            this.extButtonEconomy.Location = new System.Drawing.Point(185, 2);
            this.extButtonEconomy.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.extButtonEconomy.Name = "extButtonEconomy";
            this.extButtonEconomy.SettingsSplittingChar = ';';
            this.extButtonEconomy.Size = new System.Drawing.Size(28, 28);
            this.extButtonEconomy.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonEconomy, "Filter on economy");
            this.extButtonEconomy.UseVisualStyleBackColor = true;
            // 
            // extButtonShipyard
            // 
            this.extButtonShipyard.Image = global::EDDiscovery.Icons.Controls.Shipyard;
            this.extButtonShipyard.Location = new System.Drawing.Point(221, 2);
            this.extButtonShipyard.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.extButtonShipyard.Name = "extButtonShipyard";
            this.extButtonShipyard.SettingsSplittingChar = ';';
            this.extButtonShipyard.Size = new System.Drawing.Size(28, 28);
            this.extButtonShipyard.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonShipyard, "Filter on ships for sale");
            this.extButtonShipyard.UseVisualStyleBackColor = true;
            // 
            // extButtonOutfitting
            // 
            this.extButtonOutfitting.Image = global::EDDiscovery.Icons.Controls.Outfitting;
            this.extButtonOutfitting.Location = new System.Drawing.Point(257, 2);
            this.extButtonOutfitting.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.extButtonOutfitting.Name = "extButtonOutfitting";
            this.extButtonOutfitting.SettingsSplittingChar = ';';
            this.extButtonOutfitting.Size = new System.Drawing.Size(28, 28);
            this.extButtonOutfitting.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonOutfitting, "Filter on outfitting");
            this.extButtonOutfitting.UseVisualStyleBackColor = true;
            // 
            // extButtonEditCommodities
            // 
            this.extButtonEditCommodities.Image = global::EDDiscovery.Icons.Controls.Commodity;
            this.extButtonEditCommodities.Location = new System.Drawing.Point(476, 2);
            this.extButtonEditCommodities.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.extButtonEditCommodities.Name = "extButtonEditCommodities";
            this.extButtonEditCommodities.Size = new System.Drawing.Size(28, 28);
            this.extButtonEditCommodities.TabIndex = 34;
            this.toolTip.SetToolTip(this.extButtonEditCommodities, "Show up to three commodities, either buy or sell price");
            this.extButtonEditCommodities.UseVisualStyleBackColor = true;
            this.extButtonEditCommodities.Click += new System.EventHandler(this.extButtonEditCommodities_Click);
            // 
            // extButtonCommoditiesSell
            // 
            this.extButtonCommoditiesSell.Image = global::EDDiscovery.Icons.Controls.CommoditySell;
            this.extButtonCommoditiesSell.Location = new System.Drawing.Point(113, 2);
            this.extButtonCommoditiesSell.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.extButtonCommoditiesSell.Name = "extButtonCommoditiesSell";
            this.extButtonCommoditiesSell.SettingsSplittingChar = ';';
            this.extButtonCommoditiesSell.Size = new System.Drawing.Size(28, 28);
            this.extButtonCommoditiesSell.TabIndex = 32;
            this.toolTip.SetToolTip(this.extButtonCommoditiesSell, "Filter on commodities that the station wants which has a price and demand");
            this.extButtonCommoditiesSell.UseVisualStyleBackColor = true;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.Controls.Add(this.flowLayoutPanelFilters);
            this.panelControls.Controls.Add(this.flowLayouSearch);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(978, 66);
            this.panelControls.TabIndex = 25;
            // 
            // flowLayoutPanelFilters
            // 
            this.flowLayoutPanelFilters.AutoSize = true;
            this.flowLayoutPanelFilters.Controls.Add(this.labelFilter);
            this.flowLayoutPanelFilters.Controls.Add(this.extButtonType);
            this.flowLayoutPanelFilters.Controls.Add(this.extButtonCommoditiesBuy);
            this.flowLayoutPanelFilters.Controls.Add(this.extButtonCommoditiesSell);
            this.flowLayoutPanelFilters.Controls.Add(this.extButtonServices);
            this.flowLayoutPanelFilters.Controls.Add(this.extButtonEconomy);
            this.flowLayoutPanelFilters.Controls.Add(this.extButtonShipyard);
            this.flowLayoutPanelFilters.Controls.Add(this.extButtonOutfitting);
            this.flowLayoutPanelFilters.Controls.Add(this.labelMaxLs);
            this.flowLayoutPanelFilters.Controls.Add(this.valueBoxMaxLs);
            this.flowLayoutPanelFilters.Controls.Add(this.labelShow);
            this.flowLayoutPanelFilters.Controls.Add(this.extButtonEditCommodities);
            this.flowLayoutPanelFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanelFilters.Location = new System.Drawing.Point(0, 34);
            this.flowLayoutPanelFilters.Name = "flowLayoutPanelFilters";
            this.flowLayoutPanelFilters.Size = new System.Drawing.Size(978, 32);
            this.flowLayoutPanelFilters.TabIndex = 26;
            // 
            // labelFilter
            // 
            this.labelFilter.AutoSize = true;
            this.labelFilter.Location = new System.Drawing.Point(4, 6);
            this.labelFilter.Margin = new System.Windows.Forms.Padding(4, 6, 4, 0);
            this.labelFilter.Name = "labelFilter";
            this.labelFilter.Size = new System.Drawing.Size(29, 13);
            this.labelFilter.TabIndex = 3;
            this.labelFilter.Text = "Filter";
            // 
            // labelShow
            // 
            this.labelShow.AutoSize = true;
            this.labelShow.Location = new System.Drawing.Point(434, 4);
            this.labelShow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 0);
            this.labelShow.Name = "labelShow";
            this.labelShow.Size = new System.Drawing.Size(34, 13);
            this.labelShow.TabIndex = 3;
            this.labelShow.Text = "Show";
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 66);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(978, 711);
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
            this.vScrollBarCustom.Size = new System.Drawing.Size(16, 711);
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
            this.colSystem,
            this.colDistanceRef,
            this.colBodyName,
            this.colStationName,
            this.colDistance,
            this.colType,
            this.colLattitude,
            this.colLongitude,
            this.colHasMarket,
            this.ColPrice1,
            this.ColStockDemand1,
            this.ColPrice2,
            this.ColStockDemand2,
            this.ColPrice3,
            this.ColStockDemand3,
            this.colOutfitting,
            this.colShipyard,
            this.colAllegiance,
            this.colFaction,
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
            this.dataGridView.Size = new System.Drawing.Size(962, 711);
            this.dataGridView.TabIndex = 23;
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellDoubleClick);
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            this.dataGridView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseDown);
            // 
            // colSystem
            // 
            this.colSystem.FillWeight = 75F;
            this.colSystem.HeaderText = "System";
            this.colSystem.Name = "colSystem";
            this.colSystem.ReadOnly = true;
            // 
            // colDistanceRef
            // 
            this.colDistanceRef.FillWeight = 40F;
            this.colDistanceRef.HeaderText = "LY Distance";
            this.colDistanceRef.Name = "colDistanceRef";
            this.colDistanceRef.ReadOnly = true;
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
            this.colDistance.FillWeight = 40F;
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
            this.colLattitude.FillWeight = 50F;
            this.colLattitude.HeaderText = "Latitude";
            this.colLattitude.Name = "colLattitude";
            this.colLattitude.ReadOnly = true;
            // 
            // colLongitude
            // 
            this.colLongitude.FillWeight = 50F;
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
            // ColPrice1
            // 
            this.ColPrice1.FillWeight = 50F;
            this.ColPrice1.HeaderText = "Price 1";
            this.ColPrice1.Name = "ColPrice1";
            this.ColPrice1.ReadOnly = true;
            // 
            // ColStockDemand1
            // 
            this.ColStockDemand1.FillWeight = 50F;
            this.ColStockDemand1.HeaderText = "S/D 1";
            this.ColStockDemand1.Name = "ColStockDemand1";
            this.ColStockDemand1.ReadOnly = true;
            // 
            // ColPrice2
            // 
            this.ColPrice2.FillWeight = 50F;
            this.ColPrice2.HeaderText = "Price 2";
            this.ColPrice2.Name = "ColPrice2";
            this.ColPrice2.ReadOnly = true;
            // 
            // ColStockDemand2
            // 
            this.ColStockDemand2.FillWeight = 50F;
            this.ColStockDemand2.HeaderText = "S/D 2";
            this.ColStockDemand2.Name = "ColStockDemand2";
            this.ColStockDemand2.ReadOnly = true;
            // 
            // ColPrice3
            // 
            this.ColPrice3.FillWeight = 50F;
            this.ColPrice3.HeaderText = "Price 3";
            this.ColPrice3.Name = "ColPrice3";
            this.ColPrice3.ReadOnly = true;
            // 
            // ColStockDemand3
            // 
            this.ColStockDemand3.FillWeight = 50F;
            this.ColStockDemand3.HeaderText = "S/D 3";
            this.ColStockDemand3.Name = "ColStockDemand3";
            this.ColStockDemand3.ReadOnly = true;
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
            this.colAllegiance.FillWeight = 50F;
            this.colAllegiance.HeaderText = "Allegiance";
            this.colAllegiance.Name = "colAllegiance";
            this.colAllegiance.ReadOnly = true;
            // 
            // colFaction
            // 
            this.colFaction.HeaderText = "Faction";
            this.colFaction.Name = "colFaction";
            this.colFaction.ReadOnly = true;
            // 
            // colEconomy
            // 
            this.colEconomy.HeaderText = "Economy";
            this.colEconomy.Name = "colEconomy";
            this.colEconomy.ReadOnly = true;
            // 
            // colGovernment
            // 
            this.colGovernment.FillWeight = 75F;
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
            this.colSmallPad.FillWeight = 25F;
            this.colSmallPad.HeaderText = "SPad";
            this.colSmallPad.Name = "colSmallPad";
            this.colSmallPad.ReadOnly = true;
            // 
            // colMediumPads
            // 
            this.colMediumPads.FillWeight = 25F;
            this.colMediumPads.HeaderText = "MPad";
            this.colMediumPads.Name = "colMediumPads";
            this.colMediumPads.ReadOnly = true;
            // 
            // colLargePads
            // 
            this.colLargePads.FillWeight = 25F;
            this.colLargePads.HeaderText = "LPad";
            this.colLargePads.Name = "colLargePads";
            this.colLargePads.ReadOnly = true;
            // 
            // SpanshStationsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelControls);
            this.Name = "SpanshStationsUserControl";
            this.Size = new System.Drawing.Size(978, 777);
            this.flowLayouSearch.ResumeLayout(false);
            this.flowLayouSearch.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.flowLayoutPanelFilters.ResumeLayout(false);
            this.flowLayoutPanelFilters.PerformLayout();
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayouSearch;
        private System.Windows.Forms.Label labelSystem;
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
        private ExtendedControls.ExtButtonWithNewCheckedListBox extButtonType;
        private ExtendedControls.ExtButtonWithNewCheckedListBox extButtonCommoditiesBuy;
        private ExtendedControls.ExtButtonWithNewCheckedListBox extButtonOutfitting;
        private ExtendedControls.ExtButtonWithNewCheckedListBox extButtonShipyard;
        private ExtendedControls.ExtButtonWithNewCheckedListBox extButtonEconomy;
        private ExtendedControls.ExtButtonWithNewCheckedListBox extButtonServices;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelFilters;
        private ExtendedControls.ExtButton extButtonSearchServiceTypes;
        private ExtendedControls.ExtButton extButtonSearchCommodities;
        private ExtendedControls.ExtButton extButtonTravelSystem;
        private ExtendedControls.ExtButton extButtonSearchEconomy;
        private ExtendedControls.ExtButton extButtonSearchShips;
        private ExtendedControls.ExtButton extButtonSearchOutfitting;
        private ExtendedControls.ExtButton extButtonEditCommodities;
        private System.Windows.Forms.Label labelShow;
        private System.Windows.Forms.Label labelFilter;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.Label labelSettings;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDistanceRef;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBodyName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStationName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLattitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLongitude;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHasMarket;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPrice1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStockDemand1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPrice2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStockDemand2;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPrice3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColStockDemand3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOutfitting;
        private System.Windows.Forms.DataGridViewTextBoxColumn colShipyard;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAllegiance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEconomy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colGovernment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colServices;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSmallPad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMediumPads;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLargePads;
        private ExtendedControls.ExtButtonWithNewCheckedListBox extButtonCommoditiesSell;
    }
}
