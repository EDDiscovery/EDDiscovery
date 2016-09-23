using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: this player has joined a wing
    //Parameters:
    //•	Others: JSON array of other player names already in wing
    public class JournalWingJoin : JournalEntry
    {
        public JournalWingJoin(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.WingJoin, reader)
        {
            if (!Tools.IsNullOrEmptyT(evt["Others"]))
                Others = evt.Value<JArray>("Others").Values<string>().ToArray();

        }

        public string[] Others { get; set; }

    }
}
