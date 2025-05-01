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
 * 
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
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewEngineering = new BaseUtils.DataGridViewColumnControl();
            this.UpgradeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ModuleCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaxCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WantedCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AvailableCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PercentageCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotesCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecipeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EngineersCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonFilterUpgrade = new ExtendedControls.ExtButton();
            this.buttonFilterModule = new ExtendedControls.ExtButton();
            this.buttonFilterLevel = new ExtendedControls.ExtButton();
            this.buttonFilterEngineer = new ExtendedControls.ExtButton();
            this.buttonFilterMaterial = new ExtendedControls.ExtButton();
            this.buttonClear = new ExtendedControls.ExtButton();
            this.chkNotHistoric = new ExtendedControls.ExtCheckBox();
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.extButtonPushResources = new ExtendedControls.ExtButton();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEngineering)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewEngineering);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 30);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(800, 542);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewEngineering
            // 
            this.dataGridViewEngineering.AllowDrop = true;
            this.dataGridViewEngineering.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewEngineering.AllowUserToAddRows = false;
            this.dataGridViewEngineering.AllowUserToDeleteRows = false;
            this.dataGridViewEngineering.AllowUserToOrderColumns = true;
            this.dataGridViewEngineering.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewEngineering.AutoSortByColumnName = false;
            this.dataGridViewEngineering.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEngineering.ColumnReorder = true;
            this.dataGridViewEngineering.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.UpgradeCol,
            this.ModuleCol,
            this.LevelCol,
            this.MaxCol,
            this.WantedCol,
            this.AvailableCol,
            this.PercentageCol,
            this.NotesCol,
            this.RecipeCol,
            this.EngineersCol});
            this.dataGridViewEngineering.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewEngineering.Name = "dataGridViewEngineering";
            this.dataGridViewEngineering.PerColumnWordWrapControl = true;
            this.dataGridViewEngineering.RowHeaderMenuStrip = null;
            this.dataGridViewEngineering.RowHeadersVisible = false;
            this.dataGridViewEngineering.RowHeadersWidth = 25;
            this.dataGridViewEngineering.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewEngineering.SingleRowSelect = true;
            this.dataGridViewEngineering.Size = new System.Drawing.Size(784, 542);
            this.dataGridViewEngineering.TabIndex = 1;
            this.dataGridViewEngineering.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewEngineering_CellDoubleClick);
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
            // ModuleCol
            // 
            this.ModuleCol.HeaderText = "Module";
            this.ModuleCol.MinimumWidth = 50;
            this.ModuleCol.Name = "ModuleCol";
            this.ModuleCol.ReadOnly = true;
            this.ModuleCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // LevelCol
            // 
            this.LevelCol.FillWeight = 25F;
            this.LevelCol.HeaderText = "Level";
            this.LevelCol.MinimumWidth = 50;
            this.LevelCol.Name = "LevelCol";
            this.LevelCol.ReadOnly = true;
            this.LevelCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // AvailableCol
            // 
            this.AvailableCol.FillWeight = 25F;
            this.AvailableCol.HeaderText = "Avail.";
            this.AvailableCol.MinimumWidth = 50;
            this.AvailableCol.Name = "AvailableCol";
            this.AvailableCol.ReadOnly = true;
            this.AvailableCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // PercentageCol
            // 
            this.PercentageCol.FillWeight = 25F;
            this.PercentageCol.HeaderText = "%";
            this.PercentageCol.Name = "PercentageCol";
            this.PercentageCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // NotesCol
            // 
            this.NotesCol.FillWeight = 150F;
            this.NotesCol.HeaderText = "Notes";
            this.NotesCol.MinimumWidth = 50;
            this.NotesCol.Name = "NotesCol";
            this.NotesCol.ReadOnly = true;
            this.NotesCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // RecipeCol
            // 
            this.RecipeCol.FillWeight = 50F;
            this.RecipeCol.HeaderText = "Recipe";
            this.RecipeCol.MinimumWidth = 15;
            this.RecipeCol.Name = "RecipeCol";
            this.RecipeCol.ReadOnly = true;
            this.RecipeCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // EngineersCol
            // 
            this.EngineersCol.HeaderText = "Engineers";
            this.EngineersCol.MinimumWidth = 50;
            this.EngineersCol.Name = "EngineersCol";
            this.EngineersCol.ReadOnly = true;
            this.EngineersCol.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // vScrollBarCustomMC
            // 
            this.vScrollBarCustomMC.AlwaysHideScrollBar = false;
            this.vScrollBarCustomMC.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustomMC.ArrowButtonColor = System.Drawing.Color.LightGray;
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
            this.vScrollBarCustomMC.ThumbDrawAngle = 0F;
            this.vScrollBarCustomMC.Value = -1;
            this.vScrollBarCustomMC.ValueLimited = -1;
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // buttonFilterUpgrade
            // 
            this.buttonFilterUpgrade.Image = global::EDDiscovery.Icons.Controls.EngineerRecipe;
            this.buttonFilterUpgrade.Location = new System.Drawing.Point(8, 1);
            this.buttonFilterUpgrade.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonFilterUpgrade.Name = "buttonFilterUpgrade";
            this.buttonFilterUpgrade.Size = new System.Drawing.Size(28, 28);
            this.buttonFilterUpgrade.TabIndex = 3;
            this.toolTip.SetToolTip(this.buttonFilterUpgrade, "Filter the table by upgrade type");
            this.buttonFilterUpgrade.UseVisualStyleBackColor = true;
            this.buttonFilterUpgrade.Click += new System.EventHandler(this.buttonFilterUpgrade_Click);
            // 
            // buttonFilterModule
            // 
            this.buttonFilterModule.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFilterModule.Image = global::EDDiscovery.Icons.Controls.ModuleInfo;
            this.buttonFilterModule.Location = new System.Drawing.Point(48, 1);
            this.buttonFilterModule.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonFilterModule.Name = "buttonFilterModule";
            this.buttonFilterModule.Size = new System.Drawing.Size(28, 28);
            this.buttonFilterModule.TabIndex = 0;
            this.toolTip.SetToolTip(this.buttonFilterModule, "Filter the table by module type");
            this.buttonFilterModule.UseVisualStyleBackColor = true;
            this.buttonFilterModule.Click += new System.EventHandler(this.buttonFilterModule_Click);
            // 
            // buttonFilterLevel
            // 
            this.buttonFilterLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFilterLevel.Image = global::EDDiscovery.Icons.Controls.Level;
            this.buttonFilterLevel.Location = new System.Drawing.Point(88, 1);
            this.buttonFilterLevel.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonFilterLevel.Name = "buttonFilterLevel";
            this.buttonFilterLevel.Size = new System.Drawing.Size(28, 28);
            this.buttonFilterLevel.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonFilterLevel, "Filter the table by level");
            this.buttonFilterLevel.UseVisualStyleBackColor = true;
            this.buttonFilterLevel.Click += new System.EventHandler(this.buttonFilterLevel_Click);
            // 
            // buttonFilterEngineer
            // 
            this.buttonFilterEngineer.Image = global::EDDiscovery.Icons.Controls.EngineerCraft;
            this.buttonFilterEngineer.Location = new System.Drawing.Point(128, 1);
            this.buttonFilterEngineer.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonFilterEngineer.Name = "buttonFilterEngineer";
            this.buttonFilterEngineer.Size = new System.Drawing.Size(28, 28);
            this.buttonFilterEngineer.TabIndex = 1;
            this.toolTip.SetToolTip(this.buttonFilterEngineer, "Filter the table by engineer");
            this.buttonFilterEngineer.UseVisualStyleBackColor = true;
            this.buttonFilterEngineer.Click += new System.EventHandler(this.buttonFilterEngineer_Click);
            // 
            // buttonFilterMaterial
            // 
            this.buttonFilterMaterial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonFilterMaterial.Image = global::EDDiscovery.Icons.Controls.Materials;
            this.buttonFilterMaterial.Location = new System.Drawing.Point(168, 1);
            this.buttonFilterMaterial.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonFilterMaterial.Name = "buttonFilterMaterial";
            this.buttonFilterMaterial.Size = new System.Drawing.Size(28, 28);
            this.buttonFilterMaterial.TabIndex = 4;
            this.toolTip.SetToolTip(this.buttonFilterMaterial, "Filter the table by material");
            this.buttonFilterMaterial.UseVisualStyleBackColor = true;
            this.buttonFilterMaterial.Click += new System.EventHandler(this.buttonFilterMaterial_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClear.Image = global::EDDiscovery.Icons.Controls.Cross;
            this.buttonClear.Location = new System.Drawing.Point(208, 1);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(28, 28);
            this.buttonClear.TabIndex = 5;
            this.toolTip.SetToolTip(this.buttonClear, "Set all wanted values to zero");
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // chkNotHistoric
            // 
            this.chkNotHistoric.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkNotHistoric.CheckBoxColor = System.Drawing.Color.Gray;
            this.chkNotHistoric.CheckBoxInnerColor = System.Drawing.Color.White;
            this.chkNotHistoric.CheckColor = System.Drawing.Color.DarkBlue;
            this.chkNotHistoric.Image = global::EDDiscovery.Icons.Controls.CursorToTop;
            this.chkNotHistoric.ImageIndeterminate = null;
            this.chkNotHistoric.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.chkNotHistoric.ImageUnchecked = global::EDDiscovery.Icons.Controls.CursorStill;
            this.chkNotHistoric.Location = new System.Drawing.Point(288, 1);
            this.chkNotHistoric.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.chkNotHistoric.Name = "chkNotHistoric";
            this.chkNotHistoric.Size = new System.Drawing.Size(28, 28);
            this.chkNotHistoric.TabIndex = 6;
            this.chkNotHistoric.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.chkNotHistoric, "When red, use the materials at the cursor to estimate, when green always use the " +
        "latest materials.");
            this.chkNotHistoric.UseVisualStyleBackColor = true;
            // 
            // extCheckBoxWordWrap
            // 
            this.extCheckBoxWordWrap.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxWordWrap.BackColor = System.Drawing.Color.Transparent;
            this.extCheckBoxWordWrap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.extCheckBoxWordWrap.CheckBoxColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWordWrap.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWordWrap.Cursor = System.Windows.Forms.Cursors.Default;
            this.extCheckBoxWordWrap.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlDarkDark;
            this.extCheckBoxWordWrap.FlatAppearance.CheckedBackColor = System.Drawing.Color.Green;
            this.extCheckBoxWordWrap.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.extCheckBoxWordWrap.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Silver;
            this.extCheckBoxWordWrap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extCheckBoxWordWrap.Image = global::EDDiscovery.Icons.Controls.WordWrapOn;
            this.extCheckBoxWordWrap.ImageIndeterminate = null;
            this.extCheckBoxWordWrap.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWordWrap.ImageUnchecked = global::EDDiscovery.Icons.Controls.WordWrapOff;
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(328, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 31;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // extButtonPushResources
            // 
            this.extButtonPushResources.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonPushResources.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonPushResources.Image = global::EDDiscovery.Icons.Controls.Resources;
            this.extButtonPushResources.Location = new System.Drawing.Point(248, 1);
            this.extButtonPushResources.Margin = new System.Windows.Forms.Padding(8, 1, 4, 1);
            this.extButtonPushResources.Name = "extButtonPushResources";
            this.extButtonPushResources.Size = new System.Drawing.Size(28, 28);
            this.extButtonPushResources.TabIndex = 5;
            this.toolTip.SetToolTip(this.extButtonPushResources, "Push items wanted to Resources panel");
            this.extButtonPushResources.UseVisualStyleBackColor = true;
            this.extButtonPushResources.Click += new System.EventHandler(this.extButtonPushResources_Click);
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.buttonFilterUpgrade);
            this.panelTop.Controls.Add(this.buttonFilterModule);
            this.panelTop.Controls.Add(this.buttonFilterLevel);
            this.panelTop.Controls.Add(this.buttonFilterEngineer);
            this.panelTop.Controls.Add(this.buttonFilterMaterial);
            this.panelTop.Controls.Add(this.buttonClear);
            this.panelTop.Controls.Add(this.extButtonPushResources);
            this.panelTop.Controls.Add(this.chkNotHistoric);
            this.panelTop.Controls.Add(this.extCheckBoxWordWrap);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 30);
            this.panelTop.TabIndex = 2;
            // 
            // UserControlEngineering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlEngineering";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEngineering)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private BaseUtils.DataGridViewColumnControl dataGridViewEngineering;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonFilterLevel;
        private ExtendedControls.ExtButton buttonFilterEngineer;
        private ExtendedControls.ExtButton buttonFilterModule;
        private ExtendedControls.ExtButton buttonFilterUpgrade;
        private ExtendedControls.ExtButton buttonFilterMaterial;
        private ExtendedControls.ExtButton buttonClear;
        private ExtendedControls.ExtCheckBox chkNotHistoric;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private System.Windows.Forms.DataGridViewTextBoxColumn UpgradeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ModuleCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn LevelCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaxCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn WantedCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn AvailableCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn PercentageCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotesCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecipeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn EngineersCol;
        private ExtendedControls.ExtButton extButtonPushResources;
    }
}
