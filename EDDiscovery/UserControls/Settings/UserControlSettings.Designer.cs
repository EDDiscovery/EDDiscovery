/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
namespace EDDiscovery.UserControls
{
    partial class UserControlSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlSettings));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.buttonEditCommander = new ExtendedControls.ExtButton();
            this.btnDeleteCommander = new ExtendedControls.ExtButton();
            this.buttonAddCommander = new ExtendedControls.ExtButton();
            this.comboBoxCustomEssentialEntries = new ExtendedControls.ExtComboBox();
            this.comboBoxCustomHistoryLoadTime = new ExtendedControls.ExtComboBox();
            this.checkBoxOrderRowsInverted = new ExtendedControls.ExtCheckBox();
            this.checkBoxPanelSortOrder = new ExtendedControls.ExtCheckBox();
            this.checkBoxKeepOnTop = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomResize = new ExtendedControls.ExtCheckBox();
            this.checkBoxMinimizeToNotifyIcon = new ExtendedControls.ExtCheckBox();
            this.comboBoxClickThruKey = new ExtendedControls.ExtComboBox();
            this.checkBoxUseNotifyIcon = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomCopyToClipboard = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomMarkHiRes = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomRemoveOriginals = new ExtendedControls.ExtCheckBox();
            this.buttonExtScreenshot = new ExtendedControls.ExtButton();
            this.checkBoxCustomEnableScreenshots = new ExtendedControls.ExtCheckBox();
            this.buttonExtEDSMConfigureArea = new ExtendedControls.ExtButton();
            this.checkBoxCustomEDSMEDDBDownload = new ExtendedControls.ExtCheckBox();
            this.comboBoxTheme = new ExtendedControls.ExtComboBox();
            this.button_edittheme = new ExtendedControls.ExtButton();
            this.buttonSaveTheme = new ExtendedControls.ExtButton();
            this.buttonExtSafeMode = new ExtendedControls.ExtButton();
            this.extPanelScroll = new ExtendedControls.ExtPanelScroll();
            this.extScrollBarSettings = new ExtendedControls.ExtScrollBar();
            this.groupBoxCommanders = new ExtendedControls.ExtGroupBox();
            this.dataViewScrollerCommanders = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewCommanders = new System.Windows.Forms.DataGridView();
            this.ColumnCommander = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EdsmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JournalDirCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotesCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCommanders = new ExtendedControls.ExtScrollBar();
            this.flowLayoutButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBoxTheme = new ExtendedControls.ExtGroupBox();
            this.groupBoxCustomHistoryLoad = new ExtendedControls.ExtGroupBox();
            this.extComboBoxGameTime = new ExtendedControls.ExtComboBox();
            this.labelTimeDisplay = new System.Windows.Forms.Label();
            this.extGroupBoxWebServer = new ExtendedControls.ExtGroupBox();
            this.numberBoxLongPortNo = new ExtendedControls.NumberBoxLong();
            this.labelPortNo = new System.Windows.Forms.Label();
            this.extButtonTestWeb = new ExtendedControls.ExtButton();
            this.extCheckBoxWebServerEnable = new ExtendedControls.ExtCheckBox();
            this.groupBoxInteraction = new ExtendedControls.ExtGroupBox();
            this.labelTKey = new System.Windows.Forms.Label();
            this.groupBoxMemory = new ExtendedControls.ExtGroupBox();
            this.labelHistoryEssItems = new System.Windows.Forms.Label();
            this.labelHistorySel = new System.Windows.Forms.Label();
            this.groupBoxCustomScreenShots = new ExtendedControls.ExtGroupBox();
            this.groupBoxCustomEDSM = new ExtendedControls.ExtGroupBox();
            this.groupBoxPopOuts = new ExtendedControls.ExtGroupBox();
            this.groupBoxCustomLanguage = new ExtendedControls.ExtGroupBox();
            this.comboBoxCustomLanguage = new ExtendedControls.ExtComboBox();
            this.groupBoxCustomSafeMode = new ExtendedControls.ExtGroupBox();
            this.labelSafeMode = new System.Windows.Forms.Label();
            this.extPanelScroll.SuspendLayout();
            this.groupBoxCommanders.SuspendLayout();
            this.dataViewScrollerCommanders.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).BeginInit();
            this.flowLayoutButtons.SuspendLayout();
            this.groupBoxTheme.SuspendLayout();
            this.groupBoxCustomHistoryLoad.SuspendLayout();
            this.extGroupBoxWebServer.SuspendLayout();
            this.groupBoxInteraction.SuspendLayout();
            this.groupBoxMemory.SuspendLayout();
            this.groupBoxCustomScreenShots.SuspendLayout();
            this.groupBoxCustomEDSM.SuspendLayout();
            this.groupBoxPopOuts.SuspendLayout();
            this.groupBoxCustomLanguage.SuspendLayout();
            this.groupBoxCustomSafeMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // buttonEditCommander
            // 
            this.buttonEditCommander.Location = new System.Drawing.Point(767, 3);
            this.buttonEditCommander.Name = "buttonEditCommander";
            this.buttonEditCommander.Size = new System.Drawing.Size(90, 23);
            this.buttonEditCommander.TabIndex = 5;
            this.buttonEditCommander.Text = "Edit";
            this.toolTip.SetToolTip(this.buttonEditCommander, "Edit selected commander");
            this.buttonEditCommander.UseVisualStyleBackColor = true;
            this.buttonEditCommander.Click += new System.EventHandler(this.buttonEditCommander_Click);
            // 
            // btnDeleteCommander
            // 
            this.btnDeleteCommander.Location = new System.Drawing.Point(863, 3);
            this.btnDeleteCommander.Name = "btnDeleteCommander";
            this.btnDeleteCommander.Size = new System.Drawing.Size(90, 23);
            this.btnDeleteCommander.TabIndex = 3;
            this.btnDeleteCommander.Text = "Delete";
            this.toolTip.SetToolTip(this.btnDeleteCommander, "Delete selected commander");
            this.btnDeleteCommander.UseVisualStyleBackColor = true;
            this.btnDeleteCommander.Click += new System.EventHandler(this.btnDeleteCommander_Click);
            // 
            // buttonAddCommander
            // 
            this.buttonAddCommander.Location = new System.Drawing.Point(671, 3);
            this.buttonAddCommander.Name = "buttonAddCommander";
            this.buttonAddCommander.Size = new System.Drawing.Size(90, 23);
            this.buttonAddCommander.TabIndex = 0;
            this.buttonAddCommander.Text = "Add";
            this.toolTip.SetToolTip(this.buttonAddCommander, "Add a new commander");
            this.buttonAddCommander.UseVisualStyleBackColor = true;
            this.buttonAddCommander.Click += new System.EventHandler(this.buttonAddCommander_Click);
            // 
            // comboBoxCustomEssentialEntries
            // 
            this.comboBoxCustomEssentialEntries.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomEssentialEntries.ButtonColorScaling = 0.5F;
            this.comboBoxCustomEssentialEntries.DataSource = null;
            this.comboBoxCustomEssentialEntries.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomEssentialEntries.DisplayMember = "";
            this.comboBoxCustomEssentialEntries.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomEssentialEntries.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomEssentialEntries.Location = new System.Drawing.Point(128, 48);
            this.comboBoxCustomEssentialEntries.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomEssentialEntries.Name = "comboBoxCustomEssentialEntries";
            this.comboBoxCustomEssentialEntries.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomEssentialEntries.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomEssentialEntries.SelectedIndex = -1;
            this.comboBoxCustomEssentialEntries.SelectedItem = null;
            this.comboBoxCustomEssentialEntries.SelectedValue = null;
            this.comboBoxCustomEssentialEntries.Size = new System.Drawing.Size(137, 21);
            this.comboBoxCustomEssentialEntries.TabIndex = 7;
            this.comboBoxCustomEssentialEntries.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCustomEssentialEntries, "Select which items you consider essential to load older than the time above");
            this.comboBoxCustomEssentialEntries.ValueMember = "";
            // 
            // comboBoxCustomHistoryLoadTime
            // 
            this.comboBoxCustomHistoryLoadTime.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomHistoryLoadTime.ButtonColorScaling = 0.5F;
            this.comboBoxCustomHistoryLoadTime.DataSource = null;
            this.comboBoxCustomHistoryLoadTime.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomHistoryLoadTime.DisplayMember = "";
            this.comboBoxCustomHistoryLoadTime.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomHistoryLoadTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomHistoryLoadTime.Location = new System.Drawing.Point(128, 19);
            this.comboBoxCustomHistoryLoadTime.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomHistoryLoadTime.Name = "comboBoxCustomHistoryLoadTime";
            this.comboBoxCustomHistoryLoadTime.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomHistoryLoadTime.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomHistoryLoadTime.SelectedIndex = -1;
            this.comboBoxCustomHistoryLoadTime.SelectedItem = null;
            this.comboBoxCustomHistoryLoadTime.SelectedValue = null;
            this.comboBoxCustomHistoryLoadTime.Size = new System.Drawing.Size(137, 21);
            this.comboBoxCustomHistoryLoadTime.TabIndex = 7;
            this.comboBoxCustomHistoryLoadTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCustomHistoryLoadTime, "Reduce Memory use. Select either load all records, or load only essential items o" +
        "f records older than a set time before now");
            this.comboBoxCustomHistoryLoadTime.ValueMember = "";
            // 
            // checkBoxOrderRowsInverted
            // 
            this.checkBoxOrderRowsInverted.AutoSize = true;
            this.checkBoxOrderRowsInverted.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxOrderRowsInverted.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxOrderRowsInverted.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxOrderRowsInverted.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxOrderRowsInverted.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxOrderRowsInverted.ImageIndeterminate = null;
            this.checkBoxOrderRowsInverted.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxOrderRowsInverted.ImageUnchecked = null;
            this.checkBoxOrderRowsInverted.Location = new System.Drawing.Point(9, 23);
            this.checkBoxOrderRowsInverted.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxOrderRowsInverted.Name = "checkBoxOrderRowsInverted";
            this.checkBoxOrderRowsInverted.Size = new System.Drawing.Size(196, 17);
            this.checkBoxOrderRowsInverted.TabIndex = 2;
            this.checkBoxOrderRowsInverted.Text = "Number Rows Lastest Entry Highest";
            this.checkBoxOrderRowsInverted.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxOrderRowsInverted, "Number oldest entry 1, latest entry highest");
            this.checkBoxOrderRowsInverted.UseVisualStyleBackColor = true;
            // 
            // checkBoxPanelSortOrder
            // 
            this.checkBoxPanelSortOrder.AutoSize = true;
            this.checkBoxPanelSortOrder.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxPanelSortOrder.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxPanelSortOrder.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxPanelSortOrder.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxPanelSortOrder.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxPanelSortOrder.ImageIndeterminate = null;
            this.checkBoxPanelSortOrder.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxPanelSortOrder.ImageUnchecked = null;
            this.checkBoxPanelSortOrder.Location = new System.Drawing.Point(13, 103);
            this.checkBoxPanelSortOrder.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxPanelSortOrder.Name = "checkBoxPanelSortOrder";
            this.checkBoxPanelSortOrder.Size = new System.Drawing.Size(188, 17);
            this.checkBoxPanelSortOrder.TabIndex = 5;
            this.checkBoxPanelSortOrder.Text = "Panel List Sorted Alphanumerically";
            this.checkBoxPanelSortOrder.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxPanelSortOrder, "Panel lists sorted alphanumerically instead of ordered in groups. Note Requires R" +
        "estart");
            this.checkBoxPanelSortOrder.UseVisualStyleBackColor = true;
            // 
            // checkBoxKeepOnTop
            // 
            this.checkBoxKeepOnTop.AutoSize = true;
            this.checkBoxKeepOnTop.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxKeepOnTop.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxKeepOnTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxKeepOnTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxKeepOnTop.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxKeepOnTop.ImageIndeterminate = null;
            this.checkBoxKeepOnTop.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxKeepOnTop.ImageUnchecked = null;
            this.checkBoxKeepOnTop.Location = new System.Drawing.Point(13, 80);
            this.checkBoxKeepOnTop.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxKeepOnTop.Name = "checkBoxKeepOnTop";
            this.checkBoxKeepOnTop.Size = new System.Drawing.Size(88, 17);
            this.checkBoxKeepOnTop.TabIndex = 5;
            this.checkBoxKeepOnTop.Text = "Keep on Top";
            this.checkBoxKeepOnTop.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxKeepOnTop, "This window, and its children, top");
            this.checkBoxKeepOnTop.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomResize
            // 
            this.checkBoxCustomResize.AutoSize = true;
            this.checkBoxCustomResize.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomResize.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomResize.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomResize.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomResize.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomResize.ImageIndeterminate = null;
            this.checkBoxCustomResize.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomResize.ImageUnchecked = null;
            this.checkBoxCustomResize.Location = new System.Drawing.Point(13, 59);
            this.checkBoxCustomResize.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomResize.Name = "checkBoxCustomResize";
            this.checkBoxCustomResize.Size = new System.Drawing.Size(186, 17);
            this.checkBoxCustomResize.TabIndex = 6;
            this.checkBoxCustomResize.Text = "Redraw the screen during resizing";
            this.checkBoxCustomResize.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomResize, "Check to allow EDD to redraw the screen during main window resize. Only disable i" +
        "f its too slow");
            this.checkBoxCustomResize.UseVisualStyleBackColor = true;
            // 
            // checkBoxMinimizeToNotifyIcon
            // 
            this.checkBoxMinimizeToNotifyIcon.AutoSize = true;
            this.checkBoxMinimizeToNotifyIcon.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxMinimizeToNotifyIcon.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxMinimizeToNotifyIcon.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMinimizeToNotifyIcon.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMinimizeToNotifyIcon.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxMinimizeToNotifyIcon.ImageIndeterminate = null;
            this.checkBoxMinimizeToNotifyIcon.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMinimizeToNotifyIcon.ImageUnchecked = null;
            this.checkBoxMinimizeToNotifyIcon.Location = new System.Drawing.Point(13, 39);
            this.checkBoxMinimizeToNotifyIcon.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxMinimizeToNotifyIcon.Name = "checkBoxMinimizeToNotifyIcon";
            this.checkBoxMinimizeToNotifyIcon.Size = new System.Drawing.Size(179, 17);
            this.checkBoxMinimizeToNotifyIcon.TabIndex = 6;
            this.checkBoxMinimizeToNotifyIcon.Text = "Minimize to notification area icon";
            this.checkBoxMinimizeToNotifyIcon.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxMinimizeToNotifyIcon, "Minimize the main window to the system notification area (system tray) icon.");
            this.checkBoxMinimizeToNotifyIcon.UseVisualStyleBackColor = true;
            // 
            // comboBoxClickThruKey
            // 
            this.comboBoxClickThruKey.BorderColor = System.Drawing.Color.White;
            this.comboBoxClickThruKey.ButtonColorScaling = 0.5F;
            this.comboBoxClickThruKey.DataSource = null;
            this.comboBoxClickThruKey.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxClickThruKey.DisplayMember = "";
            this.comboBoxClickThruKey.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxClickThruKey.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxClickThruKey.Location = new System.Drawing.Point(18, 48);
            this.comboBoxClickThruKey.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxClickThruKey.Name = "comboBoxClickThruKey";
            this.comboBoxClickThruKey.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxClickThruKey.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxClickThruKey.SelectedIndex = -1;
            this.comboBoxClickThruKey.SelectedItem = null;
            this.comboBoxClickThruKey.SelectedValue = null;
            this.comboBoxClickThruKey.Size = new System.Drawing.Size(215, 21);
            this.comboBoxClickThruKey.TabIndex = 6;
            this.comboBoxClickThruKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxClickThruKey, resources.GetString("comboBoxClickThruKey.ToolTip"));
            this.comboBoxClickThruKey.ValueMember = "";
            // 
            // checkBoxUseNotifyIcon
            // 
            this.checkBoxUseNotifyIcon.AutoSize = true;
            this.checkBoxUseNotifyIcon.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxUseNotifyIcon.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxUseNotifyIcon.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxUseNotifyIcon.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxUseNotifyIcon.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxUseNotifyIcon.ImageIndeterminate = null;
            this.checkBoxUseNotifyIcon.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxUseNotifyIcon.ImageUnchecked = null;
            this.checkBoxUseNotifyIcon.Location = new System.Drawing.Point(13, 18);
            this.checkBoxUseNotifyIcon.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxUseNotifyIcon.Name = "checkBoxUseNotifyIcon";
            this.checkBoxUseNotifyIcon.Size = new System.Drawing.Size(154, 17);
            this.checkBoxUseNotifyIcon.TabIndex = 5;
            this.checkBoxUseNotifyIcon.Text = "Show notification area icon";
            this.checkBoxUseNotifyIcon.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxUseNotifyIcon, "Show a system notification area (system tray) icon for EDDiscovery.");
            this.checkBoxUseNotifyIcon.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomCopyToClipboard
            // 
            this.checkBoxCustomCopyToClipboard.AutoSize = true;
            this.checkBoxCustomCopyToClipboard.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomCopyToClipboard.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomCopyToClipboard.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomCopyToClipboard.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomCopyToClipboard.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomCopyToClipboard.ImageIndeterminate = null;
            this.checkBoxCustomCopyToClipboard.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomCopyToClipboard.ImageUnchecked = null;
            this.checkBoxCustomCopyToClipboard.Location = new System.Drawing.Point(157, 61);
            this.checkBoxCustomCopyToClipboard.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomCopyToClipboard.Name = "checkBoxCustomCopyToClipboard";
            this.checkBoxCustomCopyToClipboard.Size = new System.Drawing.Size(108, 17);
            this.checkBoxCustomCopyToClipboard.TabIndex = 5;
            this.checkBoxCustomCopyToClipboard.Text = "Copy to clipboard";
            this.checkBoxCustomCopyToClipboard.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomCopyToClipboard, "Auto copy the image to the clipboard");
            this.checkBoxCustomCopyToClipboard.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomMarkHiRes
            // 
            this.checkBoxCustomMarkHiRes.AutoSize = true;
            this.checkBoxCustomMarkHiRes.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomMarkHiRes.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomMarkHiRes.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomMarkHiRes.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomMarkHiRes.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomMarkHiRes.ImageIndeterminate = null;
            this.checkBoxCustomMarkHiRes.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomMarkHiRes.ImageUnchecked = null;
            this.checkBoxCustomMarkHiRes.Location = new System.Drawing.Point(9, 61);
            this.checkBoxCustomMarkHiRes.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomMarkHiRes.Name = "checkBoxCustomMarkHiRes";
            this.checkBoxCustomMarkHiRes.Size = new System.Drawing.Size(103, 17);
            this.checkBoxCustomMarkHiRes.TabIndex = 5;
            this.checkBoxCustomMarkHiRes.Text = "Mark HiRes files";
            this.checkBoxCustomMarkHiRes.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomMarkHiRes, "For Hi-Res files, mark them in the file name");
            this.checkBoxCustomMarkHiRes.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomRemoveOriginals
            // 
            this.checkBoxCustomRemoveOriginals.AutoSize = true;
            this.checkBoxCustomRemoveOriginals.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomRemoveOriginals.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomRemoveOriginals.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomRemoveOriginals.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomRemoveOriginals.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomRemoveOriginals.ImageIndeterminate = null;
            this.checkBoxCustomRemoveOriginals.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomRemoveOriginals.ImageUnchecked = null;
            this.checkBoxCustomRemoveOriginals.Location = new System.Drawing.Point(9, 40);
            this.checkBoxCustomRemoveOriginals.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomRemoveOriginals.Name = "checkBoxCustomRemoveOriginals";
            this.checkBoxCustomRemoveOriginals.Size = new System.Drawing.Size(109, 17);
            this.checkBoxCustomRemoveOriginals.TabIndex = 5;
            this.checkBoxCustomRemoveOriginals.Text = "Remove Originals";
            this.checkBoxCustomRemoveOriginals.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomRemoveOriginals, "After conversion, remove originals");
            this.checkBoxCustomRemoveOriginals.UseVisualStyleBackColor = true;
            // 
            // buttonExtScreenshot
            // 
            this.buttonExtScreenshot.Location = new System.Drawing.Point(157, 23);
            this.buttonExtScreenshot.Name = "buttonExtScreenshot";
            this.buttonExtScreenshot.Size = new System.Drawing.Size(99, 23);
            this.buttonExtScreenshot.TabIndex = 10;
            this.buttonExtScreenshot.Text = "Configure";
            this.toolTip.SetToolTip(this.buttonExtScreenshot, "Configure further screenshot options");
            this.buttonExtScreenshot.UseVisualStyleBackColor = true;
            this.buttonExtScreenshot.Click += new System.EventHandler(this.buttonExtScreenshot_Click);
            // 
            // checkBoxCustomEnableScreenshots
            // 
            this.checkBoxCustomEnableScreenshots.AutoSize = true;
            this.checkBoxCustomEnableScreenshots.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEnableScreenshots.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomEnableScreenshots.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEnableScreenshots.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEnableScreenshots.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomEnableScreenshots.ImageIndeterminate = null;
            this.checkBoxCustomEnableScreenshots.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomEnableScreenshots.ImageUnchecked = null;
            this.checkBoxCustomEnableScreenshots.Location = new System.Drawing.Point(9, 19);
            this.checkBoxCustomEnableScreenshots.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEnableScreenshots.Name = "checkBoxCustomEnableScreenshots";
            this.checkBoxCustomEnableScreenshots.Size = new System.Drawing.Size(59, 17);
            this.checkBoxCustomEnableScreenshots.TabIndex = 5;
            this.checkBoxCustomEnableScreenshots.Text = "Enable";
            this.checkBoxCustomEnableScreenshots.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomEnableScreenshots, "Screen shot conversion on/off");
            this.checkBoxCustomEnableScreenshots.UseVisualStyleBackColor = true;
            // 
            // buttonExtEDSMConfigureArea
            // 
            this.buttonExtEDSMConfigureArea.Location = new System.Drawing.Point(19, 49);
            this.buttonExtEDSMConfigureArea.Name = "buttonExtEDSMConfigureArea";
            this.buttonExtEDSMConfigureArea.Size = new System.Drawing.Size(196, 23);
            this.buttonExtEDSMConfigureArea.TabIndex = 10;
            this.buttonExtEDSMConfigureArea.Text = "Select Galaxy Sectors";
            this.toolTip.SetToolTip(this.buttonExtEDSMConfigureArea, "Configure what parts of the galaxy is stored in the databases");
            this.buttonExtEDSMConfigureArea.UseVisualStyleBackColor = true;
            this.buttonExtEDSMConfigureArea.Click += new System.EventHandler(this.buttonExtEDSMConfigureArea_Click);
            // 
            // checkBoxCustomEDSMEDDBDownload
            // 
            this.checkBoxCustomEDSMEDDBDownload.AutoSize = true;
            this.checkBoxCustomEDSMEDDBDownload.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEDSMEDDBDownload.CheckBoxDisabledScaling = 0.5F;
            this.checkBoxCustomEDSMEDDBDownload.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEDSMEDDBDownload.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDSMEDDBDownload.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxCustomEDSMEDDBDownload.ImageIndeterminate = null;
            this.checkBoxCustomEDSMEDDBDownload.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomEDSMEDDBDownload.ImageUnchecked = null;
            this.checkBoxCustomEDSMEDDBDownload.Location = new System.Drawing.Point(9, 19);
            this.checkBoxCustomEDSMEDDBDownload.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomEDSMEDDBDownload.Name = "checkBoxCustomEDSMEDDBDownload";
            this.checkBoxCustomEDSMEDDBDownload.Size = new System.Drawing.Size(158, 17);
            this.checkBoxCustomEDSMEDDBDownload.TabIndex = 5;
            this.checkBoxCustomEDSMEDDBDownload.Text = "Enable Star Data Download";
            this.checkBoxCustomEDSMEDDBDownload.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomEDSMEDDBDownload, "Click to enable downloading of stars from EDSM and system information from EDDB. " +
        " Will apply at next start.");
            this.checkBoxCustomEDSMEDDBDownload.UseVisualStyleBackColor = true;
            // 
            // comboBoxTheme
            // 
            this.comboBoxTheme.BackColor = System.Drawing.Color.Gray;
            this.comboBoxTheme.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTheme.ButtonColorScaling = 0.5F;
            this.comboBoxTheme.DataSource = null;
            this.comboBoxTheme.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTheme.DisplayMember = "";
            this.comboBoxTheme.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxTheme.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTheme.Location = new System.Drawing.Point(10, 19);
            this.comboBoxTheme.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxTheme.Name = "comboBoxTheme";
            this.comboBoxTheme.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxTheme.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxTheme.SelectedIndex = -1;
            this.comboBoxTheme.SelectedItem = null;
            this.comboBoxTheme.SelectedValue = null;
            this.comboBoxTheme.Size = new System.Drawing.Size(223, 21);
            this.comboBoxTheme.TabIndex = 0;
            this.comboBoxTheme.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxTheme, "Select the theme to use");
            this.comboBoxTheme.ValueMember = "";
            // 
            // button_edittheme
            // 
            this.button_edittheme.Location = new System.Drawing.Point(128, 48);
            this.button_edittheme.Name = "button_edittheme";
            this.button_edittheme.Size = new System.Drawing.Size(105, 23);
            this.button_edittheme.TabIndex = 10;
            this.button_edittheme.Text = "Edit Theme";
            this.toolTip.SetToolTip(this.button_edittheme, "Edit theme and change colours fonts");
            this.button_edittheme.UseVisualStyleBackColor = true;
            this.button_edittheme.Click += new System.EventHandler(this.button_edittheme_Click);
            // 
            // buttonSaveTheme
            // 
            this.buttonSaveTheme.Location = new System.Drawing.Point(9, 48);
            this.buttonSaveTheme.Name = "buttonSaveTheme";
            this.buttonSaveTheme.Size = new System.Drawing.Size(105, 23);
            this.buttonSaveTheme.TabIndex = 7;
            this.buttonSaveTheme.Text = "Save Theme";
            this.toolTip.SetToolTip(this.buttonSaveTheme, "Save theme to disk");
            this.buttonSaveTheme.UseVisualStyleBackColor = true;
            this.buttonSaveTheme.Click += new System.EventHandler(this.buttonSaveTheme_Click);
            // 
            // buttonExtSafeMode
            // 
            this.buttonExtSafeMode.Location = new System.Drawing.Point(16, 94);
            this.buttonExtSafeMode.Name = "buttonExtSafeMode";
            this.buttonExtSafeMode.Size = new System.Drawing.Size(219, 23);
            this.buttonExtSafeMode.TabIndex = 10;
            this.buttonExtSafeMode.Text = "Restart in Safe Mode";
            this.toolTip.SetToolTip(this.buttonExtSafeMode, "Safe Mode allows you to perform special operations, such as moving the databases," +
        " resetting the UI, resetting the action packs,  DLLs etc.");
            this.buttonExtSafeMode.UseVisualStyleBackColor = true;
            this.buttonExtSafeMode.Click += new System.EventHandler(this.buttonExtSafeMode_Click);
            // 
            // extPanelScroll
            // 
            this.extPanelScroll.Controls.Add(this.extScrollBarSettings);
            this.extPanelScroll.Controls.Add(this.groupBoxCommanders);
            this.extPanelScroll.Controls.Add(this.groupBoxTheme);
            this.extPanelScroll.Controls.Add(this.groupBoxCustomHistoryLoad);
            this.extPanelScroll.Controls.Add(this.extGroupBoxWebServer);
            this.extPanelScroll.Controls.Add(this.groupBoxInteraction);
            this.extPanelScroll.Controls.Add(this.groupBoxMemory);
            this.extPanelScroll.Controls.Add(this.groupBoxCustomScreenShots);
            this.extPanelScroll.Controls.Add(this.groupBoxCustomEDSM);
            this.extPanelScroll.Controls.Add(this.groupBoxPopOuts);
            this.extPanelScroll.Controls.Add(this.groupBoxCustomLanguage);
            this.extPanelScroll.Controls.Add(this.groupBoxCustomSafeMode);
            this.extPanelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelScroll.FlowControlsLeftToRight = true;
            this.extPanelScroll.Location = new System.Drawing.Point(0, 0);
            this.extPanelScroll.Name = "extPanelScroll";
            this.extPanelScroll.Size = new System.Drawing.Size(981, 623);
            this.extPanelScroll.TabIndex = 22;
            this.extPanelScroll.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarSettings
            // 
            this.extScrollBarSettings.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarSettings.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarSettings.ArrowColorScaling = 0.5F;
            this.extScrollBarSettings.ArrowDownDrawAngle = 270F;
            this.extScrollBarSettings.ArrowUpDrawAngle = 90F;
            this.extScrollBarSettings.BorderColor = System.Drawing.Color.White;
            this.extScrollBarSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarSettings.HideScrollBar = true;
            this.extScrollBarSettings.LargeChange = 10;
            this.extScrollBarSettings.Location = new System.Drawing.Point(965, 0);
            this.extScrollBarSettings.Maximum = -5;
            this.extScrollBarSettings.Minimum = 0;
            this.extScrollBarSettings.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarSettings.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarSettings.Name = "extScrollBarSettings";
            this.extScrollBarSettings.Size = new System.Drawing.Size(16, 623);
            this.extScrollBarSettings.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarSettings.SmallChange = 1;
            this.extScrollBarSettings.TabIndex = 22;
            this.extScrollBarSettings.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarSettings.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarSettings.ThumbColorScaling = 0.5F;
            this.extScrollBarSettings.ThumbDrawAngle = 0F;
            this.extScrollBarSettings.Value = -5;
            this.extScrollBarSettings.ValueLimited = -5;
            // 
            // groupBoxCommanders
            // 
            this.groupBoxCommanders.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCommanders.BackColorScaling = 0.5F;
            this.groupBoxCommanders.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCommanders.BorderColorScaling = 0.5F;
            this.groupBoxCommanders.Controls.Add(this.dataViewScrollerCommanders);
            this.groupBoxCommanders.Controls.Add(this.flowLayoutButtons);
            this.groupBoxCommanders.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxCommanders.FillClientAreaWithAlternateColor = false;
            this.groupBoxCommanders.Location = new System.Drawing.Point(3, 3);
            this.groupBoxCommanders.Name = "groupBoxCommanders";
            this.groupBoxCommanders.Padding = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.groupBoxCommanders.Size = new System.Drawing.Size(965, 153);
            this.groupBoxCommanders.TabIndex = 15;
            this.groupBoxCommanders.TabStop = false;
            this.groupBoxCommanders.Text = "Commanders";
            this.groupBoxCommanders.TextPadding = 0;
            this.groupBoxCommanders.TextStartPosition = -1;
            // 
            // dataViewScrollerCommanders
            // 
            this.dataViewScrollerCommanders.Controls.Add(this.dataGridViewCommanders);
            this.dataViewScrollerCommanders.Controls.Add(this.vScrollBarCommanders);
            this.dataViewScrollerCommanders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerCommanders.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerCommanders.LimitLargeChange = 2147483647;
            this.dataViewScrollerCommanders.Location = new System.Drawing.Point(3, 16);
            this.dataViewScrollerCommanders.Name = "dataViewScrollerCommanders";
            this.dataViewScrollerCommanders.Size = new System.Drawing.Size(956, 104);
            this.dataViewScrollerCommanders.TabIndex = 4;
            this.dataViewScrollerCommanders.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewCommanders
            // 
            this.dataGridViewCommanders.AllowUserToAddRows = false;
            this.dataGridViewCommanders.AllowUserToDeleteRows = false;
            this.dataGridViewCommanders.AllowUserToOrderColumns = true;
            this.dataGridViewCommanders.AllowUserToResizeRows = false;
            this.dataGridViewCommanders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCommanders.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewCommanders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCommanders.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnCommander,
            this.EdsmName,
            this.JournalDirCol,
            this.NotesCol});
            this.dataGridViewCommanders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewCommanders.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewCommanders.MultiSelect = false;
            this.dataGridViewCommanders.Name = "dataGridViewCommanders";
            this.dataGridViewCommanders.ReadOnly = true;
            this.dataGridViewCommanders.RowHeadersVisible = false;
            this.dataGridViewCommanders.RowHeadersWidth = 20;
            this.dataGridViewCommanders.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCommanders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCommanders.Size = new System.Drawing.Size(940, 104);
            this.dataGridViewCommanders.TabIndex = 2;
            // 
            // ColumnCommander
            // 
            this.ColumnCommander.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnCommander.DataPropertyName = "Name";
            this.ColumnCommander.FillWeight = 120F;
            this.ColumnCommander.HeaderText = "Commander";
            this.ColumnCommander.MinimumWidth = 50;
            this.ColumnCommander.Name = "ColumnCommander";
            this.ColumnCommander.ReadOnly = true;
            // 
            // EdsmName
            // 
            this.EdsmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.EdsmName.DataPropertyName = "EdsmName";
            this.EdsmName.HeaderText = "EDSM Name";
            this.EdsmName.MinimumWidth = 50;
            this.EdsmName.Name = "EdsmName";
            this.EdsmName.ReadOnly = true;
            // 
            // JournalDirCol
            // 
            this.JournalDirCol.DataPropertyName = "JournalDir";
            this.JournalDirCol.FillWeight = 120F;
            this.JournalDirCol.HeaderText = "Journal Folder";
            this.JournalDirCol.MinimumWidth = 50;
            this.JournalDirCol.Name = "JournalDirCol";
            this.JournalDirCol.ReadOnly = true;
            // 
            // NotesCol
            // 
            this.NotesCol.DataPropertyName = "Info";
            this.NotesCol.FillWeight = 180F;
            this.NotesCol.HeaderText = "Notes";
            this.NotesCol.MinimumWidth = 50;
            this.NotesCol.Name = "NotesCol";
            this.NotesCol.ReadOnly = true;
            // 
            // vScrollBarCommanders
            // 
            this.vScrollBarCommanders.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCommanders.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCommanders.ArrowColorScaling = 0.5F;
            this.vScrollBarCommanders.ArrowDownDrawAngle = 270F;
            this.vScrollBarCommanders.ArrowUpDrawAngle = 90F;
            this.vScrollBarCommanders.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCommanders.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBarCommanders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCommanders.HideScrollBar = false;
            this.vScrollBarCommanders.LargeChange = 0;
            this.vScrollBarCommanders.Location = new System.Drawing.Point(940, 0);
            this.vScrollBarCommanders.Maximum = -1;
            this.vScrollBarCommanders.Minimum = 0;
            this.vScrollBarCommanders.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCommanders.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCommanders.Name = "vScrollBarCommanders";
            this.vScrollBarCommanders.Size = new System.Drawing.Size(16, 104);
            this.vScrollBarCommanders.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCommanders.SmallChange = 1;
            this.vScrollBarCommanders.TabIndex = 3;
            this.vScrollBarCommanders.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCommanders.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCommanders.ThumbColorScaling = 0.5F;
            this.vScrollBarCommanders.ThumbDrawAngle = 0F;
            this.vScrollBarCommanders.Value = -1;
            this.vScrollBarCommanders.ValueLimited = -1;
            // 
            // flowLayoutButtons
            // 
            this.flowLayoutButtons.Controls.Add(this.btnDeleteCommander);
            this.flowLayoutButtons.Controls.Add(this.buttonEditCommander);
            this.flowLayoutButtons.Controls.Add(this.buttonAddCommander);
            this.flowLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutButtons.Location = new System.Drawing.Point(3, 120);
            this.flowLayoutButtons.Name = "flowLayoutButtons";
            this.flowLayoutButtons.Size = new System.Drawing.Size(956, 30);
            this.flowLayoutButtons.TabIndex = 6;
            // 
            // groupBoxTheme
            // 
            this.groupBoxTheme.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxTheme.BackColorScaling = 0.5F;
            this.groupBoxTheme.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxTheme.BorderColorScaling = 0.5F;
            this.groupBoxTheme.Controls.Add(this.comboBoxTheme);
            this.groupBoxTheme.Controls.Add(this.button_edittheme);
            this.groupBoxTheme.Controls.Add(this.buttonSaveTheme);
            this.groupBoxTheme.FillClientAreaWithAlternateColor = false;
            this.groupBoxTheme.Location = new System.Drawing.Point(3, 162);
            this.groupBoxTheme.Name = "groupBoxTheme";
            this.groupBoxTheme.Size = new System.Drawing.Size(281, 85);
            this.groupBoxTheme.TabIndex = 18;
            this.groupBoxTheme.TabStop = false;
            this.groupBoxTheme.Text = "Theme";
            this.groupBoxTheme.TextPadding = 0;
            this.groupBoxTheme.TextStartPosition = -1;
            // 
            // groupBoxCustomHistoryLoad
            // 
            this.groupBoxCustomHistoryLoad.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomHistoryLoad.BackColorScaling = 0.5F;
            this.groupBoxCustomHistoryLoad.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomHistoryLoad.BorderColorScaling = 0.5F;
            this.groupBoxCustomHistoryLoad.Controls.Add(this.checkBoxOrderRowsInverted);
            this.groupBoxCustomHistoryLoad.Controls.Add(this.extComboBoxGameTime);
            this.groupBoxCustomHistoryLoad.Controls.Add(this.labelTimeDisplay);
            this.groupBoxCustomHistoryLoad.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomHistoryLoad.Location = new System.Drawing.Point(290, 162);
            this.groupBoxCustomHistoryLoad.Name = "groupBoxCustomHistoryLoad";
            this.groupBoxCustomHistoryLoad.Size = new System.Drawing.Size(281, 85);
            this.groupBoxCustomHistoryLoad.TabIndex = 26;
            this.groupBoxCustomHistoryLoad.TabStop = false;
            this.groupBoxCustomHistoryLoad.Text = "History";
            this.groupBoxCustomHistoryLoad.TextPadding = 0;
            this.groupBoxCustomHistoryLoad.TextStartPosition = -1;
            // 
            // extComboBoxGameTime
            // 
            this.extComboBoxGameTime.BorderColor = System.Drawing.Color.White;
            this.extComboBoxGameTime.ButtonColorScaling = 0.5F;
            this.extComboBoxGameTime.DataSource = null;
            this.extComboBoxGameTime.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxGameTime.DisplayMember = "";
            this.extComboBoxGameTime.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.extComboBoxGameTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxGameTime.Location = new System.Drawing.Point(128, 48);
            this.extComboBoxGameTime.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.extComboBoxGameTime.Name = "extComboBoxGameTime";
            this.extComboBoxGameTime.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.extComboBoxGameTime.ScrollBarColor = System.Drawing.Color.LightGray;
            this.extComboBoxGameTime.SelectedIndex = -1;
            this.extComboBoxGameTime.SelectedItem = null;
            this.extComboBoxGameTime.SelectedValue = null;
            this.extComboBoxGameTime.Size = new System.Drawing.Size(137, 21);
            this.extComboBoxGameTime.TabIndex = 7;
            this.extComboBoxGameTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxGameTime.ValueMember = "";
            // 
            // labelTimeDisplay
            // 
            this.labelTimeDisplay.AutoSize = true;
            this.labelTimeDisplay.Location = new System.Drawing.Point(10, 52);
            this.labelTimeDisplay.Name = "labelTimeDisplay";
            this.labelTimeDisplay.Size = new System.Drawing.Size(33, 13);
            this.labelTimeDisplay.TabIndex = 5;
            this.labelTimeDisplay.Text = "Time:";
            // 
            // extGroupBoxWebServer
            // 
            this.extGroupBoxWebServer.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.extGroupBoxWebServer.BackColorScaling = 0.5F;
            this.extGroupBoxWebServer.BorderColor = System.Drawing.Color.LightGray;
            this.extGroupBoxWebServer.BorderColorScaling = 0.5F;
            this.extGroupBoxWebServer.Controls.Add(this.numberBoxLongPortNo);
            this.extGroupBoxWebServer.Controls.Add(this.labelPortNo);
            this.extGroupBoxWebServer.Controls.Add(this.extButtonTestWeb);
            this.extGroupBoxWebServer.Controls.Add(this.extCheckBoxWebServerEnable);
            this.extGroupBoxWebServer.FillClientAreaWithAlternateColor = false;
            this.extGroupBoxWebServer.Location = new System.Drawing.Point(577, 162);
            this.extGroupBoxWebServer.Name = "extGroupBoxWebServer";
            this.extGroupBoxWebServer.Size = new System.Drawing.Size(281, 85);
            this.extGroupBoxWebServer.TabIndex = 23;
            this.extGroupBoxWebServer.TabStop = false;
            this.extGroupBoxWebServer.Text = "Web Server";
            this.extGroupBoxWebServer.TextPadding = 0;
            this.extGroupBoxWebServer.TextStartPosition = -1;
            // 
            // numberBoxLongPortNo
            // 
            this.numberBoxLongPortNo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.numberBoxLongPortNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.numberBoxLongPortNo.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxLongPortNo.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxLongPortNo.BorderColorScaling = 0.5F;
            this.numberBoxLongPortNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxLongPortNo.ClearOnFirstChar = false;
            this.numberBoxLongPortNo.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxLongPortNo.DelayBeforeNotification = 0;
            this.numberBoxLongPortNo.EndButtonEnable = true;
            this.numberBoxLongPortNo.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxLongPortNo.EndButtonImage")));
            this.numberBoxLongPortNo.EndButtonVisible = false;
            this.numberBoxLongPortNo.Format = "D";
            this.numberBoxLongPortNo.InErrorCondition = false;
            this.numberBoxLongPortNo.Location = new System.Drawing.Point(57, 49);
            this.numberBoxLongPortNo.Maximum = ((long)(65535));
            this.numberBoxLongPortNo.Minimum = ((long)(1024));
            this.numberBoxLongPortNo.Multiline = false;
            this.numberBoxLongPortNo.Name = "numberBoxLongPortNo";
            this.numberBoxLongPortNo.ReadOnly = false;
            this.numberBoxLongPortNo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxLongPortNo.SelectionLength = 0;
            this.numberBoxLongPortNo.SelectionStart = 0;
            this.numberBoxLongPortNo.Size = new System.Drawing.Size(50, 23);
            this.numberBoxLongPortNo.TabIndex = 6;
            this.numberBoxLongPortNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxLongPortNo.Value = ((long)(6502));
            this.numberBoxLongPortNo.WordWrap = true;
            // 
            // labelPortNo
            // 
            this.labelPortNo.AutoSize = true;
            this.labelPortNo.Location = new System.Drawing.Point(7, 53);
            this.labelPortNo.Name = "labelPortNo";
            this.labelPortNo.Size = new System.Drawing.Size(26, 13);
            this.labelPortNo.TabIndex = 1;
            this.labelPortNo.Text = "Port";
            // 
            // extButtonTestWeb
            // 
            this.extButtonTestWeb.Location = new System.Drawing.Point(157, 48);
            this.extButtonTestWeb.Name = "extButtonTestWeb";
            this.extButtonTestWeb.Size = new System.Drawing.Size(99, 23);
            this.extButtonTestWeb.TabIndex = 3;
            this.extButtonTestWeb.Text = "Test";
            this.extButtonTestWeb.UseVisualStyleBackColor = true;
            this.extButtonTestWeb.Click += new System.EventHandler(this.extButtonTestWebClick);
            // 
            // extCheckBoxWebServerEnable
            // 
            this.extCheckBoxWebServerEnable.AutoSize = true;
            this.extCheckBoxWebServerEnable.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxWebServerEnable.CheckBoxDisabledScaling = 0.5F;
            this.extCheckBoxWebServerEnable.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWebServerEnable.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWebServerEnable.ImageButtonDisabledScaling = 0.5F;
            this.extCheckBoxWebServerEnable.ImageIndeterminate = null;
            this.extCheckBoxWebServerEnable.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWebServerEnable.ImageUnchecked = null;
            this.extCheckBoxWebServerEnable.Location = new System.Drawing.Point(10, 21);
            this.extCheckBoxWebServerEnable.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.extCheckBoxWebServerEnable.Name = "extCheckBoxWebServerEnable";
            this.extCheckBoxWebServerEnable.Size = new System.Drawing.Size(59, 17);
            this.extCheckBoxWebServerEnable.TabIndex = 5;
            this.extCheckBoxWebServerEnable.Text = "Enable";
            this.extCheckBoxWebServerEnable.TickBoxReductionRatio = 0.75F;
            this.extCheckBoxWebServerEnable.UseVisualStyleBackColor = true;
            // 
            // groupBoxInteraction
            // 
            this.groupBoxInteraction.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxInteraction.BackColorScaling = 0.5F;
            this.groupBoxInteraction.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxInteraction.BorderColorScaling = 0.5F;
            this.groupBoxInteraction.Controls.Add(this.comboBoxClickThruKey);
            this.groupBoxInteraction.Controls.Add(this.labelTKey);
            this.groupBoxInteraction.FillClientAreaWithAlternateColor = false;
            this.groupBoxInteraction.Location = new System.Drawing.Point(3, 253);
            this.groupBoxInteraction.Name = "groupBoxInteraction";
            this.groupBoxInteraction.Size = new System.Drawing.Size(281, 85);
            this.groupBoxInteraction.TabIndex = 25;
            this.groupBoxInteraction.TabStop = false;
            this.groupBoxInteraction.Text = "Interaction";
            this.groupBoxInteraction.TextPadding = 0;
            this.groupBoxInteraction.TextStartPosition = -1;
            // 
            // labelTKey
            // 
            this.labelTKey.AutoSize = true;
            this.labelTKey.Location = new System.Drawing.Point(7, 23);
            this.labelTKey.Name = "labelTKey";
            this.labelTKey.Size = new System.Drawing.Size(178, 13);
            this.labelTKey.TabIndex = 5;
            this.labelTKey.Text = "Key to activate transparent windows";
            // 
            // groupBoxMemory
            // 
            this.groupBoxMemory.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxMemory.BackColorScaling = 0.5F;
            this.groupBoxMemory.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxMemory.BorderColorScaling = 0.5F;
            this.groupBoxMemory.Controls.Add(this.comboBoxCustomEssentialEntries);
            this.groupBoxMemory.Controls.Add(this.comboBoxCustomHistoryLoadTime);
            this.groupBoxMemory.Controls.Add(this.labelHistoryEssItems);
            this.groupBoxMemory.Controls.Add(this.labelHistorySel);
            this.groupBoxMemory.FillClientAreaWithAlternateColor = false;
            this.groupBoxMemory.Location = new System.Drawing.Point(290, 253);
            this.groupBoxMemory.Name = "groupBoxMemory";
            this.groupBoxMemory.Size = new System.Drawing.Size(281, 85);
            this.groupBoxMemory.TabIndex = 21;
            this.groupBoxMemory.TabStop = false;
            this.groupBoxMemory.Text = "Memory";
            this.groupBoxMemory.TextPadding = 0;
            this.groupBoxMemory.TextStartPosition = -1;
            // 
            // labelHistoryEssItems
            // 
            this.labelHistoryEssItems.AutoSize = true;
            this.labelHistoryEssItems.Location = new System.Drawing.Point(10, 53);
            this.labelHistoryEssItems.Name = "labelHistoryEssItems";
            this.labelHistoryEssItems.Size = new System.Drawing.Size(86, 13);
            this.labelHistoryEssItems.TabIndex = 5;
            this.labelHistoryEssItems.Text = "Essential entries:";
            // 
            // labelHistorySel
            // 
            this.labelHistorySel.AutoSize = true;
            this.labelHistorySel.Location = new System.Drawing.Point(10, 23);
            this.labelHistorySel.Name = "labelHistorySel";
            this.labelHistorySel.Size = new System.Drawing.Size(78, 13);
            this.labelHistorySel.TabIndex = 5;
            this.labelHistorySel.Text = "Entries to read:";
            // 
            // groupBoxCustomScreenShots
            // 
            this.groupBoxCustomScreenShots.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomScreenShots.BackColorScaling = 0.5F;
            this.groupBoxCustomScreenShots.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomScreenShots.BorderColorScaling = 0.5F;
            this.groupBoxCustomScreenShots.Controls.Add(this.checkBoxCustomCopyToClipboard);
            this.groupBoxCustomScreenShots.Controls.Add(this.checkBoxCustomMarkHiRes);
            this.groupBoxCustomScreenShots.Controls.Add(this.checkBoxCustomRemoveOriginals);
            this.groupBoxCustomScreenShots.Controls.Add(this.buttonExtScreenshot);
            this.groupBoxCustomScreenShots.Controls.Add(this.checkBoxCustomEnableScreenshots);
            this.groupBoxCustomScreenShots.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomScreenShots.Location = new System.Drawing.Point(577, 253);
            this.groupBoxCustomScreenShots.Name = "groupBoxCustomScreenShots";
            this.groupBoxCustomScreenShots.Size = new System.Drawing.Size(281, 85);
            this.groupBoxCustomScreenShots.TabIndex = 20;
            this.groupBoxCustomScreenShots.TabStop = false;
            this.groupBoxCustomScreenShots.Text = "Screenshots conversion";
            this.groupBoxCustomScreenShots.TextPadding = 0;
            this.groupBoxCustomScreenShots.TextStartPosition = -1;
            // 
            // groupBoxCustomEDSM
            // 
            this.groupBoxCustomEDSM.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomEDSM.BackColorScaling = 0.5F;
            this.groupBoxCustomEDSM.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomEDSM.BorderColorScaling = 0.5F;
            this.groupBoxCustomEDSM.Controls.Add(this.buttonExtEDSMConfigureArea);
            this.groupBoxCustomEDSM.Controls.Add(this.checkBoxCustomEDSMEDDBDownload);
            this.groupBoxCustomEDSM.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomEDSM.Location = new System.Drawing.Point(3, 344);
            this.groupBoxCustomEDSM.Name = "groupBoxCustomEDSM";
            this.groupBoxCustomEDSM.Size = new System.Drawing.Size(281, 85);
            this.groupBoxCustomEDSM.TabIndex = 21;
            this.groupBoxCustomEDSM.TabStop = false;
            this.groupBoxCustomEDSM.Text = "EDSM/EDDB Control";
            this.groupBoxCustomEDSM.TextPadding = 0;
            this.groupBoxCustomEDSM.TextStartPosition = -1;
            // 
            // groupBoxPopOuts
            // 
            this.groupBoxPopOuts.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxPopOuts.BackColorScaling = 0.5F;
            this.groupBoxPopOuts.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxPopOuts.BorderColorScaling = 0.5F;
            this.groupBoxPopOuts.Controls.Add(this.checkBoxPanelSortOrder);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxKeepOnTop);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxCustomResize);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxMinimizeToNotifyIcon);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxUseNotifyIcon);
            this.groupBoxPopOuts.FillClientAreaWithAlternateColor = false;
            this.groupBoxPopOuts.Location = new System.Drawing.Point(290, 344);
            this.groupBoxPopOuts.Name = "groupBoxPopOuts";
            this.groupBoxPopOuts.Size = new System.Drawing.Size(281, 129);
            this.groupBoxPopOuts.TabIndex = 19;
            this.groupBoxPopOuts.TabStop = false;
            this.groupBoxPopOuts.Text = "Window Options";
            this.groupBoxPopOuts.TextPadding = 0;
            this.groupBoxPopOuts.TextStartPosition = -1;
            // 
            // groupBoxCustomLanguage
            // 
            this.groupBoxCustomLanguage.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomLanguage.BackColorScaling = 0.5F;
            this.groupBoxCustomLanguage.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomLanguage.BorderColorScaling = 0.5F;
            this.groupBoxCustomLanguage.Controls.Add(this.comboBoxCustomLanguage);
            this.groupBoxCustomLanguage.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomLanguage.Location = new System.Drawing.Point(577, 344);
            this.groupBoxCustomLanguage.Name = "groupBoxCustomLanguage";
            this.groupBoxCustomLanguage.Size = new System.Drawing.Size(281, 52);
            this.groupBoxCustomLanguage.TabIndex = 21;
            this.groupBoxCustomLanguage.TabStop = false;
            this.groupBoxCustomLanguage.Text = "Language";
            this.groupBoxCustomLanguage.TextPadding = 0;
            this.groupBoxCustomLanguage.TextStartPosition = -1;
            // 
            // comboBoxCustomLanguage
            // 
            this.comboBoxCustomLanguage.BackColor = System.Drawing.Color.Gray;
            this.comboBoxCustomLanguage.BorderColor = System.Drawing.Color.Red;
            this.comboBoxCustomLanguage.ButtonColorScaling = 0.5F;
            this.comboBoxCustomLanguage.DataSource = null;
            this.comboBoxCustomLanguage.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomLanguage.DisplayMember = "";
            this.comboBoxCustomLanguage.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomLanguage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomLanguage.Location = new System.Drawing.Point(10, 19);
            this.comboBoxCustomLanguage.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomLanguage.Name = "comboBoxCustomLanguage";
            this.comboBoxCustomLanguage.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomLanguage.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomLanguage.SelectedIndex = -1;
            this.comboBoxCustomLanguage.SelectedItem = null;
            this.comboBoxCustomLanguage.SelectedValue = null;
            this.comboBoxCustomLanguage.Size = new System.Drawing.Size(246, 21);
            this.comboBoxCustomLanguage.TabIndex = 0;
            this.comboBoxCustomLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxCustomLanguage.ValueMember = "";
            // 
            // groupBoxCustomSafeMode
            // 
            this.groupBoxCustomSafeMode.AlternateClientBackColor = System.Drawing.Color.Blue;
            this.groupBoxCustomSafeMode.BackColorScaling = 0.5F;
            this.groupBoxCustomSafeMode.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomSafeMode.BorderColorScaling = 0.5F;
            this.groupBoxCustomSafeMode.Controls.Add(this.buttonExtSafeMode);
            this.groupBoxCustomSafeMode.Controls.Add(this.labelSafeMode);
            this.groupBoxCustomSafeMode.FillClientAreaWithAlternateColor = false;
            this.groupBoxCustomSafeMode.Location = new System.Drawing.Point(3, 479);
            this.groupBoxCustomSafeMode.Name = "groupBoxCustomSafeMode";
            this.groupBoxCustomSafeMode.Size = new System.Drawing.Size(281, 127);
            this.groupBoxCustomSafeMode.TabIndex = 21;
            this.groupBoxCustomSafeMode.TabStop = false;
            this.groupBoxCustomSafeMode.Text = "Advanced";
            this.groupBoxCustomSafeMode.TextPadding = 0;
            this.groupBoxCustomSafeMode.TextStartPosition = -1;
            // 
            // labelSafeMode
            // 
            this.labelSafeMode.Location = new System.Drawing.Point(10, 22);
            this.labelSafeMode.Name = "labelSafeMode";
            this.labelSafeMode.Size = new System.Drawing.Size(265, 62);
            this.labelSafeMode.TabIndex = 5;
            this.labelSafeMode.Text = "Click this to perform special operations like to move system databases to another" +
    " drive, reset UI, and other maintenance tasks...\r\n\r\n";
            // 
            // UserControlSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extPanelScroll);
            this.Name = "UserControlSettings";
            this.Size = new System.Drawing.Size(981, 623);
            this.extPanelScroll.ResumeLayout(false);
            this.groupBoxCommanders.ResumeLayout(false);
            this.dataViewScrollerCommanders.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommanders)).EndInit();
            this.flowLayoutButtons.ResumeLayout(false);
            this.groupBoxTheme.ResumeLayout(false);
            this.groupBoxCustomHistoryLoad.ResumeLayout(false);
            this.groupBoxCustomHistoryLoad.PerformLayout();
            this.extGroupBoxWebServer.ResumeLayout(false);
            this.extGroupBoxWebServer.PerformLayout();
            this.groupBoxInteraction.ResumeLayout(false);
            this.groupBoxInteraction.PerformLayout();
            this.groupBoxMemory.ResumeLayout(false);
            this.groupBoxMemory.PerformLayout();
            this.groupBoxCustomScreenShots.ResumeLayout(false);
            this.groupBoxCustomScreenShots.PerformLayout();
            this.groupBoxCustomEDSM.ResumeLayout(false);
            this.groupBoxCustomEDSM.PerformLayout();
            this.groupBoxPopOuts.ResumeLayout(false);
            this.groupBoxPopOuts.PerformLayout();
            this.groupBoxCustomLanguage.ResumeLayout(false);
            this.groupBoxCustomSafeMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtGroupBox groupBoxCommanders;
        private ExtendedControls.ExtButton buttonAddCommander;
        private System.Windows.Forms.DataGridView dataGridViewCommanders;
        private ExtendedControls.ExtComboBox comboBoxTheme;
        private System.Windows.Forms.ToolTip toolTip;
        private ExtendedControls.ExtButton buttonSaveTheme;
        private ExtendedControls.ExtButton button_edittheme;
        private ExtendedControls.ExtGroupBox groupBoxTheme;
        private ExtendedControls.ExtCheckBox checkBoxOrderRowsInverted;
        private ExtendedControls.ExtCheckBox checkBoxKeepOnTop;
        private ExtendedControls.ExtButton btnDeleteCommander;
        private ExtendedControls.ExtPanelDataGridViewScroll dataViewScrollerCommanders;
        private ExtendedControls.ExtScrollBar vScrollBarCommanders;
        private ExtendedControls.ExtGroupBox groupBoxPopOuts;
        private ExtendedControls.ExtCheckBox checkBoxMinimizeToNotifyIcon;
        private ExtendedControls.ExtCheckBox checkBoxUseNotifyIcon;
        private ExtendedControls.ExtButton buttonEditCommander;
        private ExtendedControls.ExtComboBox comboBoxClickThruKey;
        private System.Windows.Forms.Label labelTKey;
        private ExtendedControls.ExtGroupBox groupBoxCustomScreenShots;
        private ExtendedControls.ExtCheckBox checkBoxCustomMarkHiRes;
        private ExtendedControls.ExtCheckBox checkBoxCustomRemoveOriginals;
        private ExtendedControls.ExtButton buttonExtScreenshot;
        private ExtendedControls.ExtCheckBox checkBoxCustomEnableScreenshots;
        private ExtendedControls.ExtCheckBox checkBoxCustomCopyToClipboard;
        private ExtendedControls.ExtGroupBox groupBoxCustomEDSM;
        private ExtendedControls.ExtButton buttonExtEDSMConfigureArea;
        private ExtendedControls.ExtCheckBox checkBoxCustomEDSMEDDBDownload;
        private ExtendedControls.ExtGroupBox groupBoxMemory;
        private ExtendedControls.ExtComboBox comboBoxCustomHistoryLoadTime;
        private System.Windows.Forms.Label labelHistorySel;
        private ExtendedControls.ExtGroupBox groupBoxCustomLanguage;
        private ExtendedControls.ExtComboBox comboBoxCustomLanguage;
        private ExtendedControls.ExtCheckBox checkBoxCustomResize;
        private ExtendedControls.ExtCheckBox checkBoxPanelSortOrder;
        private ExtendedControls.ExtGroupBox groupBoxCustomSafeMode;
        private ExtendedControls.ExtButton buttonExtSafeMode;
        private System.Windows.Forms.Label labelSafeMode;
        private ExtendedControls.ExtComboBox comboBoxCustomEssentialEntries;
        private System.Windows.Forms.Label labelHistoryEssItems;
        private ExtendedControls.ExtPanelScroll extPanelScroll;
        private ExtendedControls.ExtScrollBar extScrollBarSettings;
        private ExtendedControls.ExtGroupBox extGroupBoxWebServer;
        private System.Windows.Forms.Label labelPortNo;
        private ExtendedControls.ExtButton extButtonTestWeb;
        private ExtendedControls.NumberBoxLong numberBoxLongPortNo;
        private ExtendedControls.ExtCheckBox extCheckBoxWebServerEnable;
        private ExtendedControls.ExtGroupBox groupBoxCustomHistoryLoad;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCommander;
        private System.Windows.Forms.DataGridViewTextBoxColumn EdsmName;
        private System.Windows.Forms.DataGridViewTextBoxColumn JournalDirCol;
        private System.Windows.Forms.DataGridViewTextBoxColumn NotesCol;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutButtons;
        private ExtendedControls.ExtGroupBox groupBoxInteraction;
        private ExtendedControls.ExtComboBox extComboBoxGameTime;
        private System.Windows.Forms.Label labelTimeDisplay;
    }
}
