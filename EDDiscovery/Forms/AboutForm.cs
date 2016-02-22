using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        }
    }
}
