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
            StationName = JSONHelper.GetStringDef(evt["StationName"]);
            StationType = JSONHelper.GetStringDef(evt["StationType"]);
            StarSystem = JSONHelper.GetStringDef(evt["StarSystem"]);
            CockpitBreach = JSONHelper.GetBool(evt["CockpitBreach"]);
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            FactionState = JSONHelper.GetStringDef(evt["FactionState"]);
            Allegiance = JSONHelper.GetStringDef(evt["Allegiance"]);
            Economy = JSONHelper.GetStringDef(evt["Economy"]);
            Economy_Localised = JSONHelper.GetStringDef(evt["Economy_Localised"]);
            Government = JSONHelper.GetStringDef(evt["Government"]);
            Government_Localised = JSONHelper.GetStringDef(evt["Government_Localised"]);
            Security = JSONHelper.GetStringDef(evt["Security"]);
            Security_Localised = JSONHelper.GetStringDef(evt["Security_Localised"]);
        }

        public string StationName { get; set; }
        public string StationType { get; set; }
        public string StarSystem { get; set; }
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

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.Stationenter; } }

    }
}
