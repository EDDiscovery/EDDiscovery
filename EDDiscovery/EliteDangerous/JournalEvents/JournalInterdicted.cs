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
        public JournalInterdicted(JObject evt ) : base(evt, JournalTypeEnum.Interdicted)
        {
            Submitted = JSONHelper.GetBool(evt["Submitted"]);
            Interdictor = JSONHelper.GetStringDef(evt["Interdictor"]);
            IsPlayer = JSONHelper.GetBool(evt["IsPlayer"]);
            CombatRank = CombatRank.Harmless;
            if (!JSONHelper.IsNullOrEmptyT(evt["CombatRank"]))
                CombatRank = (CombatRank)(JSONHelper.GetIntNull(evt["CombatRank"]));
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Power = JSONHelper.GetStringDef(evt["Power"]);
        }
        public bool Submitted { get; set; }
        public string Interdictor { get; set; }
        public bool IsPlayer { get; set; }
        public CombatRank CombatRank { get; set; }
        public string Faction { get; set; }
        public string Power { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.interdicted; } }
    }
}
