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
using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EliteDangerousCore.CompanionAPI
{
    public class CLastStarport
    {
        public long id { get; private set; }
        public string name { get; private set; }
        public string faction { get; private set; }
        public JArray jcommodities { get; private set; }
        public List<CCommodities> commodities;

        public CLastStarport(JObject jo)
        {
            FromJson(jo);
        }

        public bool FromJson(JObject jo)
        {
            try
            {
                id = jo["id"].Long();
                name = jo["name"].Str();
                faction = jo["faction"].Str();

                jcommodities = (JArray)jo["commodities"];

                if (jcommodities != null)
                {
                    commodities = new List<CCommodities>();
                    foreach (JObject commodity in jcommodities)
                    {
                        CCommodities com = new CCommodities(commodity);
                        commodities.Add(com);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
