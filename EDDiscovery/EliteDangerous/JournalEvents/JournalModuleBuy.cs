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
    //When Written: when buying a module in outfitting
    //Parameters:
    //•	Slot: the outfitting slot
    //•	BuyItem: the module being purchased
    //•	BuyPrice: price paid
    //•	Ship: the players ship
    //If replacing an existing module:
    //•	SellItem: item being sold
    //•	SellPrice: sale price
    [JournalEntryType(JournalTypeEnum.ModuleBuy)]
    public class JournalModuleBuy : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleBuy(JObject evt ) : base(evt, JournalTypeEnum.ModuleBuy)
        {
            Slot = JournalFieldNaming.GetBetterSlotName(evt["Slot"].Str());
            BuyItem = JournalFieldNaming.GetBetterItemNameEvents(evt["BuyItem"].Str());
            BuyItemLocalised = evt["BuyItem_Localised"].Str();
            BuyPrice = evt["BuyPrice"].Long();
            Ship = JournalFieldNaming.GetBetterShipName(evt["Ship"].Str());
            ShipId = evt["ShipID"].Int();
            SellItem = JournalFieldNaming.GetBetterItemNameEvents(evt["SellItem"].Str());
            SellItemLocalised = evt["SellItem_Localised"].Str();
            SellPrice = evt["SellPrice"].LongNull();

            StoredItem = JournalFieldNaming.GetBetterItemNameEvents(evt["StoredItem"].Str());
            StoredItemLocalised = evt["StoredItem_Localised"].Str();
        }

        public string Slot { get; set; }

        public string Ship { get; set; }
        public int ShipId { get; set; }

        public string BuyItem { get; set; }
        public string BuyItemLocalised { get; set; }
        public long BuyPrice { get; set; }

        public string SellItem { get; set; }                    // if sold previous one
        public string SellItemLocalised { get; set; }
        public long? SellPrice { get; set; }

        public string StoredItem { get; set; }                  // if stored previous one
        public string StoredItemLocalised { get; set; }         // if stored previous one

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulebuy; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (BuyItemLocalised.Length > 0) ? BuyItemLocalised : BuyItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, -BuyPrice + ( SellPrice??0) );
        }

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleBuy(this);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("", BuyItemLocalised.Alt(BuyItem), "<into ", Slot, "Cost:; credits", BuyPrice);
            if (SellItem.Length > 0)
                info += ", " + Tools.FieldBuilder("Sold:", SellItemLocalised.Alt(SellItem), "Price:; credits", SellPrice);
            if (StoredItem.Length > 0)
                info += ", " + Tools.FieldBuilder("Stored:", StoredItemLocalised.Alt(StoredItem));

            detailed = "";
        }
    }
}
