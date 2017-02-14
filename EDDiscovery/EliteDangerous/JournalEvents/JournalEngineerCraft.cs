﻿/*
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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when requesting an engineer upgrade
    //Parameters:
    //•	Engineer: name of engineer
    //•	Blueprint: name of blueprint
    //•	Level: crafting level
    //•	Ingredients: JSON object with names and quantities of materials required
    public class JournalEngineerCraft : JournalEntry
    {
        public JournalEngineerCraft(JObject evt ) : base(evt, JournalTypeEnum.EngineerCraft)
        {
            Engineer = JSONHelper.GetStringDef(evt["Engineer"]);
            Blueprint = JSONHelper.GetStringDef(evt["Blueprint"]);
            Level = JSONHelper.GetInt(evt["Level"]);

            JToken mats = (JToken)evt["Ingredients"];

            if (mats != null)
            {
                if (mats.Type == JTokenType.Object)
                {
                    Ingredients = mats?.ToObject<Dictionary<string, int>>();
                }
                else
                {
                    Ingredients = new Dictionary<string, int>();
                    foreach (JObject jo in (JArray)mats)
                    {
                        Ingredients[(string)jo["Name"]] = JSONHelper.GetInt(jo["Count"]);
                    }
                }
            }

        }

        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        public Dictionary<string,int> Ingredients { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.engineercraft; } }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Ingredients != null)
            {
                foreach (KeyValuePair<string, int> k in Ingredients)        // may be commodities or materials
                    mc.Craft(k.Key, k.Value);
            }
        }
    }
}
