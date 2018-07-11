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
    [JournalEntryType(JournalTypeEnum.ModuleBuy)]
    public class JournalModuleBuy : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleBuy(JObject evt ) : base(evt, JournalTypeEnum.ModuleBuy)
        {
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());
            Slot = JournalFieldNaming.GetBetterSlotName(SlotFD);

            BuyItemFD = JournalFieldNaming.NormaliseFDItemName(evt["BuyItem"].Str());
            BuyItem = JournalFieldNaming.GetBetterItemName(BuyItemFD);
            BuyItemLocalised = JournalFieldNaming.CheckLocalisation(evt["BuyItem_Localised"].Str(),BuyItem);
            BuyPrice = evt["BuyPrice"].Long();

            ShipFD = JournalFieldNaming.NormaliseFDShipName(evt["Ship"].Str());
            Ship = JournalFieldNaming.GetBetterShipName(ShipFD);
            ShipId = evt["ShipID"].Int();

            SellItemFD = JournalFieldNaming.NormaliseFDItemName(evt["SellItem"].Str());
            SellItem = JournalFieldNaming.GetBetterItemName(SellItemFD);
            SellItemLocalised = JournalFieldNaming.CheckLocalisation(evt["SellItem_Localised"].Str(),SellItem);
            SellPrice = evt["SellPrice"].LongNull();

            StoredItemFD = JournalFieldNaming.NormaliseFDItemName(evt["StoredItem"].Str());
            StoredItem = JournalFieldNaming.GetBetterItemName(StoredItemFD);
            StoredItemLocalised = JournalFieldNaming.CheckLocalisation(evt["StoredItem_Localised"].Str(),StoredItem);

            MarketID = evt["MarketID"].LongNull();
        }

        public string Slot { get; set; }
        public string SlotFD { get; set; }

        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }

        public string BuyItem { get; set; }
        public string BuyItemFD { get; set; }
        public string BuyItemLocalised { get; set; }
        public long BuyPrice { get; set; }

        public string SellItem { get; set; }                    // if sold previous one
        public string SellItemFD { get; set; }                    // if sold previous one
        public string SellItemLocalised { get; set; }
        public long? SellPrice { get; set; }

        public string StoredItem { get; set; }                  // if stored previous one
        public string StoredItemFD { get; set; }                  // if stored previous one
        public string StoredItemLocalised { get; set; }         // if stored previous one

        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (BuyItemLocalised.Length > 0) ? BuyItemLocalised : BuyItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, -BuyPrice + ( SellPrice??0) );
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleBuy(this);
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("", BuyItemLocalised, "< into ".Txb(this), Slot, "Cost:; cr;N0".Txb(this), BuyPrice);
            if (SellItem.Length > 0)
                info += ", " + BaseUtils.FieldBuilder.Build("Sold:".Txb(this), SellItemLocalised, "Price:; cr;N0".Txb(this), SellPrice);
            if (StoredItem.Length > 0)
                info += ", " + BaseUtils.FieldBuilder.Build("Stored:".Txb(this), StoredItemLocalised);

            detailed = "";
        }
    }
}
