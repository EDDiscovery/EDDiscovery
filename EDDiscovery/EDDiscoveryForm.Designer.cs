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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EDDiscoveryForm));
            ExtendedControls.TabStyleSquare tabStyleSquare1 = new ExtendedControls.TabStyleSquare();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.show2DMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.show3DMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeMapColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editThemeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.materialSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllPopoutsInTaskBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllInTaskBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOffAllTransparencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceEDDBUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncEDSMSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openEliteDangerousDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.read21AndFormerLogFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanAllJournalFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendUnsuncedEDDNEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eDDiscoveryHomepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontierForumThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportIssueIdeasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eDDiscoveryChatDiscordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForNewReleaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelInfo = new System.Windows.Forms.Panel();
            this.labelPanelText = new System.Windows.Forms.Label();
            this.label_version = new System.Windows.Forms.Label();
            this.panel_eddiscovery = new System.Windows.Forms.Panel();
            this._syncWorker = new System.ComponentModel.BackgroundWorker();
            this._checkSystemsWorker = new System.ComponentModel.BackgroundWorker();
            this.edsmRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this._refreshWorker = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new ExtendedControls.TabControlCustom();
            this.tabPageTravelHistory = new System.Windows.Forms.TabPage();
            this.travelHistoryControl1 = new EDDiscovery.TravelHistoryControl();
            this.tabPageJournal = new System.Windows.Forms.TabPage();
            this.journalViewControl1 = new EDDiscovery.JournalViewControl();
            this.tabPageTriletaration = new System.Windows.Forms.TabPage();
            this.trilaterationControl = new EDDiscovery.TrilaterationControl();
            this.tabPageScreenshots = new System.Windows.Forms.TabPage();
            this.imageHandler1 = new EDDiscovery2.ImageHandler.ImageHandler();
            this.tabPageRoute = new System.Windows.Forms.TabPage();
            this.routeControl1 = new EDDiscovery.RouteControl();
            this.tabPageRoutesExpeditions = new System.Windows.Forms.TabPage();
            this.savedRouteExpeditionControl1 = new EDDiscovery.SavedRouteExpeditionControl();
            this.tabPageExport = new System.Windows.Forms.TabPage();
            this.exportControl1 = new EDDiscovery.ExportControl();
            this.tabPageSettings = new System.Windows.Forms.TabPage();
            this.settings = new EDDiscovery2.Settings();
            this.button_test = new ExtendedControls.ButtonExt();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.statusStrip1 = new ExtendedControls.StatusStripCustom();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panelInfo.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageTravelHistory.SuspendLayout();
            this.tabPageJournal.SuspendLayout();
            this.tabPageTriletaration.SuspendLayout();
            this.tabPageScreenshots.SuspendLayout();
            this.tabPageRoute.SuspendLayout();
            this.tabPageRoutesExpeditions.SuspendLayout();
            this.tabPageExport.SuspendLayout();
            this.tabPageSettings.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(247, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.show2DMapsToolStripMenuItem,
            this.show3DMapsToolStripMenuItem,
            this.changeMapColorToolStripMenuItem,
            this.editThemeToolStripMenuItem,
            this.materialSearchToolStripMenuItem,
            this.showAllPopoutsInTaskBarToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // show2DMapsToolStripMenuItem
            // 
            this.show2DMapsToolStripMenuItem.Name = "show2DMapsToolStripMenuItem";
            this.show2DMapsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.show2DMapsToolStripMenuItem.Text = "Show 2D maps";
            this.show2DMapsToolStripMenuItem.Click += new System.EventHandler(this.show2DMapsToolStripMenuItem_Click);
            // 
            // show3DMapsToolStripMenuItem
            // 
            this.show3DMapsToolStripMenuItem.Name = "show3DMapsToolStripMenuItem";
            this.show3DMapsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.show3DMapsToolStripMenuItem.Text = "Show 3D maps";
            this.show3DMapsToolStripMenuItem.Click += new System.EventHandler(this.show3DMapsToolStripMenuItem_Click);
            // 
            // changeMapColorToolStripMenuItem
            // 
            this.changeMapColorToolStripMenuItem.Name = "changeMapColorToolStripMenuItem";
            this.changeMapColorToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.changeMapColorToolStripMenuItem.Text = "Set Default Map Colour";
            this.changeMapColorToolStripMenuItem.Click += new System.EventHandler(this.changeMapColorToolStripMenuItem_Click);
            // 
            // editThemeToolStripMenuItem
            // 
            this.editThemeToolStripMenuItem.Name = "editThemeToolStripMenuItem";
            this.editThemeToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.editThemeToolStripMenuItem.Text = "Edit Theme";
            this.editThemeToolStripMenuItem.Click += new System.EventHandler(this.editThemeToolStripMenuItem_Click);
            // 
            // materialSearchToolStripMenuItem
            // 
            this.materialSearchToolStripMenuItem.Name = "materialSearchToolStripMenuItem";
            this.materialSearchToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.materialSearchToolStripMenuItem.Text = "Material Search";
            this.materialSearchToolStripMenuItem.Visible = false;
            this.materialSearchToolStripMenuItem.Click += new System.EventHandler(this.materialSearchToolStripMenuItem_Click);
            // 
            // showAllPopoutsInTaskBarToolStripMenuItem
            // 
            this.showAllPopoutsInTaskBarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllInTaskBarToolStripMenuItem,
            this.turnOffAllTransparencyToolStripMenuItem});
            this.showAllPopoutsInTaskBarToolStripMenuItem.Name = "showAllPopoutsInTaskBarToolStripMenuItem";
            this.showAllPopoutsInTaskBarToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.showAllPopoutsInTaskBarToolStripMenuItem.Text = "Pop-outs";
            // 
            // showAllInTaskBarToolStripMenuItem
            // 
            this.showAllInTaskBarToolStripMenuItem.Name = "showAllInTaskBarToolStripMenuItem";
            this.showAllInTaskBarToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.showAllInTaskBarToolStripMenuItem.Text = "Show All In Task Bar";
            this.showAllInTaskBarToolStripMenuItem.Click += new System.EventHandler(this.showAllInTaskBarToolStripMenuItem_Click);
            // 
            // turnOffAllTransparencyToolStripMenuItem
            // 
            this.turnOffAllTransparencyToolStripMenuItem.Name = "turnOffAllTransparencyToolStripMenuItem";
            this.turnOffAllTransparencyToolStripMenuItem.Size = new System.Drawing.Size(203, 22);
            this.turnOffAllTransparencyToolStripMenuItem.Text = "Turn off all transparency";
            this.turnOffAllTransparencyToolStripMenuItem.Click += new System.EventHandler(this.turnOffAllTransparencyToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forceEDDBUpdateToolStripMenuItem,
            this.syncEDSMSystemsToolStripMenuItem,
            this.showLogfilesToolStripMenuItem,
            this.openEliteDangerousDirectoryToolStripMenuItem,
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem,
            this.read21AndFormerLogFilesToolStripMenuItem,
            this.rescanAllJournalFilesToolStripMenuItem,
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem,
            this.sendUnsuncedEDDNEventsToolStripMenuItem,
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem});
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.adminToolStripMenuItem.Text = "Admin";
            // 
            // forceEDDBUpdateToolStripMenuItem
            // 
            this.forceEDDBUpdateToolStripMenuItem.Name = "forceEDDBUpdateToolStripMenuItem";
            this.forceEDDBUpdateToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.forceEDDBUpdateToolStripMenuItem.Text = "Synchronise with EDDB";
            this.forceEDDBUpdateToolStripMenuItem.Click += new System.EventHandler(this.forceEDDBUpdateToolStripMenuItem_Click);
            // 
            // syncEDSMSystemsToolStripMenuItem
            // 
            this.syncEDSMSystemsToolStripMenuItem.Name = "syncEDSMSystemsToolStripMenuItem";
            this.syncEDSMSystemsToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.syncEDSMSystemsToolStripMenuItem.Text = "Synchronise with EDSM Stars";
            this.syncEDSMSystemsToolStripMenuItem.Click += new System.EventHandler(this.syncEDSMSystemsToolStripMenuItem_Click);
            // 
            // showLogfilesToolStripMenuItem
            // 
            this.showLogfilesToolStripMenuItem.Name = "showLogfilesToolStripMenuItem";
            this.showLogfilesToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.showLogfilesToolStripMenuItem.Text = "Show journal files directory of current commander";
            this.showLogfilesToolStripMenuItem.Click += new System.EventHandler(this.showLogfilesToolStripMenuItem_Click);
            // 
            // openEliteDangerousDirectoryToolStripMenuItem
            // 
            this.openEliteDangerousDirectoryToolStripMenuItem.Name = "openEliteDangerousDirectoryToolStripMenuItem";
            this.openEliteDangerousDirectoryToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.openEliteDangerousDirectoryToolStripMenuItem.Text = "Show Elite Dangerous directory";
            this.openEliteDangerousDirectoryToolStripMenuItem.Click += new System.EventHandler(this.openEliteDangerousDirectoryToolStripMenuItem_Click);
            // 
            // dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem
            // 
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Name = "dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem";
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Text = "Reset all history to current commander";
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Click += new System.EventHandler(this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click);
            // 
            // read21AndFormerLogFilesToolStripMenuItem
            // 
            this.read21AndFormerLogFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem});
            this.read21AndFormerLogFilesToolStripMenuItem.Name = "read21AndFormerLogFilesToolStripMenuItem";
            this.read21AndFormerLogFilesToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.read21AndFormerLogFilesToolStripMenuItem.Text = "Read 2.1 and former log files";
            this.read21AndFormerLogFilesToolStripMenuItem.Click += new System.EventHandler(this.read21AndFormerLogFilesToolStripMenuItem_Click);
            // 
            // read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem
            // 
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem.Name = "read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem";
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem.Text = "Force reload logs";
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem.Click += new System.EventHandler(this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem_Click);
            // 
            // rescanAllJournalFilesToolStripMenuItem
            // 
            this.rescanAllJournalFilesToolStripMenuItem.Name = "rescanAllJournalFilesToolStripMenuItem";
            this.rescanAllJournalFilesToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.rescanAllJournalFilesToolStripMenuItem.Text = "Re-scan all journal files";
            this.rescanAllJournalFilesToolStripMenuItem.Click += new System.EventHandler(this.rescanAllJournalFilesToolStripMenuItem_Click);
            // 
            // deleteDuplicateFSDJumpEntriesToolStripMenuItem
            // 
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Name = "deleteDuplicateFSDJumpEntriesToolStripMenuItem";
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Text = "Delete duplicate FSD Jump entries";
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Click += new System.EventHandler(this.deleteDuplicateFSDJumpEntriesToolStripMenuItem_Click);
            // 
            // sendUnsuncedEDDNEventsToolStripMenuItem
            // 
            this.sendUnsuncedEDDNEventsToolStripMenuItem.Name = "sendUnsuncedEDDNEventsToolStripMenuItem";
            this.sendUnsuncedEDDNEventsToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.sendUnsuncedEDDNEventsToolStripMenuItem.Text = "Send unsynced EDDN events";
            this.sendUnsuncedEDDNEventsToolStripMenuItem.Click += new System.EventHandler(this.sendUnsuncedEDDNEventsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.eDDiscoveryHomepageToolStripMenuItem,
            this.gitHubToolStripMenuItem,
            this.frontierForumThreadToolStripMenuItem,
            this.reportIssueIdeasToolStripMenuItem,
            this.eDDiscoveryChatDiscordToolStripMenuItem,
            this.checkForNewReleaseToolStripMenuItem});
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
            // eDDiscoveryHomepageToolStripMenuItem
            // 
            this.eDDiscoveryHomepageToolStripMenuItem.Name = "eDDiscoveryHomepageToolStripMenuItem";
            this.eDDiscoveryHomepageToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.eDDiscoveryHomepageToolStripMenuItem.Text = "EDDiscovery Help";
            this.eDDiscoveryHomepageToolStripMenuItem.Click += new System.EventHandler(this.eDDiscoveryHomepageToolStripMenuItem_Click);
            // 
            // gitHubToolStripMenuItem
            // 
            this.gitHubToolStripMenuItem.Name = "gitHubToolStripMenuItem";
            this.gitHubToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.gitHubToolStripMenuItem.Text = "Github";
            this.gitHubToolStripMenuItem.Click += new System.EventHandler(this.gitHubToolStripMenuItem_Click);
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
            // checkForNewReleaseToolStripMenuItem
            // 
            this.checkForNewReleaseToolStripMenuItem.Name = "checkForNewReleaseToolStripMenuItem";
            this.checkForNewReleaseToolStripMenuItem.Size = new System.Drawing.Size(216, 22);
            this.checkForNewReleaseToolStripMenuItem.Text = "Check for new release";
            this.checkForNewReleaseToolStripMenuItem.Click += new System.EventHandler(this.checkForNewReleaseToolStripMenuItem_Click);
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
            this.panelInfo.Click += new System.EventHandler(this.panelInfo_Click);
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
            this.labelPanelText.Click += new System.EventHandler(this.labelPanelText_Click);
            // 
            // label_version
            // 
            this.label_version.AutoSize = true;
            this.label_version.Location = new System.Drawing.Point(296, 4);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(71, 13);
            this.label_version.TabIndex = 21;
            this.label_version.Text = "Version Label";
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
            this.panel_eddiscovery.Click += new System.EventHandler(this.paneleddiscovery_Click);
            // 
            // _syncWorker
            // 
            this._syncWorker.WorkerReportsProgress = true;
            this._syncWorker.WorkerSupportsCancellation = true;
            this._syncWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this._syncWorker_DoWork);
            this._syncWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this._syncWorker_ProgressChanged);
            this._syncWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this._syncWorker_RunWorkerCompleted);
            // 
            // _checkSystemsWorker
            // 
            this._checkSystemsWorker.WorkerReportsProgress = true;
            this._checkSystemsWorker.WorkerSupportsCancellation = true;
            this._checkSystemsWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this._checkSystemsWorker_DoWork);
            this._checkSystemsWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this._checkSystemsWorker_ProgressChanged);
            this._checkSystemsWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this._checkSystemsWorker_RunWorkerCompleted);
            // 
            // edsmRefreshTimer
            // 
            this.edsmRefreshTimer.Interval = 3600000;
            this.edsmRefreshTimer.Tick += new System.EventHandler(this.edsmRefreshTimer_Tick);
            // 
            // _refreshWorker
            // 
            this._refreshWorker.WorkerReportsProgress = true;
            this._refreshWorker.WorkerSupportsCancellation = true;
            this._refreshWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.RefreshHistoryWorker);
            this._refreshWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.RefreshHistoryWorkerProgressChanged);
            this._refreshWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.RefreshHistoryWorkerCompleted);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageTravelHistory);
            this.tabControl1.Controls.Add(this.tabPageJournal);
            this.tabControl1.Controls.Add(this.tabPageTriletaration);
            this.tabControl1.Controls.Add(this.tabPageScreenshots);
            this.tabControl1.Controls.Add(this.tabPageRoute);
            this.tabControl1.Controls.Add(this.tabPageRoutesExpeditions);
            this.tabControl1.Controls.Add(this.tabPageExport);
            this.tabControl1.Controls.Add(this.tabPageSettings);
            this.tabControl1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tabControl1.Location = new System.Drawing.Point(0, 28);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(993, 697);
            this.tabControl1.TabColorScaling = 0.5F;
            this.tabControl1.TabControlBorderBrightColor = System.Drawing.Color.LightGray;
            this.tabControl1.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.tabControl1.TabDisabledScaling = 0.5F;
            this.tabControl1.TabIndex = 15;
            this.tabControl1.TabMouseOverColor = System.Drawing.Color.White;
            this.tabControl1.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.tabControl1.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.tabControl1.TabOpaque = 100F;
            this.tabControl1.TabSelectedColor = System.Drawing.Color.LightGray;
            this.tabControl1.TabStyle = tabStyleSquare1;
            this.tabControl1.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.tabControl1.TextSelectedColor = System.Drawing.SystemColors.ControlText;
            // 
            // tabPageTravelHistory
            // 
            this.tabPageTravelHistory.Controls.Add(this.travelHistoryControl1);
            this.tabPageTravelHistory.Location = new System.Drawing.Point(4, 22);
            this.tabPageTravelHistory.Name = "tabPageTravelHistory";
            this.tabPageTravelHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTravelHistory.Size = new System.Drawing.Size(985, 671);
            this.tabPageTravelHistory.TabIndex = 0;
            this.tabPageTravelHistory.Text = "History";
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
            // tabPageJournal
            // 
            this.tabPageJournal.Controls.Add(this.journalViewControl1);
            this.tabPageJournal.Location = new System.Drawing.Point(4, 22);
            this.tabPageJournal.Name = "tabPageJournal";
            this.tabPageJournal.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageJournal.Size = new System.Drawing.Size(985, 671);
            this.tabPageJournal.TabIndex = 7;
            this.tabPageJournal.Text = "Journal";
            this.tabPageJournal.UseVisualStyleBackColor = true;
            // 
            // journalViewControl1
            // 
            this.journalViewControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.journalViewControl1.Location = new System.Drawing.Point(3, 3);
            this.journalViewControl1.Name = "journalViewControl1";
            this.journalViewControl1.Size = new System.Drawing.Size(979, 665);
            this.journalViewControl1.TabIndex = 0;
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
            this.imageHandler1.AutoSize = true;
            this.imageHandler1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageHandler1.Location = new System.Drawing.Point(0, 0);
            this.imageHandler1.Name = "imageHandler1";
            this.imageHandler1.Size = new System.Drawing.Size(985, 671);
            this.imageHandler1.TabIndex = 0;
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
            // tabPageRoutesExpeditions
            // 
            this.tabPageRoutesExpeditions.Controls.Add(this.savedRouteExpeditionControl1);
            this.tabPageRoutesExpeditions.Location = new System.Drawing.Point(4, 22);
            this.tabPageRoutesExpeditions.Name = "tabPageRoutesExpeditions";
            this.tabPageRoutesExpeditions.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageRoutesExpeditions.Size = new System.Drawing.Size(985, 671);
            this.tabPageRoutesExpeditions.TabIndex = 6;
            this.tabPageRoutesExpeditions.Text = "Routes/Expeditions";
            this.tabPageRoutesExpeditions.UseVisualStyleBackColor = true;
            // 
            // savedRouteExpeditionControl1
            // 
            this.savedRouteExpeditionControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.savedRouteExpeditionControl1.Location = new System.Drawing.Point(3, 3);
            this.savedRouteExpeditionControl1.Name = "savedRouteExpeditionControl1";
            this.savedRouteExpeditionControl1.Size = new System.Drawing.Size(979, 665);
            this.savedRouteExpeditionControl1.TabIndex = 0;
            // 
            // tabPageExport
            // 
            this.tabPageExport.Controls.Add(this.exportControl1);
            this.tabPageExport.Location = new System.Drawing.Point(4, 22);
            this.tabPageExport.Name = "tabPageExport";
            this.tabPageExport.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageExport.Size = new System.Drawing.Size(985, 671);
            this.tabPageExport.TabIndex = 8;
            this.tabPageExport.Text = "Export/Import";
            this.tabPageExport.UseVisualStyleBackColor = true;
            // 
            // exportControl1
            // 
            this.exportControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.exportControl1.Location = new System.Drawing.Point(3, 3);
            this.exportControl1.Name = "exportControl1";
            this.exportControl1.Size = new System.Drawing.Size(979, 665);
            this.exportControl1.TabIndex = 0;
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
            // button_test
            // 
            this.button_test.BorderColorScaling = 1.25F;
            this.button_test.ButtonColorScaling = 0.5F;
            this.button_test.ButtonDisabledScaling = 0.5F;
            this.button_test.Location = new System.Drawing.Point(772, -1);
            this.button_test.Name = "button_test";
            this.button_test.Size = new System.Drawing.Size(41, 23);
            this.button_test.TabIndex = 1;
            this.button_test.Text = "Test";
            this.button_test.UseVisualStyleBackColor = true;
            this.button_test.Visible = false;
            this.button_test.Click += new System.EventHandler(this.button_test_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.BackColor = System.Drawing.SystemColors.Control;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.DrawnImage = null;
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.ImageText = null;
            this.panel_minimize.Location = new System.Drawing.Point(941, 1);
            this.panel_minimize.MarginSize = 6;
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 20;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.BackColor = System.Drawing.SystemColors.Control;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.DrawnImage = null;
            this.panel_close.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Close;
            this.panel_close.ImageText = null;
            this.panel_close.Location = new System.Drawing.Point(966, 1);
            this.panel_close.MarginSize = 6;
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 19;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 722);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(993, 22);
            this.statusStrip1.TabIndex = 22;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem
            // 
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem.Name = "clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem";
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem.Text = "Clear EDSM ID assigned to all records for current commander";
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem.Click += new System.EventHandler(this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem_Click);
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
            this.Controls.Add(this.button_test);
            this.Controls.Add(this.panel_minimize);
            this.Controls.Add(this.panel_close);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EDDiscoveryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EDDiscovery";
            this.Activated += new System.EventHandler(this.EDDiscoveryForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EDDiscoveryForm_FormClosing);
            this.Load += new System.EventHandler(this.EDDiscoveryForm_Load);
            this.Shown += new System.EventHandler(this.EDDiscoveryForm_Shown);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.EDDiscoveryForm_Layout);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelInfo.ResumeLayout(false);
            this.panelInfo.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageTravelHistory.ResumeLayout(false);
            this.tabPageJournal.ResumeLayout(false);
            this.tabPageTriletaration.ResumeLayout(false);
            this.tabPageScreenshots.ResumeLayout(false);
            this.tabPageScreenshots.PerformLayout();
            this.tabPageRoute.ResumeLayout(false);
            this.tabPageRoutesExpeditions.ResumeLayout(false);
            this.tabPageExport.ResumeLayout(false);
            this.tabPageSettings.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ButtonExt button_test;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eDDiscoveryHomepageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frontierForumThreadToolStripMenuItem;
        private System.Windows.Forms.Panel panelInfo;
        private System.Windows.Forms.Label labelPanelText;
        private System.Windows.Forms.ToolStripMenuItem show2DMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceEDDBUpdateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncEDSMSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gitHubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportIssueIdeasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem;
        private System.Windows.Forms.Panel panel_eddiscovery;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eDDiscoveryChatDiscordToolStripMenuItem;
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.TabPage tabPageSettings;
        public EDDiscovery2.Settings settings;
        private System.Windows.Forms.TabPage tabPageRoute;
        private RouteControl routeControl1;
        private System.Windows.Forms.TabPage tabPageScreenshots;
        private EDDiscovery2.ImageHandler.ImageHandler imageHandler1;
        private System.Windows.Forms.TabPage tabPageTriletaration;
        public TrilaterationControl trilaterationControl;
        private ExtendedControls.TabControlCustom tabControl1;
        private System.Windows.Forms.TabPage tabPageTravelHistory;
        private TravelHistoryControl travelHistoryControl1;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.ToolStripMenuItem changeMapColorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editThemeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageRoutesExpeditions;
        public SavedRouteExpeditionControl savedRouteExpeditionControl1;
        private ExtendedControls.StatusStripCustom statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem show3DMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.ComponentModel.BackgroundWorker _syncWorker;
        private System.ComponentModel.BackgroundWorker _checkSystemsWorker;
        private System.Windows.Forms.Timer edsmRefreshTimer;
        private System.Windows.Forms.TabPage tabPageJournal;
        private JournalViewControl journalViewControl1;
        private System.ComponentModel.BackgroundWorker _refreshWorker;
        private System.Windows.Forms.ToolStripMenuItem read21AndFormerLogFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openEliteDangerousDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPageExport;
        private ExportControl exportControl1;
        private System.Windows.Forms.ToolStripMenuItem rescanAllJournalFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForNewReleaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteDuplicateFSDJumpEntriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendUnsuncedEDDNEventsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem materialSearchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllPopoutsInTaskBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllInTaskBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOffAllTransparencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem;
    }
}
