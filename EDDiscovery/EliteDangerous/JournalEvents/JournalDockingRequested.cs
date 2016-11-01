using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when the player requests docking at a station
    //Parameters:
    //•	StationName: name of station
    public class JournalDockingRequested : JournalEntry
    {
        public JournalDockingRequested(JObject evt ) : base(evt, JournalTypeEnum.DockingRequested)
        {
            StationName = JSONHelper.GetStringDef(evt["StationName"]);
        }
        public string StationName { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.dockingrequest; } }
    }
}
