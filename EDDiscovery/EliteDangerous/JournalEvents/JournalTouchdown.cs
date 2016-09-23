using Newtonsoft.Json.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: landing on a planet surface
    //Parameters:
    //•	Latitude
    //•	Longitude
    public class JournalTouchdown : JournalEntry
    {
        public JournalTouchdown(JObject evt ) : base(evt, JournalTypeEnum.Touchdown)
        {
            Latitude = Tools.GetDouble(evt["Latitude"]);
            Longitude = Tools.GetDouble(evt["Longitude"]);
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
