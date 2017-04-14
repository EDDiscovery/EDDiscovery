/*
 * Copyright © 2016 EDDiscovery development team
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
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public class EngineerFilterSelector
    {
        ExtendedControls.CheckedListControlCustom cc;
        string dbstring;
        public event EventHandler Changed;

        private int reserved = 1;

        private List<string> engineers = new List<string> { "Bill Turner", "Broo Tarquin", "Colonel Bris Dekker", "Didi Vatermann", "Elvira Martuk", "Felicity Farseer", "Hera Tani", "Juri Ishmaak", "Lei Cheung", "Liz Ryder",
                                                            "Lori Jameson", "Marco Qwent", "Professor Palin", "Ram Tah", "Selene Jean", "The Dweller", "The Sarge", "Tiana Fortune", "Tod \"The Blaster\" McQuinn", "Zacariah Nemo" };

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent)
        {
            FilterButton(db, ctr, back, fore, parent, engineers);
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent, List<string> list)
        {
            FilterButton(db, ctr.PointToScreen(new Point(0, ctr.Size.Height)), new Size(ctr.Width * 2, 400), back, fore, parent, list);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent)
        {
            FilterButton(db, p, s, back, fore, parent, engineers);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent, List<string> list)
        {
            if (cc == null)
            {
                dbstring = db;
                cc = new ExtendedControls.CheckedListControlCustom();
                cc.Items.Add("All");
                cc.Items.Add("None");

                cc.Items.AddRange(list.ToArray());

                cc.SetChecked(SQLiteDBClass.GetSettingString(dbstring, "All"));
                SetFilterSet();

                cc.FormClosed += FilterClosed;
                cc.CheckedChanged += FilterCheckChanged;
                cc.PositionSize(p, s);
                cc.SetColour(back, fore);
                cc.Show(parent);
            }
            else
                cc.Close();
        }

        private void SetFilterSet()
        {
            string list = cc.GetChecked(reserved);
            cc.SetChecked(list.Equals("All"), 0, 1);
            cc.SetChecked(list.Equals("None"), 1, 1);

        }

        private void FilterCheckChanged(Object sender, ItemCheckEventArgs e)
        {
            //Console.WriteLine("Changed " + e.Index);

            cc.SetChecked(e.NewValue == CheckState.Checked, e.Index, 1);        // force check now (its done after it) so our functions work..

            if (e.Index == 0 && e.NewValue == CheckState.Checked)
                cc.SetChecked(true, reserved);

            if (e.Index == 1 && e.NewValue == CheckState.Checked)
                cc.SetChecked(false, reserved);

            SetFilterSet();
        }

        private void FilterClosed(Object sender, FormClosedEventArgs e)
        {
            SQLiteDBClass.PutSettingString(dbstring, cc.GetChecked(2));
            cc = null;

            if (Changed != null)
                Changed(sender, e);
        }
    }

    public class ModuleFilterSelector
    {
        ExtendedControls.CheckedListControlCustom cc;
        string dbstring;
        public event EventHandler Changed;

        private int reserved = 1;

        private List<string> modules = new List<string> { "Armour", "Auto Field-Maintenance Unit", "Beam Laser", "Burst Laser", "Cannon", "Chaff Launcher", "Collector Limpet Controller", "Electronic Countermeasure", "Fragment Cannon",
                                                          "Frame Shift Drive", "Frame Shift Drive Interdictor", "Fuel Scoop", "Fuel Transfer Limpit Controller", "Hatch Breaker Limpit Controller", "Heat Sink Launcher", "Hull Reinforcement Package",
                                                          "Kill Warrant Scanner", "Life Support", "Manifest Scanner", "Mine Launcher", "Missile Rack", "Multi-cannon", "Plasma Accelerator", "Point Defence", "Power Distributor", "Power Plant",
                                                          "Prospector Limpit Controller", "Pulse Laser", "Rail Gun", "Shield Booster", "Shield Cell Bank", "Shield Generator", "Thrusters", "Torpedo Pylon" };

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent)
        {
            FilterButton(db, ctr, back, fore, parent, modules);
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent, List<string> list)
        {
            FilterButton(db, ctr.PointToScreen(new Point(0, ctr.Size.Height)), new Size(ctr.Width * 2, 400), back, fore, parent, list);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent)
        {
            FilterButton(db, p, s, back, fore, parent, modules);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent, List<string> list)
        {
            if (cc == null)
            {
                dbstring = db;
                cc = new ExtendedControls.CheckedListControlCustom();
                cc.Items.Add("All");
                cc.Items.Add("None");

                cc.Items.AddRange(list.ToArray());

                cc.SetChecked(SQLiteDBClass.GetSettingString(dbstring, "All"));
                SetFilterSet();

                cc.FormClosed += FilterClosed;
                cc.CheckedChanged += FilterCheckChanged;
                cc.PositionSize(p, s);
                cc.SetColour(back, fore);
                cc.Show(parent);
            }
            else
                cc.Close();
        }

        private void SetFilterSet()
        {
            string list = cc.GetChecked(reserved);
            cc.SetChecked(list.Equals("All"), 0, 1);
            cc.SetChecked(list.Equals("None"), 1, 1);

        }

        private void FilterCheckChanged(Object sender, ItemCheckEventArgs e)
        {
            //Console.WriteLine("Changed " + e.Index);

            cc.SetChecked(e.NewValue == CheckState.Checked, e.Index, 1);        // force check now (its done after it) so our functions work..

            if (e.Index == 0 && e.NewValue == CheckState.Checked)
                cc.SetChecked(true, reserved);

            if (e.Index == 1 && e.NewValue == CheckState.Checked)
                cc.SetChecked(false, reserved);

            SetFilterSet();
        }

        private void FilterClosed(Object sender, FormClosedEventArgs e)
        {
            SQLiteDBClass.PutSettingString(dbstring, cc.GetChecked(2));
            cc = null;

            if (Changed != null)
                Changed(sender, e);
        }
    }

    public class LevelFilterSelector
    {
        ExtendedControls.CheckedListControlCustom cc;
        string dbstring;
        public event EventHandler Changed;

        private List<string> levels = new List<string> { "1", "2", "3", "4", "5" };
        private int reserved = 1;

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent)
        {
            FilterButton(db, ctr, back, fore, parent, levels);
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent, List<string> list)
        {
            FilterButton(db, ctr.PointToScreen(new Point(0, ctr.Size.Height)), new Size(ctr.Width * 2, 400), back, fore, parent, list);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent)
        {
            FilterButton(db, p, s, back, fore, parent, levels);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent, List<string> list)
        {
            if (cc == null)
            {
                dbstring = db;
                cc = new ExtendedControls.CheckedListControlCustom();
                cc.Items.Add("All");
                cc.Items.Add("None");

                cc.Items.AddRange(list.ToArray());

                cc.SetChecked(SQLiteDBClass.GetSettingString(dbstring, "All"));
                SetFilterSet();

                cc.FormClosed += FilterClosed;
                cc.CheckedChanged += FilterCheckChanged;
                cc.PositionSize(p, s);
                cc.SetColour(back, fore);
                cc.Show(parent);
            }
            else
                cc.Close();
        }

        private void SetFilterSet()
        {
            string list = cc.GetChecked(reserved);
            cc.SetChecked(list.Equals("All"), 0, 1);
            cc.SetChecked(list.Equals("None"), 1, 1);
        }

        private void FilterCheckChanged(Object sender, ItemCheckEventArgs e)
        {
            //Console.WriteLine("Changed " + e.Index);

            cc.SetChecked(e.NewValue == CheckState.Checked, e.Index, 1);        // force check now (its done after it) so our functions work..

            if (e.Index == 0 && e.NewValue == CheckState.Checked)
                cc.SetChecked(true, reserved);

            if (e.Index == 1 && e.NewValue == CheckState.Checked)
                cc.SetChecked(false, reserved);
            
            SetFilterSet();
        }

        private void FilterClosed(Object sender, FormClosedEventArgs e)
        {
            SQLiteDBClass.PutSettingString(dbstring, cc.GetChecked(2));
            cc = null;

            if (Changed != null)
                Changed(sender, e);
        }
    }
}
