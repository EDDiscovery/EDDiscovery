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
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    //When written: player is awarded a bounty for a kill
    //Parameters:
    //•	Faction: the faction awarding the bounty
    //•	Reward: the reward value
    //•	VictimFaction: the victim’s faction
    //•	SharedWithOthers: whether shared with other players
    [JournalEntryType(JournalTypeEnum.Bounty)]
    public class JournalBounty : JournalEntry, ILedgerNoCashJournalEntry
    {
        public class BountyReward
        {
            public string Faction;
            public long Reward;
        }

        public JournalBounty(JObject evt) : base(evt, JournalTypeEnum.Bounty)
        {
            TotalReward = evt["TotalReward"].Long();     // others of them..

            VictimFaction = evt["VictimFaction"].Str();
            VictimFactionLocalised = evt["VictimFaction_Localised"].Str().Alt(VictimFaction);

            SharedWithOthers = evt["SharedWithOthers"].Bool(false);
            Rewards = evt["Rewards"]?.ToObjectProtected<BountyReward[]>();
        }

        public long TotalReward { get; set; }
        public string VictimFaction { get; set; }
        public string VictimFactionLocalised { get; set; }
        public string Target { get; set; }
        public bool SharedWithOthers { get; set; }
        public BountyReward[] Rewards { get; set; }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            string n = (VictimFactionLocalised.Length > 0) ? VictimFactionLocalised : VictimFaction;
            n += " total " + TotalReward.ToString("N0");

            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, n);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("; cr;N0", TotalReward, "Target:", (string)Target, "Victim faction:", VictimFactionLocalised);

            detailed = "";
            if ( Rewards!=null)
            {
                foreach (BountyReward r in Rewards)
                {
                    if (detailed.Length > 0)
                        detailed += ", ";

                    detailed += BaseUtils.FieldBuilder.Build("Faction:", r.Faction, "; cr;N0", r.Reward);
                }
            }
        }
    }
}
