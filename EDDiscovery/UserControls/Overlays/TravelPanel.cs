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
using EliteDangerousCore.UIEvents;
using ExtendedControls;
using System.Drawing;
using System.Linq;
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

        UIOverallStatus uistatus;
        HistoryEntry lasthe;

        protected override void Init()
        {
            DiscoveryForm.OnNewUIEvent += DiscoveryForm_OnNewUIEvent;
            DiscoveryForm.OnNewEntry += DiscoveryForm_OnNewEntry;
            DiscoveryForm.OnHistoryChange += DiscoveryForm_OnHistoryChange;

            // if opened after start, we need to pick up state from discoveryform

            uistatus = DiscoveryForm.UIOverallStatus;
            lasthe = DiscoveryForm.History.GetLast;
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
            System.Diagnostics.Debug.WriteLine($"Autopanel NewHistory {he.EventTimeUTC}");
            lasthe = DiscoveryForm.History.GetLast;
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
            // use for debug purposes, follows history cursor
            // lasthe = he; UpdateDisplay();
        }

        async void UpdateDisplay()
        {
            string text = "";
            if (uistatus.MajorMode == UIMode.MajorModeType.MainShip)
            {
                if (uistatus.Mode == UIMode.ModeType.MainShipNormalSpace)
                {
                    text = "Normal space";
                    // show destination if set, if approach body show that, get body info from scan data
                }
                else if (uistatus.Mode == UIMode.ModeType.MainShipDockedStarPort)
                {
                    text = "Docked Starport";
                    // show information about the station, economy, etc
                }
                else if (uistatus.Mode == UIMode.ModeType.MainShipDockedPlanet)
                {
                    text = "Docked Planet";
                    // show information about the station, economy, etc
                }
                else if (uistatus.Mode == UIMode.ModeType.MainShipSupercruise)
                {
                    bool injump = uistatus.Flags.Contains(UITypeEnum.FsdJump);
                    if (injump)
                        text = "Jumping to ";
                    else
                        text = "Supercruising"; // tbd to?

                    // show destination if set, if approach body show that, get body info from scan data
                }
                else if (uistatus.Mode == UIMode.ModeType.MainShipLanded)
                {
                    text = "Landed";
                    // show destination if set, if approach body show that, get body info from scan data
                }
            }
            else if (uistatus.MajorMode == UIMode.MajorModeType.SRV)
            {
                text = "SRV";
                // show information about the station, economy, etc
            }
            else if (uistatus.MajorMode == UIMode.MajorModeType.Fighter)
            {
                text = "Fighter";
                // show information about the station, economy, etc
            }
            if (uistatus.MajorMode == UIMode.MajorModeType.OnFoot)
            {
                text = "OnFoot";
                // show information about planet or station
            }

            label1.Text = text;

            if (uistatus.DestinationName.HasChars() && uistatus.DestinationSystemAddress.HasValue)
            {
                var uis = uistatus;     // async below may result in a change to this, protect, found during debugging! This async stuff is evil

                // lookup system..

                ISystem sys = DiscoveryForm.History.StarScan2.GetISystemWithCache(uis.DestinationSystemAddress.Value, WebExternalDataLookup.SpanshThenEDSM);
                    
                if (sys != null)    // we have it, find info on system
                {
                    var ss = await DiscoveryForm.History.StarScan2.FindSystemAsync(sys, WebExternalDataLookup.Spansh);

                    if (IsClosed)
                        return;

                    System.Diagnostics.Debug.WriteLine($"Travel Panel Dest found system  {sys.Name} {uis.DestinationSystemAddress} -> {ss?.System.Name}");

                    //  body name destination or $POI $MULTIPLAYER etc
                    // Now (oct 25) its localised, but if not, so attempt a rename for those $xxx forms ($Multiplayer.. $POI)

                    string destname = uis.DestinationName_Localised.Alt(JournalFieldNaming.SignalBodyName(uis.DestinationName));

                    System.Diagnostics.Debug.WriteLine($".. Travel Destination non star {destname}");

                    // with a found system, see if we can get the body name so we know what body its on (defensive -1 in case bodyid = null)
                    if (ss != null)
                    {
                        var sd = new EliteDangerousCore.StarScan2.SystemDisplay();
                        sd.SetSize(48);
                        sd.Font = this.Font;
                        sd.TextBackColor = Color.Transparent;
                        ExtPictureBox pb = new ExtPictureBox();
                        sd.DrawSystemRender(pb, 800, ss);

                        //System.Diagnostics.Debug.Write("Dumping images");
                        //pb.Image.Save(@"c:\code\dump\systemdisplay.png");

                        int count = 0;
                        foreach (var bodys in ss.Bodies())
                        {
                            sd.DrawSingleObject( pb, bodys);
                            if (pb.Image != null)
                            {
                                //pb.Image.Save($"c:\\code\\dump\\image_{count}_{bodys.Name()}.png");
                                System.Diagnostics.Debug.WriteLine($"dump {count} {bodys.OwnName} parent `{bodys.Parent?.OwnName}` scan data `{bodys.Scan?.BodyName}`");
                                count++;
                            }
                        }
                        System.Diagnostics.Debug.Write("End dump");

                        // if we find it, we are targetting a body (note orbiting stations are not added to the nodesbyid even though they get bodyids)

                        EliteDangerousCore.StarScan2.BodyNode body = uis.DestinationBodyID.HasValue ? ss.FindBody(uis.DestinationBodyID.Value) : null;
                        if (body != null)
                        {
                            System.Diagnostics.Debug.WriteLine($".. body found  {body.OwnName} {body.CanonicalName}");

                            if (body.Scan != null)
                            {
                                System.Diagnostics.Debug.WriteLine($".. body dist {body.Scan.DistanceFromArrivalLS.ToString("N0")} ls");
                            }
                        }

                        IBodyFeature feature = ss.GetFeature(destname);
                        if (feature != null)
                        {
                            System.Diagnostics.Debug.WriteLine($".. feature found {feature.Name} {feature.BodyName} {feature.BodyType}");
                        }
                    }
                }

            }

            // all the stuff about the station
        }
    }
}
