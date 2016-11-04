namespace EDDiscovery.UserControls
{
    partial class UserControlLedger
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
            this.dataGridViewLedger = new System.Windows.Forms.DataGridView();
            this.TimeCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Notes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Credits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Debits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NormProfit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCustomMC = new ExtendedControls.VScrollBarCustom();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.buttonFilter = new ExtendedControls.ButtonExt();
            this.textBoxFilter = new ExtendedControls.TextBoxBorder();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxHistoryWindow = new ExtendedControls.ComboBoxCustom();
            this.contextMenuStripLedger = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemGotoItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataViewScrollerPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).BeginInit();
            this.panelButtons.SuspendLayout();
            this.contextMenuStripLedger.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataViewScrollerPanel
            // 
            this.dataViewScrollerPanel.Controls.Add(this.labelNoItems);
            this.dataViewScrollerPanel.Controls.Add(this.dataGridViewLedger);
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
            // labelNoItems
            // 
            this.labelNoItems.AutoSize = true;
            this.labelNoItems.Location = new System.Drawing.Point(3, 33);
            this.labelNoItems.Name = "labelNoItems";
            this.labelNoItems.Size = new System.Drawing.Size(82, 13);
            this.labelNoItems.TabIndex = 2;
            this.labelNoItems.Text = "No Items Found";
            // 
            // dataGridViewLedger
            // 
            this.dataGridViewLedger.AllowUserToAddRows = false;
            this.dataGridViewLedger.AllowUserToDeleteRows = false;
            this.dataGridViewLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TimeCol,
            this.Type,
            this.Notes,
            this.Credits,
            this.Debits,
            this.Balance,
            this.NormProfit});
            this.dataGridViewLedger.ContextMenuStrip = this.contextMenuStripLedger;
            this.dataGridViewLedger.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewLedger.Name = "dataGridViewLedger";
            this.dataGridViewLedger.RowHeadersVisible = false;
            this.dataGridViewLedger.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewLedger.Size = new System.Drawing.Size(780, 540);
            this.dataGridViewLedger.TabIndex = 1;
            this.dataGridViewLedger.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridViewMC_ColumnWidthChanged);
            this.dataGridViewLedger.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewLedger_MouseDown);
            this.dataGridViewLedger.Resize += new System.EventHandler(this.dataGridViewMC_Resize);
            // 
            // TimeCol
            // 
            this.TimeCol.HeaderText = "Time";
            this.TimeCol.MinimumWidth = 50;
            this.TimeCol.Name = "TimeCol";
            this.TimeCol.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.MinimumWidth = 50;
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Notes
            // 
            this.Notes.HeaderText = "Notes";
            this.Notes.MinimumWidth = 50;
            this.Notes.Name = "Notes";
            this.Notes.ReadOnly = true;
            // 
            // Credits
            // 
            this.Credits.HeaderText = "Credits";
            this.Credits.MinimumWidth = 50;
            this.Credits.Name = "Credits";
            this.Credits.ReadOnly = true;
            // 
            // Debits
            // 
            this.Debits.HeaderText = "Debits";
            this.Debits.MinimumWidth = 50;
            this.Debits.Name = "Debits";
            this.Debits.ReadOnly = true;
            // 
            // Balance
            // 
            this.Balance.HeaderText = "Balance";
            this.Balance.MinimumWidth = 50;
            this.Balance.Name = "Balance";
            this.Balance.ReadOnly = true;
            // 
            // NormProfit
            // 
            this.NormProfit.HeaderText = "Norm Profit";
            this.NormProfit.MinimumWidth = 20;
            this.NormProfit.Name = "NormProfit";
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
            this.panelButtons.Controls.Add(this.buttonFilter);
            this.panelButtons.Controls.Add(this.textBoxFilter);
            this.panelButtons.Controls.Add(this.label2);
            this.panelButtons.Controls.Add(this.label1);
            this.panelButtons.Controls.Add(this.comboBoxHistoryWindow);
            this.panelButtons.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelButtons.Location = new System.Drawing.Point(0, 0);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(800, 32);
            this.panelButtons.TabIndex = 2;
            // 
            // buttonFilter
            // 
            this.buttonFilter.BorderColorScaling = 1.25F;
            this.buttonFilter.ButtonColorScaling = 0.5F;
            this.buttonFilter.ButtonDisabledScaling = 0.5F;
            this.buttonFilter.Location = new System.Drawing.Point(444, 4);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonFilter.TabIndex = 25;
            this.buttonFilter.Text = "Event Filter";
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFilter.BorderColorScaling = 0.5F;
            this.textBoxFilter.Location = new System.Drawing.Point(278, 6);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(148, 20);
            this.textBoxFilter.TabIndex = 1;
            this.textBoxFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxFilter_KeyUp);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Show History ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(220, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 24;
            this.label1.Text = "Search";
            // 
            // comboBoxHistoryWindow
            // 
            this.comboBoxHistoryWindow.ArrowWidth = 1;
            this.comboBoxHistoryWindow.BorderColor = System.Drawing.Color.Red;
            this.comboBoxHistoryWindow.ButtonColorScaling = 0.5F;
            this.comboBoxHistoryWindow.DataSource = null;
            this.comboBoxHistoryWindow.DisplayMember = "";
            this.comboBoxHistoryWindow.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxHistoryWindow.DropDownHeight = 200;
            this.comboBoxHistoryWindow.DropDownWidth = 94;
            this.comboBoxHistoryWindow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxHistoryWindow.ItemHeight = 13;
            this.comboBoxHistoryWindow.Location = new System.Drawing.Point(102, 4);
            this.comboBoxHistoryWindow.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxHistoryWindow.Name = "comboBoxHistoryWindow";
            this.comboBoxHistoryWindow.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxHistoryWindow.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxHistoryWindow.ScrollBarWidth = 16;
            this.comboBoxHistoryWindow.SelectedIndex = -1;
            this.comboBoxHistoryWindow.SelectedItem = null;
            this.comboBoxHistoryWindow.SelectedValue = null;
            this.comboBoxHistoryWindow.Size = new System.Drawing.Size(94, 20);
            this.comboBoxHistoryWindow.TabIndex = 0;
            this.comboBoxHistoryWindow.ValueMember = "";
            this.comboBoxHistoryWindow.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
            // 
            // contextMenuStripLedger
            // 
            this.contextMenuStripLedger.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemGotoItem});
            this.contextMenuStripLedger.Name = "contextMenuStripLedger";
            this.contextMenuStripLedger.Size = new System.Drawing.Size(207, 26);
            // 
            // toolStripMenuItemGotoItem
            // 
            this.toolStripMenuItemGotoItem.Name = "toolStripMenuItemGotoItem";
            this.toolStripMenuItemGotoItem.Size = new System.Drawing.Size(206, 22);
            this.toolStripMenuItemGotoItem.Text = "Go to entry on travel grid";
            this.toolStripMenuItemGotoItem.Click += new System.EventHandler(this.toolStripMenuItemGotoItem_Click);
            // 
            // UserControlLedger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel);
            this.Controls.Add(this.panelButtons);
            this.Name = "UserControlLedger";
            this.Size = new System.Drawing.Size(800, 572);
            this.dataViewScrollerPanel.ResumeLayout(false);
            this.dataViewScrollerPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewLedger)).EndInit();
            this.panelButtons.ResumeLayout(false);
            this.panelButtons.PerformLayout();
            this.contextMenuStripLedger.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel;
        private System.Windows.Forms.DataGridView dataGridViewLedger;
        private ExtendedControls.VScrollBarCustom vScrollBarCustomMC;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Label labelNoItems;
        private ExtendedControls.ButtonExt buttonFilter;
        private System.Windows.Forms.Label label2;
        internal ExtendedControls.ComboBoxCustom comboBoxHistoryWindow;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.TextBoxBorder textBoxFilter;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Notes;
        private System.Windows.Forms.DataGridViewTextBoxColumn Credits;
        private System.Windows.Forms.DataGridViewTextBoxColumn Debits;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn NormProfit;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLedger;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGotoItem;
    }
}
