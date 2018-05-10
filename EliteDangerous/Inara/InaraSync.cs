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

using EliteDangerousCore.JournalEvents;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore.Inara
{
    public static class InaraSync
    {
        public static bool Refresh(Action<string> logger, HistoryList history)
        {
            return true;
            HistoryEntry last = history.GetLast;
            if (last != null)
            {
                List<JToken> eventstosend = new List<JToken>();

                JournalStatistics stats = history.GetLastJournalEntry<JournalStatistics>(x => x.EntryType == JournalTypeEnum.Statistics);
                if (stats != null)
                    eventstosend.Add(InaraClass.setCommanderGameStatistics(stats.Json, stats.EventTimeUTC));

                JournalRank rank = history.GetLastJournalEntry<JournalRank>(x => x.EntryType == JournalTypeEnum.Rank);
                JournalProgress progress = history.GetLastJournalEntry<JournalProgress>(x => x.EntryType == JournalTypeEnum.Progress);
                if (rank != null )
                {
                    eventstosend.Add(InaraClass.setCommanderRankPilot("Combat", (int)rank.Combat, progress?.Combat ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("Trade", (int)rank.Trade, progress?.Trade ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("Explore", (int)rank.Explore, progress?.Explore ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("Empire", (int)rank.Empire, progress?.Empire ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("Federation", (int)rank.Federation, progress?.Federation ?? -1, rank.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderRankPilot("CQC", (int)rank.CQC, progress?.Combat ?? -1, rank.EventTimeUTC));
                }

                JournalPowerplay power = history.GetLastJournalEntry<JournalPowerplay>(x => x.EntryType == JournalTypeEnum.Powerplay);
                if (power != null)
                    eventstosend.Add(InaraClass.setCommanderRankPower(power.Power, power.Rank, power.EventTimeUTC));

                JournalReputation reputation = history.GetLastJournalEntry<JournalReputation>(x => x.EntryType == JournalTypeEnum.Reputation);
                if (reputation != null)
                {
                    eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("Federation", reputation.Federation.HasValue ? reputation.Federation.Value : 0, reputation.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("Empire", reputation.Empire.HasValue ? reputation.Empire.Value : 0, reputation.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("Independent", reputation.Independent.HasValue ? reputation.Independent.Value : 0, reputation.EventTimeUTC));
                    eventstosend.Add(InaraClass.setCommanderReputationMajorFaction("Alliance", reputation.Alliance.HasValue ? reputation.Alliance.Value : 0, reputation.EventTimeUTC));
                }

                eventstosend.Add(InaraClass.setCommanderCredits(last.Credits, last.EventTimeUTC));

                InaraClass inara = new InaraClass();
                inara.Send(eventstosend);
            }

            return true;
        }


        public static bool NewEvent(Action<string> log, HistoryEntry he)
        {
            List<JToken> eventstosend = new List<JToken>();

            switch( he.journalEntry.EventTypeID)
            {
                case JournalTypeEnum.MissionCompleted:
                    {
                        JournalMissionCompleted je = he.journalEntry as JournalMissionCompleted;
                        if (je.PermitsAwarded != null)
                        {
                            foreach (string s in je.PermitsAwarded)
                                eventstosend.Add(InaraClass.addCommanderPermit(s, je.EventTimeUTC));
                        }
                        break;
                    }
                case JournalTypeEnum.EngineerProgress:
                    {
                        JournalEngineerProgress je = he.journalEntry as JournalEngineerProgress;
                        eventstosend.Add(InaraClass.setCommanderRankEngineer(je.Engineer, je.Progress, je.Rank.HasValue ? je.Rank.Value : 1, je.EventTimeUTC));
                        break;
                    }
            }

            return true;
        }
    }
}

