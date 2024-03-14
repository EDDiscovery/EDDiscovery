namespace EDDiscovery.UserControls
{
    partial class UserControlFactions
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
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMissionsForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCommoditymaterialTradesForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBountiesAndBondsForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFactionSystemDetailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.startDateTimePicker = new ExtendedControls.ExtDateTimePicker();
            this.endDateTimePicker = new ExtendedControls.ExtDateTimePicker();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.dataViewScrollerPanelFactions = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.scrollBarFactions = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.colFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMissions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfluence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReputation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMissionCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CBought = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MBought = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CrimeCommitted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BountyKills = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BountyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BountyRewardsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRedeemVoucher = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFines = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBountyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Interdicted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Interdiction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KillBondVictim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KillBondsAward = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KillBondsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CartoValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataLinkVictimFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataLinkPayeeFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataLinkPayeeValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.contextMenuStrip.SuspendLayout();
            this.dataViewScrollerPanelFactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMissionsForFactionToolStripMenuItem,
            this.showCommoditymaterialTradesForFactionToolStripMenuItem,
            this.showBountiesAndBondsForFactionToolStripMenuItem,
            this.showFactionSystemDetailToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(310, 92);
            // 
            // showMissionsForFactionToolStripMenuItem
            // 
            this.showMissionsForFactionToolStripMenuItem.Name = "showMissionsForFactionToolStripMenuItem";
            this.showMissionsForFactionToolStripMenuItem.Size = new System.Drawing.Size(309, 22);
            this.showMissionsForFactionToolStripMenuItem.Text = "Show missions for faction";
            this.showMissionsForFactionToolStripMenuItem.Click += new System.EventHandler(this.showMissionsForFactionToolStripMenuItem_Click);
            // 
            // showCommoditymaterialTradesForFactionToolStripMenuItem
            // 
            this.showCommoditymaterialTradesForFactionToolStripMenuItem.Name = "showCommoditymaterialTradesForFactionToolStripMenuItem";
            this.showCommoditymaterialTradesForFactionToolStripMenuItem.Size = new System.Drawing.Size(309, 22);
            this.showCommoditymaterialTradesForFactionToolStripMenuItem.Text = "Show commodity/material trades for faction";
            this.showCommoditymaterialTradesForFactionToolStripMenuItem.Click += new System.EventHandler(this.showCommoditymaterialTradesForFactionToolStripMenuItem_Click);
            // 
            // showBountiesAndBondsForFactionToolStripMenuItem
            // 
            this.showBountiesAndBondsForFactionToolStripMenuItem.Name = "showBountiesAndBondsForFactionToolStripMenuItem";
            this.showBountiesAndBondsForFactionToolStripMenuItem.Size = new System.Drawing.Size(309, 22);
            this.showBountiesAndBondsForFactionToolStripMenuItem.Text = "Show bounties and bonds for faction";
            this.showBountiesAndBondsForFactionToolStripMenuItem.Click += new System.EventHandler(this.showBountiesAndBondsForFactionToolStripMenuItem_Click);
            // 
            // showFactionSystemDetailToolStripMenuItem
            // 
            this.showFactionSystemDetailToolStripMenuItem.Name = "showFactionSystemDetailToolStripMenuItem";
            this.showFactionSystemDetailToolStripMenuItem.Size = new System.Drawing.Size(309, 22);
            this.showFactionSystemDetailToolStripMenuItem.Text = "Show system detail for faction";
            this.showFactionSystemDetailToolStripMenuItem.Click += new System.EventHandler(this.showFactionSystemDetailToolStripMenuItem_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.BorderColor = System.Drawing.Color.Transparent;
            this.startDateTimePicker.BorderColorScaling = 0.5F;
            this.startDateTimePicker.Checked = false;
            this.startDateTimePicker.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDateTimePicker.Location = new System.Drawing.Point(0, 1);
            this.startDateTimePicker.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.startDateTimePicker.Name = "startDateTimePicker";
            this.startDateTimePicker.SelectedColor = System.Drawing.Color.Yellow;
            this.startDateTimePicker.ShowCheckBox = true;
            this.startDateTimePicker.ShowUpDown = false;
            this.startDateTimePicker.Size = new System.Drawing.Size(220, 24);
            this.startDateTimePicker.TabIndex = 2;
            this.startDateTimePicker.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.toolTip.SetToolTip(this.startDateTimePicker, "Include from");
            this.startDateTimePicker.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            // 
            // endDateTimePicker
            // 
            this.endDateTimePicker.BorderColor = System.Drawing.Color.Transparent;
            this.endDateTimePicker.BorderColorScaling = 0.5F;
            this.endDateTimePicker.Checked = false;
            this.endDateTimePicker.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDateTimePicker.Location = new System.Drawing.Point(252, 1);
            this.endDateTimePicker.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.endDateTimePicker.Name = "endDateTimePicker";
            this.endDateTimePicker.SelectedColor = System.Drawing.Color.Yellow;
            this.endDateTimePicker.ShowCheckBox = true;
            this.endDateTimePicker.ShowUpDown = false;
            this.endDateTimePicker.Size = new System.Drawing.Size(218, 24);
            this.endDateTimePicker.TabIndex = 3;
            this.endDateTimePicker.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.toolTip.SetToolTip(this.endDateTimePicker, "Include to");
            this.endDateTimePicker.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(478, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 33;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // dataViewScrollerPanelFactions
            // 
            this.dataViewScrollerPanelFactions.Controls.Add(this.scrollBarFactions);
            this.dataViewScrollerPanelFactions.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanelFactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelFactions.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelFactions.Location = new System.Drawing.Point(0, 32);
            this.dataViewScrollerPanelFactions.Margin = new System.Windows.Forms.Padding(2);
            this.dataViewScrollerPanelFactions.Name = "dataViewScrollerPanelFactions";
            this.dataViewScrollerPanelFactions.Size = new System.Drawing.Size(853, 540);
            this.dataViewScrollerPanelFactions.TabIndex = 1;
            this.dataViewScrollerPanelFactions.VerticalScrollBarDockRight = true;
            // 
            // scrollBarFactions
            // 
            this.scrollBarFactions.AlwaysHideScrollBar = false;
            this.scrollBarFactions.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.scrollBarFactions.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.scrollBarFactions.ArrowColorScaling = 0.5F;
            this.scrollBarFactions.ArrowDownDrawAngle = 270F;
            this.scrollBarFactions.ArrowUpDrawAngle = 90F;
            this.scrollBarFactions.BorderColor = System.Drawing.Color.White;
            this.scrollBarFactions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.scrollBarFactions.HideScrollBar = false;
            this.scrollBarFactions.LargeChange = 0;
            this.scrollBarFactions.Location = new System.Drawing.Point(837, 0);
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
            // dataGridView
            // 
            this.dataGridView.AllowRowHeaderVisibleSelection = false;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
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
            this.colFaction,
            this.colMissions,
            this.colInfluence,
            this.colReputation,
            this.colMissionCredits,
            this.CBought,
            this.CSold,
            this.CProfit,
            this.MBought,
            this.MSold,
            this.CrimeCommitted,
            this.BountyKills,
            this.BountyValue,
            this.BountyRewardsValue,
            this.colRedeemVoucher,
            this.colFines,
            this.colBountyValue,
            this.Interdicted,
            this.Interdiction,
            this.KillBondVictim,
            this.KillBondsAward,
            this.KillBondsValue,
            this.CartoValue,
            this.colDataLinkVictimFaction,
            this.colDataLinkPayeeFaction,
            this.colDataLinkPayeeValue,
            this.colInfo});
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
            this.dataGridView.Margin = new System.Windows.Forms.Padding(2);
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
            this.dataGridView.RowHeadersWidth = 62;
            this.dataGridView.RowTemplate.Height = 28;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(837, 540);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewFactions_SortCompare);
            // 
            // colFaction
            // 
            this.colFaction.FillWeight = 200.039F;
            this.colFaction.HeaderText = "Faction";
            this.colFaction.MinimumWidth = 8;
            this.colFaction.Name = "colFaction";
            this.colFaction.ReadOnly = true;
            // 
            // colMissions
            // 
            this.colMissions.FillWeight = 100.0195F;
            this.colMissions.HeaderText = "Missions";
            this.colMissions.MinimumWidth = 8;
            this.colMissions.Name = "colMissions";
            this.colMissions.ReadOnly = true;
            // 
            // colInfluence
            // 
            this.colInfluence.FillWeight = 100.0195F;
            this.colInfluence.HeaderText = "Inf +";
            this.colInfluence.MinimumWidth = 8;
            this.colInfluence.Name = "colInfluence";
            this.colInfluence.ReadOnly = true;
            // 
            // colReputation
            // 
            this.colReputation.FillWeight = 100.0195F;
            this.colReputation.HeaderText = "Rep +";
            this.colReputation.MinimumWidth = 8;
            this.colReputation.Name = "colReputation";
            this.colReputation.ReadOnly = true;
            // 
            // colMissionCredits
            // 
            this.colMissionCredits.HeaderText = "Mission Credits";
            this.colMissionCredits.MinimumWidth = 8;
            this.colMissionCredits.Name = "colMissionCredits";
            this.colMissionCredits.ReadOnly = true;
            // 
            // CBought
            // 
            this.CBought.FillWeight = 100.0195F;
            this.CBought.HeaderText = "Commds +";
            this.CBought.MinimumWidth = 8;
            this.CBought.Name = "CBought";
            this.CBought.ReadOnly = true;
            // 
            // CSold
            // 
            this.CSold.FillWeight = 100.0195F;
            this.CSold.HeaderText = "Commds -";
            this.CSold.MinimumWidth = 8;
            this.CSold.Name = "CSold";
            // 
            // CProfit
            // 
            this.CProfit.FillWeight = 99.59389F;
            this.CProfit.HeaderText = "Commds Profit";
            this.CProfit.MinimumWidth = 8;
            this.CProfit.Name = "CProfit";
            // 
            // MBought
            // 
            this.MBought.FillWeight = 100.0195F;
            this.MBought.HeaderText = "Mats +";
            this.MBought.MinimumWidth = 8;
            this.MBought.Name = "MBought";
            this.MBought.ReadOnly = true;
            // 
            // MSold
            // 
            this.MSold.FillWeight = 100.0195F;
            this.MSold.HeaderText = "Mats -";
            this.MSold.MinimumWidth = 8;
            this.MSold.Name = "MSold";
            this.MSold.ReadOnly = true;
            // 
            // CrimeCommitted
            // 
            this.CrimeCommitted.FillWeight = 100.0195F;
            this.CrimeCommitted.HeaderText = "Crimes Committed";
            this.CrimeCommitted.MinimumWidth = 8;
            this.CrimeCommitted.Name = "CrimeCommitted";
            // 
            // BountyKills
            // 
            this.BountyKills.FillWeight = 100.0195F;
            this.BountyKills.HeaderText = "Bounty Kills";
            this.BountyKills.MinimumWidth = 8;
            this.BountyKills.Name = "BountyKills";
            this.BountyKills.ReadOnly = true;
            // 
            // BountyValue
            // 
            this.BountyValue.FillWeight = 100.0195F;
            this.BountyValue.HeaderText = "Bounty Rewards";
            this.BountyValue.MinimumWidth = 8;
            this.BountyValue.Name = "BountyValue";
            this.BountyValue.ReadOnly = true;
            // 
            // BountyRewardsValue
            // 
            this.BountyRewardsValue.FillWeight = 100.0195F;
            this.BountyRewardsValue.HeaderText = "Bounty Rewards Value";
            this.BountyRewardsValue.MinimumWidth = 8;
            this.BountyRewardsValue.Name = "BountyRewardsValue";
            this.BountyRewardsValue.ReadOnly = true;
            // 
            // colRedeemVoucher
            // 
            this.colRedeemVoucher.HeaderText = "Redeem Voucher";
            this.colRedeemVoucher.Name = "colRedeemVoucher";
            this.colRedeemVoucher.ReadOnly = true;
            // 
            // colFines
            // 
            this.colFines.HeaderText = "Pay Fines";
            this.colFines.Name = "colFines";
            this.colFines.ReadOnly = true;
            // 
            // colBountyValue
            // 
            this.colBountyValue.HeaderText = "Pay Bounty Value";
            this.colBountyValue.Name = "colBountyValue";
            this.colBountyValue.ReadOnly = true;
            // 
            // Interdicted
            // 
            this.Interdicted.FillWeight = 100.0195F;
            this.Interdicted.HeaderText = "Interdicted";
            this.Interdicted.MinimumWidth = 8;
            this.Interdicted.Name = "Interdicted";
            this.Interdicted.ReadOnly = true;
            // 
            // Interdiction
            // 
            this.Interdiction.FillWeight = 100.0195F;
            this.Interdiction.HeaderText = "Interdiction";
            this.Interdiction.MinimumWidth = 8;
            this.Interdiction.Name = "Interdiction";
            this.Interdiction.ReadOnly = true;
            // 
            // KillBondVictim
            // 
            this.KillBondVictim.FillWeight = 100.0195F;
            this.KillBondVictim.HeaderText = "Kill Bonds Victim";
            this.KillBondVictim.MinimumWidth = 8;
            this.KillBondVictim.Name = "KillBondVictim";
            this.KillBondVictim.ReadOnly = true;
            // 
            // KillBondsAward
            // 
            this.KillBondsAward.FillWeight = 100.0195F;
            this.KillBondsAward.HeaderText = "Kill Bonds Awarded";
            this.KillBondsAward.MinimumWidth = 8;
            this.KillBondsAward.Name = "KillBondsAward";
            this.KillBondsAward.ReadOnly = true;
            // 
            // KillBondsValue
            // 
            this.KillBondsValue.FillWeight = 100.0195F;
            this.KillBondsValue.HeaderText = "Kill Bonds Value";
            this.KillBondsValue.MinimumWidth = 8;
            this.KillBondsValue.Name = "KillBondsValue";
            this.KillBondsValue.ReadOnly = true;
            // 
            // CartoValue
            // 
            this.CartoValue.HeaderText = "Cartographic Value";
            this.CartoValue.MinimumWidth = 8;
            this.CartoValue.Name = "CartoValue";
            this.CartoValue.ReadOnly = true;
            // 
            // colDataLinkVictimFaction
            // 
            this.colDataLinkVictimFaction.HeaderText = "DL Victim";
            this.colDataLinkVictimFaction.Name = "colDataLinkVictimFaction";
            this.colDataLinkVictimFaction.ReadOnly = true;
            // 
            // colDataLinkPayeeFaction
            // 
            this.colDataLinkPayeeFaction.HeaderText = "DL Payee Faction";
            this.colDataLinkPayeeFaction.Name = "colDataLinkPayeeFaction";
            this.colDataLinkPayeeFaction.ReadOnly = true;
            // 
            // colDataLinkPayeeValue
            // 
            this.colDataLinkPayeeValue.HeaderText = "DL Payee Value";
            this.colDataLinkPayeeValue.Name = "colDataLinkPayeeValue";
            this.colDataLinkPayeeValue.ReadOnly = true;
            // 
            // colInfo
            // 
            this.colInfo.FillWeight = 150.0293F;
            this.colInfo.HeaderText = "Other Info";
            this.colInfo.MinimumWidth = 8;
            this.colInfo.Name = "colInfo";
            this.colInfo.ReadOnly = true;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.startDateTimePicker);
            this.panelTop.Controls.Add(this.labelTo);
            this.panelTop.Controls.Add(this.endDateTimePicker);
            this.panelTop.Controls.Add(this.buttonExtExcel);
            this.panelTop.Controls.Add(this.labelInfo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(853, 32);
            this.panelTop.TabIndex = 0;
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(228, 4);
            this.labelTo.Margin = new System.Windows.Forms.Padding(0, 4, 8, 1);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(16, 13);
            this.labelTo.TabIndex = 4;
            this.labelTo.Text = "to";
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(520, 4);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(6, 4, 8, 1);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(43, 13);
            this.labelInfo.TabIndex = 5;
            this.labelInfo.Text = "<code>";
            // 
            // UserControlFactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.dataViewScrollerPanelFactions);
            this.Controls.Add(this.panelTop);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserControlFactions";
            this.Size = new System.Drawing.Size(853, 572);
            this.contextMenuStrip.ResumeLayout(false);
            this.dataViewScrollerPanelFactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtDateTimePicker startDateTimePicker;
        private System.Windows.Forms.Label labelTo;
        private ExtendedControls.ExtDateTimePicker endDateTimePicker;
        private System.Windows.Forms.Label labelInfo;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelFactions;
        private ExtendedControls.ExtScrollBar scrollBarFactions;
        private BaseUtils.DataGridViewColumnControl dataGridView;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showMissionsForFactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showCommoditymaterialTradesForFactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showBountiesAndBondsForFactionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showFactionSystemDetailToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMissions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfluence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReputation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMissionCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn CBought;
        private System.Windows.Forms.DataGridViewTextBoxColumn CSold;
        private System.Windows.Forms.DataGridViewTextBoxColumn CProfit;
        private System.Windows.Forms.DataGridViewTextBoxColumn MBought;
        private System.Windows.Forms.DataGridViewTextBoxColumn MSold;
        private System.Windows.Forms.DataGridViewTextBoxColumn CrimeCommitted;
        private System.Windows.Forms.DataGridViewTextBoxColumn BountyKills;
        private System.Windows.Forms.DataGridViewTextBoxColumn BountyValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn BountyRewardsValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRedeemVoucher;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFines;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBountyValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Interdicted;
        private System.Windows.Forms.DataGridViewTextBoxColumn Interdiction;
        private System.Windows.Forms.DataGridViewTextBoxColumn KillBondVictim;
        private System.Windows.Forms.DataGridViewTextBoxColumn KillBondsAward;
        private System.Windows.Forms.DataGridViewTextBoxColumn KillBondsValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn CartoValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataLinkVictimFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataLinkPayeeFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataLinkPayeeValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfo;
        private ExtendedControls.ExtButton buttonExtExcel;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
    }
}
