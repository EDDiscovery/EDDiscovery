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
        public JournalLocation(JObject evt ) : base(evt, JournalTypeEnum.Location)
        {
            Body = Tools.GetStringDef(evt["Body"]);
            Docked = evt.Value < bool ?>("Docked") ?? false;
            StationName = Tools.GetStringDef(evt["StationName"]);
            StationType = Tools.GetStringDef(evt["StationType"]);
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

        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = "At " + StarSystem;
            if (Docked)
                info = "Docked at " + StationName + " " + StationType + " Type (" + Economy_Localised + ")";
            else
                info = "In space";

            detailed = ToShortString("StarSystem;StationName;StationType;Docked;Economy_Localised");      
        }

    }
}
