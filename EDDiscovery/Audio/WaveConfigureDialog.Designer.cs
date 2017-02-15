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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
namespace EDDiscovery.Audio
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
            this.checkBoxCustomPreempt = new ExtendedControls.CheckBoxCustom();
            this.panelOuter = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarVolume)).BeginInit();
            this.panelOuter.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxBorderText
            // 
            this.textBoxBorderText.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxBorderText.BorderColorScaling = 0.5F;
            this.textBoxBorderText.Location = new System.Drawing.Point(12, 16);
            this.textBoxBorderText.Multiline = true;
            this.textBoxBorderText.Name = "textBoxBorderText";
            this.textBoxBorderText.Size = new System.Drawing.Size(324, 24);
            this.textBoxBorderText.TabIndex = 0;
            this.textBoxBorderText.WordWrap = false;
            // 
            // buttonExtBrowse
            // 
            this.buttonExtBrowse.BorderColorScaling = 1.25F;
            this.buttonExtBrowse.ButtonColorScaling = 0.5F;
            this.buttonExtBrowse.ButtonDisabledScaling = 0.5F;
            this.buttonExtBrowse.Location = new System.Drawing.Point(368, 17);
            this.buttonExtBrowse.Name = "buttonExtBrowse";
            this.buttonExtBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonExtBrowse.TabIndex = 1;
            this.buttonExtBrowse.Text = "Browse";
            this.buttonExtBrowse.UseVisualStyleBackColor = true;
            this.buttonExtBrowse.Click += new System.EventHandler(this.buttonExtBrowse_Click);
            // 
            // trackBarVolume
            // 
            this.trackBarVolume.Location = new System.Drawing.Point(57, 75);
            this.trackBarVolume.Maximum = 100;
            this.trackBarVolume.Name = "trackBarVolume";
            this.trackBarVolume.Size = new System.Drawing.Size(203, 45);
            this.trackBarVolume.TabIndex = 44;
            this.trackBarVolume.TickFrequency = 5;
            this.trackBarVolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBarVolume.Value = 60;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 91);
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
            this.checkBoxCustomV.Location = new System.Drawing.Point(276, 87);
            this.checkBoxCustomV.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomV.Name = "checkBoxCustomV";
            this.checkBoxCustomV.Size = new System.Drawing.Size(66, 17);
            this.checkBoxCustomV.TabIndex = 46;
            this.checkBoxCustomV.Text = "Override";
            this.checkBoxCustomV.TickBoxReductionSize = 10;
            this.checkBoxCustomV.UseVisualStyleBackColor = true;
            // 
            // checkBoxCustomComplete
            // 
            this.checkBoxCustomComplete.AutoSize = true;
            this.checkBoxCustomComplete.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomComplete.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomComplete.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomComplete.FontNerfReduction = 0.5F;
            this.checkBoxCustomComplete.Location = new System.Drawing.Point(12, 46);
            this.checkBoxCustomComplete.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomComplete.Name = "checkBoxCustomComplete";
            this.checkBoxCustomComplete.Size = new System.Drawing.Size(150, 17);
            this.checkBoxCustomComplete.TabIndex = 48;
            this.checkBoxCustomComplete.Text = "Wait until audio completes";
            this.checkBoxCustomComplete.TickBoxReductionSize = 10;
            this.checkBoxCustomComplete.UseVisualStyleBackColor = true;
            // 
            // buttonExtTest
            // 
            this.buttonExtTest.BorderColorScaling = 1.25F;
            this.buttonExtTest.ButtonColorScaling = 0.5F;
            this.buttonExtTest.ButtonDisabledScaling = 0.5F;
            this.buttonExtTest.Location = new System.Drawing.Point(368, 97);
            this.buttonExtTest.Name = "buttonExtTest";
            this.buttonExtTest.Size = new System.Drawing.Size(75, 23);
            this.buttonExtTest.TabIndex = 42;
            this.buttonExtTest.Text = "Test";
            this.buttonExtTest.UseVisualStyleBackColor = true;
            this.buttonExtTest.Click += new System.EventHandler(this.buttonExtTest_Click);
            // 
            // buttonExtOK
            // 
            this.buttonExtOK.BorderColorScaling = 1.25F;
            this.buttonExtOK.ButtonColorScaling = 0.5F;
            this.buttonExtOK.ButtonDisabledScaling = 0.5F;
            this.buttonExtOK.Location = new System.Drawing.Point(368, 146);
            this.buttonExtOK.Name = "buttonExtOK";
            this.buttonExtOK.Size = new System.Drawing.Size(75, 23);
            this.buttonExtOK.TabIndex = 5;
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
            this.buttonExtCancel.Location = new System.Drawing.Point(276, 146);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonExtCancel.TabIndex = 6;
            this.buttonExtCancel.Text = "Cancel";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            // 
            // buttonExtEffects
            // 
            this.buttonExtEffects.BorderColorScaling = 1.25F;
            this.buttonExtEffects.ButtonColorScaling = 0.5F;
            this.buttonExtEffects.ButtonDisabledScaling = 0.5F;
            this.buttonExtEffects.Location = new System.Drawing.Point(368, 57);
            this.buttonExtEffects.Name = "buttonExtEffects";
            this.buttonExtEffects.Size = new System.Drawing.Size(75, 23);
            this.buttonExtEffects.TabIndex = 42;
            this.buttonExtEffects.Text = "Effects";
            this.buttonExtEffects.UseVisualStyleBackColor = true;
            this.buttonExtEffects.Click += new System.EventHandler(this.buttonExtEffects_Click);
            // 
            // checkBoxCustomPreempt
            // 
            this.checkBoxCustomPreempt.AutoSize = true;
            this.checkBoxCustomPreempt.CheckBoxColor = System.Drawing.Color.Gray;
            this.checkBoxCustomPreempt.CheckBoxInnerColor = System.Drawing.Color.White;
            this.checkBoxCustomPreempt.CheckColor = System.Drawing.Color.DarkBlue;
            this.checkBoxCustomPreempt.FontNerfReduction = 0.5F;
            this.checkBoxCustomPreempt.Location = new System.Drawing.Point(200, 46);
            this.checkBoxCustomPreempt.MouseOverColor = System.Drawing.Color.CornflowerBlue;
            this.checkBoxCustomPreempt.Name = "checkBoxCustomPreempt";
            this.checkBoxCustomPreempt.Size = new System.Drawing.Size(136, 17);
            this.checkBoxCustomPreempt.TabIndex = 49;
            this.checkBoxCustomPreempt.Text = "Preempt all wave audio";
            this.checkBoxCustomPreempt.TickBoxReductionSize = 10;
            this.checkBoxCustomPreempt.UseVisualStyleBackColor = true;
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.trackBarVolume);
            this.panelOuter.Controls.Add(this.textBoxBorderText);
            this.panelOuter.Controls.Add(this.buttonExtBrowse);
            this.panelOuter.Controls.Add(this.buttonExtOK);
            this.panelOuter.Controls.Add(this.buttonExtCancel);
            this.panelOuter.Controls.Add(this.checkBoxCustomComplete);
            this.panelOuter.Controls.Add(this.buttonExtEffects);
            this.panelOuter.Controls.Add(this.buttonExtTest);
            this.panelOuter.Controls.Add(this.checkBoxCustomV);
            this.panelOuter.Controls.Add(this.checkBoxCustomPreempt);
            this.panelOuter.Controls.Add(this.label1);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(455, 181);
            this.panelOuter.TabIndex = 50;
            // 
            // WaveConfigureDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonExtCancel;
            this.ClientSize = new System.Drawing.Size(455, 181);
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
        private ExtendedControls.CheckBoxCustom checkBoxCustomPreempt;
        private System.Windows.Forms.Panel panelOuter;
    }
}