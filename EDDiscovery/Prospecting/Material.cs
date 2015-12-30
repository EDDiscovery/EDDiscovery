using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.EliteDangerous
{
    public enum MaterialEnum
    {
        Unknown = 0,
        Carbon,
        Iron,
        Nickel,
        Phosphorus,
        Sulphur,
        Arsenic,
        Chromium,
        Germanium,
        Manganese,
        Selenium,
        Vanadium,
        Zinc,
        Zirconium,
        Cadmium,
        Mercury,
        Molybdenum,
        Niobium,
        Tin,
        Tungsten,
        Antimony,
        Polonium,
        Ruthenium,
        Technetium,
        Tellurium,
        Yttrium,
    }


    public enum ProspectNodeTypeEnum
    {
        Unknown = 0,
        Mesosiderite,
        BronziteChondrite,
        MetallicMeteorite,
        OutcropGeiger,
        OutcropMetallic,
    }




    public enum SynthesisEnum
    {
        Unknown = 0,
        FSDInjection,
        PlasmaMunitions,
        ExplosiveMunitions,
        SmallCalibreMunitions,
        HighVelocityMunitions,
        LargeCalibreMunitions,
        AFMRefill,
        SRVAmmoRestock,
        SRVRepair,
        SRVRefuel,
    }


    public class Material
    {
        public MaterialEnum material;
        public int number;


        public Material(MaterialEnum m)
        {
            material = m;
            number = 1;
        }
        public Material(MaterialEnum m, int nr)
        {
            material = m;
            number = nr;
        }
    }

    public class Synthesis
    {
        public string name;
        List<Material> materials;

    }
}
