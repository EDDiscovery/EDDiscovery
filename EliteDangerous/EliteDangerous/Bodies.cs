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
using System.Drawing;
using EliteDangerousCore;
using System.Linq;

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
        B_BlueWhiteSuperGiant,
        G_WhiteSuperGiant,
    };

    public enum EDPlanet
    {
        Unknown_Body_Type = 0,
        Metal_rich_body = 1000,     // no idea why it does this, but keeping it
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

            var searchstr = star.Replace("_", "").Replace(" ", "").Replace("-", "").ToLowerInvariant();

            if (StarStr2EnumLookup.ContainsKey(searchstr))
                return StarStr2EnumLookup[searchstr];

            return EDStar.Unknown;
        }

        private static Dictionary<string, EDPlanet> PlanetStr2EnumLookup;

        public static EDPlanet PlanetStr2Enum(string star)
        {
            if (star == null)
                return EDPlanet.Unknown_Body_Type;

            if (PlanetStr2EnumLookup == null)
            {
                PlanetStr2EnumLookup = new Dictionary<string, EDPlanet>(StringComparer.InvariantCultureIgnoreCase);
                foreach (EDPlanet atm in Enum.GetValues(typeof(EDPlanet)))
                {
                    PlanetStr2EnumLookup[atm.ToString().Replace("_", "")] = atm;
                }
            }

            var searchstr = star.Replace("_", "").Replace(" ", "").Replace("-", "").ToLowerInvariant();

            if (PlanetStr2EnumLookup.ContainsKey(searchstr))
                return PlanetStr2EnumLookup[searchstr];

            return EDPlanet.Unknown_Body_Type;
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

            var searchstr = v.ToLowerInvariant().Replace("_", "").Replace(" ", "").Replace("-", "").Replace("atmosphere", "");

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

            string searchstr = v.ToLowerInvariant().Replace("_", "").Replace(" ", "").Replace("-", "").Replace("volcanism", "");

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

            var searchstr = star.Replace("_", "").Replace(" ", "").Replace("-", "").ToLowerInvariant();

            if (ReserveStr2EnumLookup.ContainsKey(searchstr))
                return ReserveStr2EnumLookup[searchstr];

            return EDReserve.None;
        }

        public static string StarName( EDStar id )
        {
            switch (id)       // see journal, section 11.2
            {
                case EDStar.O:
                    return string.Format("Luminous Hot Main Sequence {0} star".T(EDTx.Bodies_HMS), id.ToString());

                case EDStar.B:
                    // also have an B1V
                    return string.Format("Luminous Blue Main Sequence {0} star".T(EDTx.Bodies_BMS), id.ToString());

                case EDStar.A:
                    // also have an A3V..
                    return string.Format("Bluish-White Main Sequence {0} star".T(EDTx.Bodies_BWMS), id.ToString());

                case EDStar.F:
                    return string.Format("White Main Sequence {0} star".T(EDTx.Bodies_WMS), id.ToString());

                case EDStar.G:
                    // also have a G8V
                    return string.Format("Yellow Main Sequence {0} star".T(EDTx.Bodies_YMS), id.ToString());

                case EDStar.K:
                    // also have a K0V
                    return string.Format("Orange Main Sequence {0} star".T(EDTx.Bodies_OMS), id.ToString());
                case EDStar.M:
                    // also have a M1VA
                    return string.Format("Red Main Sequence {0} star".T(EDTx.Bodies_RMS), id.ToString());

                // dwarfs
                case EDStar.L:
                    return string.Format("Dark Red Non Main Sequence {0} star".T(EDTx.Bodies_DRNS), id.ToString());
                case EDStar.T:
                    return string.Format("Methane Dwarf star".T(EDTx.Bodies_MD));
                case EDStar.Y:
                    return string.Format("Brown Dwarf star".T(EDTx.Bodies_BD));

                // proto stars
                case EDStar.AeBe:    // Herbig
                    return "Herbig Ae/Be".T(EDTx.Bodies_Herbig);
                case EDStar.TTS:     // seen in logs
                    return "T Tauri".T(EDTx.Bodies_TTauri);

                // wolf rayet
                case EDStar.W:
                case EDStar.WN:
                case EDStar.WNC:
                case EDStar.WC:
                case EDStar.WO:
                    return string.Format("Wolf-Rayet {0} star".T(EDTx.Bodies_WR), id.ToString());

                // Carbon
                case EDStar.CS:
                case EDStar.C:
                case EDStar.CN:
                case EDStar.CJ:
                case EDStar.CHd:
                    return string.Format("Carbon {0} star".T(EDTx.Bodies_C), id.ToString());

                case EDStar.MS: //seen in log https://en.wikipedia.org/wiki/S-type_star
                    return string.Format("Intermediate low Zirconium Monoxide Type star".T(EDTx.Bodies_IZ));

                case EDStar.S:   // seen in log, data from http://elite-dangerous.wikia.com/wiki/Stars
                    return string.Format("Cool Giant Zirconium Monoxide rich Type star".T(EDTx.Bodies_CGZ));

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
                    return string.Format("White Dwarf {0} star".T(EDTx.Bodies_WD), id.ToString());

                case EDStar.N:
                    return "Neutron Star".T(EDTx.Bodies_NS);

                case EDStar.H:

                    return "Black Hole".T(EDTx.Bodies_BH);

                case EDStar.X:
                    // currently speculative, not confirmed with actual data... in journal
                    return "Exotic".T(EDTx.Bodies_EX);

                // Journal.. really?  need evidence these actually are formatted like this.

                case EDStar.SuperMassiveBlackHole:
                    return "Super Massive Black Hole".T(EDTx.Bodies_SMBH);
                case EDStar.A_BlueWhiteSuperGiant:
                    return "Blue White Super Giant".T(EDTx.Bodies_BSG);
                case EDStar.F_WhiteSuperGiant:
                    return "F White Super Giant".T(EDTx.Bodies_WSG);
                case EDStar.M_RedSuperGiant:
                    return "M Red Super Giant".T(EDTx.Bodies_MSR);
                case EDStar.M_RedGiant:
                    return "M Red Giant".T(EDTx.Bodies_MOG);
                case EDStar.K_OrangeGiant:
                    return "K Orange Giant".T(EDTx.Bodies_KOG);
                case EDStar.RoguePlanet:
                    return "Rogue Planet".T(EDTx.Bodies_RP);

                default:
                    return string.Format("Class {0} star".T(EDTx.Bodies_UNK), id.ToString());
            }
        }

           // These should be translated to match the in-game star types
        private static readonly Dictionary<EDStar, string> StarEnumToNameLookup = new Dictionary<EDStar, string>
        {
            [EDStar.O] = "O (Blue-White) Star".T(EDTx.EDStar_O),
            [EDStar.B] = "B (Blue-White) Star".T(EDTx.EDStar_B),
            [EDStar.B_BlueWhiteSuperGiant] = "B (Blue-White super giant) Star".T(EDTx.EDStar_BBlueWhiteSuperGiant),
            [EDStar.A] = "A (Blue-White) Star".T(EDTx.EDStar_A),
            [EDStar.A_BlueWhiteSuperGiant] = "A (Blue-White super giant) Star".T(EDTx.EDStar_ABlueWhiteSuperGiant),
            [EDStar.F] = "F (White) Star".T(EDTx.EDStar_F),
            [EDStar.F_WhiteSuperGiant] = "F (White super giant) Star".T(EDTx.EDStar_FWhiteSuperGiant),
            [EDStar.G] = "G (White-Yellow) Star".T(EDTx.EDStar_G),
            [EDStar.G_WhiteSuperGiant] = "G (White-Yellow super giant) Star".T(EDTx.EDStar_GWhiteSuperGiant),
            [EDStar.K] = "K (Yellow-Orange) Star".T(EDTx.EDStar_K),
            [EDStar.K_OrangeGiant] = "K (Yellow-Orange giant) Star".T(EDTx.EDStar_KOrangeGiant),
            [EDStar.M] = "M (Red dwarf) Star".T(EDTx.EDStar_M),
            [EDStar.M_RedGiant] = "M (Red giant) Star".T(EDTx.EDStar_MRedGiant),
            [EDStar.M_RedSuperGiant] = "M (Red super giant) Star".T(EDTx.EDStar_MRedSuperGiant),
            [EDStar.L] = "L (Brown dwarf) Star".T(EDTx.EDStar_L),
            [EDStar.T] = "T (Brown dwarf) Star".T(EDTx.EDStar_T),
            [EDStar.Y] = "Y (Brown dwarf) Star".T(EDTx.EDStar_Y),
            [EDStar.TTS] = "T Tauri Star".T(EDTx.EDStar_TTS),
            [EDStar.AeBe] = "Herbig Ae/Be Star".T(EDTx.EDStar_AeBe),
            [EDStar.W] = "Wolf-Rayet Star".T(EDTx.EDStar_W),
            [EDStar.WN] = "Wolf-Rayet N Star".T(EDTx.EDStar_WN),
            [EDStar.WNC] = "Wolf-Rayet NC Star".T(EDTx.EDStar_WNC),
            [EDStar.WC] = "Wolf-Rayet C Star".T(EDTx.EDStar_WC),
            [EDStar.WO] = "Wolf-Rayet O Star".T(EDTx.EDStar_WO),
            [EDStar.CS] = "CS Star".T(EDTx.EDStar_CS),
            [EDStar.C] = "C Star".T(EDTx.EDStar_C),
            [EDStar.CN] = "CN Star".T(EDTx.EDStar_CN),
            [EDStar.CJ] = "CJ Star".T(EDTx.EDStar_CJ),
            [EDStar.CHd] = "CHd Star".T(EDTx.EDStar_CHd),
            [EDStar.MS] = "MS-type Star".T(EDTx.EDStar_MS),
            [EDStar.S] = "S-type Star".T(EDTx.EDStar_S),
            [EDStar.D] = "White Dwarf (D) Star".T(EDTx.EDStar_D),
            [EDStar.DA] = "White Dwarf (DA) Star".T(EDTx.EDStar_DA),
            [EDStar.DAB] = "White Dwarf (DAB) Star".T(EDTx.EDStar_DAB),
            [EDStar.DAO] = "White Dwarf (DAO) Star".T(EDTx.EDStar_DAO),
            [EDStar.DAZ] = "White Dwarf (DAZ) Star".T(EDTx.EDStar_DAZ),
            [EDStar.DAV] = "White Dwarf (DAV) Star".T(EDTx.EDStar_DAV),
            [EDStar.DB] = "White Dwarf (DB) Star".T(EDTx.EDStar_DB),
            [EDStar.DBZ] = "White Dwarf (DBZ) Star".T(EDTx.EDStar_DBZ),
            [EDStar.DBV] = "White Dwarf (DBV) Star".T(EDTx.EDStar_DBV),
            [EDStar.DO] = "White Dwarf (DO) Star".T(EDTx.EDStar_DO),
            [EDStar.DOV] = "White Dwarf (DOV) Star".T(EDTx.EDStar_DOV),
            [EDStar.DQ] = "White Dwarf (DQ) Star".T(EDTx.EDStar_DQ),
            [EDStar.DC] = "White Dwarf (DC) Star".T(EDTx.EDStar_DC),
            [EDStar.DCV] = "White Dwarf (DCV) Star".T(EDTx.EDStar_DCV),
            [EDStar.DX] = "White Dwarf (DX) Star".T(EDTx.EDStar_DX),
            [EDStar.N] = "Neutron Star".T(EDTx.EDStar_N),
            [EDStar.H] = "Black Hole".T(EDTx.EDStar_H),
            [EDStar.SuperMassiveBlackHole] = "Supermassive Black Hole".T(EDTx.EDStar_SuperMassiveBlackHole),
            [EDStar.Unknown] = "Unknown star type".T(EDTx.EDStar_Unknown),
        };

        // Removed the reverse lookups since they are english dependent. If we need them they are in 11.0.0.   They are not used and i'm worried
        // they would get used on the translated strings above to reverse it.  So thats why they are gone.

        public static List<EDStar> StarTypes
        {
            get
            {
                return StarEnumToNameLookup.Keys.ToList();
            }
        }

        public static string StarTypeNameShorter(EDStar type)
        {
            if (StarEnumToNameLookup.TryGetValue(type, out var name))
            {
                return name;
            }
            else
            {
                return type.ToString().Replace("_", " ");
            }
        }

        // These should be translated to match the in-game planet types
        private static readonly Dictionary<EDPlanet, string> PlanetEnumToNameLookup = new Dictionary<EDPlanet, string>
        {
            [EDPlanet.Metal_rich_body] = "Metal-rich body".T(EDTx.EDPlanet_Metalrichbody),
            [EDPlanet.High_metal_content_body] = "High metal content world".T(EDTx.EDPlanet_Highmetalcontentbody),
            [EDPlanet.Rocky_body] = "Rocky body".T(EDTx.EDPlanet_Rockybody),
            [EDPlanet.Icy_body] = "Icy body".T(EDTx.EDPlanet_Icybody),
            [EDPlanet.Rocky_ice_body] = "Rocky ice world".T(EDTx.EDPlanet_Rockyicebody),
            [EDPlanet.Earthlike_body] = "Earth-like world".T(EDTx.EDPlanet_Earthlikebody),
            [EDPlanet.Water_world] = "Water world".T(EDTx.EDPlanet_Waterworld),
            [EDPlanet.Ammonia_world] = "Ammonia world".T(EDTx.EDPlanet_Ammoniaworld),
            [EDPlanet.Water_giant] = "Water giant".T(EDTx.EDPlanet_Watergiant),
            [EDPlanet.Water_giant_with_life] = "Water giant with life".T(EDTx.EDPlanet_Watergiantwithlife),
            [EDPlanet.Gas_giant_with_water_based_life] = "Gas giant with water-based life".T(EDTx.EDPlanet_Gasgiantwithwaterbasedlife),
            [EDPlanet.Gas_giant_with_ammonia_based_life] = "Gas giant with ammonia-based life".T(EDTx.EDPlanet_Gasgiantwithammoniabasedlife),
            [EDPlanet.Sudarsky_class_I_gas_giant] = "Class I gas giant".T(EDTx.EDPlanet_SudarskyclassIgasgiant),
            [EDPlanet.Sudarsky_class_II_gas_giant] = "Class II gas giant".T(EDTx.EDPlanet_SudarskyclassIIgasgiant),
            [EDPlanet.Sudarsky_class_III_gas_giant] = "Class III gas giant".T(EDTx.EDPlanet_SudarskyclassIIIgasgiant),
            [EDPlanet.Sudarsky_class_IV_gas_giant] = "Class IV gas giant".T(EDTx.EDPlanet_SudarskyclassIVgasgiant),
            [EDPlanet.Sudarsky_class_V_gas_giant] = "Class V gas giant".T(EDTx.EDPlanet_SudarskyclassVgasgiant),
            [EDPlanet.Helium_rich_gas_giant] = "Helium-rich gas giant".T(EDTx.EDPlanet_Heliumrichgasgiant),
            [EDPlanet.Helium_gas_giant] = "Helium gas giant".T(EDTx.EDPlanet_Heliumgasgiant),
            [EDPlanet.Unknown_Body_Type] = "Unknown planet type".T(EDTx.EDPlanet_Unknown),
        };

        public static string PlanetTypeName(EDPlanet type)
        {
            string name;
            if (PlanetEnumToNameLookup.TryGetValue(type, out name))
            {
                return name;
            }
            else
            {
                return type.ToString().Replace("_", " ");
            }
        }

        public static bool AmmoniaWorld(EDPlanet PlanetTypeID) { return PlanetTypeID == EDPlanet.Ammonia_world; }
        public static bool Earthlike(EDPlanet PlanetTypeID) { return PlanetTypeID == EDPlanet.Earthlike_body; } 
        public static bool WaterWorld(EDPlanet PlanetTypeID) { return PlanetTypeID == EDPlanet.Water_world; } 
        public static bool SudarskyGasGiant(EDPlanet PlanetTypeID) { return PlanetTypeID >= EDPlanet.Sudarsky_class_I_gas_giant && PlanetTypeID <= EDPlanet.Sudarsky_class_V_gas_giant; }
        public static string SudarskyClass(EDPlanet PlanetTypeID) { return (new string[] { "I", "II", "III", "IV", "V" })[(int)(PlanetTypeID-EDPlanet.Sudarsky_class_I_gas_giant)]; }
        public static bool GasGiant(EDPlanet PlanetTypeID) { return PlanetTypeID >= EDPlanet.Gas_giant_with_water_based_life && PlanetTypeID <= EDPlanet.Gas_giant_with_ammonia_based_life; }
        public static bool WaterGiant(EDPlanet PlanetTypeID) { return PlanetTypeID >= EDPlanet.Water_giant && PlanetTypeID <= EDPlanet.Water_giant_with_life; }
        public static bool HeliumGasGiant(EDPlanet PlanetTypeID) { return PlanetTypeID >= EDPlanet.Helium_rich_gas_giant && PlanetTypeID <= EDPlanet.Helium_gas_giant; }

        private static string[] ClassificationAbv = new string[]
        {
            "MR","HMC","R","I","R+I","E","W","A","WG","WGL","GWL","GAL","S-I","S-II","S-III","S-IV","S-V","HRG","HG"
        };

        public static string PlanetAbv(EDPlanet PlanetTypeID)
        {
            if (PlanetTypeID == EDPlanet.Unknown_Body_Type)
                return "U";
            else
                return ClassificationAbv[(int)PlanetTypeID - (int)EDPlanet.Metal_rich_body];
        }
    }
}
