using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when voting for a system expansion
    //Parameters:
    //•	Power
    //•	Votes
    //•	System
    public class JournalPowerplayVote : JournalEntry
    {
        public JournalPowerplayVote(JObject evt) : base(evt, JournalTypeEnum.PowerplayVote)
        {
            Power = Tools.GetStringDef(evt["Power"]);
            System = Tools.GetStringDef(evt["System"]);
            Votes = Tools.GetInt(evt["Votes"]);
        }
        public string Power { get; set; }
        public string System { get; set; }
        public int Votes { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.powerplayvote; } }
    }
}
