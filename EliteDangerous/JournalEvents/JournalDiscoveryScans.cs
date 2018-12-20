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
using System.Collections.Generic;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.DiscoveryScan)]
    public class JournalDiscoveryScan : JournalEntry
    {
        public JournalDiscoveryScan(JObject evt) : base(evt, JournalTypeEnum.DiscoveryScan)
        {
            SystemAddress = evt["SystemAddress"].Long();
            Bodies = evt["Bodies"].Int();
        }

        public long SystemAddress { get; set; }
        public int Bodies { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("New bodies discovered:".Txb(this, "Dscan"), Bodies);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.FSSDiscoveryScan)]
    public class JournalFSSDiscoveryScan : JournalEntry
    {
        public JournalFSSDiscoveryScan(JObject evt) : base(evt, JournalTypeEnum.FSSDiscoveryScan)
        {
            Progress = evt["Progress"].Double() * 100.0;
            BodyCount = evt["BodyCount"].Int();
            NonBodyCount = evt["NonBodyCount"].Int();
        }

        public double Progress { get; set; }
        public int BodyCount { get; set; }
        public int NonBodyCount { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Progress:;%;N1".Tx(this), Progress, "Bodies:", BodyCount, "Others:".Tx(this), NonBodyCount);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.FSSSignalDiscovered)]
    public class JournalFSSSignalDiscovered : JournalEntry
    {
        public class FSSSignal
        {
            public string SignalName { get; set; }
            public string SignalName_Localised { get; set; }
            public string SpawingState { get; set; }
            public string SpawingState_Localised { get; set; }
            public string SpawingFaction { get; set; }
            public string SpawingFaction_Localised { get; set; }
            public double? TimeRemaining { get; set; }          // null if not expiring
            public long? SystemAddress { get; set; }

            public int? ThreatLevel { get; set; }
            public string USSType { get; set; }
            public string USSTypeLocalised { get; set; }
            public bool? IsStation { get; set; }

            public System.DateTime ExpiryUTC { get; set; }
            public System.DateTime ExpiryLocal { get; set; }

            public FSSSignal(JObject evt , System.DateTime EventTimeUTC)
            {
                SignalName = evt["SignalName"].Str();
                SignalName_Localised = JournalFieldNaming.CheckLocalisation(evt["SignalName_Localised"].Str(), SignalName);

                SpawingState = evt["SpawingState"].Str();
                SpawingState_Localised = JournalFieldNaming.CheckLocalisation(evt["SpawingState_Localised"].Str(), SpawingState);

                SpawingFaction = evt["SpawingFaction"].Str();
                SpawingFaction_Localised = JournalFieldNaming.CheckLocalisation(evt["SpawingFaction_Localised"].Str(), SpawingFaction);

                TimeRemaining = evt["TimeRemaining"].DoubleNull();

                SystemAddress = evt["SystemAddress"].LongNull();

                ThreatLevel = evt["USSThreat"].IntNull();
                USSType = evt["USSType"].Str();
                USSTypeLocalised = JournalFieldNaming.CheckLocalisation(evt["USSType_Localised"].Str(), USSType);
                IsStation = evt["IsStation"].BoolNull();

                if (TimeRemaining != null)
                {
                    ExpiryUTC = EventTimeUTC.AddSeconds(TimeRemaining.Value);
                    ExpiryLocal = ExpiryUTC.ToLocalTime();
                }
            }

            public override string ToString()
            {
                return BaseUtils.FieldBuilder.Build("", SignalName_Localised, "State:".Tx(this),
                            SpawingState_Localised, "Faction:".Tx(this), SpawingFaction_Localised,
                            "USS Type:".Tx(this), USSTypeLocalised, " Threat Level:".Tx(this), ThreatLevel,
                            ";Station".Tx(this, "StationBool"), IsStation
                            );
            }
        }

        public JournalFSSSignalDiscovered(JObject evt) : base(evt, JournalTypeEnum.FSSSignalDiscovered)
        {
            Signals = new List<FSSSignal>();
            Signals.Add(new FSSSignal(evt, EventTimeUTC));
        }

        public void Add(JournalFSSSignalDiscovered next )
        {
            Signals.Add(next.Signals[0]);
        }

        public List<FSSSignal> Signals;

        public override void FillInformation(out string info, out string detailed)
        {
            detailed = "";

            if (Signals.Count > 1)
            {
                info = BaseUtils.FieldBuilder.Build("Detected ; signals".Tx(this), Signals.Count);

                foreach (var s in Signals)
                    info += ", " + s.SignalName_Localised;

                foreach (var s in Signals)
                    detailed = detailed.AppendPrePad(s.ToString(),System.Environment.NewLine);
            }
            else
            {
                info = Signals[0].ToString();
            }
        }
    }

   
    [JournalEntryType(JournalTypeEnum.NavBeaconScan)]
    public class JournalNavBeaconScan : JournalEntry
    {
        public JournalNavBeaconScan(JObject evt) : base(evt, JournalTypeEnum.NavBeaconScan)
        {
            NumBodies = evt["NumBodies"].Int();
            SystemAddress = evt["SystemAddress"].LongNull();
        }

        public int NumBodies { get; set; }
        public long? SystemAddress { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Bodies:".Txb(this), NumBodies);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.SAAScanComplete)]
    public class JournalSAAScanComplete : JournalEntry
    {
        public JournalSAAScanComplete(JObject evt) : base(evt, JournalTypeEnum.SAAScanComplete)
        {
            BodyName = evt["BodyName"].Str();
            BodyID = evt["BodyID"].Long();
            ProbesUsed = evt["ProbesUsed"].Int();
            EfficiencyTarget = evt["EfficiencyTarget"].Int();
        }

        public long BodyID { get; set; }
        public string BodyName { get; set; }
        public int ProbesUsed { get; set; }
        public int EfficiencyTarget { get; set; }
        public string BodyDesignation { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", BodyName,
                                                "Probes:".Tx(this), ProbesUsed,
                                                "Efficiency Target:".Tx(this), EfficiencyTarget);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.FSSAllBodiesFound)]
    public class JournalFSSAllBodiesFound : JournalEntry
    {
        public JournalFSSAllBodiesFound(JObject evt) : base(evt, JournalTypeEnum.FSSAllBodiesFound)
        {
            SystemName = evt["SystemName"].Str();
            SystemAddress = evt["SystemAddress"].Long();
        }

        public long SystemAddress { get; set; }
        public string SystemName { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", SystemName);
            detailed = "";
        }
    }


}
