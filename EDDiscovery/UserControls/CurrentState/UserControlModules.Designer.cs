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
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewModules = new System.Windows.Forms.DataGridView();
            this.ItemLocalised = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SlotCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemInfo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Mass = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BluePrint = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PriorityEnable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonExtConfigure = new ExtendedControls.ExtButton();
            this.buttonExtEDShipyard = new ExtendedControls.ExtButton();
            this.buttonExtCoriolis = new ExtendedControls.ExtButton();
            this.labelVehicle = new System.Windows.Forms.Label();
            this.labelShip = new System.Windows.Forms.Label();
            this.comboBoxShips = new ExtendedControls.ExtComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModules)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewModules);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 32);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 20;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(800, 540);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewModules
            // 
            this.dataGridViewModules.AllowUserToAddRows = false;
            this.dataGridViewModules.AllowUserToDeleteRows = false;
            this.dataGridViewModules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewModules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
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
            this.dataGridViewModules.RowHeadersVisible = false;
            this.dataGridViewModules.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewModules.Size = new System.Drawing.Size(780, 540);
            this.dataGridViewModules.TabIndex = 1;
            this.dataGridViewModules.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewModules_CellClick);
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
            this.vScrollBarCustomMC.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustomMC.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustomMC.ArrowColorScaling = 0.5F;
            this.vScrollBarCustomMC.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustomMC.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustomMC.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustomMC.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustomMC.HideScrollBar = false;
            this.vScrollBarCustomMC.LargeChange = 0;
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(780, 21);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(20, 519);
            this.vScrollBarCustomMC.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomMC.SmallChange = 1;
            this.vScrollBarCustomMC.TabIndex = 0;
            this.vScrollBarCustomMC.Text = "vScrollBarCustom1";
            this.vScrollBarCustomMC.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustomMC.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomMC.ThumbColorScaling = 0.5F;
            this.vScrollBarCustomMC.ThumbDrawAngle = 0F;
            this.vScrollBarCustomMC.Value = -1;
            this.vScrollBarCustomMC.ValueLimited = -1;
            // 
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.buttonExtConfigure);
            this.panelButtons.Controls.Add(this.buttonExtEDShipyard);
            this.panelButtons.Controls.Add(this.buttonExtCoriolis);
            this.panelButtons.Controls.Add(this.labelVehicle);
            this.panelButtons.Controls.Add(this.labelShip);
            this.panelButtons.Controls.Add(this.comboBoxShips);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 32);
            this.panelButtons.TabIndex = 2;
            // 
            // buttonExtConfigure
            // 
            this.buttonExtConfigure.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtConfigure.Image = global::EDDiscovery.Icons.Controls.Modules_Configure;
            this.buttonExtConfigure.Location = new System.Drawing.Point(246, 3);
            this.buttonExtConfigure.Name = "buttonExtConfigure";
            this.buttonExtConfigure.Size = new System.Drawing.Size(24, 24);
            this.buttonExtConfigure.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtConfigure, "Configure extra data missing from Elite Journal Output");
            this.buttonExtConfigure.UseVisualStyleBackColor = false;
            this.buttonExtConfigure.Click += new System.EventHandler(this.buttonExtConfigure_Click);
            // 
            // buttonExtEDShipyard
            // 
            this.buttonExtEDShipyard.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtEDShipyard.Image = global::EDDiscovery.Icons.Controls.Modules_EDShipYard;
            this.buttonExtEDShipyard.Location = new System.Drawing.Point(306, 3);
            this.buttonExtEDShipyard.Name = "buttonExtEDShipyard";
            this.buttonExtEDShipyard.Size = new System.Drawing.Size(24, 24);
            this.buttonExtEDShipyard.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtEDShipyard, "Send to ED Ship Yard");
            this.buttonExtEDShipyard.UseVisualStyleBackColor = false;
            this.buttonExtEDShipyard.Click += new System.EventHandler(this.buttonExtEDShipyard_Click);
            this.buttonExtEDShipyard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExtEDShipyard_MouseDown);
            // 
            // buttonExtCoriolis
            // 
            this.buttonExtCoriolis.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtCoriolis.Image = global::EDDiscovery.Icons.Controls.Modules_ShowOnCoriolis;
            this.buttonExtCoriolis.Location = new System.Drawing.Point(276, 3);
            this.buttonExtCoriolis.Name = "buttonExtCoriolis";
            this.buttonExtCoriolis.Size = new System.Drawing.Size(24, 24);
            this.buttonExtCoriolis.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtCoriolis, "Send to Corolis");
            this.buttonExtCoriolis.UseVisualStyleBackColor = false;
            this.buttonExtCoriolis.Click += new System.EventHandler(this.buttonExtCoriolis_Click);
            this.buttonExtCoriolis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.buttonExtCoriolis_MouseDown);
            // 
            // labelVehicle
            // 
            this.labelVehicle.AutoSize = true;
            this.labelVehicle.Location = new System.Drawing.Point(333, 9);
            this.labelVehicle.Name = "labelVehicle";
            this.labelVehicle.Size = new System.Drawing.Size(53, 13);
            this.labelVehicle.TabIndex = 28;
            this.labelVehicle.Text = "Unknown";
            // 
            // labelShip
            // 
            this.labelShip.AutoSize = true;
            this.labelShip.Location = new System.Drawing.Point(6, 7);
            this.labelShip.Name = "labelShip";
            this.labelShip.Size = new System.Drawing.Size(28, 13);
            this.labelShip.TabIndex = 26;
            this.labelShip.Text = "Ship";
            // 
            // comboBoxShips
            // 
            this.comboBoxShips.ArrowWidth = 1;
            this.comboBoxShips.BorderColor = System.Drawing.Color.Red;
            this.comboBoxShips.ButtonColorScaling = 0.5F;
            this.comboBoxShips.DataSource = null;
            this.comboBoxShips.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxShips.DisplayMember = "";
            this.comboBoxShips.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxShips.DropDownHeight = 300;
            this.comboBoxShips.DropDownWidth = 400;
            this.comboBoxShips.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxShips.ItemHeight = 13;
            this.comboBoxShips.Location = new System.Drawing.Point(52, 4);
            this.comboBoxShips.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxShips.Name = "comboBoxShips";
            this.comboBoxShips.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxShips.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxShips.ScrollBarWidth = 16;
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
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlModules
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelButtons);
            this.Name = "UserControlModules";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewModules)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewModules;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.Panel panelButtons;
        internal ExtendedControls.ExtComboBox comboBoxShips;
        private System.Windows.Forms.Label labelShip;
        private System.Windows.Forms.Label labelVehicle;
        private ExtendedControls.ExtButton buttonExtCoriolis;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonExtEDShipyard;
        private ExtendedControls.ExtButton buttonExtConfigure;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemLocalised;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SlotCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Mass;
        private System.Windows.Forms.DataGridViewTextBoxColumn BluePrint;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn PriorityEnable;
    }
}
