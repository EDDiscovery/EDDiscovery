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
    //When Written: when repairing the ship
    //Parameters:
    //•	Item: all, wear, hull, paint, or name of module
    //•	Cost: cost of repair
    [JournalEntryType(JournalTypeEnum.Repair)]
    public class JournalRepair : JournalEntry, ILedgerJournalEntry
    {
        public JournalRepair(JObject evt ) : base(evt, JournalTypeEnum.Repair)
        {
            Item = JournalFieldNaming.GetBetterItemNameEvents(evt["Item"].Str());
            ItemLocalised = evt["Item_Localised"].Str().Alt(Item);
            Cost = evt["Cost"].Long();
        }

        public string Item { get; set; }
        public string ItemLocalised { get; set; }
        public long Cost { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Item, -Cost);
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("",ItemLocalised, "Cost:; cr;N0" , Cost );
            detailed = "";
        }
    }
}
