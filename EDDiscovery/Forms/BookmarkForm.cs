/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using EDDiscovery;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static EliteDangerousCore.DB.PlanetMarks;

namespace EDDiscovery.Forms
{
    public partial class BookmarkForm : ExtendedControls.DraggableForm
    {
        public string StarHeading { get { return textBoxName.Text; } }
        public string Notes { get { return textBoxNotes.Text; } }
        public string x { get { return textBoxX.Text; } }
        public string y { get { return textBoxY.Text; } }
        public string z { get { return textBoxZ.Text; } }
        public bool IsTarget { get { return checkBoxTarget.Checked;  } }
        public PlanetMarks SurfaceLocations { get { return userControlSurfaceBookmarks1.PlanetMarks; } }
        
        private string edsmurl = null;
        bool freeSystemEntry = false;

        public BookmarkForm()
        {
            InitializeComponent();
            EDDTheme.Instance.ApplyToFormStandardFontSize(this);
        }

        public void InitialisePos(ISystem system)
        {
            textBoxX.Text = system.X.ToString("0.00");
            textBoxY.Text = system.Y.ToString("0.00");
            textBoxZ.Text = system.Z.ToString("0.00");
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
            userControlSurfaceBookmarks1.Hide();
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

            var edsm = new EDSMClass();
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
            userControlSurfaceBookmarks1.Hide();

            int delta = buttonOK.Location.Y - labelTravelNote.Location.Y;
            ShiftLocationY(buttonOK, delta);
            ShiftLocationY(buttonCancel, delta);
            this.Height -= delta;
            buttonOK.Enabled = ValidateData();
        }

        public void Update(string name, string note, string bookmarknote, string tme, bool regionmark , bool istarget, PlanetMarks locations)
        {
            this.Text = "Update Bookmark";
            buttonOK.Text = "Update";
            textBoxName.Text = name;
            textBoxName.ReadOnly = !regionmark;
            textBoxNotes.Text = bookmarknote;
            textBoxNotes.CursorToEnd();
            textBoxNotes.ScrollToCaret();
            textBoxTravelNote.Text = note;
            textBoxTime.Text = tme;
            checkBoxTarget.Checked = istarget;

            if ( regionmark )
            {
                buttonEDSM.Hide();
                labelTravelNote.Hide();
                labelTravelNoteEdit.Hide();
                textBoxTravelNote.Hide();
                userControlSurfaceBookmarks1.Hide();
                int delta = buttonOK.Location.Y - labelTravelNote.Location.Y;
                ShiftLocationY(buttonOK, delta);
                ShiftLocationY(buttonCancel, delta);
                ShiftLocationY(buttonDelete, delta);
                this.Height -= delta;
            }
            else
            {
                var edsm = new EDSMClass();
                edsmurl = edsm.GetUrlToEDSMSystem(name);
                userControlSurfaceBookmarks1.NewForSystem(name);
            }
            
        }

        public void Update(BookmarkClass bk)
        {
            string note = "";
            string name;
            if (!bk.isRegion)
            {
                SystemNoteClass sn = SystemNoteClass.GetNoteOnSystem(bk.StarName);
                ISystem s = SystemClassDB.GetSystem(bk.StarName);
                InitialisePos(s);
                note = (sn != null) ? sn.Note : "";
                name = bk.StarName;
            }
            else
            {
                name = bk.Heading;
            }
             
            Update(name, note, bk.Note, bk.Time.ToString(), bk.isRegion, false, bk.PlanetaryMarks);
            userControlSurfaceBookmarks1.ApplyBookmark(bk);
            buttonOK.Enabled = true;
        }

        public void Update(BookmarkClass bk, string planet, double latitude, double longitude)
        {
            Update(bk);
            userControlSurfaceBookmarks1.AddSurfaceLocation(planet, latitude, longitude);
        }

        public void NewSystemBookmark(ISystem system, string note, string tme)
        {
            this.Text = "New System Bookmark";
            textBoxName.Text = system.Name;
            textBoxTravelNote.Text = note;
            textBoxTime.Text = tme;
            InitialisePos(system);
            buttonDelete.Hide();
            var edsm = new EDSMClass();
            edsmurl = edsm.GetUrlToEDSMSystem(system.Name);
            userControlSurfaceBookmarks1.NewForSystem(system.Name);
            buttonOK.Enabled = true;
        }


        public void NewSystemBookmark(ISystem system, string note, string tme, string planet, double latitude, double longitude)
        {
            NewSystemBookmark(system, note, tme);
            userControlSurfaceBookmarks1.AddSurfaceLocation(planet, latitude, longitude);
        }

        public void NewSystemBookmark(string tme)
        {
            this.Text = "New System Bookmark";
            textBoxName.Text = "Enter a system name...";
            textBoxName.SelectAll();
            textBoxName.Focus();
            textBoxTime.Text = tme;
            buttonDelete.Hide();
            freeSystemEntry = true;
            buttonOK.Enabled = false;
            userControlSurfaceBookmarks1.NewForSystem("");
        }

        public void GMO(string name, string descr , bool istarget , string url )
        {
            this.Text = "Galactic Mapping Object";
            textBoxName.Text = name;
            textBoxNotes.Text = descr.WordWrap(40);
            textBoxNotes.CursorToEnd();
            textBoxNotes.ScrollToCaret();
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
            if(!freeSystemEntry)
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

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void textBoxName_Validated(object sender, EventArgs e)
        {
            if (!freeSystemEntry)
                return;
            string t = textBoxName.Text.Trim();
            ISystem s = SystemClassDB.GetSystem(t);

            if (s != null)
            {
                var edsm = new EDSMClass();
                edsmurl = edsm.GetUrlToEDSMSystem(t);
                userControlSurfaceBookmarks1.NewForSystem(t);
                SystemNoteClass sn = SystemNoteClass.GetNoteOnSystem(t);
                textBoxNotes.Text = (sn != null) ? sn.Note : "";
                InitialisePos(s);
                buttonOK.Enabled = true;
                labelBadSystem.Text = "";
            }
            else
            {
                buttonOK.Enabled = false;
                textBoxName.SelectAll();
                labelBadSystem.Text = "System name not recognised";
            }
        }
    }
}
