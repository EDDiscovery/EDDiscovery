using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace EDDiscovery2.PlanetSystems
{
    public class EdMaterializer : EDMaterizliaerCom
    {


        public EdMaterializer()
        {
#if DEBUG
            // Dev server. Mess with data as much as you like here
            _serverAddress = "https://ed-materializer.herokuapp.com/";
            //_serverAddress = "http://ed-materializer-env.elasticbeanstalk.com/";
#else
            // Production
            _serverAddress = "http://ed-materializer-env.elasticbeanstalk.com/";
#endif
        }


        public List<EDWorld>GetAllWorlds(string system)
        {
            List<EDWorld> listObjects = new List<EDWorld>();
            string query = "api/v2/worlds";

            if (!String.IsNullOrEmpty(system))
                query = query + "?system="+HttpUtility.UrlEncode(system);

            var response = RequestGet(query);
            var json = response.Body;

            JArray jArray = null;
            JObject jObject = null;
            if (json != null && json.Length > 5)
                jObject = (JObject)JObject.Parse(json);

            if (jObject == null)
                return listObjects;


            jArray = (JArray)jObject["worlds"];


            foreach (JObject jo in jArray)
            {
                EDWorld obj = new EDWorld();

                if (obj.ParseJson(jo))
                    listObjects.Add(obj);
            }


            return listObjects;
        }

        public List<EDWorld> GetWorldSurveys(string system)
        {
            List<EDWorld> listObjects = new List<EDWorld>();
            string query = "api/v2/world_surveys";

            if (!String.IsNullOrEmpty(system))
                query = query + "?system=" + HttpUtility.UrlEncode(system);

            var response = RequestGet(query);
            var json = response.Body;

            JArray jArray = null;
            JObject jObject = null;
            if (json != null && json.Length > 5)
                jObject = (JObject)JObject.Parse(json);

            if (jObject == null)
                return listObjects;


            jArray = (JArray)jObject["world_surveys"];


            foreach (JObject jo in jArray)
            {
                EDWorld obj = new EDWorld();

                if (obj.ParseJson(jo))
                    listObjects.Add(obj);
            }


            return listObjects;
        }



        public List<EDStar> GetAllStars(string system)
        {
            List<EDStar> listObjects = new List<EDStar>();
            string query = "api/v2/stars";

            if (!String.IsNullOrEmpty(system))
                query = query + "?system=" + HttpUtility.UrlEncode(system);

            var response = RequestGet(query);
            var json = response.Body;

            JArray jArray = null;
            JObject jObject = null;
            if (json != null && json.Length > 5)
                jObject = (JObject)JObject.Parse(json);

            if (jObject == null)
                return listObjects;


            jArray = (JArray)jObject["stars"];


            foreach (JObject jo in jArray)
            {
                EDStar obj = new EDStar();

                if (obj.ParseJson(jo))
                    listObjects.Add(obj);
            }


            return listObjects;
        }


        public bool StorePlanet(EDWorld edobj)
        {
            
            dynamic jo = new JObject();

            jo.system = edobj.system;
            jo.updater = edobj.updater;
            jo.world = edobj.objectName;
            jo.world_type = edobj.Description;

            jo.mass = edobj.mass;
            jo.radius = edobj.radius;
            jo.gravity = edobj.gravity;
            jo.surface_temp = edobj.surfaceTemp;
            jo.surface_pressure = edobj.surfacePressure;
            jo.orbit_period = edobj.orbitPeriod;
            jo.rotation_period = edobj.rotationPeriod;
            jo.semi_major_axis = edobj.semiMajorAxis;
            jo.terrain_difficulty = edobj.terrain_difficulty;

            jo.vulcanism_type = edobj.vulcanism.ToString();
            jo.rock_pct = edobj.rockPct;
            jo.metal_pct = edobj.metalPct;
            jo.ice_pct = edobj.metalPct;
            jo.reserve = edobj.Reserve;
            jo.arrival_point = edobj.arrivalPoint;
            jo.terraformable = edobj.terraformable;
            jo.atmosphere_type = edobj.atmosphere.ToString();

            jo.notes = edobj.notes;
            jo.images_url = edobj.image_url;



            //jo.carbon = edobj.materials[MaterialEnum.Carbon];
            //jo.iron = edobj.materials[MaterialEnum.Iron];
            //jo.nickel = edobj.materials[MaterialEnum.Nickel];
            //jo.phosphorus = edobj.materials[MaterialEnum.Phosphorus];
            //jo.sulphur = edobj.materials[MaterialEnum.Sulphur];
            //jo.arsenic = edobj.materials[MaterialEnum.Arsenic];
            //jo.chromium = edobj.materials[MaterialEnum.Chromium];
            //jo.germanium = edobj.materials[MaterialEnum.Germanium];
            //jo.manganese = edobj.materials[MaterialEnum.Manganese];
            //jo.selenium = edobj.materials[MaterialEnum.Selenium];
            //jo.vanadium = edobj.materials[MaterialEnum.Vanadium];
            //jo.zinc = edobj.materials[MaterialEnum.Zinc];
            //jo.zirconium = edobj.materials[MaterialEnum.Zirconium];
            //jo.cadmium = edobj.materials[MaterialEnum.Cadmium];
            //jo.mercury = edobj.materials[MaterialEnum.Mercury];
            //jo.molybdenum = edobj.materials[MaterialEnum.Molybdenum];
            //jo.niobium = edobj.materials[MaterialEnum.Niobium];
            //jo.tin = edobj.materials[MaterialEnum.Tin];
            //jo.tungsten = edobj.materials[MaterialEnum.Tungsten];
            //jo.antimony = edobj.materials[MaterialEnum.Antimony];
            //jo.polonium = edobj.materials[MaterialEnum.Polonium];
            //jo.ruthenium = edobj.materials[MaterialEnum.Ruthenium];
            //jo.technetium = edobj.materials[MaterialEnum.Technetium];
            //jo.tellurium = edobj.materials[MaterialEnum.Tellurium];
            //jo.yttrium = edobj.materials[MaterialEnum.Yttrium];

            JObject joPost = new JObject(new JProperty("world", jo));

            if (edobj.id == 0)
            {
                var response = RequestSecurePost(joPost.ToString(), "api/v2/worlds");
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    JObject jo2 = (JObject)JObject.Parse(response.Body);
                    JObject obj = (JObject)jo2["world"];
                    edobj.id = obj["id"].Value<int>();
                }
                else if ((int)response.StatusCode == 422)
                {
                    // Surprise, record is already there for some reason!
                    // I may create an api method on the server that negates the need to check for 
                    // this at some point
                    // - Greg

                    var queryParam = $"system={jo.system}&world={jo.world}";
                    response = RequestGet($"api/v2/worlds?{queryParam}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        JObject jo2 = (JObject)JObject.Parse(response.Body);
                        JObject obj = (JObject)jo2["world"][0];
                        edobj.id = obj["id"].Value<int>();

                        response = RequestSecurePatch(joPost.ToString(), "api/v2/worlds/" + edobj.id.ToString());
                    }

                }
            }
            else
            {
                var response = RequestSecurePatch(joPost.ToString(), "api/v2/worlds/" + edobj.id.ToString());
            }
            return true;
        }

        public bool StoreStar(EDStar edobj)
        {

            dynamic jo = new JObject();

            jo.system = edobj.system;
            jo.updater = edobj.updater;
            jo.star = edobj.objectName;
            jo.spectral_class = edobj.Description;
            jo.spectral_subclass = edobj.subclass;
            jo.solar_mass = edobj.mass;
            jo.solar_radius = edobj.radius;
            jo.surface_temp = edobj.surfaceTemp;
            jo.star_age = edobj.star_age;
            jo.orbit_period = edobj.orbitPeriod;
            jo.arrival_point = edobj.arrivalPoint;
            jo.luminosity = edobj.luminosity;
            jo.notes = edobj.notes;
            jo.image_url = edobj.image_url;

            JObject joPost = new JObject(new JProperty("star", jo));

            if (edobj.id == 0)
            {
                var response = RequestSecurePost(joPost.ToString(), "api/v2/stars");
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    JObject jo2 = (JObject)JObject.Parse(response.Body);
                    JObject obj = (JObject)jo2["star"];
                    edobj.id = obj["id"].Value<int>();
                }
                else if ((int)response.StatusCode == 422)
                {
                    // Surprise, record is already there for some reason!
                    // I may create an api method on the server that negates the need to check for 
                    // this at some point
                    // - Greg

                    var queryParam = $"system={jo.system}&star={jo.star}&updater={jo.updater}";
                    response = RequestGet($"api/v2/stars?{queryParam}");
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        JObject jo2 = (JObject)JObject.Parse(response.Body);
                        JObject obj = (JObject)jo2["stars"][0];
                        edobj.id = obj["id"].Value<int>();

                        response = RequestSecurePatch(joPost.ToString(), "api/v2/stars/" + edobj.id.ToString());
                    }

                }
            }
            else
            {
                var response = RequestSecurePatch(joPost.ToString(), "api/v2/stars/" + edobj.id.ToString());
            }
            return true;


        }



        public bool DeletePlanetID(int id)
        {
            var response = RequestDelete("api/v2/world_surveys/"+id.ToString());
            
            return true;
        }

        public bool Delete(EDWorld obj)
        {
            if (obj.id > 0)
                return DeletePlanetID(obj.id);

            return true;
        }

    }
}
