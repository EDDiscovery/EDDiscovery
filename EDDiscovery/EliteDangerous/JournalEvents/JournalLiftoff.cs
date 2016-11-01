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
            Latitude = JSONHelper.GetDouble(evt["Latitude"]);
            Longitude = JSONHelper.GetDouble(evt["Longitude"]);
        }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.liftoff; } }

    }
}
