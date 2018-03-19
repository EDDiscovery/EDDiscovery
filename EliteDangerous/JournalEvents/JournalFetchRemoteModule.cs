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

namespace EliteDangerousCore.JournalEvents
{
//    When written: when requesting a module is transferred from storage at another station
//Parameters:
//•	StorageSlot
//•	StoredItem
//•	ServerId
//•	TransferCost
//•	Ship
//•	ShipId
//* TransferTime: (in seconds) 
    [JournalEntryType(JournalTypeEnum.FetchRemoteModule)]
    public class JournalFetchRemoteModule : JournalEntry, ILedgerJournalEntry
    {
        public JournalFetchRemoteModule(JObject evt) : base(evt, JournalTypeEnum.FetchRemoteModule)
        {
            StorageSlot = evt["StorageSlot"].Str();          // Slot number, not a slot on our ship

            StoredItem = JournalFieldNaming.GetBetterItemNameEvents(evt["StoredItem"].Str());
            StoredItemFD = JournalFieldNaming.NormaliseFDItemName(evt["StoredItem"].Str());
            StoredItemLocalised = evt["StoredItem_Localised"].Str().Alt(StoredItem);

            TransferCost = evt["TransferCost"].Long();

            Ship = JournalFieldNaming.GetBetterShipName( evt["Ship"].Str() );
            ShipId = evt["ShipID"].Int();
            ServerId = evt["ServerId"].Int();
            nTransferTime = evt["TransferTime"].IntNull();
        }

        public string StorageSlot { get; set; }
        public string StoredItem { get; set; }
        public string StoredItemFD { get; set; }
        public string StoredItemLocalised { get; set; }
        public long TransferCost { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public int ServerId { get; set; }
        public int? nTransferTime { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, StoredItemLocalised + " on " + Ship, -TransferCost);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("", StoredItemLocalised, "Cost:", TransferCost, "into ship:", Ship, "Transfer Time:", JournalFieldNaming.GetBetterTimeinSeconds(nTransferTime));
            detailed = "";
        }
    }
}
