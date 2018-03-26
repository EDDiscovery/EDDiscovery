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
    //    When written: at startup, when loading from main menu
    //Parameters:
    //•	Modules: array of installed items, each with:
    //o Slot: slot name
    //o Item: module name
    //o On: bool, indicates on or off
    //o Priority: power priority
    //o AmmoInClip: (if relevant)
    //o AmmoInHopper: (if relevant)
    //o EngineerBlueprint: blueprint name(if engineered)
    //o EngineerLevel: blueprint level(if engineered)
    //(For a passenger cabin, AmmoInClip holds the number of places in the cabin)

    //Example:
    //{ "timestamp":"2017-02-10T14:25:51Z", "event":"Loadout", "Modules":[ { "Slot":"HugeHardpoint1", "Item":"Hpt_MultiCannon_Gimbal_Huge", "On":true, "Priority":0, "AmmoInClip":90, "AmmoInHopper":2010 }, { "Slot":"MediumHardpoint1", "Item":"Hpt_BeamLaser_Turret_Medium", "On":true, "Priority":0 }, { "Slot":"MediumHardpoint2", "Item":"Hpt_BeamLaser_Turret_Medium", "On":true, "Priority":0, { "Slot":"TinyHardpoint1", "Item":"Hpt_PlasmaPointDefence_Turret_Tiny", "On":true, "Priority":0, "AmmoInClip":12, "AmmoInHopper":9940 }, { "Slot":"Armour", "Item":"FerDeLance_Armour_Grade1", "On":true, "Priority":1 }, { "Slot":"PaintJob", "Item":"PaintJob_FerDeLance_Tactical_White", "On":true, "Priority":1 }, { "Slot":"PowerPlant", "Item":"Int_Powerplant_Size6_Class5", "On":true, "Priority":1 }, { "Slot":"MainEngines", "Item":"Int_Engine_Size5_Class5", "On":true, "Priority":0 }, { "Slot":"FrameShiftDrive", "Item":"Int_Hyperdrive_Size4_Class5", "On":true, "Priority":0, "EngineerBlueprint":"FSD_LongRange", "EngineerLevel":5 }, { "Slot":"LifeSupport", "Item":"Int_LifeSupport_Size4_Class2", "On":true, "Priority":0 }, { "Slot":"PowerDistributor", "Item":"Int_PowerDistributor_Size6_Class5", "On":true, "Priority":0, "EngineerBlueprint":"PowerDistributor_PriorityWeapons", "EngineerLevel":1 }, { "Slot":"Radar", "Item":"Int_Sensors_Size4_Class5",

    [System.Diagnostics.DebuggerDisplay("{ShipId} {Ship} {ShipModules.Count}")]
    [JournalEntryType(JournalTypeEnum.Loadout)]
    public class JournalLoadout : JournalEntry, IShipInformation
    {
        [System.Diagnostics.DebuggerDisplay("{Slot} {Item} {LocalisedItem}")]
        public class ShipModule
        {
            public string Slot { get; private set; }        // never null       - nice name, used to track 
            public string SlotFD { get; private set; }      // never null       - FD normalised ID name (Int_CargoRack_Size6_Class1)
            public string Item { get; private set; }        // never null       - nice name, used to track
            public string ItemFD { get; private set; }        // never null     - FD normalised ID name

            public string LocalisedItem { get; set; }       // Modulex events only.  may be null

            public bool? Enabled { get; private set; }      // Loadout events, may be null
            public int? Priority { get; private set; }      // 0..4 not 1..5
            public int? Health { get; private set; }        //0-100
            public long? Value { get; private set; }
            public int? AmmoClip { get; private set; }
            public int? AmmoHopper { get; private set; }
            public double? Power { get; private set; }      // ONLY via Modules Info

            public JournalEngineerCraftBase.EngineeringData Engineering { get; private set; }       // may be NULL if module is not engineered or unknown

            public ShipModule()
            { }

            public ShipModule(string s, string sfd, string i, string ifd, 
                            bool? e, int? prior, int? ac, int? ah, double? health, long? value, 
                            double? power,
                            JournalEngineerCraftBase.EngineeringData engineering )
            {
                Slot = s; SlotFD = sfd; Item = i; ItemFD = ifd; Enabled = e; Priority = prior; AmmoClip = ac; AmmoHopper = ah;
                if (health.HasValue)
                    Health = (int)(health * 100.0);
                Value = value;
                Power = power;
                Engineering = engineering;
            }

            public ShipModule(string s, string sfd, string i, string ifd, string l )
            {
                Slot = s; SlotFD = sfd; Item = i; ItemFD = ifd; LocalisedItem = l;
            }

            public bool Same(ShipModule other)      // ignore localisased item, it does not occur everywhere..
            {
                bool basics = (Slot == other.Slot && Item == other.Item && Enabled == other.Enabled &&
                         Priority == other.Priority && AmmoClip == other.AmmoClip && AmmoHopper == other.AmmoHopper &&
                         Health == other.Health && Value == other.Value);
                return basics;  // Engineering is not needed, we have checked slots/items
            }

            public bool Same(string item )
            {
                return Item == item;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder(64);
                sb.AppendFormat("{0} = {1} {2}", Slot, Item, LocalisedItem);
                return sb.ToString();
            }

            public string PE()
            {
                string pe = "";
                if (Priority.HasValue)
                    pe = "P" + (Priority.Value+1).ToString();
                if (Enabled.HasValue)
                    pe += Enabled.Value ? "E" : "D";

                return pe;
            }
        }

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
                    JournalEngineerCraftBase.EngineeringData engineering = null;

                    JObject jeng = (JObject)jo["Engineering"];
                    if (jeng != null)
                    {
                        engineering = new JournalEngineerCraftBase.EngineeringData(jeng);
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

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
//            if (HullValue != null)                System.Diagnostics.Debug.WriteLine("!)");
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

                detailed += BaseUtils.FieldBuilder.Build("", m.Slot, "<:", m.Item , "" , m.PE() );
            }
        }



    }
}
