/*
 * Copyright © 2016 EDDiscovery development team
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
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
            //checkBoxAllInNetlog.Visible = multisystems;
            return true;
        }

        private void buttonTransfer_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void MoveToCommander_Load(object sender, EventArgs e)
        {
            List<EDCommander> commanders = EDDiscovery.EDDiscoveryForm.EDDConfig.ListOfCommanders.ToList();

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
