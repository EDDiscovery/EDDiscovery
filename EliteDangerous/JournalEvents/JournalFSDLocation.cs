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
using EliteDangerousCore.DB;
using System.Linq;
using System.Text;
using System.Data.Common;

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

        public string Faction { get; set; }         // System Faction - keep name for backwards compat.
        public string FactionState { get; set; }    // System Faction State - keep name for backwards compat.
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

        public ConflictInfo[] Conflicts { get; set; }

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

            public bool? SquadronFaction;       //3.3, may be null
            public bool? HappiestSystem;       //3.3, may be null
            public bool? HomeSystem;       //3.3, may be null
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

        public class ConflictFactionInfo   // 3.4
        {
            public string Name { get; set; }
            public string Stake { get; set; }
            public int WonDays { get; set; }
        }

        public class ConflictInfo   // 3.4
        {
            public string WarType { get; set; }
            public string Status { get; set; }
            public ConflictFactionInfo Faction1 { get; set; }
            public ConflictFactionInfo Faction2 { get; set; }
        }

        protected JournalLocOrJump(DateTime utc, ISystem sys, JournalTypeEnum jtype, bool edsmsynced ) : base(utc, jtype, edsmsynced)
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

            JToken jk = (JToken)evt["SystemFaction"];
            if (jk != null && jk.Type == JTokenType.Object)     // new 3.03
            {
                JObject jo = jk as JObject;
                Faction = jk["Name"].Str();                // system faction pick up
                FactionState = jk["FactionState"].Str();
            }
            else
            {
                // old pre 3.3.3 had this - for system faction
                Faction = JSONObjectExtensions.GetMultiStringDef(evt, new string[] { "SystemFaction", "Faction" });
                FactionState = evt["FactionState"].Str();           // PRE 2.3 .. not present in newer files, fixed up in next bit of code (but see 3.3.2 as its been incorrectly reintroduced)
            }

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

            Conflicts = evt["Conflicts"]?.ToObjectProtected<ConflictInfo[]>();   // 3.4

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
                if (Government != null && Government.StartsWith("$government_") && Enum.TryParse(Government.Substring(12), true, out government))
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
                if (Economy != null && Economy.StartsWith("$economy_") && Enum.TryParse(Economy.Substring(9), true, out economy))
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
                if (FactionState != null && Enum.TryParse(FactionState, true, out state))
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
                if (Allegiance != null && Enum.TryParse(Allegiance, true, out allegiance))
                {
                    return allegiance;
                }
                return EDAllegiance.Unknown;
            }
        }

        public string PowerList
        {
            get
            {
                if (PowerplayPowers != null && PowerplayPowers.Length > 0)
                    return string.Join(",", PowerplayPowers);
                else
                    return "";
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
            DistFromStarLS = evt["DistFromStarLS"].DoubleNull();

            Latitude = evt["Latitude"].DoubleNull();
            Longitude = evt["Longitude"].DoubleNull();

            MarketID = evt["MarketID"].LongNull();

            // station data only if docked..

            JToken jk = (JToken)evt["StationFaction"];  // 3.3.3 post

            if ( jk != null && jk.Type == JTokenType.Object)
            {
                JObject jo = jk as JObject;
                StationFaction = jk["Name"].Str();                // system faction pick up
                StationFactionState = jk["FactionState"].Str();
            }

            StationGovernment = evt["StationGovernment"].Str();       // 3.3.2 empty before
            StationGovernment_Localised = evt["StationGovernment_Localised"].Str();       // 3.3.2 empty before
            StationAllegiance = evt["StationAllegiance"].Str();       // 3.3.2 empty before
                                                                      // tbd bug in journal over FactionState - its repeated twice..
            StationServices = evt["StationServices"]?.ToObjectProtected<string[]>();
            StationEconomyList = evt["StationEconomies"]?.ToObjectProtected<JournalDocked.Economies[]>();
        }

        public bool Docked { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        public string Body { get; set; }
        public int? BodyID { get; set; }
        public string BodyType { get; set; }
        public string BodyDesignation { get; set; }
        public double? DistFromStarLS { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public long? MarketID { get; set; }

        // 3.3.2 will be empty/null for previous logs.
        public string StationFaction { get; set; }
        public string StationFactionState { get; set; }
        public string StationGovernment { get; set; }
        public string StationGovernment_Localised { get; set; }
        public string StationAllegiance { get; set; }
        public string[] StationServices { get; set; }
        public JournalDocked.Economies[] StationEconomyList { get; set; }        // may be null

        public override string SummaryName(ISystem sys) 
        {
            if (Docked)
                return string.Format("At {0}".T(EDTx.JournalLocation_AtStat), StationName);
            else
            {
                string bodyname = Body.HasChars() ? Body.ReplaceIfStartsWith(StarSystem) : StarSystem;
                if (Latitude.HasValue && Longitude.HasValue)
                    return string.Format("Landed on {0}".T(EDTx.JournalLocation_LND), bodyname);
                else
                    return string.Format("At {0}".T(EDTx.JournalLocation_AtStar), bodyname);
            }

        }

        public override void FillInformation(out string info, out string detailed) 
        {
            if (Docked)
            {
                info = BaseUtils.FieldBuilder.Build("Type ".T(EDTx.JournalLocOrJump_Type), StationType, "< in system ".T(EDTx.JournalLocOrJump_insystem), StarSystem);

                detailed = BaseUtils.FieldBuilder.Build("<;(Wanted) ".T(EDTx.JournalLocOrJump_Wanted), Wanted, "Faction:".T(EDTx.JournalLocOrJump_Faction), StationFaction, "Allegiance:".T(EDTx.JournalLocOrJump_Allegiance), Allegiance, "Economy:".T(EDTx.JournalLocOrJump_Economy), Economy_Localised, "Government:".T(EDTx.JournalLocOrJump_Government), Government_Localised, "Security:".T(EDTx.JournalLocOrJump_Security), Security_Localised);

                if (Factions != null)
                {
                    foreach (FactionInformation f in Factions)
                    {
                        detailed += Environment.NewLine;
                        detailed += BaseUtils.FieldBuilder.Build("", f.Name, "State:".T(EDTx.JournalLocOrJump_State), f.FactionState.SplitCapsWord(), "Government:".T(EDTx.JournalLocOrJump_Government), f.Government,
                            "Inf:;%".T(EDTx.JournalLocOrJump_Inf), (int)(f.Influence * 100), "Allegiance:".T(EDTx.JournalLocOrJump_Allegiance), f.Allegiance, "Happiness:".T(EDTx.JournalLocOrJump_Happiness), f.Happiness_Localised,
                            "Reputation:;%;N1".T(EDTx.JournalLocOrJump_Reputation), f.MyReputation,
                            ";Squadron System".T(EDTx.JournalLocOrJump_SquadronSystem), f.SquadronFaction, 
                            ";Happiest System".T(EDTx.JournalLocOrJump_HappiestSystem), f.HappiestSystem, 
                            ";Home System".T(EDTx.JournalLocOrJump_HomeSystem), f.HomeSystem
                            );

                        if (f.PendingStates != null)
                        {
                            detailed += BaseUtils.FieldBuilder.Build(",", "Pending State:".T(EDTx.JournalLocOrJump_PendingState));
                            foreach (JournalLocation.PowerStatesInfo state in f.PendingStates)
                                detailed += BaseUtils.FieldBuilder.Build(" ", state.State, "<(;)", state.Trend);

                            if (f.RecoveringStates != null)
                            {
                                detailed += BaseUtils.FieldBuilder.Build(",", "Recovering State:".T(EDTx.JournalLocOrJump_RecoveringState));
                                foreach (JournalLocation.PowerStatesInfo state in f.RecoveringStates)
                                    detailed += BaseUtils.FieldBuilder.Build(" ", state.State, "<(;)", state.Trend);
                            }

                            if (f.ActiveStates != null)    
                            {
                                detailed += BaseUtils.FieldBuilder.Build(",", "Active State:".T(EDTx.JournalLocOrJump_ActiveState));
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
                info = "In space near ".T(EDTx.JournalLocOrJump_Inspacenear) + BodyType + " " + Body;
                detailed = "";
            }
        }
    }

    //When written: when jumping with a fleet carrier
    [JournalEntryType(JournalTypeEnum.CarrierJump)]
    public class JournalCarrierJump : JournalLocOrJump, ISystemStationEntry, IBodyNameAndID
    {
        public JournalCarrierJump(JObject evt) : base(evt, JournalTypeEnum.CarrierJump)
        {
            // base class does StarSystem/StarPos/Faction/Powerplay

            Docked = evt.Value<bool?>("Docked") ?? false;
            StationName = evt["StationName"].Str();
            StationType = evt["StationType"].Str().SplitCapsWord();
            Body = evt["Body"].Str();
            BodyID = evt["BodyID"].IntNull();
            BodyType = JournalFieldNaming.NormaliseBodyType(evt["BodyType"].Str());
            DistFromStarLS = evt["DistFromStarLS"].DoubleNull();

            Latitude = evt["Latitude"].DoubleNull();
            Longitude = evt["Longitude"].DoubleNull();

            MarketID = evt["MarketID"].LongNull();

            // station data only if docked..

            JToken jk = (JToken)evt["StationFaction"];  // 3.3.3 post

            if (jk != null && jk.Type == JTokenType.Object)
            {
                JObject jo = jk as JObject;
                StationFaction = jk["Name"].Str();                // system faction pick up
                StationFactionState = jk["FactionState"].Str();
            }

            StationGovernment = evt["StationGovernment"].Str();       // 3.3.2 empty before
            StationGovernment_Localised = evt["StationGovernment_Localised"].Str();       // 3.3.2 empty before
            StationAllegiance = evt["StationAllegiance"].Str();       // 3.3.2 empty before
                                                                      // tbd bug in journal over FactionState - its repeated twice..
            StationServices = evt["StationServices"]?.ToObjectProtected<string[]>();
            StationEconomyList = evt["StationEconomies"]?.ToObjectProtected<JournalDocked.Economies[]>();
        }

        public bool Docked { get; set; }
        public string StationName { get; set; }
        public string StationType { get; set; }
        public string Body { get; set; }
        public int? BodyID { get; set; }
        public string BodyType { get; set; }
        public string BodyDesignation { get; set; }
        public double? DistFromStarLS { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public long? MarketID { get; set; }

        public string StationFaction { get; set; }
        public string StationFactionState { get; set; }
        public string StationGovernment { get; set; }
        public string StationGovernment_Localised { get; set; }
        public string StationAllegiance { get; set; }
        public string[] StationServices { get; set; }
        public JournalDocked.Economies[] StationEconomyList { get; set; }        // may be null

        public override string SummaryName(ISystem sys)
        {
            return string.Format("Jumped with carrier {0} to {1}".T(EDTx.JournalCarrierJump_JumpedWith), StationName, Body);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Type ".T(EDTx.JournalLocOrJump_Type), StationType, "< in system ".T(EDTx.JournalLocOrJump_insystem), StarSystem);

            detailed = BaseUtils.FieldBuilder.Build("<;(Wanted) ".T(EDTx.JournalLocOrJump_Wanted), Wanted, "Faction:".T(EDTx.JournalLocOrJump_Faction), StationFaction, "Allegiance:".T(EDTx.JournalLocOrJump_Allegiance), Allegiance, "Economy:".T(EDTx.JournalLocOrJump_Economy), Economy_Localised, "Government:".T(EDTx.JournalLocOrJump_Government), Government_Localised, "Security:".T(EDTx.JournalLocOrJump_Security), Security_Localised);

            if (Factions != null)
            {
                foreach (FactionInformation f in Factions)
                {
                    detailed += Environment.NewLine;
                    detailed += BaseUtils.FieldBuilder.Build("", f.Name, "State:".T(EDTx.JournalLocOrJump_State), f.FactionState.SplitCapsWord(), "Government:".T(EDTx.JournalLocOrJump_Government), f.Government,
                        "Inf:;%".T(EDTx.JournalLocOrJump_Inf), (int)(f.Influence * 100), "Allegiance:".T(EDTx.JournalLocOrJump_Allegiance), f.Allegiance, "Happiness:".T(EDTx.JournalLocOrJump_Happiness), f.Happiness_Localised,
                        "Reputation:;%;N1".T(EDTx.JournalLocOrJump_Reputation), f.MyReputation,
                        ";Squadron System".T(EDTx.JournalLocOrJump_SquadronSystem), f.SquadronFaction,
                        ";Happiest System".T(EDTx.JournalLocOrJump_HappiestSystem), f.HappiestSystem,
                        ";Home System".T(EDTx.JournalLocOrJump_HomeSystem), f.HomeSystem
                        );

                    if (f.PendingStates != null)
                    {
                        detailed += BaseUtils.FieldBuilder.Build(",", "Pending State:".T(EDTx.JournalLocOrJump_PendingState));
                        foreach (JournalLocation.PowerStatesInfo state in f.PendingStates)
                            detailed += BaseUtils.FieldBuilder.Build(" ", state.State, "<(;)", state.Trend);

                        if (f.RecoveringStates != null)
                        {
                            detailed += BaseUtils.FieldBuilder.Build(",", "Recovering State:".T(EDTx.JournalLocOrJump_RecoveringState));
                            foreach (JournalLocation.PowerStatesInfo state in f.RecoveringStates)
                                detailed += BaseUtils.FieldBuilder.Build(" ", state.State, "<(;)", state.Trend);
                        }

                        if (f.ActiveStates != null)
                        {
                            detailed += BaseUtils.FieldBuilder.Build(",", "Active State:".T(EDTx.JournalLocOrJump_ActiveState));
                            foreach (JournalLocation.ActiveStatesInfo state in f.ActiveStates)
                                detailed += BaseUtils.FieldBuilder.Build(" ", state.State);
                        }
                    }
                }
            }
        }
    }

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
            Body = evt["Body"].StrNull();
            BodyID = evt["BodyID"].IntNull();
            BodyType = JournalFieldNaming.NormaliseBodyType(evt["BodyType"].Str());

            JToken jm = evt["EDDMapColor"];
            MapColor = jm.Int(EDCommander.Current.MapColour);
            if (jm.Empty())
                evt["EDDMapColor"] = EDCommander.Current.MapColour;      // new entries get this default map colour if its not already there

            EDSMFirstDiscover = evt["EDD_EDSMFirstDiscover"].Bool(false);
        }

        public JournalFSDJump(DateTime utc, ISystem sys, int colour, bool first, bool edsmsynced) : base(utc, sys, JournalTypeEnum.FSDJump, edsmsynced)
        {
            MapColor = colour;
            EDSMFirstDiscover = first;
        }

        public double JumpDist { get; set; }
        public double FuelUsed { get; set; }
        public double FuelLevel { get; set; }
        public bool BoostUsed { get; set; }
        public int BoostValue { get; set; }
        public int MapColor { get; set; }
        public bool RealJournalEvent { get; private set; } // True if real ED 2.2+ journal event and not pre 2.2 imported.
        public bool EDSMFirstDiscover { get; set; }
        public string Body { get; set; }
        public int? BodyID { get; set; }
        public string BodyType { get; set; }

        public override string SummaryName(ISystem sys) { return string.Format("Jump to {0}".T(EDTx.JournalFSDJump_Jumpto), StarSystem); }

        public override void FillInformation(out string info, out string detailed)
        {
            StringBuilder sb = new StringBuilder();
            if (JumpDist > 0)
                sb.Append(JumpDist.ToString("0.00") + " ly");
            if (FuelUsed > 0)
                sb.Append(", Fuel ".T(EDTx.JournalFSDJump_Fuel) + FuelUsed.ToString("0.0") + "t");
            if (FuelLevel > 0)
                sb.Append(" left ".T(EDTx.JournalFSDJump_left) + FuelLevel.ToString("0.0") + "t");

            string econ = Economy_Localised.Alt(Economy);
            if (econ.Equals("None"))
                econ = "";

            sb.Append(" ");
            sb.Append(BaseUtils.FieldBuilder.Build("Faction:".T(EDTx.JournalLocOrJump_Faction), Faction, "<;(Wanted) ".T(EDTx.JournalLocOrJump_Wanted), Wanted,
                                                    "State:".T(EDTx.JournalLocOrJump_State), FactionState, "Allegiance:".T(EDTx.JournalLocOrJump_Allegiance), Allegiance,
                                                    "Economy:".T(EDTx.JournalLocOrJump_Economy), econ, "Population:".T(EDTx.JournalLocOrJump_Population), Population));
            info = sb.ToString();

            sb.Clear();

            if (Factions != null)
            {
                foreach (FactionInformation i in Factions)
                {
                    sb.Append(BaseUtils.FieldBuilder.Build("", i.Name, "State:".T(EDTx.JournalLocOrJump_State), i.FactionState,
                                                                    "Government:".T(EDTx.JournalLocOrJump_Government), i.Government,
                                                                    "Inf:;%".T(EDTx.JournalLocOrJump_Inf), (i.Influence * 100.0).ToString("0.0"),
                                                                    "Allegiance:".T(EDTx.JournalLocOrJump_Allegiance), i.Allegiance,
                                                                    "Happiness:".T(EDTx.JournalLocOrJump_Happiness), i.Happiness_Localised,
                                                                    "Reputation:;%;N1".T(EDTx.JournalLocOrJump_Reputation), i.MyReputation,
                                                                    ";Squadron System".T(EDTx.JournalLocOrJump_SquadronSystem), i.SquadronFaction, 
                                                                    ";Happiest System".T(EDTx.JournalLocOrJump_HappiestSystem), i.HappiestSystem, 
                                                                    ";Home System".T(EDTx.JournalLocOrJump_HomeSystem), i.HomeSystem
                                                                    ));

                    if (i.PendingStates != null)
                    {
                        sb.Append(BaseUtils.FieldBuilder.Build(",", "Pending State:".T(EDTx.JournalLocOrJump_PendingState)));

                        foreach (JournalLocation.PowerStatesInfo state in i.PendingStates)
                            sb.Append(BaseUtils.FieldBuilder.Build(" ", state.State, "<(;)", state.Trend));

                    }

                    if (i.RecoveringStates != null)
                    {
                        sb.Append(BaseUtils.FieldBuilder.Build(",", "Recovering State:".T(EDTx.JournalLocOrJump_RecoveringState)));

                        foreach (JournalLocation.PowerStatesInfo state in i.RecoveringStates)
                            sb.Append(BaseUtils.FieldBuilder.Build(" ", state.State, "<(;)", state.Trend));
                    }

                    if (i.ActiveStates != null)
                    {
                        sb.Append(BaseUtils.FieldBuilder.Build(",", "Active State:".T(EDTx.JournalLocOrJump_ActiveState)));

                        foreach (JournalLocation.ActiveStatesInfo state in i.ActiveStates)
                            sb.Append(BaseUtils.FieldBuilder.Build(" ", state.State));
                    }
                    sb.Append(Environment.NewLine);

                }
            }

            detailed = sb.ToString();
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system)
        {
            shp.FSDJump(this);
        }

        public void SetMapColour(int mapcolour)
        {
            UserDatabase.Instance.ExecuteWithDatabase(cn =>
            {
                JObject jo = GetJson(Id, cn.Connection);

                if (jo != null)
                {
                    jo["EDDMapColor"] = mapcolour;
                    UpdateJsonEntry(jo, cn.Connection);
                    MapColor = mapcolour;
                }
            });
        }

        public void UpdateFirstDiscover(bool value)
        {
            UserDatabase.Instance.ExecuteWithDatabase(cn =>
            {
                UpdateFirstDiscover(value, cn.Connection);
            });
        }

        internal void UpdateFirstDiscover(bool value, SQLiteConnectionUser cn, DbTransaction txnl = null)
        {
            JObject jo = GetJson(Id, cn, txnl);

            if (jo != null)
            {
                jo["EDD_EDSMFirstDiscover"] = value;
                UpdateJsonEntry(jo, cn, txnl);
                EDSMFirstDiscover = value;
            }
        }

        public JObject CreateFSDJournalEntryJson()          // minimal version, not the whole schebang
        {
            JObject jo = new JObject();
            jo["timestamp"] = EventTimeUTC.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'");
            jo["event"] = "FSDJump";
            jo["StarSystem"] = StarSystem;
            jo["StarPos"] = new JArray(StarPos.X, StarPos.Y, StarPos.Z);
            jo["EDDMapColor"] = MapColor;
            jo["EDD_EDSMFirstDiscover"] = EDSMFirstDiscover;
            return jo;
        }
    }


    [JournalEntryType(JournalTypeEnum.FSDTarget)]
    public class JournalFSDTarget : JournalEntry
    {
        public JournalFSDTarget(JObject evt) : base(evt, JournalTypeEnum.FSDTarget)
        {
            StarSystem = evt["Name"].Str();
            StarClass = evt["StarClass"].Str();
            SystemAddress = evt["SystemAddress"].Long();
            RemainingJumpsInRoute = evt["RemainingJumpsInRoute"].IntNull();
        }

        public string StarSystem { get; set; }
        public string StarClass { get; set; }
        public long SystemAddress { get; set; }
        public int? RemainingJumpsInRoute { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", StarSystem,"",StarClass,"Remaining Jumps".T(EDTx.JournalEntry_RemainingJumps), RemainingJumpsInRoute);
            detailed = "";
        }
    }



    [JournalEntryType(JournalTypeEnum.StartJump)]
    public class JournalStartJump : JournalEntry
    {
        public JournalStartJump(JObject evt) : base(evt, JournalTypeEnum.StartJump)
        {
            JumpType = evt["JumpType"].Str();
            IsHyperspace = JumpType.Equals("Hyperspace", System.StringComparison.InvariantCultureIgnoreCase);
            StarSystem = evt["StarSystem"].Str();
            StarClass = evt["StarClass"].Str();
            FriendlyStarClass = (StarClass.Length > 0) ? Bodies.StarName(Bodies.StarStr2Enum(StarClass)) : "";
            SystemAddress = evt["SystemAddress"].LongNull();
        }

        public string JumpType { get; set; }            // Hyperspace, Supercruise
        public bool IsHyperspace { get; set; }
        public string StarSystem { get; set; }
        public long? SystemAddress { get; set; }
        public string StarClass { get; set; }
        public string FriendlyStarClass { get; set; }

        public override string SummaryName(ISystem sys) { return "Charging FSD".T(EDTx.JournalStartJump_ChargingFSD); }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", JumpType, "< to ".T(EDTx.JournalEntry_to), StarSystem, "", FriendlyStarClass);
            detailed = "";
        }
    }

}
