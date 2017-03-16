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
    //    When Written: when claiming payment for combat bounties and bonds
    //Parameters:
    //•	Type
    //•	Amount: (Net amount received, after any broker fee)
    //•	Faction: name of faction
    //•	BrokerPercenentage
    [JournalEntryType(JournalTypeEnum.RedeemVoucher)]
    public class JournalRedeemVoucher : JournalEntry, ILedgerJournalEntry
    {
        public JournalRedeemVoucher(JObject evt) : base(evt, JournalTypeEnum.RedeemVoucher)
        {
            Type = evt["Type"].Str().SplitCapsWordFull();
            Amount = evt["Amount"].Long();
            Faction = evt["Faction"].Str();
            BrokerPercentage = evt["BrokerPercentage"].Double();
        }
        public string Type { get; set; }
        public long Amount { get; set; }
        public string Faction { get; set; }
        public double BrokerPercentage { get; set; }

        public override System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.bounty; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Type + " Broker " + BrokerPercentage.ToString("0.0") + "%", Amount);
        }

        public override void FillInformation(out string summary, out string info, out string detailed)      //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = Tools.FieldBuilder("Type:" , Type , "Amount:; credits", Amount, "Faction:" , Faction);
            if (BrokerPercentage > 0)
                info += ", Broker took " + BrokerPercentage.ToString("0") + "%";
            detailed = "";
        }
    }
}


