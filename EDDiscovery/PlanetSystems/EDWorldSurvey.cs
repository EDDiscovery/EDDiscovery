using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{

    public class EDWorldSurvey : EDObject
    {
        public int worldId;

        public EDWorldSurvey()
        {
        }

        public bool ParseJson(JObject jo)
        {

            id = jo["id"].Value<int>();
            var attributes = jo["attributes"];
            worldId = attributes["world-id"].Value<int>();


            //TODO: Not quite sure how to work with materials object in their current state, 
            //      but something like this should do it - Greg
            
            //foreach (var mat in mlist)
            //{
            //    materials[mat.material] = attributes[mat.Name.ToLower()].Value<bool>();
            //}
            return true;
        }


        public ObjectTypesEnum ShortName2ObjectType(string v)
        {
            EDWorld ed = new EDWorld();

            foreach (ObjectTypesEnum mat in Enum.GetValues(typeof(ObjectTypesEnum)))
            {
                ed.ObjectType = mat;
                if (v.ToLower().Equals(ed.ShortName.ToLower()))
                    return mat;

            }

            return ObjectTypesEnum.UnknownObject;
        }


        public MaterialEnum MaterialFromString(string v)
        {
            if (v == null)
                return MaterialEnum.Unknown;

            foreach (MaterialEnum mat in Enum.GetValues(typeof(MaterialEnum)))
            {
                if (v.ToLower().Equals(mat.ToString().ToLower()))
                    return mat;
            }

            return MaterialEnum.Unknown;
        }

    }
}
