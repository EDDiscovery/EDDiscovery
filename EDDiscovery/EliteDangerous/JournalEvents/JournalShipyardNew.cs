using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: after a new ship has been purchased
    //Parameters:
    //•	ShipType
    //•	ShipID
    public class JournalShipyardNew : JournalEntry
    {
        public JournalShipyardNew(JObject evt ) : base(evt, JournalTypeEnum.ShipyardNew)
        {
            ShipType = Tools.GetStringDef(evt["ShipType"]);
            ShipId = Tools.GetInt(evt["NewShipID"]);

        }
        public string ShipType { get; set; }
        public int ShipId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID;NewShipID";
        }
    }
}
