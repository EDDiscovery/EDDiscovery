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
using System.Text;

namespace EliteDangerousCore.JournalEvents
{
    //    When written: The player has been rewarded for a capital ship combat
    //Parameters:
    //•	Reward: value of award
    //•	AwardingFaction
    //•	VictimFaction
    [JournalEntryType(JournalTypeEnum.CapShipBond)]
    public class JournalCapShipBond : JournalEntry, ILedgerNoCashJournalEntry
    {
        public JournalCapShipBond(JObject evt) : base(evt, JournalTypeEnum.CapShipBond)
        {
            AwardingFaction = evt["AwardingFaction"].Str();
            AwardingFaction_Localised = evt["AwardingFaction_Localised"].Str();
            VictimFaction = evt["VictimFaction"].Str();
            VictimFaction_Localised = evt["VictimFaction_Localised"].Str();
            Reward = evt["Reward"].Long();
        }

        public string AwardingFaction { get; set; }
        public string AwardingFaction_Localised { get; set; }       // may be empty
        public string VictimFaction { get; set; }
        public string VictimFaction_Localised { get; set; }         // may be empty

        public long Reward { get; set; }

        public void LedgerNC(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, AwardingFaction_Localised.Alt(AwardingFaction) + " " + Reward);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("; cr;N0", Reward, "< from ", AwardingFaction_Localised.Alt(AwardingFaction),
                "< , due to ", VictimFaction_Localised.Alt(VictimFaction));
            detailed = "";
        }
    }
}
