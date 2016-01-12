using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace EDDiscovery2.PlanetSystems
{
    public class EdMaterializer : HttpCom
    {


        public EdMaterializer()
        {
            ServerAdress = "https://ed-materializer.herokuapp.com/";
        }


        public List<EDObject>GetAll(string system)
        {
            List<EDObject> listObjects = new List<EDObject>();
            string query = "api/v1/world_surveys";

            if (!String.IsNullOrEmpty(system))
                query = query + "/?q[system]="+HttpUtility.UrlEncode(system);

            string json = RequestGet(query);


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
    }
}
