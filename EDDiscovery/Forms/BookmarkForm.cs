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
    public partial class BookmarkForm : Form
    {
        public string Heading { get { return textBoxName.Text; } }
        public string Notes { get { return textBoxNotes.Text; } }

        public BookmarkForm()
        {
            InitializeComponent();
        }

        public void Initialise(string starname, double x, double y, double z, string heading,
            string note, DateTime tme, string travelnote)
        {
            textBoxName.Text = (heading != null) ? heading : starname;
            textBoxName.ReadOnly = (heading == null);

            textBoxX.Text = x.ToString("0.00");
            textBoxY.Text = y.ToString("0.00");
            textBoxZ.Text = z.ToString("0.00");

            textBoxNotes.Text = note;
            textBoxTime.Text = tme.ToString();

            textBoxTravelNote.Text = (travelnote != null) ? travelnote : "";
            buttonDelete.Hide();
        }

        public void SetForUpdate()
        {
            buttonOK.Text = "Update";
            this.Text = "Update Bookmark";
            buttonDelete.Show();
        }

        public void SetForRegionBookmark()
        {
            this.Text = "Create Region Bookmark";
            textBoxX.ReadOnly = false;
            textBoxY.ReadOnly = false;
            textBoxZ.ReadOnly = false;
            textBoxName.ReadOnly = false;
            labelTravelNote.Hide();
            textBoxTravelNote.Hide();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }
    }
}
