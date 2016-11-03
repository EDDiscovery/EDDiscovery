
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalBuyDrones : JournalEntry
    {
        public JournalBuyDrones(JObject evt ) : base(evt, JournalTypeEnum.BuyDrones)
        {
            Type = JSONHelper.GetStringDef(evt["Type"]);
            Count = JSONHelper.GetInt(evt["Count"]);
            BuyPrice = JSONHelper.GetLong(evt["BuyPrice"]);
            TotalCost = JSONHelper.GetLong(evt["TotalCost"]);

        }
        public string Type { get; set; }
        public int Count { get; set; }
        public long BuyPrice { get; set; }
        public long TotalCost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.buydrones; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Type + " " + Count + " drones", -TotalCost);
        }

    }
}
