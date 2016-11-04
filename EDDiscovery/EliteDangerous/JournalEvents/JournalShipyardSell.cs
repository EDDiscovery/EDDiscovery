using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{

  public class JournalShipyardSell : JournalEntry
    {
        //When Written: when selling a ship stored in the shipyard
        //Parameters:
        //•	ShipType: type of ship being sold
        //•	SellShipID
        //•	ShipPrice: sale price
        //•	System: (if ship is in another system) name of system
        public JournalShipyardSell(JObject evt ) : base(evt, JournalTypeEnum.ShipyardSell)
        {
            ShipType = JSONHelper.GetStringDef(evt["ShipType"]);
            SellShipId = JSONHelper.GetInt(evt["SellShipID"]);
            ShipPrice = JSONHelper.GetLong(evt["ShipPrice"]);
            System = JSONHelper.GetStringDef(evt["System"]);
        }
        public string ShipType { get; set; }
        public int SellShipId { get; set; }
        public long ShipPrice { get; set; }
        public string System { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";SellShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.shipyardsell; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, ShipType, ShipPrice);
        }

    }
}
