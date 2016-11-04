
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: by EDD when a user manually sets an item count (material or commodity)

    public class JournalEDDItemSet : JournalEntry
    {
        public JournalEDDItemSet(JObject evt) : base(evt, JournalTypeEnum.EDDItemSet)
        {
            Materials = new MaterialList(evt["Materials"]?.ToObject<MaterialItem[]>().ToList());
            Commodities = new CommodityList(evt["Commodities"]?.ToObject<CommodityItem[]>().ToList());
        }

        public JournalEDDItemSet() : base( JournalTypeEnum.EDDItemSet)
        {
            Materials = new MaterialList();
            Commodities = new CommodityList();
        }

        public MaterialList Materials { get; set; }
        public CommodityList Commodities { get; set; }

        public string UpdateState()                      // calculates the JSON string and returns it, plus updates the class so as it would look when loaded
        {
            JObject evt = new JObject();
            evt["timestamp"] = EventTimeUTC;
            evt["event"] = EventTypeStr;

            if (Materials != null)
            {
                JArray ja = new JArray();

                foreach (MaterialItem i in Materials.Materials)
                {
                    JObject jo = new JObject();
                    jo["Name"] = i.Name;
                    jo["Category"] = i.Category;
                    jo["Count"] = i.Count;
                    ja.Add(jo);
                }

                evt["Materials"] = ja;
            }

            if (Commodities != null)
            {
                JArray ja = new JArray();

                foreach (CommodityItem c in Commodities.Commodities)
                {
                    JObject jo = new JObject();
                    jo["Name"] = c.Name;
                    jo["Count"] = c.Count;
                    jo["BuyPrice"] = c.BuyPrice;
                    ja.Add(jo);
                }

                evt["Commodities"] = ja;
            }

            jEventData = evt;
            return evt.ToString();
        }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Materials != null)
            {
                foreach (MaterialItem m in Materials.Materials)
                    mc.Set(m.Category, m.Name, m.Count, 0, conn);
            }

            if (Commodities != null)
            {
                foreach (CommodityItem m in Commodities.Commodities)
                    mc.Set(EDDiscovery2.DB.MaterialCommodities.CommodityCategory, m.Name, m.Count, m.BuyPrice, conn);
            }
        }

    }

    public class MaterialItem
    {
        public string Name;
        public string Category;
        public int Count;
    }

    public class CommodityItem
    {
        public string Name;
        public int Count;
        public double BuyPrice;
    }

    public class MaterialList
    {
        public MaterialList()
        {
            Materials = new System.Collections.Generic.List<MaterialItem>();
        }

        public MaterialList(System.Collections.Generic.List<MaterialItem> ma )
        {
            Materials = ma;
        }

        public System.Collections.Generic.List<MaterialItem> Materials { get; protected set; }

        public void Set(string cat, string name, int count)
        {
            if (Materials == null)
                Materials = new System.Collections.Generic.List<MaterialItem>();

            int i = Materials.FindIndex(x => x.Name.Equals(name));

            if (i == -1)
                Materials.Add(new MaterialItem { Category=cat, Name = name, Count = count });
            else
                Materials[i].Count = count;
        }

    }

    public class CommodityList
    {
        public CommodityList()
        {
            Commodities = new System.Collections.Generic.List<CommodityItem>();
        }

        public CommodityList(System.Collections.Generic.List<CommodityItem> ma)
        {
            Commodities = ma;
        }

        public System.Collections.Generic.List<CommodityItem> Commodities { get; protected set; }

        public void Set(string name, int count, double buyprice)
        {
            if (Commodities == null)
                Commodities = new System.Collections.Generic.List<CommodityItem>();

            int i = Commodities.FindIndex(x => x.Name.Equals(name));

            if (i == -1)
                Commodities.Add(new CommodityItem { Name = name, Count = count, BuyPrice = buyprice });
            else
            {
                Commodities[i].Count = count;
                Commodities[i].BuyPrice = buyprice;
            }
        }
    }

}


