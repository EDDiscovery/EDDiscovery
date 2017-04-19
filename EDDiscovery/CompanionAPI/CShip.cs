using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EDDiscovery.CompanionAPI
{
    public class CShip
    {
        public int id { get; private set; }
        public string name { get; private set; }
        public List<CModules> modules;
        public long valueHull { get; private set; }
        public long valueModules { get; private set; }
        public long valueCargo { get; private set; }
        public long valueTotal { get; private set; }
        public long valueUnloaned { get; private set; }


        public bool free { get; private set; }

        public int healthHull { get; private set; }
        public int healthShield { get; private set; }
        public bool shieldUp { get; private set; }
        public int integrity { get; private set; }
        public int paintwork { get; private set; }


        public bool cockpitBreached { get; private set; }
        public int oxygenRemaining { get; private set; }
        public float fuelMainCapacity { get; private set; }
        public float fuelMainLevel { get; private set; }
        public float fuelReserveCapacity { get; private set; }
        public float fuelResrveLevel { get; private set; }

        public int superchargedFSD { get; private set; }
        public int cargoCapacity { get; private set; }
        public int cargoQty { get; private set; }
        public JArray cargoItems { get; private set; }
        //	"lock" : 643120447,   ???????
        //	"ts" : {
        //		"sec" : 1480776109,
        //		"usec" : 823000

        public JObject refinery { get; private set; }
        public JArray passenger { get; private set; }


        public CShip(JObject jo)
        {
            FromJson(jo);
        }

        public bool FromJson(JObject jo)
        {
            try
            {
                id = JSONHelper.GetInt(jo["id"]);
                name = JSONHelper.GetStringDef(jo["name"]);

                valueHull = JSONHelper.GetInt(jo["value"]["hull"]);
                valueModules = JSONHelper.GetInt(jo["value"]["modules"]);
                valueCargo = JSONHelper.GetInt(jo["value"]["cargo"]);
                valueTotal = JSONHelper.GetInt(jo["value"]["total"]);
                valueUnloaned = JSONHelper.GetInt(jo["value"]["unloaned"]);

                free = JSONHelper.GetBool(jo["free"]);

                if (jo["health"] != null)
                {
                    healthHull = JSONHelper.GetInt(jo["health"]["hull"]);
                    healthShield = JSONHelper.GetInt(jo["health"]["shield"]);
                    shieldUp = JSONHelper.GetBool(jo["health"]["shieldup"]);
                    integrity = JSONHelper.GetInt(jo["health"]["integrity"]);
                    paintwork = JSONHelper.GetInt(jo["health"]["paintwork"]);
                }
                cockpitBreached = JSONHelper.GetBool(jo["cockpitBreached"]);
                oxygenRemaining = JSONHelper.GetInt(jo["oxygenRemaining"]);



                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}