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
    public class ExportScan : ExportBase
    {
        private List<JournalScan> scans;
        private bool ShowPlanets;
        private bool ShowStars;


        public ExportScan()
        {
            ShowPlanets = true;
            ShowStars = true;
        }

        public ExportScan(bool stars, bool planets)
        {
            ShowPlanets = planets;
            ShowStars = stars;
        }


        override public bool GetData(EDDiscoveryForm _discoveryForm)
        {
            var filter = _discoveryForm.TravelControl.GetPrimaryFilter;

            List<HistoryEntry> result = filter.Filter(_discoveryForm.history);

            scans = new List<JournalScan>();

            var entries = JournalEntry.GetByEventType(JournalTypeEnum.Scan, EDDiscoveryForm.EDDConfig.CurrentCmdrID, _discoveryForm.history.GetMinDate, _discoveryForm.history.GetMaxDate);
            scans = scans.ConvertAll<JournalScan>(x => (JournalScan)x);

            return true;
        }

        override public bool ToCSV(string filename)
        {
   
            using (StreamWriter writer = new StreamWriter(filename))
            {
                // Write header

                writer.Write("Time" + delimiter);
                writer.Write("BodyName" + delimiter);
                writer.Write("DistanceFromArrivalLS" + delimiter);
                if (ShowStars)
                {
                    writer.Write("StarType" + delimiter);
                    writer.Write("StellarMass" + delimiter);
                    writer.Write("AbsoluteMagnitude" + delimiter);
                    writer.Write("Age MY" + delimiter);
                }
                writer.Write("Radius" + delimiter);
                writer.Write("RotationPeriod" + delimiter);
                writer.Write("SurfaceTemperature" + delimiter);

                if (ShowPlanets)
                {
                    writer.Write("TidalLock" + delimiter);
                    writer.Write("TerraformState" + delimiter);
                    writer.Write("PlanetClass" + delimiter);
                    writer.Write("Atmosphere" + delimiter);
                    writer.Write("Volcanism" + delimiter);
                    writer.Write("SurfaceGravity" + delimiter);
                    writer.Write("SurfacePressure" + delimiter);
                    writer.Write("Landable" + delimiter);
                }
                // Common orbital param
                writer.Write("SemiMajorAxis" + delimiter);
                writer.Write("Eccentricity" + delimiter);
                writer.Write("OrbitalInclination" + delimiter);
                writer.Write("Periapsis" + delimiter);
                writer.Write("OrbitalPeriod" + delimiter);


                if (ShowPlanets)
                {
                    writer.Write("Carbon" + delimiter);
                    writer.Write("Iron" + delimiter);
                    writer.Write("Nickel" + delimiter);
                    writer.Write("Phosphorus" + delimiter);
                    writer.Write("Sulphur" + delimiter);
                    writer.Write("Arsenic" + delimiter);
                    writer.Write("Chromium" + delimiter);
                    writer.Write("Germanium" + delimiter);
                    writer.Write("Manganese" + delimiter);
                    writer.Write("Selenium" + delimiter);
                    writer.Write("Vanadium" + delimiter);
                    writer.Write("Zinc" + delimiter);
                    writer.Write("Zirconium" + delimiter);
                    writer.Write("Cadmium" + delimiter);
                    writer.Write("Mercury" + delimiter);
                    writer.Write("Molybdenum" + delimiter);
                    writer.Write("Niobium" + delimiter);
                    writer.Write("Tin" + delimiter);
                    writer.Write("Tungsten" + delimiter);
                    writer.Write("Antimony" + delimiter);
                    writer.Write("Polonium" + delimiter);
                    writer.Write("Ruthenium" + delimiter);
                    writer.Write("Technetium" + delimiter);
                    writer.Write("Tellurium" + delimiter);
                    writer.Write("Yttrium" + delimiter);
                }


                writer.WriteLine();

                foreach (JournalScan je in scans)
                {

                    JournalScan scan = je as JournalScan;

                    if (ShowPlanets == false)  // Then only show stars.
                        if (scan.StarType.Equals(""))
                            continue;


                    if (ShowStars == false)   // Then only show planets
                        if (scan.PlanetClass.Equals(""))
                            continue;


                    writer.Write(MakeValueCsvFriendly(scan.EventTimeUTC));
                    writer.Write(MakeValueCsvFriendly(scan.BodyName));
                    writer.Write(MakeValueCsvFriendly(scan.DistanceFromArrivalLS));

                    if (ShowStars)
                    {
                        writer.Write(MakeValueCsvFriendly(scan.StarType));
                        writer.Write(MakeValueCsvFriendly(scan.StellarMass));
                        writer.Write(MakeValueCsvFriendly(scan.AbsoluteMagnitude));
                        writer.Write(MakeValueCsvFriendly(scan.Age));
                    }


                    writer.Write(MakeValueCsvFriendly(scan.Radius));
                    writer.Write(MakeValueCsvFriendly(scan.RotationPeriod));
                    writer.Write(MakeValueCsvFriendly(scan.SurfaceTemperature));

                    if (ShowPlanets)
                    {
                        writer.Write(MakeValueCsvFriendly(scan.TidalLock));
                        writer.Write(MakeValueCsvFriendly(scan.TerraformState));
                        writer.Write(MakeValueCsvFriendly(scan.PlanetClass));
                        writer.Write(MakeValueCsvFriendly(scan.Atmosphere));
                        writer.Write(MakeValueCsvFriendly(scan.Volcanism));
                        writer.Write(MakeValueCsvFriendly(scan.SurfaceGravity));
                        writer.Write(MakeValueCsvFriendly(scan.SurfacePressure));
                        writer.Write(MakeValueCsvFriendly(scan.Landable));
                    }
                    // Common orbital param
                    writer.Write(MakeValueCsvFriendly(scan.SemiMajorAxis));
                    writer.Write(MakeValueCsvFriendly(scan.Eccentricity));
                    writer.Write(MakeValueCsvFriendly(scan.OrbitalInclination));
                    writer.Write(MakeValueCsvFriendly(scan.Periapsis));
                    writer.Write(MakeValueCsvFriendly(scan.OrbitalPeriod));



                    if (ShowPlanets)
                    {
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Carbon")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Iron")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Nickel")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Phosphorus")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Sulphur")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Arsenic")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Chromium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Germanium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Manganese")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Selenium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Vanadium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Zinc")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Zirconium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Cadmium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Mercury")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Molybdenum")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Niobium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Tin")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Tungsten")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Antimony")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Polonium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Ruthenium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Technetium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Tellurium")));
                        writer.Write(MakeValueCsvFriendly(scan.GetMaterial("Yttrium")));
                    }


                    writer.WriteLine();

                }
            }

            return true;
        }

  
    }
}
