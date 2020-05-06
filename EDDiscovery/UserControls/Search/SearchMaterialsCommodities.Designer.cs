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
    partial class SearchMaterialsCommodities      
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
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new EDDiscovery.UserControls.Search.DataGridViewStarResults();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCurrentDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelTop = new System.Windows.Forms.Panel();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.buttonExtFind = new ExtendedControls.ExtButton();
            this.comboBoxCustomCMANDOR = new ExtendedControls.ExtComboBox();
            this.comboBoxCustomCM2 = new ExtendedControls.ExtComboBox();
            this.comboBoxCustomCM1 = new ExtendedControls.ExtComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.LimitLargeChange = 2147483647;
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 67);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(804, 649);
            this.dataViewScrollerPanel.TabIndex = 7;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = false;
            this.vScrollBarCustom.LargeChange = 0;
            this.vScrollBarCustom.Location = new System.Drawing.Point(788, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(16, 649);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 7;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -1;
            this.vScrollBarCustom.ValueLimited = -1;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.CheckEDSM = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnDate,
            this.ColumnStar,
            this.ColumnLocation,
            this.ColumnCurrentDistance,
            this.ColumnPosition});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.Size = new System.Drawing.Size(788, 649);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            // 
            // ColumnDate
            // 
            this.ColumnDate.FillWeight = 80F;
            this.ColumnDate.HeaderText = "Date";
            this.ColumnDate.Name = "ColumnDate";
            // 
            // ColumnStar
            // 
            this.ColumnStar.HeaderText = "Star";
            this.ColumnStar.MinimumWidth = 50;
            this.ColumnStar.Name = "ColumnStar";
            this.ColumnStar.ReadOnly = true;
            // 
            // ColumnLocation
            // 
            this.ColumnLocation.HeaderText = "Location";
            this.ColumnLocation.MinimumWidth = 50;
            this.ColumnLocation.Name = "ColumnLocation";
            this.ColumnLocation.ReadOnly = true;
            // 
            // ColumnCurrentDistance
            // 
            this.ColumnCurrentDistance.FillWeight = 40F;
            this.ColumnCurrentDistance.HeaderText = "Current Distance";
            this.ColumnCurrentDistance.MinimumWidth = 50;
            this.ColumnCurrentDistance.Name = "ColumnCurrentDistance";
            this.ColumnCurrentDistance.ReadOnly = true;
            // 
            // ColumnPosition
            // 
            this.ColumnPosition.FillWeight = 75F;
            this.ColumnPosition.HeaderText = "Position";
            this.ColumnPosition.MinimumWidth = 50;
            this.ColumnPosition.Name = "ColumnPosition";
            this.ColumnPosition.ReadOnly = true;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 120000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.buttonExtExcel);
            this.panelTop.Controls.Add(this.buttonExtFind);
            this.panelTop.Controls.Add(this.comboBoxCustomCMANDOR);
            this.panelTop.Controls.Add(this.comboBoxCustomCM2);
            this.panelTop.Controls.Add(this.comboBoxCustomCM1);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(804, 67);
            this.panelTop.TabIndex = 8;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.TravelGrid_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(521, 14);
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(24, 24);
            this.buttonExtExcel.TabIndex = 37;
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // buttonExtFind
            // 
            this.buttonExtFind.Location = new System.Drawing.Point(386, 14);
            this.buttonExtFind.Name = "buttonExtFind";
            this.buttonExtFind.Size = new System.Drawing.Size(120, 23);
            this.buttonExtFind.TabIndex = 3;
            this.buttonExtFind.Text = "Find";
            this.buttonExtFind.UseVisualStyleBackColor = true;
            this.buttonExtFind.Click += new System.EventHandler(this.buttonExtFind_Click);
            // 
            // comboBoxCustomCMANDOR
            // 
            this.comboBoxCustomCMANDOR.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomCMANDOR.ButtonColorScaling = 0.5F;
            this.comboBoxCustomCMANDOR.DataSource = null;
            this.comboBoxCustomCMANDOR.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomCMANDOR.DisplayMember = "";
            this.comboBoxCustomCMANDOR.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomCMANDOR.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomCMANDOR.Location = new System.Drawing.Point(386, 43);
            this.comboBoxCustomCMANDOR.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomCMANDOR.Name = "comboBoxCustomCMANDOR";
            this.comboBoxCustomCMANDOR.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomCMANDOR.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomCMANDOR.SelectedIndex = -1;
            this.comboBoxCustomCMANDOR.SelectedItem = null;
            this.comboBoxCustomCMANDOR.SelectedValue = null;
            this.comboBoxCustomCMANDOR.Size = new System.Drawing.Size(79, 21);
            this.comboBoxCustomCMANDOR.TabIndex = 2;
            this.comboBoxCustomCMANDOR.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxCustomCMANDOR.ValueMember = "";
            // 
            // comboBoxCustomCM2
            // 
            this.comboBoxCustomCM2.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomCM2.ButtonColorScaling = 0.5F;
            this.comboBoxCustomCM2.DataSource = null;
            this.comboBoxCustomCM2.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomCM2.DisplayMember = "";
            this.comboBoxCustomCM2.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomCM2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomCM2.Location = new System.Drawing.Point(91, 42);
            this.comboBoxCustomCM2.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomCM2.Name = "comboBoxCustomCM2";
            this.comboBoxCustomCM2.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomCM2.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomCM2.SelectedIndex = -1;
            this.comboBoxCustomCM2.SelectedItem = null;
            this.comboBoxCustomCM2.SelectedValue = null;
            this.comboBoxCustomCM2.Size = new System.Drawing.Size(283, 21);
            this.comboBoxCustomCM2.TabIndex = 2;
            this.comboBoxCustomCM2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxCustomCM2.ValueMember = "";
            // 
            // comboBoxCustomCM1
            // 
            this.comboBoxCustomCM1.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomCM1.ButtonColorScaling = 0.5F;
            this.comboBoxCustomCM1.DataSource = null;
            this.comboBoxCustomCM1.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomCM1.DisplayMember = "";
            this.comboBoxCustomCM1.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomCM1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomCM1.Location = new System.Drawing.Point(91, 14);
            this.comboBoxCustomCM1.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomCM1.Name = "comboBoxCustomCM1";
            this.comboBoxCustomCM1.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomCM1.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomCM1.SelectedIndex = -1;
            this.comboBoxCustomCM1.SelectedItem = null;
            this.comboBoxCustomCM1.SelectedValue = null;
            this.comboBoxCustomCM1.Size = new System.Drawing.Size(283, 21);
            this.comboBoxCustomCM1.TabIndex = 2;
            this.comboBoxCustomCM1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxCustomCM1.ValueMember = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Item 2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Item 1";
            // 
            // SearchMaterialsCommodities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelTop);
            this.Name = "SearchMaterialsCommodities";
            this.Size = new System.Drawing.Size(804, 716);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private EDDiscovery.UserControls.Search.DataGridViewStarResults dataGridView;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtButton buttonExtFind;
        private ExtendedControls.ExtComboBox comboBoxCustomCM2;
        private ExtendedControls.ExtComboBox comboBoxCustomCM1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.ExtComboBox comboBoxCustomCMANDOR;
        private ExtendedControls.ExtButton buttonExtExcel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCurrentDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPosition;
    }
}
