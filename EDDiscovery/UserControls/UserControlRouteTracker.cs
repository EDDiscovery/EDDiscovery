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
using EDDiscovery.DB;
using EliteDangerousCore;

namespace EDDiscovery.UserControls
{
    public partial class UserControlRouteTracker :   UserControlCommonBase
    {
        private EDDiscoveryForm discoveryform;
        private int displaynumber = 0;
        private Font displayfont;
        private string DbSave { get { return "RouteTracker" + ((displaynumber > 0) ? displaynumber.ToString() : ""); } }
        private SavedRouteClass _currentRoute;
        private  HistoryEntry currentSystem;
        private string lastsystem;

        public UserControlRouteTracker()
        {
            InitializeComponent();
        }

        public override void Init(EDDiscoveryForm ed, UserControlTravelGrid thc, int vn) //0=primary, 1 = first windowed version, etc
        {
            discoveryform = ed;
            displaynumber = vn;

            discoveryform.OnHistoryChange += Display;
            discoveryform.OnNewEntry += NewEntry;
            discoveryform.OnNewTarget += NewTarget;

            displayfont = discoveryform.theme.GetFont;

            autoCopyWPToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "autoCopyWP", false);
            autoSetTargetToolStripMenuItem.Checked = SQLiteDBClass.GetSettingBool(DbSave + "autoSetTarget", false);

            String selRoute = SQLiteDBClass.GetSettingString(DbSave + "SelectedRoute", "-1");
            long id = long.Parse(selRoute);
            _currentRoute = SavedRouteClass.GetAllSavedRoutes().Find(r => r.Id.Equals(id));
            updateScreen();
        }

        public override void Closing()
        {
            SQLiteDBClass.PutSettingBool(DbSave + "autoCopyWP", autoCopyWPToolStripMenuItem.Checked);
            SQLiteDBClass.PutSettingBool(DbSave + "autoSetTarget", autoSetTargetToolStripMenuItem.Checked);
            discoveryform.OnHistoryChange -= Display;
            discoveryform.OnNewEntry -= NewEntry;
            discoveryform.OnNewTarget -= NewTarget;
        }

        public override Color ColorTransparency { get { return Color.Green; } }
        public override void SetTransparency(bool on, Color curcol)
        {
            pictureBox.BackColor = this.BackColor = curcol;
            updateScreen();
        }

        public override void Display(HistoryEntry current, HistoryList history)
        {
            Display(history);
        }


        public void Display(HistoryList hl)            // when user clicks around..  HE may be null here
        {
            currentSystem = hl.GetLastFSD;
            updateScreen();
        }

        private void NewEntry(HistoryEntry l, HistoryList hl)
        {
            currentSystem = hl.GetLastFSD;
            updateScreen();
        }

        private void NewTarget(Object sender)
        {
           //updateScreen();
        }


        static class Prompt
        {
            public static Boolean ShowDialog(Form p, 
                List<SavedRouteClass> savedRoutes, 
                String defaultValue,
                string caption,
                out SavedRouteClass selectedRoute)
            {
                Form prompt = new Form()
                {
                    Width = 440,
                    Height = 160,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    Text = caption,
                    StartPosition = FormStartPosition.CenterScreen,
                };

                Label textLabel = new Label() { Left = 10, Top = 20, Width = 400, Text = "Route" };
                Button confirmation = new Button() { Text = "Ok", Left = 245, Width = 80, Top = 90, DialogResult = DialogResult.OK };
                Button cancel = new Button() { Text = "Cancel", Left = 330, Width = 80, Top = 90, DialogResult = DialogResult.Cancel };
                confirmation.Click += (sender, e) => { prompt.Close(); };
                cancel.Click += (sender, e) => { prompt.Close(); };
                ComboBox cb = new ComboBox() { Left = 10, Top = 50, Width = 400 };

                foreach (SavedRouteClass src in savedRoutes)
                {
                    cb.Items.Add(src.Name);
                    if (src.Name.Equals(defaultValue))
                    {
                        cb.SelectedItem = defaultValue;
                    }
                }
                
                prompt.Controls.Add(cb);
                prompt.Controls.Add(confirmation);
                prompt.Controls.Add(cancel);
                prompt.Controls.Add(textLabel);
                prompt.AcceptButton = confirmation;

                var  res = prompt.ShowDialog(p); 
                selectedRoute = (cb.SelectedIndex != -1) ? savedRoutes[cb.SelectedIndex] : null;
                return (res  == DialogResult.OK && selectedRoute != null);
            }
        }

        private void setRouteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavedRouteClass selectedRoute;
            if (Prompt.ShowDialog(discoveryform, SavedRouteClass.GetAllSavedRoutes(),
                _currentRoute!=null? _currentRoute.Name:"", "Select route", out selectedRoute))
            {
                if (selectedRoute == null)
                    return;
                _currentRoute = selectedRoute;
                SQLiteDBClass.PutSettingString(DbSave + "SelectedRoute", selectedRoute.Id.ToString());
                updateScreen();
            }
        }

        void DisplayText(String topline, String bottomLine)
        {
            pictureBox.ClearImageList();
            Color textcolour = IsTransparent ? discoveryform.theme.SPanelColor : discoveryform.theme.LabelColor;
            Color backcolour = IsTransparent ? Color.Transparent : this.BackColor;
            pictureBox.AddTextAutoSize(new Point(10, 5), new Size(1000, 35), topline==null?"":topline, displayfont, textcolour, backcolour, 1.0F);
            pictureBox.AddTextAutoSize(new Point(10, 35), new Size(1000, 35), bottomLine==null?"": bottomLine, displayfont, textcolour, backcolour, 1.0F);
            pictureBox.Render();
        }

        private void updateScreen()
        {
            if (currentSystem == null)
                return;

            if (_currentRoute == null)
            {
                DisplayText("Please set a route, by right clicking", "");
                return;
            }

            if (_currentRoute.Systems.Count == 0)
            {
                DisplayText(_currentRoute.Name, "Route contains no waypoints");
                return;
            }

            string topline = "";
            string bottomLine="";
            string firstSystemName = _currentRoute.Systems[0];

            SystemClassDB firstSystem = SystemClassDB.GetSystem(firstSystemName);
            SystemClassDB finalSystem = SystemClassDB.GetSystem(_currentRoute.Systems[_currentRoute.Systems.Count - 1]);

            if (finalSystem != null)
            {
                string mesg = "remain";
                double distX = SystemClassDB.Distance(currentSystem.System, finalSystem);
                //Small hack to pull the jump range from TripPanel1
                var jumpRange = SQLiteDBClass.GetSettingDouble("TripPanel1" + "JumpRange", -1.0);       //TBD Not a good idea.
                if (jumpRange > 0)
                {
                    int jumps = (int)Math.Ceiling(distX / jumpRange);
                    if (jumps > 0)
                        mesg = "@ " + jumps.ToString() + ((jumps == 1) ? " jump" : " jumps");
                }
                topline = String.Format("{0} {1} WPs {2:N2}ly {3}", _currentRoute.Name, _currentRoute.Systems.Count, distX, mesg);
            }
            else
            {
                topline = String.Format("{0} {1} WPs remain", _currentRoute.Name, _currentRoute.Systems.Count);
            }
            SystemClassDB nearestSystem = null;
            double minDist = double.MaxValue;
            int nearestidx = -1;
            for (int i = 0; i < _currentRoute.Systems.Count; i++)
            {
                String sys = _currentRoute.Systems[i];
                SystemClassDB sc = SystemClassDB.GetSystem(sys);
                if (sc == null)
                    continue;
                double dist = SystemClassDB.Distance(currentSystem.System, sc);
                if (dist <= minDist)
                {
                    if (nearestSystem == null || !nearestSystem.name.Equals(sc.name))
                    {
                        minDist = dist;
                        nearestidx = i;
                        nearestSystem = sc;
                    }
                }
            }


            string name = null;
            if (nearestSystem != null && firstSystem != null)
            {
                double first2Neasest = SystemClassDB.Distance(firstSystem, nearestSystem);
                double first2Me = SystemClassDB.Distance(firstSystem, currentSystem.System);

                string nextName = null;
                int wp = nearestidx + 1;
                if (nearestidx < _currentRoute.Systems.Count - 1)
                    nextName = _currentRoute.Systems[nearestidx + 1];
                if (first2Me >= first2Neasest && !String.IsNullOrWhiteSpace(nextName))
                {
                    name = nextName;
                    wp++;
                }
                else
                    name = nearestSystem.name;

                SystemClassDB nextSystem = SystemClassDB.GetSystem(name);
                if (nextSystem == null)
                {
                    bottomLine = String.Format("WP{0}: {1} {2}", wp, nextName, autoCopyWPToolStripMenuItem.Checked ? " (AUTO)" : "");
                }
                else
                {
                    double distance = SystemClassDB.Distance(currentSystem.System, nextSystem);
                    bottomLine = String.Format("{0:N2}ly to WP{1}: {2} {3}", distance, wp, name, autoCopyWPToolStripMenuItem.Checked ? " (AUTO)" : "");
                }

            }else 
            {
                bottomLine = String.Format("WP{0}: {1} {2}", 1, firstSystemName, autoCopyWPToolStripMenuItem.Checked ? " (AUTO)" : "");
                name = firstSystemName;
            }
            if (name!=null && name.CompareTo(lastsystem) != 0)
            {
                if (autoCopyWPToolStripMenuItem.Checked)
                    Clipboard.SetText(name);
                if (autoSetTargetToolStripMenuItem.Checked)
                {
                    string targetName;
                    double x, y, z;
                    TargetClass.GetTargetPosition(out targetName, out x, out y, out z);
                    if (name.CompareTo(targetName) != 0)
                        TargetHelpers.setTargetSystem(this,discoveryform, name, false);
                }
            }
            lastsystem = name;
            DisplayText(topline, bottomLine);
        }

        private void autoCopyWPToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            updateScreen();
        }
    }
}
