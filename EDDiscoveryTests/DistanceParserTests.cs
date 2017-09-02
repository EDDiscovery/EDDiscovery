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
using EDDiscovery;
using NFluent;
using NUnit.Framework;
using System;
using System.Globalization;

namespace EDDiscoveryTests
{
    [TestFixture(TestOf = typeof(DistanceParser))]
    public class DistanceParserTests
    {
        private static readonly CultureInfo frenchCulture = CultureInfo.GetCultureInfo("fr-FR");
        private static readonly CultureInfo americanCulture = CultureInfo.GetCultureInfo("en-US");

        private static void TestWithCulture(CultureInfo culture, Action test)
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

        [Test]
        public void Empty_jump_distance_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("")).IsNull();
        }

        [Test]
        public void Jump_distance_without_decimals_is_parsed_correctly()
        {
            Check.That(DistanceParser.ParseJumpDistance("15")).IsEqualTo(15);
        }

        [Test]
        public void Jump_distance_with_point_as_decimal_separator_is_parsed_correctly()
        {
            Check.That(DistanceParser.ParseJumpDistance("15.5")).IsEqualTo(15.5);
        }

        [Test]
        public void Jump_distance_with_point_as_decimal_separator_is_parsed_correctly_with_french_culture()
        {
            TestWithCulture(frenchCulture, Jump_distance_with_point_as_decimal_separator_is_parsed_correctly);
        }

        [Test]
        public void Jump_distance_with_point_as_decimal_separator_is_parsed_correctly_with_american_culture()
        {
            TestWithCulture(americanCulture, Jump_distance_with_point_as_decimal_separator_is_parsed_correctly);
        }

        [Test]
        public void Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly()
        {
            Check.That(DistanceParser.ParseJumpDistance("15,5")).IsEqualTo(15.5);
        }

        [Test]
        public void Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly_with_french_culture()
        {
            TestWithCulture(frenchCulture, Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly);
        }

        [Test]
        public void Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly_with_american_culture()
        {
            TestWithCulture(americanCulture, Jump_distance_with_comma_as_decimal_separator_is_parsed_correctly);
        }

        [Test]
        public void Jump_distance_greater_than_the_maximum_allowed_value_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("15", 14)).IsNull();
        }

        [Test]
        public void Jump_distance_equal_to_the_maximum_allowed_value_is_parsed_correctly()
        {
            Check.That(DistanceParser.ParseJumpDistance("15", 15)).IsEqualTo(15);
        }

        [Test]
        public void Negative_jump_distance_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("-15")).IsNull();
        }

        [Test]
        public void Jump_distance_with_more_than_2_decimals_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("15.000")).IsNull();
        }

        [Test]
        public void Jump_distance_that_does_not_match_any_expected_format_is_not_parsed()
        {
            Check.That(DistanceParser.ParseJumpDistance("not a distance")).IsNull();
        }

        [Test]
        public void Interstellar_distance_without_any_decimal_separator_can_be_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("12345")).IsEqualTo(12345);
        }

        [Test]
        public void Interstellar_distance_with_dot_decimal_separator_can_be_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("12345.6789")).IsEqualTo(12345.6789);
        }

        [Test]
        public void Interstellar_distance_with_dot_decimal_separator_can_be_parsed_with_french_culture()
        {
            TestWithCulture(frenchCulture, Interstellar_distance_with_dot_decimal_separator_can_be_parsed);
        }

        [Test]
        public void Interstellar_distance_with_dot_decimal_separator_can_be_parsed_with_american_culture()
        {
            TestWithCulture(americanCulture, Interstellar_distance_with_dot_decimal_separator_can_be_parsed);
        }

        [Test]
        public void Negative_interstellar_distance_is_not_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("-12345")).IsEqualTo(null);
        }

        [Test]
        public void Interstellar_distance_with_comma_decimal_separator_can_be_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("12345,6789")).IsEqualTo(12345.6789);
        }

        [Test]
        public void Interstellar_distance_with_comma_decimal_separator_can_be_parsed_with_french_culture()
        {
            TestWithCulture(frenchCulture, Interstellar_distance_with_comma_decimal_separator_can_be_parsed);
        }

        [Test]
        public void Interstellar_distance_with_comma_decimal_separator_can_be_parsed_with_american_culture()
        {
            TestWithCulture(americanCulture, Interstellar_distance_with_comma_decimal_separator_can_be_parsed);
        }

        [Test]
        public void Invalid_interstellar_is_not_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("not a distance")).IsNull();
        }

        [Test]
        public void Empty_interstellar_is_not_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance("")).IsNull();
        }

        [Test]
        public void Null_interstellar_is_not_parsed()
        {
            Check.That(DistanceParser.ParseInterstellarDistance(null)).IsNull();
        }
    }
}
