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
    partial class Settings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.btnDeleteCommander = new ExtendedControls.ExtButton();
            this.buttonEditCommander = new ExtendedControls.ExtButton();
            this.buttonAddCommander = new ExtendedControls.ExtButton();
            this.comboBoxTheme = new ExtendedControls.ExtComboBox();
            this.button_edittheme = new ExtendedControls.ExtButton();
            this.buttonSaveTheme = new ExtendedControls.ExtButton();
            this.checkBoxOrderRowsInverted = new ExtendedControls.ExtCheckBox();
            this.comboBoxClickThruKey = new ExtendedControls.ExtComboBox();
            this.comboBoxCustomEssentialEntries = new ExtendedControls.ExtComboBox();
            this.comboBoxCustomHistoryLoadTime = new ExtendedControls.ExtComboBox();
            this.buttonExtScreenshot = new ExtendedControls.ExtButton();
            this.checkBoxCustomEnableScreenshots = new ExtendedControls.ExtCheckBox();
            this.buttonExtEDSMConfigureArea = new ExtendedControls.ExtButton();
            this.checkBoxCustomEDSMDownload = new ExtendedControls.ExtCheckBox();
            this.checkBoxPanelSortOrder = new ExtendedControls.ExtCheckBox();
            this.checkBoxKeepOnTop = new ExtendedControls.ExtCheckBox();
            this.checkBoxCustomResize = new ExtendedControls.ExtCheckBox();
            this.checkBoxMinimizeToNotifyIcon = new ExtendedControls.ExtCheckBox();
            this.checkBoxUseNotifyIcon = new ExtendedControls.ExtCheckBox();
            this.buttonExtSafeMode = new ExtendedControls.ExtButton();
            this.extPanelScroll = new ExtendedControls.ExtPanelScroll();
            this.extScrollBarSettings = new ExtendedControls.ExtScrollBar();
            this.groupBoxCommanders = new ExtendedControls.ExtGroupBox();
            this.dataViewScrollerCommanders = new ExtendedControls.ExtPanelDataGridViewScroll();
            this.dataGridViewCommanders = new BaseUtils.DataGridViewBaseEnhancements();
            this.ColumnCommander = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EdsmName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.JournalDirCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NotesCol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.vScrollBarCommanders = new ExtendedControls.ExtScrollBar();
            this.flowLayoutButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.extButtonDrawnHelpCommanders = new ExtendedControls.ExtButtonDrawn();
            this.extButtonUnDelete = new ExtendedControls.ExtButton();
            this.extSplitterResizeParentGroupBoxCommanders = new ExtendedControls.ExtSplitterResizeParent();
            this.groupBoxTheme = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpTheme = new ExtendedControls.ExtButtonDrawn();
            this.groupBoxCustomHistoryLoad = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpHistory = new ExtendedControls.ExtButtonDrawn();
            this.extComboBoxGameTime = new ExtendedControls.ExtComboBox();
            this.labelTimeDisplay = new System.Windows.Forms.Label();
            this.extGroupBoxWebServer = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpWebServer = new ExtendedControls.ExtButtonDrawn();
            this.numberBoxLongPortNo = new ExtendedControls.NumberBoxLong();
            this.labelPortNo = new System.Windows.Forms.Label();
            this.extButtonTestWeb = new ExtendedControls.ExtButton();
            this.extCheckBoxWebServerEnable = new ExtendedControls.ExtCheckBox();
            this.groupBoxInteraction = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpTransparency = new ExtendedControls.ExtButtonDrawn();
            this.labelTKey = new System.Windows.Forms.Label();
            this.groupBoxMemory = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpMemory = new ExtendedControls.ExtButtonDrawn();
            this.labelHistoryEssItems = new System.Windows.Forms.Label();
            this.labelHistorySel = new System.Windows.Forms.Label();
            this.groupBoxCustomScreenShots = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpScreenshots = new ExtendedControls.ExtButtonDrawn();
            this.groupBoxCustomEDSM = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpEDSM = new ExtendedControls.ExtButtonDrawn();
            this.extButtonReloadStarDatabase = new ExtendedControls.ExtButton();
            this.groupBoxPopOuts = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpWindowOptions = new ExtendedControls.ExtButtonDrawn();
            this.extGroupBoxDLLPerms = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpDLL = new ExtendedControls.ExtButtonDrawn();
            this.extButtonDLLConfigure = new ExtendedControls.ExtButton();
            this.extButtonDLLPerms = new ExtendedControls.ExtButton();
            this.extGroupBoxWebLookup = new ExtendedControls.ExtGroupBox();
            this.extComboBoxWebLookup = new ExtendedControls.ExtComboBox();
            this.groupBoxCustomLanguage = new ExtendedControls.ExtGroupBox();
            this.comboBoxCustomLanguage = new ExtendedControls.ExtComboBox();
            this.groupBoxCustomSafeMode = new ExtendedControls.ExtGroupBox();
            this.extButtonDrawnHelpSafeMode = new ExtendedControls.ExtButtonDrawn();
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
            this.extGroupBoxDLLPerms.SuspendLayout();
            this.extGroupBoxWebLookup.SuspendLayout();
            this.groupBoxCustomLanguage.SuspendLayout();
            this.groupBoxCustomSafeMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolTip
            // 
            this.toolTip.ShowAlways = true;
            // 
            // btnDeleteCommander
            // 
            this.btnDeleteCommander.BackColor2 = System.Drawing.Color.Red;
            this.btnDeleteCommander.ButtonDisabledScaling = 0.5F;
            this.btnDeleteCommander.GradientDirection = 90F;
            this.btnDeleteCommander.Location = new System.Drawing.Point(823, 3);
            this.btnDeleteCommander.MouseOverScaling = 1.3F;
            this.btnDeleteCommander.MouseSelectedScaling = 1.3F;
            this.btnDeleteCommander.Name = "btnDeleteCommander";
            this.btnDeleteCommander.Size = new System.Drawing.Size(100, 23);
            this.btnDeleteCommander.TabIndex = 3;
            this.btnDeleteCommander.Text = "Delete";
            this.toolTip.SetToolTip(this.btnDeleteCommander, "Delete selected commander");
            this.btnDeleteCommander.UseVisualStyleBackColor = true;
            this.btnDeleteCommander.Click += new System.EventHandler(this.btnDeleteCommander_Click);
            // 
            // buttonEditCommander
            // 
            this.buttonEditCommander.BackColor2 = System.Drawing.Color.Red;
            this.buttonEditCommander.ButtonDisabledScaling = 0.5F;
            this.buttonEditCommander.GradientDirection = 90F;
            this.buttonEditCommander.Location = new System.Drawing.Point(717, 3);
            this.buttonEditCommander.MouseOverScaling = 1.3F;
            this.buttonEditCommander.MouseSelectedScaling = 1.3F;
            this.buttonEditCommander.Name = "buttonEditCommander";
            this.buttonEditCommander.Size = new System.Drawing.Size(100, 23);
            this.buttonEditCommander.TabIndex = 5;
            this.buttonEditCommander.Text = "Edit";
            this.toolTip.SetToolTip(this.buttonEditCommander, "Edit selected commander");
            this.buttonEditCommander.UseVisualStyleBackColor = true;
            this.buttonEditCommander.Click += new System.EventHandler(this.buttonEditCommander_Click);
            // 
            // buttonAddCommander
            // 
            this.buttonAddCommander.BackColor2 = System.Drawing.Color.Red;
            this.buttonAddCommander.ButtonDisabledScaling = 0.5F;
            this.buttonAddCommander.GradientDirection = 90F;
            this.buttonAddCommander.Location = new System.Drawing.Point(611, 3);
            this.buttonAddCommander.MouseOverScaling = 1.3F;
            this.buttonAddCommander.MouseSelectedScaling = 1.3F;
            this.buttonAddCommander.Name = "buttonAddCommander";
            this.buttonAddCommander.Size = new System.Drawing.Size(100, 23);
            this.buttonAddCommander.TabIndex = 0;
            this.buttonAddCommander.Text = "Add";
            this.toolTip.SetToolTip(this.buttonAddCommander, "Add a new commander");
            this.buttonAddCommander.UseVisualStyleBackColor = true;
            this.buttonAddCommander.Click += new System.EventHandler(this.buttonAddCommander_Click);
            // 
            // comboBoxTheme
            // 
            this.comboBoxTheme.BackColor = System.Drawing.Color.Gray;
            this.comboBoxTheme.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxTheme.BorderColor = System.Drawing.Color.Red;
            this.comboBoxTheme.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxTheme.DataSource = null;
            this.comboBoxTheme.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxTheme.DisabledScaling = 0.5F;
            this.comboBoxTheme.DisplayMember = "";
            this.comboBoxTheme.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxTheme.GradientDirection = 90F;
            this.comboBoxTheme.Location = new System.Drawing.Point(10, 19);
            this.comboBoxTheme.MouseOverScalingColor = 1.3F;
            this.comboBoxTheme.Name = "comboBoxTheme";
            this.comboBoxTheme.SelectedIndex = -1;
            this.comboBoxTheme.SelectedItem = null;
            this.comboBoxTheme.SelectedValue = null;
            this.comboBoxTheme.Size = new System.Drawing.Size(242, 21);
            this.comboBoxTheme.TabIndex = 0;
            this.comboBoxTheme.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxTheme, "Select the theme to use");
            this.comboBoxTheme.ValueMember = "";
            // 
            // button_edittheme
            // 
            this.button_edittheme.BackColor2 = System.Drawing.Color.Red;
            this.button_edittheme.ButtonDisabledScaling = 0.5F;
            this.button_edittheme.GradientDirection = 90F;
            this.button_edittheme.Location = new System.Drawing.Point(152, 48);
            this.button_edittheme.MouseOverScaling = 1.3F;
            this.button_edittheme.MouseSelectedScaling = 1.3F;
            this.button_edittheme.Name = "button_edittheme";
            this.button_edittheme.Size = new System.Drawing.Size(100, 23);
            this.button_edittheme.TabIndex = 10;
            this.button_edittheme.Text = "Edit Theme";
            this.toolTip.SetToolTip(this.button_edittheme, "Edit theme and change colours fonts");
            this.button_edittheme.UseVisualStyleBackColor = true;
            this.button_edittheme.Click += new System.EventHandler(this.button_edittheme_Click);
            // 
            // buttonSaveTheme
            // 
            this.buttonSaveTheme.BackColor2 = System.Drawing.Color.Red;
            this.buttonSaveTheme.ButtonDisabledScaling = 0.5F;
            this.buttonSaveTheme.GradientDirection = 90F;
            this.buttonSaveTheme.Location = new System.Drawing.Point(9, 48);
            this.buttonSaveTheme.MouseOverScaling = 1.3F;
            this.buttonSaveTheme.MouseSelectedScaling = 1.3F;
            this.buttonSaveTheme.Name = "buttonSaveTheme";
            this.buttonSaveTheme.Size = new System.Drawing.Size(100, 23);
            this.buttonSaveTheme.TabIndex = 7;
            this.buttonSaveTheme.Text = "Save Theme";
            this.toolTip.SetToolTip(this.buttonSaveTheme, "Save theme to disk");
            this.buttonSaveTheme.UseVisualStyleBackColor = true;
            this.buttonSaveTheme.Click += new System.EventHandler(this.buttonSaveTheme_Click);
            // 
            // checkBoxOrderRowsInverted
            // 
            this.checkBoxOrderRowsInverted.AutoSize = true;
            this.checkBoxOrderRowsInverted.ButtonGradientDirection = 90F;
            this.checkBoxOrderRowsInverted.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxOrderRowsInverted.CheckBoxGradientDirection = 225F;
            this.checkBoxOrderRowsInverted.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxOrderRowsInverted.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxOrderRowsInverted.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxOrderRowsInverted.DisabledScaling = 0.5F;
            this.checkBoxOrderRowsInverted.ImageIndeterminate = null;
            this.checkBoxOrderRowsInverted.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxOrderRowsInverted.ImageUnchecked = null;
            this.checkBoxOrderRowsInverted.Location = new System.Drawing.Point(9, 23);
            this.checkBoxOrderRowsInverted.MouseOverScaling = 1.3F;
            this.checkBoxOrderRowsInverted.MouseSelectedScaling = 1.3F;
            this.checkBoxOrderRowsInverted.Name = "checkBoxOrderRowsInverted";
            this.checkBoxOrderRowsInverted.Size = new System.Drawing.Size(186, 17);
            this.checkBoxOrderRowsInverted.TabIndex = 2;
            this.checkBoxOrderRowsInverted.Text = "Number Rows by Date Ascending";
            this.checkBoxOrderRowsInverted.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxOrderRowsInverted, "Number oldest entry 1, latest entry highest");
            this.checkBoxOrderRowsInverted.UseVisualStyleBackColor = true;
            // 
            // comboBoxClickThruKey
            // 
            this.comboBoxClickThruKey.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxClickThruKey.BorderColor = System.Drawing.Color.White;
            this.comboBoxClickThruKey.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxClickThruKey.DataSource = null;
            this.comboBoxClickThruKey.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxClickThruKey.DisabledScaling = 0.5F;
            this.comboBoxClickThruKey.DisplayMember = "";
            this.comboBoxClickThruKey.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxClickThruKey.GradientDirection = 90F;
            this.comboBoxClickThruKey.Location = new System.Drawing.Point(9, 48);
            this.comboBoxClickThruKey.MouseOverScalingColor = 1.3F;
            this.comboBoxClickThruKey.Name = "comboBoxClickThruKey";
            this.comboBoxClickThruKey.SelectedIndex = -1;
            this.comboBoxClickThruKey.SelectedItem = null;
            this.comboBoxClickThruKey.SelectedValue = null;
            this.comboBoxClickThruKey.Size = new System.Drawing.Size(243, 21);
            this.comboBoxClickThruKey.TabIndex = 6;
            this.comboBoxClickThruKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxClickThruKey, resources.GetString("comboBoxClickThruKey.ToolTip"));
            this.comboBoxClickThruKey.ValueMember = "";
            // 
            // comboBoxCustomEssentialEntries
            // 
            this.comboBoxCustomEssentialEntries.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxCustomEssentialEntries.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomEssentialEntries.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxCustomEssentialEntries.DataSource = null;
            this.comboBoxCustomEssentialEntries.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomEssentialEntries.DisabledScaling = 0.5F;
            this.comboBoxCustomEssentialEntries.DisplayMember = "";
            this.comboBoxCustomEssentialEntries.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomEssentialEntries.GradientDirection = 90F;
            this.comboBoxCustomEssentialEntries.Location = new System.Drawing.Point(128, 48);
            this.comboBoxCustomEssentialEntries.MouseOverScalingColor = 1.3F;
            this.comboBoxCustomEssentialEntries.Name = "comboBoxCustomEssentialEntries";
            this.comboBoxCustomEssentialEntries.SelectedIndex = -1;
            this.comboBoxCustomEssentialEntries.SelectedItem = null;
            this.comboBoxCustomEssentialEntries.SelectedValue = null;
            this.comboBoxCustomEssentialEntries.Size = new System.Drawing.Size(124, 21);
            this.comboBoxCustomEssentialEntries.TabIndex = 7;
            this.comboBoxCustomEssentialEntries.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCustomEssentialEntries, "Select which items you consider essential to load older than the time above");
            this.comboBoxCustomEssentialEntries.ValueMember = "";
            // 
            // comboBoxCustomHistoryLoadTime
            // 
            this.comboBoxCustomHistoryLoadTime.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxCustomHistoryLoadTime.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomHistoryLoadTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxCustomHistoryLoadTime.DataSource = null;
            this.comboBoxCustomHistoryLoadTime.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomHistoryLoadTime.DisabledScaling = 0.5F;
            this.comboBoxCustomHistoryLoadTime.DisplayMember = "";
            this.comboBoxCustomHistoryLoadTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomHistoryLoadTime.GradientDirection = 90F;
            this.comboBoxCustomHistoryLoadTime.Location = new System.Drawing.Point(128, 19);
            this.comboBoxCustomHistoryLoadTime.MouseOverScalingColor = 1.3F;
            this.comboBoxCustomHistoryLoadTime.Name = "comboBoxCustomHistoryLoadTime";
            this.comboBoxCustomHistoryLoadTime.SelectedIndex = -1;
            this.comboBoxCustomHistoryLoadTime.SelectedItem = null;
            this.comboBoxCustomHistoryLoadTime.SelectedValue = null;
            this.comboBoxCustomHistoryLoadTime.Size = new System.Drawing.Size(124, 21);
            this.comboBoxCustomHistoryLoadTime.TabIndex = 7;
            this.comboBoxCustomHistoryLoadTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.toolTip.SetToolTip(this.comboBoxCustomHistoryLoadTime, "Reduce Memory use. Select either load all records, or load only essential items o" +
        "f records older than a set time before now");
            this.comboBoxCustomHistoryLoadTime.ValueMember = "";
            // 
            // buttonExtScreenshot
            // 
            this.buttonExtScreenshot.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtScreenshot.ButtonDisabledScaling = 0.5F;
            this.buttonExtScreenshot.GradientDirection = 90F;
            this.buttonExtScreenshot.Location = new System.Drawing.Point(10, 48);
            this.buttonExtScreenshot.MouseOverScaling = 1.3F;
            this.buttonExtScreenshot.MouseSelectedScaling = 1.3F;
            this.buttonExtScreenshot.Name = "buttonExtScreenshot";
            this.buttonExtScreenshot.Size = new System.Drawing.Size(100, 23);
            this.buttonExtScreenshot.TabIndex = 10;
            this.buttonExtScreenshot.Text = "Configure";
            this.toolTip.SetToolTip(this.buttonExtScreenshot, "Configure further screenshot options");
            this.buttonExtScreenshot.UseVisualStyleBackColor = true;
            this.buttonExtScreenshot.Click += new System.EventHandler(this.buttonExtScreenshot_Click);
            // 
            // checkBoxCustomEnableScreenshots
            // 
            this.checkBoxCustomEnableScreenshots.AutoSize = true;
            this.checkBoxCustomEnableScreenshots.ButtonGradientDirection = 90F;
            this.checkBoxCustomEnableScreenshots.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEnableScreenshots.CheckBoxGradientDirection = 225F;
            this.checkBoxCustomEnableScreenshots.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEnableScreenshots.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEnableScreenshots.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEnableScreenshots.DisabledScaling = 0.5F;
            this.checkBoxCustomEnableScreenshots.ImageIndeterminate = null;
            this.checkBoxCustomEnableScreenshots.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomEnableScreenshots.ImageUnchecked = null;
            this.checkBoxCustomEnableScreenshots.Location = new System.Drawing.Point(10, 23);
            this.checkBoxCustomEnableScreenshots.MouseOverScaling = 1.3F;
            this.checkBoxCustomEnableScreenshots.MouseSelectedScaling = 1.3F;
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
            this.buttonExtEDSMConfigureArea.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtEDSMConfigureArea.ButtonDisabledScaling = 0.5F;
            this.buttonExtEDSMConfigureArea.GradientDirection = 90F;
            this.buttonExtEDSMConfigureArea.Location = new System.Drawing.Point(9, 43);
            this.buttonExtEDSMConfigureArea.MouseOverScaling = 1.3F;
            this.buttonExtEDSMConfigureArea.MouseSelectedScaling = 1.3F;
            this.buttonExtEDSMConfigureArea.Name = "buttonExtEDSMConfigureArea";
            this.buttonExtEDSMConfigureArea.Size = new System.Drawing.Size(243, 23);
            this.buttonExtEDSMConfigureArea.TabIndex = 10;
            this.buttonExtEDSMConfigureArea.Text = "Select Galaxy Sectors";
            this.toolTip.SetToolTip(this.buttonExtEDSMConfigureArea, "Configure what parts of the galaxy is stored in the databases");
            this.buttonExtEDSMConfigureArea.UseVisualStyleBackColor = true;
            this.buttonExtEDSMConfigureArea.Click += new System.EventHandler(this.buttonExtSystemDBConfigureArea_Click);
            // 
            // checkBoxCustomEDSMDownload
            // 
            this.checkBoxCustomEDSMDownload.AutoSize = true;
            this.checkBoxCustomEDSMDownload.ButtonGradientDirection = 90F;
            this.checkBoxCustomEDSMDownload.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomEDSMDownload.CheckBoxGradientDirection = 225F;
            this.checkBoxCustomEDSMDownload.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomEDSMDownload.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDSMDownload.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomEDSMDownload.DisabledScaling = 0.5F;
            this.checkBoxCustomEDSMDownload.ImageIndeterminate = null;
            this.checkBoxCustomEDSMDownload.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomEDSMDownload.ImageUnchecked = null;
            this.checkBoxCustomEDSMDownload.Location = new System.Drawing.Point(9, 19);
            this.checkBoxCustomEDSMDownload.MouseOverScaling = 1.3F;
            this.checkBoxCustomEDSMDownload.MouseSelectedScaling = 1.3F;
            this.checkBoxCustomEDSMDownload.Name = "checkBoxCustomEDSMDownload";
            this.checkBoxCustomEDSMDownload.Size = new System.Drawing.Size(158, 17);
            this.checkBoxCustomEDSMDownload.TabIndex = 5;
            this.checkBoxCustomEDSMDownload.Text = "Enable Star Data Download";
            this.checkBoxCustomEDSMDownload.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxCustomEDSMDownload, "Click to enable downloading of stars from EDSM or Spansh. Will apply at next star" +
        "t.");
            this.checkBoxCustomEDSMDownload.UseVisualStyleBackColor = true;
            // 
            // checkBoxPanelSortOrder
            // 
            this.checkBoxPanelSortOrder.AutoSize = true;
            this.checkBoxPanelSortOrder.ButtonGradientDirection = 90F;
            this.checkBoxPanelSortOrder.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxPanelSortOrder.CheckBoxGradientDirection = 225F;
            this.checkBoxPanelSortOrder.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxPanelSortOrder.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxPanelSortOrder.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxPanelSortOrder.DisabledScaling = 0.5F;
            this.checkBoxPanelSortOrder.ImageIndeterminate = null;
            this.checkBoxPanelSortOrder.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxPanelSortOrder.ImageUnchecked = null;
            this.checkBoxPanelSortOrder.Location = new System.Drawing.Point(13, 103);
            this.checkBoxPanelSortOrder.MouseOverScaling = 1.3F;
            this.checkBoxPanelSortOrder.MouseSelectedScaling = 1.3F;
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
            this.checkBoxKeepOnTop.ButtonGradientDirection = 90F;
            this.checkBoxKeepOnTop.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxKeepOnTop.CheckBoxGradientDirection = 225F;
            this.checkBoxKeepOnTop.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxKeepOnTop.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxKeepOnTop.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxKeepOnTop.DisabledScaling = 0.5F;
            this.checkBoxKeepOnTop.ImageIndeterminate = null;
            this.checkBoxKeepOnTop.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxKeepOnTop.ImageUnchecked = null;
            this.checkBoxKeepOnTop.Location = new System.Drawing.Point(13, 80);
            this.checkBoxKeepOnTop.MouseOverScaling = 1.3F;
            this.checkBoxKeepOnTop.MouseSelectedScaling = 1.3F;
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
            this.checkBoxCustomResize.ButtonGradientDirection = 90F;
            this.checkBoxCustomResize.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomResize.CheckBoxGradientDirection = 225F;
            this.checkBoxCustomResize.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomResize.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomResize.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomResize.DisabledScaling = 0.5F;
            this.checkBoxCustomResize.ImageIndeterminate = null;
            this.checkBoxCustomResize.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxCustomResize.ImageUnchecked = null;
            this.checkBoxCustomResize.Location = new System.Drawing.Point(13, 59);
            this.checkBoxCustomResize.MouseOverScaling = 1.3F;
            this.checkBoxCustomResize.MouseSelectedScaling = 1.3F;
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
            this.checkBoxMinimizeToNotifyIcon.ButtonGradientDirection = 90F;
            this.checkBoxMinimizeToNotifyIcon.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxMinimizeToNotifyIcon.CheckBoxGradientDirection = 225F;
            this.checkBoxMinimizeToNotifyIcon.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxMinimizeToNotifyIcon.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxMinimizeToNotifyIcon.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxMinimizeToNotifyIcon.DisabledScaling = 0.5F;
            this.checkBoxMinimizeToNotifyIcon.ImageIndeterminate = null;
            this.checkBoxMinimizeToNotifyIcon.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxMinimizeToNotifyIcon.ImageUnchecked = null;
            this.checkBoxMinimizeToNotifyIcon.Location = new System.Drawing.Point(13, 39);
            this.checkBoxMinimizeToNotifyIcon.MouseOverScaling = 1.3F;
            this.checkBoxMinimizeToNotifyIcon.MouseSelectedScaling = 1.3F;
            this.checkBoxMinimizeToNotifyIcon.Name = "checkBoxMinimizeToNotifyIcon";
            this.checkBoxMinimizeToNotifyIcon.Size = new System.Drawing.Size(179, 17);
            this.checkBoxMinimizeToNotifyIcon.TabIndex = 6;
            this.checkBoxMinimizeToNotifyIcon.Text = "Minimize to notification area icon";
            this.checkBoxMinimizeToNotifyIcon.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxMinimizeToNotifyIcon, "Minimize the main window to the system notification area (system tray) icon.");
            this.checkBoxMinimizeToNotifyIcon.UseVisualStyleBackColor = true;
            // 
            // checkBoxUseNotifyIcon
            // 
            this.checkBoxUseNotifyIcon.AutoSize = true;
            this.checkBoxUseNotifyIcon.ButtonGradientDirection = 90F;
            this.checkBoxUseNotifyIcon.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxUseNotifyIcon.CheckBoxGradientDirection = 225F;
            this.checkBoxUseNotifyIcon.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxUseNotifyIcon.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxUseNotifyIcon.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.checkBoxUseNotifyIcon.DisabledScaling = 0.5F;
            this.checkBoxUseNotifyIcon.ImageIndeterminate = null;
            this.checkBoxUseNotifyIcon.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.checkBoxUseNotifyIcon.ImageUnchecked = null;
            this.checkBoxUseNotifyIcon.Location = new System.Drawing.Point(13, 18);
            this.checkBoxUseNotifyIcon.MouseOverScaling = 1.3F;
            this.checkBoxUseNotifyIcon.MouseSelectedScaling = 1.3F;
            this.checkBoxUseNotifyIcon.Name = "checkBoxUseNotifyIcon";
            this.checkBoxUseNotifyIcon.Size = new System.Drawing.Size(154, 17);
            this.checkBoxUseNotifyIcon.TabIndex = 5;
            this.checkBoxUseNotifyIcon.Text = "Show notification area icon";
            this.checkBoxUseNotifyIcon.TickBoxReductionRatio = 0.75F;
            this.toolTip.SetToolTip(this.checkBoxUseNotifyIcon, "Show a system notification area (system tray) icon for EDDiscovery.");
            this.checkBoxUseNotifyIcon.UseVisualStyleBackColor = true;
            // 
            // buttonExtSafeMode
            // 
            this.buttonExtSafeMode.BackColor2 = System.Drawing.Color.Red;
            this.buttonExtSafeMode.ButtonDisabledScaling = 0.5F;
            this.buttonExtSafeMode.GradientDirection = 90F;
            this.buttonExtSafeMode.Location = new System.Drawing.Point(16, 94);
            this.buttonExtSafeMode.MouseOverScaling = 1.3F;
            this.buttonExtSafeMode.MouseSelectedScaling = 1.3F;
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
            this.extPanelScroll.Controls.Add(this.extGroupBoxWebLookup);
            this.extPanelScroll.Controls.Add(this.extGroupBoxDLLPerms);
            this.extPanelScroll.Controls.Add(this.groupBoxCustomLanguage);
            this.extPanelScroll.Controls.Add(this.groupBoxCustomEDSM);
            this.extPanelScroll.Controls.Add(this.groupBoxPopOuts);
            this.extPanelScroll.Controls.Add(this.groupBoxCustomSafeMode);
            this.extPanelScroll.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extPanelScroll.FlowControlsLeftToRight = true;
            this.extPanelScroll.Location = new System.Drawing.Point(0, 0);
            this.extPanelScroll.Name = "extPanelScroll";
            this.extPanelScroll.ScrollBarWidth = 24;
            this.extPanelScroll.Size = new System.Drawing.Size(1086, 802);
            this.extPanelScroll.TabIndex = 22;
            this.extPanelScroll.VerticalScrollBarDockRight = true;
            // 
            // extScrollBarSettings
            // 
            this.extScrollBarSettings.AlwaysHideScrollBar = false;
            this.extScrollBarSettings.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.extScrollBarSettings.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.extScrollBarSettings.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.extScrollBarSettings.ArrowDownDrawAngle = 270F;
            this.extScrollBarSettings.ArrowUpDrawAngle = 90F;
            this.extScrollBarSettings.BorderColor = System.Drawing.Color.White;
            this.extScrollBarSettings.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extScrollBarSettings.HideScrollBar = true;
            this.extScrollBarSettings.LargeChange = 10;
            this.extScrollBarSettings.Location = new System.Drawing.Point(1062, 0);
            this.extScrollBarSettings.Maximum = -184;
            this.extScrollBarSettings.Minimum = 0;
            this.extScrollBarSettings.MouseOverButtonColor = System.Drawing.Color.Green;
            this.extScrollBarSettings.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.extScrollBarSettings.MousePressedButtonColor = System.Drawing.Color.Red;
            this.extScrollBarSettings.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.extScrollBarSettings.Name = "extScrollBarSettings";
            this.extScrollBarSettings.Size = new System.Drawing.Size(24, 802);
            this.extScrollBarSettings.SkinnyStyle = ExtendedControls.ExtScrollBar.ScrollStyle.Normal;
            this.extScrollBarSettings.SliderColor = System.Drawing.Color.DarkGray;
            this.extScrollBarSettings.SliderColor2 = System.Drawing.Color.DarkGray;
            this.extScrollBarSettings.SliderDrawAngle = 90F;
            this.extScrollBarSettings.SmallChange = 1;
            this.extScrollBarSettings.TabIndex = 22;
            this.extScrollBarSettings.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.extScrollBarSettings.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.extScrollBarSettings.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.extScrollBarSettings.ThumbDrawAngle = 0F;
            this.extScrollBarSettings.Value = -184;
            this.extScrollBarSettings.ValueLimited = -184;
            // 
            // groupBoxCommanders
            // 
            this.groupBoxCommanders.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCommanders.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxCommanders.ChildrenThemed = true;
            this.groupBoxCommanders.Controls.Add(this.dataViewScrollerCommanders);
            this.groupBoxCommanders.Controls.Add(this.flowLayoutButtons);
            this.groupBoxCommanders.Controls.Add(this.extSplitterResizeParentGroupBoxCommanders);
            this.groupBoxCommanders.GradientDirection = 0F;
            this.groupBoxCommanders.Location = new System.Drawing.Point(3, 3);
            this.groupBoxCommanders.Name = "groupBoxCommanders";
            this.groupBoxCommanders.Padding = new System.Windows.Forms.Padding(3, 3, 6, 3);
            this.groupBoxCommanders.Size = new System.Drawing.Size(965, 153);
            this.groupBoxCommanders.TabIndex = 15;
            this.groupBoxCommanders.TabStop = false;
            this.groupBoxCommanders.Text = "Commanders";
            this.groupBoxCommanders.TextPadding = 0;
            this.groupBoxCommanders.TextStartPosition = -1;
            this.groupBoxCommanders.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxCommanders.ThemeColorSet = -1;
            // 
            // dataViewScrollerCommanders
            // 
            this.dataViewScrollerCommanders.Controls.Add(this.dataGridViewCommanders);
            this.dataViewScrollerCommanders.Controls.Add(this.vScrollBarCommanders);
            this.dataViewScrollerCommanders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataViewScrollerCommanders.InternalMargin = new System.Windows.Forms.Padding(0);
            this.dataViewScrollerCommanders.Location = new System.Drawing.Point(3, 16);
            this.dataViewScrollerCommanders.Name = "dataViewScrollerCommanders";
            this.dataViewScrollerCommanders.ScrollBarWidth = 24;
            this.dataViewScrollerCommanders.Size = new System.Drawing.Size(956, 101);
            this.dataViewScrollerCommanders.TabIndex = 4;
            this.dataViewScrollerCommanders.VerticalScrollBarDockRight = true;
            // 
            // dataGridViewCommanders
            // 
            this.dataGridViewCommanders.AllowUserToAddRows = false;
            this.dataGridViewCommanders.AllowUserToDeleteRows = false;
            this.dataGridViewCommanders.AllowUserToResizeRows = false;
            this.dataGridViewCommanders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewCommanders.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewCommanders.AutoSortByColumnName = false;
            this.dataGridViewCommanders.ColumnHeaderMenuStrip = null;
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
            this.dataGridViewCommanders.RowHeaderMenuStrip = null;
            this.dataGridViewCommanders.RowHeadersVisible = false;
            this.dataGridViewCommanders.RowHeadersWidth = 20;
            this.dataGridViewCommanders.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.dataGridViewCommanders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewCommanders.SingleRowSelect = true;
            this.dataGridViewCommanders.Size = new System.Drawing.Size(932, 101);
            this.dataGridViewCommanders.TabIndex = 2;
            this.dataGridViewCommanders.TopLeftHeaderMenuStrip = null;
            this.dataGridViewCommanders.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewCommanders_CellDoubleClick);
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
            this.vScrollBarCommanders.AlwaysHideScrollBar = false;
            this.vScrollBarCommanders.ArrowBorderColor = System.Drawing.Color.LightBlue;
            this.vScrollBarCommanders.ArrowButtonColor = System.Drawing.Color.LightGray;
            this.vScrollBarCommanders.ArrowButtonColor2 = System.Drawing.Color.LightGray;
            this.vScrollBarCommanders.ArrowDownDrawAngle = 270F;
            this.vScrollBarCommanders.ArrowUpDrawAngle = 90F;
            this.vScrollBarCommanders.BorderColor = System.Drawing.Color.White;
            this.vScrollBarCommanders.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScrollBarCommanders.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.vScrollBarCommanders.HideScrollBar = false;
            this.vScrollBarCommanders.LargeChange = 0;
            this.vScrollBarCommanders.Location = new System.Drawing.Point(932, 0);
            this.vScrollBarCommanders.Maximum = -1;
            this.vScrollBarCommanders.Minimum = 0;
            this.vScrollBarCommanders.MouseOverButtonColor = System.Drawing.Color.Green;
            this.vScrollBarCommanders.MouseOverButtonColor2 = System.Drawing.Color.Green;
            this.vScrollBarCommanders.MousePressedButtonColor = System.Drawing.Color.Red;
            this.vScrollBarCommanders.MousePressedButtonColor2 = System.Drawing.Color.Red;
            this.vScrollBarCommanders.Name = "vScrollBarCommanders";
            this.vScrollBarCommanders.Size = new System.Drawing.Size(24, 101);
            this.vScrollBarCommanders.SkinnyStyle = ExtendedControls.ExtScrollBar.ScrollStyle.Normal;
            this.vScrollBarCommanders.SliderColor = System.Drawing.Color.DarkGray;
            this.vScrollBarCommanders.SliderColor2 = System.Drawing.Color.DarkGray;
            this.vScrollBarCommanders.SliderDrawAngle = 90F;
            this.vScrollBarCommanders.SmallChange = 1;
            this.vScrollBarCommanders.TabIndex = 3;
            this.vScrollBarCommanders.ThumbBorderColor = System.Drawing.Color.Yellow;
            this.vScrollBarCommanders.ThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.vScrollBarCommanders.ThumbButtonColor2 = System.Drawing.Color.DarkBlue;
            this.vScrollBarCommanders.ThumbDrawAngle = 0F;
            this.vScrollBarCommanders.Value = -1;
            this.vScrollBarCommanders.ValueLimited = -1;
            // 
            // flowLayoutButtons
            // 
            this.flowLayoutButtons.Controls.Add(this.extButtonDrawnHelpCommanders);
            this.flowLayoutButtons.Controls.Add(this.btnDeleteCommander);
            this.flowLayoutButtons.Controls.Add(this.buttonEditCommander);
            this.flowLayoutButtons.Controls.Add(this.buttonAddCommander);
            this.flowLayoutButtons.Controls.Add(this.extButtonUnDelete);
            this.flowLayoutButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutButtons.Location = new System.Drawing.Point(3, 117);
            this.flowLayoutButtons.Name = "flowLayoutButtons";
            this.flowLayoutButtons.Size = new System.Drawing.Size(956, 30);
            this.flowLayoutButtons.TabIndex = 6;
            // 
            // extButtonDrawnHelpCommanders
            // 
            this.extButtonDrawnHelpCommanders.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpCommanders.AutoEllipsis = false;
            this.extButtonDrawnHelpCommanders.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpCommanders.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpCommanders.BorderWidth = 1;
            this.extButtonDrawnHelpCommanders.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpCommanders.Image = null;
            this.extButtonDrawnHelpCommanders.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpCommanders.Location = new System.Drawing.Point(929, 3);
            this.extButtonDrawnHelpCommanders.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpCommanders.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpCommanders.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpCommanders.Name = "extButtonDrawnHelpCommanders";
            this.extButtonDrawnHelpCommanders.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpCommanders.Selectable = true;
            this.extButtonDrawnHelpCommanders.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpCommanders.TabIndex = 26;
            this.extButtonDrawnHelpCommanders.Text = "?";
            this.extButtonDrawnHelpCommanders.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpCommanders.UseMnemonic = true;
            this.extButtonDrawnHelpCommanders.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // extButtonUnDelete
            // 
            this.extButtonUnDelete.BackColor2 = System.Drawing.Color.Red;
            this.extButtonUnDelete.ButtonDisabledScaling = 0.5F;
            this.extButtonUnDelete.GradientDirection = 90F;
            this.extButtonUnDelete.Location = new System.Drawing.Point(505, 3);
            this.extButtonUnDelete.MouseOverScaling = 1.3F;
            this.extButtonUnDelete.MouseSelectedScaling = 1.3F;
            this.extButtonUnDelete.Name = "extButtonUnDelete";
            this.extButtonUnDelete.Size = new System.Drawing.Size(100, 23);
            this.extButtonUnDelete.TabIndex = 0;
            this.extButtonUnDelete.Text = "Undelete";
            this.extButtonUnDelete.UseVisualStyleBackColor = true;
            this.extButtonUnDelete.Click += new System.EventHandler(this.extButtonUnDelete_Click);
            // 
            // extSplitterResizeParentGroupBoxCommanders
            // 
            this.extSplitterResizeParentGroupBoxCommanders.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.extSplitterResizeParentGroupBoxCommanders.Location = new System.Drawing.Point(3, 147);
            this.extSplitterResizeParentGroupBoxCommanders.MaxSize = 2147483647;
            this.extSplitterResizeParentGroupBoxCommanders.MinSize = 10;
            this.extSplitterResizeParentGroupBoxCommanders.Name = "extSplitterResizeParentGroupBoxCommanders";
            this.extSplitterResizeParentGroupBoxCommanders.Size = new System.Drawing.Size(956, 3);
            this.extSplitterResizeParentGroupBoxCommanders.TabIndex = 27;
            this.extSplitterResizeParentGroupBoxCommanders.TabStop = false;
            // 
            // groupBoxTheme
            // 
            this.groupBoxTheme.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxTheme.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxTheme.ChildrenThemed = true;
            this.groupBoxTheme.Controls.Add(this.extButtonDrawnHelpTheme);
            this.groupBoxTheme.Controls.Add(this.comboBoxTheme);
            this.groupBoxTheme.Controls.Add(this.button_edittheme);
            this.groupBoxTheme.Controls.Add(this.buttonSaveTheme);
            this.groupBoxTheme.GradientDirection = 0F;
            this.groupBoxTheme.Location = new System.Drawing.Point(3, 162);
            this.groupBoxTheme.Name = "groupBoxTheme";
            this.groupBoxTheme.Size = new System.Drawing.Size(281, 85);
            this.groupBoxTheme.TabIndex = 18;
            this.groupBoxTheme.TabStop = false;
            this.groupBoxTheme.Text = "Theme";
            this.groupBoxTheme.TextPadding = 0;
            this.groupBoxTheme.TextStartPosition = -1;
            this.groupBoxTheme.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxTheme.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpTheme
            // 
            this.extButtonDrawnHelpTheme.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpTheme.AutoEllipsis = false;
            this.extButtonDrawnHelpTheme.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpTheme.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpTheme.BorderWidth = 1;
            this.extButtonDrawnHelpTheme.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpTheme.Image = null;
            this.extButtonDrawnHelpTheme.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpTheme.Location = new System.Drawing.Point(253, 12);
            this.extButtonDrawnHelpTheme.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpTheme.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpTheme.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpTheme.Name = "extButtonDrawnHelpTheme";
            this.extButtonDrawnHelpTheme.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpTheme.Selectable = true;
            this.extButtonDrawnHelpTheme.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpTheme.TabIndex = 26;
            this.extButtonDrawnHelpTheme.Text = "?";
            this.extButtonDrawnHelpTheme.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpTheme.UseMnemonic = true;
            this.extButtonDrawnHelpTheme.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // groupBoxCustomHistoryLoad
            // 
            this.groupBoxCustomHistoryLoad.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomHistoryLoad.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxCustomHistoryLoad.ChildrenThemed = true;
            this.groupBoxCustomHistoryLoad.Controls.Add(this.extButtonDrawnHelpHistory);
            this.groupBoxCustomHistoryLoad.Controls.Add(this.checkBoxOrderRowsInverted);
            this.groupBoxCustomHistoryLoad.Controls.Add(this.extComboBoxGameTime);
            this.groupBoxCustomHistoryLoad.Controls.Add(this.labelTimeDisplay);
            this.groupBoxCustomHistoryLoad.GradientDirection = 0F;
            this.groupBoxCustomHistoryLoad.Location = new System.Drawing.Point(290, 162);
            this.groupBoxCustomHistoryLoad.Name = "groupBoxCustomHistoryLoad";
            this.groupBoxCustomHistoryLoad.Size = new System.Drawing.Size(281, 85);
            this.groupBoxCustomHistoryLoad.TabIndex = 26;
            this.groupBoxCustomHistoryLoad.TabStop = false;
            this.groupBoxCustomHistoryLoad.Text = "History";
            this.groupBoxCustomHistoryLoad.TextPadding = 0;
            this.groupBoxCustomHistoryLoad.TextStartPosition = -1;
            this.groupBoxCustomHistoryLoad.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxCustomHistoryLoad.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpHistory
            // 
            this.extButtonDrawnHelpHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpHistory.AutoEllipsis = false;
            this.extButtonDrawnHelpHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpHistory.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpHistory.BorderWidth = 1;
            this.extButtonDrawnHelpHistory.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpHistory.Image = null;
            this.extButtonDrawnHelpHistory.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpHistory.Location = new System.Drawing.Point(253, 12);
            this.extButtonDrawnHelpHistory.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpHistory.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpHistory.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpHistory.Name = "extButtonDrawnHelpHistory";
            this.extButtonDrawnHelpHistory.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpHistory.Selectable = true;
            this.extButtonDrawnHelpHistory.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpHistory.TabIndex = 26;
            this.extButtonDrawnHelpHistory.Text = "?";
            this.extButtonDrawnHelpHistory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpHistory.UseMnemonic = true;
            this.extButtonDrawnHelpHistory.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // extComboBoxGameTime
            // 
            this.extComboBoxGameTime.BackColor2 = System.Drawing.Color.Red;
            this.extComboBoxGameTime.BorderColor = System.Drawing.Color.White;
            this.extComboBoxGameTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.extComboBoxGameTime.DataSource = null;
            this.extComboBoxGameTime.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxGameTime.DisabledScaling = 0.5F;
            this.extComboBoxGameTime.DisplayMember = "";
            this.extComboBoxGameTime.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxGameTime.GradientDirection = 90F;
            this.extComboBoxGameTime.Location = new System.Drawing.Point(113, 48);
            this.extComboBoxGameTime.MouseOverScalingColor = 1.3F;
            this.extComboBoxGameTime.Name = "extComboBoxGameTime";
            this.extComboBoxGameTime.SelectedIndex = -1;
            this.extComboBoxGameTime.SelectedItem = null;
            this.extComboBoxGameTime.SelectedValue = null;
            this.extComboBoxGameTime.Size = new System.Drawing.Size(139, 21);
            this.extComboBoxGameTime.TabIndex = 7;
            this.extComboBoxGameTime.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxGameTime.ValueMember = "";
            // 
            // labelTimeDisplay
            // 
            this.labelTimeDisplay.AutoSize = true;
            this.labelTimeDisplay.Location = new System.Drawing.Point(10, 52);
            this.labelTimeDisplay.Name = "labelTimeDisplay";
            this.labelTimeDisplay.Size = new System.Drawing.Size(30, 13);
            this.labelTimeDisplay.TabIndex = 5;
            this.labelTimeDisplay.Text = "Time";
            // 
            // extGroupBoxWebServer
            // 
            this.extGroupBoxWebServer.BorderColor = System.Drawing.Color.LightGray;
            this.extGroupBoxWebServer.BorderColor2 = System.Drawing.Color.Gray;
            this.extGroupBoxWebServer.ChildrenThemed = true;
            this.extGroupBoxWebServer.Controls.Add(this.extButtonDrawnHelpWebServer);
            this.extGroupBoxWebServer.Controls.Add(this.numberBoxLongPortNo);
            this.extGroupBoxWebServer.Controls.Add(this.labelPortNo);
            this.extGroupBoxWebServer.Controls.Add(this.extButtonTestWeb);
            this.extGroupBoxWebServer.Controls.Add(this.extCheckBoxWebServerEnable);
            this.extGroupBoxWebServer.GradientDirection = 0F;
            this.extGroupBoxWebServer.Location = new System.Drawing.Point(577, 162);
            this.extGroupBoxWebServer.Name = "extGroupBoxWebServer";
            this.extGroupBoxWebServer.Size = new System.Drawing.Size(281, 85);
            this.extGroupBoxWebServer.TabIndex = 23;
            this.extGroupBoxWebServer.TabStop = false;
            this.extGroupBoxWebServer.Text = "Web Server";
            this.extGroupBoxWebServer.TextPadding = 0;
            this.extGroupBoxWebServer.TextStartPosition = -1;
            this.extGroupBoxWebServer.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.extGroupBoxWebServer.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpWebServer
            // 
            this.extButtonDrawnHelpWebServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpWebServer.AutoEllipsis = false;
            this.extButtonDrawnHelpWebServer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpWebServer.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpWebServer.BorderWidth = 1;
            this.extButtonDrawnHelpWebServer.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpWebServer.Image = null;
            this.extButtonDrawnHelpWebServer.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpWebServer.Location = new System.Drawing.Point(253, 12);
            this.extButtonDrawnHelpWebServer.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpWebServer.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpWebServer.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpWebServer.Name = "extButtonDrawnHelpWebServer";
            this.extButtonDrawnHelpWebServer.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpWebServer.Selectable = true;
            this.extButtonDrawnHelpWebServer.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpWebServer.TabIndex = 26;
            this.extButtonDrawnHelpWebServer.Text = "?";
            this.extButtonDrawnHelpWebServer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpWebServer.UseMnemonic = true;
            this.extButtonDrawnHelpWebServer.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // numberBoxLongPortNo
            // 
            this.numberBoxLongPortNo.BackErrorColor = System.Drawing.Color.Red;
            this.numberBoxLongPortNo.BorderColor = System.Drawing.Color.Transparent;
            this.numberBoxLongPortNo.BorderColor2 = System.Drawing.Color.Transparent;
            this.numberBoxLongPortNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberBoxLongPortNo.ClearOnFirstChar = false;
            this.numberBoxLongPortNo.ControlBackground = System.Drawing.SystemColors.Control;
            this.numberBoxLongPortNo.DelayBeforeNotification = 0;
            this.numberBoxLongPortNo.EndButtonEnable = true;
            this.numberBoxLongPortNo.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("numberBoxLongPortNo.EndButtonImage")));
            this.numberBoxLongPortNo.EndButtonSize16ths = 10;
            this.numberBoxLongPortNo.EndButtonVisible = false;
            this.numberBoxLongPortNo.Format = "D";
            this.numberBoxLongPortNo.InErrorCondition = true;
            this.numberBoxLongPortNo.Location = new System.Drawing.Point(57, 49);
            this.numberBoxLongPortNo.Maximum = ((long)(65535));
            this.numberBoxLongPortNo.Minimum = ((long)(1024));
            this.numberBoxLongPortNo.Multiline = false;
            this.numberBoxLongPortNo.Name = "numberBoxLongPortNo";
            this.numberBoxLongPortNo.NumberStyles = ((System.Globalization.NumberStyles)((System.Globalization.NumberStyles.AllowLeadingSign | System.Globalization.NumberStyles.AllowThousands)));
            this.numberBoxLongPortNo.ReadOnly = false;
            this.numberBoxLongPortNo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.numberBoxLongPortNo.SelectionLength = 0;
            this.numberBoxLongPortNo.SelectionStart = 0;
            this.numberBoxLongPortNo.Size = new System.Drawing.Size(50, 23);
            this.numberBoxLongPortNo.TabIndex = 6;
            this.numberBoxLongPortNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.numberBoxLongPortNo.TextNoChange = "6502";
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
            this.extButtonTestWeb.BackColor2 = System.Drawing.Color.Red;
            this.extButtonTestWeb.ButtonDisabledScaling = 0.5F;
            this.extButtonTestWeb.GradientDirection = 90F;
            this.extButtonTestWeb.Location = new System.Drawing.Point(152, 48);
            this.extButtonTestWeb.MouseOverScaling = 1.3F;
            this.extButtonTestWeb.MouseSelectedScaling = 1.3F;
            this.extButtonTestWeb.Name = "extButtonTestWeb";
            this.extButtonTestWeb.Size = new System.Drawing.Size(100, 23);
            this.extButtonTestWeb.TabIndex = 3;
            this.extButtonTestWeb.Text = "Test";
            this.extButtonTestWeb.UseVisualStyleBackColor = true;
            this.extButtonTestWeb.Click += new System.EventHandler(this.extButtonTestWebClick);
            // 
            // extCheckBoxWebServerEnable
            // 
            this.extCheckBoxWebServerEnable.AutoSize = true;
            this.extCheckBoxWebServerEnable.ButtonGradientDirection = 90F;
            this.extCheckBoxWebServerEnable.CheckBoxColor = System.Drawing.Color.Gray;
            this.extCheckBoxWebServerEnable.CheckBoxGradientDirection = 225F;
            this.extCheckBoxWebServerEnable.CheckBoxInnerColor = System.Drawing.Color.White;
            this.extCheckBoxWebServerEnable.CheckColor = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWebServerEnable.CheckColor2 = System.Drawing.Color.DarkBlue;
            this.extCheckBoxWebServerEnable.DisabledScaling = 0.5F;
            this.extCheckBoxWebServerEnable.ImageIndeterminate = null;
            this.extCheckBoxWebServerEnable.ImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.extCheckBoxWebServerEnable.ImageUnchecked = null;
            this.extCheckBoxWebServerEnable.Location = new System.Drawing.Point(10, 21);
            this.extCheckBoxWebServerEnable.MouseOverScaling = 1.3F;
            this.extCheckBoxWebServerEnable.MouseSelectedScaling = 1.3F;
            this.extCheckBoxWebServerEnable.Name = "extCheckBoxWebServerEnable";
            this.extCheckBoxWebServerEnable.Size = new System.Drawing.Size(59, 17);
            this.extCheckBoxWebServerEnable.TabIndex = 5;
            this.extCheckBoxWebServerEnable.Text = "Enable";
            this.extCheckBoxWebServerEnable.TickBoxReductionRatio = 0.75F;
            this.extCheckBoxWebServerEnable.UseVisualStyleBackColor = true;
            // 
            // groupBoxInteraction
            // 
            this.groupBoxInteraction.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxInteraction.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxInteraction.ChildrenThemed = true;
            this.groupBoxInteraction.Controls.Add(this.extButtonDrawnHelpTransparency);
            this.groupBoxInteraction.Controls.Add(this.comboBoxClickThruKey);
            this.groupBoxInteraction.Controls.Add(this.labelTKey);
            this.groupBoxInteraction.GradientDirection = 0F;
            this.groupBoxInteraction.Location = new System.Drawing.Point(3, 253);
            this.groupBoxInteraction.Name = "groupBoxInteraction";
            this.groupBoxInteraction.Size = new System.Drawing.Size(281, 85);
            this.groupBoxInteraction.TabIndex = 25;
            this.groupBoxInteraction.TabStop = false;
            this.groupBoxInteraction.Text = "Interaction";
            this.groupBoxInteraction.TextPadding = 0;
            this.groupBoxInteraction.TextStartPosition = -1;
            this.groupBoxInteraction.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxInteraction.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpTransparency
            // 
            this.extButtonDrawnHelpTransparency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpTransparency.AutoEllipsis = false;
            this.extButtonDrawnHelpTransparency.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpTransparency.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpTransparency.BorderWidth = 1;
            this.extButtonDrawnHelpTransparency.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpTransparency.Image = null;
            this.extButtonDrawnHelpTransparency.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpTransparency.Location = new System.Drawing.Point(253, 12);
            this.extButtonDrawnHelpTransparency.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpTransparency.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpTransparency.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpTransparency.Name = "extButtonDrawnHelpTransparency";
            this.extButtonDrawnHelpTransparency.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpTransparency.Selectable = true;
            this.extButtonDrawnHelpTransparency.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpTransparency.TabIndex = 26;
            this.extButtonDrawnHelpTransparency.Text = "?";
            this.extButtonDrawnHelpTransparency.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpTransparency.UseMnemonic = true;
            this.extButtonDrawnHelpTransparency.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
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
            this.groupBoxMemory.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxMemory.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxMemory.ChildrenThemed = true;
            this.groupBoxMemory.Controls.Add(this.extButtonDrawnHelpMemory);
            this.groupBoxMemory.Controls.Add(this.comboBoxCustomEssentialEntries);
            this.groupBoxMemory.Controls.Add(this.comboBoxCustomHistoryLoadTime);
            this.groupBoxMemory.Controls.Add(this.labelHistoryEssItems);
            this.groupBoxMemory.Controls.Add(this.labelHistorySel);
            this.groupBoxMemory.GradientDirection = 0F;
            this.groupBoxMemory.Location = new System.Drawing.Point(290, 253);
            this.groupBoxMemory.Name = "groupBoxMemory";
            this.groupBoxMemory.Size = new System.Drawing.Size(281, 85);
            this.groupBoxMemory.TabIndex = 21;
            this.groupBoxMemory.TabStop = false;
            this.groupBoxMemory.Text = "Memory";
            this.groupBoxMemory.TextPadding = 0;
            this.groupBoxMemory.TextStartPosition = -1;
            this.groupBoxMemory.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxMemory.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpMemory
            // 
            this.extButtonDrawnHelpMemory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpMemory.AutoEllipsis = false;
            this.extButtonDrawnHelpMemory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpMemory.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpMemory.BorderWidth = 1;
            this.extButtonDrawnHelpMemory.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpMemory.Image = null;
            this.extButtonDrawnHelpMemory.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpMemory.Location = new System.Drawing.Point(253, 12);
            this.extButtonDrawnHelpMemory.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpMemory.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpMemory.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpMemory.Name = "extButtonDrawnHelpMemory";
            this.extButtonDrawnHelpMemory.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpMemory.Selectable = true;
            this.extButtonDrawnHelpMemory.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpMemory.TabIndex = 26;
            this.extButtonDrawnHelpMemory.Text = "?";
            this.extButtonDrawnHelpMemory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpMemory.UseMnemonic = true;
            this.extButtonDrawnHelpMemory.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // labelHistoryEssItems
            // 
            this.labelHistoryEssItems.AutoSize = true;
            this.labelHistoryEssItems.Location = new System.Drawing.Point(10, 53);
            this.labelHistoryEssItems.Name = "labelHistoryEssItems";
            this.labelHistoryEssItems.Size = new System.Drawing.Size(83, 13);
            this.labelHistoryEssItems.TabIndex = 5;
            this.labelHistoryEssItems.Text = "Essential entries";
            // 
            // labelHistorySel
            // 
            this.labelHistorySel.AutoSize = true;
            this.labelHistorySel.Location = new System.Drawing.Point(10, 23);
            this.labelHistorySel.Name = "labelHistorySel";
            this.labelHistorySel.Size = new System.Drawing.Size(75, 13);
            this.labelHistorySel.TabIndex = 5;
            this.labelHistorySel.Text = "Entries to read";
            // 
            // groupBoxCustomScreenShots
            // 
            this.groupBoxCustomScreenShots.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomScreenShots.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxCustomScreenShots.ChildrenThemed = true;
            this.groupBoxCustomScreenShots.Controls.Add(this.extButtonDrawnHelpScreenshots);
            this.groupBoxCustomScreenShots.Controls.Add(this.buttonExtScreenshot);
            this.groupBoxCustomScreenShots.Controls.Add(this.checkBoxCustomEnableScreenshots);
            this.groupBoxCustomScreenShots.GradientDirection = 0F;
            this.groupBoxCustomScreenShots.Location = new System.Drawing.Point(577, 253);
            this.groupBoxCustomScreenShots.Name = "groupBoxCustomScreenShots";
            this.groupBoxCustomScreenShots.Size = new System.Drawing.Size(281, 85);
            this.groupBoxCustomScreenShots.TabIndex = 20;
            this.groupBoxCustomScreenShots.TabStop = false;
            this.groupBoxCustomScreenShots.Text = "Screenshots conversion";
            this.groupBoxCustomScreenShots.TextPadding = 0;
            this.groupBoxCustomScreenShots.TextStartPosition = -1;
            this.groupBoxCustomScreenShots.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxCustomScreenShots.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpScreenshots
            // 
            this.extButtonDrawnHelpScreenshots.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpScreenshots.AutoEllipsis = false;
            this.extButtonDrawnHelpScreenshots.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpScreenshots.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpScreenshots.BorderWidth = 1;
            this.extButtonDrawnHelpScreenshots.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpScreenshots.Image = null;
            this.extButtonDrawnHelpScreenshots.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpScreenshots.Location = new System.Drawing.Point(253, 12);
            this.extButtonDrawnHelpScreenshots.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpScreenshots.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpScreenshots.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpScreenshots.Name = "extButtonDrawnHelpScreenshots";
            this.extButtonDrawnHelpScreenshots.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpScreenshots.Selectable = true;
            this.extButtonDrawnHelpScreenshots.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpScreenshots.TabIndex = 26;
            this.extButtonDrawnHelpScreenshots.Text = "?";
            this.extButtonDrawnHelpScreenshots.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpScreenshots.UseMnemonic = true;
            this.extButtonDrawnHelpScreenshots.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // groupBoxCustomEDSM
            // 
            this.groupBoxCustomEDSM.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomEDSM.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxCustomEDSM.ChildrenThemed = true;
            this.groupBoxCustomEDSM.Controls.Add(this.extButtonDrawnHelpEDSM);
            this.groupBoxCustomEDSM.Controls.Add(this.extButtonReloadStarDatabase);
            this.groupBoxCustomEDSM.Controls.Add(this.buttonExtEDSMConfigureArea);
            this.groupBoxCustomEDSM.Controls.Add(this.checkBoxCustomEDSMDownload);
            this.groupBoxCustomEDSM.GradientDirection = 0F;
            this.groupBoxCustomEDSM.Location = new System.Drawing.Point(3, 344);
            this.groupBoxCustomEDSM.Name = "groupBoxCustomEDSM";
            this.groupBoxCustomEDSM.Size = new System.Drawing.Size(281, 105);
            this.groupBoxCustomEDSM.TabIndex = 21;
            this.groupBoxCustomEDSM.TabStop = false;
            this.groupBoxCustomEDSM.Text = "System DB Control";
            this.groupBoxCustomEDSM.TextPadding = 0;
            this.groupBoxCustomEDSM.TextStartPosition = -1;
            this.groupBoxCustomEDSM.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxCustomEDSM.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpEDSM
            // 
            this.extButtonDrawnHelpEDSM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpEDSM.AutoEllipsis = false;
            this.extButtonDrawnHelpEDSM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpEDSM.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpEDSM.BorderWidth = 1;
            this.extButtonDrawnHelpEDSM.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpEDSM.Image = null;
            this.extButtonDrawnHelpEDSM.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpEDSM.Location = new System.Drawing.Point(253, 12);
            this.extButtonDrawnHelpEDSM.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpEDSM.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpEDSM.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpEDSM.Name = "extButtonDrawnHelpEDSM";
            this.extButtonDrawnHelpEDSM.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpEDSM.Selectable = true;
            this.extButtonDrawnHelpEDSM.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpEDSM.TabIndex = 26;
            this.extButtonDrawnHelpEDSM.Text = "?";
            this.extButtonDrawnHelpEDSM.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpEDSM.UseMnemonic = true;
            this.extButtonDrawnHelpEDSM.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // extButtonReloadStarDatabase
            // 
            this.extButtonReloadStarDatabase.BackColor2 = System.Drawing.Color.Red;
            this.extButtonReloadStarDatabase.ButtonDisabledScaling = 0.5F;
            this.extButtonReloadStarDatabase.GradientDirection = 90F;
            this.extButtonReloadStarDatabase.Location = new System.Drawing.Point(10, 72);
            this.extButtonReloadStarDatabase.MouseOverScaling = 1.3F;
            this.extButtonReloadStarDatabase.MouseSelectedScaling = 1.3F;
            this.extButtonReloadStarDatabase.Name = "extButtonReloadStarDatabase";
            this.extButtonReloadStarDatabase.Size = new System.Drawing.Size(243, 23);
            this.extButtonReloadStarDatabase.TabIndex = 10;
            this.extButtonReloadStarDatabase.Text = "Reload Star Database";
            this.extButtonReloadStarDatabase.UseVisualStyleBackColor = true;
            this.extButtonReloadStarDatabase.Click += new System.EventHandler(this.extButtonReloadStarDatabase_Click);
            // 
            // groupBoxPopOuts
            // 
            this.groupBoxPopOuts.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxPopOuts.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxPopOuts.ChildrenThemed = true;
            this.groupBoxPopOuts.Controls.Add(this.extButtonDrawnHelpWindowOptions);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxPanelSortOrder);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxKeepOnTop);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxCustomResize);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxMinimizeToNotifyIcon);
            this.groupBoxPopOuts.Controls.Add(this.checkBoxUseNotifyIcon);
            this.groupBoxPopOuts.GradientDirection = 0F;
            this.groupBoxPopOuts.Location = new System.Drawing.Point(290, 344);
            this.groupBoxPopOuts.Name = "groupBoxPopOuts";
            this.groupBoxPopOuts.Size = new System.Drawing.Size(281, 129);
            this.groupBoxPopOuts.TabIndex = 19;
            this.groupBoxPopOuts.TabStop = false;
            this.groupBoxPopOuts.Text = "Window Options";
            this.groupBoxPopOuts.TextPadding = 0;
            this.groupBoxPopOuts.TextStartPosition = -1;
            this.groupBoxPopOuts.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxPopOuts.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpWindowOptions
            // 
            this.extButtonDrawnHelpWindowOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpWindowOptions.AutoEllipsis = false;
            this.extButtonDrawnHelpWindowOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpWindowOptions.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpWindowOptions.BorderWidth = 1;
            this.extButtonDrawnHelpWindowOptions.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpWindowOptions.Image = null;
            this.extButtonDrawnHelpWindowOptions.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpWindowOptions.Location = new System.Drawing.Point(253, 12);
            this.extButtonDrawnHelpWindowOptions.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpWindowOptions.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpWindowOptions.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpWindowOptions.Name = "extButtonDrawnHelpWindowOptions";
            this.extButtonDrawnHelpWindowOptions.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpWindowOptions.Selectable = true;
            this.extButtonDrawnHelpWindowOptions.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpWindowOptions.TabIndex = 26;
            this.extButtonDrawnHelpWindowOptions.Text = "?";
            this.extButtonDrawnHelpWindowOptions.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpWindowOptions.UseMnemonic = true;
            this.extButtonDrawnHelpWindowOptions.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // extGroupBoxDLLPerms
            // 
            this.extGroupBoxDLLPerms.BorderColor = System.Drawing.Color.LightGray;
            this.extGroupBoxDLLPerms.BorderColor2 = System.Drawing.Color.Gray;
            this.extGroupBoxDLLPerms.ChildrenThemed = true;
            this.extGroupBoxDLLPerms.Controls.Add(this.extButtonDrawnHelpDLL);
            this.extGroupBoxDLLPerms.Controls.Add(this.extButtonDLLConfigure);
            this.extGroupBoxDLLPerms.Controls.Add(this.extButtonDLLPerms);
            this.extGroupBoxDLLPerms.GradientDirection = 0F;
            this.extGroupBoxDLLPerms.Location = new System.Drawing.Point(3, 479);
            this.extGroupBoxDLLPerms.Name = "extGroupBoxDLLPerms";
            this.extGroupBoxDLLPerms.Size = new System.Drawing.Size(281, 52);
            this.extGroupBoxDLLPerms.TabIndex = 21;
            this.extGroupBoxDLLPerms.TabStop = false;
            this.extGroupBoxDLLPerms.Text = "DLLs";
            this.extGroupBoxDLLPerms.TextPadding = 0;
            this.extGroupBoxDLLPerms.TextStartPosition = -1;
            this.extGroupBoxDLLPerms.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.extGroupBoxDLLPerms.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpDLL
            // 
            this.extButtonDrawnHelpDLL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpDLL.AutoEllipsis = false;
            this.extButtonDrawnHelpDLL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpDLL.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpDLL.BorderWidth = 1;
            this.extButtonDrawnHelpDLL.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpDLL.Image = null;
            this.extButtonDrawnHelpDLL.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpDLL.Location = new System.Drawing.Point(253, 12);
            this.extButtonDrawnHelpDLL.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpDLL.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpDLL.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpDLL.Name = "extButtonDrawnHelpDLL";
            this.extButtonDrawnHelpDLL.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpDLL.Selectable = true;
            this.extButtonDrawnHelpDLL.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpDLL.TabIndex = 26;
            this.extButtonDrawnHelpDLL.Text = "?";
            this.extButtonDrawnHelpDLL.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpDLL.UseMnemonic = true;
            this.extButtonDrawnHelpDLL.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // extButtonDLLConfigure
            // 
            this.extButtonDLLConfigure.BackColor2 = System.Drawing.Color.Red;
            this.extButtonDLLConfigure.ButtonDisabledScaling = 0.5F;
            this.extButtonDLLConfigure.GradientDirection = 90F;
            this.extButtonDLLConfigure.Location = new System.Drawing.Point(152, 17);
            this.extButtonDLLConfigure.MouseOverScaling = 1.3F;
            this.extButtonDLLConfigure.MouseSelectedScaling = 1.3F;
            this.extButtonDLLConfigure.Name = "extButtonDLLConfigure";
            this.extButtonDLLConfigure.Size = new System.Drawing.Size(100, 23);
            this.extButtonDLLConfigure.TabIndex = 10;
            this.extButtonDLLConfigure.Text = "Configure";
            this.extButtonDLLConfigure.UseVisualStyleBackColor = true;
            this.extButtonDLLConfigure.Click += new System.EventHandler(this.extButtonDLLConfigure_Click);
            // 
            // extButtonDLLPerms
            // 
            this.extButtonDLLPerms.BackColor2 = System.Drawing.Color.Red;
            this.extButtonDLLPerms.ButtonDisabledScaling = 0.5F;
            this.extButtonDLLPerms.GradientDirection = 90F;
            this.extButtonDLLPerms.Location = new System.Drawing.Point(9, 17);
            this.extButtonDLLPerms.MouseOverScaling = 1.3F;
            this.extButtonDLLPerms.MouseSelectedScaling = 1.3F;
            this.extButtonDLLPerms.Name = "extButtonDLLPerms";
            this.extButtonDLLPerms.Size = new System.Drawing.Size(100, 23);
            this.extButtonDLLPerms.TabIndex = 10;
            this.extButtonDLLPerms.Text = "Permissions";
            this.extButtonDLLPerms.UseVisualStyleBackColor = true;
            this.extButtonDLLPerms.Click += new System.EventHandler(this.extButtonDLLPerms_Click);
            // 
            // extGroupBoxWebLookup
            // 
            this.extGroupBoxWebLookup.BorderColor = System.Drawing.Color.LightGray;
            this.extGroupBoxWebLookup.BorderColor2 = System.Drawing.Color.Gray;
            this.extGroupBoxWebLookup.ChildrenThemed = true;
            this.extGroupBoxWebLookup.Controls.Add(this.extComboBoxWebLookup);
            this.extGroupBoxWebLookup.GradientDirection = 0F;
            this.extGroupBoxWebLookup.Location = new System.Drawing.Point(577, 344);
            this.extGroupBoxWebLookup.Name = "extGroupBoxWebLookup";
            this.extGroupBoxWebLookup.Size = new System.Drawing.Size(281, 52);
            this.extGroupBoxWebLookup.TabIndex = 21;
            this.extGroupBoxWebLookup.TabStop = false;
            this.extGroupBoxWebLookup.Text = "System Lookup from Web";
            this.extGroupBoxWebLookup.TextPadding = 0;
            this.extGroupBoxWebLookup.TextStartPosition = -1;
            this.extGroupBoxWebLookup.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.extGroupBoxWebLookup.ThemeColorSet = -1;
            // 
            // extComboBoxWebLookup
            // 
            this.extComboBoxWebLookup.BackColor = System.Drawing.Color.Gray;
            this.extComboBoxWebLookup.BackColor2 = System.Drawing.Color.Red;
            this.extComboBoxWebLookup.BorderColor = System.Drawing.Color.Red;
            this.extComboBoxWebLookup.ControlBackground = System.Drawing.SystemColors.Control;
            this.extComboBoxWebLookup.DataSource = null;
            this.extComboBoxWebLookup.DisableBackgroundDisabledShadingGradient = false;
            this.extComboBoxWebLookup.DisabledScaling = 0.5F;
            this.extComboBoxWebLookup.DisplayMember = "";
            this.extComboBoxWebLookup.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.extComboBoxWebLookup.GradientDirection = 90F;
            this.extComboBoxWebLookup.Location = new System.Drawing.Point(9, 19);
            this.extComboBoxWebLookup.MouseOverScalingColor = 1.3F;
            this.extComboBoxWebLookup.Name = "extComboBoxWebLookup";
            this.extComboBoxWebLookup.SelectedIndex = -1;
            this.extComboBoxWebLookup.SelectedItem = null;
            this.extComboBoxWebLookup.SelectedValue = null;
            this.extComboBoxWebLookup.Size = new System.Drawing.Size(266, 21);
            this.extComboBoxWebLookup.TabIndex = 0;
            this.extComboBoxWebLookup.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.extComboBoxWebLookup.ValueMember = "";
            // 
            // groupBoxCustomLanguage
            // 
            this.groupBoxCustomLanguage.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomLanguage.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxCustomLanguage.ChildrenThemed = true;
            this.groupBoxCustomLanguage.Controls.Add(this.comboBoxCustomLanguage);
            this.groupBoxCustomLanguage.GradientDirection = 0F;
            this.groupBoxCustomLanguage.Location = new System.Drawing.Point(290, 479);
            this.groupBoxCustomLanguage.Name = "groupBoxCustomLanguage";
            this.groupBoxCustomLanguage.Size = new System.Drawing.Size(281, 52);
            this.groupBoxCustomLanguage.TabIndex = 21;
            this.groupBoxCustomLanguage.TabStop = false;
            this.groupBoxCustomLanguage.Text = "Language";
            this.groupBoxCustomLanguage.TextPadding = 0;
            this.groupBoxCustomLanguage.TextStartPosition = -1;
            this.groupBoxCustomLanguage.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxCustomLanguage.ThemeColorSet = -1;
            // 
            // comboBoxCustomLanguage
            // 
            this.comboBoxCustomLanguage.BackColor = System.Drawing.Color.Gray;
            this.comboBoxCustomLanguage.BackColor2 = System.Drawing.Color.Red;
            this.comboBoxCustomLanguage.BorderColor = System.Drawing.Color.Red;
            this.comboBoxCustomLanguage.ControlBackground = System.Drawing.SystemColors.Control;
            this.comboBoxCustomLanguage.DataSource = null;
            this.comboBoxCustomLanguage.DisableBackgroundDisabledShadingGradient = false;
            this.comboBoxCustomLanguage.DisabledScaling = 0.5F;
            this.comboBoxCustomLanguage.DisplayMember = "";
            this.comboBoxCustomLanguage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomLanguage.GradientDirection = 90F;
            this.comboBoxCustomLanguage.Location = new System.Drawing.Point(9, 19);
            this.comboBoxCustomLanguage.MouseOverScalingColor = 1.3F;
            this.comboBoxCustomLanguage.Name = "comboBoxCustomLanguage";
            this.comboBoxCustomLanguage.SelectedIndex = -1;
            this.comboBoxCustomLanguage.SelectedItem = null;
            this.comboBoxCustomLanguage.SelectedValue = null;
            this.comboBoxCustomLanguage.Size = new System.Drawing.Size(266, 21);
            this.comboBoxCustomLanguage.TabIndex = 0;
            this.comboBoxCustomLanguage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.comboBoxCustomLanguage.ValueMember = "";
            // 
            // groupBoxCustomSafeMode
            // 
            this.groupBoxCustomSafeMode.BorderColor = System.Drawing.Color.LightGray;
            this.groupBoxCustomSafeMode.BorderColor2 = System.Drawing.Color.Gray;
            this.groupBoxCustomSafeMode.ChildrenThemed = true;
            this.groupBoxCustomSafeMode.Controls.Add(this.extButtonDrawnHelpSafeMode);
            this.groupBoxCustomSafeMode.Controls.Add(this.buttonExtSafeMode);
            this.groupBoxCustomSafeMode.Controls.Add(this.labelSafeMode);
            this.groupBoxCustomSafeMode.GradientDirection = 0F;
            this.groupBoxCustomSafeMode.Location = new System.Drawing.Point(577, 479);
            this.groupBoxCustomSafeMode.Name = "groupBoxCustomSafeMode";
            this.groupBoxCustomSafeMode.Size = new System.Drawing.Size(281, 127);
            this.groupBoxCustomSafeMode.TabIndex = 21;
            this.groupBoxCustomSafeMode.TabStop = false;
            this.groupBoxCustomSafeMode.Text = "Advanced";
            this.groupBoxCustomSafeMode.TextPadding = 0;
            this.groupBoxCustomSafeMode.TextStartPosition = -1;
            this.groupBoxCustomSafeMode.ThemeColors = new System.Drawing.Color[] {
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control,
        System.Drawing.SystemColors.Control};
            this.groupBoxCustomSafeMode.ThemeColorSet = -1;
            // 
            // extButtonDrawnHelpSafeMode
            // 
            this.extButtonDrawnHelpSafeMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.extButtonDrawnHelpSafeMode.AutoEllipsis = false;
            this.extButtonDrawnHelpSafeMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.extButtonDrawnHelpSafeMode.BorderColor = System.Drawing.Color.Orange;
            this.extButtonDrawnHelpSafeMode.BorderWidth = 1;
            this.extButtonDrawnHelpSafeMode.ButtonDisabledScaling = 0.25F;
            this.extButtonDrawnHelpSafeMode.Image = null;
            this.extButtonDrawnHelpSafeMode.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Text;
            this.extButtonDrawnHelpSafeMode.Location = new System.Drawing.Point(251, 16);
            this.extButtonDrawnHelpSafeMode.MouseOverColor = System.Drawing.Color.White;
            this.extButtonDrawnHelpSafeMode.MouseSelectedColor = System.Drawing.Color.Green;
            this.extButtonDrawnHelpSafeMode.MouseSelectedColorEnable = true;
            this.extButtonDrawnHelpSafeMode.Name = "extButtonDrawnHelpSafeMode";
            this.extButtonDrawnHelpSafeMode.Padding = new System.Windows.Forms.Padding(6);
            this.extButtonDrawnHelpSafeMode.Selectable = true;
            this.extButtonDrawnHelpSafeMode.Size = new System.Drawing.Size(24, 24);
            this.extButtonDrawnHelpSafeMode.TabIndex = 26;
            this.extButtonDrawnHelpSafeMode.Text = "?";
            this.extButtonDrawnHelpSafeMode.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.extButtonDrawnHelpSafeMode.UseMnemonic = true;
            this.extButtonDrawnHelpSafeMode.Click += new System.EventHandler(this.extButtonDrawnHelp_Click);
            // 
            // labelSafeMode
            // 
            this.labelSafeMode.Location = new System.Drawing.Point(10, 19);
            this.labelSafeMode.Name = "labelSafeMode";
            this.labelSafeMode.Size = new System.Drawing.Size(235, 65);
            this.labelSafeMode.TabIndex = 5;
            this.labelSafeMode.Text = "Click this to perform special operations, such as to move system databases to ano" +
    "ther drive, reset UI, and other maintenance tasks...\r\n\r\n";
            // 
            // UserControlSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.extPanelScroll);
            this.Name = "UserControlSettings";
            this.Size = new System.Drawing.Size(1086, 802);
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
            this.extGroupBoxDLLPerms.ResumeLayout(false);
            this.extGroupBoxWebLookup.ResumeLayout(false);
            this.groupBoxCustomLanguage.ResumeLayout(false);
            this.groupBoxCustomSafeMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtGroupBox groupBoxCommanders;
        private ExtendedControls.ExtButton buttonAddCommander;
        private BaseUtils.DataGridViewBaseEnhancements dataGridViewCommanders;
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
        private ExtendedControls.ExtButton buttonExtScreenshot;
        private ExtendedControls.ExtCheckBox checkBoxCustomEnableScreenshots;
        private ExtendedControls.ExtGroupBox groupBoxCustomEDSM;
        private ExtendedControls.ExtButton buttonExtEDSMConfigureArea;
        private ExtendedControls.ExtCheckBox checkBoxCustomEDSMDownload;
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
        private ExtendedControls.ExtGroupBox extGroupBoxDLLPerms;
        private ExtendedControls.ExtButton extButtonDLLPerms;
        private ExtendedControls.ExtButton extButtonDLLConfigure;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpTheme;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpTransparency;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpSafeMode;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpWebServer;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpScreenshots;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpMemory;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpEDSM;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpHistory;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpWindowOptions;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpDLL;
        private ExtendedControls.ExtButtonDrawn extButtonDrawnHelpCommanders;
        private ExtendedControls.ExtSplitterResizeParent extSplitterResizeParentGroupBoxCommanders;
        private ExtendedControls.ExtButton extButtonReloadStarDatabase;
        private ExtendedControls.ExtButton extButtonUnDelete;
        private ExtendedControls.ExtGroupBox extGroupBoxWebLookup;
        private ExtendedControls.ExtComboBox extComboBoxWebLookup;
    }
}
