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
        WebBrowser webbrowser;

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
            SetTipAndTag(linkLabelYouTube, Resources.URLProjectVideos);
            SetTipAndTag(panelLogo, Resources.URLProjectWiki);
            SetTipAndTag(panelEDCD, Resources.URLEDCD);

#if !MONO
            var x = Properties.Resources.EDD_License;
            textBoxLicense.Rtf = x;     // we use the RTB to convert from RTF to text, and double space the result. this makes the scroll bar work.
            textBoxLicense.Text = textBoxLicense.Text.LineTextInsersion("","\n","\n");
#else
            textBoxLicense.Text = Properties.Resources.EDD_Licence_Mono;
#endif

            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            bool winborder = theme.ApplyDialog(this);
            paneltop.Visible = !winborder;

            extTextBoxDevs.Text = Properties.Resources.Credits;
        }


        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            buttonOK.Focus();

#if !MONO
            webbrowser = new WebBrowser();
            webbrowser.Dock = DockStyle.Fill;
            webbrowser.Visible = false;
            webbrowser.DocumentCompleted += Webbrowser_DocumentCompleted;
            webbrowser.ScriptErrorsSuppressed = true;
            panelWebBrowser.Controls.Add(webbrowser);
            webbrowser.Navigate(Properties.Resources.URLReleaseVideo);
#else
            panelWebBrowser.Visible = false;
            textBoxLicense.Dock = DockStyle.Fill;

#endif
        }

        private void Webbrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            webbrowser.Visible = true;
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
                BaseUtils.BrowserInfo.LaunchBrowser((string)ctl.Tag);
        }

        private void panel_close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
