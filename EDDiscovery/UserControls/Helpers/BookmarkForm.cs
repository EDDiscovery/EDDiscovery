/*
 * Copyright © 2016-2024 EDDiscovery development team
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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class BookmarkForm : ExtendedControls.DraggableForm
    {
        public string StarHeading { get { return textBoxName.Text; } }
        public string Notes { get { return textBoxBookmarkNotes.Text; } }
        public string x { get { return textBoxX.Text; } }
        public string y { get { return textBoxY.Text; } }
        public string z { get { return textBoxZ.Text; } }
        public bool IsTarget { get { return checkBoxTarget.Checked; } }
        public PlanetMarks SurfaceLocations { get { return SurfaceBookmarks.PlanetMarks; } }
        public string Tags { get; private set; }

        public BookmarkForm(HistoryList he, bool enabletargetsetting = false)
        {
            InitializeComponent();

            bool winborder = ExtendedControls.Theme.Current.ApplyDialog(this);
            panelTop.Visible = !winborder;

            BaseUtils.TranslatorMkII.Instance.TranslateControls(this, 3);

            historyentry = he;
            checkBoxTarget.Visible = enabletargetsetting;

            SurfaceBookmarks.TagFilter = "All";
        }

        #region External Initialisation

        // open an existing bookmark, region or system
        public void Bookmark(BookmarkClass bk)
        {
            labelTitle.Text = this.Text = "Update Bookmark".Tx();
            InitialisePos(bk.X, bk.Y, bk.Z);

            if (!bk.IsRegion)       // Star..
            {
                textBoxName.Text = bk.StarName;
                textBoxName.ReadOnly = true; // no change
                textBoxTravelNote.Text = EliteDangerousCore.DB.SystemNoteClass.GetTextNotesOnSystem(bk.StarName);

                SurfaceBookmarks.Display(bk.StarName, historyentry, () => closing, bk.PlanetaryMarks);
            }
            else
            {
                textBoxName.Text = bk.Heading;
                textBoxX.ReadOnly = textBoxY.ReadOnly = textBoxZ.ReadOnly = false;      // enable editing
                HideExternalButtonLinks();
                HideTravelNote();   // in order note
                HideSurfaceBookmarks();
            }

            textBoxBookmarkNotes.Text = bk.Note;
            textBoxBookmarkNotes.CursorToEnd();
            textBoxBookmarkNotes.ScrollToCaret();
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(bk.TimeUTC).ToString();

            Tags = bk.Tags;
            var taglist = EDDConfig.BookmarkTagArray(Tags);
            TagsForm.FillTags(taglist, EDDConfig.Instance.BookmarkTagDictionary, panelTags, panelTags_MouseDown, toolTip);

            buttonOK.Text = "Update".Tx();
            buttonOK.Enabled = true;

            textBoxName.ReturnPressed += (ctrl) => { return true; };
            checkBoxTarget.Checked = bk.ID == TargetClass.GetTargetBookmarkID();      // is this the target?
        }

        // create a new region bookmark
        public void NewRegionBookmark(DateTime timeutc)              // from map, a region bookmark at this time
        {
            labelTitle.Text = this.Text = "Create Region Bookmark".Tx();
            textBoxName.Text = "Enter a region name...".Tx();
            textBoxName.ClearOnFirstChar = true;
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(timeutc).ToString();
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxX.ReadOnly = textBoxY.ReadOnly = textBoxZ.ReadOnly = false;      // enable editing

            HideExternalButtonLinks();
            HideTravelNote();   // in order
            HideSurfaceBookmarks();
            buttonDelete.Hide();

            buttonOK.Enabled = ValidateData();
        }

        // create a new system bookmark from Isystem
        public void NewSystemBookmark(ISystem system, DateTime timeutc)    // from multipe, create a new system bookmark
        {
            labelTitle.Text = this.Text = "New System Bookmark".Tx();
            textBoxName.Text = system.Name;
            textBoxName.ReadOnly = true;
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxName.TextChanged += new System.EventHandler(this.textBox_TextNameChanged);       // we verify the name and see if we can co-ord
            textBoxTravelNote.Text = EliteDangerousCore.DB.SystemNoteClass.GetTextNotesOnSystem(system.Name);
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(timeutc).ToString();
            InitialisePos(system);
            buttonDelete.Hide();
            SurfaceBookmarks.Display(system.Name, historyentry, () => closing);
            buttonOK.Enabled = true;
        }

        // create a new planet mark on this existing bookmark
        public void NewPlanetBookmark(BookmarkClass bk, string planet, double latitude, double longitude)  // from compass, bookmark at planet/lat/long
        {
            Bookmark(bk);
            SurfaceBookmarks.AddSurfaceLocation(planet, latitude, longitude);
        }

        // from compass, new system bookmark and planet bookmark
        public void NewSystemBookmark(ISystem system, DateTime timeutc, string planet, double latitude, double longitude)
        {
            NewSystemBookmark(system, timeutc);
            SurfaceBookmarks.AddSurfaceLocation(planet, latitude, longitude);
        }

        // New anywhere bookmark
        public void NewFreeEntrySystemBookmark(DateTime timeutc)                     // new system bookmark anywhere
        {
            labelTitle.Text = this.Text = "New System Bookmark".Tx();
            textBoxName.Text = "Enter a system name...".Tx();
            textBoxName.ClearOnFirstChar = true;
            textBoxName.ReturnPressed += (ctrl) => { return true; };
            textBoxName.TextChanged += new System.EventHandler(this.textBox_TextNameChanged);       // we verify the name and see if we can co-ord
            textBoxName.SetAutoCompletor(SystemCache.ReturnSystemAutoCompleteList, true);
            textBoxName.ClearOnFirstChar = true;
            textBoxName.SelectAll();
            textBoxName.Focus();
            textBoxTime.Text = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(timeutc).ToString();
            buttonDelete.Hide();
            HideExternalButtonLinks();
            buttonOK.Enabled = false;
            textBoxX.ReadOnly = textBoxY.ReadOnly = textBoxZ.ReadOnly = false;      // enable editing
            SurfaceBookmarks.Display("", historyentry, () => closing);
        }

        private void HideTravelNote() 
        { 
            labelTravelNote.Visible = textBoxTravelNote.Visible = false;
            this.ShiftControls(textBoxTravelNote.Top, new Point(0, SurfaceBookmarks.Top - textBoxTravelNote.Top), true);
        }
        private void HideSurfaceBookmarks()
        {
            SurfaceBookmarks.Visible = false;
            this.ShiftControls(SurfaceBookmarks.Top, new Point(0, SurfaceBookmarks.Height), true);
        }
        private void HideExternalButtonLinks() 
        {
            extButtonInaraSystem.Visible = extButtonEDSMSystem.Visible = extButtonSpanshSystem.Visible = false;
            checkBoxTarget.Left = extButtonEDSMSystem.Left; 
        }

        #endregion

        #region UI
        private void panelTags_MouseDown(object sender, MouseEventArgs e)
        {
            TagsForm.EditTags(this.FindForm(),
                                        EDDConfig.Instance.BookmarkTagDictionary, "", Tags,
                                        panelTags.PointToScreen(new Point(0, 0)),
                                        TagsChanged, null, EDDConfig.TagSplitStringBK);
        }

        private void TagsChanged(string newtags, Object tag)
        {
            Tags = newtags;
            var taglist = EDDConfig.BookmarkTagArray(Tags);
            TagsForm.FillTags(taglist, EDDConfig.Instance.BookmarkTagDictionary, panelTags, panelTags_MouseDown, toolTip);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void extButtonDrawnClose_Click(object sender, EventArgs e)
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
            ISystem f = SystemCache.FindSystem(textBoxName.Text, null);       // no web lookup, too slow to do interactively.
            if (f != null && f.HasCoordinate)
            {
                InitialisePos(f);
                SurfaceBookmarks.Display(f.Name, historyentry, () => closing);
            }
            else
                textBoxX.Text = textBoxY.Text = textBoxZ.Text = "";

            buttonOK.Enabled = ValidateData() && f != null;
        }

        private bool ValidateData()
        {
            double xp, yp, zp;
            return (textBoxName.Text.Length > 0 && double.TryParse(textBoxX.Text, out xp) && double.TryParse(textBoxY.Text, out yp) && double.TryParse(textBoxZ.Text, out zp));
        }

        private void extButtonEDSMSystem_Click(object sender, EventArgs e)
        {
            var edsm = new EDSMClass();
            string edsmurl = edsm.GetUrlToSystem(textBoxName.Text);
            if (edsmurl != null)
                BaseUtils.BrowserInfo.LaunchBrowser(edsmurl);
        }

        private void extButtonInaraSystem_Click(object sender, EventArgs e)
        {
            var ir = new EliteDangerousCore.Inara.InaraClass();
            ir.LaunchBrowserForSystem(textBoxName.Text);
        }

        private void extButtonSpanshSystem_Click(object sender, EventArgs e)
        {
            EliteDangerousCore.Spansh.SpanshClass.LaunchBrowserForSystem(textBoxName.Text);
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

        private void extPanelScroll_MouseDown(object sender, MouseEventArgs e)
        {
            OnCaptionMouseDown((Control)sender, e);
        }

        private void extPanelScroll_MouseUp(object sender, MouseEventArgs e)
        { 
            OnCaptionMouseUp((Control) sender, e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            closing = true;
        }

        private bool closing = false;
        private HistoryList historyentry;

        #endregion


    }
}
