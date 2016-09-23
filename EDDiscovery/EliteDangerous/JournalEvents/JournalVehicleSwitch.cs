using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when switching control between the main ship and a fighter
    //Parameters:
    //•	To: ( Mothership/Fighter)
    public class JournalVehicleSwitch : JournalEntry
    {
        public JournalVehicleSwitch(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.VehicleSwitch, reader)
        {
            To = Tools.GetStringDef("To");
        }
        public string To { get; set; }

    }
}
