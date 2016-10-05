using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when engaging a new member of crew
    //Parameters:
    //•	Name
    //•	Faction
    //•	Cost
    //•	CombatRank

    public class JournalCrewHire : JournalEntry
    {
        public JournalCrewHire(JObject evt) : base(evt, JournalTypeEnum.CrewHire)
        {
            Name = Tools.GetStringDef(evt["Name"]);
            Faction = Tools.GetStringDef(evt["Faction"]);
            Cost = Tools.GetInt(evt["Cost"]);
            CombatRank = (CombatRank)Tools.GetInt(evt["CombatRank"]);
        }
        public string Name { get; set; }
        public string Faction { get; set; }
        public int Cost { get; set; }
        public CombatRank CombatRank { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.crew; } }
    }
}
