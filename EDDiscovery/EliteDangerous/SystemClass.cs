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

namespace EDDiscovery.EliteDangerous
{
    // For when you need a minimal version and don't want to mess up the database. 
    // Useful for creation of test doubles
    public class SystemClassBase : ISystemBase
    {
        public long id { get; set; }
        public long id_edsm { get; set; }
        public string name { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public DateTime UpdateDate { get; set; }
        public int gridid { get; set; }
        public int randomid { get; set; }

        public bool HasCoordinate
        {
            get
            {
                return (!double.IsNaN(x));
            }
        }

        public bool Equals(ISystemBase other)
        {
            return other != null &&
                   other.name.Equals(this.name, StringComparison.InvariantCultureIgnoreCase) &&
                   (!this.HasCoordinate || !other.HasCoordinate ||
                    (Math.Abs(this.x - other.x) < 0.125 &&
                     Math.Abs(this.y - other.y) < 0.125 &&
                     Math.Abs(this.z - other.z) < 0.125));
        }
    }

    public class SystemClass : SystemClassBase, ISystem
    {
        public int cr { get; set; }
        public string CommanderCreate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CommanderUpdate { get; set; }
        public EDDiscovery.DB.SystemStatusEnum status { get; set; }
        public string SystemNote { get; set; }

        public long id_eddb { get; set; }
        public string faction { get; set; }
        public long population { get; set; }
        public EDGovernment government { get; set; }
        public EDAllegiance allegiance { get; set; }
        public EDState state { get; set; }
        public EDSecurity security { get; set; }
        public EDEconomy primary_economy { get; set; }
        public int needs_permit { get; set; }
        public int eddb_updated_at { get; set; }

        public bool HasEDDBInformation
        {
            get
            {
                return population != 0 || government != EDGovernment.Unknown || needs_permit != 0 || allegiance != EDAllegiance.Unknown ||
                       state != EDState.Unknown || security != EDSecurity.Unknown || primary_economy != EDEconomy.Unknown || (faction != null && faction.Length>0); 
            }
        }
    }
}
