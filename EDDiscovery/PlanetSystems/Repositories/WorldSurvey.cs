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
    public class WorldSurvey : EdMaterializer
    {
        // Use this to get the WorldSurvey given a world-survey id.
        // Easy to get to from a World object using the world-id attribute.
        public EDWorldSurvey GetForId(int id)
        {
            if (id > 0)
            {
                var request = RequestGet($"{ApiNamespace}/world-surveys/{id}");
                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var jo = JObject.Parse(request.Body);
                    var data = jo["data"];
                    EDWorldSurvey obj = new EDWorldSurvey();

                    return (obj.ParseJson((JObject)data)) ? obj : null;
                }
            }

            return null;
        }

        public EDWorldSurvey GetFirst(string scope)
        {
            var items = GetAll(scope);
            return (items.Count > 0) ? items[0] : null;
        }

        public List<EDWorldSurvey> GetAll(string scope)
        {
            List<EDWorldSurvey> listObjects = new List<EDWorldSurvey>();
            string query = ApiNamespace + "/world-surveys";

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
                EDWorldSurvey obj = new EDWorldSurvey();

                if (obj.ParseJson((JObject)jo))
                    listObjects.Add(obj);
            }

            return listObjects;
        }

        // NO STORE METHOD BY DESIGN
        // World Surveys are now Read Only.
        //
        // The World Survey is calculated through updating of surveys. Note that the world_survey
        // record is summing of all commanders surveys for a world.
    }
}
