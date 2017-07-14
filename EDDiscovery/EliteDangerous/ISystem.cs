/*
 * Copyright © 2015 - 2016 EDDiscovery development team
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

namespace EDDiscovery.EliteDangerous
{
    public enum EDGovernment
    {
        Unknown = 0,
        Anarchy = 1,
        Communism = 2,
        Confederacy = 3,
        Corporate = 4,
        Cooperative = 5,
        Democracy = 6,
        Dictatorship,
        Feudal,
        Imperial,
        Patronage,
        Colony,
        Prison_Colony,
        Theocracy,
        None,

    }

    public enum EDAllegiance
    {
        Unknown = 0,
        Alliance = 1,
        Anarchy = 2,
        Empire = 3,
        Federation = 4,
        Independent = 5,
        None = 6,
    }

    public enum EDState
    {
        Unknown = 0,
        None = 1,
        Boom,
        Bust,
        Civil_Unrest,
        Civil_War,
        Expansion,
        Lockdown,
        Outbreak,
        War,
    }

    public enum EDSecurity
    {
        Unknown = 0,
        Low,
        Medium,
        High,
    }

    public enum EDStationType
    {
        Unknown = 0,
        Civilian_Outpost = 1,
        Commercial_Outpost = 2,
        Coriolis_Starport = 3,
        Industrial_Outpost = 4,
        Military_Outpost = 5,
        Mining_Outpost = 6,
        Ocellus_Starport = 7,
        Orbis_Starport = 8,
        Scientific_Outpost = 9,
        Unsanctioned_Outpost = 10,
        Unknown_Outpost = 11,
        Unknown_Starport = 12,
    }

    public enum EDEconomy
    {
        Unknown = 0,
        Agriculture = 1,
        Extraction = 2,
        High_Tech = 3,
        Industrial = 4,
        Military = 5,
        Refinery = 6,
        Service = 7,
        Terraforming = 8,
        Tourism = 9,
        None = 10,

    }

    public interface ISystemBase : IEquatable<ISystemBase>
    {
        long id { get; set; }
        long id_edsm { get; set; }

        string name { get; set; }
        double x { get; set; }
        double y { get; set; }
        double z { get; set; }
        DateTime UpdateDate { get; set; }
        bool HasCoordinate { get; }
        int gridid { get; set; }
        int randomid { get; set; }
    }


    public interface ISystemEDDB
    {

        long id_eddb { get; set; }
        string faction { get; set; }
        long population { get; set; }
        EDGovernment government { get; set; }
        EDAllegiance allegiance { get; set; }
        EDState state { get; set; }
        EDSecurity security { get; set; }
        EDEconomy primary_economy { get; set; }
        int needs_permit { get; set; }
        int eddb_updated_at { get; set; }
        bool HasEDDBInformation { get; }
    }

    // Definition of the core interface so we can swap out an "offline" version during testing
    public interface ISystem : ISystemBase, ISystemEDDB
    {
        int cr { get; set; }
        string CommanderCreate { get; set; }
        DateTime CreateDate { get; set; }
        string CommanderUpdate { get; set; }
        EDDiscovery.DB.SystemStatusEnum status { get; set; }        // Who made this entry, where did the info come from?
        string SystemNote { get; set; }
    }
}
