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
    [JournalEntryType(JournalTypeEnum.DiscoveryScan)]
    public class JournalDiscoveryScan : JournalEntry
    {
        public JournalDiscoveryScan(JObject evt) : base(evt, JournalTypeEnum.DiscoveryScan)
        {
            SystemAddress = evt["SystemAddress"].Long();
            Bodies = evt["Bodies"].Int();
        }

        public long SystemAddress { get; set; }
        public int Bodies { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("New bodies discovered:", Bodies);
            detailed = "";
        }
    }
}
