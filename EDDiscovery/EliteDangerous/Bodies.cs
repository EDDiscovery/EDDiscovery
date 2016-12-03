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

    public enum EDAtmospehere
    {
        Unknown = 0,
        No_atmosphere,
        Suitable_for_water_based_life,
        Ammonia_and_oxygen,

        Ammonia,
        Ammonia_rich,

        Water,
        Water_rich,
        Hot_Water,
        Hot_thick_water,

        Carbon_dioxide,
        Thin_carbon_dioxide,
        Carbon_dioxide_rich,
        Hot_thick_carbon_dioxide,



        Methane,
        Methane_rich,
        Thin_methane,
        Thick_methane,
        Thick_methane_rich,
        Hot_thick_methane_rich,

        Helium,
        Thin_helium,

        Argon,
        Argon_rich,
        Thick_argon_rich,

        Neon,
        Neon_rich,
        Thin_neon_rich,

        Sulphur_dioxide,
        Thin_sulphur_dioxide,

        Nitrogen,

        Silicate_vapour,
        Hot_thick_silicate_vapour,

        Metallic_vapour,

        Oxygen,
    }


    public enum EDVolcanism
    {
        Unknown = 0,
        None,
        Water_Magma = 100,
        Minor_Water_Magma,
        Major_Water_Magma,

        Sulphur_Dioxide_Magma = 200,
        Minor_Sulphur_Dioxide_Magma,
        Major_Sulphur_Dioxide_Magma,

        Ammonia_Magma = 300,
        Minor_Ammonia_Magma,
        Major_Ammonia_Magma,

        Methane_Magma = 400,
        Minor_Methane_Magma,
        Major_Methane_Magma,

        Nitrogen_Magma = 500,
        Minor_Nitrogen_Magma,
        Major_Nitrogen_Magma,

        Silicate_Magma = 600,
        Minor_Silicate_Magma,
        Major_Silicate_Magma,

        Metallic_Magma = 700,
        Minor_Metallic_Magma,
        Major_Metallic_Magma,

        Water_Geysers = 800,
        Minor_Water_Geysers,
        Major_Water_Geysers,

        Carbon_Dioxide_Geysers = 900,
        Minor_Carbon_Dioxide_Geysers,
        Major_Carbon_Dioxide_Geysers,

        Ammonia_Geysers = 1000,
        Minor_Ammonia_Geysers,
        Major_Ammonia_Geysers,

        Methane_Geysers = 1100,
        Minor_Methane_Geysers,
        Major_Methane_Geysers,

        Nitrogen_Geysers = 1200,
        Minor_Nitrogen_Geysers,
        Major_Nitrogen_Geysers,

        Helium_Geysers = 1300,
        Minor_Helium_Geysers,
        Major_Helium_Geysers,

        Silicate_Vapour_Geysers = 1400,
        Minor_Silicate_Vapour_Geysers,
        Major_Silicate_Vapour_Geysers,

        Rocky_Magma = 1500,
        Minor_Rocky_Magma,
        Major_Rocky_Magma,
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

        public static EDAtmospehere AtmosphereStr2Enum(string v)
        {
            if (v == null)
                return EDAtmospehere.Unknown;

            var searchstr = v.ToLower().Replace("_", "").Replace(" ", "").Replace("-", "").Replace("atmosphere", "");

            foreach (EDAtmospehere atm in Enum.GetValues(typeof(EDAtmospehere)))
            {
                string str = atm.ToString().Replace("_", "").ToLower();

                if (searchstr.Equals(str))
                    return atm;
            }

            System.Diagnostics.Trace.WriteLine("atm: " + v);

            return EDAtmospehere.Unknown;
        }

        public static EDVolcanism VolcanismStr2Enum(string v)
        {
            if (v == null)
                return EDVolcanism.Unknown;

            string searchstr = v.ToLower().Replace("_", "").Replace(" ", "").Replace("-", "").Replace("volcanism", "");

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
