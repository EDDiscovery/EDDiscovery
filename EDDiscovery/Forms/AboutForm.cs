﻿/*
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery.Properties;

namespace EDDiscovery.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
            labelVersion.Text = EDDApplicationContext.UserAgent;

            SetTipAndTag(linkLabelDeveloperChat, Resources.URLProjectDiscord);
            SetTipAndTag(linkLabelEDDB, Resources.URLeddb);
            SetTipAndTag(linkLabelEDSM, Resources.URLedsm);
            SetTipAndTag(linkLabelEliteDangerous, Resources.URLEDHomepage);
            SetTipAndTag(linkLabelFDForum, Resources.URLProjectEDForumPost);
            SetTipAndTag(linkLabelGitHub, Resources.URLProjectGithub);
            SetTipAndTag(linkLabelGitHubIssue, Resources.URLProjectFeedback);
            SetTipAndTag(linkLabelHelp, Resources.URLProjectWiki);
            SetTipAndTag(linkLabelLicense, Resources.URLProjectLicense);

            panelLogo.Tag = Resources.URLProjectGithub;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            buttonOK.Select();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetTipAndTag(LinkLabel c, string text)
        {
            toolTip1.SetToolTip(c, text);
            c.Tag = text;
        }

        private void link_Click(object sender, EventArgs e)
        {
            Control ctl = (Control)sender;
            if (ctl?.Tag != null)
                Process.Start((string)ctl.Tag);
            else
                Trace.WriteLine($"{nameof(AboutForm)}.{nameof(link_Click)}: control and/or Tag is null: control {ctl?.Name ?? "(null)"}, tag {ctl?.Tag ?? "(null)"}.");   
        }
    }
}
