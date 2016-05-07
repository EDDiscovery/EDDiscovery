using EDDiscovery;
using EDDiscovery2.HTTP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;

namespace EDDiscovery2.PlanetSystems.Repositories
{
    public class Basecamp : EdMaterializer
    {
        public List<EDBasecamp> GetForIds(List<int> ids)
        {
            var basecamps = new List<EDBasecamp>();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var basecamp = GetForId(id);
                    if (basecamp != null)
                        basecamps.Add(basecamp);
                }
            }
            return basecamps;
        }

        public EDBasecamp GetForId(int id)
        {
            if (id > 0)
            {
                var request = RequestGet($"{ApiNamespace}/basecamps/{id}");
                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var jo = JObject.Parse(request.Body);
                    var data = jo["data"];
                    EDBasecamp obj = new EDBasecamp();

                    return (obj.ParseJson((JObject)data)) ? obj : null;
                }
            }

            return null;
        }

        public EDBasecamp GetFirst(string scope)
        {
            var items = GetAll(scope);
            return (items.Count > 0) ? items[0] : null;
        }

        public List<EDBasecamp> GetAll(string scope)
        {
            List<EDBasecamp> listObjects = new List<EDBasecamp>();
            string query = ApiNamespace + "/basecamps";

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
                EDBasecamp obj = new EDBasecamp();

                if (obj.ParseJson((JObject)jo))
                    listObjects.Add(obj);
            }

            return listObjects;
        }

        // NOTE: With Stars and Worlds we get away with attempting to POST and resorting to a PATCH
        // if a record is already present. This won't work for Basecamps because there is no uniqueness rule.
        // A user might create several basecamp records for a world each with differet Resource types.
        // Also they may make extra sets for each Basecamp if they're using Basecamps.
        // If in doubt search for a record first and create a new one if there isn't one in existence.
        public ResponseData Store(EDBasecamp edobj)
        {
            dynamic jo = new JObject();


            ResponseData response;
            if (edobj.id == 0)
            {
                JObject joPost = Payload(edobj);

                response = RequestSecurePost(joPost.ToString(), $"{ApiNamespace}/basecamps");
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    JObject jo2 = (JObject)JObject.Parse(response.Body);
                    JObject obj = (JObject)jo2["data"];
                    edobj.id = obj["id"].Value<int>();
                }
            }
            else
            {
                JObject joPatch = Payload(edobj, edobj.id);

                response = RequestSecurePatch(joPatch.ToString(), $"{ApiNamespace}/basecamps/" + edobj.id.ToString());
            }
            return response;
        }

        public ResponseData Delete(int id)
        {
            var response = RequestSecureDelete($"{ApiNamespace}/basecamps/" + id.ToString());

            return response;
        }

        public ResponseData Delete(EDBasecamp obj)
        {
            if (obj.id > 0)
                return Delete(obj.id);
            else
                return new ResponseData(HttpStatusCode.NotFound);
        }

        // Payload can be for Insert or Update. The difference is whether there is an id or not
        private JObject Payload(EDBasecamp edobj, int id = 0)
        {
            var joPayload = new JObject {
                { "data", new JObject {
                    { "type", "basecamps" },
                    { "attributes", new JObject {
                        { "updater", EDDiscoveryForm.EDDConfig.CurrentCommander.Name },
                        { "name", edobj.name },
                        { "description", edobj.description },
                        { "landing-zone-terrain", edobj.landingZoneTerrain },
                        { "terrain-hue-1", edobj.terrainHue1 },
                        { "terrain-hue-2", edobj.terrainHue2 },
                        { "terrain-hue-3", edobj.terrainHue3 },
                        { "landing-zone-lat", edobj.landingZoneLat },
                        { "landing-zone-lon", edobj.landingZoneLon },
                        { "notes", edobj.notes },
                        { "images-url", edobj.imageUrl },
                    } },
                    { "relationships", new JObject {
                        { "worlds", new JObject {
                            { "id",  edobj.worldId },
                            { "type", "worlds" },
                        } },
                    } },
                } }
            };
            if (id > 0)
            {
                var jData = joPayload["data"];
                jData["id"] = id;
            }

            return joPayload;
        }

    }

}
