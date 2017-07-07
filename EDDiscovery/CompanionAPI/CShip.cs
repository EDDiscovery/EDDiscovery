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
                id = jo["id"].Int();
                name = jo["name"].Str();

                valueHull = jo["value"]["hull"].Int();
                valueModules = jo["value"]["modules"].Int();
                valueCargo = jo["value"]["cargo"].Int();
                valueTotal = jo["value"]["total"].Int();
                valueUnloaned = jo["value"]["unloaned"].Int();

                free = jo["free"].Bool();

                if (jo["health"] != null)
                {
                    healthHull = jo["health"]["hull"].Int();
                    healthShield = jo["health"]["shield"].Int();
                    shieldUp = jo["health"]["shieldup"].Bool();
                    integrity = jo["health"]["integrity"].Int();
                    paintwork = jo["health"]["paintwork"].Int();
                }
                cockpitBreached = jo["cockpitBreached"].Bool();
                oxygenRemaining = jo["oxygenRemaining"].Int();



                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}