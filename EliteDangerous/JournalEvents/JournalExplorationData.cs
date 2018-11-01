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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.BuyExplorationData)]
    public class JournalBuyExplorationData : JournalEntry, ILedgerJournalEntry
    {
        public JournalBuyExplorationData(JObject evt ) : base(evt, JournalTypeEnum.BuyExplorationData)
        {
            System = evt["System"].Str();
            Cost = evt["Cost"].Long();

        }
        public string System { get; set; }
        public long Cost { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, System, -Cost);
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("System:".Txb(this), System, "Cost:; cr;N0".Txb(this), Cost);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.SellExplorationData)]
    public class JournalSellExplorationData : JournalEntry, ILedgerJournalEntry
    {
        public JournalSellExplorationData(JObject evt) : base(evt, JournalTypeEnum.SellExplorationData)
        {
            Systems = evt["Systems"]?.ToObjectProtected<string[]>();
            Discovered = evt["Discovered"]?.ToObjectProtected<string[]>();
            BaseValue = evt["BaseValue"].Long();
            Bonus = evt["Bonus"].Long();
            TotalEarnings = evt["TotalEarnings"].Long(0);        // may not be present - get 0. also 3.02 has a bug with incorrect value - actually fed from the FD web server so may not be version tied
            if (TotalEarnings < BaseValue + Bonus)        // so if less than the bv+bonus, it's either not there or bugged.  Fix
                TotalEarnings = BaseValue + Bonus;
        }

        public string[] Systems { get; set; }
        public string[] Discovered { get; set; }
        public long BaseValue { get; set; }
        public long Bonus { get; set; }
        public long TotalEarnings { get; set; }        // 3.0

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            if (Systems != null)
                mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Systems.Length + " systems", TotalEarnings);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Amount:; cr;N0".Txb(this), BaseValue, "Bonus:; cr;N0".Txb(this), Bonus,
                                "Total:; cr;N0".Tx(this), TotalEarnings);
            detailed = "";
            if (Systems != null)
            {
                detailed += "Scanned:".Txb(this);
                foreach (string s in Systems)
                    detailed += s + " ";
            }
            if (Discovered != null)
            {
                detailed += System.Environment.NewLine + "Discovered:".Txb(this);
                foreach (string s in Discovered)
                    detailed += s + " ";
            }
        }
    }

}
