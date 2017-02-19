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

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: when requesting a module is transferred from storage at another station
//Parameters:
//•	StorageSlot
//•	StoredItem
//•	ServerId
//•	TransferCost
//•	Ship
//•	ShipId
    [JournalEntryType(JournalTypeEnum.FetchRemoteModule)]
    public class JournalFetchRemoteModule : JournalEntry, ILedgerJournalEntry
    {
        public JournalFetchRemoteModule(JObject evt) : base(evt, JournalTypeEnum.FetchRemoteModule)
        {
            StorageSlot = JSONHelper.GetStringDef(evt["StorageSlot"]);
            StoredItem = JSONHelper.GetStringDef(evt["StoredItem"]);
            StoredItemLocalised = JSONHelper.GetStringDef(evt["StoredItem_Localised"]);
            TransferCost = JSONHelper.GetLong(evt["TransferCost"]);
            Ship = JournalEntry.GetBetterShipName( JSONHelper.GetStringDef(evt["Ship"]) );
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            ServerId = JSONHelper.GetInt(evt["ServerId"]);
        }
        public string StorageSlot { get; set; }
        public string StoredItem { get; set; }
        public string StoredItemLocalised { get; set; }
        public long TransferCost { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public int ServerId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID;ServerID";
        }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, StoredItemLocalised + " on " + Ship, -TransferCost);
        }


    }
}
