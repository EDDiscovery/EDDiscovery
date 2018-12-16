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
namespace EDDiscovery.Forms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelDevelopersEnum = new System.Windows.Forms.Label();
            this.buttonOK = new ExtendedControls.ButtonExt();
            this.textBoxLicense = new ExtendedControls.RichTextBoxScroll();
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelNoAffiliation = new System.Windows.Forms.Label();
            this.panelLogo = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel_close = new ExtendedControls.DrawnPanel();
            this.panelLinks.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(12, 19);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(187, 24);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "EDDiscovery v4.3.2.1";
            // 
            // labelDevelopersEnum
            // 
            this.labelDevelopersEnum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDevelopersEnum.BackColor = System.Drawing.Color.Transparent;
            this.labelDevelopersEnum.Location = new System.Drawing.Point(676, 57);
            this.labelDevelopersEnum.Name = "labelDevelopersEnum";
            this.labelDevelopersEnum.Size = new System.Drawing.Size(267, 383);
            this.labelDevelopersEnum.TabIndex = 3;
            this.labelDevelopersEnum.Text = resources.GetString("labelDevelopersEnum.Text");
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(868, 620);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxLicense
            // 
            this.textBoxLicense.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLicense.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxLicense.BorderColorScaling = 0.5F;
            this.textBoxLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLicense.HideScrollBar = true;
            this.textBoxLicense.Location = new System.Drawing.Point(12, 57);
            this.textBoxLicense.Name = "textBoxLicense";
            this.textBoxLicense.ReadOnly = true;
            this.textBoxLicense.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Microsoft S" +
    "ans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs17\\par\r\n}\r\n";
            this.textBoxLicense.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.textBoxLicense.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.textBoxLicense.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.textBoxLicense.ScrollBarBorderColor = System.Drawing.Color.White;
            this.textBoxLicense.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxLicense.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxLicense.ScrollBarLineTweak = 0;
            this.textBoxLicense.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.textBoxLicense.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.textBoxLicense.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.textBoxLicense.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.textBoxLicense.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.textBoxLicense.ScrollBarWidth = 20;
            this.textBoxLicense.ShowLineCount = false;
            this.textBoxLicense.Size = new System.Drawing.Size(644, 380);
            this.textBoxLicense.TabIndex = 1;
            this.textBoxLicense.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.textBoxLicense.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // panelLinks
            // 
            this.panelLinks.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
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
            this.panelLinks.Location = new System.Drawing.Point(676, 443);
            this.panelLinks.Name = "panelLinks";
            this.panelLinks.Size = new System.Drawing.Size(267, 171);
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
            this.linkLabelDeveloperChat.Click += new System.EventHandler(this.link_Click);
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
            this.linkLabelHelp.Click += new System.EventHandler(this.link_Click);
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
            this.linkLabelLicense.Click += new System.EventHandler(this.link_Click);
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
            this.linkLabelFDForum.Click += new System.EventHandler(this.link_Click);
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
            this.linkLabelGitHubIssue.Click += new System.EventHandler(this.link_Click);
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
            this.linkLabelGitHub.Click += new System.EventHandler(this.link_Click);
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
            this.linkLabelEDSM.Click += new System.EventHandler(this.link_Click);
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
            this.linkLabelEDDB.Click += new System.EventHandler(this.link_Click);
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
            this.linkLabelEliteDangerous.Click += new System.EventHandler(this.link_Click);
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
            this.labelDevelopers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDevelopers.AutoSize = true;
            this.labelDevelopers.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDevelopers.Location = new System.Drawing.Point(672, 19);
            this.labelDevelopers.Name = "labelDevelopers";
            this.labelDevelopers.Size = new System.Drawing.Size(106, 24);
            this.labelDevelopers.TabIndex = 7;
            this.labelDevelopers.Text = "Developers";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.labelNoAffiliation, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panelLogo, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 444);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(644, 200);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // labelNoAffiliation
            // 
            this.labelNoAffiliation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelNoAffiliation.Location = new System.Drawing.Point(3, 156);
            this.labelNoAffiliation.Name = "labelNoAffiliation";
            this.labelNoAffiliation.Size = new System.Drawing.Size(698, 44);
            this.labelNoAffiliation.TabIndex = 12;
            this.labelNoAffiliation.Text = "EDDiscovery is not affiliated with Frontier Developments plc.\r\nFree Advanced Inst" +
    "aller License for Open-Source";
            this.labelNoAffiliation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelLogo
            // 
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLogo.Image = global::EDDiscovery.Properties.Resources.Logo;
            this.panelLogo.Location = new System.Drawing.Point(3, 3);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Padding = new System.Windows.Forms.Padding(1);
            this.panelLogo.Size = new System.Drawing.Size(698, 150);
            this.panelLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.panelLogo.TabIndex = 14;
            this.panelLogo.TabStop = false;
            this.panelLogo.Click += new System.EventHandler(this.link_Click);
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.Location = new System.Drawing.Point(936, 1);
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 28;
            this.panel_close.TabStop = false;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(962, 655);
            this.Controls.Add(this.panel_close);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.labelDevelopers);
            this.Controls.Add(this.panelLinks);
            this.Controls.Add(this.textBoxLicense);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelDevelopersEnum);
            this.DoubleBuffered = true;
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 470);
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About EDDiscovery";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.panelLinks.ResumeLayout(false);
            this.panelLinks.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelDevelopersEnum;
        private ExtendedControls.ButtonExt buttonOK;
        internal System.Windows.Forms.Label labelVersion;
        private ExtendedControls.RichTextBoxScroll textBoxLicense;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelNoAffiliation;
        private System.Windows.Forms.PictureBox panelLogo;
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.DrawnPanel panel_close;
    }
}