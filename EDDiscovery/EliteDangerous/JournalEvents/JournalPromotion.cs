using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{

    public class JournalPromotion : JournalEntry
    {
        //        When written: when the player’s rank increases
        //Parameters: one of the following
        //•	Combat: new rank
        //•	Trade: new rank
        //•	Explore: new rank
        //•	CQC: new rank

        public JournalPromotion(JObject evt) : base(evt, JournalTypeEnum.Promotion)
        {
            Combat = (CombatRank)JSONHelper.GetInt(evt["Combat"]);
            Trade = (TradeRank)JSONHelper.GetInt(evt["Trade"]);
            Explore = (ExplorationRank)JSONHelper.GetInt(evt["Explore"]);
            CQC = (CQCRank)JSONHelper.GetInt(evt["CQC"]);
        }
        public CombatRank Combat { get; set; }
        public TradeRank Trade { get; set; }
        public ExplorationRank Explore { get; set; }
        public CQCRank CQC { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.promotion; } }
    }
}
