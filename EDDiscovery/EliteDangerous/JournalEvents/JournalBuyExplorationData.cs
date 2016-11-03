
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when buying system data via the galaxy map
    //Parameters:
    //•	System
    //•	Cost
    public class JournalBuyExplorationData : JournalEntry
    {
        public JournalBuyExplorationData(JObject evt ) : base(evt, JournalTypeEnum.BuyExplorationData)
        {
            System = JSONHelper.GetStringDef(evt["System"]);
            Cost = JSONHelper.GetLong(evt["Cost"]);

        }
        public string System { get; set; }
        public long Cost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.buyexplorationdata; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, System, -Cost);
        }

    }
}
