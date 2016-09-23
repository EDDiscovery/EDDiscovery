using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player has (attempted to) interdict another player or npc
    //Parameters: 
    //•	Success : true or false
    //•	Interdicted: victim pilot name
    //•	IsPlayer: whether player or npc
    //•	CombatRank: if a player
    //•	Faction: if an npc
    //•	Power: if npc working for power
    public class JournalInterdiction : JournalEntry
    {
        public JournalInterdiction(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.Interdiction, reader)
        {
            Success = Tools.GetBool("Success");
            Interdictor = Tools.GetStringDef("Interdictor");
            IsPlayer = Tools.GetBool("IsPlayer");
            CombatRank = CombatRank.Harmless;
            if (!Tools.IsNullOrEmptyT(evt["CombatRank"]))
                CombatRank = (CombatRank)(evt.Value<int?>("CombatRank"));
            Faction = Tools.GetStringDef("Faction");
            Power = Tools.GetStringDef("Power");
        }
        public bool Success { get; set; }
        public string Interdictor { get; set; }
        public bool IsPlayer { get; set; }
        public CombatRank CombatRank { get; set; }
        public string Faction { get; set; }
        public string Power { get; set; }

    }
}
