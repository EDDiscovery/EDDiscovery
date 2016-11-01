using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when this player has killed another player
    //Parameters: 
    //•	Victim: name of victim
    //•	CombatRank: victim’s rank in range 0..8
    public class JournalPVPKill : JournalEntry
    {
        public JournalPVPKill(JObject evt) : base(evt, JournalTypeEnum.PVPKill)
        {
            Victim = JSONHelper.GetStringDef(evt["Victim"]);

            CombatRank = (CombatRank)JSONHelper.GetInt(evt["CombatRank"]);

        }
        public string Victim { get; set; }
        public CombatRank CombatRank { get; set; }

    }
}
