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
    public partial class FindMaterialsForm : Form
    {
        public FindMaterialsForm()
        {
            InitializeComponent();
        }

        private void FindMaterialsForm_Load(object sender, EventArgs e)
        {
            comboBoxMaxLy.Items.Add("100");
            comboBoxMaxLy.Items.Add("200");
            comboBoxMaxLy.Items.Add("500");
            comboBoxMaxLy.Items.Add("1000");
            comboBoxMaxLy.Items.Add("2000");

        }
    }
}
