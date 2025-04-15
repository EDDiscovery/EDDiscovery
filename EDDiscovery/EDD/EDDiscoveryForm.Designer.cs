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
            this.notifyIconEDD = new System.Windows.Forms.NotifyIcon(this.components);
            this.notifyIconContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.notifyIconMenu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenu_Hide = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIconMenu_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControlMain = new EDDiscovery.MajorTabControl();
            this.comboBoxCommander = new ExtendedControls.ExtComboBox();
            this.comboBoxCustomProfiles = new ExtendedControls.ExtComboBox();
            this.buttonExtRefresh = new ExtendedControls.ExtButton();
            this.buttonExtManageAddOns = new ExtendedControls.ExtButton();
            this.buttonExtEditAddOns = new ExtendedControls.ExtButton();
            this.buttonExtPopOut = new ExtendedControls.ExtButton();
            this.contextMenuStripTabs = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.popOutPanelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpTabToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelToolBar = new ExtendedControls.ExtPanelRollUp();
            this.extButtonNewFeature = new ExtendedControls.ExtButton();
            this.flowToolBar = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonReloadActions = new ExtendedControls.ExtButton();
            this.extButtonCAPI = new ExtendedControls.ExtButton();
            this.extButtonDrawnHelp = new ExtendedControls.ExtButtonDrawn();
            this.statusStripEDD = new ExtendedControls.ExtStatusStrip();
            this.toolStripProgressBarEDD = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabelEDD = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.menuFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllPopoutsInTaskBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllInTaskBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.turnOffAllTransparencyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.adminToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncEDSMSystemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendUnsyncedEDSMJournalsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fetchLogsAgainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rescanAllJournalFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendHistoricDataToInaraToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rebuildUserDBIndexesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rebuildSystemDBIndexesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showLogfilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fetchStarDataAgainToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.read21AndFormerLogFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.load21ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addOnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.manageAddOnsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureAddOnActionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editLastActionPackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopCurrentlyRunningActionProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wikiHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewHelpVideosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuSeparatorTop = new System.Windows.Forms.ToolStripSeparator();
            this.eDDiscoveryChatDiscordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frontierForumThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gitHubToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportIssueIdeasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemListBindings = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuSeparatorBottom = new System.Windows.Forms.ToolStripSeparator();
            this.checkForNewReleaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label_version = new System.Windows.Forms.Label();
            this.labelInfoBoxTop = new System.Windows.Forms.Label();
            this.labelGameDateTime = new System.Windows.Forms.Label();
            this.closeminimizeFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.panel_close = new ExtendedControls.ExtButtonDrawn();
            this.panel_minimize = new ExtendedControls.ExtButtonDrawn();
            this.panel_eddiscovery = new System.Windows.Forms.Panel();
            this.extPanelTopResizer = new ExtendedControls.ExtPanelResizer();
            this.notifyIconContextMenuStrip.SuspendLayout();
            this.contextMenuStripTabs.SuspendLayout();
            this.panelToolBar.SuspendLayout();
            this.flowToolBar.SuspendLayout();
            this.statusStripEDD.SuspendLayout();
            this.tableLayoutPanelTop.SuspendLayout();
            this.menuFlowPanel.SuspendLayout();
            this.mainMenu.SuspendLayout();
            this.closeminimizeFlowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIconEDD
            // 
            this.notifyIconEDD.ContextMenuStrip = this.notifyIconContextMenuStrip;
            this.notifyIconEDD.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.notifyIconEDD.Text = "EDDiscovery";
            this.notifyIconEDD.DoubleClick += new System.EventHandler(this.notifyIcon1_DoubleClick);
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
            // tabControlMain
            // 
            this.tabControlMain.AllowDragReorder = true;
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.tabControlMain.Location = new System.Drawing.Point(0, 62);
            this.tabControlMain.Multiline = true;
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(984, 627);
            this.tabControlMain.TabControlBorderColor = System.Drawing.Color.DarkGray;
            this.tabControlMain.TabDisabledScaling = 0.5F;
            this.tabControlMain.TabIndex = 15;
            this.tabControlMain.TabMouseOverColor = System.Drawing.Color.White;
            this.tabControlMain.TabNotSelectedBorderColor = System.Drawing.Color.Gray;
            this.tabControlMain.TabNotSelectedColor = System.Drawing.Color.Gray;
            this.tabControlMain.TabSelectedColor = System.Drawing.Color.LightGray;
            this.tabControlMain.TabStyle = tabStyleSquare1;
            this.tabControlMain.TextNotSelectedColor = System.Drawing.SystemColors.ControlText;
            this.tabControlMain.TextSelectedColor = System.Drawing.SystemColors.ControlText;
            this.toolTip.SetToolTip(this.tabControlMain, "Right click to add/remove tabs, Left click drag to reorder");
            this.tabControlMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControlMain_MouseClick);
            // 
            // comboBoxCommander
            // 
            this.comboBoxCommander.BorderColor = System.Drawing.Color.White;
            this.comboBoxCommander.DataSource = null;
            this.comboBoxCommander.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCommander.DisplayMember = "";
            this.comboBoxCommander.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCommander.Location = new System.Drawing.Point(1, 3);
            this.comboBoxCommander.Margin = new System.Windows.Forms.Padding(1, 3, 3, 1);
            this.comboBoxCommander.Name = "comboBoxCommander";
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
            // comboBoxCustomProfiles
            // 
            this.comboBoxCustomProfiles.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomProfiles.DataSource = null;
            this.comboBoxCustomProfiles.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomProfiles.DisplayMember = "";
            this.comboBoxCustomProfiles.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomProfiles.Location = new System.Drawing.Point(186, 3);
            this.comboBoxCustomProfiles.Margin = new System.Windows.Forms.Padding(1, 3, 3, 1);
            this.comboBoxCustomProfiles.Name = "comboBoxCustomProfiles";
            this.comboBoxCustomProfiles.SelectedIndex = -1;
            this.comboBoxCustomProfiles.SelectedItem = null;
            this.comboBoxCustomProfiles.SelectedValue = null;
            this.comboBoxCustomProfiles.Size = new System.Drawing.Size(100, 21);
            this.comboBoxCustomProfiles.TabIndex = 4;
            this.comboBoxCustomProfiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCustomProfiles, "Use to select new profile or edit profile settings");
            this.comboBoxCustomProfiles.ValueMember = "";
            // 
            // buttonExtRefresh
            // 
            this.buttonExtRefresh.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtRefresh.Image = global::EDDiscovery.Icons.Controls.Refresh;
            this.buttonExtRefresh.Location = new System.Drawing.Point(154, 1);
            this.buttonExtRefresh.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.buttonExtRefresh.Name = "buttonExtRefresh";
            this.buttonExtRefresh.Size = new System.Drawing.Size(28, 28);
            this.buttonExtRefresh.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtRefresh, "Refresh the history");
            this.buttonExtRefresh.UseVisualStyleBackColor = false;
            this.buttonExtRefresh.Click += new System.EventHandler(this.buttonExtRefresh_Click);
            // 
            // buttonExtManageAddOns
            // 
            this.buttonExtManageAddOns.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtManageAddOns.Image = global::EDDiscovery.Icons.Controls.ManageAddOns;
            this.buttonExtManageAddOns.Location = new System.Drawing.Point(290, 1);
            this.buttonExtManageAddOns.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.buttonExtManageAddOns.Name = "buttonExtManageAddOns";
            this.buttonExtManageAddOns.Size = new System.Drawing.Size(28, 28);
            this.buttonExtManageAddOns.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtManageAddOns, "Manage Add-Ons");
            this.buttonExtManageAddOns.UseVisualStyleBackColor = false;
            this.buttonExtManageAddOns.Click += new System.EventHandler(this.buttonExtManageAddOns_Click);
            // 
            // buttonExtEditAddOns
            // 
            this.buttonExtEditAddOns.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtEditAddOns.Image = global::EDDiscovery.Icons.Controls.EditAddons;
            this.buttonExtEditAddOns.Location = new System.Drawing.Point(322, 1);
            this.buttonExtEditAddOns.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.buttonExtEditAddOns.Name = "buttonExtEditAddOns";
            this.buttonExtEditAddOns.Size = new System.Drawing.Size(28, 28);
            this.buttonExtEditAddOns.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtEditAddOns, "Edit Add-Ons");
            this.buttonExtEditAddOns.UseVisualStyleBackColor = false;
            this.buttonExtEditAddOns.Click += new System.EventHandler(this.buttonExtEditAddOns_Click);
            // 
            // buttonExtPopOut
            // 
            this.buttonExtPopOut.BackColor = System.Drawing.Color.Transparent;
            this.buttonExtPopOut.Image = global::EDDiscovery.Icons.Controls.Popout;
            this.buttonExtPopOut.Location = new System.Drawing.Point(354, 1);
            this.buttonExtPopOut.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.buttonExtPopOut.Name = "buttonExtPopOut";
            this.buttonExtPopOut.Size = new System.Drawing.Size(28, 28);
            this.buttonExtPopOut.TabIndex = 2;
            this.toolTip.SetToolTip(this.buttonExtPopOut, "Click to select a pop out panel to display");
            this.buttonExtPopOut.UseVisualStyleBackColor = false;
            this.buttonExtPopOut.Click += new System.EventHandler(this.buttonExtPopOut_Click);
            // 
            // contextMenuStripTabs
            // 
            this.contextMenuStripTabs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addTabToolStripMenuItem,
            this.removeTabToolStripMenuItem,
            this.renameTabToolStripMenuItem,
            this.popOutPanelToolStripMenuItem,
            this.helpTabToolStripMenuItem});
            this.contextMenuStripTabs.Name = "contextMenuStripTabs";
            this.contextMenuStripTabs.Size = new System.Drawing.Size(189, 114);
            this.contextMenuStripTabs.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStripTabs_Opening);
            // 
            // addTabToolStripMenuItem
            // 
            this.addTabToolStripMenuItem.Name = "addTabToolStripMenuItem";
            this.addTabToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.addTabToolStripMenuItem.Text = "Insert Tab with panel..";
            // 
            // removeTabToolStripMenuItem
            // 
            this.removeTabToolStripMenuItem.Name = "removeTabToolStripMenuItem";
            this.removeTabToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.removeTabToolStripMenuItem.Text = "Remove Tab";
            // 
            // renameTabToolStripMenuItem
            // 
            this.renameTabToolStripMenuItem.Name = "renameTabToolStripMenuItem";
            this.renameTabToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.renameTabToolStripMenuItem.Text = "Rename Tab";
            // 
            // popOutPanelToolStripMenuItem
            // 
            this.popOutPanelToolStripMenuItem.Name = "popOutPanelToolStripMenuItem";
            this.popOutPanelToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.popOutPanelToolStripMenuItem.Text = "Pop Out Panel..";
            // 
            // helpTabToolStripMenuItem
            // 
            this.helpTabToolStripMenuItem.Name = "helpTabToolStripMenuItem";
            this.helpTabToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.helpTabToolStripMenuItem.Text = "Help";
            // 
            // panelToolBar
            // 
            this.panelToolBar.AutoHeight = false;
            this.panelToolBar.AutoHeightWidthDisable = false;
            this.panelToolBar.AutoSize = true;
            this.panelToolBar.AutoWidth = false;
            this.panelToolBar.BackColor = System.Drawing.Color.Transparent;
            this.panelToolBar.Controls.Add(this.extButtonNewFeature);
            this.panelToolBar.Controls.Add(this.flowToolBar);
            this.panelToolBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelToolBar.HiddenMarkerWidth = 0;
            this.panelToolBar.Location = new System.Drawing.Point(0, 28);
            this.panelToolBar.Margin = new System.Windows.Forms.Padding(0);
            this.panelToolBar.Name = "panelToolBar";
            this.panelToolBar.PinState = true;
            this.panelToolBar.RolledUpHeight = 5;
            this.panelToolBar.RollUpAnimationTime = 500;
            this.panelToolBar.RollUpDelay = 1000;
            this.panelToolBar.SecondHiddenMarkerWidth = 0;
            this.panelToolBar.ShowHiddenMarker = true;
            this.panelToolBar.Size = new System.Drawing.Size(984, 34);
            this.panelToolBar.TabIndex = 1;
            this.panelToolBar.UnrollHoverDelay = 1000;
            // 
            // extButtonNewFeature
            // 
            this.extButtonNewFeature.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonNewFeature.Image = global::EDDiscovery.Icons.Controls.NewFeature;
            this.extButtonNewFeature.Location = new System.Drawing.Point(902, 3);
            this.extButtonNewFeature.Name = "extButtonNewFeature";
            this.extButtonNewFeature.Size = new System.Drawing.Size(48, 28);
            this.extButtonNewFeature.TabIndex = 6;
            this.extButtonNewFeature.UseVisualStyleBackColor = true;
            this.extButtonNewFeature.Click += new System.EventHandler(this.extButtonNewFeature_Click);
            // 
            // flowToolBar
            // 
            this.flowToolBar.AutoSize = true;
            this.flowToolBar.Controls.Add(this.comboBoxCommander);
            this.flowToolBar.Controls.Add(this.buttonExtRefresh);
            this.flowToolBar.Controls.Add(this.comboBoxCustomProfiles);
            this.flowToolBar.Controls.Add(this.buttonExtManageAddOns);
            this.flowToolBar.Controls.Add(this.buttonExtEditAddOns);
            this.flowToolBar.Controls.Add(this.buttonExtPopOut);
            this.flowToolBar.Controls.Add(this.buttonReloadActions);
            this.flowToolBar.Controls.Add(this.extButtonCAPI);
            this.flowToolBar.Controls.Add(this.extButtonDrawnHelp);
            this.flowToolBar.Location = new System.Drawing.Point(0, 0);
            this.flowToolBar.Margin = new System.Windows.Forms.Padding(0);
            this.flowToolBar.Name = "flowToolBar";
            this.flowToolBar.Size = new System.Drawing.Size(624, 34);
            this.flowToolBar.TabIndex = 5;
            this.flowToolBar.WrapContents = false;
            // 
            // buttonReloadActions
            // 
            this.buttonReloadActions.Image = global::EDDiscovery.Icons.Controls.Refresh;
            this.buttonReloadActions.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonReloadActions.Location = new System.Drawing.Point(386, 1);
            this.buttonReloadActions.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.buttonReloadActions.Name = "buttonReloadActions";
            this.buttonReloadActions.Size = new System.Drawing.Size(65, 28);
            this.buttonReloadActions.TabIndex = 1;
            this.buttonReloadActions.Text = "Actions";
            this.buttonReloadActions.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonReloadActions.UseVisualStyleBackColor = true;
            this.buttonReloadActions.Visible = false;
            this.buttonReloadActions.Click += new System.EventHandler(this.buttonReloadActions_Click);
            // 
            // extButtonCAPI
            // 
            this.extButtonCAPI.Image = global::EDDiscovery.Icons.Controls.Refresh;
            this.extButtonCAPI.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extButtonCAPI.Location = new System.Drawing.Point(455, 1);
            this.extButtonCAPI.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.extButtonCAPI.Name = "extButtonCAPI";
            this.extButtonCAPI.Size = new System.Drawing.Size(65, 28);
            this.extButtonCAPI.TabIndex = 1;
            this.extButtonCAPI.Text = "CAPI";
            this.extButtonCAPI.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.extButtonCAPI.UseVisualStyleBackColor = true;
            this.extButtonCAPI.Visible = false;
            this.extButtonCAPI.Click += new System.EventHandler(this.extButtonCAPI_Click);
            // 
            // extButtonDrawnHelp
            // 
            this.extButtonDrawnHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelp.AutoEllipsis = false;
            this.extButtonDrawnHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelp.Image = null;
            this.extButtonDrawnHelp.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelp.Location = new System.Drawing.Point(524, 1);
            this.extButtonDrawnHelp.Margin = new System.Windows.Forms.Padding(1, 1, 3, 1);
            this.extButtonDrawnHelp.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelp.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelp.Name = "extButtonDrawnHelp";
            this.extButtonDrawnHelp.Padding = new System.Windows.Forms.Padding(0);
            this.extButtonDrawnHelp.Selectable = true;
            this.extButtonDrawnHelp.Size = new System.Drawing.Size(28, 28);
            this.extButtonDrawnHelp.TabIndex = 25;
            this.extButtonDrawnHelp.Text = "?";
            this.extButtonDrawnHelp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelp.UseMnemonic = true;
            this.extButtonDrawnHelp.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // statusStripEDD
            // 
            this.statusStripEDD.AutoSize = false;
            this.statusStripEDD.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.statusStripEDD.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBarEDD,
            this.toolStripStatusLabelEDD});
            this.statusStripEDD.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStripEDD.Location = new System.Drawing.Point(0, 689);
            this.statusStripEDD.Name = "statusStripEDD";
            this.statusStripEDD.Size = new System.Drawing.Size(984, 22);
            this.statusStripEDD.TabIndex = 22;
            // 
            // toolStripProgressBarEDD
            // 
            this.toolStripProgressBarEDD.MarqueeAnimationSpeed = 10;
            this.toolStripProgressBarEDD.Name = "toolStripProgressBarEDD";
            this.toolStripProgressBarEDD.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabelEDD
            // 
            this.toolStripStatusLabelEDD.Name = "toolStripStatusLabelEDD";
            this.toolStripStatusLabelEDD.Size = new System.Drawing.Size(0, 17);
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.AutoSize = true;
            this.tableLayoutPanelTop.ColumnCount = 2;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTop.Controls.Add(this.menuFlowPanel, 0, 0);
            this.tableLayoutPanelTop.Controls.Add(this.closeminimizeFlowPanel, 1, 0);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 3);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 1;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(984, 25);
            this.tableLayoutPanelTop.TabIndex = 23;
            this.tableLayoutPanelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownCAPTION);
            this.tableLayoutPanelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // menuFlowPanel
            // 
            this.menuFlowPanel.AutoSize = true;
            this.menuFlowPanel.Controls.Add(this.mainMenu);
            this.menuFlowPanel.Controls.Add(this.label_version);
            this.menuFlowPanel.Controls.Add(this.labelInfoBoxTop);
            this.menuFlowPanel.Controls.Add(this.labelGameDateTime);
            this.menuFlowPanel.Location = new System.Drawing.Point(0, 0);
            this.menuFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.menuFlowPanel.Name = "menuFlowPanel";
            this.menuFlowPanel.Size = new System.Drawing.Size(376, 24);
            this.menuFlowPanel.TabIndex = 23;
            this.menuFlowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownCAPTION);
            this.menuFlowPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // mainMenu
            // 
            this.mainMenu.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mainMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.adminToolStripMenuItem,
            this.addOnsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Padding = new System.Windows.Forms.Padding(0);
            this.mainMenu.Size = new System.Drawing.Size(214, 24);
            this.mainMenu.TabIndex = 16;
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.settingsToolStripMenuItem,
            this.showAllPopoutsInTaskBarToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Settings;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
            // 
            // showAllPopoutsInTaskBarToolStripMenuItem
            // 
            this.showAllPopoutsInTaskBarToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showAllInTaskBarToolStripMenuItem,
            this.turnOffAllTransparencyToolStripMenuItem});
            this.showAllPopoutsInTaskBarToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Menu;
            this.showAllPopoutsInTaskBarToolStripMenuItem.Name = "showAllPopoutsInTaskBarToolStripMenuItem";
            this.showAllPopoutsInTaskBarToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.showAllPopoutsInTaskBarToolStripMenuItem.Text = "&Pop-outs";
            // 
            // showAllInTaskBarToolStripMenuItem
            // 
            this.showAllInTaskBarToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.ShowAllInTaskbar;
            this.showAllInTaskBarToolStripMenuItem.Name = "showAllInTaskBarToolStripMenuItem";
            this.showAllInTaskBarToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.showAllInTaskBarToolStripMenuItem.Text = "&Show All In Task Bar";
            this.showAllInTaskBarToolStripMenuItem.Click += new System.EventHandler(this.showAllInTaskBarToolStripMenuItem_Click);
            // 
            // turnOffAllTransparencyToolStripMenuItem
            // 
            this.turnOffAllTransparencyToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.DisableTransparency;
            this.turnOffAllTransparencyToolStripMenuItem.Name = "turnOffAllTransparencyToolStripMenuItem";
            this.turnOffAllTransparencyToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.turnOffAllTransparencyToolStripMenuItem.Text = "&Turn Off All Transparency";
            this.turnOffAllTransparencyToolStripMenuItem.Click += new System.EventHandler(this.turnOffAllTransparencyToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Exit;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(123, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // adminToolStripMenuItem
            // 
            this.adminToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.syncEDSMSystemsToolStripMenuItem,
            this.rescanAllJournalFilesToolStripMenuItem,
            this.sendHistoricDataToInaraToolStripMenuItem,
            this.rebuildUserDBIndexesToolStripMenuItem,
            this.rebuildSystemDBIndexesToolStripMenuItem,
            this.updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem,
            this.showLogfilesToolStripMenuItem,
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem,
            this.fetchStarDataAgainToolStripMenuItem,
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem,
            this.read21AndFormerLogFilesToolStripMenuItem});
            this.adminToolStripMenuItem.Name = "adminToolStripMenuItem";
            this.adminToolStripMenuItem.Size = new System.Drawing.Size(55, 24);
            this.adminToolStripMenuItem.Text = "A&dmin";
            // 
            // syncEDSMSystemsToolStripMenuItem
            // 
            this.syncEDSMSystemsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sendUnsyncedEDSMJournalsToolStripMenuItem,
            this.fetchLogsAgainToolStripMenuItem});
            this.syncEDSMSystemsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.EDSM;
            this.syncEDSMSystemsToolStripMenuItem.Name = "syncEDSMSystemsToolStripMenuItem";
            this.syncEDSMSystemsToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.syncEDSMSystemsToolStripMenuItem.Text = "Synchronise with EDSM";
            // 
            // sendUnsyncedEDSMJournalsToolStripMenuItem
            // 
            this.sendUnsyncedEDSMJournalsToolStripMenuItem.Name = "sendUnsyncedEDSMJournalsToolStripMenuItem";
            this.sendUnsyncedEDSMJournalsToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.sendUnsyncedEDSMJournalsToolStripMenuItem.Text = "Send Unsynced Journals To EDSM";
            this.sendUnsyncedEDSMJournalsToolStripMenuItem.Click += new System.EventHandler(this.sendUnsyncedEDSMJournalsToolStripMenuItem_Click);
            // 
            // fetchLogsAgainToolStripMenuItem
            // 
            this.fetchLogsAgainToolStripMenuItem.Name = "fetchLogsAgainToolStripMenuItem";
            this.fetchLogsAgainToolStripMenuItem.Size = new System.Drawing.Size(250, 22);
            this.fetchLogsAgainToolStripMenuItem.Text = "Fetch Logs Again";
            this.fetchLogsAgainToolStripMenuItem.Click += new System.EventHandler(this.fetchLogsAgainToolStripMenuItem_Click);
            // 
            // rescanAllJournalFilesToolStripMenuItem
            // 
            this.rescanAllJournalFilesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.RescanJournals;
            this.rescanAllJournalFilesToolStripMenuItem.Name = "rescanAllJournalFilesToolStripMenuItem";
            this.rescanAllJournalFilesToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.rescanAllJournalFilesToolStripMenuItem.Text = "Re-scan all journal files";
            this.rescanAllJournalFilesToolStripMenuItem.Click += new System.EventHandler(this.rescanAllJournalFilesToolStripMenuItem_Click);
            // 
            // sendHistoricDataToInaraToolStripMenuItem
            // 
            this.sendHistoricDataToInaraToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.InaraDark;
            this.sendHistoricDataToInaraToolStripMenuItem.Name = "sendHistoricDataToInaraToolStripMenuItem";
            this.sendHistoricDataToInaraToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.sendHistoricDataToInaraToolStripMenuItem.Text = "Send to Inara historic data (previous ships, stored modules)";
            this.sendHistoricDataToInaraToolStripMenuItem.Click += new System.EventHandler(this.sendHistoricDataToInaraToolStripMenuItem_Click);
            // 
            // rebuildUserDBIndexesToolStripMenuItem
            // 
            this.rebuildUserDBIndexesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.RescanJournals;
            this.rebuildUserDBIndexesToolStripMenuItem.Name = "rebuildUserDBIndexesToolStripMenuItem";
            this.rebuildUserDBIndexesToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.rebuildUserDBIndexesToolStripMenuItem.Text = "Rebuild User DB Indexes";
            this.rebuildUserDBIndexesToolStripMenuItem.Click += new System.EventHandler(this.rebuildUserDBIndexesToolStripMenuItem_Click);
            // 
            // rebuildSystemDBIndexesToolStripMenuItem
            // 
            this.rebuildSystemDBIndexesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.RescanJournals;
            this.rebuildSystemDBIndexesToolStripMenuItem.Name = "rebuildSystemDBIndexesToolStripMenuItem";
            this.rebuildSystemDBIndexesToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.rebuildSystemDBIndexesToolStripMenuItem.Text = "Rebuild System DB Indexes";
            this.rebuildSystemDBIndexesToolStripMenuItem.Click += new System.EventHandler(this.rebuildSystemDBIndexesToolStripMenuItem_Click);
            // 
            // updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem
            // 
            this.updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.FSDJumps;
            this.updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem.Name = "updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem";
            this.updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem.Text = "Update systems with unknown co-ordinates";
            this.updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem.Click += new System.EventHandler(this.updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem_Click);
            // 
            // showLogfilesToolStripMenuItem
            // 
            this.showLogfilesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.ShowLogFiles;
            this.showLogfilesToolStripMenuItem.Name = "showLogfilesToolStripMenuItem";
            this.showLogfilesToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.showLogfilesToolStripMenuItem.Text = "Show journal files directory of current commander";
            this.showLogfilesToolStripMenuItem.Click += new System.EventHandler(this.showLogfilesToolStripMenuItem_Click);
            // 
            // dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem
            // 
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Warning;
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Name = "dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem";
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Text = "Reset all history to current commander";
            this.dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem.Click += new System.EventHandler(this.debugResetAllHistoryToFirstCommanderToolStripMenuItem_Click);
            // 
            // fetchStarDataAgainToolStripMenuItem
            // 
            this.fetchStarDataAgainToolStripMenuItem.Name = "fetchStarDataAgainToolStripMenuItem";
            this.fetchStarDataAgainToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.fetchStarDataAgainToolStripMenuItem.Text = "Fetch Star Data Again";
            this.fetchStarDataAgainToolStripMenuItem.Click += new System.EventHandler(this.syncStarDataSystemsToolStripMenuItem_Click);
            // 
            // deleteDuplicateFSDJumpEntriesToolStripMenuItem
            // 
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.FSDJumps;
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Name = "deleteDuplicateFSDJumpEntriesToolStripMenuItem";
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Text = "Delete duplicate FSD Jump entries";
            this.deleteDuplicateFSDJumpEntriesToolStripMenuItem.Click += new System.EventHandler(this.deleteDuplicateFSDJumpEntriesToolStripMenuItem_Click);
            // 
            // read21AndFormerLogFilesToolStripMenuItem
            // 
            this.read21AndFormerLogFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.load21ToolStripMenuItem,
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem});
            this.read21AndFormerLogFilesToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.ReadNetLogs;
            this.read21AndFormerLogFilesToolStripMenuItem.Name = "read21AndFormerLogFilesToolStripMenuItem";
            this.read21AndFormerLogFilesToolStripMenuItem.Size = new System.Drawing.Size(385, 22);
            this.read21AndFormerLogFilesToolStripMenuItem.Text = "Read 2.1 and former log files";
            // 
            // load21ToolStripMenuItem
            // 
            this.load21ToolStripMenuItem.Name = "load21ToolStripMenuItem";
            this.load21ToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.load21ToolStripMenuItem.Text = "Scan Netlogs";
            this.load21ToolStripMenuItem.Click += new System.EventHandler(this.read21AndFormerLogFilesToolStripMenuItem_Click);
            // 
            // read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem
            // 
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem.Name = "read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem";
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem.Text = "Force reload logs";
            this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem.Click += new System.EventHandler(this.read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem_Click);
            // 
            // addOnsToolStripMenuItem
            // 
            this.addOnsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.manageAddOnsToolStripMenuItem,
            this.configureAddOnActionsToolStripMenuItem,
            this.editLastActionPackToolStripMenuItem,
            this.stopCurrentlyRunningActionProgramToolStripMenuItem});
            this.addOnsToolStripMenuItem.Name = "addOnsToolStripMenuItem";
            this.addOnsToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.addOnsToolStripMenuItem.Text = "&Add-Ons";
            // 
            // manageAddOnsToolStripMenuItem
            // 
            this.manageAddOnsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.ManageAddOns;
            this.manageAddOnsToolStripMenuItem.Name = "manageAddOnsToolStripMenuItem";
            this.manageAddOnsToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.manageAddOnsToolStripMenuItem.Text = "&Manage Add-Ons";
            this.manageAddOnsToolStripMenuItem.Click += new System.EventHandler(this.manageAddOnsToolStripMenuItem_Click);
            // 
            // configureAddOnActionsToolStripMenuItem
            // 
            this.configureAddOnActionsToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.ConfigureAddOnActions;
            this.configureAddOnActionsToolStripMenuItem.Name = "configureAddOnActionsToolStripMenuItem";
            this.configureAddOnActionsToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.configureAddOnActionsToolStripMenuItem.Text = "&Edit Add-On Action Files";
            this.configureAddOnActionsToolStripMenuItem.Click += new System.EventHandler(this.configureAddOnActionsToolStripMenuItem_Click);
            // 
            // editLastActionPackToolStripMenuItem
            // 
            this.editLastActionPackToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.EditLastActionPack;
            this.editLastActionPackToolStripMenuItem.Name = "editLastActionPackToolStripMenuItem";
            this.editLastActionPackToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.editLastActionPackToolStripMenuItem.Text = "Edit Last Action Pack";
            this.editLastActionPackToolStripMenuItem.Click += new System.EventHandler(this.editLastActionPackToolStripMenuItem_Click);
            // 
            // stopCurrentlyRunningActionProgramToolStripMenuItem
            // 
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Pause;
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Name = "stopCurrentlyRunningActionProgramToolStripMenuItem";
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Size = new System.Drawing.Size(280, 22);
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Text = "&Stop currently running Action Program";
            this.stopCurrentlyRunningActionProgramToolStripMenuItem.Click += new System.EventHandler(this.stopCurrentlyRunningActionProgramToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.wikiHelpToolStripMenuItem,
            this.viewHelpVideosToolStripMenuItem,
            this.helpMenuSeparatorTop,
            this.eDDiscoveryChatDiscordToolStripMenuItem,
            this.frontierForumThreadToolStripMenuItem,
            this.gitHubToolStripMenuItem,
            this.reportIssueIdeasToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItemListBindings,
            this.helpMenuSeparatorBottom,
            this.checkForNewReleaseToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.About;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // wikiHelpToolStripMenuItem
            // 
            this.wikiHelpToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Wiki;
            this.wikiHelpToolStripMenuItem.Name = "wikiHelpToolStripMenuItem";
            this.wikiHelpToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.wikiHelpToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.wikiHelpToolStripMenuItem.Text = "&View Help";
            this.wikiHelpToolStripMenuItem.Click += new System.EventHandler(this.wikiHelpToolStripMenuItem_Click);
            // 
            // viewHelpVideosToolStripMenuItem
            // 
            this.viewHelpVideosToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Video;
            this.viewHelpVideosToolStripMenuItem.Name = "viewHelpVideosToolStripMenuItem";
            this.viewHelpVideosToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F1)));
            this.viewHelpVideosToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.viewHelpVideosToolStripMenuItem.Text = "View Help Videos";
            this.viewHelpVideosToolStripMenuItem.Click += new System.EventHandler(this.viewHelpVideosToolStripMenuItem_Click);
            // 
            // helpMenuSeparatorTop
            // 
            this.helpMenuSeparatorTop.Name = "helpMenuSeparatorTop";
            this.helpMenuSeparatorTop.Size = new System.Drawing.Size(239, 6);
            // 
            // eDDiscoveryChatDiscordToolStripMenuItem
            // 
            this.eDDiscoveryChatDiscordToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.DiscordChat;
            this.eDDiscoveryChatDiscordToolStripMenuItem.Name = "eDDiscoveryChatDiscordToolStripMenuItem";
            this.eDDiscoveryChatDiscordToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.eDDiscoveryChatDiscordToolStripMenuItem.Text = "&Discord - EDD Community Chat";
            this.eDDiscoveryChatDiscordToolStripMenuItem.Click += new System.EventHandler(this.eddiscoveryChatDiscordToolStripMenuItem_Click);
            // 
            // frontierForumThreadToolStripMenuItem
            // 
            this.frontierForumThreadToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.FrontierForumThread;
            this.frontierForumThreadToolStripMenuItem.Name = "frontierForumThreadToolStripMenuItem";
            this.frontierForumThreadToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.frontierForumThreadToolStripMenuItem.Text = "&Frontier Forum Thread";
            this.frontierForumThreadToolStripMenuItem.Click += new System.EventHandler(this.frontierForumThreadToolStripMenuItem_Click);
            // 
            // gitHubToolStripMenuItem
            // 
            this.gitHubToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.Github;
            this.gitHubToolStripMenuItem.Name = "gitHubToolStripMenuItem";
            this.gitHubToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.gitHubToolStripMenuItem.Text = "&Project Page (GitHub)";
            this.gitHubToolStripMenuItem.Click += new System.EventHandler(this.gitHubToolStripMenuItem_Click);
            // 
            // reportIssueIdeasToolStripMenuItem
            // 
            this.reportIssueIdeasToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.ReportIssue;
            this.reportIssueIdeasToolStripMenuItem.Name = "reportIssueIdeasToolStripMenuItem";
            this.reportIssueIdeasToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.reportIssueIdeasToolStripMenuItem.Text = "&Report Issue / Idea";
            this.reportIssueIdeasToolStripMenuItem.Click += new System.EventHandler(this.reportIssueIdeasToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(239, 6);
            // 
            // toolStripMenuItemListBindings
            // 
            this.toolStripMenuItemListBindings.Name = "toolStripMenuItemListBindings";
            this.toolStripMenuItemListBindings.Size = new System.Drawing.Size(242, 22);
            this.toolStripMenuItemListBindings.Text = "List Elite Bindings";
            this.toolStripMenuItemListBindings.Click += new System.EventHandler(this.toolStripMenuItemListBindings_Click);
            // 
            // helpMenuSeparatorBottom
            // 
            this.helpMenuSeparatorBottom.Name = "helpMenuSeparatorBottom";
            this.helpMenuSeparatorBottom.Size = new System.Drawing.Size(239, 6);
            // 
            // checkForNewReleaseToolStripMenuItem
            // 
            this.checkForNewReleaseToolStripMenuItem.Image = global::EDDiscovery.Icons.Controls.CheckForNewRelease;
            this.checkForNewReleaseToolStripMenuItem.Name = "checkForNewReleaseToolStripMenuItem";
            this.checkForNewReleaseToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.checkForNewReleaseToolStripMenuItem.Text = "&Check for Updates";
            this.checkForNewReleaseToolStripMenuItem.Click += new System.EventHandler(this.checkForNewReleaseToolStripMenuItem_Click);
            // 
            // label_version
            // 
            this.label_version.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label_version.AutoSize = true;
            this.label_version.Location = new System.Drawing.Point(222, 6);
            this.label_version.Margin = new System.Windows.Forms.Padding(8, 1, 3, 0);
            this.label_version.Name = "label_version";
            this.label_version.Size = new System.Drawing.Size(43, 13);
            this.label_version.TabIndex = 21;
            this.label_version.Text = "<code>";
            this.label_version.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label_version.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelversion_MouseDown);
            this.label_version.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // labelInfoBoxTop
            // 
            this.labelInfoBoxTop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelInfoBoxTop.AutoSize = true;
            this.labelInfoBoxTop.Location = new System.Drawing.Point(276, 6);
            this.labelInfoBoxTop.Margin = new System.Windows.Forms.Padding(8, 1, 3, 0);
            this.labelInfoBoxTop.Name = "labelInfoBoxTop";
            this.labelInfoBoxTop.Size = new System.Drawing.Size(43, 13);
            this.labelInfoBoxTop.TabIndex = 0;
            this.labelInfoBoxTop.Text = "<code>";
            this.labelInfoBoxTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelversion_MouseDown);
            this.labelInfoBoxTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // labelGameDateTime
            // 
            this.labelGameDateTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelGameDateTime.AutoSize = true;
            this.labelGameDateTime.Location = new System.Drawing.Point(330, 6);
            this.labelGameDateTime.Margin = new System.Windows.Forms.Padding(8, 1, 3, 0);
            this.labelGameDateTime.Name = "labelGameDateTime";
            this.labelGameDateTime.Size = new System.Drawing.Size(43, 13);
            this.labelGameDateTime.TabIndex = 0;
            this.labelGameDateTime.Text = "<code>";
            this.labelGameDateTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.labelversion_MouseDown);
            this.labelGameDateTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // closeminimizeFlowPanel
            // 
            this.closeminimizeFlowPanel.Controls.Add(this.panel_close);
            this.closeminimizeFlowPanel.Controls.Add(this.panel_minimize);
            this.closeminimizeFlowPanel.Controls.Add(this.panel_eddiscovery);
            this.closeminimizeFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.closeminimizeFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.closeminimizeFlowPanel.Location = new System.Drawing.Point(894, 0);
            this.closeminimizeFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.closeminimizeFlowPanel.Name = "closeminimizeFlowPanel";
            this.closeminimizeFlowPanel.Size = new System.Drawing.Size(90, 25);
            this.closeminimizeFlowPanel.TabIndex = 22;
            this.closeminimizeFlowPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MouseDownCAPTION);
            this.closeminimizeFlowPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MouseUpCAPTION);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(74, 2);
            this.panel_close.Margin = new System.Windows.Forms.Padding(0);
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(3);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(16, 16);
            this.panel_close.TabIndex = 20;
            this.panel_close.TabStop = false;
            this.panel_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_close.UseMnemonic = true;
            this.panel_close.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_close_MouseClick);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.AutoEllipsis = false;
            this.panel_minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_minimize.Image = null;
            this.panel_minimize.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(58, 0);
            this.panel_minimize.Margin = new System.Windows.Forms.Padding(0);
            this.panel_minimize.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_minimize.MouseSelectedColorEnable = true;
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(3);
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(16, 16);
            this.panel_minimize.TabIndex = 20;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_minimize.UseMnemonic = true;
            this.panel_minimize.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panel_minimize_MouseClick);
            // 
            // panel_eddiscovery
            // 
            this.panel_eddiscovery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_eddiscovery.BackColor = System.Drawing.SystemColors.Control;
            this.panel_eddiscovery.BackgroundImage = global::EDDiscovery.Properties.Resources.Logo;
            this.panel_eddiscovery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_eddiscovery.Location = new System.Drawing.Point(11, 0);
            this.panel_eddiscovery.Margin = new System.Windows.Forms.Padding(0);
            this.panel_eddiscovery.Name = "panel_eddiscovery";
            this.panel_eddiscovery.Size = new System.Drawing.Size(47, 20);
            this.panel_eddiscovery.TabIndex = 18;
            this.panel_eddiscovery.Click += new System.EventHandler(this.paneleddiscovery_Click);
            // 
            // extPanelTopResizer
            // 
            this.extPanelTopResizer.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.extPanelTopResizer.Dock = System.Windows.Forms.DockStyle.Top;
            this.extPanelTopResizer.Location = new System.Drawing.Point(0, 0);
            this.extPanelTopResizer.Movement = System.Windows.Forms.DockStyle.Top;
            this.extPanelTopResizer.Name = "extPanelTopResizer";
            this.extPanelTopResizer.Size = new System.Drawing.Size(984, 3);
            this.extPanelTopResizer.TabIndex = 22;
            // 
            // EDDiscoveryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 711);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.panelToolBar);
            this.Controls.Add(this.tableLayoutPanelTop);
            this.Controls.Add(this.extPanelTopResizer);
            this.Controls.Add(this.statusStripEDD);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "EDDiscoveryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EDDiscovery";
            this.Load += new System.EventHandler(this.EDDiscoveryForm_Load);
            this.Shown += new System.EventHandler(this.EDDiscoveryForm_Shown);
            this.ResizeBegin += new System.EventHandler(this.EDDiscoveryForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.EDDiscoveryForm_ResizeEnd);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EDDiscoveryForm_MouseDown);
            this.Resize += new System.EventHandler(this.EDDiscoveryForm_Resize);
            this.notifyIconContextMenuStrip.ResumeLayout(false);
            this.contextMenuStripTabs.ResumeLayout(false);
            this.panelToolBar.ResumeLayout(false);
            this.panelToolBar.PerformLayout();
            this.flowToolBar.ResumeLayout(false);
            this.statusStripEDD.ResumeLayout(false);
            this.statusStripEDD.PerformLayout();
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.tableLayoutPanelTop.PerformLayout();
            this.menuFlowPanel.ResumeLayout(false);
            this.menuFlowPanel.PerformLayout();
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
            this.closeminimizeFlowPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ExtendedControls.ExtButton buttonReloadActions;
        private ExtendedControls.ExtStatusStrip statusStripEDD;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarEDD;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelEDD;
        private System.Windows.Forms.NotifyIcon notifyIconEDD;
        private System.Windows.Forms.ContextMenuStrip notifyIconContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenu_Exit;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenu_Hide;
        private System.Windows.Forms.ToolStripMenuItem notifyIconMenu_Open;
        private ExtendedControls.ExtPanelRollUp panelToolBar;
        private ExtendedControls.ExtComboBox comboBoxCommander;
        private ExtendedControls.ExtButton buttonExtRefresh;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonExtEditAddOns;
        private ExtendedControls.ExtButton buttonExtManageAddOns;
        private ExtendedControls.ExtButton buttonExtPopOut;
        private EDDiscovery.MajorTabControl tabControlMain;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTabs;
        private System.Windows.Forms.ToolStripMenuItem addTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeTabToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameTabToolStripMenuItem;
        private ExtendedControls.ExtComboBox comboBoxCustomProfiles;
        private System.Windows.Forms.ToolStripMenuItem popOutPanelToolStripMenuItem;
        private System.Windows.Forms.FlowLayoutPanel flowToolBar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTop;
        private System.Windows.Forms.FlowLayoutPanel menuFlowPanel;
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllPopoutsInTaskBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllInTaskBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem turnOffAllTransparencyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem adminToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncEDSMSystemsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fetchLogsAgainToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendUnsyncedEDSMJournalsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showLogfilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dEBUGResetAllHistoryToFirstCommandeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem read21AndFormerLogFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem read21AndFormerLogFiles_forceReloadLogsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rescanAllJournalFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteDuplicateFSDJumpEntriesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendHistoricDataToInaraToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addOnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem manageAddOnsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureAddOnActionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editLastActionPackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopCurrentlyRunningActionProgramToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wikiHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator helpMenuSeparatorTop;
        private System.Windows.Forms.ToolStripMenuItem eDDiscoveryChatDiscordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frontierForumThreadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gitHubToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportIssueIdeasToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator helpMenuSeparatorBottom;
        private System.Windows.Forms.ToolStripMenuItem checkForNewReleaseToolStripMenuItem;
        private System.Windows.Forms.Label label_version;
        private System.Windows.Forms.Label labelInfoBoxTop;
        private System.Windows.Forms.FlowLayoutPanel closeminimizeFlowPanel;
        private ExtendedControls.ExtButtonDrawn panel_close;
        private ExtendedControls.ExtButtonDrawn panel_minimize;
        private System.Windows.Forms.Panel panel_eddiscovery;
        private System.Windows.Forms.ToolStripMenuItem rebuildSystemDBIndexesToolStripMenuItem;
        private ExtendedControls.ExtPanelResizer extPanelTopResizer;
        private System.Windows.Forms.Label labelGameDateTime;
        private System.Windows.Forms.ToolStripMenuItem load21ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpTabToolStripMenuItem;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelp;
        private System.Windows.Forms.ToolStripMenuItem viewHelpVideosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rebuildUserDBIndexesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateUnknownSystemCoordsWithDataFromSystemDBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemListBindings;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private ExtendedControls.ExtButton extButtonCAPI;
        private ExtendedControls.ExtButton extButtonNewFeature;
        private System.Windows.Forms.ToolStripMenuItem fetchStarDataAgainToolStripMenuItem;
    }
}
