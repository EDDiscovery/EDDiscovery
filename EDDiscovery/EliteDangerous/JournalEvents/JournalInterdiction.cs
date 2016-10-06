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
            Success = Tools.GetBool(evt["Success"]);
            Interdictor = Tools.GetStringDef(evt["Interdictor"]);
            IsPlayer = Tools.GetBool(evt["IsPlayer"]);
            CombatRank = CombatRank.Harmless;
            if (!Tools.IsNullOrEmptyT(evt["CombatRank"]))
                CombatRank = (CombatRank)(evt.Value<int?>("CombatRank"));
            Faction = Tools.GetStringDef(evt["Faction"]);
            Power = Tools.GetStringDef(evt["Power"]);
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
