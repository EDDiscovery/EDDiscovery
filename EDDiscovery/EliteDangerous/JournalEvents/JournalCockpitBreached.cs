using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when cockpit canopy is breached
    //Parameters: none
    public class JournalCockpitBreached : JournalEntry
    {
        public JournalCockpitBreached(JObject evt ) : base(evt, JournalTypeEnum.CockpitBreached)
        {

        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.cockpitbreached; } }
    }
}
