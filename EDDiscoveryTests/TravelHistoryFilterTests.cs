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

            Check.That(new TravelHistoryFilter(TimeSpan.FromDays(2)).Filter(input)).ContainsExactly(now);
        }

        [TestMethod]
        public void Last_2_items_filter_returns_the_2_most_recent_items_sorted_by_most_recent_and_removes_the_older_items()
        {
            var twentyDaysAgo = new SystemPosition { time = DateTime.Now.Subtract(TimeSpan.FromDays(20)) };
            var tenDaysAgo = new SystemPosition { time = DateTime.Now.Subtract(TimeSpan.FromDays(10)) };
            var thirtyDaysAgo = new SystemPosition { time = DateTime.Now.Subtract(TimeSpan.FromDays(30)) };
            var input = new List<SystemPosition> { twentyDaysAgo, tenDaysAgo, thirtyDaysAgo };

            Check.That(new TravelHistoryFilter(2).Filter(input)).ContainsExactly(tenDaysAgo, twentyDaysAgo);
        }
    }
}
