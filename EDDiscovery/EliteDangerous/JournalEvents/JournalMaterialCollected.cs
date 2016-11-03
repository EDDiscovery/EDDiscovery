using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: whenever materials are collected 
    //Parameters: 
    //•	Category: type of material (Raw/Encoded/Manufactured)
    //•	Name: name of material
    public class JournalMaterialCollected : JournalEntry
    {
        public JournalMaterialCollected(JObject evt ) : base(evt, JournalTypeEnum.MaterialCollected)
        {
            Category = JSONHelper.GetStringDef(evt["Category"]);
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Count = JSONHelper.GetInt(evt["Count"], 1);
        }
        public string Category { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.materialcollected; } }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            mc.Change(Category, Name, Count, 0, conn);
        }
        
    }
}
