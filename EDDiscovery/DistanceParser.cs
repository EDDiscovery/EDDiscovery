using System.Globalization;
using System.Text.RegularExpressions;

namespace EDDiscovery
{
    public static class DistanceParser
    {
        /// <summary>
        /// Parse a distance as a positive double, in the formats "xx", "xx.yy", "xx,yy" or "xxxx,yy".
        /// </summary>
        /// <param name="value">Decimal string to be parsed.</param>
        /// <param name="maximum">Upper limit or null if not required.</param>
        /// <returns>Parsed value or null on conversion failure.</returns>
        public static double? DistanceAsDouble(string value, double? maximum = null)
        {
            if (value.Length == 0)
            {
                return null;
            }

            if (!new Regex(@"^\d+([.,]\d{1,2})?$").IsMatch(value))
            {
                return null;
            }

            double valueDouble;

            // Allow regions with , as decimal separator to also use . as decimal separator and vice versa
            var decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (!double.TryParse(decimalSeparator == "," ? value.Replace(".", ",") : value.Replace(",", "."), out valueDouble))
            {
                return null;
            }

            if (maximum.HasValue && valueDouble > maximum)
            {
                return null;
            }

            return valueDouble;
        }
    }
}
