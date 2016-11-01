using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when the station denies a docking request
    //Parameters:
    //•	StationName: name of station
    //•	Reason: reason for denial
    //
    //Reasons include: NoSpace, TooLarge, Hostile, Offences, Distance, ActiveFighter, NoReason

    public class JournalDockingDenied : JournalEntry
    {
        public JournalDockingDenied(JObject evt ) : base(evt, JournalTypeEnum.DockingDenied)
        {
            StationName = JSONHelper.GetStringDef(evt["StationName"]);
            Reason = JSONHelper.GetStringDef(evt["Reason"]);
        }
        public string StationName { get; set; }
        public string Reason { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.dockingdenied; } }

    }
}
