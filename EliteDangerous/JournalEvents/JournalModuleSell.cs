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
    //When Written: when selling a module in outfitting
    //Parameters:
    //•	Slot
    //•	SellItem
    //•	SellPrice
    //•	Ship
    [JournalEntryType(JournalTypeEnum.ModuleSell)]
    public class JournalModuleSell : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalModuleSell(JObject evt ) : base(evt, JournalTypeEnum.ModuleSell)
        {
            Slot = JournalFieldNaming.GetBetterSlotName(evt["Slot"].Str());
            SlotFD = JournalFieldNaming.NormaliseFDSlotName(evt["Slot"].Str());

            SellItem = JournalFieldNaming.GetBetterItemNameEvents(evt["SellItem"].Str());
            SellItemFD = JournalFieldNaming.NormaliseFDItemName(evt["SellItem"].Str());
            SellItemLocalised = evt["SellItem_Localised"].Str().Alt(SellItem);

            SellPrice = evt["SellPrice"].Long();

            ShipFD = evt["Ship"].Str();
            Ship = JournalFieldNaming.GetBetterShipName(evt["Ship"].Str());
            ShipId = evt["ShipID"].Int();

            MarketID = evt["MarketID"].LongNull();
        }

        public string Slot { get; set; }
        public string SlotFD { get; set; }
        public string SellItem { get; set; }
        public string SellItemFD { get; set; }
        public string SellItemLocalised { get; set; }
        public long SellPrice { get; set; }
        public string Ship { get; set; }
        public string ShipFD { get; set; }
        public int ShipId { get; set; }
        public long? MarketID { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (SellItemLocalised.Length > 0) ? SellItemLocalised : SellItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, SellPrice);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.ModuleSell(this);
        }
        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", SellItemLocalised, "< from ", Slot, "Price:; cr;N0", SellPrice);
            detailed = "";
        }

    }
}
