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
            if (!Tools.IsNullOrEmptyT(evt["Systems"]))
                Systems = evt.Value<JArray>("Systems").Values<string>().ToArray();

            if (!Tools.IsNullOrEmptyT(evt["Discovered"]))
                Discovered = evt.Value<JArray>("Discovered").Values<string>().ToArray();

            BaseValue = Tools.GetInt(evt["BaseValue"]);
            Bonus = Tools.GetInt(evt["Bonus"]);
        }
        public string[] Systems { get; set; }
        public string[] Discovered { get; set; }
        public int BaseValue { get; set; }
        public int Bonus { get; set; }
    }
}
