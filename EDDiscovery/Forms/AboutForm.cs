using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2.Forms
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = "Lead developer:" + Environment.NewLine +
            "Cmdr Finwen  (Robert Wahlström )" + Environment.NewLine + Environment.NewLine + 
            "Additional developers:" +Environment.NewLine +
            "Corbin Moran " +Environment.NewLine +
            "Cruento Mucrone" + Environment.NewLine+
            "Robby" + Environment.NewLine+
            "Myshka" + Environment.NewLine+
            "Zed" + Environment.NewLine+
            "Marlon Blake" + Environment.NewLine+
            "Majkl" + Environment.NewLine+
            "Smacker" + Environment.NewLine;

            buttonOK.Select();

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabelForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://forums.frontier.co.uk/showthread.php?t=138155&p=2113535#post2113535");
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery");
        }

        private void linkLabelGitHubIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/EDDiscovery/EDDiscovery/issues");
        }

        private void linkLabelEDSM_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.edsm.net/");
        }

        private void linkLabelEDDB_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://eddb.io/");
        }
    }
}
