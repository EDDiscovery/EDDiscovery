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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlFactions));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMissionsForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showCommoditymaterialTradesForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showBountiesAndBondsForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.startDateTimePicker = new ExtendedControls.ExtDateTimePicker();
            this.endDateTimePicker = new ExtendedControls.ExtDateTimePicker();
            this.extCheckBoxShowHideSystemInfo = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowHideMission = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowHideCommodities = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowHideMaterials = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowHideBounties = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowHideInterdictions = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowHideKillBonds = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowHideDataLink = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowHideCartographic = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.labelTo = new System.Windows.Forms.Label();
            this.labelSearch = new System.Windows.Forms.Label();
            this.textBoxSearch = new ExtendedControls.ExtTextBox();
            this.labelInfo = new System.Windows.Forms.Label();
            this.dataViewScrollerPanelFactions = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.scrollBarFactions = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
            this.colFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastRep = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFactionState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFactionGov = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFactionAllegiance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFactionSystemInfluence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFactionOtherSystemInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.colOrganicDataSold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataLinkVictimFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataLinkPayeeFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataLinkPayeeValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.dataViewScrollerPanelFactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMissionsForFactionToolStripMenuItem,
            this.showCommoditymaterialTradesForFactionToolStripMenuItem,
            this.showBountiesAndBondsForFactionToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(310, 70);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
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
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // startDateTimePicker
            // 
            this.startDateTimePicker.BorderColor = System.Drawing.Color.Transparent;
            this.startDateTimePicker.BorderColor2 = System.Drawing.Color.Transparent;
            this.startDateTimePicker.Checked = false;
            this.startDateTimePicker.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.startDateTimePicker.DisabledScaling = 0.5F;
            this.startDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDateTimePicker.Location = new System.Drawing.Point(3, 1);
            this.startDateTimePicker.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
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
            this.endDateTimePicker.BorderColor2 = System.Drawing.Color.Transparent;
            this.endDateTimePicker.Checked = false;
            this.endDateTimePicker.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.endDateTimePicker.DisabledScaling = 0.5F;
            this.endDateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endDateTimePicker.Location = new System.Drawing.Point(249, 1);
            this.endDateTimePicker.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
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
            // extCheckBoxShowHideSystemInfo
            // 
            this.extCheckBoxShowHideSystemInfo.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowHideSystemInfo.ButtonGradientDirection = 90F;
            this.extCheckBoxShowHideSystemInfo.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowHideSystemInfo.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowHideSystemInfo.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowHideSystemInfo.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideSystemInfo.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideSystemInfo.DisabledScaling = 0.5F;
            this.extCheckBoxShowHideSystemInfo.Image = global::EDDiscovery.Icons.Controls.SearchStars;
            this.extCheckBoxShowHideSystemInfo.ImageIndeterminate = null;
            this.extCheckBoxShowHideSystemInfo.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowHideSystemInfo.ImageUnchecked = null;
            this.extCheckBoxShowHideSystemInfo.Location = new System.Drawing.Point(672, 1);
            this.extCheckBoxShowHideSystemInfo.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowHideSystemInfo.MouseOverScaling = 1.3F;
            this.extCheckBoxShowHideSystemInfo.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowHideSystemInfo.Name = "extCheckBoxShowHideSystemInfo";
            this.extCheckBoxShowHideSystemInfo.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowHideSystemInfo.TabIndex = 36;
            this.extCheckBoxShowHideSystemInfo.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowHideSystemInfo, "Show/Hide System Info");
            this.extCheckBoxShowHideSystemInfo.UseVisualStyleBackColor = true;
            this.extCheckBoxShowHideSystemInfo.CheckedChanged += new System.EventHandler(this.extCheckBoxShowHideSystemInfo_CheckedChanged);
            // 
            // extCheckBoxShowHideMission
            // 
            this.extCheckBoxShowHideMission.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowHideMission.ButtonGradientDirection = 90F;
            this.extCheckBoxShowHideMission.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowHideMission.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowHideMission.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowHideMission.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideMission.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideMission.DisabledScaling = 0.5F;
            this.extCheckBoxShowHideMission.Image = global::EDDiscovery.Icons.Controls.Missions;
            this.extCheckBoxShowHideMission.ImageIndeterminate = null;
            this.extCheckBoxShowHideMission.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowHideMission.ImageUnchecked = null;
            this.extCheckBoxShowHideMission.Location = new System.Drawing.Point(706, 1);
            this.extCheckBoxShowHideMission.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowHideMission.MouseOverScaling = 1.3F;
            this.extCheckBoxShowHideMission.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowHideMission.Name = "extCheckBoxShowHideMission";
            this.extCheckBoxShowHideMission.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowHideMission.TabIndex = 36;
            this.extCheckBoxShowHideMission.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowHideMission, "Show/Hide Mission");
            this.extCheckBoxShowHideMission.UseVisualStyleBackColor = true;
            this.extCheckBoxShowHideMission.CheckedChanged += new System.EventHandler(this.extCheckBoxShowHideMission_CheckedChanged);
            // 
            // extCheckBoxShowHideCommodities
            // 
            this.extCheckBoxShowHideCommodities.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowHideCommodities.ButtonGradientDirection = 90F;
            this.extCheckBoxShowHideCommodities.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowHideCommodities.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowHideCommodities.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowHideCommodities.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideCommodities.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideCommodities.DisabledScaling = 0.5F;
            this.extCheckBoxShowHideCommodities.Image = global::EDDiscovery.Icons.Controls.Commodity;
            this.extCheckBoxShowHideCommodities.ImageIndeterminate = null;
            this.extCheckBoxShowHideCommodities.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowHideCommodities.ImageUnchecked = null;
            this.extCheckBoxShowHideCommodities.Location = new System.Drawing.Point(740, 1);
            this.extCheckBoxShowHideCommodities.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowHideCommodities.MouseOverScaling = 1.3F;
            this.extCheckBoxShowHideCommodities.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowHideCommodities.Name = "extCheckBoxShowHideCommodities";
            this.extCheckBoxShowHideCommodities.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowHideCommodities.TabIndex = 36;
            this.extCheckBoxShowHideCommodities.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowHideCommodities, "Show/Hide Commodities");
            this.extCheckBoxShowHideCommodities.UseVisualStyleBackColor = true;
            this.extCheckBoxShowHideCommodities.CheckedChanged += new System.EventHandler(this.extCheckBoxShowHideCommodities_CheckedChanged);
            // 
            // extCheckBoxShowHideMaterials
            // 
            this.extCheckBoxShowHideMaterials.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowHideMaterials.ButtonGradientDirection = 90F;
            this.extCheckBoxShowHideMaterials.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowHideMaterials.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowHideMaterials.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowHideMaterials.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideMaterials.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideMaterials.DisabledScaling = 0.5F;
            this.extCheckBoxShowHideMaterials.Image = global::EDDiscovery.Icons.Controls.Materials;
            this.extCheckBoxShowHideMaterials.ImageIndeterminate = null;
            this.extCheckBoxShowHideMaterials.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowHideMaterials.ImageUnchecked = null;
            this.extCheckBoxShowHideMaterials.Location = new System.Drawing.Point(774, 1);
            this.extCheckBoxShowHideMaterials.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowHideMaterials.MouseOverScaling = 1.3F;
            this.extCheckBoxShowHideMaterials.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowHideMaterials.Name = "extCheckBoxShowHideMaterials";
            this.extCheckBoxShowHideMaterials.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowHideMaterials.TabIndex = 36;
            this.extCheckBoxShowHideMaterials.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowHideMaterials, "Show/Hide Materials");
            this.extCheckBoxShowHideMaterials.UseVisualStyleBackColor = true;
            this.extCheckBoxShowHideMaterials.CheckedChanged += new System.EventHandler(this.extCheckBoxShowHideMaterials_CheckedChanged);
            // 
            // extCheckBoxShowHideBounties
            // 
            this.extCheckBoxShowHideBounties.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowHideBounties.ButtonGradientDirection = 90F;
            this.extCheckBoxShowHideBounties.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowHideBounties.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowHideBounties.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowHideBounties.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideBounties.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideBounties.DisabledScaling = 0.5F;
            this.extCheckBoxShowHideBounties.Image = global::EDDiscovery.Icons.Controls.Bounty;
            this.extCheckBoxShowHideBounties.ImageIndeterminate = null;
            this.extCheckBoxShowHideBounties.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowHideBounties.ImageUnchecked = null;
            this.extCheckBoxShowHideBounties.Location = new System.Drawing.Point(808, 1);
            this.extCheckBoxShowHideBounties.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowHideBounties.MouseOverScaling = 1.3F;
            this.extCheckBoxShowHideBounties.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowHideBounties.Name = "extCheckBoxShowHideBounties";
            this.extCheckBoxShowHideBounties.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowHideBounties.TabIndex = 36;
            this.extCheckBoxShowHideBounties.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowHideBounties, "Show/Hide Crimes/Bounties");
            this.extCheckBoxShowHideBounties.UseVisualStyleBackColor = true;
            this.extCheckBoxShowHideBounties.CheckedChanged += new System.EventHandler(this.extCheckBoxShowHideBounties_CheckedChanged);
            // 
            // extCheckBoxShowHideInterdictions
            // 
            this.extCheckBoxShowHideInterdictions.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowHideInterdictions.ButtonGradientDirection = 90F;
            this.extCheckBoxShowHideInterdictions.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowHideInterdictions.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowHideInterdictions.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowHideInterdictions.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideInterdictions.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideInterdictions.DisabledScaling = 0.5F;
            this.extCheckBoxShowHideInterdictions.Image = global::EDDiscovery.Icons.Controls.Interdiction;
            this.extCheckBoxShowHideInterdictions.ImageIndeterminate = null;
            this.extCheckBoxShowHideInterdictions.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowHideInterdictions.ImageUnchecked = null;
            this.extCheckBoxShowHideInterdictions.Location = new System.Drawing.Point(842, 1);
            this.extCheckBoxShowHideInterdictions.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowHideInterdictions.MouseOverScaling = 1.3F;
            this.extCheckBoxShowHideInterdictions.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowHideInterdictions.Name = "extCheckBoxShowHideInterdictions";
            this.extCheckBoxShowHideInterdictions.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowHideInterdictions.TabIndex = 36;
            this.extCheckBoxShowHideInterdictions.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowHideInterdictions, "Show/Hide Interdictions");
            this.extCheckBoxShowHideInterdictions.UseVisualStyleBackColor = true;
            this.extCheckBoxShowHideInterdictions.CheckedChanged += new System.EventHandler(this.extCheckBoxShowHideInterdictions_CheckedChanged);
            // 
            // extCheckBoxShowHideKillBonds
            // 
            this.extCheckBoxShowHideKillBonds.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowHideKillBonds.ButtonGradientDirection = 90F;
            this.extCheckBoxShowHideKillBonds.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowHideKillBonds.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowHideKillBonds.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowHideKillBonds.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideKillBonds.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideKillBonds.DisabledScaling = 0.5F;
            this.extCheckBoxShowHideKillBonds.Image = global::EDDiscovery.Icons.Controls.FactionKillBond;
            this.extCheckBoxShowHideKillBonds.ImageIndeterminate = null;
            this.extCheckBoxShowHideKillBonds.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowHideKillBonds.ImageUnchecked = null;
            this.extCheckBoxShowHideKillBonds.Location = new System.Drawing.Point(876, 1);
            this.extCheckBoxShowHideKillBonds.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowHideKillBonds.MouseOverScaling = 1.3F;
            this.extCheckBoxShowHideKillBonds.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowHideKillBonds.Name = "extCheckBoxShowHideKillBonds";
            this.extCheckBoxShowHideKillBonds.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowHideKillBonds.TabIndex = 36;
            this.extCheckBoxShowHideKillBonds.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowHideKillBonds, "Show/Hide Kill Bonds");
            this.extCheckBoxShowHideKillBonds.UseVisualStyleBackColor = true;
            this.extCheckBoxShowHideKillBonds.CheckedChanged += new System.EventHandler(this.extCheckBoxShowHideKillBonds_CheckedChanged);
            // 
            // extCheckBoxShowHideDataLink
            // 
            this.extCheckBoxShowHideDataLink.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowHideDataLink.ButtonGradientDirection = 90F;
            this.extCheckBoxShowHideDataLink.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowHideDataLink.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowHideDataLink.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowHideDataLink.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideDataLink.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideDataLink.DisabledScaling = 0.5F;
            this.extCheckBoxShowHideDataLink.Image = global::EDDiscovery.Icons.Controls.DatalinkScan;
            this.extCheckBoxShowHideDataLink.ImageIndeterminate = null;
            this.extCheckBoxShowHideDataLink.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowHideDataLink.ImageUnchecked = null;
            this.extCheckBoxShowHideDataLink.Location = new System.Drawing.Point(910, 1);
            this.extCheckBoxShowHideDataLink.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowHideDataLink.MouseOverScaling = 1.3F;
            this.extCheckBoxShowHideDataLink.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowHideDataLink.Name = "extCheckBoxShowHideDataLink";
            this.extCheckBoxShowHideDataLink.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowHideDataLink.TabIndex = 36;
            this.extCheckBoxShowHideDataLink.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowHideDataLink, "Show/Hide Datalink");
            this.extCheckBoxShowHideDataLink.UseVisualStyleBackColor = true;
            this.extCheckBoxShowHideDataLink.CheckedChanged += new System.EventHandler(this.extCheckBoxShowHideDataLink_CheckedChanged);
            // 
            // extCheckBoxShowHideCartographic
            // 
            this.extCheckBoxShowHideCartographic.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowHideCartographic.ButtonGradientDirection = 90F;
            this.extCheckBoxShowHideCartographic.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowHideCartographic.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowHideCartographic.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowHideCartographic.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideCartographic.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowHideCartographic.DisabledScaling = 0.5F;
            this.extCheckBoxShowHideCartographic.Image = global::EDDiscovery.Icons.Controls.SellExplorationData;
            this.extCheckBoxShowHideCartographic.ImageIndeterminate = null;
            this.extCheckBoxShowHideCartographic.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowHideCartographic.ImageUnchecked = null;
            this.extCheckBoxShowHideCartographic.Location = new System.Drawing.Point(944, 1);
            this.extCheckBoxShowHideCartographic.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowHideCartographic.MouseOverScaling = 1.3F;
            this.extCheckBoxShowHideCartographic.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowHideCartographic.Name = "extCheckBoxShowHideCartographic";
            this.extCheckBoxShowHideCartographic.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowHideCartographic.TabIndex = 36;
            this.extCheckBoxShowHideCartographic.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxShowHideCartographic, "Show Hide Cartographic/Organic Sold");
            this.extCheckBoxShowHideCartographic.UseVisualStyleBackColor = true;
            this.extCheckBoxShowHideCartographic.CheckedChanged += new System.EventHandler(this.extCheckBoxShowHideCartographic_CheckedChanged);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtExcel.ButtonDisabledScaling = 0.5F;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.GradientDirection = 90F;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(978, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtExcel.MouseOverScaling = 1.3F;
            this.buttonExtExcel.MouseSelectedScaling = 1.3F;
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 33;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.startDateTimePicker);
            this.panelTop.Controls.Add(this.labelTo);
            this.panelTop.Controls.Add(this.endDateTimePicker);
            this.panelTop.Controls.Add(this.labelSearch);
            this.panelTop.Controls.Add(this.textBoxSearch);
            this.panelTop.Controls.Add(this.extCheckBoxShowHideSystemInfo);
            this.panelTop.Controls.Add(this.extCheckBoxShowHideMission);
            this.panelTop.Controls.Add(this.extCheckBoxShowHideCommodities);
            this.panelTop.Controls.Add(this.extCheckBoxShowHideMaterials);
            this.panelTop.Controls.Add(this.extCheckBoxShowHideBounties);
            this.panelTop.Controls.Add(this.extCheckBoxShowHideInterdictions);
            this.panelTop.Controls.Add(this.extCheckBoxShowHideKillBonds);
            this.panelTop.Controls.Add(this.extCheckBoxShowHideDataLink);
            this.panelTop.Controls.Add(this.extCheckBoxShowHideCartographic);
            this.panelTop.Controls.Add(this.buttonExtExcel);
            this.panelTop.Controls.Add(this.labelInfo);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(2);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1573, 32);
            this.panelTop.TabIndex = 0;
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(227, 4);
            this.labelTo.Margin = new System.Windows.Forms.Padding(1, 4, 3, 1);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(16, 13);
            this.labelTo.TabIndex = 4;
            this.labelTo.Text = "to";
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(471, 4);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(1, 4, 3, 1);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 35;
            this.labelSearch.Text = "Search";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxSearch.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSearch.BorderColor2 = System.Drawing.Color.Transparent;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearch.ClearOnFirstChar = false;
            this.textBoxSearch.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSearch.EndButtonEnable = true;
            this.textBoxSearch.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxSearch.EndButtonImage")));
            this.textBoxSearch.EndButtonSize16ths = 10;
            this.textBoxSearch.EndButtonVisible = false;
            this.textBoxSearch.InErrorCondition = false;
            this.textBoxSearch.Location = new System.Drawing.Point(518, 4);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.textBoxSearch.Multiline = false;
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.ReadOnly = false;
            this.textBoxSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSearch.SelectionLength = 0;
            this.textBoxSearch.SelectionStart = 0;
            this.textBoxSearch.Size = new System.Drawing.Size(148, 20);
            this.textBoxSearch.TabIndex = 34;
            this.textBoxSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxSearch.TextNoChange = "";
            this.textBoxSearch.WordWrap = true;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(1015, 4);
            this.labelInfo.Margin = new System.Windows.Forms.Padding(6, 4, 8, 1);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(43, 13);
            this.labelInfo.TabIndex = 5;
            this.labelInfo.Text = "<code>";
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
            this.dataViewScrollerPanelFactions.ScrollBarWidth = 24;
            this.dataViewScrollerPanelFactions.Size = new System.Drawing.Size(1573, 589);
            this.dataViewScrollerPanelFactions.TabIndex = 1;
            this.dataViewScrollerPanelFactions.VerticalScrollBarDockRight = true;
            // 
            // scrollBarFactions
            // 
            this.scrollBarFactions.AlwaysHideScrollBar = false;
            this.scrollBarFactions.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.scrollBarFactions.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.scrollBarFactions.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.scrollBarFactions.ArrowDownDrawAngle = 270F;
            this.scrollBarFactions.ArrowUpDrawAngle = 90F;
            this.scrollBarFactions.BorderColor = System.Drawing.Color.White;
            this.scrollBarFactions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.scrollBarFactions.HideScrollBar = false;
            this.scrollBarFactions.LargeChange = 0;
            this.scrollBarFactions.Location = new System.Drawing.Point(1549, 0);
            this.scrollBarFactions.Margin = new System.Windows.Forms.Padding(2);
            this.scrollBarFactions.Maximum = -1;
            this.scrollBarFactions.Minimum = 0;
            this.scrollBarFactions.MouseOverButtonColor = System.Drawing.Color.Green;
            this.scrollBarFactions.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.scrollBarFactions.MousePressedButtonColor = System.Drawing.Color.Red;
            this.scrollBarFactions.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.scrollBarFactions.Name = "scrollBarFactions";
            this.scrollBarFactions.Size = new System.Drawing.Size(24, 589);
            this.scrollBarFactions.SkinnyStyle = false;
            this.scrollBarFactions.SliderColor = System.Drawing.Color.DarkGray;
            this.scrollBarFactions.SliderColor2 = System.Drawing.Color.DarkGray;
            this.scrollBarFactions.SliderDrawAngle = 90F;
            this.scrollBarFactions.SmallChange = 1;
            this.scrollBarFactions.TabIndex = 1;
            this.scrollBarFactions.Text = "";
            this.scrollBarFactions.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.scrollBarFactions.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.scrollBarFactions.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
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
            this.colSystem,
            this.colLastRep,
            this.colFactionState,
            this.colFactionGov,
            this.colFactionAllegiance,
            this.colFactionSystemInfluence,
            this.colFactionOtherSystemInfo,
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
            this.colOrganicDataSold,
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
            this.dataGridView.Size = new System.Drawing.Size(1549, 589);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellDoubleClick);
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewFactions_SortCompare);
            // 
            // colFaction
            // 
            this.colFaction.FillWeight = 200F;
            this.colFaction.HeaderText = "Faction";
            this.colFaction.MinimumWidth = 8;
            this.colFaction.Name = "colFaction";
            this.colFaction.ReadOnly = true;
            // 
            // colSystem
            // 
            this.colSystem.HeaderText = "System";
            this.colSystem.Name = "colSystem";
            this.colSystem.ReadOnly = true;
            // 
            // colLastRep
            // 
            this.colLastRep.HeaderText = "Last Known Reputation";
            this.colLastRep.Name = "colLastRep";
            this.colLastRep.ReadOnly = true;
            // 
            // colFactionState
            // 
            this.colFactionState.HeaderText = "Last State";
            this.colFactionState.Name = "colFactionState";
            this.colFactionState.ReadOnly = true;
            // 
            // colFactionGov
            // 
            this.colFactionGov.HeaderText = "Government";
            this.colFactionGov.Name = "colFactionGov";
            this.colFactionGov.ReadOnly = true;
            // 
            // colFactionAllegiance
            // 
            this.colFactionAllegiance.HeaderText = "Allegiance";
            this.colFactionAllegiance.Name = "colFactionAllegiance";
            this.colFactionAllegiance.ReadOnly = true;
            // 
            // colFactionSystemInfluence
            // 
            this.colFactionSystemInfluence.HeaderText = "System Influence";
            this.colFactionSystemInfluence.Name = "colFactionSystemInfluence";
            this.colFactionSystemInfluence.ReadOnly = true;
            // 
            // colFactionOtherSystemInfo
            // 
            this.colFactionOtherSystemInfo.HeaderText = "Other System Info";
            this.colFactionOtherSystemInfo.Name = "colFactionOtherSystemInfo";
            this.colFactionOtherSystemInfo.ReadOnly = true;
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
            this.CSold.ReadOnly = true;
            // 
            // CProfit
            // 
            this.CProfit.FillWeight = 99.59389F;
            this.CProfit.HeaderText = "Commds Profit";
            this.CProfit.MinimumWidth = 8;
            this.CProfit.Name = "CProfit";
            this.CProfit.ReadOnly = true;
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
            this.CrimeCommitted.ReadOnly = true;
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
            // colOrganicDataSold
            // 
            this.colOrganicDataSold.HeaderText = "Organic Data Value";
            this.colOrganicDataSold.Name = "colOrganicDataSold";
            this.colOrganicDataSold.ReadOnly = true;
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
            // UserControlFactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.dataViewScrollerPanelFactions);
            this.Controls.Add(this.panelTop);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserControlFactions";
            this.Size = new System.Drawing.Size(1573, 621);
            this.contextMenuStrip.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.dataViewScrollerPanelFactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
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
        private ExtendedControls.ExtButton buttonExtExcel;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private System.Windows.Forms.Label labelSearch;
        private ExtendedControls.ExtTextBox textBoxSearch;
        private ExtendedControls.ExtCheckBox extCheckBoxShowHideMaterials;
        private ExtendedControls.ExtCheckBox extCheckBoxShowHideSystemInfo;
        private ExtendedControls.ExtCheckBox extCheckBoxShowHideCommodities;
        private ExtendedControls.ExtCheckBox extCheckBoxShowHideBounties;
        private ExtendedControls.ExtCheckBox extCheckBoxShowHideInterdictions;
        private ExtendedControls.ExtCheckBox extCheckBoxShowHideKillBonds;
        private ExtendedControls.ExtCheckBox extCheckBoxShowHideDataLink;
        private ExtendedControls.ExtCheckBox extCheckBoxShowHideMission;
        private ExtendedControls.ExtCheckBox extCheckBoxShowHideCartographic;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastRep;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFactionState;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFactionGov;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFactionAllegiance;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFactionSystemInfluence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFactionOtherSystemInfo;
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrganicDataSold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataLinkVictimFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataLinkPayeeFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataLinkPayeeValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfo;
    }
}
