 /*
 * Copyright 2025 - 2025 EDDiscovery development team
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
using EliteDangerousCore.JournalEvents;
using EliteDangerousCore.StarScan2;
using EliteDangerousCore.UIEvents;
using ExtendedControls;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace EDDiscovery.UserControls
{
    public partial class TravelPanel : UserControlCommonBase
    {
        public TravelPanel()
        {
            InitializeComponent();
            DBBaseName = "TravelPanel";
        }
        protected override void Init()
        {
            edsmSpanshButton.Init(this, "EDSMSpansh", "");
            edsmSpanshButton.ValueChanged += (s, ch) =>
            {
                UpdateDisplay();
            };

            DiscoveryForm.OnNewUIEvent += DiscoveryForm_OnNewUIEvent;
            DiscoveryForm.OnPreHistoryChange += DiscoveryForm_OnPreHistoryChange;         // we use this before other panels as we are also trapping new entry (new june 26)
            DiscoveryForm.OnNewEntry += DiscoveryForm_OnNewEntry;

            // if opened after start, we need to pick up state from discoveryform

            uistatus = DiscoveryForm.UIOverallStatus;
            lasthe = DiscoveryForm.History.GetLast;

            PopulateCtrlList();

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, false);
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_CheckedChanged;

            displayfont = BaseUtils.FontHandler.GetFontFromSetting(GetSetting(dbFont, ""), null);        // null if not set
        }

        protected override void InitialDisplay()
        {
            UpdateDisplay();
        }

        protected override void Closing()
        {
            DiscoveryForm.OnNewUIEvent -= DiscoveryForm_OnNewUIEvent;
            DiscoveryForm.OnPreHistoryChange -= DiscoveryForm_OnPreHistoryChange;
            DiscoveryForm.OnNewEntry -= DiscoveryForm_OnNewEntry;
        }

        public override bool SupportTransparency => true;
        protected override void SetTransparency(bool on, Color curcol)
        {
            System.Diagnostics.Debug.WriteLine($"Transparent mode {on}");
            this.BackColor = curcol;
        }

        private void DiscoveryForm_OnPreHistoryChange()
        {
            lasthe = DiscoveryForm.History.GetLast;
            UpdateDisplay();
        }

        private void DiscoveryForm_OnNewEntry(HistoryEntry he)
        {
#if DEBUG
#else
            lasthe = DiscoveryForm.History.GetLast;
            System.Diagnostics.Debug.WriteLine($"Travel Panel NewHistory {lasthe.EventTimeUTC}");

            if ( he.journalEntry.EventTypeID != JournalTypeEnum.EDDDestinationSelected)     // don't repeat the UI push
                UpdateDisplay();
#endif
        }

        private void DiscoveryForm_OnNewUIEvent(EliteDangerousCore.UIEvent ui)
        {
            if (ui is UIOverallStatus os)      // if opened at start we get this, and we get it every time flags changes
            {
                System.Diagnostics.Debug.WriteLine($"AutoPanel UI {ui.EventTypeID} : {ui.ToString()}");
                uistatus = os;
                UpdateDisplay();
            }
        }

        public override void ReceiveHistoryEntry(EliteDangerousCore.HistoryEntry he)
        {
#if DEBUG
            lasthe = he;
            System.Diagnostics.Debug.WriteLine($"TravelPanel Receive HE {lasthe.EventTimeUTC} {lasthe.EventSummary} {lasthe.System.Name}");

            if (he.journalEntry.EventTypeID != JournalTypeEnum.EDDDestinationSelected)     // don't repeat the UI push
                UpdateDisplay();
#endif
        }

        async void UpdateDisplay()
        {
            extPictureBox.ClearImageList();

            EliteDangerousCore.StarScan2.SystemNode sysnode = lasthe != null ? await DiscoveryForm.History.StarScan2.FindSystemAsync(lasthe.System, edsmSpanshButton.WebLookup) : null;
            if (IsClosed || sysnode == null || lasthe == null)
            {
                extPictureBox.Render();
                return;
            }

            // this is the current system information (may be null) either in a Location, FSDJump or CarrierJump

            Point pos = new Point(3, 3);
            Size textsize = new Size(Math.Max(extPictureBox.Width - 6, 24), 10000);
            var textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
            var backcolour = IsTransparentModeOn ? Color.Transparent : this.BackColor;
            Font dfont = displayfont ?? this.Font;

            ExtendedControls.ImageElement.List el = new ExtendedControls.ImageElement.List();

            {
                string startext = string.Empty;
                Image starimage = null;

                var currentsystemclass = lasthe.Status.LastFSDJump;
                if (currentsystemclass != null)
                {
                    var starbody = sysnode.GetStarWithZeroDistls() ?? sysnode.GetBodesSimplifiedTopStar();          // zerols star or any star
                    var scan = starbody?.Scan;

                    startext = $"System {currentsystemclass.StarSystem} @ {currentsystemclass.StarPos.X:N2}, {currentsystemclass.StarPos.Y:N2}, {currentsystemclass.StarPos.Z:N2}";

                    if (scan != null)
                    {
                        startext += $": {scan.StarClassificationAbv}" + Environment.NewLine;
                        startext += $"{scan.nStellarMass:N2} sm {scan.nAbsoluteMagnitude:N2} m {scan.nAge:N0} my" + Environment.NewLine;
                    }
                    else
                        startext += Environment.NewLine;

                    string facstate = currentsystemclass.FactionState == FactionDefinitions.State.Unknown ? "" : (FactionDefinitions.ToLocalisedLanguage(currentsystemclass.FactionState) + ", ");

                    startext += $"{currentsystemclass.Faction}, {facstate}{currentsystemclass.Government_Localised}, {currentsystemclass.Population:N0}";

                    starimage = BaseUtils.Icons.IconSet.GetImage(scan != null ? scan.StarTypeImageName : "Bodies.Stars.Unknown");
                }
                else
                {
                    startext = "No FSD Jump";
                }

                el.AddPictureTextHorzDivider(new Rectangle(pos, textsize),
                        starimage, new Size(dfont.Height * 4, dfont.Height * 4),
                        startext,
                        dfont, extCheckBoxWordWrap.Checked, alignment,
                        true,
                        textcolour, backcolour
                        );
            }

            IBodyLocation currentlocationclass = lasthe.Status.CurrentLocation;

            if (currentlocationclass != null)
            {
                BodyNode bn = currentlocationclass.BodyID != null ? sysnode.FindBody(currentlocationclass.BodyID.Value) : null;     // may be null
                JournalScan scan = bn?.Scan;    // may be null

                string loctext = $"@ {currentlocationclass.BodyName}";
                Image locimage = null;

                // possible location types are all IBodyLocation
                //      ApproachBody, ApproachSettlement, SupercruiseExit
                //      Docked/Location (both ILocDocked)       Location is also JournalLocOrJump
                //      FSDJump/Carrier Jump (both JournalLocOrJump)

                if (currentlocationclass is ILocDocked jld)           // Location or Docking 
                {
                    if (jld.Docked == true)
                    {
                        loctext += $": {jld.StationName_Localised}" + Environment.NewLine + $"{StationDefinitions.ToLocalisedLanguage(jld.FDStationType)}, {jld.StationFaction}, {AllegianceDefinitions.ToEnglish(jld.StationAllegiance)}";
                        string iname = StationDefinitions.StationImageName(jld.FDStationType);
                        BaseUtils.Icons.IconSet.TryGetImage(iname, out locimage);      // may not have an image 
                    }
                    else
                    {
                        if (scan != null)
                            locimage = bn.Scan.GetImage();
                    }
                }
                else if (currentlocationclass is JournalLocOrJump jlj)    // FSDJump or Carrier Jump
                {
                    if (jlj.Faction.HasChars())
                    {
                        loctext += Environment.NewLine + $"{jlj.Faction}, {AllegianceDefinitions.ToLocalisedLanguage(jlj.Allegiance)}, {EconomyDefinitions.ToLocalisedLanguage(jlj.Economy)}"
                                            + $", {SecurityDefinitions.ToLocalisedLanguage(jlj.Security)}), {jlj.Population:N0} ";
                    }

                    if (scan != null && bn.BodyID != 0)
                        locimage = bn.Scan.GetImage();
                }
                else if (currentlocationclass is JournalApproachBody jab)
                {
                    if (scan != null)
                    {
                        if (scan.IsPlanet)
                        {
                            loctext += Environment.NewLine + $"{scan.DisplayShortInformation(null)}";
                        }
                        locimage = bn.Scan.GetImage();
                    }
                }
                else if (currentlocationclass is JournalApproachSettlement jas)
                {
                    loctext += $": {jas.Name}" + Environment.NewLine + $"{jas.StationFaction}, {AllegianceDefinitions.ToEnglish(jas.StationAllegiance)}, {EconomyDefinitions.ToLocalisedLanguage(jas.StationEconomy)}";
                    loctext += Environment.NewLine + $"@ {jas.Latitude:N2}, {jas.Longitude:N2}";
                    string iname = StationDefinitions.StationImageName(StationDefinitions.StarportTypes.OnFootSettlement);
                    BaseUtils.Icons.IconSet.TryGetImage(iname, out locimage);      // may not have an image 
                }
                else if (currentlocationclass is JournalSupercruiseExit jse)
                {
                    loctext += $": {currentlocationclass.BodyType}";

                    if (jse.BodyType == BodyDefinitions.BodyType.Station)
                    {
                        string iname = StationDefinitions.StationImageName(StationDefinitions.StarportTypes.Coriolis);
                        BaseUtils.Icons.IconSet.TryGetImage(iname, out locimage);      // may not have an image 
                    }
                    else if (jse.BodyType == BodyDefinitions.BodyType.Planet)
                    {
                        if (bn?.Scan != null)
                            locimage = bn.Scan.GetImage();
                    }
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false, $"TravelPanel unexpected Location class {currentlocationclass.GetType().Name}");
                }

                //loctext = currentlocationclass.GetType().Name + ": " + loctext; //DEBUG

                System.Diagnostics.Debug.WriteLine($"TravelPanel writing {loctext}");

                el.AddPictureTextHorzDivider(new Rectangle(new Point(pos.X, el.MaxOrDefault.Y + 4), textsize),
                            locimage, new Size(dfont.Height * 3, dfont.Height * 3),
                            loctext,
                            dfont, extCheckBoxWordWrap.Checked, alignment,
                            true,
                            textcolour, backcolour
                            );
            }

            {
                if (uistatus.DestinationName.HasChars() && uistatus.DestinationSystemAddress.HasValue)
                {
                    var uis = uistatus;     // async below may result in a change to this, protect, found during debugging! This async stuff is evil
                    var ss = await DiscoveryForm.History.StarScan2.FindSystemAsync(new SystemClass(uis.DestinationSystemAddress.Value), edsmSpanshButton.WebLookup);
                    if (IsClosed)
                        return;

                    string desttext = ">>> " + uistatus.DestinationName_Localised;
                    Image destimage = null;

                    if (ss != null)    // we have it, find info on system
                    {
                        System.Diagnostics.Debug.WriteLine($"Travel Panel Dest found system  {ss.System.Name} {uis.DestinationSystemAddress} ");

                        BodyNode bn = uistatus.DestinationBodyID != null ? ss.FindBody(uistatus.DestinationBodyID.Value) : null;     // may be null

                        // targetting a barycentre, replace with the best star if possible by finding something with a scan at DistLS = 0 
                        if (bn?.BodyType == BodyDefinitions.BodyType.Barycentre)
                        {
                            var better = ss.Bodies(x => x.DistLS == 0).FirstOrDefault();        
                            if ( better != null)
                                bn = better;
                        }

                        JournalScan scan = bn?.Scan;    // may be null

                        bool destisbody = bn != null ? bn.CanonicalNameOrOwnName.EqualsIIC(uistatus.DestinationName) : true;
                        var classification = SignalDefinitions.ClassifyStationName(uistatus.DestinationName_Localised);

                        if (bn?.BodyType == BodyDefinitions.BodyType.PlanetaryRing)
                        {
                            desttext += ": " + "Planetary Ring";
                        }
                        else if (bn?.BodyType == BodyDefinitions.BodyType.StellarRing)
                        {
                            desttext += ": " + "Belt Cluster";
                        }
                        else if (bn?.BodyType == BodyDefinitions.BodyType.AsteroidCluster)
                        {
                            desttext += ": " + "Belt Cluster Body";
                        }
                        else if (bn?.BodyType == BodyDefinitions.BodyType.Barycentre)
                        {
                            desttext += ": " + "Barycentre";
                        }
                        else if (bn?.BodyType == BodyDefinitions.BodyType.Star)
                        {
                            if (!destisbody)
                            {
                                desttext += ": " + (classification == SignalDefinitions.Classification.Carrier ? "Carrier" + " " : "") +
                                    "Orbiting" + ": ";
                            }
                            else
                                desttext += ": " + "Star";
                        }
                        else if (bn?.BodyType == BodyDefinitions.BodyType.Planet)
                        {
                            if (!destisbody)
                            {
                                desttext += ": " + (classification == SignalDefinitions.Classification.Carrier ? "Carrier" + " " + "Orbiting" :
                                            "Station" + " " + "Orbiting" + " " + "or on surface" + " of") + ": ";
                            }
                            else
                                desttext += ": " + "Planet";
                        }

                        else 
                        {
                        }

                        if (scan != null)
                        {
                            destimage = bn.Scan.GetImage();
                            desttext += Environment.NewLine + $"{scan.DisplayShortInformation(null)}";
                        }
                        else if (bn != null)
                        {
                            desttext += Environment.NewLine + "No Scan data";
                        }
                        else
                        {
                            // may be an orbital stations
                            IBodyFeature orbitalstation = uistatus.DestinationBodyID != null ? ss.GetOrbitalStation(uistatus.DestinationBodyID.Value) : null;
                            if (orbitalstation is JournalDocked jld)      // paranoia to make sure its a journal docked - should always be
                            {
                                //desttext += Environment.NewLine + "Orbital station";
                                desttext += Environment.NewLine + $"{StationDefinitions.ToLocalisedLanguage(jld.FDStationType)}, {jld.StationFaction}, {AllegianceDefinitions.ToEnglish(jld.StationAllegiance)}";
                                string iname = StationDefinitions.StationImageName(jld.FDStationType);
                                BaseUtils.Icons.IconSet.TryGetImage(iname, out destimage);      // may not have an image 

                            }
                        }
                    }
                    else
                    {   // no info
                    }

                    el.AddPictureTextHorzDivider(new Rectangle(new Point(pos.X, el.MaxOrDefault.Y + 4), textsize),
                                destimage, new Size(dfont.Height * 3, dfont.Height * 3),
                                desttext,
                                dfont, extCheckBoxWordWrap.Checked, alignment,
                                false,
                                textcolour, backcolour
                                );

                }
            }


            System.Diagnostics.Debug.WriteLine($"Update picture box @ {lasthe.EventTimeUTC} {lasthe.EventSummary} {lasthe.System.Name}");
            extPictureBox.AddRange(el);
            extPictureBox.Render();
        }


        #region UI
        private void extButtonAlignment_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            string lt = CtrlList.alignleft.ToString();
            string ct = CtrlList.aligncenter.ToString();
            string rt = CtrlList.alignright.ToString();

            displayfilter.UC.Add(lt, "Alignment Left".Tx(), global::EDDiscovery.Icons.Controls.AlignLeft, exclusivetags: ct + ";" + rt, disableuncheck: true);
            displayfilter.UC.Add(ct, "Alignment Center".Tx(), global::EDDiscovery.Icons.Controls.AlignCentre, exclusivetags: lt + ";" + rt, disableuncheck: true);
            displayfilter.UC.Add(rt, "Alignment Right".Tx(), global::EDDiscovery.Icons.Controls.AlignRight, exclusivetags: lt + ";" + ct, disableuncheck: true);
            displayfilter.CloseOnChange = true;
            CommonCtrl(displayfilter, extButtonAlignment);
        }

        private void CommonCtrl(ExtendedControls.CheckedIconNewListBoxForm displayfilter, Control under, string saveasstring = null)
        {
            displayfilter.CloseBoundaryRegion = new Size(32, under.Height);
            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.UC.ScreenMargin = new Size(0, 0);
            displayfilter.UC.MultiColumnSlide = true;

            displayfilter.SaveSettings = (s, o) =>
            {
                if (saveasstring == null)
                    PutBoolSettingsFromString(s, displayfilter.UC.TagList());
                else
                    PutSetting(saveasstring, s);

                PopulateCtrlList();
                // tbdSetVisibility();
                UpdateDisplay();
            };

            if (saveasstring == null)
                displayfilter.Show(typeof(CtrlList), ctrlset, under, this.FindForm());
            else
                displayfilter.Show(GetSetting(saveasstring, ""), under, this.FindForm());
        }

        private void PopulateCtrlList()
        {
            ctrlset = GetSettingAsCtrlSet<CtrlList>((e) => (e != CtrlList.alignright && e != CtrlList.aligncenter));
            alignment = ctrlset[(int)CtrlList.alignright] ? StringAlignment.Far : ctrlset[(int)CtrlList.aligncenter] ? StringAlignment.Center : StringAlignment.Near;
        }

        private void extButtonFont_Click(object sender, EventArgs e)
        {
            Font f = BaseUtils.FontDialog.SelectFont(this.FindForm(), displayfont ?? this.Font, true);
            string setting = BaseUtils.FontHandler.GetFontSettingString(f);
            System.Diagnostics.Debug.WriteLine($"Travel Panel Font selected {setting}");
            PutSetting(dbFont, setting);
            displayfont = f;
            UpdateDisplay();
        }

        private void extCheckBoxWordWrap_CheckedChanged(object sender, EventArgs e)
        {
            PutSetting(dbWordWrap, extCheckBoxWordWrap.Checked);
            UpdateDisplay();
        }

        protected enum CtrlList
        {
            alignleft, aligncenter, alignright,
        };

        #endregion

        #region Vars

        private bool[] ctrlset; // holds current state of each control above
        private UIOverallStatus uistatus;
        private HistoryEntry lasthe;
        private Font displayfont;                       // font selected
        private StringAlignment alignment = StringAlignment.Near;   // text alignment
        public const string dbFont = "font";
        public const string dbWordWrap = "wordwrap";
        private const char SettingsSplittingChar = '\u2188';     // pick a crazy one soe

        #endregion
    }
}
