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
        public JournalShipyardTransfer(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.ShipyardTransfer, reader)
        {
            ShipType = evt.Value<string>("ShipType");
            ShipId = evt.Value<int>("ShipID");
            System = evt.Value<string>("System");
            Distance = evt.Value<double>("Distance");
            TransferPrice = evt.Value<int>("TransferPrice");


        }
        public string ShipType { get; set; }
        public int ShipId { get; set; }
        public string System { get; set; }
        public double Distance { get; set; }
        public int TransferPrice { get; set; }

    }
}
