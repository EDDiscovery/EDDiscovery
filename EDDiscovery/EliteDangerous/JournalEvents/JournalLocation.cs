using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: at startup, or when being resurrected at a station
    //Parameters:
    //•	StarSystem: name of destination starsystem
    //•	StarPos: star position, as a Json array [x, y, z], in light years
    //•	Body: star’s body name
    //•	Docked: true (if docked)
    //•	StationName: station name, (if docked)
    //•	StationType: (if docked)
    //•	Faction: star system controlling faction
    //•	FactionState
    //•	Allegiance
    //•	Economy
    //•	Government
    //•	Security
    public class JournalLocation : JournalLocOrJump
    {
        public JournalLocation(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.Location, reader)
        {
            Body = Tools.GetStringDef("Body");
            Docked = evt.Value < bool ?>("Docked") ?? false;
            StationName = Tools.GetStringDef("StationName");
            StationType = Tools.GetStringDef("StationType");
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

        public string Body { get; set; }
        public bool Docked { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
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
