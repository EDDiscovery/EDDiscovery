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

namespace EliteDangerousCore.JournalEvents
{
   
    [JournalEntryType(JournalTypeEnum.StoredModules)]
    public class JournalStoredModules : JournalEntry, IShipInformation
    {
        public JournalStoredModules(JObject evt) : base(evt, JournalTypeEnum.StoredModules)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();

            ModuleItems = evt["Items"]?.ToObjectProtected<ModulesInStore.StoredModule[]>();

            if (ModuleItems != null)
            {
                foreach (ModulesInStore.StoredModule i in ModuleItems)
                    i.Normalise();
            }
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }

        public ModulesInStore.StoredModule[] ModuleItems { get; set; }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.UpdateStoredModules(this);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Total:", ModuleItems?.Count());
            detailed = "";

            if (ModuleItems != null)
            {
                foreach (ModulesInStore.StoredModule m in ModuleItems)
                {
                    detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", m.Name, "< at ", m.StarSystem, "Transfer Cost:; cr;N0", m
                                .TransferCost, "Time:", m.TransferTimeString, "Value:; cr;N0", m.TransferCost, ";(Hot)", m.Hot), System.Environment.NewLine);
                }
            }
        }
    }
}
