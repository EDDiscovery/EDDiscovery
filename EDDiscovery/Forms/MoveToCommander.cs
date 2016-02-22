using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class MoveToCommander : Form
    {
        public EDCommander selectedCommander;

        public MoveToCommander()
        {
            InitializeComponent();
        }


        public bool Init(bool multisystems)
        {
            checkBoxAllInNetlog.Visible = multisystems;
            return true;
        }

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void MoveToCommander_Load(object sender, EventArgs e)
        {
            List<EDCommander>  commanders = EDDiscovery.EDDiscoveryForm.EDDConfig.listCommanders;

            comboBoxCommanders.DataSource = commanders;
            comboBoxCommanders.DisplayMember = "Name";
            comboBoxCommanders.ValueMember = "Nr";

        }

        private void comboBoxCommanders_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedCommander = (EDCommander)comboBoxCommanders.SelectedItem;
        }
    }
}
