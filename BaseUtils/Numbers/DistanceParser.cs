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
using System.Globalization;
using System.Text.RegularExpressions;

namespace BaseUtils
{
    public static class DistanceParser
    {
        private static readonly Regex RegexDistance = new Regex(@"^\d+([.,]\d{1,2})?$", RegexOptions.Compiled | RegexOptions.Singleline);

        /// <summary>
        /// Parse a jump distance as a positive double, in the formats "xx", "xx.yy", "xx,yy" or "xxxx,yy".
        /// </summary>
        /// <param name="value">Decimal string to be parsed.</param>
        /// <param name="maximum">Upper limit or null if not required. If this is set and the parsed value is greater, 
        /// the result will be null</param>
        /// <returns>Parsed value or null on conversion failure or null if value is greater than maximum value.</returns>
        public static double? ParseJumpDistance(string value, double? maximum = null)
        {
            if (value.Length == 0)
            {
                return null;
            }

            if (!RegexDistance.IsMatch(value))
            {
                return null;
            }

            // Replace comma decimal point separator with dot, as it is the invariant culture separator that we
            // will be using to parse the distance. TryParse is not need, the regex ensures it always parses
            var valueDouble = double.Parse(value.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

            if (maximum.HasValue && valueDouble > maximum)
            {
                return null;
            }

            return valueDouble;
        }

        public static double? ParseInterstellarDistance(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            double result;
            if (double.TryParse(input.Replace(",", "."), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
    }
}
