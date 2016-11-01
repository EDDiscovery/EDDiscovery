using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when delivering powerplay commodities
    //Parameters:
    //•	Power
    //•	Type
    //•	Count
    public class JournalPowerplayDeliver : JournalEntry
    {
        public JournalPowerplayDeliver(JObject evt) : base(evt, JournalTypeEnum.PowerplayDeliver)
        {
            Power = JSONHelper.GetStringDef(evt["Power"]);
            Type = JSONHelper.GetStringDef(evt["Type"]);
            Count = JSONHelper.GetInt(evt["Count"]);

        }
        public string Power { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.powerplaydeliver; } }
    }
}
