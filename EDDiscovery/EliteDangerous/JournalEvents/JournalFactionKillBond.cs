﻿/*
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
    //When written: Player rewarded for taking part in a combat zone
    //Parameters:
    //•	Reward
    //•	AwardingFaction
    //•	VictimFaction
    [JournalEntryType(JournalTypeEnum.FactionKillBond)]
    public class JournalFactionKillBond : JournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalFactionKillBond(JObject evt ) : base(evt, JournalTypeEnum.FactionKillBond)
        {
            AwardingFaction = evt["AwardingFaction"].Str();
            VictimFaction = evt["VictimFaction"].Str();
            Reward = evt["Reward"].Long();
        }
        public string AwardingFaction { get; set; }
        public string VictimFaction { get; set; }
        public long Reward { get; set; }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, AwardingFaction + " " + Reward.ToString("N0"));
        }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.factionkillbond; } }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("Reward:", Reward, "<from " , AwardingFaction , "<, due to " , VictimFaction );
            detailed = "";
        }
    }
}
