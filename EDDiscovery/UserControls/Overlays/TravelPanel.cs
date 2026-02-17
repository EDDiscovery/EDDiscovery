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
            DiscoveryForm.OnNewUIEvent += DiscoveryForm_OnNewUIEvent;
            DiscoveryForm.OnNewEntry += DiscoveryForm_OnNewEntry;
#if DEBUG
            DiscoveryForm.OnHistoryChange += DiscoveryForm_OnHistoryChange;         // panel normally tracks the top
#endif

            // if opened after start, we need to pick up state from discoveryform

            uistatus = DiscoveryForm.UIOverallStatus;
            lasthe = DiscoveryForm.History.GetLast;

            PopulateCtrlList();

            extCheckBoxWordWrap.Checked = GetSetting(dbWordWrap, false);
            extCheckBoxWordWrap.Click += extCheckBoxWordWrap_CheckedChanged;

            displayfont = FontHelpers.GetFont(GetSetting(dbFont, ""), null);        // null if not set
        }

        protected override void InitialDisplay()
        {
            UpdateDisplay();
        }

        protected override void Closing()
        {
            DiscoveryForm.OnNewUIEvent -= DiscoveryForm_OnNewUIEvent;
            DiscoveryForm.OnHistoryChange -= DiscoveryForm_OnHistoryChange;
            DiscoveryForm.OnNewEntry -= DiscoveryForm_OnNewEntry;
        }

        public override bool SupportTransparency => true;
        protected override void SetTransparency(bool on, Color curcol)
        {
            System.Diagnostics.Debug.WriteLine($"Transparent mode {on}");
            this.BackColor = curcol;
        }

        private void DiscoveryForm_OnHistoryChange()
        {
            lasthe = DiscoveryForm.History.GetLast;
            UpdateDisplay();
        }

        private void DiscoveryForm_OnNewEntry(HistoryEntry he)
        {
            lasthe = DiscoveryForm.History.GetLast;
            System.Diagnostics.Debug.WriteLine($"Travel Panel NewHistory {lasthe.EventTimeUTC}");
            UpdateDisplay();
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
            lasthe = he;
            //System.Diagnostics.Debug.WriteLine($"Receive HE {lasthe.EventTimeUTC} {lasthe.EventSummary} {lasthe.System.Name}");
            UpdateDisplay();
        }

        async void UpdateDisplay()
        {
            extPictureBox.ClearImageList();

            EliteDangerousCore.StarScan2.SystemNode sysnode = lasthe != null ? await DiscoveryForm.History.StarScan2.FindSystemAsync(lasthe.System, WebExternalDataLookup.None) : null;
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

            var currentsystemclass = lasthe.Status.LastFSDJump;
            if (currentsystemclass != null)
            {
                var starbody = sysnode.GetStarWithZeroDistls() ?? sysnode.GetBodesSimplifiedTopStar();          // zerols star or any star
                var scan = starbody?.Scan;
                string starimage = scan != null ? scan.StarTypeImageName : "Bodies.Stars.Unknown";

                string startext = $"System {currentsystemclass.StarSystem} @ {currentsystemclass.StarPos.X:N2}, {currentsystemclass.StarPos.Y:N2}, {currentsystemclass.StarPos.Z:N2}";

                if (scan != null)
                {
                    startext += $" {scan.StarClassificationAbv}" + Environment.NewLine;
                    startext += $" {scan.nStellarMass:N2} sm {scan.nAbsoluteMagnitude:N2} m {scan.nAge:N0} my" + Environment.NewLine;
                }
                else
                    startext += Environment.NewLine;

                string facstate =currentsystemclass.FactionState == FactionDefinitions.State.Unknown ? "" : (FactionDefinitions.ToLocalisedLanguage(currentsystemclass.FactionState) + ", ");

                startext += $"{currentsystemclass.Faction}, {facstate}{currentsystemclass.Government_Localised}, {currentsystemclass.Population:N0}";

                el.AddPictureTextHorzDivider(new Rectangle(pos, textsize),
                    BaseUtils.Icons.IconSet.GetImage(starimage), new Size(dfont.Height * 4, dfont.Height * 4),
                        startext,
                        dfont, extCheckBoxWordWrap.Checked, alignment,
                        true,
                        textcolour, backcolour
                        );
            }

            var currentlocationclass = lasthe.Status.CurrentLocation;

            if (currentlocationclass != null)
            {
                BodyNode bn = currentlocationclass.BodyID != null ? sysnode.FindBody(currentlocationclass.BodyID.Value) : null;

                string loctext = "";
                Image image = null;

                if (currentlocationclass is JournalDocked dck)
                {
                    loctext += Environment.NewLine + $"{dck.StationName_Localised} {StationDefinitions.ToLocalisedLanguage(dck.FDStationType)}, {dck.StationFaction}, {AllegianceDefinitions.ToEnglish(dck.StationAllegiance)}";

                    string iname = StationDefinitions.StationImageName(dck.FDStationType);
                    BaseUtils.Icons.IconSet.TryGetImage(iname, out image);      // may not have an image so 
                }
                else if (currentlocationclass is JournalApproachSettlement)
                {

                }
                else
                {
                    loctext = $"{currentlocationclass.BodyName} {currentlocationclass.BodyType}";

                    if (bn?.Scan != null)
                    {
                        image = bn.Scan.GetImage();

                    }
                }

                el.AddPictureTextHorzDivider(new Rectangle(new Point(pos.X, el.Max.Y + 4), textsize),
                            image, new Size(dfont.Height * 3, dfont.Height * 3),
                            loctext,
                            dfont, extCheckBoxWordWrap.Checked, alignment,
                            true,
                            textcolour, backcolour
                            );
            }

            System.Diagnostics.Debug.WriteLine($"Update picture box @ {lasthe.EventTimeUTC} {lasthe.EventSummary} {lasthe.System.Name}");

            extPictureBox.AddRange(el);
            extPictureBox.Render();










            ////if (data != null)
            ////{
            //    var system  


            //    if ( lasthe.Status.IsDocked)
            //    {
            //        //IBodyFeature bf = data.GetFeature(lasthe.Status.)
            //    }


            //    //string text = "";
            //    //if (uistatus.MajorMode == UIMode.MajorModeType.MainShip)
            //    //{
            //    //    if (uistatus.Mode == UIMode.ModeType.MainShipNormalSpace)
            //    //    {
            //    //        text = "Normal space";
            //    //        // show destination if set, if approach body show that, get body info from scan data
            //    //    }
            //    //    else if (uistatus.Mode == UIMode.ModeType.MainShipDockedStarPort)
            //    //    {
            //    //        if ( data.FindCanonicalBodyName)
            //    //        text = "Docked Starport";
            //    //        // show information about the station, economy, etc
            //    //    }
            //    //    else if (uistatus.Mode == UIMode.ModeType.MainShipDockedPlanet)
            //    //    {
            //    //        text = "Docked Planet";
            //    //        // show information about the station, economy, etc
            //    //    }
            //    //    else if (uistatus.Mode == UIMode.ModeType.MainShipSupercruise)
            //    //    {
            //    //        bool injump = uistatus.Flags.Contains(UITypeEnum.FsdJump);
            //    //        if (injump)
            //    //            text = "Jumping to ";
            //    //        else
            //    //            text = "Supercruising"; // tbd to?

            //    //        // show destination if set, if approach body show that, get body info from scan data
            //    //    }
            //    //    else if (uistatus.Mode == UIMode.ModeType.MainShipLanded)
            //    //    {
            //    //        text = "Landed";
            //    //        // show destination if set, if approach body show that, get body info from scan data
            //    //    }
            //    //}
            //    //else if (uistatus.MajorMode == UIMode.MajorModeType.SRV)
            //    //{
            //    //    text = "SRV";
            //    //    // show information about lat/long
            //    //}
            //    //else if (uistatus.MajorMode == UIMode.MajorModeType.Fighter)
            //    //{
            //    //    text = "Fighter";
            //    //    // show information about stuff
            //    //}
            //    //if (uistatus.MajorMode == UIMode.MajorModeType.OnFoot)
            //    //{
            //    //    text = "OnFoot";
            //    //    // show information about planet or station
            //    //}
            //}

            //if (uistatus.DestinationName.HasChars() && uistatus.DestinationSystemAddress.HasValue)
            //{
            //    var uis = uistatus;     // async below may result in a change to this, protect, found during debugging! This async stuff is evil

            //    var ss = await DiscoveryForm.History.StarScan2.FindSystemAsync(new SystemClass(uis.DestinationSystemAddress.Value), WebExternalDataLookup.Spansh);

            //    if (IsClosed)
            //        return;

            //    if (ss != null)    // we have it, find info on system
            //    {
            //        System.Diagnostics.Debug.WriteLine($"Travel Panel Dest found system  {ss.System.Name} {uis.DestinationSystemAddress} ");

            //        //  body name destination or $POI $MULTIPLAYER etc
            //        // Now (oct 25) its localised, but if not, so attempt a rename for those $xxx forms ($Multiplayer.. $POI)

            //        string destname = uis.DestinationName_Localised.Alt(JournalFieldNaming.SignalBodyName(uis.DestinationName));

            //        System.Diagnostics.Debug.WriteLine($".. Travel Destination non star {destname}");

            //        if (uis.DestinationBodyID == 0)
            //        {
            //            // system itself
            //        }
            //        else
            //        {
            //            EliteDangerousCore.StarScan2.BodyNode body = ss.FindBody(uis.DestinationBodyID.Value);
            //            if (body != null)
            //            {
            //                // body itself
            //            }
            //        }

            //        IBodyFeature feature = ss.GetFeature(destname);
            //        if (feature != null)
            //        {
            //            System.Diagnostics.Debug.WriteLine($".. feature found {feature.Name} {feature.BodyName} {feature.BodyType}");
            //        }
            //    }



            //var sd = new EliteDangerousCore.StarScan2.SystemDisplay();
            //    sd.SetSize(48);
            //    sd.Font = this.Font;
            //    sd.TextBackColor = Color.Transparent;
            //    ExtPictureBox pb = new ExtPictureBox();
            //    sd.DrawSystemRender(pb, 800, ss);

            //        //System.Diagnostics.Debug.Write("Dumping images");
            //        //pb.Image.Save(@"c:\code\dump\systemdisplay.png");

            //        int count = 0;
            //        foreach (var bodys in ss.Bodies())
            //        {
            //        //tbd    sd.DrawSingleObject( pb, bodys, new Point(0,0));
            //            if (pb.Image != null)
            //            {
            //                //pb.Image.Save($"c:\\code\\dump\\image_{count}_{bodys.Name()}.png");
            //                System.Diagnostics.Debug.WriteLine($"dump {count} {bodys.OwnName} parent `{bodys.Parent?.OwnName}` scan data `{bodys.Scan?.BodyName}`");
            //                count++;
            //            }
            //        }
            //        System.Diagnostics.Debug.Write("End dump");

            //        // if we find it, we are targetting a body (note orbiting stations are not added to the nodesbyid even though they get bodyids)


            //        if (body != null)
            //        {
            //            System.Diagnostics.Debug.WriteLine($".. body found  {body.OwnName} {body.CanonicalName}");

            //            if (body.Scan != null)
            //            {
            //                System.Diagnostics.Debug.WriteLine($".. body dist {body.Scan.DistanceFromArrivalLS.ToString("N0")} ls");
            //            }

            // all the stuff about the station
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
            Font f = FontHelpers.FontSelection(this.FindForm(), displayfont ?? this.Font);     // will be null on cancel
            string setting = FontHelpers.GetFontSettingString(f);
            //System.Diagnostics.Debug.WriteLine($"Surveyor Font selected {setting}");
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
