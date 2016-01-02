using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.PlanetSystems
{
    public enum PlanetTypesEnum
    {
        Unknown = 0,
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
    }
        
    public class Planets
    {
        public PlanetTypesEnum type;


        public string Description
        {
            get
            {
                switch (type)
                {
                    case PlanetTypesEnum.Unknown:
                        return "?";
                    case PlanetTypesEnum.EarthLikeWorld: // FD
                        return "Earth-like world";
                    case PlanetTypesEnum.WaterWorld:  //FD
                        return "Water world";
                    case PlanetTypesEnum.MetalRich: // FD
                        return "Metal-rich body";
                    case PlanetTypesEnum.HighMetalContent:
                        return "High metal content";  // FD
                    case PlanetTypesEnum.Icy:   // FD
                        return "Icy body ";
                    case PlanetTypesEnum.Rocky:  // FD
                        return "Rocky body";
                    case PlanetTypesEnum.RockyIce:  // FD
                        return "Rocky ice world";
                    case PlanetTypesEnum.GasGiant_WaterBasedLife:
                        return "Gas Giant with water-based life";  // FD
                    case PlanetTypesEnum.GasGiant_AmmoniaBasedLife:
                        return "Gas Giant with ammonia-based life"; // FD
                    case PlanetTypesEnum.GasGiant_HeliumRich:
                        return "Gas Giant, Helium Rich";
                    case PlanetTypesEnum.Class_I_GasGiant:  //FD
                        return "Class I Gas Giant";
                    case PlanetTypesEnum.Class_II_GasGiant:  //FD
                        return "Class II Gas Giant";
                    case PlanetTypesEnum.Class_III_GasGiant: //FD
                        return "Class III Gas Giant";
                    case PlanetTypesEnum.Class_IV_GasGiant:
                        return "Class IV Gas Giant";
                    case PlanetTypesEnum.Class_V_GasGiant:
                        return "Class V Gas Giant";
                    case PlanetTypesEnum.WaterGiant:
                        return "Water Giant";
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
                    case PlanetTypesEnum.Unknown:
                        return "?";
                    case PlanetTypesEnum.EarthLikeWorld:
                        return "ELW";
                    case PlanetTypesEnum.WaterWorld:
                        return "WW";
                    case PlanetTypesEnum.MetalRich:
                        return "Metal-rich" ;
                    case PlanetTypesEnum.HighMetalContent:
                        return "High Metal";
                    case PlanetTypesEnum.Icy:
                        return "Icy";
                    case PlanetTypesEnum.Rocky:
                        return "Rocky";
                    case PlanetTypesEnum.RockyIce:
                        return "Rocky Ice";
                    case PlanetTypesEnum.GasGiant_WaterBasedLife:
                        return "Gas Giant water life";
                    case PlanetTypesEnum.GasGiant_AmmoniaBasedLife:
                        return "Gas Giant ammonia life";
                    case PlanetTypesEnum.GasGiant_HeliumRich:
                        return "Gas Giant Helium Rich";
                    case PlanetTypesEnum.Class_I_GasGiant:
                        return "Class I Gas Giant";
                    case PlanetTypesEnum.Class_II_GasGiant:
                        return "Class II Gas Giant";
                    case PlanetTypesEnum.Class_III_GasGiant:
                        return "Class III Gas Giant";
                    case PlanetTypesEnum.Class_IV_GasGiant:
                        return "Class IV Gas Giant";
                    case PlanetTypesEnum.Class_V_GasGiant:
                        return "Class V Gas Giant";
                    case PlanetTypesEnum.WaterGiant:
                        return "Water Giant";
                    default:
                        return "";
                }
            }
        }




    }
}
