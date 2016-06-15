using EDDiscovery;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{

    // Relationships:
    //   A World can have a single World Survey
    //   A World can have multiple Surveys
    public class EDWorld : EDObject
    {
        public string terraformable;
        public float gravity;
        public AtmosphereEnum atmosphere;
        public VulcanismEnum vulcanism;
        public int terrain_difficulty;
        public string Reserve;
        public float surfacePressure;
        public float rotationPeriod;

        public float semiMajorAxis;
        public float rockPct;
        public float metalPct;
        public float icePct;

        public int worldSurveyId;
        public List<int> surveyIds;

        static public List<EDWorld> listObjectTypes = EDWorld.GetEDObjList;
        
        static private Dictionary<string, ObjectTypesEnum> objectAliases = ObjectsType.GetAllTypesAlias();


        public EDWorld()
        {

            if (objectsTypes != null)
                type = objectsTypes[0];


        }


        public string ShortName
        {
            get
            {
                switch (ObjectType)
                {
                    case ObjectTypesEnum.UnknownObject:
                        return "?";
                    case ObjectTypesEnum.EarthLikeWorld:
                        return "ELW";
                    case ObjectTypesEnum.WaterWorld:
                        return "WW";
                    case ObjectTypesEnum.MetalRich:
                        return "Metal-rich";
                    case ObjectTypesEnum.HighMetalContent:
                        return "High Metal";
                    case ObjectTypesEnum.Icy:
                        return "Icy";
                    case ObjectTypesEnum.Rocky:
                        return "Rocky";
                    case ObjectTypesEnum.RockyIce:
                        return "Rocky Ice";
                    case ObjectTypesEnum.GasGiant_WaterBasedLife:
                        return "Giant water life";
                    case ObjectTypesEnum.GasGiant_AmmoniaBasedLife:
                        return "Giant ammonia life";
                    case ObjectTypesEnum.GasGiant_HeliumRich:
                        return "Giant Helium Rich";
                    case ObjectTypesEnum.Class_I_GasGiant:
                        return "Class I";
                    case ObjectTypesEnum.Class_II_GasGiant:
                        return "Class II";
                    case ObjectTypesEnum.Class_III_GasGiant:
                        return "Class III";
                    case ObjectTypesEnum.Class_IV_GasGiant:
                        return "Class IV";
                    case ObjectTypesEnum.Class_V_GasGiant:
                        return "Class V";
                    case ObjectTypesEnum.WaterGiant:
                        return "Water Giant";
                    default:
                        return ObjectType.ToString();
                }
            }
        }

        public static List<EDWorld> GetEDObjList
        {
            get
            {
                List<EDWorld> list = new List<EDWorld>();
                foreach (ObjectTypesEnum objtype in Enum.GetValues(typeof(ObjectTypesEnum)))
                {
                    EDWorld obj = new EDWorld();
                    obj.ObjectType = objtype;
                    list.Add(obj);
                }
                return list;
            }
        }


        public bool ParseJson(JObject jo)
        {

            id = jo["id"].Value<int>();
            var attributes = jo["attributes"];
            system = attributes["system-name"].Value<string>();
            objectName = attributes["world"].Value<string>();
            updater = attributes["updater"].Value<string>();

            ObjectType = String2ObjectType(attributes["world-type"].Value<string>());

            mass = Tools.GetFloat(attributes["mass"]);
            radius = Tools.GetFloat(attributes["radius"]);
            gravity = Tools.GetFloat(attributes["gravity"]);
            surfaceTemp = Tools.GetInt(attributes["surface-temp"]);
            surfacePressure = Tools.GetFloat(attributes["surface-pressure"]);

            orbitPeriod = Tools.GetFloat(attributes["orbit-period"]);
            rotationPeriod = Tools.GetFloat(attributes["rotation-period"]);
            semiMajorAxis = Tools.GetFloat(attributes["semi-major-axis"]);

            terrain_difficulty = Tools.GetInt(attributes["terrain-difficulty"]);
            vulcanism = (VulcanismEnum)VulcanismStr2Enum(attributes["vulcanism-type"].Value<string>());
            rockPct = Tools.GetFloat(attributes["rock-pct"]);
            metalPct = Tools.GetFloat(attributes["metal-pct"]);
            icePct = Tools.GetFloat(attributes["ice-pct"]);
            Reserve = Tools.GetString(attributes["reserve"]);

            arrivalPoint = Tools.GetFloat(attributes["arrival-point"]);
            terraformable = Tools.GetString(attributes["terraformable"]);
            notes = Tools.GetString(attributes["notes"]);
            atmosphere = (AtmosphereEnum)AtmosphereStr2Enum(attributes["atmosphere-type"].Value<string>());
            imageUrl = Tools.GetString(attributes["image-url"]);

            var relationships = (JObject) jo["relationships"];
            var data = relationships["world-survey"]["data"] as JObject;
            if (data != null)
                worldSurveyId = Tools.GetInt(data["id"]);

            // TODO: Make this lookup Lazy Loading to reduce on server
            // traffic and keep it fresh
            surveyIds = new List<int>();
            foreach(var survey in relationships["surveys"]["data"] as JArray)
            {
                surveyIds.Add(Tools.GetInt(survey["id"]));
            }
            return true;
        }


        
        public AtmosphereEnum AtmosphereStr2Enum(string v)
        {
            if (v == null)
                return AtmosphereEnum.Unknown;

            foreach (AtmosphereEnum mat in Enum.GetValues(typeof(AtmosphereEnum)))
            {
                string str = mat.ToString().Replace("_", "").ToLower();

                if (v.Replace("_", "").Replace(" ", "").ToLower().Equals(str))
                    return mat;

            }

            return AtmosphereEnum.Unknown;
        }

        public VulcanismEnum VulcanismStr2Enum(string v)
        {
            if (v == null)
                return VulcanismEnum.Unknown;

            foreach (VulcanismEnum mat in Enum.GetValues(typeof(VulcanismEnum)))
            {
                string str = mat.ToString().Replace("_", "").ToLower();

                if (v.Replace("_", "").Replace(" ", "").ToLower().Equals(str))
                    return mat;

            }

            return VulcanismEnum.Unknown;
        }

        // WorldSurvey related I'm guessing? -Greg
        //
        //public MaterialEnum MaterialFromString(string v)
        //{
        //    if (v == null)
        //        return MaterialEnum.Unknown;

        //    foreach (MaterialEnum mat in Enum.GetValues(typeof(MaterialEnum)))
        //    {
        //        if (v.ToLower().Equals(mat.ToString().ToLower()))
        //            return mat;
        //    }

        //    return MaterialEnum.Unknown;
        //}

        // Obtain a World Survey from a World object here!
        public EDWorldSurvey GetWorldSurvey()
        {
            if (worldSurveyId > 0)
            {
                Repositories.WorldSurvey worldSurveyRepo = new Repositories.WorldSurvey();
                return worldSurveyRepo.GetForId(worldSurveyId);
            }
            else
            {
                return null;
            }
        }

        public List<EDSurvey> GetSurveys()
        {
            Repositories.Survey surveyRepo = new Repositories.Survey();
            return surveyRepo.GetForIds(surveyIds);
        }
    }
}
