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

        public override void FillInformation(out string summary, out string info, out string detailed) //V
        {
            summary = EventTypeStr.SplitCapsWord();
            info = "";
            detailed = "";
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
        }

        public class CrimeClass
        {
            public double Notoriety { get; set; }
            public int Fines { get; set; }
            public long TotalFines { get; set; }
            public int BountiesReceived { get; set; }
            public long TotalBounties { get; set; }
            public long HighestBounty { get; set; }
        }

        public class SmugglingClass
        {
            public int BlackMarketsTradedWith { get; set; }
            public long BlackMarketsProfits { get; set; }
            public int ResourcesSmuggled { get; set; }
            public double AverageProfit { get; set; }
            public long HighestSingleTransaction { get; set; }
        }

        public class TradingClass
        {
            public int MarketsTradedWith { get; set; }
            public long MarketProfits { get; set; }
            public int ResourcesTraded { get; set; }
            public double AverageProfit { get; set; }
            public long HighestSingleTransaction { get; set; }
        }

        public class MiningClass
        {
            public long MiningProfits { get; set; }
            public int QuantityMined { get; set; }
            public int MaterialsCollected { get; set; }
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
            public string TimePlayedDisplay()
            {
                TimeSpan s = TimeSpan.FromSeconds(TimePlayed);
                int days = s.Days % 7;
                int weeks = (s.Days - days) / 7;
                return $"{weeks} weeks {days} days {s.Hours} hours";
            }
        }

        public class PassengerMissionsClass
        {
            public int Bulk { get; set; }
            public int VIP { get; set; }
            public int Delivered { get; set; }
            public int Ejected { get; set; }
        }

        public class SearchAndRescueClass
        {
            public long Traded { get; set; }
            public long Profit { get; set; }
            public int Count { get; set; }
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
        }

        public class CrewClass
        {
            public long NpcCrewTotalWages { get; set; }
            public int NpcCrewHired { get; set; }
            public int NpcCrewFired { get; set; }
            public int NpcCrewDied { get; set; }
        }

        public class MulticrewClass
        {
            public int TimeTotal { get; set; }
            public int GunnerTimeTotal { get; set; }
            public int FighterTimeTotal { get; set; }
            public long CreditsTotal { get; set; }
            public long FinesTotal { get; set; }
        }

        public class MaterialTraderStatsClass
        {
            public int TradesCompleted { get; set; }
            public int MaterialsTraded { get; set; }
        }

        public class CQCClass
        {
            public int TimePlayed { get; set; }
            public double KD { get; set; }
            public int Kills { get; set; }
            public double WL { get; set; }
        }
    }
}
