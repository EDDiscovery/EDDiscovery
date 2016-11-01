using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When Written: when selling unwanted drones back to the market
    //Parameters:
    //•	Type
    //•	Count
    //•	SellPrice
    //•	TotalSale

    public class JournalCrewFire : JournalEntry
    {
        public JournalCrewFire(JObject evt) : base(evt, JournalTypeEnum.CrewFire)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);

        }
        public string Name { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.crew; } }

    }
}
