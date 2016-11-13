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

        public JournalScan(JObject evt ) : base(evt, JournalTypeEnum.Scan)
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

            Rings = evt["Rings"]?.ToObject<PlanetRing[]>();

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

        public PlanetRing[] Rings { get; set; }

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

        public Dictionary<string, double> Materials { get; set; }

        public string MaterialsString { get { return jEventData["Materials"].ToString(); } }
        
        public List<JournalScan> children;      // any sub children in scans in memory

        public string DisplayString(bool printbodyname = true , int indent = 0)
        {
            string inds = "                                         ".Substring(0, 1 + indent);

            StringBuilder scanText = new StringBuilder();

            scanText.Append(inds);

            if ( printbodyname)
                scanText.AppendFormat("{0}\n\n", BodyName);

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

                if ( nAge.HasValue )
                    scanText.AppendFormat("Age: {0} million years\n", nAge.Value.ToString("##,####"));

                if ( nStellarMass.HasValue)
                    scanText.AppendFormat("Solar Masses: {0:0.00}\n", nStellarMass.Value);

                if ( nRadius.HasValue )
                    scanText.AppendFormat("Solar Radius: {0:0.00}\n", (nRadius.Value / solarRadius_m));

                if ( nSurfaceTemperature.HasValue)
                    scanText.AppendFormat("Surface Temp: {0}K\n", nSurfaceTemperature.Value.ToString("#,###,###"));

                if (nOrbitalPeriod.HasValue && nOrbitalPeriod>0)
                    scanText.AppendFormat("Orbital Period: {0}D\n", (nOrbitalPeriod.Value / oneDay_s).ToString("###,###,##0.0"));
                if (nSemiMajorAxis.HasValue)
                    scanText.AppendFormat("Semi Major Axis: {0}AU\n", (nSemiMajorAxis.Value / oneAU_m).ToString("#0.0#"));
                if ( nEccentricity.HasValue)
                    scanText.AppendFormat("Orbital Eccentricity: {0:0.00}°\n", nEccentricity.Value);
                if ( nOrbitalInclination.HasValue)
                    scanText.AppendFormat("Orbital Inclination: {0:0.00}°\n", nOrbitalInclination.Value);
                if ( nPeriapsis.HasValue)
                    scanText.AppendFormat("Arg Of Periapsis: {0:0.00}°\n", nPeriapsis.Value);

                if (nAbsoluteMagnitude.HasValue)
                    scanText.AppendFormat("Absolute Magnitude: {0:0.00}\n", nAbsoluteMagnitude.Value);

                if ( nRotationPeriod.HasValue )
                    scanText.AppendFormat("Rotation Period: {0} days\n", (nRotationPeriod.Value / oneDay_s).ToString("###,###,##0.0"));

                if (Rings != null && Rings.Any())
                {
                    scanText.Append("\n");
                    scanText.AppendFormat("Belt{0}", Rings.Count() == 1 ? "" : "s");
                    foreach (PlanetRing ring in Rings)
                    {
                        scanText.Append("\n");
                        scanText.AppendFormat("{0} ({1})\n", ring.Name, ring.RingClass.Replace("eRingClass_", ""));
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
                                        ToTitleCase(PlanetClass.ToLower()));

                scanText.Append((TerraformState!=null && TerraformState == "Terraformable" )? "Candidate for terraforming\n" : "\n");

                if ( nMassEM.HasValue )
                    scanText.AppendFormat("Earth Masses: {0:0.00}\n", nMassEM.Value);

                if ( nRadius.HasValue )
                    scanText.AppendFormat("Radius: {0:0.0}km\n", (nRadius.Value / 1000).ToString("###,##0"));

                if ( nSurfaceGravity.HasValue )
                    scanText.AppendFormat("Gravity: {0:0.0}g\n", nSurfaceGravity.Value / 9.8);

                if ( nSurfaceTemperature.HasValue)
                    scanText.AppendFormat("Surface Temp: {0}K\n", nSurfaceTemperature.Value.ToString("#,###,###.0"));

                if (nSurfacePressure.HasValue && nSurfacePressure.Value > 0.00 && !PlanetClass.ToLower().Contains("gas"))
                    scanText.AppendFormat("Surface Pressure: {0} Atmospheres\n", (nSurfacePressure.Value / 100000).ToString("#,###,###,###,##0.00"));

                if ( Volcanism != null )
                    scanText.AppendFormat("Volcanism: {0}\n", Volcanism == String.Empty ? "No Volcanism" : System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                                                                                ToTitleCase(Volcanism.ToLower()));

                if (PlanetClass != null && !PlanetClass.ToLower().Contains("gas"))
                {
                    scanText.AppendFormat("Atmosphere Type: {0}\n", (Atmosphere ==null || Atmosphere == String.Empty ) ? "No Atmosphere" :
                                                        System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Atmosphere.ToLower()));
                }

                if ( nOrbitalPeriod.HasValue )
                    scanText.AppendFormat("Orbital Period: {0:0.00}D\n", (nOrbitalPeriod.Value / oneDay_s).ToString("###,###,##0.0"));
                if (nSemiMajorAxis.HasValue )
                    scanText.AppendFormat("Semi Major Axis: {0:0.00}AU\n", (nSemiMajorAxis.Value / oneAU_m).ToString("#0.0#"));
                if ( nEccentricity.HasValue )
                    scanText.AppendFormat("Orbital Eccentricity: {0:0.00}°\n", nEccentricity.Value);
                if ( nOrbitalInclination.HasValue )
                    scanText.AppendFormat("Orbital Inclination: {0:0.00}°\n", nOrbitalInclination.Value);
                if ( nPeriapsis.HasValue)
                    scanText.AppendFormat("Arg Of Periapsis: {0:0.00}°\n", nPeriapsis.Value);
                if ( nRotationPeriod.HasValue)
                    scanText.AppendFormat("Rotation Period: {0:0.00} days", (nRotationPeriod.Value / oneDay_s).ToString("###,###,##0.0"));

                scanText.Append(( nTidalLock.HasValue && nTidalLock.Value )? " (Tidally locked)\n" : "\n");

                if (Rings != null && Rings.Any())
                {
                    scanText.Append("\n");
                    scanText.AppendFormat("Ring{0}", Rings.Count() == 1 ? "" : "s");
                    foreach(PlanetRing ring in Rings)
                    {
                        scanText.Append("\n");
                        scanText.AppendFormat("  {0} ({1})\n", ring.Name, ring.RingClass.Replace("eRingClass_", ""));
                        scanText.AppendFormat("  Mass: {0}MT\n", ring.MassMT.ToString("#,###,###,###,###,###,###,###,###"));
                        scanText.AppendFormat("  Inner Radius: {0}km\n", (ring.InnerRad / 1000).ToString("#,###,###,###,###"));
                        scanText.AppendFormat("  Outer Radius: {0}km\n", (ring.OuterRad / 1000).ToString("#,###,###,###,###"));
                    }
                }

                if (Materials != null && Materials.Any())
                {
                    scanText.Append("\n");
                    scanText.Append("Materials\n");
                    foreach(KeyValuePair<string, double> mat in Materials)
                    {
                        scanText.AppendFormat("  {0} - {1}%\n", 
                                        System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(mat.Key.ToLower()), 
                                        mat.Value.ToString("#0.0#"));
                    }
                    
                }
            }

            return scanText.ToNullSafeString().Replace("\n", "\n" + inds);
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




    public class StarScan
    {
        Dictionary<Tuple<string, long>, SystemNode> scandata = new Dictionary<Tuple<string, long>, SystemNode>();

        public class SystemNode
        {
            public EDDiscovery2.DB.ISystem system;
            public List<ScanNode> starnodes;
        };

        public class ScanNode
        {
            public string rootname;
            public JournalScan scandata;            // can be null if no scan, its a place holder.
            public List<ScanNode> children;
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


        public void Process(JournalEntry je, EDDiscovery2.DB.ISystem sys)
        {
            if (je.EventTypeID != JournalTypeEnum.Scan)     // only one processed so far
                return;

            JournalScan sc = je as JournalScan;

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
                sn = new SystemNode() { system = sys, starnodes = new List<ScanNode>() };
                scandata.Add(new Tuple<string, long>(sys.name, sys.id_edsm), sn);
            }

            if (!String.IsNullOrEmpty(sc.StarType))
            {
                ScanNode star = FindOrAddStar(sn, sc.BodyName);
                star.scandata = sc;
            }
            else
                AddUpdatePlanetMoon(sn, sc, sys.name);
        }

        ScanNode FindOrAddStar(SystemNode sn, string starname)
        {                                                           // so a star line Eol Prou LW-L c8 - 306 A, it it there?
            ScanNode starscan = sn.starnodes.Find(x => x.rootname.Equals(starname, StringComparison.InvariantCultureIgnoreCase));

            if (starscan == null)
            {
                starscan = new ScanNode() { rootname = starname, scandata = null, children = null };
                sn.starnodes.Add(starscan);
                System.Diagnostics.Debug.WriteLine("Added star " + starname);
            }

            return starscan;
        }

        bool AddUpdatePlanetMoon(SystemNode sn, JournalScan sc, string starname)
        {
            // handle Earth, starname = Sol
            // handle Eol Prou LW-L c8-306 A 4 a and Eol Prou LW-L c8-306
            // handle Colonia 4 , starname = Colonia, planet 4
            // handle Aurioum B A BELT

            List<string> starplanetmoons = ReturnStarNameAndPostElements(sc.BodyName, starname);

            if (starplanetmoons.Count >= 2)            // must have a star, and at least a planet..
            {
                ScanNode star = FindOrAddStar(sn, starplanetmoons[0]);     // add star or find it

                ScanNode planetscan = star.children?.Find(x => x.rootname.Equals(starplanetmoons[1], StringComparison.InvariantCultureIgnoreCase));

                if (planetscan == null)
                {
                    if (star.children == null)
                        star.children = new List<ScanNode>();

                    planetscan = new ScanNode() { rootname = starplanetmoons[1], scandata = null, children = null };
                    star.children.Add(planetscan);
                }

                if (starplanetmoons.Count >= 3)          // moon!
                {
                    ScanNode moonscan = planetscan.children?.Find(x => x.rootname.Equals(starplanetmoons[2], StringComparison.InvariantCultureIgnoreCase));

                    if (moonscan == null)
                    {
                        if (planetscan.children == null)
                            planetscan.children = new List<ScanNode>();

                        moonscan = new ScanNode() { rootname = starplanetmoons[2], scandata = null, children = null };
                        planetscan.children.Add(moonscan);
                    }

                    moonscan.scandata = sc;
                    System.Diagnostics.Debug.WriteLine("Added moon scan '{0}' to {1} to {2}", starplanetmoons[2], starplanetmoons[1], starplanetmoons[0]);
                }
                else
                {
                    planetscan.scandata = sc;
                    System.Diagnostics.Debug.WriteLine("Added planet scan '{0}' to {1}", starplanetmoons[1], starplanetmoons[0]);
                }

                return true;
            }
            else
                return false;
        }

        List<string> ReturnStarNameAndPostElements(string bodyname, string starname)      // 0 = star, 1 = first name, etc
        {
            bool namerelatestostar = bodyname.Length > starname.Length && starname.Equals(bodyname.Substring(0, starname.Length), StringComparison.InvariantCultureIgnoreCase);

            List<string> starplanetmoons = new List<string>();

            if (namerelatestostar)
            {
                string restname = bodyname.Substring(starname.Length);
                restname = restname.Trim();

                if (restname.Length > 0)
                {
                    starplanetmoons = restname.Split(' ').ToList();

                    if (!char.IsDigit(starplanetmoons[0][0]))      // not digits, we have a star designator
                    {
                        starplanetmoons[0] = starname + " " + starplanetmoons[0];
                    }
                    else
                        starplanetmoons.Insert(0, starname);
                }
                else
                {
                    starplanetmoons = new List<string>();
                    starplanetmoons.Add(starname);
                }
            }
            else
            {
                starplanetmoons = bodyname.Split(' ').ToList();
                starplanetmoons.Insert(0, starname);
            }

            if ( starplanetmoons[starplanetmoons.Count-1].Equals("BELT",StringComparison.InvariantCultureIgnoreCase) && starplanetmoons.Count>=2)
            {
                starplanetmoons[starplanetmoons.Count - 2] = starplanetmoons[starplanetmoons.Count - 2] + " " + starplanetmoons[starplanetmoons.Count - 1];
                starplanetmoons.RemoveAt(starplanetmoons.Count - 1);
            }

            for (int i = 1; i < starplanetmoons.Count; i++)
            {
                starplanetmoons[i] = starplanetmoons[i - 1] + " " + starplanetmoons[i];
            }

            return starplanetmoons;
        }

    }
}

