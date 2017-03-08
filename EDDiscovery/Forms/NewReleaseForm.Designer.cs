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
    partial class NewReleaseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewReleaseForm));
            this.textBoxReleaseName = new ExtendedControls.TextBoxBorder();
            this.labelName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxGitHubURL = new ExtendedControls.TextBoxBorder();
            this.buttonUrlOpen = new ExtendedControls.ButtonExt();
            this.richTextBoxReleaseInfo = new ExtendedControls.RichTextBoxScroll();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonExeInstaller = new ExtendedControls.ButtonExt();
            this.buttonPortablezip = new ExtendedControls.ButtonExt();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonMsiInstaller = new ExtendedControls.ButtonExt();
            this.buttonExtCancel = new ExtendedControls.ButtonExt();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxReleaseName
            // 
            this.textBoxReleaseName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxReleaseName.BorderColorScaling = 0.5F;
            this.textBoxReleaseName.Location = new System.Drawing.Point(99, 19);
            this.textBoxReleaseName.Name = "textBoxReleaseName";
            this.textBoxReleaseName.ReadOnly = true;
            this.textBoxReleaseName.Size = new System.Drawing.Size(155, 20);
            this.textBoxReleaseName.TabIndex = 0;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(14, 22);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            this.labelName.Click += new System.EventHandler(this.labelName_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "GitHub url";
            // 
            // textBoxGitHubURL
            // 
            this.textBoxGitHubURL.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxGitHubURL.BorderColorScaling = 0.5F;
            this.textBoxGitHubURL.Location = new System.Drawing.Point(101, 52);
            this.textBoxGitHubURL.Name = "textBoxGitHubURL";
            this.textBoxGitHubURL.ReadOnly = true;
            this.textBoxGitHubURL.Size = new System.Drawing.Size(278, 20);
            this.textBoxGitHubURL.TabIndex = 3;
            // 
            // buttonUrlOpen
            // 
            this.buttonUrlOpen.BorderColorScaling = 1.25F;
            this.buttonUrlOpen.ButtonColorScaling = 0.5F;
            this.buttonUrlOpen.ButtonDisabledScaling = 0.5F;
            this.buttonUrlOpen.Location = new System.Drawing.Point(385, 50);
            this.buttonUrlOpen.Name = "buttonUrlOpen";
            this.buttonUrlOpen.Size = new System.Drawing.Size(75, 23);
            this.buttonUrlOpen.TabIndex = 4;
            this.buttonUrlOpen.Text = "Open";
            this.buttonUrlOpen.UseVisualStyleBackColor = true;
            this.buttonUrlOpen.Click += new System.EventHandler(this.buttonUrlOpen_Click);
            // 
            // richTextBoxReleaseInfo
            // 
            this.richTextBoxReleaseInfo.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBoxReleaseInfo.BorderColorScaling = 0.5F;
            this.richTextBoxReleaseInfo.HideScrollBar = true;
            this.richTextBoxReleaseInfo.Location = new System.Drawing.Point(101, 92);
            this.richTextBoxReleaseInfo.Name = "richTextBoxReleaseInfo";
            this.richTextBoxReleaseInfo.ScrollBarWidth = 20;
            this.richTextBoxReleaseInfo.ShowLineCount = false;
            this.richTextBoxReleaseInfo.Size = new System.Drawing.Size(358, 180);
            this.richTextBoxReleaseInfo.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Release info";
            // 
            // buttonExeInstaller
            // 
            this.buttonExeInstaller.BorderColorScaling = 1.25F;
            this.buttonExeInstaller.ButtonColorScaling = 0.5F;
            this.buttonExeInstaller.ButtonDisabledScaling = 0.5F;
            this.buttonExeInstaller.Location = new System.Drawing.Point(101, 292);
            this.buttonExeInstaller.Name = "buttonExeInstaller";
            this.buttonExeInstaller.Size = new System.Drawing.Size(107, 23);
            this.buttonExeInstaller.TabIndex = 7;
            this.buttonExeInstaller.Text = "Exe installer";
            this.buttonExeInstaller.UseVisualStyleBackColor = true;
            this.buttonExeInstaller.Click += new System.EventHandler(this.buttonExeInstaller_Click);
            // 
            // buttonPortablezip
            // 
            this.buttonPortablezip.BorderColorScaling = 1.25F;
            this.buttonPortablezip.ButtonColorScaling = 0.5F;
            this.buttonPortablezip.ButtonDisabledScaling = 0.5F;
            this.buttonPortablezip.Location = new System.Drawing.Point(327, 292);
            this.buttonPortablezip.Name = "buttonPortablezip";
            this.buttonPortablezip.Size = new System.Drawing.Size(132, 23);
            this.buttonPortablezip.TabIndex = 8;
            this.buttonPortablezip.Text = "Portable Zip";
            this.buttonPortablezip.UseVisualStyleBackColor = true;
            this.buttonPortablezip.Click += new System.EventHandler(this.buttonPortablezip_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 297);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Download";
            // 
            // buttonMsiInstaller
            // 
            this.buttonMsiInstaller.BorderColorScaling = 1.25F;
            this.buttonMsiInstaller.ButtonColorScaling = 0.5F;
            this.buttonMsiInstaller.ButtonDisabledScaling = 0.5F;
            this.buttonMsiInstaller.Location = new System.Drawing.Point(214, 292);
            this.buttonMsiInstaller.Name = "buttonMsiInstaller";
            this.buttonMsiInstaller.Size = new System.Drawing.Size(107, 23);
            this.buttonMsiInstaller.TabIndex = 10;
            this.buttonMsiInstaller.Text = "Msi installer";
            this.buttonMsiInstaller.UseVisualStyleBackColor = true;
            this.buttonMsiInstaller.Click += new System.EventHandler(this.buttonMsiInstaller_Click);
            // 
            // buttonExtCancel
            // 
            this.buttonExtCancel.BorderColorScaling = 1.25F;
            this.buttonExtCancel.ButtonColorScaling = 0.5F;
            this.buttonExtCancel.ButtonDisabledScaling = 0.5F;
            this.buttonExtCancel.Location = new System.Drawing.Point(385, 330);
            this.buttonExtCancel.Name = "buttonExtCancel";
            this.buttonExtCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonExtCancel.TabIndex = 11;
            this.buttonExtCancel.Text = "Close";
            this.buttonExtCancel.UseVisualStyleBackColor = true;
            this.buttonExtCancel.Click += new System.EventHandler(this.buttonExtCancel_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.buttonExtCancel);
            this.panel1.Controls.Add(this.textBoxReleaseName);
            this.panel1.Controls.Add(this.buttonMsiInstaller);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.textBoxGitHubURL);
            this.panel1.Controls.Add(this.buttonPortablezip);
            this.panel1.Controls.Add(this.buttonUrlOpen);
            this.panel1.Controls.Add(this.buttonExeInstaller);
            this.panel1.Controls.Add(this.richTextBoxReleaseInfo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(477, 368);
            this.panel1.TabIndex = 12;
            // 
            // NewReleaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(477, 368);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NewReleaseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EDDiscovery release";
            this.Load += new System.EventHandler(this.NewReleaseForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.TextBoxBorder textBoxReleaseName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label1;
        private ExtendedControls.TextBoxBorder textBoxGitHubURL;
        private ExtendedControls.ButtonExt buttonUrlOpen;
        private ExtendedControls.RichTextBoxScroll richTextBoxReleaseInfo;
        private System.Windows.Forms.Label label2;
        private ExtendedControls.ButtonExt buttonExeInstaller;
        private ExtendedControls.ButtonExt buttonPortablezip;
        private System.Windows.Forms.Label label3;
        private ExtendedControls.ButtonExt buttonMsiInstaller;
        private ExtendedControls.ButtonExt buttonExtCancel;
        private System.Windows.Forms.Panel panel1;
    }
}