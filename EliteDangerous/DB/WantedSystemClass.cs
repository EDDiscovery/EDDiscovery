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

using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace EliteDangerousCore.DB
{
    public class WantedSystemClass
    {
        public long id;
        public string system;

        public WantedSystemClass()
        { }

        public WantedSystemClass(DbDataReader dr)
        {
            id = (long)dr["id"];
            system = (string)dr["systemname"];
        }

        public WantedSystemClass(string SystemName)
        {
            system = SystemName;
        }

        public bool Add()
        {
            id = UserDatabase.Instance.Add<long>("wanted_systems", "id", new Dictionary<string, object>
            {
                ["systemname"] = system
            });

            return true;
        }

        public bool Delete()
        {
            UserDatabase.Instance.Delete("wanted_systems", "id", id);
            return true;
        }

        public static List<WantedSystemClass> GetAllWantedSystems()     // CAN return null
        {
            try
            {
                return UserDatabase.Instance.Retrieve("wanted_systems", rdr => new WantedSystemClass(rdr));
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception " + ex.ToString());
                return null;
            }
        }
    }
}
