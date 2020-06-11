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
using System;
using System.Diagnostics;
using System.Windows.Forms;
using EDDiscovery.Properties;

namespace EDDiscovery.Forms
{
    public partial class AboutForm : ExtendedControls.DraggableForm
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
            SetTipAndTag(panelLogo, Resources.URLProjectWiki);
            SetTipAndTag(panelEDCD, Resources.URLEDCD);

            var x = Properties.Resources.EDD_License;
            textBoxLicense.Rtf = x;     // we use the RTB to convert from RTF to text, and double space the result. this makes the scroll bar work.
            textBoxLicense.Text = textBoxLicense.Text.LineTextInsersion("","\n","\n");

            System.Diagnostics.Debug.WriteLine("Theme AF");
            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyDialog(this);
            panel_close.Visible = !winborder;

            labelDevelopersEnum.Text = Properties.Resources.Credits;
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            buttonOK.Select();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SetTipAndTag(Control c, string text)
        {
            toolTip1.SetToolTip(c, text);
            c.Tag = text;
        }

        private void link_Click(object sender, EventArgs e)
        {
            Control ctl = (Control)sender;
            if (ctl != null && ctl.Tag != null)
                Process.Start((string)ctl.Tag);
            else
                BaseUtils.TraceLog.WriteLine($"AboutForm: Control and/or Tag is null: control {ctl?.Name ?? "(null)"}, tag {ctl?.Tag ?? "(null)"}.");   
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
