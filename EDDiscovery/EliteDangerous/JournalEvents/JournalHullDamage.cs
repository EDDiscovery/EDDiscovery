using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player was HullDamage by player or npc
    //Parameters: 
    public class JournalHullDamage : JournalEntry
    {
        public JournalHullDamage(JObject evt ) : base(evt, JournalTypeEnum.HullDamage)
        {
            Health = Tools.GetDouble(evt["Health"]);

        }
        public double Health { get; set; }

    }
}
