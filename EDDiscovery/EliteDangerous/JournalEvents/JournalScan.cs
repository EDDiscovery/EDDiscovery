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

        public JournalScan(JObject evt ) : base(evt, JournalTypeEnum.Scan)
        {
            BodyName = JSONHelper.GetStringDef(evt["BodyName"]);
            StarType = JSONHelper.GetStringDef(evt["StarType"]);
            StellarMass = JSONHelper.GetDoubleNull(evt["StellarMass"]);
            Radius = JSONHelper.GetDoubleNull(evt["Radius"]);
            AbsoluteMagnitude = JSONHelper.GetDoubleNull(evt["AbsoluteMagnitude"]);
            RotationPeriod = JSONHelper.GetDouble(evt["RotationPeriod"]);
            Age = JSONHelper.GetDouble(evt["Age_MY"]);
            Rings = evt["Rings"]?.ToObject<PlanetRing[]>();
            DistanceFromArrivalLS = JSONHelper.GetDouble(evt["DistanceFromArrivalLS"]);

            TidalLock = JSONHelper.GetBool(evt["TidalLock"]);
            TerraformState = JSONHelper.GetStringDef(evt["TerraformState"]);
            PlanetClass = JSONHelper.GetStringDef(evt["PlanetClass"]);
            Atmosphere = JSONHelper.GetStringDef(evt["Atmosphere"]);
            Volcanism = JSONHelper.GetStringDef(evt["Volcanism"]);
            MassEM = JSONHelper.GetDoubleNull(evt["MassEM"]);
            SurfaceGravity = JSONHelper.GetDoubleNull(evt["SurfaceGravity"]);
            SurfaceTemperature = JSONHelper.GetDoubleNull(evt["SurfaceTemperature"]);
            SurfacePressure = JSONHelper.GetDoubleNull(evt["SurfacePressure"]);
            Landable = JSONHelper.GetBoolNull(evt["Landable"]);
            Materials = evt["Materials"]?.ToObject<Dictionary<string, double>>();

            SemiMajorAxis = JSONHelper.GetDouble(evt["SemiMajorAxis"]);
            Eccentricity = JSONHelper.GetDouble(evt["Eccentricity"]);
            OrbitalInclination = JSONHelper.GetDouble(evt["OrbitalInclination"]);
            Periapsis = JSONHelper.GetDouble(evt["Periapsis"]);
            OrbitalPeriod = JSONHelper.GetDouble(evt["OrbitalPeriod"]);


        }

        public string BodyName { get; set; }
        public double DistanceFromArrivalLS { get; set; }
        public string StarType { get; set; }
        public double? StellarMass { get; set; }
        public double? Radius { get; set; }
        public double? AbsoluteMagnitude { get; set; }
        public double OrbitalPeriod { get; set; }
        public double RotationPeriod { get; set; }
        public PlanetRing[] Rings { get; set; }

        public bool TidalLock { get; set; }
        public string TerraformState { get; set; }
        public string PlanetClass { get; set; }
        public string Atmosphere { get; set; }
        public string Volcanism { get; set; }
        public double? MassEM { get; set; } // not in description of event
        public double? SurfaceGravity { get; set; } // not in description of event
        public double? SurfaceTemperature { get; set; }
        public double? SurfacePressure { get; set; }
        public bool? Landable { get; set; }
        public Dictionary<string, double> Materials { get; set; }

        public string MaterialsString { get { return jEventData["Materials"].ToString(); } }
        
        public double Age { get; set; }

        public double SemiMajorAxis;
        public double Eccentricity;
        public double OrbitalInclination;
        public double Periapsis;

        public string DisplayString()
        {
            StringBuilder scanText = new StringBuilder();
            scanText.AppendFormat("{0}\n", BodyName);
            scanText.Append("\n");
            if (!String.IsNullOrEmpty(StarType))
            {
                //star
                switch (StarType)
                {
                    case "N":
                        scanText.Append("Neutron Star\n");
                        break;
                    case "AeBe":
                        scanText.Append("Herbig Ae/Be\n");
                        break;
                    case "H":
                        // currently speculative, not confirmed with actual data...
                        scanText.Append("Black Hole\n");
                        break;
                    case "TTS":
                        scanText.Append("T Tauri\n");
                        break;
                    case "CS":
                    case "C":
                    case "CN":
                    case "CJ":
                    case "CHd":
                        scanText.AppendFormat("Carbon ({0}) star\n", StarType);
                        break;
                    case "W":
                    case "WN":
                    case "WNC":
                    case "WC":
                    case "WO":
                        scanText.AppendFormat("Wolf-Rayet ({0}) star\n", StarType);
                        break;
                    case "D":
                    case "DA":
                    case "DAB":
                    case "DAO":
                    case "DAZ":
                    case "DAV":
                    case "DB":
                    case "DBZ":
                    case "DBV":
                    case "DO":
                    case "DOV":
                    case "DQ":
                    case "DC":
                    case "DCV":
                    case "DX":
                        scanText.AppendFormat("White Dwarf ({0}) star\n", StarType);
                        break;
                    default:
                        scanText.AppendFormat("Class {0} star\n", StarType.Replace("_", " ").Replace("Super", " Super").Replace("Giant", " Giant"));
                        break;
                }
                scanText.AppendFormat("Age: {0} million years\n", Age.ToString("##,####"));
                scanText.AppendFormat("Solar Masses: {0}\n", StellarMass);
                scanText.AppendFormat("Solar Radius: {0}\n", (Radius.Value / solarRadius_m).ToString("0.0####"));
                scanText.AppendFormat("Surface Temp: {0}K\n", SurfaceTemperature.Value.ToString("#,###,###"));
                if (OrbitalPeriod > 0)
                {
                    scanText.AppendFormat("Orbital Period: {0}D\n", (OrbitalPeriod / oneDay_s).ToString("###,###,##0.0"));
                    scanText.AppendFormat("Semi Major Axis: {0}AU\n", (SemiMajorAxis / oneAU_m).ToString("#0.0#"));
                    scanText.AppendFormat("Oribtal Eccentricity: {0}°\n", Eccentricity);
                    scanText.AppendFormat("Orbtial Inclination: {0}°\n", OrbitalInclination);
                    scanText.AppendFormat("Arg Of Periapsis: {0}°\n", Periapsis);
                }                
                scanText.AppendFormat("Absolute Magnitude: {0}\n", AbsoluteMagnitude);
                scanText.AppendFormat("Rotation Period: {0} days\n", (RotationPeriod / oneDay_s).ToString("###,###,##0.0"));
                if (Rings != null && Rings.Any())
                {
                    scanText.Append("\n");
                    scanText.AppendFormat("Belt{0}", Rings.Count() == 1 ? "" : "s");
                    foreach (PlanetRing ring in Rings)
                    {
                        scanText.Append("\n");
                        scanText.AppendFormat("{0} ({1})\n", ring.Name, DisplayStringFromRingClass(ring.RingClass));
                        scanText.AppendFormat("Moon Masses: {0}\n", (ring.MassMT / oneMoon_MT).ToString("#,###,###,##0.0######"));
                        scanText.AppendFormat("Inner Radius: {0}ls\n", (ring.InnerRad / 300000000).ToString("#,###,###,###,###"));
                        scanText.AppendFormat("Outer Radius: {0}ls\n", (ring.OuterRad / 300000000).ToString("#,###,###,###,###"));
                    }
                }
            }
            else
            {
                //planet
                scanText.AppendFormat("{0}\n", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                        ToTitleCase(PlanetClass.ToLower()).Replace("Ii", "II").Replace("Ii", "II").Replace("Iv", "IV"));
                scanText.Append(TerraformState == "Terraformable" ? "Candidate for terraforming\n" : "\n");
                scanText.AppendFormat("Earth Masses: {0}\n", MassEM);
                scanText.AppendFormat("Radius: {0}km\n", (Radius.Value / 1000).ToString("###,##0"));
                scanText.AppendFormat("Gravity: {0}g\n", SurfaceGravity / 10);
                scanText.AppendFormat("Surface Temp: {0}K\n", SurfaceTemperature.Value.ToString("#,###,###.00"));
                if (!PlanetClass.ToLower().Contains("gas"))
                {
                    if (SurfacePressure.HasValue && SurfacePressure.Value > 0.00)
                        scanText.AppendFormat("Surface Pressure: {0} Atmospheres\n", (SurfacePressure.Value / 100000).ToString("#,###,###,###,##0.00"));
                    scanText.AppendFormat("Volcanism: {0}\n", Volcanism == String.Empty ? "No Volcanism" : System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                        ToTitleCase(Volcanism.ToLower()));
                    scanText.AppendFormat("Atmosphere Type: {0}\n", Atmosphere == String.Empty ? "No Atmosphere" :
                                        System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Atmosphere.ToLower()));
                }
                scanText.AppendFormat("Orbital Period: {0}D\n", (OrbitalPeriod / oneDay_s).ToString("###,###,##0.0"));
                scanText.AppendFormat("Semi Major Axis: {0}AU\n", (SemiMajorAxis / oneAU_m).ToString("#0.0#"));
                scanText.AppendFormat("Oribtal Eccentricity: {0}°\n", Eccentricity);
                scanText.AppendFormat("Orbtial Inclination: {0}°\n", OrbitalInclination);
                scanText.AppendFormat("Arg Of Periapsis: {0}°\n", Periapsis);
                scanText.AppendFormat("Rotation Period: {0} days", (RotationPeriod / oneDay_s).ToString("###,###,##0.0"));
                scanText.Append(TidalLock ? " (Tidally locked)\n" : "\n");
                if (Rings != null && Rings.Any())
                {
                    scanText.Append("\n");
                    scanText.AppendFormat("Ring{0}", Rings.Count() == 1 ? "" : "s");
                    foreach(PlanetRing ring in Rings)
                    {
                        scanText.Append("\n");
                        scanText.AppendFormat("{0} ({1})\n", ring.Name, DisplayStringFromRingClass(ring.RingClass));
                        scanText.AppendFormat("Mass: {0}MT\n", ring.MassMT.ToString("#,###,###,###,###,###,###,###,###"));
                        scanText.AppendFormat("Inner Radius: {0}km\n", (ring.InnerRad / 1000).ToString("#,###,###,###,###"));
                        scanText.AppendFormat("Outer Radius: {0}km\n", (ring.OuterRad / 1000).ToString("#,###,###,###,###"));
                    }
                }
                if (Materials != null && Materials.Any())
                {
                    scanText.Append("\n");
                    scanText.Append("Materials\n");
                    foreach(KeyValuePair<string, double> mat in Materials)
                    {
                        scanText.AppendFormat("{0} - {1}%\n", 
                                        System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(mat.Key.ToLower()), 
                                        mat.Value.ToString("#0.0#"));
                    }
                    
                }
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
                    return ringClass;
            }
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

    }

    public class PlanetRing
    {
        public string Name;
        public string RingClass;
        public double MassMT;
        public double InnerRad;
        public double OuterRad;
    }
}
