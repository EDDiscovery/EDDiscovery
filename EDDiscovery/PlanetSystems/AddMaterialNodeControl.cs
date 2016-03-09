using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery2.PlanetSystems;

namespace EDDiscovery2
{
    public partial class AddMaterialNodeControl : UserControl
    {
        string currentSystem;
        EdMaterializer edmat;
        public string CurrentSystem
        {
            get
            {
                return currentSystem;
            }
            set
            {
                dataGridViewPlanet.Rows.Clear();
                currentSystem = value;
                GetData();
            }
        }

        public AddMaterialNodeControl()
        {
            InitializeComponent();
            edmat = new EdMaterializer();
            PopulateColumn();
        }


        private void GetData()
        {
           List<EDWorld> objs = edmat.GetAllWorlds(currentSystem);
        }

        private Dictionary<int, string> dictComboShortNames = new Dictionary<int, string>();
        private void PopulateColumn()
        {
            try
            {

                List<Material> mlist = Material.GetMaterialList;

                mlist.ForEach(c => c.number = 0);  // Set all material numbers to 0.

                dataGridViewPlanet.Rows.Clear();

                dictComboShortNames.Clear();
                //foreach (EDObject obj in EDObject.listObjectTypes)
                //{
                //    int nr = ColumnPlanetType.Items.Add(obj.ShortName);
                //    dictComboShortNames[nr] = obj.ShortName;

                //    //ColumnPlanetType.Items[nr]
                //}

                foreach (Material mat in mlist)
                {

                    var col = new DataGridViewCheckBoxColumn();
                    col.HeaderText = mat.ShortName;
                    col.Name = mat.ShortName;
                    col.ToolTipText = mat.Name;
                    col.Width = 22;
                    col.DefaultCellStyle.BackColor = mat.RareityColor;
                    col.Tag = mat;
                    dataGridViewPlanet.Columns.AddRange(new DataGridViewColumn[] { col });

                }



            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("PopulateRows exception: " + ex.Message);
            }
        }

        private void toolStripButtonAddPlanet_Click(object sender, EventArgs e)
        {
            int nr = dataGridViewPlanet.Rows.Add();

            DataGridViewRow newRow = (DataGridViewRow)dataGridViewPlanet.Rows[nr];

            DataGridViewComboBoxCell cmbCell = (DataGridViewComboBoxCell)newRow.Cells[1];
            cmbCell.Value = dictComboShortNames[0];
            cmbCell = (DataGridViewComboBoxCell)newRow.Cells[3];
            cmbCell.Value = "Unknown"; 

            var edobj = new EDWorld();
            edobj.system = currentSystem;
            dataGridViewPlanet.Rows[nr].Tag = edobj;
           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Save();
        }

        public void Save()
        {
            foreach (DataGridViewRow row in dataGridViewPlanet.Rows)
            {
                EDWorld edobj = (EDWorld)row.Tag;


                edobj.objectName = row.Cells[0].Value.ToString();

                
                foreach (var cell in row.Cells)
                {
                    if (cell is DataGridViewCheckBoxCell)
                    {
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)cell;
                        if (chk.OwningColumn.Tag is Material)
                        {
                            Material mat = (Material)chk.OwningColumn.Tag;
                            bool val = false;

                            if (chk.Value != null && (bool)chk.Value == true)
                                val = true;

                            //edobj.materials[mat.material] = val; 
                            
                        }
                    }
                }
            }
        }

        private void dataGridViewPlanet_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
