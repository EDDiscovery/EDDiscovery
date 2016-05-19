using System;
using System.Globalization;
using EDDiscovery;
using NFluent;

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
    public class DistanceParserTests
    {
        private static readonly CultureInfo frenchCulture = CultureInfo.GetCultureInfo("fr-FR");
        private static readonly CultureInfo americanCulture = CultureInfo.GetCultureInfo("en-US");

        public static void TestWithCulture(CultureInfo culture, Action test)
        {
            var originalCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            try
            {
                test();
            }
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = originalCulture;
            }
        }

        [TestMethod]
        [Test]
        public void Empty_jump_distance_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("")).IsNull();
        }

        [TestMethod]
        [Test]
        public void Jump_distance_without_decimals_is_parsed_correctly()
        {
            Check.That(DistanceParser.ParseJumpDistance("15")).IsEqualTo(15);
        }

        public void Jump_distance_with_point_as_decimal_separator_is_parsed_correctly()
        {
            Check.That(DistanceParser.ParseJumpDistance("15.5")).IsEqualTo(15.5);
        }

        [TestMethod]
        [Test]
        public void Jump_distance_with_point_as_decimal_separator_is_parsed_correctly_with_french_culture()
        {
            TestWithCulture(frenchCulture, Jump_distance_with_point_as_decimal_separator_is_parsed_correctly);
        }

        [TestMethod]
        [Test]
        public void Jump_distance_with_point_as_decimal_separator_is_parsed_correctly_with_american_culture()
        {
            TestWithCulture(americanCulture, Jump_distance_with_point_as_decimal_separator_is_parsed_correctly);
        }

        public void Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly()
        {
            Check.That(DistanceParser.ParseJumpDistance("15,5")).IsEqualTo(15.5);
        }

        [TestMethod]
        [Test]
        public void Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly_with_french_culture()
        {
            TestWithCulture(frenchCulture, Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly);
        }

        [TestMethod]
        [Test]
        public void Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly_with_american_culture()
        {
            TestWithCulture(americanCulture, Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly);
        }

        [TestMethod]
        [Test]
        public void Jump_distance_greater_than_the_maximum_allowed_value_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("15", 14)).IsNull();
        }

        [TestMethod]
        [Test]
        public void Jump_distance_equal_to_the_maximum_allowed_value_is_parsed_correctly()
        {
            Check.That(DistanceParser.ParseJumpDistance("15", 15)).IsEqualTo(15);
        }

        [TestMethod]
        [Test]
        public void Negative_jump_distance_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("-15")).IsNull();
        }

        [TestMethod]
        [Test]
        public void Jump_distance_with_more_than_2_decimals_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("15.000")).IsNull();
        }

        [TestMethod]
        [Test]
        public void Jump_distance_that_does_not_match_any_expected_format_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("not a distance")).IsNull();
        }

        [TestMethod]
        [Test]
        public void Interstellar_distance_without_any_decimal_separator_can_be_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("12345")).IsEqualTo(12345);
        }

        public void Interstellar_distance_with_dot_decimal_separator_can_be_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("12345.6789")).IsEqualTo(12345.6789);
        }

        [TestMethod]
        [Test]
        public void Interstellar_distance_with_dot_decimal_separator_can_be_parsed_with_french_culture()
        {
            TestWithCulture(frenchCulture, Interstellar_distance_with_dot_decimal_separator_can_be_parsed);
        }

        [TestMethod]
        [Test]
        public void Interstellar_distance_with_dot_decimal_separator_can_be_parsed_with_american_culture()
        {
            TestWithCulture(americanCulture, Interstellar_distance_with_dot_decimal_separator_can_be_parsed);
        }

        [TestMethod]
        [Test]
        public void Negative_interstellar_distance_is_not_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("-12345")).IsEqualTo(null);
        }

        public void Interstellar_distance_with_comma_decimal_separator_can_be_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("12345,6789")).IsEqualTo(12345.6789);
        }

        [TestMethod]
        [Test]
        public void Interstellar_distance_with_comma_decimal_separator_can_be_parsed_with_french_culture()
        {
            TestWithCulture(frenchCulture, Interstellar_distance_with_comma_decimal_separator_can_be_parsed);
        }

        [TestMethod]
        [Test]
        public void Interstellar_distance_with_comma_decimal_separator_can_be_parsed_with_american_culture()
        {
            TestWithCulture(americanCulture, Interstellar_distance_with_comma_decimal_separator_can_be_parsed);
        }


        [TestMethod]
        [Test]
        public void Invalid_interstellar_is_not_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("not a distance")).IsNull();
        }

        [TestMethod]
        [Test]
        public void Empty_interstellar_is_not_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("")).IsNull();
        }

        [TestMethod]
        [Test]
        public void Null_interstellar_is_not_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance(null)).IsNull();
        }
    }
}
