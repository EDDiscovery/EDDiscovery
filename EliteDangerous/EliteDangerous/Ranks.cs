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

 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EliteDangerousCore
{
    public enum CombatRank
    {
        Unknown = -1,
        Harmless = 0,
        MostlyHarmless,
        Novice,
        Competent,
        Expert,
        Master,
        Dangerous,
        Deadly,
        Elite
    }

    public enum TradeRank
    {
        Unknown = -1,
        Penniless = 0,
        MostlyPenniless,
        Peddler,
        Dealer,
        Merchant,
        Broker,
        Entrepreneur,
        Tycoon,
        Elite
    }

    public enum ExplorationRank
    {
        Unknown = -1,
        Aimless = 0,
        MostlyAimless,
        Scout,
        Surveyor,
        Explorer,
        Pathfinder,
        Ranger,
        Pioneer,
        Elite
    }

    public enum FederationRank
    {
        Unknown = -1,
        None = 0,
        Recruit,
        Cadet,
        Midshipman,
        PettyOfficer,
        ChiefPettyOfficer,
        WarrantOfficer,
        Ensign,
        Lieutenant,
        LtCommander,
        PostCommander,
        PostCaptain,
        RearAdmiral,
        ViceAdmiral,
        Admiral
    }

    public enum EmpireRank
    {
        Unknown = -1,
        None = 0,
        Outsider,
        Serf,
        Master,
        Squire,
        Knight,
        Lord,
        Baron,
        Viscount,
        Count,
        Earl,
        Marquis,
        Duke,
        Prince,
        King
    }

    public enum CQCRank
    {
        Unknown = -1,
        Helpless = 0,
        MostlyHelpless,
        Amateur,
        SemiProfessional,
        Professional,
        Champion,
        Hero,
        Legend,
        Elite
    }

    public enum SquadronRank       // these, as of 1/11/2018, are provisional
    {
        Unknown = -1,
        Leader = 0,
        SeniorOfficer = 1,
        Officer = 2,
        Agent = 3,
        Rookie = 4,
    }
}
