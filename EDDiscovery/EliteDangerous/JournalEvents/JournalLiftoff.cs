using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when taking off from planet surface
    //Parameters:
    //•	Latitude
    //•	Longitude
    public class JournalLiftoff : JournalEntry
    {
        public JournalLiftoff(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.Liftoff, reader)
        {
            Latitude = evt.Value<double>("Latitude");
            Longitude = evt.Value<double>("Longitude");
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
