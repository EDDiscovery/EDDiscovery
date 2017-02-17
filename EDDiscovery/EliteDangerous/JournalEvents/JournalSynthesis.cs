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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when synthesis is used to repair or rearm
    //Parameters:
    //•	Name: synthesis blueprint
    //•	Materials: JSON object listing materials used and quantities
    public class JournalSynthesis : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalSynthesis(JObject evt ) : base(evt, JournalTypeEnum.Synthesis)
        {
            Materials = null;

            Name = JSONHelper.GetStringDef(evt["Name"]);
            JToken mats = (JToken)evt["Materials"];

            if (mats != null)
            {
                if (mats.Type == JTokenType.Object)
                {
                    Materials = mats?.ToObject<Dictionary<string, int>>();
                }
                else
                {
                    Materials = new Dictionary<string, int>();
                    foreach (JObject ja in (JArray)mats)
                    {
                        Materials[(string)ja["Name"]] = JSONHelper.GetInt(ja["Count"]);
                    }
                }
            }
        }
        public string Name { get; set; }
        public Dictionary<string, int> Materials { get; set; }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Materials != null)
            {
                foreach (KeyValuePair<string, int> k in Materials)        // may be commodities or materials
                    mc.Craft(k.Key, k.Value);        // same as this, uses up materials
            }
        }

    }
}
