using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalRefuelAll : JournalEntry
    {
        public JournalRefuelAll(JObject evt ) : base(evt, JournalTypeEnum.RefuelAll)
        {
            Cost = JSONHelper.GetLong(evt["Cost"]);
            Amount = JSONHelper.GetInt(evt["Amount"]);
        }
        public long Cost { get; set; }
        public int Amount { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.refuelall; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Amount " + Amount.ToString() + "t", -Cost);
        }

    }
}
