using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
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
            Allegiance = Tools.GetStringDef(evt["Allegiance"]);
            Economy = Tools.GetStringDef(evt["Economy"]);
            Economy_Localised = Tools.GetStringDef(evt["Economy_Localised"]);
            Government = Tools.GetStringDef(evt["Government"]);
            Government_Localised = Tools.GetStringDef(evt["Government_Localised"]);
            Security = Tools.GetStringDef(evt["Security"]);
            Security_Localised = Tools.GetStringDef(evt["Security_Localised"]);
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

        public override void FillInformation(out string summary, out string info, out string detailed)
        {
            summary = "Jump to " + StarSystem;
            info = "";
            if (JumpDist > 0)
                info += JumpDist.ToString("0.00") + " ly";
            if ( FuelUsed > 0 )
                info += " Fuel " + FuelUsed.ToString("0.0");
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
    }
}
