using EDDiscovery.DB;
using EDDiscovery2.DB;
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
            comboBoxMaxLy.SelectedIndex = 1;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            double MaxLy = Double.Parse(comboBoxMaxLy.SelectedItem.ToString());

            EDDiscoveryForm edfrm = this.Owner as EDDiscoveryForm;

            HistoryEntry hi =   edfrm.history.First<HistoryEntry>();


            List<ISystem> distlist;
            distlist = SystemClass.GetSystemDistancesFrom(hi.System.x, hi.System.y, hi.System.z, 1000, MaxLy);



        }
    }
}
