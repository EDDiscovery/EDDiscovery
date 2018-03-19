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
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    public abstract class JournalLocOrJump : JournalEntry, ISystemStationEntry
    {
        private static DateTime ED_No_Training_Timestamp = new DateTime(2017, 10, 4, 0, 0, 0, DateTimeKind.Utc);
        private static DateTime ED_No_Faction_Timestamp = new DateTime(2017, 9, 26, 0, 0, 0, DateTimeKind.Utc);

        public string StarSystem { get; set; }
        public EMK.LightGeometry.Vector3 StarPos { get; set; }
        public long? SystemAddress { get; set; }
        public bool StarPosFromEDSM { get; set; }
        public bool EDSMFirstDiscover { get; set; }

        public string Faction { get; set; }
        public string FactionState { get; set; }
        public string Allegiance { get; set; }
        public string Economy { get; set; }
        public string Economy_Localised { get; set; }
        public string SecondEconomy { get; set; }
        public string SecondEconomy_Localised { get; set; }
        public string Government { get; set; }
        public string Government_Localised { get; set; }
        public string Security { get; set; }
        public string Security_Localised { get; set; }
        public long? Population { get; set; }
        public string PowerplayState { get; set; }
        public string[] PowerplayPowers { get; set; }
        public bool? Wanted { get; set; }

        public FactionInformation[] Factions { get; set; }

        public bool HasCoordinate { get { return !float.IsNaN(StarPos.X); } }

        public bool IsTrainingEvent { get; private set; } // True if detected to be in training

        public class FactionInformation
        {
            public string Name { get; set; }
            public string FactionState { get; set; }
            public string Government { get; set; }
            public double Influence { get; set; }
            public string Allegiance { get; set; }

            public PowerStatesInfo[] PendingStates { get; set; }
            public PowerStatesInfo[] RecoveringStates { get; set; }
        }

        public class PowerStatesInfo
        {
            public string State { get; set; }
            public int Trend { get; set; }
        }

        protected JournalLocOrJump(JObject evt, JournalTypeEnum jtype) : base(evt, jtype)
        {
            StarSystem = evt["StarSystem"].Str();
            StarPosFromEDSM = evt["StarPosFromEDSM"].Bool(false);
            EDSMFirstDiscover = evt["EDD_EDSMFirstDiscover"].Bool(false);

            EMK.LightGeometry.Vector3 pos = new EMK.LightGeometry.Vector3();

            if (!evt["StarPos"].Empty())            // if its an old VS entry, may not have co-ords
            {
                JArray coords = evt["StarPos"] as JArray;
                pos.X = coords[0].Float();
                pos.Y = coords[1].Float();
                pos.Z = coords[2].Float();
            }
            else
            {
                pos.X = pos.Y = pos.Z = float.NaN;
            }

            StarPos = pos;

            SystemAddress = evt["SystemAddress"].LongNull();

            Faction = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemFaction", "Faction" });
            FactionState = evt["FactionState"].Str();           // PRE 2.3 .. not present in newer files, fixed up in next bit of code
            Factions = evt["Factions"]?.ToObjectProtected<FactionInformation[]>()?.OrderByDescending(x => x.Influence)?.ToArray();  // POST 2.3

            if (Factions != null)
            {
                int i = Array.FindIndex(Factions, x => x.Name == Faction);
                if (i != -1)
                    FactionState = Factions[i].FactionState;        // set to State of controlling faction
            }

            Allegiance = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemAllegiance", "Allegiance" });

            Economy = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy", "Economy" });
            Economy_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy_Localised", "Economy_Localised" }).Alt(Economy);

            SecondEconomy = evt["SystemSecondEconomy"].Str();
            SecondEconomy_Localised = evt["SystemSecondEconomy_Localised"].Str().Alt(SecondEconomy);

            Government = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemGovernment", "Government" });
            Government_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemGovernment_Localised", "Government_Localised" }).Alt(Government);

            Security = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemSecurity", "Security" });
            Security_Localised = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemSecurity_Localised", "Security_Localised" }).Alt(Security);

            Wanted = evt["Wanted"].BoolNull();

            PowerplayState = evt["PowerplayState"].Str();            // NO evidence
            PowerplayPowers = evt["Powers"]?.ToObjectProtected<string[]>();

            // Allegiance without Faction only occurs in Training
            if (!String.IsNullOrEmpty(Allegiance) && Faction == null && EventTimeUTC <= ED_No_Training_Timestamp && (EventTimeUTC <= ED_No_Faction_Timestamp || EventTypeID != JournalTypeEnum.FSDJump || StarSystem == "Eranin"))
            {
                IsTrainingEvent = true;
            }
        }

        public EDGovernment EDGovernment
        {
            get
            {
                EDGovernment government;
                if (Government != null && Government.StartsWith("$government_") && Enum.TryParse(Government.Substring(12), out government))
                {
                    return government;
                }
                if (Government == "$government_PrisonColony;") return EDGovernment.Prison_Colony;
                return EDGovernment.Unknown;
            }
        }

        public EDEconomy EDEconomy
        {
            get
            {
                EDEconomy economy;
                if (Economy != null && Economy.StartsWith("$economy_") && Enum.TryParse(Economy.Substring(9), out economy))
                {
                    return economy;
                }
                if (Economy == "$economy_Agri;") return EDEconomy.Agriculture;
                return EDEconomy.Unknown;
            }
        }

        public EDSecurity EDSecurity
        {
            get
            {
                switch (Security)
                {
                    case "$GAlAXY_MAP_INFO_state_anarchy;": return EDSecurity.Anarchy;
                    case "$GALAXY_MAP_INFO_state_lawless;": return EDSecurity.Lawless;
                    case "$SYSTEM_SECURITY_low;": return EDSecurity.Low;
                    case "$SYSTEM_SECURITY_medium;": return EDSecurity.Medium;
                    case "$SYSTEM_SECURITY_high;": return EDSecurity.High;
                    default: return EDSecurity.Unknown;
                }
            }
        }

        public EDState EDState
        {
            get
            {
                EDState state;
                if (FactionState != null && Enum.TryParse(FactionState, out state))
                {
                    return state;
                }
                if (FactionState == "CivilUnrest") return EDState.Civil_Unrest;
                if (FactionState == "CivilWar") return EDState.Civil_War;
                return EDState.Unknown;
            }
        }

        public EDAllegiance EDAllegiance
        {
            get
            {
                EDAllegiance allegiance;
                if (Allegiance != null && Enum.TryParse(Allegiance, out allegiance))
                {
                    return allegiance;
                }
                return EDAllegiance.Unknown;
            }
        }
    }
}
