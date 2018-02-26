namespace EDDiscovery.UserControls
{
    partial class UserControlContainerSplit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlContainerSplit));
            this.splitContainerCustom = new ExtendedControls.SplitContainerCustom();
            this.tabStrip1 = new ExtendedControls.TabStrip();
            this.tabStrip2 = new ExtendedControls.TabStrip();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.orientationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verticalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.horizontalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCustom)).BeginInit();
            this.splitContainerCustom.Panel1.SuspendLayout();
            this.splitContainerCustom.Panel2.SuspendLayout();
            this.splitContainerCustom.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerCustom
            // 
            this.splitContainerCustom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCustom.Location = new System.Drawing.Point(0, 0);
            this.splitContainerCustom.Name = "splitContainerCustom";
            // 
            // splitContainerCustom.Panel1
            // 
            this.splitContainerCustom.Panel1.Controls.Add(this.tabStrip1);
            // 
            // splitContainerCustom.Panel2
            // 
            this.splitContainerCustom.Panel2.Controls.Add(this.tabStrip2);
            this.splitContainerCustom.Size = new System.Drawing.Size(600, 300);
            this.splitContainerCustom.SplitterDistance = 330;
            this.splitContainerCustom.SplitterIncrement = 5;
            this.splitContainerCustom.TabIndex = 0;
            this.splitContainerCustom.MouseClick += new System.Windows.Forms.MouseEventHandler(this.splitContainerCustom_MouseClick);
            // 
            // tabStrip1
            // 
            this.tabStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStrip1.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.tabStrip1.DropDownBorderColor = System.Drawing.Color.Green;
            this.tabStrip1.DropDownHeight = 200;
            this.tabStrip1.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.tabStrip1.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.tabStrip1.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.tabStrip1.DropDownWidth = 400;
            this.tabStrip1.EmptyPanelIcon = ((System.Drawing.Image)(resources.GetObject("tabStrip1.EmptyPanelIcon")));
            this.tabStrip1.Location = new System.Drawing.Point(0, 0);
            this.tabStrip1.Name = "tabStrip1";
            this.tabStrip1.SelectedIndex = -1;
            this.tabStrip1.ShowPopOut = true;
            this.tabStrip1.Size = new System.Drawing.Size(330, 300);
            this.tabStrip1.StripMode = ExtendedControls.TabStrip.StripModeType.ListSelection;
            this.tabStrip1.TabFieldSpacing = 8;
            this.tabStrip1.TabIndex = 0;
            // 
            // tabStrip2
            // 
            this.tabStrip2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStrip2.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.tabStrip2.DropDownBorderColor = System.Drawing.Color.Green;
            this.tabStrip2.DropDownHeight = 200;
            this.tabStrip2.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.tabStrip2.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.tabStrip2.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.tabStrip2.DropDownWidth = 400;
            this.tabStrip2.EmptyPanelIcon = ((System.Drawing.Image)(resources.GetObject("tabStrip2.EmptyPanelIcon")));
            this.tabStrip2.Location = new System.Drawing.Point(0, 0);
            this.tabStrip2.Name = "tabStrip2";
            this.tabStrip2.SelectedIndex = -1;
            this.tabStrip2.ShowPopOut = true;
            this.tabStrip2.Size = new System.Drawing.Size(266, 300);
            this.tabStrip2.StripMode = ExtendedControls.TabStrip.StripModeType.ListSelection;
            this.tabStrip2.TabFieldSpacing = 8;
            this.tabStrip2.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.orientationToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(153, 48);
            // 
            // orientationToolStripMenuItem
            // 
            this.orientationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verticalToolStripMenuItem,
            this.horizontalToolStripMenuItem});
            this.orientationToolStripMenuItem.Name = "orientationToolStripMenuItem";
            this.orientationToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.orientationToolStripMenuItem.Text = "Orientation";
            // 
            // verticalToolStripMenuItem
            // 
            this.verticalToolStripMenuItem.Checked = true;
            this.verticalToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.verticalToolStripMenuItem.Name = "verticalToolStripMenuItem";
            this.verticalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.verticalToolStripMenuItem.Text = "Vertical";
            this.verticalToolStripMenuItem.Click += new System.EventHandler(this.verticalToolStripMenuItem_Click);
            // 
            // horizontalToolStripMenuItem
            // 
            this.horizontalToolStripMenuItem.Name = "horizontalToolStripMenuItem";
            this.horizontalToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.horizontalToolStripMenuItem.Text = "Horizontal";
            this.horizontalToolStripMenuItem.Click += new System.EventHandler(this.horizontalToolStripMenuItem_Click);
            // 
            // UserControlContainerSplit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainerCustom);
            this.Name = "UserControlContainerSplit";
            this.Size = new System.Drawing.Size(600, 300);
            this.splitContainerCustom.Panel1.ResumeLayout(false);
            this.splitContainerCustom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerCustom)).EndInit();
            this.splitContainerCustom.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.SplitContainerCustom splitContainerCustom;
        private ExtendedControls.TabStrip tabStrip1;
        private ExtendedControls.TabStrip tabStrip2;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem orientationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem verticalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem horizontalToolStripMenuItem;
    }
}
