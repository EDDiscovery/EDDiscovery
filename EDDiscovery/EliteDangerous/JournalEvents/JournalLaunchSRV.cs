using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: deploying the SRV from a ship onto planet surface
    //Parameters:
    //•	Loadout
    public class JournalLaunchSRV : JournalEntry
    {
        public JournalLaunchSRV(JObject evt ) : base(evt, JournalTypeEnum.LaunchSRV)
        {
            Loadout = Tools.GetStringDef(evt["Loadout"]);

        }
        public string Loadout { get; set; }

    }
}
