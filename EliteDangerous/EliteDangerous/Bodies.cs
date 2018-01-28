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
using EDDiscovery.Icons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
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

        public static IReadOnlyDictionary<EDStar, Image> StarTypeIcons { get; } = new IconGroup<EDStar>("Stars");

        public static System.Drawing.Image GetStarTypeImage(EDStar type)
        {
            return StarTypeIcons[type];
        }

        public static IReadOnlyDictionary<EDPlanet, Image> PlanetTypeIcons { get; } = new IconGroup<EDPlanet>("Planets");

        public static System.Drawing.Image GetPlanetClassImage(EDPlanet type)
        {
            return PlanetTypeIcons[type];
        }

        public static string StarName( EDStar id )
        {
            switch (id)       // see journal, section 11.2
            {
                case EDStar.O:
                    return string.Format("Luminous Hot Main Sequence {0} star", id.ToString());

                case EDStar.B:
                    // also have an B1V
                    return string.Format("Luminous Blue Main Sequence {0} star", id.ToString());

                case EDStar.A:
                    // also have an A3V..
                    return string.Format("Bluish-White Main Sequence {0} star", id.ToString());

                case EDStar.F:
                    return string.Format("White Main Sequence {0} star", id.ToString());

                case EDStar.G:
                    // also have a G8V
                    return string.Format("Yellow Main Sequence {0} star", id.ToString());

                case EDStar.K:
                    // also have a K0V
                    return string.Format("Orange Main Sequence {0} star", id.ToString());
                case EDStar.M:
                    // also have a M1VA
                    return string.Format("Red Main Sequence {0} star", id.ToString());

                // dwarfs
                case EDStar.L:
                    return string.Format("Dark Red Non Main Sequence {0} star", id.ToString());
                case EDStar.T:
                    return string.Format("Methane Dwarf star");
                case EDStar.Y:
                    return string.Format("Brown Dwarf star");

                // proto stars
                case EDStar.AeBe:    // Herbig
                    return "Herbig Ae/Be";
                case EDStar.TTS:     // seen in logs
                    return "T Tauri";

                // wolf rayet
                case EDStar.W:
                case EDStar.WN:
                case EDStar.WNC:
                case EDStar.WC:
                case EDStar.WO:
                    return string.Format("Wolf-Rayet {0} star", id.ToString());

                // Carbon
                case EDStar.CS:
                case EDStar.C:
                case EDStar.CN:
                case EDStar.CJ:
                case EDStar.CHd:
                    return string.Format("Carbon {0} star", id.ToString());

                case EDStar.MS: //seen in log https://en.wikipedia.org/wiki/S-type_star
                    return string.Format("Intermediate low Zirconium Monoxide Type star");

                case EDStar.S:   // seen in log, data from http://elite-dangerous.wikia.com/wiki/Stars
                    return string.Format("Cool Giant Zirconium Monoxide rich Type star");

                // white dwarf
                case EDStar.D:
                case EDStar.DA:
                case EDStar.DAB:
                case EDStar.DAO:
                case EDStar.DAZ:
                case EDStar.DAV:
                case EDStar.DB:
                case EDStar.DBZ:
                case EDStar.DBV:
                case EDStar.DO:
                case EDStar.DOV:
                case EDStar.DQ:
                case EDStar.DC:
                case EDStar.DCV:
                case EDStar.DX:
                    return string.Format("White Dwarf {0} star", id.ToString());

                case EDStar.N:
                    return "Neutron Star";

                case EDStar.H:

                    return "Black Hole";

                case EDStar.X:
                    // currently speculative, not confirmed with actual data... in journal
                    return "Exotic";

                // Journal.. really?  need evidence these actually are formatted like this.

                case EDStar.SuperMassiveBlackHole:
                    return "Super Massive Black Hole";
                case EDStar.A_BlueWhiteSuperGiant:
                    return "Blue White Super Giant";
                case EDStar.F_WhiteSuperGiant:
                    return "F White Super Giant";
                case EDStar.M_RedSuperGiant:
                    return "M Red Super Giant";
                case EDStar.M_RedGiant:
                    return "M Red Giant";
                case EDStar.K_OrangeGiant:
                    return "K Orange Giant";
                case EDStar.RoguePlanet:
                    return "Rogue Planet";

                default:
                    return string.Format("Class {0} star\n", id.ToString());
            }
        }
    }
}
