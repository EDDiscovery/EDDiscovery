namespace EDDiscovery.UserControls
{
    partial class UserControlContainerSplitter
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
            this.panelPlayfield = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStripSplitter = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripOrientation = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitPanel1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMergePanel1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitPanel2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMergePanel2 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPlayfield
            // 
            this.panelPlayfield.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPlayfield.Location = new System.Drawing.Point(0, 0);
            this.panelPlayfield.Name = "panelPlayfield";
            this.panelPlayfield.Size = new System.Drawing.Size(912, 644);
            this.panelPlayfield.TabIndex = 2;
            // 
            // toolTip
            // 
            this.toolTip.AutomaticDelay = 250;
            this.toolTip.AutoPopDelay = 5000;
            this.toolTip.InitialDelay = 250;
            this.toolTip.ReshowDelay = 50;
            this.toolTip.ShowAlways = true;
            // 
            // contextMenuStripSplitter
            // 
            this.contextMenuStripSplitter.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripOrientation,
            this.toolStripSplitPanel1,
            this.toolStripMergePanel1,
            this.toolStripSplitPanel2,
            this.toolStripMergePanel2});
            this.contextMenuStripSplitter.Name = "contextMenuStripSplitter";
            this.contextMenuStripSplitter.Size = new System.Drawing.Size(179, 114);
            // 
            // toolStripOrientation
            // 
            this.toolStripOrientation.Name = "toolStripOrientation";
            this.toolStripOrientation.Size = new System.Drawing.Size(178, 22);
            this.toolStripOrientation.Text = "Change Orientation";
            this.toolStripOrientation.Click += new System.EventHandler(this.toolStripOrientation_Click);
            // 
            // toolStripSplitPanel1
            // 
            this.toolStripSplitPanel1.Name = "toolStripSplitPanel1";
            this.toolStripSplitPanel1.Size = new System.Drawing.Size(178, 22);
            this.toolStripSplitPanel1.Text = "Split Panel1";
            this.toolStripSplitPanel1.Click += new System.EventHandler(this.toolStripSplitPanel1_Click);
            // 
            // toolStripMergePanel1
            // 
            this.toolStripMergePanel1.Name = "toolStripMergePanel1";
            this.toolStripMergePanel1.Size = new System.Drawing.Size(178, 22);
            this.toolStripMergePanel1.Text = "Merge Panel 1";
            this.toolStripMergePanel1.Click += new System.EventHandler(this.toolStripMergePanel1_Click);
            // 
            // toolStripSplitPanel2
            // 
            this.toolStripSplitPanel2.Name = "toolStripSplitPanel2";
            this.toolStripSplitPanel2.Size = new System.Drawing.Size(178, 22);
            this.toolStripSplitPanel2.Text = "Split Panel 2";
            this.toolStripSplitPanel2.Click += new System.EventHandler(this.toolStripSplitPanel2_Click);
            // 
            // toolStripMergePanel2
            // 
            this.toolStripMergePanel2.Name = "toolStripMergePanel2";
            this.toolStripMergePanel2.Size = new System.Drawing.Size(178, 22);
            this.toolStripMergePanel2.Text = "Merge Panel 2";
            this.toolStripMergePanel2.Click += new System.EventHandler(this.toolStripMergePanel2_Click);
            // 
            // UserControlContainerSplitter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelPlayfield);
            this.Name = "UserControlContainerSplitter";
            this.Size = new System.Drawing.Size(912, 644);
            this.contextMenuStripSplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelPlayfield;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSplitter;
        private System.Windows.Forms.ToolStripMenuItem toolStripOrientation;
        private System.Windows.Forms.ToolStripMenuItem toolStripSplitPanel1;
        private System.Windows.Forms.ToolStripMenuItem toolStripSplitPanel2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMergePanel1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMergePanel2;
    }
}
