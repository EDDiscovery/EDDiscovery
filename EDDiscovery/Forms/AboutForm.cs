/*
 * Copyright 2016 - 2025 EDDiscovery development team
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
 */

using EDDiscovery.Properties;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class AboutForm : ExtendedControls.DraggableForm
    {
        //private EDDiscovery.UserControls.Webbrowser.BrowserBase wbb;

        public AboutForm()
        {
            InitializeComponent();
            buttonOK.Text = "OK".TxID(EDTx.OK);
            labelVersion.Text = EDDApplicationContext.UserAgent;

            SetTipAndTag(linkLabelDeveloperChat, Resources.URLProjectDiscord);
            SetTipAndTag(linkLabelEliteDangerous, Resources.URLEDHomepage);
            SetTipAndTag(linkLabelFDForum, Resources.URLProjectEDForumPost);
            SetTipAndTag(linkLabelGitHub, Resources.URLProjectGithub);
            SetTipAndTag(linkLabelGitHubIssue, Resources.URLProjectFeedback);
            SetTipAndTag(linkLabelHelp, Resources.URLProjectWiki);
            SetTipAndTag(linkLabelYouTube, Resources.URLProjectVideos);
            SetTipAndTag(panelLogo, Resources.URLProjectWiki);
            SetTipAndTag(panelEDCD, Resources.URLEDCD);

            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                var x = Properties.Resources.EDD_License;
                textBoxLicense.Rtf = x;     // we use the RTB to convert from RTF to text, and double space the result. this makes the scroll bar work.
                textBoxLicense.Text = textBoxLicense.Text.LineTextInsersion("", "\n", "\n");
            }
            else
            {
                textBoxLicense.Text = Properties.Resources.EDD_Licence_Mono;
            }

            var theme = ExtendedControls.Theme.Current;
            bool winborder = theme.ApplyDialog(this);
            paneltop.Visible = !winborder;

            extTextBoxDevs.Text = Properties.Resources.Credits;
        }

         
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            buttonOK.Focus();

            //if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            //{
            //    if (Properties.Resources.URLReleaseVideo.HasChars())
            //        InitBrowser();
            //}
            //else
            {
                panelWebBrowser.Visible = false;
                textBoxLicense.Dock = DockStyle.Fill;
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            //wbb?.Stop();
            base.OnClosing(e);
        }

        private void Webbrowser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            // Guess that the Youtube button was clicked
            if (Properties.Resources.URLReleaseVideo.StartsWith("https://www.youtube.com/embed/"))
            {
                var uri = new Uri(Properties.Resources.URLReleaseVideo);
                var url = "https://www.youtube.com/watch?v=" + System.Linq.Enumerable.Last(uri.AbsolutePath.Split('/'));

                BaseUtils.BrowserInfo.LaunchBrowser(url);
            }
            else
            {
                BaseUtils.BrowserInfo.LaunchBrowser(Properties.Resources.URLReleaseVideo);
            }
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
