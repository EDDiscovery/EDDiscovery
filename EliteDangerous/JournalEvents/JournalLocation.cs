/*
 * Copyright © 2016-2018 EDDiscovery development team
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
        public bool Wanted { get; set; }

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
            public string Happiness { get; set; }   //3.3, may be null
            public string Happiness_Localised { get; set; } //3.3, may be null
            public double? MyReputation { get; set; } //3.3, may be null

            public PowerStatesInfo[] PendingStates { get; set; }
            public PowerStatesInfo[] RecoveringStates { get; set; }
            public ActiveStatesInfo[] ActiveStates { get; set; }    //3.3, may be null
        }

        public class PowerStatesInfo
        {
            public string State { get; set; }
            public int Trend { get; set; }
        }

        public class ActiveStatesInfo       // Howard info via discord .. just state
        {
            public string State { get; set; }
        }

        protected JournalLocOrJump(DateTime utc, ISystem sys, int synced, JournalTypeEnum jtype) : base(utc, synced, jtype)
        {
            StarSystem = sys.Name;
            StarPos = new EMK.LightGeometry.Vector3((float)sys.X, (float)sys.Y, (float)sys.Z);
            EdsmID = sys.EDSMID;
        }

        protected JournalLocOrJump(JObject evt, JournalTypeEnum jtype) : base(evt, jtype)
        {
            StarSystem = evt["StarSystem"].Str();
            StarPosFromEDSM = evt["StarPosFromEDSM"].Bool(false);

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

                foreach( var x in Factions)     // normalise localised
                    x.Happiness_Localised = JournalFieldNaming.CheckLocalisation(x.Happiness_Localised, x.Happiness);
            }

            Allegiance = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemAllegiance", "Allegiance" });

            Economy = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy", "Economy" });
            Economy_Localised = JournalFieldNaming.CheckLocalisation(JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemEconomy_Localised", "Economy_Localised" }), Economy);

            SecondEconomy = evt["SystemSecondEconomy"].Str();
            SecondEconomy_Localised = JournalFieldNaming.CheckLocalisation(evt["SystemSecondEconomy_Localised"].Str(), SecondEconomy);

            Government = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemGovernment", "Government" });
            Government_Localised = JournalFieldNaming.CheckLocalisation(JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemGovernment_Localised", "Government_Localised" }), Government);

            Security = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemSecurity", "Security" });
            Security_Localised = JournalFieldNaming.CheckLocalisation(JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemSecurity_Localised", "Security_Localised" }), Security);

            Wanted = evt["Wanted"].Bool();      // if absence, your not wanted, by definition of frontier in journal (only present if wanted, see docked)

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


    //When written: at startup, or when being resurrected at a station
    [JournalEntryType(JournalTypeEnum.Location)]
    public class JournalLocation : JournalLocOrJump, ISystemStationEntry, IBodyNameAndID
    {
        public JournalLocation(JObject evt) : base(evt, JournalTypeEnum.Location)      // all have evidence 16/3/2017
        {
            // base class does StarSystem/StarPos/Faction/Powerplay

            Docked = evt.Value<bool?>("Docked") ?? false;
            StationName = evt["StationName"].Str();
            StationType = evt["StationType"].Str().SplitCapsWord();
            Body = evt["Body"].Str();
            BodyID = evt["BodyID"].IntNull();
            BodyType = JournalFieldNaming.NormaliseBodyType(evt["BodyType"].Str());

            Latitude = evt["Latitude"].DoubleNull();
            Longitude = evt["Longitude"].DoubleNull();

            MarketID = evt["MarketID"].LongNull();
        }

        public bool Docked { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        public string Body { get; set; }
        public int? BodyID { get; set; }
        public string BodyType { get; set; }
        public string BodyDesignation { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public long? MarketID { get; set; }

        public override string FillSummary { get
            {
                if (Docked)
                    return string.Format("At {0}".Tx(this, "AtStat"), StationName);
                else if (Latitude.HasValue && Longitude.HasValue)
                    return string.Format("Landed on {0}".Tx(this, "LND"), Body);
                else
                    return string.Format("At {0}".Tx(this, "AtStar"), StarSystem);
            } }

        public override void FillInformation(out string info, out string detailed) 
        {
            if (Docked)
            {
                info = BaseUtils.FieldBuilder.Build("Type ".Txb(this), StationType, "< in system ".Txb(this), StarSystem);
                detailed = BaseUtils.FieldBuilder.Build("<;(Wanted) ".Txb(this), Wanted, "Allegiance:".Txb(this), Allegiance, "Economy:".Txb(this), Economy_Localised, "Government:".Txb(this), Government_Localised, "Security:".Txb(this), Security_Localised);

                if (Factions != null)
                {
                    foreach (FactionInformation f in Factions)
                    {
                        detailed += Environment.NewLine;
                        detailed += BaseUtils.FieldBuilder.Build("", f.Name, "State:".Txb(this), f.FactionState, "Government:".Txb(this), f.Government, 
                            "Inf:;%".Txb(this), (int)(f.Influence * 100), "Allegiance:".Txb(this), f.Allegiance, "Happiness:".Txb(this), f.Happiness_Localised,
                            "Reputation:;%;N1".Txb(this), f.MyReputation);

                        if (f.PendingStates != null)
                        {
                            detailed += BaseUtils.FieldBuilder.Build(",", "Pending State:".Txb(this));
                            foreach (JournalLocation.PowerStatesInfo state in f.PendingStates)
                                detailed += BaseUtils.FieldBuilder.Build(" ", state.State, "<(;)", state.Trend);

                            if (f.RecoveringStates != null)
                            {
                                detailed += BaseUtils.FieldBuilder.Build(",", "Recovering State:".Txb(this));
                                foreach (JournalLocation.PowerStatesInfo state in f.RecoveringStates)
                                    detailed += BaseUtils.FieldBuilder.Build(" ", state.State, "<(;)", state.Trend);
                            }

                            if (f.ActiveStates != null)       // re-do when fixed.
                            {
                                detailed += BaseUtils.FieldBuilder.Build(",", "Active State:".Txb(this));
                                foreach (JournalLocation.ActiveStatesInfo state in f.ActiveStates)
                                    detailed += BaseUtils.FieldBuilder.Build(" ", state.State);
                            }
                        }
                    }
                }
            }
            else if (Latitude.HasValue && Longitude.HasValue)
            {
                info = "At " + JournalFieldNaming.RLat(Latitude.Value) + " " + JournalFieldNaming.RLong(Longitude.Value);
                detailed = "";
            }
            else
            {
                info = BaseUtils.FieldBuilder.Build("In space near ".Txb(this), Body, "< of type ".Txb(this), BodyType);
                detailed = "";
            }
        }
    }
}
