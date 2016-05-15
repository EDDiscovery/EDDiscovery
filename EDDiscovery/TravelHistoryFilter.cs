using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<SystemPosition> Filter(List<SystemPosition> input)
        {
            if (MaximumNumberOfItems.HasValue)
            {
                return input.OrderByDescending(s => s.time).Take(MaximumNumberOfItems.Value).ToList();
            }
            else if(MaximumDataAge.HasValue)
            {
                var oldestData = DateTime.Now.Subtract(MaximumDataAge.Value);
                return (from systems in input where systems.time > oldestData orderby systems.time descending select systems).ToList();
            }
            else
            {
                return input;
            }
        }
    }
}