using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: when jumping from one star system to another
//Parameters:
//•	StarSystem: name of destination starsystem
//•	StarPos: star position, as a Json array[x, y, z], in light years
//•	Body: star’s body name
//•	JumpDist: distance jumped
//•	FuelUsed
//•	FuelLevel
//•	BoostUsed: whether FSD boost was used
//•	Faction: system controlling faction
//•	FactionState
//•	Allegiance
//•	Economy
//•	Government
//•	Security

//If the player is pledged to a Power in Powerplay, and the star system is involved in powerplay,
//•	Powers: a json array with the names of any powers contesting the system, or the name of the controlling power
//•	PowerplayState: the system state – one of("InPrepareRadius", "Prepared", "Exploited", "Contested", "Controlled", "Turmoil", "HomeSystem")



    public class JournalFSDJump : JournalLocOrJump
    {
        public JournalFSDJump(JObject evt ) : base(evt, JournalTypeEnum.FSDJump)
        {
            Body = JSONHelper.GetStringDef(evt["body"], "Unknown");
            JumpDist = JSONHelper.GetDouble(evt["JumpDist"]);
            FuelUsed = JSONHelper.GetDouble(evt["FuelUsed"]);
            FuelLevel = JSONHelper.GetDouble(evt["FuelLevel"]);
            BoostUsed = JSONHelper.GetBool(evt["BoostUsed"]);
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            FactionState = JSONHelper.GetStringDef(evt["FactionState"]);
            Allegiance = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemAllegiance", "Allegiance" });
            Economy = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemEconomy", "Economy" });
            Economy_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemEconomy_Localised", "Economy_Localised" });
            Government = JSONHelper.GetMultiStringDef(evt, new string[] { "SystemGovernment", "Government" });
            Government_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "", "SystemGovernment_Localised" });
            Security = JSONHelper.GetMultiStringDef(evt, new string[] { "", "SystemSecurity" });
            Security_Localised = JSONHelper.GetMultiStringDef(evt, new string[] { "", "SystemSecurity_Localised" });
            PowerplayState = JSONHelper.GetStringDef(evt["PowerplayState"]);

            if (!JSONHelper.IsNullOrEmptyT(evt["Powers"]))
                Powers = evt.Value<JArray>("Powers").Values<string>().ToArray();


            JToken jm = jEventData["EDDMapColor"];
            if (JSONHelper.IsNullOrEmptyT(jm))
                MapColor = EDDiscovery2.EDDConfig.Instance.DefaultMapColour;      // new entries get this default map colour if its not already there



        }

        public string Body { get; set; }
        public double JumpDist { get; set; }
        public double FuelUsed { get; set; }
        public double FuelLevel { get; set; }
        public bool BoostUsed { get; set; }
        public string Faction { get; set; }
        public string FactionState { get; set; }
        public string Allegiance { get; set; }
        public string Economy { get; set; }
        public string Economy_Localised { get; set; }
        public string Government { get; set; }
        public string Government_Localised { get; set; }
        public string Security { get; set; }
        public string Security_Localised { get; set; }
        public string PowerplayState { get; set; }
        public string[] Powers { get; set; }

        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = "Jump to " + StarSystem;
            info = "";
            if (JumpDist > 0)
                info += JumpDist.ToString("0.00") + " ly";
            if ( FuelUsed > 0 )
                info += " Fuel " + FuelUsed.ToString("0.0") + "t";
            detailed = ToShortString("StarSystem;JumpDist;FuelUsed");       // don't repeat these.
        }

        public bool RealJournalEvent  // True if real ED 2.2+ journal event and not pre 2.2 imported.
        {
            get
            {
                if (JSONHelper.IsNullOrEmptyT(jEventData["FuelUsed"]))  // Old pre ED 2.2 messages has no Fuel used fields
                    return false;
                else
                    return true;
            }
        }


        public int MapColor
        {
            get
            {
                return JSONHelper.GetInt(jEventData["EDDMapColor"], Color.Red.ToArgb());
            }
            set
            {
                jEventData["EDDMapColor"] = value;
            }
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.hyperspace; } }

    }
}
