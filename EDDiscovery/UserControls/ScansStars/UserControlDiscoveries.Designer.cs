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
    partial class UserControlDiscoveries
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlDiscoveries));
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extButtonSearches = new ExtendedControls.ExtButton();
            this.textBoxSearch = new ExtendedControls.ExtTextBox();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.rollUpPanelTop = new ExtendedControls.ExtPanelRollUp();
            this.panelControls = new System.Windows.Forms.FlowLayoutPanel();
            this.labelTime = new System.Windows.Forms.Label();
            this.comboBoxTime = new ExtendedControls.ExtComboBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.labelCount = new System.Windows.Forms.Label();
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.vScrollBarCustom = new ExtendedControls.ExtScrollBar();
            this.dataGridView = new EDDiscovery.UserControls.Search.DataGridViewStarResults();
            this.ColumnDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSearches = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnParentParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStarStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rollUpPanelTop.SuspendLayout();
            this.panelControls.SuspendLayout();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor = System.Drawing.SystemColors.Control;
            this.buttonExtExcel.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtExcel.ButtonDisabledScaling = 0.5F;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.GradientDirection = 90F;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.Scan_ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(422, 2);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtExcel.MouseOverScaling = 1.3F;
            this.buttonExtExcel.MouseSelectedScaling = 1.3F;
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 29;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Export");
            this.buttonExtExcel.UseVisualStyleBackColor = false;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 20000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
            // 
            // extButtonSearches
            // 
            this.extButtonSearches.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonSearches.BackColor2 = System.Drawing.Color.Red;
            this.extButtonSearches.ButtonDisabledScaling = 0.5F;
            this.extButtonSearches.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonSearches.GradientDirection = 90F;
            this.extButtonSearches.Image = global::EDDiscovery.Icons.Controls.Find;
            this.extButtonSearches.Location = new System.Drawing.Point(354, 2);
            this.extButtonSearches.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonSearches.MouseOverScaling = 1.3F;
            this.extButtonSearches.MouseSelectedScaling = 1.3F;
            this.extButtonSearches.Name = "extButtonSearches";
            this.extButtonSearches.Size = new System.Drawing.Size(28, 28);
            this.extButtonSearches.TabIndex = 30;
            this.toolTip.SetToolTip(this.extButtonSearches, "Select discoveries to search for, taken from the search panel scan searches");
            this.extButtonSearches.UseVisualStyleBackColor = false;
            this.extButtonSearches.Click += new System.EventHandler(this.extButtonSearches_Click);
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxSearch.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSearch.BorderColor2 = System.Drawing.Color.Transparent;
            this.textBoxSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSearch.ClearOnFirstChar = false;
            this.textBoxSearch.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSearch.EndButtonEnable = true;
            this.textBoxSearch.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxSearch.EndButtonImage")));
            this.textBoxSearch.EndButtonSize16ths = 10;
            this.textBoxSearch.EndButtonVisible = false;
            this.textBoxSearch.InErrorCondition = false;
            this.textBoxSearch.Location = new System.Drawing.Point(200, 5);
            this.textBoxSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.textBoxSearch.Multiline = false;
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.ReadOnly = false;
            this.textBoxSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSearch.SelectionLength = 0;
            this.textBoxSearch.SelectionStart = 0;
            this.textBoxSearch.Size = new System.Drawing.Size(148, 20);
            this.textBoxSearch.TabIndex = 31;
            this.textBoxSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxSearch.TextNoChange = "";
            this.toolTip.SetToolTip(this.textBoxSearch, resources.GetString("textBoxSearch.ToolTip"));
            this.textBoxSearch.WordWrap = true;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.ButtonGradientDirection = 90F;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxGradientDirection = 225F;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.DisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(388, 2);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxWordWrap.MouseOverScaling = 1.3F;
            this.extCheckBoxWordWrap.MouseSelectedScaling = 1.3F;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 40;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // rollUpPanelTop
            // 
            this.rollUpPanelTop.AutoHeight = false;
            this.rollUpPanelTop.AutoHeightWidthDisable = false;
            this.rollUpPanelTop.AutoSize = true;
            this.rollUpPanelTop.AutoWidth = false;
            this.rollUpPanelTop.ChildrenThemed = true;
            this.rollUpPanelTop.Controls.Add(this.panelControls);
            this.rollUpPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.rollUpPanelTop.FlowDirection = null;
            this.rollUpPanelTop.GradientDirection = 0F;
            this.rollUpPanelTop.HiddenMarkerWidth = 400;
            this.rollUpPanelTop.Location = new System.Drawing.Point(0, 0);
            this.rollUpPanelTop.Name = "rollUpPanelTop";
            this.rollUpPanelTop.PaintTransparentColor = System.Drawing.Color.Transparent;
            this.rollUpPanelTop.PinState = true;
            this.rollUpPanelTop.RolledUpHeight = 5;
            this.rollUpPanelTop.RollUpAnimationTime = 500;
            this.rollUpPanelTop.RollUpDelay = 1000;
            this.rollUpPanelTop.SecondHiddenMarkerWidth = 0;
            this.rollUpPanelTop.ShowHiddenMarker = true;
            this.rollUpPanelTop.Size = new System.Drawing.Size(748, 32);
            this.rollUpPanelTop.TabIndex = 4;
            this.rollUpPanelTop.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.rollUpPanelTop.ThemeColorSet = -1;
            this.rollUpPanelTop.UnrollHoverDelay = 1000;
            // 
            // panelControls
            // 
            this.panelControls.AutoSize = true;
            this.panelControls.BackColor = System.Drawing.SystemColors.Control;
            this.panelControls.Controls.Add(this.labelTime);
            this.panelControls.Controls.Add(this.comboBoxTime);
            this.panelControls.Controls.Add(this.labelSearch);
            this.panelControls.Controls.Add(this.textBoxSearch);
            this.panelControls.Controls.Add(this.extButtonSearches);
            this.panelControls.Controls.Add(this.extCheckBoxWordWrap);
            this.panelControls.Controls.Add(this.buttonExtExcel);
            this.panelControls.Controls.Add(this.labelCount);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Padding = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.panelControls.Size = new System.Drawing.Size(748, 32);
            this.panelControls.TabIndex = 32;
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(11, 5);
            this.labelTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(30, 13);
            this.labelTime.TabIndex = 0;
            this.labelTime.Text = "Time";
            // 
            // comboBoxTime
            // 
            this.comboBoxTime.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxTime.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxTime.DataSource = null;
            this.comboBoxTime.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTime.DisabledScaling = 0.5F;
            this.comboBoxTime.DisplayMember = "";
            this.comboBoxTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTime.GradientDirection = 90F;
            this.comboBoxTime.Location = new System.Drawing.Point(47, 5);
            this.comboBoxTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.comboBoxTime.MouseOverScalingColor = 1.3F;
            this.comboBoxTime.Name = "comboBoxTime";
            this.comboBoxTime.SelectedIndex = -1;
            this.comboBoxTime.SelectedItem = null;
            this.comboBoxTime.SelectedValue = null;
            this.comboBoxTime.Size = new System.Drawing.Size(100, 21);
            this.comboBoxTime.TabIndex = 0;
            this.comboBoxTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxTime.ValueMember = "";
            this.comboBoxTime.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Location = new System.Drawing.Point(153, 5);
            this.labelSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(41, 13);
            this.labelSearch.TabIndex = 32;
            this.labelSearch.Text = "Search";
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(456, 5);
            this.labelCount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(43, 13);
            this.labelCount.TabIndex = 39;
            this.labelCount.Text = "<code>";
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 32);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 24;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(748, 650);
            this.dataViewScrollerPanel.TabIndex = 8;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom
            // 
            this.vScrollBarCustom.AlwaysHideScrollBar = false;
            this.vScrollBarCustom.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.vScrollBarCustom.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom.HideScrollBar = false;
            this.vScrollBarCustom.LargeChange = 0;
            this.vScrollBarCustom.Location = new System.Drawing.Point(724, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(24, 650);
            this.vScrollBarCustom.SkinnyStyle = false;
            this.vScrollBarCustom.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom.SliderDrawAngle = 90F;
            this.vScrollBarCustom.SmallChange = 1;
            this.vScrollBarCustom.TabIndex = 7;
            this.vScrollBarCustom.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom.ThumbDrawAngle = 0F;
            this.vScrollBarCustom.Value = -1;
            this.vScrollBarCustom.ValueLimited = -1;
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
            this.ColumnDate,
            this.ColumnBody,
            this.ColumnPosition,
            this.ColumnSearches,
            this.ColumnInformation,
            this.ColumnParent,
            this.ColumnParentParent,
            this.ColumnStar,
            this.ColumnStarStar});
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 0);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.PerColumnWordWrapControl = true;
            this.dataGridView.RowHeaderMenuStrip = null;
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridView.SingleRowSelect = true;
            this.dataGridView.Size = new System.Drawing.Size(724, 650);
            this.dataGridView.TabIndex = 0;
            this.dataGridView.WebLookup = EliteDangerousCore.WebExternalDataLookup.None;
            this.dataGridView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridView_SortCompare);
            // 
            // ColumnDate
            // 
            this.ColumnDate.FillWeight = 60F;
            this.ColumnDate.HeaderText = "Date";
            this.ColumnDate.Name = "ColumnDate";
            // 
            // ColumnBody
            // 
            this.ColumnBody.FillWeight = 60F;
            this.ColumnBody.HeaderText = "Body";
            this.ColumnBody.Name = "ColumnBody";
            this.ColumnBody.ReadOnly = true;
            // 
            // ColumnPosition
            // 
            this.ColumnPosition.FillWeight = 60F;
            this.ColumnPosition.HeaderText = "Position";
            this.ColumnPosition.Name = "ColumnPosition";
            this.ColumnPosition.ReadOnly = true;
            // 
            // ColumnSearches
            // 
            this.ColumnSearches.HeaderText = "Searches";
            this.ColumnSearches.Name = "ColumnSearches";
            this.ColumnSearches.ReadOnly = true;
            // 
            // ColumnInformation
            // 
            this.ColumnInformation.FillWeight = 200F;
            this.ColumnInformation.HeaderText = "Information";
            this.ColumnInformation.Name = "ColumnInformation";
            this.ColumnInformation.ReadOnly = true;
            // 
            // ColumnParent
            // 
            this.ColumnParent.FillWeight = 150F;
            this.ColumnParent.HeaderText = "Parent";
            this.ColumnParent.Name = "ColumnParent";
            this.ColumnParent.ReadOnly = true;
            // 
            // ColumnParentParent
            // 
            this.ColumnParentParent.FillWeight = 150F;
            this.ColumnParentParent.HeaderText = "Grandparent";
            this.ColumnParentParent.Name = "ColumnParentParent";
            this.ColumnParentParent.ReadOnly = true;
            // 
            // ColumnStar
            // 
            this.ColumnStar.FillWeight = 150F;
            this.ColumnStar.HeaderText = "Star";
            this.ColumnStar.Name = "ColumnStar";
            this.ColumnStar.ReadOnly = true;
            // 
            // ColumnStarStar
            // 
            this.ColumnStarStar.FillWeight = 150F;
            this.ColumnStarStar.HeaderText = "Parent of Star";
            this.ColumnStarStar.Name = "ColumnStarStar";
            this.ColumnStarStar.ReadOnly = true;
            // 
            // UserControlDiscoveries
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.rollUpPanelTop);
            this.Name = "UserControlDiscoveries";
            this.Size = new System.Drawing.Size(748, 682);
            this.rollUpPanelTop.ResumeLayout(false);
            this.rollUpPanelTop.PerformLayout();
            this.panelControls.ResumeLayout(false);
            this.panelControls.PerformLayout();
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonExtExcel;
        private ExtendedControls.ExtPanelRollUp rollUpPanelTop;
        private System.Windows.Forms.FlowLayoutPanel panelControls;
        private System.Windows.Forms.Label labelTime;
        internal ExtendedControls.ExtComboBox comboBoxTime;
        private ExtendedControls.ExtButton extButtonSearches;
        private System.Windows.Forms.Label labelSearch;
        private ExtendedControls.ExtTextBox textBoxSearch;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private Search.DataGridViewStarResults dataGridView;
        private System.Windows.Forms.Label labelCount;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBody;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSearches;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnParentParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStarStar;
    }
}
