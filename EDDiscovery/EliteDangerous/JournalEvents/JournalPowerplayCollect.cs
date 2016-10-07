using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when collecting powerplay commodities for delivery
    //Parameters:
    //•	Power: name of power
    //•	Type: type of commodity
    //•	Count: number of units
    public class JournalPowerplayCollect : JournalEntry
    {
        public JournalPowerplayCollect(JObject evt) : base(evt, JournalTypeEnum.PowerplayCollect)
        {
            Power = Tools.GetStringDef(evt["Power"]);
            Type = Tools.GetStringDef(evt["Type"]);
            Count = Tools.GetInt(evt["Count"]);

        }
        public string Power { get; set; }
        public string Type { get; set; }
        public int Count { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.powerplaycollect; } }
    }
}
