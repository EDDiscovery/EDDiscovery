using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when enough material has been collected from a solar jet code (at a white dwarf or neutron star) for a jump boost
    //Parameters:
    //•	BoostValue
    public class JournalJetConeBoost : JournalEntry
    {
        public JournalJetConeBoost(JObject evt ) : base(evt, JournalTypeEnum.JetConeBoost)
        {
            BoostValue = JSONHelper.GetDouble(evt["BoostValue"]);

        }
        public double BoostValue { get; set; }

    }
}
