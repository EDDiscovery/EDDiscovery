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
using System;
using System.Linq;

namespace EliteDangerousCore.JournalEvents
{
    //When written: loading the game
    //Parameters: statistics broken down by group
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

        public override void FillInformation(out string info, out string detailed) //V
        {
            info = BaseUtils.FieldBuilder.Build("Wealth:;cr;N", BankAccount.CurrentWealth, "Notoriety Index:;;N0" , Crime.Notoriety);

            detailed =  "Bank Account" + Environment.NewLine + BankAccount?.Format() + Environment.NewLine + 
                        "Combat" + Environment.NewLine + Combat?.Format() + Environment.NewLine +
                        "Crime" + Environment.NewLine + Crime?.Format() + Environment.NewLine +
                        "Smuggling" + Environment.NewLine + Smuggling?.Format() + Environment.NewLine +
                        "Trading" + Environment.NewLine + Trading?.Format() + Environment.NewLine +
                        "Mining" + Environment.NewLine + Mining?.Format() + Environment.NewLine +
                        "Exploration" + Environment.NewLine + Exploration?.Format() + Environment.NewLine +
                        "Passengers" + Environment.NewLine + PassengerMissions?.Format() + Environment.NewLine +
                        "Search and Rescue" + Environment.NewLine + SearchAndRescue?.Format() + Environment.NewLine +
                        "Engineers" + Environment.NewLine + Crafting?.Format() + Environment.NewLine +
                        "Crew" + Environment.NewLine + Crew?.Format() + Environment.NewLine +
                        "Multicrew" + Environment.NewLine + Multicrew?.Format() + Environment.NewLine +
                        "Materials and Commodity Trading" + Environment.NewLine + MaterialTraderStats?.Format() + Environment.NewLine +
                        "CQC" + Environment.NewLine + CQC?.Format();
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
                    "Wealth:;cr;N0", CurrentWealth, "Spent on Ships:;cr;N0", SpentOnShips,
                    "Spent on Outfitting:;cr;N0", SpentOnOutfitting, "Spent on Repairs:;cr;N0", SpentOnRepairs, "Spent on Fuel:;cr;N0", SpentOnFuel,
                    "Spend on Ammo:;cr;N0", SpentOnAmmoConsumables, "Insurance Claims:;;N0", InsuranceClaims, "Spent on Insurance:;cr;N0", SpentOnInsurance);
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
                    "Bounties :;;N0", BountiesClaimed, "Bounty Profits:;cr;N0", BountyHuntingProfit,
                    "Combat Bonds:;;N0", CombatBonds, "Combat Bond Profits:;cr;N0", CombatBondProfits,
                    "Assassinations:;;N0", Assassinations, "Assassination Profits:;cr;N0", AssassinationProfits,
                    "Highest Reward:;cr;N0", HighestSingleReward, "Skimmers Killed:;;N0", SkimmersKilled);
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
                    "Notoriety Index:;;N0", Notoriety, "Fines:;;N0", Fines,
                    "Total Fines:;cr;N0", TotalFines, "Bounties:;;N0", BountiesReceived,
                    "Total Bounties:;cr;N0", TotalBounties, "Highest Bounty:;cr;N0", HighestBounty);
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
                    "Black Markets:;;N0", BlackMarketsTradedWith, "Black Market Profits:;cr;N0", BlackMarketsProfits,
                    "Resources Smuggled:;;N0", ResourcesSmuggled, "Average Profit:;cr;N0", AverageProfit,
                    "Highest Single Transaction:;cr;N0", HighestSingleTransaction);
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
                        "Markets Traded:;;N0", MarketsTradedWith, "Profits:;cr;N0", MarketProfits, "No. of Resources:;;N0", ResourcesTraded,
                        "Average Profit:;cr;N0", AverageProfit, "Highest Single Transaction:;cr;N0", HighestSingleTransaction);
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
                        "Profits:;cr;N0", MiningProfits, "Quantity:;;N0", QuantityMined, "Materials Types Collected:;;N0", MaterialsCollected);


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
                        "Systems Visited:;;N0", SystemsVisited, "Profits:;cr;N0", ExplorationProfits,
                        "Level 2 Scans:;;N0", PlanetsScannedToLevel2, "Level 3 Scans:;;N0", PlanetsScannedToLevel3,
                        "Highest Payout:;cr;N0", HighestPayout, "Total Distance:;;N0", TotalHyperspaceDistance,
                        "No of Jumps:;;N0", TotalHyperspaceJumps,
                        "Greatest Distance:;;N0", GreatestDistanceFromStart, "Time Played:", TimePlayed.SecondsToWeeksDaysHoursMinutesSeconds());
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
                    "Bulk Mission Passengers:;;N0", Bulk, "VIP Mission Passengers:;;N0", VIP, "Passengers Delivered:;;N0", Delivered, "Passengers Ejected:;;N0", Ejected);
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
                    "Total Items Rescued:;;N0", Traded, "Profit:;cr;N0", Profit, "Total Rescue Transactions:;;N0", Count);
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
                    "Engineers Used:;;N0", CountOfUsedEngineers, "Blueprints:;;N0", RecipesGenerated,
                    "At Level 1:;;N0", RecipesGeneratedRank1, "At Level 2:;;N0", RecipesGeneratedRank2, "At Level 3:;;N0", RecipesGeneratedRank3,
                    "At Level 4:;;N0", RecipesGeneratedRank4, "At Level 5:;;N0", RecipesGeneratedRank5);
            }
        }

        public class CrewClass
        {
            public long NpcCrewTotalWages { get; set; }
            public int NpcCrewHired { get; set; }
            public int NpcCrewFired { get; set; }
            public int NpcCrewDied { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Total Wages:;cr;N0", NpcCrewTotalWages, "Hired:;;N0", NpcCrewHired, "Fired:;;N0", NpcCrewFired, "Killed in Action:;;N0", NpcCrewDied);
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
                    "Total Time:", TimeTotal.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "Gunner Time:;;N0", GunnerTimeTotal.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "Fighter Time:;;N0", FighterTimeTotal.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "Credits:;cr;N0", CreditsTotal,
                    "Fines:;cr;N0", FinesTotal);
            }
        }

        public class MaterialTraderStatsClass
        {
            public int TradesCompleted { get; set; }
            public int MaterialsTraded { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Commodity Trades:;;N0", TradesCompleted, "Material Traded:;;N0", MaterialsTraded);
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
                    "Time Played:;;N0", TimePlayed.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "KD Ratio:;;N", KD,
                    "Kills:;;N0", Kills,
                    "Win/Loss:;;N", WL);
            }
        }
    }
}
