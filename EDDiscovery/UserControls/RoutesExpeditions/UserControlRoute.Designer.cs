namespace EDDiscovery.UserControls
{
    partial class UserControlRoute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlRoute));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.textBox_ToName = new ExtendedControls.ExtTextBox();
            this.textBox_FromName = new ExtendedControls.ExtTextBox();
            this.comboBoxRoutingMetric = new ExtendedControls.ExtComboBox();
            this.buttonExtTravelTo = new ExtendedControls.ExtButton();
            this.buttonExtTravelFrom = new ExtendedControls.ExtButton();
            this.buttonExtTargetTo = new ExtendedControls.ExtButton();
            this.buttonToEDSM = new ExtendedControls.ExtButton();
            this.buttonFromEDSM = new ExtendedControls.ExtButton();
            this.buttonTargetFrom = new ExtendedControls.ExtButton();
            this.cmd3DMap = new ExtendedControls.ExtButton();
            this.textBox_From = new ExtendedControls.ExtTextBoxAutoComplete();
            this.textBox_Range = new ExtendedControls.NumberBoxLong();
            this.textBox_To = new ExtendedControls.ExtTextBoxAutoComplete();
            this.labelLy2 = new System.Windows.Forms.Label();
            this.labelLy1 = new System.Windows.Forms.Label();
            this.textBox_Distance = new ExtendedControls.ExtTextBox();
            this.labelTo = new System.Windows.Forms.Label();
            this.textBox_ToZ = new ExtendedControls.ExtTextBox();
            this.labelMaxJump = new System.Windows.Forms.Label();
            this.textBox_ToY = new ExtendedControls.ExtTextBox();
            this.labelDistance = new System.Windows.Forms.Label();
            this.textBox_ToX = new ExtendedControls.ExtTextBox();
            this.labelMetric = new System.Windows.Forms.Label();
            this.textBox_FromZ = new ExtendedControls.ExtTextBox();
            this.button_Route = new ExtendedControls.ExtButton();
            this.textBox_FromY = new ExtendedControls.ExtTextBox();
            this.labelFrom = new System.Windows.Forms.Label();
            this.textBox_FromX = new ExtendedControls.ExtTextBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showInEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel1 = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.dataGridViewRoute = new System.Windows.Forms.DataGridView();
            this.SystemCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DistanceCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.XCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.YCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WayPointDistCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeviationCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRoute)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonExtExcel);
            this.panel1.Controls.Add(this.textBox_ToName);
            this.panel1.Controls.Add(this.textBox_FromName);
            this.panel1.Controls.Add(this.comboBoxRoutingMetric);
            this.panel1.Controls.Add(this.buttonExtTravelTo);
            this.panel1.Controls.Add(this.buttonExtTravelFrom);
            this.panel1.Controls.Add(this.buttonExtTargetTo);
            this.panel1.Controls.Add(this.buttonToEDSM);
            this.panel1.Controls.Add(this.buttonFromEDSM);
            this.panel1.Controls.Add(this.buttonTargetFrom);
            this.panel1.Controls.Add(this.cmd3DMap);
            this.panel1.Controls.Add(this.textBox_From);
            this.panel1.Controls.Add(this.textBox_Range);
            this.panel1.Controls.Add(this.textBox_To);
            this.panel1.Controls.Add(this.labelLy2);
            this.panel1.Controls.Add(this.labelLy1);
            this.panel1.Controls.Add(this.textBox_Distance);
            this.panel1.Controls.Add(this.labelTo);
            this.panel1.Controls.Add(this.textBox_ToZ);
            this.panel1.Controls.Add(this.labelMaxJump);
            this.panel1.Controls.Add(this.textBox_ToY);
            this.panel1.Controls.Add(this.labelDistance);
            this.panel1.Controls.Add(this.textBox_ToX);
            this.panel1.Controls.Add(this.labelMetric);
            this.panel1.Controls.Add(this.textBox_FromZ);
            this.panel1.Controls.Add(this.button_Route);
            this.panel1.Controls.Add(this.textBox_FromY);
            this.panel1.Controls.Add(this.labelFrom);
            this.panel1.Controls.Add(this.textBox_FromX);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(706, 193);
            this.panel1.TabIndex = 0;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.Route_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(312, 148);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(24, 24);
            this.buttonExtExcel.TabIndex = 59;
            this.buttonExtExcel.Text = "2";
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // textBox_ToName
            // 
            this.textBox_ToName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_ToName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_ToName.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_ToName.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_ToName.BorderColorScaling = 0.5F;
            this.textBox_ToName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ToName.ClearOnFirstChar = false;
            this.textBox_ToName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_ToName.InErrorCondition = false;
            this.textBox_ToName.Location = new System.Drawing.Point(342, 39);
            this.textBox_ToName.Multiline = false;
            this.textBox_ToName.Name = "textBox_ToName";
            this.textBox_ToName.ReadOnly = true;
            this.textBox_ToName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_ToName.SelectionLength = 0;
            this.textBox_ToName.SelectionStart = 0;
            this.textBox_ToName.Size = new System.Drawing.Size(234, 20);
            this.textBox_ToName.TabIndex = 58;
            this.textBox_ToName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_ToName, "Alternate Name");
            this.textBox_ToName.WordWrap = true;
            // 
            // textBox_FromName
            // 
            this.textBox_FromName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_FromName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_FromName.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_FromName.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_FromName.BorderColorScaling = 0.5F;
            this.textBox_FromName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_FromName.ClearOnFirstChar = false;
            this.textBox_FromName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_FromName.InErrorCondition = false;
            this.textBox_FromName.Location = new System.Drawing.Point(57, 39);
            this.textBox_FromName.Multiline = false;
            this.textBox_FromName.Name = "textBox_FromName";
            this.textBox_FromName.ReadOnly = true;
            this.textBox_FromName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_FromName.SelectionLength = 0;
            this.textBox_FromName.SelectionStart = 0;
            this.textBox_FromName.Size = new System.Drawing.Size(234, 20);
            this.textBox_FromName.TabIndex = 57;
            this.textBox_FromName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_FromName, "Alternate name");
            this.textBox_FromName.WordWrap = true;
            // 
            // comboBoxRoutingMetric
            // 
            this.comboBoxRoutingMetric.ArrowWidth = 1;
            this.comboBoxRoutingMetric.BorderColor = System.Drawing.Color.Red;
            this.comboBoxRoutingMetric.ButtonColorScaling = 0.5F;
            this.comboBoxRoutingMetric.DataSource = null;
            this.comboBoxRoutingMetric.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxRoutingMetric.DisplayMember = "";
            this.comboBoxRoutingMetric.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxRoutingMetric.DropDownHeight = 200;
            this.comboBoxRoutingMetric.DropDownWidth = 234;
            this.comboBoxRoutingMetric.Enabled = false;
            this.comboBoxRoutingMetric.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxRoutingMetric.ItemHeight = 13;
            this.comboBoxRoutingMetric.Location = new System.Drawing.Point(57, 120);
            this.comboBoxRoutingMetric.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxRoutingMetric.Name = "comboBoxRoutingMetric";
            this.comboBoxRoutingMetric.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxRoutingMetric.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxRoutingMetric.ScrollBarWidth = 16;
            this.comboBoxRoutingMetric.SelectedIndex = -1;
            this.comboBoxRoutingMetric.SelectedItem = null;
            this.comboBoxRoutingMetric.SelectedValue = null;
            this.comboBoxRoutingMetric.Size = new System.Drawing.Size(234, 21);
            this.comboBoxRoutingMetric.TabIndex = 41;
            this.comboBoxRoutingMetric.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxRoutingMetric, "Pick the metric to use when searching for a route");
            this.comboBoxRoutingMetric.ValueMember = "";
            // 
            // buttonExtTravelTo
            // 
            this.buttonExtTravelTo.Location = new System.Drawing.Point(342, 91);
            this.buttonExtTravelTo.Name = "buttonExtTravelTo";
            this.buttonExtTravelTo.Size = new System.Drawing.Size(72, 23);
            this.buttonExtTravelTo.TabIndex = 56;
            this.buttonExtTravelTo.Text = "History";
            this.toolTip.SetToolTip(this.buttonExtTravelTo, "Copy the entry in the main travel grid to end route entry");
            this.buttonExtTravelTo.UseVisualStyleBackColor = true;
            this.buttonExtTravelTo.Click += new System.EventHandler(this.buttonExtTravelTo_Click);
            // 
            // buttonExtTravelFrom
            // 
            this.buttonExtTravelFrom.Location = new System.Drawing.Point(57, 91);
            this.buttonExtTravelFrom.Name = "buttonExtTravelFrom";
            this.buttonExtTravelFrom.Size = new System.Drawing.Size(72, 23);
            this.buttonExtTravelFrom.TabIndex = 55;
            this.buttonExtTravelFrom.Text = "History";
            this.toolTip.SetToolTip(this.buttonExtTravelFrom, "Copy the entry in the main travel grid to start route entry");
            this.buttonExtTravelFrom.UseVisualStyleBackColor = true;
            this.buttonExtTravelFrom.Click += new System.EventHandler(this.buttonExtTravelFrom_Click);
            // 
            // buttonExtTargetTo
            // 
            this.buttonExtTargetTo.Location = new System.Drawing.Point(420, 91);
            this.buttonExtTargetTo.Name = "buttonExtTargetTo";
            this.buttonExtTargetTo.Size = new System.Drawing.Size(72, 23);
            this.buttonExtTargetTo.TabIndex = 54;
            this.buttonExtTargetTo.Text = "Target";
            this.toolTip.SetToolTip(this.buttonExtTargetTo, "Copy the target system to end route entry");
            this.buttonExtTargetTo.UseVisualStyleBackColor = true;
            this.buttonExtTargetTo.Click += new System.EventHandler(this.buttonTargetTo_Click);
            // 
            // buttonToEDSM
            // 
            this.buttonToEDSM.Location = new System.Drawing.Point(498, 91);
            this.buttonToEDSM.Name = "buttonToEDSM";
            this.buttonToEDSM.Size = new System.Drawing.Size(72, 23);
            this.buttonToEDSM.TabIndex = 53;
            this.buttonToEDSM.Text = "EDSM";
            this.toolTip.SetToolTip(this.buttonToEDSM, "Open this end route system in EDSM");
            this.buttonToEDSM.UseVisualStyleBackColor = true;
            this.buttonToEDSM.Click += new System.EventHandler(this.buttonToEDSM_Click);
            // 
            // buttonFromEDSM
            // 
            this.buttonFromEDSM.Location = new System.Drawing.Point(213, 91);
            this.buttonFromEDSM.Name = "buttonFromEDSM";
            this.buttonFromEDSM.Size = new System.Drawing.Size(72, 23);
            this.buttonFromEDSM.TabIndex = 52;
            this.buttonFromEDSM.Text = "EDSM";
            this.toolTip.SetToolTip(this.buttonFromEDSM, "Open this start route system in EDSM");
            this.buttonFromEDSM.UseVisualStyleBackColor = true;
            this.buttonFromEDSM.Click += new System.EventHandler(this.buttonFromEDSM_Click);
            // 
            // buttonTargetFrom
            // 
            this.buttonTargetFrom.Location = new System.Drawing.Point(135, 91);
            this.buttonTargetFrom.Name = "buttonTargetFrom";
            this.buttonTargetFrom.Size = new System.Drawing.Size(72, 23);
            this.buttonTargetFrom.TabIndex = 51;
            this.buttonTargetFrom.Text = "Target";
            this.toolTip.SetToolTip(this.buttonTargetFrom, "Copy the target system to start route entry");
            this.buttonTargetFrom.UseVisualStyleBackColor = true;
            this.buttonTargetFrom.Click += new System.EventHandler(this.buttonTargetFrom_Click);
            // 
            // cmd3DMap
            // 
            this.cmd3DMap.Location = new System.Drawing.Point(174, 148);
            this.cmd3DMap.Name = "cmd3DMap";
            this.cmd3DMap.Size = new System.Drawing.Size(111, 26);
            this.cmd3DMap.TabIndex = 50;
            this.cmd3DMap.Text = "3D Map";
            this.toolTip.SetToolTip(this.cmd3DMap, "Show route on 3D Map");
            this.cmd3DMap.UseVisualStyleBackColor = true;
            this.cmd3DMap.Click += new System.EventHandler(this.cmd3DMap_Click);
            // 
            // textBox_From
            // 
            this.textBox_From.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_From.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_From.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_From.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_From.BorderColorScaling = 0.5F;
            this.textBox_From.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_From.ClearOnFirstChar = false;
            this.textBox_From.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_From.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.textBox_From.DropDownBorderColor = System.Drawing.Color.Green;
            this.textBox_From.DropDownHeight = 200;
            this.textBox_From.DropDownItemHeight = 13;
            this.textBox_From.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.textBox_From.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.textBox_From.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.textBox_From.DropDownWidth = 0;
            this.textBox_From.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBox_From.InErrorCondition = false;
            this.textBox_From.Location = new System.Drawing.Point(57, 13);
            this.textBox_From.Multiline = false;
            this.textBox_From.Name = "textBox_From";
            this.textBox_From.ReadOnly = true;
            this.textBox_From.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_From.SelectionLength = 0;
            this.textBox_From.SelectionStart = 0;
            this.textBox_From.Size = new System.Drawing.Size(234, 20);
            this.textBox_From.TabIndex = 31;
            this.textBox_From.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_From, "Select system to start the route");
            this.textBox_From.WordWrap = true;
            this.textBox_From.TextChanged += new System.EventHandler(this.textBox_From_TextChanged);
            this.textBox_From.Enter += new System.EventHandler(this.textBox_From_Enter);
            // 
            // textBox_Range
            // 
            this.textBox_Range.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBox_Range.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBox_Range.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_Range.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_Range.BorderColorScaling = 0.5F;
            this.textBox_Range.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Range.ClearOnFirstChar = false;
            this.textBox_Range.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_Range.DelayBeforeNotification = 0;
            this.textBox_Range.Format = "D";
            this.textBox_Range.InErrorCondition = false;
            this.textBox_Range.Location = new System.Drawing.Point(498, 121);
            this.textBox_Range.Maximum = ((long)(999));
            this.textBox_Range.Minimum = ((long)(1));
            this.textBox_Range.Multiline = false;
            this.textBox_Range.Name = "textBox_Range";
            this.textBox_Range.ReadOnly = false;
            this.textBox_Range.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_Range.SelectionLength = 0;
            this.textBox_Range.SelectionStart = 0;
            this.textBox_Range.Size = new System.Drawing.Size(57, 20);
            this.textBox_Range.TabIndex = 33;
            this.textBox_Range.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_Range, "Give your jump range, or search range for long jumps");
            this.textBox_Range.Value = ((long)(0));
            this.textBox_Range.WordWrap = true;
            // 
            // textBox_To
            // 
            this.textBox_To.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_To.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_To.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_To.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_To.BorderColorScaling = 0.5F;
            this.textBox_To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_To.ClearOnFirstChar = false;
            this.textBox_To.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_To.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.textBox_To.DropDownBorderColor = System.Drawing.Color.Green;
            this.textBox_To.DropDownHeight = 200;
            this.textBox_To.DropDownItemHeight = 13;
            this.textBox_To.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.textBox_To.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.textBox_To.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.textBox_To.DropDownWidth = 0;
            this.textBox_To.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBox_To.InErrorCondition = false;
            this.textBox_To.Location = new System.Drawing.Point(342, 13);
            this.textBox_To.Multiline = false;
            this.textBox_To.Name = "textBox_To";
            this.textBox_To.ReadOnly = true;
            this.textBox_To.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_To.SelectionLength = 0;
            this.textBox_To.SelectionStart = 0;
            this.textBox_To.Size = new System.Drawing.Size(234, 20);
            this.textBox_To.TabIndex = 32;
            this.textBox_To.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_To, "Select the system to end in");
            this.textBox_To.WordWrap = true;
            this.textBox_To.TextChanged += new System.EventHandler(this.textBox_To_TextChanged);
            this.textBox_To.Enter += new System.EventHandler(this.textBox_To_Enter);
            // 
            // labelLy2
            // 
            this.labelLy2.AutoSize = true;
            this.labelLy2.Location = new System.Drawing.Point(563, 150);
            this.labelLy2.Name = "labelLy2";
            this.labelLy2.Size = new System.Drawing.Size(14, 13);
            this.labelLy2.TabIndex = 43;
            this.labelLy2.Text = "ly";
            // 
            // labelLy1
            // 
            this.labelLy1.AutoSize = true;
            this.labelLy1.Location = new System.Drawing.Point(563, 123);
            this.labelLy1.Name = "labelLy1";
            this.labelLy1.Size = new System.Drawing.Size(14, 13);
            this.labelLy1.TabIndex = 44;
            this.labelLy1.Text = "ly";
            // 
            // textBox_Distance
            // 
            this.textBox_Distance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_Distance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_Distance.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_Distance.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_Distance.BorderColorScaling = 0.5F;
            this.textBox_Distance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_Distance.ClearOnFirstChar = false;
            this.textBox_Distance.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_Distance.InErrorCondition = false;
            this.textBox_Distance.Location = new System.Drawing.Point(498, 154);
            this.textBox_Distance.Multiline = false;
            this.textBox_Distance.Name = "textBox_Distance";
            this.textBox_Distance.ReadOnly = true;
            this.textBox_Distance.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_Distance.SelectionLength = 0;
            this.textBox_Distance.SelectionStart = 0;
            this.textBox_Distance.Size = new System.Drawing.Size(57, 20);
            this.textBox_Distance.TabIndex = 40;
            this.textBox_Distance.TabStop = false;
            this.textBox_Distance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_Distance, "Distance between start and end");
            this.textBox_Distance.WordWrap = true;
            this.textBox_Distance.Click += new System.EventHandler(this.textBox_Clicked);
            // 
            // labelTo
            // 
            this.labelTo.AutoSize = true;
            this.labelTo.Location = new System.Drawing.Point(301, 14);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new System.Drawing.Size(20, 13);
            this.labelTo.TabIndex = 49;
            this.labelTo.Text = "To";
            // 
            // textBox_ToZ
            // 
            this.textBox_ToZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_ToZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_ToZ.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_ToZ.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_ToZ.BorderColorScaling = 0.5F;
            this.textBox_ToZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ToZ.ClearOnFirstChar = false;
            this.textBox_ToZ.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_ToZ.InErrorCondition = false;
            this.textBox_ToZ.Location = new System.Drawing.Point(498, 65);
            this.textBox_ToZ.Multiline = false;
            this.textBox_ToZ.Name = "textBox_ToZ";
            this.textBox_ToZ.ReadOnly = true;
            this.textBox_ToZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_ToZ.SelectionLength = 0;
            this.textBox_ToZ.SelectionStart = 0;
            this.textBox_ToZ.Size = new System.Drawing.Size(72, 20);
            this.textBox_ToZ.TabIndex = 39;
            this.textBox_ToZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_ToZ, "Z Co-ord");
            this.textBox_ToZ.WordWrap = true;
            this.textBox_ToZ.TextChanged += new System.EventHandler(this.textBox_ToXYZ_TextChanged);
            this.textBox_ToZ.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_ToZ.Enter += new System.EventHandler(this.textBox_ToXYZ_Enter);
            // 
            // labelMaxJump
            // 
            this.labelMaxJump.AutoSize = true;
            this.labelMaxJump.Location = new System.Drawing.Point(419, 122);
            this.labelMaxJump.Name = "labelMaxJump";
            this.labelMaxJump.Size = new System.Drawing.Size(52, 13);
            this.labelMaxJump.TabIndex = 45;
            this.labelMaxJump.Text = "Max jump";
            // 
            // textBox_ToY
            // 
            this.textBox_ToY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_ToY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_ToY.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_ToY.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_ToY.BorderColorScaling = 0.5F;
            this.textBox_ToY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ToY.ClearOnFirstChar = false;
            this.textBox_ToY.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_ToY.InErrorCondition = false;
            this.textBox_ToY.Location = new System.Drawing.Point(420, 65);
            this.textBox_ToY.Multiline = false;
            this.textBox_ToY.Name = "textBox_ToY";
            this.textBox_ToY.ReadOnly = true;
            this.textBox_ToY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_ToY.SelectionLength = 0;
            this.textBox_ToY.SelectionStart = 0;
            this.textBox_ToY.Size = new System.Drawing.Size(72, 20);
            this.textBox_ToY.TabIndex = 38;
            this.textBox_ToY.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_ToY, "Y (Vertical) Co-ord");
            this.textBox_ToY.WordWrap = true;
            this.textBox_ToY.TextChanged += new System.EventHandler(this.textBox_ToXYZ_TextChanged);
            this.textBox_ToY.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_ToY.Enter += new System.EventHandler(this.textBox_ToXYZ_Enter);
            // 
            // labelDistance
            // 
            this.labelDistance.AutoSize = true;
            this.labelDistance.Location = new System.Drawing.Point(422, 155);
            this.labelDistance.Name = "labelDistance";
            this.labelDistance.Size = new System.Drawing.Size(49, 13);
            this.labelDistance.TabIndex = 46;
            this.labelDistance.Text = "Distance";
            // 
            // textBox_ToX
            // 
            this.textBox_ToX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_ToX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_ToX.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_ToX.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_ToX.BorderColorScaling = 0.5F;
            this.textBox_ToX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_ToX.ClearOnFirstChar = false;
            this.textBox_ToX.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_ToX.InErrorCondition = false;
            this.textBox_ToX.Location = new System.Drawing.Point(342, 65);
            this.textBox_ToX.Multiline = false;
            this.textBox_ToX.Name = "textBox_ToX";
            this.textBox_ToX.ReadOnly = true;
            this.textBox_ToX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_ToX.SelectionLength = 0;
            this.textBox_ToX.SelectionStart = 0;
            this.textBox_ToX.Size = new System.Drawing.Size(72, 20);
            this.textBox_ToX.TabIndex = 37;
            this.textBox_ToX.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_ToX, "X Co-Ord");
            this.textBox_ToX.WordWrap = true;
            this.textBox_ToX.TextChanged += new System.EventHandler(this.textBox_ToXYZ_TextChanged);
            this.textBox_ToX.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_ToX.Enter += new System.EventHandler(this.textBox_ToXYZ_Enter);
            // 
            // labelMetric
            // 
            this.labelMetric.AutoSize = true;
            this.labelMetric.Location = new System.Drawing.Point(3, 122);
            this.labelMetric.Name = "labelMetric";
            this.labelMetric.Size = new System.Drawing.Size(36, 13);
            this.labelMetric.TabIndex = 47;
            this.labelMetric.Text = "Metric";
            // 
            // textBox_FromZ
            // 
            this.textBox_FromZ.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_FromZ.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_FromZ.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_FromZ.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_FromZ.BorderColorScaling = 0.5F;
            this.textBox_FromZ.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_FromZ.ClearOnFirstChar = false;
            this.textBox_FromZ.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_FromZ.InErrorCondition = false;
            this.textBox_FromZ.Location = new System.Drawing.Point(213, 65);
            this.textBox_FromZ.Multiline = false;
            this.textBox_FromZ.Name = "textBox_FromZ";
            this.textBox_FromZ.ReadOnly = true;
            this.textBox_FromZ.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_FromZ.SelectionLength = 0;
            this.textBox_FromZ.SelectionStart = 0;
            this.textBox_FromZ.Size = new System.Drawing.Size(72, 20);
            this.textBox_FromZ.TabIndex = 36;
            this.textBox_FromZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_FromZ, "Z Co-ord");
            this.textBox_FromZ.WordWrap = true;
            this.textBox_FromZ.TextChanged += new System.EventHandler(this.textBox_FromXYZ_TextChanged);
            this.textBox_FromZ.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_FromZ.Enter += new System.EventHandler(this.textBox_FromXYZ_Enter);
            // 
            // button_Route
            // 
            this.button_Route.Enabled = false;
            this.button_Route.Location = new System.Drawing.Point(57, 147);
            this.button_Route.Name = "button_Route";
            this.button_Route.Size = new System.Drawing.Size(111, 27);
            this.button_Route.TabIndex = 42;
            this.button_Route.Text = "Find route";
            this.toolTip.SetToolTip(this.button_Route, "Compute the route");
            this.button_Route.UseVisualStyleBackColor = true;
            this.button_Route.Click += new System.EventHandler(this.button_Route_Click);
            // 
            // textBox_FromY
            // 
            this.textBox_FromY.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_FromY.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_FromY.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_FromY.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_FromY.BorderColorScaling = 0.5F;
            this.textBox_FromY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_FromY.ClearOnFirstChar = false;
            this.textBox_FromY.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_FromY.InErrorCondition = false;
            this.textBox_FromY.Location = new System.Drawing.Point(135, 65);
            this.textBox_FromY.Multiline = false;
            this.textBox_FromY.Name = "textBox_FromY";
            this.textBox_FromY.ReadOnly = true;
            this.textBox_FromY.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_FromY.SelectionLength = 0;
            this.textBox_FromY.SelectionStart = 0;
            this.textBox_FromY.Size = new System.Drawing.Size(72, 20);
            this.textBox_FromY.TabIndex = 35;
            this.textBox_FromY.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_FromY, "Y (Vertical) Co-ord");
            this.textBox_FromY.WordWrap = true;
            this.textBox_FromY.TextChanged += new System.EventHandler(this.textBox_FromXYZ_TextChanged);
            this.textBox_FromY.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_FromY.Enter += new System.EventHandler(this.textBox_FromXYZ_Enter);
            // 
            // labelFrom
            // 
            this.labelFrom.AutoSize = true;
            this.labelFrom.Location = new System.Drawing.Point(3, 14);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new System.Drawing.Size(30, 13);
            this.labelFrom.TabIndex = 48;
            this.labelFrom.Text = "From";
            // 
            // textBox_FromX
            // 
            this.textBox_FromX.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBox_FromX.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBox_FromX.BackErrorColor = System.Drawing.Color.Red;
            this.textBox_FromX.BorderColor = System.Drawing.Color.Transparent;
            this.textBox_FromX.BorderColorScaling = 0.5F;
            this.textBox_FromX.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox_FromX.ClearOnFirstChar = false;
            this.textBox_FromX.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBox_FromX.InErrorCondition = false;
            this.textBox_FromX.Location = new System.Drawing.Point(57, 65);
            this.textBox_FromX.Multiline = false;
            this.textBox_FromX.Name = "textBox_FromX";
            this.textBox_FromX.ReadOnly = true;
            this.textBox_FromX.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBox_FromX.SelectionLength = 0;
            this.textBox_FromX.SelectionStart = 0;
            this.textBox_FromX.Size = new System.Drawing.Size(72, 20);
            this.textBox_FromX.TabIndex = 34;
            this.textBox_FromX.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBox_FromX, "X Co-ord");
            this.textBox_FromX.WordWrap = true;
            this.textBox_FromX.TextChanged += new System.EventHandler(this.textBox_FromXYZ_TextChanged);
            this.textBox_FromX.Click += new System.EventHandler(this.textBox_Clicked);
            this.textBox_FromX.Enter += new System.EventHandler(this.textBox_FromXYZ_Enter);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showInEDSMToolStripMenuItem,
            this.copyToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(151, 48);
            // 
            // showInEDSMToolStripMenuItem
            // 
            this.showInEDSMToolStripMenuItem.Name = "showInEDSMToolStripMenuItem";
            this.showInEDSMToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.showInEDSMToolStripMenuItem.Text = "Show in EDSM";
            this.showInEDSMToolStripMenuItem.Click += new System.EventHandler(this.showInEDSMToolStripMenuItem_Click);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewRoute);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 193);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(706, 221);
            this.dataViewScrollerPanel1.TabIndex = 29;
            this.dataViewScrollerPanel1.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = true;
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(683, 0);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 221);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 4;
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // dataGridViewRoute
            // 
            this.dataGridViewRoute.AllowUserToAddRows = false;
            this.dataGridViewRoute.AllowUserToDeleteRows = false;
            this.dataGridViewRoute.AllowUserToResizeRows = false;
            this.dataGridViewRoute.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewRoute.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewRoute.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SystemCol,
            this.DistanceCol,
            this.XCol,
            this.YCol,
            this.ZCol,
            this.WayPointDistCol,
            this.DeviationCol});
            this.dataGridViewRoute.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridViewRoute.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewRoute.Name = "dataGridViewRoute";
            this.dataGridViewRoute.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewRoute.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewRoute.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewRoute.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewRoute.Size = new System.Drawing.Size(683, 221);
            this.dataGridViewRoute.TabIndex = 1;
            this.dataGridViewRoute.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewRoute_CellClick);
            this.dataGridViewRoute.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewRoute_MouseDown);
            // 
            // SystemCol
            // 
            this.SystemCol.FillWeight = 200F;
            this.SystemCol.HeaderText = "System";
            this.SystemCol.MinimumWidth = 50;
            this.SystemCol.Name = "SystemCol";
            this.SystemCol.ReadOnly = true;
            // 
            // DistanceCol
            // 
            this.DistanceCol.HeaderText = "Distance";
            this.DistanceCol.Name = "DistanceCol";
            this.DistanceCol.ReadOnly = true;
            // 
            // XCol
            // 
            this.XCol.HeaderText = "X";
            this.XCol.Name = "XCol";
            this.XCol.ReadOnly = true;
            // 
            // YCol
            // 
            this.YCol.HeaderText = "Y";
            this.YCol.Name = "YCol";
            this.YCol.ReadOnly = true;
            // 
            // ZCol
            // 
            this.ZCol.HeaderText = "Z";
            this.ZCol.Name = "ZCol";
            this.ZCol.ReadOnly = true;
            // 
            // WayPointDistCol
            // 
            this.WayPointDistCol.HeaderText = "Dist. Waypoint";
            this.WayPointDistCol.Name = "WayPointDistCol";
            this.WayPointDistCol.ReadOnly = true;
            // 
            // DeviationCol
            // 
            this.DeviationCol.HeaderText = "Deviation";
            this.DeviationCol.Name = "DeviationCol";
            this.DeviationCol.ReadOnly = true;
            // 
            // UserControlRoute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "UserControlRoute";
            this.Size = new System.Drawing.Size(706, 414);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewRoute)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ExtendedControls.ExtButton buttonExtExcel;
        internal ExtendedControls.ExtTextBox textBox_ToName;
        internal ExtendedControls.ExtTextBox textBox_FromName;
        private ExtendedControls.ExtComboBox comboBoxRoutingMetric;
        private ExtendedControls.ExtButton buttonExtTravelTo;
        private ExtendedControls.ExtButton buttonExtTravelFrom;
        private ExtendedControls.ExtButton buttonExtTargetTo;
        private ExtendedControls.ExtButton buttonToEDSM;
        private ExtendedControls.ExtButton buttonFromEDSM;
        private ExtendedControls.ExtButton buttonTargetFrom;
        private ExtendedControls.ExtButton cmd3DMap;
        internal ExtendedControls.ExtTextBoxAutoComplete textBox_From;
        internal ExtendedControls.NumberBoxLong textBox_Range;
        internal ExtendedControls.ExtTextBoxAutoComplete textBox_To;
        private System.Windows.Forms.Label labelLy2;
        private System.Windows.Forms.Label labelLy1;
        internal ExtendedControls.ExtTextBox textBox_Distance;
        private System.Windows.Forms.Label labelTo;
        internal ExtendedControls.ExtTextBox textBox_ToZ;
        private System.Windows.Forms.Label labelMaxJump;
        internal ExtendedControls.ExtTextBox textBox_ToY;
        private System.Windows.Forms.Label labelDistance;
        internal ExtendedControls.ExtTextBox textBox_ToX;
        private System.Windows.Forms.Label labelMetric;
        internal ExtendedControls.ExtTextBox textBox_FromZ;
        private ExtendedControls.ExtButton button_Route;
        internal ExtendedControls.ExtTextBox textBox_FromY;
        private System.Windows.Forms.Label labelFrom;
        internal ExtendedControls.ExtTextBox textBox_FromX;
        private System.Windows.Forms.DataGridView dataGridViewRoute;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel1;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SystemCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DistanceCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn XCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn YCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WayPointDistCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeviationCol;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem showInEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
    }
}
