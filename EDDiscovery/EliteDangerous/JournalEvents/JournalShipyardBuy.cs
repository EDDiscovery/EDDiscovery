using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when buying a new ship in the shipyard
    //Parameters:
    //•	ShipType: ship being purchased
    //•	ShipPrice: purchase cost
    //•	StoreOldShip: (if storing old ship) ship type being stored
    //•	StoreShipID
    //•	SellOldShip: (if selling current ship) ship type being sold
    //•	SellShipID
    //•	SellPrice: (if selling current ship) ship sale price
    //
    //Note: the new ship’s ShipID will be logged in a separate event after the purchase
    public class JournalShipyardBuy : JournalEntry
    {
        public JournalShipyardBuy(JObject evt ) : base(evt, JournalTypeEnum.ShipyardBuy)
        {
            ShipType = JSONHelper.GetStringDef(evt["ShipType"]);
            ShipPrice = JSONHelper.GetLong(evt["ShipPrice"]);
            StoreOldShip = JSONHelper.GetStringDef(evt["StoreOldShip"]);
            StoreShipId = JSONHelper.GetIntNull(evt["StoreShipID"]);
            SellOldShip = JSONHelper.GetStringDef(evt["SellOldShip"]);
            SellShipId = JSONHelper.GetIntNull(evt["SellShipID"]);
            SellPrice = JSONHelper.GetLongNull(evt["SellPrice"]);

        }
        public string ShipType { get; set; }
        public long ShipPrice { get; set; }
        public string StoreOldShip { get; set; }
        public int? StoreShipId { get; set; }
        public string SellOldShip { get; set; }
        public int? SellShipId { get; set; }
        public long? SellPrice { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";SellShipID;StoreShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.shipyardnew; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, -ShipPrice + (SellPrice ?? 0));
        }

    }
}
