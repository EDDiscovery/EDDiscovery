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
        public DateTime? StartDateUTC { get; }
        public DateTime? EndDateUTC { get; }

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

        private TravelHistoryFilter(DateTime startutc, DateTime endutc)
        {
            StartDateUTC = startutc;
            EndDateUTC = endutc;
            Label = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(StartDateUTC.Value).ToString() + ".." + EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EndDateUTC.Value).ToString();
        }

        private TravelHistoryFilter()
        {
            Label = "All".T(EDTx.All);
        }

        public static TravelHistoryFilter FromHours(int hours)
        {
            return new TravelHistoryFilter(TimeSpan.FromHours(hours), string.Format("{0} hours".T(EDTx.TravelHistoryFilter_Hours),hours));
        }

        public static TravelHistoryFilter FromDays(int days)
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(days), string.Format("{0} days".T(EDTx.TravelHistoryFilter_Days),days));
        }

        public static TravelHistoryFilter FromWeeks(int weeks)
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(7 * weeks), weeks == 1 ? "One Week".T(EDTx.TravelHistoryFilter_1Week) : string.Format( "{0} weeks".T(EDTx.TravelHistoryFilter_Weeks), weeks));
        }

        public static TravelHistoryFilter LastMonth()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(30), "Month".T(EDTx.TravelHistoryFilter_Month));
        }

        public static TravelHistoryFilter LastQuarter()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(90), "Quarter".T(EDTx.TravelHistoryFilter_Quarter));
        }

        public static TravelHistoryFilter LastHalfYear()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(180), "Half year".T(EDTx.TravelHistoryFilter_HYear));
        }

        public static TravelHistoryFilter LastYear()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(365), "Year".T(EDTx.TravelHistoryFilter_Year));
        }

        public static TravelHistoryFilter LastTwoYears()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(365*2), "2 " + "Year".T(EDTx.TravelHistoryFilter_Year));
        }
        public static TravelHistoryFilter LastThreeYears()
        {
            return new TravelHistoryFilter(TimeSpan.FromDays(365*3), "3 " + "Year".T(EDTx.TravelHistoryFilter_Year));
        }

        public static TravelHistoryFilter Last(int number)
        {
            return new TravelHistoryFilter(number, string.Format("Last {0} entries".T(EDTx.TravelHistoryFilter_LastN), number));
        }

        public static TravelHistoryFilter LastDock()
        {
            return new TravelHistoryFilter(true, false, $"Last dock".T(EDTx.TravelHistoryFilter_LDock));
        }

        public static TravelHistoryFilter StartEnd()
        {
            return new TravelHistoryFilter(false, true, $"Start/End Flag".T(EDTx.TravelHistoryFilter_StartEnd));
        }
        public static TravelHistoryFilter StartEnd(DateTime start, DateTime end)
        {
            return new TravelHistoryFilter(start, end);
        }

        public List<HistoryEntry> Filter(List<HistoryEntry> list, HashSet<JournalTypeEnum> entries = null, bool reverse = true)      // list should be in entry order. oldest first
        {
            if (Lastdockflag)
            {
                return HistoryList.ToLastDock(list, entries, reverse);
            }
            else if (Startendflag)
            {
                return HistoryList.StartStopFlags(list, entries, reverse);
            }
            else if (MaximumNumberOfItems.HasValue)
            {
                System.Diagnostics.Debug.Assert(entries == null && reverse == true);        // don't support this at present
                return HistoryList.LatestFirstLimitNumber(list, MaximumNumberOfItems.Value);
            }
            else if (MaximumDataAge.HasValue)
            {
                DateTime lastentry = list.Count > 0 ? list.Last().EventTimeUTC : DateTime.UtcNow;       // if a list, last date in list, else UTC now
                return HistoryList.LimitByDate(list, lastentry, MaximumDataAge.Value, entries, reverse);
            }
            else if (StartDateUTC.HasValue)
            {
                return HistoryList.LimitByDate(list, StartDateUTC.Value, EndDateUTC.Value, entries, reverse);
            }
            else
            {
                if (entries != null)
                {
                    return reverse ? HistoryList.LatestFirst(list, entries) : HistoryList.FilterByEventEntryOrder(list, entries);
                }
                else
                    return reverse ? HistoryList.LatestFirst(list) : list;
            }

        }

        // list should be in latest first order, supports a limited set
        public List<HistoryEntry> FilterLatestFirst(List<HistoryEntry> list)      
        {
            if (MaximumNumberOfItems.HasValue)
            {
                return list.GetRange(0, Math.Min(MaximumNumberOfItems.Value, list.Count));
            }
            else if (MaximumDataAge.HasValue)
            {
                var oldestData = DateTime.UtcNow.Subtract(MaximumDataAge.Value);
                int index = list.FindIndex(x => x.EventTimeUTC < oldestData);       // find first entry with date younger than oldest data
                return index >= 0 ? list.GetRange(0, index) : list;                 // if not found one, they are all younger than the time, so its all, else its a range
            }
            else
            {
                return list;
            }
        }

        // list in is in ascending order, return in ascending date order
        public List<Ledger.Transaction> Filter(List<Ledger.Transaction> txlist )
        {                                                             
            if (MaximumNumberOfItems.HasValue)
            {
                int startdata = Math.Max(0, txlist.Count - MaximumNumberOfItems.Value);
                return txlist.GetRange(startdata, txlist.Count - startdata);
            }
            else if (MaximumDataAge.HasValue)
            {
                var oldestData = DateTime.UtcNow.Subtract(MaximumDataAge.Value);
                return txlist.Where(x => x.EventTimeUTC >= oldestData).ToList();
            }
            else
                return txlist;
        }

        public static void InitaliseComboBox(ExtendedControls.ExtComboBox cc, string last, bool incldock, bool inclnumberlimit, bool inclstartend)
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
                TravelHistoryFilter.LastTwoYears(),
                TravelHistoryFilter.LastThreeYears(),
            };

            //HtmlElementErrorEventArgs pick up from config extra time filters

            if (inclnumberlimit)
            {
                el.Add(TravelHistoryFilter.Last(10));
                el.Add(TravelHistoryFilter.Last(20));
                el.Add(TravelHistoryFilter.Last(100));
                el.Add(TravelHistoryFilter.Last(500));
            };

            if (incldock)
            {
                el.Add(TravelHistoryFilter.LastDock());
            }
            if (inclstartend)
            {
                el.Add(TravelHistoryFilter.StartEnd());
            }

            cc.DataSource = el;

            int entry = el.FindIndex(x => x.Label == last);
            //System.Diagnostics.Debug.WriteLine(dbname + "=" + last + "=" + entry);
            cc.SelectedIndex = (entry >= 0) ? entry : 0;

            cc.Enabled = true;
        }

        public static void EditUserDataTimeRange()
        {
           // ...
        }
    }

}