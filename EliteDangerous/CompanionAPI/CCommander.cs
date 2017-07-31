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

namespace EliteDangerousCore.CompanionAPI
{
    public class CCommander
    {
        public int id { get; private set; }
        public string name { get; private set; }
        public long credits { get; private set; }
        public long debt { get; private set; }
        public int currentShipId { get; private set; }
        public bool alive { get; private set; }
        public bool docked { get; private set; }

        public CombatRank combatrank { get; private set; }
        public TradeRank traderank { get; private set; }
        public ExplorationRank explorationrank { get; private set; }
        public int crimeRank { get; private set; }  // ?
        public int serviceRank { get; private set; } //?
        public EmpireRank empirerank { get; private set; }
        public FederationRank federationrank { get; private set; }
        public int powerRank{ get; private set; }
        public CQCRank CQCRank { get; private set; }

        public CCommander(JObject json)
        {
            id = json["id"].Int();
            name =  json["name"].Str();
            credits = json["credits"].Long();
            debt = json["debt"].Long();
            currentShipId = json["currentShipId"].Int();
            alive = json["alive"].Bool();
            docked = json["docked"].Bool();

            combatrank = (CombatRank)json["rank"]["combat"].Int();
            traderank = (TradeRank)json["rank"]["trade"].Int();
            explorationrank = (ExplorationRank)json["rank"]["explore"].Int();
            crimeRank = json["rank"]["crime"].Int();
            serviceRank = json["rank"]["service"].Int();
            empirerank = (EmpireRank)json["rank"]["empire"].Int();
            federationrank = (FederationRank)json["rank"]["federation"].Int();
            powerRank = json["rank"]["power"].Int();
            CQCRank = (CQCRank)json["rank"]["cqc"].Int();

        }
    }
 }