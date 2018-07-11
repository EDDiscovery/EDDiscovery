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
    [JournalEntryType(JournalTypeEnum.Resurrect)]
    public class JournalResurrect : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalResurrect(JObject evt) : base(evt, JournalTypeEnum.Resurrect)
        {
            Option = evt["Option"].Str().SplitCapsWordFull();
            Cost = evt["Cost"].Long();
            Bankrupt = evt["Bankrupt"].Bool();
        }

        public string Option { get; set; }
        public long Cost { get; set; }
        public bool Bankrupt { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Option, -Cost);
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.Resurrect(Option.Equals("free", System.StringComparison.InvariantCultureIgnoreCase));    // if free, we did not rebuy the ship
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Option:".Txb(this), Option, "Cost:; cr;N0".Txb(this), Cost, ";Bankrupt".Txb(this), Bankrupt);
            detailed = "";
        }
    }
}
