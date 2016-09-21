using Newtonsoft.Json.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: landing on a planet surface
    //Parameters:
    //•	Latitude
    //•	Longitude
    public class JournalTouchdown : JournalEntry
    {
        public JournalTouchdown(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.Touchdown, reader)
        {
            Latitude = evt.Value<double>("Latitude");
            Longitude = evt.Value<double>("Longitude");
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
