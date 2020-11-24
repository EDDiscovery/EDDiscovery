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
            this.scrollBarFactions = new ExtendedControls.ExtScrollBar();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.dataViewScrollerPanelFactions = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewFactions = new BaseUtils.DataGridViewColumnHider();
            this.colFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMissions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSystems = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfluence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReputation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRewards = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataViewScrollerPanelSystems = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.scrollBarSystems = new ExtendedControls.ExtScrollBar();
            this.dataGridViewSystems = new BaseUtils.DataGridViewColumnHider();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMissionsForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sColSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sColInfluence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sColFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sColAllegience = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sColGovernment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sColState = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.dataViewScrollerPanelFactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFactions)).BeginInit();
            this.dataViewScrollerPanelSystems.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSystems)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
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
            this.datePanel.Name = "datePanel";
            this.datePanel.Size = new System.Drawing.Size(1280, 42);
            this.datePanel.TabIndex = 1;
            // 
            // startDateTime
            // 
            this.startDateTime.BorderColor = System.Drawing.Color.Transparent;
            this.startDateTime.BorderColorScaling = 0.5F;
            this.startDateTime.Checked = false;
            this.startDateTime.CustomFormat = "yyyy/MM/dd | HH:mm:ss";
            this.startDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.startDateTime.Location = new System.Drawing.Point(0, 2);
            this.startDateTime.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.startDateTime.Name = "startDateTime";
            this.startDateTime.SelectedColor = System.Drawing.Color.Yellow;
            this.startDateTime.ShowCheckBox = true;
            this.startDateTime.ShowUpDown = false;
            this.startDateTime.Size = new System.Drawing.Size(330, 31);
            this.startDateTime.TabIndex = 2;
            this.startDateTime.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.startDateTime.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            this.startDateTime.ValueChanged += new System.EventHandler(this.startDateTime_ValueChanged);
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(342, 6);
            this.labelTo.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(23, 20);
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
            this.endDateTime.Location = new System.Drawing.Point(376, 2);
            this.endDateTime.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.endDateTime.Name = "endDateTime";
            this.endDateTime.SelectedColor = System.Drawing.Color.Yellow;
            this.endDateTime.ShowCheckBox = true;
            this.endDateTime.ShowUpDown = false;
            this.endDateTime.Size = new System.Drawing.Size(327, 31);
            this.endDateTime.TabIndex = 3;
            this.endDateTime.TextBackColor = System.Drawing.SystemColors.ControlLight;
            this.endDateTime.Value = new System.DateTime(2017, 4, 7, 9, 2, 29, 549);
            this.endDateTime.ValueChanged += new System.EventHandler(this.endDateTime_ValueChanged);
            // 
            // labelValue
            // 
            this.labelValue.AutoSize = true;
            this.labelValue.Location = new System.Drawing.Point(716, 8);
            this.labelValue.Margin = new System.Windows.Forms.Padding(0, 2, 12, 2);
            this.labelValue.Name = "labelValue";
            this.labelValue.Size = new System.Drawing.Size(62, 20);
            this.labelValue.TabIndex = 5;
            this.labelValue.Text = "<code>";
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
            this.scrollBarFactions.Location = new System.Drawing.Point(1256, 0);
            this.scrollBarFactions.Maximum = -1;
            this.scrollBarFactions.Minimum = 0;
            this.scrollBarFactions.MouseOverButtonColor = System.Drawing.Color.Green;
            this.scrollBarFactions.MousePressedButtonColor = System.Drawing.Color.Red;
            this.scrollBarFactions.Name = "scrollBarFactions";
            this.scrollBarFactions.Size = new System.Drawing.Size(24, 426);
            this.scrollBarFactions.SliderColor = System.Drawing.Color.DarkGray;
            this.scrollBarFactions.SmallChange = 1;
            this.scrollBarFactions.TabIndex = 2;
            this.scrollBarFactions.Text = "scrollBarFactions";
            this.scrollBarFactions.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.scrollBarFactions.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.scrollBarFactions.ThumbColorScaling = 0.5F;
            this.scrollBarFactions.ThumbDrawAngle = 0F;
            this.scrollBarFactions.Value = -1;
            this.scrollBarFactions.ValueLimited = -1;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 42);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dataViewScrollerPanelFactions);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataViewScrollerPanelSystems);
            this.splitContainer.Size = new System.Drawing.Size(1280, 838);
            this.splitContainer.SplitterDistance = 426;
            this.splitContainer.TabIndex = 2;
            // 
            // dataViewScrollerPanelFactions
            // 
            this.dataViewScrollerPanelFactions.Controls.Add(this.scrollBarFactions);
            this.dataViewScrollerPanelFactions.Controls.Add(this.dataGridViewFactions);
            this.dataViewScrollerPanelFactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelFactions.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelFactions.LimitLargeChange = 2147483647;
            this.dataViewScrollerPanelFactions.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanelFactions.Name = "dataViewScrollerPanelFactions";
            this.dataViewScrollerPanelFactions.Size = new System.Drawing.Size(1280, 426);
            this.dataViewScrollerPanelFactions.TabIndex = 2;
            this.dataViewScrollerPanelFactions.VerticalScrollBarDockRight = true;
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
            this.colSystems,
            this.colInfluence,
            this.colReputation,
            this.colCredits,
            this.colRewards});
            this.dataGridViewFactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFactions.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewFactions.MultiSelect = false;
            this.dataGridViewFactions.Name = "dataGridViewFactions";
            this.dataGridViewFactions.RowHeaderMenuStrip = null;
            this.dataGridViewFactions.RowHeadersVisible = false;
            this.dataGridViewFactions.RowHeadersWidth = 62;
            this.dataGridViewFactions.RowTemplate.Height = 28;
            this.dataGridViewFactions.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewFactions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewFactions.SingleRowSelect = true;
            this.dataGridViewFactions.Size = new System.Drawing.Size(1256, 426);
            this.dataGridViewFactions.TabIndex = 0;
            this.dataGridViewFactions.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFactions_CellClick);
            this.dataGridViewFactions.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFactions_CellDoubleClick);
            // 
            // colFaction
            // 
            this.colFaction.FillWeight = 90F;
            this.colFaction.HeaderText = "Faction";
            this.colFaction.MinimumWidth = 220;
            this.colFaction.Name = "colFaction";
            this.colFaction.ReadOnly = true;
            this.colFaction.ToolTipText = "Mission giver faction";
            // 
            // colMissions
            // 
            this.colMissions.FillWeight = 80F;
            this.colMissions.HeaderText = "Missions";
            this.colMissions.MinimumWidth = 20;
            this.colMissions.Name = "colMissions";
            this.colMissions.ReadOnly = true;
            this.colMissions.ToolTipText = "Number of missions";
            // 
            // colSystems
            // 
            this.colSystems.HeaderText = "Systems";
            this.colSystems.MinimumWidth = 8;
            this.colSystems.Name = "colSystems";
            this.colSystems.ReadOnly = true;
            // 
            // colInfluence
            // 
            this.colInfluence.FillWeight = 80F;
            this.colInfluence.HeaderText = "+Influence Total";
            this.colInfluence.MinimumWidth = 20;
            this.colInfluence.Name = "colInfluence";
            this.colInfluence.ReadOnly = true;
            this.colInfluence.ToolTipText = "Faction influence gained";
            // 
            // colReputation
            // 
            this.colReputation.FillWeight = 80F;
            this.colReputation.HeaderText = "+Reputation";
            this.colReputation.MinimumWidth = 20;
            this.colReputation.Name = "colReputation";
            this.colReputation.ReadOnly = true;
            this.colReputation.ToolTipText = "Faction reputation gained";
            // 
            // colCredits
            // 
            this.colCredits.FillWeight = 80F;
            this.colCredits.HeaderText = "Credits";
            this.colCredits.MinimumWidth = 40;
            this.colCredits.Name = "colCredits";
            this.colCredits.ReadOnly = true;
            this.colCredits.ToolTipText = "Credits earned";
            // 
            // colRewards
            // 
            this.colRewards.HeaderText = "Other Rewards";
            this.colRewards.MinimumWidth = 250;
            this.colRewards.Name = "colRewards";
            this.colRewards.ReadOnly = true;
            this.colRewards.ToolTipText = "Other rewards gained";
            // 
            // dataViewScrollerPanelSystems
            // 
            this.dataViewScrollerPanelSystems.Controls.Add(this.scrollBarSystems);
            this.dataViewScrollerPanelSystems.Controls.Add(this.dataGridViewSystems);
            this.dataViewScrollerPanelSystems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelSystems.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelSystems.LimitLargeChange = 2147483647;
            this.dataViewScrollerPanelSystems.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanelSystems.Name = "dataViewScrollerPanelSystems";
            this.dataViewScrollerPanelSystems.Size = new System.Drawing.Size(1280, 408);
            this.dataViewScrollerPanelSystems.TabIndex = 0;
            this.dataViewScrollerPanelSystems.VerticalScrollBarDockRight = true;
            // 
            // scrollBarSystems
            // 
            this.scrollBarSystems.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.scrollBarSystems.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.scrollBarSystems.ArrowColorScaling = 0.5F;
            this.scrollBarSystems.ArrowDownDrawAngle = 270F;
            this.scrollBarSystems.ArrowUpDrawAngle = 90F;
            this.scrollBarSystems.BorderColor = System.Drawing.Color.White;
            this.scrollBarSystems.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.scrollBarSystems.HideScrollBar = false;
            this.scrollBarSystems.LargeChange = 0;
            this.scrollBarSystems.Location = new System.Drawing.Point(1256, 0);
            this.scrollBarSystems.Maximum = -1;
            this.scrollBarSystems.Minimum = 0;
            this.scrollBarSystems.MouseOverButtonColor = System.Drawing.Color.Green;
            this.scrollBarSystems.MousePressedButtonColor = System.Drawing.Color.Red;
            this.scrollBarSystems.Name = "scrollBarSystems";
            this.scrollBarSystems.Size = new System.Drawing.Size(24, 408);
            this.scrollBarSystems.SliderColor = System.Drawing.Color.DarkGray;
            this.scrollBarSystems.SmallChange = 1;
            this.scrollBarSystems.TabIndex = 1;
            this.scrollBarSystems.Text = "scrollBarSystems";
            this.scrollBarSystems.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.scrollBarSystems.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.scrollBarSystems.ThumbColorScaling = 0.5F;
            this.scrollBarSystems.ThumbDrawAngle = 0F;
            this.scrollBarSystems.Value = -1;
            this.scrollBarSystems.ValueLimited = -1;
            // 
            // dataGridViewSystems
            // 
            this.dataGridViewSystems.AllowUserToAddRows = false;
            this.dataGridViewSystems.AllowUserToDeleteRows = false;
            this.dataGridViewSystems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSystems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSystems.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.sColSystem,
            this.sColInfluence,
            this.sColFaction,
            this.sColAllegience,
            this.sColGovernment,
            this.sColState});
            this.dataGridViewSystems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSystems.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSystems.MultiSelect = false;
            this.dataGridViewSystems.Name = "dataGridViewSystems";
            this.dataGridViewSystems.ReadOnly = true;
            this.dataGridViewSystems.RowHeaderMenuStrip = null;
            this.dataGridViewSystems.RowHeadersVisible = false;
            this.dataGridViewSystems.RowHeadersWidth = 62;
            this.dataGridViewSystems.RowTemplate.Height = 28;
            this.dataGridViewSystems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewSystems.SingleRowSelect = true;
            this.dataGridViewSystems.Size = new System.Drawing.Size(1256, 408);
            this.dataGridViewSystems.TabIndex = 0;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMissionsForFactionToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(290, 36);
            this.contextMenuStrip.Click += new System.EventHandler(this.showMissionsForFactionToolStripMenuItem_Click);
            // 
            // showMissionsForFactionToolStripMenuItem
            // 
            this.showMissionsForFactionToolStripMenuItem.Name = "showMissionsForFactionToolStripMenuItem";
            this.showMissionsForFactionToolStripMenuItem.Size = new System.Drawing.Size(289, 32);
            this.showMissionsForFactionToolStripMenuItem.Text = "Show missions for faction";
            // 
            // sColSystem
            // 
            this.sColSystem.HeaderText = "System";
            this.sColSystem.MinimumWidth = 8;
            this.sColSystem.Name = "sColSystem";
            this.sColSystem.ReadOnly = true;
            // 
            // sColInfluence
            // 
            this.sColInfluence.HeaderText = "+Influence";
            this.sColInfluence.MinimumWidth = 8;
            this.sColInfluence.Name = "sColInfluence";
            this.sColInfluence.ReadOnly = true;
            // 
            // sColFaction
            // 
            this.sColFaction.HeaderText = "Controlling Faction";
            this.sColFaction.MinimumWidth = 8;
            this.sColFaction.Name = "sColFaction";
            this.sColFaction.ReadOnly = true;
            // 
            // sColAllegience
            // 
            this.sColAllegience.HeaderText = "Allegience";
            this.sColAllegience.MinimumWidth = 8;
            this.sColAllegience.Name = "sColAllegience";
            this.sColAllegience.ReadOnly = true;
            // 
            // sColGovernment
            // 
            this.sColGovernment.HeaderText = "Government";
            this.sColGovernment.MinimumWidth = 8;
            this.sColGovernment.Name = "sColGovernment";
            this.sColGovernment.ReadOnly = true;
            // 
            // sColState
            // 
            this.sColState.HeaderText = "State";
            this.sColState.MinimumWidth = 8;
            this.sColState.Name = "sColState";
            this.sColState.ReadOnly = true;
            // 
            // UserControlFactions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.datePanel);
            this.Name = "UserControlFactions";
            this.Size = new System.Drawing.Size(1280, 880);
            this.datePanel.ResumeLayout(false);
            this.datePanel.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.dataViewScrollerPanelFactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFactions)).EndInit();
            this.dataViewScrollerPanelSystems.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSystems)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel datePanel;
        private ExtendedControls.ExtDateTimePicker startDateTime;
        private System.Windows.Forms.Label labelTo;
        private ExtendedControls.ExtDateTimePicker endDateTime;
        private System.Windows.Forms.Label labelValue;
        private System.Windows.Forms.SplitContainer splitContainer;
        private ExtendedControls.ExtScrollBar scrollBarFactions;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelFactions;
        private BaseUtils.DataGridViewColumnHider dataGridViewFactions;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelSystems;
        private ExtendedControls.ExtScrollBar scrollBarSystems;
        private BaseUtils.DataGridViewColumnHider dataGridViewSystems;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showMissionsForFactionToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMissions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSystems;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfluence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReputation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRewards;
        private System.Windows.Forms.DataGridViewTextBoxColumn sColSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn sColInfluence;
        private System.Windows.Forms.DataGridViewTextBoxColumn sColFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn sColAllegience;
        private System.Windows.Forms.DataGridViewTextBoxColumn sColGovernment;
        private System.Windows.Forms.DataGridViewTextBoxColumn sColState;
    }
}
