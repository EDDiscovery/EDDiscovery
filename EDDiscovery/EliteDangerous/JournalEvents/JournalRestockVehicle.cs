using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when purchasing an SRV or Fighter
    //Parameters:
    //•	Type: type of vehicle being purchased (SRV or fighter model)
    //•	Loadout: variant
    //•	Cost: purchase cost
    //•	Count: number of vehicles purchased
    public class JournalRestockVehicle : JournalEntry
    {
        public JournalRestockVehicle(JObject evt ) : base(evt, JournalTypeEnum.RestockVehicle)
        {
            Type = Tools.GetStringDef(evt["Type"]);
            Loadout = Tools.GetStringDef(evt["Loadout"]);
            Cost = Tools.GetLong(evt["Cost"]);
            Count = Tools.GetInt(evt["Count"]);
        }
        public string Type { get; set; }
        public string Loadout { get; set; }
        public long Cost { get; set; }
        public int Count { get; set; }

        public static System.Drawing.Bitmap IconSelect(string desc)
        {
            if (desc.Contains("SRV"))
                return EDDiscovery.Properties.Resources.srv;
            else
                return EDDiscovery.Properties.Resources.fighter;
        }
    }
}
