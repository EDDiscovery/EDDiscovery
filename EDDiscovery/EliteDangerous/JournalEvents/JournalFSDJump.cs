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
            Body = Tools.GetStringDef(evt["body"], "Unknown");
            JumpDist = Tools.GetDouble(evt["JumpDist"]);
            FuelUsed = Tools.GetDouble(evt["FuelUsed"]);
            FuelLevel = Tools.GetDouble(evt["FuelLevel"]);
            BoostUsed = Tools.GetBool(evt["BoostUsed"]);
            Faction = Tools.GetStringDef(evt["Faction"]);
            FactionState = Tools.GetStringDef(evt["FactionState"]);
            Allegiance = Tools.GetMultiStringDef(evt, new string[] { "SystemAllegiance", "Allegiance" });
            Economy = Tools.GetMultiStringDef(evt, new string[] { "SystemEconomy", "Economy" });
            Economy_Localised = Tools.GetMultiStringDef(evt, new string[] { "SystemEconomy_Localised", "Economy_Localised" });
            Government = Tools.GetMultiStringDef(evt, new string[] { "SystemGovernment", "Government" });
            Government_Localised = Tools.GetMultiStringDef(evt, new string[] { "", "SystemGovernment_Localised" });
            Security = Tools.GetMultiStringDef(evt, new string[] { "", "SystemSecurity" });
            Security_Localised = Tools.GetMultiStringDef(evt, new string[] { "", "SystemSecurity_Localised" });
            PowerplayState = Tools.GetStringDef(evt["PowerplayState"]);

            if (!Tools.IsNullOrEmptyT(evt["Powers"]))
                Powers = evt.Value<JArray>("Powers").Values<string>().ToArray();


            JToken jm = jEventData["EDDMapColor"];
            if (Tools.IsNullOrEmptyT(jm))
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

        public int MapColor
        {
            get
            {
                return Tools.GetInt(jEventData["EDDMapColor"], Color.Red.ToArgb());
            }
            set
            {
                jEventData["EDDMapColor"] = value;
            }
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.hyperspace; } }

    }
}
