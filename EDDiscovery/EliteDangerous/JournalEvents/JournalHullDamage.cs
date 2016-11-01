using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player was HullDamage by player or npc
    //Parameters: 
    public class JournalHullDamage : JournalEntry
    {
        public JournalHullDamage(JObject evt ) : base(evt, JournalTypeEnum.HullDamage)
        {
            Health = JSONHelper.GetDouble(evt["Health"]);

        }
        public double Health { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.damage; } }

    }
}
