using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2.Prospecting
{
    public partial class AddMaterialNodeControl : UserControl
    {
        public AddMaterialNodeControl()
        {
            InitializeComponent();
            PopulateRows();
        }


        private void PopulateRows()
        {
            List<Material> mlist = Material.GetMaterialList;

            mlist.ForEach(c => c.number = 0);  // Set all material numbers to 0.

            dataGridView1.Rows.Clear();

            foreach (Material mat in mlist)
            {
                int rownr;
                string[] row = new string[] { mat.Name, mat.number.ToString(), "+", "-" };

                dataGridView1.Rows.Add(row);
                rownr = dataGridView1.Rows.GetLastRow(DataGridViewElementStates.Visible);
                dataGridView1.Rows[rownr].Tag = mat;

                dataGridView1.Rows[rownr].Cells[0].Style.BackColor = mat.RareityColor;

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex==2) // increase
            {

            }
            if (e.ColumnIndex == 3) // decreas number.
            {
                
            }

        }
    }
}
