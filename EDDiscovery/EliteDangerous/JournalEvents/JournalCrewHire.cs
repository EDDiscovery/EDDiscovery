using Newtonsoft.Json.Linq;
using System.Linq;

namespace EDDiscovery.EliteDangerous.JournalEvents
{
    //When Written: when engaging a new member of crew
    //Parameters:
    //•	Name
    //•	Faction
    //•	Cost
    //•	CombatRank

    public class JournalCrewHire : JournalEntry
    {
        public JournalCrewHire(JObject evt) : base(evt, JournalTypeEnum.CrewHire)
        {
            Name = JSONHelper.GetStringDef(evt["Name"]);
            Faction = JSONHelper.GetStringDef(evt["Faction"]);
            Cost = JSONHelper.GetLong(evt["Cost"]);
            CombatRank = (CombatRank)JSONHelper.GetInt(evt["CombatRank"]);
        }
        public string Name { get; set; }
        public string Faction { get; set; }
        public long Cost { get; set; }
        public CombatRank CombatRank { get; set; }

        public static System.Drawing.Bitmap Icon { get { return EDDiscovery.Properties.Resources.crew; } }

        public void Ledger(EDDiscovery2.DB.MaterialCommoditiesLedger mcl, DB.SQLiteConnectionUser conn)
        {
            mcl.AddEvent(Id, EventTimeUTC, EventTypeID, Name + " " + Faction, -Cost);
        }

    }
}
