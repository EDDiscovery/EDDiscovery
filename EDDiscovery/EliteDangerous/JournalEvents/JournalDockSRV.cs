using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when docking an SRV with the ship
    //Parameters: none
    public class JournalDockSRV : JournalEntry
    {
        public JournalDockSRV(JObject evt ) : base(evt, JournalTypeEnum.DockSRV)
        {

        }
 

    }
}
