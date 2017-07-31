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
    public class CLastSystem
    {
        public long id { get; private set; }
        public string name { get; private set; }
        public string faction { get; private set; }

        public CLastSystem(JObject jo)
        {
            FromJson(jo);
        }

        public bool FromJson(JObject jo)
        {
            try
            {
                id = jo["id"].Long();
                name = jo["name"].Str();
                faction = jo["faction"].Str();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
