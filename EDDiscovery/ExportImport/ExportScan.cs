using EDDiscovery.EliteDangerous;
using EDDiscovery.EliteDangerous.JournalEvents;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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
            scans = entries.ConvertAll<JournalScan>(x => (JournalScan)x);

            return true;
        }

        override public bool ToCSV(string filename)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filename))
            {
                if (IncludeHeader)
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
                        writer.Write("EarthMasses" + delimiter);
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
                }

                foreach (JournalScan je in scans)
                {

                    JournalScan scan = je as JournalScan;

                    if (ShowPlanets == false)  // Then only show stars.
                            if (String.IsNullOrEmpty(scan.StarType))
                            continue;


                    if (ShowStars == false)   // Then only show planets
                            if (String.IsNullOrEmpty(scan.PlanetClass))
                            continue;


                    writer.Write(MakeValueCsvFriendly(scan.EventTimeUTC));
                    writer.Write(MakeValueCsvFriendly(scan.BodyName));
                    writer.Write(MakeValueCsvFriendly(scan.DistanceFromArrivalLS));

                    if (ShowStars)
                    {
                        writer.Write(MakeValueCsvFriendly(scan.StarType));
                            writer.Write(MakeValueCsvFriendly((scan.nStellarMass.HasValue) ? scan.nStellarMass.Value : 0));
                            writer.Write(MakeValueCsvFriendly((scan.nAbsoluteMagnitude.HasValue) ? scan.nAbsoluteMagnitude.Value : 0));
                            writer.Write(MakeValueCsvFriendly((scan.nAge.HasValue) ? scan.nAge.Value : 0));
                    }


                        writer.Write(MakeValueCsvFriendly(scan.nRadius.HasValue ? scan.nRadius.Value : 0));
                        writer.Write(MakeValueCsvFriendly(scan.nRotationPeriod.HasValue ? scan.nRotationPeriod.Value : 0));
                        writer.Write(MakeValueCsvFriendly(scan.nSurfaceTemperature.HasValue ? scan.nSurfaceTemperature.Value : 0));

                    if (ShowPlanets)
                    {
                            writer.Write(MakeValueCsvFriendly(scan.nTidalLock.HasValue ? scan.nTidalLock.Value : false));
                            writer.Write(MakeValueCsvFriendly((scan.TerraformState != null )? scan.TerraformState : ""));
                            writer.Write(MakeValueCsvFriendly((scan.PlanetClass != null )? scan.PlanetClass : ""));
                            writer.Write(MakeValueCsvFriendly((scan.Atmosphere != null )? scan.Atmosphere : ""));
                            writer.Write(MakeValueCsvFriendly((scan.Volcanism != null )? scan.Volcanism : ""));
                            writer.Write(MakeValueCsvFriendly(scan.nSurfaceGravity.HasValue ? scan.nSurfaceGravity.Value : 0));
                            writer.Write(MakeValueCsvFriendly(scan.nSurfacePressure.HasValue ? scan.nSurfacePressure.Value : 0));
                            writer.Write(MakeValueCsvFriendly(scan.nLandable.HasValue ? scan.nLandable.Value : false));
                            writer.Write(MakeValueCsvFriendly((scan.nMassEM.HasValue) ? scan.nMassEM.Value : 0));
                    }
                    // Common orbital param
                        writer.Write(MakeValueCsvFriendly(scan.nSemiMajorAxis.HasValue ? scan.nSemiMajorAxis.Value : 0));
                        writer.Write(MakeValueCsvFriendly(scan.nEccentricity.HasValue ? scan.nEccentricity.Value : 0));
                        writer.Write(MakeValueCsvFriendly(scan.nOrbitalInclination.HasValue ? scan.nOrbitalInclination.Value : 0));
                        writer.Write(MakeValueCsvFriendly(scan.nPeriapsis.HasValue ? scan.nPeriapsis.Value : 0));
                        writer.Write(MakeValueCsvFriendly(scan.nOrbitalPeriod.HasValue ? scan.nOrbitalPeriod.Value : 0));



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
            catch (IOException)
            {
                MessageBox.Show(String.Format("Is file {0} open?", filename), "Export Scan",
                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

  
    }
}
