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
using EliteDangerousCore;
using EliteDangerousCore.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    public class EventFilterSelector
    {
        public event EventHandler Changed;

        private ExtendedControls.CheckedIconListBoxForm cc;
        private string dbstring;
        class ExtraOption
        {
            public string Name;
            public string Events;
            public Image Image;
        }

        private List<ExtraOption> extraoptions = new List<ExtraOption>();
        private int ReservedEntries { get { return 2 + extraoptions.Count(); } }

        public void AddExtraOption(string n, string ev, Image img)      // could be extended later for more..
        {
            extraoptions.Add(new ExtraOption() { Name = n, Events = ev, Image = img });
        }

        public void AddStandardExtraOptions()
        {
            // must be in alpha order..
            AddExtraOption("Travel".Tx(), "Docked;FSD Jump;Undocked;", JournalEntry.JournalTypeIcons[JournalTypeEnum.FSDJump]);
            AddExtraOption("Missions".Tx(), "Mission Abandoned;Mission Accepted;Mission Completed;Mission Failed;Mission Redirected;", JournalEntry.JournalTypeIcons[JournalTypeEnum.Missions]);
        }

        public void JournalEvents(string db, Control ctr, Form parent, string[] methods = null)
        {
            var x = JournalEntry.GetListOfEventsWithOptMethodSortedImage(true, methods);
            FilterButton(db, ctr, parent, x.Select(y=>y.Item1).ToList(), x.Select(y=>y.Item2).ToList());
        }

        public void FilterButton(string db, Control ctr, Form parent, List<string> list, List<Image> images = null )
        {
            FilterButtonInt(db, ctr.PointToScreen(new Point(0, ctr.Size.Height)), new Size(ctr.Width * 3,600), parent, list , images);
        }

        public void FilterButtonJournal(string db, Point p, Size s, Form parent)
        {
            List<string> events = JournalEntry.GetListOfEventsWithOptMethod(towords: true);
            events.Sort();
            FilterButtonInt(db, p, s, parent, events);
        }

        private void FilterButtonInt(string db, Point p, Size s, Form parent, List<string> list, List<Image> images = null)
        {
            if (cc == null)
            {
                dbstring = db;
                cc = new ExtendedControls.CheckedIconListBoxForm();

                cc.AddItem("All".Tx());       // displayed, translate
                cc.AddImageItem(EDDiscovery.Icons.Controls.All);

                cc.AddItem("None".Tx());
                cc.AddImageItem(EDDiscovery.Icons.Controls.None);

                foreach (var x in extraoptions)
                {
                    cc.AddItem(x.Name);
                    cc.AddImageItem(x.Image);
                }

                cc.AddItems(list.ToArray());
                if ( images != null )
                    cc.AddImageItems(images);

                cc.SetChecked(SQLiteDBClass.GetSettingString(dbstring, "All"));
                SetFilterSet();

                cc.FormClosed += FilterClosed;
                cc.CheckedChanged += CheckChanged;
                cc.PositionSize(p,s);
                cc.LargeChange = list.Count * EDDiscovery.Icons.Controls.All.Height / 40;   // 40 ish scroll movements

                EDDiscovery.EDDTheme.Instance.ApplyToControls(cc, applytothis:true);

                cc.Show(parent);
            }
            else
                cc.Close();
        }

        private void SetFilterSet()
        {
            string list = cc.GetChecked(ReservedEntries);       // using All or None.. on items beyond reserved entries
            System.Diagnostics.Debug.WriteLine("Checked" + list);
            cc.SetChecked(list.Equals("All"), 0, 1);
            cc.SetChecked(list.Equals("None"), 1, 1);

            int p = 2;
            foreach (var eo in extraoptions)
            {
                System.Diagnostics.Debug.WriteLine("Filter check for " + eo.Events);
                cc.SetChecked(list.Equals(eo.Events), p++, 1);
            }
        }

        private void CheckChanged(Object sender, ItemCheckEventArgs e)          // called after check changed for the new system
        {
            if (e.NewValue == CheckState.Checked)       // all or none set all of them
            { 
                if (e.Index <= 1)
                {
                    cc.SetChecked(e.Index == 0, ReservedEntries);
                }
                else if ( e.Index < ReservedEntries)
                {
                    cc.SetChecked(false, ReservedEntries);
                    cc.SetChecked(extraoptions[e.Index - 2].Events);
                }
            }

            SetFilterSet();
        }

        private void FilterClosed(Object sender, FormClosedEventArgs e)
        {
            SQLiteDBClass.PutSettingString(dbstring, cc.GetChecked(ReservedEntries));
            cc = null;
            Changed?.Invoke(sender, e);
        }
    }

}