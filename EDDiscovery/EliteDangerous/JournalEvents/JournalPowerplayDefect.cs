using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when a player defects from one power to another
    //Parameters:
    //•	FromPower
    //•	ToPower
    public class JournalPowerplayDefect : JournalEntry
    {
        public JournalPowerplayDefect(JObject evt) : base(evt, JournalTypeEnum.PowerplayDefect)
        {
            FromPower = JSONHelper.GetStringDef(evt["FromPower"]);
            ToPower = JSONHelper.GetStringDef(evt["ToPower"]);

        }

        public string FromPower { get; set; }
        public string ToPower { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.powerplaydefect; } }
    }
}
