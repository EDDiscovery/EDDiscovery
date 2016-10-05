
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: liftoff from a landing pad in a station, outpost or settlement
    //Parameters:
    //•	StationName: name of station

    //•	Security
    public class JournalMaterialDiscovered : JournalEntry
    {
        public JournalMaterialDiscovered(JObject evt ) : base(evt, JournalTypeEnum.MaterialDiscovered)
        {
            Category = Tools.GetStringDef(evt["Category"]);
            Name = Tools.GetStringDef(evt["Name"]);
            DiscoveryNumber = Tools.GetInt(evt["DiscoveryNumber"]);
        }
        public string Category { get; set; }
        public string Name { get; set; }
        public int DiscoveryNumber { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.materialdiscovered; } }

    }
}
