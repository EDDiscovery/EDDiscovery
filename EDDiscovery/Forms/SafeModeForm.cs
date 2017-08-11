using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class SafeModeForm : Form
    {
        public Action<bool, bool> Run;
        bool theme = false;
        bool pos = false;

        public SafeModeForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Run(pos,theme);
        }

        private void buttonResetTheme_Click(object sender, EventArgs e)
        {
            theme = !theme;
            buttonResetTheme.Enabled = false;
        }

        private void buttonPositions_Click(object sender, EventArgs e)
        {
            pos = !pos;
            buttonPositions.Enabled = false;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
