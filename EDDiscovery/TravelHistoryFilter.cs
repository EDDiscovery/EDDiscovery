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
                return hl.OrderByDate;
            }
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

        public void FilterButton(string db , Control ctr, Color back, Color fore)
        {
            if (cc == null)
            {
                dbstring = db;
                cc = new ExtendedControls.CheckedListControlCustom();
                cc.Items.Add("All");
                cc.Items.Add("None");
                cc.Items.Add("Travel");

                foreach (JournalTypeEnum jte in Enum.GetValues(typeof(JournalTypeEnum)))
                {
                    cc.Items.Add(Tools.SplitCapsWord(jte.ToString()));
                }

                cc.SetChecked(SQLiteDBClass.GetSettingString(dbstring, "All"));
                SetFilterSet();

                cc.FormClosed += FilterClosed;
                cc.CheckedChanged += FilterCheckChanged;
                cc.PositionBelow(ctr, new Size(ctr.Width*2, 400));
                cc.SetColour(back,fore);
                cc.Show();
            }
            else
                cc.Close();
        }

        private void SetFilterSet()
        {
            string list = cc.GetChecked(3);
            //Console.WriteLine("List {0}", list);
            cc.SetChecked(list.Equals("All"), 0, 1);
            cc.SetChecked(list.Equals("None"), 1, 1);
            cc.SetChecked(list.Equals("Docked;FSD Jump;Undocked;"), 2, 1);
        }

        private void FilterCheckChanged(Object sender, ItemCheckEventArgs e)
        {
            //Console.WriteLine("Changed " + e.Index);

            cc.SetChecked(e.NewValue == CheckState.Checked, e.Index, 1);        // force check now (its done after it) so our functions work..

            if (e.Index == 0 && e.NewValue == CheckState.Checked)
                cc.SetChecked(true, 3);

            if ((e.Index == 1 && e.NewValue == CheckState.Checked) || (e.Index <= 2 && e.NewValue == CheckState.Unchecked))
                cc.SetChecked(false, 3);

            if (e.Index == 2 && e.NewValue == CheckState.Checked)
            {
                cc.SetChecked(false, 3);
                cc.SetChecked("Docked;FSD Jump;Undocked;");
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

            DataGridViewRow[] theRows = new DataGridViewRow[vw.Rows.Count];
            vw.Rows.CopyTo(theRows, 0);
            vw.Rows.Clear();

            for (int loop = 0; loop < theRows.Length; loop++)
            {
                bool found = false;

                if (searchstr.Length < 1)
                    found = true;
                else
                {
                    foreach (DataGridViewCell cell in theRows[loop].Cells)
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

                theRows[loop].Visible = found;
            }

            vw.Rows.AddRange(theRows);
            vw.ResumeLayout();
        }
    }

}