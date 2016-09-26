using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When Written: when paying fines
//    Parameters:
//•	Amount: (total amount paid , including any broker fee)
//•	BrokerPercentage(present if paid via a Broker)

    public class JournalPayFines : JournalEntry
    {
        public JournalPayFines(JObject evt) : base(evt, JournalTypeEnum.PayFines)
        {
            Amount = Tools.GetInt(evt["Amount"]);
            BrokerPercentage = Tools.GetInt(evt["BrokerPercentage"]);
        }
        public int Amount { get; set; }
        public int BrokerPercentage { get; set; }
    }
}
