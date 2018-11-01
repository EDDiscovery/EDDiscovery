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

        protected override JournalTypeEnum IconEventType { get { return Type.Contains("SRV") ? JournalTypeEnum.RestockVehicle_SRV : JournalTypeEnum.RestockVehicle_Fighter; } }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("",Type , "Cost:; cr;N0".Txb(this), Cost , "Count:".Txb(this), Count , "Loadout:".Txb(this), Loadout);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.VehicleSwitch)]
    public class JournalVehicleSwitch : JournalEntry, IShipInformation
    {
        public JournalVehicleSwitch(JObject evt) : base(evt, JournalTypeEnum.VehicleSwitch)
        {
            To = evt["To"].Str();
            if (To.Length == 0)             // Frontier BUG, sometimes To is missing
                To = "Mothership";
        }
        public string To { get; set; }

        protected override JournalTypeEnum IconEventType { get { return To.Contains("Mothership") ? JournalTypeEnum.VehicleSwitch_Mothership : JournalTypeEnum.VehicleSwitch_Fighter; } }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.VehicleSwitch(To);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = To;
            detailed = "";
        }
    }

}
