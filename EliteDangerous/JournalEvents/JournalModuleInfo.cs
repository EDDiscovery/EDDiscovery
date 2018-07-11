/*
 * Copyright © 2016-2018 EDDiscovery development team
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
    [System.Diagnostics.DebuggerDisplay("{ShipId} {Ship} {ShipModules.Count}")]
    [JournalEntryType(JournalTypeEnum.ModuleInfo)]
    public class JournalModuleInfo : JournalEntry , IAdditionalFiles
    {
        public JournalModuleInfo(JObject evt) : base(evt, JournalTypeEnum.ModuleInfo)
        {
            Rescan(evt);
        }

        public void Rescan(JObject evt)
        { 
            ShipModules = new List<ShipModule>();

            JArray jmodules = (JArray)evt["Modules"];
            if (jmodules != null)
            {
                foreach (JObject jo in jmodules)
                {
                    string slotfdname = JournalFieldNaming.NormaliseFDSlotName(jo["Slot"].Str());
                    string itemfdname = JournalFieldNaming.NormaliseFDItemName(jo["Item"].Str());

                    ShipModule module = new ShipModule( 
                                                        JournalFieldNaming.GetBetterSlotName(slotfdname),
                                                        slotfdname,
                                                        JournalFieldNaming.GetBetterItemName(itemfdname),
                                                        itemfdname,
                                                        null, // unknown
                                                        jo["Priority"].IntNull(),
                                                        null, // aclip
                                                        null, // ahooper
                                                        null, // health
                                                        null, // Value
                                                        jo["Power"].DoubleNull(),
                                                        null //engineering
                                                        );
                    ShipModules.Add(module);
                }

                ShipModules = ShipModules.OrderBy(x => x.Slot).ToList();            // sort for presentation..
            }
        }

        public bool ReadAdditionalFiles(string directory, ref JObject jo)
        {
            JObject jnew = ReadAdditionalFile(System.IO.Path.Combine(directory, "ModulesInfo.json"));
            if (jnew != null)        // new json, rescan
            {
                jo = jnew;      // replace current
                Rescan(jo);
            }
            return jnew != null;
        }

        public List<ShipModule> ShipModules;

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("Modules:".Txb(this), ShipModules.Count);
            detailed = "";

            foreach (ShipModule m in ShipModules)
            {
                double? power = (m.Power.HasValue && m.Power.Value > 0) ? m.Power : null;

                detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", m.Slot, "<:", m.Item, "; MW;0.###", power) , Environment.NewLine);
            }
        }
    }
}
