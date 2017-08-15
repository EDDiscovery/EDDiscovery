using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DialogTest
{
    public partial class ChooseTestsForm : Form
    {
        public ChooseTestsForm()
        {
            InitializeComponent();
        }

        private void btnAutoComplete_Click(object sender, EventArgs e)
        {
            new TestAutoComplete().ShowDialog(this);
        }

        private void btnExtendedControls_Click(object sender, EventArgs e)
        {
            new TestForm().ShowDialog(this);
        }

        private void btnRollUp_Click(object sender, EventArgs e)
        {
            new TestRollUpPanel().ShowDialog(this);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
