using EDDiscovery.EliteDangerous;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EDDiscovery.CompanionAPI
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

        public CProfile(JObject json)
        {
            if (json["commander"] != null)
            {
                Cmdr = new CCommander((JObject)json["commander"]);
                CurrentStarSystem = new CLastSystem((JObject)json["lastSystem"]);
                StarPort = new CLastStarport((JObject)json["lastStarport"]);
                Ship = new CShip((JObject)json["ship"]);

                JObject jships = (JObject)json["ships"];
                Ships = new List<CShip>();

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
}
