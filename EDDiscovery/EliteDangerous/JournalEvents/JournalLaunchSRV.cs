using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: deploying the SRV from a ship onto planet surface
    //Parameters:
    //•	Loadout
    public class JournalLaunchSRV : JournalEntry
    {
        public JournalLaunchSRV(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.LaunchSRV, reader)
        {
            Loadout = Tools.GetStringDef("Loadout");

        }
        public string Loadout { get; set; }

    }
}
