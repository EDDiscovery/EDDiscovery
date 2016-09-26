using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //    When written: If you should ever reset your game
    //    Parameters:
    //•	Name: commander name

    public class JournalClearSavedGame : JournalEntry
    {
        public JournalClearSavedGame(JObject evt) : base(evt, JournalTypeEnum.ClearSavedGame)
        {
            Name = Tools.GetStringDef(evt["Name"]);

        }
        public string Name { get; set; }

    }
}
