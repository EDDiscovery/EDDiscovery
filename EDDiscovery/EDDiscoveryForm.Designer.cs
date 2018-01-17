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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
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
            this.showLogfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.read21AndFormerLogFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanAllJournalFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendUnsuncedEDDNEventsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendUnsyncedEGOScansToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.buttonReloadActions = new ExtendedControls.ButtonExt();
            this.panel_minimize = new ExtendedControls.DrawnPanel();
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.statusStrip1 = new ExtendedControls.StatusStripCustom();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyIconMenu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenu_Hide = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.panelToolBar = new ExtendedControls.RollUpPanel();
            this.comboBoxCommander = new ExtendedControls.ComboBoxCustom();
            this.buttonExtEDSMSync = new ExtendedControls.ButtonExt();
            this.buttonExtPopOut = new ExtendedControls.ButtonExt();
            this.buttonExtEditAddOns = new ExtendedControls.ButtonExt();
            this.buttonExtManageAddOns = new ExtendedControls.ButtonExt();
            this.buttonExtRefresh = new ExtendedControls.ButtonExt();
            this.buttonExt2dmap = new ExtendedControls.ButtonExt();
            this.buttonExt3dmap = new ExtendedControls.ButtonExt();
            this.panel_eddiscovery = new System.Windows.Forms.Panel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControlMain = new ExtendedControls.TabControlCustom();
            this.contextMenuStripTabs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPageTravelHistory = new System.Windows.Forms.TabPage();
            this.travelHistoryControl = new EDDiscovery.UserControls.UserControlHistory();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.notifyIconContextMenuStrip1.SuspendLayout();
            this.panelToolBar.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.contextMenuStripTabs.SuspendLayout();
            this.tabPageTravelHistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.addOnsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(2, 2);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(313, 24);
            this.menuStrip1.TabIndex = 16;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownCAPTION);
            this.menuStrip1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
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
            this.syncEDSMSystemsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Main_Admin_EDSMSystemsSync;
            this.syncEDSMSystemsToolStripMenuItem.Name = "syncEDSMSystemsToolStripMenuItem";
            this.syncEDSMSystemsToolStripMenuItem.Size = new System.Drawing.Size(396, 22);
            this.syncEDSMSystemsToolStripMenuItem.Text = "Synchronise with EDSM Stars";
            this.syncEDSMSystemsToolStripMenuItem.Click += new System.EventHandler(this.syncEDSMSystemsToolStripMenuItem_Click);
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
            this.labelInfoBoxTop.Size = new System.Drawing.Size(108, 13);
            this.labelInfoBoxTop.TabIndex = 0;
            this.labelInfoBoxTop.Text = "Loading. Please wait!";
            this.labelInfoBoxTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelInfoBoxTop_MouseDown);
            this.labelInfoBoxTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // label_version
            // 
            this.label_version.AutoSize = true;
            this.label_version.Location = new System.Drawing.Point(296, 4);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(71, 13);
            this.label_version.TabIndex = 21;
            this.label_version.Text = "Version Label";
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
            this.buttonReloadActions.BorderColorScaling = 1.25F;
            this.buttonReloadActions.ButtonColorScaling = 0.5F;
            this.buttonReloadActions.ButtonDisabledScaling = 0.5F;
            this.buttonReloadActions.Location = new System.Drawing.Point(651, 4);
            this.buttonReloadActions.Name = "buttonReloadActions";
            this.buttonReloadActions.Size = new System.Drawing.Size(71, 23);
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
            this.panel_minimize.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(952, 2);
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 20;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_minimize_MouseClick);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(977, 2);
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 19;
            this.panel_close.TabStop = false;
            this.panel_close.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_close_MouseClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip1.Location = new System.Drawing.Point(0, 512);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1004, 22);
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
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.notifyIconContextMenuStrip1;
            this.notifyIcon1.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.notifyIcon1.Text = "EDDiscovery";
            this.notifyIcon1.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
            // 
            // notifyIconContextMenuStrip1
            // 
            this.notifyIconContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.notifyIconMenu_Open,
            this.notifyIconMenu_Hide,
            this.notifyIconMenu_Exit});
            this.notifyIconContextMenuStrip1.Name = "notifyIconContextMenuStrip1";
            this.notifyIconContextMenuStrip1.Size = new System.Drawing.Size(172, 70);
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
            this.panelToolBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelToolBar.BackColor = System.Drawing.Color.Transparent;
            this.panelToolBar.Controls.Add(this.comboBoxCommander);
            this.panelToolBar.Controls.Add(this.buttonExtEDSMSync);
            this.panelToolBar.Controls.Add(this.buttonExtPopOut);
            this.panelToolBar.Controls.Add(this.buttonExtEditAddOns);
            this.panelToolBar.Controls.Add(this.buttonExtManageAddOns);
            this.panelToolBar.Controls.Add(this.buttonExtRefresh);
            this.panelToolBar.Controls.Add(this.buttonExt2dmap);
            this.panelToolBar.Controls.Add(this.buttonExt3dmap);
            this.panelToolBar.Controls.Add(this.buttonReloadActions);
            this.panelToolBar.HiddenMarkerWidth = 0;
            this.panelToolBar.Location = new System.Drawing.Point(2, 26);
            this.panelToolBar.Name = "panelToolBar";
            this.panelToolBar.PinState = true;
            this.panelToolBar.RolledUpHeight = 5;
            this.panelToolBar.RollUpAnimationTime = 500;
            this.panelToolBar.RollUpDelay = 1000;
            this.panelToolBar.ShowHiddenMarker = true;
            this.panelToolBar.Size = new System.Drawing.Size(1000, 32);
            this.panelToolBar.TabIndex = 1;
            this.panelToolBar.UnrolledHeight = 32;
            this.panelToolBar.UnrollHoverDelay = 1000;
            this.panelToolBar.DeployStarting += new System.EventHandler(this.panelToolBar_DeployStarting);
            this.panelToolBar.RetractCompleted += new System.EventHandler(this.panelToolBar_RetractCompleted);
            this.panelToolBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownCAPTION);
            this.panelToolBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            this.panelToolBar.Resize += new System.EventHandler(this.panelToolBar_Resize);
            // 
            // comboBoxCommander
            // 
            this.comboBoxCommander.ArrowWidth = 1;
            this.comboBoxCommander.BorderColor = System.Drawing.Color.White;
            this.comboBoxCommander.ButtonColorScaling = 0.5F;
            this.comboBoxCommander.DataSource = null;
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
            this.comboBoxCommander.ScrollBarWidth = 16;
            this.comboBoxCommander.SelectedIndex = -1;
            this.comboBoxCommander.SelectedItem = null;
            this.comboBoxCommander.SelectedValue = null;
            this.comboBoxCommander.Size = new System.Drawing.Size(149, 21);
            this.comboBoxCommander.TabIndex = 3;
            this.comboBoxCommander.Text = "Cmdr";
            this.toolTip.SetToolTip(this.comboBoxCommander, "Select the commander to view");
            this.comboBoxCommander.ValueMember = "";
            this.comboBoxCommander.SelectedIndexChanged += new System.EventHandler(this.comboBoxCommander_SelectedIndexChanged);
            // 
            // buttonExtEDSMSync
            // 
            this.buttonExtEDSMSync.BorderColorScaling = 1.25F;
            this.buttonExtEDSMSync.ButtonColorScaling = 0.5F;
            this.buttonExtEDSMSync.ButtonDisabledScaling = 0.5F;
            this.buttonExtEDSMSync.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_SyncEDSM;
            this.buttonExtEDSMSync.Location = new System.Drawing.Point(332, 3);
            this.buttonExtEDSMSync.Name = "buttonExtEDSMSync";
            this.buttonExtEDSMSync.Size = new System.Drawing.Size(24, 24);
            this.buttonExtEDSMSync.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtEDSMSync, "Synchronize with EDSM, To and From");
            this.buttonExtEDSMSync.UseVisualStyleBackColor = true;
            this.buttonExtEDSMSync.Click += new System.EventHandler(this.buttonExtEDSMSync_Click);
            // 
            // buttonExtPopOut
            // 
            this.buttonExtPopOut.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtPopOut.BorderColorScaling = 1.25F;
            this.buttonExtPopOut.ButtonColorScaling = 0.5F;
            this.buttonExtPopOut.ButtonDisabledScaling = 0.5F;
            this.buttonExtPopOut.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_Popouts;
            this.buttonExtPopOut.Location = new System.Drawing.Point(362, 3);
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
            this.buttonExtEditAddOns.BorderColorScaling = 1.25F;
            this.buttonExtEditAddOns.ButtonColorScaling = 0.5F;
            this.buttonExtEditAddOns.ButtonDisabledScaling = 0.5F;
            this.buttonExtEditAddOns.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_EditAddons;
            this.buttonExtEditAddOns.Location = new System.Drawing.Point(422, 3);
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
            this.buttonExtManageAddOns.BorderColorScaling = 1.25F;
            this.buttonExtManageAddOns.ButtonColorScaling = 0.5F;
            this.buttonExtManageAddOns.ButtonDisabledScaling = 0.5F;
            this.buttonExtManageAddOns.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_ManageAddOns;
            this.buttonExtManageAddOns.Location = new System.Drawing.Point(392, 3);
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
            this.buttonExtRefresh.BorderColorScaling = 1.25F;
            this.buttonExtRefresh.ButtonColorScaling = 0.5F;
            this.buttonExtRefresh.ButtonDisabledScaling = 0.5F;
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
            this.buttonExt2dmap.BorderColorScaling = 1.25F;
            this.buttonExt2dmap.ButtonColorScaling = 0.5F;
            this.buttonExt2dmap.ButtonDisabledScaling = 0.5F;
            this.buttonExt2dmap.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_Open2DMap;
            this.buttonExt2dmap.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonExt2dmap.Location = new System.Drawing.Point(210, 3);
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
            this.buttonExt3dmap.BorderColorScaling = 1.25F;
            this.buttonExt3dmap.ButtonColorScaling = 0.5F;
            this.buttonExt3dmap.ButtonDisabledScaling = 0.5F;
            this.buttonExt3dmap.Image = global::EDDiscovery.Icons.Controls.Main_Toolbar_Open3DMap;
            this.buttonExt3dmap.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonExt3dmap.Location = new System.Drawing.Point(270, 3);
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
            this.panel_eddiscovery.Location = new System.Drawing.Point(829, 2);
            this.panel_eddiscovery.Name = "panel_eddiscovery";
            this.panel_eddiscovery.Size = new System.Drawing.Size(101, 46);
            this.panel_eddiscovery.TabIndex = 18;
            this.panel_eddiscovery.Click += new System.EventHandler(this.paneleddiscovery_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.AllowDragReorder = true;
            this.tabControlMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlMain.Controls.Add(this.tabPageTravelHistory);
            this.tabControlMain.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tabControlMain.Location = new System.Drawing.Point(2, 58);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1000, 454);
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
            this.removeTabToolStripMenuItem});
            this.contextMenuStripTabs.Name = "contextMenuStripTabs";
            this.contextMenuStripTabs.Size = new System.Drawing.Size(190, 70);
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
            // tabPageTravelHistory
            // 
            this.tabPageTravelHistory.Controls.Add(this.travelHistoryControl);
            this.tabPageTravelHistory.Location = new System.Drawing.Point(4, 22);
            this.tabPageTravelHistory.Name = "tabPageTravelHistory";
            this.tabPageTravelHistory.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTravelHistory.Size = new System.Drawing.Size(996, 430);
            this.tabPageTravelHistory.TabIndex = 0;
            this.tabPageTravelHistory.Text = "History";
            this.tabPageTravelHistory.UseVisualStyleBackColor = true;
            // 
            // travelHistoryControl
            // 
            this.travelHistoryControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.travelHistoryControl.Location = new System.Drawing.Point(3, 3);
            this.travelHistoryControl.Name = "travelHistoryControl";
            this.travelHistoryControl.Size = new System.Drawing.Size(990, 424);
            this.travelHistoryControl.TabIndex = 0;
            // 
            // EDDiscoveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1004, 534);
            this.Controls.Add(this.labelInfoBoxTop);
            this.Controls.Add(this.label_version);
            this.Controls.Add(this.panel_eddiscovery);
            this.Controls.Add(this.panel_minimize);
            this.Controls.Add(this.panel_close);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.panelToolBar);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "EDDiscoveryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EDDiscovery";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EDDiscoveryForm_FormClosing);
            this.Load += new System.EventHandler(this.EDDiscoveryForm_Load);
            this.Shown += new System.EventHandler(this.EDDiscoveryForm_Shown);
            this.ResizeEnd += new System.EventHandler(this.EDDiscoveryForm_ResizeEnd);
            this.Resize += new System.EventHandler(this.EDDiscoveryForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.notifyIconContextMenuStrip1.ResumeLayout(false);
            this.panelToolBar.ResumeLayout(false);
            this.tabControlMain.ResumeLayout(false);
            this.contextMenuStripTabs.ResumeLayout(false);
            this.tabPageTravelHistory.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ButtonExt buttonReloadActions;
        private System.Windows.Forms.MenuStrip menuStrip1;
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
        private ExtendedControls.DrawnPanel panel_close;
        private ExtendedControls.DrawnPanel panel_minimize;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private ExtendedControls.StatusStripCustom statusStrip1;
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
        private System.Windows.Forms.ContextMenuStrip notifyIconContextMenuStrip1;
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
        private ExtendedControls.RollUpPanel panelToolBar;
        private ExtendedControls.ComboBoxCustom comboBoxCommander;
        private ExtendedControls.ButtonExt buttonExt3dmap;
        private ExtendedControls.ButtonExt buttonExt2dmap;
        private ExtendedControls.ButtonExt buttonExtRefresh;
        private ExtendedControls.ButtonExt buttonExtEDSMSync;
        private System.Windows.Forms.ToolStripMenuItem sendUnsyncedEGOScansToolStripMenuItem;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ButtonExt buttonExtEditAddOns;
        private ExtendedControls.ButtonExt buttonExtManageAddOns;
        private System.Windows.Forms.ToolStripMenuItem howToRunInSafeModeToResetVariousParametersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportVistedStarsListToEliteDangerousToolStripMenuItem;
        private ExtendedControls.ButtonExt buttonExtPopOut;
        private ExtendedControls.TabControlCustom tabControlMain;
        private System.Windows.Forms.TabPage tabPageTravelHistory;
        private UserControls.UserControlHistory travelHistoryControl;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTabs;
        private System.Windows.Forms.ToolStripMenuItem addTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
    }
}
