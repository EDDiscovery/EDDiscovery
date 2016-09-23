using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player was killed
    //Parameters: 
    //•	KillerName
    //•	KillerShip
    //•	KillerRank
    //When written: player was killed by a wing
    //Parameters:
    //•	Killers: a JSON array of objects containing player name, ship, and rank
    public class JournalDied : JournalEntry
    {
        public class Killer
        {
            public string Name;
            public string Ship;
            public string Rank;
        }

        public JournalDied(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.Died, reader)
        {
            string killerName = Tools.GetStringDef("KillerName");
            if (string.IsNullOrEmpty(killerName))
            {
                if (evt["Killers"]!=null)
                    Killers = evt["Killers"].ToObject<Killer[]>();
            }
            else
            {
                // it was an individual
                Killers = new Killer[1]
                {
                        new Killer
                        {
                            Name = killerName,
                            Ship = Tools.GetStringDef("KillerShip"),
                            Rank = Tools.GetStringDef("KillerRank")
                        }
                };
            }

        }
        public Killer[] Killers { get; set; }
    }
}
