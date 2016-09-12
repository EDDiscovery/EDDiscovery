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
        public JournalLocation(JObject evt) : base(evt, JournalTypeEnum.Location)
        {
            Body = evt.Value<string>("Body");
            Docked = evt.Value<bool?>("Docked") ?? false;
            StationName = evt.Value<string>("StationName");
            StationType = evt.Value<string>("StationType");
            Faction = evt.Value<string>("Faction");
            FactionState = evt.Value<string>("FactionState");
            Allegiance = evt.Value<string>("Allegiance");
            Economy = evt.Value<string>("Economy");
            Economy_Localised = evt.Value<string>("Economy_Localised");
            Government = evt.Value<string>("Government");
            Government_Localised = evt.Value<string>("Government_Localised");
            Security = evt.Value<string>("Security");
            Security_Localised = evt.Value<string>("Security_Localised");
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
