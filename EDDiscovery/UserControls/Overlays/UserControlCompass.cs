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
        private EliteDangerousCore.UIEvents.UIGUIFocus.Focus guistate;

        private Font displayfont;

        private ISystem current_sys;   // current system we are in, may be null, picked up from last_he
        private string current_body;    // current body, picked up from last_he, and checked via UI Position for changes

        private string sentbookmarktext;    // Another panel has sent a bookmark, and its position, add it to the combobox and allow selection
        private EliteDangerousCore.UIEvents.UIPosition.Position sentposition;

        bool lasttransparentmode = false;

        #region Init

        public UserControlCompass()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "Compass";

            double lat = GetSetting(dbLatSave, double.NaN);     // pick up target, it will be Nan if not set
            double lon = GetSetting(dbLongSave, double.NaN);
            if (double.IsNaN(lat) || double.IsNaN(lon))
            {
                numberBoxTargetLatitude.SetBlank();
                numberBoxTargetLongitude.SetBlank();
            }
            else
            {
                numberBoxTargetLatitude.ValueNoChange = lat;
                numberBoxTargetLongitude.ValueNoChange = lon;
            }
            this.numberBoxTargetLatitude.ValueChanged += new System.EventHandler(this.numberBoxTargetLatitude_ValueChanged);
            this.numberBoxTargetLongitude.ValueChanged += new System.EventHandler(this.numberBoxTargetLongitude_ValueChanged);

            comboBoxBookmarks.Text = "";

            buttonNewBookmark.Enabled = false;

            var enumlist = new Enum[] { EDTx.UserControlCompass_labelTargetLat };
            BaseUtils.Translator.Instance.TranslateControls(this, enumlist);
            var enumlisttt = new Enum[] {EDTx.UserControlCompass_numberBoxTargetLatitude_ToolTip, EDTx.UserControlCompass_numberBoxTargetLongitude_ToolTip,
                                        EDTx.UserControlCompass_comboBoxBookmarks_ToolTip, EDTx.UserControlCompass_extButtonBlank_ToolTip, EDTx.UserControlCompass_buttonNewBookmark_ToolTip,
                                        EDTx.UserControlCompass_extButtonShowControl_ToolTip, EDTx.UserControlCompass_extButtonFont_ToolTip};
            BaseUtils.Translator.Instance.TranslateTooltip(toolTip, enumlisttt, this);

            PopulateCtrlList();

            DiscoveryForm.OnNewEntry += OnNewEntry;
            DiscoveryForm.OnNewUIEvent += OnNewUIEvent;
            DiscoveryForm.OnHistoryChange += Discoveryform_OnHistoryChange;
            GlobalBookMarkList.Instance.OnBookmarkChange += GlobalBookMarkList_OnBookmarkChange;

            // new! april23 pick up last major mode ad uistate.  Need to do this now, before load/initial display, as set transparent uses it
            elitemode = new EliteDangerousCore.UIEvents.UIMode(DiscoveryForm.UIOverallStatus.Mode, DiscoveryForm.UIOverallStatus.MajorMode);
            guistate = DiscoveryForm.UIOverallStatus.Focus;
        }

        public override void LoadLayout()
        {
            base.LoadLayout();
            displayfont = FontHelpers.GetFont(GetSetting(dbFont, ""), null);        // null if not set, keep selection in displayfont
            if (displayfont != null)
                compassControl.Font = displayfont;
        }

        public override void InitialDisplay()       
        {
            Discoveryform_OnHistoryChange();
        }

        public override void Closing()
        {
            bool validsetting = numberBoxTargetLatitude.IsValid && numberBoxTargetLongitude.IsValid;
            PutSetting(dbLatSave, validsetting ? numberBoxTargetLatitude.Value : double.NaN);
            PutSetting(dbLongSave, validsetting ? numberBoxTargetLongitude.Value : double.NaN);
            DiscoveryForm.OnNewEntry -= OnNewEntry;
            DiscoveryForm.OnNewUIEvent -= OnNewUIEvent;
            DiscoveryForm.OnHistoryChange -= Discoveryform_OnHistoryChange;
            GlobalBookMarkList.Instance.OnBookmarkChange -= GlobalBookMarkList_OnBookmarkChange;
        }

        private void Discoveryform_OnHistoryChange() 
        {
            var lasthe = DiscoveryForm.History.GetLast;
            current_sys = lasthe?.System;       // pick up last system and body 
            current_body = lasthe?.Status.BodyName;
            PopulateBookmarkComboSetBookmarkEnable();
            UpdateCompass();
            SetCompassVisibility();
            SetCompassForegroundColours();      // do this in case history change caused by theme change
        }

        public override bool SupportTransparency { get { return true; } }
        public override bool DefaultTransparent { get { return true; } }
        public override void SetTransparency(bool on, Color curbackcol)
        {
            lasttransparentmode = on;   // keep this because its the only record of transparency mode, and we may need it in history change for SetCompassForegroundColour
            BackColor = curbackcol;

            numberBoxTargetLatitude.BackColor = numberBoxTargetLongitude.BackColor = curbackcol;
            numberBoxTargetLatitude.ControlBackground = numberBoxTargetLongitude.ControlBackground = curbackcol;
            labelTargetLat.TextBackColor = curbackcol;
            flowLayoutPanelTop.BackColor = curbackcol;
            comboBoxBookmarks.DisableBackgroundDisabledShadingGradient = on;
            comboBoxBookmarks.BackColor = curbackcol;
            buttonNewBookmark.BackColor = curbackcol;

            SetCompassForegroundColours();
        }

        public void SetCompassForegroundColours()
        { 
            Color fore = lasttransparentmode ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            compassControl.ForeColor = fore;
            compassControl.StencilColor = fore;
            compassControl.CentreTickColor = fore.Multiply(1.2F);
            compassControl.BugColor = fore.Multiply(0.8F);
            compassControl.BackColor = lasttransparentmode ? Color.Transparent : BackColor;
            compassControl.Font = ExtendedControls.Theme.Current.GetScaledFont(1f);
            flowLayoutPanelTop.Visible = !lasttransparentmode;
            compassControl.Font = displayfont ?? this.Font;     // due to themeing, set control font again

            intransparent = lasttransparentmode;
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

                System.Diagnostics.Debug.WriteLine($"Compass lat {pos.Location.Latitude}, {pos.Location.Longitude} A {pos.Location.Altitude} H {pos.Heading} R {pos.PlanetRadius} BN {pos.BodyName}");

                UpdateCompass();
                SetCompassVisibility();
            }

            EliteDangerousCore.UIEvents.UIBodyName bn = uievent as EliteDangerousCore.UIEvents.UIBodyName;

            if ( bn != null )
            {
                current_body = bn.BodyName;
                System.Diagnostics.Debug.WriteLine($"Compass changed body name {current_body}");

                PopulateBookmarkComboSetBookmarkEnable();

                // option to clear to blank target lat on body name disappearing
                if ( current_body.IsEmpty() && IsSet(CtrlList.clearlatlong))
                {
                    extButtonBlank_Click(null, null);
                }
            }

            var gui = uievent as EliteDangerousCore.UIEvents.UIGUIFocus;

            if ( gui != null )
            {
                guistate = gui.GUIFocus;
                System.Diagnostics.Debug.WriteLine($"Compass changed GUI state {guistate}");
                SetCompassVisibility();
            }
        }

        private void OnNewEntry(HistoryEntry he)
        {
            if (current_sys == null || current_sys.Name != he.System.Name)       // changed system
            {
                current_sys = he.System;        // always there
                current_body = he.Status.BodyName;        // may be blank or null
                PopulateBookmarkComboSetBookmarkEnable();
            }

            if ( he.journalEntry is IBodyFeature )       // an IBodyfeature would affect the body feature list, used to populate the combo box, so update combo
            {
                PopulateBookmarkComboSetBookmarkEnable();
            }
        }

        public override PanelActionState PerformPanelOperation(UserControlCommonBase sender, object actionobj)
        {
            if (actionobj is SetCompassTarget sct)
            {
                if (!comboBoxBookmarks.Items.Contains(sct.Name))        // only add to combo if not in list..
                {
                    sentbookmarktext = sct.Name;
                    sentposition = new EliteDangerousCore.UIEvents.UIPosition.Position() { Altitude = 0, AltitudeFromAverageRadius = false, Latitude = sct.Latitude, Longitude = sct.Longitude };
                    PopulateBookmarkComboSetBookmarkEnable();
                }
                comboBoxBookmarks.SelectedItem = sct.Name;      // must be in list, so select and set the compass
                return PanelActionState.Success;
            }
            return PanelActionState.NotHandled;
        }

        #endregion

        #region Display

        // we determine visibility from the stored flags.
        private void SetCompassVisibility()
        {
            bool visible = true;

            if (intransparent && IsSet(CtrlList.autohide))       // autohide turns off when transparent And..
            {
                if (guistate != EliteDangerousCore.UIEvents.UIGUIFocus.Focus.NoFocus)    // not on main screen
                {
                    visible = false;
                }
                            // if in mainship, or srv, or we are on foot planet, we can show
                else if ((!IsSet(CtrlList.hidewheninship) && elitemode.InFlight) ||
                         (!IsSet(CtrlList.hidewheninSRV) && elitemode.Mode == EliteDangerousCore.UIEvents.UIMode.ModeType.SRV ) ||
                         (!IsSet(CtrlList.hidewhenonfoot) && ( elitemode.Mode == EliteDangerousCore.UIEvents.UIMode.ModeType.OnFootPlanet ||
                                                                elitemode.Mode == EliteDangerousCore.UIEvents.UIMode.ModeType.OnFootInstallationInside )) 
                    )
                {
                }
                else
                    visible = false;    // else off
            }

            if (intransparent && ((IsSet(CtrlList.hidewithnolatlong) && !position.Location.ValidPosition) || // hide if no lat/long..
                                  (IsSet(CtrlList.hidewithnotarget) && !(numberBoxTargetLatitude.IsValid && numberBoxTargetLongitude.IsValid)))) // or hide if no target
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
            double bearing = double.NaN;
            double displaydistance = double.NaN;
            double glideslope = double.NaN;

            if (position.Location.ValidPosition && numberBoxTargetLatitude.IsValid && numberBoxTargetLongitude.IsValid)
            {
                double targetlat = numberBoxTargetLatitude.Value;
                double targetlong = numberBoxTargetLongitude.Value;

                bearing = ObjectExtensionsNumbersBool.CalculateBearing(position.Location.Latitude, position.Location.Longitude, targetlat, targetlong);

                if (position.ValidRadius)
                {
                    // we use distance including altitude as the prefered display distance

                    double distanceinclaltitude = position.Location.ValidAltitude ? 
                                ObjectExtensionsNumbersBool.CalculateDistance(position.Location.Latitude, position.Location.Longitude, position.Location.Altitude, targetlat, targetlong, 0, position.PlanetRadius) : 
                                ObjectExtensionsNumbersBool.CalculateDistance(position.Location.Latitude, position.Location.Longitude, targetlat, targetlong, position.PlanetRadius);

                    if ( distanceinclaltitude >= 100)       // if worth displaying
                    {
                        displaydistance = distanceinclaltitude;

                        if (distanceinclaltitude < 1000)
                        {
                            compassControl.DistanceFormat = "{0:0.#}m";
                        }
                        else if (distanceinclaltitude < 1000000)
                        {
                            displaydistance /= 1000;
                            compassControl.DistanceFormat = "{0:0.#}km";
                        }
                        else
                        {
                            displaydistance /= 1000000;
                            compassControl.DistanceFormat = "{0:0.#}Mm";
                        }
                    }

                    // if worth doing a glideslope

                    if ( distanceinclaltitude >= 3000 && position.Location.ValidAltitude && position.Location.Altitude >= 3000)    
                    {
                        // for glideslope, we don't want altitude in play for distance, just ground distance
                        double distanceground = ObjectExtensionsNumbersBool.CalculateDistance(position.Location.Latitude, position.Location.Longitude, targetlat, targetlong, position.PlanetRadius);

                        var res = ObjectExtensionsNumbersBool.CalculateGlideslope(distanceground, position.Location.Altitude, position.PlanetRadius);

                        if (res.Item2 < 0) // Don't return a slope if it would be > 0 deg at the target (ie. we are facing backwards)
                        {
                            glideslope = res.Item1;
                        }
                    }
                }
            }

            compassControl.Set(position.ValidHeading ? position.Heading : double.NaN, bearing, displaydistance, glideslope, true);
        }

        #endregion

        #region UI

        bool preventbookmarkcomboreentry = false;
        List<EliteDangerousCore.UIEvents.UIPosition.Position> comboboxpositions = new List<EliteDangerousCore.UIEvents.UIPosition.Position>(); // holds position for entries

        private void PopulateBookmarkComboSetBookmarkEnable()
        {
            preventbookmarkcomboreentry = true;

            string curselection = comboBoxBookmarks.Text; 

            comboBoxBookmarks.Items.Clear();
            comboboxpositions.Clear();

            buttonNewBookmark.Enabled = current_sys != null;

            System.Diagnostics.Debug.WriteLine($"Compass populate bookmark sys {current_sys?.Name} body {current_body}");

            if (current_sys != null)
            {
                var sysbookmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(current_sys.Name);
                if (sysbookmark != null)
                {
                    List<PlanetMarks.Planet> planetMarks = sysbookmark.PlanetaryMarks?.Planets;

                    if (current_body != null)
                    {
                        string planetname = current_body.ReplaceIfStartsWith(current_sys.Name, "");
                        System.Diagnostics.Debug.WriteLine($"..Compass Combobox check for planet '{planetname}'");
                        if ( planetname.HasChars() )
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
                                    System.Diagnostics.Debug.WriteLine($"..Compass Combobox Add {pl.Name}: {loc.Name}");
                                    comboBoxBookmarks.Items.Add($"{pl.Name}: {loc.Name} @ {loc.Latitude:0.####}, {loc.Longitude:0.####}");
                                    comboboxpositions.Add(new EliteDangerousCore.UIEvents.UIPosition.Position() { Latitude = loc.Latitude, Longitude = loc.Longitude });                                    
                                }
                            }
                        }
                    }
                }

                var sysnode = DiscoveryForm.History.StarScan.FindSystemSynchronous(current_sys);     // not edsm, so no delay

                if ( sysnode != null)
                {
                    var scannode = current_body.HasChars() ? sysnode.Find(current_body) : null;

                    if ( scannode != null)
                    {
                        System.Diagnostics.Debug.WriteLine($"..Compass Found scannode for {current_body}");

                        foreach (var sf in scannode.SurfaceFeatures.EmptyIfNull())
                        {
                            if (sf.HasLatLong)  // only want positions
                            {
                                System.Diagnostics.Debug.WriteLine($"..Compass Combobox Add {sf.Name_Localised}");
                                comboBoxBookmarks.Items.Add($"{sf.Name_Localised} @ {sf.Latitude.Value:0.####}, {sf.Longitude.Value:0.####}");
                                comboboxpositions.Add(new EliteDangerousCore.UIEvents.UIPosition.Position() { Latitude = sf.Latitude.Value, Longitude = sf.Longitude.Value });
                            }
                        }
                    }
                    else
                    {
                        foreach( var bodies in sysnode.Bodies)
                        {
                            System.Diagnostics.Debug.WriteLine($"..Compass no current body processing {bodies.BodyDesignator}");

                            foreach (var sf in bodies.SurfaceFeatures.EmptyIfNull())
                            {
                                if (sf.HasLatLong)  // only want positions
                                {
                                    System.Diagnostics.Debug.WriteLine($"..Compass Combobox Add {sf.Name_Localised}");
                                    comboBoxBookmarks.Items.Add($"{bodies.BodyNameOrOwnName}: {sf.Name_Localised} @ {sf.Latitude.Value:0.####}, {sf.Longitude.Value:0.####}");
                                    comboboxpositions.Add(new EliteDangerousCore.UIEvents.UIPosition.Position() { Latitude = sf.Latitude.Value, Longitude = sf.Longitude.Value });
                                }
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
            }

            preventbookmarkcomboreentry = false;
        }

        private void comboBoxBookmarks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ( !preventbookmarkcomboreentry && comboBoxBookmarks.SelectedIndex>=0)
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
                        frm.NewPlanetBookmark(sysbookmark, position.BodyName.ReplaceIfStartsWith(current_sys.Name, ""), position.Location.Latitude, position.Location.Longitude);
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
                        frm.NewSystemBookmark(current_sys, datetimeutc, position.BodyName.ReplaceIfStartsWith(current_sys.Name, ""), position.Location.Latitude, position.Location.Longitude);
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
                    GlobalBookMarkList.Instance.AddOrUpdateBookmark(sysbookmark, true, frm.StarHeading, double.Parse(frm.x), double.Parse(frm.y), double.Parse(frm.z), datetimeutc, frm.Notes, frm.Tags, frm.SurfaceLocations);
                }
                if (dr == DialogResult.Abort)
                {
                    GlobalBookMarkList.Instance.Delete(sysbookmark);
                }

                PopulateBookmarkComboSetBookmarkEnable();
            }
        }

        private void GlobalBookMarkList_OnBookmarkChange(BookmarkClass bk, bool deleted)
        {
            PopulateBookmarkComboSetBookmarkEnable();
        }

        private void numberBoxTargetLatitude_ValueChanged(object sender, EventArgs e)
        {
            UpdateCompass();
        }

        private void numberBoxTargetLongitude_ValueChanged(object sender, EventArgs e)
        {
            UpdateCompass();
        }

        private void extButtonBlank_Click(object sender, EventArgs e)
        {
            numberBoxTargetLongitude.SetBlank();      
            numberBoxTargetLatitude.SetBlank();
            comboBoxBookmarks.SelectedIndex = -1;
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
            hidewithnotarget,
            hidewhenonfoot,
            hidewheninSRV,
            hidewheninship,
            clearlatlong,
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
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();
            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(CtrlList.autohide.ToString(), "Auto Hide".TxID(EDTx.UserControlSurveyor_autoHideToolStripMenuItem));
            displayfilter.UC.Add(CtrlList.hidewithnolatlong.ToString(), "Hide when Elite has no Lat/Long".TxID(EDTx.UserControlCompass_hidewhennolatlong)); 
            displayfilter.UC.Add(CtrlList.hidewithnotarget.ToString(), "Hide when no Lat/Long target is set".TxID(EDTx.UserControlCompass_hidewhennotarget)); 
            displayfilter.UC.Add(CtrlList.hidewhenonfoot.ToString(), "Hide when on foot".TxID(EDTx.UserControlCompass_hidewhenonfoot)); 
            displayfilter.UC.Add(CtrlList.hidewheninSRV.ToString(), "Hide when in SRV".TxID(EDTx.UserControlCompass_hidewheninSRV)); 
            displayfilter.UC.Add(CtrlList.hidewheninship.ToString(), "Hide when in ship".TxID(EDTx.UserControlCompass_hidewheninship));
            displayfilter.UC.Add(CtrlList.clearlatlong.ToString(), "Clear target when leaving a body".TxID(EDTx.UserControlCompass_cleartargetonleavingbody)); 
            CommonCtrl(displayfilter, extButtonShowControl);
        }

        private void CommonCtrl(ExtendedControls.CheckedIconNewListBoxForm displayfilter, Control under)
        {
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.UC.ScreenMargin = new Size(0, 0);
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);

            displayfilter.SaveSettings = (s, o) =>
            {
                PutBoolSettingsFromString(s, displayfilter.UC.TagList());
                PopulateCtrlList();
                SetCompassVisibility();
            };

            displayfilter.Show(typeof(CtrlList), ctrlset, under, this.FindForm());
        }

        #endregion

    }
}
