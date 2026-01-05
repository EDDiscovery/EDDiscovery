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
    partial class SearchScans     
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
            this.ColumnSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBody = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnPosition = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnCurrentDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnInformation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnParentParent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStarStar = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.conditionFilterUC = new ExtendedConditionsForms.ConditionFilterUC();
            this.scanSortControl = new EDDiscovery.UserControls.Search.ScanSortControl();
            this.comboBoxSearches = new ExtendedControls.ExtComboBox();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonFind = new ExtendedControls.ExtButton();
            this.extButtonNew = new ExtendedControls.ExtButton();
            this.buttonSave = new ExtendedControls.ExtButton();
            this.buttonDelete = new ExtendedControls.ExtButton();
            this.extButtonExport = new ExtendedControls.ExtButton();
            this.extButtonImport = new ExtendedControls.ExtButton();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.buttonExtExcel = new ExtendedControls.ExtButton();
            this.extCheckBoxDebug = new ExtendedControls.ExtCheckBox();
            this.labelCount = new System.Windows.Forms.Label();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustom);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridView);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 24;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(804, 342);
            this.dataViewScrollerPanel.TabIndex = 7;
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
            this.vScrollBarCustom.Location = new System.Drawing.Point(780, 0);
            this.vScrollBarCustom.Maximum = -1;
            this.vScrollBarCustom.Minimum = 0;
            this.vScrollBarCustom.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCustom.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCustom.Name = "vScrollBarCustom";
            this.vScrollBarCustom.Size = new System.Drawing.Size(24, 342);
            this.vScrollBarCustom.SkinnyStyle = ExtendedControls.ExtScrollBar.ScrollStyle.Normal;
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
            this.ColumnSystem,
            this.ColumnBody,
            this.ColumnPosition,
            this.ColumnCurrentDistance,
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
            this.dataGridView.Size = new System.Drawing.Size(780, 342);
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
            // ColumnSystem
            // 
            this.ColumnSystem.FillWeight = 60F;
            this.ColumnSystem.HeaderText = "System";
            this.ColumnSystem.Name = "ColumnSystem";
            this.ColumnSystem.ReadOnly = true;
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
            // ColumnCurrentDistance
            // 
            this.ColumnCurrentDistance.FillWeight = 40F;
            this.ColumnCurrentDistance.HeaderText = "Current Distance";
            this.ColumnCurrentDistance.Name = "ColumnCurrentDistance";
            this.ColumnCurrentDistance.ReadOnly = true;
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
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 30);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.conditionFilterUC);
            this.splitContainer.Panel1.Controls.Add(this.scanSortControl);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dataViewScrollerPanel);
            this.splitContainer.Size = new System.Drawing.Size(804, 686);
            this.splitContainer.SplitterDistance = 340;
            this.splitContainer.TabIndex = 8;
            // 
            // conditionFilterUC
            // 
            this.conditionFilterUC.AutoCompleteCommentMarker = " | ";
            this.conditionFilterUC.AutoCompleteOnMatch = false;
            this.conditionFilterUC.AutoCompleteStringCropLength = 1000;
            this.conditionFilterUC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionFilterUC.Location = new System.Drawing.Point(0, 0);
            this.conditionFilterUC.Name = "conditionFilterUC";
            this.conditionFilterUC.Padding = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.conditionFilterUC.Size = new System.Drawing.Size(804, 312);
            this.conditionFilterUC.TabIndex = 0;
            this.conditionFilterUC.VariableNames = null;
            // 
            // scanSortControl
            // 
            this.scanSortControl.Ascending = false;
            this.scanSortControl.Condition = "";
            this.scanSortControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scanSortControl.Location = new System.Drawing.Point(0, 312);
            this.scanSortControl.Name = "scanSortControl";
            this.scanSortControl.Size = new System.Drawing.Size(804, 28);
            this.scanSortControl.TabIndex = 42;
            // 
            // comboBoxSearches
            // 
            this.comboBoxSearches.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxSearches.BorderColor = System.Drawing.Color.White;
            this.comboBoxSearches.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxSearches.DataSource = null;
            this.comboBoxSearches.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxSearches.DisabledScaling = 0.5F;
            this.comboBoxSearches.DisplayMember = "";
            this.comboBoxSearches.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxSearches.GradientDirection = 90F;
            this.comboBoxSearches.Location = new System.Drawing.Point(3, 4);
            this.comboBoxSearches.Margin = new System.Windows.Forms.Padding(3, 4, 3, 1);
            this.comboBoxSearches.MouseOverScalingColor = 1.3F;
            this.comboBoxSearches.Name = "comboBoxSearches";
            this.comboBoxSearches.SelectedIndex = -1;
            this.comboBoxSearches.SelectedItem = null;
            this.comboBoxSearches.SelectedValue = null;
            this.comboBoxSearches.Size = new System.Drawing.Size(199, 21);
            this.comboBoxSearches.TabIndex = 1;
            this.comboBoxSearches.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxSearches, "Select a predefined condition");
            this.comboBoxSearches.ValueMember = "";
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.comboBoxSearches);
            this.panelTop.Controls.Add(this.buttonFind);
            this.panelTop.Controls.Add(this.extButtonNew);
            this.panelTop.Controls.Add(this.buttonSave);
            this.panelTop.Controls.Add(this.buttonDelete);
            this.panelTop.Controls.Add(this.extButtonExport);
            this.panelTop.Controls.Add(this.extButtonImport);
            this.panelTop.Controls.Add(this.extCheckBoxWordWrap);
            this.panelTop.Controls.Add(this.buttonExtExcel);
            this.panelTop.Controls.Add(this.extCheckBoxDebug);
            this.panelTop.Controls.Add(this.labelCount);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(804, 30);
            this.panelTop.TabIndex = 1;
            // 
            // buttonFind
            // 
            this.buttonFind.BackColor2 = System.Drawing.Color.Red;
            this.buttonFind.ButtonDisabledScaling = 0.5F;
            this.buttonFind.GradientDirection = 90F;
            this.buttonFind.Image = global::EDDiscovery.Icons.Controls.Find;
            this.buttonFind.Location = new System.Drawing.Point(208, 1);
            this.buttonFind.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonFind.MouseOverScaling = 1.3F;
            this.buttonFind.MouseSelectedScaling = 1.3F;
            this.buttonFind.Name = "buttonFind";
            this.buttonFind.Size = new System.Drawing.Size(28, 28);
            this.buttonFind.TabIndex = 0;
            this.toolTip.SetToolTip(this.buttonFind, "Search for condition below");
            this.buttonFind.UseVisualStyleBackColor = true;
            this.buttonFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // extButtonNew
            // 
            this.extButtonNew.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonNew.BackColor2 = System.Drawing.Color.Red;
            this.extButtonNew.ButtonDisabledScaling = 0.5F;
            this.extButtonNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonNew.GradientDirection = 90F;
            this.extButtonNew.Image = global::EDDiscovery.Icons.Controls.Stop;
            this.extButtonNew.Location = new System.Drawing.Point(242, 1);
            this.extButtonNew.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonNew.MouseOverScaling = 1.3F;
            this.extButtonNew.MouseSelectedScaling = 1.3F;
            this.extButtonNew.Name = "extButtonNew";
            this.extButtonNew.Size = new System.Drawing.Size(28, 28);
            this.extButtonNew.TabIndex = 29;
            this.toolTip.SetToolTip(this.extButtonNew, "Clear current condition");
            this.extButtonNew.UseVisualStyleBackColor = false;
            this.extButtonNew.Click += new System.EventHandler(this.extButtonNew_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor2 = System.Drawing.Color.Red;
            this.buttonSave.ButtonDisabledScaling = 0.5F;
            this.buttonSave.GradientDirection = 90F;
            this.buttonSave.Image = global::EDDiscovery.Icons.Controls.Save;
            this.buttonSave.Location = new System.Drawing.Point(276, 1);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonSave.MouseOverScaling = 1.3F;
            this.buttonSave.MouseSelectedScaling = 1.3F;
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(28, 28);
            this.buttonSave.TabIndex = 0;
            this.toolTip.SetToolTip(this.buttonSave, "Save current condition");
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.extButtonSave_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.BackColor2 = System.Drawing.Color.Red;
            this.buttonDelete.ButtonDisabledScaling = 0.5F;
            this.buttonDelete.GradientDirection = 90F;
            this.buttonDelete.Image = global::EDDiscovery.Icons.Controls.Delete;
            this.buttonDelete.Location = new System.Drawing.Point(310, 1);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonDelete.MouseOverScaling = 1.3F;
            this.buttonDelete.MouseSelectedScaling = 1.3F;
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(28, 28);
            this.buttonDelete.TabIndex = 0;
            this.toolTip.SetToolTip(this.buttonDelete, "Delete current condition");
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // extButtonExport
            // 
            this.extButtonExport.BackColor2 = System.Drawing.Color.Red;
            this.extButtonExport.ButtonDisabledScaling = 0.5F;
            this.extButtonExport.GradientDirection = 90F;
            this.extButtonExport.Image = global::EDDiscovery.Icons.Controls.ExportFile;
            this.extButtonExport.Location = new System.Drawing.Point(344, 1);
            this.extButtonExport.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonExport.MouseOverScaling = 1.3F;
            this.extButtonExport.MouseSelectedScaling = 1.3F;
            this.extButtonExport.Name = "extButtonExport";
            this.extButtonExport.Size = new System.Drawing.Size(28, 28);
            this.extButtonExport.TabIndex = 0;
            this.toolTip.SetToolTip(this.extButtonExport, "Export user searches to a file");
            this.extButtonExport.UseVisualStyleBackColor = true;
            this.extButtonExport.Click += new System.EventHandler(this.extButtonExport_Click);
            // 
            // extButtonImport
            // 
            this.extButtonImport.BackColor2 = System.Drawing.Color.Red;
            this.extButtonImport.ButtonDisabledScaling = 0.5F;
            this.extButtonImport.GradientDirection = 90F;
            this.extButtonImport.Image = global::EDDiscovery.Icons.Controls.ImportFile;
            this.extButtonImport.Location = new System.Drawing.Point(378, 1);
            this.extButtonImport.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonImport.MouseOverScaling = 1.3F;
            this.extButtonImport.MouseSelectedScaling = 1.3F;
            this.extButtonImport.Name = "extButtonImport";
            this.extButtonImport.Size = new System.Drawing.Size(28, 28);
            this.extButtonImport.TabIndex = 0;
            this.toolTip.SetToolTip(this.extButtonImport, "Import user queries from a file (overwrite existing queries if present)");
            this.extButtonImport.UseVisualStyleBackColor = true;
            this.extButtonImport.Click += new System.EventHandler(this.extButtonImport_Click);
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
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(412, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxWordWrap.MouseOverScaling = 1.3F;
            this.extCheckBoxWordWrap.MouseSelectedScaling = 1.3F;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 41;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // buttonExtExcel
            // 
            this.buttonExtExcel.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtExcel.ButtonDisabledScaling = 0.5F;
            this.buttonExtExcel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtExcel.GradientDirection = 90F;
            this.buttonExtExcel.Image = global::EDDiscovery.Icons.Controls.ExportToExcel;
            this.buttonExtExcel.Location = new System.Drawing.Point(446, 1);
            this.buttonExtExcel.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtExcel.MouseOverScaling = 1.3F;
            this.buttonExtExcel.MouseSelectedScaling = 1.3F;
            this.buttonExtExcel.Name = "buttonExtExcel";
            this.buttonExtExcel.Size = new System.Drawing.Size(28, 28);
            this.buttonExtExcel.TabIndex = 37;
            this.toolTip.SetToolTip(this.buttonExtExcel, "Output results grid to CSV");
            this.buttonExtExcel.UseVisualStyleBackColor = true;
            this.buttonExtExcel.Click += new System.EventHandler(this.buttonExtExcel_Click);
            // 
            // extCheckBoxDebug
            // 
            this.extCheckBoxDebug.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxDebug.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxDebug.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxDebug.ButtonGradientDirection = 90F;
            this.extCheckBoxDebug.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxDebug.CheckBoxGradientDirection = 225F;
            this.extCheckBoxDebug.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxDebug.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxDebug.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxDebug.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxDebug.DisabledScaling = 0.5F;
            this.extCheckBoxDebug.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxDebug.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxDebug.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxDebug.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxDebug.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxDebug.Image = global::EDDiscovery.Icons.Controls.ConfigureAddOnActions;
            this.extCheckBoxDebug.ImageIndeterminate = null;
            this.extCheckBoxDebug.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxDebug.ImageUnchecked = global::EDDiscovery.Icons.Controls.SafeModeHelp;
            this.extCheckBoxDebug.Location = new System.Drawing.Point(480, 1);
            this.extCheckBoxDebug.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxDebug.MouseOverScaling = 1.3F;
            this.extCheckBoxDebug.MouseSelectedScaling = 1.3F;
            this.extCheckBoxDebug.Name = "extCheckBoxDebug";
            this.extCheckBoxDebug.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxDebug.TabIndex = 41;
            this.extCheckBoxDebug.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxDebug, "Click to enable debug log production on search");
            this.extCheckBoxDebug.UseVisualStyleBackColor = false;
            this.extCheckBoxDebug.CheckedChanged += new System.EventHandler(this.extCheckBoxDebug_CheckedChanged);
            // 
            // labelCount
            // 
            this.labelCount.AutoSize = true;
            this.labelCount.Location = new System.Drawing.Point(514, 4);
            this.labelCount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.labelCount.Name = "labelCount";
            this.labelCount.Size = new System.Drawing.Size(43, 13);
            this.labelCount.TabIndex = 38;
            this.labelCount.Text = "<code>";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.FillWeight = 60F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Date";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.Width = 83;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.FillWeight = 60F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Star";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 82;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.FillWeight = 60F;
            this.dataGridViewTextBoxColumn3.HeaderText = "Position";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 83;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.FillWeight = 40F;
            this.dataGridViewTextBoxColumn4.HeaderText = "Current Distance";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 55;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.FillWeight = 200F;
            this.dataGridViewTextBoxColumn5.HeaderText = "Information";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 50;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 276;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.FillWeight = 150F;
            this.dataGridViewTextBoxColumn6.HeaderText = "Parent";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 206;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // SearchScans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panelTop);
            this.Name = "SearchScans";
            this.Size = new System.Drawing.Size(804, 716);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private ExtendedControls.ExtScrollBar vScrollBarCustom;
        private EDDiscovery.UserControls.Search.DataGridViewStarResults dataGridView;
        private System.Windows.Forms.SplitContainer splitContainer;
        private ExtendedConditionsForms.ConditionFilterUC conditionFilterUC;
        private ExtendedControls.ExtButton buttonFind;
        private ExtendedControls.ExtComboBox comboBoxSearches;
        private ExtendedControls.ExtButton buttonSave;
        private ExtendedControls.ExtButton buttonDelete;
        private ExtendedControls.ExtButton buttonExtExcel;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label labelCount;
        private ExtendedControls.ExtButton extButtonExport;
        private ExtendedControls.ExtButton extButtonImport;
        private ExtendedControls.ExtCheckBox extCheckBoxDebug;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private ExtendedControls.ExtButton extButtonNew;
        private Search.ScanSortControl scanSortControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnBody;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCurrentDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnInformation;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnParentParent;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStar;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnStarStar;
    }
}
