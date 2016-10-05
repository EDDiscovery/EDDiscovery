using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when docking a fighter back with the mothership
    //Parameters: none
    public class JournalDockFighter : JournalEntry
    {
        public JournalDockFighter(JObject evt ) : base(evt, JournalTypeEnum.DockFighter)
        {


        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.fighter; } }

    }
}
