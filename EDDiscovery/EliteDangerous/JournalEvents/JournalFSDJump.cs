using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    public class JournalFSDJump : JournalLocOrJump
    {
        public JournalFSDJump(JObject evt, EDJournalReader reader) : base(evt, JournalTypeEnum.FSDJump, reader)
        {
            Body = evt.Value<string>("Body");
            JumpDist = evt.Value<double>("JumpDist");
            FuelUsed = evt.Value<double>("FuelUsed");
            FuelLevel = evt.Value<double>("FuelLevel");
            BoostUsed = evt.Value<bool>("BoostUsed");
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
    }
}
