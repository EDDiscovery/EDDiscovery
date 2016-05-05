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
    public class Star : EdMaterializer
    {
        public List<EDStar> GetAllForSystem(string system)
        {
            var scope = "system=" + HttpUtility.UrlEncode(system);
            return GetAll(scope);
        }

        public EDStar GetFirstForStar(string system, string star)
        {
            var scope = $"system={HttpUtility.UrlEncode(system)}&star={HttpUtility.UrlEncode(star)}";
            return GetFirst(scope);
        }

        public EDStar GetFirst(string scope)
        {
            var items = GetAll(scope);
            return (items.Count > 0) ? items[0] : null;
        }

        public List<EDStar> GetAll(string scope)
        {
            List<EDStar> listObjects = new List<EDStar>();
            string query = ApiNamespace + "/stars";

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
                EDStar obj = new EDStar();

                if (obj.ParseJson((JObject)jo))
                    listObjects.Add(obj);
            }

            return listObjects;
        }

        public ResponseData Store(EDStar edobj)
        {
            dynamic jo = new JObject();

            var joPost = new JObject {
                { "data", new JObject {
                    { "type", "stars" },
                    { "attributes", new JObject {
                        { "system-name", edobj.system },
                        { "updater", EDDiscoveryForm.EDDConfig.CurrentCommander.Name },
                        { "star", edobj.objectName },
                        { "spectral-class", edobj.Description },
                        { "spectral-subclass", edobj.subclass },
                        { "solar-mass", edobj.mass },
                        { "solar-radius", edobj.radius },
                        { "surface-temp", edobj.surfaceTemp },
                        { "star-age", edobj.star_age },
                        { "orbit-period", edobj.orbitPeriod },
                        { "arrival-point", edobj.arrivalPoint },
                        { "luminosity", edobj.luminosity },
                        { "notes", edobj.notes },
                        { "image-url", edobj.imageUrl }
                    } }
                } }
            };
  
            ResponseData response;
            if (edobj.id == 0)
            {
                response = RequestSecurePost(joPost.ToString(), $"{ApiNamespace}/stars");
                if (response.StatusCode == HttpStatusCode.Created)
                {
                    JObject jo2 = (JObject)JObject.Parse(response.Body);
                    JObject obj = (JObject)jo2["data"];
                    edobj.id = obj["id"].Value<int>();
                }
                else if ((int)response.StatusCode == 422)
                {
                    var queryParams = $"system={jo.system}&star={jo.star}";
                    var response2 = RequestGet($"{ApiNamespace}/stars/?{queryParams}");
                    if (response2.StatusCode == HttpStatusCode.OK)
                    {
                        JObject jo2 = (JObject)JObject.Parse(response2.Body);
                        JArray items = (JArray)jo2["data"];
                        if (items.Count > 0)
                        {
                            edobj.id = items[0]["id"].Value<int>();
                            JObject jData = (JObject)joPost["data"];
                            jData["id"] = edobj.id;
                            response2 = RequestSecurePatch(joPost.ToString(), $"{ApiNamespace}/stars/" + edobj.id.ToString());
                            response = response2;
                        }
                    }
                }
            }
            else
            {
                response = RequestSecurePatch(joPost.ToString(), $"{ApiNamespace}/stars/" + edobj.id.ToString());
            }
            return response;
        }

        public ResponseData Delete(int id)
        {
            var response = RequestSecureDelete($"{ApiNamespace}/stars/" + id.ToString());

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
