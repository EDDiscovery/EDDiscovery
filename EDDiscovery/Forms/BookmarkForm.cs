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
        public string StarHeading { get { return textBoxName.Text; } }
        public string Notes { get { return textBoxNotes.Text; } }
        public string x { get { return textBoxX.Text; } }
        public string y { get { return textBoxY.Text; } }
        public string z { get { return textBoxZ.Text; } }

        public BookmarkForm()
        {
            InitializeComponent();
        }

        public void InitialisePos(double x, double y, double z)
        {
            textBoxX.Text = x.ToString("0.00");
            textBoxY.Text = y.ToString("0.00");
            textBoxZ.Text = z.ToString("0.00");
        }

        public void SystemInfo(string name, string note)
        {
            this.Text = "System Information";
            buttonCancel.Hide();
            buttonDelete.Hide();
            textBoxNotes.Hide();
            textBoxTime.Hide();
            labelTimeMade.Hide();
            labelBookmarkNotes.Hide();
            textBoxName.Text = name;
            textBoxTravelNote.Text = (note != null) ? note : "";
            int delta = textBoxTravelNote.Location.Y - labelTimeMade.Location.Y;
            buttonOK.Location = new Point(buttonOK.Location.X, buttonOK.Location.Y - delta);
            labelTravelNote.Location = new Point(labelTravelNote.Location.X, labelTravelNote.Location.Y - delta);
            labelTravelNoteEdit.Location = new Point(labelTravelNoteEdit.Location.X, labelTravelNoteEdit.Location.Y - delta);
            textBoxTravelNote.Location = new Point(textBoxTravelNote.Location.X, textBoxTravelNote.Location.Y - delta);
            this.Height -= delta;
        }

        public void RegionBookmark(string tme)
        {
            this.Text = "Create Region Bookmark";
            textBoxTime.Text = tme;
            textBoxX.ReadOnly = false;
            textBoxY.ReadOnly = false;
            textBoxZ.ReadOnly = false;
            textBoxName.ReadOnly = false;
            buttonDelete.Hide();
            labelTravelNote.Hide();
            labelTravelNoteEdit.Hide();
            textBoxTravelNote.Hide();

            int delta = buttonOK.Location.Y - labelTravelNote.Location.Y;
            buttonOK.Location = new Point(buttonOK.Location.X, buttonOK.Location.Y - delta);
            buttonCancel.Location = new Point(buttonCancel.Location.X, buttonCancel.Location.Y - delta);
            this.Height -= delta;
            buttonOK.Enabled = ValidateData();
        }

        public void Update(string name, string note, string bookmarknote, string tme, bool editheading )
        {
            this.Text = "Update Bookmark";
            buttonOK.Text = "Update";
            textBoxName.Text = name;
            textBoxName.ReadOnly = !editheading;
            textBoxNotes.Text = bookmarknote;
            textBoxTravelNote.Text = note;
            textBoxTime.Text = tme;

        }

        public void New(string name, string note, string tme)
        {
            this.Text = "New Bookmark";
            textBoxName.Text = name;
            textBoxTravelNote.Text = note;
            textBoxTime.Text = tme;
            buttonDelete.Hide();
        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            //            double xv, yv, zv;

            //          if (double.TryParse(x, out xv) && double.TryParse(y, out yv) && double.TryParse(z, out zv))
            ///        {
            //         DialogResult = DialogResult.OK;
            //       Close();
            // }
            //lse
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

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = ValidateData();
        }

        private bool ValidateData()
        {
            double xp, yp, zp;
            return (textBoxName.Text.Length > 0 && double.TryParse(textBoxX.Text, out xp) && double.TryParse(textBoxY.Text, out yp) && double.TryParse(textBoxZ.Text, out zp));
        }
    }
}
