using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when repairing the ship
    //Parameters:
    //•	Item: all, wear, hull, paint, or name of module
    //•	Cost: cost of repair
    public class JournalRepair : JournalEntry
    {
        public JournalRepair(JObject evt ) : base(evt, JournalTypeEnum.Repair)
        {
            Item = JSONHelper.GetStringDef(evt["Item"]);
            Cost = JSONHelper.GetLong(evt["Cost"]);

        }
        public string Item { get; set; }
        public long Cost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.repair; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Item, -Cost);
        }

    }
}
