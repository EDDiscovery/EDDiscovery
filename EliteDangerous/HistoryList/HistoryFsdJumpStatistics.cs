﻿/*
 * Copyright © 2020 EDDiscovery development team
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

namespace EliteDangerousCore
{
    public struct HistoryFsdJumpStatistics
    {
        public int Count;
        public double Distance;
        public int BasicBoosts;
        public int StandardBoosts;
        public int PremiumBoosts;

        public HistoryFsdJumpStatistics(int count, double distance, int basicBoosts, int standardBoosts, int premiumBoosts)
        {
            Count = count;
            Distance = distance;
            BasicBoosts = basicBoosts;
            StandardBoosts = standardBoosts;
            PremiumBoosts = premiumBoosts;
        }
    }
}
