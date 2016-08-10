using EDDiscovery;
using EDDiscovery2.EDSM;
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
        public bool IsTarget { get { return checkBoxTarget.Checked;  } }

        private string edsmurl = null;

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

        public void NotedSystem(string name, string note, bool target)
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

            int delta = textBoxTravelNote.Location.Y - checkBoxTarget.Location.Y;       // before we move it

            checkBoxTarget.Location = new Point(checkBoxTarget.Location.X, labelTimeMade.Location.Y);
            buttonEDSM.Location = new Point(buttonEDSM.Location.X, labelTimeMade.Location.Y);

            ShiftLocationY(buttonOK, delta);
            ShiftLocationY(labelTravelNote, delta);
            ShiftLocationY(labelTravelNoteEdit, delta);
            ShiftLocationY(textBoxTravelNote, delta);
            this.Height -= delta;
            checkBoxTarget.Checked = target;

            var edsm = new EDSM.EDSMClass();
            edsmurl = edsm.GetUrlToEDSMSystem(name);
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
            buttonEDSM.Hide();

            int delta = buttonOK.Location.Y - labelTravelNote.Location.Y;
            ShiftLocationY(buttonOK, delta);
            ShiftLocationY(buttonCancel, delta);
            this.Height -= delta;
            buttonOK.Enabled = ValidateData();
        }

        public void Update(string name, string note, string bookmarknote, string tme, bool regionmark , bool istarget )
        {
            this.Text = "Update Bookmark";
            buttonOK.Text = "Update";
            textBoxName.Text = name;
            textBoxName.ReadOnly = !regionmark;
            textBoxNotes.Text = bookmarknote;
            textBoxTravelNote.Text = note;
            textBoxTime.Text = tme;
            checkBoxTarget.Checked = istarget;

            if ( regionmark )
            {
                buttonEDSM.Hide();
                labelTravelNote.Hide();
                labelTravelNoteEdit.Hide();
                textBoxTravelNote.Hide();
                int delta = buttonOK.Location.Y - labelTravelNote.Location.Y;
                ShiftLocationY(buttonOK, delta);
                ShiftLocationY(buttonCancel, delta);
                ShiftLocationY(buttonDelete, delta);
                this.Height -= delta;
            }
            else
            {
                var edsm = new EDSM.EDSMClass();
                edsmurl = edsm.GetUrlToEDSMSystem(name);
            }
        }

        public void NewSystemBookmark(string name, string note, string tme)
        {
            this.Text = "New System Bookmark";
            textBoxName.Text = name;
            textBoxTravelNote.Text = note;
            textBoxTime.Text = tme;
            buttonDelete.Hide();
            var edsm = new EDSM.EDSMClass();
            edsmurl = edsm.GetUrlToEDSMSystem(name);
        }

        public void GMO(string name, string descr , bool istarget , string url )
        {
            this.Text = "Galactic Mapping Object";
            textBoxName.Text = name;
            textBoxNotes.Text = Tools.WordWrap(descr,40);
            textBoxNotes.SelectionStart = textBoxNotes.Text.Length;
            textBoxNotes.SelectionLength = 0;
            textBoxNotes.ReadOnly = true;
            labelBookmarkNotes.Text = "Description";
            buttonDelete.Hide();
            textBoxTime.Hide();
            labelTimeMade.Hide();
            textBoxTravelNote.Hide();
            labelTravelNote.Hide();
            labelTravelNoteEdit.Hide();

            checkBoxTarget.Checked = istarget;

            int delta = buttonOK.Location.Y - labelTravelNote.Location.Y;
            ShiftLocationY(buttonOK, delta);
            ShiftLocationY(buttonCancel, delta);
            this.Height -= delta;

            delta = buttonEDSM.Location.Y - labelTimeMade.Location.Y;
            ShiftLocationY(buttonEDSM, delta);
            ShiftLocationY(checkBoxTarget, delta);
            ShiftLocationY(labelBookmarkNotes, delta);
            ShiftLocationY(textBoxNotes, delta);
            ShiftLocationY(buttonOK, delta);
            ShiftLocationY(buttonCancel, delta);
            this.Height -= delta;

            edsmurl = url;
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

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            buttonOK.Enabled = ValidateData();
        }

        private bool ValidateData()
        {
            double xp, yp, zp;
            return (textBoxName.Text.Length > 0 && double.TryParse(textBoxX.Text, out xp) && double.TryParse(textBoxY.Text, out yp) && double.TryParse(textBoxZ.Text, out zp));
        }

        private void buttonEDSM_Click(object sender, EventArgs e)
        {
            if (edsmurl != null)
                System.Diagnostics.Process.Start(edsmurl);
        }

        void ShiftLocationY(Control c, int d)
        {
            c.Location = new Point(c.Location.X, c.Location.Y - d);
        }

    }
}
