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
using System.Collections.Generic;

namespace EliteDangerousCore.CompanionAPI
{
    /// <summary>
    /// Profile information returned by the companion app service
    /// </summary>
    public class CProfile
    {
        /// <summary>The commander</summary>
        public CCommander Cmdr { get; set; }

        /// <summary>The commander's current ship</summary>
        public CShip Ship { get; set; }

        /// <summary>The commander's stored ships</summary>
        public List<CShip> Ships { get; set; }

        /// <summary>The current starsystem</summary>
        public CLastSystem CurrentStarSystem{ get; set; }

        /// <summary>The last station the commander docked at</summary>
        public CLastStarport StarPort { get; set; }

        public CProfile()
        {
        }

        public CProfile(string jsonstring)
        {
            JObject jo = JObject.Parse(jsonstring);
            FromJson(jo);
        }

        public CProfile(JObject json)
        {
            FromJson(json);
        }

        private void FromJson(JObject json)
        {
            if (json["commander"] != null)
            {
                try
                {           // protect bad json

                    Cmdr = new CCommander((JObject)json["commander"]);
                    CurrentStarSystem = new CLastSystem((JObject)json["lastSystem"]);
                    StarPort = new CLastStarport((JObject)json["lastStarport"]);
                    Ship = new CShip((JObject)json["ship"]);

                    Ships = new List<CShip>();

                    JToken st = json["ships"];

                    if (st != null)
                    {
                        if (st is JArray)
                        {
                            foreach( JObject tship in st )
                            {
                                CShip ship = new CShip((JObject)tship);
                                Ships.Add(ship);
                            }
                        }
                        else
                        {
                            JObject jships = (JObject)json["ships"];

                            if (jships != null)
                            {
                                foreach (JToken tship in jships.Values())
                                {
                                    CShip ship = new CShip((JObject)tship);
                                    Ships.Add(ship);
                                }
                            }
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
}
