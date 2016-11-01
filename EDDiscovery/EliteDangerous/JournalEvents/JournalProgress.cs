using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When written: at startup
    //Parameters:
    //•	Combat: percent progress to next rank
    //•	Trade: 		“
    //•	Explore: 	“
    //•	Empire: 	“
    //•	Federation: 	“
    //•	CQC: 		“ ranks: 0=’Helpless’, 1=’Mostly Helpless’, 2=’Amateur’, 3=’Semi Professional’, 4=’Professional’, 5=’Champion’, 6=’Hero’, 7=’Legend’, 8=’Elite’

    public class JournalProgress : JournalEntry
    {

        public JournalProgress(JObject evt ) : base(evt, JournalTypeEnum.Progress)
        {
            Combat = JSONHelper.GetInt(evt["Combat"]);
            Trade = JSONHelper.GetInt(evt["Trade"]);
            Explore = JSONHelper.GetInt(evt["Explore"]);
            Empire = JSONHelper.GetInt(evt["Empire"]);
            Federation = JSONHelper.GetInt(evt["Federation"]);
            CQC = JSONHelper.GetInt(evt["CQC"]);

        }
        public int Combat { get; set; }
        public int Trade { get; set; }
        public int Explore { get; set; }
        public int Empire { get; set; }
        public int Federation { get; set; }
        public int CQC { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.progress; } }

    }
}
