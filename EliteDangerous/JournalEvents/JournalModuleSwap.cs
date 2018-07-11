/*
 * Copyright © 2016-2018 EDDiscovery development team
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
    [JournalEntryType(JournalTypeEnum.ModuleSwap)]
    public class JournalModuleSwap : JournalEntry, IShipInformation
    {
        public JournalModuleSwap(JObject evt ) : base(evt, JournalTypeEnum.ModuleSwap)
        {
            FromSlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["FromSlot"].Str());
            FromSlot = JournalFieldNaming.GetBetterSlotName(FromSlotFD);

            ToSlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["ToSlot"].Str());
            ToSlot = JournalFieldNaming.GetBetterSlotName(ToSlotFD);

            FromItemFD = JournalFieldNaming.NormaliseFDItemName(evt["FromItem"].Str());
            FromItem = JournalFieldNaming.GetBetterItemName(FromItemFD);
            FromItemLocalised = JournalFieldNaming.CheckLocalisation(evt["FromItem_Localised"].Str(),FromItem);

            ToItemFD = JournalFieldNaming.NormaliseFDItemName(evt["ToItem"].Str());
            ToItem = JournalFieldNaming.GetBetterItemName(ToItemFD);
            if (ToItem.Equals("Null"))      // Frontier bug.. something Null is here.. remove
                ToItem = ToItemFD = "";
            ToItemLocalised = JournalFieldNaming.CheckLocalisation(evt["ToItem_Localised"].Str(),ToItem);        // if ToItem is null or not there, this won't be

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
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

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleSwap(this);
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("Slot:".Txb(this), FromSlot , "< to ".Txb(this), ToSlot , "Item:".Txb(this), FromItemLocalised);
            if (ToItem.Length > 0 )                         
                info += ", Swapped with ".Txb(this) + ToItemLocalised;
            detailed = "";
        }
    }
}
