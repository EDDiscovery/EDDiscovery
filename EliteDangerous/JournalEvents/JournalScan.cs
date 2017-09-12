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
        public string BodyName { get; set; }                        // direct (meaning no translation)
        public double DistanceFromArrivalLS { get; set; }           // direct
        public double? nRotationPeriod { get; set; }                // direct
        public double? nSurfaceTemperature { get; set; }            // direct
        public double? nRadius { get; set; }                        // direct
        public bool HasRings { get { return Rings != null && Rings.Length > 0; } }
        public StarPlanetRing[] Rings { get; set; }

        // STAR
        public string StarType { get; set; }                        // null if no StarType, direct from journal, K, A, B etc
        public EDStar StarTypeID { get; }                           // star type -> identifier
        public string StarTypeText { get { return IsStar ? GetStarTypeImage().Item2 : ""; } }   // Long form star name, from StarTypeID
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

        // Classes

        public class StarPlanetRing
        {
            public string Name;
            public string RingClass;
            public double MassMT;
            public double InnerRad;
            public double OuterRad;
        }
        
        private const double solarRadius_m = 695700000;
        private const double oneAU_m = 149597870000;
        private const double oneDay_s = 86400;
        private const double oneMoon_MT = 73420000000000;

        public JournalScan(JObject evt) : base(evt, JournalTypeEnum.Scan)
        {
            BodyName = evt["BodyName"].Str();
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


            Rings = evt["Rings"]?.ToObject<StarPlanetRing[]>();

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
                    Materials = mats?.ToObject<Dictionary<string, double>>();
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
                    AtmosphereComposition = atmos?.ToObject<Dictionary<string, double>>();
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
        }

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = $"Scan of {BodyName}";

            if (IsStar)
            {
                double? r = nRadius;
                if (r.HasValue)
                    r = r / solarRadius_m;

                info = BaseUtils.FieldBuilder.Build("", GetStarTypeImage().Item2, "Mass:;SM;0.00", nStellarMass, "Age:;my;0.0", nAge, "Radius:;SR;0.00", r);
            }
            else
            {
                double? r = nRadius;
                if (r.HasValue)
                    r = r / 1000;
                double? g = nSurfaceGravity;
                if (g.HasValue)
                    g = g / 9.8;

                info = BaseUtils.FieldBuilder.Build("", PlanetClass, "Mass:;EM;0.00", nMassEM, "<;, Landable", IsLandable, "<;, Terraformable", TerraformState == "Terraformable", "", Atmosphere, "Gravity:;G;0.0", g, "Radius:;km;0", r);
            }

            detailed = DisplayString(0, false);
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
                    scanText.AppendFormat(GetStarTypeImage().Item2);
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
                scanText.AppendFormat("Gravity: {0:0.0}g\n", nSurfaceGravity.Value / 9.8);

            if (nSurfacePressure.HasValue && nSurfacePressure.Value > 0.00 && !PlanetClass.ToLower().Contains("gas"))
                if (nSurfacePressure.Value > 1000) { scanText.AppendFormat("Surface Pressure: {0} Atmospheres\n", (nSurfacePressure.Value / 100000).ToString("N2")); }
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
                scanText.AppendFormat("Orbital Eccentricity: {0:0.000}°\n", nEccentricity.Value);

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
                        if (Rings[i].MassMT > 7342000000) { scanText.Append("\n" + RingInformation(i, 1.0 / oneMoon_MT, " Moons")); }
                        else { scanText.Append("\n" + RingInformation(i)); }
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

            if (IsStar && HabitableZoneInner.HasValue && HabitableZoneOuter.HasValue)
            {
                StringBuilder habZone = new StringBuilder();
                habZone.AppendFormat("Habitable Zone Approx. {0}-{1}ls ({2}-{3} AU)\n", HabitableZoneInner.Value.ToString("N0"), HabitableZoneOuter.Value.ToString("N0"),
                                                                                             (HabitableZoneInner.Value / 499).ToString("N2"), (HabitableZoneOuter.Value / 499).ToString("N2"));
                if (nSemiMajorAxis.HasValue && nSemiMajorAxis.Value > 0)
                    habZone.AppendFormat(" (This star only, others not considered)\n");
                scanText.Append("\n" + habZone);
            }

            if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                scanText.Remove(scanText.Length - 1, 1);


            int estvalue = EstimatedValue();
            if (estvalue > 0)
                scanText.AppendFormat("\nEstimated value: {0}", estvalue);

            return scanText.ToNullSafeString().Replace("\n", "\n" + inds);
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
            StringBuilder scanText = new StringBuilder();
            scanText.AppendFormat("  {0} ({1})\n", ring.Name, DisplayStringFromRingClass(ring.RingClass));
            scanText.AppendFormat("  Mass: {0}{1}\n", (ring.MassMT * scale).ToString("N4"), scaletype);
            if (IsStar && ring.InnerRad > 3000000)
            {
                scanText.AppendFormat("  Inner Radius: {0:0.00}ls\n", (ring.InnerRad / 300000000));
                scanText.AppendFormat("  Outer Radius: {0:0.00}ls\n", (ring.OuterRad / 300000000));
            }
            else
            {
                scanText.AppendFormat("  Inner Radius: {0}km\n", (ring.InnerRad / 1000).ToString("N0"));
                scanText.AppendFormat("  Outer Radius: {0}km\n", (ring.OuterRad / 1000).ToString("N0"));
            }
            return scanText.ToNullSafeString();
        }

        public string DisplayStringFromRingClass(string ringClass)
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


        public Tuple<System.Drawing.Image, string> GetStarTypeImage()           // give image and description to star class
        {
            System.Drawing.Image ret = EliteDangerous.Properties.Resources.Star_K1IV;

            switch (StarTypeID)       // see journal, section 11.2
            {
                case EDStar.O:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.O, string.Format("Luminous Hot Main Sequence star", StarType));

                case EDStar.B:
                    // also have an B1V
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.B6V_Blueish, string.Format("Luminous Blue Main Sequence star", StarType));

                case EDStar.A:
                    // also have an A3V..
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.A9III_White, string.Format("Bluish-White Main Sequence star", StarType));

                case EDStar.F:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.F5VAB, string.Format("White Main Sequence star", StarType));

                case EDStar.G:
                    // also have a G8V
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.G1IV, string.Format("Yellow Main Sequence star", StarType));

                case EDStar.K:
                    // also have a K0V
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.Star_K1IV, string.Format("Orange Main Sequence {0} star", StarType));
                case EDStar.M:
                    // also have a M1VA
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.M5V, string.Format("Red Main Sequence {0} star", StarType));

                // dwarfs
                case EDStar.L:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.L3V, string.Format("Dark Red Non Main Sequence {0} star", StarType));
                case EDStar.T:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.T4V, string.Format("Methane Dwarf star", StarType));
                case EDStar.Y:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.Y2, string.Format("Brown Dwarf star", StarType));

                // proto stars
                case EDStar.AeBe:    // Herbig
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, "Herbig Ae/Be");
                case EDStar.TTS:     // seen in logs
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, "T Tauri");

                // wolf rayet
                case EDStar.W:
                case EDStar.WN:
                case EDStar.WNC:
                case EDStar.WC:
                case EDStar.WO:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, string.Format("Wolf-Rayet {0} star", StarType));

                // Carbon
                case EDStar.CS:
                case EDStar.C:
                case EDStar.CN:
                case EDStar.CJ:
                case EDStar.CHd:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.C7III, string.Format("Carbon {0} star", StarType));

                case EDStar.MS: //seen in log https://en.wikipedia.org/wiki/S-type_star
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.M5V, string.Format("Intermediate low Zirconium Monoxide Type {0} star", StarType));

                case EDStar.S:   // seen in log, data from http://elite-dangerous.wikia.com/wiki/Stars
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, string.Format("Cool Giant Zirconium Monoxide rich Type {0} star", StarType));

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
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DA6VII_White, string.Format("White Dwarf {0} star", StarType));

                case EDStar.N:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.Neutron_Star, "Neutron Star");

                case EDStar.H:

                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.Black_Hole, "Black Hole");

                case EDStar.X:
                    // currently speculative, not confirmed with actual data... in journal
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, "Exotic");

                // Journal.. really?  need evidence these actually are formatted like this.

                case EDStar.SuperMassiveBlackHole:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.Black_Hole, "Super Massive Black Hole");
                case EDStar.A_BlueWhiteSuperGiant:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.A9III_White, "Blue White Super Giant");
                case EDStar.F_WhiteSuperGiant:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, "F White Super Giant");
                case EDStar.M_RedSuperGiant:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, "M Red Super Giant");
                case EDStar.M_RedGiant:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, "M Red Giant");
                case EDStar.K_OrangeGiant:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, "K Orange Giant");
                case EDStar.RoguePlanet:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, "Rouge Planet");

                default:
                    return new Tuple<System.Drawing.Image, string>(EliteDangerous.Properties.Resources.DefaultStar, string.Format("Class {0} star\n", StarType.Replace("_", " ")));
            }
        }

        static public System.Drawing.Image GetStarImageNotScanned()
        {
            return EliteDangerous.Properties.Resources.Globe_yellow;
        }

        public System.Drawing.Image GetPlanetClassImage()
        {
            if (PlanetClass == null)
            {
                return EliteDangerous.Properties.Resources.Globe;
            }

            string name = PlanetClass.ToLower();

            if (name.Contains("gas"))
            {
                if (name.Contains("helium"))
                    return EliteDangerous.Properties.Resources.Helium_Rich_Gas_Giant1;
                else if (name.Contains("water"))
                    return EliteDangerous.Properties.Resources.Gas_giant_water_based_life_Brown3;
                else if (name.Contains("ammonia"))
                    return EliteDangerous.Properties.Resources.Gas_giant_ammonia_based_life1;
                else if (name.Contains("iv"))
                    return EliteDangerous.Properties.Resources.Class_I_Gas_Giant_Brown2;               // MISSING.
                else if (name.Contains("iii"))
                    return EliteDangerous.Properties.Resources.Class_III_Gas_Giant_Blue3;
                else if (name.Contains("ii"))
                    return EliteDangerous.Properties.Resources.Class_II_Gas_Giant_Sand1;
                else if (name.Contains("v"))
                    return EliteDangerous.Properties.Resources.Class_I_Gas_Giant_Brown2;               // MISSING.
                else
                    return EliteDangerous.Properties.Resources.Class_I_Gas_Giant_Brown2;
            }
            else if (name.Contains("ammonia"))
                return EliteDangerous.Properties.Resources.Ammonia_Brown;      // also have orange.
            else if (name.Contains("earth"))
                return EliteDangerous.Properties.Resources.Earth_Like_Standard;
            else if (name.Contains("ice"))
                return EliteDangerous.Properties.Resources.Rocky_Ice_World_Sol_Titan;
            else if (name.Contains("icy"))
                return EliteDangerous.Properties.Resources.Icy_Body_Greenish1;
            else if (name.Contains("water"))
            {
                if (name.Contains("giant"))
                    return EliteDangerous.Properties.Resources.Water_Giant1;
                else
                    return EliteDangerous.Properties.Resources.Water_World_Poles_Cloudless4;
            }
            else if (name.Contains("metal"))
            {
                if (AtmosphereProperty == (EDAtmosphereProperty.Hot | EDAtmosphereProperty.Thick))
                    return EliteDangerous.Properties.Resources.High_metal_content_world_White3;

                if (name.Contains("rich"))
                    return EliteDangerous.Properties.Resources.metal_rich;
                else if (nSurfaceTemperature > 700)
                    return EliteDangerous.Properties.Resources.High_metal_content_world_Lava1;
                else if (nSurfaceTemperature > 250)
                    return EliteDangerous.Properties.Resources.High_metal_content_world_Mix3;
                else
                    return EliteDangerous.Properties.Resources.High_metal_content_world_Orange8;
            }
            else if (name.Contains("rocky"))
                return EliteDangerous.Properties.Resources.Rocky_Body_Sand2;
            else
                return EliteDangerous.Properties.Resources.Globe;
        }

        static public System.Drawing.Image GetPlanetImageNotScanned()
        {
            return EliteDangerous.Properties.Resources.Globe;
        }

        static public System.Drawing.Image GetMoonImageNotScanned()
        {
            return EliteDangerous.Properties.Resources.Globe;
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

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.scan; } }

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
            return radius_metres / 300000000;
        }

        public int EstimatedValue()
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

                    default:
                        kValue = 2880;
                        break;
                }
                return (int)StarValue(kValue, nStellarMass.Value);
            }
            else   // Planet
            {
                switch (PlanetTypeID)      // http://elite-dangerous.wikia.com/wiki/Explorer
                {
                    
                    case EDPlanet.Metal_rich_body:
                        kValue = 52292;
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

        public int EstimatedValueED22()
        {


            //int low;
            //int high;

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