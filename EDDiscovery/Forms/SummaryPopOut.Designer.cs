namespace EDDiscovery2
{
    partial class SummaryPopOut
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SummaryPopOut));
            this.contextMenuStripConfig = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemTargetLine = new System.Windows.Forms.ToolStripMenuItem();
            this.EDSMButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTime = new System.Windows.Forms.ToolStripMenuItem();
            this.showDistanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showNotesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showXYZToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showTargetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripComboBoxOrder = new System.Windows.Forms.ToolStripComboBox();
            this.labelExt_NoSystems = new ExtendedControls.LabelExt();
            this.panel_grip = new ExtendedControls.DrawnPanel();
            this.blackBoxAroundTextToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripConfig
            // 
            this.contextMenuStripConfig.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemTargetLine,
            this.EDSMButtonToolStripMenuItem,
            this.toolStripMenuItemTime,
            this.showDistanceToolStripMenuItem,
            this.showNotesToolStripMenuItem,
            this.showXYZToolStripMenuItem,
            this.showTargetToolStripMenuItem,
            this.blackBoxAroundTextToolStripMenuItem,
            this.toolStripComboBoxOrder});
            this.contextMenuStripConfig.Name = "contextMenuStripConfig";
            this.contextMenuStripConfig.Size = new System.Drawing.Size(261, 229);
            // 
            // toolStripMenuItemTargetLine
            // 
            this.toolStripMenuItemTargetLine.Checked = true;
            this.toolStripMenuItemTargetLine.CheckOnClick = true;
            this.toolStripMenuItemTargetLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemTargetLine.Name = "toolStripMenuItemTargetLine";
            this.toolStripMenuItemTargetLine.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemTargetLine.Text = "Show Target Line";
            this.toolStripMenuItemTargetLine.Click += new System.EventHandler(this.targetLinetoolStripMenuItem_Click);
            // 
            // EDSMButtonToolStripMenuItem
            // 
            this.EDSMButtonToolStripMenuItem.Checked = true;
            this.EDSMButtonToolStripMenuItem.CheckOnClick = true;
            this.EDSMButtonToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.EDSMButtonToolStripMenuItem.Name = "EDSMButtonToolStripMenuItem";
            this.EDSMButtonToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.EDSMButtonToolStripMenuItem.Text = "EDSM Button";
            this.EDSMButtonToolStripMenuItem.Click += new System.EventHandler(this.EDSMButtonToolStripMenuItem_Click);
            // 
            // toolStripMenuItemTime
            // 
            this.toolStripMenuItemTime.Checked = true;
            this.toolStripMenuItemTime.CheckOnClick = true;
            this.toolStripMenuItemTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemTime.Name = "toolStripMenuItemTime";
            this.toolStripMenuItemTime.Size = new System.Drawing.Size(260, 22);
            this.toolStripMenuItemTime.Text = "Show Time";
            this.toolStripMenuItemTime.Click += new System.EventHandler(this.toolStripMenuItemTime_Click);
            // 
            // showDistanceToolStripMenuItem
            // 
            this.showDistanceToolStripMenuItem.Checked = true;
            this.showDistanceToolStripMenuItem.CheckOnClick = true;
            this.showDistanceToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showDistanceToolStripMenuItem.Name = "showDistanceToolStripMenuItem";
            this.showDistanceToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.showDistanceToolStripMenuItem.Text = "Show Distance";
            this.showDistanceToolStripMenuItem.Click += new System.EventHandler(this.showDistanceToolStripMenuItem_Click);
            // 
            // showNotesToolStripMenuItem
            // 
            this.showNotesToolStripMenuItem.Checked = true;
            this.showNotesToolStripMenuItem.CheckOnClick = true;
            this.showNotesToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showNotesToolStripMenuItem.Name = "showNotesToolStripMenuItem";
            this.showNotesToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.showNotesToolStripMenuItem.Text = "Show Notes";
            this.showNotesToolStripMenuItem.Click += new System.EventHandler(this.showNotesToolStripMenuItem_Click);
            // 
            // showXYZToolStripMenuItem
            // 
            this.showXYZToolStripMenuItem.Checked = true;
            this.showXYZToolStripMenuItem.CheckOnClick = true;
            this.showXYZToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showXYZToolStripMenuItem.Name = "showXYZToolStripMenuItem";
            this.showXYZToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.showXYZToolStripMenuItem.Text = "Show XYZ";
            this.showXYZToolStripMenuItem.Click += new System.EventHandler(this.showXYZToolStripMenuItem_Click);
            // 
            // showTargetToolStripMenuItem
            // 
            this.showTargetToolStripMenuItem.Checked = true;
            this.showTargetToolStripMenuItem.CheckOnClick = true;
            this.showTargetToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showTargetToolStripMenuItem.Name = "showTargetToolStripMenuItem";
            this.showTargetToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.showTargetToolStripMenuItem.Text = "Show Target Distance per Star";
            this.showTargetToolStripMenuItem.Click += new System.EventHandler(this.showTargetPerStarToolStripMenuItem_Click);
            // 
            // toolStripComboBoxOrder
            // 
            this.toolStripComboBoxOrder.Items.AddRange(new object[] {
            "Order: Default",
            "Order: Notes after XYZ",
            "Order: Target Dist,XYZ,Notes"});
            this.toolStripComboBoxOrder.Name = "toolStripComboBoxOrder";
            this.toolStripComboBoxOrder.Size = new System.Drawing.Size(200, 23);
            this.toolStripComboBoxOrder.SelectedIndexChanged += new System.EventHandler(this.toolStripComboBoxOrder_SelectedIndexChanged);
            // 
            // labelExt_NoSystems
            // 
            this.labelExt_NoSystems.AutoSize = true;
            this.labelExt_NoSystems.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelExt_NoSystems.ForeColor = System.Drawing.Color.White;
            this.labelExt_NoSystems.Location = new System.Drawing.Point(4, 32);
            this.labelExt_NoSystems.Name = "labelExt_NoSystems";
            this.labelExt_NoSystems.Size = new System.Drawing.Size(153, 20);
            this.labelExt_NoSystems.TabIndex = 18;
            this.labelExt_NoSystems.Text = "No Systems to show";
            // 
            // panel_grip
            // 
            this.panel_grip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_grip.BackColor = System.Drawing.Color.Transparent;
            this.panel_grip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_grip.Image = ExtendedControls.DrawnPanel.ImageType.Gripper;
            this.panel_grip.ImageText = null;
            this.panel_grip.Location = new System.Drawing.Point(580, 236);
            this.panel_grip.MarginSize = 6;
            this.panel_grip.MouseOverColor = System.Drawing.Color.Black;
            this.panel_grip.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_grip.Name = "panel_grip";
            this.panel_grip.Size = new System.Drawing.Size(20, 20);
            this.panel_grip.TabIndex = 17;
            this.panel_grip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_grip_MouseDown);
            // 
            // blackBoxAroundTextToolStripMenuItem
            // 
            this.blackBoxAroundTextToolStripMenuItem.Checked = true;
            this.blackBoxAroundTextToolStripMenuItem.CheckOnClick = true;
            this.blackBoxAroundTextToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.blackBoxAroundTextToolStripMenuItem.Name = "blackBoxAroundTextToolStripMenuItem";
            this.blackBoxAroundTextToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.blackBoxAroundTextToolStripMenuItem.Text = "Black box around text";
            this.blackBoxAroundTextToolStripMenuItem.Click += new System.EventHandler(this.blackBoxAroundTextToolStripMenuItem_Click);
            // 
            // SummaryPopOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(600, 268);
            this.Controls.Add(this.labelExt_NoSystems);
            this.Controls.Add(this.panel_grip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SummaryPopOut";
            this.Text = "SummaryPopOut";
            this.TransparencyKey = System.Drawing.Color.Transparent;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SummaryPopOut_FormClosing);
            this.Load += new System.EventHandler(this.SummaryPopOut_Load);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.SummaryPopOut_Layout);
            this.contextMenuStripConfig.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.DrawnPanel panel_grip;
        private ExtendedControls.LabelExt labelExt_NoSystems;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripConfig;
        private System.Windows.Forms.ToolStripMenuItem EDSMButtonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showTargetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showNotesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showXYZToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDistanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxOrder;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTargetLine;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTime;
        private System.Windows.Forms.ToolStripMenuItem blackBoxAroundTextToolStripMenuItem;
    }
}