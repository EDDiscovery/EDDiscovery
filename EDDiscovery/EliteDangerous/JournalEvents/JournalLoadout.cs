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

namespace EDDiscovery.EliteDangerous.JournalEvents
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

    [JournalEntryType(JournalTypeEnum.Loadout)]
    public class JournalLoadout : JournalEntry
    {
        public class ShipModule
        {
            public string Slot { get; set; }
            public string Item { get; set; }
            public bool Enabled { get; set; }
            public int Priority { get; set; }
            public int AmmoClip { get; set; }               // or 0
            public int AmmoHopper { get; set; }             // or 0
            public string Blueprint { get; set; }           // or null
            public int BlueprintLevel { get; set; }         // or 0

            public string ItemFriendlyName { get { return Item.SplitCapsWordUnderscoreTitleCase(); } }
        }

        public JournalLoadout(JObject evt) : base(evt, JournalTypeEnum.Loadout)
        {
            JArray jmodules = (JArray)evt["Modules"];
            ShipModules = new List<ShipModule>();

            foreach (JObject jo in jmodules)
            {
                ShipModule module = new ShipModule();

                module.Slot = JSONHelper.GetStringDef(jo["Slot"]);
                module.Item = JSONHelper.GetStringDef(jo["Item"]);
                module.Enabled = JSONHelper.GetBool(jo["On"]);
                module.Priority = JSONHelper.GetInt(jo["Priority"]);
                module.AmmoClip = JSONHelper.GetInt(jo["AmmoInClip"]);
                module.AmmoHopper = JSONHelper.GetInt(jo["AmmoInHopper"]);
                module.Blueprint = JSONHelper.GetStringDef(jo["EngineerBlueprint"]);
                module.BlueprintLevel = JSONHelper.GetInt(jo["EngineerLevel"]);
                ShipModules.Add(module);
            }
        }

        public List<ShipModule> ShipModules;


        //public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.location; } }

    }
}
