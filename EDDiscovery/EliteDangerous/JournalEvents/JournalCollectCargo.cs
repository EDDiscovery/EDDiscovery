using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when scooping cargo from space or planet surface
    //Parameters:
    //•	Type: cargo type
    //•	Stolen: whether stolen goods
    public class JournalCollectCargo : JournalEntry
    {
        public JournalCollectCargo(JObject evt ) : base(evt, JournalTypeEnum.CollectCargo)
        {
            Type = JSONHelper.GetStringDef(evt["Type"]);
            Stolen = JSONHelper.GetBool(evt["Stolen"]);
        }
        public string Type { get; set; }
        public bool Stolen { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.collectcargo; } }

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
