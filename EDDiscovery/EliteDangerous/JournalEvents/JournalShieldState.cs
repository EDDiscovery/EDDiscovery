
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalShieldState : JournalEntry
    {
        public JournalShieldState(JObject evt ) : base(evt, JournalTypeEnum.ShieldState)
        {
            ShieldsUp = JSONHelper.GetBool(evt["ShieldsUp"]);

        }
        public bool ShieldsUp { get; set; }

        public static System.Drawing.Bitmap IconSelect(string desc)
        {
            if (desc.Contains("Down"))
                return EDDiscovery.Properties.Resources.shieldsdown;
            else
                return EDDiscovery.Properties.Resources.shieldsup;
        }

    }
}
