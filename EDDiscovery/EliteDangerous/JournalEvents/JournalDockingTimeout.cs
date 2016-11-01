using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when a docking request has timed out
    //Parameters:
    //•	StationName: name of station
    public class JournalDockingTimeout : JournalEntry
    {
        public JournalDockingTimeout(JObject evt ) : base(evt, JournalTypeEnum.DockingTimeout)
        {
            StationName = JSONHelper.GetStringDef(evt["StationName"]);
        }
        public string StationName { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.dockingtimeout; } }
    }
}
