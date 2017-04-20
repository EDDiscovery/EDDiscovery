using EDDiscovery.EliteDangerous;
using Newtonsoft.Json.Linq;

namespace EDDiscovery.CompanionAPI
{
    public class CCommander
    {
        public int id { get; private set; }
        public string name { get; private set; }
        public long credits { get; private set; }
        public long debt { get; private set; }
        public int currentShipId { get; private set; }
        public bool alive { get; private set; }
        public bool docked { get; private set; }

        public CombatRank combatrank { get; private set; }
        public TradeRank traderank { get; private set; }
        public ExplorationRank explorationrank { get; private set; }
        public int crimeRank { get; private set; }  // ?
        public int serviceRank { get; private set; } //?
        public EmpireRank empirerank { get; private set; }
        public FederationRank federationrank { get; private set; }
        public int powerRank{ get; private set; }
        public CQCRank CQCRank { get; private set; }

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