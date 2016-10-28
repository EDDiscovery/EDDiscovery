namespace EDDiscovery.UserControls
{
    partial class UserControlTravelGrid
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
            this.TopPanel = new System.Windows.Forms.Panel();
            this.buttonFilter = new ExtendedControls.ButtonExt();
            this.textBoxFilter = new ExtendedControls.TextBoxBorder();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxHistoryWindow = new ExtendedControls.ComboBoxCustom();
            this.label2 = new System.Windows.Forms.Label();
            this.dataViewScrollerPanel1 = new ExtendedControls.DataViewScrollerPanel();
            this.vScrollBarCustom1 = new ExtendedControls.VScrollBarCustom();
            this.dataGridViewTravel = new System.Windows.Forms.DataGridView();
            this.ColumnTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Icon = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnSystem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDistance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnNote = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlTG = new EDDiscovery.UserControls.UserControlLog();
            this.historyContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mapGotoStartoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.starMapColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveToAnotherCommanderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToTrilaterationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trilaterationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wantedSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bothToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.routeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewOnEDSMToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectCorrectSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemStartStop = new System.Windows.Forms.ToolStripMenuItem();
            this.TopPanel.SuspendLayout();
            this.dataViewScrollerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).BeginInit();
            this.historyContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // TopPanel
            // 
            this.TopPanel.Controls.Add(this.userControlTG);
            this.TopPanel.Controls.Add(this.buttonFilter);
            this.TopPanel.Controls.Add(this.textBoxFilter);
            this.TopPanel.Controls.Add(this.label1);
            this.TopPanel.Controls.Add(this.comboBoxHistoryWindow);
            this.TopPanel.Controls.Add(this.label2);
            this.TopPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.TopPanel.Location = new System.Drawing.Point(0, 0);
            this.TopPanel.Name = "TopPanel";
            this.TopPanel.Size = new System.Drawing.Size(870, 32);
            this.TopPanel.TabIndex = 27;
            // 
            // buttonFilter
            // 
            this.buttonFilter.BorderColorScaling = 1.25F;
            this.buttonFilter.ButtonColorScaling = 0.5F;
            this.buttonFilter.ButtonDisabledScaling = 0.5F;
            this.buttonFilter.Location = new System.Drawing.Point(443, 3);
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
            this.textBoxFilter.Location = new System.Drawing.Point(279, 6);
            this.textBoxFilter.Name = "textBoxFilter";
            this.textBoxFilter.Size = new System.Drawing.Size(148, 20);
            this.textBoxFilter.TabIndex = 1;
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
            this.dataViewScrollerPanel1.Controls.Add(this.dataGridViewTravel);
            this.dataViewScrollerPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerPanel1.InternalMargin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.dataViewScrollerPanel1.Location = new System.Drawing.Point(0, 32);
            this.dataViewScrollerPanel1.Name = "dataViewScrollerPanel1";
            this.dataViewScrollerPanel1.ScrollBarWidth = 20;
            this.dataViewScrollerPanel1.Size = new System.Drawing.Size(870, 578);
            this.dataViewScrollerPanel1.TabIndex = 28;
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
            this.vScrollBarCustom1.HideScrollBar = true;
            this.vScrollBarCustom1.LargeChange = 0;
            this.vScrollBarCustom1.Location = new System.Drawing.Point(847, 21);
            this.vScrollBarCustom1.Maximum = -1;
            this.vScrollBarCustom1.Minimum = 0;
            this.vScrollBarCustom1.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCustom1.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCustom1.Name = "vScrollBarCustom1";
            this.vScrollBarCustom1.Size = new System.Drawing.Size(20, 557);
            this.vScrollBarCustom1.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCustom1.SmallChange = 1;
            this.vScrollBarCustom1.TabIndex = 4;
            this.vScrollBarCustom1.Text = "vScrollBarCustom1";
            this.vScrollBarCustom1.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCustom1.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCustom1.ThumbColorScaling = 0.5F;
            this.vScrollBarCustom1.ThumbDrawAngle = 0F;
            this.vScrollBarCustom1.Value = -1;
            this.vScrollBarCustom1.ValueLimited = -1;
            // 
            // dataGridViewTravel
            // 
            this.dataGridViewTravel.AllowUserToAddRows = false;
            this.dataGridViewTravel.AllowUserToDeleteRows = false;
            this.dataGridViewTravel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewTravel.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTravel.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnTime,
            this.Icon,
            this.ColumnSystem,
            this.ColumnDistance,
            this.ColumnNote});
            this.dataGridViewTravel.ContextMenuStrip = this.historyContextMenu;
            this.dataGridViewTravel.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewTravel.Name = "dataGridViewTravel";
            this.dataGridViewTravel.RowHeadersWidth = 50;
            this.dataGridViewTravel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewTravel.Size = new System.Drawing.Size(847, 578);
            this.dataGridViewTravel.TabIndex = 3;
            this.dataGridViewTravel.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellClick);
            this.dataGridViewTravel.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellContentDoubleClick);
            this.dataGridViewTravel.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTravel_CellDoubleClick);
            this.dataGridViewTravel.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewTravel_ColumnHeaderMouseClick);
            this.dataGridViewTravel.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridViewTravel_ColumnWidthChanged);
            this.dataGridViewTravel.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridViewTravel_RowPostPaint);
            this.dataGridViewTravel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridViewTravel_MouseDown);
            this.dataGridViewTravel.Resize += new System.EventHandler(this.dataGridViewTravel_Resize);
            // 
            // ColumnTime
            // 
            this.ColumnTime.HeaderText = "Time";
            this.ColumnTime.MinimumWidth = 100;
            this.ColumnTime.Name = "ColumnTime";
            this.ColumnTime.ReadOnly = true;
            this.ColumnTime.Width = 140;
            // 
            // Icon
            // 
            this.Icon.HeaderText = "Event";
            this.Icon.MinimumWidth = 32;
            this.Icon.Name = "Icon";
            this.Icon.ReadOnly = true;
            this.Icon.Width = 80;
            // 
            // ColumnSystem
            // 
            this.ColumnSystem.HeaderText = "Description";
            this.ColumnSystem.MinimumWidth = 100;
            this.ColumnSystem.Name = "ColumnSystem";
            this.ColumnSystem.ReadOnly = true;
            this.ColumnSystem.Width = 150;
            // 
            // ColumnDistance
            // 
            this.ColumnDistance.HeaderText = "Information";
            this.ColumnDistance.MinimumWidth = 50;
            this.ColumnDistance.Name = "ColumnDistance";
            this.ColumnDistance.ReadOnly = true;
            this.ColumnDistance.Width = 70;
            // 
            // ColumnNote
            // 
            this.ColumnNote.FillWeight = 150F;
            this.ColumnNote.HeaderText = "Note";
            this.ColumnNote.MinimumWidth = 10;
            this.ColumnNote.Name = "ColumnNote";
            this.ColumnNote.ReadOnly = true;
            this.ColumnNote.Width = 137;
            // 
            // userControlTG
            // 
            this.userControlTG.Location = new System.Drawing.Point(748, 12);
            this.userControlTG.Name = "userControlTG";
            this.userControlTG.Size = new System.Drawing.Size(496, 224);
            this.userControlTG.TabIndex = 26;
            this.userControlTG.Load += new System.EventHandler(this.userControlTG_Load);
            // 
            // historyContextMenu
            // 
            this.historyContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapGotoStartoolStripMenuItem,
            this.starMapColourToolStripMenuItem,
            this.hideSystemToolStripMenuItem,
            this.moveToAnotherCommanderToolStripMenuItem,
            this.addToTrilaterationToolStripMenuItem,
            this.viewOnEDSMToolStripMenuItem,
            this.selectCorrectSystemToolStripMenuItem,
            this.toolStripMenuItemStartStop});
            this.historyContextMenu.Name = "historyContextMenu";
            this.historyContextMenu.Size = new System.Drawing.Size(294, 180);
            this.historyContextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.historyContextMenu_Opening);
            // 
            // mapGotoStartoolStripMenuItem
            // 
            this.mapGotoStartoolStripMenuItem.Name = "mapGotoStartoolStripMenuItem";
            this.mapGotoStartoolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.mapGotoStartoolStripMenuItem.Text = "Go to star on 3D Map";
            this.mapGotoStartoolStripMenuItem.Click += new System.EventHandler(this.mapGotoStartoolStripMenuItem_Click);
            // 
            // starMapColourToolStripMenuItem
            // 
            this.starMapColourToolStripMenuItem.Name = "starMapColourToolStripMenuItem";
            this.starMapColourToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.starMapColourToolStripMenuItem.Text = "Star Map Colour...";
            this.starMapColourToolStripMenuItem.Click += new System.EventHandler(this.starMapColourToolStripMenuItem_Click);
            // 
            // hideSystemToolStripMenuItem
            // 
            this.hideSystemToolStripMenuItem.Name = "hideSystemToolStripMenuItem";
            this.hideSystemToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.hideSystemToolStripMenuItem.Text = "Hide system";
            this.hideSystemToolStripMenuItem.Click += new System.EventHandler(this.hideSystemToolStripMenuItem_Click);
            // 
            // moveToAnotherCommanderToolStripMenuItem
            // 
            this.moveToAnotherCommanderToolStripMenuItem.Name = "moveToAnotherCommanderToolStripMenuItem";
            this.moveToAnotherCommanderToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.moveToAnotherCommanderToolStripMenuItem.Text = "Move to another Commander";
            this.moveToAnotherCommanderToolStripMenuItem.Click += new System.EventHandler(this.moveToAnotherCommanderToolStripMenuItem_Click);
            // 
            // addToTrilaterationToolStripMenuItem
            // 
            this.addToTrilaterationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trilaterationToolStripMenuItem,
            this.wantedSystemsToolStripMenuItem,
            this.bothToolStripMenuItem,
            this.routeToolStripMenuItem});
            this.addToTrilaterationToolStripMenuItem.Name = "addToTrilaterationToolStripMenuItem";
            this.addToTrilaterationToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.addToTrilaterationToolStripMenuItem.Text = "Add to ...";
            // 
            // trilaterationToolStripMenuItem
            // 
            this.trilaterationToolStripMenuItem.Name = "trilaterationToolStripMenuItem";
            this.trilaterationToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.trilaterationToolStripMenuItem.Text = "Trilateration";
            this.trilaterationToolStripMenuItem.Click += new System.EventHandler(this.trilaterationToolStripMenuItem_Click);
            // 
            // wantedSystemsToolStripMenuItem
            // 
            this.wantedSystemsToolStripMenuItem.Name = "wantedSystemsToolStripMenuItem";
            this.wantedSystemsToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.wantedSystemsToolStripMenuItem.Text = "Wanted Systems";
            this.wantedSystemsToolStripMenuItem.Click += new System.EventHandler(this.wantedSystemsToolStripMenuItem_Click);
            // 
            // bothToolStripMenuItem
            // 
            this.bothToolStripMenuItem.Name = "bothToolStripMenuItem";
            this.bothToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.bothToolStripMenuItem.Text = "Both";
            this.bothToolStripMenuItem.Click += new System.EventHandler(this.bothToolStripMenuItem_Click);
            // 
            // routeToolStripMenuItem
            // 
            this.routeToolStripMenuItem.Name = "routeToolStripMenuItem";
            this.routeToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.routeToolStripMenuItem.Text = "Saved Route";
            this.routeToolStripMenuItem.Click += new System.EventHandler(this.routeToolStripMenuItem_Click);
            // 
            // viewOnEDSMToolStripMenuItem
            // 
            this.viewOnEDSMToolStripMenuItem.Name = "viewOnEDSMToolStripMenuItem";
            this.viewOnEDSMToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.viewOnEDSMToolStripMenuItem.Text = "View on EDSM";
            this.viewOnEDSMToolStripMenuItem.Click += new System.EventHandler(this.viewOnEDSMToolStripMenuItem_Click);
            // 
            // selectCorrectSystemToolStripMenuItem
            // 
            this.selectCorrectSystemToolStripMenuItem.Name = "selectCorrectSystemToolStripMenuItem";
            this.selectCorrectSystemToolStripMenuItem.Size = new System.Drawing.Size(293, 22);
            this.selectCorrectSystemToolStripMenuItem.Text = "Assign new system";
            this.selectCorrectSystemToolStripMenuItem.Click += new System.EventHandler(this.selectCorrectSystemToolStripMenuItem_Click);
            // 
            // toolStripMenuItemStartStop
            // 
            this.toolStripMenuItemStartStop.Name = "toolStripMenuItemStartStop";
            this.toolStripMenuItemStartStop.Size = new System.Drawing.Size(293, 22);
            this.toolStripMenuItemStartStop.Text = "Set Start/Stop point for travel calculations";
            this.toolStripMenuItemStartStop.Click += new System.EventHandler(this.toolStripMenuItemStartStop_Click);
            // 
            // UserControlTravelGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataViewScrollerPanel1);
            this.Controls.Add(this.TopPanel);
            this.Name = "UserControlTravelGrid";
            this.Size = new System.Drawing.Size(870, 610);
            this.TopPanel.ResumeLayout(false);
            this.TopPanel.PerformLayout();
            this.dataViewScrollerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTravel)).EndInit();
            this.historyContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel TopPanel;
        private ExtendedControls.ButtonExt buttonFilter;
        private ExtendedControls.TextBoxBorder textBoxFilter;
        private System.Windows.Forms.Label label1;
        internal ExtendedControls.ComboBoxCustom comboBoxHistoryWindow;
        private System.Windows.Forms.Label label2;
        private ExtendedControls.DataViewScrollerPanel dataViewScrollerPanel1;
        private ExtendedControls.VScrollBarCustom vScrollBarCustom1;
        public System.Windows.Forms.DataGridView dataGridViewTravel;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Icon;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnSystem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDistance;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnNote;
        private UserControlLog userControlTG;
        private System.Windows.Forms.ContextMenuStrip historyContextMenu;
        private System.Windows.Forms.ToolStripMenuItem mapGotoStartoolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem starMapColourToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hideSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem moveToAnotherCommanderToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToTrilaterationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trilaterationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wantedSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bothToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem routeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewOnEDSMToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectCorrectSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemStartStop;
    }
}
