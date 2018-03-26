/*
 * Copyright © 2016 - 2017 EDDiscovery development team
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
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    //When Written: when requesting an engineer upgrade

    [JournalEntryType(JournalTypeEnum.EngineerCraft)]
    public class JournalEngineerCraft : JournalEngineerCraftBase
    {
        public JournalEngineerCraft(JObject evt) : base(evt, JournalTypeEnum.EngineerCraft)
        {

        }
    }

    // Base class used for craft and legacy

    public class JournalEngineerCraftBase : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalEngineerCraftBase(JObject evt, JournalTypeEnum en) : base(evt, en)
        {
            Slot = JournalFieldNaming.GetBetterSlotName(evt["Slot"].Str());
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());

            Module = JournalFieldNaming.GetBetterItemNameEvents(evt["Module"].Str());
            ModuleFD = JournalFieldNaming.NormaliseFDItemName(evt["Module"].Str());

            Engineering = new EngineeringData(evt);
            
            IsPreview = evt["IsPreview"].BoolNull();
            JToken mats = (JToken)evt["Ingredients"];

            if (mats != null)
            {
                Ingredients = new Dictionary<string, int>();

                if (mats.Type == JTokenType.Object)
                {
                    Dictionary<string, int> temp = mats?.ToObjectProtected<Dictionary<string, int>>();

                    if (temp != null)
                    {
                        foreach (string key in temp.Keys)
                            Ingredients[JournalFieldNaming.FDNameTranslation(key)] = temp[key];
                    }
                }
                else
                {
                    foreach (JObject jo in (JArray)mats)
                    {
                        Ingredients[JournalFieldNaming.FDNameTranslation((string)jo["Name"])] = jo["Count"].Int();
                    }
                }
            }

        }

        public string Slot { get; set; }
        public string SlotFD { get; set; }
        public string Module { get; set; }
        public string ModuleFD { get; set; }

        public EngineeringData Engineering { get; set; }

        public bool? IsPreview { get; set; }            // Only for legacy convert

        public Dictionary<string, int> Ingredients { get; set; }        // not for legacy convert

        public EngineeringModifiers[] Modifiers;

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Ingredients != null)
            {
                foreach (KeyValuePair<string, int> k in Ingredients)        // may be commodities or materials
                    mc.Craft(k.Key, k.Value);
            }
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("In ", Slot , "" , Module, "By ", Engineering.Engineer, "Blueprint ", Engineering.FriendlyBlueprintName, "Level ", Engineering.Level);

            detailed = "";
            if (Ingredients != null)
            {
                foreach (KeyValuePair<string, int> k in Ingredients)        // may be commodities or materials
                    detailed += BaseUtils.FieldBuilder.Build("", JournalFieldNaming.RMat(k.Key), "", k.Value) + "; ";
            }
        }

        // Engineering data - used here and for Loadout.

        public class EngineeringData
        {
            public string Engineer { get; set; }
            public string BlueprintName { get; set; }
            public string FriendlyBlueprintName { get; set; }
            public long EngineerID { get; set; }
            public long BlueprintID { get; set; }
            public int Level { get; set; }
            public double Quality { get; set; }
            public string ExperimentalEffect { get; set; }
            public EngineeringModifiers[] Modifiers { get; set; }       // may be null

            public EngineeringData(JObject evt)
            {
                Engineer = evt["Engineer"].Str();
                EngineerID = evt["EngineerID"].Long();
                BlueprintName = evt["BlueprintName"].Str();
                FriendlyBlueprintName = BlueprintName.SplitCapsWordFull();
                BlueprintID = evt["BlueprintID"].Long();
                Level = evt["Level"].Int();
                Quality = evt["Quality"].Double(0);
                // EngineerCraft has it as Apply.. Loadout has just ExperimentalEffect.  Check both
                ExperimentalEffect = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "ExperimentalEffect", "ApplyExperimentalEffect" });

                Modifiers = evt["Modifiers"]?.ToObjectProtected<EngineeringModifiers[]>();
            }

        }

        public class EngineeringModifiers
        {
            public string Label { get; set; }
            public string ValueStr { get; set; }            // 3.02 if set, means ones further on do not apply. check first
            public double Value { get; set; }               // may be 0
            public double OriginalValue { get; set; }
            public bool LessIsGood { get; set; }

        }
    }
}
