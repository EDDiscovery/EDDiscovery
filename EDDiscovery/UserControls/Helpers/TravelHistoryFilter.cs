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

        public static void InitaliseComboBox( ExtendedControls.ExtComboBox cc , string dbname , bool incldockstartend = true )
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

}