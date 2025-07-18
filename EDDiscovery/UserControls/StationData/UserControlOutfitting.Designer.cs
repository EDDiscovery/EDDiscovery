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
    partial class UserControlOutfitting
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
            this.dataGridViewOutfitting = new BaseUtils.DataGridViewColumnControl();
            this.Col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.labelYard = new System.Windows.Forms.Label();
            this.labelYardSel = new System.Windows.Forms.Label();
            this.comboBoxYards = new ExtendedControls.ExtComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.topPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOutfitting)).BeginInit();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewOutfitting);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 23);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 24;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(800, 549);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewOutfitting
            // 
            this.dataGridViewOutfitting.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewOutfitting.AllowUserToAddRows = false;
            this.dataGridViewOutfitting.AllowUserToDeleteRows = false;
            this.dataGridViewOutfitting.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewOutfitting.AutoSortByColumnName = false;
            this.dataGridViewOutfitting.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOutfitting.ColumnReorder = true;
            this.dataGridViewOutfitting.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col1,
            this.Col2,
            this.Col3,
            this.Col4,
            this.ColPrice});
            this.dataGridViewOutfitting.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewOutfitting.Name = "dataGridViewOutfitting";
            this.dataGridViewOutfitting.PerColumnWordWrapControl = true;
            this.dataGridViewOutfitting.RowHeaderMenuStrip = null;
            this.dataGridViewOutfitting.RowHeadersVisible = false;
            this.dataGridViewOutfitting.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewOutfitting.SingleRowSelect = true;
            this.dataGridViewOutfitting.Size = new System.Drawing.Size(776, 549);
            this.dataGridViewOutfitting.TabIndex = 1;
            this.dataGridViewOutfitting.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            // 
            // Col1
            // 
            this.Col1.HeaderText = "<code>";
            this.Col1.MinimumWidth = 50;
            this.Col1.Name = "Col1";
            this.Col1.ReadOnly = true;
            // 
            // Col2
            // 
            this.Col2.HeaderText = "<code>";
            this.Col2.Name = "Col2";
            this.Col2.ReadOnly = true;
            // 
            // Col3
            // 
            this.Col3.HeaderText = "<code>";
            this.Col3.Name = "Col3";
            this.Col3.ReadOnly = true;
            // 
            // Col4
            // 
            this.Col4.HeaderText = "<code>";
            this.Col4.Name = "Col4";
            this.Col4.ReadOnly = true;
            // 
            // ColPrice
            // 
            this.ColPrice.HeaderText = "<code>";
            this.ColPrice.Name = "ColPrice";
            this.ColPrice.ReadOnly = true;
            // 
            // vScrollBarCustomMC
            // 
            this.vScrollBarCustomMC.AlwaysHideScrollBar = false;
            this.vScrollBarCustomMC.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustomMC.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustomMC.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.vScrollBarCustomMC.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustomMC.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustomMC.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustomMC.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustomMC.HideScrollBar = false;
            this.vScrollBarCustomMC.LargeChange = 0;
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(776, 0);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(24, 549);
            this.vScrollBarCustomMC.SkinnyStyle = false;
            this.vScrollBarCustomMC.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomMC.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomMC.SliderDrawAngle = 90F;
            this.vScrollBarCustomMC.SmallChange = 1;
            this.vScrollBarCustomMC.TabIndex = 0;
            this.vScrollBarCustomMC.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustomMC.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomMC.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomMC.ThumbDrawAngle = 0F;
            this.vScrollBarCustomMC.Value = -1;
            this.vScrollBarCustomMC.ValueLimited = -1;
            // 
            // labelYard
            // 
            this.labelYard.AutoSize = true;
            this.labelYard.Location = new System.Drawing.Point(225, 4);
            this.labelYard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.labelYard.Name = "labelYard";
            this.labelYard.Size = new System.Drawing.Size(53, 13);
            this.labelYard.TabIndex = 28;
            this.labelYard.Text = "Unknown";
            // 
            // labelYardSel
            // 
            this.labelYardSel.AutoSize = true;
            this.labelYardSel.Location = new System.Drawing.Point(3, 4);
            this.labelYardSel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.labelYardSel.Name = "labelYardSel";
            this.labelYardSel.Size = new System.Drawing.Size(22, 13);
            this.labelYardSel.TabIndex = 26;
            this.labelYardSel.Text = "Sel";
            // 
            // comboBoxYards
            // 
            this.comboBoxYards.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxYards.BorderColor = System.Drawing.Color.Red;
            this.comboBoxYards.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxYards.DataSource = null;
            this.comboBoxYards.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxYards.DisabledScaling = 0.5F;
            this.comboBoxYards.DisplayMember = "";
            this.comboBoxYards.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxYards.GradientDirection = 90F;
            this.comboBoxYards.Location = new System.Drawing.Point(31, 1);
            this.comboBoxYards.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.comboBoxYards.MouseOverScalingColor = 1.3F;
            this.comboBoxYards.Name = "comboBoxYards";
            this.comboBoxYards.SelectedIndex = -1;
            this.comboBoxYards.SelectedItem = null;
            this.comboBoxYards.SelectedValue = null;
            this.comboBoxYards.Size = new System.Drawing.Size(188, 21);
            this.comboBoxYards.TabIndex = 0;
            this.comboBoxYards.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxYards, "Select ship to view");
            this.comboBoxYards.ValueMember = "";
            this.comboBoxYards.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // topPanel
            // 
            this.topPanel.AutoSize = true;
            this.topPanel.Controls.Add(this.labelYardSel);
            this.topPanel.Controls.Add(this.comboBoxYards);
            this.topPanel.Controls.Add(this.labelYard);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(800, 23);
            this.topPanel.TabIndex = 29;
            // 
            // UserControlOutfitting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "UserControlOutfitting";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOutfitting)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private BaseUtils.DataGridViewColumnControl dataGridViewOutfitting;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        internal ExtendedControls.ExtComboBox comboBoxYards;
        private System.Windows.Forms.Label labelYardSel;
        private System.Windows.Forms.Label labelYard;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPrice;
        private System.Windows.Forms.FlowLayoutPanel topPanel;
    }
}
