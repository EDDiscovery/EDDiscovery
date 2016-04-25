using EDDiscovery;
using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace EDDiscovery2.PlanetSystems.Repositories
{
    public class World : EdMaterializer
    {


        public List<EDWorld> GetAllForSystem(string system)
        {
            var scope = $"system={HttpUtility.UrlEncode(system)}";
            return GetAll(scope);
        }

        public EDWorld GetFirstForWorld(string system, string world)
        {
            var scope = $"system={HttpUtility.UrlEncode(system)}&world={HttpUtility.UrlEncode(world)}";
            return GetFirst(scope);
        }

        public EDWorld GetFirst(string scope)
        {
            var items = GetAll(scope);
            return (items.Count > 0) ? items[0] : null;           
        }

        public List<EDWorld> GetAll(string scope)
        {
            List<EDWorld> listObjects = new List<EDWorld>();
            string query = ApiNamespace + "/worlds";

            if (!String.IsNullOrEmpty(scope))
                query = query + "?" + scope;

            var response = RequestGet(query);
            var json = response.Body;

            JArray jArray = null;
            JObject jObject = null;
            if (json != null && json.Length > 5)
                jObject = (JObject)JObject.Parse(json);

            if (jObject == null)
                return listObjects;

            jArray = (JArray)jObject["data"];

            foreach (JObject jo in jArray)
            {
                EDWorld obj = new EDWorld();

                if (obj.ParseJson((JObject)jo))
                    listObjects.Add(obj);
            }

            return listObjects;
        }

        public ResponseData Store(EDWorld edobj)
        {
            dynamic jo = new JObject();

            jo.system = edobj.system;
            jo.updater = EDDiscoveryForm.EDDConfig.CurrentCommander.Name;
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

            JObject joPost = new JObject(new JProperty("world", jo));

            ResponseData response;
            if (edobj.id == 0)
            {
                response = RequestSecurePost(joPost.ToString(), $"{ApiNamespace}/worlds");
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    JObject jo2 = (JObject)JObject.Parse(response.Body);
                    JObject obj = (JObject)jo2["data"];
                    edobj.id = obj["id"].Value<int>();
                }
                    else if ((int)response.StatusCode == 422)
                {
                    var queryParams = $"system={jo.system}&world={jo.world}";
                    var response2 = RequestGet($"{ApiNamespace}/worlds?{queryParams}");
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        JObject jo2 = (JObject)JObject.Parse(response2.Body);
                        JArray items = (JArray)jo2["data"];
                        if (items.Count > 0)
                        {
                            edobj.id = items[0]["id"].Value<int>();
                            response2 = RequestSecurePatch(joPost.ToString(), $"{ApiNamespace}/worlds/" + edobj.id.ToString());
                            response = response2;
                        }
                    }
                }
            }
            else
            {
                response = RequestSecurePatch(joPost.ToString(), $"{ApiNamespace}/worlds/" + edobj.id.ToString());
            }
            return response;
        }

        public ResponseData Delete(int id)
        {
            var response = RequestSecureDelete($"{ApiNamespace}/worlds/" + id.ToString());

            return response;
        }

        public ResponseData Delete(EDWorld obj)
        {
            if (obj.id > 0)
                return Delete(obj.id);
            else
                return new ResponseData(HttpStatusCode.NotFound);
        }

    }
}
