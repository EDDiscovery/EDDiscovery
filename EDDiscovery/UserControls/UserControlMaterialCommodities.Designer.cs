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
            this.dataViewScrollerPanel = new ExtendedControls.DataViewScrollerPanel();
            this.labelNoItems = new System.Windows.Forms.Label();
            this.dataGridViewMC = new System.Windows.Forms.DataGridView();
            this.NameCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ShortName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.VScrollBarCustom();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.comboBoxCustomAdd = new ExtendedControls.ComboBoxCustom();
            this.buttonExtApply = new ExtendedControls.ButtonExt();
            this.buttonExtModify = new ExtendedControls.ButtonExt();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.checkBoxClear = new ExtendedControls.CheckBoxCustom();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMC)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.labelNoItems);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewMC);
            this.dataViewScrollerPanel.Controls.Add(this.vScrollBarCustomMC);
            this.dataViewScrollerPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel.Location = new System.Drawing.Point(0, 32);
            this.dataViewScrollerPanel.Name = "dataViewScrollerPanel";
            this.dataViewScrollerPanel.ScrollBarWidth = 20;
            this.dataViewScrollerPanel.Size = new System.Drawing.Size(704, 532);
            this.dataViewScrollerPanel.TabIndex = 0;
            this.dataViewScrollerPanel.VerticalScrollBarDockRight = true;
            // 
            // labelNoItems
            // 
            this.labelNoItems.AutoSize = true;
            this.labelNoItems.Location = new System.Drawing.Point(3, 33);
            this.labelNoItems.Name = "labelNoItems";
            this.labelNoItems.Size = new System.Drawing.Size(82, 13);
            this.labelNoItems.TabIndex = 2;
            this.labelNoItems.Text = "No Items Found";
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
            this.dataGridViewMC.Size = new System.Drawing.Size(684, 532);
            this.dataGridViewMC.TabIndex = 1;
            this.dataGridViewMC.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewMC_CellEndEdit);
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
            this.vScrollBarCustomMC.Size = new System.Drawing.Size(20, 511);
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
            this.panelButtons.Controls.Add(this.checkBoxClear);
            this.panelButtons.Controls.Add(this.comboBoxCustomAdd);
            this.panelButtons.Controls.Add(this.buttonExtApply);
            this.panelButtons.Controls.Add(this.buttonExtModify);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(704, 32);
            this.panelButtons.TabIndex = 2;
            // 
            // comboBoxCustomAdd
            // 
            this.comboBoxCustomAdd.ArrowWidth = 1;
            this.comboBoxCustomAdd.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomAdd.ButtonColorScaling = 0.5F;
            this.comboBoxCustomAdd.DataSource = null;
            this.comboBoxCustomAdd.DisplayMember = "";
            this.comboBoxCustomAdd.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomAdd.DropDownHeight = 106;
            this.comboBoxCustomAdd.DropDownWidth = 1;
            this.comboBoxCustomAdd.Enabled = false;
            this.comboBoxCustomAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomAdd.ItemHeight = 13;
            this.comboBoxCustomAdd.Location = new System.Drawing.Point(96, 4);
            this.comboBoxCustomAdd.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomAdd.Name = "comboBoxCustomAdd";
            this.comboBoxCustomAdd.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomAdd.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomAdd.ScrollBarWidth = 16;
            this.comboBoxCustomAdd.SelectedIndex = -1;
            this.comboBoxCustomAdd.SelectedItem = null;
            this.comboBoxCustomAdd.SelectedValue = null;
            this.comboBoxCustomAdd.Size = new System.Drawing.Size(147, 23);
            this.comboBoxCustomAdd.TabIndex = 1;
            this.comboBoxCustomAdd.Text = "Add new";
            this.toolTip1.SetToolTip(this.comboBoxCustomAdd, "Select new entries to add to table");
            this.comboBoxCustomAdd.ValueMember = "";
            this.comboBoxCustomAdd.SelectedIndexChanged += new System.EventHandler(this.comboBoxCustomAdd_SelectedIndexChanged);
            // 
            // buttonExtApply
            // 
            this.buttonExtApply.BorderColorScaling = 1.25F;
            this.buttonExtApply.ButtonColorScaling = 0.5F;
            this.buttonExtApply.ButtonDisabledScaling = 0.5F;
            this.buttonExtApply.Enabled = false;
            this.buttonExtApply.Location = new System.Drawing.Point(263, 4);
            this.buttonExtApply.Name = "buttonExtApply";
            this.buttonExtApply.Size = new System.Drawing.Size(75, 23);
            this.buttonExtApply.TabIndex = 0;
            this.buttonExtApply.Text = "Apply";
            this.toolTip1.SetToolTip(this.buttonExtApply, "Apply changes entered into grid list");
            this.buttonExtApply.UseVisualStyleBackColor = true;
            this.buttonExtApply.Click += new System.EventHandler(this.buttonExtApply_Click);
            // 
            // buttonExtModify
            // 
            this.buttonExtModify.BorderColorScaling = 1.25F;
            this.buttonExtModify.ButtonColorScaling = 0.5F;
            this.buttonExtModify.ButtonDisabledScaling = 0.5F;
            this.buttonExtModify.Location = new System.Drawing.Point(4, 4);
            this.buttonExtModify.Name = "buttonExtModify";
            this.buttonExtModify.Size = new System.Drawing.Size(75, 23);
            this.buttonExtModify.TabIndex = 0;
            this.buttonExtModify.Text = "Modify";
            this.toolTip1.SetToolTip(this.buttonExtModify, "Enable modification or cancel modification to entries");
            this.buttonExtModify.UseVisualStyleBackColor = true;
            this.buttonExtModify.Click += new System.EventHandler(this.buttonExtModify_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // checkBoxClear
            // 
            this.checkBoxClear.AutoSize = true;
            this.checkBoxClear.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxClear.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxClear.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxClear.FontNerfReduction = 0.5F;
            this.checkBoxClear.Location = new System.Drawing.Point(355, 8);
            this.checkBoxClear.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxClear.Name = "checkBoxClear";
            this.checkBoxClear.Size = new System.Drawing.Size(116, 17);
            this.checkBoxClear.TabIndex = 2;
            this.checkBoxClear.Text = "Remove zero items";
            this.checkBoxClear.TickBoxReductionSize = 10;
            this.checkBoxClear.UseVisualStyleBackColor = true;
            this.checkBoxClear.CheckStateChanged += new System.EventHandler(this.checkBoxClear_CheckStateChanged);
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
            this.dataViewScrollerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewMC)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewMC;
        private ExtendedControls.VScrollBarCustom vScrollBarCustomMC;
        private System.Windows.Forms.Panel panelButtons;
        private ExtendedControls.ButtonExt buttonExtModify;
        private System.Windows.Forms.DataGridViewTextBoxColumn NameCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ShortName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Category;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private ExtendedControls.ButtonExt buttonExtApply;
        private ExtendedControls.ComboBoxCustom comboBoxCustomAdd;
        private System.Windows.Forms.Label labelNoItems;
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.CheckBoxCustom checkBoxClear;
    }
}
