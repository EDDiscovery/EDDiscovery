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

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.PayBounties)]
    public class JournalPayBounties : JournalEntry, ILedgerJournalEntry
    {
        public JournalPayBounties(JObject evt) : base(evt, JournalTypeEnum.PayBounties)
        {
            Amount = evt["Amount"].Long();
            BrokerPercentage = evt["BrokerPercentage"].Double();
            AllFines = evt["AllFines"].Bool();
            Faction = evt["Faction"].Str();
            Faction_Localised = evt["Faction_Localised"].Str().Alt(Faction);
            ShipId = evt["ShipID"].Int();
        }

        public long Amount { get; set; }
        public double BrokerPercentage { get; set; }
        public bool AllFines { get; set; }
        public string Faction { get; set; }      // may be blank
        public string Faction_Localised { get; set; }    // may be blank
        public int ShipId { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, (Faction_Localised.Length > 0 ? "Faction " + Faction_Localised : "") + " Broker " + BrokerPercentage.ToString("0.0") + "%", -Amount);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("Cost:; cr;N0", Amount, "< To ", Faction_Localised);
            if (BrokerPercentage > 0)
                info += ", Broker took " + BrokerPercentage.ToString("0") + "%";
            detailed = "";
        }
    }
}
