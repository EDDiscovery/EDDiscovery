using EDDiscovery.DB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
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
    //
    // Rings properties
    //•	Name
    //•	RingClass
    //•	MassMT - ie in megatons
    //•	InnerRad
    //•	OuterRad
    //
    public class JournalScan : JournalEntry
    {
        private const double solarRadius_m = 695700000;
        private const double oneAU_m = 149597870000;
        private const double oneDay_s = 86400;
        private const double oneMoon_MT = 73420000000000;

        public JournalScan(JObject evt) : base(evt, JournalTypeEnum.Scan)
        {
            BodyName = JSONHelper.GetStringDef(evt["BodyName"]);
            StarType = JSONHelper.GetStringNull(evt["StarType"]);
            DistanceFromArrivalLS = JSONHelper.GetDouble(evt["DistanceFromArrivalLS"]);

            nAge = JSONHelper.GetDoubleNull(evt["Age_MY"]);
            nStellarMass = JSONHelper.GetDoubleNull(evt["StellarMass"]);
            nRadius = JSONHelper.GetDoubleNull(evt["Radius"]);
            nAbsoluteMagnitude = JSONHelper.GetDoubleNull(evt["AbsoluteMagnitude"]);
            nRotationPeriod = JSONHelper.GetDoubleNull(evt["RotationPeriod"]);

            nOrbitalPeriod = JSONHelper.GetDoubleNull(evt["OrbitalPeriod"]);
            nSemiMajorAxis = JSONHelper.GetDoubleNull(evt["SemiMajorAxis"]);
            nEccentricity = JSONHelper.GetDoubleNull(evt["Eccentricity"]);
            nOrbitalInclination = JSONHelper.GetDoubleNull(evt["OrbitalInclination"]);
            nPeriapsis = JSONHelper.GetDoubleNull(evt["Periapsis"]);

            Rings = evt["Rings"]?.ToObject<StarPlanetRing[]>();

            nTidalLock = JSONHelper.GetBoolNull(evt["TidalLock"]);
            TerraformState = JSONHelper.GetStringNull(evt["TerraformState"]);
            PlanetClass = JSONHelper.GetStringNull(evt["PlanetClass"]);
            Atmosphere = JSONHelper.GetStringNull(evt["Atmosphere"]);
            Volcanism = JSONHelper.GetStringNull(evt["Volcanism"]);
            nMassEM = JSONHelper.GetDoubleNull(evt["MassEM"]);
            nSurfaceGravity = JSONHelper.GetDoubleNull(evt["SurfaceGravity"]);
            nSurfaceTemperature = JSONHelper.GetDoubleNull(evt["SurfaceTemperature"]);
            nSurfacePressure = JSONHelper.GetDoubleNull(evt["SurfacePressure"]);
            nLandable = JSONHelper.GetBoolNull(evt["Landable"]);

            Materials = evt["Materials"]?.ToObject<Dictionary<string, double>>();
        }

        public string BodyName { get; set; }
        public double DistanceFromArrivalLS { get; set; }
        public string StarType { get; set; }                            // null if no StarType
        public bool IsStar { get { return !String.IsNullOrEmpty(StarType); } }

        public double? nAge { get; set; }
        public double? nStellarMass { get; set; }
        public double? nRadius { get; set; }
        public double? nAbsoluteMagnitude { get; set; }
        public double? nRotationPeriod { get; set; }

        public double? nOrbitalPeriod { get; set; }
        public double? nSemiMajorAxis;
        public double? nEccentricity;
        public double? nOrbitalInclination;
        public double? nPeriapsis;

        public class StarPlanetRing
        {
            public string Name;
            public string RingClass;
            public double MassMT;
            public double InnerRad;
            public double OuterRad;
        }

        public StarPlanetRing[] Rings { get; set; }
        public bool HasRings { get { return Rings != null && Rings.Length > 0; } }

        public bool? nTidalLock { get; set; }
        public string TerraformState { get; set; }
        public string PlanetClass { get; set; }
        public string Atmosphere { get; set; }
        public string Volcanism { get; set; }
        public double? nMassEM { get; set; } // not in description of event
        public double? nSurfaceGravity { get; set; } // not in description of event
        public double? nSurfaceTemperature { get; set; }
        public double? nSurfacePressure { get; set; }
        public bool? nLandable { get; set; }

        public bool IsLandable { get { return nLandable.HasValue && nLandable.Value; } }
        public bool HasMaterials { get { return Materials != null && Materials.Any(); } }
        public Dictionary<string, double> Materials { get; set; }

        //public string MaterialsString { get { return jEventData["Materials"].ToString(); } }

        

        public string DisplayString(bool printbodyname = true, int indent = 0)
        {
            string inds = new string(' ' , indent);

            StringBuilder scanText = new StringBuilder();

            scanText.Append(inds);

            if (printbodyname)
                scanText.AppendFormat("{0}\n\n", BodyName);

            if (IsStar)
            {
                scanText.AppendFormat(GetStarTypeImage().Item2);
            }
            else
            {
                scanText.AppendFormat("{0}", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                        ToTitleCase(PlanetClass.ToLower()).Replace("Ii", "II").Replace("Ii", "II").Replace("Iv", "IV"));
            }

            if (PlanetClass != null && !PlanetClass.ToLower().Contains("gas"))
            {
                scanText.AppendFormat((Atmosphere == null || Atmosphere == String.Empty) ? ", No Atmosphere" : ( ", " + 
                                                            System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Atmosphere.ToLower()) )
                                     );
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
                if ( IsStar )
                    scanText.AppendFormat("Solar Radius: {0:0.00} Sols\n", (nRadius.Value / solarRadius_m));
                else
                    scanText.AppendFormat("Body Radius: {0:0.00}km\n", (nRadius.Value / 1000));
            }

            if (nSurfaceTemperature.HasValue)
                scanText.AppendFormat("Surface Temp: {0}K\n", nSurfaceTemperature.Value.ToString("N0"));

            if (nSurfaceGravity.HasValue)
                scanText.AppendFormat("Gravity: {0:0.0}g\n", nSurfaceGravity.Value / 9.8);

            if (nSurfacePressure.HasValue && nSurfacePressure.Value > 0.00 && !PlanetClass.ToLower().Contains("gas"))
                scanText.AppendFormat("Surface Pressure: {0} Atmospheres\n", (nSurfacePressure.Value / 100000).ToString("N2"));

            if (Volcanism != null)
                scanText.AppendFormat("Volcanism: {0}\n", Volcanism == String.Empty ? "No Volcanism" : System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                                                                            ToTitleCase(Volcanism.ToLower()));

            if (DistanceFromArrivalLS > 0)
                scanText.AppendFormat("Distance from Arrival Point {0:N1}ls\n", DistanceFromArrivalLS);

            if (nOrbitalPeriod.HasValue && nOrbitalPeriod > 0)
                scanText.AppendFormat("Orbital Period: {0} days\n", (nOrbitalPeriod.Value / oneDay_s).ToString("N1"));

            if (nSemiMajorAxis.HasValue)
            {
                if ( IsStar || nSemiMajorAxis.Value > oneAU_m/10 )
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

            if (nRotationPeriod.HasValue)
                scanText.AppendFormat("Rotation Period: {0} days\n", (nRotationPeriod.Value / oneDay_s).ToString("N1"));

            if (nTidalLock.HasValue && nTidalLock.Value)
                scanText.Append("Tidally locked\n");

            if ( TerraformState != null && TerraformState == "Terraformable") 
                scanText.Append("Candidate for terraforming\n");

            if (HasRings)
            {
                scanText.Append("\n");
                if (IsStar)
                {
                    scanText.AppendFormat("Belt{0}", Rings.Count() == 1 ? ":" : "s:");
                    for (int i = 0; i < Rings.Length; i++)
                        scanText.Append("\n" + RingInformation(i, 1.0/oneMoon_MT, " Moons"));
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

            if (scanText.Length > 0 && scanText[scanText.Length - 1] == '\n')
                scanText.Remove(scanText.Length - 1, 1);

            return scanText.ToNullSafeString().Replace("\n", "\n" + inds);
        }

        public string DisplayMaterials(int indent = 0)
        {
            StringBuilder scanText = new StringBuilder();
            string indents = new string(' ', indent);

            scanText.Append("Materials:\n");
            foreach (KeyValuePair<string, double> mat in Materials)
            {
                EDDiscovery2.DB.MaterialCommodities mc = EDDiscovery2.DB.MaterialCommodities.GetCachedMaterial(mat.Key);
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

        public string RingInformationMoons(int ringno)
        {
            return RingInformation(ringno, 1 / oneMoon_MT, " Moons");
        }

        public string RingInformation(int ringno, double scale = 1, string scaletype = " MT")
        {
            StarPlanetRing ring = Rings[ringno];
            StringBuilder scanText = new StringBuilder();
            scanText.AppendFormat("  {0} ({1})\n", ring.Name, DisplayStringFromRingClass(ring.RingClass));
            scanText.AppendFormat("  Mass: {0}{1}\n", (ring.MassMT * scale).ToString("N4"),scaletype );
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

        internal string DisplayStringFromRingClass(string ringClass)
        {
            switch (ringClass)
            {
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
                    return ringClass.Replace("eRingClass_","");
            }
        }


        public Tuple<System.Drawing.Image, string> GetStarTypeImage()           // give image and description to star class
        {
            System.Drawing.Image ret = EDDiscovery.Properties.Resources.Star_K1IV;

            switch (StarType.ToLower())       // see journal, section 11.2
            {
                case "o":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.O, string.Format("Luminous Hot Main Sequence star", StarType));

                case "b":
                    // also have an B1V
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.B6V_Blueish, string.Format("Luminous Blue Main Sequence star", StarType));

                case "a":
                    // also have an A3V..
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.A9III_White, string.Format("Bluish-White Main Sequence star", StarType));

                case "f":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.F5VAB, string.Format("White Main Sequence star", StarType));

                case "g":
                    // also have a G8V
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.G1IV, string.Format("Yellow Main Sequence star", StarType));

                case "k":
                    // also have a K0V
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.Star_K1IV, string.Format("Orange Main Sequence {0} star", StarType));
                case "m":
                    // also have a M1VA
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.M5V, string.Format("Red Main Sequence {0} star", StarType));

                // dwarfs
                case "l":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.L3V, string.Format("Dark Red Non Main Sequence {0} star", StarType));
                case "t":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.T4V, string.Format("Methane Dwarf star", StarType));
                case "y":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.Y2, string.Format("Brown Dwarf star", StarType));

                // proto stars
                case "aebe":    // seen in logs
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "Herbig Ae/Be");
                case "tts":     // seen in logs
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "T Tauri");

                // wolf rayet
                case "w":
                case "wn":
                case "wne": // wiki not journal may not be in game
                case "wnl": // wiki not journal may not be in game
                case "wnc":
                case "wc":
                case "wce": // wiki not journal may not be in game
                case "wcl": // wiki not journal may not be in game
                case "wo":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, string.Format("Wolf-Rayet {0} star", StarType));

                // Carbon
                case "cs":
                case "c":
                case "cn":
                case "cj":
                case "chd":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.C7III, string.Format("Carbon {0} star", StarType));

                case "ms": //seen in log
                case "s":   // seen in log
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, string.Format("Unknown Type {0} star", StarType));

                // white dwarf
                case "d":
                case "da":
                case "dab":
                case "dao":
                case "daz":
                case "dav":
                case "db":
                case "dbz":
                case "dbv":
                case "do":
                case "dov":
                case "dq":
                case "dc":
                case "dcv":
                case "dx":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DA6VII_White, string.Format("White Dwarf ({0}) star", StarType));

                case "n":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.Neutron_Star, "Neutron Star");

                case "h":
                    // currently speculative, not confirmed with actual data...
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.Black_Hole, "Black Hole");

                case "x":
                    // currently speculative, not confirmed with actual data... in journal
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "Exotic");

                // Journal.. really?  need evidence these actually are formatted like this.

                case "supermassiveblackhole":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.Black_Hole, "Super Massive Black Hole");
                case "a bluewhitesupergiant":   
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "Blue White Giant");
                case "f whitesupergiant":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "F White Super Giant");
                case "m redsupergiant":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "M Red Super Giant");
                case "m redgiant":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "M Red Giant");
                case "k orangegiant":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "K Orange Giant");
                case "rogueplanet":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "Rouge Planet");

                default:
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, string.Format("Class {0} star\n", StarType.Replace("_", " ")));
            }
        }

        static public System.Drawing.Image GetStarImageNotScanned()
        {
            return EDDiscovery.Properties.Resources.Globe_yellow;
        }

        public System.Drawing.Image GetPlanetClassImage()
        {
            string name = PlanetClass.ToLower();

            if (name.Contains("gas"))
            {
                if (name.Contains("helium"))
                    return EDDiscovery.Properties.Resources.Helium_Rich_Gas_Giant1;
                else if (name.Contains("water"))
                    return EDDiscovery.Properties.Resources.Gas_giant_water_based_life_Brown3;
                else if (name.Contains("ammonia"))
                    return EDDiscovery.Properties.Resources.Gas_giant_ammonia_based_life1;
                else if (name.Contains("iv"))
                    return EDDiscovery.Properties.Resources.Class_I_Gas_Giant_Brown2;               // MISSING.
                else if (name.Contains("iii"))
                    return EDDiscovery.Properties.Resources.Class_III_Gas_Giant_Blue3;
                else if (name.Contains("ii"))
                    return EDDiscovery.Properties.Resources.Class_II_Gas_Giant_Sand1;
                else if (name.Contains("v"))
                    return EDDiscovery.Properties.Resources.Class_I_Gas_Giant_Brown2;               // MISSING.
                else
                    return EDDiscovery.Properties.Resources.Class_I_Gas_Giant_Brown2;
            }
            else if (name.Contains("ammonia"))
                return EDDiscovery.Properties.Resources.Ammonia_Brown;      // also have orange.
            else if (name.Contains("earth"))
                return EDDiscovery.Properties.Resources.Earth_Like_Standard;
            else if (name.Contains("ice"))
                return EDDiscovery.Properties.Resources.Rocky_Ice_World_Sol_Titan;
            else if (name.Contains("icy"))
                return EDDiscovery.Properties.Resources.Icy_Body_Greenish1;
            else if (name.Contains("water"))
            {
                if (name.Contains("giant"))
                    return EDDiscovery.Properties.Resources.Water_Giant1;
                else
                    return EDDiscovery.Properties.Resources.Water_World_Poles_Cloudless4;
            }
            else if (name.Contains("metal"))
            {
                if (name.Contains("rich"))
                    return EDDiscovery.Properties.Resources.metal_rich;
                else if (nSurfaceTemperature > 400)
                    return EDDiscovery.Properties.Resources.High_metal_content_world_Lava1;
                else if (nSurfaceTemperature > 250)
                    return EDDiscovery.Properties.Resources.High_metal_content_world_Mix3;
                else
                    return EDDiscovery.Properties.Resources.High_metal_content_world_Orange8;
            }
            else if (name.Contains("rocky"))
                return EDDiscovery.Properties.Resources.Rocky_Body_Sand2;
            else
                return EDDiscovery.Properties.Resources.Globe;
        }

        static public System.Drawing.Image GetPlanetImageNotScanned()
        {
            return EDDiscovery.Properties.Resources.Globe;
        }

        static public System.Drawing.Image GetMoonImageNotScanned()
        {
            return EDDiscovery.Properties.Resources.Icy_Body_Greenish1;
        }

        internal double GetMaterial(string v)
        {
            if (Materials == null)
                return 0.0;

            if (!Materials.ContainsKey(v.ToLower()))
                return 0.0;

            return Materials[v.ToLower()];
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.scan; } }

        public bool IsStarNameRelated(string starname)
        {
            if (BodyName.Length >= starname.Length)
            {
                string s = BodyName.Substring(0, starname.Length);
                return starname.Equals(s, StringComparison.InvariantCultureIgnoreCase);
            }
            else
                return false;
        }

        public string IsStarNameRelatedReturnRest(string starname)          // null if not related, else rest of string
        {
            if (BodyName.Length >= starname.Length)
            {
                string s = BodyName.Substring(0, starname.Length);
                if (starname.Equals(s, StringComparison.InvariantCultureIgnoreCase))
                    return BodyName.Substring(starname.Length).Trim();
            }

            return null;
        }
    }



    public class StarScan
    {
        Dictionary<Tuple<string, long>, SystemNode> scandata = new Dictionary<Tuple<string, long>, SystemNode>();

        public class SystemNode
        {
            public EDDiscovery2.DB.ISystem system;
            public SortedList<string, ScanNode> starnodes;
        };

        public enum ScanNodeType { star , barycentre, planet , moon, submoon, starbelt, rings };

        public class ScanNode
        {
            public ScanNodeType type;
            public string fullname;                 // full name
            public string ownname;                  // own name              
            public SortedList<string, ScanNode> children;         // kids

            public JournalScan scandata;            // can be null if no scan, its a place holder.
        };

        public SystemNode FindSystem(EDDiscovery2.DB.ISystem sys)
        {
            Tuple<string, long> withedsm = new Tuple<string, long>(sys.name, sys.id_edsm);
            Tuple<string, long> withoutedsm = new Tuple<string, long>(sys.name, 0);

            if (scandata.ContainsKey(withedsm))         // if with edsm (if id_edsm=0, then thats okay)
                return scandata[withedsm];

            if (scandata.ContainsKey(withoutedsm))  // if we now have an edsm id, see if we have one without it 
                return scandata[withoutedsm];

            return null;
        }

        public bool Process(JournalScan sc, EDDiscovery2.DB.ISystem sys)           // FALSE if you can't process it
        {
            Tuple<string, long> withedsm = new Tuple<string, long>(sys.name, sys.id_edsm);
            Tuple<string, long> withoutedsm = new Tuple<string, long>(sys.name, 0);

            SystemNode sn;
            if (scandata.ContainsKey(withedsm))         // if with edsm (if id_edsm=0, then thats okay)
                sn = scandata[withedsm];
            else if (scandata.ContainsKey(withoutedsm))  // if we now have an edsm id, see if we have one without it 
            {
                sn = scandata[withoutedsm];

                if (sys.id_edsm != 0)             // yep, replace
                {
                    scandata.Remove(new Tuple<string, long>(sys.name, 0));
                    scandata.Add(new Tuple<string, long>(sys.name, sys.id_edsm), sn);
                }
            }
            else
            {
                sn = new SystemNode() { system = sys, starnodes = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>()) };
                scandata.Add(new Tuple<string, long>(sys.name, sys.id_edsm), sn);
            }

            if (!String.IsNullOrEmpty(sc.StarType))
            {
                return FindOrAddStar(sn, sc, sys.name);
            }
            else
            {
                return AddUpdatePlanetMoon(sn, sc, sys.name);
            }
        }

        bool FindOrAddStar(SystemNode sn, JournalScan sc, string starname)
        {
            string rest = sc.IsStarNameRelatedReturnRest(starname);             // so a star line Eol Prou LW-L c8 - 306 A, it it there?

            if (rest != null)           // must have a relationship.. otherwise we are getting scans of stuff not in our system
            {
                string designator = (rest.Length > 0) ? rest : "MAIN";          // if no more text, its the MAIN star, else use it direct

                ScanNode s = FindOrAdd(sn, starname, designator, ScanNodeType.star);
                s.scandata = sc;
                return true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to add star " + sc.BodyName + " name not related to " + starname);
            }
            return false;
        }

        ScanNode FindOrAdd(SystemNode sn, string starname , string designator , ScanNodeType ty)
        {
            ScanNode star;
            if (!sn.starnodes.TryGetValue(designator, out star))
            {
                bool nodesignator = designator.Equals("MAIN");          // MAIN is used as the key name for stars without designators..  but is not stored in ownname or fullname

                star = new ScanNode()
                {
                    ownname = nodesignator ? starname : designator,
                    fullname = starname + (nodesignator ? "" : (" " + designator)),
                    scandata = null,
                    children = null,
                    type = ty
                };

                sn.starnodes.Add(designator, star);
                //System.Diagnostics.Debug.WriteLine("Added star " + star.fullname + " '" + star.ownname + "'" + " under '" + designator + "' type " + ty);
            }

            return star;
        }

        bool AddUpdatePlanetMoon(SystemNode sn, JournalScan sc, string starname)
        {
            // handle Earth, starname = Sol
            // handle Eol Prou LW-L c8-306 A 4 a and Eol Prou LW-L c8-306
            // handle Colonia 4 , starname = Colonia, planet 4
            // handle Aurioum B A BELT
            // Kyloasly OY-Q d5-906 13 1

            List<string> elements;

            string rest = sc.IsStarNameRelatedReturnRest(starname);
            ScanNodeType starscannodetype = ScanNodeType.star;          // presuming..       

            if (rest != null)           // must have a relationship.. otherwise we are getting scans of stuff not in our system
            {
                if ( rest.Length > 0 )
                {
                    elements = rest.Split(' ').ToList();

                    if (char.IsDigit(elements[0][0]))       // if digits, planet number, no star designator
                        elements.Insert(0, "MAIN");         // no star designator, main star, add MAIN
                    else if (elements[0].Length > 1)        // designator, is is multiple chars.. 
                        starscannodetype = ScanNodeType.barycentre;
                }
                else
                {
                    elements = new List<string>();          // only 1 item, will fail later..
                    elements.Add("MAIN");
                }
            }
            else
            {
                elements = sc.BodyName.Split(' ').ToList();     // not related in any way (earth) so assume all bodyparts, and 
                elements.Insert(0, "MAIN");                     // insert the MAIN designator as the star designator
            }

            if (elements.Count >= 2)                            // must have a star designator, and at least a planet..
            {
                ScanNode star = FindOrAdd(sn, starname, elements[0] , starscannodetype );  // find or add the star...

                ScanNode planetscan;

                if (star.children == null || !star.children.TryGetValue(elements[1], out planetscan))
                {
                    if (star.children == null)
                        star.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());

                    planetscan = new ScanNode() { ownname = elements[1], fullname = star.fullname + " " + elements[1], scandata = null, children = null, type = ScanNodeType.planet };
                    star.children.Add(elements[1], planetscan);
                }

                if (elements.Count >= 3)          // star, planet, moon
                {
                    ScanNode moonscan;

                    if (planetscan.children == null || !planetscan.children.TryGetValue(elements[2], out moonscan))
                    {
                        if (planetscan.children == null)
                            planetscan.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());

                        moonscan = new ScanNode() { ownname = elements[2], fullname = star.fullname + " " + elements[1] + " " + elements[2], scandata = null, children = null, type = ScanNodeType.moon };
                        planetscan.children.Add(elements[2], moonscan);
                    }

                    if (elements.Count >= 4)          // star, planet, moon, moon
                    {
                        ScanNode submoonscan;

                        if (moonscan.children == null || !moonscan.children.TryGetValue(elements[3], out submoonscan))
                        {
                            if (moonscan.children == null)
                                moonscan.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());

                            submoonscan = new ScanNode() { ownname = elements[3], fullname = star.fullname + " " + elements[1] + " " + elements[2] + " " + elements[3], scandata = null, children = null, type = ScanNodeType.submoon };
                            moonscan.children.Add(elements[3], submoonscan);
                        }

                        if (elements.Count == 4)            // okay, need only 4 elements now.. if not, we have not coped..
                            submoonscan.scandata = sc;
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to add system " + sc.BodyName + " too long");
                            return false;
                        }
                    }
                    else
                    {
                        moonscan.scandata = sc;
                        //System.Diagnostics.Debug.WriteLine("Added moon scan '{0}' to {1} to {2}", elements[2], elements[1], elements[0]);
                    }
                }
                else
                {
                    planetscan.scandata = sc;
                    //System.Diagnostics.Debug.WriteLine("Added planet scan '{0}' to {1}", elements[1], elements[0]);
                }

                return true;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Failed to add system " + sc.BodyName + " not enough elements");
                return false;
            }
        }

        public string SystemDescription(EDDiscovery2.DB.ISystem sys)
        {
            SystemNode sn = FindSystem(sys);
            string ret = null;

            if (sn != null)
            {
                ret = "";

                foreach (StarScan.ScanNode starnode in sn.starnodes.Values)        // always has scan nodes
                {
                    ret += string.Format("Star Name " + starnode.fullname) + Environment.NewLine;

                    if (starnode.scandata != null)
                        ret += starnode.scandata.DisplayString(false, 4);

                    if (starnode.children != null)
                    {
                        foreach (StarScan.ScanNode planetnode in starnode.children.Values)
                        {
                            ret += string.Format("    Planet " + planetnode.fullname) + Environment.NewLine;
                            if (planetnode.scandata != null)
                                ret += planetnode.scandata.DisplayString(false, 8);

                            if (planetnode.children != null)
                            {
                                foreach (StarScan.ScanNode moonnode in planetnode.children.Values)
                                {
                                    ret += string.Format("        Moon " + moonnode.fullname) + Environment.NewLine;
                                    if (moonnode.scandata != null)
                                        ret += moonnode.scandata.DisplayString(false, 12);
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private class DuplicateKeyComparer<TKey> : IComparer<string> where TKey : IComparable      // special compare for sortedlist
        {
            public int Compare(string x, string y)
            {
                if (x.Length < y.Length)
                    return -1;
                else if (x.Length > y.Length)
                    return 1;
                else
                    return x.CompareTo(y);
            }
        }

    }
}

