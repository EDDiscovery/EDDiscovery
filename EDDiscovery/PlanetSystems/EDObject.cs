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
        public string updater;
        protected ObjectTypesEnum objectType;
        protected ObjectsType type;

        public float arrivalPoint;
        public float radius;
        public float mass;
        public int surfaceTemp;
        public float orbitPeriod;

        public string notes;

        public DateTime updated_at;
        public DateTime created_at;
        public string creator; // Read only

        public string imageUrl;

        static protected List<ObjectsType> objectsTypes = ObjectsType.GetAllTypes();
        static private Dictionary<string, ObjectTypesEnum> objectAliases = ObjectsType.GetAllTypesAlias();



        public ObjectTypesEnum ObjectType
        {
            get
            {
                return objectType;
            }

            set
            {
                if (objectsTypes == null)
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


        public ObjectTypesEnum String2ObjectType(string v)
        {
            EDWorld ed = new EDWorld();
            if (v == null)
                return ObjectTypesEnum.UnknownObject;

            if (objectAliases.ContainsKey(v.ToLower()))
                return objectAliases[v.ToLower()];


            return ObjectTypesEnum.UnknownObject;
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


    }

}
