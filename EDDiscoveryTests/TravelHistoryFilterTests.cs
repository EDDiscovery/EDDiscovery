/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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
using EDDiscovery.UserControls;
using EliteDangerousCore;
using NFluent;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace EDDiscoveryTests
{
    [TestFixture(TestOf = typeof(TravelHistoryFilter))]
    public class TravelHistoryFilterTests
    {
        private readonly ISystem sol = new EliteDangerousCore.SystemClass
        {
            name = "Sol",
            x = 0,
            y = 0,
            z = 0
        };

        [Test]
        public void No_filter_does_not_filter_anything()
        {
            var veryOldData = HistoryEntry.MakeVSEntry(sol, DateTime.UtcNow.Subtract(TimeSpan.FromDays(500000)), 0, "", "");
            var input = new HistoryList(new List<HistoryEntry> { veryOldData });

            Check.That(TravelHistoryFilter.NoFilter.Filter(input)).ContainsExactly(veryOldData);
        }

        [Test]
        public void Data_age_filter_removes_data_older_than_the_limit_and_keeps_data_more_recent_than_the_limit()
        {
            var now = HistoryEntry.MakeVSEntry(sol, DateTime.UtcNow, 0, "", "");
            var fourDaysAgo = HistoryEntry.MakeVSEntry(sol, DateTime.UtcNow.Subtract(TimeSpan.FromDays(4)), 0, "", "");
            var input = new HistoryList(new List<HistoryEntry> { fourDaysAgo, now });

            Check.That(TravelHistoryFilter.FromDays(2).Filter(input)).ContainsExactly(now);
        }

        [Test]
        public void Last_2_items_filter_returns_the_2_most_recent_items_sorted_by_most_recent_and_removes_the_older_items()
        {
            var twentyDaysAgo = HistoryEntry.MakeVSEntry(sol, DateTime.UtcNow.Subtract(TimeSpan.FromDays(20)), 0, "", "");
            var tenDaysAgo = HistoryEntry.MakeVSEntry(sol, DateTime.UtcNow.Subtract(TimeSpan.FromDays(10)), 0, "", "");
            var thirtyDaysAgo = HistoryEntry.MakeVSEntry(sol, DateTime.UtcNow.Subtract(TimeSpan.FromDays(30)), 0, "", "");
            var input = new HistoryList(new List<HistoryEntry> { twentyDaysAgo, tenDaysAgo, thirtyDaysAgo });

            Check.That(TravelHistoryFilter.Last(2).Filter(input)).ContainsExactly(tenDaysAgo, twentyDaysAgo);
        }

        [Test]
        public void No_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.NoFilter.Label).IsEqualTo("All");
        }

        [Test]
        public void last_20_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.Last(20).Label).IsEqualTo("Last 20 entries");
        }

        [Test]
        public void Last_6_hours_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromHours(6).Label).IsEqualTo("6 hours");
        }

        [Test]
        public void Last_days_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromDays(3).Label).IsEqualTo("3 days");
        }

        [Test]
        public void Last_week_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromWeeks(1).Label).IsEqualTo("One Week");
        }

        [Test]
        public void Last_3_weeks_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromWeeks(3).Label).IsEqualTo("3 weeks");
        }

        [Test]
        public void Last_month_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.LastMonth().Label).IsEqualTo("Month");
        }
    }
}
