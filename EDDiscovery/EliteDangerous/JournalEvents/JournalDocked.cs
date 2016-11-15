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
            StarSystem = Tools.GetStringDef(evt["StarSystem"]);
            CockpitBreach = Tools.GetBool(evt["CockpitBreach"]);

            Faction = Tools.GetMultiStringDef(evt, new string[] { "StationFaction", "Faction" });
            FactionState = Tools.GetStringDef(evt["FactionState"]);

            Allegiance = Tools.GetMultiStringDef(evt, new string[] { "StationAllegiance", "Allegiance" });
            Economy = Tools.GetMultiStringDef(evt, new string[] { "StationEconomy", "Economy" });
            Economy_Localised = Tools.GetMultiStringDef(evt, new string[] { "StationEconomy_Localised", "Economy_Localised" });
            Government = Tools.GetMultiStringDef(evt, new string[] { "StationGovernment", "Government" });
            Government_Localised = Tools.GetMultiStringDef(evt, new string[] { "StationGovernment_Localised", "Government_Localised" });

            //Security = Tools.GetMultiStringDef(evt["Security"]);
            //Security_Localised = Tools.GetMultiStringDef(evt["Security_Localised"]);
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
