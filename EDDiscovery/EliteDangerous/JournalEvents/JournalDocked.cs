using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: when landing at landing pad in a space station, outpost, or surface settlement
    //Parameters:
    //•	StationName: name of station
    //•	StationType: type of station
    //•	StarSystem: name of system
    //•	CockpitBreach:true (only if landing with breached cockpit)
    //•	Faction: station’s controlling faction
    //•	FactionState
    //•	Allegiance
    //•	Economy
    //•	Government
    //•	Security
    public class JournalDocked : JournalEntry
    {


        public JournalDocked(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.Docked, reader)
        {
            StationName = Tools.GetStringDef("StationName");
            StationType = Tools.GetStringDef("StationType");
            CockpitBreach = Tools.GetBool("CockpitBreach");
            Faction = Tools.GetStringDef("Faction");
            FactionState = Tools.GetStringDef("FactionState");
            Allegiance = Tools.GetStringDef("Allegiance");
            Economy = Tools.GetStringDef("Economy");
            Economy_Localised = Tools.GetStringDef("Economy_Localised");
            Government = Tools.GetStringDef("Government");
            Government_Localised = Tools.GetStringDef("Government_Localised");
            Security = Tools.GetStringDef("Security");
            Security_Localised = Tools.GetStringDef("Security_Localised");
        }

        public string StationName { get; set; }
        public string StationType { get; set; }
        public bool CockpitBreach { get; set; }
        public string Faction { get; set; }
        public string FactionState { get; set; }
        public string Allegiance { get; set; }
        public string Economy { get; set; }
        public string Economy_Localised { get; set; }
        public string Government { get; set; }
        public string Government_Localised { get; set; }
        public string Security { get; set; }
        public string Security_Localised { get; set; }
    }
}
