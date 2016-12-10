using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
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

        H,      // currently speculative, not confirmed with actual data...

        X,    // currently speculative, not confirmed with actual data... in journal

        A_BlueWhiteSuperGiant,
        F_WhiteSuperGiant,
        M_RedSuperGiant,
        M_RedGiant,
        K_OrangeGiant,
        RoguePlanet,
        Nebula,
        StellarRemnantNebula,
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


    public class Bodies
    {

        public static EDStar StarStr2Enum(string star)
        {
            if (star == null)
                return EDStar.Unknown;

            var searchstr = star.Replace("_", "").Replace(" ", "").Replace("-", "").ToLower();


            foreach (EDStar atm in Enum.GetValues(typeof(EDStar)))
            {
                string str = atm.ToString().Replace("_", "").ToLower();

                if (searchstr.Equals(str))
                    return atm;
            }

            return EDStar.Unknown;
        }

        public static EDPlanet PlanetStr2Enum(string star)
        {
            if (star == null)
                return EDPlanet.Unknown;

            var searchstr = star.Replace("_", "").Replace(" ", "").Replace("-", "").ToLower();

            foreach (EDPlanet atm in Enum.GetValues(typeof(EDPlanet)))
            {
                string str = atm.ToString().Replace("_", "").ToLower();

                if (searchstr.Equals(str))
                    return atm;
            }

            return EDPlanet.Unknown;
        }



        public static EDAtmosphereType AtmosphereStr2Enum(string v, out EDAtmosphereProperty atmprop)
        {
            atmprop = EDAtmosphereProperty.None;

            if (v == null)
                return EDAtmosphereType.Unknown;

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
                atmprop |= EDAtmosphereProperty.Thin;
                searchstr = searchstr.Replace("hot", "");
            }


            foreach (EDAtmosphereType atm in Enum.GetValues(typeof(EDAtmosphereType)))
            {
                string str = atm.ToString().Replace("_", "").ToLower();

                if (searchstr.Equals(str))
                    return atm;
            }

            System.Diagnostics.Trace.WriteLine("atm: " + v);

            return EDAtmosphereType.Unknown;
        }

        public static EDVolcanism VolcanismStr2Enum(string v, out EDVolcanismProperty vprop )
        {
            vprop = EDVolcanismProperty.None;
            if (v == null)
                return EDVolcanism.Unknown;

            string searchstr = v.ToLower().Replace("_", "").Replace(" ", "").Replace("-", "").Replace("volcanism", "");



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

            foreach (EDVolcanism atm in Enum.GetValues(typeof(EDVolcanism)))
            {
                string str = atm.ToString().Replace("_", "").ToLower();

                if (searchstr.Equals(str))
                    return atm;
            }

            return EDVolcanism.Unknown;
        }


    }
}
