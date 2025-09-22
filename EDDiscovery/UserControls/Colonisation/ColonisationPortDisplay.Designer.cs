namespace EDDiscovery.UserControls.Colonisation
{
    partial class ColonisationPortDisplay
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
            this.extPanelDataGridViewScrollRL = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarRL = new ExtendedControls.ExtScrollBar();
            this.dataGridViewRL = new System.Windows.Forms.DataGridView();
            this.ColRLName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRLRequired = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRLProvided = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRLRemaining = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRLPayment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extPanelGradientFill1 = new ExtendedControls.ExtPanelGradientFill();
            this.extCheckBoxShowContributions = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxShowRL = new ExtendedControls.ExtCheckBox();
            this.labelDataProgress = new ExtendedControls.LabelData();
            this.labelDataGov = new ExtendedControls.LabelData();
            this.labelDataFaction = new ExtendedControls.LabelData();
            this.extLabelFailed = new ExtendedControls.ExtLabel();
            this.extLabelStationName = new ExtendedControls.ExtLabel();
            this.dataGridViewContributions = new System.Windows.Forms.DataGridView();
            this.colContributionsTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContributionsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colContributionsAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.extPanelDataGridViewScrollContributions = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarContributions = new ExtendedControls.ExtScrollBar();
            this.extPanelGradientFillBot = new ExtendedControls.ExtPanelGradientFill();
            this.extCheckBoxShowZeros = new ExtendedControls.ExtCheckBox();
            this.extPanelDataGridViewScrollRL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRL)).BeginInit();
            this.extPanelGradientFill1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContributions)).BeginInit();
            this.extPanelDataGridViewScrollContributions.SuspendLayout();
            this.SuspendLayout();
            // 
            // extPanelDataGridViewScrollRL
            // 
            this.extPanelDataGridViewScrollRL.Controls.Add(this.extScrollBarRL);
            this.extPanelDataGridViewScrollRL.Controls.Add(this.dataGridViewRL);
            this.extPanelDataGridViewScrollRL.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelDataGridViewScrollRL.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollRL.Location = new System.Drawing.Point(0, 60);
            this.extPanelDataGridViewScrollRL.Name = "extPanelDataGridViewScrollRL";
            this.extPanelDataGridViewScrollRL.ScrollBarWidth = 24;
            this.extPanelDataGridViewScrollRL.Size = new System.Drawing.Size(1117, 227);
            this.extPanelDataGridViewScrollRL.TabIndex = 4;
            this.extPanelDataGridViewScrollRL.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarRL
            // 
            this.extScrollBarRL.AlwaysHideScrollBar = false;
            this.extScrollBarRL.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarRL.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarRL.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.extScrollBarRL.ArrowDownDrawAngle = 270F;
            this.extScrollBarRL.ArrowUpDrawAngle = 90F;
            this.extScrollBarRL.BorderColor = System.Drawing.Color.White;
            this.extScrollBarRL.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarRL.HideScrollBar = false;
            this.extScrollBarRL.LargeChange = 0;
            this.extScrollBarRL.Location = new System.Drawing.Point(1093, 0);
            this.extScrollBarRL.Maximum = -1;
            this.extScrollBarRL.Minimum = 0;
            this.extScrollBarRL.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarRL.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.extScrollBarRL.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarRL.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.extScrollBarRL.Name = "extScrollBarRL";
            this.extScrollBarRL.Size = new System.Drawing.Size(24, 227);
            this.extScrollBarRL.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarRL.SliderColor2 = System.Drawing.Color.DarkGray;
            this.extScrollBarRL.SliderDrawAngle = 90F;
            this.extScrollBarRL.SmallChange = 1;
            this.extScrollBarRL.TabIndex = 1;
            this.extScrollBarRL.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarRL.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarRL.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.extScrollBarRL.ThumbDrawAngle = 0F;
            this.extScrollBarRL.Value = -1;
            this.extScrollBarRL.ValueLimited = -1;
            // 
            // dataGridViewRL
            // 
            this.dataGridViewRL.AllowUserToAddRows = false;
            this.dataGridViewRL.AllowUserToDeleteRows = false;
            this.dataGridViewRL.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColRLName,
            this.ColRLRequired,
            this.ColRLProvided,
            this.ColRLRemaining,
            this.ColRLPayment});
            this.dataGridViewRL.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRL.Name = "dataGridViewRL";
            this.dataGridViewRL.RowHeadersVisible = false;
            this.dataGridViewRL.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewRL.Size = new System.Drawing.Size(1093, 227);
            this.dataGridViewRL.TabIndex = 0;
            // 
            // ColRLName
            // 
            this.ColRLName.FillWeight = 200F;
            this.ColRLName.HeaderText = "Name";
            this.ColRLName.Name = "ColRLName";
            this.ColRLName.ReadOnly = true;
            // 
            // ColRLRequired
            // 
            this.ColRLRequired.HeaderText = "Required";
            this.ColRLRequired.Name = "ColRLRequired";
            this.ColRLRequired.ReadOnly = true;
            // 
            // ColRLProvided
            // 
            this.ColRLProvided.HeaderText = "Provided";
            this.ColRLProvided.Name = "ColRLProvided";
            this.ColRLProvided.ReadOnly = true;
            // 
            // ColRLRemaining
            // 
            this.ColRLRemaining.HeaderText = "Remaining";
            this.ColRLRemaining.Name = "ColRLRemaining";
            // 
            // ColRLPayment
            // 
            this.ColRLPayment.HeaderText = "Payment";
            this.ColRLPayment.Name = "ColRLPayment";
            this.ColRLPayment.ReadOnly = true;
            // 
            // extPanelGradientFill1
            // 
            this.extPanelGradientFill1.ChildrenThemed = true;
            this.extPanelGradientFill1.Controls.Add(this.extCheckBoxShowZeros);
            this.extPanelGradientFill1.Controls.Add(this.extCheckBoxShowContributions);
            this.extPanelGradientFill1.Controls.Add(this.extCheckBoxShowRL);
            this.extPanelGradientFill1.Controls.Add(this.labelDataProgress);
            this.extPanelGradientFill1.Controls.Add(this.labelDataGov);
            this.extPanelGradientFill1.Controls.Add(this.labelDataFaction);
            this.extPanelGradientFill1.Controls.Add(this.extLabelFailed);
            this.extPanelGradientFill1.Controls.Add(this.extLabelStationName);
            this.extPanelGradientFill1.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelGradientFill1.FlowDirection = null;
            this.extPanelGradientFill1.GradientDirection = 0F;
            this.extPanelGradientFill1.Location = new System.Drawing.Point(0, 0);
            this.extPanelGradientFill1.Name = "extPanelGradientFill1";
            this.extPanelGradientFill1.PaintTransparentColor = System.Drawing.Color.Transparent;
            this.extPanelGradientFill1.Size = new System.Drawing.Size(1117, 60);
            this.extPanelGradientFill1.TabIndex = 3;
            this.extPanelGradientFill1.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.extPanelGradientFill1.ThemeColorSet = 1;
            // 
            // extCheckBoxShowContributions
            // 
            this.extCheckBoxShowContributions.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowContributions.BackColor = System.Drawing.SystemColors.Control;
            this.extCheckBoxShowContributions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowContributions.ButtonGradientDirection = 90F;
            this.extCheckBoxShowContributions.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxShowContributions.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowContributions.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowContributions.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowContributions.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowContributions.Checked = true;
            this.extCheckBoxShowContributions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.extCheckBoxShowContributions.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxShowContributions.DisabledScaling = 0.5F;
            this.extCheckBoxShowContributions.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxShowContributions.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxShowContributions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxShowContributions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxShowContributions.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.extCheckBoxShowContributions.Image = global::EDDiscovery.Icons.Controls.CommodityBuy;
            this.extCheckBoxShowContributions.ImageIndeterminate = null;
            this.extCheckBoxShowContributions.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowContributions.ImageUnchecked = null;
            this.extCheckBoxShowContributions.Location = new System.Drawing.Point(39, 28);
            this.extCheckBoxShowContributions.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowContributions.MouseOverScaling = 1.3F;
            this.extCheckBoxShowContributions.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowContributions.Name = "extCheckBoxShowContributions";
            this.extCheckBoxShowContributions.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowContributions.TabIndex = 34;
            this.extCheckBoxShowContributions.TickBoxReductionRatio = 0.75F;
            this.extCheckBoxShowContributions.UseVisualStyleBackColor = false;
            // 
            // extCheckBoxShowRL
            // 
            this.extCheckBoxShowRL.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowRL.BackColor = System.Drawing.SystemColors.Control;
            this.extCheckBoxShowRL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowRL.ButtonGradientDirection = 90F;
            this.extCheckBoxShowRL.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxShowRL.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowRL.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowRL.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowRL.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowRL.Checked = true;
            this.extCheckBoxShowRL.CheckState = System.Windows.Forms.CheckState.Checked;
            this.extCheckBoxShowRL.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxShowRL.DisabledScaling = 0.5F;
            this.extCheckBoxShowRL.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxShowRL.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxShowRL.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxShowRL.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxShowRL.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.extCheckBoxShowRL.Image = global::EDDiscovery.Icons.Controls.Commodity;
            this.extCheckBoxShowRL.ImageIndeterminate = null;
            this.extCheckBoxShowRL.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowRL.ImageUnchecked = null;
            this.extCheckBoxShowRL.Location = new System.Drawing.Point(3, 28);
            this.extCheckBoxShowRL.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowRL.MouseOverScaling = 1.3F;
            this.extCheckBoxShowRL.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowRL.Name = "extCheckBoxShowRL";
            this.extCheckBoxShowRL.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowRL.TabIndex = 34;
            this.extCheckBoxShowRL.TickBoxReductionRatio = 0.75F;
            this.extCheckBoxShowRL.UseVisualStyleBackColor = false;
            // 
            // labelDataProgress
            // 
            this.labelDataProgress.BorderColor = System.Drawing.Color.Orange;
            this.labelDataProgress.BorderWidth = 1;
            this.labelDataProgress.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataProgress.Data = null;
            this.labelDataProgress.DataFont = null;
            this.labelDataProgress.InterSpacing = 4;
            this.labelDataProgress.Location = new System.Drawing.Point(127, 32);
            this.labelDataProgress.Name = "labelDataProgress";
            this.labelDataProgress.NoDataText = null;
            this.labelDataProgress.Size = new System.Drawing.Size(151, 23);
            this.labelDataProgress.TabIndex = 2;
            this.labelDataProgress.TabSpacingData = 0;
            this.labelDataProgress.Text = "Progress: {N2}%";
            // 
            // labelDataGov
            // 
            this.labelDataGov.BorderColor = System.Drawing.Color.Orange;
            this.labelDataGov.BorderWidth = 1;
            this.labelDataGov.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataGov.Data = null;
            this.labelDataGov.DataFont = null;
            this.labelDataGov.InterSpacing = 4;
            this.labelDataGov.Location = new System.Drawing.Point(400, 32);
            this.labelDataGov.Name = "labelDataGov";
            this.labelDataGov.NoDataText = null;
            this.labelDataGov.Size = new System.Drawing.Size(329, 23);
            this.labelDataGov.TabIndex = 2;
            this.labelDataGov.TabSpacingData = 0;
            this.labelDataGov.Text = "Gov: {} Allegiance :{} Economy: {}";
            // 
            // labelDataFaction
            // 
            this.labelDataFaction.BorderColor = System.Drawing.Color.Orange;
            this.labelDataFaction.BorderWidth = 1;
            this.labelDataFaction.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataFaction.Data = null;
            this.labelDataFaction.DataFont = null;
            this.labelDataFaction.InterSpacing = 4;
            this.labelDataFaction.Location = new System.Drawing.Point(400, 3);
            this.labelDataFaction.Name = "labelDataFaction";
            this.labelDataFaction.NoDataText = null;
            this.labelDataFaction.Size = new System.Drawing.Size(319, 23);
            this.labelDataFaction.TabIndex = 2;
            this.labelDataFaction.TabSpacingData = 0;
            this.labelDataFaction.Text = "Faction: {} State: {} ";
            // 
            // extLabelFailed
            // 
            this.extLabelFailed.AutoSize = true;
            this.extLabelFailed.Location = new System.Drawing.Point(339, 32);
            this.extLabelFailed.Name = "extLabelFailed";
            this.extLabelFailed.Size = new System.Drawing.Size(35, 13);
            this.extLabelFailed.TabIndex = 0;
            this.extLabelFailed.Text = "Failed";
            // 
            // extLabelStationName
            // 
            this.extLabelStationName.AutoSize = true;
            this.extLabelStationName.Location = new System.Drawing.Point(4, 4);
            this.extLabelStationName.Name = "extLabelStationName";
            this.extLabelStationName.Size = new System.Drawing.Size(74, 13);
            this.extLabelStationName.TabIndex = 0;
            this.extLabelStationName.Text = "<code Name>";
            // 
            // dataGridViewContributions
            // 
            this.dataGridViewContributions.AllowUserToAddRows = false;
            this.dataGridViewContributions.AllowUserToDeleteRows = false;
            this.dataGridViewContributions.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewContributions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewContributions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colContributionsTime,
            this.colContributionsName,
            this.colContributionsAmount});
            this.dataGridViewContributions.Dock = System.Windows.Forms.DockStyle.Top;
            this.dataGridViewContributions.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewContributions.Name = "dataGridViewContributions";
            this.dataGridViewContributions.RowHeadersVisible = false;
            this.dataGridViewContributions.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewContributions.Size = new System.Drawing.Size(1093, 227);
            this.dataGridViewContributions.TabIndex = 0;
            // 
            // colContributionsTime
            // 
            this.colContributionsTime.HeaderText = "Date";
            this.colContributionsTime.Name = "colContributionsTime";
            this.colContributionsTime.ReadOnly = true;
            // 
            // colContributionsName
            // 
            this.colContributionsName.FillWeight = 200F;
            this.colContributionsName.HeaderText = "Name";
            this.colContributionsName.Name = "colContributionsName";
            this.colContributionsName.ReadOnly = true;
            // 
            // colContributionsAmount
            // 
            this.colContributionsAmount.HeaderText = "Amount";
            this.colContributionsAmount.Name = "colContributionsAmount";
            this.colContributionsAmount.ReadOnly = true;
            // 
            // extPanelDataGridViewScrollContributions
            // 
            this.extPanelDataGridViewScrollContributions.Controls.Add(this.extScrollBarContributions);
            this.extPanelDataGridViewScrollContributions.Controls.Add(this.dataGridViewContributions);
            this.extPanelDataGridViewScrollContributions.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelDataGridViewScrollContributions.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollContributions.Location = new System.Drawing.Point(0, 287);
            this.extPanelDataGridViewScrollContributions.Name = "extPanelDataGridViewScrollContributions";
            this.extPanelDataGridViewScrollContributions.ScrollBarWidth = 24;
            this.extPanelDataGridViewScrollContributions.Size = new System.Drawing.Size(1117, 227);
            this.extPanelDataGridViewScrollContributions.TabIndex = 5;
            this.extPanelDataGridViewScrollContributions.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarContributions
            // 
            this.extScrollBarContributions.AlwaysHideScrollBar = false;
            this.extScrollBarContributions.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarContributions.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarContributions.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.extScrollBarContributions.ArrowDownDrawAngle = 270F;
            this.extScrollBarContributions.ArrowUpDrawAngle = 90F;
            this.extScrollBarContributions.BorderColor = System.Drawing.Color.White;
            this.extScrollBarContributions.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarContributions.HideScrollBar = false;
            this.extScrollBarContributions.LargeChange = 0;
            this.extScrollBarContributions.Location = new System.Drawing.Point(1093, 0);
            this.extScrollBarContributions.Maximum = -1;
            this.extScrollBarContributions.Minimum = 0;
            this.extScrollBarContributions.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarContributions.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.extScrollBarContributions.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarContributions.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.extScrollBarContributions.Name = "extScrollBarContributions";
            this.extScrollBarContributions.Size = new System.Drawing.Size(24, 227);
            this.extScrollBarContributions.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarContributions.SliderColor2 = System.Drawing.Color.DarkGray;
            this.extScrollBarContributions.SliderDrawAngle = 90F;
            this.extScrollBarContributions.SmallChange = 1;
            this.extScrollBarContributions.TabIndex = 1;
            this.extScrollBarContributions.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarContributions.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarContributions.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.extScrollBarContributions.ThumbDrawAngle = 0F;
            this.extScrollBarContributions.Value = -1;
            this.extScrollBarContributions.ValueLimited = -1;
            // 
            // extPanelGradientFillBot
            // 
            this.extPanelGradientFillBot.ChildrenThemed = true;
            this.extPanelGradientFillBot.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelGradientFillBot.FlowDirection = null;
            this.extPanelGradientFillBot.GradientDirection = 0F;
            this.extPanelGradientFillBot.Location = new System.Drawing.Point(0, 514);
            this.extPanelGradientFillBot.Name = "extPanelGradientFillBot";
            this.extPanelGradientFillBot.PaintTransparentColor = System.Drawing.Color.Transparent;
            this.extPanelGradientFillBot.Size = new System.Drawing.Size(1117, 15);
            this.extPanelGradientFillBot.TabIndex = 36;
            this.extPanelGradientFillBot.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.extPanelGradientFillBot.ThemeColorSet = -1;
            // 
            // extCheckBoxShowZeros
            // 
            this.extCheckBoxShowZeros.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxShowZeros.ButtonGradientDirection = 90F;
            this.extCheckBoxShowZeros.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxShowZeros.CheckBoxGradientDirection = 225F;
            this.extCheckBoxShowZeros.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxShowZeros.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowZeros.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxShowZeros.Checked = true;
            this.extCheckBoxShowZeros.CheckState = System.Windows.Forms.CheckState.Checked;
            this.extCheckBoxShowZeros.DisabledScaling = 0.5F;
            this.extCheckBoxShowZeros.Image = global::EDDiscovery.Icons.Controls.greenzero;
            this.extCheckBoxShowZeros.ImageIndeterminate = null;
            this.extCheckBoxShowZeros.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxShowZeros.ImageUnchecked = global::EDDiscovery.Icons.Controls.redzero;
            this.extCheckBoxShowZeros.Location = new System.Drawing.Point(73, 28);
            this.extCheckBoxShowZeros.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxShowZeros.MouseOverScaling = 1.3F;
            this.extCheckBoxShowZeros.MouseSelectedScaling = 1.3F;
            this.extCheckBoxShowZeros.Name = "extCheckBoxShowZeros";
            this.extCheckBoxShowZeros.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxShowZeros.TabIndex = 35;
            this.extCheckBoxShowZeros.TickBoxReductionRatio = 0.75F;
            this.extCheckBoxShowZeros.UseVisualStyleBackColor = true;
            // 
            // ColonisationPortDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.extPanelGradientFillBot);
            this.Controls.Add(this.extPanelDataGridViewScrollContributions);
            this.Controls.Add(this.extPanelDataGridViewScrollRL);
            this.Controls.Add(this.extPanelGradientFill1);
            this.Name = "ColonisationPortDisplay";
            this.Size = new System.Drawing.Size(1117, 671);
            this.extPanelDataGridViewScrollRL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRL)).EndInit();
            this.extPanelGradientFill1.ResumeLayout(false);
            this.extPanelGradientFill1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewContributions)).EndInit();
            this.extPanelDataGridViewScrollContributions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtLabel extLabelStationName;
        private ExtendedControls.LabelData labelDataFaction;
        private ExtendedControls.ExtPanelGradientFill extPanelGradientFill1;
        private ExtendedControls.LabelData labelDataGov;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollRL;
        private ExtendedControls.ExtScrollBar extScrollBarRL;
        private System.Windows.Forms.DataGridView dataGridViewRL;
        private ExtendedControls.LabelData labelDataProgress;
        private ExtendedControls.ExtLabel extLabelFailed;
        private System.Windows.Forms.DataGridView dataGridViewContributions;
        private ExtendedControls.ExtCheckBox extCheckBoxShowRL;
        private ExtendedControls.ExtCheckBox extCheckBoxShowContributions;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollContributions;
        private ExtendedControls.ExtScrollBar extScrollBarContributions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContributionsTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContributionsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colContributionsAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRLName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRLRequired;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRLProvided;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRLRemaining;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRLPayment;
        private ExtendedControls.ExtPanelGradientFill extPanelGradientFillBot;
        private ExtendedControls.ExtCheckBox extCheckBoxShowZeros;
    }
}
