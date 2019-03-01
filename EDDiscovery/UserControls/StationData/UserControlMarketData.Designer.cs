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
    partial class UserControlMarketData
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
            this.dataGridViewMarketData = new System.Windows.Forms.DataGridView();
            this.CategoryCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SellCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BuyCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CargoCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DemandCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SupplyCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GalAvgCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProfitToCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProfitFromCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.checkBoxAutoSwap = new ExtendedControls.ExtCheckBox();
            this.checkBoxBuyOnly = new ExtendedControls.ExtCheckBox();
            this.labelVs = new System.Windows.Forms.Label();
            this.comboBoxCustomTo = new ExtendedControls.ExtComboBox();
            this.comboBoxCustomFrom = new ExtendedControls.ExtComboBox();
            this.labelLocation = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarketData)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewMarketData);
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
            // dataGridViewMarketData
            // 
            this.dataGridViewMarketData.AllowUserToAddRows = false;
            this.dataGridViewMarketData.AllowUserToDeleteRows = false;
            this.dataGridViewMarketData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewMarketData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMarketData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.CategoryCol,
            this.NameCol,
            this.SellCol,
            this.BuyCol,
            this.CargoCol,
            this.DemandCol,
            this.SupplyCol,
            this.GalAvgCol,
            this.ProfitToCol,
            this.ProfitFromCol});
            this.dataGridViewMarketData.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMarketData.Name = "dataGridViewMarketData";
            this.dataGridViewMarketData.RowHeadersVisible = false;
            this.dataGridViewMarketData.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewMarketData.Size = new System.Drawing.Size(780, 540);
            this.dataGridViewMarketData.TabIndex = 1;
            this.dataGridViewMarketData.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewMarketData_SortCompare);
            // 
            // CategoryCol
            // 
            this.CategoryCol.FillWeight = 50F;
            this.CategoryCol.HeaderText = "Category";
            this.CategoryCol.MinimumWidth = 50;
            this.CategoryCol.Name = "CategoryCol";
            // 
            // NameCol
            // 
            this.NameCol.HeaderText = "Name";
            this.NameCol.MinimumWidth = 100;
            this.NameCol.Name = "NameCol";
            this.NameCol.ReadOnly = true;
            // 
            // SellCol
            // 
            this.SellCol.FillWeight = 30F;
            this.SellCol.HeaderText = "Sell";
            this.SellCol.MinimumWidth = 50;
            this.SellCol.Name = "SellCol";
            this.SellCol.ReadOnly = true;
            // 
            // BuyCol
            // 
            this.BuyCol.FillWeight = 30F;
            this.BuyCol.HeaderText = "Buy";
            this.BuyCol.MinimumWidth = 50;
            this.BuyCol.Name = "BuyCol";
            this.BuyCol.ReadOnly = true;
            // 
            // CargoCol
            // 
            this.CargoCol.FillWeight = 30F;
            this.CargoCol.HeaderText = "Cargo";
            this.CargoCol.MinimumWidth = 50;
            this.CargoCol.Name = "CargoCol";
            this.CargoCol.ReadOnly = true;
            // 
            // DemandCol
            // 
            this.DemandCol.FillWeight = 30F;
            this.DemandCol.HeaderText = "Demand";
            this.DemandCol.MinimumWidth = 50;
            this.DemandCol.Name = "DemandCol";
            this.DemandCol.ReadOnly = true;
            // 
            // SupplyCol
            // 
            this.SupplyCol.FillWeight = 30F;
            this.SupplyCol.HeaderText = "Supply";
            this.SupplyCol.MinimumWidth = 50;
            this.SupplyCol.Name = "SupplyCol";
            this.SupplyCol.ReadOnly = true;
            // 
            // GalAvgCol
            // 
            this.GalAvgCol.FillWeight = 30F;
            this.GalAvgCol.HeaderText = "Galactic Avg";
            this.GalAvgCol.MinimumWidth = 50;
            this.GalAvgCol.Name = "GalAvgCol";
            this.GalAvgCol.ReadOnly = true;
            // 
            // ProfitToCol
            // 
            this.ProfitToCol.FillWeight = 30F;
            this.ProfitToCol.HeaderText = "Profit To cr/t";
            this.ProfitToCol.MinimumWidth = 50;
            this.ProfitToCol.Name = "ProfitToCol";
            this.ProfitToCol.ReadOnly = true;
            // 
            // ProfitFromCol
            // 
            this.ProfitFromCol.FillWeight = 30F;
            this.ProfitFromCol.HeaderText = "Profit From cr/t";
            this.ProfitFromCol.MinimumWidth = 50;
            this.ProfitFromCol.Name = "ProfitFromCol";
            this.ProfitFromCol.ReadOnly = true;
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
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(780, 0);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(20, 540);
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
            // panelButtons
            // 
            this.panelButtons.Controls.Add(this.checkBoxAutoSwap);
            this.panelButtons.Controls.Add(this.checkBoxBuyOnly);
            this.panelButtons.Controls.Add(this.labelVs);
            this.panelButtons.Controls.Add(this.comboBoxCustomTo);
            this.panelButtons.Controls.Add(this.comboBoxCustomFrom);
            this.panelButtons.Controls.Add(this.labelLocation);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 32);
            this.panelButtons.TabIndex = 2;
            this.toolTip.SetToolTip(this.panelButtons, "When arriving at a station where you are comparing against (on the right)\r\nswap t" +
        "he comparision to the previous market data.\r\nUseful when doing A->B->A trading");
            // 
            // checkBoxAutoSwap
            // 
            this.checkBoxAutoSwap.AutoSize = true;
            this.checkBoxAutoSwap.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxAutoSwap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxAutoSwap.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxAutoSwap.FontNerfReduction = 0.5F;
            this.checkBoxAutoSwap.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxAutoSwap.Location = new System.Drawing.Point(658, 9);
            this.checkBoxAutoSwap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxAutoSwap.Name = "checkBoxAutoSwap";
            this.checkBoxAutoSwap.Size = new System.Drawing.Size(75, 17);
            this.checkBoxAutoSwap.TabIndex = 29;
            this.checkBoxAutoSwap.Text = "AutoSwap";
            this.checkBoxAutoSwap.TickBoxReductionSize = 10;
            this.checkBoxAutoSwap.UseVisualStyleBackColor = true;
            // 
            // checkBoxBuyOnly
            // 
            this.checkBoxBuyOnly.AutoSize = true;
            this.checkBoxBuyOnly.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxBuyOnly.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxBuyOnly.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxBuyOnly.FontNerfReduction = 0.5F;
            this.checkBoxBuyOnly.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxBuyOnly.Location = new System.Drawing.Point(584, 9);
            this.checkBoxBuyOnly.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxBuyOnly.Name = "checkBoxBuyOnly";
            this.checkBoxBuyOnly.Size = new System.Drawing.Size(68, 17);
            this.checkBoxBuyOnly.TabIndex = 29;
            this.checkBoxBuyOnly.Text = "Buy Only";
            this.checkBoxBuyOnly.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxBuyOnly, "Show items you can buy only");
            this.checkBoxBuyOnly.UseVisualStyleBackColor = true;
            // 
            // labelVs
            // 
            this.labelVs.AutoSize = true;
            this.labelVs.Location = new System.Drawing.Point(355, 13);
            this.labelVs.Name = "labelVs";
            this.labelVs.Size = new System.Drawing.Size(19, 13);
            this.labelVs.TabIndex = 28;
            this.labelVs.Text = "Vs";
            // 
            // comboBoxCustomTo
            // 
            this.comboBoxCustomTo.ArrowWidth = 1;
            this.comboBoxCustomTo.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomTo.ButtonColorScaling = 0.5F;
            this.comboBoxCustomTo.DataSource = null;
            this.comboBoxCustomTo.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomTo.DisplayMember = "";
            this.comboBoxCustomTo.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomTo.DropDownHeight = 150;
            this.comboBoxCustomTo.DropDownWidth = 400;
            this.comboBoxCustomTo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomTo.ItemHeight = 13;
            this.comboBoxCustomTo.Location = new System.Drawing.Point(389, 7);
            this.comboBoxCustomTo.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomTo.Name = "comboBoxCustomTo";
            this.comboBoxCustomTo.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomTo.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomTo.ScrollBarWidth = 16;
            this.comboBoxCustomTo.SelectedIndex = -1;
            this.comboBoxCustomTo.SelectedItem = null;
            this.comboBoxCustomTo.SelectedValue = null;
            this.comboBoxCustomTo.Size = new System.Drawing.Size(176, 21);
            this.comboBoxCustomTo.TabIndex = 27;
            this.comboBoxCustomTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCustomTo, "Click to select a comparision market data set");
            this.comboBoxCustomTo.ValueMember = "";
            this.comboBoxCustomTo.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomTo_SelectedIndexChanged);
            // 
            // comboBoxCustomFrom
            // 
            this.comboBoxCustomFrom.ArrowWidth = 1;
            this.comboBoxCustomFrom.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomFrom.ButtonColorScaling = 0.5F;
            this.comboBoxCustomFrom.DataSource = null;
            this.comboBoxCustomFrom.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomFrom.DisplayMember = "";
            this.comboBoxCustomFrom.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomFrom.DropDownHeight = 150;
            this.comboBoxCustomFrom.DropDownWidth = 400;
            this.comboBoxCustomFrom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomFrom.ItemHeight = 13;
            this.comboBoxCustomFrom.Location = new System.Drawing.Point(167, 7);
            this.comboBoxCustomFrom.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomFrom.Name = "comboBoxCustomFrom";
            this.comboBoxCustomFrom.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomFrom.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomFrom.ScrollBarWidth = 16;
            this.comboBoxCustomFrom.SelectedIndex = -1;
            this.comboBoxCustomFrom.SelectedItem = null;
            this.comboBoxCustomFrom.SelectedValue = null;
            this.comboBoxCustomFrom.Size = new System.Drawing.Size(176, 21);
            this.comboBoxCustomFrom.TabIndex = 27;
            this.comboBoxCustomFrom.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCustomFrom, "Click to select between tracking the cursor or a particular market data set");
            this.comboBoxCustomFrom.ValueMember = "";
            this.comboBoxCustomFrom.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomFrom_SelectedIndexChanged);
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(3, 7);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(47, 13);
            this.labelLocation.TabIndex = 26;
            this.labelLocation.Text = "No Data";
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlMarketData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelButtons);
            this.Name = "UserControlMarketData";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMarketData)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewMarketData;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelLocation;
        private ExtendedControls.ExtComboBox comboBoxCustomTo;
        private ExtendedControls.ExtComboBox comboBoxCustomFrom;
        private System.Windows.Forms.Label labelVs;
        private ExtendedControls.ExtCheckBox checkBoxBuyOnly;
        private System.Windows.Forms.DataGridViewTextBoxColumn CategoryCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SellCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn BuyCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn CargoCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn DemandCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn SupplyCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn GalAvgCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProfitToCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProfitFromCol;
        private ExtendedControls.ExtCheckBox checkBoxAutoSwap;
    }
}
