/*
 * Copyright © 2016-2019 EDDiscovery development team
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

 using System.Linq;

namespace EliteDangerousCore
{
    public static class JournalEssentialEvents
    {
        static public JournalTypeEnum[] EssentialEvents = new JournalTypeEnum[]     // 
            {
                // due to materials/commodities
                JournalTypeEnum.Cargo, JournalTypeEnum.CargoDepot,JournalTypeEnum.CollectCargo,
                JournalTypeEnum.EjectCargo,
                JournalTypeEnum.EngineerContribution,
                JournalTypeEnum.EngineerCraft, JournalTypeEnum.MarketBuy, JournalTypeEnum.MarketSell,
                JournalTypeEnum.MaterialCollected, JournalTypeEnum.MaterialDiscarded, JournalTypeEnum.Materials, JournalTypeEnum.MaterialTrade,
                JournalTypeEnum.Synthesis, JournalTypeEnum.TechnologyBroker,

                // Missions
                JournalTypeEnum.MissionAccepted, JournalTypeEnum.MissionCompleted, JournalTypeEnum.MissionAbandoned, JournalTypeEnum.MissionFailed, JournalTypeEnum.MissionRedirected,

                // Combat
                JournalTypeEnum.Bounty, JournalTypeEnum.CommitCrime, JournalTypeEnum.FactionKillBond,  JournalTypeEnum.PVPKill,
                JournalTypeEnum.Died, JournalTypeEnum.Resurrect, JournalTypeEnum.SelfDestruct, 

                // Journey
                JournalTypeEnum.FSDJump, JournalTypeEnum.CarrierJump, JournalTypeEnum.Location, JournalTypeEnum.Docked,

                // Ship state
                JournalTypeEnum.Loadout, JournalTypeEnum.MassModuleStore, JournalTypeEnum.ModuleBuy, JournalTypeEnum.ModuleSell,
                JournalTypeEnum.ModuleRetrieve,
                JournalTypeEnum.ModuleSellRemote, JournalTypeEnum.ModuleStore, JournalTypeEnum.ModuleSwap, JournalTypeEnum.SellShipOnRebuy,
                JournalTypeEnum.SetUserShipName, JournalTypeEnum.ShipyardBuy, JournalTypeEnum.ShipyardNew, JournalTypeEnum.ShipyardSell,
                JournalTypeEnum.ShipyardSwap , JournalTypeEnum.ShipyardTransfer, JournalTypeEnum.StoredModules, JournalTypeEnum.StoredShips,

                // scan
                JournalTypeEnum.Scan, JournalTypeEnum.SellExplorationData, 

                // misc
                JournalTypeEnum.ClearSavedGame,
            };

        static public JournalTypeEnum[] FullStatsEssentialEvents
        {
            get
            {
                var statsAdditional = new JournalTypeEnum[]
                {
                    // Travel
                    JournalTypeEnum.JetConeBoost, JournalTypeEnum.SAAScanComplete
                };
                return EssentialEvents.Concat(statsAdditional).ToArray();
            }
        }

        static public JournalTypeEnum[] JumpScanEssentialEvents = new JournalTypeEnum[]     // 
            {
                JournalTypeEnum.FSDJump,JournalTypeEnum.CarrierJump,
                JournalTypeEnum.Scan,
            };
        static public JournalTypeEnum[] JumpEssentialEvents = new JournalTypeEnum[]     // 
            {
                JournalTypeEnum.FSDJump,JournalTypeEnum.CarrierJump
            };

        static public JournalTypeEnum[] NoEssentialEvents = new JournalTypeEnum[]     // 
            {
            };
    }
}

