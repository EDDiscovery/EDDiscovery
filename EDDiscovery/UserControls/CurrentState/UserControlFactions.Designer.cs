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
            this.datePanel = new System.Windows.Forms.Panel();
            this.startDateTime = new ExtendedControls.ExtDateTimePicker();
            this.labelTo = new System.Windows.Forms.Label();
            this.endDateTime = new ExtendedControls.ExtDateTimePicker();
            this.labelValue = new System.Windows.Forms.Label();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMissionsForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCommoditymaterialTradesForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBountiesAndBondsForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showFactionSystemDetailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanelFactions = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.scrollBarFactions = new ExtendedControls.ExtScrollBar();
            this.dataGridViewFactions = new BaseUtils.DataGridViewColumnHider();
            this.colFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMissions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfluence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReputation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMissionCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CBought = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MBought = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CrimeCommitted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BountyKills = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BountyValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BountyRewardsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Interdicted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Interdiction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KillBondVictim = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KillBondsAward = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KillBondsValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datePanel.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.dataViewScrollerPanelFactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFactions)).BeginInit();
            this.SuspendLayout();
            // 
            // datePanel
            // 
            this.datePanel.Controls.Add(this.startDateTime);
            this.datePanel.Controls.Add(this.labelTo);
            this.datePanel.Controls.Add(this.endDateTime);
            this.datePanel.Controls.Add(this.labelValue);
            this.datePanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.datePanel.Location = new System.Drawing.Point(0, 0);
            this.datePanel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.datePanel.Name = "datePanel";
            this.datePanel.Size = new System.Drawing.Size(853, 27);
            this.datePanel.TabIndex = 0;
            // 
            // startDateTime
            // 
            this.startDateTime.BorderColor = System.Drawing.Color.Transparent;
            this.startDateTime.BorderColorScaling = 0.5F;
            this.startDateTime.Checked = false;
            this.startDateTime.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.startDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDateTime.Location = new System.Drawing.Point(0, 1);
            this.startDateTime.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.startDateTime.Name = "startDateTime";
            this.startDateTime.SelectedColor = System.Drawing.Color.Yellow;
            this.startDateTime.ShowCheckBox = true;
            this.startDateTime.ShowUpDown = false;
            this.startDateTime.Size = new System.Drawing.Size(220, 20);
            this.startDateTime.TabIndex = 2;
            this.startDateTime.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.toolTip.SetToolTip(this.startDateTime, "Include from");
            this.startDateTime.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            this.startDateTime.ValueChanged += new System.EventHandler(this.startDateTime_ValueChanged);
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(228, 4);
            this.labelTo.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(16, 13);
            this.labelTo.TabIndex = 4;
            this.labelTo.Text = "to";
            // 
            // endDateTime
            // 
            this.endDateTime.BorderColor = System.Drawing.Color.Transparent;
            this.endDateTime.BorderColorScaling = 0.5F;
            this.endDateTime.Checked = false;
            this.endDateTime.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.endDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDateTime.Location = new System.Drawing.Point(251, 1);
            this.endDateTime.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.endDateTime.Name = "endDateTime";
            this.endDateTime.SelectedColor = System.Drawing.Color.Yellow;
            this.endDateTime.ShowCheckBox = true;
            this.endDateTime.ShowUpDown = false;
            this.endDateTime.Size = new System.Drawing.Size(218, 20);
            this.endDateTime.TabIndex = 3;
            this.endDateTime.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.toolTip.SetToolTip(this.endDateTime, "Include to");
            this.endDateTime.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            this.endDateTime.ValueChanged += new System.EventHandler(this.endDateTime_ValueChanged);
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(477, 5);
            this.labelValue.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(43, 13);
            this.labelValue.TabIndex = 5;
            this.labelValue.Text = "<code>";
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
            // dataViewScrollerPanelFactions
            // 
            this.dataViewScrollerPanelFactions.Controls.Add(this.scrollBarFactions);
            this.dataViewScrollerPanelFactions.Controls.Add(this.dataGridViewFactions);
            this.dataViewScrollerPanelFactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelFactions.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelFactions.Location = new System.Drawing.Point(0, 27);
            this.dataViewScrollerPanelFactions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataViewScrollerPanelFactions.Name = "dataViewScrollerPanelFactions";
            this.dataViewScrollerPanelFactions.Size = new System.Drawing.Size(853, 545);
            this.dataViewScrollerPanelFactions.TabIndex = 1;
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
            this.scrollBarFactions.Location = new System.Drawing.Point(837, 0);
            this.scrollBarFactions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.scrollBarFactions.Maximum = -1;
            this.scrollBarFactions.Minimum = 0;
            this.scrollBarFactions.MouseOverButtonColor = System.Drawing.Color.Green;
            this.scrollBarFactions.MousePressedButtonColor = System.Drawing.Color.Red;
            this.scrollBarFactions.Name = "scrollBarFactions";
            this.scrollBarFactions.Size = new System.Drawing.Size(16, 545);
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
            // dataGridViewFactions
            // 
            this.dataGridViewFactions.AllowUserToAddRows = false;
            this.dataGridViewFactions.AllowUserToDeleteRows = false;
            this.dataGridViewFactions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewFactions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewFactions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFaction,
            this.colMissions,
            this.colInfluence,
            this.colReputation,
            this.colMissionCredits,
            this.CBought,
            this.CSold,
            this.MBought,
            this.MSold,
            this.CrimeCommitted,
            this.BountyKills,
            this.BountyValue,
            this.BountyRewardsValue,
            this.Interdicted,
            this.Interdiction,
            this.KillBondVictim,
            this.KillBondsAward,
            this.KillBondsValue,
            this.colInfo});
            this.dataGridViewFactions.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridViewFactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFactions.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewFactions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dataGridViewFactions.Name = "dataGridViewFactions";
            this.dataGridViewFactions.RowHeaderMenuStrip = null;
            this.dataGridViewFactions.RowHeadersVisible = false;
            this.dataGridViewFactions.RowHeadersWidth = 62;
            this.dataGridViewFactions.RowTemplate.Height = 28;
            this.dataGridViewFactions.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewFactions.SingleRowSelect = true;
            this.dataGridViewFactions.Size = new System.Drawing.Size(837, 545);
            this.dataGridViewFactions.TabIndex = 0;
            this.dataGridViewFactions.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewFactions_SortCompare);
            // 
            // colFaction
            // 
            this.colFaction.FillWeight = 200F;
            this.colFaction.HeaderText = "Faction";
            this.colFaction.MinimumWidth = 8;
            this.colFaction.Name = "colFaction";
            this.colFaction.ReadOnly = true;
            this.colFaction.ToolTipText = "Mission giver faction";
            // 
            // colMissions
            // 
            this.colMissions.HeaderText = "Missions";
            this.colMissions.MinimumWidth = 8;
            this.colMissions.Name = "colMissions";
            this.colMissions.ReadOnly = true;
            this.colMissions.ToolTipText = "Number of missions";
            // 
            // colInfluence
            // 
            this.colInfluence.HeaderText = "Inf +";
            this.colInfluence.MinimumWidth = 8;
            this.colInfluence.Name = "colInfluence";
            this.colInfluence.ReadOnly = true;
            this.colInfluence.ToolTipText = "Faction influence gained";
            // 
            // colReputation
            // 
            this.colReputation.HeaderText = "Rep +";
            this.colReputation.MinimumWidth = 8;
            this.colReputation.Name = "colReputation";
            this.colReputation.ReadOnly = true;
            this.colReputation.ToolTipText = "Faction reputation gained";
            // 
            // colMissionCredits
            // 
            this.colMissionCredits.FillWeight = 130F;
            this.colMissionCredits.HeaderText = "Mission Credits";
            this.colMissionCredits.MinimumWidth = 8;
            this.colMissionCredits.Name = "colMissionCredits";
            this.colMissionCredits.ReadOnly = true;
            this.colMissionCredits.ToolTipText = "Mission Credits earned";
            // 
            // CBought
            // 
            this.CBought.HeaderText = "Commds +";
            this.CBought.MinimumWidth = 8;
            this.CBought.Name = "CBought";
            this.CBought.ReadOnly = true;
            this.CBought.ToolTipText = "Commodities Bought";
            // 
            // CSold
            // 
            this.CSold.HeaderText = "Commds -";
            this.CSold.MinimumWidth = 8;
            this.CSold.Name = "CSold";
            this.CSold.ToolTipText = "Commodities Sold";
            // 
            // MBought
            // 
            this.MBought.HeaderText = "Mats +";
            this.MBought.MinimumWidth = 8;
            this.MBought.Name = "MBought";
            this.MBought.ReadOnly = true;
            this.MBought.ToolTipText = "Materials Acquired";
            // 
            // MSold
            // 
            this.MSold.HeaderText = "Mats -";
            this.MSold.MinimumWidth = 8;
            this.MSold.Name = "MSold";
            this.MSold.ReadOnly = true;
            this.MSold.ToolTipText = "Materials Given";
            // 
            // CrimeCommitted
            // 
            this.CrimeCommitted.HeaderText = "Crimes Committed";
            this.CrimeCommitted.MinimumWidth = 8;
            this.CrimeCommitted.Name = "CrimeCommitted";
            this.CrimeCommitted.ToolTipText = "How many times you\'ve been naughty";
            // 
            // BountyKills
            // 
            this.BountyKills.HeaderText = "Bounty Kills";
            this.BountyKills.MinimumWidth = 8;
            this.BountyKills.Name = "BountyKills";
            this.BountyKills.ReadOnly = true;
            this.BountyKills.ToolTipText = "Number of bounty kills";
            // 
            // BountyValue
            // 
            this.BountyValue.HeaderText = "Bounty Rewards";
            this.BountyValue.MinimumWidth = 8;
            this.BountyValue.Name = "BountyValue";
            this.BountyValue.ReadOnly = true;
            this.BountyValue.ToolTipText = "Number of bounty rewards";
            // 
            // BountyRewardsValue
            // 
            this.BountyRewardsValue.HeaderText = "Bounty Rewards Value";
            this.BountyRewardsValue.MinimumWidth = 8;
            this.BountyRewardsValue.Name = "BountyRewardsValue";
            this.BountyRewardsValue.ReadOnly = true;
            this.BountyRewardsValue.ToolTipText = "Value of bounty rewards";
            // 
            // Interdicted
            // 
            this.Interdicted.HeaderText = "Interdicted";
            this.Interdicted.MinimumWidth = 8;
            this.Interdicted.Name = "Interdicted";
            this.Interdicted.ReadOnly = true;
            this.Interdicted.ToolTipText = "How many times you\'ve been pulled over";
            // 
            // Interdiction
            // 
            this.Interdiction.HeaderText = "Interdiction";
            this.Interdiction.MinimumWidth = 8;
            this.Interdiction.Name = "Interdiction";
            this.Interdiction.ReadOnly = true;
            this.Interdiction.ToolTipText = "Number of times you\'ve interdicted someone";
            // 
            // KillBondVictim
            // 
            this.KillBondVictim.HeaderText = "Kill Bonds Victim";
            this.KillBondVictim.MinimumWidth = 8;
            this.KillBondVictim.Name = "KillBondVictim";
            this.KillBondVictim.ReadOnly = true;
            this.KillBondVictim.ToolTipText = "Number of kill bonds with faction as a victim";
            // 
            // KillBondsAward
            // 
            this.KillBondsAward.HeaderText = "Kill Bonds Awarded";
            this.KillBondsAward.MinimumWidth = 8;
            this.KillBondsAward.Name = "KillBondsAward";
            this.KillBondsAward.ReadOnly = true;
            this.KillBondsAward.ToolTipText = "Number of kill bonds with faction awarded";
            // 
            // KillBondsValue
            // 
            this.KillBondsValue.HeaderText = "Kill Bonds Value";
            this.KillBondsValue.MinimumWidth = 8;
            this.KillBondsValue.Name = "KillBondsValue";
            this.KillBondsValue.ReadOnly = true;
            this.KillBondsValue.ToolTipText = "Value of kill bonds awarded by the faction";
            // 
            // colInfo
            // 
            this.colInfo.FillWeight = 150F;
            this.colInfo.HeaderText = "Other Info";
            this.colInfo.MinimumWidth = 8;
            this.colInfo.Name = "colInfo";
            this.colInfo.ReadOnly = true;
            this.colInfo.ToolTipText = "Other Information";
            // 
            // UserControlFactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.dataViewScrollerPanelFactions);
            this.Controls.Add(this.datePanel);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "UserControlFactions";
            this.Size = new System.Drawing.Size(853, 572);
            this.datePanel.ResumeLayout(false);
            this.datePanel.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.dataViewScrollerPanelFactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFactions)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel datePanel;
        private ExtendedControls.ExtDateTimePicker startDateTime;
        private System.Windows.Forms.Label labelTo;
        private ExtendedControls.ExtDateTimePicker endDateTime;
        private System.Windows.Forms.Label labelValue;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelFactions;
        private ExtendedControls.ExtScrollBar scrollBarFactions;
        private BaseUtils.DataGridViewColumnHider dataGridViewFactions;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn MBought;
        private System.Windows.Forms.DataGridViewTextBoxColumn MSold;
        private System.Windows.Forms.DataGridViewTextBoxColumn CrimeCommitted;
        private System.Windows.Forms.DataGridViewTextBoxColumn BountyKills;
        private System.Windows.Forms.DataGridViewTextBoxColumn BountyValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn BountyRewardsValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn Interdicted;
        private System.Windows.Forms.DataGridViewTextBoxColumn Interdiction;
        private System.Windows.Forms.DataGridViewTextBoxColumn KillBondVictim;
        private System.Windows.Forms.DataGridViewTextBoxColumn KillBondsAward;
        private System.Windows.Forms.DataGridViewTextBoxColumn KillBondsValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfo;
    }
}
