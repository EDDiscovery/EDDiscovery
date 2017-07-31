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

            IDiscoveryController edfrm = this.Owner as IDiscoveryController;

            HistoryEntry hi =   edfrm.history.First<HistoryEntry>();


            List<ISystem> distlist;
            distlist = SystemClassDB.GetSystemDistancesFrom(hi.System.x, hi.System.y, hi.System.z, 1000, MaxLy);



        }
    }
}
