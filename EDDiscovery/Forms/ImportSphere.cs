/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
            if (!worked)
                Forms.MessageBoxTheme.Show("Radius in wrong format", "Spehere error");

            return (res == DialogResult.OK && worked);
        }

        public ImportSphere(EDDiscoveryForm discoveryForm)
        {
            InitializeComponent();
            this.discoveryForm = discoveryForm;
            double ly = 10.0;
            txtsphereRadius.Text = ly.ToString("0.00");
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
