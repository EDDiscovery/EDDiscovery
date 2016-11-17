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
        public bool HasRings { get { return Rings != null && Rings.Length>0; } }

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

        public bool HasMaterials { get { return Materials != null && Materials.Any(); } }
        public Dictionary<string, double> Materials { get; set; }

        //public string MaterialsString { get { return jEventData["Materials"].ToString(); } }

        

        public string DisplayString(bool printbodyname = true, int indent = 0)
        {
            string inds = "                                         ".Substring(0, 1 + indent);

            StringBuilder scanText = new StringBuilder();

            scanText.Append(inds);

            if (printbodyname)
                scanText.AppendFormat("{0}\n\n", BodyName);

            if (!String.IsNullOrEmpty(StarType))
            {
                scanText.AppendFormat(GetStarTypeImage().Item2 + Environment.NewLine);

                if (nAge.HasValue)
                    scanText.AppendFormat("Age: {0} million years\n", nAge.Value.ToString("N0"));

                if (nStellarMass.HasValue)
                    scanText.AppendFormat("Solar Masses: {0:0.00}\n", nStellarMass.Value);

                if (nRadius.HasValue)
                    scanText.AppendFormat("Solar Radius: {0:0.00}\n", (nRadius.Value / solarRadius_m));

                if (nSurfaceTemperature.HasValue)
                    scanText.AppendFormat("Surface Temp: {0}K\n", nSurfaceTemperature.Value.ToString("N0"));

                if (nOrbitalPeriod.HasValue && nOrbitalPeriod > 0)
                    scanText.AppendFormat("Orbital Period: {0}D\n", (nOrbitalPeriod.Value / oneDay_s).ToString("N1"));
                if (nSemiMajorAxis.HasValue)
                    scanText.AppendFormat("Semi Major Axis: {0}AU\n", (nSemiMajorAxis.Value / oneAU_m).ToString("N1"));
                if (nEccentricity.HasValue)
                    scanText.AppendFormat("Orbital Eccentricity: {0:0.00}°\n", nEccentricity.Value);
                if (nOrbitalInclination.HasValue)
                    scanText.AppendFormat("Orbital Inclination: {0:0.00}°\n", nOrbitalInclination.Value);
                if (nPeriapsis.HasValue)
                    scanText.AppendFormat("Arg Of Periapsis: {0:0.00}°\n", nPeriapsis.Value);

                if (nAbsoluteMagnitude.HasValue)
                    scanText.AppendFormat("Absolute Magnitude: {0:0.00}\n", nAbsoluteMagnitude.Value);

                if (nRotationPeriod.HasValue)
                    scanText.AppendFormat("Rotation Period: {0} days\n", (nRotationPeriod.Value / oneDay_s).ToString("N1"));

                if (HasRings)
                {
                    scanText.Append("\n");
                    scanText.AppendFormat("Belt{0}", Rings.Count() == 1 ? "" : "s");
                    for (int i = 0; i < Rings.Length; i++)
                        scanText.Append("\n" + RingInformation(i, oneMoon_MT, " Moons"));
                }
            }
            else
            {
                //planet
                scanText.AppendFormat("{0}\n", System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                        ToTitleCase(PlanetClass.ToLower()));

                scanText.Append((TerraformState != null && TerraformState == "Terraformable") ? "Candidate for terraforming\n" : "\n");

                if (nMassEM.HasValue)
                    scanText.AppendFormat("Earth Masses: {0:0.00}\n", nMassEM.Value);

                if (nRadius.HasValue)
                    scanText.AppendFormat("Radius: {0:0.0}km\n", (nRadius.Value / 1000).ToString("N0"));

                if (nSurfaceGravity.HasValue)
                    scanText.AppendFormat("Gravity: {0:0.0}g\n", nSurfaceGravity.Value / 9.8);

                if (nSurfaceTemperature.HasValue)
                    scanText.AppendFormat("Surface Temp: {0}K\n", nSurfaceTemperature.Value.ToString("N1"));

                if (nSurfacePressure.HasValue && nSurfacePressure.Value > 0.00 && !PlanetClass.ToLower().Contains("gas"))
                    scanText.AppendFormat("Surface Pressure: {0} Atmospheres\n", (nSurfacePressure.Value / 100000).ToString("N2"));

                if (Volcanism != null)
                    scanText.AppendFormat("Volcanism: {0}\n", Volcanism == String.Empty ? "No Volcanism" : System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.
                                                                                                ToTitleCase(Volcanism.ToLower()));

                if (PlanetClass != null && !PlanetClass.ToLower().Contains("gas"))
                {
                    scanText.AppendFormat("Atmosphere Type: {0}\n", (Atmosphere == null || Atmosphere == String.Empty) ? "No Atmosphere" :
                                                        System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Atmosphere.ToLower()));
                }

                if (nOrbitalPeriod.HasValue)
                    scanText.AppendFormat("Orbital Period: {0}D\n", (nOrbitalPeriod.Value / oneDay_s).ToString("N0"));
                if (nSemiMajorAxis.HasValue)
                    scanText.AppendFormat("Semi Major Axis: {0}AU\n", (nSemiMajorAxis.Value / oneAU_m).ToString("N1"));
                if (nEccentricity.HasValue)
                    scanText.AppendFormat("Orbital Eccentricity: {0:0.00}°\n", nEccentricity.Value);
                if (nOrbitalInclination.HasValue)
                    scanText.AppendFormat("Orbital Inclination: {0:0.00}°\n", nOrbitalInclination.Value);
                if (nPeriapsis.HasValue)
                    scanText.AppendFormat("Arg Of Periapsis: {0:0.00}°\n", nPeriapsis.Value);
                if (nRotationPeriod.HasValue)
                    scanText.AppendFormat("Rotation Period: {0} days", (nRotationPeriod.Value / oneDay_s).ToString("N1"));

                scanText.Append((nTidalLock.HasValue && nTidalLock.Value) ? " (Tidally locked)\n" : "\n");

                if (HasRings)
                {
                    scanText.Append("\n");
                    scanText.AppendFormat("Ring{0}", Rings.Count() == 1 ? "" : "s");
                    for (int i = 0; i < Rings.Length; i++)
                        scanText.Append("\n" + RingInformation(i));
                }

                if (HasMaterials) 
                {
                    scanText.Append("\n");
                    scanText.Append("Materials\n");
                    foreach (KeyValuePair<string, double> mat in Materials)
                    {
                        scanText.AppendFormat("  {0} - {1}%\n",
                                        System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(mat.Key.ToLower()),
                                        mat.Value.ToString("#0.0#"));
                    }

                }
            }

            return scanText.ToNullSafeString().Replace("\n", "\n" + inds);
        }

        public string RingInformationMoons(int ringno)
        {
            return RingInformation(ringno, 1.0 / oneMoon_MT, " Moons");
        }

        public string RingInformation(int ringno, double scale = 1, string scaletype = " MT")
        {
            StarPlanetRing ring = Rings[ringno];
            StringBuilder scanText = new StringBuilder();
            scanText.AppendFormat("  {0} ({1})\n", ring.Name, ring.RingClass.Replace("eRingClass_", ""));
            scanText.AppendFormat("  Mass: {0}{1}\n", (ring.MassMT*scale).ToString("N0"),scaletype );
            scanText.AppendFormat("  Inner Radius: {0}km\n", (ring.InnerRad / 1000).ToString("N0"));
            scanText.AppendFormat("  Outer Radius: {0}km\n", (ring.OuterRad / 1000).ToString("N0"));
            return scanText.ToNullSafeString();
        }


        public Tuple<System.Drawing.Image, string> GetStarTypeImage()           // give image and description to star class
        {
            System.Drawing.Image ret = EDDiscovery.Properties.Resources.Star_K1IV;

            switch (StarType)
            {
                case "N":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "Neutron Star");
                case "AeBe":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "Herbig Ae/Be");
                case "H":
                    // currently speculative, not confirmed with actual data...
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "Black Hole");
                case "TTS":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, "T Tauri");
                case "CS":
                case "C":
                case "CN":
                case "CJ":
                case "CHd":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, string.Format("Carbon ({0}) star", StarType));
                case "W":
                case "WN":
                case "WNC":
                case "WC":
                case "WO":
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, string.Format("Wolf-Rayet ({0}) star", StarType));
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
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, string.Format("White Dwarf ({0}) star", StarType));
                default:
                    string s = string.Format("Class {0} star\n", StarType.Replace("_", " ").Replace("Super", " Super").Replace("Giant", " Giant"));
                    return new Tuple<System.Drawing.Image, string>(EDDiscovery.Properties.Resources.DefaultStar, s);
            }
        }

        static public System.Drawing.Image GetStarImageNotScanned()
        {
            return EDDiscovery.Properties.Resources.Star_K1IV;
        }

        public System.Drawing.Image GetPlanetClassImage()
        {
            System.Drawing.Image ret = EDDiscovery.Properties.Resources.Class_II_Gas_Giant_Sand1;
            return ret;
        }

        static public System.Drawing.Image GetPlanetImageNotScanned()
        {
            return EDDiscovery.Properties.Resources.Ammonia_Brown;
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

    }



    public class StarScan
    {
        Dictionary<Tuple<string, long>, SystemNode> scandata = new Dictionary<Tuple<string, long>, SystemNode>();

        public class SystemNode
        {
            public EDDiscovery2.DB.ISystem system;
            public SortedList<string, ScanNode> starnodes;
        };

        public enum ScanNodeType { star , barycentre, planet , moon, starbelt, rings };

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
                sn = new SystemNode() { system = sys, starnodes = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>()) };
                scandata.Add(new Tuple<string, long>(sys.name, sys.id_edsm), sn);
            }

            if (!String.IsNullOrEmpty(sc.StarType))
            {
                FindOrAddStar(sn, sc, sys.name);
            }
            else
                AddUpdatePlanetMoon(sn, sc, sys.name);
        }

        void FindOrAddStar(SystemNode sn, JournalScan sc, string starname)
        {                                                           // so a star line Eol Prou LW-L c8 - 306 A, it it there?
            List<string> elements;
            ReturnElements(sc.BodyName, starname, out elements);    // elements[0] = star designator.. may be MAIN meaning there is not one..
            ScanNode s = FindOrAdd(sn, starname, elements[0]);
            s.scandata = sc;
        }

        ScanNode FindOrAdd(SystemNode sn, string starname , string designator )
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
                    type = (nodesignator) ? ScanNodeType.star : ((designator.Length > 1) ? ScanNodeType.barycentre : ScanNodeType.star)
                };

                sn.starnodes.Add(designator, star);
                System.Diagnostics.Debug.WriteLine("Added star " + star.fullname + " '" + star.ownname + "'" + " under " + designator);
            }

            return star;
        }

        bool AddUpdatePlanetMoon(SystemNode sn, JournalScan sc, string starname)
        {
            // handle Earth, starname = Sol
            // handle Eol Prou LW-L c8-306 A 4 a and Eol Prou LW-L c8-306
            // handle Colonia 4 , starname = Colonia, planet 4
            // handle Aurioum B A BELT

            List<string> elements;
            ReturnElements(sc.BodyName, starname, out elements);       // elements[0] = star designator or MAIN, 1 = planet, 2 = moon

            if (elements.Count >= 2)            // must have a star, and at least a planet..
            {
                ScanNode star = FindOrAdd(sn, starname, elements[0]);  // find or add the star...

                ScanNode planetscan;

                if (star.children == null || !star.children.TryGetValue(elements[1], out planetscan))
                {
                    if (star.children == null)
                        star.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());

                    planetscan = new ScanNode() { ownname = elements[1], fullname = star.fullname + " " + elements[1], scandata = null, children = null , type = ScanNodeType.planet };
                    star.children.Add(elements[1], planetscan);
                }

                if (elements.Count >= 3)          // star, planet, moon
                {
                    ScanNode moonscan;

                    if (planetscan.children == null || !planetscan.children.TryGetValue(elements[2], out moonscan))
                    {
                        if (planetscan.children == null)
                            planetscan.children = new SortedList<string, ScanNode>(new DuplicateKeyComparer<string>());

                        moonscan = new ScanNode() { ownname = elements[2] , fullname = star.fullname + " " + elements[1] + " " + elements[2], scandata = null, children = null , type = ScanNodeType.moon };
                        planetscan.children.Add(elements[2],moonscan);
                    }

                    moonscan.scandata = sc;
                    System.Diagnostics.Debug.WriteLine("Added moon scan '{0}' to {1} to {2}", elements[2], elements[1], elements[0]);
                }
                else
                {
                    planetscan.scandata = sc;
                    System.Diagnostics.Debug.WriteLine("Added planet scan '{0}' to {1}", elements[1], elements[0]);
                }

                return true;
            }
            else
                return false;
        }

        private void ReturnElements(string bodyname, string starname, out List<string> elements)      // 0 = star designator (MAIN if no designator), 1 = first name, etc
        {
            bool namerelatestostar = bodyname.Length > starname.Length && starname.Equals(bodyname.Substring(0, starname.Length), StringComparison.InvariantCultureIgnoreCase);

            if (namerelatestostar)
            {
                string restname = bodyname.Substring(starname.Length);
                restname = restname.Trim();

                if (restname.Length > 0)                    // if something is left..
                {
                    elements = restname.Split(' ').ToList();

                    if (char.IsDigit(elements[0][0]))       // if digits, planet number, no star designator
                        elements.Insert(0, "MAIN");         // no star designator
                }
                else
                {                                           // nothing, so just a star...
                    elements = new List<string>();
                    elements.Add("MAIN");
                }
            }
            else
            {
                elements = bodyname.Split(' ').ToList();    // these are body parts (planet moon etc)
                elements.Insert(0, "MAIN");                     
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

