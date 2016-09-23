using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when switching to another ship already stored at this station
    //Parameters:
    //•	ShipType: type of ship being switched to
    //•	ShipID
    //•	StoreOldShip: (if storing old ship) type of ship being stored
    //•	StoreShipID
    //•	SellOldShip: (if selling old ship) type of ship being sold
    //•	SellShipID  
    public class JournalShipyardSwap : JournalEntry
    {
        public JournalShipyardSwap(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.ShipyardSwap, reader)
        {
            ShipType = Tools.GetStringDef("ShipType");
            ShipId = Tools.GetInt("ShipID");
            StoreOldShip = Tools.GetStringDef("StoreOldShip");
            StoreShipId = evt.Value<int?>("StoreShipID");
            SellOldShip = Tools.GetStringDef("SellOldShip");
            SellShipId = evt.Value<int?>("SellShipID");
            SellPrice = evt.Value<int?>("SellPrice");
        }
        public string ShipType { get; set; }
        public int ShipId { get; set; }
        public string StoreOldShip { get; set; }
        public int? StoreShipId { get; set; }
        public string SellOldShip { get; set; }
        public int? SellShipId { get; set; }
        public int? SellPrice { get; set; }
    }
}
