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
            this.dataGridView = new BaseUtils.DataGridViewColumnControl();
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
            this.checkBoxAutoSwap = new ExtendedControls.ExtCheckBox();
            this.checkBoxBuyOnly = new ExtendedControls.ExtCheckBox();
            this.labelVs = new System.Windows.Forms.Label();
            this.labelLocation = new System.Windows.Forms.Label();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxHasDemand = new ExtendedControls.ExtCheckBox();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonSelectWhere = new ExtendedControls.ExtButton();
            this.labelComparison = new System.Windows.Forms.Label();
            this.extButtonSelectComparision = new ExtendedControls.ExtButton();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 34);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(1055, 538);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowRowHeaderVisibleSelection = false;
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.AutoSortByColumnName = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.ColumnReorder = true;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
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
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.PerColumnWordWrapControl = true;
            this.dataGridView.RowHeaderMenuStrip = null;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(1039, 538);
            this.dataGridView.TabIndex = 1;
            this.dataGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewMarketData_RowPostPaint);
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewMarketData_SortCompare);
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
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(1039, 0);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(16, 538);
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
            // checkBoxAutoSwap
            // 
            this.checkBoxAutoSwap.AutoSize = true;
            this.checkBoxAutoSwap.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxAutoSwap.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxAutoSwap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxAutoSwap.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxAutoSwap.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxAutoSwap.ImageIndeterminate = null;
            this.checkBoxAutoSwap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxAutoSwap.ImageUnchecked = null;
            this.checkBoxAutoSwap.Location = new System.Drawing.Point(369, 8);
            this.checkBoxAutoSwap.Margin = new System.Windows.Forms.Padding(0, 8, 8, 1);
            this.checkBoxAutoSwap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxAutoSwap.Name = "checkBoxAutoSwap";
            this.checkBoxAutoSwap.Size = new System.Drawing.Size(75, 17);
            this.checkBoxAutoSwap.TabIndex = 29;
            this.checkBoxAutoSwap.Text = "AutoSwap";
            this.checkBoxAutoSwap.TickBoxReductionRatio = 0.75F;
            this.checkBoxAutoSwap.UseVisualStyleBackColor = true;
            // 
            // checkBoxBuyOnly
            // 
            this.checkBoxBuyOnly.AutoSize = true;
            this.checkBoxBuyOnly.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxBuyOnly.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxBuyOnly.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxBuyOnly.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxBuyOnly.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxBuyOnly.ImageIndeterminate = null;
            this.checkBoxBuyOnly.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxBuyOnly.ImageUnchecked = null;
            this.checkBoxBuyOnly.Location = new System.Drawing.Point(197, 8);
            this.checkBoxBuyOnly.Margin = new System.Windows.Forms.Padding(0, 8, 8, 1);
            this.checkBoxBuyOnly.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxBuyOnly.Name = "checkBoxBuyOnly";
            this.checkBoxBuyOnly.Size = new System.Drawing.Size(68, 17);
            this.checkBoxBuyOnly.TabIndex = 29;
            this.checkBoxBuyOnly.Text = "Buy Only";
            this.checkBoxBuyOnly.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxBuyOnly, "Show items you can buy only");
            this.checkBoxBuyOnly.UseVisualStyleBackColor = true;
            // 
            // labelVs
            // 
            this.labelVs.AutoSize = true;
            this.labelVs.Location = new System.Drawing.Point(85, 4);
            this.labelVs.Margin = new System.Windows.Forms.Padding(0, 4, 8, 1);
            this.labelVs.Name = "labelVs";
            this.labelVs.Size = new System.Drawing.Size(19, 13);
            this.labelVs.TabIndex = 28;
            this.labelVs.Text = "Vs";
            // 
            // labelLocation
            // 
            this.labelLocation.AutoSize = true;
            this.labelLocation.Location = new System.Drawing.Point(0, 8);
            this.labelLocation.Margin = new System.Windows.Forms.Padding(0, 8, 8, 1);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(43, 13);
            this.labelLocation.TabIndex = 26;
            this.labelLocation.Text = "<code>";
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // checkBoxHasDemand
            // 
            this.checkBoxHasDemand.AutoSize = true;
            this.checkBoxHasDemand.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxHasDemand.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxHasDemand.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxHasDemand.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxHasDemand.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxHasDemand.ImageIndeterminate = null;
            this.checkBoxHasDemand.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxHasDemand.ImageUnchecked = null;
            this.checkBoxHasDemand.Location = new System.Drawing.Point(273, 8);
            this.checkBoxHasDemand.Margin = new System.Windows.Forms.Padding(0, 8, 8, 1);
            this.checkBoxHasDemand.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxHasDemand.Name = "checkBoxHasDemand";
            this.checkBoxHasDemand.Size = new System.Drawing.Size(88, 17);
            this.checkBoxHasDemand.TabIndex = 29;
            this.checkBoxHasDemand.Text = "Has Demand";
            this.checkBoxHasDemand.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxHasDemand, "Show items with demand");
            this.checkBoxHasDemand.UseVisualStyleBackColor = true;
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.labelLocation);
            this.panelTop.Controls.Add(this.extButtonSelectWhere);
            this.panelTop.Controls.Add(this.labelVs);
            this.panelTop.Controls.Add(this.labelComparison);
            this.panelTop.Controls.Add(this.extButtonSelectComparision);
            this.panelTop.Controls.Add(this.checkBoxBuyOnly);
            this.panelTop.Controls.Add(this.checkBoxHasDemand);
            this.panelTop.Controls.Add(this.checkBoxAutoSwap);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1055, 34);
            this.panelTop.TabIndex = 2;
            // 
            // extButtonSelectWhere
            // 
            this.extButtonSelectWhere.Image = global::EDDiscovery.Icons.Controls.Find;
            this.extButtonSelectWhere.Location = new System.Drawing.Point(54, 3);
            this.extButtonSelectWhere.Name = "extButtonSelectWhere";
            this.extButtonSelectWhere.Size = new System.Drawing.Size(28, 28);
            this.extButtonSelectWhere.TabIndex = 30;
            this.extButtonSelectWhere.UseVisualStyleBackColor = true;
            this.extButtonSelectWhere.Click += new System.EventHandler(this.extButtonSelectWhere_Click);
            // 
            // labelComparison
            // 
            this.labelComparison.AutoSize = true;
            this.labelComparison.Location = new System.Drawing.Point(112, 8);
            this.labelComparison.Margin = new System.Windows.Forms.Padding(0, 8, 8, 1);
            this.labelComparison.Name = "labelComparison";
            this.labelComparison.Size = new System.Drawing.Size(43, 13);
            this.labelComparison.TabIndex = 26;
            this.labelComparison.Text = "<code>";
            // 
            // extButtonSelectComparision
            // 
            this.extButtonSelectComparision.Image = global::EDDiscovery.Icons.Controls.Find;
            this.extButtonSelectComparision.Location = new System.Drawing.Point(166, 3);
            this.extButtonSelectComparision.Name = "extButtonSelectComparision";
            this.extButtonSelectComparision.Size = new System.Drawing.Size(28, 28);
            this.extButtonSelectComparision.TabIndex = 31;
            this.extButtonSelectComparision.UseVisualStyleBackColor = true;
            this.extButtonSelectComparision.Click += new System.EventHandler(this.extButtonSelectComparision_Click);
            // 
            // UserControlMarketData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlMarketData";
            this.Size = new System.Drawing.Size(1055, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private BaseUtils.DataGridViewColumnControl dataGridView;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelLocation;
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
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private ExtendedControls.ExtCheckBox checkBoxHasDemand;
        private System.Windows.Forms.Label labelComparison;
        private ExtendedControls.ExtButton extButtonSelectWhere;
        private ExtendedControls.ExtButton extButtonSelectComparision;
    }
}
