using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EDDiscovery2
{
    public partial class AddMaterialNodeControl : UserControl
    {
        public AddMaterialNodeControl()
        {
            InitializeComponent();
            PopulateColumn();
        }


        private void PopulateColumn()
        {
            try
            {

                List<Material> mlist = Material.GetMaterialList;

                mlist.ForEach(c => c.number = 0);  // Set all material numbers to 0.

                dataGridViewPlanet.Rows.Clear();

                foreach (Material mat in mlist)
                {

                    var col = new DataGridViewCheckBoxColumn();
                    col.HeaderText = mat.ShortName;
                    col.Name = mat.ShortName;
                    col.Width = 22;
                    col.DefaultCellStyle.BackColor = mat.RareityColor;

                    dataGridViewPlanet.Columns.AddRange(new DataGridViewColumn[] { col });

                    /*
                    int rownr;
                    rownr = dataGridViewMAterials.Rows.GetLastRow(DataGridViewElementStates.Visible);
                    dataGridViewMAterials.Rows[rownr].Tag = mat;
                    dataGridViewMAterials.Rows[rownr].Cells[0].Style.BackColor = mat.RareityColor;
                    */
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("PopulateRows exception: " + ex.Message);
            }
        }

        private void toolStripButtonAddPlanet_Click(object sender, EventArgs e)
        {
            dataGridViewPlanet.Rows.Add();
        }
    }
}
