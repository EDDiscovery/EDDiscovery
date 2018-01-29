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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    //    When written:
    //Parameters:
    //•	Modules: array of installed items, each with:
    //o Slot: slot name
    //o Item: module name
    //o Power: module power
    //o Priority: power priority

    [System.Diagnostics.DebuggerDisplay("{ShipId} {Ship} {ShipModules.Count}")]
    [JournalEntryType(JournalTypeEnum.ModuleInfo)]
    public class JournalModuleInfo : JournalEntry
    {
        public JournalModuleInfo(JObject evt) : base(evt, JournalTypeEnum.ModuleInfo)
        {
            ShipModules = new List<JournalLoadout.ShipModule>();

            JArray jmodules = (JArray)evt["Modules"];
            if (jmodules != null)
            {
                foreach (JObject jo in jmodules)
                {
                    JournalLoadout.ShipModule module = new JournalLoadout.ShipModule( 
                                                        JournalFieldNaming.GetBetterSlotName(jo["Slot"].Str()),
                                                        JournalFieldNaming.NormaliseFDSlotName(jo["Slot"].Str()),
                                                        JournalFieldNaming.GetBetterItemNameLoadout(jo["Item"].Str()),
                                                        JournalFieldNaming.NormaliseFDItemName(jo["Item"].Str()),
                                                        null, // unknown
                                                        jo["Priority"].IntNull(),
                                                        null, //aclip
                                                        null, //ahooper
                                                        null, //health
                                                        null, //value
                                                        jo["Power"].DoubleNull(),
                                                        null //engineering
                                                        );
                    ShipModules.Add(module);
                }

                ShipModules = ShipModules.OrderBy(x => x.Slot).ToList();            // sort for presentation..
            }
        }

        public List<JournalLoadout.ShipModule> ShipModules;

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Modules:", ShipModules.Count);
            detailed = "";

            foreach (JournalLoadout.ShipModule m in ShipModules)
            {
                if (detailed.Length > 0)
                    detailed += Environment.NewLine;

                double? power = (m.Power.HasValue && m.Power.Value > 0) ? m.Power : null;

                detailed += BaseUtils.FieldBuilder.Build("", m.Slot, "<:", m.Item, "Power:; MW;0.###", power);
            }
        }
    }
}
