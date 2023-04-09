/*
 * Copyright © 2016 - 2023 EDDiscovery development team
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
using EDDiscovery.Forms;
using EliteDangerousCore;
using EliteDangerousCore.DB;
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
        private string dbLatSave = "LatTarget";
        private string dbLongSave = "LongTarget";
        private string dbFont = "Font";

        EliteDangerousCore.UIEvents.UIPosition position = new EliteDangerousCore.UIEvents.UIPosition();

        EliteDangerousCore.UIEvents.UIMode elitemode;
        private bool intransparent = false;
        private EliteDangerousCore.UIEvents.UIGUIFocus.Focus uistate;

        private Font displayfont;

        private ISystem current_sys;   // current system we are in, may be null
        private string current_body;    // current body

        private string sentbookmarktext;    // Another panel has sent a bookmark, and its position, add it to the combobox and allow selection
        private EliteDangerousCore.UIEvents.UIPosition.Position sentposition;

        #region Init

        public UserControlCompass()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "Compass";

            numberBoxTargetLatitude.ValueNoChange = GetSetting(dbLatSave, 0.0);     // note need to explicity state its double
            numberBoxTargetLongitude.ValueNoChange = GetSetting(dbLongSave, 0.0);
            comboBoxBookmarks.Text = "";
            compassControl.SlewRateDegreesSec = 40;
            compassControl.AutoSetStencilTicks = true;
            compassControl.TextBandRatioToFont = 1;
            buttonNewBookmark.Enabled = false;
            var enumlist = new Enum[] { EDTx.UserControlCompass_labelTargetLat };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);

            PopulateCtrlList();

            DiscoveryForm.OnNewEntry += OnNewEntry;
            DiscoveryForm.OnNewUIEvent += OnNewUIEvent;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            GlobalBookMarkList.Instance.OnBookmarkChange += GlobalBookMarkList_OnBookmarkChange;

            // new! april23 pick up last major mode..
            elitemode = new EliteDangerousCore.UIEvents.UIMode(DiscoveryForm.UIOverallStatus.Mode, DiscoveryForm.UIOverallStatus.MajorMode);
            uistate = DiscoveryForm.UIOverallStatus.Focus;
        }

        public override void LoadLayout()
        {
            base.LoadLayout();
            displayfont = FontHelpers.GetFont(GetSetting(dbFont, ""), null);        // null if not set, keep selection in displayfont
            if (displayfont != null)
                compassControl.Font = displayfont;
        }

        public override void Closing()
        {
            PutSetting(dbLatSave, numberBoxTargetLatitude.Value);
            PutSetting(dbLongSave, numberBoxTargetLongitude.Value);
            DiscoveryForm.OnNewEntry -= OnNewEntry;
            DiscoveryForm.OnNewUIEvent -= OnNewUIEvent;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            GlobalBookMarkList.Instance.OnBookmarkChange -= GlobalBookMarkList_OnBookmarkChange;
        }

        public override bool SupportTransparency { get { return true; } }
        public override bool DefaultTransparent { get { return true; } }
        public override void SetTransparency(bool on, Color curbackcol)
        {
            BackColor = curbackcol;

            numberBoxTargetLatitude.BackColor = numberBoxTargetLongitude.BackColor = curbackcol;
            numberBoxTargetLatitude.ControlBackground = numberBoxTargetLongitude.ControlBackground = curbackcol;
            labelTargetLat.TextBackColor = curbackcol;
            flowLayoutPanelTop.BackColor = curbackcol;
            comboBoxBookmarks.DisableBackgroundDisabledShadingGradient = on;
            comboBoxBookmarks.BackColor = curbackcol;
            buttonNewBookmark.BackColor = curbackcol;

            Color fore = on ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            compassControl.ForeColor = fore;
            compassControl.StencilColor = fore;
            compassControl.CentreTickColor = fore.Multiply(1.2F);
            compassControl.BugColor = fore.Multiply(0.8F);
            compassControl.BackColor = on ? Color.Transparent : BackColor;
            compassControl.Font = ExtendedControls.Theme.Current.GetScaledFont(1f);
            flowLayoutPanelTop.Visible = !on;
            compassControl.Font = displayfont ?? this.Font;     // due to themeing, set control font again

            intransparent = on;
            SetCompassVisibility();
        }

        // UI event in, accumulate state information
        private void OnNewUIEvent(UIEvent uievent)       
        {
            EliteDangerousCore.UIEvents.UIMode mode = uievent as EliteDangerousCore.UIEvents.UIMode;
            if ( mode != null )
            {
                elitemode = mode;
                System.Diagnostics.Debug.WriteLine($"Compass Elitemode {elitemode.MajorMode} {elitemode.Mode}");
                SetCompassVisibility();
            }

            EliteDangerousCore.UIEvents.UIPosition pos = uievent as EliteDangerousCore.UIEvents.UIPosition;

            if (pos != null)
            {
                position = pos;

                if (position.ValidBodyName && position.BodyName != current_body )       // changed body name (this is the full name)
                {
                    current_body = position.BodyName;
                    PopulateBookmarkCombo();
                }

                System.Diagnostics.Debug.WriteLine($"Compass lat {pos.Location.Latitude}, {pos.Location.Longitude} A {pos.Location.Altitude} H {pos.Heading} R {pos.PlanetRadius} BN {pos.BodyName}");

                UpdateCompass();
                SetCompassVisibility();
            }

            var gui = uievent as EliteDangerousCore.UIEvents.UIGUIFocus;

            if ( gui != null )
            {
                uistate = gui.GUIFocus;
                SetCompassVisibility();
            }
        }

        private void Discoveryform_OnHistoryChange() // need to handle this in case commander changed..
        {
            var lasthe = DiscoveryForm.History.GetLast;
            current_sys = lasthe?.System;
            System.Diagnostics.Debug.WriteLine($"Compass start with {current_sys?.Name}");
            PopulateBookmarkCombo();
            UpdateCompass();
            SetCompassVisibility();
        }

        public override void InitialDisplay()       // on start up, this will have an empty history
        {
            Discoveryform_OnHistoryChange();
        }

        private void OnNewEntry(HistoryEntry he)
        {
            if (current_sys == null || current_sys.Name != he.System.Name)       // changed system
            {
                current_sys = he.System;
                current_body = null;        // don't know it now
                PopulateBookmarkCombo();
            }
        }

        public override bool PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            if (actionobj is SetCompassTarget sct)
            {
                if (!comboBoxBookmarks.Items.Contains(sct.Name))        // only add to combo if not in list..
                {
                    sentbookmarktext = sct.Name;
                    sentposition = new EliteDangerousCore.UIEvents.UIPosition.Position() { Altitude = 0, AltitudeFromAverageRadius = false, Latitude = sct.Latitude, Longitude = sct.Longitude };
                    PopulateBookmarkCombo();
                }
                comboBoxBookmarks.SelectedItem = sct.Name;      // must be in list, so select and set the compass
                return true;
            }
            return false;
        }

        #endregion

        #region Display

        // we determine visibility from the stored flags.
        private void SetCompassVisibility()
        {
            bool visible = true;

            if (intransparent && IsSet(CtrlList.autohide))       // autohide turns off when transparent And..
            {
                if (uistate != EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus)    // not on main screen
                {
                    visible = false;
                }
                            // if in mainship, or srv, or we are on foot planet, we can show
                else if (elitemode.InFlight ||
                         elitemode.Mode == EliteDangerousCore.UIEvents.UIMode.ModeType.SRV ||
                         elitemode.Mode == EliteDangerousCore.UIEvents.UIMode.ModeType.OnFootPlanet ||
                         elitemode.Mode == EliteDangerousCore.UIEvents.UIMode.ModeType.OnFootInstallationInside
                    )
                {
                }
                else
                    visible = false;    // else off
            }

            if (intransparent && IsSet(CtrlList.hidewithnolatlong) && !position.Location.ValidPosition)     // if hide if no lat/long..
            {
                visible = false;
            }

            if ( compassControl.Visible != visible)     // not sure this makes a difference, but..
                compassControl.Visible = visible;
        }

        // update the compass, given the info we have
        // done even if invisible so it will be right when reshown
        private void UpdateCompass()
        {
            double targetlat = numberBoxTargetLatitude.Value;
            double targetlong = numberBoxTargetLongitude.Value;

            double? targetDistance = CalculateDistance(targetlat, targetlong);

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

            double? targetSlope = CalculateGlideslope(targetDistance);
            compassControl.GlideSlope = targetSlope.HasValue ? targetSlope.Value : double.NaN;

            double? targetBearing = CalculateBearing(targetlat, targetlong);

            try
            {
                compassControl.Set(position.ValidHeading ? position.Heading : double.NaN,
                                targetBearing.HasValue ? targetBearing.Value : double.NaN,
                                targetDistance.HasValue ? targetDistance.Value : double.NaN, true);
                // compassControl.Set(120, 180, 20.2, true);
            }
            catch (Exception ex)   // unlikely but Status.JSON could send duff values, trap out the exception on bad values
            {
                System.Diagnostics.Debug.WriteLine($"Compass exception {ex}");
            }       
        }

        private double? CalculateBearing(double targetLat, double targetLong)
        {
            if (!position.Location.ValidPosition)
                return null;

            // turn degrees to radians
            double long1 = position.Location.Longitude * PI / 180;
            double lat1 = position.Location.Latitude * PI / 180;
            double long2 = targetLong * PI / 180;
            double lat2 = targetLat * PI / 180;

            double y = Sin(long2 - long1) * Cos(lat2);
            double x = Cos(lat1) * Sin(lat2) -
                        Sin(lat1) * Cos(lat2) * Cos(long2 - long1);
            
            // back to degrees again
            double bearing = Atan2(y, x) / PI * 180;

            //bearing in game HUD is 0-360, not -180 to 180
            return bearing > 0 ? bearing : 360 + bearing;
        }

        private double? CalculateDistance(double targetLat, double targetLong)
        {
            if (!position.Location.ValidPosition || !position.ValidRadius)       // must have lat/long and radius
                return null;

            double lat1 = position.Location.Latitude * PI / 180;
            double lat2 = targetLat * PI / 180;
            double deltaLong = (targetLong - position.Location.Longitude) * PI / 180;
            double deltaLat = (targetLat - position.Location.Latitude) * PI / 180;

            double a = Sin(deltaLat / 2) * Sin(deltaLat / 2) + Cos(lat1) * Cos(lat2) * Sin(deltaLong / 2) * Sin(deltaLong / 2);
            double c = 2 * Atan2(Sqrt(a), Sqrt(1 - a));

            return position.PlanetRadius * c;
        }

        private double? CalculateGlideslope(double? distance)
        {
            if (!distance.HasValue || !position.Location.ValidAltitude || !position.ValidRadius)
                return null;

            if (position.Location.Altitude < 10000 || distance.Value < 10000) // Don't return a slope if less than 10km out or below 10km
                return null;

            double theta = distance.Value / position.PlanetRadius;
            double rad = 1 + position.Location.Altitude / position.PlanetRadius;
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

        #endregion

        #region UI

        bool preventbookmarkcomboreentry = false;
        List<EliteDangerousCore.UIEvents.UIPosition.Position> comboboxpositions = new List<EliteDangerousCore.UIEvents.UIPosition.Position>(); // holds position for entries

        private void PopulateBookmarkCombo()
        {
            preventbookmarkcomboreentry = true;
            //string curselection = externallyForcedBookmark ? externalLocationName : comboBoxBookmarks.Text; 
            string curselection = comboBoxBookmarks.Text; 
            comboBoxBookmarks.Items.Clear();
            comboboxpositions.Clear();

            if (current_sys != null)
            {
                var sysbookmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(current_sys.Name);
                if (sysbookmark != null)
                {
                    List<PlanetMarks.Planet> planetMarks = sysbookmark.PlanetaryMarks?.Planets;

                    if (position.ValidBodyName)
                    {
                        string planetname = position.BodyName.ReplaceIfStartsWith(current_sys.Name, "");
                        System.Diagnostics.Debug.WriteLine($"Compass Combobox check for planet {planetname}");
                        planetMarks = planetMarks?.Where(p => p.Name.EqualsIIC(planetname))?.ToList();
                    }

                    if (planetMarks != null)
                    {
                        foreach (PlanetMarks.Planet pl in planetMarks)
                        {

                            if (pl.Locations != null && pl.Locations.Any())
                            {
                                foreach (PlanetMarks.Location loc in pl.Locations.OrderBy(l => l.Name))
                                {
                                    System.Diagnostics.Debug.WriteLine($"Compass Combobox Add {pl.Name}: {loc.Name}");
                                    comboBoxBookmarks.Items.Add($"{pl.Name}: {loc.Name}");
                                    comboboxpositions.Add(new EliteDangerousCore.UIEvents.UIPosition.Position() { Latitude = loc.Latitude, Longitude = loc.Longitude });
                                }
                            }
                        }
                    }
                }

                var sysnode = DiscoveryForm.History.StarScan.FindSystemSynchronous(current_sys, false);     // not edsm, so no delay

                if ( sysnode != null)
                {
                    var scannode = sysnode.Find(position.BodyName);
                    if ( scannode != null)
                    {
                        //System.Diagnostics.Debug.WriteLine($"Compass Found scannode for {position.BodyName}");
                        foreach(var sf in scannode.SurfaceFeatures.EmptyIfNull())
                        {
                            if (sf.HasLatLong)  // only want positions
                            {
                                System.Diagnostics.Debug.WriteLine($"Compass Combobox Add {sf.Name_Localised}");
                                comboBoxBookmarks.Items.Add($"{sf.Name_Localised} @ {sf.Latitude.Value:0.####}, {sf.Longitude.Value:0.####}");
                                comboboxpositions.Add(new EliteDangerousCore.UIEvents.UIPosition.Position() { Latitude = sf.Latitude.Value, Longitude = sf.Longitude.Value });
                            }
                        }

                    }
                }

                if ( sentbookmarktext.HasChars())       // add an externally given bookmark to our list
                {
                    comboBoxBookmarks.Items.Add(sentbookmarktext);
                    comboboxpositions.Add(sentposition);
                }

                if (curselection.HasChars() && comboBoxBookmarks.Items.Contains(curselection))
                {
                    comboBoxBookmarks.Text = curselection;
                }

                comboBoxBookmarks.Invalidate();     // items list has changed, invalidate
                buttonNewBookmark.Enabled = true;
            }
            else
            {
                buttonNewBookmark.Enabled = false;
            }
            preventbookmarkcomboreentry = false;
        }

        private void comboBoxBookmarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( !preventbookmarkcomboreentry)
            {
                var pos = comboboxpositions[comboBoxBookmarks.SelectedIndex];
                numberBoxTargetLatitude.Value = pos.Latitude;       // changing these cause the number box change to fire, updating the compass
                numberBoxTargetLongitude.Value = pos.Longitude;
            }
        }

        private void buttonNewBookmark_Click(object sender, EventArgs e)
        {
            if (current_sys != null)
            {
                BookmarkForm frm = new BookmarkForm(DiscoveryForm.History);
                DateTime datetimeutc = DateTime.UtcNow;

                var sysbookmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(current_sys.Name);
                if (sysbookmark != null)    // if we have one, edit it
                {
                    if (position.Location.ValidPosition)    // and we have a valid position, autocreate a new planet mark
                    {
                        frm.Bookmark(sysbookmark, position.BodyName, position.Location.Latitude, position.Location.Longitude);
                    }
                    else
                    {
                        frm.Bookmark(sysbookmark);
                    }
                }
                else
                {
                    if (position.Location.ValidPosition)        // don't have a system bookmark, so create a new one here. If we have position, autocreate a new planet mark
                    {
                        frm.NewSystemBookmark(current_sys, datetimeutc, position.BodyName, position.Location.Latitude, position.Location.Longitude);
                    }
                    else
                    {
                        frm.NewSystemBookmark(current_sys, datetimeutc);
                    }
                }
                
                frm.StartPosition = FormStartPosition.CenterScreen;
                DialogResult dr = frm.ShowDialog(this);

                if (dr == DialogResult.OK)
                {
                    GlobalBookMarkList.Instance.AddOrUpdateBookmark(sysbookmark, true, frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z), datetimeutc, frm.Notes, frm.SurfaceLocations);
                }
                if (dr == DialogResult.Abort)
                {
                    GlobalBookMarkList.Instance.Delete(sysbookmark);
                }

                PopulateBookmarkCombo();
            }
        }

        private void GlobalBookMarkList_OnBookmarkChange(BookmarkClass bk, bool deleted)
        {
            PopulateBookmarkCombo();
        }

        private void numberBoxTargetLatitude_ValueChanged(object sender, EventArgs e)
        {
            UpdateCompass();
        }

        private void numberBoxTargetLongitude_ValueChanged(object sender, EventArgs e)
        {
            UpdateCompass();
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = FontHelpers.FontSelection(this.FindForm(), displayfont ?? this.Font);     // will be null on cancel
            string setting = FontHelpers.GetFontSettingString(f);
            //System.Diagnostics.Debug.WriteLine($"Surveyor Font selected {setting}");
            PutSetting(dbFont, setting);
            displayfont = f;
            compassControl.Font = displayfont ?? this.Font;     // set control font
        }

        protected enum CtrlList
        {
            autohide,
            hidewithnolatlong,
        };

        private bool[] ctrlset; // holds current state of each control above

        private void PopulateCtrlList()
        {
            ctrlset = GetSettingAsCtrlSet<CtrlList>((e)=> { return e == CtrlList.autohide || e == CtrlList.hidewithnolatlong; });
        }

        private bool IsSet(CtrlList v)
        {
            return ctrlset[(int)v];
        }

        private void extButtonShowControl_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconListBoxFormGroup displayfilter = new CheckedIconListBoxFormGroup();
            displayfilter.AddAllNone();
            displayfilter.AddStandardOption(CtrlList.autohide.ToString(), "Auto Hide".TxID(EDTx.UserControlSurveyor_autoHideToolStripMenuItem));
            displayfilter.AddStandardOption(CtrlList.hidewithnolatlong.ToString(), "Hide when no Lat/Long".TxID(EDTx.UserControlSurveyor_autoHideToolStripMenuItem)); //tbd
            CommonCtrl(displayfilter, extButtonShowControl);
        }

        private void CommonCtrl(ExtendedControls.CheckedIconListBoxFormGroup displayfilter, Control under)
        {
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.ImageSize = new Size(24, 24);
            displayfilter.ScreenMargin = new Size(0, 0);
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);

            displayfilter.SaveSettings = (s, o) =>
            {
                PutBoolSettingsFromString(s, displayfilter.SettingsTagList());
                PopulateCtrlList();
                SetCompassVisibility();
            };

            displayfilter.Show(typeof(CtrlList), ctrlset, under, this.FindForm());
        }


        //    last_he = he;
        //    if (last_he != null)
        //    {
        //        if ( bodyRadius == null || lastradiusbody != he.WhereAmI)       // try and get radius, this is cleared on target selection
        //        { 
        //            StarScan.SystemNode last_sn = await DiscoveryForm.History.StarScan.FindSystemAsync(he.System, false);       // find scan if we have one
        //            JournalScan sd = last_sn?.Find(he.WhereAmI)?.ScanData;  // find body scan data if present, null if not
        //            bodyRadius = sd?.nRadius;
        //            if (bodyRadius.HasValue)
        //            {
        //                lastradiusbody = he.WhereAmI;
        //                System.Diagnostics.Debug.WriteLine("Compass Radius Set " + lastradiusbody + " " + bodyRadius.Value);
        //            }
        //        }

        //        switch (he.journalEntry.EventTypeID)
        //        {
        //            case JournalTypeEnum.Screenshot:
        //                JournalScreenshot js = he.journalEntry as JournalScreenshot;
        //                latitude = js.nLatitude;
        //                longitude = js.nLongitude;
        //                altitude = js.nAltitude;
        //                break;
        //            case JournalTypeEnum.Touchdown:
        //                JournalTouchdown jt = he.journalEntry as JournalTouchdown;
        //                if (jt.PlayerControlled.HasValue && jt.PlayerControlled.Value)
        //                {
        //                    latitude = jt.Latitude;
        //                    longitude = jt.Longitude;
        //                    altitude = 0;
        //                }
        //                break;
        //            case JournalTypeEnum.Location:
        //                JournalLocation jl = he.journalEntry as JournalLocation;
        //                latitude = jl.Latitude;
        //                longitude = jl.Longitude;
        //                altitude = null;
        //                break;
        //            case JournalTypeEnum.Liftoff:
        //                JournalLiftoff jlo = he.journalEntry as JournalLiftoff;
        //                if (jlo.PlayerControlled.HasValue && jlo.PlayerControlled.Value)
        //                {
        //                    latitude = jlo.Latitude;
        //                    longitude = jlo.Longitude;
        //                    altitude = 0;
        //                }
        //                break;
        //            case JournalTypeEnum.LeaveBody:
        //                latitude = null;
        //                longitude = null;
        //                altitude = null;
        //                break;
        //            case JournalTypeEnum.FSDJump:       // to allow us to do PopulateBookmark..
        //            case JournalTypeEnum.CarrierJump:     
        //                break;
        //            default:
        //                return;
        //        }


        #endregion
    }
}
