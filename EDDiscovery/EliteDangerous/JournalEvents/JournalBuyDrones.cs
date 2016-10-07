
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalBuyDrones : JournalEntry
    {
        public JournalBuyDrones(JObject evt ) : base(evt, JournalTypeEnum.BuyDrones)
        {
            Type = Tools.GetStringDef(evt["Type"]);
            Count = Tools.GetInt(evt["Count"]);
            BuyPrice = Tools.GetInt(evt["BuyPrice"]);
            TotalCost = Tools.GetInt(evt["TotalCost"]);

        }
        public string Type { get; set; }
        public int Count { get; set; }
        public int BuyPrice { get; set; }
        public int TotalCost { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.buydrones; } }
    }
}
