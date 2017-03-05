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
    //When Written: when selling a module in outfitting
    //Parameters:
    //•	Slot
    //•	SellItem
    //•	SellPrice
    //•	Ship
    [JournalEntryType(JournalTypeEnum.ModuleSellRemote)]
    public class JournalModuleSellRemote : JournalEntry, ILedgerJournalEntry
    {
        public JournalModuleSellRemote(JObject evt) : base(evt, JournalTypeEnum.ModuleSellRemote)
        {
            Slot = JSONHelper.GetStringDef(evt["StorageSlot"]);
            SellItem = JSONHelper.GetStringDef(evt["SellItem"]);
            SellItemLocalised = JSONHelper.GetStringDef(evt["SellItem_Localised"]);
            SellPrice = JSONHelper.GetLong(evt["SellPrice"]);
            Ship = JournalEntry.GetBetterShipName(JSONHelper.GetStringDef(evt["Ship"]));
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            ServerId = JSONHelper.GetInt(evt["ServerId"]);
        }
        public string Slot { get; set; }
        public string SellItem { get; set; }
        public string SellItemLocalised { get; set; }
        public long SellPrice { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public int ServerId { get; set; }
        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID;ServerID";
        }
        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulesell; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (SellItemLocalised.Length > 0) ? SellItemLocalised : SellItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, SellItemLocalised + " on " + Ship, SellPrice);
        }

    }
}
