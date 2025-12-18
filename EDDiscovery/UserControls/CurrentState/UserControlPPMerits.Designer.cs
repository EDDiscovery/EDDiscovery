using System.Windows.Forms;
using BaseUtils;
using EDDiscovery.UserControls;
using ExtendedControls;

namespace EDDiscovery.UserControls
{
    partial class UserControlPPMerits
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.checkCycle = new ExtendedControls.ExtCheckBox();
            this.checkSession = new ExtendedControls.ExtCheckBox();
            this.checkSystem = new ExtendedControls.ExtCheckBox();
            this.grid = new BaseUtils.DataGridViewColumnControl();
            this.col0 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.col4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuGrid = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.findInJournalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMeritsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyMeritsReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copySystemNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelTop = new System.Windows.Forms.Panel();
            this.extPanelDataGridViewScroll = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.extScrollBarDGV = new ExtendedControls.ExtScrollBar();
            this.findInHistoryPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.contextMenuGrid.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.extPanelDataGridViewScroll.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkCycle
            // 
            this.checkCycle.AutoSize = true;
            this.checkCycle.ButtonGradientDirection = 90F;
            this.checkCycle.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkCycle.CheckBoxGradientDirection = 225F;
            this.checkCycle.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkCycle.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkCycle.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkCycle.Checked = true;
            this.checkCycle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkCycle.DisabledScaling = 0.5F;
            this.checkCycle.ImageIndeterminate = null;
            this.checkCycle.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkCycle.ImageUnchecked = null;
            this.checkCycle.Location = new System.Drawing.Point(3, 6);
            this.checkCycle.MouseOverScaling = 1.3F;
            this.checkCycle.MouseSelectedScaling = 1.3F;
            this.checkCycle.Name = "checkCycle";
            this.checkCycle.Size = new System.Drawing.Size(52, 17);
            this.checkCycle.TabIndex = 1;
            this.checkCycle.Text = "Cycle";
            this.checkCycle.TickBoxReductionRatio = 0.75F;
            // 
            // checkSession
            // 
            this.checkSession.AutoSize = true;
            this.checkSession.ButtonGradientDirection = 90F;
            this.checkSession.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkSession.CheckBoxGradientDirection = 225F;
            this.checkSession.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkSession.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkSession.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkSession.Checked = true;
            this.checkSession.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSession.DisabledScaling = 0.5F;
            this.checkSession.ImageIndeterminate = null;
            this.checkSession.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkSession.ImageUnchecked = null;
            this.checkSession.Location = new System.Drawing.Point(103, 6);
            this.checkSession.MouseOverScaling = 1.3F;
            this.checkSession.MouseSelectedScaling = 1.3F;
            this.checkSession.Name = "checkSession";
            this.checkSession.Size = new System.Drawing.Size(63, 17);
            this.checkSession.TabIndex = 2;
            this.checkSession.Text = "Session";
            this.checkSession.TickBoxReductionRatio = 0.75F;
            // 
            // checkSystem
            // 
            this.checkSystem.AutoSize = true;
            this.checkSystem.ButtonGradientDirection = 90F;
            this.checkSystem.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkSystem.CheckBoxGradientDirection = 225F;
            this.checkSystem.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkSystem.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkSystem.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkSystem.Checked = true;
            this.checkSystem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkSystem.DisabledScaling = 0.5F;
            this.checkSystem.ImageIndeterminate = null;
            this.checkSystem.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkSystem.ImageUnchecked = null;
            this.checkSystem.Location = new System.Drawing.Point(203, 6);
            this.checkSystem.MouseOverScaling = 1.3F;
            this.checkSystem.MouseSelectedScaling = 1.3F;
            this.checkSystem.Name = "checkSystem";
            this.checkSystem.Size = new System.Drawing.Size(60, 17);
            this.checkSystem.TabIndex = 3;
            this.checkSystem.Text = "System";
            this.checkSystem.TickBoxReductionRatio = 0.75F;
            // 
            // grid
            // 
            this.grid.AllowRowHeaderVisibleSelection = false;
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToOrderColumns = true;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.AutoSortByColumnName = false;
            this.grid.ColumnReorder = true;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.col0,
            this.col1,
            this.col2,
            this.col3,
            this.col4});
            this.grid.ContextMenuStrip = this.contextMenuGrid;
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.PerColumnWordWrapControl = true;
            this.grid.ReadOnly = true;
            this.grid.RowHeaderMenuStrip = null;
            this.grid.RowHeadersVisible = false;
            this.grid.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.grid.SingleRowSelect = true;
            this.grid.Size = new System.Drawing.Size(776, 468);
            this.grid.TabIndex = 4;
            this.grid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grid_MouseDown);
            // 
            // col0
            // 
            this.col0.HeaderText = "Time";
            this.col0.Name = "col0";
            this.col0.ReadOnly = true;
            // 
            // col1
            // 
            this.col1.HeaderText = "Power";
            this.col1.Name = "col1";
            this.col1.ReadOnly = true;
            // 
            // col2
            // 
            this.col2.HeaderText = "Merits";
            this.col2.Name = "col2";
            this.col2.ReadOnly = true;
            // 
            // col3
            // 
            this.col3.HeaderText = "Total";
            this.col3.Name = "col3";
            this.col3.ReadOnly = true;
            // 
            // col4
            // 
            this.col4.HeaderText = "System";
            this.col4.Name = "col4";
            this.col4.ReadOnly = true;
            // 
            // contextMenuGrid
            // 
            this.contextMenuGrid.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findInHistoryPanelToolStripMenuItem,
            this.findInJournalToolStripMenuItem,
            this.copyMeritsToolStripMenuItem,
            this.copyMeritsReportToolStripMenuItem,
            this.copySystemNameToolStripMenuItem});
            this.contextMenuGrid.Name = "contextMenuGrid";
            this.contextMenuGrid.Size = new System.Drawing.Size(184, 136);
            this.contextMenuGrid.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuGrid_Opening);
            // 
            // findInJournalToolStripMenuItem
            // 
            this.findInJournalToolStripMenuItem.Name = "findInJournalToolStripMenuItem";
            this.findInJournalToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.findInJournalToolStripMenuItem.Text = "Find in Journal Panel";
            this.findInJournalToolStripMenuItem.Click += new System.EventHandler(this.findInJournalToolStripMenuItem_Click);
            // 
            // copyMeritsToolStripMenuItem
            // 
            this.copyMeritsToolStripMenuItem.Name = "copyMeritsToolStripMenuItem";
            this.copyMeritsToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.copyMeritsToolStripMenuItem.Text = "Copy Merits";
            this.copyMeritsToolStripMenuItem.Click += new System.EventHandler(this.copyMeritsToolStripMenuItem_Click);
            // 
            // copyMeritsReportToolStripMenuItem
            // 
            this.copyMeritsReportToolStripMenuItem.Name = "copyMeritsReportToolStripMenuItem";
            this.copyMeritsReportToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.copyMeritsReportToolStripMenuItem.Text = "Copy Merits Report";
            this.copyMeritsReportToolStripMenuItem.Click += new System.EventHandler(this.copyMeritsReportToolStripMenuItem_Click);
            // 
            // copySystemNameToolStripMenuItem
            // 
            this.copySystemNameToolStripMenuItem.Name = "copySystemNameToolStripMenuItem";
            this.copySystemNameToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.copySystemNameToolStripMenuItem.Text = "Copy System Name";
            this.copySystemNameToolStripMenuItem.Click += new System.EventHandler(this.copySystemNameToolStripMenuItem_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.checkSystem);
            this.panelTop.Controls.Add(this.checkSession);
            this.panelTop.Controls.Add(this.checkCycle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 32);
            this.panelTop.TabIndex = 5;
            // 
            // extPanelDataGridViewScroll
            // 
            this.extPanelDataGridViewScroll.Controls.Add(this.grid);
            this.extPanelDataGridViewScroll.Controls.Add(this.extScrollBarDGV);
            this.extPanelDataGridViewScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelDataGridViewScroll.InternalMargin = new System.Windows.Forms.Padding(0);
            this.extPanelDataGridViewScroll.Location = new System.Drawing.Point(0, 32);
            this.extPanelDataGridViewScroll.Name = "extPanelDataGridViewScroll";
            this.extPanelDataGridViewScroll.ScrollBarWidth = 24;
            this.extPanelDataGridViewScroll.Size = new System.Drawing.Size(800, 468);
            this.extPanelDataGridViewScroll.TabIndex = 6;
            this.extPanelDataGridViewScroll.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarDGV
            // 
            this.extScrollBarDGV.AlwaysHideScrollBar = false;
            this.extScrollBarDGV.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarDGV.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarDGV.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.extScrollBarDGV.ArrowDownDrawAngle = 270F;
            this.extScrollBarDGV.ArrowUpDrawAngle = 90F;
            this.extScrollBarDGV.BorderColor = System.Drawing.Color.White;
            this.extScrollBarDGV.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarDGV.HideScrollBar = false;
            this.extScrollBarDGV.LargeChange = 0;
            this.extScrollBarDGV.Location = new System.Drawing.Point(776, 0);
            this.extScrollBarDGV.Maximum = -1;
            this.extScrollBarDGV.Minimum = 0;
            this.extScrollBarDGV.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarDGV.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.extScrollBarDGV.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarDGV.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.extScrollBarDGV.Name = "extScrollBarDGV";
            this.extScrollBarDGV.Size = new System.Drawing.Size(24, 468);
            this.extScrollBarDGV.SkinnyStyle = false;
            this.extScrollBarDGV.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarDGV.SliderColor2 = System.Drawing.Color.DarkGray;
            this.extScrollBarDGV.SliderDrawAngle = 90F;
            this.extScrollBarDGV.SmallChange = 1;
            this.extScrollBarDGV.TabIndex = 5;
            this.extScrollBarDGV.Text = "extScrollBar1";
            this.extScrollBarDGV.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarDGV.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarDGV.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.extScrollBarDGV.ThumbDrawAngle = 0F;
            this.extScrollBarDGV.Value = -1;
            this.extScrollBarDGV.ValueLimited = -1;
            // 
            // findInHistoryPanelToolStripMenuItem
            // 
            this.findInHistoryPanelToolStripMenuItem.Name = "findInHistoryPanelToolStripMenuItem";
            this.findInHistoryPanelToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.findInHistoryPanelToolStripMenuItem.Text = "Find in History Panel";
            this.findInHistoryPanelToolStripMenuItem.Click += new System.EventHandler(this.findInHistoryPanelToolStripMenuItem_Click);
            // 
            // UserControlPPMerits
            // 
            this.Controls.Add(this.extPanelDataGridViewScroll);
            this.Controls.Add(this.panelTop);
            this.Name = "UserControlPPMerits";
            this.Size = new System.Drawing.Size(800, 500);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.contextMenuGrid.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.extPanelDataGridViewScroll.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private ExtCheckBox checkCycle;
        private ExtCheckBox checkSession;
        private ExtCheckBox checkSystem;
        private DataGridViewColumnControl grid;
        private ContextMenuStrip contextMenuGrid;
        private ToolStripMenuItem findInJournalToolStripMenuItem;
        private ToolStripMenuItem copyMeritsToolStripMenuItem;
        private ToolStripMenuItem copyMeritsReportToolStripMenuItem;
        private ToolStripMenuItem copySystemNameToolStripMenuItem;
        private DataGridViewTextBoxColumn col0;
        private DataGridViewTextBoxColumn col1;
        private DataGridViewTextBoxColumn col2;
        private DataGridViewTextBoxColumn col3;
        private DataGridViewTextBoxColumn col4;
        private Panel panelTop;
        private ExtPanelDataGridViewScroll extPanelDataGridViewScroll;
        private ExtScrollBar extScrollBarDGV;
        private ToolStripMenuItem findInHistoryPanelToolStripMenuItem;
    }
}
