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
using System.Globalization;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{

    /*
    When written: when checking the status of a community goal
   This event contains the current status of all community goals the player is currently subscribed to
   Parameters:
    CurrentGoals: an array with an entry for each CG, containing:
   o CGID: a unique ID number for this CG
   o Title: the description of the CG
   o SystemName
   o MarketName
   o Expiry: time and date
   o IsComplete: Boolean
   o CurrentTotal
   o PlayerContribution
   o NumContributors
   o PlayerPercentileBand
   If the community goal is constructed with a fixed-size top rank (ie max reward for top 10 players)
   o TopRankSize: (integer)
   o PlayerInTopRank: (Boolean)
   If the community goal has reached the first success tier:
   o TierReached
   o Bonus
   Example:
   { "timestamp":"2017-08-14T13:20:28Z", "event":"CommunityGoal", "CurrentGoals":[ { "CGID":726,
   "Title":"Alliance Research Initiative – Trade", "SystemName":"Kaushpoos", "MarketName":"Neville
   Horizons", "Expiry":"2017-08-17T14:58:14Z", "IsComplete":false, "CurrentTotal":10062,
   "PlayerContribution":562, "NumContributors":101, "TopRankSize":10, "PlayerInTopRank":false,
   "TierReached":"Tier 1", "PlayerPercentileBand":50, "Bonus":200000 } ] } 
       */
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
            // TBD TopTier in 3.0 journal.. need to see it
            public int TopRankSize { get; set; }
            public bool PlayerInTopRank { get; set; }
            public string TierReached { get; set; }
            public long Bonus { get; set; }

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
                TopRankSize = jo["TopRankSize"].Int();
                PlayerInTopRank = jo["PlayerInTopRank"].Bool();
                TierReached = jo["TierReached"].Str();
                Bonus = jo["Bonus"].Long();
            }

            public override string ToString()
            {
                BaseUtils.FieldBuilder.NewPrefix nl = new BaseUtils.FieldBuilder.NewPrefix(Environment.NewLine+"  ");

                DateTime exp = Expiry;
                if (exp != null && !EliteConfigInstance.InstanceConfig.DisplayUTC)
                    exp = exp.ToLocalTime();

                return BaseUtils.FieldBuilder.Build(
                     "Title:", Title, "System:", SystemName,
                     nl,"At:", MarketName, "Expires:", exp,
                     nl,"Not Complete;Complete", IsComplete,  "Current Total:" , CurrentTotal, "Contribution:", PlayerContribution, "Num Contributors:", NumContributors,
                     nl,"Player % Band:", PlayerPercentileBand, "Top Rank:", TopRankSize, "Not In Top Rank;In Top Rank", PlayerInTopRank,
                     nl,"Tier Reached:", TierReached,  "Bonus:" , Bonus
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

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
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
}
