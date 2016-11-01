using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when a mission has failed
    //Parameters:
    //•	Name: name of mission
    public class JournalMissionFailed : JournalEntry
    {
        public JournalMissionFailed(JObject evt ) : base(evt, JournalTypeEnum.MissionFailed)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            MissionId = JSONHelper.GetInt(evt["MissionID"]);
        }

        public string Name { get; set; }
        public int MissionId { get; set; }

        public override string DefaultRemoveItems()
        {
            return base.DefaultRemoveItems() + ";MissionID";
        }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.missionfailed; } }

    }
}
