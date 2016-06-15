using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EDSM
{
    public enum GalMapGroup
    {
        Markers = 1,
        Routes,
        Regions,
    }
    public class GalMapType
    {
        public string typeid;
        public string description;
        public GalMapGroup group;

        public GalMapType(string id, string desc, GalMapGroup group)
        {
            typeid = id;
            description = desc;
            this.group = group;
        }



        static private List<GalMapType> GetTypes()
        {
            List<GalMapType> type = new List<GalMapType>();

            type.Add(new GalMapType("historicalLocation", "η Historical Location", GalMapGroup.Markers));
            type.Add(new GalMapType("nebula", "α Nebula", GalMapGroup.Markers));
            type.Add(new GalMapType("planetaryNebula", "β Planetary Nebula", GalMapGroup.Markers));
            type.Add(new GalMapType("stellarRemnant", "γ Stellar Remnant", GalMapGroup.Markers));
            type.Add(new GalMapType("blackHole", "δ Black Hole", GalMapGroup.Markers));
            type.Add(new GalMapType("starCluster", "σ Star Cluster", GalMapGroup.Markers));
            type.Add(new GalMapType("pulsar", "ζ Pulsar", GalMapGroup.Markers));
            type.Add(new GalMapType("explorationHazard", "λ Exploration Hazard", GalMapGroup.Markers));
            type.Add(new GalMapType("minorPOI", "★ Minor POI or Star", GalMapGroup.Markers));
            type.Add(new GalMapType("beacon", "⛛ Beacon", GalMapGroup.Markers));
            type.Add(new GalMapType("surfacePOI", "∅ Surface POI", GalMapGroup.Markers));
            type.Add(new GalMapType("cometaryBody", "☄ Cometary Body", GalMapGroup.Markers));
            type.Add(new GalMapType("jumponiumRichSystem", "☢ Jumponium-Rich System", GalMapGroup.Markers));

            type.Add(new GalMapType("travelRoute", "Travel Route", GalMapGroup.Routes));
            type.Add(new GalMapType("historicalRoute", "Historical Route", GalMapGroup.Routes));
            type.Add(new GalMapType("minorRoute", "Minor Route", GalMapGroup.Routes));

            type.Add(new GalMapType("region", "Region", GalMapGroup.Regions));
            type.Add(new GalMapType("regionQuadrants", "Galactic Quadrants", GalMapGroup.Regions));


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