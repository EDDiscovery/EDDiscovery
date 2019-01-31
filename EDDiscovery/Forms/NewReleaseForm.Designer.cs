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
            this.textBoxReleaseName = new ExtendedControls.ExtTextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.labelURL = new System.Windows.Forms.Label();
            this.textBoxGitHubURL = new ExtendedControls.ExtTextBox();
            this.buttonUrlOpen = new ExtendedControls.ExtButton();
            this.richTextBoxReleaseInfo = new ExtendedControls.ExtRichTextBox();
            this.labelRelease = new System.Windows.Forms.Label();
            this.buttonExeInstaller = new ExtendedControls.ExtButton();
            this.buttonPortablezip = new ExtendedControls.ExtButton();
            this.labelDownload = new System.Windows.Forms.Label();
            this.buttonMsiInstaller = new ExtendedControls.ExtButton();
            this.btnClose = new ExtendedControls.ExtButton();
            this.panelMain = new System.Windows.Forms.Panel();
            this.pnlCaption = new System.Windows.Forms.Panel();
            this.pnlClose = new ExtendedControls.ExtPanelDrawn();
            this.lblCaption = new System.Windows.Forms.Label();
            this.pnlBack = new System.Windows.Forms.Panel();
            this.panelMain.SuspendLayout();
            this.pnlCaption.SuspendLayout();
            this.pnlBack.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxReleaseName
            // 
            this.textBoxReleaseName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReleaseName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxReleaseName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxReleaseName.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxReleaseName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxReleaseName.BorderColorScaling = 0.5F;
            this.textBoxReleaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxReleaseName.ClearOnFirstChar = false;
            this.textBoxReleaseName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxReleaseName.InErrorCondition = false;
            this.textBoxReleaseName.Location = new System.Drawing.Point(146, 9);
            this.textBoxReleaseName.Multiline = false;
            this.textBoxReleaseName.Name = "textBoxReleaseName";
            this.textBoxReleaseName.ReadOnly = true;
            this.textBoxReleaseName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxReleaseName.SelectionLength = 0;
            this.textBoxReleaseName.SelectionStart = 0;
            this.textBoxReleaseName.Size = new System.Drawing.Size(677, 20);
            this.textBoxReleaseName.TabIndex = 0;
            this.textBoxReleaseName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxReleaseName.WordWrap = true;
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(15, 12);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 1;
            this.labelName.Text = "Name";
            // 
            // labelURL
            // 
            this.labelURL.AutoSize = true;
            this.labelURL.Location = new System.Drawing.Point(15, 43);
            this.labelURL.Name = "labelURL";
            this.labelURL.Size = new System.Drawing.Size(65, 13);
            this.labelURL.TabIndex = 2;
            this.labelURL.Text = "GitHub URL";
            // 
            // textBoxGitHubURL
            // 
            this.textBoxGitHubURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGitHubURL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxGitHubURL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxGitHubURL.BackErrorColor = System.Drawing.Color.Red;
            this.textBoxGitHubURL.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxGitHubURL.BorderColorScaling = 0.5F;
            this.textBoxGitHubURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxGitHubURL.ClearOnFirstChar = false;
            this.textBoxGitHubURL.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxGitHubURL.InErrorCondition = false;
            this.textBoxGitHubURL.Location = new System.Drawing.Point(146, 37);
            this.textBoxGitHubURL.Multiline = false;
            this.textBoxGitHubURL.Name = "textBoxGitHubURL";
            this.textBoxGitHubURL.ReadOnly = true;
            this.textBoxGitHubURL.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxGitHubURL.SelectionLength = 0;
            this.textBoxGitHubURL.SelectionStart = 0;
            this.textBoxGitHubURL.Size = new System.Drawing.Size(595, 20);
            this.textBoxGitHubURL.TabIndex = 3;
            this.textBoxGitHubURL.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxGitHubURL.WordWrap = true;
            // 
            // buttonUrlOpen
            // 
            this.buttonUrlOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUrlOpen.Location = new System.Drawing.Point(747, 37);
            this.buttonUrlOpen.Name = "buttonUrlOpen";
            this.buttonUrlOpen.Size = new System.Drawing.Size(100, 23);
            this.buttonUrlOpen.TabIndex = 4;
            this.buttonUrlOpen.Text = "&Open";
            this.buttonUrlOpen.UseVisualStyleBackColor = true;
            this.buttonUrlOpen.Click += new System.EventHandler(this.buttonUrlOpen_Click);
            // 
            // richTextBoxReleaseInfo
            // 
            this.richTextBoxReleaseInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxReleaseInfo.BorderColor = System.Drawing.Color.Transparent;
            this.richTextBoxReleaseInfo.BorderColorScaling = 0.5F;
            this.richTextBoxReleaseInfo.HideScrollBar = true;
            this.richTextBoxReleaseInfo.Location = new System.Drawing.Point(146, 67);
            this.richTextBoxReleaseInfo.Name = "richTextBoxReleaseInfo";
            this.richTextBoxReleaseInfo.ReadOnly = true;
            this.richTextBoxReleaseInfo.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.richTextBoxReleaseInfo.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.richTextBoxReleaseInfo.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxReleaseInfo.ScrollBarBorderColor = System.Drawing.Color.White;
            this.richTextBoxReleaseInfo.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.richTextBoxReleaseInfo.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.richTextBoxReleaseInfo.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.richTextBoxReleaseInfo.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.richTextBoxReleaseInfo.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.richTextBoxReleaseInfo.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.richTextBoxReleaseInfo.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.richTextBoxReleaseInfo.ScrollBarWidth = 20;
            this.richTextBoxReleaseInfo.ShowLineCount = false;
            this.richTextBoxReleaseInfo.Size = new System.Drawing.Size(701, 300);
            this.richTextBoxReleaseInfo.TabIndex = 5;
            this.richTextBoxReleaseInfo.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxReleaseInfo.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // labelRelease
            // 
            this.labelRelease.AutoSize = true;
            this.labelRelease.Location = new System.Drawing.Point(15, 67);
            this.labelRelease.Name = "labelRelease";
            this.labelRelease.Size = new System.Drawing.Size(66, 13);
            this.labelRelease.TabIndex = 6;
            this.labelRelease.Text = "Release info";
            // 
            // buttonExeInstaller
            // 
            this.buttonExeInstaller.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExeInstaller.Location = new System.Drawing.Point(146, 385);
            this.buttonExeInstaller.Name = "buttonExeInstaller";
            this.buttonExeInstaller.Size = new System.Drawing.Size(150, 23);
            this.buttonExeInstaller.TabIndex = 7;
            this.buttonExeInstaller.Text = "&Exe Installer";
            this.buttonExeInstaller.UseVisualStyleBackColor = true;
            this.buttonExeInstaller.Click += new System.EventHandler(this.buttonExeInstaller_Click);
            // 
            // buttonPortablezip
            // 
            this.buttonPortablezip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPortablezip.Location = new System.Drawing.Point(317, 385);
            this.buttonPortablezip.Name = "buttonPortablezip";
            this.buttonPortablezip.Size = new System.Drawing.Size(150, 23);
            this.buttonPortablezip.TabIndex = 8;
            this.buttonPortablezip.Text = "&Portable Zip";
            this.buttonPortablezip.UseVisualStyleBackColor = true;
            this.buttonPortablezip.Click += new System.EventHandler(this.buttonPortablezip_Click);
            // 
            // labelDownload
            // 
            this.labelDownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDownload.AutoSize = true;
            this.labelDownload.Location = new System.Drawing.Point(15, 390);
            this.labelDownload.Name = "labelDownload";
            this.labelDownload.Size = new System.Drawing.Size(55, 13);
            this.labelDownload.TabIndex = 9;
            this.labelDownload.Text = "Download";
            // 
            // buttonMsiInstaller
            // 
            this.buttonMsiInstaller.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMsiInstaller.Location = new System.Drawing.Point(491, 385);
            this.buttonMsiInstaller.Name = "buttonMsiInstaller";
            this.buttonMsiInstaller.Size = new System.Drawing.Size(150, 23);
            this.buttonMsiInstaller.TabIndex = 10;
            this.buttonMsiInstaller.Text = "&Msi Installer";
            this.buttonMsiInstaller.UseVisualStyleBackColor = true;
            this.buttonMsiInstaller.Click += new System.EventHandler(this.buttonMsiInstaller_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(747, 422);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.labelName);
            this.panelMain.Controls.Add(this.btnClose);
            this.panelMain.Controls.Add(this.textBoxReleaseName);
            this.panelMain.Controls.Add(this.buttonMsiInstaller);
            this.panelMain.Controls.Add(this.labelURL);
            this.panelMain.Controls.Add(this.labelDownload);
            this.panelMain.Controls.Add(this.textBoxGitHubURL);
            this.panelMain.Controls.Add(this.buttonPortablezip);
            this.panelMain.Controls.Add(this.buttonUrlOpen);
            this.panelMain.Controls.Add(this.buttonExeInstaller);
            this.panelMain.Controls.Add(this.richTextBoxReleaseInfo);
            this.panelMain.Controls.Add(this.labelRelease);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 32);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(863, 454);
            this.panelMain.TabIndex = 12;
            // 
            // pnlCaption
            // 
            this.pnlCaption.Controls.Add(this.pnlClose);
            this.pnlCaption.Controls.Add(this.lblCaption);
            this.pnlCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCaption.Location = new System.Drawing.Point(0, 0);
            this.pnlCaption.Name = "pnlCaption";
            this.pnlCaption.Size = new System.Drawing.Size(863, 32);
            this.pnlCaption.TabIndex = 12;
            this.pnlCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseDown);
            this.pnlCaption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseUp);
            // 
            // pnlClose
            // 
            this.pnlClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlClose.Location = new System.Drawing.Point(839, 0);
            this.pnlClose.Name = "pnlClose";
            this.pnlClose.Selectable = false;
            this.pnlClose.Size = new System.Drawing.Size(24, 24);
            this.pnlClose.TabIndex = 1;
            this.pnlClose.TabStop = false;
            this.pnlClose.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlClose_MouseClick);
            // 
            // lblCaption
            // 
            this.lblCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCaption.AutoEllipsis = true;
            this.lblCaption.Location = new System.Drawing.Point(3, 3);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(794, 23);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "EDDiscovery Release";
            this.lblCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseDown);
            this.lblCaption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseUp);
            // 
            // pnlBack
            // 
            this.pnlBack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlBack.Controls.Add(this.panelMain);
            this.pnlBack.Controls.Add(this.pnlCaption);
            this.pnlBack.Location = new System.Drawing.Point(3, 3);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Size = new System.Drawing.Size(863, 486);
            this.pnlBack.TabIndex = 13;
            // 
            // NewReleaseForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.CaptionHeight = ((uint)(27u));
            this.ClientSize = new System.Drawing.Size(869, 492);
            this.Controls.Add(this.pnlBack);
            this.DoubleBuffered = true;
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(435, 285);
            this.Name = "NewReleaseForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "EDDiscovery Release";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.pnlCaption.ResumeLayout(false);
            this.pnlBack.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExtendedControls.ExtTextBox textBoxReleaseName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelURL;
        private ExtendedControls.ExtTextBox textBoxGitHubURL;
        private ExtendedControls.ExtButton buttonUrlOpen;
        private ExtendedControls.ExtRichTextBox richTextBoxReleaseInfo;
        private System.Windows.Forms.Label labelRelease;
        private ExtendedControls.ExtButton buttonExeInstaller;
        private ExtendedControls.ExtButton buttonPortablezip;
        private System.Windows.Forms.Label labelDownload;
        private ExtendedControls.ExtButton buttonMsiInstaller;
        private ExtendedControls.ExtButton btnClose;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel pnlCaption;
        private System.Windows.Forms.Label lblCaption;
        private ExtendedControls.ExtPanelDrawn pnlClose;
        private System.Windows.Forms.Panel pnlBack;
    }
}