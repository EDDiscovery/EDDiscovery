using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when selling exploration data in Cartographics
    //Parameters:
    //•	Systems: JSON array of system names
    //•	Discovered: JSON array of discovered bodies
    //•	BaseValue: value of systems
    //•	Bonus: bonus for first discoveries
    public class JournalSellExplorationData : JournalEntry
    {
        public JournalSellExplorationData(JObject evt ) : base(evt, JournalTypeEnum.SellExplorationData)
        {
            if (!JSONHelper.IsNullOrEmptyT(evt["Systems"]))
                Systems = evt.Value<JArray>("Systems").Values<string>().ToArray();

            if (!JSONHelper.IsNullOrEmptyT(evt["Discovered"]))
                Discovered = evt.Value<JArray>("Discovered").Values<string>().ToArray();

            BaseValue = JSONHelper.GetLong(evt["BaseValue"]);
            Bonus = JSONHelper.GetLong(evt["Bonus"]);
        }
        public string[] Systems { get; set; }
        public string[] Discovered { get; set; }
        public long BaseValue { get; set; }
        public long Bonus { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.sellexplorationdata; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            if (Systems!=null)
                mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Systems.Length + " systems", Bonus + BaseValue);
        }

    }
}
