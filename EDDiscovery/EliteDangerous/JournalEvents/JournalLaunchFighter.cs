using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when launching a fighter
    //Parameters:
    //•	Loadout
    //•	PlayerControlled: whether player is controlling the fighter from launch
    public class JournalLaunchFighter : JournalEntry
    {
        public JournalLaunchFighter(JObject evt) : base(evt, JournalTypeEnum.LaunchFighter)
        {
            Loadout = JSONHelper.GetStringDef(evt["Loadout"]);
            PlayerControlled = JSONHelper.GetBool(evt["PlayerControlled"]);
        }
        public string Loadout { get; set; }
        public bool PlayerControlled { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.fighter; } }

    }
}
