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
