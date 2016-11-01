using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when joining up with a power
    //Parameters:
    //•	Power
    public class JournalPowerplayJoin : JournalEntry
    {
        public JournalPowerplayJoin(JObject evt) : base(evt, JournalTypeEnum.PowerplayJoin)
        {
            Power = JSONHelper.GetStringDef(evt["Power"]);

        }
        public string Power { get; set; }
        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.powerplayjoin; } }
    }
}
