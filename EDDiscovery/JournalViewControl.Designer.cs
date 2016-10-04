namespace EDDiscovery
{
    partial class JournalViewControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonRefresh = new ExtendedControls.ButtonExt();
            this.textBoxFilter = new ExtendedControls.TextBoxBorder();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonFilter = new ExtendedControls.ButtonExt();
            this.comboBoxJournalWindow = new ExtendedControls.ComboBoxCustom();
            this.label2 = new System.Windows.Forms.Label();
            this.dataViewScrollerPanel1 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewJournal = new System.Windows.Forms.DataGridView();
            this.historyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTipEddb = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Event = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJournal)).BeginInit();
            this.historyContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonRefresh);
            this.panel1.Controls.Add(this.textBoxFilter);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonFilter);
            this.panel1.Controls.Add(this.comboBoxJournalWindow);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(912, 34);
            this.panel1.TabIndex = 5;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.BorderColorScaling = 1.25F;
            this.buttonRefresh.ButtonColorScaling = 0.5F;
            this.buttonRefresh.ButtonDisabledScaling = 0.5F;
            this.buttonRefresh.Location = new System.Drawing.Point(560, 4);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 3;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);
            // 
            // textBoxFilter
            // 
            this.textBoxFilter.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFilter.BorderColorScaling = 0.5F;
            this.textBoxFilter.Location = new System.Drawing.Point(279, 6);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(148, 20);
            this.textBoxFilter.TabIndex = 1;
            this.toolTipEddb.SetToolTip(this.textBoxFilter, "Enter text to filter travel history on text");
            this.textBoxFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxFilter_KeyUp);
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
            // buttonFilter
            // 
            this.buttonFilter.BorderColorScaling = 1.25F;
            this.buttonFilter.ButtonColorScaling = 0.5F;
            this.buttonFilter.ButtonDisabledScaling = 0.5F;
            this.buttonFilter.Location = new System.Drawing.Point(458, 4);
            this.buttonFilter.Name = "buttonFilter";
            this.buttonFilter.Size = new System.Drawing.Size(75, 23);
            this.buttonFilter.TabIndex = 4;
            this.buttonFilter.Text = "Event Filter";
            this.buttonFilter.UseVisualStyleBackColor = true;
            this.buttonFilter.Click += new System.EventHandler(this.buttonFilter_Click);
            // 
            // comboBoxJournalWindow
            // 
            this.comboBoxJournalWindow.ArrowWidth = 1;
            this.comboBoxJournalWindow.BorderColor = System.Drawing.Color.Red;
            this.comboBoxJournalWindow.ButtonColorScaling = 0.5F;
            this.comboBoxJournalWindow.DataSource = null;
            this.comboBoxJournalWindow.DisplayMember = "";
            this.comboBoxJournalWindow.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxJournalWindow.DropDownHeight = 200;
            this.comboBoxJournalWindow.DropDownWidth = 94;
            this.comboBoxJournalWindow.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxJournalWindow.ItemHeight = 13;
            this.comboBoxJournalWindow.Location = new System.Drawing.Point(102, 4);
            this.comboBoxJournalWindow.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxJournalWindow.Name = "comboBoxJournalWindow";
            this.comboBoxJournalWindow.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxJournalWindow.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxJournalWindow.ScrollBarWidth = 16;
            this.comboBoxJournalWindow.SelectedIndex = -1;
            this.comboBoxJournalWindow.SelectedItem = null;
            this.comboBoxJournalWindow.SelectedValue = null;
            this.comboBoxJournalWindow.Size = new System.Drawing.Size(94, 20);
            this.comboBoxJournalWindow.TabIndex = 0;
            this.toolTip1.SetToolTip(this.comboBoxJournalWindow, "Select the limit of age for travel history entries to show");
            this.comboBoxJournalWindow.ValueMember = "";
            this.comboBoxJournalWindow.SelectedIndexChanged += new System.EventHandler(this.comboBoxHistoryWindow_SelectedIndexChanged);
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
            // dataViewScrollerPanel1
            // 
            this.dataViewScrollerPanel1.Controls.Add(this.vScrollBarCustom1);
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewJournal);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 34);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(912, 548);
            this.dataViewScrollerPanel1.TabIndex = 6;
            this.dataViewScrollerPanel1.VerticalScrollBarDockRight = true;
            // 
            // vScrollBarCustom1
            // 
            this.vScrollBarCustom1.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCustom1.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCustom1.ArrowColorScaling = 0.5F;
            this.vScrollBarCustom1.ArrowDownDrawAngle = 270F;
            this.vScrollBarCustom1.ArrowUpDrawAngle = 90F;
            this.vScrollBarCustom1.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCustom1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCustom1.HideScrollBar = false;
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(892, 21);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 527);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 7;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // dataGridViewJournal
            // 
            this.dataGridViewJournal.AllowUserToAddRows = false;
            this.dataGridViewJournal.AllowUserToDeleteRows = false;
            this.dataGridViewJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewJournal.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.Event,
            this.ColumnType,
            this.ColumnText});
            this.dataGridViewJournal.ContextMenuStrip = this.historyContextMenu;
            this.dataGridViewJournal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewJournal.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewJournal.Name = "dataGridViewJournal";
            this.dataGridViewJournal.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewJournal.Size = new System.Drawing.Size(892, 548);
            this.dataGridViewJournal.TabIndex = 0;
            this.dataGridViewJournal.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewJournal_ColumnHeaderMouseClick);
            this.dataGridViewJournal.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridViewJournal_ColumnWidthChanged);
            this.dataGridViewJournal.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewJournal_RowPostPaint);
            this.dataGridViewJournal.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewJournal_MouseDown);
            this.dataGridViewJournal.Resize += new System.EventHandler(this.dataGridViewJournal_Resize);
            // 
            // historyContextMenu
            // 
            this.historyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapGotoStartoolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.toolStripMenuItemStartStop});
            this.historyContextMenu.Name = "historyContextMenu";
            this.historyContextMenu.Size = new System.Drawing.Size(294, 70);
            this.historyContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.historyContextMenu_Opening);
            // 
            // mapGotoStartoolStripMenuItem
            // 
            this.mapGotoStartoolStripMenuItem.Name = "mapGotoStartoolStripMenuItem";
            this.mapGotoStartoolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.mapGotoStartoolStripMenuItem.Text = "Go to star on 3D Map";
            this.mapGotoStartoolStripMenuItem.Click += new System.EventHandler(this.mapGotoStartoolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // toolStripMenuItemStartStop
            // 
            this.toolStripMenuItemStartStop.Name = "toolStripMenuItemStartStop";
            this.toolStripMenuItemStartStop.Size = new System.Drawing.Size(293, 22);
            this.toolStripMenuItemStartStop.Text = "Set Start/Stop point for travel calculations";
            this.toolStripMenuItemStartStop.Click += new System.EventHandler(this.toolStripMenuItemStartStop_Click);
            // 
            // ColumnTime
            // 
            this.ColumnTime.HeaderText = "Time";
            this.ColumnTime.MinimumWidth = 100;
            this.ColumnTime.Name = "ColumnTime";
            this.ColumnTime.ReadOnly = true;
            this.ColumnTime.Width = 140;
            // 
            // Event
            // 
            this.Event.HeaderText = "Event";
            this.Event.MinimumWidth = 32;
            this.Event.Name = "Event";
            this.Event.ReadOnly = true;
            this.Event.Width = 80;
            // 
            // ColumnType
            // 
            this.ColumnType.HeaderText = "Description";
            this.ColumnType.MinimumWidth = 50;
            this.ColumnType.Name = "ColumnType";
            this.ColumnType.ReadOnly = true;
            // 
            // ColumnText
            // 
            this.ColumnText.HeaderText = "Information";
            this.ColumnText.MinimumWidth = 100;
            this.ColumnText.Name = "ColumnText";
            this.ColumnText.ReadOnly = true;
            this.ColumnText.Width = 700;
            // 
            // JournalViewControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "JournalViewControl";
            this.Size = new System.Drawing.Size(912, 582);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewJournal)).EndInit();
            this.historyContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewJournal;
        private ExtendedControls.ButtonExt buttonRefresh;
        private ExtendedControls.ButtonExt buttonFilter;
        private System.Windows.Forms.Panel panel1;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel1;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        private ExtendedControls.TextBoxBorder textBoxFilter;
        private System.Windows.Forms.ToolTip toolTipEddb;
        private ExtendedControls.ComboBoxCustom comboBoxJournalWindow;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip historyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStartStop;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Event;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnText;
    }
}
