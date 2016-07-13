using EDDiscovery;
using EDDiscovery2.EDDB;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDDiscovery2.DB
{
    public enum SystemInfoSource
    {
        RW = 1,
        EDSC = 2,
        EDDB = 4,
        EDSM = 5
    }

    public class StationClass
    {
        public int id, eddb_id;
        public string name, SearchName;
        public int system_id;
        public string max_landing_pad_size;
        public int distance_to_star;
        public string faction;
        public EDGovernment government;
        public EDAllegiance allegiance;

        public EDStationType stationtype;
        public EDState state;


	    public int has_blackmarket;   // 1 = has,  0 = dont have,   -1 = unknown
		public int has_commodities;
		public int has_refuel;
		public int has_repair;
		public int has_rearm;
		public int has_outfitting;
        public int has_shipyard;

        public List<Commodity>  import_commodities;
        public List<Commodity> export_commodities;
        public List<Commodity> prohibited_commodities;

        public List<EDEconomy> economies;
        public int eddb_updated_at;

        public StationClass(JObject jo, SystemInfoSource source)
        {
            if (source == SystemInfoSource.EDDB)
            {
                name = jo["name"].Value<string>();
                SearchName = name.ToLower();

                eddb_id = jo["id"].Value<int>();
                system_id = jo["system_id"].Value<int>();

                if (jo["max_landing_pad_size"].Type == JTokenType.String) 
                    max_landing_pad_size = jo["max_landing_pad_size"].Value<string>();

                if (jo["distance_to_star"].Type == JTokenType.Integer)
                    distance_to_star = jo["distance_to_star"].Value<int>();

                faction = jo["faction"].Value<string>();

                government = EliteDangerous.Government2ID(jo["government"]);
                allegiance = EliteDangerous.Allegiance2ID(jo["allegiance"]);

                state = EliteDangerous.EDState2ID(jo["state"]);

                stationtype = EliteDangerous.EDStationType2ID(jo["type"]);

                if (jo["has_blackmarket"].Type == JTokenType.Integer)
                    has_blackmarket = jo["has_blackmarket"].Value<int>();
                else
                    has_blackmarket = -1;

                if (jo["has_commodities"].Type == JTokenType.Integer)
                    has_commodities = jo["has_commodities"].Value<int>();
                else
                    has_commodities = -1;

                if (jo["has_refuel"].Type == JTokenType.Integer)
                    has_refuel = jo["has_refuel"].Value<int>();
                else
                    has_refuel = -1;

                if (jo["has_repair"].Type == JTokenType.Integer)
                    has_repair = jo["has_repair"].Value<int>();
                else
                    has_repair = -1;

                if (jo["has_rearm"].Type == JTokenType.Integer)
                    has_rearm = jo["has_rearm"].Value<int>();
                else
                    has_rearm = -1;

                if (jo["has_outfitting"].Type == JTokenType.Integer)
                    has_outfitting = jo["has_outfitting"].Value<int>();
                else
                    has_outfitting = -1;

                if (jo["has_shipyard"].Type == JTokenType.Integer)
                    has_shipyard = jo["has_shipyard"].Value<int>();
                else
                    has_shipyard = -1;

                economies = EliteDangerous.EDEconomies2ID((JArray)jo["economies"]);
                import_commodities = EliteDangerous.EDCommodities2ID((JArray)jo["import_commodities"]);
                export_commodities = EliteDangerous.EDCommodities2ID((JArray)jo["export_commodities"]);
                prohibited_commodities = EliteDangerous.EDCommodities2ID((JArray)jo["prohibited_commodities"]);
                eddb_updated_at = jo["updated_at"].Value<int>();

            }
        }

    }
}
