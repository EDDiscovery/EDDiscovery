using Newtonsoft.Json.Linq;

namespace EDDiscovery2.PlanetSystems
{

    public class EDBasecamp : EDObject
    {
        public int worldId; // A basecamp has to have a worldid
        public string name;
        public string description;
        public string landingZoneTerrain;
        public int terrainHue1; // rgb color ints
        public int terrainHue2;
        public int terrainHue3;
        public float landingZoneLat;
        public float landingZoneLon;

        public EDBasecamp()
        {
        }

        public bool ParseJson(JObject jo)
        {
            //Reminder - JSONAPI attributes and relationships structure

            id = jo["id"].Value<int>();

            var attributes = jo["attributes"];
            updater = attributes["updater"].Value<string>();
            name = attributes["name"].Value<string>();
            description = attributes["description"].Value<string>();
            landingZoneTerrain = attributes["landing-zone-terrain"].Value<string>();
            terrainHue1 = attributes["terrain-hue-1"].Value<int>();
            terrainHue2 = attributes["terrain-hue-2"].Value<int>();
            terrainHue3 = attributes["terrain-hue-3"].Value<int>(); ;
            landingZoneLat = attributes["landing-zone-lat"].Value<float>();
            landingZoneLon = attributes["landing-zone-lon"].Value<float>();
            notes = attributes["notes"].Value<string>();
            imageUrl = attributes["image-url"].Value<string>();

            var relationships = jo["relationships"];
            var world = relationships["world"];
            worldId = world["id"].Value<int>();
            return true;
        }

    }
}
    