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
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    [JournalEntryType(JournalTypeEnum.Statistics)]
    public class JournalStatistics : JournalEntry
    {
        public JournalStatistics(JObject evt ) : base(evt, JournalTypeEnum.Statistics)
        {
            BankAccount = evt["Bank_Account"].RemoveKeyUnderscores()?.ToObjectProtected<BankAccountClass>() ?? new BankAccountClass();
            Combat = evt["Combat"].RemoveKeyUnderscores()?.ToObjectProtected<CombatClass>() ?? new CombatClass();
            Crime = evt["Crime"].RemoveKeyUnderscores()?.ToObjectProtected<CrimeClass>() ?? new CrimeClass();
            Smuggling = evt["Smuggling"].RemoveKeyUnderscores()?.ToObjectProtected<SmugglingClass>() ?? new SmugglingClass();
            Trading = evt["Trading"].RemoveKeyUnderscores()?.ToObjectProtected<TradingClass>() ?? new TradingClass();
            Mining = evt["Mining"].RemoveKeyUnderscores()?.ToObjectProtected<MiningClass>() ?? new MiningClass();
            Exploration = evt["Exploration"].RemoveKeyUnderscores()?.ToObjectProtected<ExplorationClass>() ?? new ExplorationClass();
            PassengerMissions = evt["Passengers"].RemoveKeyUnderscores().RemoveKeyPrefix("PassengersMissions")?.ToObjectProtected<PassengerMissionsClass>() ?? new PassengerMissionsClass();
            SearchAndRescue = evt["Search_And_Rescue"].RemoveKeyUnderscores().RemoveKeyPrefix("SearchRescue")?.ToObjectProtected<SearchAndRescueClass>() ?? new SearchAndRescueClass();
            Crafting = evt["Crafting"].RemoveKeyUnderscores()?.ToObjectProtected<CraftingClass>() ?? new CraftingClass();
            Crew = evt["Crew"].RemoveKeyUnderscores().RemoveKeyPrefix("NpcCrew")?.ToObjectProtected<CrewClass>() ?? new CrewClass();
            Multicrew = evt["Multicrew"].RemoveKeyUnderscores().RemoveKeyPrefix("Multicrew")?.ToObjectProtected<MulticrewClass>() ?? new MulticrewClass();
            MaterialTraderStats = evt["Material_Trader_Stats"].RemoveKeyUnderscores()?.ToObjectProtected<MaterialTraderStatsClass>() ?? new MaterialTraderStatsClass();
            CQC = evt["CQC"].RemoveKeyUnderscores().RemoveKeyPrefix("CQC")?.ToObjectProtected<CQCClass>() ?? new CQCClass();

            Json = evt;     // keep for inara purposes..
        }

        public BankAccountClass BankAccount { get; set; }
        public CombatClass Combat { get; set; }
        public CrimeClass Crime { get; set; }
        public SmugglingClass Smuggling { get; set; }
        public TradingClass Trading { get; set; }
        public MiningClass Mining { get; set; }
        public ExplorationClass Exploration { get; set; }
        public PassengerMissionsClass PassengerMissions { get; set; }
        public SearchAndRescueClass SearchAndRescue { get; set; }
        public CraftingClass Crafting { get; set; }
        public CrewClass Crew { get; set; }
        public MulticrewClass Multicrew { get; set; }
        public MaterialTraderStatsClass MaterialTraderStats { get; set; }
        public CQCClass CQC { get; set; }
        public JObject Json { get; set; }

        public override void FillInformation(out string info, out string detailed) 
        {
            info = BaseUtils.FieldBuilder.Build("Wealth:;cr;N0".T(EDTx.JournalEntry_Wealth), BankAccount.CurrentWealth, "Notoriety Index:;;N0".T(EDTx.JournalEntry_NotorietyIndex), Crime.Notoriety);

            detailed =  "Bank Account".T(EDTx.JournalStatistics_BankAccount) + Environment.NewLine + BankAccount?.Format() + Environment.NewLine + 
                        "Combat".T(EDTx.JournalStatistics_Combat) + Environment.NewLine + Combat?.Format() + Environment.NewLine +
                        "Crime".T(EDTx.JournalStatistics_Crime) + Environment.NewLine + Crime?.Format() + Environment.NewLine +
                        "Smuggling".T(EDTx.JournalStatistics_Smuggling) + Environment.NewLine + Smuggling?.Format() + Environment.NewLine +
                        "Trading".T(EDTx.JournalStatistics_Trading) + Environment.NewLine + Trading?.Format() + Environment.NewLine +
                        "Mining".T(EDTx.JournalStatistics_Mining) + Environment.NewLine + Mining?.Format() + Environment.NewLine +
                        "Exploration".T(EDTx.JournalStatistics_Exploration) + Environment.NewLine + Exploration?.Format() + Environment.NewLine +
                        "Passengers".T(EDTx.JournalStatistics_Passengers) + Environment.NewLine + PassengerMissions?.Format() + Environment.NewLine +
                        "Search and Rescue".T(EDTx.JournalStatistics_SearchandRescue) + Environment.NewLine + SearchAndRescue?.Format() + Environment.NewLine +
                        "Engineers".T(EDTx.JournalStatistics_Engineers) + Environment.NewLine + Crafting?.Format() + Environment.NewLine +
                        "Crew".T(EDTx.JournalStatistics_Crew) + Environment.NewLine + Crew?.Format() + Environment.NewLine +
                        "Multicrew".T(EDTx.JournalStatistics_Multicrew) + Environment.NewLine + Multicrew?.Format() + Environment.NewLine +
                        "Materials and Commodity Trading".T(EDTx.JournalStatistics_MaterialsandCommodityTrading) + Environment.NewLine + MaterialTraderStats?.Format() + Environment.NewLine +
                        "CQC".T(EDTx.JournalStatistics_CQC) + Environment.NewLine + CQC?.Format();
        }

        public class BankAccountClass
        {
            public long CurrentWealth { get; set; }
            public long SpentOnShips { get; set; }
            public long SpentOnOutfitting { get; set; }
            public long SpentOnRepairs { get; set; }
            public long SpentOnFuel { get; set; }
            public long SpentOnAmmoConsumables { get; set; }
            public int InsuranceClaims { get; set; }
            public long SpentOnInsurance { get; set; }

            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline, 
                    "Wealth:;cr;N0".T(EDTx.BankAccountClass_Wealth),  CurrentWealth, 
                    "Spent on Ships:;cr;N0".T(EDTx.BankAccountClass_SpentonShips), SpentOnShips,
                    "Spent on Outfitting:;cr;N0".T(EDTx.BankAccountClass_SpentonOutfitting), SpentOnOutfitting, 
                    "Spent on Repairs:;cr;N0".T(EDTx.BankAccountClass_SpentonRepairs), SpentOnRepairs, 
                    "Spent on Fuel:;cr;N0".T(EDTx.BankAccountClass_SpentonFuel), SpentOnFuel,
                    "Spent on Ammo:;cr;N0".T(EDTx.BankAccountClass_SpendonAmmo), SpentOnAmmoConsumables, 
                    "Insurance Claims:;;N0".T(EDTx.BankAccountClass_InsuranceClaims), InsuranceClaims, 
                    "Spent on Insurance:;cr;N0".T(EDTx.BankAccountClass_SpentonInsurance), SpentOnInsurance);
            }
        }

        public class CombatClass
        {
            public int BountiesClaimed { get; set; }
            public long BountyHuntingProfit { get; set; }
            public int CombatBonds { get; set; }
            public long CombatBondProfits { get; set; }
            public int Assassinations { get; set; }
            public long AssassinationProfits { get; set; }
            public long HighestSingleReward { get; set; }
            public int SkimmersKilled { get; set; }

            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Bounties :;;N0".T(EDTx.CombatClass_Bounties), BountiesClaimed, 
                    "Bounty Profits:;cr;N0".T(EDTx.CombatClass_BountyProfits), BountyHuntingProfit,
                    "Combat Bonds:;;N0".T(EDTx.CombatClass_CombatBonds), CombatBonds, 
                    "Combat Bond Profits:;cr;N0".T(EDTx.CombatClass_CombatBondProfits), CombatBondProfits,
                    "Assassinations:;;N0".T(EDTx.CombatClass_Assassinations), Assassinations, 
                    "Assassination Profits:;cr;N0".T(EDTx.CombatClass_AssassinationProfits), AssassinationProfits,
                    "Highest Reward:;cr;N0".T(EDTx.CombatClass_HighestReward), HighestSingleReward, 
                    "Skimmers Killed:;;N0".T(EDTx.CombatClass_SkimmersKilled), SkimmersKilled);
            }
        }

        public class CrimeClass
        {
            public double Notoriety { get; set; }
            public int Fines { get; set; }
            public long TotalFines { get; set; }
            public int BountiesReceived { get; set; }
            public long TotalBounties { get; set; }
            public long HighestBounty { get; set; }

            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Notoriety Index:;;N0".T(EDTx.CrimeClass_NotorietyIndex), Notoriety, 
                    "Fines:;;N0".T(EDTx.CrimeClass_Fines), Fines,
                    "Total Fines:;cr;N0".T(EDTx.CrimeClass_TotalFines), TotalFines, 
                    "Bounties:;;N0".T(EDTx.CrimeClass_Bounties), BountiesReceived,
                    "Total Bounties:;cr;N0".T(EDTx.CrimeClass_TotalBounties), TotalBounties, 
                    "Highest Bounty:;cr;N0".T(EDTx.CrimeClass_HighestBounty), HighestBounty);
            }
        }

        public class SmugglingClass
        {
            public int BlackMarketsTradedWith { get; set; }
            public long BlackMarketsProfits { get; set; }
            public int ResourcesSmuggled { get; set; }
            public double AverageProfit { get; set; }
            public long HighestSingleTransaction { get; set; }

            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Black Markets:;;N0".T(EDTx.SmugglingClass_BlackMarkets), BlackMarketsTradedWith, 
                    "Black Market Profits:;cr;N0".T(EDTx.SmugglingClass_BlackMarketProfits), BlackMarketsProfits,
                    "Resources Smuggled:;;N0".T(EDTx.SmugglingClass_ResourcesSmuggled), ResourcesSmuggled, 
                    "Average Profit:;cr;N0".T(EDTx.SmugglingClass_AverageProfit), AverageProfit,
                    "Highest Single Transaction:;cr;N0".T(EDTx.SmugglingClass_HighestSingleTransaction), HighestSingleTransaction);
            }
        }

        public class TradingClass
        {
            public int MarketsTradedWith { get; set; }
            public long MarketProfits { get; set; }
            public int ResourcesTraded { get; set; }
            public double AverageProfit { get; set; }
            public long HighestSingleTransaction { get; set; }

            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                        "Markets Traded:;;N0".T(EDTx.TradingClass_MarketsTraded), MarketsTradedWith, 
                        "Profits:;cr;N0".T(EDTx.TradingClass_Profits), MarketProfits, 
                        "No. of Resources:;;N0".T(EDTx.TradingClass_No), ResourcesTraded,
                        "Average Profit:;cr;N0".T(EDTx.TradingClass_AverageProfit), AverageProfit, 
                        "Highest Single Transaction:;cr;N0".T(EDTx.TradingClass_HighestSingleTransaction), HighestSingleTransaction);
            }
        }

        public class MiningClass
        {
            public long MiningProfits { get; set; }
            public int QuantityMined { get; set; }
            public int MaterialsCollected { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                        "Profits:;cr;N0".T(EDTx.MiningClass_Profits), MiningProfits, 
                        "Quantity:;;N0".T(EDTx.MiningClass_Quantity), QuantityMined, 
                        "Materials Types Collected:;;N0".T(EDTx.MiningClass_MaterialsTypesCollected), MaterialsCollected);


            }
        }

        public class ExplorationClass
        {
            public int SystemsVisited { get; set; }
            public long ExplorationProfits { get; set; }
            public int PlanetsScannedToLevel2 { get; set; }
            public int PlanetsScannedToLevel3 { get; set; }
            public long HighestPayout { get; set; }
            public long TotalHyperspaceDistance { get; set; }
            public int TotalHyperspaceJumps { get; set; }
            public double GreatestDistanceFromStart { get; set; }
            public int TimePlayed { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                        "Systems Visited:;;N0".T(EDTx.ExplorationClass_SystemsVisited), SystemsVisited, 
                        "Profits:;cr;N0".T(EDTx.ExplorationClass_Profits), ExplorationProfits,
                        "Level 2 Scans:;;N0".T(EDTx.ExplorationClass_Level2Scans), PlanetsScannedToLevel2, 
                        "Level 3 Scans:;;N0".T(EDTx.ExplorationClass_Level3Scans), PlanetsScannedToLevel3,
                        "Highest Payout:;cr;N0".T(EDTx.ExplorationClass_HighestPayout), HighestPayout, 
                        "Total Distance:;;N0".T(EDTx.ExplorationClass_TotalDistance), TotalHyperspaceDistance,
                        "No of Jumps:;;N0".T(EDTx.ExplorationClass_NoofJumps), TotalHyperspaceJumps,
                        "Greatest Distance:;;N0".T(EDTx.ExplorationClass_GreatestDistance), GreatestDistanceFromStart, 
                        "Time Played:".T(EDTx.ExplorationClass_TimePlayed), TimePlayed.SecondsToWeeksDaysHoursMinutesSeconds());
            }
        }

        public class PassengerMissionsClass
        {
            public int Bulk { get; set; }
            public int VIP { get; set; }
            public int Delivered { get; set; }
            public int Ejected { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Bulk Mission Passengers:;;N0".T(EDTx.PassengerMissionsClass_BulkMissionPassengers), Bulk, 
                    "VIP Mission Passengers:;;N0".T(EDTx.PassengerMissionsClass_VIPMissionPassengers), VIP, 
                    "Passengers Delivered:;;N0".T(EDTx.PassengerMissionsClass_PassengersDelivered), Delivered, 
                    "Passengers Ejected:;;N0".T(EDTx.PassengerMissionsClass_PassengersEjected), Ejected);
            }
        }

        public class SearchAndRescueClass
        {
            public long Traded { get; set; }
            public long Profit { get; set; }
            public int Count { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Total Items Rescued:;;N0".T(EDTx.SearchAndRescueClass_TotalItemsRescued), Traded, 
                    "Profit:;cr;N0".T(EDTx.SearchAndRescueClass_Profit), Profit, 
                    "Total Rescue Transactions:;;N0".T(EDTx.SearchAndRescueClass_TotalRescueTransactions), Count);
            }
        }

        public class CraftingClass
        {
            public int CountOfUsedEngineers { get; set; }
            public int RecipesGenerated { get; set; }
            public int RecipesGeneratedRank1 { get; set; }
            public int RecipesGeneratedRank2 { get; set; }
            public int RecipesGeneratedRank3 { get; set; }
            public int RecipesGeneratedRank4 { get; set; }
            public int RecipesGeneratedRank5 { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Engineers Used:;;N0".T(EDTx.CraftingClass_EngineersUsed), CountOfUsedEngineers, 
                    "Blueprints:;;N0".T(EDTx.CraftingClass_Blueprints), RecipesGenerated,
                    "At Level 1:;;N0".T(EDTx.CraftingClass_AtLevel1), RecipesGeneratedRank1, 
                    "At Level 2:;;N0".T(EDTx.CraftingClass_AtLevel2), RecipesGeneratedRank2, 
                    "At Level 3:;;N0".T(EDTx.CraftingClass_AtLevel3), RecipesGeneratedRank3,
                    "At Level 4:;;N0".T(EDTx.CraftingClass_AtLevel4), RecipesGeneratedRank4, 
                    "At Level 5:;;N0".T(EDTx.CraftingClass_AtLevel5), RecipesGeneratedRank5);
            }
        }

        public class CrewClass
        {
            public long TotalWages { get; set; }
            public int Hired { get; set; }
            public int Fired { get; set; }
            public int Died { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Total Wages:;cr;N0".T(EDTx.CrewClass_TotalWages), TotalWages, "Hired:;;N0".T(EDTx.CrewClass_Hired), Hired, 
                    "Fired:;;N0".T(EDTx.CrewClass_Fired), Fired, "Killed in Action:;;N0".T(EDTx.CrewClass_KilledinAction), Died);
            }
        }

        public class MulticrewClass
        {
            public int TimeTotal { get; set; }
            public int GunnerTimeTotal { get; set; }
            public int FighterTimeTotal { get; set; }
            public long CreditsTotal { get; set; }
            public long FinesTotal { get; set; }

            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Total Time:".T(EDTx.MulticrewClass_TotalTime), TimeTotal.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "Gunner Time:;;N0".T(EDTx.MulticrewClass_GunnerTime), GunnerTimeTotal.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "Fighter Time:;;N0".T(EDTx.MulticrewClass_FighterTime), FighterTimeTotal.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "Credits:;cr;N0".T(EDTx.MulticrewClass_Credits), CreditsTotal,
                    "Fines:;cr;N0".T(EDTx.MulticrewClass_Fines), FinesTotal);
            }
        }

        public class MaterialTraderStatsClass
        {
            public int TradesCompleted { get; set; }
            public int MaterialsTraded { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Commodity Trades:;;N0".T(EDTx.MaterialTraderStatsClass_CommodityTrades), TradesCompleted, "Material Traded:;;N0".T(EDTx.MaterialTraderStatsClass_MaterialTraded), MaterialsTraded);
            }
        }

        public class CQCClass
        {
            public int TimePlayed { get; set; }
            public double KD { get; set; }
            public int Kills { get; set; }
            public double WL { get; set; }

            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Time Played:;;N0".T(EDTx.CQCClass_TimePlayed), TimePlayed.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "KD Ratio:;;N".T(EDTx.CQCClass_KDRatio), KD,
                    "Kills:;;N0".T(EDTx.CQCClass_Kills), Kills,
                    "Win/Loss:;;N".T(EDTx.CQCClass_Win), WL);
            }
        }
    }
}
