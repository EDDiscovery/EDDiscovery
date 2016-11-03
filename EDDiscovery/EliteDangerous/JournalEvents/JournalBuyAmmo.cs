
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when purchasing ammunition
    //Parameters:
    //•	Cost
    public class JournalBuyAmmo : JournalEntry
    {
        public JournalBuyAmmo(JObject evt ) : base(evt, JournalTypeEnum.BuyAmmo)
        {
            Cost = JSONHelper.GetLong(evt["Cost"]);

        }
        public long Cost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.ammunition; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "", -Cost);
        }

    }
}
