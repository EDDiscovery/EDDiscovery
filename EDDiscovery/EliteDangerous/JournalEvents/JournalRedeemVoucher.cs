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
            Type = JSONHelper.GetStringDef(evt["Type"]);
            Amount = JSONHelper.GetLong(evt["Amount"]);
            BrokerPercentage = JSONHelper.GetDouble(evt["BrokerPercentage"]);
        }
        public string Type { get; set; }
        public long Amount { get; set; }
        public double BrokerPercentage { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.bounty; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Type + " Broker " + BrokerPercentage.ToString("0.0") + "%", Amount);
        }

    }
}


