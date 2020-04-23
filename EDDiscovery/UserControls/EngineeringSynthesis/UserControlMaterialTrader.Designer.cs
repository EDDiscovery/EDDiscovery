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
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.dataGridViewTrades = new System.Windows.Forms.DataGridView();
            this.UpgradeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WantedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailableCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearTradeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonClear = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxCursorToTop = new ExtendedControls.ExtCheckBox();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.extPanelScrollTrades = new ExtendedControls.ExtPanelScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.extPictureTrades = new ExtendedControls.ExtPictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.extComboBoxTraderType = new ExtendedControls.ExtComboBox();
            this.extPanelDataGridViewScrollDGVTrades = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarDGVTrades = new ExtendedControls.ExtScrollBar();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTrades)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.extPanelScrollTrades.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureTrades)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.extPanelDataGridViewScrollDGVTrades.SuspendLayout();
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
            this.AvailableCol});
            this.dataGridViewTrades.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridViewTrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewTrades.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTrades.Name = "dataGridViewTrades";
            this.dataGridViewTrades.RowHeadersVisible = false;
            this.dataGridViewTrades.RowHeadersWidth = 25;
            this.dataGridViewTrades.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewTrades.Size = new System.Drawing.Size(784, 143);
            this.dataGridViewTrades.TabIndex = 1;
            this.dataGridViewTrades.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewTrades_MouseDown);
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
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearTradeToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(133, 26);
            // 
            // clearTradeToolStripMenuItem
            // 
            this.clearTradeToolStripMenuItem.Name = "clearTradeToolStripMenuItem";
            this.clearTradeToolStripMenuItem.Size = new System.Drawing.Size(132, 22);
            this.clearTradeToolStripMenuItem.Text = "Clear Trade";
            this.clearTradeToolStripMenuItem.Click += new System.EventHandler(this.clearTradeToolStripMenuItem_Click);
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
            // checkBoxCursorToTop
            // 
            this.checkBoxCursorToTop.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxCursorToTop.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxCursorToTop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxCursorToTop.CheckBoxColor = System.Drawing.Color.White;
            this.checkBoxCursorToTop.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCursorToTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCursorToTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCursorToTop.Cursor = System.Windows.Forms.Cursors.Default;
            this.checkBoxCursorToTop.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.checkBoxCursorToTop.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.checkBoxCursorToTop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBoxCursorToTop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.checkBoxCursorToTop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxCursorToTop.Image = global::EDDiscovery.Icons.Controls.TravelGrid_CursorToTop;
            this.checkBoxCursorToTop.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCursorToTop.ImageIndeterminate = null;
            this.checkBoxCursorToTop.ImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.checkBoxCursorToTop.ImageUnchecked = global::EDDiscovery.Icons.Controls.TravelGrid_CursorStill;
            this.checkBoxCursorToTop.Location = new System.Drawing.Point(204, 1);
            this.checkBoxCursorToTop.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.checkBoxCursorToTop.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCursorToTop.Name = "checkBoxCursorToTop";
            this.checkBoxCursorToTop.Size = new System.Drawing.Size(28, 28);
            this.checkBoxCursorToTop.TabIndex = 31;
            this.checkBoxCursorToTop.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCursorToTop, "When red, use the materials at the cursor to estimate, when green always use the " +
        "latest materials.");
            this.checkBoxCursorToTop.UseVisualStyleBackColor = false;
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
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.extPanelScrollTrades);
            this.splitContainer.Panel1.Controls.Add(this.flowLayoutPanel1);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.extPanelDataGridViewScrollDGVTrades);
            this.splitContainer.Panel2.Controls.Add(this.panelTop);
            this.splitContainer.Size = new System.Drawing.Size(800, 572);
            this.splitContainer.SplitterDistance = 400;
            this.splitContainer.TabIndex = 2;
            // 
            // extPanelScrollTrades
            // 
            this.extPanelScrollTrades.Controls.Add(this.vScrollBarCustom);
            this.extPanelScrollTrades.Controls.Add(this.extPictureTrades);
            this.extPanelScrollTrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelScrollTrades.FlowControlsLeftToRight = false;
            this.extPanelScrollTrades.Location = new System.Drawing.Point(0, 30);
            this.extPanelScrollTrades.Name = "extPanelScrollTrades";
            this.extPanelScrollTrades.Size = new System.Drawing.Size(800, 370);
            this.extPanelScrollTrades.TabIndex = 4;
            this.extPanelScrollTrades.VerticalScrollBarDockRight = true;
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
            this.vScrollBarCustom.HideScrollBar = true;
            this.vScrollBarCustom.LargeChange = 20;
            this.vScrollBarCustom.Location = new System.Drawing.Point(784, 0);
            this.vScrollBarCustom.Maximum = -132;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(16, 370);
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 3;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -132;
            this.vScrollBarCustom.ValueLimited = -132;
            // 
            // extPictureTrades
            // 
            this.extPictureTrades.Location = new System.Drawing.Point(0, 0);
            this.extPictureTrades.Name = "extPictureTrades";
            this.extPictureTrades.Size = new System.Drawing.Size(632, 219);
            this.extPictureTrades.TabIndex = 0;
            this.extPictureTrades.ClickElement += new ExtendedControls.ExtPictureBox.OnElement(this.extPictureTrades_ClickElement);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.extComboBoxTraderType);
            this.flowLayoutPanel1.Controls.Add(this.checkBoxCursorToTop);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(800, 30);
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
            this.extComboBoxTraderType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxTraderType.ValueMember = "";
            // 
            // extPanelDataGridViewScrollDGVTrades
            // 
            this.extPanelDataGridViewScrollDGVTrades.Controls.Add(this.extScrollBarDGVTrades);
            this.extPanelDataGridViewScrollDGVTrades.Controls.Add(this.dataGridViewTrades);
            this.extPanelDataGridViewScrollDGVTrades.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScrollDGVTrades.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScrollDGVTrades.Location = new System.Drawing.Point(0, 25);
            this.extPanelDataGridViewScrollDGVTrades.Name = "extPanelDataGridViewScrollDGVTrades";
            this.extPanelDataGridViewScrollDGVTrades.Size = new System.Drawing.Size(800, 143);
            this.extPanelDataGridViewScrollDGVTrades.TabIndex = 4;
            this.extPanelDataGridViewScrollDGVTrades.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarDGVTrades
            // 
            this.extScrollBarDGVTrades.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarDGVTrades.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarDGVTrades.ArrowColorScaling = 0.5F;
            this.extScrollBarDGVTrades.ArrowDownDrawAngle = 270F;
            this.extScrollBarDGVTrades.ArrowUpDrawAngle = 90F;
            this.extScrollBarDGVTrades.BorderColor = System.Drawing.Color.White;
            this.extScrollBarDGVTrades.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarDGVTrades.HideScrollBar = false;
            this.extScrollBarDGVTrades.LargeChange = 0;
            this.extScrollBarDGVTrades.Location = new System.Drawing.Point(784, 0);
            this.extScrollBarDGVTrades.Maximum = -1;
            this.extScrollBarDGVTrades.Minimum = 0;
            this.extScrollBarDGVTrades.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarDGVTrades.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarDGVTrades.Name = "extScrollBarDGVTrades";
            this.extScrollBarDGVTrades.Size = new System.Drawing.Size(16, 143);
            this.extScrollBarDGVTrades.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarDGVTrades.SmallChange = 1;
            this.extScrollBarDGVTrades.TabIndex = 4;
            this.extScrollBarDGVTrades.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarDGVTrades.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarDGVTrades.ThumbColorScaling = 0.5F;
            this.extScrollBarDGVTrades.ThumbDrawAngle = 0F;
            this.extScrollBarDGVTrades.Value = -1;
            this.extScrollBarDGVTrades.ValueLimited = -1;
            // 
            // UserControlMaterialTrader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Name = "UserControlMaterialTrader";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTrades)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.extPanelScrollTrades.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.extPictureTrades)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.extPanelDataGridViewScrollDGVTrades.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewTrades;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonClear;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private System.Windows.Forms.SplitContainer splitContainer;
        private ExtendedControls.ExtPanelScroll extPanelScrollTrades;
        private ExtendedControls.ExtPictureBox extPictureTrades;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private ExtendedControls.ExtComboBox extComboBoxTraderType;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearTradeToolStripMenuItem;
        private ExtendedControls.ExtPanelDataGridViewScroll extPanelDataGridViewScrollDGVTrades;
        private ExtendedControls.ExtScrollBar extScrollBarDGVTrades;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpgradeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WantedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailableCol;
        private ExtendedControls.ExtCheckBox checkBoxCursorToTop;
    }
}
