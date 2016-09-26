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
            Amount = Tools.GetInt(evt["Amount"]);
            BrokerPercentage = Tools.GetInt(evt["BrokerPercentage"]);
        }
        public string Type { get; set; }
        public int Amount { get; set; }
        public int BrokerPercentage { get; set; }
    }
}


