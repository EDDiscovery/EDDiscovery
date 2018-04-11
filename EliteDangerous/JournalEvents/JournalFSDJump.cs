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
    /*
     When written: when jumping from one star system to another
Parameters:
 StarSystem: name of destination starsystem
 StarPos: star position, as a Json array [x, y, z], in light years
 Body: star’s body name
 JumpDist: distance jumped
 FuelUsed
 FuelLevel
 BoostUsed: whether FSD boost was used
 SystemFaction: system controlling faction
 FactionState
 SystemAllegiance
 SystemEconomy
 SystemGovernment
 SystemSecurity
 Population 
10
 Factions: an array of info for the local minor factions
o Name
o FactionState
o Government
o Influence
o PendingStates: array (if any) with State name and Trend value
o RecovingStates: array (if any)with State name and Trend value
If the player is pledged to a Power in Powerplay, and the star system is involved in powerplay,
 Powers: a json array with the names of any powers contesting the system, or the name of
the controlling power
 PowerplayState: the system state – one of ("InPrepareRadius", "Prepared", "Exploited",
"Contested", "Controlled", "Turmoil", "HomeSystem")
Example:
{ "timestamp":"2017-02-27T15:37:47Z", "event":"FSDJump", "StarSystem":"HR 3316", "StarPos":[100.719,19.813,-51.125],
"SystemAllegiance":"Independent", "SystemEconomy":"$economy_Colony;", "SystemEconomy_Localised":"Colony",
"SystemGovernment":"$government_Democracy;", "SystemGovernment_Localised":"Democracy",
"SystemSecurity":"$SYSTEM_SECURITY_medium;", "SystemSecurity_Localised":"Medium Security", "JumpDist":20.326,
"FuelUsed":1.260775, "FuelLevel":12.872868, "Factions":[ { "Name":"Independent HR 3316 Liberals",
"FactionState":"Outbreak", "Government":"Democracy", "Influence":0.550000 }, { "Name":"Jet Natural Partners",
"FactionState":"None", "Government":"Corporate", "Influence":0.150000 }, { "Name":"Camorra of HR 3316",
"FactionState":"None", "Government":"Anarchy", "Influence":0.090000 }, { "Name":"HR 3316 Nobles",
"FactionState":"None", "Government":"Feudal", "Influence":0.210000 } ], "SystemFaction":"Independent HR 3316
Liberals", "FactionState":"Outbreak" }
Examples of trending states:
... "Factions":[ { "Name":"Inupiates Patrons of Law", "FactionState":"Lockdown", "Government":"Patronage",
"Influence":0.550000, "Allegiance":"Empire", "PendingStates":[ { "State":"Boom", "Trend":0 }, { "State":"CivilUnrest",
"Trend":0 } ] }, ...
... "Factions":[ { "Name":"IV Comae Berenices Purple Creative", "FactionState":"CivilWar", "Government":"Corporat
     * */


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
    public class JournalFSDJump : JournalLocOrJump, IShipInformation, ISystemStationEntry
    {
        public JournalFSDJump(JObject evt) : base(evt, JournalTypeEnum.FSDJump)
        {
            RealJournalEvent = evt["FuelUsed"].Empty(); // Old pre ED 2.2 messages has no Fuel used fields

            // base class does StarSystem/StarPos/Faction/Powerplay

            JumpDist = evt["JumpDist"].Double();
            FuelUsed = evt["FuelUsed"].Double();
            FuelLevel = evt["FuelLevel"].Double();
            BoostUsed = evt["BoostUsed"].Bool();
            BoostValue = evt["BoostUsed"].Int();

            JToken jm = evt["EDDMapColor"];
            MapColor = jm.Int(EliteDangerousCore.EliteConfigInstance.InstanceConfig.DefaultMapColour);
            if (jm.Empty())
                evt["EDDMapColor"] = EliteDangerousCore.EliteConfigInstance.InstanceConfig.DefaultMapColour;      // new entries get this default map colour if its not already there
        }

        public double JumpDist { get; set; }
        public double FuelUsed { get; set; }
        public double FuelLevel { get; set; }
        public bool BoostUsed { get; set; }
        public int BoostValue { get; set; }
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
            info += BaseUtils.FieldBuilder.Build("Faction:", Faction, "<;(Wanted) ", Wanted, "State:", FactionState, "Allegiance:", Allegiance, "Economy:", econ, "Population:", Population);
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

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.FSDJump(this);
        }
    }
}
