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
using BaseUtils;
using ExtendedControls;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class NewReleaseForm : DraggableForm
    {
        private GitHubRelease release = null;

        public NewReleaseForm(GitHubRelease release)
        {
            this.release = release;
            InitializeComponent();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            release = null;
        }

        protected override void OnLoad(EventArgs e)
        {
            var framed = EDDTheme.Instance?.ApplyToFormStandardFontSize(this) ?? true;
            if (framed)
            {
                // hide the caption panel, and resize the bottom panel to fit.
                var yoff = panelMain.Location.Y - pnlCaption.Location.Y;
                pnlCaption.Visible = false;
            }
            else
            {
                // draw a thin border to serve as a resizing frame.
                pnlBack.BorderStyle = BorderStyle.FixedSingle;
            }

            if (release != null)
            {
                textBoxReleaseName.Text = release.ReleaseName;
                textBoxGitHubURL.Text = release.HtmlURL;
                richTextBoxReleaseInfo.Text = release.Description;

                if (string.IsNullOrEmpty(release.ExeInstallerLink))
                    buttonExeInstaller.Visible = false;

                if (string.IsNullOrEmpty(release.PortableInstallerLink))
                    buttonPortablezip.Visible = false;

                if (string.IsNullOrEmpty(release.MsiInstallerLink))
                    buttonMsiInstaller.Visible = false;
            }

            BaseUtils.Translator.Instance.Translate(this, new Control[] { lblCaption });

            base.OnLoad(e);
        }

        private void buttonUrlOpen_Click(object sender, EventArgs e)
        {
            Process.Start(release.HtmlURL);
        }

        private void buttonExeInstaller_Click(object sender, EventArgs e)
        {
            Process.Start(release.ExeInstallerLink);
        }

        private void buttonPortablezip_Click(object sender, EventArgs e)
        {
            Process.Start(release.PortableInstallerLink);
        }

        private void buttonMsiInstaller_Click(object sender, EventArgs e)
        {
            Process.Start(release.MsiInstallerLink);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #region Caption controls

        private void Caption_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void Caption_MouseUp(object sender, MouseEventArgs e)
        {
            OnCaptionMouseUp((Control)sender, e);
        }

        private void pnlClose_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                Close();
            else if (e.Button == MouseButtons.Right)
                Caption_MouseUp(sender, e);
        }

        #endregion
    }
}
