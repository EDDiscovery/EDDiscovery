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
    partial class UserControlMaterialTrader
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
            this.dataGridViewTrades = new System.Windows.Forms.DataGridView();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.buttonClear = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.extComboBoxTraderType = new ExtendedControls.ExtComboBox();
            this.extPanelScroll1 = new ExtendedControls.ExtPanelScroll();
            this.extPictureTrades = new ExtendedControls.ExtPictureBox();
            this.UpgradeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WantedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailableCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotesCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTrades)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.extPanelScroll1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureTrades)).BeginInit();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewTrades
            // 
            this.dataGridViewTrades.AllowDrop = true;
            this.dataGridViewTrades.AllowUserToAddRows = false;
            this.dataGridViewTrades.AllowUserToDeleteRows = false;
            this.dataGridViewTrades.AllowUserToOrderColumns = true;
            this.dataGridViewTrades.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTrades.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTrades.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UpgradeCol,
            this.LevelCol,
            this.WantedCol,
            this.ModuleCol,
            this.MaxCol,
            this.AvailableCol,
            this.NotesCol});
            this.dataGridViewTrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTrades.Location = new System.Drawing.Point(0, 25);
            this.dataGridViewTrades.Name = "dataGridViewTrades";
            this.dataGridViewTrades.RowHeadersVisible = false;
            this.dataGridViewTrades.RowHeadersWidth = 25;
            this.dataGridViewTrades.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewTrades.Size = new System.Drawing.Size(800, 143);
            this.dataGridViewTrades.TabIndex = 1;
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
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(784, 0);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(16, 572);
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
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(0, 1);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(80, 23);
            this.buttonClear.TabIndex = 5;
            this.buttonClear.Text = "Clear";
            this.toolTip.SetToolTip(this.buttonClear, "Set all wanted values to zero");
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.buttonClear);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 25);
            this.panelTop.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.extPanelScroll1);
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewTrades);
            this.splitContainer1.Panel2.Controls.Add(this.panelTop);
            this.splitContainer1.Size = new System.Drawing.Size(800, 572);
            this.splitContainer1.SplitterDistance = 400;
            this.splitContainer1.TabIndex = 2;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.extComboBoxTraderType);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 27);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // extComboBoxTraderType
            // 
            this.extComboBoxTraderType.BorderColor = System.Drawing.Color.White;
            this.extComboBoxTraderType.ButtonColorScaling = 0.5F;
            this.extComboBoxTraderType.DataSource = null;
            this.extComboBoxTraderType.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxTraderType.DisplayMember = "";
            this.extComboBoxTraderType.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.extComboBoxTraderType.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxTraderType.Location = new System.Drawing.Point(3, 3);
            this.extComboBoxTraderType.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.extComboBoxTraderType.Name = "extComboBoxTraderType";
            this.extComboBoxTraderType.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.extComboBoxTraderType.ScrollBarColor = System.Drawing.Color.LightGray;
            this.extComboBoxTraderType.SelectedIndex = -1;
            this.extComboBoxTraderType.SelectedItem = null;
            this.extComboBoxTraderType.SelectedValue = null;
            this.extComboBoxTraderType.Size = new System.Drawing.Size(198, 21);
            this.extComboBoxTraderType.TabIndex = 6;
            this.extComboBoxTraderType.Text = "extComboBox2";
            this.extComboBoxTraderType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxTraderType.ValueMember = "";
            // 
            // extPanelScroll1
            // 
            this.extPanelScroll1.Controls.Add(this.extPictureTrades);
            this.extPanelScroll1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelScroll1.FlowControlsLeftToRight = false;
            this.extPanelScroll1.Location = new System.Drawing.Point(0, 27);
            this.extPanelScroll1.Name = "extPanelScroll1";
            this.extPanelScroll1.Size = new System.Drawing.Size(800, 373);
            this.extPanelScroll1.TabIndex = 4;
            this.extPanelScroll1.VerticalScrollBarDockRight = true;
            // 
            // extPictureTrades
            // 
            this.extPictureTrades.Location = new System.Drawing.Point(0, 0);
            this.extPictureTrades.Name = "extPictureTrades";
            this.extPictureTrades.Size = new System.Drawing.Size(632, 219);
            this.extPictureTrades.TabIndex = 0;
            // 
            // UpgradeCol
            // 
            this.UpgradeCol.HeaderText = "From";
            this.UpgradeCol.MinimumWidth = 50;
            this.UpgradeCol.Name = "UpgradeCol";
            this.UpgradeCol.ReadOnly = true;
            this.UpgradeCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LevelCol
            // 
            this.LevelCol.FillWeight = 25F;
            this.LevelCol.HeaderText = "Use";
            this.LevelCol.MinimumWidth = 50;
            this.LevelCol.Name = "LevelCol";
            this.LevelCol.ReadOnly = true;
            this.LevelCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // WantedCol
            // 
            this.WantedCol.FillWeight = 25F;
            this.WantedCol.HeaderText = "Left";
            this.WantedCol.MinimumWidth = 50;
            this.WantedCol.Name = "WantedCol";
            this.WantedCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ModuleCol
            // 
            this.ModuleCol.HeaderText = "To";
            this.ModuleCol.MinimumWidth = 50;
            this.ModuleCol.Name = "ModuleCol";
            this.ModuleCol.ReadOnly = true;
            this.ModuleCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MaxCol
            // 
            this.MaxCol.FillWeight = 25F;
            this.MaxCol.HeaderText = "Get";
            this.MaxCol.MinimumWidth = 50;
            this.MaxCol.Name = "MaxCol";
            this.MaxCol.ReadOnly = true;
            this.MaxCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AvailableCol
            // 
            this.AvailableCol.FillWeight = 25F;
            this.AvailableCol.HeaderText = "Total";
            this.AvailableCol.MinimumWidth = 50;
            this.AvailableCol.Name = "AvailableCol";
            this.AvailableCol.ReadOnly = true;
            this.AvailableCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // NotesCol
            // 
            this.NotesCol.FillWeight = 150F;
            this.NotesCol.HeaderText = "Recipe";
            this.NotesCol.MinimumWidth = 50;
            this.NotesCol.Name = "NotesCol";
            this.NotesCol.ReadOnly = true;
            this.NotesCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // UserControlMaterialTrader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Name = "UserControlMaterialTrader";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTrades)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.extPanelScroll1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.extPictureTrades)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewTrades;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonClear;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private ExtendedControls.ExtPanelScroll extPanelScroll1;
        private ExtendedControls.ExtPictureBox extPictureTrades;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ExtendedControls.ExtComboBox extComboBoxTraderType;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpgradeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WantedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailableCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotesCol;
    }
}
