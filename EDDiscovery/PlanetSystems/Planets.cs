using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{
    public enum ObjectTypesEnum
    {
        UnknownObject = 0,
        EarthLikeWorld,
        WaterWorld,
        AmmoniaWorld,
        MetalRich,
        HighMetalContent,
        Icy,
        Rocky,
        RockyIce,
        GasGiant_WaterBasedLife,
        GasGiant_AmmoniaBasedLife,
        GasGiant_HeliumRich,
        Class_I_GasGiant,
        Class_II_GasGiant,
        Class_III_GasGiant,
        Class_IV_GasGiant,
        Class_V_GasGiant,
        WaterGiant,
        Belt,


        Unknown_Star = 100,
        Star_O,
        Star_B,
        Star_A,
        Star_F,
        Star_G,
        Star_K,
        Star_M, 
        Star_L,
        Star_T,
        Star_Y,

        Star_W,
        Star_WN,
        Star_WNC,
        Star_WC,
        Star_WO,
        Star_C,
        Star_S,
        Star_TTauri,
        Star_AeBe,

        Star_GA,
        Star_GF,
        Star_GK,
        Star_GM,
        Star_SGM,
        Star_GS, 
        Star_MS,

        Star_CN,

        Star_DA,
        Star_DB,
        Star_DAB, 
        Star_DC,
        Star_DCV,
        Star_N,
        BlackHole,
        SuperBlackHole,
    }

  


    public enum VulcanismEnum
    {
        Unknown = 0,
        No_volcanism,
        Iron_magma,
        Silicate_magma,
        Water_magma,
        Silicate_vapour_geysers,
        Carbon_dioxide_geysers,
        Water_geysers,
        Methane_magma,
        Nitrogen_magma,
        Ammonia_magma,
    }

public enum AtmosphereEnum
{
    Unknown = 0,
        No_atmosphere,
        Suitable_for_water_based_life,
        Nitrogen,
        Carbon_dioxide,
        Sulphur_dioxide,
        Argon,
        Neon,
        Neon_rich,
        Argon_rich,
        Nitrogen_rich,
        Water_rich,
        Carbon_dioxide_rich,
        Methane_rich,
        Silicate_vapour,
        Methane,
        Helium,
        Ammonia,
        Ammonia_and_oxygen,
        Water,
    }

    /*
      <AtmosphereComponents>
        NITROGEN,
        OXYGEN,
        WATER,
        NEON,
        CARBON DIOXIDE,
        AMMONIA,
        METHANE,
        SULPHUR_DIOXIDE,
        HYDROGEN,
        HELIUM,
        ARGON,
        IRON,
        SILICATES,
      </AtmosphereComponents>
      <SolidComponents>
        METAL,
        ROCK,
        ICE,
      </SolidComponents>
      <RingTypes>
        METALLIC,
        METAL RICH,
        ROCKY,
        ICY,
      </RingTypes>
      <MiningReserves>
        Pristine reserves,
        Major reserves,
        Common reserves,
        Low reserves,
        Depleted reserves,
      </MiningReserves>
      <Terraforming>
        This body is a candidate for terraforming.,
        This world has been terraformed.,
      </Terraforming>
      */

    public class EDObject
    {
        public int id;
        public string system;
        public string objectName;
        public string commander;
        private ObjectTypesEnum objectType;
        private ObjectsType type;
        public string terraformable;
        public float gravity;
        public float arrivalPoint;
        public float radius;
        public AtmosphereEnum atmosphere;
        public VulcanismEnum vulcanism;
        public int terrain_difficulty;
        public string notes;
        public Dictionary<MaterialEnum, bool> materials;
        public DateTime updated_at;
        public DateTime created_at;

        static private List<Material> mlist = Material.GetMaterialList;
        static public List<EDObject> listObjectTypes = EDObject.GetEDObjList;
        static private List<ObjectsType> objectsTypes = ObjectsType.GetAllTypes();

        static private Dictionary<string, ObjectTypesEnum>  objectAliases = ObjectsType.GetAllTypesAlias();


        public EDObject()
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

        public ObjectTypesEnum ObjectType
        {
            get
            {
                return objectType;
            }

            set
            {
                if (objectsTypes==null)
                    objectsTypes = ObjectsType.GetAllTypes();

                objectType = value;
                type = objectsTypes.Where(obj => obj.type == value).FirstOrDefault<ObjectsType>();
            }
        }


        public ObjectsType Type
        {
            get
            {
                return type;
            }
        }

        public string Description
        {
            get
            {
                if (type == null)
                    return "";
                return Type.Short;
            }
        }

        public bool IsPlanet
        {
            get
            {
                return type.Planet;
            }
        }

        public bool IsStar
        {
            get
            {
                return type.Star;
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
                        return "Metal-rich" ;
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

        public static List<EDObject> GetEDObjList
        { get
            {
                List<EDObject> list = new List<EDObject>();
                foreach (ObjectTypesEnum objtype in Enum.GetValues(typeof(ObjectTypesEnum)))
                {
                    EDObject obj = new EDObject();
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
            terrain_difficulty  = GetInt(jo["terrain_difficulty"]);

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

        private bool GetBool(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return false;
            return jToken.Value<bool>();
        }

        private float GetFloat(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return 0f;
            return jToken.Value<float>();
        }


        private int GetInt(JToken jToken)
        {
            if (IsNullOrEmptyT(jToken))
                return 0;
            return jToken.Value<int>();
        }

        public bool IsNullOrEmptyT(JToken token)
        {
            return (token == null) ||
                   (token.Type == JTokenType.Array && !token.HasValues) ||
                   (token.Type == JTokenType.Object && !token.HasValues) ||
                   (token.Type == JTokenType.String && token.ToString() == String.Empty) ||
                   (token.Type == JTokenType.Null);
        }

        public ObjectTypesEnum String2ObjectType(string v)
        {
            EDObject ed = new EDObject();


            if (objectAliases.ContainsKey(v.ToLower()))
                return objectAliases[v.ToLower()];
         

            return ObjectTypesEnum.UnknownObject;
        }

        public ObjectTypesEnum ShortName2ObjectType(string v)
        {
            EDObject ed = new EDObject();

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
