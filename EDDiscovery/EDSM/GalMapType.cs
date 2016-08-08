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
        }

        public string typeid;
        public string description;
        public Bitmap image;
        public GalMapGroup group;

        public GalMapType(string id, string desc, GalMapGroup g, Bitmap b)
        {
            typeid = id;
            description = desc;
            group = g;
            image = b;
        }
        
        static private List<GalMapType> GetTypes()
        {
            List<GalMapType> type = new List<GalMapType>();

            type.Add(new GalMapType("historicalLocation", "η Historical Location", GalMapGroup.Markers, EDDiscovery.Properties.Resources.pointofinterest));
            type.Add(new GalMapType("nebula", "α Nebula", GalMapGroup.Markers, EDDiscovery.Properties.Resources.nebula));
            type.Add(new GalMapType("planetaryNebula", "β Planetary Nebula", GalMapGroup.Markers, EDDiscovery.Properties.Resources.PlanetaryNebula));
            type.Add(new GalMapType("stellarRemnant", "γ Stellar Remnant", GalMapGroup.Markers, EDDiscovery.Properties.Resources.stellaremnant));
            type.Add(new GalMapType("blackHole", "δ Black Hole", GalMapGroup.Markers, EDDiscovery.Properties.Resources.Blackhole));
            type.Add(new GalMapType("starCluster", "σ Star Cluster", GalMapGroup.Markers, EDDiscovery.Properties.Resources.starcluster));
            type.Add(new GalMapType("pulsar", "ζ Pulsar", GalMapGroup.Markers , EDDiscovery.Properties.Resources.pulsar));
            type.Add(new GalMapType("explorationHazard", "λ Exploration Hazard", GalMapGroup.Markers , EDDiscovery.Properties.Resources.ExplorationHazard));
            type.Add(new GalMapType("minorPOI", "★ Minor POI or Star", GalMapGroup.Markers , EDDiscovery.Properties.Resources.pointofinterest));
            type.Add(new GalMapType("beacon", "⛛ Beacon", GalMapGroup.Markers , EDDiscovery.Properties.Resources.pointofinterest));
            type.Add(new GalMapType("surfacePOI", "∅ Surface POI", GalMapGroup.Markers , EDDiscovery.Properties.Resources.pointofinterest));
            type.Add(new GalMapType("cometaryBody", "☄ Cometary Body", GalMapGroup.Markers , EDDiscovery.Properties.Resources.comet));
            type.Add(new GalMapType("jumponiumRichSystem", "☢ Jumponium-Rich System", GalMapGroup.Markers , EDDiscovery.Properties.Resources.pointofinterest));

            type.Add(new GalMapType("travelRoute", "Travel Route", GalMapGroup.Routes , null));
            type.Add(new GalMapType("historicalRoute", "Historical Route", GalMapGroup.Routes , null ));
            type.Add(new GalMapType("minorRoute", "Minor Route", GalMapGroup.Routes, null ));

            type.Add(new GalMapType("region", "Region", GalMapGroup.Regions , null));
            type.Add(new GalMapType("regionQuadrants", "Galactic Quadrants", GalMapGroup.Regions , null));

            type.Add(new GalMapType("EDSMUnknown", "EDSM other POI type", GalMapGroup.Markers, null));

            return type;
           
        }

        static public Dictionary<string, GalMapType> GetDictionary
        {
            get
            {
                List<GalMapType> types = GetTypes();
                Dictionary<string, GalMapType> dict = new Dictionary<string, GalMapType>();

                foreach (GalMapType  type in types)
                {
                    dict[type.typeid] = type;
                }
                return dict;

            }
        }
    }
}