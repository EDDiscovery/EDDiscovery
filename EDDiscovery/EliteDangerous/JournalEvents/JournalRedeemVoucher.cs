using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When Written: when claiming payment for combat bounties and bonds
//Parameters:
//•	Type
//•	Amount: (Net amount received, after any broker fee)
//•	BrokerPercenentage

    public class JournalRedeemVoucher : JournalEntry
    {
        public JournalRedeemVoucher(JObject evt) : base(evt, JournalTypeEnum.RedeemVoucher)
        {
            Type = Tools.GetStringDef(evt["Type"]);
            Amount = Tools.GetLong(evt["Amount"]);
            BrokerPercentage = Tools.GetDouble(evt["BrokerPercentage"]);
        }
        public string Type { get; set; }
        public long Amount { get; set; }
        public double BrokerPercentage { get; set; }
    }
}


