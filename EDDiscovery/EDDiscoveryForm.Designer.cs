namespace EDDiscovery
{
    partial class EDDiscoveryForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDiscoveryForm));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageTravelHistory = new System.Windows.Forms.TabPage();
            this.travelHistoryControl1 = new EDDiscovery.TravelHistoryControl();
            this.tabPageTriletaration = new System.Windows.Forms.TabPage();
            this.trilaterationControl = new EDDiscovery.TrilaterationControl();
            this.tabPageScreenshots = new System.Windows.Forms.TabPage();
            this.imageHandler1 = new EDDiscovery2.ImageHandler.ImageHandler();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.routeControl1 = new EDDiscovery.RouteControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxEDSMApiKey = new System.Windows.Forms.TextBox();
            this.button_Save = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button_Browse = new System.Windows.Forms.Button();
            this.textBoxNetLogDir = new System.Windows.Forms.TextBox();
            this.radioButton_Manual = new System.Windows.Forms.RadioButton();
            this.radioButton_Auto = new System.Windows.Forms.RadioButton();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewStarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openEliteDangerousDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.show2DMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eDDiscoveryHomepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontierForumThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelPanelText = new System.Windows.Forms.Label();
            this.setDefaultMapColourToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPageTravelHistory.SuspendLayout();
            this.tabPageTriletaration.SuspendLayout();
            this.tabPageScreenshots.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageTravelHistory);
            this.tabControl1.Controls.Add(this.tabPageTriletaration);
            this.tabControl1.Controls.Add(this.tabPageScreenshots);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(905, 593);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPageTravelHistory
            // 
            this.tabPageTravelHistory.Controls.Add(this.travelHistoryControl1);
            this.tabPageTravelHistory.Location = new System.Drawing.Point(4, 22);
            this.tabPageTravelHistory.Name = "tabPageTravelHistory";
            this.tabPageTravelHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTravelHistory.Size = new System.Drawing.Size(897, 567);
            this.tabPageTravelHistory.TabIndex = 0;
            this.tabPageTravelHistory.Text = "Travel history";
            this.tabPageTravelHistory.UseVisualStyleBackColor = true;
            // 
            // travelHistoryControl1
            // 
            this.travelHistoryControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.travelHistoryControl1.Location = new System.Drawing.Point(3, 3);
            this.travelHistoryControl1.Name = "travelHistoryControl1";
            this.travelHistoryControl1.Size = new System.Drawing.Size(891, 561);
            this.travelHistoryControl1.TabIndex = 0;
            this.travelHistoryControl1.Load += new System.EventHandler(this.travelHistoryControl1_Load);
            // 
            // tabPageTriletaration
            // 
            this.tabPageTriletaration.Controls.Add(this.trilaterationControl);
            this.tabPageTriletaration.Location = new System.Drawing.Point(4, 22);
            this.tabPageTriletaration.Name = "tabPageTriletaration";
            this.tabPageTriletaration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTriletaration.Size = new System.Drawing.Size(897, 567);
            this.tabPageTriletaration.TabIndex = 3;
            this.tabPageTriletaration.Text = "Trilateration";
            this.tabPageTriletaration.UseVisualStyleBackColor = true;
            // 
            // trilaterationControl
            // 
            this.trilaterationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trilaterationControl.Location = new System.Drawing.Point(3, 3);
            this.trilaterationControl.Name = "trilaterationControl";
            this.trilaterationControl.Size = new System.Drawing.Size(891, 561);
            this.trilaterationControl.TabIndex = 21;
            // 
            // tabPageScreenshots
            // 
            this.tabPageScreenshots.Controls.Add(this.imageHandler1);
            this.tabPageScreenshots.Location = new System.Drawing.Point(4, 22);
            this.tabPageScreenshots.Name = "tabPageScreenshots";
            this.tabPageScreenshots.Size = new System.Drawing.Size(897, 567);
            this.tabPageScreenshots.TabIndex = 4;
            this.tabPageScreenshots.Text = "Screenshots";
            this.tabPageScreenshots.UseVisualStyleBackColor = true;
            // 
            // imageHandler1
            // 
            this.imageHandler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageHandler1.Location = new System.Drawing.Point(0, 0);
            this.imageHandler1.Name = "imageHandler1";
            this.imageHandler1.Size = new System.Drawing.Size(897, 567);
            this.imageHandler1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.routeControl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(897, 567);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Route";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // routeControl1
            // 
            this.routeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.routeControl1.Location = new System.Drawing.Point(3, 3);
            this.routeControl1.Name = "routeControl1";
            this.routeControl1.Size = new System.Drawing.Size(891, 561);
            this.routeControl1.TabIndex = 0;
            this.routeControl1.Load += new System.EventHandler(this.routeControl1_Load);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.textBoxEDSMApiKey);
            this.tabPage3.Controls.Add(this.button_Save);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(897, 567);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 113);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "EDSM Api key";
            // 
            // textBoxEDSMApiKey
            // 
            this.textBoxEDSMApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEDSMApiKey.Location = new System.Drawing.Point(128, 110);
            this.textBoxEDSMApiKey.Name = "textBoxEDSMApiKey";
            this.textBoxEDSMApiKey.Size = new System.Drawing.Size(365, 20);
            this.textBoxEDSMApiKey.TabIndex = 5;
            // 
            // button_Save
            // 
            this.button_Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Save.Location = new System.Drawing.Point(819, 178);
            this.button_Save.Name = "button_Save";
            this.button_Save.Size = new System.Drawing.Size(75, 23);
            this.button_Save.TabIndex = 4;
            this.button_Save.Text = "Save";
            this.button_Save.UseVisualStyleBackColor = true;
            this.button_Save.Click += new System.EventHandler(this.button_Save_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button_Browse);
            this.groupBox1.Controls.Add(this.textBoxNetLogDir);
            this.groupBox1.Controls.Add(this.radioButton_Manual);
            this.groupBox1.Controls.Add(this.radioButton_Auto);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(891, 87);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Elite Dangerous netlog location";
            // 
            // button_Browse
            // 
            this.button_Browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button_Browse.Location = new System.Drawing.Point(814, 44);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(75, 23);
            this.button_Browse.TabIndex = 3;
            this.button_Browse.Text = "Browse";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button_Browse_Click);
            // 
            // textBoxNetLogDir
            // 
            this.textBoxNetLogDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNetLogDir.Location = new System.Drawing.Point(125, 46);
            this.textBoxNetLogDir.Name = "textBoxNetLogDir";
            this.textBoxNetLogDir.Size = new System.Drawing.Size(683, 20);
            this.textBoxNetLogDir.TabIndex = 2;
            // 
            // radioButton_Manual
            // 
            this.radioButton_Manual.AutoSize = true;
            this.radioButton_Manual.Location = new System.Drawing.Point(17, 49);
            this.radioButton_Manual.Name = "radioButton_Manual";
            this.radioButton_Manual.Size = new System.Drawing.Size(60, 17);
            this.radioButton_Manual.TabIndex = 1;
            this.radioButton_Manual.TabStop = true;
            this.radioButton_Manual.Text = "Manual";
            this.radioButton_Manual.UseVisualStyleBackColor = true;
            // 
            // radioButton_Auto
            // 
            this.radioButton_Auto.AutoSize = true;
            this.radioButton_Auto.Location = new System.Drawing.Point(17, 26);
            this.radioButton_Auto.Name = "radioButton_Auto";
            this.radioButton_Auto.Size = new System.Drawing.Size(47, 17);
            this.radioButton_Auto.TabIndex = 0;
            this.radioButton_Auto.TabStop = true;
            this.radioButton_Auto.Text = "Auto";
            this.radioButton_Auto.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(823, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(908, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewStarToolStripMenuItem,
            this.openEliteDangerousDirectoryToolStripMenuItem,
            this.showLogfilesToolStripMenuItem,
            this.show2DMapsToolStripMenuItem,
            this.statisticsToolStripMenuItem,
            this.setDefaultMapColourToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // addNewStarToolStripMenuItem
            // 
            this.addNewStarToolStripMenuItem.Name = "addNewStarToolStripMenuItem";
            this.addNewStarToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.addNewStarToolStripMenuItem.Text = "Add new star";
            this.addNewStarToolStripMenuItem.Click += new System.EventHandler(this.addNewStarToolStripMenuItem_Click);
            // 
            // openEliteDangerousDirectoryToolStripMenuItem
            // 
            this.openEliteDangerousDirectoryToolStripMenuItem.Name = "openEliteDangerousDirectoryToolStripMenuItem";
            this.openEliteDangerousDirectoryToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.openEliteDangerousDirectoryToolStripMenuItem.Text = "Open Elite Dangerous directory";
            this.openEliteDangerousDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openEliteDangerousDirectoryToolStripMenuItem_Click);
            // 
            // showLogfilesToolStripMenuItem
            // 
            this.showLogfilesToolStripMenuItem.Name = "showLogfilesToolStripMenuItem";
            this.showLogfilesToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.showLogfilesToolStripMenuItem.Text = "Show logfiles";
            this.showLogfilesToolStripMenuItem.Click += new System.EventHandler(this.showLogfilesToolStripMenuItem_Click);
            // 
            // show2DMapsToolStripMenuItem
            // 
            this.show2DMapsToolStripMenuItem.Name = "show2DMapsToolStripMenuItem";
            this.show2DMapsToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.show2DMapsToolStripMenuItem.Text = "Show 2D maps";
            this.show2DMapsToolStripMenuItem.Click += new System.EventHandler(this.show2DMapsToolStripMenuItem_Click);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.statisticsToolStripMenuItem.Text = "Statistics";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.eDDiscoveryHomepageToolStripMenuItem,
            this.frontierForumThreadToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // eDDiscoveryHomepageToolStripMenuItem
            // 
            this.eDDiscoveryHomepageToolStripMenuItem.Name = "eDDiscoveryHomepageToolStripMenuItem";
            this.eDDiscoveryHomepageToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.eDDiscoveryHomepageToolStripMenuItem.Text = "EDDiscovery homepage";
            this.eDDiscoveryHomepageToolStripMenuItem.Click += new System.EventHandler(this.eDDiscoveryHomepageToolStripMenuItem_Click);
            // 
            // frontierForumThreadToolStripMenuItem
            // 
            this.frontierForumThreadToolStripMenuItem.Name = "frontierForumThreadToolStripMenuItem";
            this.frontierForumThreadToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.frontierForumThreadToolStripMenuItem.Text = "Frontier forum thread";
            this.frontierForumThreadToolStripMenuItem.Click += new System.EventHandler(this.frontierForumThreadToolStripMenuItem_Click);
            // 
            // panelInfo
            // 
            this.panelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInfo.BackColor = System.Drawing.Color.Salmon;
            this.panelInfo.Controls.Add(this.labelPanelText);
            this.panelInfo.Location = new System.Drawing.Point(514, 1);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(394, 23);
            this.panelInfo.TabIndex = 17;
            // 
            // labelPanelText
            // 
            this.labelPanelText.AutoSize = true;
            this.labelPanelText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPanelText.Location = new System.Drawing.Point(3, 0);
            this.labelPanelText.Name = "labelPanelText";
            this.labelPanelText.Size = new System.Drawing.Size(158, 20);
            this.labelPanelText.TabIndex = 0;
            this.labelPanelText.Text = "Loading. Please wait!";
            this.labelPanelText.Click += new System.EventHandler(this.label1_Click);
            // 
            // setDefaultMapColourToolStripMenuItem
            // 
            this.setDefaultMapColourToolStripMenuItem.Name = "setDefaultMapColourToolStripMenuItem";
            this.setDefaultMapColourToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.setDefaultMapColourToolStripMenuItem.Text = "Set Default Map Colour";
            this.setDefaultMapColourToolStripMenuItem.Click += new System.EventHandler(this.setDefaultMapColourToolStripMenuItem_Click);
            // 
            // EDDiscoveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 626);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EDDiscoveryForm";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EDDiscoveryForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Shown += new System.EventHandler(this.EDDiscoveryForm_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPageTravelHistory.ResumeLayout(false);
            this.tabPageTriletaration.ResumeLayout(false);
            this.tabPageScreenshots.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageTravelHistory;
        private System.Windows.Forms.TabPage tabPage2;
        private TravelHistoryControl travelHistoryControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton_Manual;
        private System.Windows.Forms.RadioButton radioButton_Auto;
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.TextBox textBoxNetLogDir;
        private System.Windows.Forms.Button button_Save;
        private RouteControl routeControl1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewStarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eDDiscoveryHomepageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frontierForumThreadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openEliteDangerousDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label labelPanelText;
        private System.Windows.Forms.TabPage tabPageTriletaration;
        public TrilaterationControl trilaterationControl;
        private System.Windows.Forms.ToolStripMenuItem show2DMapsToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageScreenshots;
        private EDDiscovery2.ImageHandler.ImageHandler imageHandler1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxEDSMApiKey;
        private System.Windows.Forms.ToolStripMenuItem setDefaultMapColourToolStripMenuItem;
    }
}

