/*
 * Copyright © 2016 - 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
namespace EDDiscovery.UserControls
{
    partial class UserControlCombatPanel
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
            this.panelStatus = new System.Windows.Forms.Panel();
            this.labelCredits = new ExtendedControls.ExtLabelBitmap();
            this.labelTotalKills = new ExtendedControls.ExtLabelBitmap();
            this.labelFactionKills = new ExtendedControls.ExtLabelBitmap();
            this.labelBalance = new ExtendedControls.ExtLabelBitmap();
            this.labelDied = new ExtendedControls.ExtLabelBitmap();
            this.labelTotalCrimes = new ExtendedControls.ExtLabelBitmap();
            this.labelTarget = new ExtendedControls.ExtLabelBitmap();
            this.labelFaction = new ExtendedControls.ExtLabelBitmap();
            this.labelTotalReward = new ExtendedControls.ExtLabelBitmap();
            this.labelFactionReward = new ExtendedControls.ExtLabelBitmap();
            this.checkBoxCustomGridOn = new ExtendedControls.ExtCheckBox();
            this.comboBoxCustomCampaign = new ExtendedControls.ExtComboBox();
            this.buttonExtEditCampaign = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanelCombat = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.dataGridViewCombat = new BaseUtils.DataGridViewColumnControl();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reward = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelStatus.SuspendLayout();
            this.dataViewScrollerPanelCombat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCombat)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelStatus
            // 
            this.panelStatus.Controls.Add(this.labelCredits);
            this.panelStatus.Controls.Add(this.labelTotalKills);
            this.panelStatus.Controls.Add(this.labelFactionKills);
            this.panelStatus.Controls.Add(this.labelBalance);
            this.panelStatus.Controls.Add(this.labelDied);
            this.panelStatus.Controls.Add(this.labelTotalCrimes);
            this.panelStatus.Controls.Add(this.labelTarget);
            this.panelStatus.Controls.Add(this.labelFaction);
            this.panelStatus.Controls.Add(this.labelTotalReward);
            this.panelStatus.Controls.Add(this.labelFactionReward);
            this.panelStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelStatus.Location = new System.Drawing.Point(0, 30);
            this.panelStatus.Name = "panelStatus";
            this.panelStatus.Size = new System.Drawing.Size(713, 80);
            this.panelStatus.TabIndex = 5;
            // 
            // labelCredits
            // 
            this.labelCredits.Location = new System.Drawing.Point(4, 25);
            this.labelCredits.Name = "labelCredits";
            this.labelCredits.Size = new System.Drawing.Size(142, 24);
            this.labelCredits.TabIndex = 4;
            this.labelCredits.Text = "Credits";
            this.labelCredits.TextBackColor = System.Drawing.SystemColors.Control;
            this.toolTip.SetToolTip(this.labelCredits, "Current Credits");
            // 
            // labelTotalKills
            // 
            this.labelTotalKills.Location = new System.Drawing.Point(145, 25);
            this.labelTotalKills.Name = "labelTotalKills";
            this.labelTotalKills.Size = new System.Drawing.Size(100, 24);
            this.labelTotalKills.TabIndex = 4;
            this.labelTotalKills.Text = "Kills";
            this.labelTotalKills.TextBackColor = System.Drawing.SystemColors.Control;
            this.toolTip.SetToolTip(this.labelTotalKills, "Total kills (NPC/PVP) in campaign");
            // 
            // labelFactionKills
            // 
            this.labelFactionKills.Location = new System.Drawing.Point(145, 51);
            this.labelFactionKills.Name = "labelFactionKills";
            this.labelFactionKills.Size = new System.Drawing.Size(100, 24);
            this.labelFactionKills.TabIndex = 4;
            this.labelFactionKills.Text = "Faction Kills";
            this.labelFactionKills.TextBackColor = System.Drawing.SystemColors.Control;
            this.toolTip.SetToolTip(this.labelFactionKills, "Faction Kills");
            // 
            // labelBalance
            // 
            this.labelBalance.Location = new System.Drawing.Point(245, 25);
            this.labelBalance.Name = "labelBalance";
            this.labelBalance.Size = new System.Drawing.Size(100, 24);
            this.labelBalance.TabIndex = 4;
            this.labelBalance.Text = "Balance";
            this.labelBalance.TextBackColor = System.Drawing.SystemColors.Control;
            this.toolTip.SetToolTip(this.labelBalance, "Total reward less any costs (fines, rebuys etc)");
            // 
            // labelDied
            // 
            this.labelDied.Location = new System.Drawing.Point(570, 25);
            this.labelDied.Name = "labelDied";
            this.labelDied.Size = new System.Drawing.Size(100, 24);
            this.labelDied.TabIndex = 4;
            this.labelDied.Text = "Died";
            this.labelDied.TextBackColor = System.Drawing.SystemColors.Control;
            // 
            // labelTotalCrimes
            // 
            this.labelTotalCrimes.Location = new System.Drawing.Point(455, 25);
            this.labelTotalCrimes.Name = "labelTotalCrimes";
            this.labelTotalCrimes.Size = new System.Drawing.Size(100, 24);
            this.labelTotalCrimes.TabIndex = 4;
            this.labelTotalCrimes.Text = "Crimes";
            this.labelTotalCrimes.TextBackColor = System.Drawing.SystemColors.Control;
            this.toolTip.SetToolTip(this.labelTotalCrimes, "How many times you\'ve been caught!");
            // 
            // labelTarget
            // 
            this.labelTarget.Location = new System.Drawing.Point(3, 1);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(720, 24);
            this.labelTarget.TabIndex = 4;
            this.labelTarget.Text = "Target";
            this.labelTarget.TextBackColor = System.Drawing.SystemColors.Control;
            // 
            // labelFaction
            // 
            this.labelFaction.Location = new System.Drawing.Point(3, 51);
            this.labelFaction.Name = "labelFaction";
            this.labelFaction.Size = new System.Drawing.Size(143, 24);
            this.labelFaction.TabIndex = 4;
            this.labelFaction.Text = "Faction";
            this.labelFaction.TextBackColor = System.Drawing.SystemColors.Control;
            this.toolTip.SetToolTip(this.labelFaction, "Target Faction");
            // 
            // labelTotalReward
            // 
            this.labelTotalReward.Location = new System.Drawing.Point(349, 25);
            this.labelTotalReward.Name = "labelTotalReward";
            this.labelTotalReward.Size = new System.Drawing.Size(100, 24);
            this.labelTotalReward.TabIndex = 4;
            this.labelTotalReward.Text = "TotalReward";
            this.labelTotalReward.TextBackColor = System.Drawing.SystemColors.Control;
            this.toolTip.SetToolTip(this.labelTotalReward, "Total reward");
            // 
            // labelFactionReward
            // 
            this.labelFactionReward.Location = new System.Drawing.Point(245, 51);
            this.labelFactionReward.Name = "labelFactionReward";
            this.labelFactionReward.Size = new System.Drawing.Size(100, 24);
            this.labelFactionReward.TabIndex = 4;
            this.labelFactionReward.Text = "FactionReward";
            this.labelFactionReward.TextBackColor = System.Drawing.SystemColors.Control;
            this.toolTip.SetToolTip(this.labelFactionReward, "Reward associated with destroying the faction ships");
            // 
            // checkBoxCustomGridOn
            // 
            this.checkBoxCustomGridOn.AutoSize = true;
            this.checkBoxCustomGridOn.ButtonGradientDirection = 90F;
            this.checkBoxCustomGridOn.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomGridOn.CheckBoxGradientDirection = 225F;
            this.checkBoxCustomGridOn.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomGridOn.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomGridOn.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomGridOn.DisabledScaling = 0.5F;
            this.checkBoxCustomGridOn.ImageIndeterminate = null;
            this.checkBoxCustomGridOn.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomGridOn.ImageUnchecked = null;
            this.checkBoxCustomGridOn.Location = new System.Drawing.Point(321, 4);
            this.checkBoxCustomGridOn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.checkBoxCustomGridOn.MouseOverScaling = 1.3F;
            this.checkBoxCustomGridOn.MouseSelectedScaling = 1.3F;
            this.checkBoxCustomGridOn.Name = "checkBoxCustomGridOn";
            this.checkBoxCustomGridOn.Size = new System.Drawing.Size(45, 17);
            this.checkBoxCustomGridOn.TabIndex = 3;
            this.checkBoxCustomGridOn.Text = "Grid";
            this.checkBoxCustomGridOn.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomGridOn, "Show grid when in transparent mode");
            this.checkBoxCustomGridOn.UseVisualStyleBackColor = true;
            // 
            // comboBoxCustomCampaign
            // 
            this.comboBoxCustomCampaign.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxCustomCampaign.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomCampaign.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxCustomCampaign.DataSource = null;
            this.comboBoxCustomCampaign.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomCampaign.DisabledScaling = 0.5F;
            this.comboBoxCustomCampaign.DisplayMember = "";
            this.comboBoxCustomCampaign.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomCampaign.GradientDirection = 90F;
            this.comboBoxCustomCampaign.Location = new System.Drawing.Point(3, 4);
            this.comboBoxCustomCampaign.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.comboBoxCustomCampaign.MouseOverScalingColor = 1.3F;
            this.comboBoxCustomCampaign.Name = "comboBoxCustomCampaign";
            this.comboBoxCustomCampaign.SelectedIndex = -1;
            this.comboBoxCustomCampaign.SelectedItem = null;
            this.comboBoxCustomCampaign.SelectedValue = null;
            this.comboBoxCustomCampaign.Size = new System.Drawing.Size(222, 21);
            this.comboBoxCustomCampaign.TabIndex = 1;
            this.comboBoxCustomCampaign.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCustomCampaign, "Select Campaign to view");
            this.comboBoxCustomCampaign.ValueMember = "";
            this.comboBoxCustomCampaign.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomCampaign_SelectedIndexChanged);
            // 
            // buttonExtEditCampaign
            // 
            this.buttonExtEditCampaign.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtEditCampaign.ButtonDisabledScaling = 0.5F;
            this.buttonExtEditCampaign.GradientDirection = 90F;
            this.buttonExtEditCampaign.Location = new System.Drawing.Point(231, 1);
            this.buttonExtEditCampaign.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtEditCampaign.MouseOverScaling = 1.3F;
            this.buttonExtEditCampaign.MouseSelectedScaling = 1.3F;
            this.buttonExtEditCampaign.Name = "buttonExtEditCampaign";
            this.buttonExtEditCampaign.Size = new System.Drawing.Size(50, 24);
            this.buttonExtEditCampaign.TabIndex = 2;
            this.buttonExtEditCampaign.Text = "Edit";
            this.toolTip.SetToolTip(this.buttonExtEditCampaign, "Edit user defined campaign");
            this.buttonExtEditCampaign.UseVisualStyleBackColor = true;
            this.buttonExtEditCampaign.Click += new System.EventHandler(this.buttonExtEditCampaign_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // dataViewScrollerPanelCombat
            // 
            this.dataViewScrollerPanelCombat.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanelCombat.Controls.Add(this.dataGridViewCombat);
            this.dataViewScrollerPanelCombat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanelCombat.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanelCombat.Location = new System.Drawing.Point(0, 110);
            this.dataViewScrollerPanelCombat.Name = "dataViewScrollerPanelCombat";
            this.dataViewScrollerPanelCombat.ScrollBarWidth = 24;
            this.dataViewScrollerPanelCombat.Size = new System.Drawing.Size(713, 251);
            this.dataViewScrollerPanelCombat.TabIndex = 3;
            this.dataViewScrollerPanelCombat.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.AlwaysHideScrollBar = false;
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = false;
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(689, 0);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(24, 251);
            this.vScrollBarCustom1.SkinnyStyle = false;
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SliderDrawAngle = 90F;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 1;
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // dataGridViewCombat
            // 
            this.dataGridViewCombat.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewCombat.AllowUserToAddRows = false;
            this.dataGridViewCombat.AllowUserToDeleteRows = false;
            this.dataGridViewCombat.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCombat.AutoSortByColumnName = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCombat.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewCombat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCombat.ColumnReorder = true;
            this.dataGridViewCombat.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.Event,
            this.Description,
            this.Reward});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewCombat.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewCombat.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCombat.Name = "dataGridViewCombat";
            this.dataGridViewCombat.PerColumnWordWrapControl = true;
            this.dataGridViewCombat.RowHeaderMenuStrip = null;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewCombat.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewCombat.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCombat.SingleRowSelect = true;
            this.dataGridViewCombat.Size = new System.Drawing.Size(689, 251);
            this.dataGridViewCombat.TabIndex = 0;
            this.dataGridViewCombat.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCombat_CellDoubleClick);
            this.dataGridViewCombat.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewCombat_SortCompare);
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // Event
            // 
            this.Event.HeaderText = "Event";
            this.Event.Name = "Event";
            this.Event.ReadOnly = true;
            // 
            // Description
            // 
            this.Description.FillWeight = 200F;
            this.Description.HeaderText = "Description";
            this.Description.Name = "Description";
            this.Description.ReadOnly = true;
            // 
            // Reward
            // 
            this.Reward.HeaderText = "Reward";
            this.Reward.Name = "Reward";
            this.Reward.ReadOnly = true;
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.comboBoxCustomCampaign);
            this.panelTop.Controls.Add(this.buttonExtEditCampaign);
            this.panelTop.Controls.Add(this.buttonFilter);
            this.panelTop.Controls.Add(this.checkBoxCustomGridOn);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(713, 30);
            this.panelTop.TabIndex = 4;
            // 
            // buttonFilter
            // 
            this.buttonFilter.BackColor2 = System.Drawing.Color.Red;
            this.buttonFilter.ButtonDisabledScaling = 0.5F;
            this.buttonFilter.GradientDirection = 90F;
            this.buttonFilter.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.buttonFilter.Location = new System.Drawing.Point(287, 1);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonFilter.MouseOverScaling = 1.3F;
            this.buttonFilter.MouseSelectedScaling = 1.3F;
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(28, 28);
            this.buttonFilter.TabIndex = 5;
            this.toolTip1.SetToolTip(this.buttonFilter, "Filter out items");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 30000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ShowAlways = true;
            // 
            // UserControlCombatPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanelCombat);
            this.Controls.Add(this.panelStatus);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlCombatPanel";
            this.Size = new System.Drawing.Size(713, 361);
            this.panelStatus.ResumeLayout(false);
            this.dataViewScrollerPanelCombat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCombat)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtComboBox comboBoxCustomCampaign;
        private ExtendedControls.ExtButton buttonExtEditCampaign;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanelCombat;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private BaseUtils.DataGridViewColumnControl dataGridViewCombat;
        private ExtendedControls.ExtLabelBitmap labelTotalKills;
        private ExtendedControls.ExtLabelBitmap labelFactionKills;
        private ExtendedControls.ExtLabelBitmap labelTotalCrimes;
        private ExtendedControls.ExtLabelBitmap labelTotalReward;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Event;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reward;
        private ExtendedControls.ExtLabelBitmap labelFaction;
        private ExtendedControls.ExtLabelBitmap labelFactionReward;
        private ExtendedControls.ExtLabelBitmap labelBalance;
        private System.Windows.Forms.Panel panelStatus;
        private ExtendedControls.ExtLabelBitmap labelCredits;
        private ExtendedControls.ExtCheckBox checkBoxCustomGridOn;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtLabelBitmap labelTarget;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private ExtendedControls.ExtLabelBitmap labelDied;
        private ExtendedControls.ExtButton buttonFilter;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
