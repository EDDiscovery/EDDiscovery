using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when receiving salary payment from a power
    //Parameters:
    //•	Power
    //•	Amount
    public class JournalPowerplaySalary : JournalEntry
    {
        public JournalPowerplaySalary(JObject evt) : base(evt, JournalTypeEnum.PowerplaySalary)
        {
            Power = JSONHelper.GetStringDef(evt["Power"]);
            Amount = JSONHelper.GetLong(evt["Amount"]);
        }
        public string Power { get; set; }
        public long Amount { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.powerplaysalary; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Power, Amount);
        }

    }
}
