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
            this.panelButtons = new System.Windows.Forms.Panel();
            this.chkHistoric = new ExtendedControls.ExtCheckBox();
            this.buttonMaterialFilter = new ExtendedControls.ExtButton();
            this.buttonFilterLevel = new ExtendedControls.ExtButton();
            this.buttonRecipeFilter = new ExtendedControls.ExtButton();
            this.buttonClear = new ExtendedControls.ExtButton();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSynthesis)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewSynthesis);
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
            this.dataGridViewSynthesis.Size = new System.Drawing.Size(780, 540);
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
            this.panelButtons.Controls.Add(this.chkHistoric);
            this.panelButtons.Controls.Add(this.buttonMaterialFilter);
            this.panelButtons.Controls.Add(this.buttonFilterLevel);
            this.panelButtons.Controls.Add(this.buttonRecipeFilter);
            this.panelButtons.Controls.Add(this.buttonClear);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 32);
            this.panelButtons.TabIndex = 2;
            this.toolTip.SetToolTip(this.panelButtons, "Left click and drag on grid to reorder");
            // 
            // chkHistoric
            // 
            this.chkHistoric.AutoSize = true;
            this.chkHistoric.CheckBoxColor = System.Drawing.Color.Gray;
            this.chkHistoric.CheckBoxInnerColor = System.Drawing.Color.White;
            this.chkHistoric.CheckColor = System.Drawing.Color.DarkBlue;
            this.chkHistoric.FontNerfReduction = 0.5F;
            this.chkHistoric.ImageButtonDisabledScaling = 0.5F;
            this.chkHistoric.Location = new System.Drawing.Point(322, 8);
            this.chkHistoric.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.chkHistoric.Name = "chkHistoric";
            this.chkHistoric.Size = new System.Drawing.Size(118, 17);
            this.chkHistoric.TabIndex = 7;
            this.chkHistoric.Text = "Use Cursor Position";
            this.chkHistoric.TickBoxReductionSize = 10;
            this.toolTip.SetToolTip(this.chkHistoric, "When clicked on, use the materials at the cursor to estimate, when off always use" +
        " the latest materials.");
            this.chkHistoric.UseVisualStyleBackColor = true;
            this.chkHistoric.CheckedChanged += new System.EventHandler(this.chkHistoric_CheckedChanged);
            // 
            // buttonMaterialFilter
            // 
            this.buttonMaterialFilter.Location = new System.Drawing.Point(216, 4);
            this.buttonMaterialFilter.Name = "buttonMaterialFilter";
            this.buttonMaterialFilter.Size = new System.Drawing.Size(100, 23);
            this.buttonMaterialFilter.TabIndex = 4;
            this.buttonMaterialFilter.Text = "Filter By Material";
            this.toolTip.SetToolTip(this.buttonMaterialFilter, "Filter the table by the material type");
            this.buttonMaterialFilter.UseVisualStyleBackColor = true;
            this.buttonMaterialFilter.Click += new System.EventHandler(this.buttonMaterialFilter_Click);
            // 
            // buttonFilterLevel
            // 
            this.buttonFilterLevel.Location = new System.Drawing.Point(110, 4);
            this.buttonFilterLevel.Name = "buttonFilterLevel";
            this.buttonFilterLevel.Size = new System.Drawing.Size(100, 23);
            this.buttonFilterLevel.TabIndex = 3;
            this.buttonFilterLevel.Text = "Filter By Level";
            this.toolTip.SetToolTip(this.buttonFilterLevel, "Filter the table by the synthesis level");
            this.buttonFilterLevel.UseVisualStyleBackColor = true;
            this.buttonFilterLevel.Click += new System.EventHandler(this.buttonFilterLevel_Click);
            // 
            // buttonRecipeFilter
            // 
            this.buttonRecipeFilter.Location = new System.Drawing.Point(4, 4);
            this.buttonRecipeFilter.Name = "buttonRecipeFilter";
            this.buttonRecipeFilter.Size = new System.Drawing.Size(100, 23);
            this.buttonRecipeFilter.TabIndex = 2;
            this.buttonRecipeFilter.Text = "Filter By Synthesis";
            this.toolTip.SetToolTip(this.buttonRecipeFilter, "Filter the table by the synthesis type");
            this.buttonRecipeFilter.UseVisualStyleBackColor = true;
            this.buttonRecipeFilter.Click += new System.EventHandler(this.buttonRecipeFilter_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(697, 4);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(100, 23);
            this.buttonClear.TabIndex = 1;
            this.buttonClear.Text = "Clear Wanted";
            this.toolTip.SetToolTip(this.buttonClear, "Set all wanted values to zero");
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // UserControlSynthesis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelButtons);
            this.Name = "UserControlSynthesis";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSynthesis)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewSynthesis;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonClear;
        private ExtendedControls.ExtButton buttonRecipeFilter;
        private ExtendedControls.ExtButton buttonFilterLevel;
        private ExtendedControls.ExtButton buttonMaterialFilter;
        private ExtendedControls.ExtCheckBox chkHistoric;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpgradeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Level;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WantedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Available;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Recipe;
    }
}
