/*
 * Copyright © 2016-2022 EDDiscovery development team
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

        private HistoryList historyentry;

        public BookmarkForm( HistoryList he, bool enabletargetsetting = false )
        {
            InitializeComponent();
            ExtendedControls.Theme.Current.ApplyDialog(this);

            var enumlist = new Enum[] { EDTx.BookmarkForm_buttonEDSM, EDTx.BookmarkForm_labelName, EDTx.BookmarkForm_checkBoxTarget, EDTx.BookmarkForm_labelBookmarkNotes, EDTx.BookmarkForm_labelTravelNote, EDTx.BookmarkForm_labelTimeMade };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist, new Control[] { labelX, labelY, labelZ, SurfaceBookmarks });

            historyentry = he;
            checkBoxTarget.Visible = enabletargetsetting;
        }

        #region External Initialisation

        // open an existing bookmark, region or system

        public void Bookmark(BookmarkClass bk)        
        {
            this.Text = "Update Bookmark".T(EDTx.BookmarkForm_UB);
            InitialisePos(bk.x, bk.y, bk.z);

            if (!bk.isRegion)       // Star..
            {
                textBoxName.Text = bk.StarName;
                textBoxName.ReadOnly = true; // no change
                textBoxTravelNote.Text = EliteDangerousCore.DB.SystemNoteClass.GetTextNotesOnSystem(bk.StarName);

                var edsm = new EDSMClass();
                edsmurl = edsm.GetUrlToSystem(bk.StarName);
                SurfaceBookmarks.Init(bk.StarName, historyentry, bk.PlanetaryMarks);
            }
            else
            {
                textBoxName.Text = bk.Heading;
                textBoxX.ReadOnly = textBoxY.ReadOnly = textBoxZ.ReadOnly = false;      // enable editing
                HideEDSM();
                HideTravelNote();   // in order note
                HideSurfaceBookmarks();
            }

            textBoxBookmarkNotes.Text = bk.Note;
            textBoxBookmarkNotes.CursorToEnd();
            textBoxBookmarkNotes.ScrollToCaret();
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(bk.TimeUTC).ToString();

            buttonOK.Text = "Update".T(EDTx.BookmarkForm_Update);
            buttonOK.Enabled = true;

            textBoxName.ReturnPressed += (ctrl) => { return true; };
            checkBoxTarget.Checked = bk.id == TargetClass.GetTargetBookmarkID();      // is this the target?
        }

        public void Bookmark(BookmarkClass bk, string planet, double latitude, double longitude)  // from compass, bookmark at planet/lat/long
        {
            Bookmark(bk);
            SurfaceBookmarks.AddSurfaceLocation(planet, latitude, longitude);
        }

        public void NewRegionBookmark(DateTime timeutc)              // from map, a region bookmark at this time
        {
            this.Text = "Create Region Bookmark".T(EDTx.BookmarkForm_RB);
            textBoxName.Text = "Enter a region name...".T(EDTx.BookmarkForm_RN);
            textBoxName.ClearOnFirstChar = true;
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(timeutc).ToString();
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxX.ReadOnly = textBoxY.ReadOnly = textBoxZ.ReadOnly = false;      // enable editing

            HideEDSM();
            HideTravelNote();   // in order
            HideSurfaceBookmarks();
            buttonDelete.Hide();

            buttonOK.Enabled = ValidateData();
        }

        public void NewSystemBookmark(ISystem system, DateTime timeutc)    // from multipe, create a new system bookmark
        {
            this.Text = "New System Bookmark".T(EDTx.BookmarkForm_SB);
            textBoxName.Text = system.Name;
            textBoxName.ReadOnly = true;
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxName.TextChanged += new System.EventHandler(this.textBox_TextNameChanged);       // we verify the name and see if we can co-ord
            textBoxTravelNote.Text = EliteDangerousCore.DB.SystemNoteClass.GetTextNotesOnSystem(system.Name);
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(timeutc).ToString();
            InitialisePos(system);
            buttonDelete.Hide();
            var edsm = new EDSMClass();
            edsmurl = edsm.GetUrlToSystem(system.Name);
            SurfaceBookmarks.Init(system.Name, historyentry);
            buttonOK.Enabled = true;
        }
                                                                                   // from compass, new system bookmark at position
        public void NewSystemBookmark(ISystem system, DateTime timeutc, string planet, double latitude, double longitude)
        {
            NewSystemBookmark(system, timeutc);
            SurfaceBookmarks.AddSurfaceLocation(planet, latitude, longitude);
        }

        public void NewFreeEntrySystemBookmark(DateTime timeutc)                     // new system bookmark anywhere
        {
            this.Text = "New System Bookmark".T(EDTx.BookmarkForm_NSB);
            textBoxName.Text = "Enter a system name...".T(EDTx.BookmarkForm_ESN);
            textBoxName.ClearOnFirstChar = true;
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxName.TextChanged += new System.EventHandler(this.textBox_TextNameChanged);       // we verify the name and see if we can co-ord
            textBoxName.SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList,true);
            textBoxName.ClearOnFirstChar = true;
            textBoxName.SelectAll();
            textBoxName.Focus();
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(timeutc).ToString();
            buttonDelete.Hide();
            buttonEDSM.Enabled = false;
            buttonOK.Enabled = false;
            textBoxX.ReadOnly = textBoxY.ReadOnly = textBoxZ.ReadOnly = false;      // enable editing
            SurfaceBookmarks.Init("", historyentry);
        }

        private void HideTravelNote() { labelTravelNote.Hide();  textBoxTravelNote.Hide(); ShiftControls(SurfaceBookmarks, textBoxTravelNote); }
        private void HideSurfaceBookmarks() { SurfaceBookmarks.Hide(); ShiftControls(buttonOK, SurfaceBookmarks); }
        private void HideEDSM() { buttonEDSM.Hide(); checkBoxTarget.Left = buttonEDSM.Left; }

        private void ShiftControls(Control bot, Control top)
        {
            int topline = top.Top;
            int delta = bot.Top - topline;

            //System.Diagnostics.Debug.WriteLine("Control bot {0} {1} to {2} {3} delta {4}", bot.Name, bot.Top, top.Name, top.Top, delta);
            foreach (Control c in extPanelScroll.Controls)
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

        private void textBox_TextNumChanged(object sender, EventArgs e)
        {
             buttonOK.Enabled = ValidateData();
        }
        private void textBox_TextNameChanged(object sender, EventArgs e)
        {
            ISystem f = SystemCache.FindSystem(textBoxName.Text, null, false);       // no edsm lookup, too slow to do interactively.
            if (f != null && f.HasCoordinate)
            {
                InitialisePos(f);
                var edsm = new EDSMClass();
                edsmurl = edsm.GetUrlToSystem(f.Name);
                SurfaceBookmarks.Init(f.Name, historyentry);
            }
            else
                textBoxX.Text = textBoxY.Text = textBoxZ.Text = "";

            buttonEDSM.Enabled = buttonOK.Enabled = ValidateData() && f != null;
        }

        private bool ValidateData()
        {
            double xp, yp, zp;
            return (textBoxName.Text.Length > 0 && double.TryParse(textBoxX.Text, out xp) && double.TryParse(textBoxY.Text, out yp) && double.TryParse(textBoxZ.Text, out zp));
        }

        private void buttonEDSM_Click(object sender, EventArgs e)
        {
            if (edsmurl != null)
                BaseUtils.BrowserInfo.LaunchBrowser(edsmurl);
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void InitialisePos(ISystem system)
        {
            textBoxX.Text = system.X.ToString("0.00");
            textBoxY.Text = system.Y.ToString("0.00");
            textBoxZ.Text = system.Z.ToString("0.00");
        }

        private void InitialisePos(double x, double y, double z)
        {
            textBoxX.Text = x.ToString("0.00");
            textBoxY.Text = y.ToString("0.00");
            textBoxZ.Text = z.ToString("0.00");
        }


        #endregion
    }
}
