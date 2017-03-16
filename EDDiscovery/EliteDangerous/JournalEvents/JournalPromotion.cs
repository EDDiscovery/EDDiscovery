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
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Promotion)]
    public class JournalPromotion : JournalEntry
    {
        //        When written: when the player’s rank increases
        //Parameters: one of the following
        //•	Combat: new rank
        //•	Trade: new rank
        //•	Explore: new rank
        //•	CQC: new rank

        public JournalPromotion(JObject evt) : base(evt, JournalTypeEnum.Promotion)
        {
            int? c = JSONHelper.GetIntNull(evt["Combat"]);
            if (c.HasValue)
                Combat = (CombatRank)c.Value;

            int? t = JSONHelper.GetIntNull(evt["Trade"]);
            if ( t.HasValue)
                Trade = (TradeRank)t;

            int? e = JSONHelper.GetIntNull(evt["Explore"]);
            if (e.HasValue)
                Explore = (ExplorationRank)e;

            int? q = JSONHelper.GetIntNull(evt["CQC"]);
            if ( q.HasValue)
                CQC = (CQCRank)q;

            int? f = JSONHelper.GetIntNull(evt["Federation"]);
            if ( f.HasValue)
                Federation = (FederationRank)f;

            int? evilempire = JSONHelper.GetIntNull(evt["Empire"]);
            if ( evilempire.HasValue)
                Empire = (EmpireRank)evilempire;
        }

        public CombatRank? Combat { get; set; }
        public TradeRank? Trade { get; set; }
        public ExplorationRank? Explore { get; set; }
        public CQCRank? CQC { get; set; }
        public FederationRank? Federation { get; set; }
        public EmpireRank? Empire { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.promotion; } }

        public override void FillInformation(out string summary, out string info, out string detailed)  //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("Combat:", Combat.HasValue ? Combat.ToString() : null,
                                      "Trade:", Trade.HasValue ? Trade.ToString() : null,
                                      "Exploration:", Explore.HasValue ? Explore.ToString() : null,
                                      "Empire:", Empire.HasValue ? Empire.ToString() : null,
                                      "Federation:", Federation.HasValue ? Federation.ToString() : null,
                                      "CQC:", CQC.HasValue ? CQC.ToString() : null);
            detailed = "";
        }
    }
}
