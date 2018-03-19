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
    //When Written: when moving a module to a different slot on the ship
    //Parameters:
    //•	FromSlot
    //•	ToSlot
    //•	FromItem
    //•	ToItem
    //•	Ship
    [JournalEntryType(JournalTypeEnum.ModuleSwap)]
    public class JournalModuleSwap : JournalEntry, IShipInformation
    {
        public JournalModuleSwap(JObject evt ) : base(evt, JournalTypeEnum.ModuleSwap)
        {
            FromSlot = JournalFieldNaming.GetBetterSlotName(evt["FromSlot"].Str());
            FromSlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["FromSlot"].Str());

            ToSlot = JournalFieldNaming.GetBetterSlotName(evt["ToSlot"].Str());
            ToSlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["ToSlot"].Str());

            FromItem = JournalFieldNaming.GetBetterItemNameEvents(evt["FromItem"].Str());
            FromItemFD = JournalFieldNaming.NormaliseFDItemName(evt["FromItem"].Str());
            FromItemLocalised = evt["FromItem_Localised"].Str().Alt(FromItem);

            ToItem = JournalFieldNaming.GetBetterItemNameEvents(evt["ToItem"].Str());
            ToItemFD = JournalFieldNaming.NormaliseFDItemName(evt["ToItem"].Str());
            if (ToItem.Equals("Null"))      // Frontier bug.. something Null is here.. remove
                ToItem = ToItemFD = "";
            ToItemLocalised = evt["ToItem_Localised"].Str().Alt(ToItem);        // if ToItem is null or not there, this won't be

            ShipFD = evt["Ship"].Str();
            Ship = JournalFieldNaming.GetBetterShipName(evt["Ship"].Str());
            ShipId = evt["ShipID"].Int();

            MarketID = evt["MarketID"].LongNull();
        }

        public string FromSlot { get; set; }
        public string FromSlotFD { get; set; }
        public string ToSlot { get; set; }
        public string ToSlotFD { get; set; }
        public string FromItem { get; set; }
        public string FromItemFD { get; set; }
        public string FromItemLocalised { get; set; }
        public string ToItem { get; set; }
        public string ToItemFD { get; set; }
        public string ToItemLocalised { get; set; }
        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }
        public long? MarketID { get; set; }

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleSwap(this);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("From ", FromSlot , "< to " , ToSlot , "Item:" , FromItemLocalised);
            if (ToItem.Length > 0 )                         
                info += ", Swapped with " + ToItemLocalised;
            detailed = "";
        }
    }
}
