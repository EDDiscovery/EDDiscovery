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
        public JournalShipyardBuy(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.ShipyardBuy, reader)
        {
            ShipType = evt.Value<string>("ShipType");
            ShipPrice = evt.Value<int>("ShipPrice");
            StoreOldShip = evt.Value<string>("StoreOldShip");
            StoreShipId = evt.Value<int?>("StoreShipID");
            SellOldShip = evt.Value<string>("SellOldShip");
            SellShipId = evt.Value<int?>("SellShipID");
            SellPrice = evt.Value<int?>("SellPrice");

        }
        public string ShipType { get; set; }
        public int ShipPrice { get; set; }
        public string StoreOldShip { get; set; }
        public int? StoreShipId { get; set; }
        public string SellOldShip { get; set; }
        public int? SellShipId { get; set; }
        public int? SellPrice { get; set; }

    }
}
