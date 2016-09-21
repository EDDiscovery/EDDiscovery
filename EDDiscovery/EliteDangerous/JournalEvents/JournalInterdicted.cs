using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player was interdicted by player or npc
    //Parameters: 
    //•	Submitted: true or false
    //•	Interdictor: interdicting pilot name
    //•	IsPlayer: whether player or npc
    //•	CombatRank: if player
    //•	Faction: if npc
    //•	Power: if npc working for a power
    public class JournalInterdicted : JournalEntry
    {
        public JournalInterdicted(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.Interdicted, reader)
        {
            Submitted = evt.Value<bool>("Submitted");
            Interdictor = evt.Value<string>("Interdictor");
            IsPlayer = evt.Value<bool>("IsPlayer");
            CombatRank = (CombatRank)(evt.Value<int?>("CombatRank") ?? 0);
            Faction = evt.Value<string>("Faction");
            Power = evt.Value<string>("Power");
        }
        public bool Submitted { get; set; }
        public string Interdictor { get; set; }
        public bool IsPlayer { get; set; }
        public CombatRank CombatRank { get; set; }
        public string Faction { get; set; }
        public string Power { get; set; }

    }
}
