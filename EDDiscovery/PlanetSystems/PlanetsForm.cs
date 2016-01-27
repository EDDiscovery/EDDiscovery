using EDDiscovery;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EDDiscovery2.DB;

namespace EDDiscovery2.PlanetSystems
{
    public partial class PlanetsForm : Form
    {
        public EDDiscovery.EDDiscoveryForm edForm;
        private List<EDObject> edObjects = new List<EDObject>();
        private EdMaterializer edmat = new EdMaterializer();
        private Dictionary<int, string> dictComboDesc = new Dictionary<int, string>();

        private int CurrentItem = 0;

        private System.Windows.Forms.ColumnHeader columnName;
        private System.Windows.Forms.ColumnHeader columnType;
        private System.Windows.Forms.ColumnHeader columnGravity;
        private System.Windows.Forms.ColumnHeader columnArrivalPoint;
        private System.Windows.Forms.ColumnHeader[] ColumnMats;

        private List<Material> mlist;

        public PlanetsForm()
        {
            InitializeComponent();
        }


        public void InitForm(EDDiscoveryForm frm)
        {
            edForm = frm;
        }

        private void textBoxSystemName_TextChanged(object sender, EventArgs e)
        {

        }

        private void PlanetsForm_Load(object sender, EventArgs e)
        {
            this.columnName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnGravity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnArrivalPoint = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));

            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnName,
            this.columnType, columnGravity, columnArrivalPoint });
            // 
            // columnName
            // 
            this.columnName.Text = "Name";
            this.columnName.Width = 80;
            // 
            // columnData
            // 
            this.columnType.Text = "Data";
            this.columnType.Width = 150;

            columnGravity.Text = "G";
            columnGravity.Width = 40;

            columnArrivalPoint.Text = "Dist";
            columnArrivalPoint.Width = 40;


            mlist = Material.GetMaterialList;

            ColumnMats = new ColumnHeader[mlist.Count];
            for (int ii = 0; ii < mlist.Count; ii++)
            {
                ColumnHeader col = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
                ColumnMats[ii] = col;
                col.Name = mlist[ii].Name;
                col.Text = mlist[ii].ShortName;
                col.Tag = mlist[ii];
                col.Width = 25;
                listView1.Columns.Add(col);
            }





            dictComboDesc.Clear();
            foreach (EDObject obj in EDObject.listObjectTypes)
            {
                if (obj.IsPlanet)
                {
                    int nr = comboBoxType.Items.Add(obj.Description);
                    dictComboDesc[nr] = obj.Description;
                }
            }


            foreach (VulcanismEnum vulc in Enum.GetValues(typeof(VulcanismEnum)))
            {
                comboBoxVulcanism.Items.Add(vulc.ToString().Replace("_", " "));
            }
            foreach (AtmosphereEnum vulc in Enum.GetValues(typeof(AtmosphereEnum)))
            {
                comboBoxAtmosphere.Items.Add(vulc.ToString().Replace("_", " "));
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetCurrentSystem();

      //      ObjectsType objt = new ObjectsType();

//            objt.ParseJson();
        }

        private void SetCurrentSystem()
        {
            //SetSystem(edForm.TravelControl.CurrentSystem);
            ISystem sys = SystemData.GetSystem("SHINRARTA DEZHRA");
            SetSystem(sys);
        }

        private void SetSystem(ISystem currentSystem)
        {
            if (currentSystem == null)
                return;

            textBoxSystemName.Text = currentSystem.name;

            edObjects.Clear();

            edObjects = edmat.GetAll(textBoxSystemName.Text);
            UpDateListView();
            //addMaterialNodeControl1.CurrentSystem = currentSystem.name;
        }


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            edObjects.Add(new EDObject());
            CurrentItem = edObjects.Count - 1;
            UpDateListView();

        }

        private void UpDateListView()
        {
            listView1.Items.Clear();

            foreach (var obj in edObjects)
            {
                ListViewItem lvi;
                lvi = listView1.Items.Add(obj.objectName);
                lvi.SubItems.Add(obj.Description);

                lvi.SubItems.Add(obj.gravity.ToString("0.00"));
                lvi.SubItems.Add(obj.arrivalPoint.ToString("0"));

                lvi.UseItemStyleForSubItems = false;


                for (int ii = 0; ii < mlist.Count; ii++)
                {
                    ListViewItem.ListViewSubItem lvsi;
                    if (obj.materials[mlist[ii].material])
                        lvsi = lvi.SubItems.Add("X");
                    else
                        lvsi =  lvi.SubItems.Add(" ");

                    lvsi.BackColor = mlist[ii].RareityColor;
                }

                lvi.Tag = obj;
            }
            listView1.Items[0].Selected = true;
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            //Just Greg testing stuff. Go ahead and delete this comment if it's
            //in your way...
            //var edo = new EDObject();
            //edo.system = "MarlonTest";
            //edo.objectName = "A 1";
            //edo.commander = "Marlon Blake";
            //edmat.Store(edo);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {

                UpdateObject(listView1.SelectedItems[0].Index);
            }
        }

        private void UpdateObject(int v)
        {
            if (v < 0)
                return;

            EDObject obj = edObjects[v];

            textBoxName.Text = obj.objectName;
            if (obj.IsPlanet)
            {
                panelPlanets.Visible = true;
                panelStar.Visible = false;
            }
            else
            {
                panelPlanets.Visible = false;
                panelStar.Visible = true;
            }


            var nr = (from str in dictComboDesc where str.Value == obj.Description select str.Key).FirstOrDefault<int>();
            comboBoxType.SelectedIndex = nr;


            textBoxGravity.Text = obj.gravity.ToString("0.00");
            textBoxRadius.Text = obj.radius.ToString("0");
            textBoxArrivalPoint.Text = obj.arrivalPoint.ToString("0");

            try
            {
                comboBoxAtmosphere.SelectedIndex = (int)obj.atmosphere;
                comboBoxVulcanism.SelectedIndex = (int)obj.vulcanism;
            }
            catch (Exception)
            {

            }
            SetMaterials(obj, checkedListBox1);
            SetMaterials(obj, checkedListBox2);
            SetMaterials(obj, checkedListBox3);
            SetMaterials(obj, checkedListBox4);

        }

        private void SetMaterials(EDObject obj, CheckedListBox box)
        {
            for (int i = 0; i < box.Items.Count; i++)
            {
                string item = (string)box.Items[i];
                MaterialEnum mat = obj.MaterialFromString(item);
                box.SetItemChecked(i, obj.materials[mat]);
            }
        }

        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBoxGravity_TextChanged(object sender, EventArgs e)
        {

        }

        private void PlanetsForm_Shown(object sender, EventArgs e)
        {
            SetCurrentSystem();
        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void panelStar_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
