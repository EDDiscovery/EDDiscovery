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
    partial class WaveConfigureDialog
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
            this.textBoxBorderText = new ExtendedControls.TextBoxBorder();
            this.buttonExtBrowse = new ExtendedControls.ButtonExt();
            this.trackBarVolume = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBoxCustomV = new ExtendedControls.CheckBoxCustom();
            this.checkBoxCustomComplete = new ExtendedControls.CheckBoxCustom();
            this.buttonExtTest = new ExtendedControls.ButtonExt();
            this.buttonExtOK = new ExtendedControls.ButtonExt();
            this.buttonExtCancel = new ExtendedControls.ButtonExt();
            this.buttonExtEffects = new ExtendedControls.ButtonExt();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.buttonExtDevice = new ExtendedControls.ButtonExt();
            this.textBoxBorderStartTrigger = new ExtendedControls.TextBoxBorder();
            this.labelStartTrigger = new System.Windows.Forms.Label();
            this.textBoxBorderEndTrigger = new ExtendedControls.TextBoxBorder();
            this.comboBoxCustomPriority = new ExtendedControls.ComboBoxCustom();
            this.labelEndTrigger = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            this.panelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxBorderText
            // 
            this.textBoxBorderText.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderText.BorderColorScaling = 0.5F;
            this.textBoxBorderText.Location = new System.Drawing.Point(12, 37);
            this.textBoxBorderText.Multiline = true;
            this.textBoxBorderText.Name = "textBoxBorderText";
            this.textBoxBorderText.Size = new System.Drawing.Size(364, 24);
            this.textBoxBorderText.TabIndex = 0;
            this.textBoxBorderText.WordWrap = false;
            // 
            // buttonExtBrowse
            // 
            this.buttonExtBrowse.BorderColorScaling = 1.25F;
            this.buttonExtBrowse.ButtonColorScaling = 0.5F;
            this.buttonExtBrowse.ButtonDisabledScaling = 0.5F;
            this.buttonExtBrowse.Location = new System.Drawing.Point(382, 21);
            this.buttonExtBrowse.Name = "buttonExtBrowse";
            this.buttonExtBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonExtBrowse.TabIndex = 1;
            this.buttonExtBrowse.Text = "Browse";
            this.buttonExtBrowse.UseVisualStyleBackColor = true;
            this.buttonExtBrowse.Click += new System.EventHandler(this.buttonExtBrowse_Click);
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Location = new System.Drawing.Point(57, 147);
            this.trackBarVolume.Maximum = 100;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Size = new System.Drawing.Size(203, 45);
            this.trackBarVolume.TabIndex = 4;
            this.trackBarVolume.TickFrequency = 5;
            this.trackBarVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarVolume.Value = 60;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 163);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Volume";
            // 
            // checkBoxCustomV
            // 
            this.checkBoxCustomV.AutoSize = true;
            this.checkBoxCustomV.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomV.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomV.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomV.FontNerfReduction = 0.5F;
            this.checkBoxCustomV.Location = new System.Drawing.Point(276, 159);
            this.checkBoxCustomV.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomV.Name = "checkBoxCustomV";
            this.checkBoxCustomV.Size = new System.Drawing.Size(66, 17);
            this.checkBoxCustomV.TabIndex = 5;
            this.checkBoxCustomV.Text = "Override";
            this.checkBoxCustomV.TickBoxReductionSize = 10;
            this.checkBoxCustomV.UseVisualStyleBackColor = true;
            this.checkBoxCustomV.CheckedChanged += new System.EventHandler(this.checkBoxCustomV_CheckedChanged);
            // 
            // checkBoxCustomComplete
            // 
            this.checkBoxCustomComplete.AutoSize = true;
            this.checkBoxCustomComplete.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomComplete.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomComplete.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomComplete.FontNerfReduction = 0.5F;
            this.checkBoxCustomComplete.Location = new System.Drawing.Point(12, 82);
            this.checkBoxCustomComplete.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomComplete.Name = "checkBoxCustomComplete";
            this.checkBoxCustomComplete.Size = new System.Drawing.Size(150, 17);
            this.checkBoxCustomComplete.TabIndex = 2;
            this.checkBoxCustomComplete.Text = "Wait until audio completes";
            this.checkBoxCustomComplete.TickBoxReductionSize = 10;
            this.checkBoxCustomComplete.UseVisualStyleBackColor = true;
            // 
            // buttonExtTest
            // 
            this.buttonExtTest.BorderColorScaling = 1.25F;
            this.buttonExtTest.ButtonColorScaling = 0.5F;
            this.buttonExtTest.ButtonDisabledScaling = 0.5F;
            this.buttonExtTest.Location = new System.Drawing.Point(382, 50);
            this.buttonExtTest.Name = "buttonExtTest";
            this.buttonExtTest.Size = new System.Drawing.Size(75, 23);
            this.buttonExtTest.TabIndex = 7;
            this.buttonExtTest.Text = "Test";
            this.buttonExtTest.UseVisualStyleBackColor = true;
            this.buttonExtTest.Click += new System.EventHandler(this.buttonExtTest_Click);
            // 
            // buttonExtOK
            // 
            this.buttonExtOK.BorderColorScaling = 1.25F;
            this.buttonExtOK.ButtonColorScaling = 0.5F;
            this.buttonExtOK.ButtonDisabledScaling = 0.5F;
            this.buttonExtOK.Location = new System.Drawing.Point(381, 198);
            this.buttonExtOK.Name = "buttonExtOK";
            this.buttonExtOK.Size = new System.Drawing.Size(75, 23);
            this.buttonExtOK.TabIndex = 8;
            this.buttonExtOK.Text = "OK";
            this.buttonExtOK.UseVisualStyleBackColor = true;
            this.buttonExtOK.Click += new System.EventHandler(this.buttonExtOK_Click);
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.BorderColorScaling = 1.25F;
            this.buttonExtCancel.ButtonColorScaling = 0.5F;
            this.buttonExtCancel.ButtonDisabledScaling = 0.5F;
            this.buttonExtCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonExtCancel.Location = new System.Drawing.Point(275, 198);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonExtCancel.TabIndex = 9;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            // 
            // buttonExtEffects
            // 
            this.buttonExtEffects.BorderColorScaling = 1.25F;
            this.buttonExtEffects.ButtonColorScaling = 0.5F;
            this.buttonExtEffects.ButtonDisabledScaling = 0.5F;
            this.buttonExtEffects.Location = new System.Drawing.Point(382, 154);
            this.buttonExtEffects.Name = "buttonExtEffects";
            this.buttonExtEffects.Size = new System.Drawing.Size(75, 23);
            this.buttonExtEffects.TabIndex = 6;
            this.buttonExtEffects.Text = "Effects";
            this.buttonExtEffects.UseVisualStyleBackColor = true;
            this.buttonExtEffects.Click += new System.EventHandler(this.buttonExtEffects_Click);
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.buttonExtDevice);
            this.panelOuter.Controls.Add(this.textBoxBorderStartTrigger);
            this.panelOuter.Controls.Add(this.labelStartTrigger);
            this.panelOuter.Controls.Add(this.textBoxBorderEndTrigger);
            this.panelOuter.Controls.Add(this.comboBoxCustomPriority);
            this.panelOuter.Controls.Add(this.labelEndTrigger);
            this.panelOuter.Controls.Add(this.labelTitle);
            this.panelOuter.Controls.Add(this.trackBarVolume);
            this.panelOuter.Controls.Add(this.textBoxBorderText);
            this.panelOuter.Controls.Add(this.buttonExtBrowse);
            this.panelOuter.Controls.Add(this.buttonExtOK);
            this.panelOuter.Controls.Add(this.buttonExtCancel);
            this.panelOuter.Controls.Add(this.checkBoxCustomComplete);
            this.panelOuter.Controls.Add(this.buttonExtEffects);
            this.panelOuter.Controls.Add(this.buttonExtTest);
            this.panelOuter.Controls.Add(this.checkBoxCustomV);
            this.panelOuter.Controls.Add(this.label1);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(475, 247);
            this.panelOuter.TabIndex = 50;
            // 
            // buttonExtDevice
            // 
            this.buttonExtDevice.BorderColorScaling = 1.25F;
            this.buttonExtDevice.ButtonColorScaling = 0.5F;
            this.buttonExtDevice.ButtonDisabledScaling = 0.5F;
            this.buttonExtDevice.Location = new System.Drawing.Point(382, 92);
            this.buttonExtDevice.Name = "buttonExtDevice";
            this.buttonExtDevice.Size = new System.Drawing.Size(75, 23);
            this.buttonExtDevice.TabIndex = 50;
            this.buttonExtDevice.Text = "Device";
            this.buttonExtDevice.UseVisualStyleBackColor = true;
            this.buttonExtDevice.Click += new System.EventHandler(this.buttonExtDevice_Click);
            // 
            // textBoxBorderStartTrigger
            // 
            this.textBoxBorderStartTrigger.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderStartTrigger.BorderColorScaling = 0.5F;
            this.textBoxBorderStartTrigger.Location = new System.Drawing.Point(98, 116);
            this.textBoxBorderStartTrigger.Name = "textBoxBorderStartTrigger";
            this.textBoxBorderStartTrigger.Size = new System.Drawing.Size(100, 20);
            this.textBoxBorderStartTrigger.TabIndex = 49;
            // 
            // labelStartTrigger
            // 
            this.labelStartTrigger.AutoSize = true;
            this.labelStartTrigger.Location = new System.Drawing.Point(12, 119);
            this.labelStartTrigger.Name = "labelStartTrigger";
            this.labelStartTrigger.Size = new System.Drawing.Size(65, 13);
            this.labelStartTrigger.TabIndex = 48;
            this.labelStartTrigger.Text = "Start Trigger";
            // 
            // textBoxBorderEndTrigger
            // 
            this.textBoxBorderEndTrigger.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderEndTrigger.BorderColorScaling = 0.5F;
            this.textBoxBorderEndTrigger.Location = new System.Drawing.Point(357, 116);
            this.textBoxBorderEndTrigger.Name = "textBoxBorderEndTrigger";
            this.textBoxBorderEndTrigger.Size = new System.Drawing.Size(100, 20);
            this.textBoxBorderEndTrigger.TabIndex = 47;
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
            this.comboBoxCustomPriority.Location = new System.Drawing.Point(226, 79);
            this.comboBoxCustomPriority.MouseOverBackgroundColor = System.Drawing.Color.Silver;
            this.comboBoxCustomPriority.Name = "comboBoxCustomPriority";
            this.comboBoxCustomPriority.ScrollBarButtonColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomPriority.ScrollBarColor = System.Drawing.Color.LightGray;
            this.comboBoxCustomPriority.ScrollBarWidth = 16;
            this.comboBoxCustomPriority.SelectedIndex = -1;
            this.comboBoxCustomPriority.SelectedItem = null;
            this.comboBoxCustomPriority.SelectedValue = null;
            this.comboBoxCustomPriority.Size = new System.Drawing.Size(110, 23);
            this.comboBoxCustomPriority.TabIndex = 46;
            this.comboBoxCustomPriority.Text = "comboBoxCustom1";
            this.comboBoxCustomPriority.ValueMember = "";
            // 
            // labelEndTrigger
            // 
            this.labelEndTrigger.AutoSize = true;
            this.labelEndTrigger.Location = new System.Drawing.Point(261, 119);
            this.labelEndTrigger.Name = "labelEndTrigger";
            this.labelEndTrigger.Size = new System.Drawing.Size(62, 13);
            this.labelEndTrigger.TabIndex = 45;
            this.labelEndTrigger.Text = "End Trigger";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(12, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(23, 13);
            this.labelTitle.TabIndex = 44;
            this.labelTitle.Text = "title";
            // 
            // WaveConfigureDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonExtCancel;
            this.ClientSize = new System.Drawing.Size(475, 247);
            this.Controls.Add(this.panelOuter);
            this.Name = "WaveConfigureDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WaveConfigure";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).EndInit();
            this.panelOuter.ResumeLayout(false);
            this.panelOuter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.TextBoxBorder textBoxBorderText;
        private ExtendedControls.ButtonExt buttonExtBrowse;
        private System.Windows.Forms.TrackBar trackBarVolume;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.CheckBoxCustom checkBoxCustomV;
        private ExtendedControls.CheckBoxCustom checkBoxCustomComplete;
        private ExtendedControls.ButtonExt buttonExtTest;
        private ExtendedControls.ButtonExt buttonExtOK;
        private ExtendedControls.ButtonExt buttonExtCancel;
        private ExtendedControls.ButtonExt buttonExtEffects;
        private System.Windows.Forms.Panel panelOuter;
        private System.Windows.Forms.Label labelTitle;
        private ExtendedControls.TextBoxBorder textBoxBorderEndTrigger;
        private ExtendedControls.ComboBoxCustom comboBoxCustomPriority;
        private System.Windows.Forms.Label labelEndTrigger;
        private ExtendedControls.TextBoxBorder textBoxBorderStartTrigger;
        private System.Windows.Forms.Label labelStartTrigger;
        private ExtendedControls.ButtonExt buttonExtDevice;
    }
}