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
        Star_L,
        Star_T,
        Star_Y,
        Star_Proto,
        Star_W,
        Star_C,
        Star_S,
        Star_TTauri,
        Star_WhiteDwarf,
        Star_AeBe,
        BlackHole,
        NeutronStar,


    }
        
    public class Planets
    {
        public ObjectTypesEnum type;


        public string Description
        {
            get
            {
                switch (type)
                {
                    case ObjectTypesEnum.UnknownObject:
                        return "?";
                    case ObjectTypesEnum.EarthLikeWorld: // FD
                        return "Earth-like world";
                    case ObjectTypesEnum.WaterWorld:  //FD
                        return "Water world";
                    case ObjectTypesEnum.MetalRich: // FD
                        return "Metal-rich body";
                    case ObjectTypesEnum.HighMetalContent:
                        return "High metal content";  // FD
                    case ObjectTypesEnum.Icy:   // FD
                        return "Icy body ";
                    case ObjectTypesEnum.Rocky:  // FD
                        return "Rocky body";
                    case ObjectTypesEnum.RockyIce:  // FD
                        return "Rocky ice world";
                    case ObjectTypesEnum.GasGiant_WaterBasedLife:
                        return "Gas Giant with water-based life";  // FD
                    case ObjectTypesEnum.GasGiant_AmmoniaBasedLife:
                        return "Gas Giant with ammonia-based life"; // FD
                    case ObjectTypesEnum.GasGiant_HeliumRich:
                        return "Gas Giant, Helium Rich";
                    case ObjectTypesEnum.Class_I_GasGiant:  //FD
                        return "Class I Gas Giant";
                    case ObjectTypesEnum.Class_II_GasGiant:  //FD
                        return "Class II Gas Giant";
                    case ObjectTypesEnum.Class_III_GasGiant: //FD
                        return "Class III Gas Giant";
                    case ObjectTypesEnum.Class_IV_GasGiant:
                        return "Class IV Gas Giant";
                    case ObjectTypesEnum.Class_V_GasGiant:
                        return "Class V Gas Giant";
                    case ObjectTypesEnum.WaterGiant:
                        return "Water Giant";
                    case ObjectTypesEnum.Belt:
                        return "Belt";
                    default:
                        return "";
                }
            }
        }


        public string ShortName
        {
            get
            {
                switch (type)
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
                        return "Gas Giant water life";
                    case ObjectTypesEnum.GasGiant_AmmoniaBasedLife:
                        return "Gas Giant ammonia life";
                    case ObjectTypesEnum.GasGiant_HeliumRich:
                        return "Gas Giant Helium Rich";
                    case ObjectTypesEnum.Class_I_GasGiant:
                        return "Class I Gas Giant";
                    case ObjectTypesEnum.Class_II_GasGiant:
                        return "Class II Gas Giant";
                    case ObjectTypesEnum.Class_III_GasGiant:
                        return "Class III Gas Giant";
                    case ObjectTypesEnum.Class_IV_GasGiant:
                        return "Class IV Gas Giant";
                    case ObjectTypesEnum.Class_V_GasGiant:
                        return "Class V Gas Giant";
                    case ObjectTypesEnum.WaterGiant:
                        return "Water Giant";
                    default:
                        return "";
                }
            }
        }




    }
}
