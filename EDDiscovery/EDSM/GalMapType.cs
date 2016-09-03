using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EDDiscovery.EDSM
{
    public class GalMapType
    {
        public enum GalMapGroup
        {
            Markers = 1,
            Routes,
            Regions,
            Quadrants,
        }

        public string Typeid;
        public string Description;
        public Bitmap Image;
        public GalMapGroup Group;
        public bool Enabled;

        public GalMapType(string id, string desc, GalMapGroup g, Bitmap b, int i)
        {
            Typeid = id;
            Description = desc;
            Group = g;
            Image = b;
            Enabled = false;
        }
        
        static public List<GalMapType> GetTypes()
        {
            List<GalMapType> type = new List<GalMapType>();

            int index = 0;

            type.Add(new GalMapType("historicalLocation", "η Historical Location", GalMapGroup.Markers, EDDiscovery.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("nebula", "α Nebula", GalMapGroup.Markers, EDDiscovery.Properties.Resources.nebula, index++));
            type.Add(new GalMapType("planetaryNebula", "β Planetary Nebula", GalMapGroup.Markers, EDDiscovery.Properties.Resources.PlanetaryNebula, index++));
            type.Add(new GalMapType("stellarRemnant", "γ Stellar Remnant", GalMapGroup.Markers, EDDiscovery.Properties.Resources.stellaremnant, index++));
            type.Add(new GalMapType("blackHole", "δ Black Hole", GalMapGroup.Markers, EDDiscovery.Properties.Resources.Blackhole, index++));
            type.Add(new GalMapType("starCluster", "σ Star Cluster", GalMapGroup.Markers, EDDiscovery.Properties.Resources.starcluster, index++));
            type.Add(new GalMapType("pulsar", "ζ Pulsar", GalMapGroup.Markers , EDDiscovery.Properties.Resources.pulsar, index++));
            type.Add(new GalMapType("explorationHazard", "λ Exploration Hazard", GalMapGroup.Markers , EDDiscovery.Properties.Resources.ExplorationHazard, index++));
            type.Add(new GalMapType("minorPOI", "★ Minor POI or Star", GalMapGroup.Markers , EDDiscovery.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("beacon", "⛛ Beacon", GalMapGroup.Markers , EDDiscovery.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("surfacePOI", "∅ Surface POI", GalMapGroup.Markers , EDDiscovery.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("cometaryBody", "☄ Cometary Body", GalMapGroup.Markers , EDDiscovery.Properties.Resources.comet, index++));
            type.Add(new GalMapType("jumponiumRichSystem", "☢ Jumponium-Rich System", GalMapGroup.Markers, EDDiscovery.Properties.Resources.pointofinterest, index++));
            type.Add(new GalMapType("planetFeatures", "∅ Planetary Features", GalMapGroup.Markers, EDDiscovery.Properties.Resources.pointofinterest, index++));

            type.Add(new GalMapType("travelRoute", "Travel Route", GalMapGroup.Routes , null, index++));
            type.Add(new GalMapType("historicalRoute", "Historical Route", GalMapGroup.Routes , null, index++));
            type.Add(new GalMapType("minorRoute", "Minor Route", GalMapGroup.Routes, null, index++));

            type.Add(new GalMapType("region", "Region", GalMapGroup.Regions , null, index++));
            type.Add(new GalMapType("regionQuadrants", "Galactic Quadrants", GalMapGroup.Quadrants , null, index++));

            type.Add(new GalMapType("EDSMUnknown", "EDSM other POI type", GalMapGroup.Markers, EDDiscovery.Properties.Resources.pointofinterest, index++));

            return type;
        }
    }
}