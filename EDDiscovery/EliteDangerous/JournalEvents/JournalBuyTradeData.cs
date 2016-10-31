using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalBuyTradeData : JournalEntry
    {
        public JournalBuyTradeData(JObject evt ) : base(evt, JournalTypeEnum.BuyTradeData)
        {
            System = Tools.GetStringDef(evt["System"]);
            Cost = Tools.GetLong(evt["Cost"]);
            Count = Tools.GetLong(evt["Count"]);
        }

        public string System { get; set; }
        public long Cost { get; set; }
        public long Count { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.buytradedata; } }
    }
}
