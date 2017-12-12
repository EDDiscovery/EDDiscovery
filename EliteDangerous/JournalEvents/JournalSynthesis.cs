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
    //When written: when synthesis is used to repair or rearm
    //Parameters:
    //•	Name: synthesis blueprint
    //•	Materials: JSON object listing materials used and quantities
    [JournalEntryType(JournalTypeEnum.Synthesis)]
    public class JournalSynthesis : JournalEntry, IMaterialCommodityJournalEntry
    {
        public JournalSynthesis(JObject evt ) : base(evt, JournalTypeEnum.Synthesis)
        {
            Materials = null;

            Name = evt["Name"].Str().SplitCapsWordFull();
            JToken mats = (JToken)evt["Materials"];

            if (mats != null)
            {
                Materials = new Dictionary<string, int>();

                if (mats.Type == JTokenType.Object)
                {
                    Dictionary<string, int> temp = mats?.ToObject<Dictionary<string, int>>();

                    if (temp != null)
                    {
                        foreach (string key in temp.Keys)
                            Materials[JournalFieldNaming.FDNameTranslation(key)] = temp[key];
                    }
                }
                else
                {
                    foreach (JObject ja in (JArray)mats)
                    {
                        Materials[JournalFieldNaming.FDNameTranslation((string)ja["Name"])] = ja["Count"].Int();
                    }
                }
            }
        }
        public string Name { get; set; }
        public Dictionary<string, int> Materials { get; set; }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Materials != null)
            {
                foreach (KeyValuePair<string, int> k in Materials)        // may be commodities or materials
                    mc.Craft(k.Key, k.Value);        // same as this, uses up materials
            }
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Name;
            if (Materials != null)
                foreach (KeyValuePair<string, int> k in Materials)
                    info += ", " + JournalFieldNaming.RMat(k.Key) + ":" + k.Value.ToString();

            detailed = "";
        }
    }
}
