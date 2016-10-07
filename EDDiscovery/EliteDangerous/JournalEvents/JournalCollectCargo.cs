using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when scooping cargo from space or planet surface
    //Parameters:
    //•	Type: cargo type
    //•	Stolen: whether stolen goods
    public class JournalCollectCargo : JournalEntry
    {
        public JournalCollectCargo(JObject evt ) : base(evt, JournalTypeEnum.CollectCargo)
        {
            Type = Tools.GetStringDef(evt["Type"]);
            Stolen = Tools.GetBool(evt["Stolen"]);
        }
        public string Type { get; set; }
        public bool Stolen { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.collectcargo; } }
    }
}
