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
            ShipType = JSONHelper.GetStringDef(evt["ShipType"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            StoreOldShip = JSONHelper.GetStringDef(evt["StoreOldShip"]);
            StoreShipId = JSONHelper.GetIntNull(evt["StoreShipID"]);
            //SellOldShip = JSONHelper.GetStringDef(evt["SellOldShip"]);        // NO EVIDENCE OF THESE
            //SellShipId = JSONHelper.GetIntNull(evt["SellShipID"]);
            //SellPrice = JSONHelper.GetLongNull(evt["SellPrice"]);
        }

        public string ShipType { get; set; }
        public int ShipId { get; set; }
        public string StoreOldShip { get; set; }
        public int? StoreShipId { get; set; }
//        public string SellOldShip { get; set; }
//        public int? SellShipId { get; set; }
//        public long? SellPrice { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID;SellShipID;StoreShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.shipyardswap; } }

    }
}
