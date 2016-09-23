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
        public JournalResurrect(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.Resurrect, reader)
        {
            Option = Tools.GetStringDef("Option");
            Cost = Tools.GetInt("Cost");
            Bankrupt = Tools.GetBool("Bankrupt");

        }
        public string Option { get; set; }
        public int Cost { get; set; }
        public bool Bankrupt { get; set; }

    }
}
