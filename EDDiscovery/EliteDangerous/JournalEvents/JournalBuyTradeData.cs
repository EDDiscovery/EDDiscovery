using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalBuyTradeData : JournalEntry
    {
        public JournalBuyTradeData(JObject evt ) : base(evt, JournalTypeEnum.BuyTradeData)
        {
            System = JSONHelper.GetStringDef(evt["System"]);
            Cost = JSONHelper.GetLong(evt["Cost"]);
            Count = JSONHelper.GetLong(evt["Count"]);
        }

        public string System { get; set; }
        public long Cost { get; set; }
        public long Count { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.buytradedata; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, System, -Cost);
        }

    }
}
