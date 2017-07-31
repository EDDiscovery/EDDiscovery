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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
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
