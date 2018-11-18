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
    [JournalEntryType(JournalTypeEnum.RefuelAll)]
    public class JournalRefuelAll : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalRefuelAll(JObject evt ) : base(evt, JournalTypeEnum.RefuelAll)
        {
            Cost = evt["Cost"].Long();
            Amount = evt["Amount"].Double();
        }

        public long Cost { get; set; }
        public double Amount { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Amount.ToString() + "t", -Cost);
        }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Cost:; cr;N0".Txb(this), Cost, "Fuel:; tons;0.0".Txb(this), Amount);
            detailed = "";
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.RefuelAll(this);
        }
    }

    [JournalEntryType(JournalTypeEnum.RefuelPartial)]
    public class JournalRefuelPartial : JournalEntry, ILedgerJournalEntry, IShipInformation
    {
        public JournalRefuelPartial(JObject evt) : base(evt, JournalTypeEnum.RefuelPartial)
        {
            Cost = evt["Cost"].Long();
            Amount = evt["Amount"].Int();
        }

        public long Cost { get; set; }
        public int Amount { get; set; }

        public void Ledger(Ledger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Amount.ToString() + "t", -Cost);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Cost:; cr;N0".Txb(this), Cost, "Fuel:; tons;0.0".Txb(this), Amount);
            detailed = "";
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.RefuelPartial(this);
        }
    }


    [JournalEntryType(JournalTypeEnum.FuelScoop)]
    public class JournalFuelScoop : JournalEntry, IShipInformation
    {
        public JournalFuelScoop(JObject evt) : base(evt, JournalTypeEnum.FuelScoop)
        {
            Scooped = evt["Scooped"].Double();
            Total = evt["Total"].Double();
        }
        public double Scooped { get; set; }
        public double Total { get; set; }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build(";t;0.0", Scooped, "Total:;t;0.0".Tx(this), Total);
            detailed = "";
        }

        public void ShipInformation(ShipInformationList shp, string whereami, ISystem system, DB.SQLiteConnectionUser conn)
        {
            shp.FuelScoop(this);
        }
    }

}
