using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: this player has joined a wing
    //Parameters:
    //•	Others: JSON array of other player names already in wing
    public class JournalWingJoin : JournalEntry
    {
        public JournalWingJoin(JObject evt ) : base(evt, JournalTypeEnum.WingJoin)
        {
            if (!JSONHelper.IsNullOrEmptyT(evt["Others"]))
                Others = evt.Value<JArray>("Others").Values<string>().ToArray();

        }

        public string[] Others { get; set; }

    }
}
