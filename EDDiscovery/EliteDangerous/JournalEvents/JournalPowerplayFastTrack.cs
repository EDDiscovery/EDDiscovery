
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when paying to fast-track allocation of commodities
    //Parameters:
    //•	Power
    //•	Cost
    public class JournalPowerplayFastTrack : JournalEntry
    {
        public JournalPowerplayFastTrack(JObject evt) : base(evt, JournalTypeEnum.PowerplayFastTrack)
        {
            Power = Tools.GetStringDef(evt["Power"]);
            Cost = Tools.GetLong(evt["Cost"]);

        }
        public string Power { get; set; }
        public long Cost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.powerplayfasttrack; } }
    }
}
