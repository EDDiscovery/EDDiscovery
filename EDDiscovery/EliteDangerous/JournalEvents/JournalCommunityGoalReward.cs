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
    //When Written: when receiving a reward for a community goal
    //Parameters:
    //•	Name
    //•	System
    //•	Reward
    [JournalEntryType(JournalTypeEnum.CommunityGoalReward)]
    public class JournalCommunityGoalReward : JournalEntry, ILedgerJournalEntry
    {
        public JournalCommunityGoalReward(JObject evt ) : base(evt, JournalTypeEnum.CommunityGoalReward)
        {
            Name = evt["Name"].Str();
            System = evt["System"].Str();
            Reward = evt["Reward"].Long();
        }
        public string Name { get; set; }
        public string System { get; set; }
        public long Reward { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name + " " + System, Reward);
        }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.communitygoalreward; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("", Name, "<at ; Star System", System, "Reward ; credits", Reward);
            detailed = "";
        }
    }
}
