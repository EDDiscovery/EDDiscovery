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
    partial class UserControlModules
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
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.comboBoxShips = new ExtendedControls.ExtComboBox();
            this.extButtonShowControl = new ExtendedControls.ExtButton();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.buttonExtCoriolis = new ExtendedControls.ExtButton();
            this.buttonExtEDShipyard = new ExtendedControls.ExtButton();
            this.buttonExtConfigure = new ExtendedControls.ExtButton();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.extButtonLoadLoadout = new ExtendedControls.ExtButton();
            this.extButtonSaveLoadout = new ExtendedControls.ExtButton();
            this.extButtonDeleteLoadout = new ExtendedControls.ExtButton();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewModules = new BaseUtils.DataGridViewColumnControl();
            this.ItemLocalised = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlotCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BluePrint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriorityEnable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.extPanelRollUpStats = new ExtendedControls.ExtPanelRollUp();
            this.labelDataArmour = new ExtendedControls.LabelData();
            this.labelDataShields = new ExtendedControls.LabelData();
            this.labelDataMass = new ExtendedControls.LabelData();
            this.labelDataCost = new ExtendedControls.LabelData();
            this.labelDataFSD = new ExtendedControls.LabelData();
            this.labelDataThrust = new ExtendedControls.LabelData();
            this.labelDataWep = new ExtendedControls.LabelData();
            this.extButtonDrawnResetPips = new ExtendedControls.ExtButtonDrawn();
            this.multiPipControlSys = new ExtendedControls.MultiPipControl();
            this.multiPipControlWep = new ExtendedControls.MultiPipControl();
            this.multiPipControlEng = new ExtendedControls.MultiPipControl();
            this.labelMass = new System.Windows.Forms.Label();
            this.labelCost = new System.Windows.Forms.Label();
            this.labelFSD = new System.Windows.Forms.Label();
            this.labelThrusters = new System.Windows.Forms.Label();
            this.labelWep = new System.Windows.Forms.Label();
            this.labelShields = new System.Windows.Forms.Label();
            this.labelArmour = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.labelShip = new System.Windows.Forms.Label();
            this.labelVehicle = new System.Windows.Forms.Label();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModules)).BeginInit();
            this.extPanelRollUpStats.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // comboBoxShips
            // 
            this.comboBoxShips.BorderColor = System.Drawing.Color.Red;
            this.comboBoxShips.ButtonColorScaling = 0.5F;
            this.comboBoxShips.DataSource = null;
            this.comboBoxShips.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxShips.DisplayMember = "";
            this.comboBoxShips.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxShips.Location = new System.Drawing.Point(36, 1);
            this.comboBoxShips.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.comboBoxShips.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxShips.Name = "comboBoxShips";
            this.comboBoxShips.SelectedIndex = -1;
            this.comboBoxShips.SelectedItem = null;
            this.comboBoxShips.SelectedValue = null;
            this.comboBoxShips.Size = new System.Drawing.Size(188, 21);
            this.comboBoxShips.TabIndex = 0;
            this.comboBoxShips.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxShips, "Select ship to view");
            this.comboBoxShips.ValueMember = "";
            this.comboBoxShips.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
            // 
            // extButtonShowControl
            // 
            this.extButtonShowControl.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonShowControl.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonShowControl.Image = global::EDDiscovery.Icons.Controls.DisplayFilters;
            this.extButtonShowControl.Location = new System.Drawing.Point(240, 1);
            this.extButtonShowControl.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonShowControl.Name = "extButtonShowControl";
            this.extButtonShowControl.Size = new System.Drawing.Size(28, 28);
            this.extButtonShowControl.TabIndex = 33;
            this.toolTip.SetToolTip(this.extButtonShowControl, "Display Settings");
            this.extButtonShowControl.UseVisualStyleBackColor = false;
            this.extButtonShowControl.Click += new System.EventHandler(this.extButtonShowControl_Click);
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
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(272, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extCheckBoxWordWrap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 32;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // buttonExtCoriolis
            // 
            this.buttonExtCoriolis.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtCoriolis.Image = global::EDDiscovery.Icons.Controls.Coriolis;
            this.buttonExtCoriolis.Location = new System.Drawing.Point(308, 1);
            this.buttonExtCoriolis.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonExtCoriolis.Name = "buttonExtCoriolis";
            this.buttonExtCoriolis.Size = new System.Drawing.Size(24, 24);
            this.buttonExtCoriolis.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtCoriolis, "Send to Coriolis");
            this.buttonExtCoriolis.UseVisualStyleBackColor = false;
            this.buttonExtCoriolis.Click += new System.EventHandler(this.buttonExtCoriolis_Click);
            this.buttonExtCoriolis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExtCoriolis_MouseDown);
            // 
            // buttonExtEDShipyard
            // 
            this.buttonExtEDShipyard.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtEDShipyard.Image = global::EDDiscovery.Icons.Controls.EDShipYard;
            this.buttonExtEDShipyard.Location = new System.Drawing.Point(340, 1);
            this.buttonExtEDShipyard.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonExtEDShipyard.Name = "buttonExtEDShipyard";
            this.buttonExtEDShipyard.Size = new System.Drawing.Size(24, 24);
            this.buttonExtEDShipyard.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtEDShipyard, "Send to ED Ship Yard");
            this.buttonExtEDShipyard.UseVisualStyleBackColor = false;
            this.buttonExtEDShipyard.Click += new System.EventHandler(this.buttonExtEDShipyard_Click);
            this.buttonExtEDShipyard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExtEDShipyard_MouseDown);
            // 
            // buttonExtConfigure
            // 
            this.buttonExtConfigure.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtConfigure.Image = global::EDDiscovery.Icons.Controls.Spaceship;
            this.buttonExtConfigure.Location = new System.Drawing.Point(372, 1);
            this.buttonExtConfigure.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonExtConfigure.Name = "buttonExtConfigure";
            this.buttonExtConfigure.Size = new System.Drawing.Size(24, 24);
            this.buttonExtConfigure.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtConfigure, "Configure extra data missing from Elite Journal Output");
            this.buttonExtConfigure.UseVisualStyleBackColor = false;
            this.buttonExtConfigure.Click += new System.EventHandler(this.buttonExtConfigure_Click);
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(404, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 31;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Send data on grid to excel");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // extButtonLoadLoadout
            // 
            this.extButtonLoadLoadout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonLoadLoadout.Image = global::EDDiscovery.Icons.Controls.ImportExcel;
            this.extButtonLoadLoadout.Location = new System.Drawing.Point(440, 1);
            this.extButtonLoadLoadout.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extButtonLoadLoadout.Name = "extButtonLoadLoadout";
            this.extButtonLoadLoadout.Size = new System.Drawing.Size(28, 28);
            this.extButtonLoadLoadout.TabIndex = 31;
            this.toolTip.SetToolTip(this.extButtonLoadLoadout, "Import a ship definition for display");
            this.extButtonLoadLoadout.UseVisualStyleBackColor = true;
            this.extButtonLoadLoadout.Click += new System.EventHandler(this.extButtonLoadLoadout_Click);
            // 
            // extButtonSaveLoadout
            // 
            this.extButtonSaveLoadout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonSaveLoadout.Image = global::EDDiscovery.Icons.Controls.Save;
            this.extButtonSaveLoadout.Location = new System.Drawing.Point(476, 1);
            this.extButtonSaveLoadout.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extButtonSaveLoadout.Name = "extButtonSaveLoadout";
            this.extButtonSaveLoadout.Size = new System.Drawing.Size(28, 28);
            this.extButtonSaveLoadout.TabIndex = 31;
            this.toolTip.SetToolTip(this.extButtonSaveLoadout, "Save this loadout to the EDD loadout folder");
            this.extButtonSaveLoadout.UseVisualStyleBackColor = true;
            this.extButtonSaveLoadout.Click += new System.EventHandler(this.extButtonSaveLoadout_Click);
            // 
            // extButtonDeleteLoadout
            // 
            this.extButtonDeleteLoadout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonDeleteLoadout.Image = global::EDDiscovery.Icons.Controls.Delete;
            this.extButtonDeleteLoadout.Location = new System.Drawing.Point(512, 1);
            this.extButtonDeleteLoadout.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extButtonDeleteLoadout.Name = "extButtonDeleteLoadout";
            this.extButtonDeleteLoadout.Size = new System.Drawing.Size(28, 28);
            this.extButtonDeleteLoadout.TabIndex = 31;
            this.toolTip.SetToolTip(this.extButtonDeleteLoadout, "Delete loadout");
            this.extButtonDeleteLoadout.UseVisualStyleBackColor = true;
            this.extButtonDeleteLoadout.Click += new System.EventHandler(this.extButtonDeleteLoadout_Click);
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewModules);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 226);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(800, 346);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewModules
            // 
            this.dataGridViewModules.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewModules.AllowUserToAddRows = false;
            this.dataGridViewModules.AllowUserToDeleteRows = false;
            this.dataGridViewModules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewModules.AutoSortByColumnName = false;
            this.dataGridViewModules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewModules.ColumnReorder = true;
            this.dataGridViewModules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemLocalised,
            this.ItemCol,
            this.SlotCol,
            this.ItemInfo,
            this.Mass,
            this.BluePrint,
            this.Value,
            this.PriorityEnable});
            this.dataGridViewModules.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewModules.Name = "dataGridViewModules";
            this.dataGridViewModules.PerColumnWordWrapControl = true;
            this.dataGridViewModules.RowHeaderMenuStrip = null;
            this.dataGridViewModules.RowHeadersVisible = false;
            this.dataGridViewModules.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewModules.SingleRowSelect = true;
            this.dataGridViewModules.Size = new System.Drawing.Size(784, 346);
            this.dataGridViewModules.TabIndex = 1;
            this.dataGridViewModules.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewModules_CellDoubleClick);
            this.dataGridViewModules.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewModules_SortCompare);
            // 
            // ItemLocalised
            // 
            this.ItemLocalised.HeaderText = "Type";
            this.ItemLocalised.MinimumWidth = 100;
            this.ItemLocalised.Name = "ItemLocalised";
            this.ItemLocalised.ReadOnly = true;
            // 
            // ItemCol
            // 
            this.ItemCol.FillWeight = 120F;
            this.ItemCol.HeaderText = "Item";
            this.ItemCol.MinimumWidth = 100;
            this.ItemCol.Name = "ItemCol";
            this.ItemCol.ReadOnly = true;
            // 
            // SlotCol
            // 
            this.SlotCol.HeaderText = "Slot";
            this.SlotCol.MinimumWidth = 50;
            this.SlotCol.Name = "SlotCol";
            this.SlotCol.ReadOnly = true;
            // 
            // ItemInfo
            // 
            this.ItemInfo.FillWeight = 50F;
            this.ItemInfo.HeaderText = "Info";
            this.ItemInfo.MinimumWidth = 20;
            this.ItemInfo.Name = "ItemInfo";
            this.ItemInfo.ReadOnly = true;
            // 
            // Mass
            // 
            this.Mass.FillWeight = 50F;
            this.Mass.HeaderText = "Mass";
            this.Mass.MinimumWidth = 20;
            this.Mass.Name = "Mass";
            this.Mass.ReadOnly = true;
            // 
            // BluePrint
            // 
            this.BluePrint.FillWeight = 50F;
            this.BluePrint.HeaderText = "BluePrint";
            this.BluePrint.MinimumWidth = 20;
            this.BluePrint.Name = "BluePrint";
            this.BluePrint.ReadOnly = true;
            // 
            // Value
            // 
            this.Value.FillWeight = 50F;
            this.Value.HeaderText = "Value";
            this.Value.MinimumWidth = 20;
            this.Value.Name = "Value";
            this.Value.ReadOnly = true;
            // 
            // PriorityEnable
            // 
            this.PriorityEnable.FillWeight = 50F;
            this.PriorityEnable.HeaderText = "P/E";
            this.PriorityEnable.MinimumWidth = 20;
            this.PriorityEnable.Name = "PriorityEnable";
            this.PriorityEnable.ReadOnly = true;
            // 
            // vScrollBarCustomMC
            // 
            this.vScrollBarCustomMC.AlwaysHideScrollBar = false;
            this.vScrollBarCustomMC.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustomMC.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustomMC.ArrowColorScaling = 0.5F;
            this.vScrollBarCustomMC.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustomMC.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustomMC.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustomMC.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustomMC.HideScrollBar = false;
            this.vScrollBarCustomMC.LargeChange = 0;
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(784, 0);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(16, 346);
            this.vScrollBarCustomMC.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomMC.SmallChange = 1;
            this.vScrollBarCustomMC.TabIndex = 0;
            this.vScrollBarCustomMC.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustomMC.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomMC.ThumbColorScaling = 0.5F;
            this.vScrollBarCustomMC.ThumbDrawAngle = 0F;
            this.vScrollBarCustomMC.Value = -1;
            this.vScrollBarCustomMC.ValueLimited = -1;
            // 
            // extPanelRollUpStats
            // 
            this.extPanelRollUpStats.AutoHeight = false;
            this.extPanelRollUpStats.AutoHeightWidthDisable = false;
            this.extPanelRollUpStats.AutoWidth = false;
            this.extPanelRollUpStats.Controls.Add(this.labelDataArmour);
            this.extPanelRollUpStats.Controls.Add(this.labelDataShields);
            this.extPanelRollUpStats.Controls.Add(this.labelDataMass);
            this.extPanelRollUpStats.Controls.Add(this.labelDataCost);
            this.extPanelRollUpStats.Controls.Add(this.labelDataFSD);
            this.extPanelRollUpStats.Controls.Add(this.labelDataThrust);
            this.extPanelRollUpStats.Controls.Add(this.labelDataWep);
            this.extPanelRollUpStats.Controls.Add(this.extButtonDrawnResetPips);
            this.extPanelRollUpStats.Controls.Add(this.multiPipControlSys);
            this.extPanelRollUpStats.Controls.Add(this.multiPipControlWep);
            this.extPanelRollUpStats.Controls.Add(this.multiPipControlEng);
            this.extPanelRollUpStats.Controls.Add(this.labelMass);
            this.extPanelRollUpStats.Controls.Add(this.labelCost);
            this.extPanelRollUpStats.Controls.Add(this.labelFSD);
            this.extPanelRollUpStats.Controls.Add(this.labelThrusters);
            this.extPanelRollUpStats.Controls.Add(this.labelWep);
            this.extPanelRollUpStats.Controls.Add(this.labelShields);
            this.extPanelRollUpStats.Controls.Add(this.labelArmour);
            this.extPanelRollUpStats.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelRollUpStats.HiddenMarkerWidth = 0;
            this.extPanelRollUpStats.Location = new System.Drawing.Point(0, 30);
            this.extPanelRollUpStats.Name = "extPanelRollUpStats";
            this.extPanelRollUpStats.PinState = true;
            this.extPanelRollUpStats.RolledUpHeight = 5;
            this.extPanelRollUpStats.RollUpAnimationTime = 500;
            this.extPanelRollUpStats.RollUpDelay = 1000;
            this.extPanelRollUpStats.SecondHiddenMarkerWidth = 0;
            this.extPanelRollUpStats.ShowHiddenMarker = true;
            this.extPanelRollUpStats.Size = new System.Drawing.Size(800, 196);
            this.extPanelRollUpStats.TabIndex = 2;
            this.extPanelRollUpStats.UnrollHoverDelay = 1000;
            // 
            // labelDataArmour
            // 
            this.labelDataArmour.BorderColor = System.Drawing.Color.Orange;
            this.labelDataArmour.BorderWidth = 1;
            this.labelDataArmour.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataArmour.Data = null;
            this.labelDataArmour.DataFont = null;
            this.labelDataArmour.InterSpacing = 4;
            this.labelDataArmour.Location = new System.Drawing.Point(68, 38);
            this.labelDataArmour.Name = "labelDataArmour";
            this.labelDataArmour.NoDataText = "-";
            this.labelDataArmour.Size = new System.Drawing.Size(1200, 19);
            this.labelDataArmour.TabIndex = 6;
            this.labelDataArmour.TabSpacingData = 120;
            this.labelDataArmour.Text = "Raw {0.#} Kin {0.#|% =|0.#} Thm {0.#|% =|0.#} Exp {0.#|% =|0.#} Cau {0.#|% =|0.#}" +
    "";
            // 
            // labelDataShields
            // 
            this.labelDataShields.BorderColor = System.Drawing.Color.Orange;
            this.labelDataShields.BorderWidth = 1;
            this.labelDataShields.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataShields.Data = null;
            this.labelDataShields.DataFont = null;
            this.labelDataShields.InterSpacing = 4;
            this.labelDataShields.Location = new System.Drawing.Point(68, 58);
            this.labelDataShields.Name = "labelDataShields";
            this.labelDataShields.NoDataText = "-";
            this.labelDataShields.Size = new System.Drawing.Size(1200, 19);
            this.labelDataShields.TabIndex = 6;
            this.labelDataShields.TabSpacingData = 120;
            this.labelDataShields.Text = "Raw {0.#} Sys {0.#|% =|0.#} Kin {0.#|% =|0.#} Thm {0.#|% =|0.#} Exp {0.#|% =|0.#}" +
    "";
            // 
            // labelDataMass
            // 
            this.labelDataMass.BorderColor = System.Drawing.Color.Orange;
            this.labelDataMass.BorderWidth = 1;
            this.labelDataMass.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataMass.Data = null;
            this.labelDataMass.DataFont = null;
            this.labelDataMass.InterSpacing = 4;
            this.labelDataMass.Location = new System.Drawing.Point(68, 158);
            this.labelDataMass.Name = "labelDataMass";
            this.labelDataMass.NoDataText = "-";
            this.labelDataMass.Size = new System.Drawing.Size(1200, 19);
            this.labelDataMass.TabIndex = 6;
            this.labelDataMass.TabSpacingData = 120;
            this.labelDataMass.Text = "Mass {0.##|t} Hull {0.##|t} Modules {0.##|t} Unladen {0.##|t} Cargo {0|/|0|t} War" +
    "ning at {0.#|%}";
            // 
            // labelDataCost
            // 
            this.labelDataCost.BorderColor = System.Drawing.Color.Orange;
            this.labelDataCost.BorderWidth = 1;
            this.labelDataCost.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataCost.Data = null;
            this.labelDataCost.DataFont = null;
            this.labelDataCost.InterSpacing = 4;
            this.labelDataCost.Location = new System.Drawing.Point(68, 138);
            this.labelDataCost.Name = "labelDataCost";
            this.labelDataCost.NoDataText = "-";
            this.labelDataCost.Size = new System.Drawing.Size(1200, 19);
            this.labelDataCost.TabIndex = 6;
            this.labelDataCost.TabSpacingData = 120;
            this.labelDataCost.Text = "Cost Hull {N0|cr} Modules {N0|cr} Total {N0|cr} Rebuy {N0|cr}";
            // 
            // labelDataFSD
            // 
            this.labelDataFSD.BorderColor = System.Drawing.Color.Orange;
            this.labelDataFSD.BorderWidth = 1;
            this.labelDataFSD.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataFSD.Data = null;
            this.labelDataFSD.DataFont = null;
            this.labelDataFSD.InterSpacing = 4;
            this.labelDataFSD.Location = new System.Drawing.Point(68, 118);
            this.labelDataFSD.Name = "labelDataFSD";
            this.labelDataFSD.NoDataText = "-";
            this.labelDataFSD.Size = new System.Drawing.Size(1200, 19);
            this.labelDataFSD.TabIndex = 6;
            this.labelDataFSD.TabSpacingData = 120;
            this.labelDataFSD.Text = "Cur {0.##|ly} Range {0.##|ly} Laden {0.##|ly} Unladen {0.##|ly} Max {0.##|ly} Max" +
    "Fuel {0.##|t} Current {0.#|/|0.#|t} Reserve {0.#|/|0.#|t} In Transit to {} Store" +
    "d at {}";
            // 
            // labelDataThrust
            // 
            this.labelDataThrust.BorderColor = System.Drawing.Color.Orange;
            this.labelDataThrust.BorderWidth = 1;
            this.labelDataThrust.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataThrust.Data = null;
            this.labelDataThrust.DataFont = null;
            this.labelDataThrust.InterSpacing = 4;
            this.labelDataThrust.Location = new System.Drawing.Point(68, 98);
            this.labelDataThrust.Name = "labelDataThrust";
            this.labelDataThrust.NoDataText = "-";
            this.labelDataThrust.Size = new System.Drawing.Size(1200, 19);
            this.labelDataThrust.TabIndex = 6;
            this.labelDataThrust.TabSpacingData = 120;
            this.labelDataThrust.Text = "Cur Spd {0.#} Bst {0.#} Laden {0.#|/|0.#} Unladen {0.#|/|0.#} Max {0.#|/|0.#} Boo" +
    "st Cur {0.#|s} Max {0.#|s}";
            // 
            // labelDataWep
            // 
            this.labelDataWep.BorderColor = System.Drawing.Color.Orange;
            this.labelDataWep.BorderWidth = 1;
            this.labelDataWep.BoxStyle = ExtendedControls.LabelData.DataBoxStyle.AllAround;
            this.labelDataWep.Data = null;
            this.labelDataWep.DataFont = null;
            this.labelDataWep.InterSpacing = 4;
            this.labelDataWep.Location = new System.Drawing.Point(68, 78);
            this.labelDataWep.Name = "labelDataWep";
            this.labelDataWep.NoDataText = "-";
            this.labelDataWep.Size = new System.Drawing.Size(1200, 19);
            this.labelDataWep.TabIndex = 6;
            this.labelDataWep.TabSpacingData = 80;
            this.labelDataWep.Text = "Raw {0.#} Abs {0.#|%} Kin {0.#|%} Thm {0.#|%} Exp {0.#|%} AX {0.#|%} Dur {0.#|s} " +
    "DurMax {0.#|s} Ammo {0.#|s} Cur {0.#|%} Max {0.#|%}";
            // 
            // extButtonDrawnResetPips
            // 
            this.extButtonDrawnResetPips.AutoEllipsis = false;
            this.extButtonDrawnResetPips.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnResetPips.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnResetPips.BorderWidth = 2;
            this.extButtonDrawnResetPips.Image = null;
            this.extButtonDrawnResetPips.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.TextBorder;
            this.extButtonDrawnResetPips.Location = new System.Drawing.Point(177, 10);
            this.extButtonDrawnResetPips.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnResetPips.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnResetPips.MouseSelectedColorEnable = true;
            this.extButtonDrawnResetPips.Name = "extButtonDrawnResetPips";
            this.extButtonDrawnResetPips.Selectable = true;
            this.extButtonDrawnResetPips.Size = new System.Drawing.Size(47, 22);
            this.extButtonDrawnResetPips.TabIndex = 5;
            this.extButtonDrawnResetPips.Text = "<code>";
            this.extButtonDrawnResetPips.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnResetPips.UseMnemonic = true;
            this.extButtonDrawnResetPips.Click += new System.EventHandler(this.extButtonDrawnResetPips_Click);
            // 
            // multiPipControlSys
            // 
            this.multiPipControlSys.BorderColor = System.Drawing.Color.Orange;
            this.multiPipControlSys.BorderWidth = 2;
            this.multiPipControlSys.HalfPipColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(124)))), ((int)(((byte)(0)))));
            this.multiPipControlSys.InterSpacing = 1;
            this.multiPipControlSys.Location = new System.Drawing.Point(9, 1);
            this.multiPipControlSys.MaxValue = 8;
            this.multiPipControlSys.Name = "multiPipControlSys";
            this.multiPipControlSys.PipColor = System.Drawing.Color.Orange;
            this.multiPipControlSys.PipsPerClick = 2;
            this.multiPipControlSys.PipsTakenPerCLickFromOthers = 1;
            this.multiPipControlSys.Size = new System.Drawing.Size(50, 31);
            this.multiPipControlSys.TabIndex = 3;
            this.multiPipControlSys.Text = "SYS";
            this.multiPipControlSys.Value = 4;
            this.multiPipControlSys.ValueChanged = null;
            // 
            // multiPipControlWep
            // 
            this.multiPipControlWep.BorderColor = System.Drawing.Color.Orange;
            this.multiPipControlWep.BorderWidth = 2;
            this.multiPipControlWep.HalfPipColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(124)))), ((int)(((byte)(0)))));
            this.multiPipControlWep.InterSpacing = 1;
            this.multiPipControlWep.Location = new System.Drawing.Point(121, 1);
            this.multiPipControlWep.MaxValue = 8;
            this.multiPipControlWep.Name = "multiPipControlWep";
            this.multiPipControlWep.PipColor = System.Drawing.Color.Turquoise;
            this.multiPipControlWep.PipsPerClick = 2;
            this.multiPipControlWep.PipsTakenPerCLickFromOthers = 1;
            this.multiPipControlWep.Size = new System.Drawing.Size(50, 31);
            this.multiPipControlWep.TabIndex = 3;
            this.multiPipControlWep.Text = "WEP";
            this.multiPipControlWep.Value = 4;
            this.multiPipControlWep.ValueChanged = null;
            // 
            // multiPipControlEng
            // 
            this.multiPipControlEng.BorderColor = System.Drawing.Color.Orange;
            this.multiPipControlEng.BorderWidth = 2;
            this.multiPipControlEng.HalfPipColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(124)))), ((int)(((byte)(0)))));
            this.multiPipControlEng.InterSpacing = 1;
            this.multiPipControlEng.Location = new System.Drawing.Point(65, 1);
            this.multiPipControlEng.MaxValue = 8;
            this.multiPipControlEng.Name = "multiPipControlEng";
            this.multiPipControlEng.PipColor = System.Drawing.Color.Orange;
            this.multiPipControlEng.PipsPerClick = 2;
            this.multiPipControlEng.PipsTakenPerCLickFromOthers = 1;
            this.multiPipControlEng.Size = new System.Drawing.Size(50, 31);
            this.multiPipControlEng.TabIndex = 3;
            this.multiPipControlEng.Text = "ENG";
            this.multiPipControlEng.Value = 4;
            this.multiPipControlEng.ValueChanged = null;
            // 
            // labelMass
            // 
            this.labelMass.AutoSize = true;
            this.labelMass.Location = new System.Drawing.Point(6, 158);
            this.labelMass.Name = "labelMass";
            this.labelMass.Size = new System.Drawing.Size(35, 13);
            this.labelMass.TabIndex = 0;
            this.labelMass.Text = "Mass:";
            // 
            // labelCost
            // 
            this.labelCost.AutoSize = true;
            this.labelCost.Location = new System.Drawing.Point(6, 138);
            this.labelCost.Name = "labelCost";
            this.labelCost.Size = new System.Drawing.Size(31, 13);
            this.labelCost.TabIndex = 0;
            this.labelCost.Text = "Cost:";
            // 
            // labelFSD
            // 
            this.labelFSD.AutoSize = true;
            this.labelFSD.Location = new System.Drawing.Point(6, 118);
            this.labelFSD.Name = "labelFSD";
            this.labelFSD.Size = new System.Drawing.Size(31, 13);
            this.labelFSD.TabIndex = 0;
            this.labelFSD.Text = "FSD:";
            // 
            // labelThrusters
            // 
            this.labelThrusters.AutoSize = true;
            this.labelThrusters.Location = new System.Drawing.Point(6, 98);
            this.labelThrusters.Name = "labelThrusters";
            this.labelThrusters.Size = new System.Drawing.Size(40, 13);
            this.labelThrusters.TabIndex = 0;
            this.labelThrusters.Text = "Thrust:";
            // 
            // labelWep
            // 
            this.labelWep.AutoSize = true;
            this.labelWep.Location = new System.Drawing.Point(6, 78);
            this.labelWep.Name = "labelWep";
            this.labelWep.Size = new System.Drawing.Size(56, 13);
            this.labelWep.TabIndex = 0;
            this.labelWep.Text = "Weapons:";
            // 
            // labelShields
            // 
            this.labelShields.AutoSize = true;
            this.labelShields.Location = new System.Drawing.Point(6, 58);
            this.labelShields.Name = "labelShields";
            this.labelShields.Size = new System.Drawing.Size(44, 13);
            this.labelShields.TabIndex = 0;
            this.labelShields.Text = "Shields:";
            // 
            // labelArmour
            // 
            this.labelArmour.AutoSize = true;
            this.labelArmour.Location = new System.Drawing.Point(6, 38);
            this.labelArmour.Name = "labelArmour";
            this.labelArmour.Size = new System.Drawing.Size(43, 13);
            this.labelArmour.TabIndex = 0;
            this.labelArmour.Text = "Armour:";
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.labelShip);
            this.panelTop.Controls.Add(this.comboBoxShips);
            this.panelTop.Controls.Add(this.extButtonShowControl);
            this.panelTop.Controls.Add(this.extCheckBoxWordWrap);
            this.panelTop.Controls.Add(this.buttonExtCoriolis);
            this.panelTop.Controls.Add(this.buttonExtEDShipyard);
            this.panelTop.Controls.Add(this.buttonExtConfigure);
            this.panelTop.Controls.Add(this.buttonExtExcel);
            this.panelTop.Controls.Add(this.extButtonLoadLoadout);
            this.panelTop.Controls.Add(this.extButtonSaveLoadout);
            this.panelTop.Controls.Add(this.extButtonDeleteLoadout);
            this.panelTop.Controls.Add(this.labelVehicle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 30);
            this.panelTop.TabIndex = 30;
            // 
            // labelShip
            // 
            this.labelShip.AutoSize = true;
            this.labelShip.Location = new System.Drawing.Point(0, 1);
            this.labelShip.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelShip.Name = "labelShip";
            this.labelShip.Size = new System.Drawing.Size(28, 13);
            this.labelShip.TabIndex = 26;
            this.labelShip.Text = "Ship";
            // 
            // labelVehicle
            // 
            this.labelVehicle.AutoSize = true;
            this.labelVehicle.Location = new System.Drawing.Point(548, 1);
            this.labelVehicle.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.labelVehicle.Name = "labelVehicle";
            this.labelVehicle.Size = new System.Drawing.Size(53, 13);
            this.labelVehicle.TabIndex = 28;
            this.labelVehicle.Text = "Unknown";
            // 
            // UserControlModules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.extPanelRollUpStats);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlModules";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModules)).EndInit();
            this.extPanelRollUpStats.ResumeLayout(false);
            this.extPanelRollUpStats.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private BaseUtils.DataGridViewColumnControl dataGridViewModules;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        internal ExtendedControls.ExtComboBox comboBoxShips;
        private System.Windows.Forms.Label labelShip;
        private System.Windows.Forms.Label labelVehicle;
        private ExtendedControls.ExtButton buttonExtCoriolis;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonExtEDShipyard;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemLocalised;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlotCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mass;
        private System.Windows.Forms.DataGridViewTextBoxColumn BluePrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriorityEnable;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private ExtendedControls.ExtButton extButtonShowControl;
        private ExtendedControls.ExtButton buttonExtConfigure;
        private ExtendedControls.ExtButton extButtonLoadLoadout;
        private System.Windows.Forms.Label labelArmour;
        private ExtendedControls.ExtPanelRollUp extPanelRollUpStats;
        private System.Windows.Forms.Label labelShields;
        private System.Windows.Forms.Label labelThrusters;
        private System.Windows.Forms.Label labelFSD;
        private System.Windows.Forms.Label labelCost;
        private System.Windows.Forms.Label labelMass;
        private ExtendedControls.MultiPipControl multiPipControlEng;
        private ExtendedControls.MultiPipControl multiPipControlSys;
        private ExtendedControls.MultiPipControl multiPipControlWep;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnResetPips;
        private ExtendedControls.ExtButton extButtonSaveLoadout;
        private ExtendedControls.LabelData labelDataWep;
        private System.Windows.Forms.Label labelWep;
        private ExtendedControls.LabelData labelDataShields;
        private ExtendedControls.LabelData labelDataArmour;
        private ExtendedControls.LabelData labelDataCost;
        private ExtendedControls.LabelData labelDataFSD;
        private ExtendedControls.LabelData labelDataThrust;
        private ExtendedControls.LabelData labelDataMass;
        private ExtendedControls.ExtButton extButtonDeleteLoadout;
    }
}
