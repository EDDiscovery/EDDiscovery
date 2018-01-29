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
    //    When written: when opening outfitting
    //Parameters:
    //•	MarketID
    //• StationName
    //• StarSystem
    //•	Items: Array of records
    //o   StorageSlot
    //o   Name
    //o   Name_Localised
    //o   StarSystem
    //o   MarketID
    //o   TransferCost
    //o   TransferTime
    //o   EngineerModifications(only present if modified)

    [JournalEntryType(JournalTypeEnum.StoredModules)]
    public class JournalStoredModules : JournalEntry
    {
        public JournalStoredModules(JObject evt) : base(evt, JournalTypeEnum.StoredModules)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();

            ModuleItems = evt["Items"]?.ToObject<StoredModuleItem[]>();

            if ( ModuleItems != null )
            {
                foreach (StoredModuleItem i in ModuleItems)
                {
                    i.Name = JournalFieldNaming.GetBetterItemNameEvents(i.Name);
                }
            }
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }

        public StoredModuleItem[] ModuleItems { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";

            if ( ModuleItems != null )
                foreach (StoredModuleItem m in ModuleItems)
                {
                    if (info.Length>0)
                        info += ", ";
                    info += m.Name;
                }
                
            detailed = "";
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
    }

}
