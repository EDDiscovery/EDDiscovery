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
            system = jo["system"].Value<string>();
            objectName = jo["star"].Value<string>();
            updater = jo["updater"].Value<string>();

            ObjectType = String2ObjectType(jo["spectral_class"].Value<string>());
            radius = GetFloat(jo["solar_radius"]);
            mass = GetFloat(jo["solar_mass"]);
            surfaceTemp = GetInt(jo["surface_temp"]);
            star_age = GetFloat(jo["star_age"]);
            orbitPeriod = GetFloat(jo["orbit_period"]);
            arrivalPoint = GetFloat(jo["arrival_point"]);
            notes = GetString(jo["notes"]);
            subclass = GetString(jo["spectral_subclass"]);
            luminosity = GetString(jo["luminosity"]);
            image_url = GetString(jo["image_url"]);


            return true;
        }



    }
}
