using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: this player has left a wing
    //Parameters: none
    public class JournalWingLeave : JournalEntry
    {
        public JournalWingLeave(JObject evt ) : base(evt, JournalTypeEnum.WingLeave)
        {

        }


    }
}
