using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When Written: when paying legacy fines
    //    Parameters:
    //•	Amount: (total amount paid , including any broker fee)
    //•	BrokerPercentage(present if paid via a Broker)

    public class JournalPayLegacyFines : JournalEntry
    {
        public JournalPayLegacyFines(JObject evt) : base(evt, JournalTypeEnum.PayLegacyFines)
        {
            Amount = JSONHelper.GetLong(evt["Amount"]);
            BrokerPercentage = JSONHelper.GetDouble(evt["BrokerPercentage"]);
        }
        public long Amount { get; set; }
        public double BrokerPercentage { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.payfines; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, "Broker " + BrokerPercentage.ToString("0.0") + "%", -Amount);
        }

    }
}
