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
            Name = Tools.GetStringDef(evt["Name"]);

        }
        public string Name { get; set; }
    }
}
