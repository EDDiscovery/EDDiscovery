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


        protected string MakeValueCsvFriendly(object value)
        {
            if (value == null) return delimiter;
            if (value is INullable && ((INullable)value).IsNull) return delimiter;

            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime)value).ToString("yyyy-MM-dd") + delimiter; ;
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + delimiter; ;
            }
            string output = Convert.ToString(value, CultureInfo.InvariantCulture);

            if (output.Contains(",") || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';

            return output + delimiter;

        }


    }
}
