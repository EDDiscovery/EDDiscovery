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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Rank)]
    public class JournalRank : JournalEntry
    {
        public JournalRank(JObject evt ) : base(evt, JournalTypeEnum.Rank)
        {
            Combat = (CombatRank)evt["Combat"].Int();
            Trade = (TradeRank)evt["Trade"].Int();
            Explore = (ExplorationRank)evt["Explore"].Int();
            Empire = (EmpireRank)evt["Empire"].Int();
            Federation = (FederationRank)evt["Federation"].Int();
            CQC = (CQCRank)evt["CQC"].Int();
        }

        public CombatRank Combat { get; set; }
        public TradeRank Trade { get; set; }
        public ExplorationRank Explore { get; set; }
        public EmpireRank Empire { get; set; }
        public FederationRank Federation { get; set; }
        public CQCRank CQC { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("", Combat.ToString().SplitCapsWord(),
                                      "", Trade.ToString().SplitCapsWord(),
                                      "", Explore.ToString().SplitCapsWord(),
                                      "", Federation.ToString().SplitCapsWord(),
                                      "", Empire.ToString().SplitCapsWord(),
                                      "", CQC.ToString().SplitCapsWord());
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.Promotion)]
    public class JournalPromotion : JournalEntry
    {
        public JournalPromotion(JObject evt) : base(evt, JournalTypeEnum.Promotion)
        {
            int? c = evt["Combat"].IntNull();
            if (c.HasValue)
                Combat = (CombatRank)c.Value;

            int? t = evt["Trade"].IntNull();
            if (t.HasValue)
                Trade = (TradeRank)t;

            int? e = evt["Explore"].IntNull();
            if (e.HasValue)
                Explore = (ExplorationRank)e;

            int? q = evt["CQC"].IntNull();
            if (q.HasValue)
                CQC = (CQCRank)q;

            int? f = evt["Federation"].IntNull();
            if (f.HasValue)
                Federation = (FederationRank)f;

            int? evilempire = evt["Empire"].IntNull();
            if (evilempire.HasValue)
                Empire = (EmpireRank)evilempire;
        }

        public CombatRank? Combat { get; set; }
        public TradeRank? Trade { get; set; }
        public ExplorationRank? Explore { get; set; }
        public CQCRank? CQC { get; set; }
        public FederationRank? Federation { get; set; }
        public EmpireRank? Empire { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Combat:".Tx(this), Combat.HasValue ? Combat.ToString() : null,
                                      "Trade:".Tx(this), Trade.HasValue ? Trade.ToString() : null,
                                      "Exploration:".Tx(this), Explore.HasValue ? Explore.ToString() : null,
                                      "Empire:".Tx(this), Empire.HasValue ? Empire.ToString() : null,
                                      "Federation:".Tx(this), Federation.HasValue ? Federation.ToString() : null,
                                      "CQC:".Tx(this), CQC.HasValue ? CQC.ToString() : null);
            detailed = "";
        }
    }

}
