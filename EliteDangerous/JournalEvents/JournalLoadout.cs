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
    [System.Diagnostics.DebuggerDisplay("{ShipId} {Ship} {ShipModules.Count}")]
    [JournalEntryType(JournalTypeEnum.Loadout)]
    public class JournalLoadout : JournalEntry, IShipInformation
    {
        public JournalLoadout(JObject evt) : base(evt, JournalTypeEnum.Loadout)
        {
            Ship = JournalFieldNaming.GetBetterShipName(evt["Ship"].Str());
            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            ShipId = evt["ShipID"].Int();
            ShipName = evt["ShipName"].Str();
            ShipIdent = evt["ShipIdent"].Str();
            HullValue = evt["HullValue"].LongNull();
            ModulesValue = evt["ModulesValue"].LongNull();
            Rebuy = evt["Rebuy"].LongNull();

            ShipModules = new List<ShipModule>();

            JArray jmodules = (JArray)evt["Modules"];
            if (jmodules != null)       // paranoia
            {
                foreach (JObject jo in jmodules)
                {
                    ShipModule.EngineeringData engineering = null;

                    JObject jeng = (JObject)jo["Engineering"];
                    if (jeng != null)
                    {
                        engineering = new ShipModule.EngineeringData(jeng);
                    }

                    ShipModule module = new ShipModule( JournalFieldNaming.GetBetterSlotName(jo["Slot"].Str()),
                                                        JournalFieldNaming.NormaliseFDSlotName(jo["Slot"].Str()),
                                                        JournalFieldNaming.GetBetterItemNameEvents(jo["Item"].Str()),
                                                        JournalFieldNaming.NormaliseFDItemName(jo["Item"].Str()),
                                                        jo["On"].BoolNull(),
                                                        jo["Priority"].IntNull(),
                                                        jo["AmmoInClip"].IntNull(),
                                                        jo["AmmoInHopper"].IntNull(),
                                                        jo["Health"].DoubleNull(),
                                                        jo["Value"].IntNull() , 
                                                        null,  //power not received here
                                                        engineering );
                    ShipModules.Add(module);
                }

                ShipModules = ShipModules.OrderBy(x => x.Slot).ToList();            // sort for presentation..
            }
        }

        public string Ship { get; set; }        // type, pretty name fer-de-lance
        public string ShipFD { get; set; }        // type,  fdname
        public int ShipId { get; set; }
        public string ShipName { get; set; } // : user-defined ship name
        public string ShipIdent { get; set; } //   user-defined ship ID string
        long? HullValue { get; set; }   //3.0
        long? ModulesValue { get; set; }   //3.0
        long? Rebuy { get; set; }   //3.0

        public List<ShipModule> ShipModules;

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.Loadout(ShipId, Ship, ShipFD, ShipName, ShipIdent, ShipModules, HullValue?? 0, ModulesValue ?? 0, Rebuy ?? 0);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Ship:", Ship, "Name:", ShipName, "Ident:", ShipIdent, "Modules:", ShipModules.Count , "Hull:; cr;N0", HullValue , "Modules:; cr;N0" , ModulesValue , "Rebuy:; cr;N0", Rebuy);
            detailed = "";

            foreach (ShipModule m in ShipModules)
            {
                if (detailed.Length > 0)
                    detailed += Environment.NewLine;

                detailed += BaseUtils.FieldBuilder.Build("", m.Slot, "<:", m.Item , "" , m.PE(), "Blueprint:" , m.Engineering?.FriendlyBlueprintName, "<+" , m.Engineering?.ExperimentalEffect_Localised, "< by " , m.Engineering?.Engineer );
            }
        }
    }
}

