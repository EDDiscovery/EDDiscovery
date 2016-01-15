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
            dictComboDesc.Clear();
            foreach (EDObject obj in EDObject.listObjectTypes)
            {
                int nr = comboBoxType.Items.Add(obj.Description);
                dictComboDesc[nr] = obj.Description;
            }


            foreach (VulcanismEnum vulc in Enum.GetValues(typeof(VulcanismEnum)))
            {
                comboBoxVulcanism.Items.Add(vulc.ToString().Replace("_", " "));
            }
            foreach (AtmosphereEnum vulc in Enum.GetValues(typeof(AtmosphereEnum)))
            {
                comboBoxAtmosphere.Items.Add(vulc.ToString().Replace("_", " "));
            }
            SetCurrentSystem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetCurrentSystem();

      //      ObjectsType objt = new ObjectsType();

//            objt.ParseJson();
        }

        private void SetCurrentSystem()
        {
            SetSystem(edForm.TravelControl.CurrentSystem);
        }

        private void SetSystem(ISystem currentSystem)
        {
            if (currentSystem == null)
                return;

            textBoxSystemName.Text = currentSystem.name;

            edObjects.Clear();

            edObjects = edmat.GetAll(textBoxSystemName.Text);

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
                lvi.SubItems.Add("P S");
                lvi.SubItems.Add("Cd");
                lvi.SubItems.Add("W");
                lvi.SubItems.Add("Po");
                lvi.Tag = obj;
            }
            listView1.Items[0].Selected = true;
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {

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
    }

}
