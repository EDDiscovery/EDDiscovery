using EDDiscovery.EliteDangerous;
using Newtonsoft.Json.Linq;

namespace EDDiscovery.CompanionAPI
{
    public class CCommander
    {
        public int id { get; set; }
        public string name { get; set; }
        public long credits { get; set; }
        public long debt { get; set; }
        public int currentShipId { get; set; }
        public bool alive { get; set; }
        public bool docked { get; set; }

        public CombatRank combatrank { get; set; }
        public TradeRank traderank { get; set; }
        public ExplorationRank explorationrank { get; set; }
        public int crimeRank { get; set; }  // ?
        public int serviceRank { get; set; } //?
        public EmpireRank empirerank { get; set; }
        public FederationRank federationrank { get; set; }
        public int powerRank{ get; set; }
        public CQCRank CQCRank { get; set; }

        public CCommander(JObject json)
        {
            id = JSONHelper.GetInt(json["id"]);
            name =  JSONHelper.GetStringDef(json["name"]);
            credits = JSONHelper.GetLong(json["credits"]);
            debt = JSONHelper.GetLong(json["debt"]);
            currentShipId = JSONHelper.GetInt(json["currentShipId"]);
            alive = JSONHelper.GetBool(json["alive"]);
            docked = JSONHelper.GetBool(json["docked"]);

            combatrank = (CombatRank)JSONHelper.GetInt(json["rank"]["combat"]);
            traderank = (TradeRank)JSONHelper.GetInt(json["rank"]["trade"]);
            explorationrank = (ExplorationRank)JSONHelper.GetInt(json["rank"]["explore"]);
            crimeRank = JSONHelper.GetInt(json["rank"]["crime"]);
            serviceRank = JSONHelper.GetInt(json["rank"]["service"]);
            empirerank = (EmpireRank)JSONHelper.GetInt(json["rank"]["empire"]);
            federationrank = (FederationRank)JSONHelper.GetInt(json["rank"]["federation"]);
            powerRank = JSONHelper.GetInt(json["rank"]["power"]);
            CQCRank = (CQCRank)JSONHelper.GetInt(json["rank"]["cqc"]);

        }
    }
 }