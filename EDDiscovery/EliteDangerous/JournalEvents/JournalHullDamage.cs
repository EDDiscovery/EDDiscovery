using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player was HullDamage by player or npc
    //Parameters: 
    public class JournalHullDamage : JournalEntry
    {
        public JournalHullDamage(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.HullDamage, reader)
        {
            Health = Tools.GetDouble("Health");

        }
        public double Health { get; set; }

    }
}
