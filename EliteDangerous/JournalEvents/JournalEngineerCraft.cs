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

    public class JournalEngineerCraftBase : JournalEntry, IMaterialCommodityJournalEntry, IShipInformation
    {
        public JournalEngineerCraftBase(JObject evt, JournalTypeEnum en) : base(evt, en)
        {
            Slot = JournalFieldNaming.GetBetterSlotName(evt["Slot"].Str());
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());

            Module = JournalFieldNaming.GetBetterItemNameEvents(evt["Module"].Str());
            ModuleFD = JournalFieldNaming.NormaliseFDItemName(evt["Module"].Str());

            Engineering = new ShipModule.EngineeringData(evt);
            
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

        public ShipModule.EngineeringData Engineering { get; set; }

        public bool? IsPreview { get; set; }            // Only for legacy convert

        public Dictionary<string, int> Ingredients { get; set; }        // not for legacy convert

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Ingredients != null)
            {
                foreach (KeyValuePair<string, int> k in Ingredients)        // may be commodities or materials
                    mc.Craft(k.Key, k.Value);
            }
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            if ( (IsPreview==null || IsPreview.Value == false) && Engineering != null )
            {
                shp.EngineerCraft(this);
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

    }
}
