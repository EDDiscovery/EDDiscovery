/*
 * Copyright © 2016 EDDiscovery development team
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
namespace EDDiscovery.Forms
{
    partial class RecordStep
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
            this.labelGoto = new System.Windows.Forms.Label();
            this.labelPAt = new System.Windows.Forms.Label();
            this.labelTimeChange = new System.Windows.Forms.Label();
            this.labelTimeDir = new System.Windows.Forms.Label();
            this.labelZoom = new System.Windows.Forms.Label();
            this.textBoxPos = new ExtendedControls.ExtTextBox();
            this.textBoxDir = new ExtendedControls.ExtTextBox();
            this.textBoxZoom = new ExtendedControls.ExtTextBox();
            this.textBoxFlyTime = new ExtendedControls.ExtTextBox();
            this.textBoxPanTime = new ExtendedControls.ExtTextBox();
            this.textBoxWait = new ExtendedControls.ExtTextBox();
            this.buttonOK = new ExtendedControls.ExtButton();
            this.buttonCancel = new ExtendedControls.ExtButton();
            this.textBoxMessage = new ExtendedControls.ExtTextBox();
            this.labelMsg = new System.Windows.Forms.Label();
            this.labelWait = new System.Windows.Forms.Label();
            this.textBoxZoomTime = new ExtendedControls.ExtTextBox();
            this.labelZoomTime = new System.Windows.Forms.Label();
            this.labelMsgTime = new System.Windows.Forms.Label();
            this.textBoxMsgTime = new ExtendedControls.ExtTextBox();
            this.checkBoxWaitForSlew = new ExtendedControls.ExtCheckBox();
            this.checkBoxPos = new ExtendedControls.ExtCheckBox();
            this.checkBoxPan = new ExtendedControls.ExtCheckBox();
            this.checkBoxChangeZoom = new ExtendedControls.ExtCheckBox();
            this.labelTMEInfo = new System.Windows.Forms.Label();
            this.checkBoxWaitComplete = new ExtendedControls.ExtCheckBox();
            this.checkBoxDisplayMessageWhenComplete = new ExtendedControls.ExtCheckBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.panel_close = new ExtendedControls.ExtPanelDrawn();
            this.panel_minimize = new ExtendedControls.ExtPanelDrawn();
            this.label_index = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelTop.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelGoto
            // 
            this.labelGoto.AutoSize = true;
            this.labelGoto.Location = new System.Drawing.Point(3, 52);
            this.labelGoto.Name = "labelGoto";
            this.labelGoto.Size = new System.Drawing.Size(33, 13);
            this.labelGoto.TabIndex = 0;
            this.labelGoto.Text = "Go to";
            // 
            // labelPAt
            // 
            this.labelPAt.AutoSize = true;
            this.labelPAt.Location = new System.Drawing.Point(3, 115);
            this.labelPAt.Name = "labelPAt";
            this.labelPAt.Size = new System.Drawing.Size(58, 13);
            this.labelPAt.TabIndex = 0;
            this.labelPAt.Text = "Pointing At";
            // 
            // labelTimeChange
            // 
            this.labelTimeChange.AutoSize = true;
            this.labelTimeChange.Location = new System.Drawing.Point(2, 79);
            this.labelTimeChange.Name = "labelTimeChange";
            this.labelTimeChange.Size = new System.Drawing.Size(137, 13);
            this.labelTimeChange.TabIndex = 0;
            this.labelTimeChange.Text = "Time to take to change pos";
            // 
            // labelTimeDir
            // 
            this.labelTimeDir.AutoSize = true;
            this.labelTimeDir.Location = new System.Drawing.Point(3, 145);
            this.labelTimeDir.Name = "labelTimeDir";
            this.labelTimeDir.Size = new System.Drawing.Size(160, 13);
            this.labelTimeDir.TabIndex = 0;
            this.labelTimeDir.Text = "Time to take to change direction";
            // 
            // labelZoom
            // 
            this.labelZoom.AutoSize = true;
            this.labelZoom.Location = new System.Drawing.Point(3, 185);
            this.labelZoom.Name = "labelZoom";
            this.labelZoom.Size = new System.Drawing.Size(34, 13);
            this.labelZoom.TabIndex = 0;
            this.labelZoom.Text = "Zoom";
            // 
            // textBoxPos
            // 
            this.textBoxPos.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxPos.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxPos.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxPos.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxPos.BorderColorScaling = 0.5F;
            this.textBoxPos.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPos.ClearOnFirstChar = false;
            this.textBoxPos.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxPos.InErrorCondition = false;
            this.textBoxPos.Location = new System.Drawing.Point(273, 52);
            this.textBoxPos.Multiline = false;
            this.textBoxPos.Name = "textBoxPos";
            this.textBoxPos.ReadOnly = true;
            this.textBoxPos.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxPos.SelectionLength = 0;
            this.textBoxPos.SelectionStart = 0;
            this.textBoxPos.Size = new System.Drawing.Size(186, 20);
            this.textBoxPos.TabIndex = 1;
            this.textBoxPos.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxPos.WordWrap = true;
            // 
            // textBoxDir
            // 
            this.textBoxDir.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxDir.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxDir.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxDir.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxDir.BorderColorScaling = 0.5F;
            this.textBoxDir.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxDir.ClearOnFirstChar = false;
            this.textBoxDir.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxDir.InErrorCondition = false;
            this.textBoxDir.Location = new System.Drawing.Point(273, 115);
            this.textBoxDir.Multiline = false;
            this.textBoxDir.Name = "textBoxDir";
            this.textBoxDir.ReadOnly = true;
            this.textBoxDir.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxDir.SelectionLength = 0;
            this.textBoxDir.SelectionStart = 0;
            this.textBoxDir.Size = new System.Drawing.Size(186, 20);
            this.textBoxDir.TabIndex = 1;
            this.textBoxDir.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxDir.WordWrap = true;
            // 
            // textBoxZoom
            // 
            this.textBoxZoom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxZoom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxZoom.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxZoom.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxZoom.BorderColorScaling = 0.5F;
            this.textBoxZoom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxZoom.ClearOnFirstChar = false;
            this.textBoxZoom.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxZoom.InErrorCondition = false;
            this.textBoxZoom.Location = new System.Drawing.Point(273, 185);
            this.textBoxZoom.Multiline = false;
            this.textBoxZoom.Name = "textBoxZoom";
            this.textBoxZoom.ReadOnly = true;
            this.textBoxZoom.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxZoom.SelectionLength = 0;
            this.textBoxZoom.SelectionStart = 0;
            this.textBoxZoom.Size = new System.Drawing.Size(100, 20);
            this.textBoxZoom.TabIndex = 1;
            this.textBoxZoom.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxZoom.WordWrap = true;
            // 
            // textBoxFlyTime
            // 
            this.textBoxFlyTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxFlyTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxFlyTime.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxFlyTime.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxFlyTime.BorderColorScaling = 0.5F;
            this.textBoxFlyTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxFlyTime.ClearOnFirstChar = false;
            this.textBoxFlyTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxFlyTime.InErrorCondition = false;
            this.textBoxFlyTime.Location = new System.Drawing.Point(273, 79);
            this.textBoxFlyTime.Multiline = false;
            this.textBoxFlyTime.Name = "textBoxFlyTime";
            this.textBoxFlyTime.ReadOnly = false;
            this.textBoxFlyTime.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxFlyTime.SelectionLength = 0;
            this.textBoxFlyTime.SelectionStart = 0;
            this.textBoxFlyTime.Size = new System.Drawing.Size(100, 20);
            this.textBoxFlyTime.TabIndex = 1;
            this.textBoxFlyTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxFlyTime.WordWrap = true;
            this.textBoxFlyTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxPanTime
            // 
            this.textBoxPanTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxPanTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxPanTime.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxPanTime.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxPanTime.BorderColorScaling = 0.5F;
            this.textBoxPanTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxPanTime.ClearOnFirstChar = false;
            this.textBoxPanTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxPanTime.InErrorCondition = false;
            this.textBoxPanTime.Location = new System.Drawing.Point(273, 145);
            this.textBoxPanTime.Multiline = false;
            this.textBoxPanTime.Name = "textBoxPanTime";
            this.textBoxPanTime.ReadOnly = false;
            this.textBoxPanTime.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxPanTime.SelectionLength = 0;
            this.textBoxPanTime.SelectionStart = 0;
            this.textBoxPanTime.Size = new System.Drawing.Size(100, 20);
            this.textBoxPanTime.TabIndex = 2;
            this.textBoxPanTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxPanTime.WordWrap = true;
            this.textBoxPanTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxWait
            // 
            this.textBoxWait.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxWait.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxWait.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxWait.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxWait.BorderColorScaling = 0.5F;
            this.textBoxWait.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxWait.ClearOnFirstChar = false;
            this.textBoxWait.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxWait.InErrorCondition = false;
            this.textBoxWait.Location = new System.Drawing.Point(273, 17);
            this.textBoxWait.Multiline = false;
            this.textBoxWait.Name = "textBoxWait";
            this.textBoxWait.ReadOnly = false;
            this.textBoxWait.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxWait.SelectionLength = 0;
            this.textBoxWait.SelectionStart = 0;
            this.textBoxWait.Size = new System.Drawing.Size(100, 20);
            this.textBoxWait.TabIndex = 0;
            this.textBoxWait.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxWait.WordWrap = true;
            this.textBoxWait.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(458, 377);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 23);
            this.buttonOK.TabIndex = 5;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(335, 377);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(100, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // textBoxMessage
            // 
            this.textBoxMessage.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxMessage.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxMessage.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxMessage.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxMessage.BorderColorScaling = 0.5F;
            this.textBoxMessage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxMessage.ClearOnFirstChar = false;
            this.textBoxMessage.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxMessage.InErrorCondition = false;
            this.textBoxMessage.Location = new System.Drawing.Point(273, 251);
            this.textBoxMessage.Multiline = false;
            this.textBoxMessage.Name = "textBoxMessage";
            this.textBoxMessage.ReadOnly = false;
            this.textBoxMessage.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxMessage.SelectionLength = 0;
            this.textBoxMessage.SelectionStart = 0;
            this.textBoxMessage.Size = new System.Drawing.Size(186, 20);
            this.textBoxMessage.TabIndex = 3;
            this.textBoxMessage.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxMessage.WordWrap = true;
            this.textBoxMessage.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelMsg
            // 
            this.labelMsg.AutoSize = true;
            this.labelMsg.Location = new System.Drawing.Point(2, 251);
            this.labelMsg.Name = "labelMsg";
            this.labelMsg.Size = new System.Drawing.Size(97, 13);
            this.labelMsg.TabIndex = 0;
            this.labelMsg.Text = "Message to display";
            // 
            // labelWait
            // 
            this.labelWait.AutoSize = true;
            this.labelWait.Location = new System.Drawing.Point(3, 17);
            this.labelWait.Name = "labelWait";
            this.labelWait.Size = new System.Drawing.Size(150, 13);
            this.labelWait.TabIndex = 0;
            this.labelWait.Text = "Wait for this time before action";
            // 
            // textBoxZoomTime
            // 
            this.textBoxZoomTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxZoomTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxZoomTime.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxZoomTime.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxZoomTime.BorderColorScaling = 0.5F;
            this.textBoxZoomTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxZoomTime.ClearOnFirstChar = false;
            this.textBoxZoomTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxZoomTime.InErrorCondition = false;
            this.textBoxZoomTime.Location = new System.Drawing.Point(273, 211);
            this.textBoxZoomTime.Multiline = false;
            this.textBoxZoomTime.Name = "textBoxZoomTime";
            this.textBoxZoomTime.ReadOnly = false;
            this.textBoxZoomTime.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxZoomTime.SelectionLength = 0;
            this.textBoxZoomTime.SelectionStart = 0;
            this.textBoxZoomTime.Size = new System.Drawing.Size(100, 20);
            this.textBoxZoomTime.TabIndex = 2;
            this.textBoxZoomTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxZoomTime.WordWrap = true;
            this.textBoxZoomTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelZoomTime
            // 
            this.labelZoomTime.AutoSize = true;
            this.labelZoomTime.Location = new System.Drawing.Point(3, 211);
            this.labelZoomTime.Name = "labelZoomTime";
            this.labelZoomTime.Size = new System.Drawing.Size(108, 13);
            this.labelZoomTime.TabIndex = 0;
            this.labelZoomTime.Text = "Time to take to Zoom";
            // 
            // labelMsgTime
            // 
            this.labelMsgTime.AutoSize = true;
            this.labelMsgTime.Location = new System.Drawing.Point(3, 286);
            this.labelMsgTime.Name = "labelMsgTime";
            this.labelMsgTime.Size = new System.Drawing.Size(122, 13);
            this.labelMsgTime.TabIndex = 0;
            this.labelMsgTime.Text = "Message on screen time";
            // 
            // textBoxMsgTime
            // 
            this.textBoxMsgTime.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxMsgTime.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxMsgTime.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxMsgTime.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxMsgTime.BorderColorScaling = 0.5F;
            this.textBoxMsgTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxMsgTime.ClearOnFirstChar = false;
            this.textBoxMsgTime.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxMsgTime.InErrorCondition = false;
            this.textBoxMsgTime.Location = new System.Drawing.Point(272, 280);
            this.textBoxMsgTime.Multiline = false;
            this.textBoxMsgTime.Name = "textBoxMsgTime";
            this.textBoxMsgTime.ReadOnly = false;
            this.textBoxMsgTime.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxMsgTime.SelectionLength = 0;
            this.textBoxMsgTime.SelectionStart = 0;
            this.textBoxMsgTime.Size = new System.Drawing.Size(101, 20);
            this.textBoxMsgTime.TabIndex = 3;
            this.textBoxMsgTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxMsgTime.WordWrap = true;
            this.textBoxMsgTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // checkBoxWaitForSlew
            // 
            this.checkBoxWaitForSlew.AutoSize = true;
            this.checkBoxWaitForSlew.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxWaitForSlew.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxWaitForSlew.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxWaitForSlew.FontNerfReduction = 0.5F;
            this.checkBoxWaitForSlew.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxWaitForSlew.Location = new System.Drawing.Point(376, 20);
            this.checkBoxWaitForSlew.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxWaitForSlew.Name = "checkBoxWaitForSlew";
            this.checkBoxWaitForSlew.Size = new System.Drawing.Size(87, 17);
            this.checkBoxWaitForSlew.TabIndex = 7;
            this.checkBoxWaitForSlew.Text = "Wait for slew";
            this.checkBoxWaitForSlew.TickBoxReductionSize = 10;
            this.checkBoxWaitForSlew.UseVisualStyleBackColor = true;
            this.checkBoxWaitForSlew.CheckedChanged += new System.EventHandler(this.checkBoxWaitForSlew_CheckedChanged);
            // 
            // checkBoxPos
            // 
            this.checkBoxPos.AutoSize = true;
            this.checkBoxPos.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxPos.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxPos.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxPos.FontNerfReduction = 0.5F;
            this.checkBoxPos.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxPos.Location = new System.Drawing.Point(379, 83);
            this.checkBoxPos.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxPos.Name = "checkBoxPos";
            this.checkBoxPos.Size = new System.Drawing.Size(56, 17);
            this.checkBoxPos.TabIndex = 7;
            this.checkBoxPos.Text = "Go To";
            this.checkBoxPos.TickBoxReductionSize = 10;
            this.checkBoxPos.UseVisualStyleBackColor = true;
            this.checkBoxPos.CheckedChanged += new System.EventHandler(this.checkBoxGoTo_CheckedChanged);
            // 
            // checkBoxPan
            // 
            this.checkBoxPan.AutoSize = true;
            this.checkBoxPan.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxPan.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxPan.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxPan.FontNerfReduction = 0.5F;
            this.checkBoxPan.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxPan.Location = new System.Drawing.Point(379, 148);
            this.checkBoxPan.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxPan.Name = "checkBoxPan";
            this.checkBoxPan.Size = new System.Drawing.Size(79, 17);
            this.checkBoxPan.TabIndex = 7;
            this.checkBoxPan.Text = "Change Dir";
            this.checkBoxPan.TickBoxReductionSize = 10;
            this.checkBoxPan.UseVisualStyleBackColor = true;
            this.checkBoxPan.CheckedChanged += new System.EventHandler(this.checkBoxChangeDir_CheckedChanged);
            // 
            // checkBoxChangeZoom
            // 
            this.checkBoxChangeZoom.AutoSize = true;
            this.checkBoxChangeZoom.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxChangeZoom.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxChangeZoom.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxChangeZoom.FontNerfReduction = 0.5F;
            this.checkBoxChangeZoom.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxChangeZoom.Location = new System.Drawing.Point(379, 214);
            this.checkBoxChangeZoom.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxChangeZoom.Name = "checkBoxChangeZoom";
            this.checkBoxChangeZoom.Size = new System.Drawing.Size(93, 17);
            this.checkBoxChangeZoom.TabIndex = 7;
            this.checkBoxChangeZoom.Text = "Change Zoom";
            this.checkBoxChangeZoom.TickBoxReductionSize = 10;
            this.checkBoxChangeZoom.UseVisualStyleBackColor = true;
            this.checkBoxChangeZoom.CheckedChanged += new System.EventHandler(this.checkBoxChangeZoom_CheckedChanged);
            // 
            // labelTMEInfo
            // 
            this.labelTMEInfo.AutoSize = true;
            this.labelTMEInfo.Location = new System.Drawing.Point(380, 287);
            this.labelTMEInfo.Name = "labelTMEInfo";
            this.labelTMEInfo.Size = new System.Drawing.Size(73, 13);
            this.labelTMEInfo.TabIndex = 0;
            this.labelTMEInfo.Text = "0=default time";
            // 
            // checkBoxWaitComplete
            // 
            this.checkBoxWaitComplete.AutoSize = true;
            this.checkBoxWaitComplete.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxWaitComplete.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxWaitComplete.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxWaitComplete.FontNerfReduction = 0.5F;
            this.checkBoxWaitComplete.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxWaitComplete.Location = new System.Drawing.Point(272, 316);
            this.checkBoxWaitComplete.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxWaitComplete.Name = "checkBoxWaitComplete";
            this.checkBoxWaitComplete.Size = new System.Drawing.Size(158, 17);
            this.checkBoxWaitComplete.TabIndex = 7;
            this.checkBoxWaitComplete.Text = "Wait for actions to complete";
            this.checkBoxWaitComplete.TickBoxReductionSize = 10;
            this.checkBoxWaitComplete.UseVisualStyleBackColor = true;
            this.checkBoxWaitComplete.CheckedChanged += new System.EventHandler(this.checkBoxWaitForSlew_CheckedChanged);
            // 
            // checkBoxDisplayMessageWhenComplete
            // 
            this.checkBoxDisplayMessageWhenComplete.AutoSize = true;
            this.checkBoxDisplayMessageWhenComplete.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxDisplayMessageWhenComplete.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxDisplayMessageWhenComplete.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxDisplayMessageWhenComplete.FontNerfReduction = 0.5F;
            this.checkBoxDisplayMessageWhenComplete.ImageButtonDisabledScaling = 0.5F;
            this.checkBoxDisplayMessageWhenComplete.Location = new System.Drawing.Point(272, 339);
            this.checkBoxDisplayMessageWhenComplete.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxDisplayMessageWhenComplete.Name = "checkBoxDisplayMessageWhenComplete";
            this.checkBoxDisplayMessageWhenComplete.Size = new System.Drawing.Size(174, 17);
            this.checkBoxDisplayMessageWhenComplete.TabIndex = 7;
            this.checkBoxDisplayMessageWhenComplete.Text = "Display Message with complete";
            this.checkBoxDisplayMessageWhenComplete.TickBoxReductionSize = 10;
            this.checkBoxDisplayMessageWhenComplete.UseVisualStyleBackColor = true;
            this.checkBoxDisplayMessageWhenComplete.CheckedChanged += new System.EventHandler(this.checkBoxWaitForSlew_CheckedChanged);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.panel_close);
            this.panelTop.Controls.Add(this.panel_minimize);
            this.panelTop.Controls.Add(this.label_index);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(571, 32);
            this.panelTop.TabIndex = 32;
            this.panelTop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(548, 0);
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 27;
            this.panel_close.TabStop = false;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panel_minimize
            // 
            this.panel_minimize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_minimize.ImageSelected = ExtendedControls.ExtPanelDrawn.ImageType.Minimize;
            this.panel_minimize.Location = new System.Drawing.Point(518, 0);
            this.panel_minimize.Name = "panel_minimize";
            this.panel_minimize.Padding = new System.Windows.Forms.Padding(6);
            this.panel_minimize.Selectable = false;
            this.panel_minimize.Size = new System.Drawing.Size(24, 24);
            this.panel_minimize.TabIndex = 26;
            this.panel_minimize.TabStop = false;
            this.panel_minimize.Click += new System.EventHandler(this.panel_minimize_Click);
            // 
            // label_index
            // 
            this.label_index.AutoSize = true;
            this.label_index.Location = new System.Drawing.Point(3, 8);
            this.label_index.Name = "label_index";
            this.label_index.Size = new System.Drawing.Size(67, 13);
            this.label_index.TabIndex = 23;
            this.label_index.Text = "Record Step";
            this.label_index.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label_index_MouseDown);
            // 
            // panelMain
            // 
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMain.Controls.Add(this.labelGoto);
            this.panelMain.Controls.Add(this.labelPAt);
            this.panelMain.Controls.Add(this.checkBoxWaitForSlew);
            this.panelMain.Controls.Add(this.checkBoxChangeZoom);
            this.panelMain.Controls.Add(this.textBoxWait);
            this.panelMain.Controls.Add(this.labelZoom);
            this.panelMain.Controls.Add(this.labelWait);
            this.panelMain.Controls.Add(this.checkBoxPan);
            this.panelMain.Controls.Add(this.labelZoomTime);
            this.panelMain.Controls.Add(this.checkBoxPos);
            this.panelMain.Controls.Add(this.labelMsgTime);
            this.panelMain.Controls.Add(this.checkBoxDisplayMessageWhenComplete);
            this.panelMain.Controls.Add(this.labelTimeChange);
            this.panelMain.Controls.Add(this.checkBoxWaitComplete);
            this.panelMain.Controls.Add(this.labelTimeDir);
            this.panelMain.Controls.Add(this.labelTMEInfo);
            this.panelMain.Controls.Add(this.buttonCancel);
            this.panelMain.Controls.Add(this.labelMsg);
            this.panelMain.Controls.Add(this.buttonOK);
            this.panelMain.Controls.Add(this.textBoxPos);
            this.panelMain.Controls.Add(this.textBoxMsgTime);
            this.panelMain.Controls.Add(this.textBoxDir);
            this.panelMain.Controls.Add(this.textBoxMessage);
            this.panelMain.Controls.Add(this.textBoxFlyTime);
            this.panelMain.Controls.Add(this.textBoxZoom);
            this.panelMain.Controls.Add(this.textBoxZoomTime);
            this.panelMain.Controls.Add(this.textBoxPanTime);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 32);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(571, 418);
            this.panelMain.TabIndex = 33;
            // 
            // RecordStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(571, 450);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelTop);
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.Name = "RecordStep";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RecordStep";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelGoto;
        private System.Windows.Forms.Label labelPAt;
        private System.Windows.Forms.Label labelTimeChange;
        private System.Windows.Forms.Label labelTimeDir;
        private System.Windows.Forms.Label labelZoom;
        private ExtendedControls.ExtTextBox textBoxPos;
        private ExtendedControls.ExtTextBox textBoxDir;
        private ExtendedControls.ExtTextBox textBoxZoom;
        private ExtendedControls.ExtTextBox textBoxFlyTime;
        private ExtendedControls.ExtTextBox textBoxPanTime;
        private ExtendedControls.ExtTextBox textBoxWait;
        private ExtendedControls.ExtButton buttonOK;
        private ExtendedControls.ExtButton buttonCancel;
        private ExtendedControls.ExtTextBox textBoxMessage;
        private System.Windows.Forms.Label labelMsg;
        private System.Windows.Forms.Label labelWait;
        private ExtendedControls.ExtTextBox textBoxZoomTime;
        private System.Windows.Forms.Label labelZoomTime;
        private System.Windows.Forms.Label labelMsgTime;
        private ExtendedControls.ExtTextBox textBoxMsgTime;
        private ExtendedControls.ExtCheckBox checkBoxWaitForSlew;
        private ExtendedControls.ExtCheckBox checkBoxPos;
        private ExtendedControls.ExtCheckBox checkBoxPan;
        private ExtendedControls.ExtCheckBox checkBoxChangeZoom;
        private System.Windows.Forms.Label labelTMEInfo;
        private ExtendedControls.ExtCheckBox checkBoxWaitComplete;
        private ExtendedControls.ExtCheckBox checkBoxDisplayMessageWhenComplete;
        private System.Windows.Forms.Panel panelTop;
        private ExtendedControls.ExtPanelDrawn panel_close;
        private ExtendedControls.ExtPanelDrawn panel_minimize;
        private System.Windows.Forms.Label label_index;
        private System.Windows.Forms.Panel panelMain;
    }
}