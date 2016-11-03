using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when the player restarts after death
    //Parameters:
    //•	Option: the option selected on the insurance rebuy screen
    //•	Cost: the price paid
    //•	Bankrupt: whether the commander declared bankruptcy
    public class JournalResurrect : JournalEntry
    {
        public JournalResurrect(JObject evt ) : base(evt, JournalTypeEnum.Resurrect)
        {
            Option = JSONHelper.GetStringDef(evt["Option"]);
            Cost = JSONHelper.GetLong(evt["Cost"]);
            Bankrupt = JSONHelper.GetBool(evt["Bankrupt"]);

        }
        public string Option { get; set; }
        public long Cost { get; set; }
        public bool Bankrupt { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.ressurect; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Option, -Cost);
        }

    }
}
