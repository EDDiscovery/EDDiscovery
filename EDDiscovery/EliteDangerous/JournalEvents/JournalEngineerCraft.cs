using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when requesting an engineer upgrade
    //Parameters:
    //•	Engineer: name of engineer
    //•	Blueprint: name of blueprint
    //•	Level: crafting level
    //•	Ingredients: JSON object with names and quantities of materials required
    public class JournalEngineerCraft : JournalEntry
    {
        public JournalEngineerCraft(JObject evt ) : base(evt, JournalTypeEnum.EngineerCraft)
        {
            Engineer = JSONHelper.GetStringDef(evt["Engineer"]);
            Blueprint = JSONHelper.GetStringDef(evt["Blueprint"]);
            Level = JSONHelper.GetInt(evt["Level"]);
            Ingredients = evt["Ingredients"]?.ToObject<Dictionary<string, int>>();
        }

        public string Engineer { get; set; }
        public string Blueprint { get; set; }
        public int Level { get; set; }
        public Dictionary<string,int> Ingredients { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.engineercraft; } }

        public void MaterialList(EDDiscovery2.DB.MaterialCommoditiesList mc, DB.SQLiteConnectionUser conn)
        {
            if (Ingredients != null)
            {
                foreach (KeyValuePair<string, int> k in Ingredients)        // may be commodities or materials
                    mc.Craft(k.Key, k.Value);
            }
        }
    }
}
