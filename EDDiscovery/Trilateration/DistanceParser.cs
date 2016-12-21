using System.Globalization;
using System.Text.RegularExpressions;

namespace EDDiscovery
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
