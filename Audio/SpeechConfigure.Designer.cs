/*
 * Copyright © 2017 EDDiscovery development team
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
namespace AudioExtensions
{
    partial class SpeechConfigure
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
            this.Title = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxBorderText = new ExtendedControls.TextBoxBorder();
            this.checkBoxCustomComplete = new ExtendedControls.CheckBoxCustom();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.textBoxBorderEndTrigger = new ExtendedControls.TextBoxBorder();
            this.textBoxBorderStartTrigger = new ExtendedControls.TextBoxBorder();
            this.comboBoxCustomPriority = new ExtendedControls.ComboBoxCustom();
            this.buttonExtTest = new ExtendedControls.ButtonExt();
            this.buttonExtDevice = new ExtendedControls.ButtonExt();
            this.buttonExtEffects = new ExtendedControls.ButtonExt();
            this.trackBarVolume = new System.Windows.Forms.TrackBar();
            this.buttonExtCancel = new ExtendedControls.ButtonExt();
            this.buttonExtOK = new ExtendedControls.ButtonExt();
            this.comboBoxCustomVoice = new ExtendedControls.ComboBoxCustom();
            this.checkBoxCustomLiteral = new ExtendedControls.CheckBoxCustom();
            this.textBoxBorderTest = new ExtendedControls.TextBoxBorder();
            this.labelEndTrigger = new System.Windows.Forms.Label();
            this.labelStartTrigger = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBarRate = new System.Windows.Forms.TrackBar();
            this.checkBoxCustomR = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomV = new ExtendedControls.CheckBoxCustom();
            this.panelOuter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).BeginInit();
            this.SuspendLayout();
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Location = new System.Drawing.Point(12, 12);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(35, 13);
            this.Title.TabIndex = 0;
            this.Title.Text = "label1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 261);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Volume";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 319);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Rate";
            // 
            // textBoxBorderText
            // 
            this.textBoxBorderText.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderText.BorderColorScaling = 0.5F;
            this.textBoxBorderText.Location = new System.Drawing.Point(14, 42);
            this.textBoxBorderText.Multiline = true;
            this.textBoxBorderText.Name = "textBoxBorderText";
            this.textBoxBorderText.Size = new System.Drawing.Size(401, 74);
            this.textBoxBorderText.TabIndex = 0;
            this.textBoxBorderText.WordWrap = false;
            // 
            // checkBoxCustomComplete
            // 
            this.checkBoxCustomComplete.AutoSize = true;
            this.checkBoxCustomComplete.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomComplete.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomComplete.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomComplete.FontNerfReduction = 0.5F;
            this.checkBoxCustomComplete.Location = new System.Drawing.Point(15, 132);
            this.checkBoxCustomComplete.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomComplete.Name = "checkBoxCustomComplete";
            this.checkBoxCustomComplete.Size = new System.Drawing.Size(159, 17);
            this.checkBoxCustomComplete.TabIndex = 1;
            this.checkBoxCustomComplete.Text = "Wait until speech completes";
            this.checkBoxCustomComplete.TickBoxReductionSize = 10;
            this.checkBoxCustomComplete.UseVisualStyleBackColor = true;
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.textBoxBorderEndTrigger);
            this.panelOuter.Controls.Add(this.textBoxBorderStartTrigger);
            this.panelOuter.Controls.Add(this.comboBoxCustomPriority);
            this.panelOuter.Controls.Add(this.buttonExtTest);
            this.panelOuter.Controls.Add(this.buttonExtDevice);
            this.panelOuter.Controls.Add(this.buttonExtEffects);
            this.panelOuter.Controls.Add(this.trackBarVolume);
            this.panelOuter.Controls.Add(this.buttonExtCancel);
            this.panelOuter.Controls.Add(this.buttonExtOK);
            this.panelOuter.Controls.Add(this.comboBoxCustomVoice);
            this.panelOuter.Controls.Add(this.checkBoxCustomLiteral);
            this.panelOuter.Controls.Add(this.checkBoxCustomComplete);
            this.panelOuter.Controls.Add(this.textBoxBorderTest);
            this.panelOuter.Controls.Add(this.textBoxBorderText);
            this.panelOuter.Controls.Add(this.labelEndTrigger);
            this.panelOuter.Controls.Add(this.labelStartTrigger);
            this.panelOuter.Controls.Add(this.label3);
            this.panelOuter.Controls.Add(this.label6);
            this.panelOuter.Controls.Add(this.label1);
            this.panelOuter.Controls.Add(this.trackBarRate);
            this.panelOuter.Controls.Add(this.checkBoxCustomR);
            this.panelOuter.Controls.Add(this.checkBoxCustomV);
            this.panelOuter.Controls.Add(this.Title);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(433, 475);
            this.panelOuter.TabIndex = 0;
            // 
            // textBoxBorderEndTrigger
            // 
            this.textBoxBorderEndTrigger.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderEndTrigger.BorderColorScaling = 0.5F;
            this.textBoxBorderEndTrigger.Location = new System.Drawing.Point(315, 165);
            this.textBoxBorderEndTrigger.Name = "textBoxBorderEndTrigger";
            this.textBoxBorderEndTrigger.Size = new System.Drawing.Size(100, 20);
            this.textBoxBorderEndTrigger.TabIndex = 12;
            // 
            // textBoxBorderStartTrigger
            // 
            this.textBoxBorderStartTrigger.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderStartTrigger.BorderColorScaling = 0.5F;
            this.textBoxBorderStartTrigger.Location = new System.Drawing.Point(83, 165);
            this.textBoxBorderStartTrigger.Name = "textBoxBorderStartTrigger";
            this.textBoxBorderStartTrigger.Size = new System.Drawing.Size(100, 20);
            this.textBoxBorderStartTrigger.TabIndex = 12;
            // 
            // comboBoxCustomPriority
            // 
            this.comboBoxCustomPriority.ArrowWidth = 1;
            this.comboBoxCustomPriority.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomPriority.ButtonColorScaling = 0.5F;
            this.comboBoxCustomPriority.DataSource = null;
            this.comboBoxCustomPriority.DisplayMember = "";
            this.comboBoxCustomPriority.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomPriority.DropDownHeight = 106;
            this.comboBoxCustomPriority.DropDownWidth = 110;
            this.comboBoxCustomPriority.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomPriority.ItemHeight = 13;
            this.comboBoxCustomPriority.Location = new System.Drawing.Point(305, 132);
            this.comboBoxCustomPriority.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomPriority.Name = "comboBoxCustomPriority";
            this.comboBoxCustomPriority.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomPriority.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomPriority.ScrollBarWidth = 16;
            this.comboBoxCustomPriority.SelectedIndex = -1;
            this.comboBoxCustomPriority.SelectedItem = null;
            this.comboBoxCustomPriority.SelectedValue = null;
            this.comboBoxCustomPriority.Size = new System.Drawing.Size(110, 23);
            this.comboBoxCustomPriority.TabIndex = 11;
            this.comboBoxCustomPriority.Text = "comboBoxCustom1";
            this.comboBoxCustomPriority.ValueMember = "";
            // 
            // buttonExtTest
            // 
            this.buttonExtTest.BorderColorScaling = 1.25F;
            this.buttonExtTest.ButtonColorScaling = 0.5F;
            this.buttonExtTest.ButtonDisabledScaling = 0.5F;
            this.buttonExtTest.Location = new System.Drawing.Point(340, 368);
            this.buttonExtTest.Name = "buttonExtTest";
            this.buttonExtTest.Size = new System.Drawing.Size(75, 23);
            this.buttonExtTest.TabIndex = 8;
            this.buttonExtTest.Text = "Test";
            this.buttonExtTest.UseVisualStyleBackColor = true;
            this.buttonExtTest.Click += new System.EventHandler(this.buttonExtTest_Click);
            // 
            // buttonExtDevice
            // 
            this.buttonExtDevice.BorderColorScaling = 1.25F;
            this.buttonExtDevice.ButtonColorScaling = 0.5F;
            this.buttonExtDevice.ButtonDisabledScaling = 0.5F;
            this.buttonExtDevice.Location = new System.Drawing.Point(340, 256);
            this.buttonExtDevice.Name = "buttonExtDevice";
            this.buttonExtDevice.Size = new System.Drawing.Size(75, 23);
            this.buttonExtDevice.TabIndex = 4;
            this.buttonExtDevice.Text = "Device";
            this.buttonExtDevice.UseVisualStyleBackColor = true;
            this.buttonExtDevice.Click += new System.EventHandler(this.buttonExtDevice_Click);
            // 
            // buttonExtEffects
            // 
            this.buttonExtEffects.BorderColorScaling = 1.25F;
            this.buttonExtEffects.ButtonColorScaling = 0.5F;
            this.buttonExtEffects.ButtonDisabledScaling = 0.5F;
            this.buttonExtEffects.Location = new System.Drawing.Point(340, 211);
            this.buttonExtEffects.Name = "buttonExtEffects";
            this.buttonExtEffects.Size = new System.Drawing.Size(75, 23);
            this.buttonExtEffects.TabIndex = 4;
            this.buttonExtEffects.Text = "Effects";
            this.buttonExtEffects.UseVisualStyleBackColor = true;
            this.buttonExtEffects.Click += new System.EventHandler(this.buttonExtEffects_Click);
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Location = new System.Drawing.Point(59, 249);
            this.trackBarVolume.Maximum = 100;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Size = new System.Drawing.Size(203, 45);
            this.trackBarVolume.TabIndex = 5;
            this.trackBarVolume.TickFrequency = 5;
            this.trackBarVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarVolume.Value = 60;
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.BorderColorScaling = 1.25F;
            this.buttonExtCancel.ButtonColorScaling = 0.5F;
            this.buttonExtCancel.ButtonDisabledScaling = 0.5F;
            this.buttonExtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonExtCancel.Location = new System.Drawing.Point(249, 433);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonExtCancel.TabIndex = 9;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            // 
            // buttonExtOK
            // 
            this.buttonExtOK.BorderColorScaling = 1.25F;
            this.buttonExtOK.ButtonColorScaling = 0.5F;
            this.buttonExtOK.ButtonDisabledScaling = 0.5F;
            this.buttonExtOK.Location = new System.Drawing.Point(340, 433);
            this.buttonExtOK.Name = "buttonExtOK";
            this.buttonExtOK.Size = new System.Drawing.Size(75, 23);
            this.buttonExtOK.TabIndex = 10;
            this.buttonExtOK.Text = "OK";
            this.buttonExtOK.UseVisualStyleBackColor = true;
            this.buttonExtOK.Click += new System.EventHandler(this.buttonExtOK_Click);
            // 
            // comboBoxCustomVoice
            // 
            this.comboBoxCustomVoice.ArrowWidth = 1;
            this.comboBoxCustomVoice.BorderColor = System.Drawing.Color.White;
            this.comboBoxCustomVoice.ButtonColorScaling = 0.5F;
            this.comboBoxCustomVoice.DataSource = null;
            this.comboBoxCustomVoice.DisplayMember = "";
            this.comboBoxCustomVoice.DropDownBackgroundColor = System.Drawing.Color.Gray;
            this.comboBoxCustomVoice.DropDownHeight = 106;
            this.comboBoxCustomVoice.DropDownWidth = 203;
            this.comboBoxCustomVoice.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxCustomVoice.ItemHeight = 13;
            this.comboBoxCustomVoice.Location = new System.Drawing.Point(59, 211);
            this.comboBoxCustomVoice.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomVoice.Name = "comboBoxCustomVoice";
            this.comboBoxCustomVoice.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomVoice.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomVoice.ScrollBarWidth = 16;
            this.comboBoxCustomVoice.SelectedIndex = -1;
            this.comboBoxCustomVoice.SelectedItem = null;
            this.comboBoxCustomVoice.SelectedValue = null;
            this.comboBoxCustomVoice.Size = new System.Drawing.Size(203, 23);
            this.comboBoxCustomVoice.TabIndex = 3;
            this.comboBoxCustomVoice.Text = "comboBoxCustom1";
            this.comboBoxCustomVoice.ValueMember = "";
            // 
            // checkBoxCustomLiteral
            // 
            this.checkBoxCustomLiteral.AutoSize = true;
            this.checkBoxCustomLiteral.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomLiteral.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomLiteral.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomLiteral.FontNerfReduction = 0.5F;
            this.checkBoxCustomLiteral.Location = new System.Drawing.Point(197, 132);
            this.checkBoxCustomLiteral.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomLiteral.Name = "checkBoxCustomLiteral";
            this.checkBoxCustomLiteral.Size = new System.Drawing.Size(54, 17);
            this.checkBoxCustomLiteral.TabIndex = 1;
            this.checkBoxCustomLiteral.Text = "Literal";
            this.checkBoxCustomLiteral.TickBoxReductionSize = 10;
            this.checkBoxCustomLiteral.UseVisualStyleBackColor = true;
            // 
            // textBoxBorderTest
            // 
            this.textBoxBorderTest.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderTest.BorderColorScaling = 0.5F;
            this.textBoxBorderTest.Location = new System.Drawing.Point(11, 368);
            this.textBoxBorderTest.Multiline = true;
            this.textBoxBorderTest.Name = "textBoxBorderTest";
            this.textBoxBorderTest.Size = new System.Drawing.Size(313, 38);
            this.textBoxBorderTest.TabIndex = 7;
            // 
            // labelEndTrigger
            // 
            this.labelEndTrigger.AutoSize = true;
            this.labelEndTrigger.Location = new System.Drawing.Point(233, 168);
            this.labelEndTrigger.Name = "labelEndTrigger";
            this.labelEndTrigger.Size = new System.Drawing.Size(62, 13);
            this.labelEndTrigger.TabIndex = 0;
            this.labelEndTrigger.Text = "End Trigger";
            // 
            // labelStartTrigger
            // 
            this.labelStartTrigger.AutoSize = true;
            this.labelStartTrigger.Location = new System.Drawing.Point(12, 168);
            this.labelStartTrigger.Name = "labelStartTrigger";
            this.labelStartTrigger.Size = new System.Drawing.Size(65, 13);
            this.labelStartTrigger.TabIndex = 0;
            this.labelStartTrigger.Text = "Start Trigger";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 214);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Voice";
            // 
            // trackBarRate
            // 
            this.trackBarRate.LargeChange = 2;
            this.trackBarRate.Location = new System.Drawing.Point(59, 306);
            this.trackBarRate.Minimum = -10;
            this.trackBarRate.Name = "trackBarRate";
            this.trackBarRate.Size = new System.Drawing.Size(203, 45);
            this.trackBarRate.TabIndex = 6;
            this.trackBarRate.TickStyle = System.Windows.Forms.TickStyle.Both;
            // 
            // checkBoxCustomR
            // 
            this.checkBoxCustomR.AutoSize = true;
            this.checkBoxCustomR.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomR.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomR.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomR.FontNerfReduction = 0.5F;
            this.checkBoxCustomR.Location = new System.Drawing.Point(271, 319);
            this.checkBoxCustomR.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomR.Name = "checkBoxCustomR";
            this.checkBoxCustomR.Size = new System.Drawing.Size(66, 17);
            this.checkBoxCustomR.TabIndex = 8;
            this.checkBoxCustomR.Text = "Override";
            this.checkBoxCustomR.TickBoxReductionSize = 10;
            this.checkBoxCustomR.UseVisualStyleBackColor = true;
            this.checkBoxCustomR.CheckedChanged += new System.EventHandler(this.checkBoxCustomR_CheckedChanged);
            // 
            // checkBoxCustomV
            // 
            this.checkBoxCustomV.AutoSize = true;
            this.checkBoxCustomV.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomV.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomV.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomV.FontNerfReduction = 0.5F;
            this.checkBoxCustomV.Location = new System.Drawing.Point(271, 261);
            this.checkBoxCustomV.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomV.Name = "checkBoxCustomV";
            this.checkBoxCustomV.Size = new System.Drawing.Size(66, 17);
            this.checkBoxCustomV.TabIndex = 8;
            this.checkBoxCustomV.Text = "Override";
            this.checkBoxCustomV.TickBoxReductionSize = 10;
            this.checkBoxCustomV.UseVisualStyleBackColor = true;
            this.checkBoxCustomV.CheckedChanged += new System.EventHandler(this.checkBoxCustomV_CheckedChanged);
            // 
            // SpeechConfigure
            // 
            this.AcceptButton = this.buttonExtOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonExtCancel;
            this.ClientSize = new System.Drawing.Size(433, 475);
            this.Controls.Add(this.panelOuter);
            this.Name = "SpeechConfigure";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configure Speech";
            this.panelOuter.ResumeLayout(false);
            this.panelOuter.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private ExtendedControls.TextBoxBorder textBoxBorderText;
        private ExtendedControls.CheckBoxCustom checkBoxCustomComplete;
        private System.Windows.Forms.Panel panelOuter;
        private ExtendedControls.ComboBoxCustom comboBoxCustomVoice;
        private System.Windows.Forms.Label label6;
        private ExtendedControls.ButtonExt buttonExtCancel;
        private ExtendedControls.ButtonExt buttonExtOK;
        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.TrackBar trackBarRate;
        private ExtendedControls.ButtonExt buttonExtEffects;
        private ExtendedControls.CheckBoxCustom checkBoxCustomR;
        private ExtendedControls.CheckBoxCustom checkBoxCustomV;
        private ExtendedControls.ButtonExt buttonExtTest;
        private ExtendedControls.TextBoxBorder textBoxBorderTest;
        private ExtendedControls.ComboBoxCustom comboBoxCustomPriority;
        private ExtendedControls.TextBoxBorder textBoxBorderEndTrigger;
        private ExtendedControls.TextBoxBorder textBoxBorderStartTrigger;
        private System.Windows.Forms.Label labelEndTrigger;
        private System.Windows.Forms.Label labelStartTrigger;
        private ExtendedControls.ButtonExt buttonExtDevice;
        private ExtendedControls.CheckBoxCustom checkBoxCustomLiteral;
    }
}