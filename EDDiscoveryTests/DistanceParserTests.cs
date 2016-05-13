using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EDDiscovery;
using NFluent;

namespace EDDiscoveryTests
{
    [TestClass]
    public class DistanceParserTests
    {
        [TestMethod]
        public void Empty_value_is_not_parsed()
        {
            Check.That(DistanceParser.DistanceAsDouble("")).IsNull();
        }

        [TestMethod]
        public void Distance_without_decimals_is_parsed_correctly()
        {
            Check.That(DistanceParser.DistanceAsDouble("15")).IsEqualTo(15);
        }

        [TestMethod]
        public void Distance_with_point_as_decimal_separator_is_parsed_correctly()
        {
            Check.That(DistanceParser.DistanceAsDouble("15.5")).IsEqualTo(15.5);
        }

        [TestMethod]
        public void Distance_with_comma_as_decimal_separator_is_parsed_correctly()
        {
            Check.That(DistanceParser.DistanceAsDouble("15,5")).IsEqualTo(15.5);
        }

        [TestMethod]
        public void Distance_greater_than_the_maximum_allowed_value_is_not_parsed()
        {
            Check.That(DistanceParser.DistanceAsDouble("10", 9)).IsNull();
        }

        [TestMethod]
        public void Distance_that_does_not_match_any_expected_format_is_not_parsed()
        {
            Check.That(DistanceParser.DistanceAsDouble("not a distance")).IsNull();
        }
    }
}
