using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: entering supercruise from normal space
    //Parameters:
    //•	Starsystem
    public class JournalSupercruiseEntry : JournalEntry
    {
        public JournalSupercruiseEntry(JObject evt ) : base(evt, JournalTypeEnum.SupercruiseEntry)
        {
            StarSystem = JSONHelper.GetStringDef(evt["StarSystem"]);

        }
        public string StarSystem { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.supercruiseenter; } }

    }
}
