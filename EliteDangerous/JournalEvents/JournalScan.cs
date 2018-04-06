/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    //When Written: detailed discovery scan of a star, planet or moon
    //Parameters(star)
    //•	Bodyname: name of body
    //•	DistanceFromArrivalLS
    //•	StarType: Stellar classification (for a star)
    //•	StellarMass: mass as multiple of Sol’s mass
    //•	Radius
    //•	AbsoluteMagnitude
    //•	OrbitalPeriod (seconds)
    //•	RotationPeriod (seconds)
    //•	Rings: [ array ] - if present
    //
    //Parameters(Planet/Moon)
    //•	Bodyname: name of body
    //•	DistanceFromArrivalLS
    //•	TidalLock: 1 if tidally locked
    //•	TerraformState: Terraformable, Terraforming, Terraformed, or null
    //•	PlanetClass
    //•	Atmosphere
    //•	Volcanism
    //•	SurfaceGravity
    //•	SurfaceTemperature
    //•	SurfacePressure
    //•	Landable: true (if landable)
    //•	Materials: JSON object with material names and percentage occurrence
    //•	OrbitalPeriod (seconds)
    //•	RotationPeriod (seconds)
    //•	Rings [ array of info ] - if rings present
    //•	ReserveLevel: (Pristine/Major/Common/Low/Depleted) – if rings present
    //
    // Rings properties
    //•	Name
    //•	RingClass
    //•	MassMT - ie in megatons
    //•	InnerRad
    //•	OuterRad
    //
    [JournalEntryType(JournalTypeEnum.Scan)]
    public class JournalScan : JournalEntry
    {
        public bool IsStar { get { return !String.IsNullOrEmpty(StarType); } }
        public string BodyDesignation { get; set; }

        // ALL
        public string ScanType { get; set; }                        // 3.0 scan type  Basic, Detailed, NavBeacon, NavBeaconDetail or empty for older ones
        public string BodyName { get; set; }                        // direct (meaning no translation)
        public int? BodyID { get; set; }                            // direct
        public double DistanceFromArrivalLS { get; set; }           // direct
        public double? nRotationPeriod { get; set; }                // direct
        public double? nSurfaceTemperature { get; set; }            // direct
        public double? nRadius { get; set; }                        // direct
        public bool HasRings { get { return Rings != null && Rings.Length > 0; } }
        public StarPlanetRing[] Rings { get; set; }
        public int EstimatedValue { get; set; }
        public List<BodyParent> Parents { get; set; }

        // STAR
        public string StarType { get; set; }                        // null if no StarType, direct from journal, K, A, B etc
        public EDStar StarTypeID { get; }                           // star type -> identifier
        public string StarTypeText { get { return IsStar ? GetStarTypeName() : ""; } }   // Long form star name, from StarTypeID
        public double? nStellarMass { get; set; }                   // direct
        public double? nAbsoluteMagnitude { get; set; }             // direct
        public string Luminosity { get; set; }
        public double? nAge { get; set; }                           // direct
        public double? HabitableZoneInner { get; set; }             // calculated
        public double? HabitableZoneOuter { get; set; }             // calculated

        // All orbiting bodies (Stars/Planets), not main star
        public double? nSemiMajorAxis;                              // direct
        public double? nEccentricity;                               // direct
        public double? nOrbitalInclination;                         // direct
        public double? nPeriapsis;                                  // direct
        public double? nOrbitalPeriod { get; set; }                 // direct
        public double? nAxialTilt { get; set; }                 // direct, radians

        // Planets
        public string PlanetClass { get; set; }                     // planet class, direct
        public EDPlanet PlanetTypeID { get; }                       // planet class -> ID
        public bool? nTidalLock { get; set; }                       // direct
        public string TerraformState { get; set; }                  // direct, can be empty or a string
        public bool Terraformable { get { return TerraformState != null && TerraformState.ToLower().Equals("terraformable"); } }
        public string Atmosphere { get; set; }                      // direct from journal, if not there or blank, tries AtmosphereType (Earthlikes)
        public EDAtmosphereType AtmosphereID { get; }               // Atmosphere -> ID (Ammonia, Carbon etc)
        public EDAtmosphereProperty AtmosphereProperty;             // Atomsphere -> Property (None, Rich, Thick , Thin, Hot)
        public bool HasAtmosphericComposition { get { return AtmosphereComposition != null && AtmosphereComposition.Any(); } }
        public Dictionary<string, double> AtmosphereComposition { get; set; }
        public string Volcanism { get; set; }                       // direct from journal
        public EDVolcanism VolcanismID { get; }                     // Volcanism -> ID (Water_Magma, Nitrogen_Magma etc)
        public bool HasMeaningfulVolcanism { get { return VolcanismID != EDVolcanism.None && VolcanismID != EDVolcanism.Unknown; } }
        public EDVolcanismProperty VolcanismProperty;               // Volcanism -> Property (None, Major, Minor)
        public double? nSurfaceGravity { get; set; }                // direct
        public double? nSurfacePressure { get; set; }               // direct
        public bool? nLandable { get; set; }                        // direct
        public bool IsLandable { get { return nLandable.HasValue && nLandable.Value; } }
        public double? nMassEM { get; set; }                        // direct, not in description of event, mass in EMs
        public bool HasMaterials { get { return Materials != null && Materials.Any(); } }
        public Dictionary<string, double> Materials { get; set; }
        public bool IsEDSMBody { get; private set; }

        public EDReserve ReserveLevel { get; set; }
        public string ReserveLevelStr
        {
            get
            {
                return ReserveLevel.ToString();
            }
            set
            {
                ReserveLevel = Bodies.ReserveStr2Enum(value);
            }
        }

        public const double solarRadius_m = 695700000;
        public const double oneLS_m = 299792458;
        public const double oneAU_m = 149597870700;
        public const double oneAU_LS = oneAU_m / oneLS_m;
        public const double oneDay_s = 86400;
        public const double oneMoon_MT = 73420000000000;
        public const double oneAtmosphere_Pa = 101325;
        public const double oneGee_m_s2 = 9.80665;

        public class StarPlanetRing
        {
            public string Name;     // may be null
            public string RingClass;    // may be null
            public double MassMT;
            public double InnerRad;
            public double OuterRad;

            public string RingInformation(double scale = 1, string scaletype = " MT", bool parentIsStar = false)
            {
                StringBuilder scanText = new StringBuilder();
                scanText.AppendFormat("  {0} ({1})\n", Name.Alt("Unknown"), DisplayStringFromRingClass(RingClass));
                scanText.AppendFormat("  Mass: {0}{1}\n", (MassMT * scale).ToString("N4"), scaletype);
                if (parentIsStar && InnerRad > 3000000)
                {
                    scanText.AppendFormat("  Inner Radius: {0:0.00}ls\n", (InnerRad / oneLS_m));
                    scanText.AppendFormat("  Outer Radius: {0:0.00}ls\n", (OuterRad / oneLS_m));
                }
                else
                {
                    scanText.AppendFormat("  Inner Radius: {0}km\n", (InnerRad / 1000).ToString("N0"));
                    scanText.AppendFormat("  Outer Radius: {0}km\n", (OuterRad / 1000).ToString("N0"));
                }
                return scanText.ToNullSafeString();
            }

            public string RingInformationMoons(bool parentIsStar = false)
            {
                return RingInformation(1 / oneMoon_MT, " Moons", parentIsStar);
            }

            public static string DisplayStringFromRingClass(string ringClass)
            {
                switch (ringClass)
                {
                    case null:
                        return "Unknown";
                    case "eRingClass_Icy":
                        return "Icy";
                    case "eRingClass_Rocky":
                        return "Rocky";
                    case "eRingClass_MetalRich":
                        return "Metal Rich";
                    case "eRingClass_Metalic":
                        return "Metallic";
                    case "eRingClass_RockyIce":
                        return "Rocky Ice";
                    default:
                        return ringClass.Replace("eRingClass_", "");
                }
            }
        }

        public class BodyParent
        {
            public string Type;
            public int BodyID;
        }

        public JournalScan(JObject evt) : base(evt, JournalTypeEnum.Scan)
        {
            ScanType = evt["ScanType"].Str();
            BodyName = evt["BodyName"].Str();
            BodyID = evt["BodyID"].IntNull();
            StarType = evt["StarType"].StrNull();

            DistanceFromArrivalLS = evt["DistanceFromArrivalLS"].Double();

            nAge = evt["Age_MY"].DoubleNull();
            nStellarMass = evt["StellarMass"].DoubleNull();
            nRadius = evt["Radius"].DoubleNull();
            nAbsoluteMagnitude = evt["AbsoluteMagnitude"].DoubleNull();
            Luminosity = evt["Luminosity"].StrNull();

            nRotationPeriod = evt["RotationPeriod"].DoubleNull();

            nOrbitalPeriod = evt["OrbitalPeriod"].DoubleNull();
            nSemiMajorAxis = evt["SemiMajorAxis"].DoubleNull();
            nEccentricity = evt["Eccentricity"].DoubleNull();
            nOrbitalInclination = evt["OrbitalInclination"].DoubleNull();
            nPeriapsis = evt["Periapsis"].DoubleNull();
            nAxialTilt = evt["AxialTilt"].DoubleNull();

            Rings = evt["Rings"]?.ToObjectProtected<StarPlanetRing[]>(); // may be Null

            nTidalLock = evt["TidalLock"].Bool();
            TerraformState = evt["TerraformState"].StrNull();
            if (TerraformState != null && TerraformState.Equals("Not Terraformable", StringComparison.InvariantCultureIgnoreCase)) // EDSM returns this, normalise to journal
                TerraformState = String.Empty;
            PlanetClass = evt["PlanetClass"].StrNull();

            Atmosphere = evt["Atmosphere"].StrNull();
            if (Atmosphere == null || Atmosphere.Length == 0)             // Earthlikes appear to have empty atmospheres but AtmosphereType
                Atmosphere = evt["AtmosphereType"].StrNull();
            if (Atmosphere != null)
                Atmosphere = Atmosphere.SplitCapsWordFull();
            
            AtmosphereID = Bodies.AtmosphereStr2Enum(Atmosphere, out AtmosphereProperty);
            Volcanism = evt["Volcanism"].StrNull();
            VolcanismID = Bodies.VolcanismStr2Enum(Volcanism, out VolcanismProperty);
            nMassEM = evt["MassEM"].DoubleNull();
            nSurfaceGravity = evt["SurfaceGravity"].DoubleNull();
            nSurfaceTemperature = evt["SurfaceTemperature"].DoubleNull();
            nSurfacePressure = evt["SurfacePressure"].DoubleNull();
            nLandable = evt["Landable"].BoolNull();

            ReserveLevelStr = evt["ReserveLevel"].Str();

            if (IsStar)
            {
                StarTypeID = Bodies.StarStr2Enum(StarType);

                if (nRadius.HasValue && nSurfaceTemperature.HasValue)
                {
                    HabitableZoneInner = DistanceForBlackBodyTemperature(315);
                    HabitableZoneOuter = DistanceForBlackBodyTemperature(223);
                }
            }
            else if (PlanetClass != null)
            {
                PlanetTypeID = Bodies.PlanetStr2Enum(PlanetClass);
                // Fix naming to standard and fix case..
                PlanetClass = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                        ToTitleCase(PlanetClass.ToLower()).Replace("Ii ", "II ").Replace("Iv ", "IV ").Replace("Iii ", "III ");
            }
            else
            {
                PlanetTypeID = EDPlanet.Unknown;
            }

            JToken mats = (JToken)evt["Materials"];

            if (mats != null)
            {
                if (mats.Type == JTokenType.Object)
                {
                    Materials = mats?.ToObjectProtected<Dictionary<string, double>>();
                }
                else
                {
                    Materials = new Dictionary<string, double>();
                    foreach (JObject jo in mats)
                    {
                        Materials[(string)jo["Name"]] = jo["Percent"].Double();
                    }
                }
            }

            JToken atmos = (JToken)evt["AtmosphereComposition"];

            if (atmos != null)
            {
                if (atmos.Type == JTokenType.Object)
                {
                    AtmosphereComposition = atmos?.ToObjectProtected<Dictionary<string, double>>();
                }
                else
                {
                    AtmosphereComposition = new Dictionary<string, double>();
                    foreach (JObject jo in atmos)
                    {
                        AtmosphereComposition[(string)jo["Name"]] = jo["Percent"].Double();
                    }
                }
            }

            IsEDSMBody = evt["EDDFromEDSMBodie"].Bool(false);

            EstimatedValue = CalculateEstimatedValue();

            if (evt["Parents"] != null)
            {
                Parents = new List<BodyParent>();
                foreach (JObject parent in evt["Parents"])
                {
                    JProperty prop = parent.Properties().First();
                    Parents.Add(new BodyParent { Type = prop.Name, BodyID = prop.Value.Int() });
                }
            }
        }

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = $"Scan of {BodyName}";

            if (IsStar)
            {
                double? r = nRadius;
                if (r.HasValue)
                    r = r / solarRadius_m;

                info = BaseUtils.FieldBuilder.Build("", GetStarTypeName(), "Mass:;SM;0.00", nStellarMass, "Age:;my;0.0", nAge, "Radius:;SR;0.00", r);
            }
            else
            {
                double? r = nRadius;
                if (r.HasValue)
                    r = r / 1000;
                double? g = nSurfaceGravity;
                if (g.HasValue)
                    g = g / oneGee_m_s2;

                info = BaseUtils.FieldBuilder.Build("", PlanetClass, "Mass:;EM;0.00", nMassEM, "<;, Landable", IsLandable, "<;, Terraformable", TerraformState == "Terraformable", "", Atmosphere, "Gravity:;G;0.0", g, "Radius:;km;0", r);
            }

            detailed = DisplayString(0, false);

            if (info.IsEmpty())
            {
                info = detailed;
                detailed = "";
            }
        }


        private void ConvertFromEDSMBodies()
        {
            EventTimeUTC = DateTime.UtcNow;
            throw new NotImplementedException();
        }

        public string DisplayString(int indent = 0, bool includefront = true)
        {
            string inds = new string(' ', indent);

            StringBuilder scanText = new StringBuilder();

            scanText.Append(inds);

            if (includefront)
            {
                scanText.AppendFormat("{0}\n\n", BodyName);

                if (IsStar)
                {
                    scanText.AppendFormat(GetStarTypeName());
                }
                else if (PlanetClass != null)
                {
                    scanText.AppendFormat("{0}", PlanetClass);

                    if (!PlanetClass.ToLower().Contains("gas"))
                    {
                        scanText.AppendFormat((Atmosphere == null || Atmosphere == String.Empty) ? ", No Atmosphere" : (", " + Atmosphere));
                    }
                }

                if (HasAtmosphericComposition)
                {
                    scanText.Append("\n" + DisplayAtmosphere(2) + "\n");
                }

                if (IsLandable)
                    scanText.AppendFormat(", Landable");

                scanText.AppendFormat("\n");

                if (nAge.HasValue)
                    scanText.AppendFormat("Age: {0} million years\n", nAge.Value.ToString("N0"));

                if (nStellarMass.HasValue)
                    scanText.AppendFormat("Solar Masses: {0:0.00}\n", nStellarMass.Value);

                if (nMassEM.HasValue)
                    scanText.AppendFormat("Earth Masses: {0:0.0000}\n", nMassEM.Value);

                if (nRadius.HasValue)
                {
                    if (IsStar)
                        scanText.AppendFormat("Solar Radius: {0:0.00} Sols\n", (nRadius.Value / solarRadius_m));
                    else
                        scanText.AppendFormat("Body Radius: {0:0.00}km\n", (nRadius.Value / 1000));
                }
            }

            if (nSurfaceTemperature.HasValue)
                scanText.AppendFormat("Surface Temp: {0}K\n", nSurfaceTemperature.Value.ToString("N0"));

            if (Luminosity != null)
                scanText.AppendFormat("Luminosity: {0}\n", Luminosity);

            if (nSurfaceGravity.HasValue)
                scanText.AppendFormat("Gravity: {0:0.0}g\n", nSurfaceGravity.Value / oneGee_m_s2);

            if (nSurfacePressure.HasValue && nSurfacePressure.Value > 0.00 && !PlanetClass.ToLower().Contains("gas"))
                if (nSurfacePressure.Value > 1000) { scanText.AppendFormat("Surface Pressure: {0} Atmospheres\n", (nSurfacePressure.Value / oneAtmosphere_Pa).ToString("N2")); }
                else { { scanText.AppendFormat("Surface Pressure: {0} Pa\n", (nSurfacePressure.Value).ToString("N2")); } }

            if (Volcanism != null)
                scanText.AppendFormat("Volcanism: {0}\n", Volcanism == String.Empty ? "No Volcanism" : System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                                                                            ToTitleCase(Volcanism.ToLower()));

            if (DistanceFromArrivalLS > 0)
                scanText.AppendFormat("Distance from Arrival Point {0:N1}ls\n", DistanceFromArrivalLS);

            if (nOrbitalPeriod.HasValue && nOrbitalPeriod > 0)
                scanText.AppendFormat("Orbital Period: {0} days\n", (nOrbitalPeriod.Value / oneDay_s).ToString("N1"));

            if (nSemiMajorAxis.HasValue)
            {
                if (IsStar || nSemiMajorAxis.Value > oneAU_m / 10)
                    scanText.AppendFormat("Semi Major Axis: {0:0.00}AU\n", (nSemiMajorAxis.Value / oneAU_m));
                else
                    scanText.AppendFormat("Semi Major Axis: {0}km\n", (nSemiMajorAxis.Value / 1000).ToString("N1"));
            }

            if (nEccentricity.HasValue)
                scanText.AppendFormat("Orbital Eccentricity: {0:0.000}\n", nEccentricity.Value);

            if (nOrbitalInclination.HasValue)
                scanText.AppendFormat("Orbital Inclination: {0:0.000}°\n", nOrbitalInclination.Value);

            if (nPeriapsis.HasValue)
                scanText.AppendFormat("Arg Of Periapsis: {0:0.000}°\n", nPeriapsis.Value);

            if (nAbsoluteMagnitude.HasValue)
                scanText.AppendFormat("Absolute Magnitude: {0:0.00}\n", nAbsoluteMagnitude.Value);

            if (nAxialTilt.HasValue)
                scanText.AppendFormat("Axial tilt: {0:0.00}°\n", nAxialTilt.Value*180.0/Math.PI);
            
            if (nRotationPeriod.HasValue)
                scanText.AppendFormat("Rotation Period: {0} days\n", (nRotationPeriod.Value / oneDay_s).ToString("N1"));

            if (nTidalLock.HasValue && nTidalLock.Value)
                scanText.Append("Tidally locked\n");

            if (Terraformable)
                scanText.Append("Candidate for terraforming\n");

            if (HasRings)
            {
                scanText.Append("\n");
                if (IsStar)
                {
                    scanText.AppendFormat("Belt{0}", Rings.Count() == 1 ? ":" : "s:");
                    for (int i = 0; i < Rings.Length; i++)
                    {
                        if (Rings[i].MassMT > (oneMoon_MT / 10000))
                        {
                            scanText.Append("\n" + RingInformation(i, 1.0 / oneMoon_MT, " Moons"));
                        }
                        else
                        {
                            scanText.Append("\n" + RingInformation(i));
                        }
                    }
                }
                else
                {
                    scanText.AppendFormat("Ring{0}", Rings.Count() == 1 ? ":" : "s:");

                    for (int i = 0; i < Rings.Length; i++)
                        scanText.Append("\n" + RingInformation(i));
                }
            }

            if (HasMaterials)
            {
                scanText.Append("\n" + DisplayMaterials(2) + "\n");
            }

            string habzonestring = HabZoneString();
            if (habzonestring != null)
                scanText.Append("\n" + habzonestring);

            if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                scanText.Remove(scanText.Length - 1, 1);

            if (EstimatedValue > 0)
                scanText.AppendFormat("\nEstimated value: {0:N0}", EstimatedValue);

            return scanText.ToNullSafeString().Replace("\n", "\n" + inds);
        }

        public string GetHabZoneStringLs()
        {
            if (IsStar && HabitableZoneInner.HasValue && HabitableZoneOuter.HasValue)
            {
                return $"{HabitableZoneInner:N0}-{HabitableZoneOuter:N0}ls";
            }
            else
            {
                return string.Empty;
            }
        }

        public string HabZoneString()
        {
            if (IsStar && HabitableZoneInner.HasValue && HabitableZoneOuter.HasValue)
            {
                StringBuilder habZone = new StringBuilder();
                habZone.AppendFormat("Habitable Zone Approx. {0} ({1}-{2} AU)\n", GetHabZoneStringLs(),
                                                                                  (HabitableZoneInner.Value / oneAU_LS).ToString("N2"), (HabitableZoneOuter.Value / oneAU_LS).ToString("N2"));
                if (nSemiMajorAxis.HasValue && nSemiMajorAxis.Value > 0)
                    habZone.AppendFormat(" (Others stars not considered)\n");

                return habZone.ToNullSafeString();
            }
            else
                return null;
        }

        public string DisplayMaterials(int indent = 0)
        {
            StringBuilder scanText = new StringBuilder();
            string indents = new string(' ', indent);

            scanText.Append("Materials:\n");
            foreach (KeyValuePair<string, double> mat in Materials)
            {
                MaterialCommodityDB mc = MaterialCommodityDB.GetCachedMaterial(mat.Key);
                if (mc != null)
                    scanText.AppendFormat(indents + "{0} ({1}) {2} {3}%\n", mc.name, mc.shortname, mc.type, mat.Value.ToString("N1"));
                else
                    scanText.AppendFormat(indents + "{0} {1}%\n", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(mat.Key.ToLower()),
                                                                mat.Value.ToString("N1"));
            }

            if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                scanText.Remove(scanText.Length - 1, 1);

            return scanText.ToNullSafeString();
        }

        public string DisplayAtmosphere(int indent = 0)
        {
            StringBuilder scanText = new StringBuilder();
            string indents = new string(' ', indent);

            scanText.Append(indents + "Atmospheric Composition:\n");
            foreach (KeyValuePair<string, double> comp in AtmosphereComposition)
            {
                scanText.AppendFormat(indents + indents + "{0} - {1}%\n", comp.Key, comp.Value.ToString("N2"));
            }

            if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                scanText.Remove(scanText.Length - 1, 1);

            return scanText.ToNullSafeString();
        }

        public string RingInformationMoons(int ringno)
        {
            return RingInformation(ringno, 1 / oneMoon_MT, " Moons");
        }

        public string RingInformation(int ringno, double scale = 1, string scaletype = " MT")
        {
            StarPlanetRing ring = Rings[ringno];
            return ring.RingInformation(scale, scaletype, IsStar);
        }

        public string GetStarTypeName()           // give description to star class
        {
            return Bodies.StarName(StarTypeID);
        }

        public System.Drawing.Image GetStarTypeImage()           // give image and description to star class
        {
            return Bodies.GetStarTypeImage(StarTypeID);
        }

        static public System.Drawing.Image GetStarImageNotScanned()
        {
            return Bodies.GetStarTypeImage(EDStar.Unknown);
        }

        public System.Drawing.Image GetPlanetClassImage()
        {
            if (PlanetClass == null)
            {
                return GetPlanetImageNotScanned();
            }

            EDPlanet planetclass = PlanetTypeID;

            if (planetclass == EDPlanet.High_metal_content_body || planetclass == EDPlanet.Metal_rich_body)
            {
                if (AtmosphereProperty == (EDAtmosphereProperty.Hot | EDAtmosphereProperty.Thick))
                {
                    planetclass = EDPlanet.High_metal_content_body_hot_thick;
                }
                else if (planetclass == EDPlanet.High_metal_content_body && nSurfaceTemperature > 700)
                {
                    planetclass = EDPlanet.High_metal_content_body_700;
                }
                else if (planetclass == EDPlanet.High_metal_content_body && nSurfaceTemperature > 250)
                {
                    planetclass = EDPlanet.High_metal_content_body_250;
                }
            }

            return Bodies.GetPlanetClassImage(planetclass);
        }

        static public System.Drawing.Image GetPlanetImageNotScanned()
        {
            return Bodies.GetPlanetClassImage(EDPlanet.Unknown);
        }

        static public System.Drawing.Image GetMoonImageNotScanned()
        {
            return Bodies.GetPlanetClassImage(EDPlanet.Unknown);
        }

        public double GetMaterial(string v)
        {
            if (Materials == null)
                return 0.0;

            if (!Materials.ContainsKey(v.ToLower()))
                return 0.0;

            return Materials[v.ToLower()];
        }

        public double? GetAtmosphereComponent(string c)
        {
            if (!HasAtmosphericComposition)
                return null;

            if (!AtmosphereComposition.ContainsKey(c))
                return 0.0;

            return AtmosphereComposition[c];

        }

        public bool IsStarNameRelated(string starname, string designation = null)
        {
            if (designation == null)
            {
                designation = BodyName;
            }

            if (designation.Length >= starname.Length)
            {
                string s = designation.Substring(0, starname.Length);
                return starname.Equals(s, StringComparison.InvariantCultureIgnoreCase);
            }
            else
                return false;
        }

        public string IsStarNameRelatedReturnRest(string starname)          // null if not related, else rest of string
        {
            string designation = BodyDesignation ?? BodyName;
            if (designation.Length >= starname.Length)
            {
                string s = designation.Substring(0, starname.Length);
                if (starname.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                    return designation.Substring(starname.Length).Trim();
            }

            return null;
        }

        // Habitable zone calculations, formulae cribbed from JackieSilver's HabZone Calculator with permission
        private double DistanceForBlackBodyTemperature(double targetTemp)
        {
            double top = Math.Pow(nRadius.Value, 2.0) * Math.Pow(nSurfaceTemperature.Value, 4.0);
            double bottom = 4.0 * Math.Pow(targetTemp, 4.0);
            double radius_metres = Math.Pow(top / bottom, 0.5);
            return radius_metres / oneLS_m;
        }

        private int CalculateEstimatedValue()
        {
            if (EventTimeUTC < new DateTime(2017, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc))
                return EstimatedValueED22();

            double kValue;
            double kBonus = 0;

            if (IsStar)
            {
                switch (StarTypeID)      // http://elite-dangerous.wikia.com/wiki/Explorer
                {
                    // white dwarf
                    case EDStar.D:
                    case EDStar.DA:
                    case EDStar.DAB:
                    case EDStar.DAO:
                    case EDStar.DAZ:
                    case EDStar.DAV:
                    case EDStar.DB:
                    case EDStar.DBZ:
                    case EDStar.DBV:
                    case EDStar.DO:
                    case EDStar.DOV:
                    case EDStar.DQ:
                    case EDStar.DC:
                    case EDStar.DCV:
                    case EDStar.DX:
                        kValue = 33737;
                        break;

                    case EDStar.N:
                    case EDStar.H:
                        kValue = 54309;
                        break;

                    case EDStar.SuperMassiveBlackHole:
                        kValue = 80.5654;
                        break;

                    default:
                        kValue = 2880;
                        break;
                }
                return (int)StarValue(kValue, nStellarMass.HasValue ? nStellarMass.Value : 1.0);
            }
            else if (PlanetClass == null)  //Asteroid belt
                return 0;
            else   // Planet
            {
                switch (PlanetTypeID)      // http://elite-dangerous.wikia.com/wiki/Explorer
                {
                    
                    case EDPlanet.Metal_rich_body:
                        kValue = 52292;
                        if (Terraformable) { kBonus = 245306; }
                        break;
                    case EDPlanet.High_metal_content_body:
                    case EDPlanet.Sudarsky_class_II_gas_giant:
                        kValue = 23168;
                        if (Terraformable) { kBonus = 241607; }
                        break;
                    case EDPlanet.Earthlike_body:
                        kValue = 155581;
                        kBonus = 279088;
                        break;
                    case EDPlanet.Water_world:
                        kValue = 155581;
                        if (Terraformable) { kBonus = 279088; }
                        break;
                    case EDPlanet.Ammonia_world:
                        kValue = 232619;
                        break;
                    case EDPlanet.Sudarsky_class_I_gas_giant:
                        kValue = 3974;
                        break;
                    default:
                        kValue = 720;
                        if (Terraformable) { kBonus = 223971; }
                        break;
                }

                double mass = nMassEM.HasValue ? nMassEM.Value : 1.0;       // some old entries don't have mass, so just presume 1

                int val = (int)PlanetValue(kValue, mass);
                if (Terraformable || PlanetTypeID == EDPlanet.Earthlike_body)
                {
                    val += (int)PlanetValue(kBonus, mass);
                }

                return val;
            }

        }

        private double StarValue(double k, double m)
        {
            return k + (m * k / 66.25);
        }

        private double PlanetValue(double k, double m)
        {
            return k + (3 * k * Math.Pow(m, 0.199977) / 5.3);
        }

        private int EstimatedValueED22()
        {
            if (IsStar)
            {
                switch (StarTypeID)      // http://elite-dangerous.wikia.com/wiki/Explorer
                {
                    case EDStar.O:
                        //low = 3677;
                        //high = 4465;
                        return 4170;

                    case EDStar.B:
                        //low = 2992;
                        //high = 3456;
                        return 3098;

                    case EDStar.A:
                        //low = 2938;
                        //high = 2986;
                        return 2950;

                    case EDStar.F:
                        //low = 2915;
                        //high = 2957;
                        return 2932;

                    case EDStar.G:
                        //low = 2912;
                        //high = 2935;
                        // also have a G8V
                        return 2923;

                    case EDStar.K:
                        //low = 2898;
                        //high = 2923;
                        return 2911;
                    case EDStar.M:
                        //low = 2887;
                        //high = 2905;
                        return 2911;

                    // dwarfs
                    case EDStar.L:
                        //low = 2884;
                        //high = 2890;
                        return 2887;
                    case EDStar.T:
                        //low = 2881;
                        //high = 2885;
                        return 2883;
                    case EDStar.Y:
                        //low = 2880;
                        //high = 2882;
                        return 2881;

                    // proto stars
                    case EDStar.AeBe:    // Herbig
                        //                ??
                        //low = //high = 0;
                        return 2500;
                    case EDStar.TTS:
                        //low = 2881;
                        //high = 2922;
                        return 2900;

                    // wolf rayet
                    case EDStar.W:
                    case EDStar.WN:
                    case EDStar.WNC:
                    case EDStar.WC:
                    case EDStar.WO:
                        //low = //high = 7794;
                        return 7794;

                    // Carbon
                    case EDStar.CS:
                    case EDStar.C:
                    case EDStar.CN:
                    case EDStar.CJ:
                    case EDStar.CHd:
                        //low = //high = 2920;
                        return 2920;

                    case EDStar.MS: //seen in log
                    case EDStar.S:   // seen in log
                                     //                ??
                                     //low = //high = 0;
                        return 2000;


                    // white dwarf
                    case EDStar.D:
                    case EDStar.DA:
                    case EDStar.DAB:
                    case EDStar.DAO:
                    case EDStar.DAZ:
                    case EDStar.DAV:
                    case EDStar.DB:
                    case EDStar.DBZ:
                    case EDStar.DBV:
                    case EDStar.DO:
                    case EDStar.DOV:
                    case EDStar.DQ:
                    case EDStar.DC:
                    case EDStar.DCV:
                    case EDStar.DX:
                        //low = 25000;
                        //high = 27000;

                        return 26000;

                    case EDStar.N:
                        //low = 43276;
                        //high = 44619;
                        return 43441;

                    case EDStar.H:
                        //low = 44749;
                        //high = 80305;
                        return 61439;

                    case EDStar.X:
                    case EDStar.A_BlueWhiteSuperGiant:
                    case EDStar.F_WhiteSuperGiant:
                    case EDStar.M_RedSuperGiant:
                    case EDStar.M_RedGiant:
                    case EDStar.K_OrangeGiant:
                    case EDStar.RoguePlanet:

                    default:
                        //low = 0;
                        //high = 0;
                        return 2000;
                }
            }
            else   // Planet
            {
                switch (PlanetTypeID)      // http://elite-dangerous.wikia.com/wiki/Explorer
                {
                    case EDPlanet.Icy_body:
                        //low = 792; // (0.0001 EM)
                        //high = 1720; // 89.17
                        return 933; // 0.04

                    case EDPlanet.Rocky_ice_body:
                        //low = 792; // (0.0001 EM)
                        //high = 1720; // 89.17
                        return 933; // 0.04

                    case EDPlanet.Rocky_body:
                        if (TerraformState != null && TerraformState.ToLower().Equals("terraformable"))
                        {
                            //low = 36000;
                            //high = 36500;
                            return 37000;
                        }
                        else
                        {
                            //low = 792; // (0.0001 EM)
                            //high = 1720; // 89.17
                            return 933; // 0.04
                        }
                    case EDPlanet.Metal_rich_body:
                        //low = 9145; // (0.0002 EM)
                        //high = 14562; // (4.03 EM)
                        return 12449; // 0.51 EM
                    case EDPlanet.High_metal_content_body:
                        if (TerraformState != null && TerraformState.ToLower().Equals("terraformable"))
                        {
                            //low = 36000;
                            //high = 54000;
                            return 42000;
                        }
                        else
                        {
                            //low = 4966; // (0.0015 EM)
                            //high = 9632;  // 31.52 EM
                            return 6670; // 0.41
                        }

                    case EDPlanet.Earthlike_body:
                        //low = 65000; // 0.24 EM
                        //high = 71885; // 196.60 EM
                        return 67798; // 0.47 EM

                    case EDPlanet.Water_world:
                        //low = 26589; // (0.09 EM)
                        //high = 43437; // (42.77 EM)
                        return 30492; // (0.82 EM)
                    case EDPlanet.Ammonia_world:
                        //low = 37019; // 0.09 EM
                        //high = 71885; //(196.60 EM)
                        return 40322; // (0.41 EM)
                    case EDPlanet.Sudarsky_class_I_gas_giant:
                        //low = 2472; // (2.30 EM)
                        //high = 4514; // (620.81 EM
                        return 3400;  // 62.93 EM

                    case EDPlanet.Sudarsky_class_II_gas_giant:
                        //low = 8110; // (5.37 EM)
                        //high = 14618; // (949.98 EM)
                        return 12319;  // 260.84 EM

                    case EDPlanet.Sudarsky_class_III_gas_giant:
                        //low = 1368; // (10.16 EM)
                        //high = 2731; // (2926 EM)
                        return 2339; // 990.92 EM

                    case EDPlanet.Sudarsky_class_IV_gas_giant:
                        //low = 2739; //(2984 EM)
                        //high = 2827; // (3697 EM)
                        return 2782; // 3319 em

                    case EDPlanet.Sudarsky_class_V_gas_giant:
                        //low = 2225; // 688.2 EM
                        //high = 2225;
                        return 2225;



                    case EDPlanet.Water_giant:
                    case EDPlanet.Water_giant_with_life:
                    case EDPlanet.Gas_giant_with_water_based_life:
                    case EDPlanet.Gas_giant_with_ammonia_based_life:
                    case EDPlanet.Helium_rich_gas_giant:
                    case EDPlanet.Helium_gas_giant:
                        //low = 0;
                        //high = 0;
                        return 2000;

                    default:
                        //low = 0;
                        //high = 2000;
                        return 0;
                }


            }

        }

    }
}
