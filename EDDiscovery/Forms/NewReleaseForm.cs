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
    }
}
