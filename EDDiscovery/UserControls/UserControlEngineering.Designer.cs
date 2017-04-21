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
    partial class UserControlEngineering
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
            this.dataViewScrollerPanel = new ExtendedControls.DataViewScrollerPanel();
            this.dataGridViewEngineering = new System.Windows.Forms.DataGridView();
            this.UpgradeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Module = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Level = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WantedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Available = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Recipe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Engineers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.VScrollBarCustom();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonFilterUpgrade = new ExtendedControls.ButtonExt();
            this.buttonFilterLevel = new ExtendedControls.ButtonExt();
            this.buttonFilterEngineer = new ExtendedControls.ButtonExt();
            this.buttonFilterModule = new ExtendedControls.ButtonExt();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.buttonFilterMaterial = new ExtendedControls.ButtonExt();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEngineering)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewEngineering);
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
            // dataGridViewEngineering
            // 
            this.dataGridViewEngineering.AllowDrop = true;
            this.dataGridViewEngineering.AllowUserToAddRows = false;
            this.dataGridViewEngineering.AllowUserToDeleteRows = false;
            this.dataGridViewEngineering.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEngineering.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEngineering.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UpgradeCol,
            this.Module,
            this.Level,
            this.MaxCol,
            this.WantedCol,
            this.Available,
            this.Notes,
            this.Recipe,
            this.Engineers});
            this.dataGridViewEngineering.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewEngineering.Name = "dataGridViewEngineering";
            this.dataGridViewEngineering.RowHeadersVisible = false;
            this.dataGridViewEngineering.RowHeadersWidth = 25;
            this.dataGridViewEngineering.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewEngineering.Size = new System.Drawing.Size(780, 540);
            this.dataGridViewEngineering.TabIndex = 1;
            this.dataGridViewEngineering.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewModules_CellEndEdit);
            this.dataGridViewEngineering.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridViewEngineering_DragDrop);
            this.dataGridViewEngineering.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridViewEngineering_DragOver);
            this.dataGridViewEngineering.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewEngineering_MouseDown);
            this.dataGridViewEngineering.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridViewEngineering_MouseMove);
            // 
            // UpgradeCol
            // 
            this.UpgradeCol.HeaderText = "Upgrade/Mat";
            this.UpgradeCol.MinimumWidth = 50;
            this.UpgradeCol.Name = "UpgradeCol";
            this.UpgradeCol.ReadOnly = true;
            this.UpgradeCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Module
            // 
            this.Module.HeaderText = "Module";
            this.Module.MinimumWidth = 50;
            this.Module.Name = "Module";
            this.Module.ReadOnly = true;
            this.Module.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Level
            // 
            this.Level.HeaderText = "Level";
            this.Level.Name = "Level";
            this.Level.ReadOnly = true;
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
            // Engineers
            // 
            this.Engineers.HeaderText = "Engineers";
            this.Engineers.Name = "Engineers";
            this.Engineers.ReadOnly = true;
            this.Engineers.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(780, 21);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(20, 519);
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
            this.panelButtons.Controls.Add(this.buttonFilterMaterial);
            this.panelButtons.Controls.Add(this.buttonFilterUpgrade);
            this.panelButtons.Controls.Add(this.buttonFilterLevel);
            this.panelButtons.Controls.Add(this.buttonFilterEngineer);
            this.panelButtons.Controls.Add(this.buttonFilterModule);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 32);
            this.panelButtons.TabIndex = 2;
            this.toolTip1.SetToolTip(this.panelButtons, "Left click and drag on grid to reorder");
            // 
            // buttonFilterUpgrade
            // 
            this.buttonFilterUpgrade.BorderColorScaling = 1.25F;
            this.buttonFilterUpgrade.ButtonColorScaling = 0.5F;
            this.buttonFilterUpgrade.ButtonDisabledScaling = 0.5F;
            this.buttonFilterUpgrade.Location = new System.Drawing.Point(3, 4);
            this.buttonFilterUpgrade.Name = "buttonFilterUpgrade";
            this.buttonFilterUpgrade.Size = new System.Drawing.Size(100, 23);
            this.buttonFilterUpgrade.TabIndex = 3;
            this.buttonFilterUpgrade.Text = "Filter By Upgrade";
            this.buttonFilterUpgrade.UseVisualStyleBackColor = true;
            this.buttonFilterUpgrade.Click += new System.EventHandler(this.buttonFilterUpgrade_Click);
            // 
            // buttonFilterLevel
            // 
            this.buttonFilterLevel.BorderColorScaling = 1.25F;
            this.buttonFilterLevel.ButtonColorScaling = 0.5F;
            this.buttonFilterLevel.ButtonDisabledScaling = 0.5F;
            this.buttonFilterLevel.Location = new System.Drawing.Point(215, 4);
            this.buttonFilterLevel.Name = "buttonFilterLevel";
            this.buttonFilterLevel.Size = new System.Drawing.Size(100, 23);
            this.buttonFilterLevel.TabIndex = 2;
            this.buttonFilterLevel.Text = "Filter By Level";
            this.buttonFilterLevel.UseVisualStyleBackColor = true;
            this.buttonFilterLevel.Click += new System.EventHandler(this.buttonFilterLevel_Click);
            // 
            // buttonFilterEngineer
            // 
            this.buttonFilterEngineer.BorderColorScaling = 1.25F;
            this.buttonFilterEngineer.ButtonColorScaling = 0.5F;
            this.buttonFilterEngineer.ButtonDisabledScaling = 0.5F;
            this.buttonFilterEngineer.Location = new System.Drawing.Point(321, 4);
            this.buttonFilterEngineer.Name = "buttonFilterEngineer";
            this.buttonFilterEngineer.Size = new System.Drawing.Size(100, 23);
            this.buttonFilterEngineer.TabIndex = 1;
            this.buttonFilterEngineer.Text = "Filter By Engineer";
            this.buttonFilterEngineer.UseVisualStyleBackColor = true;
            this.buttonFilterEngineer.Click += new System.EventHandler(this.buttonFilterEngineer_Click);
            // 
            // buttonFilterModule
            // 
            this.buttonFilterModule.BorderColorScaling = 1.25F;
            this.buttonFilterModule.ButtonColorScaling = 0.5F;
            this.buttonFilterModule.ButtonDisabledScaling = 0.5F;
            this.buttonFilterModule.Location = new System.Drawing.Point(109, 4);
            this.buttonFilterModule.Name = "buttonFilterModule";
            this.buttonFilterModule.Size = new System.Drawing.Size(100, 23);
            this.buttonFilterModule.TabIndex = 0;
            this.buttonFilterModule.Text = "Filter By Module";
            this.buttonFilterModule.UseVisualStyleBackColor = true;
            this.buttonFilterModule.Click += new System.EventHandler(this.buttonFilterModule_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // buttonFilterMaterial
            // 
            this.buttonFilterMaterial.BorderColorScaling = 1.25F;
            this.buttonFilterMaterial.ButtonColorScaling = 0.5F;
            this.buttonFilterMaterial.ButtonDisabledScaling = 0.5F;
            this.buttonFilterMaterial.Location = new System.Drawing.Point(427, 4);
            this.buttonFilterMaterial.Name = "buttonFilterMaterial";
            this.buttonFilterMaterial.Size = new System.Drawing.Size(100, 23);
            this.buttonFilterMaterial.TabIndex = 4;
            this.buttonFilterMaterial.Text = "Filter By Material";
            this.buttonFilterMaterial.UseVisualStyleBackColor = true;
            this.buttonFilterMaterial.Click += new System.EventHandler(this.buttonFilterMaterial_Click);
            // 
            // UserControlEngineering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelButtons);
            this.Name = "UserControlEngineering";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEngineering)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewEngineering;
        private ExtendedControls.VScrollBarCustom vScrollBarCustomMC;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.ButtonExt buttonFilterLevel;
        private ExtendedControls.ButtonExt buttonFilterEngineer;
        private ExtendedControls.ButtonExt buttonFilterModule;
        private ExtendedControls.ButtonExt buttonFilterUpgrade;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpgradeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Module;
        private System.Windows.Forms.DataGridViewTextBoxColumn Level;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WantedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Available;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Recipe;
        private System.Windows.Forms.DataGridViewTextBoxColumn Engineers;
        private ExtendedControls.ButtonExt buttonFilterMaterial;
    }
}
