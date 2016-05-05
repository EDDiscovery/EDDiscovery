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
    public class Survey : EdMaterializer
    {
        // There can be multiple surveys for any given world. Multiple commanders and they
        // can each create multiple records. See the Logs part of the Monster sheet to get
        // a feel for it
        public List<EDSurvey> GetForIds(List<int> ids)
        {
            var surveys = new List<EDSurvey>();
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var survey = GetForId(id);
                    if (survey != null )
                      surveys.Add(survey);
                }
            }
            return surveys;
        }

        // Use this to get the Survey given a world-survey id.
        // Easy to get to from a World object using the world-id attribute.
        public EDSurvey GetForId(int id)
        {
            if (id > 0)
            {
                var request = RequestGet($"{ApiNamespace}/surveys/{id}");
                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var jo = JObject.Parse(request.Body);
                    var data = jo["data"];
                    EDSurvey obj = new EDSurvey();

                    return (obj.ParseJson((JObject)data)) ? obj : null;
                }
            }

            return null;
        }

        public EDSurvey GetFirst(string scope)
        {
            var items = GetAll(scope);
            return (items.Count > 0) ? items[0] : null;
        }

        public List<EDSurvey> GetAll(string scope)
        {
            List<EDSurvey> listObjects = new List<EDSurvey>();
            string query = ApiNamespace + "/surveys";

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
                EDSurvey obj = new EDSurvey();

                if (obj.ParseJson((JObject)jo))
                    listObjects.Add(obj);
            }

            return listObjects;
        }

        // NOTE: With Stars and Worlds we get away with attempting to POST and resorting to a PATCH
        // if a record is already present. This won't work for Surveys because there is no uniqueness rule.
        // A user might create several survey records for a world each with differet Resource types.
        // Also they may make extra sets for each Basecamp if they're using Basecamps.
        // If in doubt search for a record first and create a new one if there isn't one in existence.
        public ResponseData Store(EDSurvey edobj)
        {
            dynamic jo = new JObject();


            ResponseData response;
            if (edobj.id == 0)
            {
                JObject joPost = Payload(edobj);

                response = RequestSecurePost(joPost.ToString(), $"{ApiNamespace}/surveys");
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

                response = RequestSecurePatch(joPatch.ToString(), $"{ApiNamespace}/surveys/" + edobj.id.ToString());
            }
            return response;
        }

        public ResponseData Delete(int id)
        {
            var response = RequestSecureDelete($"{ApiNamespace}/surveys/" + id.ToString());

            return response;
        }

        public ResponseData Delete(EDSurvey obj)
        {
            if (obj.id > 0)
                return Delete(obj.id);
            else
                return new ResponseData(HttpStatusCode.NotFound);
        }

        // Payload can be for Insert or Update. The difference is whether there is an id or not
        private JObject Payload(EDSurvey edobj, int id = 0)
        {
            // TODO: Add materials. They will be integers for this. Use Resource of "BINARY"
            // and 1s and 0s for a simple survey.

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

            var joPayload = new JObject {
                { "data", new JObject {
                    { "type", "surveys" },
                    { "attributes", new JObject {
                        { "commander", EDDiscoveryForm.EDDConfig.CurrentCommander.Name },
                        { "resource", edobj.resource },
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
            if (id>0)
            {
                var jData = joPayload["data"];
                jData["id"] = id;
            }

            return joPayload;
        }

    }
}
