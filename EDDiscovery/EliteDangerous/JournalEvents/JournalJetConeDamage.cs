using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when passing through the jet code from a white dwarf or neutron star has caused damage to a ship module
    //Parameters:
    //•	Module: the name of the module that has taken some damage
    public class JournalJetConeDamage : JournalEntry
    {
        public JournalJetConeDamage(JObject evt ) : base(evt, JournalTypeEnum.JetConeDamage)
        {
            Module = JSONHelper.GetStringDef(evt["Module"]);

        }
        public string Module { get; set; }

    }
}
