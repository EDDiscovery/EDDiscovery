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
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    /*
     When written: when repairing modules using the Auto Field Maintenance Unit (AFMU)
    Parameters:
     Module: module name
     FullyRepaired: (bool)
     Health; (float 0.0..1.0)
    If the AFMU runs out of ammo, the module may not be fully repaired.
    Example:
    { "timestamp":"2017-08-14T15:41:50Z", "event":"AfmuRepairs",
    "Module":"$modularcargobaydoor_name;", "Module_Localised":"Cargo Hatch",
    "FullyRepaired":true, "Health":1.000000 } 
     */
    [JournalEntryType(JournalTypeEnum.AfmuRepairs)]
    public class JournalAfmuRepairs : JournalEntry
    {
        public JournalAfmuRepairs(JObject evt) : base(evt, JournalTypeEnum.AfmuRepairs)
        {
            Module = JournalFieldNaming.GetBetterItemNameEvents(evt["Module"].Str());
            ModuleLocalised = evt["Module_Localised"].Str().Alt(Module);
            FullyRepaired = evt["FullyRepaired"].Bool();
            Health = evt["Health"].Float()*100.0F;
        }

        public string Module { get; set; }
        public string ModuleLocalised { get; set; }
        public bool FullyRepaired { get; set; }
        public float Health { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", ModuleLocalised, "Health:;%", (int)Health, ";Fully Repaired", FullyRepaired);
            detailed = "";
        }
    }
}
