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
    public partial class ImportSphere : Form
    {
        private EDDiscoveryForm discoveryForm;

        public static Boolean showDialog(EDDiscoveryForm discoveryForm, out string systemName, out  double radius)
        {
            ImportSphere prompt = new ImportSphere(discoveryForm);
            var res = prompt.ShowDialog(discoveryForm);
            systemName =  prompt.txtExportVisited.Text;
            bool worked = Double.TryParse(prompt.txtsphereRadius.Text, out radius);
            return (res == DialogResult.OK && worked);
        }

        public ImportSphere(EDDiscoveryForm discoveryForm)
        {
            InitializeComponent();
            this.discoveryForm = discoveryForm;
            txtExportVisited.SetAutoCompletor(EDDiscovery.DB.SystemClass.ReturnSystemListForAutoComplete);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExportTravel_Click(object sender, EventArgs e)
        {
            HistoryEntry lastSys = discoveryForm.history.GetLastFSD ;
            if(lastSys!=null && lastSys.System!=null)
                txtExportVisited.Text = lastSys.System.name;
        }
    }
}
