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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlMaterialCommodities));
            this.dataViewScrollerPanel = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewMC = new BaseUtils.DataGridViewColumnControl();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColShortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColBackPack = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColWanted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColNeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColRecipes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openRecipeInWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayItemInShoppingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearAllDisplayItemsInShoppingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.displayAllInShoppingListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vScrollBarCustomMC = new ExtendedControls.ExtScrollBar();
            this.buttonFilter = new ExtendedControls.ExtButton();
            this.textBoxItems2 = new ExtendedControls.ExtTextBox();
            this.textBoxItems1 = new ExtendedControls.ExtTextBox();
            this.labelItems2 = new System.Windows.Forms.Label();
            this.labelItems1 = new System.Windows.Forms.Label();
            this.checkBoxShowZeros = new ExtendedControls.ExtCheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.extCheckBoxWordWrap = new ExtendedControls.ExtCheckBox();
            this.extButtonFont = new ExtendedControls.ExtButton();
            this.buttonClear = new ExtendedControls.ExtButton();
            this.buttonExtImport = new ExtendedControls.ExtButton();
            this.extCheckBoxMaterialView = new ExtendedControls.ExtCheckBox();
            this.panelTop = new System.Windows.Forms.FlowLayoutPanel();
            this.extPictureBoxShoppingList = new ExtendedControls.ExtPictureBox();
            this.contextMenuStripSL = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemSLClearAll = new System.Windows.Forms.ToolStripMenuItem();
            this.extPictureBoxScrollShoppingList = new ExtendedControls.ExtPictureBoxScroll();
            this.extScrollBarShoppingList = new ExtendedControls.ExtScrollBar();
            this.dataGridViewMatView = new BaseUtils.DataGridViewColumnControl();
            this.colMType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMG1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMG2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMG3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMG4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMG5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainerPanel = new System.Windows.Forms.SplitContainer();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMC)).BeginInit();
            this.contextMenuStrip.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxShoppingList)).BeginInit();
            this.contextMenuStripSL.SuspendLayout();
            this.extPictureBoxScrollShoppingList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPanel)).BeginInit();
            this.splitContainerPanel.Panel1.SuspendLayout();
            this.splitContainerPanel.Panel2.SuspendLayout();
            this.splitContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewMC);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 0);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(1011, 716);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewMC
            // 
            this.dataGridViewMC.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewMC.AllowUserToAddRows = false;
            this.dataGridViewMC.AllowUserToDeleteRows = false;
            this.dataGridViewMC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewMC.AutoSortByColumnName = false;
            this.dataGridViewMC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMC.ColumnReorder = true;
            this.dataGridViewMC.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColShortName,
            this.ColCategory,
            this.ColType,
            this.ColNumber,
            this.ColBackPack,
            this.ColPrice,
            this.ColWanted,
            this.ColNeed,
            this.ColRecipes});
            this.dataGridViewMC.ContextMenuStrip = this.contextMenuStrip;
            this.dataGridViewMC.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewMC.Name = "dataGridViewMC";
            this.dataGridViewMC.PerColumnWordWrapControl = true;
            this.dataGridViewMC.RowHeaderMenuStrip = null;
            this.dataGridViewMC.RowHeadersVisible = false;
            this.dataGridViewMC.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewMC.SingleRowSelect = true;
            this.dataGridViewMC.Size = new System.Drawing.Size(992, 716);
            this.dataGridViewMC.TabIndex = 1;
            this.dataGridViewMC.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMC_CellDoubleClick);
            this.dataGridViewMC.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMC_CellEndEdit);
            this.dataGridViewMC.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewMC_SortCompare);
            // 
            // ColName
            // 
            this.ColName.HeaderText = "Name";
            this.ColName.MinimumWidth = 50;
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            // 
            // ColShortName
            // 
            this.ColShortName.HeaderText = "Abv";
            this.ColShortName.MinimumWidth = 25;
            this.ColShortName.Name = "ColShortName";
            this.ColShortName.ReadOnly = true;
            // 
            // ColCategory
            // 
            this.ColCategory.HeaderText = "Category";
            this.ColCategory.MinimumWidth = 50;
            this.ColCategory.Name = "ColCategory";
            this.ColCategory.ReadOnly = true;
            // 
            // ColType
            // 
            this.ColType.HeaderText = "Type";
            this.ColType.MinimumWidth = 50;
            this.ColType.Name = "ColType";
            this.ColType.ReadOnly = true;
            // 
            // ColNumber
            // 
            this.ColNumber.FillWeight = 50F;
            this.ColNumber.HeaderText = "Number";
            this.ColNumber.MinimumWidth = 50;
            this.ColNumber.Name = "ColNumber";
            this.ColNumber.ReadOnly = true;
            // 
            // ColBackPack
            // 
            this.ColBackPack.HeaderText = "Backpack";
            this.ColBackPack.Name = "ColBackPack";
            this.ColBackPack.ReadOnly = true;
            // 
            // ColPrice
            // 
            this.ColPrice.HeaderText = "Avg. Price";
            this.ColPrice.MinimumWidth = 50;
            this.ColPrice.Name = "ColPrice";
            this.ColPrice.ReadOnly = true;
            // 
            // ColWanted
            // 
            this.ColWanted.FillWeight = 50F;
            this.ColWanted.HeaderText = "Want";
            this.ColWanted.Name = "ColWanted";
            // 
            // ColNeed
            // 
            this.ColNeed.FillWeight = 50F;
            this.ColNeed.HeaderText = "Need";
            this.ColNeed.Name = "ColNeed";
            this.ColNeed.ReadOnly = true;
            // 
            // ColRecipes
            // 
            this.ColRecipes.FillWeight = 200F;
            this.ColRecipes.HeaderText = "Recipes";
            this.ColRecipes.Name = "ColRecipes";
            this.ColRecipes.ReadOnly = true;
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openRecipeInWindowToolStripMenuItem,
            this.displayItemInShoppingListToolStripMenuItem,
            this.clearAllDisplayItemsInShoppingListToolStripMenuItem,
            this.displayAllInShoppingListToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(293, 92);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // openRecipeInWindowToolStripMenuItem
            // 
            this.openRecipeInWindowToolStripMenuItem.Name = "openRecipeInWindowToolStripMenuItem";
            this.openRecipeInWindowToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.openRecipeInWindowToolStripMenuItem.Text = "Open Recipe in Window";
            this.openRecipeInWindowToolStripMenuItem.Click += new System.EventHandler(this.openRecipeInWindowToolStripMenuItem_Click);
            // 
            // displayItemInShoppingListToolStripMenuItem
            // 
            this.displayItemInShoppingListToolStripMenuItem.Checked = true;
            this.displayItemInShoppingListToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.displayItemInShoppingListToolStripMenuItem.Name = "displayItemInShoppingListToolStripMenuItem";
            this.displayItemInShoppingListToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.displayItemInShoppingListToolStripMenuItem.Text = "Display Item in shopping list";
            this.displayItemInShoppingListToolStripMenuItem.Click += new System.EventHandler(this.displayItemInShoppingListToolStripMenuItem_Click);
            // 
            // clearAllDisplayItemsInShoppingListToolStripMenuItem
            // 
            this.clearAllDisplayItemsInShoppingListToolStripMenuItem.Name = "clearAllDisplayItemsInShoppingListToolStripMenuItem";
            this.clearAllDisplayItemsInShoppingListToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.clearAllDisplayItemsInShoppingListToolStripMenuItem.Text = "Clear all display items in shopping list";
            this.clearAllDisplayItemsInShoppingListToolStripMenuItem.Click += new System.EventHandler(this.clearAllDisplayItemsInShoppingListToolStripMenuItem_Click);
            // 
            // displayAllInShoppingListToolStripMenuItem
            // 
            this.displayAllInShoppingListToolStripMenuItem.Name = "displayAllInShoppingListToolStripMenuItem";
            this.displayAllInShoppingListToolStripMenuItem.Size = new System.Drawing.Size(292, 22);
            this.displayAllInShoppingListToolStripMenuItem.Text = "Display all non zero items in shopping list";
            this.displayAllInShoppingListToolStripMenuItem.Click += new System.EventHandler(this.displayAllInShoppingListToolStripMenuItem_Click);
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
            this.vScrollBarCustomMC.Location = new System.Drawing.Point(992, 0);
            this.vScrollBarCustomMC.Maximum = -1;
            this.vScrollBarCustomMC.Minimum = 0;
            this.vScrollBarCustomMC.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustomMC.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustomMC.Name = "vScrollBarCustomMC";
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(19, 716);
            this.vScrollBarCustomMC.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustomMC.SmallChange = 1;
            this.vScrollBarCustomMC.TabIndex = 0;
            this.vScrollBarCustomMC.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustomMC.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustomMC.ThumbDrawAngle = 0F;
            this.vScrollBarCustomMC.Value = -1;
            this.vScrollBarCustomMC.ValueLimited = -1;
            // 
            // buttonFilter
            // 
            this.buttonFilter.Image = global::EDDiscovery.Icons.Controls.EventFilter;
            this.buttonFilter.Location = new System.Drawing.Point(306, 1);
            this.buttonFilter.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(28, 28);
            this.buttonFilter.TabIndex = 5;
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
            this.textBoxItems2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxItems2.ClearOnFirstChar = false;
            this.textBoxItems2.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxItems2.EndButtonEnable = true;
            this.textBoxItems2.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxItems2.EndButtonImage")));
            this.textBoxItems2.EndButtonSize16ths = 10;
            this.textBoxItems2.EndButtonVisible = false;
            this.textBoxItems2.InErrorCondition = false;
            this.textBoxItems2.Location = new System.Drawing.Point(223, 3);
            this.textBoxItems2.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
            this.textBoxItems2.Multiline = false;
            this.textBoxItems2.Name = "textBoxItems2";
            this.textBoxItems2.ReadOnly = false;
            this.textBoxItems2.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxItems2.SelectionLength = 0;
            this.textBoxItems2.SelectionStart = 0;
            this.textBoxItems2.Size = new System.Drawing.Size(75, 20);
            this.textBoxItems2.TabIndex = 4;
            this.textBoxItems2.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxItems2.TextNoChange = "";
            this.toolTip.SetToolTip(this.textBoxItems2, "Count of Items");
            this.textBoxItems2.WordWrap = true;
            // 
            // textBoxItems1
            // 
            this.textBoxItems1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxItems1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxItems1.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxItems1.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxItems1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxItems1.ClearOnFirstChar = false;
            this.textBoxItems1.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxItems1.EndButtonEnable = true;
            this.textBoxItems1.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("textBoxItems1.EndButtonImage")));
            this.textBoxItems1.EndButtonSize16ths = 10;
            this.textBoxItems1.EndButtonVisible = false;
            this.textBoxItems1.InErrorCondition = false;
            this.textBoxItems1.Location = new System.Drawing.Point(89, 3);
            this.textBoxItems1.Margin = new System.Windows.Forms.Padding(0, 3, 8, 1);
            this.textBoxItems1.Multiline = false;
            this.textBoxItems1.Name = "textBoxItems1";
            this.textBoxItems1.ReadOnly = false;
            this.textBoxItems1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxItems1.SelectionLength = 0;
            this.textBoxItems1.SelectionStart = 0;
            this.textBoxItems1.Size = new System.Drawing.Size(75, 20);
            this.textBoxItems1.TabIndex = 4;
            this.textBoxItems1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxItems1.TextNoChange = "";
            this.toolTip.SetToolTip(this.textBoxItems1, "Count of Items");
            this.textBoxItems1.WordWrap = true;
            // 
            // labelItems2
            // 
            this.labelItems2.AutoSize = true;
            this.labelItems2.Location = new System.Drawing.Point(172, 6);
            this.labelItems2.Margin = new System.Windows.Forms.Padding(0, 6, 8, 1);
            this.labelItems2.Name = "labelItems2";
            this.labelItems2.Size = new System.Drawing.Size(43, 13);
            this.labelItems2.TabIndex = 3;
            this.labelItems2.Text = "<code>";
            // 
            // labelItems1
            // 
            this.labelItems1.AutoSize = true;
            this.labelItems1.Location = new System.Drawing.Point(38, 6);
            this.labelItems1.Margin = new System.Windows.Forms.Padding(0, 6, 8, 1);
            this.labelItems1.Name = "labelItems1";
            this.labelItems1.Size = new System.Drawing.Size(43, 13);
            this.labelItems1.TabIndex = 3;
            this.labelItems1.Text = "<code>";
            // 
            // checkBoxShowZeros
            // 
            this.checkBoxShowZeros.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxShowZeros.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxShowZeros.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxShowZeros.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxShowZeros.Image = global::EDDiscovery.Icons.Controls.greenzero;
            this.checkBoxShowZeros.ImageIndeterminate = null;
            this.checkBoxShowZeros.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxShowZeros.ImageUnchecked = global::EDDiscovery.Icons.Controls.redzero;
            this.checkBoxShowZeros.Location = new System.Drawing.Point(378, 1);
            this.checkBoxShowZeros.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.checkBoxShowZeros.Name = "checkBoxShowZeros";
            this.checkBoxShowZeros.Size = new System.Drawing.Size(28, 28);
            this.checkBoxShowZeros.TabIndex = 2;
            this.checkBoxShowZeros.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxShowZeros, "Green will show materials with zero counts, red means remove them");
            this.checkBoxShowZeros.UseVisualStyleBackColor = true;
            // 
            // toolTip
            // 
            this.toolTip.AutoPopDelay = 30000;
            this.toolTip.InitialDelay = 500;
            this.toolTip.ReshowDelay = 100;
            this.toolTip.ShowAlways = true;
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
            this.extCheckBoxWordWrap.Location = new System.Drawing.Point(414, 1);
            this.extCheckBoxWordWrap.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extCheckBoxWordWrap.Name = "extCheckBoxWordWrap";
            this.extCheckBoxWordWrap.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxWordWrap.TabIndex = 33;
            this.extCheckBoxWordWrap.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxWordWrap, "Enable or disable word wrap");
            this.extCheckBoxWordWrap.UseVisualStyleBackColor = false;
            // 
            // extButtonFont
            // 
            this.extButtonFont.BackColor = System.Drawing.SystemColors.Control;
            this.extButtonFont.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.extButtonFont.Image = global::EDDiscovery.Icons.Controls.Font;
            this.extButtonFont.Location = new System.Drawing.Point(450, 1);
            this.extButtonFont.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.extButtonFont.Name = "extButtonFont";
            this.extButtonFont.Size = new System.Drawing.Size(28, 28);
            this.extButtonFont.TabIndex = 34;
            this.toolTip.SetToolTip(this.extButtonFont, "Font for shopping list");
            this.extButtonFont.UseVisualStyleBackColor = false;
            this.extButtonFont.Click += new System.EventHandler(this.extButtonFont_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClear.Image = global::EDDiscovery.Icons.Controls.Cross;
            this.buttonClear.Location = new System.Drawing.Point(342, 1);
            this.buttonClear.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(28, 28);
            this.buttonClear.TabIndex = 35;
            this.toolTip.SetToolTip(this.buttonClear, "Set all wanted values to zero");
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonExtImport
            // 
            this.buttonExtImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonExtImport.Image = global::EDDiscovery.Icons.Controls.ImportExcel;
            this.buttonExtImport.Location = new System.Drawing.Point(486, 1);
            this.buttonExtImport.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.buttonExtImport.Name = "buttonExtImport";
            this.buttonExtImport.Size = new System.Drawing.Size(28, 28);
            this.buttonExtImport.TabIndex = 39;
            this.toolTip.SetToolTip(this.buttonExtImport, "Import materials list into panel");
            this.buttonExtImport.UseVisualStyleBackColor = true;
            this.buttonExtImport.Click += new System.EventHandler(this.buttonExtImport_Click);
            // 
            // extCheckBoxMaterialView
            // 
            this.extCheckBoxMaterialView.Appearance = System.Windows.Forms.Appearance.Button;
            this.extCheckBoxMaterialView.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxMaterialView.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxMaterialView.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxMaterialView.Image = global::EDDiscovery.Icons.Controls.Materials;
            this.extCheckBoxMaterialView.ImageIndeterminate = null;
            this.extCheckBoxMaterialView.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxMaterialView.ImageUnchecked = null;
            this.extCheckBoxMaterialView.Location = new System.Drawing.Point(2, 1);
            this.extCheckBoxMaterialView.Margin = new System.Windows.Forms.Padding(2, 1, 8, 1);
            this.extCheckBoxMaterialView.Name = "extCheckBoxMaterialView";
            this.extCheckBoxMaterialView.Size = new System.Drawing.Size(28, 28);
            this.extCheckBoxMaterialView.TabIndex = 40;
            this.extCheckBoxMaterialView.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.extCheckBoxMaterialView, "Switch Material Views");
            this.extCheckBoxMaterialView.UseVisualStyleBackColor = true;
            // 
            // panelTop
            // 
            this.panelTop.AutoSize = true;
            this.panelTop.Controls.Add(this.extCheckBoxMaterialView);
            this.panelTop.Controls.Add(this.labelItems1);
            this.panelTop.Controls.Add(this.textBoxItems1);
            this.panelTop.Controls.Add(this.labelItems2);
            this.panelTop.Controls.Add(this.textBoxItems2);
            this.panelTop.Controls.Add(this.buttonFilter);
            this.panelTop.Controls.Add(this.buttonClear);
            this.panelTop.Controls.Add(this.checkBoxShowZeros);
            this.panelTop.Controls.Add(this.extCheckBoxWordWrap);
            this.panelTop.Controls.Add(this.extButtonFont);
            this.panelTop.Controls.Add(this.buttonExtImport);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1011, 30);
            this.panelTop.TabIndex = 6;
            // 
            // extPictureBoxShoppingList
            // 
            this.extPictureBoxShoppingList.ContextMenuStrip = this.contextMenuStripSL;
            this.extPictureBoxShoppingList.FillColor = System.Drawing.Color.Transparent;
            this.extPictureBoxShoppingList.FreezeTracking = false;
            this.extPictureBoxShoppingList.Location = new System.Drawing.Point(0, 0);
            this.extPictureBoxShoppingList.Name = "extPictureBoxShoppingList";
            this.extPictureBoxShoppingList.Size = new System.Drawing.Size(992, 4);
            this.extPictureBoxShoppingList.TabIndex = 2;
            // 
            // contextMenuStripSL
            // 
            this.contextMenuStripSL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemSLClearAll});
            this.contextMenuStripSL.Name = "contextMenuStrip";
            this.contextMenuStripSL.Size = new System.Drawing.Size(273, 26);
            // 
            // toolStripMenuItemSLClearAll
            // 
            this.toolStripMenuItemSLClearAll.Name = "toolStripMenuItemSLClearAll";
            this.toolStripMenuItemSLClearAll.Size = new System.Drawing.Size(272, 22);
            this.toolStripMenuItemSLClearAll.Text = "Clear all display items in shopping list";
            this.toolStripMenuItemSLClearAll.Click += new System.EventHandler(this.clearAllDisplayItemsInShoppingListToolStripMenuItem_Click);
            // 
            // extPictureBoxScrollShoppingList
            // 
            this.extPictureBoxScrollShoppingList.Controls.Add(this.extScrollBarShoppingList);
            this.extPictureBoxScrollShoppingList.Controls.Add(this.extPictureBoxShoppingList);
            this.extPictureBoxScrollShoppingList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPictureBoxScrollShoppingList.Location = new System.Drawing.Point(0, 0);
            this.extPictureBoxScrollShoppingList.Name = "extPictureBoxScrollShoppingList";
            this.extPictureBoxScrollShoppingList.ScrollBarEnabled = true;
            this.extPictureBoxScrollShoppingList.Size = new System.Drawing.Size(1011, 99);
            this.extPictureBoxScrollShoppingList.TabIndex = 2;
            this.extPictureBoxScrollShoppingList.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarShoppingList
            // 
            this.extScrollBarShoppingList.AlwaysHideScrollBar = false;
            this.extScrollBarShoppingList.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarShoppingList.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarShoppingList.ArrowDownDrawAngle = 270F;
            this.extScrollBarShoppingList.ArrowUpDrawAngle = 90F;
            this.extScrollBarShoppingList.BorderColor = System.Drawing.Color.White;
            this.extScrollBarShoppingList.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarShoppingList.HideScrollBar = true;
            this.extScrollBarShoppingList.LargeChange = 99;
            this.extScrollBarShoppingList.Location = new System.Drawing.Point(992, 0);
            this.extScrollBarShoppingList.Maximum = 3;
            this.extScrollBarShoppingList.Minimum = 0;
            this.extScrollBarShoppingList.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarShoppingList.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarShoppingList.Name = "extScrollBarShoppingList";
            this.extScrollBarShoppingList.Size = new System.Drawing.Size(19, 99);
            this.extScrollBarShoppingList.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarShoppingList.SmallChange = 1;
            this.extScrollBarShoppingList.TabIndex = 2;
            this.extScrollBarShoppingList.Text = "extScrollBar1";
            this.extScrollBarShoppingList.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarShoppingList.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarShoppingList.ThumbDrawAngle = 0F;
            this.extScrollBarShoppingList.Value = 0;
            this.extScrollBarShoppingList.ValueLimited = 0;
            // 
            // dataGridViewMatView
            // 
            this.dataGridViewMatView.AllowRowHeaderVisibleSelection = false;
            this.dataGridViewMatView.AllowUserToAddRows = false;
            this.dataGridViewMatView.AllowUserToDeleteRows = false;
            this.dataGridViewMatView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewMatView.AutoSortByColumnName = false;
            this.dataGridViewMatView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewMatView.ColumnReorder = true;
            this.dataGridViewMatView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMType,
            this.colMG1,
            this.colMG2,
            this.colMG3,
            this.colMG4,
            this.colMG5});
            this.dataGridViewMatView.Location = new System.Drawing.Point(1200, 50);
            this.dataGridViewMatView.Name = "dataGridViewMatView";
            this.dataGridViewMatView.PerColumnWordWrapControl = true;
            this.dataGridViewMatView.RowHeaderMenuStrip = null;
            this.dataGridViewMatView.RowHeadersVisible = false;
            this.dataGridViewMatView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewMatView.SingleRowSelect = true;
            this.dataGridViewMatView.Size = new System.Drawing.Size(360, 163);
            this.dataGridViewMatView.TabIndex = 1;
            this.dataGridViewMatView.Visible = false;
            this.dataGridViewMatView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMatView_CellDoubleClick);
            this.dataGridViewMatView.SortCompare += new System.Windows.Forms.DataGridViewSortCompareEventHandler(this.dataGridViewMC_SortCompare);
            // 
            // colMType
            // 
            this.colMType.FillWeight = 70F;
            this.colMType.HeaderText = "Type";
            this.colMType.Name = "colMType";
            this.colMType.ReadOnly = true;
            this.colMType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMG1
            // 
            this.colMG1.HeaderText = "G1";
            this.colMG1.Name = "colMG1";
            this.colMG1.ReadOnly = true;
            this.colMG1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMG2
            // 
            this.colMG2.HeaderText = "G2";
            this.colMG2.Name = "colMG2";
            this.colMG2.ReadOnly = true;
            this.colMG2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMG3
            // 
            this.colMG3.HeaderText = "G3";
            this.colMG3.Name = "colMG3";
            this.colMG3.ReadOnly = true;
            this.colMG3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMG4
            // 
            this.colMG4.HeaderText = "G4";
            this.colMG4.Name = "colMG4";
            this.colMG4.ReadOnly = true;
            this.colMG4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMG5
            // 
            this.colMG5.HeaderText = "G5";
            this.colMG5.Name = "colMG5";
            this.colMG5.ReadOnly = true;
            this.colMG5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // splitContainerPanel
            // 
            this.splitContainerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerPanel.Location = new System.Drawing.Point(0, 30);
            this.splitContainerPanel.Name = "splitContainerPanel";
            this.splitContainerPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerPanel.Panel1
            // 
            this.splitContainerPanel.Panel1.Controls.Add(this.extPictureBoxScrollShoppingList);
            this.splitContainerPanel.Panel1MinSize = 5;
            // 
            // splitContainerPanel.Panel2
            // 
            this.splitContainerPanel.Panel2.Controls.Add(this.dataViewScrollerPanel);
            this.splitContainerPanel.Panel2MinSize = 5;
            this.splitContainerPanel.Size = new System.Drawing.Size(1011, 819);
            this.splitContainerPanel.SplitterDistance = 99;
            this.splitContainerPanel.TabIndex = 2;
            // 
            // UserControlMaterialCommodities
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridViewMatView);
            this.Controls.Add(this.splitContainerPanel);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlMaterialCommodities";
            this.Size = new System.Drawing.Size(1011, 849);
            this.dataViewScrollerPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMC)).EndInit();
            this.contextMenuStrip.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.extPictureBoxShoppingList)).EndInit();
            this.contextMenuStripSL.ResumeLayout(false);
            this.extPictureBoxScrollShoppingList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMatView)).EndInit();
            this.splitContainerPanel.Panel1.ResumeLayout(false);
            this.splitContainerPanel.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerPanel)).EndInit();
            this.splitContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerPanel;
        private BaseUtils.DataGridViewColumnControl dataGridViewMC;
        private ExtendedControls.ExtScrollBar vScrollBarCustomMC;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtCheckBox checkBoxShowZeros;
        private ExtendedControls.ExtTextBox textBoxItems2;
        private ExtendedControls.ExtTextBox textBoxItems1;
        private System.Windows.Forms.Label labelItems2;
        private System.Windows.Forms.Label labelItems1;
        private ExtendedControls.ExtButton buttonFilter;
        private System.Windows.Forms.FlowLayoutPanel panelTop;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem openRecipeInWindowToolStripMenuItem;
        private ExtendedControls.ExtCheckBox extCheckBoxWordWrap;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColShortName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCategory;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColBackPack;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColWanted;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColNeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColRecipes;
        private ExtendedControls.ExtPictureBox extPictureBoxShoppingList;
        private ExtendedControls.ExtButton extButtonFont;
        private ExtendedControls.ExtButton buttonClear;
        private System.Windows.Forms.ToolStripMenuItem displayItemInShoppingListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearAllDisplayItemsInShoppingListToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem displayAllInShoppingListToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSL;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSLClearAll;
        private ExtendedControls.ExtPictureBoxScroll extPictureBoxScrollShoppingList;
        private ExtendedControls.ExtScrollBar extScrollBarShoppingList;
        private System.Windows.Forms.SplitContainer splitContainerPanel;
        private ExtendedControls.ExtButton buttonExtImport;
        private BaseUtils.DataGridViewColumnControl dataGridViewMatView;
        private ExtendedControls.ExtCheckBox extCheckBoxMaterialView;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMG1;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMG2;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMG3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMG4;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMG5;
    }
}
