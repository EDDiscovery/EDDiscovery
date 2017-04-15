using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace EDDiscovery.CompanionAPI
{


    public class CShip
    {
        // Translations from the internal names used by Frontier to clean human-readable
        private static Dictionary<string, string> shipTranslations = new Dictionary<string, string>()
        {
            { "Adder" , "Adder"},
            { "Anaconda", "Anaconda" },
            { "Asp", "Asp Explorer" },
            { "Asp_Scout", "Asp Scout" },
            { "BelugaLiner", "Beluga Liner" },
            { "CobraMkIII", "Cobra Mk. III" },
            { "CobraMkIV", "Cobra Mk. IV" },
            { "Cutter", "Imperial Cutter" },
            { "DiamondBack", "Diamondback Scout" },
            { "DiamondBackXL", "Diamondback Explorer" },
            { "Eagle", "Eagle" },
            { "Empire_Courier", "Imperial Courier" },
            { "Empire_Eagle", "Imperial Eagle" },
            { "Empire_Fighter", "Imperial Fighter" },
            { "Empire_Trader", "Imperial Clipper" },
            { "Federation_Corvette", "Federal Corvette" },
            { "Federation_Dropship", "Federal Dropship" },
            { "Federation_Dropship_MkII", "Federal Assault Ship" },
            { "Federation_Gunship", "Federal Gunship" },
            { "Federation_Fighter", "F63 Condor" },
            { "FerDeLance", "Fer-de-Lance" },
            { "Hauler", "Hauler" },
            { "Independant_Trader", "Keelback" },
            { "Orca", "Orca" },
            { "Python", "Python" },
            { "SideWinder", "Sidewinder" },
            { "Type6", "Type-6 Transporter" },
            { "Type7", "Type-7 Transporter" },
            { "Type9", "Type-9 Heavy" },
            { "Viper", "Viper Mk. III" },
            { "Viper_MkIV", "Viper Mk. IV" },
            { "Vulture", "Vulture" }
        };

        private JObject _jo;
        public int id { get; set; }
        public string name { get; set; }
        public List<CModules> modules;
        public long valueHull { get; set; }
        public long valueModules { get; set; }
        public long valueCargo { get; set; }
        public long valueTotal { get; set; }
        public long valueUnloaned { get; set; }


        public bool free { get; set; }

        public int healthHull { get; set; }
        public int healthShield { get; set; }
        public bool shieldUp { get; set; }
        public int integrity { get; set; }
        public int paintwork { get; set; }


        public bool cockpitBreached { get; set; }
        public int oxygenRemaining { get; set; }
        public float fuelMainCapacity { get; set; }
        public float fuelMainLevel { get; set; }
        public float fuelReserveCapacity { get; set; }
        public float fuelResrveLevel { get; set; }

        public int superchargedFSD { get; set; }
        public int cargoCapacity { get; set; }
        public int cargoQty { get; set; }
        public JArray cargoItems { get; set; }
        //	"lock" : 643120447,   ???????
        //	"ts" : {
        //		"sec" : 1480776109,
        //		"usec" : 823000

        public JObject refinery { get; set; }
        public JArray passenger { get; set; }


        public CShip(JObject jo)
        {
            FromJson(jo);
        }

        public bool FromJson(JObject jo)
        {
            try
            {
                _jo = jo;
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