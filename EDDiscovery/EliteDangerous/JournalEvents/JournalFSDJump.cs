/*
 * Copyright © 2016 EDDiscovery development team
 *
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this
 * file except in compliance with the License. You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software distributed under
 * the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF
 * ANY KIND, either express or implied. See the License for the specific language
 * governing permissions and limitations under the License.
 *
 * EDDiscovery is not affiliated with Frontier Developments plc.
 */
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


    [JournalEntryType(JournalTypeEnum.FSDJump)]
    public class JournalFSDJump : JournalLocOrJump
    {
        public JournalFSDJump(JObject evt) : base(evt, JournalTypeEnum.FSDJump)
        {
            Body = evt["body"].Str();
            JumpDist = evt["JumpDist"].Double();
            FuelUsed = evt["FuelUsed"].Double();
            RealJournalEvent = evt["FuelUsed"].Empty(); // Old pre ED 2.2 messages has no Fuel used fields
            FuelLevel = evt["FuelLevel"].Double();
            BoostUsed = evt["BoostUsed"].Bool();
            Faction = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemFaction", "Faction"});
            FactionState = evt["FactionState"].Str();
            Allegiance = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemAllegiance", "Allegiance" });
            Economy = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy", "Economy" });
            Economy_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy_Localised", "Economy_Localised" });
            Government = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemGovernment", "Government" });
            Government_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "", "SystemGovernment_Localised" });
            Security = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "", "SystemSecurity" });
            Security_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "", "SystemSecurity_Localised" });
            PowerplayState = evt["PowerplayState"].Str();

            if (!evt["Powers"].Empty())
                Powers = evt.Value<JArray>("Powers").Values<string>().ToArray();

            JToken jm = evt["EDDMapColor"];
            MapColor = jm.Int(EDDConfig.Instance.DefaultMapColour.ToArgb());
            if (jm.Empty())
                evt["EDDMapColor"] = EDDConfig.Instance.DefaultMapColour.ToArgb();      // new entries get this default map colour if its not already there
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
        public int MapColor { get; set; }
        public bool RealJournalEvent { get; private set; } // True if real ED 2.2+ journal event and not pre 2.2 imported.

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = "Jump to " + StarSystem;
            info = "";
            if (JumpDist > 0)
                info += JumpDist.ToString("0.00") + " ly";
            if (FuelUsed > 0)
                info += ", Fuel " + FuelUsed.ToString("0.0") + "t";
            if (FuelLevel > 0)
                info += " left " + FuelLevel.ToString("0.0") + "t";

            string econ = Economy_Localised.Alt(Economy);
            if (econ.Equals("None"))
                econ = "";

            info += " ";
            info += Tools.FieldBuilder("Body:", Body, "Faction:", Faction, "<state:", FactionState, "Allegiance:", Allegiance, "Economy:", econ);
            detailed = "";
        }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.hyperspace; } }

    }
}
