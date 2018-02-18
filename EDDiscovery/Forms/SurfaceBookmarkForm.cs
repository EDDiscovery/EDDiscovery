using EliteDangerousCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static EliteDangerousCore.DB.PlanetMarks;

namespace EDDiscovery.Forms
{
    public partial class SurfaceBookmarkForm : ExtendedControls.DraggableForm
    {
        public Location BookmarkLocation;
        public string PlanetName;

        string SystemName;
        ISystem thisSystem;

        private void Initialise()
        {
            InitializeComponent();
            EDDTheme.Instance.ApplyToFormStandardFontSize(this);
            labelSystemName.Text = SystemName;
            if (thisSystem == null)
                thisSystem = EDDApplicationContext.EDDMainForm.history.FindSystem(SystemName);
            comboBoxBodies.Items.Add("");
            if (thisSystem != null)
            {
                var landables = EDDApplicationContext.EDDMainForm.history.starscan.FindSystem(thisSystem, true)?.Bodies?.Where(b => b.ScanData != null && b.ScanData.IsLandable)?.Select(b => b.fullname);
                foreach (string s in landables)
                {
                    comboBoxBodies.Items.Add(s);
                }
            }
            if (!String.IsNullOrEmpty(PlanetName))
                comboBoxBodies.SelectedItem = PlanetName;
            else
                comboBoxBodies.SelectedItem = "";

            BookmarkLocation = new Location();
        }

        public SurfaceBookmarkForm(string systemName)
        {
            SystemName = systemName;
            Initialise();
        }

        public SurfaceBookmarkForm(ISystem system)
        {
            SystemName = system.Name;
            thisSystem = system;
            Initialise();
        }

        public SurfaceBookmarkForm(string systemName, string planet)
        {
            PlanetName = planet;
            SystemName = systemName;
            Initialise();
        }

        public SurfaceBookmarkForm(Location existing, string systemName, string planet)
        {
            PlanetName = planet;
            SystemName = systemName;
            Initialise();

            BookmarkLocation = existing;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            BookmarkLocation.Comment = richTextBoxDescription.Text;
            BookmarkLocation.Name = textBoxName.Text;
            BookmarkLocation.Latitude = numberBoxLatitude.Value;
            BookmarkLocation.Longitude = numberBoxLongitude.Value;
            Close();
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = !String.IsNullOrEmpty(textBoxName.Text) && !String.IsNullOrEmpty(comboBoxBodies.Text);
        }

        private void comboBoxBodies_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = !String.IsNullOrEmpty(textBoxName.Text) && !String.IsNullOrEmpty(comboBoxBodies.Text);
            PlanetName = comboBoxBodies.Text;
        }
    }
}
