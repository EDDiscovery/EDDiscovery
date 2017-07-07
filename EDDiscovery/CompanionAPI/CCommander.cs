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
            id = json["id"].Int();
            name =  json["name"].Str();
            credits = json["credits"].Long();
            debt = json["debt"].Long();
            currentShipId = json["currentShipId"].Int();
            alive = json["alive"].Bool();
            docked = json["docked"].Bool();

            combatrank = (CombatRank)json["rank"]["combat"].Int();
            traderank = (TradeRank)json["rank"]["trade"].Int();
            explorationrank = (ExplorationRank)json["rank"]["explore"].Int();
            crimeRank = json["rank"]["crime"].Int();
            serviceRank = json["rank"]["service"].Int();
            empirerank = (EmpireRank)json["rank"]["empire"].Int();
            federationrank = (FederationRank)json["rank"]["federation"].Int();
            powerRank = json["rank"]["power"].Int();
            CQCRank = (CQCRank)json["rank"]["cqc"].Int();

        }
    }
 }