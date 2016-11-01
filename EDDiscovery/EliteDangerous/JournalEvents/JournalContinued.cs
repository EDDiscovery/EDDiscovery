using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    // When written: if the journal file grows to 500k lines, we write this event, close the file, and start a new one
    // Parameters:
    // •	Part: next part number

    public class JournalContinued : JournalEntry
    {
        public JournalContinued(JObject evt ) : base(evt, JournalTypeEnum.Continued)
        {
            Part = JSONHelper.GetInt(evt["Part"]);
        }

        public int Part { get; set; }
    }
}
