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
            Submitted = Tools.GetBool(evt["Submitted"]);
            Interdictor = Tools.GetStringDef(evt["Interdictor"]);
            IsPlayer = Tools.GetBool(evt["IsPlayer"]);
            CombatRank = CombatRank.Harmless;
            if (!Tools.IsNullOrEmptyT(evt["CombatRank"]))
                CombatRank = (CombatRank)(evt.Value<int?>("CombatRank"));
            Faction = Tools.GetStringDef(evt["Faction"]);
            Power = Tools.GetStringDef(evt["Power"]);
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
