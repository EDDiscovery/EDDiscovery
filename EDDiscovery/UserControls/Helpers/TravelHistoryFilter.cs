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
    public class TravelHistoryFilter
    {
        public TimeSpan? MaximumDataAge { get; }
        public int? MaximumNumberOfItems { get; }
        public bool Lastdockflag { get; }
        public bool Startendflag { get; }
        public string Label { get; }

        public static TravelHistoryFilter NoFilter { get; } = new TravelHistoryFilter();

        private TravelHistoryFilter(TimeSpan maximumDataAge, string label)
        {
            MaximumDataAge = maximumDataAge;
            Label = label;
        }

        private TravelHistoryFilter(int maximumNumberOfItems, string label)
        {
            MaximumNumberOfItems = maximumNumberOfItems;
            Label = label;
        }

        private TravelHistoryFilter(bool ld, bool startend, string label)
        {
            Lastdockflag = ld;
            Startendflag = startend;
            Label = label;
        }

        private TravelHistoryFilter()
        {
            Label = "All".Tx();
        }

        public static TravelHistoryFilter FromHours(int hours)
        {
            return new TravelHistoryFilter(TimeSpan.FromHours(hours), string.Format("{0} hours".Tx(typeof(TravelHistoryFilter), "Hours"),hours));
        }

        public static TravelHistoryFilter FromDays(int days)
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(days), string.Format("{0} days".Tx(typeof(TravelHistoryFilter), "Days"),days));
        }

        public static TravelHistoryFilter FromWeeks(int weeks)
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(7 * weeks), weeks == 1 ? "One Week".Tx(typeof(TravelHistoryFilter), "1Week") : string.Format( "{0} weeks".Tx(typeof(TravelHistoryFilter), "Weeks"), weeks));
        }

        public static TravelHistoryFilter LastMonth()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(30), "Month".Tx(typeof(TravelHistoryFilter), "Month"));
        }

        public static TravelHistoryFilter LastQuarter()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(90), "Quarter".Tx(typeof(TravelHistoryFilter), "Quarter"));
        }

        public static TravelHistoryFilter LastHalfYear()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(180), "Half year".Tx(typeof(TravelHistoryFilter), "HYear"));
        }

        public static TravelHistoryFilter LastYear()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(365), "Year".Tx(typeof(TravelHistoryFilter), "Year"));
        }

        public static TravelHistoryFilter Last(int number)
        {
            return new TravelHistoryFilter(number, string.Format("Last {0} entries".Tx(typeof(TravelHistoryFilter), "LastN"), number));
        }

        public static TravelHistoryFilter LastDock()
        {
            return new TravelHistoryFilter(true, false, $"Last dock".Tx(typeof(TravelHistoryFilter), "LDock"));
        }

        public static TravelHistoryFilter StartEnd()
        {
            return new TravelHistoryFilter(false, true, $"Start/End Flag".Tx(typeof(TravelHistoryFilter), "StartEnd"));
        }

        public List<HistoryEntry> Filter(HistoryList hl )
        {
            if (Lastdockflag)
            {
                return hl.FilterToLastDock();
            }
            else if (Startendflag)
            {
                return hl.FilterStartEnd();
            }
            else if (MaximumNumberOfItems.HasValue)
            {
                return hl.FilterByNumber(MaximumNumberOfItems.Value);
            }
            else if (MaximumDataAge.HasValue)
            {
                return hl.FilterByDate(MaximumDataAge.Value);
            }
            else
            {
                return hl.LastFirst;
            }
        }

        public List<Ledger.Transaction> Filter(List<Ledger.Transaction> txlist )
        {                                                               // LASTDOCK not supported
            if (MaximumNumberOfItems.HasValue)
            {
                return txlist.OrderByDescending(s => s.utctime).Take(MaximumNumberOfItems.Value).ToList();
            }
            else if (MaximumDataAge.HasValue)
            {
                var oldestData = DateTime.UtcNow.Subtract(MaximumDataAge.Value);
                return (from tx in txlist where tx.utctime >= oldestData orderby tx.utctime descending select tx).ToList();
            }
            else
                return txlist;
        }

        public static void InitaliseComboBox( ExtendedControls.ComboBoxCustom cc , string dbname , bool incldockstartend = true )
        {
            cc.Enabled = false;
            cc.DisplayMember = nameof(TravelHistoryFilter.Label);

            List<TravelHistoryFilter> el = new List<TravelHistoryFilter>()
            {
                TravelHistoryFilter.NoFilter,
                TravelHistoryFilter.FromHours(6),
                TravelHistoryFilter.FromHours(12),
                TravelHistoryFilter.FromHours(24),
                TravelHistoryFilter.FromDays(3),
                TravelHistoryFilter.FromWeeks(1),
                TravelHistoryFilter.FromWeeks(2),
                TravelHistoryFilter.LastMonth(),
                TravelHistoryFilter.LastQuarter(),
                TravelHistoryFilter.LastHalfYear(),
                TravelHistoryFilter.LastYear(),
                TravelHistoryFilter.Last(10),
                TravelHistoryFilter.Last(20),
                TravelHistoryFilter.Last(100),
                TravelHistoryFilter.Last(500),
            };

            if (incldockstartend)
            {
                el.Add(TravelHistoryFilter.LastDock());
                el.Add(TravelHistoryFilter.StartEnd());
            }

            cc.DataSource = el;

            string last = SQLiteDBClass.GetSettingString(dbname, "");
            int entry = el.FindIndex(x => x.Label == last);
            //System.Diagnostics.Debug.WriteLine(dbname + "=" + last + "=" + entry);
            cc.SelectedIndex = (entry >=0) ? entry: 0;
            
            cc.Enabled = true;
        }
    }

    public class EventFilterSelector
    {
        ExtendedControls.CheckedListControlCustom cc;
        string dbstring;
        public event EventHandler Changed;

        private List<Tuple<string, string>> extraoptions = new List<Tuple<string, string>>();
        private int ReservedEntries { get { return 2 + extraoptions.Count(); } }

        public void AddExtraOption(string n , string events )      // could be extended later for more..
        {
            extraoptions.Add(new Tuple<string,string>(n,events));
        }

        public void AddStandardExtraOptions()
        {
            AddExtraOption("Travel".Tx(), "Docked;FSD Jump;Undocked;");
            AddExtraOption("Missions".Tx(), "Mission Accepted;Mission Completed;Mission Abandoned;Mission Redirected;Mission Failed;");
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Font fnt, Form parent)
        {
            List<string> events = JournalEntry.GetListOfEventsWithOptMethod(towords: true);
            events.Sort();
            FilterButton(db, ctr, back, fore, fnt, parent, events);
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Font fnt, Form parent, List<string> list)
        {
            FilterButton(db, ctr.PointToScreen(new Point(0, ctr.Size.Height)), new Size(ctr.Width * 3,600), back, fore, fnt, parent, list);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Font fnt, Form parent)
        {
            List<string> events = JournalEntry.GetListOfEventsWithOptMethod(towords: true);
            events.Sort();
            FilterButton(db, p, s, back, fore, fnt, parent, events);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Font fnt, Form parent, List<string> list)
        {
            if (cc == null)
            {
                dbstring = db;
                cc = new ExtendedControls.CheckedListControlCustom();
                cc.Items.Add("All".Tx());       // displayed, translate
                cc.Items.Add("None".Tx());

                cc.Items.AddRange(extraoptions.Select((x)=>x.Item1).ToArray());

                cc.Items.AddRange(list.ToArray());

                cc.SetChecked(SQLiteDBClass.GetSettingString(dbstring, "All"));
                SetFilterSet();

                cc.FormClosed += FilterClosed;
                cc.CheckedChanged += FilterCheckChanged;
                cc.PositionSize(p,s);
                cc.SetColour(back,fore);
                cc.SetFont(fnt);
                cc.Show(parent);
            }
            else
                cc.Close();
        }

        private void SetFilterSet()
        {
            string list = cc.GetChecked(ReservedEntries);
            //Console.WriteLine("List {0}", list);
            cc.SetChecked(list.Equals("All"), 0, 1);
            cc.SetChecked(list.Equals("None"), 1, 1);

            int p = 2;
            foreach(var eo in extraoptions)
                cc.SetChecked(list.Equals(eo.Item2), p++, 1);
        }

        private void FilterCheckChanged(Object sender, ItemCheckEventArgs e)
        {
            //Console.WriteLine("Changed " + e.Index);

            cc.SetChecked(e.NewValue == CheckState.Checked, e.Index, 1);        // force check now (its done after it) so our functions work..

            if (e.Index == 0 && e.NewValue == CheckState.Checked)
                cc.SetChecked(true, ReservedEntries);

            if ((e.Index == 1 && e.NewValue == CheckState.Checked) || (e.Index <= 2 && e.NewValue == CheckState.Unchecked))
                cc.SetChecked(false, ReservedEntries);

            if (e.Index >= 2 && e.Index < 2 + extraoptions.Count() && e.NewValue == CheckState.Checked)
            {
                cc.SetChecked(false, e.Index );
                cc.SetChecked(extraoptions[e.Index-2].Item2);
            }

            SetFilterSet();
        }

        private void FilterClosed(Object sender, FormClosedEventArgs e)
        {
            SQLiteDBClass.PutSettingString(dbstring, cc.GetChecked(ReservedEntries));
            cc = null;

            if (Changed != null)
                Changed(sender, e);
        }
    }

}