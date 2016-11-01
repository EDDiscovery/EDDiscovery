using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player is awarded a bounty for a kill
    //Parameters: 
    //•	Faction: the faction awarding the bounty
    //•	Reward: the reward value
    //•	VictimFaction: the victim’s faction
    //•	SharedWithOthers: whether shared with other players
    public class JournalScientificResearch : JournalEntry
    {
        public JournalScientificResearch(JObject evt) : base(evt, JournalTypeEnum.ScientificResearch)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Count = JSONHelper.GetInt(evt["Count"]);
            Category = JSONHelper.GetStringDef(evt["Category"]);
        }
        public string Name { get; set; }
        public int Count { get; set; }
        public string Category { get; set; }

    }
}
