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
        public JournalLiftoff(JObject evt ) : base(evt, JournalTypeEnum.Liftoff)
        {
            Latitude = Tools.GetDouble(evt["Latitude"]);
            Longitude = Tools.GetDouble(evt["Longitude"]);
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
