namespace EDDiscovery.UserControls
{
    partial class UserControlMissionAccounting
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
            this.dataViewScrollerPanelFactions = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.scrollBarFactions = new ExtendedControls.ExtScrollBar();
            this.dataGridViewFactions = new BaseUtils.DataGridViewColumnHider();
            this.colFaction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMissions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInfluence = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReputation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRewards = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showMissionsForFactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datePanel.SuspendLayout();
            this.dataViewScrollerPanelFactions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFactions)).BeginInit();
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
            this.datePanel.Margin = new System.Windows.Forms.Padding(2);
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
            this.toolTip.SetToolTip(this.startDateTime, "Include missions from");
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
            this.toolTip.SetToolTip(this.endDateTime, "Include missions to");
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
            // dataViewScrollerPanelFactions
            // 
            this.dataViewScrollerPanelFactions.Controls.Add(this.scrollBarFactions);
            this.dataViewScrollerPanelFactions.Controls.Add(this.dataGridViewFactions);
            this.dataViewScrollerPanelFactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelFactions.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelFactions.LimitLargeChange = 2147483647;
            this.dataViewScrollerPanelFactions.Location = new System.Drawing.Point(0, 27);
            this.dataViewScrollerPanelFactions.Margin = new System.Windows.Forms.Padding(2);
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
            this.scrollBarFactions.Margin = new System.Windows.Forms.Padding(2);
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
            this.colCredits,
            this.colRewards});
            this.dataGridViewFactions.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridViewFactions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewFactions.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewFactions.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridViewFactions.Name = "dataGridViewFactions";
            this.dataGridViewFactions.RowHeaderMenuStrip = null;
            this.dataGridViewFactions.RowHeadersVisible = false;
            this.dataGridViewFactions.RowHeadersWidth = 62;
            this.dataGridViewFactions.RowTemplate.Height = 28;
            this.dataGridViewFactions.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewFactions.SingleRowSelect = true;
            this.dataGridViewFactions.Size = new System.Drawing.Size(837, 545);
            this.dataGridViewFactions.TabIndex = 0;
            this.dataGridViewFactions.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewFactions_CellDoubleClick);
            this.dataGridViewFactions.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewFactions_SortCompare);
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
            // colInfluence
            // 
            this.colInfluence.FillWeight = 80F;
            this.colInfluence.HeaderText = "+Influence";
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
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showMissionsForFactionToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(211, 26);
            // 
            // showMissionsForFactionToolStripMenuItem
            // 
            this.showMissionsForFactionToolStripMenuItem.Name = "showMissionsForFactionToolStripMenuItem";
            this.showMissionsForFactionToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.showMissionsForFactionToolStripMenuItem.Text = "Show missions for faction";
            this.showMissionsForFactionToolStripMenuItem.Click += new System.EventHandler(this.showMissionsForFactionToolStripMenuItem_Click);
            // 
            // UserControlMissionAccounting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.dataViewScrollerPanelFactions);
            this.Controls.Add(this.datePanel);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "UserControlMissionAccounting";
            this.Size = new System.Drawing.Size(853, 572);
            this.datePanel.ResumeLayout(false);
            this.datePanel.PerformLayout();
            this.dataViewScrollerPanelFactions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewFactions)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn colFaction;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMissions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInfluence;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReputation;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRewards;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showMissionsForFactionToolStripMenuItem;
    }
}
