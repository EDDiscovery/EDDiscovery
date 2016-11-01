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

        public JournalDied(JObject evt ) : base(evt, JournalTypeEnum.Died)
        {
            string killerName = JSONHelper.GetStringDef(evt["KillerName"]);
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
                            Ship = JSONHelper.GetStringDef(evt["KillerShip"]),
                            Rank = JSONHelper.GetStringDef(evt["KillerRank"])
                        }
                };
            }

        }
        public Killer[] Killers { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.Coffinicon; } }

    }
}
