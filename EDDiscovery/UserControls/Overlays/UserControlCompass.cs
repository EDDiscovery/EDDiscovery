/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Math;

namespace EDDiscovery.UserControls
{
    public partial class UserControlCompass : UserControlCommonBase
    {
        string dbLatSave = "LatTarget";
        string dbLongSave = "LongTarget";
        string dbHideSave = "AutoHide";

        double? latitude = null;
        double? longitude = null;
        double? heading = null;
        double? altitude = null;
        double? bodyRadius = null;
        string lastradiusbody = null;
        bool autoHideTargetCoords = false;
        HistoryEntry last_he;
        BookmarkClass currentBookmark;
        bool externallyForcedBookmark = false;
        PlanetMarks.Location externalLocation;
        string externalLocationName;
        EliteDangerousCore.UIEvents.UIGUIFocus.Focus uistate = EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus;

        #region Init

        public UserControlCompass()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "Compass";
            discoveryform.OnNewEntry += OnNewEntry;
            discoveryform.OnNewUIEvent += OnNewUIEvent;
            discoveryform.OnHistoryChange += Discoveryform_OnHistoryChange;
            numberBoxTargetLatitude.ValueNoChange = GetSetting(dbLatSave, 0.0);     // note need to explicity state its double
            numberBoxTargetLongitude.ValueNoChange = GetSetting(dbLongSave, 0.0);
            autoHideTargetCoords = GetSetting(dbHideSave, false);
            checkBoxHideTransparent.Checked = autoHideTargetCoords;
            comboBoxBookmarks.Text = "";
            GlobalBookMarkList.Instance.OnBookmarkChange += GlobalBookMarkList_OnBookmarkChange;
            compassControl.SlewRateDegreesSec = 40;
            compassControl.AutoSetStencilTicks = true;
            buttonNewBookmark.Enabled = false;

            checkBoxHideTransparent.Visible = IsFloatingWindow;

            var enumlist = new Enum[] { EDTx.UserControlCompass_labelBookmark, EDTx.UserControlCompass_labelTargetLat, EDTx.UserControlCompass_checkBoxHideTransparent };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
        }

        public override void Closing()
        {
            PutSetting(dbLatSave, numberBoxTargetLatitude.Value);
            PutSetting(dbLongSave, numberBoxTargetLongitude.Value);
            PutSetting(dbHideSave, autoHideTargetCoords);
            discoveryform.OnNewEntry -= OnNewEntry;
            discoveryform.OnNewUIEvent -= OnNewUIEvent;
            discoveryform.OnHistoryChange -= Discoveryform_OnHistoryChange;
            GlobalBookMarkList.Instance.OnBookmarkChange -= GlobalBookMarkList_OnBookmarkChange;
        }

        public override bool SupportTransparency { get { return true; } }
        public override bool DefaultTransparent { get { return true; } }
        public override void SetTransparency(bool on, Color curbackcol)
        {
            numberBoxTargetLatitude.BackColor = numberBoxTargetLongitude.BackColor = curbackcol;
            numberBoxTargetLatitude.ControlBackground = numberBoxTargetLongitude.ControlBackground = curbackcol;
            labelTargetLat.TextBackColor = curbackcol;
            labelBookmark.TextBackColor = curbackcol;
            flowLayoutPanelTop.BackColor = curbackcol;
            flowLayoutPanelBookmarks.BackColor = curbackcol;
            checkBoxHideTransparent.BackColor = curbackcol;
            comboBoxBookmarks.DisableBackgroundDisabledShadingGradient = on;
            comboBoxBookmarks.BackColor = curbackcol;
            buttonNewBookmark.BackColor = curbackcol;
            BackColor = curbackcol;

            Color fore = on ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            compassControl.ForeColor = fore;
            compassControl.StencilColor = fore;
            compassControl.CentreTickColor = fore.Multiply(1.2F);
            compassControl.BugColor = fore.Multiply(0.8F);
            compassControl.BackColor = on ? Color.Transparent : BackColor;
            compassControl.Font = ExtendedControls.Theme.Current.GetScaledFont(0.8f);

            if (autoHideTargetCoords)
            {
                flowLayoutPanelBookmarks.Visible = flowLayoutPanelTop.Visible = !on;
            }

            SetCompassVisibility(on);
        }

        private void SetCompassVisibility(bool transparent)
        {
            if (uistate != EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus && transparent)
            {
                compassControl.Visible = false;
                return;
            }
            else if (!compassControl.Visible)
            {
                compassControl.Visible = true;
            }
        }

        #endregion

        #region Display

        private void Discoveryform_OnHistoryChange(HistoryList obj) // need to handle this in case commander changed..
        {
            last_he = discoveryform.history.GetLast;
            currentBookmark = null;
            PopulateBookmarkCombo();
            OnNewEntry(last_he, discoveryform.history);
        }

        public override void InitialDisplay()       // on start up, this will have an empty history
        {
            last_he = discoveryform.history.GetLast;
            PopulateBookmarkCombo();
            DisplayCompass();
        }


        private async void OnNewEntry(HistoryEntry he, HistoryList hl)
        {
            last_he = he;
            if (last_he != null)
            {
                if ( bodyRadius == null || lastradiusbody != he.WhereAmI)       // try and get radius, this is cleared on target selection
                { 
                    StarScan.SystemNode last_sn = await discoveryform.history.StarScan.FindSystemAsync(he.System, false);       // find scan if we have one
                    JournalScan sd = last_sn?.Find(he.WhereAmI)?.ScanData;  // find body scan data if present, null if not
                    bodyRadius = sd?.nRadius;
                    if (bodyRadius.HasValue)
                    {
                        lastradiusbody = he.WhereAmI;
                        System.Diagnostics.Debug.WriteLine("Compass Radius Set " + lastradiusbody + " " + bodyRadius.Value);
                    }
                }

                switch (he.journalEntry.EventTypeID)
                {
                    case JournalTypeEnum.Screenshot:
                        JournalScreenshot js = he.journalEntry as JournalScreenshot;
                        latitude = js.nLatitude;
                        longitude = js.nLongitude;
                        altitude = js.nAltitude;
                        break;
                    case JournalTypeEnum.Touchdown:
                        JournalTouchdown jt = he.journalEntry as JournalTouchdown;
                        if (jt.PlayerControlled.HasValue && jt.PlayerControlled.Value)
                        {
                            latitude = jt.Latitude;
                            longitude = jt.Longitude;
                            altitude = 0;
                        }
                        break;
                    case JournalTypeEnum.Location:
                        JournalLocation jl = he.journalEntry as JournalLocation;
                        latitude = jl.Latitude;
                        longitude = jl.Longitude;
                        altitude = null;
                        break;
                    case JournalTypeEnum.Liftoff:
                        JournalLiftoff jlo = he.journalEntry as JournalLiftoff;
                        if (jlo.PlayerControlled.HasValue && jlo.PlayerControlled.Value)
                        {
                            latitude = jlo.Latitude;
                            longitude = jlo.Longitude;
                            altitude = 0;
                        }
                        break;
                    case JournalTypeEnum.LeaveBody:
                        latitude = null;
                        longitude = null;
                        altitude = null;
                        break;
                    case JournalTypeEnum.FSDJump:       // to allow us to do PopulateBookmark..
                    case JournalTypeEnum.CarrierJump:     
                        break;
                    default:
                        return;
                }

                PopulateBookmarkCombo();
                DisplayCompass();
            }
        }

        private void OnNewUIEvent(UIEvent uievent)       // UI event in, see if we want to hide.  UI events come before any onNew
        {
            EliteDangerousCore.UIEvents.UIPosition up = uievent as EliteDangerousCore.UIEvents.UIPosition;

            if (up != null)
            {
                if (up.Location.ValidPosition)
                {
                    latitude = up.Location.Latitude;
                    longitude = up.Location.Longitude;
                    altitude = up.Location.Altitude;
                    heading = up.Heading;
                    DisplayCompass();
                }
                else
                {
                    latitude = longitude = heading = altitude = null;
                    DisplayCompass();
                }
            }

            var gui = uievent as EliteDangerousCore.UIEvents.UIGUIFocus;

            if ( gui != null )
            {
                uistate = gui.GUIFocus;
                DisplayCompass();
            }
        }


        private void DisplayCompass()
        {
            SetCompassVisibility(IsTransparentModeOn);

            if (compassControl.Visible)
            {
                double? targetlat = numberBoxTargetLatitude.Value;
                double? targetlong = numberBoxTargetLongitude.Value;

                double? targetDistance = CalculateDistance(targetlat, targetlong);
                double? targetSlope = CalculateGlideslope(targetDistance);

                if (targetDistance.HasValue)
                {
                    if (targetDistance.Value < 1000)
                        compassControl.DistanceFormat = "{0:0.#}m";
                    else if (targetDistance.Value < 1000000)
                    {
                        targetDistance = targetDistance.Value / 1000;
                        compassControl.DistanceFormat = "{0:0.#}km";
                    }
                    else
                    {
                        targetDistance = targetDistance.Value / 1000000;
                        compassControl.DistanceFormat = "{0:0.#}Mm";
                    }
                }

                if (targetSlope.HasValue)
                {
                    compassControl.GlideSlope = targetSlope.Value;
                }
                else
                {
                    compassControl.GlideSlope = double.NaN;
                }

                double? targetBearing = CalculateBearing(targetlat, targetlong);

                try
                {
                    compassControl.Set(heading.HasValue ? heading.Value : double.NaN,
                                    targetBearing.HasValue ? targetBearing.Value : double.NaN,
                                    targetDistance.HasValue ? targetDistance.Value : double.NaN, true);
                }
                catch { }       // unlikely but Status.JSON could send duff values, trap out the exception on bad values
            }
        }

        private double? CalculateBearing(double? targetLat, double? targetLong)
        {
            if (!(latitude.HasValue && longitude.HasValue && targetLat.HasValue && targetLong.HasValue))
                return null;
            // turn degrees to radians
            double long1 = longitude.Value * PI / 180;
            double lat1 = latitude.Value * PI / 180;
            double long2 = targetLong.Value * PI / 180;
            double lat2 = targetLat.Value * PI / 180;

            double y = Sin(long2 - long1) * Cos(lat2);
            double x = Cos(lat1) * Sin(lat2) -
                        Sin(lat1) * Cos(lat2) * Cos(long2 - long1);
            
            // back to degrees again
            double bearing = Atan2(y, x) / PI * 180;

            //bearing in game HUD is 0-360, not -180 to 180
            return bearing > 0 ? bearing : 360 + bearing;
        }

        private double? CalculateDistance(double? targetLat, double? targetLong)
        {
            if (!(latitude.HasValue && longitude.HasValue && targetLat.HasValue && targetLong.HasValue && bodyRadius.HasValue))
                return null;

            double lat1 = latitude.Value * PI / 180;
            double lat2 = targetLat.Value * PI / 180;
            double deltaLong = (targetLong.Value - longitude.Value) * PI / 180;
            double deltaLat = (targetLat.Value - latitude.Value) * PI / 180;

            double a = Sin(deltaLat / 2) * Sin(deltaLat / 2) + Cos(lat1) * Cos(lat2) * Sin(deltaLong / 2) * Sin(deltaLong / 2);
            double c = 2 * Atan2(Sqrt(a), Sqrt(1 - a));

            return bodyRadius.Value * c;
        }

        private double? CalculateGlideslope(double? distance)
        {
            if (!(distance.HasValue && altitude.HasValue && bodyRadius.HasValue))
                return null;

            if (altitude.Value < 10000 || distance.Value < 10000) // Don't return a slope if less than 10km out or below 10km
                return null;

            double theta = distance.Value / bodyRadius.Value;
            double rad = 1 + altitude.Value / bodyRadius.Value;
            double dist2 = 1 + rad * rad - 2 * rad * Cos(theta);

            // a = radius, b = distance, c = altitude + radius
            // c^2 = a^2 + b^2 - 2.a.b.cos(C) -> cos(C) = (a^2 + b^2 - c^2) / (2.a.b)
            double sintgtslope = (1 + dist2 - rad * rad) / (2 * Sqrt(dist2));

            if (sintgtslope >= 0)  // Don't return a slope if it would be > 0 deg at the target (ie. we are facing backwards)
                return null;

            // a = altitude + radius, b = distance, c = radius
            double slope = -Asin((rad * rad + dist2 - 1) / (2 * rad * Sqrt(dist2))) * 180 / PI;
            return slope;
        }

        private void PopulateBookmarkCombo()
        {
            if (last_he == null)
            {
                buttonNewBookmark.Enabled = false;
                return;
            }
            buttonNewBookmark.Enabled = true;
            if (currentBookmark != null && currentBookmark.StarName == last_he.System.Name)
            {
                return;
            }
            string selection = externallyForcedBookmark ? externalLocationName : comboBoxBookmarks.Text;
            comboBoxBookmarks.Items.Clear();
            currentBookmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(last_he.System.Name);
            if (currentBookmark != null)
            {
                List<PlanetMarks.Planet> planetMarks = currentBookmark.PlanetaryMarks?.Planets;

                if (heading.HasValue)
                {
                    // orbital flight or landed, just do this body
                    labelBookmark.Text = "Planet Bookmarks";
                    planetMarks = planetMarks?.Where(p => p.Name == last_he.WhereAmI)?.ToList();
                }
                else
                {
                    // add whole system
                    labelBookmark.Text = "System Bookmarks";
                }

                if (planetMarks != null)
                {
                    foreach (PlanetMarks.Planet pl in planetMarks)
                    {
                        if (pl.Locations != null && pl.Locations.Any())
                        {
                            foreach (PlanetMarks.Location loc in pl.Locations.OrderBy(l => l.Name))
                            {
                                comboBoxBookmarks.Items.Add($"{loc.Name} ({pl.Name})");
                            }
                        }
                    }
                }
            }
            if (externallyForcedBookmark && !comboBoxBookmarks.Items.Contains(externalLocationName))
            {
                comboBoxBookmarks.Items.Add(externalLocationName);
            }
            if (!String.IsNullOrEmpty(selection) && comboBoxBookmarks.Items.Contains(selection))
            {
                comboBoxBookmarks.Text = selection;
            }
            else
            {
                comboBoxBookmarks.Text = "";
            }
        }


        #endregion

        private void checkBoxHideTransparent_CheckedChanged(object sender, EventArgs e)
        {
            autoHideTargetCoords = ((ExtCheckBox)sender).Checked;
           // SetTransparency(IsTransparent, BackColor);
        }

        private void comboBoxBookmarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (currentBookmark == null || currentBookmark.PlanetaryMarks == null || currentBookmark.PlanetaryMarks.Planets == null) return;
            foreach (PlanetMarks.Planet pl in currentBookmark.PlanetaryMarks.Planets)
            {
                if (pl.Locations != null && pl.Locations.Any())
                {
                    foreach (PlanetMarks.Location loc in pl.Locations.OrderBy(l => l.Name))
                    {
                        if ($"{loc.Name} ({pl.Name})" == comboBoxBookmarks.Text)
                        {
                            numberBoxTargetLatitude.Value = loc.Latitude;
                            numberBoxTargetLongitude.Value = loc.Longitude;
                            bodyRadius = null;

                            if (externallyForcedBookmark && comboBoxBookmarks.Text != externalLocationName)
                            {
                                comboBoxBookmarks.Items.Remove(externalLocationName);
                                externallyForcedBookmark = false;
                                externalLocationName = "";
                                externalLocation = null;
                            }

                            DisplayCompass();
                            return;
                        }
                    }
                }
            }
        }

        private void buttonNewBookmark_Click(object sender, EventArgs e)
        {
            if (last_he == null)
                return;

            BookmarkForm frm = new BookmarkForm(discoveryform.history);
            DateTime timeutc = DateTime.UtcNow;
            if (currentBookmark == null)
            {
                if (latitude.HasValue)
                {
                    frm.NewSystemBookmark(last_he.System,timeutc, last_he.WhereAmI, latitude.Value, longitude.Value);
                }
                else
                {
                    frm.NewSystemBookmark(last_he.System, timeutc);
                }
            }
            else
            {
                if (latitude.HasValue)
                {
                    frm.Bookmark(currentBookmark, last_he.WhereAmI, latitude.Value, longitude.Value);
                }
                else
                {
                    frm.Bookmark(currentBookmark);
                }
            }

            frm.StartPosition = FormStartPosition.CenterScreen;
            DialogResult dr = frm.ShowDialog();
            if (dr == DialogResult.OK)
            {
                currentBookmark = GlobalBookMarkList.Instance.AddOrUpdateBookmark(currentBookmark, true, frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z), timeutc, frm.Notes, frm.SurfaceLocations);
            }
            if (dr == DialogResult.Abort)
            {
                GlobalBookMarkList.Instance.Delete(currentBookmark);
                currentBookmark = null;
            }

            PopulateBookmarkCombo();
            DisplayCompass();
        }

        private void GlobalBookMarkList_OnBookmarkChange(BookmarkClass bk, bool deleted)
        {
            if (currentBookmark != null && currentBookmark.id == bk.id)
                currentBookmark = null;

            PopulateBookmarkCombo();
            DisplayCompass();
        }


        public void SetSurfaceBookmark(BookmarkClass bk, string planetName, string locName)
        {
            externallyForcedBookmark = true;
            externalLocation = bk.PlanetaryMarks.Planets.Where(pl => pl.Name == planetName).FirstOrDefault()?.Locations.Where(l => l.Name == locName).FirstOrDefault();
            if (externalLocation != null)
            {
                externalLocationName = $"{locName} ({planetName})";
                numberBoxTargetLatitude.Value = externalLocation.Latitude;
                numberBoxTargetLongitude.Value = externalLocation.Longitude;
            }

            PopulateBookmarkCombo();
            DisplayCompass();
        }

        private void numberBoxTargetLatitude_ValueChanged(object sender, EventArgs e)
        {
            DisplayCompass();
        }

        private void numberBoxTargetLongitude_ValueChanged(object sender, EventArgs e)
        {
            DisplayCompass();
        }
    }
}
