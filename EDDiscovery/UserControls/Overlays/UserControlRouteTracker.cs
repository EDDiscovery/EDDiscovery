/*
 * Copyright © 2017 EDDiscovery development team
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
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public partial class UserControlRouteTracker :   UserControlCommonBase
    {
        private Font displayfont;
        private SavedRouteClass currentRoute;
        private  HistoryEntry currentHE;
        private string lastsystem;

        public override bool SupportTransparency { get { return true; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            Display();
        }

        #region Init

        public UserControlRouteTracker()
        {
            InitializeComponent();
        }

        public override void Init()
        {
            DBBaseName = "RouteTracker";

            displayfont = discoveryform.theme.GetFont;

            showJumpsToolStripMenuItem.Checked = GetSetting("showjumps", true);
            autoCopyWPToolStripMenuItem.Checked = GetSetting("autoCopyWP", false);
            autoSetTargetToolStripMenuItem.Checked = GetSetting("autoSetTarget", false);
            showWaypointCoordinatesToolStripMenuItem.Checked = GetSetting("coords", true);
            showDeviationFromRouteToolStripMenuItem.Checked = GetSetting("dev", true);
            showBookmarkNotesToolStripMenuItem.Checked = GetSetting("bookmarkNotes", true);

            string ids = GetSetting("SelectedRoute", "-1");        // for some reason, it was saved as a string.. so keep for backwards compat
            int? id = ids.InvariantParseIntNull();
            if ( id != null )
                currentRoute = SavedRouteClass.GetAllSavedRoutes().Find(r => r.Id.Equals(id.Value));  // may be null

            Display();

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
            GlobalBookMarkList.Instance.OnBookmarkChange += GlobalBookMarkList_OnBookmarkChange;

            BaseUtils.Translator.Instance.Translate(this);
            BaseUtils.Translator.Instance.Translate(contextMenuStrip, this);
       }

        public override void Closing()
        {
            PutSetting("showjumps", showJumpsToolStripMenuItem.Checked);
            PutSetting("autoCopyWP", autoCopyWPToolStripMenuItem.Checked);
            PutSetting("autoSetTarget", autoSetTargetToolStripMenuItem.Checked);
            PutSetting("coords", showWaypointCoordinatesToolStripMenuItem.Checked);
            PutSetting("dev", showDeviationFromRouteToolStripMenuItem.Checked);
            PutSetting("bookmarkNotes", showBookmarkNotesToolStripMenuItem.Checked);
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            GlobalBookMarkList.Instance.OnBookmarkChange -= GlobalBookMarkList_OnBookmarkChange;
        }

        #endregion

        #region Implementation

        public override void InitialDisplay()
        {
            Display(discoveryform.history);
        }

        private void Display(HistoryList hl)            // when user clicks around..  HE may be null here
        {
            currentHE = hl.GetLastFSDCarrierJump();
            Display();
        }

        private void NewEntry(HistoryEntry l, HistoryList hl)
        {
            currentHE = hl.GetLastFSDCarrierJump();
            Display();
        }

        private void GlobalBookMarkList_OnBookmarkChange(BookmarkClass bk, bool deleted)
        {
            Display();
        }


        private void Display()
        {
            if (currentHE == null)
                return;

            Display(currentHE.System);

        //   t.Interval = 200; t.Tick += (s,e)=> { Display(currentRoute.PosAlongRoute(percent,100)); percent += 0.5; }; t.Start();  // debug to make it play thru.. leave
        }

     //    double percent = -10; Timer t = new Timer();// play thru harness

        private void Display(ISystem cursys)
        {
            if (currentRoute == null)
            {
                DisplayText("Please set a route, by right clicking".T(EDTx.UserControlRouteTracker_NoRoute), "", "");
                return;
            }

            if (currentRoute.Systems.Count == 0)
            {
                DisplayText(currentRoute.Name, "Route contains no waypoints".T(EDTx.UserControlRouteTracker_NoWay), "");
                return;
            }

            string topline = "", bottomline = "", note = "";

            if (!cursys.HasCoordinate)
            {
                topline = String.Format("Unknown location".T(EDTx.UserControlRouteTracker_Unk));
                bottomline = "";
            }
            else
            {
                SavedRouteClass.ClosestInfo closest = currentRoute.ClosestTo(cursys);

                if (closest == null)  // if null, no systems found.. uh oh
                {
                    topline = String.Format("No systems in route have known co-ords".T(EDTx.UserControlRouteTracker_NoCo));
                    bottomline = "";
                }
                else
                {
                    double routedistance = currentRoute.CumulativeDistance();
                    double distleft = closest.disttowaypoint + (closest.deviation < 0 ? 0 : closest.cumulativewpdist);

                    topline = String.Format("{0} {1} WPs, {2:N1}ly", currentRoute.Name,
                                    currentRoute.Systems.Count, routedistance);

                    EliteDangerousCalculations.FSDSpec.JumpInfo ji = currentHE.GetJumpInfo(discoveryform.history.MaterialCommoditiesMicroResources.CargoCount(currentHE.MaterialCommodity));
                    string jumpmsg = "";
                    if (showJumpsToolStripMenuItem.Checked && ji != null )
                    {
                        int jumps = (int)Math.Ceiling(routedistance / ji.avgsinglejump);
                        if (jumps > 0)
                            topline += ", " + jumps.ToString() + " " + ((jumps == 1) ? "jump".T(EDTx.UserControlRouteTracker_J1) : "jumps".T(EDTx.UserControlRouteTracker_JS));

                        jumps = (int)Math.Ceiling(closest.disttowaypoint / ji.avgsinglejump);

                        if (jumps > 0)
                            jumpmsg = ", " + jumps.ToString() + " " + ((jumps == 1) ? "jump".T(EDTx.UserControlRouteTracker_J1) : "jumps".T(EDTx.UserControlRouteTracker_JS));
                        else
                            jumpmsg = " No Ship FSD Information".T(EDTx.UserControlRouteTracker_NoFSD);
                    }

                    string wpposmsg = closest.system.Name;
                    if ( showWaypointCoordinatesToolStripMenuItem.Checked )
                        wpposmsg += String.Format(" @{0:N1},{1:N1},{2:N1}", closest.system.X, closest.system.Y, closest.system.Z);
                    wpposmsg += String.Format(" {0:N1}ly", closest.disttowaypoint);

                    if (closest.deviation < 0)        // if not on path
                    {
                        bottomline += closest.cumulativewpdist == 0 ? "From Last WP ".T(EDTx.UserControlRouteTracker_FL) : "To First WP ".T(EDTx.UserControlRouteTracker_TF);
                        bottomline += wpposmsg + jumpmsg;
                    }
                    else
                    {
                        topline += String.Format(", Left {0:N1}ly".T(EDTx.UserControlRouteTracker_LF), distleft);
                        bottomline += String.Format("To WP {0} ".T(EDTx.UserControlRouteTracker_ToWP), closest.waypoint + 1);
                        bottomline += wpposmsg + jumpmsg;
                        if ( showDeviationFromRouteToolStripMenuItem.Checked)
                            bottomline += String.Format(", Dev {0:N1}ly".T(EDTx.UserControlRouteTracker_Dev), closest.deviation);
                    }

                    if (showBookmarkNotesToolStripMenuItem.Checked)
                    {
                        BookmarkClass bookmark = GlobalBookMarkList.Instance.FindBookmarkOnSystem(cursys.Name);
                        if (bookmark != null )
                            note = String.Format("Note: {0}".T(EDTx.UserControlRouteTracker_Note), bookmark.Note);
                    }

                    //System.Diagnostics.Debug.WriteLine("T:" + topline + Environment.NewLine + "B:" + bottomline);
                    string name = closest.system.Name;

                    if (lastsystem == null || name.CompareTo(lastsystem) != 0)
                    {
                        if (autoCopyWPToolStripMenuItem.Checked)
                            SetClipboardText(name);

                        if (autoSetTargetToolStripMenuItem.Checked)
                        {
                            string targetName;
                            double x, y, z;
                            TargetClass.GetTargetPosition(out targetName, out x, out y, out z);
                            if (name.CompareTo(targetName) != 0)
                                TargetHelpers.SetTargetSystem(this, discoveryform, name, false);
                        }

                        lastsystem = name;
                    }
                }
            }

            DisplayText(topline, bottomline, note);
        }

        void DisplayText(String topline, String bottomLine, String note)
        {
            pictureBox.ClearImageList();
            Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
            Color backcolour = IsTransparent ? Color.Transparent : this.BackColor;
            var ie = pictureBox.AddTextAutoSize(new Point(10, 5), new Size(10000, 100), topline == null ? "" : topline, displayfont, textcolour, backcolour, 1.0F);
            var bt = pictureBox.AddTextAutoSize(new Point(10, ie.Location.Bottom + displayfont.ScalePixels(4)), new Size(10000, 100), bottomLine == null ? "" : bottomLine, displayfont, textcolour, backcolour, 1.0F);
            pictureBox.AddTextAutoSize(new Point(10, bt.Location.Bottom + displayfont.ScalePixels(4)), new Size(10000, 100), note == null ? "" : note, displayfont, textcolour, backcolour, 1.0F);
            pictureBox.Render();
        }

        #endregion

        #region UI

        private void autoCopyWPToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void showJumpsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display();
        }

        private void showWaypointCoordinatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display();
        }

        private void showDeviationFromRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display();
        }

        private void showBookmarkNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Display();
        }

        private void setRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            var routes = SavedRouteClass.GetAllSavedRoutes();
            var routenames = (from x in routes select x.Name).ToList();
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Route", "", new Point(10, 40), new Size(400, 24), "Select route".T(EDTx.UserControlRouteTracker_Selectroute), routenames));
            f.AddCancel(new Point(410 - 80, 80), "Press to Cancel".T(EDTx.UserControlRouteTracker_PresstoCancel));

            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname == "Cancel" || controlname == "Close")
                    f.ReturnResult(DialogResult.Cancel);
                else if (controlname == "Route")
                    f.ReturnResult(DialogResult.OK);
            };

            if ( f.ShowDialogCentred(this.FindForm(), this.FindForm().Icon,  "Enter route".T(EDTx.UserControlRouteTracker_Enterroute), closeicon:true) == DialogResult.OK )
            {
                string routename = f.Get("Route");
                currentRoute = routes.Find(x => x.Name.Equals(routename));       // not going to be null, but consider the upset.

                //currentRoute.TestHarness(); // enable for debug testing of route finding

                if (currentRoute != null)
                    PutSetting("SelectedRoute", currentRoute.Id.ToStringInvariant());        // write ID back

                Display();
            }
        }

        #endregion
    }
}
