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
            ObjectType = ObjectTypesEnum.Unknown_Star;
        }


        public bool ParseJson(JObject jo)
        {

            id = jo["id"].Value<int>();
            var attributes = jo["attributes"];
            system = attributes["system-name"].Value<string>();
            objectName = attributes["star"].Value<string>();
            updater = attributes["updater"].Value<string>();

            ObjectType = String2ObjectType(attributes["spectral-class"].Value<string>());
            radius = GetFloat(attributes["solar-radius"]);
            mass = GetFloat(attributes["solar-mass"]);
            surfaceTemp = GetInt(attributes["surface-temp"]);
            star_age = GetFloat(attributes["star-age"]);
            orbitPeriod = GetFloat(attributes["orbit-period"]);
            arrivalPoint = GetFloat(attributes["arrival-point"]);
            notes = GetString(attributes["notes"]);
            subclass = GetString(attributes["spectral-subclass"]);
            luminosity = GetString(attributes["luminosity"]);
            image_url = GetString(attributes["image-url"]);

            return true;
        }



    }
}
