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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.EDSM;
using System;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class BookmarkForm : ExtendedControls.DraggableForm
    {
        public string StarHeading { get { return textBoxName.Text; } }
        public string Notes { get { return textBoxBookmarkNotes.Text; } }
        public string x { get { return textBoxX.Text; } }
        public string y { get { return textBoxY.Text; } }
        public string z { get { return textBoxZ.Text; } }
        public bool IsTarget { get { return checkBoxTarget.Checked;  } }
        public PlanetMarks SurfaceLocations { get { return SurfaceBookmarks.PlanetMarks; } }
        
        private string edsmurl = null;
        bool validatestarname = false;

        public BookmarkForm()
        {
            InitializeComponent();
            EDDTheme.Instance.ApplyDialog(this);

            BaseUtils.Translator.Instance.Translate(this, new Control[] { labelX, labelY, labelZ, SurfaceBookmarks  });
        }

        #region External Initialisation

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

        public void NotedSystem(string name, string note, bool istarget)          // from target, a system with notes
        {
            this.Text = "System Information".T(EDTx.BookmarkForm_SI);
            ISystem s = SystemCache.FindSystem(name);
            textBoxName.Text = name;
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxTravelNote.Text = (note != null) ? note : "";
            checkBoxTarget.Checked = istarget;

            HideTime();
            HideBookmarkNotes();
            HideSurfaceBookmarks();

            var edsm = new EDSMClass();
            edsmurl = edsm.GetUrlToEDSMSystem(s?.Name ?? name, s?.EDSMID);
        }


        public void NewRegionBookmark(DateTime timeutc)              // from map, a region bookmark at this time
        {
            this.Text = "Create Region Bookmark".T(EDTx.BookmarkForm_RB);
            textBoxName.Text = "Enter a region name...".T(EDTx.BookmarkForm_RN);
            textBoxName.ClearOnFirstChar = true;
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(timeutc).ToString();
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxX.ReadOnly = false;
            textBoxY.ReadOnly = false;
            textBoxZ.ReadOnly = false;

            HideEDSM();
            HideTravelNote();   // in order
            HideSurfaceBookmarks();
            buttonDelete.Hide();

            buttonOK.Enabled = ValidateData();
        }

        public void Bookmark(BookmarkClass bk)        // from multiple places, update this bookmark, region or system..
        {
            string note = "";
            string name;
            long? edsmid = null;

            if (!bk.isRegion)
            {
                ISystem s = SystemCache.FindSystem(bk.StarName);
                if (s != null)    // paranoia
                {
                    InitialisePos(s);
                    edsmid = s.EDSMID;
                }
                else
                    InitialisePos(bk.x, bk.y, bk.z);

                SystemNoteClass sn = SystemNoteClass.GetNoteOnSystem(bk.StarName);
                note = (sn != null) ? sn.Note : "";

                name = bk.StarName;
            }
            else
            {       // region, set position, set name
                InitialisePos(bk.x, bk.y, bk.z);
                name = bk.Heading;
            }

            this.Text = "Update Bookmark".T(EDTx.BookmarkForm_UB);
            buttonOK.Text = "Update".T(EDTx.BookmarkForm_Update);
            textBoxName.Text = name;
            textBoxName.ReadOnly = !bk.isRegion;
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxBookmarkNotes.Text = bk.Note;
            textBoxBookmarkNotes.CursorToEnd();
            textBoxBookmarkNotes.ScrollToCaret();
            textBoxTravelNote.Text = note;
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(bk.TimeUTC).ToString();
            checkBoxTarget.Checked = bk.id == TargetClass.GetTargetBookmark();      // who is the target of a bookmark (0=none)

            //foreach (Control c in panelOuter.Controls) System.Diagnostics.Debug.WriteLine("All Control {0} at {1}", c.Name, c.Top);

            if (bk.isRegion)
            {
                HideEDSM();
                HideTravelNote();   // in order note
                HideSurfaceBookmarks();
            }
            else
            {
                var edsm = new EDSMClass();
                edsmurl = edsm.GetUrlToEDSMSystem(name, edsmid);
                SurfaceBookmarks.Init(bk.StarName, bk.PlanetaryMarks);
            }


            buttonOK.Enabled = true;
        }

        public void Bookmark(BookmarkClass bk, string planet, double latitude, double longitude)  // from compass, bookmark at planet/lat/long
        {
            Bookmark(bk);
            SurfaceBookmarks.AddSurfaceLocation(planet, latitude, longitude);
        }

        public void NewSystemBookmark(ISystem system, string note, DateTime timeutc)    // from multipe, create a new system bookmark
        {
            this.Text = "New System Bookmark".T(EDTx.BookmarkForm_SB);
            textBoxName.Text = system.Name;
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxTravelNote.Text = note;
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(timeutc).ToString();
            InitialisePos(system);
            buttonDelete.Hide();
            var edsm = new EDSMClass();
            edsmurl = edsm.GetUrlToEDSMSystem(system.Name,system.EDSMID);
            SurfaceBookmarks.Init(system.Name);
            buttonOK.Enabled = true;
        }

                                                                                    // from compass, new system bookmark at position
        public void NewSystemBookmark(ISystem system, string note, DateTime timeutc, string planet, double latitude, double longitude)
        {
            NewSystemBookmark(system, note, timeutc);
            SurfaceBookmarks.AddSurfaceLocation(planet, latitude, longitude);
        }

        public void NewFreeEntrySystemBookmark(DateTime timeutc)     // new system bookmark anywhere
        {
            this.Text = "New System Bookmark".T(EDTx.BookmarkForm_NSB);
            textBoxName.Text = "Enter a system name...".T(EDTx.BookmarkForm_ESN);
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            validatestarname = true;
            textBoxName.SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList,true);
            textBoxName.ClearOnFirstChar = true;
            textBoxName.SelectAll();
            textBoxName.Focus();
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(timeutc).ToString();
            validatestarname = true;
            buttonDelete.Hide();
            buttonEDSM.Enabled = false;
            buttonOK.Enabled = false;
            SurfaceBookmarks.Init("");
        }

        public void GMO(string name, string descr , bool istarget , string url )    // from formmap, new GMO bookmark
        {
            this.Text = "Galactic Mapping Object".T(EDTx.BookmarkForm_GMO);
            textBoxName.Text = name;
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxBookmarkNotes.Text = descr.WordWrap(40);
            textBoxBookmarkNotes.CursorToEnd();
            textBoxBookmarkNotes.ScrollToCaret();
            textBoxBookmarkNotes.ReadOnly = true;
            labelBookmarkNotes.Text = "Description".T(EDTx.BookmarkForm_Description);
            buttonDelete.Hide();
            HideTime();
            HideTravelNote();   // in order
            HideSurfaceBookmarks();

            checkBoxTarget.Checked = istarget;

            edsmurl = url;
        }

        private void HideTime() { labelTimeMade.Hide(); textBoxTime.Hide(); ShiftControls(textBoxBookmarkNotes, textBoxTime); }
        private void HideBookmarkNotes() { labelBookmarkNotes.Hide(); textBoxBookmarkNotes.Hide(); ShiftControls(textBoxTravelNote, textBoxBookmarkNotes); }
        private void HideTravelNote() { labelTravelNote.Hide();  textBoxTravelNote.Hide(); ShiftControls(SurfaceBookmarks, textBoxTravelNote); }
        private void HideSurfaceBookmarks() { SurfaceBookmarks.Hide(); ShiftControls(buttonOK, SurfaceBookmarks); }
        private void HideEDSM() { buttonEDSM.Hide(); checkBoxTarget.Left = buttonEDSM.Left; }

        void ShiftControls(Control bot, Control top)
        {
            int topline = top.Top;
            int delta = bot.Top - topline;

            //System.Diagnostics.Debug.WriteLine("Control bot {0} {1} to {2} {3} delta {4}", bot.Name, bot.Top, top.Name, top.Top, delta);
            foreach (Control c in panelOuter.Controls)
            {
                if (c.Top >= topline)  // if below..
                {
                    //System.Diagnostics.Debug.WriteLine("Control {0} at {1} -> {2}", c.Name, c.Top, c.Top - delta);
                    c.Top -= delta;
                }
            }
            //System.Diagnostics.Debug.WriteLine("height now " + Height + " delta " + delta);
            Height -= delta;
        }

        #endregion

        #region UI

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
            if ( validatestarname )
            {
                ISystem f = SystemCache.FindSystem(textBoxName.Text);
                if (f != null && f.HasCoordinate)
                {
                    InitialisePos(f);
                    var edsm = new EDSMClass();
                    edsmurl = edsm.GetUrlToEDSMSystem(f.Name,f.EDSMID);
                    SurfaceBookmarks.Init(f.Name);
                }
                else
                    textBoxX.Text = textBoxY.Text = textBoxZ.Text = "";

                buttonEDSM.Enabled = buttonOK.Enabled = ValidateData() && f != null;
            }
            else
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

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }


        #endregion
    }
}
