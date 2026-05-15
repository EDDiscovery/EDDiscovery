/*
 * Copyright 2016 - 2025 EDDiscovery development team
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
using EliteDangerousCore.JournalEvents;
using ExtendedControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class SurveyorPanel : UserControlCommonBase
    {
        // Route selection
        private const string NavRouteNameLabel = "!*NavRoute";      // special label to identify a save of using the nav route - not user presented
        private string translatednavroutename = "";     // presented to the user, found by the translator
        private SavedRouteClass currentRoute = null;    // the current route selected by the user. ID=-1 means a navroute
        private string lastsystemonroute;               // last system on route, used to check for updates


        // draw route, direct, no async. Sys is current system
        private void DrawRoute(ISystem sys)
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor draw route {sys?.Name}");

            string lastroutetext = "No System Info";

            SavedRouteClass.ClosestInfo closest = null;

            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Current route set, sys has coord, route is {currentRoute.Systems.Count} {currentRoute.Id}");
            if (sys != null && currentRoute != null && sys.HasCoordinate)      // must have a system, a current route, and coord
            {
                if (currentRoute.Systems.Count == 0)
                {
                    lastroutetext = "Route contains no waypoints".Tx();
                }
                else
                {
                    closest = currentRoute.ClosestTo(sys, currentRouteManualTarget);

                    if (closest == null)  // if null, no systems found.. uh oh
                    {
                        lastroutetext = "No systems in route have known co-ords".Tx();
                    }
                    else
                    {
                        double routedistance = currentRoute.CumulativeDistance();       // total distance

                        double distleft = closest.disttowaypoint + (closest.deviation < 0 ? 0 : closest.cumulativewpdist);

                        //System.Diagnostics.Debug.WriteLine($"Surveyor: {closest.lastsystem?.Name}->{closest.nextsystem?.Name} {closest.waypoint} distance to {closest.disttowaypoint} dev {closest.deviation} cuml after wp {closest.cumulativewpdist} inc wp {distleft} route {routedistance}");

                        lastroutetext = $"{currentRoute.Name} {currentRoute.Systems.Count} WPs, {routedistance:N1}ly -> {closest.finalsystem.Name}";

                        string jumpmsg = "";
                        if (IsSet(RouteControl.showJumps))
                        {
                            if (shipfsdinfo != null)
                            {
                                // navroutes are precomputed, so the total jump count is from it. Else do it by distance
                                int jumps = currentRoute.Id != -1 ? (int)Math.Ceiling(routedistance / shipfsdinfo.avgsinglejump) : currentRoute.Systems.Count - 1;
                                if (jumps > 0)
                                    lastroutetext += ", " + jumps.ToString() + " " + ((jumps == 1) ? "jump".Tx() : "jumps".Tx());

                                jumps = (int)Math.Ceiling(closest.disttowaypoint / shipfsdinfo.avgsinglejump);

                                if (jumps > 1 && currentRoute.Id != -1) // if more than 1 jump, and not nav route
                                    jumpmsg = ", " + jumps.ToString() + " " + ((jumps == 1) ? "jump".Tx() : "jumps".Tx());
                            }
                            else
                                jumpmsg = " No Ship FSD Information".Tx();
                        }

                        string wpposmsg = "";
                        if (IsSet(RouteControl.showwaypoints))
                            wpposmsg = String.Format(" @{0:N1},{1:N1},{2:N1}", closest.nextsystem.X, closest.nextsystem.Y, closest.nextsystem.Z);

                        if (closest.deviation < 0)        // if not on path
                        {
                            lastroutetext += Environment.NewLine;
                            lastroutetext += closest.cumulativewpdist == 0 ? "From Last WP ".Tx() : "To First WP ".Tx();
                            lastroutetext += $" >> {closest.disttowaypoint:N1}ly >> " + closest.nextsystem.Name + wpposmsg + jumpmsg;
                        }
                        else
                        {
                            lastroutetext += String.Format(", Left {0:N1}ly".Tx(), distleft) + Environment.NewLine;

                            if (distleft == 0)
                                lastroutetext += $"{closest.nextsystem.Name}";
                            else
                            {
                                // if in auto mode, and we have a last system, we give the last >> dist >> next

                                if (currentRouteManualTarget < 0 && closest.lastsystem != null)
                                    lastroutetext += $"{closest.lastsystem?.Name} >> {closest.disttowaypoint:N1}ly >> {closest.nextsystem.Name}";
                                else
                                    lastroutetext += $"{closest.disttowaypoint:N1}ly >> {closest.nextsystem.Name}";
                            }

                            lastroutetext += " " + String.Format("(WP {0})".Tx(), closest.waypoint + 1);
                            lastroutetext += wpposmsg + jumpmsg;

                            if (IsSet(RouteControl.showdeviation) && closest.deviation > 0)
                                lastroutetext += String.Format(", Dev {0:N1}ly".Tx(), closest.deviation);
                        }

                        //System.Diagnostics.Debug.WriteLine(lastroutetext);
                        //System.Diagnostics.Debug.WriteLine("---");

                        if (IsSet(RouteControl.showbookmarks))
                        {
                            BookmarkClass bookmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(sys.Name);
                            if (bookmark != null)
                                lastroutetext += Environment.NewLine + String.Format("Note: {0}".Tx(), bookmark.Note);
                        }

                        if (IsSet(RouteControl.shownotetext))
                        {
                            // closest lastsystem can be null. If we are at the last system, we print both last system and this next waypoint
                            if (closest.lastsystemwaypointnote.HasChars())
                                lastroutetext = lastroutetext.AppendPrePad(closest.lastsystem.Name + ": " + closest.lastsystemwaypointnote, Environment.NewLine);

                            if (closest.nextsystemwaypointnote.HasChars())
                                lastroutetext = lastroutetext.AppendPrePad(closest.nextsystem.Name + ": " + closest.nextsystemwaypointnote, Environment.NewLine);
                        }

                        // if a new closestsystem, then autocopy

                        if (lastsystemonroute == null || closest.nextsystem.Name.CompareTo(lastsystemonroute) != 0)
                        {
                            if (IsSet(RouteControl.autocopy))
                                SetClipboardText(closest.nextsystem.Name);

                            if (IsSet(RouteControl.settarget))
                            {
                                if (TargetClass.SetTargetOnSystemConditional(closest.nextsystem.Name, closest.nextsystem.X, closest.nextsystem.Y, closest.nextsystem.Z))
                                {
                                    DiscoveryForm.NewTargetSet(this);
                                }
                            }

                            lastsystemonroute = closest.nextsystem.Name;
                        }
                    }
                }
            }

            extPictureBoxRoute.ClearImageList();

            Point pos = new Point(3, 20);


            extPictureBoxRoute.AddPictureTextHorzDivider(
                             new Rectangle(pos.X, pos.Y, Math.Max(extPictureBoxSystemDetails.Width - 6 - pos.X, 24), 10000),
                             null, Size.Empty,
                             lastroutetext, displayfont ?? this.Font, extCheckBoxWordWrap.Checked, alignment,
                             IsSet(CtrlList.showdividers),
                             IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor,
                             IsTransparentModeOn ? Color.Transparent : this.BackColor
                             );

            // display the manual selection buttons when appropriate
            if (closest != null && !IsCurrentlyTransparent)
            {
                if (alignment == StringAlignment.Near)
                {
                    extPictureBoxRoute[0].X += 44;
                }
                else
                    pos = new Point(extPictureBoxRoute[0].X - 44, pos.Y);

                var textcolour = IsTransparentModeOn ? ExtendedControls.Theme.Current.SPanelColor : ExtendedControls.Theme.Current.LabelColor;
                var but1 = new ExtendedControls.ImageElement.Button() { Text = "<", Font = this.Font, Bounds = new Rectangle(pos.X, pos.Y, 20, 20) };
                but1.Enabled = currentRouteManualTarget > 0;
                but1.BackColor = Color.Transparent; but1.ForeColor = textcolour; but1.ButtonFaceColor = Theme.Current.ButtonBackColor; but1.MouseOverFaceColor = Theme.Current.ButtonBackColor.Multiply(1.3f);
                but1.Click += (pb, iel, w) => { currentRouteManualTarget = currentRouteManualTarget > 0 ? currentRouteManualTarget - 1 : 0; DrawRoute(cur_sys); };
                extPictureBoxRoute.Add(but1);

                var but2 = new ExtendedControls.ImageElement.Button() { Text = ">", Font = this.Font, Bounds = new Rectangle(but1.Bounds.Right, but1.Bounds.Top, 20, 20) };
                but2.Enabled = currentRouteManualTarget < 0 || currentRouteManualTarget < currentRoute.Systems.Count - 1;
                but2.BackColor = Color.Transparent; but2.ForeColor = textcolour; but2.ButtonFaceColor = Theme.Current.ButtonBackColor; but2.MouseOverFaceColor = Theme.Current.ButtonBackColor.Multiply(1.3f);
                but2.Click += (pb, iel, w) => { currentRouteManualTarget = currentRouteManualTarget < currentRoute.Systems.Count - 1 ? currentRouteManualTarget + 1 : 0; DrawRoute(cur_sys); };
                extPictureBoxRoute.Add(but2);
            }

            extPictureBoxRoute.Render();
        }



        ExtendedControls.ExtListBoxForm dropdown;
        private void extButtonSetRoute_Click(object sender, EventArgs e)
        {
            ExtendedControls.ExtButton but = sender as ExtendedControls.ExtButton;

            dropdown = new ExtendedControls.ExtListBoxForm("", true);

            var savedroutes = SavedRouteClass.GetAllSavedRoutes();

            dropdown.FitImagesToItemHeight = true;
            var list = savedroutes.Select(x => x.Name).ToList();
            list.Insert(0, "Off".Tx());
            list.Insert(1, translatednavroutename);
            dropdown.Items = list;
            dropdown.FlatStyle = FlatStyle.Popup;
            dropdown.PositionBelow(sender as Control);
            dropdown.SelectedIndexChanged += (s, ea, key) =>
            {
                if (dropdown.SelectedIndex == 0)    // off
                {
                    LoadRoute("");
                }
                else if (dropdown.SelectedIndex == 1)      // navroute
                {
                    LoadRoute(NavRouteNameLabel);
                }
                else
                {
                    string name = savedroutes[dropdown.SelectedIndex - 2].Name;
                    LoadRoute(name);
                }

                SetVisibility();
                DrawRoute(cur_sys);
            };

            ExtendedControls.Theme.Current.ApplyDialog(dropdown, true);
            dropdown.Show(this.FindForm());
        }

        private void LoadRoute(string name, int manualpos = -1)
        {
            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Order load of route '{name}'");
            PutSetting(dbRouteName, name);      // store back the current name - this is used to wipe out a route with LoadRoute("")

            //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} In DB its now '{GetSetting(dbRouteName,"???")}'");

            currentRoute = null;        // clear route and held text
            currentRouteManualTarget = -1;

            if (name.HasChars())
            {
                if (name.Equals(NavRouteNameLabel))     // if selecting navroutes
                {
                    // find last from history
                    var route = DiscoveryForm.History.GetLastHistoryEntry(x => x.EntryType == JournalTypeEnum.NavRoute)?.journalEntry as EliteDangerousCore.JournalEvents.JournalNavRoute;

                    if (route?.Route != null)   // if found
                    {
                        // pick out x/y/z to fill in route so it does not need any system lookup
                        var systems = route.Route.Where(x => x.StarSystem.HasChars()).
                                Select(rt => new SavedRouteClass.SystemEntry(rt.StarSystem, "", rt.StarPos.X, rt.StarPos.Y, rt.StarPos.Z)).ToList();

                        currentRoute = new SavedRouteClass(translatednavroutename, systems);      // with an ID of -1 note, used to detect navroutes
                        currentRouteManualTarget = manualpos;
                        //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Loaded Nav route with {systems.Length}");
                    }
                    else
                    {
                        currentRoute = new SavedRouteClass();
                        currentRoute.Name = translatednavroutename;     // no known systems yet, but make a navroute so we have it selected
                        //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} No route available, loaded empty Nav route");
                    }
                }
                else
                {
                    var savedroutes = SavedRouteClass.GetAllSavedRoutes();      // load routes
                    currentRoute = savedroutes.Find(x => x.Name == name);       // pick, if not found, will be null
                    currentRoute?.FillInCoordinates();                           // fill in any co-ords into DB - it may be in the DB without known co-ords
                    currentRouteManualTarget = manualpos;
                    //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} Loaded route with {currentRoute?.Systems.Count}");
                }
            }
            else
            {
                //System.Diagnostics.Debug.WriteLine($"Surveyor {displaynumber} No route wanted '{name}'");
            }
        }

        private string routecontrolsettings;

        enum RouteControl
        {
            showJumps,
            showwaypoints,
            showdeviation,
            showbookmarks,
            autocopy,
            settarget,
            showtarget,
            showfuel,
            shownotetext,
        };

        private bool IsSet(RouteControl c)
        {
            return routecontrolsettings.Contains(c.ToString());
        }
        private void extButtonControlRoute_Click(object sender, EventArgs e)
        {
            ExtendedControls.CheckedIconNewListBoxForm displayfilter = new CheckedIconNewListBoxForm();

            displayfilter.UC.AddAllNone();
            displayfilter.UC.Add(RouteControl.showJumps.ToString(), "Show Jumps To Go".Tx());
            displayfilter.UC.Add(RouteControl.showwaypoints.ToString(), "Show Waypoint Coordinates".Tx());
            displayfilter.UC.Add(RouteControl.showdeviation.ToString(), "Show Deviation from route".Tx());
            displayfilter.UC.Add(RouteControl.showbookmarks.ToString(), "Show Bookmark Notes".Tx());
            displayfilter.UC.Add(RouteControl.shownotetext.ToString(), "Show Route Note on waypoint".Tx());
            displayfilter.UC.Add(RouteControl.autocopy.ToString(), "Auto copy waypoint".Tx());
            displayfilter.UC.Add(RouteControl.settarget.ToString(), "Auto set target".Tx());
            displayfilter.UC.Add(RouteControl.showtarget.ToString(), "Show Target Information".Tx());
            displayfilter.UC.Add(RouteControl.showfuel.ToString(), "Show Fuel Information".Tx());

            displayfilter.AllOrNoneBack = false;
            displayfilter.UC.ImageSize = new Size(24, 24);
            displayfilter.UC.ScreenMargin = new Size(0, 0);
            displayfilter.CloseBoundaryRegion = new Size(32, ((Control)sender).Height);

            displayfilter.SaveSettings = (s, o) =>
            {
                routecontrolsettings = s;
                PutSetting(dbroutecontrol, s);
                DrawRoute(cur_sys);
                DrawFuel();
                SetVisibility();
            };

            displayfilter.Show(routecontrolsettings, (Control)sender, this.FindForm());
        }

    }
}

  