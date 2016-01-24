using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
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
#else
            // Production
            _serverAddress = "http://ed-materializer-env.elasticbeanstalk.com/";
#endif
        }


        public List<EDObject>GetAll(string system)
        {
            List<EDObject> listObjects = new List<EDObject>();
            string query = "api/v1/world_surveys";

            if (!String.IsNullOrEmpty(system))
                query = query + "/?q[system]="+HttpUtility.UrlEncode(system);

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
                EDObject obj = new EDObject();

                if (obj.ParseJson(jo))
                    listObjects.Add(obj);
            }


            return listObjects;
        }

        public bool Store(EDObject edobj)
        {
            


            dynamic jo = new JObject();

            jo.system = edobj.system;
            jo.commander = edobj.commander;
            jo.world = edobj.objectName;
            jo.world_type = edobj.Description;
            jo.terraformable = edobj.terraformable;
            if (edobj.gravity>0)
                jo.gravity = edobj.gravity;
            jo.terrain_difficulty = edobj.terrain_difficulty;
            jo.notes = edobj.notes;

            if (edobj.arrivalPoint>0)
                jo.arrival_point = edobj.arrivalPoint;

            jo.atmosphere_type = edobj.atmosphere;
            jo.vulcanism_type = edobj.vulcanism;

            if (edobj.radius>0)
                jo.radius = edobj.radius;

            jo.carbon = edobj.materials[MaterialEnum.Carbon];
            jo.iron = edobj.materials[MaterialEnum.Iron];
            jo.nickel = edobj.materials[MaterialEnum.Nickel];
            jo.phosphorus = edobj.materials[MaterialEnum.Phosphorus];
            jo.sulphur = edobj.materials[MaterialEnum.Sulphur];
            jo.arsenic = edobj.materials[MaterialEnum.Arsenic];
            jo.chromium = edobj.materials[MaterialEnum.Chromium];
            jo.germanium = edobj.materials[MaterialEnum.Germanium];
            jo.manganese = edobj.materials[MaterialEnum.Manganese];
            jo.selenium = edobj.materials[MaterialEnum.Selenium];
            jo.vanadium = edobj.materials[MaterialEnum.Vanadium];
            jo.zinc = edobj.materials[MaterialEnum.Zinc];
            jo.zirconium = edobj.materials[MaterialEnum.Zirconium];
            jo.cadmium = edobj.materials[MaterialEnum.Cadmium];
            jo.mercury = edobj.materials[MaterialEnum.Mercury];
            jo.molybdenum = edobj.materials[MaterialEnum.Molybdenum];
            jo.niobium = edobj.materials[MaterialEnum.Niobium];
            jo.tin = edobj.materials[MaterialEnum.Tin];
            jo.tungsten = edobj.materials[MaterialEnum.Tungsten];
            jo.antimony = edobj.materials[MaterialEnum.Antimony];
            jo.polonium = edobj.materials[MaterialEnum.Polonium];
            jo.ruthenium = edobj.materials[MaterialEnum.Ruthenium];
            jo.technetium = edobj.materials[MaterialEnum.Technetium];
            jo.tellurium = edobj.materials[MaterialEnum.Tellurium];
            jo.yttrium = edobj.materials[MaterialEnum.Yttrium];

            JObject joPost = new JObject(new JProperty("world_survey", jo));

            // Suggestion: Instead of checking local data, just try a POST first. if the response 
            //             is a 422 then try again with a PATCH. This'll work even if the local data 
            //             is out of sync with the server.
            //
            //             Note: There is no enum for 422, so you'd have to check it as:
            //             response.StatusCode == (HttpStatusCode)422;
            //             - Greg
            if (edobj.id == 0)
            {
                var response = RequestSecurePost(joPost.ToString(), "api/v1/world_surveys");
                var json = response.Body;

                JObject jo2 = (JObject)JObject.Parse(json);
                JObject obj = (JObject)jo2["world_survey"];
                edobj.id = obj["id"].Value<int>();
            }
            else
            {
                var response = RequestPatch(joPost.ToString(), "api/v1/world_surveys/" + edobj.id.ToString());
            }
            return true;


        }

        public bool DeleteID(int id)
        {
            var response = RequestDelete("api/v1/world_surveys/"+id.ToString());
            
            return true;
        }

        public bool Delete(EDObject obj)
        {
            if (obj.id > 0)
                return DeleteID(obj.id);

            return true;
        }

    }
}
