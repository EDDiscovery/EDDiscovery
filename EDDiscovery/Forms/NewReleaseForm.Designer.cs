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
            this.btnClose = new ExtendedControls.ButtonExt();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlCaption = new System.Windows.Forms.Panel();
            this.pnlMaxRestore = new ExtendedControls.DrawnPanel();
            this.pnlClose = new ExtendedControls.DrawnPanel();
            this.lblCaption = new System.Windows.Forms.Label();
            this.pnlBack = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
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
            this.textBoxReleaseName.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxReleaseName.BorderColorScaling = 0.5F;
            this.textBoxReleaseName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxReleaseName.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxReleaseName.Location = new System.Drawing.Point(99, 9);
            this.textBoxReleaseName.Multiline = false;
            this.textBoxReleaseName.Name = "textBoxReleaseName";
            this.textBoxReleaseName.ReadOnly = true;
            this.textBoxReleaseName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxReleaseName.SelectionLength = 0;
            this.textBoxReleaseName.SelectionStart = 0;
            this.textBoxReleaseName.Size = new System.Drawing.Size(298, 20);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "GitHub URL";
            // 
            // textBoxGitHubURL
            // 
            this.textBoxGitHubURL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGitHubURL.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.textBoxGitHubURL.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.textBoxGitHubURL.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxGitHubURL.BorderColorScaling = 0.5F;
            this.textBoxGitHubURL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxGitHubURL.ControlBackground = System.Drawing.SystemColors.Control;
            this.textBoxGitHubURL.Location = new System.Drawing.Point(99, 38);
            this.textBoxGitHubURL.Multiline = false;
            this.textBoxGitHubURL.Name = "textBoxGitHubURL";
            this.textBoxGitHubURL.ReadOnly = true;
            this.textBoxGitHubURL.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.textBoxGitHubURL.SelectionLength = 0;
            this.textBoxGitHubURL.SelectionStart = 0;
            this.textBoxGitHubURL.Size = new System.Drawing.Size(218, 20);
            this.textBoxGitHubURL.TabIndex = 3;
            this.textBoxGitHubURL.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.textBoxGitHubURL.WordWrap = true;
            // 
            // buttonUrlOpen
            // 
            this.buttonUrlOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonUrlOpen.Location = new System.Drawing.Point(323, 38);
            this.buttonUrlOpen.Name = "buttonUrlOpen";
            this.buttonUrlOpen.Size = new System.Drawing.Size(74, 23);
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
            this.richTextBoxReleaseInfo.Location = new System.Drawing.Point(99, 67);
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
            this.richTextBoxReleaseInfo.Size = new System.Drawing.Size(298, 75);
            this.richTextBoxReleaseInfo.TabIndex = 5;
            this.richTextBoxReleaseInfo.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.richTextBoxReleaseInfo.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Release info";
            // 
            // buttonExeInstaller
            // 
            this.buttonExeInstaller.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExeInstaller.Location = new System.Drawing.Point(100, 148);
            this.buttonExeInstaller.Name = "buttonExeInstaller";
            this.buttonExeInstaller.Size = new System.Drawing.Size(95, 23);
            this.buttonExeInstaller.TabIndex = 7;
            this.buttonExeInstaller.Text = "&Exe Installer";
            this.buttonExeInstaller.UseVisualStyleBackColor = true;
            this.buttonExeInstaller.Click += new System.EventHandler(this.buttonExeInstaller_Click);
            // 
            // buttonPortablezip
            // 
            this.buttonPortablezip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPortablezip.Location = new System.Drawing.Point(302, 148);
            this.buttonPortablezip.Name = "buttonPortablezip";
            this.buttonPortablezip.Size = new System.Drawing.Size(95, 23);
            this.buttonPortablezip.TabIndex = 8;
            this.buttonPortablezip.Text = "&Portable Zip";
            this.buttonPortablezip.UseVisualStyleBackColor = true;
            this.buttonPortablezip.Click += new System.EventHandler(this.buttonPortablezip_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Download";
            // 
            // buttonMsiInstaller
            // 
            this.buttonMsiInstaller.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonMsiInstaller.Location = new System.Drawing.Point(201, 148);
            this.buttonMsiInstaller.Name = "buttonMsiInstaller";
            this.buttonMsiInstaller.Size = new System.Drawing.Size(95, 23);
            this.buttonMsiInstaller.TabIndex = 10;
            this.buttonMsiInstaller.Text = "&Msi Installer";
            this.buttonMsiInstaller.UseVisualStyleBackColor = true;
            this.buttonMsiInstaller.Click += new System.EventHandler(this.buttonMsiInstaller_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(322, 177);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.labelName);
            this.panel1.Controls.Add(this.btnClose);
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
            this.panel1.Location = new System.Drawing.Point(0, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(413, 210);
            this.panel1.TabIndex = 12;
            // 
            // pnlCaption
            // 
            this.pnlCaption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlCaption.Controls.Add(this.pnlMaxRestore);
            this.pnlCaption.Controls.Add(this.pnlClose);
            this.pnlCaption.Controls.Add(this.lblCaption);
            this.pnlCaption.Location = new System.Drawing.Point(0, 0);
            this.pnlCaption.Name = "pnlCaption";
            this.pnlCaption.Size = new System.Drawing.Size(413, 24);
            this.pnlCaption.TabIndex = 12;
            this.pnlCaption.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseDown);
            this.pnlCaption.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Caption_MouseUp);
            // 
            // pnlMaxRestore
            // 
            this.pnlMaxRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlMaxRestore.ImageSelected = ExtendedControls.DrawnPanel.ImageType.Maximize;
            this.pnlMaxRestore.Location = new System.Drawing.Point(359, 0);
            this.pnlMaxRestore.Name = "pnlMaxRestore";
            this.pnlMaxRestore.Selectable = false;
            this.pnlMaxRestore.Size = new System.Drawing.Size(24, 24);
            this.pnlMaxRestore.TabIndex = 2;
            this.pnlMaxRestore.TabStop = false;
            this.pnlMaxRestore.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnlMaxRestore_MouseClick);
            // 
            // pnlClose
            // 
            this.pnlClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlClose.Location = new System.Drawing.Point(389, 0);
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
            this.lblCaption.Location = new System.Drawing.Point(9, 6);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(344, 13);
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
            this.pnlBack.Controls.Add(this.panel1);
            this.pnlBack.Controls.Add(this.pnlCaption);
            this.pnlBack.Location = new System.Drawing.Point(3, 3);
            this.pnlBack.Name = "pnlBack";
            this.pnlBack.Size = new System.Drawing.Size(413, 240);
            this.pnlBack.TabIndex = 13;
            // 
            // NewReleaseForm
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.CaptionHeight = ((uint)(27u));
            this.ClientSize = new System.Drawing.Size(419, 246);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnlCaption.ResumeLayout(false);
            this.pnlBack.ResumeLayout(false);
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
        private ExtendedControls.ButtonExt btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnlCaption;
        private System.Windows.Forms.Label lblCaption;
        private ExtendedControls.DrawnPanel pnlMaxRestore;
        private ExtendedControls.DrawnPanel pnlClose;
        private System.Windows.Forms.Panel pnlBack;
    }
}