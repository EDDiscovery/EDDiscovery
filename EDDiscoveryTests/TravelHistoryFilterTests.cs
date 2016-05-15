using System;
using System.Collections.Generic;
using EDDiscovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NFluent;

namespace EDDiscoveryTests
{
    [TestClass]
    public class TravelHistoryFilterTests
    {
        [TestMethod]
        public void No_filter_does_not_filter_anything()
        {
            var veryOldData = new SystemPosition { time = DateTime.Now.Subtract(TimeSpan.FromDays(500000))};
            var input = new List<SystemPosition> { veryOldData };

            Check.That(TravelHistoryFilter.NoFilter.Filter(input)).ContainsExactly(veryOldData);
        }

        [TestMethod]
        public void Data_age_filter_removes_data_older_than_the_limit_and_keeps_data_more_recent_than_the_limit()
        {
            var now = new SystemPosition { time = DateTime.Now };
            var fourDaysAgo = new SystemPosition {time = DateTime.Now.Subtract(TimeSpan.FromDays(4))};
            var input = new List<SystemPosition> { fourDaysAgo, now };

            Check.That(TravelHistoryFilter.FromDays(2).Filter(input)).ContainsExactly(now);
        }

        [TestMethod]
        public void Last_2_items_filter_returns_the_2_most_recent_items_sorted_by_most_recent_and_removes_the_older_items()
        {
            var twentyDaysAgo = new SystemPosition { time = DateTime.Now.Subtract(TimeSpan.FromDays(20)) };
            var tenDaysAgo = new SystemPosition { time = DateTime.Now.Subtract(TimeSpan.FromDays(10)) };
            var thirtyDaysAgo = new SystemPosition { time = DateTime.Now.Subtract(TimeSpan.FromDays(30)) };
            var input = new List<SystemPosition> { twentyDaysAgo, tenDaysAgo, thirtyDaysAgo };

            Check.That(TravelHistoryFilter.Last(2).Filter(input)).ContainsExactly(tenDaysAgo, twentyDaysAgo);
        }

        [TestMethod]
        public void No_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.NoFilter.Label).IsEqualTo("All");
        }

        [TestMethod]
        public void last_20_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.Last(20).Label).IsEqualTo("last 20");
        }

        [TestMethod]
        public void Last_6_hours_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromHours(6).Label).IsEqualTo("6 hours");
        }

        [TestMethod]
        public void Last_days_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromDays(3).Label).IsEqualTo("3 days");
        }

        [TestMethod]
        public void Last_week_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromWeeks(1).Label).IsEqualTo("week");
        }

        [TestMethod]
        public void Last_3_weeks_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromWeeks(3).Label).IsEqualTo("3 weeks");
        }

        [TestMethod]
        public void Last_month_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.LastMonth().Label).IsEqualTo("month");
        }
    }
}
