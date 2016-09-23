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
        public JournalShipyardNew(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.ShipyardNew, reader)
        {
            ShipType = Tools.GetStringDef("ShipType");
            ShipId = Tools.GetInt("ShipID");

        }
        public string ShipType { get; set; }
        public int ShipId { get; set; }

    }
}
