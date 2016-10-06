
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when buying system data via the galaxy map
    //Parameters: 
    //•	System
    //•	Cost
    public class JournalBuyExplorationData : JournalEntry
    {
        public JournalBuyExplorationData(JObject evt ) : base(evt, JournalTypeEnum.BuyExplorationData)
        {
            System = Tools.GetStringDef(evt["System"]);
            Cost = Tools.GetInt(evt["Cost"]);

        }
        public string System { get; set; }
        public int Cost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.buyexplorationdata; } }
    }
}
