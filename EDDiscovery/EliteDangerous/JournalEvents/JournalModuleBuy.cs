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
 * EDDiscovery is not affiliated with Fronter Developments plc.
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
    public class JournalModuleBuy : JournalEntry
    {
        public JournalModuleBuy(JObject evt ) : base(evt, JournalTypeEnum.ModuleBuy)
        {
            Slot = JSONHelper.GetStringDef(evt["Slot"]);
            BuyItem = JSONHelper.GetStringDef(evt["BuyItem"]);
            BuyItemLocalised = JSONHelper.GetStringDef(evt["BuyItem_Localised"]);
            BuyPrice = JSONHelper.GetLong(evt["BuyPrice"]);
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            SellItem = JSONHelper.GetStringDef(evt["SellItem"]);
            SellItemLocalised = JSONHelper.GetStringDef(evt["SellItem_Localised"]);
            SellPrice = JSONHelper.GetLongNull(evt["SellPrice"]);

        }
        public string Slot { get; set; }
        public string BuyItem { get; set; }
        public string BuyItemLocalised { get; set; }
        public long BuyPrice { get; set; }
        public string Ship { get; set; }
        public int ShipId { get; set; }
        public string SellItem { get; set; }
        public string SellItemLocalised { get; set; }
        public long? SellPrice { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulebuy; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string s = (BuyItemLocalised.Length > 0) ? BuyItemLocalised : BuyItem;

            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, s + " on " + Ship, -BuyPrice + ( SellPrice??0) );
        }

    }
}
