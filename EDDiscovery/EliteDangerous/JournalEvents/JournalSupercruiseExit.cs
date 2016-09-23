using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: leaving supercruise for normal space
    //Parameters:
    //•	Starsystem
    //•	Body
    public class JournalSupercruiseExit : JournalEntry
    {
        public JournalSupercruiseExit(JObject evt ) : base(evt, JournalTypeEnum.SupercruiseExit)
        {
            StarSystem = Tools.GetStringDef(evt["StarSystem"]);
            Body = Tools.GetStringDef(evt["Body"]);

        }
        public string StarSystem { get; set; }
        public string Body { get; set; }

    }
}
