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
//•	Inventory: array of cargo, with Name and Count for each

//Example:
//{ "timestamp":"2017-02-10T14:25:51Z", "event":"Cargo", "Inventory":[ { "Name":"syntheticmeat", "Count":2 }, { "Name":"evacuationshelter", "Count":1 }, { "Name":"progenitorcells", "Count":3 }, { "Name":"bioreducinglichen", "Count":1 }, { "Name":"neofabricinsulation", "Count":2 } ] }

    [JournalEntryType(JournalTypeEnum.Cargo)]
    public class JournalCargo : JournalEntry, IMaterialCommodityJournalEntry
    {
        public class Cargo
        {
            public string Name { get; set; }            // FDNAME
            public int Count { get; set; }
        }

        public JournalCargo(JObject evt) : base(evt, JournalTypeEnum.Cargo)
        {
            Inventory = evt["Inventory"]?.ToObject<Cargo[]>().OrderBy(x => x.Name).ToArray();
            foreach (Cargo c in Inventory)
                c.Name = JournalFieldNaming.FDNameTranslation(c.Name);
        }

        public Cargo[] Inventory { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "No Cargo";
            detailed = "";

            if (Inventory != null && Inventory.Length > 0)
            {
                int total = 0;
                foreach (Cargo c in Inventory)
                    total += c.Count;

                info = "Cargo, " + total + " items";
                detailed = "";

                foreach (Cargo c in Inventory)
                {
                    if (detailed.Length > 0)
                        detailed += Environment.NewLine;
                    detailed += BaseUtils.FieldBuilder.Build("", JournalFieldNaming.RMat(c.Name), "; items", c.Count);
                }
            }
        }

        public void MaterialList(MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            //System.Diagnostics.Debug.WriteLine("Updated at " + this.EventTimeUTC.ToString());
            mc.Clear(true);

            foreach (Cargo c in Inventory)
                mc.Set(MaterialCommodities.CommodityCategory, c.Name, c.Count, 0, conn);
        }
    }
}
