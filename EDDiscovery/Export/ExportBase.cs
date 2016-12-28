using EDDiscovery.EliteDangerous;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EDDiscovery.Export
{
    public enum CSVFormat
    {
        USA_UK = 0,
        EU = 1,
    }

    public abstract class ExportBase
    {
        protected string delimiter = ",";
        protected CultureInfo formatculture;
        private CSVFormat csvformat;
        public bool IncludeHeader;

        public CSVFormat Csvformat
        {
            get
            {
                return csvformat;
            }

            set
            {
                csvformat = value;
                if (csvformat == CSVFormat.EU)
                {
                    delimiter = ";";
                    formatculture = new System.Globalization.CultureInfo("sv");
                }
                else
                {
                    delimiter = ",";
                    formatculture = new System.Globalization.CultureInfo("en-US");
                }
            }
        }

        abstract public bool ToCSV(string filename);
        abstract public bool GetData(EDDiscoveryForm _discoveryForm);

        protected string MakeValueCsvFriendly(object value, bool delimit = true)
        {
            string output = "";

            if ( value != null && !(value is INullable && ((INullable)value).IsNull) )  // if not null
            {
                if (value is DateTime)
                {
                    if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                        return ((DateTime)value).ToString("yyyy-MM-dd") + delimiter;

                    output = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    output = Convert.ToString(value, formatculture);

                    if (output.Contains(",") || output.Contains("\"") || output.Contains("\r") || output.Contains("\n"))
                    {
                        output = output.Replace("\r\n", "\n");
                        output = output.Replace("\"", "\"\"");
                        output = "\"" + output + "\"";
                    }
                }
            }

            if (delimit)
                return output + delimiter;
            else
                return output;
        }
    }
}
