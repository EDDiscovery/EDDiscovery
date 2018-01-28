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
        [System.Diagnostics.DebuggerDisplay("{Slot} {Item} {LocalisedItem}")]
        public class ShipModuleInfo
        {
            public string Slot { get; private set; }        // never null       - nice name, used to track 
            public string SlotFD { get; private set; }      // never null       - FD normalised ID name (Int_CargoRack_Size6_Class1)
            public string Item { get; private set; }        // never null       - nice name, used to track
            public string ItemFD { get; private set; }        // never null     - FD normalised ID name

            public double? Power { get; private set; }
            public int? Priority { get; private set; }      // 0..4 not 1..5

            public ShipModuleInfo()
            { }

            public ShipModuleInfo(string s, string sfd, string i, string ifd, double? w, int? p)
            {
                Slot = s; SlotFD = sfd; Item = i; ItemFD = ifd; Power = w; Priority = p;
            }

            public ShipModuleInfo(string s, string sfd, string i, string ifd)
            {
                Slot = s; SlotFD = sfd; Item = i; ItemFD = ifd;
            }

            public bool Same(ShipModuleInfo other)      // ignore localisased item, it does not occur everywhere..
            {
                return (Slot == other.Slot && Item == other.Item && Power == other.Power && Priority == other.Priority);
            }

            public bool Same(string item )
            {
                return Item == item;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder(64);
                sb.AppendFormat("{0} = {1}", Slot, Item);
                return sb.ToString();
            }
        }

        public JournalModuleInfo(JObject evt) : base(evt, JournalTypeEnum.ModuleInfo)
        {
            ShipModules = new List<ShipModuleInfo>();

            JArray jmodules = (JArray)evt["Modules"];
            if (jmodules != null)
            {
                foreach (JObject jo in jmodules)
                {
                    ShipModuleInfo module = new ShipModuleInfo( 
                                                        JournalFieldNaming.GetBetterSlotName(jo["Slot"].Str()),
                                                        JournalFieldNaming.NormaliseFDSlotName(jo["Slot"].Str()),
                                                        JournalFieldNaming.GetBetterItemNameLoadout(jo["Item"].Str()),
                                                        JournalFieldNaming.NormaliseFDItemName(jo["Item"].Str()),
                                                        jo["Power"].DoubleNull(),
                                                        jo["Priority"].IntNull() );
                    ShipModules.Add(module);
                }

                ShipModules = ShipModules.OrderBy(x => x.Slot).ToList();            // sort for presentation..
            }
        }

        public List<ShipModuleInfo> ShipModules;

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Modules:", ShipModules.Count);
            detailed = "";

            foreach (ShipModuleInfo m in ShipModules)
            {
                if (detailed.Length > 0)
                    detailed += Environment.NewLine;

                double? power = (m.Power.HasValue && m.Power.Value > 0) ? m.Power : null;

                detailed += BaseUtils.FieldBuilder.Build("", m.Slot, "<:", m.Item, "Power:; MW;0.###", power);
            }
        }
    }
}
