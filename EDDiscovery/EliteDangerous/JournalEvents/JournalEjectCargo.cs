
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When Written:
    //Parameters:
    //•	Type: cargo type
    //•	Count: number of units
    //•	Abandoned: whether ‘abandoned’

    public class JournalEjectCargo : JournalEntry
    {
        public JournalEjectCargo(JObject evt) : base(evt, JournalTypeEnum.EjectCargo)
        {
            Type = Tools.GetStringDef(evt["Type"]);
            Count = Tools.GetInt(evt["Count"]);
            Abandoned = Tools.GetBool(evt["Abandoned"]);

        }
        public string Type { get; set; }
        public int Count { get; set; }
        public bool Abandoned { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.ejectcargo; } }
    }
}
