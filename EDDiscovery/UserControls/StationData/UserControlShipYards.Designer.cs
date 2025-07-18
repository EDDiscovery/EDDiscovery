﻿/*
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
    partial class UserControlShipYards
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
            this.labelYard = new System.Windows.Forms.Label();
            this.labelYardSel = new System.Windows.Forms.Label();
            this.comboBoxYards = new ExtendedControls.ExtComboBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel1 = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewShips = new BaseUtils.DataGridViewColumnControl();
            this.Col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Col3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustom1 = new ExtendedControls.ExtScrollBar();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShips)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
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
            this.comboBoxYards.Location = new System.Drawing.Point(31, 4);
            this.comboBoxYards.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
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
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewShips);
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 26);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 24;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(800, 546);
            this.dataViewScrollerPanel1.TabIndex = 3;
            this.dataViewScrollerPanel1.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewShips
            // 
            this.dataGridViewShips.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewShips.AllowUserToAddRows = false;
            this.dataGridViewShips.AllowUserToDeleteRows = false;
            this.dataGridViewShips.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewShips.AutoSortByColumnName = false;
            this.dataGridViewShips.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewShips.ColumnReorder = true;
            this.dataGridViewShips.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Col1,
            this.Col2,
            this.Col3,
            this.ColPrice});
            this.dataGridViewShips.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewShips.Name = "dataGridViewShips";
            this.dataGridViewShips.PerColumnWordWrapControl = true;
            this.dataGridViewShips.RowHeaderMenuStrip = null;
            this.dataGridViewShips.RowHeadersVisible = false;
            this.dataGridViewShips.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewShips.SingleRowSelect = true;
            this.dataGridViewShips.Size = new System.Drawing.Size(776, 546);
            this.dataGridViewShips.TabIndex = 1;
            this.dataGridViewShips.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewShips_SortCompare);
            // 
            // Col1
            // 
            this.Col1.HeaderText = "";
            this.Col1.MinimumWidth = 50;
            this.Col1.Name = "Col1";
            this.Col1.ReadOnly = true;
            // 
            // Col2
            // 
            this.Col2.FillWeight = 200F;
            this.Col2.HeaderText = "";
            this.Col2.Name = "Col2";
            this.Col2.ReadOnly = true;
            // 
            // Col3
            // 
            this.Col3.HeaderText = "";
            this.Col3.Name = "Col3";
            this.Col3.ReadOnly = true;
            // 
            // ColPrice
            // 
            this.ColPrice.HeaderText = "";
            this.ColPrice.MinimumWidth = 50;
            this.ColPrice.Name = "ColPrice";
            this.ColPrice.ReadOnly = true;
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
            this.vScrollBarCustom1.Location = new System.Drawing.Point(776, 0);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(24, 546);
            this.vScrollBarCustom1.SkinnyStyle = false;
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SliderDrawAngle = 90F;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 0;
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.labelYardSel);
            this.panelTop.Controls.Add(this.comboBoxYards);
            this.panelTop.Controls.Add(this.labelYard);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 26);
            this.panelTop.TabIndex = 2;
            // 
            // UserControlShipYards
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlShipYards";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewShips)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal ExtendedControls.ExtComboBox comboBoxYards;
        private System.Windows.Forms.Label labelYardSel;
        private System.Windows.Forms.Label labelYard;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel1;
        private BaseUtils.DataGridViewColumnControl dataGridViewShips;
        private ExtendedControls.ExtScrollBar vScrollBarCustom1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Col3;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPrice;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
    }
}
