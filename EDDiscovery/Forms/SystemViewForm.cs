﻿/*
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
using EDDiscovery;
using EDDiscovery.DB;
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
    public partial class SystemViewForm : Form
    {
        public readonly EDDiscoveryForm _eddiscoveryForm;

        public SystemViewForm()
        {
            InitializeComponent();
        }

        public SystemViewForm(EDDiscoveryForm frm)
        {
            _eddiscoveryForm = frm;
            InitializeComponent();
        }


        private void buttonShow_Click(object sender, EventArgs e)
        {
            // Class not used  broken for now due to systemdata removal.  would need work, maybe a helper func in systemclass.cs
#if false
            SystemClass sys = SystemClass.GetSystem(textBox_From.Text);
            if (sys == null) return;
            
            var syslist  = (from c in SystemData.SystemList orderby (c.x-sys.x)* (c.x - sys.x) + (c.y - sys.y) * (c.y - sys.y) + (c.z - sys.z) * (c.z - sys.z) select c).ToList<SystemClass>();

            dataGridView1.Rows.Clear();

            dataGridView1.Columns.Clear();
            dataGridView1.Columns.Add("Name", "Name");
            dataGridView1.Columns.Add("Dist", "Dist");
            dataGridView1.Columns.Add("Government", "Government");
            dataGridView1.Columns.Add("Government", "Allegiance");
            dataGridView1.Columns.Add("Government", "Population");


            foreach (SystemClass sys2 in syslist)
            {
                double dist = SystemClass.Distance(sys, sys2);

                object[] rowobj = { sys2.name, dist.ToString("0.00"), sys2.government.ToString(), sys2.allegiance.ToString(), sys2.population };
                int rownr;


                    dataGridView1.Rows.Add(rowobj);
                    rownr = dataGridView1.Rows.Count - 1;


                var cell = dataGridView1.Rows[rownr].Cells[1];

                cell.Tag = sys2;
            }
#endif
        }

        private void SystemViewForm_Load(object sender, EventArgs e)
        {
        }
    }
}
