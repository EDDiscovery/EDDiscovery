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
    [JournalEntryType(JournalTypeEnum.RepairDrone)]
    public class JournalRepairDrone : JournalEntry
    {
        public JournalRepairDrone(JObject evt ) : base(evt, JournalTypeEnum.RepairDrone)
        {
            HullRepaired = evt["HullRepaired"].Double();
            CockpitRepaired = evt["CockpitRepaired"].Double();
            CorrosionRepaired = evt["CorrosionRepaired"].Double();
        }

        public double HullRepaired { get; set; }
        public double CockpitRepaired { get; set; }
        public double CorrosionRepaired { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Hull:".T(EDTx.JournalRepairDrone_Hull), HullRepaired.ToString("0.0"), "Cockpit:".T(EDTx.JournalEntry_Cockpit), CockpitRepaired.ToString("0.0"), 
                                "Corrosion:".T(EDTx.JournalEntry_Corrosion), CorrosionRepaired.ToString("0.0"));
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.BuyDrones)]
    public class JournalBuyDrones : JournalEntry, ILedgerJournalEntry, ICommodityJournalEntry
    {
        public JournalBuyDrones(JObject evt) : base(evt, JournalTypeEnum.BuyDrones)
        {
            Type = evt["Type"].Str();
            Count = evt["Count"].Int();
            BuyPrice = evt["BuyPrice"].Long();
            TotalCost = evt["TotalCost"].Long();

        }
        public string Type { get; set; }
        public int Count { get; set; }
        public long BuyPrice { get; set; }
        public long TotalCost { get; set; }

        public void UpdateCommodities(MaterialCommoditiesList mc, DB.IUserDatabase conn)
        {
            mc.Change(MaterialCommodityData.CommodityCategory, "drones", Count, 0);
        }

        public void Ledger(Ledger mcl, DB.IUserDatabase conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Type + " " + Count + " drones", -TotalCost);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Type:".T(EDTx.JournalEntry_Type), Type, "Count:".T(EDTx.JournalEntry_Count), Count, "Total Cost:; cr;N0".T(EDTx.JournalEntry_TotalCost), TotalCost, "each:; cr;N0".T(EDTx.JournalEntry_each), BuyPrice);
            detailed = "";
        }
    }


    [JournalEntryType(JournalTypeEnum.SellDrones)]
    public class JournalSellDrones : JournalEntry, ILedgerJournalEntry, ICommodityJournalEntry
    {
        public JournalSellDrones(JObject evt) : base(evt, JournalTypeEnum.SellDrones)
        {
            Type = evt["Type"].Str();
            Count = evt["Count"].Int();
            SellPrice = evt["SellPrice"].Long();
            TotalSale = evt["TotalSale"].Long();
        }
        public string Type { get; set; }
        public int Count { get; set; }
        public long SellPrice { get; set; }
        public long TotalSale { get; set; }

        public void UpdateCommodities(MaterialCommoditiesList mc, DB.IUserDatabase conn)
        {
            mc.Change(MaterialCommodityData.CommodityCategory, "drones", -Count, 0);
        }

        public void Ledger(Ledger mcl, DB.IUserDatabase conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Count.ToString() + " " + "Drones".T(EDTx.JournalEntry_Drones), TotalSale);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Type, "Count:".T(EDTx.JournalEntry_Count), Count, "Price:; cr;N0".T(EDTx.JournalEntry_Price), SellPrice, "Amount:; cr;N0".T(EDTx.JournalEntry_Amount), TotalSale);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.LaunchDrone)]
    public class JournalLaunchDrone : JournalEntry, ICommodityJournalEntry
    {
        public JournalLaunchDrone(JObject evt) : base(evt, JournalTypeEnum.LaunchDrone)
        {
            Type = evt["Type"].Str();
            FriendlyType = Type.SplitCapsWordFull();
        }

        public string Type { get; set; }
        public string FriendlyType { get; set; }

        public void UpdateCommodities(MaterialCommoditiesList mc, DB.IUserDatabase conn)
        {
            mc.Change(MaterialCommodityData.CommodityCategory, "drones", -1, 0);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Type:".T(EDTx.JournalEntry_Type), FriendlyType);
            detailed = "";
        }
    }


}
