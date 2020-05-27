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
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Scan)]
    public class JournalScan : JournalEntry, IScanDataChanges
    {
        public bool IsStar { get { return !String.IsNullOrEmpty(StarType); } }
        public bool IsPlanet { get { return !IsStar && !IsBeltCluster; } }
        public bool IsBeltCluster { get { return !IsStar && PlanetClass == null; } }

        public string BodyDesignation { get; set; }

        // ALL
        public string ScanType { get; set; }                        // 3.0 scan type  Basic, Detailed, NavBeacon, NavBeaconDetail, (3.3) AutoScan, or empty for older ones
        public string BodyName { get; set; }                        // direct (meaning no translation)
        public int? BodyID { get; set; }                            // direct
        public string StarSystem { get; set; }                      // direct (3.5)
        public long? SystemAddress { get; set; }                    // direct (3.5)
        public double DistanceFromArrivalLS { get; set; }           // direct
        public string DistanceFromArrivalText { get { return string.Format("{0:0.00}AU ({1:0.0}ls)", DistanceFromArrivalLS / JournalScan.oneAU_LS, DistanceFromArrivalLS); } }
        public double? nRotationPeriod { get; set; }                // direct
        public double? nRotationPeriodDays { get; set; }      
        public double? nSurfaceTemperature { get; set; }            // direct
        public double? nRadius { get; set; }                        // direct
        public double? nRadiusSols { get; set; }
        public double? nRadiusEarths { get; set; }
        public bool HasRings { get { return Rings != null && Rings.Length > 0; } }
        public StarPlanetRing[] Rings { get; set; }

        public List<BodyParent> Parents { get; set; }
        public bool IsOrbitingBaryCentre { get { return Parents?.FirstOrDefault()?.Type == "Null"; } }
        public bool IsOrbitingPlanet { get { return Parents?.FirstOrDefault()?.Type == "Planet"; } }
        public bool IsOrbitingStar { get { return Parents?.FirstOrDefault()?.Type == "Star"; } }

        public bool? WasDiscovered { get; set; }                    // direct, 3.4, indicates whether the body has already been discovered
        public bool IsNotDiscovered { get { return WasDiscovered.HasValue && WasDiscovered == false; } }

        public bool? WasMapped { get; set; }                        // direct, 3.4, indicates whether the body has already been mapped
        public bool IsNotMapped { get { return WasMapped.HasValue && WasMapped == false; } }

        // STAR
        public string StarType { get; set; }                        // null if no StarType, direct from journal, K, A, B etc
        public EDStar StarTypeID { get; }                           // star type -> identifier
        public string StarTypeText { get { return IsStar ? GetStarTypeName() : ""; } }   // Long form star name, from StarTypeID
        public double? nStellarMass { get; set; }                   // direct
        public double? nAbsoluteMagnitude { get; set; }             // direct
        public string Luminosity { get; set; }                      // character string (I,II,.. V)
        public string StarClassification { get { return (StarType ?? "") + (StarSubclass?.ToStringInvariant() ?? "") + (Luminosity ?? ""); } }
        public double? nAge { get; set; }                           // direct, in million years
        public double? HabitableZoneInner { get; set; }             // in AU
        public double? HabitableZoneOuter { get; set; }             // in AU
        public double? MetalRichZoneInner { get; set; }             // in AU etc
        public double? MetalRichZoneOuter { get; set; }
        public double? WaterWrldZoneInner { get; set; }
        public double? WaterWrldZoneOuter { get; set; }
        public double? EarthLikeZoneInner { get; set; }
        public double? EarthLikeZoneOuter { get; set; }
        public double? AmmonWrldZoneInner { get; set; }
        public double? AmmonWrldZoneOuter { get; set; }
        public double? IcyPlanetZoneInner { get; set; }
        public string IcyPlanetZoneOuter { get; set; }              // in AU

        // All orbiting bodies (Stars/Planets), not main star

        public double DistanceAccountingForBarycentre { get { return nSemiMajorAxis.HasValue && !IsOrbitingBaryCentre ? nSemiMajorAxis.Value : DistanceFromArrivalLS * oneLS_m; } } // in metres

        public double? nSemiMajorAxis { get; set; }                 // direct, m
        public double? nSemiMajorAxisAU { get; set; }               // direct
        public string SemiMajorAxisLSKM { get { return nSemiMajorAxis.HasValue ? (nSemiMajorAxis >= oneLS_m / 10 ? ((nSemiMajorAxis.Value / oneLS_m).ToString("N1") + "ls") : ((nSemiMajorAxis.Value / 1000).ToString("N0") + "km")) : ""; } }

        public double? nEccentricity { get; set; }                  // direct
        public double? nOrbitalInclination { get; set; }            // direct
        public double? nPeriapsis { get; set; }                     // direct
        public double? nOrbitalPeriod { get; set; }                 // direct
        public double? nOrbitalPeriodDays { get; set; }
        public double? nAxialTilt { get; set; }                     // direct, radians
        public double? nAxialTiltDeg { get; set; }      
        public int? StarSubclass { get; set; }                      // star Subclass, direct, 3.4

        // Planets
        public string PlanetClass { get; set; }                     // planet class, direct

        public EDPlanet PlanetTypeID { get; }                       // planet class -> ID
        public bool AmmoniaWorld { get { return Bodies.AmmoniaWorld(PlanetTypeID); } }
        public bool Earthlike { get { return Bodies.Earthlike(PlanetTypeID); } }
        public bool WaterWorld { get { return Bodies.WaterWorld(PlanetTypeID); } }
        public bool SudarskyGasGiant { get { return Bodies.SudarskyGasGiant(PlanetTypeID); } }
        public bool GasGiant { get { return Bodies.GasGiant(PlanetTypeID); } }
        public bool WaterGiant { get { return Bodies.WaterGiant(PlanetTypeID); } }
        public bool HeliumGasGiant { get { return Bodies.HeliumGasGiant(PlanetTypeID); } }

        public bool? nTidalLock { get; set; }                       // direct
        public string TerraformState { get; set; }                  // direct, can be empty or a string
        public bool Terraformable { get { return TerraformState != null && TerraformState.ToLowerInvariant().Equals("terraformable"); } }
        public string Atmosphere { get; set; }                      // direct from journal, if not there or blank, tries AtmosphereType (Earthlikes)
        public EDAtmosphereType AtmosphereID { get; }               // Atmosphere -> ID (Ammonia, Carbon etc)
        public EDAtmosphereProperty AtmosphereProperty { get; set; }             // Atomsphere -> Property (None, Rich, Thick , Thin, Hot)
        public bool HasAtmosphericComposition { get { return AtmosphereComposition != null && AtmosphereComposition.Any(); } }
        public Dictionary<string, double> AtmosphereComposition { get; set; }
        public Dictionary<string, double> PlanetComposition { get; set; }
        public bool HasPlanetaryComposition { get { return PlanetComposition != null && PlanetComposition.Any(); } }

        public string Volcanism { get; set; }                       // direct from journal
        public EDVolcanism VolcanismID { get; }                     // Volcanism -> ID (Water_Magma, Nitrogen_Magma etc)
        public bool HasMeaningfulVolcanism { get { return VolcanismID != EDVolcanism.None && VolcanismID != EDVolcanism.Unknown; } }
        public EDVolcanismProperty VolcanismProperty { get; set; }               // Volcanism -> Property (None, Major, Minor)
        public double? nSurfaceGravity { get; set; }                // direct
        public double? nSurfaceGravityG { get; set; }
        public double? nSurfacePressure { get; set; }               // direct
        public double? nSurfacePressureEarth { get; set; }   
        public bool? nLandable { get; set; }                        // direct
        public bool IsLandable { get { return nLandable.HasValue && nLandable.Value; } }
        public double? nMassEM { get; set; }                        // direct, not in description of event, mass in EMs
        public double? nMassMM { get; set; }                        // only if nMassEM is set, moon mass in MMs

        public bool HasMaterials { get { return Materials != null && Materials.Any(); } }
        public Dictionary<string, double> Materials { get; set; }       // fdname and name is the same for materials on planets.  name is lower case
        public bool HasMaterial(string name) { return Materials != null && Materials.ContainsKey(name.ToLowerInvariant()); }
        public string MaterialList { get; private set; }                         // material list, names, comma separ, or null

        public bool IsEDSMBody { get; private set; }
        public string EDSMDiscoveryCommander { get; private set; }      // may be null if not known
        public DateTime EDSMDiscoveryUTC { get; private set; }

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

        public bool Mapped { get { return mapped;}  }                          // affects prices
        public bool EfficientMapped { get { return efficientmapped; } }                          // affects prices

        public int EstimatedValue { get // best guess based on discovered/wasmapped and Mapped flags
            {
                if (EstimatedValueFirstDiscovered > 0)      // for previous scans before 3.3 and stars, these are not set.
                {
                    if (IsNotDiscovered && IsNotMapped && Mapped)
                        return EfficientMapped ? EstimatedValueFirstDiscoveredFirstMappedEfficiently : EstimatedValueFirstDiscoveredFirstMapped;
                    else if (IsNotMapped && Mapped)
                        return EfficientMapped ? EstimatedValueFirstMappedEfficiently : EstimatedValueFirstMapped;
                    else if (IsNotDiscovered)
                        return EstimatedValueFirstDiscovered;
                }

                return EstimatedValueBase;
            }
        }

        public int EstimatedValueBase { get; private set; }     // Estimated value without mapping or first discovery - all types, all versions

        public int EstimatedValueFirstDiscovered { get; private set; }     // Estimated value with first discovery  - 3.3 onwards for these for planets only
        public int EstimatedValueFirstDiscoveredFirstMapped { get; private set; }           // with both
        public int EstimatedValueFirstDiscoveredFirstMappedEfficiently { get; private set; }           // with both efficiently
        public int EstimatedValueFirstMapped { get; private set; }             // with just mapped                 
        public int EstimatedValueFirstMappedEfficiently { get; private set; }             // with just mapped                 

        public void SetMapped(bool m, bool e)
        {
            mapped = m; efficientmapped = e;
        }

        public int HasSameParents(JournalScan other)     // return -1 if not, or index of last match , 0,1,2
        {
            if (Parents != null && other.Parents != null)
            {
                for (int i = 0; ; i++)
                {
                    int p1 = Parents.Count - 1 - i;
                    int p2 = other.Parents.Count - 1 - i;

                    if (p1 < 0 || p2 < 0)     // if out of parents, return how many of p2 are left
                        return i;

                    if (Parents[p1].BodyID != other.Parents[p2].BodyID || Parents[p1].Type != other.Parents[p2].Type)
                        return -1;
                }
            }
            else
                return -1;
        }

        public string ParentList() { return Parents != null ? string.Join(",", Parents.Select(x => x.Type + ":" + x.BodyID)) : ""; }     // not get on purpose

        private bool mapped, efficientmapped;

        // Constants:

        // stellar references
        public const double oneSolRadius_m = 695700000; // 695,700km

        // planetary bodies
        public const double oneEarthRadius_m = 6371000;
        public const double oneAtmosphere_Pa = 101325;
        public const double oneGee_m_s2 = 9.80665;
        public const double oneEarth_MT = 5.972e15;        // mega tons, 1 meta ton = 1e6 tons = 1e9 kg (google 5.972e21 tons)
        public const double oneMoon_MT = 7.34767309e13;     // mega tons, 1 meta ton = 1e6 tons = 1e9 kg
        public const double EarthMoonMassRatio = oneEarth_MT/oneMoon_MT; 

        // astrometric
        public const double oneLS_m = 299792458;
        public const double oneAU_m = 149597870700;
        public const double oneAU_LS = oneAU_m / oneLS_m;
        public const double oneDay_s = 86400;

        public class StarPlanetRing
        {
            public string Name;     // may be null
            public string RingClass;    // may be null
            public double MassMT;
            public double InnerRad;
            public double OuterRad;

            // has trailing LF
            public string RingInformation(double scale = 1, string scaletype = " MT", bool parentIsStar = false)
            {
                StringBuilder scanText = new StringBuilder();
                scanText.AppendFormat("  {0} ({1})\n", Name.Alt("Unknown".T(EDTx.Unknown)), DisplayStringFromRingClass(RingClass));
                scanText.AppendFormat("  Mass: {0:N4}{1}\n".T(EDTx.StarPlanetRing_Mass), MassMT * scale, scaletype);
                if (parentIsStar && InnerRad > 3000000)
                {
                    scanText.AppendFormat("  Inner Radius: {0:0.00}ls\n".T(EDTx.StarPlanetRing_InnerRadius), (InnerRad / oneLS_m));
                    scanText.AppendFormat("  Outer Radius: {0:0.00}ls\n".T(EDTx.StarPlanetRing_OuterRadius), (OuterRad / oneLS_m));
                }
                else
                {
                    scanText.AppendFormat("  Inner Radius: {0}km\n".T(EDTx.StarPlanetRing_IK), (InnerRad / 1000).ToString("N0"));
                    scanText.AppendFormat("  Outer Radius: {0}km\n".T(EDTx.StarPlanetRing_OK), (OuterRad / 1000).ToString("N0"));
                }
                return scanText.ToNullSafeString();
            }

            // has trailing LF
            public string RingInformationMoons(bool parentIsStar = false)
            {
                return RingInformation(1 / oneMoon_MT, " Moons".T(EDTx.StarPlanetRing_Moons), parentIsStar);
            }

            public static string DisplayStringFromRingClass(string ringClass)   // no trailing LF
            {
                switch (ringClass)
                {
                    case null:
                        return "Unknown".T(EDTx.Unknown);
                    case "eRingClass_Icy":
                        return "Icy".T(EDTx.StarPlanetRing_Icy);
                    case "eRingClass_Rocky":
                        return "Rocky".T(EDTx.StarPlanetRing_Rocky);
                    case "eRingClass_MetalRich":
                        return "Metal Rich".T(EDTx.StarPlanetRing_MetalRich);
                    case "eRingClass_Metalic":
                        return "Metallic".T(EDTx.StarPlanetRing_Metallic);
                    case "eRingClass_RockyIce":
                        return "Rocky Ice".T(EDTx.StarPlanetRing_RockyIce);
                    default:
                        return ringClass.Replace("eRingClass_", "");
                }
            }

            public string RingClassNormalised()
            {
                return RingClass.Replace("eRingClass_", "").SplitCapsWordFull();
            }
        }

        public class BodyParent
        {
            public string Type;
            public int BodyID;
        }

        public JournalScan(JObject evt, long edsmid) : this(evt)
        {
            EdsmID = edsmid;
        }

        public JournalScan(JObject evt) : base(evt, JournalTypeEnum.Scan)
        {
            ScanType = evt["ScanType"].Str();
            BodyName = evt["BodyName"].Str();
            BodyID = evt["BodyID"].IntNull();
            StarSystem = evt["StarSystem"].StrNull();
            SystemAddress = evt["SystemAddress"].LongNull();
            StarType = evt["StarType"].StrNull();

            DistanceFromArrivalLS = evt["DistanceFromArrivalLS"].Double();

            nAge = evt["Age_MY"].DoubleNull();
            nStellarMass = evt["StellarMass"].DoubleNull();
            nRadius = evt["Radius"].DoubleNull();
            if (nRadius != null)
            {
                nRadiusSols = nRadius.Value / oneSolRadius_m;
                nRadiusEarths = nRadius.Value / oneEarthRadius_m;
            }
            nAbsoluteMagnitude = evt["AbsoluteMagnitude"].DoubleNull();
            Luminosity = evt["Luminosity"].StrNull();

            nRotationPeriod = evt["RotationPeriod"].DoubleNull();
            if (nRotationPeriod != null)
                nRotationPeriodDays = nRotationPeriod.Value / oneDay_s;

            nOrbitalPeriod = evt["OrbitalPeriod"].DoubleNull();
            if (nOrbitalPeriod != null)
                nOrbitalPeriodDays = nOrbitalPeriod.Value / oneDay_s;

            nSemiMajorAxis = evt["SemiMajorAxis"].DoubleNull();
            if (nSemiMajorAxis != null)
                nSemiMajorAxisAU = nSemiMajorAxis.Value / oneAU_m;

            nEccentricity = evt["Eccentricity"].DoubleNull();
            nOrbitalInclination = evt["OrbitalInclination"].DoubleNull();
            nPeriapsis = evt["Periapsis"].DoubleNull();
            nAxialTilt = evt["AxialTilt"].DoubleNull();
            if ( nAxialTilt!= null)
                nAxialTiltDeg = nAxialTilt.Value * 180.0 / Math.PI;

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

            AtmosphereID = Bodies.AtmosphereStr2Enum(Atmosphere, out EDAtmosphereProperty ap);
            AtmosphereProperty = ap;

            Volcanism = evt["Volcanism"].StrNull();
            VolcanismID = Bodies.VolcanismStr2Enum(Volcanism, out EDVolcanismProperty vp);
            VolcanismProperty = vp;

            nMassEM = evt["MassEM"].DoubleNull();
            if (nMassEM.HasValue)
                nMassMM = nMassEM.Value * EarthMoonMassRatio;
            nSurfaceGravity = evt["SurfaceGravity"].DoubleNull();
            if (nSurfaceGravity.HasValue)
                nSurfaceGravityG = nSurfaceGravity / oneGee_m_s2;
            nSurfaceTemperature = evt["SurfaceTemperature"].DoubleNull();
            nSurfacePressure = evt["SurfacePressure"].DoubleNull();
            if (nSurfacePressure != null)
                nSurfacePressureEarth = nSurfacePressure.Value / oneAtmosphere_Pa;

            nLandable = evt["Landable"].BoolNull();

            WasMapped = evt["WasMapped"].BoolNull();            // new 3.4
            WasDiscovered = evt["WasDiscovered"].BoolNull();        // new 3.4

            ReserveLevelStr = evt["ReserveLevel"].Str();

            if (IsStar)
            {
                StarTypeID = Bodies.StarStr2Enum(StarType);
                StarSubclass = evt["Subclass"].IntNull();

                if (nRadius.HasValue && nSurfaceTemperature.HasValue)
                {
                    // values initially calculated by Jackie Silver (https://forums.frontier.co.uk/member.php/37962-Jackie-Silver)

                    HabitableZoneInner = DistanceForBlackBodyTemperature(315); // this is the goldilocks zone, where is possible to expect to find planets with liquid water. 
                    HabitableZoneOuter = DistanceForBlackBodyTemperature(223);
                    MetalRichZoneInner = DistanceForNoMaxTemperatureBody(oneSolRadius_m); // we don't know the maximum temperature that the galaxy simulation take as possible...
                    MetalRichZoneOuter = DistanceForBlackBodyTemperature(1100);
                    WaterWrldZoneInner = DistanceForBlackBodyTemperature(307);
                    WaterWrldZoneOuter = DistanceForBlackBodyTemperature(156);
                    EarthLikeZoneInner = DistanceForBlackBodyTemperature(281); // I enlarged a bit the range to fit my and other CMDRs discoveries.
                    EarthLikeZoneOuter = DistanceForBlackBodyTemperature(227);
                    AmmonWrldZoneInner = DistanceForBlackBodyTemperature(193);
                    AmmonWrldZoneOuter = DistanceForBlackBodyTemperature(117);
                    IcyPlanetZoneInner = DistanceForBlackBodyTemperature(150);
                    IcyPlanetZoneOuter = "\u221E"; // practically infinite, at least until the body suffer from the gravitational bond with its host star
                }
            }
            else if (PlanetClass != null)
            {
                PlanetTypeID = Bodies.PlanetStr2Enum(PlanetClass);
                // Fix naming to standard and fix case..
                PlanetClass = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                        ToTitleCase(PlanetClass.ToLowerInvariant()).Replace("Ii ", "II ").Replace("Iv ", "IV ").Replace("Iii ", "III ");
            }
            else
            {
                PlanetTypeID = EDPlanet.Unknown_Body_Type;
            }

            JToken mats = (JToken)evt["Materials"];

            if (mats != null)
            {
                if (mats.Type == JTokenType.Object)
                {
                    Materials = mats?.ToObjectProtected<Dictionary<string, double>>();  // name in fd logs is lower case
                }
                else
                {
                    Materials = new Dictionary<string, double>();
                    foreach (JObject jo in mats)                                        // name in fd logs is lower case
                    {
                        Materials[jo["Name"].Str().ToLowerInvariant()] = jo["Percent"].Double();
                    }
                }

                var na = (from x in Materials select x.Key).ToArray();
                MaterialList = String.Join(",", na);
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

            JToken composition = evt["Composition"];

            if (composition != null)
            {
                PlanetComposition = new Dictionary<string, double>();
                foreach (JProperty jp in composition)
                {
                    PlanetComposition[jp.Name] = (double)jp.Value;
                }
            }

            if (evt["Parents"] != null)
            {
                Parents = new List<BodyParent>();
                foreach (JObject parent in evt["Parents"])
                {
                    JProperty prop = parent.Properties().First();
                    Parents.Add(new BodyParent { Type = prop.Name, BodyID = prop.Value.Int() });
                }
            }

            // EDSM bodies fields

            IsEDSMBody = evt["EDDFromEDSMBodie"].Bool(false);           // Bodie? Who is bodie?  Did you mean Body Finwen ;-)

            JToken discovery = evt["discovery"];
            if (discovery != null)
            {
                EDSMDiscoveryCommander = discovery["commander"].StrNull();
                EDSMDiscoveryUTC = discovery["date"].DateTimeUTC();
            }

            EstimateScanValue();                        // do the basic no mapped no efficency map as the basic formula
        }

        #region Information Returns

        public string RadiusText()  // null if not set, or the best representation
        {
            if (nRadius != null)
            {
                if (nRadius >= oneSolRadius_m / 5)
                    return nRadiusSols.Value.ToString("0.#" + "SR");
                else
                    return (nRadius.Value / 1000).ToString("0.#") + "km";
            }
            else
                return null;
        }

        public string MassEMText()
        {
            if (nMassEM.HasValue)
            {
                if (nMassEM.Value < 0.01)
                    return nMassMM.Value.ToString("0.####") + "MM";
                else
                    return nMassEM.Value.ToString("0.##") + "EM";
            }
            else
                return null;
        }

        public override string SummaryName(ISystem sys)
        {
            string text = "Scan of {0}".T(EDTx.JournalScan_Scanof);
            if (ScanType == "AutoScan")
                text = "Autoscan of {0}".T(EDTx.JournalScan_Autoscanof);
            else if (ScanType == "Detailed")
                text = "Detailed scan of {0}".T(EDTx.JournalScan_Detailedscanof);
            else if (ScanType == "Basic")
                text = "Basic scan of {0}".T(EDTx.JournalScan_Basicscanof);
            else if (ScanType.Contains("Nav"))
                text = "Nav scan of {0}".T(EDTx.JournalScan_Navscanof);

            return string.Format(text, BodyName.ReplaceIfStartsWith(sys.Name));
        }

        public override string EventFilterName
        {
            get
            {
                if (ScanType == "AutoScan" )
                    return "Scan Auto";
                else if (ScanType == "Basic")
                    return "Scan Basic";
                else if (ScanType.Contains("Nav"))
                    return "Scan Nav";
                else
                    return base.EventFilterName;
            }
        }

        static public List<Tuple<string, string, Image>> FilterItems()
        {
            return new List<Tuple<string, string, Image>>()
            {
                new Tuple<string, string,Image>( "Scan Auto", "Scan Auto".T(EDTx.JournalScan_ScanAuto), JournalEntry.JournalTypeIcons[JournalTypeEnum.Scan] ),
                new Tuple<string,string,Image>( "Scan Basic", "Scan Basic".T(EDTx.JournalScan_ScanBasic), JournalEntry.JournalTypeIcons[JournalTypeEnum.Scan] ),
                new Tuple<string,string,Image>( "Scan Nav", "Scan Nav".T(EDTx.JournalScan_ScanNav), JournalEntry.JournalTypeIcons[JournalTypeEnum.Scan] ),
            };
        }

        public override void FillInformation(out string info, out string detailed)      
        {
            if (IsStar)
            {
                info = BaseUtils.FieldBuilder.Build("", GetStarTypeName(), "Mass:;SM;0.00".T(EDTx.JournalScan_MSM), nStellarMass,
                                                "Age:;my;0.0".T(EDTx.JournalScan_Age), nAge,
                                                "Radius:".T(EDTx.JournalScan_RS), RadiusText(),
                                                "Dist:;ls;0.0".T(EDTx.JournalScan_DISTA), DistanceFromArrivalLS,
                                                "Name:".T(EDTx.JournalScan_BNME), BodyName);
            }
            else
            {
                info = BaseUtils.FieldBuilder.Build("", PlanetClass, "Mass:".T(EDTx.JournalScan_MASS), MassEMText(),
                                                "<;, Landable".T(EDTx.JournalScan_Landable), IsLandable,
                                                "<;, Terraformable".T(EDTx.JournalScan_Terraformable), TerraformState == "Terraformable", "", Atmosphere,
                                                 "Gravity:;G;0.00".T(EDTx.JournalScan_Gravity), nSurfaceGravityG,
                                                 "Radius:".T(EDTx.JournalScan_RS), RadiusText(),
                                                 "Dist:;ls;0.0".T(EDTx.JournalScan_DISTA), DistanceFromArrivalLS,
                                                 "Name:".T(EDTx.JournalScan_SNME), BodyName);
            }

            detailed = DisplayString(0, includefront: false);
        }

        public string ShortInformation(bool name = false)
        {
            if (IsStar)
            {
                return BaseUtils.FieldBuilder.Build("", GetStarTypeName(), "Mass:;SM;0.00".T(EDTx.JournalScan_MSM), nStellarMass,
                                                "Age:;my;0.0".T(EDTx.JournalScan_Age), nAge,
                                                "Radius:".T(EDTx.JournalScan_RS), RadiusText(),
                                                "Dist:".T(EDTx.JournalScan_DIST), DistanceFromArrivalText,
                                                "Name:".T(EDTx.JournalScan_BNME), name ? BodyName : null);
            }
            else
            {
                return BaseUtils.FieldBuilder.Build("", PlanetClass, "Mass:".T(EDTx.JournalScan_MASS), MassEMText(),
                                                "<;, Landable".T(EDTx.JournalScan_Landable), IsLandable,
                                                "<;, Terraformable".T(EDTx.JournalScan_Terraformable), TerraformState == "Terraformable", "", Atmosphere,
                                                 "Gravity:;G;0.00".T(EDTx.JournalScan_Gravity), nSurfaceGravityG,
                                                 "Radius:".T(EDTx.JournalScan_RS), RadiusText(),
                                                 "Dist:".T(EDTx.JournalScan_DIST), DistanceFromArrivalText,
                                                 "Name:".T(EDTx.JournalScan_SNME), name ? BodyName : null);
            }
        }

        // has no trailing LF
        public string DisplayString(int indent = 0, MaterialCommoditiesList historicmatlist = null, MaterialCommoditiesList currentmatlist = null, bool includefront = true)//, bool mapped = false, bool efficiencyBonus = false)
        {
            string inds = new string(' ', indent);

            StringBuilder scanText = new StringBuilder();

            scanText.Append(inds);

            if (includefront)
            {
                scanText.AppendFormat("{0} {1}\n\n", BodyName, IsEDSMBody ? " (EDSM)" : "");

                if (IsStar)
                {
                    scanText.AppendFormat(GetStarTypeName() + " (" + StarClassification + ")");
                }
                else if (PlanetClass != null)
                {
                    scanText.AppendFormat("{0}", PlanetClass);

                    if (!PlanetClass.ToLowerInvariant().Contains("gas"))
                    {
                        scanText.AppendFormat((Atmosphere == null || Atmosphere == String.Empty) ? ", No Atmosphere".T(EDTx.JournalScan_NoAtmosphere) : (", " + Atmosphere));
                    }
                }

                if (IsLandable)
                    scanText.AppendFormat(", Landable".T(EDTx.JournalScan_LandC));

                scanText.AppendFormat("\n");

                if (Terraformable)
                    scanText.Append("Candidate for terraforming\n".T(EDTx.JournalScan_Candidateforterraforming));


                if (nAge.HasValue)
                    scanText.AppendFormat("Age: {0} my\n".T(EDTx.JournalScan_AMY), nAge.Value.ToString("N0"));

                if (nStellarMass.HasValue)
                    scanText.AppendFormat("Solar Masses: {0:0.00}\n".T(EDTx.JournalScan_SolarMasses), nStellarMass.Value);

                if (nMassEM.HasValue)
                    scanText.AppendFormat("Mass:".T(EDTx.JournalScan_MASS) + " " + MassEMText() + "\n");

                if (nRadius.HasValue)
                    scanText.AppendFormat("Radius:".T(EDTx.JournalScan_RS) + " " + RadiusText() + "\n");

                if (DistanceFromArrivalLS > 0)
                    scanText.AppendFormat("Distance from Arrival Point {0:N1}ls\n".T(EDTx.JournalScan_DistancefromArrivalPoint), DistanceFromArrivalLS);

                if (HasAtmosphericComposition)
                    scanText.Append(DisplayAtmosphere(4));

                if (HasPlanetaryComposition)
                    scanText.Append(DisplayComposition(4));
            }

            if (nSurfaceTemperature.HasValue)
                scanText.AppendFormat("Surface Temp: {0}K\n".T(EDTx.JournalScan_SurfaceTemp), nSurfaceTemperature.Value.ToString("N0"));

            if (nSurfaceGravity.HasValue)
                scanText.AppendFormat("Gravity: {0:0.00}g\n".T(EDTx.JournalScan_GV), nSurfaceGravityG.Value);

            if (nSurfacePressure.HasValue && nSurfacePressure.Value > 0.00 && !PlanetClass.ToLowerInvariant().Contains("gas"))
            {
                if (nSurfacePressure.Value > 1000)
                {
                    scanText.AppendFormat("Surface Pressure: {0} Atmospheres\n".T(EDTx.JournalScan_SPA), nSurfacePressureEarth.Value.ToString("N2"));
                }
                else
                {
                    scanText.AppendFormat("Surface Pressure: {0} Pa\n".T(EDTx.JournalScan_SPP), (nSurfacePressure.Value).ToString("N2"));
                }
            }

            if (Volcanism != null)
                scanText.AppendFormat("Volcanism: {0}\n".T(EDTx.JournalScan_Volcanism), Volcanism == String.Empty ? "No Volcanism".T(EDTx.JournalScan_NoVolcanism) : System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                                                                            ToTitleCase(Volcanism.ToLowerInvariant()));


            if (nOrbitalPeriodDays.HasValue && nOrbitalPeriodDays > 0)
                scanText.AppendFormat("Orbital Period: {0} days\n".T(EDTx.JournalScan_OrbitalPeriod), nOrbitalPeriodDays.Value.ToString("N1"));

            if (nSemiMajorAxis.HasValue)
            {
                if (IsStar || nSemiMajorAxis.Value > oneAU_m / 10)
                    scanText.AppendFormat("Semi Major Axis: {0:0.00}AU\n".T(EDTx.JournalScan_SMA), nSemiMajorAxisAU.Value);
                else
                    scanText.AppendFormat("Semi Major Axis: {0}km\n".T(EDTx.JournalScan_SMK), (nSemiMajorAxis.Value / 1000).ToString("N1"));
            }

            if (nEccentricity.HasValue)
                scanText.AppendFormat("Orbital Eccentricity: {0:0.000}\n".T(EDTx.JournalScan_OrbitalEccentricity), nEccentricity.Value);

            if (nOrbitalInclination.HasValue)
                scanText.AppendFormat("Orbital Inclination: {0:0.000}°\n".T(EDTx.JournalScan_OrbitalInclination), nOrbitalInclination.Value);

            if (nPeriapsis.HasValue)
                scanText.AppendFormat("Arg Of Periapsis: {0:0.000}°\n".T(EDTx.JournalScan_ArgOfPeriapsis), nPeriapsis.Value);

            if (nAbsoluteMagnitude.HasValue)
                scanText.AppendFormat("Absolute Magnitude: {0:0.00}\n".T(EDTx.JournalScan_AbsoluteMagnitude), nAbsoluteMagnitude.Value);

            if (nAxialTiltDeg.HasValue)
                scanText.AppendFormat("Axial tilt: {0:0.00}°\n".T(EDTx.JournalScan_Axialtilt), nAxialTiltDeg.Value);

            if (nRotationPeriodDays.HasValue)
                scanText.AppendFormat("Rotation Period: {0} days\n".T(EDTx.JournalScan_RotationPeriod), nRotationPeriodDays.Value.ToString("N1"));

            if (nTidalLock.HasValue && nTidalLock.Value)
                scanText.Append("Tidally locked\n".T(EDTx.JournalScan_Tidallylocked));

            if (HasRings)
            {
                scanText.Append("\n");
                if (IsStar)
                {
                    scanText.AppendFormat(Rings.Count() == 1 ? "Belt".T(EDTx.JournalScan_Belt) : "Belts".T(EDTx.JournalScan_Belts), ""); // OLD translator files had "Belt{0}" so supply an empty string just in case
                    for (int i = 0; i < Rings.Length; i++)
                    {
                        if (Rings[i].MassMT > (oneMoon_MT / 10000))
                        {
                            scanText.Append("\n" + RingInformation(i, 1.0 / oneMoon_MT, " Moons".T(EDTx.JournalScan_Moons)));
                        }
                        else
                        {
                            scanText.Append("\n" + RingInformation(i));
                        }
                    }
                }
                else
                {
                    scanText.AppendFormat(Rings.Count() == 1 ? "Ring".T(EDTx.JournalScan_Ring) : "Rings".T(EDTx.JournalScan_Rings), ""); // OLD translator files had "Rings{0}" so supply an empty string just in case

                    for (int i = 0; i < Rings.Length; i++)
                        scanText.Append("\n" + RingInformation(i));
                }
                scanText.Append("\n");
            }

            if (HasMaterials)
            {
                scanText.Append(DisplayMaterials(4, historicmatlist, currentmatlist));
            }

            if (CircumstellarZonesString() != null)
                scanText.Append(CircumstellarZonesString());

            if (IsStar && HabZoneOtherStarsString() != null)
                scanText.Append(HabZoneOtherStarsString());

            if (Mapped)
            {
                scanText.Append("Mapped".T(EDTx.JournalScan_MPI));
                if (EfficientMapped)
                    scanText.Append(" " + "Efficiently".T(EDTx.JournalScan_MPIE));
                scanText.Append("\n");
            }

            scanText.AppendFormat("Current value: {0:N0}".T(EDTx.JournalScan_CV) + "\n", EstimatedValue);

            if (EstimatedValueFirstDiscoveredFirstMapped > 0 && (!WasDiscovered.HasValue || !WasDiscovered.Value))  // if we don't know, or its not discovered
            {
                string msg = "First Discovered+Mapped value: {0:N0}/{1:N0}e".T(EDTx.JournalScan_EVFD) + "\n";
                scanText.AppendFormat(msg, EstimatedValueFirstDiscoveredFirstMapped, EstimatedValueFirstDiscoveredFirstMappedEfficiently);
            }

            if (EstimatedValueFirstMapped > 0 && (!WasMapped.HasValue || !WasMapped.Value))    // if was discovered, but not mapped 
            {
                scanText.AppendFormat("First Mapped value: {0:N0}/{1:N0}e".T(EDTx.JournalScan_EVFM) + "\n", EstimatedValueFirstMapped, EstimatedValueFirstMappedEfficiently);
            }

            if (EstimatedValueFirstDiscovered > 0 && (!WasDiscovered.HasValue || !WasDiscovered.Value))  // if we don't know, or its not discovered
            {
                scanText.AppendFormat("First Discovered value: {0:N0}".T(EDTx.JournalScan_FDV) + "\n", EstimatedValueFirstDiscovered);
            }

            if (EstimatedValueFirstDiscovered > 0 ) // if we have extra details, on planets, show the base value
                scanText.AppendFormat("Base Estimated value: {0:N0}".T(EDTx.JournalScan_EV) + "\n", EstimatedValueBase);

            if (WasDiscovered.HasValue && WasDiscovered.Value)
                scanText.AppendFormat("Already Discovered".T(EDTx.JournalScan_EVAD) + "\n");
            if (WasMapped.HasValue && WasMapped.Value)
                scanText.AppendFormat("Already Mapped".T(EDTx.JournalScan_EVAM) + "\n");

            if (EDSMDiscoveryCommander != null)
                scanText.AppendFormat("Discovered by {0} on {1}".T(EDTx.JournalScan_DB) + "\n", EDSMDiscoveryCommander, EDSMDiscoveryUTC.ToStringZulu());

            scanText.AppendFormat("Scan Type: {0}".T(EDTx.JournalScan_SCNT) + "\n", ScanType);

            scanText.AppendFormat("BID+Parents: {0} - {1}\n", BodyID ?? -1, ParentList());

            if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                scanText.Remove(scanText.Length - 1, 1);

            return scanText.ToNullSafeString().Replace("\n", "\n" + inds);
        }

		// goldilocks zone. No trailing LF
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

        private string CircumstellarZonesString()    // has trailing LF
        {
            if (IsStar && HabitableZoneInner.HasValue && HabitableZoneOuter.HasValue)
            {
                StringBuilder habZone = new StringBuilder();

				habZone.Append("Inferred Circumstellar zones:\n".T(EDTx.JournalScan_InferredCircumstellarzones));

				habZone.AppendFormat(" - Habitable Zone, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_HabitableZone),
									 GetHabZoneStringLs(),
									 (HabitableZoneInner.Value / oneAU_LS).ToString("N2"),
									 (HabitableZoneOuter.Value / oneAU_LS).ToString("N2"));

				habZone.AppendFormat(" - Metal Rich planets, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_MetalRichplanets),
									 GetMetalRichZoneStringLs(),
									 (MetalRichZoneInner.Value / oneAU_LS).ToString("N2"),
									 (MetalRichZoneInner.Value / oneAU_LS).ToString("N2"));
				
				habZone.AppendFormat(" - Water Worlds, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_WaterWorlds),
									 GetWaterWorldZoneStringLs(),
									 (WaterWrldZoneInner.Value / oneAU_LS).ToString("N2"),
									 (WaterWrldZoneOuter.Value / oneAU_LS).ToString("N2"));
				
				habZone.AppendFormat(" - Earth Like Worlds, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_EarthLikeWorlds),
									 GetEarthLikeZoneStringLs(),
									 (EarthLikeZoneInner.Value / oneAU_LS).ToString("N2"),
									 (EarthLikeZoneOuter.Value / oneAU_LS).ToString("N2"));
				
				habZone.AppendFormat(" - Ammonia Worlds, {0} ({1}-{2} AU),\n".T(EDTx.JournalScan_AmmoniaWorlds),
									 GetAmmoniaWorldZoneStringLs(),
									 (AmmonWrldZoneInner.Value / oneAU_LS).ToString("N2"),
									 (AmmonWrldZoneOuter.Value / oneAU_LS).ToString("N2"));
				
				habZone.AppendFormat(" - Icy Planets, {0} (from {1} AU)\n".T(EDTx.JournalScan_IcyPlanets),
									 GetIcyPlanetsZoneStringLs(),
				(IcyPlanetZoneInner.Value / oneAU_LS).ToString("N2"));

                return habZone.ToNullSafeString();
            }
            else
                return null;
        }

		// string which tell us that other stars are not considered in the habitable zone calculations.
		public string HabZoneOtherStarsString() // has trailing LF 
		{
			StringBuilder habZoneAddend = new StringBuilder();
			if (nSemiMajorAxis.HasValue && nSemiMajorAxis.Value > 0)
				habZoneAddend.Append(" - Others stars not considered\n".T(EDTx.JournalScan_Othersstarsnotconsidered));
			
			return habZoneAddend.ToNullSafeString();
		}

		// metal rich zone
		public string GetMetalRichZoneStringLs() // no trailing LF
        {
			if (IsStar && MetalRichZoneInner.HasValue && MetalRichZoneOuter.HasValue)
			{
				return $"{MetalRichZoneInner:N0}-{MetalRichZoneOuter:N0}ls";
			}
			else
			{
				return string.Empty;
			}
		}

		public string MetalRichZoneString() // no trailing LF
        {
			if (IsStar && MetalRichZoneInner.HasValue && MetalRichZoneOuter.HasValue)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Metal Rich Planets: {0} ({1}-{2} AU)\n".T(EDTx.JournalScan_MRP),
									 GetMetalRichZoneStringLs(), 
									 (MetalRichZoneInner.Value / oneAU_LS).ToString("N2"), 
									 (MetalRichZoneOuter.Value / oneAU_LS).ToString("N2"));
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

		// water world zone
		public string GetWaterWorldZoneStringLs() // no trailing LF
        {
			if (IsStar && WaterWrldZoneInner.HasValue && WaterWrldZoneOuter.HasValue)
			{
				return $"{WaterWrldZoneInner:N0}-{WaterWrldZoneOuter:N0}ls";
			}
			else
			{
				return string.Empty;
			}
		}

		public string WaterWorldZoneString() // has trailing LF
        {
			if (IsStar && WaterWrldZoneInner.HasValue && WaterWrldZoneOuter.HasValue)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Water Worlds: {0} ({1}-{2} AU)\n".T(EDTx.JournalScan_WWP),
									 GetWaterWorldZoneStringLs(), 
									 (WaterWrldZoneInner.Value / oneAU_LS).ToString("N2"), 
									 (WaterWrldZoneOuter.Value / oneAU_LS).ToString("N2"));
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

        // earth like world zone. No trailing LF
        public string GetEarthLikeZoneStringLs()
		{
			if (IsStar && EarthLikeZoneInner.HasValue && EarthLikeZoneOuter.HasValue)
			{
				return $"{EarthLikeZoneInner:N0}-{EarthLikeZoneOuter:N0}ls";
			}
			else
			{
				return string.Empty;
			}
		}

		public string EarthLikeZoneString() // has trailing LF
		{
			if (IsStar && EarthLikeZoneInner.HasValue && EarthLikeZoneOuter.HasValue)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Earth Like Worlds: {0} ({1}-{2} AU)\n".T(EDTx.JournalScan_ELWP),
									 GetEarthLikeZoneStringLs(), 
									 (EarthLikeZoneInner.Value / oneAU_LS).ToString("N2"), 
									 (EarthLikeZoneOuter.Value / oneAU_LS).ToString("N2"));
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

		// ammonia world zone. No trailing LF
		public string GetAmmoniaWorldZoneStringLs()
		{
			if (IsStar && AmmonWrldZoneInner.HasValue && AmmonWrldZoneOuter.HasValue)
			{
				return $"{AmmonWrldZoneInner:N0}-{AmmonWrldZoneOuter:N0}ls";
			}
			else
			{
				return string.Empty;
			}
		}

		public string AmmoniaWorldZoneString() // has trailing LF
		{
			if (IsStar && AmmonWrldZoneInner.HasValue && AmmonWrldZoneOuter.HasValue)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Ammonia Worlds: {0} ({1}-{2} AU)\n".T(EDTx.JournalScan_AWP),
									 GetAmmoniaWorldZoneStringLs(), 
									 (AmmonWrldZoneInner.Value / oneAU_LS).ToString("N2"), 
									 (AmmonWrldZoneOuter.Value / oneAU_LS).ToString("N2"));
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

		// icy planets zone. No trailing LF
		public string GetIcyPlanetsZoneStringLs()
		{
			if (IsStar && IcyPlanetZoneInner.HasValue && IcyPlanetZoneOuter != null)
			{
				return $"{IcyPlanetZoneInner:N0}ls to {IcyPlanetZoneOuter:N0}";
			}
			else
			{
				return string.Empty;
			}
		}

		public string IcyPlanetsZoneString()    // has trailing LF
        {
			if (IsStar && IcyPlanetZoneInner.HasValue && IcyPlanetZoneOuter != null)
			{
				StringBuilder habZone = new StringBuilder();
				habZone.AppendFormat("Icy Planets: {0} ({1} AU to {2})\n".T(EDTx.JournalScan_ICYP),
									 GetIcyPlanetsZoneStringLs(), 
									 (IcyPlanetZoneInner.Value / oneAU_LS).ToString("N2"),
									 IcyPlanetZoneOuter);
				return habZone.ToNullSafeString();
			}
			else
				return null;
		}

        // show material counts at the historic point and current.  Has trailing LF if text present.
        public string DisplayMaterials(int indent = 0, MaterialCommoditiesList historicmatlist = null, MaterialCommoditiesList currentmatlist = null)
        {
            StringBuilder scanText = new StringBuilder();

            if (HasMaterials)
            {
                string indents = new string(' ', indent);

                scanText.Append("Materials:\n".T(EDTx.JournalScan_Materials));
                foreach (KeyValuePair<string, double> mat in Materials)
                {
                    scanText.Append(indents + DisplayMaterial(mat.Key, mat.Value, historicmatlist, currentmatlist));
                }
            }

            return scanText.ToNullSafeString();
        }

        public string DisplayMaterial(string fdname, double percent, MaterialCommoditiesList historicmatlist = null, 
                                        MaterialCommoditiesList currentmatlist = null)  // has trailing LF
        {
            StringBuilder scanText = new StringBuilder();

            MaterialCommodityData mc = MaterialCommodityData.GetByFDName(fdname);

            if (mc != null && (historicmatlist != null || currentmatlist != null))
            {
                MaterialCommodities historic = historicmatlist?.Find(mc);
                MaterialCommodities current = ReferenceEquals(historicmatlist,currentmatlist) ? null : currentmatlist?.Find(mc);
                int? limit = mc.MaterialLimit();

                string matinfo = historic?.Count.ToString() ?? "0";
                if (limit != null)
                    matinfo += "/" + limit.Value.ToString();

                if (current != null && (historic == null || historic.Count != current.Count) )
                    matinfo += " Cur " + current.Count.ToString();

                scanText.AppendFormat("{0} ({1}) {2} {3}% {4}\n", mc.Name, mc.Shortname, mc.TranslatedType, percent.ToString("N1"), matinfo);
            }
            else
                scanText.AppendFormat("{0} {1}%\n", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(fdname.ToLowerInvariant()),
                                                            percent.ToString("N1"));

            return scanText.ToNullSafeString();
        }

        private string DisplayAtmosphere(int indent = 0)     // has trailing LF
        {
            StringBuilder scanText = new StringBuilder();
            string indents = new string(' ', indent);

            scanText.Append("Atmospheric Composition:\n".T(EDTx.JournalScan_AtmosphericComposition));
            foreach (KeyValuePair<string, double> comp in AtmosphereComposition)
            {
                scanText.AppendFormat(indents + "{0} - {1}%\n", comp.Key, comp.Value.ToString("N2"));
            }

            return scanText.ToNullSafeString();
        }

        private string DisplayComposition(int indent = 0)   // has trailing LF
        {
            StringBuilder scanText = new StringBuilder();
            string indents = new string(' ', indent);

            scanText.Append("Planetary Composition:\n".T(EDTx.JournalScan_PlanetaryComposition));
            foreach (KeyValuePair<string, double> comp in PlanetComposition)
            {
                if (comp.Value > 0)
                    scanText.AppendFormat(indents + "{0} - {1}%\n", comp.Key, (comp.Value * 100).ToString("N2"));
            }

            return scanText.ToNullSafeString();
        }

        // Has Trailing LF
        private string RingInformation(int ringno, double scale = 1, string scaletype = " MT")
        {
            StarPlanetRing ring = Rings[ringno];
            return ring.RingInformation(scale, scaletype, IsStar);
        }

        public StarPlanetRing FindRing(string name)
        {
            if (Rings != null)
                return Array.Find(Rings,x => x.Name.Equals(name,StringComparison.InvariantCultureIgnoreCase));
            else
                return null;
        }

        public string GetStarTypeName()           // give description to star class
        {
            return Bodies.StarName(StarTypeID);
        }

        public System.Drawing.Image GetStarTypeImage()           // give image and description to star class
        {
            // parameters needed for variations
            var sm = nStellarMass;
            var st = nSurfaceTemperature;

            string iconName = StarTypeID.ToString(); // fallback

            // black holes variations. According to theorethical papers, we should find four classes of black holes: 
            // supermassive, intermediate, stellar and micro (https://en.wikipedia.org/wiki/Black_hole) gravitional collapses.
            // The in-game black hole population do not really fit quite well that nomenclature, they are somewhat capped, so we have to quantize the masses ranges to more reasonable limits.
            // For example, SagA* is known to have at least 4 millions solar masses, in real life: the in-game stats shows only 516.608, way smaller! The same for Great Annihilator: the biggest
            // of the couple should be more than some hundrends of thousands of solar masses, which is the expected mass range of an intermediate black holes...
            if (StarTypeID == EDStar.H)
            {
                if (sm <= 7.0)
                    iconName = "H_stellar";
                else if (sm < 70.0)
                    iconName = "H";
                else
                    iconName = "H_intermediate";
            }

            // Sagittarius A* receive it's own icon, by David Braben himself.
            if (StarTypeID == EDStar.SuperMassiveBlackHole)
                iconName = "SuperMassiveBlackHole";

            // neutron stars variations. 
            // Uses theorethical masses: https://en.wikipedia.org/wiki/Neutron_star
            if (StarTypeID == EDStar.N)
            {
                if (sm < 1.1)
                    iconName = "N";
                else if (sm < 1.9)
                    iconName = "N_massive";
                else
                    iconName = "N_veryMassive";
            }

            // white dwarfs variations
            if (StarTypeID == EDStar.D || StarTypeID == EDStar.DA || StarTypeID == EDStar.DAB || StarTypeID == EDStar.DAO || StarTypeID == EDStar.DAV || StarTypeID == EDStar.DAZ || StarTypeID == EDStar.DB ||
                StarTypeID == EDStar.DBV || StarTypeID == EDStar.DBZ || StarTypeID == EDStar.DC || StarTypeID == EDStar.DCV || StarTypeID == EDStar.DO || StarTypeID == EDStar.DOV || StarTypeID == EDStar.DQ ||
                StarTypeID == EDStar.DX)
            {
                if (st <= 5500)
                    iconName = "D";
                else if (st < 8000)
                    iconName = "D_hot";
                else if (st < 14000)
                    iconName = "D_veryHot";
                else if (st >= 14000)
                    iconName = "D_extremelyHot";
            }

            // carbon stars
            if (StarTypeID == EDStar.C || StarTypeID == EDStar.CHd || StarTypeID == EDStar.CJ || StarTypeID == EDStar.CN || StarTypeID == EDStar.CS)
                iconName = "C";

            // Herbig AeBe
            // https://en.wikipedia.org/wiki/Herbig_Ae/Be_star
            // This kind of star classes show a spectrum of an A or B star class. It all depend on their surface temperature
            if (StarTypeID == EDStar.AeBe)
            {
                if (st < 5000)
                    iconName = "A";
                else
                    iconName = "B";
            }

            // giants and supergiants can use the same icons of their classes, so we'll use them, to avoid duplication. In case we really want, we can force a bigger size in scan panel...
            // better: huge corona overlay? ;)
            if (StarTypeID == EDStar.A_BlueWhiteSuperGiant)
            {
                iconName = "A";
            }
            else if (StarTypeID == EDStar.B_BlueWhiteSuperGiant)
            {
                iconName = "B";
            }
            else if (StarTypeID == EDStar.F_WhiteSuperGiant)
            {
                iconName = "F";
            }
            else if (StarTypeID == EDStar.G_WhiteSuperGiant)
            {
                iconName = "G";
            }
            else if (StarTypeID == EDStar.K_OrangeGiant)
            {
                iconName = "K";
            }
            else if (StarTypeID == EDStar.M_RedGiant)
            {
                iconName = "M";
            }
            else if (StarTypeID == EDStar.M_RedSuperGiant)
            {
                iconName = "M";
            }

            // t-tauri shows spectral colours related to their surface temperature...
            // They are pre-main sequence stars, so their spectrum shows similarities to main-sequence stars with the same surface temperature; are usually brighter, however, because of the contraction process.
            // https://en.wikipedia.org/wiki/Stellar_classification
            // https://en.wikipedia.org/wiki/T_Tauri_star
            if (StarTypeID == EDStar.TTS)
            {
                if (st < 3700)
                    iconName = "M";
                else if (st < 5200)
                    iconName = "K";
                else if (st < 6000)
                    iconName = "G";
                else if (st < 7500)
                    iconName = "F";
                else if (st < 10000)
                    iconName = "A";
                else if (st < 30000)
                    iconName = "B";
                else
                    iconName = "O";
            }

            // wolf-rayets stars
            // https://en.wikipedia.org/wiki/Wolf%E2%80%93Rayet_star
            if (StarTypeID == EDStar.W || StarTypeID == EDStar.WC || StarTypeID == EDStar.WN || StarTypeID == EDStar.WNC || StarTypeID == EDStar.WO)
            {
                if (st < 50000)
                    iconName = "F";
                if (st < 90000)
                    iconName = "A";
                if (st < 140000)
                    iconName = "B";
                if (st > 140000)
                    iconName = "O";
            }

            if (StarTypeID == (EDStar.MS | EDStar.S ))
                iconName = "M";

            if (StarTypeID == EDStar.Nebula)
                return EDDiscovery.Icons.IconSet.GetIcon($"Bodies.Nebula");

            if (StarTypeID == EDStar.StellarRemnantNebula)
                return EDDiscovery.Icons.IconSet.GetIcon($"Bodies.StellarRemnantNebula");

            if (StarTypeID == EDStar.X || StarTypeID == EDStar.RoguePlanet)
            {
                System.Diagnostics.Debug.WriteLine(StarTypeID + ": " + iconName);
                return EDDiscovery.Icons.IconSet.GetIcon($"Bodies.Unknown");
            }
            else

                System.Diagnostics.Debug.WriteLine(StarTypeID + ": " + iconName);
            return EDDiscovery.Icons.IconSet.GetIcon($"Bodies.Stars.{iconName}");
        }

        static public System.Drawing.Image GetStarImageNotScanned()
        {
            return EDDiscovery.Icons.IconSet.GetIcon($"Bodies.Unknown");
        }

        public System.Drawing.Image GetPlanetClassImage()
        {
            var st = nSurfaceTemperature;

            if (PlanetClass == null)
            {
                return GetPlanetImageNotScanned();
            }

            string iconName = PlanetTypeID.ToString();

            // Gas Giants variants
            if (PlanetTypeID.ToNullSafeString().ToLowerInvariant().Contains("giant"))
            {
                iconName = "GG1v1"; // fallback

                if (PlanetTypeID == EDPlanet.Gas_giant_with_ammonia_based_life)
                {
                    if (st < 105)
                        iconName = "GGAv8";
                    else if (st < 110)
                        iconName = "GGAv11";
                    else if (st < 115)
                        iconName = "GGAv9";
                    else if (st < 120)
                        iconName = "GGAv2";
                    else if (st < 124)
                        iconName = "GGAv12";
                    else if (st < 128)
                        iconName = "GGAv14";
                    else if (st < 130)
                        iconName = "GGAv7";
                    else if (st < 134)
                        iconName = "GGAv13";
                    else if (st < 138)
                        iconName = "GGAv6";
                    else if (st < 142)
                        iconName = "GGAv1";
                    else if (st < 148)
                        iconName = "GGAv3";
                    else if (st < 152)
                        iconName = "GGAv5";
                    else
                        iconName = "GGAv4";
                }

                if (PlanetTypeID == (EDPlanet.Water_giant_with_life | EDPlanet.Gas_giant_with_water_based_life))
                {
                    if (st < 152)
                        iconName = "GGWv24";
                    else if (st < 155)
                    {
                        if (AtmosphereComposition.ToNullSafeString().ToLowerInvariant().Contains("oxygen"))
                            iconName = "GGWv1";
                        else
                            iconName = "GGWv16";
                    }
                    else if (st < 158)
                        iconName = "GGWv3";
                    else if (st < 160)
                        iconName = "GGWv14";
                    else if (st < 162)
                        iconName = "GGWv22";
                    else if (st < 165)
                        iconName = "GGWv20";
                    else if (st < 172)
                        iconName = "GGWv25";
                    else if (st < 175)
                        iconName = "GGWv2";
                    else if (st < 180)
                        iconName = "GGWv13";
                    else if (st < 185)
                        iconName = "GGWv9";
                    else if (st < 190)
                        iconName = "GGWv21";
                    else if (st < 200)
                        iconName = "GGWv7";
                    else if (st < 205)
                        iconName = "GGWv8";
                    else if (st < 210)
                        iconName = "GGWv15";
                    else if (st < 213)
                        iconName = "GGWv17";
                    else if (st < 216)
                        iconName = "GGWv6";
                    else if (st < 219)
                        iconName = "GGWv18";
                    else if (st < 222)
                        iconName = "GGWv10";
                    else if (st < 225)
                        iconName = "GGWv11";
                    else if (st < 228)
                        iconName = "GGWv23";
                    else if (st < 232)
                        iconName = "GGWv5";
                    else if (st < 236)
                        iconName = "GGWv12";
                    else
                    {
                        if (AtmosphereComposition.ToNullSafeString().ToLowerInvariant().Contains("oxygen"))
                            iconName = "GGWv19";
                        else
                            iconName = "GGWv4";
                    }
                }

                if (PlanetTypeID == (EDPlanet.Helium_gas_giant | EDPlanet.Helium_rich_gas_giant))
                {
                    if (st < 110)
                    {
                        if (AtmosphereComposition.ToNullSafeString().ToLowerInvariant().Contains("antimony") || 
                            AtmosphereComposition.ToNullSafeString().ToLowerInvariant().Contains("cadmium") || 
                            AtmosphereComposition.ToNullSafeString().ToLowerInvariant().Contains("niobium"))
                            iconName = "GGHv7";
                        else
                            iconName = "GGHv7";
                    }
                    else if (st < 125)
                        iconName = "GGHv6";
                    else if (st < 140)
                        iconName = "GGHv2";
                    else if (st < 180)
                        iconName = "GGHv5";
                    else if (st < 270)
                        iconName = "GGHv4";
                    else if (st < 600)
                        iconName = "GGHv1";
                    else if (st < 700)
                        iconName = "GGHv9";
                    else
                        iconName = "GGHv8";
                }

                if (PlanetTypeID == EDPlanet.Sudarsky_class_I_gas_giant)
                {

                    if (st <= 30)
                        iconName = "GG1v12";
                    else if (st < 35)
                        iconName = "GG1v15";
                    else if (st < 40)
                        iconName = "GG1v13";
                    else if (st < 45)
                        iconName = "GG1v4"; // neptune
                    else if (st < 50)
                        iconName = "GG1v9";
                    else if (st < 55)
                        iconName = "GG1v2";
                    else if (st < 60)
                        iconName = "GG1v16"; // uranus
                    else if (st < 65)
                        iconName = "GG1v19";
                    else if (st < 70)
                        iconName = "GG1v18";
                    else if (st < 78)
                        iconName = "GG1v11";
                    else if (st < 85)
                        iconName = "GG1v3";
                    else if (st < 90)
                        iconName = "GG1v6";
                    else if (st < 100)
                        iconName = "GG1v8";
                    else if (st < 110)
                        iconName = "GG1v1";
                    else if (st < 130)
                        iconName = "GG1v5";
                    else if (st < 135)
                        iconName = "GG1v17";
                    else if (st < 140)
                        iconName = "GG1v20";
                    else if (st < 150)
                        iconName = "GG1v14"; // jupiter
                    else if (st < 170)
                        iconName = "GG1v7";
                    else
                        iconName = "GG1v10";
                }

                if (PlanetTypeID == EDPlanet.Sudarsky_class_II_gas_giant)
                {
                    if (st < 160)
                        iconName = "GG2v4";
                    else if (st < 175)
                        iconName = "GG2v7";
                    else if (st < 200)
                        iconName = "GG2v5";
                    else if (st < 2450)
                        iconName = "GG2v8";
                    else if (st < 260)
                        iconName = "GG2v6";
                    else if (st < 275)
                        iconName = "GG2v1";
                    else if (st < 300)
                        iconName = "GG2v2";
                    else
                        iconName = "GG2v3";
                }

                if (PlanetTypeID == EDPlanet.Sudarsky_class_III_gas_giant)
                {
                    if (st < 300)
                        iconName = "GG3v2";
                    else if (st < 340)
                        iconName = "GG3v3";
                    else if (st < 370)
                        iconName = "GG3v12";
                    else if (st < 400)
                        iconName = "GG3v1";
                    else if (st < 500)
                        iconName = "GG3v5";
                    else if (st < 570)
                        iconName = "GG3v4";
                    else if (st < 600)
                        iconName = "GG3v8";
                    else if (st < 620)
                        iconName = "GG3v10";
                    else if (st < 660)
                        iconName = "GG3v7";
                    else if (st < 700)
                        iconName = "GG3v9";
                    else if (st < 742)
                        iconName = "GG3v11";
                    else if (st < 760)
                        iconName = "GG3v13";
                    else                    
                        iconName = "GG3v6";                    
                }

                if (PlanetTypeID == EDPlanet.Sudarsky_class_IV_gas_giant)
                {
                    if (st < 810)
                        iconName = "GG4v9";
                    else if (st < 830)
                        iconName = "GG4v6";
                    else if (st < 880)
                        iconName = "GG4v4";
                    else if (st < 950)
                        iconName = "GG4v10";
                    else if (st < 1010)
                        iconName = "GG4v3";
                    else if (st < 1070)
                        iconName = "GG4v1";
                    else if (st < 1125)                    
                        iconName = "GG4v7";
                    else if (st < 1200)
                        iconName = "GG4v2";
                    else if (st < 1225)
                        iconName = "GG4v11";
                    else if (st < 1250)
                        iconName = "GG4v8";
                    else
                        iconName = "GG4v5";
                }

                if (PlanetTypeID == EDPlanet.Sudarsky_class_V_gas_giant)
                {
                    if (st < 1600)
                        iconName = "GG5v3";
                    else if (st < 1620)
                        iconName = "GG5v4";
                    else if (st < 1700)
                        iconName = "GG5v1";
                    else if (st < 1850)
                        iconName = "GG5v2";
                    else
                        iconName = "GG5v5";
                }

                if (PlanetTypeID == EDPlanet.Water_giant)
                {
                    if (st < 155)
                        iconName = "WTGv6";
                    else if (st < 160)
                        iconName = "WTGv2";
                    else if (st < 165)
                        iconName = "WTGv1";
                    else if (st < 170)
                        iconName = "WTGv3";
                    else if (st < 180)
                        iconName = "WTGv4";
                    else if (st < 190)
                        iconName = "WTGv5";
                    else
                        iconName = "WTGv7";
                }

                System.Diagnostics.Debug.WriteLine(PlanetTypeID + ": " + iconName);
                return EDDiscovery.Icons.IconSet.GetIcon($"Bodies.Planets.Giant.{iconName}");
            }

            // Terrestrial planets variants

            // Ammonia world 
            if (PlanetTypeID == EDPlanet.Ammonia_world)
            {
                iconName = "AMWv1"; // fallback                

                if (Terraformable) // extremely rare, but they exists
                    iconName = "AMWv2";
                else if (AtmosphereProperty == EDAtmosphereProperty.Thick || AtmosphereProperty == EDAtmosphereProperty.Hot)
                    iconName = "AMWv3";
                else if (AtmosphereProperty == EDAtmosphereProperty.Rich || AtmosphereID == EDAtmosphereType.Ammonia_and_oxygen)
                    iconName = "AMWv4"; // kindly provided by CMDR CompleteNOOB
                else if (nLandable == true || AtmosphereID == EDAtmosphereType.No_atmosphere && st < 140)
                    iconName = "AMWv5"; // kindly provided by CMDR CompleteNOOB
                else if (st < 190)
                    iconName = "AMWv6";
                else if (st < 200)
                    iconName = "AMWv3";
                else if (st < 210)
                {
                    iconName = "AMWv1";
                }
                else
                    iconName = "AMWv4";
            }

            // Earth world
            if (PlanetTypeID == EDPlanet.Earthlike_body)
            {
                iconName = "ELWv5"; // fallback

                if ((int)nMassEM == 1 && st == 288) // earth, or almost identical to
                {
                    iconName = "ELWv1";
                }
                else
                {
                    if (nTidalLock == true)
                        iconName = "ELWv7";
                    else
                    {
                        if (nMassEM < 0.15 && st < 262) // mars, or extremely similar to
                            iconName = "ELWv4";
                        else if (st < 270)
                            iconName = "ELWv8";
                        else if (st < 285)
                            iconName = "ELWv2";
                        else if (st < 300)
                            iconName = "ELWv3";
                        else
                            iconName = "ELWv5"; // kindly provided by CMDR CompleteNOOB
                    }
                }
            }

            if (PlanetTypeID == EDPlanet.High_metal_content_body)
            {
                iconName = "HMCv3"; // fallback

                // landable, atmosphere-less high metal content bodies
                if (nLandable == true || AtmosphereProperty == EDAtmosphereProperty.None || AtmosphereID == EDAtmosphereType.No_atmosphere)
                {
                    if (st < 300)
                    {
                        if (nTidalLock == true)
                            iconName = "HMCv30";
                        else
                            iconName = "HMCv27"; // kindly provided by CMDR CompleteNOOB
                    }
                    else if (st < 500)
                        iconName = "HMCv34";
                    else if (st < 700)
                        iconName = "HMCv32";
                    else if (st < 900)
                        iconName = "HMCv31";
                    else if (st < 1000)
                    {
                        if (nTidalLock == true)
                            iconName = "HMCv33";
                        else
                            iconName = "HMCv35";
                    }
                    else if (st >= 1000)
                        iconName = "HMCv36";
                }
                // non landable, high metal content bodies with atmosphere
                else if (nLandable == false)
                {
                    if (AtmosphereID == EDAtmosphereType.Ammonia)
                    {
                        if (nTidalLock == true)
                            iconName = "HMCv29";
                        else
                            iconName = "HMCv17";
                    }
                    else if (AtmosphereID == EDAtmosphereType.Argon)
                        iconName = "HMCv26";
                    else if (AtmosphereID == EDAtmosphereType.Carbon_dioxide)
                    {
                        if (st < 220)
                            iconName = "HMCv9";
                        else if (st < 250)
                            iconName = "HMCv12";
                        else if (st < 285)
                            iconName = "HMCv6";
                        else if (st < 350)
                            iconName = "HMCv28";
                        else if (st < 400)
                        {
                            if (nTidalLock == true)
                                iconName = "HMCv7";
                            else
                                iconName = "HMCv8";
                        }
                        else if (st < 600)
                        {
                            if (nTidalLock == true)
                                iconName = "HMCv1";
                            else
                                iconName = "HMCv24";
                        }
                        else if (st < 700)
                            iconName = "HMCv3";
                        else if (st < 900)
                            iconName = "HMCv25";
                        else if (st > 1250)
                            iconName = "HMCv14";
                        else
                            iconName = "HMCv18"; // kindly provided by CMDR CompleteNOOB
                    }
                    else if (AtmosphereID == EDAtmosphereType.Methane)
                    {
                        if (nTidalLock == true)
                            iconName = "HMCv19";
                        else
                            iconName = "HMCv11";
                    }
                    else if (AtmosphereID == EDAtmosphereType.Nitrogen)
                    {
                        if (st < 200)
                            iconName = "HMCv2";
                        else
                            iconName = "HMCv5";
                    }
                    else if (AtmosphereID == EDAtmosphereType.Sulphur_dioxide)
                    {
                        if (st < 700)
                            iconName = "HMCv23";
                        else
                            iconName = "HMCv37"; // kindly provided by CMDR CompleteNOOB
                    }
                    else if (AtmosphereID == EDAtmosphereType.Water)
                    {
                        if (st < 400)
                            iconName = "HMCv4";
                        else if (st < 700)
                            iconName = "HMCv13";
                        else if (st < 1000)
                            iconName = "HMCv16";
                        else
                            iconName = "HMCv20";
                    }
                    else // for all other non yet available atmospheric types
                        iconName = "HMCv3"; // fallback
                }
                else
                    iconName = "HMCv3"; // fallback                                
            }

            if (PlanetTypeID == EDPlanet.Icy_body)
            {
                iconName = "ICYv4"; // fallback

                if (nLandable == true)
                {
                    iconName = "ICYv7";
                }
                else
                {
                    if (AtmosphereID == EDAtmosphereType.Helium)
                        iconName = "ICYv10";
                    else if (AtmosphereID == EDAtmosphereType.Neon || AtmosphereComposition.ToNullSafeString().ToLowerInvariant().Contains("neon"))
                    {
                        if (st < 55)
                            iconName = "ICYv6";
                        else
                            iconName = "ICYv9";
                    }
                    else if (AtmosphereID == EDAtmosphereType.Argon)
                    {
                        if (st < 100)
                            iconName = "ICYv1";
                        else
                            iconName = "ICYv5";
                    }
                    else if (AtmosphereID == EDAtmosphereType.Nitrogen)
                    {
                        if (st < 105)
                            iconName = "ICYv2";
                        else if (st < 150)
                            iconName = "ICYv3";
                        else
                            iconName = "ICYv4";
                    }
                    else if (AtmosphereID == EDAtmosphereType.Methane)
                    {
                        if (nTidalLock == true)
                            iconName = "ICYv3";
                        else
                            iconName = "ICYv8";
                    }
                    else
                        iconName = "ICYv5";
                }
            }

            if (PlanetTypeID == EDPlanet.Metal_rich_body)
            {
                iconName = "MRBv1"; // fallback

                if (nLandable == true)
                {
                    if (st < 1000)
                        iconName = "MRBv7";
                    else if (st < 1200)
                        iconName = "MRBv2";
                    else if (st < 2000)
                        iconName = "MRBv12";
                    else
                        iconName = "MRBv8";
                }
                else
                {
                    if (st < 1600)
                        iconName = "MRBv9";
                    else if (st < 1800)
                        iconName = "MRBv3";
                    else if (st < 1900)
                        iconName = "MRBv4";
                    else if (st < 2000)
                        iconName = "MRBv10";
                    else if (st < 2200)
                        iconName = "MRBv11";
                    else if (st < 2400)
                        iconName = "MRBv14";
                    else if (st < 2600)
                        iconName = "MRBv8";
                    else if (st < 3500)
                        iconName = "MRBv13";
                    else if (st < 5000)
                        iconName = "MRBv1";
                    else if (st < 6000)
                        iconName = "MRBv5";
                    else
                        iconName = "MRBv6";
                }
            }

            if (PlanetTypeID == EDPlanet.Rocky_body)
            {
                iconName = "RBDv1"; // fallback

                if (st == 55 && !IsLandable) // pluto (actually, pluto is a rocky-ice body, in real life; however, the game consider it a rocky body. Too bad...)
                    iconName = "RBDv6";
                else if (st < 150)
                    iconName = "RBDv2";
                else if (st < 300)
                    iconName = "RBDv1";
                else if (st < 400)
                    iconName = "RBDv3";
                else if (st < 500)
                    iconName = "RBDv4";
                else // for high temperature rocky bodies
                    iconName = "RBDv5";
            }

            if (PlanetTypeID == EDPlanet.Rocky_ice_body)
            {
                iconName = "RIBv1"; // fallback

                if (st < 50)
                    iconName = "RIBv1";
                else if (st < 150)
                    iconName = "RIBv2";
                else
                    iconName = "RIBv4";

                if (nTidalLock == true)
                    iconName = "RIBv3";
                else
                {
                    if (AtmosphereProperty == (EDAtmosphereProperty.Thick | EDAtmosphereProperty.Rich))
                    {
                        iconName = "RIBv4";
                    }
                    else if (AtmosphereProperty == (EDAtmosphereProperty.Hot | EDAtmosphereProperty.Thin))
                        iconName = "RIBv1";
                }
            }

            if (PlanetTypeID == EDPlanet.Water_world)
            {
                iconName = "WTRv7"; // fallback

                if (AtmosphereID == EDAtmosphereType.No_atmosphere)
                {
                    iconName = "WTRv10"; // kindly provided by CMDR CompleteNOOB
                }
                else
                {
                    if (AtmosphereID == EDAtmosphereType.Carbon_dioxide)
                    {
                        if (st < 260)
                            iconName = "WTRv6";
                        else if (st < 280)
                            iconName = "WTRv5";
                        else if (st < 300)
                            iconName = "WTRv7";
                        else if (st < 400)
                            iconName = "WTRv2";
                        else
                            iconName = "WTRv11"; // kindly provided by CMDR Eahlstan
                    }
                    else if (AtmosphereID == EDAtmosphereType.Ammonia)
                    {
                        if (nTidalLock == true)
                            iconName = "WTRv12"; // kindly provided by CMDR Eahlstan
                        else
                        {
                            if (st < 275)
                                iconName = "WTRv1";
                            else if (st < 350)
                                iconName = "WTRv13"; // kindly provided by CMDR Eahlstan
                            else if (st < 380)
                                iconName = "WTRv9"; // kindly provided by CMDR CompleteNOOB
                            else
                                iconName = "WTRv4";
                        }
                    }
                    else if (AtmosphereID == EDAtmosphereType.Nitrogen)
                    {
                        if (st < 250)
                            iconName = "WTRv3";
                        else
                            iconName = "WTRv8";
                    }
                    else
                        iconName = "WTRv7"; // fallback                                
                }
            }

            System.Diagnostics.Debug.WriteLine(PlanetTypeID + ": " + iconName);
            return EDDiscovery.Icons.IconSet.GetIcon($"Bodies.Planets.Terrestrial.{iconName}");
        }

        static public System.Drawing.Image GetPlanetImageNotScanned()
        {
            return EDDiscovery.Icons.IconSet.GetIcon($"Bodies.Unknown");
        }

        static public System.Drawing.Image GetMoonImageNotScanned()
        {
            return EDDiscovery.Icons.IconSet.GetIcon($"Bodies.Unknown");
        }

        public double GetMaterial(string v)
        {
            if (Materials == null)
                return 0.0;

            if (!Materials.ContainsKey(v.ToLowerInvariant()))
                return 0.0;

            return Materials[v.ToLowerInvariant()];
        }

        public double? GetAtmosphereComponent(string c)
        {
            if (!HasAtmosphericComposition)
                return null;

            if (!AtmosphereComposition.ContainsKey(c))
                return 0.0;

            return AtmosphereComposition[c];

        }

        public double? GetCompositionPercent(string c)
        {
            if (!HasPlanetaryComposition)
                return null;

            if (!PlanetComposition.ContainsKey(c))
                return 0.0;

            return PlanetComposition[c] * 100;
        }

        public bool IsStarNameRelated(string starname, string designation = null, long? sysaddr = null)
        {
            if (StarSystem != null && SystemAddress != null && sysaddr != null)
            {
                return starname.Equals(StarSystem, StringComparison.InvariantCultureIgnoreCase) && sysaddr == SystemAddress;
            }

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

        public string IsStarNameRelatedReturnRest(string starname, long? sysaddr = null)          // null if not related, else rest of string
        {
            string designation = BodyDesignation ?? BodyName;
            string desigrest = null;

            if (StarSystem != null && SystemAddress != null && sysaddr != null)
            {
                if (starname != StarSystem || sysaddr != SystemAddress)
                {
                    return null;
                }

                desigrest = designation;
            }

            if (designation.Length >= starname.Length)
            {
                string s = designation.Substring(0, starname.Length);
                if (starname.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                    desigrest = designation.Substring(starname.Length).Trim();
            }

            return desigrest;
        }

        // Habitable zone calculations, formula cribbed from JackieSilver's HabZone Calculator with permission
        private double DistanceForBlackBodyTemperature(double targetTemp)
        {
            double top = Math.Pow(nRadius.Value, 2.0) * Math.Pow(nSurfaceTemperature.Value, 4.0);
            double bottom = 4.0 * Math.Pow(targetTemp, 4.0);
            double radius_metres = Math.Pow(top / bottom, 0.5);
            return radius_metres / oneLS_m;
        }

		private double DistanceForNoMaxTemperatureBody(double radius)
		{
			return radius / oneLS_m;
		}

        #endregion

        #region Estimated Value

        private void EstimateScanValue()       // call to estimate scan value given these parameters. Updates EstimatedValue
        {
            // see https://forums.frontier.co.uk/showthread.php/232000-Exploration-value-formulae/ for detail

            if (EventTimeUTC < new DateTime(2017, 4, 11, 12, 0, 0, 0, DateTimeKind.Utc))
            {
                EstimatedValueBase = EstimatedValueED22();
                return;
            }

            if (EventTimeUTC < new DateTime(2018, 12, 11, 9, 0, 0, DateTimeKind.Utc))
            {
                EstimatedValueBase = EstimatedValue32();
                return;
            }

            // 3.3 onwards

            //System.Diagnostics.Debug.WriteLine("Scan calc " + mapped + " ef " + efficient + " Current " + EstimatedValue);

            double kValue;

            if (IsStar)
            {
                switch (StarTypeID)
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
                        kValue = 14057;
                        break;

                    case EDStar.N:
                    case EDStar.H:
                        kValue = 22628;
                        break;

                    case EDStar.SuperMassiveBlackHole:
                        // this is applying the same scaling to the 3.2 value as a normal black hole, not confirmed in game
                        kValue = 33.5678;   
                        break;

                    default:
                        kValue = 1200;
                        break;
                }

                EstimatedValueBase = (int)StarValue32And33(kValue, nStellarMass.HasValue ? nStellarMass.Value : 1.0);
            }
            else
            {
                EstimatedValueBase = 0;

                if (PlanetClass != null)  //Asteroid belt is null
                {
                    switch (PlanetTypeID)
                    {
                        case EDPlanet.Metal_rich_body:
                            // CFT value is scaled same as WW/ELW from 3.2, not confirmed in game
                            // They're like hen's teeth anyway....
                            kValue = 21790;
                            if (Terraformable) kValue += 65631;
                            break;
                        case EDPlanet.Ammonia_world:
                            kValue = 96932;
                            break;
                        case EDPlanet.Sudarsky_class_I_gas_giant:
                            kValue = 1656;
                            break;
                        case EDPlanet.Sudarsky_class_II_gas_giant:
                        case EDPlanet.High_metal_content_body:
                            kValue = 9654;
                            if (Terraformable) kValue += 100677;
                            break;
                        case EDPlanet.Water_world:
                            kValue = 64831;
                            if (Terraformable) kValue += 116295;
                            break;
                        case EDPlanet.Earthlike_body:
                            // Always terraformable so WW + bonus
                            kValue = 64831 + 116295;
                            break;
                        default:
                            kValue = 300;
                            if (Terraformable) kValue += 93328;
                            break;
                    }

                    double mass = nMassEM.HasValue ? nMassEM.Value : 1.0;
                    double effmapped = 1.25;
                    double firstdiscovery = 2.6;

                    double basevalue = PlanetValue33(kValue, mass);

                    EstimatedValueBase = (int)basevalue;

                    EstimatedValueFirstDiscovered = (int)(basevalue*firstdiscovery);

                    EstimatedValueFirstDiscoveredFirstMapped = (int)(basevalue * firstdiscovery * 3.699622554);
                    EstimatedValueFirstDiscoveredFirstMappedEfficiently = (int)(basevalue * firstdiscovery * 3.699622554 * effmapped);

                    EstimatedValueFirstMapped = (int)(basevalue * 8.0956);
                    EstimatedValueFirstMappedEfficiently = (int)(basevalue * 8.0956 * effmapped);
                }
            }
        }

        private double StarValue32And33(double k, double m)
        {
            return k + (m * k / 66.25);
        }

        private double PlanetValue33(double k, double m)
        {
            const double q = 0.56591828;
            return Math.Max((k + (k * Math.Pow(m, 0.2) * q)), 500);
        }

        #endregion

        #region ED 3.2 values

        private int EstimatedValue32()
        {
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

                return (int)StarValue32And33(kValue, nStellarMass.HasValue ? nStellarMass.Value : 1.0);
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

                int val = (int)PlanetValueED32(kValue, mass);
                if (Terraformable || PlanetTypeID == EDPlanet.Earthlike_body)
                {
                    val += (int)PlanetValueED32(kBonus, mass);
                }

                return val;
            }
        }

        private double PlanetValueED32(double k, double m)
        {
            return k + (3 * k * Math.Pow(m, 0.199977) / 5.3);
        }

        #endregion

        #region ED 22 

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
                        if (TerraformState != null && TerraformState.ToLowerInvariant().Equals("terraformable"))
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
                        if (TerraformState != null && TerraformState.ToLowerInvariant().Equals("terraformable"))
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

    #endregion

    public class ScansAreForSameBody : EqualityComparer<JournalScan>
    {
        public override bool Equals(JournalScan x, JournalScan y)
        {
            return x.BodyName == y.BodyName;
        }

        public override int GetHashCode(JournalScan obj)
        {
            return obj.BodyName.GetHashCode();
        }
    }
}


