/*
 * Copyright © 2015 - 2017 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 *
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
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
            ExtendedControls.TabStyleSquare tabStyleSquare1 = new ExtendedControls.TabStyleSquare();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.show2DMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.show3DMapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllPopoutsInTaskBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllInTaskBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOffAllTransparencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceEDDBUpdateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncEDSMSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fetchLogsAgainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fetchStarDataAgainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.read21AndFormerLogFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanAllJournalFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendUnsuncedEDDNEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendUnsyncedEGOScansToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendHistoricDataToInaraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportVistedStarsListToEliteDangerousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addOnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageAddOnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureAddOnActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editLastActionPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopCurrentlyRunningActionProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eDDiscoveryHomepageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuSeparatorTop = new System.Windows.Forms.ToolStripSeparator();
            this.eDDiscoveryChatDiscordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontierForumThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportIssueIdeasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToRunInSafeModeToResetVariousParametersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuSeparatorBottom = new System.Windows.Forms.ToolStripSeparator();
            this.checkForNewReleaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labelInfoBoxTop = new System.Windows.Forms.Label();
            this.label_version = new System.Windows.Forms.Label();
            this.edsmRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.buttonReloadActions = new ExtendedControls.ExtButton();
            this.panel_minimize = new ExtendedControls.ExtPanelDrawn();
            this.panel_close = new ExtendedControls.ExtPanelDrawn();
            this.statusStrip = new ExtendedControls.ExtStatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyIconMenu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenu_Hide = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.panelToolBar = new ExtendedControls.ExtPanelRollUp();
            this.comboBoxCustomProfiles = new ExtendedControls.ExtComboBox();
            this.comboBoxCommander = new ExtendedControls.ExtComboBox();
            this.buttonExtEDSMSync = new ExtendedControls.ExtButton();
            this.buttonExtPopOut = new ExtendedControls.ExtButton();
            this.buttonExtEditAddOns = new ExtendedControls.ExtButton();
            this.buttonExtManageAddOns = new ExtendedControls.ExtButton();
            this.buttonExtRefresh = new ExtendedControls.ExtButton();
            this.buttonExt2dmap = new ExtendedControls.ExtButton();
            this.buttonExt3dmap = new ExtendedControls.ExtButton();
            this.panel_eddiscovery = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControlMain = new EDDiscovery.MajorTabControl();
            this.contextMenuStripTabs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.popOutPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMenuTop = new System.Windows.Forms.Panel();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.notifyIconContextMenuStrip.SuspendLayout();
            this.panelToolBar.SuspendLayout();
            this.contextMenuStripTabs.SuspendLayout();
            this.panelMenuTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.addOnsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(221, 24);
            this.menuStrip.TabIndex = 16;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownCAPTION);
            this.menuStrip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.show2DMapsToolStripMenuItem,
            this.show3DMapsToolStripMenuItem,
            this.showAllPopoutsInTaskBarToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Tools_Settings;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // show2DMapsToolStripMenuItem
            // 
            this.show2DMapsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Tools_Open2DMap;
            this.show2DMapsToolStripMenuItem.Name = "show2DMapsToolStripMenuItem";
            this.show2DMapsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.show2DMapsToolStripMenuItem.Text = "Show &2D maps";
            this.show2DMapsToolStripMenuItem.Click += new System.EventHandler(this.show2DMapsToolStripMenuItem_Click);
            // 
            // show3DMapsToolStripMenuItem
            // 
            this.show3DMapsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Tools_Open3DMap;
            this.show3DMapsToolStripMenuItem.Name = "show3DMapsToolStripMenuItem";
            this.show3DMapsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.show3DMapsToolStripMenuItem.Text = "Show &3D maps";
            this.show3DMapsToolStripMenuItem.Click += new System.EventHandler(this.show3DMapsToolStripMenuItem_Click);
            // 
            // showAllPopoutsInTaskBarToolStripMenuItem
            // 
            this.showAllPopoutsInTaskBarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllInTaskBarToolStripMenuItem,
            this.turnOffAllTransparencyToolStripMenuItem});
            this.showAllPopoutsInTaskBarToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Tools_Popouts_Menu;
            this.showAllPopoutsInTaskBarToolStripMenuItem.Name = "showAllPopoutsInTaskBarToolStripMenuItem";
            this.showAllPopoutsInTaskBarToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.showAllPopoutsInTaskBarToolStripMenuItem.Text = "&Pop-outs";
            // 
            // showAllInTaskBarToolStripMenuItem
            // 
            this.showAllInTaskBarToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Tools_Popouts_ShowAllInTaskbar;
            this.showAllInTaskBarToolStripMenuItem.Name = "showAllInTaskBarToolStripMenuItem";
            this.showAllInTaskBarToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.showAllInTaskBarToolStripMenuItem.Text = "&Show All In Task Bar";
            this.showAllInTaskBarToolStripMenuItem.Click += new System.EventHandler(this.showAllInTaskBarToolStripMenuItem_Click);
            // 
            // turnOffAllTransparencyToolStripMenuItem
            // 
            this.turnOffAllTransparencyToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Tools_Popouts_DisableTransparency;
            this.turnOffAllTransparencyToolStripMenuItem.Name = "turnOffAllTransparencyToolStripMenuItem";
            this.turnOffAllTransparencyToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.turnOffAllTransparencyToolStripMenuItem.Text = "&Turn Off All Transparency";
            this.turnOffAllTransparencyToolStripMenuItem.Click += new System.EventHandler(this.turnOffAllTransparencyToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Tools_Exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.forceEDDBUpdateToolStripMenuItem,
            this.syncEDSMSystemsToolStripMenuItem,
            this.showLogfilesToolStripMenuItem,
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem,
            this.read21AndFormerLogFilesToolStripMenuItem,
            this.rescanAllJournalFilesToolStripMenuItem,
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem,
            this.sendUnsuncedEDDNEventsToolStripMenuItem,
            this.sendUnsyncedEGOScansToolStripMenuItem,
            this.sendHistoricDataToInaraToolStripMenuItem,
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem,
            this.exportVistedStarsListToEliteDangerousToolStripMenuItem});
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.adminToolStripMenuItem.Text = "A&dmin";
            // 
            // forceEDDBUpdateToolStripMenuItem
            // 
            this.forceEDDBUpdateToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_EDDBSystemsSync;
            this.forceEDDBUpdateToolStripMenuItem.Name = "forceEDDBUpdateToolStripMenuItem";
            this.forceEDDBUpdateToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.forceEDDBUpdateToolStripMenuItem.Text = "Synchronise with EDDB";
            this.forceEDDBUpdateToolStripMenuItem.Click += new System.EventHandler(this.forceEDDBUpdateToolStripMenuItem_Click);
            // 
            // syncEDSMSystemsToolStripMenuItem
            // 
            this.syncEDSMSystemsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fetchLogsAgainToolStripMenuItem,
            this.fetchStarDataAgainToolStripMenuItem});
            this.syncEDSMSystemsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_EDSMSystemsSync;
            this.syncEDSMSystemsToolStripMenuItem.Name = "syncEDSMSystemsToolStripMenuItem";
            this.syncEDSMSystemsToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.syncEDSMSystemsToolStripMenuItem.Text = "Synchronise with EDSM";
            // 
            // fetchLogsAgainToolStripMenuItem
            // 
            this.fetchLogsAgainToolStripMenuItem.Name = "fetchLogsAgainToolStripMenuItem";
            this.fetchLogsAgainToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.fetchLogsAgainToolStripMenuItem.Text = "Fetch Logs Again";
            this.fetchLogsAgainToolStripMenuItem.Click += new System.EventHandler(this.fetchLogsAgainToolStripMenuItem_Click);
            // 
            // fetchStarDataAgainToolStripMenuItem
            // 
            this.fetchStarDataAgainToolStripMenuItem.Name = "fetchStarDataAgainToolStripMenuItem";
            this.fetchStarDataAgainToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.fetchStarDataAgainToolStripMenuItem.Text = "Fetch Star Data Again";
            this.fetchStarDataAgainToolStripMenuItem.Click += new System.EventHandler(this.syncEDSMSystemsToolStripMenuItem_Click);
            // 
            // showLogfilesToolStripMenuItem
            // 
            this.showLogfilesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_ShowLogFiles;
            this.showLogfilesToolStripMenuItem.Name = "showLogfilesToolStripMenuItem";
            this.showLogfilesToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.showLogfilesToolStripMenuItem.Text = "Show journal files directory of current commander";
            this.showLogfilesToolStripMenuItem.Click += new System.EventHandler(this.showLogfilesToolStripMenuItem_Click);
            // 
            // dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem
            // 
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_ResetHistory;
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Name = "dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem";
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Text = "Reset all history to current commander";
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Click += new System.EventHandler(this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem_Click);
            // 
            // read21AndFormerLogFilesToolStripMenuItem
            // 
            this.read21AndFormerLogFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem});
            this.read21AndFormerLogFilesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_ReadNetLogs;
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
            this.rescanAllJournalFilesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_RescanJournals;
            this.rescanAllJournalFilesToolStripMenuItem.Name = "rescanAllJournalFilesToolStripMenuItem";
            this.rescanAllJournalFilesToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.rescanAllJournalFilesToolStripMenuItem.Text = "Re-scan all journal files";
            this.rescanAllJournalFilesToolStripMenuItem.Click += new System.EventHandler(this.rescanAllJournalFilesToolStripMenuItem_Click);
            // 
            // deleteDuplicateFSDJumpEntriesToolStripMenuItem
            // 
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_DeleteDupFSDJumps;
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Name = "deleteDuplicateFSDJumpEntriesToolStripMenuItem";
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Text = "Delete duplicate FSD Jump entries";
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Click += new System.EventHandler(this.deleteDuplicateFSDJumpEntriesToolStripMenuItem_Click);
            // 
            // sendUnsuncedEDDNEventsToolStripMenuItem
            // 
            this.sendUnsuncedEDDNEventsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_SendUnsyncedEDDN;
            this.sendUnsuncedEDDNEventsToolStripMenuItem.Name = "sendUnsuncedEDDNEventsToolStripMenuItem";
            this.sendUnsuncedEDDNEventsToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.sendUnsuncedEDDNEventsToolStripMenuItem.Text = "Send unsynced EDDN events";
            this.sendUnsuncedEDDNEventsToolStripMenuItem.Click += new System.EventHandler(this.sendUnsuncedEDDNEventsToolStripMenuItem_Click);
            // 
            // sendUnsyncedEGOScansToolStripMenuItem
            // 
            this.sendUnsyncedEGOScansToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_SendUnsyncedEGO;
            this.sendUnsyncedEGOScansToolStripMenuItem.Name = "sendUnsyncedEGOScansToolStripMenuItem";
            this.sendUnsyncedEGOScansToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.sendUnsyncedEGOScansToolStripMenuItem.Text = "Send unsynced EGO scans";
            this.sendUnsyncedEGOScansToolStripMenuItem.Click += new System.EventHandler(this.sendUnsyncedEGOScansToolStripMenuItem_Click);
            // 
            // sendHistoricDataToInaraToolStripMenuItem
            // 
            this.sendHistoricDataToInaraToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_SendUnsyncedEGO;
            this.sendHistoricDataToInaraToolStripMenuItem.Name = "sendHistoricDataToInaraToolStripMenuItem";
            this.sendHistoricDataToInaraToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.sendHistoricDataToInaraToolStripMenuItem.Text = "Send to Inara historic data (previous ships, stored modules)";
            this.sendHistoricDataToInaraToolStripMenuItem.Click += new System.EventHandler(this.sendHistoricDataToInaraToolStripMenuItem_Click);
            // 
            // clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem
            // 
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_ClearEDSMIDs;
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem.Name = "clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem";
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem.Text = "Clear EDSM ID assigned to all records for current commander";
            this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem.Click += new System.EventHandler(this.clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem_Click);
            // 
            // exportVistedStarsListToEliteDangerousToolStripMenuItem
            // 
            this.exportVistedStarsListToEliteDangerousToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_ExportVisitedStars;
            this.exportVistedStarsListToEliteDangerousToolStripMenuItem.Name = "exportVistedStarsListToEliteDangerousToolStripMenuItem";
            this.exportVistedStarsListToEliteDangerousToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.exportVistedStarsListToEliteDangerousToolStripMenuItem.Text = "Export Visted Stars List to Elite Dangerous";
            this.exportVistedStarsListToEliteDangerousToolStripMenuItem.Click += new System.EventHandler(this.exportVistedStarsListToEliteDangerousToolStripMenuItem_Click);
            // 
            // addOnsToolStripMenuItem
            // 
            this.addOnsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageAddOnsToolStripMenuItem,
            this.configureAddOnActionsToolStripMenuItem,
            this.editLastActionPackToolStripMenuItem,
            this.stopCurrentlyRunningActionProgramToolStripMenuItem});
            this.addOnsToolStripMenuItem.Name = "addOnsToolStripMenuItem";
            this.addOnsToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.addOnsToolStripMenuItem.Text = "&Add-Ons";
            // 
            // manageAddOnsToolStripMenuItem
            // 
            this.manageAddOnsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Addons_ManageAddOns;
            this.manageAddOnsToolStripMenuItem.Name = "manageAddOnsToolStripMenuItem";
            this.manageAddOnsToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.manageAddOnsToolStripMenuItem.Text = "&Manage Add-Ons";
            this.manageAddOnsToolStripMenuItem.Click += new System.EventHandler(this.manageAddOnsToolStripMenuItem_Click);
            // 
            // configureAddOnActionsToolStripMenuItem
            // 
            this.configureAddOnActionsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Addons_ConfigureAddOnActions;
            this.configureAddOnActionsToolStripMenuItem.Name = "configureAddOnActionsToolStripMenuItem";
            this.configureAddOnActionsToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.configureAddOnActionsToolStripMenuItem.Text = "&Edit Add-On Action Files";
            this.configureAddOnActionsToolStripMenuItem.Click += new System.EventHandler(this.configureAddOnActionsToolStripMenuItem_Click);
            // 
            // editLastActionPackToolStripMenuItem
            // 
            this.editLastActionPackToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Addons_EditLastActionPack;
            this.editLastActionPackToolStripMenuItem.Name = "editLastActionPackToolStripMenuItem";
            this.editLastActionPackToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.editLastActionPackToolStripMenuItem.Text = "Edit Last Action Pack";
            this.editLastActionPackToolStripMenuItem.Click += new System.EventHandler(this.editLastActionPackToolStripMenuItem_Click);
            // 
            // stopCurrentlyRunningActionProgramToolStripMenuItem
            // 
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Addons_StopCurrentAction;
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Name = "stopCurrentlyRunningActionProgramToolStripMenuItem";
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Text = "&Stop currently running Action Program";
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Click += new System.EventHandler(this.stopCurrentlyRunningActionProgramToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.eDDiscoveryHomepageToolStripMenuItem,
            this.helpMenuSeparatorTop,
            this.eDDiscoveryChatDiscordToolStripMenuItem,
            this.frontierForumThreadToolStripMenuItem,
            this.gitHubToolStripMenuItem,
            this.reportIssueIdeasToolStripMenuItem,
            this.howToRunInSafeModeToResetVariousParametersToolStripMenuItem,
            this.helpMenuSeparatorBottom,
            this.checkForNewReleaseToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Help_About;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(354, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // eDDiscoveryHomepageToolStripMenuItem
            // 
            this.eDDiscoveryHomepageToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Help_Help;
            this.eDDiscoveryHomepageToolStripMenuItem.Name = "eDDiscoveryHomepageToolStripMenuItem";
            this.eDDiscoveryHomepageToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.eDDiscoveryHomepageToolStripMenuItem.Size = new System.Drawing.Size(354, 22);
            this.eDDiscoveryHomepageToolStripMenuItem.Text = "&View Help";
            this.eDDiscoveryHomepageToolStripMenuItem.Click += new System.EventHandler(this.eDDiscoveryHomepageToolStripMenuItem_Click);
            // 
            // helpMenuSeparatorTop
            // 
            this.helpMenuSeparatorTop.Name = "helpMenuSeparatorTop";
            this.helpMenuSeparatorTop.Size = new System.Drawing.Size(351, 6);
            // 
            // eDDiscoveryChatDiscordToolStripMenuItem
            // 
            this.eDDiscoveryChatDiscordToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Help_DiscordChat;
            this.eDDiscoveryChatDiscordToolStripMenuItem.Name = "eDDiscoveryChatDiscordToolStripMenuItem";
            this.eDDiscoveryChatDiscordToolStripMenuItem.Size = new System.Drawing.Size(354, 22);
            this.eDDiscoveryChatDiscordToolStripMenuItem.Text = "&Discord - EDD Community Chat";
            this.eDDiscoveryChatDiscordToolStripMenuItem.Click += new System.EventHandler(this.eDDiscoveryChatDiscordToolStripMenuItem_Click);
            // 
            // frontierForumThreadToolStripMenuItem
            // 
            this.frontierForumThreadToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Help_FrontierForumThread;
            this.frontierForumThreadToolStripMenuItem.Name = "frontierForumThreadToolStripMenuItem";
            this.frontierForumThreadToolStripMenuItem.Size = new System.Drawing.Size(354, 22);
            this.frontierForumThreadToolStripMenuItem.Text = "&Frontier Forum Thread";
            this.frontierForumThreadToolStripMenuItem.Click += new System.EventHandler(this.frontierForumThreadToolStripMenuItem_Click);
            // 
            // gitHubToolStripMenuItem
            // 
            this.gitHubToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Help_Github;
            this.gitHubToolStripMenuItem.Name = "gitHubToolStripMenuItem";
            this.gitHubToolStripMenuItem.Size = new System.Drawing.Size(354, 22);
            this.gitHubToolStripMenuItem.Text = "&Project Page (GitHub)";
            this.gitHubToolStripMenuItem.Click += new System.EventHandler(this.gitHubToolStripMenuItem_Click);
            // 
            // reportIssueIdeasToolStripMenuItem
            // 
            this.reportIssueIdeasToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Help_ReportIssue;
            this.reportIssueIdeasToolStripMenuItem.Name = "reportIssueIdeasToolStripMenuItem";
            this.reportIssueIdeasToolStripMenuItem.Size = new System.Drawing.Size(354, 22);
            this.reportIssueIdeasToolStripMenuItem.Text = "&Report Issue / Idea";
            this.reportIssueIdeasToolStripMenuItem.Click += new System.EventHandler(this.reportIssueIdeasToolStripMenuItem_Click);
            // 
            // howToRunInSafeModeToResetVariousParametersToolStripMenuItem
            // 
            this.howToRunInSafeModeToResetVariousParametersToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Help_SafeModeHelp;
            this.howToRunInSafeModeToResetVariousParametersToolStripMenuItem.Name = "howToRunInSafeModeToResetVariousParametersToolStripMenuItem";
            this.howToRunInSafeModeToResetVariousParametersToolStripMenuItem.Size = new System.Drawing.Size(354, 22);
            this.howToRunInSafeModeToResetVariousParametersToolStripMenuItem.Text = "How to Run in Safe Mode to reset various parameters";
            this.howToRunInSafeModeToResetVariousParametersToolStripMenuItem.Click += new System.EventHandler(this.howToRunInSafeModeToResetVariousParametersToolStripMenuItem_Click);
            // 
            // helpMenuSeparatorBottom
            // 
            this.helpMenuSeparatorBottom.Name = "helpMenuSeparatorBottom";
            this.helpMenuSeparatorBottom.Size = new System.Drawing.Size(351, 6);
            // 
            // checkForNewReleaseToolStripMenuItem
            // 
            this.checkForNewReleaseToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Help_CheckForNewRelease;
            this.checkForNewReleaseToolStripMenuItem.Name = "checkForNewReleaseToolStripMenuItem";
            this.checkForNewReleaseToolStripMenuItem.Size = new System.Drawing.Size(354, 22);
            this.checkForNewReleaseToolStripMenuItem.Text = "&Check for Updates";
            this.checkForNewReleaseToolStripMenuItem.Click += new System.EventHandler(this.checkForNewReleaseToolStripMenuItem_Click);
            // 
            // labelInfoBoxTop
            // 
            this.labelInfoBoxTop.AutoSize = true;
            this.labelInfoBoxTop.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInfoBoxTop.Location = new System.Drawing.Point(382, 4);
            this.labelInfoBoxTop.Name = "labelInfoBoxTop";
            this.labelInfoBoxTop.Size = new System.Drawing.Size(43, 13);
            this.labelInfoBoxTop.TabIndex = 0;
            this.labelInfoBoxTop.Text = "<code>";
            this.labelInfoBoxTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelInfoBoxTop_MouseDown);
            this.labelInfoBoxTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // label_version
            // 
            this.label_version.AutoSize = true;
            this.label_version.Location = new System.Drawing.Point(296, 4);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(43, 13);
            this.label_version.TabIndex = 21;
            this.label_version.Text = "<code>";
            this.label_version.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownCAPTION);
            this.label_version.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // edsmRefreshTimer
            // 
            this.edsmRefreshTimer.Interval = 3600000;
            this.edsmRefreshTimer.Tick += new System.EventHandler(this.edsmRefreshTimer_Tick);
            // 
            // buttonReloadActions
            // 
            this.buttonReloadActions.Location = new System.Drawing.Point(651, 4);
            this.buttonReloadActions.Name = "buttonReloadActions";
            this.buttonReloadActions.Size = new System.Drawing.Size(140, 23);
            this.buttonReloadActions.TabIndex = 1;
            this.buttonReloadActions.Text = "Reload-A";
            this.toolTip.SetToolTip(this.buttonReloadActions, "DEBUG reload action system");
            this.buttonReloadActions.UseVisualStyleBackColor = true;
            this.buttonReloadActions.Visible = false;
            this.buttonReloadActions.Click += new System.EventHandler(this.buttonReloadActions_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.AutoEllipsis = false;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.Image = null;
            this.panel_minimize.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(937, 2);
            this.panel_minimize.MouseOverColor = System.Drawing.Color.White;
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.PanelDisabledScaling = 0.25F;
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 20;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_minimize.UseMnemonic = true;
            this.panel_minimize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_minimize_MouseClick);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(957, 2);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.PanelDisabledScaling = 0.25F;
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 19;
            this.panel_close.TabStop = false;
            this.panel_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_close.UseMnemonic = true;
            this.panel_close.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_close_MouseClick);
            // 
            // statusStrip
            // 
            this.statusStrip.AutoSize = false;
            this.statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 539);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(984, 22);
            this.statusStrip.TabIndex = 22;
            this.statusStrip.Text = "statusStrip1";
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
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.notifyIconContextMenuStrip;
            this.notifyIcon1.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.notifyIcon1.Text = "EDDiscovery";
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // notifyIconContextMenuStrip
            // 
            this.notifyIconContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyIconMenu_Open,
            this.notifyIconMenu_Hide,
            this.notifyIconMenu_Exit});
            this.notifyIconContextMenuStrip.Name = "notifyIconContextMenuStrip1";
            this.notifyIconContextMenuStrip.Size = new System.Drawing.Size(172, 70);
            // 
            // notifyIconMenu_Open
            // 
            this.notifyIconMenu_Open.Name = "notifyIconMenu_Open";
            this.notifyIconMenu_Open.Size = new System.Drawing.Size(171, 22);
            this.notifyIconMenu_Open.Text = "&Open EDDiscovery";
            this.notifyIconMenu_Open.Click += new System.EventHandler(this.notifyIconMenu_Open_Click);
            // 
            // notifyIconMenu_Hide
            // 
            this.notifyIconMenu_Hide.Name = "notifyIconMenu_Hide";
            this.notifyIconMenu_Hide.Size = new System.Drawing.Size(171, 22);
            this.notifyIconMenu_Hide.Text = "&Hide Tray Icon";
            this.notifyIconMenu_Hide.Click += new System.EventHandler(this.notifyIconMenu_Hide_Click);
            // 
            // notifyIconMenu_Exit
            // 
            this.notifyIconMenu_Exit.Name = "notifyIconMenu_Exit";
            this.notifyIconMenu_Exit.Size = new System.Drawing.Size(171, 22);
            this.notifyIconMenu_Exit.Text = "E&xit";
            this.notifyIconMenu_Exit.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // panelToolBar
            // 
            this.panelToolBar.BackColor = System.Drawing.Color.Transparent;
            this.panelToolBar.Controls.Add(this.comboBoxCustomProfiles);
            this.panelToolBar.Controls.Add(this.comboBoxCommander);
            this.panelToolBar.Controls.Add(this.buttonExtEDSMSync);
            this.panelToolBar.Controls.Add(this.buttonExtPopOut);
            this.panelToolBar.Controls.Add(this.buttonExtEditAddOns);
            this.panelToolBar.Controls.Add(this.buttonExtManageAddOns);
            this.panelToolBar.Controls.Add(this.buttonExtRefresh);
            this.panelToolBar.Controls.Add(this.buttonExt2dmap);
            this.panelToolBar.Controls.Add(this.buttonExt3dmap);
            this.panelToolBar.Controls.Add(this.buttonReloadActions);
            this.panelToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolBar.HiddenMarkerWidth = 0;
            this.panelToolBar.Location = new System.Drawing.Point(0, 24);
            this.panelToolBar.Name = "panelToolBar";
            this.panelToolBar.PinState = true;
            this.panelToolBar.RolledUpHeight = 5;
            this.panelToolBar.RollUpAnimationTime = 500;
            this.panelToolBar.RollUpDelay = 1000;
            this.panelToolBar.SecondHiddenMarkerWidth = 0;
            this.panelToolBar.ShowHiddenMarker = true;
            this.panelToolBar.Size = new System.Drawing.Size(984, 32);
            this.panelToolBar.TabIndex = 1;
            this.panelToolBar.UnrolledHeight = 32;
            this.panelToolBar.UnrollHoverDelay = 1000;
            // 
            // comboBoxCustomProfiles
            // 
            this.comboBoxCustomProfiles.ArrowWidth = 1;
            this.comboBoxCustomProfiles.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomProfiles.ButtonColorScaling = 0.5F;
            this.comboBoxCustomProfiles.DataSource = null;
            this.comboBoxCustomProfiles.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomProfiles.DisplayMember = "";
            this.comboBoxCustomProfiles.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomProfiles.DropDownHeight = 400;
            this.comboBoxCustomProfiles.DropDownWidth = 150;
            this.comboBoxCustomProfiles.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomProfiles.ItemHeight = 13;
            this.comboBoxCustomProfiles.Location = new System.Drawing.Point(197, 4);
            this.comboBoxCustomProfiles.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomProfiles.Name = "comboBoxCustomProfiles";
            this.comboBoxCustomProfiles.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomProfiles.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomProfiles.ScrollBarWidth = 16;
            this.comboBoxCustomProfiles.SelectedIndex = -1;
            this.comboBoxCustomProfiles.SelectedItem = null;
            this.comboBoxCustomProfiles.SelectedValue = null;
            this.comboBoxCustomProfiles.Size = new System.Drawing.Size(100, 21);
            this.comboBoxCustomProfiles.TabIndex = 4;
            this.comboBoxCustomProfiles.Text = "comboBoxCustom1";
            this.comboBoxCustomProfiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCustomProfiles, "Use to select new profile or edit profile settings");
            this.comboBoxCustomProfiles.ValueMember = "";
            // 
            // comboBoxCommander
            // 
            this.comboBoxCommander.ArrowWidth = 1;
            this.comboBoxCommander.BorderColor = System.Drawing.Color.White;
            this.comboBoxCommander.ButtonColorScaling = 0.5F;
            this.comboBoxCommander.DataSource = null;
            this.comboBoxCommander.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCommander.DisplayMember = "";
            this.comboBoxCommander.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCommander.DropDownHeight = 400;
            this.comboBoxCommander.DropDownWidth = 200;
            this.comboBoxCommander.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCommander.ItemHeight = 13;
            this.comboBoxCommander.Location = new System.Drawing.Point(12, 4);
            this.comboBoxCommander.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCommander.Name = "comboBoxCommander";
            this.comboBoxCommander.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCommander.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCommander.ScrollBarWidth = 13;
            this.comboBoxCommander.SelectedIndex = -1;
            this.comboBoxCommander.SelectedItem = null;
            this.comboBoxCommander.SelectedValue = null;
            this.comboBoxCommander.Size = new System.Drawing.Size(149, 21);
            this.comboBoxCommander.TabIndex = 3;
            this.comboBoxCommander.Text = "Cmdr";
            this.comboBoxCommander.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCommander, "Select the commander to view");
            this.comboBoxCommander.ValueMember = "";
            this.comboBoxCommander.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommander_SelectedIndexChanged);
            // 
            // buttonExtEDSMSync
            // 
            this.buttonExtEDSMSync.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_SyncEDSM;
            this.buttonExtEDSMSync.Location = new System.Drawing.Point(428, 3);
            this.buttonExtEDSMSync.Name = "buttonExtEDSMSync";
            this.buttonExtEDSMSync.Size = new System.Drawing.Size(24, 24);
            this.buttonExtEDSMSync.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtEDSMSync, "Send any unsynced logs to EDSM");
            this.buttonExtEDSMSync.UseVisualStyleBackColor = true;
            this.buttonExtEDSMSync.Click += new System.EventHandler(this.buttonExtEDSMSync_Click);
            // 
            // buttonExtPopOut
            // 
            this.buttonExtPopOut.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtPopOut.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_Popouts;
            this.buttonExtPopOut.Location = new System.Drawing.Point(458, 3);
            this.buttonExtPopOut.Name = "buttonExtPopOut";
            this.buttonExtPopOut.Size = new System.Drawing.Size(24, 24);
            this.buttonExtPopOut.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtPopOut, "Click to select a pop out panel to display");
            this.buttonExtPopOut.UseVisualStyleBackColor = false;
            this.buttonExtPopOut.Click += new System.EventHandler(this.buttonExtPopOut_Click);
            // 
            // buttonExtEditAddOns
            // 
            this.buttonExtEditAddOns.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtEditAddOns.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_EditAddons;
            this.buttonExtEditAddOns.Location = new System.Drawing.Point(518, 3);
            this.buttonExtEditAddOns.Name = "buttonExtEditAddOns";
            this.buttonExtEditAddOns.Size = new System.Drawing.Size(24, 24);
            this.buttonExtEditAddOns.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtEditAddOns, "Edit Add-Ons");
            this.buttonExtEditAddOns.UseVisualStyleBackColor = false;
            this.buttonExtEditAddOns.Click += new System.EventHandler(this.buttonExtEditAddOns_Click);
            // 
            // buttonExtManageAddOns
            // 
            this.buttonExtManageAddOns.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtManageAddOns.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_ManageAddOns;
            this.buttonExtManageAddOns.Location = new System.Drawing.Point(488, 3);
            this.buttonExtManageAddOns.Name = "buttonExtManageAddOns";
            this.buttonExtManageAddOns.Size = new System.Drawing.Size(24, 24);
            this.buttonExtManageAddOns.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtManageAddOns, "Manage Add-Ons");
            this.buttonExtManageAddOns.UseVisualStyleBackColor = false;
            this.buttonExtManageAddOns.Click += new System.EventHandler(this.buttonExtManageAddOns_Click);
            // 
            // buttonExtRefresh
            // 
            this.buttonExtRefresh.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtRefresh.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_Refresh;
            this.buttonExtRefresh.Location = new System.Drawing.Point(167, 3);
            this.buttonExtRefresh.Name = "buttonExtRefresh";
            this.buttonExtRefresh.Size = new System.Drawing.Size(24, 24);
            this.buttonExtRefresh.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtRefresh, "Refresh the history");
            this.buttonExtRefresh.UseVisualStyleBackColor = false;
            this.buttonExtRefresh.Click += new System.EventHandler(this.buttonExtRefresh_Click);
            // 
            // buttonExt2dmap
            // 
            this.buttonExt2dmap.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt2dmap.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_Open2DMap;
            this.buttonExt2dmap.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonExt2dmap.Location = new System.Drawing.Point(306, 3);
            this.buttonExt2dmap.Name = "buttonExt2dmap";
            this.buttonExt2dmap.Size = new System.Drawing.Size(56, 24);
            this.buttonExt2dmap.TabIndex = 2;
            this.buttonExt2dmap.Text = "2D";
            this.buttonExt2dmap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonExt2dmap, "Open the 2D Map");
            this.buttonExt2dmap.UseVisualStyleBackColor = false;
            this.buttonExt2dmap.Click += new System.EventHandler(this.buttonExt2dmap_Click);
            // 
            // buttonExt3dmap
            // 
            this.buttonExt3dmap.BackColor = System.Drawing.Color.Transparent;
            this.buttonExt3dmap.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_Open3DMap;
            this.buttonExt3dmap.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonExt3dmap.Location = new System.Drawing.Point(366, 3);
            this.buttonExt3dmap.Name = "buttonExt3dmap";
            this.buttonExt3dmap.Size = new System.Drawing.Size(56, 24);
            this.buttonExt3dmap.TabIndex = 2;
            this.buttonExt3dmap.Text = "3D";
            this.buttonExt3dmap.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.buttonExt3dmap, "Open the 3D Map");
            this.buttonExt3dmap.UseVisualStyleBackColor = false;
            this.buttonExt3dmap.Click += new System.EventHandler(this.buttonExt3dmap_Click);
            // 
            // panel_eddiscovery
            // 
            this.panel_eddiscovery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_eddiscovery.BackColor = System.Drawing.SystemColors.Control;
            this.panel_eddiscovery.BackgroundImage = global::EDDiscovery.Properties.Resources.Logo;
            this.panel_eddiscovery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_eddiscovery.Location = new System.Drawing.Point(820, 0);
            this.panel_eddiscovery.Name = "panel_eddiscovery";
            this.panel_eddiscovery.Size = new System.Drawing.Size(100, 46);
            this.panel_eddiscovery.TabIndex = 18;
            this.panel_eddiscovery.Click += new System.EventHandler(this.paneleddiscovery_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.AllowDragReorder = true;
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tabControlMain.Location = new System.Drawing.Point(0, 56);
            this.tabControlMain.Multiline = true;
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(984, 483);
            this.tabControlMain.TabColorScaling = 0.5F;
            this.tabControlMain.TabControlBorderBrightColor = System.Drawing.Color.LightGray;
            this.tabControlMain.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.tabControlMain.TabDisabledScaling = 0.5F;
            this.tabControlMain.TabIndex = 15;
            this.tabControlMain.TabMouseOverColor = System.Drawing.Color.White;
            this.tabControlMain.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.tabControlMain.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.tabControlMain.TabOpaque = 100F;
            this.tabControlMain.TabSelectedColor = System.Drawing.Color.LightGray;
            this.tabControlMain.TabStyle = tabStyleSquare1;
            this.tabControlMain.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.tabControlMain.TextSelectedColor = System.Drawing.SystemColors.ControlText;
            this.toolTip.SetToolTip(this.tabControlMain, "Right click to add/remove tabs, Left click drag to reorder");
            this.tabControlMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControlMain_MouseClick);
            // 
            // contextMenuStripTabs
            // 
            this.contextMenuStripTabs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTabToolStripMenuItem,
            this.removeTabToolStripMenuItem,
            this.renameTabToolStripMenuItem,
            this.popOutPanelToolStripMenuItem});
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
            // popOutPanelToolStripMenuItem
            // 
            this.popOutPanelToolStripMenuItem.Name = "popOutPanelToolStripMenuItem";
            this.popOutPanelToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.popOutPanelToolStripMenuItem.Text = "Pop Out Panel..";
            // 
            // panelMenuTop
            // 
            this.panelMenuTop.Controls.Add(this.menuStrip);
            this.panelMenuTop.Controls.Add(this.panel_close);
            this.panelMenuTop.Controls.Add(this.panel_minimize);
            this.panelMenuTop.Controls.Add(this.labelInfoBoxTop);
            this.panelMenuTop.Controls.Add(this.label_version);
            this.panelMenuTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelMenuTop.Location = new System.Drawing.Point(0, 0);
            this.panelMenuTop.Name = "panelMenuTop";
            this.panelMenuTop.Size = new System.Drawing.Size(984, 24);
            this.panelMenuTop.TabIndex = 1;
            this.panelMenuTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownCAPTION);
            this.panelMenuTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // EDDiscoveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.panel_eddiscovery);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.panelToolBar);
            this.Controls.Add(this.panelMenuTop);
            this.Controls.Add(this.statusStrip);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.MainMenuStrip = this.menuStrip;
            this.Name = "EDDiscoveryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EDDiscovery";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EDDiscoveryForm_FormClosing);
            this.Load += new System.EventHandler(this.EDDiscoveryForm_Load);
            this.Shown += new System.EventHandler(this.EDDiscoveryForm_Shown);
            this.ResizeBegin += new System.EventHandler(this.EDDiscoveryForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.EDDiscoveryForm_ResizeEnd);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EDDiscoveryForm_MouseDown);
            this.Resize += new System.EventHandler(this.EDDiscoveryForm_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.notifyIconContextMenuStrip.ResumeLayout(false);
            this.panelToolBar.ResumeLayout(false);
            this.contextMenuStripTabs.ResumeLayout(false);
            this.panelMenuTop.ResumeLayout(false);
            this.panelMenuTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtButton buttonReloadActions;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eDDiscoveryHomepageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frontierForumThreadToolStripMenuItem;
        private System.Windows.Forms.Label labelInfoBoxTop;
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
        private ExtendedControls.ExtPanelDrawn panel_close;
        private ExtendedControls.ExtPanelDrawn panel_minimize;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private ExtendedControls.ExtStatusStrip statusStrip;
        private System.Windows.Forms.ToolStripMenuItem show3DMapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Timer edsmRefreshTimer;
        private System.Windows.Forms.ToolStripMenuItem read21AndFormerLogFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rescanAllJournalFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForNewReleaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteDuplicateFSDJumpEntriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendUnsuncedEDDNEventsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllPopoutsInTaskBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllInTaskBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOffAllTransparencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearEDSMIDAssignedToAllRecordsForCurrentCommanderToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip notifyIconContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenu_Exit;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenu_Hide;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenu_Open;
        private System.Windows.Forms.ToolStripMenuItem addOnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageAddOnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureAddOnActionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopCurrentlyRunningActionProgramToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator helpMenuSeparatorTop;
        private System.Windows.Forms.ToolStripSeparator helpMenuSeparatorBottom;
        private System.Windows.Forms.ToolStripMenuItem editLastActionPackToolStripMenuItem;
        private ExtendedControls.ExtPanelRollUp panelToolBar;
        private ExtendedControls.ExtComboBox comboBoxCommander;
        private ExtendedControls.ExtButton buttonExt3dmap;
        private ExtendedControls.ExtButton buttonExt2dmap;
        private ExtendedControls.ExtButton buttonExtRefresh;
        private ExtendedControls.ExtButton buttonExtEDSMSync;
        private System.Windows.Forms.ToolStripMenuItem sendUnsyncedEGOScansToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonExtEditAddOns;
        private ExtendedControls.ExtButton buttonExtManageAddOns;
        private System.Windows.Forms.ToolStripMenuItem howToRunInSafeModeToResetVariousParametersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportVistedStarsListToEliteDangerousToolStripMenuItem;
        private ExtendedControls.ExtButton buttonExtPopOut;
        private EDDiscovery.MajorTabControl tabControlMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTabs;
        private System.Windows.Forms.ToolStripMenuItem addTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameTabToolStripMenuItem;
        private System.Windows.Forms.Panel panelMenuTop;
        private System.Windows.Forms.ToolStripMenuItem sendHistoricDataToInaraToolStripMenuItem;
        private ExtendedControls.ExtComboBox comboBoxCustomProfiles;
        private System.Windows.Forms.ToolStripMenuItem popOutPanelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fetchLogsAgainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fetchStarDataAgainToolStripMenuItem;
    }
}
