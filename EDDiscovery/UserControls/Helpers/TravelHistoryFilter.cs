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
using ExtendedControls;
using QuickJSON;
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
            Label = EDDConfig.Instance.ConvertTimeToSelectedFromUTC(StartDateUTC.Value).ToString() + " .. " + EDDConfig.Instance.ConvertTimeToSelectedFromUTC(EndDateUTC.Value).ToString();
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

        // list should be in entry order. oldest first
        public List<HistoryEntry> Filter(List<HistoryEntry> list, HashSet<JournalTypeEnum> entries = null, bool reverse = true)      
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

        // list should be in latest first order, supports a limited set of options
        public List<HistoryEntry> FilterLatestFirst(List<HistoryEntry> list)      
        {
            if (MaximumNumberOfItems.HasValue)
            {
                return list.GetRange(0, Math.Min(MaximumNumberOfItems.Value, list.Count));
            }
            else if (MaximumDataAge.HasValue)
            {
                DateTime lastentry = list.Count > 0 ? list.First().EventTimeUTC : DateTime.UtcNow;       // if a list, last date in list, else UTC now
                var oldestData = lastentry.Subtract(MaximumDataAge.Value);
                int index = list.FindIndex(x => x.EventTimeUTC < oldestData);       // find first entry with date younger than oldest data
                return index >= 0 ? list.GetRange(0, index) : list;                 // if not found one, they are all younger than the time, so its all, else its a range
            }
            else if (StartDateUTC.HasValue)
            {
                return HistoryList.LimitByDate(list, StartDateUTC.Value, EndDateUTC.Value, reverse:false);
            }
            else
            {
                return list;
            }
        }

        // list in is in ascending order, oldest first, return in ascending date order
        public List<Ledger.Transaction> Filter(List<Ledger.Transaction> list )
        {                                                             
            if (MaximumNumberOfItems.HasValue)
            {
                int startdata = Math.Max(0, list.Count - MaximumNumberOfItems.Value);
                return list.GetRange(startdata, list.Count - startdata);
            }
            else if (MaximumDataAge.HasValue)
            {
                DateTime lastentry = list.Count > 0 ? list.Last().EventTimeUTC : DateTime.UtcNow;       // if a list, last date in list, else UTC now
                var oldestData = lastentry.Subtract(MaximumDataAge.Value);
                return list.Where(x => x.EventTimeUTC >= oldestData).ToList();
            }
            else
                return list;
        }

        // given a combobox, and the current setting, fill with time selection options
        // if customtimerangesettings, its a JSON with start/end dates
        // return if we can select the same entry
        public static bool InitialiseComboBox(ExtComboBox cc, string currentsetting, bool incldock, bool inclnumberlimit, bool inclstartend, string customtimerangesetting = null)
        {
            cc.Enabled = false;
            cc.DisplayMember = nameof(TravelHistoryFilter.Label);

            List<TravelHistoryFilter> el = new List<TravelHistoryFilter>()
            {
                NoFilter,
                FromHours(6),
                FromHours(12),
                FromHours(24),
                FromDays(3),
                FromWeeks(1),
                FromWeeks(2),
                LastMonth(),
                LastQuarter(),
                LastHalfYear(),
                LastYear(),
                LastTwoYears(),
                LastThreeYears(),
            };

            if ( customtimerangesetting!=null)
            {
                var customrange = ConvertTimeRangeFromJson(customtimerangesetting);   // may be null if bad. Returns in UTC
                if (customrange != null)
                {
                    for( int i = 0; i < customrange.Item1.Count; i++)
                        el.Add(StartEnd(customrange.Item1[i], customrange.Item2[i]));
                }
            }

            if (inclnumberlimit)
            {
                el.Add(Last(10));
                el.Add(Last(20));
                el.Add(Last(100));
                el.Add(Last(500));
            };

            if (incldock)
            {
                el.Add(LastDock());
            }
            if (inclstartend)
            {
                el.Add(StartEnd());
            }

            cc.DataSource = el;

            // find current setting
            int entry = el.FindIndex(x => x.Label == currentsetting);

            // if not found, we select All, else we select the entry
            cc.SelectedIndex = entry >= 0 ? entry : 0;

            cc.Enabled = true;

            return entry >= 0;      // return if selected current setting
        }


        // Edit time settings
        public static string EditUserDataTimeRange(Form form, string settings)
        {
            ConfigurableForm frm = new ConfigurableForm();

            int vpos = 32, spacing = 32;
            int addvpos = vpos;                         // record where these fields start from
            int controlnumber = 0;                      // a unique ID which always incremements

            var customrange = ConvertTimeRangeFromJson(settings);   // may be null, all times in UTC
            if (customrange != null)
            {
                for (int i = 0; i < customrange.Item1.Count ; i++)
                {
                    AddDT(frm, customrange.Item1[i], customrange.Item2[i], vpos, controlnumber++);  // converts to local and presents local
                    vpos += spacing;
                }
            }

            frm.Add(new ConfigurableEntryList.Entry("add", typeof(ExtButton), "+", new Point(8, vpos), new Size(24, 24), "Add a new date time range"));
            frm.AddOK(new Point(420, vpos), "OK");
            frm.InstallStandardTriggers();
            frm.Trigger += (name, control, obj) =>      // same as spansh extButtonFleetCarrier_Click
            {
                if (control == "add")
                {
                    int numcontrols = frm.GetByStartingName("dates:").Length;     // get number up there
                    int pos = addvpos + numcontrols * spacing;           // work out where next should be
                    frm.MoveControls(pos - 10, spacing);                     // move anything after this (less a bit for safety) to space
                    AddDT(frm, DateTime.UtcNow.StartOfMinute(), DateTime.UtcNow.StartOfMinute(), pos, controlnumber++);     // give current UTC time, it will be converted local by AddDT
                    frm.UpdateDisplayAfterAddNewControls();
                }
                else if (control.StartsWith("del:"))
                {
                    //System.Diagnostics.Debug.WriteLine($"Delete control {control}");
                    frm.MoveControls(control, -32);                   // move everything at or below this up
                    frm.Remove(control);                         // and remove the two controls
                    frm.Remove("dates:" + control.Substring(4));
                    frm.Remove("datee:" + control.Substring(4));
                    frm.UpdateEntries();
                }

            };

            if (frm.ShowDialogCentred(form, form.Icon, "Times", closeicon: true) == DialogResult.OK)
            {
                DateTime[] startt = frm.GetByStartingName<DateTime>("dates:");  // local time
                DateTime[] endt = frm.GetByStartingName<DateTime>("datee:");    // local time
                JObject jo = new JObject { ["start"] = new JArray(), ["end"] = new JArray() };
                jo["start"].AddRange(startt.Select(x => EDDConfig.Instance.ConvertTimeToUTCFromPicker(x)));     // store, but convert back to UTC before storing
                jo["end"].AddRange(endt.Select(x => EDDConfig.Instance.ConvertTimeToUTCFromPicker(x)));
                return jo.ToString();
            }

            return null;
        }

        // JSON string->DateTimes
        // JSON string keeps it in UTC so all times returned are UTC
        public static Tuple<List<DateTime>, List<DateTime>> ConvertTimeRangeFromJson(string settings)
        {
            try
            {
                JObject jo = JObject.Parse(settings);
                if (!(jo == null || !jo.Contains("start") || !jo.Contains("end") || jo["start"].Count != jo["end"].Count))     // triage ok
                    return new Tuple<List<DateTime>, List<DateTime>>(jo["start"].Array().DateTime(), jo["end"].Array().DateTime());
            }
            catch ( Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"ConvertTimeRangeFromJSON bad settings string {settings} {ex}");
            }
            return null;
        }

        // taking the UTC times, produce a local entry
        private static void AddDT(ConfigurableForm frm, DateTime starttimeutc, DateTime endtimeutc, int vpos, int controlnumber)
        {
            frm.Add(new ConfigurableEntryList.Entry("dates:" + controlnumber, EDDConfig.Instance.ConvertTimeToSelectedFromUTC(starttimeutc), new Point(8, vpos), new Size(200, 24), null) { CustomDateFormat = "yyyy-MM-dd HH:mm:ss" });
            frm.Add(new ConfigurableEntryList.Entry("datee:" + controlnumber, EDDConfig.Instance.ConvertTimeToSelectedFromUTC(endtimeutc), new Point(250, vpos), new Size(200, 24), null) { CustomDateFormat = "yyyy-MM-dd HH:mm:ss" });
            frm.Add(new ConfigurableEntryList.Entry("del:" + controlnumber, typeof(ExtButton), "X", new Point(470, vpos), new Size(24, 24), null));
        }
    }

}