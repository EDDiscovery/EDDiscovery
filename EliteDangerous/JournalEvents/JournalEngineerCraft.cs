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
    //Parameters:
    //•	Engineer: name of engineer
    //•	Blueprint: name of blueprint
    //•	Level: crafting level
    //•	Ingredients: JSON object with names and quantities of materials required
    [JournalEntryType(JournalTypeEnum.EngineerCraft)]
    public class JournalEngineerCraft : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalEngineerCraft(JObject evt ) : base(evt, JournalTypeEnum.EngineerCraft)
        {
            Engineer = evt["Engineer"].Str();
            Blueprint = evt["Blueprint"].Str().SplitCapsWordFull();
            Level = evt["Level"].Int();

            JToken mats = (JToken)evt["Ingredients"];

            if (mats != null)
            {
                Ingredients = new Dictionary<string, int>();

                if (mats.Type == JTokenType.Object)
                {
                    Dictionary<string, int> temp = mats?.ToObject<Dictionary<string, int>>();

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

        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        public Dictionary<string, int> Ingredients { get; set; }


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
            info = BaseUtils.FieldBuilder.Build("", Engineer, "Blueprint:", Blueprint, "Level:", Level);

            detailed = "";
            if (Ingredients != null)
            {
                foreach (KeyValuePair<string, int> k in Ingredients)        // may be commodities or materials
                    detailed += BaseUtils.FieldBuilder.Build("Name:", JournalFieldNaming.RMat(k.Key), "", k.Value) + "; ";
            }
        }
    }
}
