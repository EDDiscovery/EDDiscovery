using System.Windows.Forms;
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
            this.checkCycle = new CheckBox();
            this.checkSession = new CheckBox();
            this.checkSystem = new CheckBox();
            this.grid = new DataGridView();
            this.contextMenuGrid = new ContextMenuStrip(this.components);
            this.findInJournalToolStripMenuItem = new ToolStripMenuItem();
            this.copyMeritsToolStripMenuItem = new ToolStripMenuItem();
            this.copyMeritsReportToolStripMenuItem = new ToolStripMenuItem();
            this.copySystemNameToolStripMenuItem = new ToolStripMenuItem();

            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.contextMenuGrid.SuspendLayout();
            this.SuspendLayout();

            // checkCycle
            this.checkCycle.Text = "Cycle";
            this.checkCycle.AutoSize = true;
            this.checkCycle.Checked = true;
            this.checkCycle.Location = new System.Drawing.Point(3, 6);

            // checkSession
            this.checkSession.Text = "Session";
            this.checkSession.AutoSize = true;
            this.checkSession.Checked = true;
            this.checkSession.Location = new System.Drawing.Point(70, 6);

            // checkSystem
            this.checkSystem.Text = "System";
            this.checkSystem.AutoSize = true;
            this.checkSystem.Checked = true;
            this.checkSystem.Location = new System.Drawing.Point(150, 6);

            // grid
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToOrderColumns = true;
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.grid.Location = new System.Drawing.Point(3, 32);
            this.grid.Size = new System.Drawing.Size(794, 465);
            this.grid.ContextMenuStrip = this.contextMenuGrid;
            this.grid.MouseDown += new MouseEventHandler(this.grid_MouseDown);

            var col0 = new DataGridViewTextBoxColumn(); col0.HeaderText = "Time"; col0.Width = 160;
            var col1 = new DataGridViewTextBoxColumn(); col1.HeaderText = "Power"; col1.Width = 140;
            var col2 = new DataGridViewTextBoxColumn(); col2.HeaderText = "Merits"; col2.Width = 80;
            var col3 = new DataGridViewTextBoxColumn(); col3.HeaderText = "Total"; col3.Width = 100;
            var col4 = new DataGridViewTextBoxColumn(); col4.HeaderText = "System"; col4.Width = 160;

            this.grid.Columns.AddRange(new DataGridViewColumn[] { col0, col1, col2, col3, col4 });

            // contextMenuGrid
            this.contextMenuGrid.Items.AddRange(new ToolStripItem[] { 
                this.findInJournalToolStripMenuItem,
                this.copyMeritsToolStripMenuItem,
                this.copyMeritsReportToolStripMenuItem,
                this.copySystemNameToolStripMenuItem
            });
            this.contextMenuGrid.Name = "contextMenuGrid";
            this.contextMenuGrid.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuGrid_Opening);

            // findInJournalToolStripMenuItem
            this.findInJournalToolStripMenuItem.Name = "findInJournalToolStripMenuItem";
            this.findInJournalToolStripMenuItem.Text = "Find in Journal";
            this.findInJournalToolStripMenuItem.Click += new System.EventHandler(this.findInJournalToolStripMenuItem_Click);

            // copyMeritsToolStripMenuItem
            this.copyMeritsToolStripMenuItem.Name = "copyMeritsToolStripMenuItem";
            this.copyMeritsToolStripMenuItem.Text = "Copy Merits";
            this.copyMeritsToolStripMenuItem.Click += new System.EventHandler(this.copyMeritsToolStripMenuItem_Click);

            // copyMeritsReportToolStripMenuItem
            this.copyMeritsReportToolStripMenuItem.Name = "copyMeritsReportToolStripMenuItem";
            this.copyMeritsReportToolStripMenuItem.Text = "Copy Merits Report";
            this.copyMeritsReportToolStripMenuItem.Click += new System.EventHandler(this.copyMeritsReportToolStripMenuItem_Click);

            // copySystemNameToolStripMenuItem
            this.copySystemNameToolStripMenuItem.Name = "copySystemNameToolStripMenuItem";
            this.copySystemNameToolStripMenuItem.Text = "Copy System Name";
            this.copySystemNameToolStripMenuItem.Click += new System.EventHandler(this.copySystemNameToolStripMenuItem_Click);

            // UserControlPPMerits
            this.Controls.Add(this.checkCycle);
            this.Controls.Add(this.checkSession);
            this.Controls.Add(this.checkSystem);
            this.Controls.Add(this.grid);
            this.Name = "UserControlPPMerits";
            this.Size = new System.Drawing.Size(800, 500);

            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.contextMenuGrid.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private CheckBox checkCycle;
        private CheckBox checkSession;
        private CheckBox checkSystem;
        private DataGridView grid;
        private ContextMenuStrip contextMenuGrid;
        private ToolStripMenuItem findInJournalToolStripMenuItem;
        private ToolStripMenuItem copyMeritsToolStripMenuItem;
        private ToolStripMenuItem copyMeritsReportToolStripMenuItem;
        private ToolStripMenuItem copySystemNameToolStripMenuItem;
    }
}
