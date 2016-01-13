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
            SetCurrentSystem();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetCurrentSystem();
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

            //addMaterialNodeControl1.CurrentSystem = currentSystem.name;
        }

        private void addMaterialNodeControl1_Load(object sender, EventArgs e)
        {

        }
    }

}
