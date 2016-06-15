using EDDiscovery;
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
            radius = Tools.GetFloat(attributes["solar-radius"]);
            mass = Tools.GetFloat(attributes["solar-mass"]);
            surfaceTemp = Tools.GetInt(attributes["surface-temp"]);
            star_age = Tools.GetFloat(attributes["star-age"]);
            orbitPeriod = Tools.GetFloat(attributes["orbit-period"]);
            arrivalPoint = Tools.GetFloat(attributes["arrival-point"]);
            notes = Tools.GetString(attributes["notes"]);
            subclass = Tools.GetString(attributes["spectral-subclass"]);
            luminosity = Tools.GetString(attributes["luminosity"]);
            imageUrl = Tools.GetString(attributes["image-url"]);

            return true;
        }



    }
}
