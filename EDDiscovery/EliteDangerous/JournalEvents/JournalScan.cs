using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
