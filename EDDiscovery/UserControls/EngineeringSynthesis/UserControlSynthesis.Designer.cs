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
    partial class UserControlSynthesis
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
            this.dataGridViewSynthesis = new System.Windows.Forms.DataGridView();
            this.UpgradeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WantedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Available = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Recipe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.chkNotHistoric = new ExtendedControls.ExtCheckBox();
            this.buttonMaterialFilter = new ExtendedControls.ExtButton();
            this.buttonFilterLevel = new ExtendedControls.ExtButton();
            this.buttonRecipeFilter = new ExtendedControls.ExtButton();
            this.buttonClear = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSynthesis)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewSynthesis);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(800, 542);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewSynthesis
            // 
            this.dataGridViewSynthesis.AllowDrop = true;
            this.dataGridViewSynthesis.AllowUserToAddRows = false;
            this.dataGridViewSynthesis.AllowUserToDeleteRows = false;
            this.dataGridViewSynthesis.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewSynthesis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSynthesis.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UpgradeCol,
            this.Level,
            this.MaxCol,
            this.WantedCol,
            this.Available,
            this.Notes,
            this.Recipe});
            this.dataGridViewSynthesis.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSynthesis.Name = "dataGridViewSynthesis";
            this.dataGridViewSynthesis.RowHeadersVisible = false;
            this.dataGridViewSynthesis.RowHeadersWidth = 25;
            this.dataGridViewSynthesis.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewSynthesis.Size = new System.Drawing.Size(784, 542);
            this.dataGridViewSynthesis.TabIndex = 1;
            this.dataGridViewSynthesis.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewModules_CellEndEdit);
            this.dataGridViewSynthesis.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewSynthesis_DragDrop);
            this.dataGridViewSynthesis.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewSynthesis_DragOver);
            this.dataGridViewSynthesis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewSynthesis_MouseDown);
            this.dataGridViewSynthesis.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewSynthesis_MouseMove);
            // 
            // UpgradeCol
            // 
            this.UpgradeCol.HeaderText = "Upgrade/Mat";
            this.UpgradeCol.MinimumWidth = 50;
            this.UpgradeCol.Name = "UpgradeCol";
            this.UpgradeCol.ReadOnly = true;
            this.UpgradeCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Level
            // 
            this.Level.FillWeight = 50F;
            this.Level.HeaderText = "Level";
            this.Level.MinimumWidth = 50;
            this.Level.Name = "Level";
            this.Level.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // MaxCol
            // 
            this.MaxCol.FillWeight = 25F;
            this.MaxCol.HeaderText = "Max";
            this.MaxCol.MinimumWidth = 50;
            this.MaxCol.Name = "MaxCol";
            this.MaxCol.ReadOnly = true;
            this.MaxCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // WantedCol
            // 
            this.WantedCol.FillWeight = 25F;
            this.WantedCol.HeaderText = "Wanted";
            this.WantedCol.MinimumWidth = 50;
            this.WantedCol.Name = "WantedCol";
            this.WantedCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Available
            // 
            this.Available.FillWeight = 25F;
            this.Available.HeaderText = "Avail.";
            this.Available.MinimumWidth = 50;
            this.Available.Name = "Available";
            this.Available.ReadOnly = true;
            this.Available.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Notes
            // 
            this.Notes.FillWeight = 150F;
            this.Notes.HeaderText = "Notes";
            this.Notes.MinimumWidth = 50;
            this.Notes.Name = "Notes";
            this.Notes.ReadOnly = true;
            this.Notes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Recipe
            // 
            this.Recipe.FillWeight = 50F;
            this.Recipe.HeaderText = "Recipe";
            this.Recipe.MinimumWidth = 15;
            this.Recipe.Name = "Recipe";
            this.Recipe.ReadOnly = true;
            this.Recipe.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(16, 542);
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
            // chkNotHistoric
            // 
            this.chkNotHistoric.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkNotHistoric.CheckBoxColor = System.Drawing.Color.Gray;
            this.chkNotHistoric.CheckBoxDisabledScaling = 0.5F;
            this.chkNotHistoric.CheckBoxInnerColor = System.Drawing.Color.White;
            this.chkNotHistoric.CheckColor = System.Drawing.Color.DarkBlue;
            this.chkNotHistoric.Image = global::EDDiscovery.Icons.Controls.TravelGrid_CursorToTop;
            this.chkNotHistoric.ImageButtonDisabledScaling = 0.5F;
            this.chkNotHistoric.ImageIndeterminate = null;
            this.chkNotHistoric.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chkNotHistoric.ImageUnchecked = global::EDDiscovery.Icons.Controls.TravelGrid_CursorStill;
            this.chkNotHistoric.Location = new System.Drawing.Point(352, 1);
            this.chkNotHistoric.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.chkNotHistoric.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.chkNotHistoric.Name = "chkNotHistoric";
            this.chkNotHistoric.Size = new System.Drawing.Size(28, 28);
            this.chkNotHistoric.TabIndex = 7;
            this.chkNotHistoric.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.chkNotHistoric, "When red, use the materials at the cursor to estimate, when green always use the " +
        "latest materials.");
            this.chkNotHistoric.UseVisualStyleBackColor = true;
            this.chkNotHistoric.CheckedChanged += new System.EventHandler(this.chkHistoric_CheckedChanged);
            // 
            // buttonMaterialFilter
            // 
            this.buttonMaterialFilter.Location = new System.Drawing.Point(176, 1);
            this.buttonMaterialFilter.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonMaterialFilter.Name = "buttonMaterialFilter";
            this.buttonMaterialFilter.Size = new System.Drawing.Size(80, 23);
            this.buttonMaterialFilter.TabIndex = 4;
            this.buttonMaterialFilter.Text = "Material";
            this.toolTip.SetToolTip(this.buttonMaterialFilter, "Filter the table by the material type");
            this.buttonMaterialFilter.UseVisualStyleBackColor = true;
            this.buttonMaterialFilter.Click += new System.EventHandler(this.buttonMaterialFilter_Click);
            // 
            // buttonFilterLevel
            // 
            this.buttonFilterLevel.Location = new System.Drawing.Point(88, 1);
            this.buttonFilterLevel.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonFilterLevel.Name = "buttonFilterLevel";
            this.buttonFilterLevel.Size = new System.Drawing.Size(80, 23);
            this.buttonFilterLevel.TabIndex = 3;
            this.buttonFilterLevel.Text = "Level";
            this.toolTip.SetToolTip(this.buttonFilterLevel, "Filter the table by the synthesis level");
            this.buttonFilterLevel.UseVisualStyleBackColor = true;
            this.buttonFilterLevel.Click += new System.EventHandler(this.buttonFilterLevel_Click);
            // 
            // buttonRecipeFilter
            // 
            this.buttonRecipeFilter.Location = new System.Drawing.Point(0, 1);
            this.buttonRecipeFilter.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonRecipeFilter.Name = "buttonRecipeFilter";
            this.buttonRecipeFilter.Size = new System.Drawing.Size(80, 23);
            this.buttonRecipeFilter.TabIndex = 2;
            this.buttonRecipeFilter.Text = "Synthesis";
            this.toolTip.SetToolTip(this.buttonRecipeFilter, "Filter the table by the synthesis type");
            this.buttonRecipeFilter.UseVisualStyleBackColor = true;
            this.buttonRecipeFilter.Click += new System.EventHandler(this.buttonRecipeFilter_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(264, 1);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(80, 23);
            this.buttonClear.TabIndex = 1;
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
            this.panelTop.Controls.Add(this.buttonRecipeFilter);
            this.panelTop.Controls.Add(this.buttonFilterLevel);
            this.panelTop.Controls.Add(this.buttonMaterialFilter);
            this.panelTop.Controls.Add(this.buttonClear);
            this.panelTop.Controls.Add(this.chkNotHistoric);
            this.panelTop.Controls.Add(this.extCheckBoxWordWrap);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Margin = new System.Windows.Forms.Padding(0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 30);
            this.panelTop.TabIndex = 2;
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(388, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(0, 1, 8, 1);
            this.extCheckBoxWordWrap.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 32;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // UserControlSynthesis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlSynthesis";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSynthesis)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewSynthesis;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonClear;
        private ExtendedControls.ExtButton buttonRecipeFilter;
        private ExtendedControls.ExtButton buttonFilterLevel;
        private ExtendedControls.ExtButton buttonMaterialFilter;
        private ExtendedControls.ExtCheckBox chkNotHistoric;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpgradeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Level;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WantedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Available;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Recipe;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
    }
}
