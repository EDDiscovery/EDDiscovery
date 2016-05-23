using System;
using System.Collections.Generic;
using EDDiscovery;
using NFluent;
using EDDiscovery2.DB;

namespace EDDiscoveryTests
{
#if false
    // Visual Studio Test Framework
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    partial class TestFixtureAttribute : Attribute { }
    partial class TestAttribute : Attribute { }
#else
    // NUnit Test Framework
    using NUnit.Framework;

    partial class TestClassAttribute : Attribute { }
    partial class TestMethodAttribute : Attribute { }
#endif

    [TestFixture]
    [TestClass]
    public class TravelHistoryFilterTests
    {
        [Test]
        [TestMethod]
        public void No_filter_does_not_filter_anything()
        {
            var veryOldData = new VisitedSystemsClass { Time = DateTime.Now.Subtract(TimeSpan.FromDays(500000))};
            var input = new List<VisitedSystemsClass> { veryOldData };

            Check.That(TravelHistoryFilter.NoFilter.Filter(input)).ContainsExactly(veryOldData);
        }

        [Test]
        [TestMethod]
        public void Data_age_filter_removes_data_older_than_the_limit_and_keeps_data_more_recent_than_the_limit()
        {
            var now = new VisitedSystemsClass { Time = DateTime.Now };
            var fourDaysAgo = new VisitedSystemsClass { Time = DateTime.Now.Subtract(TimeSpan.FromDays(4))};
            var input = new List<VisitedSystemsClass> { fourDaysAgo, now };

            Check.That(TravelHistoryFilter.FromDays(2).Filter(input)).ContainsExactly(now);
        }

        [Test]
        [TestMethod]
        public void Last_2_items_filter_returns_the_2_most_recent_items_sorted_by_most_recent_and_removes_the_older_items()
        {
            var twentyDaysAgo = new VisitedSystemsClass { Time = DateTime.Now.Subtract(TimeSpan.FromDays(20)) };
            var tenDaysAgo = new VisitedSystemsClass { Time = DateTime.Now.Subtract(TimeSpan.FromDays(10)) };
            var thirtyDaysAgo = new VisitedSystemsClass { Time = DateTime.Now.Subtract(TimeSpan.FromDays(30)) };
            var input = new List<VisitedSystemsClass> { twentyDaysAgo, tenDaysAgo, thirtyDaysAgo };

            Check.That(TravelHistoryFilter.Last(2).Filter(input)).ContainsExactly(tenDaysAgo, twentyDaysAgo);
        }

        [Test]
        [TestMethod]
        public void No_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.NoFilter.Label).IsEqualTo("All");
        }

        [Test]
        [TestMethod]
        public void last_20_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.Last(20).Label).IsEqualTo("last 20");
        }

        [Test]
        [TestMethod]
        public void Last_6_hours_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromHours(6).Label).IsEqualTo("6 hours");
        }

        [Test]
        [TestMethod]
        public void Last_days_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromDays(3).Label).IsEqualTo("3 days");
        }

        [Test]
        [TestMethod]
        public void Last_week_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromWeeks(1).Label).IsEqualTo("week");
        }

        [Test]
        [TestMethod]
        public void Last_3_weeks_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.FromWeeks(3).Label).IsEqualTo("3 weeks");
        }

        [Test]
        [TestMethod]
        public void Last_month_filter_has_correct_label()
        {
            Check.That(TravelHistoryFilter.LastMonth().Label).IsEqualTo("month");
        }
    }
}
