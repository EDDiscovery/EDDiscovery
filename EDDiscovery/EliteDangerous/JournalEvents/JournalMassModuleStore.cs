using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
//    When written: when putting multiple modules into storage
//Parameters:
//•	Ship
//•	ShipId
//•	Items: Array of records
//o   Slot
//o   Name
//o   EngineerModifications(only present if modified)


    public class JournalMassModuleStore : JournalEntry
    {
        public JournalMassModuleStore(JObject evt) : base(evt, JournalTypeEnum.MassModuleStore)
        {
            Ship = JSONHelper.GetStringDef(evt["Ship"]);
            ShipId = JSONHelper.GetInt(evt["ShipID"]);
            ModuleItems = evt["Items"]?.ToObject<ModuleItem[]>();
        }
        public string Ship { get; set; }
        public int ShipId { get; set; }

        public ModuleItem[] ModuleItems { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";ShipID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.modulestore; } }

    }


    public class ModuleItem
    {
        public string Slot;
        public string Name;
        public double EngineerModifications;
    }

}
