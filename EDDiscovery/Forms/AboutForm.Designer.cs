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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
namespace EDDiscovery2.Forms
{
    partial class AboutForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.panelLogo = new System.Windows.Forms.Panel();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelDevelopersEnum = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxLicense = new System.Windows.Forms.TextBox();
            this.labelNoAffiliation = new System.Windows.Forms.Label();
            this.panelLinks = new System.Windows.Forms.Panel();
            this.linkLabelDeveloperChat = new System.Windows.Forms.LinkLabel();
            this.linkLabelHelp = new System.Windows.Forms.LinkLabel();
            this.linkLabelLicense = new System.Windows.Forms.LinkLabel();
            this.linkLabelFDForum = new System.Windows.Forms.LinkLabel();
            this.linkLabelGitHubIssue = new System.Windows.Forms.LinkLabel();
            this.linkLabelGitHub = new System.Windows.Forms.LinkLabel();
            this.linkLabelEDSM = new System.Windows.Forms.LinkLabel();
            this.linkLabelEDDB = new System.Windows.Forms.LinkLabel();
            this.linkLabelEliteDangerous = new System.Windows.Forms.LinkLabel();
            this.labelLinks = new System.Windows.Forms.Label();
            this.labelDevelopers = new System.Windows.Forms.Label();
            this.panelLinks.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLogo
            // 
            this.panelLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLogo.BackColor = System.Drawing.Color.Transparent;
            this.panelLogo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panelLogo.BackgroundImage")));
            this.panelLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panelLogo.Location = new System.Drawing.Point(67, 210);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Size = new System.Drawing.Size(325, 212);
            this.panelLogo.TabIndex = 4;
            this.panelLogo.Click += new System.EventHandler(this.panelLogo_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(12, 19);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(132, 24);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "EDDiscovery v";
            // 
            // labelDevelopersEnum
            // 
            this.labelDevelopersEnum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDevelopersEnum.BackColor = System.Drawing.Color.Transparent;
            this.labelDevelopersEnum.Location = new System.Drawing.Point(453, 57);
            this.labelDevelopersEnum.Name = "labelDevelopersEnum";
            this.labelDevelopersEnum.Size = new System.Drawing.Size(203, 162);
            this.labelDevelopersEnum.TabIndex = 3;
            this.labelDevelopersEnum.Text = resources.GetString("labelDevelopersEnum.Text");
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(581, 399);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxLicense
            // 
            this.textBoxLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLicense.Location = new System.Drawing.Point(12, 57);
            this.textBoxLicense.Multiline = true;
            this.textBoxLicense.Name = "textBoxLicense";
            this.textBoxLicense.ReadOnly = true;
            this.textBoxLicense.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLicense.Size = new System.Drawing.Size(435, 134);
            this.textBoxLicense.TabIndex = 1;
            this.textBoxLicense.Text = resources.GetString("textBoxLicense.Text");
            // 
            // labelNoAffiliation
            // 
            this.labelNoAffiliation.AutoSize = true;
            this.labelNoAffiliation.Location = new System.Drawing.Point(85, 194);
            this.labelNoAffiliation.Name = "labelNoAffiliation";
            this.labelNoAffiliation.Size = new System.Drawing.Size(288, 13);
            this.labelNoAffiliation.TabIndex = 2;
            this.labelNoAffiliation.Text = "EDDiscovery is not affiliated with Fronter Developments plc.";
            // 
            // panelLinks
            // 
            this.panelLinks.Controls.Add(this.linkLabelDeveloperChat);
            this.panelLinks.Controls.Add(this.linkLabelHelp);
            this.panelLinks.Controls.Add(this.linkLabelLicense);
            this.panelLinks.Controls.Add(this.linkLabelFDForum);
            this.panelLinks.Controls.Add(this.linkLabelGitHubIssue);
            this.panelLinks.Controls.Add(this.linkLabelGitHub);
            this.panelLinks.Controls.Add(this.linkLabelEDSM);
            this.panelLinks.Controls.Add(this.linkLabelEDDB);
            this.panelLinks.Controls.Add(this.linkLabelEliteDangerous);
            this.panelLinks.Controls.Add(this.labelLinks);
            this.panelLinks.Location = new System.Drawing.Point(446, 222);
            this.panelLinks.Name = "panelLinks";
            this.panelLinks.Size = new System.Drawing.Size(210, 171);
            this.panelLinks.TabIndex = 5;
            // 
            // linkLabelDeveloperChat
            // 
            this.linkLabelDeveloperChat.AutoSize = true;
            this.linkLabelDeveloperChat.Location = new System.Drawing.Point(7, 41);
            this.linkLabelDeveloperChat.Name = "linkLabelDeveloperChat";
            this.linkLabelDeveloperChat.Size = new System.Drawing.Size(81, 13);
            this.linkLabelDeveloperChat.TabIndex = 2;
            this.linkLabelDeveloperChat.TabStop = true;
            this.linkLabelDeveloperChat.Text = "Developer Chat";
            this.linkLabelDeveloperChat.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDeveloperChat_LinkClicked);
            // 
            // linkLabelHelp
            // 
            this.linkLabelHelp.AutoSize = true;
            this.linkLabelHelp.Location = new System.Drawing.Point(7, 114);
            this.linkLabelHelp.Name = "linkLabelHelp";
            this.linkLabelHelp.Size = new System.Drawing.Size(29, 13);
            this.linkLabelHelp.TabIndex = 7;
            this.linkLabelHelp.TabStop = true;
            this.linkLabelHelp.Text = "Help";
            this.linkLabelHelp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelHelp_LinkClicked);
            // 
            // linkLabelLicense
            // 
            this.linkLabelLicense.AutoSize = true;
            this.linkLabelLicense.Location = new System.Drawing.Point(7, 128);
            this.linkLabelLicense.Margin = new System.Windows.Forms.Padding(45, 1, 3, 1);
            this.linkLabelLicense.Name = "linkLabelLicense";
            this.linkLabelLicense.Size = new System.Drawing.Size(44, 13);
            this.linkLabelLicense.TabIndex = 8;
            this.linkLabelLicense.TabStop = true;
            this.linkLabelLicense.Text = "License";
            this.linkLabelLicense.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelLicense_LinkClicked);
            // 
            // linkLabelFDForum
            // 
            this.linkLabelFDForum.AutoSize = true;
            this.linkLabelFDForum.Location = new System.Drawing.Point(7, 85);
            this.linkLabelFDForum.Margin = new System.Windows.Forms.Padding(45, 1, 3, 1);
            this.linkLabelFDForum.Name = "linkLabelFDForum";
            this.linkLabelFDForum.Size = new System.Drawing.Size(74, 13);
            this.linkLabelFDForum.TabIndex = 5;
            this.linkLabelFDForum.TabStop = true;
            this.linkLabelFDForum.Text = "Frontier Forum";
            this.linkLabelFDForum.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelFDForum_LinkClicked);
            // 
            // linkLabelGitHubIssue
            // 
            this.linkLabelGitHubIssue.AutoSize = true;
            this.linkLabelGitHubIssue.Location = new System.Drawing.Point(7, 143);
            this.linkLabelGitHubIssue.Margin = new System.Windows.Forms.Padding(45, 1, 3, 1);
            this.linkLabelGitHubIssue.Name = "linkLabelGitHubIssue";
            this.linkLabelGitHubIssue.Size = new System.Drawing.Size(90, 13);
            this.linkLabelGitHubIssue.TabIndex = 9;
            this.linkLabelGitHubIssue.TabStop = true;
            this.linkLabelGitHubIssue.Text = "Submit Feedback";
            this.linkLabelGitHubIssue.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGitHubIssue_LinkClicked);
            // 
            // linkLabelGitHub
            // 
            this.linkLabelGitHub.AutoSize = true;
            this.linkLabelGitHub.Location = new System.Drawing.Point(7, 100);
            this.linkLabelGitHub.Margin = new System.Windows.Forms.Padding(45, 1, 3, 1);
            this.linkLabelGitHub.Name = "linkLabelGitHub";
            this.linkLabelGitHub.Size = new System.Drawing.Size(40, 13);
            this.linkLabelGitHub.TabIndex = 6;
            this.linkLabelGitHub.TabStop = true;
            this.linkLabelGitHub.Text = "GitHub";
            this.linkLabelGitHub.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGitHub_LinkClicked);
            // 
            // linkLabelEDSM
            // 
            this.linkLabelEDSM.AutoSize = true;
            this.linkLabelEDSM.Location = new System.Drawing.Point(7, 70);
            this.linkLabelEDSM.Margin = new System.Windows.Forms.Padding(45, 1, 3, 1);
            this.linkLabelEDSM.Name = "linkLabelEDSM";
            this.linkLabelEDSM.Size = new System.Drawing.Size(38, 13);
            this.linkLabelEDSM.TabIndex = 4;
            this.linkLabelEDSM.TabStop = true;
            this.linkLabelEDSM.Text = "EDSM";
            this.linkLabelEDSM.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEDSM_LinkClicked);
            // 
            // linkLabelEDDB
            // 
            this.linkLabelEDDB.AutoSize = true;
            this.linkLabelEDDB.Location = new System.Drawing.Point(7, 55);
            this.linkLabelEDDB.Margin = new System.Windows.Forms.Padding(45, 1, 3, 1);
            this.linkLabelEDDB.Name = "linkLabelEDDB";
            this.linkLabelEDDB.Size = new System.Drawing.Size(37, 13);
            this.linkLabelEDDB.TabIndex = 3;
            this.linkLabelEDDB.TabStop = true;
            this.linkLabelEDDB.Text = "EDDB";
            this.linkLabelEDDB.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEDDB_LinkClicked);
            // 
            // linkLabelEliteDangerous
            // 
            this.linkLabelEliteDangerous.AutoSize = true;
            this.linkLabelEliteDangerous.Location = new System.Drawing.Point(7, 27);
            this.linkLabelEliteDangerous.Margin = new System.Windows.Forms.Padding(45, 1, 3, 1);
            this.linkLabelEliteDangerous.Name = "linkLabelEliteDangerous";
            this.linkLabelEliteDangerous.Size = new System.Drawing.Size(82, 13);
            this.linkLabelEliteDangerous.TabIndex = 1;
            this.linkLabelEliteDangerous.TabStop = true;
            this.linkLabelEliteDangerous.Text = "Elite Dangerous";
            this.linkLabelEliteDangerous.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelEliteDangerous_LinkClicked);
            // 
            // labelLinks
            // 
            this.labelLinks.AutoSize = true;
            this.labelLinks.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLinks.Location = new System.Drawing.Point(6, 1);
            this.labelLinks.Margin = new System.Windows.Forms.Padding(30, 1, 3, 1);
            this.labelLinks.Name = "labelLinks";
            this.labelLinks.Size = new System.Drawing.Size(53, 24);
            this.labelLinks.TabIndex = 0;
            this.labelLinks.Text = "Links";
            // 
            // labelDevelopers
            // 
            this.labelDevelopers.AutoSize = true;
            this.labelDevelopers.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDevelopers.Location = new System.Drawing.Point(452, 19);
            this.labelDevelopers.Name = "labelDevelopers";
            this.labelDevelopers.Size = new System.Drawing.Size(106, 24);
            this.labelDevelopers.TabIndex = 7;
            this.labelDevelopers.Text = "Developers";
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(675, 434);
            this.Controls.Add(this.labelDevelopers);
            this.Controls.Add(this.panelLinks);
            this.Controls.Add(this.labelNoAffiliation);
            this.Controls.Add(this.textBoxLicense);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.panelLogo);
            this.Controls.Add(this.labelDevelopersEnum);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(600, 420);
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About EDDiscovery";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.panelLinks.ResumeLayout(false);
            this.panelLinks.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelLogo;
        private System.Windows.Forms.Label labelDevelopersEnum;
        private System.Windows.Forms.Button buttonOK;
        internal System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.TextBox textBoxLicense;
        private System.Windows.Forms.Label labelNoAffiliation;
        private System.Windows.Forms.Panel panelLinks;
        internal System.Windows.Forms.Label labelLinks;
        private System.Windows.Forms.LinkLabel linkLabelEliteDangerous;
        private System.Windows.Forms.LinkLabel linkLabelDeveloperChat;
        private System.Windows.Forms.LinkLabel linkLabelEDDB;
        private System.Windows.Forms.LinkLabel linkLabelEDSM;
        private System.Windows.Forms.LinkLabel linkLabelFDForum;
        private System.Windows.Forms.LinkLabel linkLabelGitHub;
        private System.Windows.Forms.LinkLabel linkLabelHelp;
        private System.Windows.Forms.LinkLabel linkLabelLicense;
        private System.Windows.Forms.LinkLabel linkLabelGitHubIssue;
        internal System.Windows.Forms.Label labelDevelopers;
    }
}