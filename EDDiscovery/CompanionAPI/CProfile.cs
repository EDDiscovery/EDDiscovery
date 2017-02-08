using EDDiscovery.EliteDangerous;
using EDDiscovery2.DB;
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
        public ISystem CurrentStarSystem{ get; set; }

        /// <summary>The last station the commander docked at</summary>
        //public Station LastStation { get; set; }

        public CProfile()
        {
            Ships = new List<CShip>();
        }

        public CProfile(JObject json)
        {
            if (json["commander"] != null)
            {
                Cmdr = new CCommander((JObject)json["commander"]);
            }


         
        }

    }
}
