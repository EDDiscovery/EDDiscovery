using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when requesting a ship at another station be transported to this station
    //Parameters:
    //•	ShipType: type of ship
    //•	ShipID
    //•	System: where it is
    //•	Distance: how far away
    //•	TransferPrice: cost of transfer
    public class JournalShipyardTransfer : JournalEntry
    {
        public JournalShipyardTransfer(JObject evt ) : base(evt, JournalTypeEnum.ShipyardTransfer)
        {
            ShipType = JSONHelper.GetStringDef(evt["ShipType"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            System = JSONHelper.GetStringDef(evt["System"]);
            Distance = JSONHelper.GetDouble(evt["Distance"]);
            TransferPrice = JSONHelper.GetLong(evt["TransferPrice"]);
        }

        public string ShipType { get; set; }
        public int ShipId { get; set; }
        public string System { get; set; }
        public double Distance { get; set; }
        public long TransferPrice { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.shipyardtransfer; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, -TransferPrice);
        }

    }
}
