/*
 * Copyright © 2016 EDDiscovery development team
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

using EliteDangerousCore;
using EliteDangerousCore.DB;
using EliteDangerousCore.JournalEvents;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EDDiscovery.UserControls
{
    static public class CSVHelpers
    {
        static public bool WriteGrid( this BaseUtils.CSVWriteGrid grid , string file, bool autoopen, Form parent )
        {
            if (grid.WriteCSV(file))
            {
                if (autoopen)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(file);
                        return true;
                    }
                    catch
                    {
                        FailedToOpen(parent, file);
                    }
                }
                else
                    return true;
            }
            else
                WriteFailed(parent, file);

            return false;
        }

        static public void FailedToOpen(Form parent, string file)
        {
            ExtendedControls.MessageBoxTheme.Show(parent, "Failed to open ".Tx()+ file, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }
        static public void WriteFailed(Form parent, string file)
        {
            ExtendedControls.MessageBoxTheme.Show(parent, "Failed to write to ".Tx()+ file, "Warning".Tx(), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static bool OutputScanCSV(List<JournalScan> scans, string path, string delimiter, bool IncludeHeader, bool ShowStars, bool ShowPlanets, bool showMapped, bool ShowBeltClusters)
        {
            BaseUtils.CSVWrite csv = new BaseUtils.CSVWrite(delimiter);

            try
            {
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(path))
                {
                    if (IncludeHeader)
                    {
                        // Write header - old fashioned, but too many to convert to grid style

                        writer.Write(csv.Format("Time"));
                        writer.Write(csv.Format("BodyName"));
                        writer.Write(csv.Format("Estimated Value"));
                        writer.Write(csv.Format("DistanceFromArrivalLS"));
                        if (showMapped)
                        {
                            writer.Write(csv.Format("WasMapped"));
                            writer.Write(csv.Format("WasDiscovered"));
                        }
                        if (ShowStars)
                        {
                            writer.Write(csv.Format("X"));
                            writer.Write(csv.Format("Y"));
                            writer.Write(csv.Format("Z"));
                            writer.Write(csv.Format("Star Type"));
                            writer.Write(csv.Format("Star Class"));
                            writer.Write(csv.Format("StellarMass"));
                            writer.Write(csv.Format("AbsoluteMagnitude"));
                            writer.Write(csv.Format("Age MY"));
                            writer.Write(csv.Format("Luminosity"));
                        }
                        writer.Write(csv.Format("Radius"));
                        writer.Write(csv.Format("RotationPeriod"));
                        writer.Write(csv.Format("SurfaceTemperature"));
                        if (showMapped )
                        {
                            writer.Write(csv.Format("Mapped"));
                            writer.Write(csv.Format("EfficientlyMapped"));
                        }
                        if (ShowPlanets)
                        {
                            writer.Write(csv.Format("TidalLock"));
                            writer.Write(csv.Format("TerraformState"));
                            writer.Write(csv.Format("PlanetClass"));
                            writer.Write(csv.Format("Atmosphere"));
                            writer.Write(csv.Format("Iron"));
                            writer.Write(csv.Format("Silicates"));
                            writer.Write(csv.Format("SulphurDioxide"));
                            writer.Write(csv.Format("CarbonDioxide"));
                            writer.Write(csv.Format("Nitrogen"));
                            writer.Write(csv.Format("Oxygen"));
                            writer.Write(csv.Format("Water"));
                            writer.Write(csv.Format("Argon"));
                            writer.Write(csv.Format("Ammonia"));
                            writer.Write(csv.Format("Methane"));
                            writer.Write(csv.Format("Hydrogen"));
                            writer.Write(csv.Format("Helium"));
                            writer.Write(csv.Format("Volcanism"));
                            writer.Write(csv.Format("SurfaceGravity"));
                            writer.Write(csv.Format("SurfacePressure"));
                            writer.Write(csv.Format("Landable"));
                            writer.Write(csv.Format("EarthMasses"));
                            writer.Write(csv.Format("IcePercent"));
                            writer.Write(csv.Format("RockPercent"));
                            writer.Write(csv.Format("MetalPercent"));
                        }
                        // Common orbital param
                        writer.Write(csv.Format("SemiMajorAxis"));
                        writer.Write(csv.Format("Eccentricity"));
                        writer.Write(csv.Format("OrbitalInclination"));
                        writer.Write(csv.Format("Periapsis"));
                        writer.Write(csv.Format("OrbitalPeriod"));
                        writer.Write(csv.Format("AxialTilt"));


                        if (ShowPlanets)
                        {
                            writer.Write(csv.Format("Carbon"));
                            writer.Write(csv.Format("Iron"));
                            writer.Write(csv.Format("Nickel"));
                            writer.Write(csv.Format("Phosphorus"));
                            writer.Write(csv.Format("Sulphur"));
                            writer.Write(csv.Format("Arsenic"));
                            writer.Write(csv.Format("Chromium"));
                            writer.Write(csv.Format("Germanium"));
                            writer.Write(csv.Format("Manganese"));
                            writer.Write(csv.Format("Selenium"));
                            writer.Write(csv.Format("Vanadium"));
                            writer.Write(csv.Format("Zinc"));
                            writer.Write(csv.Format("Zirconium"));
                            writer.Write(csv.Format("Cadmium"));
                            writer.Write(csv.Format("Mercury"));
                            writer.Write(csv.Format("Molybdenum"));
                            writer.Write(csv.Format("Niobium"));
                            writer.Write(csv.Format("Tin"));
                            writer.Write(csv.Format("Tungsten"));
                            writer.Write(csv.Format("Antimony"));
                            writer.Write(csv.Format("Polonium"));
                            writer.Write(csv.Format("Ruthenium"));
                            writer.Write(csv.Format("Technetium"));
                            writer.Write(csv.Format("Tellurium"));
                            writer.Write(csv.Format("Yttrium"));
                        }

                        writer.WriteLine();
                    }

                    foreach (JournalScan scan in scans)
                    {
                        if (scan.BodyName.Equals("Nuemai SI-B d13-0 4"))
                        {

                        }
                        if ((scan.IsPlanet && ShowPlanets) || (scan.IsStar && ShowStars) || (scan.IsBeltCluster && ShowBeltClusters))
                        {
                            writer.Write(csv.Format(EDDConfig.Instance.ConvertTimeToSelectedFromUTC(scan.EventTimeUTC)));
                            writer.Write(csv.Format(scan.BodyName));
                            writer.Write(csv.Format(scan.EstimatedValue));
                            writer.Write(csv.Format(scan.DistanceFromArrivalLS));
                            if (showMapped)
                            {
                                writer.Write(csv.Format(scan.WasMapped));
                                writer.Write(csv.Format(scan.WasDiscovered));
                            }
                            if (ShowStars)
                            {
                                string name = scan.StarSystem ?? scan.BodyName;   // early scans did not have starsystem
                                ISystem sys = null;
                                if (name.HasChars())
                                    sys = SystemCache.FindSystem(name, null);       // not doing EDSM Due to no of lookups. Should be in the cache since its one of ours

                                if (sys != null)
                                {
                                    writer.Write(csv.Format(sys.X));
                                    writer.Write(csv.Format(sys.Y));
                                    writer.Write(csv.Format(sys.Z));
                                }
                                else
                                {
                                    writer.Write(csv.Format(""));
                                    writer.Write(csv.Format(""));
                                    writer.Write(csv.Format(""));
                                }

                                writer.Write(csv.Format(scan.StarType));
                                writer.Write(csv.Format(scan.StarClassification));
                                writer.Write(csv.Format((scan.nStellarMass.HasValue) ? scan.nStellarMass.Value : 0));
                                writer.Write(csv.Format((scan.nAbsoluteMagnitude.HasValue) ? scan.nAbsoluteMagnitude.Value : 0));
                                writer.Write(csv.Format((scan.nAge.HasValue) ? scan.nAge.Value : 0));
                                writer.Write(csv.Format(scan.Luminosity));
                            }

                            writer.Write(csv.Format(scan.nRadius.HasValue ? scan.nRadius.Value : 0));
                            writer.Write(csv.Format(scan.nRotationPeriod.HasValue ? scan.nRotationPeriod.Value : 0));
                            writer.Write(csv.Format(scan.nSurfaceTemperature.HasValue ? scan.nSurfaceTemperature.Value : 0));

                            if (showMapped)
                            {
                                writer.Write(csv.Format(scan.Mapped));
                                writer.Write(csv.Format(scan.EfficientMapped));
                            }

                            if (ShowPlanets)
                            {
                                writer.Write(csv.Format(scan.nTidalLock.HasValue ? scan.nTidalLock.Value : false));
                                writer.Write(csv.Format((scan.TerraformState != null) ? scan.TerraformState : ""));
                                writer.Write(csv.Format(scan?.PlanetClass ?? ""));
                                writer.Write(csv.Format(scan?.Atmosphere ?? ""));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Iron")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Silicates")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("SulphurDioxide")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("CarbonDioxide")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Nitrogen")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Oxygen")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Water")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Argon")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Ammonia")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Methane")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Hydrogen")));
                                writer.Write(csv.Format(scan.GetAtmosphereComponent("Helium")));
                                writer.Write(csv.Format(scan.Volcanism.HasChars() ? scan.Volcanism : ""));
                                writer.Write(csv.Format(scan.nSurfaceGravity.HasValue ? scan.nSurfaceGravity.Value : 0));
                                writer.Write(csv.Format(scan.nSurfacePressure.HasValue ? scan.nSurfacePressure.Value : 0));
                                writer.Write(csv.Format(scan.nLandable.HasValue ? scan.nLandable.Value : false));
                                writer.Write(csv.Format((scan.nMassEM.HasValue) ? scan.nMassEM.Value : 0));
                                writer.Write(csv.Format(scan.GetCompositionPercent("Ice")));
                                writer.Write(csv.Format(scan.GetCompositionPercent("Rock")));
                                writer.Write(csv.Format(scan.GetCompositionPercent("Metal")));
                            }
                            // Common orbital param
                            writer.Write(csv.Format(scan.nSemiMajorAxis.HasValue ? scan.nSemiMajorAxis.Value : 0));
                            writer.Write(csv.Format(scan.nEccentricity.HasValue ? scan.nEccentricity.Value : 0));
                            writer.Write(csv.Format(scan.nOrbitalInclination.HasValue ? scan.nOrbitalInclination.Value : 0));
                            writer.Write(csv.Format(scan.nPeriapsis.HasValue ? scan.nPeriapsis.Value : 0));
                            writer.Write(csv.Format(scan.nOrbitalPeriod.HasValue ? scan.nOrbitalPeriod.Value : 0));
                            writer.Write(csv.Format(scan.nAxialTilt.HasValue ? scan.nAxialTilt : null));

                            if (ShowPlanets)
                            {
                                writer.Write(csv.Format(scan.GetMaterial("Carbon")));
                                writer.Write(csv.Format(scan.GetMaterial("Iron")));
                                writer.Write(csv.Format(scan.GetMaterial("Nickel")));
                                writer.Write(csv.Format(scan.GetMaterial("Phosphorus")));
                                writer.Write(csv.Format(scan.GetMaterial("Sulphur")));
                                writer.Write(csv.Format(scan.GetMaterial("Arsenic")));
                                writer.Write(csv.Format(scan.GetMaterial("Chromium")));
                                writer.Write(csv.Format(scan.GetMaterial("Germanium")));
                                writer.Write(csv.Format(scan.GetMaterial("Manganese")));
                                writer.Write(csv.Format(scan.GetMaterial("Selenium")));
                                writer.Write(csv.Format(scan.GetMaterial("Vanadium")));
                                writer.Write(csv.Format(scan.GetMaterial("Zinc")));
                                writer.Write(csv.Format(scan.GetMaterial("Zirconium")));
                                writer.Write(csv.Format(scan.GetMaterial("Cadmium")));
                                writer.Write(csv.Format(scan.GetMaterial("Mercury")));
                                writer.Write(csv.Format(scan.GetMaterial("Molybdenum")));
                                writer.Write(csv.Format(scan.GetMaterial("Niobium")));
                                writer.Write(csv.Format(scan.GetMaterial("Tin")));
                                writer.Write(csv.Format(scan.GetMaterial("Tungsten")));
                                writer.Write(csv.Format(scan.GetMaterial("Antimony")));
                                writer.Write(csv.Format(scan.GetMaterial("Polonium")));
                                writer.Write(csv.Format(scan.GetMaterial("Ruthenium")));
                                writer.Write(csv.Format(scan.GetMaterial("Technetium")));
                                writer.Write(csv.Format(scan.GetMaterial("Tellurium")));
                                writer.Write(csv.Format(scan.GetMaterial("Yttrium")));
                            }
                            writer.WriteLine();
                        }
                    }

                    writer.Close();

                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception {ex}");
                return false;
            }
        }
    }
}
