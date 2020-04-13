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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.CarrierBuy)]
    public class JournalCarrierBuy : JournalEntry
    {
        public long BoughtAtMarket { get; set; }
        public long CarrierID { get; set; }
        public string Location { get; set; }
        public long Price { get; set; }
        public string Variant { get; set; }
        public string Callsign { get; set; }

        public JournalCarrierBuy(JObject evt) : base(evt, JournalTypeEnum.CarrierBuy)
        {
            BoughtAtMarket = evt["BoughtAtMarket"].Long();
            CarrierID = evt["CarrierID"].Long();
            Location = evt["Location"].Str();
            Price = evt["Price"].Long();
            Variant = evt["Variant"].Str();
            Callsign = evt["Callsign"].Str();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Buy";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierStats)]
    public class JournalCarrierStats : JournalEntry
    {
        public long CarrierID { get; set; }
        public string Callsign { get; set; }
        public string Name { get; set; }
        public string DockingAccess { get; set; }
        public bool AllowNotorious { get; set; }
        public int FuelLevel { get; set; }
        public double JumpRangeCurr { get; set; }
        public double JumpRangeMax { get; set; }
        public bool PendingDecommission { get; set; }
        public long SpaceUsage_TotalCapacity { get; set; }
        public long SpaceUsage_Crew { get; set; }
        public long SpaceUsage_Cargo { get; set; }
        public long SpaceUsage_CargoSpaceReserved { get; set; }
        public long SpaceUsage_ShipPacks { get; set; }
        public long SpaceUsage_ModulePacks { get; set; }
        public long SpaceUsage_FreeSpace { get; set; }
        public long Finance_CarrierBalance { get; set; }
        public long Finance_ReserveBalance { get; set; }
        public long Finance_AvailableBalance { get; set; }
        public double Finance_ReservePercent { get; set; }
        public double Finance_TaxRate { get; set; }
        public CrewClass[] Crew { get; set; }
        public PackClass[] ShipPacks { get; set; }
        public PackClass[] ModulePacks { get; set; }

        public JournalCarrierStats(JObject evt) : base(evt, JournalTypeEnum.CarrierStats)
        {
            CarrierID = evt["CarrierID"].Long();
            Callsign = evt["Callsign"].Str();
            Name = evt["Name"].Str();
            DockingAccess = evt["DockingAccess"].Str();
            AllowNotorious = evt["AllowNotorious"].Bool();
            FuelLevel = evt["FuelLevel"].Int();
            JumpRangeCurr = evt["JumpRangeCurr"].Double();
            JumpRangeMax = evt["JumpRangeMax"].Double();
            PendingDecommission = evt["PendingDecommission"].Bool();

            var spaceusage = evt["SpaceUsage"];
            if (spaceusage != null)
            {
                SpaceUsage_Cargo = spaceusage["Cargo"].Long();
                SpaceUsage_CargoSpaceReserved = spaceusage["CargoSpaceReserved"].Long();
                SpaceUsage_Crew = spaceusage["Crew"].Long();
                SpaceUsage_FreeSpace = spaceusage["FreeSpace"].Long();
                SpaceUsage_ModulePacks = spaceusage["ModulePacks"].Long();
                SpaceUsage_ShipPacks = spaceusage["ShipPacks"].Long();
                SpaceUsage_TotalCapacity = spaceusage["TotalCapacity"].Long();
            }

            var finance = evt["finance"];
            if (finance != null)
            {
                Finance_CarrierBalance = finance["CarrierBalance"].Long();
                Finance_ReserveBalance = finance["ReserveBalance"].Long();
                Finance_AvailableBalance = finance["AvailableBalance"].Long();
                Finance_ReservePercent = finance["ReservePercent"].Double();
                Finance_TaxRate = finance["TaxRate"].Double();
            }

            Crew = evt["Crew"]?.ToObjectProtected<CrewClass[]>();
            ShipPacks = evt["ShipPacks"]?.ToObjectProtected<PackClass[]>();
            ModulePacks = evt["ModulePacks"]?.ToObjectProtected<PackClass[]>();
        }

        public class CrewClass
        {
            public string CrewRole { get; set; }
            public bool Activated { get; set; }
            public string CrewName { get; set; }
        }

        public class PackClass
        {
            public string PackTheme { get; set; }
            public int PackTier { get; set; }
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Stats";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierJumpRequest)]
    public class JournalCarrierJumpRequest : JournalEntry
    {
        public long CarrierID { get; set; }
        public string SystemName { get; set; }
        public long SystemID { get; set; }

        public JournalCarrierJumpRequest(JObject evt) : base(evt, JournalTypeEnum.CarrierJumpRequest)
        {
            CarrierID = evt["CarrierID"].Long();
            SystemName = evt["SystemName"].Str();
            SystemID = evt["SystemID"].Long();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Jump Request";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierDecommission)]
    public class JournalCarrierDecommission : JournalEntry
    {
        public long CarrierID { get; set; }
        public long ScrapRefund { get; set; }
        public long ScrapTime { get; set; }
        public DateTime ScrapDateTime { get; set; }

        public JournalCarrierDecommission(JObject evt) : base(evt, JournalTypeEnum.CarrierDecommission)
        {
            CarrierID = evt["CarrierID"].Long();
            ScrapRefund = evt["ScrapRefund"].Long();
            ScrapTime = evt["ScrapTime"].Long();
            ScrapDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(ScrapTime);
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Decommission";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierCancelDecommission)]
    public class JournalCarrierCancelDecommission : JournalEntry
    {
        public long CarrierID { get; set; }

        public JournalCarrierCancelDecommission(JObject evt) : base(evt, JournalTypeEnum.CarrierCancelDecommission)
        {
            CarrierID = evt["CarrierID"].Long();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Cancel Carrier Decommission";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierBankTransfer)]
    public class JournalCarrierBankTransfer : JournalEntry, ILedgerJournalEntry
    {
        public long CarrierID { get; set; }
        public long Deposit { get; set; }
        public long Withdraw { get; set; }
        public long PlayerBalance { get; set; }
        public long CarrierBalance { get; set; }

        public JournalCarrierBankTransfer(JObject evt) : base(evt, JournalTypeEnum.CarrierBankTransfer)
        {
            CarrierID = evt["CarrierID"].Long();
            Deposit = evt["Deposit"].Long();
            Withdraw = evt["Withdraw"].Long();
            PlayerBalance = evt["PlayerBalance"].Long();
            CarrierBalance = evt["CarrierBalance"].Long();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Bank Transfer";
            detailed = "";
        }

        public void Ledger(Ledger mcl)
        {
            mcl.AddEvent(Id, EventTimeUTC, JournalTypeEnum.CarrierBankTransfer, Deposit == 0 ? "Withdraw" : "Deposit", PlayerBalance - mcl.CashTotal);
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierDepositFuel)]
    public class JournalCarrierDepositFuel : JournalEntry, ICommodityJournalEntry
    {
        public long CarrierID { get; set; }
        public int Amount { get; set; }
        public int Total { get; set; }

        public JournalCarrierDepositFuel(JObject evt) : base(evt, JournalTypeEnum.CarrierDepositFuel)
        {
            CarrierID = evt["CarrierID"].Long();
            Amount = evt["Amount"].Int();
            Total = evt["Total"].Int();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Deposit Fuel";
            detailed = "";
        }

        public void UpdateCommodities(MaterialCommoditiesList mc)
        {
            mc.Change(MaterialCommodityData.CatType.Commodity, "$tritium_name;", -Amount, 0);
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierCrewServices)]
    public class JournalCarrierCrewServices : JournalEntry
    {
        public long CarrierID { get; set; }
        public string Operation { get; set; }
        public string CrewRole { get; set; }
        public string CrewName { get; set; }

        public JournalCarrierCrewServices(JObject evt) : base(evt, JournalTypeEnum.CarrierCrewServices)
        {
            CarrierID = evt["CarrierID"].Long();
            Operation = evt["Operation"].Str();
            CrewRole = evt["CrewRole"].Str();
            CrewName = evt["CrewName"].Str();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Crew Services";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierFinance)]
    public class JournalCarrierFinance : JournalEntry
    {
        public long CarrierID { get; set; }
        public double TaxRate { get; set; }
        public long CarrierBalance { get; set; }
        public long ReserveBalance { get; set; }
        public long AvailableBalance { get; set; }
        public double ReservePercent { get; set; }

        public JournalCarrierFinance(JObject evt) : base(evt, JournalTypeEnum.CarrierFinance)
        {
            CarrierID = evt["CarrierID"].Long();
            TaxRate = evt["TaxRate"].Double();
            CarrierBalance = evt["CarrierBalance"].Long();
            ReserveBalance = evt["ReserveBalance"].Long();
            AvailableBalance = evt["AvailableBalance"].Long();
            ReservePercent = evt["ReservePercent"].Double();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Finance";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierShipPack)]
    public class JournalCarrierShipPack : JournalEntry
    {
        public long CarrierID { get; set; }
        public string Operation { get; set; }
        public string PackTheme { get; set; }
        public int PackTier { get; set; }
        public long Cost { get; set; }
        public long Refund { get; set; }

        public JournalCarrierShipPack(JObject evt) : base(evt, JournalTypeEnum.CarrierShipPack)
        {
            CarrierID = evt["CarrierID"].Long();
            Operation = evt["Operation"].Str();
            PackTheme = evt["PackTheme"].Str();
            PackTier = evt["PackTier"].Int();
            Cost = evt["Cost"].Long();
            Refund = evt["Refund"].Long();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Ship Pack";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierModulePack)]
    public class JournalCarrierModulePack : JournalEntry
    {
        public long CarrierID { get; set; }
        public string Operation { get; set; }
        public string PackTheme { get; set; }
        public int PackTier { get; set; }
        public long Cost { get; set; }
        public long Refund { get; set; }

        public JournalCarrierModulePack(JObject evt) : base(evt, JournalTypeEnum.CarrierModulePack)
        {
            CarrierID = evt["CarrierID"].Long();
            Operation = evt["Operation"].Str();
            PackTheme = evt["PackTheme"].Str();
            PackTier = evt["PackTier"].Int();
            Cost = evt["Cost"].Long();
            Refund = evt["Refund"].Long();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Ship Pack";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierTradeOrder)]
    public class JournalCarrierTradeOrder : JournalEntry
    {
        public long CarrierID { get; set; }
        public bool BlackMarket { get; set; }
        public string Commodity { get; set; }
        public int PurchaseOrder { get; set; }
        public int SaleOrder { get; set; }
        public bool CancelTrade { get; set; }
        public long Price { get; set; }

        public JournalCarrierTradeOrder(JObject evt) : base(evt, JournalTypeEnum.CarrierTradeOrder)
        {
            CarrierID = evt["CarrierID"].Long();
            BlackMarket = evt["BlackMarket"].Bool();
            Commodity = evt["Commodity"].Str();
            PurchaseOrder = evt["PurchaseOrder"].Int();
            SaleOrder = evt["SaleOrder"].Int();
            CancelTrade = evt["CancelTrade"].Bool();
            Price = evt["Price"].Long();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Trade Order";
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierDockingPermission)]
    public class JournalCarrierDockingPermission : JournalEntry
    {
        public long CarrierID { get; set; }
        public string DockingAccess { get; set; }
        public bool AllowNotorious { get; set; }

        public JournalCarrierDockingPermission(JObject evt) : base(evt, JournalTypeEnum.CarrierDockingPermission)
        {
            CarrierID = evt["CarrierID"].Long();
            DockingAccess = evt["DockingAccess"].Str();
            AllowNotorious = evt["AllowNotorious"].Bool();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "Carrier Docking Permission";
            detailed = "";
        }
    }
}
