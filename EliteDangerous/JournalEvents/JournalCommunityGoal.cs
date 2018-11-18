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
using System.Globalization;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.CommunityGoal)]
    public class JournalCommunityGoal : JournalEntry
    {
        public class CommunityGoal
        {
            public int CGID { get; set; }
            public string Title { get; set; }
            public string SystemName { get; set; }
            public string MarketName { get; set; }
            public DateTime Expiry { get; set; }
            public bool IsComplete { get; set; }
            public long CurrentTotal { get; set; }
            public long PlayerContribution { get; set; }
            public int NumContributors { get; set; }
            public int PlayerPercentileBand { get; set; }
            public string TopTierName { get; set; }
            public int? TopTierInt { get; set; }
            public string TopTierBonus { get; set; }    

            public int? TopRankSize { get; set; }           // optional
            public bool? PlayerInTopRank { get; set; }      // optional

            public string TierReached { get; set; }         // only when reached first success rank
            public int? TierReachedInt { get; set; }        // its defined as Tier X, this is X
            public long? Bonus { get; set; }            

            public CommunityGoal(JObject jo)
            {
                CGID = jo["CGID"].Int();
                Title = jo["Title"].Str();
                SystemName = jo["SystemName"].Str();
                MarketName = jo["MarketName"].Str();
                Expiry = jo["Expiry"].DateTimeUTC();
                IsComplete = jo["IsComplete"].Bool();
                CurrentTotal = jo["CurrentTotal"].Long();
                PlayerContribution = jo["PlayerContribution"].Long();
                NumContributors = jo["NumContributors"].Int();
                PlayerPercentileBand = jo["PlayerPercentileBand"].Int();

                JObject toptier = (JObject)jo["TopTier"];
                if ( toptier != null )
                {
                    TopTierName = toptier["Name"].Str();
                    TopTierBonus = toptier["Bonus"].Str();
                    if (TopTierName.StartsWith("Tier "))
                        TopTierInt = TopTierName.Substring(5).InvariantParseIntNull();
                }

                TopRankSize = jo["TopRankSize"].IntNull();
                PlayerInTopRank = jo["PlayerInTopRank"].BoolNull();

                TierReached = jo["TierReached"].Str();
                if (TierReached.StartsWith("Tier "))
                    TierReachedInt = TierReached.Substring(5).InvariantParseIntNull();

                Bonus = jo["Bonus"].LongNull();
            }

            public override string ToString()
            {
                BaseUtils.FieldBuilder.NewPrefix nl = new BaseUtils.FieldBuilder.NewPrefix(Environment.NewLine+"  ");

                DateTime exp = Expiry;
                if (exp != null && !EliteConfigInstance.InstanceConfig.DisplayUTC)
                    exp = exp.ToLocalTime();

                return BaseUtils.FieldBuilder.Build(
                     "Title:".Tx(this), Title, "System:".Tx(this), SystemName,                  
                     nl,"At:".Tx(this), MarketName, "Expires:".Tx(this), exp,
                     nl,"Not Complete;Complete".Tx(this), IsComplete,  "Current Total:".Tx(this), CurrentTotal, "Contribution:".Tx(this), PlayerContribution, "Num Contributors:".Tx(this), NumContributors,
                     nl,"Player % Band:".Tx(this), PlayerPercentileBand, "Top Rank:".Tx(this), TopRankSize, "Not In Top Rank;In Top Rank".Tx(this), PlayerInTopRank,
                     nl,"Tier Reached:".Tx(this), TierReached,  "Bonus:".Tx(this), Bonus, "Top Tier Name".Tx(this), TopTierName , "TT. Bonus".Tx(this), TopTierBonus
                      );
            }
        }

        public JournalCommunityGoal(JObject evt ) : base(evt, JournalTypeEnum.CommunityGoal)
        {
            JArray jmodules = (JArray)evt["CurrentGoals"];

            CommunityGoalList = string.Empty;

            if ( jmodules != null )
            {
                CommunityGoals = new List<CommunityGoal>();
                foreach (JObject jo in jmodules)
                {
                    CommunityGoal g = new CommunityGoal(jo);
                    CommunityGoals.Add(g);
                    CommunityGoalList = CommunityGoalList.AppendPrePad(g.Title, ", ");
                }
            }
        }

        
        public List<CommunityGoal> CommunityGoals;
        public string CommunityGoalList;

        public override void FillInformation(out string info, out string detailed) 
        {
            info = CommunityGoalList;
            detailed = "";
            if ( CommunityGoals!=null )
            {
                foreach( CommunityGoal g in CommunityGoals)
                {
                    detailed = detailed.AppendPrePad(g.ToString(), Environment.NewLine + Environment.NewLine);
                }
            }
        }
    }


    [JournalEntryType(JournalTypeEnum.CommunityGoalDiscard)]
    public class JournalCommunityGoalDiscard : JournalEntry
    {
        public JournalCommunityGoalDiscard(JObject evt) : base(evt, JournalTypeEnum.CommunityGoalDiscard)
        {
            CGID = evt["CGID"].Int();
            Name = evt["Name"].Str();
            System = evt["System"].Str();
        }

        public int CGID { get; set; }
        public string Name { get; set; }
        public string System { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Name, "< at ; Star System".Txb(this, "CGS"), System);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CommunityGoalJoin)]
    public class JournalCommunityGoalJoin : JournalEntry
    {
        public JournalCommunityGoalJoin(JObject evt) : base(evt, JournalTypeEnum.CommunityGoalJoin)
        {
            CGID = evt["CGID"].Int();
            Name = evt["Name"].Str();
            System = evt["System"].Str();
        }
        public int CGID { get; set; }
        public string Name { get; set; }
        public string System { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {

            info = BaseUtils.FieldBuilder.Build("", Name, "< at ; Star System".Txb(this, "CGS"), System);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CommunityGoalReward)]
    public class JournalCommunityGoalReward : JournalEntry, ILedgerJournalEntry
    {
        public JournalCommunityGoalReward(JObject evt) : base(evt, JournalTypeEnum.CommunityGoalReward)
        {
            CGID = evt["CGID"].Int();
            Name = evt["Name"].Str();
            System = evt["System"].Str();
            Reward = evt["Reward"].Long();
        }
        public int CGID { get; set; }
        public string Name { get; set; }
        public string System { get; set; }
        public long Reward { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name + " " + System, Reward);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Name, "< at ; Star System".Txb(this, "CGS"), System, "Reward:; cr;N0".Txb(this), Reward);
            detailed = "";
        }
    }

}
