using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player was HeatWarning by player or npc
    //Parameters: 
    public class JournalHeatWarning : JournalEntry
    {
        public JournalHeatWarning(JObject evt ) : base(evt, JournalTypeEnum.HeatWarning)
        {
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.heatdamage; } }
    }
}
