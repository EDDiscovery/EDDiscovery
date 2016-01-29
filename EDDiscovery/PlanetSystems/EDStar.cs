using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{
    public class EDStar : EDObject
    {
        public string subclass;
        public float star_age;
        public string luminosity;

        public EDStar()
        {
            if (objectsTypes != null)
                type = objectsTypes[0];
        }


        public bool ParseJson(JObject jo)
        {

            id = jo["id"].Value<int>();
            system = jo["system"].Value<string>();
            objectName = jo["world"].Value<string>();
            commander = jo["commander"].Value<string>();

            ObjectType = String2ObjectType(jo["world_type"].Value<string>());


            radius = GetFloat(jo["radius"]);
            arrivalPoint = GetFloat(jo["arrival_point"]);

            return true;
        }


    }
}
