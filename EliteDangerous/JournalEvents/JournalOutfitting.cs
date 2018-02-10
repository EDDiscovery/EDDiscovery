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
    //o   id
    //o   Name
    //o   Name_Localised
    //o   BuyPrice

    [JournalEntryType(JournalTypeEnum.Outfitting)]
    public class JournalOutfitting : JournalEntry, IAdditionalFiles
    {
        public JournalOutfitting(JObject evt) : base(evt, JournalTypeEnum.Outfitting)
        {
            Rescan(evt);
        }

        public void Rescan(JObject evt)
        {
            StationName = evt["StationName"].Str();
            StarSystem = evt["StarSystem"].Str();
            MarketID = evt["MarketID"].LongNull();

            ModuleItems = evt["Items"]?.ToObject<OutfittingModuleItem[]>();

            if ( ModuleItems != null )
            {
                foreach (OutfittingModuleItem i in ModuleItems)
                {
                    i.Name = JournalFieldNaming.GetBetterItemNameEvents(i.Name);
                }
            }
        }

        public bool ReadAdditionalFiles(string directory, ref JObject jo)
        {
            JObject jnew = ReadAdditionalFile(System.IO.Path.Combine(directory, "Outfitting.json"));
            if (jnew != null)        // new json, rescan
            {
                jo = jnew;      // replace current
                Rescan(jo);
            }
            return jnew != null;
        }

        public string StationName { get; set; }
        public string StarSystem { get; set; }
        public long? MarketID { get; set; }

        public OutfittingModuleItem[] ModuleItems { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            detailed = "";

            if (ModuleItems != null)
            {
                info = ModuleItems.Length.ToString() + " items available";
                int itemno = 0;
                foreach (OutfittingModuleItem m in ModuleItems)
                {
                    detailed = detailed.AppendPrePad(m.Name + ":" + m.BuyPrice.ToString("N0"), (itemno % 3 < 2) ? ", " : System.Environment.NewLine);
                    itemno++;
                }
            }
                
        }
    }


    public class OutfittingModuleItem
    {
        public long id;
        public string Name;
        public long BuyPrice;
    }

}
