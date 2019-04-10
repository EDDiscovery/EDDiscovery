/*
 * Copyright © 2015 - 2019 EDDiscovery development team
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

namespace EliteDangerousCore
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
        Famine,
        Election,
        Retreat,
        Investment,
    }

    public enum EDSecurity
    {
        Unknown = 0,
        Low,
        Medium,
        High,
        Anarchy,
        Lawless,
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

    public enum SystemStatusEnum                // Who made the information?
    {
        Unknown = 0,
        EDSM = 1,
        EDDiscovery = 3,
    }

    public interface ISystemBase : IEquatable<ISystemBase>
    {
        long EDSMID { get; set; }

        string Name { get; set; }
        double X { get; set; }
        int Xi { get; set; }
        double Y { get; set; }
        int Yi { get; set; }
        double Z { get; set; }
        int Zi { get; set; }
        bool HasCoordinate { get; }
        int GridID { get; set; }
        long? SystemAddress { get; set; }

        double Distance(ISystemBase other);
        double Distance(double x, double y, double z);
        bool Distance(ISystemBase other, double min, double max);
        bool Cuboid(ISystemBase other, double min, double max);
    }

    public interface ISystemEDDB
    {
        long EDDBID { get; set; }
        string Faction { get; set; }
        long Population { get; set; }
        EDGovernment Government { get; set; }
        EDAllegiance Allegiance { get; set; }
        EDState State { get; set; }
        EDSecurity Security { get; set; }
        EDEconomy PrimaryEconomy { get; set; }
        string Power { get; set; }
        string PowerState { get; set; }
        int NeedsPermit { get; set; }
        int EDDBUpdatedAt { get; set; }
        bool HasEDDBInformation { get; }
    }

    // Definition of the core interface so we can swap out an "offline" version during testing
    public interface ISystem : ISystemBase, ISystemEDDB
    {
        SystemStatusEnum status { get; set; }        // Who made this entry, where did the info come from?
        string ToString();
        string ToStringVerbose();
    }
}
