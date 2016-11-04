
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When Written: when selling unwanted drones back to the market
    //Parameters:
    //•	Type
    //•	Count
    //•	SellPrice
    //•	TotalSale

    public class JournalSellDrones : JournalEntry
    {
        public JournalSellDrones(JObject evt) : base(evt, JournalTypeEnum.SellDrones)
        {
            Type = JSONHelper.GetStringDef(evt["Type"]);
            Count = JSONHelper.GetInt(evt["Count"]);
            SellPrice = JSONHelper.GetLong(evt["SellPrice"]);
            TotalSale = JSONHelper.GetLong(evt["TotalSale"]);
        }
        public string Type { get; set; }
        public int Count { get; set; }
        public long SellPrice { get; set; }
        public long TotalSale { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.selldrones; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Count.ToString() + " drones", TotalSale);
        }

    }
}
