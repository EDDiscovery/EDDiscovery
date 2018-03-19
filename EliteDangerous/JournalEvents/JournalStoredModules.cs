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
    public class JournalStoredModules : JournalEntry
    {
        public JournalStoredModules(JObject evt) : base(evt, JournalTypeEnum.StoredModules)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();

            ModuleItems = evt["Items"]?.ToObjectProtected<StoredModuleItem[]>();

            if (ModuleItems != null)
            {
                foreach (StoredModuleItem i in ModuleItems)
                    i.Normalise();
            }
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }

        public StoredModuleItem[] ModuleItems { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Total:", ModuleItems?.Count());
            detailed = "";

            if (ModuleItems != null)
                foreach (StoredModuleItem m in ModuleItems)
                {
                    detailed = detailed.AppendPrePad(BaseUtils.FieldBuilder.Build("", m.Name, "< at ", m.StarSystem, "Transfer Cost:; cr;N0", m
                                .TransferCost, "Time:", m.TransferTimeString, "Value:; cr;N0", m.TransferCost, ";(Hot)", m.Hot), System.Environment.NewLine);
                }

        }

        public class StoredModuleItem
        {
            public int StorageSlot;
            public string Name;
            public string Name_Localised;
            public string StarSystem;
            public long MarketID;
            public long TransferCost;
            public int TransferTime;
            public string EngineerModifications;
            public double Quality;
            public int Level;
            public bool Hot;
            public bool InTransit;
            public int BuyPrice;

            public System.TimeSpan TransferTimeSpan;        // computed
            public string TransferTimeString; // computed

            public void Normalise()
            {
                Name = JournalFieldNaming.GetBetterItemNameEvents(Name);
                Name_Localised = Name_Localised.Alt(Name);
                TransferTimeSpan = new System.TimeSpan((int)(TransferTime / 60 / 60), (int)((TransferTime / 60) % 60), (int)(TransferTime % 60));
                TransferTimeString = TransferTimeSpan.ToString();
            }
        }
    }
}
