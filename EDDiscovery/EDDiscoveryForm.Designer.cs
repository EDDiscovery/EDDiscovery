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
            this.button_test = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openEliteDangerousDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.show2DMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.prospectingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statisticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceEDDBUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncEDSMSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.keepOnTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fastTravelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eDDiscoveryHomepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontierForumThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportIssueIdeasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eDDiscoveryChatDiscordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelPanelText = new System.Windows.Forms.Label();
            this.panel_eddiscovery = new System.Windows.Forms.Panel();
            this.panel_grip = new System.Windows.Forms.Panel();
            this.panel_minimize = new System.Windows.Forms.Panel();
            this.panel_close = new System.Windows.Forms.Panel();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.settings = new EDDiscovery2.Settings();
            this.tabPageRoute = new System.Windows.Forms.TabPage();
            this.routeControl1 = new EDDiscovery.RouteControl();
            this.tabPageScreenshots = new System.Windows.Forms.TabPage();
            this.imageHandler1 = new EDDiscovery2.ImageHandler.ImageHandler();
            this.tabPageTriletaration = new System.Windows.Forms.TabPage();
            this.trilaterationControl = new EDDiscovery.TrilaterationControl();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageTravelHistory = new System.Windows.Forms.TabPage();
            this.travelHistoryControl1 = new EDDiscovery.TravelHistoryControl();
            this.label_version = new System.Windows.Forms.Label();
            this.changeMapColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.tabPageRoute.SuspendLayout();
            this.tabPageScreenshots.SuspendLayout();
            this.tabPageTriletaration.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageTravelHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_test
            // 
            this.button_test.Location = new System.Drawing.Point(772, -1);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(41, 23);
            this.button_test.TabIndex = 1;
            this.button_test.Text = "Test";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Visible = false;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(308, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menuStrip1_MouseDown);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openEliteDangerousDirectoryToolStripMenuItem,
            this.showLogfilesToolStripMenuItem,
            this.show2DMapsToolStripMenuItem,
            this.prospectingToolStripMenuItem,
            this.statisticsToolStripMenuItem,
            this.changeMapColorToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
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
            // prospectingToolStripMenuItem
            // 
            this.prospectingToolStripMenuItem.Name = "prospectingToolStripMenuItem";
            this.prospectingToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.prospectingToolStripMenuItem.Text = "Prospecting";
            this.prospectingToolStripMenuItem.Click += new System.EventHandler(this.prospectingToolStripMenuItem_Click);
            // 
            // statisticsToolStripMenuItem
            // 
            this.statisticsToolStripMenuItem.Name = "statisticsToolStripMenuItem";
            this.statisticsToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.statisticsToolStripMenuItem.Text = "Statistics";
            this.statisticsToolStripMenuItem.Click += new System.EventHandler(this.statisticsToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forceEDDBUpdateToolStripMenuItem,
            this.syncEDSMSystemsToolStripMenuItem,
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem});
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.adminToolStripMenuItem.Text = "Admin";
            // 
            // forceEDDBUpdateToolStripMenuItem
            // 
            this.forceEDDBUpdateToolStripMenuItem.Name = "forceEDDBUpdateToolStripMenuItem";
            this.forceEDDBUpdateToolStripMenuItem.Size = new System.Drawing.Size(309, 22);
            this.forceEDDBUpdateToolStripMenuItem.Text = "Force EDDB update";
            this.forceEDDBUpdateToolStripMenuItem.Click += new System.EventHandler(this.forceEDDBUpdateToolStripMenuItem_Click);
            // 
            // syncEDSMSystemsToolStripMenuItem
            // 
            this.syncEDSMSystemsToolStripMenuItem.Name = "syncEDSMSystemsToolStripMenuItem";
            this.syncEDSMSystemsToolStripMenuItem.Size = new System.Drawing.Size(309, 22);
            this.syncEDSMSystemsToolStripMenuItem.Text = "Sync EDSM Systems";
            this.syncEDSMSystemsToolStripMenuItem.Click += new System.EventHandler(this.syncEDSMSystemsToolStripMenuItem_Click);
            // 
            // dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem
            // 
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Name = "dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem";
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Size = new System.Drawing.Size(309, 22);
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Text = "(DEBUG) Reset all history to first commander";
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Click += new System.EventHandler(this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.keepOnTopToolStripMenuItem,
            this.fastTravelToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // keepOnTopToolStripMenuItem
            // 
            this.keepOnTopToolStripMenuItem.Name = "keepOnTopToolStripMenuItem";
            this.keepOnTopToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.keepOnTopToolStripMenuItem.Text = "Keep on Top";
            this.keepOnTopToolStripMenuItem.Click += new System.EventHandler(this.keepOnTopToolStripMenuItem_Click);
            // 
            // fastTravelToolStripMenuItem
            // 
            this.fastTravelToolStripMenuItem.CheckOnClick = true;
            this.fastTravelToolStripMenuItem.Name = "fastTravelToolStripMenuItem";
            this.fastTravelToolStripMenuItem.Size = new System.Drawing.Size(141, 22);
            this.fastTravelToolStripMenuItem.Text = "Fast Travel";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.gitHubToolStripMenuItem,
            this.eDDiscoveryHomepageToolStripMenuItem,
            this.frontierForumThreadToolStripMenuItem,
            this.reportIssueIdeasToolStripMenuItem,
            this.eDDiscoveryChatDiscordToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // gitHubToolStripMenuItem
            // 
            this.gitHubToolStripMenuItem.Name = "gitHubToolStripMenuItem";
            this.gitHubToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.gitHubToolStripMenuItem.Text = "Github";
            this.gitHubToolStripMenuItem.Click += new System.EventHandler(this.gitHubToolStripMenuItem_Click);
            // 
            // eDDiscoveryHomepageToolStripMenuItem
            // 
            this.eDDiscoveryHomepageToolStripMenuItem.Name = "eDDiscoveryHomepageToolStripMenuItem";
            this.eDDiscoveryHomepageToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.eDDiscoveryHomepageToolStripMenuItem.Text = "EDDiscovery homepage";
            this.eDDiscoveryHomepageToolStripMenuItem.Click += new System.EventHandler(this.eDDiscoveryHomepageToolStripMenuItem_Click);
            // 
            // frontierForumThreadToolStripMenuItem
            // 
            this.frontierForumThreadToolStripMenuItem.Name = "frontierForumThreadToolStripMenuItem";
            this.frontierForumThreadToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.frontierForumThreadToolStripMenuItem.Text = "Frontier forum thread";
            this.frontierForumThreadToolStripMenuItem.Click += new System.EventHandler(this.frontierForumThreadToolStripMenuItem_Click);
            // 
            // reportIssueIdeasToolStripMenuItem
            // 
            this.reportIssueIdeasToolStripMenuItem.Name = "reportIssueIdeasToolStripMenuItem";
            this.reportIssueIdeasToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.reportIssueIdeasToolStripMenuItem.Text = "Report issue / ideas";
            this.reportIssueIdeasToolStripMenuItem.Click += new System.EventHandler(this.reportIssueIdeasToolStripMenuItem_Click);
            // 
            // eDDiscoveryChatDiscordToolStripMenuItem
            // 
            this.eDDiscoveryChatDiscordToolStripMenuItem.Name = "eDDiscoveryChatDiscordToolStripMenuItem";
            this.eDDiscoveryChatDiscordToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.eDDiscoveryChatDiscordToolStripMenuItem.Text = "EDDiscovery chat (Discord)";
            this.eDDiscoveryChatDiscordToolStripMenuItem.Click += new System.EventHandler(this.eDDiscoveryChatDiscordToolStripMenuItem_Click);
            // 
            // panelInfo
            // 
            this.panelInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelInfo.BackColor = System.Drawing.Color.Salmon;
            this.panelInfo.Controls.Add(this.labelPanelText);
            this.panelInfo.Location = new System.Drawing.Point(435, 1);
            this.panelInfo.Name = "panelInfo";
            this.panelInfo.Size = new System.Drawing.Size(331, 23);
            this.panelInfo.TabIndex = 17;
            // 
            // labelPanelText
            // 
            this.labelPanelText.AutoSize = true;
            this.labelPanelText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPanelText.Location = new System.Drawing.Point(3, -1);
            this.labelPanelText.Name = "labelPanelText";
            this.labelPanelText.Size = new System.Drawing.Size(158, 20);
            this.labelPanelText.TabIndex = 0;
            this.labelPanelText.Text = "Loading. Please wait!";
            // 
            // panel_eddiscovery
            // 
            this.panel_eddiscovery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_eddiscovery.BackColor = System.Drawing.SystemColors.Control;
            this.panel_eddiscovery.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_eddiscovery.BackgroundImage")));
            this.panel_eddiscovery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_eddiscovery.Location = new System.Drawing.Point(818, 1);
            this.panel_eddiscovery.Name = "panel_eddiscovery";
            this.panel_eddiscovery.Size = new System.Drawing.Size(101, 46);
            this.panel_eddiscovery.TabIndex = 18;
            this.panel_eddiscovery.Click += new System.EventHandler(this.panel1_Click);
            // 
            // panel_grip
            // 
            this.panel_grip.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panel_grip.BackColor = System.Drawing.SystemColors.Control;
            this.panel_grip.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_grip.BackgroundImage")));
            this.panel_grip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_grip.Location = new System.Drawing.Point(977, 727);
            this.panel_grip.Name = "panel_grip";
            this.panel_grip.Size = new System.Drawing.Size(16, 16);
            this.panel_grip.TabIndex = 16;
            this.panel_grip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel_grip_MouseDown);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.BackColor = System.Drawing.SystemColors.Control;
            this.panel_minimize.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_minimize.BackgroundImage")));
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.Location = new System.Drawing.Point(943, 1);
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Size = new System.Drawing.Size(22, 24);
            this.panel_minimize.TabIndex = 20;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel_close.BackgroundImage")));
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Location = new System.Drawing.Point(961, 1);
            this.panel_close.Name = "panel_close";
            this.panel_close.Size = new System.Drawing.Size(32, 24);
            this.panel_close.TabIndex = 19;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // tabPageSettings
            // 
            this.tabPageSettings.Controls.Add(this.settings);
            this.tabPageSettings.Location = new System.Drawing.Point(4, 22);
            this.tabPageSettings.Name = "tabPageSettings";
            this.tabPageSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSettings.Size = new System.Drawing.Size(985, 671);
            this.tabPageSettings.TabIndex = 5;
            this.tabPageSettings.Text = "Settings";
            this.tabPageSettings.UseVisualStyleBackColor = true;
            // 
            // settings
            // 
            this.settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settings.Location = new System.Drawing.Point(3, 3);
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(979, 665);
            this.settings.TabIndex = 0;
            // 
            // tabPageRoute
            // 
            this.tabPageRoute.Controls.Add(this.routeControl1);
            this.tabPageRoute.Location = new System.Drawing.Point(4, 22);
            this.tabPageRoute.Name = "tabPageRoute";
            this.tabPageRoute.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRoute.Size = new System.Drawing.Size(985, 671);
            this.tabPageRoute.TabIndex = 1;
            this.tabPageRoute.Text = "Route";
            this.tabPageRoute.UseVisualStyleBackColor = true;
            // 
            // routeControl1
            // 
            this.routeControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.routeControl1.Location = new System.Drawing.Point(3, 3);
            this.routeControl1.Name = "routeControl1";
            this.routeControl1.Size = new System.Drawing.Size(979, 665);
            this.routeControl1.TabIndex = 0;
            // 
            // tabPageScreenshots
            // 
            this.tabPageScreenshots.Controls.Add(this.imageHandler1);
            this.tabPageScreenshots.Location = new System.Drawing.Point(4, 22);
            this.tabPageScreenshots.Name = "tabPageScreenshots";
            this.tabPageScreenshots.Size = new System.Drawing.Size(985, 671);
            this.tabPageScreenshots.TabIndex = 4;
            this.tabPageScreenshots.Text = "Screenshots";
            this.tabPageScreenshots.UseVisualStyleBackColor = true;
            // 
            // imageHandler1
            // 
            this.imageHandler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageHandler1.Location = new System.Drawing.Point(0, 0);
            this.imageHandler1.Name = "imageHandler1";
            this.imageHandler1.Size = new System.Drawing.Size(985, 671);
            this.imageHandler1.TabIndex = 0;
            // 
            // tabPageTriletaration
            // 
            this.tabPageTriletaration.Controls.Add(this.trilaterationControl);
            this.tabPageTriletaration.Location = new System.Drawing.Point(4, 22);
            this.tabPageTriletaration.Name = "tabPageTriletaration";
            this.tabPageTriletaration.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTriletaration.Size = new System.Drawing.Size(985, 671);
            this.tabPageTriletaration.TabIndex = 3;
            this.tabPageTriletaration.Text = "Trilateration";
            this.tabPageTriletaration.UseVisualStyleBackColor = true;
            // 
            // trilaterationControl
            // 
            this.trilaterationControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trilaterationControl.Location = new System.Drawing.Point(3, 3);
            this.trilaterationControl.Name = "trilaterationControl";
            this.trilaterationControl.Size = new System.Drawing.Size(979, 665);
            this.trilaterationControl.TabIndex = 21;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageTravelHistory);
            this.tabControl1.Controls.Add(this.tabPageTriletaration);
            this.tabControl1.Controls.Add(this.tabPageScreenshots);
            this.tabControl1.Controls.Add(this.tabPageRoute);
            this.tabControl1.Controls.Add(this.tabPageSettings);
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(993, 697);
            this.tabControl1.TabIndex = 15;
            // 
            // tabPageTravelHistory
            // 
            this.tabPageTravelHistory.Controls.Add(this.travelHistoryControl1);
            this.tabPageTravelHistory.Location = new System.Drawing.Point(4, 22);
            this.tabPageTravelHistory.Name = "tabPageTravelHistory";
            this.tabPageTravelHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTravelHistory.Size = new System.Drawing.Size(985, 671);
            this.tabPageTravelHistory.TabIndex = 0;
            this.tabPageTravelHistory.Text = "Travel history";
            this.tabPageTravelHistory.UseVisualStyleBackColor = true;
            // 
            // travelHistoryControl1
            // 
            this.travelHistoryControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.travelHistoryControl1.Location = new System.Drawing.Point(3, 3);
            this.travelHistoryControl1.Name = "travelHistoryControl1";
            this.travelHistoryControl1.Size = new System.Drawing.Size(979, 665);
            this.travelHistoryControl1.TabIndex = 0;
            // 
            // label_version
            // 
            this.label_version.AutoSize = true;
            this.label_version.Location = new System.Drawing.Point(296, 3);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(71, 13);
            this.label_version.TabIndex = 21;
            this.label_version.Text = "Version Label";
            // 
            // changeMapColorToolStripMenuItem
            // 
            this.changeMapColorToolStripMenuItem.Name = "changeMapColorToolStripMenuItem";
            this.changeMapColorToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.changeMapColorToolStripMenuItem.Text = "Set Default Map Colour";
            this.changeMapColorToolStripMenuItem.Click += new System.EventHandler(this.changeMapColorToolStripMenuItem_Click);
            // 
            // EDDiscoveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(993, 744);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.panel_eddiscovery);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelInfo);
            this.Controls.Add(this.panel_grip);
            this.Controls.Add(this.button_test);
            this.Controls.Add(this.panel_minimize);
            this.Controls.Add(this.panel_close);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EDDiscoveryForm";
            this.Text = "EDDiscovery";
            this.Activated += new System.EventHandler(this.EDDiscoveryForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EDDiscoveryForm_FormClosing);
            this.Load += new System.EventHandler(this.EDDiscoveryForm_Load);
            this.Shown += new System.EventHandler(this.EDDiscoveryForm_Shown);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.EDDiscoveryForm_Layout);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EDDiscoveryForm_MouseDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.tabPageSettings.ResumeLayout(false);
            this.tabPageRoute.ResumeLayout(false);
            this.tabPageScreenshots.ResumeLayout(false);
            this.tabPageTriletaration.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPageTravelHistory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button button_test;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eDDiscoveryHomepageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frontierForumThreadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openEliteDangerousDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem statisticsToolStripMenuItem;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label labelPanelText;
        private System.Windows.Forms.ToolStripMenuItem show2DMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceEDDBUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem prospectingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncEDSMSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gitHubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportIssueIdeasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem keepOnTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fastTravelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem;
        private System.Windows.Forms.Panel panel_eddiscovery;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eDDiscoveryChatDiscordToolStripMenuItem;
        private System.Windows.Forms.Panel panel_close;
        private System.Windows.Forms.Panel panel_minimize;
        private System.Windows.Forms.Panel panel_grip;
        private System.Windows.Forms.TabPage tabPageSettings;
        private EDDiscovery2.Settings settings;
        private System.Windows.Forms.TabPage tabPageRoute;
        private RouteControl routeControl1;
        private System.Windows.Forms.TabPage tabPageScreenshots;
        private EDDiscovery2.ImageHandler.ImageHandler imageHandler1;
        private System.Windows.Forms.TabPage tabPageTriletaration;
        public TrilaterationControl trilaterationControl;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageTravelHistory;
        private TravelHistoryControl travelHistoryControl1;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.ToolStripMenuItem changeMapColorToolStripMenuItem;
    }
}

