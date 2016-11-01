using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when approaching a planetary settlement
    //Parameters:
    // •	Name

    public class JournalApproachSettlement : JournalEntry
    {
        public JournalApproachSettlement(JObject evt) : base(evt, JournalTypeEnum.ApproachSettlement)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);

        }
        public string Name { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.approachsettlement; } }
    }
}
