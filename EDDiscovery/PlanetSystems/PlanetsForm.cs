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
using EDDiscovery2.PlanetSystems;

namespace EDDiscovery2.PlanetSystems
{
    public partial class PlanetsForm : Form
    {
        public EDDiscovery.EDDiscoveryForm edForm;
        private List<EDObject> edObjects = new List<EDObject>();
        private Dictionary<int, string> dictComboPlanetDesc = new Dictionary<int, string>();
        private Dictionary<int, string> dictComboStarDesc = new Dictionary<int, string>();

        private int CurrentItem = 0;
        private EDObject currentObj;

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





            dictComboPlanetDesc.Clear();
            dictComboStarDesc.Clear();
            foreach (EDWorld obj in EDWorld.listObjectTypes)
            {
                if (obj.IsPlanet)
                {
                    int nr = comboBoxType.Items.Add(obj.Description);
                    dictComboPlanetDesc[nr] = obj.Description;
                }
                if (obj.IsStar)
                {
                    int nr = comboBoxStarType.Items.Add(obj.Description);
                    dictComboStarDesc[nr] = obj.Description;
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
            List<EDWorld> planets;
            List<EDStar> stars;
            if (systemname == null)
                return;

            textBoxSystemName.Text = systemname;

            edObjects.Clear();

            Repositories.World world = new Repositories.World();
            planets = world.GetAllForSystem(textBoxSystemName.Text);

            Repositories.Star star = new Repositories.Star();
            stars = star.GetAllForSystem(textBoxSystemName.Text);

            // Proof of concept tests. Assumes the 2nd planet has surveys and world surveys
            // Uncomment to see in action! 

            //if (planets != null && planets.Count > 1)
            //{               
            //    //pulling a world_survey from a world
            //    var worldSurvey = planets[1].GetWorldSurvey();

            //    if (worldSurvey != null)
            //        System.Diagnostics.Trace.WriteLine($"WorldSurvey id = {worldSurvey.id}");

            //    var surveys = planets[1].GetSurveys();
            //    if (surveys != null && surveys.Count > 0 )
            //        System.Diagnostics.Trace.WriteLine($"Survey[0] id = {surveys[0].id}");
            //}

            edObjects.AddRange(planets);
            edObjects.AddRange(stars);
            UpDateListView();
            //addMaterialNodeControl1.CurrentSystem = currentSystem.name;
        }


        private void toolStripButtonAdd_Click(object sender, EventArgs e)
        {
            EDWorld obj = new EDWorld();
            obj.system = textBoxSystemName.Text;
            obj.updater = edForm.CommanderName;
            edObjects.Add(obj);
            CurrentItem = edObjects.Count - 1;
            UpDateListView();

        }

        private void UpDateListView()
        {
            listView1.Items.Clear();

            foreach (var ob in edObjects)
            {
                if (ob is EDWorld)
                {
                    EDWorld planet = (EDWorld)ob;
                    ListViewItem lvi;
                    lvi = listView1.Items.Add(planet.objectName);
                    lvi.SubItems.Add(planet.Description);

                    lvi.SubItems.Add(planet.gravity.ToString("0.00"));
                    lvi.SubItems.Add(planet.arrivalPoint.ToString("0"));

                    lvi.UseItemStyleForSubItems = false;


                    lvi.Tag = planet;
                }
                if (ob is EDStar)
                {
                    EDStar planet = (EDStar)ob;
                    ListViewItem lvi;
                    lvi = listView1.Items.Add(planet.objectName);
                    lvi.SubItems.Add(planet.Description);

                    lvi.SubItems.Add("");  // Gravity
                    lvi.SubItems.Add(planet.arrivalPoint.ToString("0"));

                    lvi.UseItemStyleForSubItems = false;

                    
                    for (int ii = 0; ii < mlist.Count; ii++)
                    {
                        ListViewItem.ListViewSubItem lvsi;
                         lvsi = lvi.SubItems.Add("");
                        lvsi.BackColor = Color.Gray;
                    }
                    
                    lvi.Tag = planet;
                }
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

                if (currentObj is EDWorld)
                {
                    EDWorld planet = (EDWorld)currentObj;
                    lvi.SubItems[0].Text = planet.objectName;
                    lvi.SubItems[1].Text = planet.Description;
                    lvi.SubItems[2].Text = planet.gravity.ToString("0.00");
                    lvi.SubItems[3].Text = planet.arrivalPoint.ToString("0");
           
                }
                if (currentObj is EDStar)
                {
                    EDStar star = (EDStar)currentObj;
                    lvi.SubItems[0].Text = star.objectName;
                    lvi.SubItems[1].Text = star.Description;
                    lvi.SubItems[2].Text = "";
                    lvi.SubItems[3].Text = star.arrivalPoint.ToString("0");

                }
            }
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            UpdateEDObject(currentObj);
            if (currentObj is EDWorld)
            {
                Repositories.World world = new Repositories.World();
                world.Store((EDWorld)currentObj);
            }
            if (currentObj is EDStar)
            {
                Repositories.Star star = new Repositories.Star();
                star.Store((EDStar)currentObj);
            }
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


            if (currentObj is EDWorld)
            {
                EDWorld planet = (EDWorld)currentObj;

                textBoxName.Text = currentObj.objectName;


                var nr = (from str in dictComboPlanetDesc where str.Value == planet.Description select str.Key).FirstOrDefault<int>();
                comboBoxType.SelectedIndex = nr;

                textBoxMass.Text = planet.mass.ToString("0.0000");
                textBoxRadius.Text = planet.radius.ToString("0");

                if (planet.gravity == 0)
                    planet.gravity = (float)CalcG(planet.mass, planet.radius);

                textBoxGravity.Text = planet.gravity.ToString("0.00");
                textBoxSurfaceTemp.Text = planet.surfaceTemp.ToString("0");
                textBoxPreasure.Text = planet.surfacePressure.ToString("0.00");

                textBoxArrivalPoint.Text = planet.arrivalPoint.ToString("0");
                textBoxOrbitPeriod.Text = planet.orbitPeriod.ToString("0.0");
                textBoxRotationPeriod.Text = planet.rotationPeriod.ToString("0.0");
                textBoxSemiMajorAxis.Text = planet.semiMajorAxis.ToString("0.00");
                textBoxArrivalPoint.Text = planet.arrivalPoint.ToString("0.0");
                textBoxRock.Text = planet.rockPct.ToString("0.0");
                textBoxMetal.Text = planet.metalPct.ToString("0.0");
                textBoxIce.Text = planet.icePct.ToString("0.0");


                try
                {
                    comboBoxAtmosphere.SelectedIndex = (int)planet.atmosphere;
                    comboBoxVulcanism.SelectedIndex = (int)planet.vulcanism;
                }
                catch (Exception)
                {

                }
                //SetMaterials(planet, checkedListBox1);
                //SetMaterials(planet, checkedListBox2);
                //SetMaterials(planet, checkedListBox3);
                //SetMaterials(planet, checkedListBox4);
            }
            if (currentObj is EDStar)
            {
                EDStar star = (EDStar)currentObj;

                textBoxStarName.Text = star.objectName;

                var nr = (from str in dictComboStarDesc where str.Value == star.Description select str.Key).FirstOrDefault<int>();
                comboBoxStarType.SelectedIndex = nr;
                textBoxStarSubClass.Text = star.subclass;
                textBoxStarMass.Text = star.mass.ToString();
                textBoxS_Radius.Text = star.radius.ToString();
                textBox_StarAge.Text = star.star_age.ToString("0");
                textBoxStarOrbitPeriod.Text = star.orbitPeriod.ToString();
                textBoxS_ArrivalPoint.Text = star.arrivalPoint.ToString("0");
                textBoxStarNote.Text = star.notes;
                textBoxStarLuminosity.Text = star.luminosity;
                textBoxStarTemp.Text = star.surfaceTemp.ToString("0");
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


        private void UpdateEDObject(EDObject obj)
        {
            if (obj is EDWorld)
            {
                EDWorld planet = (EDWorld)obj;
                planet.objectName = textBoxName.Text;
                planet.ObjectType = obj.String2ObjectType(comboBoxType.Text);

                var culture = new CultureInfo("en-US");
                planet.mass = float.Parse(textBoxMass.Text.Replace(",", "."), culture);
                planet.gravity = float.Parse(textBoxGravity.Text.Replace(",", "."), culture);
                planet.radius = float.Parse(textBoxRadius.Text.Replace(",", "."), culture);
                planet.surfaceTemp = Int16.Parse(textBoxSurfaceTemp.Text, culture);
                planet.arrivalPoint = float.Parse(textBoxArrivalPoint.Text.Replace(",", "."), culture);
                planet.surfacePressure = float.Parse(textBoxSurfaceTemp.Text.Replace(",", "."), culture);

                planet.atmosphere = planet.AtmosphereStr2Enum(comboBoxAtmosphere.Text);
                planet.vulcanism = planet.VulcanismStr2Enum(comboBoxVulcanism.Text);

                //GetMaterials(ref planet, checkedListBox1);
                //GetMaterials(ref planet, checkedListBox2);
                //GetMaterials(ref planet, checkedListBox3);
                //GetMaterials(ref planet, checkedListBox4);
            }

            if (obj is EDStar)
            {
                EDStar star = (EDStar)obj;
                star.objectName = textBoxStarName.Text;
                star.ObjectType = obj.String2ObjectType(comboBoxStarType.Text);
                star.subclass = textBoxStarSubClass.Text;

                var culture = new CultureInfo("en-US");
                star.mass = float.Parse(textBoxStarMass.Text.Replace(",", "."), culture); 
                star.radius = float.Parse(textBoxS_Radius.Text.Replace(",", "."), culture);
                star.arrivalPoint = float.Parse(textBoxS_ArrivalPoint.Text.Replace(",", "."), culture);

                star.star_age = float.Parse(textBox_StarAge.Text.Replace(",", "."), culture);
                star.surfaceTemp = int.Parse(textBoxStarTemp.Text, culture);
                star.orbitPeriod = float.Parse(textBoxStarOrbitPeriod.Text.Replace(",", "."), culture);
                star.luminosity = textBoxStarLuminosity.Text;
                star.notes = textBoxStarNote.Text;
            }



            UpdateListViewLine();
        }




        private void toolStripButtonAddStar_Click(object sender, EventArgs e)
        {
            EDStar obj = new EDStar();
            obj.system = textBoxSystemName.Text;
            obj.updater = edForm.CommanderName;
            edObjects.Add(obj);
            CurrentItem = edObjects.Count - 1;
            UpDateListView();
        }

        private void panelPlanets_Paint(object sender, PaintEventArgs e)
        {

        }

        private double CalcG(double mass, double radius)
        {
            if (mass == 0 || radius == 0)
                return 0;
            return mass * 5.9722E+24 * 6.67E-11 / ((radius * 1000)* (radius * 1000)) / 9.80665;
        }

    }

}
