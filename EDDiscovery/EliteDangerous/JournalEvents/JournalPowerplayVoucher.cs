
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when receiving payment for powerplay combat
    //Parameters:
    //•	Power
    //•	Systems:[name,name]
    public class JournalPowerplayVoucher : JournalEntry
    {
        public JournalPowerplayVoucher(JObject evt) : base(evt, JournalTypeEnum.PowerplayVoucher)
        {
            Power = JSONHelper.GetStringDef(evt["Power"]);

            Systems = evt.Value<JArray>("Systems").Values<string>().ToArray();
        }
        public string Power { get; set; }
        public string[] Systems { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.powerplayvoucher; } }
    }
}
