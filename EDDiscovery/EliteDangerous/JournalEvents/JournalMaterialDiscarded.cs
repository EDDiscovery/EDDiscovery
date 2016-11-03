
using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{

    public class JournalMaterialDiscarded : JournalEntry
    {
        public JournalMaterialDiscarded(JObject evt ) : base(evt, JournalTypeEnum.MaterialDiscarded)
        {
            Category = JSONHelper.GetStringDef(evt["Category"]);
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Count = JSONHelper.GetInt(evt["Count"]);

        }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.materialdiscarded; } }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(Category, Name, -Count, 0, conn);
        }

    }
}
