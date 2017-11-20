/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 * 
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public enum EDStar
    {
        Unknown = 0,
        O = 1,
        B,
        A,
        F,
        G,
        K,
        M,

        // Dwarf
        L,
        T,
        Y,

        // proto stars
        AeBe,
        TTS,


        // wolf rayet
        W,
        WN,
        WNC,
        WC,
        WO,

        // Carbon
        CS,
        C,
        CN,
        CJ,
        CHd,


        MS,  //seen in log
        S,   // seen in log

        // white dwarf
        D,
        DA,
        DAB,
        DAO,
        DAZ,
        DAV,
        DB,
        DBZ,
        DBV,
        DO,
        DOV,
        DQ,
        DC,
        DCV,
        DX,

        N,   // Neutron

        H,   // Black Hole

        X,    // currently speculative, not confirmed with actual data... in journal

        A_BlueWhiteSuperGiant,
        F_WhiteSuperGiant,
        M_RedSuperGiant,
        M_RedGiant,
        K_OrangeGiant,
        RoguePlanet,
        Nebula,
        StellarRemnantNebula,
        SuperMassiveBlackHole,
    };


    public enum EDPlanet
    {
        Unknown = 0,

        Metal_rich_body = 1000,
        High_metal_content_body,
        Rocky_body,
        Icy_body,
        Rocky_ice_body,
        Earthlike_body,
        Water_world,
        Ammonia_world,
        Water_giant,
        Water_giant_with_life,
        Gas_giant_with_water_based_life,
        Gas_giant_with_ammonia_based_life,
        Sudarsky_class_I_gas_giant,
        Sudarsky_class_II_gas_giant,
        Sudarsky_class_III_gas_giant,
        Sudarsky_class_IV_gas_giant,
        Sudarsky_class_V_gas_giant,
        Helium_rich_gas_giant,
        Helium_gas_giant,

        // Custom types
        High_metal_content_body_700 = 2000,
        High_metal_content_body_250,
        High_metal_content_body_hot_thick,
    }

    [Flags]
    public enum EDAtmosphereProperty
    {
        None = 0,
        Rich = 1,
        Thick = 2,
        Thin = 4,
        Hot = 8,
    }

    public enum EDAtmosphereType
    {
        Unknown = 0,
        No_atmosphere,
        Suitable_for_water_based_life,
        Ammonia_and_oxygen,

        Ammonia = 1000,
        Water = 2000,
        Carbon_dioxide = 3000,
        Methane = 4000,
        Helium = 5000,
        Argon = 6000,
        Neon = 7000,
        Sulphur_dioxide = 8000,
        Nitrogen = 9000,
        Silicate_vapour = 10000,
        Metallic_vapour = 11000,
        Oxygen = 12000,
    }


    [Flags]
    public enum EDVolcanismProperty
    {
        None = 0,
        Minor = 1,
        Major = 2,
    }

    public enum EDVolcanism
    {
        Unknown = 0,
        None,
        Water_Magma = 100,
        Sulphur_Dioxide_Magma = 200,
        Ammonia_Magma = 300,
        Methane_Magma = 400,
        Nitrogen_Magma = 500,
        Silicate_Magma = 600,
        Metallic_Magma = 700,
        Water_Geysers = 800,
        Carbon_Dioxide_Geysers = 900,
        Ammonia_Geysers = 1000,
        Methane_Geysers = 1100,
        Nitrogen_Geysers = 1200,
        Helium_Geysers = 1300,
        Silicate_Vapour_Geysers = 1400,
        Rocky_Magma = 1500,
    }

    public enum EDReserve
    {
        None = 0,
        Depleted,
        Low,
        Common,
        Major,
        Pristine,
    }

   

    public class Bodies
    {
        private static Dictionary<string, EDStar> StarStr2EnumLookup;

        public static EDStar StarStr2Enum(string star)
        {
            if (star == null)
                return EDStar.Unknown;

            if (StarStr2EnumLookup == null)
            {
                StarStr2EnumLookup = new Dictionary<string, EDStar>(StringComparer.InvariantCultureIgnoreCase);
                foreach (EDStar atm in Enum.GetValues(typeof(EDStar)))
                {
                    StarStr2EnumLookup[atm.ToString().Replace("_", "")] = atm;
                }
            }

            var searchstr = star.Replace("_", "").Replace(" ", "").Replace("-", "").ToLower();

            if (StarStr2EnumLookup.ContainsKey(searchstr))
                return StarStr2EnumLookup[searchstr];

            return EDStar.Unknown;
        }

        private static Dictionary<string, EDPlanet> PlanetStr2EnumLookup;

        public static EDPlanet PlanetStr2Enum(string star)
        {
            if (star == null)
                return EDPlanet.Unknown;

            if (PlanetStr2EnumLookup == null)
            {
                PlanetStr2EnumLookup = new Dictionary<string, EDPlanet>(StringComparer.InvariantCultureIgnoreCase);
                foreach (EDPlanet atm in Enum.GetValues(typeof(EDPlanet)))
                {
                    PlanetStr2EnumLookup[atm.ToString().Replace("_", "")] = atm;
                }
            }

            var searchstr = star.Replace("_", "").Replace(" ", "").Replace("-", "").ToLower();

            if (PlanetStr2EnumLookup.ContainsKey(searchstr))
                return PlanetStr2EnumLookup[searchstr];

            return EDPlanet.Unknown;
        }

        private static Dictionary<string, EDAtmosphereType> AtmosphereStr2EnumLookup;

        public static EDAtmosphereType AtmosphereStr2Enum(string v, out EDAtmosphereProperty atmprop)
        {
            atmprop = EDAtmosphereProperty.None;

            if (v == null)
                return EDAtmosphereType.Unknown;

            if (AtmosphereStr2EnumLookup == null)
            {
                AtmosphereStr2EnumLookup = new Dictionary<string, EDAtmosphereType>(StringComparer.InvariantCultureIgnoreCase);
                foreach (EDAtmosphereType atm in Enum.GetValues(typeof(EDAtmosphereType)))
                {
                    AtmosphereStr2EnumLookup[atm.ToString().Replace("_", "")] = atm;
                }
            }

            var searchstr = v.ToLower().Replace("_", "").Replace(" ", "").Replace("-", "").Replace("atmosphere", "");

            if (searchstr.Contains("rich"))
            {
                atmprop |= EDAtmosphereProperty.Rich;
                searchstr = searchstr.Replace("rich", "");
            }
            if (searchstr.Contains("thick"))
            {
                atmprop |= EDAtmosphereProperty.Thick;
                searchstr = searchstr.Replace("thick", "");
            }
            if (searchstr.Contains("thin"))
            {
                atmprop |= EDAtmosphereProperty.Thin;
                searchstr = searchstr.Replace("thin", "");
            }
            if (searchstr.Contains("hot"))
            {
                atmprop |= EDAtmosphereProperty.Hot;
                searchstr = searchstr.Replace("hot", "");
            }

            if (AtmosphereStr2EnumLookup.ContainsKey(searchstr))
                return AtmosphereStr2EnumLookup[searchstr];

           // System.Diagnostics.Trace.WriteLine("atm: " + v);

            return EDAtmosphereType.Unknown;
        }

        private static Dictionary<string, EDVolcanism> VolcanismStr2EnumLookup;

        public static EDVolcanism VolcanismStr2Enum(string v, out EDVolcanismProperty vprop )
        {
            vprop = EDVolcanismProperty.None;
            if (v == null)
                return EDVolcanism.Unknown;

            string searchstr = v.ToLower().Replace("_", "").Replace(" ", "").Replace("-", "").Replace("volcanism", "");

            if (VolcanismStr2EnumLookup == null)
            {
                VolcanismStr2EnumLookup = new Dictionary<string, EDVolcanism>(StringComparer.InvariantCultureIgnoreCase);
                foreach (EDVolcanism atm in Enum.GetValues(typeof(EDVolcanism)))
                {
                    VolcanismStr2EnumLookup[atm.ToString().Replace("_", "")] = atm;
                }
            }

            if (searchstr.Contains("minor"))
            {
                vprop |= EDVolcanismProperty.Minor;
                searchstr = searchstr.Replace("minor", "");
            }
            if (searchstr.Contains("major"))
            {
                vprop |= EDVolcanismProperty.Major;
                searchstr = searchstr.Replace("major", "");
            }

            if (VolcanismStr2EnumLookup.ContainsKey(searchstr))
                return VolcanismStr2EnumLookup[searchstr];

            return EDVolcanism.Unknown;
        }

        private static Dictionary<string, EDReserve> ReserveStr2EnumLookup;

        public static EDReserve ReserveStr2Enum(string star)
        {
            if (star == null)
                return EDReserve.None;

            if (ReserveStr2EnumLookup == null)
            {
                ReserveStr2EnumLookup = new Dictionary<string, EDReserve>(StringComparer.InvariantCultureIgnoreCase);
                foreach (EDReserve atm in Enum.GetValues(typeof(EDReserve)))
                {
                    ReserveStr2EnumLookup[atm.ToString().Replace("_", "")] = atm;
                }
            }

            var searchstr = star.Replace("_", "").Replace(" ", "").Replace("-", "").ToLower();

            if (ReserveStr2EnumLookup.ContainsKey(searchstr))
                return ReserveStr2EnumLookup[searchstr];

            return EDReserve.None;
        }

        #region Default Body Type Images
        private static Dictionary<EDStar, System.Drawing.Image> DefaultStarIcons = new Dictionary<EDStar, System.Drawing.Image>
        {
            // Main Sequence
            [EDStar.O] = EliteDangerous.Properties.Resources.O,
            [EDStar.B] = EliteDangerous.Properties.Resources.B6V_Blueish,
            [EDStar.A] = EliteDangerous.Properties.Resources.A9III_White,
            [EDStar.F] = EliteDangerous.Properties.Resources.F5VAB,
            [EDStar.G] = EliteDangerous.Properties.Resources.G1IV,
            [EDStar.K] = EliteDangerous.Properties.Resources.Star_K1IV,
            [EDStar.M] = EliteDangerous.Properties.Resources.M5V,
            // Giants
            [EDStar.A_BlueWhiteSuperGiant] = EliteDangerous.Properties.Resources.A9III_White,
            [EDStar.F_WhiteSuperGiant] = EliteDangerous.Properties.Resources.F5VAB,
            [EDStar.M_RedSuperGiant] = EliteDangerous.Properties.Resources.M5V,
            [EDStar.M_RedGiant] = EliteDangerous.Properties.Resources.M5V,
            [EDStar.K_OrangeGiant] = EliteDangerous.Properties.Resources.K0V,
            // Brown Dwarfs
            [EDStar.L] = EliteDangerous.Properties.Resources.L3V,
            [EDStar.T] = EliteDangerous.Properties.Resources.T4V,
            [EDStar.Y] = EliteDangerous.Properties.Resources.Y2,
            // Proto Stars
            [EDStar.AeBe] = EliteDangerous.Properties.Resources.DefaultStar,    // Herbig
            [EDStar.TTS] = EliteDangerous.Properties.Resources.DefaultStar,     // T Tauri
            // Wolf Rayet
            [EDStar.W] = EliteDangerous.Properties.Resources.WolfRayet,
            [EDStar.WN] = EliteDangerous.Properties.Resources.WolfRayet,
            [EDStar.WNC] = EliteDangerous.Properties.Resources.WolfRayet,
            [EDStar.WC] = EliteDangerous.Properties.Resources.WolfRayet,
            [EDStar.WO] = EliteDangerous.Properties.Resources.WolfRayet,
            // Carbon
            [EDStar.CS] = EliteDangerous.Properties.Resources.C7III,
            [EDStar.C] = EliteDangerous.Properties.Resources.C7III,
            [EDStar.CN] = EliteDangerous.Properties.Resources.C7III,
            [EDStar.CJ] = EliteDangerous.Properties.Resources.C7III,
            [EDStar.CHd] = EliteDangerous.Properties.Resources.C7III,
            // White Dwarf
            [EDStar.D] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DA] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DAB] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DAO] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DAZ] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DAV] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DB] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DBZ] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DBV] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DO] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DOV] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DQ] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DC] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DCV] = EliteDangerous.Properties.Resources.DA6VII_White,
            [EDStar.DX] = EliteDangerous.Properties.Resources.DA6VII_White,
            // S Stars - https://en.wikipedia.org/wiki/S-type_star
            [EDStar.MS] = EliteDangerous.Properties.Resources.M5V,
            [EDStar.S] = EliteDangerous.Properties.Resources.M5V,
            // Neutron Star
            [EDStar.N] = EliteDangerous.Properties.Resources.Neutron_Star,
            // Black Hole
            [EDStar.H] = EliteDangerous.Properties.Resources.Black_Hole,
            // Super-massive Black Hole
            [EDStar.SuperMassiveBlackHole] = EliteDangerous.Properties.Resources.Black_Hole,
            // Exotic objects (not yet seen)
            [EDStar.X] = EliteDangerous.Properties.Resources.Globe,
            [EDStar.RoguePlanet] = EliteDangerous.Properties.Resources.Globe,
            [EDStar.Nebula] = EliteDangerous.Properties.Resources.Globe,
            [EDStar.StellarRemnantNebula] = EliteDangerous.Properties.Resources.Globe,
            [EDStar.Unknown] = EliteDangerous.Properties.Resources.Globe_yellow
        };

        private static Dictionary<EDPlanet, System.Drawing.Image> DefaultPlanetIcons = new Dictionary<EDPlanet, System.Drawing.Image>
        {
            // Gas giants
            [EDPlanet.Helium_gas_giant] = EliteDangerous.Properties.Resources.Helium_Rich_Gas_Giant1,
            [EDPlanet.Helium_rich_gas_giant] = EliteDangerous.Properties.Resources.Helium_Rich_Gas_Giant1,
            [EDPlanet.Gas_giant_with_water_based_life] = EliteDangerous.Properties.Resources.Gas_giant_water_based_life_Brown3,
            [EDPlanet.Gas_giant_with_ammonia_based_life] = EliteDangerous.Properties.Resources.Gas_giant_ammonia_based_life1,
            [EDPlanet.Sudarsky_class_I_gas_giant] = EliteDangerous.Properties.Resources.Class_I_Gas_Giant_Brown2,
            [EDPlanet.Sudarsky_class_II_gas_giant] = EliteDangerous.Properties.Resources.Class_II_Gas_Giant_Sand1,
            [EDPlanet.Sudarsky_class_III_gas_giant] = EliteDangerous.Properties.Resources.Class_III_Gas_Giant_Blue3,
            [EDPlanet.Sudarsky_class_IV_gas_giant] = EliteDangerous.Properties.Resources.Class_I_Gas_Giant_Brown2,  // MISSING
            [EDPlanet.Sudarsky_class_V_gas_giant] = EliteDangerous.Properties.Resources.Class_I_Gas_Giant_Brown2,   // MISSING
            // Other giants
            [EDPlanet.Water_giant] = EliteDangerous.Properties.Resources.Water_Giant1,
            [EDPlanet.Water_giant_with_life] = EliteDangerous.Properties.Resources.Water_Giant1,
            // Rocky with atmospheres
            [EDPlanet.Water_world] = EliteDangerous.Properties.Resources.Water_World_Poles_Cloudless4,
            [EDPlanet.Earthlike_body] = EliteDangerous.Properties.Resources.Earth_Like_Standard,
            [EDPlanet.Ammonia_world] = EliteDangerous.Properties.Resources.Ammonia_Brown,
            [EDPlanet.High_metal_content_body_hot_thick] = EliteDangerous.Properties.Resources.High_metal_content_world_White3,
            // Other rocky bodies
            [EDPlanet.Icy_body] = EliteDangerous.Properties.Resources.Icy_Body_Greenish1,
            [EDPlanet.Rocky_ice_body] = EliteDangerous.Properties.Resources.Rocky_Ice_World_Sol_Titan,
            [EDPlanet.Rocky_body] = EliteDangerous.Properties.Resources.Rocky_Body_Sand2,
            [EDPlanet.High_metal_content_body] = EliteDangerous.Properties.Resources.High_metal_content_world_Orange8,
            [EDPlanet.High_metal_content_body_250] = EliteDangerous.Properties.Resources.High_metal_content_world_Mix3,
            [EDPlanet.High_metal_content_body_700] = EliteDangerous.Properties.Resources.High_metal_content_world_Lava1,
            [EDPlanet.Metal_rich_body] = EliteDangerous.Properties.Resources.metal_rich,
            [EDPlanet.Unknown] = EliteDangerous.Properties.Resources.Globe,
        };
        #endregion

        public static System.Drawing.Image GetStarTypeImage(EDStar type)
        {
            Dictionary<EDStar, System.Drawing.Image> icons = EliteConfigInstance.InstanceIconSet?.StarTypeIcons;

            if (icons != null && icons.ContainsKey(type))
            {
                return icons[type];
            }
            else if (Bodies.DefaultStarIcons.ContainsKey(type))
            {
                return Bodies.DefaultStarIcons[type];
            }
            else if (icons.ContainsKey(EDStar.Unknown))
            {
                return icons[EDStar.Unknown];
            }
            else
            {
                return EliteDangerous.Properties.Resources.DefaultStar;
            }
        }

        public static System.Drawing.Image GetPlanetClassImage(EDPlanet type)
        {
            Dictionary<EDPlanet, System.Drawing.Image> icons = EliteConfigInstance.InstanceIconSet?.PlanetTypeIcons;

            if (icons != null && icons.ContainsKey(type))
            {
                return icons[type];
            }
            else if (Bodies.DefaultPlanetIcons.ContainsKey(type))
            {
                return Bodies.DefaultPlanetIcons[type];
            }
            else if (icons != null && icons.ContainsKey(EDPlanet.Unknown))
            {
                return icons[EDPlanet.Unknown];
            }
            else
            {
                return EliteDangerous.Properties.Resources.Globe;
            }
        }

    }
}
