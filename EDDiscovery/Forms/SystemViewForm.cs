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
        private readonly AutoCompleteStringCollection _systemNames;
        public readonly EDDiscoveryForm _eddiscoveryForm;

        public SystemViewForm()
        {
            InitializeComponent();
        }

        public SystemViewForm(EDDiscoveryForm frm)
        {
            _eddiscoveryForm = frm;
            _systemNames = frm.SystemNames;
            InitializeComponent();
        }


        private void buttonShow_Click(object sender, EventArgs e)
        {
            SystemClass sys = SystemData.GetSystem(textBox_From.Text);
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
                double dist = SystemData.Distance(sys, sys2);

                object[] rowobj = { sys2.name, dist.ToString("0.00"), sys2.government.ToString(), sys2.allegiance.ToString(), sys2.population };
                int rownr;


                    dataGridView1.Rows.Add(rowobj);
                    rownr = dataGridView1.Rows.Count - 1;


                var cell = dataGridView1.Rows[rownr].Cells[1];

                cell.Tag = sys2;
            }
        }

        private void SystemViewForm_Load(object sender, EventArgs e)
        {
            textBox_From.AutoCompleteCustomSource = _systemNames;
        }
    }
}
