using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{

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
            system = jo["system"].Value<string>();
            objectName = jo["world"].Value<string>();
            updater = jo["updater"].Value<string>();

            ObjectType = String2ObjectType(jo["world_type"].Value<string>());

            mass = GetFloat(jo["mass"]);
            radius = GetFloat(jo["radius"]);
            gravity = GetFloat(jo["gravity"]);
            surfaceTemp = GetInt(jo["surface_temp"]);
            surfacePressure = GetFloat(jo["surface_pressure"]);

            orbitPeriod = GetFloat(jo["orbit_period"]);
            rotationPeriod = GetFloat(jo["rotation_period"]);
            semiMajorAxis = GetFloat(jo["semi_major_axis"]);

            terrain_difficulty = GetInt(jo["terrain_difficulty"]);
            vulcanism = (VulcanismEnum)VulcanismStr2Enum(jo["vulcanism_type"].Value<string>());
            rockPct = GetFloat(jo["rock_pct"]);
            metalPct = GetFloat(jo["metal_pct"]);
            icePct = GetFloat(jo["ice_pct"]);
            Reserve = GetString(jo["reserve"]);

            arrivalPoint = GetFloat(jo["arrival_point"]);
            terraformable = GetString(jo["terraformable"]);
            notes = GetString(jo["notes"]);
            atmosphere = (AtmosphereEnum)AtmosphereStr2Enum(jo["atmosphere_type"].Value<string>());
            image_url = GetString(jo["image_url"]);


            //foreach (var mat in mlist)
            //{
            //    materials[mat.material] = GetBool(jo[mat.Name.ToLower()]);
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
