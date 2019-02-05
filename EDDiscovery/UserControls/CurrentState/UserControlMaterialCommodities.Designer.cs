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
    partial class UserControlMaterialCommodities
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
            this.dataGridViewMC = new System.Windows.Forms.DataGridView();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.textBoxItems2 = new ExtendedControls.ExtTextBox();
            this.textBoxItems1 = new ExtendedControls.ExtTextBox();
            this.labelItems2 = new System.Windows.Forms.Label();
            this.labelItems1 = new System.Windows.Forms.Label();
            this.checkBoxClear = new ExtendedControls.ExtCheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMC)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewMC);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 28);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 20;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(704, 536);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewMC
            // 
            this.dataGridViewMC.AllowUserToAddRows = false;
            this.dataGridViewMC.AllowUserToDeleteRows = false;
            this.dataGridViewMC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewMC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NameCol,
            this.ShortName,
            this.Category,
            this.Type,
            this.Number,
            this.Price});
            this.dataGridViewMC.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMC.Name = "dataGridViewMC";
            this.dataGridViewMC.RowHeadersVisible = false;
            this.dataGridViewMC.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewMC.Size = new System.Drawing.Size(684, 536);
            this.dataGridViewMC.TabIndex = 1;
            this.dataGridViewMC.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewMC_SortCompare);
            // 
            // NameCol
            // 
            this.NameCol.HeaderText = "Name";
            this.NameCol.MinimumWidth = 50;
            this.NameCol.Name = "NameCol";
            this.NameCol.ReadOnly = true;
            // 
            // ShortName
            // 
            this.ShortName.HeaderText = "Abv";
            this.ShortName.MinimumWidth = 25;
            this.ShortName.Name = "ShortName";
            this.ShortName.ReadOnly = true;
            // 
            // Category
            // 
            this.Category.HeaderText = "Category";
            this.Category.MinimumWidth = 50;
            this.Category.Name = "Category";
            this.Category.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.MinimumWidth = 50;
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Number
            // 
            this.Number.HeaderText = "Number";
            this.Number.MinimumWidth = 50;
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.HeaderText = "Avg. Price";
            this.Price.MinimumWidth = 50;
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
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
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(684, 21);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(20, 515);
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
            this.panelButtons.Controls.Add(this.buttonFilter);
            this.panelButtons.Controls.Add(this.textBoxItems2);
            this.panelButtons.Controls.Add(this.textBoxItems1);
            this.panelButtons.Controls.Add(this.labelItems2);
            this.panelButtons.Controls.Add(this.labelItems1);
            this.panelButtons.Controls.Add(this.checkBoxClear);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(704, 28);
            this.panelButtons.TabIndex = 2;
            // 
            // buttonFilter
            // 
            this.buttonFilter.Location = new System.Drawing.Point(6, 2);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonFilter.TabIndex = 5;
            this.buttonFilter.Text = "Filter";
            this.toolTip.SetToolTip(this.buttonFilter, "Filter out items");
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // textBoxItems2
            // 
            this.textBoxItems2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxItems2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxItems2.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxItems2.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxItems2.BorderColorScaling = 0.5F;
            this.textBoxItems2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxItems2.ClearOnFirstChar = false;
            this.textBoxItems2.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxItems2.InErrorCondition = false;
            this.textBoxItems2.Location = new System.Drawing.Point(295, 3);
            this.textBoxItems2.Multiline = false;
            this.textBoxItems2.Name = "textBoxItems2";
            this.textBoxItems2.ReadOnly = false;
            this.textBoxItems2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxItems2.SelectionLength = 0;
            this.textBoxItems2.SelectionStart = 0;
            this.textBoxItems2.Size = new System.Drawing.Size(75, 20);
            this.textBoxItems2.TabIndex = 4;
            this.textBoxItems2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxItems2, "Count of Items");
            this.textBoxItems2.WordWrap = true;
            // 
            // textBoxItems1
            // 
            this.textBoxItems1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxItems1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxItems1.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxItems1.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxItems1.BorderColorScaling = 0.5F;
            this.textBoxItems1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxItems1.ClearOnFirstChar = false;
            this.textBoxItems1.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxItems1.InErrorCondition = false;
            this.textBoxItems1.Location = new System.Drawing.Point(145, 3);
            this.textBoxItems1.Multiline = false;
            this.textBoxItems1.Name = "textBoxItems1";
            this.textBoxItems1.ReadOnly = false;
            this.textBoxItems1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxItems1.SelectionLength = 0;
            this.textBoxItems1.SelectionStart = 0;
            this.textBoxItems1.Size = new System.Drawing.Size(75, 20);
            this.textBoxItems1.TabIndex = 4;
            this.textBoxItems1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip.SetToolTip(this.textBoxItems1, "Count of Items");
            this.textBoxItems1.WordWrap = true;
            // 
            // labelItems2
            // 
            this.labelItems2.AutoSize = true;
            this.labelItems2.Location = new System.Drawing.Point(245, 4);
            this.labelItems2.Name = "labelItems2";
            this.labelItems2.Size = new System.Drawing.Size(43, 13);
            this.labelItems2.TabIndex = 3;
            this.labelItems2.Text = "<code>";
            // 
            // labelItems1
            // 
            this.labelItems1.AutoSize = true;
            this.labelItems1.Location = new System.Drawing.Point(95, 4);
            this.labelItems1.Name = "labelItems1";
            this.labelItems1.Size = new System.Drawing.Size(43, 13);
            this.labelItems1.TabIndex = 3;
            this.labelItems1.Text = "<code>";
            // 
            // checkBoxClear
            // 
            this.checkBoxClear.AutoSize = true;
            this.checkBoxClear.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxClear.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxClear.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxClear.FontNerfReduction = 0.5F;
            this.checkBoxClear.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxClear.Location = new System.Drawing.Point(376, 6);
            this.checkBoxClear.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxClear.Name = "checkBoxClear";
            this.checkBoxClear.Size = new System.Drawing.Size(116, 17);
            this.checkBoxClear.TabIndex = 2;
            this.checkBoxClear.Text = "Remove zero items";
            this.checkBoxClear.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.checkBoxClear, "Remove zero items the time after they go to zero");
            this.checkBoxClear.UseVisualStyleBackColor = true;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlMaterialCommodities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelButtons);
            this.Name = "UserControlMaterialCommodities";
            this.Size = new System.Drawing.Size(704, 564);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMC)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewMC;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShortName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtCheckBox checkBoxClear;
        private ExtendedControls.ExtTextBox textBoxItems2;
        private ExtendedControls.ExtTextBox textBoxItems1;
        private System.Windows.Forms.Label labelItems2;
        private System.Windows.Forms.Label labelItems1;
        private ExtendedControls.ExtButton buttonFilter;
    }
}
