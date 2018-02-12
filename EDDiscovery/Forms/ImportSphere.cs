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
using EliteDangerousCore;
using EliteDangerousCore.DB;
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
    public partial class ImportSphere : ExtendedControls.DraggableForm
    {
        private EDDiscoveryForm discoveryForm;

        public static Boolean showDialog(EDDiscoveryForm discoveryForm, out string systemName, out double radius, Form owner)
        {
            owner = owner ?? discoveryForm;

            ImportSphere prompt = new ImportSphere(discoveryForm);
            prompt.Icon = owner.Icon;

            EDDTheme.Instance.ApplyToFormStandardFontSize(prompt);

            var res = prompt.ShowDialog(owner);

            systemName =  prompt.txtExportVisited.Text;
            radius = prompt.txtsphereRadius.Value;

            return res == DialogResult.OK;
        }

        public ImportSphere(EDDiscoveryForm discoveryForm)
        {
            InitializeComponent();
            this.discoveryForm = discoveryForm;
            txtsphereRadius.ValidityChanged = ValidityChanged;
            txtExportVisited.SetAutoCompletor(SystemClassDB.ReturnSystemListForAutoComplete);
        }

        private void ValidityChanged(bool v)
        {
            buttonOK.Enabled = v;
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
                txtExportVisited.Text = lastSys.System.Name;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }
    }
}
