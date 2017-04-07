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
using EDDiscovery.HTTP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class NewReleaseForm : Form
    {
        public GitHubRelease release;
        public NewReleaseForm()
        {
            InitializeComponent();
            EDDiscovery.EDDTheme theme = EDDiscovery.EDDTheme.Instance;
            theme.ApplyToForm(this);
        }


        private void NewReleaseForm_Load(object sender, EventArgs e)
        {
            if (release == null)
                return;


            textBoxReleaseName.Text = release.ReleaseName;
            textBoxGitHubURL.Text = release.HtmlURL;
            richTextBoxReleaseInfo.Text = release.Description;

            if (release.ExeInstallerLink == null)
                buttonExeInstaller.Visible = false;

            if (release.PortableInstallerLink == null)
                buttonPortablezip.Visible = false;

            if (release.MsiInstallerLink == null)
                buttonMsiInstaller.Visible = false;

        }


        private void buttonUrlOpen_Click(object sender, EventArgs e)
        {
            Process.Start(release.HtmlURL);
        }

        private void labelName_Click(object sender, EventArgs e)
        {

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

        private void buttonExtCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
