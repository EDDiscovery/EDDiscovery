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
using System.Drawing;

namespace EliteDangerousCore.JournalEvents
{
    //When Written: when purchasing an SRV or Fighter
    //Parameters:
    //•	Type: type of vehicle being purchased (SRV or fighter model)
    //•	Loadout: variant
    //•	Cost: purchase cost
    //•	Count: number of vehicles purchased
    [JournalEntryType(JournalTypeEnum.RestockVehicle)]
    public class JournalRestockVehicle : JournalEntry, ILedgerJournalEntry
    {
        public JournalRestockVehicle(JObject evt ) : base(evt, JournalTypeEnum.RestockVehicle)
        {
            Type = JournalFieldNaming.GetBetterShipName(evt["Type"].Str());
            Loadout = evt["Loadout"].Str();
            Cost = evt["Cost"].Long();
            Count = evt["Count"].Int();
        }
        public string Type { get; set; }
        public string Loadout { get; set; }
        public long Cost { get; set; }
        public int Count { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Type + " " + Count.ToString(), -Cost);
        }

        public override Image Icon
        {
            get
            {
                return Type.Contains("SRV") ? GetIcon(JournalTypeEnum.RestockVehicle_SRV) : GetIcon(JournalTypeEnum.RestockVehicle_Fighter);
            }
        }

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = BaseUtils.FieldBuilder.Build("",Type , "Cost:; credits" , Cost , "Count:" , Count , "Loadout:" , Loadout);
            detailed = "";
        }
    }
}
