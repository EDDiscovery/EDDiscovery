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
    [JournalEntryType(JournalTypeEnum.Loadout)]
    public class JournalLoadout : JournalEntry, IShipInformation
    {
        public JournalLoadout(JObject evt) : base(evt, JournalTypeEnum.Loadout)
        {
            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();
            ShipName = evt["ShipName"].Str();
            ShipIdent = evt["ShipIdent"].Str();
            HullValue = evt["HullValue"].LongNull();
            HullHealth = evt["HullHealth"].DoubleNull();
            if (HullHealth != null)
                HullHealth *= 100.0;        // convert to 0-100
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

                    string slotfdname = JournalFieldNaming.NormaliseFDSlotName(jo["Slot"].Str());
                    string itemfdname = JournalFieldNaming.NormaliseFDItemName(jo["Item"].Str());

                    ShipModule module = new ShipModule( JournalFieldNaming.GetBetterSlotName(slotfdname),
                                                        slotfdname,
                                                        JournalFieldNaming.GetBetterItemName(itemfdname),
                                                        itemfdname,
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
        public long? HullValue { get; set; }   //3.0
        public double? HullHealth { get; set; }   //3.3, 1.0-0.0, multipled by 100.0
        public long? ModulesValue { get; set; }   //3.0
        public long? Rebuy { get; set; }   //3.0

        public List<ShipModule> ShipModules;

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.Loadout(ShipId, Ship, ShipFD, ShipName, ShipIdent, ShipModules, HullValue?? 0, ModulesValue ?? 0, Rebuy ?? 0);
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("Ship:".Txb(this), Ship, "Name:".Txb(this), ShipName, "Ident:".Txb(this), ShipIdent, 
                "Modules:".Tx(this), ShipModules.Count , "Hull Health;%;N1", HullHealth, "Hull:; cr;N0".Txb(this), HullValue , "Modules:; cr;N0".Txb(this), ModulesValue , "Rebuy:; cr;N0".Txb(this), Rebuy);
            detailed = "";

            foreach (ShipModule m in ShipModules)
            {
                if (detailed.Length > 0)
                    detailed += Environment.NewLine;

                detailed += BaseUtils.FieldBuilder.Build("", m.Slot, "<:", m.Item , "" , m.PE(), "Blueprint:".Txb(this), m.Engineering?.FriendlyBlueprintName, "<+" , m.Engineering?.ExperimentalEffect_Localised, "< " , m.Engineering?.Engineer );
            }
        }
    }
}

