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
            info = BaseUtils.FieldBuilder.Build("New bodies discovered:".T(EDTx.JournalEntry_Dscan), Bodies);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.FSSDiscoveryScan)]
    public class JournalFSSDiscoveryScan : JournalEntry, IScanDataChanges
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
            info = BaseUtils.FieldBuilder.Build("Progress:;%;N1".T(EDTx.JournalFSSDiscoveryScan_Progress), Progress, "Bodies:", BodyCount, "Others:".T(EDTx.JournalFSSDiscoveryScan_Others), NonBodyCount);
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
            public string SpawingState { get; set; }            // keep the typo - its in the voice pack
            public string SpawingState_Localised { get; set; }
            public string SpawingFaction { get; set; }          // keep the typo - its in the voice pack
            public string SpawingFaction_Localised { get; set; }
            public double? TimeRemaining { get; set; }          // null if not expiring
            public long? SystemAddress { get; set; }

            public int? ThreatLevel { get; set; }
            public string USSType { get; set; }
            public string USSTypeLocalised { get; set; }
            public bool? IsStation { get; set; }

            public System.DateTime ExpiryUTC { get; set; }
            public System.DateTime ExpiryLocal { get; set; }

            public FSSSignal(JObject evt, System.DateTime EventTimeUTC)
            {
                SignalName = evt["SignalName"].Str();
                SignalName_Localised = JournalFieldNaming.CheckLocalisation(evt["SignalName_Localised"].Str(), SignalName);

                SpawingState = evt["SpawningState"].Str();
                SpawingState_Localised = JournalFieldNaming.CheckLocalisation(evt["SpawningState_Localised"].Str(), SpawingState);

                SpawingFaction = evt["SpawningFaction"].Str();
                SpawingFaction_Localised = JournalFieldNaming.CheckLocalisation(evt["SpawningFaction_Localised"].Str(), SpawingFaction);

                TimeRemaining = evt["TimeRemaining"].DoubleNull();

                SystemAddress = evt["SystemAddress"].LongNull();

                ThreatLevel = evt["ThreatLevel"].IntNull();
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
                if (SignalName.StartsWith("$USS_"))
                {
                    return BaseUtils.FieldBuilder.Build("", USSTypeLocalised, 
                                "Threat Level:".T(EDTx.FSSSignal_ThreatLevel), ThreatLevel,
                                "Faction:".T(EDTx.FSSSignal_Faction), SpawingFaction_Localised,
                                ";Station".T(EDTx.FSSSignal_StationBool), IsStation,
                                "State:".T(EDTx.FSSSignal_State), SpawingState_Localised
                                );
                }
                else
                {
                    return BaseUtils.FieldBuilder.Build("", SignalName_Localised, 
                                "USS Type:".T(EDTx.FSSSignal_USSType), USSTypeLocalised, 
                                "Threat Level:".T(EDTx.FSSSignal_ThreatLevel), ThreatLevel,
                                "Faction:".T(EDTx.FSSSignal_Faction), SpawingFaction_Localised,
                                ";Station".T(EDTx.FSSSignal_StationBool), IsStation,
                                "State:".T(EDTx.FSSSignal_State), SpawingState_Localised
                                );
                }
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
                info = BaseUtils.FieldBuilder.Build("Detected ; signals".T(EDTx.JournalFSSSignalDiscovered_Detected), Signals.Count);

                foreach (var s in Signals)
                {
                    if (s.SignalName.StartsWith("$USS_"))
                        info += ", " + s.USSTypeLocalised;
                    else
                        info += ", " + s.SignalName_Localised;
                }

                foreach (var s in Signals)
                    detailed = detailed.AppendPrePad(s.ToString(), System.Environment.NewLine);
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
            info = BaseUtils.FieldBuilder.Build("Bodies:".T(EDTx.JournalEntry_Bodies), NumBodies);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.SAAScanComplete)]
    public class JournalSAAScanComplete : JournalEntry, IScanDataChanges
    {
        public JournalSAAScanComplete(JObject evt) : base(evt, JournalTypeEnum.SAAScanComplete)
        {
            BodyName = evt["BodyName"].Str();
            BodyID = evt["BodyID"].Long();
            ProbesUsed = evt["ProbesUsed"].Int();
            EfficiencyTarget = evt["EfficiencyTarget"].Int();
            SystemAddress = evt["SystemAddress"].LongNull();
        }

        public long BodyID { get; set; }
        public string BodyName { get; set; }
        public int ProbesUsed { get; set; }
        public int EfficiencyTarget { get; set; }
        public long? SystemAddress { get; set; }    // 3.5

        public string BodyDesignation { get; set; }     // set by scan system to best body designation for this entry

        public override string SummaryName(ISystem sys)
        {
            return base.SummaryName(sys) + " " + "of ".T(EDTx.JournalEntry_ofa) + BodyName.ReplaceIfStartsWith(sys.Name);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Probes:".T(EDTx.JournalSAAScanComplete_Probes), ProbesUsed,
                                                "Efficiency Target:".T(EDTx.JournalSAAScanComplete_EfficiencyTarget), EfficiencyTarget);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.SAASignalsFound)]
    public class JournalSAASignalsFound : JournalEntry, IScanDataChanges
    {
        public JournalSAASignalsFound(JObject evt) : base(evt, JournalTypeEnum.SAASignalsFound)
        {
            SystemAddress = evt["SystemAddress"].Long();
            BodyName = evt["BodyName"].Str();
            BodyID = evt["BodyID"].Int();
            Signals = evt["Signals"].ToObjectProtected<List<SAASignal>>();
            if ( Signals != null )
            {
                foreach (var s in Signals)      // some don't have localisation
                {
                    s.Type_Localised = JournalFieldNaming.CheckLocalisation(s.Type_Localised, s.Type.SplitCapsWordFull());
                }
            }
        }

        public long SystemAddress { get; set; }
        public string BodyName { get; set; }
        public int BodyID { get; set; }
        public List<SAASignal> Signals { get; set; }

        public string BodyDesignation { get; set; }     // set by scan system to best body designation for this entry

        public class SAASignal 
        {
            public string Type { get; set; }        // material fdname, or $SAA_SignalType..
            public string Type_Localised { get; set; }
            public int Count { get; set; }
        }
      
        public override string SummaryName(ISystem sys)
        {
            return base.SummaryName(sys) + " " + "of ".T(EDTx.JournalEntry_ofa) + BodyName.ReplaceIfStartsWith(sys.Name);
        }

        static public string SignalList(List<SAASignal> list, int indent = 0, string separ = ", " , bool logtype = false)
        {
            string inds = new string(' ', indent);

            string info = "";
            if (list != null)
            {
                foreach (var x in list)
                {
                    info = info.AppendPrePad(inds + (logtype ? x.Type : x.Type_Localised.Alt(x.Type)) + ":" + x.Count.ToString("N0"), separ);
                }
            }
            return info;
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = SignalList(Signals);
            detailed = "";
        }

        public int Contains(string fdname)      // give count if contains fdname, else zero
        {
            int index = Signals?.FindIndex((x) => x.Type.Equals(fdname, System.StringComparison.InvariantCultureIgnoreCase)) ?? -1;
            return (index >= 0) ? Signals[index].Count : 0;
        }

        public string ContainsStr(string fdname, bool showit = true)      // give count if contains fdname, else empty string
        {
            int contains = Contains(fdname);
            return showit && contains > 0 ? contains.ToStringInvariant() : "";
        }
    }

    [JournalEntryType(JournalTypeEnum.FSSAllBodiesFound)]
    public class JournalFSSAllBodiesFound : JournalEntry
    {
        public JournalFSSAllBodiesFound(JObject evt) : base(evt, JournalTypeEnum.FSSAllBodiesFound)
        {
            SystemName = evt["SystemName"].Str();
            SystemAddress = evt["SystemAddress"].Long();
            Count = evt["Count"].Int();
        }

        public long SystemAddress { get; set; }
        public string SystemName { get; set; }
        public int Count { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = Count.ToString() + " @ " + SystemName;
            detailed = "";
        }
    }


}
