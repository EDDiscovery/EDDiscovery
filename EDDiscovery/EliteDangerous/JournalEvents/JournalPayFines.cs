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
            BrokerPercentage = Tools.GetDouble(evt["BrokerPercentage"]);
        }
        public int Amount { get; set; }
        public double BrokerPercentage { get; set; }


        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.payfines; } }
    }
}
