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
using System.Linq;
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
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
            VictimFactionLocalised = JournalFieldNaming.CheckLocalisation(evt["VictimFaction_Localised"].Str(),VictimFaction);

            SharedWithOthers = evt["SharedWithOthers"].Bool(false);
            Rewards = evt["Rewards"]?.ToObjectProtected<BountyReward[]>();

            Target = evt["Target"].StrNull();       // only set for skimmer target missions

            if ( Rewards == null )                  // for skimmers, its Faction/Reward.  Bug in manual reported to FD 23/5/2018
            {
                string faction = evt["Faction"].StrNull();
                long? reward = evt["Reward"].IntNull();

                if (faction != null && reward != null)
                {
                    Rewards = new BountyReward[1];
                    Rewards[0] = new BountyReward() { Faction = faction, Reward = reward.Value };
                    TotalReward = reward.Value;
                }
            }
        }

        public long TotalReward { get; set; }
        public string VictimFaction { get; set; }
        public string VictimFactionLocalised { get; set; }
        public string Target { get; set; }
        public bool SharedWithOthers { get; set; }
        public BountyReward[] Rewards { get; set; }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, string.Format("{0} total {1:N0}".Txb(this,"LegBounty"), VictimFactionLocalised, TotalReward));
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            
            info = BaseUtils.FieldBuilder.Build("; cr;N0", TotalReward, "Target:".Txb(this), (string)Target, "Victim faction:".Txb(this), VictimFactionLocalised);

            detailed = "";
            if ( Rewards!=null)
            {
                foreach (BountyReward r in Rewards)
                {
                    if (detailed.Length > 0)
                        detailed += ", ";

                    detailed += BaseUtils.FieldBuilder.Build("Faction:".Txb(this), r.Faction, "; cr;N0", r.Reward);
                }
            }
        }
    }
}
