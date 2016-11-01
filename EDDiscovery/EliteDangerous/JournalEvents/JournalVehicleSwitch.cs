using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when switching control between the main ship and a fighter
    //Parameters:
    //•	To: ( Mothership/Fighter)
    public class JournalVehicleSwitch : JournalEntry
    {
        public JournalVehicleSwitch(JObject evt ) : base(evt, JournalTypeEnum.VehicleSwitch)
        {
            To = JSONHelper.GetStringDef(evt["To"]);
        }
        public string To { get; set; }

        public static System.Drawing.Bitmap IconSelect(string desc)
        {
            if (desc.Contains("Mothership"))
                return EDDiscovery.Properties.Resources.mothership;
            else
                return EDDiscovery.Properties.Resources.fighter;
        }
    }
}
