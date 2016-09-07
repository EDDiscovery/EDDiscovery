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
        public JournalScan(JObject evt) : base(evt, JournalTypeEnum.Scan)
        {
            BodyName = evt.Value<string>("BodyName");
            StarType = evt.Value<string>("StarType");
            StellarMass = evt.Value<double?>("StellarMass");
            Radius = evt.Value<double?>("StellarMass");
            AbsoluteMagnitude = evt.Value<double?>("StellarMass");
            OrbitalPeriod = evt.Value<double>("StellarMass");
            RotationPeriod = evt.Value<double>("StellarMass");
            Rings = evt["Rings"]?.ToObject<PlanetRing[]>();

            TidalLock = evt.Value<bool?>("TidalLock") ?? false;
            TerraformState = evt.Value<string>("TerraformState");
            PlanetClass = evt.Value<string>("PlanetClass");
            Atmosphere = evt.Value<string>("Atmosphere");
            Volcanism = evt.Value<string>("Volcanism");
            MassEM = evt.Value<double?>("MassEM");
            SurfaceGravity = evt.Value<double?>("SurfaceGravity");
            SurfaceTemperature = evt.Value<double?>("SurfaceTemperature");
            SurfacePressure = evt.Value<double?>("SurfacePressure");
            Landable = evt.Value<bool?>("Landable") ?? false;
            Materials = evt["Materials"]?.ToObject<Dictionary<string, double>>();

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

        public bool? TidalLock { get; set; }
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
