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
            this.buttonOK = new ExtendedControls.ExtButton();
            this.textBoxLicense = new ExtendedControls.ExtRichTextBox();
            this.panelLinksBR = new System.Windows.Forms.Panel();
            this.flowLayoutPanelLabels = new System.Windows.Forms.FlowLayoutPanel();
            this.labelLinks = new System.Windows.Forms.Label();
            this.linkLabelYouTube = new System.Windows.Forms.LinkLabel();
            this.linkLabelDeveloperChat = new System.Windows.Forms.LinkLabel();
            this.linkLabelGitHubIssue = new System.Windows.Forms.LinkLabel();
            this.linkLabelHelp = new System.Windows.Forms.LinkLabel();
            this.linkLabelGitHub = new System.Windows.Forms.LinkLabel();
            this.linkLabelFDForum = new System.Windows.Forms.LinkLabel();
            this.linkLabelEliteDangerous = new System.Windows.Forms.LinkLabel();
            this.labelDevelopers = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel_close = new ExtendedControls.ExtButtonDrawn();
            this.panelLogo = new System.Windows.Forms.PictureBox();
            this.panelEDCD = new System.Windows.Forms.PictureBox();
            this.paneltop = new System.Windows.Forms.Panel();
            this.panelbot = new System.Windows.Forms.Panel();
            this.panelOuter = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelWebBrowser = new System.Windows.Forms.Panel();
            this.panelName = new System.Windows.Forms.Panel();
            this.labelNoAffiliation = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelDevs = new System.Windows.Forms.Panel();
            this.panelDevsSurround = new System.Windows.Forms.Panel();
            this.extTextBoxDevs = new ExtendedControls.ExtTextBox();
            this.panelLinksBR.SuspendLayout();
            this.flowLayoutPanelLabels.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelLogo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEDCD)).BeginInit();
            this.paneltop.SuspendLayout();
            this.panelbot.SuspendLayout();
            this.panelOuter.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelName.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelDevs.SuspendLayout();
            this.panelDevsSurround.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelVersion.Location = new System.Drawing.Point(0, 0);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.labelVersion.Size = new System.Drawing.Size(75, 32);
            this.labelVersion.TabIndex = 0;
            this.labelVersion.Text = "<code>";
            // 
            // buttonOK
            // 
            this.buttonOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOK.Location = new System.Drawing.Point(867, 13);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(100, 24);
            this.buttonOK.TabIndex = 6;
            this.buttonOK.Text = "%OK%";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxLicense
            // 
            this.textBoxLicense.BorderColor = System.Drawing.Color.Transparent;
            this.textBoxLicense.BorderColorScaling = 0.5F;
            this.textBoxLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLicense.DetectUrls = true;
            this.textBoxLicense.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBoxLicense.HideScrollBar = true;
            this.textBoxLicense.Location = new System.Drawing.Point(0, 64);
            this.textBoxLicense.Name = "textBoxLicense";
            this.textBoxLicense.ReadOnly = true;
            this.textBoxLicense.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\nouicompat\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 " +
    "Microsoft Sans Serif;}}\r\n{\\*\\generator Riched20 10.0.19041}\\viewkind4\\uc1 \r\n\\par" +
    "d\\f0\\fs17\\par\r\n}\r\n";
            this.textBoxLicense.ScrollBarArrowBorderColor = System.Drawing.Color.LightBlue;
            this.textBoxLicense.ScrollBarArrowButtonColor = System.Drawing.Color.LightGray;
            this.textBoxLicense.ScrollBarBackColor = System.Drawing.SystemColors.Control;
            this.textBoxLicense.ScrollBarBorderColor = System.Drawing.Color.White;
            this.textBoxLicense.ScrollBarFlatStyle = System.Windows.Forms.FlatStyle.System;
            this.textBoxLicense.ScrollBarForeColor = System.Drawing.SystemColors.ControlText;
            this.textBoxLicense.ScrollBarMouseOverButtonColor = System.Drawing.Color.Green;
            this.textBoxLicense.ScrollBarMousePressedButtonColor = System.Drawing.Color.Red;
            this.textBoxLicense.ScrollBarSliderColor = System.Drawing.Color.DarkGray;
            this.textBoxLicense.ScrollBarThumbBorderColor = System.Drawing.Color.Yellow;
            this.textBoxLicense.ScrollBarThumbButtonColor = System.Drawing.Color.DarkBlue;
            this.textBoxLicense.ShowLineCount = false;
            this.textBoxLicense.Size = new System.Drawing.Size(682, 189);
            this.textBoxLicense.TabIndex = 1;
            this.textBoxLicense.TextBoxBackColor = System.Drawing.SystemColors.Control;
            this.textBoxLicense.TextBoxForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // panelLinksBR
            // 
            this.panelLinksBR.AutoSize = true;
            this.panelLinksBR.Controls.Add(this.flowLayoutPanelLabels);
            this.panelLinksBR.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLinksBR.Location = new System.Drawing.Point(0, 450);
            this.panelLinksBR.Name = "panelLinksBR";
            this.panelLinksBR.Size = new System.Drawing.Size(300, 182);
            this.panelLinksBR.TabIndex = 5;
            // 
            // flowLayoutPanelLabels
            // 
            this.flowLayoutPanelLabels.AutoSize = true;
            this.flowLayoutPanelLabels.Controls.Add(this.labelLinks);
            this.flowLayoutPanelLabels.Controls.Add(this.linkLabelYouTube);
            this.flowLayoutPanelLabels.Controls.Add(this.linkLabelDeveloperChat);
            this.flowLayoutPanelLabels.Controls.Add(this.linkLabelGitHubIssue);
            this.flowLayoutPanelLabels.Controls.Add(this.linkLabelHelp);
            this.flowLayoutPanelLabels.Controls.Add(this.linkLabelGitHub);
            this.flowLayoutPanelLabels.Controls.Add(this.linkLabelFDForum);
            this.flowLayoutPanelLabels.Controls.Add(this.linkLabelEliteDangerous);
            this.flowLayoutPanelLabels.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelLabels.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelLabels.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelLabels.Name = "flowLayoutPanelLabels";
            this.flowLayoutPanelLabels.Padding = new System.Windows.Forms.Padding(1, 10, 1, 10);
            this.flowLayoutPanelLabels.Size = new System.Drawing.Size(300, 182);
            this.flowLayoutPanelLabels.TabIndex = 17;
            // 
            // labelLinks
            // 
            this.labelLinks.AutoSize = true;
            this.labelLinks.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLinks.Location = new System.Drawing.Point(1, 10);
            this.labelLinks.Margin = new System.Windows.Forms.Padding(0);
            this.labelLinks.Name = "labelLinks";
            this.labelLinks.Size = new System.Drawing.Size(46, 20);
            this.labelLinks.TabIndex = 0;
            this.labelLinks.Text = "Links";
            // 
            // linkLabelYouTube
            // 
            this.linkLabelYouTube.AutoSize = true;
            this.linkLabelYouTube.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelYouTube.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelYouTube.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.linkLabelYouTube.Location = new System.Drawing.Point(4, 33);
            this.linkLabelYouTube.Margin = new System.Windows.Forms.Padding(3);
            this.linkLabelYouTube.Name = "linkLabelYouTube";
            this.linkLabelYouTube.Size = new System.Drawing.Size(64, 16);
            this.linkLabelYouTube.TabIndex = 1;
            this.linkLabelYouTube.TabStop = true;
            this.linkLabelYouTube.Text = "YouTube";
            this.linkLabelYouTube.Click += new System.EventHandler(this.link_Click);
            // 
            // linkLabelDeveloperChat
            // 
            this.linkLabelDeveloperChat.AutoSize = true;
            this.linkLabelDeveloperChat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelDeveloperChat.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelDeveloperChat.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.linkLabelDeveloperChat.Location = new System.Drawing.Point(4, 52);
            this.linkLabelDeveloperChat.Name = "linkLabelDeveloperChat";
            this.linkLabelDeveloperChat.Size = new System.Drawing.Size(102, 16);
            this.linkLabelDeveloperChat.TabIndex = 2;
            this.linkLabelDeveloperChat.TabStop = true;
            this.linkLabelDeveloperChat.Text = "Developer Chat";
            this.linkLabelDeveloperChat.Click += new System.EventHandler(this.link_Click);
            // 
            // linkLabelGitHubIssue
            // 
            this.linkLabelGitHubIssue.AutoSize = true;
            this.linkLabelGitHubIssue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelGitHubIssue.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelGitHubIssue.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.linkLabelGitHubIssue.Location = new System.Drawing.Point(4, 71);
            this.linkLabelGitHubIssue.Margin = new System.Windows.Forms.Padding(3);
            this.linkLabelGitHubIssue.Name = "linkLabelGitHubIssue";
            this.linkLabelGitHubIssue.Size = new System.Drawing.Size(114, 16);
            this.linkLabelGitHubIssue.TabIndex = 9;
            this.linkLabelGitHubIssue.TabStop = true;
            this.linkLabelGitHubIssue.Text = "Submit Feedback";
            this.linkLabelGitHubIssue.Click += new System.EventHandler(this.link_Click);
            // 
            // linkLabelHelp
            // 
            this.linkLabelHelp.AutoSize = true;
            this.linkLabelHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelHelp.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelHelp.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.linkLabelHelp.Location = new System.Drawing.Point(4, 90);
            this.linkLabelHelp.Name = "linkLabelHelp";
            this.linkLabelHelp.Size = new System.Drawing.Size(37, 16);
            this.linkLabelHelp.TabIndex = 7;
            this.linkLabelHelp.TabStop = true;
            this.linkLabelHelp.Text = "Help";
            this.linkLabelHelp.Click += new System.EventHandler(this.link_Click);
            // 
            // linkLabelGitHub
            // 
            this.linkLabelGitHub.AutoSize = true;
            this.linkLabelGitHub.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelGitHub.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelGitHub.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.linkLabelGitHub.Location = new System.Drawing.Point(4, 109);
            this.linkLabelGitHub.Margin = new System.Windows.Forms.Padding(3);
            this.linkLabelGitHub.Name = "linkLabelGitHub";
            this.linkLabelGitHub.Size = new System.Drawing.Size(49, 16);
            this.linkLabelGitHub.TabIndex = 6;
            this.linkLabelGitHub.TabStop = true;
            this.linkLabelGitHub.Text = "GitHub";
            this.linkLabelGitHub.Click += new System.EventHandler(this.link_Click);
            // 
            // linkLabelFDForum
            // 
            this.linkLabelFDForum.AutoSize = true;
            this.linkLabelFDForum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelFDForum.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelFDForum.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.linkLabelFDForum.Location = new System.Drawing.Point(4, 131);
            this.linkLabelFDForum.Margin = new System.Windows.Forms.Padding(3);
            this.linkLabelFDForum.Name = "linkLabelFDForum";
            this.linkLabelFDForum.Size = new System.Drawing.Size(94, 16);
            this.linkLabelFDForum.TabIndex = 5;
            this.linkLabelFDForum.TabStop = true;
            this.linkLabelFDForum.Text = "Frontier Forum";
            this.linkLabelFDForum.Click += new System.EventHandler(this.link_Click);
            // 
            // linkLabelEliteDangerous
            // 
            this.linkLabelEliteDangerous.AutoSize = true;
            this.linkLabelEliteDangerous.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelEliteDangerous.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkLabelEliteDangerous.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.linkLabelEliteDangerous.Location = new System.Drawing.Point(4, 153);
            this.linkLabelEliteDangerous.Margin = new System.Windows.Forms.Padding(3);
            this.linkLabelEliteDangerous.Name = "linkLabelEliteDangerous";
            this.linkLabelEliteDangerous.Size = new System.Drawing.Size(104, 16);
            this.linkLabelEliteDangerous.TabIndex = 1;
            this.linkLabelEliteDangerous.TabStop = true;
            this.linkLabelEliteDangerous.Text = "Elite Dangerous";
            this.linkLabelEliteDangerous.Click += new System.EventHandler(this.link_Click);
            // 
            // labelDevelopers
            // 
            this.labelDevelopers.AutoSize = true;
            this.labelDevelopers.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDevelopers.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDevelopers.Location = new System.Drawing.Point(0, 0);
            this.labelDevelopers.Name = "labelDevelopers";
            this.labelDevelopers.Size = new System.Drawing.Size(106, 24);
            this.labelDevelopers.TabIndex = 7;
            this.labelDevelopers.Text = "Developers";
            // 
            // panel_close
            // 
            this.panel_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_close.AutoEllipsis = false;
            this.panel_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel_close.Image = null;
            this.panel_close.ImageSelected = ExtendedControls.ExtButtonDrawn.ImageType.Close;
            this.panel_close.Location = new System.Drawing.Point(955, 3);
            this.panel_close.MouseOverColor = System.Drawing.Color.White;
            this.panel_close.MouseSelectedColor = System.Drawing.Color.Green;
            this.panel_close.MouseSelectedColorEnable = true;
            this.panel_close.Name = "panel_close";
            this.panel_close.Padding = new System.Windows.Forms.Padding(6);
            this.panel_close.Selectable = false;
            this.panel_close.Size = new System.Drawing.Size(24, 24);
            this.panel_close.TabIndex = 28;
            this.panel_close.TabStop = false;
            this.panel_close.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel_close.UseMnemonic = true;
            this.panel_close.Click += new System.EventHandler(this.panel_close_Click);
            // 
            // panelLogo
            // 
            this.panelLogo.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelLogo.Image = global::EDDiscovery.Properties.Resources.Logo;
            this.panelLogo.Location = new System.Drawing.Point(524, 0);
            this.panelLogo.Name = "panelLogo";
            this.panelLogo.Padding = new System.Windows.Forms.Padding(1);
            this.panelLogo.Size = new System.Drawing.Size(158, 64);
            this.panelLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.panelLogo.TabIndex = 15;
            this.panelLogo.TabStop = false;
            this.panelLogo.Click += new System.EventHandler(this.link_Click);
            // 
            // panelEDCD
            // 
            this.panelEDCD.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelEDCD.Image = global::EDDiscovery.Properties.Resources.EDCD;
            this.panelEDCD.Location = new System.Drawing.Point(0, 0);
            this.panelEDCD.Name = "panelEDCD";
            this.panelEDCD.Size = new System.Drawing.Size(87, 49);
            this.panelEDCD.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.panelEDCD.TabIndex = 16;
            this.panelEDCD.TabStop = false;
            this.panelEDCD.Click += new System.EventHandler(this.link_Click);
            // 
            // paneltop
            // 
            this.paneltop.Controls.Add(this.panel_close);
            this.paneltop.Dock = System.Windows.Forms.DockStyle.Top;
            this.paneltop.Location = new System.Drawing.Point(0, 0);
            this.paneltop.Name = "paneltop";
            this.paneltop.Size = new System.Drawing.Size(982, 28);
            this.paneltop.TabIndex = 29;
            // 
            // panelbot
            // 
            this.panelbot.Controls.Add(this.panelEDCD);
            this.panelbot.Controls.Add(this.buttonOK);
            this.panelbot.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelbot.Location = new System.Drawing.Point(0, 660);
            this.panelbot.Name = "panelbot";
            this.panelbot.Size = new System.Drawing.Size(982, 49);
            this.panelbot.TabIndex = 30;
            // 
            // panelOuter
            // 
            this.panelOuter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelOuter.Controls.Add(this.panelContent);
            this.panelOuter.Controls.Add(this.panelbot);
            this.panelOuter.Controls.Add(this.paneltop);
            this.panelOuter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOuter.Location = new System.Drawing.Point(0, 0);
            this.panelOuter.Name = "panelOuter";
            this.panelOuter.Size = new System.Drawing.Size(984, 711);
            this.panelOuter.TabIndex = 2;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.panelLeft);
            this.panelContent.Controls.Add(this.panelRight);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 28);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(982, 632);
            this.panelContent.TabIndex = 31;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.panelWebBrowser);
            this.panelLeft.Controls.Add(this.textBoxLicense);
            this.panelLeft.Controls.Add(this.panelName);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(682, 632);
            this.panelLeft.TabIndex = 2;
            // 
            // panelWebBrowser
            // 
            this.panelWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWebBrowser.Location = new System.Drawing.Point(0, 253);
            this.panelWebBrowser.Name = "panelWebBrowser";
            this.panelWebBrowser.Size = new System.Drawing.Size(682, 379);
            this.panelWebBrowser.TabIndex = 2;
            // 
            // panelName
            // 
            this.panelName.Controls.Add(this.labelNoAffiliation);
            this.panelName.Controls.Add(this.panelLogo);
            this.panelName.Controls.Add(this.labelVersion);
            this.panelName.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelName.Location = new System.Drawing.Point(0, 0);
            this.panelName.Name = "panelName";
            this.panelName.Size = new System.Drawing.Size(682, 64);
            this.panelName.TabIndex = 2;
            // 
            // labelNoAffiliation
            // 
            this.labelNoAffiliation.AutoSize = true;
            this.labelNoAffiliation.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelNoAffiliation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNoAffiliation.Location = new System.Drawing.Point(326, 0);
            this.labelNoAffiliation.Margin = new System.Windows.Forms.Padding(0);
            this.labelNoAffiliation.Name = "labelNoAffiliation";
            this.labelNoAffiliation.Padding = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.labelNoAffiliation.Size = new System.Drawing.Size(198, 44);
            this.labelNoAffiliation.TabIndex = 18;
            this.labelNoAffiliation.Text = "(C) 2015-2024 \r\nRobby && EDDiscovery Team";
            this.labelNoAffiliation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.panelLinksBR);
            this.panelRight.Controls.Add(this.panelDevs);
            this.panelRight.Controls.Add(this.labelDevelopers);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(682, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(300, 632);
            this.panelRight.TabIndex = 0;
            // 
            // panelDevs
            // 
            this.panelDevs.AutoSize = true;
            this.panelDevs.Controls.Add(this.panelDevsSurround);
            this.panelDevs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDevs.Location = new System.Drawing.Point(0, 24);
            this.panelDevs.Name = "panelDevs";
            this.panelDevs.Size = new System.Drawing.Size(300, 608);
            this.panelDevs.TabIndex = 8;
            // 
            // panelDevsSurround
            // 
            this.panelDevsSurround.AutoSize = true;
            this.panelDevsSurround.Controls.Add(this.extTextBoxDevs);
            this.panelDevsSurround.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelDevsSurround.Location = new System.Drawing.Point(0, 0);
            this.panelDevsSurround.Name = "panelDevsSurround";
            this.panelDevsSurround.Padding = new System.Windows.Forms.Padding(5);
            this.panelDevsSurround.Size = new System.Drawing.Size(300, 608);
            this.panelDevsSurround.TabIndex = 5;
            // 
            // extTextBoxDevs
            // 
            this.extTextBoxDevs.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.extTextBoxDevs.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.extTextBoxDevs.BackErrorColor = System.Drawing.Color.Red;
            this.extTextBoxDevs.BorderColor = System.Drawing.Color.Transparent;
            this.extTextBoxDevs.BorderColorScaling = 0.5F;
            this.extTextBoxDevs.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.extTextBoxDevs.ClearOnFirstChar = false;
            this.extTextBoxDevs.ControlBackground = System.Drawing.SystemColors.Control;
            this.extTextBoxDevs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.extTextBoxDevs.EndButtonEnable = true;
            this.extTextBoxDevs.EndButtonImage = ((System.Drawing.Image)(resources.GetObject("extTextBoxDevs.EndButtonImage")));
            this.extTextBoxDevs.EndButtonSize16ths = 10;
            this.extTextBoxDevs.EndButtonVisible = false;
            this.extTextBoxDevs.InErrorCondition = false;
            this.extTextBoxDevs.Location = new System.Drawing.Point(5, 5);
            this.extTextBoxDevs.Multiline = true;
            this.extTextBoxDevs.Name = "extTextBoxDevs";
            this.extTextBoxDevs.ReadOnly = true;
            this.extTextBoxDevs.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.extTextBoxDevs.SelectionLength = 0;
            this.extTextBoxDevs.SelectionStart = 0;
            this.extTextBoxDevs.Size = new System.Drawing.Size(290, 598);
            this.extTextBoxDevs.TabIndex = 4;
            this.extTextBoxDevs.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.extTextBoxDevs.TextNoChange = "";
            this.extTextBoxDevs.WordWrap = true;
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(984, 711);
            this.Controls.Add(this.panelOuter);
            this.DoubleBuffered = true;
            this.Icon = global::EDDiscovery.Properties.Resources.edlogo_3mo_icon;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(650, 470);
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About EDDiscovery";
            this.panelLinksBR.ResumeLayout(false);
            this.panelLinksBR.PerformLayout();
            this.flowLayoutPanelLabels.ResumeLayout(false);
            this.flowLayoutPanelLabels.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelLogo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelEDCD)).EndInit();
            this.paneltop.ResumeLayout(false);
            this.panelbot.ResumeLayout(false);
            this.panelOuter.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelName.ResumeLayout(false);
            this.panelName.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.panelRight.PerformLayout();
            this.panelDevs.ResumeLayout(false);
            this.panelDevs.PerformLayout();
            this.panelDevsSurround.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private ExtendedControls.ExtButton buttonOK;
        internal System.Windows.Forms.Label labelVersion;
        private ExtendedControls.ExtRichTextBox textBoxLicense;
        private System.Windows.Forms.Panel panelLinksBR;
        internal System.Windows.Forms.Label labelLinks;
        private System.Windows.Forms.LinkLabel linkLabelEliteDangerous;
        private System.Windows.Forms.LinkLabel linkLabelDeveloperChat;
        private System.Windows.Forms.LinkLabel linkLabelFDForum;
        private System.Windows.Forms.LinkLabel linkLabelGitHub;
        private System.Windows.Forms.LinkLabel linkLabelHelp;
        private System.Windows.Forms.LinkLabel linkLabelGitHubIssue;
        internal System.Windows.Forms.Label labelDevelopers;
        private System.Windows.Forms.ToolTip toolTip1;
        private ExtendedControls.ExtButtonDrawn panel_close;
        private System.Windows.Forms.PictureBox panelLogo;
        private System.Windows.Forms.Panel paneltop;
        private System.Windows.Forms.Panel panelbot;
        private System.Windows.Forms.PictureBox panelEDCD;
        private System.Windows.Forms.Panel panelOuter;
        private System.Windows.Forms.Panel panelName;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelDevs;
        private ExtendedControls.ExtTextBox extTextBoxDevs;
        private System.Windows.Forms.LinkLabel linkLabelYouTube;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLabels;
        private System.Windows.Forms.Panel panelDevsSurround;
        private System.Windows.Forms.Panel panelWebBrowser;
        private System.Windows.Forms.Label labelNoAffiliation;
    }
}