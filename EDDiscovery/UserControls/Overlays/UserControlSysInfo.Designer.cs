﻿/*
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
    partial class UserControlSysInfo
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
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSystem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripEDSM = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripEDSMDownLine = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripVisits = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripBody = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripPosition = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDistanceFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSystemState = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripNotes = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTarget = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripShip = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripFuel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripCargo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDataCount = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMaterialCounts = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripCredits = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripGameMode = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripTravel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMissionList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSkinny = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripReset = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripRemoveAll = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxBody = new ExtendedControls.ExtTextBox();
            this.labelBodyName = new System.Windows.Forms.Label();
            this.textBoxPosition = new ExtendedControls.ExtTextBox();
            this.labelPosition = new System.Windows.Forms.Label();
            this.textBoxVisits = new ExtendedControls.ExtTextBox();
            this.labelVisits = new System.Windows.Forms.Label();
            this.labelAllegiance = new System.Windows.Forms.Label();
            this.labelEconomy = new System.Windows.Forms.Label();
            this.textBoxAllegiance = new ExtendedControls.ExtTextBox();
            this.textBoxGovernment = new ExtendedControls.ExtTextBox();
            this.labelGov = new System.Windows.Forms.Label();
            this.labelState = new System.Windows.Forms.Label();
            this.textBoxEconomy = new ExtendedControls.ExtTextBox();
            this.textBoxState = new ExtendedControls.ExtTextBox();
            this.buttonEDDB = new ExtendedControls.ExtPanelDrawn();
            this.buttonRoss = new ExtendedControls.ExtPanelDrawn();
            this.textBoxHomeDist = new ExtendedControls.ExtTextBox();
            this.labelHomeDist = new System.Windows.Forms.Label();
            this.buttonEDSM = new ExtendedControls.ExtPanelDrawn();
            this.textBoxSolDist = new ExtendedControls.ExtTextBox();
            this.labelSolDist = new System.Windows.Forms.Label();
            this.labelNote = new System.Windows.Forms.Label();
            this.richTextBoxNote = new ExtendedControls.ExtRichTextBox();
            this.labelTarget = new System.Windows.Forms.Label();
            this.buttonEDSMTarget = new ExtendedControls.ExtPanelDrawn();
            this.textBoxTarget = new ExtendedControls.ExtTextBoxAutoComplete();
            this.textBoxTargetDist = new ExtendedControls.ExtTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.textBoxSystem = new ExtendedControls.ExtTextBox();
            this.labelSysName = new System.Windows.Forms.Label();
            this.labelOpen = new System.Windows.Forms.Label();
            this.labelGamemode = new System.Windows.Forms.Label();
            this.labelTravel = new System.Windows.Forms.Label();
            this.textBoxGameMode = new ExtendedControls.ExtTextBox();
            this.textBoxTravelDist = new ExtendedControls.ExtTextBox();
            this.textBoxTravelTime = new ExtendedControls.ExtTextBox();
            this.textBoxTravelJumps = new ExtendedControls.ExtTextBox();
            this.labelCargo = new System.Windows.Forms.Label();
            this.textBoxCargo = new ExtendedControls.ExtTextBox();
            this.textBoxMaterials = new ExtendedControls.ExtTextBox();
            this.labelMaterials = new System.Windows.Forms.Label();
            this.labelData = new System.Windows.Forms.Label();
            this.textBoxData = new ExtendedControls.ExtTextBox();
            this.labelShip = new System.Windows.Forms.Label();
            this.textBoxShip = new ExtendedControls.ExtTextBox();
            this.labelFuel = new System.Windows.Forms.Label();
            this.textBoxFuel = new ExtendedControls.ExtTextBox();
            this.textBoxCredits = new ExtendedControls.ExtTextBox();
            this.labelCredits = new System.Windows.Forms.Label();
            this.panelFD = new ExtendedControls.PanelNoTheme();
            this.richTextBoxScrollMissions = new ExtendedControls.ExtRichTextBox();
            this.labelMissions = new System.Windows.Forms.Label();
            this.textBoxJumpRange = new ExtendedControls.ExtTextBox();
            this.labelJumpRange = new System.Windows.Forms.Label();
            this.toolStripJumpRange = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSystem,
            this.toolStripEDSM,
            this.toolStripEDSMDownLine,
            this.toolStripVisits,
            this.toolStripBody,
            this.toolStripPosition,
            this.toolStripDistanceFrom,
            this.toolStripSystemState,
            this.toolStripNotes,
            this.toolStripTarget,
            this.toolStripShip,
            this.toolStripFuel,
            this.toolStripCargo,
            this.toolStripDataCount,
            this.toolStripMaterialCounts,
            this.toolStripCredits,
            this.toolStripGameMode,
            this.toolStripTravel,
            this.toolStripMissionList,
            this.toolStripJumpRange,
            this.toolStripSkinny,
            this.toolStripReset,
            this.toolStripRemoveAll});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(256, 532);
            // 
            // toolStripSystem
            // 
            this.toolStripSystem.Checked = true;
            this.toolStripSystem.CheckOnClick = true;
            this.toolStripSystem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripSystem.Name = "toolStripSystem";
            this.toolStripSystem.Size = new System.Drawing.Size(255, 22);
            this.toolStripSystem.Text = "Display System Name";
            this.toolStripSystem.Click += new System.EventHandler(this.toolStripSystem_Click);
            // 
            // toolStripEDSM
            // 
            this.toolStripEDSM.Checked = true;
            this.toolStripEDSM.CheckOnClick = true;
            this.toolStripEDSM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripEDSM.Name = "toolStripEDSM";
            this.toolStripEDSM.Size = new System.Drawing.Size(255, 22);
            this.toolStripEDSM.Text = "Display EDSM Buttons";
            this.toolStripEDSM.Click += new System.EventHandler(this.toolStripEDSM_Click);
            // 
            // toolStripEDSMDownLine
            // 
            this.toolStripEDSMDownLine.Checked = true;
            this.toolStripEDSMDownLine.CheckOnClick = true;
            this.toolStripEDSMDownLine.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripEDSMDownLine.Name = "toolStripEDSMDownLine";
            this.toolStripEDSMDownLine.Size = new System.Drawing.Size(255, 22);
            this.toolStripEDSMDownLine.Text = "EDSM buttons on separate line";
            this.toolStripEDSMDownLine.Click += new System.EventHandler(this.toolStripEDSMButtons_Click);
            // 
            // toolStripVisits
            // 
            this.toolStripVisits.Checked = true;
            this.toolStripVisits.CheckOnClick = true;
            this.toolStripVisits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripVisits.Name = "toolStripVisits";
            this.toolStripVisits.Size = new System.Drawing.Size(255, 22);
            this.toolStripVisits.Text = "Display Visits";
            this.toolStripVisits.Click += new System.EventHandler(this.toolStripVisits_Click);
            // 
            // toolStripBody
            // 
            this.toolStripBody.Checked = true;
            this.toolStripBody.CheckOnClick = true;
            this.toolStripBody.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripBody.Name = "toolStripBody";
            this.toolStripBody.Size = new System.Drawing.Size(255, 22);
            this.toolStripBody.Text = "Display Body Name";
            this.toolStripBody.Click += new System.EventHandler(this.toolStripBody_Click);
            // 
            // toolStripPosition
            // 
            this.toolStripPosition.Checked = true;
            this.toolStripPosition.CheckOnClick = true;
            this.toolStripPosition.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripPosition.Name = "toolStripPosition";
            this.toolStripPosition.Size = new System.Drawing.Size(255, 22);
            this.toolStripPosition.Text = "Display Position";
            this.toolStripPosition.Click += new System.EventHandler(this.toolStripPosition_Click);
            // 
            // toolStripDistanceFrom
            // 
            this.toolStripDistanceFrom.Checked = true;
            this.toolStripDistanceFrom.CheckOnClick = true;
            this.toolStripDistanceFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripDistanceFrom.Name = "toolStripDistanceFrom";
            this.toolStripDistanceFrom.Size = new System.Drawing.Size(255, 22);
            this.toolStripDistanceFrom.Text = "Display Distance From";
            this.toolStripDistanceFrom.Click += new System.EventHandler(this.enableDistanceFromToolStripMenuItem_Click);
            // 
            // toolStripSystemState
            // 
            this.toolStripSystemState.Checked = true;
            this.toolStripSystemState.CheckOnClick = true;
            this.toolStripSystemState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripSystemState.Name = "toolStripSystemState";
            this.toolStripSystemState.Size = new System.Drawing.Size(255, 22);
            this.toolStripSystemState.Text = "Display System State";
            this.toolStripSystemState.Click += new System.EventHandler(this.toolStripSystemState_Click);
            // 
            // toolStripNotes
            // 
            this.toolStripNotes.Checked = true;
            this.toolStripNotes.CheckOnClick = true;
            this.toolStripNotes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripNotes.Name = "toolStripNotes";
            this.toolStripNotes.Size = new System.Drawing.Size(255, 22);
            this.toolStripNotes.Text = "Display Notes";
            this.toolStripNotes.Click += new System.EventHandler(this.toolStripNotes_Click);
            // 
            // toolStripTarget
            // 
            this.toolStripTarget.Checked = true;
            this.toolStripTarget.CheckOnClick = true;
            this.toolStripTarget.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripTarget.Name = "toolStripTarget";
            this.toolStripTarget.Size = new System.Drawing.Size(255, 22);
            this.toolStripTarget.Text = "Display Target";
            this.toolStripTarget.Click += new System.EventHandler(this.toolStripTarget_Click);
            // 
            // toolStripShip
            // 
            this.toolStripShip.Checked = true;
            this.toolStripShip.CheckOnClick = true;
            this.toolStripShip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripShip.Name = "toolStripShip";
            this.toolStripShip.Size = new System.Drawing.Size(255, 22);
            this.toolStripShip.Text = "Display Ship Information";
            this.toolStripShip.Click += new System.EventHandler(this.toolStripShip_Click);
            // 
            // toolStripFuel
            // 
            this.toolStripFuel.Checked = true;
            this.toolStripFuel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripFuel.Name = "toolStripFuel";
            this.toolStripFuel.Size = new System.Drawing.Size(255, 22);
            this.toolStripFuel.Text = "Display Fuel Level";
            this.toolStripFuel.Click += new System.EventHandler(this.toolStripFuel_Click);
            // 
            // toolStripCargo
            // 
            this.toolStripCargo.Checked = true;
            this.toolStripCargo.CheckOnClick = true;
            this.toolStripCargo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripCargo.Name = "toolStripCargo";
            this.toolStripCargo.Size = new System.Drawing.Size(255, 22);
            this.toolStripCargo.Text = "Display Cargo Count";
            this.toolStripCargo.Click += new System.EventHandler(this.toolStripCargo_Click);
            // 
            // toolStripDataCount
            // 
            this.toolStripDataCount.Checked = true;
            this.toolStripDataCount.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripDataCount.Name = "toolStripDataCount";
            this.toolStripDataCount.Size = new System.Drawing.Size(255, 22);
            this.toolStripDataCount.Text = "Display Data Count";
            this.toolStripDataCount.Click += new System.EventHandler(this.toolStripDataCount_Click);
            // 
            // toolStripMaterialCounts
            // 
            this.toolStripMaterialCounts.Checked = true;
            this.toolStripMaterialCounts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMaterialCounts.Name = "toolStripMaterialCounts";
            this.toolStripMaterialCounts.Size = new System.Drawing.Size(255, 22);
            this.toolStripMaterialCounts.Text = "Display Material Count";
            this.toolStripMaterialCounts.Click += new System.EventHandler(this.toolStripMaterialCount_Click);
            // 
            // toolStripCredits
            // 
            this.toolStripCredits.Checked = true;
            this.toolStripCredits.CheckOnClick = true;
            this.toolStripCredits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripCredits.Name = "toolStripCredits";
            this.toolStripCredits.Size = new System.Drawing.Size(255, 22);
            this.toolStripCredits.Text = "Display Credits";
            this.toolStripCredits.Click += new System.EventHandler(this.toolStripCredits_Click);
            // 
            // toolStripGameMode
            // 
            this.toolStripGameMode.Checked = true;
            this.toolStripGameMode.CheckOnClick = true;
            this.toolStripGameMode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripGameMode.Name = "toolStripGameMode";
            this.toolStripGameMode.Size = new System.Drawing.Size(255, 22);
            this.toolStripGameMode.Text = "Display Game Mode";
            this.toolStripGameMode.Click += new System.EventHandler(this.toolStripGameMode_Click);
            // 
            // toolStripTravel
            // 
            this.toolStripTravel.Checked = true;
            this.toolStripTravel.CheckOnClick = true;
            this.toolStripTravel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripTravel.Name = "toolStripTravel";
            this.toolStripTravel.Size = new System.Drawing.Size(255, 22);
            this.toolStripTravel.Text = "Display Travel Trip Statistics";
            this.toolStripTravel.Click += new System.EventHandler(this.toolStripTravel_Click);
            // 
            // toolStripMissionList
            // 
            this.toolStripMissionList.Checked = true;
            this.toolStripMissionList.CheckOnClick = true;
            this.toolStripMissionList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMissionList.Name = "toolStripMissionList";
            this.toolStripMissionList.Size = new System.Drawing.Size(255, 22);
            this.toolStripMissionList.Text = "Display Mission List";
            this.toolStripMissionList.Click += new System.EventHandler(this.toolStripMissionsList_Click);
            // 
            // toolStripSkinny
            // 
            this.toolStripSkinny.Checked = true;
            this.toolStripSkinny.CheckOnClick = true;
            this.toolStripSkinny.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripSkinny.Name = "toolStripSkinny";
            this.toolStripSkinny.Size = new System.Drawing.Size(255, 22);
            this.toolStripSkinny.Text = "When transparent, use skinny look";
            this.toolStripSkinny.Click += new System.EventHandler(this.whenTransparentUseSkinnyLookToolStripMenuItem_Click);
            // 
            // toolStripReset
            // 
            this.toolStripReset.Name = "toolStripReset";
            this.toolStripReset.Size = new System.Drawing.Size(255, 22);
            this.toolStripReset.Text = "Reset";
            this.toolStripReset.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // toolStripRemoveAll
            // 
            this.toolStripRemoveAll.Name = "toolStripRemoveAll";
            this.toolStripRemoveAll.Size = new System.Drawing.Size(255, 22);
            this.toolStripRemoveAll.Text = "Remove All";
            this.toolStripRemoveAll.Click += new System.EventHandler(this.toolStripRemoveAll_Click);
            // 
            // textBoxBody
            // 
            this.textBoxBody.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxBody.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxBody.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxBody.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBody.BorderColorScaling = 0.5F;
            this.textBoxBody.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxBody.ClearOnFirstChar = false;
            this.textBoxBody.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxBody.InErrorCondition = false;
            this.textBoxBody.Location = new System.Drawing.Point(52, 80);
            this.textBoxBody.Multiline = false;
            this.textBoxBody.Name = "textBoxBody";
            this.textBoxBody.ReadOnly = true;
            this.textBoxBody.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxBody.SelectionLength = 0;
            this.textBoxBody.SelectionStart = 0;
            this.textBoxBody.Size = new System.Drawing.Size(199, 20);
            this.textBoxBody.TabIndex = 3;
            this.textBoxBody.TabStop = false;
            this.textBoxBody.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxBody.WordWrap = true;
            this.textBoxBody.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxBody.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxBody.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxBody.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelBodyName
            // 
            this.labelBodyName.AutoSize = true;
            this.labelBodyName.Location = new System.Drawing.Point(3, 82);
            this.labelBodyName.Name = "labelBodyName";
            this.labelBodyName.Size = new System.Drawing.Size(31, 13);
            this.labelBodyName.TabIndex = 3;
            this.labelBodyName.Text = "Body";
            this.labelBodyName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelBodyName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelBodyName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxPosition.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxPosition.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxPosition.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxPosition.BorderColorScaling = 0.5F;
            this.textBoxPosition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPosition.ClearOnFirstChar = false;
            this.textBoxPosition.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxPosition.InErrorCondition = false;
            this.textBoxPosition.Location = new System.Drawing.Point(52, 104);
            this.textBoxPosition.Multiline = false;
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.ReadOnly = true;
            this.textBoxPosition.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxPosition.SelectionLength = 0;
            this.textBoxPosition.SelectionStart = 0;
            this.textBoxPosition.Size = new System.Drawing.Size(152, 20);
            this.textBoxPosition.TabIndex = 8;
            this.textBoxPosition.TabStop = false;
            this.textBoxPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxPosition.WordWrap = true;
            this.textBoxPosition.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxPosition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxPosition.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxPosition.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelPosition
            // 
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new System.Drawing.Point(3, 104);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new System.Drawing.Size(25, 13);
            this.labelPosition.TabIndex = 7;
            this.labelPosition.Text = "Pos";
            this.labelPosition.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelPosition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelPosition.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelPosition.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxVisits
            // 
            this.textBoxVisits.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxVisits.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxVisits.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxVisits.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxVisits.BorderColorScaling = 0.5F;
            this.textBoxVisits.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxVisits.ClearOnFirstChar = false;
            this.textBoxVisits.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxVisits.InErrorCondition = false;
            this.textBoxVisits.Location = new System.Drawing.Point(52, 56);
            this.textBoxVisits.Multiline = false;
            this.textBoxVisits.Name = "textBoxVisits";
            this.textBoxVisits.ReadOnly = true;
            this.textBoxVisits.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxVisits.SelectionLength = 0;
            this.textBoxVisits.SelectionStart = 0;
            this.textBoxVisits.Size = new System.Drawing.Size(32, 20);
            this.textBoxVisits.TabIndex = 10;
            this.textBoxVisits.TabStop = false;
            this.textBoxVisits.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxVisits.WordWrap = true;
            this.textBoxVisits.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxVisits.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxVisits.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxVisits.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelVisits
            // 
            this.labelVisits.AutoSize = true;
            this.labelVisits.Location = new System.Drawing.Point(3, 57);
            this.labelVisits.Name = "labelVisits";
            this.labelVisits.Size = new System.Drawing.Size(31, 13);
            this.labelVisits.TabIndex = 9;
            this.labelVisits.Text = "Visits";
            this.labelVisits.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelVisits.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelVisits.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelVisits.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelAllegiance
            // 
            this.labelAllegiance.AutoSize = true;
            this.labelAllegiance.Location = new System.Drawing.Point(125, 231);
            this.labelAllegiance.Name = "labelAllegiance";
            this.labelAllegiance.Size = new System.Drawing.Size(56, 13);
            this.labelAllegiance.TabIndex = 13;
            this.labelAllegiance.Text = "Allegiance";
            this.labelAllegiance.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelAllegiance.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelAllegiance.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelAllegiance.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelEconomy
            // 
            this.labelEconomy.AutoSize = true;
            this.labelEconomy.Location = new System.Drawing.Point(125, 249);
            this.labelEconomy.Name = "labelEconomy";
            this.labelEconomy.Size = new System.Drawing.Size(51, 13);
            this.labelEconomy.TabIndex = 34;
            this.labelEconomy.Text = "Economy";
            this.labelEconomy.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelEconomy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelEconomy.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelEconomy.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxAllegiance
            // 
            this.textBoxAllegiance.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxAllegiance.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxAllegiance.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxAllegiance.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxAllegiance.BorderColorScaling = 0.5F;
            this.textBoxAllegiance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxAllegiance.ClearOnFirstChar = false;
            this.textBoxAllegiance.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxAllegiance.InErrorCondition = false;
            this.textBoxAllegiance.Location = new System.Drawing.Point(191, 231);
            this.textBoxAllegiance.Multiline = false;
            this.textBoxAllegiance.Name = "textBoxAllegiance";
            this.textBoxAllegiance.ReadOnly = true;
            this.textBoxAllegiance.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxAllegiance.SelectionLength = 0;
            this.textBoxAllegiance.SelectionStart = 0;
            this.textBoxAllegiance.Size = new System.Drawing.Size(78, 20);
            this.textBoxAllegiance.TabIndex = 14;
            this.textBoxAllegiance.TabStop = false;
            this.textBoxAllegiance.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxAllegiance.WordWrap = true;
            this.textBoxAllegiance.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxAllegiance.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxAllegiance.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxAllegiance.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxGovernment
            // 
            this.textBoxGovernment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxGovernment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxGovernment.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxGovernment.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxGovernment.BorderColorScaling = 0.5F;
            this.textBoxGovernment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxGovernment.ClearOnFirstChar = false;
            this.textBoxGovernment.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxGovernment.InErrorCondition = false;
            this.textBoxGovernment.Location = new System.Drawing.Point(53, 249);
            this.textBoxGovernment.Multiline = false;
            this.textBoxGovernment.Name = "textBoxGovernment";
            this.textBoxGovernment.ReadOnly = true;
            this.textBoxGovernment.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxGovernment.SelectionLength = 0;
            this.textBoxGovernment.SelectionStart = 0;
            this.textBoxGovernment.Size = new System.Drawing.Size(78, 20);
            this.textBoxGovernment.TabIndex = 35;
            this.textBoxGovernment.TabStop = false;
            this.textBoxGovernment.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxGovernment.WordWrap = true;
            this.textBoxGovernment.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxGovernment.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxGovernment.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxGovernment.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelGov
            // 
            this.labelGov.AutoSize = true;
            this.labelGov.Location = new System.Drawing.Point(4, 249);
            this.labelGov.Name = "labelGov";
            this.labelGov.Size = new System.Drawing.Size(27, 13);
            this.labelGov.TabIndex = 36;
            this.labelGov.Text = "Gov";
            this.labelGov.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelGov.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelGov.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelGov.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelState
            // 
            this.labelState.AutoSize = true;
            this.labelState.Location = new System.Drawing.Point(4, 231);
            this.labelState.Name = "labelState";
            this.labelState.Size = new System.Drawing.Size(32, 13);
            this.labelState.TabIndex = 38;
            this.labelState.Text = "State";
            this.labelState.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelState.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelState.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelState.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxEconomy
            // 
            this.textBoxEconomy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxEconomy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxEconomy.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxEconomy.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxEconomy.BorderColorScaling = 0.5F;
            this.textBoxEconomy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxEconomy.ClearOnFirstChar = false;
            this.textBoxEconomy.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxEconomy.InErrorCondition = false;
            this.textBoxEconomy.Location = new System.Drawing.Point(191, 249);
            this.textBoxEconomy.Multiline = false;
            this.textBoxEconomy.Name = "textBoxEconomy";
            this.textBoxEconomy.ReadOnly = true;
            this.textBoxEconomy.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxEconomy.SelectionLength = 0;
            this.textBoxEconomy.SelectionStart = 0;
            this.textBoxEconomy.Size = new System.Drawing.Size(78, 20);
            this.textBoxEconomy.TabIndex = 33;
            this.textBoxEconomy.TabStop = false;
            this.textBoxEconomy.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxEconomy.WordWrap = true;
            this.textBoxEconomy.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxEconomy.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxEconomy.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxEconomy.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxState
            // 
            this.textBoxState.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxState.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxState.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxState.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxState.BorderColorScaling = 0.5F;
            this.textBoxState.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxState.ClearOnFirstChar = false;
            this.textBoxState.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxState.InErrorCondition = false;
            this.textBoxState.Location = new System.Drawing.Point(53, 231);
            this.textBoxState.Multiline = false;
            this.textBoxState.Name = "textBoxState";
            this.textBoxState.ReadOnly = true;
            this.textBoxState.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxState.SelectionLength = 0;
            this.textBoxState.SelectionStart = 0;
            this.textBoxState.Size = new System.Drawing.Size(78, 20);
            this.textBoxState.TabIndex = 37;
            this.textBoxState.TabStop = false;
            this.textBoxState.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxState.WordWrap = true;
            this.textBoxState.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxState.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxState.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxState.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // buttonEDDB
            // 
            this.buttonEDDB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonEDDB.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.EDDB;
            this.buttonEDDB.Location = new System.Drawing.Point(97, 30);
            this.buttonEDDB.Name = "buttonEDDB";
            this.buttonEDDB.Padding = new System.Windows.Forms.Padding(2);
            this.buttonEDDB.Size = new System.Drawing.Size(20, 20);
            this.buttonEDDB.TabIndex = 5;
            this.buttonEDDB.Click += new System.EventHandler(this.buttonEDDB_Click);
            this.buttonEDDB.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.buttonEDDB.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.buttonEDDB.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // buttonRoss
            // 
            this.buttonRoss.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonRoss.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Ross;
            this.buttonRoss.Location = new System.Drawing.Point(120, 30);
            this.buttonRoss.Name = "buttonRoss";
            this.buttonRoss.Padding = new System.Windows.Forms.Padding(2);
            this.buttonRoss.Size = new System.Drawing.Size(20, 20);
            this.buttonRoss.TabIndex = 6;
            this.buttonRoss.Click += new System.EventHandler(this.buttonRoss_Click);
            this.buttonRoss.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.buttonRoss.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.buttonRoss.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxHomeDist
            // 
            this.textBoxHomeDist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxHomeDist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxHomeDist.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxHomeDist.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxHomeDist.BorderColorScaling = 0.5F;
            this.textBoxHomeDist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxHomeDist.ClearOnFirstChar = false;
            this.textBoxHomeDist.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxHomeDist.InErrorCondition = false;
            this.textBoxHomeDist.Location = new System.Drawing.Point(60, 161);
            this.textBoxHomeDist.Multiline = false;
            this.textBoxHomeDist.Name = "textBoxHomeDist";
            this.textBoxHomeDist.ReadOnly = true;
            this.textBoxHomeDist.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxHomeDist.SelectionLength = 0;
            this.textBoxHomeDist.SelectionStart = 0;
            this.textBoxHomeDist.Size = new System.Drawing.Size(64, 20);
            this.textBoxHomeDist.TabIndex = 42;
            this.textBoxHomeDist.TabStop = false;
            this.textBoxHomeDist.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxHomeDist.WordWrap = true;
            this.textBoxHomeDist.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxHomeDist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxHomeDist.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxHomeDist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelHomeDist
            // 
            this.labelHomeDist.AutoSize = true;
            this.labelHomeDist.Location = new System.Drawing.Point(4, 160);
            this.labelHomeDist.Name = "labelHomeDist";
            this.labelHomeDist.Size = new System.Drawing.Size(35, 13);
            this.labelHomeDist.TabIndex = 43;
            this.labelHomeDist.Text = "Home";
            this.labelHomeDist.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelHomeDist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelHomeDist.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelHomeDist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // buttonEDSM
            // 
            this.buttonEDSM.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.InverseText;
            this.buttonEDSM.Location = new System.Drawing.Point(53, 30);
            this.buttonEDSM.Name = "buttonEDSM";
            this.buttonEDSM.Padding = new System.Windows.Forms.Padding(2);
            this.buttonEDSM.Size = new System.Drawing.Size(44, 20);
            this.buttonEDSM.TabIndex = 4;
            this.buttonEDSM.Text = "EDSM";
            this.buttonEDSM.Click += new System.EventHandler(this.buttonEDSM_Click);
            this.buttonEDSM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.buttonEDSM.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.buttonEDSM.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxSolDist
            // 
            this.textBoxSolDist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxSolDist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxSolDist.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxSolDist.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSolDist.BorderColorScaling = 0.5F;
            this.textBoxSolDist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSolDist.ClearOnFirstChar = false;
            this.textBoxSolDist.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSolDist.InErrorCondition = false;
            this.textBoxSolDist.Location = new System.Drawing.Point(245, 160);
            this.textBoxSolDist.Multiline = false;
            this.textBoxSolDist.Name = "textBoxSolDist";
            this.textBoxSolDist.ReadOnly = true;
            this.textBoxSolDist.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSolDist.SelectionLength = 0;
            this.textBoxSolDist.SelectionStart = 0;
            this.textBoxSolDist.Size = new System.Drawing.Size(64, 20);
            this.textBoxSolDist.TabIndex = 44;
            this.textBoxSolDist.TabStop = false;
            this.textBoxSolDist.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxSolDist.WordWrap = true;
            this.textBoxSolDist.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxSolDist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxSolDist.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxSolDist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelSolDist
            // 
            this.labelSolDist.AutoSize = true;
            this.labelSolDist.Location = new System.Drawing.Point(181, 160);
            this.labelSolDist.Name = "labelSolDist";
            this.labelSolDist.Size = new System.Drawing.Size(22, 13);
            this.labelSolDist.TabIndex = 45;
            this.labelSolDist.Text = "Sol";
            this.labelSolDist.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.labelSolDist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelSolDist.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelSolDist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelNote
            // 
            this.labelNote.AutoSize = true;
            this.labelNote.Location = new System.Drawing.Point(4, 275);
            this.labelNote.Name = "labelNote";
            this.labelNote.Size = new System.Drawing.Size(30, 13);
            this.labelNote.TabIndex = 28;
            this.labelNote.Text = "Note";
            this.labelNote.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelNote.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelNote.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // richTextBoxNote
            // 
            this.richTextBoxNote.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBoxNote.BorderColorScaling = 0.5F;
            this.richTextBoxNote.HideScrollBar = true;
            this.richTextBoxNote.Location = new System.Drawing.Point(51, 275);
            this.richTextBoxNote.Name = "richTextBoxNote";
            this.richTextBoxNote.ReadOnly = false;
            this.richTextBoxNote.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.richTextBoxNote.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.richTextBoxNote.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.richTextBoxNote.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxNote.ScrollBarBorderColor = System.Drawing.Color.White;
            this.richTextBoxNote.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.richTextBoxNote.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBoxNote.ScrollBarLineTweak = 0;
            this.richTextBoxNote.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.richTextBoxNote.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.richTextBoxNote.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.richTextBoxNote.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.richTextBoxNote.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.richTextBoxNote.ScrollBarWidth = 20;
            this.richTextBoxNote.ShowLineCount = false;
            this.richTextBoxNote.Size = new System.Drawing.Size(200, 50);
            this.richTextBoxNote.TabIndex = 0;
            this.richTextBoxNote.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxNote.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBoxNote.TextBoxChanged += new ExtendedControls.ExtRichTextBox.OnTextBoxChanged(this.richTextBoxNote_TextBoxChanged);
            this.richTextBoxNote.Leave += new System.EventHandler(this.richTextBoxNote_Leave);
            this.richTextBoxNote.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.richTextBoxNote.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.richTextBoxNote.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelTarget
            // 
            this.labelTarget.AutoSize = true;
            this.labelTarget.Location = new System.Drawing.Point(3, 338);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(38, 13);
            this.labelTarget.TabIndex = 16;
            this.labelTarget.Text = "Target";
            this.labelTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // buttonEDSMTarget
            // 
            this.buttonEDSMTarget.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.InverseText;
            this.buttonEDSMTarget.Location = new System.Drawing.Point(216, 335);
            this.buttonEDSMTarget.Name = "buttonEDSMTarget";
            this.buttonEDSMTarget.Padding = new System.Windows.Forms.Padding(2);
            this.buttonEDSMTarget.Size = new System.Drawing.Size(44, 20);
            this.buttonEDSMTarget.TabIndex = 23;
            this.buttonEDSMTarget.Text = "EDSM";
            this.buttonEDSMTarget.Click += new System.EventHandler(this.buttonEDSMTarget_Click);
            this.buttonEDSMTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.buttonEDSMTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.buttonEDSMTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxTarget
            // 
            this.textBoxTarget.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.textBoxTarget.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.textBoxTarget.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxTarget.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTarget.BorderColorScaling = 0.5F;
            this.textBoxTarget.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTarget.ClearOnFirstChar = false;
            this.textBoxTarget.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTarget.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.textBoxTarget.DropDownBorderColor = System.Drawing.Color.Green;
            this.textBoxTarget.DropDownHeight = 200;
            this.textBoxTarget.DropDownItemHeight = 13;
            this.textBoxTarget.DropDownMouseOverBackgroundColor = System.Drawing.Color.Red;
            this.textBoxTarget.DropDownScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.textBoxTarget.DropDownScrollBarColor = System.Drawing.Color.LightGray;
            this.textBoxTarget.DropDownWidth = 0;
            this.textBoxTarget.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxTarget.InErrorCondition = false;
            this.textBoxTarget.Location = new System.Drawing.Point(54, 335);
            this.textBoxTarget.Multiline = false;
            this.textBoxTarget.Name = "textBoxTarget";
            this.textBoxTarget.ReadOnly = false;
            this.textBoxTarget.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTarget.SelectionLength = 0;
            this.textBoxTarget.SelectionStart = 0;
            this.textBoxTarget.Size = new System.Drawing.Size(100, 20);
            this.textBoxTarget.TabIndex = 15;
            this.textBoxTarget.TabStop = false;
            this.textBoxTarget.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxTarget, "Sets the target");
            this.textBoxTarget.WordWrap = true;
            this.textBoxTarget.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxTarget.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textBoxTarget_KeyUp);
            this.textBoxTarget.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxTarget.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxTarget.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxTargetDist
            // 
            this.textBoxTargetDist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTargetDist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTargetDist.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxTargetDist.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTargetDist.BorderColorScaling = 0.5F;
            this.textBoxTargetDist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTargetDist.ClearOnFirstChar = false;
            this.textBoxTargetDist.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTargetDist.InErrorCondition = false;
            this.textBoxTargetDist.Location = new System.Drawing.Point(164, 335);
            this.textBoxTargetDist.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.textBoxTargetDist.Multiline = false;
            this.textBoxTargetDist.Name = "textBoxTargetDist";
            this.textBoxTargetDist.ReadOnly = true;
            this.textBoxTargetDist.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTargetDist.SelectionLength = 0;
            this.textBoxTargetDist.SelectionStart = 0;
            this.textBoxTargetDist.Size = new System.Drawing.Size(48, 20);
            this.textBoxTargetDist.TabIndex = 15;
            this.textBoxTargetDist.TabStop = false;
            this.textBoxTargetDist.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.toolTip1.SetToolTip(this.textBoxTargetDist, "Distance to target");
            this.textBoxTargetDist.WordWrap = true;
            this.textBoxTargetDist.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxTargetDist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxTargetDist.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxTargetDist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // toolTip1
            // 
            this.toolTip1.ShowAlways = true;
            // 
            // textBoxSystem
            // 
            this.textBoxSystem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxSystem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxSystem.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxSystem.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxSystem.BorderColorScaling = 0.5F;
            this.textBoxSystem.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxSystem.ClearOnFirstChar = false;
            this.textBoxSystem.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxSystem.InErrorCondition = false;
            this.textBoxSystem.Location = new System.Drawing.Point(52, 8);
            this.textBoxSystem.Multiline = false;
            this.textBoxSystem.Name = "textBoxSystem";
            this.textBoxSystem.ReadOnly = true;
            this.textBoxSystem.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxSystem.SelectionLength = 0;
            this.textBoxSystem.SelectionStart = 0;
            this.textBoxSystem.Size = new System.Drawing.Size(152, 20);
            this.textBoxSystem.TabIndex = 1;
            this.textBoxSystem.TabStop = false;
            this.textBoxSystem.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxSystem.WordWrap = true;
            this.textBoxSystem.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxSystem.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxSystem.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxSystem.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelSysName
            // 
            this.labelSysName.AutoSize = true;
            this.labelSysName.Location = new System.Drawing.Point(3, 8);
            this.labelSysName.Name = "labelSysName";
            this.labelSysName.Size = new System.Drawing.Size(41, 13);
            this.labelSysName.TabIndex = 4;
            this.labelSysName.Text = "System";
            this.labelSysName.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelSysName.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelSysName.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelOpen
            // 
            this.labelOpen.AutoSize = true;
            this.labelOpen.Location = new System.Drawing.Point(3, 32);
            this.labelOpen.Name = "labelOpen";
            this.labelOpen.Size = new System.Drawing.Size(33, 13);
            this.labelOpen.TabIndex = 3;
            this.labelOpen.Text = "Open";
            this.labelOpen.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelOpen.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelOpen.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelGamemode
            // 
            this.labelGamemode.AutoSize = true;
            this.labelGamemode.Location = new System.Drawing.Point(6, 523);
            this.labelGamemode.Name = "labelGamemode";
            this.labelGamemode.Size = new System.Drawing.Size(34, 13);
            this.labelGamemode.TabIndex = 16;
            this.labelGamemode.Text = "Mode";
            this.labelGamemode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelGamemode.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelGamemode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelTravel
            // 
            this.labelTravel.AutoSize = true;
            this.labelTravel.Location = new System.Drawing.Point(7, 549);
            this.labelTravel.Name = "labelTravel";
            this.labelTravel.Size = new System.Drawing.Size(37, 13);
            this.labelTravel.TabIndex = 16;
            this.labelTravel.Text = "Travel";
            this.labelTravel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelTravel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelTravel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxGameMode
            // 
            this.textBoxGameMode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxGameMode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxGameMode.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxGameMode.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxGameMode.BorderColorScaling = 0.5F;
            this.textBoxGameMode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxGameMode.ClearOnFirstChar = false;
            this.textBoxGameMode.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxGameMode.InErrorCondition = false;
            this.textBoxGameMode.Location = new System.Drawing.Point(54, 523);
            this.textBoxGameMode.Multiline = false;
            this.textBoxGameMode.Name = "textBoxGameMode";
            this.textBoxGameMode.ReadOnly = true;
            this.textBoxGameMode.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxGameMode.SelectionLength = 0;
            this.textBoxGameMode.SelectionStart = 0;
            this.textBoxGameMode.Size = new System.Drawing.Size(152, 20);
            this.textBoxGameMode.TabIndex = 8;
            this.textBoxGameMode.TabStop = false;
            this.textBoxGameMode.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxGameMode.WordWrap = true;
            this.textBoxGameMode.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxGameMode.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxGameMode.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxGameMode.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxTravelDist
            // 
            this.textBoxTravelDist.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTravelDist.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTravelDist.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxTravelDist.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTravelDist.BorderColorScaling = 0.5F;
            this.textBoxTravelDist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTravelDist.ClearOnFirstChar = false;
            this.textBoxTravelDist.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTravelDist.InErrorCondition = false;
            this.textBoxTravelDist.Location = new System.Drawing.Point(54, 549);
            this.textBoxTravelDist.Multiline = false;
            this.textBoxTravelDist.Name = "textBoxTravelDist";
            this.textBoxTravelDist.ReadOnly = true;
            this.textBoxTravelDist.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTravelDist.SelectionLength = 0;
            this.textBoxTravelDist.SelectionStart = 0;
            this.textBoxTravelDist.Size = new System.Drawing.Size(70, 20);
            this.textBoxTravelDist.TabIndex = 8;
            this.textBoxTravelDist.TabStop = false;
            this.textBoxTravelDist.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxTravelDist.WordWrap = true;
            this.textBoxTravelDist.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxTravelDist.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxTravelDist.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxTravelDist.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxTravelTime
            // 
            this.textBoxTravelTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTravelTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTravelTime.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxTravelTime.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTravelTime.BorderColorScaling = 0.5F;
            this.textBoxTravelTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTravelTime.ClearOnFirstChar = false;
            this.textBoxTravelTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTravelTime.InErrorCondition = false;
            this.textBoxTravelTime.Location = new System.Drawing.Point(131, 549);
            this.textBoxTravelTime.Multiline = false;
            this.textBoxTravelTime.Name = "textBoxTravelTime";
            this.textBoxTravelTime.ReadOnly = true;
            this.textBoxTravelTime.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTravelTime.SelectionLength = 0;
            this.textBoxTravelTime.SelectionStart = 0;
            this.textBoxTravelTime.Size = new System.Drawing.Size(72, 20);
            this.textBoxTravelTime.TabIndex = 8;
            this.textBoxTravelTime.TabStop = false;
            this.textBoxTravelTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxTravelTime.WordWrap = true;
            this.textBoxTravelTime.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxTravelTime.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxTravelTime.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxTravelTime.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxTravelJumps
            // 
            this.textBoxTravelJumps.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxTravelJumps.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxTravelJumps.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxTravelJumps.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxTravelJumps.BorderColorScaling = 0.5F;
            this.textBoxTravelJumps.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxTravelJumps.ClearOnFirstChar = false;
            this.textBoxTravelJumps.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxTravelJumps.InErrorCondition = false;
            this.textBoxTravelJumps.Location = new System.Drawing.Point(211, 549);
            this.textBoxTravelJumps.Multiline = false;
            this.textBoxTravelJumps.Name = "textBoxTravelJumps";
            this.textBoxTravelJumps.ReadOnly = true;
            this.textBoxTravelJumps.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxTravelJumps.SelectionLength = 0;
            this.textBoxTravelJumps.SelectionStart = 0;
            this.textBoxTravelJumps.Size = new System.Drawing.Size(48, 20);
            this.textBoxTravelJumps.TabIndex = 8;
            this.textBoxTravelJumps.TabStop = false;
            this.textBoxTravelJumps.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxTravelJumps.WordWrap = true;
            this.textBoxTravelJumps.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxTravelJumps.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxTravelJumps.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxTravelJumps.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelCargo
            // 
            this.labelCargo.AutoSize = true;
            this.labelCargo.Location = new System.Drawing.Point(6, 436);
            this.labelCargo.Name = "labelCargo";
            this.labelCargo.Size = new System.Drawing.Size(35, 13);
            this.labelCargo.TabIndex = 16;
            this.labelCargo.Text = "Cargo";
            this.labelCargo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelCargo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelCargo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxCargo
            // 
            this.textBoxCargo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxCargo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxCargo.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxCargo.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCargo.BorderColorScaling = 0.5F;
            this.textBoxCargo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCargo.ClearOnFirstChar = false;
            this.textBoxCargo.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxCargo.InErrorCondition = false;
            this.textBoxCargo.Location = new System.Drawing.Point(54, 433);
            this.textBoxCargo.Multiline = false;
            this.textBoxCargo.Name = "textBoxCargo";
            this.textBoxCargo.ReadOnly = true;
            this.textBoxCargo.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxCargo.SelectionLength = 0;
            this.textBoxCargo.SelectionStart = 0;
            this.textBoxCargo.Size = new System.Drawing.Size(48, 20);
            this.textBoxCargo.TabIndex = 8;
            this.textBoxCargo.TabStop = false;
            this.textBoxCargo.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxCargo.WordWrap = true;
            this.textBoxCargo.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxCargo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxCargo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxCargo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxMaterials
            // 
            this.textBoxMaterials.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxMaterials.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxMaterials.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxMaterials.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxMaterials.BorderColorScaling = 0.5F;
            this.textBoxMaterials.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxMaterials.ClearOnFirstChar = false;
            this.textBoxMaterials.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxMaterials.InErrorCondition = false;
            this.textBoxMaterials.Location = new System.Drawing.Point(60, 492);
            this.textBoxMaterials.Multiline = false;
            this.textBoxMaterials.Name = "textBoxMaterials";
            this.textBoxMaterials.ReadOnly = true;
            this.textBoxMaterials.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxMaterials.SelectionLength = 0;
            this.textBoxMaterials.SelectionStart = 0;
            this.textBoxMaterials.Size = new System.Drawing.Size(48, 20);
            this.textBoxMaterials.TabIndex = 8;
            this.textBoxMaterials.TabStop = false;
            this.textBoxMaterials.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxMaterials.WordWrap = true;
            this.textBoxMaterials.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxMaterials.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxMaterials.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxMaterials.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelMaterials
            // 
            this.labelMaterials.AutoSize = true;
            this.labelMaterials.Location = new System.Drawing.Point(-3, 495);
            this.labelMaterials.Name = "labelMaterials";
            this.labelMaterials.Size = new System.Drawing.Size(49, 13);
            this.labelMaterials.TabIndex = 16;
            this.labelMaterials.Text = "Materials";
            this.labelMaterials.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelMaterials.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelMaterials.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelData
            // 
            this.labelData.AutoSize = true;
            this.labelData.Location = new System.Drawing.Point(15, 469);
            this.labelData.Name = "labelData";
            this.labelData.Size = new System.Drawing.Size(30, 13);
            this.labelData.TabIndex = 16;
            this.labelData.Text = "Data";
            this.labelData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelData.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxData
            // 
            this.textBoxData.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxData.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxData.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxData.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxData.BorderColorScaling = 0.5F;
            this.textBoxData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxData.ClearOnFirstChar = false;
            this.textBoxData.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxData.InErrorCondition = false;
            this.textBoxData.Location = new System.Drawing.Point(51, 466);
            this.textBoxData.Multiline = false;
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.ReadOnly = true;
            this.textBoxData.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxData.SelectionLength = 0;
            this.textBoxData.SelectionStart = 0;
            this.textBoxData.Size = new System.Drawing.Size(48, 20);
            this.textBoxData.TabIndex = 8;
            this.textBoxData.TabStop = false;
            this.textBoxData.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxData.WordWrap = true;
            this.textBoxData.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxData.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxData.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelShip
            // 
            this.labelShip.AutoSize = true;
            this.labelShip.Location = new System.Drawing.Point(6, 361);
            this.labelShip.Name = "labelShip";
            this.labelShip.Size = new System.Drawing.Size(28, 13);
            this.labelShip.TabIndex = 16;
            this.labelShip.Text = "Ship";
            this.labelShip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelShip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelShip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxShip
            // 
            this.textBoxShip.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxShip.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxShip.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxShip.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxShip.BorderColorScaling = 0.5F;
            this.textBoxShip.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxShip.ClearOnFirstChar = false;
            this.textBoxShip.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxShip.InErrorCondition = false;
            this.textBoxShip.Location = new System.Drawing.Point(54, 361);
            this.textBoxShip.Multiline = false;
            this.textBoxShip.Name = "textBoxShip";
            this.textBoxShip.ReadOnly = true;
            this.textBoxShip.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxShip.SelectionLength = 0;
            this.textBoxShip.SelectionStart = 0;
            this.textBoxShip.Size = new System.Drawing.Size(200, 20);
            this.textBoxShip.TabIndex = 8;
            this.textBoxShip.TabStop = false;
            this.textBoxShip.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxShip.WordWrap = true;
            this.textBoxShip.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxShip.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxShip.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxShip.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelFuel
            // 
            this.labelFuel.AutoSize = true;
            this.labelFuel.Location = new System.Drawing.Point(6, 387);
            this.labelFuel.Name = "labelFuel";
            this.labelFuel.Size = new System.Drawing.Size(27, 13);
            this.labelFuel.TabIndex = 16;
            this.labelFuel.Text = "Fuel";
            this.labelFuel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelFuel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelFuel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxFuel
            // 
            this.textBoxFuel.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFuel.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFuel.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxFuel.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFuel.BorderColorScaling = 0.5F;
            this.textBoxFuel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFuel.ClearOnFirstChar = false;
            this.textBoxFuel.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFuel.InErrorCondition = false;
            this.textBoxFuel.Location = new System.Drawing.Point(53, 387);
            this.textBoxFuel.Multiline = false;
            this.textBoxFuel.Name = "textBoxFuel";
            this.textBoxFuel.ReadOnly = true;
            this.textBoxFuel.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFuel.SelectionLength = 0;
            this.textBoxFuel.SelectionStart = 0;
            this.textBoxFuel.Size = new System.Drawing.Size(64, 20);
            this.textBoxFuel.TabIndex = 8;
            this.textBoxFuel.TabStop = false;
            this.textBoxFuel.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxFuel.WordWrap = true;
            this.textBoxFuel.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxFuel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxFuel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxFuel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxCredits
            // 
            this.textBoxCredits.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxCredits.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxCredits.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxCredits.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxCredits.BorderColorScaling = 0.5F;
            this.textBoxCredits.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxCredits.ClearOnFirstChar = false;
            this.textBoxCredits.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxCredits.InErrorCondition = false;
            this.textBoxCredits.Location = new System.Drawing.Point(60, 644);
            this.textBoxCredits.Multiline = false;
            this.textBoxCredits.Name = "textBoxCredits";
            this.textBoxCredits.ReadOnly = true;
            this.textBoxCredits.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxCredits.SelectionLength = 0;
            this.textBoxCredits.SelectionStart = 0;
            this.textBoxCredits.Size = new System.Drawing.Size(82, 20);
            this.textBoxCredits.TabIndex = 8;
            this.textBoxCredits.TabStop = false;
            this.textBoxCredits.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxCredits.WordWrap = true;
            this.textBoxCredits.Click += new System.EventHandler(this.clickTextBox);
            this.textBoxCredits.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.textBoxCredits.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.textBoxCredits.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelCredits
            // 
            this.labelCredits.AutoSize = true;
            this.labelCredits.Location = new System.Drawing.Point(4, 644);
            this.labelCredits.Name = "labelCredits";
            this.labelCredits.Size = new System.Drawing.Size(39, 13);
            this.labelCredits.TabIndex = 16;
            this.labelCredits.Text = "Credits";
            this.labelCredits.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelCredits.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelCredits.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // panelFD
            // 
            this.panelFD.BackgroundImage = global::EDDiscovery.Icons.Controls.firstdiscover;
            this.panelFD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelFD.Location = new System.Drawing.Point(206, 8);
            this.panelFD.Name = "panelFD";
            this.panelFD.Size = new System.Drawing.Size(24, 24);
            this.panelFD.TabIndex = 46;
            // 
            // richTextBoxScrollMissions
            // 
            this.richTextBoxScrollMissions.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBoxScrollMissions.BorderColorScaling = 0.5F;
            this.richTextBoxScrollMissions.HideScrollBar = true;
            this.richTextBoxScrollMissions.Location = new System.Drawing.Point(54, 575);
            this.richTextBoxScrollMissions.Name = "richTextBoxScrollMissions";
            this.richTextBoxScrollMissions.ReadOnly = false;
            this.richTextBoxScrollMissions.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.richTextBoxScrollMissions.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.richTextBoxScrollMissions.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.richTextBoxScrollMissions.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxScrollMissions.ScrollBarBorderColor = System.Drawing.Color.White;
            this.richTextBoxScrollMissions.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.richTextBoxScrollMissions.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBoxScrollMissions.ScrollBarLineTweak = 0;
            this.richTextBoxScrollMissions.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.richTextBoxScrollMissions.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.richTextBoxScrollMissions.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.richTextBoxScrollMissions.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.richTextBoxScrollMissions.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.richTextBoxScrollMissions.ScrollBarWidth = 20;
            this.richTextBoxScrollMissions.ShowLineCount = false;
            this.richTextBoxScrollMissions.Size = new System.Drawing.Size(200, 50);
            this.richTextBoxScrollMissions.TabIndex = 0;
            this.richTextBoxScrollMissions.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxScrollMissions.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBoxScrollMissions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.richTextBoxScrollMissions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.richTextBoxScrollMissions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // labelMissions
            // 
            this.labelMissions.AutoSize = true;
            this.labelMissions.Location = new System.Drawing.Point(1, 575);
            this.labelMissions.Name = "labelMissions";
            this.labelMissions.Size = new System.Drawing.Size(47, 13);
            this.labelMissions.TabIndex = 28;
            this.labelMissions.Text = "Missions";
            this.labelMissions.MouseDown += new System.Windows.Forms.MouseEventHandler(this.controlMouseDown);
            this.labelMissions.MouseMove += new System.Windows.Forms.MouseEventHandler(this.controlMouseMove);
            this.labelMissions.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controlMouseUp);
            // 
            // textBoxJumpRange
            // 
            this.textBoxJumpRange.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxJumpRange.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxJumpRange.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxJumpRange.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxJumpRange.BorderColorScaling = 0.5F;
            this.textBoxJumpRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxJumpRange.ClearOnFirstChar = false;
            this.textBoxJumpRange.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxJumpRange.InErrorCondition = false;
            this.textBoxJumpRange.Location = new System.Drawing.Point(57, 671);
            this.textBoxJumpRange.Multiline = false;
            this.textBoxJumpRange.Name = "textBoxJumpRange";
            this.textBoxJumpRange.ReadOnly = true;
            this.textBoxJumpRange.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxJumpRange.SelectionLength = 0;
            this.textBoxJumpRange.SelectionStart = 0;
            this.textBoxJumpRange.Size = new System.Drawing.Size(48, 20);
            this.textBoxJumpRange.TabIndex = 47;
            this.textBoxJumpRange.TabStop = false;
            this.textBoxJumpRange.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxJumpRange.WordWrap = true;
            // 
            // labelJumpRange
            // 
            this.labelJumpRange.AutoSize = true;
            this.labelJumpRange.Location = new System.Drawing.Point(9, 674);
            this.labelJumpRange.Name = "labelJumpRange";
            this.labelJumpRange.Size = new System.Drawing.Size(32, 13);
            this.labelJumpRange.TabIndex = 48;
            this.labelJumpRange.Text = "Jump";
            // 
            // displayJumpRangeToolStripMenuItem
            // 
            this.toolStripJumpRange.Checked = true;
            this.toolStripJumpRange.CheckOnClick = true;
            this.toolStripJumpRange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripJumpRange.Name = "toolStripJumpRange";
            this.toolStripJumpRange.Size = new System.Drawing.Size(255, 22);
            this.toolStripJumpRange.Text = "Display Jump Range";
            this.toolStripJumpRange.Click += new System.EventHandler(this.displayJumpRangeToolStripMenuItem_Click);
            // 
            // UserControlSysInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip;
            this.Controls.Add(this.textBoxJumpRange);
            this.Controls.Add(this.labelJumpRange);
            this.Controls.Add(this.panelFD);
            this.Controls.Add(this.labelSysName);
            this.Controls.Add(this.textBoxSystem);
            this.Controls.Add(this.labelOpen);
            this.Controls.Add(this.buttonEDSM);
            this.Controls.Add(this.buttonEDDB);
            this.Controls.Add(this.buttonRoss);
            this.Controls.Add(this.labelVisits);
            this.Controls.Add(this.textBoxVisits);
            this.Controls.Add(this.labelBodyName);
            this.Controls.Add(this.textBoxBody);
            this.Controls.Add(this.labelPosition);
            this.Controls.Add(this.textBoxTravelJumps);
            this.Controls.Add(this.textBoxTravelTime);
            this.Controls.Add(this.textBoxFuel);
            this.Controls.Add(this.textBoxData);
            this.Controls.Add(this.textBoxMaterials);
            this.Controls.Add(this.textBoxCredits);
            this.Controls.Add(this.textBoxCargo);
            this.Controls.Add(this.textBoxTravelDist);
            this.Controls.Add(this.textBoxShip);
            this.Controls.Add(this.textBoxGameMode);
            this.Controls.Add(this.textBoxPosition);
            this.Controls.Add(this.labelHomeDist);
            this.Controls.Add(this.textBoxHomeDist);
            this.Controls.Add(this.labelSolDist);
            this.Controls.Add(this.textBoxSolDist);
            this.Controls.Add(this.labelState);
            this.Controls.Add(this.textBoxState);
            this.Controls.Add(this.labelAllegiance);
            this.Controls.Add(this.textBoxAllegiance);
            this.Controls.Add(this.labelGov);
            this.Controls.Add(this.textBoxGovernment);
            this.Controls.Add(this.labelEconomy);
            this.Controls.Add(this.textBoxEconomy);
            this.Controls.Add(this.labelMissions);
            this.Controls.Add(this.labelNote);
            this.Controls.Add(this.richTextBoxScrollMissions);
            this.Controls.Add(this.richTextBoxNote);
            this.Controls.Add(this.labelFuel);
            this.Controls.Add(this.labelData);
            this.Controls.Add(this.labelMaterials);
            this.Controls.Add(this.labelShip);
            this.Controls.Add(this.labelCredits);
            this.Controls.Add(this.labelCargo);
            this.Controls.Add(this.labelTravel);
            this.Controls.Add(this.labelGamemode);
            this.Controls.Add(this.labelTarget);
            this.Controls.Add(this.textBoxTarget);
            this.Controls.Add(this.textBoxTargetDist);
            this.Controls.Add(this.buttonEDSMTarget);
            this.Name = "UserControlSysInfo";
            this.Size = new System.Drawing.Size(393, 731);
            this.toolTip1.SetToolTip(this, "Hold down Ctrl Key then left drag a item to reposition, 8 columns are available");
            this.Resize += new System.EventHandler(this.UserControlSysInfo_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private ExtendedControls.ExtTextBox textBoxBody;
        private System.Windows.Forms.Label labelBodyName;
        private ExtendedControls.ExtTextBox textBoxPosition;
        private System.Windows.Forms.Label labelPosition;
        private ExtendedControls.ExtTextBox textBoxVisits;
        private System.Windows.Forms.Label labelVisits;
        private System.Windows.Forms.Label labelAllegiance;
        private System.Windows.Forms.Label labelEconomy;
        private ExtendedControls.ExtTextBox textBoxAllegiance;
        private ExtendedControls.ExtTextBox textBoxGovernment;
        private System.Windows.Forms.Label labelGov;
        private System.Windows.Forms.Label labelState;
        private ExtendedControls.ExtTextBox textBoxEconomy;
        private ExtendedControls.ExtTextBox textBoxState;
        private ExtendedControls.ExtPanelDrawn buttonEDDB;
        private ExtendedControls.ExtPanelDrawn buttonRoss;
        private ExtendedControls.ExtTextBox textBoxHomeDist;
        private System.Windows.Forms.Label labelHomeDist;
        private ExtendedControls.ExtPanelDrawn buttonEDSM;
        private ExtendedControls.ExtTextBox textBoxSolDist;
        private System.Windows.Forms.Label labelSolDist;
        private ExtendedControls.ExtRichTextBox richTextBoxNote;
        private System.Windows.Forms.Label labelNote;
        private System.Windows.Forms.Label labelTarget;
        private ExtendedControls.ExtPanelDrawn buttonEDSMTarget;
        private ExtendedControls.ExtTextBoxAutoComplete textBoxTarget;
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.ExtTextBox textBoxTargetDist;
        private ExtendedControls.ExtTextBox textBoxSystem;
        private System.Windows.Forms.Label labelSysName;
        private System.Windows.Forms.ToolStripMenuItem toolStripNotes;
        private System.Windows.Forms.ToolStripMenuItem toolStripSystem;
        private System.Windows.Forms.ToolStripMenuItem toolStripBody;
        private System.Windows.Forms.ToolStripMenuItem toolStripTarget;
        private System.Windows.Forms.ToolStripMenuItem toolStripEDSMDownLine;
        private System.Windows.Forms.ToolStripMenuItem toolStripEDSM;
        private System.Windows.Forms.Label labelOpen;
        private System.Windows.Forms.ToolStripMenuItem toolStripVisits;
        private System.Windows.Forms.ToolStripMenuItem toolStripPosition;
        private System.Windows.Forms.ToolStripMenuItem toolStripSystemState;
        private System.Windows.Forms.ToolStripMenuItem toolStripDistanceFrom;
        private System.Windows.Forms.Label labelGamemode;
        private System.Windows.Forms.Label labelTravel;
        private ExtendedControls.ExtTextBox textBoxGameMode;
        private ExtendedControls.ExtTextBox textBoxTravelDist;
        private ExtendedControls.ExtTextBox textBoxTravelTime;
        private ExtendedControls.ExtTextBox textBoxTravelJumps;
        private System.Windows.Forms.Label labelCargo;
        private ExtendedControls.ExtTextBox textBoxCargo;
        private ExtendedControls.ExtTextBox textBoxMaterials;
        private System.Windows.Forms.Label labelMaterials;
        private System.Windows.Forms.ToolStripMenuItem toolStripGameMode;
        private System.Windows.Forms.ToolStripMenuItem toolStripTravel;
        private System.Windows.Forms.ToolStripMenuItem toolStripCargo;
        private System.Windows.Forms.Label labelData;
        private ExtendedControls.ExtTextBox textBoxData;
        private System.Windows.Forms.Label labelShip;
        private ExtendedControls.ExtTextBox textBoxShip;
        private System.Windows.Forms.ToolStripMenuItem toolStripShip;
        private System.Windows.Forms.ToolStripMenuItem toolStripReset;
        private System.Windows.Forms.Label labelFuel;
        private ExtendedControls.ExtTextBox textBoxFuel;
        private System.Windows.Forms.ToolStripMenuItem toolStripSkinny;
        private System.Windows.Forms.ToolStripMenuItem toolStripFuel;
        private System.Windows.Forms.ToolStripMenuItem toolStripMaterialCounts;
        private System.Windows.Forms.ToolStripMenuItem toolStripDataCount;
        private ExtendedControls.ExtTextBox textBoxCredits;
        private System.Windows.Forms.Label labelCredits;
        private System.Windows.Forms.ToolStripMenuItem toolStripCredits;
        private System.Windows.Forms.ToolStripMenuItem toolStripRemoveAll;
        private ExtendedControls.PanelNoTheme panelFD;
        private ExtendedControls.ExtRichTextBox richTextBoxScrollMissions;
        private System.Windows.Forms.Label labelMissions;
        private System.Windows.Forms.ToolStripMenuItem toolStripMissionList;
        private ExtendedControls.ExtTextBox textBoxJumpRange;
        private System.Windows.Forms.Label labelJumpRange;
        private System.Windows.Forms.ToolStripMenuItem toolStripJumpRange;
    }
}
