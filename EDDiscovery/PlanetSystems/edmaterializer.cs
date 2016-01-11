using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{
    public class EdMaterializer : HttpCom
    {


        public EdMaterializer()
        {
            ServerAdress = "https://ed-materializer.herokuapp.com/";
        }


        public List<EDObject>GetAll()
        {
            List<EDObject> listObjects = new List<EDObject>();
            string json = RequestGet("api/v1/world_surveys");


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
