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

            Faction = JSONHelper.GetMultiStringDef(evt, new string[] { "StationFaction", "Faction" });
            FactionState = JSONHelper.GetStringDef(evt["FactionState"]);

            Allegiance = JSONHelper.GetMultiStringDef(evt, new string[] { "StationAllegiance", "Allegiance" });
            Economy = JSONHelper.GetMultiStringDef(evt, new string[] { "StationEconomy", "Economy" });
            Economy_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "StationEconomy_Localised", "Economy_Localised" });
            Government = JSONHelper.GetMultiStringDef(evt, new string[] { "StationGovernment", "Government" });
            Government_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "StationGovernment_Localised", "Government_Localised" });

            //Security = JSONHelper.GetMultiStringDef(evt["Security"]);
            //Security_Localised = JSONHelper.GetMultiStringDef(evt["Security_Localised"]);
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
        //public string Security { get; set; }
        //public string Security_Localised { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.Stationenter; } }

    }
}
