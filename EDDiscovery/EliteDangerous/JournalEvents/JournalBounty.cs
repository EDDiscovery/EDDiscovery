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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: player is awarded a bounty for a kill
    //Parameters:
    //•	Faction: the faction awarding the bounty
    //•	Reward: the reward value
    //•	VictimFaction: the victim’s faction
    //•	SharedWithOthers: whether shared with other players
    public class JournalBounty : JournalEntry
    {
        public JournalBounty(JObject evt ) : base(evt, JournalTypeEnum.Bounty)
        {
            Reward = JSONHelper.GetLong(evt["Reward"]);     // some of them..
            TotalReward = JSONHelper.GetLong(evt["TotalReward"]);     // others of them..

            VictimFaction = JSONHelper.GetStringDef(evt["VictimFaction"]);
            VictimFactionLocalised = JSONHelper.GetStringDef(evt["VictimFaction_Localised"]); // may not be present

            SharedWithOthers = JSONHelper.GetBool(evt["SharedWithOthers"],false);
            Rewards = evt["Rewards"]?.ToObject<BountyReward[]>();
        }

        public long Reward { get; set; }     
        public long TotalReward { get; set; }
        public string VictimFaction { get; set; }
        public string VictimFactionLocalised { get; set; }
        public bool SharedWithOthers { get; set; }
        public BountyReward[] Rewards { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.bounty; } }

        public void LedgerNC(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            string n = (VictimFactionLocalised.Length > 0) ? VictimFactionLocalised : VictimFaction;
            n += " total " + (TotalReward + Reward).ToString("N0");

            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, n);
        }

    }

    public class BountyReward
    {
        public string Faction;
        public long Reward;
    }

}
