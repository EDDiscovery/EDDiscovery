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
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when selling exploration data in Cartographics
    //Parameters:
    //•	Systems: JSON array of system names
    //•	Discovered: JSON array of discovered bodies
    //•	BaseValue: value of systems
    //•	Bonus: bonus for first discoveries
    public class JournalSellExplorationData : JournalEntry
    {
        public JournalSellExplorationData(JObject evt ) : base(evt, JournalTypeEnum.SellExplorationData)
        {
            if (!JSONHelper.IsNullOrEmptyT(evt["Systems"]))
                Systems = evt.Value<JArray>("Systems").Values<string>().ToArray();

            if (!JSONHelper.IsNullOrEmptyT(evt["Discovered"]))
                Discovered = evt.Value<JArray>("Discovered").Values<string>().ToArray();

            BaseValue = JSONHelper.GetLong(evt["BaseValue"]);
            Bonus = JSONHelper.GetLong(evt["Bonus"]);
        }
        public string[] Systems { get; set; }
        public string[] Discovered { get; set; }
        public long BaseValue { get; set; }
        public long Bonus { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.sellexplorationdata; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            if (Systems!=null)
                mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Systems.Length + " systems", Bonus + BaseValue);
        }

    }
}
