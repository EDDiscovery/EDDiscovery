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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using EDDiscovery.DB;
using EDDiscovery.EliteDangerous;
using EDDiscovery2.DB;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace EDDiscovery
{
    public class TravelHistoryFilter
    {
        public TimeSpan? MaximumDataAge { get; }
        public int? MaximumNumberOfItems { get; }
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

        private TravelHistoryFilter()
        {
            Label = "All";
        }

        public static TravelHistoryFilter FromHours(int hours)
        {
            return new TravelHistoryFilter(TimeSpan.FromHours(hours), $"{hours} hours");
        }

        public static TravelHistoryFilter FromDays(int days)
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(days), $"{days} days");
        }

        public static TravelHistoryFilter FromWeeks(int weeks)
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(7 * weeks), weeks == 1 ? "week" : $"{weeks} weeks");
        }

        public static TravelHistoryFilter LastMonth()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(30), "month");
        }

        public static TravelHistoryFilter Last(int number)
        {
            return new TravelHistoryFilter(number, $"last {number}");
        }

        public List<HistoryEntry> Filter(HistoryList hl )
        {
            if (MaximumNumberOfItems.HasValue)
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

        public List<MaterialCommoditiesLedger.Transaction> Filter(List<MaterialCommoditiesLedger.Transaction> txlist )
        {
            if (MaximumDataAge.HasValue)
            {
                var oldestData = DateTime.UtcNow.Subtract(MaximumDataAge.Value);
                return (from tx in txlist where tx.utctime >= oldestData orderby tx.utctime descending select tx).ToList();
            }
            else
                return txlist;
        }

        public const int DefaultTravelHistoryFilterIndex = 8;

        public static void InitaliseComboBox( ExtendedControls.ComboBoxCustom cc , string dbname )
        {
            cc.Enabled = false;
            cc.DataSource = new[]
            {
                TravelHistoryFilter.FromHours(6),
                TravelHistoryFilter.FromHours(12),
                TravelHistoryFilter.FromHours(24),
                TravelHistoryFilter.FromDays(3),
                TravelHistoryFilter.FromWeeks(1),
                TravelHistoryFilter.FromWeeks(2),
                TravelHistoryFilter.LastMonth(),
                TravelHistoryFilter.Last(20),
                TravelHistoryFilter.NoFilter,
            };

            cc.DisplayMember = nameof(TravelHistoryFilter.Label);
            cc.SelectedIndex = SQLiteDBClass.GetSettingInt(dbname, DefaultTravelHistoryFilterIndex);
            cc.Enabled = true;
        }
    }

    public class EventFilterSelector
    {
        ExtendedControls.CheckedListControlCustom cc;
        string dbstring;
        public event EventHandler Changed;

        private string selectedlist;
        private string selectedlistname;
        private int reserved = 2;

        public void ConfigureThirdOption(string n , string l )      // could be extended later for more..
        {
            selectedlistname = n;
            selectedlist = l;
            reserved = 3;
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent)
        {
            FilterButton(db, ctr, back, fore, parent, JournalEntry.GetListOfEventsWithOptMethod(true));
        }

        public void FilterButton(string db, Control ctr, Color back, Color fore, Form parent, List<string> list)
        {
            FilterButton(db, ctr.PointToScreen(new Point(0, ctr.Size.Height)), new Size(ctr.Width * 2, 400), back, fore, parent, list);
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent)
        {
            FilterButton(db, p, s, back, fore, parent, JournalEntry.GetListOfEventsWithOptMethod(true));
        }

        public void FilterButton(string db, Point p, Size s, Color back, Color fore, Form parent, List<string> list)
        {
            if (cc == null)
            {
                dbstring = db;
                cc = new ExtendedControls.CheckedListControlCustom();
                cc.Items.Add("All");
                cc.Items.Add("None");

                if (selectedlist != null)
                    cc.Items.Add(selectedlistname);

                cc.Items.AddRange(list.ToArray());

                cc.SetChecked(SQLiteDBClass.GetSettingString(dbstring, "All"));
                SetFilterSet();

                cc.FormClosed += FilterClosed;
                cc.CheckedChanged += FilterCheckChanged;
                cc.PositionSize(p,s);
                cc.SetColour(back,fore);
                cc.Show(parent);
            }
            else
                cc.Close();
        }

        private void SetFilterSet()
        {
            string list = cc.GetChecked(reserved);
            //Console.WriteLine("List {0}", list);
            cc.SetChecked(list.Equals("All"), 0, 1);
            cc.SetChecked(list.Equals("None"), 1, 1);

            if ( selectedlist!=null)
                cc.SetChecked(list.Equals(selectedlist), 2, 1);
        }

        private void FilterCheckChanged(Object sender, ItemCheckEventArgs e)
        {
            //Console.WriteLine("Changed " + e.Index);

            cc.SetChecked(e.NewValue == CheckState.Checked, e.Index, 1);        // force check now (its done after it) so our functions work..

            if (e.Index == 0 && e.NewValue == CheckState.Checked)
                cc.SetChecked(true, reserved);

            if ((e.Index == 1 && e.NewValue == CheckState.Checked) || (e.Index <= 2 && e.NewValue == CheckState.Unchecked))
                cc.SetChecked(false, reserved);

            if (selectedlist != null && e.Index == 2 && e.NewValue == CheckState.Checked)
            {
                cc.SetChecked(false, reserved);
                cc.SetChecked(selectedlist);
            }

            SetFilterSet();
        }

        private void FilterClosed(Object sender, FormClosedEventArgs e)
        {
            SQLiteDBClass.PutSettingString(dbstring, cc.GetChecked(3));
            cc = null;

            if (Changed != null)
                Changed(sender, e);
        }
    }

    public class StaticFilters
    {
        public static void FilterGridView(DataGridView vw, string searchstr)
        {
            vw.SuspendLayout();
            vw.Enabled = false;

            bool[] visible = new bool[vw.RowCount];
            bool visibleChanged = false;

            foreach (DataGridViewRow row in vw.Rows.OfType<DataGridViewRow>())
            {
                bool found = false;

                if (searchstr.Length < 1)
                    found = true;
                else
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Value != null)
                        {
                            if (cell.Value.ToString().IndexOf(searchstr, 0, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            {
                                found = true;
                                break;
                            }
                        }
                    }
                }

                visible[row.Index] = found;
                visibleChanged |= found != row.Visible;
            }

            if (visibleChanged)
            {
                var selectedrow = vw.SelectedRows.OfType<DataGridViewRow>().Select(r => r.Index).FirstOrDefault();
                DataGridViewRow[] rows = vw.Rows.OfType<DataGridViewRow>().ToArray();
                vw.Rows.Clear();

                for (int i = 0; i < rows.Length; i++)
                {
                    rows[i].Visible = visible[i];
                }

                vw.Rows.Clear();
                vw.Rows.AddRange(rows.ToArray());

                vw.Rows[selectedrow].Selected = true;
            }

            vw.Enabled = true;
            vw.ResumeLayout();
        }
    }

}