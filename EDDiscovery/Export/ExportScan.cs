using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace EDDiscovery.Export
{
    public class ExportScan
    {
        string delimiter = ",";

        public bool ScanToCSV(string filename, List<JournalEntry> scans)
        {
            if (CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator.Equals(","))
                delimiter = ";";

            using (StreamWriter writer = new StreamWriter(filename))
            {
                // Write header

                writer.Write("Time" + delimiter);
                writer.Write("BodyName" + delimiter);
                writer.Write("DistanceFromArrivalLS" + delimiter);
                writer.Write("StarType" + delimiter);
                writer.Write("StellarMass" + delimiter);
                writer.Write("Radius" + delimiter);
                writer.Write("AbsoluteMagnitude" + delimiter);
                writer.Write("OrbitalPeriod" + delimiter);


                writer.Write("TidalLock" + delimiter);
                writer.Write("TerraformState" + delimiter);
                writer.Write("PlanetClass" + delimiter);
                writer.Write("Atmosphere" + delimiter);
                writer.Write("Volcanism" + delimiter);
                writer.Write("SurfaceGravity" + delimiter);
                writer.Write("SurfaceTemperature" + delimiter);
                writer.Write("SurfacePressure" + delimiter);
                writer.Write("Landable" + delimiter);
                writer.Write("OrbitalPeriod" + delimiter);
                writer.Write("RotationPeriod" + delimiter);






                writer.WriteLine();

                foreach (JournalEntry je in scans)
                {
                    if (je.EventTypeID == JournalTypeEnum.Scan)
                    {
                        JournalScan scan = je as JournalScan;

                        writer.Write(MakeValueCsvFriendly(scan.EventTimeUTC));
                        writer.Write(MakeValueCsvFriendly(scan.BodyName));
                        writer.Write(MakeValueCsvFriendly(scan.DistanceFromArrivalLS));
                        writer.Write(MakeValueCsvFriendly(scan.StarType));
                        writer.Write(MakeValueCsvFriendly(scan.StellarMass));
                        writer.Write(MakeValueCsvFriendly(scan.Radius));
                        writer.Write(MakeValueCsvFriendly(scan.AbsoluteMagnitude));
                        writer.Write(MakeValueCsvFriendly(scan.OrbitalPeriod));

                        writer.Write(MakeValueCsvFriendly(scan.TidalLock));
                        writer.Write(MakeValueCsvFriendly(scan.TerraformState));
                        writer.Write(MakeValueCsvFriendly(scan.PlanetClass));
                        writer.Write(MakeValueCsvFriendly(scan.Atmosphere));
                        writer.Write(MakeValueCsvFriendly(scan.Volcanism));
                        writer.Write(MakeValueCsvFriendly(scan.SurfaceGravity));
                        writer.Write(MakeValueCsvFriendly(scan.SurfaceTemperature));
                        writer.Write(MakeValueCsvFriendly(scan.SurfacePressure));
                        writer.Write(MakeValueCsvFriendly(scan.Landable));
                        writer.Write(MakeValueCsvFriendly(scan.OrbitalPeriod));
                        writer.Write(MakeValueCsvFriendly(scan.RotationPeriod));

                        writer.WriteLine();
                    }
                }
            }

            return true;
        }

      private string MakeValueCsvFriendly(object value)
        {
            if (value == null) return delimiter;
            if (value is Nullable && ((INullable)value).IsNull) return delimiter;

            if (value is DateTime)
            {
                if (((DateTime)value).TimeOfDay.TotalSeconds == 0)
                    return ((DateTime)value).ToString("yyyy-MM-dd") + delimiter; ;
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + delimiter; ;
            }
            string output = value.ToString();

            if (output.Contains(",") || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';

            return output + delimiter;

        }
    }
}
