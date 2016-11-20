using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.EliteDangerous
{
    public enum SynthesisEnum
    {
        FSDInjection1,
        FSDInjection2,
        FSDInjection3,
        /*        PlasmaMunitions,
                        ExplosiveMunitions,
                        SmallCalibreMunitions,
                        HighVelocityMunitions,
                        LargeCalibreMunitions,
                        AFMRefill,
                        SRVAmmoRestock,
                        SRVRepair,
                        SRVRefuel,
                        */
    }


    public class Synthesis
    {
        public string name;
        List<Material> materials;


        public Synthesis(SynthesisEnum mat)
        {
            name = mat.ToString();
            materials = new List<Material>();
            switch (mat)
            {
                case SynthesisEnum.FSDInjection1:
                    materials.Add(new Material(MaterialEnum.Germanium));
                    materials.Add(new Material(MaterialEnum.Vanadium, 2));
                    break;
                case SynthesisEnum.FSDInjection2:
                    materials.Add(new Material(MaterialEnum.Germanium));
                    materials.Add(new Material(MaterialEnum.Vanadium));
                    materials.Add(new Material(MaterialEnum.Cadmium,2));
                    materials.Add(new Material(MaterialEnum.Niobium));
                    break;
                case SynthesisEnum.FSDInjection3:
                    materials.Add(new Material(MaterialEnum.Arsenic));
                    materials.Add(new Material(MaterialEnum.Niobium, 3));
                    materials.Add(new Material(MaterialEnum.Polonium));
                    materials.Add(new Material(MaterialEnum.Yttrium));
                    break;

                default:
                    name = "unknown";
                    break;
            }
        }
    }

}
