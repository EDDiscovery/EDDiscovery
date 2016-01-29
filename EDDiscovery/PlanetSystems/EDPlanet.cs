using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{

    public class EDPlanet : EDObject
    {
        public string terraformable;
        public float gravity;
        public AtmosphereEnum atmosphere;
        public VulcanismEnum vulcanism;
        public int terrain_difficulty;
        public Dictionary<MaterialEnum, bool> materials;
        public string Reserve;
        public float Surface_preasure;




        static private List<Material> mlist = Material.GetMaterialList;
        static public List<EDPlanet> listObjectTypes = EDPlanet.GetEDObjList;
        




        public EDPlanet()
        {
            materials = new Dictionary<MaterialEnum, bool>();

            if (objectsTypes != null)
                type = objectsTypes[0];
            // Create an empty dictionary
            foreach (MaterialEnum mat in Enum.GetValues(typeof(MaterialEnum)))
            {
                if (mat != MaterialEnum.Unknown)
                    materials[mat] = false;

            }


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
                    case ObjectTypesEnum.Unknown_Star:
                        return "Star unknown";
                    case ObjectTypesEnum.Star_O:
                        return "O";
                    case ObjectTypesEnum.Star_B:
                        return "B";
                    case ObjectTypesEnum.Star_A:
                        return "A";
                    case ObjectTypesEnum.Star_F:
                        return "F";
                    case ObjectTypesEnum.Star_G:
                        return "G";
                    case ObjectTypesEnum.Star_K:
                        return "K";
                    case ObjectTypesEnum.Star_L:
                        return "L";
                    case ObjectTypesEnum.Star_T:
                        return "T";
                    case ObjectTypesEnum.Star_Y:
                        return "Y";
                    //           case ObjectTypesEnum.Star_Proto:
                    //               return "Proto";
                    case ObjectTypesEnum.Star_W:
                        return "W";
                    case ObjectTypesEnum.Star_C:
                        return "C";
                    case ObjectTypesEnum.Star_S:
                        return "S";
                    case ObjectTypesEnum.Star_TTauri:
                        return "T Tauri";
                    //         case ObjectTypesEnum.Star_WhiteDwarf:
                    //             return "DA";
                    case ObjectTypesEnum.Star_AeBe:
                        return "AeBe";
                    case ObjectTypesEnum.BlackHole:
                        return "Black hole";

                    default:
                        return ObjectType.ToString();
                }
            }
        }

        public static List<EDPlanet> GetEDObjList
        {
            get
            {
                List<EDPlanet> list = new List<EDPlanet>();
                foreach (ObjectTypesEnum objtype in Enum.GetValues(typeof(ObjectTypesEnum)))
                {
                    EDPlanet obj = new EDPlanet();
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
            commander = jo["commander"].Value<string>();

            ObjectType = String2ObjectType(jo["world_type"].Value<string>());
            terraformable = jo["terraformable"].Value<string>();
            gravity = GetFloat(jo["gravity"]);
            terrain_difficulty = GetInt(jo["terrain_difficulty"]);

            radius = GetFloat(jo["radius"]);
            arrivalPoint = GetFloat(jo["arrival_point"]);
            atmosphere = (AtmosphereEnum)AtmosphereStr2Enum(jo["atmosphere_type"].Value<string>());
            vulcanism = (VulcanismEnum)VulcanismStr2Enum(jo["vulcanism_type"].Value<string>());

            foreach (var mat in mlist)
            {
                materials[mat.material] = GetBool(jo[mat.Name.ToLower()]);
            }
            return true;
        }





        public ObjectTypesEnum ShortName2ObjectType(string v)
        {
            EDPlanet ed = new EDPlanet();

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
