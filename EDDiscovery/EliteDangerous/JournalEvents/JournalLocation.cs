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
            Body = JSONHelper.GetStringDef(evt["Body"]);
            Docked = evt.Value < bool ?>("Docked") ?? false;
            StationName = JSONHelper.GetStringDef(evt["StationName"]);
            StationType = JSONHelper.GetStringDef(evt["StationType"]);
            Faction = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemFaction", "Faction" });
            FactionState = JSONHelper.GetStringDef(evt["FactionState"]);
            Allegiance = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemAllegiance", "Allegiance"});
            Economy = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemEconomy", "Economy" });
            Economy_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemEconomy_Localised", "Economy_Localised" });
            Government = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemGovernment", "Government" });
            Government_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemGovernment_Localised", "Government_Localised" });
            Security = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemSecurity", "Security" });
            Security_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemSecurity_Localised", "Security_Localised" });
            BodyType = JSONHelper.GetStringDef(evt["BodyType"]);

            PowerplayState = JSONHelper.GetStringDef(evt["PowerplayState"]);

            if (!JSONHelper.IsNullOrEmptyT(evt["Powers"]))
                Powers = evt.Value<JArray>("Powers").Values<string>().ToArray();

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
        public string BodyType { get; set; }
        public string PowerplayState { get; set; }
        public string[] Powers { get; set; }


        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = "At " + StarSystem;
            if (Docked)
                info = "Docked at " + StationName + " " + StationType + " Type (" + Economy_Localised + ")";
            else
                info = "In space";

            detailed = ToShortString("StarSystem;StationName;StationType;Docked;Economy_Localised");      
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.location; } }

    }
}
