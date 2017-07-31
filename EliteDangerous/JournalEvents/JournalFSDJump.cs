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

namespace EliteDangerousCore.JournalEvents
{
//    When written: when jumping from one star system to another

//{ "timestamp":"2017-05-01T08:24:52Z", "event":"FSDJump",
//"StarSystem":"Lauma", "StarPos":[16.813,-43.813,38.594],          -- BASE CLASS

//"SystemAllegiance":"Federation",      -- Allegiance
//"SystemEconomy":"$economy_Refinery;", "SystemEconomy_Localised":"Refinery",       --ECONOMY/LOC
//"SystemGovernment":"$government_Corporate;", "SystemGovernment_Localised":"Corporate",         -- GOVERNMENT/LOC  
//"SystemSecurity":"$SYSTEM_SECURITY_medium;", "SystemSecurity_Localised":"Medium Security", // SECURITY
// "JumpDist":6.237, //JUMPDIST
//"FuelUsed":0.129626, "FuelLevel":31.685003,   // FUELx

//"Factions":[ { "Name":"Workers
//of Lauma Labour", "FactionState":"Boom", "Government":"Democracy",
//"Influence":0.181000, "Allegiance":"Federation" }, { "Name":"Nerthus
//Citizens of Tradition", "FactionState":"Boom", "Government":"Patronage",
//"Influence":0.118000, "Allegiance":"Empire" }, { "Name":"HDS 3215 Defence
//Party", "FactionState":"Boom", "Government":"Dictatorship",
//"Influence":0.109000, "Allegiance":"Empire" }, { "Name":"Lauma
//Nationalists", "FactionState":"Boom", "Government":"Dictatorship",
//"Influence":0.056000, "Allegiance":"Independent" }, { "Name":"Gebel Empire
//League", "FactionState":"Boom", "Government":"Patronage",
//"Influence":0.109000, "Allegiance":"Empire" }, { "Name":"Silver Legal Ltd",
//"FactionState":"None", "Government":"Corporate", "Influence":0.380000,
//"Allegiance":"Federation" }, { "Name":"Lauma Jet Cartel",
//"FactionState":"None", "Government":"Anarchy", "Influence":0.047000,
//"Allegiance":"Independent" } ],

//"SystemFaction":"Silver Legal Ltd" }
//If the player is pledged to a Power in Powerplay, and the star system is involved in powerplay,
//•	Powers: a json array with the names of any powers contesting the system, or the name of the controlling power
//•	PowerplayState: the system state – one of("InPrepareRadius", "Prepared", "Exploited", "Contested", "Controlled", "Turmoil", "HomeSystem")


    [JournalEntryType(JournalTypeEnum.FSDJump)]
    public class JournalFSDJump : JournalLocOrJump, IShipInformation
    {
        public JournalFSDJump(JObject evt) : base(evt, JournalTypeEnum.FSDJump)
        {
            RealJournalEvent = evt["FuelUsed"].Empty(); // Old pre ED 2.2 messages has no Fuel used fields

            // base class does StarSystem/StarPos

            Allegiance = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemAllegiance", "Allegiance" });
            Economy = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy", "Economy" });
            Economy_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy_Localised", "Economy_Localised" });
            Government = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemGovernment", "Government" });
            Government_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "", "SystemGovernment_Localised" });
            Security = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "", "SystemSecurity" });
            Security_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "", "SystemSecurity_Localised" });
            JumpDist = evt["JumpDist"].Double();
            FuelUsed = evt["FuelUsed"].Double();
            FuelLevel = evt["FuelLevel"].Double();
            BoostUsed = evt["BoostUsed"].Bool();
            BoostValue = evt["BoostUsed"].Int();

            Faction = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemFaction", "Faction" });
            FactionState = evt["FactionState"].Str();           // PRE 2.3 .. not present in newer files, fixed up in next bit of code

            if (evt["Factions"] != null)
            {
                Factions = evt["Factions"]?.ToObject<FactionInformation[]>().OrderByDescending(x => x.Influence).ToArray();  // POST 2.3
                int i = Array.FindIndex(Factions, x => x.Name == Faction);
                if (i != -1)
                    FactionState = Factions[i].FactionState;        // set to State of controlling faction
            }

            PowerplayState = evt["PowerplayState"].Str();
            if (!evt["Powers"].Empty())
                Powers = evt.Value<JArray>("Powers").Values<string>().ToArray();

            JToken jm = evt["EDDMapColor"];
            MapColor = jm.Int(EliteDangerousCore.EliteConfigInstance.InstanceConfig.DefaultMapColour);
            if (jm.Empty())
                evt["EDDMapColor"] = EliteDangerousCore.EliteConfigInstance.InstanceConfig.DefaultMapColour;      // new entries get this default map colour if its not already there
        }

        public class FactionInformation
        {
            public string Name { get; set; }
            public string FactionState { get; set; }
            public string Government { get; set; }
            public double Influence { get; set; }
            public string Allegiance { get; set; }

            public JournalLocation.PowerStatesInfo[] PendingStates { get; set; }
            public JournalLocation.PowerStatesInfo[] RecoveringStates { get; set; }
        }

        public double JumpDist { get; set; }
        public double FuelUsed { get; set; }
        public double FuelLevel { get; set; }
        public bool BoostUsed { get; set; }
        public int BoostValue { get; set; }
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
        public FactionInformation[] Factions;
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
            info += BaseUtils.FieldBuilder.Build("Faction:", Faction, "State:", FactionState, "Allegiance:", Allegiance, "Economy:", econ);
            detailed = "";

            if ( Factions != null )
            {
                foreach (FactionInformation i in Factions)
                {
                    detailed += BaseUtils.FieldBuilder.Build("", i.Name, "State:", i.FactionState, "Gov:", i.Government, "Inf:;%", (i.Influence * 100.0).ToString("0.0"), "Alg:", i.Allegiance) ;
                    if (i.PendingStates != null)
                    {
                        detailed += BaseUtils.FieldBuilder.Build(",", "Pending State:");
                        foreach (JournalLocation.PowerStatesInfo state in i.PendingStates)
                            detailed += BaseUtils.FieldBuilder.Build(",", state.State, "", state.Trend);

                    }

                    if (i.RecoveringStates != null)
                    {
                        detailed += BaseUtils.FieldBuilder.Build(",", "Recovering State:");
                        foreach (JournalLocation.PowerStatesInfo state in i.RecoveringStates)
                            detailed += BaseUtils.FieldBuilder.Build(",", state.State, "", state.Trend);
                    }
                    detailed += Environment.NewLine;

                }
            }
        }

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.hyperspace; } }

        public void ShipInformation(ShipInformationList shp, DB.SQLiteConnectionUser conn)
        {
            shp.FSDJump(this);
        }
    }
}
