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
 * EDDiscovery is not affiliated with Fronter Developments plc.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.DB
{
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
