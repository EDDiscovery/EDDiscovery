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


        public JournalDocked(JObject evt ) : base(evt, JournalTypeEnum.Docked)
        {
            StationName = Tools.GetStringDef(evt["StationName"]);
            StationType = Tools.GetStringDef(evt["StationType"]);
            CockpitBreach = Tools.GetBool(evt["CockpitBreach"]);
            Faction = Tools.GetStringDef(evt["Faction"]);
            FactionState = Tools.GetStringDef(evt["FactionState"]);
            Allegiance = Tools.GetStringDef(evt["Allegiance"]);
            Economy = Tools.GetStringDef(evt["Economy"]);
            Economy_Localised = Tools.GetStringDef(evt["Economy_Localised"]);
            Government = Tools.GetStringDef(evt["Government"]);
            Government_Localised = Tools.GetStringDef(evt["Government_Localised"]);
            Security = Tools.GetStringDef(evt["Security"]);
            Security_Localised = Tools.GetStringDef(evt["Security_Localised"]);
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
