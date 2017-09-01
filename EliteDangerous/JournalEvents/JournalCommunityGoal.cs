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
            public int PlayerContribution { get; set; }
            public int NumContributors { get; set; }
            public int PlayerPercentileBand { get; set; }
            public int TopRankSize { get; set; }
            public int PlayerInTopRank { get; set; }
            public int TierReached { get; set; }
            public int Bonus { get; set; }

            public CommunityGoal(JObject jo)
            {
                CGID = jo["CGID"].Int();
                Title = jo["Title"].Str();
                SystemName = jo["SystemName"].Str();
                MarketName = jo["MarketName"].Str();

//Expiry: time and date
//IsComplete: Boolean
//CurrentTotal
//PlayerContribution
//NumContributors
//PlayerPercentileBand

//                    //If the community goal is constructed with a fixed-size top rank(ie max reward for top 10 players)
//                    TopRankSize: (integer)
//                    PlayerInTopRank: (Boolean)
//                    //If the community goal has reached the first success tier:
//                 TierReached
//        Bonus

            }
        }



        public JournalCommunityGoal(JObject evt ) : base(evt, JournalTypeEnum.CommunityGoal)
        {
            //Name = evt["Name"].Str();
            //System = evt["System"].Str();


            //CommunityGoal = new List<CommunityGoal>();

            //JArray jmodules = (JArray)evt["Modules"];
            //if (jmodules != null)       // paranoia
            //{
            //    foreach (JObject jo in jmodules)
            //    {
            //        CommunityGoal module = new CommunityGoal(JournalFieldNaming.GetBetterSlotName(jo["Slot"].Str()),
            //                                            JournalFieldNaming.NormaliseFDSlotName(jo["Slot"].Str()),
            //                                            JournalFieldNaming.GetBetterItemNameLoadout(jo["Item"].Str()),
            //                                            JournalFieldNaming.NormaliseFDItemName(jo["Item"].Str()),
            //                                            jo["On"].BoolNull(),
            //                                            jo["Priority"].IntNull(),
            //                                            jo["AmmoInClip"].IntNull(),
            //                                            jo["AmmoInHopper"].IntNull(),
            //                                            jo["EngineerBlueprint"].Str().SplitCapsWordFull(),
            //                                            jo["EngineerLevel"].IntNull(),
            //                                            jo["Health"].DoubleNull(),
            //                                            jo["Value"].IntNull());
            //        CommunityGoals.Add(module);
            //    }

            
            }

        
        //public List<CommunityGoal> CommunityGoals;

        public override System.Drawing.Bitmap Icon { get { return EliteDangerous.Properties.Resources.communitygoal; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";// BaseUtils.FieldBuilder.Build("", Name, "< at ; Star System", System);
            detailed = "";
        }
    }
}
