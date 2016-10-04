
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: by EDD when a user manually sets an item count (material or commodity)

    public class JournalEDDItemSet : JournalEntry
    {
        public JournalEDDItemSet(JObject evt) : base(evt, JournalTypeEnum.EDDItemSet)
        {
            Materials = evt["Materials"]?.ToObject<MaterialsItems[]>();
            Commodities = evt["Commodities"]?.ToObject<CommoditiesItems[]>();
        }

        public MaterialsItems[] Materials { get; set; }
        public CommoditiesItems[] Commodities { get; set; }

        public class MaterialsItems
        {
            public string Name;
            public int Count;
        }

        public class CommoditiesItems
        {
            public string Name;
            public int Count;
            public int Buyprice;
        }
    }
}
