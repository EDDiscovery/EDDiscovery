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
using System.Globalization;

namespace EDDiscovery2.PlanetSystems
{
    public partial class PlanetsForm : Form
    {
        public EDDiscovery.EDDiscoveryForm edForm;
        private List<EDPlanet> edObjects = new List<EDPlanet>();
        private EdMaterializer edmat = new EdMaterializer();
        private Dictionary<int, string> dictComboDesc = new Dictionary<int, string>();

        private int CurrentItem = 0;
        private EDPlanet currentObj;

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
            foreach (EDPlanet obj in EDPlanet.listObjectTypes)
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
            if (edForm.TravelControl.CurrentSystem!=null)
                SetSystem(edForm.TravelControl.CurrentSystem.name);
        }

        private void SetSystem(string systemname)
        {
            if (systemname == null)
                return;

            textBoxSystemName.Text = systemname;

            edObjects.Clear();

            edObjects = edmat.GetAllPlanets(textBoxSystemName.Text);
            UpDateListView();
            //addMaterialNodeControl1.CurrentSystem = currentSystem.name;
        }


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            EDPlanet obj = new EDPlanet();
            obj.system = textBoxSystemName.Text;
            obj.commander = edForm.CommanderName;
            edObjects.Add(obj);
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
            if (edObjects.Count>0)
                listView1.Items[0].Selected = true;
        }

        private void UpdateListViewLine()
        {
            ListViewItem lvi;

            if (listView1.SelectedItems.Count > 0)
            {
                lvi = listView1.SelectedItems[0];

                lvi.SubItems[0].Text = currentObj.objectName;
                lvi.SubItems[1].Text = currentObj.Description;
                lvi.SubItems[2].Text = currentObj.gravity.ToString("0.00");
                lvi.SubItems[3].Text = currentObj.arrivalPoint.ToString("0");


                for (int ii = 0; ii < mlist.Count; ii++)
                {
                    if (currentObj.materials[mlist[ii].material])
                        lvi.SubItems[3+ii].Text = "X";
                    else
                        lvi.SubItems[3 + ii].Text = " ";
                }


            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            UpdateEDObject(currentObj);

            edmat.StorePlanet(currentObj);
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

            currentObj = edObjects[v];

            textBoxName.Text = currentObj.objectName;
            if (currentObj.IsPlanet)
            {
                panelPlanets.Visible = true;
                panelStar.Visible = false;
            }
            else
            {
                panelPlanets.Visible = false;
                panelStar.Visible = true;
            }


            var nr = (from str in dictComboDesc where str.Value == currentObj.Description select str.Key).FirstOrDefault<int>();
            comboBoxType.SelectedIndex = nr;


            textBoxGravity.Text = currentObj.gravity.ToString("0.00");
            textBoxRadius.Text = currentObj.radius.ToString("0");
            textBoxArrivalPoint.Text = currentObj.arrivalPoint.ToString("0");

            try
            {
                comboBoxAtmosphere.SelectedIndex = (int)currentObj.atmosphere;
                comboBoxVulcanism.SelectedIndex = (int)currentObj.vulcanism;
            }
            catch (Exception)
            {

            }
            SetMaterials(currentObj, checkedListBox1);
            SetMaterials(currentObj, checkedListBox2);
            SetMaterials(currentObj, checkedListBox3);
            SetMaterials(currentObj, checkedListBox4);

        }

        private void SetMaterials(EDPlanet obj, CheckedListBox box)
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

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            SetSystem(textBoxSystemName.Text);
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
         
            
        }


        private void UpdateEDObject(EDPlanet obj)
        {
            
            obj.objectName = textBoxName.Text;
            obj.ObjectType = obj.String2ObjectType(comboBoxType.Text);

            var culture = new CultureInfo("en-US");
            currentObj.gravity = float.Parse(textBoxGravity.Text.Replace(",", "."), culture);
            currentObj.radius = float.Parse(textBoxRadius.Text.Replace(",", "."), culture);
            currentObj.arrivalPoint = float.Parse(textBoxArrivalPoint.Text.Replace(",", "."), culture);

            currentObj.atmosphere = obj.AtmosphereStr2Enum(comboBoxAtmosphere.Text);
            currentObj.vulcanism = obj.VulcanismStr2Enum(comboBoxVulcanism.Text);

 
            GetMaterials(ref currentObj, checkedListBox1);
            GetMaterials(ref currentObj, checkedListBox2);
            GetMaterials(ref currentObj, checkedListBox3);
            GetMaterials(ref currentObj, checkedListBox4);


            UpdateListViewLine();
        }


        private void GetMaterials(ref EDPlanet obj, CheckedListBox box)
        {
            for (int i = 0; i < box.Items.Count; i++)
            {
                string item = (string)box.Items[i];
                MaterialEnum mat = obj.MaterialFromString(item);
                obj.materials[mat] =  box.GetItemChecked(i);
            }
        }



    }

}
