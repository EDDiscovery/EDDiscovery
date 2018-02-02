﻿/*
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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EliteDangerousCore.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlRouteTracker :   UserControlCommonBase
    {
        private Font displayfont;
        private string DbSave { get { return "RouteTracker" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private SavedRouteClass currentRoute;
        private  HistoryEntry currentHE;
        private string lastsystem;

        public override Color ColorTransparency { get { return Color.Green; } }
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
            displayfont = discoveryform.theme.GetFont;

            autoCopyWPToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "autoCopyWP", false);
            autoSetTargetToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "autoSetTarget", false);
            string ids = SQLiteDBClass.GetSettingString(DbSave + "SelectedRoute", "-1");        // for some reason, it was saved as a string.. so keep for backwards compat
            int? id = ids.InvariantParseIntNull();
            if ( id != null )
                currentRoute = SavedRouteClass.GetAllSavedRoutes().Find(r => r.Id.Equals(id.Value));  // may be null

            Display();

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
        }

        public override void Closing()
        {
            SQLiteDBClass.PutSettingBool(DbSave + "autoCopyWP", autoCopyWPToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "autoSetTarget", autoSetTargetToolStripMenuItem.Checked);
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
        }

        #endregion

        #region Implementation

        public override void InitialDisplay()
        {
            Display(discoveryform.history);
        }

        private void Display(HistoryList hl)            // when user clicks around..  HE may be null here
        {
            currentHE = hl.GetLastFSD;
            Display();
        }

        private void NewEntry(HistoryEntry l, HistoryList hl)
        {
            currentHE = hl.GetLastFSD;
            Display();
        }

        private void Display()
        {
            if (currentHE == null)
                return;

            Display(currentHE.System);

          //  t.Interval = 200; t.Tick += (s,e)=> { Display(currentRoute.PosAlongRoute(percent)); percent += 0.2; }; t.Start();  // debug to make it play thru.. leave
        }

        // double percent = 0; Timer t = new Timer();// play thru harness

        private void Display(ISystem cursys)
        {
            if (currentRoute == null)
            {
                DisplayText("Please set a route, by right clicking", "");
                return;
            }

            if (currentRoute.Systems.Count == 0)
            {
                DisplayText(currentRoute.Name, "Route contains no waypoints");
                return;
            }

            string topline = "";

            ISystem finalSystem = SystemClassDB.GetSystem(currentRoute.Systems[currentRoute.Systems.Count - 1]);

            if (finalSystem != null && cursys.HasCoordinate)
            {
                string mesg = "remain";
                double distX = cursys.Distance(finalSystem);
                //Small hack to pull the jump range from TripPanel1
                var jumpRange = SQLiteDBClass.GetSettingDouble("TripPanel1" + "JumpRange", -1.0);       //TBD Not a good idea.
                if (jumpRange > 0)
                {
                    int jumps = (int)Math.Ceiling(distX / jumpRange);
                    if (jumps > 0)
                        mesg = "@ " + jumps.ToString() + ((jumps == 1) ? " jump" : " jumps");
                }
                topline = String.Format("{0} {1} WPs, {2:N2}ly {3}", currentRoute.Name, currentRoute.Systems.Count, distX, mesg);
            }
            else
            {
                topline = String.Format("{0} {1} WPs remain", currentRoute.Name, currentRoute.Systems.Count);
            }

            string bottomline = "";

            Tuple<ISystem, int> closest = cursys.HasCoordinate ? currentRoute.ClosestTo(cursys) : null;

            if (closest != null)
            {
                if (closest.Item2 >= currentRoute.Systems.Count) // if past end..
                {
                    bottomline = String.Format("Past Last WP{0} {1}", closest.Item2, currentRoute.LastSystem);
                }
                else
                {
                    string name = null;

                    if (closest.Item1 != null )         // if have a closest system
                    {
                        double distance = cursys.Distance(closest.Item1);

                        bottomline = String.Format("{0:N2}ly to WP{1} {2} @ {3},{4},{5}", distance, closest.Item2 + 1, closest.Item1.Name,
                            closest.Item1.X.ToString("0.#"), closest.Item1.Y.ToString("0.#"), closest.Item1.Z.ToString("0.#"));

                        name = closest.Item1.Name;
                    }
                    else
                    {           // just know waypoint..
                        bottomline = String.Format("To WP{0} {1}", closest.Item2 + 1, currentRoute.Systems[closest.Item2]);

                        name = currentRoute.Systems[closest.Item2];
                    }

                    if (lastsystem == null || name.CompareTo(lastsystem) != 0)
                    {
                        if (autoCopyWPToolStripMenuItem.Checked)
                            Clipboard.SetText(name);

                        if (autoSetTargetToolStripMenuItem.Checked)
                        {
                            string targetName;
                            double x, y, z;
                            TargetClass.GetTargetPosition(out targetName, out x, out y, out z);
                            if (name.CompareTo(targetName) != 0)
                                TargetHelpers.setTargetSystem(this, discoveryform, name, false);
                        }

                        lastsystem = name;
                    }
                }
            }
            else
                bottomline = "No current position/no systems found in database";

            DisplayText(topline, bottomline);
        }

        void DisplayText(String topline, String bottomLine)
        {
            pictureBox.ClearImageList();
            Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
            Color backcolour = IsTransparent ? Color.Transparent : this.BackColor;
            pictureBox.AddTextAutoSize(new Point(10, 5), new Size(1000, 35), topline == null ? "" : topline, displayfont, textcolour, backcolour, 1.0F);
            pictureBox.AddTextAutoSize(new Point(10, 35), new Size(1000, 35), bottomLine == null ? "" : bottomLine, displayfont, textcolour, backcolour, 1.0F);
            pictureBox.Render();
        }

        #endregion

        #region UI

        private void autoCopyWPToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Display();
        }

        private void setRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExtendedControls.ConfigurableForm f = new ExtendedControls.ConfigurableForm();

            var routes = SavedRouteClass.GetAllSavedRoutes();
            var routenames = (from x in routes select x.Name).ToList();
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Route", "", new Point(10, 40), new Size(400, 24), "Select route", routenames, new Size(400, 400)));
            f.Add(new ExtendedControls.ConfigurableForm.Entry("Cancel", typeof(ExtendedControls.ButtonExt), "Cancel", new Point(410-100, 80), new Size(100, 24), "Press to Cancel"));
            f.Trigger += (dialogname, controlname, tag) =>
            {
                if (controlname != "Route")
                    f.DialogResult = DialogResult.Cancel;
                else
                    f.DialogResult = DialogResult.OK;
                f.Close();
            };
            if ( f.ShowDialog(this.FindForm(), this.FindForm().Icon, new Size(430, 120), new Point(-999, -999), "Enter route") == DialogResult.OK )
            {
                string routename = f.Get("Route");
                currentRoute = routes.Find(x => x.Name.Equals(routename));       // not going to be null, but consider the upset.
                if (currentRoute != null)
                    SQLiteDBClass.PutSettingString(DbSave + "SelectedRoute", currentRoute.Id.ToStringInvariant());        // write ID back

                Display();
            }
        }

        #endregion


    }
}
