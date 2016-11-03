using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: when the ‘reboot repair’ function is used
//    Parameters:
//•	Modules: JSON array of names of modules repaired


    public class JournalRebootRepair : JournalEntry
    {
        public JournalRebootRepair(JObject evt) : base(evt, JournalTypeEnum.RebootRepair)
        {
            if (!JSONHelper.IsNullOrEmptyT(evt["Modules"]))
                Modules = evt.Value<JArray>("Modules").Values<string>().ToArray();
        }

        public string[] Modules { get; set; }
        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.rebootrepair; } }
    }
}


