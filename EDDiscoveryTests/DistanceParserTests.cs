using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Check.That(DistanceParser.DistanceAsDouble("15", 14)).IsNull();
        }

        [TestMethod]
        public void Distance_equal_to_the_maximum_allowed_value_is_parsed_correctly()
        {
            Check.That(DistanceParser.DistanceAsDouble("15", 15)).IsEqualTo(15);
        }

        [TestMethod]
        public void Negative_distance_is_not_parsed()
        {
            Check.That(DistanceParser.DistanceAsDouble("-15")).IsNull();
        }

        [TestMethod]
        public void Distance_with_more_than_2_decimals_is_not_parsed()
        {
            Check.That(DistanceParser.DistanceAsDouble("15.000")).IsNull();
        }

        [TestMethod]
        public void Distance_that_does_not_match_any_expected_format_is_not_parsed()
        {
            Check.That(DistanceParser.DistanceAsDouble("not a distance")).IsNull();
        }
    }
}
