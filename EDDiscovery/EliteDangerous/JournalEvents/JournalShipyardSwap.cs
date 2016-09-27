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
        public JournalShipyardSwap(JObject evt ) : base(evt, JournalTypeEnum.ShipyardSwap)
        {
            ShipType = Tools.GetStringDef(evt["ShipType"]);
            ShipId = Tools.GetInt(evt["ShipID"]);
            StoreOldShip = Tools.GetStringDef(evt["StoreOldShip"]);
            StoreShipId = evt.Value<int?>("StoreShipID");
            SellOldShip = Tools.GetStringDef(evt["SellOldShip"]);
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

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID;SellShipID;StoreShipID";
        }

    }
}
