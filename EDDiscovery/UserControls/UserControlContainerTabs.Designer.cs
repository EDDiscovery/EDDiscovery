namespace EDDiscovery.UserControls
{
    partial class UserControlContainerTabs
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
            ExtendedControls.TabStyleSquare tabStyleSquare1 = new ExtendedControls.TabStyleSquare();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlContainerTabs));
            this.tabControlMain = new ExtendedControls.TabControlCustom();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabStrip1 = new ExtendedControls.TabStrip();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabStrip2 = new ExtendedControls.TabStrip();
            this.contextMenuStripTabs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControlMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStripTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlMain
            // 
            this.tabControlMain.AllowDragReorder = false;
            this.tabControlMain.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControlMain.Controls.Add(this.tabPage1);
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tabControlMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.tabControlMain.HotTrack = true;
            this.tabControlMain.Location = new System.Drawing.Point(0, 0);
            this.tabControlMain.Multiline = true;
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(341, 345);
            this.tabControlMain.TabColorScaling = 0.5F;
            this.tabControlMain.TabControlBorderBrightColor = System.Drawing.Color.LightGray;
            this.tabControlMain.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.tabControlMain.TabDisabledScaling = 0.5F;
            this.tabControlMain.TabIndex = 0;
            this.tabControlMain.TabMouseOverColor = System.Drawing.Color.White;
            this.tabControlMain.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.tabControlMain.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.tabControlMain.TabOpaque = 100F;
            this.tabControlMain.TabSelectedColor = System.Drawing.Color.LightGray;
            this.tabControlMain.TabStyle = tabStyleSquare1;
            this.tabControlMain.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.tabControlMain.TextSelectedColor = System.Drawing.SystemColors.ControlText;
            this.tabControlMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControlMain_MouseClick);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tabStrip1);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(333, 313);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.tabStrip1.Location = new System.Drawing.Point(3, 3);
            this.tabStrip1.Name = "tabStrip1";
            this.tabStrip1.SelectedIndex = -1;
            this.tabStrip1.ShowPopOut = true;
            this.tabStrip1.Size = new System.Drawing.Size(327, 307);
            this.tabStrip1.StripMode = ExtendedControls.TabStrip.StripModeType.ListSelection;
            this.tabStrip1.TabFieldSpacing = 8;
            this.tabStrip1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tabStrip2);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(333, 313);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
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
            this.tabStrip2.Location = new System.Drawing.Point(3, 3);
            this.tabStrip2.Name = "tabStrip2";
            this.tabStrip2.SelectedIndex = -1;
            this.tabStrip2.ShowPopOut = true;
            this.tabStrip2.Size = new System.Drawing.Size(327, 307);
            this.tabStrip2.StripMode = ExtendedControls.TabStrip.StripModeType.ListSelection;
            this.tabStrip2.TabFieldSpacing = 8;
            this.tabStrip2.TabIndex = 0;
            // 
            // contextMenuStripTabs
            // 
            this.contextMenuStripTabs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTabToolStripMenuItem,
            this.removeTabToolStripMenuItem,
            this.renameTabToolStripMenuItem});
            this.contextMenuStripTabs.Name = "contextMenuStripTabs";
            this.contextMenuStripTabs.Size = new System.Drawing.Size(190, 92);
            this.contextMenuStripTabs.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStripTabs_Opening);
            // 
            // addTabToolStripMenuItem
            // 
            this.addTabToolStripMenuItem.Name = "addTabToolStripMenuItem";
            this.addTabToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.addTabToolStripMenuItem.Text = "Insert Tab with panel..";
            // 
            // removeTabToolStripMenuItem
            // 
            this.removeTabToolStripMenuItem.Name = "removeTabToolStripMenuItem";
            this.removeTabToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.removeTabToolStripMenuItem.Text = "Remove Tab";
            // 
            // renameTabToolStripMenuItem
            // 
            this.renameTabToolStripMenuItem.Name = "renameTabToolStripMenuItem";
            this.renameTabToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.renameTabToolStripMenuItem.Text = "Rename Tab";
            // 
            // UserControlContainerTabs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControlMain);
            this.Name = "UserControlContainerTabs";
            this.Size = new System.Drawing.Size(341, 345);
            this.tabControlMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.contextMenuStripTabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.TabControlCustom tabControlMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private ExtendedControls.TabStrip tabStrip1;
        private ExtendedControls.TabStrip tabStrip2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTabs;
        private System.Windows.Forms.ToolStripMenuItem addTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameTabToolStripMenuItem;
    }
}
