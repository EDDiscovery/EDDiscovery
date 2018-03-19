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
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{

    /*
    When written: when landing at landing pad in a space station, outpost, or surface settlement
    Parameters:
     StationName: name of station
     StationType: type of station
     StarSystem: name of system
     CockpitBreach:true (only if landing with breached cockpit)
     StationFaction: station’s controlling faction
     FactionState
     StationAllegiance
     StationEconomy
     StationGovernment
     DistFromStarLS
     StationServices: (Array of strings)
    Example:
    { "timestamp":"2017-08-18T10:52:26Z", "event":"Docked", "StationName":"Goddard Hub",
    "StationType":"Coriolis", "StarSystem":"HR 3499", "StationFaction":"Labour of HR 3499",
    "StationGovernment":"$government_Democracy;", "StationGovernment_Localised":"Democracy",
    "StationAllegiance":"Federation", "StationServices":[ "Dock", "Autodock", "BlackMarket",
    "Commodities", "Contacts", "Exploration", "Missions", "Outfitting", "CrewLounge", "Rearm",
    "Refuel", "Repair", "Shipyard", "Tuning", "MissionsGenerated", "FlightController",
    "StationOperations", "Powerplay", "SearchAndRescue" ], "StationEconomy":"$economy_Industrial;",
    "StationEconomy_Localised":"Industrial", "DistFromStarLS":129.454132 }
    StationServices can include:
    Dock, Autodock, BlackMarket, Commodities, Contacts, Exploration, Initiatives, Missions,
    Outfitting,CrewLounge, Rearm, Refuel, Repair, Shipyard, Tuning, Workshop, MissionsGenerated,
    Facilitator, Research, FlightController, StationOperations, OnDockMission, Powerplay,
    SearchAndRescue, 
     */
    [JournalEntryType(JournalTypeEnum.Docked)]
    public class JournalDocked : JournalEntry, ISystemStationEntry
    {
        public JournalDocked(JObject evt ) : base(evt, JournalTypeEnum.Docked)
        {
            StationName = evt["StationName"].Str();
            StationType = evt["StationType"].Str().SplitCapsWord();
            StarSystem = evt["StarSystem"].Str();
            SystemAddress = evt["SystemAddress"].LongNull();
            MarketID = evt["MarketID"].LongNull();
            CockpitBreach = evt["CockpitBreach"].Bool();

            Faction = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationFaction", "Faction" });
            FactionState = evt["FactionState"].Str().SplitCapsWord();

            Allegiance = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationAllegiance", "Allegiance" });

            Economy = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationEconomy", "Economy" });
            Economy_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationEconomy_Localised", "Economy_Localised" }).Alt(Economy);
            EconomyList = evt["StationEconomies"]?.ToObjectProtected<Economies[]>();

            Government = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationGovernment", "Government" });
            Government_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "StationGovernment_Localised", "Government_Localised" }).Alt(Government);

            Wanted = evt["Wanted"].BoolNull();

            StationServices = evt["StationServices"]?.ToObjectProtected<string[]>();

            // Government = None only happens in Training
            if (Government == "$government_None;")
            {
                IsTrainingEvent = true;
            }
        }

        public string StationName { get; set; }
        public string StationType { get; set; }
        public string StarSystem { get; set; }
        public long? SystemAddress { get; set; }
        public long? MarketID { get; set; }
        public bool CockpitBreach { get; set; }
        public string Faction { get; set; }
        public string FactionState { get; set; }
        public string Allegiance { get; set; }
        public string Economy { get; set; }
        public string Economy_Localised { get; set; }
        public Economies[] EconomyList { get; set; }        // may be null
        public string Government { get; set; }
        public string Government_Localised { get; set; }
        public string[] StationServices { get; set; }
        public bool? Wanted { get; set; }

        public bool IsTrainingEvent { get; private set; }

        public class Economies
        {
            public string Name;
            public string Name_Localised;
            public double Proportion;
        }

        public override void FillInformation(out string summary, out string info, out string detailed)      //V
        {
            summary = $"At {StationName}";
            info = BaseUtils.FieldBuilder.Build("Type ", StationType, "< in system ", StarSystem, ";Wanted" , Wanted, "Faction:", Faction, "< in state ", FactionState);
            detailed = BaseUtils.FieldBuilder.Build("Allegiance:", Allegiance, "Economy:", Economy_Localised, "Government:", Government_Localised);

            if (StationServices != null)
            {
                string l = "";
                foreach (string s in StationServices)
                    l = l.AppendPrePad(s, ", ");
                detailed += System.Environment.NewLine + "Station services:" + l;
            }

            if ( EconomyList != null )
            {
                string l = "";
                foreach (Economies e in EconomyList)
                    l = l.AppendPrePad(e.Name_Localised.Alt(e.Name) + " " + (e.Proportion * 100).ToString("0.#") + "%", ", ");
                detailed += System.Environment.NewLine + "Economies:" + l;
            }
        }

    }
}
