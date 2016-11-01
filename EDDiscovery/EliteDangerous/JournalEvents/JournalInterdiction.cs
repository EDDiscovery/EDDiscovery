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
        public JournalInterdiction(JObject evt ) : base(evt, JournalTypeEnum.Interdiction)
        {
            Success = JSONHelper.GetBool(evt["Success"]);
            Interdictor = JSONHelper.GetStringDef(evt["Interdictor"]);
            IsPlayer = JSONHelper.GetBool(evt["IsPlayer"]);
            CombatRank = CombatRank.Harmless;
            if (!JSONHelper.IsNullOrEmptyT(evt["CombatRank"]))
                CombatRank = (CombatRank)(JSONHelper.GetIntNull(evt["CombatRank"]));
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Power = JSONHelper.GetStringDef(evt["Power"]);
        }
        public bool Success { get; set; }
        public string Interdictor { get; set; }
        public bool IsPlayer { get; set; }
        public CombatRank CombatRank { get; set; }
        public string Faction { get; set; }
        public string Power { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.interdicted; } }
    }
}
