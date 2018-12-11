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
            info = BaseUtils.FieldBuilder.Build("Wealth:;cr;N".Txb(this), BankAccount.CurrentWealth, "Notoriety Index:;;N0".Txb(this), Crime.Notoriety);

            detailed =  "Bank Account".Tx(this) + Environment.NewLine + BankAccount?.Format() + Environment.NewLine + 
                        "Combat".Tx(this) + Environment.NewLine + Combat?.Format() + Environment.NewLine +
                        "Crime".Tx(this) + Environment.NewLine + Crime?.Format() + Environment.NewLine +
                        "Smuggling".Tx(this) + Environment.NewLine + Smuggling?.Format() + Environment.NewLine +
                        "Trading".Tx(this) + Environment.NewLine + Trading?.Format() + Environment.NewLine +
                        "Mining".Tx(this) + Environment.NewLine + Mining?.Format() + Environment.NewLine +
                        "Exploration".Tx(this) + Environment.NewLine + Exploration?.Format() + Environment.NewLine +
                        "Passengers".Tx(this) + Environment.NewLine + PassengerMissions?.Format() + Environment.NewLine +
                        "Search and Rescue".Tx(this) + Environment.NewLine + SearchAndRescue?.Format() + Environment.NewLine +
                        "Engineers".Tx(this) + Environment.NewLine + Crafting?.Format() + Environment.NewLine +
                        "Crew".Tx(this) + Environment.NewLine + Crew?.Format() + Environment.NewLine +
                        "Multicrew".Tx(this) + Environment.NewLine + Multicrew?.Format() + Environment.NewLine +
                        "Materials and Commodity Trading".Tx(this) + Environment.NewLine + MaterialTraderStats?.Format() + Environment.NewLine +
                        "CQC".Tx(this) + Environment.NewLine + CQC?.Format();
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
                    "Wealth:;cr;N0".Tx(this),  CurrentWealth, 
                    "Spent on Ships:;cr;N0".Tx(this), SpentOnShips,
                    "Spent on Outfitting:;cr;N0".Tx(this), SpentOnOutfitting, 
                    "Spent on Repairs:;cr;N0".Tx(this), SpentOnRepairs, 
                    "Spent on Fuel:;cr;N0".Tx(this), SpentOnFuel,
                    "Spend on Ammo:;cr;N0".Tx(this), SpentOnAmmoConsumables, 
                    "Insurance Claims:;;N0".Tx(this), InsuranceClaims, 
                    "Spent on Insurance:;cr;N0".Tx(this), SpentOnInsurance);
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
                    "Bounties :;;N0".Tx(this), BountiesClaimed, 
                    "Bounty Profits:;cr;N0".Tx(this), BountyHuntingProfit,
                    "Combat Bonds:;;N0".Tx(this), CombatBonds, 
                    "Combat Bond Profits:;cr;N0".Tx(this), CombatBondProfits,
                    "Assassinations:;;N0".Tx(this), Assassinations, 
                    "Assassination Profits:;cr;N0".Tx(this), AssassinationProfits,
                    "Highest Reward:;cr;N0".Tx(this), HighestSingleReward, 
                    "Skimmers Killed:;;N0".Tx(this), SkimmersKilled);
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
                    "Notoriety Index:;;N0".Tx(this), Notoriety, 
                    "Fines:;;N0".Tx(this), Fines,
                    "Total Fines:;cr;N0".Tx(this), TotalFines, 
                    "Bounties:;;N0".Tx(this), BountiesReceived,
                    "Total Bounties:;cr;N0".Tx(this), TotalBounties, 
                    "Highest Bounty:;cr;N0".Tx(this), HighestBounty);
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
                    "Black Markets:;;N0".Tx(this), BlackMarketsTradedWith, 
                    "Black Market Profits:;cr;N0".Tx(this), BlackMarketsProfits,
                    "Resources Smuggled:;;N0".Tx(this), ResourcesSmuggled, 
                    "Average Profit:;cr;N0".Tx(this), AverageProfit,
                    "Highest Single Transaction:;cr;N0".Tx(this), HighestSingleTransaction);
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
                        "Markets Traded:;;N0".Tx(this), MarketsTradedWith, 
                        "Profits:;cr;N0".Tx(this), MarketProfits, 
                        "No. of Resources:;;N0".Tx(this), ResourcesTraded,
                        "Average Profit:;cr;N0".Tx(this), AverageProfit, 
                        "Highest Single Transaction:;cr;N0".Tx(this), HighestSingleTransaction);
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
                        "Profits:;cr;N0".Tx(this), MiningProfits, 
                        "Quantity:;;N0".Tx(this), QuantityMined, 
                        "Materials Types Collected:;;N0".Tx(this), MaterialsCollected);


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
                        "Systems Visited:;;N0".Tx(this), SystemsVisited, 
                        "Profits:;cr;N0".Tx(this), ExplorationProfits,
                        "Level 2 Scans:;;N0".Tx(this), PlanetsScannedToLevel2, 
                        "Level 3 Scans:;;N0".Tx(this), PlanetsScannedToLevel3,
                        "Highest Payout:;cr;N0".Tx(this), HighestPayout, 
                        "Total Distance:;;N0".Tx(this), TotalHyperspaceDistance,
                        "No of Jumps:;;N0".Tx(this), TotalHyperspaceJumps,
                        "Greatest Distance:;;N0".Tx(this), GreatestDistanceFromStart, 
                        "Time Played:".Tx(this), TimePlayed.SecondsToWeeksDaysHoursMinutesSeconds());
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
                    "Bulk Mission Passengers:;;N0".Tx(this), Bulk, 
                    "VIP Mission Passengers:;;N0".Tx(this), VIP, 
                    "Passengers Delivered:;;N0".Tx(this), Delivered, 
                    "Passengers Ejected:;;N0".Tx(this), Ejected);
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
                    "Total Items Rescued:;;N0".Tx(this), Traded, 
                    "Profit:;cr;N0".Tx(this), Profit, 
                    "Total Rescue Transactions:;;N0".Tx(this), Count);
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
                    "Engineers Used:;;N0".Tx(this), CountOfUsedEngineers, 
                    "Blueprints:;;N0".Tx(this), RecipesGenerated,
                    "At Level 1:;;N0".Tx(this), RecipesGeneratedRank1, 
                    "At Level 2:;;N0".Tx(this), RecipesGeneratedRank2, 
                    "At Level 3:;;N0".Tx(this), RecipesGeneratedRank3,
                    "At Level 4:;;N0".Tx(this), RecipesGeneratedRank4, 
                    "At Level 5:;;N0".Tx(this), RecipesGeneratedRank5);
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
                    "Total Wages:;cr;N0".Tx(this), TotalWages, "Hired:;;N0".Tx(this), Hired, 
                    "Fired:;;N0".Tx(this), Fired, "Killed in Action:;;N0".Tx(this), Died);
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
                    "Total Time:".Tx(this), TimeTotal.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "Gunner Time:;;N0".Tx(this), GunnerTimeTotal.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "Fighter Time:;;N0".Tx(this), FighterTimeTotal.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "Credits:;cr;N0".Tx(this), CreditsTotal,
                    "Fines:;cr;N0".Tx(this), FinesTotal);
            }
        }

        public class MaterialTraderStatsClass
        {
            public int TradesCompleted { get; set; }
            public int MaterialsTraded { get; set; }
            public string Format(string frontline = "    ")
            {
                return frontline + BaseUtils.FieldBuilder.BuildSetPad(Environment.NewLine + frontline,
                    "Commodity Trades:;;N0".Tx(this), TradesCompleted, "Material Traded:;;N0".Tx(this), MaterialsTraded);
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
                    "Time Played:;;N0".Tx(this), TimePlayed.SecondsToWeeksDaysHoursMinutesSeconds(),
                    "KD Ratio:;;N".Tx(this), KD,
                    "Kills:;;N0".Tx(this), Kills,
                    "Win/Loss:;;N".Tx(this), WL);
            }
        }
    }
}
