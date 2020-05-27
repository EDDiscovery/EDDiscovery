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
using System;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.CarrierBuy)]
    public class JournalCarrierBuy : JournalEntry, ILedgerJournalEntry
    {
        public long CarrierID { get; set; }
        public long BoughtAtMarket { get; set; }
        public string Location { get; set; }
        public long SystemAddress { get; set; }
        public long Price { get; set; }
        public string Variant { get; set; }
        public string Callsign { get; set; }

        public JournalCarrierBuy(JObject evt) : base(evt, JournalTypeEnum.CarrierBuy)
        {
            CarrierID = evt["CarrierID"].Long();
            BoughtAtMarket = evt["BoughtAtMarket"].Long();
            Location = evt["Location"].Str();
            SystemAddress = evt["SystemAddress"].Long();
            Price = evt["Price"].Long();
            Variant = evt["Variant"].Str();
            Callsign = evt["Callsign"].Str();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("At ".T(EDTx.JournalCarrier_At), Location,
                                              "Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Price,
                                              "Call Sign:".T(EDTx.JournalCarrier_Callsign), Callsign);
            detailed = "";
        }

        public void Ledger(Ledger mcl)
        {
            string x = "Call Sign:".T(EDTx.JournalCarrier_Callsign) + Callsign;
            mcl.AddEvent(Id, EventTimeUTC, JournalTypeEnum.CarrierBuy, x, -Price);
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

        public int SpaceUsage_TotalCapacity { get; set; }
        public int SpaceUsage_Crew { get; set; }
        public int SpaceUsage_Cargo { get; set; }
        public int SpaceUsage_CargoSpaceReserved { get; set; }
        public int SpaceUsage_ShipPacks { get; set; }
        public int SpaceUsage_ModulePacks { get; set; }
        public int SpaceUsage_FreeSpace { get; set; }

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
                SpaceUsage_TotalCapacity = spaceusage["TotalCapacity"].Int();
                SpaceUsage_Crew = spaceusage["Crew"].Int();
                SpaceUsage_Cargo = spaceusage["Cargo"].Int();
                SpaceUsage_CargoSpaceReserved = spaceusage["CargoSpaceReserved"].Int();
                SpaceUsage_ShipPacks = spaceusage["ShipPacks"].Int();
                SpaceUsage_ModulePacks = spaceusage["ModulePacks"].Int();
                SpaceUsage_FreeSpace = spaceusage["FreeSpace"].Int();
            }

            var finance = evt["Finance"];
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
            public bool Enabled { get; set; }
            public string CrewName { get; set; }
        }

        public class PackClass
        {
            public string PackTheme { get; set; }
            public int PackTier { get; set; }
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Name:".T(EDTx.JournalCarrier_Name), Name,
                                                "Call Sign:".T(EDTx.JournalCarrier_Callsign), Callsign,
                                                "Fuel Level:;;N0".T(EDTx.JournalCarrier_FuelLevel), FuelLevel,
                                                "Jump Range:;ly;0.0".T(EDTx.JournalCarrier_JumpRange), JumpRangeCurr,
                                                "Carrier Balance:; cr;N0".T(EDTx.JournalCarrier_Balance), Finance_CarrierBalance,
                                                "Reserve Balance:; cr;N0".T(EDTx.JournalCarrier_ReserveBalance), Finance_ReserveBalance,
                                                "Available Balance:; cr;N0".T(EDTx.JournalCarrier_AvailableBalance), Finance_AvailableBalance,
                                                "Reserve Percent:;;N1".T(EDTx.JournalCarrier_ReservePercent), Finance_ReservePercent,
                                                "Tax Rate:;;N1".T(EDTx.JournalCarrier_TaxRate), Finance_TaxRate
                                                );

            detailed = BaseUtils.FieldBuilder.Build("Total Capacity:".T(EDTx.JournalCarrier_TotalCapacity), SpaceUsage_TotalCapacity,
                                                    "Crew:".T(EDTx.JournalCarrier_Crew), SpaceUsage_Crew,
                                                    "Cargo:".T(EDTx.JournalCarrier_Cargo), SpaceUsage_Cargo,
                                                    "Cargo Space Reserved:".T(EDTx.JournalCarrier_CargoReserved), SpaceUsage_CargoSpaceReserved,
                                                    "Ship Packs:".T(EDTx.JournalCarrier_ShipPacks), SpaceUsage_ShipPacks,
                                                    "Module Packs:".T(EDTx.JournalCarrier_ModulePacks), SpaceUsage_ModulePacks,
                                                    "Free Space:".T(EDTx.JournalCarrier_FreeSpace), SpaceUsage_FreeSpace);
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierJumpRequest)]
    public class JournalCarrierJumpRequest : JournalEntry
    {
        public long CarrierID { get; set; }
        public string SystemName { get; set; }
        public string Body { get; set; }
        public long SystemAddress { get; set; }
        public int BodyID { get; set; }

        public JournalCarrierJumpRequest(JObject evt) : base(evt, JournalTypeEnum.CarrierJumpRequest)
        {
            CarrierID = evt["CarrierID"].Long();
            SystemName = evt["SystemName"].Str();
            Body = evt["Body"].Str();
            SystemAddress = evt["SystemID"].Long();
            BodyID = evt["BodyID"].Int();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("To ".T(EDTx.JournalCarrier_ToSystem), SystemName,
                                                "Body ".T(EDTx.JournalCarrier_Body), Body
                                                );
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierDecommission)]
    public class JournalCarrierDecommission : JournalEntry      // TBD Ledger
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
            info = BaseUtils.FieldBuilder.Build("Refund:; cr;N0".T(EDTx.JournalCarrier_Refund), ScrapRefund,
                                                "at UTC ".T(EDTx.JournalCarrier_RefundTime), ScrapDateTime
                                                );
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
            info = "";
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
            if (Deposit > 0)
                info = BaseUtils.FieldBuilder.Build("Deposit:; cr;N0".T(EDTx.JournalCarrier_Deposit), Deposit, "Carrier Balance:; cr;N0".T(EDTx.JournalCarrier_Balance), CarrierBalance);
            else
                info = BaseUtils.FieldBuilder.Build("Withdraw:; cr;N0".T(EDTx.JournalCarrier_Withdraw), Withdraw, "Carrier Balance:; cr;N0".T(EDTx.JournalCarrier_Balance), CarrierBalance);
            detailed = "";
        }

        public void Ledger(Ledger mcl)
        {
            mcl.AddEvent(Id, EventTimeUTC, JournalTypeEnum.CarrierBankTransfer, "" , Withdraw - Deposit);
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierDepositFuel)]
    public class JournalCarrierDepositFuel : JournalEntry, ICommodityJournalEntry
    {
        public long CarrierID { get; set; }
        public int Amount { get; set; }      // being paranoid about this .. don't trust they will be ints forever.
        public int Total { get; set; }

        public JournalCarrierDepositFuel(JObject evt) : base(evt, JournalTypeEnum.CarrierDepositFuel)
        {
            CarrierID = evt["CarrierID"].Long();
            Amount = evt["Amount"].Int();
            Total = evt["Total"].Int();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Amount:;;N0".T(EDTx.JournalCarrier_Amount), Amount,
                                                "Fuel Level:;;N0".T(EDTx.JournalCarrier_FuelLevel), Total);
            detailed = "";
        }

        public void UpdateCommodities(MaterialCommoditiesList mc)
        {
            mc.Change(MaterialCommodityData.CatType.Commodity, "tritium", -Amount, 0);
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
            CrewRole = evt["CrewRole"].Str();
            Operation = evt["Operation"].Str();
            CrewName = evt["CrewName"].Str();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Role:".T(EDTx.JournalEntry_Role), CrewRole.SplitCapsWordFull(),
                                                "Operation:".T(EDTx.JournalCarrier_Operation), Operation,
                                                "Crew Member:".T(EDTx.JournalEntry_CrewMember), CrewName
                                                );
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierFinance)]
    public class JournalCarrierFinance : JournalEntry
    {
        public long CarrierID { get; set; }
        public long Finance_CarrierBalance { get; set; }        // names aligned to Stats
        public long Finance_ReserveBalance { get; set; }
        public long Finance_AvailableBalance { get; set; }
        public double Finance_ReservePercent { get; set; }
        public double Finance_TaxRate { get; set; }

        public JournalCarrierFinance(JObject evt) : base(evt, JournalTypeEnum.CarrierFinance)
        {
            CarrierID = evt["CarrierID"].Long();
            Finance_TaxRate = evt["TaxRate"].Double();
            Finance_CarrierBalance = evt["CarrierBalance"].Long();
            Finance_ReserveBalance = evt["ReserveBalance"].Long();
            Finance_AvailableBalance = evt["AvailableBalance"].Long();
            Finance_ReservePercent = evt["ReservePercent"].Double();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Carrier Balance:; cr;N0".T(EDTx.JournalCarrier_Balance), Finance_CarrierBalance,
                                                "Reserve Balance:; cr;N0".T(EDTx.JournalCarrier_ReserveBalance), Finance_ReserveBalance,
                                                "Available Balance:; cr;N0".T(EDTx.JournalCarrier_AvailableBalance), Finance_AvailableBalance,
                                                "Reserve Percent:;;N1".T(EDTx.JournalCarrier_ReservePercent), Finance_ReservePercent,
                                                "Tax Rate:;;N1".T(EDTx.JournalCarrier_TaxRate), Finance_TaxRate
                                                );
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierShipPack)]
    public class JournalCarrierShipPack : JournalEntry, ILedgerJournalEntry
    {
        public long CarrierID { get; set; }
        public string Operation { get; set; }       // BuyPack, SellPack
        public string PackTheme { get; set; }
        public int PackTier { get; set; }
        public long? Cost { get; set; }
        public long? Refund { get; set; }

        public JournalCarrierShipPack(JObject evt) : base(evt, JournalTypeEnum.CarrierShipPack)
        {
            CarrierID = evt["CarrierID"].Long();
            Operation = evt["Operation"].Str();
            PackTheme = evt["PackTheme"].Str();
            PackTier = evt["PackTier"].Int();
            Cost = evt["Cost"].LongNull();
            Refund = evt["Refund"].LongNull();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build(
                                                "", Operation.SplitCapsWordFull(),
                                                "", PackTheme,
                                                "Tier:".T(EDTx.JournalCarrier_Tier), PackTier,
                                                "Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Cost,
                                                "Refund:; cr;N0".T(EDTx.JournalCarrier_Refund), Refund
                                                );

            detailed = "";
        }

        public void Ledger(Ledger mcl)
        {
            mcl.AddEvent(Id, EventTimeUTC, JournalTypeEnum.CarrierShipPack, Operation.SplitCapsWordFull(), Refund - Cost);
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierModulePack)]
    public class JournalCarrierModulePack : JournalEntry, ILedgerJournalEntry
    {
        public long CarrierID { get; set; }
        public string Operation { get; set; }
        public string PackTheme { get; set; }
        public int PackTier { get; set; }
        public long? Cost { get; set; }
        public long? Refund { get; set; }

        public JournalCarrierModulePack(JObject evt) : base(evt, JournalTypeEnum.CarrierModulePack)
        {
            CarrierID = evt["CarrierID"].Long();
            Operation = evt["Operation"].Str();
            PackTheme = evt["PackTheme"].Str();
            PackTier = evt["PackTier"].Int();
            Cost = evt["Cost"].LongNull();
            Refund = evt["Refund"].LongNull();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("", Operation.SplitCapsWordFull(),
                                                "", PackTheme,
                                                "Tier:".T(EDTx.JournalCarrier_Tier), PackTier,
                                                "Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Cost,
                                                "Refund:; cr;N0".T(EDTx.JournalCarrier_Refund), Refund
                                                );

            detailed = "";
        }

        public void Ledger(Ledger mcl)
        {
            mcl.AddEvent(Id, EventTimeUTC, JournalTypeEnum.CarrierShipPack, Operation.SplitCapsWordFull(), Refund - Cost);
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierTradeOrder)]
    public class JournalCarrierTradeOrder : JournalEntry
    {
        public long CarrierID { get; set; }
        public bool BlackMarket { get; set; }
        public string Commodity { get; set; }
        public string Commodity_Localised { get; set; }
        public int? PurchaseOrder { get; set; }
        public int? SaleOrder { get; set; }
        public bool? CancelTrade { get; set; }
        public int Price { get; set; }

        public JournalCarrierTradeOrder(JObject evt) : base(evt, JournalTypeEnum.CarrierTradeOrder)
        {
            CarrierID = evt["CarrierID"].Long();
            BlackMarket = evt["BlackMarket"].Bool();
            Commodity = evt["Commodity"].Str();
            Commodity_Localised =JournalFieldNaming.CheckLocalisation(evt["Commodity_Localised"].Str(),Commodity);
            PurchaseOrder = evt["PurchaseOrder"].IntNull();
            SaleOrder = evt["SaleOrder"].IntNull();
            CancelTrade = evt["CancelTrade"].BoolNull();
            Price = evt["Price"].Int();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            if (PurchaseOrder != null)
            {
                info = BaseUtils.FieldBuilder.Build("Purchase:".T(EDTx.JournalCarrier_Purchase), Commodity_Localised,
                                                    "", PurchaseOrder,
                                                    "Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Price);
            }
            else if ( SaleOrder != null)
            {
                info = BaseUtils.FieldBuilder.Build("Sell:".T(EDTx.JournalCarrier_Sell), Commodity_Localised,
                                                    "", SaleOrder,
                                                    "Cost:; cr;N0".T(EDTx.JournalEntry_Cost), Price);
            }
            else if ( CancelTrade != null && CancelTrade.Value == true )
            {
                info = BaseUtils.FieldBuilder.Build("Cancel Sell of:".T(EDTx.JournalCarrier_CancelSell), Commodity_Localised);
            }
            else
            {
                info = "Incorrect options for this entry, report journal entry to EDD Team";
            }

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
            info = BaseUtils.FieldBuilder.Build("Access:".T(EDTx.JournalCarrier_Access), DockingAccess, 
                                                ";Allow Notorious".T(EDTx.JournalCarrier_AllowNotorious), AllowNotorious);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierNameChanged)]
    public class JournalCarrierNameChanged : JournalEntry
    {
        public long CarrierID { get; set; }
        public string Callsign { get; set; }
        public string Name { get; set; }

        public JournalCarrierNameChanged(JObject evt) : base(evt, JournalTypeEnum.CarrierNameChanged)
        {
            CarrierID = evt["CarrierID"].Long();
            Callsign = evt["Callsign"].Str();
            Name = evt["Name"].Str();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = BaseUtils.FieldBuilder.Build("Name:".T(EDTx.JournalCarrier_Name), Name, "Call Sign:".T(EDTx.JournalCarrier_Callsign), Callsign);
            detailed = "";
        }
    }

    [JournalEntryType(JournalTypeEnum.CarrierJumpCancelled)]
    public class JournalCarrierJumpCancelled : JournalEntry
    {
        public long CarrierID { get; set; }

        public JournalCarrierJumpCancelled(JObject evt) : base(evt, JournalTypeEnum.CarrierJumpCancelled)
        {
            CarrierID = evt["CarrierID"].Long();
        }

        public override void FillInformation(out string info, out string detailed)
        {
            info = "";
            detailed = "";
        }
    }
}

