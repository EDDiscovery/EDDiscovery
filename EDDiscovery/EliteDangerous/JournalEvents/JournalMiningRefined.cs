using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when mining fragments are converted unto a unit of cargo by refinery
    //Parameters:
    //•	Type: cargo type
    public class JournalMiningRefined : JournalEntry
    {
        public JournalMiningRefined(JObject evt ) : base(evt, JournalTypeEnum.MiningRefined)
        {
            Type = JSONHelper.GetStringDef(evt["Type"]);

        }
        public string Type { get; set; }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(EDDiscovery2.DB.MaterialCommodities.CommodityCategory, Type, 1, 0, conn);
        }

        public void LedgerNC(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            EDDiscovery2.DB.MaterialCommodities mc = mcl.GetMaterialCommodity(EDDiscovery2.DB.MaterialCommodities.CommodityCategory, Type, conn);
            mcl.AddEventNoCash(Id, EventTimeUTC, EventTypeID, mc.name );
        }

    }
}
