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
        public JournalFSDJump(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.FSDJump, reader)
        {
            Body = Tools.GetStringDef(evt["body"], "Unknown");
            JumpDist = Tools.GetDouble("JumpDist");
            FuelUsed = Tools.GetDouble("FuelUsed");
            FuelLevel = Tools.GetDouble("FuelLevel");
            BoostUsed = Tools.GetBool("BoostUsed");
            Faction = Tools.GetStringDef("Faction","");
            FactionState = Tools.GetStringDef("FactionState", "");
            Allegiance = Tools.GetStringDef("Allegiance", "");
            Economy = Tools.GetStringDef("Economy", "");
            Economy_Localised = Tools.GetStringDef("Economy_Localised", "");
            Government = Tools.GetStringDef("Government", "");
            Government_Localised = Tools.GetStringDef("Government_Localised", "");
            Security = Tools.GetStringDef("Security", "");
            Security_Localised = Tools.GetStringDef("Security_Localised", "");
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
            summary = Tools.SplitCapsWord(EventType.ToString());
            info = "Fuel Used " + FuelUsed.ToString("0.0"); 
            detailed = Tools.SplitCapsWord(ToShortString().Replace("\"", "")); 
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
